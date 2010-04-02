using System;
using System.Text;
using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;
using UniqueStudio.Core.Site;

namespace UniqueStudio.CGE
{
    public partial class list : Controls.PlBasePage
    {
        protected int CategoryId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CategoryId = Converter.IntParse(Request.QueryString["catId"], 0);
                if (CategoryId == 0)
                {
                    Response.Redirect(PathHelper.PathCombine(SiteManager.BaseAddress(SiteId), "404.aspx"));
                }

                int pageIndex = Converter.IntParse(Request.QueryString["page"], 1);
                int pageSize = Converter.IntParse(Request.QueryString["size"], SiteManager.Config(SiteId).PageSizeOfSectionPostList);

                PostManager postManager = new PostManager();
                PostCollection posts = postManager.GetPostListByCatId(pageIndex, pageSize, false, PostListType.PublishedOnly, CategoryId);
                if (posts != null)
                {
                    if (posts.Count == 1)
                    {
                        //如果只有一篇文章，默认直接显示该文章
                        Response.Redirect(string.Format("view.aspx?catId={0}&uri={1}", CategoryId, posts[0].Uri));
                    }
                    else
                    {
                        rptList.DataSource = posts;
                        rptList.DataBind();

                        pagination.CurrentPage = posts.PageIndex;
                        pagination.Count = posts.PageCount;
                    }
                }

                CategoryManager manager = new CategoryManager();
                CategoryInfo category = manager.GetCategoryPath(CategoryId);
                if (category == null)
                {
                    Response.Redirect(PathHelper.PathCombine(SiteManager.BaseAddress(SiteId), "404.aspx"));
                }
                else
                {
                    //侧边栏
                    categories.CategoryId = category.CategoryId;

                    //路径和标题
                    StringBuilder sb = new StringBuilder();
                    while (category != null)
                    {
                        sb.Append(string.Format("-><a href='list.aspx?catId={0}'>{1}</a>  ", category.CategoryId, category.CategoryName));
                        if (category.ChildCategories != null && category.ChildCategories.Count > 0)
                        {
                            category = category.ChildCategories[0];
                        }
                        else
                        {
                            Page.Header.Title = category.CategoryName + " - " + SiteManager.Config(SiteId).WebName;
                            break;
                        }
                    }
                    ltlCategoryLink.Text = sb.ToString();
                }
            }
        }
    }
}
