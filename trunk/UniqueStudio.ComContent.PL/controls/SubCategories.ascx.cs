using System;
using System.Text;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;

namespace UniqueStudio.ComContent.PL.controls
{
    public partial class SubCategories : System.Web.UI.UserControl
    {
        private int categoryId;

        public int CategoryId
        {
            set { categoryId = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CategoryInfo root = (new CategoryManager()).GetCategoryChain(categoryId);
                if (root != null)
                {
                    ltlCategoryName.Text = root.CategoryName;
                    ltlList.Text = GetCategoryHtml(root);
                }
            }
        }

        private string GetCategoryHtml(CategoryInfo root)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul>").Append("\r\n");
            foreach (CategoryInfo item in root.ChildCategories)
            {
                sb.Append(GetHtml(item)).Append("\r\n");
            }
            sb.Append("</ul>").Append("\r\n");
            return sb.ToString();
        }

        private string GetHtml(CategoryInfo node)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<li>\r\n<a href='list.aspx?catId={0}' >{1}</a>\r\n", node.CategoryId, node.CategoryName));

            if (node.ChildCategories != null && node.ChildCategories.Count != 0)
            {
                sb.Append("<ul>\r\n");
                foreach (CategoryInfo child in node.ChildCategories)
                {
                    sb.Append(GetHtml(child)).Append("\r\n");
                }
                sb.Append("</ul>\r\n");
            }

            sb.Append("</li>\r\n");
            return sb.ToString();
        }
    }
}