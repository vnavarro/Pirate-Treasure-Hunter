using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PirateTreasure
{
    class FallingObjectsSprite : Sprite
    {        
        protected Vector2 fallingSpeed;
        public Random nrGenerator;
        public bool isFalling = false;        

        public FallingObjectsSprite()
        {
        }

        public FallingObjectsSprite(string assetName, Random nrGenerator,Vector2 fallingSpeed)
        {
            this.nrGenerator = nrGenerator;
            AssetName = assetName;
            this.fallingSpeed = fallingSpeed;
        }

        public FallingObjectsSprite(string assetName, Random nrGenerator)
        {
            this.nrGenerator = nrGenerator;
            AssetName = assetName;
            this.fallingSpeed = new Vector2(0, nrGenerator.Next(60, 120));
        }

        public virtual void LoadContent(ContentManager gameContent)
        {
            base.LoadContent(gameContent, AssetName);            
            Position = new Vector2(GenerateNewLocationX(), GenerateNewLocationY());
        }

        public virtual void Update(GameTime gameTime)
        {
            if (this.Position.Y < 540)//640 + Size.Height))
            {
                base.Update(gameTime, fallingSpeed, new Vector2(0, 1));
                isFalling = true;
            }
            else            
                Reset();            
        }

        private int GenerateNewLocationX()
        {
             return nrGenerator.Next(0 + Size.Width, 480 - Size.Height);
        }
        private int GenerateNewLocationY()
        {
           return nrGenerator.Next(10, 90) * -1;
        }

        public virtual void Reset()
        {
            Position.X = GenerateNewLocationX();
            Position.Y = GenerateNewLocationY();
            fallingSpeed = new Vector2(0, nrGenerator.Next(60, 120));
            isFalling = false;
            IsColliding = false;
        }       
    }
}
