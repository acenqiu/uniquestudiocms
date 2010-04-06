using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Text;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;

namespace UniqueStudio.ComContent.Admin
{
    public partial class postlist : Controls.AdminBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CategoryCollection categories = (new CategoryManager()).GetAllCategories(SiteId);
                    ddlCategories.Items.Clear();
                    ddlCategories.Items.Add(new ListItem("所有分类", "0"));
                    if (categories != null)
                    {
                        foreach (CategoryInfo category in categories)
                        {
                            ddlCategories.Items.Add(new ListItem(category.CategoryName, category.CategoryId.ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("分类信息获取失败：" + ex.Message);
                }

                GetData();
            }
        }

        private void GetData()
        {
            int pageIndex = Converter.IntParse(Request.QueryString["page"], 1);
            int pageSize = Converter.IntParse(Request.QueryString["pageSize"], 15);
            DateTime cFrom = Converter.DatetimeParse(Request["cFrom"], DateTime.MinValue);
            if (cFrom != DateTime.MinValue)
            {
                txtCreateFrom.Text = cFrom.ToString("yyyy-MM-dd");
            }
            DateTime cTo = Converter.DatetimeParse(Request["cTo"], DateTime.MinValue);
            if (cTo != DateTime.MinValue)
            {
                txtCreateTo.Text = cTo.ToString("yyyy-MM-dd");
            }
            DateTime eFrom = Converter.DatetimeParse(Request["eFrom"], DateTime.MinValue);
            if (eFrom != DateTime.MinValue)
            {
                txtEditFrom.Text = eFrom.ToString("yyyy-MM-dd");
            }
            DateTime eTo = Converter.DatetimeParse(Request["eTo"], DateTime.MinValue);
            if (eTo != DateTime.MinValue)
            {
                txtEditTo.Text = eTo.ToString("yyyy-MM-dd");
            }
            int categoryId = Converter.IntParse(Request["catId"], 0);
            ddlCategories.SelectedValue = categoryId.ToString();
            PostListType postListType = PostListType.Both;
            try
            {
                if (Request.QueryString["type"] != null)
                {
                    postListType = (PostListType)Enum.Parse(typeof(PostListType), Request["type"], true);
                    ddlPostType.SelectedValue = Request["type"];
                }
            }
            catch
            {
                //do nothing
            }
            txtKeyWord.Text = Request.QueryString["k"];

            try
            {
                PostManager manager = new PostManager();
                PostCollection collection = manager.GetPostList(CurrentUser, SiteId, pageIndex, pageSize, cFrom, cTo
                                                                                            , eFrom, eTo, categoryId, postListType
                                                                                            , Request["k"]);
                if (collection == null)
                {
                    rptList.DataSource = null;
                }
                else
                {
                    if (collection.Count == 0)
                    {
                        message.SetSuccessMessage("没有找到符合要求的文章！");
                    }
                    else
                    {
                        rptList.DataSource = collection;
                        rptList.DataBind();

                        pagination.Count = collection.PageCount;
                        pagination.CurrentPage = collection.PageIndex;
                        string query = PathHelper.CleanUrlQueryString(Request.Url.Query, new string[] { "msg", "msgtype", "page" });
                        pagination.Url = "postlist.aspx?" + query + "&page={0}";
                    }
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("文章列表读取失败： " + ex.Message);
            }
        }

        protected void txtSearch_Click(object sender, EventArgs e)
        {
            StringBuilder query = new StringBuilder();
            query.Append("siteId=" + SiteId);
            if (!string.IsNullOrEmpty(txtCreateFrom.Text))
            {
                query.Append("&cFrom=" + txtCreateFrom.Text);
            }
            if (!string.IsNullOrEmpty(txtCreateTo.Text))
            {
                query.Append("&cTo=" + txtCreateTo.Text);
            }
            if (!string.IsNullOrEmpty(txtEditFrom.Text))
            {
                query.Append("&eFrom=" + txtEditFrom.Text);
            }
            if (!string.IsNullOrEmpty(txtEditTo.Text))
            {
                query.Append("&eTo=" + txtEditTo.Text);
            }
            if (ddlCategories.SelectedValue != "0")
            {
                query.Append("&catId=" + ddlCategories.SelectedValue);
            }
            if (ddlPostType.SelectedValue != "Both")
            {
                query.Append("&type=" + ddlPostType.SelectedValue);
            }
            if (!string.IsNullOrEmpty(txtKeyWord.Text))
            {
                query.Append("&k=" + HttpUtility.UrlEncode(txtKeyWord.Text));
            }
            Response.Redirect("postlist.aspx?" + query.ToString());
        }

        protected void btnExcute_Click(object sender, EventArgs e)
        {
            PostManager manager = new PostManager();
            List<long> list = new List<long>();
            if (Request.Form["chkSelected"] != null)
            {
                string[] ids = Request.Form["chkSelected"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ids.Length; i++)
                {
                    list.Add(Converter.LongParse(ids[i], 0));
                }
            }
            else
            {
                return;
            }

            try
            {
                switch (ddlOperation.SelectedValue)
                {
                    case "delete":
                        if (manager.DeletePosts(CurrentUser, list.ToArray()))
                        {
                            message.SetSuccessMessage("所选文章已删除！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选文章删除失败！");
                        }
                        break;
                    case "publish":
                        if (manager.PublishPosts(CurrentUser, list.ToArray()))
                        {
                            message.SetSuccessMessage("所选文章已发布！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选文章发布失败！");
                        }
                        break;
                    case "stoppublish":
                        if (manager.StopPublishPosts(CurrentUser, list.ToArray()))
                        {
                            message.SetSuccessMessage("所选文章已停止发布！");
                            GetData();
                        }
                        else
                        {
                            message.SetErrorMessage("所选文章停止发布失败！");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("所执行的批量操作失败：" + ex.Message);
            }
        }
    }
}
