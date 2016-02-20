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
    public partial class frmATS_BaseSetting : PageBase
    {
        string _key, _ParentId;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];                  //主键
            _ParentId = Request["ParentId"];        //父节点
            if (!IsPostBack)
            {
                Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_ATS_BaseSetting", "id", "1");
                if (ht.Count > 0 && ht != null)
                {
                    ControlBindHelper.SetWebControls(this.Page, ht);                    
                }
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht = ControlBindHelper.GetWebControls(this.Page);
            int DelOK = DataFactory.SqlDataBase().DeleteData("Base_ATS_BaseSetting", "id", "1");
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_ATS_BaseSetting", "id", _key, ht);
            if (IsOk)
            {
                ShowMsgHelper.AlertMsg("操作成功！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("操作失败！");
            }
        }
    }
}