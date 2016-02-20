using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace RM.DataBase.DataBase.Common
{
    /// <summary>
    /// SqlServer数据备份还原
    /// </summary>
    public class SqlServerBackup
    {
        public SqlServerBackup()
        {
        }
        /// <summary>
        /// 数据库备份
        /// </summary>
        private string database, server, uid, pwd;
        public string Database
        {
            set { database = value; }
            get { return database; }
        }
        public string Server
        {
            set { server = value; }
            get { return server; }
        }
        public string Pwd
        {
            set { pwd = value; }
            get { return pwd; }
        }
        public string Uid
        {
            set { uid = value; }
            get { return uid; }
        }
        /// <summary>
        /// 　备份　　
        /// </summary>
        /// <param name="url">备份的地址　　必须是绝对路径　</param>
        /// <returns></returns>
        public bool DbBackup(string url)
        {
            SQLDMO.Backup oBackup = new SQLDMO.BackupClass();
            SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
            try
            {
                oSQLServer.LoginSecure = false;
                oSQLServer.Connect(server, uid, pwd);
                oBackup.Action = SQLDMO.SQLDMO_BACKUP_TYPE.SQLDMOBackup_Database;
                oBackup.Database = database;
                oBackup.Files = url;//"d:\Northwind.bak";
                oBackup.BackupSetName = database;
                oBackup.BackupSetDescription = "数据库备份";
                oBackup.Initialize = true;
                oBackup.SQLBackup(oSQLServer);
                return true;
            }
            catch
            {
                return false;
                throw;
            }
            finally
            {
                oSQLServer.DisConnect();
            }
        }
        /// <summary>
        /// 数据库还原
        /// </summary>
        /// <param name="url">备份文件的地址</param>
        /// <returns></returns>
        public bool DbRestore(string url)
        {
            if (exepro() != true)//执行存储过程
            {
                return false;
            }
            else
            {
                SQLDMO.Restore oRestore = new SQLDMO.RestoreClass();
                SQLDMO.SQLServer oSQLServer = new SQLDMO.SQLServerClass();
                try
                {
                    oSQLServer.LoginSecure = false;
                    oSQLServer.Connect(server, uid, pwd);
                    oRestore.Action = SQLDMO.SQLDMO_RESTORE_TYPE.SQLDMORestore_Database;
                    oRestore.Database = database;
                    oRestore.Files = url;//@"d:\Northwind.bak";
                    oRestore.FileNumber = 1;
                    oRestore.ReplaceDatabase = true;
                    oRestore.SQLRestore(oSQLServer);
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    oSQLServer.DisConnect();
                }
            }
        }
        //把正在连接中的用户都中断　然后才能还原
        private bool exepro()
        {
            SqlConnection conn1 = new SqlConnection("server=" + server + ";uid=" + uid + ";pwd=" + pwd + ";database=master");
            SqlCommand cmd = new SqlCommand("killspid", conn1);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@dbname", database);
            try
            {
                conn1.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                conn1.Close();
            }
        }
        //必须在master这个系统数据库 加上这个存储过程
        //create proc killspid (@dbname varchar(20))
        //as
        //begin
        //declare @sql nvarchar(500)
        //declare @spid int
        //set @sql='declare getspid cursor for 
        //select spid from sysprocesses where dbid=db_id('''+@dbname+''')'
        //exec (@sql)
        //open getspid
        //fetch next from getspid into @spid
        //while @@fetch_status<>-1
        //begin
        //exec('kill '+@spid)
        //fetch next from getspid into @spid
        //end
        //close getspid
        //deallocate getspid
        //end
        //GO
    }
}
