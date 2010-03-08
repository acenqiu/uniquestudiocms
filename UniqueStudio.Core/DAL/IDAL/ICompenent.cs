using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    internal interface ICompenent
    {
        CompenentInfo CreateCompenent(CompenentInfo compenent);

        CompenentCollection GetAllCompenents();

        bool DeleteCompenent(int compenentId);
    }
}
