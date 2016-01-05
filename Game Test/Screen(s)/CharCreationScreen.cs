using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Test
{
    class CharCreationScreen : Screen
    {
        Control1 control;
        CharCreation_Members charCreatin_member = new CharCreation_Members();
        static int numFields = 3;

        Control1_Field[] fields = new Control1_Field[numFields];

        Control1_Item[] items1 = new Control1_Item[9];
        Control1_Item[] items2 = new Control1_Item[9];
        Control1_Item[] items3 = new Control1_Item[9];

        static int numitems1;
        static int numitems2;
        static int numitems3;

        //Contructor
        public CharCreationScreen()
        {
            charCreatin_member = new CharCreation_Members();

            numitems1 = charCreatin_member.GetList(10, 2).Count;
            numitems2 = 1;
            numitems3 = 1;

            fields[0] = new Control1_Field( 0, numFields, "Character", numitems3);
            fields[1] = new Control1_Field( 1, numFields, "Attributes", numitems2);
            fields[2] = new Control1_Field( 2, numFields, "Appearance", numitems1);

            control = new Control1(numFields, numitems1);

            for (int i = 0; i < numitems1; i++)
            {
                items1[i] = new Control1_Item(
                    itemID:i,
                    itemname: charCreatin_member.GetList(10, 2)[i], 
                    itemsetting: charCreatin_member.GetString(i, 0),
                    fieldID: 2,
                    maxindex: charCreatin_member.GetList(i, 2).Count
                    );
            }
        }


        public override void LoadContent()
        {
            base.LoadContent();
            control.LoadContent();

            foreach (var control_field in fields)
            {
                control_field.LoadContent();
            }

            foreach (var item in items1)
            {
                if (item != null)
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

            foreach (var item in items1)
            {
                if (item != null)
                    item.UnloadContent();
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            control.CurrentNumberControlItems = fields[control.CurrentActiveField].maxItems;

            control.Update(gameTime);

            if(control.CurrentActiveItem != 10)
                items1[control.CurrentActiveItem].itemsetting.Text = charCreatin_member.GetString(control.CurrentActiveItem, items1[control.CurrentActiveItem].currentIndex);

            foreach (var control_field in fields)
            {
                control_field.Status = -1;
                control_field.SetStatus(control.CurrentActiveField);
                control_field.Update(gameTime);
            }

            foreach (var item in items1)
            {
                if (item != null)
                {
                    item.IsSelected = false;
                    item.SetSelected((int)control.currentSelectedItemControl);
                    item.Update(gameTime);
                }
            }
            if (control.currentSelectedMainControl == Control1.selection.fieldactive)
            {
                if (control.CurrentActiveItem != 10)
                {
                    items1[control.CurrentActiveItem].IsSelected = true;
                    items1[control.CurrentActiveItem].Update(gameTime);
                }

            }

            //When the Escape key has been pressed exit the game
            if (InputManager.Instance.KeyPressed(Keys.Escape))
            {
                ScreenManager.Instance.ChangeScreen("MenuScreen");
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

            foreach (var item in items1)
            {
                if (item != null)
                    if(item.fieldID == control.CurrentActiveField)
                        item.Draw(spriteBatch);
            }

            foreach (var item in items2)
            {
                if (item != null)
                    if (item.fieldID == control.CurrentActiveField)
                        item.Draw(spriteBatch);
            }

            foreach (var item in items3)
            {
                if (item != null)
                    if (item.fieldID == control.CurrentActiveField)
                        item.Draw(spriteBatch);
            }

        }
    }
}
