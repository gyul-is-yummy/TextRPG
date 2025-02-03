using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Models
{
    public enum ItemType
    {
        Weapon,
        Armor,
        Accessories
    }

    public enum ItemState
    {
        HaveNot,
        Have,
        Use
    }

    public class Item
    {
        public string Name { get; set; } = string.Empty;
        public string ItemInfo { get; set; } = string.Empty;
        public int Gold { get; set; } = 100;
        public float Power { get; set; } = 0f;
        public float Defense { get; set; } = 0f;
        public int InventoryNum { get; set; } = 0;
        public ItemType Type { get; set; }
        public ItemState State { get; set; } = ItemState.HaveNot;
    }
}
