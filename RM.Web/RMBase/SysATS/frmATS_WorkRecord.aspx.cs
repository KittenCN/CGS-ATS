using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using RM.Busines;
using RM.Common.DotNetUI;
using System.Text;
using System.Data;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using RM.Common.DotNetCode;
using RM.Common.DotNetBean;
using RM.Common.DotNetData;
using RM.Web.App_Code;
using RM.Common.DotNetEncrypt;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_WorkRecord : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitData();
            if (!IsPostBack)
            {

            }
        }

        private void InitData()
        {
            //初始化下拉框
            string sql = "select user_name from Base_UserInfo";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
                builder.Append(String.Format("<option value='{0}'>", dt.Rows[i][0]));
            Emplist.InnerHtml = builder.ToString();
        }
    }
}