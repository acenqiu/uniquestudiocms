using System;


namespace UniqueStudio.Admin.admin
{
    public partial class sidebar : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (SiteId != 0)
                {
                    pnlSite.Visible = true;
                    pnlSystem.Visible = false;
                }
                else
                {
                    pnlSystem.Visible = true;
                    pnlSite.Visible = false;
                }
            }
        }
    }
}
