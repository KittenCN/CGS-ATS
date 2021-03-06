﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Busines;
using System.Collections;
using RM.Common.DotNetUI;
using RM.Common.DotNetBean;
using RM.Common.DotNetCode;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysAppend
{
    public partial class AppendProperty_Form : PageBase
    {
        string _Function, _key;
        protected void Page_Load(object sender, EventArgs e)
        {
            _Function = Server.UrlDecode(Request["Function"]);  //所属功能
            _key = Request["key"];                              //主键
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(_key))
                {
                    InitData();
                }
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_AppendProperty", "Property_ID", _key);
            if (ht.Count > 0 && ht != null)
            {
                ControlBindHelper.SetWebControls(this.Page, ht);
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, EventArgs e)
        {
            Hashtable ht = new Hashtable();
            ht = ControlBindHelper.GetWebControls(this.Page);
            if (!string.IsNullOrEmpty(_key))
            {
                ht["ModifyDate"] = DateTime.Now;
                ht["ModifyUserId"] = RequestSession.GetSessionUser().UserId;
                ht["ModifyUserName"] = RequestSession.GetSessionUser().UserName;
            }
            else
            {
                ht["Property_Function"] = _Function;
                ht["Property_ID"] = CommonHelper.GetGuid;
                ht["CreateUserId"] = RequestSession.GetSessionUser().UserId;
                ht["CreateUserName"] = RequestSession.GetSessionUser().UserName;
            }
            bool IsOk = DataFactory.SqlDataBase().Submit_AddOrEdit("Base_AppendProperty", "Property_ID", _key, ht);
            if (IsOk)
            {
                ShowMsgHelper.ParmAlertMsg("Success！");
            }
            else
            {
                ShowMsgHelper.Alert_Error("Error！");
            }
        }
    }
}