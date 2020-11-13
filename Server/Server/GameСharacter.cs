using System.Collections.Generic;

namespace Server
{
    internal class GameСharacter
    {
        public Backpack Backpack { set; get; }
        public int Health { set; get; }
        public int Energy { set; get; }
        public List<Perk> Perks { set; get; }
        public string State { set; get; }
        public Weapon EquipmentWeapon { set; get; }
    }
}