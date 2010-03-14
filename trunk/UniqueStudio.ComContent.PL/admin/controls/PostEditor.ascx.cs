using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;

using UniqueStudio.Core.Category;
using UniqueStudio.Core.User;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.XmlHelper;

namespace UniqueStudio.ComContent.PL
{
    public partial class PostEditor : System.Web.UI.UserControl
    {
        public Unit Width
        {
            get
            {
                return fckContent.Width;
            }
            set { fckContent.Width = value; }
        }
        public Unit Height
        {
            get
            {
                return fckContent.Height;
            }
            set
            {
                fckContent.Height = value;
            }
        }

        private PostManager bll = new PostManager();
        private XmlManager xm = new XmlManager();
        private long uri;
        private EditorMode mode;

        public long Uri
        {
            get { return uri; }
            set { uri = value; }
        }
        public EditorMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CategoryManager manager = new CategoryManager();
                CategoryCollection categories = manager.GetAllCategories();
                cblCategory.DataSource = categories;
                cblCategory.DataTextField = "CategoryName";
                cblCategory.DataValueField = "CategoryID";
                cblCategory.DataBind();
                if (mode == EditorMode.Edit)
                {
                    PostInfo post = bll.GetPost(uri);

                    if (post != null)
                    {
                        if (post.Settings != string.Empty)
                        {
                            filename.Visible = true;
                            Enclosure attachement = (Enclosure)xm.ConvertToEntity(post.Settings, typeof(Enclosure), null);
                            filename.Text = attachement.Tittle;
                        }
                        txtTitle.Text = post.Title;
                        txtSubTitle.Text = post.SubTitle;
                        txtAuthor.Text = post.Author;
                        chbRecommend.Checked = post.IsRecommend;
                        chbHot.Checked = post.IsHot;
                        chbTop.Checked = post.IsTop;
                        chbAllowComment.Checked = post.IsAllowComment;
                        fckContent.Value = post.Content;
                        fckSummary.Value = post.Summary;
                        txtAddDate.Text = post.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                        switch (post.PostDisplay)
                        {
                            case 1:
                                tittleChecked.Checked = true;
                                break;
                            case 2:
                                otherChecked.Checked = true;
                                break;
                            case 3:
                                tittleChecked.Checked = true;
                                otherChecked.Checked = true;
                                break;
                            default:
                                break;
                        }
                        foreach (CategoryInfo item in post.Categories)
                        {
                            foreach (ListItem ite in cblCategory.Items)
                            {
                                if (item.CategoryId.ToString() == ite.Value)
                                {
                                    ite.Selected = true;
                                }
                            }
                        }
                        btnSave.Text = "保存";
                        if (post.IsPublished)
                        {
                            btnPublish.Visible = false;
                        }

                        ViewState["post"] = post;
                    }
                    else
                    {
                        message.SetErrorMessage("指定文章不存在");
                        btnPublish.Enabled = false;
                        btnSave.Enabled = false;
                    }
                }
                else
                {
                    UserInfo user = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
                    UserManager um = new UserManager();
                    user = um.GetUserInfo(user, user.UserId);
                    if (user.ExInfo != null)
                    {
                        txtAuthor.Text = user.ExInfo.PenName;
                    }
                    else
                    {
                        txtAuthor.Text = ((UserInfo)this.Session[GlobalConfig.SESSION_USER]).UserName;
                    }
                    txtAddDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            UserInfo user = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            PostPermissionManager ppm = new PostPermissionManager();
            PostInfo post = null;

            if (mode == EditorMode.Add)
            {
                post = new PostInfo();
                if (!enclosure.HasFile)
                {
                    //显示无文件信息
                    post.Settings = string.Empty;
                }
                else
                {
                    if (WebSiteConfig.EnclosureExtension.IndexOf(System.IO.Path.GetExtension(enclosure.FileName).ToLower()) < 0)
                    {
                        //显示扩展名不正确信息
                        Response.Write("<script type='text/javascript'>alert('扩展名不正确')</script>");
                        return;
                    }
                    else
                    {
                        string filepath = Server.MapPath(@"~/upload/" + post.Uri.ToString() + enclosure.FileName);
                        try
                        {
                            enclosure.SaveAs(filepath);
                            filename.Style["Visible"] = "true";
                            filename.Text = enclosure.FileName;
                        }
                        catch
                        {
                            //显示上传失败
                            Response.Write("<script type='text/javascript'>alert('文件上传失败，请重新上传')</script>");
                            return;
                        }
                        Enclosure enclosureInfo = new Enclosure();
                        enclosureInfo.Tittle = enclosure.FileName;
                        enclosureInfo.Length = enclosure.FileContent.Length;
                        enclosureInfo.Type = System.IO.Path.GetExtension(enclosure.FileName);
                        enclosureInfo.Url = "/upload/" + post.Uri.ToString() + enclosure.FileName;
                        XmlDocument xmldoc = xm.ConvertToXml(enclosureInfo, typeof(Enclosure));
                        post.Settings = xmldoc.OuterXml;
                    }
                }
                post.Title = txtTitle.Text;
                post.AddUserName = user.UserName;
                post.CreateDate = Converter.DatetimeParse(txtAddDate.Text, DateTime.Now);
                post.SubTitle = txtSubTitle.Text;
                post.Author = txtAuthor.Text;
                post.IsRecommend = chbRecommend.Checked;
                post.IsHot = chbHot.Checked;
                post.IsTop = chbTop.Checked;
                post.IsAllowComment = chbAllowComment.Checked;
                post.Content = fckContent.Value;
                //控制文章显示内容
                //0正常显示
                //1不显示标题
                //2不显示时间等其它信息
                //3只显示文章内容
                post.PostDisplay = 0;
                if (tittleChecked.Checked == true)
                {
                    post.PostDisplay += 1;
                }
                if (otherChecked.Checked == true)
                {
                    post.PostDisplay += 2;
                }
                CategoryManager cm = new CategoryManager();
                CategoryCollection cates = new CategoryCollection();
                bool IsSelected = false;
                foreach (ListItem item in cblCategory.Items)
                {
                    if (item.Selected)
                    {
                        IsSelected = true;
                        cates.Add(cm.GetCategory(Convert.ToInt32(item.Value)));
                    }
                }
                if (!IsSelected)
                {
                    Response.Write("<script type='text/javascript'>alert('请选择分类')</script>");
                }
                post.Categories = cates;
                if (!string.IsNullOrEmpty(fckSummary.Value))
                {
                    post.Summary = fckSummary.Value;
                }
                else
                {
                    post.Summary = "";
                }
                post.Taxis = 1;
                post.IsPublished = true;

                long postId = bll.AddPost(user, post);
                if (postId > 0)
                {
                    Response.Redirect("editpost.aspx?msg=" + HttpUtility.UrlEncode("发布成功！") + "&uri=" + postId);
                }
                else
                {
                    message.SetErrorMessage("发布失败，请重试！");
                }
            }
            else
            {
                if (ViewState["post"] != null)
                {
                    post = (PostInfo)ViewState["post"];
                    if (!ppm.HasEditPermission(user, post.AddUserName, false))
                    {
                        Response.Redirect("PostPermissionError.aspx?Error=编辑文章&Page=" + Request.UrlReferrer.ToString());
                    }
                    post.Title = txtTitle.Text;
                    post.SubTitle = txtSubTitle.Text;
                    post.Author = txtAuthor.Text;
                    post.IsRecommend = chbRecommend.Checked;
                    post.IsHot = chbHot.Checked;
                    post.IsTop = chbTop.Checked;
                    post.IsAllowComment = chbAllowComment.Checked;
                    post.Content = fckContent.Value;
                    post.CreateDate = Convert.ToDateTime(txtAddDate.Text);
                    //控制文章显示内容
                    //0正常显示
                    //1不显示标题
                    //2不显示时间等其它信息
                    //3只显示文章内容
                    post.PostDisplay = 0;
                    if (tittleChecked.Checked == true)
                    {
                        post.PostDisplay += 1;
                    }
                    if (otherChecked.Checked == true)
                    {
                        post.PostDisplay += 2;
                    }
                    post.Settings = string.Empty;
                    if (filename.Visible == true)
                    {
                        if (!enclosure.HasFile)
                        {
                            //显示无文件信息
                            post.Settings = string.Empty;
                        }
                        else
                        {
                            Enclosure attachement = (Enclosure)xm.ConvertToEntity(post.Settings, typeof(Enclosure), null);
                            if (!(attachement.Tittle == enclosure.FileName))
                            {
                                if (WebSiteConfig.EnclosureExtension.IndexOf(System.IO.Path.GetExtension(enclosure.FileName).ToLower()) < 0)
                                {
                                    //显示扩展名不正确信息
                                    Response.Write("<script type='text/javascript'>alert('扩展名不正确')</script>");
                                    return;
                                }
                                else
                                {
                                    string filepath = Server.MapPath(@"~/upload/" + post.Uri.ToString() + enclosure.FileName);
                                    try
                                    {
                                        enclosure.SaveAs(filepath);
                                        filename.Style["Visible"] = "true";
                                        filename.Text = enclosure.FileName;
                                    }
                                    catch
                                    {
                                        //显示上传失败
                                        Response.Write("<script type='text/javascript'>alert('文件上传失败，请重新上传')</script>");
                                        return;
                                    }
                                    Enclosure enclosureInfo = new Enclosure();
                                    enclosureInfo.Tittle = enclosure.FileName;
                                    enclosureInfo.Length = enclosure.FileContent.Length;
                                    enclosureInfo.Type = System.IO.Path.GetExtension(enclosure.FileName);
                                    enclosureInfo.Url = "/upload/" + post.Uri.ToString() + enclosure.FileName;
                                    XmlDocument xmldoc = xm.ConvertToXml(enclosureInfo, typeof(Enclosure));
                                    post.Settings = xmldoc.OuterXml;
                                }
                            }
                        }
                    }

                    CategoryManager cm = new CategoryManager();
                    CategoryCollection cates = new CategoryCollection();
                    bool isCategoryChecked = false;
                    foreach (ListItem item in cblCategory.Items)
                    {
                        if (item.Selected)
                        {
                            isCategoryChecked = true;
                            cates.Add(cm.GetCategory(Convert.ToInt32(item.Value)));
                        }
                    }
                    if (!isCategoryChecked)
                    {
                        Response.Write("<script type='text/javascript'>alert('请选择分类')</script>");
                    }
                    post.Categories = cates;
                    if (!string.IsNullOrEmpty(fckSummary.Value))
                    {
                        post.Summary = fckSummary.Value;
                    }
                    else
                    {
                        post.Summary = "";
                    }
                    post.LastEditUserName = user.UserName;
                    post.LastEditDate = DateTime.Now;
                    post.IsPublished = true;

                    if (bll.EditPost(user, post))
                    {
                        message.SetSuccessMessage("发布成功");
                    }
                    else
                    {
                        message.SetErrorMessage("发布失败，请重试！");
                    }
                }
                else
                {
                    message.SetErrorMessage("啊，出错了");
                    btnPublish.Enabled = false;
                    btnSave.Enabled = false;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserInfo user = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            PostPermissionManager ppm = new PostPermissionManager();
            PostInfo post = null;

            if (mode == EditorMode.Add)
            {
                post = new PostInfo();
                if (!enclosure.HasFile)
                {
                    //显示无文件信息
                    post.Settings = string.Empty;
                }
                else
                {
                    if (WebSiteConfig.EnclosureExtension.IndexOf(System.IO.Path.GetExtension(enclosure.FileName).ToLower()) < 0)
                    {
                        //显示扩展名不正确信息
                        Response.Write("<script type='text/javascript'>alert('扩展名不正确')</script>");
                        return;
                    }
                    else
                    {
                        string filepath = Server.MapPath(@"~/upload/" + post.Uri.ToString() + enclosure.FileName);
                        try
                        {
                            enclosure.SaveAs(filepath);
                            filename.Style["Visible"] = "true";
                            filename.Text = enclosure.FileName;
                        }
                        catch
                        {
                            //显示上传失败
                            Response.Write("<script type='text/javascript'>alert('文件上传失败，请重新上传')</script>");
                            return;
                        }
                        Enclosure enclosureInfo = new Enclosure();
                        enclosureInfo.Tittle = enclosure.FileName;
                        enclosureInfo.Length = enclosure.FileContent.Length;
                        enclosureInfo.Type = System.IO.Path.GetExtension(enclosure.FileName);
                        enclosureInfo.Url = "/upload/" + post.Uri.ToString() + enclosure.FileName;
                        XmlDocument xmldoc = xm.ConvertToXml(enclosureInfo, typeof(Enclosure));
                        post.Settings = xmldoc.OuterXml;
                    }
                }
                post.Title = txtTitle.Text;
                post.SubTitle = txtSubTitle.Text;
                post.Author = txtAuthor.Text;
                post.IsRecommend = chbRecommend.Checked;
                post.IsHot = chbHot.Checked;
                post.IsTop = chbTop.Checked;
                post.IsAllowComment = chbAllowComment.Checked;
                post.Content = fckContent.Value;
                //控制文章显示内容
                //0正常显示
                //1不显示标题
                //2不显示时间等其它信息
                //3只显示文章内容
                post.PostDisplay = 0;
                if (tittleChecked.Checked == true)
                {
                    post.PostDisplay += 1;
                }
                if (otherChecked.Checked == true)
                {
                    post.PostDisplay += 2;
                }
                //post.CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                CategoryManager cm = new CategoryManager();
                //string[] s = StringManager(cblCategory.SelectedValue);
                //CategoryCollection cates = new CategoryCollection();
                //for (int i = 0; i < s.Length; i++)
                //{
                //    cates.Add(cm.GetCategory(Convert.ToInt32(s[i])));
                //}
                //post.Categories = cates;
                CategoryCollection cates = new CategoryCollection();
                bool isCategoryChecked = false;
                foreach (ListItem item in cblCategory.Items)
                {
                    if (item.Selected)
                    {
                        isCategoryChecked = true;
                        cates.Add(cm.GetCategory(Convert.ToInt32(item.Value)));
                    }
                }
                if (!isCategoryChecked)
                {
                    Response.Write("<script type='text/javascript'>alert('请选择分类')</script>");
                }
                post.Categories = cates;
                if (!string.IsNullOrEmpty(fckSummary.Value))
                {
                    post.Summary = fckSummary.Value;
                }
                else
                {
                    //post.Summary = post.Content.Substring(0, Global.SummaryLength > post.Content.Length ? post.Content.Length : Global.SummaryLength);
                    post.Summary = "";
                }
                post.Taxis = 1;
                //post.AddUserName = this.Session[Global.SESSION_NAME].ToString();
                post.AddUserName = string.Empty;
                post.IsPublished = false;

                long postId = bll.AddPost(user, post);
                if (postId > 0)
                {
                    Response.Redirect("editpost.aspx?msg=" + HttpUtility.UrlEncode("保存成功！") + "&uri=" + postId);
                }
                else
                {
                    message.SetErrorMessage("保存失败，请重试！");
                }
            }
            else
            {
                if (ViewState["post"] != null)
                {
                    post = (PostInfo)ViewState["post"];
                    post.Title = txtTitle.Text;
                    post.SubTitle = txtSubTitle.Text;
                    post.Author = txtAuthor.Text;
                    post.IsRecommend = chbRecommend.Checked;
                    post.IsHot = chbHot.Checked;
                    post.IsTop = chbTop.Checked;
                    post.IsAllowComment = chbAllowComment.Checked;
                    post.Content = fckContent.Value;
                    post.CreateDate = Convert.ToDateTime(txtAddDate.Text);
                    //控制文章显示内容
                    //0正常显示
                    //1不显示标题
                    //2不显示时间等其它信息
                    //3只显示文章内容
                    post.PostDisplay = 0;
                    if (tittleChecked.Checked == true)
                    {
                        post.PostDisplay += 1;
                    }
                    if (otherChecked.Checked == true)
                    {
                        post.PostDisplay += 2;
                    }
                    //  post.CategoryId = Convert.ToInt32(ddlCategory.SelectedValue);
                    CategoryManager cm = new CategoryManager();
                    //string[] s = StringManager(cblCategory.SelectedValue);
                    //CategoryCollection cates = new CategoryCollection();
                    //for (int i = 0; i < s.Length; i++)
                    //{
                    //    cates.Add(cm.GetCategory(Convert.ToInt32(s[i])));
                    //}
                    //post.Categories = cates;
                    post.Settings = string.Empty;
                    if (filename.Visible == true)
                    {
                        if (!enclosure.HasFile)
                        {
                            //显示无文件信息
                            post.Settings = string.Empty;
                        }
                        else
                        {
                            Enclosure attachement = (Enclosure)xm.ConvertToEntity(post.Settings, typeof(Enclosure), "");
                            if (!(attachement.Tittle == enclosure.FileName))
                            {
                                if (WebSiteConfig.EnclosureExtension.IndexOf(System.IO.Path.GetExtension(enclosure.FileName).ToLower()) < 0)
                                {
                                    //显示扩展名不正确信息
                                    Response.Write("<script type='text/javascript'>alert('扩展名不正确')</script>");
                                    return;
                                }
                                else
                                {
                                    string filepath = Server.MapPath(@"~/upload/" + post.Uri.ToString() + enclosure.FileName);
                                    try
                                    {
                                        enclosure.SaveAs(filepath);
                                        filename.Style["Visible"] = "true";
                                        filename.Text = enclosure.FileName;
                                    }
                                    catch
                                    {
                                        //显示上传失败
                                        Response.Write("<script type='text/javascript'>alert('文件上传失败，请重新上传')</script>");
                                        return;
                                    }
                                    Enclosure enclosureInfo = new Enclosure();
                                    enclosureInfo.Tittle = enclosure.FileName;
                                    enclosureInfo.Length = enclosure.FileContent.Length;
                                    enclosureInfo.Type = System.IO.Path.GetExtension(enclosure.FileName);
                                    enclosureInfo.Url = "/upload/" + post.Uri.ToString() + enclosure.FileName;
                                    XmlDocument xmldoc = xm.ConvertToXml(enclosureInfo, typeof(Enclosure));
                                    post.Settings = xmldoc.OuterXml;
                                }
                            }
                        }
                    }
                    CategoryCollection cates = new CategoryCollection();
                    bool isCategoryChecked = false;
                    foreach (ListItem item in cblCategory.Items)
                    {
                        if (item.Selected)
                        {
                            isCategoryChecked = true;
                            cates.Add(cm.GetCategory(Convert.ToInt32(item.Value)));
                        }
                    }
                    if (!isCategoryChecked)
                    {
                        Response.Write("<script type='text/javascript'>alert('请选择分类')</script>");
                    }
                    post.Categories = cates;
                    if (!string.IsNullOrEmpty(fckSummary.Value))
                    {
                        post.Summary = fckSummary.Value;
                    }
                    else
                    {
                        //post.Summary = post.Content.Substring(0, Global.SummaryLength > post.Content.Length ? post.Content.Length : Global.SummaryLength);
                    }
                    //post.LastEditUserName = this.Session[Global.SESSION_NAME].ToString();

                    if (bll.EditPost(user, post))
                    {
                        message.SetSuccessMessage("保存成功");
                        //Response.Redirect("editpost.aspx?uri=" + post.Uri);
                    }
                    else
                    {
                        message.SetErrorMessage(" 保存失败，请重试！");
                    }
                }
                else
                {
                    message.SetErrorMessage("啊，出错了");
                    btnPublish.Enabled = false;
                    btnSave.Enabled = false;
                }
            }
        }
    }
}
////protected void upfilebtn_Click(object sender, EventArgs e)
//{
//    //if (!(filename.Style["Visible"] == "false"))
//    //{
//    //    //提示会删除原附件
//    //}
//    //if (!enclosure.HasFile)
//    //{
//    //    //显示无文件信息
//    //    return;
//    //}
//    //else
//    //{
//    //    if (WebSiteConfig.EnclosureExtension.IndexOf(System.IO.Path.GetExtension(enclosure.FileName).ToLower()) < 0)
//    //    {
//    //        //显示扩展名不正确信息
//    //        return;
//    //    }
//    //    else
//    //    {
//    //        string filepath = Server.MapPath(@"~/upload/" + enclosure.FileName);
//    //        try
//    //        {
//    //            enclosure.SaveAs(filepath);
//    //            filename.Style["Visible"] = "true";
//    //            filename.Text = enclosure.FileName;
//    //        }
//    //        catch
//    //        {
//    //            //显示上传失败
//    //            return;
//    //        }
//    //    }
//    //}
//}
