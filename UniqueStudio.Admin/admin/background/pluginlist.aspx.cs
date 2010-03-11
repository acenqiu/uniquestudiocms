﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using UniqueStudio.Core.PlugIn;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;

namespace UniqueStudio.Admin.admin.background
{
    public partial class pluginlist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetData();
            }
        }

        private void GetData()
        {
            UserInfo currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            PlugInManager manager = new PlugInManager();
            PlugInCollection collection = manager.GetAllPlugIns(currentUser);
            rptList.DataSource = collection;
            rptList.DataBind();
        }
    }
}
