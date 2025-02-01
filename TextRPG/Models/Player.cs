using System;
using System.Collections;
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

        public float ItemPow { get; set; }
        public float ItemDef { get; set; }

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

            ItemPow = 0f;
            ItemDef = 0f;
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

        //장비 착용 메서드
        public void UseItem(float pow, float def)
        {
            ItemPow += pow;
            ItemDef += def;

            power += pow;
            defense += def;
        }
        //장비 해제 메서드
        public void DisuseItem(float pow, float def)
        {
            ItemPow -= pow;
            ItemDef -= def;

            power -= pow;
            defense -= def;
        }
    }
}
