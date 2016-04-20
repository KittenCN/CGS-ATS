using System;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using RM.Busines;
using RM.Common.DotNetUI;
using System.Collections;
using System.Text;
using System.Data;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_ATSResultEdit : PageBase
    {
        string _key;

        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];
            if (!IsPostBack)
            {
                InitData();
            }
        }

        private void InitData()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_ATSResult", "id", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                //EmpID.Text = GetNameFromID(EmpID.Text);
                //lab_CreateDate.Text = "";
                //BeginDate.Text = Convert.ToDateTime(BeginDate.Text).ToString("yyyy-MM-dd");
                //EndDate.Text = Convert.ToDateTime(EndDate.Text).ToString("yyyy-MM-dd");
                Label lab_EmpID = FindControl("EmpID") as Label;
                Label lab_Flag = FindControl("Flag") as Label;
                Label lab_ATS_DateStatus = FindControl("ATS_DateStatus") as Label;
                Label lab_ATS_Holiday = FindControl("ATS_Holiday") as Label;
                Label lab_ATS_Leave = FindControl("ATS_Leave") as Label;
                Label lab_ATS_Travel = FindControl("ATS_Travel") as Label;

                if (lab_EmpID != null)
                {
                    lab_EmpID.Text = GetNameFromID(lab_EmpID.Text);
                }

                if (lab_Flag != null)
                {
                    string text = lab_Flag.Text;
                    text = text.Replace("0", "未审查");
                    text = text.Replace("1", "已审查");
                    text = text.Replace("2", "已修改");
                    lab_Flag.Text = text;
                }

                if (lab_ATS_DateStatus != null)
                {
                    string text = lab_ATS_DateStatus.Text;
                    text = text.Replace("0", "Sun");
                    text = text.Replace("1", "Mon");
                    text = text.Replace("2", "Tue");
                    text = text.Replace("3", "Wed");
                    text = text.Replace("4", "Thu");
                    text = text.Replace("5", "Fri");
                    text = text.Replace("6", "Sat");
                    lab_ATS_DateStatus.Text = text;
                }

                if (lab_ATS_Holiday != null && int.Parse(lab_ATS_Holiday.Text) > 0)
                {
                    lab_ATS_Holiday.Text = GetHNFromID(lab_ATS_Holiday.Text);                   
                }
                else
                {
                    lab_ATS_Holiday.Text = "-";
                }

                if (lab_ATS_Leave != null)
                {
                    lab_ATS_Leave.Text = GetLNFromID(lab_ATS_Leave.Text);
                }

                if (lab_ATS_Travel != null && int.Parse(lab_ATS_Travel.Text) > 0)
                {
                    lab_ATS_Travel.Text = "公出";
                }
                else
                {
                    lab_ATS_Travel.Text = "-";
                }
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht = ControlBindHelper.GetWebControls(this.Page);
            ht["Flag"] = 2;
            int IsOk = DataFactory.SqlDataBase().UpdateByHashtable("Base_ATSResult", "id", _key, ht);
            if (IsOk > 0)
            {
                ShowMsgHelper.AlertMsg("Success！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("Error！");
            }
        }

        private string GetNameFromID(string EmpID)
        {
            string txt_Result = "";

            string sql = "select User_name from Base_UserInfo where User_ID='" + EmpID + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count != 0 && dt.Rows[0].ItemArray[0].ToString() != "")
            {
                txt_Result = dt.Rows[0].ItemArray[0].ToString();
            }

            return txt_Result;
        }

        private string GetLNFromID(string LID)
        {
            string txt_Result = "-";

            string sql = "select LeaveName from Base_ATS_LeaveSetting where id='" + LID + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count != 0 && dt.Rows[0].ItemArray[0].ToString() != "")
            {
                txt_Result = dt.Rows[0].ItemArray[0].ToString();
            }

            return txt_Result;
        }

        private string GetHNFromID(string HID)
        {
            string txt_Result = "-";

            string sql = "select Holiday_name from Base_ATS_HolidaySetting where id='" + HID + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count != 0 && dt.Rows[0].ItemArray[0].ToString() != "")
            {
                txt_Result = dt.Rows[0].ItemArray[0].ToString();
            }

            return txt_Result;
        }

    }
}