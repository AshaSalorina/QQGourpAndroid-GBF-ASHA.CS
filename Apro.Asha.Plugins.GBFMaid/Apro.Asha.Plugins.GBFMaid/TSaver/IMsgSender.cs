﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apro.Asha.Plugins.GBFMaid.TSaver
{
    /// <summary>
    /// 用于执行异步定时任务
    /// </summary>
    internal interface IMsgThreadTimer
    {
        Task BeginClockThread();

        Task EndClockThread();
    }
}