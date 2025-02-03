using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TextRPG.Models;

namespace TextRPG.Managers
{
    public class ItemManager
    {
        public int OwnedItemCount { get; set; }
        public int ItemCount {  get; private set; }

        private Item[] Items { get; set; }  // 아이템 정보를 저장할 배열

        //Event
        public event Func<int, bool> BuyAndSell;         //장비 구매
        public event Action<float, float> EquipEvent;    //장비 착용
        public event Action<float, float> UnequipEvent;  //장비 해제

        //ItemManager생성자에서는 주로 아이템의 정보를 넣어줌
        public ItemManager()
        {
            OwnedItemCount = 0;
            ItemCount = 9;
            Items = new Item[ItemCount];

            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new Item();
            }

            //방어구
            Items[0].Name = "수련자 갑옷";
            Items[0].Power = 0f;
            Items[0].Defense = 5f;
            Items[0].ItemInfo = "수련에 도움을 주는 갑옷입니다.";
            Items[0].Gold = 1000;
            Items[0].Type = ItemType.Armor;

            Items[1].Name = "무쇠갑옷";
            Items[1].Power = 0f;
            Items[1].Defense = 9f;
            Items[1].ItemInfo = "무쇠로 만들어져 튼튼한 갑옷입니다. ";
            Items[1].Gold = 1800;
            Items[1].Type = ItemType.Armor;

            Items[2].Name = "스파르타의 갑옷";
            Items[2].Power = 0f;
            Items[2].Defense = 15f;
            Items[2].ItemInfo = "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.";
            Items[2].Gold = 3500;
            Items[2].Type = ItemType.Armor;

            //무기
            Items[3].Name = "낡은 검";
            Items[3].Power = 2f;
            Items[3].Defense = 0f;
            Items[3].ItemInfo = "쉽게 볼 수 있는 낡은 검 입니다.";
            Items[3].Gold = 600;
            Items[3].Type = ItemType.Weapon;

            Items[4].Name = "청동 도끼";
            Items[4].Power = 5f;
            Items[4].Defense = 0f;
            Items[4].ItemInfo = "어디선가 사용됐던거 같은 도끼입니다.";
            Items[4].Gold = 1500;
            Items[4].Type = ItemType.Weapon;

            Items[5].Name = "스파르타의 창";
            Items[5].Power = 7f;
            Items[5].Defense = 0f;
            Items[5].ItemInfo = "스파르타의 전사들이 사용했다는 전설의 창입니다.";
            Items[5].Gold = 2700;
            Items[5].Type = ItemType.Weapon;

            //악세서리
            Items[6].Name = "나무 목걸이";
            Items[6].Power = 3f;
            Items[6].Defense = 3f;
            Items[6].ItemInfo = "누군가의 기원이 깃든 목걸이입니다.";
            Items[6].Gold = 2700;
            Items[6].Type = ItemType.Accessories;

            Items[7].Name = "누군가의 은반지";
            Items[7].Power = 6f;
            Items[7].Defense = 6f;
            Items[7].ItemInfo = "반지 뒤에 이니셜이 음각되어 있습니다.";
            Items[7].Gold = 2700;
            Items[7].Type = ItemType.Accessories;

            Items[8].Name = "스파르타의 허리띠";
            Items[8].Power = 9f;
            Items[8].Defense = 9f;
            Items[8].ItemInfo = "스파르타의 전사들이 사용했다는 전설의 허리띠입니다.";
            Items[8].Gold = 2700;
            Items[8].Type = ItemType.Accessories;

        }


        //상점에서 아이템 리스트를 보여주는 메서드
        public void ShowItemList(bool IsBuyMode)
        {
            int top = Console.CursorTop;

            //아이템 목록 출력
            for (int i = 0; i < Items.Length; i++)
            {
                top++;

                //만약 아이템 구매로 진입했다면
                if (IsBuyMode)
                {
                    //숫자 표시
                    Console.SetCursorPosition(1, top);
                    Console.Write($"{i + 1}  ");
                }
                //아직 인벤토리라면
                else
                { 
                    //숫자 없음
                    Console.SetCursorPosition(4, top);
                }

                Console.Write(Items[i].Name);

                Console.SetCursorPosition(23, top);
                Console.Write("| 공격력 +{0}", Items[i].Power);
                Console.SetCursorPosition(35, top);
                Console.Write(" 방어력 +{0} ", Items[i].Defense);
                    
                Console.SetCursorPosition(46, top);
                Console.Write(" | ");

                Console.Write(Items[i].ItemInfo);

                Console.SetCursorPosition(103, top);
                Console.Write(" | ");

                if (Items[i].State == ItemState.Have)
                    Console.WriteLine("판매완료");
                else
                {
                    Console.Write(Items[i].Gold);
                    Console.SetCursorPosition(111, top);
                    Console.WriteLine("G");
                }

                if ((i + 1) % 3 == 0) 
                {
                    top++;
                }

            }

        }

        //인벤토리를 보여주는 메서드
        public void ShowInventory(bool IsEquipmentMode)
        {
            int top = Console.CursorTop;
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
                        Console.Write($"{count + 1} ");
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
                    
                    Console.SetCursorPosition(24, top + count);
                    Console.Write(" | ");
                    
                    Console.Write("공격력 +{0}", Items[i].Power);

                    Console.SetCursorPosition(46, top + count);
                    Console.Write("방어력 +{0}", Items[i].Defense);

                    Console.Write(" | ");

                    Console.WriteLine(Items[i].ItemInfo);

                    

                    count++;
                }
            }
        }

        //장비 착용 메서드
        public void WearEquipment(int select)
        {
            int count = 1;
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].State != ItemState.HaveNot)
                {
                    Items[i].InventoryNum = count;
                    count++;
                }
            }

            for (int i = 0; i < Items.Length; i++)
            {
                if(Items[i].InventoryNum == select)
                {

                    if (Items[i].State == ItemState.Use)
                    {
                        //해제:
                        UnequipEvent?.Invoke(Items[i].Power, Items[i].Defense);
                        Items[i].State = ItemState.Have;
                    }
                    //장비를 착용하지 않은 상태라면
                    else if (Items[i].State == ItemState.Have)
                    {
                        //장착:
                        EquipEvent?.Invoke(Items[i].Power, Items[i].Defense);
                        Items[i].State = ItemState.Use;
                    }
                    else
                    {
                        Console.WriteLine("오류: 가진적 없는 장비를 장착하려합니다.");
                    }

                }
            }

            //장비를 착용 중인 상태라면
            
        }


        //아이템 구매
        public bool BuyItems(int select)
        {
            int i = select - 1;

            if (Items[i].State == ItemState.HaveNot)
            {
                bool HasEnoughGold = BuyAndSell.Invoke(Items[i].Gold);

                if (HasEnoughGold)
                {
                    Console.WriteLine($"\n{Items[i].Name}을(를) 성공적으로 구매했습니다!");
                    Items[i].State = ItemState.Have;
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

        //아이템 판매
        public bool SellItems(int select)
        {
            int i = select - 1;

            if (Items[i].State == ItemState.HaveNot)
            {
                bool HasEnoughGold = BuyAndSell.Invoke(Items[i].Gold);

                if (HasEnoughGold)
                {
                    Console.WriteLine($"\n{Items[i].Name}을(를) 성공적으로 구매했습니다!");
                    Items[i].State = ItemState.Have;
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
