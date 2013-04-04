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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Native;

namespace MetroFramework.Forms
{

    public enum TextAlign
    {
        Left,
        Center,
        Right
    }

    public enum ShadowType
    {
        None,
        Flat,
        DropShadow,
        SystemShadow,
        AeroShadow
    }

    public enum MetroFormBorderStyle
    {
        None,
        FixedSingle
    }
   
    public class MetroForm : Form, IMetroForm, IDisposable
    {
        #region Interface

        private MetroColorStyle metroStyle = MetroColorStyle.Blue;
        [Category("Metro Appearance")]
        public MetroColorStyle Style
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Style;

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Light;

        [Category("Metro Appearance")]
        public MetroThemeStyle Theme
        {
            get
            {
                if (StyleManager != null)
                    return StyleManager.Theme;

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        #endregion

        #region Properties

        //private bool isInitialized = false;

        private TextAlign textAlign = TextAlign.Left;
        [Browsable(true)]
        [Category("Metro Appearance")]
        public TextAlign TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; }
        }

        [Browsable(false)]
        public override Color BackColor
        {
            get { return MetroPaint.BackColor.Form(Theme); }
        }

        /// <summary>
        ///     A border drawn inside the client area.
        /// </summary>
        /// <remarks>
        ///     This currently only supports <see cref="MetroFormBorderStyle.None"/> (the default) and 
        ///     <see cref="MetroFormBorderStyle.FixedSingle"/> (a thin grey line).
        /// </remarks>
        private MetroFormBorderStyle formBorderStyle = MetroFormBorderStyle.None;
        [DefaultValue(MetroFormBorderStyle.None)]
        [Browsable(true)]
        [Category("Metro Appearance")]
        public MetroFormBorderStyle BorderStyle
        {
            get { return formBorderStyle;}
            set { formBorderStyle = value;}
        }

        private bool isMovable = true;
        [Category("Metro Appearance")]
        public bool Movable
        {
            get { return isMovable; }
            set { isMovable = value; }
        }

        public new Padding Padding
        {
            get { return base.Padding; }
            set
            {
                value.Top = Math.Max(value.Top, DisplayHeader ? 60 : 30);
                base.Padding = value;
            }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(20, DisplayHeader ? 60 : 20, 20, 20); }
        }

        private bool displayHeader = true;
        [Category("Metro Appearance")]
        [DefaultValue(true)]
        public bool DisplayHeader
        {
            get { return displayHeader; }
            set 
            {
                if (value != displayHeader)
                {
                    Padding p = base.Padding;
                    p.Top += value ? 30 : -30;
                    base.Padding = p;
                }
                displayHeader = value;
                //Invalidate();
            }
        }

        private bool isResizable = true;
        [Category("Metro Appearance")]
        public bool Resizable
        {
            get { return isResizable; }
            set { isResizable = value; }
        }

        private ShadowType shadowType = ShadowType.AeroShadow;
        [Category("Metro Appearance")]
        [DefaultValue(ShadowType.AeroShadow)]
        public ShadowType ShadowType
        {
            get { return shadowType; }
            set { shadowType = value; }
        }

        /// <summary>
        ///     The colored border at the top of the form
        /// </summary>
        private const int borderWidth = 5;

        #endregion

        #region Constructor

        public MetroForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            FormBorderStyle = FormBorderStyle.None;
            Name = "MetroForm";
            StartPosition = FormStartPosition.CenterScreen;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveShadow();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Paint Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            Color backColor = MetroPaint.BackColor.Form(Theme);
            Color foreColor = MetroPaint.ForeColor.Title(Theme);

            e.Graphics.Clear(backColor);

            using (SolidBrush b = MetroPaint.GetStyleBrush(Style))
            {
                Rectangle topRect = new Rectangle(0, 0, Width, borderWidth);
                e.Graphics.FillRectangle(b, topRect);
            }

            if (BorderStyle != MetroFormBorderStyle.None)
            {
                Color c = MetroPaint.BorderColor.Form(Theme); // TODO: Use style color for active window?
                using (Pen pen = new Pen(c))
                {
                    e.Graphics.DrawLines(pen, new[]
                        {
                            new Point(0, borderWidth),
                            new Point(0, Height - 1),
                            new Point(Width - 1, Height - 1),
                            new Point(Width - 1, borderWidth)
                        });
                }
            }

            if (displayHeader)
            {
                // Assuming padding 20px on left/right; 20px from top; max 40px height
                Rectangle bounds = new Rectangle(20, 20, ClientRectangle.Width - 2*20, 40);
                TextFormatFlags flags = TextFormatFlags.EndEllipsis | GetTextFormatFlags();
                TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Title, bounds, foreColor, flags);
            }

            if (Resizable && (SizeGripStyle == SizeGripStyle.Auto || SizeGripStyle == SizeGripStyle.Show))
            {
                using (SolidBrush b = new SolidBrush(MetroPaint.ForeColor.Button.Disabled(Theme)))
                {
                    Size resizeHandleSize = new Size(2, 2);
                    e.Graphics.FillRectangles(b, new Rectangle[] {
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-10), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-10), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-14,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-14), resizeHandleSize)
                    });
                }
            }
        }

        private TextFormatFlags GetTextFormatFlags()
        {
            switch (TextAlign)
            {
                case TextAlign.Left: return TextFormatFlags.Left;
                case TextAlign.Center: return TextFormatFlags.HorizontalCenter;
                case TextAlign.Right: return TextFormatFlags.Right;
            }
            throw new InvalidOperationException();
        }

        #endregion

        #region Management Methods

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!(this is MetroTaskWindow))
                MetroTaskWindow.ForceClose();

            base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            RemoveShadow();

            base.OnClosed(e);
        }

        //protected override void OnDeactivate(EventArgs e)
        //{
        //    base.OnDeactivate(e);
        //    //if (isInitialized)
        //    //{
        //        Refresh();
        //    //}
        //}

        [SecuritySafeCritical]
        public bool FocusMe()
        {
            return WinApi.SetForegroundWindow(Handle);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode) return;

            switch (StartPosition)
            {
                case FormStartPosition.CenterParent:
                    CenterToParent();
                    break;
                case FormStartPosition.CenterScreen:
                    CenterToScreen();
                    break;
            }

            RemoveCloseButton();
        
            if (ControlBox)
            {
                AddWindowButton(WindowButtons.Close);

                if (MaximizeBox)
                    AddWindowButton(WindowButtons.Maximize);

                if (MinimizeBox)
                    AddWindowButton(WindowButtons.Minimize);

                UpdateWindowButtonPosition();
            }

            CreateShadow();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (DesignMode) return;

            // TODO: necessary??
            //Refresh();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);

            Invalidate();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            UpdateWindowButtonPosition();
        }

        protected override void WndProc(ref Message m)
        {
            if (DesignMode)
            {
                base.WndProc(ref m);
                return;
            }

            switch (m.Msg)
            {
                case WinApi.Messages.WM_SYSCOMMAND:
                    int sc = m.WParam.ToInt32() & 0xFFF0;
                    switch (sc)
                    {
                        case WinApi.Messages.SC_MOVE: 
                            if (!Movable) return; 
                            break;
                        case WinApi.Messages.SC_MAXIMIZE: 
                            break;
                        case WinApi.Messages.SC_RESTORE:
                            break;
                    }
                    break;

                case WinApi.Messages.WM_NCLBUTTONDBLCLK:
                case WinApi.Messages.WM_LBUTTONDBLCLK: // I think this one can be removed...
                    if  (!MaximizeBox) return;
                    break;

                case WinApi.Messages.WM_NCHITTEST:
                    WinApi.HitTest ht = HitTestNCA(m.HWnd, m.WParam, m.LParam);
                    if (ht != WinApi.HitTest.HTCLIENT)
                    {
                        m.Result = (IntPtr)ht;
                        return;
                    }
                    break;

                case WinApi.Messages.WM_DWMCOMPOSITIONCHANGED:
                    // not sure what we need to do here - update our aero shadow??
                    break;
            }

            base.WndProc(ref m);

            // some messages are better post-processed ...

            switch (m.Msg)
            {
                case WinApi.Messages.WM_GETMINMAXINFO:
                    OnGetMinMaxInfo(m.HWnd, m.LParam);
                      //m.Result = IntPtr.Zero;
                    break;
            }
        }

        [SecuritySafeCritical]
        private unsafe void OnGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
                WinApi.MINMAXINFO* pmmi = (WinApi.MINMAXINFO*)lParam;

                //  NOTE: ptMaxPosition is always relative to the origin of the window's current screen
                // e.g. usually (0, 0) unless the taskbar is on the left or top.

                Screen s = Screen.FromHandle(hwnd);
                pmmi->ptMaxSize.X = s.WorkingArea.Width;
                pmmi->ptMaxSize.Y = s.WorkingArea.Height;
                pmmi->ptMaxPosition.X = Math.Abs(s.WorkingArea.Left - s.Bounds.Left);
                pmmi->ptMaxPosition.Y = Math.Abs(s.WorkingArea.Top - s.Bounds.Top);

                // The form seems to fill these in just fine...
                //if (MinimumSize.Width > 0) pmmi->ptMinTrackSize.x = MinimumSize.Width;
                //if (MinimumSize.Height > 0) pmmi->ptMinTrackSize.y = MinimumSize.Height;
                //if (MaximumSize.Width > 0) pmmi->ptMaxTrackSize.x = MaximumSize.Width;
                //if (MaximumSize.Height > 0) pmmi->ptMaxTrackSize.y = MaximumSize.Height;
        }

        private WinApi.HitTest HitTestNCA(IntPtr hwnd, IntPtr wparam, IntPtr lparam)
        {
            //Point vPoint = PointToClient(new Point((int)lparam & 0xFFFF, (int)lparam >> 16 & 0xFFFF));
            //Point vPoint = PointToClient(new Point((Int16)lparam, (Int16)((int)lparam >> 16)));
            Point vPoint = new Point((Int16)lparam, (Int16)((int)lparam >> 16));
            int vPadding = Math.Max(Padding.Right, Padding.Bottom);

            if (Resizable)
            {
                if (RectangleToScreen(new Rectangle(ClientRectangle.Width - vPadding, ClientRectangle.Height - vPadding, vPadding, vPadding)).Contains(vPoint))
                    return WinApi.HitTest.HTBOTTOMRIGHT;
            }

          if (RectangleToScreen(new Rectangle(borderWidth, borderWidth, ClientRectangle.Width - 2 * borderWidth, 50)).Contains(vPoint))
                return WinApi.HitTest.HTCAPTION;

            return WinApi.HitTest.HTCLIENT;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left && Movable)
            {
                if (WindowState == FormWindowState.Maximized) return;
                if (Width - borderWidth > e.Location.X && e.Location.X > borderWidth && e.Location.Y > borderWidth)
                {
                    MoveControl();                    
                }
            }
            
        }

        [SecuritySafeCritical]
        private void MoveControl()
        {
            WinApi.ReleaseCapture();
            WinApi.SendMessage(Handle, WinApi.Messages.WM_NCLBUTTONDOWN, (int)WinApi.HitTest.HTCAPTION, 0);
        }

        #endregion

        #region Window Buttons

        private enum WindowButtons
        {
            Minimize,
            Maximize,
            Close
        }

        private Dictionary<WindowButtons, MetroFormButton> windowButtonList;

        private void AddWindowButton(WindowButtons button)
        {
            if (windowButtonList == null)
                windowButtonList = new Dictionary<WindowButtons, MetroFormButton>();

            if (windowButtonList.ContainsKey(button))
                return;

            MetroFormButton newButton = new MetroFormButton();

            if (button == WindowButtons.Close)
            {
                newButton.Text = "r";
            }
            else if (button == WindowButtons.Minimize)
            {
                newButton.Text = "0";
            }
            else if (button == WindowButtons.Maximize)
            {
                if (WindowState == FormWindowState.Normal)
                    newButton.Text = "1";
                else
                    newButton.Text = "2";
            }

            newButton.Tag = button;
            newButton.Size = new Size(25, 20);
            newButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            newButton.Click += WindowButton_Click;
            Controls.Add(newButton);

            windowButtonList.Add(button, newButton);
        }

        private void WindowButton_Click(object sender, EventArgs e)
        {
            var btn = sender as MetroFormButton;
            if (btn != null)
            {
                var btnFlag = (WindowButtons)btn.Tag;
                if (btnFlag == WindowButtons.Close)
                {
                    Close();
                }
                else if (btnFlag == WindowButtons.Minimize)
                {
                    WindowState = FormWindowState.Minimized;
                }
                else if (btnFlag == WindowButtons.Maximize)
                {
                    if (WindowState == FormWindowState.Normal)
                    {
                        WindowState = FormWindowState.Maximized;
                        btn.Text = "2";
                    }
                    else
                    {
                        WindowState = FormWindowState.Normal;
                        btn.Text = "1";
                    }
                }
            }
        }

        private void UpdateWindowButtonPosition()
        {
            if (!ControlBox) return;

            Dictionary<int, WindowButtons> priorityOrder = new Dictionary<int, WindowButtons>(3) { {0, WindowButtons.Close}, {1, WindowButtons.Maximize}, {2, WindowButtons.Minimize} };

            Point firstButtonLocation = new Point(ClientRectangle.Width - borderWidth - 25, borderWidth);
            int lastDrawedButtonPosition = firstButtonLocation.X - 25;

            MetroFormButton firstButton = null;

            if (windowButtonList.Count == 1)
            {
                foreach (KeyValuePair<WindowButtons, MetroFormButton> button in windowButtonList)
                {
                    button.Value.Location = firstButtonLocation;
                }
            }
            else
            {
                foreach (KeyValuePair<int, WindowButtons> button in priorityOrder)
                {
                    bool buttonExists = windowButtonList.ContainsKey(button.Value);

                    if (firstButton == null && buttonExists)
                    {
                        firstButton = windowButtonList[button.Value];
                        firstButton.Location = firstButtonLocation;
                        continue;
                    }

                    if (firstButton == null || !buttonExists) continue;

                    windowButtonList[button.Value].Location = new Point(lastDrawedButtonPosition, borderWidth);
                    lastDrawedButtonPosition = lastDrawedButtonPosition - 25;
                }
            }

            Refresh();
        }

        private class MetroFormButton : Button, IMetroControl
        {
            #region Interface

            private MetroColorStyle metroStyle = MetroColorStyle.Blue;
            [Category("Metro Appearance")]
            public MetroColorStyle Style
            {
                get
                {
                    if (StyleManager != null)
                        return StyleManager.Style;

                    return metroStyle;
                }
                set { metroStyle = value; }
            }

            private MetroThemeStyle metroTheme = MetroThemeStyle.Light;
            [Category("Metro Appearance")]
            public MetroThemeStyle Theme
            {
                get
                {
                    if (StyleManager != null)
                        return StyleManager.Theme;

                    return metroTheme;
                }
                set { metroTheme = value; }
            }

            private MetroStyleManager metroStyleManager = null;
            [Browsable(false)]
            public MetroStyleManager StyleManager
            {
                get { return metroStyleManager; }
                set { metroStyleManager = value; }
            }

            #endregion

            #region Fields

            private bool isHovered = false;
            private bool isPressed = false;

            #endregion

            #region Constructor

            public MetroFormButton()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.UserPaint, true);
            }

            #endregion

            #region Paint Methods

            protected override void OnPaint(PaintEventArgs e)
            {
                Color backColor, foreColor;

                if (Parent != null)
                {
                    if (Parent is IMetroForm)
                    {
                        backColor = MetroPaint.BackColor.Form(Theme);
                    }
                    else if (Parent is IMetroControl)
                    {
                        backColor = MetroPaint.GetStyleColor(Style);
                    }
                    else
                    {
                        backColor = Parent.BackColor;
                    }
                }
                else
                {
                    backColor = MetroPaint.BackColor.Form(Theme);
                }

                if (isHovered && !isPressed && Enabled)
                {
                    foreColor = MetroPaint.ForeColor.Button.Normal(Theme);
                    backColor = MetroPaint.BackColor.Button.Normal(Theme);
                }
                else if (isHovered && isPressed && Enabled)
                {
                    foreColor = MetroPaint.ForeColor.Button.Press(Theme);
                    backColor = MetroPaint.GetStyleColor(Style);
                }
                else if (!Enabled)
                {
                    foreColor = MetroPaint.ForeColor.Button.Disabled(Theme);
                    backColor = MetroPaint.BackColor.Button.Disabled(Theme);
                }
                else
                {
                    foreColor = MetroPaint.ForeColor.Button.Normal(Theme);
                }

                e.Graphics.Clear(backColor);
                Font buttonFont = new Font("Webdings", 9.25f);
                TextRenderer.DrawText(e.Graphics, Text, buttonFont, ClientRectangle, foreColor, backColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            #endregion

            #region Mouse Methods

            protected override void OnMouseEnter(EventArgs e)
            {
                isHovered = true;
                Invalidate();

                base.OnMouseEnter(e);
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left)
                {
                    isPressed = true;
                    Invalidate();
                }

                base.OnMouseDown(e);
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                isPressed = false;
                Invalidate();

                base.OnMouseUp(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                isHovered = false;
                Invalidate();

                base.OnMouseLeave(e);
            }

            #endregion
        }

        #endregion

        #region Shadows

        private const int CS_DROPSHADOW = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (ShadowType == ShadowType.SystemShadow)
                    cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private Form shadowForm;

        private void CreateShadow()
        {
            Debug.Assert(shadowForm==null);

            switch (ShadowType)
            {
                case ShadowType.Flat:
                    shadowForm = new MetroFlatDropShadow(this);
                    return;

                case ShadowType.DropShadow:
                    shadowForm = new MetroRealisticDropShadow(this);
                    return;

                case ShadowType.AeroShadow:
                    try
                    {
                        shadowForm = new MetroAeroDropShadow(this);
                        return;
                    }
                    catch (Exception)
                    {
                        // fallback to DropShadow
                        ShadowType = ShadowType.DropShadow;
                        CreateShadow();
                    }
                    return;
            }
        }

        private void RemoveShadow()
        {
            if (shadowForm == null || shadowForm.IsDisposed) return;

            shadowForm.Visible = false;
            Owner = shadowForm.Owner;
            shadowForm.Owner = null;
            shadowForm.Dispose();
            shadowForm = null;
        }

        #region MetroShadowBase

        protected abstract class MetroShadowBase : Form
        {
            protected Form TargetForm { get; private set; }

            private readonly int _shadowSize;
            private readonly int _wsExStyle;

            private const int TICKS_PER_MS = 10000;
            private const long RESIZE_REDRAW_INTERVAL = 10*TICKS_PER_MS; 

            protected MetroShadowBase(Form targetForm, int shadowSize, int wsExStyle)
            {
                Debug.WriteLine(MethodInfo.GetCurrentMethod());

                TargetForm = targetForm;
                _shadowSize = shadowSize;
                _wsExStyle = wsExStyle;

                TargetForm.Activated += OnTargetFormActivated;
                TargetForm.ResizeBegin += OnTargetFormResizeBegin;
                TargetForm.ResizeEnd += OnTargetFormResizeEnd;
                TargetForm.VisibleChanged += OnTargetFormVisibleChanged;
                TargetForm.SizeChanged += OnTargetFormSizeChanged;

                TargetForm.Move += OnTargetFormMove;
                TargetForm.Resize += OnTargetFormResize;

                if (TargetForm.Owner != null)
                    Owner = TargetForm.Owner;

                TargetForm.Owner = this;

                MaximizeBox = false;
                MinimizeBox = false;
                ShowInTaskbar = false;
                ShowIcon = false;
                FormBorderStyle = FormBorderStyle.None;

                Bounds = GetShadowBounds();
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= _wsExStyle;
                    return cp;
                }
            }

            private Rectangle GetShadowBounds()
            {
                Rectangle r = TargetForm.Bounds;
                r.Inflate(_shadowSize, _shadowSize);
                return r;
            }

            protected abstract void PaintIt();

            protected abstract void ClearIt();

            #region event handlers

            private bool _isBringingToFront;

            protected override void OnDeactivate(EventArgs e)
            {
                base.OnDeactivate(e);
                _isBringingToFront = true;
            }

            private void OnTargetFormActivated(object sender, EventArgs e)
            {
                if (Visible) Update();
                if (_isBringingToFront)
                {
                    _isBringingToFront = false;
                    return;
                }
                BringToFront();
            }

            private void OnTargetFormVisibleChanged(object sender, EventArgs e)
            {
                Visible = TargetForm.Visible && TargetForm.WindowState == FormWindowState.Normal;
                Update();
            }

            private long _lastResizedOn;

            private bool IsResizing { get { return _lastResizedOn > 0; } }

            private void OnTargetFormResizeBegin(object sender, EventArgs e)
            {
                _lastResizedOn = DateTime.Now.Ticks;
            }

            private void OnTargetFormMove(object sender, EventArgs e)
            {
                if (!TargetForm.Visible || TargetForm.WindowState != FormWindowState.Normal) 
                    Visible = false; // maximized 
                else
                    Bounds = GetShadowBounds(); // just track the window - no need to invalidate
            }

            private void OnTargetFormResize(object sender, EventArgs e)
            {
                ClearIt();
            }

            private void OnTargetFormSizeChanged(object sender, EventArgs e)
            {
                Bounds = GetShadowBounds();
                if (IsResizing)
                {
                    if (DateTime.Now.Ticks - _lastResizedOn <= RESIZE_REDRAW_INTERVAL)
                        return;
                    _lastResizedOn = DateTime.Now.Ticks;
                }
                PaintIfVisible();
            }

            private void OnTargetFormResizeEnd(object sender, EventArgs e)
            {
                _lastResizedOn = 0;
                PaintIfVisible();
            }

            private void PaintIfVisible()
            {
                if (TargetForm.Visible && TargetForm.WindowState == FormWindowState.Normal)
                    PaintIt();
            }

            #endregion

            #region Constants

            /// <summary>
            ///     WS_EX_TRANSPARENT does two things:
            ///     (i) It makes the window 'transparent' to mouse clicks, i.e. all clicks are forwarded to windows below.
            ///     (ii) it alters the painting algorithm as follows: 
            ///     If a WS_EX_TRANSPARENT window needs to be painted, and it has any non-WS_EX_TRANSPARENT 
            ///     windows siblings (which belong to the same process) which also need to be painted, 
            ///     then the window manager will paint the non-WS_EX_TRANSPARENT windows first.
            /// </summary>
            /// <seealso href="http://blogs.msdn.com/b/oldnewthing/archive/2012/12/17/10378525.aspx"/>
            protected const int WS_EX_TRANSPARENT = 0x20;

            /// <summary>
            ///     Using a layered window can significantly improve performance and visual effects for a window that has a complex shape, 
            ///     animates its shape, or wishes to use alpha blending effects. The system automatically composes and repaints layered 
            ///     windows and the windows of underlying applications. As a result, layered windows are rendered smoothly, without the 
            ///     flickering typical of complex window regions. In addition, layered windows can be partially translucent, that is, alpha-blended.
            /// </summary>
            protected const int WS_EX_LAYERED = 0x80000;

            /// <summary>
            ///     A top-level window created with this style does not become the foreground window when the user clicks it. 
            ///     The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
            /// </summary>
            protected const int WS_EX_NOACTIVATE = 0x8000000;

            #endregion

        }

        #endregion

        #region MetroAeroDropShadow

        protected class MetroAeroDropShadow : MetroShadowBase
        {
            public MetroAeroDropShadow(Form targetForm)
                : base(targetForm, 0, WS_EX_TRANSPARENT | WS_EX_NOACTIVATE )
            {
                // Results differ between 32-bit and 64-bit... In 32-it, we seem to be able to use all border styles
                // (except None, of course). In 64 bit, the only fully working style is SizableToolWindow.
                // In particular (64-bit):
                //   FixedDialog gives a small transparent frame and round corners. 
                //   Same with Fixed 3D or FixedSingle, but there are strange animation artifacts!?
                //   FixedToolWindow results in a small frame straight corners, no anim artifacts
                //   Sizable comes with no frames, but artifacts (64, OK in 32)
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
            }

            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                // ignore bogus size-only updates, we get better info from TargetForm
                if (specified == BoundsSpecified.Size) return;
                base.SetBoundsCore(x, y, width, height, specified);
            }

            protected override void PaintIt() { Visible = true; }

            protected override void ClearIt() { /* nothing */ }

        }

        #endregion

        #region Flat DropShadow Form

        protected class MetroFlatDropShadow : MetroShadowBase
        {
            public MetroFlatDropShadow(Form targetForm)
                : base(targetForm, 5, WS_EX_TRANSPARENT | WS_EX_NOACTIVATE)
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.UserPaint, true);
                Opacity = 0.2;
            }

            protected override void ClearIt()
            {
                Visible = false;
            }

            protected override void PaintIt()
            {
                Visible = true;
                Invalidate(false);
                Update();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.Clear(Color.Gray);

                using (Brush b = new SolidBrush(Color.Black))
                {
                    e.Graphics.FillRectangle(b, new Rectangle(4, 4, ClientRectangle.Width - 8, ClientRectangle.Height - 8));
                }
            }

        }

        #endregion

        #region Windows7 DropShadow Form

        protected class MetroRealisticDropShadow : MetroShadowBase
        {
            public MetroRealisticDropShadow(Form targetForm) 
                : base(targetForm, 15, WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE)
            {
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                PaintIt();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Visible = true;
                PaintIt();
            }

            protected override void PaintIt()
            {
                using( Bitmap getShadow = DrawBlurBorder() )
                    SetBitmap(getShadow, 255);
            }

            protected override void ClearIt()
            {
                Bitmap img = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);
                g.Clear(Color.Transparent);
                g.Flush();
                g.Dispose();
                SetBitmap(img, 255);
                img.Dispose();
            }

            #region Drawing methods

            [SecuritySafeCritical]
            private void SetBitmap(Bitmap bitmap, byte opacity)
            {
                if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                    throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

                IntPtr screenDc = WinApi.GetDC(IntPtr.Zero);
                IntPtr memDc = WinApi.CreateCompatibleDC(screenDc);
                IntPtr hBitmap = IntPtr.Zero;
                IntPtr oldBitmap = IntPtr.Zero;

                try
                {
                    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                    oldBitmap = WinApi.SelectObject(memDc, hBitmap);

                    SIZE size = new SIZE(bitmap.Width, bitmap.Height);
                    POINT pointSource = new POINT(0, 0);
                    POINT topPos = new POINT(Left, Top);
                    WinApi.BLENDFUNCTION blend = new WinApi.BLENDFUNCTION
                        {
                            BlendOp = WinApi.AC_SRC_OVER,
                            BlendFlags = 0,
                            SourceConstantAlpha = opacity,
                            AlphaFormat = WinApi.AC_SRC_ALPHA
                        };

                    WinApi.UpdateLayeredWindow(Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, WinApi.ULW_ALPHA);
                }
                finally
                {
                    WinApi.ReleaseDC(IntPtr.Zero, screenDc);
                    if (hBitmap != IntPtr.Zero)
                    {
                        WinApi.SelectObject(memDc, oldBitmap);
                        WinApi.DeleteObject(hBitmap);
                    }
                    WinApi.DeleteDC(memDc);
                }
            }

            private Bitmap DrawBlurBorder()
            {
                return (Bitmap)DrawOutsetShadow(0, 0, 40, 1, Color.Black, new Rectangle(1, 1, ClientRectangle.Width, ClientRectangle.Height));
            }

            private Image DrawOutsetShadow(int hShadow, int vShadow, int blur, int spread, Color color, Rectangle shadowCanvasArea)
            {
                Rectangle rOuter = shadowCanvasArea;
                Rectangle rInner = shadowCanvasArea;
                rInner.Offset(hShadow, vShadow);
                rInner.Inflate(-blur, -blur);
                rOuter.Inflate(spread, spread);
                rOuter.Offset(hShadow, vShadow);

                Rectangle originalOuter = rOuter;

                Bitmap img = new Bitmap(originalOuter.Width, originalOuter.Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                var currentBlur = 0;
                do
                {
                    var transparency = (rOuter.Height - rInner.Height) / (double)(blur * 2 + spread * 2);
                    var shadowColor = Color.FromArgb(((int)(200 * (transparency * transparency))), color);
                    var rOutput = rInner;
                    rOutput.Offset(-originalOuter.Left, -originalOuter.Top);

                    DrawRoundedRectangle(g, rOutput, currentBlur, Pens.Transparent, shadowColor);
                    rInner.Inflate(1, 1);
                    currentBlur = (int)((double)blur * (1 - (transparency * transparency)));

                } while (rOuter.Contains(rInner));

                g.Flush();
                g.Dispose();

                return img;
            }

            private void DrawRoundedRectangle(Graphics g, Rectangle bounds, int cornerRadius, Pen drawPen, Color fillColor)
            {
                int strokeOffset = Convert.ToInt32(Math.Ceiling(drawPen.Width));
                bounds = Rectangle.Inflate(bounds, -strokeOffset, -strokeOffset);

                var gfxPath = new GraphicsPath();

                if (cornerRadius > 0)
                {
                    gfxPath.AddArc(bounds.X, bounds.Y, cornerRadius, cornerRadius, 180, 90);
                    gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y, cornerRadius, cornerRadius, 270, 90);
                    gfxPath.AddArc(bounds.X + bounds.Width - cornerRadius, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
                    gfxPath.AddArc(bounds.X, bounds.Y + bounds.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
                }
                else
                {
                    gfxPath.AddRectangle(bounds);
                }

                gfxPath.CloseAllFigures();

                if (cornerRadius > 5)
                {
                    using (SolidBrush b = new SolidBrush(fillColor))
                    {
                        g.FillPath(b, gfxPath);
                    }
                }
                if (drawPen != Pens.Transparent)
                {
                    using (Pen p = new Pen(drawPen.Color))
                    {
                        p.EndCap = p.StartCap = LineCap.Round;
                        g.DrawPath(p, gfxPath);
                    }
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Helper Methods

        [SecuritySafeCritical]
        public void RemoveCloseButton()
        {
            IntPtr hMenu = WinApi.GetSystemMenu(Handle, false);
            if (hMenu == IntPtr.Zero) return;

            int n = WinApi.GetMenuItemCount(hMenu);
            if (n <= 0) return;

            WinApi.RemoveMenu(hMenu, (uint)(n - 1), WinApi.MfByposition | WinApi.MfRemove);
            WinApi.RemoveMenu(hMenu, (uint)(n - 2), WinApi.MfByposition | WinApi.MfRemove);
            WinApi.DrawMenuBar(Handle);
        }

        private Rectangle MeasureText(Graphics g, Rectangle clientRectangle, Font font, string text, TextFormatFlags flags)
        {
            var proposedSize = new Size(int.MaxValue, int.MinValue);
            var actualSize = TextRenderer.MeasureText(g, text, font, proposedSize, flags);
            return new Rectangle(clientRectangle.X, clientRectangle.Y, actualSize.Width, actualSize.Height);
        }

        #endregion
    }
}
