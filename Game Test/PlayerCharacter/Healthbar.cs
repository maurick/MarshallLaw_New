﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game_Test
{
    public class Healthbar
    {
        private Texture2D TextureRect, HorLine, VerLine;
        private Color[] HorLineData, VerLineData, Rectangle;
        private float width, height;
        public float rectwidth;
        private bool Changed = true, Drawn = false;

        public Healthbar()
        {
            width = 2 * GameSettings.Instance.Tilescale.X;
            rectwidth = width;
            height = 0.25f * GameSettings.Instance.Tilescale.Y;
            HorLineData = new Color[(int)(width * 1)];
            VerLineData = new Color[(int)(1 * height)];
            Rectangle = new Color[(int)(rectwidth * height)];
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 Position)
        {
            if (!Drawn)
            {
                Drawn = true;
                HorLine = new Texture2D(spriteBatch.GraphicsDevice, (int)width, 1);
                VerLine = new Texture2D(spriteBatch.GraphicsDevice, 1, (int)height);
                for (int i = 0; i < HorLineData.Length; i++)
                    HorLineData[i] = Color.Black;
                for (int i = 0; i < VerLineData.Length; i++)
                    VerLineData[i] = Color.Black;

                HorLine.SetData(HorLineData);
                VerLine.SetData(VerLineData);
            }
            if (Changed)
            {
                Changed = false;
                TextureRect = new Texture2D(spriteBatch.GraphicsDevice, (int)rectwidth, (int)height);
                for (int i = 0; i < Rectangle.Length; i++)
                    Rectangle[i] = Color.Red;
                TextureRect.SetData(Rectangle);
            }

            spriteBatch.Draw(TextureRect, Position, Color.White);
            spriteBatch.Draw(HorLine, Position, Color.White);
            spriteBatch.Draw(HorLine, Position + new Vector2(0, height), Color.White);
            spriteBatch.Draw(VerLine, Position, Color.White);
            spriteBatch.Draw(VerLine, Position + new Vector2(width, 0), Color.White);
        }

        public void LoseHealth(float value)
        {
            Changed = true;
            rectwidth -= value;
            if (rectwidth < 1)
                rectwidth = 1;
            Rectangle = new Color[(int)(rectwidth * height)];
        }
    }
}