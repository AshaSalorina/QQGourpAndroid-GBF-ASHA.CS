using Newbe.Mahua;
using Newbe.Mahua.MahuaEvents;
using System;
using System.Timers;

namespace Apro.Asha.Plugins.GBFMaid.MahuaEvents
{
    /// <summary>
    /// 来自好友的私聊消息接收事件
    /// </summary>
    public class PrivateMessageFromFriendReceivedMahuaEventMSOT
        : IPrivateMessageFromFriendReceivedMahuaEvent
    {
        private readonly IMahuaApi _mahuaApi;

        public PrivateMessageFromFriendReceivedMahuaEventMSOT(
            IMahuaApi mahuaApi)
        {
            _mahuaApi = mahuaApi;
        }

        public void ProcessFriendMessage(PrivateMessageFromFriendReceivedContext context)
        {
            // todo 填充处理逻辑
            //throw new NotImplementedException();
            var str = context.Message;
            var strgp = str.Split(' ');
            string ctrl = strgp[0];

            switch (ctrl)
            {
                case "!mt":
                    //!mt 
                    Mtmsg(context);
                    break;
                default:
                    _mahuaApi.SendPrivateMessage(context.FromQq).Text("( ,,`･ω･´)ﾝﾝﾝ？").Done();
                    break;
            }

            // 不要忘记在MahuaModule中注册
        }

        /// <summary>
        /// 收到 “!mt qq群号 msg h:m:ID” 的消息后，启动定时任务,到达每天的h:m时发送msg消息
        /// 该任务将注册ID,注册ID重复会报出提示,预计留100条消息位
        /// Warning:这个部分应当设计为异步调用,现在仅仅只做测试,后面进行修改
        /// </summary>
        private void Mtmsg(PrivateMessageFromFriendReceivedContext context)
        {
            try
            {
                var str = context.Message;
                var strgp = str.Split(' ');

                //分割消息信息和时间
                string group = strgp[1];
                string msg = strgp[2];
                //todo:需要对time进行再度分割
                string time = strgp[3];

                //创建定时器
                Timer timer = new Timer();
                timer.Enabled = true;
                timer.Interval = 1000;
                //每隔一分钟判断
                timer.Elapsed += new ElapsedEventHandler((object source, ElapsedEventArgs e) =>
                {
                    _mahuaApi.SendGroupMessage(group)
                        //.AtlAll()
                        .Text(msg)
                        .Done();
                });
            }
            catch (Exception)
            {
                _mahuaApi.SendPrivateMessage(context.FromQq).Text("( ,,`･ω･´)ﾝﾝﾝ？").Done();
            }

        }


    }
}
