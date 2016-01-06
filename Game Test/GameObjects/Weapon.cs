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
        private SprSheetImage sprbow, sprquiver, sprarrow;

        public int SprSheetX
        {
            get { return sprSheetX; }
            set
            {
                sprSheetX = value;
                if (sprite != null)
                    sprite.SprSheetX = sprSheetX;
                else
                {
                    sprbow.SprSheetX = sprSheetX;
                    sprquiver.SprSheetX = sprSheetX;
                    sprarrow.SprSheetX = sprSheetX;
                }
            }
        }

        public int SprSheetY
        {
            get { return sprSheetY; }
            set
            {
                sprSheetY = value;
                if (sprite != null)
                    sprite.SprSheetY = sprSheetY;
                else
                {
                    sprbow.SprSheetY = sprSheetY;
                    sprquiver.SprSheetY = sprSheetY;
                    sprarrow.SprSheetY = sprSheetY;
                }
            }
        }

        public Weapon(string path)
        {
            sprite = new SprSheetImage(path);
        }

        /// <summary>
        /// 3 paths needed for bow quiver and arrow
        /// </summary>
        /// <param name="bow"></param>
        /// <param name="quiver"></param>
        /// <param name="arrow"></param>
        public Weapon(string bow, string quiver, string arrow)
        {
            sprbow = new SprSheetImage(bow);
            sprquiver = new SprSheetImage(quiver);
            sprarrow = new SprSheetImage(arrow);   
        }

        public void LoadContent(int X, int Y)
        {
            if (sprite != null)
                sprite.LoadContent(X, Y, new Vector2(64 / (GameSettings.Instance.Tilescale.X * 2), 64 / (GameSettings.Instance.Tilescale.Y * 2)));
            else
            {
                sprbow.LoadContent(X, Y, new Vector2(64 / (GameSettings.Instance.Tilescale.X * 2), 64 / (GameSettings.Instance.Tilescale.Y * 2)));
                sprquiver.LoadContent(X, Y, new Vector2(64 / (GameSettings.Instance.Tilescale.X * 2), 64 / (GameSettings.Instance.Tilescale.Y * 2)));
                sprarrow.LoadContent(X, Y, new Vector2(64 / (GameSettings.Instance.Tilescale.X * 2), 64 / (GameSettings.Instance.Tilescale.Y * 2)));
            }
        }

        public void UnloadContent()
        {
            if (sprite != null)
                sprite.UnloadContent();
            else
            {
                sprbow.UnloadContent();
                sprquiver.UnloadContent();
                sprarrow.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (sprite != null)
                sprite.Update(gameTime);
            else
            {
                sprbow.Update(gameTime);
                sprquiver.Update(gameTime);
                sprarrow.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (sprite != null)
                sprite.Draw(spriteBatch);
            else
            {
                sprquiver.Draw(spriteBatch);
                sprbow.Draw(spriteBatch);
                sprarrow.Draw(spriteBatch);
            }
        }

        public void setPosition(Vector2 position)
        {
            if (sprite != null)
                sprite.Position = position;
            else
            {
                sprbow.Position = position;
                sprquiver.Position = position;
                sprarrow.Position = position;
            }
        }
    }
}
