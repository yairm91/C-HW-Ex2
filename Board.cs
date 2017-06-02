using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02
{
    class Board
    {
        private StringBuilder m_CurrentBoard;
        private int m_NumberOfAllowedGuesses;
        private int m_CurrentTurnNumber;
        const int numberOfCharsInFulline = 19;
        const int lengthOfPinsField = 9;
        const int lengthOfResultField = 7;
        const int numberOfColumSeperatorInLine = 3;

        internal Board(int i_NumberOfGuesses)
        {
            m_NumberOfAllowedGuesses = i_NumberOfGuesses;
            m_NumberOfGussesUntilCurrent = 0;
            m_CurrentBoard = getInitialBoard();
            AddNewGuessLine("####", "    ");

        }

        internal void AddNewGuessLine(string i_GuessField, string i_ResultField)
        {
            setPinsInCurrentGuess(i_GuessField);
            setResultInCurrentGuess(i_ResultField);
            m_CurrentTurnNumber++;
        }

        internal void printBoard()
        {
            Console.WriteLine(m_CurrentBoard.ToString);
        }

        private StringBuilder getInitialBoard()
        {
            StringBuilder InitialBoard = new StringBuilder(m_NumberOfAllowedGuesses * numberOfCharsInFulline * 2);

            InitialBoard.Append(String.Format("@|Pins:   |Result:|"));
            InitialBoard.Append(getNewLineSeperator());
            for (int i = 0; i < m_NumberOfAllowedGuesses + 1; i++)
            {
                InitialBoard.Append(getNewEmptyLine());
                InitialBoard.Append(getNewLineSeperator());
            }

            return InitialBoard;
        }
  
        private string getNewLineSeperator()
        {
            return setAFullLineWithSpecificChar("=");
        }

        private string getNewEmptyLine()
        {
            return setAFullLineWithSpecificChar(" ");
        }

        private string setAFullLineWithSpecificChar(char i_Char)
        {
            StringBuilder fullLine = new StringBuilder();
            fullLine.AppendLine();
            fullLine.Append("|");
            fullLine.Append(getPinsBlockWithChar(i_Char));
            fullLine.Append("|");
            fullLine.Append(getResultBlockWithChar(i_Char));
            fullLine.Append("|");
            return fullLine.ToString;
        }

        private void setPinsInCurrentGuess(string i_Guess)
        {
            m_CurrentBoard.Replace(getPinsBlockWithChar(" "), warpWithSpaces(i_Guess), setBoardOffest(numberOfColumSeperatorInLine - 2), lengthOfPinsField);
        }

        private void setResultInCurrentGuess(string i_Result)
        {
            m_CurrentBoard.Replace(getResultBlockWithChar(" "), warpWithSpaces(i_Result) , setBoardOffest(numberOfColumSeperatorInLine - 1), 9);
        }

        private int setBoardOffest(int i_AddToDefault)
        {
            int lengthOfLineWithNewLineSperator = numberOfCharsInFulline + Environment.NewLine.Length;
            // set board offset after the default 2 first lines with the addition of i_addToDefault
            int boardOffset = i_AddToDefault + 2 * lengthOfLineWithNewLineSperator;

            boardOffset += 2 * m_CurrentTurnNumber * lengthOfLineWithNewLineSperator;

            return boardOffset;
        }
        
        private string warpWithSpaces(string i_StringToWarp)
        {
            StringBuilder stringWithSpaces = new StringBuilder(i_StringToWarp);
            for (int i = 0; i <= i_StringToWarp.length; i++)
            {
                stringWithSpaces.Insert(i * 2, " ");
            }
            return stringWithSpaces.ToString;
        }

        private string getPinsBlockWithChar(char i_Char)
        {
            StringBuilder pinsBlock = new StringBuilder();

            pinsBlock.Append(i_Char, lengthOfPinsField);

            return pinsBlock.ToString;
        }

        private string getResultBlockWithChar(har i_Char)
        {
            StringBuilder resultBlock = new StringBuilder();

            resultBlock.Append(i_Char, lengthOfResultField);

            return resultBlock.ToString;
        }
    }
}
