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

        List<string> appearance = new List<string>();
        List<string> skincolors = new List<string>();
        List<string> gender = new List<string>();
        List<string> head = new List<string>();
        List<string> belt = new List<string>();
        List<string> pants = new List<string>();

        public void Create_Lists()
        {
            #region "FieldID 1"
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

            #region "FieldID 2"

            #endregion
        }

        public List<string> GetList(int itemID, int fieldID)
        {
            switch(fieldID)
            {
                #region "Field 1"
                case 1:
                    switch (itemID)
                    {
                        case 0:
                            return null;
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

                #region "Field 3"
                case 3:
                    switch (itemID)
                    {
                        case 0:
                            return null;
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
                    return null;
                    #endregion
            }
            return null;
        }

        public string GetString(int ID, int index)
        {
            switch(ID)
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
