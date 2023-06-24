namespace VoxPopuliLibrary.Engine.Font
{
    public class Word
    {

        private List<Character> characters = new List<Character>();
        private double width = 0;
        private double fontSize;

        /**
         * Create a new empty word.
         * @param fontSize - the font size of the text which this word is in.
         */
        internal Word(double fontSize)
        {
            this.fontSize = fontSize;
        }

        /**
         * Adds a character to the end of the current word and increases the screen-space width of the word.
         * @param character - the character to be added.
         */
        internal void addCharacter(Character character)
        {
            characters.Add(character);
            width += character.getxAdvance() * fontSize;
        }

        /**
         * @return The list of characters in the word.
         */
        internal List<Character> getCharacters()
        {
            return characters;
        }

        /**
         * @return The width of the word in terms of screen size.
         */
        internal double getWordWidth()
        {
            return width;
        }

    }
}
