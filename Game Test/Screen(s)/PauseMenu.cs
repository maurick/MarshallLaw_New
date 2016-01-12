using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Test
{
    public class PauseMenu
    {
        private Image P_sign, P_poster;
        private Vector2 P_menuLenght;
        private Vector2 P_menuPosition;
        private int P_currentSelected;
        private string[] P_text = { "Continue", "Exit to menu" };
        private PauseMenuItem[] P_menuItems;
        public bool Pause { get; private set; }

        public PauseMenu()
        {
            Pause = false;
            P_sign = new Image("TitleScreen/woodsign_marshal_law");
            P_poster = new Image("TitleScreen/gun_poster1280x720");

            P_menuItems = new PauseMenuItem[P_text.Length];

            for (int i = 0; i < P_text.Length; i++)
            {
                P_menuItems[i] = new PauseMenuItem(P_text[i], i);
            }
            P_menuItems[0].Selected = true;

            LoadContent();
        }

        private void LoadContent()
        {
            P_sign.LoadContent(0, 0, false, new Vector2(GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f));
            P_poster.LoadContent(pos_X: 0,
                               pos_Y: (int)(350 * (GameSettings.Instance.Dimensions.Y / 1080f)),
                               centered: true,
                               scale: new Vector2(GameSettings.Instance.Dimensions.X / 1920f, GameSettings.Instance.Dimensions.Y / 1080f));

            for (int i = 0; i < P_text.Length; i++)
            {
                P_menuItems[i].LoadContent(i);
                P_menuLenght.Y += P_menuItems[i].Text.SourceRect.Height;
            }
            P_menuPosition.Y = P_poster.Position.Y + (((P_poster.SourceRect.Height) - P_menuLenght.Y));

            float temp = 0;
            for (int i = 0; i < P_text.Length; i++)
            {
                if (i > 0)
                {
                    temp += P_menuItems[i - 1].Text.SourceRect.Height / 2;
                    P_menuItems[i].Position.Y = P_menuPosition.Y + temp;
                }
                else
                    P_menuItems[i].Position.Y = P_menuPosition.Y;

                P_menuItems[i].SetPosition();
            }
        }

        public void UnloadContent()
        {
            P_sign.UnloadContent();
            P_poster.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {

            P_sign.Update(gameTime);
            P_poster.Update(gameTime);

            //If the down key is selected then move the selected 1 down
            if (InputManager.Instance.KeyPressed(Keys.Down))
            {
                P_menuItems[P_currentSelected].Selected = false;
                P_currentSelected++;
                if (P_currentSelected == P_text.Length)
                    P_currentSelected = 0;
                P_menuItems[P_currentSelected].Selected = true;
            }

            //If the up key is selected then move the selected 1 up
            if (InputManager.Instance.KeyPressed(Keys.Up))
            {
                P_menuItems[P_currentSelected].Selected = false;
                P_currentSelected--;
                if (P_currentSelected == -1)
                    P_currentSelected = P_text.Length - 1;
                P_menuItems[P_currentSelected].Selected = true;
            }

            if (P_menuItems[P_currentSelected].ItemID == 0 && (InputManager.Instance.KeyPressed(Keys.Enter)))
            {
                Pause = true;
            }

            //If the Exit button is selected and Enter has been pressed exit the game
            if (P_menuItems[P_currentSelected].ItemID == 1 && (InputManager.Instance.KeyPressed(Keys.Enter)))
            {
                ScreenManager.Instance.ChangeScreen("MenuScreen");
            }

            for (int i = 0; i < P_text.Length; i++)
            {
                P_menuItems[i].Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            P_sign.Draw(spriteBatch);
            P_poster.Draw(spriteBatch);
            for (int i = 0; i < P_text.Length; i++)
            {
                P_menuItems[i].Draw(spriteBatch);
            }
        }

        private class PauseMenuItem
        {
            public int ItemID { get; private set; }
            public bool Selected { get; set; }
            private FadeEffect fadeEffect;
            public float Alpha { get; private set; }
            public cText Text;
            public Vector2 Position;

            public PauseMenuItem(string Text, int ID)
            {
                this.Text = new cText(Text, "DryGood");
                ItemID = ID;
            }

            public void LoadContent(int temp)
            {
                Text.Scale = new Vector2(GameSettings.Instance.Dimensions.X / 1366, GameSettings.Instance.Dimensions.Y / 768);
                Text.LoadContent();
            }

            public void SetPosition()
            {
                Text.Position = new Vector2((GameSettings.Instance.Dimensions.X - Text.SourceRect.Width) / 2, Position.Y);
            }

            public void Update(GameTime gameTime)
            {
                Text.Alpha = Alpha;
                if (Selected == true)
                {
                    if (fadeEffect == null)
                        fadeEffect = new FadeEffect(0.5f, 1.0f, 0.5f);
                    Alpha = fadeEffect.Update(gameTime);
                }
                else
                    Alpha = 1.0f;
            }
            
            public void Draw(SpriteBatch spriteBatch)
            {
                Text.DrawString(spriteBatch);
            }
        }
    }
}
