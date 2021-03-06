﻿using RM.Busines;
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


namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_ATSResult : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageControl1.pageHandler += new EventHandler(pager_PageChanged);
            DataBindGrid();
            if (!IsPostBack)
            {
                DateTime FirstDay = DateTime.Now.AddDays(-DateTime.Now.Day + 1);
                DateTime LastDay = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);

                FirstDay = DateTime.Parse("2016-1-1");
                LastDay = DateTime.Parse("2016-1-31");

                tb_BeginDate.Text = FirstDay.ToString("yyyy-MM-dd");
                tb_EndDate.Text = LastDay.ToString("yyyy-MM-dd");
            }
        }

        private void DataBindGrid()
        {
            int count = 0;
            string txtBeginDate = tb_BeginDate.Text;
            string txtEndDate = tb_EndDate.Text;
            string strEXSQL = " 1=1 ";

            //初始化下拉框
            string dlsql = "select user_name from Base_UserInfo";
            StringBuilder sb_dlsql = new StringBuilder(dlsql);
            DataTable dldt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_dlsql);
            var builder = new System.Text.StringBuilder();
            for (int i = 0; i < dldt.Rows.Count; i++)
                builder.Append(String.Format("<option value='{0}'>", dldt.Rows[i][0]));
            Emplist.InnerHtml = builder.ToString();

            if(EmpID.Value!=null && EmpID.Value!="")
            {
                strEXSQL = " and  EmpID='" + GetIDFromName(EmpID.Value) + "' ";
            }
            else
            {
                strEXSQL = " and 1=1 ";
            }

            StringBuilder SqlWhere = new StringBuilder();
            IList<SqlParam> IList_param = new List<SqlParam>();
            //DataTable dt = DataFactory.SqlDataBase().GetDataTable("Base_ATS_OriDataIn");
            string sql = "select * from Base_ATSResult where ATS_Date>='" + txtBeginDate + "' and ATS_Date<='" + txtEndDate + "' " + strEXSQL;
            StringBuilder sb_sql = new StringBuilder(sql);
            // DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            DataTable dt = DataFactory.SqlDataBase().GetPageList(sql, null, "ATS_Date", "asc", PageControl1.PageIndex, PageControl1.PageSize, ref count);
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
            this.PageControl1.RecordCount = Convert.ToInt32(count);


        }

        private string GetIDFromName(string EmpName)
        {
            string txt_Result = "";

            string sql = "select User_ID from Base_UserInfo where User_name='" + EmpName + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count != 0 && dt.Rows[0].ItemArray[0].ToString() != "")
            {
                txt_Result = dt.Rows[0].ItemArray[0].ToString();
            }

            return txt_Result;
        }

        protected void pager_PageChanged(object sender, EventArgs e)
        {
            DataBindGrid();
        }

        protected void btn_ATSCheck_Click(object sender, EventArgs e)
        {
            int count = 0;
            string txtBeginDate = tb_BeginDate.Text;
            string txtEndDate = tb_EndDate.Text;
            DateTime dtBeginDate = DateTime.Parse(txtBeginDate);
            DateTime dtEndDate = DateTime.Parse(txtEndDate);

            //屏蔽按钮
            btn_ATSCheck.Enabled = false;
            btn_EditTravel.Enabled = false;
            btn_Search.Enabled = false;
            btn_SetNor.Enabled = false;
            System.Threading.Thread.Sleep(5000);

            //删除时间段内的记录
            string sql = "delete from Base_ATSResult where ATS_Date>='" + txtBeginDate + "' and ATS_Date<='" + txtEndDate + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            int int_sqlresult = DataFactory.SqlDataBase().ExecuteBySql(sb_sql);

            for (DateTime dt_ATS_Date = dtBeginDate; dt_ATS_Date <= dtEndDate; dt_ATS_Date = dt_ATS_Date.AddDays(1))
            {
                string txt_ATS_Date = dt_ATS_Date.ToShortDateString();
                int int_ATS_DateStatus = (int)DateTime.Parse(txt_ATS_Date).DayOfWeek;

                string insql = "select * from Base_UserInfo where (out_date='1900-1-1' or isnull(out_date,'')='') ";
                StringBuilder insb_sql = new StringBuilder(insql);
                DataTable indt = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sql);
                if (indt.Rows.Count > 0)
                {
                    int indt_count = indt.Rows.Count;
                    for (int i = 0; i < indt_count; i++)
                    {
                        string txt_EmpID = indt.Rows[i].ItemArray[0].ToString();
                        string txt_EmpCode = indt.Rows[i].ItemArray[1].ToString();
                        //判断是否重复
                        sql = "select * from Base_ATSResult where EmpID='" + txt_EmpID + "' and ATS_Date='" + txt_ATS_Date + "' ";
                        sb_sql = new StringBuilder(sql);
                        DataTable dt2 = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
                        if (dt2.Rows.Count == 0)
                        {
                            //按人,日期插入基础数据
                            int intATS_Result = 0;
                            if (int_ATS_DateStatus == 0 || int_ATS_DateStatus == 6)
                            {
                                intATS_Result = 1;
                            }
                            sql = "insert into Base_ATSResult(EmpID,EmpCode,Flag,ATS_Date,ATS_DateStatus,PunchINTime,PunchOutTime,ATS_Result) ";
                            sql = sql + "select '" + txt_EmpID + "','" + txt_EmpCode + "',1,'" + txt_ATS_Date + "'," + int_ATS_DateStatus + ",'00:00:00','00:00:00'," + intATS_Result;
                            sb_sql = new StringBuilder(sql);
                            int_sqlresult = DataFactory.SqlDataBase().ExecuteBySql(sb_sql);
                        }

                        //获取休假记录
                        int intATSResult = 0;
                        int int_ATS_Leave = 0;
                        int int_ATS_LeaveID = 0;
                        int int_ATS_LeaveStatus = 0;
                        //txt_EmpID = "80cc9090-885f-4bec-a96e-93df654a8d53";
                        string insqli = "select * from dbo.Base_PerLeaveApply where ApprovalFlag=2 and '" + txt_ATS_Date + "' between BeginDate and EndDate and EmpID='" + txt_EmpID + "' ";
                        StringBuilder insb_sqli = new StringBuilder(insqli);
                        DataTable indti = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sqli);
                        if (indti.Rows.Count != 0 && indti.Rows[0].ItemArray[0].ToString().Length != 0)
                        {
                            DateTime dt_LBeginDate = DateTime.Parse(indti.Rows[0].ItemArray[3].ToString());
                            DateTime dt_LEndDate = DateTime.Parse(indti.Rows[0].ItemArray[5].ToString());
                            int int_LBeginDateFlag = int.Parse(indti.Rows[0].ItemArray[4].ToString());
                            int int_LEndDateFlag = int.Parse(indti.Rows[0].ItemArray[6].ToString());
                            dt_ATS_Date = DateTime.Parse(txt_ATS_Date);

                            int_ATS_Leave = int.Parse(indti.Rows[0].ItemArray[2].ToString());
                            int_ATS_LeaveID = int.Parse(indti.Rows[0].ItemArray[0].ToString());

                            if (dt_ATS_Date == dt_LBeginDate && dt_LBeginDate != dt_LEndDate)
                            {
                                switch (int_LBeginDateFlag)
                                {
                                    case 0:
                                        {
                                            int_ATS_LeaveStatus = 1;
                                            intATSResult = 0;
                                            break;
                                        }
                                    case 1:
                                        {
                                            int_ATS_LeaveStatus = 0;
                                            intATSResult = 1;
                                            break;
                                        }
                                }


                            }
                            if (dt_ATS_Date == dt_LEndDate && dt_LBeginDate != dt_LEndDate)
                            {
                                switch (int_LEndDateFlag)
                                {
                                    case 0:
                                        {
                                            int_ATS_LeaveStatus = 1;
                                            intATSResult = 0;
                                            break;
                                        }
                                    case 1:
                                        {
                                            int_ATS_LeaveStatus = 0;
                                            intATSResult = 1;
                                            break;
                                        }
                                }
                            }
                            if (dt_ATS_Date == dt_LEndDate && dt_LBeginDate == dt_LEndDate)
                            {
                                if (int_LBeginDateFlag == 0 && int_LEndDateFlag == 1)
                                {
                                    int_ATS_LeaveStatus = 1;
                                    intATSResult = 0;
                                }
                                if (int_LBeginDateFlag == 1 && int_LEndDateFlag == 0)
                                {
                                    int_ATS_LeaveStatus = 1;
                                    intATSResult = 0;
                                }
                                if (int_LBeginDateFlag == 1 && int_LEndDateFlag == 1)
                                {
                                    int_ATS_LeaveStatus = 0;
                                    intATSResult = 1;
                                }
                            }
                        }

                        //获取公出记录
                        int int_ATS_Travel = 0;
                        int int_ATS_TravelID = 0;
                        int int_ATS_TravelStatus = 0;
                        insqli = "select * from dbo.Base_PerTravelApply where ApprovalFlag=2 and '" + txt_ATS_Date + "' between BeginDate and EndDate and EmpID='" + txt_EmpID + "' ";
                        insb_sqli = new StringBuilder(insqli);
                        indti = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sqli);
                        if (indti.Rows.Count != 0 && indti.Rows[0].ItemArray[0].ToString().Length != 0)
                        {
                            DateTime dt_TBeginDate = DateTime.Parse(indti.Rows[0].ItemArray[2].ToString());
                            DateTime dt_TEndDate = DateTime.Parse(indti.Rows[0].ItemArray[4].ToString());
                            int int_TBeginDateFlag = int.Parse(indti.Rows[0].ItemArray[3].ToString());
                            int int_TEndDateFlag = int.Parse(indti.Rows[0].ItemArray[5].ToString());
                            dt_ATS_Date = DateTime.Parse(txt_ATS_Date);

                            int_ATS_Travel = int.Parse(indti.Rows[0].ItemArray[0].ToString());
                            int_ATS_TravelID = int.Parse(indti.Rows[0].ItemArray[0].ToString());

                            if (dt_ATS_Date == dt_TBeginDate && dt_TBeginDate != dt_TEndDate)
                            {
                                switch (int_TBeginDateFlag)
                                {
                                    case 0:
                                        {
                                            int_ATS_TravelStatus = 1;
                                            intATSResult = 0;
                                            break;
                                        }
                                    case 1:
                                        {
                                            int_ATS_TravelStatus = 0;
                                            intATSResult = 1;
                                            break;
                                        }
                                }


                            }
                            if (dt_ATS_Date == dt_TEndDate && dt_TBeginDate != dt_TEndDate)
                            {
                                switch (int_TEndDateFlag)
                                {
                                    case 0:
                                        {
                                            int_ATS_TravelStatus = 1;
                                            intATSResult = 0;
                                            break;
                                        }
                                    case 1:
                                        {
                                            int_ATS_TravelStatus = 0;
                                            intATSResult = 1;
                                            break;
                                        }
                                }
                            }
                            if (dt_ATS_Date == dt_TEndDate && dt_TBeginDate == dt_TEndDate)
                            {
                                if (int_TBeginDateFlag == 0 && int_TEndDateFlag == 1)
                                {
                                    int_ATS_TravelStatus = 1;
                                    intATSResult = 0;
                                }
                                if (int_TBeginDateFlag == 1 && int_TEndDateFlag == 0)
                                {
                                    int_ATS_TravelStatus = 1;
                                    intATSResult = 0;
                                }
                                if (int_TBeginDateFlag == 1 && int_TEndDateFlag == 1)
                                {
                                    int_ATS_TravelStatus = 0;
                                    intATSResult = 1;
                                }
                            }
                        }

                        //获取节日记录
                        int int_ATS_Holiday = 0;
                        int int_ATS_HolidayStatus = 0;
                        string strinsql = "select * from dbo.Base_ATS_HolidaySetting where '" + txt_ATS_Date + "' between BeginDate and EndDate";
                        StringBuilder sbinsb_sql = new StringBuilder(strinsql);
                        DataTable dtindt = DataFactory.SqlDataBase().GetDataTableBySQL(sbinsb_sql);
                        if (dtindt.Rows.Count != 0 && dtindt.Rows[0].ItemArray[0].ToString().Length != 0)
                        {
                            DateTime dt_HBeginDate = DateTime.Parse(dtindt.Rows[0].ItemArray[2].ToString());
                            DateTime dt_HEndDate = DateTime.Parse(dtindt.Rows[0].ItemArray[3].ToString());
                            int int_HBeginDateFlag = int.Parse(dtindt.Rows[0].ItemArray[4].ToString());
                            int int_HEndDateFlag = int.Parse(dtindt.Rows[0].ItemArray[5].ToString());

                            int_ATS_Holiday = int.Parse(dtindt.Rows[0].ItemArray[0].ToString());

                            if (dt_ATS_Date == dt_HBeginDate && dt_HBeginDate != dt_HEndDate)
                            {
                                switch (int_HBeginDateFlag)
                                {
                                    case 0:
                                        {
                                            int_ATS_HolidayStatus = 1;
                                            intATSResult = 0;
                                            break;
                                        }
                                    case 1:
                                        {
                                            int_ATS_HolidayStatus = 0;
                                            intATSResult = 1;
                                            break;
                                        }
                                }


                            }
                            if (dt_ATS_Date == dt_HEndDate && dt_HBeginDate != dt_HEndDate)
                            {
                                switch (int_HEndDateFlag)
                                {
                                    case 0:
                                        {
                                            int_ATS_HolidayStatus = 1;
                                            intATSResult = 0;
                                            break;
                                        }
                                    case 1:
                                        {
                                            int_ATS_HolidayStatus = 0;
                                            intATSResult = 1;
                                            break;
                                        }
                                }
                            }
                            if (dt_ATS_Date == dt_HEndDate && dt_HBeginDate == dt_HEndDate)
                            {
                                if (int_HBeginDateFlag == 0 && int_HEndDateFlag == 1)
                                {
                                    int_ATS_HolidayStatus = 1;
                                    intATSResult = 0;
                                }
                                if (int_HBeginDateFlag == 1 && int_HEndDateFlag == 0)
                                {
                                    int_ATS_HolidayStatus = 1;
                                    intATSResult = 0;
                                }
                                if (int_HBeginDateFlag == 1 && int_HEndDateFlag == 1)
                                {
                                    int_ATS_HolidayStatus = 0;
                                    intATSResult = 1;
                                }
                            }
                        }

                        //更新节日,休假,公出记录
                        string insqlii = "update Base_ATSResult ";
                        insqlii = insqlii + "set ATS_Holiday='" + int_ATS_Holiday + "',ATS_HolidayStatus='" + int_ATS_HolidayStatus + "' ";
                        insqlii = insqlii + ", ATS_Leave='" + int_ATS_Leave + "',ATS_LeaveID='" + int_ATS_LeaveID + "',ATS_LeaveStatus='" + int_ATS_LeaveStatus + "' ";
                        insqlii = insqlii + ", ATS_Travel='" + int_ATS_Travel + "',ATS_TravelID='" + int_ATS_TravelID + "',ATS_TravelStatus='" + int_ATS_TravelStatus + "' ";
                        //insqlii = insqlii + ", PunchINTime = '" + txt_PunchInTime + "',PunchOutTime = '" + txt_PunchOutTime + "',LunchTime = '" + txt_LunchTime + "' ";
                        insqlii = insqlii + ", ATS_Result=" + intATSResult + " ";
                        insqlii = insqlii + " where EmpID='" + txt_EmpID + "' and EmpCode='" + txt_EmpCode + "' and ATS_Date='" + txt_ATS_Date + "' ";
                        StringBuilder insb_sqlii = new StringBuilder(insqlii);
                        int int_sqlresultii = DataFactory.SqlDataBase().ExecuteBySql(insb_sqlii);
                        if (int_sqlresultii < 0)
                        {
                            ShowMsgHelper.Alert_Wern("Exam Error!#ATSResult001");
                        }
                    }
                }
                sql = "update Base_ATSResult set ATS_Result=1 where ATS_DateStatus=0 or ATS_DateStatus=6";
                sb_sql = new StringBuilder(sql);
                int intsql = DataFactory.SqlDataBase().ExecuteBySql(sb_sql);
                if (intsql < 0)
                {
                    ShowMsgHelper.Alert_Wern("Exam Error!");
                }
            }

            //StringBuilder SqlWhere = new StringBuilder();
            //IList<SqlParam> IList_param = new List<SqlParam>();
            //DataTable dt = DataFactory.SqlDataBase().GetDataTable("Base_ATS_OriDataIn");
            sql = "select * from Base_ATS_OriDataIn where OriData>='" + txtBeginDate + "' and OriData<='" + txtEndDate + "' ";
            sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);

            if (dt.Rows.Count != 0 && dt.Rows[0].ItemArray[0].ToString().Length != 0)
            {
                count = dt.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    string txt_ATS_Date = dt.Rows[i].ItemArray[7].ToString();
                    txt_ATS_Date = txt_ATS_Date.Substring(0, txt_ATS_Date.IndexOf(' '));
                    int int_ATS_DateStatus = (int)DateTime.Parse(txt_ATS_Date).DayOfWeek;
                    string txt_EmpCode = dt.Rows[i].ItemArray[2].ToString();   //修改为base_userinfo的ats_code
                    //string txt_EmpID = GetIDfromCode(txt_EmpCode);
                    string txt_EmpID = GetIDfromATSCode(txt_EmpCode);
                    DateTime dt_ATS_Date = DateTime.Parse(txt_ATS_Date);

                    string strLunchBeginTime = "00:00:00";
                    string strLunchEndTime = "00:00:00";
                    string strBeginTime = "00:00:00";
                    string strEndTime = "00:00:00";
                    string strAMEndTime = "00:00:00";
                    string strPMBeginTime = "00:00:00";
                    int intAMWorkHour = 3;
                    int intPMWorkHour = 5;
                    int intNorWorkHour = 8;
                    int intExWorkMin = 30;

                    DateTime dtBeginTime;
                    DateTime dtAMEndTime;
                    DateTime dtPMBeginTime;
                    DateTime dtEndTime;
                    DateTime dtLunchBeginTime;
                    DateTime dtLunchEndTime;
                    DateTime dtPunchInTime;
                    DateTime dtPunchOutTime;

                    string insql = "select * from [Base_ATS_BaseSetting]";
                    StringBuilder insb_sql = new StringBuilder(insql);
                    DataTable dtLunch = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sql);
                    if (dtLunch.Rows.Count != 0)
                    {
                        strBeginTime = dtLunch.Rows[0].ItemArray[1].ToString();
                        strEndTime = dtLunch.Rows[0].ItemArray[2].ToString();
                        strLunchBeginTime = dtLunch.Rows[0].ItemArray[3].ToString();
                        strLunchEndTime = dtLunch.Rows[0].ItemArray[4].ToString();
                        intNorWorkHour = int.Parse(dtLunch.Rows[0].ItemArray[5].ToString());
                        intExWorkMin = int.Parse(dtLunch.Rows[0].ItemArray[6].ToString());
                        strAMEndTime = dtLunch.Rows[0].ItemArray[7].ToString();
                        strPMBeginTime = dtLunch.Rows[0].ItemArray[8].ToString();
                        intAMWorkHour = int.Parse(dtLunch.Rows[0].ItemArray[9].ToString());
                        intPMWorkHour = int.Parse(dtLunch.Rows[0].ItemArray[10].ToString());

                        dtBeginTime = DateTime.Parse(txt_ATS_Date + " " + strBeginTime).AddMinutes(intExWorkMin);
                        dtEndTime = DateTime.Parse(txt_ATS_Date + " " + strEndTime).AddMinutes((-1) * intExWorkMin);
                        dtLunchBeginTime = DateTime.Parse(txt_ATS_Date + " " + strLunchBeginTime);
                        dtLunchEndTime = DateTime.Parse(txt_ATS_Date + " " + strLunchEndTime);
                        dtAMEndTime = DateTime.Parse(txt_ATS_Date + " " + strAMEndTime);
                        dtPMBeginTime = DateTime.Parse(txt_ATS_Date + " " + strPMBeginTime);
                    }
                    else
                    {
                        ShowMsgHelper.Alert_Error("Parameter Error!");
                        break;
                    }

                    string txt_PunchInTime = "";
                    string txt_PunchOutTime = "";
                    string txt_LunchTime = "";

                    //获取首次打卡时间
                    insql = "select MIN(OriTime) as OriTime from dbo.Base_ATS_OriDataIn where OriData='" + txt_ATS_Date + "' and OriID='" + txt_EmpCode + "' ";
                    insb_sql = new StringBuilder(insql);
                    DataTable indt = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sql);
                    if (indt.Rows.Count != 0 && indt.Rows[0].ItemArray[0].ToString().Length != 0)
                    {
                        txt_PunchInTime = indt.Rows[0].ItemArray[0].ToString();
                    }
                    else
                    {
                        txt_PunchInTime = "00:00:00";
                    }


                    //获取最后一次打卡时间
                    insql = "select MAX(OriTime) as OriTime from dbo.Base_ATS_OriDataIn where OriData='" + txt_ATS_Date + "' and OriID='" + txt_EmpCode + "' ";
                    insb_sql = new StringBuilder(insql);
                    indt = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sql);
                    if (indt.Rows.Count != 0 && indt.Rows[0].ItemArray[0].ToString().Length != 0)
                    {
                        txt_PunchOutTime = indt.Rows[0].ItemArray[0].ToString();
                    }
                    else
                    {
                        txt_PunchOutTime = "00:00:00";
                    }

                    //根据刷卡记录判断考勤结果
                    dtPunchInTime = DateTime.Parse(txt_ATS_Date + " " + txt_PunchInTime);
                    dtPunchOutTime = DateTime.Parse(txt_ATS_Date + " " + txt_PunchOutTime);
                    int intATSResult = 1;
                    TimeSpan tsPunchInTime = new TimeSpan(dtPunchInTime.Ticks);
                    TimeSpan tsPunchOutTime = new TimeSpan(dtPunchOutTime.Ticks);
                    int intResult = tsPunchOutTime.Subtract(tsPunchInTime).Hours;

                    if (txt_PunchInTime == null || txt_PunchInTime == "" || txt_PunchInTime == "00:00:00" || txt_PunchOutTime == null || txt_PunchOutTime == "" || txt_PunchOutTime == "00:00:00")
                    {
                        intATSResult = 0; //Punch Error
                    }
                    if (dtPunchInTime > dtBeginTime || dtPunchOutTime < dtEndTime || intResult < intNorWorkHour)
                    {
                        intATSResult = 2; //迟到早退
                        if (DateDiff(dtPunchInTime, dtBeginTime) > 4 || DateDiff(dtPunchOutTime, dtEndTime) > 4)
                        { intATSResult = 3; }
                    }

                    //获取期间打卡时间
                    insql = "select OriTime from dbo.Base_ATS_OriDataIn where OriData='" + txt_ATS_Date + "' and OriID='" + txt_EmpCode + "' and OriTime != '" + txt_PunchInTime + "' and OriTime !='" + txt_PunchOutTime + "' ";
                    insql = insql + " and OriTime>='" + strLunchBeginTime + "' and OriTime<='" + strLunchEndTime + "' ";
                    insb_sql = new StringBuilder(insql);
                    indt = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sql);
                    if (indt.Rows.Count != 0 && indt.Rows[0].ItemArray[0].ToString().Length != 0)
                    {
                        int incount = indt.Rows.Count;
                        for (int ii = 0; ii < incount; ii++)
                        {
                            if (ii != 0)
                            {
                                txt_LunchTime = txt_LunchTime + ",";
                            }
                            txt_LunchTime = txt_LunchTime + indt.Rows[ii].ItemArray[0].ToString();
                        }
                    }
                    else
                    {
                        txt_LunchTime = "";
                    }

                    //获取节日记录
                    int int_ATS_Holiday = 0;
                    int int_ATS_HolidayStatus = 0;
                    insql = "select * from dbo.Base_ATS_HolidaySetting where '" + txt_ATS_Date + "' between BeginDate and EndDate";
                    insb_sql = new StringBuilder(insql);
                    indt = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sql);
                    if (indt.Rows.Count != 0 && indt.Rows[0].ItemArray[0].ToString().Length != 0)
                    {
                        DateTime dt_HBeginDate = DateTime.Parse(indt.Rows[0].ItemArray[2].ToString());
                        DateTime dt_HEndDate = DateTime.Parse(indt.Rows[0].ItemArray[3].ToString());
                        int int_HBeginDateFlag = int.Parse(indt.Rows[0].ItemArray[4].ToString());
                        int int_HEndDateFlag = int.Parse(indt.Rows[0].ItemArray[5].ToString());

                        int_ATS_Holiday = int.Parse(indt.Rows[0].ItemArray[0].ToString());

                        if (dt_ATS_Date == dt_HBeginDate && dt_HBeginDate != dt_HEndDate)
                        {
                            switch (int_HBeginDateFlag)
                            {
                                case 0:
                                    {
                                        int_ATS_HolidayStatus = 1;
                                        if (dtPunchInTime > dtPMBeginTime || dtPunchOutTime < dtEndTime || intResult < intPMWorkHour)
                                        {                                           
                                            intATSResult = 2; //迟到早退
                                            if (DateDiff(dtPunchInTime, dtPMBeginTime) > 4 || DateDiff(dtPunchOutTime, dtEndTime) > 4)
                                            { intATSResult = 3; }

                                        }
                                        else
                                        {
                                            intATSResult = 1;
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        int_ATS_HolidayStatus = 0;
                                        intATSResult = 1;
                                        break;
                                    }
                            }


                        }
                        if (dt_ATS_Date == dt_HEndDate && dt_HBeginDate != dt_HEndDate)
                        {
                            switch (int_HEndDateFlag)
                            {
                                case 0:
                                    {
                                        int_ATS_HolidayStatus = 1;
                                        if (dtPunchInTime > dtBeginTime || dtPunchOutTime < dtAMEndTime || intResult < intAMWorkHour)
                                        {
                                            intATSResult = 2; //迟到早退
                                            if (DateDiff(dtPunchInTime, dtBeginTime) > 4 || DateDiff(dtPunchOutTime, dtAMEndTime) > 4)
                                            { intATSResult = 3; }
                                        }
                                        else
                                        {
                                            intATSResult = 1;
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        int_ATS_HolidayStatus = 0;
                                        intATSResult = 1;
                                        break;
                                    }
                            }
                        }
                        if (dt_ATS_Date == dt_HEndDate && dt_HBeginDate == dt_HEndDate)
                        {
                            if (int_HBeginDateFlag == 0 && int_HEndDateFlag == 1)
                            {
                                int_ATS_HolidayStatus = 1;
                                if (dtPunchInTime > dtPMBeginTime || dtPunchOutTime < dtEndTime || intResult < intPMWorkHour)
                                {
                                    intATSResult = 2; //迟到早退
                                    if (DateDiff(dtPunchInTime, dtPMBeginTime) > 4 || DateDiff(dtPunchOutTime, dtEndTime) > 4)
                                    { intATSResult = 3; }
                                }
                                else
                                {
                                    intATSResult = 1;
                                }
                            }
                            if (int_HBeginDateFlag == 1 && int_HEndDateFlag == 0)
                            {
                                int_ATS_HolidayStatus = 1;
                                if (dtPunchInTime > dtBeginTime || dtPunchOutTime < dtAMEndTime || intResult < intAMWorkHour)
                                {
                                    intATSResult = 2; //迟到早退
                                    if (DateDiff(dtPunchInTime, dtBeginTime) > 4 || DateDiff(dtPunchOutTime, dtAMEndTime) > 4)
                                    { intATSResult = 3; }
                                }
                                else
                                {
                                    intATSResult = 1;
                                }
                            }
                            if (int_HBeginDateFlag == 1 && int_HEndDateFlag == 1)
                            {
                                int_ATS_HolidayStatus = 0;
                                if (dtPunchInTime > dtBeginTime || dtPunchOutTime < dtEndTime || intResult < intNorWorkHour)
                                {
                                    intATSResult = 2; //迟到早退
                                    if (DateDiff(dtPunchInTime, dtBeginTime) > 4 || DateDiff(dtPunchOutTime, dtEndTime) > 4)
                                    { intATSResult = 3; }
                                }
                                else
                                {
                                    intATSResult = 1;
                                }
                            }
                        }
                    }

                    //更新所有人节日记录
                    if (int_ATS_Holiday == 0)
                    {
                        insql = "update Base_ATSResult ";
                        insql = insql + "set ATS_Holiday='" + int_ATS_Holiday + "',ATS_HolidayStatus='" + int_ATS_HolidayStatus + "' ";
                        //insql = insql + ", ATS_Result=" + intATSResult + " ";
                        insql = insql + " where ATS_Date='" + txt_ATS_Date + "' ";
                        insb_sql = new StringBuilder(insql);
                        int_sqlresult = DataFactory.SqlDataBase().ExecuteBySql(insb_sql);
                        if (int_sqlresult < 0)
                        {
                            ShowMsgHelper.Alert_Wern("Exam Error!#ATSResult002");
                        }
                    }
                    else
                    {
                        insql = "update Base_ATSResult ";
                        insql = insql + "set ATS_Holiday='" + int_ATS_Holiday + "',ATS_HolidayStatus='" + int_ATS_HolidayStatus + "' ";
                        insql = insql + ", ATS_Result=" + intATSResult + " ";
                        insql = insql + " where ATS_Date='" + txt_ATS_Date + "' ";
                        insb_sql = new StringBuilder(insql);
                        int_sqlresult = DataFactory.SqlDataBase().ExecuteBySql(insb_sql);
                        if (int_sqlresult < 0)
                        {
                            ShowMsgHelper.Alert_Wern("Exam Error!#ATSResult003");
                        }
                    }


                    //获取休假记录
                    int int_ATS_Leave = 0;
                    int int_ATS_LeaveID = 0;
                    int int_ATS_LeaveStatus = 0;
                    //txt_EmpID = "80cc9090-885f-4bec-a96e-93df654a8d53";
                    insql = "select * from dbo.Base_PerLeaveApply where ApprovalFlag=2 and '" + txt_ATS_Date + "' between BeginDate and EndDate and EmpID='" + txt_EmpID + "' ";
                    insb_sql = new StringBuilder(insql);
                    indt = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sql);
                    if (indt.Rows.Count != 0 && indt.Rows[0].ItemArray[0].ToString().Length != 0)
                    {
                        DateTime dt_LBeginDate = DateTime.Parse(indt.Rows[0].ItemArray[3].ToString());
                        DateTime dt_LEndDate = DateTime.Parse(indt.Rows[0].ItemArray[5].ToString());
                        int int_LBeginDateFlag = int.Parse(indt.Rows[0].ItemArray[4].ToString());
                        int int_LEndDateFlag = int.Parse(indt.Rows[0].ItemArray[6].ToString());
                        dt_ATS_Date = DateTime.Parse(txt_ATS_Date);

                        int_ATS_Leave = int.Parse(indt.Rows[0].ItemArray[2].ToString());
                        int_ATS_LeaveID = int.Parse(indt.Rows[0].ItemArray[0].ToString());

                        if (dt_ATS_Date == dt_LBeginDate && dt_LBeginDate != dt_LEndDate)
                        {
                            switch (int_LBeginDateFlag)
                            {
                                case 0:
                                    {
                                        int_ATS_LeaveStatus = 1;
                                        if (dtPunchInTime > dtPMBeginTime || dtPunchOutTime < dtEndTime || intResult < intPMWorkHour)
                                        {
                                            intATSResult = 2; //迟到早退
                                            if (DateDiff(dtPunchInTime, dtPMBeginTime) > 4 || DateDiff(dtPunchOutTime, dtEndTime) > 4)
                                            { intATSResult = 3; }
                                        }
                                        else
                                        {
                                            intATSResult = 1;
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        int_ATS_LeaveStatus = 0;
                                        intATSResult = 1;
                                        break;
                                    }
                            }


                        }
                        if (dt_ATS_Date == dt_LEndDate && dt_LBeginDate != dt_LEndDate)
                        {
                            switch (int_LEndDateFlag)
                            {
                                case 0:
                                    {
                                        int_ATS_LeaveStatus = 1;
                                        if (dtPunchInTime > dtBeginTime || dtPunchOutTime < dtAMEndTime || intResult < intAMWorkHour)
                                        {
                                            intATSResult = 2; //迟到早退
                                            if (DateDiff(dtPunchInTime, dtBeginTime) > 4 || DateDiff(dtPunchOutTime, dtAMEndTime) > 4)
                                            { intATSResult = 3; }
                                        }
                                        else
                                        {
                                            intATSResult = 1;
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        int_ATS_LeaveStatus = 0;
                                        intATSResult = 1;
                                        break;
                                    }
                            }
                        }
                        if (dt_ATS_Date == dt_LEndDate && dt_LBeginDate == dt_LEndDate)
                        {
                            if(int_LBeginDateFlag==0 && int_LEndDateFlag==1)
                            {
                                int_ATS_LeaveStatus = 1;
                                if (dtPunchInTime > dtPMBeginTime || dtPunchOutTime < dtEndTime || intResult < intPMWorkHour)
                                {
                                    intATSResult = 2; //迟到早退
                                    if (DateDiff(dtPunchInTime, dtPMBeginTime) > 4 || DateDiff(dtPunchOutTime, dtEndTime) > 4)
                                    { intATSResult = 3; }
                                }
                                else
                                {
                                    intATSResult = 1;
                                }
                            }
                            if(int_LBeginDateFlag==1 && int_LEndDateFlag==0)
                            {
                                int_ATS_LeaveStatus = 1;
                                if (dtPunchInTime > dtBeginTime || dtPunchOutTime < dtAMEndTime || intResult < intAMWorkHour)
                                {
                                    intATSResult = 2; //迟到早退
                                    if (DateDiff(dtPunchInTime, dtBeginTime) > 4 || DateDiff(dtPunchOutTime, dtAMEndTime) > 4)
                                    { intATSResult = 3; }
                                }
                                else
                                {
                                    intATSResult = 1;
                                }
                            }
                            if(int_LBeginDateFlag==1 && int_LEndDateFlag==1)
                            {
                                int_ATS_LeaveStatus = 0;
                                if (dtPunchInTime > dtBeginTime || dtPunchOutTime < dtEndTime || intResult < intNorWorkHour)
                                {
                                    intATSResult = 2; //迟到早退
                                    if (DateDiff(dtPunchInTime, dtBeginTime) > 4 || DateDiff(dtPunchOutTime, dtEndTime) > 4)
                                    { intATSResult = 3; }
                                }
                                else
                                {
                                    intATSResult = 1;
                                }
                            }
                        }
                    }

                    //获取公出记录
                    int int_ATS_Travel = 0;
                    int int_ATS_TravelID = 0;
                    int int_ATS_TravelStatus = 0;
                    insql = "select * from dbo.Base_PerTravelApply where ApprovalFlag=2 and '" + txt_ATS_Date + "' between BeginDate and EndDate and EmpID='" + txt_EmpID + "' ";
                    insb_sql = new StringBuilder(insql);
                    indt = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sql);
                    if (indt.Rows.Count != 0 && indt.Rows[0].ItemArray[0].ToString().Length != 0)
                    {
                        DateTime dt_TBeginDate = DateTime.Parse(indt.Rows[0].ItemArray[2].ToString());
                        DateTime dt_TEndDate = DateTime.Parse(indt.Rows[0].ItemArray[4].ToString());
                        int int_TBeginDateFlag = int.Parse(indt.Rows[0].ItemArray[3].ToString());
                        int int_TEndDateFlag = int.Parse(indt.Rows[0].ItemArray[5].ToString());
                        dt_ATS_Date = DateTime.Parse(txt_ATS_Date);

                        int_ATS_Travel = int.Parse(indt.Rows[0].ItemArray[0].ToString());
                        int_ATS_TravelID = int.Parse(indt.Rows[0].ItemArray[0].ToString());

                        if (dt_ATS_Date == dt_TBeginDate && dt_TBeginDate != dt_TEndDate)
                        {
                            switch (int_TBeginDateFlag)
                            {
                                case 0:
                                    {
                                        int_ATS_TravelStatus = 1;
                                        if (dtPunchInTime > dtPMBeginTime || dtPunchOutTime < dtEndTime || intResult < intPMWorkHour)
                                        {
                                            intATSResult = 2; //迟到早退
                                            if (DateDiff(dtPunchInTime, dtPMBeginTime) > 4 || DateDiff(dtPunchOutTime, dtEndTime) > 4)
                                            { intATSResult = 3; }
                                        }
                                        else
                                        {
                                            intATSResult = 1;
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        int_ATS_TravelStatus = 0;
                                        intATSResult = 1;
                                        break;
                                    }
                            }


                        }
                        if (dt_ATS_Date == dt_TEndDate && dt_TBeginDate != dt_TEndDate)
                        {
                            switch (int_TEndDateFlag)
                            {
                                case 0:
                                    {
                                        int_ATS_TravelStatus = 1;
                                        if (dtPunchInTime > dtBeginTime || dtPunchOutTime < dtAMEndTime || intResult < intAMWorkHour)
                                        {
                                            intATSResult = 2; //迟到早退
                                            if (DateDiff(dtPunchInTime, dtBeginTime) > 4 || DateDiff(dtPunchOutTime, dtAMEndTime) > 4)
                                            { intATSResult = 3; }
                                        }
                                        else
                                        {
                                            intATSResult = 1;
                                        }
                                        break;
                                    }
                                case 1:
                                    {
                                        int_ATS_TravelStatus = 0;
                                        intATSResult = 1;
                                        break;
                                    }
                            }
                        }
                        if (dt_ATS_Date == dt_TEndDate && dt_TBeginDate == dt_TEndDate)
                        {
                            if (int_TBeginDateFlag == 0 && int_TEndDateFlag == 1)
                            {
                                int_ATS_TravelStatus = 1;
                                if (dtPunchInTime > dtPMBeginTime || dtPunchOutTime < dtEndTime || intResult < intPMWorkHour)
                                {
                                    intATSResult = 2; //迟到早退
                                    if (DateDiff(dtPunchInTime, dtPMBeginTime) > 4 || DateDiff(dtPunchOutTime, dtEndTime) > 4)
                                    { intATSResult = 3; }
                                }
                                else
                                {
                                    intATSResult = 1;
                                }
                            }
                            if (int_TBeginDateFlag == 1 && int_TEndDateFlag == 0)
                            {
                                int_ATS_TravelStatus = 1;
                                if (dtPunchInTime > dtBeginTime || dtPunchOutTime < dtAMEndTime || intResult < intAMWorkHour)
                                {
                                    intATSResult = 2; //迟到早退
                                    if (DateDiff(dtPunchInTime, dtBeginTime) > 4 || DateDiff(dtPunchOutTime, dtAMEndTime) > 4)
                                    { intATSResult = 3; }
                                }
                                else
                                {
                                    intATSResult = 1;
                                }
                            }
                            if (int_TBeginDateFlag == 1 && int_TEndDateFlag == 1)
                            {
                                int_ATS_TravelStatus = 0;
                                if (dtPunchInTime > dtBeginTime || dtPunchOutTime < dtEndTime || intResult < intNorWorkHour)
                                {
                                    intATSResult = 2; //迟到早退
                                    if (DateDiff(dtPunchInTime, dtBeginTime) > 4 || DateDiff(dtPunchOutTime, dtEndTime) > 4)
                                    { intATSResult = 3; }
                                }
                                else
                                {
                                    intATSResult = 1;
                                }
                            }
                        }
                    }

                    //更新周末打卡记录
                    int intWeek = (int)DateTime.Parse(txt_ATS_Date).DayOfWeek;
                    if(intWeek==0 || intWeek==6)
                    {
                        intATSResult = 1;
                    }

          
                    //更新节日,休假,公出记录及三组打卡记录
                    if (int_ATS_Holiday==0)
                    {
                        insql = "update Base_ATSResult ";
                        insql = insql + "set ATS_Holiday='" + int_ATS_Holiday + "',ATS_HolidayStatus='" + int_ATS_HolidayStatus + "' ";
                        insql = insql + ", ATS_Leave='" + int_ATS_Leave + "',ATS_LeaveID='" + int_ATS_LeaveID + "',ATS_LeaveStatus='" + int_ATS_LeaveStatus + "' ";
                        insql = insql + ", ATS_Travel='" + int_ATS_Travel + "',ATS_TravelID='" + int_ATS_TravelID + "',ATS_TravelStatus='" + int_ATS_TravelStatus + "' ";
                        insql = insql + ", PunchINTime = '" + txt_PunchInTime + "',PunchOutTime = '" + txt_PunchOutTime + "',LunchTime = '" + txt_LunchTime + "' ";
                        insql = insql + ", ATS_Result=" + intATSResult + " ";
                        insql = insql + " where EmpID='" + txt_EmpID + "' and ATS_Date='" + txt_ATS_Date + "' and ATS_Holiday=0";
                    }
                    else
                    {
                        insql = "update Base_ATSResult ";
                        //insql = insql + "set ATS_Holiday='" + int_ATS_Holiday + "',ATS_HolidayStatus='" + int_ATS_HolidayStatus + "' ";
                        insql = insql + "set ATS_Leave='" + int_ATS_Leave + "',ATS_LeaveID='" + int_ATS_LeaveID + "',ATS_LeaveStatus='" + int_ATS_LeaveStatus + "' ";
                        insql = insql + ", ATS_Travel='" + int_ATS_Travel + "',ATS_TravelID='" + int_ATS_TravelID + "',ATS_TravelStatus='" + int_ATS_TravelStatus + "' ";
                        insql = insql + ", PunchINTime = '" + txt_PunchInTime + "',PunchOutTime = '" + txt_PunchOutTime + "',LunchTime = '" + txt_LunchTime + "' ";
                        insql = insql + ", ATS_Result=" + intATSResult + " ";
                        insql = insql + " where EmpID='" + txt_EmpID + "' and ATS_Date='" + txt_ATS_Date + "' and ATS_Holiday!=0";
                    }
                    insb_sql = new StringBuilder(insql);
                    int_sqlresult = DataFactory.SqlDataBase().ExecuteBySql(insb_sql);
                    if (int_sqlresult < 0)
                    {
                        ShowMsgHelper.Alert_Wern("Exam Error!#ATSResult004");
                    }
                    //}
                    //else
                    //{
                    //    ShowMsgHelper.Alert_Wern("Exam Error!");
                    //}
                    //    }
                    //    else
                    //    {

                    //    }
                    //}
                }
                ShowMsgHelper.AlertMsg("Succecc!");
            }
            else
            {
                ShowMsgHelper.Alert_Wern("Exam Error!#ATSResult005");
            }

            //恢复按钮
            btn_ATSCheck.Enabled = true;
            btn_EditTravel.Enabled = true;
            btn_Search.Enabled = true;
            btn_SetNor.Enabled = true;
        }

        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lab_EmpID = e.Item.FindControl("EmpID") as Label;
                Label lab_Flag = e.Item.FindControl("Flag") as Label;
                Label lab_ATS_DateStatus = e.Item.FindControl("ATS_DateStatus") as Label;
                Label lab_ATS_Holiday = e.Item.FindControl("ATS_Holiday") as Label;
                Label lab_ATS_HolidayStatus = e.Item.FindControl("ATS_HolidayStatus") as Label;
                Label lab_ATS_Leave = e.Item.FindControl("ATS_Leave") as Label;
                Label lab_ATS_Travel = e.Item.FindControl("ATS_Travel") as Label;
                Label lab_ATS_Result = e.Item.FindControl("ATS_Result") as Label;

                if (lab_ATS_Result != null)
                {
                    string text = lab_ATS_Result.Text;
                    text = text.Replace("0", "ATS Error");
                    text = text.Replace("1", "Normal");
                    text = text.Replace("2", "Late/Leave early(Short)");
                    text = text.Replace("3", "Late/Leave early(Long)");
                    lab_ATS_Result.Text = text;
                }

                if (lab_EmpID != null)
                {
                    lab_EmpID.Text = GetNameFromID(lab_EmpID.Text);
                }

                if (lab_Flag != null)
                {
                    string text = lab_Flag.Text;
                    text = text.Replace("0", "Waitting");
                    text = text.Replace("1", "Be Examined");
                    text = text.Replace("2", "Be Edited");
                    lab_Flag.Text = text;
                }

                if (lab_ATS_DateStatus != null)
                {
                    string text = lab_ATS_DateStatus.Text;
                    text = text.Replace("0", "Sun");
                    text = text.Replace("1", "Mon");
                    text = text.Replace("2", "Tue");
                    text = text.Replace("3", "Wed");
                    text = text.Replace("4", "Thu");
                    text = text.Replace("5", "Fri");
                    text = text.Replace("6", "Sat");
                    lab_ATS_DateStatus.Text = text;
                }

                if (lab_ATS_Holiday != null && int.Parse(lab_ATS_Holiday.Text) > 0)
                {
                    lab_ATS_Holiday.Text = GetHNFromID(lab_ATS_Holiday.Text);
                    if (lab_ATS_HolidayStatus != null)
                    {
                        string text = lab_ATS_HolidayStatus.Text;
                        text = text.Replace("0", "All Day");
                        text = text.Replace("1", "Morning");
                        text = text.Replace("2", "Afternoon");
                        lab_ATS_HolidayStatus.Text = text;
                    }
                    else
                    {
                        lab_ATS_HolidayStatus.Text = "-";
                    }
                }
                else
                {
                    lab_ATS_HolidayStatus.Text = "-";
                    lab_ATS_Holiday.Text = "-";
                }

                if (lab_ATS_Leave != null)
                {
                    lab_ATS_Leave.Text = GetLNFromID(lab_ATS_Leave.Text);
                }

                if (lab_ATS_Travel != null && int.Parse(lab_ATS_Travel.Text) > 0)
                {
                    lab_ATS_Travel.Text = "Biz Travel";
                }
                else
                {
                    lab_ATS_Travel.Text = "-";
                }
            }

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

        private string GetIDfromATSCode(string ATSCode)
        {
            string txt_Result = "";

            string sql = "select User_ID from Base_UserInfo where ATS_Code='" + ATSCode + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count != 0 && dt.Rows[0].ItemArray[0].ToString() != "")
            {
                txt_Result = dt.Rows[0].ItemArray[0].ToString();
            }

            return txt_Result;
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            DataBindGrid();
        }

        public static int DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            int dateDiff = 0;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                string hours = ts.Hours.ToString(), minutes = ts.Minutes.ToString(), seconds = ts.Seconds.ToString();
                //if (ts.Hours < 10)
                //{
                //    hours = "0" + ts.Hours.ToString();
                //}
                //if (ts.Minutes < 10)
                //{
                //    minutes = "0" + ts.Minutes.ToString();
                //}
                //if (ts.Seconds < 10)
                //{
                //    seconds = "0" + ts.Seconds.ToString();
                //}
                //dateDiff = hours + ":" + minutes + ":" + seconds;     
                dateDiff = ts.Hours;          
            }
            catch
            {

            }
            return dateDiff;
        }

        protected void btn_SetNor_Click(object sender, EventArgs e)
        {
            string strSelect = Page.Request.Form["tbtrtdin"];
            string strSQL = "update Base_ATSResult set ATS_Result=1,Flag=2 where id in (" + strSelect + ") ";
            StringBuilder sbSQL = new StringBuilder(strSQL);
            int intResult = DataFactory.SqlDataBase().ExecuteBySql(sbSQL);
            DataBindGrid();
        }
    }
}