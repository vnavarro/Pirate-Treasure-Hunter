using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace PirateTreasure
{
    class Coins
    {
        public List<FallingObjectsSprite> coins = new List<FallingObjectsSprite>();
        private int fullQuantity = 6;
        private Random nrGenerator = new Random();
        public int coinValue = 10;
        private SoundEffect catchCoinEffect;

        public Coins()
        {
            for (int i = 0; i < fullQuantity; i++)
            {
                coins.Add(new FallingObjectsSprite("Sprites/Coin",nrGenerator));
            }
        }

        public void LoadContent(ContentManager gameContent)
        {
            catchCoinEffect = gameContent.Load<SoundEffect>("Sounds/Effects/coin_2");
            foreach (FallingObjectsSprite coin in coins)
            {
                coin.LoadContent(gameContent);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (FallingObjectsSprite coin in coins)
            {                
                if(coin.IsColliding)
                {    
                    Game1.songPlayer.Play(catchCoinEffect);
                }
                coin.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (FallingObjectsSprite coin in coins)
            {                
                coin.Draw(spriteBatch);
            }
        }

        public void ResetAll()
        {
            foreach (FallingObjectsSprite coin in coins)
            {
                coin.Reset();
            }
        }

        public void ResetCollidingCoin()
        {
            foreach (FallingObjectsSprite coin in coins)
            {
                if(coin.IsColliding)
                {
                    coin.Reset();
                }
            }
        }

        public void PlayEffects()
        {
            Game1.songPlayer.Play(catchCoinEffect);
        }
    }
}
