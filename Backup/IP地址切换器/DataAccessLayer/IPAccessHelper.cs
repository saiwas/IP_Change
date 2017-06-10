using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace IP地址切换器.DataAccessLayer
{
    class IPAccessHelper
    {
        Database db = new Database();
        /// <summary>
        /// 查询一行(所有列）
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Col">列名</param>
        /// <param name="Value">值</param>         
        /// <returns>DataTable</returns>
        public DataRow SelectAllColsOneRow(string Table,string Col,string Value)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from ");
            strSql.Append(Table);
            strSql.Append(" where ");
            strSql.Append(Col);
            strSql.Append("='");
            strSql.Append(Value);
            strSql.Append("'");

            DataRow dr=db.GetDataRow(strSql.ToString ());
            return dr;
        }
        /// <summary>
        /// 查询一列（所有行）
        /// </summary>
        /// <param name="Col">列名</param>
        /// <param name="Table">表名</param>
        /// <returns>OleDbDataReader</returns>
        public DataTable SelectOneCol(string Table, string Col)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(Col);
            strSql.Append(" from ");
            strSql.Append(Table);

            DataTable dr = db.GetDataTable(strSql.ToString());
            return dr;
        }
        /// <summary>
        /// 插入一行
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Cols">哈西表，键值为字段名，值为字段值</param>
        /// <returns>bool</returns>
        public bool Insert(string Table, Hashtable Cols)
        {
            int Count = 0;
            if (Cols.Count <= 0)
            {
                return false;
            }
            string Fields = " (";
            string Values = " Values(";
            foreach (DictionaryEntry item in Cols)
            {
                if (Count != 0)//第一个不需要添加逗号insert into userlist (username,password) values ('"&username&"','"&pass&"')
                {
                    Fields += ",";
                    Values += ",";
                }
                Fields += "[" + item.Key.ToString() + "]";
                Values += item.Value.ToString();
                Count++;
            }
            Fields += ")";
            Values += ")";
            string strSql = "Insert into " + Table + Fields + Values;
            return Convert .ToBoolean(db.ExecuteSQL(strSql.ToString()));
        }
        /// <summary>
        /// 更新一行
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Cols">哈西表，键值为字段名，值为字段值</param>
        /// <param name="Where">列名</param>
        /// <returns>bool</returns>
        public bool Update(string Table, Hashtable Cols, string Col,string Value)
        {
            int Count = 0;
            if (Cols.Count <= 0)
            {
                return true;
            }
            string Fields = " ";
            foreach (DictionaryEntry item in Cols)
            {
                if (Count != 0)
                {
                    Fields += ",";
                }
                Fields += "[" + item.Key.ToString() + "]";
                Fields += "=";
                Fields += item.Value.ToString();
                Count++;
            }
            Fields += " ";
            string strSql = "Update " + Table + " Set " + Fields + " Where "+ Col +" = "+"'"+Value+"'";
            return Convert.ToBoolean(db.ExecuteSQL(strSql.ToString()));
        }
        /// <summary>
        /// 删除一行
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Col">列名</param>
        /// <param name="Value">值</param>
        /// <returns></returns>
        public bool Delete(string Table, string Col, string Value)
        {
            bool success=false;
            string strSql = "Delete From " + Table + " Where " + Col + "=" + "'" + Value + "'";
            try
            {
                if (db.ExecuteSQL(strSql) == 1)
                    success = true;
            }
            catch
            {                
                success = false;
            }
            return success;
        }
        /// <summary>
        /// 某行是否存在
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Col">列名</param>
        /// <param name="Value">值</param>
        /// <returns>bool</returns>
        public bool IsExist(string Table, string Col, string Value)
        {
            bool success = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(Col) from ");
            strSql.Append(Table);
            strSql.Append(" where ");
            strSql.Append(Col);
            strSql.Append("=");
            strSql.Append(Value);
            try
            {
                if (db.ExecuteSQL(strSql.ToString ())==0)
                {
                    success = false;
                }
            }
            catch
            {                
                success = false;
            }
            return success;
        }

        /// <summary>
        /// 某行是否存在(多条件)
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Cols">哈西表，键值为字段名，值为字段值</param>
        /// <returns>bool</returns>
        public bool IsExist(string Table, Hashtable Cols)
        {
            bool success = true;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from ");
            strSql.Append(Table);
            strSql.Append(" where ");
            int Count = 0;
            foreach (DictionaryEntry item in Cols)
            {
                if (Count != 0)//select count(*) from table where A=a and B=b
                {
                    strSql.Append("and");
                }
                strSql.Append(item.Key.ToString());
                strSql.Append("=");
                strSql.Append(item.Value.ToString());
                Count++;
            }
            try
            {
                if (db.ExecuteSQL(strSql.ToString()) == 0)
                {
                    success = false;
                }
            }
            catch
            {
                success = false;
            }
            return success;
        }
    }
}
