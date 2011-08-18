using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PirateTreasure
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GameStates
        {
            TitleScreen,
            Playing,
            GameOver,
            Credits,
            Pause
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private BackgroundSprite backgroundTitleScreen;
        private BackgroundSprite backgroundFase1;
        private Bombs pirateBombs;        
        private PlayerSprite pirateSprite;
        private PlayerLife pirateLife;
        private CollisionControl collisionController;
        private GameStates currentGameState = GameStates.TitleScreen;
        private Coins treasureCoins;
        private int treasureScore = 0;
        private TreasureChests treasureChests;
        private ExtraLife extraLife;
        private Song titleSong;
        private Song playingSong;
        public static SongPlayer songPlayer;
        private int selectedIndex = 0;
        Color startColor = Color.Black;
        Color creditsColor = Color.Black;
        KeyboardState previousKeyboardState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = Settings.screenHeigth;
            graphics.PreferredBackBufferWidth = Settings.screenWidth;
            Content.RootDirectory = "Content";
            backgroundTitleScreen = new BackgroundSprite("Sprites/TitleScreen");
            backgroundFase1 = new BackgroundSprite("Sprites/Background_Fase1");
            pirateSprite = new PlayerSprite();
            pirateLife = new PlayerLife();
            pirateBombs = new Bombs();
            collisionController = new CollisionControl();
            treasureCoins = new Coins();
            treasureChests = new TreasureChests();
            extraLife = new ExtraLife();
            songPlayer = new SongPlayer();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            backgroundFase1.LoadContent(Content);
            pirateSprite.LoadContent(Content);
            pirateLife.LoadContent(Content);
            pirateBombs.LoadContent(Content);
            treasureCoins.LoadContent(Content);
            backgroundTitleScreen.LoadContent(Content);
            treasureChests.LoadContent(Content);
            extraLife.LoadContent(Content);
            titleSong = Content.Load<Song>(@"Sounds/title");
            playingSong = Content.Load<Song>(@"Sounds/Tropical paradise");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            switch (currentGameState)
            {
                case GameStates.TitleScreen:
                    UpdateTitleState(gameTime);
                    break;
                case GameStates.Playing:
                    UpdatePlayingState(gameTime);
                    break;
                case GameStates.GameOver:
                    UpdateGameOverState(gameTime);
                    break;
                case GameStates.Credits:
                    UpdateCreditsState(gameTime);
                    break;
                case GameStates.Pause:
                    UpdatePauseState();
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdatePauseState()
        {
            KeyboardState currentState = Keyboard.GetState();
            if (currentState.IsKeyDown(Keys.Escape))
            {
                currentGameState = GameStates.Playing;
            }
        }

        private void UpdateCreditsState(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.Back) ||
                currentKeyboardState.IsKeyDown(Keys.Escape))
                currentGameState = GameStates.TitleScreen;
        }

        private void UpdateTitleState(GameTime gameTime)
        {
            ResetGame();

            KeyboardState currentKeyboardState = Keyboard.GetState();
            songPlayer.Play(titleSong);
            if (currentKeyboardState.IsKeyDown(Keys.Space) == true || currentKeyboardState.IsKeyDown(Keys.Enter))
            {
                if (selectedIndex == 0)
                {
                    currentGameState = GameStates.Playing;
                    songPlayer.Stop();
                }
                else if (selectedIndex == 1)
                {
                    currentGameState = GameStates.Credits;
                }
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Down) == true && !previousKeyboardState.IsKeyDown(Keys.Down))
            {
                selectedIndex++;
                selectedIndex = selectedIndex % 2;
                previousKeyboardState = currentKeyboardState;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Up) == true && !previousKeyboardState.IsKeyDown(Keys.Up))
            {
                selectedIndex--;
                selectedIndex = selectedIndex % 2;
                previousKeyboardState = currentKeyboardState;
            }

            if (selectedIndex == 0)
            {
                startColor = Color.DarkOliveGreen;
                creditsColor = Color.Black;
            }
            else if (selectedIndex == 1)
            {
                creditsColor = Color.DarkOliveGreen;
                startColor = Color.Black;
            }
        }

        private void UpdatePlayingState(GameTime gameTime)
        {
            KeyboardState currentKeyState = Keyboard.GetState();
            if (currentKeyState.IsKeyDown(Keys.P))            
                currentGameState = GameStates.Pause;
            else if (currentKeyState.IsKeyDown(Keys.Back))
            {
                currentGameState = GameStates.TitleScreen;
                songPlayer.Stop();
            }
            else
            {
                songPlayer.Play(playingSong);
                pirateSprite.Update(gameTime);
                pirateBombs.Update(gameTime);
                treasureCoins.Update(gameTime);
                treasureChests.Update(gameTime);
                extraLife.Update(gameTime);

                collisionController.SearchSpriteCollision(pirateBombs.bombs, pirateSprite);
                if (pirateSprite.IsColliding)
                {
                    pirateBombs.PlayEffects();
                    pirateSprite.LifeLost(pirateLife);
                    pirateBombs.ResetCollidingBombs();
                    pirateSprite.IsColliding = false;
                }

                collisionController.SearchSpriteCollision(treasureCoins.coins, pirateSprite);
                foreach (FallingObjectsSprite coin in treasureCoins.coins)
                {
                    if (coin.IsColliding)
                    {
                        UpdateTreasure(treasureCoins.coinValue);
                        treasureCoins.PlayEffects();
                    }
                }
                treasureCoins.ResetCollidingCoin();

                collisionController.SearchSpriteCollision(treasureChests.chests, pirateSprite);
                foreach (FallingObjectsSprite chest in treasureChests.chests)
                {
                    if (chest.IsColliding)
                    {
                        UpdateTreasure(treasureChests.treasureValue);
                        treasureChests.PlayEffects();
                    }
                }
                treasureChests.ResetCollidingChest();

                collisionController.SearchSpriteCollision(extraLife, pirateSprite);
                if (extraLife.IsColliding)
                {
                    pirateSprite.LifeGained(pirateLife);
                }
                extraLife.CollisionReset();

                if (pirateSprite.Life <= 0)
                {
                    currentGameState = GameStates.GameOver;
                    songPlayer.Stop();
                }
            }
        }

        private void UpdateGameOverState(GameTime gameTime)
        {
            ResetGame();
            KeyboardState currentKeyboardState = Keyboard.GetState();
            if (currentKeyboardState.IsKeyDown(Keys.Space) || currentKeyboardState.IsKeyDown(Keys.Enter))
                currentGameState = GameStates.Playing;
            else if (currentKeyboardState.IsKeyDown(Keys.Back) == true)
                currentGameState = GameStates.TitleScreen;
        }

        private void ResetGame()
        {
            extraLife.Reset();
            pirateSprite.ResetPlayer(pirateLife);
            pirateBombs.ResetAll();
            treasureCoins.ResetAll();
            treasureChests.ResetAll();
            ResetTreasure();            
        }


        private void UpdateTreasure(int treasureValue)
        {
            treasureScore += treasureValue;
        }

        private void ResetTreasure()
        {
            treasureScore = 0;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            switch (currentGameState)
            {
                case GameStates.TitleScreen:
                    DrawTitleScreenState(gameTime);
                    break;
                case GameStates.Playing:
                    DrawPlayingState(gameTime);
                    break;
                case GameStates.GameOver:
                    DrawGameOverState(gameTime);
                    break;
                case GameStates.Credits:
                    DrawCreditsState();
                    break;
                case GameStates.Pause:
                    DrawPlayingState(gameTime);
                    DrawPauseState();
                    break;
            }
            base.Draw(gameTime);
        }

        private void DrawPauseState()
        {
            spriteBatch.Begin();            
            backgroundFase1.Draw(spriteBatch);
            treasureChests.Draw(spriteBatch);
            treasureCoins.Draw(spriteBatch);
            pirateBombs.Draw(spriteBatch);
            extraLife.Draw(spriteBatch);
            pirateSprite.Draw(spriteBatch);
            pirateLife.Draw(spriteBatch);
            SpriteFont font = Content.Load<SpriteFont>("Fonts/InGameFont");
            Vector2 fontPosition = new Vector2(5, -5);
            spriteBatch.DrawString(font, treasureScore.ToString(), fontPosition, Color.Black);
            font = Content.Load<SpriteFont>("Fonts/PauseFont");
            fontPosition = new Vector2(10, 120);
            spriteBatch.DrawString(font, "Pause (press esc to continue)", fontPosition, Color.Black, 0.0f, Vector2.Zero,
                1.0f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        private void DrawTitleScreenState(GameTime gameTime)
        {
            spriteBatch.Begin();
            backgroundTitleScreen.Draw(spriteBatch);
            SpriteFont font = Content.Load<SpriteFont>("Fonts/TitleFont");
            Vector2 fontPosition = new Vector2(80, Settings.screenHeigth - 150);

            spriteBatch.DrawString(font, "Start", fontPosition, startColor, 0.0f, Vector2.Zero,
                0.7f, SpriteEffects.None, 0);
            fontPosition.Y += 25;
            spriteBatch.DrawString(font, "Credits", fontPosition, creditsColor, 0.0f, Vector2.Zero,
                0.7f, SpriteEffects.None, 0);
            fontPosition.Y = 10;
            spriteBatch.DrawString(font, "Pirate Treasure Hunter", fontPosition, Color.Black);
            spriteBatch.End();
        }

        private void DrawPlayingState(GameTime gameTime)
        {
            spriteBatch.Begin();
            backgroundFase1.Draw(spriteBatch);
            treasureChests.Draw(spriteBatch);
            treasureCoins.Draw(spriteBatch);
            pirateBombs.Draw(spriteBatch);
            extraLife.Draw(spriteBatch);
            pirateSprite.Draw(spriteBatch);
            pirateLife.Draw(spriteBatch);
            SpriteFont font = Content.Load<SpriteFont>("Fonts/InGameFont");
            Vector2 fontPosition = new Vector2(5, -5);
            spriteBatch.DrawString(font, treasureScore.ToString(), fontPosition, Color.Black);
            spriteBatch.End();
        }

        private void DrawGameOverState(GameTime gameTime)
        {
            spriteBatch.Begin();
            backgroundTitleScreen.Draw(spriteBatch);

            SpriteFont font = Content.Load<SpriteFont>("Fonts/GameOverFont");
            Vector2 fontPosition = new Vector2(Settings.screenWidth - font.MeasureString("GameOver!").X, (Settings.screenHeigth / 2.0f));
            spriteBatch.DrawString(font, "GameOver!", fontPosition, Color.DarkRed);

            SpriteFont fontBackToTitleScreen = Content.Load<SpriteFont>("Fonts/TitleFont");
            fontPosition.X = 40;
            fontPosition.Y = Settings.screenHeigth - 120;
            spriteBatch.DrawString(fontBackToTitleScreen, "Try again...", fontPosition, Color.Black, 0.0f, Vector2.Zero,
                0.7f, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        private void DrawCreditsState()
        {
            spriteBatch.Begin();
            backgroundTitleScreen.Draw(spriteBatch);
            SpriteFont font = Content.Load<SpriteFont>("Fonts/TitleFont");
            Vector2 fontPosition = new Vector2(20, 200);

            spriteBatch.DrawString(font, "Pirate Treasure Hunter", fontPosition, Color.Black, 0.0f, Vector2.Zero,
                0.7f, SpriteEffects.None, 0);
            fontPosition.Y += 25;
            spriteBatch.DrawString(font, "created by Vitor Navarro", fontPosition, Color.Black, 0.0f, Vector2.Zero,
                0.5f, SpriteEffects.None, 0);
            fontPosition.Y += 25;
            spriteBatch.DrawString(font, "www.vnavarro.com.br", fontPosition, Color.Black, 0.0f, Vector2.Zero,
                0.5f, SpriteEffects.None, 0);
            fontPosition.Y += 25;
            spriteBatch.DrawString(font, "Game song is a courtesy from chocobo eater", fontPosition, Color.Black, 0.0f, Vector2.Zero,
                0.5f, SpriteEffects.None, 0);
            fontPosition.Y += 25;
            spriteBatch.DrawString(font, "Sound Effects are a courtesy from http://www.pacdv.com/", fontPosition, Color.Black, 0.0f, Vector2.Zero,
                0.5f, SpriteEffects.None, 0);


            spriteBatch.End();
        }
    }


}
