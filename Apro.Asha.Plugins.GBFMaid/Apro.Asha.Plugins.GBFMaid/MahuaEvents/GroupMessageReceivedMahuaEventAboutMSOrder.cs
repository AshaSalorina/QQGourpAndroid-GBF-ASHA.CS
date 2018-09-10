using Newbe.Mahua;
using Newbe.Mahua.MahuaEvents;
using System;

namespace Apro.Asha.Plugins.GBFMaid.MahuaEvents
{
    /// <summary>
    /// 群消息接收事件
    /// </summary>
    public class GroupMessageReceivedMahuaEventAboutMSOrder
        : IGroupMessageReceivedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public GroupMessageReceivedMahuaEventAboutMSOrder(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessGroupMessage(GroupMessageReceivedContext context)
        {
            // todo 填充处理逻辑
            //throw new NotImplementedException();
            if (context.Message == "姬塔zaima" && context.FromQq == "670004272")
            {
                _mahuaApi.SendGroupMessage(context.FromGroup)
                    .At(context.FromQq)
                    .Text("ここです！");
            }
            // 不要忘记在MahuaModule中注册
        }
    }
}
