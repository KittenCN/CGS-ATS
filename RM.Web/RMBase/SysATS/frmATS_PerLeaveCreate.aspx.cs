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
        public static string strHRid;

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
                if(dt!=null && dt.Rows.Count>0)
                {
                    flo_njDays = float.Parse(dt.Rows[0].ItemArray[1].ToString());
                    flo_txDays = float.Parse(dt.Rows[0].ItemArray[2].ToString());
                }
                else
                {
                    flo_njDays = 0;
                    flo_txDays = 0;
                }
                sql = "select top 1 USER_ID from Base_UserInfo where hr=1 order by User_Code";
                sb_sql = new StringBuilder(sql);
                dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                if(dt!=null && dt.Rows.Count>0)
                {
                    strHRid = dt.Rows[0].ItemArray[0].ToString();
                }
                else
                {
                    strHRid = "0000";
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
            string strNextApprover = "";

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
                ShowMsgHelper.Alert_Wern("Date Error");
            }
            else
            {
                if (intMustFile==1 && (txt_FilesAdd == null || txt_FilesAdd == ""))
                {
                    ShowMsgHelper.Alert_Wern("Must Upload the Proof Files!");
                }
                else
                {
                    if(float.Parse(LeaveDays.Text)> intMaxPerTime || floAllLeaveDays>intMaxPerYear)
                    {
                        ShowMsgHelper.Alert_Wern("Over the Limit!");
                    }
                    else
                    {
                        if (LeaveID.SelectedValue == "7" && float.Parse(LeaveDays.Text) > float.Parse(njDays.Text))
                        {
                            ShowMsgHelper.Alert_Wern("Over the number of AL!");
                        }
                        else
                        {
                            string txt_NextApprover = "";
                            string sql = "select Boss_id from Base_UserInfo where user_id='" + txt_EmpID + "'";
                            StringBuilder sb_sql = new StringBuilder(sql);
                            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                            if (strHRid != "0000")
                            {
                                txt_NextApprover = strHRid;
                            }
                            else
                            {
                                if (dt.Rows[0].ItemArray[0] != null)
                                {
                                    txt_NextApprover = dt.Rows[0].ItemArray[0].ToString(); ;
                                }
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

                                if(BLisAutoApproval(txt_NextApprover))
                                {
                                    string strsql = "select id from Base_PerLeaveApply where EmpID='" + txt_EmpID + "' and LeaveID='" + LeaveID.SelectedValue + "' and BeginDate='" + BeginDate.Text + "' and EndDate='" + EndDate.Text + "' and CreateDate='" + CreateDate.Text + "' ";
                                    StringBuilder sbsql = new StringBuilder(strsql);
                                    DataTable dtsql = DataFactory.SqlDataBase().GetDataTableBySQL(sbsql);
                                    string strid = dtsql.Rows[0].ItemArray[0].ToString();
                                    strNextApprover = AutoApproval(txt_NextApprover, strid);
                                    while(BLisAutoApproval(strNextApprover))
                                    {
                                        strNextApprover = AutoApproval(strNextApprover, strid);
                                    }
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

        private Boolean BLisAutoApproval(string empid)
        {
            Boolean BLResult = false;
            string strsql = "select Auto_Approval from Base_UserInfo where user_id='" + empid + "' ";
            StringBuilder sbsql = new StringBuilder(strsql);
            DataTable dtsql = DataFactory.SqlDataBase().GetDataTableBySQL(sbsql);
            if(dtsql.Rows.Count>0 && dtsql.Rows[0].ItemArray[0]!=null)
            {
                if(dtsql.Rows[0].ItemArray[0].ToString()=="1")
                {
                    BLResult = true;
                }
                else
                {
                    BLResult = false;
                }
            }
            else
            {
                BLResult = false;
            }
            return BLResult;
        }

        private string AutoApproval(string strApproverID,string _key)
        {
            string strResult = "";
            string txt_NextApprover = "";
            //CallDays();
            string sql1 = "select Boss_id from Base_UserInfo where user_id='" + strApproverID + "'";
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

            txt_Remark = "System Auto Approval!";
            float flotxDays = float.Parse(txDays.Text);

            sql1 = "update Base_PerTravelApply set ApprovalFlag=" + int_AppStatus + ",NextApprover='" + txt_NextApprover + "' where id='" + _key + "' ";
            sb_sql1 = new StringBuilder(sql1);
            int i1 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql1);
            if (i1 > 0)
            {
                string Cur_Date = DateTime.Now.ToString("yyyy-MM-dd");
                string sql2 = "insert into Base_PerTravelApplyDetail(PTid,ApproverId,ApprovalStatus,ApprovalRemark,ApprovalDate) ";
                sql2 = sql2 + "select " + _key + ",'" + strApproverID + "'," + int_AppStatus + ",'" + txt_Remark + "','" + Cur_Date + "' ";
                StringBuilder sb_sql2 = new StringBuilder(sql2);
                int i2 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql2);
                if (i2 > 0)
                {
                    GenModel gm = new GenModel();
                    if (gm.GetEMailFromID(txt_EmpID) != null)
                    {
                        gm.SendMail2(gm.GetEMailFromID(txt_EmpID), "Your TraveList has been updated!", "Your TraveList has been updated!");
                    }
                    ShowMsgHelper.AlertMsg("Success");
                    if (txt_NextApprover == null || txt_NextApprover == "")
                    {
                        string sql3 = "update Base_LeaveConsole set SYTX=SYTX+" + flotxDays + " where EmpID='" + txt_EmpID + "' ";
                        StringBuilder sb_sql3 = new StringBuilder(sql3);
                        int i3 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql3);
                    }
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
            strResult = txt_NextApprover;
            return strResult;
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
            //产假Calculation
            if(LeaveID.SelectedValue=="3")  //产假特殊计算
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
                    intDBT = (int.Parse(DBT.Text)-1) * 15;  //修改为输入所有胎数
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
                //按工作日计算休假天数
                if(LeaveID.SelectedValue!="11")  //非陪产假,陪产假按自然日计算
                {
                    int intHolieWeek = HoliWeek(dtBeginDate, dtEndDate);
                    if (fResult - intHolieWeek < 0)
                    {
                        fResult = 0;
                    }
                    else
                    {
                        fResult = fResult - intHolieWeek;
                    }
                }
                LeaveDays.Text = fResult.ToString();
            }

        }

        private int HoliWeek(DateTime dtBeginDate,DateTime dtEndDate)
        {
            int intResult = 0;
            for(DateTime dti=dtBeginDate;dti<=dtEndDate;dti=dti.AddDays(1))
            {
                if(boolHoliWeek(dti))
                {
                    intResult = intResult + 1;
                }
            }
            return intResult;
        }

        private Boolean boolHoliWeek(DateTime CurrDate)
        {
            Boolean boolResult = false;
            string strSQL = "select * from Base_ATS_HolidaySetting where begindate>='" + CurrDate + "' and enddate<='" + CurrDate + "' ";
            StringBuilder sbSQL = new StringBuilder(strSQL);
            DataTable dtSQL = DataFactory.SqlDataBase().GetDataTableBySQL(sbSQL);
            if(dtSQL.Rows.Count>0)
            {
                boolResult = true;
            }
            else
            {
                if((int)CurrDate.DayOfWeek == 0 || (int)CurrDate.DayOfWeek == 6)
                {
                    boolResult = true;
                }
            }
            return boolResult;
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