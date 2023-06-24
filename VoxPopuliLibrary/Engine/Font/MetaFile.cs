using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.Font
{
    public class MetaFile
    {

        private static int PAD_TOP = 0;
        private static int PAD_LEFT = 1;
        private static int PAD_BOTTOM = 2;
        private static int PAD_RIGHT = 3;

        private static int DESIRED_PADDING = 3;

        private static string SPLITTER = " ";
	    private static string NUMBER_SEPARATOR = ",";

	    private double aspectRatio;

        private double verticalPerPixelSize;
        private double horizontalPerPixelSize;
        private double spaceWidth;
        private int[] padding;
        private int paddingWidth;
        private int paddingHeight;

        private Dictionary<int, Character> metaData = new Dictionary<int, Character>();

        private StreamReader reader;
        private Dictionary<string, string> values = new Dictionary<string, string>();

        /**
         * Opens a font file in preparation for reading.
         * 
         * @param file
         *            - the font file.
         */
        internal MetaFile(string file)
        {
            this.aspectRatio = (double)API.API.WindowWidth() / (double)API.API.WindowHeight();
            OpenFile(file);
            loadPaddingData();
            loadLineSizes();
            int imageWidth = GetValueOfVariable("scaleW");
            loadCharacterData(imageWidth);
            close();
        }

        internal double getSpaceWidth()
        {
            return spaceWidth;
        }

        internal Character getCharacter(int ascii)
        {
            return metaData[ascii];
        }

        /**
         * Read in the next line and store the variable values.
         * 
         * @return {@code true} if the end of the file hasn't been reached.
         */
        private bool processNextLine()
        {
            values.Clear();
            string line = null;
            try
            {
                line = reader.ReadLine();
            }
            catch (IOException e1)
            {
            }
            if (line == null)
            {
                return false;
            }
            foreach (string part in line.Split(SPLITTER))
            {
                String[] valuePairs = part.Split("=");
                if (valuePairs.Length == 2)
                {
                    values.Add(valuePairs[0], valuePairs[1]);
                }
            }
            return true;
        }

        /**
         * Gets the {@code int} value of the variable with a certain name on the
         * current line.
         * 
         * @param variable
         *            - the name of the variable.
         * @return The value of the variable.
         */
        private int GetValueOfVariable(string variable)
        {
            return int.Parse(values[variable]);
        }

        /**
         * Gets the array of ints associated with a variable on the current line.
         * 
         * @param variable
         *            - the name of the variable.
         * @return The int array of values associated with the variable.
         */
        private int[] getValuesOfVariable(string variable)
        {
            String[] numbers = values[variable].Split(NUMBER_SEPARATOR);
            int[] actualValues = new int[numbers.Length];
            for (int i = 0; i < actualValues.Length; i++)
            {
                actualValues[i] = int.Parse(numbers[i]);
            }
            return actualValues;
        }

        /**
         * Closes the font file after finishing reading.
         */
        private void close()
        {
            try
            {
                reader.Close();
            }
            catch (IOException e)
            {
                throw e;
            }
        }

        /**
         * Opens the font file, ready for reading.
         * 
         * @param file
         *            - the font file.
         */
        private void OpenFile(string file)
        {
            try
            {
                reader = new StreamReader(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Couldn't read font meta file!");
            }
        }

        /**
         * Loads the data about how much padding is used around each character in
         * the texture atlas.
         */
        private void loadPaddingData()
        {
            processNextLine();
            this.padding = getValuesOfVariable("padding");
            this.paddingWidth = padding[PAD_LEFT] + padding[PAD_RIGHT];
            this.paddingHeight = padding[PAD_TOP] + padding[PAD_BOTTOM];
        }

        /**
         * Loads information about the line height for this font in pixels, and uses
         * this as a way to find the conversion rate between pixels in the texture
         * atlas and screen-space.
         */
        private void loadLineSizes()
        {
            processNextLine();
            int lineHeightPixels = GetValueOfVariable("lineHeight") - paddingHeight;
            verticalPerPixelSize = TextMeshCreator.LINE_HEIGHT / (double)lineHeightPixels;
            horizontalPerPixelSize = verticalPerPixelSize / aspectRatio;
        }

        /**
         * Loads in data about each character and stores the data in the
         * {@link Character} class.
         * 
         * @param imageWidth
         *            - the width of the texture atlas in pixels.
         */
        private void loadCharacterData(int imageWidth)
        {
            processNextLine();
            processNextLine();
            while (processNextLine())
            {
                Character c = loadCharacter(imageWidth);
                if (c != null)
                {
                    metaData.Add(c.getId(), c);
                }
            }
        }

        /**
         * Loads all the data about one character in the texture atlas and converts
         * it all from 'pixels' to 'screen-space' before storing. The effects of
         * padding are also removed from the data.
         * 
         * @param imageSize
         *            - the size of the texture atlas in pixels.
         * @return The data about the character.
         */
        private Character loadCharacter(int imageSize)
        {
            int id = GetValueOfVariable("id");
            if (id == TextMeshCreator.SPACE_ASCII)
            {
                this.spaceWidth = (GetValueOfVariable("xadvance") - paddingWidth) * horizontalPerPixelSize;
                return null;
            }
            double xTex = ((double)GetValueOfVariable("x") + (padding[PAD_LEFT] - DESIRED_PADDING)) / imageSize;
            double yTex = ((double)GetValueOfVariable("y") + (padding[PAD_TOP] - DESIRED_PADDING)) / imageSize;
            int width = GetValueOfVariable("width") - (paddingWidth - (2 * DESIRED_PADDING));
            int height = GetValueOfVariable("height") - ((paddingHeight) - (2 * DESIRED_PADDING));
            double quadWidth = width * horizontalPerPixelSize;
            double quadHeight = height * verticalPerPixelSize;
            double xTexSize = (double)width / imageSize;
            double yTexSize = (double)height / imageSize;
            double xOff = (GetValueOfVariable("xoffset") + padding[PAD_LEFT] - DESIRED_PADDING) * horizontalPerPixelSize;
            double yOff = (GetValueOfVariable("yoffset") + (padding[PAD_TOP] - DESIRED_PADDING)) * verticalPerPixelSize;
            double xAdvance = (GetValueOfVariable("xadvance") - paddingWidth) * horizontalPerPixelSize;
            return new Character(id, xTex, yTex, xTexSize, yTexSize, xOff, yOff, quadWidth, quadHeight, xAdvance);
        }
    }
}
