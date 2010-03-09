﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UniqueStudio.Common.Model
{
    public class UserCollection:List<UserInfo>
    {
        private int amount;
        private int pageIndex;
        private int pageSize;

        public UserCollection()
            :base()
        {
        }

        public UserCollection(int capacity)
            : base(capacity)
        {
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