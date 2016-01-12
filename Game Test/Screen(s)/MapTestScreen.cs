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

        public bool GamePaused;

        private PauseMenu menu;

        public MapTestScreen()
        {

            map = new Map("testmap7");
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
            if (GamePaused)
            {
                base.Update(gameTime);
                menu.Update(gameTime);
                if (menu.Pause == true)
                {
                    menu.UnloadContent();
                    GamePaused = false;
                }
                else
                    return;
            }

            base.Update(gameTime);

            map.Update(gameTime);

            //When the Escape key has been pressed exit the game
            if (InputManager.Instance.KeyReleased(Keys.Escape) && GamePaused == false)
            {
                //ScreenManager.Instance.ChangeScreen("MenuScreen");
                GamePaused = true;
                menu = new PauseMenu();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (GamePaused)
            {
                menu.Draw(spriteBatch);
                return;
            }
            base.Draw(spriteBatch);
            
            map.DrawBackground(spriteBatch);
            map.Draw(spriteBatch);
        }
    }
}
