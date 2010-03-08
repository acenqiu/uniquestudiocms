using System.Reflection;

using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.DAL
{
    internal sealed class DALFactory
    {
        private static string databasePreFix = "Sql";
        private static string basePath = "UniqueStudio.DAL";

        internal static ICategory CreateCategory()
        {
            string className = string.Format("{0}.Category.{1}CategoryProvider", basePath, databasePreFix);
            return (ICategory)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static ICompenent CreateCompenent()
        {
            string className = string.Format("{0}.Compenent.{1}CompenentProvider", basePath, databasePreFix);
            return (ICompenent)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static IEngine CreateEngine()
        {
            string className = string.Format("{0}.Engine.{1}EngineProvider", basePath, databasePreFix);
            return (IEngine)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static IMenu CreateMenu()
        {
            string className = string.Format("{0}.Menu.{1}MenuProvider", basePath, databasePreFix);
            return (IMenu)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static IModule CreateModule()
        {
            string className = string.Format("{0}.Module.{1}ModuleProvider", basePath, databasePreFix);
            return (IModule)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static IModuleControl CreateModuleControl()
        {
            string className = string.Format("{0}.Module.{1}ModuleControlProvider", basePath, databasePreFix);
            return (IModuleControl)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static IPageVisit CreatePageVisit()
        {
            string className = string.Format("{0}.PageVisit.{1}PageVisitProvider", basePath, databasePreFix);
            return (IPageVisit)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static IPermission CreatePermission()
        {
            string className = string.Format("{0}.Permission.{1}PermissionProvider", basePath, databasePreFix);
            return (IPermission)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static IPlugIn CreatePlugIn()
        {
            string className = string.Format("{0}.PlugIn.{1}PlugInProvider", basePath, databasePreFix);
            return (IPlugIn)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static IRole CreateRole()
        {
            string className = string.Format("{0}.Permission.{1}RoleProvider", basePath, databasePreFix);
            return (IRole)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static ISiteMap CreateSiteMap()
        {
            string className = string.Format("{0}.SiteMap.{1}SiteMapProvider", basePath, databasePreFix);
            return (ISiteMap)Assembly.GetExecutingAssembly().CreateInstance(className);
        }

        internal static IUser CreateUser()
        {
            string className = string.Format("{0}.User.{1}UserProvider", basePath, databasePreFix);
            return (IUser)Assembly.GetExecutingAssembly().CreateInstance(className);
        }
    }
}
