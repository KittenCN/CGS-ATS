using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Web.App_Code;
using RM.Busines;
using RM.Common.DotNetUI;
using RM.Common.DotNetBean;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Data;
using System.IO;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_PerTravelAppPro : PageBase
    {
        string _key;
        public string txt_EmpID;
        public string txt_EmpName;
        public static string txt_FilesAdd;
        public static string txt_downFilesAdd;
        public static string txt_NextApprover;
        public static float flotxDays;
        public static string strUserID;

        protected void Page_Load(object sender, EventArgs e)
        {
            int i = 0;
            _key = Request["key"];
            txt_EmpID = RequestSession.GetSessionUser().UserId.ToString();
            txt_EmpName = RequestSession.GetSessionUser().UserName.ToString();

            string sql1 = "select Boss_id from Base_UserInfo where user_id='" + txt_EmpID + "'";
            StringBuilder sb_sql1 = new StringBuilder(sql1);
            DataTable dt1 = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql1);
            if (dt1.Rows[0].ItemArray[0] != null)
            {
                txt_NextApprover = dt1.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                txt_NextApprover = "";
            }

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_key))
                {

                    string sql = "select ApprovalFlag,FilesAdd from Base_PerTravelApply where id='" + _key + "'";
                    StringBuilder sb_sql = new StringBuilder(sql);
                    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                    if (dt.Rows[0].ItemArray[0] != null)
                    {
                        i = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                    }
                    InitData();
                    //if (i != 0)
                    //{
                    //    Save.Enabled = false;
                    //    ShowMsgHelper.Alert_Error("申请已在审批中或已审批完成,不得被修改!");
                    //}
                    if (dt.Rows[0].ItemArray[1] == null || dt.Rows[0].ItemArray[1].ToString().Length == 0)
                    {
                        DownFiles.Visible = false;
                    }
                    else
                    {
                        DownFiles.Visible = true;
                        txt_downFilesAdd = dt.Rows[0].ItemArray[1].ToString();
                    }
                }
            }
        }

        private void InitData()
        {

            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_PerTravelApply", "id", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                strUserID = EmpID.Text;
                EmpID.Text = GetNameFromID(EmpID.Text);
                //lab_CreateDate.Text = "";
                BeginDate.Text = Convert.ToDateTime(BeginDate.Text).ToString("yyyy-MM-dd");
                EndDate.Text = Convert.ToDateTime(EndDate.Text).ToString("yyyy-MM-dd");

            }
            CallDays();
        }

        private void CallDays()
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
            flotxDays = 0;
            for (DateTime dtT = dtBeginDate; dtT < dtEndDate.AddDays(1); dtT = dtT.AddDays(1))
            {
                int intdtT = (int)dtT.DayOfWeek;
                if (intdtT == 6 || intdtT == 0)
                {
                    flotxDays = flotxDays + 1;
                }
            }
            txDays.Text = flotxDays.ToString();

            if (intBeginFlag == 1 && intEndFlag == 1)
            {
                ts = dtEndDate - dtBeginDate;
                fResult = ts.Days + float.Parse("1");
            }
            if (intBeginFlag == 0 && intEndFlag == 1)
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

            TravelDays.Text = fResult.ToString();
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

        protected void Pass_Click(object sender, EventArgs e)
        {
            CallDays();

            string txt_Remark = "";
            int int_AppStatus = 99;

            if (txt_NextApprover == null || txt_NextApprover.Length <= 0)
            {
                int_AppStatus = 2;
                txt_NextApprover = "";
            }
            else
            {
                int_AppStatus = 1;
            }

            txt_Remark = ApprovalRemark.InnerText;
            flotxDays = float.Parse(txDays.Text);

            string sql1 = "update Base_PerTravelApply set ApprovalFlag=" + int_AppStatus + ",NextApprover='" + txt_NextApprover + "' where id='" + _key + "' ";
            StringBuilder sb_sql1 = new StringBuilder(sql1);
            int i1 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql1);
            if (i1 > 0)
            {
                string Cur_Date = DateTime.Now.ToString("yyyy-MM-dd");
                string sql2 = "insert into Base_PerTravelApplyDetail(PTid,ApproverId,ApprovalStatus,ApprovalRemark,ApprovalDate) ";
                sql2 = sql2 + "select " + _key + ",'" + txt_EmpID + "'," + int_AppStatus + ",'" + txt_Remark + "','" + Cur_Date + "' ";
                StringBuilder sb_sql2 = new StringBuilder(sql2);
                int i2 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql2);
                if (i2 > 0)
                {
                    GenModel gm = new GenModel();
                    if (gm.GetEMailFromID(strUserID) != null)
                    {
                        gm.SendMail2(gm.GetEMailFromID(strUserID), "Your TraveList has been updated!", "Your TraveList has been updated!");
                    }
                    ShowMsgHelper.AlertMsg("Success");
                    string sql3 = "update Base_LeaveConsole set SYTX=SYTX+" + flotxDays + " where EmpID='" + txt_EmpID + "' ";
                    StringBuilder sb_sql3 = new StringBuilder(sql3);
                    int i3 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql3);
                }
                else
                {
                    ShowMsgHelper.Alert_Wern("Error");
                }
            }
            else
            {
                ShowMsgHelper.Alert_Wern("Error");
            }

        }

        protected void Reject_Click(object sender, EventArgs e)
        {
            string txt_Remark = "";
            int int_AppStatus = 3;

            //if (txt_NextApprover == null || txt_NextApprover.Length <= 0)
            //{
            //    int_AppStatus = 2;
            //    txt_NextApprover = "";
            //}
            //else
            //{
            //    int_AppStatus = 1;
            //}

            string sql1 = "update Base_PerTravelApply set ApprovalFlag=" + int_AppStatus + " where id='" + _key + "' ";
            StringBuilder sb_sql1 = new StringBuilder(sql1);
            int i1 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql1);
            if (i1 > 0)
            {
                string Cur_Date = DateTime.Now.ToString("yyyy-MM-dd");
                string sql2 = "insert into Base_PerTravelApplyDetail(PTid,ApproverId,ApprovalStatus,ApprovalRemark,ApprovalDate) ";
                sql2 = sql2 + "select " + _key + ",'" + txt_EmpID + "'," + int_AppStatus + ",'" + txt_Remark + "','" + Cur_Date + "' ";
                StringBuilder sb_sql2 = new StringBuilder(sql2);
                int i2 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql2);
                if (i2 > 0)
                {
                    ShowMsgHelper.AlertMsg("Success");
                }
                else
                {
                    ShowMsgHelper.Alert_Wern("Error");
                }
            }
            else
            {
                ShowMsgHelper.Alert_Wern("Error");
            }
        }

        protected void btn_down_Click(object sender, EventArgs e)
        {
            //WriteFile实现Download 
            string fileName = "ceshi.rar";//客户端保存的文件名
            string filePath = txt_downFilesAdd;//路径

            FileInfo fileInfo = new FileInfo(filePath);
            fileName = Path.GetFileName(filePath);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            Response.End();
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
                    ShowMsgHelper.Alert_Error("Error: " + ex.Message);
                }
            }
            else
            {
                //Response.Write("Please select a file to upload.");
                ShowMsgHelper.Alert_Error("Please select a file to upload.");
            }
        }
    }
}