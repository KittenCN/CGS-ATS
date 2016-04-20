using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using RM.Common.DotNetCode;
using System.Diagnostics;
using System.IO;
using System.Text;
using RM.Common.DotNetConfig;

namespace RM.Web
{
    public class Global : System.Web.HttpApplication
    {
        /// <summary>
        /// 创建系统异常日志
        /// </summary>
        protected LogHelper Logger = new LogHelper("Global");
        void Application_Start(object sender, EventArgs e)
        {
            // Calculation人数
            Application.Lock();
            Application["CurrentUsers"] = 0;
            Application.UnLock();

            #region 定时器
            System.Timers.Timer myTimer = new System.Timers.Timer(60000);
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            myTimer.Interval = 1000;//
            myTimer.Enabled = true;
            #endregion
        }
        /// <summary>
        /// 错误处理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError().GetBaseException();
            string error = objErr.Message + "";
            Server.ClearError();
            Application["error"] = error;
            Response.Redirect("~/Error/ErrorPage.aspx");
        }
        void Session_Start(object sender, EventArgs e)
        {
            // Calculation人数
            Application.Lock();
            Application["CurrentUsers"] = (int)Application["CurrentUsers"] + 1;
            Application.UnLock();
        }
        void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["CurrentUsers"] = (int)Application["CurrentUsers"] - 1;
            Application.UnLock();
        }
        /// <summary>
        /// 定时器触发事件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            this.RestartIIS();
        }
        /// <summary>
        /// 自动重启IIS
        /// </summary>
        private void RestartIIS()
        {
            if (ConfigHelper.GetAppSettings("IsRestartIIS").Equals("true"))//判断是否自动重启IIS
            {
                if (DateTime.Now.ToString("HH:mm:ss").Equals(ConfigHelper.GetAppSettings("RestartIISTime")))
                {
                    Logger.WriteLog("自动重启IIS时间到了");
                }
            }
        }
    }
}