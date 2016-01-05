using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class Tile
    {
        public int TileID { get; private set; }

        public float Scale { get; set; }

        public Rectangle Source { get; set; }

        public Vector2 TileOnSheetPosition { get; private set; }

        public string TextureName{ get; private set; }

        public float Alpha { get; private set; }

        public Tile(int tileID)
        {
            this.TileID = tileID;
            Alpha = 1.0f;
        }

        public void LoadContent(string texturename)
        {
            this.TextureName = texturename;
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        { 
        }

        public void SetAlpha(float alpha)
        {
            Alpha = alpha;
        }
    }
}
