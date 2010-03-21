using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 二维点的结合。
    /// </summary>
    /// <typeparam name="XType">横坐标数据类型。</typeparam>
    /// <typeparam name="YType">纵坐标数据类型。</typeparam>
    public class PointCollection<XType, YType> : List<PointInfo<XType, YType>>
    {
        private XType maxX;
        private XType minX;
        private YType maxY;
        private YType minY;

        /// <summary>
        /// 初始化<see cref="PointCollection"/>类的实例。
        /// </summary>
        public PointCollection()
            : base()
        {
        }

        /// <summary>
        /// 以集合容量初始化<see cref="PointCollection"/>类的实例。
        /// </summary>
        /// <param name="capacity">集合容量。</param>
        public PointCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// 横轴最大值。
        /// </summary>
        public XType MaxX
        {
            get { return maxX; }
            set { maxX = value; }
        }
        /// <summary>
        /// 横轴最小值。
        /// </summary>
        public XType MinX
        {
            get { return minX; }
            set { minX = value; }
        }
        /// <summary>
        /// 纵轴最大值。
        /// </summary>
        public YType MaxY
        {
            get { return maxY; }
            set { maxY = value; }
        }
        /// <summary>
        /// 纵轴最小值。
        /// </summary>
        public YType MinY
        {
            get { return minY; }
            set { minY = value; }
        }
    }
}
