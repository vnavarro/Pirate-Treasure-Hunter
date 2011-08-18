using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace PirateTreasure
{
    class ExtraLife : FallingObjectsSprite
    {
        private float createTimeInSec = 40;
        private float timeUntilNextLife = 0;        

        public ExtraLife()
        {            
            nrGenerator = new Random();
            AssetName = "Sprites/heart";
            Scale = 0.5f;
        }

        public override void Update(GameTime gameTime)
        {
            bool canInsertLife = false;
            timeUntilNextLife += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeUntilNextLife > createTimeInSec)
            {
                timeUntilNextLife = 0;
                if (!isFalling)
                {
                    canInsertLife = true;
                    this.fallingSpeed = new Vector2(0, nrGenerator.Next(120, 150));
                }
            }

            if (isFalling || canInsertLife)
            {
                base.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isFalling)
            {
                base.Draw(spriteBatch);
            }
        }

        public void CollisionReset()
        {
            if (IsColliding)
            {
                timeUntilNextLife = 0;
                base.Reset();
            }
        }

        public override void Reset()
        {            
            timeUntilNextLife = 0;
            base.Reset();
        }
    }
}
