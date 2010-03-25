using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using UniqueStudio.Core.Module;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.FileAccessHelper;

using UniqueStudio.ComContent.BLL;
using UniqueStudio.ComContent.Model;

namespace UniqueStudio.ModPostList
{
    public class ModPostList
    {
        private string controlId;

        //以下变量需删除
        private string basePath = @"E:\Projects\CMS\CMS\UniqueStudio.ComContent.PL\admin\";

        public ModPostList(string controlId)
        {
            this.controlId = controlId;
        }

        public string GetHtmlContent()
        {
            //StringBuilder template = new StringBuilder(FileAccess.ReadFile(basePath + @"mod_postlist\template.html"));

            ////应从UniqueStudio.BLL.Module.ModuleControlsManager获取
            //Dictionary<string, string> settings = GetControlSettings();

            //template.Replace("{CategoryName}", settings["CategoryName"]);

            //Regex rForeach = new Regex(@"<us:foreach[^/>]*>[\s\S]*?</us:foreach>");
            //Regex rForeachInside = new Regex(@"(?<=<us:foreach[^/>]*>)[\s\S]*?(?=</us:foreach>)");
            //string foreachInside = Regex.Match(template).Value;
            //StringBuilder sb = new StringBuilder();

            //PostManager manager = new PostManager();
            //PostCollection posts = manager.GetPostListByCategoryId(1, Convert.ToInt32(settings["number"]),
            //                                                                                    Convert.ToInt32(settings["categoryId"]));
            //foreach (PostInfo post in post)
            //{
            //    //string temp = 
            //}
            throw new NotImplementedException();
        }

        private Dictionary<string, string> GetControlSettings()
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            return settings;
        }
    }
}
