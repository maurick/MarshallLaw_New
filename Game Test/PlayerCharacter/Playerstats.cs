using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class Playerstats
    {
        //Fields
        private static Playerstats instance;

        /// <summary>
        /// Maken van een Instance, Deze Class is een Singleton class
        /// </summary>
        public static Playerstats Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Playerstats();
                }
                return instance;
            }
        }

        #region"Player1"
        public int[] Player1_Appearencesettings;
        public int[] Player1_curClothessettings;
        #endregion

        #region"Player2"
        public int[] Player2_Appearencesettings;
        public int[] Player2_curClothessettings;
        #endregion

        //Constructor
        private Playerstats()
        {

        }
    }
}
