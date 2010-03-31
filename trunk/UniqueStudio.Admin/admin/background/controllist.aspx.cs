//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：创建控件及控件列表页面。
// 完成日期：2010年03月30日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Module;

namespace UniqueStudio.Admin.admin.background
{
    public partial class controllist : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    ModuleManager manager = new ModuleManager();
                    ddlModules.DataSource = manager.GetAllModules(CurrentUser);
                    ddlModules.DataTextField = "ModuleName";
                    ddlModules.DataValueField = "ModuleId";
                    ddlModules.DataBind();
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("数据读取失败：" + ex.Message);
                }

                GetData();
            }
        }

        private void GetData()
        {
            try
            {
                txtControlName.Text = string.Empty;

                ModuleControlManager manager = new ModuleControlManager();
                rptList.DataSource = manager.GetAllModuleControls(CurrentUser, SiteId);
                rptList.DataBind();
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            ModuleControlInfo control = new ModuleControlInfo();
            control.ControlName = txtControlName.Text.Trim();
            control.SiteId = SiteId;
            control.ModuleId = Converter.IntParse(ddlModules.SelectedValue, 0);
            control.IsEnabled = chkIsEnabled.Checked;

            try
            {
                if ((new ModuleControlManager()).CreateModuleControl(CurrentUser, control))
                {
                    message.SetSuccessMessage("控件创建成功！");
                    GetData();
                }
                else
                {
                    message.SetErrorMessage("控件创建失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("控件创建失败：" + ex.Message);
            }
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            ModuleControlManager manager = new ModuleControlManager(CurrentUser);
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
                    case "start":
                        if (manager.StartModuleControls(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选控件已启用！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选控件启用失败！");
                        }
                        break;
                    case "stop":
                        if (manager.StopModuleControls(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选控件已停用！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选控件停用失败！");
                        }
                        break;
                    case "delete":
                        if (manager.DeleteModuleControls(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选控件已删除！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选控件删除失败！");
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
