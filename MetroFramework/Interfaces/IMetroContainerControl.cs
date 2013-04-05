using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Components;

namespace MetroFramework.Interfaces
{
    public interface IMetroContainerControl : IContainerControl, IMetroControl
    {
        /// <summary>
        ///     A style manager controlling this container and all child controls.
        /// </summary>
        /// <remarks>
        ///     To assign a Style Manager to a <see cref="Forms.MetroForm"/> or
        ///     <see cref="Controls.MetroUserControl"/>, add a <see cref="Components.MetroStyleManager"/>
        ///     component to the designer and assign the form or user control as Owner.
        /// </remarks>
        MetroStyleManager StyleManager { get; set; }
    }
}
