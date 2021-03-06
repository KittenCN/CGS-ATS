﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Net.Mail;
using System.Text;
using RM.Busines;
//using Microsoft.Office.Interop.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Data;

namespace RM.Web
{
    public class GenModel
    {
        //SendMail(发件者, 收件者, 主旨, 内容, 主机,发件者昵称, 密码 ,Attachment) 
        public string SendMail(string send, string recieve, string subject, string mailbody)
        {
            ConHelper ch =new ConHelper();
            ch.GetMailSetting();
            string host = ch.strHost;
            string uname = ch.strUname;
            string pwd = ch.strPWD;
            string strFileName = "";

            int intRecFlag = 0;

            //生成一个   使用SMTP发送邮件的客户端对象  
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //生成一个主机IP  
            //client.Port = 25; //587, 465, 995  
            client.Host = host;

            //表示不以当前登录用户的默认凭据进行身份验证  
            client.UseDefaultCredentials = true;
            //包含用户名和密码  
            if (uname != "")
            {
                client.Credentials = new System.Net.NetworkCredential(uname, pwd);
            }

            //指定如何发送电子邮件。  
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.PickupDirectoryFromIis;
            //通过本机SMTP服务器传送该邮件，  
            //其实使用该项的话就可以随意设定“主机,发件者昵称, 密码”，因为你的IIS服务器已经设定好了。而且公司内部发邮件是不需要验证的。  

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            if(recieve==null || recieve=="")
            {
                message.To.Add(send);
                intRecFlag = 1;
            }
            else
            {
                message.To.Add(recieve);
                intRecFlag = 0;
            }
            message.From = new System.Net.Mail.MailAddress(send, uname, System.Text.Encoding.UTF8);
            message.Subject = subject;
            message.Body = mailbody;
            //定义邮件正文，主题的编码方式  
            message.BodyEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            message.SubjectEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            //获取或设置一个值，该值指示电子邮件正文是否为   HTML。  
            message.IsBodyHtml = false;
            //指定邮件优先级  
            message.Priority = System.Net.Mail.MailPriority.High;
            //添加Attachment  
            //System.Net.Mail.Attachment data = new Attachment(@"E:\9527\tubu\PA260445.JPG", System.Net.Mime.MediaTypeNames.Application.Octet);  
            if (strFileName != "" && strFileName != null)
            {
                Attachment data = new Attachment(strFileName);
                message.Attachments.Add(data);
            }

            try
            {
                //发送  
                client.Send(message);
                string strSResult = "Send Mail Success!";
                string strEResult = "Unknow Receiver Mail Add.,Mail sent to DEFAULT Add.!";
                if(intRecFlag==1)
                {
                    return strEResult;
                }
                else
                {
                    return strSResult;
                }
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                return "Mail Error：" + ex.Message;
            }
        }

        public string SendMail2(string Recieve, string MailSubject, string MailBody)
        {
            ConHelper ch = new ConHelper();
            ch.GetMailSetting();
            string host = ch.strHost;
            string uname = ch.strUname;
            string pwd = ch.strPWD;
            string sender = ch.strSender;
            int intRecFlag = 0;

            try
            {
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                client.Host = host;//使用163的SMTP服务器发送邮件
                client.UseDefaultCredentials = true;
                client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                client.Credentials = new System.Net.NetworkCredential(uname, pwd);//163的SMTP服务器需要用163邮箱的用户名和密码作认证，如果没有需要去163申请个,
                                                                                           //这里假定你已经拥有了一个163邮箱的账户，用户名为abc，密码为*******
                System.Net.Mail.MailMessage Message = new System.Net.Mail.MailMessage();
                Message.From = new System.Net.Mail.MailAddress(sender);//这里需要注意，163似乎有规定发信人的邮箱地址必须是163的，而且发信人的邮箱用户名必须和上面SMTP服务器认证时的用户名相同
                                                                       //因为上面用的用户名abc作SMTP服务器认证，所以这里发信人的邮箱地址也应该写为abc@163.com
                if (Recieve == null || Recieve == "")
                {
                    Message.To.Add(sender);
                    intRecFlag = 1;
                }
                else
                {
                    Message.To.Add(Recieve);
                    intRecFlag = 0;
                }
                Message.Subject = MailSubject;
                Message.Body = MailBody;
                Message.SubjectEncoding = System.Text.Encoding.UTF8;
                Message.BodyEncoding = System.Text.Encoding.UTF8;
                Message.Priority = System.Net.Mail.MailPriority.High;
                Message.IsBodyHtml = true;
                client.Send(Message);
                string strSResult = "Send Mail Success!";
                string strEResult = "Unknow Receiver Mail Add.,Mail sent to DEFAULT Add.!";
                if (intRecFlag == 1)
                {
                    return strEResult;
                }
                else
                {
                    return strSResult;
                }
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                return "Mail Error：" + ex.Message;
            }
        }

        public string GetEMailFromID(string EmpID)
        {
            string txt_Result = "";

            string sql = "select EMail from Base_UserInfo where User_ID='" + EmpID + "' ";
            StringBuilder sb_sql = new StringBuilder(sql);
            System.Data.DataTable dt = DataFactory.SqlDataBase().GetDataTableBySQL(sb_sql);
            if (dt.Rows.Count != 0 && dt.Rows[0].ItemArray[0].ToString() != "")
            {
                txt_Result = dt.Rows[0].ItemArray[0].ToString();
            }

            return txt_Result;
        }

        public int ExportExcel(System.Data.DataTable tmpDataTable, string strFileName)
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;
            FileStream fs = null;
            IWorkbook workbook = null;
            string sheetName = "ATSReport";
            bool isColumnWritten = true;
            fs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            if (strFileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook();
            else if (strFileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook();
            try
            {
                if (workbook != null)
                {
                    sheet = workbook.CreateSheet(sheetName);
                }
                else
                {
                    return -1;
                }
                if (isColumnWritten == true) //写入DataTable的列名
                {
                    IRow row = sheet.CreateRow(0);
                    for (j = 0; j < tmpDataTable.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(tmpDataTable.Columns[j].ColumnName);
                    }
                    count = 1;
                }
                else
                {
                    count = 0;
                }

                for (i = 0; i < tmpDataTable.Rows.Count; ++i)
                {
                    IRow row = sheet.CreateRow(count);
                    for (j = 0; j < tmpDataTable.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(tmpDataTable.Rows[i][j].ToString());
                    }
                    ++count;
                }
                workbook.Write(fs); //写入到excel
                fs.Close();
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return -1;
            }
        }

        public void rpExportExcel(ref System.Web.UI.WebControls.Repeater rp, string strFileName, String FileType)
        {
            //	DataTable dt = (DataTable)this.Session["GridToExcel"];
            //	if (dt==null) return;
            strFileName = System.Web.HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8);

            //System.IO.StringWriter sw = new System.IO.StringWriter();
            //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            //rp.RenderControl(hw);

            //System.Web.HttpContext.Current.Response.Clear();
            //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //System.Web.HttpContext.Current.Response.Charset = "";
            //rp.Page.EnableViewState = false;

            //System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + strFileName + ".xls");
            //System.Web.HttpContext.Current.Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=GB2312\"><title>Copyright by SDU</title></head><body><center>");
            //System.Web.HttpContext.Current.Response.Write(sw.ToString());
            //System.Web.HttpContext.Current.Response.Write("</center></body></html>");
            //System.Web.HttpContext.Current.Response.End();

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.BufferOutput = true;
            //设定输出字符集
            System.Web.HttpContext.Current.Response.Charset = "GB2312";
            System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            System.Web.HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename="
            + HttpUtility.UrlEncode(strFileName, System.Text.Encoding.UTF8));
            //设置输出流HttpMiME类型(导出文件格式)
            System.Web.HttpContext.Current.Response.ContentType = FileType;
            //关闭ViewState
            rp.Page.EnableViewState = false;
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ZH-CN", true);
            System.IO.StringWriter stringWriter = new System.IO.StringWriter(cultureInfo);
            HtmlTextWriter textWriter = new HtmlTextWriter(stringWriter);
            //rpt_pro为repeater控件的ID
            //数据源要有边框，否则导出数据也无边框
            rp.RenderControl(textWriter);
            //把HTML写回游览器
            System.Web.HttpContext.Current.Response.Write(stringWriter.ToString());
            System.Web.HttpContext.Current.Response.End();
            System.Web.HttpContext.Current.Response.Flush();
        }
    }
}