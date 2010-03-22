<%@ WebHandler Language="C#" Class="StopUpLoad" %>

using System;
using System.Web;

public class StopUpLoad : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("11");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}