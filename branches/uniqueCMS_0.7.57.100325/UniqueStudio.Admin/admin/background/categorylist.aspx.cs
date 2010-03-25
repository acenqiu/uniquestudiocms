//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：创建分类及分类列表页面。
// 完成日期：2010年03月19日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;

namespace UniqueStudio.Admin.admin.background
{
    public partial class categorylist : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            try
            {
                CategoryCollection collection = (new CategoryManager()).GetAllCategories(SiteId);
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
                    message.SetErrorMessage("数据读取失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            CategoryInfo category = new CategoryInfo();
            category.SiteId = SiteId;
            category.CategoryName = txtCategoryName.Text.Trim();
            category.CategoryNiceName = txtNiceName.Text.Trim();
            category.Description = txtDescription.Text.Trim();
            category.ParentCategoryId = Converter.IntParse(ddlCategories.SelectedValue, 0);

            try
            {
                if ((new CategoryManager()).CreateCategory(CurrentUser, category) != null)
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
            catch (Exception ex)
            {
                message.SetErrorMessage("分类创建失败：" + ex.Message);
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
                    list.Add(Converter.IntParse(ids[i], 0));
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
                    if (manager.DeleteCategories(CurrentUser, list.ToArray(), false))
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
                    message.SetErrorMessage("所选分类删除失败：" + ex.Message);
                }
                GetData();
            }
        }
    }
}
