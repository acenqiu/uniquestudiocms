using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.XmlHelper;

namespace UniqueStudio.Common.ErrorLogging
{
    /// <summary>
    /// 异常记录类
    /// </summary>
    public class ErrorLogger
    {
        private static string XML_PATH = Path.Combine(GlobalConfig.BasePhysicalPath, @"admin\xml\error.xml");
        private static string XML_ROOT = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ArrayOfErrorInfo xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></ArrayOfErrorInfo>";

        /// <summary>
        /// 将异常信息记录至错误日志
        /// </summary>
        /// <param name="e">异常</param>
        public static void LogError(Exception e)
        {
            LogError(e, string.Empty);
        }

        /// <summary>
        /// 将异常信息记录至错误日志
        /// </summary>
        /// <param name="e">异常</param>
        /// <param name="remarks">备注信息</param>
        public static void LogError(Exception e, string remarks)
        {
            XmlManager manager = new XmlManager();
            if (!File.Exists(XML_PATH))
            {
                try
                {
                    manager.SaveXml(XML_PATH, XML_ROOT);
                }
                catch
                {
                    return;
                }
            }
            XmlDocument doc = manager.LoadXml(XML_PATH);

            ErrorInfo error = new ErrorInfo();
            error.ExceptionType = e.GetType().FullName;
            error.ErrorMessage = e.Message;
            if (e.InnerException != null)
            {
                error.InnerExceptionType = e.InnerException.GetType().FullName;
                error.InnerErrorMessage = e.InnerException.Message;
            }
            error.RepresentationString = e.ToString();
            error.Time = DateTime.Now;
            error.Remarks = remarks;

            manager.InsertNode(doc, "/ArrayOfErrorInfo", error, typeof(ErrorInfo));
            doc.Save(XML_PATH);
        }

        /// <summary>
        /// 将异常信息记录至错误日志
        /// </summary>
        /// <param name="errorType">异常类型</param>
        /// <param name="message">错误信息</param>
        /// <param name="remarks">备注信息</param>
        public static void LogError(string errorType, string message, string remarks)
        {
            XmlManager manager = new XmlManager();
            if (!File.Exists(XML_PATH))
            {
                try
                {
                    manager.SaveXml(XML_PATH, XML_ROOT);
                }
                catch
                {
                    return;
                }
            }
            XmlDocument doc = manager.LoadXml(XML_PATH);

            ErrorInfo error = new ErrorInfo();
            error.ExceptionType = errorType;
            error.ErrorMessage = message;
            error.Time = DateTime.Now;
            error.Remarks = remarks;

            manager.InsertNode(doc, "/ArrayOfErrorInfo", error, typeof(ErrorInfo));
            doc.Save(XML_PATH);
        }

        /// <summary>
        /// 返回所有错误日志
        /// </summary>
        /// <returns>错误日志的集合</returns>
        public static ErrorCollection GetAllErrors()
        {
            XmlManager manager = new XmlManager();
            if (!File.Exists(XML_PATH))
            {
                try
                {
                    manager.SaveXml(XML_PATH, XML_ROOT);
                    return new ErrorCollection();
                }
                catch
                {
                    return null;
                }
            }

            XmlDocument doc = manager.LoadXml(XML_PATH);
            ErrorCollection collection = (ErrorCollection)manager.ConvertToEntity(doc, typeof(ErrorCollection), null);
            collection.Reverse();
            return collection;
        }
    }
}
