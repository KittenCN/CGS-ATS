using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Common.DotNetBean;
using System.Text;
using System.Data;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetData;
using RM.Web.App_Code;

namespace RM.Web.Frame
{
    public partial class HomeIndex : PageBase
    {
        public StringBuilder sbHomeShortcouHtml = new StringBuilder();
        public StringBuilder Login_InfoHtml = new StringBuilder();
        public string _UserName;
        RM_System_IDAO sys_idao = new RM_System_Dal();
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            _UserName = RequestSession.GetSessionUser().UserName.ToString();
            InitShortcouData();
            BindLogin_Info();
        }
        /// <summary>
        /// 加载快捷功能
        /// </summary>
        private void InitShortcouData()
        {
            DataTable dt = sys_idao.GetHomeShortcut_List(RequestSession.GetSessionUser().UserId.ToString());
            if (DataTableHelper.IsExistRows(dt))
            {
                int rowSum = dt.Rows.Count;
                if (rowSum > 0)
                {
                    for (int i = 0; i < rowSum; i++)
                    {
                        sbHomeShortcouHtml.Append("<div onclick=\"ClickShortcut('" + dt.Rows[i]["NavigateUrl"] + "','" + dt.Rows[i]["Setup_IName"] + "','" + dt.Rows[i]["Target"] + "')\" class=\"shortcuticons\">");
                        sbHomeShortcouHtml.Append("<img src=\"/Themes/Images/32/" + dt.Rows[i]["Setup_Img"] + "\" alt=\"\" /><br />");
                        sbHomeShortcouHtml.Append(dt.Rows[i]["Setup_IName"] + "</div>");
                    }
                }
            }
        }
        public void BindLogin_Info()
        {
            int count = 0;
            DataTable dt = user_idao.GetLogin_Info(ref count);
            Login_InfoHtml.Append("本月登录总数：" + count + " 次 <br />");
            Login_InfoHtml.Append("本次登录IP：" + dt.Rows[0]["SYS_LOGINLOG_IP"].ToString() + "<br />");
            Login_InfoHtml.Append("本次登录时间：" + dt.Rows[0]["SYS_LOGINLOG_TIME"].ToString() + "<br />");
            if (dt.Rows.Count != 1)
            {
                Login_InfoHtml.Append("上次登录IP：" + dt.Rows[1]["SYS_LOGINLOG_IP"].ToString() + "<br />");
                Login_InfoHtml.Append("上次登录时间：" + dt.Rows[1]["SYS_LOGINLOG_TIME"].ToString() + "<br />");
            }
            else
            {
                Login_InfoHtml.Append("上次登录IP：127.0.0.1 <br />");
                Login_InfoHtml.Append("上次登录时间：1900-01-01 00:00:00<br />");
            }
        }
    }
}