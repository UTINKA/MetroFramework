 
 
/**************************************************************************************

                        GENERATED FILE - DO NOT EDIT

 **************************************************************************************/
  /*
 
MetroFramework - Modern UI for WinForms

Copyright (c) 2013 Jens Thiel, http://github.com/thielj/winforms-modernui
Portions of this software are Copyright (c) 2011 Sven Walter, http://github.com/viperneo

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
 
 */

using System;
using System.ComponentModel;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroControlBase : Control, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroControlBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroButtonBase : Button, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroButtonBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroCheckBoxBase : CheckBox, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroCheckBoxBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroComboBoxBase : ComboBox, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroComboBoxBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroFormBase : Form, IMetroContainerControl, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroFormBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
	    /// <summary>
        ///     A style manager controlling this container and all child controls.
        /// </summary>
        /// <remarks>
        ///     To assign a Style Manager to a <see cref="Forms.MetroForm"/> or
        ///     <see cref="Controls.MetroUserControl"/>, add a <see cref="Components.MetroStyleManager"/>
        ///     component to the designer and assign the form or user control as Owner.
        /// </remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager { get; set; }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroLabelBase : Label, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroLabelBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroPanelBase : Panel, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroPanelBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroProgressBarBase : ProgressBar, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroProgressBarBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroRadioButtonBase : RadioButton, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroRadioButtonBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroTabControlBase : TabControl, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroTabControlBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroTabPageBase : TabPage, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroTabPageBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
		#endregion

    }


	[EditorBrowsable(EditorBrowsableState.Advanced)]
    public abstract class MetroUserControlBase : UserControl, IMetroContainerControl, 
		IMetroControl, IMetroStyledComponent
    {

		#region Fields, Constructor & IDisposable

        private readonly MetroStyleManager _styleManager;

	    protected MetroUserControlBase()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }
                   
        protected override void Dispose(bool disposing)
        {
            if (disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }

		#endregion

        #region Style Manager Interface

        /*  NOTE: when copying this code, make sure that the class creates and disposes the style manager, e.g.
          
                public MyControl()
                {
                    _styleManager = new MetroStyleManager();
                    _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                }
                   
                protected override void Dispose(bool disposing)
                {
                    if (disposing) _styleManager.Dispose();
                    base.Dispose(disposing);
                }
         
         */

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; }
            // NOTE: we don't replace our style manager, but instead assign the style manager a new manager
            set { ((IMetroStyledComponent)_styleManager).InternalStyleManager = value; }
        }

        // Event handler for our style manager's updates
        // NOTE: The event may have been triggered from a different thread.
        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(Invalidate));
            else
                Invalidate();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion

		#region Properties

        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get { return _styleManager.Theme; }
            set { _styleManager.Theme = value; }
        }

		[DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get { return _styleManager.Style; }
            set { _styleManager.Style = value; }
        }

	
	    /// <summary>
        ///     A style manager controlling this container and all child controls.
        /// </summary>
        /// <remarks>
        ///     To assign a Style Manager to a <see cref="Forms.MetroForm"/> or
        ///     <see cref="Controls.MetroUserControl"/>, add a <see cref="Components.MetroStyleManager"/>
        ///     component to the designer and assign the form or user control as Owner.
        /// </remarks>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager { get; set; }

	
		#endregion

    }


}
 
