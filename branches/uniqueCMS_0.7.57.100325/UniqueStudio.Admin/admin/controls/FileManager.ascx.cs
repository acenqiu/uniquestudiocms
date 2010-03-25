using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace UniqueStudio.Admin.admin.controls
{
    public partial class FileManager : System.Web.UI.UserControl
    {
        private Regex r = new Regex("^~/([^/]+/?)*$");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["path"] != null)
                {
                    RefreshView(Request.QueryString["path"]);
                }
                else
                {
                    RefreshView("/");
                }
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            RefreshView(txtPath.Text);
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            DirectoryInfo currentDirectory = new DirectoryInfo(Server.MapPath(txtPath.Text));
            txtPath.Text = GetRelativePath(currentDirectory.Parent.FullName);
            RefreshView(txtPath.Text);
        }

        private void RefreshView(string path)
        {
            //if (!r.IsMatch(path))
            //{
            //    return;
            //}

            string physicalPath = Server.MapPath(path);
            if (!Directory.Exists(physicalPath))
            {
                return;
            }

            StringBuilder sbDirectories = new StringBuilder();
            string[] directories = Directory.GetDirectories(physicalPath);
            foreach (string directory in directories)
            {
                sbDirectories.Append(GetRelativePath(directory)).Append("<br/>");
            }
            lblDirectories.Text = sbDirectories.ToString();

            StringBuilder sbFiles = new StringBuilder();
            string[] files = Directory.GetFiles(physicalPath);
            foreach (string file in files)
            {
                sbFiles.Append(GetRelativePath(file)).Append("<br/>");
            }
            lblFiles.Text = sbFiles.ToString();
        }

        private string GetRelativePath(string physicalPath)
        {
            string rootPath = Server.MapPath("~");
            int length = rootPath.Length;
            if (physicalPath.IndexOf(rootPath) == 0)
            {
                return physicalPath.Substring(length - 1);
            }
            else
            {
                return "\\";
            }
        }
    }
}