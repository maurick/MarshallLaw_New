using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game_Test
{
    public class Expbar
    {
        private Texture2D TextureRect, HorLine, VerLine;
        private Color[] HorLineData, VerLineData, Rectangle;
        private float width, height;
        public float rectwidth;

        public Expbar()
        {
            width = 2 * GameSettings.Instance.Tilescale.X;
            rectwidth = width;
            height = 0.15f * GameSettings.Instance.Tilescale.Y;
            HorLineData = new Color[(int)(width * 1)];
            VerLineData = new Color[(int)(1 * height)];
            Rectangle = new Color[(int)(rectwidth * height)];
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position)
        {
            Position = Position + new Vector2(0, 0.25f * GameSettings.Instance.Tilescale.Y);
            TextureRect = new Texture2D(spriteBatch.GraphicsDevice, (int)rectwidth, (int)height);
            HorLine = new Texture2D(spriteBatch.GraphicsDevice, (int)width, 1);
            VerLine = new Texture2D(spriteBatch.GraphicsDevice, 1, (int)height);
            for (int i = 0; i < HorLineData.Length; i++)
                HorLineData[i] = Color.Black;
            for (int i = 0; i < VerLineData.Length; i++)
                VerLineData[i] = Color.Black;
            for (int i = 0; i < Rectangle.Length; i++)
                Rectangle[i] = Color.Yellow;

            TextureRect.SetData(Rectangle);
            HorLine.SetData(HorLineData);
            VerLine.SetData(VerLineData);

            spriteBatch.Draw(TextureRect, Position, Color.White);
            spriteBatch.Draw(HorLine, Position, Color.White);
            spriteBatch.Draw(HorLine, Position + new Vector2(0, height), Color.White);
            spriteBatch.Draw(VerLine, Position, Color.White);
            spriteBatch.Draw(VerLine, Position + new Vector2(width, 0), Color.White);
        }
        
        public void SetExp(float Percentage)
        {
            Percentage = Percentage / 100 * (2 * GameSettings.Instance.Tilescale.X);
            rectwidth = Percentage;
            if (rectwidth < 1)
                rectwidth = 1;
            Rectangle = new Color[(int)(rectwidth * height)];
        }

        public void IncreaseExp(float Percentage)
        {
            Percentage = Percentage / 100 * (2 * GameSettings.Instance.Tilescale.X);
            rectwidth += Percentage;
            if (rectwidth > 2 * GameSettings.Instance.Tilescale.X)
                rectwidth = 2 * GameSettings.Instance.Tilescale.X;
            Rectangle = new Color[(int)(rectwidth * height)];
        }
    }
}