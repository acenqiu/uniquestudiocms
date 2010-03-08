using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface IPlugIn
    {
        PlugInInfo CreatePlugIn(PlugInInfo plugIn);

        PlugInCollection GetAllPlugIns();

        ClassCollection GetAllPlugInsForInit();

        bool DeletePlugIn(int plugInId);
    }
}
