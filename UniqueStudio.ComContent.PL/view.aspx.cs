using System;
using System.Text;
using System.Web.UI;
using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.Core.Category;
using UniqueStudio.Core.Site;
using UniqueStudio.Common.Utilities;

namespace UniqueStudio.ComContent.PL
{
    public partial class view : Controls.PlBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                long uri = Converter.LongParse(Request.QueryString["uri"], 0);
                if (uri == 0)
                {
                    Response.Redirect(PathHelper.PathCombine(SiteManager.BaseAddress(SiteId), "404.aspx"));
                }

                PostManager postManager = new PostManager();
                PostInfo post = postManager.GetPost(uri);
                if (post == null)
                {
                    Response.Redirect(PathHelper.PathCombine(SiteManager.BaseAddress(SiteId), "404.aspx"));
                }
                else
                {
                    Page.Header.Title = post.Title + " - " + SiteManager.Config(SiteId).WebName;
                    ltlTitle.Text = post.Title;
                    ltlAuthor.Text = post.Author.Length == 0 ? "匿名" : post.Author;
                    ltlCount.Text = post.Count.ToString();
                    ltlCreateDate.Text = post.CreateDate.ToString("yyyy-MM-dd");
                    ltlContent.Text = post.Content;

                    switch (post.PostDisplay)
                    {
                        case 1:
                            ltlTitle.Visible = false;
                            break;
                        case 2:
                            divDetail.Visible = false;
                            break;
                        case 3:
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
                            ltlAttachmentLink.Text = PathHelper.PathCombine(SiteManager.BaseAddress(SiteId), enclosure.Url);
                            ltlAttachmentTitle.Text = enclosure.Tittle;
                        }
                    }

                    //增加访问量
                    try
                    {
                        postManager.IncPostReadCount(uri);
                    }
                    catch
                    {
                        //不予处理
                    }

                    CategoryCollection categories = post.Categories;
                    if (categories.Count == 0)
                    {
                        return;
                    }
                    int categoryId = Converter.IntParse(Request.QueryString["catId"], 0);
                    if (categoryId == 0)
                    {
                        categoryId = categories[0].CategoryId;
                    }

                    //设置侧边栏及分类路径
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
                        if (category.ChildCategories != null && category.ChildCategories.Count > 0)
                        {
                            category = category.ChildCategories[0];
                        }
                        else
                        {
                            break;
                        }
                    }
                    ltlCategoryLink.Text = sb.ToString();
                }
            }
        }
    }
}
