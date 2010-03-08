using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class ModuleInfo
    {
        private int moduleId;
        private string moduleName;
        private string displayName;
        private string moduleAuthor;
        private string description;
        private string installFilePath;
        private string parameters;

        public ModuleInfo()
        {
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

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public string ModuleAuthor
        {
            get { return moduleAuthor; }
            set { moduleAuthor = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string InstallFilePath
        {
            get { return installFilePath; }
            set { installFilePath = value; }
        }
        public string Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
    }
}
