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
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetData;
using System.Collections;
using System.Threading;
using System.IO;    
//using Microsoft.Office.Interop.Excel;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_ATSReport : PageBase
    {
        public static string txtJoinDate;
        public static string txtOutDate;
        public static string txt_FilesAdd;
        public static DataTable dtExport=new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageControl1.pageHandler += new EventHandler(pager_PageChanged);
            DataBindGrid();
            if (!IsPostBack)
            {
                DateTime FirstDay = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
                DateTime LastDay = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);

                FirstDay = DateTime.Parse("2015-11-1");
                LastDay = DateTime.Parse("2015-11-30");

                tb_BeginDate.Text = FirstDay.ToString("yyyy-MM-dd");
                tb_EndDate.Text = LastDay.ToString("yyyy-MM-dd");

                dtExport = new DataTable();
                dtExport.Columns.Add(new DataColumn("Name", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Days of Work", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Working Days", typeof(string)));
                dtExport.Columns.Add(new DataColumn("LL times(<= 4h)", typeof(string)));
                dtExport.Columns.Add(new DataColumn("LL times(> 4h)", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Absent times", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Personal leave", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Marital Leave", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Maternity Leave", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Funeral Leave", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Leave in lieu", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Sick Leave", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Annual Leave", typeof(string)));
                dtExport.Columns.Add(new DataColumn("Actual Working Days", typeof(string)));
            }
        }

        private void DataBindGrid()
        {
            int count = 0;
            if (dtExport.Rows.Count > 0)
            {
                for (int i = dtExport.Rows.Count - 1; i >= 0; i--)
                {
                    dtExport.Rows.RemoveAt(i);
                }
            }
            txtJoinDate = tb_BeginDate.Text;
            txtOutDate = tb_EndDate.Text;
            if(txtJoinDate!="" || txtOutDate!="")
            {
                string sql = "select User_Name,USER_ID from Base_UserInfo where (join_date<='" + txtOutDate + "' and (out_date is null or out_date='1900-01-01' or out_date>='" + txtJoinDate + "'))";
                StringBuilder sb_sql = new StringBuilder(sql);
                DataTable dt = DataFactory.SqlDataBase().GetPageList(sql, null, "User_Name", "asc", PageControl1.PageIndex, PageControl1.PageSize, ref count);
                ControlBindHelper.BindRepeaterList(dt, rp_Item);
                this.PageControl1.RecordCount = Convert.ToInt32(count);
            }
        }

        protected void pager_PageChanged(object sender, EventArgs e)
        {
            DataBindGrid();
        }
        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string txtEmpID = "";
                Label lab_EmpID = e.Item.FindControl("USER_ID") as Label;
                Label lab_ygzts = e.Item.FindControl("ygzts") as Label;
                Label lab_jrts = e.Item.FindControl("jrts") as Label;

                if (lab_EmpID != null && lab_EmpID.Text != "")
                {
                    txtEmpID = lab_EmpID.Text;
                    lab_EmpID.Text = GetNameFromID(lab_EmpID.Text);
                }

                string txtBeginDate = "1900-1-1";
                string txtEndDate = "1900-1-1";
                //txtEmpID = "80cc9090-885f-4bec-a96e-93df654a8d53";
                string sql = "select join_date,out_date from base_userinfo where user_id='" + txtEmpID + "' ";
                StringBuilder sb_sql = new StringBuilder(sql);
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["join_date"].ToString() != null && dt.Rows[0]["join_date"].ToString() != "" && DateTime.Parse(dt.Rows[0]["join_date"].ToString()).Year!=1900)
                    {
                        if (DateTime.Parse(dt.Rows[0]["join_date"].ToString()) <= DateTime.Parse(txtJoinDate)) { txtBeginDate = txtJoinDate; }
                        if (DateTime.Parse(dt.Rows[0]["join_date"].ToString()) > DateTime.Parse(txtJoinDate) && DateTime.Parse(dt.Rows[0]["join_date"].ToString()) <= DateTime.Parse(txtOutDate)) { txtBeginDate = dt.Rows[0]["join_date"].ToString(); }
                    }
                    else
                    {
                        txtBeginDate = txtJoinDate;
                    }
                    if (dt.Rows[0]["out_date"].ToString() != null && dt.Rows[0]["out_date"].ToString() != "" && DateTime.Parse(dt.Rows[0]["out_date"].ToString()).Year != 1900)
                    {
                        if (DateTime.Parse(dt.Rows[0]["out_date"].ToString()) >= DateTime.Parse(txtOutDate)) { txtEndDate = txtOutDate; }
                        if (DateTime.Parse(dt.Rows[0]["out_date"].ToString()) < DateTime.Parse(txtOutDate) && DateTime.Parse(dt.Rows[0]["out_date"].ToString()) >= DateTime.Parse(txtJoinDate)) { txtEndDate = dt.Rows[0]["out_date"].ToString(); }
                    }
                    else
                    {
                        txtEndDate = txtOutDate;
                    }
                    lab_ygzts.Text = CallWD(txtBeginDate, txtEndDate).ToString();
                    lab_jrts.Text = CallHoliday(txtBeginDate, txtEndDate).ToString();
                }

                float czl = 0;
                float czp = 0;
                float kgcs = 0; //旷工
                float shjts = 0;
                float hjts = 0;
                float cjts = 0;
                float sjts = 0;
                float txjts = 0;
                float bjts = 0;
                float njts = 0;
                float sjgz = 0;

                Label lab_czl = e.Item.FindControl("czl") as Label;
                Label lab_czp = e.Item.FindControl("czp") as Label;
                Label lab_kgcs = e.Item.FindControl("kgcs") as Label;
                Label lab_shjts = e.Item.FindControl("shjts") as Label;
                Label lab_hjts = e.Item.FindControl("hjts") as Label;
                Label lab_cjts = e.Item.FindControl("cjts") as Label;
                Label lab_sjts = e.Item.FindControl("sjts") as Label;
                Label lab_txjts = e.Item.FindControl("txjts") as Label;
                Label lab_bjts = e.Item.FindControl("bjts") as Label;
                Label lab_njts = e.Item.FindControl("njts") as Label;
                Label lab_sjgz = e.Item.FindControl("sjgz") as Label;

                //txtEmpID = "80cc9090-885f-4bec-a96e-93df654a8d53";
                sql = "select empid,iid,sum(calldays) as calldays from uvw_CallDyas where empid='" + txtEmpID + "' and ats_date>='" + txtJoinDate + "' and ats_date<='" + txtOutDate + "' group by empid,iid ";
                sb_sql = new StringBuilder(sql);
                dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        switch (int.Parse(dt.Rows[i]["iid"].ToString()))
                        {
                            case 222:
                                czl = float.Parse(dt.Rows[i]["calldays"].ToString());
                                kgcs = kgcs +  float.Parse((float.Parse((int.Parse(czl.ToString()) / 2).ToString()) * 0.5).ToString());
                                break;
                            case 333:
                                czp = float.Parse(dt.Rows[i]["calldays"].ToString());
                                kgcs = kgcs + float.Parse((float.Parse((int.Parse(czp.ToString()) / 2).ToString()) * 1).ToString());
                                break;
                            case 999:
                                txjts = float.Parse(dt.Rows[i]["calldays"].ToString());
                                break;
                            case 2:
                                shjts = float.Parse(dt.Rows[i]["calldays"].ToString());
                                break;
                            case 1:
                                hjts = float.Parse(dt.Rows[i]["calldays"].ToString());
                                break;
                            case 3:
                                cjts = float.Parse(dt.Rows[i]["calldays"].ToString());
                                break;
                            case 4:
                                sjts = float.Parse(dt.Rows[i]["calldays"].ToString());
                                break;
                            case 5:
                                txjts = float.Parse(dt.Rows[i]["calldays"].ToString());
                                break;
                            case 6:
                                bjts = float.Parse(dt.Rows[i]["calldays"].ToString());
                                break;
                            case 7:
                                njts = float.Parse(dt.Rows[i]["calldays"].ToString());
                                break;
                        }
                    }
                }
                lab_czl.Text = czl.ToString();
                lab_czp.Text = czp.ToString();
                lab_kgcs.Text = kgcs.ToString();
                lab_shjts.Text = shjts.ToString();
                lab_hjts.Text = hjts.ToString();
                lab_cjts.Text = cjts.ToString();
                lab_sjts.Text = sjts.ToString();
                lab_txjts.Text = txjts.ToString();
                lab_bjts.Text = bjts.ToString();
                lab_njts.Text = njts.ToString();
                lab_sjgz.Text = sjgz.ToString();
                lab_sjgz.Text = (float.Parse(lab_ygzts.Text) - float.Parse(lab_jrts.Text) - shjts - hjts - cjts - sjts - txjts - bjts - njts - sjgz).ToString();

                DataRow drRow = dtExport.NewRow();
                drRow[0] = lab_EmpID.Text;
                drRow[1] = lab_ygzts.Text;
                drRow[2] = lab_jrts.Text;
                drRow[3] = lab_czl.Text;
                drRow[4] = lab_czp.Text;
                drRow[5] = lab_kgcs.Text;
                drRow[6] = lab_shjts.Text;
                drRow[7] = lab_hjts.Text;
                drRow[8] = lab_cjts.Text;
                drRow[9] = lab_sjts.Text;
                drRow[10] = lab_txjts.Text;
                drRow[11] = lab_bjts.Text;
                drRow[12] = lab_njts.Text;
                drRow[13] = lab_sjgz.Text;
                dtExport.Rows.Add(drRow);
            }

        }

        private float CallWD(string BeginDate, string EndDate)
        {
            float floResult = 0;
            for (DateTime dtT = DateTime.Parse(BeginDate); dtT <= DateTime.Parse(EndDate); dtT = dtT.AddDays(1))
            {
                int intdtT = (int)dtT.DayOfWeek;
                if (intdtT != 6 && intdtT != 0)
                {
                    floResult = floResult + 1;
                }
            }
            return floResult;
        }

        private float CallHoliday(string BeginDate, string EndDate)
        {
            float floResult = 0;
            DateTime dtHBeginDate;
            DateTime dtHEndDate;
            int intHBeginFlag = 1;
            int intHEndFlag = 1;
            DateTime dtBeginDate = DateTime.Parse(BeginDate);
            DateTime dtEndDate = DateTime.Parse(EndDate);

            string sql = "select * from Base_ATS_HolidaySetting where BeginDate<='" + txtOutDate + "' and EndDate>='" + txtJoinDate + "'";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dtHBeginDate = DateTime.Parse(dt.Rows[i]["BeginDate"].ToString());
                    dtHEndDate = DateTime.Parse(dt.Rows[i]["EndDate"].ToString());
                    intHBeginFlag = int.Parse(dt.Rows[i]["BeginFlag"].ToString());
                    intHEndFlag = int.Parse(dt.Rows[i]["EndFlag"].ToString());

                    for (DateTime dtT = dtBeginDate; dtT <= dtEndDate; dtT = dtT.AddDays(1))
                    {
                        if (dtT == dtHBeginDate)
                        {
                            switch (intHBeginFlag)
                            {
                                case 0:
                                    floResult = floResult + float.Parse("0.5");
                                    break;
                                case 1:
                                    floResult = floResult + float.Parse("1");
                                    break;
                            }
                        }
                        if (dtT == dtHEndDate && dtHBeginDate != dtHEndDate)
                        {
                            switch (intHEndFlag)
                            {
                                case 0:
                                    floResult = floResult + float.Parse("0.5");
                                    break;
                                case 1:
                                    floResult = floResult + float.Parse("1");
                                    break;
                            }
                        }
                        if (dtT > dtHBeginDate && dtT < dtHEndDate)
                        { floResult = floResult + float.Parse("1"); }
                    }
                }
            }

            return floResult;
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

        private string GetIDfromCode(string EmpCode)
        {
            string txt_Result = "";

            string sql = "select User_ID from Base_UserInfo where User_Code='" + EmpCode + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count != 0 && dt.Rows[0].ItemArray[0].ToString() != "")
            {
                txt_Result = dt.Rows[0].ItemArray[0].ToString();
            }

            return txt_Result;
        }

        protected void btn_LVsearch_Click(object sender, EventArgs e)
        {
            DataBindGrid();
        }

        protected void btn_Export_Click(object sender, EventArgs e)
        {
            txtJoinDate = tb_BeginDate.Text;
            txtOutDate = tb_EndDate.Text;
            if (txtJoinDate != "" || txtOutDate != "")
            {
                //string sql = "select User_Name,USER_ID from Base_UserInfo where (join_date<='" + txtOutDate + "' and (out_date is null or out_date='1900-01-01' or out_date>='" + txtJoinDate + "'))";
                //StringBuilder sb_sql = new StringBuilder(sql);
                //DataTable dt = DataFactory.SqlDataBase().GetPageList(sql, null, "User_Name", "asc", 1, 99999, ref count);
                DataTable dt = dtExport;
                GenModel GM = new GenModel();
                string fn = txtJoinDate + "_" + txtOutDate + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "ATSReport.xls";
                string SaveLocation = Server.MapPath("TravelFiles") + "\\" + fn;
                int intResult = GM.ExportExcel(dt, SaveLocation);
                //GM.rpExportExcel(ref rp_Item, fn, "application/ms-excel");
                if(intResult!=-1)
                {
                    string fileName = "ceshi.rar";//客户端保存的文件名
                    string filePath = SaveLocation;//路径
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
            }
        }
        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
        }
    }
}