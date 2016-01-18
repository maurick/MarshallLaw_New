using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class SpriteClasscs
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

        SprSheetImage body;
        SprSheetImage shirt;
        SprSheetImage head;
        SprSheetImage belt;
        SprSheetImage pants;

        public SpriteClasscs(string path1, string path2, string path3, string path4, string path5)
        {
            Position = Vector2.Zero;
            SprSheetX = 0;
            SprSheetY = 10;
            Source = new Rectangle(SprSheetX * ImageSize, SprSheetY * ImageSize, ImageSize, ImageSize);
            Color = Color.White;
            Real_Scale = new Vector2(0.75f, 0.75f);

            body = new SprSheetImage(path1);
            shirt = new SprSheetImage(path2);
            head = new SprSheetImage(path3);
            belt = new SprSheetImage(path4);
            pants = new SprSheetImage(path5);
        }

        public SpriteClasscs(string path1, string path2, string path3, string path4, string path5, int sprSheetX, int sprSheetY)
        {
            this.SprSheetX = sprSheetX;
            this.SprSheetY = sprSheetY;
            Source = new Rectangle(SprSheetX * ImageSize, SprSheetY * ImageSize, ImageSize, ImageSize);
            Color = Color.White;
            Real_Scale = new Vector2(0.75f, 0.75f);

            body = new SprSheetImage(path1);
            shirt = new SprSheetImage(path2);
            head = new SprSheetImage(path3);
            belt = new SprSheetImage(path4);
            pants = new SprSheetImage(path5);
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
            body.LoadContent(pos_X, pos_Y, scale);
            shirt.LoadContent(pos_X, pos_Y, scale);
            head.LoadContent(pos_X, pos_Y, scale);
            belt.LoadContent(pos_X, pos_Y, scale);
            pants.LoadContent(pos_X, pos_Y, scale);

            Vector2 dimensions = Vector2.Zero;
            Position = new Vector2(pos_X, pos_Y);

            dimensions.X = scale.X * dimensions.X;
            dimensions.Y = scale.Y * dimensions.Y;

            Position = new Vector2(pos_X, pos_Y);

            Scale = scale;
        }

        public void LoadContent(Vector2 position, Vector2 scale)
        {
            body.LoadContent(position, scale);
            shirt.LoadContent(position, scale);
            head.LoadContent(position, scale);
            belt.LoadContent(position, scale);
            pants.LoadContent(position, scale);



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
            body.UnloadContent();
            shirt.UnloadContent();
            head.UnloadContent();
            belt.UnloadContent();
            pants.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            //Update position in spritesheet
            Source = new Rectangle(SprSheetX * ImageSize, SprSheetY * ImageSize, ImageSize, ImageSize);

            body.SprSheetX = SprSheetX;
            body.SprSheetY = SprSheetY;

            shirt.SprSheetX = SprSheetX;
            shirt.SprSheetY = SprSheetY;

            head.SprSheetX = SprSheetX;
            head.SprSheetY = SprSheetY;

            belt.SprSheetX = SprSheetX;
            belt.SprSheetY = SprSheetY;

            pants.SprSheetX = SprSheetX;
            pants.SprSheetY = SprSheetY;

            body.Position = Position;
            shirt.Position = Position;
            head.Position = Position;
            belt.Position = Position;
            pants.Position = Position;

            body.Update(gameTime);
            shirt.Update(gameTime);
            head.Update(gameTime);
            belt.Update(gameTime);
            pants.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            body.Draw(spriteBatch);
            shirt.Draw(spriteBatch);
            head.Draw(spriteBatch);
            belt.Draw(spriteBatch);
            pants.Draw(spriteBatch);

        }

        public void SetScale(Vector2 Scale)
        {
            this.Scale = Scale;
        }
    }
}
