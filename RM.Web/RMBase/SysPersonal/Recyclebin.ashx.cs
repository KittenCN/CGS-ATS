using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RM.Busines.DAL;
using RM.Busines.IDAO;

namespace RM.Web.RMBase.SysPersonal
{
    /// <summary>
    /// Recyclebin 的摘要说明
    /// </summary>
    public class Recyclebin : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            string Action = context.Request["action"];                      //提交动作
            string pkVal = context.Request["pkVal"];                        //字段值
            RM_System_IDAO sys_idao = new RM_System_Dal();
            int Return = -1;
            switch (Action)
            {
                case "restore_Data"://还原回收站数据
                    Return = sys_idao.Recyclebin_Restore(pkVal.Split(','));
                    context.Response.Write(Return.ToString());
                    break;
                case "restore_Empty"://清空回收站数据
                    Return = sys_idao.Recyclebin_Empty(pkVal.Split(','));
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