using System;
using System.Text;
using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;
using UniqueStudio.Core.Site;

namespace UniqueStudio.ComContent.PL
{
    public partial class list : Controls.PlBasePage
    {
        protected int CategoryId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CategoryId = Converter.IntParse(Request.QueryString["catId"], 1);
                int pageIndex = Converter.IntParse(Request.QueryString["page"], 1);
                int pageSize = Converter.IntParse(Request.QueryString["size"], SiteManager.Config(SiteId).PageSizeOfSectionPostList);


                CategoryManager manager = new CategoryManager();
                CategoryInfo category = manager.GetCategoryPath(CategoryId);
                if (category != null)
                {
                    //设置网页标题
                    Page.Header.Title = category.CategoryName + " - " + SiteManager.Config(SiteId).WebName;

                    categories.CategoryId = category.CategoryId;

                    StringBuilder sb = new StringBuilder();
                    while (category != null)
                    {
                        sb.Append(string.Format("-><a href='list.aspx?catId={0}'>{1}</a>  ", category.CategoryId, category.CategoryName));
                        category = category.ChildCategory;
                    }
                    ltlCategoryLink.Text = sb.ToString();
                }
                else
                {
                    Response.Redirect(PathHelper.PathCombine(SiteManager.BaseAddress(SiteId), "404.aspx"));
                }

                PostManager postManager = new PostManager();
                PostCollection posts = postManager.GetPostListByCatId(pageIndex, pageSize, false, PostListType.PublishedOnly, CategoryId);
                if (posts != null)
                {
                    rptList.DataSource = posts;
                    rptList.DataBind();

                    pagination.CurrentPage = posts.PageIndex;
                    pagination.Count = posts.PageCount;
                }
            }
        }
    }
}
