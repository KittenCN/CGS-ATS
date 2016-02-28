using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using System.Data;
using RM.Common.DotNetBean;
using RM.Common.DotNetJson;
using System.Collections;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;


namespace RM.Web.Frame
{
    /// <summary>
    /// Frame 的摘要说明
    /// </summary>
    public class Frame : IHttpHandler, IRequiresSessionState
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
            string user_Account = context.Request["user_Account"];          //账户
            string userPwd = context.Request["userPwd"];                    //密码
            string code = context.Request["code"];                          //验证码
            RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
            RM_System_IDAO sys_idao = new RM_System_Dal();
            IPScanerHelper objScan = new IPScanerHelper();
            switch (Action)
            {
                case "login":
                    if (code.ToLower() != context.Session["dt_session_code"].ToString().ToLower())
                    {
                        context.Response.Write("1");//验证码输入不正确！
                        context.Response.End();
                    }
                    DataTable dtlogin = user_idao.UserLogin(user_Account.Trim(), userPwd.Trim());
                    if (dtlogin != null)
                    {
                        objScan.DataPath = context.Server.MapPath("/Themes/IPScaner/QQWry.Dat");
                        objScan.IP = RequestHelper.GetIP();
                        string OWNER_address = objScan.IPLocation();
                        if (dtlogin.Rows.Count != 0)
                        {
                            user_idao.SysLoginLog(user_Account, "1", OWNER_address);
                            if (dtlogin.Rows[0]["DeleteMark"].ToString() == "1" && dtlogin.Rows[0]["work_flag"].ToString() == "1")
                            {
                                if (Islogin(context, user_Account))
                                {
                                    SessionUser user = new SessionUser();
                                    user.UserId = dtlogin.Rows[0]["User_ID"].ToString();
                                    user.UserAccount = dtlogin.Rows[0]["User_Account"].ToString();
                                    user.UserName = dtlogin.Rows[0]["User_Name"].ToString() + "(" + dtlogin.Rows[0]["User_Account"].ToString() + ")";
                                    user.UserPwd = dtlogin.Rows[0]["User_Pwd"].ToString();
                                    RequestSession.AddSessionUser(user);
                                    context.Response.Write("3");//验证成功
                                    context.Response.End();
                                }
                                else
                                {
                                    context.Response.Write("6");//该用户已经登录，不允许重复登录
                                    context.Response.End();
                                }
                            }
                            else
                            {
                                user_idao.SysLoginLog(user_Account, "2", OWNER_address);//账户被锁,联系管理员！
                                context.Response.Write("2");
                                context.Response.End();
                            }
                        }
                        else
                        {
                            user_idao.SysLoginLog(user_Account, "0", OWNER_address);
                            context.Response.Write("4");//账户或者密码有错误！
                            context.Response.End();
                        }
                    }
                    else
                    {
                        context.Response.Write("5");//服务连接不上！
                        context.Response.End();
                    }
                    break;
                case "Menu":
                    string UserId = RequestSession.GetSessionUser().UserId.ToString();//用户ID
                    string strMenus = JsonHelper.DataTableToJson(sys_idao.GetMenuHtml(UserId), "MENU");
                    context.Response.Write(strMenus);
                    context.Response.End();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 同一账号不能同时登陆
        /// </summary>
        /// <param name="context"></param>
        /// <param name="User_Account">账户</param>
        /// <returns></returns>
        public bool Islogin(HttpContext context, string User_Account)
        {
            //将Session转换为Arraylist数组
            //ArrayList list = context.Session["GLOBAL_USER_LIST"] as ArrayList;
            //if (list == null)
            //{
            //    list = new ArrayList();
            //}
            //for (int i = 0; i < list.Count; i++)
            //{
            //    if (User_Account == (list[i] as string))
            //    {
            //        //已经登录了，提示错误信息 
            //        return false; ;
            //    }
            //}
            ////将用户信息添加到list数组中
            //list.Add(User_Account);
            ////将数组放入Session
            //context.Session.Add("GLOBAL_USER_LIST", list);
            return true;
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