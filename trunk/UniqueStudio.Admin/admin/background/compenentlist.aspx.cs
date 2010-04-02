//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：组件列表页面。
// 完成日期：2010年04月02日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Compenent;

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

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            CompenentManager manager = new CompenentManager(CurrentUser);
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
                        if (manager.UninstallCompenents(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选组件已卸载！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选组件卸载失败！");
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
