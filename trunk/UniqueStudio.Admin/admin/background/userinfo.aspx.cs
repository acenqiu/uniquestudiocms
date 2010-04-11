//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：用户信息配置页面。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Core.User;

namespace UniqueStudio.Admin.admin.background
{
    public partial class userinfo : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CurrentUser = (new UserManager()).GetUserInfo(CurrentUser, CurrentUser.UserId);
                    if (CurrentUser.ExInfo == null)
                    {
                        message.SetTipMessage("您还没有设置您的个人信息。");
                    }
                    else
                    {
                        txtPenName.Text = CurrentUser.ExInfo.PenName;
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("数据获取失败：" + ex.Message);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CurrentUser.ExInfo = new UserExInfo();
            CurrentUser.ExInfo.PenName = txtPenName.Text.Trim();

            try
            {
                if ((new UserManager()).UpdateUserExInfo(CurrentUser))
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
                message.SetErrorMessage("个人信息修改失败：" + ex.Message);
            }
        }
    }
}
