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
        List<string> empty0 = new List<string>();
        #endregion

        #region "Field 1"
        List<string> attributes = new List<string>();
        List<string> empty1 = new List<string>();
        #endregion

        #region "Field 2"
        List<string> appearance = new List<string>();
        List<string> skincolors = new List<string>();
        List<string> gender = new List<string>();
        List<string> head = new List<string>();
        List<string> belt = new List<string>();
        List<string> pants = new List<string>();
        #endregion

        public void Create_Lists()
        {
            #region "FieldID 2"
            string[] stringAppearance = { "Gender", "Skincolor", "Head", "Belt", "Pants" };

            string[] stringSkincolors = { "Light", "Tanned", "Tanned2", "Dark", "Dark2" };
            string[] stringGender = { "Male", "Female"};
            string[] stringHead = { "Red Bandana", "Leather Cap", "Chain Hat", "Coth Hood" };
            string[] stringBelt = { "Leather", "Cloth"};
            string[] stringPants = { "Red", "Magenta", "White", "Teal" };

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
            #endregion

            #region "FieldID 1"
            string[] stringAttributes = { "empty, uncompleted field" };

            string[] stringEmpty = { "Empty" };

            for (int i = 0; i < stringAttributes.Length; i++)
            {
                attributes.Add(stringAttributes[i]);
            }
            for (int i = 0; i < stringEmpty.Length; i++)
            {
                empty1.Add(stringEmpty[i]);
            }
            #endregion

            #region "FieldID 0"
            string[] stringCharacter = { "Name:" };

            for (int i = 0; i < stringCharacter.Length; i++)
            {
                character.Add(stringCharacter[i]);
            }
            for (int i = 0; i < stringEmpty.Length; i++)
            {
                empty0.Add(stringEmpty[i]);
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
                            return empty0;
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

                #region "Field 1"
                case 1:
                    switch (itemID)
                    {
                        case 0:
                            return empty1;
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
                            return attributes;
                    }
                    return null;
                #endregion

                #region "Field 2"
                case 2:
                    switch (itemID)
                    {
                        case 0:
                            return gender;
                        case 1:
                            return skincolors;
                        case 2:
                            return head;
                        case 3:
                            return belt;
                        case 4:
                            return pants;
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

        public string GetString(int itemID, int index)
        {
            switch(itemID)
            {
                case 0:
                    return gender[index];
                case 1:
                    return skincolors[index];
                case 2:
                    return head[index];
                case 3:
                    return belt[index];
                case 4:
                    return pants[index];
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
    }
}
