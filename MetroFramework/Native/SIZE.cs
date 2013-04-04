using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace MetroFramework.Native
{
    //  JT: I think these are obsolete - it should be possible to use the equivalent struct from System.Drawing

    [DebuggerDisplay("({Width},{Height})")]
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int Width;
        public int Height;

        public static SIZE Empty;

        public SIZE(int width, int height)
        {
            Width = width; 
            Height = height;
        }

        public SIZE(Size other)
        {
            Width = other.Width;
            Height = other.Height;
        }

        public Size ToSize()
        {
            return new Size(Width, Height);
        }

        public static implicit operator SIZE(Size other)
        {
            return new SIZE(other);
        }

        public static implicit operator Size(SIZE other)
        {
            return other.ToSize();
        }
    }
}