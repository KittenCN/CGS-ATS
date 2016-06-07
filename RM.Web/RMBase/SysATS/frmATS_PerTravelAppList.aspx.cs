using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_PerTravelAppList : PageBase
    {
        public string EmpID;
        public string EmpName;

        protected void Page_Load(object sender, EventArgs e)
        {
            EmpID = RequestSession.GetSessionUser().UserId.ToString();
            EmpName = RequestSession.GetSessionUser().UserName.ToString();

            this.PageControl1.pageHandler += new EventHandler(pager_PageChanged);
            DataBindGrid();
            if (!IsPostBack)
            {

            }
        }

        private void DataBindGrid()
        {
            int count = 0;
            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            //DataTable dt = DataFactory.SqlDataBase().GetDataTable("Base_ATS_OriDataIn");
            string strSQL = "select Auto_Approval from Base_UserInfo where User_ID='" + EmpID + "' ";
            StringBuilder sbSQL = new StringBuilder(strSQL);
            DataTable dtSQL = DataFactory.SqlDataBase().GetDataTableBySQL(sbSQL);
            if (dtSQL.Rows[0][0].ToString() == "1")
            {
                Auto_Approval.Value = "1";
            }
            else
            {
                Auto_Approval.Value = "0";
            }
            string sql = "select * from Base_PerTravelApply where (ApprovalFlag=0 or ApprovalFlag=1) and empid in (select User_ID from Base_UserInfo where boss_id='" + EmpID + "') ";
            StringBuilder sb_sql = new StringBuilder(sql);
            // DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            DataTable dt = DataFactory.SqlDataBase().GetPageList(sql, null, "CreateDate", "asc", PageControl1.PageIndex, PageControl1.PageSize, ref count);
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
            this.PageControl1.RecordCount = Convert.ToInt32(count);
        }

        protected void pager_PageChanged(object sender, EventArgs e)
        {
            DataBindGrid();
        }

        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lab_EmpID = e.Item.FindControl("EmpID") as Label;
                Label lab_BeginFlag = e.Item.FindControl("BeginFlag") as Label;
                Label lab_EndFlag = e.Item.FindControl("EndFlag") as Label;
                Label lab_ApprovalFlag = e.Item.FindControl("ApprovalFlag") as Label;
                Label lab_NextApprover = e.Item.FindControl("NextApprover") as Label;

                if (lab_BeginFlag != null)
                {
                    string text = lab_BeginFlag.Text;
                    text = text.Replace("0", "Afternoon");
                    text = text.Replace("1", "Morning");
                    lab_BeginFlag.Text = text;
                }
                if (lab_EndFlag != null)
                {
                    string text = lab_EndFlag.Text;
                    text = text.Replace("0", "Morning");
                    text = text.Replace("1", "Aftermoon");
                    lab_EndFlag.Text = text;
                }
                if (lab_ApprovalFlag != null)
                {
                    string text = lab_ApprovalFlag.Text;
                    text = text.Replace("0", "未审批");
                    text = text.Replace("1", "审批中");
                    text = text.Replace("2", "审批通过");
                    text = text.Replace("3", "审批不通过");
                    lab_ApprovalFlag.Text = text;
                }
                if (lab_EmpID != null)
                {
                    lab_EmpID.Text = GetNameFromID(lab_EmpID.Text);
                }
                if (lab_NextApprover != null)
                {
                    lab_NextApprover.Text = GetNameFromID(lab_NextApprover.Text);
                }
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

        protected void btn_auto_Click(object sender, EventArgs e)
        {
            string[] str_Auto_Approval = Request.Form.GetValues("Auto_Approval");
            if (str_Auto_Approval[0] == "1")
            {
                string strSQL = "update Base_UserInfo set Auto_Approval=1 where User_ID='" + EmpID + "' ";
                StringBuilder sbSQL = new StringBuilder(strSQL);
                DataFactory.SqlDataBase().ExecuteBySql(sbSQL);
                ShowMsgHelper.AlertMsg("Set Auto Approval Success!");
            }
            else
            {
                string strSQL = "update Base_UserInfo set Auto_Approval=0 where User_ID='" + EmpID + "' ";
                StringBuilder sbSQL = new StringBuilder(strSQL);
                DataFactory.SqlDataBase().ExecuteBySql(sbSQL);
                ShowMsgHelper.AlertMsg("Set Manual Approval Success!");
            }
        }
    }
}