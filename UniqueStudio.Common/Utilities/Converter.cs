﻿//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：提供基本类型间转换的方法。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;

namespace UniqueStudio.Common.Utilities
{
    /// <summary>
    /// 提供基本类型间转换的方法。
    /// </summary>
    public class Converter
    {
        private Converter()
        {
            //默认构造函数
        }

        /// <summary>
        /// 字符串转换为Bool。
        /// </summary>
        /// <remarks>如果str为空或者转换失败，则返回默认值。</remarks>
        /// <param name="str">字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>转换结果。</returns>
        public static bool BoolParse(string str, bool defaultValue)
        {
            bool tmp;
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            if (bool.TryParse(str, out tmp))
            {
                return tmp;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 字符串转换为DateTime。
        /// </summary>
        /// <remarks>如果str为空或者转换失败，则返回默认值。</remarks>
        /// <param name="str">字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>转换结果。</returns>
        public static DateTime DatetimeParse(string str, DateTime defaultValue)
        {
            DateTime tmp;
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            if (DateTime.TryParse(str, out tmp))
            {
                return tmp;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 字符串转换为Double。
        /// </summary>
        /// <remarks>如果str为空或者转换失败，则返回默认值。</remarks>
        /// <param name="str">字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>转换结果。</returns>
        public static double DoubleParse(string str, double defaultValue)
        {
            double tmp;
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            if (double.TryParse(str, out tmp))
            {
                return tmp;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 字符串转换为int。
        /// </summary>
        /// <remarks>如果str为空或者转换失败，则返回默认值。</remarks>
        /// <param name="str">字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>转换结果。</returns>
        public static int IntParse(string str, int defaultValue)
        {
            int tmp;
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            if (int.TryParse(str, out tmp))
            {
                return tmp;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 字符串转换为long。
        /// </summary>
        /// <remarks>如果str为空或者转换失败，则返回默认值。</remarks>
        /// <param name="str">字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>转换结果。</returns>
        public static long LongParse(string str, long defaultValue)
        {
            long tmp;
            if (string.IsNullOrEmpty(str))
            {
                return defaultValue;
            }
            if (long.TryParse(str, out tmp))
            {
                return tmp;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
