using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WpfApplication1.ViewModel
{
    /// <summary>
    /// 家具类型
    /// </summary>
    public enum FurnitureTypeAttributes
    {
        General = 0, // 目前无此类型的家具
        InWall, // 嵌入墙体中的家具
        OnWall // 可以贴墙的家具
    }

    /// <summary>
    /// 家具垂直方向移动的自由度
    /// </summary>
    public enum FurnVerticalFreedom
    {
        InFloor = 0,
        OnFloor,
        Free,
        OnCeiling,
        InCeiling
    }


    /// <summary>
    /// 家具透明属性
    /// </summary>
    //public enum FurnTransparent
    //{
    //    NonTransparent = 0, //不透明
    //    Transparent=100, //透明
    //}

    public class FurnitureAttributes
    {  
        [XmlElement("FurnitureType")]
        public FurnitureTypeAttributes Type;

        [XmlElement("FurnVerticalFreedom")]
        public FurnVerticalFreedom Freedom;

        /// <summary>
        /// 家具透明属性:0是不透明，100是全透明
        /// </summary>
        [XmlElement("FurnTransparent")]
         public int FurnTransparent;
        /// <summary>
        /// 顶视图用于绘制InWall家具的xaml文件，目前未使用
        /// </summary>
        public string TopVisualFile;
        /// <summary>
        /// 家具默认离地高度
        /// </summary>
        public double DefaultDisFromFloor;
        public  FurnitureAttributes()
        {}
    }
    public class LangItemAttributes
    {
        [XmlAttribute]
        public string Lang;
        [XmlAttribute]
        public string Content;
    }
    public enum FinFileTypeAttributes
    {
        Dir = 0,//模型目录
        Obj,//家具模型
        Material,//材质模型
    }

}
