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
        public static float flo_njDays;
        public static float flo_txDays;

        protected void Page_Load(object sender, EventArgs e)
        {
            txt_EmpID= RequestSession.GetSessionUser().UserId.ToString();
            txt_EmpName = RequestSession.GetSessionUser().UserName.ToString();
         
            if (!IsPostBack)
            {
                string sql = "select * from uvw_doLeaveDays where EmpID='" + txt_EmpID + "' ";
                StringBuilder sb_sql = new StringBuilder(sql);
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                CJform.Visible = false;
                if(dt!=null || dt.Rows.Count>0)
                {
                    flo_njDays = float.Parse(dt.Rows[0].ItemArray[1].ToString());
                    flo_txDays = float.Parse(dt.Rows[0].ItemArray[2].ToString());
                }
                else
                {
                    flo_njDays = 0;
                    flo_txDays = 0;
                }
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
            njDays.Text = flo_njDays.ToString();
            txDays.Text = flo_txDays.ToString();
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            btnLeaveDays_Click(null,null);
            int intMaxPerTime = 9999;
            int intMaxPerYear = 9999;
            int intMustFile = 0;

            String yy = DateTime.Now.Year.ToString();
            String mm = DateTime.Now.Month.ToString();
            String days = DateTime.DaysInMonth(int.Parse(yy), int.Parse(mm)).ToString();
            DateTime FirstDay = DateTime.Parse(yy + "/" + mm + "/1");
            DateTime LastDay = FirstDay.AddYears(1);
            float floAllLeaveDays = 0;
            string sqlii = "select isnull(SUM(leavedays),0) as leavedays from Base_PerLeaveApply where EmpID='" + txt_EmpID + "' and LeaveID=" + LeaveID.SelectedValue + " and BeginDate>='" + FirstDay + "' and EndDate<'" + LastDay + "' and ApprovalFlag=2";
            StringBuilder sb_sqlii = new StringBuilder(sqlii);
            DataTable dtii = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sqlii);
            if (dtii != null && dtii.Rows.Count > 0)
            {
                floAllLeaveDays = float.Parse(dtii.Rows[0].ItemArray[0].ToString());
            }

            string sqli = "select * from Base_ATS_LeaveSetting where id=" + LeaveID.SelectedValue;
            StringBuilder sb_sqli = new StringBuilder(sqli);
            DataTable dti = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sqli);
            if(dti!=null && dti.Rows.Count>0)
            {
                if(int.Parse(dti.Rows[0].ItemArray[2].ToString()) > 0)
                {
                    intMaxPerTime = int.Parse(dti.Rows[0].ItemArray[2].ToString());
                }
                if (int.Parse(dti.Rows[0].ItemArray[3].ToString()) > 0)
                {
                    intMaxPerYear = int.Parse(dti.Rows[0].ItemArray[3].ToString());
                }
                if (int.Parse(dti.Rows[0].ItemArray[4].ToString()) > 0)
                {
                    intMustFile = int.Parse(dti.Rows[0].ItemArray[4].ToString());
                }
            }
            else
            {
                ShowMsgHelper.Alert_Error("System Error,Call Admin");
            }

            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            if ((Convert.ToDateTime(BeginDate.Text, dtFormat) > Convert.ToDateTime(EndDate.Text, dtFormat)) || (Convert.ToDateTime(BeginDate.Text, dtFormat) == Convert.ToDateTime(EndDate.Text, dtFormat) && BeginFlag.Value == "0" && EndFlag.Value == "0"))
            {
                ShowMsgHelper.Alert_Wern("日期设置错误");
            }
            else
            {
                if (intMustFile==1 && (txt_FilesAdd == null || txt_FilesAdd == ""))
                {
                    ShowMsgHelper.Alert_Wern("必须上传证明附件!");
                }
                else
                {
                    if(float.Parse(LeaveDays.Text)> intMaxPerTime || floAllLeaveDays>intMaxPerYear)
                    {
                        ShowMsgHelper.Alert_Wern("申请天数超限!");
                    }
                    else
                    {
                        if (LeaveID.SelectedValue == "7" && float.Parse(LeaveDays.Text) > float.Parse(njDays.Text))
                        {
                            ShowMsgHelper.Alert_Wern("年假申请天数超过年假可用天数!");
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
                            if (cbNC.Checked == true)
                            {
                                ht["NCJ"] = 1;
                            }
                            else
                            {
                                ht["NCJ"] = 0;
                            }
                            if (cbDBT.Checked == true)
                            {
                                ht["DBT"] = 1;
                                ht["DBTnum"] = DBT.Text;
                            }
                            else
                            {
                                ht["DBT"] = 0;
                                ht["DBTnum"] = 0;
                            }
                            int IsOk = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Base_PerLeaveApply", ht);
                            if (IsOk > 0)
                            {
                                //GenModel gm = new GenModel();
                                //string strMailResult = gm.SendMail("hrtest@coopgs.com", "candy.lv@longint.net", "MailTest", "MailTest");
                                //if (strMailResult != "发送成功！")
                                //{
                                //    ShowMsgHelper.AlertMsg(strMailResult);
                                //}
                                GenModel gm = new GenModel();                               
                                if (txt_NextApprover!=null && txt_NextApprover!="" && gm.GetEMailFromID(txt_NextApprover)!="" && gm.GetEMailFromID(txt_NextApprover)!=null)
                                {                                  
                                    gm.SendMail2(gm.GetEMailFromID(txt_NextApprover), "You have a new Task!", "You have a new Task!");
                                }
                                if (gm.GetEMailFromID(txt_EmpID) != null)
                                {
                                    gm.SendMail2(gm.GetEMailFromID(txt_EmpID), "Your LeaveList has been updated!", "Your LeaveList has been updated!");
                                }

                                ShowMsgHelper.AlertMsg("Success!");
                            }
                            else
                            {
                                ShowMsgHelper.Alert_Error("Error!");
                            }
                        }
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
                    ShowMsgHelper.Alert("The file has been uploaded.");

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
            //产假计算
            if(LeaveID.SelectedValue=="3")
            {
                int intCJ = 98;
                DateTime dtCJed = dtBeginDate.AddDays(intCJ+30);
                int intCJT = CallCJDays(dtBeginDate, dtCJed);
                int intNC = 0;
                int intDBT = 0;
                int intTotalCJ = 0;
                if(cbNC.Checked==true)
                {
                    intNC = 15;
                }
                if(cbDBT.Checked==true)
                {
                    intDBT = int.Parse(DBT.Text) * 15;
                }
                intTotalCJ = intCJ + 30 + intCJT + intNC + intDBT;
                EndDate.Text = dtBeginDate.AddDays(intTotalCJ).ToString("yyyy-MM-dd");
                fResult = float.Parse(intTotalCJ.ToString());
                LeaveDays.Text = fResult.ToString();
            }
            else
            {
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

                LeaveDays.Text = fResult.ToString();
            }

        }

        private int CallCJDays(DateTime dtBeginDate,DateTime dtEndDate)
        {
            int intResult = 0;
            int intJR = 0;
            int intZM = 0;

            string sql = "select * from Base_ATS_HolidaySetting where (BeginDate>='" + dtBeginDate + "' and BeginDate<='" + dtEndDate + "') or (EndDate>='" + dtBeginDate + "' and EndDate<='" + dtEndDate + "')";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count > 0)
            {
                float fResult = 0;
                TimeSpan ts;
                for (int i=0;i<dt.Rows.Count;i++)
                {
                    DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
                    dtFormat.ShortDatePattern = "yyyy/MM/dd";
                    DateTime dtBDate = Convert.ToDateTime(dt.Rows[0]["BeginDate"].ToString(), dtFormat);
                    DateTime dtEDate = Convert.ToDateTime(dt.Rows[0]["EndDate"].ToString(), dtFormat);
                    ts = dtEDate - dtBDate;
                    fResult = ts.Days;
                    intJR = intJR + (int)fResult;
                }
            }

            for (DateTime dtCurr=dtBeginDate;dtCurr<=dtEndDate; dtCurr=dtCurr.AddDays(1))
            {
                if((int)dtCurr.DayOfWeek==0 || (int)dtCurr.DayOfWeek == 6)
                {
                    intZM = intZM + 1;
                }
            }
            intResult = intZM + intJR;
            return intResult;
        }

        protected void LeaveID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(LeaveID.SelectedValue=="3")
            {
                CJform.Visible = true;
            }
            else
            {
                CJform.Visible = false;
            }
        }

        protected void DBT_CheckedChanged(object sender, EventArgs e)
        {
            if(cbDBT.Checked==true)
            {
                DBT.Enabled = true;
            }
            else
            {
                DBT.Enabled = false;
            }
        }
    }
}