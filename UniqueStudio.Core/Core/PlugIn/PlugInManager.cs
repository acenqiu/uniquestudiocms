using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
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

namespace UniqueStudio.Core.PlugIn
{
    /// <summary>
    /// 提供插件管理的方法。
    /// </summary>
    public class PlugInManager
    {
        private static readonly DAL.IDAL.IPlugIn provider = DALFactory.CreatePlugIn();
        private UserInfo currentUser;

        /// <summary>
        /// 初始化<see cref="PlugInManager"/>类的实例。
        /// </summary>
        public PlugInManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以当前用户初始化<see cref="PlugInManager"/>类的实例。
        /// </summary>
        /// <param name="currentUser">当前用户。</param>
        public PlugInManager(UserInfo currentUser)
        {
            Validator.CheckNull(currentUser, "currentUser");
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 新增插件实例。
        /// </summary>
        /// <param name="plugInId">插件ID。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>是否增加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑插件的权限时抛出该异常。</exception>
        public bool AddPlugInInstance(int plugInId, int siteId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return AddPlugInInstance(currentUser, plugInId, siteId);
        }

        /// <summary>
        /// 新增插件实例。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="plugInId">插件ID。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>是否增加成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑插件的权限时抛出该异常。</exception>
        public bool AddPlugInInstance(UserInfo currentUser, int plugInId, int siteId)
        {
            Validator.CheckNotPositive(plugInId, "plugInId");
            Validator.CheckNegative(siteId, "siteId");
            PermissionManager.CheckPermission(currentUser, "EditPlugIn", "编辑插件");

            try
            {
                return provider.AddPlugInInstance(plugInId, siteId);
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
        /// 删除插件实例。
        /// </summary>
        /// <param name="instanceId">待删除插件实例ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑插件的权限时抛出该异常。</exception>
        public bool DeletePlugInInstance(int instanceId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return DeletePlugInInstance(currentUser, instanceId);
        }

        /// <summary>
        /// 删除插件实例。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="instanceId">待删除插件实例ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑插件的权限时抛出该异常。</exception>
        public bool DeletePlugInInstance(UserInfo currentUser, int instanceId)
        {
            Validator.CheckNotPositive(instanceId, "instanceId");
            PermissionManager.CheckPermission(currentUser, "EditPlugIn", "编辑插件");

            try
            {
                return provider.DeletePlugInInstance(instanceId);
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
        /// 删除插件实例。
        /// </summary>
        /// <param name="instanceIds">待删除插件实例ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑插件的权限时抛出该异常。</exception>
        public bool DeletePlugInInstances(int[] instanceIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return DeletePlugInInstances(currentUser, instanceIds);
        }

        /// <summary>
        /// 删除插件实例。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="instanceIds">待删除插件实例ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑插件的权限时抛出该异常。</exception>
        public bool DeletePlugInInstances(UserInfo currentUser, int[] instanceIds)
        {
            foreach (int instanceId in instanceIds)
            {
                Validator.CheckNotPositive(instanceId, "instanceIds");
            }
            PermissionManager.CheckPermission(currentUser, "EditPlugIn", "编辑插件");

            try
            {
                return provider.DeletePlugInInstances(instanceIds);
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
        /// 返回插件列表。
        /// </summary>
        /// <returns>插件列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看插件信息的权限时抛出该异常。</exception>
        public PlugInCollection GetAllPlugIns()
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return GetAllPlugIns(currentUser);
        }

        /// <summary>
        /// 返回插件列表。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <returns>插件列表。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看插件信息的权限时抛出该异常。</exception>
        public PlugInCollection GetAllPlugIns(UserInfo currentUser)
        {
            PermissionManager.CheckPermission(currentUser, "ViewPlugInInfo", "查看插件信息");

            try
            {
                return provider.GetAllPlugIns();
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
        /// 返回插件列表。
        /// </summary>
        /// <remarks>该方法只在程序系统启动时调用，用于完成插件的初始化工作。</remarks>
        /// <returns>类集合。</returns>
        public ClassCollection GetAllPlugInsForInit()
        {
            try
            {
                return provider.GetAllPlugInsForInit();
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
        /// 返回指定插件的信息。
        /// </summary>
        /// <param name="plugInId">插件ID。</param>
        /// <returns>插件信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看插件信息的权限时抛出该异常。</exception>
        public PlugInInfo GetPlugIn(int plugInId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return GetPlugIn(currentUser, plugInId);
        }

        /// <summary>
        /// 返回指定插件的信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="plugInId">插件ID。</param>
        /// <returns>插件信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看插件信息的权限时抛出该异常。</exception>
        public PlugInInfo GetPlugIn(UserInfo currentUser, int plugInId)
        {
            Validator.CheckNotPositive(plugInId, "plugInId");
            PermissionManager.CheckPermission(currentUser, "ViewPlugInInfo", "查看插件信息");

            try
            {
                return provider.GetPlugIn(plugInId);
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
        /// 安装插件。
        /// </summary>
        /// <param name="workingPath">安装文件路径。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>是否安装成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装插件的权限时抛出该异常。</exception>
        public bool InstallPlugIn(string workingPath, int siteId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return InstallPlugIn(currentUser, workingPath, siteId);
        }

        /// <summary>
        /// 安装插件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="workingPath">安装文件路径。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>是否安装成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装插件的权限时抛出该异常。</exception>
        public bool InstallPlugIn(UserInfo currentUser, string workingPath, int siteId)
        {
            Validator.CheckStringNull(workingPath, "workingPath");
            Validator.CheckNegative(siteId, "siteId");
            PermissionManager.CheckPermission(currentUser, "InstallPlugIn", "安装插件");

            string path = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, workingPath);
            path = PathHelper.PathCombine(path, "install.xml");

            XmlManager manager = new XmlManager();
            XmlDocument doc = manager.LoadXml(path);
            PlugInInfo plugIn = (PlugInInfo)manager.ConvertToEntity(doc, typeof(PlugInInfo), "/install/*");
            if (plugIn == null)
            {
                return false;
            }
            else
            {
                plugIn.Config = manager.ConstructSubXmlDocument(doc, "/install/params", "config").OuterXml;
                plugIn.WorkingPath = workingPath;
                plugIn.Instances = new List<PlugInInstanceInfo>();
                PlugInInstanceInfo instance = new PlugInInstanceInfo();
                instance.SiteId = siteId;
                instance.IsEnabled = true;
                plugIn.Instances.Add(instance);

                plugIn = provider.CreatePlugIn(plugIn);
                if (plugIn != null)
                {
                    try
                    {
                        IPlugIn p = (IPlugIn)Assembly.Load(plugIn.Assembly).CreateInstance(plugIn.ClassPath);
                        p.Register();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.LogError(ex);
                        throw new Exception("插件启用失败！");
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取指定插件实例的配置信息。
        /// </summary>
        /// <param name="instanceId">实例ID。</param>
        /// <returns>配置信息（xml格式）</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑插件的权限时抛出该异常。</exception>
        public string LoadConfig(int instanceId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return LoadConfig(currentUser, instanceId);
        }

        /// <summary>
        /// 获取指定插件实例的配置信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="instanceId">实例ID。</param>
        /// <returns>配置信息（xml格式）</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑插件的权限时抛出该异常。</exception>
        public string LoadConfig(UserInfo currentUser, int instanceId)
        {
            Validator.CheckNotPositive(instanceId, "instanceId");
            PermissionManager.CheckPermission(currentUser, "EditPlugIn", "编辑插件");

            try
            {
                return provider.LoadConfig(instanceId);
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
        /// 读取插件安装文件。
        /// </summary>
        /// <param name="workingPath">安装文件路径。</param>
        /// <returns>待安装插件信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装插件的权限时抛出该异常。</exception>
        public PlugInInfo ReadInstallFile(string workingPath)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return ReadInstallFile(currentUser, workingPath);
        }

        /// <summary>
        /// 读取插件安装文件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="workingPath">安装文件路径。</param>
        /// <returns>待安装插件信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装插件的权限时抛出该异常。</exception>
        public PlugInInfo ReadInstallFile(UserInfo currentUser, string workingPath)
        {
            Validator.CheckStringNull(workingPath, "workingPath");
            PermissionManager.CheckPermission(currentUser, "InstallPlugIn", "安装插件");

            string path = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, workingPath);
            path = PathHelper.PathCombine(path, "install.xml");

            XmlManager manager = new XmlManager();
            XmlDocument doc = manager.LoadXml(path);
            PlugInInfo plugin = (PlugInInfo)manager.ConvertToEntity(doc, typeof(PlugInInfo), "/install/*");
            plugin.WorkingPath = workingPath;
            return plugin;
        }
        
        /// <summary>
        /// 保存插件实例配置信息。
        /// </summary>
        /// <param name="instanceId">插件实例ID。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否保存成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑插件的权限时抛出该异常。</exception>
        public bool SaveConfig(int instanceId, string config)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return SaveConfig(currentUser, instanceId, config);
        }

        /// <summary>
        /// 保存插件实例配置信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="instanceId">插件实例ID。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否保存成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑插件的权限时抛出该异常。</exception>
        public bool SaveConfig(UserInfo currentUser, int instanceId, string config)
        {
            Validator.CheckNotPositive(instanceId, "instanceId");
            Validator.CheckStringNull(config, "config");

            try
            {
                return provider.SaveConfig(instanceId, config);
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
        /// 启用指定插件。
        /// </summary>
        /// <param name="instanceId">待启用插件实例ID。</param>
        /// <returns>是否启用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有启用插件的权限时抛出该异常。</exception>
        public bool StartPlugIn(int instanceId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return StartPlugIn(currentUser, instanceId);
        }

        /// <summary>
        /// 启用指定插件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="instanceId">待启用插件实例ID。</param>
        /// <returns>是否启用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有启用插件的权限时抛出该异常。</exception>
        public bool StartPlugIn(UserInfo currentUser, int instanceId)
        {
            Validator.CheckNotPositive(instanceId, "instanceId");
            PermissionManager.CheckPermission(currentUser, "StartPlugIn", "启用插件");

            try
            {
                return provider.StartPlugIn(instanceId);
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
        /// 启用多个插件。
        /// </summary>
        /// <param name="instanceIds">待启用插件实例ID的集合。</param>
        /// <returns>是否启用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有启用插件的权限时抛出该异常。</exception>
        public bool StartPlugIns(int[] instanceIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return StartPlugIns(currentUser, instanceIds);
        }

        /// <summary>
        /// 启用多个插件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="instanceIds">待启用插件实例ID的集合。</param>
        /// <returns>是否启用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有启用插件的权限时抛出该异常。</exception>
        public bool StartPlugIns(UserInfo currentUser, int[] instanceIds)
        {
            Validator.CheckNull(instanceIds, "instanceIds");
            foreach (int instanceId in instanceIds)
            {
                Validator.CheckNotPositive(instanceId, "instanceIds");
            }
            PermissionManager.CheckPermission(currentUser, "StartPlugIn", "启用插件");

            try
            {
                return provider.StartPlugIns(instanceIds);
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
        /// 停用指定插件。
        /// </summary>
        /// <param name="instanceId">待停用插件实例ID。</param>
        /// <returns>是否停用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有停用插件的权限时抛出该异常。</exception>
        public bool StopPlugIn(int instanceId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return StopPlugIn(currentUser, instanceId);
        }

        /// <summary>
        /// 停用指定插件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="instanceId">待停用插件实例ID。</param>
        /// <returns>是否停用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有停用插件的权限时抛出该异常。</exception>
        public bool StopPlugIn(UserInfo currentUser, int instanceId)
        {
            throw new NotImplementedException(); Validator.CheckNotPositive(instanceId, "instanceId");
            PermissionManager.CheckPermission(currentUser, "StopPlugIn", "停用插件");

            try
            {
                return provider.StopPlugIn(instanceId);
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
        /// 停用多个插件。
        /// </summary>
        /// <param name="instanceIds">待停用插件实例ID的集合。</param>
        /// <returns>是否停用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有停用插件的权限时抛出该异常。</exception>
        public bool StopPlugIns(int[] instanceIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return StopPlugIns(currentUser, instanceIds);
        }

        /// <summary>
        /// 停用多个插件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="instanceIds">待停用插件实例ID的集合。</param>
        /// <returns>是否停用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有停用插件的权限时抛出该异常。</exception>
        public bool StopPlugIns(UserInfo currentUser, int[] instanceIds)
        {
            Validator.CheckNull(instanceIds, "instanceIds");
            foreach (int instanceId in instanceIds)
            {
                Validator.CheckNotPositive(instanceId, "instanceIds");
            }
            PermissionManager.CheckPermission(currentUser, "StopPlugIn", "停用插件");

            try
            {
                return provider.StopPlugIns(instanceIds);
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
        /// 卸载指定插件
        /// </summary>
        /// <param name="plugInId">待卸载插件ID。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载插件的权限时抛出该异常。</exception>
        public bool UninstallPlugIn(int plugInId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return UninstallPlugIn(currentUser, plugInId);
        }

        /// <summary>
        /// 卸载指定插件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="plugInId">待卸载插件ID。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载插件的权限时抛出该异常。</exception>
        public bool UninstallPlugIn(UserInfo currentUser, int plugInId)
        {
            Validator.CheckNotPositive(plugInId, "plugInId");
            PermissionManager.CheckPermission(currentUser, "", "");

            throw new NotImplementedException();
        }

        /// <summary>
        /// 卸载多个插件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="plugIns">待卸载插件的集合</param>
        /// <returns>是否卸载成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载插件的权限时抛出该异常</exception>
        public bool UninstallPlugIns(UserInfo currentUser, PlugInCollection plugIns)
        {
            throw new NotImplementedException();
        }
    }
}
