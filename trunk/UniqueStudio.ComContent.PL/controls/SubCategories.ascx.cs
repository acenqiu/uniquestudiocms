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

using UniqueStudio.Core.Category;
using UniqueStudio.Common.Model;

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
                if (categoryId != 0)
                {
                    CategoryManager manager = new CategoryManager();
                    CategoryInfo category = manager.GetCategory(categoryId);
                    if (category != null)
                    {
                        ltlCategoryName.Text = category.CategoryName;
                    }
                    else
                    {
                        return;
                    }

                    CategoryCollection collection = manager.GetChildCategories(categoryId);
                    if (collection != null)
                    {
                        rptList.DataSource = collection;
                        rptList.DataBind();
                    }
                }
            }
        }
    }
}