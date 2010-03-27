using System;
using System.Collections.Generic;
using System.Text;
using UniqueStudio.ComContent.Model;
using System.IO;
using System.Xml;
using UniqueStudio.Common.XmlHelper;

namespace UniqueStudio.ComContent.BLL
{
    public class SettingsManager
    {
        private static string head = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Settings xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><ArrayOfEnclosure/></Settings>";
        XmlDocument xmldoc = new XmlDocument();
        public SettingsManager()
        {
            xmldoc.LoadXml(head);
        }
        XmlManager xm = new XmlManager();
        public string GetPostXMLFromEnclosures(string uri, string path)
        {
            Enclosure enclosure;
            //string filepath = @"~/upload/";
            string[] filenames = Directory.GetFiles(path, uri + "*");
            FileInfo fileInfo;
            if (filenames.Length > 0)
            {
                //xm.InsertNode(xmldoc, "/Settings", "", "ArrayOfEnclosure");
                foreach (string item in filenames)
                {
                    enclosure = new Enclosure();
                    fileInfo = new FileInfo(item);
                    enclosure.Tittle = fileInfo.Name.Remove(0, 15);
                    enclosure.Type = Path.GetExtension(item);
                    enclosure.Url = "/upload/" + fileInfo.Name;
                    enclosure.Length = fileInfo.Length;
                    xm.InsertNode(xmldoc, "/Settings/ArrayOfEnclosure", enclosure, typeof(Enclosure));
                }
                return xmldoc.OuterXml;
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
            XmlDocument temp = xm.ConstructSubXmlDocument(settings, "/Settings/ArrayOfEnclosure/*", "ArrayOfEnclosure");
            EnclosureCollection enclosures = (EnclosureCollection)xm.ConvertToEntity(temp, typeof(EnclosureCollection), null);
            return enclosures;
        }
    }
}
