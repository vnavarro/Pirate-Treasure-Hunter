using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace PirateTreasure
{
    class TreasureChests
    {
        public List<FallingObjectsSprite> chests = new List<FallingObjectsSprite>();
        private int quantity = 1;
        private readonly Random nrGenerator = new Random();
        public int treasureValue = 250;
        private float createTimeInSec = 20;
        private float timeUntilNextChest = 0;
        private SoundEffect catchTreasureChest;

        public TreasureChests()
        {
            for (int i = 0; i < quantity; i++)
            {
                chests.Add(new FallingObjectsSprite("Sprites/chest", nrGenerator));
            }
        }

        public void LoadContent(ContentManager gameContent)
        {
            catchTreasureChest = gameContent.Load<SoundEffect>("Sounds/Effects/coins_4");
            foreach (FallingObjectsSprite chest in chests)
            {
                chest.LoadContent(gameContent);
            }
        }

        public void Update(GameTime gameTime)
        {
            bool canInsertChest = false;
            timeUntilNextChest += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timeUntilNextChest > createTimeInSec)
            {                
                timeUntilNextChest = 0;
                if(!IsChestFalling()) canInsertChest = true;
            }

            if (IsChestFalling() || canInsertChest)
            {
                foreach (FallingObjectsSprite chest in chests)
                {
                    chest.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsChestFalling())
            {
                foreach (FallingObjectsSprite chest in chests)
                {
                    chest.Draw(spriteBatch);
                }
            }
        }

        public bool IsChestFalling()
        {
            foreach (FallingObjectsSprite chest in chests)
            {
                if (chest.isFalling) return chest.isFalling;
            }
            return false;
        }

        public void ResetAll()
        {
            timeUntilNextChest = 0;
            foreach (FallingObjectsSprite chest in chests)
            {
                chest.Reset();
            }
        }

        public void ResetCollidingChest()
        {
            foreach (FallingObjectsSprite chest in chests)
            {
                if (chest.IsColliding)
                {
                    chest.Reset();
                }
            }
        }

        public void PlayEffects()
        {
            Game1.songPlayer.Play(catchTreasureChest);
        }
    }
}
