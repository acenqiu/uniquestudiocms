﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Utilities;
using UniqueStudio.ComContent.BLL;

namespace UniqueStudio.ComContent.PL
{
    public partial class editpost : Controls.AdminBasePage
    {
        protected string PostListQuery;

        protected void Page_Load(object sender, EventArgs e)
        {
            editor.SiteId = SiteId;
            PostListQuery = PathHelper.CleanUrlQueryString(HttpUtility.UrlDecode(Request.QueryString["ret"]),
                                                                                            new string[] { "msg", "msgtype" });
            if (!IsPostBack)
            {
                if (Request.QueryString["uri"] != null)
                {
                    if (!PostPermissionManager.HasEditPermission(CurrentUser, Convert.ToInt64(Request.QueryString["uri"])))
                    {
                        Response.Redirect("PostPermissionError.aspx?Error=编辑文章&Page=" + Request.UrlReferrer.ToString());
                    }
                    long uri = 0;
                    if (long.TryParse(Request.QueryString["uri"], out uri))
                    {
                        editor.Uri = uri;
                    }
                    else
                    {
                        //Response.Redirect("~/admin/postList.aspx?" + ret + "&retMsg=" + HttpUtility.UrlEncode("参数异常"));
                    }
                }
                else
                {
                    //Response.Redirect("~/admin/postList.aspx?" + ret + "&retMsg=" + HttpUtility.UrlEncode("参数异常"));
                }
            }
        }
    }
}
