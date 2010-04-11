//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：访问统计图页面。
// 完成日期：2010年04月11日
// 版本：v1.0alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Text;

using UniqueStudio.Common.Config;
using UniqueStudio.Common.Model;
using UniqueStudio.Common.Utilities;
using UniqueStudio.Core.PageVisit;

namespace UniqueStudio.Admin.admin.background
{
    public partial class pvstat : Controls.AdminBasePage
    {
        protected DateTime Date;
        protected string JsArrayOfDay;
        protected string JsArrayOfCount;
        protected int MinCount;
        protected int MaxCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillDropDownLists();
                GetData();
            }
        }

        private void FillDropDownLists()
        {
            Date = DateTime.Now;

            for (int i = Date.Year; i >= 2010; i--)
            {
                ddlYears.Items.Add(i.ToString());
            }
            for (int i = 1; i <= 12; i++)
            {
                ddlMonths.Items.Add(i.ToString());
            }
            ddlMonths.SelectedIndex = Date.Month - 1;
        }

        private void GetData()
        {
            PageVisitManager manager = new PageVisitManager();
            try
            {
                PointCollection<int, int> stat = manager.GetPvStatByMonth(CurrentUser, Date);

                StringBuilder sbJsArrayOfDay = new StringBuilder("[");
                StringBuilder sbJsArrayOfCount = new StringBuilder("[");
                StringBuilder sbHead = new StringBuilder();
                StringBuilder sbData = new StringBuilder();
                for (int i = 0; i < stat.Count; i++)
                {
                    sbJsArrayOfDay.Append(string.Format("'{0}日',", stat[i].X));
                    sbJsArrayOfCount.Append((1.0 * stat[i].Y / stat.MaxY) * 100).Append(",");
                    sbHead.Append(string.Format("<td>{0}日</td>", stat[i].X));
                    sbData.Append(string.Format("<td>{0}</td>", stat[i].Y));
                }
                sbJsArrayOfDay[sbJsArrayOfDay.Length - 1] = ']';
                sbJsArrayOfCount[sbJsArrayOfCount.Length - 1] = ']';
                JsArrayOfDay = sbJsArrayOfDay.ToString();
                JsArrayOfCount = sbJsArrayOfCount.ToString();
                MinCount = 0;
                MaxCount = stat.MaxY;

                ltlHead.Text = sbHead.ToString();
                ltlData.Text = sbData.ToString();
            }
            catch (Exception ex)
            {
                message.SetErrorMessage("数据读取失败：" + ex.Message);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            Date = new DateTime(DateTime.Now.Year - ddlYears.SelectedIndex, 1 + ddlMonths.SelectedIndex, 1);
            GetData();
        }
    }
}
