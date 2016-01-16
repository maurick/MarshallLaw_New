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
        cText Text;
        private float width, height, rectwidth;

        public LevelIndicator()
        {
            Text = new cText("Level 1", "DryGood");
            Text.LoadContent();

            width = 0.5f * GameSettings.Instance.Tilescale.X;
            rectwidth = width;
            height = 0.25f * GameSettings.Instance.Tilescale.Y;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position)
        {
            Text.Position = Position + new Vector2(0, -2 * height);
            Text.DrawString(spriteBatch, new Vector2(0.2f, 0.15f));
        }
    }
}