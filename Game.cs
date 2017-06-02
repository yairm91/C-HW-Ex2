using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02
{
    internal class Game
    {
        private const char k_CorrectCharacterCorrectPlace = 'V';
        private const char k_CorrectCharacterIncorrectPlace = 'X';
        private const int k_CharacterNotFoundInGuess = -1;
        private const string k_WinningGuess = "VVVV";
        internal const char k_EndOfGameCharacter = 'Q';
        internal const int k_MaxLengthOfGuessWords = 4;
        private int m_MaxNumberOfTurns;
        private int m_CurrentTurnNumber;
        private List<char> m_ComputerGuess;
        private Board m_GameBoard;
        private bool m_GameInProgress;

        private Game()
        {
            m_MaxNumberOfTurns = 0;
            m_CurrentTurnNumber = 1;
            m_GameInProgress = true;
        }

        public static void PlayGame()
        {
            Game myGame = new Game();
            myGame.m_MaxNumberOfTurns = GameInterface.GetNumberOfTurnsFromPlayer();
            myGame.m_GameBoard = new Board(myGame.m_MaxNumberOfTurns);
            myGame.SetComputerGuess();
            GameRunner(myGame);
        }

        internal static string CastCharListToString(List<char> i_characterList)
        {
            StringBuilder stringFromList = new StringBuilder();
            foreach (char characterInList in i_characterList)
            {
                stringFromList.Append(characterInList);
            }

            return stringFromList.ToString();
        }

        private static List<char> CreateComputerGuess()
        {
            Random randomGenerator = new Random();
            List<char> computerGuess = new List<char>(k_MaxLengthOfGuessWords);

            for (int i = 0; i < k_MaxLengthOfGuessWords; i++)
            {
                char characterToAdd = (char)randomGenerator.Next('A', 'H' + 1);
                if (computerGuess.IndexOf(characterToAdd) == k_CharacterNotFoundInGuess)
                {
                    computerGuess.Add(characterToAdd);
                }
                else
                {
                    i--;
                }
            }

            return computerGuess;
        }

        private static void GameRunner(Game i_myGame)
        {
            GameInterface.PrintNextTurn(i_myGame.m_GameBoard);
            while (i_myGame.m_GameInProgress)
            {
                List<char> currentGuess = GameInterface.GetCurrentGuess();
                List<char> answerForTheUser = new List<char>();
                eGameState currentGameState = eGameState.GameTurn;
                if (currentGuess[0] == k_EndOfGameCharacter)
                {
                    currentGameState = eGameState.GameEnd;
                }
                else
                {
                    currentGameState = i_myGame.CheckGuess(currentGuess, out answerForTheUser);
                }

                string answerForTheUserStringFormat = correctAnswerToTheUser(answerForTheUser);

                bool anotherGo = true;

                switch (currentGameState)
                {
                    case eGameState.GameWin:
                        anotherGo = WinTheGame(i_myGame, currentGuess, answerForTheUserStringFormat);
                        break;
                    case eGameState.GameLose:
                        anotherGo = LoseTheGame(i_myGame, currentGuess, answerForTheUserStringFormat);
                        break;
                    case eGameState.GameTurn:
                        AnotherTurn(i_myGame, currentGuess, answerForTheUserStringFormat);
                        break;
                    case eGameState.GameEnd:
                        EndTheGame(i_myGame);
                        break;
                    default:
                        break;
                }

                if (!anotherGo && (currentGameState == eGameState.GameLose || currentGameState == eGameState.GameWin))
                {
                    GameInterface.PrintByeMessege();
                }
                else if (currentGameState == eGameState.GameLose || currentGameState == eGameState.GameWin)
                {
                    PlayGame();
                }

                i_myGame.m_CurrentTurnNumber++;
            }
        }

        private static void EndTheGame(Game i_myGame)
        {
            GameInterface.PrintByeMessege();
            i_myGame.m_GameInProgress = false;
        }

        private static void AnotherTurn(Game i_myGame, List<char> i_CurrentGuess, string i_AnswerForTheUserStringFormat)
        {
            i_myGame.m_GameBoard.AddNewGuessLine(CastCharListToString(i_CurrentGuess), i_AnswerForTheUserStringFormat);
            GameInterface.PrintNextTurn(i_myGame.m_GameBoard);
        }

        private static bool LoseTheGame(Game i_myGame, List<char> i_CurrentGuess, string i_AnswerForTheUserStringFormat)
        {
            bool anotherGo;
            i_myGame.m_GameBoard.AddNewGuessLine(CastCharListToString(i_CurrentGuess), i_AnswerForTheUserStringFormat);
            GameInterface.PrintGameLose(i_myGame.m_GameBoard, CastCharListToString(i_myGame.m_ComputerGuess));
            anotherGo = GameInterface.AskIfUserWantsAnotherGo();
            i_myGame.m_GameInProgress = false;
            Console.WriteLine();
            return anotherGo;
        }

        private static bool WinTheGame(Game i_myGame, List<char> i_CurrentGuess, string i_AnswerForTheUserStringFormat)
        {
            bool anotherGo;
            i_myGame.m_GameBoard.AddNewGuessLine(CastCharListToString(i_CurrentGuess), i_AnswerForTheUserStringFormat);
            GameInterface.PrintGameWin(i_myGame.m_GameBoard, i_myGame.m_CurrentTurnNumber);
            anotherGo = GameInterface.AskIfUserWantsAnotherGo();
            i_myGame.m_GameInProgress = false;
            Console.WriteLine();
            return anotherGo;
        }

        private static string correctAnswerToTheUser(List<char> i_AnswerForTheUser)
        {
            string answerForTheUserStringFormat = CastCharListToString(i_AnswerForTheUser);
            int offset = k_MaxLengthOfGuessWords - answerForTheUserStringFormat.Length;
            for (int i = 0; i < offset; i++)
            {
                answerForTheUserStringFormat += " ";
            }

            return answerForTheUserStringFormat;
        }

        private void SetComputerGuess()
        {
            List<char> computerGuess = CreateComputerGuess();
            m_ComputerGuess = computerGuess;
        }

        private eGameState CheckGuess(List<char> i_currentGuess, out List<char> io_answerForTheUser)
        {
            io_answerForTheUser = new List<char>();

            CheckCorrectCharactersCorrectPlaces(i_currentGuess, io_answerForTheUser);
            CheckCorrectCharacterIncorrectPlaces(i_currentGuess, io_answerForTheUser);

            return CheckGameState(io_answerForTheUser);
        }

        private void CheckCorrectCharacterIncorrectPlaces(List<char> i_currentGuess, List<char> io_answerForTheUser)
        {
            for (int indexOfCharacterInGuess = 0; indexOfCharacterInGuess < k_MaxLengthOfGuessWords; indexOfCharacterInGuess++)
            {
                if (m_ComputerGuess.IndexOf(i_currentGuess[indexOfCharacterInGuess]) != k_CharacterNotFoundInGuess
                    && m_ComputerGuess.IndexOf(i_currentGuess[indexOfCharacterInGuess]) != indexOfCharacterInGuess)
                {
                    io_answerForTheUser.Add(k_CorrectCharacterIncorrectPlace);
                }
            }
        }

        private void CheckCorrectCharactersCorrectPlaces(List<char> i_currentGuess, List<char> io_answerForTheUser)
        {
            for (int indexOfCharacterInGuess = 0; indexOfCharacterInGuess < k_MaxLengthOfGuessWords; indexOfCharacterInGuess++)
            {
                if (i_currentGuess[indexOfCharacterInGuess].Equals(m_ComputerGuess[indexOfCharacterInGuess]))
                {
                    io_answerForTheUser.Add(k_CorrectCharacterCorrectPlace);
                }
            }
        }

        private eGameState CheckGameState(List<char> io_answerForTheUser)
        {
            eGameState gameState = eGameState.GameTurn;
            string answerFromUserInStringFormat = CastCharListToString(io_answerForTheUser);
            if (answerFromUserInStringFormat.Equals(k_WinningGuess))
            {
                gameState = eGameState.GameWin;
            }
            else if (m_CurrentTurnNumber == m_MaxNumberOfTurns)
            {
                gameState = eGameState.GameLose;
            }

            return gameState;
        }

        private enum eGameState
        {
            GameWin,
            GameLose,
            GameTurn,
            GameEnd
        }
    }
}
