using System;
using System.Text;
using System.Web.UI;
using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.Core.Category;

namespace UniqueStudio.ComContent.PL
{
    public partial class view : System.Web.UI.Page
    {
        protected DateTime dt;

        protected override void OnPreInit(EventArgs e)
        {
            dt = DateTime.Now;
            base.OnPreInit(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            TimeSpan ts = DateTime.Now - dt;
            Response.Write("<!--query time:" + ts.TotalMilliseconds + "-->");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long uri = Common.Utility.LongParse(Request.QueryString["uri"], 0);
                if (uri == 0)
                {
                    Response.Redirect(WebSiteConfig.BaseAddress + "/404.aspx");
                }

                PostManager postManager = new PostManager();
                PostInfo post = postManager.GetPost(uri);
                if (post != null)
                {
                    Page.Header.Title = post.Title + " - " + Common.Config.WebSiteConfig.WebName;
                    ltlTitle.Text = post.Title;
                    ltlAuthor.Text = post.Author.Length == 0 ? "匿名" : post.Author;
                    ltlCount.Text = post.Count.ToString();
                    ltlCreateDate.Text = post.CreateDate.ToString("yyyy-MM-dd");
                    ltlContent.Text = post.Content;

                    switch (post.PostDisplay)
                    {
                        case 1://不显示标题
                            ltlTitle.Visible = false;
                            break;
                        case 2://不显示时间作者等信息（在一个div里）
                            divDetail.Visible = false;
                            break;
                        case 3://标题及时间作者都不显示
                            ltlTitle.Visible = false;
                            divDetail.Visible = false;
                            break;
                        default:
                            break;
                    }

                    //设置附件
                    if (!string.IsNullOrEmpty(post.Settings))
                    {
                        divAttachment.Visible = true;
                        XmlManager xm = new XmlManager();
                        Enclosure enclosure = (Enclosure)xm.ConvertToEntity(post.Settings, typeof(Enclosure), null);
                        if (enclosure != null)
                        {
                            ltlAttachmentExt.Text = enclosure.Type.Substring(1);
                            ltlAttachmentLink.Text = WebSiteConfig.BaseAddress + enclosure.Url;
                            ltlAttachmentTitle.Text = enclosure.Tittle;
                        }
                    }

                    CategoryCollection categories = post.Categories;

                    int categoryId = Common.Utility.IntParse(Request.QueryString["catId"], 0);
                    if (categoryId == 0 && categories.Count > 0)
                    {
                        categoryId = categories[0].CategoryId;
                    }

                    StringBuilder sb = new StringBuilder();
                    CategoryManager manager = new CategoryManager();
                    CategoryInfo category = manager.GetCategoryPath(categoryId);
                    if (category != null)
                    {
                        //设置侧边栏
                        subCategories.CategoryId = category.CategoryId;
                    }
                    while (category != null)
                    {
                        sb.Append(string.Format("-><a href='list.aspx?catId={0}'>{1}</a>  ", category.CategoryId, category.CategoryName));
                        category = category.ChildCategory;
                    }
                    ltlCategoryLink.Text = sb.ToString();

                    //增加访问量
                    try
                    {
                        postManager.IncPostReadCount(uri);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    Response.Redirect(WebSiteConfig.BaseAddress + "/404.aspx");
                }
            }
        }
    }
}
