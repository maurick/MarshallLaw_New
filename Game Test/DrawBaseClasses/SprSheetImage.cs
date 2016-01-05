using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Test
{
    public class SprSheetImage
    {
        //Fields
        private string Path;
        private const int ImageSize = 64;

        public Vector2 Position { get; set; }

        public Vector2 Scale { get; set; }

        public Color Color { get; set; }

        public int SprSheetX { get; set; }

        public int SprSheetY { get; set; }

        private Rectangle SourceRect, Source;
        private Texture2D Texture;

        ContentManager content;

        public SprSheetImage(string path)
        {
            Path = path;
            Position = Vector2.Zero;
            SourceRect = Rectangle.Empty;
            SprSheetX = 0;
            SprSheetY = 10;
            Source = new Rectangle(SprSheetX * ImageSize, SprSheetY * ImageSize, ImageSize, ImageSize);
            Color = Color.White;
        }

        public SprSheetImage(string path, int sprSheetX, int sprSheetY)
        {
            Path = path;
            Position = Vector2.Zero;
            SourceRect = Rectangle.Empty;
            this.SprSheetX = sprSheetX;
            this.SprSheetY = sprSheetY;
            Source = new Rectangle(SprSheetX * ImageSize, SprSheetY * ImageSize, ImageSize, ImageSize);
            Color = Color.White;
        }

        /// <summary>
        /// Load the content for the Image
        /// </summary>
        /// <param name="pos_X">Defines the X coordinate for the Position</param>
        /// <param name="pos_Y">Defines the Y coordinate for the Position</param>
        /// <param name="centered">If true then if the coordinates are 0 then the image will be centered</param>
        /// <param name="scale">Scale is used to create the Image size</param>
        public void LoadContent(float pos_X, float pos_Y, bool centered, Vector2 scale)
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (Texture == null)
                Texture = content.Load<Texture2D>(Path);

            Vector2 dimensions = Vector2.Zero;
            Position = new Vector2(pos_X, pos_Y);

            dimensions.X = scale.X * dimensions.X;
            dimensions.Y = scale.Y * dimensions.Y;

            //When the Position is 0 and the Texture dimensions are smaller then the window and
            //the image is supposed to be centered it will center the image

            if (Position.X == 0 && GameSettings.Instance.Dimensions.X > dimensions.X && centered)
            {
                pos_X = (GameSettings.Instance.Dimensions.X - dimensions.X) / 2;
            }
            if (Position.Y == 0 && GameSettings.Instance.Dimensions.Y > dimensions.Y && centered)
            {
                pos_Y = (GameSettings.Instance.Dimensions.Y - dimensions.Y) / 2;
            }

            Position = new Vector2(pos_X, pos_Y);

            SourceRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(dimensions.X), (int)(dimensions.Y));

            Scale = scale;
        }

        public void UnloadContent()
        {
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {
            //Update position in spritesheet
            Source = new Rectangle(SprSheetX * ImageSize, SprSheetY * ImageSize, ImageSize, ImageSize);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Texture, Position + origin, SourceRect, Color.White * Alpha, 0.0f, origin, Scale, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(Texture, Position, SourceRect, Color.White * Alpha, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(Texture, SourceRect, Color.White * Alpha);

            //Draw the Image
            spriteBatch.Draw(Texture, Position, Source, Color.White, 0, new Vector2(0, 0), 0.75f, SpriteEffects.None, 0.5f);
            //spriteBatch.Draw(Texture, Position, Color.White);
            //spriteBatch.Draw(Texture, Position, SourceRect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(Texture, Position, null, Color * Alpha, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0.0f);
        }

        public void SetScale(Vector2 Scale)
        {
            this.Scale = Scale;

        }
    }
}
