using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace UniqueStudio.Core.Module
{
    public interface IModule
    {
        string RenderContents(int siteId, string controlName, NameValueCollection queryString);
    }
}
