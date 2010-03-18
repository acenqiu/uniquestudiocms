//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：显示角色列表。
// 完成日期：2010年03月17日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;

using UniqueStudio.Core.Permission;
using UniqueStudio.Common;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.Admin.admin.background
{
    public partial class rolelist : Controls.BasePage
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
                RoleManager manager = new RoleManager();
                rptList.DataSource = manager.GetAllRoles(CurrentUser);
                rptList.DataBind();
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            RoleManager manager = new RoleManager();
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
                    if (manager.DeleteRoles(CurrentUser, list.ToArray()))
                    {
                        message.SetSuccessMessage("指定角色已删除！");
                        GetData();
                    }
                    else
                    {
                        message.SetErrorMessage("指定角色删除失败！");
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("指定角色删除失败：" + ex.Message);
                }
            }
        }
    }
}
