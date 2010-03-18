//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：显示权限列表。
// 完成日期：2010年03月17日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.Permission;

namespace UniqueStudio.Admin.admin.background
{
    public partial class permissionlist : Controls.BasePage
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
                PermissionManager manager = new PermissionManager();
                rptList.DataSource = manager.GetAllPermissions(CurrentUser);
                rptList.DataBind();
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败："+ex.Message);
            }
        }
    }
}
