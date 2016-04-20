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
    public partial class frmATS_PerTravelEdit : PageBase
    {
        string _key;
        public string txt_EmpID;
        public string txt_EmpName;
        public static string txt_FilesAdd;
        public static string txt_downFilesAdd;
        public static int int_AppFlag;
        public static int inttxDays;

        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];
            txt_EmpID = RequestSession.GetSessionUser().UserId.ToString();
            txt_EmpName = RequestSession.GetSessionUser().UserName.ToString();
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_key))
                {

                    string sql = "select ApprovalFlag,FilesAdd from Base_PerTravelApply where id='" + _key + "'";
                    StringBuilder sb_sql = new StringBuilder(sql);
                    DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                    if (dt.Rows[0].ItemArray[0] != null)
                    {
                        int_AppFlag = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                        InitData();
                        if (int_AppFlag != 0 && int_AppFlag != 3)
                        {
                            Save.Visible = false;
                            ShowMsgHelper.Alert_Error("申请已在审批中或已审批完成,不得被修改!");
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
                        ShowMsgHelper.Alert_Error("数据错误,请联系管理员");
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
                EmpID.Text = GetNameFromID(EmpID.Text);
                //lab_CreateDate.Text = "";
                BeginDate.Text = Convert.ToDateTime(BeginDate.Text).ToString("yyyy-MM-dd");
                EndDate.Text = Convert.ToDateTime(EndDate.Text).ToString("yyyy-MM-dd");

            }
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
                Hashtable ht = new Hashtable();
                ht = ControlBindHelper.GetWebControls(this.Page);
                //ht["EmpID"] = txt_EmpID;
                //ht["CreateDate"] = CreateDate.Text;
                //ht["ApprovalFlag"] = 0;
                if (int_AppFlag == 3) { ht["ApprovalFlag"] = 1; }
                //ht["NextApprover"] = txt_NextApprover;
                ht["FilesAdd"] = txt_FilesAdd;
                ht["TravelDays"] = TravelDays.Text;
                int IsOk = DataFactory.SqlDataBase().UpdateByHashtable("Base_PerTravelApply", "id", _key, ht);
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

        protected void btn_submit_Click(object sender, EventArgs e)
        {
            if (FilesAdd.PostedFile != null && FilesAdd.PostedFile.ContentLength > 0)
            {
                //string fn = System.IO.Path.GetFileName(FileAdd.PostedFile.FileName);
                string fn = string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + "_" + System.IO.Path.GetFileName(FilesAdd.PostedFile.FileName);
                string SaveLocation = Server.MapPath("TravelFiles") + "\\" + fn;
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

            for (DateTime dtT = dtBeginDate; dtT < dtEndDate; dtT = dtT.AddDays(1))
            {
                int intdtT = (int)dtT.DayOfWeek;
                if (intdtT == 6 || intdtT == 0)
                {
                    inttxDays = inttxDays + 1;
                }
            }
            txDays.Text = inttxDays.ToString();

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