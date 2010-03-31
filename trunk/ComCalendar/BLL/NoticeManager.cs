using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Core.Permission;
using UniqueStudio.ComCalendar.DAL;
using UniqueStudio.ComCalendar.Model;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Exceptions;

namespace UniqueStudio.ComCalendar.BLL
{
    public class CalNoticeManager
    {
        private NoticeProvider provider = new NoticeProvider();
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CalNoticeManager()
        { }

        /// <summary>
        /// 为日历添加一条通知
        /// </summary>
        /// <param name="calNotice">通知信息</param>
        /// <returns>是否添加成功</returns>
        public bool AddCalNotice(CalendarNotice calNotice)
        {
            //if (!PermissionManager.HasPermission(user, "AddCalendarNotice"))
            //{
            //    throw new InvalidPermissionException("AddCalendarNotice", "添加日历通知");
            //}
            return provider.AddCalNotice(calNotice);
        }

        /// <summary>
        /// 编辑一条日历通知
        /// </summary>
        /// <param name="calNotice">日历通知信息</param>
        /// <returns>是否编辑成功</returns>
        public bool EditCalNotice(CalendarNotice calNotice)
        {
            //if (!PermissionManager.HasPermission(user, "EditCalendarNotice"))
            //{
            //    throw new InvalidPermissionException("EditCalendarNotice", "编辑日历通知");
            //}
            return provider.EditCalNotice(calNotice);
        }

        /// <summary>
        /// 删除一条日历通知
        /// </summary>
        /// <param name="calId">日历通知ID</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteCalendarNoticeByCatId(Guid calId)
        {
            //if (!PermissionManager.HasPermission(user, "DeleteCalendarNotice"))
            //{
            //    throw new InvalidPermissionException("DeleteCalendarNotice", "删除日历通知");
            //}
            return provider.DeleteCalNoticeByCalId(calId);
        }
        /// <summary>
        /// 删除特定日期的全部通知
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>删除是否成功</returns>
        public bool DeleteCalendarNoticesByDate(DateTime date)
        {
            //if (!PermissionManager.HasPermission(user, "DeleteCalendarNotice"))
            //{
            //    throw new InvalidPermissionException("DeleteCalendarNotice", "删除日历通知");
            //}
            return provider.DeleteCalNoticesByDate(date);
        }

        /// <summary>
        /// 获得所有有通知的日期
        /// </summary>
        /// <returns>有通知的日期</returns>
        public List<DateTime> GetAllCalNoticeDate()
        {
            return provider.GetAllNoticedDate();
        }

        /// <summary>
        /// 获得特定日期的所有通知
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public CalendarNoticeCollection GetNoticesByDate(DateTime date)
        {
            return provider.GetNoticesByDate(date);
        }
    }
}
