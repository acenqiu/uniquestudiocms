using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.PlugIn;

namespace UniqueStudio.PlugInPictureNews
{
    public class PlugInPictureNews : Core.PlugIn.IPlugIn
    {
        private const string PLUGIN_NAME = "plugIn_PictureNews";
        private const string CATEGOEY_ID = "CategoryId";

        #region IPlugIn Members

        public void Register()
        {
            PostManager.OnPostPublished += new PostManager.PostPublishedEventHandler(PostManager_OnPostPublished);
        }

        public void UnRegister()
        {
            PostManager.OnPostPublished -= this.PostManager_OnPostPublished;
        }

        #endregion

        private void PostManager_OnPostPublished(object sender, PostEventArgs e)
        {
            if (e == null || e.Post == null)
            {
                return;
            }

            PostInfo post = e.Post;
            if (!PlugInManager.IsEnabled(PLUGIN_NAME, post.SiteId))
            {
                return;
            }

            int categoryId = Converter.IntParse(PlugInManager.GetConfigValue(PLUGIN_NAME, post.SiteId, CATEGOEY_ID), 0);

            if (!string.IsNullOrEmpty(post.NewsImage))
            {
                foreach (CategoryInfo item in post.Categories)
                {
                    if (item.CategoryId == categoryId)
                    {
                        PictureNewsManager.UpdatePictureNews();
                        break;
                    }
                }
            }
        }
    }
}
