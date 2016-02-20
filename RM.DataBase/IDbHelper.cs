using System;
using System.Collections.Generic;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using System.Collections;

namespace RM.DataBase
{
    /// <summary>
    /// 数据库通用操作层接口
    /// 版本：2.0
    /// </summary>
    public interface IDbHelper
    {
        #region 根据SQL返回影响行数
        /// <summary>
        /// 根据SQL返回影响行数
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns></returns>
        object GetObjectValue(StringBuilder sql);
        /// <summary>
        /// 根据SQL返回影响行数,带参数
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        object GetObjectValue(StringBuilder sql, SqlParam[] param);
        #endregion

        #region 根据SQL执行
        /// <summary>
        /// 根据SQL执行
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns></returns>
        int ExecuteBySql(StringBuilder sql);
        /// <summary>
        /// 根据SQL执行,带参数
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        int ExecuteBySql(StringBuilder sql, SqlParam[] param);
        /// <summary>
        /// 批量执行sql语句,带参数
        /// </summary>
        /// <param name="sqls">SQL</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        int BatchExecuteBySql(StringBuilder[] sql, object[] param);
        #endregion

        #region 根据 SQL 返回 DataTable
        /// <summary>
        /// 获取数据集，没有条件
        /// </summary>
        /// <param name="TargetTable">目标表名</param>
        /// <returns>DataTable</returns>
        DataTable GetDataTable(string TargetTable);
        /// <summary>
        /// 获取数据集,排序
        /// </summary>
        /// <param name="TargetTable">目标表名</param>
        /// <param name="orderField">排序字段</param>
        /// <param name="orderType">排序类型</param>
        /// <returns></returns>
        DataTable GetDataTable(string TargetTable, string orderField, string orderType);
        /// <summary>
        /// 根据 SQL 返回 DataTable
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns></returns>
        DataTable GetDataTableBySQL(StringBuilder sql);
        /// <summary>
        /// 根据 SQL 返回 DataTable,带参数
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        DataTable GetDataTableBySQL(StringBuilder sql, SqlParam[] param);
        /// <summary>
        /// 摘要:
        ///     执行一存储过程DataTable
        /// 参数：
        ///     procName：存储过程名称
        ///     Hashtable：传入参数字段名
        /// </summary>
        DataTable GetDataTableProc(string procName, Hashtable ht);
        /// <summary>
        /// 摘要:
        ///     执行一存储过程DataTable 返回多个值
        /// 参数：
        ///     procName：存储过程名称
        ///     Hashtable：传入参数字段名
        ///     rs       :返回值
        /// </summary>
        DataTable GetDataTableProcReturn(string procName, Hashtable ht, ref Hashtable rs);
        #endregion

        #region 根据 SQL 返回 DataSet
        /// <summary>
        /// 根据 SQL 返回 DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DataSet</returns>
        DataSet GetDataSetBySQL(StringBuilder sql);
        /// <summary>
        /// 根据 SQL 返回 DataSet
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        DataSet GetDataSetBySQL(StringBuilder sql, SqlParam[] param);
        #endregion

        #region 根据 SQL 返回 IList
        /// <summary>
        /// 根据 SQL 返回 IList
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        IList GetDataListBySQL<T>(StringBuilder sql);
        /// <summary>
        /// 根据 SQL 返回 IList,带参数 (比DataSet效率高)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数化</param>
        /// <returns></returns>
        IList GetDataListBySQL<T>(StringBuilder sql, SqlParam[] param);
        #endregion

        #region 存储过程
        /// <summary>
        /// 摘要:
        ///     执行一存储过程返回标识
        /// 参数：
        ///     procName：存储过程名称
        ///     Hashtable：传入参数字段名
        /// </summary>
        int ExecuteByProc(string procName, Hashtable ht);
        /// <summary>
        /// 执行一存储过程返回标识(不带事务)
        /// </summary>
        int ExecuteByProcNotTran(string procName, Hashtable ht);
        /// <summary>
        /// 执行存储过程返回指定消息
        /// </summary>
        int ExecuteByProcReturnMsg(string procName, Hashtable ht, ref object rs);
        /// <summary>
        /// 执行存储过程返回多个值
        /// </summary>
        int ExecuteByProcReturn(string procName, Hashtable intputHt, ref Hashtable outputHt);
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
        bool Submit_AddOrEdit(string tableName, string pkName, string pkVal, Hashtable ht);
        /// <summary>
        /// 根据唯一ID获取Hashtable
        /// </summary>
        Hashtable GetHashtableById(string tableName, string pkName, string pkVal);
        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        int IsExist(string tableName, string pkName, string pkVal);
        /// <summary>
        /// 通过Hashtable插入数据
        /// </summary>
        int InsertByHashtable(string tableName, Hashtable ht);
        /// <summary>
        /// 通过Hashtable插入数据 返回主键（针对整型主键返回）
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="ht">Hashtable</param>
        /// <returns></returns>
        int InsertByHashtableReturnPkVal(string tableName, Hashtable ht);
        /// <summary>
        /// 通过Hashtable修改数据
        /// </summary>
        int UpdateByHashtable(string tableName, string pkName, string pkVal, Hashtable ht);
        /// <summary>
        /// 删除数据
        /// </summary>
        int DeleteData(string tableName, string pkName, string pkVal);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="pkName">字段主键</param>
        /// <param name="pkVal">字段值</param>
        /// <returns></returns>
        int BatchDeleteData(string tableName, string pkName, object[] pkValues);
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
        DataTable GetPageList(string sql, SqlParam[] param, string orderField, string orderType, int pageIndex, int pageSize, ref int count);
        #endregion

        #region SqlBulkCopy 批量导入数据库
        /// <summary>
        /// 利用Net SqlBulkCopyImport 批量导入数据库,速度超快
        /// </summary>
        /// <param name="dt">源内存数据表</param>
        /// <returns></returns>
        bool SqlBulkCopyImport(DataTable dt);
        #endregion
    }
}
