using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    public interface IModuleControl
    {
        ModuleControlInfo CreateModuleControl(ModuleControlInfo moduleControl);

        ModuleControlCollection GetAllModuleControls();

        ModuleControlInfo GetModuleControl(string controlId);

        bool UpdateControlParameters(string controlId, string parameters);
    }
}
