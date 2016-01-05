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

        public Vector2 Real_Scale;

        private Rectangle Source;
        private Texture2D Texture;

        ContentManager content;

        public SprSheetImage(string path)
        {
            Path = path;
            Position = Vector2.Zero;
            SprSheetX = 0;
            SprSheetY = 10;
            Source = new Rectangle(SprSheetX * ImageSize, SprSheetY * ImageSize, ImageSize, ImageSize);
            Color = Color.White;
            Real_Scale = new Vector2(0.75f, 0.75f);
        }

        public SprSheetImage(string path, int sprSheetX, int sprSheetY)
        {
            Path = path;
            Position = Vector2.Zero;
            this.SprSheetX = sprSheetX;
            this.SprSheetY = sprSheetY;
            Source = new Rectangle(SprSheetX * ImageSize, SprSheetY * ImageSize, ImageSize, ImageSize);
            Color = Color.White;
            Real_Scale = new Vector2(0.75f, 0.75f);
        }

        /// <summary>
        /// Load the content for the Image
        /// </summary>
        /// <param name="pos_X">Defines the X coordinate for the Position</param>
        /// <param name="pos_Y">Defines the Y coordinate for the Position</param>
        /// <param name="centered">If true then if the coordinates are 0 then the image will be centered</param>
        /// <param name="scale">Scale is used to create the Image size</param>
        public void LoadContent(float pos_X, float pos_Y, Vector2 scale)
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (Texture == null)
                Texture = content.Load<Texture2D>(Path);

            Vector2 dimensions = Vector2.Zero;
            Position = new Vector2(pos_X, pos_Y);

            dimensions.X = scale.X * dimensions.X;
            dimensions.Y = scale.Y * dimensions.Y;

            Position = new Vector2(pos_X, pos_Y);

            Scale = scale;
        }

        public void LoadContent(Vector2 position, Vector2 scale)
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            if (Texture == null)
                Texture = content.Load<Texture2D>(Path);

            Vector2 dimensions = Vector2.Zero;
            Position = position;

            dimensions.X = scale.X * dimensions.X;
            dimensions.Y = scale.Y * dimensions.Y;

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
            spriteBatch.Draw(Texture, Position, Source, Color.White, 0, new Vector2(0, 0), Real_Scale, SpriteEffects.None, 0.5f);
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
