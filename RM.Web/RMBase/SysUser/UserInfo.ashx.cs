using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RM.Busines;
using System.Collections;

namespace RM.Web.RMBase.SysUser
{
    /// <summary>
    /// UserInfo 的摘要说明
    /// </summary>
    public class UserInfo : IHttpHandler, IRequiresSessionState
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
            string user_ID = context.Request["user_ID"];                    //用户主键
            Hashtable ht = new Hashtable();
            int Return = -1;
            switch (Action)
            {
                case "accredit":                                            //用户信息启用
                    ht["DeleteMark"] = 1;
                    Return = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", user_ID, ht);
                    context.Response.Write(Return.ToString());
                    break;
                case "lock":                                                //锁定用户信息
                    ht["DeleteMark"] = 2;
                    Return = DataFactory.SqlDataBase().UpdateByHashtable("Base_UserInfo", "User_ID", user_ID, ht);
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