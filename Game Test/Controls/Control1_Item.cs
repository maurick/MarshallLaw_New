using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class Control1_Item
    {
        public int ItemID { get; private set; }
        public Vector2 Dimensions;
        private Image arrow_left, arrow_right;
        private cText itemtitle;
        public cText itemsetting;
        public int fieldID;
        public int currentIndex;
        public int maxIndex;
        float tempPosition;

        FadeEffect fadeeffect;
        Vector2 textScale;
        float textPos_Y;
        float imagePos_Y;
        float x_scale;
        Vector2 imageScale;
        public bool IsSelected { get; set; }
        public bool LeftIsSelected;

        public enum selection
        {
            arrowleft, arrowright
        }

        public selection currentSelected;

        public Control1_Item(int itemID, string itemname, string itemsetting, int fieldID, int maxindex)
        {
            this.fieldID = fieldID;
            ItemID = itemID;
            this.itemtitle = new cText(itemname + ":", "DryGood");
            this.itemsetting = new cText(itemsetting, "DryGood");
            arrow_left = new Image("OptionsScreen/arrow_left");
            arrow_right = new Image("OptionsScreen/arrow_right");
            arrow_left.Color = Color.Black;
            arrow_right.Color = Color.Black;
            fadeeffect = new FadeEffect(1.5f, 1.0f, 0.3f);
            currentSelected = selection.arrowleft;
            currentIndex = 0;
            this.maxIndex = maxindex;
        }

        public void LoadContent()
        {
            textScale = new Vector2(GameSettings.Instance.Dimensions.X / (3200 / 1.2f), GameSettings.Instance.Dimensions.Y / (1800 / 1.2f));
            imageScale = new Vector2(GameSettings.Instance.Dimensions.X / 2732f, GameSettings.Instance.Dimensions.Y / 1536f);

            #region "Position the Text on the Y axis"
            textPos_Y = 400 + ItemID*110;
            imagePos_Y = 420 + ItemID*110;

            textPos_Y = textPos_Y * (GameSettings.Instance.Dimensions.Y / 1920);
            imagePos_Y = imagePos_Y * (GameSettings.Instance.Dimensions.Y / 1920);
            #endregion

            #region "Load Content"
            itemtitle.LoadContent();
            itemsetting.LoadContent();

            arrow_right.LoadContent(
                        pos_X: 0,
                        pos_Y: imagePos_Y,
                        centered: false,
                        scale: imageScale
                        );
            arrow_left.LoadContent(
                        pos_X: 0,
                        pos_Y: imagePos_Y,
                        centered: false,
                        scale: imageScale
                        );
            #endregion

            itemtitle.Scale = textScale;
            itemsetting.Scale = textScale;


            x_scale = (GameSettings.Instance.Dimensions.X / 1920);


            tempPosition = 610 * x_scale;

            itemtitle.Position = new Vector2(tempPosition, textPos_Y);

            tempPosition += (itemtitle.SourceRect.Width * textScale.X) + (10 * x_scale);

            arrow_left.Position = new Vector2(tempPosition, arrow_left.Position.Y);

            tempPosition += (arrow_left.SourceRect.Width) + (10 * x_scale);

            itemsetting.Position = new Vector2(tempPosition, textPos_Y);

            arrow_right.Position = new Vector2(tempPosition + ((itemsetting.SourceRect.Width * textScale.X) + (10 * x_scale)), arrow_right.Position.Y);

        }

        public void UnloadContent()
        {
            itemtitle.UnloadContent();
            itemsetting.UnloadContent();
            arrow_left.UnloadContent();
            arrow_right.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {

            itemtitle.Update(gameTime);
            itemsetting.Update(gameTime);
            arrow_left.Update(gameTime);
            arrow_right.Update(gameTime);

            arrow_right.Position = new Vector2(tempPosition + ((itemsetting.GetTextSize(itemsetting.Text).X) + (10 * x_scale)), arrow_right.Position.Y);

            itemtitle.Color = Color.Black;
            arrow_left.Color = Color.Black;
            arrow_right.Color = Color.Black;

            if (IsSelected)
            {
                var temp = fadeeffect.Update(gameTime);
                itemtitle.Alpha = temp;
                itemsetting.Alpha = temp;
                arrow_left.Alpha = temp;
                arrow_right.Alpha = temp;
                switch (currentSelected)
                {
                    case selection.arrowleft:
                        arrow_left.Color = Color.White;
                        break;
                    case selection.arrowright:
                        arrow_right.Color = Color.White;
                        break;
                }

                if (InputManager.Instance.KeyPressed(Keys.Enter))
                {
                    if (currentSelected == selection.arrowleft)
                    {
                        currentIndex--;
                        if (currentIndex < 0)
                            currentIndex = maxIndex - 1;
                    }
                    if (currentSelected == selection.arrowright)
                    {
                        currentIndex++;
                        if (currentIndex >= maxIndex)
                            currentIndex = 0;
                    }
                }
            }
            else
            {
                itemtitle.Alpha = 1.0f;
                itemsetting.Alpha = 1.0f;
                arrow_left.Alpha = 1.0f;
                arrow_right.Alpha = 1.0f;
                itemtitle.Color = Color.Black;
                arrow_left.Color = Color.Black;
                arrow_right.Color = Color.Black;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            itemtitle.DrawString(spriteBatch);
            itemsetting.DrawString(spriteBatch);
            arrow_left.Draw(spriteBatch);
            arrow_right.Draw(spriteBatch);
        }

        public void ResetSelected()
        {
            currentSelected = selection.arrowleft;
        }

        public void SetSelected(int selected)
        {
            switch(selected)
            {
                case 4:
                    currentSelected = selection.arrowleft;
                    break;
                case 5:
                    currentSelected = selection.arrowright;
                    break;
            }
        }
    }
}
