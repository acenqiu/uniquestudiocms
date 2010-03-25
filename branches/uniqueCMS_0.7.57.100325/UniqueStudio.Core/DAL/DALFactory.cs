//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：数据访问层工厂类。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System.Reflection;

using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL
{
    /// <summary>
    /// 数据访问层工厂类。
    /// </summary>
    internal sealed class DALFactory
    {
        private static string databasePreFix = "Sql";
        private static string basePath = "UniqueStudio.DAL";

        /// <summary>
        /// 创建分类管理提供类的实例。
        /// </summary>
        /// <returns>分类管理提供类接口。</returns>
        internal static ICategory CreateCategory()
        {
            string className = string.Format("{0}.Category.{1}CategoryProvider", basePath, databasePreFix);
            return (ICategory)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建组件管理提供类的实例。
        /// </summary>
        /// <returns>组件管理提供类接口。</returns>
        internal static ICompenent CreateCompenent()
        {
            string className = string.Format("{0}.Compenent.{1}CompenentProvider", basePath, databasePreFix);
            return (ICompenent)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建页面解析引擎提供类的实例。
        /// </summary>
        /// <returns>页面解析引擎提供类接口。</returns>
        internal static IEngine CreateEngine()
        {
            string className = string.Format("{0}.Engine.{1}EngineProvider", basePath, databasePreFix);
            return (IEngine)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建菜单管理提供类的实例。
        /// </summary>
        /// <returns>菜单管理提供类接口。</returns>
        internal static IMenu CreateMenu()
        {
            string className = string.Format("{0}.Menu.{1}MenuProvider", basePath, databasePreFix);
            return (IMenu)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建模块管理提供类的实例。
        /// </summary>
        /// <returns>模块管理提供类接口。</returns>
        internal static IModule CreateModule()
        {
            string className = string.Format("{0}.Module.{1}ModuleProvider", basePath, databasePreFix);
            return (IModule)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建模块控件管理提供类的实例。
        /// </summary>
        /// <returns>模块控件管理提供类接口。</returns>
        internal static IModuleControl CreateModuleControl()
        {
            string className = string.Format("{0}.Module.{1}ModuleControlProvider", basePath, databasePreFix);
            return (IModuleControl)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建页面访问管理提供类的实例。
        /// </summary>
        /// <returns>页面访问管理提供类接口。</returns>
        internal static IPageVisit CreatePageVisit()
        {
            string className = string.Format("{0}.PageVisit.{1}PageVisitProvider", basePath, databasePreFix);
            return (IPageVisit)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建权限管理提供类的实例。
        /// </summary>
        /// <returns>权限管理提供类接口。</returns>
        internal static IPermission CreatePermission()
        {
            string className = string.Format("{0}.Permission.{1}PermissionProvider", basePath, databasePreFix);
            return (IPermission)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建插件管理提供类的实例。
        /// </summary>
        /// <returns>插件管理提供类接口。</returns>
        internal static IPlugIn CreatePlugIn()
        {
            string className = string.Format("{0}.PlugIn.{1}PlugInProvider", basePath, databasePreFix);
            return (IPlugIn)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建角色管理提供类的实例。
        /// </summary>
        /// <returns>角色管理提供类接口。</returns>
        internal static IRole CreateRole()
        {
            string className = string.Format("{0}.Permission.{1}RoleProvider", basePath, databasePreFix);
            return (IRole)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建网站管理提供类的实例。
        /// </summary>
        /// <returns>网站管理提供类接口。</returns>
        internal static ISite CreateSite()
        {
            string className = string.Format("{0}.Site.{1}SiteProvider", basePath, databasePreFix);
            return (ISite)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建网站地图管理提供类的实例。
        /// </summary>
        /// <returns>网站地图管理提供类接口。</returns>
        internal static ISiteMap CreateSiteMap()
        {
            string className = string.Format("{0}.SiteMap.{1}SiteMapProvider", basePath, databasePreFix);
            return (ISiteMap)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        /// <summary>
        /// 创建用户管理提供类的实例。
        /// </summary>
        /// <returns>用户管理提供类接口。</returns>
        internal static IUser CreateUser()
        {
            string className = string.Format("{0}.User.{1}UserProvider", basePath, databasePreFix);
            return (IUser)Assembly.GetExecutingAssembly().CreateInstance(className);
        }
    }
}
