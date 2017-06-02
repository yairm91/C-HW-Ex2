using System;
using System.Text;

namespace Ex02
{
    internal class Board
    {
        private const int k_NumberOfCharsInFulline = 19;
        private const int k_LengthOfPinsField = 9;
        private const int k_LengthOfResultField = 7;
        private const int k_NumberOfColumnSepratorsBeforePinField = 1;
        private const int k_NumberOfColumnSepratorsBeforeResultField = 2;
        private StringBuilder m_CurrentBoard;
        private int m_NumberOfAllowedGuesses;
        private int m_CurrentTurnNumber;

        internal Board(int i_NumberOfGuesses)
        {
            m_NumberOfAllowedGuesses = i_NumberOfGuesses;
            m_CurrentTurnNumber = 0;
            m_CurrentBoard = getInitialBoard();
            AddNewGuessLine("####", "    ");
        }

        internal void AddNewGuessLine(string i_GuessField, string i_ResultField)
        {
            setPinsInCurrentGuess(i_GuessField);
            setResultInCurrentGuess(i_ResultField);
            m_CurrentTurnNumber++;
        }

        internal void PrintBoard()
        {
            Console.WriteLine(m_CurrentBoard.ToString());
        }

        private StringBuilder getInitialBoard()
        {
            StringBuilder InitialBoard = new StringBuilder(m_NumberOfAllowedGuesses * k_NumberOfCharsInFulline * 2);

            InitialBoard.Append(string.Format(@"|Pins:    |Result:|"));
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
            return setAFullLineWithSpecificChar('=');
        }

        private string getNewEmptyLine()
        {
            return setAFullLineWithSpecificChar(' ');
        }

        private string setAFullLineWithSpecificChar(char i_Character)
        {
            StringBuilder fullLine = new StringBuilder();
            fullLine.AppendLine();
            fullLine.Append("|");
            fullLine.Append(getPinsBlockWithChar(i_Character));
            fullLine.Append("|");
            fullLine.Append(getResultBlockWithChar(i_Character));
            fullLine.Append("|");
            return fullLine.ToString();
        }

        private void setPinsInCurrentGuess(string i_Guess)
        {
            m_CurrentBoard.Replace(
                getPinsBlockWithChar(' '),
                warpWithSpacesForPins(i_Guess),
                setBoardOffest(k_NumberOfColumnSepratorsBeforePinField),
                k_LengthOfPinsField);
        }

        private void setResultInCurrentGuess(string i_Result)
        {
            m_CurrentBoard.Replace(
                getResultBlockWithChar(' '),
                warpWithSpacesForResult(i_Result),
                setBoardOffest(k_LengthOfPinsField + k_NumberOfColumnSepratorsBeforeResultField),
                k_LengthOfResultField);
        }

        private int setBoardOffest(int i_AddToDefault)
        {
            int lengthOfLineWithNewLineSperator = k_NumberOfCharsInFulline + Environment.NewLine.Length;

            // set board offset after the default 2 first lines with the addition of i_addToDefault
            int boardOffset = i_AddToDefault + (2 * lengthOfLineWithNewLineSperator);

            boardOffset += 2 * m_CurrentTurnNumber * lengthOfLineWithNewLineSperator;

            return boardOffset;
        }

        private string warpWithSpacesForPins(string i_StringToWarp)
        {
            StringBuilder stringWithSpaces = new StringBuilder(i_StringToWarp);
            for (int i = 0; i <= i_StringToWarp.Length; i++)
            {
                stringWithSpaces.Insert(i * 2, " ");
            }

            return stringWithSpaces.ToString();
        }

        private string warpWithSpacesForResult(string i_StringToWarp)
        {
            StringBuilder stringWithSpaces = new StringBuilder(i_StringToWarp);
            for (int i = 0; i < i_StringToWarp.Length - 1; i++)
            {
                stringWithSpaces.Insert((i * 2) + 1, " ");
            }

            return stringWithSpaces.ToString();
        }

        private string getPinsBlockWithChar(char i_Character)
        {
            StringBuilder pinsBlock = new StringBuilder();

            pinsBlock.Append(i_Character, k_LengthOfPinsField);

            return pinsBlock.ToString();
        }

        private string getResultBlockWithChar(char i_Character)
        {
            StringBuilder resultBlock = new StringBuilder();

            resultBlock.Append(i_Character, k_LengthOfResultField);

            return resultBlock.ToString();
        }
    }
}
