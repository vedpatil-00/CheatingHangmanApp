using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hangmanApp
{
    internal class Algorithm
    {
        public static void CaseOne(string[] lines, List<char> guessesRemaining)
        {
            // Generating an integer of value between 4 and 12 for word length
            Random integerSize = new Random();
            int intRange = integerSize.Next(4, 13);
            char userGuess = ' ';

            // Diplaying empty dashes(_) equal to the number of random word chosen from array
            StringBuilder emptyDashes = new StringBuilder(intRange);
            for (int i = 0; i < intRange; i++)
                emptyDashes.Append('_');

            // creating lists that contain alphabets of correct guesses and incorrect guesses
            List<char> correctGuesses = new List<char>();
            List<char> incorrectGuesses = new List<char>();

            // lives = Total number of guesses that the user will have
            // in this case, it depends on the word length to its multiplied by 2
            int lives = intRange * 2;

            // New list containing only words with similar length as of the random integer
            List<string> sameNumOfLetters = lines.Where(x => x.Length == intRange).ToList();
            // new chosenFamily list that will be updated in the do while list. After picking largest word family.
            List<string> chosenFamily = new List<string>();
            // new list for updated words with similar index valuees of letter in words with letter instance.
            List<string> newLetterInstance = new List<string>();

            bool loopCondition;

            // adding similar elements into new list from sameNumOfLetters list.
            chosenFamily = sameNumOfLetters.ToList();

            do
            {
                // Crearing the screen to avoid repetition of the output in the console.
                Console.Clear();

                // new list that will contain words with Instances and no instances and will be renewed 
                // every time the loop runs untill exit condition is satisfied.
                List<string> letterInstance = new List<string>();
                List<string> noInstance = new List<string>();

                // Displaying the Empty dashes to the user withoud any letters in it. 
                Console.WriteLine(emptyDashes.ToString());

                // list of all alphabets displayed to the user and gets updated after every guess
                Console.WriteLine("Guesses Remaining = [" + String.Join(", ", guessesRemaining) + "]\n");
                Console.WriteLine();
                loopCondition = false;

                // Displaying the number of lives (number of available guesses)
                Console.WriteLine("Number of Guesses Remaining: " + lives);

                Console.Write("Guess a Letter:");
                //char userGuess = Console.ReadKey().KeyChar;

                // calling the UserVerificationGuess Method 
                char userVerify = UserVerificationGuess(userGuess, emptyDashes.ToString(), lives, guessesRemaining, correctGuesses, incorrectGuesses);

                // Verification of the user input
                userGuess = userVerify;
                Console.WriteLine();

                // If loop for checking if the letter guess currently has already been guessed by the user
                // if so display appropriate message to user.
                if (incorrectGuesses.Contains(userGuess) | correctGuesses.Contains(userGuess))
                {
                    Console.WriteLine("You have already guessed " + userGuess + ". Try again!");
                    Console.WriteLine();
                    Console.ReadKey();
                    loopCondition = true;
                }
                // else loop where the picking of largest family occurs if user
                // chosen letter is a new unique guess
                else
                {
                    // adding to different list based on if letter appears in word or not
                    LetterChecker(chosenFamily, letterInstance, noInstance, userGuess);

                    // Looping to segregate the word family. updating the chosenList based
                    // which word family is larger
                    if (letterInstance.Count > noInstance.Count)
                    {
                        // generating a random word from the list and getting index
                        // values of the picked letter (by user) in the word.
                        Random randomWord = new Random();
                        int randIndex = randomWord.Next(letterInstance.Count);
                        string chosenWord = letterInstance[randIndex];

                        // new list to store the index values.
                        List<int> intValues = new List<int>();
                        for (int i = 0; i < chosenWord.Length; i++)
                        {
                            if (chosenWord[i] == userGuess)
                            {
                                intValues.Add(i);
                            }
                        }
                        // samilar index values in the empty dashes filled with letter
                        for (int i = 0; i < intValues.Count; i++)
                        {
                            emptyDashes[intValues[i]] = userGuess;
                        }
                        // removing letter from list of available alphabets for next guess
                        guessesRemaining.Remove(userGuess);

                        // displaying a success message and the new updated empty dashes.
                        Console.WriteLine("YES! There was " + userGuess + " in the word.\n");
                        Console.WriteLine(emptyDashes);

                        // new list for words with the chosen index value words
                        // for example. all words with letter e in index value 4.
                        List<string> chosenIndexWords = new List<string>();

                        foreach (string word in letterInstance)
                        {
                            if (word.Split(userGuess).Length - 1 == intValues.Count)
                            {
                                int indexCount = 0;
                                for (int i = 0; i < intValues.Count; i++)
                                {
                                    if (word[intValues[i]] == userGuess)
                                    {
                                        indexCount++;
                                    }
                                }
                                if (indexCount == intValues.Count)
                                {
                                    chosenIndexWords.Add(word);
                                }
                            }
                        }
                        // clearing and updating chosenFamily with updated
                        // chosen words for the next loop iteration.
                        chosenFamily.Clear();
                        chosenFamily = chosenIndexWords;
                        chosenFamily.TrimExcess();

                        // adding the user letter guess into correct guesses.
                        correctGuesses.Add(userGuess);
                        loopCondition = true;
                        Console.ReadKey();
                    }

                    else
                    {
                        // clearing and updating chosenFamily with updated
                        // chosen words ( word family with no letter present)
                        // for the next loop iteration.
                        chosenFamily.Clear();
                        chosenFamily = noInstance;
                        chosenFamily.TrimExcess();

                        // adding guess to incorrect guess
                        // then displaying incorrect guess message
                        incorrectGuesses.Add(userGuess);
                        Console.WriteLine("NOPE! There was no " + userGuess + " in the word. Guess Again");

                        guessesRemaining.Remove(userGuess);

                        // decreasing number of lives if the picked letter is incorrect
                        lives--;
                        Console.ReadKey();
                        loopCondition = true;
                    }
                    // exit statement if either conditions are true
                    loopCondition = ExitCondition(lives, emptyDashes.ToString());
                }
            } while (loopCondition);

            // if lives = 0, displaying user an appropriate word and failed message
            // if no dashes left, congragulate.
            GameEnd(lives, emptyDashes.ToString(), chosenFamily);
        }



        public static void CaseTwo(string[] lines, List<char> guessesRemaining)
        {
            // new array with numbers from 4-12. Multiple occurance of smaller numbers
            int[] wordLengthNums = { 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 7, 7, 8, 8, 9, 10, 11, 12 };

            // Generating a random number for array index.
            // Then getting random element based on the random array index. 
            Random random = new Random();
            int randomIndex = random.Next(0, 26);
            int intRange = wordLengthNums[randomIndex];
            Console.WriteLine(intRange);

            char userGuess = ' ';


            // Diplaying empty dashes(_) equal to the number of random word chosen from array
            StringBuilder emptyDashes = new StringBuilder(intRange);
            for (int i = 0; i < intRange; i++)
                emptyDashes.Append('_');

            // creating lists that contain alphabets of correct guesses and incorrect guesses
            List<char> correctGuesses = new List<char>();
            List<char> incorrectGuesses = new List<char>();

            // lives = Total number of guesses that the user will have
            // in this case, it depends on the word length to its multiplied by 2
            int lives = intRange * 2;

            // New list containing only words with similar length as of the random integer
            List<string> sameNumOfLetters = lines.Where(x => x.Length == intRange).ToList();
            List<string> leastOccFiltered = new List<string>();

            // segregating words based on least occuring letters 
            foreach (string word in sameNumOfLetters)
            {
                if ((word.Contains('j')) | (word.Contains('q')) | (word.Contains('x')) | (word.Contains('z')))
                {
                    leastOccFiltered.Add(word);
                }
            }

            // new chosenFamily list that will be updated in the do while list. After picking largest word family.
            List<string> chosenFamily = new List<string>();
            // new list for updated words with similar index valuees of letter in words with letter instance.
            List<string> newLetterInstance = new List<string>();

            bool loopCondition;

            // adding similar elements into new list from sameNumOfLetters list.
            chosenFamily = leastOccFiltered.ToList();

            do
            {
                // Crearing the screen to avoid repetition of the output in the console.
                Console.Clear();

                // new list that will contain words with Instances and no instances and will be renewed 
                // every time the loop runs untill exit condition is satisfied.
                List<string> letterInstance = new List<string>();
                List<string> noInstance = new List<string>();

                // Displaying the Empty dashes to the user withoud any letters in it. 
                Console.WriteLine(emptyDashes.ToString());

                // list of all alphabets displayed to the user and gets updated after every guess
                Console.WriteLine("Guesses Remaining = [" + String.Join(", ", guessesRemaining) + "]\n");
                Console.WriteLine();

                loopCondition = false;

                // Displaying the number of lives (number of available guesses)
                Console.WriteLine("Number of Guesses Remaining: " + lives);

                Console.Write("Guess a Letter:");
                //userGuess = Char.ToLower(Console.ReadKey().KeyChar); 


                // calling the UserVerificationGuess Method 
                char userVerify = UserVerificationGuess(userGuess, emptyDashes.ToString(), lives, guessesRemaining, correctGuesses, incorrectGuesses);

                // Verification of the user input
                userGuess = userVerify;
                Console.WriteLine();

                // If loop for checking if the letter guess currently has already been guessed by the user
                // if so display appropriate message to user.
                if (incorrectGuesses.Contains(userGuess) | correctGuesses.Contains(userGuess))
                {
                    Console.WriteLine("You have already guessed " + userGuess + ". Try again!");
                    Console.WriteLine();
                    Console.ReadKey();
                    loopCondition = true;
                }
                // else loop where the picking of largest family occurs if user
                // chosen letter is a new unique guess
                else
                {
                    // adding to different list based on if letter appears in word or not
                    LetterChecker(chosenFamily, letterInstance, noInstance, userGuess);

                    // Looping to segregate the word family. updating the chosenList based
                    // which word family is larger
                    if (letterInstance.Count > noInstance.Count)
                    {
                        // generating a random word from the list and getting index
                        // values of the picked letter (by user) in the word.
                        Random randomWord = new Random();
                        int randIndex = randomWord.Next(letterInstance.Count);
                        string chosenWord = letterInstance[randIndex];

                        // new list to store the index values.
                        List<int> intValues = new List<int>();
                        for (int i = 0; i < chosenWord.Length; i++)
                        {
                            if (chosenWord[i] == userGuess)
                            {
                                intValues.Add(i);
                            }
                        }
                        // samilar index values in the empty dashes filled with letter
                        for (int i = 0; i < intValues.Count; i++)
                        {
                            emptyDashes[intValues[i]] = userGuess;
                        }
                        // removing letter from list of available alphabets for next guess
                        guessesRemaining.Remove(userGuess);

                        // displaying a success message and the new updated empty dashes.
                        Console.WriteLine("YES! There was " + userGuess + " in the word.\n");
                        Console.WriteLine(emptyDashes);

                        // new list for words with the chosen index value words
                        // for example. all words with letter e in index value 4.
                        List<string> chosenIndexWords = new List<string>();

                        foreach (string word in letterInstance)
                        {
                            if (word.Split(userGuess).Length - 1 == intValues.Count)
                            {
                                int indexCount = 0;
                                for (int i = 0; i < intValues.Count; i++)
                                {
                                    if (word[intValues[i]] == userGuess)
                                    {
                                        indexCount++;
                                    }
                                }
                                if (indexCount == intValues.Count)
                                {
                                    chosenIndexWords.Add(word);
                                }
                            }
                        }
                        // clearing and updating chosenFamily with updated
                        // chosen words for the next loop iteration.
                        chosenFamily.Clear();
                        chosenFamily = chosenIndexWords;
                        chosenFamily.TrimExcess();
                        lives--;
                        // adding the user letter guess into correct guesses.
                        correctGuesses.Add(userGuess);
                        loopCondition = true;
                        Console.ReadKey();
                    }

                    else
                    {
                        // clearing and updating chosenFamily with updated
                        // chosen words ( word family with no letter present)
                        // for the next loop iteration.
                        chosenFamily.Clear();
                        chosenFamily = noInstance;
                        chosenFamily.TrimExcess();

                        // adding guess to incorrect guess
                        // then displaying incorrect guess message
                        incorrectGuesses.Add(userGuess);
                        Console.WriteLine("NOPE! There was no " + userGuess + " in the word. Guess Again");

                        guessesRemaining.Remove(userGuess);

                        // decreasing number of lives if the picked letter is incorrect
                        lives--;
                        Console.ReadKey();
                        loopCondition = true;
                    }

                    // exit statement if either conditions are true, return false
                    loopCondition = ExitCondition(lives, emptyDashes.ToString());                       
                    
                }
            } while (loopCondition);

            // if lives = 0, displaying user an appropriate word and failed message. 
            GameEnd(lives, emptyDashes.ToString(), chosenFamily);

        }


        public static char UserVerificationGuess(char userInput, string emptyDashes, int lives, List<char> guessesRemaining, List<char> correctGuesses, List<char> incorrectGuesses)
        {
            bool correct = false;
            char userGuess = ' ';

            // do while loop to update the characters in the alphabet list, number of guesses remaining.
            // calling CharacterVerification method to verify guesses within.
            // helps in clearing screen after every guess to avoid a cramped screen.
            do
            {
                Console.Clear();

                Console.WriteLine();
                Console.WriteLine(emptyDashes.ToString());

                Console.WriteLine("\nCorrect Guesses = [" + String.Join(", ", correctGuesses) + "]\n");
                Console.WriteLine("Incorrect Guesses = [" + String.Join(", ", incorrectGuesses) + "]\n");

                Console.WriteLine("\nAvalaible letters = [" + String.Join(", ", guessesRemaining) + "]");
                Console.WriteLine();

                Console.WriteLine("Guesses Remaining: " + lives);

                Console.Write("\nGuess a Letter:");
                userGuess = Char.ToLower(Console.ReadKey().KeyChar);
                correct = CharacterVerification(userGuess);

                Console.WriteLine();
            }
            while (!correct);
            return userGuess;
        }

        public static bool CharacterVerification(char guess)
        {
            // method created for character verification
            // only accept aplhabets and not invalid characters.
            bool valid = false;
            guess = Char.ToLower(guess);
            ;
            if (Char.IsLetter(guess))
            {
                valid = true;
            }
            else
            {
                valid = false;
                Console.WriteLine("\nPlease enter a valid letter!");
                Console.ReadKey();
            }
            return valid;
        }

        // check letter if occuring in a word.
        public static void LetterChecker(List<string> chosenFamily, List<string> letterInstance, List<string> noInstance, char userGuess)
        {
            foreach (string word in chosenFamily)
            {
                if (word.Contains(userGuess.ToString().ToLower()))
                {
                    letterInstance.Add(word);
                }
                else
                {
                    noInstance.Add(word);
                }
            }
        }

        // Exit Condition to exit while loop
        public static bool ExitCondition(int lives, string emptyDashes)
        {
            if (lives == 0 | !emptyDashes.Contains('_'))
            {
                return false;
            }
            return true;
        }

        // Game End Method, DIsplay message if won or lost
        public static void GameEnd(int lives, string emptyDashes, List<string> chosenFamily)
        {
            if (lives == 0)
            {
                Random fw = new Random();
                int final = fw.Next(chosenFamily.Count);
                string finalWord = chosenFamily[final];

                Console.WriteLine("\nThe Chosen Word to guess was " + finalWord);
                Console.WriteLine("\nSorry You have lost! Better Luck Next Time :)");
            }

            // successful message printed if user wins game.
            if (!emptyDashes.ToString().Contains('_'))
            {
                Console.WriteLine("\nHURAAY! You have won. You managed to beat the Hangman Game. CONGRAGULATIONS!");
            }
        }
    }
}
