using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Common.DotNetUI;
using RM.Common.DotNetBean;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysPersonal
{
    public partial class Login_List : PageBase
    {
        RM_UserInfo_IDAO user_idao = new RM_UserInfo_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageControl1.pageHandler += new EventHandler(pager_PageChanged);
        }
        /// <summary>
        /// 绑定数据，分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void pager_PageChanged(object sender, EventArgs e)
        {
            DataBindGrid();
        }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void DataBindGrid()
        {
            int count = 0;
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            if (BeginBuilTime.Value != "" || endBuilTime.Value != "")
            {
                SqlWhere.Append(" and Sys_LoginLog_Time >= @BeginBuilTime");
                SqlWhere.Append(" and Sys_LoginLog_Time <= @endBuilTime");
                IList_param.Add(new SqlParam("@BeginBuilTime", CommonHelper.GetDateTime(BeginBuilTime.Value)));
                IList_param.Add(new SqlParam("@endBuilTime", CommonHelper.GetDateTime(endBuilTime.Value).AddDays(1)));
            }
            SqlWhere.Append(" and User_Account = @User_Account");
            IList_param.Add(new SqlParam("@User_Account", RequestSession.GetSessionUser().UserAccount));
            DataTable dt = user_idao.GetSysLoginLogPage(SqlWhere, IList_param, PageControl1.PageIndex, PageControl1.PageSize, ref count);
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
            this.PageControl1.RecordCount = Convert.ToInt32(count);
        }
        /// <summary>
        /// 绑定后激发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbl_Sys_LoginLog = e.Item.FindControl("lbl_Sys_LoginLog_Status") as Label;
                if (lbl_Sys_LoginLog != null)
                {
                    string text = lbl_Sys_LoginLog.Text;
                    text = text.Replace("1", "<span style='color:Blue'>成功登陆</span>");
                    text = text.Replace("0", "<span style='color:red'>登陆失败</span>");
                    lbl_Sys_LoginLog.Text = text;
                }
            }
        }
        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            DataBindGrid();
        }
    }
}