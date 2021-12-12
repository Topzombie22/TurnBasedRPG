using System;

namespace TurnBasedRPG
{
    class Program
    {
        static int maxMonsterHP;
        static int halfMonsterHP;
        static int currentMonsterHP;
        static int monsterDamage;
        static int monsterHeal;
        static int maxPlayerHP = 20;
        static int currentPlayerHP = 10;
        static int playerDamage = 10;
        static int healthPotionHeal;
        static int blockReduction;
        static int playerLvl = 2;
        static int playerPotions = 3;
        static int monsterChoice;
        static bool onMenu;
        static bool inGame;
        static bool monsterTryingToHeal;
        static bool gameOver;
        static bool inFight = true;
        static bool inStatScreen;
        static bool inShop;
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
                MonsterTurn();
            } 
            while (inFight == false && inStatScreen == true)
            {

            }
            while (inFight == false && inStatScreen == false && inShop == true)
            {

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
                Console.WriteLine("You save the game");
                System.Threading.Thread.Sleep(500);
                PlayerTurn();
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
                Random random = new Random();
                healthPotionHeal = random.Next(0 * playerLvl, 7 * playerLvl);
                currentPlayerHP = currentPlayerHP + healthPotionHeal;
                UIClear();
                Console.SetCursorPosition(8, 23);
                Console.WriteLine("You healed " + healthPotionHeal + " Damage...");
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
                halfMonsterHP = maxMonsterHP / 2;
                monsterDamage = random.Next(2 * playerLvl, 5 * playerLvl);
            }
            else if (Loaded == true)
            {

            }
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
        }
        
        static void MonsterAI()
        {
            Random random = new Random();
            monsterChoice = random.Next(1, 6);
            if (monsterTryingToHeal == true)
            {
                MonsterHeal();
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
            currentPlayerHP = currentPlayerHP - monsterDamage;
            UIClear();
            Console.SetCursorPosition(8, 23);
            Console.WriteLine("The monster attacks you dealing " + monsterDamage + "Damage...");
            Console.ReadKey();
        }

        static void MonsterHeal()
        {
            if (monsterTryingToHeal == true)
            {
                Random random = new Random();
                monsterHeal = random.Next(2 * playerLvl, 10 * playerLvl);
                currentMonsterHP = currentMonsterHP + monsterHeal;
                monsterTryingToHeal = false;
            }
            else
            {
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
                inFight = false;
                return;
            }
        }

        static void ResetPlayerActions()
        {
            attacking = false;
            healing = false;
            defending = false;
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
    }
}
