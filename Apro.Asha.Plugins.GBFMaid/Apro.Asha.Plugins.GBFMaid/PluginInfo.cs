﻿using Newbe.Mahua;

namespace Apro.Asha.Plugins.GBFMaid
{
    /// <summary>
    /// 本插件的基本信息
    /// </summary>
    public class PluginInfo : IPluginInfo
    {
        /// <summary>
        /// 版本号， 主版本.次版本.修订号
        /// </summary>
        public string Version { get; set; } = "0.2.1";

        /// <summary>
        /// 插件名称
        /// </summary>

        public string Name { get; set; } = "GBFMaid";

        /// <summary>
        /// 作者名称
        /// </summary>
        public string Author { get; set; } = "AshaSalorina";

        /// <summary>
        /// 插件Id，用于唯一标识插件产品的Id，至少包含 AAA.BBB.CCC 三个部分
        /// </summary>
        public string Id { get; set; } = "Apro.Asha.Plugins.GBFMaid";

        /// <summary>
        /// 插件描述
        /// </summary>
        public string Description { get; set; } = "测试版本";
    }
}