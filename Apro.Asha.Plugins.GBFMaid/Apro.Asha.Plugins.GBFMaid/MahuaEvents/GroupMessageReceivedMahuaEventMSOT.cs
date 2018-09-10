using Newbe.Mahua;
using Newbe.Mahua.MahuaEvents;
using System;
using Apro.Asha.Plugins.GBFMaid.TSaver;

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
            //默认关闭的复读机,测试用
            if (ASwitchBox.复读模式)
            {
                复读机(context);
            }

            // 不要忘记在MahuaModule中注册
        }

        private void 复读机(GroupMessageReceivedContext context)
        {
            if (context.FromQq == "670004272" || context.FromQq == "975116202")
            {
                _mahuaApi.SendGroupMessage(context.FromGroup)
                    .Text(context.Message)
                    .Done();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

    }
}
