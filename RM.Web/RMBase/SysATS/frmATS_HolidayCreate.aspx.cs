using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using RM.Common.DotNetUI;
using RM.Busines;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_HolidayCreate : PageBase
    {
        string _key, _ParentId;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            _ParentId = Request["ParentId"];        //父节点
            if (!IsPostBack)
            {

            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht = ControlBindHelper.GetWebControls(this.Page);           
            int IsOk = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Base_ATS_HolidaySetting", ht);
            if (IsOk>0)
            {
                ShowMsgHelper.AlertMsg("Success！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("Error！");
            }
        }
    }
}