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
        SprSheetImage[,] Belt;
        SprSheetImage[,] Pants;

        public CharacterCreator()
        {
            charCreation_member = new CharCreation_Members();

            Skincolor = new SprSheetImage[2, charCreation_member.GetList(1, 2).Count];
            Head = new SprSheetImage[2, charCreation_member.GetList(2, 2).Count];
            Belt = new SprSheetImage[2, charCreation_member.GetList(3, 2).Count];
            Pants = new SprSheetImage[2, charCreation_member.GetList(4, 2).Count];

        }

        public void FillArrays()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < charCreation_member.GetList(1, 2).Count; j++)
                {
                    //string 

                    //Skincolor[i,j] = new SprSheetImage();

                }
            }
        }
    }
}
