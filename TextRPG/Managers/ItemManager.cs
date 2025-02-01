using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Models;

namespace TextRPG.Managers
{
    public class ItemManager
    {
        private Item[] Items { get; set; }
        public event Func<int, bool> BuyAndSell;    //장비 구매
        public event Action<float, ItemType> EquipItemEvent;
        public int OwnedItemCount;   //소유 아이템 갯수

        //ItemManager생성자에서는 주로 아이템의 정보를 넣어줌
        public ItemManager()
        {
            OwnedItemCount = 0;
            Items = new Item[6];

            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new Item();
            }

            //방어구
            Items[0].Name = "수련자 갑옷";
            Items[0].Defense = 5f;
            Items[0].ItemInfo = "무쇠로 만들어져 튼튼한 갑옷입니다.";
            Items[0].Gold = 1000;
            Items[0].Type = ItemType.Armor;

            Items[1].Name = "무쇠갑옷";
            Items[1].Defense = 9f;
            Items[1].ItemInfo = "무쇠로 만들어져 튼튼한 갑옷입니다. ";
            Items[1].Gold = 1800;
            Items[1].Type = ItemType.Armor;

            Items[2].Name = "스파르타의 갑옷";
            Items[2].Defense = 15f;
            Items[2].ItemInfo = "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.";
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


        //상점에서 아이템 리스트를 보여주는 메서드
        public void ShowItemList(int top, bool IsBuyMode)
        {
            //아이템 목록 출력
            for (int i = 0; i < Items.Length; i++)
            {
                //만약 아이템 구매로 진입했다면
                if (IsBuyMode)
                {
                    //숫자 표시
                    Console.SetCursorPosition(1, top + i);
                    Console.Write($"{i + 1}  ");
                }
                //아직 인벤토리라면
                else
                {
                    //숫자 없음
                    Console.SetCursorPosition(4, top + i);
                }

                Console.Write(Items[i].Name);

                Console.SetCursorPosition(18, top + i);
                Console.Write(" | ");

                if (Items[i].Type == ItemType.Weapon)
                    Console.Write("공격력 +{0}", Items[i].Power);

                else if (Items[i].Type == ItemType.Armor)
                    Console.Write("방어력 +{0}", Items[i].Defense);

                Console.SetCursorPosition(32, top + i);
                Console.Write(" | ");

                Console.Write(Items[i].ItemInfo);

                Console.SetCursorPosition(90, top + i);
                Console.Write(" | ");

                if (Items[i].State == ItemState.Have)
                    Console.WriteLine("판매완료");
                else
                    Console.WriteLine(Items[i].Gold + " G");

            }

        }


        //인벤토리를 보여주는 메서드
        public void ShowInventory(int top, bool IsEquipmentMode)
        {
            int count = 0;  //줄바꿈을 위한 count

            //보유 중인 아이템을 전부 보여줍니다.
            //이때 장착중인 아이템 앞에는 [E] 표시를 붙여 줍니다.
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].State != ItemState.HaveNot)
                {
                    //만약 장비 관리로 진입했다면
                    if (IsEquipmentMode)
                    {
                        //숫자 표시
                        Console.SetCursorPosition(1, top + count);
                        Console.Write($"{i + 1} ");
                    }
                    //아직 인벤토리라면
                    else
                    {
                        //숫자 없음
                        Console.SetCursorPosition(3, top + count);
                    }

                    //착용중인 장비인지 확인
                    if (Items[i].State == ItemState.Use)
                    {
                        Console.Write($"[E] ");
                    }
                    else
                    {
                        Console.Write($" -  ");
                    }

                    Console.Write(Items[i].Name);

                    Console.SetCursorPosition(18, top + count);
                    Console.Write(" | ");

                    if (Items[i].Type == ItemType.Weapon)
                        Console.Write("공격력 +{0}", Items[i].Power);

                    else if (Items[i].Type == ItemType.Armor)
                        Console.Write("방어력 +{0}", Items[i].Defense);

                    Console.SetCursorPosition(32, top + count);
                    Console.Write(" | ");

                    Console.WriteLine(Items[i].ItemInfo);

                    count++;
                }
            }
        }

        //장비 착용 메서드
        public void WearEquipment(int select)
        {

            if (Items[select - 1].State == ItemState.Use)
            {
                Items[select - 1].State = ItemState.Have;

                if (Items[select - 1].Type == ItemType.Weapon)
                    EquipItemEvent?.Invoke(Items[select - 1].Power, Items[select - 1].Type);
            }
            else if (Items[select - 1].State == ItemState.Have)
            {
                Items[select - 1].State = ItemState.Use;
            }
            else
            {
                Console.WriteLine("오류: 가진적 없는 장비를 장착하려함");
            }

        }



        //아이템 구매
        public bool SellItems(int select)
        {
            if (Items[select - 1].State == ItemState.HaveNot)
            {

                bool HasEnoughGold = BuyAndSell.Invoke(Items[select - 1].Gold);

                if (HasEnoughGold)
                {
                    Console.WriteLine($"\n{Items[select - 1].Name}을(를) 성공적으로 구매했습니다!");
                    Items[select - 1].State = ItemState.Have;
                    OwnedItemCount++;
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine("\nGold 가 부족합니다.");
                    Thread.Sleep(1000);
                }

                return HasEnoughGold;

            }
            else
            {
                Console.WriteLine("\n이미 구매한 아이템입니다.");
                Thread.Sleep(1000);
                return false;
            }
        }
    }
}
