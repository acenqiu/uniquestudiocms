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
    public class ModuleControlManager
    {
        private static readonly IModuleControl provider = DALFactory.CreateModuleControl();

        public ModuleControlManager()
        {
        }

        public ModuleControlCollection GetAllModuleControls()
        {
            return provider.GetAllModuleControls();
        }

        public ModuleControlCollection GetModuleControlsForCurrentTemplate()
        {
            throw new NotImplementedException();
        }

        public ModuleControlInfo GetModuleControl(UserInfo currentUser, string controlId)
        {
            if (!PermissionManager.HasPermission(currentUser, "ViewModuleControl"))
            {
                throw new InvalidPermissionException("");
            }

            return provider.GetModuleControl(controlId);
        }

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

        public List<int> CreateModuleControls(ModuleControlCollection moduleControls)
        {
            throw new NotImplementedException();
        }

        public bool UpdateModuleControl(ModuleControlInfo moduleControl)
        {
            throw new NotImplementedException();
        }

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

        public bool DeleteModuleControl(int moduleControlId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteModuleControls(int[] moduleControlIds)
        {
            throw new NotImplementedException();
        }
    }
}
