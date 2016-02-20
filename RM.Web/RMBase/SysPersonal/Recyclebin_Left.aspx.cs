using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using RM.Common.DotNetCode;
using RM.Busines;
using RM.Common.DotNetBean;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysPersonal
{
    public partial class Recyclebin_Left : PageBase
    {
        public StringBuilder strHtml = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {
            GetTreeNode();
        }
        public void GetTreeNode()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT DISTINCT(Recyclebin_Name) FROM Base_Recyclebin where CreateUserId = @CreateUserId");
            SqlParam[] para = {
                                         new SqlParam("@CreateUserId",RequestSession.GetSessionUser().UserId)};
            DataTable Recyclebin_dt = DataFactory.SqlDataBase().GetDataTableBySQL(strSql, para);
            DataView dv = new DataView(Recyclebin_dt);
            if (dv.Count > 0)
            {
                strHtml.Append("<ul>");
                foreach (DataRowView drv in dv)
                {
                    strHtml.Append("<li>");
                    strHtml.Append("<div onclick=\"GetRecyclebin_Name('" + drv["Recyclebin_Name"].ToString() + "')\">" + drv["Recyclebin_Name"] + "</div>");
                    strHtml.Append("</li>");
                }
                strHtml.Append("</ul>");
            }
            else
            {
                strHtml.Append("<ul>");
                strHtml.Append("<li>");
                strHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
                strHtml.Append("</li>");
                strHtml.Append("</ul>");
            }
        }
    }
}