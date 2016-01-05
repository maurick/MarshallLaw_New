using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class Control1
    {
        Image background, field_active, mainbuttonup, mainbuttonmiddle, mainbuttondown, mainbuttonup_pressed, mainbuttondown_pressed;
        cText buttonleft, buttonright;

        public enum selection
        {
            buttonup, buttonmiddle, buttondown, fieldactive, arrow_left, arrow_right, buttonleft, buttonright
        };

        int numberControlFields;
        public int CurrentNumberControlItems;
        public int CurrentActiveField;
        public int CurrentActiveItem;
        public selection currentSelectedMainControl;
        public selection currentSelectedItemControl;
        public bool LeftItemSelected;

        public Control1(int numFields, int currentNumItems)
        {

            #region "Create Instances of all the Images"
            background = new Image("OptionsScreen/poster_background");
            field_active = new Image("OptionsScreen/field_active");
            mainbuttonup = new Image("OptionsScreen/buttonup_selected");
            mainbuttonmiddle = new Image("OptionsScreen/buttonmiddel_selected");
            mainbuttondown = new Image("OptionsScreen/buttondown_selected");
            mainbuttonup_pressed = new Image("OptionsScreen/buttonup_selected_pressed");
            mainbuttondown_pressed = new Image("OptionsScreen/buttondown_selected_pressed");
            buttonleft = new cText("Main Menu", "DryGood");
            buttonright = new cText("Apply Changes", "DryGood");
            #endregion

            this.numberControlFields = numFields;
            this.CurrentNumberControlItems = currentNumItems;
            currentSelectedMainControl = selection.buttonmiddle;
            currentSelectedItemControl = selection.arrow_left;


            int[] Fields = new int[numFields];
            CurrentActiveField = 0;

        }

        public virtual void LoadContent()
        {
            #region "LoadContent for the Images, and position them to the window dimensions"
            background.LoadContent(
                                   pos_X: 0,
                                   pos_Y: 0,
                                   centered: true,
                                   scale: new Vector2(GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f)
                                  );
            field_active.LoadContent(
                                     pos_X: (int)(573 * GameSettings.Instance.Dimensions.X / 1920),
                                     pos_Y: 0,
                                     centered: true,
                                     scale: new Vector2(GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f)
                                    );
            mainbuttonup.LoadContent(
                                     pos_X: (int)(214 * GameSettings.Instance.Dimensions.X / 1920),
                                     pos_Y: (int)(174 * GameSettings.Instance.Dimensions.Y / 1080),
                                     centered: true,
                                     scale: new Vector2(GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f)
                                    );
            mainbuttonup_pressed.LoadContent(
                                     pos_X: (int)(214 * GameSettings.Instance.Dimensions.X / 1920),
                                     pos_Y: (int)(174 * GameSettings.Instance.Dimensions.Y / 1080),
                                     centered: true,
                                     scale: new Vector2(GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f)
                                    );
            mainbuttonmiddle.LoadContent(
                                     pos_X: (int)(214 * GameSettings.Instance.Dimensions.X / 1920),
                                     pos_Y: (int)(443 * GameSettings.Instance.Dimensions.Y / 1080),
                                     centered: true,
                                     scale: new Vector2(GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f)
                                    );
            mainbuttondown.LoadContent(
                                     pos_X: (int)(214 * GameSettings.Instance.Dimensions.X / 1920),
                                     pos_Y: (int)(802 * GameSettings.Instance.Dimensions.Y / 1080),
                                     centered: true,
                                     scale: new Vector2(GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f)
                                    );
            mainbuttondown_pressed.LoadContent(
                                     pos_X: (int)(214 * GameSettings.Instance.Dimensions.X / 1920),
                                     pos_Y: (int)(802 * GameSettings.Instance.Dimensions.Y / 1080),
                                     centered: true,
                                     scale: new Vector2(GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f)
                                    );

            #endregion

            Vector2 textScale = new Vector2(GameSettings.Instance.Dimensions.X / (3200 / 1.2f), GameSettings.Instance.Dimensions.Y / (1800 / 1.2f));

            buttonleft.LoadContent();
            buttonright.LoadContent();

            buttonleft.Scale = textScale;
            buttonright.Scale = textScale;

            float x_scale = (GameSettings.Instance.Dimensions.X / 1920);

            buttonleft.Position = new Vector2(610 * x_scale, 800 * x_scale);
            buttonright.Position = new Vector2(1360 * x_scale, 800 * x_scale);


        }
        public virtual void UnloadContent()
        {
            background.UnloadContent();
            field_active.UnloadContent();

            mainbuttonup.UnloadContent();
            mainbuttonup_pressed.UnloadContent();
            mainbuttonmiddle.UnloadContent();
            mainbuttondown.UnloadContent();
            mainbuttondown_pressed.UnloadContent();

            buttonleft.UnloadContent();
            buttonright.UnloadContent();

        }

        public virtual void Update(GameTime gameTime)
        {
            background.Update(gameTime);

            if (currentSelectedMainControl != selection.fieldactive)
            {
                currentSelectedItemControl = selection.arrow_left;
                CurrentActiveItem = 0;
            }   
            
            #region "Seleced Update"
            switch (currentSelectedMainControl)
            {
                case selection.buttonup:
                    mainbuttonup.Update(gameTime);
                    break;
                case selection.buttonmiddle:
                    mainbuttonmiddle.Update(gameTime);
                    break;
                case selection.buttondown:
                    mainbuttondown.Update(gameTime);
                    break;
                case selection.fieldactive:
                    field_active.Update(gameTime);
                    break;
                case selection.buttonleft:
                    buttonleft.Update(gameTime);
                    break;
                case selection.buttonright:
                    buttonright.Update(gameTime);
                    break;
            }
            #endregion

            #region "Navigation of the control"
            if(currentSelectedMainControl == selection.fieldactive)
            {
                if (InputManager.Instance.KeyPressed(Keys.Left))
                {
                    if (currentSelectedItemControl == selection.arrow_right)
                        currentSelectedItemControl = selection.arrow_left;
                    else if (currentSelectedItemControl == selection.arrow_left)
                        currentSelectedMainControl = selection.buttonup;
                    else if (currentSelectedItemControl == selection.buttonleft)
                        currentSelectedMainControl = selection.buttondown;
                    else if (currentSelectedItemControl == selection.buttonright)
                        currentSelectedItemControl = selection.buttonleft;
                }
                if (InputManager.Instance.KeyPressed(Keys.Right))
                {
                    if (currentSelectedItemControl == selection.arrow_right)
                        currentSelectedItemControl = selection.arrow_left;
                    else if (currentSelectedItemControl == selection.arrow_left)
                        currentSelectedItemControl = selection.arrow_right;
                    else if (currentSelectedItemControl == selection.buttonleft)
                        currentSelectedItemControl = selection.buttonright;
                }
                if (InputManager.Instance.KeyPressed(Keys.Down))
                {
                    if (CurrentActiveItem + 1 >= CurrentNumberControlItems)
                    {
                        currentSelectedItemControl = selection.buttonleft;
                        CurrentActiveItem = 10;
                    }
                    else
                    {
                        CurrentActiveItem++;
                        if (currentSelectedItemControl == selection.buttonleft)
                            currentSelectedItemControl = selection.arrow_left;
                        if (currentSelectedItemControl == selection.buttonright)
                            currentSelectedItemControl = selection.arrow_right;
                    }
                }
                if (InputManager.Instance.KeyPressed(Keys.Up))
                {
                    if (CurrentActiveItem == 10)
                    {
                        CurrentActiveItem = CurrentNumberControlItems - 1;
                        if (currentSelectedItemControl == selection.buttonleft)
                            currentSelectedItemControl = selection.arrow_left;
                        if (currentSelectedItemControl == selection.buttonright)
                            currentSelectedItemControl = selection.arrow_right;
                    }
                    else
                        CurrentActiveItem--;
                }
            }
            else
            {
                if (InputManager.Instance.KeyPressed(Keys.Up))
                {
                    if (currentSelectedMainControl == selection.buttonmiddle)
                        currentSelectedMainControl = selection.buttonup;
                    else if (currentSelectedMainControl == selection.buttondown)
                        currentSelectedMainControl = selection.buttonup;
                }

                if (InputManager.Instance.KeyPressed(Keys.Down))
                {
                    if (currentSelectedMainControl == selection.buttonmiddle)
                        currentSelectedMainControl = selection.buttondown;
                    else if (currentSelectedMainControl == selection.buttonup)
                        currentSelectedMainControl = selection.buttondown;
                }
                if (InputManager.Instance.KeyPressed(Keys.Right))
                {
                    currentSelectedMainControl = selection.fieldactive;
                }
            }
            #endregion


            #region "Actions"
            if (InputManager.Instance.KeyPressed(Keys.Enter))
            {
                switch(currentSelectedMainControl)
                {
                    case selection.buttondown:
                        if (CurrentActiveField == numberControlFields - 1)
                            CurrentActiveField = 0;
                        else
                            CurrentActiveField++;
                        break;
                    case selection.buttonup:
                        if (CurrentActiveField == 0)
                            CurrentActiveField = numberControlFields - 1;
                        else 
                            CurrentActiveField--;
                        break;
                }
            }

            #endregion
            //mainbuttonup_pressed.Update(gameTime);
            //mainbuttondown_pressed.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            switch(currentSelectedMainControl)
            {
                case selection.buttonup:
                    mainbuttonup.Draw(spriteBatch);
                    break;
                case selection.buttondown:
                    mainbuttondown.Draw(spriteBatch);
                    break;
                case selection.fieldactive:
                    field_active.Draw(spriteBatch);
                    break;
            }

            mainbuttonmiddle.Draw(spriteBatch);

            if(currentSelectedItemControl == selection.buttonleft && currentSelectedMainControl == selection.fieldactive)
                buttonleft.Color = Color.White;
            else
                buttonleft.Color = Color.Black;

            if (currentSelectedItemControl == selection.buttonright && currentSelectedMainControl == selection.fieldactive)
                buttonright.Color = Color.White;
            else
                buttonright.Color = Color.Black;


            buttonleft.DrawString(spriteBatch);
            buttonright.DrawString(spriteBatch);
             
            //mainbuttonup_pressed.Draw(spriteBatch);
            //mainbuttondown_pressed.Draw(spriteBatch);

        }

        public void AnimationDown(GameTime gameTime, bool downPressed)
        {
            if (downPressed)
            {

            }
            else
            {

            }
        }
    }
}
