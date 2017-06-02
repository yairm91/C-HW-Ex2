using System;
using System.Collections.Generic;

namespace Ex02
{
    internal static class GameInterface
    {
        internal static int GetNumberOfTurnsFromPlayer()
        {
            ConsoleUtils.Screen.Clear();
            Console.WriteLine("Please enter the maximum number of guess you want - between 4 and 10 and press enter.");
            string numberOfGuessAsString = Console.ReadLine();
            int numberOfGuesses = ValidateNumberOfGuesses(numberOfGuessAsString);

            return numberOfGuesses;
        }

        private static int ValidateNumberOfGuesses(string i_numberOfGuessAsString)
        {
            int numberOfGuesses;
            bool didParseWork = int.TryParse(i_numberOfGuessAsString, out numberOfGuesses);

            while (!didParseWork || numberOfGuesses < 4 || numberOfGuesses > 10)
            {
                Console.WriteLine("Invalid number of guesses, please try again to enter a valid number and press enter:");
                i_numberOfGuessAsString = Console.ReadLine();
                didParseWork = int.TryParse(i_numberOfGuessAsString, out numberOfGuesses);
            }

            return numberOfGuesses;
        }

        internal static List<char> GetCurrentGuess()
        {
            Console.WriteLine("Please type your next guess (A B C D) or 'Q' to quit");
            string currentGuessAsString = Console.ReadLine();
            currentGuessAsString = currentGuessAsString.Replace(" ", string.Empty).ToUpper();
            bool isGuessValid = CheckGuessValidaity(currentGuessAsString);

            while (!isGuessValid)
            {
                Console.WriteLine("This guess is invalid, try again to enter 4 letters between A and H and press enter.");
                currentGuessAsString = Console.ReadLine();
                isGuessValid = CheckGuessValidaity(currentGuessAsString);
            }

            List<char> currentGuess = new List<char>();
            currentGuess.AddRange(currentGuessAsString.ToCharArray());

            return currentGuess;
        }

        private static bool CheckGuessValidaity(string currentGuessAsString)
        {
            bool isGuessValid = true;

            if (currentGuessAsString.Length != Game.k_MaxLengthOfGuessWords)
            {
                if (currentGuessAsString.Length != 1 || currentGuessAsString != Game.k_EndOfGameCharacter.ToString().ToUpper())
                {
                    isGuessValid = false;
                }
            }
            else
            {
                foreach (char characterInGuess in currentGuessAsString.ToUpper().ToCharArray())
                {
                    if (characterInGuess > 'H' || characterInGuess < 'A')
                    {
                        isGuessValid = false;
                    }
                }
            }

            return isGuessValid;
        }

        internal static void PrintGameWin(Board i_GameBoard, int i_NumberOfGuesses)
        {
            ConsoleUtils.Screen.Clear();
            Console.WriteLine("Current board status:");
            i_GameBoard.PrintBoard();
            Console.WriteLine(string.Format("You guessed after {0} steps!", i_NumberOfGuesses));
        }

        internal static void PrintGameLose(Board i_GameBoard, string i_ComputerGuess)
        {
            ConsoleUtils.Screen.Clear();
            Console.WriteLine("Current board status:");
            i_GameBoard.PrintBoard();
            Console.WriteLine("No more guesses allowed. You Lost.");
            Console.WriteLine(string.Format("The word was: {0}", i_ComputerGuess));
        }

        internal static void PrintNextTurn(Board i_GameBoard)
        {
            ConsoleUtils.Screen.Clear();
            Console.WriteLine("Current board status:");
            i_GameBoard.PrintBoard();
        }

        internal static bool AskIfUserWantsAnotherGo()
        {
            Console.WriteLine("Would you like to start a new game? (Y/N)");
            string answerFromUser = Console.ReadLine();
            string validAnswerFromUser;
            checkValidityOfAnswer(answerFromUser, out validAnswerFromUser);

            return validAnswerFromUser.ToUpper().Equals("Y");
        }

        private static void checkValidityOfAnswer(string i_AnswerFromUser, out string o_ValidAnswerFromUser)
        {
            o_ValidAnswerFromUser = string.Copy(i_AnswerFromUser);
            bool isAnswerValid = o_ValidAnswerFromUser.ToUpper().Equals("Y") || o_ValidAnswerFromUser.ToUpper().Equals("N");

            while (!isAnswerValid)
            {
                Console.WriteLine("Answer was Invalid! Please enter again (Y/N).");
                o_ValidAnswerFromUser = Console.ReadLine();
                isAnswerValid = o_ValidAnswerFromUser.ToUpper().Equals("Y") || o_ValidAnswerFromUser.ToUpper().Equals("N");
            }
        }

        internal static void PrintByeMessege()
        {
            ConsoleUtils.Screen.Clear();
            Console.WriteLine("Thank you for playing. Goodbye!");
        }
    }
}
