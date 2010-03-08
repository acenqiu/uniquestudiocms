using System;

using UniqueStudio.Core.Category;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;

namespace UniqueStudio.ComContent.PL.controls
{
    public partial class PostList : System.Web.UI.UserControl
    {
        private int categoryId;
        private int number = WebSiteConfig.PageSizeOfIndexPostList;
        private int maxTitleLength = 15;

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        public int Number
        {
            get { return number; }
            set { number = value; }
        }
        public int MaxTitleLength
        {
            get { return maxTitleLength; }
            set { maxTitleLength = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CategoryManager catManager = new CategoryManager();
                CategoryInfo category = catManager.GetCategory(CategoryId);
                if (category != null)
                {
                    ltlCategoryName.Text = category.CategoryName;
                    PostManager manager = new PostManager();
                    PostCollection posts = manager.GetPostListByCatId(1, number, false,
                                                            PostListType.PublishedOnly, categoryId);
                    rptList.DataSource = posts;
                    rptList.DataBind();
                }
                else
                {
                    ltlCategoryName.Text = "分类ID设置错误";
                }
            }
        }
    }
}