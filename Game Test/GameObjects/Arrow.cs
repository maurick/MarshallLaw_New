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
        public Vector2 sprSheet { get; private set; }
        private const int Velocity = 5;

        public Rectangle ArrowRect { get; private set; }

        /// <summary>
        /// sprSheetY:
        /// up = 1,2
        /// left = 2,1
        /// down = 2,2
        /// right = 1,1
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sprSheetY"></param>
        /// <param name="Position"></param>
        public Arrow(string path, Vector2 Position, int sprSheetX, int sprSheetY)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;
            sprite = new SprSheetImage(path, sprSheetX - 1, sprSheetY - 1);
            sprSheet = new Vector2(sprSheetX, sprSheetY);
            LoadContent((int)Position.X, (int)Position.Y);
            if (sprSheet == new Vector2(2, 1)) //up
            {
                ArrowRect = new Rectangle((int)(sprite.Position.X + 0.5 * tilescale_x), (int)sprite.Position.Y, (int)(tilescale_x * 0.5), (int)tilescale_y);
            }
            else if (sprSheet == new Vector2(1, 2)) //left
            {
                ArrowRect = new Rectangle((int)sprite.Position.X, (int)(sprite.Position.Y + tilescale_y), (int)tilescale_x, (int)(tilescale_y * 0.5));
            }
            else if (sprSheet == new Vector2(2, 2)) //down
            {
                ArrowRect = new Rectangle((int)(sprite.Position.X + 0.5 * tilescale_x), (int)sprite.Position.Y, (int)(tilescale_x * 0.5), (int)tilescale_y * 2);
            }
            else if (sprSheet == new Vector2(1, 1)) //right
            {
                ArrowRect = new Rectangle((int)sprite.Position.X, (int)(sprite.Position.Y + 0.5 * tilescale_y), (int)tilescale_x, (int)(tilescale_y * 0.5));
            }
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
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;
            if (sprSheet == new Vector2(2, 1))
            {
                sprite.Position -= new Vector2(0, Velocity);
                ArrowRect = new Rectangle((int)(sprite.Position.X + 0.75 * tilescale_x), (int)sprite.Position.Y, ArrowRect.Width, ArrowRect.Height);
            }
            else if (sprSheet == new Vector2(1, 2))
            {
                sprite.Position -= new Vector2(Velocity, 0);
                ArrowRect = new Rectangle((int)sprite.Position.X, (int)(sprite.Position.Y + 0.75 * tilescale_x), ArrowRect.Width, ArrowRect.Height);
            }
            else if (sprSheet == new Vector2(2, 2))
            {
                sprite.Position += new Vector2(0, Velocity);
                ArrowRect = new Rectangle((int)(sprite.Position.X + 0.75 * tilescale_x), (int)sprite.Position.Y, ArrowRect.Width, ArrowRect.Height);
            }
            else if (sprSheet == new Vector2(1, 1))
            {
                sprite.Position += new Vector2(Velocity, 0);
                ArrowRect = new Rectangle((int)sprite.Position.X, (int)(sprite.Position.Y + 0.75 * tilescale_y), ArrowRect.Width, ArrowRect.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }

        #region CheckCollision
        public bool CheckCollision(Layer layer)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;
            //TODO
            //Add advanced collision
            int[] x = new int[2],
                y = new int[2];
            x[0] = (int)(sprite.Position.X / tilescale_x);
            x[1] = (int)(sprite.Position.X / tilescale_x);
            y[0] = (int)((sprite.Position.Y + 1 * tilescale_y) / tilescale_y);
            y[1] = (int)((sprite.Position.Y + 1 * tilescale_y) / tilescale_y);
            
            Vector2 temp;

            if (sprSheet == new Vector2(1, 2)) //Left
            {
                if (!((int)(ArrowRect.Y / tilescale_y) == (int)(ArrowRect.Y + 0.5 * tilescale_y)))
                    y[1]++;
                temp = CheckCollision2(ArrowRect, x, y, layer);
                if (temp.X == 1 || temp.Y == 1)
                    return true;
                else
                    return false;
            }
            else if (sprSheet == new Vector2(2, 2)) //Down
            {
                y[0]++;
                y[1]++;
                if (!((int)(ArrowRect.X / tilescale_x) == (int)(ArrowRect.X + 0.5 * tilescale_x)))
                    x[1]++;
                temp = CheckCollision2(ArrowRect, x, y, layer);
                if (temp.X == 1 || temp.Y == 1)
                    return true;
                else
                    return false;
            }
            else if (sprSheet == new Vector2(1, 1)) //Right
            {
                x[0]++;
                x[1]++;
                if (!((int)(ArrowRect.Y / tilescale_y) == (int)(ArrowRect.Y + 0.5 * tilescale_y)))
                    y[1]++;
                temp = CheckCollision2(ArrowRect, x, y, layer);
                if (temp.X == 1 || temp.Y == 1)
                    return true;
                else
                    return false;
            }
            else if (sprSheet == new Vector2(2, 1)) //Up
            {
                if (!((int)(ArrowRect.X / tilescale_x) == (int)(ArrowRect.X + 0.5 * tilescale_x)))
                    x[1]++;
                temp = CheckCollision2(ArrowRect, x, y, layer);
                if (temp.X == 1 || temp.Y == 1)
                    return true;
                else
                    return false;
            }
            return false;
        }

        private Vector2 CheckCollision2(Rectangle playerRect, int[] x, int[] y, Layer layer)
        {
            int temp1 = 0, temp2 = 0;

            temp1 = CheckCollision3(x[0], y[0], playerRect, layer);
            temp2 = CheckCollision3(x[1], y[1], playerRect, layer);

            return new Vector2(temp1, temp2);
        }

        private int CheckCollision3(int x, int y, Rectangle playerRect, Layer layer)
        {
            float tilescale_x = GameSettings.Instance.Tilescale.X, tilescale_y = GameSettings.Instance.Tilescale.Y;
            Rectangle rect;
            int temp = 0;

            if (x == -1)
            { }
            int TileID = layer.getTileID(x, y);

            if (TileID != 0)
            {
                switch (TileID)
                {
                    case 1677: //Full
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)tilescale_x, (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1678: //Tophalf
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1679: //Bottomhalf
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(0.5 * tilescale_y), (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1680: //Diagonallefttoright
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y + (int)(tilescale_x * 0.5), (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1681: //Diagonalrighttoleft
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(tilescale_x * 0.5), (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1683: //Righthalf
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1684: //Lefttopcorner
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1685: //Righttopcorner
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1686: //Lefttop
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1687: //Righttop
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1689: //Lefthalf
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1690: //Leftbottomcorner
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(0.5 * tilescale_y), (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1691: //Rightbottomcorner
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y, (int)(tilescale_x * 0.5), (int)tilescale_y);
                        temp = CheckCollision4(rect, playerRect);
                        if (temp == 1)
                            break;
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(0.5 * tilescale_y), (int)tilescale_x, (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1692: //Leftbottom
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(tilescale_x * 0.5), (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1693: //Rightbottom
                        rect = new Rectangle(x * (int)tilescale_x + (int)(tilescale_x * 0.5), y * (int)tilescale_y + (int)(tilescale_x * 0.5), (int)(tilescale_x * 0.5), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1694: //Bridgeright
                        rect = new Rectangle(x * (int)tilescale_x + (int)(0.125 * tilescale_x), y * (int)tilescale_y, (int)(tilescale_x * 0.875), (int)(tilescale_y));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1688: //Bridgeleft
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.875), (int)(tilescale_y));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1695: //Bridgebottomright
                        rect = new Rectangle(x * (int)tilescale_x + (int)(0.125 * tilescale_x), y * (int)tilescale_y + (int)(tilescale_y * 0.5), (int)(tilescale_x * 0.875), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1696: //Bridgebottomleft
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y + (int)(tilescale_y * 0.5), (int)(tilescale_x * 0.875), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1697: //Bridgetopright
                        rect = new Rectangle(x * (int)tilescale_x + (int)(0.125 * tilescale_x), y * (int)tilescale_y, (int)(tilescale_x * 0.875), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;
                    case 1698: //Bridgetopleft
                        rect = new Rectangle(x * (int)tilescale_x, y * (int)tilescale_y, (int)(tilescale_x * 0.875), (int)(tilescale_y * 0.5));
                        temp = CheckCollision4(rect, playerRect);
                        break;

                }
            }
            return temp;
        }

        private int CheckCollision4(Rectangle rect, Rectangle playerRect)
        {
            if (rect.Intersects(playerRect))
            {
                return 1;
            }
            else return 0;
        }
        #endregion

    }
}
