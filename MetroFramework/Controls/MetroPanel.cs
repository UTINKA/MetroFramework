/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Drawing;
using System.ComponentModel;
using System.Security;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Interfaces;
using MetroFramework.Drawing;
using MetroFramework.Native;
using Point = System.Drawing.Point;

namespace MetroFramework.Controls
{
    [ToolboxBitmap(typeof(Panel))]
    public class MetroPanel : MetroPanelBase
    {

        #region Fields

        private MetroScrollBar verticalScrollbar = new MetroScrollBar(MetroScrollOrientation.Vertical);
        private MetroScrollBar horizontalScrollbar = new MetroScrollBar(MetroScrollOrientation.Horizontal);

        private bool showHorizontalScrollbar = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.CatAppearance)]
        public bool HorizontalScrollbar
        {
            get { return showHorizontalScrollbar; }
            set { showHorizontalScrollbar = value; }
        }

        [Category(MetroDefaults.CatAppearance)]
        public int HorizontalScrollbarSize
        {
            get { return horizontalScrollbar.ScrollbarSize; }
            set { horizontalScrollbar.ScrollbarSize = value; }
        }

        [Category(MetroDefaults.CatAppearance)]
        public bool HorizontalScrollbarBarColor
        {
            get { return horizontalScrollbar.UseBarColor; }
            set { horizontalScrollbar.UseBarColor = value; }
        }

        [Category(MetroDefaults.CatAppearance)]
        public bool HorizontalScrollbarHighlightOnWheel
        {
            get { return horizontalScrollbar.HighlightOnWheel; }
            set { horizontalScrollbar.HighlightOnWheel = value; }
        }

        private bool showVerticalScrollbar = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.CatAppearance)]
        public bool VerticalScrollbar
        {
            get { return showVerticalScrollbar; }
            set { showVerticalScrollbar = value; }
        }

        [Category(MetroDefaults.CatAppearance)]
        public int VerticalScrollbarSize
        {
            get { return verticalScrollbar.ScrollbarSize; }
            set { verticalScrollbar.ScrollbarSize = value; }
        }

        [Category(MetroDefaults.CatAppearance)]
        public bool VerticalScrollbarBarColor
        {
            get { return verticalScrollbar.UseBarColor; }
            set { verticalScrollbar.UseBarColor = value; }
        }

        [Category(MetroDefaults.CatAppearance)]
        public bool VerticalScrollbarHighlightOnWheel
        {
            get { return verticalScrollbar.HighlightOnWheel; }
            set { verticalScrollbar.HighlightOnWheel = value; }
        }

        [Category(MetroDefaults.CatAppearance)]
        public new bool AutoScroll
        {
            get
            {
                return base.AutoScroll;
            }
            set
            {
                if (value)
                {
                    showHorizontalScrollbar = true;
                    showVerticalScrollbar = true;
                }

                base.AutoScroll = value;
            }
        }

        private bool useCustomBackground = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.CatAppearance)]
        public bool CustomBackground
        {
            get { return useCustomBackground; }
            set { useCustomBackground = value; }
        }

        /// <summary>
        ///     A border drawn inside the client area.
        /// </summary>
        /// <remarks>
        ///     This currently only supports <see cref="MetroBorderStyle.None"/> (the default) and 
        ///     <see cref="MetroBorderStyle.FixedSingle"/> (a thin grey line).
        /// </remarks>
        private MetroBorderStyle _borderStyle = MetroBorderStyle.None;
        [DefaultValue(MetroBorderStyle.None)]
        [Browsable(true)]
        [Category(MetroDefaults.CatAppearance)]
        public new MetroBorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set { _borderStyle = value; }
        }

        #endregion

        #region Constructor

        public MetroPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);

            base.BorderStyle = System.Windows.Forms.BorderStyle.None;

            Controls.Add(verticalScrollbar);
            Controls.Add(horizontalScrollbar);

            verticalScrollbar.UseBarColor = true;
            horizontalScrollbar.UseBarColor = true;

            verticalScrollbar.Scroll += VerticalScrollbarScroll;
            horizontalScrollbar.Scroll += HorizontalScrollbarScroll;
        }

        #endregion

        #region Scroll Events

        private void HorizontalScrollbarScroll(object sender, ScrollEventArgs e)
        {
            AutoScrollPosition = new Point(e.NewValue, verticalScrollbar.Value);
            UpdateScrollBarPositions();
        }

        private void VerticalScrollbarScroll(object sender, ScrollEventArgs e)
        {
            AutoScrollPosition = new Point(horizontalScrollbar.Value, e.NewValue);
            UpdateScrollBarPositions();
        }

        #endregion

        #region Overridden Methods

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Color backColor = MetroPaint.BackColor.Form(Theme);

            if (useCustomBackground)
                backColor = BackColor;

            e.Graphics.Clear(backColor);

            if (BorderStyle != MetroBorderStyle.None)
            {
                Color c = MetroPaint.BorderColor.Form(Theme);
                using (Pen pen = new Pen(c))
                {
                    e.Graphics.DrawLines(pen, new[]
                        {
                            new Point(0, 0),
                            new Point(0, Height - 1),
                            new Point(Width - 1, Height - 1),
                            new Point(Width - 1, 0),
                            new Point(0, 0)
                        });
                }
            }

            if (DesignMode)
            {
                horizontalScrollbar.Visible = false;
                verticalScrollbar.Visible = false;
                return;
            }

            UpdateScrollBarPositions();

            if (HorizontalScrollbar)
            {
                horizontalScrollbar.Visible = HorizontalScroll.Visible;
            }
            if (HorizontalScroll.Visible)
            {
                horizontalScrollbar.Minimum = HorizontalScroll.Minimum;
                horizontalScrollbar.Maximum = HorizontalScroll.Maximum;
                horizontalScrollbar.SmallChange = HorizontalScroll.SmallChange;
                horizontalScrollbar.LargeChange = HorizontalScroll.LargeChange;
            }

            if (VerticalScrollbar)
            {
                verticalScrollbar.Visible = VerticalScroll.Visible;
            }
            if (VerticalScroll.Visible)
            {
                verticalScrollbar.Minimum = VerticalScroll.Minimum;
                verticalScrollbar.Maximum = VerticalScroll.Maximum;
                verticalScrollbar.SmallChange = VerticalScroll.SmallChange;
                verticalScrollbar.LargeChange = VerticalScroll.LargeChange;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            verticalScrollbar.Value = VerticalScroll.Value;
            horizontalScrollbar.Value = HorizontalScroll.Value;
        }

        [SecuritySafeCritical]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (!DesignMode)
            {
                WinApi.ShowScrollBar(Handle, (int)WinApi.ScrollBar.SB_BOTH, 0);
            }
        }

        #endregion

        #region Management Methods

        private void UpdateScrollBarPositions()
        {
            if (DesignMode)
            {
                return;
            }

            if (!AutoScroll)
            {
                verticalScrollbar.Visible = false;
                horizontalScrollbar.Visible = false;
                return;
            }

            int margin = BorderStyle != MetroBorderStyle.None ? 1 : 0;

            if (VerticalScrollbar)
            {
                if (VerticalScroll.Visible)
                {
                    verticalScrollbar.Location = new Point(ClientRectangle.Width - verticalScrollbar.Width - margin, ClientRectangle.Y + margin) ;
                    verticalScrollbar.Height = ClientRectangle.Height - 2*margin;
                }
            }
            else
            {
                verticalScrollbar.Visible = false;
            }

            if (HorizontalScrollbar)
            {
                if (HorizontalScroll.Visible)
                {
                    horizontalScrollbar.Location = new Point(ClientRectangle.X + margin, ClientRectangle.Height - horizontalScrollbar.Height - margin);
                    horizontalScrollbar.Width = ClientRectangle.Width - 2*margin;
                }
            }
            else
            {
                horizontalScrollbar.Visible = false;
            }
        }

        #endregion
    }
}
