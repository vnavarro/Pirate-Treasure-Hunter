using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace PirateTreasure
{
    class CollisionControl
    {

        //TODO:Discover why List<Sprite> doesn't accept List<FallingObjectsSprite> both T are children of Sprite
        public void SearchSpriteCollision(List<FallingObjectsSprite> searchingSprites, Sprite comparingSprite)
        {
            comparingSprite.IsColliding = false;
            foreach (FallingObjectsSprite currentSprite in searchingSprites)
            {
                currentSprite.IsColliding = false;                
                if (AreBoxesColliding(currentSprite.BoundingBoxRect, comparingSprite.BoundingBoxRect)){
                    currentSprite.IsColliding = true;
                    comparingSprite.IsColliding = true;
                }
            }
        }

        public void SearchSpriteCollision(Sprite spriteOne,Sprite spriteTwo)
        {
            spriteOne.IsColliding = false;
            spriteTwo.IsColliding = false;
            if(AreBoxesColliding(spriteOne.BoundingBoxRect,spriteTwo.BoundingBoxRect))
            {
                spriteOne.IsColliding = true;
                spriteTwo.IsColliding = true;
            }
        }

        public bool AreBoxesColliding(Rectangle firstSpriteBox, Rectangle secondSpriteBox)
        {
            return firstSpriteBox.Intersects(secondSpriteBox);
        }

        public bool AreCirclesColliding(Vector2 firstSpriteCenter, float firstSpriteRadius,
            Vector2 secondSpriteCenter, float secondSpriteRadius)
        {
            if (Vector2.Distance(firstSpriteCenter, secondSpriteCenter) < (firstSpriteRadius + secondSpriteRadius))
                return true;
            else
                return false;
        }
    }
}
