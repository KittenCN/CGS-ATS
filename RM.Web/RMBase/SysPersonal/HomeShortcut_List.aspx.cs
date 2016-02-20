using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RM.Common.DotNetUI;
using RM.Busines;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetBean;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysPersonal
{
    public partial class HomeShortcut_List : PageBase
    {
        RM_System_IDAO sys_idao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitBindData();
            }
        }
        /// <summary>
        /// 初始化绑定数据源
        /// </summary>
        private void InitBindData()
        {
            DataTable dt = sys_idao.GetHomeShortcut_List(RequestSession.GetSessionUser().UserId.ToString());
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
        }
    }
}