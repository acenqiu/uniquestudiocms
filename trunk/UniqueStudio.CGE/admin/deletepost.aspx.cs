using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.ComContent.Admin
{
    public partial class deletepost : Controls.AdminBasePage
    {
        private PostManager bll = new PostManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["uriCollection"] != null)
                {
                    string[] uris = Request.QueryString["uriCollection"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string item in uris)
                    {
                        if (!PostPermissionManager.HasDeletePermission(CurrentUser, Convert.ToInt64(item)))
                        {
                            Response.Redirect("PostPermissionError.aspx?Error=删除文章&Page=" + Request.UrlReferrer.ToString());
                        }
                    }
                }
                else
                {
                    Return();
                }
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string[] uriCollection = Request.QueryString["uriCollection"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            List<string> errorCollection = new List<string>();
            long uri;
            foreach (string uriString in uriCollection)
            {
                if (long.TryParse(uriString, out uri))
                {
                    if (!bll.DeletePost(CurrentUser, uri))
                    {
                        errorCollection.Add(uriString);
                    }
                }
                else
                {
                    errorCollection.Add(uriString);
                }
            }
            if (errorCollection.Count == 0)
            {
                Return();
            }
            else
            {
                pnlError.Visible = true;
                StringBuilder sb = new StringBuilder();
                foreach (string errorUri in errorCollection)
                {
                    sb.AppendFormat("{0}<br />", errorUri);
                }
                lblErrorList.Text = sb.ToString();
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Return();
        }

        private void Return()
        {
            if (Request.QueryString["ret"] != null)
            {
                string query = PathHelper.CleanUrlQueryString(HttpUtility.UrlDecode(Request.QueryString["ret"]),
                                                                                                new string[] { "msg", "msgtype" });
                Response.Redirect("postlist.aspx?" + query);
            }
            else
            {
                Response.Redirect("postlist.aspx?siteId=" + SiteId);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Return();
        }
    }
}
