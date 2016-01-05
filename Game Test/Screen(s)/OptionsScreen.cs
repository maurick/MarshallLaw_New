using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Test
{
    public class OptionsScreen : Screen
    {
        Control1 control;
        Options_Members option_member;
        static int numFields = 1;
        static int numItems = 1;
        Control1_Field[] fields = new Control1_Field[numFields];
        Control1_Item[] items = new Control1_Item[numItems];

        //Contructor
        public OptionsScreen()
        {
            option_member = new Options_Members();
            option_member.Create_Lists();

            fields[0] = new Control1_Field( 0, numFields,  "Video", numItems);

            control = new Control1(numFields, numItems);

            items[0] = new Control1_Item( 0, "Resolution", option_member.GetString(1, 0), 3, option_member.GetList(1).Count);
        }


        public override void LoadContent()
        {
            base.LoadContent();
            control.LoadContent();

            foreach (var control_field in fields)
            {
                control_field.LoadContent();
            }

            foreach (var item in items)
            {
                item.LoadContent();
            }

        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            control.UnloadContent();

            foreach (var control_field in fields)
            {
                control_field.UnloadContent();
            }

            foreach (var item in items)
            {
                item.UnloadContent();
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            control.CurrentNumberControlItems = fields[control.CurrentActiveField].maxItems;

            control.Update(gameTime);

            if (control.CurrentActiveItem != 10)
                items[control.CurrentActiveItem].itemsetting.Text = option_member.GetString(1, items[control.CurrentActiveItem].currentIndex);

            foreach (var control_field in fields)
            {
                control_field.Status = -1;
                control_field.SetStatus(control.CurrentActiveField);
                control_field.Update(gameTime);
            }

            foreach (var item in items)
            {
                item.IsSelected = false;
                item.SetSelected((int)control.currentSelectedItemControl);
                item.Update(gameTime);
            }
            if(control.currentSelectedMainControl == Control1.selection.fieldactive)
            {
                items[control.CurrentActiveItem].IsSelected = true;
                items[control.CurrentActiveItem].Update(gameTime);
            }

            //When the Escape key has been pressed exit the game
            if (InputManager.Instance.KeyPressed(Keys.Escape))
            {
                ScreenManager.Instance.ChangeScreen("MenuScreen");
            }

            if (InputManager.Instance.KeyPressed(Keys.I))
            {
                //option_member.ExcuteAction(Options_Members.Actions.changeResolution, items[control.CurrentActiveItem].currentIndex);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            control.Draw(spriteBatch);

            foreach (var control_field in fields)
            {
                control_field.Draw(spriteBatch);
            }

            switch (control.CurrentActiveField)
            {
                case 0:
                    foreach (var item in items)
                    {
                        item.Draw(spriteBatch);
                    }
                    break;
            }
        }
    }
}
