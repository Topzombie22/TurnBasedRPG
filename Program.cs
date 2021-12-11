using System;

namespace TurnBasedRPG
{
    class Program
    {
        static int maxMonsterHP;
        static int currentMonsterHP;
        static int monsterDamage;
        static int maxPlayerHP = 20;
        static int currentPlayerHP = 10;
        static int playerDamage;
        static int healthPotionHeal;
        static int blockReduction;
        static int playerLvl = 2;
        static int playerPotions = 3;
        static bool onMenu;
        static bool inGame;
        static bool gameOver;
        static bool inFight = true;
        static bool Loaded;
        static bool playerTurn = true;
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
            attacking = false;
            healing = false;
            defending = false;
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
            playerTurn = false;
        }

        static void PlayerHeal()
        {
            if (playerPotions > 0 && currentPlayerHP < maxPlayerHP)
            {
                Sprites();
                Random random = new Random();
                healthPotionHeal = random.Next(0 * playerLvl, 7 * playerLvl);
                currentPlayerHP = currentPlayerHP + healthPotionHeal;
                if (healthPotionHeal == 0)
                {
                    Console.SetCursorPosition(0, 15);
                    Console.WriteLine("That health potion was a dud!");
                }
                if (currentPlayerHP > maxPlayerHP)
                {
                    currentPlayerHP = maxPlayerHP;
                    Console.SetCursorPosition(0, 15);
                    Console.WriteLine("Your health has been capped to your max health!");
                }
                playerTurn = false;
            }
            else if (currentPlayerHP == maxPlayerHP)
            {
                Console.SetCursorPosition(0, 15);
                Console.WriteLine("You are max health and cannot heal anymore");
            }
            else if (playerPotions <= 0)
            {
                Console.SetCursorPosition(0, 15);
                Console.WriteLine("You have no more potions left...");
            }
        }

        static void Defending()
        {
            playerTurn = false;
        }

        static void PlayerBlocking()
        {

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
        }

        static void PlayerMenu()
        {
            Console.SetCursorPosition(0, 14);
            Console.Write("          " + currentPlayerHP + "/" + maxPlayerHP + "                                                              " + currentMonsterHP + "/" + maxMonsterHP);

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
    }
}
