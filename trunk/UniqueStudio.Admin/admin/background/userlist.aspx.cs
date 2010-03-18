﻿using System;
using System.Collections.Generic;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.User;

namespace UniqueStudio.Admin.admin.background
{
    public partial class userlist : Controls.BasePage
    {
        private int pageIndex;
        private int pageSize;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            pageIndex = Converter.IntParse(Request.QueryString["page"], 1);
            pageSize = Converter.IntParse(Request.QueryString["number"], 10);

            try
            {
                UserManager manager = new UserManager(CurrentUser);
                UserCollection users = manager.GetUserList(pageIndex, pageSize);
                rptList.DataSource = users;
                rptList.DataBind();

                pagination.Count = users.PageCount;
                pagination.CurrentPage = users.PageIndex;
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            UserManager manager = new UserManager(CurrentUser);
            List<Guid> list = new List<Guid>();
            if (Request.Form["chkSelected"] != null)
            {
                string[] ids = Request.Form["chkSelected"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    for (int i = 0; i < ids.Length; i++)
                    {
                        list.Add(new Guid(ids[i]));
                    }
                }
                catch (FormatException ex)
                {
                    message.SetErrorMessage("用户ID格式错误");
                    return;
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
                    case "delete":
                        if (manager.DeleteUsers(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选用户已删除！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选用户删除失败，请重试！");
                        }
                        break;
                    case "approve":
                        if (manager.ApproveUsers(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选用户已激活！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选用户激活失败，请重试！");
                        }
                        break;
                    case "lock":
                        if (manager.LockUsers(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选用户已锁定！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选用户锁定失败，请重试！");
                        }
                        break;
                    case "unlock":
                        if (manager.UnLockUsers(list.ToArray()))
                        {
                            message.SetSuccessMessage("所选用户已解锁！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选用户解锁失败，请重试！");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
