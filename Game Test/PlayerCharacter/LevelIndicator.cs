using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game_Test
{
    public class LevelIndicator
    {
        public cText Text;
        private float width, height;

        public LevelIndicator()
        {
            Text = new cText("1", "DryGood");
            Text.LoadContent();

            width = 1f/3f * GameSettings.Instance.Tilescale.X;
            height = 0.25f * GameSettings.Instance.Tilescale.Y;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position)
        {
            Text.Position = Position + new Vector2(-width, 0);
            Text.DrawString(spriteBatch, new Vector2(0.2f, 0.15f));
        }
    }
}