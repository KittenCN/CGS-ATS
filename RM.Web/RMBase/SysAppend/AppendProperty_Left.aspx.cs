using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using RM.Busines;
using RM.Common.DotNetData;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysAppend
{
    public partial class AppendProperty_Left : PageBase
    {
        public StringBuilder strHtml = new StringBuilder();
        RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitInfo();
            }
        }
        /// <summary>
        /// 所属功能
        /// </summary>
        public void InitInfo()
        {
            DataTable dt = systemidao.AppendProperty_Function();
            if (DataTableHelper.IsExistRows(dt))
            {
                DataView dv = new DataView(dt);
                foreach (DataRowView drv in dv)
                {
                    strHtml.Append("<li>");
                    strHtml.Append("<div onclick=\"Property_Function('" + drv["Property_Function"].ToString() + "')\">" + drv["Property_Function"].ToString() + "</div>");
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