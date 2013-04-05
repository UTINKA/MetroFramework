using System;
using System.Collections.Generic;
using System.Text;
using MetroFramework.Components;

namespace MetroFramework.Interfaces
{
    public interface IMetroStyledComponent
    {
        MetroStyleManager InternalStyleManager { get; set; }
    }
}
