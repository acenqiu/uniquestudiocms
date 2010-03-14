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

using UniqueStudio.Common.Mailing;

namespace UniqueStudio.Admin.admin.background
{
    public partial class feedback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            SMTP smtp = new SMTP();
            try
            {
                smtp.Send("smtp.qq.com", 465, true, "imacen", "keepahead",
                                            "support@uniquestudio.org", "Feedback",
                                            fckContent.Value + "<p>"+txtEmail.Text+"</p>");
                message.SetSuccessMessage("反馈信息已经发送，感谢您的配合，我们将及时回复您，谢谢！");
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(ex.Message);
            }
        }
    }
}
