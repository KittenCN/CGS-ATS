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
    public partial class frmATS_PerLeaveAppPro : PageBase
    {
        string _key;
        public string txt_EmpID;
        public string txt_EmpName;
        public static string txt_FilesAdd;
        public static string txt_downFilesAdd;
        public static string txt_NextApprover;
        public static string strUserID;

        protected void Page_Load(object sender, EventArgs e)
        {
            int i = 0;
            _key = Request["key"];
            txt_EmpID = RequestSession.GetSessionUser().UserId.ToString();
            txt_EmpName = RequestSession.GetSessionUser().UserName.ToString();

            string sql = "select ApprovalFlag,EmpID from Base_PerLeaveApply where id='" + _key + "' ";
            StringBuilder sbsql = new StringBuilder(sql);
            DataTable dtsql = DataFactory.SqlDataBase().GetDataTableBySQL(sbsql);
            if (dtsql.Rows[0].ItemArray[0].ToString() == "0")
            {
                string sql1 = "select Boss_id from Base_UserInfo where user_id='" + dtsql.Rows[0].ItemArray[1].ToString() + "'";
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
            }
            else
            {
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
            }

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_key))
                {

                    sql = "select ApprovalFlag,FilesAdd from Base_PerLeaveApply where id='" + _key + "'";
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
                ControlBindHelper.SetWebControls(this.Page, ht);
                strUserID = EmpID.Text;
                EmpID.Text = GetNameFromID(EmpID.Text);
                //lab_CreateDate.Text = "";
                BeginDate.Text = Convert.ToDateTime(BeginDate.Text).ToString("yyyy-MM-dd");
                EndDate.Text = Convert.ToDateTime(EndDate.Text).ToString("yyyy-MM-dd");

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

        protected void Pass_Click(object sender, EventArgs e)
        {
            string txt_Remark = "";
            int int_AppStatus = 99;

            if(txt_NextApprover==null ||txt_NextApprover.Length<=0)
            {
                int_AppStatus = 2;
                txt_NextApprover = "";
            }
            else
            {
                int_AppStatus = 1;
            }

            txt_Remark = ApprovalRemark.InnerText;

            string sql1 = "update Base_PerLeaveApply set ApprovalFlag=" + int_AppStatus + ",NextApprover='" + txt_NextApprover + "' where id='" + _key + "' ";
            StringBuilder sb_sql1 = new StringBuilder(sql1);
            int i1 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql1);
            if(i1>0)
            {
                string Cur_Date = DateTime.Now.ToString("yyyy-MM-dd");
                string sql2 = "insert into Base_PerLeaveApplyDetail(PAid,ApproverId,ApprovalStatus,ApprovalRemark,ApprovalDate) ";
                sql2 = sql2 + "select " + _key + ",'" + txt_EmpID + "'," + int_AppStatus + ",'" + txt_Remark + "','" + Cur_Date + "' ";
                StringBuilder sb_sql2 = new StringBuilder(sql2);
                int i2 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql2);
                if(i2>0)
                {
                    GenModel gm = new GenModel();
                    if (txt_NextApprover != null && txt_NextApprover != "" && gm.GetEMailFromID(txt_NextApprover) != "" && gm.GetEMailFromID(txt_NextApprover) != null)
                    {
                        gm.SendMail2(gm.GetEMailFromID(txt_NextApprover), "You have a new Task!", "You have a new Task!");
                    }
                    if (gm.GetEMailFromID(strUserID) != null)
                    {
                        gm.SendMail2(gm.GetEMailFromID(strUserID), "Your LeaveList has been updated!", "Your LeaveList has been updated!");
                    }
                    if (txt_NextApprover == null || txt_NextApprover == "")
                    {
                        
                    }
                    else
                    {
                        //string strNextApprover = AutoApproval(txt_NextApprover, _key);
                        string strNextApprover = txt_NextApprover;
                        while (BLisAutoApproval(strNextApprover))
                        {
                            strNextApprover = AutoApproval(strNextApprover, _key);
                        }
                    }
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

        private Boolean BLisAutoApproval(string empid)
        {
            Boolean BLResult = false;
            string strsql = "select Auto_Approval from Base_UserInfo where user_id='" + empid + "' ";
            StringBuilder sbsql = new StringBuilder(strsql);
            DataTable dtsql = DataFactory.SqlDataBase().GetDataTableBySQL(sbsql);
            if (dtsql.Rows.Count > 0 && dtsql.Rows[0].ItemArray[0] != null)
            {
                if (dtsql.Rows[0].ItemArray[0].ToString() == "1")
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

        private string AutoApproval(string strApproverID, string _key)
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
            //float flotxDays = float.Parse(txDays.Text);

            sql1 = "update Base_PerLeaveApply set ApprovalFlag=" + int_AppStatus + ",NextApprover='" + txt_NextApprover + "' where id='" + _key + "' ";
            sb_sql1 = new StringBuilder(sql1);
            int i1 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql1);
            if (i1 > 0)
            {
                string Cur_Date = DateTime.Now.ToString("yyyy-MM-dd");
                string sql2 = "insert into Base_PerLeaveApplyDetail(PAid,ApproverId,ApprovalStatus,ApprovalRemark,ApprovalDate) ";
                sql2 = sql2 + "select " + _key + ",'" + strApproverID + "'," + int_AppStatus + ",'" + txt_Remark + "','" + Cur_Date + "' ";
                StringBuilder sb_sql2 = new StringBuilder(sql2);
                int i2 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql2);
                if (i2 > 0)
                {
                    GenModel gm = new GenModel();
                    if (gm.GetEMailFromID(txt_EmpID) != null)
                    {
                        gm.SendMail2(gm.GetEMailFromID(txt_EmpID), "Your LeaveList has been updated!", "Your LeaveList has been updated!");
                    }
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
            strResult = txt_NextApprover;
            return strResult;
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

            string sql1 = "update Base_PerLeaveApply set ApprovalFlag=" + int_AppStatus + " where id='" + _key + "' ";
            StringBuilder sb_sql1 = new StringBuilder(sql1);
            int i1 = DataFactory.SqlDataBase().ExecuteBySql(sb_sql1);
            if (i1 > 0)
            {
                string Cur_Date = DateTime.Now.ToString("yyyy-MM-dd");
                string sql2 = "insert into Base_PerLeaveApplyDetail(PAid,ApproverId,ApprovalStatus,ApprovalRemark,ApprovalDate) ";
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