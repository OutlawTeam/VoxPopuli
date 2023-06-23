namespace VoxPopuliLibrary.Engine.API.GUI
{
    public enum PositionType
    {
        Pixel,
        Proportion,
        ProportionScale
    }
    public struct Position
    {
        PositionType type;
        public float x;
        public float y;
        public Position(PositionType type, float x, float y)
        {
            this.type = type;
            this.x = x;
            this.y = y;
        }
        public int GetRealX()
        {
            if(type == PositionType.Pixel)
            {
                return (int)x;
            }else if(type == PositionType.Proportion)
            {
                return (int)(x * API.WindowWidth());
            }
            else if(type == PositionType.ProportionScale)
            {
                return (int)(x * API.WindowWidth() *(API.WindowWidth()/1920));
            }
            else
            {
                return 0;
            }
        }
        public int GetRealY()
        {
            if (type == PositionType.Pixel)
            {
                return (int)y;
            }
            else if (type == PositionType.Proportion)
            {
                return (int)(y * API.WindowHeight());
            }
            else if (type == PositionType.ProportionScale)
            {
                return (int)(y * API.WindowWidth() * (API.WindowWidth() / 1920));
            }
            else
            {
                return 0;
            }
        }
    }
}
