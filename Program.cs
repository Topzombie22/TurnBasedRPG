using System;

namespace TurnBasedRPG
{
    class Program
    {
        static int maxMonsterHP;
        static int currentMonsterHP;
        static int monsterDamage;
        static int maxPlayerHP;
        static int currentPlayerHP;
        static int playerDamage;
        static int healthPotionHeal;
        static int blockReduction;
        static int playerLvl = 2;
        static bool onMenu;
        static bool inGame;
        static bool gameOver;
        static bool inFight = true;
        static bool Loaded;
        static bool playerTurn;
        static bool monsterTurn;
        static bool shopping;
        static bool attacking;
        static bool defending;
        static bool healing;

        static int playerInGameMenu = 1;


        static void Main(string[] args)
        {
            GameLoop();
            Console.WriteLine("FightDome...");
            Console.ReadKey();
        }

        static void GameLoop()
        {
            MonsterInitializer();
            Sprites();
            while (inFight == true)
            {
                PlayerTurn();
            } 
        }

        static void PlayerTurn()
        {
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
            attacking = false;
            healing = false;
            defending = false;
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
            }
            if (playerInGameMenu == 3)
            {
                Console.WriteLine("The player heals");
                System.Threading.Thread.Sleep(500);
                healing = true;
                Sprites();
            }
            if (playerInGameMenu == 4)
            {
                Console.WriteLine("You save the game");
                System.Threading.Thread.Sleep(500);
            }
            if (playerInGameMenu == 5)
            {
                Console.WriteLine("You go back to menu");
                System.Threading.Thread.Sleep(500);
                gameOver = true;
            }
        }

        static void PlayerAttack()
        {
            currentMonsterHP = currentMonsterHP - playerDamage;
            if (currentMonsterHP < 0)
            {
                currentMonsterHP = 0;
            }
            Console.WriteLine("The monster took " + playerDamage + " Damage.");
            Console.WriteLine("The monster has " + currentMonsterHP + " Hp left.");
            System.Threading.Thread.Sleep(500);
        }

        //May conflict with loading later
        static void MonsterInitializer()
        {
            if (Loaded == false)
            {
                Random random = new Random();
                maxMonsterHP = random.Next(10 * playerLvl, 30 * playerLvl);
                currentMonsterHP = maxMonsterHP;
                monsterDamage = random.Next(2 * playerLvl, 5 * playerLvl);
            }
            else if (Loaded == true)
            {

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

            }
            else
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
        }

        static void PlayerMenu()
        {
            Console.SetCursorPosition(0, 16);
            Console.WriteLine("Players Turn");
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
    }
}
