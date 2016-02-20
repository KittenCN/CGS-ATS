using RM.Common.DotNetCode;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RM.DataBase.DataBase.Common
{
    /// <summary>
    /// IDbHelperExpand
    /// 数据库访问扩展接口
    /// 版本：1.0
    public class IDbHelperExpand
    {
        /// <summary>
        /// 创建系统异常日志
        /// </summary>
        protected LogHelper Logger = new LogHelper("IDbHelperExpand");

        /// <summary>
        /// 利用Net SqlBulkCopy 批量导入数据库,速度超快
        /// </summary>
        /// <param name="dataTable">源内存数据表</param>
        public bool MsSqlBulkCopyData(DataTable dt, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlTransaction trans = conn.BeginTransaction();

                    SqlBulkCopy sqlbulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);
                    // 设置源表名称
                    sqlbulkCopy.DestinationTableName = dt.TableName;
                    //分几次拷贝
                    //sqlbulkCopy.BatchSize = 10;
                    // 设置超时限制
                    sqlbulkCopy.BulkCopyTimeout = 1000;
                    foreach (DataColumn dtColumn in dt.Columns)
                    {
                        sqlbulkCopy.ColumnMappings.Add(dtColumn.ColumnName, dtColumn.ColumnName);
                    }
                    try
                    {
                        // 写入
                        sqlbulkCopy.WriteToServer(dt);
                        // 提交事务
                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        sqlbulkCopy.Close();
                        return false;
                    }
                    finally
                    {
                        sqlbulkCopy.Close();
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog("-----------利用Net SqlBulkCopyData 批量导入数据库,速度超快-----------\r\n" + e.Message + "\r\n");
                return false;
            }
        }
    }
}
