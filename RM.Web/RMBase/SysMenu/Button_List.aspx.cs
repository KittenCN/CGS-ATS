using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RM.Busines;
using RM.Common.DotNetUI;
using System.Collections;
using System.Text;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysMenu
{
    public partial class Button_List : PageBase
    {
        RM_System_IDAO systemidao = new RM_System_Dal();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            DataTable dt = systemidao.GetButtonList();
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
        }
    }
}