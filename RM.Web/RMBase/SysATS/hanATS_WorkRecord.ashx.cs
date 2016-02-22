using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

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


            //测试数据
            List<classTask> tasks = new List<classTask>() {
            new classTask(){ ID=1,Name="任务1",Content="修改某处Bug",StartDate=new DateTime(2015,12,16,08,32,33),EndDate=new DateTime(2015,12,16,11,27,33)},
            new classTask(){ ID=2,Name="任务2",Content="与刘总开会讨论需求分析",StartDate=new DateTime(2015,12,09,18,32,33),EndDate=new DateTime(2015,12,09,19,27,33)},
            new classTask(){ ID=3,Name="任务3",Content="代码上传与整理",StartDate=new DateTime(2015,12,17,13,32,33),EndDate=new DateTime(2015,12,17,17,27,33)},
            new classTask(){ ID=4,Name="任务4",Content="上线测试",StartDate=new DateTime(2015,12,30,15,32,33),EndDate=new DateTime(2015,12,15,17,27,33)},
            new classTask(){ ID=5,Name="任务5",Content="代码上传与整理",StartDate=new DateTime(2015,12,07,13,32,33),EndDate=new DateTime(2015,12,07,17,27,33)}
            };

            JavaScriptSerializer jss = new JavaScriptSerializer();
            List<Dictionary<string, object>> gas = new List<Dictionary<string, object>>();
            foreach (var entity in tasks)
            {
                Dictionary<string, object> drow = new Dictionary<string, object>();
                drow.Add("id", entity.ID);
                drow.Add("title", string.Format("任务名称：{0}", entity.Name));
                drow.Add("start", ReturnDate(entity.StartDate));
                drow.Add("end", ReturnDate(entity.EndDate));
                //鼠标悬浮上展现的是这个属性信息，可以自己设置
                drow.Add("fullname", string.Format("任务名称：{0}", entity.Name));
                drow.Add("allDay", false);
                gas.Add(drow);
            }
            context.Response.Write(jss.Serialize(gas));
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