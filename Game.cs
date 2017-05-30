using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02
{
    class Game
    {
        private int m_MaxNumberOfTurns;
        private int m_CurrentTurnNumber;
        private eGameState m_CurrentGameState;
        private GameInterface m_GameInterface;

        private Game()
        {
            m_MaxNumberOfTurns = 0;
            m_CurrentTurnNumber = 0;
            m_CurrentGameState = eGameState.GameTurn;
            m_GameInterface = new GameInterface();
        }

        public static void PlayGame()
        {
            Game myGame = new Game();
            myGame.m_MaxNumberOfTurns = myGame.m_GameInterface.GetNumberOfTurnsFromPlayer();

            GameRunner(myGame);

        }

        private static void GameRunner(Game i_myGame)
        {
            while (i_myGame.m_CurrentTurnNumber < i_myGame.m_MaxNumberOfTurns)
            {
                i_myGame.m_CurrentTurnNumber++;
                string currentGuess = i_myGame.m_GameInterface.GetCurrentGuess();
                string answerForTheUser;
                eGameState currentGameState = i_myGame.CheckGuess(currentGuess, out answerForTheUser);
                bool anotherGo = true;

                switch (currentGameState)
                {
                    case eGameState.GameWin:
                        i_myGame.m_GameInterface.PrintGameWin(answerForTheUser);
                        anotherGo = i_myGame.m_GameInterface.AskIfUserWantsAnotherGo();
                        break;
                    case eGameState.GameLose:
                        i_myGame.m_GameInterface.PrintGameLose(answerForTheUser);
                        anotherGo = i_myGame.m_GameInterface.AskIfUserWantsAnotherGo();
                        break;
                    case eGameState.GameTurn:
                        i_myGame.m_GameInterface.PrintNextTurn(answerForTheUser);
                        break;
                    default:
                        break;
                }

                if (!anotherGo && (currentGameState == eGameState.GameLose || currentGameState == eGameState.GameWin))
                {
                    i_myGame.m_GameInterface.PrintByeMessege();
                }

            }
        }

        private eGameState CheckGuess(string currentGuess, out string answerForTheUser)
        {
            throw new NotImplementedException();
        }

        private enum eGameState
        {
            GameWin,
            GameLose,
            GameTurn
        }       
    }
}
