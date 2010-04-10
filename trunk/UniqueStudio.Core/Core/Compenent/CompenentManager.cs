//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供组件管理的方法。
// 完成日期：2010年04月02日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;
using System.Data.Common;

namespace UniqueStudio.Core.Compenent
{
    /// <summary>
    /// 提供组件管理的方法。
    /// </summary>
    public class CompenentManager
    {
        private static readonly ICompenent provider = DALFactory.CreateCompenent();

        private static Dictionary<int, SystemConfig> dicConfigs = new Dictionary<int, SystemConfig>();
        private static Dictionary<int, Dictionary<string, int>> compenentIdMapping = new Dictionary<int, Dictionary<string, int>>();

        private UserInfo currentUser;

        /// <summary>
        /// 初始化<see cref="CompenentManager"/>类的实例。
        /// </summary>
        public CompenentManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以当前用户初始化<see cref="CompenentManager"/>类的实例。
        /// </summary>
        /// <param name="currentUser">当前用户。</param>
        public CompenentManager(UserInfo currentUser)
        {
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 返回指定组件的配置信息。
        /// </summary>
        /// <remarks>当缓存中没有该组件的配置信息时，将使用组件配置类进行初始化。</remarks>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <param name="compenentConfig">组件配置类。</param>
        /// <returns>组件实体类。</returns>
        public static SystemConfig Config(int siteId, string compenentName,SystemConfig compenentConfig)
        {
            int compenentId = GetCompenentId(siteId, compenentName);
            if (compenentId == 0)
            {
                return null;
            }

            if (dicConfigs.ContainsKey(compenentId))
            {
                return dicConfigs[compenentId];
            }
            else
            {
                string config = provider.GetCompenentConfig(compenentId);
                if (string.IsNullOrEmpty(config))
                {
                    return null;
                }
                else
                {
                    compenentConfig.LoadConfig(config);
                    dicConfigs.Add(compenentId, compenentConfig);
                    return compenentConfig;
                }
            }
        }

        /// <summary>
        /// 根据网站ID及组件名返回组件ID。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <returns>组件ID。</returns>
        public static int GetCompenentId(int siteId, string compenentName)
        {
            if (!compenentIdMapping.ContainsKey(siteId))
            {
                compenentIdMapping.Clear();
                CompenentCollection compenents = provider.GetAllCompenents();
                foreach (CompenentInfo compenent in compenents)
                {
                    if (!compenentIdMapping.ContainsKey(compenent.SiteId))
                    {
                        compenentIdMapping.Add(compenent.SiteId, new Dictionary<string, int>());
                    }
                    compenentIdMapping[compenent.SiteId].Add(compenent.CompenentName, compenent.CompenentId);
                }
            }

            if (compenentIdMapping.ContainsKey(siteId))
            {
                if (compenentIdMapping[siteId].ContainsKey(compenentName))
                {
                    return compenentIdMapping[siteId][compenentName];
                }
            }
            return 0;
        }

        /// <summary>
        /// 返回所有组件的信息。
        /// </summary>
        /// <returns>组件的集合。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看组件信息的权限时抛出该异常</exception>
        public CompenentCollection GetAllCompenents()
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CompenentManager(UserInfo)实例化该类。");
            }
            return GetAllCompenents(currentUser);
        }

        /// <summary>
        /// 返回所有组件的信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <returns>组件的集合。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看组件信息的权限时抛出该异常</exception>
        public CompenentCollection GetAllCompenents(UserInfo currentUser)
        {
            PermissionManager.CheckPermission(currentUser, "ViewCompenentInfo", "查看组件信息");

            try
            {
                return provider.GetAllCompenents();
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
        /// 安装组件。
        /// </summary>
        /// <param name="workingPath">组件工作路径。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>是否安装成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装组件的权限时抛出该异常</exception>
        public bool InstallCompenent(string workingPath, int siteId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CompenentManager(UserInfo)实例化该类。");
            }
            return InstallCompenent(currentUser, workingPath, siteId);
        }

        /// <summary>
        /// 安装组件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="workingPath">组件工作路径。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>是否安装成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装组件的权限时抛出该异常</exception>
        public bool InstallCompenent(UserInfo currentUser, string workingPath, int siteId)
        {
            Validator.CheckStringNull(workingPath, "workingPath");
            Validator.CheckNotPositive(siteId, "siteId");
            PermissionManager.CheckPermission(currentUser, "InstallCompenent", "安装组件");

            string path = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, workingPath);
            path = PathHelper.PathCombine(path, "install.xml");

            XmlDocument doc = XmlManager.LoadXml(path);
            CompenentInfo compenent = (CompenentInfo)XmlManager.ConvertToEntity(doc, typeof(CompenentInfo), "/install/*");
            if (compenent == null)
            {
                return false;
            }
            else
            {
                if (IsCompenentExists(siteId, compenent.CompenentName))
                {
                    throw new Exception("该组件已经安装过了。");
                }

                compenent.SiteId = siteId;
                compenent.WorkingPath = workingPath;
                XmlDocument subDoc = XmlManager.SubXmlDocument(doc, "/install/params", "config");
                if (subDoc != null)
                {
                    compenent.Config = subDoc.OuterXml;
                }

                XmlNodeList nodes = doc.SelectNodes("//Permissions/Permission");
                PermissionCollection permissions = new PermissionCollection();
                foreach (XmlNode node in nodes)
                {
                    PermissionInfo permission = new PermissionInfo();
                    permission.PermissionName = node.Attributes[0].Value;
                    permission.Description = node.Attributes[1].Value;
                    permission.Provider = compenent.CompenentName;
                    permissions.Add(permission);
                }
                compenent.Permissions = permissions;

                try
                {
                    if (provider.CreateCompenent(compenent) != null)
                    {
                        XmlNode tabNode = doc.SelectSingleNode("/install/TabCollection");
                        if (tabNode != null)
                        {
                            string navigationPath = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath,
                                                            string.Format(@"admin\xml\NavigationOfSite{0}.xml", siteId));
                            XmlDocument navigationDoc = new XmlDocument();
                            navigationDoc.Load(navigationPath);
                            XmlNode compenentNode = navigationDoc.SelectSingleNode("/TabCollection/Compenents");
                            if (compenentNode != null)
                            {
                                compenentNode.InnerXml += tabNode.InnerXml;
                            }
                            else
                            {
                                XmlNode root = doc.SelectSingleNode("/TabCollection");
                                XmlElement element = doc.CreateElement("Compenent");
                                element.InnerXml = tabNode.InnerXml;
                                root.InsertAfter(element, root.FirstChild);
                            }
                            navigationDoc.Save(navigationPath);
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
        }

        /// <summary>
        /// 返回指定组件是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <returns>是否存在。</returns>
        public bool IsCompenentExists(int siteId, string compenentName)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(compenentName, "compenentName");

            try
            {
                return provider.IsCompenentExists(siteId, compenentName);
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
        /// 返回指定组件的配置信息。
        /// </summary>
        /// <param name="compenentId">组件ID。</param>
        /// <returns>配置信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看组件信息的权限时抛出该异常</exception>
        public string LoadConfig(int compenentId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CompenentManager(UserInfo)实例化该类。");
            }
            return LoadConfig(currentUser, compenentId);
        }

        /// <summary>
        /// 返回指定组件的配置信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="compenentId">组件ID。</param>
        /// <returns>配置信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看组件信息的权限时抛出该异常</exception>
        public string LoadConfig(UserInfo currentUser, int compenentId)
        {
            Validator.CheckNotPositive(compenentId, "compenentId");

            try
            {
                return provider.GetCompenentConfig(compenentId);
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
        /// 返回指定组件的配置信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <returns>配置信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看组件信息的权限时抛出该异常</exception>
        public string LoadConfig(int siteId, string compenentName)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CompenentManager(UserInfo)实例化该类。");
            }
            return LoadConfig(currentUser, siteId, compenentName);
        }

        /// <summary>
        /// 返回指定组件的配置信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <returns>配置信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看组件信息的权限时抛出该异常</exception>
        public string LoadConfig(UserInfo currentUser, int siteId, string compenentName)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(compenentName, "compenentName");

            try
            {
                return provider.GetCompenentConfig(siteId, compenentName);
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
        /// 读取组件安装文件。
        /// </summary>
        /// <param name="workingPath">组件工作路径。</param>
        /// <returns>组件信息</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装组件的权限时抛出该异常</exception>
        public CompenentInfo ReadInstallFile(string workingPath)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CompenentManager(UserInfo)实例化该类。");
            }
            return ReadInstallFile(currentUser, workingPath);
        }

        /// <summary>
        /// 读取组件安装文件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="workingPath">组件工作路径。</param>
        /// <returns>组件信息。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装组件的权限时抛出该异常</exception>
        public CompenentInfo ReadInstallFile(UserInfo currentUser, string workingPath)
        {
            Validator.CheckStringNull(workingPath, "workingPath");
            PermissionManager.CheckPermission(currentUser, "InstallCompenent", "安装组件");

            string path = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath, workingPath);
            path = PathHelper.PathCombine(path, "install.xml");

            XmlManager manager = new XmlManager(path);
            return (CompenentInfo)manager.ConvertToEntity(typeof(CompenentInfo), "/install/*");
        }

        /// <summary>
        /// 更新组件配置信息。
        /// </summary>
        /// <param name="compenentId">组件ID。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑组件的权限时抛出该异常</exception>
        public bool SaveConfig(int compenentId, string config)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CompenentManager(UserInfo)实例化该类。");
            }
            return SaveConfig(currentUser, compenentId, config);
        }

        /// <summary>
        /// 更新组件配置信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="compenentId">组件ID。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑组件的权限时抛出该异常</exception>
        public bool SaveConfig(UserInfo currentUser, int compenentId, string config)
        {
            Validator.CheckNotPositive(compenentId, "compenentId");
            Validator.CheckStringNull(config, "config");
            PermissionManager.CheckPermission(currentUser, "EditCompenent", "编辑组件");

            try
            {
                if (provider.UpdateCompenentConfig(compenentId, config))
                {
                    if (dicConfigs.ContainsKey(compenentId))
                    {
                        dicConfigs[compenentId].LoadConfig(config);
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
        /// 更新组件配置信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑组件的权限时抛出该异常</exception>
        public bool SaveConfig(int siteId, string compenentName, string config)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CompenentManager(UserInfo)实例化该类。");
            }
            return SaveConfig(currentUser, siteId, compenentName, config);
        }

        /// <summary>
        /// 更新组件配置信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑组件的权限时抛出该异常</exception>
        public bool SaveConfig(UserInfo currentUser, int siteId, string compenentName, string config)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(compenentName, "compenentName");
            Validator.CheckStringNull(config, "config");
            PermissionManager.CheckPermission(currentUser, "EditCompenent", "编辑组件");

            try
            {
                if (provider.UpdateCompenentConfig(siteId, compenentName, config))
                {
                    int compenentId = GetCompenentId(siteId,compenentName);
                    if (dicConfigs.ContainsKey(compenentId))
                    {
                        dicConfigs[compenentId].LoadConfig(config);
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
        /// 卸载组件。
        /// </summary>
        /// <param name="compenentId">待删除组件ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载组件的权限时抛出该异常。</exception>
        public bool UninstallCompenent(int compenentId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CompenentManager(UserInfo)实例化该类。");
            }
            return UninstallCompenent(currentUser, compenentId);
        }

        /// <summary>
        /// 卸载组件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="compenentId">待删除组件ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载组件的权限时抛出该异常。</exception>
        public bool UninstallCompenent(UserInfo currentUser, int compenentId)
        {
            Validator.CheckNotPositive(compenentId, "compenentId");
            PermissionManager.CheckPermission(currentUser, "UninstallCompenent", "卸载组件");

            try
            {
                CompenentInfo compenent = provider.GetCompenent(compenentId);
                if (compenent == null)
                {
                    return false;
                }

                //删除导航信息
                string navigationPath = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath,
                                                            string.Format(@"admin\xml\NavigationOfSite{0}.xml", compenent.SiteId));
                XmlDocument navigationDoc = new XmlDocument();
                navigationDoc.Load(navigationPath);
                string xPath = string.Format("/TabCollection/Compenents/Tab[@id='{0}']", compenent.CompenentName);
                XmlNode tabNode = navigationDoc.SelectSingleNode(xPath);
                XmlNode navigationNode = navigationDoc.SelectSingleNode("/TabCollection/Compenents");
                if (tabNode != null)
                {
                    navigationNode.RemoveChild(tabNode);
                }
                navigationDoc.Save(navigationPath);

                //删除数据记录
                return provider.DeleteCompenent(compenentId);
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
        /// 卸载多个组件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="compenentIds">待删除组件ID的集合。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载组件的权限时抛出该异常。</exception>
        public bool UninstallCompenents(int[] compenentIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用CompenentManager(UserInfo)实例化该类。");
            }
            return UninstallCompenents(currentUser, compenentIds);
        }

        /// <summary>
        /// 卸载多个组件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="compenentIds">待删除组件ID的集合。</param>
        /// <returns>是否卸载成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载组件的权限时抛出该异常。</exception>
        public bool UninstallCompenents(UserInfo currentUser, int[] compenentIds)
        {
            Validator.CheckNull(compenentIds, "compenentIds");
            foreach (int compenentId in compenentIds)
            {
                Validator.CheckNotPositive(compenentId, "compenentId");
            }
            PermissionManager.CheckPermission(currentUser, "UninstallCompenent", "卸载组件");

            try
            {
                foreach (int compenentId in compenentIds)
                {
                    CompenentInfo compenent = provider.GetCompenent(compenentId);
                    if (compenent == null)
                    {
                        continue;
                    }
                    //删除导航信息
                    string navigationPath = PathHelper.PathCombine(GlobalConfig.BasePhysicalPath,
                                                                string.Format(@"admin\xml\NavigationOfSite{0}.xml", compenent.SiteId));
                    XmlDocument navigationDoc = new XmlDocument();
                    navigationDoc.Load(navigationPath);
                    string xPath = string.Format("/TabCollection/Compenents/Tab[@id='{0}']", compenent.CompenentName);
                    XmlNode tabNode = navigationDoc.SelectSingleNode(xPath);
                    XmlNode navigationNode = navigationDoc.SelectSingleNode("/TabCollection/Compenents");
                    if (tabNode != null)
                    {
                        navigationNode.RemoveChild(tabNode);
                    }
                    navigationDoc.Save(navigationPath);
                }

                //删除数据记录
                return provider.DeleteCompenents(compenentIds);
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
