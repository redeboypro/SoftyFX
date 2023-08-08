using System.Runtime.InteropServices;

namespace SoftyFX
{
    public static class SoftyContext
    {
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitDevice();
        
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReleaseDevice();
        
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Clear();
        
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void WaitForFrame(int fps);

        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FillPolygon(int[] vertices, int vertexCount, byte r, byte g, byte b);
        
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DrawPoint(int x, int y, byte r, byte g, byte b);
        
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DrawLine(int x1, int y1, int x2, int y2, byte r, byte g, byte b);
        
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DrawBresenhamLine(int x1, int y1, int x2, int y2, byte r, byte g, byte b);

        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetCursorLocation(out int x, out int y);
        
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetWindowSize(out int width, out int height);
        
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UnlockDoubleBuffer();
        
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LockDoubleBuffer();

        public static bool ReadyToQuit { get; private set; }

        public static void Quit()
        {
            ReadyToQuit = true;
        }
    }
}