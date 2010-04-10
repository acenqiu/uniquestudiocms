//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供模块管理的方法。
// 完成日期：2010年03月31日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Xml;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;

namespace UniqueStudio.Core.Module
{
    /// <summary>
    /// 提供模块管理的方法。
    /// </summary>
    public class ModuleManager
    {
        private static readonly DAL.IDAL.IModule provider = DALFactory.CreateModule();
        private static Dictionary<string, IModule> dicModules = new Dictionary<string, IModule>();

        private UserInfo currentUser;

        /// <summary>
        /// 初始化<see cref="ModuleManager"/>类的实例。
        /// </summary>
        public ModuleManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以当前用户初始化<see cref="ModuleManager"/>类的实例。
        /// </summary>
        /// <param name="currentUser">当前用户。</param>
        public ModuleManager(UserInfo currentUser)
        {
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 返回模块实例。
        /// </summary>
        /// <param name="moduleName">模块名。</param>
        /// <returns>模块实例。</returns>
        public static IModule GetInstance(string moduleName)
        {
            Validator.CheckStringNull(moduleName, "moduleName");

            if (dicModules.ContainsKey(moduleName))
            {
                return dicModules[moduleName];
            }
            else
            {
                try
                {
                    ModuleInfo module = provider.GetModule(moduleName);
                    if (module != null)
                    {
                        IModule instance = (IModule)Assembly.Load(module.Assembly).CreateInstance(module.ClassPath);
                        if (instance != null)
                        {
                            dicModules.Add(moduleName, instance);
                            return dicModules[moduleName];
                        }
                    }
                    return null;
                }
                catch (DbException ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new DatabaseException();
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new UnhandledException();
                }
            }
        }

        /// <summary>
        /// 返回所有模块。
        /// </summary>
        /// <returns>模块的集合。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看模块信息的权限时抛出该异常。</exception>
        public ModuleCollection GetAllModules()
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleManager(UserInfo)实例化该类。");
            }
            return GetAllModules(currentUser);
        }

        /// <summary>
        /// 返回所有模块。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <returns>模块的集合。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看模块信息的权限时抛出该异常。</exception>
        public ModuleCollection GetAllModules(UserInfo currentUser)
        {
            PermissionManager.CheckPermission(currentUser, "ViewModuleInfo", "查看模块信息");

            try
            {
                return provider.GetAllModules();
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回指定模块。
        /// </summary>
        /// <param name="moduleId">模块ID。</param>
        /// <returns>模块信息，如果获取失败，返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看模块信息的权限时抛出该异常。</exception>
        public ModuleInfo GetModule(int moduleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleManager(UserInfo)实例化该类。");
            }
            return GetModule(currentUser, moduleId);
        }

        /// <summary>
        /// 返回指定模块。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="moduleId">模块ID。</param>
        /// <returns>模块信息，如果获取失败，返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看模块信息的权限时抛出该异常。</exception>
        public ModuleInfo GetModule(UserInfo currentUser, int moduleId)
        {
            Validator.CheckNotPositive(moduleId, "moduleId");
            PermissionManager.CheckPermission(currentUser, "ViewModuleInfo", "查看模块信息");

            try
            {
                return provider.GetModule(moduleId);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 返回指定模块。
        /// </summary>
        /// <param name="moduleName">模块名称。</param>
        /// <returns>模块信息，如果获取失败，返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看模块信息的权限时抛出该异常。</exception>
        public ModuleInfo GetModule(string moduleName)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleManager(UserInfo)实例化该类。");
            }
            return GetModule(currentUser, moduleName);
        }

        /// <summary>
        /// 返回指定模块。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="moduleName">模块名称。</param>
        /// <returns>模块信息，如果获取失败，返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看模块信息的权限时抛出该异常。</exception>
        public ModuleInfo GetModule(UserInfo currentUser, string moduleName)
        {
            Validator.CheckStringNull(moduleName, "moduleName");
            PermissionManager.CheckPermission(currentUser, "ViewModuleInfo", "查看模块信息");

            try
            {
                return provider.GetModule(moduleName);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 安装模块。
        /// </summary>
        /// <param name="workingPath">模块工作路径。</param>
        /// <returns>是否安装成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装模块的权限时抛出该异常。</exception>
        public bool InstallModule(string workingPath)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleManager(UserInfo)实例化该类。");
            }
            return InstallModule(currentUser, workingPath);
        }

        /// <summary>
        /// 安装模块。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="workingPath">模块工作路径。</param>
        /// <returns>是否安装成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装模块的权限时抛出该异常。</exception>
        public bool InstallModule(UserInfo currentUser, string workingPath)
        {
            Validator.CheckStringNull(workingPath, "workingPath");
            PermissionManager.CheckPermission(currentUser, "InstallModule", "安装模块");

            string path = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, workingPath);
            path = PathHelper.PathCombine(path, "install.xml");

            XmlManager manager = new XmlManager(path);
            ModuleInfo module = (ModuleInfo)manager.ConvertToEntity(typeof(ModuleInfo), "/install/*");
            if (module == null)
            {
                return false;
            }
            else
            {
                if (IsModuleExists(module.ModuleName))
                {
                    throw new Exception("该模块已经安装过了。");
                }

                XmlDocument subDoc = manager.SubXmlDocument("/install/params", "config");
                if (subDoc!=null)
                {
                    module.Config = subDoc.OuterXml;
                }
                module.WorkingPath = workingPath;

                try
                {
                    return (provider.CreateModule(module) != null);
                }
                catch (DbException ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new DatabaseException();
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex);
                    throw new UnhandledException();
                }
            }
        }

        /// <summary>
        /// 返回指定模块是否存在。
        /// </summary>
        /// <param name="moduleName">模块名。</param>
        /// <returns>是否存在。</returns>
        public bool IsModuleExists(string moduleName)
        {
            Validator.CheckStringNull(moduleName, "moduleName");

            try
            {
                return provider.IsModuleExists(moduleName);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 读取模块安装文件。
        /// </summary>
        /// <param name="workingPath">模块工作路径。</param>
        /// <returns>模块信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装模块的权限时抛出该异常。</exception>
        public ModuleInfo ReadInstallFile(string workingPath)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleManager(UserInfo)实例化该类。");
            }
            return ReadInstallFile(currentUser, workingPath);
        }

        /// <summary>
        /// 读取模块安装文件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="workingPath">模块工作路径。</param>
        /// <returns>模块信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装模块的权限时抛出该异常。</exception>
        public ModuleInfo ReadInstallFile(UserInfo currentUser, string workingPath)
        {
            Validator.CheckStringNull(workingPath, "workingPath");
            PermissionManager.CheckPermission(currentUser, "InstallModule", "安装模块");

            string path = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, workingPath);
            path = PathHelper.PathCombine(path, "install.xml");

            XmlManager manager = new XmlManager(path);
            return (ModuleInfo)manager.ConvertToEntity(typeof(ModuleInfo), "/install/*");
        }

        /// <summary>
        /// 卸载指定模块
        /// </summary>。
        /// <param name="moduleId">待卸载模块ID。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载模块的权限时抛出该异常。</exception>
        public bool UninstallModule(int moduleId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleManager(UserInfo)实例化该类。");
            }
            return UninstallModule(currentUser, moduleId);
        }

        /// <summary>
        /// 卸载指定模块。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="moduleId">待卸载模块ID。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载模块的权限时抛出该异常。</exception>
        public bool UninstallModule(UserInfo currentUser, int moduleId)
        {
            Validator.CheckNotPositive(moduleId, "moduleId");

            try
            {
                string moduleName = provider.GetModuleName(moduleId);
                if (moduleName != null)
                {
                    if (provider.DeleteModule(moduleId))
                    {
                        if (dicModules.ContainsKey(moduleName))
                        {
                            dicModules.Remove(moduleName);
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }

        /// <summary>
        /// 卸载多个模块。
        /// </summary>
        /// <param name="moduleIds">待卸载模块ID的集合。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载模块的权限时抛出该异常。</exception>
        public bool UninstallModules(int[] moduleIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleManager(UserInfo)实例化该类。");
            }
            return UninstallModules(currentUser, moduleIds);
        }

        /// <summary>
        /// 卸载多个模块。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="moduleIds">待卸载模块ID的集合。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载模块的权限时抛出该异常。</exception>
        public bool UninstallModules(UserInfo currentUser, int[] moduleIds)
        {
            Validator.CheckNull(moduleIds, "moduleIds");
            foreach (int moduleId in moduleIds)
            {
                Validator.CheckNotPositive(moduleId, "moduleIds");
            }

            try
            {
                foreach (int moduleId in moduleIds)
                {
                    string moduleName = provider.GetModuleName(moduleId);
                    if (dicModules.ContainsKey(moduleName))
                    {
                        dicModules.Remove(moduleName);
                    }
                }

                return provider.DeleteModules(moduleIds);
            }
            catch (DbException ex)
            {
                ErrorLogger.LogError(ex);
                throw new DatabaseException();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new UnhandledException();
            }
        }
    }
}
