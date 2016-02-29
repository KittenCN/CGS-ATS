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

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_ATSReport : PageBase
    {
        public static string txtJoinDate;
        public static string txtOutDate;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageControl1.pageHandler += new EventHandler(pager_PageChanged);
            DataBindGrid();

            if (!IsPostBack)
            {

            }
        }

        private void DataBindGrid()
        {
            int count = 0;
            txtJoinDate = "2015-11-01";
            txtOutDate = "2015-11-30";

            string sql = "select User_Name,USER_ID from Base_UserInfo where (join_date<='" + txtOutDate + "' and (out_date is null or out_date='1900-01-01' or out_date>='" + txtJoinDate + "'))";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetPageList(sql, null, "User_Name", "asc", PageControl1.PageIndex, PageControl1.PageSize, ref count);
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
                string txtEmpID="";
                Label lab_EmpID = e.Item.FindControl("USER_ID") as Label;
                Label lab_ygzts = e.Item.FindControl("ygzts") as Label;
                Label lab_jrts = e.Item.FindControl("jrts") as Label;

                if (lab_EmpID != null && lab_EmpID.Text!="")
                {
                    txtEmpID = lab_EmpID.Text;
                    lab_EmpID.Text = GetNameFromID(lab_EmpID.Text);
                }

                string txtBeginDate="1900-1-1";
                string txtEndDate= "1900-1-1";
                string sql = "select join_date,out_date from base_userinfo where user_id='" + txtEmpID + "' ";
                StringBuilder sb_sql = new StringBuilder(sql);
                DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                if(dt!=null && dt.Rows.Count>0)
                {
                    if (dt.Rows[0]["join_date"].ToString() != null && dt.Rows[0]["join_date"].ToString()!="")
                    {
                        if (DateTime.Parse(dt.Rows[0]["join_date"].ToString()) <= DateTime.Parse(txtJoinDate)) { txtBeginDate = txtJoinDate; }
                        if (DateTime.Parse(dt.Rows[0]["join_date"].ToString()) > DateTime.Parse(txtJoinDate) && DateTime.Parse(dt.Rows[0]["join_date"].ToString()) <= DateTime.Parse(txtOutDate)) { txtBeginDate = dt.Rows[0]["join_date"].ToString(); }
                    }
                    else
                    {
                        txtBeginDate = txtJoinDate;
                    }
                    if (dt.Rows[0]["out_date"].ToString() != null && dt.Rows[0]["out_date"].ToString()!="")
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


            }

        }

        private float CallWD(string BeginDate,string EndDate)
        {
            float floResult=0;
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
            if(dt!=null && dt.Rows.Count>0)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    dtHBeginDate = DateTime.Parse(dt.Rows[i]["BeginDate"].ToString());
                    dtHEndDate = DateTime.Parse(dt.Rows[i]["EndDate"].ToString());
                    intHBeginFlag = int.Parse(dt.Rows[i]["BeginFlag"].ToString());
                    intHEndFlag = int.Parse(dt.Rows[i]["EndFlag"].ToString());

                    for (DateTime dtT = dtBeginDate; dtT <= dtEndDate; dtT = dtT.AddDays(1))
                    {
                        if(dtT==dtHBeginDate)
                        {
                            switch(intHBeginFlag)
                            {                               
                                case 0:
                                    floResult = floResult + float.Parse("0.5");
                                    break;
                                case 1:
                                    floResult = floResult + float.Parse("1");
                                    break;
                            }
                        }
                        if (dtT == dtHEndDate && dtHBeginDate!=dtHEndDate)
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
                        if(dtT>dtHBeginDate && dtT < dtHEndDate)
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
    }
}