using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using RM.Busines.DAL;
using RM.Busines.IDAO;
using System.Data;
using RM.Common.DotNetData;
using RM.Busines;
using System.Collections;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysPersonal
{
    public partial class Recyclebin_Info : PageBase
    {
        public StringBuilder str_OutputHtml = new StringBuilder();
        RM_System_IDAO systemidao = new RM_System_Dal();
        public string _Recyclebin_Name, _key,_ItemValue;
        protected void Page_Load(object sender, EventArgs e)
        {
            _key = Request["key"];//对象主键
            LoadInfoHtml();
        }
        public void LoadInfoHtml()
        {
            Hashtable ht = DataFactory.SqlDataBase().GetHashtableById("Base_Recyclebin", "Recyclebin_ID", _key);
            if (ht.Count > 0 && ht != null)
            {
                _Recyclebin_Name = ht["RECYCLEBIN_NAME"].ToString();
                DataTable dt = systemidao.GetRecyclebin_ObjField(ht["RECYCLEBIN_TABLE"].ToString());
                if (DataTableHelper.IsExistRows(dt))
                {
                    int rowSum = dt.Rows.Count;
                    for (int i = 0; i < rowSum; i++)
                    {
                        str_OutputHtml.Append("<tr>");
                        str_OutputHtml.Append("<th>" + dt.Rows[i]["Field_Name"] + "</th>");
                        str_OutputHtml.Append("<td>");
                        str_OutputHtml.Append("<lable id=\"" + dt.Rows[i]["Field_Key"].ToString().ToUpper() + "\"/>");
                        str_OutputHtml.Append("</td>");
                        str_OutputHtml.Append("</tr>");
                    }
                }
                Hashtable Obj_ht = DataFactory.SqlDataBase().GetHashtableById(ht["RECYCLEBIN_TABLE"].ToString(), ht["RECYCLEBIN_FIELDKEY"].ToString(), ht["RECYCLEBIN_EVENTFIELD"].ToString());
                foreach (string key in Obj_ht.Keys)
                {
                    _ItemValue += key + "∫" + Obj_ht[key] + "∮";
                }
            }
        }
    }
}