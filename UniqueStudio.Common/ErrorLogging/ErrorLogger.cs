﻿//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：异常记录类。
// 完成日期：2010年03月20日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;
using System.IO;
using System.Xml;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.XmlHelper;

namespace UniqueStudio.Common.ErrorLogging
{
    /// <summary>
    /// 异常记录类。
    /// </summary>
    public class ErrorLogger
    {
        private static string XML_PATH = Path.Combine(GlobalConfig.BasePhysicalPath, @"admin\xml\log\{0}");
        private static string XML_ROOT = "<?xml version=\"1.0\" encoding=\"utf-8\"?><ArrayOfErrorInfo xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></ArrayOfErrorInfo>";

        /// <summary>
        /// 将异常信息记录至错误日志。
        /// </summary>
        /// <param name="e">异常。</param>
        public static void LogError(Exception e)
        {
            LogError(e, string.Empty);
        }

        /// <summary>
        /// 将异常信息记录至错误日志。
        /// </summary>
        /// <param name="e">异常。</param>
        /// <param name="remarks">备注信息。</param>
        public static void LogError(Exception e, string remarks)
        {
            if (e == null)
            {
                return;
            }

            string path = string.Format(XML_PATH, DateTime.Now.ToString("yyyyMMdd") + ".xml");
            try
            {
                XmlManager manager = null;
                if (!File.Exists(path))
                {
                    manager = new XmlManager();
                    manager.LoadContent(XML_ROOT);
                }
                else
                {
                    manager = new XmlManager(path);
                }

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

                manager.InsertNode("/ArrayOfErrorInfo", error, typeof(ErrorInfo));
                manager.SaveXml(path);
            }
            catch
            {
                //do nothing
            }
        }

        /// <summary>
        /// 将异常信息记录至错误日志。
        /// </summary>
        /// <param name="errorType">异常类型。</param>
        /// <param name="message">错误信息。</param>
        /// <param name="remarks">备注信息。</param>
        public static void LogError(string errorType, string message, string remarks)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            string path = string.Format(XML_PATH, DateTime.Now.ToString("yyyyMMdd") + ".xml");
            try
            {
                XmlManager manager = null;
                if (!File.Exists(path))
                {
                    manager = new XmlManager();
                    manager.LoadContent(XML_ROOT);
                }
                else
                {
                    manager = new XmlManager(path);
                }

                ErrorInfo error = new ErrorInfo();
                error.ExceptionType = errorType;
                error.ErrorMessage = message;
                error.Time = DateTime.Now;
                error.Remarks = remarks;

                manager.InsertNode("/ArrayOfErrorInfo", error, typeof(ErrorInfo));
                manager.SaveXml(path);
            }
            catch
            {
                //do nothing
            }
        }

        /// <summary>
        /// 返回指定日期的错误日志。
        /// </summary>
        /// <param name="date">日期。</param>
        /// <returns>错误日志的集合。</returns>
        public static ErrorCollection GetAllErrors(DateTime date)
        {
            try
            {
                string path = string.Format(XML_PATH, date.ToString("yyyyMMdd") + ".xml");
                if (!File.Exists(path))
                {
                    return new ErrorCollection();
                }

                XmlDocument doc = XmlManager.LoadXml(path);
                ErrorCollection collection = (ErrorCollection)XmlManager.ConvertToEntity(doc, typeof(ErrorCollection), null);
                if (collection != null)
                {
                    collection.Reverse();
                }
                return collection;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 返回指定日志文件中的错误日志。
        /// </summary>
        /// <param name="fileName">日志文件。</param>
        /// <returns>错误日志的集合。</returns>
        public static ErrorCollection GetAllErrors(string fileName)
        {
            Utilities.Validator.CheckStringNull(fileName, "fileName");

            try
            {
                string path = string.Format(XML_PATH, fileName);

                if (!File.Exists(path))
                {
                    return new ErrorCollection();
                }

                XmlDocument doc = XmlManager.LoadXml(path);
                ErrorCollection collection = (ErrorCollection)XmlManager.ConvertToEntity(doc, typeof(ErrorCollection), null);
                if (collection != null)
                {
                    collection.Reverse();
                }
                return collection;
            }
            catch
            {
                return null;
            }
        }
    }
}
