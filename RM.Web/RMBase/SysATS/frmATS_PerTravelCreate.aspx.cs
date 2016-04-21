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
    public partial class frmATS_PerTravelCreate : PageBase
    {
        public string txt_EmpID;
        public string txt_EmpName;
        public static string txt_FilesAdd;
        public static float flotxDays;

        protected void Page_Load(object sender, EventArgs e)
        {
            txt_EmpID = RequestSession.GetSessionUser().UserId.ToString();
            txt_EmpName = RequestSession.GetSessionUser().UserName.ToString();

            if (!IsPostBack)
            {
                DataBindGrid();
            }
        }

        private void DataBindGrid()
        {
            EmpID.Text = txt_EmpName;
            CreateDate.Text = DateTime.Now.ToShortDateString();
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            btnTravelDays_Click(null, null);

            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            if ((Convert.ToDateTime(BeginDate.Text, dtFormat) > Convert.ToDateTime(EndDate.Text, dtFormat)) || (Convert.ToDateTime(BeginDate.Text, dtFormat) == Convert.ToDateTime(EndDate.Text, dtFormat) && BeginFlag.Value == "0" && EndFlag.Value == "0"))
            {
                ShowMsgHelper.Alert_Error("Date Error");
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
                ht["FilesAdd"] = txt_FilesAdd;
                ht["TravelDays"] = TravelDays.Text;
                int IsOk = DataFactory.SqlDataBase().InsertByHashtableReturnPkVal("Base_PerTravelApply", ht);
                if (IsOk > 0)
                {
                    GenModel gm = new GenModel();
                    if (txt_NextApprover != null && txt_NextApprover != "" && gm.GetEMailFromID(txt_NextApprover) != "" && gm.GetEMailFromID(txt_NextApprover) != null)
                    {
                        gm.SendMail2(gm.GetEMailFromID(txt_NextApprover), "You have a new Task!", "You have a new Task!");
                    }
                    if (gm.GetEMailFromID(txt_EmpID) != null)
                    {
                        gm.SendMail2(gm.GetEMailFromID(txt_EmpID), "Your TraveList has been updated!", "Your TraveList has been updated!");
                    }
                    ShowMsgHelper.AlertMsg("Success！");
                }
                else
                {
                    ShowMsgHelper.Alert_Error("Error！");
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

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (FileAdd.PostedFile != null && FileAdd.PostedFile.ContentLength > 0)
            {
                //string fn = System.IO.Path.GetFileName(FileAdd.PostedFile.FileName);
                string fn = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + "_" + System.IO.Path.GetFileName(FileAdd.PostedFile.FileName);
                string SaveLocation = Server.MapPath("TravelFiles") + "\\" + fn;
                try
                {
                    FileAdd.PostedFile.SaveAs(SaveLocation);
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
                ShowMsgHelper.Alert_Wern("Please select a file to upload.");
            }
        }

        protected void btnTravelDays_Click(object sender, EventArgs e)
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
            flotxDays = 0;  //初始化inttxDays

            for(DateTime dtT=dtBeginDate;dtT<dtEndDate.AddDays(1); dtT = dtT.AddDays(1))
            {
                int intdtT = (int)dtT.DayOfWeek;
                if(intdtT==6 || intdtT==0)
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
    }
}