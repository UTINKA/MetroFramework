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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Controls;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Native;

namespace MetroFramework.Forms
{

    public partial class MetroForm : MetroFormBase, IMetroForm
    {

        #region Properties

        private HorizontalAlignment textAlign = HorizontalAlignment.Left;
        [Browsable(true)]
        [Category(MetroDefaults.CatAppearance)]
        public HorizontalAlignment TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; Invalidate(); }
        }

        private bool isMovable = true;
        [Category(MetroDefaults.CatAppearance)]
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
                Invalidate();
            }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(20, DisplayHeader ? 60 : 20, 20, 20); }
        }

        private bool displayHeader = true;
        [Category(MetroDefaults.CatAppearance)]
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
                Invalidate();
            }
        }

        private bool isResizable = true;
        [Category(MetroDefaults.CatAppearance)]
        public bool Resizable
        {
            get { return isResizable; }
            set { isResizable = value; }
        }

        private MetroFormShadowType shadowType = MetroFormShadowType.Default;
        [Category(MetroDefaults.CatAppearance)]
        [DefaultValue(MetroFormShadowType.Default)]
        public MetroFormShadowType ShadowType
        {
            get { return shadowType; }
            set { shadowType = value; UpdateShadow(value); }
        }

        public new Form MdiParent
        {
            get { return base.MdiParent; }
            set
            {
                UpdateShadow(value == null ? ShadowType : MetroFormShadowType.None);
                base.MdiParent = value;
            }
        }

        /// <summary>
        ///     The colored border at the top of the form
        /// </summary>
        private const int BORDER_WIDTH = 5;

        #endregion

        #region Constructor

        public MetroForm()
        {

            // http://msdn.microsoft.com/en-gb/library/system.windows.forms.controlstyles.aspx
            // https://sites.google.com/site/craigandera/craigs-stuff/windows-forms/flicker-free-control-drawing

            SetStyle(
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |  // <-- prevents size handle artifacts
                ControlStyles.UserPaint, true);
            //UseBorderStyle();
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UpdateShadow(MetroFormShadowType.None);
            }

            base.Dispose(disposing);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (DesignMode) return;

            Microsoft.Win32.SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            UpdateShadow(ShadowType);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode) return;

            SessionSwitch("OnLoad");

            switch (IsMdiChild ? FormStartPosition.CenterParent : StartPosition)
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
                if (MaximizeBox) AddWindowButton(WindowButtons.Maximize);
                if (MinimizeBox) AddWindowButton(WindowButtons.Minimize);
                UpdateWindowButtonPosition();
            }
        }

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            /*
             *     ConsoleConnect = 1,
             *     ConsoleDisconnect = 2,
             *     RemoteConnect = 3,
             *     RemoteDisconnect = 4,
             *     SessionLogon = 5,
             *     SessionLogoff = 6,
             *     SessionLock = 7,
             *     SessionUnlock = 8,
             *     SessionRemoteControl = 9,
             */
            SessionSwitch(e.Reason.ToString());
        }

        protected bool IsRemoteSession { get; private set; }

        void SessionSwitch(string reason)
        {
            Debug.WriteLine("SessionSwitch, reason=" + reason + ", terminalServerSession=" + SystemInformation.TerminalServerSession +
                ", aeroThemeEnabled=" + IsAeroThemeEnabled() + ", dropShadowSupported=" + IsDropShadowSupported());

            //causes unpleasant re-draws in Terminal Server
            SetStyle(ControlStyles.AllPaintingInWmPaint, !SystemInformation.TerminalServerSession);

            bool oldVal = IsRemoteSession;
            IsRemoteSession = !IsDropShadowSupported() || !IsAeroThemeEnabled() || SystemInformation.TerminalServerSession;
            if (oldVal != IsRemoteSession)
            {
                OnMetroStyleChanged(new EventArgs());
                // TODO: Consider updating ControlStyles.AllPaintingInWmPaint - it looks better on console, but worse over remote
            }
        }

        #endregion

        #region Paint Methods

        protected override string MetroControlState
        {
            get { return IsRemoteSession && Enabled ? "Remote" : base.MetroControlState; }
        }

        /// <summary>
        ///     A border drawn inside the client area.
        /// </summary>
        /// <remarks>
        ///     This currently only supports <see cref="MetroBorderStyle.None"/> (the default) and 
        ///     <see cref="MetroBorderStyle.FixedSingle"/> (a thin grey line).
        /// </remarks>
        private MetroBorderStyle _borderStyle = MetroBorderStyle.Default;
        [DefaultValue(MetroBorderStyle.Default)]
        [Browsable(true)]
        [Category(MetroDefaults.CatAppearance)]
#pragma warning disable 109
        public new MetroBorderStyle BorderStyle
#pragma warning restore 109
        {
            get { return _borderStyle; }
            set { _borderStyle = value; Invalidate(); }
        }
        
        protected override void OnMetroStyleChanged(EventArgs e)
        {
            base.OnMetroStyleChanged(e);
            UpdateShadow(ShadowType);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try // without ControlStyles.AllPaintingInWmPaint, we need our own error handling
            {
                e.Graphics.Clear(EffectiveBackColor);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Invalidate();
            }
        }

        protected override void OnPaintForeground(PaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(GetStyleColor()))
                e.Graphics.FillRectangle(b, 0, 0, Width, BORDER_WIDTH);

            if (displayHeader)
            {
                // Assuming padding 20px on left/right; 20px from top; max 40px height
                Rectangle bounds = new Rectangle(20, 20, ClientRectangle.Width - 2 * 20, 40);
                TextRenderer.DrawText(e.Graphics, Text, GetThemeFont("Form.Title"), bounds, EffectiveForeColor, TextAlign.AsTextFormatFlags() | TextFormatFlags.EndEllipsis);
            }

            MetroBorderStyle bs = BorderStyle;
            if (bs == MetroBorderStyle.Default && !TryGetThemeProperty("BorderStyle", out bs))
                bs = MetroBorderStyle.None;

            if (bs == MetroBorderStyle.FixedSingle)
            {
                using (Pen pen = new Pen(GetThemeColor("BorderColor"))) // TODO: Use style color for active window?
                {
                    e.Graphics.DrawLines(pen, new[]
                                {
                                    new Point(0, BORDER_WIDTH),
                                    new Point(0, Height - 1),
                                    new Point(Width - 1, Height - 1),
                                    new Point(Width - 1, BORDER_WIDTH)
                                });
                }
            }

            if (Resizable && (SizeGripStyle == SizeGripStyle.Auto || SizeGripStyle == SizeGripStyle.Show))
            {
                using (SolidBrush b = new SolidBrush(GetThemeColor("Button.ForeColor.Disabled")) )
                {
                    Size resizeHandleSize = new Size(2, 2);
                    e.Graphics.FillRectangles(b, new Rectangle[] {
                        new Rectangle(new Point(ClientRectangle.Width-14,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-10,ClientRectangle.Height-10), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-6), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-10), resizeHandleSize),
                        new Rectangle(new Point(ClientRectangle.Width-6,ClientRectangle.Height-14), resizeHandleSize)
                    });
                }
            }
        }

        #endregion

        #region Management Methods

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (e.Cancel) return;

            // TODO: This looks wrong / assuming that there's only one window
            if (!(this is MetroTaskWindow))
                MetroTaskWindow.ForceClose();
        }

        protected override void OnClosed(EventArgs e)
        {
            UpdateShadow(MetroFormShadowType.None);
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


        //protected override void OnActivated(EventArgs e)
        //{
        //    base.OnActivated(e);

        //    if (DesignMode) return;

        //    // TODO: necessary??
        //    //Refresh();
        //}

        //protected override void OnEnabledChanged(EventArgs e)
        //{
        //    base.OnEnabledChanged(e);

        //    Invalidate();
        //}

        protected override void OnResizeBegin(EventArgs e)
        {
            base.OnResizeBegin(e);
            if (MetroDefaults.FormSuspendLayoutDuringResize||SystemInformation.TerminalServerSession) SuspendLayout();
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            if (MetroDefaults.FormSuspendLayoutDuringResize || SystemInformation.TerminalServerSession) ResumeLayout();
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

                case WinApi.Messages.WM_SIZING:
                    OnWmSizing(m.WParam, m.LParam);
                    break;

                case WinApi.Messages.WM_DWMCOMPOSITIONCHANGED:
                    // Vista API only - but we should never receive the message on XP
                    SessionSwitch("WM_DWMCOMPOSITIONCHANGED");
                    //UpdateSessionState();
                    break;

                case WinApi.Messages.WM_DISPLAYCHANGE:
                    SessionSwitch("WM_DISPLAYCHANGE");
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

            //  NOTE: MaxPosition is always relative to the origin of the window's current screen
            // e.g. usually (0, 0) unless the taskbar is on the left or top.

            Screen s = Screen.FromHandle(hwnd);
            pmmi->MaxSize = s.WorkingArea.Size;
            pmmi->MaxPosition.X = Math.Abs(s.WorkingArea.Left - s.Bounds.Left);
            pmmi->MaxPosition.Y = Math.Abs(s.WorkingArea.Top - s.Bounds.Top);

            // The form seems to fill these in just fine...
            //if (MinimumSize.Width > 0) pmmi->MinTrackSize.x = MinimumSize.Width;
            //if (MinimumSize.Height > 0) pmmi->MinTrackSize.y = MinimumSize.Height;
            //if (MaximumSize.Width > 0) pmmi->MaxTrackSize.x = MaximumSize.Width;
            //if (MaximumSize.Height > 0) pmmi->MaxTrackSize.y = MaximumSize.Height;

            // I guess these should have the normal window frame dimensions added to be correct
            // see SystemInformation.XXX
            _minTrackSize.Width = Math.Max(_minTrackSize.Width, pmmi->MinTrackSize.Width);
            _minTrackSize.Height = Math.Max(_minTrackSize.Height, pmmi->MinTrackSize.Height);
            _minTrackSize = SizeFromClientSize(_minTrackSize);
        }

        private Size _minTrackSize;

        [SecuritySafeCritical]
        private unsafe void OnWmSizing(IntPtr wParam, IntPtr lParam)
        {
            RECT* rc = (RECT*)lParam;
            rc->Width = Math.Max(rc->Width, _minTrackSize.Width);
            rc->Height = Math.Max(rc->Height, _minTrackSize.Height);
        }

        private WinApi.HitTest HitTestNCA(IntPtr hwnd, IntPtr wparam, IntPtr lparam)
        {
            Point vPoint = PointToClient(new Point(WinApi.LoWord((int)lparam), WinApi.HiWord((int)lparam)));
            var cr = ClientRectangle; 

            if (Resizable && SizeGripRectangle.Contains(vPoint))
                return WinApi.HitTest.HTBOTTOMRIGHT;

            // Caption area - TODO: vary height depending on DisplayHeader / Padding ?
            if ( CaptionRectangle.Contains(vPoint))
                return WinApi.HitTest.HTCAPTION;

            return WinApi.HitTest.HTCLIENT;
        }

        private Rectangle CaptionRectangle
        {
            get { return new Rectangle(BORDER_WIDTH, BORDER_WIDTH, ClientSize.Width - 2 * BORDER_WIDTH, 50); }
        }

        private Rectangle SizeGripRectangle
        {
            get
            {
                int padMax = Math.Max(Padding.Right, Padding.Bottom);
                var cs = ClientSize;
                return new Rectangle(cs.Width - padMax, cs.Height - padMax, padMax, padMax);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (Movable && WindowState != FormWindowState.Maximized && 
                e.Button == MouseButtons.Left && CaptionRectangle.Contains(e.Location))
            {
                MoveControl();                    
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
            // do not change values - they are used as index into _windowButtons below
            // they must be in right-to-left order, too
            Close = 0, 
            Maximize = 1,
            Minimize = 2,
        }

        private readonly MetroFormButton[] _windowButtons = new MetroFormButton[3];

        private void AddWindowButton(WindowButtons button)
        {
            if (_windowButtons[(int)button] != null ) throw new InvalidOperationException();

            MetroFormButton newButton = new MetroFormButton
                {
                    Text = GetButtonText(button),
                    Tag = button,
                    Size = new Size(25, 20),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };

            newButton.Click += WindowButton_Click;
            Controls.Add(newButton);
            _windowButtons[(int)button] = newButton;
        }

        private string GetButtonText(WindowButtons button)
        {
            switch (button)
            {
                case WindowButtons.Close: return "r";
                case WindowButtons.Minimize: return "0";
                case WindowButtons.Maximize: return WindowState == FormWindowState.Normal ? "1" : "2";
            }
            throw new InvalidOperationException();
        }

        private void WindowButton_Click(object sender, EventArgs e)
        {
            var btn = sender as MetroFormButton;
            if (btn == null) return;
            switch ((WindowButtons)btn.Tag)
            {
                case WindowButtons.Close:
                    Close();
                    return;
                case WindowButtons.Minimize:
                    WindowState = FormWindowState.Minimized;
                    return;
                case WindowButtons.Maximize:
                    WindowState = (WindowState == FormWindowState.Normal) ? FormWindowState.Maximized : FormWindowState.Normal;
                    btn.Text = GetButtonText(WindowButtons.Maximize);
                    return;
            }
            throw new InvalidOperationException();
        }

        private void UpdateWindowButtonPosition()
        {
            if (!ControlBox) return;

            Point location = new Point(ClientRectangle.Width - BORDER_WIDTH - 25, BORDER_WIDTH);
            foreach (var metroFormButton in _windowButtons.Where(metroFormButton => metroFormButton != null))
            {
                metroFormButton.Location = location;
                location.Offset(-25, 0);
            }
        }

        private class MetroFormButton : MetroButtonBase
        {
            private bool isHovered;
            private bool isPressed;

            public MetroFormButton()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            }

            #region Paint Methods

            protected override string MetroControlState
            {
                get { if (!Enabled) return "Disabled"; if (!isHovered) return "Normal"; return isPressed ? "Press" : "Hover"; }
            }

            public override Color EffectiveBackColor
            {
                get
                {
                switch (MetroControlState)
                {
                    case "Normal": 
                            var c = Parent as IMetroControl;    // TODO: Cant we just draw transparent??
                        if (c != null) return c.EffectiveBackColor;
                        break;
                    case "Press": 
                        return GetStyleColor();
                }
                    return base.EffectiveBackColor;
            }
            }

            protected override void OnPaintBackground(PaintEventArgs e)
            {
                try
                {
                    e.Graphics.Clear(EffectiveBackColor);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                    Invalidate();
                }
            }

            protected override void OnPaintForeground(PaintEventArgs e)
            {
                Font buttonFont = new Font("Webdings", 9.25f);
                TextRenderer.DrawText(e.Graphics, Text, buttonFont, ClientRectangle, EffectiveForeColor, EffectiveBackColor, 
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
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

        protected override void SetVisibleCore(bool value)
        {
            // Try to make the shadow visible before us (we are owned by the shadow form!)
            if (_shadowForm != null) _shadowForm.Visible = value;
            base.SetVisibleCore(value);
        }

        public enum MetroFormShadowType
        {
            Default = -1,
            None = 0,
            Flat,
            DropShadow,
            SystemDropShadow,
            SystemAeroShadow
        }

        private Form _shadowForm;

        private MetroFormShadowType _currentShadowType = MetroFormShadowType.None;

        private void UpdateShadow(MetroFormShadowType newShadowType)
        {
            if (DesignMode || !IsHandleCreated || IsDisposed) return;

            if (newShadowType == MetroFormShadowType.Default && !TryGetThemeProperty("ShadowType", out newShadowType))
                newShadowType = MetroDefaults.FormShadowType;

            if (_currentShadowType == newShadowType) return;

            if (_currentShadowType == MetroFormShadowType.SystemDropShadow)
            {
                SetCsDropShadow(false);
            }
            else if (_shadowForm != null)
            {
                if(!_shadowForm.IsDisposed) _shadowForm.Dispose();
                _shadowForm = null;
            }

            switch (newShadowType)
            {
                case MetroFormShadowType.Flat:
                    _shadowForm = new MetroFlatDropShadow(this);
                    break;

                case MetroFormShadowType.DropShadow:
                    _shadowForm = new MetroRealisticDropShadow(this);
                    break;

                case MetroFormShadowType.SystemAeroShadow:
                    _shadowForm = new MetroAeroDropShadow(this);
                    break;

                case MetroFormShadowType.SystemDropShadow:
                    SetCsDropShadow(true);
                    break;
            }

            _currentShadowType = newShadowType;
        }

        [SecuritySafeCritical]
        private static bool IsAeroThemeEnabled()
        {
            if (Environment.OSVersion.Version.Major <= 5) return false; // Make sure we dont even call the API !!

            bool aeroEnabled;
            DwmApi.DwmIsCompositionEnabled(out aeroEnabled);
            return aeroEnabled;
        }

        private static bool IsDropShadowSupported()
        {
            return Environment.OSVersion.Version.Major > 5 && SystemInformation.IsDropShadowEnabled;
        }

        [SecuritySafeCritical]
        private void SetCsDropShadow(bool hasShadow)
        {
            const int CS_DROPSHADOW = 0x20000;
            const int GCL_STYLE = -26;

            if (!IsDropShadowSupported())
                return;
            int classStyle = WinApi.GetClassLongPtr(Handle, GCL_STYLE).ToInt32();
            if (hasShadow)
                classStyle |= CS_DROPSHADOW;
            else
                classStyle &= ~CS_DROPSHADOW;
            WinApi.SetClassLongPtr(Handle, GCL_STYLE, new IntPtr(classStyle));
            //Is there a way to force Windows to update the class style ??
            //WinApi.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, WinApi.SetWindowPosFlags.FRAMECHANGED |
            //    WinApi.SetWindowPosFlags.NOMOVE | WinApi.SetWindowPosFlags.NOSIZE | WinApi.SetWindowPosFlags.NOZORDER);
            //WinApi.RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, WinApi.RedrawWindowFlags.Invalidate | WinApi.RedrawWindowFlags.Frame);
        }

        #region MetroShadowBase

        protected abstract class MetroShadowBase : Form
        {
            private Form TargetForm { get; set; }

            private readonly int _shadowSize;
            private readonly int _wsExStyle;

            private const int TICKS_PER_MS = 10000;
            private const long RESIZE_REDRAW_INTERVAL = 10*TICKS_PER_MS; 

            protected MetroShadowBase(Form targetForm, int shadowSize, int wsExStyle)
            {
                TargetForm = targetForm;
                _shadowSize = shadowSize;
                _wsExStyle = wsExStyle;

                MaximizeBox = false;
                MinimizeBox = false;
                ShowInTaskbar = false;
                ShowIcon = false;
                FormBorderStyle = FormBorderStyle.None;

                TargetForm.Activated += OnTargetFormActivated;
                TargetForm.ResizeBegin += OnTargetFormResizeBegin;
                TargetForm.ResizeEnd += OnTargetFormResizeEnd;
                TargetForm.VisibleChanged += OnTargetFormVisibleChanged;
                TargetForm.SizeChanged += OnTargetFormSizeChanged;

                TargetForm.Move += OnTargetFormMove;
                TargetForm.Resize += OnTargetFormResize;

                // When a form is owned by another form, it is closed or hidden with the owner form.
                // Owned forms are also never displayed behind their owner form. 

                if (TargetForm.Owner != null)
                    Owner = TargetForm.Owner;

                TargetForm.Owner = this;

                // This should "do the right thing" (tm)
                OnTargetFormVisibleChanged(this, new EventArgs());
            }

            [SecuritySafeCritical]
            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                Bounds = GetShadowBounds();
                //WinApi.SetWindowPos(Handle, IntPtr.Zero, 0, 0, 0, 0, WinApi.SetWindowPosFlags.FRAMECHANGED |
                //    WinApi.SetWindowPosFlags.NOMOVE | WinApi.SetWindowPosFlags.NOSIZE | WinApi.SetWindowPosFlags.NOZORDER);
                //WinApi.RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, WinApi.RedrawWindowFlags.Invalidate | WinApi.RedrawWindowFlags.Frame);
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    TargetForm.Activated -= OnTargetFormActivated;
                    TargetForm.ResizeBegin -= OnTargetFormResizeBegin;
                    TargetForm.ResizeEnd -= OnTargetFormResizeEnd;
                    TargetForm.VisibleChanged -= OnTargetFormVisibleChanged;
                    TargetForm.SizeChanged -= OnTargetFormSizeChanged;

                    TargetForm.Move -= OnTargetFormMove;
                    TargetForm.Resize -= OnTargetFormResize;

                    TargetForm.Owner = Owner; // dis-own so we don't dispose it
                    TargetForm = Owner = null;
                }
                base.Dispose(disposing);
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

            protected abstract void PaintShadow();

            protected abstract void ClearShadow();

            #region Helpers

            [SecuritySafeCritical]
            protected void SetBitmap(Bitmap bitmap, byte opacity)
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

                    Size size = new Size(bitmap.Width, bitmap.Height);
                    Point pointSource = new Point(0, 0);
                    Point topPos = new Point(Left, Top);
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

            #endregion

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
                // TODO: This is causing massive flickering - can this be prevented??
                //ClearShadow();
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

                PaintShadowIfVisible();
            }

            private void OnTargetFormResizeEnd(object sender, EventArgs e)
            {
                _lastResizedOn = 0;
                PaintShadowIfVisible();
            }

            private void PaintShadowIfVisible()
            {
                if (TargetForm.Visible && TargetForm.WindowState == FormWindowState.Normal)
                    PaintShadow();
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
                TransparencyKey = Color.Black;

            }

            protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
            {
                // ignore bogus size-only updates, we get better info from TargetForm
                if (specified == BoundsSpecified.Size) return;
                base.SetBoundsCore(x, y, width, height, specified);
            }

            protected override void PaintShadow() { Visible = true; }

            protected override void ClearShadow() { /* nothing */ }

            protected override void OnPaint(PaintEventArgs e)
            {
                //e.Graphics.Clear(Color.Black);
            }

        }

        #endregion

        #region Flat DropShadow Form

        protected class MetroFlatDropShadow : MetroShadowBase
        {
            private Point _offset = new Point(-6, -6);

            public MetroFlatDropShadow(Form targetForm) 
                : base(targetForm, 6, WS_EX_LAYERED | WS_EX_TRANSPARENT | WS_EX_NOACTIVATE)
            {
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);
                PaintShadow();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                PaintShadow();
            }

            protected override void PaintShadow()
            {
                Visible = true;
                using( Bitmap getShadow = DrawBlurBorder() )
                    SetBitmap(getShadow, 255);
            }

            protected override void ClearShadow()
            {
                // This flickers when running on console
                Bitmap img = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);
                g.Clear(Color.Transparent);
                g.Flush();
                g.Dispose();
                SetBitmap(img, 255);
                img.Dispose();
            }

            #region Drawing methods

            private Bitmap DrawBlurBorder()
            {
                return (Bitmap)DrawOutsetShadow(Color.Black, new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height));
            }

            private Image DrawOutsetShadow(Color color, Rectangle shadowCanvasArea)
            {
                Rectangle rOuter = shadowCanvasArea;
                Rectangle rInner = new Rectangle(shadowCanvasArea.X + (-_offset.X - 1), shadowCanvasArea.Y + (-_offset.Y - 1), shadowCanvasArea.Width - (-_offset.X * 2 - 1), shadowCanvasArea.Height - (-_offset.Y * 2 - 1));

                Bitmap img = new Bitmap(rOuter.Width, rOuter.Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                using (Brush bgBrush = new SolidBrush(Color.FromArgb(30, Color.Black)))
                {
                    g.FillRectangle(bgBrush, rOuter);
                }
                using (Brush bgBrush = new SolidBrush(Color.FromArgb(60, Color.Black)))
                {
                    g.FillRectangle(bgBrush, rInner);
                }

                g.Flush();
                g.Dispose();

                return img;
            }

            #endregion
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
                PaintShadow();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                PaintShadow();
            }

            protected override void PaintShadow()
            {
                Visible = true;
                using (Bitmap getShadow = DrawBlurBorder())
                    SetBitmap(getShadow, 255);
            }

            protected override void ClearShadow()
            {
                // This flickers when running on console, no matter if we clear the shadow nor not
                Bitmap img = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(img);
                g.Clear(Color.Transparent);
                g.Flush();
                g.Dispose();
                SetBitmap(img, 255);
                img.Dispose();
            }

            #region Drawing methods

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
        private void RemoveCloseButton()
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
