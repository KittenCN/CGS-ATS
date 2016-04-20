using RM.Busines;
using RM.Common.DotNetUI;
using System;
using System.Collections;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_HolidayEdit : PageBase
    {
        string _key;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_key))
                {
                    InitData();
                }
            }
        }

        private void InitData()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_ATS_HolidaySetting", "id", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                BeginDate.Text = Convert.ToDateTime(BeginDate.Text).ToString("yyyy-MM-dd");
                EndDate.Text = Convert.ToDateTime(EndDate.Text).ToString("yyyy-MM-dd");
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht = ControlBindHelper.GetWebControls(this.Page);
            int IsOk = DataFactory.SqlDataBase().UpdateByHashtable("Base_ATS_HolidaySetting", "id", _key, ht);
            if (IsOk > 0)
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