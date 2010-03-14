
namespace UniqueStudio.Common.XmlHelper
{
    public class XmlParameter
    {

        public XmlParameter()
        {
        }

        public XmlParameter(string name)
            : this(name, string.Empty)
        {
            //null
        }

        public XmlParameter(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string value;
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
