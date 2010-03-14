using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    /// <summary>
    /// 表示系统异常的实体类
    /// </summary>
    /// <remarks>可能在后续版本中修改名称</remarks>
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
        /// 初始化<see cref="ErrorInfo"/>类的实例
        /// </summary>
        public ErrorInfo() 
        {
        }

        /// <summary>
        /// 异常类型
        /// </summary>
        public string ExceptionType
        {
            get { return exceptionType; }
            set { exceptionType = value; }
        }
        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
        /// <summary>
        /// 内部异常类型
        /// </summary>
        public string InnerExceptionType
        {
            get { return innerExceptionType; }
            set { innerExceptionType = value; }
        }
        /// <summary>
        /// 内部异常信息
        /// </summary>
        public string InnerErrorMessage
        {
            get { return innerErrorMessage; }
            set { innerErrorMessage = value; }
        }
        /// <summary>
        /// 错误描述信息
        /// </summary>
        public string RepresentationString
        {
            get { return representationString; }
            set { representationString = value; }
        }
        /// <summary>
        /// 异常抛出时间
        /// </summary>
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
    }
}
