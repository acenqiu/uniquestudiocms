using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.Core.Category;
using UniqueStudio.Core.User;

namespace UniqueStudio.ComContent.PL
{
    public partial class PostEditor : System.Web.UI.UserControl
    {
        private UserInfo currentUser;
        private long uri;
        private int siteId;
        private EditorMode mode;
        private SettingsManager settingsManager = new SettingsManager();
        private PostManager postManager = new PostManager();
        private XmlManager xmlManager = new XmlManager();
        private AutoSaveManager am = new AutoSaveManager();
        protected Guid userId;
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
            if (!IsPostBack)
            {
                //获取分类列表
                try
                {
                    CategoryManager manager = new CategoryManager();
                    CategoryCollection categories = manager.GetAllCategories(siteId);
                    cblCategory.DataSource = categories;
                    cblCategory.DataTextField = "CategoryName";
                    cblCategory.DataValueField = "CategoryID";
                    cblCategory.DataBind();
                }
                catch (Exception ex)
                {
                    message.SetErrorMessage("分类信息读取失败：" + ex.Message);
                }

                if (mode == EditorMode.Add)
                {
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
                    #region 自动保存载入
                    PostInfo autoSavedPost = am.GetEftAutoSavedFileForAdd(userId);
                    if (autoSavedPost != null)
                    {
                        if (autoSavedPost.Uri != 0)
                        {
                            Session["posturi"] = autoSavedPost.Uri;
                        }
                        txtAuthor.Text = autoSavedPost.Author;
                        txtTitle.Text = autoSavedPost.Title;
                        txtSubTitle.Text = autoSavedPost.SubTitle;
                        fckContent.Value = autoSavedPost.Content;
                        fckSummary.Value = autoSavedPost.Summary;
                        message.SetSuccessMessage("以下为系统自动保存的内容");
                    }
                    #endregion
                }
                else
                {
                    {
                        LoadPost();
                    }
                }
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
                        sb.Append("<div id=\"editenclosure" + i.ToString() + "\">" + enclosure.Title + "<img src=\"img/f2.gif\" onclick=\"HideEditDiv('" + i.ToString() + "');DeleteEnclosure('" + enclosure.Title + "')\">" + "</div>");
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
                post = am.GetEftAutoSavedFileForEdit(userId, uri);
                if (post != null)
                {
                    fckContent.Value = post.Content;
                    fckSummary.Value = post.Summary;
                    message.SetSuccessMessage("已载入此文章自动保存内容");
                }
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

        //保存为草稿
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
                long postUri = postManager.AddPost(currentUser, post);
                if (postUri > 0)
                {
                    am.SetAutoSaveFileEft(userId, false);
                    Response.Redirect(string.Format("editpost.aspx?msg={0}&siteId={1}&uri={2}", HttpUtility.UrlEncode(postType + "添加成功！")
                                                                                                                                          , siteId, postUri));
                }
                else
                {
                    message.SetErrorMessage(postType + "添加失败！");
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                message.SetErrorMessage(postType + "添加失败：" + ex.Message);
            }
        }

        private void EditPost(bool isPublished)
        {
            try
            {
                PostInfo post = PreparePost(uri, isPublished);
                post.LastEditUserName = currentUser.UserName;
                if (postManager.EditPost(currentUser, post))
                {
                    message.SetSuccessMessage("保存成功！");
                }
                else
                {
                    message.SetErrorMessage("保存失败！");
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
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
            post.Taxis = 1;
            post.Title = txtTitle.Text;
            post.SubTitle = txtSubTitle.Text;
            post.Author = txtAuthor.Text;
            post.Summary = fckSummary.Value;
            post.Content = fckContent.Value;
            post.NewsImage = GetNewsImage();
            post.Settings = settingsManager.GetPostXMLFromEnclosures(uri.ToString(), Server.MapPath(@"~/upload/"));
            post.Categories = GetSelectedCategories();
            if (post.Categories.Count == 0)
            {
                message.SetErrorMessage("请至少选择一个分类！");
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
                string imageExtension = ".jpg,.jpeg,.gif,.png";
                if (imageExtension.IndexOf(System.IO.Path.GetExtension(fuNewsImage.FileName).ToLower()) < 0)
                {
                    //显示扩展名不正确信息
                    Response.Write("<script type='text/javascript'>alert('扩展名不正确')</script>");
                    return null;
                }
                else
                {
                    string urlpath = DateTime.Now.ToString("yyyyMMddHHmmss") + fuNewsImage.FileName;
                    string filepath = Server.MapPath(@"~/upload/image/" + urlpath);
                    try
                    {
                        fuNewsImage.SaveAs(filepath);
                        lblImageName.Visible = true;
                        lblImageName.Text = fuNewsImage.FileName;
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
