using System;
using System.Collections.Generic;

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
        private eGameState m_CurrentGameState;
        private List<char> m_ComputerGuess;
        private Board m_GameBoard;
        private bool m_GameInProgress;
     
        private Game()
        {
            m_MaxNumberOfTurns = 0;
            m_CurrentTurnNumber = 0;
            m_CurrentGameState = eGameState.GameTurn;
            m_GameBoard = new Board();
            m_GameInProgress = true;
        }

        public static void PlayGame()
        {
            Game myGame = new Game();
            myGame.m_MaxNumberOfTurns = GameInterface.GetNumberOfTurnsFromPlayer();
            myGame.SetComputerGuess();
            GameRunner(myGame);
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
            while (i_myGame.m_GameInProgress)
            {
                i_myGame.m_CurrentTurnNumber++;
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

                bool anotherGo = true;

                switch (currentGameState)
                {
                    case eGameState.GameWin:
                        i_myGame.m_GameBoard.AddNewGuessLine(currentGuess.ToString(), answerForTheUser.ToString());
                        GameInterface.PrintGameWin(i_myGame.m_GameBoard, i_myGame.m_CurrentTurnNumber);
                        anotherGo = GameInterface.AskIfUserWantsAnotherGo();
                        break;
                    case eGameState.GameLose:
                        i_myGame.m_GameBoard.AddNewGuessLine(currentGuess.ToString(), answerForTheUser.ToString());
                        GameInterface.PrintGameLose(i_myGame.m_GameBoard);
                        anotherGo = GameInterface.AskIfUserWantsAnotherGo();
                        break;
                    case eGameState.GameTurn:
                        i_myGame.m_GameBoard.AddNewGuessLine(currentGuess.ToString(), answerForTheUser.ToString());
                        GameInterface.PrintNextTurn(i_myGame.m_GameBoard);
                        break;
                    case eGameState.GameEnd:
                        GameInterface.PrintByeMessege();
                        i_myGame.m_GameInProgress = false;
                        break;
                    default:
                        break;
                }

                if (!anotherGo && (currentGameState == eGameState.GameLose || currentGameState == eGameState.GameWin))
                {
                    GameInterface.PrintByeMessege();
                    i_myGame.m_GameInProgress = false;
                }
            }
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
                if (m_ComputerGuess.IndexOf(i_currentGuess[indexOfCharacterInGuess]) != k_CharacterNotFoundInGuess && m_ComputerGuess.IndexOf(i_currentGuess[indexOfCharacterInGuess]) != indexOfCharacterInGuess)
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
            if (io_answerForTheUser.ToString().Equals(k_WinningGuess))
            {
                gameState = eGameState.GameWin;
            }
            else if(m_CurrentTurnNumber == m_MaxNumberOfTurns)
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
