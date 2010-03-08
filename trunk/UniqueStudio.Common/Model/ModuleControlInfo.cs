using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class ModuleControlInfo
    {
        private string controlId;
        private int moduleId;
        private string moduleName;
        private bool isEnabled;
        private string layoutPath;
        private string parameters;

        public ModuleControlInfo()
        {
        }

        public string ControlId
        {
            get { return controlId; }
            set { controlId = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public string ModuleName
        {
            get { return moduleName; }
            set { moduleName = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public string LayoutPath
        {
            get { return layoutPath; }
            set { layoutPath = value; }
        }
        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
    }
}
