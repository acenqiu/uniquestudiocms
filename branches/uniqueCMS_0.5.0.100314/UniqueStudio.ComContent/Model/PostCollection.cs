using System;
using System.Collections.Generic;
using System.Text;


namespace UniqueStudio.ComContent.Model
{
    public class PostCollection:List<PostInfo>
    {
        private int amount;
        private int pageIndex;
        private int pageSize;

        public PostCollection()
            :base()
        {

        }

        public PostCollection(int pageSize)
            : base(pageSize)
        {
            this.pageSize = pageSize;
        }
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        public int PageCount
        {
            get { return (amount - 1) / pageSize + 1; }
        }
    }
}
