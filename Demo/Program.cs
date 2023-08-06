using System;
using SoftyFX;

namespace Demo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SoftyContext.InitDevice();
            while (!SoftyContext.ReadyToQuit)
            {
                if (SoftyContext.GetCursorLocation(out var x, out var y))
                {
                    SoftyContext.Clear();
                    SoftyContext.DrawBresenhamLine(10, 10, x, y, 255, 0, 0);
                    SoftyContext.WaitForFrame(60);
                }
            }
            SoftyContext.ReleaseDevice();
        }
    }
}