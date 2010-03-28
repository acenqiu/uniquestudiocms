//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供插件管理的方法。
// 完成日期：2010年03月28日
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

namespace UniqueStudio.Core.PlugIn
{
    /// <summary>
    /// 提供插件管理的方法。
    /// </summary>
    public class PlugInManager
    {
        private static readonly DAL.IDAL.IPlugIn provider = DALFactory.CreatePlugIn();
        private static List<PlugInInstanceInfo> instances = new List<PlugInInstanceInfo>();

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
        /// 返回指定插件实例是否启用。
        /// </summary>
        /// <param name="plugInName">插件名称。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>是否启用。</returns>
        public static bool IsEnabled(string plugInName, int siteId)
        {
            Validator.CheckStringNull(plugInName, "plugInName");
            Validator.CheckNegative(siteId, "siteId");

            PlugInInstanceInfo instance = GetInstanceBasicInfo(plugInName, siteId);
            if (instance != null)
            {
                return instance.IsEnabled;
            }
            else
            {
                throw new PlugInNotFoundException();
            }
        }

        /// <summary>
        /// 返回指定插件实例指定配置项的值。
        /// </summary>
        /// <param name="plugInName">插件名称。</param>
        /// <param name="siteId">网站ID。</param>
        /// <param name="key">配置项名称。</param>
        /// <returns>值。</returns>
        public static string GetConfigValue(string plugInName, int siteId, string key)
        {
            Validator.CheckStringNull(plugInName, "plugInName");
            Validator.CheckNegative(siteId, "siteId");

            PlugInInstanceInfo instance = GetInstanceBasicInfo(plugInName, siteId);
            if (instance != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(instance.Config);
                XmlNode node = doc.SelectSingleNode(string.Format("//param[@name=\"{0}\"]", key));
                if (node != null)
                {
                    return node.Attributes["value"].Value;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new PlugInNotFoundException();
            }
        }

        private static PlugInInstanceInfo GetInstanceBasicInfo(string plugInName, int siteId)
        {
            for (int i = 0; i < instances.Count; i++)
            {
                if (instances[i].PlugInName == plugInName && instances[i].SiteId == siteId)
                {
                    return instances[i];
                }
            }

            //从数据库载入
            try
            {
                PlugInInstanceInfo instance = provider.GetInstanceBasicInfo(plugInName, siteId);
                if (instance != null)
                {
                    instances.Add(instance);
                    return instance;
                }
                else
                {
                    return null;
                }
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
                if (provider.SaveConfig(instanceId, config))
                {
                    for (int i = 0; i < instances.Count; i++)
                    {
                        if (instances[i].InstanceId == instanceId)
                        {
                            instances[i].Config = config;
                            break;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
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
            Validator.CheckNotPositive(instanceId, "instanceId");
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
        /// 卸载指定插件。
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
        /// 卸载指定插件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="plugInId">待卸载插件ID。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载插件的权限时抛出该异常。</exception>
        public bool UninstallPlugIn(UserInfo currentUser, int plugInId)
        {
            Validator.CheckNotPositive(plugInId, "plugInId");
            PermissionManager.CheckPermission(currentUser, "UninstallPlugIn", "卸载插件");

            try
            {
                ClassInfo plugIn = provider.GetClassInfo(plugInId);
                if (plugIn == null)
                {
                    throw new PlugInNotFoundException();
                }

                //反注册事件
                IPlugIn p = (IPlugIn)Assembly.Load(plugIn.Assembly).CreateInstance(plugIn.ClassPath);
                p.UnRegister();

                //删除插件
                if (provider.DeletePlugIn(plugInId))
                {
                    return true;
                }
                else
                {
                    p.Register();
                    return false;
                }
            }
            catch (PlugInNotFoundException)
            {
                throw;
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
        /// 卸载多个插件。
        /// </summary>
        /// <param name="plugInIds">待卸载插件ID的集合。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载插件的权限时抛出该异常。</exception>
        public bool UninstallPlugIns(int[] plugInIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用PlugInManager(UserInfo)实例化该类。");
            }
            return UninstallPlugIns(currentUser, plugInIds);
        }

        /// <summary>
        /// 卸载多个插件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="plugInIds">待卸载插件ID的集合。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载插件的权限时抛出该异常。</exception>
        public bool UninstallPlugIns(UserInfo currentUser, int[] plugInIds)
        {
            Validator.CheckNull(plugInIds, "plugInIds");
            foreach (int plugInId in plugInIds)
            {
                Validator.CheckNotPositive(plugInId, "plugInIds");
            }
            PermissionManager.CheckPermission(currentUser, "UninstallPlugIn", "卸载插件");

            try
            {
                List<IPlugIn> SucceedPlugIns = new List<IPlugIn>(plugInIds.Length);
                try
                {
                    foreach (int plugInId in plugInIds)
                    {
                        ClassInfo plugIn = provider.GetClassInfo(plugInId);
                        if (plugIn == null)
                        {
                            throw new PlugInNotFoundException();
                        }

                        //反注册事件
                        IPlugIn p = (IPlugIn)Assembly.Load(plugIn.Assembly).CreateInstance(plugIn.ClassPath);
                        p.UnRegister();

                        SucceedPlugIns.Add(p);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLogger.LogError(ex);
                    foreach (IPlugIn p in SucceedPlugIns)
                    {
                        p.Register();
                    }
                    return false;
                }

                //删除插件
                if (provider.DeletePlugIns(plugInIds))
                {
                    return true;
                }
                else
                {
                    foreach (IPlugIn p in SucceedPlugIns)
                    {
                        p.Register();
                    }
                    return false;
                }
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
