using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UniqueStudio.Core.Compenent;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class compenentlist : Controls.AdminBasePage
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
            try
            {
                CompenentManager manager = new CompenentManager();
                CompenentCollection collection = manager.GetAllCompenents(CurrentUser);
                rptList.DataSource = collection;
                rptList.DataBind();
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }
    }
}
