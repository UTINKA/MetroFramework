using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Components
{
    /// <summary>
    ///     Extend legacy controls with an <c>ApplyMetroTheme</c> property.
    /// </summary>
    /// <remarks>
    ///     The theme is applied to <see cref="Control.BackColor"/> and <see cref="Control.ForeColor"/> only
    ///     as these properties can change on a global style update.
    /// </remarks>
    /// <seelso href="http://www.codeproject.com/Articles/4683/Getting-to-know-IExtenderProvider"/>
    [ProvideProperty("ApplyMetroTheme", typeof(Control))] // we can provide more than one property if we like
    public sealed class MetroStyleExtender : Component, IExtenderProvider, IMetroComponent, IMetroStyledComponent
    {

        private MetroStyleManager _styleManager;

        public MetroStyleExtender()
        {
            _styleManager = new MetroStyleManager();
            _styleManager.MetroStyleChanged += OnMetroStyleChanged;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing) _styleManager.Dispose();
            base.Dispose(disposing);
        }
        
        #region IMetroStyledComponent implementation

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
            UpdateTheme();
        }

        // Override Site property to set the style manager into design mode, too.
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = _styleManager.Site = value; }
        }

        #endregion IStyleManager

        #region IExtenderProvider support

        // TODO: Need something more performant here if we extend a large number of controls on each form
        private readonly List<Control> _extendedControls = new List<Control>();

        public MetroStyleExtender(IContainer parent)
            : this()
        {
            if (parent != null)
            {
                parent.Add(this);
            }
        }

        // This might be called from a thread other than the control's UI thread
        private void UpdateTheme()
        {
            if (_extendedControls.Count == 0) return;
            Control c = _extendedControls[0];
            if (c.InvokeRequired)
            {
                // assume all contros are on the same form / same UI thread
                c.Invoke(new MethodInvoker(UpdateTheme));
                return;
            }

            Color backColor = MetroPaint.BackColor.Form(_styleManager.Theme);
            Color foreColor = MetroPaint.ForeColor.Label.Normal(_styleManager.Theme);

            foreach (Control ctrl in _extendedControls)
            {
                try
                {
                    if( ctrl.BackColor != backColor) ctrl.BackColor = backColor;
                }
                catch { }
                try
                {
                    if( ctrl.ForeColor != foreColor) ctrl.ForeColor = foreColor;
                }
                catch { }
            }
        }

        // Thuis must only be called from the control's UI thread
        private void UpdateTheme(Control ctrl)
        {
            Color backColor = MetroPaint.BackColor.Form(_styleManager.Theme);
            Color foreColor = MetroPaint.ForeColor.Label.Normal(_styleManager.Theme);

            try
            {
                if (ctrl.BackColor != backColor) ctrl.BackColor = backColor;
            }
            catch { }
            try
            {
                if (ctrl.ForeColor != foreColor) ctrl.ForeColor = foreColor;
            }
            catch { }
        }

        #endregion

        #region IExtenderProvider implementation

        bool IExtenderProvider.CanExtend(object target)
		{
		    return target is Control && !(target is IMetroControl || target is IMetroForm);
		}

        [DefaultValue(false)]
        [Category(MetroDefaults.CatAppearance)]
        [Description("Apply Metro Theme BackColor and ForeColor.")]
        public bool GetApplyMetroTheme(Control control)
		{
		    return control != null && _extendedControls.Contains(control);
		}

        // TODO: Allow specifying the properties to override.

        public void SetApplyMetroTheme(Control control, bool value)
        {
            if (control == null)
            {
                return;
            }

            if (_extendedControls.Contains(control))
            {
                if (!value)
                {
                    _extendedControls.Remove(control);
                }
            }
            else
            {
                if (value)
                {
                    _extendedControls.Add(control);
                    UpdateTheme(control);
                }
            }
        }

        #endregion

    }
}
