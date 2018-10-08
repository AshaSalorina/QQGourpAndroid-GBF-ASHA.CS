using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apro.Asha.Plugins.GBFMaid.TSaver
{
    /// <summary>
    /// 连接数据库的接口
    /// </summary>
    internal interface IDBHealper
    {
        IDbConnection DBConnection();

        void DBRead();

        void DBInsert();

        void DBUpdate();
    }
}