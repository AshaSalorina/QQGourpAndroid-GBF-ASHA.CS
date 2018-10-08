using Apro.Asha.Plugins.GBFMaid.TSaver;
using Newbe.Mahua;
using Newbe.Mahua.MahuaEvents;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Apro.Asha.Plugins.GBFMaid.MahuaEvents
{
    /// <summary>
    /// 来自好友的私聊消息接收事件
    /// 因为无法进行try语句,所有的越界判断都需要自行解决,并尽可能采用没有异常抛出的函数.
    /// </summary>
    public class PrivateMessageFromFriendReceivedMahuaEventMSOT
        : IPrivateMessageFromFriendReceivedMahuaEvent
    {
        //内联数据类,用于重组ms消息
        private class MsgData
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

        private Dictionary<int, Thread> threadList = new Dictionary<int, Thread>();

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
                //确保群号的正确性
                if (!int.TryParse(strgp[1], out int group))
                {
                    _mahuaApi.SendPrivateMessage(context.FromQq)
                        .Text(@"群号似乎不正确哦?请使用!mt qq群号 msg h:m:ID这样的格式")
                        .Done();
                }
                else
                {
                    if (!_mahuaApi.GetGroups().Contains(group.ToString()))
                    {
                        _mahuaApi.SendPrivateMessage(context.FromQq)
                        .Text($@"{_mahuaApi.GetLoginNick()}似乎没有加入这个群呢...")
                        .Done();
                    }
                }
                string msg = strgp[2];
                //todo:需要对time进行再度分割
                string time = strgp[3];
                MsgData newMsg = new MsgData(group.ToString(), msg, time, context.FromQq);

                #region 线程池测试

                if (ASwitchBox.多线程启用)
                {
                    if (threadList.Count >= 100)
                    {
                        _mahuaApi.SendPrivateMessage(context.FromQq).Text("( ,,`･ω･´)ﾝﾝ任务已满哦~").Done();
                        return;
                    }
                    for (int i = 0; i < 100; i++)
                    {
                        if (threadList[i] == null)
                        {
                            Thread thd = new Thread(MtmsgThread);
                            threadList.Add(i, thd);
                            thd.Start(newMsg);
                            break;
                        }
                    }
                }
                else
                {
                    #region 不启用线程,不定时(仅供测试)

                    MtmsgThread(newMsg);

                    #endregion 不启用线程,不定时(仅供测试)
                }

                #endregion 线程池测试
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

            //Thread.Sleep(100);

            _mahuaApi.SendGroupMessage(source.group)
                //.AtlAll()
                .Text(source.msg)
                .Done();
            _mahuaApi.SendPrivateMessage(source.fromQq).Text("done").Done();
        }

        /// <summary>
        /// 收到 “!mtd int [msg]” 的消息后，移除ID为int的定时任务,其中msg为可选项,标注取消原因
        /// </summary>
        /// <param name="context"></param>
        private void MtDismsg(PrivateMessageFromFriendReceivedContext context)
        {
            var msgg = context.Message.Split(' ');
            if (msgg.Length < 2 || !int.TryParse(msgg[1], out int threadID))
            {
                _mahuaApi.SendPrivateMessage(context.FromQq)
                    .Text("参数似乎不正确哦?").Done();
                return;
            }
            string resout = "";
            if (msgg.Length > 2)
            {
                for (int i = 2; i < msgg.Length; i++)
                {
                    resout += msgg[i] + " ";
                }
            }
            if (threadID > 0 && threadID < 100)
            {
                _mahuaApi.SendPrivateMessage(context.FromQq)
                    .Text($@"{(resout == "" ? "" : $"因为{resout}")}已取消ID为{threadID}的定时任务")
                    .Done();
            }
        }
    }
}