//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：插件列表页面。
// 完成日期：2010年03月28日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.PlugIn;

namespace UniqueStudio.Admin.admin.background
{
    public partial class pluginlist : Controls.AdminBasePage
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
                PlugInManager manager = new PlugInManager();
                PlugInCollection collection = manager.GetAllPlugIns(CurrentUser);
                rptList.DataSource = collection;
                rptList.DataBind();
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            PlugInManager manager = new PlugInManager(CurrentUser);
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

            try
            {
                switch (ddlOperation.SelectedValue)
                {
                    case "uninstall":
                        if (manager.UninstallPlugIns(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选插件已卸载！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选插件卸载失败！");
                        }
                        break;                  
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("所执行的批量操作失败：" + ex.Message);
            }
        }
    }
}
