using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetData;
using System.Data;
using RM.Common.DotNetBean;

namespace RM.Web.UserControl
{
    public partial class LoadButton : System.Web.UI.UserControl
    {
        public StringBuilder sb_Button = new StringBuilder();
        RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            string UserId = RequestSession.GetSessionUser().UserId.ToString();//用户ID
            DataTable dt_Button = systemidao.GetButtonHtml(UserId);
            if (DataTableHelper.IsExistRows(dt_Button))
            {
                foreach (DataRow dr in dt_Button.Rows)
                {
                    sb_Button.Append("<a title=\"" + dr["Menu_Title"].ToString() + "\" onclick=\"" + dr["NavigateUrl"].ToString() + ";\" class=\"button green\">");
                    sb_Button.Append("<span class=\"icon-botton\" style=\"background: url('/Themes/images/16/" + dr["Menu_Img"].ToString() + "') no-repeat scroll 0px 4px;\"></span>");
                    sb_Button.Append(dr["Menu_Name"].ToString());
                    sb_Button.Append("</a>");
                }
            }
            else
            {
                sb_Button.Append("<a class=\"button green\">");
                sb_Button.Append("无操作按钮");
                sb_Button.Append("</a>");
            }
        }
    }
}