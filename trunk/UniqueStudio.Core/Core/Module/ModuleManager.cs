using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using UniqueStudio.Core.Permission;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.Core.Module
{
    /// <summary>
    /// 提供模块管理的方法
    /// </summary>
    public class ModuleManager
    {
        private static readonly IModule provider = DALFactory.CreateModule();

        /// <summary>
        /// 初始化<see cref="ModuleManager"/>类的实例
        /// </summary>
        public ModuleManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ModuleCollection GetAllModules()
        {
            return provider.GetAllModules();
        }

        public ModuleInfo GetModule(UserInfo currentUser, int moduleId)
        {
            if (!PermissionManager.HasPermission(currentUser, "ViewModuleInfo"))
            {
                throw new InvalidPermissionException("");
            }
            return provider.GetModule(moduleId);
        }

        public ModuleInfo ReadInstallFile(UserInfo currentUser, string installFilePath)
        {
            XmlManager manager = new XmlManager();
            XmlDocument doc = manager.LoadXml(Path.Combine(GlobalConfig.BasePhysicalPath, installFilePath));
            ModuleInfo module = (ModuleInfo)manager.ConvertToEntity(doc, typeof(ModuleInfo), "/install/*");
            return module;
        }

        public ModuleInfo InstallModule(UserInfo currentUser, string installFilePath)
        {
            throw new NotImplementedException();
        }

        public ModuleInfo InstallModule(UserInfo currentUser, ModuleInfo module, string installFilePath)
        {
            if (!PermissionManager.HasPermission(currentUser,"InstallModule"))
            {
                throw new InvalidPermissionException("");
            }

            XmlManager manager = new XmlManager();
            XmlDocument doc = manager.ConstructSubXmlDocument(Path.Combine(GlobalConfig.BasePhysicalPath, installFilePath), "/install/params", "config");
            module.Parameters = doc.OuterXml;

            return provider.CreateModule(module);
        }

        public bool UninstallModule(ModuleInfo module)
        {
            throw new NotImplementedException();
        }

        public bool UninstallModules(ModuleCollection modules)
        {
            throw new NotImplementedException();
        }
    }
}
