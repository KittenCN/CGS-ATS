using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Busines;

namespace RM.Web.Frame
{
    public partial class MainIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string strsql = "insert into Base_LeaveConsole(EmpID) select USER_ID from Base_UserInfo where USER_ID not in (select empid from Base_LeaveConsole)";
                StringBuilder sbsql = new StringBuilder(strsql);
                DataFactory.SqlDataBase().ExecuteBySql(sbsql);
            }
        }
    }
}