using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using UniqueStudio.Common.Exceptions;

namespace UniqueStudio.Common.Utilities
{
    public class Validator
    {
        private static Regex rEmail = new Regex(RegularExpressions.EMAIL);

        private Validator()
        {
            //默认构造函数
        }

        /// <summary>
        /// 检测指定GUID值是否为空，如果为空，抛出<see cref="ArgumentNullException"/>。
        /// </summary>
        /// <param name="value">待检测GUID值</param>
        /// <param name="paramName">参数名</param>
        public static void CheckGuid(Guid value, string paramName)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// 检测指定参数是否为空，如果为空，抛出<see cref="ArgumentNullException"/>。
        /// </summary>
        /// <param name="value">待查空参数</param>
        /// <param name="paramName">参数名</param>
        public static void CheckNull(object value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// 检测指定字符串是否为空或空白，如果为空或空白，抛出<see cref="ArgumentNullException"/>。
        /// </summary>
        /// <param name="value">待检测字符串</param>
        /// <param name="paramName">参数名</param>
        public static void CheckStringNull(string value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// 返回指定字符串是否为邮箱格式
        /// </summary>
        /// <param name="value">待检测字符串</param>
        /// <returns>是否为邮箱格式</returns>
        /// <exception cref="ArgumentNullException">
        /// 当待检测字符串为空时抛出该异常</exception>
        public static bool CheckEmail(string value)
        {
            CheckStringNull(value,null);
            return rEmail.IsMatch(value);
        }

        /// <summary>
        /// 检测指定字符串是否为邮箱格式，如果该字符串为空，抛出<see cref="ArgumentNullException"/>，
        /// 如果不是邮箱格式，抛出<see cref="ArgumentException"/>。
        /// </summary>
        /// <param name="value">待检测字符串</param>
        /// <param name="paramName">参数名，可空</param>
        public static void CheckEmail(string value, string paramName)
        {
            CheckStringNull(value, paramName);
            if (!rEmail.IsMatch(value))
            {
                if (string.IsNullOrEmpty(paramName))
                {
                    throw new ArgumentException("邮箱格式不正确！");
                }
                else
                {
                    throw new ArgumentException("邮箱格式不正确！", paramName);
                }
            }
        }

        /// <summary>
        /// 检测指定数值是否为负数，如果是，抛出<see cref="NegativeNumberException"/>。
        /// </summary>
        /// <param name="value">待检测数值</param>
        /// <param name="paramName">参数名</param>
        public static void CheckNegative(int value, string paramName)
        {
            if (value<0)
            {
                throw new NegativeNumberException(paramName);
            }
        }

        /// <summary>
        /// 检测指定数值是否为非正数，如果是，抛出<see cref="NotPositiveNumberException"/>。
        /// </summary>
        /// <param name="value">待检测数值</param>
        /// <param name="paramName">参数名</param>
        public static void CheckNotPositive(int value, string paramName)
        {
            if (value <= 0)
            {
                throw new NotPositiveNumberException(paramName);
            }
        }
    }
}
