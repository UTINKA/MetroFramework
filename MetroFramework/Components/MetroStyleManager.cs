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
using System.Diagnostics;
using System.Windows.Forms;
using MetroFramework.Interfaces;

namespace MetroFramework.Components
{
    /// <summary>
    ///     Cascading theme and styles management.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This class has multiple use cases:
    ///     </para>
    ///      <list type="numbered">
    ///     <item>
    ///         <description>
    ///             On the global level, it is instantited as a static singleton instance (<see cref="Default"/>).
    ///             Any changes applied here will be propagated to all active and future controls, unless 
    ///             more specific values are applied.
    ///             The global instance is instantiated using the default constructor and pulls default values
    ///             from <see cref="MetroDefaults"/>.
    ///         </description>
    ///         <description>
    ///             It can be added as a component to container controls (e.g. <see cref="MetroFramework.Forms.MetroForm"/>
    ///             or <see cref="MetroFramework.Controls.MetroUserControl"/>) to override settings 
    ///             on the container level. Any setting not specified is still managed by the global instance.
    ///             The designer is initializing components using the <see cref="MetroStyleManager(IContainer)"/>constructor
    ///             and the developer must assign the form to the style manager's _owner property.
    ///         </description>
    ///         <description>
    ///             On the control level, it acts as a property store for per-control overrides. 
    ///             Controls use the default constructor.
    ///         </description>
    ///     </item>
    ///     </list>
    /// </remarks>
    public sealed class MetroStyleManager : Component, ISupportInitialize, IMetroComponent, IMetroStyledComponent
    {

        public static readonly MetroStyleManager Default = new MetroStyleManager();

        private MetroColorStyle _metroStyle = MetroColorStyle.Default;
        [DefaultValue(MetroColorStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroColorStyle Style
        {
            get
            {
                return (DesignMode || _metroStyle != MetroColorStyle.Default) ? _metroStyle : _styleManager.Style ;
            }
            set
            {
                // The singleton instance must always have a non-default value
                if (_styleManager == null && value == MetroColorStyle.Default) value = MetroDefaults.Style;

                bool changed = Style != value;
                _metroStyle = value;
                if(changed) OnMetroStyleChanged(this, new EventArgs());
            }
        }

        private MetroThemeStyle _metroTheme = MetroThemeStyle.Default;
        [DefaultValue(MetroThemeStyle.Default)]
        [Category(MetroDefaults.CatAppearance)]
        public MetroThemeStyle Theme
        {
            get
            {
                return (DesignMode || _metroTheme != MetroThemeStyle.Default) ? _metroTheme : _styleManager.Theme ;
            }
            set
            {
                // The singleton instance must always have a non-default value
                if (_styleManager == null && value == MetroThemeStyle.Default) value = MetroDefaults.Theme;

                bool changed = Theme != value;
                _metroTheme = value;
                if(changed) OnMetroStyleChanged(this, new EventArgs());
            }         
        }

        private MetroStyleManager _styleManager;
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        MetroStyleManager IMetroStyledComponent.InternalStyleManager
        {
            get { return _styleManager; } 
            set
            {
                if(value==null) throw new ArgumentNullException();
                if(value==this) throw new ArgumentException();
                bool changed = value.Theme != Theme || value.Style != Style;
                if( _styleManager != null) _styleManager.MetroStyleChanged -= OnMetroStyleChanged;
                _styleManager = value;
                _styleManager.MetroStyleChanged += OnMetroStyleChanged;
                if(changed) OnMetroStyleChanged(this, new EventArgs());
            }
        }

        public event EventHandler MetroStyleChanged;

        private void OnMetroStyleChanged(object sender, EventArgs e)
        {
            if (!isInitializing)
            {
                Update();
                var ev = MetroStyleChanged;
                if (ev != null) ev(this, e);
            }
        }

        public MetroStyleManager()
        {
            _styleManager = Default;
            if (_styleManager != null)
            {
                _styleManager.MetroStyleChanged += OnMetroStyleChanged;
            }
            else // we are the singleton instance - provide actua defaults here
            {
                _metroTheme = MetroDefaults.Theme;
                _metroStyle = MetroDefaults.Style;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _styleManager != null)
            {
                _styleManager.MetroStyleChanged -= OnMetroStyleChanged;
                // NOTE: we don't dispose _styleManager as we don't have ownership !
            }
            base.Dispose(disposing);
        }

        /// <summary>
        ///     The Container owning the components. This is usualy the form's "components" field, not the form itself.
        /// </summary>
        private readonly IContainer _parentContainer;

        public MetroStyleManager(IContainer parentContainer)
            : this()
        {
            if (parentContainer != null)
            {
                this._parentContainer = parentContainer;
                this._parentContainer.Add(this);
            }
        }

        #region ISupportInitialize

        /// <summary>
        ///     Defer propagating style information until all controls and components have ben added 
        ///     and all properties have been set.
        /// </summary>
        private bool isInitializing;

        void ISupportInitialize.BeginInit()
        {
            isInitializing = true;
        }

        void ISupportInitialize.EndInit()
        {
            isInitializing = false;
            Update();
        }

        #endregion

        /// <summary>
        ///     The container control (e.g. form or user control) this style manager is managing.
        /// </summary>
        private IMetroContainerControl _owner;

        public IMetroContainerControl Owner
        {
            get { return _owner; }
            set
            {
                // We attach to ControlAdded to propagate styles to dynamically added controls

                if (_owner != null) 
                {
                    if ( _owner.StyleManager == this) _owner.StyleManager = null;
                    ContainerControl cc = _owner as ContainerControl;
                    if( cc != null ) cc.ControlAdded -= ControlAdded;
                }

                _owner = value;

                if (value != null)
                {
                    value.StyleManager = this;
                    ContainerControl cc = _owner as ContainerControl;
                    if (cc != null) cc.ControlAdded += ControlAdded;

                    if (!isInitializing)
                    {
                        UpdateControl((Control)value);
                    }
                }
            }
        }

         private void ControlAdded(object sender, ControlEventArgs e)
        {
            if (!isInitializing)
            {
                UpdateControl(e.Control);
            }
        } 
        
        public void Update()
        {
            if (_owner != null)
            {
                UpdateControl((Control)_owner);
            }

            if (_parentContainer == null || _parentContainer.Components == null)
            {
                return;
            }

            // propagate style information to components, i.e. MetroStyleExtender
            foreach (Object obj in _parentContainer.Components)
            {
                // avoid infinite loops
                if (ReferenceEquals(obj, this)) continue;
                IMetroStyledComponent c = obj as IMetroStyledComponent;
                if (c != null) c.InternalStyleManager = this;
            }
        }

        private void UpdateControl(Control control)
        {
            if (control == null)
            {
                return;
            }

            // If a container conrol is exposing a Style Manager, we link to it
            // but do not access the container's children.
            IMetroContainerControl container = control as IMetroContainerControl;
            if (container != null && container.StyleManager != null && !ReferenceEquals(this, container.StyleManager))
            {
                ((IMetroStyledComponent)container.StyleManager).InternalStyleManager = this;
                return;
            }

            // Link to metro controls
            IMetroStyledComponent styledComponent = control as IMetroStyledComponent;
            if (styledComponent != null)
            {
                styledComponent.InternalStyleManager = this;
            }

            if (control.ContextMenuStrip != null)
            {
                UpdateControl(control.ContextMenuStrip);
            }

            // descend into tab pages
            TabControl tabControl = control as TabControl;
            if (tabControl != null)
            {
                foreach (TabPage tp in tabControl.TabPages)
                {
                    UpdateControl(tp);
                }
            }

            // descend into child controls
            if (control.Controls != null)
            {
                foreach (Control child in control.Controls)
                {
                    UpdateControl(child);
                }
            }

        }

    }
}
