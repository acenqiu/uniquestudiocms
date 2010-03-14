using System;
using System.Collections.Generic;
using System.Text;

using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 组件管理提供类需实现的方法
    /// </summary>
    internal interface ICompenent
    {

        /// <summary>
        /// 创建组件
        /// </summary>
        /// <remarks>在数据库中插入组件记录</remarks>
        /// <param name="compenent">组件信息</param>
        /// <returns>如果创建成功，返回组件信息，否则返回空</returns>
        CompenentInfo CreateCompenent(CompenentInfo compenent);

        /// <summary>
        /// 返回所有组件的信息
        /// </summary>
        /// <returns>包含所有信息的组件集合</returns>
        CompenentCollection GetAllCompenents();

        /// <summary>
        /// 删除组件
        /// </summary> 
        /// <remarks>在数据库中删除组件信息</remarks>
        /// <param name="compenentId">待删除组件ID</param>
        /// <returns>是否删除成功</returns>
        bool DeleteCompenent(int compenentId);
    }
}
