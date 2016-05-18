using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RM.Web.RMBase.SysATS
{
    public class classTask
    {
        public int ID { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Begin Time
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// End Time
        /// </summary>
        public DateTime EndDate { get; set; }
    }
}