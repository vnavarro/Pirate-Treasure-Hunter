using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PirateTreasure
{
    class BackgroundSprite:Sprite
    {        
        private int START_POSITION_X = 0;
        private int START_POSITION_Y = 0;

        public BackgroundSprite(string assetName)
        {
            this.AssetName = assetName;
        }

        public void LoadContent(ContentManager gameContent)
        { 
            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(gameContent, this.AssetName);
        }
    }
}
