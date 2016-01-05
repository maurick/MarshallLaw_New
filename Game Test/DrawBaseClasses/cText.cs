using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Test
{
    public class cText
    {
        //Fields
        public float Alpha;
        public string Text, FontName, Path;
        public Vector2 Position;
        public Rectangle SourceRect;
        private Vector2 scale;
        public Color Color { get; set; }
        //Vector2 dimensions;

        public Vector2 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
            }
        }

        //Vector2 origin;
        ContentManager content;
        SpriteFont font;
        Vector2 dimensions = Vector2.Zero;

        public cText(string Text, string fontname)
        {
            this.Text = Text;
            this.Path = String.Empty;

            switch(fontname)
            {
                case "Carne":
                    this.FontName = "SpriteFonts/Carne";
                    break;
                case "DryGood":
                    this.FontName = "SpriteFonts/DryGood/DryGood";
                    break;
                case "DryGood_12":
                    this.FontName = "SpriteFonts/DryGood/DryGood_12";
                    break;
            }
            this.Position = Vector2.Zero;
            this.scale = Vector2.One;
            this.Alpha = 1.0f;
            this.SourceRect = Rectangle.Empty;
            this.Color = Color.Black;
        }

        public void LoadContent()
        {
            //Load the content for the text
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            font = content.Load<SpriteFont>(FontName);

            dimensions = Vector2.Zero;

            //Make sure the text class has the dimensions from the font
            if(Text != string.Empty)
            {
                dimensions.X = font.MeasureString(Text).X;
                dimensions.Y = font.MeasureString(Text).Y;
            }

            //Create a rectangle wich other classes can work with
            SourceRect = new Rectangle(0, 0, (int)(dimensions.X * scale.X), (int)(dimensions.Y * scale.Y));

            this.scale.X = (GameSettings.Instance.Dimensions.X / 1920);
            this.scale.Y = (GameSettings.Instance.Dimensions.Y / 1080);
        }

        public void LoadContent(Vector2 position)
        {
            //Load the content for the text
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            font = content.Load<SpriteFont>(FontName);

            

            //Make sure the text class has the dimensions from the font
            if (Text != string.Empty)
            {
                dimensions.X = font.MeasureString(Text).X;
                dimensions.Y = font.MeasureString(Text).Y;
            }

            //Create a rectangle wich other classes can work with

            this.scale.X = (GameSettings.Instance.Dimensions.X / 1920);
            this.scale.Y = (GameSettings.Instance.Dimensions.Y / 1080);

            SourceRect = new Rectangle(0, 0, (int)(dimensions.X * scale.X), (int)(dimensions.Y * scale.Y));

            Position = position;
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {
            dimensions.X = font.MeasureString(Text).X;
            dimensions.Y = font.MeasureString(Text).Y;
            SourceRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(dimensions.X * scale.X), (int)(dimensions.Y * scale.Y));
        }

        public void DrawString(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, Text, Position, Color * Alpha, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        public Vector2 GetTextSize(string text)
        {
            Vector2 temp;
            temp.X = font.MeasureString(Text).X * scale.X;
            temp.Y = font.MeasureString(Text).Y * scale.Y;

            return temp;
        }
    }
}
