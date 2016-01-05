using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Test
{
    /// <summary>
    /// Base class for drawing screen, each screen has to inherit this class
    /// Then the class can be used with the ScreenManager
    /// </summary>
    public class Screen
    {
        protected ContentManager content;

        public Type Type;
        public bool IsVisible;
        public Vector2 Size;

        public Screen()
        {
            Type = this.GetType();
        }

        public virtual void LoadContent()
        {
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime)
        {
            if(!ScreenManager.Instance.IsTransitioning)
                InputManager.Instance.Update();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
