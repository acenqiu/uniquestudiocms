using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

namespace UniqueStudio.Core.Compenent
{
    /// <summary>
    /// 提供组件管理的方法
    /// </summary>
    public class CompenentManager
    {
        private static readonly ICompenent provider = DALFactory.CreateCompenent();

        /// <summary>
        /// 初始化<see cref="CompenentManager"/>类的实例
        /// </summary>
        public CompenentManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 返回所有组件的信息
        /// </summary>
        /// <returns>包含所有信息的组件集合</returns>
        public CompenentCollection GetAllCompenents()
        {
            return provider.GetAllCompenents();
        }

        /// <summary>
        /// 读取组件安装文件
        /// </summary>
        /// <remarks>在后续版本中可能修改函数签名</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="installFilePath">组件安装文件完整路径</param>
        /// <returns>组件信息</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装组件的权限时抛出该异常</exception>
        public CompenentInfo ReadInstallFile(UserInfo currentUser, string installFilePath)
        {
            if (!PermissionManager.HasPermission(currentUser, "InstallCompenent"))
            {
                throw new InvalidPermissionException("");
            }

            XmlManager manager = new XmlManager();
            XmlDocument doc = manager.LoadXml(Path.Combine(GlobalConfig.BasePhysicalPath, installFilePath));
            CompenentInfo compenent = (CompenentInfo)manager.ConvertToEntity(doc, typeof(CompenentInfo), "/install/*");
            return compenent;
        }

        /// <summary>
        /// 安装组件
        /// </summary>
        /// <remarks>路径暂时在页面端完成物理路径映射</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="installFilePath">组件安装文件完整路径</param>
        /// <returns>如果安装成功则返回组件信息，否则返回null</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装组件的权限时抛出该异常</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.CompenentInstallException">
        /// 当用户安装失败时抛出该异常</exception>
        public CompenentInfo InstallCompenent(UserInfo currentUser, string installFilePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 安装组件
        /// </summary>
        /// <remarks>该方法在实现上存在问题，需重写</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="compenent">待安装组件信息</param>
        /// <param name="installFilePath">组件安装文件完整路径</param>
        /// <returns>如果安装成功则返回包含组件信息的实体，否则返回null或抛出异常</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装组件的权限时抛出该异常</exception>
        /// <exception cref="UniqueStudio.Common.Exceptions.CompenentInstallException">
        /// 当用户安装失败时抛出该异常</exception>
        public CompenentInfo InstallCompenent(UserInfo currentUser, CompenentInfo compenent, string installFilePath)
        {
            if (compenent == null)
            {
                throw new ArgumentNullException("compenent");
            }
            if (string.IsNullOrEmpty(installFilePath))
            {
                throw new ArgumentNullException("installFilePath");
            }

            if (!PermissionManager.HasPermission(currentUser,"InstallCompenent"))
            {
                throw new InvalidPermissionException("");
            }

            XmlManager manager = new XmlManager();
            XmlDocument doc = manager.LoadXml(Path.Combine(GlobalConfig.BasePhysicalPath,installFilePath));

            try
            {
                compenent = provider.CreateCompenent(compenent);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex);
                throw new CompenentInstallException();
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
            IPermission permissionProvider = DALFactory.CreatePermission();
            if (!permissionProvider.CreatePermissions(permissions))
            {
                //回滚

                throw new CompenentInstallException();
            }

            nodes = doc.SelectNodes("//Role");
            RoleCollection roles = new RoleCollection();
            foreach (XmlNode node in nodes)
            {
                RoleInfo role = new RoleInfo();
                role.RoleName = node.Attributes[0].Value;
                role.Description = node.Attributes[1].Value;
                role.Permissions = new PermissionCollection();

                foreach (XmlNode childNode in node.ChildNodes)
                {
                    PermissionInfo permission = new PermissionInfo();
                    permission.PermissionName = childNode.InnerText;
                    role.Permissions.Add(permission);
                }
                roles.Add(role);
            }
            IRole roleProvider = DALFactory.CreateRole();
            roleProvider.CreateRoles(roles);

            return compenent;
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
