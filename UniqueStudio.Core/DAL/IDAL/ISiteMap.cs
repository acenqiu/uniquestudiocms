using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface ISiteMap
    {
        PageInfo CreatePage(PageInfo page);

        bool DeletePage(int pageId);

        PageCollection GetAllPages();

        bool UpdatePage(PageInfo page);
    }
}
