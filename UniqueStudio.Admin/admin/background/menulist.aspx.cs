//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：菜单列表。
// 完成日期：2010年03月16日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Menu;

namespace UniqueStudio.Admin.admin.background
{
    public partial class menulist : Controls.BasePage
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
                rptList.DataSource = (new MenuManager()).GetAllMenus(SiteId);
                rptList.DataBind();
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据获取失败：" + ex.Message);
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            MenuInfo menu = new MenuInfo();
            menu.SiteId = SiteId;
            menu.MenuName = txtMenuName.Text.Trim();
            menu.Description = txtDescription.Text.Trim();

            try
            {
                menu = (new MenuManager()).CreateMenu(CurrentUser, menu);
                if (menu != null)
                {
                    message.SetSuccessMessage("菜单创建成功！");
                    GetData();
                }
                else
                {
                    message.SetErrorMessage("菜单创建失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("菜单创建失败：" + ex.Message);
            }
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            MenuManager manager = new MenuManager(CurrentUser);
            List<int> list = new List<int>();
            if (Request.Form["chkSelected"] != null)
            {
                string[] ids = Request.Form["chkSelected"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ids.Length; i++)
                {
                    list.Add(Converter.IntParse(ids[i], 0));
                }
            }
            else
            {
                return;
            }

            if (ddlOperation.SelectedValue == "delete")
            {
                try
                {
                    if (manager.DeleteMenus(list.ToArray()))
                    {
                        message.SetSuccessMessage("所选菜单已删除！");
                    }
                    else
                    {
                        message.SetErrorMessage("所选菜单删除失败！");
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("所选菜单删除失败：" + ex.Message);
                }
                GetData();
            }
        }
    }
}
