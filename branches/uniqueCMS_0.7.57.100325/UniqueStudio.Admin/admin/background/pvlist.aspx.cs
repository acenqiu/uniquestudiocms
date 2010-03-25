//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：页面访问列表。
// 完成日期：2010年03月20日
// 版本：v0.3
// 作者：邱江毅
//=================================================================
using System;
using System.Text;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.PageVisit;

namespace UniqueStudio.Admin.admin.background
{
    public partial class pvlist : Controls.AdminBasePage
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
                PageVisitCollection collection = manager.GetPageVisitList(CurrentUser, pageIndex, 50);
                rptList.DataSource = collection;
                rptList.DataBind();

                pagination.CurrentPage = collection.PageIndex;
                pagination.Count = collection.PageCount;
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }
    }
}
