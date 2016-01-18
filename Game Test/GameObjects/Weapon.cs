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
        public bool Quiver { get; private set; }

        public PlayerEnums.Weapontype weapontype { get; private set; }
        public int WeaponID { get; private set; }

        public int GetMaxID { get { return 2; } }

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

        public Weapon(string path, PlayerEnums.Weapontype weapontype, Vector2 Position, int weaponID, object sender)
        {
            sprite = new SprSheetImage(path);
            this.weapontype = weapontype;
            LoadContent((int)Position.X, (int)Position.Y);
            WeaponID = weaponID;
            Quiver = false;
        }

        /// <summary>
        /// 3 paths needed for bow quiver and arrow
        /// </summary>
        /// <param name="bow"></param>
        /// <param name="quiver"></param>
        /// <param name="arrow"></param>
        public Weapon(string bow, string quiver, string arrow, Vector2 Position, int weaponID, object sender)
        {
            sprbow = new SprSheetImage(bow);
            sprquiver = new SprSheetImage(quiver);
            sprarrow = new SprSheetImage(arrow);
            weapontype = PlayerEnums.Weapontype.Bow;
            LoadContent((int)Position.X, (int)Position.Y);
            WeaponID = weaponID;
            Quiver = true;

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
                sprbow.Draw(spriteBatch);
                sprarrow.Draw(spriteBatch);
            }
        }

        public void DrawQuiver(SpriteBatch spriteBatch)
        {
            sprquiver.Draw(spriteBatch);
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
