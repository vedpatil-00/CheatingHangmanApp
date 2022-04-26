using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hangmanApp
{
    public static class GameEngine
    {
        // char list for storing all alphabets that will show users the letters available, correct guesses letters and incorrect guesses letters.
        public static List<char> guessesRemaining = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        // Reading the dictionary text file in the workspace
        public static void Dictionary()
        {
            //char input;
            // reading the dictionary file into the program that contains over 120k words
            string[] lines = Properties.Resources.dictionary.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }

            GameMenu(lines);
        }

        // method for the Game Menu Screen 
        public static void GameMenu(string[] lines)
        {
            char input;
            
            // initialising a do while loop for the menu page 
            do
            {
                Console.Clear();
                Console.WriteLine("Hello, Welcome to the Hangman Game.\n\nPlease select the difficulty option below and press enter:");
                Console.WriteLine();
                Console.WriteLine("1.) EASY");
                Console.WriteLine("2.) HARD");
                Console.WriteLine("3.) EXIT");
                Console.WriteLine();
                input = Console.ReadKey().KeyChar;

                switch (input)
                {
                    // DIFFICULTY LEVEL EASY CODE
                    case '1':

                        // intro message for easy version of the game
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Hello User.");
                        Console.WriteLine("\nWelcome to the Easy Game Mode of the Hangman Game.");
                        Console.WriteLine("\nPress any key to proceed ahead and begin the Hangman Game");
                        Console.ReadKey();
                        Console.ForegroundColor = ConsoleColor.White;
                        // calling class for easy algorithm
                        Algorithm.CaseOne(lines, guessesRemaining);
                        Console.ReadKey();
                        break;

                    // DIFFICULTY LEVEL HARD
                    case '2':
                        // intro message for hard version of the game
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Hello User.");
                        Console.WriteLine("\nWelcome to the Hard Game Mode of the Hangman Game.");
                        Console.WriteLine("\nPress any key to proceed ahead and begin the Hangman Game");
                        Console.ReadKey();
                        Console.ForegroundColor = ConsoleColor.White;
                        // calling class for hard algorith
                        Algorithm.CaseTwo(lines, guessesRemaining);
                        Console.ReadKey();
                        break;

                    // EXIT PROGRAM
                    case '3':
                        Environment.Exit(0);
                        break;

                    default:                        
                        Console.WriteLine("\nYou did not select a valid option. Please select option '1', '2' or '3'.");
                        Console.WriteLine();
                        break;
                }
            }
            while (!Char.IsDigit(input));
        }       
    }
}
