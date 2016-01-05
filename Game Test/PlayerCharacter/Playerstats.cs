using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class Playerstats
    {
        //Fields
        private string name;
        private bool gender;
        private int totalHP;
        private int totalEnergy;
        private int totalStamina;

        private int gold;
        private int xP;
        private int lvl;
        private int currentHP;
        private int currentEnergy;
        private int currentStamina;

        private List<Perks> playerPerks;
        private List<Items> inventory;

        //properties
        public string Name {  get { return this.name; } }
        public bool Gender { get { return this.gender; } }
        public int TotalHP { get { return this.totalHP; } }
        public int TotalEnergy { get { return this.totalEnergy; } }
        public int TotalStamina { get { return this.totalStamina; } }

        public int Gold { get { return this.gold; } set { this.gold = value; } }
        public int XP { get { return this.xP; } set { this.xP = value; } }
        public int Lvl { get { return this.lvl; } set { this.lvl = value; } }
        public int CurrentHP { get { return this.currentHP; } set { this.currentHP = value; } }
        public int CurrentEnergy { get { return this.currentEnergy; } set { this.currentEnergy = value; } }
        public int CurrentStamina { get { return this.currentStamina; } set { this.currentStamina = value; } }

        //Constructor
        public Playerstats(string name, bool gender, Perks startperk, int X, int Y)
        {
            this.name = name;
            this.gender = gender;
            this.totalHP = 1; //Default Value
            this.totalEnergy = 1; //Default Value
            this.totalStamina = 1; //Default Value
            this.gold = 1; //Default Value
            this.xP = 0; //Default Value
            this.lvl = 1; //Default Value
            this.playerPerks = new List<Perks> {startperk };
            this.inventory = new List<Items> { };

            this.currentHP = this.totalHP;
            this.currentEnergy = this.totalEnergy;
            this.currentStamina = this.totalStamina;
        }

        //methods
        public Items InventoryItem(int index)
        {
            return inventory[index];
        }

        public List<Items> Inventory()
        {
            return inventory;
        }

        public Perks PlayerPerk(int index)
        {
            return playerPerks[index];
        }

        public List<Perks> PlayerPerks()
        {
            return playerPerks;
        }


        public void XPcheck()
        {
            if (xP - lvl*100 == 100)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            totalEnergy++;
            totalHP++;
            totalStamina++;
            lvl++;
            if (lvl % 2 == 0)
            {
                //Choose Perk
            }
        }
    }
}
