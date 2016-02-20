using System;
using System.Collections.Generic;
using System.Text;
using RM.Common.DotNetCode;
using System.Collections;
using System.Data;
using RM.Common.DotNetData;
using System.Data.Common;
using RM.DataBase.DataBase.Common;
using RM.DataBase;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using RM.Common.DotNetEncrypt;
using RM.Common.DotNetConfig;
using System.Diagnostics;

namespace PDA_Service.DataBase.DataBase.SqlServer
{
    /// <summary>
    /// 数据库通用操作层(微软企业库5.1)
    /// 版本：2.0
    /// </summary>
    public class SqlServerHelper : IDbHelper, IDisposable
    {
        /// <summary>
        /// 创建系统异常日志
        /// </summary>
        protected LogHelper Logger = new LogHelper("SQLServerLog");

        #region DbCommand 命令
        //命令
        private DbCommand dbCommand = null;
        /// <summary>
        /// 命令
        /// </summary>
        public DbCommand DbCommand
        {
            get
            {
                return this.dbCommand;
            }
            set
            {
                this.dbCommand = value;
            }
        }
        #endregion

        #region 数据库连接字符串
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string connectionString = "";
        public SqlServerHelper(string connString)
        {
            connectionString = connString;
        }
        private static Object locker = new Object();

        private SqlDatabase db = null;
        /// <summary>
        /// 取得单身实例
        /// </summary>
        public SqlDatabase GetDatabase()
        {
            //在并发时，使用单一对象
            if (db == null)
            {
                if (ConfigHelper.GetAppSettings("ConStringEncrypt") == "true")//判断是否加密
                    db = new SqlDatabase(DESEncrypt.Decrypt(connectionString));
                else
                    db = new SqlDatabase(connectionString);
                return db;
            }
            else
            {
                lock (locker)
                {
                    return db;
                }
            }
        }
        #endregion

        #region 通过参数类构造键值
        /// <summary>
        /// 通过参数类构造键值
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <param name="_params">参数化</param>
        protected void AddInParameter(DbCommand cmd, SqlParam[] _params)
        {
            if (_params == null) return;
            foreach (SqlParam _param in _params)
            {
                DbType type = DbType.AnsiString;
                if (_param.FiledValue is DateTime)
                    type = DbType.DateTime;
                this.GetDatabase().AddInParameter(cmd, _param.FieldName.Replace(":", "@"), type, _param.FiledValue);
            }
        }
        /// <summary>
        /// 通过Hashtable对象构造键值
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <param name="_params">参数化</param>
        protected void AddInParameter(DbCommand cmd, Hashtable ht)
        {
            if (ht == null) return;
            foreach (string key in ht.Keys)
            {
                if (key == "Msg")
                {
                    this.GetDatabase().AddOutParameter(cmd, "@" + key, DbType.AnsiString, 1000);
                }
                else
                {
                    this.GetDatabase().AddInParameter(cmd, "@" + key, DbType.AnsiString, ht[key]);
                }
            }
        }

        /// <summary>
        /// 通过Hashtable对象构造键值
        /// </summary>
        /// <param name="cmd">SQL命令</param>
        /// <param name="_params">参数化</param>
        protected void AddMoreParameter(DbCommand cmd, Hashtable ht)
        {
            if (ht == null) return;
            foreach (string key in ht.Keys)
            {
                if (key.StartsWith("OUT_"))
                {
                    string tmp = key.Remove(0, 4);
                    this.GetDatabase().AddOutParameter(cmd, "@" + tmp, DbType.AnsiString, 1000);
                }
                else
                {
                    this.GetDatabase().AddInParameter(cmd, "@" + key, DbType.AnsiString, ht[key]);
                }
            }
        }
        #endregion

        #region 对象参数转换
        /// <summary>
        /// 对象参数转换
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public SqlParam[] GetParameter(Hashtable ht)
        {
            SqlParam[] _params = new SqlParam[ht.Count];
            int i = 0;
            foreach (string key in ht.Keys)
            {
                _params[i] = new SqlParam("@" + key, ht[key]);
                i++;
            }
            return _params;
        }
        #endregion

        #region 根据SQL返回影响行数
        /// <summary>
        /// 根据SQL返回影响行数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns></returns>
        public object GetObjectValue(StringBuilder sql)
        {
            return this.GetObjectValue(sql, null);
        }

        /// <summary>
        /// 根据SQL返回影响行数,带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        public object GetObjectValue(StringBuilder sql, SqlParam[] param)
        {
            try
            {
                dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                this.AddInParameter(dbCommand, param);
                return db.ExecuteScalar(dbCommand);
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------根据SQL返回影响行数-----------\r\n" + sql.ToString() + "\r\n" + e.Message + "\r\n");
                return null;
            }
        }
        #endregion

        #region 根据SQL执行
        /// <summary>
        ///  根据SQL执行
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>object</returns>
        public int ExecuteBySql(StringBuilder sql)
        {
            return this.ExecuteBySql(sql, null);
        }

        /// <summary>
        ///  根据SQL执行,带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>object</returns>
        public int ExecuteBySql(StringBuilder sql, SqlParam[] param)
        {
            int num = 0;
            try
            {
                dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                this.AddInParameter(dbCommand, param);
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    try
                    {
                        num = db.ExecuteNonQuery(dbCommand, trans);
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        num = -1;
                        Logger.WriteLog("-----------根据SQL执行,回滚事物-----------\r\n" + sql.ToString() + "\r\n" + e.Message + "\r\n返回值" + num + "\r\n");
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------执行sql语句服务器连接失败-----------\r\n" + e.Message + "\r\n返回值" + num + "\r\n");
            }
            return num;
        }
        /// <summary>
        /// 批量执行sql语句
        /// </summary>
        /// <param name="sqls"></param>
        /// <param name="m_param"></param>
        /// <returns></returns>
        public int BatchExecuteBySql(StringBuilder[] sqls, object[] param)
        {
            int num = 0;
            StringBuilder sql_Log = new StringBuilder();
            try
            {
                using (DbConnection connection = this.GetDatabase().CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        for (int i = 0; i < sqls.Length; i++)
                        {
                            StringBuilder builder = sqls[i];
                            sql_Log.Append(builder + "\r\n");
                            if (builder != null)
                            {
                                SqlParam[] paramArray = (SqlParam[])param[i];
                                DbCommand sqlStringCommand = this.db.GetSqlStringCommand(builder.ToString());
                                this.AddInParameter(sqlStringCommand, paramArray);
                                num = this.db.ExecuteNonQuery(sqlStringCommand, transaction);
                            }
                        }
                        transaction.Commit();
                        connection.Close();
                        return num;
                    }
                    catch (Exception exception)
                    {
                        num = -1;
                        transaction.Rollback();
                        this.Logger.WriteLog("-----------批量执行sql语句-----------\r\n" + sql_Log.ToString() + "\r\n" + exception.Message + "\r\n返回值" + num + "\r\n");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------批量执行sql语句服务器连接失败-----------\r\n" + e.Message + "\r\n返回值" + num + "\r\n");
            }
            return num;
        }
        #endregion

        #region 根据 SQL 返回 DataTable 数据集
        /// <summary>
        /// 获取数据表，没有条件
        /// </summary>
        /// <param name="TargetTable">目标表名</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTable(string TargetTable)
        {
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.Append("SELECT * FROM " + TargetTable + "");
                dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                return ReaderToIListHelper.DataTableToIDataReader(db.ExecuteReader(dbCommand));
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------获取数据集DataTable-----------\r\n" + sql.ToString() + "\r\n" + e.Message + "\r\n");
                return null;
            }
        }
        /// <summary>
        /// 获取数据表,排序
        /// </summary>
        /// <param name="TargetTable">目标表名</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <returns></returns>
        public DataTable GetDataTable(string TargetTable, string orderField, string orderType)
        {
            StringBuilder sql = new StringBuilder();
            try
            {
                sql.Append("SELECT * FROM " + TargetTable + " ORDER BY " + orderField + " " + orderType + "");
                dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                return ReaderToIListHelper.DataTableToIDataReader(db.ExecuteReader(dbCommand));
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------获取数据集DataTable-----------\r\n" + sql.ToString() + "\r\n" + e.Message + "\r\n");
                return null;
            }
        }
        /// <summary>
        /// 根据 SQL 返回 DataTable 数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableBySQL(StringBuilder sql)
        {
            return this.GetDataTableBySQL(sql, null);
        }
        /// <summary>
        /// 根据 SQL 返回 DataTable 数据集，带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>DataTable</returns>
        public DataTable GetDataTableBySQL(StringBuilder sql, SqlParam[] param)
        {
            try
            {
                dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                this.AddInParameter(dbCommand, param);
                return ReaderToIListHelper.DataTableToIDataReader(db.ExecuteReader(dbCommand));
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------获取数据集DataTable-----------\r\n" + sql.ToString() + "\r\n" + e.Message + "\r\n");
                return null;
            }
        }
        /// <summary>
        /// 摘要:
        ///     执行一存储过程DataTable
        /// 参数：
        ///     procName：存储过程名称
        ///     Hashtable：传入参数字段名
        /// </summary>
        public DataTable GetDataTableProc(string procName, Hashtable ht)
        {
            try
            {
                dbCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddInParameter(dbCommand, ht);
                using (DbConnection conn = db.CreateConnection())
                {
                    conn.Open();
                    DbTransaction trans = conn.BeginTransaction();
                    return ReaderToIListHelper.DataTableToIDataReader(db.ExecuteReader(dbCommand));
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------执行一存储过程DataTable-----------\r\n" + procName.ToString() + "\r\n" + e.Message + "\r\n");
                return null;
            }
        }
        /// <summary>
        /// 执行一存储过程返回数据表 返回多个值
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="rs">Hashtable</param>
        public DataTable GetDataTableProcReturn(string procName, Hashtable ht, ref Hashtable rs)
        {
            try
            {
                dbCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddMoreParameter(dbCommand, ht);
                DataSet ds = db.ExecuteDataSet(dbCommand);
                rs = new Hashtable();
                foreach (string key in ht.Keys)
                {
                    if (key.StartsWith("OUT_"))
                    {
                        string tmp = key.Remove(0, 4);
                        object val = db.GetParameterValue(dbCommand, "@" + tmp);
                        rs[key] = val;
                    }
                }
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------执行一存储过程DataTable返回多个值-----------\r\n" + procName.ToString() + rs + "\r\n" + e.Message + "\r\n");
                return null;
            }
        }
        #endregion

        #region 根据 SQL 返回 DataSet 数据集
        /// <summary>
        /// 根据 SQL 返回 DataSet 数据集
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSetBySQL(StringBuilder sql)
        {
            return GetDataSetBySQL(sql, null);
        }
        /// <summary>
        /// 根据 SQL 返回 DataSet 数据集，带参数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSetBySQL(StringBuilder sql, SqlParam[] param)
        {
            try
            {
                dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
                this.AddInParameter(dbCommand, param);
                return db.ExecuteDataSet(dbCommand);
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------获取数据集DataSet-----------\n" + sql.ToString() + "\n" + e.Message);
                return null;
            }
        }
        #endregion

        #region 根据 SQL 返回 IList
        /// <summary>
        /// 根据 SQL 返回 IList
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        public IList GetDataListBySQL<T>(StringBuilder sql)
        {
            return this.GetDataListBySQL<T>(sql, null);
        }
        /// <summary>
        /// 根据 SQL 返回 IList,带参数 (比DataSet效率高)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        public IList GetDataListBySQL<T>(StringBuilder sql, SqlParam[] param)
        {
            IList list = new List<T>();
            dbCommand = this.GetDatabase().GetSqlStringCommand(sql.ToString());
            this.AddInParameter(dbCommand, param);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                list = ReaderToIListHelper.ReaderToList<T>(dataReader);
            }
            return list;
        }
        #endregion

        #region 调用存储过程
        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">参数化</param>
        public int ExecuteByProc(string procName, Hashtable ht)
        {
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddInParameter(storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand, transaction);
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        Logger.WriteLog("-----------执行存储过程-----------\r\n" + procName + "\r\n" + exception.Message + "\r\n");
                    }
                    connection.Close();
                    return num;
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------执行存储过程服务器连接失败-----------\r\n" + procName + "\r\n" + e.Message + "\r\n");
            }
            return num;
        }

        public int ExecuteByProcNotTran(string procName, Hashtable ht)
        {
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddInParameter(storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand);
                    }
                    catch (Exception exception)
                    {
                        Logger.WriteLog("-----------执行存储过程-----------\r\n" + procName + "\r\n" + exception.Message + "\r\n");
                    }
                    connection.Close();
                    return num;
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------执行存储过程服务器连接失败-----------\r\n" + procName + "\r\n" + e.Message + "\r\n");
            }
            return num;
        }
        /// <summary>
        /// 调用存储过程返回指定消息
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="msg">OutPut rs</param>
        public int ExecuteByProcReturn(string procName, Hashtable ht, ref Hashtable rs)
        {
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddMoreParameter(storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                    connection.Close();
                }
                rs = new Hashtable();
                foreach (string str in ht.Keys)
                {
                    if (str.StartsWith("OUT_"))
                    {
                        object parameterValue = this.db.GetParameterValue(storedProcCommand, "@" + str.Remove(0, 4));
                        rs[str] = parameterValue;
                    }
                }
                return num;
            }
            catch (Exception exception)
            {
                this.Logger.WriteLog("-----------执行存储过程返回指定消息-----------\n" + procName + "\n" + exception.Message);
            }
            return num;
        }
        /// <summary>
        /// 调用存储过程返回指定消息
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="ht">Hashtable</param>
        /// <param name="msg">OutPut Msg</param>
        public int ExecuteByProcReturnMsg(string procName, Hashtable ht, ref object msg)
        {
            int num = 0;
            try
            {
                DbCommand storedProcCommand = this.GetDatabase().GetStoredProcCommand(procName);
                this.AddInParameter(storedProcCommand, ht);
                using (DbConnection connection = this.db.CreateConnection())
                {
                    connection.Open();
                    DbTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        num = this.db.ExecuteNonQuery(storedProcCommand, transaction);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                    connection.Close();
                }
                msg = this.db.GetParameterValue(storedProcCommand, "@Msg");
            }
            catch (Exception exception)
            {
                this.Logger.WriteLog("-----------执行存储过程返回指定消息-----------\n" + procName + "\n" + exception.Message);
            }
            return num;

        }
        #endregion

        #region 增、删、改、查
        /// <summary>
        /// 表单提交：新增，修改
        ///     参数：
        ///     tableName:表名
        ///     pkName：字段主键
        ///     pkVal：字段值
        ///     ht：参数
        /// </summary>
        /// <returns></returns>
        public bool Submit_AddOrEdit(string tableName, string pkName, string pkVal, Hashtable ht)
        {
            if (string.IsNullOrEmpty(pkVal))
            {
                if (this.InsertByHashtable(tableName, ht) > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                if (this.UpdateByHashtable(tableName, pkName, pkVal, ht) > 0)
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 根据唯一ID获取Hashtable
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        public Hashtable GetHashtableById(string tableName, string pkName, string pkVal)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Select * From ").Append(tableName).Append(" Where ").Append(pkName).Append("=@ID");
            DataTable dt = this.GetDataTableBySQL(sb, new SqlParam[] { new SqlParam("@ID", pkVal) });
            return DataTableHelper.DataTableToHashtable(dt);
        }
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        public int IsExist(string tableName, string pkName, string pkVal)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Select Count(1) from " + tableName + "");
            strSql.Append(" where " + pkName + " = @" + pkName + "");
            SqlParam[] param = {
                                         new SqlParam("@"+pkName+"",pkVal)};
            return CommonHelper.GetInt(this.GetObjectValue(strSql, param));
        }

        /// <summary>
        /// 通过Hashtable插入数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <returns>int</returns>
        public virtual int InsertByHashtable(string tableName, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Insert Into ");
            sb.Append(tableName);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                sb_prame.Append("," + key);
                sp.Append(",@" + key);
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ")");
            int _object = this.ExecuteBySql(sb, this.GetParameter(ht));
            return _object;
        }
        /// <summary>
        /// 通过Hashtable插入数据 返回主键（针对整型主键返回）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <returns></returns>
        public int InsertByHashtableReturnPkVal(string tableName, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Declare @ReturnValue int Insert Into ");
            sb.Append(tableName);
            sb.Append("(");
            StringBuilder sp = new StringBuilder();
            StringBuilder sb_prame = new StringBuilder();
            foreach (string key in ht.Keys)
            {
                sb_prame.Append("," + key);
                sp.Append(",@" + key);
            }
            sb.Append(sb_prame.ToString().Substring(1, sb_prame.ToString().Length - 1) + ") Values (");
            sb.Append(sp.ToString().Substring(1, sp.ToString().Length - 1) + ") Set @ReturnValue=SCOPE_IDENTITY() Select @ReturnValue");
            object _object = this.GetObjectValue(sb, this.GetParameter(ht));
            return (_object == DBNull.Value) ? 0 : Convert.ToInt32(_object);
        }
        /// <summary>
        /// 通过Hashtable修改数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkValue"></param>
        /// <param name="ht">Hashtable</param>
        /// <returns>int</returns>
        public int UpdateByHashtable(string tableName, string pkName, string pkVal, Hashtable ht)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" Update ");
            sb.Append(tableName);
            sb.Append(" Set ");
            bool isFirstValue = true;
            foreach (string key in ht.Keys)
            {
                if (isFirstValue)
                {
                    isFirstValue = false;
                    sb.Append(key);
                    sb.Append("=");
                    sb.Append("@" + key);
                }
                else
                {
                    sb.Append("," + key);
                    sb.Append("=");
                    sb.Append("@" + key);
                }
            }
            sb.Append(" Where ").Append(pkName).Append("=").Append("@" + pkName);
            ht[pkName] = pkVal;
            SqlParam[] _params = this.GetParameter(ht);
            int _object = this.ExecuteBySql(sb, _params);
            return _object;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        public int DeleteData(string tableName, string pkName, string pkVal)
        {
            StringBuilder sb = new StringBuilder("Delete From " + tableName + " Where " + pkName + " = @ID");
            return this.ExecuteBySql(sb, new SqlParam[] { new SqlParam("@ID", pkVal) });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        public int BatchDeleteData(string tableName, string pkName, object[] pkValues)
        {
            SqlParam[] param = new SqlParam[pkValues.Length];
            int index = 0;
            string str = "@ID" + index;
            StringBuilder sql = new StringBuilder("DELETE FROM " + tableName + " WHERE " + pkName + " IN (");
            for (int i = 0; i < (param.Length - 1); i++)
            {
                object obj2 = pkValues[i];
                str = "@ID" + index;
                sql.Append(str).Append(",");
                param[index] = new SqlParam(str, obj2);
                index++;
            }
            str = "@ID" + index;
            sql.Append(str);
            param[index] = new SqlParam(str, pkValues[index]);
            sql.Append(")");
            return this.ExecuteBySql(sql, param);
        }
        #endregion

        #region 数据分页
        /// <summary>
        /// 摘要:
        ///     数据分页接口
        /// 参数：
        ///     sql：传入要执行sql语句
        ///     param：参数化
        ///     orderField：排序字段
        ///     orderType：排序类型
        ///     pageIndex：当前页
        ///     pageSize：页大小
        ///     count：返回查询条数
        /// </summary>
        public DataTable GetPageList(string sql, SqlParam[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int num = (pageIndex - 1) * pageSize;
                int num1 = (pageIndex) * pageSize;
                sb.Append("Select * From (Select ROW_NUMBER() Over (Order By " + orderField + " " + orderType + "");
                sb.Append(") As rowNum, * From (" + sql + ") As T ) As N Where rowNum > " + num + " And rowNum <= " + num1 + "");
                count = Convert.ToInt32(this.GetObjectValue(new StringBuilder("Select Count(1) From (" + sql + ") As t"), param));
                return this.GetDataTableBySQL(sb, param);
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------数据分页（Oracle）-----------\r\n" + sb.ToString() + "\r\n" + e.Message + "\r\n");
                return null; ;
            }
        }
        #endregion

        #region SqlBulkCopy 批量导入
        /// <summary>
        /// 利用Net OracleBulkCopy 批量导入数据库,速度超快
        /// </summary>
        /// <param name="dt">源内存数据表</param>
        /// <returns></returns>
        public bool SqlBulkCopyImport(DataTable dt)
        {
            IDbHelperExpand copy = new IDbHelperExpand();
            return copy.MsSqlBulkCopyData(dt, connectionString);
        }
        #endregion

        #region IDisposable 成员
        /// <summary>
        /// 内存回收
        /// </summary>
        public void Dispose()
        {
            if (this.dbCommand != null)
            {
                this.dbCommand.Dispose();
            }
        }
        #endregion
    }
}
