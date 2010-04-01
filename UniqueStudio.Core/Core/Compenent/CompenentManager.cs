using System;
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

            XmlManager manager = new XmlManager();
            XmlDocument doc = manager.LoadXml(path);
            CompenentInfo compenent = (CompenentInfo)manager.ConvertToEntity(doc, typeof(CompenentInfo), "/install/*");
            if (compenent == null)
            {
                return false;
            }
            else
            {
                if (IsCompenentExists(siteId,compenent.CompenentName))
                {
                      throw new Exception("该组件已经安装过了。");
                }

                compenent.SiteId = siteId;
                compenent.WorkingPath = workingPath;
                compenent.Config = manager.ConstructSubXmlDocument(doc, "/install/params", "config").OuterXml;

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
                        XmlNode node = doc.SelectSingleNode("/install/AdminPages");
                        if (node != null)
                        {
                            //添加导航信息
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

            XmlManager manager = new XmlManager();
            XmlDocument doc = manager.LoadXml(path);
            return (CompenentInfo)manager.ConvertToEntity(doc, typeof(CompenentInfo), "/install/*");
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新组件配置信息。
        /// </summary>
        /// <param name="compenentId">组件ID。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有编辑组件的权限时抛出该异常</exception>
        public bool SaveConfig(UserInfo currentUser, int compenentId, string config)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 卸载组件
        /// </summary>
        /// <remarks>该方法签名不确定，估计要改成使用组件ID</remarks>
        /// <param name="currentUser">当前用户信息</param>
        /// <param name="compenent">待删除组件信息</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载组件的权限时抛出该异常</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.CompenentInstallException">
        /// 当用户卸载失败时抛出该异常</exception>
        public bool UninstallCompenent(UserInfo currentUser, CompenentInfo compenent)
        {
            throw new NotImplementedException();
        }
    }
}
