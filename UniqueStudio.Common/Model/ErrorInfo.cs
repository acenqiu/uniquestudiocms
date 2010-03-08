using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
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

        public ErrorInfo() 
        {
        }

        public string ExceptionType
        {
            get { return exceptionType; }
            set { exceptionType = value; }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }
        public string InnerExceptionType
        {
            get { return innerExceptionType; }
            set { innerExceptionType = value; }
        }
        public string InnerErrorMessage
        {
            get { return innerErrorMessage; }
            set { innerErrorMessage = value; }
        }
        public string RepresentationString
        {
            get { return representationString; }
            set { representationString = value; }
        }
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
    }
}
