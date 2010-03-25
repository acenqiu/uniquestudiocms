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
using System.Xml;

using UniqueStudio.Common.PageParser;

namespace UniqueStudio.Admin.admin.controls
{
    public partial class Config : System.Web.UI.UserControl
    {
        public string ConfigDocument
        {
            get
            {
                return (string)ViewState["ConfigDocument"];
            }
            set
            {
                ViewState["ConfigDocument"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ConfigDocument != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(ConfigDocument);
                    XmlPageParser parser = new XmlPageParser();
                    ltlHtmlContent.Text = parser.ProcessXML(doc);
                }
            }
        }

        public string GetConfigString()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(ConfigDocument);
          
            foreach (string key in Request.Form.Keys)
            {
                XmlNode node = doc.SelectSingleNode(string.Format("//param[@name=\"{0}\"]",key));
                if (node != null)
                {
                    node.Attributes["value"].Value = Request.Form[key];
                }
            }

            return doc.OuterXml;
        }
    }
}