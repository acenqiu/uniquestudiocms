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
using System.Text.RegularExpressions;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Site;

namespace UniqueStudio.ComContent.PL
{
    public partial class Search : System.Web.UI.Page
    {
        private const string ErrorMessage = "<div class=\"error\">{0}</div>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Header.Title = "搜索 - " + SiteManager.Config(1).WebName;

                PostCollection collection = null;
                PostManager manager = new PostManager();
                try
                {
                    string key = string.Empty;
                    if (Request.QueryString["title"] != null)
                    {
                        key = HttpUtility.UrlDecode(Request.QueryString["title"]);
                        collection = manager.SearchPostsByTitle(key);
                        if (collection != null)
                        {
                            Regex r = new Regex("(?<v>" + key + ")");
                            foreach (PostInfo post in collection)
                            {
                                post.Title = r.Replace(post.Title, "<strong>${v}</strong");
                            }
                        }
                    }
                    else if (Request.QueryString["author"] != null)
                    {
                        key = HttpUtility.UrlDecode(Request.QueryString["author"]);
                        collection = manager.SearchPostsByAuthor(key);
                    }
                    else if (Request.QueryString["start"] != null && Request.QueryString["end"] != null)
                    {
                        DateTime startTime = Converter.DatetimeParse(Request.QueryString["start"], new DateTime());
                        DateTime endTime = Converter.DatetimeParse(Request.QueryString["end"], new DateTime());
                        collection = manager.SearchPostsByTime(startTime, endTime);
                    }
                    else
                    {
                        ltlMessage.Text = string.Format(ErrorMessage, "搜索条件不正确！");
                    }

                    if (collection != null)
                    {
                        rptList.DataSource = collection;
                        rptList.DataBind();
                        ltlCount.Text = collection.Count.ToString();
                    }
                }
                catch (Exception ex)
                {
                    ltlMessage.Text = string.Format(ErrorMessage, ex.Message);
                }
            }
        }
    }
}
