using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum JobType
{
    Warrior,
    Magician,
    Archer
}


namespace TextRPG.Models
{
    public class Player
    {
        public string[] tempName;
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
                hp = value;

                if (hp > MaxHP)
                {
                    hp = MaxHP;
                }
                else if(hp < 0)
                {
                    hp = 0;
                }
            }
        }
        public int Level { get; set; }

        private int exp;
        public int Exp
        {
            get { return exp; }
            set
            {
                exp = value;
                if (exp >= MaxExp)
                {
                    LevelUp();
                    MaxExp++;
                }
            }
        }

        private int maxExp;
        public int MaxExp
        {
            get { return maxExp; }
            private set { maxExp = value; }
        }

        private int gold;
        public int Gold
        {
            get { return gold; }
            set
            {
                gold = value;

                if (gold < 0)
                {
                    gold = 0;
                }
            }
        }

        public float ItemPow { get; set; }
        public float ItemDef { get; set; }

        private JobType job = JobType.Warrior;
        public JobType Job
        {
            get { return job; }

            set
            {
                //JobType이 설정될 때 JobName과 스탯들도 함께 설정되도록 한다.
                job = value;
                JobName = jobName;
                SetStats();
            }

        }

        private string jobName;
        public string JobName
        {
            get { return jobName; }

            private set
            {
                if (job == JobType.Warrior)
                {
                    jobName = "전사";
                }
                else if (job == JobType.Magician)
                {
                    jobName = "마법사";
                }
                else if (job == JobType.Archer)
                {
                    jobName = "궁수";
                }
            }
        }

        //플레이어 생성자
        public Player()
        {
            Name = "플레이어";
            Power = 1f;
            Defense = 1f;
            MaxHP = 10f;
            Hp = MaxHP;
            Level = 1;
            MaxExp = 1;
            Exp = 0;
            JobName = "직업";
            Gold = 100000;

            ItemPow = 0f;
            ItemDef = 0f;

            tempName = new string[10]{ "올든",
                                       "세드릭",
                                       "다리안",
                                       "에즈란",
                                       "카엘",
                                       "오린",
                                       "리븐",
                                       "테론",
                                       "베이론",
                                       "제피르"};

        }

        private void SetStats()
        {
            switch (job)
            {
                case JobType.Warrior:
                    Power = 10f;
                    Defense = 5f;
                    MaxHP = 100f;
                    Hp = MaxHP;
                    break;

                case JobType.Magician:
                    Power = 15f;
                    Defense = 3f;
                    MaxHP = 80f;
                    Hp = MaxHP;
                    break;

                case JobType.Archer:
                    Power = 12f;
                    Defense = 4f;
                    MaxHP = 90f;
                    Hp = MaxHP;
                    break;
            }
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

        //이름을 랜덤으로 정해주는 메서드
        public void RandomName()
        {
            Random rand = new Random();
            int index = rand.Next(0, tempName.Length);

            Name = tempName[index];
        }

        //장비 판매 메서드
        public int SellItem(int price)
        {
            //판매가 설정
            //아이템은 원가의 85% 가격으로 판매된다.
            int ItemPrice = (int)(price * 0.85);
            Gold += ItemPrice;

            return ItemPrice;
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
        
        //레벨업 메서드
        public void LevelUp()
        {
            Level++;
            Exp = 0;
            MaxHP += 10f;
            Hp = MaxHP;
            Power += 0.5f;
            Defense += 1f;

            Console.WriteLine("\n레벨업! 현재 레벨: " + Level);
            Thread.Sleep(500);
            Console.WriteLine("공격력: " + Power + " 방어력: " + Defense + " 체력: " + MaxHP);
            Thread.Sleep(500);
            Console.WriteLine("다음 레벨업까지 필요한 경험치: " + (MaxExp - Exp));
            Thread.Sleep(1000);

        }


        //던전 클리어시 호출되는 메서드
        public void Victory(float dunjeonDef, int dunjeonGold)
        {

            //<체력 감소>
            Random rand = new Random();
            int temp = (int)(Defense - dunjeonDef);

            // 체력에서 20~35 사이의 값을 감소시킴
            int damage = rand.Next(20 - temp, 36 - temp);

            //만약 플레이어의 너무 방어력이 높아서
            //체력을 회복시켜주게 될 경우
            //데미지를 0으로 만든다.
            if (damage < 0) damage = 0;
            Hp -= damage;


            //<골드 증가>
            //공격력 추가보상: 공격력~공격력*2 % 값만큼 보너스 골드
            temp = rand.Next((int)(Power), (int)(Power * 2));
            float bonus = dunjeonGold * (temp * 0.01f);

            Gold += (int)(dunjeonGold + bonus);

        }

        //던전 공략 실패시 호출되는 메서드
        public void Defeat()
        {
            Hp -= (Hp / 2);
        }


    }
}