using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    [Serializable]
    public class CategoryCollection : List<CategoryInfo>
    {
        public CategoryCollection()
            : base()
        {
        }

        public CategoryCollection(int capacity)
            : base(capacity)
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (CategoryInfo item in this)
            {
                sb.Append(item.CategoryName + " ");
            }
            return sb.ToString();
        }
    }
}
