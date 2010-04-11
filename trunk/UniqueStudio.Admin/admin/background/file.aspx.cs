using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace UniqueStudio.Admin.admin.background
{
    public partial class file : System.Web.UI.Page
    {
        private Regex r = new Regex("^~/([^/]+/?)*$");
        private string directoryHref = "<a  href='file.aspx?path={0}'>{1}</a>";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["path"] != null)
                {
                    txtPath.Text = Request.QueryString["path"];
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
                DirectoryInfo info = new DirectoryInfo(directory);
                string href = string.Format(directoryHref, GetRelativePath(directory), info.Name);
                sbDirectories.Append("<div class='folder'>").Append(href).Append("</div>");
            }
            lblDirectories.Text = sbDirectories.ToString();

            StringBuilder sbFiles = new StringBuilder();
            string[] files = Directory.GetFiles(physicalPath);
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);
                sbFiles.Append("<div class='file'>").Append(info.Name).Append("</div>");
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
