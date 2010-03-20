//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：编辑分类页面。
// 完成日期：2010年03月19日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Web;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;

namespace UniqueStudio.Admin.admin.background
{
    public partial class editcategory : Controls.BasePage
    {
        private int categoryId;

        protected void Page_Load(object sender, EventArgs e)
        {
            categoryId = Converter.IntParse(Request.QueryString["catId"], 0);
            if (categoryId == 0)
            {
                Response.Redirect("categorylist.aspx?siteId=" + SiteId);
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
                CategoryManager manager = new CategoryManager();
                CategoryInfo category = manager.GetCategory(categoryId);
                if (category == null)
                {
                    message.SetErrorMessage("数据获取失败：该分类不存在！");
                    return;
                }
                txtCategoryName.Text = category.CategoryName;
                txtNiceName.Text = category.CategoryNiceName;
                txtDescription.Text = category.Description;

                CategoryCollection collection = manager.GetAllCategories(category.SiteId);
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
                    message.SetErrorMessage("数据读取失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            CategoryInfo category = new CategoryInfo();
            category.CategoryId = categoryId;
            category.CategoryName = txtCategoryName.Text.Trim();
            category.CategoryNiceName = txtNiceName.Text.Trim();
            category.Description = txtDescription.Text.Trim();
            category.ParentCategoryId = Converter.IntParse(ddlCategories.SelectedValue, -1);
            if (category.ParentCategoryId == -1)
            {
                message.SetErrorMessage("分类更新失败：父分类ID转换失败！");
                return;
            }

            try
            {
                if ((new CategoryManager()).UpdateCategory(CurrentUser, category))
                {
                    Response.Redirect("categorylist.aspx?msg=" + HttpUtility.UrlEncode("分类更新成功！") + "&siteId=" + SiteId);
                }
                else
                {
                    message.SetErrorMessage("分类更新失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("分类更新失败：" + ex.Message);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("categorylist.aspx?siteId=" + SiteId);
        }
    }
}
