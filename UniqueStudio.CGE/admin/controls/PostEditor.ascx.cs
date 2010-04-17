using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Category;
using UniqueStudio.Core.Site;
using UniqueStudio.Core.User;

namespace UniqueStudio.ComContent.Admin
{
    public partial class PostEditor : System.Web.UI.UserControl
    {
        private UserInfo currentUser;
        private long uri;
        private int siteId;
        private EditorMode mode;
        private SettingsManager settingsManager = new SettingsManager();
        private PostManager postManager = new PostManager();
        protected Guid userId;
        private string categoryPrototype = "#{0}#*{1}*";

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
            userId = currentUser.UserId;

            if (IsPostBack)
            {
                divASPrompt.Visible = false;
            }
            else
            {
                try
                {
                    LoadCategories();
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("分类信息读取失败：" + ex.Message);
                    return;
                }

                try
                {
                    if (mode == EditorMode.Add)
                    {
                        InitAddMode();
                    }
                    else
                    {
                        InitEditMode();
                    }
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("初始化失败：" + ex.Message);
                }
            }
        }

        private void LoadCategories()
        {
            CategoryManager manager = new CategoryManager();
            CategoryCollection categories = manager.GetAllCategories(siteId);
            cblCategory.DataSource = categories;
            cblCategory.DataTextField = "CategoryName";
            cblCategory.DataValueField = "CategoryID";
            cblCategory.DataBind();
            foreach (ListItem item in cblCategory.Items)
            {
                StringBuilder sb = new StringBuilder(item.Text);
                CategoryInfo cat = manager.GetCategory(Convert.ToInt32(item.Value));
                sb.Append(String.Format(categoryPrototype, new string[] { item.Value, cat.ParentCategoryId.ToString() }));
                item.Text = sb.ToString();
            }
        }

        private void InitAddMode()
        {
            AutoSaveManager manager = new AutoSaveManager();
            PostInfo autoSavedPost = manager.GetAutoSavedPost(userId);

            if (autoSavedPost != null)
            {
                if (manager.IsPostSaved(currentUser.UserId))
                {
                    //跳转到编辑页面
                    Response.Redirect(string.Format("editpost.aspx?siteId={0}&uri={1}", siteId, autoSavedPost.Uri));
                }
                else
                {
                    //载入
                    if (autoSavedPost.Uri != 0)
                    {
                        Session["posturi"] = autoSavedPost.Uri;
                    }
                    txtAuthor.Text = autoSavedPost.Author;
                    txtTitle.Text = autoSavedPost.Title;
                    txtSubTitle.Text = autoSavedPost.SubTitle;
                    fckContent.Value = autoSavedPost.Content;
                    fckSummary.Value = autoSavedPost.Summary;
                    divASPrompt.Visible = true;
                }
            }
            else
            {
                //没有自动保存的文章
                currentUser = (new UserManager()).GetUserInfo(currentUser, currentUser.UserId);
                if (currentUser.ExInfo != null)
                {
                    txtAuthor.Text = currentUser.ExInfo.PenName;
                }
                else
                {
                    txtAuthor.Text = currentUser.UserName;
                }
            }
            txtAddDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }

        private void InitEditMode()
        {
            AutoSaveManager manager = new AutoSaveManager();
            PostInfo autoSavedPost = manager.GetAutoSavedPost(userId);
            if (autoSavedPost != null)
            {
                if (manager.IsPostSaved(currentUser.UserId))
                {
                    if (autoSavedPost.Uri != uri)
                    {
                        Response.Redirect(string.Format("editpost.aspx?siteId={0}&uri={1}&oriUri={2}&ret={3}"
                                                                            , siteId
                                                                            , autoSavedPost.Uri
                                                                            , uri
                                                                            , Request.QueryString["ret"]));
                    }
                    else
                    {
                        //载入文章
                        LoadPost();
                        txtTitle.Text = autoSavedPost.Title;
                        txtSubTitle.Text = autoSavedPost.SubTitle;
                        fckContent.Value = autoSavedPost.Content;
                        fckSummary.Value = autoSavedPost.Summary;
                        divASPrompt.Visible = true;
                    }
                }
                else
                {
                    //跳转到新增页面
                    Response.Redirect(string.Format("addpost.aspx?siteId={0}&oriUri={1}&ret={2}"
                                                                            , siteId
                                                                            , uri
                                                                            , Request.QueryString["ret"]));
                }
            }
            else
            {
                LoadPost();
            }
        }

        private void LoadPost()
        {
            PostInfo post = null;
            try
            {
                post = postManager.GetPost(currentUser, uri);
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
                return;
            }

            if (post == null)
            {
                message.SetErrorMessage("数据读取失败：指定文章不存在！");
                btnPublish.Enabled = false;
                btnSave.Enabled = false;
                return;
            }
            else
            {
                txtTitle.Text = post.Title;
                txtSubTitle.Text = post.SubTitle;
                txtAddDate.Text = post.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                txtAuthor.Text = post.Author;
                fckContent.Value = post.Content;
                fckSummary.Value = post.Summary;
                chbRecommend.Checked = post.IsRecommend;
                chbHot.Checked = post.IsHot;
                chbTop.Checked = post.IsTop;
                chbAllowComment.Checked = post.IsAllowComment;
                if (post.Settings != string.Empty)
                {
                    //TODO:display enclosure
                    EnclosureCollection enclosures = settingsManager.GetEnclosuresFromXML(post.Settings);
                    StringBuilder sb = new StringBuilder();
                    int i = 0;
                    foreach (Enclosure enclosure in enclosures)
                    {
                        //sb.Append("<div id=\"editenclosure" + i.ToString() + "\">" + enclosure.Title + "<img src=\"images/f2.gif\" onclick=\"HideEditDiv('" + i.ToString() + "');DeleteEnclosure('" + enclosure.Title + "')\">" + "</div>");
                        sb.Append("<div id=\"editenclosure" + i.ToString() + "\">" + enclosure.Title + "<span class=\"delFile\" onclick=\"HideEditDiv('" + i.ToString() + "');DeleteEnclosure('" + enclosure.Title + "')\">删除</span></div>");
                        i++;
                    }
                    text.InnerHtml = sb.ToString();
                }
                switch (post.PostDisplay)
                {
                    case 1:
                        chkTitle.Checked = true;
                        break;
                    case 2:
                        chkOther.Checked = true;
                        break;
                    case 3:
                        chkTitle.Checked = true;
                        chkOther.Checked = true;
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

                ViewState["IsPublished"] = post.IsPublished;
                ViewState["NewsImage"] = post.NewsImage;
            }
        }

        protected void btnPublish_Click(object sender, EventArgs e)
        {
            if (mode == EditorMode.Add)
            {
                AddPost("文章", true);
            }
            else
            {
                EditPost(true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (mode == EditorMode.Add)
            {
                AddPost("草稿", false);
            }
            else
            {
                bool isPublished = (bool)ViewState["IsPublished"];
                EditPost(isPublished);
            }
        }

        private void AddPost(string postType, bool isPublished)
        {
            try
            {
                PostInfo post = PreparePost(Convert.ToInt64(Session["posturi"].ToString()), isPublished);
                post.AddUserName = currentUser.UserName;
                post.NewsImage = GetNewsImage();
                long postUri = postManager.AddPost(currentUser, post);
                if (postUri > 0)
                {
                    if (Request.QueryString["oriUri"] != null)
                    {
                        Response.Redirect(string.Format("editpost.aspx?msg={0}&siteId={1}&uri={2}&ret={3}", HttpUtility.UrlEncode("自动保存的文章处理成功，现在您可以编辑原来打算编辑的文章！")
                                                                                                                                              , siteId, Request.QueryString["oriUri"]
                                                                                                                                              , Request.QueryString["ret"]));
                    }
                    else
                    {
                        Response.Redirect(string.Format("editpost.aspx?msg={0}&siteId={1}&uri={2}", HttpUtility.UrlEncode(postType + "添加成功！")
                                                                                                                                              , siteId, postUri));
                    }
                }
                else
                {
                    message.SetErrorMessage(postType + "添加失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage(postType + "添加失败：" + ex.Message);
            }
        }

        private void EditPost(bool isPublished)
        {
            try
            {
                PostInfo post = PreparePost(uri, isPublished);
                post.LastEditUserName = currentUser.UserName;
                string newsImage = GetNewsImage();
                if (!string.IsNullOrEmpty(newsImage))
                {
                    post.NewsImage = newsImage;
                }
                else
                {
                    post.NewsImage = (string)ViewState["NewsImage"];
                }
                if (postManager.EditPost(currentUser, post))
                {
                    string prompt = "保存成功！";
                    if (Request.QueryString["oriUri"] != null)
                    {
                        prompt = string.Format("自动保存的文章处理成功，您可以<a href='editpost.aspx?siteId={0}&uri={1}&ret={2}'>编辑原来打算编辑的文章</a>。"
                                                            , siteId
                                                            , Request.QueryString["oriUri"]
                                                            , Request.QueryString["ret"]);
                    }
                    message.SetSuccessMessage(prompt);
                }
                else
                {
                    message.SetErrorMessage("保存失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("保存失败：" + ex.Message);
            }
        }

        private PostInfo PreparePost(long uri, bool isPublished)
        {
            PostInfo post = new PostInfo();
            //基本信息
            post.Uri = uri;
            post.SiteId = siteId;
            post.CreateDate = Converter.DatetimeParse(txtAddDate.Text, DateTime.Now);
            post.Type = 1;
            post.Title = txtTitle.Text;
            post.SubTitle = txtSubTitle.Text;
            post.Author = txtAuthor.Text;
            post.Summary = fckSummary.Value;
            post.Content = fckContent.Value;
            post.Settings = settingsManager.GetPostXMLFromEnclosures(uri.ToString(), Server.MapPath(@"~/upload/"));
            post.Categories = GetSelectedCategories();
            if (post.Categories.Count == 0)
            {
                throw new Exception("请至少选择一个分类！");
            }

            //显示设置
            post.IsRecommend = chbRecommend.Checked;
            post.IsHot = chbHot.Checked;
            post.IsTop = chbTop.Checked;
            post.IsAllowComment = chbAllowComment.Checked;
            post.IsPublished = isPublished;
            post.PostDisplay = 0;
            if (chkTitle.Checked == true)
            {
                post.PostDisplay = 1;
            }
            if (chkOther.Checked == true)
            {
                post.PostDisplay += 2;
            }

            return post;
        }

        private CategoryCollection GetSelectedCategories()
        {
            CategoryCollection categories = new CategoryCollection();
            foreach (ListItem item in cblCategory.Items)
            {
                if (item.Selected)
                {
                    categories.Add(new CategoryInfo(Converter.IntParse(item.Value, 0)));
                }
            }
            return categories;
        }

        private string GetNewsImage()
        {
            if (!fuNewsImage.HasFile)
            {
                return null;
            }
            else
            {
                string allowedImageExtension = ".jpg,.jpeg,.gif,.png";
                string extension = Path.GetExtension(fuNewsImage.FileName).ToLower();
                if (allowedImageExtension.IndexOf(extension) < 0)
                {
                    throw new Exception("新闻图片的格式不正确，请重新选择！");
                }
                else
                {
                    //控件对中文路径支持不好
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    string filePath = SiteManager.BasePhysicalPath(siteId) + "\\upload\\image\\" + fileName;
                    fuNewsImage.SaveAs(filePath);
                    lblImageName.Visible = true;
                    lblImageName.Text = fuNewsImage.FileName;
                    return "upload/image/" + fileName;
                }
            }
        }

        protected void btnDiscard_Click(object sender, EventArgs e)
        {
            try
            {
                if ((new AutoSaveManager()).SetAutoSaveFileEft(currentUser.UserId, false))
                {
                    if (Request.QueryString["oriUri"] != null)
                    {
                        Response.Redirect(string.Format("editpost.aspx?siteId={0}&uri={1}&ret={2}", siteId
                                                                                                                                             , Request["oriUri"]
                                                                                                                                             , Request["ret"]));
                    }
                    else
                    {
                        if (mode == EditorMode.Add)
                        {
                            Response.Redirect("addpost.aspx?siteId=" + siteId);
                        }
                        else
                        {
                            divASPrompt.Visible = false;
                            LoadPost();
                        }
                    }
                }
                else
                {
                    message.SetErrorMessage("自动保存文章舍弃失败！");
                }
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("自动保存文章舍弃失败：" + ex.Message);
            }
        }
    }
}
