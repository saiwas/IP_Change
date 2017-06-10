using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Configuration;

namespace IP地址切换器.DataAccessLayer
{
    /// <summary>
    /// 类，用于数据访问的类。
    /// </summary>
    public class Database : IDisposable
    {
        /// <summary>
        /// 保护变量，数据库连接。
        /// </summary>
        protected OleDbConnection Connection;

        /// <summary>
        /// 保护变量，数据库连接串。
        /// </summary>
        protected string ConnectionString;

        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="DatabaseConnectionString">数据库连接串</param>
        public Database()
        {
            ConnectionString = IP地址切换器.Properties.Settings.Default.IPConnectionString;
        }

        /// <summary>
        /// 析构函数，释放非托管资源
        /// </summary>
        ~Database()
        {
            if (Connection != null)
                Connection.Close();
            Dispose();

        }

        /// <summary>
        /// 保护方法，打开数据库连接。
        /// </summary>
        protected void Open()
                {
            if (Connection == null)
            {
                Connection = new OleDbConnection(ConnectionString);
            }
            if (Connection.State.Equals(ConnectionState.Closed))
            {
                Connection.Open();
            }
        
        }

        /// <summary>
        /// 公有方法，关闭数据库连接。
        /// </summary>
        public void Close()
        {
                if (Connection != null)
                    Connection.Close();
        }

        /// <summary>
        /// 公有方法，释放资源。
        /// </summary>
        public void Dispose()
        {
            // 确保连接被关闭
                if (Connection != null)
                {
                    Connection.Dispose();
                    Connection = null;
                }
        }

        /// <summary>
        /// 公有方法，获取数据，返回一个OleDbDataReader （调用后主意调用OleDbDataReader.Close()）。
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>OleDbDataReader</returns>
        public OleDbDataReader GetDataReader(string SqlString)
        {
            Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand(SqlString, Connection);
                OleDbDataReader oddr=cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                return oddr;
            }
            catch
            {
               return null;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 公有方法，获取数据，返回一个DataSet。
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string SqlString)
        {
            DataSet dataset = new DataSet();
            Open();
            try
            {
                OleDbDataAdapter adapter = new OleDbDataAdapter(SqlString, Connection);
                adapter.Fill(dataset);
                return dataset;
            }
            catch 
            {
                return null;              
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 公有方法，获取数据，返回一个DataTable。
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string SqlString)
        {
            DataSet dataset = GetDataSet(SqlString);
            dataset.CaseSensitive = false;
            return dataset.Tables[0];
        }

        /// <summary>
        /// 公有方法，获取数据，返回一个DataRow。
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>DataRow</returns>
        public DataRow GetDataRow(string SqlString)
        {
            DataSet dataset = GetDataSet(SqlString);
            dataset.CaseSensitive = false;
            if (dataset.Tables[0].Rows.Count > 0)
            {
                return dataset.Tables[0].Rows[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 公有方法，执行Sql语句。
        /// </summary>
        /// <param name="SqlString">Sql语句</param>
        /// <returns>对Update、Insert、Delete为影响到的行数，其他情况为-1</returns>
        public int ExecuteSQL(string SqlString)
        {
            int count = 0;
            Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand(SqlString, Connection);
                count = cmd.ExecuteNonQuery();
            }
            catch
            {                
                count = 0;
            }
            finally
            {
                Close();
            }
            return count;
        }

        /// <summary>
        /// 公有方法，执行一组Sql语句。
        /// </summary>
        /// <param name="SqlStrings">Sql语句组</param>
        /// <returns>是否成功</returns>
        public bool ExecuteSQL(string[] SqlStrings)
        {
            bool success = true;
            Open();
            OleDbCommand cmd = new OleDbCommand();
            OleDbTransaction trans = Connection.BeginTransaction();
            cmd.Connection = Connection;
            cmd.Transaction = trans;

            int i = 0;
            try
            {
                foreach (string str in SqlStrings)
                {
                    cmd.CommandText = str;
                    cmd.ExecuteNonQuery();
                    i++;
                }
                trans.Commit();
            }
            catch
            {                
                success = false;
                trans.Rollback();
            }
            finally
            {
                Close();
            }
            return success;
        }
    }
}
