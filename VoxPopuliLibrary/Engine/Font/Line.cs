using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.Font
{
    public class Line
    {

        private double maxLength;
        private double spaceSize;

        private List<Word> words = new List<Word>();
        private double currentLineLength = 0;

        /**
         * Creates an empty line.
         * 
         * @param spaceWidth
         *            - the screen-space width of a space character.
         * @param fontSize
         *            - the size of font being used.
         * @param maxLength
         *            - the screen-space maximum length of a line.
         */
        public Line(double spaceWidth, double fontSize, double maxLength)
        {
            this.spaceSize = spaceWidth * fontSize;
            this.maxLength = maxLength;
        }

        /**
         * Attempt to add a word to the line. If the line can fit the word in
         * without reaching the maximum line length then the word is added and the
         * line length increased.
         * 
         * @param word
         *            - the word to try to add.
         * @return {@code true} if the word has successfully been added to the line.
         */
        public bool attemptToAddWord(Word word)
        {
            double additionalLength = word.getWordWidth();
            additionalLength += (words.Count > 0) ? spaceSize : 0;
            if (currentLineLength + additionalLength <= maxLength)
            {
                words.Add(word);
                currentLineLength += additionalLength;
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * @return The max length of the line.
         */
        public double getMaxLength()
        {
            return maxLength;
        }

        /**
         * @return The current screen-space length of the line.
         */
        public double getLineLength()
        {
            return currentLineLength;
        }

        /**
         * @return The list of words in the line.
         */
        public List<Word> getWords()
        {
            return words;
        }

    }
}
