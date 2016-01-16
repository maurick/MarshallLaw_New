using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Test
{
    public class Test : Screen
    {
        Image background;
        List<Player> players;

        public Test()
        {
            players = new List<Player> { };
            for (int i = 0; i < ScreenManager.Instance.Controllers.Count; i++)
            {
                players.Add(new Player(i));
            }
            background = new Image("TitleScreen/background");
        }

        public override void LoadContent()
        {
            base.LoadContent();

            foreach (Player player in players)
            {
                player.LoadContent(0, 0);
            }
            
            background.LoadContent(0, 0, true, Vector2.One);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            foreach (Player player in players)
            {
                player.UnloadContent();
            }
            
            background.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Player player in players)
            {
                player.Update(gameTime);
            }
            

            if (InputManager.Instance.KeyPressed(Keys.Escape))
            {
                ScreenManager.Instance.ChangeScreen("MenuScreen");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            background.Draw(spriteBatch);
            foreach (Player player in players)
            {
                player.Draw(spriteBatch);
            }
            
        }
    }
}
