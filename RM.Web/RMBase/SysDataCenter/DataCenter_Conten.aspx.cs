using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using System.Data;
using RM.Common.DotNetUI;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysDataCenter
{
    public partial class DataCenter_Conten : PageBase
    {
        RM_System_IDAO systemidao = new RM_System_Dal();
        public string _Table_Name;
        protected void Page_Load(object sender, EventArgs e)
        {
            _Table_Name = Server.UrlDecode(Request["Table_Name"]);//表名
            if (!IsPostBack)
            {
                if (_Table_Name == null)
                {
                    _Table_Name = "未选择";
                }
                GridBind();
            }
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        public void GridBind()
        {
            DataTable dt = systemidao.GetSyscolumns(_Table_Name);
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
        }
    }
}