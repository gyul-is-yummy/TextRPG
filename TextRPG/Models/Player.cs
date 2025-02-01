using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG.Models
{
    public class Player : Character
    {
        public int Level { get; set; }
        public string Job { get; set; }

        private int gold;
        public int Gold
        {
            get { return gold; }
            set
            {
                if (value < 0)
                {

                }
                else
                {
                    gold = value;
                }

            }
        }

        public int ItemPow { get; set; }
        public int ItemDef { get; set; }


        public Player()
        {
            name = "플레이어";
            power = 1f;
            defense = 1f;
            hp = 1f;
            speed = 1f;
            Level = 1;
            Job = "직업";
            gold = 1000;

            ItemPow = 0;
            ItemDef = 0;
        }

        public bool BuyItem(int price)
        {
            if (price > gold)
            {
                return false;
            }
            else
            {
                Gold -= price;
                return true;
            }

        }


        //일어나서 이 메서드들 수정하기
        //Use/Disuse가 아니라 Weapon/Armor로 분류해야할...듯?
        //일단 일어나고 다시 생각해보기
        public void UseItem(float stats, ItemType type)
        {
            if (type == ItemType.Weapon)
            {
                power += stats;
            }
            else if (type == ItemType.Armor)
            {
                defense += stats;
            }
        }

        public void DisuseItem(float stats, ItemType type)
        {
            if (type == ItemType.Weapon)
            {
                power -= stats;
            }
            else if (type == ItemType.Armor)
            {
                defense -= stats;
            }
        }
    }
}
