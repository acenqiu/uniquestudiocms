using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Module;
using UniqueStudio.Core.Menu;

namespace UniqueStudio.ModFriendlyLink
{
    public class ModFriendlyLink : IModule
    {
        private const string MENU_ID = "MenuId";
        //{0}：条目
        private const string MAIN = "<div class=\"column-head\"><span>友情链接</span></div>\r\n"
                                                        + "<div class=\"column-content\">\r\n<ul>\r\n{0}</ul>\r\n</div>";
        private const string ITEM = "<li><a href='{0}' title='{1}' target='{1}'>{2}</a></li>\r\n";

        #region IModule Members

        public string RenderContents(int siteId, string controlName, NameValueCollection queryString)
        {
            try
            {
                int menuId = Converter.IntParse(ModuleControlManager.GetConfigValue(siteId, controlName, MENU_ID), 0);
                StringBuilder items = new StringBuilder();
                MenuItemCollection links = (new MenuManager()).GetMenuItems(menuId);
                if (links != null)
                {
                    foreach (MenuItemInfo link in links)
                    {
                        items.Append(string.Format(ITEM, link.Link,
                                                                            link.Target,
                                                                            link.ItemName));
                    }
                }
                return string.Format(MAIN, items.ToString());
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        public ModFriendlyLink()
        {

        }
    }
}
