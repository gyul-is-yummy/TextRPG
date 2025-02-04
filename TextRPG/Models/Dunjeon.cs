using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum DunjeonType
{
    Easy = 1,
    Nomal = 2,
    Hard = 3
}

namespace TextRPG.Models
{
    public class Dunjeon
    {
        public float Def { get; set; }
        public int Gold { get; set; }

        private DunjeonType level;
        public DunjeonType Level
        {
            get { return level; }
            set
            {
                //난이도가 입력될 때마다
                //권장 방어력과 보상 골드가 달라진다.

                if (value == DunjeonType.Easy)
                {
                    Def = 5f;
                    Gold = 1000;
                    level = value;
                }
                else if (value == DunjeonType.Nomal)
                {
                    Def = 11f;
                    Gold = 1700;
                    level = value;
                }
                else if (value == DunjeonType.Hard)
                {
                    Def = 17;
                    Gold = 2500;
                    level = value;
                }
                else
                {
                    Console.WriteLine("던전 level 입력 오류");
                }

            }
        }




        //생성자
        public Dunjeon()
        {

        }

        //던전 공략을 성공/실패하는지 체크하는 메서드
        public bool ClearCheck(float playerDef)
        {
            //플레이어 방어력이 던전 권장 방어력보다 높다면
            if (playerDef >= Def)
            {
                //무조건 클리어 성공
                return true;
            }


            Random rand = new Random();
            int probability = rand.Next(0, 100);

            //0~39가 나온다면
            if (probability < 40)
            {
                //던전 클리어 실패
                return false;
            }
            //40~99가 나온다면
            else
            {
                //던전 클리어 성공
                return true;
            }
        }

        //던전 공략 성공시 출력
        public void DungeonClear(float hp, float currentHp, int gold, int currentGold)
        {
            Console.Clear();
            Console.WriteLine("축하합니다!!");
            Console.WriteLine($"{level.ToString()} 난이도 던전을 클리어 하였습니다.\n");

            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"HP\t{hp} -> {currentHp}");
            Console.WriteLine($"Gold\t{gold} G -> {currentGold} G");
        }


        //던전 공략 실패시 출력
        public void DungeonFail(float hp, float currentHp)
        {
            Console.Clear();
            Console.WriteLine($"{level.ToString()} 난이도 던전 공략에 실패하였습니다.\n");

            Console.WriteLine("[탐험 결과]");
            Console.WriteLine($"HP\t{hp} -> {currentHp}");
        }


    }
}
