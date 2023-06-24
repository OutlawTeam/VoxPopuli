using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.Font
{
    public class TextMeshCreator
    {

        public static double LINE_HEIGHT = 0.03f;
        public static int SPACE_ASCII = 32;

        private MetaFile metaData;

        public TextMeshCreator(string metaFile)
        {
            metaData = new MetaFile(metaFile);
        }

        public TextMeshData createTextMesh(GUIText text)
        {
            List<Line> lines = createStructure(text);
            TextMeshData data = createQuadVertices(text, lines);
            return data;
        }

        private List<Line> createStructure(GUIText text)
        {
            char[] chars = text.getTextString().ToCharArray();
            List<Line> lines = new List<Line>();
            Line currentLine = new Line(metaData.getSpaceWidth(), text.getFontSize(), text.getMaxLineSize());
            Word currentWord = new Word(text.getFontSize());
            foreach (char c in chars)
            {
                int ascii = (int)c;
                if (ascii == SPACE_ASCII)
                {
                    bool added = currentLine.attemptToAddWord(currentWord);
                    if (!added)
                    {
                        lines.Add(currentLine);
                        currentLine = new Line(metaData.getSpaceWidth(), text.getFontSize(), text.getMaxLineSize());
                        currentLine.attemptToAddWord(currentWord);
                    }
                    currentWord = new Word(text.getFontSize());
                    continue;
                }
                Character character = metaData.getCharacter(ascii);
                currentWord.addCharacter(character);
            }
            completeStructure(lines, currentLine, currentWord, text);
            return lines;
        }

        private void completeStructure(List<Line> lines, Line currentLine, Word currentWord, GUIText text)
        {
            bool added = currentLine.attemptToAddWord(currentWord);
            if (!added)
            {
                lines.Add(currentLine);
                currentLine = new Line(metaData.getSpaceWidth(), text.getFontSize(), text.getMaxLineSize());
                currentLine.attemptToAddWord(currentWord);
            }
            lines.Add(currentLine);
        }

        private TextMeshData createQuadVertices(GUIText text, List<Line> lines)
        {
            text.setNumberOfLines(lines.Count);
            double curserX = 0f;
            double curserY = 0f;
            List<float> vertices = new List<float>();
            List<float> textureCoords = new List<float>();
            foreach (Line line in lines)
            {
                if (text.isCentered())
                {
                    curserX = (line.getMaxLength() - line.getLineLength()) / 2;
                }
                foreach (Word word in line.getWords())
                {
                    foreach (Character letter in word.getCharacters())
                    {
                        addVerticesForCharacter(curserX, curserY, letter, text.getFontSize(), vertices);
                        addTexCoords(textureCoords, letter.getxTextureCoord(), letter.getyTextureCoord(),
                                letter.getXMaxTextureCoord(), letter.getYMaxTextureCoord());
                        curserX += letter.getxAdvance() * text.getFontSize();
                    }
                    curserX += metaData.getSpaceWidth() * text.getFontSize();
                }
                curserX = 0;
                curserY += LINE_HEIGHT * text.getFontSize();
            }
            return new TextMeshData(vertices.ToArray(), textureCoords.ToArray());
        }

        private void addVerticesForCharacter(double curserX, double curserY, Character character, double fontSize,
                List<float> vertices)
        {
            double x = curserX + (character.getxOffset() * fontSize);
            double y = curserY + (character.getyOffset() * fontSize);
            double maxX = x + (character.getSizeX() * fontSize);
            double maxY = y + (character.getSizeY() * fontSize);
            double properX = (2 * x) - 1;
            double properY = (-2 * y) + 1;
            double properMaxX = (2 * maxX) - 1;
            double properMaxY = (-2 * maxY) + 1;
            addVertices(vertices, properX, properY, properMaxX, properMaxY);
        }

        private static void addVertices(List<float> vertices, double x, double y, double maxX, double maxY)
        {
            vertices.Add((float)x);
            vertices.Add((float)y);
            vertices.Add((float)x);
            vertices.Add((float)maxY);
            vertices.Add((float)maxX);
            vertices.Add((float)maxY);
            vertices.Add((float)maxX);
            vertices.Add((float)maxY);
            vertices.Add((float)maxX);
            vertices.Add((float)y);
            vertices.Add((float)x);
            vertices.Add((float)y);
        }

        private static void addTexCoords(List<float> texCoords, double x, double y, double maxX, double maxY)
        {
            texCoords.Add((float)x);
            texCoords.Add((float)y);
            texCoords.Add((float)x);
            texCoords.Add((float)maxY);
            texCoords.Add((float)maxX);
            texCoords.Add((float)maxY);
            texCoords.Add((float)maxX);
            texCoords.Add((float)maxY);
            texCoords.Add((float)maxX);
            texCoords.Add((float)y);
            texCoords.Add((float)x);
            texCoords.Add((float)y);
        }
    }
}
