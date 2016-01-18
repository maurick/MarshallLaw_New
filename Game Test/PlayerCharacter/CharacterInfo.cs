using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class CharacterInfo
    {
        CharCreation_Members char_member = new CharCreation_Members();
        public bool NotFound;
        int ControllerID;

        public string Name { get; set; }

        public int NameIndex { get; set; }
        public int Gender { get; set; }
        public int Skincolor { get; set; }

        public int Head { get; set; }
        public int Shirt { get; set; }
        public int Belt { get; set; }
        public int Pants { get; set; }

        public CharacterInfo(int controllerID)
        {
            ControllerID = controllerID;
        }
        
        public void SetCharacterInfo(string name, int gender, int skincolor, int head, int shirt, int belt, int pants)
        {
            this.Name = name;
            for (int i = 0; i < char_member.GetList(0, 0).Count; i++)
            {
                if(Name == char_member.GetList(0, 0)[i])
                {
                    NameIndex = i;
                }
            }
            this.Gender = gender;
            this.Skincolor = skincolor;
            this.Head = head;
            this.Shirt = shirt;
            this.Belt = belt;
            this.Pants = pants;
        }

    }
}
