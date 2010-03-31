using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Core.Module
{
    public interface IModule
    {
        string RenderContents(int siteId, string controlName);
    }
}
