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
        /// 返回所有模块
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <returns>模块的集合</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看模块信息的权限时抛出该异常</exception>
        public ModuleCollection GetAllModules(UserInfo currentUser)
        {
            return provider.GetAllModules();
        }

        /// <summary>
        /// 返回指定模块
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="moduleId">模块ID</param>
        /// <returns>模块信息，如果获取失败，返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看模块信息的权限时抛出该异常</exception>
        public ModuleInfo GetModule(UserInfo currentUser, int moduleId)
        {
            if (!PermissionManager.HasPermission(currentUser, "ViewModuleInfo"))
            {
                throw new InvalidPermissionException("");
            }
            return provider.GetModule(moduleId);
        }

        /// <summary>
        /// 读取模块安装文件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="installFilePath">安装文件路径</param>
        /// <returns>模块信息</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装模块的权限时抛出该异常</exception>
        public ModuleInfo ReadInstallFile(UserInfo currentUser, string installFilePath)
        {
            XmlManager manager = new XmlManager();
            XmlDocument doc = manager.LoadXml(Path.Combine(GlobalConfig.BasePhysicalPath, installFilePath));
            ModuleInfo module = (ModuleInfo)manager.ConvertToEntity(doc, typeof(ModuleInfo), "/install/*");
            return module;
        }

        /// <summary>
        /// 安装模块
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="installFilePath">安装文件路径</param>
        /// <returns>如果安装成功则返回模块信息，否则返回null</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装模块的权限时抛出该异常</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.ModuleInstallException">
        /// 当用户安装失败时抛出该异常</exception>
        public ModuleInfo InstallModule(UserInfo currentUser, string installFilePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 安装模块
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="module">待安装模块信息</param>
        /// <param name="installFilePath">安装文件路径</param>
        /// <returns>如果安装成功则返回模块信息，否则返回null</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装模块的权限时抛出该异常</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.ModuleInstallException">
        /// 当用户安装失败时抛出该异常</exception>
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

        /// <summary>
        /// 卸载指定模块
        /// </summary>
        /// <remarks>可能在后续版本中修改该方法签名</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="module">待卸载模块信息</param>
        /// <returns>是否卸载成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载模块的权限时抛出该异常</exception>
        public bool UninstallModule(UserInfo currentUser, ModuleInfo module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 卸载多个模块
        /// </summary>
        /// <remarks>可能在后续版本中修改该方法签名</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="modules">待卸载模块的集合</param>
        /// <returns>是否卸载成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载模块的权限时抛出该异常</exception>
        public bool UninstallModules(UserInfo currentUser, ModuleCollection modules)
        {
            throw new NotImplementedException();
        }
    }
}
