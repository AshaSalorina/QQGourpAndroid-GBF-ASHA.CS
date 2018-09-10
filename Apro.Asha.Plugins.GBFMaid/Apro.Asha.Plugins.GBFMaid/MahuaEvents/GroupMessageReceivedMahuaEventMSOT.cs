using Newbe.Mahua;
using Newbe.Mahua.MahuaEvents;
using System;

namespace Apro.Asha.Plugins.GBFMaid.MahuaEvents
{
    /// <summary>
    /// 群消息接收事件
    /// </summary>
    public class GroupMessageReceivedMahuaEventMSOT
        : IGroupMessageReceivedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public GroupMessageReceivedMahuaEventMSOT(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessGroupMessage(GroupMessageReceivedContext context)
        {
            // todo 填充处理逻辑
            //throw new NotImplementedException();
            if (context.FromQq == "670004272")
            {
                _mahuaApi.SendGroupMessage(context.FromGroup)
                    .Text(context.Message)
                    .Done();
            }
            else
            {
                throw new NotImplementedException();
            }
            // 不要忘记在MahuaModule中注册
        }
    }
}
