//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：系统安全配置类。
// 完成日期：2010年04月10日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
namespace UniqueStudio.Common.Config
{
    /// <summary>
    /// 系统安全配置类。
    /// </summary>
    public class SecurityConfig:SystemConfig
    {
        /// <summary>
        /// 初始化<see cref="SecurityConfig"/>类的实例。
        /// </summary>
        public SecurityConfig()
        {
            path = @"admin\xml\SecurityConfig.xml";
        }

        //string
        private static string enclosureExtension = ".doc;.docx;.ppt;.pps;.pptx;.xls;.xlsx;.rar;.zip;.7z;.pdf;.txt";

        //Boolean
        private static bool enableLoginByUserName = true;
        private static bool enableRegister = false;
        private static bool isApprovedAfterRegister = false;

        /// <summary>
        /// 是否启用用户名方式登录。
        /// </summary>
        public static bool EnableLoginByUserName
        {
            get { return enableLoginByUserName; }
            set { enableLoginByUserName = value; }
        }
        /// <summary>
        /// 是否开放注册。
        /// </summary>
        public static bool EnableRegister
        {
            get { return enableRegister; }
            set { enableRegister = value; }
        }
        /// <summary>
        /// 外部注册后是否处于激活状态。
        /// </summary>
        public static bool IsApprovedAfterRegister
        {
            get { return isApprovedAfterRegister; }
            set { isApprovedAfterRegister = value; }
        }
        /// <summary>
        /// 允许上传的附件扩展名。
        /// </summary>
        public static string EnclosureExtension
        {
            get { return enclosureExtension; }
            set { enclosureExtension = value; }
        }
    }
}
