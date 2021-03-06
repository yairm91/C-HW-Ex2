﻿using System;
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
            myGame.setComputerGuess();
            gameRunner(myGame);
        }

        internal static string CastCharListToString(List<char> i_CharacterList)
        {
            StringBuilder stringFromList = new StringBuilder();
            foreach (char characterInList in i_CharacterList)
            {
                stringFromList.Append(characterInList);
            }

            return stringFromList.ToString();
        }

        private static List<char> createComputerGuess()
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

        private static void gameRunner(Game i_MyGame)
        {
            GameInterface.PrintNextTurn(i_MyGame.m_GameBoard);
            while (i_MyGame.m_GameInProgress)
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
                    currentGameState = i_MyGame.checkGuess(currentGuess, out answerForTheUser);
                }

                string answerForTheUserStringFormat = correctAnswerToTheUser(answerForTheUser);

                bool anotherGo = true;

                switch (currentGameState)
                {
                    case eGameState.GameWin:
                        anotherGo = winTheGame(i_MyGame, currentGuess, answerForTheUserStringFormat);
                        break;
                    case eGameState.GameLose:
                        anotherGo = loseTheGame(i_MyGame, currentGuess, answerForTheUserStringFormat);
                        break;
                    case eGameState.GameTurn:
                        anotherTurn(i_MyGame, currentGuess, answerForTheUserStringFormat);
                        break;
                    case eGameState.GameEnd:
                        endTheGame(i_MyGame);
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

                i_MyGame.m_CurrentTurnNumber++;
            }
        }

        private static void endTheGame(Game i_MyGame)
        {
            GameInterface.PrintByeMessege();
            i_MyGame.m_GameInProgress = false;
        }

        private static void anotherTurn(Game i_MyGame, List<char> i_CurrentGuess, string i_AnswerForTheUserStringFormat)
        {
            i_MyGame.m_GameBoard.AddNewGuessLine(CastCharListToString(i_CurrentGuess), i_AnswerForTheUserStringFormat);
            GameInterface.PrintNextTurn(i_MyGame.m_GameBoard);
        }

        private static bool loseTheGame(Game i_MyGame, List<char> i_CurrentGuess, string i_AnswerForTheUserStringFormat)
        {
            bool anotherGo;
            i_MyGame.m_GameBoard.AddNewGuessLine(CastCharListToString(i_CurrentGuess), i_AnswerForTheUserStringFormat);
            GameInterface.PrintGameLose(i_MyGame.m_GameBoard, CastCharListToString(i_MyGame.m_ComputerGuess));
            anotherGo = GameInterface.AskIfUserWantsAnotherGo();
            i_MyGame.m_GameInProgress = false;
            Console.WriteLine();
            return anotherGo;
        }

        private static bool winTheGame(Game i_MyGame, List<char> i_CurrentGuess, string i_AnswerForTheUserStringFormat)
        {
            bool anotherGo;
            i_MyGame.m_GameBoard.AddNewGuessLine(CastCharListToString(i_CurrentGuess), i_AnswerForTheUserStringFormat);
            GameInterface.PrintGameWin(i_MyGame.m_GameBoard, i_MyGame.m_CurrentTurnNumber);
            anotherGo = GameInterface.AskIfUserWantsAnotherGo();
            i_MyGame.m_GameInProgress = false;
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

        private void setComputerGuess()
        {
            List<char> computerGuess = createComputerGuess();
            m_ComputerGuess = computerGuess;
        }

        private eGameState checkGuess(List<char> i_CurrentGuess, out List<char> io_AnswerForTheUser)
        {
            io_AnswerForTheUser = new List<char>();

            checkCorrectCharactersCorrectPlaces(i_CurrentGuess, io_AnswerForTheUser);
            checkCorrectCharacterIncorrectPlaces(i_CurrentGuess, io_AnswerForTheUser);

            return checkGameState(io_AnswerForTheUser);
        }

        private void checkCorrectCharacterIncorrectPlaces(List<char> i_CurrentGuess, List<char> io_AnswerForTheUser)
        {
            for (int indexOfCharacterInGuess = 0; indexOfCharacterInGuess < k_MaxLengthOfGuessWords; indexOfCharacterInGuess++)
            {
                if (m_ComputerGuess.IndexOf(i_CurrentGuess[indexOfCharacterInGuess]) != k_CharacterNotFoundInGuess
                    && m_ComputerGuess.IndexOf(i_CurrentGuess[indexOfCharacterInGuess]) != indexOfCharacterInGuess)
                {
                    io_AnswerForTheUser.Add(k_CorrectCharacterIncorrectPlace);
                }
            }
        }

        private void checkCorrectCharactersCorrectPlaces(List<char> i_CurrentGuess, List<char> io_AnswerForTheUser)
        {
            for (int indexOfCharacterInGuess = 0; indexOfCharacterInGuess < k_MaxLengthOfGuessWords; indexOfCharacterInGuess++)
            {
                if (i_CurrentGuess[indexOfCharacterInGuess].Equals(m_ComputerGuess[indexOfCharacterInGuess]))
                {
                    io_AnswerForTheUser.Add(k_CorrectCharacterCorrectPlace);
                }
            }
        }

        private eGameState checkGameState(List<char> io_AnswerForTheUser)
        {
            eGameState gameState = eGameState.GameTurn;
            string answerFromUserInStringFormat = CastCharListToString(io_AnswerForTheUser);
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
