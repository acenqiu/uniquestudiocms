using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UniqueStudio.Core.User;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class userinfo : System.Web.UI.Page
    {
        UserInfo user = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            user = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            if (!IsPostBack)
            {
                user = (new UserManager()).GetUserInfo(user, user.UserId);
                if (user.ExInfo == null)
                {
                    message.SetTipMessage("您还没有设置您的个人信息。");
                }
                else
                {
                    txtPenName.Text = user.ExInfo.PenName;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            user.ExInfo = new UserExInfo();
            user.ExInfo.PenName = txtPenName.Text.Trim();

            try
            {
                if ((new UserManager()).UpdateUserExInfo(user))
                {
                    message.SetSuccessMessage("个人信息修改成功！");
                }
                else
                {
                    message.SetErrorMessage("个人信息修改失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
