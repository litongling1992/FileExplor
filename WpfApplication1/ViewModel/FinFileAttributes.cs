using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WpfApplication1.ViewModel
{
    public class FinFileAttributes
    {
        [XmlElement("FileType")] public FinFileType FileType;
        [XmlArrayItem("Text")] public LangItem[] Name;
        public string GetLocalName(string language)
        {
            if (Name == null || Name.Length == 0)
            {
                return null;
            }
            foreach (var name in Name.Where(name => name.Lang.Equals(language)))
            {
                return name.Content;
            }
            return Name[0].Content;
        }
       public FinFileAttributes() { }        
    }
    public class LangItem
    {
        [XmlAttribute]
        public string Lang;
        [XmlAttribute]
        public string Content;
    }
    public enum FinFileType
    {
        Dir = 0,//模型目录
        Obj,//家具模型
        Material,//材质模型
    }
}
