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
