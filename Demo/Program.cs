using System;
using System.Diagnostics;
using System.Globalization;
using SoftyFX;
using SoftyFX.Common;
using SoftyFX.Graphics;
using SoftyFX.Mathematics;
using SoftyFX.Time;

namespace Demo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var display = new DemoCanvas();
            display.Run();
        }
    }
}