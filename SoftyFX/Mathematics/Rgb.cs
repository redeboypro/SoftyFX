namespace SoftyFX.Mathematics
{
    public struct Rgb
    {
        public static readonly Rgb Black = new Rgb
        {
            R = 0,
            G = 0,
            B = 0
        };
        
        public static readonly Rgb White = new Rgb
        {
            R = 255,
            G = 255,
            B = 255
        };
        
        public static readonly Rgb Red = new Rgb
        {
            R = 255,
            G = 0,
            B = 0
        };
        
        public static readonly Rgb Green = new Rgb
        {
            R = 0,
            G = 255,
            B = 0
        };
        
        public static readonly Rgb Blue = new Rgb
        {
            R = 0,
            G = 0,
            B = 255
        };
        
        public static readonly Rgb Gray = new Rgb
        {
            R = 128,
            G = 128,
            B = 128
        };
        
        public static readonly Rgb Yellow = new Rgb
        {
            R = 255,
            G = 255,
            B = 0
        };
            
        public byte R;
        public byte G;
        public byte B;
    }
}