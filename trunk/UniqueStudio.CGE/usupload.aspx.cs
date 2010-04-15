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

using UniqueStudio.Common.Config;

namespace UniqueStudio.CGE
{
    public partial class usupload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string filepath = Server.MapPath(@"~" + FileUpload1.FileName);
                try
                {
                    FileUpload1.SaveAs(filepath);
                    Label1.Text = "上传成功";
                }
                catch
                {
                    Response.Write("<script type='text/javascript'>alert('文件上传失败，请重新上传')</script>");
                }
            }
            else
            {
                Label1.Text = "请指定文件";
            }
        }
    }
}
