using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface IPageVisit
    {
        bool AddPageVisit(PageVisitInfo pv);

        int GetPageVisitCount();

        PageVisitCollection GetPageVisitList(int pageIndex,int pageSize);
    }
}
