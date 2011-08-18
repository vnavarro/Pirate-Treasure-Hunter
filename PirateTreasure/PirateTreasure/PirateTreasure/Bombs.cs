using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace PirateTreasure
{
    class Bombs
    {
        public List<FallingObjectsSprite> bombs = new List<FallingObjectsSprite>();
        private Random nrGenerator = new Random();
        private int quantity = 5;
        private SoundEffect explosion;

        public Bombs()
        {
            Vector2 speed;
            for (int i = 0; i < quantity; i++)
            {
                speed = new Vector2(0, nrGenerator.Next(80, 150));
                bombs.Add(new FallingObjectsSprite("Sprites/Bomb", nrGenerator,speed));
                
            }
        }

        public void LoadContent(ContentManager gameContent)
        {
            explosion = gameContent.Load<SoundEffect>("Sounds/Effects/bang_6");
            foreach (FallingObjectsSprite bomb in bombs)
            {
                bomb.LoadContent(gameContent);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (FallingObjectsSprite bomb in bombs)
            {                                
                bomb.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (FallingObjectsSprite bomb in bombs)
            {                
                bomb.Draw(spriteBatch);
            }
        }

        public void ResetAll()
        {
            foreach (FallingObjectsSprite bomb in bombs)
            {
                bomb.Reset();
            }
        }

        public void ResetCollidingBombs()
        {
            foreach (FallingObjectsSprite bomb in bombs)
            {
                if (bomb.IsColliding)
                {
                    bomb.Reset();
                }
            }
        }

        public void PlayEffects()
        {
            Game1.songPlayer.Play(explosion);
        }
    }
}
