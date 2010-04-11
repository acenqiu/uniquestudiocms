//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：验证码生成页面。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System;
using System.Drawing;

namespace UniqueStudio.Admin
{
    public partial class GetCheckCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string CheckCode = GenerateCheckCode();
            if (CheckCode == null || CheckCode.Trim() == String.Empty)
            {
                return;
            }

            Bitmap img = new Bitmap((int)Math.Ceiling((CheckCode.Length * 14.5)), 25);
            Graphics g = Graphics.FromImage(img);
            try
            {
                Random random = new Random();
                g.Clear(Color.White);
                for (int iCount = 0; iCount < 25; iCount++)
                {
                    int x1 = random.Next(img.Width);
                    int x2 = random.Next(img.Width);
                    int y1 = random.Next(img.Height);
                    int y2 = random.Next(img.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, img.Width, img.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(CheckCode, font, brush, 2, 2);


                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(img.Width);
                    int y = random.Next(img.Height);
                    img.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, img.Width - 1, img.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                this.Session["CheckCode"] = CheckCode;
                this.Response.ClearContent();
                this.Response.ContentType = "image/Gif";
                this.Response.BinaryWrite(ms.ToArray());
            }
            catch
            {
            }
            finally
            {
                g.Dispose();
                img.Dispose();
            }
        }

        private string GenerateCheckCode()
        {
            int number;
            char code;
            string strCheckCode = String.Empty;
            System.Random random = new Random(DateTime.Now.Millisecond);
            for (int iCount = 0; iCount < 4; iCount++)
            {
                number = random.Next();
                if (number % 2 == 0)
                {
                    code = (char)('0' + (char)(number % 10));
                }
                else
                {
                    code = (char)('A' + (char)(number % 26));
                }
                strCheckCode += code.ToString();
            }
            return strCheckCode;
        }
    }
}
