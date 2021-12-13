using System;
using System.Media;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace TurnBasedRPG
{
    class Program
    {
        static int maxMonsterHP;
        static int halfMonsterHP;
        static int currentMonsterHP;
        static int monsterDamage;
        static int monsterHeal;
        static int maxPlayerHP;
        static int currentPlayerHP;
        static int playerDamage = 10;
        static int healthPotionHeal;
        static int Coins;
        static int lootedCoins;
        static int shopKeeperLines;
        static int blockReduction;
        static int maxPlayerLvl = 10;
        static int playerLvl;
        static int exp;
        static int recievedEXP;
        static int playerPotions = 3;
        static int monsterChoice;
        static int checkheal;
        static bool gameLoop = true;
        static bool onMenu = true;
        static bool inGame;
        static bool monsterTryingToHeal;
        static bool gameOver;
        static bool inFight;
        static bool inStatScreen;
        static bool inShop = true;
        static bool Loaded;
        static bool playerTurn = true;
        static bool monsterTurn;
        static bool shopping;
        static bool attacking;
        static bool defending;
        static bool healing;
        static bool blocking;
        static bool failedToHeal;
        static bool shopanimLeftb;
        static bool shopUIInitialized;
        static bool errorFound;
        static bool shopKeeperHasSpoke = false;
        static bool changingScreen;
        static int shopanimLeft = 1;
        static bool waitingForAnim = true;
        static bool animTimerStarted;
        static bool hasConsoleCleared = false;

        static int playerInGameMenu = 1;
        static int shopMenu = 1;
        static int inMainMenu = 1;
        static int inFightMusic;

        static string playerleveltemp;
        static string exptemp;
        static string currentplayerhptemp;
        static string playerpotionstemp;
        static string coinstemp;
        static string currentmonsterhptemp;
        static string maxmonsterhptemp;
        static string infighttemp;
        static string inshoptemp;

        public const int VK_F11 = 0x7A;
        public const int SW_MAXIMIZE = 3;

        public const uint WM_KEYDOWN = 0x100;
        public const uint WM_MOUSEWHEEL = 0x20A;

        public const uint WHEEL_DELTA = 120;
        public const uint MK_CONTROL = 0x00008 << 120;

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            FullScreen();
            GameLoop();
            Console.WriteLine("FightDome...");
            Console.ReadKey();
        }

        static void GameLoop()
        {
            while (gameLoop == true)
            {
                while (onMenu == true)
                {
                    AudioController();
                    MainMenuChoice();
                }
                if (inFight == true)
                {
                    AudioController();
                    MonsterInitializer();
                    Sprites();
                }
                while (inFight == true)
                {
                    PlayerTurn();
                    MonsterTurn();
                    hasConsoleCleared = false;
                }
                if (currentPlayerHP <= 0)
                {
                    ClearConsole();
                    System.Threading.Thread.Sleep(1000);
                    AudioController();
                    GameOver();
                    hasConsoleCleared = false;
                }
                if (hasConsoleCleared == false)
                {
                    ClearConsole();
                    System.Threading.Thread.Sleep(1000);
                    Console.SetCursorPosition(0, 0);
                }
                while (inFight == false && inStatScreen == true)
                {
                    StatScreen();
                    hasConsoleCleared = false;
                }
                if(hasConsoleCleared == false)
                {
                    ClearConsole();
                    System.Threading.Thread.Sleep(1000);
                    Console.SetCursorPosition(0, 0);
                }
                if (inShop == true)
                {
                    AudioController();
                    animTimerStarted = false;
                    shopUIInitialized = false;
                }
                while (inFight == false && inStatScreen == false && inShop == true)
                {
                    ShopChoice();
                    hasConsoleCleared = false;
                    Loaded = false;
                }
                if (hasConsoleCleared == false)
                {
                    ClearConsole();
                    System.Threading.Thread.Sleep(1000);
                    Console.SetCursorPosition(0, 0);
                }
            }
        }

        static void PlayerTurn()
        {
            Sprites();
            ConsoleKeyInfo cki;
            do
            {
                PlayerMenu();
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.UpArrow && playerInGameMenu != 1)
                {
                    playerInGameMenu = playerInGameMenu - 1;
                }
                if (cki.Key == ConsoleKey.DownArrow && playerInGameMenu != 5)
                {
                    playerInGameMenu = playerInGameMenu + 1;
                }
                if (cki.Key == ConsoleKey.Spacebar)
                {
                    break;
                }
                if (cki.Key == ConsoleKey.Enter)
                {
                    break;
                }
            } while (cki.Key != ConsoleKey.Spacebar || cki.Key != ConsoleKey.Enter);
            PlayerAction();
        }

        static void PlayerAction()
        {
            if (playerInGameMenu == 1)
            {
                Console.WriteLine("The player attacks");
                System.Threading.Thread.Sleep(500);
                attacking = true;
                Sprites();
                PlayerAttack();
            }
            if (playerInGameMenu == 2)
            {
                Console.WriteLine("The player blocks");
                System.Threading.Thread.Sleep(500);
                defending = true;
                Sprites();
                Defending();
            }
            if (playerInGameMenu == 3)
            {
                Console.WriteLine("The player heals");
                System.Threading.Thread.Sleep(500);
                healing = true;
                PlayerHeal();
            }
            if (playerInGameMenu == 4)
            {
                SaveFile();
                Console.WriteLine("You save the game");
                System.Threading.Thread.Sleep(500);
                PlayerTurn();
            }
            if (playerInGameMenu == 5)
            {
                Console.WriteLine("You go back to menu");
                System.Threading.Thread.Sleep(500);
                inFight = false;
                inShop = false;
                onMenu = true;
            }
        }

        static void PlayerAttack()
        {
            Random random = new Random();
            playerDamage = random.Next(5 * playerLvl, 10 * playerLvl);
            UIClear();
            Console.SetCursorPosition(8, 23);
            currentMonsterHP = currentMonsterHP - playerDamage;
            if (currentMonsterHP < 0)
            {
                currentMonsterHP = 0;
            }
            Console.WriteLine("The monster took " + playerDamage + " Damage.");
            Console.ReadKey();
            playerTurn = false;
        }

        static void PlayerHeal()
        {
            if (playerPotions > 0 && currentPlayerHP < maxPlayerHP)
            {
                Sprites();
                playerPotions = playerPotions - 1;
                Random random = new Random();
                healthPotionHeal = random.Next(0 * playerLvl, 7 * playerLvl);
                currentPlayerHP = currentPlayerHP + healthPotionHeal;
                UIClear();
                Console.SetCursorPosition(8, 23);
                Console.WriteLine("You healed " + healthPotionHeal + " Damage...");
                Console.SetCursorPosition(8, 24);
                Console.WriteLine("You have " + playerPotions + " Potions Left!");
                Console.ReadKey();
                if (healthPotionHeal == 0)
                {
                    UIClear();
                    Console.SetCursorPosition(8, 23);
                    Console.WriteLine("That health potion was a dud!");
                    Console.ReadKey();
                }
                if (currentPlayerHP > maxPlayerHP)
                {
                    UIClear();
                    currentPlayerHP = maxPlayerHP;
                    Console.SetCursorPosition(8, 23);
                    Console.WriteLine("Your health has been capped to your max health!");
                    Console.ReadKey();
                }
                playerTurn = false;
            }
            else if (currentPlayerHP == maxPlayerHP)
            {
                UIClear();
                Console.SetCursorPosition(8, 23);
                Console.WriteLine("You are max health and cannot heal anymore");
                Console.ReadKey();
                UIClear();
                healing = false;
                Sprites();
                PlayerTurn();
            }
            else if (playerPotions <= 0)
            {
                UIClear();
                Console.SetCursorPosition(8, 23);
                Console.WriteLine("You have no more potions left...");
                Console.ReadKey();
                UIClear();
                healing = false;
                Sprites();
                PlayerTurn();
            }
        }

        static void Defending()
        {
            blocking = true;
            playerTurn = false;
        }

        static void PlayerInitializer()
        {
            if (Loaded == false)
            {
                Coins = 20;
                playerLvl = 1;
                playerPotions = 3;
                maxPlayerHP = 10 * playerLvl;
                currentPlayerHP = maxPlayerHP;
            }
            else if (Loaded == true)
            {
                maxPlayerHP = 10 * playerLvl;
            }

        }

        //May conflict with loading later
        static void MonsterInitializer()
        {
            if (Loaded == false || inShop == true)
            {
                Random random = new Random();
                maxMonsterHP = random.Next(10 * playerLvl, 30 * playerLvl);
                currentMonsterHP = maxMonsterHP;
                halfMonsterHP = maxMonsterHP / 2;
            }
            else if (Loaded == true)
            {

            }
            Loaded = false;
        }

        static void MonsterTurn()
        {
            MonsterLifeCheck();
            ResetPlayerActions();
            Sprites();
            UIClear();
            if (inFight == false)
                return;
            MonsterAI();
            UIClear();
            Sprites();
            blocking = false;
        }
        
        static void MonsterAI()
        {
            Random random = new Random();
            monsterChoice = random.Next(0, 6);
            if (failedToHeal == true)
            {
                MonsterAttack();
                failedToHeal = false;
                return;
            }
            if (monsterTryingToHeal == true)
            {
                MonsterHeal();
            }
            else if (currentMonsterHP == maxMonsterHP)
            {
                MonsterAttack();
                return;
            }
            else if (monsterChoice >= 4 && currentMonsterHP >= halfMonsterHP)
            {
                MonsterAttack();
            }
            else if (monsterChoice <= 3 && currentMonsterHP >= halfMonsterHP)
            {
                MonsterHeal();
            }
            else if (monsterChoice >= 5 && currentMonsterHP < halfMonsterHP)
            {
                MonsterAttack();
            }
            else if (monsterChoice <= 4 && currentMonsterHP < halfMonsterHP)
            {
                MonsterHeal();
            }

        }

        static void MonsterAttack()
        {
            Random random = new Random();
            monsterDamage = random.Next(2 * playerLvl, 5 * playerLvl);
            if (blocking == true)
            {
                monsterDamage = monsterDamage / 2;
                currentPlayerHP = currentPlayerHP - monsterDamage;
                UIClear();
                Console.SetCursorPosition(8, 23);
                Console.WriteLine("The monster attacks your block dealing " + monsterDamage + " Damage...");
                Console.ReadKey();
            }
            else if (blocking == false)
            {
                currentPlayerHP = currentPlayerHP - monsterDamage;
                UIClear();
                Console.SetCursorPosition(8, 23);
                Console.WriteLine("The monster attacks you dealing " + monsterDamage + " Damage...");
                Console.ReadKey();
            }
            if (currentPlayerHP <= 0)
            {
                currentPlayerHP = 0;
                Sprites();
                UIClear();
                Console.SetCursorPosition(8, 23);
                Console.WriteLine("The monster has killed you...");
                Console.ReadKey();
                inFight = false;
                inShop = false;
            }
        }

        static void MonsterHeal()
        {
            if (monsterTryingToHeal == true)
            {
                if (checkheal != currentMonsterHP)
                {
                    UIClear();
                    Console.SetCursorPosition(8, 23);
                    Console.WriteLine("You stopped the monster from healing!");
                    monsterTryingToHeal = false;
                    failedToHeal = true;
                    Console.ReadKey();
                }
                else if (checkheal == currentMonsterHP)
                {
                    Random random = new Random();
                    monsterHeal = random.Next(2 * playerLvl, 10 * playerLvl);
                    currentMonsterHP = currentMonsterHP + monsterHeal;
                    monsterTryingToHeal = false;
                    if (currentMonsterHP > maxMonsterHP)
                    {
                        currentMonsterHP = maxMonsterHP;
                    }
                    UIClear();
                    Console.SetCursorPosition(8, 23);
                    Console.WriteLine("The monster healed for " + monsterHeal + " Damage...");
                    Console.ReadKey();
                }
            }
            else
            {
                checkheal = currentMonsterHP;
                monsterTryingToHeal = true;
                UIClear();
                Console.SetCursorPosition(8, 23);
                Console.WriteLine("The monster is trying to heal... Stop it!");
                Console.ReadKey();
            }
        }

        static void MonsterLifeCheck()
        {
            if (currentMonsterHP <= 0)
            {
                Sprites();
                UIClear();
                inStatScreen = true;
                monsterTryingToHeal = false;
                inFight = false;
                Console.SetCursorPosition(8, 23);
                Console.WriteLine("You have defeated the monster!");
                Console.ReadKey();
                return;
            }
        }

        static void ResetPlayerActions()
        {
            attacking = false;
            healing = false;
            defending = false;
        }

        static void SaveFile()
        {
            if (!File.Exists("Savedata.txt"))
            {
                File.Create("Savedata.txt").Close();
                using (StreamWriter sw = new StreamWriter("Savedata.txt"))
                {
                    sw.WriteLine(playerLvl.ToString());
                    sw.WriteLine(exp.ToString());
                    sw.WriteLine(currentPlayerHP.ToString());
                    sw.WriteLine(playerPotions.ToString());
                    sw.WriteLine(Coins.ToString());
                    sw.WriteLine(currentMonsterHP.ToString());
                    sw.WriteLine(maxMonsterHP.ToString());
                    sw.WriteLine(inFight.ToString());
                    sw.WriteLine(inShop.ToString());
                    sw.Close();
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter("Savedata.txt"))
                {
                    sw.WriteLine(playerLvl.ToString());
                    sw.WriteLine(exp.ToString());
                    sw.WriteLine(currentPlayerHP.ToString());
                    sw.WriteLine(playerPotions.ToString());
                    sw.WriteLine(Coins.ToString());
                    sw.WriteLine(currentMonsterHP.ToString());
                    sw.WriteLine(maxMonsterHP.ToString());
                    sw.WriteLine(inFight.ToString());
                    sw.WriteLine(inShop.ToString());
                    sw.Close();
                }
            }
        }

        static void LoadFile()
        {
            if (File.Exists("Savedata.txt"))
            {
                using (StreamReader sr = new StreamReader("Savedata.txt"))
                {
                    playerleveltemp = sr.ReadLine();
                    exptemp = sr.ReadLine();
                    currentplayerhptemp = sr.ReadLine();
                    playerpotionstemp = sr.ReadLine();
                    coinstemp = sr.ReadLine();
                    currentmonsterhptemp = sr.ReadLine();
                    maxmonsterhptemp = sr.ReadLine();
                    infighttemp = sr.ReadLine();
                    inshoptemp = sr.ReadLine();

                    errorFound = false;

                    //parses all info to usable values

                    try
                    {
                       playerLvl = int.Parse(playerleveltemp);
                    }
                    catch (FormatException)
                    {
                        Console.SetCursorPosition(16, 12);
                        Console.WriteLine("It appears your file is corrupted...");
                        Console.SetCursorPosition(16, 13);
                        Console.WriteLine("There was an issue determining your players level...");
                        return;
                    }
                    try
                    {
                       exp = int.Parse(exptemp);
                    }
                    catch (FormatException)
                    {
                        Console.SetCursorPosition(16, 12);
                        Console.WriteLine("It appears your file is corrupted...");
                        Console.SetCursorPosition(16, 13);
                        Console.WriteLine("There was an issue determining your players exp...");
                        return;
                    }
                    try
                    {
                       currentPlayerHP = int.Parse(currentplayerhptemp);
                    }
                    catch (FormatException)
                    {
                        Console.SetCursorPosition(16, 12);
                        Console.WriteLine("It appears your file is corrupted...");
                        Console.SetCursorPosition(16, 13);
                        Console.WriteLine("There was an issue determining your players current HP...");
                        return;
                    }
                    try
                    {
                      Coins = int.Parse(coinstemp);
                    }
                    catch (FormatException)
                    {
                        Console.SetCursorPosition(16, 12);
                        Console.WriteLine("It appears your file is corrupted...");
                        Console.SetCursorPosition(16, 13);
                        Console.WriteLine("There was an issue determining your players current coins...");
                        return;
                    }
                    try
                    {
                       currentMonsterHP = int.Parse(currentmonsterhptemp);
                    }
                    catch (FormatException)
                    {
                        Console.SetCursorPosition(16, 12);
                        Console.WriteLine("It appears your file is corrupted...");
                        Console.SetCursorPosition(16, 13);
                        Console.WriteLine("There was an issue determining the monsters current HP...");
                        return;
                    }
                    try
                    {
                       maxMonsterHP = int.Parse(maxmonsterhptemp);
                    }
                    catch (FormatException)
                    {
                        Console.SetCursorPosition(16, 12);
                        Console.WriteLine("It appears your file is corrupted...");
                        Console.SetCursorPosition(16, 13);
                        Console.WriteLine("There was an issue determining the monsters max HP...");
                        return;
                    }
                    ErrorChecking();
                    if (errorFound == true)
                    {
                        return;
                    }
                    if (infighttemp == "True")
                    {
                        inFight = true;
                    }
                    else if (infighttemp == "False")
                    {
                        inFight = false;
                    }
                    else
                    {
                        Console.SetCursorPosition(16, 12);
                        Console.WriteLine("It appears your file is corrupted...");
                        Console.SetCursorPosition(16, 13);
                        Console.WriteLine("There was an issue determining your location in game...");
                        onMenu = true;
                    }
                    if (inshoptemp == "True")
                    {
                        inShop = true;
                    }
                    else if (inshoptemp == "False")
                    {
                        inShop = false;
                    }
                    else
                    {
                        Console.SetCursorPosition(16, 12);
                        Console.WriteLine("It appears your file is corrupted...");
                        Console.SetCursorPosition(16, 13);
                        Console.WriteLine("There was an issue determining your location in game...");
                        onMenu = true;
                    }
                    sr.Close();
                }
                Loaded = true;
            }
            else if (!File.Exists("Savedata.txt"))
            {
                Console.SetCursorPosition(16, 12);
                Console.WriteLine("There was no save file detected please ensure you have saved before trying to load!");
            }

        }

        static void ErrorChecking()
        {
            if (playerLvl >= 11 || playerLvl <= 0)
            {
                Console.SetCursorPosition(16, 12);
                Console.WriteLine("It appears your file is corrupted...");
                Console.SetCursorPosition(16, 13);
                Console.WriteLine("There was an issue determining your players level...");
                errorFound = true;
                return;
            }
            if (exp >= 101 || exp < 0)
            {
                Console.SetCursorPosition(16, 12);
                Console.WriteLine("It appears your file is corrupted...");
                Console.SetCursorPosition(16, 13);
                Console.WriteLine("There was an issue determining your players EXP...");
                errorFound = true;
                return;
            }
            if (currentPlayerHP <= 0 || currentPlayerHP > maxPlayerHP)
            {
                Console.SetCursorPosition(16, 12);
                Console.WriteLine("It appears your file is corrupted...");
                Console.SetCursorPosition(16, 13);
                Console.WriteLine("There was an issue determining your players HP...");
                errorFound = true;
                return;
            }
            if (maxPlayerHP <= 9 || maxPlayerHP > 100)
            {
                Console.SetCursorPosition(16, 12);
                Console.WriteLine("It appears your file is corrupted...");
                Console.SetCursorPosition(16, 13);
                Console.WriteLine("There was an issue determining your players maximum HP...");
                errorFound = true;
                return;
            }
            if (Coins < 0)
            {
                Console.SetCursorPosition(16, 12);
                Console.WriteLine("It appears your file is corrupted...");
                Console.SetCursorPosition(16, 13);
                Console.WriteLine("There was an issue determining your players coins...");
                errorFound = true;
                return;
            }
            if (currentMonsterHP < 0 || currentMonsterHP > maxMonsterHP)
            {
                Console.SetCursorPosition(16, 12);
                Console.WriteLine("It appears your file is corrupted...");
                Console.SetCursorPosition(16, 13);
                Console.WriteLine("There was an issue determining the monsters current HP...");
                errorFound = true;
                return;
            }
            if (currentMonsterHP <= -1 || currentMonsterHP > maxMonsterHP)
            {
                Console.SetCursorPosition(16, 12);
                Console.WriteLine("It appears your file is corrupted...");
                Console.SetCursorPosition(16, 13);
                Console.WriteLine("There was an issue determining the monsters current HP...");
                errorFound = true;
                return;
            }
            if (maxMonsterHP < 10 || maxMonsterHP > 300)
            {
                Console.SetCursorPosition(16, 12);
                Console.WriteLine("It appears your file is corrupted...");
                Console.SetCursorPosition(16, 13);
                Console.WriteLine("There was an issue determining the monsters current HP...");
                errorFound = true;
                return;
            }
        }

        static void Sprites()
        {
            if (attacking == true)
            {
                Console.SetCursorPosition(2, 1);
                Console.WriteLine("                                                                           #@@##  ");
                Console.WriteLine("                                                                          @&#@@@@@#");
                Console.WriteLine("                                                                          #@@@@@@@");
                Console.WriteLine("                                                                           #@@@@@@");
                Console.WriteLine("                                                                          #@@@@@@@@#");
                Console.WriteLine("                                                          ######      #*  @@@@@@@@@@");
                Console.WriteLine("                                                           #####   /##    @@@@@@@@@@");
                Console.WriteLine("                                                          #########       @@@@@@@@@@");
                Console.WriteLine("                                                          #######         @@@@@@@@@@");
                Console.WriteLine("                                                          #######         @@@@@@@@@@#");
                Console.WriteLine("                                                          ######*         @@@@@@@@@@#");
                Console.WriteLine("                                                                         #@@@@@@@@@@");
                Console.WriteLine("                                                                           *#%%%###");
                System.Threading.Thread.Sleep(500);
            }
            if (healing == true)
            {
                Console.SetCursorPosition(2, 1);
                Console.WriteLine("                                     __                                    #@@##  ");
                Console.WriteLine("                                      /  \\                                @&#@@@@@#");
                Console.WriteLine("                                      |   |                               #@@@@@@@");
                Console.WriteLine("                                ______|   |______                          #@@@@@@");
                Console.WriteLine("                               (______     ______)                        #@@@@@@@@#");
                Console.WriteLine("         #####                        |   |                               @@@@@@@@@@");
                Console.WriteLine("         ######    ##*                |   |                               @@@@@@@@@@");
                Console.WriteLine("         #####  ##                    |   |                               @@@@@@@@@@");
                Console.WriteLine("        *###### #                     |   |                               @@@@@@@@@@");
                Console.WriteLine("        #######                        \\_/                                @@@@@@@@@@#");
                Console.WriteLine("        #######                                                           @@@@@@@@@@#");
                Console.WriteLine("         ######*                                                         #@@@@@@@@@@");
                Console.WriteLine("                                                                           *#%%%###");
                System.Threading.Thread.Sleep(500);
            }
            if (defending == true)
            {
                Console.SetCursorPosition(2, 1);
                Console.WriteLine("                                                                           #@@##  ");
                Console.WriteLine("                                    |`-._/\\_.-`|                          @&#@@@@@#");
                Console.WriteLine("                                    |    ||    |                          #@@@@@@@");
                Console.WriteLine("                                    |___o()o___|                           #@@@@@@");
                Console.WriteLine("                                    |__((<>))__|                          #@@@@@@@@#");
                Console.WriteLine("         #####                      \\   o\\/o   /                          @@@@@@@@@@");
                Console.WriteLine("         ######    ##*               \\   ||   /                           @@@@@@@@@@");
                Console.WriteLine("         #####  ##                    \\  ||  /                            @@@@@@@@@@");
                Console.WriteLine("        *###### #                      '.||.'                             @@@@@@@@@@");
                Console.WriteLine("        #######                          ``                               @@@@@@@@@@#");
                Console.WriteLine("        #######                                                           @@@@@@@@@@#");
                Console.WriteLine("         ######*                                                         #@@@@@@@@@@");
                Console.WriteLine("                                                                           *#%%%###");
                System.Threading.Thread.Sleep(500);
            }
            else if (healing == false && attacking == false && defending == false)
            {
                Console.SetCursorPosition(2, 1);
                Console.WriteLine("                                                                           #@@##  ");
                Console.WriteLine("                                                                          @&#@@@@@#");
                Console.WriteLine("                                                                          #@@@@@@@");
                Console.WriteLine("                                                                           #@@@@@@");
                Console.WriteLine("                                                                          #@@@@@@@@#");
                Console.WriteLine("         #####                                                            @@@@@@@@@@");
                Console.WriteLine("         ######    ##*                                                    @@@@@@@@@@");
                Console.WriteLine("         #####  ##                                                        @@@@@@@@@@");
                Console.WriteLine("        *###### #                                                         @@@@@@@@@@");
                Console.WriteLine("        #######                                                           @@@@@@@@@@#");
                Console.WriteLine("        #######                                                           @@@@@@@@@@#");
                Console.WriteLine("         ######*                                                         #@@@@@@@@@@");
                Console.WriteLine("                                                                           *#%%%###");
            }
            Console.SetCursorPosition(0, 14);
            Console.Write("                                                                                                                                                           ");
            Console.SetCursorPosition(0, 14);
            Console.Write("          " + currentPlayerHP + "/" + maxPlayerHP + "                                                              " + currentMonsterHP + "/" + maxMonsterHP);
        } 

        static void ClearConsole()
        {
            hasConsoleCleared = true;
            for (int i = 0; i < 35; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("/");
            }
            System.Threading.Thread.Sleep(100);
            for (int i = 0; i < 35; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("               /");
            }
            System.Threading.Thread.Sleep(100);
            for (int i = 0; i < 35; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("                              /");
            }
            System.Threading.Thread.Sleep(100);
            for (int i = 0; i < 35; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("                                             /");
            }
            System.Threading.Thread.Sleep(100);
            for (int i = 0; i < 35; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("                                                            /");
            }
            System.Threading.Thread.Sleep(100);
            for (int i = 0; i < 35; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("                                                                           /");
            }
            System.Threading.Thread.Sleep(100);
            for (int i = 0; i < 35; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("                                                                                          /");
            }
            System.Threading.Thread.Sleep(100);
            for (int i = 0; i < 35; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("                                                                                                         /");
            }
            System.Threading.Thread.Sleep(100);
            for (int i = 0; i < 35; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("                                                                                                                        /");
            }
            System.Threading.Thread.Sleep(100);
            for (int i = 0; i < 35; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.WriteLine("                                                                                                                         ");
            }
        }

        static void ShopUI()
        {
            if (shopUIInitialized == false)
            {
                Console.SetCursorPosition(0, 16);
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("███████████████████████████████████████████████████████████████████████████████████████████████████████████");
                shopUIInitialized = true;
            }

            if (shopanimLeft == 2)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("███████████████████████████████████████████████████████████████████████████████████████████████████████████");
                Console.WriteLine("█            | |              | |                                     | |                                 █");
                Console.WriteLine("█            | |              |_|_____________________________________|_|                                 █");
                Console.WriteLine("█           _|_|_             |                                         |                                 █");
                Console.WriteLine("█          |  +  |            |         The Only Potion's Shop          |                                 █");
                Console.WriteLine("█          | / \\ |            |_________________________________________|                                 █");
                Console.WriteLine("█          |_|_|_|                                                                                        █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                              ____                                                       █");
                Console.WriteLine("█                                             /    \\                                                      █");
                Console.WriteLine("█                                            |      |                                                     █");
                Console.WriteLine("█                                          __\\     /__                                                    █");
                Console.WriteLine("█                                        /             \\                                                  █");
                Console.WriteLine("█                                       |               |                                                 █");
                Console.WriteLine("█=========================================================================================================█");

            }
            else if (shopanimLeft == 1)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("███████████████████████████████████████████████████████████████████████████████████████████████████████████");
                Console.WriteLine("█            / /              | |                                     | |                                 █");
                Console.WriteLine("█           / /               |_|_____________________________________|_|                                 █");
                Console.WriteLine("█         _/_/_               |                                         |                                 █");
                Console.WriteLine("█        |  +  |              |         The Only Potion's Shop          |                                 █");
                Console.WriteLine("█        | / \\ |              |_________________________________________|                                 █");
                Console.WriteLine("█        |_|_|_|                                                                                          █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                              ____                                                       █");
                Console.WriteLine("█                                             /    \\                                                      █");
                Console.WriteLine("█                                            |      |                                                     █");
                Console.WriteLine("█                                          __\\     /__                                                    █");
                Console.WriteLine("█                                        /             \\                                                  █");
                Console.WriteLine("█                                       |               |                                                 █");
                Console.WriteLine("█=========================================================================================================█");

            }
            else if (shopanimLeft == 3)
            {
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("███████████████████████████████████████████████████████████████████████████████████████████████████████████");
                Console.WriteLine("█            \\ \\              | |                                     | |                                 █");
                Console.WriteLine("█             \\ \\             |_|_____________________________________|_|                                 █");
                Console.WriteLine("█             _\\_\\_           |                                         |                                 █");
                Console.WriteLine("█             |  +  |         |         The Only Potion's Shop          |                                 █");
                Console.WriteLine("█             | / \\ |         |_________________________________________|                                 █");
                Console.WriteLine("█             |_|_|_|                                                                                     █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                                                                                         █");
                Console.WriteLine("█                                              ____                                                       █");
                Console.WriteLine("█                                             /    \\                                                      █");
                Console.WriteLine("█                                            |      |                                                     █");
                Console.WriteLine("█                                          __\\     /__                                                    █");
                Console.WriteLine("█                                        /             \\                                                  █");
                Console.WriteLine("█                                       |               |                                                 █");
                Console.WriteLine("█=========================================================================================================█");

            }
        }

        static void ShopChoice()
        {
            ConsoleKeyInfo cki;
            if (animTimerStarted == false)
            {
                ShopUIAnimTimer();
                animTimerStarted = true;
            }
            do
            {

                ShopKeeperText();
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.UpArrow && shopMenu != 1)
                {
                    shopMenu = shopMenu - 1;
                }
                if (cki.Key == ConsoleKey.DownArrow && shopMenu != 4)
                {
                    shopMenu = shopMenu + 1;
                }
                if (cki.Key == ConsoleKey.Spacebar)
                {
                    break;
                }
                if (cki.Key == ConsoleKey.Enter)
                {
                    break;
                }
            } while (cki.Key != ConsoleKey.Spacebar || cki.Key != ConsoleKey.Enter);
            if (shopMenu == 1)
            {
                PurchasePotion();
            }
            else if (shopMenu == 2)
            {
                inShop = false;
                inFight = true;
                shopKeeperHasSpoke = false;
                shopUIInitialized = false;
            }
            else if (shopMenu == 3)
            {
                SaveFile();
            }
            else if (shopMenu == 4)
            {
                onMenu = true;
                inShop = false;
                shopKeeperHasSpoke = false;
                shopUIInitialized = false;
            }
        }
        
        static void ShopKeeperText()
        {
            if (shopKeeperHasSpoke == false)
            {
                Random random = new Random();
                string line;
                shopKeeperLines = random.Next(1, 5);
                if (shopKeeperLines == 1)
                {
                    Console.SetCursorPosition(28, 18);
                    line = "Buy some potions or get lost...";
                    for (int i = 0; i < line.Length; i++)
                    {
                        Console.Write(line[i]);
                        System.Threading.Thread.Sleep(1);
                    }
                }
                if (shopKeeperLines == 2)
                {
                    Console.SetCursorPosition(28, 18);
                    line = "If you got the coin, I got the potions...";
                    for (int i = 0; i < line.Length; i++)
                    {
                        Console.Write(line[i]);
                        System.Threading.Thread.Sleep(1);
                    }
                }
                if (shopKeeperLines == 3)
                {
                    Console.SetCursorPosition(28, 18);
                    line = "You look like someone I used to know... Hes dead now!";
                    for (int i = 0; i < line.Length; i++)
                    {
                        Console.Write(line[i]);
                        System.Threading.Thread.Sleep(1);
                    }
                }
                if (shopKeeperLines == 4)
                {
                    Console.SetCursorPosition(28, 18);
                    line = "You new here? No loittering, get yer' potions and get out!";
                    for (int i = 0; i < line.Length; i++)
                    {
                        Console.Write(line[i]);
                        System.Threading.Thread.Sleep(1);
                    }
                }
                if (shopKeeperLines == 5)
                {
                    Console.SetCursorPosition(28, 18);
                    line = "Don't worry, your gold is safe with me!";
                    for (int i = 0; i < line.Length; i++)
                    {
                        Console.Write(line[i]);
                        System.Threading.Thread.Sleep(1);
                    }
                }
                shopKeeperHasSpoke = true;
            }

            if (shopMenu == 1)
            {
                Console.SetCursorPosition(2, 16);
                Console.Write("→ Buy Potions");
            }
            if (shopMenu != 1)
            {
                Console.SetCursorPosition(2, 16);
                Console.Write("Buy Potions  ");
            }
            if (shopMenu == 2)
            {
                Console.SetCursorPosition(2, 18);
                Console.Write("→ Back to the fight");
            }
            if (shopMenu != 2)
            {
                Console.SetCursorPosition(2, 18);
                Console.Write("Back to the fight  ");
            }
            if (shopMenu == 3)
            {
                Console.SetCursorPosition(2, 20);
                Console.Write("→ Save Game");
            }
            if (shopMenu != 3)
            {
                Console.SetCursorPosition(2, 20);
                Console.Write("Save Game  ");
            }
            if (shopMenu == 4)
            {
                Console.SetCursorPosition(2, 22);
                Console.Write("→ Return to menu");
            }
            if (shopMenu != 4)
            {
                Console.SetCursorPosition(2, 22);
                Console.Write("Return to menu  ");
            }
        }

        static void PurchasePotion()
        {
            if (Coins >= 25)
            {
                Console.SetCursorPosition(28, 17);
                playerPotions = playerPotions + 1;
                Coins = Coins - 20;
                Console.Write("Thanks for the buisness!");
                Console.SetCursorPosition(28, 18);
                Console.Write("You have a total of " + playerPotions + " potions and " + Coins + " Coins left!        ");
            }
            else if (Coins < 24)
            {
                Console.SetCursorPosition(28, 17);
                Console.Write("Scram kid, you don't got the cash...");
                Console.SetCursorPosition(28, 18);
                Console.Write("You have a total of " + playerPotions + " potions and " + Coins + " Coins!            ");
            }
        }

        static async void ShopUIAnimTimer()
        {
            while (inShop == true)
            {
                ShopUI();
                waitingForAnim = false;
                await Task.Delay(1000);
                ShopUIAnimSwapper();
            }
        }

        static void ShopUIAnimSwapper()
        {
            if (shopanimLeft == 1)
            {
                shopanimLeft = 2;
                ShopUI();
                shopanimLeftb = true;
                waitingForAnim = true;
            }
            else if (shopanimLeft == 3)
            {
                shopanimLeft = 2;
                ShopUI();
                waitingForAnim = true;
                shopanimLeftb = false;
            }
            else if (shopanimLeft == 2)
            {
                if (shopanimLeftb == true)
                {
                    shopanimLeft = 3;
                    ShopUI();
                    waitingForAnim = true;
                }
                else if (shopanimLeftb == false)
                {
                    shopanimLeft = 1;
                    ShopUI();
                    waitingForAnim = true;
                }
            }
        }

        static void MainMenuChoice()
        {
            ConsoleKeyInfo cki;
            do
            {
                MainMenu();
                cki = Console.ReadKey();
                if (cki.Key == ConsoleKey.UpArrow && inMainMenu != 1)
                {
                    inMainMenu = inMainMenu - 1;
                }
                if (cki.Key == ConsoleKey.DownArrow && inMainMenu != 3)
                {
                    inMainMenu = inMainMenu + 1;
                }
                if (cki.Key == ConsoleKey.Spacebar)
                {
                    break;
                }
                if (cki.Key == ConsoleKey.Enter)
                {
                    break;
                }
            } while (cki.Key != ConsoleKey.Spacebar || cki.Key != ConsoleKey.Enter);
            if (inMainMenu == 1)
            {
                inFight = true;
                onMenu = false;
                hasConsoleCleared = false;
                ClearConsole();
                System.Threading.Thread.Sleep(1000);
                Console.SetCursorPosition(0, 0);
                PlayerInitializer();
            }
            else if (inMainMenu == 2)
            {
                LoadFile();
                if (Loaded == true)
                {
                    hasConsoleCleared = false;
                    onMenu = false;
                    maxPlayerHP = maxPlayerHP * playerLvl;
                    ClearConsole();
                    PlayerInitializer();
                    System.Threading.Thread.Sleep(1000);
                    Console.SetCursorPosition(0, 0);
                }
                else
                {

                }

            }
            else if (inMainMenu == 3)
            {
                Environment.Exit(0);
            }
        }

        static void AudioController()
        {
            if (onMenu == true)
            {
                SoundPlayer menu = new SoundPlayer("SoundFiles\\Menu.wav");
                menu.Play();
            }
            else if (inFight == true)
            {
                Random random = new Random();
                inFightMusic = random.Next(1, 4);

                if (inFightMusic == 1)
                {
                    SoundPlayer battle1 = new SoundPlayer("SoundFiles\\BattleTheme1.wav");
                    battle1.Play();
                }
                else if (inFightMusic == 2)
                {
                    SoundPlayer battle2 = new SoundPlayer("SoundFiles\\BattleTheme2.wav");
                    battle2.Play();
                }
                else if (inFightMusic == 3)
                {
                    SoundPlayer battle3 = new SoundPlayer("SoundFiles\\BattleTheme3.wav");
                    battle3.Play();
                }
                else if (inFightMusic == 4)
                {
                    SoundPlayer battle4 = new SoundPlayer("SoundFiles\\BattleTheme4.wav");
                    battle4.Play();
                }
            }
            else if (inShop == true)
            {
                SoundPlayer shop = new SoundPlayer("SoundFiles\\ShopTheme.wav");
                shop.Play();
            }
            else if (currentPlayerHP <= 0)
            {
                SoundPlayer death = new SoundPlayer("SoundFiles\\DeathTheme.wav");
                death.Play();
            }
        }

        static void StatScreen()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████████████████████████████████");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████████████████████████████████");
            StatAdjuster();
            inStatScreen = false;
            inShop = true;
            hasConsoleCleared = false;
            Console.ReadKey();
        }

        static void StatAdjuster()
        {
            Random random = new Random();
            if (playerLvl >= maxPlayerLvl)
            {
                Console.SetCursorPosition(10, 6);
                Console.Write("You are max level and have not been awarded anymore EXP");
            }
            else if (playerLvl < maxPlayerLvl)
            {
                recievedEXP = random.Next(20, 50);
                exp = exp + recievedEXP;
                maxPlayerHP = 10 * playerLvl;
                Console.SetCursorPosition(10, 5);
                Console.Write("You have been awarded " + recievedEXP + " exp for defeating the monster!");
                if (exp < 100)
                {
                    Console.SetCursorPosition(10, 6);
                    Console.Write("You need " + (100 - exp) + " exp to level up!");
                }
                if (exp >= 100)
                {
                    exp = exp - 100;
                    playerLvl = playerLvl + 1;
                    Console.SetCursorPosition(10, 6);
                    Console.Write("You leveled up! You need " + (100 - exp) + " exp for your next level");
                }
            }
            Console.SetCursorPosition(10, 7);
            lootedCoins = random.Next(10, 50);
            Coins = Coins + lootedCoins;
            Console.Write("You found " + lootedCoins + " Coins!");
            Console.SetCursorPosition(10, 8);
            Console.Write("You are currently level " + playerLvl + " with a total of " + Coins + " Coins and " + playerPotions + " Health potions left.");
        }

        static void GameOver()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("                                 _____  _____");
            Console.WriteLine("                                <     `/     |");
            Console.WriteLine("                                 >          (");
            Console.WriteLine("                                |   _     _  |");
            Console.WriteLine("                                |  |_) | |_) |");
            Console.WriteLine("                                |  | \\ | |   |");
            Console.WriteLine("                                |            |");
            Console.WriteLine("                 ______.______%_|            |__________  _____");
            Console.WriteLine("               _/                                       \\|     |");
            Console.WriteLine("              |               Random Adventurer                <");
            Console.WriteLine("              |_____.-._________              ____/|___________|");
            Console.WriteLine("                                | " + DateTime.Now.ToString("yyyy-MM-dd") + " |");
            Console.WriteLine("                                |  " + DateTime.Now.ToString("hh:mm:ss") + "  |");
            Console.WriteLine("                                |            |");
            Console.WriteLine("                                |            |");
            Console.WriteLine("                                |   _        <");
            Console.WriteLine("                                |__/         |");
            Console.WriteLine("                                 / `--.      |");
            Console.WriteLine("                               %|            |%");
            Console.WriteLine("                           |/.%%|          -< @%%%");
            Console.WriteLine("                           `\\%`@|     v      |@@%@%%");
            Console.WriteLine("                         .%%%@@@|%    |    % @@@%%@%%%%");
            Console.WriteLine("                    _.%%%%%%@@@@@@%%_/%\\_%@@%%@@@@@@@%%%%%%");
            Console.ReadKey();
            onMenu = true;
            inStatScreen = false;
            inShop = false;
            inFight = false;
            hasConsoleCleared = true;
        }
    

        static void MainMenu()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("       __   __    __    _______   _______  _______ .______      .__   __.      ___      __    __  .___________.");
            Console.WriteLine("      |  | |  |  |  |  /  _____| /  _____||   ____||   _  \\     |  \\ |  |     /   \\    |  |  |  | |           |");
            Console.WriteLine("      |  | |  |  |  | |  |  __  |  |  __  |  |__   |  |_)  |    |   \\|  |    /  ^  \\   |  |  |  | `---|  |----`");
            Console.WriteLine(".--.  |  | |  |  |  | |  | |_ | |  | |_ | |   __|  |      /     |  . `  |   /  /_\\  \\  |  |  |  |     |  |     ");
            Console.WriteLine("|  `--'  | |  `--'  | |  |__| | |  |__| | |  |____ |  |\\  \\----.|  |\\   |  /  _____  \\ |  `--'  |     |  |     ");
            Console.WriteLine(" \\______/   \\______/   \\______|  \\______| |_______|| _| `._____||__| \\__| /__/     \\__\\ \\______/      |__|     ");
            Console.WriteLine("                                                                                                               ");
            Console.WriteLine("");
            Console.SetCursorPosition(0, 10);
            if (inMainMenu == 1)
            {
                Console.WriteLine("→ Start Game");
            }
            if (inMainMenu != 1)
            {
                Console.WriteLine("Start Game     ");
            }
            Console.WriteLine("");
            if (inMainMenu == 2)
            {
                Console.WriteLine("→ Load Game");
            }
            if (inMainMenu != 2)
            {
                Console.WriteLine("Load Game     ");
            }
            Console.WriteLine("");
            if (inMainMenu == 3)
            {
                Console.WriteLine("→ Quit Game");
            }
            if (inMainMenu != 3)
            {
                Console.WriteLine("Quit Game     ");
            }
        }

        static void PlayerMenu()
        {
            Console.SetCursorPosition(0, 16);
            if (playerTurn == true)
            {
                Console.WriteLine("Players Turn");
            }
            if (playerTurn == false)
            {
                Console.WriteLine("Monsters Turn");
            }
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████████████████████████████████");
            Console.WriteLine("█                                                                                                         █");
        if (playerInGameMenu == 1)
            {
                Console.WriteLine("█  → Attack                                                                                               █");
            }
        if (playerInGameMenu != 1)
            {
                Console.WriteLine("█  Attack                                                                                                 █");
            }
            Console.WriteLine("█                                                                                                         █");
        if (playerInGameMenu == 2)
            {
                Console.WriteLine("█  → Block                                                                                                █");
            }
        if (playerInGameMenu != 2)
            {
                Console.WriteLine("█  Block                                                                                                  █");
            }
            Console.WriteLine("█                                                                                                         █");
        if (playerInGameMenu == 3)
            {
                Console.WriteLine("█  →  Heal                                                                                                █");
            }
        if (playerInGameMenu != 3)
            {
                Console.WriteLine("█  Heal                                                                                                   █");
            }
            Console.WriteLine("█                                                                                                         █");
        if (playerInGameMenu == 4)
            {
                Console.WriteLine("█  →  Save Game                                                                                           █");
            }
        if (playerInGameMenu != 4)
            {
                Console.WriteLine("█  Save Game                                                                                              █");
            }
            Console.WriteLine("█                                                                                                         █");
        if (playerInGameMenu == 5)
            {
                Console.WriteLine("█  →  Exit to Menu                                                                                        █");
            }
        if (playerInGameMenu != 5)
            {
                Console.WriteLine("█  Exit to Menu                                                                                           █");
            }
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████████████████████████████████");
        }



        static void UIClear()
        {
            Console.SetCursorPosition(0, 17);
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████████████████████████████████");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("█                                                                                                         █");
            Console.WriteLine("███████████████████████████████████████████████████████████████████████████████████████████████████████████");
        }

        static void FullScreen()
        {
            var hwnd = GetConsoleWindow();

            PostMessage(hwnd, WM_MOUSEWHEEL, (IntPtr)(MK_CONTROL | WHEEL_DELTA), IntPtr.Zero);


            PostMessage(hwnd, WM_KEYDOWN, (IntPtr)VK_F11, IntPtr.Zero);


        }
    }
}
