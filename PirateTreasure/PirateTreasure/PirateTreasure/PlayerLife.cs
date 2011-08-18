using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PirateTreasure
{
    class PlayerLife : Sprite
    {
        private int POSITION_X = 0;
        private int POSITION_Y = 0;

        private int quantity = 3;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public PlayerLife()
        {
            AssetName = "Sprites/heart";
            Scale = 0.8f;
        }


        public void LoadContent(ContentManager gameContent)
        {
            base.LoadContent(gameContent, AssetName);
            POSITION_X = Settings.screenWidth - 10 - Size.Width;
            POSITION_Y = 10;
            Position = new Vector2(POSITION_X, POSITION_Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 originalPosition = Position;
            for (int i = 0; i < quantity; i++)
            {
                base.Draw(spriteBatch);
                Position += new Vector2(-this.Size.Width - 10, 0);
            }
            Position = originalPosition;
        }
    }
}
