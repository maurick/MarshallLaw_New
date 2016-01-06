using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Test
{
    public class Arrow
    {
        SprSheetImage sprite;
        private int sprSheetY;
        private const int Velocity = 5;

        public int ArrowID { get; set; }

        /// <summary>
        /// sprSheetY:
        /// 1 = up
        /// 2 = left
        /// 3 = down
        /// 4 = right
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sprSheetY"></param>
        /// <param name="position"></param>
        public Arrow(string path, int sprSheetY, Vector2 position)
        {
            this.sprSheetY = sprSheetY + 15;
            sprite = new SprSheetImage(path, 7 ,this.sprSheetY);
            this.sprSheetY = sprSheetY + 15;
            sprite.Position = position;
            LoadContent((int)position.X, (int)position.Y);
        }

        private void LoadContent(int X, int Y)
        {
            sprite.LoadContent(X, Y, new Vector2(64 / (GameSettings.Instance.Tilescale.X * 2), 64 / (GameSettings.Instance.Tilescale.Y * 2)));
        }

        public void UnloadContent()
        {
            sprite.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            switch (sprSheetY)
            {
                case 16:
                    sprite.Position -= new Vector2(0, Velocity);
                    break;
                case 17:
                    sprite.Position -= new Vector2(Velocity, 0);
                    break;
                case 18:
                    sprite.Position += new Vector2(0, Velocity);
                    break;
                case 19:
                    sprite.Position += new Vector2(Velocity, 0);
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        public bool CheckCollision(Layer layer)
        {
            int x = (int)(sprite.Position.X / GameSettings.Instance.Tilescale.X),
                y = (int)((sprite.Position.Y + 0.5 * GameSettings.Instance.Tilescale.Y) / GameSettings.Instance.Tilescale.Y);

            int TileID = layer.getTileID(x, y);
            if (TileID != 0)
                return true;
            else
                return false;
        }
    }
}
