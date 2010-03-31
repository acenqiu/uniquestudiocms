//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供模块控件管理的方法。
// 完成日期：2010年03月31日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml;

using UniqueStudio.Common.ErrorLogging;
using UniqueStudio.Common.Exceptions;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.Permission;
using UniqueStudio.DAL;
using UniqueStudio.DAL.IDAL;

namespace UniqueStudio.Core.Module
{
    /// <summary>
    /// 提供模块控件管理的方法。
    /// </summary>
    public class ModuleControlManager
    {
        private static readonly IModuleControl provider = DALFactory.CreateModuleControl();
        private static List<ModuleControlInfo> controls = new List<ModuleControlInfo>();
        private UserInfo currentUser;

        /// <summary>
        /// 初始化<see cref="ModuleControlManager"/>类的实例。
        /// </summary>
        public ModuleControlManager()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以当前用户初始化<see cref="ModuleControlManager"/>类的实例。
        /// </summary>
        /// <param name="currentUser">当前用户。</param>
        public ModuleControlManager(UserInfo currentUser)
        {
            this.currentUser = currentUser;
        }

        /// <summary>
        /// 返回指定控件是否启用。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="controlName">控件名称。</param>
        /// <returns>是否启用。</returns>
        public static bool IsEnabled(int siteId, string controlName)
        {
            Validator.CheckNegative(siteId, "siteId");
            Validator.CheckStringNull(controlName, "controlName");

            ModuleControlInfo control = GetModuleControlFromCache(siteId, controlName);
            if (control != null)
            {
                return control.IsEnabled;
            }
            else
            {
                throw new ControlNotFoundException();
            }
        }

        /// <summary>
        /// 返回指定控件指定配置项的值。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="controlName">控件名称。</param>
        /// <param name="key">配置项名称。</param>
        /// <returns>值。</returns>
        public static string GetConfigValue(int siteId, string controlName, string key)
        {
            Validator.CheckNegative(siteId, "siteId");
            Validator.CheckStringNull(controlName, "plugInName");
            Validator.CheckStringNull(key, "key");

            ModuleControlInfo control = GetModuleControlFromCache(siteId, controlName);
            if (control != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(control.Config);
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
                throw new ControlNotFoundException();
            }
        }

        private static ModuleControlInfo GetModuleControlFromCache(int siteId, string controlName)
        {
            for (int i = 0; i < controls.Count; i++)
            {
                if (controls[i].ControlName == controlName && controls[i].SiteId == siteId)
                {
                    return controls[i];
                }
            }

            //从数据库载入
            try
            {
                ModuleControlInfo control = provider.GetModuleControl(siteId, controlName);
                if (control != null)
                {
                    controls.Add(control);
                    return control;
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
        /// 创建控件。
        /// </summary>
        /// <param name="control">控件信息。</param>
        /// <returns>是否创建成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建控件的权限时抛出该异常。</exception>
        public bool CreateModuleControl(ModuleControlInfo control)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return CreateModuleControl(currentUser, control);
        }

        /// <summary>
        /// 创建控件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="control">控件信息。</param>
        /// <returns>是否创建成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有创建控件的权限时抛出该异常。</exception>
        public bool CreateModuleControl(UserInfo currentUser, ModuleControlInfo control)
        {
            Validator.CheckStringNull(control.ControlName, "control.ControlName");
            Validator.CheckNotPositive(control.SiteId, "control.SiteId");
            Validator.CheckNotPositive(control.ModuleId, "control.ModuleId");
            PermissionManager.CheckPermission(currentUser, "CreateModuleControl", "创建控件");

            if (IsModuleControlExists(control.SiteId, control.ControlName))
            {
                throw new Exception("该控件已经存在，请重新设置！");
            }

            try
            {
                return provider.CreateModuleControl(control);
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
        /// 删除指定控件。
        /// </summary>
        /// <param name="controlId">待删除控件ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除控件的权限时抛出该异常。</exception>
        public bool DeleteModuleControl(int controlId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return DeleteModuleControl(currentUser, controlId);
        }

        /// <summary>
        /// 删除指定控件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="controlId">待删除控件ID。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除控件的权限时抛出该异常。</exception>
        public bool DeleteModuleControl(UserInfo currentUser, int controlId)
        {
            Validator.CheckNotPositive(controlId, "controlId");

            try
            {
                return provider.DeleteModuleControl(controlId);
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
        /// 删除多个控件。
        /// </summary>
        /// <param name="controlIds">待删除控件ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除控件的权限时抛出该异常。</exception>
        public bool DeleteModuleControls(int[] controlIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return DeleteModuleControls(currentUser, controlIds);
        }

        /// <summary>
        /// 删除多个控件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="controlIds">待删除控件的ID的集合。</param>
        /// <returns>是否删除成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有删除控件的权限时抛出该异常。</exception>
        public bool DeleteModuleControls(UserInfo currentUser, int[] controlIds)
        {
            Validator.CheckNull(controlIds, "controlIds");
            foreach (int controlId in controlIds)
            {
                Validator.CheckNotPositive(controlId, "controlIds");
            }

            try
            {
                return provider.DeleteModuleControls(controlIds);
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
        /// 返回指定网站下的所有控件。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <returns>控件的集合。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看控件信息的权限时抛出该异常。</exception>
        public ModuleControlCollection GetAllModuleControls(int siteId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return GetAllModuleControls(currentUser, siteId);
        }

        /// <summary>
        /// 返回指定网站下的所有控件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="siteId">网站ID。</param>
        /// <returns>控件的集合。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看控件信息的权限时抛出该异常。</exception>
        public ModuleControlCollection GetAllModuleControls(UserInfo currentUser, int siteId)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            PermissionManager.CheckPermission(currentUser, "ViewModuleControl", "查看控件信息");

            try
            {
                return provider.GetAllModuleControls(siteId);
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
        /// 返回指定控件。
        /// </summary>
        /// <param name="controlId">待获取控件ID。</param>
        /// <returns>控件信息，如果不存在返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看控件信息的权限时抛出该异常。</exception>
        public ModuleControlInfo GetModuleControl(int controlId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return GetModuleControl(currentUser, controlId);
        }

        /// <summary>
        /// 返回指定控件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="controlId">待获取控件ID。</param>
        /// <returns>控件信息，如果不存在返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看控件信息的权限时抛出该异常。</exception>
        public ModuleControlInfo GetModuleControl(UserInfo currentUser, int controlId)
        {
            Validator.CheckNotPositive(controlId, "controlId");
            PermissionManager.CheckPermission(currentUser, "ViewModuleControl", "查看控件信息");

            try
            {
                return provider.GetModuleControl(controlId);
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
        /// 返回指定控件。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="controlName">待获取控件名称。</param>
        /// <returns>控件信息，如果不存在返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看控件信息的权限时抛出该异常。</exception>
        public ModuleControlInfo GetModuleControl(int siteId, string controlName)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return GetModuleControl(currentUser, siteId, controlName);
        }

        /// <summary>
        /// 返回指定控件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="siteId">网站ID。</param>
        /// <param name="controlName">待获取控件名称。</param>
        /// <returns>控件信息，如果不存在返回空。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有查看控件信息的权限时抛出该异常。</exception>
        public ModuleControlInfo GetModuleControl(UserInfo currentUser, int siteId, string controlName)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(controlName, "controlName");
            PermissionManager.CheckPermission(currentUser, "ViewModuleControl", "查看控件信息");

            try
            {
                return provider.GetModuleControl(siteId, controlName);
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
        /// 返回指定控件是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="controlName">控件名称。</param>
        /// <returns>是否存在。</returns>
        public bool IsModuleControlExists(int siteId, string controlName)
        {
            Validator.CheckNotPositive(siteId, "siteId");
            Validator.CheckStringNull(controlName, "controlName");

            try
            {
                return provider.IsModuleControlExists(siteId, controlName);
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
        /// 启用指定控件。
        /// </summary>
        /// <param name="controlId">待启用控件ID。</param>
        /// <returns>是否启用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有启用控件的权限时抛出该异常。</exception>
        public bool StartModuleControl(int controlId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return StartModuleControl(currentUser, controlId);
        }

        /// <summary>
        /// 启用指定控件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="controlId">待启用控件ID。</param>
        /// <returns>是否启用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有启用控件的权限时抛出该异常。</exception>
        public bool StartModuleControl(UserInfo currentUser, int controlId)
        {
            Validator.CheckNotPositive(controlId, "controlId");
            PermissionManager.CheckPermission(currentUser, "StartModuleControl", "启用控件");

            try
            {
                return provider.SetModuleControlStatus(controlId, true);
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
        /// 启用多个控件。
        /// </summary>
        /// <param name="controlIds">待启用控件ID的集合。</param>
        /// <returns>是否启用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有启用控件的权限时抛出该异常。</exception>
        public bool StartModuleControls(int[] controlIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return StartModuleControls(currentUser, controlIds);
        }

        /// <summary>
        /// 启用多个控件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="controlIds">待启用控件ID的集合。</param>
        /// <returns>是否启用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有启用控件的权限时抛出该异常。</exception>
        public bool StartModuleControls(UserInfo currentUser, int[] controlIds)
        {
            Validator.CheckNull(controlIds, "controlIds");
            foreach (int controlId in controlIds)
            {
                Validator.CheckNotPositive(controlId, "controlIds");
            }
            PermissionManager.CheckPermission(currentUser, "StartModuleControl", "启用控件");

            try
            {
                return provider.SetModuleControlStatus(controlIds, true);
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
        /// 停用指定控件。
        /// </summary>
        /// <param name="controlId">待停用控件ID。</param>
        /// <returns>是否停用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有停用控件的权限时抛出该异常。</exception>
        public bool StopModuleControl(int controlId)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return StopModuleControl(currentUser, controlId);
        }

        /// <summary>
        /// 停用指定控件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="controlId">待停用控件ID。</param>
        /// <returns>是否停用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有停用控件的权限时抛出该异常。</exception>
        public bool StopModuleControl(UserInfo currentUser, int controlId)
        {
            Validator.CheckNotPositive(controlId, "controlId");
            PermissionManager.CheckPermission(currentUser, "StopModuleControl", "停用控件");

            try
            {
                return provider.SetModuleControlStatus(controlId, false);
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
        /// 停用多个控件。
        /// </summary>
        /// <param name="controlIds">待停用控件ID的集合。</param>
        /// <returns>是否停用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有停用控件的权限时抛出该异常。</exception>
        public bool StopModuleControls(int[] controlIds)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return StopModuleControls(currentUser, controlIds);
        }

        /// <summary>
        /// 停用多个控件。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="controlIds">待停用控件ID的集合。</param>
        /// <returns>是否停用成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有停用控件的权限时抛出该异常。</exception>
        public bool StopModuleControls(UserInfo currentUser, int[] controlIds)
        {
            Validator.CheckNull(controlIds, "controlIds");
            foreach (int controlId in controlIds)
            {
                Validator.CheckNotPositive(controlId, "controlIds");
            }
            PermissionManager.CheckPermission(currentUser, "StopModuleControl", "停用控件");

            try
            {
                return provider.SetModuleControlStatus(controlIds, false);
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
        /// 更新控件信息。
        /// </summary>
        /// <param name="control">待更新控件信息。</param>
        /// <param name="oldControlName">原始控件名称。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有更新控件信息的权限时抛出该异常。</exception>
        public bool UpdateModuleControl(ModuleControlInfo control, string oldControlName)
        {
            if (currentUser == null)
            {
                throw new Exception("请使用ModuleControlManager(UserInfo)实例化该类。");
            }
            return UpdateModuleControl(currentUser, control, oldControlName);
        }

        /// <summary>
        /// 更新控件信息。
        /// </summary>
        /// <param name="currentUser">执行该方法的用户信息。</param>
        /// <param name="control">待更新控件信息。</param>
        /// <param name="oldControlName">原始控件名称。</param>
        /// <returns>是否更新成功。</returns>
        /// <exception cref="UniqueStudio.Common.Exceptions.InvalidPermissionException">
        /// 当用户没有更新控件信息的权限时抛出该异常。</exception>
        public bool UpdateModuleControl(UserInfo currentUser, ModuleControlInfo control, string oldControlName)
        {
            Validator.CheckNotPositive(control.ControlId, "control.ControlId");
            Validator.CheckStringNull(control.ControlName, "control.ControlName");
            Validator.CheckNotPositive(control.SiteId, "control.SiteId");
            Validator.CheckStringNull(control.Config, "control.Config");
            PermissionManager.CheckPermission(currentUser, "CreateModuleControl", "创建控件");

            if (control.ControlName != oldControlName && IsModuleControlExists(control.SiteId, control.ControlName))
            {
                throw new Exception("该控件已经存在，请重新设置！");
            }
            try
            {
                if (provider.UpdateModuleControl(control))
                {
                    ModuleControlInfo m = GetModuleControlFromCache(control.SiteId, oldControlName);
                    if (m != null)
                    {
                        m.ControlName = control.ControlName;
                        m.IsEnabled = control.IsEnabled;
                        m.Config = control.Config;
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
}
