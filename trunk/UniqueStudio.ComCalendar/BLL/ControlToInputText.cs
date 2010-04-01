using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Text;
using UniqueStudio.ComCalendar.Model;

namespace UniqueStudio.ComCalendar.BLL
{
    public class ControlToInputText : IControlToHtml<CalendarNotice>
    {

        public string prototype = " <td><label>{2}</label><input name='{0}'  type='text' class='label-input' value='{1}' /></td>";
        public string ConvertToHtml(DataControl<CalendarNotice> control)
        {
            StringBuilder sb = new StringBuilder();
            string trId = "<tr id='{0}'>";
            sb.Append(String.Format(trId, control.ID));
            for (int i = 0; i < control.Count; i++)
            {
                string name = control.PropertyName[i];
                string value = control.GetValue(name);
                string s = String.Format(prototype, new string[] { name, value, value });
                sb.Append(s);
            }
            sb.Append("<td ><span class='edit-row'><a href='#' onclick='editRow(getRowFromLink(this))' class='edit-button'>编辑</a><a href='#' onclick='deleteRow(getRowFromLink(this))' class='delete-button'>删除</a></span><span class='save-or-cancel'><a href='#' onclick='saveRow(getRowFromLink(this))' class='save-button'>保存</a><a href='#' onclick='cancelRow(getRowFromLink(this))' class='cancel-button'>取消</a></span></td>");
            sb.Append("</tr>");
            return sb.ToString();
        }
    }
}
