using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RM.Busines.IDAO;
using RM.Busines.DAL;
using RM.Common.DotNetUI;
using RM.Common.DotNetBean;
using RM.Common.DotNetConfig;
using RM.Common.DotNetFile;
using System.Data;
using System.Text;
using RM.Common.DotNetCode;
using System.IO;
using RM.Web.App_Code;

namespace RM.Web.RMBase.SysDataCenter
{
    public partial class Backup_List : PageBase
    {
        RM_System_IDAO system_idao = new RM_System_Dal();
        string _UserName;
        protected void Page_Load(object sender, EventArgs e)
        {
            _UserName = RequestSession.GetSessionUser().UserName.ToString();
            DataBindGrid();
        }
        /// <summary>
        /// 备份恢复记录
        /// </summary>
        private void DataBindGrid()
        {
            DataTable dt = system_idao.GetBackup_Restore_Log_List();
            ControlBindHelper.BindRepeaterList(dt, rp_Item);
        }
        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void bntbackups_Click(object sender, EventArgs e)
        {
            string filename = DateTime.Now.ToString("yyyyMMddhhmmssff") + ".bak";
            string FilePath = ConfigHelper.GetAppSettings("BackupsDataPath");
            string filesize;
            FileHelper.CreateDirectory(FilePath);
            bool IsOk = system_idao.DataBackups(FilePath + "//" + filename);
            if (IsOk)
            {
                filesize = FileHelper.GetFileSize(FilePath + "//" + filename);
                system_idao.Add_Backup_Restore_Log("备份", filename, filesize, _UserName, "RM_DB", Backup_Restore_Memo.Value);
                ShowMsgHelper.ShowScript("showTipsMsg('数据库备份成功！','2500','4');top.main.windowload();");
            }
            else
            {
                ShowMsgHelper.Alert_Error("备份失败！");
            }
        }
        //恢复数据库
        protected void btnrecover_Click(object sender, EventArgs e)
        {
            string filename = Backup_Restore_File.Value;
            string FilePath = ConfigHelper.GetAppSettings("BackupsDataPath");
            string filesize;
            FileHelper.CreateDirectory(FilePath);
            bool IsOk = system_idao.DataRestore(FilePath + "//" + filename);
            if (IsOk)
            {
                filesize = FileHelper.GetFileSize(FilePath + "//" + filename);
                system_idao.Add_Backup_Restore_Log("恢复", filename, filesize, _UserName, "RM_DB", Backup_Restore_Memo.Value);
                ShowMsgHelper.ShowScript("showTipsMsg('数据库恢复成功！','2500','4');top.main.windowload();");
            }
            else
            {
                ShowMsgHelper.Alert_Error("恢复失败！");
            }
        }
        /// <summary>
        /// 绑定后激发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rp_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblBackup_Restore_Type = e.Item.FindControl("lblBackup_Restore_Type") as Label;
                if (lblBackup_Restore_Type != null)
                {
                    string textlblBackup_Restore_Type = lblBackup_Restore_Type.Text;
                    textlblBackup_Restore_Type = textlblBackup_Restore_Type.Replace("恢复", "<span style='color:Blue'>恢复</span>");
                    lblBackup_Restore_Type.Text = textlblBackup_Restore_Type;
                }
            }
        }
    }
}