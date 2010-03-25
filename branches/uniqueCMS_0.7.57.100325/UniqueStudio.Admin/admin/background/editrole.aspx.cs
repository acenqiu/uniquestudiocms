//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：编辑角色页面。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//
// 修改记录1：
// 修改日期：2010年03月21日
// 版本号：v1.0 alpha
// 修改人：邱江毅
// +) 原始角色名
//=================================================================
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Permission;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin.background
{
    public partial class editrole : Controls.AdminBasePage
    {
        private int roleId;
        private Dictionary<int, bool> inPermissions;

        protected void Page_Load(object sender, EventArgs e)
        {
            roleId = Converter.IntParse(Request.QueryString["roleId"], 0);
            if (roleId == 0)
            {
                if (Request.QueryString["ret"] != null)
                {
                    //此处可能有问题
                    Response.Redirect("rolelist.aspx" + HttpUtility.UrlDecode(Request.QueryString["ret"]));
                }
                else
                {
                    Response.Redirect("rolelist.aspx");
                }
            }

            if (!IsPostBack)
            {
                try
                {
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
                    message.SetErrorMessage("数据读取失败：" + ex.Message);
                    return;
                }
                GetData();
            }
        }

        private void GetData()
        {
            try
            {
                RoleManager manager = new RoleManager();
                RoleInfo role = manager.GetRoleInfo(CurrentUser, roleId);

                txtRoleName.Text = role.RoleName;
                hfOldRoleName.Value = role.RoleName;
                txtDescription.Text = role.Description;
                ddlSites.SelectedValue = role.SiteId.ToString();

                inPermissions = new Dictionary<int, bool>(role.Permissions.Count);
                foreach (PermissionInfo permission in role.Permissions)
                {
                    inPermissions.Add(permission.PermissionId, true);
                }

                rptPermissions.ItemDataBound += new RepeaterItemEventHandler(rptPermissions_ItemDataBound);
                rptPermissions.DataSource = (new PermissionManager()).GetAllPermissions(CurrentUser);
                rptPermissions.DataBind();

                rptUsers.DataSource = role.Users;
                rptUsers.DataBind();
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }

        private void rptPermissions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (inPermissions != null)
            {
                Literal ltlSelected = (Literal)e.Item.FindControl("ltlSelected");
                if (ltlSelected != null)
                {
                    ltlSelected.Text = inPermissions.ContainsKey(((PermissionInfo)e.Item.DataItem).PermissionId) ? "checked='checked'" : string.Empty;
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["ret"] != null)
            {
                Response.Redirect("rolelist.aspx?" + HttpUtility.UrlDecode(Request.QueryString["ret"]));
            }
            else
            {
                Response.Redirect("rolelist.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            RoleInfo role = new RoleInfo();
            role.RoleId = roleId;
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
                if ((new RoleManager()).UpdateRole(CurrentUser, role, hfOldRoleName.Value))
                {
                    message.SetSuccessMessage("角色更新成功！");
                    GetData();
                }
                else
                {
                    message.SetErrorMessage("角色更新失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("角色更新失败：" + ex.Message);
            }
        }
    }
}
