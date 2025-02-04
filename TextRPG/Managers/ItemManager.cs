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
        public int ItemCount { get; private set; }
        public int ItemTypeCount { get; private set; }

        private Item[] Items { get; set; }  // 아이템 정보를 저장할 배열
        public bool[] IsEquip { get; set; }


        //Event
        public event Func<int, bool> BuyEvent;           //장비 구매
        public event Func<int, int> SellEvent;           //장비 판매
        public event Action<float, float> EquipEvent;    //장비 착용
        public event Action<float, float> UnequipEvent;  //장비 해제

        //ItemManager생성자에서는 주로 아이템의 정보를 넣어줌
        public ItemManager(JobType playerJob)
        {
            OwnedItemCount = 0;                                             //소유 중인 아이템 갯수
            ItemCount = 9;                                                  //전체 아이템 갯수
            Items = new Item[ItemCount];                                    //아이템 정보를 저장할 배열
            ItemTypeCount = ItemType.GetValues(typeof(ItemType)).Length;    //장비 타입 갯수
            IsEquip = new bool[ItemTypeCount];                              //장비 타입별 착용 중인지 체크하는 변수

            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new Item();
                Items[i].InventoryNum = -1;
            }

            for (int i = 0; i < ItemTypeCount; i++)
            {
                IsEquip[i] = false;
            }

        }


        //상점에서 아이템 리스트를 보여주는 메서드
        public void ShowItemList(bool IsBuyMode)
        {
            int top = Console.CursorTop;

            //아이템 목록 출력
            for (int i = 0; i < Items.Length; i++)
            {
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

                if (Items[i].State != ItemState.HaveNot)
                    Console.WriteLine("구매완료");
                else
                {
                    Console.Write(Items[i].Gold);
                    Console.SetCursorPosition(111, top);
                    Console.WriteLine("G");
                }

                //3줄 마다 한 줄씩 더 띄워줌
                if ((i + 1) % 3 == 0)
                {
                    top++;
                }

                top++;

            }

        }

        //인벤토리를 보여주는 메서드
        public void ShowInventory(bool IsEquipmentMode)
        {
            InventoryNumInit();

            int top = Console.CursorTop;
            int count = 0;  //줄바꿈을 위한 count

            //보유 중인 아이템을 전부 보여줍니다.
            //이때 장착중인 아이템 앞에는 [E] 표시를 붙여 줍니다.
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].State == ItemState.HaveNot) continue;

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

                Console.SetCursorPosition(38, top + count);
                Console.Write("방어력 +{0}", Items[i].Defense);

                Console.Write(" | ");

                Console.WriteLine(Items[i].ItemInfo);

                count++;

            }
        }

        //장비 착용 메서드
        public void WearEquipment(int select)
        {

            for (int i = 0; i < Items.Length; i++)
            {
                //만약 선택한 Item이 아니라면 돌아가라
                if (Items[i].InventoryNum != select)
                    continue;

                //만약 동일한 Type에 이미 착용 중인 장비가 있다면
                if (IsEquip[(int)Items[i].Type])
                {
                    //현재 착용 중인 동타입 장비를 해제
                    FindEquippedItems(Items[i].Type);

                    //선택한 장비 장착
                    EquipEvent?.Invoke(Items[i].Power, Items[i].Defense);
                    Items[i].State = ItemState.Use;
                    IsEquip[(int)Items[i].Type] = true;
                    break;
                }
                //동일한 Type에 이미 착용 중인 장비가 없다면
                else
                {
                    //선택한 장비 장착
                    EquipEvent?.Invoke(Items[i].Power, Items[i].Defense);
                    Items[i].State = ItemState.Use;
                    IsEquip[(int)Items[i].Type] = true;
                    break;
                }
            }
        }


        //아이템 구매
        public void BuyItems(int select)
        {
            int i = select - 1;

            if (Items[i].State == ItemState.HaveNot)
            {
                bool HasEnoughGold = BuyEvent.Invoke(Items[i].Gold);

                if (HasEnoughGold)
                {
                    Console.WriteLine($"\n{Items[i].Name}을(를) 성공적으로 구매했습니다!");
                    Items[i].State = ItemState.Have;
                    OwnedItemCount++;

                    //인벤토리 넘버 재부여
                    InventoryNumInit();
                    Thread.Sleep(500);
                }
                else
                {
                    Console.WriteLine("\nGold 가 부족합니다.");
                    Thread.Sleep(500);
                }

                //return HasEnoughGold;

            }
            else
            {
                Console.WriteLine("\n이미 구매한 아이템입니다.");
                Thread.Sleep(500);
                //return false;
            }
        }

        //아이템 판매
        public void SellItems(int select)
        {

            for (int i = 0; i < Items.Length; i++)
            {
                //가지고 있지 않은 아이템이라면 돌려보내준다.
                if (Items[i].State == ItemState.HaveNot) continue;

                //입력받은 select와 같은 숫자를 가지고 있는 아이템을 찾아줌
                if (Items[i].InventoryNum == select)
                {
                    //만약 착용중인 아이템을 판매한다면
                    if (Items[i].State == ItemState.Use)
                        //판매하기 전 착용 해제 이벤트 실행
                        UnequipEvent?.Invoke(Items[i].Power, Items[i].Defense);

                    //아이템의 상태를 보유X 상태로 변경
                    Items[i].State = ItemState.HaveNot;

                    //플레이어 소지 골드 추가
                    int? price = SellEvent?.Invoke(Items[i].Gold);

                    //소지 아이템 갯수 -1
                    OwnedItemCount--;

                    Console.WriteLine($"\n{Items[i].Name}를 판매했습니다.");
                    Console.WriteLine($"{price} G를 받았습니다!");

                    //인벤토리 넘버 재부여
                    InventoryNumInit();

                    break;
                }
            }

            Thread.Sleep(500);

        }

        public void InventoryNumInit()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].InventoryNum = 0;
            }

            int count = 1;
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].State != ItemState.HaveNot)
                {
                    Items[i].InventoryNum = count;
                    count++;
                }
            }
        }

        public void FindEquippedItems(ItemType searchType)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                //동일한 타입의, 현재 착용중인 장비를 서치
                if (Items[i].Type == searchType
                    && Items[i].State == ItemState.Use)
                {
                    //착용 해제 시켜준다.
                    UnequipEvent?.Invoke(Items[i].Power, Items[i].Defense);
                    Items[i].State = ItemState.Have;
                }
            }
        }


        public void SetItems(JobType playerJob)
        {
            if (playerJob == JobType.Warrior)
            {
                SetItemsForWarrior();
            }
            else if (playerJob == JobType.Magician)
            {
                SetItemsForMagician();
            }
            else if (playerJob == JobType.Archer)
            {
                SetItemsForArcher();
            }
        }

        private void SetItemsForWarrior()
        {
            //전사 방어구
            Items[0].Name = "수련용 사슬 갑옷";
            Items[0].Power = 0f;
            Items[0].Defense = 5f;
            Items[0].ItemInfo = "수련에 도움을 주는 사슬 갑옷입니다.";
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

            //전사 무기
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
            Items[8].ItemInfo = "스파르타의 용사들이 사용했다는 전설의 허리띠입니다.";
            Items[8].Gold = 2700;
            Items[8].Type = ItemType.Accessories;
        }


        private void SetItemsForMagician()
        {
            //마법사 방어구
            Items[0].Name = "수련용 로브";
            Items[0].Power = 0f;
            Items[0].Defense = 5f;
            Items[0].ItemInfo = "수련에 도움을 주는 로브입니다.";
            Items[0].Gold = 1000;
            Items[0].Type = ItemType.Armor;

            Items[1].Name = "마법사 로브";
            Items[1].Power = 0f;
            Items[1].Defense = 9f;
            Items[1].ItemInfo = "던전산 실로 한땀한땀 만든 마법사 로브입니다.";
            Items[1].Gold = 1800;
            Items[1].Type = ItemType.Armor;

            Items[2].Name = "스파르타의 로브";
            Items[2].Power = 0f;
            Items[2].Defense = 15f;
            Items[2].ItemInfo = "스파르타의 마법사들이 사용했다는 전설의 로브입니다.";
            Items[2].Gold = 3500;
            Items[2].Type = ItemType.Armor;

            //마법사 무기
            Items[3].Name = "낡은 지팡이";
            Items[3].Power = 2f;
            Items[3].Defense = 0f;
            Items[3].ItemInfo = "쉽게 볼 수 있는 낡은 지팡이 입니다.";
            Items[3].Gold = 600;
            Items[3].Type = ItemType.Weapon;

            Items[4].Name = "멋들어진 케인";
            Items[4].Power = 5f;
            Items[4].Defense = 0f;
            Items[4].ItemInfo = "성능과 디자인을 모두 잡은, 멋쟁이 마법사들의 필수 지팡이입니다.";
            Items[4].Gold = 1500;
            Items[4].Type = ItemType.Weapon;

            Items[5].Name = "스파르타의 스태프";
            Items[5].Power = 7f;
            Items[5].Defense = 0f;
            Items[5].ItemInfo = "스파르타 소속 수석 마법사들이 사용했다는 전설의 스태프입니다.";
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
            Items[8].ItemInfo = "스파르타의 용사들이 사용했다는 전설의 허리띠입니다.";
            Items[8].Gold = 2700;
            Items[8].Type = ItemType.Accessories;
        }

        private void SetItemsForArcher()
        {
            //궁수 방어구
            Items[0].Name = "수련용 천옷";
            Items[0].Power = 0f;
            Items[0].Defense = 5f;
            Items[0].ItemInfo = "가볍지만 질기고, 빨래가 쉬운 수련용 천옷입니다.";
            Items[0].Gold = 1000;
            Items[0].Type = ItemType.Armor;

            Items[1].Name = "가죽 보호대";
            Items[1].Power = 0f;
            Items[1].Defense = 9f;
            Items[1].ItemInfo = "몬스터의 가죽으로 만든 가볍고 튼튼한 가죽 보호대입니다.";
            Items[1].Gold = 1800;
            Items[1].Type = ItemType.Armor;

            Items[2].Name = "스파르타의 장갑";
            Items[2].Power = 0f;
            Items[2].Defense = 15f;
            Items[2].ItemInfo = "스파르타의 궁수들이 사용했다는 전설의 장갑입니다.";
            Items[2].Gold = 3500;
            Items[2].Type = ItemType.Armor;

            //궁수 무기
            Items[3].Name = "낡은 활";
            Items[3].Power = 2f;
            Items[3].Defense = 0f;
            Items[3].ItemInfo = "쉽게 볼 수 있는 낡은 활 입니다.";
            Items[3].Gold = 600;
            Items[3].Type = ItemType.Weapon;

            Items[4].Name = "튼튼한 철제 석궁";
            Items[4].Power = 5f;
            Items[4].Defense = 0f;
            Items[4].ItemInfo = "사용감이 있지만 매우 튼튼한 철제 석궁입니다.";
            Items[4].Gold = 1500;
            Items[4].Type = ItemType.Weapon;

            Items[5].Name = "스파르타의 대궁";
            Items[5].Power = 7f;
            Items[5].Defense = 0f;
            Items[5].ItemInfo = "스파르타의 궁수들이 사용했다는 전설의 대궁입니다.";
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
            Items[8].ItemInfo = "스파르타의 용사들이 사용했다는 전설의 허리띠입니다.";
            Items[8].Gold = 2700;
            Items[8].Type = ItemType.Accessories;
        }

    }
}
