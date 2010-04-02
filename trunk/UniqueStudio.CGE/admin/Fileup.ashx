<%@ WebHandler Language="C#" Class="Fileup" Debug="true" %>

using System;
using System.Web;
using System.IO;
using UniqueStudio.Core.Site;
using UniqueStudio.Common.Config;
using UniqueStudio.Common.ErrorLogging;

public class Fileup : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        if (context.Request.QueryString["action"] != null)
        {
            string str1 = "";
            string rvalue = "000";//000/001未上传成功，002扩展名不正确
            if (context.Request.QueryString["action"].ToString() == "add")
            {
                try
                {
                    HttpFileCollection fileList = context.Request.Files;
                    string uri;
                    if (context.Request.QueryString["uri"] == null)
                    {
                        rvalue = "000";
                    }
                    uri = context.Request.QueryString["uri"].ToString();
                    for (int i = 0; i < fileList.Count; i++)
                    {
                        HttpPostedFile hPostedFile = fileList[i];
                        string filename = "";
                        if (hPostedFile.ContentLength > 0 && hPostedFile.FileName.Length > 0)
                        {
                            filename = hPostedFile.FileName;
                            int k = filename.LastIndexOf(".");
                            int j = filename.LastIndexOf("\\");
                            string type = filename.Substring(k);
                            filename = filename.Substring(j + 1);
                            DateTime datetime1 = System.DateTime.Now;
                            type = type.ToLower();
                            if (File.Exists("~\\upload\\" + uri + filename))
                            {
                                rvalue = "005";
                            }
                            else
                            {
                                if (SecurityConfig.EnclosureExtension.IndexOf(type) < 0)
                                {
                                    rvalue = "002";
                                    break;
                                }
                                hPostedFile.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~\\upload\\" + uri + filename));
                                if (str1 == "")
                                {
                                    str1 = filename;
                                }
                                else
                                {
                                    str1 += "," + filename;
                                }
                            }
                        }
                    }
                    if (str1.Length > 0)
                    {
                        rvalue = str1;
                    }
                }
                catch (Exception e)
                {
                    ErrorLogger.LogError(e);
                    rvalue = "001";
                }
                context.Response.Expires = -1;
                context.Response.Clear();
                context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                context.Response.ContentType = "text/plain";
                context.Response.Write("@returnstart@" + rvalue + "@returnend@");
                context.Response.End();
            }
            else if (context.Request.QueryString["action"].ToString() == "delete")
            {
                if (context.Request.QueryString["uri"] != null && context.Request.QueryString["filename"] != null)
                {
                    string uri = context.Request.QueryString["uri"].ToString();
                    string filename = context.Request.QueryString["filename"].ToString();
                    string path = HttpContext.Current.Server.MapPath("~\\upload\\" + uri + filename);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                        context.Response.Write("附件" + filename + "删除成功！");
                    }
                    else
                    {
                        context.Response.Write("未删除指定附件，附件不存在！");
                    }
                }
            }
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}