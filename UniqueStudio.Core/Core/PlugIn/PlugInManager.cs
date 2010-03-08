using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

using UniqueStudio.Core.Permission;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.XmlHelper;
using UniqueStudio.DAL;

namespace UniqueStudio.Core.PlugIn
{
    public class PlugInManager
    {
        private static readonly DAL.IDAL.IPlugIn provider = DALFactory.CreatePlugIn();

        /// <summary>
        /// 初始化<see cref="PlugInManager"/>类的实例
        /// </summary>
        public PlugInManager()
        {
        }

        public bool DeletePlugIn()
        {
            throw new NotImplementedException();
        }

        public PlugInCollection GetAllPlugIns()
        {
            return provider.GetAllPlugIns();
        }

        public ClassCollection GetAllPlugInsForInit()
        {
            return provider.GetAllPlugInsForInit();
        }

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

        public bool StartPlugIn(PlugInInfo plugIn)
        {
            throw new NotImplementedException();
        }

        public bool StartPlugIns(PlugInCollection plugIns)
        {
            throw new NotImplementedException();
        }

        public bool StopPlugIn(PlugInInfo plugIn)
        {
            throw new NotImplementedException();
        }

        public bool StopPlugIns(PlugInCollection plugIns)
        {
            throw new NotImplementedException();
        }

        public bool ReoderPlugIns(PlugInCollection plugIns)
        {
            throw new NotImplementedException();
        }

        public bool UninstallPlugIn(PlugInInfo plugIn)
        {
            throw new NotImplementedException();
        }

        public bool UninstallPlugIns(PlugInCollection plugIns)
        {
            throw new NotImplementedException();
        }
    }
}
