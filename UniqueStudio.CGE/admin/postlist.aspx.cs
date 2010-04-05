using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;

namespace UniqueStudio.ComContent.Admin
{
    public partial class postlist : Controls.AdminBasePage
    {
        private PostListType postListType = PostListType.Both;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CategoryCollection categories = (new CategoryManager()).GetAllCategories(SiteId);
                    ddlCategories.Items.Clear();
                    ddlCategories.Items.Add(new ListItem("所有分类", "0"));
                    if (categories != null)
                    {
                        foreach (CategoryInfo category in categories)
                        {
                            ddlCategories.Items.Add(new ListItem(category.CategoryName, category.CategoryId.ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("分类信息获取失败：" + ex.Message);
                }

                GetData();
            }
        }

        private void GetData()
        {
            int pageIndex = Converter.IntParse(Request.QueryString["page"], 1);
            int pageSize = Converter.IntParse(Request.QueryString["pageSize"], 15);

            try
            {
                PostCollection collection = (new PostManager()).GetPostList(CurrentUser, SiteId, pageIndex, pageSize, false, postListType, true);
                if (collection == null)
                {
                    rptList.DataSource = null;
                }
                else
                {
                    if (collection.Count == 0)
                    {
                        message.SetSuccessMessage("当前没有文章");
                    }
                    else
                    {
                        rptList.DataSource = collection;
                        rptList.DataBind();

                        pagination.Count = collection.PageCount;
                        pagination.CurrentPage = collection.PageIndex;
                        pagination.Url = "postlist.aspx?siteId=" + SiteId + "&page={0}";
                    }
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("文章列表读取失败： " + ex.Message);
            }
        }

        protected void txtSearch_Click(object sender, EventArgs e)
        {

        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            PostManager manager = new PostManager();
            List<long> list = new List<long>();
            if (Request.Form["chkSelected"] != null)
            {
                string[] ids = Request.Form["chkSelected"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ids.Length; i++)
                {
                    list.Add(Converter.LongParse(ids[i], 0));
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
                        if (manager.DeletePosts(CurrentUser, list.ToArray()))
                        {
                            message.SetSuccessMessage("所选文章已删除！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选文章删除失败！");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("所执行的批量操作失败：" + ex.Message);
            }
        }
    }
}
