using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using RM.Common.DotNetUI;
using RM.Busines;


namespace RM.Web.RMBase.SysHRM
{
    public partial class frmHRM_BaseInfo_Input : System.Web.UI.Page
    {
        string _key, _ParentId;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            _ParentId = Request["ParentId"];        //父节点
            if (!IsPostBack)
            {
                //InitParentId();
                //if (!string.IsNullOrEmpty(_ParentId))
                //{
                //    ParentId.Value = _ParentId;
                //}
                //if (!string.IsNullOrEmpty(_key))
                //{
                //    InitData();
                //}
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht = ControlBindHelper.GetWebControls(this.Page);
            if( Convert.ToString(ht["id"])=="")
            {
                ht["id"] = "0";
            }
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_HRM_BaseInfo", "index", _key, ht);
            if (IsOk)
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