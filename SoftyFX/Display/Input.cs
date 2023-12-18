using System.Runtime.InteropServices;

namespace SoftyFX.Display
{
    public static class Input
    {
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern short GetKeyStateByte(byte key);
        
        [DllImport("Softy32.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern short GetKeyStateChar(char key);

        public const byte MouseLeft = 0x01;
        public const byte MouseRight = 0x02;
        public const byte Cancel = 0x03;
        public const byte MouseMiddle = 0x04;
        public const byte MouseXButton1 = 0x05;
        public const byte MouseXButton2 = 0x06;
        public const byte Back = 0x08;
        public const byte Tab = 0x09;
        public const byte Clear = 0x0C;
        public const byte Return = 0x0D;
        public const byte Shift = 0x10;
        public const byte Control = 0x11;
        public const byte Menu = 0x12;
        public const byte Pause = 0x13;
        public const byte Capital = 0x14;
        public const byte Escape = 0x1B;
        public const byte Space = 0x20;
        public const byte PageUp = 0x21;
        public const byte PageDown = 0x22;
        public const byte End = 0x23;
        public const byte Home = 0x24;
        public const byte Left = 0x25;
        public const byte Up = 0x26;
        public const byte Right = 0x27;
        public const byte Down = 0x28;
        public const byte Select = 0x29;
        public const byte Print = 0x2A;
        public const byte Execute = 0x2B;
        public const byte PrtScr = 0x2C;
        public const byte Insert = 0x2D;
        public const byte Delete = 0x2E;
        public const byte LWin = 0x5B;
        public const byte RWin = 0x5C;
        public const byte Numpad0 = 0x60;
        public const byte Numpad1 = 0x61;
        public const byte Numpad2 = 0x62;
        public const byte Numpad3 = 0x63;
        public const byte Numpad4 = 0x64;
        public const byte Numpad5 = 0x65;
        public const byte Numpad6 = 0x66;
        public const byte Numpad7 = 0x67;
        public const byte Numpad8 = 0x68;
        public const byte Numpad9 = 0x69;
        public const byte Multiply = 0x6A;
        public const byte Add = 0x6B;
        public const byte Separator = 0x6C;
        public const byte Subtract = 0x6D;
        public const byte Decimal = 0x6E;
        public const byte Divide = 0x6F;
        public const byte F1 = 0x70;
        public const byte F2 = 0x71;
        public const byte F3 = 0x72;
        public const byte F4 = 0x73;
        public const byte F5 = 0x74;
        public const byte F6 = 0x75;
        public const byte F7 = 0x76;
        public const byte F8 = 0x77;
        public const byte F9 = 0x78;
        public const byte F10 = 0x79;
        public const byte F11 = 0x7A;
        public const byte F12 = 0x7B;
        public const byte NumLock = 0x90;
        public const byte ScrollLock = 0x91;
        public const byte LShift = 0xA0;
        public const byte RShift = 0xA1;
        public const byte LControl = 0xA2;
        public const byte RControl = 0xA3;
        public const byte LAlt = 0xA4;
        public const byte RAlt = 0xA5;

        public static bool IsKeyDown(byte key)
        {
            return GetKeyStateByte(key) < 0;
        }
        
        public static bool IsKeyDown(char key)
        {
            return GetKeyStateChar(key) < 0;
        }
    }
}