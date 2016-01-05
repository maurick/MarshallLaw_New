using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class Control1_Field
    {
        private int status;
        private cText title;
        private int FieldID;
        public Vector2 Dimensions;
        private int maxFields;
        public int maxItems;
        public bool IsActive { get; set; }

        public cText Title { get { return title; } }
        public int Status { get { return status; } set { status = value; } }



        /// <summary>
        /// Contructor for a Control Item
        /// </summary>
        /// <param name="">The title of the Item</param>
        public Control1_Field(int FieldID, int maxFields, string title, int numItems)
        {
            this.title = new cText(title, "DryGood");
            this.FieldID = FieldID;
            this.maxFields = maxFields;
            this.maxItems = numItems;
            this.status = 0;
            IsActive = false;
        }

        public void LoadContent()
        {
            title.LoadContent();
            Vector2 textScale = new Vector2(GameSettings.Instance.Dimensions.X / (3200 / 1.25f), GameSettings.Instance.Dimensions.Y / (1800 / 1.25f));
            title.Scale = textScale;
        }

        public void UnloadContent()
        {
            title.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            title.Update(gameTime);
            SetTitlePosition();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if(status >= 1 && status <= 3)
                title.DrawString(spriteBatch);
        }

        public void SetTitlePosition()
        {
            //The scale is nessecary because if the window gets resized the position changes aswell.
           Vector2 scale = new Vector2(GameSettings.Instance.Dimensions.X / 1366, GameSettings.Instance.Dimensions.Y / 768);
            //The middle x coordiantes get calculated here, it takes the width of the text and the width of the control bar,
            //and divides those by 2 to calculate the middle
            float x_position = (((205 * scale.X) - title.GetTextSize(title.Text).X) / 2);

            switch (status)
            {
                case 1:
                    title.Position = new Vector2((177 * scale.X) + x_position, 231 * scale.Y);
                    break;
                case 2:
                    title.Position = new Vector2((177 * scale.X) + x_position, 356 * scale.Y);
                    break;
                case 3:
                    title.Position = new Vector2((177 * scale.X) + x_position, 481 * scale.Y);
                    break;
            }
        }


        public void SetStatus(int currentActive)
        {
            if (currentActive == FieldID)
                status = 2;
            else if (currentActive - 1 == FieldID)
                status = 1;
            else if (currentActive - 2 == FieldID)
                status = 0;
            else if (currentActive + 1 == FieldID)
                status = 3;
            else if (currentActive + 2 == FieldID)
                status = 4;
            if (FieldID == 0 && currentActive == maxFields - 1)
                status = 3;
            if (FieldID == maxFields - 1 && currentActive == 0)
                status = 1;
        }
    }
}
