using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;
using System.Text.RegularExpressions;
using System.Text;

namespace UniqueStudio.CGE
{
    public partial class stuff : System.Web.UI.Page
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
        private string requestRegex=@"name=[^&]*";
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] bytes =  Encoding.GetEncoding("ascii").GetBytes(Request.QueryString["name"]);
            string rawUrl = Request.RawUrl;
            StuffName = Request.QueryString["name"];
            Match match = Regex.Match(rawUrl, requestRegex);
            if (match.Length>0)
            {
                StuffName = HttpUtility.UrlDecode(match.Value.Replace("name=", ""), Encoding.GetEncoding("gb2312"));
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
