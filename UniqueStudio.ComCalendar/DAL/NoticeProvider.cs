using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.ComCalendar.Model;
using UniqueStudio.Common.DatabaseHelper;

namespace UniqueStudio.ComCalendar.DAL
{
    public class NoticeProvider
    {
        private const string ADD_CALNOTICE = "AddCalNotice";
        private const string EDIT_CALNOTICE = "EditCalNotice";
        private const string DELETE_CALNOTICE_BY_CALID = "DeleteCalNoticeByCalID";
        private const string DELETE_CALNOTICES_BY_DATE = "DeleteCalNoticesByDate";
        private const string GET_ALL_NOTICE_DATE = "GetAllNoticedDate";
        private const string GET_NOTICES_BY_DATE = "GetNoticesByDate";

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public NoticeProvider()
        {
        }

        /// <summary>
        /// 为日历添加一条通知
        /// </summary>
        /// <param name="calNotice">通知信息</param>
        /// <returns>是否添加成功</returns>
        public bool AddCalNotice(CalendarNotice notice)
        {
            SqlParameter[] parms = new SqlParameter[] { 
                                   new SqlParameter("@SiteID",notice.SiteId),
                                   new SqlParameter("@NoticeDate",notice.Date),
                                   new SqlParameter("@NoticeTime",notice.Time),
                                   new SqlParameter("@Content",notice.Content),
                                   new SqlParameter("@Remarks",notice.Remarks),
                                   new SqlParameter("@Link",notice.Link),
                                   new SqlParameter("@Place",notice.Place)
            };

            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, ADD_CALNOTICE, parms) > 0;
        }

        /// <summary>
        /// 编辑一条日历通知
        /// </summary>
        /// <param name="calNotice">日历通知信息</param>
        /// <returns>是否编辑成功</returns>
        public bool EditCalNotice(CalendarNotice notice)
        {
            SqlParameter[] parms = new SqlParameter[] { 
                                   new SqlParameter("@ID",notice.ID),
                                   new SqlParameter("@NoticeDate",notice.Date),
                                   new SqlParameter("@NoticeTime",notice.Time),
                                   new SqlParameter("@Content",notice.Content),
                                   new SqlParameter("@Remarks",notice.Remarks),
                                   new SqlParameter("@Link",notice.Link),
                                   new SqlParameter("@Place",notice.Place)
            };
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, EDIT_CALNOTICE, parms) > 0;
        }

        /// <summary>
        /// 删除一条日历通知
        /// </summary>
        /// <param name="calId">日历通知ID</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteCalNoticeByCalId(Guid calId)
        {
            SqlParameter parm = new SqlParameter("@ID", calId);
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_CALNOTICE_BY_CALID, parm) > 0;
        }

        /// <summary>
        /// 删除特定日期下的所有通知
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="date">日期</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteCalNoticesByDate(int siteId, DateTime date)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                                new SqlParameter("@SiteID",siteId),
                                                                new SqlParameter("@NoticeDate", date)};
            return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETE_CALNOTICES_BY_DATE, parms) > 0;
        }

        /// <summary>
        /// 获得所有有通知的日期
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <returns>有通知的日期</returns>
        public List<DateTime> GetAllNoticedDate(int siteId)
        {
            SqlParameter parm = new SqlParameter("@SiteID", siteId);
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_ALL_NOTICE_DATE, parm))
            {
                List<DateTime> dates = new List<DateTime>();
                while (reader.Read())
                {
                    dates.Add((DateTime)reader["NoticeDate"]);
                }
                return dates;
            }
        }

        /// <summary>
        /// 获得特定日期的所有通知
        /// </summary>
        /// <param name="siteId">网站ID。 </param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public CalendarNoticeCollection GetNoticesByDate(int siteId, DateTime date)
        {
            SqlParameter[] parms = new SqlParameter[]{
                                                    new SqlParameter("@SiteID",siteId),
                                                    new SqlParameter("@NoticeDate", date)};
            using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GET_NOTICES_BY_DATE, parms))
            {
                CalendarNoticeCollection notices = new CalendarNoticeCollection();
                while (reader.Read())
                {
                    CalendarNotice notice = new CalendarNotice();
                    notice.ID = new Guid(reader["ID"].ToString());
                    notice.Content = (string)reader["Content"];
                    notice.Date = (DateTime)reader["NoticeDate"];
                    if (reader["NoticeTime"] != DBNull.Value)
                    {
                        notice.Time = (string)reader["NoticeTime"];
                    }
                    if (reader["Remarks"] != DBNull.Value)
                    {
                        notice.Remarks = (string)reader["Remarks"];
                    }
                    if (reader["Link"] != DBNull.Value)
                    {
                        notice.Link = (string)reader["Link"];
                    }
                    if (reader["Place"] != DBNull.Value)
                    {
                        notice.Place = (string)reader["Place"];
                    }
                    notices.Add(notice);
                }
                return notices;
            }
        }

        /// <summary>
        /// 填充CalendarNotice
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private CalendarNotice FillCalNotice(SqlDataReader reader)
        {
            CalendarNotice notice = new CalendarNotice();
            if (reader.Read())
            {
                notice.ID = new Guid(reader["ID"].ToString());
                notice.Content = (string)reader["Content"];
                notice.Date = (DateTime)reader["NoticeDate"];
                if (reader["Linke"] != DBNull.Value)
                {
                    notice.Link = (string)reader["Link"];
                }
                if (reader["NoticeTime"] != DBNull.Value)
                {
                    notice.Time = (string)reader["NoticeTime"];
                }
                if (reader["Remarks"] != DBNull.Value)
                {
                    notice.Remarks = (string)reader["Remarks"];
                }
            }
            return notice;
        }
    }
}
