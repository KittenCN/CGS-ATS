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
    public partial class frmATS_PerLeaveEdit : PageBase
    {
        string _key;
        public string txt_EmpID;
        public string txt_EmpName;
        public static string txt_FilesAdd;
        public static string txt_downFilesAdd;
        public static int int_AppFlag;
        public static float flo_njDays;
        public static float flo_txDays;

        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];
            txt_EmpID = RequestSession.GetSessionUser().UserId.ToString();
            txt_EmpName = RequestSession.GetSessionUser().UserName.ToString();

            if (!IsPostBack)
            {
                string sql = "select * from uvw_doLeaveDays where EmpID='" + txt_EmpID + "' ";
                StringBuilder sb_sql = new StringBuilder(sql);
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                CJform.Visible = false;
                if (dt != null || dt.Rows.Count > 0)
                {
                    flo_njDays = float.Parse(dt.Rows[0].ItemArray[1].ToString());
                    flo_txDays = float.Parse(dt.Rows[0].ItemArray[2].ToString());
                }
                else
                {
                    flo_njDays = 0;
                    flo_txDays = 0;
                }

                if (!string.IsNullOrEmpty(_key))
                {
                    sql = "select ApprovalFlag,FilesAdd from Base_PerLeaveApply where id='" + _key + "'";
                    sb_sql = new StringBuilder(sql);
                    dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                    if (dt.Rows[0].ItemArray[0] != null)
                    {
                        int_AppFlag = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                        InitData();
                        if (int_AppFlag != 0 && int_AppFlag != 3)
                        {
                            Save.Visible = false;
                            ShowMsgHelper.Alert_Wern("申请已在审批中或已审批完成,不得被修改!");
                        }
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
                    else
                    {
                        ShowMsgHelper.Alert_Wern("数据错误,请联系管理员");
                    }
                    
                }
            }
        }

        private void InitData()
        {
            string sql = "select * from Base_ATS_LeaveSetting";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            LeaveID.DataSource = dt;
            LeaveID.DataTextField = "LeaveName";
            LeaveID.DataValueField = "id";
            LeaveID.DataBind();

            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_PerLeaveApply", "id", _key);
            if (ht.Count > 0 && ht != null)
            {
                if(ht["LeaveID"].ToString()=="3")
                {
                    CJform.Visible = true;
                }
                else
                {
                    CJform.Visible = false;
                }
                ControlBindHelper.SetWebControls(this.Page, ht);
                EmpID.Text = GetNameFromID(EmpID.Text);
                //lab_CreateDate.Text = "";
                BeginDate.Text = Convert.ToDateTime(BeginDate.Text).ToString("yyyy-MM-dd");
                EndDate.Text = Convert.ToDateTime(EndDate.Text).ToString("yyyy-MM-dd");
                if(ht["NCJ"].ToString()=="1")
                {
                    cbNC.Checked = true;
                }                       
                else
                {
                    cbNC.Checked = false;
                }   
                if(ht["DBT"].ToString()=="1")
                {
                    cbDBT.Checked = true;
                    DBT.Text = ht["DBTnum"].ToString();
                }
                else
                {
                    cbDBT.Checked = false;
                    DBT.Text = "0";
                }
            }

            njDays.Text = flo_njDays.ToString();
            txDays.Text = flo_txDays.ToString();
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            btnLeaveDays_Click(null, null);
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
            if (dti != null && dti.Rows.Count > 0)
            {
                if (int.Parse(dti.Rows[0].ItemArray[2].ToString()) > 0)
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
                if (intMustFile == 1 && (txt_FilesAdd == null || txt_FilesAdd == ""))
                {
                    ShowMsgHelper.Alert_Wern("必须上传证明附件!");
                }
                else
                {
                    if (float.Parse(LeaveDays.Text) > intMaxPerTime || floAllLeaveDays > intMaxPerYear)
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
                            Hashtable ht = new Hashtable();
                            ht = ControlBindHelper.GetWebControls(this.Page);
                            //ht["EmpID"] = txt_EmpID;
                            //ht["CreateDate"] = CreateDate.Text;
                            //ht["ApprovalFlag"] = 0;
                            if (int_AppFlag == 3) { ht["ApprovalFlag"] = 1; }
                            //ht["NextApprover"] = txt_NextApprover;
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
                            int IsOk = DataFactory.SqlDataBase().UpdateByHashtable("Base_PerLeaveApply", "id", _key, ht);
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
            }
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
                    ShowMsgHelper.Alert_Error("Error: " + ex.Message);
                }
            }
            else
            {
                //Response.Write("Please select a file to upload.");
                ShowMsgHelper.Alert_Wern("Please select a file to upload.");
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

        protected void btn_down_Click(object sender, EventArgs e)
        {
            //WriteFile实现下载
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

            if (LeaveID.SelectedValue == "3")
            {
                int intCJ = 98;
                DateTime dtCJed = dtBeginDate.AddDays(intCJ + 30);
                int intCJT = CallCJDays(dtBeginDate, dtCJed);
                int intNC = 0;
                int intDBT = 0;
                int intTotalCJ = 0;
                if (cbNC.Checked == true)
                {
                    intNC = 15;
                }
                if (cbDBT.Checked == true)
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

        private int CallCJDays(DateTime dtBeginDate, DateTime dtEndDate)
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
                for (int i = 0; i < dt.Rows.Count; i++)
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

            for (DateTime dtCurr = dtBeginDate; dtCurr <= dtEndDate; dtCurr = dtCurr.AddDays(1))
            {
                if ((int)dtCurr.DayOfWeek == 0 || (int)dtCurr.DayOfWeek == 6)
                {
                    intZM = intZM + 1;
                }
            }
            intResult = intZM + intJR;
            return intResult;
        }

        protected void LeaveID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LeaveID.SelectedValue == "3")
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
            if (cbDBT.Checked == true)
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