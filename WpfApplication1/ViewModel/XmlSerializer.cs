using System;
using System.IO;
using System.Xml.Serialization;

namespace XMLSerial
{
    public static class XmlSerializer
    {
        public static void SaveToXml(string filePath, object sourceObj,
            Type type, string xmlRootName)
        {
            if (!string.IsNullOrWhiteSpace(filePath) && sourceObj != null)
            {
                type = type ?? sourceObj.GetType();

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    XmlSerializerNamespaces xns = new XmlSerializerNamespaces();
                    xns.Add("", "");
                    System.Xml.Serialization.XmlSerializer xmlSerializer =
                        string.IsNullOrWhiteSpace(xmlRootName)
                            ? new System.Xml.Serialization.XmlSerializer(type)
                            : new System.Xml.Serialization.XmlSerializer(type,
                                new XmlRootAttribute(xmlRootName));
                    xmlSerializer.Serialize(writer, sourceObj,xns);
                }
            }
        }

        public static T LoadFromXml<T>(string filePath) where T : class 
        {
            object result = null;

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer =
                        new System.Xml.Serialization.XmlSerializer(typeof(T));
                    result = xmlSerializer.Deserialize(reader);
                }
            }

            return result as T;
        }
    }
}