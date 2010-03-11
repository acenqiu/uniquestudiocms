using System;
using System.IO;
using System.Reflection;
using System.Xml;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;

namespace UniqueStudio.Core.PlugIn
{
    /// <summary>
    /// 提供插件管理的方法
    /// </summary>
    public class PlugInManager
    {
        private static readonly DAL.IDAL.IPlugIn provider = DALFactory.CreatePlugIn();

        /// <summary>
        /// 初始化<see cref="PlugInManager"/>类的实例
        /// </summary>
        public PlugInManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 返回插件列表
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <returns>插件列表</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看插件信息的权限时抛出该异常</exception>
        public PlugInCollection GetAllPlugIns(UserInfo currentUser)
        {
            return provider.GetAllPlugIns();
        }

        /// <summary>
        /// 返回插件列表
        /// </summary>
        /// <remarks>该方法只在程序系统启动时调用，用于完成插件的初始化工作</remarks>
        /// <returns>类集合</returns>
        public ClassCollection GetAllPlugInsForInit()
        {
            return provider.GetAllPlugInsForInit();
        }

        /// <summary>
        /// 读取插件安装文件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="installFilePath">安装文件路径</param>
        /// <returns>待安装插件信息</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装插件的权限时抛出该异常</exception>
        public PlugInInfo ReadInstallFile(UserInfo currentUser, string installFilePath)
        {
            //if (!PermissionManager.HasPermission(currentUser, ""))
            //{
            //    throw new InvalidPermissionException("");
            //}

            XmlManager manager = new XmlManager();
            XmlDocument doc = manager.LoadXml(Path.Combine(GlobalConfig.BasePhysicalPath, installFilePath));
            PlugInInfo plugin = (PlugInInfo)manager.ConvertToEntity(doc, typeof(PlugInInfo), "/install/*");
            return plugin;
        }

        /// <summary>
        /// 安装插件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="installFilePath">安装文件路径</param>
        /// <returns>如果安装成功返回插件信息，否则返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装插件的权限时抛出该异常</exception>
        public PlugInInfo InstallPlugIn(UserInfo currentUser, string installFilePath)
        {
            PlugInInfo plugIn = ReadInstallFile(currentUser, installFilePath);
            plugIn = provider.CreatePlugIn(plugIn);

            //读取全局配置，决定是否立即启动
            if (plugIn != null)
            {
                try
                {
                    IPlugIn p = (IPlugIn)Assembly.Load(plugIn.Assembly).CreateInstance(plugIn.ClassPath);
                    p.Init();
                }
                catch
                {
                }
            }
            return plugIn;
        }

        /// <summary>
        /// 创建插件
        /// </summary>
        /// <remarks>该方法将在后续版本中移除</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="plugIn">待安装插件信息</param>
        /// <returns>如果安装成功返回插件信息，否则返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有安装插件的权限时抛出该异常</exception>
        public PlugInInfo CreatePlugIn(UserInfo currentUser, PlugInInfo plugIn)
        {
            if (!PermissionManager.HasPermission(currentUser, "InstallPlugIn"))
            {
                throw new InvalidPermissionException("当前用户没有安装插件的权限，请与管理员联系。");
            }
            plugIn = provider.CreatePlugIn(plugIn);
            if (plugIn != null)
            {
                try
                {
                    IPlugIn p = (IPlugIn)Assembly.Load(plugIn.Assembly).CreateInstance(plugIn.ClassPath);
                    p.Init();
                }
                catch
                {
                }
            }
            return plugIn;
        }

        /// <summary>
        /// 启用指定插件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="plugIn">待启用插件信息</param>
        /// <returns>是否启用成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有启用插件的权限时抛出该异常</exception>
        public bool StartPlugIn(UserInfo currentUser, PlugInInfo plugIn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 启用多个插件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="plugIns">待启用插件的集合</param>
        /// <returns>是否启用成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有启用插件的权限时抛出该异常</exception>
        public bool StartPlugIns(UserInfo currentUser, PlugInCollection plugIns)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 停用指定插件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="plugIn">待停用插件的信息</param>
        /// <returns>是否停用成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有停用插件的权限时抛出该异常</exception>
        public bool StopPlugIn(UserInfo currentUser, PlugInInfo plugIn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 停用多个插件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="plugIns">待停用插件的集合</param>
        /// <returns>是否停用成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有停用插件的权限时抛出该异常</exception>
        public bool StopPlugIns(UserInfo currentUser, PlugInCollection plugIns)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 卸载指定插件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="plugIn">待卸载插件信息</param>
        /// <returns>是否卸载成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有卸载插件的权限时抛出该异常</exception>
        public bool UninstallPlugIn(UserInfo currentUser, PlugInInfo plugIn)
        {
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
