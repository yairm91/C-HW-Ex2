using System;
using System.Collections.Generic;

namespace Ex02
{
    internal class Game
    {
        private const char CorrectCharacterCorrectPlace = 'V';
        private const char CorrectCharacterIncorrectPlace = 'X';
        private const int CharacterNotFoundInGuess = -1;
        private const string WinningGuess = "VVVV";
        internal const char EndOfGameCharacter = 'Q';
        internal const int MaxLengthOfGuessWords = 4;
        private int m_MaxNumberOfTurns;
        private int m_CurrentTurnNumber;
        private eGameState m_CurrentGameState;
        private GameInterface m_GameInterface;
        private List<char> m_ComputerGuess;
        private Board m_GameBoard;
        private bool m_GameInProgress;
     
        private Game()
        {
            m_MaxNumberOfTurns = 0;
            m_CurrentTurnNumber = 0;
            m_CurrentGameState = eGameState.GameTurn;
            m_GameInterface = new GameInterface();
            m_GameBoard = new Board();
            m_GameInProgress = true;
        }

        public static void PlayGame()
        {
            Game myGame = new Game();
            myGame.m_MaxNumberOfTurns = myGame.m_GameInterface.GetNumberOfTurnsFromPlayer();
            myGame.SetComputerGuess();
            GameRunner(myGame);
        }

        private static List<char> CreateComputerGuess()
        {
            Random randomGenerator = new Random();
            List<char> computerGuess = new List<char>(MaxLengthOfGuessWords);

            for (int i = 0; i < MaxLengthOfGuessWords; i++)
            {
                char characterToAdd = (char)randomGenerator.Next('A', 'H' + 1);
                if (computerGuess.IndexOf(characterToAdd) == CharacterNotFoundInGuess)
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
                List<char> currentGuess = i_myGame.m_GameInterface.GetCurrentGuess();
                List<char> answerForTheUser = new List<char>();
                eGameState currentGameState = eGameState.GameTurn;
                if (currentGuess[0] == EndOfGameCharacter)
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
                        i_myGame.m_GameInterface.PrintGameWin(i_myGame.m_GameBoard, i_myGame.m_CurrentTurnNumber);
                        anotherGo = i_myGame.m_GameInterface.AskIfUserWantsAnotherGo();
                        break;
                    case eGameState.GameLose:
                        i_myGame.m_GameBoard.AddNewGuessLine(currentGuess.ToString(), answerForTheUser.ToString());
                        i_myGame.m_GameInterface.PrintGameLose(i_myGame.m_GameBoard);
                        anotherGo = i_myGame.m_GameInterface.AskIfUserWantsAnotherGo();
                        break;
                    case eGameState.GameTurn:
                        i_myGame.m_GameBoard.AddNewGuessLine(currentGuess.ToString(), answerForTheUser.ToString());
                        i_myGame.m_GameInterface.PrintNextTurn(i_myGame.m_GameBoard);
                        break;
                    case eGameState.GameEnd:
                        i_myGame.m_GameInterface.PrintByeMessege();
                        i_myGame.m_GameInProgress = false;
                        break;
                    default:
                        break;
                }

                if (!anotherGo && (currentGameState == eGameState.GameLose || currentGameState == eGameState.GameWin))
                {
                    i_myGame.m_GameInterface.PrintByeMessege();
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
            for (int indexOfCharacterInGuess = 0; indexOfCharacterInGuess < MaxLengthOfGuessWords; indexOfCharacterInGuess++)
            {
                if (m_ComputerGuess.IndexOf(i_currentGuess[indexOfCharacterInGuess]) != CharacterNotFoundInGuess && m_ComputerGuess.IndexOf(i_currentGuess[indexOfCharacterInGuess]) != indexOfCharacterInGuess)
                {
                    io_answerForTheUser.Add(CorrectCharacterIncorrectPlace);
                }
            }
        }

        private void CheckCorrectCharactersCorrectPlaces(List<char> i_currentGuess, List<char> io_answerForTheUser)
        {
            for (int indexOfCharacterInGuess = 0; indexOfCharacterInGuess < MaxLengthOfGuessWords; indexOfCharacterInGuess++)
            {
                if (i_currentGuess[indexOfCharacterInGuess].Equals(m_ComputerGuess[indexOfCharacterInGuess]))
                {
                    io_answerForTheUser.Add(CorrectCharacterCorrectPlace);
                }
            }
        }

        private eGameState CheckGameState(List<char> io_answerForTheUser)
        {
            eGameState gameState = eGameState.GameTurn;
            if (io_answerForTheUser.ToString().Equals(WinningGuess))
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
