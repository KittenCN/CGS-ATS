using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines;
using System.Text;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysDataCenter
{
    public partial class SQLQuery : PageBase
    {
        public string _Table_Name;
        protected void Page_Load(object sender, EventArgs e)
        {
            _Table_Name = Server.UrlDecode(Request["Table_Name"]);//表名
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("消息提示", Type.GetType("System.String"));
                DataRow row = dt.NewRow();
                row["消息提示"] = "未执行SQL命令!";
                dt.Rows.Add(row);
                ControlBindHelper.BindGridViewList(dt, Grid);
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExeOuter_Click(object sender, EventArgs e)
        {
            StringBuilder strSql = new StringBuilder(txtSql.Value);
            if (Execute_Type.Value == "1")
            {
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql);
                if (dt != null)
                {
                    if (dt.Rows.Count != 0)
                    {
                        ControlBindHelper.BindGridViewList(dt, Grid);
                    }
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("消息提示", Type.GetType("System.String"));
                        DataRow row = dt.NewRow();
                        row["消息提示"] = "没有找到您要的相关数据";
                        dt.Rows.Add(row);
                        ControlBindHelper.BindGridViewList(dt, Grid);
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("消息提示", Type.GetType("System.String"));
                    DataRow row = dt.NewRow();
                    row["消息提示"] = "执行SQL命令,有错误!";
                    dt.Rows.Add(row);
                    ControlBindHelper.BindGridViewList(dt, Grid);
                }
            }
            else if (Execute_Type.Value == "2")
            {
                int i = DataFactory.SqlDataBase().ExecuteBySql(strSql);
                DataTable dt = new DataTable();
                dt.Columns.Add("消息提示", Type.GetType("System.String"));
                DataRow row = dt.NewRow();
                if (i > 0)
                {
                    row["消息提示"] = "执行成功!";
                    dt.Rows.Add(row);
                    ControlBindHelper.BindGridViewList(dt, Grid);
                }
                else if (i == 0)
                {
                    row["消息提示"] = "0 行受影响!";
                    dt.Rows.Add(row);
                    ControlBindHelper.BindGridViewList(dt, Grid);
                }
                else
                {
                    row["消息提示"] = "执行SQL命令,有错误!";
                    dt.Rows.Add(row);
                    ControlBindHelper.BindGridViewList(dt, Grid);
                }
            }
        }
    }
}