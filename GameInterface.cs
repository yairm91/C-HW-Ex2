using System;
using System.Collections.Generic;

namespace Ex02
{
    internal class GameInterface
    {
        internal int GetNumberOfTurnsFromPlayer()
        {
            ConsoleUtils.Screen.Clear();
            Console.WriteLine("Please enter the maximum number of guess you want - between 4 and 10 and press enter.");
            string numberOfGuessAsString = Console.ReadLine();
            int numberOfGuesses = ValidateNumberOfGuesses(numberOfGuessAsString);

            return numberOfGuesses;
        }

        private int ValidateNumberOfGuesses(string i_numberOfGuessAsString)
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

        internal List<char> GetCurrentGuess()
        {
            Console.WriteLine("Please type your next guess (A B C D) or 'Q' to quit");
            string currentGuessAsString = Console.ReadLine();
            currentGuessAsString = currentGuessAsString.Replace(" ", string.Empty);
            bool isGuessValid = CheckGuessValidaity(currentGuessAsString);

            while(isGuessValid)
            {
                Console.WriteLine("This guess is invalid, try again to enter 4 letters between A and H and press enter.");
                currentGuessAsString = Console.ReadLine();
                isGuessValid = CheckGuessValidaity(currentGuessAsString);
            }

            List<char> currentGuess = new List<char>();
            currentGuess.AddRange(currentGuessAsString.ToCharArray());

            return currentGuess;
        }

        private bool CheckGuessValidaity(string currentGuessAsString)
        {
            bool isGuessValid = true;

            if(currentGuessAsString.Length != Game.MaxLengthOfGuessWords)
            {
                if(currentGuessAsString.Length != 1 || currentGuessAsString[0] != Game.EndOfGameCharacter)
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

        internal void PrintGameWin(Board i_GameBoard, int i_NumberOfGuesses)
        {
            ConsoleUtils.Screen.Clear();
            Console.WriteLine("Current board status:");
            i_GameBoard.PrintBoard();
            Console.WriteLine(string.Format("You guessed after {0} steps!", i_NumberOfGuesses));
        }

        internal void PrintGameLose(Board i_GameBoard)
        {
            ConsoleUtils.Screen.Clear();
            Console.WriteLine("Current board status:");
            i_GameBoard.PrintBoard();
            Console.WriteLine("No more guesses allowed. You Lost.");
        }

        internal void PrintNextTurn(Board i_GameBoard)
        {
            ConsoleUtils.Screen.Clear();
            Console.WriteLine("Current board status:");
            i_GameBoard.PrintBoard();
        }

        internal bool AskIfUserWantsAnotherGo()
        {
            Console.WriteLine("Would you like to start a new game? (Y/N)");
            string answerFromUser = Console.ReadLine();
            checkValidityOfAnswer(answerFromUser);

            return answerFromUser.Equals("Y");
        }

        private void checkValidityOfAnswer(string answerFromUser)
        {
            bool isAnswerValid = answerFromUser.Equals("Y") || answerFromUser.Equals("N");

            while (!isAnswerValid)
            {
                Console.WriteLine("Answer was Invalid! Please enter again (Y/N).");
                answerFromUser = Console.ReadLine();
                isAnswerValid = answerFromUser.Equals("Y") || answerFromUser.Equals("N");
            }
        }

        internal void PrintByeMessege()
        {
            ConsoleUtils.Screen.Clear();
            Console.WriteLine("Thank you for playing. Goodbye!");
        }
    }
}
