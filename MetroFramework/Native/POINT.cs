using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;


namespace MetroFramework.Native
{
    //  JT: I think these are obsolete - it should be possible to use the equivalent struct from System.Drawing

    [DebuggerDisplay("({X},{Y})")]
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public static POINT Empty;

        public POINT(int x, int y)
        {
            X = x; 
            Y = y;
        }

        public POINT(Point other)
        {
            X = other.X;
            Y = other.Y;
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public static implicit operator POINT(Point other)
        {
            return new POINT(other);
        }

        public static implicit operator Point(POINT other)
        {
            return other.ToPoint();
        }
    }
}