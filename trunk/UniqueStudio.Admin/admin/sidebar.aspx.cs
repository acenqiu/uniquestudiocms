using System;
using System.Text;
using System.Xml;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.ErrorLogging;


namespace UniqueStudio.Admin.admin
{
    public partial class sidebar : Controls.AdminBasePage
    {
        //{0}：text
        //{1}：items
        private const string TAB_WITH_SUBTABS = "<li onclick=\"javascript:changestate(this)\"><span>{0}</span><span class=\"collapse-icon\"></span>\r\n"
                                                                            + "<div class=\"candy-menu\">\r\n<ul>\r\n{1}</ul>\r\n</div></li>\r\n";
        private const string TAB = "<li><a href='{0}' target=\"{1}\">{2}</a></li>\r\n";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTabs();
            }
        }

        protected void LoadTabs()
        {
            string path = string.Empty;
            if (SiteId != 0)
            {
                path = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, string.Format(@"admin\xml\NavigationOfSite{0}.xml", SiteId));
                pnlGoBackToWebSite.Visible = true;
            }
            else
            {
                path = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, @"admin\xml\NavigationOfSystem.xml");
                pnlGoBackToWebSite.Visible = false;
            }

            StringBuilder output = new StringBuilder();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNodeList nodes = doc.SelectNodes("/TabCollection/Compenents/Tab");
                output.Append(TranslateTabs(nodes));

                nodes = doc.SelectNodes("/TabCollection/System/Tab");
                output.Append(TranslateTabs(nodes));

                ltlNavigation.Text = output.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                ltlNavigation.Text = "导航信息读取失败，可能是文件不存在，或者格式不正确。";
            }
        }

        protected string TranslateTabs(XmlNodeList nodes)
        {
            if (nodes == null)
            {
                return string.Empty;
            }

            StringBuilder tabs = new StringBuilder();
            foreach (XmlNode node in nodes)
            {
                tabs.Append(TranslateTab(node));
            }
            return tabs.ToString();
        }

        protected string TranslateTab(XmlNode node)
        {
            if (node == null)
            {
                return string.Empty;
            }

            string text = node.Attributes["text"] == null ? string.Empty : node.Attributes["text"].Value;
            string permissions = node.Attributes["permissions"] == null ? string.Empty : node.Attributes["permissions"].Value;
            string href = node.Attributes["href"] == null ? string.Empty : node.Attributes["href"].Value;
            string target = node.Attributes["target"] == null ? string.Empty : node.Attributes["target"].Value;

            //权限检测

            if (node.HasChildNodes)
            {
                string subTabs = TranslateTabs(node.FirstChild.ChildNodes);
                return string.Format(TAB_WITH_SUBTABS, text, subTabs);
            }
            else
            {
                if (SiteId != 0)
                {
                    href += "?siteId=" + SiteId;
                }
                return string.Format(TAB, href, target, text);
            }
        }
    }
}
