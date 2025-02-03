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
        public string Name { get; set; } = string.Empty;
        public float Power { get; set; }
        public float Defense { get; set; }
        public float MaxHP { get; private set; }

        private float hp;
        public float Hp
        {
            get { return hp; }
            set
            {
                if (value + hp > MaxHP)
                    hp = MaxHP;
            }
        }
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
            Name = "플레이어";
            Power = 1f;
            Defense = 1f;
            MaxHP = 100f;
            hp = 10f;
            Level = 1;
            Job = "직업";
            gold = 100000;

            ItemPow = 0f;
            ItemDef = 0f;
        }

        //장비 구매 메서드
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

            Power += pow;
            Defense += def;
        }
        //장비 해제 메서드
        public void DisuseItem(float pow, float def)
        {
            ItemPow -= pow;
            ItemDef -= def;

            Power -= pow;
            Defense -= def;
        }


        //플레이어의 소지 골드가 충분한지 확인하는 메서드
        public bool HasEnoughGold(float price)
        {
            return Gold >= price;
        }
    }
}
