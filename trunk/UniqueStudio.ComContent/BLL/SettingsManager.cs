using System.IO;
using System.Xml;

using UniqueStudio.ComContent.Model;
using UniqueStudio.Common.XmlHelper;

namespace UniqueStudio.ComContent.BLL
{
    public class SettingsManager
    {
        private const string head = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"
                                            +"<Settings xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">"
                                            +"<ArrayOfEnclosure/></Settings>";
        private XmlDocument xmlDoc = new XmlDocument();
        private XmlManager xmlManager = new XmlManager();

        public SettingsManager()
        {
            xmlDoc.LoadXml(head);
        }

        public string GetPostXMLFromEnclosures(string uri, string path)
        {
            Enclosure enclosure;
            string[] fileNames = Directory.GetFiles(path, uri + "*");
            FileInfo fileInfo;
            if (fileNames.Length > 0)
            {
                foreach (string item in fileNames)
                {
                    enclosure = new Enclosure();
                    fileInfo = new FileInfo(item);
                    enclosure.Title = fileInfo.Name.Remove(0, 15);
                    enclosure.Type = Path.GetExtension(item);
                    enclosure.Url = "/upload/" + fileInfo.Name;
                    enclosure.Length = fileInfo.Length;
                    xmlManager.InsertNode(xmlDoc, "/Settings/ArrayOfEnclosure", enclosure, typeof(Enclosure));
                }
                return xmlDoc.OuterXml;
            }
            else
            {
                return string.Empty;
            }
        }

        public EnclosureCollection GetEnclosuresFromXML(string enclosuresXML)
        {
            XmlDocument settings = new XmlDocument();
            settings.LoadXml(enclosuresXML);
            XmlDocument temp = xmlManager.ConstructSubXmlDocument(settings, "/Settings/ArrayOfEnclosure/*", "ArrayOfEnclosure");
            EnclosureCollection enclosures = (EnclosureCollection)xmlManager.ConvertToEntity(temp, typeof(EnclosureCollection), null);
            return enclosures;
        }
    }
}
