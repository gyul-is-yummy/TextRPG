using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace TextRPG
{
    public enum ItemType
    {
        Weapon,
        Armor
    }

    public enum ItemState
    {
        HaveNot,
        Have,
        Use
    }

    public class Character
    {
        //int level { get; set; }
        public string name { get; set; } = string.Empty;
        //string job { get; set; }
        public float power { get; set; }
        public float defense { get; set; }
        public float hp { get; set; }
        public float speed { get; set; }
        //int gold { get; set; }

        public Character()
        {
            name = "캐릭터";
            power = 1f;
            defense = 1f;
            hp = 1f;
            speed = 1f;
        }

    }

    public class Item
    {
        public string Name { get; set; } = string.Empty;
        public string ItemInfo { get; set; } = string.Empty;
        public int Gold { get; set; } = 100;
        public bool IsSell { get; set; } = false;
        public float Power { get; set; } = 0f;
        public float Defense { get; set; } = 0f;
        public ItemType Type { get; set; }
        public ItemState State { get; set; } = ItemState.HaveNot;

    }


    public class ItemManager
    {
        private Item[] Items { get; set; }
        public event Func<int, bool> BuyAndSell;

        public ItemManager()
        {

            Items = new Item[6];

            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new Item();
            }

            //방어구
            Items[0].Name = "수련자 갑옷";
            Items[0].Defense = 5f;
            Items[0].ItemInfo = "쉽게 볼 수 있는 낡은 검 입니다.";
            Items[0].Gold = 1000;
            Items[0].Type = ItemType.Armor;

            Items[1].Name = "무쇠갑옷";
            Items[1].Defense = 9f;
            Items[1].ItemInfo = "어디선가 사용됐던거 같은 도끼입니다.";
            Items[1].Gold = 1800;
            Items[1].Type = ItemType.Armor;

            Items[2].Name = "스파르타의 갑옷";
            Items[2].Defense = 15f;
            Items[2].ItemInfo = "스파르타의 전사들이 사용했다는 전설의 창입니다.";
            Items[2].Gold = 3500;
            Items[2].Type = ItemType.Armor;

            //무기
            Items[3].Name = "낡은 검";
            Items[3].Power = 2f;
            Items[3].ItemInfo = "쉽게 볼 수 있는 낡은 검 입니다.";
            Items[3].Gold = 600;
            Items[3].Type = ItemType.Weapon;

            Items[4].Name = "청동 도끼";
            Items[4].Power = 5f;
            Items[4].ItemInfo = "어디선가 사용됐던거 같은 도끼입니다.";
            Items[4].Gold = 1500;
            Items[4].Type = ItemType.Weapon;

            Items[5].Name = "스파르타의 창";
            Items[5].Power = 7f;
            Items[5].ItemInfo = "스파르타의 전사들이 사용했다는 전설의 창입니다.";
            Items[5].Gold = 2700;
            Items[5].Type = ItemType.Weapon;
        }

        public void ShowItemList(int top)
        {
            //아이템 목록 출력
            for (int i = 0; i < Items.Length; i++)
            {
                Console.SetCursorPosition(0, top + i);
                Console.Write(Items[i].Name);

                Console.SetCursorPosition(18, top + i);
                Console.Write(" | ");

                if (Items[i].Type == ItemType.Weapon)
                    Console.Write("공격력 +{0}",Items[i].Power);

                else if (Items[i].Type == ItemType.Armor)
                    Console.Write("방어력 +{0}", Items[i].Defense);

                Console.SetCursorPosition(32, top + i);
                Console.Write(" | ");

                Console.Write(Items[i].ItemInfo);

                Console.SetCursorPosition(90, top + i);
                Console.Write(" | ");

                if (Items[i].IsSell == true)
                    Console.WriteLine("판매완료");
                else
                    Console.WriteLine(Items[i].Gold + " G");
                
            }

        }
        
        public void ShowInventory(int top)
        {
            int count = 0;

            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].State == ItemState.Use)
                {
                    Console.SetCursorPosition(0, top + count);
                    Console.Write(Items[i].Name);

                    Console.SetCursorPosition(18, top + count);
                    Console.Write(" | ");

                    if (Items[i].Type == ItemType.Weapon)
                        Console.Write("공격력 +{0}", Items[i].Power);

                    else if (Items[i].Type == ItemType.Armor)
                        Console.Write("방어력 +{0}", Items[i].Defense);

                    Console.SetCursorPosition(32, top + count);
                    Console.Write(" | ");

                    Console.Write(Items[i].ItemInfo);

                    Console.SetCursorPosition(90, top + count);
                    Console.Write(" | ");

                    if (Items[i].IsSell == true)
                        Console.WriteLine("판매완료");
                    else
                        Console.WriteLine(Items[i].Gold + " G");

                    count++;
                }
            }
        }

        //아이템 구매
        public bool SellItems(int select)
        {
            if(Items[select-1].State == ItemState.HaveNot)
            {

                bool HasEnoughGold = BuyAndSell.Invoke(Items[select - 1].Gold);
                return HasEnoughGold;

                //아이템을 구매한 상태가 아니고,
                //플레이어의 소지금이 아이템의 가격보다 많다면
                //구매
            }
            else
            {
                Console.WriteLine("이미 구매한 아이템입니다.");
                return false;
            }
        }



    }


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
        }

        public bool BuyItem(int price)
        {
            if(price > gold)
            {
                Console.WriteLine("소지 골드가 부족합니다.");
                return false;
            }
            else
            {
                Gold -= price;
                Console.WriteLine("소지 골드가 부족합니다.");
                return true;
            }

            
        }



    }

    public class MainGame
    {
        private Player player;
        private ItemManager itemManager;

        public MainGame()
        {
            player = new Player();
            itemManager = new ItemManager();

            itemManager.BuyAndSell += player.BuyItem;
            //itemManager.BuyAndSell += ItemShop_Buy();
        }

        //키 입력을 받고 올바른 입력인지 체크하는 메서드
        public int InputCheck(int start, int end)
        {
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            string inputString = Console.ReadLine();

            for (int i = start; i <= end; i++)
            {
                if (inputString == i.ToString())
                {
                    return int.Parse(inputString);
                }
            }

            Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
            Thread.Sleep(500);
            return -1;
        }
        
        // 게임 시작 화면
        public void GameStart()
        {

            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");

            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            //Console.WriteLine("4. 던전 입장");

            Console.WriteLine("\n0. 게임종료");

            //키를 입력받음
            int input = InputCheck(0, 3);

            if (input == -1) GameStart();
            else if (input == 1) StatusCheck();
            else if (input == 2) Inventory();
            else if (input == 3) ItemShop();
            else Environment.Exit(0);



        }

        // 플레이어 상태 확인
        public void StatusCheck()
        {

            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"{player.name} ( {player.Job} )");
            Console.WriteLine($"공격력 : {player.power}");
            Console.WriteLine($"방어력 : {player.defense}");
            Console.WriteLine($"체 력 : {player.hp}");
            Console.WriteLine($"Gold : {player.Gold} G");

            Console.WriteLine("\n0. 나가기");
            int input = InputCheck(0, 0);

            if (input == -1) StatusCheck();
            else GameStart();


        }
        
        // 플레이어 인벤토리 확인
        public void Inventory()
        {

            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]\n");

            //아이템 목록 출력
            itemManager.ShowInventory(5);

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");
            int input = InputCheck(0, 1);

            if (input == -1) Inventory();
            else if (input == 1) EquipItem();
            else GameStart();

        }

        //장착 관리
        public void EquipItem()
        {

            Console.Clear();
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]\n");

            //아이템 목록 출력
            //itemManager.ShowItemList();

            Console.WriteLine("\n0. 나가기");
            int input = InputCheck(0, 1);

            if (input == -1) EquipItem();
            else Inventory();


        }

        //상점
        public void ItemShop()
        {

            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine(player.Gold + "G\n");

            Console.WriteLine("[아이템 목록]");

            //예시
            itemManager.ShowItemList(7);

            Console.WriteLine("\n1. 아이템 구매");
            //Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            int input = InputCheck(0, 2);

            if (input == -1) ItemShop();
            else if(input == 1) ItemShop_Buy();
            else GameStart();


        }

        public void ItemShop_Buy()
        {

            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine(player.Gold + "G\n");

            Console.WriteLine("[아이템 목록]");

            //아이템 목록 호출
            itemManager.ShowItemList(7);

            Console.WriteLine("\n0. 나가기");
            int input = InputCheck(0, 6);

            if (input == -1) ItemShop_Buy();
            else if(input == 0) ItemShop();
            else
            {
                bool HasEnoughGold = itemManager.SellItems(input);
                ItemShop_Buy();
            }
                

        }


    }



    internal class Program
    {
        static void Main(string[] args)
        {
            MainGame textRPG = new MainGame();
            textRPG.GameStart();
        }
    }
}
