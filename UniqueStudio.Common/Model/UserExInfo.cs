//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示用户扩展信息的实体类。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示用户扩展信息的实体类。
    /// </summary>
    /// <remarks>该类在后续版本中将进行重大修改。</remarks>
    public class UserExInfo
    {
        private string penName;

        /// <summary>
        /// 初始化<see cref="UserExInfo"/>类的实例。
        /// </summary>
        public UserExInfo()
        {
        }

        /// <summary>
        /// 署名。
        /// </summary>
        public string PenName
        {
            get { return penName; }
            set { penName = value; }
        }
    }
}
