﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class CharacterCreator
    {
        CharCreation_Members charCreation_member;

        SprSheetImage[,] Skincolor;
        SprSheetImage[,] Head;
        SprSheetImage[,] Shirt;
        SprSheetImage[,] Belt;
        SprSheetImage[,] Pants;

        //public int[] curCharactersettings = { 0, 0, 0, 0, 0 };// 1.Gender 2.Skincolor 3.Head 4.Belt 5.Pants
        //int[] prevCharactersettings = { 0, 0, 0, 0, 0 };// 1.Gender 2.Skincolor 3.Head 4.Belt 5.Pants

        public int[] curAppearencesettings = { 0, 0 };// 1.Gender 2.Skincolor
        public int[] prevAppearencesettings = { 0, 0 };// 1.Gender 2.Skincolor

        public int[] curClothessettings = { 0, 0, 0, 0 };// 1.Head 2.Shirt 3.Belt 4.Pants
        public int[] prevClothessettings = { 0, 0, 0, 0 };//  1.Head 2.Shirt 3.Belt 4.Pants

        public CharacterCreator()
        {
            charCreation_member = new CharCreation_Members();

            Skincolor = new SprSheetImage[2, charCreation_member.GetList(1, 1).Count];
            Head = new SprSheetImage[2, charCreation_member.GetList(0, 2).Count];
            Shirt = new SprSheetImage[2, charCreation_member.GetList(1, 2).Count];
            Belt = new SprSheetImage[2, charCreation_member.GetList(2, 2).Count];
            Pants = new SprSheetImage[2, charCreation_member.GetList(3, 2).Count];

            FillArrays();

        }

        public void LoadContent()
        {
            Vector2 position = new Vector2(
                x: 950 * (GameSettings.Instance.Dimensions.X / 1366), 
                y: 300 * (GameSettings.Instance.Dimensions.Y / 768)
            );

            Vector2 scale = new Vector2(4.0f, 4.0f);

            #region "LoadContent foreach SprSheetImage"
            foreach (var sprSheetImage in Skincolor)
            {
                sprSheetImage.LoadContent(position, scale);
                sprSheetImage.Real_Scale = scale;
            }

            foreach (var sprSheetImage in Head)
            {
                sprSheetImage.LoadContent(position, scale);
                sprSheetImage.Real_Scale = scale;
            }

            foreach (var sprSheetImage in Shirt)
            {
                sprSheetImage.LoadContent(position, scale);
                sprSheetImage.Real_Scale = scale;
            }

            foreach (var sprSheetImage in Belt)
            {
                sprSheetImage.LoadContent(position, scale);
                sprSheetImage.Real_Scale = scale;
            }

            foreach (var sprSheetImage in Pants)
            {
                sprSheetImage.LoadContent(position, scale);
                sprSheetImage.Real_Scale = scale;
            }
            #endregion
        }

        public void UnloadContent()
        {
            #region "UnloadContent foreach SprSheetImage"
            foreach (var sprSheetImage in Skincolor)
            {
                sprSheetImage.UnloadContent();
            }

            foreach (var sprSheetImage in Head)
            {
                sprSheetImage.UnloadContent();
            }

            foreach (var sprSheetImage in Shirt)
            {
                sprSheetImage.UnloadContent();
            }

            foreach (var sprSheetImage in Belt)
            {
                sprSheetImage.UnloadContent();
            }

            foreach (var sprSheetImage in Pants)
            {
                sprSheetImage.UnloadContent();
            }
            #endregion
        }

        public void Update(GameTime gameTime)
        {
            #region "Update foreach SprSheetImage"
            foreach (var sprSheetImage in Skincolor)
            {
                sprSheetImage.Update(gameTime);
            }

            foreach (var sprSheetImage in Head)
            {
                sprSheetImage.Update(gameTime);
            }

            foreach (var sprSheetImage in Shirt)
            {
                sprSheetImage.Update(gameTime);
            }

            foreach (var sprSheetImage in Belt)
            {
                sprSheetImage.Update(gameTime);
            }

            foreach (var sprSheetImage in Pants)
            {
                sprSheetImage.Update(gameTime);
            }
            #endregion


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int gender = curAppearencesettings[0];

            Skincolor[ gender, curAppearencesettings[1]].Draw(spriteBatch);
            Shirt[gender, curClothessettings[1]].Draw(spriteBatch);
            Head[gender, curClothessettings[0]].Draw(spriteBatch);
            Belt[gender, curClothessettings[2]].Draw(spriteBatch);
            Pants[gender, curClothessettings[3]].Draw(spriteBatch);

        }

        public void FillArrays()
        {
            string gender = "";

            for (int i = 0; i < 2; i++)
            {
                if(i == 0)
                    gender = "Male";
                else
                    gender = "Female";

                #region "SkinColor"
                for (int j = 0; j < charCreation_member.GetList(1, 1).Count; j++)
                {
                    string spriteName = charCreation_member.GetString(1, 1, j);

                    Skincolor[i,j] = new SprSheetImage("CharacterSprites/" + gender + "/SkinColor/" + spriteName);
                }
                #endregion

                #region "Head"
                for (int j = 0; j < charCreation_member.GetList(0, 2).Count; j++)
                {
                    string spriteName = charCreation_member.GetString(2, 0, j);

                    Head[i, j] = new SprSheetImage("CharacterSprites/" + gender + "/Head/" + spriteName);
                }
                #endregion

                #region "Shirt"
                for (int j = 0; j < charCreation_member.GetList(1, 2).Count; j++)
                {
                    string spriteName = charCreation_member.GetString(2, 1, j);

                    Shirt[i, j] = new SprSheetImage("CharacterSprites/" + gender + "/Shirt/" + spriteName);
                }
                #endregion

                #region "Belt"
                for (int j = 0; j < charCreation_member.GetList(2, 2).Count; j++)
                {
                    string spriteName = charCreation_member.GetString(2, 2, j);

                    Belt[i, j] = new SprSheetImage("CharacterSprites/" + gender + "/Belt/" + spriteName);
                }
                #endregion

                #region "Pants"
                for (int j = 0; j < charCreation_member.GetList(3, 2).Count; j++)
                {
                    string spriteName = charCreation_member.GetString(2, 3, j);

                    Pants[i, j] = new SprSheetImage("CharacterSprites/" + gender + "/Pants/" + spriteName);
                }
                #endregion
            }
        }
    }
}
