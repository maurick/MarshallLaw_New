using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class CharCreation_Members
    {
        public enum Actions
        {
            gender, skincolor, head, pants
        };

        public CharCreation_Members()
        {
            Create_Lists();
        }

        public void ExcuteAction(Actions action, int index)
        {
            switch(action)
            {
                case Actions.gender:

                    break;
                case Actions.skincolor:

                    break;
                case Actions.head:

                    break;
                case Actions.pants:

                    break;
            }
        }

        #region "Field 0"
        List<string> character = new List<string>();
        List<string> names = new List<string>();
        #endregion

        #region "Field 2"
        List<string> clothes = new List<string>();
        List<string> head = new List<string>();
        List<string> shirtf = new List<string>();
        List<string> shirt = new List<string>();
        List<string> belt = new List<string>();
        List<string> pants = new List<string>();
        #endregion

        #region "Field 1"
        List<string> appearance = new List<string>();
        List<string> skincolors = new List<string>();
        List<string> gender = new List<string>();
        #endregion

        public void Create_Lists()
        {
            string[] stringNames = { "Bob       ", "Kees      ", "Einstein  ", "Geert     " };

            #region "FieldID 1"
            string[] stringAppearance = { "Gender", "Skincolor" };

            string[] stringSkincolors = { "Light", "Tanned", "Tanned2", "Dark", "Dark2" };
            string[] stringGender = { "Male", "Female"};

            for (int i = 0; i < stringAppearance.Length; i++)
            {
                appearance.Add(stringAppearance[i]);
            }
            for (int i = 0; i < stringSkincolors.Length; i++)
            {
                skincolors.Add(stringSkincolors[i]);
            }
            for (int i = 0; i < stringGender.Length; i++)
            {
                gender.Add(stringGender[i]);
            }
            #endregion

            #region "FieldID 2"
            string[] stringClothes = { "Head", "Shirt", "Belt", "Pants" };

            string[] stringHead = { "Red Bandana", "Leather Cap", "Chain Hat", "Cloth Hood" };
            //string[] stringShirt_Female = { "Brown Shirt", "Brown Strap Shirt", "Dark Red Shirt", "Dark Red Strap Shirt", "Fancy White Shirt", "Fancy White Strap Shirt", "Hipster Blue Shirt", "Hipster Blue Strap Shirt" };
            string[] stringShirt= { "Brown Shirt", "Dark Red Shirt", "Fancy White Shirt", "Hipster Blue Shirt",  };
            string[] stringBelt = { "Leather", "Cloth" };
            string[] stringPants = { "Red", "Magenta", "White", "Teal" };

            for (int i = 0; i < stringClothes.Length; i++)
            {
                clothes.Add(stringClothes[i]);
            }
            for (int i = 0; i < stringHead.Length; i++)
            {
                head.Add(stringHead[i]);
            }
            for (int i = 0; i < stringBelt.Length; i++)
            {
                belt.Add(stringBelt[i]);
            }
            for (int i = 0; i < stringPants.Length; i++)
            {
                pants.Add(stringPants[i]);
            }
            //for (int i = 0; i < stringShirt_Female.Length; i++)
            //{
                //shirtf.Add(stringShirt_Female[i]);
            //}
            for (int i = 0; i < stringShirt.Length; i++)
            {
                shirt.Add(stringShirt[i]);
            }
            for (int i = 0; i < stringPants.Length; i++)
            {
                pants.Add(stringPants[i]);
            }
            #endregion

            #region "FieldID 0"
            string[] stringCharacter = { "Name:" };

            for (int i = 0; i < stringCharacter.Length; i++)
            {
                character.Add(stringCharacter[i]);
            }
            for (int i = 0; i < stringNames.Length; i++)
            {
                names.Add(stringNames[i]);
            }
            #endregion
        }

        public List<string> GetList(int itemID, int fieldID)
        {
            switch(fieldID)
            {
                #region "Field 0"
                case 0:
                    switch (itemID)
                    {
                        case 0:
                            return names;
                        case 1:
                            return null;
                        case 2:
                            return null;
                        case 3:
                            return null;
                        case 4:
                            return null;
                        case 5:
                            return null;
                        case 6:
                            return null;
                        case 7:
                            return null;
                        case 8:
                            return null;
                        case 9:
                            return null;
                        case 10:
                            return character;
                    }
                    return null;
                #endregion

                #region "Field 2"
                case 2:
                    switch (itemID)
                    {
                        case 0:
                            return head;
                        case 1:
                            return shirt;
                        case 2:
                            return belt;
                        case 3:
                            return pants;
                        case 4:
                            return null;
                        case 5:
                            return null;
                        case 6:
                            return null;
                        case 7:
                            return null;
                        case 8:
                            return null;
                        case 9:
                            return null;
                        case 10:
                            return clothes;
                    }
                    return null;
                #endregion

                #region "Field 1"
                case 1:
                    switch (itemID)
                    {
                        case 0:
                            return gender;
                        case 1:
                            return skincolors;
                        case 2:
                            return null;
                        case 3:
                            return null;
                        case 4:
                            return null;
                        case 5:
                            return null;
                        case 6:
                            return null;
                        case 7:
                            return null;
                        case 8:
                            return null;
                        case 9:
                            return null;
                        case 10:
                            return appearance;
                    }
                    return null;
                #endregion


            }
            return null;
        }

        public string GetString(int fieldID, int itemID, int index)
        {
            switch(fieldID)
            {
                case 0:
                    switch (itemID)
                    {
                        case 0:
                            return names[index];
                        case 1:
                            return null;
                        case 2:
                            return null;
                        case 3:
                            return null;
                        case 4:
                            return null;
                        case 5:
                            return null;
                        case 6:
                            return null;
                        case 7:
                            return null;
                        case 8:
                            return null;
                        case 9:
                            return null;
                        case 10:
                            return null;
                    }
                    return "error";
                case 2:
                    switch (itemID)
                    {
                        case 0:
                            return head[index];
                        case 1:
                            return shirt[index];
                        case 2:
                            return belt[index];
                        case 3:
                            return pants[index];
                        case 4:
                            return null;
                        case 5:
                            return null;
                        case 6:
                            return null;
                        case 7:
                            return null;
                        case 8:
                            return null;
                        case 9:
                            return null;
                        case 10:
                            return null;
                    }
                    return "error";
                case 1:
                    switch (itemID)
                    {
                        case 0:
                            return gender[index];
                        case 1:
                            return skincolors[index];
                        case 2:
                            return null;
                        case 3:
                            return null;
                        case 4:
                            return null;
                        case 5:
                            return null;
                        case 6:
                            return null;
                        case 7:
                            return null;
                        case 8:
                            return null;
                        case 9:
                            return null;
                        case 10:
                            return null;
                    }
                    return "error";
            }
            return "error";
        }
    }
}
