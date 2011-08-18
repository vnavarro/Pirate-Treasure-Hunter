using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PirateTreasure
{
    public class Sprite
    {
        public Vector2 Position = new Vector2(0, 0);
        protected Texture2D _spriteTexture;
        protected string AssetName;
        public Rectangle Size;
        protected float _scale = 1.0f;
        public float Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
            }
        }
        protected int CollisionRadius = 0;
        protected int BoundingXPadding = 0;
        protected int BoundingYPadding = 0;
        
        public Rectangle BoundingBoxRect
        {
            get
            {
                return new Rectangle((int)Position.X + BoundingXPadding, (int)Position.Y + BoundingYPadding, Size.Width - (BoundingXPadding * 2), Size.Height - (BoundingYPadding * 2));                
            }
        }

        public bool IsColliding { get; set; }

        #region methods
        public void RescaleTexture()
        {
            if (_spriteTexture != null)
                Size = new Rectangle(0, 0, (int)(_spriteTexture.Width * Scale), (int)(_spriteTexture.Height * Scale));
        }

        public virtual void LoadContent(ContentManager gameContent, string assetName)
        {
            this.AssetName = assetName;
            IsColliding = false;
            _spriteTexture = gameContent.Load<Texture2D>(AssetName);
            RescaleTexture();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_spriteTexture, Position,
                new Rectangle(0, 0, _spriteTexture.Width, _spriteTexture.Height),
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public virtual void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        #endregion
    }
}
