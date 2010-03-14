using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UniqueStudio.Core.PageVisit;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Admin.admin.background
{
    public partial class pvlist : System.Web.UI.Page
    {
        private int pageIndex;

        protected void Page_Load(object sender, EventArgs e)
        {
            pageIndex = Converter.IntParse(Request.QueryString["page"], 1);
            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            PageVisitManager manager = new PageVisitManager();
            try
            {
                UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
                PageVisitCollection collection = manager.GetPageVisitList(currentUser, pageIndex,50);
                rptList.DataSource = collection;
                rptList.DataBind();

                pagination.CurrentPage = collection.PageIndex;
                pagination.Count = collection.PageCount;
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
