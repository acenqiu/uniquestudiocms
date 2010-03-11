using System;
using System.Collections.Generic;
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
    /// 提供模块控件管理的方法
    /// </summary>
    public class ModuleControlManager
    {
        private static readonly IModuleControl provider = DALFactory.CreateModuleControl();

        /// <summary>
        /// 初始化<see cref="ModuleControlManager"/>类的实例
        /// </summary>
        public ModuleControlManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="moduleControl">控件信息</param>
        /// <returns>如果创建成功，返回控件信息，否则返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建控件的权限时抛出该异常</exception>
        public ModuleControlInfo CreateModuleControl(UserInfo currentUser, ModuleControlInfo moduleControl)
        {
            if (!PermissionManager.HasPermission(currentUser, "CreateModuleControl"))
            {
                throw new InvalidPermissionException("");
            }

            ModuleManager manager = new ModuleManager();
            ModuleInfo module = manager.GetModule(currentUser, moduleControl.ModuleId);
            if (module == null)
            {
                throw new Exception();
            }
            else
            {
                moduleControl.Parameters = module.Parameters;
            }
            return provider.CreateModuleControl(moduleControl);
        }

        /// <summary>
        /// 创建多个控件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="moduleControls">待创建控件的集合</param>
        /// <returns>是否创建成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建控件的权限时抛出该异常</exception>
        public bool CreateModuleControls(UserInfo currentUser,ModuleControlCollection moduleControls)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除指定控件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="moduleControlId">待删除控件ID</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除控件的权限时抛出该异常</exception>
        public bool DeleteModuleControl(UserInfo currentUser, int moduleControlId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除多个控件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="moduleControlIds">待删除控件的ID的集合</param>
        /// <returns>是否删除成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除控件的权限时抛出该异常</exception>
        public bool DeleteModuleControls(UserInfo currentUser, int[] moduleControlIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 返回所有控件
        /// </summary>
        /// <returns>控件的集合</returns>
        public ModuleControlCollection GetAllModuleControls()
        {
            return provider.GetAllModuleControls();
        }

        /// <summary>
        /// 返回指定控件
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="controlId">待获取控件ID</param>
        /// <returns>控件信息，如果不存在返回空</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看控件信息的权限时抛出该异常</exception>
        public ModuleControlInfo GetModuleControl(UserInfo currentUser, string controlId)
        {
            if (!PermissionManager.HasPermission(currentUser, "ViewModuleControl"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.GetModuleControl(controlId);
        }

        /// <summary>
        /// 更新控件信息
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="moduleControl">待更新控件信息</param>
        /// <returns>是否更新成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有更新控件信息的权限时抛出该异常</exception>
        public bool UpdateModuleControl(UserInfo currentUser, ModuleControlInfo moduleControl)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 更新控件配置信息
        /// </summary>
        /// <remarks>该方法可能在后续版本中与UpdateModuleControl方法合并</remarks>
        /// <param name="currentUser">执行该方法的用户信息</param>
        /// <param name="controlId">待更新控件ID</param>
        /// <param name="parameters">控件配置信息（xml格式）</param>
        /// <returns>是否更新成功</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有更新控件信息的权限时抛出该异常</exception>
        public bool UpdateControlParameters(UserInfo currentUser, string controlId, string parameters)
        {
            if (string.IsNullOrEmpty(controlId))
            {
                throw new ArgumentNullException("controlId");
            }
            if (string.IsNullOrEmpty(parameters))
            {
                throw new ArgumentNullException("parameters");
            }
            //parameters格式验证

            if (!PermissionManager.HasPermission(currentUser, "EditModuleControl"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.UpdateControlParameters(controlId, parameters);
        }
    }
}
