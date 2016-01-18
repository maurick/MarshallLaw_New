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
        #region "Control"
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
        #endregion

        bool refresh;
        CharacterCreator characterCreator;



        //Contructor
        public CharCreationScreen()
        {
            charCreatin_member = new CharCreation_Members();

            numitems1 = charCreatin_member.GetList(10, 0).Count;
            numitems2 = charCreatin_member.GetList(10, 2).Count;
            numitems3 = charCreatin_member.GetList(10, 1).Count;

            fields[0] = new Control1_Field(0, numFields, "Character", numitems1);
            fields[2] = new Control1_Field(2, numFields, "Clothes", numitems2);
            fields[1] = new Control1_Field(1, numFields, "Appearance", numitems3);

            control = new Control1(numFields, numitems1);

            for (int i = 0; i < numitems1; i++)
            {
                items1[i] = new Control1_Item(
                    itemID: i,
                    itemname: charCreatin_member.GetList(10, 0)[i],
                    itemsetting: charCreatin_member.GetString(0, i, 0),
                    fieldID: 0,
                    maxindex: charCreatin_member.GetList(i, 0).Count
                    );
            }

            for (int i = 0; i < numitems2; i++)
            {
                items2[i] = new Control1_Item(
                    itemID: i,
                    itemname: charCreatin_member.GetList(10, 2)[i],
                    itemsetting: charCreatin_member.GetString(2, i, 0),
                    fieldID: 2,
                    maxindex: charCreatin_member.GetList(i, 2).Count
                    );
            }
            for (int i = 0; i < numitems3; i++)
            {
                items3[i] = new Control1_Item(
                    itemID: i,
                    itemname: charCreatin_member.GetList(10, 1)[i],
                    itemsetting: charCreatin_member.GetString(1, i, 0),
                    fieldID: 1,
                    maxindex: charCreatin_member.GetList(i, 1).Count
                    );
            }

            characterCreator = new CharacterCreator();

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
            foreach (var item in items2)
            {
                if (item != null)
                    item.LoadContent();
            }
            foreach (var item in items3)
            {
                if (item != null)
                    item.LoadContent();
            }

            characterCreator.LoadContent();
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
            foreach (var item in items2)
            {
                if (item != null)
                    item.UnloadContent();
            }
            foreach (var item in items3)
            {
                if (item != null)
                    item.UnloadContent();
            }

            characterCreator.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            control.CurrentNumberControlItems = fields[control.CurrentActiveField].maxItems;

            control.Update(gameTime);

            if (control.CurrentActiveField == 0)
            {
                for (int i = 0; i < items1.Length; i++)
                {
                    if (i != 10 && items1[i] != null)
                    {
                        items1[i].itemsetting.Text = charCreatin_member.GetString(0, i, items1[i].currentIndex);
                    }
                }
            }
            else if (control.CurrentActiveField == 2)
            {
                for (int i = 0; i < items2.Length; i++)
                {
                    if (i != 10 && items2[i] != null)
                    {
                        items2[i].itemsetting.Text = charCreatin_member.GetString(2, i, items2[i].currentIndex);
                    }
                }
            }
            else if (control.CurrentActiveField == 1)
            {
                for (int i = 0; i < items3.Length; i++)
                {
                    if (i != 10 && items3[i] != null)
                    {
                        items3[i].itemsetting.Text = charCreatin_member.GetString(1, i, items3[i].currentIndex);
                    }
                }
            }

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
            foreach (var item in items2)
            {
                if (item != null)
                {
                    item.IsSelected = false;
                    item.SetSelected((int)control.currentSelectedItemControl);
                    item.Update(gameTime);
                }
            }
            foreach (var item in items3)
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
                    if (control.CurrentActiveField == 0)
                    {
                        items1[control.CurrentActiveItem].IsSelected = true;
                        items1[control.CurrentActiveItem].Update(gameTime);
                    }
                    if (control.CurrentActiveField == 2)
                    {
                        items2[control.CurrentActiveItem].IsSelected = true;
                        items2[control.CurrentActiveItem].Update(gameTime);
                    }
                    if (control.CurrentActiveField == 1)
                    {
                        items3[control.CurrentActiveItem].IsSelected = true;
                        items3[control.CurrentActiveItem].Update(gameTime);
                    }
                }
            }

            if (control.CurrentActiveField == 1 || control.CurrentActiveField == 2)
            {
                for (int i = 0; i < characterCreator.curAppearencesettings.Length; i++)
                {
                    characterCreator.curAppearencesettings[i] = items3[i].currentIndex;
                }
                for (int i = 0; i < characterCreator.curClothessettings.Length; i++)
                {
                    characterCreator.curClothessettings[i] = items2[i].currentIndex;
                }
                characterCreator.Update(gameTime);

            }

            if (ScreenManager.Instance.Controllers[0].characterInfo.NotFound)
            {
                ScreenManager.Instance.Controllers[0].characterInfo.NameIndex = items1[0].currentIndex;
                ScreenManager.Instance.Controllers[0].characterInfo.Gender = items3[0].currentIndex;
                ScreenManager.Instance.Controllers[0].characterInfo.Skincolor = items3[1].currentIndex;

                ScreenManager.Instance.Controllers[0].characterInfo.Head = items2[0].currentIndex;
                ScreenManager.Instance.Controllers[0].characterInfo.Shirt = items2[1].currentIndex;
                ScreenManager.Instance.Controllers[0].characterInfo.Belt = items2[2].currentIndex;
                ScreenManager.Instance.Controllers[0].characterInfo.Pants = items2[3].currentIndex;

                ScreenManager.Instance.Controllers[0].characterInfo.NotFound = false;
            }


            if (control.currentSelectedMainControl == Control1.selection.buttoncontinue && (InputManager.Instance.KeyPressed(Keys.Enter) || ScreenManager.Instance.Controllers[0].A_Button(true)))
            {
                ScreenManager.Instance.Controllers[0].characterInfo.Name = charCreatin_member.GetString(0, 0, items1[0].currentIndex);
                ScreenManager.Instance.Controllers[0].characterInfo.Gender = items3[0].currentIndex;
                ScreenManager.Instance.Controllers[0].characterInfo.Skincolor = items3[1].currentIndex;

                ScreenManager.Instance.Controllers[0].characterInfo.Head = items2[0].currentIndex;
                ScreenManager.Instance.Controllers[0].characterInfo.Shirt = items2[1].currentIndex;
                ScreenManager.Instance.Controllers[0].characterInfo.Belt = items2[2].currentIndex;
                ScreenManager.Instance.Controllers[0].characterInfo.Pants = items2[3].currentIndex;

                ScreenManager.Instance.Controllers[0].SaveSettings();

                ScreenManager.Instance.ChangeScreen("MapTestScreen");
            }

            //When the Escape key has been pressed exit the game
            if (InputManager.Instance.KeyPressed(Keys.Escape))
            {
                //ScreenManager.Instance.ChangeScreen("MenuScreen");
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
                    if (item.fieldID == control.CurrentActiveField)
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

            if (control.CurrentActiveField == 1 || control.CurrentActiveField == 2)
                characterCreator.Draw(spriteBatch);
        }
    }
}
