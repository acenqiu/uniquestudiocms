//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：表示系统异常的实体类。
// 完成日期：2010年03月18日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示系统异常的实体类。
    /// </summary>
    [Serializable]
    public class ErrorInfo
    {
        private string exceptionType;
        private string errorMessage;
        private string innerExceptionType;
        private string innerErrorMessage;
        private string representationString;
        private DateTime time;
        private string remarks;

        /// <summary>
        /// 初始化<see cref="ErrorInfo"/>类的实例。
        /// </summary>
        public ErrorInfo() 
        {
        }

        /// <summary>
        /// 异常类型。
        /// </summary>
        public string ExceptionType
        {
            get { return exceptionType; }
            set { exceptionType = value; }
        }
        /// <summary>
        /// 异常信息。
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
        /// <summary>
        /// 内部异常类型。
        /// </summary>
        public string InnerExceptionType
        {
            get { return innerExceptionType; }
            set { innerExceptionType = value; }
        }
        /// <summary>
        /// 内部异常信息。
        /// </summary>
        public string InnerErrorMessage
        {
            get { return innerErrorMessage; }
            set { innerErrorMessage = value; }
        }
        /// <summary>
        /// 错误描述信息。
        /// </summary>
        public string RepresentationString
        {
            get { return representationString; }
            set { representationString = value; }
        }
        /// <summary>
        /// 异常抛出时间。
        /// </summary>
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
        /// <summary>
        /// 备注。
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
    }
}
