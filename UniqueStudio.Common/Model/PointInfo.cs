//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示二维点的实体类。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示二维点的实体类。
    /// </summary>
    /// <typeparam name="XType">横坐标数据类型。</typeparam>
    /// <typeparam name="YType">纵坐标数据类型。</typeparam>
    public class PointInfo<XType, YType>
    {
        private XType x;
        private YType y;

        /// <summary>
        /// 初始化<see cref="PointInfo"/>类的实例。
        /// </summary>
        public PointInfo()
        {
            //默认构造函数
        }

        /// <summary>
        /// 以横坐标、纵坐标初始化<see cref="PointInfo"/>类的实例。
        /// </summary>
        /// <param name="x">横坐标。</param>
        /// <param name="y">纵坐标。</param>
        public PointInfo(XType x, YType y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 横坐标。
        /// </summary>
        public XType X
        {
            get { return x; }
            set { x = value; }
        }
        /// <summary>
        /// 纵坐标。
        /// </summary>
        public YType Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}
