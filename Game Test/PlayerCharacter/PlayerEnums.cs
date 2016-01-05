using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Test
{
    public class PlayerEnums
    {
        private static PlayerEnums instance;

        public static PlayerEnums Enums
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerEnums();
                }
                return instance;
            }
        }

        /// <summary>
        /// Spell: 0-3:
        /// Spear: 4-7
        /// Walk: 8-11
        /// Slash: 12-15
        /// Shoot: 16-19
        /// Hit: 20
        /// </summary>
        public enum Action
        {
            SpellUp,
            SpellLeft,
            SpellDown,
            SpellRight,
            SpearUp,
            SpearLeft,
            SpearDown,
            SpearRight,
            WalkUp,
            WalkLeft,
            WalkDown,
            WalkRight,
            SlashUp,
            SlashLeft,
            SlashDown,
            SlashRight,
            ShootUp,
            ShootLeft,
            ShootDown,
            ShootRight,
            Hit,
            None
        };

        public enum ActionState
        {
            None,
            Spell = 7,
            Thrust = 8,
            Walk = 9,
            Slash = 6,
            Shoot = 13
        };

        public enum LookDirection
        {
            Up,
            left,
            Down,
            Right
        };
    }
}
