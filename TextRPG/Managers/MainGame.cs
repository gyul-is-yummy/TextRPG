using TextRPG.Models;

namespace TextRPG.Managers
{
    public class MainGame
    {
        private Player player;
        private ItemManager itemManager;
        private Dunjeon dunjeon;

        //Event
        public event Action<float, float, int, int> DungeonClearEvent;
        public event Action<float> DungeonFailEvent;

        public MainGame()
        {
            player = new Player();
            itemManager = new ItemManager(player.Job);
            dunjeon = new Dunjeon();

            // Event 연결
            itemManager.BuyEvent += player.BuyItem;
            itemManager.SellEvent += player.SellItem;
            itemManager.EquipEvent += player.UseItem;
            itemManager.UnequipEvent += player.DisuseItem;
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

        //이름을 입력받는 메서드
        public void InputName()
        {
            Console.Clear();
            Console.WriteLine("안녕하세요 용사님!");
            Console.WriteLine("저는 용사님의 여정을 함께할 도우미 요정 솔라입니다.");

            Console.WriteLine("\n용사님의 성함을 제게 알려주시겠어요?");

            Console.WriteLine("\n1. 기억나지 않는다 | 2. 대충 둘러댄다 | 3. \"내 이름은...\" (이름 입력)");

            int input = InputCheck(1, 3);

            if (input == -1) InputName();
            else if (input == 1)
            {
                Console.WriteLine("\n이럴수가! 그렇다면 어쩔 수 없죠...");
                Console.WriteLine("용사님이 기억을 찾으실 때까지는 용사님이라고 부르겠습니다!");
                player.Name = "용사";
                Thread.Sleep(2000);
            }
            else if (input == 2)
            {
                player.RandomName();
                Console.WriteLine($"\n{player.Name}님이시군요! 반가워요!");
                Thread.Sleep(1000);
            }
            else
            {
                Console.Write("\n\"내 이름은... ");

                //커서 위치 저장
                int left = Console.CursorLeft;
                int top = Console.CursorTop;

                //이름 입력
                player.Name = Console.ReadLine();

                //받아온 커서 위치에 이어서 출력할 수 있도록 함.
                Console.SetCursorPosition(left, top);
                Console.WriteLine($"{player.Name}(이)라고 해.\"");

                Console.WriteLine($"\n\"{player.Name}님이시군요! 반가워요! \"");

                Thread.Sleep(2000);
            }
        }


        //직업 선택
        public void SelectJob()
        {
            Console.Clear();
            Console.WriteLine($"{player.Name} 님의 특기는 무엇인가요?");

            Console.WriteLine($"\n1. 검술 | 2. 마법 | 3. 궁술");

            int input = InputCheck(1, 3);

            if (input == -1) SelectJob();
            else if (input == 1)
            {
                player.Job = JobType.Warrior;
                Console.Write("\n검술이라니!");
            }     
            else if (input == 2)
            {
                player.Job = JobType.Magician;
                Console.Write("\n마법이라니!");
            }
            else
            {
                player.Job = JobType.Archer;
                Console.Write("\n궁술이라니!");
            }
            
            Console.Write(" 정말 멋진 특기를 가지고 계시네요!");

            //플레이어 직업에 맞춰 상점 아이템 세팅
            itemManager.SetItems(player.Job);
            Thread.Sleep(1000);
        }

        // 게임 메인 화면
        public void GameStart()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");

            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");

            Console.WriteLine("\n0. 게임종료");

            //키를 입력받음
            int input = InputCheck(0, 5);

            if (input == -1) GameStart();
            else if (input == 1) StatusCheck();
            else if (input == 2) Inventory();
            else if (input == 3) ItemShop();
            else if (input == 4) EnterDungeon();
            else if (input == 5) Rest();
            else Environment.Exit(0);

        }

        // 플레이어 상태 확인 Scene
        public void StatusCheck()
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

            Console.WriteLine($"Lv. {player.Level} ({player.Exp}/{player.MaxExp})");
            Console.WriteLine($"{player.Name} ({player.JobName})");
            Console.WriteLine($"공격력 : {player.Power} (+{player.ItemPow})");
            Console.WriteLine($"방어력 : {player.Defense} (+{player.ItemDef})");
            Console.WriteLine($"체 력  : {player.Hp}/{player.MaxHP}");
            Console.WriteLine($"Gold   : {player.Gold} G");

            Console.WriteLine("\n0. 나가기");
            int input = InputCheck(0, 0);

            if (input == -1) StatusCheck();
            else GameStart();

        }

        // 플레이어 인벤토리 Scene
        public void Inventory()
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]");

            //인벤토리 호출
            itemManager.ShowInventory(false);

            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");
            int input = InputCheck(0, 1);

            if (input == -1) Inventory();
            else if (input == 1) EquipItem();
            else GameStart();
        }

        //인벤토리 - 장착 관리 Scene
        public void EquipItem()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");

            Console.WriteLine("[아이템 목록]");

            //인벤토리 호출
            //장비 관리에 들어갈 때만 true를 넣어준다.
            itemManager.ShowInventory(true);

            Console.WriteLine("\n0. 나가기");
            int input = InputCheck(0, itemManager.OwnedItemCount);

            if (input == -1) EquipItem();
            else if (input == 0) Inventory();
            else
            {
                itemManager.WearEquipment(input);
                EquipItem();
            }
        }

        //상점 Scene
        public void ItemShop()
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine(player.Gold + "G\n");

            Console.WriteLine("[아이템 목록]");

            //아이템 목록 호출
            //아이템 구매에 들어갈 때만 true를 넣어준다.
            itemManager.ShowItemList(false);

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            int input = InputCheck(0, 2);

            if (input == -1) ItemShop();
            else if (input == 1) ItemShop_Buy();
            else if (input == 2) ItemShop_Sell();
            else GameStart();

        }

        //상점 - 아이템 구매 Scene
        public void ItemShop_Buy()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine(player.Gold + "G\n");

            Console.WriteLine("[아이템 목록]");

            //아이템 목록 호출
            //아이템 구매에 들어갈 때만 true를 넣어준다.
            itemManager.ShowItemList(true);

            Console.WriteLine("\n0. 나가기");
            int input = InputCheck(0, itemManager.ItemCount);

            if (input == -1) ItemShop_Buy();
            else if (input == 0) ItemShop();
            else
            {
                itemManager.BuyItems(input);
                ItemShop_Buy();
            }

        }

        //상점 - 아이템 판매 Scene
        public void ItemShop_Sell()
        {
            Console.Clear();
            Console.WriteLine("상점 - 아이템 판매");
            Console.WriteLine("필요 없는 아이템을 85% 가격으로 팔 수 있습니다.\n");

            Console.WriteLine("[보유 골드]");
            Console.WriteLine(player.Gold + "G\n");

            Console.WriteLine("[아이템 목록]");

            //아이템 목록 호출
            //아이템 구매에 들어갈 때만 true를 넣어준다.
            itemManager.ShowInventory(true);

            Console.WriteLine("\n0. 나가기");
            int input = InputCheck(0, itemManager.OwnedItemCount);

            if (input == -1) ItemShop_Sell();
            else if (input == 0) ItemShop();
            else
            {
                itemManager.SellItems(input);
                ItemShop_Sell();
            }
        }

        //휴식하기 Scene
        public void Rest()
        {
            Console.Clear();
            Console.WriteLine("휴식하기");
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G)\n");

            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");

            int input = InputCheck(0, 1);

            if (input == -1) Rest();
            else if(input == 0) GameStart();
            else
            {
                //만약 충분한 골드를 가지고 있다면
                if(player.HasEnoughGold(500))
                {
                    Console.WriteLine("\n휴식을 완료했습니다.");
                    player.Hp += 100; //체력회복
                    Thread.Sleep(500);
                }
                //충분하지 않다면
                else
                {
                    Console.WriteLine("\nGold가 부족합니다.");
                    Thread.Sleep(500);
                }

                //휴식 후 메인 화면으로 돌아감
                GameStart();
            }
        }

        //던전입장 Scene
        public void EnterDungeon()
        {
            Console.Clear();
            Console.WriteLine("던전입장");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");

            //현재 본인 상태를 확인하고 난이도를 선택할 수 있도록 함
            Console.WriteLine($"[현재 {player.Name}의 상태]");
            Console.WriteLine($"Lv. {player.Level} ({player.Exp}/{player.MaxExp})");
            Console.WriteLine($"체력   : {player.Hp}/{player.MaxHP}");
            Console.WriteLine($"공격력 : {player.Power} (+{player.ItemPow})");
            Console.WriteLine($"방어력 : {player.Defense} (+{player.ItemDef})");

            Console.WriteLine("\n1. 쉬운 던전\t| 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전\t| 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전\t| 방어력 17 이상 권장");

            Console.WriteLine("\n0. 나가기");
            int input = InputCheck(0, 3);

            if (input == -1) EnterDungeon();
            else if (input == 0) GameStart();
            else
            {
                //입력받은 input으로 난이도 설정
                dunjeon.Level = (DunjeonType)input;

                //플레이어의 기존 소지금/체력 저장
                float tempHp = player.Hp;
                int tempGold = player.Gold;

                //던전 클리어 여부 체크
                bool isClear = dunjeon.ClearCheck(player.Defense);

                //만약 클리어했다면
                if (isClear)
                {
                    player.Victory(dunjeon.Def, dunjeon.Gold);
                    dunjeon.DungeonClear(tempHp, player.Hp, tempGold, player.Gold);

                    //던전 공략중에 플레이어가 쓰러졌다면
                    if (player.IsDie)
                    {
                        /*
                        IsDie: 플레이어의 체력이 0 이하가 될 경우 프로퍼티에서 isDie를 true로 변경
                        프로퍼티에서 바로 PlayerDie()를 실행하지 않은 이유: 순서가 꼬임...
                        */

                        //기절 스크립트를 출력하고 HP를 1로 만들어준다.
                        player.PlayerDie();
                        player.Hp = 1;
                        player.IsDie = false;

                        //메인 화면으로
                        GameStart();
                    }
                    //죽지 않았다면
                    else
                    {
                        //경험치를 증가 시켜준다.
                        player.Exp++;
                    }
                }
                //클리어하지 못했다면
                else
                {
                    player.Defeat();
                    dunjeon.DungeonFail(tempHp, player.Hp);
                }

                Thread.Sleep(2000);
                EnterDungeon();
            }
        }
    }
}
