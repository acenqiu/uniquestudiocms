using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

using UniqueStudio.Core.Category;
using UniqueStudio.Common;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class categorylist : System.Web.UI.Page
    {
        CategoryManager manager = new CategoryManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            CategoryCollection collection = manager.GetAllCategories();
            if (collection != null)
            {
                ddlCategories.Items.Clear();
                ddlCategories.Items.Add(new ListItem("无", "0"));
                foreach (CategoryInfo category in collection)
                {
                    ddlCategories.Items.Add(new ListItem(category.CategoryName, category.CategoryId.ToString()));
                }

                rptList.DataSource = collection;
                rptList.DataBind();
            }
            else
            {
                message.SetErrorMessage("分类列表读取失败，可能是数据库连接出现异常，请查看系统日志。");
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CategoryInfo category = new CategoryInfo();
            category.CategoryName = txtCategoryName.Text.Trim();
            category.CategoryNiceName = txtNiceName.Text.Trim();
            category.Description = txtDescription.Text.Trim();
            category.SubOf = Convert.ToInt32(ddlCategories.SelectedValue);

            UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];

            try
            {
                category = manager.CreateCategory(currentUser, category);
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
                return;
            }

            if (category != null)
            {
                message.SetSuccessMessage("分类创建成功！");
                GetData();
                txtCategoryName.Text = "";
                txtNiceName.Text = "";
                txtDescription.Text = "";
            }
            else
            {
                message.SetErrorMessage("分类创建失败！");
            }
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            CategoryManager manager = new CategoryManager();
            List<int> list = new List<int>();
            if (Request.Form["chkSelected"] != null)
            {
                string[] ids = Request.Form["chkSelected"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ids.Length; i++)
                {
                    list.Add(Utility.IntParse(ids[i], 0));
                }
            }
            else
            {
                return;
            }

            if (ddlOperation.SelectedValue == "delete")
            {
                try
                {
                    if (manager.DeleteCategories((UserInfo)this.Session[GlobalConfig.SESSION_USER], list.ToArray()))
                    {
                        message.SetSuccessMessage("所选分类已删除！");
                    }
                    else
                    {
                        message.SetErrorMessage("所选分类删除失败！");
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage(ex.Message);
                }
                GetData();
            }
        }
    }
}
