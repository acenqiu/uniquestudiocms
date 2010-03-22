using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.Core.Category;
using UniqueStudio.Core.Site;
using UniqueStudio.Core.User;

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

        private UserInfo currentUser;
        private long uri;
        private int siteId;
        private EditorMode mode;

        public long Uri
        {
            get { return uri; }
            set { uri = value; }
        }
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        public EditorMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = (UserInfo)this.Session[GlobalConfig.SESSION_USER];
            if (!IsPostBack)
            {
                CategoryManager manager = new CategoryManager();
                CategoryCollection categories = manager.GetAllCategories(siteId);
                cblCategory.DataSource = categories;
                cblCategory.DataTextField = "CategoryName";
                cblCategory.DataValueField = "CategoryID";
                cblCategory.DataBind();

                if (mode == EditorMode.Edit)
                {
                    PostInfo post = bll.GetPost(currentUser, uri);

                    if (post != null)
                    {
                        if (post.Settings != string.Empty)
                        {
                            filename.Visible = true;
                            Enclosure attachement = (Enclosure)xm.ConvertToEntity(post.Settings, typeof(Enclosure), null);
                            filename.Text = attachement.Tittle;
                        }
                        //if (post.NewsImage != string.Empty)
                        //{
                        //    imagename.Visible = true;
                        //    imagename.Text = post.NewsImage;
                        //}
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
                        foreach (CategoryInfo category in post.Categories)
                        {
                            foreach (ListItem item in cblCategory.Items)
                            {
                                if (category.CategoryId.ToString() == item.Value)
                                {
                                    item.Selected = true;
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
                    //EditorMode.Add
                    currentUser = (new UserManager()).GetUserInfo(currentUser, currentUser.UserId);
                    if (currentUser.ExInfo != null)
                    {
                        txtAuthor.Text = currentUser.ExInfo.PenName;
                    }
                    else
                    {
                        txtAuthor.Text = currentUser.UserName;
                    }
                    txtAddDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                }
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
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
                    if (SiteManager.Config(siteId).EnclosureExtension.IndexOf(System.IO.Path.GetExtension(enclosure.FileName).ToLower()) < 0)
                    {
                        //显示扩展名不正确信息

                        //TODO:该行重写！
                        Response.Write("<script type='text/javascript'>alert('扩展名不正确')</script>");
                        return;
                    }
                    else
                    {
                        string urlpath = DateTime.Now.ToString("yyyyMMddHHmmss") + enclosure.FileName;
                        string filepath = Server.MapPath(@"~/upload/" + urlpath);
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
                        enclosureInfo.Url = "/upload/" + urlpath;
                        XmlDocument xmldoc = xm.ConvertToXml(enclosureInfo, typeof(Enclosure));
                        post.Settings = xmldoc.OuterXml;
                    }
                }
                post.SiteId = siteId;
                post.Title = txtTitle.Text;
                post.AddUserName = currentUser.UserName;
                post.CreateDate = Converter.DatetimeParse(txtAddDate.Text, DateTime.Now);
                post.SubTitle = txtSubTitle.Text;
                post.Author = txtAuthor.Text;
                post.IsRecommend = chbRecommend.Checked;
                post.IsHot = chbHot.Checked;
                post.IsTop = chbTop.Checked;
                post.IsAllowComment = chbAllowComment.Checked;
                post.Content = fckContent.Value;
                post.NewsImage = NewsImageManager();
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

                long postId = bll.AddPost(currentUser, post);
                if (postId > 0)
                {
                    Response.Redirect(string.Format("editpost.aspx?msg={0}&siteId={1}&uri={2}",HttpUtility.UrlEncode("发布成功！"),siteId, postId));
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
                    if (!PostPermissionManager.HasEditPermission(currentUser,post.Uri))
                    {
                        //可能出现空引用
                        //Response.Redirect("PostPermissionError.aspx?Error=编辑文章&Page=" + Request.UrlReferrer.ToString());
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
                    post.NewsImage = NewsImageManager();
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
                                if (SiteManager.Config(siteId).EnclosureExtension.IndexOf(System.IO.Path.GetExtension(enclosure.FileName).ToLower()) < 0)
                                {
                                    //显示扩展名不正确信息
                                    //下行重写!
                                    Response.Write("<script type='text/javascript'>alert('扩展名不正确')</script>");
                                    return;
                                }
                                else
                                {
                                    string urlpath = DateTime.Now.ToString("yyyyMMddHHmmss") + enclosure.FileName;
                                    string filepath = Server.MapPath(@"~/upload/" + urlpath);
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
                                    enclosureInfo.Url = "/upload/" + urlpath;
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
                    post.LastEditUserName = currentUser.UserName;
                    post.LastEditDate = DateTime.Now;
                    post.IsPublished = true;

                    if (bll.EditPost(currentUser, post))
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
                    if (SiteManager.Config(siteId).EnclosureExtension.IndexOf(System.IO.Path.GetExtension(enclosure.FileName).ToLower()) < 0)
                    {
                        //显示扩展名不正确信息
                        Response.Write("<script type='text/javascript'>alert('扩展名不正确')</script>");
                        return;
                    }
                    else
                    {
                        string urlpath = DateTime.Now.ToString("yyyyMMddHHmmss") + enclosure.FileName;
                        string filepath = Server.MapPath(@"~/upload/" + urlpath);
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
                        enclosureInfo.Url = "/upload/" + urlpath;
                        XmlDocument xmldoc = xm.ConvertToXml(enclosureInfo, typeof(Enclosure));
                        post.Settings = xmldoc.OuterXml;
                    }
                }
                post.SiteId = siteId;
                post.Title = txtTitle.Text;
                post.SubTitle = txtSubTitle.Text;
                post.Author = txtAuthor.Text;
                post.IsRecommend = chbRecommend.Checked;
                post.IsHot = chbHot.Checked;
                post.IsTop = chbTop.Checked;
                post.IsAllowComment = chbAllowComment.Checked;
                post.Content = fckContent.Value;
                post.NewsImage = NewsImageManager();
                //控制文章显示内容
                //0正常显示
                //1不显示标题
                //2不显示时间等其它信息
                //3只显示文章内容
                post.PostDisplay = 0;
                if (tittleChecked.Checked == true)
                {
                    post.PostDisplay = 1;
                }
                if (otherChecked.Checked == true)
                {
                    post.PostDisplay += 2;
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
                    //post.Summary = post.Content.Substring(0, Global.SummaryLength > post.Content.Length ? post.Content.Length : Global.SummaryLength);
                    post.Summary = "";
                }
                post.Taxis = 1;
                post.AddUserName = string.Empty;
                post.IsPublished = false;

                long postId = bll.AddPost(currentUser, post);
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
                        post.PostDisplay = 1;
                    }
                    if (otherChecked.Checked == true)
                    {
                        post.PostDisplay += 2;
                    }
                    CategoryManager cm = new CategoryManager();
                    post.Settings = string.Empty;
                    post.NewsImage = NewsImageManager();
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
                                if (SiteManager.Config(siteId).EnclosureExtension.IndexOf(System.IO.Path.GetExtension(enclosure.FileName).ToLower()) < 0)
                                {
                                    //显示扩展名不正确信息
                                    Response.Write("<script type='text/javascript'>alert('扩展名不正确')</script>");
                                    return;
                                }
                                else
                                {
                                    string urlpath = DateTime.Now.ToString("yyyyMMddHHmmss") + enclosure.FileName;
                                    string filepath = Server.MapPath(@"~/upload/" + urlpath);
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
                                    enclosureInfo.Url = "/upload/" + urlpath;
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

                    if (bll.EditPost(currentUser, post))
                    {
                        message.SetSuccessMessage("保存成功");
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

        private string NewsImageManager()
        {
            if (!newsimage.HasFile)
            {
                return null;
            }
            else
            {
                string imageextension = ".jpg,.jpeg,.gif,.png";
                if (imageextension.IndexOf(System.IO.Path.GetExtension(newsimage.FileName).ToLower()) < 0)
                {
                    //显示扩展名不正确信息
                    Response.Write("<script type='text/javascript'>alert('扩展名不正确')</script>");
                    return null;
                }
                else
                {
                    string urlpath = DateTime.Now.ToString("yyyyMMddHHmmss") + newsimage.FileName;
                    string filepath = Server.MapPath(@"~/upload/image/" + urlpath);
                    try
                    {
                        newsimage.SaveAs(filepath);
                        imagename.Visible = true;
                        imagename.Text = newsimage.FileName;
                        // filename.Style["Visible"] = "true";
                        //filename.Text = enclosure.FileName;
                    }
                    catch
                    {
                        //显示上传失败
                        Response.Write("<script type='text/javascript'>alert('新闻图片上传失败，请重新上传')</script>");
                        return null;
                    }
                    return "/upload/image/" + urlpath;
                }
            }
        }
    }
}