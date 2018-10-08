using Newbe.Mahua;
using System.Collections.Generic;

namespace Apro.Asha.Plugins.GBFMaid
{
    public class MyMenuProvider : IMahuaMenuProvider
    {
        public IEnumerable<MahuaMenu> GetMenus()
        {
            return new[]
            {
                new MahuaMenu
                {
                    Id = "menu1",
                    Text = "切换复读"
                },
                new MahuaMenu
                {
                    Id = "menu2",
                    Text = "切换定时模式(启用多线程)"
                },
            };
        }
    }
}