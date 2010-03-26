using System;
using System.Text;

namespace UniqueStudio.ComContent.PL.controls
{
    public partial class Pagination : System.Web.UI.UserControl
    {
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }
        private int currentPage = 1;
        public int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                if (value > 0)
                {
                    currentPage = value;
                }
                else
                {
                    currentPage = 1;
                }
            }
        }
        private int numberOfShow = 7;
        public int NumberOfShow
        {
            get
            {
                return numberOfShow;
            }
            set
            {
                if (value >= 3)
                {
                    numberOfShow = value;
                }
                else
                {
                    numberOfShow = 7;
                }
            }
        }
        private string url;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }
        private bool isAssignable = false;
        public bool IsAssignable
        {
            get
            {
                return isAssignable;
            }
            set
            {
                isAssignable = value;
            }
        }
        private static string prototype = "<a href='[%href%]' class='[%class%]'>[%page%]</a>";
        private string generateLabelHtml(string href, string className, string content)
        {
            StringBuilder sb = new StringBuilder(prototype);
            sb = sb.Replace("[%href%]", href).Replace("[%class%]", className).Replace("[%page%]", content);
            return sb.ToString();

        }
        private string generateLabelsHtml(int start, int end)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = start; i <= end; i++)
            {
                sb.Append(generateLabelHtml(String.Format(url, i.ToString()), "", i.ToString())).Append("\r\n");
            }
            return sb.ToString();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (count == 0)
            {
                return;
            }
            if (count < numberOfShow)
            {
                numberOfShow = count;
            }
            if (currentPage > count)
            {
                currentPage = count;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("<span class='pagination'>").Append("\r\n");
            // int pre = (numberOfShow - 1) / 2;
            // int next = numberOfShow - pre-1;
            int pre = 0;
            int next = 0;
            int i = numberOfShow - 1;
            while (i > 0)
            {
                if (currentPage + next < count)
                {
                    next++;
                    i--;
                }
                if ((currentPage - pre > 1) && (i > 0))
                {
                    pre++;
                    i--;
                }
            }

            int start = 1;
            int end = count;
            if (currentPage != 1)
            {
                sb.Append(generateLabelHtml(String.Format(url, (currentPage - 1)), "", "上一页")).Append("\r\n");
            }
            if (currentPage - pre > 1)
            {
                start = currentPage - pre + 1;
                sb.Append(generateLabelHtml(String.Format(url, "1"), "", "1..")).Append("\r\n");
            }
            sb.Append(generateLabelsHtml(start, currentPage - 1));
            sb.Append("<strong>").Append(currentPage.ToString()).Append("</strong>");
            if (currentPage + next < count)
            {
                end = currentPage + next - 1;
            }
            sb.Append(generateLabelsHtml(currentPage + 1, end)).Append("\r\n");
            if (end < count)
            {
                sb.Append(generateLabelHtml(String.Format(url, count.ToString()), "", ".." + count.ToString())).Append("\r\n");
            }
            if (currentPage != count)
            {
                sb.Append(generateLabelHtml(String.Format(url, (currentPage + 1).ToString()), "", "下一页")).Append("\r\n");
            }
            sb.Append("</span>").Append("\r\n");
            if (isAssignable)
            {
                sb.Append("<style type='text/css'>.pageIndex {display:inline;}</style>").Append("\r\n");
            }
            htmlContent.Text = sb.ToString();
        }

        protected void gotoPageBtn_Click(object sender, EventArgs e)
        {
            string str = pageIndexTextBox.Text;
            int index = 0;
            try
            {
                index = Convert.ToInt16(str);
                if ((index < 1) || (index > count))
                {
                    throw new Exception();
                }
                Response.Redirect(String.Format(url, index.ToString()));
            }
            catch (Exception)
            {
                Response.Write("<script>alert('输入有误！')</script>");
            }
        }
    }
}