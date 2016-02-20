using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Busines;

namespace RM.Web.Ajax
{
    /// <summary>
    /// Common_Ajax 的摘要说明
    /// </summary>
    public class Common_Ajax : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 公共一般处理程序
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            string Action = context.Request["action"];              //提交动作
            string module = context.Request["module"];              //业务模块
            string tableName = context.Request["tableName"];        //数据库表
            string pkName = context.Request["pkName"];              //字段主键
            string pkVal = context.Request["pkVal"];                //字段值
            int Return = -1;
            RM_System_IDAO systemidao = new RM_System_Dal();
            switch (Action)
            {
                case "Cut":                     //安全退出
                    context.Session.Abandon();  //取消当前会话
                    context.Session.Clear();    //清除当前浏览器所以Session
                    context.Response.Write(1);
                    context.Response.End();
                    break;
                case "Virtualdelete":           //数据放入回收站 1,2,3,4,5,6
                    Return = systemidao.Virtualdelete(module.Trim(), tableName.Trim(), pkName.Trim(), pkVal.Trim().Split(','));
                    context.Response.Write(Return.ToString());
                    break;
                case "Delete":                  //删除多条记录 1,2,3,4,5,6
                    Return = systemidao.DeleteData_Base(tableName.Trim(), pkName.Trim(), pkVal.Split(','));
                    context.Response.Write(Return.ToString());
                    break;
                case "IsExist":                 //判断数据是否存在
                    Return = DataFactory.SqlDataBase().IsExist(tableName.Trim(), pkName.Trim(), pkVal.Trim());
                    context.Response.Write(Return.ToString());
                    break;
                default:
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}