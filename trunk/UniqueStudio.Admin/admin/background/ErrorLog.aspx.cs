//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：错误日志页面。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;
using System.IO;
using System.Web.UI.WebControls;

using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Permission;

namespace UniqueStudio.Admin.admin.background
{
    public partial class errorlog : Controls.AdminBasePage
    {
        private const string LOG_FOLDER = "admin/xml/log/";

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
                CheckPermission();

                string[] files = Directory.GetFiles(Server.MapPath("~/" + LOG_FOLDER));
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        FileInfo info = new FileInfo(file);
                        ddlLogs.Items.Add(new ListItem(info.Name, info.Name));
                    }
                }
                if (ddlLogs.Items.Count == 0)
                {
                    ddlLogs.Items.Add(new ListItem("暂无日志文件", "none"));
                }

                rptList.DataSource = ErrorLogger.GetAllErrors(DateTime.Now);
                rptList.DataBind();
                ltlFileName.Text = DateTime.Now.ToString("yyyyMMdd") + ".xml";
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据获取失败：" + ex.Message);
            }
        }

        protected void ddlLogs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CheckPermission();

                if (ddlLogs.SelectedValue != "none")
                {
                    rptList.DataSource = ErrorLogger.GetAllErrors(ddlLogs.SelectedValue);
                    rptList.DataBind();
                    ltlFileName.Text = ddlLogs.SelectedValue;
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据获取失败：" + ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                CheckPermission();

                DateTime date = Converter.DatetimeParse(txtDate.Text, DateTime.Now);
                rptList.DataSource = ErrorLogger.GetAllErrors(date);
                rptList.DataBind();
                ltlFileName.Text = date.ToString("yyyyMMdd") + ".xml";
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据获取失败：" + ex.Message);
            }
        }

        private void CheckPermission()
        {
            PermissionManager.CheckPermission(CurrentUser, "ViewErrorLog", "查看日志信息");
        }
    }
}
