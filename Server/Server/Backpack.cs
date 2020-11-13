using System.Collections.Generic;

namespace Server
{
    internal class Backpack
    {
        public int Bullets { set; get; }
        public List<Item> Items { set; get; }
        public Weapon SpareWeapon { set; get; }
    }
}