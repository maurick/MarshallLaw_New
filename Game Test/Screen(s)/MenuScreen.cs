using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Test
{
    public class MenuScreen : Screen
    {
        Image background, sign, poster;
        MenuItem[] menuItems; //The Class MenuItem is at the bottom of the this Code
        Vector2 menuLenght;
        Vector2 menuPosition;
        int currentSelected;
        string[] text = { "Start", "Options", "Exit"};

    public MenuScreen()
        {
            //Create the Images for the Menuscreen
            background = new Image("TitleScreen/background");
            sign = new Image("TitleScreen/woodsign_marshal_law");
            poster = new Image("TitleScreen/gun_poster1280x720");
            menuItems = new MenuItem[text.Length];

            //A Loop for the Text Images
            //Each element has 2 images, one for selected and one for unselected
            //They also have a ID
            for (int i = 0; i < text.Length; i++)
            {
                menuItems[i] = new MenuItem();
                menuItems[i].imageselected = new Image("TitleScreen/menutext_" + (i+1).ToString() + "_selected");
                menuItems[i].imageunselected = new Image("TitleScreen/menutext_" + (i+1).ToString() + "_unselected");
                menuItems[i].ItemID = i;
            }
            //By the default the first selected value is
            menuItems[0].Selected = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            background.LoadContent(0, 0, true, new Vector2( GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f));
            sign.LoadContent(0, 0, false, new Vector2(GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f));
            poster.LoadContent(pos_X: 0,
                               pos_Y: (int)(350 * (GameSettings.Instance.Dimensions.Y / 1080f)),
                               centered: true,
                               scale: new Vector2(GameSettings.Instance.Dimensions.X / 1920f, GameSettings.Instance.Dimensions.Y / 1080f));

            //Maak menu aan en zet op midde van scherm
            Vector2 scale = new Vector2(GameSettings.Instance.Dimensions.X / 1920, GameSettings.Instance.Dimensions.Y / 1080);

            #region
            for (int i = 0; i < text.Length; i++)
            {
                menuItems[i].Position = Vector2.Zero;
                menuItems[i].LoadContent();
                menuLenght.Y += menuItems[i].imageselected.SourceRect.Height;
            }
            menuPosition.Y = poster.Position.Y + (((poster.SourceRect.Height) - menuLenght.Y) / 2);

            float temp = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0)
                {
                    temp += menuItems[i - 1].imageselected.SourceRect.Height;
                    menuItems[i].Position.Y = menuPosition.Y + temp;
                }
                else
                    menuItems[i].Position.Y = menuPosition.Y;

                menuItems[i].SetPosition();
            }
            #endregion
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            for (int i = 0; i < text.Length; i++)
            {
                menuItems[i].imageselected.UnloadContent();
                menuItems[i].imageunselected.UnloadContent();
            }

            background.UnloadContent();
            sign.UnloadContent();
            poster.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < text.Length; i++)
            {
                menuItems[i].Update(gameTime);
            }

            background.Update(gameTime);
            sign.Update(gameTime);
            poster.Update(gameTime);

            if (background.Alpha > 0.0f)
                IsVisible = true;

            //If the down key is selected then move the selected 1 down
            if (InputManager.Instance.KeyPressed(Keys.Down))
            {
                menuItems[currentSelected].Selected = false;
                currentSelected++;
                if (currentSelected == text.Length)
                    currentSelected = 0;
                menuItems[currentSelected].Selected = true;

            }

            //If the up key is selected then move the selected 1 up
            if (InputManager.Instance.KeyPressed(Keys.Up))
            {
                menuItems[currentSelected].Selected = false;
                currentSelected--;
                if (currentSelected == -1)
                    currentSelected = text.Length -1;
                menuItems[currentSelected].Selected = true;
            }

            if (menuItems[currentSelected].ItemID == 0 && InputManager.Instance.KeyPressed(Keys.Enter))
            {
                ScreenManager.Instance.ChangeScreen("CharCreationScreen");
            }

            //If the Exit button is selected and Enter has been pressed exit the game
            if (menuItems[currentSelected].ItemID == 1 && InputManager.Instance.KeyPressed(Keys.Enter))
            {
                ScreenManager.Instance.ChangeScreen("MapTestScreen");
            }

            //If the Exit button is selected and Enter has been pressed exit the game
            if (menuItems[currentSelected].ItemID == 2 && InputManager.Instance.KeyPressed(Keys.Enter))
            {
                GameInstance.ExitGame = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            background.Draw(spriteBatch);
            sign.Draw(spriteBatch);
            poster.Draw(spriteBatch);
            
            for (int i = 0; i < text.Length; i++)
            {
                menuItems[i].Draw(spriteBatch);
            }
        }
    }

    public class MenuItem
    {
        //Fields
        public Vector2 Position;
        public int ItemID;
        public bool Selected;
        public Image imageselected;
        public Image imageunselected;
        FadeEffect fadeEffect;

        public void LoadContent()
        {
            Vector2 scale = new Vector2(GameSettings.Instance.Dimensions.X / 1920, GameSettings.Instance.Dimensions.Y / 1080);

            imageselected.LoadContent( 0, (int)Position.Y, true, scale);
            imageunselected.LoadContent( 0, (int)Position.Y, true, scale);
        }

        public void UnloadContent()
        {
            imageselected.UnloadContent();
            imageunselected.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (Selected == true)
            {
                if (fadeEffect == null)
                    fadeEffect = new FadeEffect(0.5f, 1.0f, 0.5f);
                imageselected.Alpha = fadeEffect.Update(gameTime);
            }
            else
            {
                imageselected.Alpha = 1.0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            imageunselected.Draw(spriteBatch);
            if (Selected)
                imageselected.Draw(spriteBatch);

        }

        public void SetPosition()
        {
            
            imageselected.Position =  new Vector2(imageselected.Position.X, Position.Y);
            imageunselected.Position = new Vector2(imageunselected.Position.X, Position.Y);
        }

    }
}
