using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Data;
using System.Text;
using RM.Busines;

namespace RM.Web.RMBase.SysATS
{
    /// <summary>
    /// hanATS_WorkRecord 的摘要说明
    /// </summary>
    public class hanATS_WorkRecord : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string startDate = context.Request.QueryString["start"];
            string endDate = context.Request.QueryString["end"];
            string EmpID = context.Request.QueryString["EmpID"];
            string sql;

            //测试数据
            List<classTask> tasks = new List<classTask>() {
            //new classTask(){ ID=1,Name="任务1",Content="修改某处Bug",StartDate=new DateTime(2015,12,16,08,32,33),EndDate=new DateTime(2015,12,16,11,27,33)},
            //new classTask(){ ID=2,Name="任务2",Content="与刘总开会讨论需求分析",StartDate=new DateTime(2015,12,09,18,32,33),EndDate=new DateTime(2015,12,09,19,27,33)},
            //new classTask(){ ID=3,Name="任务3",Content="代码上传与整理",StartDate=new DateTime(2015,12,17,13,32,33),EndDate=new DateTime(2015,12,17,17,27,33)},
            //new classTask(){ ID=4,Name="任务4",Content="上线测试",StartDate=new DateTime(2015,12,30,15,32,33),EndDate=new DateTime(2015,12,15,17,27,33)},
            //new classTask(){ ID=5,Name="任务5",Content="代码上传与整理",StartDate=new DateTime(2015,12,07,13,32,33),EndDate=new DateTime(2015,12,07,17,27,33)}
            };

            //tasks.Add(new classTask() { ID = 1, Name = "任务1", Content = "修改某处Bug", StartDate = new DateTime(2015, 12, 16, 13, 00, 00), EndDate = new DateTime(2015, 12, 16, 18, 00, 00) });

            if(EmpID==null || EmpID=="")
            {
                sql = "select * from uvw_PerAllRecord where BeginDate>='" + startDate + "' and EndDate<='" + endDate + "' ";               
            }
            else
            {
                sql = "select * from uvw_PerAllRecord where BeginDate>='" + startDate + "' and EndDate<='" + endDate + "' and EmpID='" + GetIDFromName(EmpID) + "' ";
            }
            StringBuilder sb_sql = new StringBuilder(sql);
            DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if(dt!=null && dt.Rows.Count>0)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    string taskName = "";
                    string taskContent = "";
                    string txtBeginDate = "";
                    string txtEndDate = "";
                    string txtBeginTime = "";
                    string txtAMEndTime = "";
                    string txtPMBeginTime = "";
                    string txtEndTime = "";
                    string txtBeginDT = "";
                    string txtEndDT = "";
                    DateTime dtBeginDT;
                    DateTime dtEndDT;
                    string txtEmpName = GetNameFromID(dt.Rows[i].ItemArray[0].ToString());

                    if (int.Parse(dt.Rows[i].ItemArray[1].ToString())!=99)
                    {
                        string insql = "select LeaveName from Base_ATS_LeaveSetting where id='" + dt.Rows[i].ItemArray[1].ToString() + "' ";
                        StringBuilder insb_sql = new StringBuilder(insql);
                        DataTable indt = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sql);
                        if(indt!=null && indt.Rows.Count>0)
                        {
                            taskName = indt.Rows[0].ItemArray[0].ToString();
                        }
                    }
                    else
                    {
                        taskName = "公出";
                    }
                    taskContent = txtEmpName + "--" +dt.Rows[i].ItemArray[6].ToString();
                    txtBeginDate = dt.Rows[i].ItemArray[2].ToString();
                    txtBeginDate = txtBeginDate.Substring(0, txtBeginDate.IndexOf(' '));
                    txtEndDate = dt.Rows[i].ItemArray[4].ToString();
                    txtEndDate = txtEndDate.Substring(0, txtEndDate.IndexOf(' '));
                    int intBeginFlag = int.Parse(dt.Rows[i].ItemArray[3].ToString());
                    int intEndFlag= int.Parse(dt.Rows[i].ItemArray[5].ToString());

                    string insql_base = "select BeginTime,AMEndTime,PMBeginTime,EndTime from Base_ATS_BaseSetting";
                    StringBuilder insb_sql_base = new StringBuilder(insql_base);
                    DataTable indt_base = DataFactory.SqlDataBase().GetDataTableBySQL(insb_sql_base);
                    if (indt_base != null && indt_base.Rows.Count > 0)
                    {
                        txtBeginTime = indt_base.Rows[0].ItemArray[0].ToString();
                        txtAMEndTime = indt_base.Rows[0].ItemArray[1].ToString();
                        txtPMBeginTime = indt_base.Rows[0].ItemArray[2].ToString();
                        txtEndTime = indt_base.Rows[0].ItemArray[3].ToString();
                    }

                    if(intBeginFlag==1)
                    {
                        txtBeginDT = txtBeginDate + " " + txtBeginTime;
                    }
                    else
                    {
                        txtBeginDT = txtBeginDate + " " + txtPMBeginTime;
                    }
                    if(intEndFlag==1)
                    {
                        txtEndDT = txtEndDate + " " + txtEndTime;
                    }
                    else
                    {
                        txtEndDT = txtEndDate + " " + txtAMEndTime;
                    }
                    dtBeginDT = DateTime.Parse(txtBeginDT);
                    dtEndDT = DateTime.Parse(txtEndDT);

                    tasks.Add(new classTask() {ID=i+1,Name= taskName,Content= taskContent,StartDate=dtBeginDT,EndDate=dtEndDT});
                }
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<Dictionary<string, object>> gas = new List<Dictionary<string, object>>();
            foreach (var entity in tasks)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                drow.Add("id", entity.ID);
                drow.Add("title", string.Format("状态：{0}", entity.Content + "::" + entity.Name));
                drow.Add("start", ReturnDate(entity.StartDate));
                drow.Add("end", ReturnDate(entity.EndDate));
                //鼠标悬浮上展现的是这个属性信息，可以自己设置
                drow.Add("fullname", string.Format("状态：{0}", entity.Content + "::" + entity.Name + "@" + entity.StartDate + "TO" + entity.EndDate));
                drow.Add("allDay", false);
                drow.Add("task", entity.Name);
                //设置颜色
                //drow.Add("backgroundColor", "red");
                //switch (entity.Name)
                //{
                //    case "公出":
                //        {
                //            //drow.Add("backgroundColor", "#378006");
                //            break;
                //        }
                //}
                gas.Add(drow);
            }
            context.Response.Write(jss.Serialize(gas));
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 时间输出格式
        /// <summary>
        /// 时间按照此格式传输
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string ReturnDate(DateTime? date)
        {
            string str = string.Empty;
            string time = Convert.ToString(date);
            string[] split = time.Split(' ');
            string viewDate = split[0].Split('/')[0] + "-" + AddZero(split[0].Split('/')[1]) + "-" + AddZero(split[0].Split('/')[2]);
            string viewTime = AddZero(split[1].Split(':')[0]) + ":" + AddZero(split[1].Split(':')[1]) + ":" + AddZero(split[1].Split(':')[2]);
            str = viewDate + "T" + viewTime;
            return str;
        }
        /// <summary>
        /// 判断数字前面是否加0
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string AddZero(string str)
        {
            if (str.Length == 1)
                return "0" + str;
            else
                return str;
        }
        #endregion
    }
}