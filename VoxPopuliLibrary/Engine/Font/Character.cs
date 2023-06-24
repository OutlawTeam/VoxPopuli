using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.Font
{
    internal class Character
    {
        private int id;
        private double xTextureCoord;
        private double yTextureCoord;
        private double xMaxTextureCoord;
        private double yMaxTextureCoord;
        private double xOffset;
        private double yOffset;
        private double sizeX;
        private double sizeY;
        private double xAdvance;

        internal Character(int id, double xTextureCoord, double yTextureCoord, double xTexSize, double yTexSize,
            double xOffset, double yOffset, double sizeX, double sizeY, double xAdvance)
        {
            this.id = id;
            this.xTextureCoord = xTextureCoord;
            this.yTextureCoord = yTextureCoord;
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            xMaxTextureCoord = xTexSize + xTextureCoord;
            yMaxTextureCoord = yTexSize + yTextureCoord;
            this.xAdvance = xAdvance;
        }
        internal int getId()
        {
            return id;
        }

        internal double getxTextureCoord()
        {
            return xTextureCoord;
        }

        internal double getyTextureCoord()
        {
            return yTextureCoord;
        }

        internal double getXMaxTextureCoord()
        {
            return xMaxTextureCoord;
        }

        internal double getYMaxTextureCoord()
        {
            return yMaxTextureCoord;
        }

        internal double getxOffset()
        {
            return xOffset;
        }

        internal double getyOffset()
        {
            return yOffset;
        }

        internal double getSizeX()
        {
            return sizeX;
        }

        internal double getSizeY()
        {
            return sizeY;
        }

        internal double getxAdvance()
        {
            return xAdvance;
        }
    }
}
