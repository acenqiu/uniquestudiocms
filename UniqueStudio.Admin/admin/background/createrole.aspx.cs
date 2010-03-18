//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：创建角色页面。
// 完成日期：2010年03月17日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Permission;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin.background
{
    public partial class createrole : Controls.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    rptPermissions.DataSource = (new PermissionManager()).GetAllPermissions(CurrentUser);
                    rptPermissions.DataBind();

                    SiteCollection sites = (new SiteManager()).GetAllSites();
                    ddlSites.Items.Clear();
                    ddlSites.Items.Add(new ListItem("所有网站", "0"));
                    foreach (SiteInfo site in sites)
                    {
                        ddlSites.Items.Add(new ListItem(site.SiteName, site.SiteId.ToString()));
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("数据读取失败："+ex.Message);
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            RoleManager manager = new RoleManager();
            RoleInfo role = new RoleInfo();
            role.RoleName = txtRoleName.Text.Trim();
            role.Description = txtDescription.Text.Trim();
            role.SiteId = Converter.IntParse(ddlSites.SelectedValue, 0);
            role.Permissions = new PermissionCollection();
            if (Request.Form["chkSelected_p"] != null)
            {
                string[] ids = Request.Form["chkSelected_p"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ids.Length; i++)
                {
                    role.Permissions.Add(new PermissionInfo(Converter.IntParse(ids[i], 0)));
                }
            }

            try
            {
                role = manager.CreateRole(CurrentUser, role);
                if (role != null)
                {
                    message.SetSuccessMessage("角色创建成功！");
                    txtRoleName.Text = "";
                    txtDescription.Text = "";
                }
                else
                {
                    message.SetErrorMessage("角色创建失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("角色创建失败：" + ex.Message);
            }
        }
    }
}
