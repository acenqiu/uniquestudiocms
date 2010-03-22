<%@ WebHandler Language="C#" Class="Fileup" Debug="true" %>

using System;
using System.Web;
using System.IO;


public class Fileup : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string str1 = "";
        string str2 = "";
        string rvalue = "";
        try
        {
            HttpFileCollection fileList = context.Request.Files;
            //HttpFileCollection fileList = HttpContext.Current.Request.Files;
            for (int i = 0; i < fileList.Count; i++)
            {
                HttpPostedFile hPostedFile = fileList[i];
                string filename = "";
                string filepath = "";
                if (hPostedFile.ContentLength > 0 && hPostedFile.FileName.Length > 0)
                {

                    //float zldx = hPostedFile.ContentLength / 1024;
                    filename = hPostedFile.FileName;

                    int k = filename.LastIndexOf(".");
                    int j = filename.LastIndexOf("\\");
                    string type = filename.Substring(k + 1);
                    //filename = filename.Substring(j + 1, k - j - 1);
                    filename = filename.Substring(j + 1);
                    DateTime datetime1 = System.DateTime.Now;
                    type = type.ToLower();

                    filepath = datetime1.ToString("yyyyMMddHHmmss") + "." + type;

                    hPostedFile.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~\\upload\\" + filepath));
                    if (str1 == "")
                        str1 = filepath;
                    else
                        str1 += "," + filepath;
                    if (str2 == "")
                    {
                        str2 += filename;
                    }
                    else
                        str2 += "," + filename;
                }
            }
            if (str1.Length > 0)
            {
                //context.Response.Write(str1 + "&" + str2);
                rvalue = str1;
            }
            else
            {
                rvalue = "000";
            }
        }
        catch (Exception e)
        {
            rvalue = "001";
        }
        finally
        {
            ;
        }

        context.Response.Expires = -1;
        context.Response.Clear();
        context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        context.Response.ContentType = "text/plain";
        //context.Response.Write("<script type='text/javascript'>parent.finish('" + rvalue + "');</script>");
        context.Response.Write("@returnstart@" + rvalue + "@returnend@");
        context.Response.End();

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}