//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：插件配置页面。
// 完成日期：2010年03月25日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.PlugIn;
using UniqueStudio.Core.Site;

namespace UniqueStudio.Admin.admin.background
{
    public partial class pluginconfig : Controls.AdminBasePage
    {
        protected int PlugInId;

        protected void Page_Load(object sender, EventArgs e)
        {
            PlugInId = Converter.IntParse(Request.QueryString["plugInId"], 0);
            if (PlugInId == 0)
            {
                Response.Redirect("pluginlist.aspx");
            }

            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            int instanceId = Converter.IntParse(Request.QueryString["instanceId"], 0);
            try
            {
                PlugInManager manager = new PlugInManager(CurrentUser);
                PlugInInfo plugIn = manager.GetPlugIn(PlugInId);
                if (plugIn != null)
                {
                    ltlPlugInName.Text = plugIn.PlugInName;
                    ltlDisplayName.Text = plugIn.DisplayName;
                    ltlAuthor.Text = plugIn.PlugInAuthor;
                    ltlCategory.Text = plugIn.PlugInCategory;
                    ltlDescription.Text = plugIn.Description;

                    rptList.DataSource = plugIn.Instances;
                    rptList.DataBind();

                    if (plugIn.Instances != null && plugIn.Instances.Count != 0)
                    {
                        PlugInInstanceInfo temp = null;
                        foreach (PlugInInstanceInfo instance in plugIn.Instances)
                        {
                            if (instance.InstanceId == instanceId)
                            {
                                temp = instance;
                                break;
                            }
                        }
                        if (temp == null)
                        {
                            temp = plugIn.Instances[0];
                            instanceId = temp.InstanceId;
                        }
                        config.ConfigDocument = manager.LoadConfig(temp.InstanceId);
                        ltlCurrentInstance.Text = temp.SiteName;
                        ViewState["InstanceId"] = instanceId;
                    }

                    SiteCollection sites = (new SiteManager()).GetAllSites();
                    ddlSites.Items.Clear();
                    ddlSites.Items.Add(new ListItem("所有网站", "0"));
                    foreach (SiteInfo site in sites)
                    {
                        ddlSites.Items.Add(new ListItem(site.SiteName, site.SiteId.ToString()));
                    }
                }
                else
                {
                    message.SetErrorMessage("数据读取错误：指定插件不存在！");
                }
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
                    case "start":
                        if (manager.StartPlugIns(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选插件实例已启用！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选插件实例启用失败！");
                        }
                        break;
                    case "stop":
                        if (manager.StopPlugIns(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选插件实例已停用！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选插件实例停用失败！");
                        }
                        break;
                    case "delete":
                        if (manager.DeletePlugInInstances(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选插件实例已删除！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选插件实例删除失败！");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("所执行的批量操作失败：" + ex.Message);
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            int siteId = Converter.IntParse(ddlSites.SelectedItem.Value, -1);
            if (siteId == -1)
            {
                message.SetErrorMessage("实例增加失败：参数转换错误！");
                return;
            }

            try
            {
                if ((new PlugInManager()).AddPlugInInstance(CurrentUser, PlugInId, siteId))
                {
                    message.SetSuccessMessage("实例增加成功！");
                    GetData();
                }
                else
                {
                    message.SetErrorMessage("实例增加失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("实例增加失败：" + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int instanceId = 0;
            if (ViewState["InstanceId"] != null)
            {
                instanceId = (int)ViewState["InstanceId"];
            }
            else
            {
                message.SetErrorMessage("配置信息保存失败：参数错误！");
            }

            try
            {
                string content = config.GetConfigString();
                if ((new PlugInManager()).SaveConfig(CurrentUser, instanceId, content))
                {
                    Response.Redirect(string.Format("pluginconfig.aspx?msg={0}&plugInId={1}&instanceId={2}",
                                                                            HttpUtility.UrlEncode("配置信息保存成功！"),
                                                                            PlugInId, instanceId));
                }
                else
                {
                    message.SetErrorMessage("配置信息保存失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("配置信息保存失败：" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("pluginlist.aspx");
        }
    }
}
