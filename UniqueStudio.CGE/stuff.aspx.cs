using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;
using UniqueStudio.Core.Site;

namespace UniqueStudio.CGE
{
    public partial class stuff : Controls.PlBasePage
    {
        public string StuffName
        {
            get;
            set;
        }
        public int CatId
        {
            get;
            set;
        }

        private string requestRegex = @"name=[^&]*";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["name"] == null)
            {
                Response.Redirect("404.aspx");
            }

            string rawUrl = Request.RawUrl;
            Match match = Regex.Match(rawUrl, requestRegex);
            if (match.Length > 0)
            {
                StuffName = HttpUtility.UrlDecode(match.Value.Replace("name=", ""), Encoding.GetEncoding("gb2312"));
                this.Header.Title = StuffName + " - " + SiteManager.Config(SiteId).WebName;
            }

            if (!IsPostBack)
            {
                int categoryId = Converter.IntParse(sitePath.Text, 0);
                subCategories.ID = categoryId.ToString();
                //设置侧边栏及分类路径
                StringBuilder sb = new StringBuilder();
                CategoryManager manager = new CategoryManager();
                CategoryInfo category = manager.GetCategoryPath(categoryId);
                if (category != null)
                {
                    //设置侧边栏
                    subCategories.CategoryId = category.CategoryId;
                }
                while (category != null)
                {
                    sb.Append(string.Format("-><a href='list.aspx?catId={0}'>{1}</a>  ", category.CategoryId, category.CategoryName));
                    if (category.ChildCategories != null && category.ChildCategories.Count > 0)
                    {
                        category = category.ChildCategories[0];
                    }
                    else
                    {
                        break;
                    }
                }
                sb.Append(string.Format("-><a href='stuff.aspx?name={0}'>{0}</a>  ", StuffName));
                sitePath.Text = sb.ToString();
            }
        }
    }
}
