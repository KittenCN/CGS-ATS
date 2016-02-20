using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using RM.Common.DotNetData;
using RM.Busines;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_LeaveLeft : PageBase
    {
        public StringBuilder strHtml = new StringBuilder();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitInfo();
            }
        }

        public void InitInfo()
        {
            DataTable dt = DataFactory.SqlDataBase().GetDataTable("Base_ATS_LeaveSetting");
            if (DataTableHelper.IsExistRows(dt))
            {
                DataView dv = new DataView(dt);
                //dv.RowFilter = "id = '10'";
                foreach (DataRowView drv in dv)
                {
                    strHtml.Append("<li>");
                    strHtml.Append("<div>" + drv["LeaveName"].ToString() + "");
                    strHtml.Append("<span style='display:none'>" + drv["id"].ToString() + "</span></div>");
                    strHtml.Append("</li>");
                }
            }
            else
            {
                strHtml.Append("<li>");
                strHtml.Append("<div><span style='color:red;'>暂无数据</span></div>");
                strHtml.Append("</li>");
            }
        }
    }
}