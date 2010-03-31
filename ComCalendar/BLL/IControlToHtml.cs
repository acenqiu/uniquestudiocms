using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.ComCalendar.BLL
{
    public interface IControlToHtml<T>
    {
        string ConvertToHtml(DataControl<T> control);
    }
}
