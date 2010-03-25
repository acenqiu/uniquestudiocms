using System;

using UniqueStudio.Common.ErrorLogging;

namespace UniqueStudio.Admin.admin.background
{
    public partial class errorlog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            rptList.DataSource = ErrorLogger.GetAllErrors();
            rptList.DataBind();
        }
    }
}
