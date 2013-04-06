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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Interfaces;
using MetroFramework.Drawing;

namespace MetroFramework.Controls
{
    //[Designer("MetroFramework.Design.MetroContainerControlDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    public class MetroUserControl : MetroUserControlBase
    {
        #region Fields

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

        public MetroUserControl()
        {
            base.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }

        #region Overridden Methods

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
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        #endregion
    }
}
