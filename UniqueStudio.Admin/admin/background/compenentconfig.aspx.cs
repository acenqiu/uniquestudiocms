using System;
using System.Web;

using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Compenent;

namespace UniqueStudio.Admin.admin.background
{
    public partial class compenentconfig : Controls.AdminBasePage
    {
        private int compenentId;

        protected void Page_Load(object sender, EventArgs e)
        {
            compenentId = Converter.IntParse(Request.QueryString["comId"], 0);
            if (compenentId == 0)
            {
                Response.Redirect("compenentlist.aspx");
            }

            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            try
            {
                string comConfig = (new CompenentManager()).LoadConfig(CurrentUser, compenentId);
                if (comConfig == null)
                {
                    message.SetErrorMessage("数据读取失败：指定组件不存在！");
                }
                else
                {
                    config.ConfigDocument = comConfig;
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string comConfig = config.GetConfigString();

                if ((new CompenentManager()).SaveConfig(CurrentUser, compenentId, comConfig))
                {
                    Response.Redirect(string.Format("compenentconfig.aspx?msg={0}&comId={1}",
                                                                            HttpUtility.UrlEncode("组件信息保存成功！"),
                                                                            compenentId));
                }
                else
                {
                    message.SetErrorMessage("组件信息保存失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("组件信息保存失败：" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("compenentlist.aspx");
        }
    }
}
