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

using UniqueStudio.Core.Menu;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class menulist : System.Web.UI.Page
    {
        private MenuManager manager = new MenuManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            rptList.DataSource = manager.GetAllMenus();
            rptList.DataBind();
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            MenuInfo menu = new MenuInfo();
            menu.MenuName = txtMenuName.Text.Trim();
            menu.Description = txtDescription.Text.Trim();

            try
            {
                menu = manager.CreateMenu((UserInfo)this.Session[GlobalConfig.SESSION_USER], menu);
                if (menu != null)
                {
                    message.SetSuccessMessage("菜单创建成功！");
                    GetData();
                }
                else
                {
                    message.SetErrorMessage("菜单创建失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            //CategoryManager manager = new CategoryManager();
            //List<int> list = new List<int>();
            //if (Request.Form["chkSelected"] != null)
            //{
            //    string[] ids = Request.Form["chkSelected"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //    for (int i = 0; i < ids.Length; i++)
            //    {
            //        list.Add(Utility.IntParse(ids[i], 0));
            //    }
            //}
            //else
            //{
            //    return;
            //}

            //if (ddlOperation.SelectedValue == "delete")
            //{
            //    try
            //    {
            //        if (manager.DeleteCategories((UserInfo)this.Session[GlobalConfig.SESSION_USER], list.ToArray()))
            //        {
            //            message.SetSuccessMessage("所选分类已删除！");
            //        }
            //        else
            //        {
            //            message.SetErrorMessage("所选分类删除失败！");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        message.SetErrorMessage(ex.Message);
            //    }
            //    GetData();
            //}
        }
    }
}
