using System;
using System.Web;

namespace UniqueStudio.ComContent.Admin
{
    public partial class Message : System.Web.UI.UserControl
    {
        private const string ErrorStringFormat = "<div class=\"error\">{0}</div>";
        private const string SuccessStringFormat = "<div class=\"success\">{0}</div>";
        private const string TipStringFormat = "<div class=\"tip\">{0}</div>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["msg"] != null)
                {
                    if (Request.QueryString["msgtype"] != null)
                    {
                        if (Request.QueryString["msgtype"] == "e")
                        {
                            SetErrorMessage(HttpUtility.UrlDecode(Request.QueryString["msg"]));
                            return;
                        }
                    }
                    SetSuccessMessage(HttpUtility.UrlDecode(Request.QueryString["msg"]));
                }
            }
            else
            {
                ltlMessage.Text = "";
            }
        }

        public void SetErrorMessage(string message)
        {
            ltlMessage.Text = string.Format(ErrorStringFormat, message);
        }

        public void SetSuccessMessage(string message)
        {
            ltlMessage.Text = string.Format(SuccessStringFormat, message);
        }

        public void SetTipMessage(string message)
        {
            ltlMessage.Text = string.Format(TipStringFormat, message);
        }
    }
}