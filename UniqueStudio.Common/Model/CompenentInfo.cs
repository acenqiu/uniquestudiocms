using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class CompenentInfo
    {
        private int compenentId;
        private string compenentName;
        private string displayName;
        private string compenentAuthor;
        private string description;
        private string installFilePath;


        public CompenentInfo()
        {
        }

        public int CompenentId
        {
            get { return compenentId; }
            set { compenentId = value; }
        }
        public string CompenentName
        {
            get { return compenentName; }
            set { compenentName = value; }
        }
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        public string CompenentAuthor
        {
            get { return compenentAuthor; }
            set { compenentAuthor = value; }
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
    }
}
