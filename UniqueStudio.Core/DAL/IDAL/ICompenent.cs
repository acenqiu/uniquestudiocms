using UniqueStudio.Common.Model;

namespace UniqueStudio.DAL.IDAL
{
    /// <summary>
    /// 组件管理提供类需实现的方法。
    /// </summary>
    internal interface ICompenent
    {
        /// <summary>
        /// 创建组件。
        /// </summary>
        /// <remarks>在数据库中插入组件记录。</remarks>
        /// <param name="compenent">组件信息。</param>
        /// <returns>如果创建成功，返回组件信息，否则返回空。</returns>
        CompenentInfo CreateCompenent(CompenentInfo compenent);

        /// <summary>
        /// 删除组件。
        /// </summary> 
        /// <remarks>在数据库中删除组件信息。</remarks>
        /// <param name="compenentId">待删除组件ID。</param>
        /// <returns>是否删除成功。</returns>
        bool DeleteCompenent(int compenentId);

        /// <summary>
        /// 返回所有组件的信息。
        /// </summary>
        /// <returns>包含所有信息的组件集合。</returns>
        CompenentCollection GetAllCompenents();

        /// <summary>
        /// 返回指定组件的配置信息。
        /// </summary>
        /// <param name="compenentId">组件ID。</param>
        /// <returns>配置信息。</returns>
        string GetCompenentConfig(int compenentId);

        /// <summary>
        /// 返回指定组件的配置信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <returns>配置信息。</returns>
        string GetCompenentConfig(int siteId, string compenentName);

        /// <summary>
        /// 返回指定组件是否存在。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <returns>是否存在。</returns>
        bool IsCompenentExists(int siteId, string compenentName);

        /// <summary>
        /// 更新组件配置信息。
        /// </summary>
        /// <param name="compenentId">组件ID。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否更新成功。</returns>
        bool UpdateCompenentConfig(int compenentId, string config);

        /// <summary>
        /// 更新组件配置信息。
        /// </summary>
        /// <param name="siteId">网站ID。</param>
        /// <param name="compenentName">组件名称。</param>
        /// <param name="config">配置信息。</param>
        /// <returns>是否更新成功。</returns>
        bool UpdateCompenentConfig(int siteId, string compenentName, string config);
    }
}
