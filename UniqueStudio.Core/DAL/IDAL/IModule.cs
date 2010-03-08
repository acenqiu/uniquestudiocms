using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface IModule
    {
        ModuleCollection GetAllModules();

        ModuleInfo GetModule(int moduleId);

        ModuleInfo CreateModule(ModuleInfo module);
    }
}
