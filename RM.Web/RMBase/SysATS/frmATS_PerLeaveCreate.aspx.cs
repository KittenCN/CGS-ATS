using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Busines;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Common.DotNetUI;
using RM.Web.App_Code;
using System.Data;
using System.Text;
using System.Collections;
using System.Globalization;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_PerLeaveCreate : PageBase
    {
        public string txt_EmpID;
        public string txt_EmpName;
        public static string txt_FilesAdd;

        protected void Page_Load(object sender, EventArgs e)
        {
            txt_EmpID= RequestSession.GetSessionUser().UserId.ToString();
            txt_EmpName = RequestSession.GetSessionUser().UserName.ToString();
         
            if (!IsPostBack)
            {
                DataBindGrid();
            }
        }
        private void DataBindGrid()
        {
            //int count = 0;
            //StringBuilder SqlWhere = new StringBuilder();
            //IList<SqlParam> IList_param = new List<SqlParam>();
            ////DataTable dt = DataFactory.SqlDataBase().GetDataTable("Base_ATS_OriDataIn");
            //string sql = "select * from Base_PerLeaveApply where empid='" + EmpID + "' ";
            //StringBuilder sb_sql = new StringBuilder(sql);
            //// DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            //DataTable dt = DataFactory.SqlDataBase().GetPageList(sql, null, "CreateDate", "asc", PageControl1.PageIndex, PageControl1.PageSize, ref count);
            //ControlBindHelper.BindRepeaterList(dt, rp_Item);
            //this.PageControl1.RecordCount = Convert.ToInt32(count);
            EmpID.Text = txt_EmpName;
            CreateDate.Text = DateTime.Now.ToShortDateString();
            string sql = "select * from Base_ATS_LeaveSetting";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            LeaveID.DataSource = dt;
            LeaveID.DataTextField = "LeaveName";
            LeaveID.DataValueField = "id";
            LeaveID.DataBind();
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            btnLeaveDays_Click(null,null);

            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            if ((Convert.ToDateTime(BeginDate.Text, dtFormat) > Convert.ToDateTime(EndDate.Text, dtFormat)) || (Convert.ToDateTime(BeginDate.Text, dtFormat) == Convert.ToDateTime(EndDate.Text, dtFormat) && BeginFlag.Value == "0" && EndFlag.Value == "1"))
            {
                ShowMsgHelper.Alert_Wern("日期设置错误");
            }
            else
            {
                if (LeaveID.SelectedValue == "6" && (txt_FilesAdd == null || txt_FilesAdd == ""))
                {
                    ShowMsgHelper.Alert_Wern("病假必须上传证明附件!");
                }
                else
                {
                    string txt_NextApprover = "";
                    string sql = "select Boss_id from Base_UserInfo where user_id='" + txt_EmpID + "'";
                    StringBuilder sb_sql = new StringBuilder(sql);
                    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                    if (dt.Rows[0].ItemArray[0] != null)
                    {
                        txt_NextApprover = dt.Rows[0].ItemArray[0].ToString();
                    }
                    Hashtable ht = new Hashtable();
                    ht = ControlBindHelper.GetWebControls(this.Page);
                    ht["EmpID"] = txt_EmpID;
                    ht["CreateDate"] = CreateDate.Text;
                    ht["ApprovalFlag"] = 0;
                    ht["NextApprover"] = txt_NextApprover;
                    ht["LeaveID"] = LeaveID.SelectedValue;
                    ht["FilesAdd"] = txt_FilesAdd;
                    ht["LeaveDays"] = LeaveDays.Text;
                    int IsOk = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Base_PerLeaveApply", ht);
                    if (IsOk > 0)
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

        private string GetNameFromID(string EmpID)
        {
            string txt_Result = "";

            string sql = "select User_name from Base_UserInfo where User_ID='" + EmpID + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count !=0 && dt.Rows[0].ItemArray[0].ToString() != "")
            {
                txt_Result = dt.Rows[0].ItemArray[0].ToString();
            }

            return txt_Result;
        }

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (FilesAdd.PostedFile != null && FilesAdd.PostedFile.ContentLength > 0)
            {
                //string fn = System.IO.Path.GetFileName(FileAdd.PostedFile.FileName);
                string fn = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + "_" + System.IO.Path.GetFileName(FilesAdd.PostedFile.FileName);
                string SaveLocation = Server.MapPath("LeaveFiles") + "\\" + fn;
                try
                {
                    FilesAdd.PostedFile.SaveAs(SaveLocation);
                    txt_FilesAdd = SaveLocation;
                    //Response.Write("The file has been uploaded.");
                    ShowMsgHelper.Alert_Wern("The file has been uploaded.");

                }
                catch (Exception ex)
                {
                    //Response.Write("Error: " + ex.Message);
                    ShowMsgHelper.Alert_Wern("Error: " + ex.Message);
                }
            }
            else
            {
                //Response.Write("Please select a file to upload.");
                ShowMsgHelper.Alert_Wern("Please select a file to upload.");
            }
        }

        protected void btnLeaveDays_Click(object sender, EventArgs e)
        {
            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            DateTime dtBeginDate = Convert.ToDateTime(BeginDate.Text, dtFormat);
            DateTime dtEndDate = Convert.ToDateTime(EndDate.Text, dtFormat);
            int intBeginFlag = int.Parse(BeginFlag.Value);
            int intEndFlag = int.Parse(EndFlag.Value);
            float fResult = 0;
            TimeSpan ts;
            //int differenceInDays = ts.Days;

            if (intBeginFlag==1 && intEndFlag==1)
            {
                ts = dtEndDate - dtBeginDate;
                fResult = ts.Days + float.Parse("1");
            }
            if(intBeginFlag==0&&intEndFlag==1)
            {
                ts = dtEndDate - dtBeginDate.AddDays(1);
                fResult = ts.Days + float.Parse("0.5") + float.Parse("1");            
            }
            if (intBeginFlag == 1 && intEndFlag == 0)
            {
                ts = dtEndDate - dtBeginDate;
                fResult = ts.Days - float.Parse("0.5") + float.Parse("1");
            }
            if (intBeginFlag == 0 && intEndFlag == 0)
            {
                ts = dtEndDate - dtBeginDate;
                fResult = ts.Days;
            }

            LeaveDays.Text = fResult.ToString();
        }
    }
}