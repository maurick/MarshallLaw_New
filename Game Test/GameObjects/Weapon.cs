using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Test
{
    public class Weapon
    {
        private int sprSheetX, sprSheetY;

        private SprSheetImage sprite;

        public int SprSheetX
        {
            get { return sprSheetX; }
            set
            {
                sprSheetX = value;
                sprite.SprSheetX = sprSheetX;
            }
        }

        public int SprSheetY
        {
            get { return sprSheetY; }
            set
            {
                sprSheetY = value;
                sprite.SprSheetY = sprSheetY;
            }
        }

        public Weapon()
        {
            sprite = new SprSheetImage("Weapons/spear_male");
        }

        public void LoadContent(int X, int Y)
        {
            sprite.LoadContent(X, Y, false, new Vector2(64 / (GameSettings.Instance.Tilescale.X * 2), 64 / (GameSettings.Instance.Tilescale.Y * 2)));
        }

        public void UnloadContent()
        {
            sprite.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public void setPosition(Vector2 position)
        {
            sprite.Position = position;
        }

        public Vector2 getPosition()
        {
            return sprite.Position;
        }
    }
}
