//=================================================================
// 版权所有：版权所有(c) 2010，联创团队
// 内容摘要：模块需实现的接口。
// 完成日期：2010年04月11日
// 版本：v1.0 alpha
// 作者：邱江毅
//=================================================================
using System.Collections.Specialized;

namespace UniqueStudio.Core.Module
{
    /// <summary>
    /// 模块需实现的接口。
    /// </summary>
    public interface IModule
    {
        string RenderContents(int siteId, string controlName, NameValueCollection queryString);
    }
}
