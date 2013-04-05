 
 
/**************************************************************************************

                        GENERATED FILE - DO NOT EDIT

 **************************************************************************************/

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
 
