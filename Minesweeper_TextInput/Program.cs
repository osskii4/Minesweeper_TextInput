using System;

namespace Minesweeper_TextInput
{
    internal class Program
    {
        static Navigator navigate = new Navigator();
        static System.Diagnostics.Stopwatch watch;

        static int BoardSize = 100;
        static char[] Arr = new char[BoardSize];  //visible array
        static char[] Arr2 = new char[BoardSize]; //invisible array 
        static bool DeveloperMode = false;       //// DEVELOPER MODE, to activate write: "devmod"
        static int InputIndex = -1; //default InputIndex 
        static int GameStatus = -1; // (-1)-game not started, 0-normal game, 1-win, 2-loss
        static int Bombs = 10;
        static int SpotsReaveled = 0; //amount of spots clicked by player default: if == 88 WIN

        static int Flags = Bombs;

        static void Main()
        {
            Console.Title = "osskii4's Minesweeper (Type Version)";
            bool exit = false;
            while (!exit)
            {
                if (GameStatus == -1)
                    arrSetUp();

                while (GameStatus < 1)
                {
                    Board();
                    PlayerInput();
                    CheckIfWin();
                }

                if (GameStatus == 2)                       // loss
                {
                    for (int i = 0; i < BoardSize; i++)
                    {
                        if (Arr2[i] == 'X') { Arr[i] = Arr2[i]; }
                    }

                    char opt = '.';
                    while (opt != 'y' && opt != 'n')
                    {
                        Board();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You've lost the game.");
                        Console.ResetColor();
                        Console.WriteLine("Try again? Y/N");


                        opt = Console.ReadKey().KeyChar;
                        switch (opt)
                        {
                            case 'y':
                                GameStatus = -1;//game not started
                                break;

                            case 'n':
                                exit = true;
                                break;

                            default:
                                break;
                        }
                    }

                }
                else if (GameStatus == 1)                  // win
                {
                    watch.Stop();


                    for (int i = 0; i < BoardSize; i++)
                    {
                        if (Arr2[i] == 'X') { Arr[i] = 'F'; }
                    }

                    char opt = '.';
                    while (opt != 'n' && opt != 'y')
                    {
                        Board();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("Congratulations!");
                        Console.Write("\tTime: ");
                        Console.WriteLine(watch.Elapsed.ToString("mm':'ss'.'ff"));
                        Console.ResetColor();
                        Console.WriteLine("Try again? Y/N");


                        opt = Console.ReadKey().KeyChar;
                        switch (opt)
                        {
                            case 'y':
                                GameStatus = -1;//game not started
                                break;

                            case 'n':
                                exit = true;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        static void arrSetUp()
        {
            Flags = 12;
            for (int i = 0; i < BoardSize; i++)
            {
                Arr[i] = '~';
                Arr2[i] = '_';
            }
            Flags = Bombs;
            SpotsReaveled = 0;
        }

        static void bombsSetUp(int startingpoint)
        { //acces through PlayerInput()

            int b = 0;
            Random random = new Random();

            do
            {
                int i = random.Next(0, BoardSize);
                if (Arr2[i] != 'X' && i != startingpoint)
                {
                    Arr2[i] = 'X';
                    b++;
                }


            } while (b < Bombs);

            GameStatus++; //set to 0 - normal game

        }

        static void numbersSetUp()
        {

            for (int i = 0; i < BoardSize; i++)
            {
                if (Arr2[i] == '_')
                {
                    int amountOfBombs = 0;
                    if (navigate.UpLeft(i) != -1 && Arr2[navigate.UpLeft(i)] == 'X') { amountOfBombs++; }
                    if (navigate.Up(i) != -1 && Arr2[navigate.Up(i)] == 'X') { amountOfBombs++; }
                    if (navigate.UpRight(i) != -1 && Arr2[navigate.UpRight(i)] == 'X') { amountOfBombs++; }
                    if (navigate.Right(i) != -1 && Arr2[navigate.Right(i)] == 'X') { amountOfBombs++; }
                    if (navigate.DownRight(i) != -1 && Arr2[navigate.DownRight(i)] == 'X') { amountOfBombs++; }
                    if (navigate.Down(i) != -1 && Arr2[navigate.Down(i)] == 'X') { amountOfBombs++; }
                    if (navigate.DownLeft(i) != -1 && Arr2[navigate.DownLeft(i)] == 'X') { amountOfBombs++; }
                    if (navigate.Left(i) != -1 && Arr2[navigate.Left(i)] == 'X') { amountOfBombs++; }

                    if (amountOfBombs > 0)
                    {
                        Arr2[i] = (char)(amountOfBombs + '0'); //converting int to char with numbers 1-8
                    }
                }
            }

        }

        static void Board() // always visible Board
        {
            Console.Clear();
            Console.WriteLine("\nFlags: " + Flags + "\n");
            Console.WriteLine("\t A B C D E F G H I J");
            Console.WriteLine("\t _ _ _ _ _ _ _ _ _ _");
            for (int i = 1; i < 11; i++)
            {
                if (i < 12)
                {
                    Console.Write(" " + i + "\t");
                }
                else { Console.Write(i + " "); }
                Console.Write("|");
                for (int j = 0; j < 10; j++)
                {
                    int index = (i - 1) * 10 + j;
                    if (Arr[index] == 'X') // adding colours to display
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("X");
                        Console.ResetColor();
                        Console.Write("|");
                    }
                    else if (Arr[index] == 'F')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(Arr[index]);
                        Console.ResetColor();
                        Console.Write("|");
                    }
                    else if (Arr[index] == '~')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.Write(Arr[index]);
                        Console.ResetColor();
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write(Arr[index] + "|");
                    }
                }
                Console.WriteLine();
            }

            if (DeveloperMode == true) //Board visible only in developer mode
            {
                Console.WriteLine("\n\n\n\t A B C D E F G H I J");
                Console.WriteLine("\t _ _ _ _ _ _ _ _ _ _");
                for (int i = 1; i < 11; i++)
                {
                    Console.WriteLine(i + "\t|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|", Arr2[10 * i - 10], Arr2[10 * i - 9], Arr2[10 * i - 8], Arr2[10 * i - 7], Arr2[10 * i - 6], Arr2[10 * i - 5], Arr2[10 * i - 4], Arr2[10 * i - 3], Arr2[10 * i - 2], Arr2[10 * i - 1]);
                }
                Console.WriteLine("\t\t\t\t\t\t\t DelevoperMode: ON");
                Console.WriteLine("\t\t\t\t\t\t\t InputIndex: " + InputIndex);
                Console.WriteLine("\t\t\t\t\t\t\t GameStatus: " + GameStatus);
                Console.WriteLine("\t\t\t\t\t\t\t SpotsReaveled: " + SpotsReaveled);
            }

        }

        static void PlayerInput()
        {

            InputIndex = -1;        //RESET OF INPUT VALUES
            bool inputFlag = false;
            int inputLetter;
            int inputNumber;


            while (InputIndex == -1 || InputIndex < 0 || InputIndex > BoardSize - 1) // 
            {
                Console.Write("Enter your type: ");
                string input = Console.ReadLine();
                if (input == "devmod")
                {
                    DeveloperMode = DeveloperMode == true ? false : true;//turning on/off developer mode in app
                    Console.WriteLine("DEVELOPER MODE: " + DeveloperMode);
                    continue;
                }
                input = input.ToUpper();

                if (input.EndsWith("F"))
                {
                    inputFlag = true;
                    input = input.Substring(0, input.Length - 1); //deletes "F" from input
                }



                if (input.Length >= 2 && char.IsLetter(input[0]) && int.TryParse(input.Substring(1), out inputNumber))
                {
                    inputLetter = (int)Convert.ToChar(input.Substring(0, 1)) - (int)'A'; //converting Upper char to number A=0 , B=1, C=2 ...

                    if (inputLetter >= 10 || inputNumber > 10) // number cannot be above InputLetter "J"(9) / InpuNumber "10"
                    {
                        Console.WriteLine("Wrong input, try again...");
                        continue;
                    }

                    InputIndex = (int)inputNumber * 10 - 10 + inputLetter; //setting inputIndex 0-99
                }
                else
                {
                    Console.WriteLine("Wrong input, try again...");
                }

            }

            if (GameStatus == -1 && inputFlag == true) //typing with a flag without already made array with bombs
            {
                return;
            }

            if (GameStatus == -1)// setting up bombs and numbers around them after first input
            {
                bombsSetUp(InputIndex);
                numbersSetUp();
                watch = System.Diagnostics.Stopwatch.StartNew();
            }

            InputSpot(InputIndex, inputFlag);

        }

        static void InputSpot(int input, bool flag)
        {
            if (flag == true && Arr[input] == '~')
            {
                Arr[input] = 'F';
                Flags--;
            }
            else if (Arr[input] == '~' && Arr2[input] == '_')
            {
                FillFlood(input);
            }
            else if (Arr[input] == '~' && Arr2[input] != 'X')
            {
                Arr[input] = Arr2[input];
            }
            else if (Arr[input] == '~' && Arr2[input] == 'X')
            {
                GameStatus = 2;
            }
            else if (Arr[input] == 'F')
            {
                Arr[input] = '~';
                Flags++;
            }
            else
            {
                Console.WriteLine("Field already revealed. Choose another one.");
                PlayerInput();
            }
        }

        static void FillFlood(int i)
        {
            if (i < 0 || i >= BoardSize || Arr2[i] != '_')
            {
                return;
            }

            if (Arr[i] == '~') //aplying to only uncovered spots
            {
                Arr[i] = Arr2[i];

                FillFlood(navigate.UpLeft(i));
                FillFlood(navigate.Up(i));
                FillFlood(navigate.UpRight(i));
                FillFlood(navigate.Right(i));
                FillFlood(navigate.DownRight(i));
                FillFlood(navigate.Down(i));
                FillFlood(navigate.DownLeft(i));
                FillFlood(navigate.Left(i));



                if (navigate.UpLeft(i) != -1 && Arr2[navigate.UpLeft(i)] != '_')
                    Arr[navigate.UpLeft(i)] = Arr2[navigate.UpLeft(i)];

                if (navigate.Up(i) != -1 && Arr2[navigate.Up(i)] != '_')
                    Arr[navigate.Up(i)] = Arr2[navigate.Up(i)];

                if (navigate.UpRight(i) != -1 && Arr2[navigate.UpRight(i)] != '_')
                    Arr[navigate.UpRight(i)] = Arr2[navigate.UpRight(i)];

                if (navigate.Right(i) != -1 && Arr2[navigate.Right(i)] != '_')
                    Arr[navigate.Right(i)] = Arr2[navigate.Right(i)];

                if (navigate.DownRight(i) != -1 && Arr2[navigate.DownRight(i)] != '_')
                    Arr[navigate.DownRight(i)] = Arr2[navigate.DownRight(i)];

                if (navigate.Down(i) != -1 && Arr2[navigate.Down(i)] != '_')
                    Arr[navigate.Down(i)] = Arr2[navigate.Down(i)];

                if (navigate.DownLeft(i) != -1 && Arr2[navigate.DownLeft(i)] != '_')
                    Arr[navigate.DownLeft(i)] = Arr2[navigate.DownLeft(i)];

                if (navigate.Left(i) != -1 && Arr2[navigate.Left(i)] != '_')
                    Arr[navigate.Left(i)] = Arr2[navigate.Left(i)];

            }
        }

        static void CheckIfWin()
        {
            SpotsReaveled = 0;
            for (int i = 0; i < BoardSize; i++)
            {
                if (Arr[i] != '~' && Arr[i] != 'X' && Arr[i] != 'F') { SpotsReaveled++; }
            }

            if (SpotsReaveled == BoardSize - Bombs)
            {
                GameStatus = 1;
            }
        }
    }

    class Navigator
    {

        public int Up(int i)
        {

            if (i < 10)
                return -1;
            else
            {
                return i - 10;
            }
        }

        public int Down(int i)
        {

            if (i > 89)
                return -1;
            else
            {
                return i + 10;
            }
        }

        public int Left(int i)
        {

            if (i % 10 == 0)
                return -1;
            else
            {
                return i - 1;
            }
        }

        public int Right(int i)
        {

            if (i % 10 == 9)
                return -1;
            else
            {
                return i + 1;
            }
        }

        public int UpLeft(int i)
        {

            if (i < 10 || i % 10 == 0)
                return -1;
            else
            {
                return i - 11;
            }
        }

        public int UpRight(int i)
        {

            if (i < 10 || i % 10 == 9)
                return -1;
            else
            {
                return i - 9;
            }
        }

        public int DownLeft(int i)
        {

            if (i > 89 || i % 10 == 0)
                return -1;
            else
            {
                return i + 9;
            }
        }

        public int DownRight(int i)
        {

            if (i > 89 || i % 10 == 9)
                return -1;
            else
            {
                return i + 11;
            }
        }




    }
}