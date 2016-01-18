using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class CharacterInfo
    {
        public bool NotFound;
        int ControllerID;

        public string Name { get; private set; }
        public int Gender { get; private set; }
        public int Skincolor { get; private set; }

        public CharacterInfo(int controllerID)
        {
            ControllerID = controllerID;
        }
        
        public void SetCharacterInfo(string name, int gender, int skincolor)
        {
            this.Name = name;
            this.Gender = gender;
            this.Skincolor = skincolor;
        }

    }
}
