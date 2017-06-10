using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Collections;

using IP地址切换器.DataAccessLayer;

namespace IP地址切换器.BusinessLogicLayer
{
    class IP_BLL
    {
        public IPAccessHelper IPah=new IPAccessHelper ();

        //查询方案（所有行一列）
        public DataTable SelectWays(string Table, string Way)
        {
            return IPah.SelectOneCol(Table, Way);
        }

        //查询方案（一行所有列）
        public DataRow SelectAllIP(string Table, string Way, string Value)
        {
            return IPah.SelectAllColsOneRow(Table, Way, Value);
        }

        //插入一个方案（一行所有列）
        public bool InsertIPRow(string Table, Hashtable ht)
        {
            return IPah.Insert(Table, ht);
        }

        //更新一个方案（一行未知列）
        public bool UpdateIPRow(string Table, Hashtable ht, string Way,string Value)
        {
            return IPah.Update(Table, ht, Way, Value); 
        }
        //删除一个方案（一行所有列）
        public bool DelIPRow(string Table, string Way, string Value)
        {
            return IPah.Delete(Table, Way, Value);
        }
    }
}
