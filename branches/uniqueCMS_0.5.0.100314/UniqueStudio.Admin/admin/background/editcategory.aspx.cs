using System;
using System.Web;
using System.Web.UI.WebControls;

using UniqueStudio.Core.Category;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class editcategory : System.Web.UI.Page
    {
        private CategoryManager manager = new CategoryManager();
        private int categoryId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["catId"] != null)
            {
                if (!int.TryParse(Request.QueryString["catId"], out categoryId))
                {
                    Response.Redirect("categorylist.aspx");
                }
            }
            else
            {
                Response.Redirect("categorylist.aspx");
            }

            if (!IsPostBack)
            {
                CategoryInfo category = manager.GetCategory(categoryId);
                if (category == null)
                {
                    Response.Redirect("categorylist.aspx");
                }
                else
                {
                    txtCategoryName.Text = category.CategoryName;
                    txtNiceName.Text = category.CategoryNiceName;
                    txtDescription.Text = category.Description;
                }

                CategoryCollection collection = manager.GetAllCategories();
                if (collection != null)
                {
                    ddlCategories.Items.Clear();
                    ddlCategories.Items.Add(new ListItem("无", "0"));
                    foreach (CategoryInfo item in collection)
                    {
                        ddlCategories.Items.Add(new ListItem(item.CategoryName, item.CategoryId.ToString()));
                    }
                    ddlCategories.SelectedValue = category.ParentCategoryId.ToString();
                }
                else
                {
                    message.SetErrorMessage("分类列表读取失败，可能是数据库连接出现异常，请查看系统日志。");
                }
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            CategoryInfo category = new CategoryInfo();
            category.CategoryId = categoryId;
            category.CategoryName = txtCategoryName.Text.Trim();
            category.CategoryNiceName = txtNiceName.Text.Trim();
            category.Description = txtDescription.Text.Trim();
            category.ParentCategoryId = Convert.ToInt32(ddlCategories.SelectedValue);

            try
            {
                if (manager.UpdateCategory((UserInfo)this.Session[GlobalConfig.SESSION_USER], category))
                {
                    Response.Redirect("categorylist.aspx?msg=" + HttpUtility.UrlEncode("分类更新成功"));
                }
                else
                {
                    message.SetErrorMessage("分类更新失败，请重试！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("categorylist.aspx");
        }
    }
}
