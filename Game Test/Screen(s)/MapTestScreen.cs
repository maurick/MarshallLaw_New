using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Game_Test
{
    public class MapTestScreen : Screen
    {
        Map map;

        Texture2D Texture;
        string Path = "SpriteSheets/terrain";

        private bool background = false;

        public MapTestScreen()
        {

            map = new Map("testmap5");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

                Texture = content.Load<Texture2D>(Path);
            map.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            map.Update(gameTime);

            //When the Escape key has been pressed exit the game
            if (InputManager.Instance.KeyPressed(Keys.Escape))
            {
                ScreenManager.Instance.ChangeScreen("MenuScreen");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            map.DrawBackground(spriteBatch);
            map.Draw(spriteBatch);
        }
    }
}
