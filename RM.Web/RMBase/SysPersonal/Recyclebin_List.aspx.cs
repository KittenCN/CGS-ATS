using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Common.DotNetCode;
using System.Data;
using RM.Common.DotNetUI;
using RM.Common.DotNetBean;
using RM.Busines;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysPersonal
{
    public partial class Recyclebin_List : PageBase
    {
        string _Recyclebin_Name;
        protected void Page_Load(object sender, EventArgs e)
        {
            _Recyclebin_Name = Server.UrlDecode(Request["Recyclebin_Name"]);
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
            StringBuilder strSql = new StringBuilder("SELECT Recyclebin_ID,Recyclebin_EventField,Recyclebin_Name,CreateUserName,CreateDate,Recyclebin_Remark FROM Base_Recyclebin WHERE 1=1");
            IList<SqlParam> IList_param = new List<SqlParam>();
            if (BeginBuilTime.Value != "" || endBuilTime.Value != "")
            {
                strSql.Append(" and CreateDate >= @BeginBuilTime");
                strSql.Append(" and CreateDate <= @endBuilTime");
                IList_param.Add(new SqlParam("@BeginBuilTime", CommonHelper.GetDateTime(BeginBuilTime.Value)));
                IList_param.Add(new SqlParam("@endBuilTime", CommonHelper.GetDateTime(endBuilTime.Value).AddDays(1)));
            }
            strSql.Append(" and CreateUserId = @CreateUserId");
            IList_param.Add(new SqlParam("@CreateUserId", RequestSession.GetSessionUser().UserId));
            strSql.Append(" and Recyclebin_Name = @Recyclebin_Name");
            IList_param.Add(new SqlParam("@Recyclebin_Name", _Recyclebin_Name));
            DataTable dt = DataFactory.SqlDataBase().GetPageList(strSql.ToString(), IList_param.ToArray(), "CreateDate", "Desc", PageControl1.PageIndex, PageControl1.PageSize, ref count);
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
            this.PageControl1.RecordCount = Convert.ToInt32(count);
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