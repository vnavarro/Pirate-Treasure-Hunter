using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace PirateTreasure
{
    class PlayerSprite : Sprite
    {
        #region Attributes,Properties,Contants & Etc
        public enum State
        {
            StillLeft,
            StillRight,
            MovingLeft,
            MovingRight
        }

        const float SPEED = 300f;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        private float timePerFrame;
        private float framesPerSec = 10.0f;
        private float totalElapsed = 0;

        //public int actualCharAnimation = 0;

        private string[] assets = new string[] { "Sprites/PirateStillLeft", "Sprites/PirateStillRight", "Sprites/PirateLeft", "Sprites/PirateRight" };
        private int START_POSITION_X = 0;
        private int START_POSITION_Y = 0;

        private ContentManager content;

        Vector2 direction = Vector2.Zero;
        Vector2 speedComposition = Vector2.Zero;
        KeyboardState previousKeyboardState;

        public State currentState = State.StillLeft;

        private int life = 3;
        public int Life
        {
            get { return life; }
            protected set { life = value; }
        }

        private SoundEffect extraLifeEffect;

        #endregion

        public void LoadContent(ContentManager gameContent)
        {
            this.content = gameContent;
            base.LoadContent(gameContent, assets[(int)currentState]);
            START_POSITION_Y = Settings.screenHeigth - Size.Height - 90;
            START_POSITION_X = Settings.screenWidth / 2;
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            this.BoundingXPadding = 18;
            this.BoundingYPadding = 2;
            timePerFrame = 1.0f / framesPerSec;
            extraLifeEffect= gameContent.Load<SoundEffect>("Sounds/Effects/extralife");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.LoadContent(content, assets[(int)currentState]);
            base.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            totalElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (totalElapsed > timePerFrame)
            {
                totalElapsed -= timePerFrame;
                KeyboardState currentKeyboardState = Keyboard.GetState();
                UpdateMovement(currentKeyboardState);

                previousKeyboardState = currentKeyboardState;
                float futurePosition = Position.X +
                                       (direction.X * speedComposition.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (futurePosition > 0 && futurePosition + Size.Width < Settings.screenWidth)
                    base.Update(gameTime, speedComposition, direction);
            }
        }
        private void UpdateMovement(KeyboardState currentKeyboardState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Left) == true)
                MoveLeft();
            else if (currentKeyboardState.IsKeyDown(Keys.Right) == true)
                MoveRight();
            else
                StayStill();
        }

        private void StayStill()
        {
            //if (direction.X == MOVE_RIGHT)
            if (previousKeyboardState.IsKeyDown(Keys.Right))
                currentState = State.StillRight;
            //else if (direction.Y == MOVE_LEFT)
            else if (previousKeyboardState.IsKeyDown(Keys.Left))
                currentState = State.StillLeft;

            speedComposition = Vector2.Zero;
            direction = Vector2.Zero;
        }

        private void MoveRight()
        {
            speedComposition.X = SPEED;
            direction.X = MOVE_RIGHT;
            if (currentState != State.StillRight && currentState != State.MovingRight)
                currentState = State.StillRight;
            ChangeAnimation(currentState);
            //currentState = State.MovingRight;
        }

        private void MoveLeft()
        {
            speedComposition.X = SPEED;
            direction.X = MOVE_LEFT;
            if (currentState != State.StillLeft && currentState != State.MovingLeft)
                currentState = State.StillLeft;
            ChangeAnimation(currentState);
            //currentState = State.MovingLeft;
        }

        public void ResetPlayer(PlayerLife myLife)
        {
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            life = 3;
            myLife.Quantity = life;
        }

        private void RemoveLife()
        {
            life--;
        }

        public void LifeLost(PlayerLife myLife)
        {
            if (IsColliding)
            {
                RemoveLife();
                myLife.Quantity = life;
            }
        }
        public void LifeGained(PlayerLife myLife)
        {
            Game1.songPlayer.Play(extraLifeEffect);
            if (life < 6)
            {
                life++;
                myLife.Quantity = life;
            }
        }


        private void ChangeAnimation(State actualState)
        {            
            switch (actualState)
            {
                case State.MovingLeft:
                    currentState = State.StillLeft;
                    break;
                case State.MovingRight:
                    currentState = State.StillRight;
                    break;
                case State.StillLeft:
                    currentState = State.MovingLeft;
                    break;
                case State.StillRight:
                    currentState = State.MovingRight;
                    break;
            }
        }
    }
}
