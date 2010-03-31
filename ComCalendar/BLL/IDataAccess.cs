using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.ComCalendar.BLL
{
  public   interface IDataAccess<T>
    {
      void Delete(DataControl<T> control);
      void Update(DataControl<T> control);
      void Add(T t);
    }
}
