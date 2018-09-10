using Newbe.Mahua;
using Newbe.Mahua.MahuaEvents;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Apro.Asha.Plugins.GBFMaid.MahuaEvents
{
    /// <summary>
    /// 来自好友的私聊消息接收事件
    /// </summary>
    public class PrivateMessageFromFriendReceivedMahuaEventMSOT
        : IPrivateMessageFromFriendReceivedMahuaEvent
    {
        
        //内联数据类,用于重组ms消息
        class MsgData
        {
            public MsgData(string group, string msg, string time, string fq)
            {
                this.group = group;
                this.msg = msg;
                this.time = time;
                this.fromQq = fq;
            }

            public string fromQq;
            public string group;
            public string msg;
            public string time;
        }
        
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



        Dictionary<int, Thread> threadList = new Dictionary<int, Thread>();

        /// <summary>
        /// 收到 “!mt qq群号 msg h:m:ID” 的消息后，启动定时任务,到达每天的h:m时发送msg消息
        /// 该任务将注册ID,注册ID重复会报出提示,预计留100条消息位
        /// Warning:这个部分应当设计为异步调用,现在仅仅只做测试,后面进行修改
        /// </summary>
        private void Mtmsg(PrivateMessageFromFriendReceivedContext context)
        {
            var source = context;
            var strgp = source.Message.Split(' ');
            if (strgp.Length == 4)
            {
                //分割消息信息和时间
                string group = strgp[1];
                string msg = strgp[2];
                //todo:需要对time进行再度分割
                string time = strgp[3];
                MsgData newMsg = new MsgData(group, msg, time, context.FromQq);
                Thread thd = new Thread(MtmsgThread);
                threadList.Add(1,thd);
                thd.Start(newMsg);

            }
            else
            {
                _mahuaApi.SendPrivateMessage(source.FromQq).Text("( ,,`･ω･´)ﾝﾝﾝ？").Done();
            }           
        }

        private void MtmsgThread(object cont)
        {
            var source = (MsgData)cont;
            //test mod

            _mahuaApi.SendPrivateMessage(source.fromQq)
                .Text($"创建完成,将要发送{source.msg}到群{source.group}")
                .Done();

            //test mod done

            Thread.Sleep(100);

            _mahuaApi.SendGroupMessage(source.group)
                //.AtlAll()
                .Text(source.msg)
                .Done();
            _mahuaApi.SendPrivateMessage(source.fromQq).Text("done").Done();
        }



    }



}
