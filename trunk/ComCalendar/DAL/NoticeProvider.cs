using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using UniqueStudio.Common.DatabaseHelper;
using UniqueStudio.ComCalendar.Model;

namespace UniqueStudio.ComCalendar.DAL
{
    public class NoticeProvider
    {
        private const string ADDCALNOTICE = "AddCalNotice";
        private const string EDITCALNOTICE = "EditCalNotice";
        private const string DELETECALNOTICEBYCALID = "DeleteCalNoticeByCalID";
        private const string DELETECALNOTICESBYDATE = "DeleteCalNoticesByDate";
        private const string GETALLNOTICEDDATE = "GetAllNoticedDate";
        private const string GETNOTICESBYDATE = "GetNoticesByDate";
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
                                   new SqlParameter("@NoticeDate",notice.Date),
                                   new SqlParameter("@NoticeTime",notice.Time),
                                   new SqlParameter("@Content",notice.Content),
                                   new SqlParameter("@Remarks",notice.Remarks)
            };
            try
            {
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, ADDCALNOTICE, parms) > 0;
            }
            catch (Exception)
            {
                return false;
            }
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
                                   new SqlParameter("@Remarks",notice.Remarks)
            };
            try
            {
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, EDITCALNOTICE, parms) > 0;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条日历通知
        /// </summary>
        /// <param name="calId">日历通知ID</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteCalNoticeByCalId(Guid calId)
        {
            SqlParameter parm = new SqlParameter("@ID", calId);
            try
            {
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETECALNOTICEBYCALID, parm) > 0;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除特定日期下的所有通知
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteCalNoticesByDate(DateTime date)
        {
            SqlParameter parm = new SqlParameter("@NoticeDate", date);
            try
            {
                return SqlHelper.ExecuteNonQuery(CommandType.StoredProcedure, DELETECALNOTICESBYDATE, parm) > 0;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 获得所有有通知的日期
        /// </summary>
        /// <returns>有通知的日期</returns>
        public List<DateTime> GetAllNoticedDate()
        {
            try
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GETALLNOTICEDDATE, null))
                {
                    List<DateTime> dates = new List<DateTime>();
                    while (reader.Read())
                    {
                        dates.Add(Convert.ToDateTime(reader["NoticeDate"]));
                    }
                    return dates;
                }
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 获得特定日期的所有通知
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public CalendarNoticeCollection GetNoticesByDate(DateTime date)
        {
            //date.AddHours(0);
            //date.AddMinutes(0);
            //date.AddSeconds(0);
            //date.AddMilliseconds(0);
            SqlParameter parm = new SqlParameter("@NoticeDate", date);
            try
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(CommandType.StoredProcedure, GETNOTICESBYDATE, parm))
                {
                    CalendarNoticeCollection notices = new CalendarNoticeCollection();
                    while (reader.Read())
                    {
                        CalendarNotice notice = new CalendarNotice();
                        notice.ID = new Guid(reader["ID"].ToString());
                        notice.Content = (string)reader["Content"];
                        notice.Date = Convert.ToDateTime(reader["NoticeDate"]);
                        if (reader["NoticeTime"] != DBNull.Value)
                        {
                            notice.Time = (string)reader["NoticeTime"];
                        }
                        if (reader["Remarks"] != DBNull.Value)
                        {
                            notice.Remarks = (string)reader["Remarks"];
                        }
                        notices.Add(notice);
                        // notices.Add(FillCalNotice(reader));
                    }
                    return notices;
                }
            }
            catch
            {
                return null;
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
                notice.Date = Convert.ToDateTime(reader["NoticeDate"]);
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
