#region Copyright (c) 2013 Jens Thiel, http://thielj.github.io/MetroFramework
/*
 
MetroFramework - Windows Modern UI for .NET WinForms applications

Copyright (c) 2013 Jens Thiel, http://thielj.github.io/MetroFramework

Permission is hereby granted, free of charge, to any person obtaining a copy of 
this software and associated documentation files (the "Software"), to deal in the 
Software without restriction, including without limitation the rights to use, copy, 
modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
and to permit persons to whom the Software is furnished to do so, subject to the 
following conditions:

The above copyright notice and this permission notice shall be included in 
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 
Portions of this software are (c) 2011 Sven Walter, http://github.com/viperneo

 */
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MetroFramework.Controls
{
    [Designer("MetroFramework.Design.MetroProgressSpinnerDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    [ToolboxBitmap(typeof(ProgressBar))]
    public partial class MetroProgressSpinner : MetroControlBase
    {

        #region Fields

        private Timer timer;
        private int progress;
        private float angle = 270;

        [DefaultValue(true)]
        [Category(MetroDefaults.CatBehavior)]
        public bool Spinning
        {
            get { return timer.Enabled; }
            set { timer.Enabled = value; }
        }

        [DefaultValue(0)]
        [Category(MetroDefaults.CatAppearance)]
        public int Value
        {
            get { return progress; }
            set
            {
                if (value != -1 && (value < minimum || value > maximum))
                    throw new ArgumentOutOfRangeException("Progress value must be -1 or between Minimum and Maximum.");
                progress = value;
                Refresh();
            }
        }

        private int minimum = 0;
        [DefaultValue(0)]
        [Category(MetroDefaults.CatAppearance)]
        public int Minimum
        {
            get { return minimum; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Minimum value must be >= 0.");
                if (value >= maximum)
                    throw new ArgumentOutOfRangeException("Minimum value must be < Maximum.");
                minimum = value;
                if (progress != -1 && progress < minimum)
                    progress = minimum;
                Refresh();
            }
        }

        private int maximum = 100;
        [DefaultValue(0)]
        [Category(MetroDefaults.CatAppearance)]
        public int Maximum
        {
            get { return maximum; }
            set
            {
                if (value <= minimum)
                    throw new ArgumentOutOfRangeException("Maximum value must be > Minimum.");
                maximum = value;
                if (progress > maximum)
                    progress = maximum;
                Refresh();
            }
        }

        private bool ensureVisible = true;
        [DefaultValue(true)]
        [Category(MetroDefaults.CatAppearance)]
        public bool EnsureVisible
        {
            get { return ensureVisible; }
            set { ensureVisible = value; Refresh(); }
        }

        private float speed = 1f;
        [DefaultValue(1f)]
        [Category(MetroDefaults.CatBehavior)]
        public float Speed
        {
            get { return speed; }
            set
            {
                if (value <= 0 || value > 10)
                    throw new ArgumentOutOfRangeException("Speed value must be > 0 and <= 10.");

                speed = value;
            }
        }

        private bool backwards;
        [DefaultValue(false)]
        [Category(MetroDefaults.CatBehavior)]
        public bool Backwards
        {
            get { return backwards; }
            set { backwards = value; Refresh(); }
        }

        #endregion

        public MetroProgressSpinner()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UseTransparency();

            Width = 16;
            Height = 16;
            timer = new Timer { Interval = 20, Enabled = true };
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode) timer.Tick += (s, a) => { angle += speed * (backwards ? -6f : 6f); Invalidate(); };
        }

        #region Public Methods

        public void Reset()
        {
            progress = minimum;
            angle = 270;
            Refresh();
        }

        #endregion

        #region Paint Methods

        // override to always use style color
        public override Color EffectiveForeColor
        {
            get { return ShouldSerializeForeColor() || MetroControlState != "Normal" ? base.EffectiveForeColor : GetStyleColor(); }
        }

        protected override void OnPaintForeground(PaintEventArgs e)
        {
            using (Pen forePen = new Pen(EffectiveForeColor, (float)Width / 5))
            {
                int padding = (int)Math.Ceiling((float)Width / 10);

                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                if (progress != -1)
                {
                    float sweepAngle;
                    float progFrac = (float)(progress - minimum) / (float)(maximum - minimum);

                    if (ensureVisible)
                    {
                        sweepAngle = 30 + 300f * progFrac;
                    }
                    else
                    {
                        sweepAngle = 360f * progFrac;
                    }

                    if (backwards)
                    {
                        sweepAngle = -sweepAngle;
                    }

                    e.Graphics.DrawArc(forePen, padding, padding, Width - 2 * padding - 1, Height - 2 * padding - 1, angle, sweepAngle);
                }
                else
                {
                    const int maxOffset = 180;
                    for (int offset = 0; offset <= maxOffset; offset += 15)
                    {
                        int alpha = 290 - (offset * 290 / maxOffset);

                        if (alpha > 255)
                        {
                            alpha = 255;
                        }
                        if (alpha < 0)
                        {
                            alpha = 0;
                        }

                        Color col = Color.FromArgb(alpha, forePen.Color);
                        using (Pen gradPen = new Pen(col, forePen.Width))
                        {
                            float startAngle = angle + (offset - (ensureVisible ? 30 : 0)) * (backwards ? 1 : -1);
                            float sweepAngle = 15 * (backwards ? 1 : -1);
                            e.Graphics.DrawArc(gradPen, padding, padding, Width - 2 * padding - 1, Height - 2 * padding - 1, startAngle, sweepAngle);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
