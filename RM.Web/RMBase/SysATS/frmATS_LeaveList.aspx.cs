using System;
using System.Linq;
using RM.Busines;
using RM.Common.DotNetUI;
using System.Collections;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysATS
{
    public partial class frmATS_LeaveList : PageBase
    {
        string _key;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];
            DateTime tnow = DateTime.Now;//現在時間 
            ArrayList AlYear = new ArrayList();
            int i;
            for (i = 2010; i <= 2020; i++)
                AlYear.Add(i);
            ArrayList AlMonth = new ArrayList();
            for (i = 1; i <= 12; i++)
                AlMonth.Add(i);
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_key))
                {
                    if(_key=="7")
                    {
                        ArrayList alldays = new ArrayList();
                        for (int ii=1; ii <= 31; ii++)
                        {
                            alldays.Add(ii);
                        }
                        //控制结余日期
                        jyYYYY.DataSource = AlYear;
                        jyYYYY.DataBind();//綁定年 
                                          //選擇當前年 
                        jyYYYY.SelectedValue = tnow.Year.ToString();
                        jyYYYY.Enabled = false;
                        jyYYYY.Visible = false;
                        jyMM.DataSource = AlMonth;
                        jyMM.DataBind();//綁定月 
                                        //選擇當前月 
                        jyDD.DataSource = alldays;
                        jyDD.DataBind();
                        //jyMM.SelectedValue = tnow.Month.ToString();
                        //int year, month;
                        //year = Int32.Parse(jyYYYY.SelectedValue);
                        //month = Int32.Parse(jyMM.SelectedValue);
                        //BindDays(0,year, month);//綁定天 
                        //                      //選擇當前日期 
                        jyDD.SelectedValue = tnow.Day.ToString();
                        //控制置零日期
                        zeroYYYY.DataSource = AlYear;
                        zeroYYYY.DataBind();//綁定年 
                                            //選擇當前年 
                        zeroYYYY.SelectedValue = tnow.Year.ToString();
                        zeroYYYY.Enabled = false;
                        zeroYYYY.Visible = false;
                        zeroMM.DataSource = AlMonth;
                        zeroMM.DataBind();//綁定月 
                                          //選擇當前月 
                        zeroDD.DataSource = alldays;
                        zeroDD.DataBind();
                        //zeroMM.SelectedValue = tnow.Month.ToString();
                        //int zyear, zmonth;
                        //zyear = Int32.Parse(zeroYYYY.SelectedValue);
                        //zmonth = Int32.Parse(zeroMM.SelectedValue);
                        //BindDays(1,zyear, zmonth);//綁定天 
                        //                        //選擇當前日期 
                        zeroDD.SelectedValue = tnow.Day.ToString();

                        NJfrm.Visible = true;
                    }
                    else
                    {
                        NJfrm.Visible = false;
                    }
                    if(_key=="8")
                    {
                        DH.Visible = true;
                    }
                    else
                    {
                        DH.Visible = false;
                    }
                    InitData();
                }
            }
        }

        //判斷閏年 
        private bool CheckLeap(int year)
        {
            if ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0))
                return true;
            else return false;
        }
        //綁定每月的天數 
        private void BindDays(int Con,int year, int month)
        {
            int i;
            ArrayList AlDay = new ArrayList();

            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    for (i = 1; i <= 31; i++)
                        AlDay.Add(i);
                    break;
                case 2:
                    if (CheckLeap(year))
                    {
                        for (i = 1; i <= 29; i++)
                            AlDay.Add(i);
                    }
                    else
                    {
                        for (i = 1; i <= 28; i++)
                            AlDay.Add(i);
                    }
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    for (i = 1; i <= 30; i++)
                        AlDay.Add(i);
                    break;
            }
            switch (Con)
            {
                case 0:
                    {
                        jyDD.DataSource = AlDay;
                        jyDD.DataBind();
                        break;
                    }
                case 1:
                    {
                        zeroDD.DataSource = AlDay;
                        zeroDD.DataBind();
                        break;
                    }
            }
            
        }

        ////選擇年 
        //private void jyYYYY_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int year, month;
        //    year = Int32.Parse(jyYYYY.SelectedValue);
        //    month = Int32.Parse(jyMM.SelectedValue);
        //    BindDays(0,year, month);
        //}
        //private void zeroYYYY_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int year, month;
        //    year = Int32.Parse(zeroYYYY.SelectedValue);
        //    month = Int32.Parse(zeroMM.SelectedValue);
        //    BindDays(1, year, month);
        //}
        ////選擇月 
        //private void jyMM_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int year, month;
        //    year = Int32.Parse(jyYYYY.SelectedValue);
        //    month = Int32.Parse(jyMM.SelectedValue);
        //    BindDays(0,year, month);
        //}
        //private void zeroMM_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int year, month;
        //    year = Int32.Parse(zeroYYYY.SelectedValue);
        //    month = Int32.Parse(zeroMM.SelectedValue);
        //    BindDays(1, year, month);
        //}


        private void InitData()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_ATS_LeaveSetting", "id", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
                //BeginDate.Text = Convert.ToDateTime(BeginDate.Text).ToString("yyyy-MM-dd");
                //EndDate.Text = Convert.ToDateTime(EndDate.Text).ToString("yyyy-MM-dd");
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht = ControlBindHelper.GetWebControls(this.Page);
            ht["jyMM"] = jyMM.Text;
            ht["jyDD"] = jyDD.Text;
            ht["zeroMM"] = zeroMM.Text;
            ht["zeroDD"] = zeroDD.Text;
            int IsOk = DataFactory.SqlDataBase().UpdateByHashtable("Base_ATS_LeaveSetting", "id", _key, ht);
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