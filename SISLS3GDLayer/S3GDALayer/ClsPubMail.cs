
using System.Collections.Generic;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using S3GBusEntity;
using System.Data.SqlClient;
using System.Data;
using System;using S3GDALayer.S3GAdminServices;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;

namespace S3GDALayer
{
    [Serializable]
    public class ClsPubMail
    {
        SqlConnection sqlcon = null;   
        SqlCommand sqlcom;
        SqlDataAdapter sqldap;
        DataSet ds;

        //Code added for getting common connection string  from config file
            Database db;
            

        string strSMTPServerName = string.Empty;
        string strUserId_Email = string.Empty;
        string strEmail_Pwd = string.Empty;
        AppSettingsReader ObjAppSettReader = new AppSettingsReader();
        public ClsPubMail()
        {
            strSMTPServerName = (string)ObjAppSettReader.GetValue("SMTPServer", typeof(string));
            db = S3GDALayer.Common.ClsIniFileAccess.FunPubGetDatabase();
            //Database db = DatabaseFactory.CreateDatabase("S3GconnectionString");
            sqlcon = new SqlConnection(db.ConnectionString);//((string)ObjAppSettReader.GetValue("connectionString", typeof(string)));
        }
        public void FunSendMail(ClsPubCOM_Mail ObjComMail)
        {

            try
            {
                MailMessage ObjMailMessage = new MailMessage();
                string[] arrToEMail;
                string[] arrCCEMail;
                string[] arrBCcEMail;

                ObjMailMessage.From = new MailAddress(ObjComMail.ProFromRW, ObjComMail.ProDisplayName);
                //opc056 start
                strEmail_Pwd = (string)ObjAppSettReader.GetValue(ObjComMail.ProFromRW, typeof(string));
                //if (ObjComMail.ProFromRW == "opc-po@opc.co.in")
                //    strEmail_Pwd = (string)ObjAppSettReader.GetValue("PO_Email_Pwd", typeof(string));
                //if (ObjComMail.ProFromRW == "s3gnoreply@opc.co.in")
                //    strEmail_Pwd = (string)ObjAppSettReader.GetValue("S3G_Support_Email_Pwd", typeof(string));
                //opc056 end
                //; semi colon added as another separator charactor for email id split 
                arrToEMail = ObjComMail.ProTORW.Split(new char[] { ',',';' });
                
                foreach (string strMailAddress in arrToEMail)
                {
                    if (!string.IsNullOrEmpty(strMailAddress))
                    {
                        ObjMailMessage.To.Add(strMailAddress);
                    }
                }

                if (!string.IsNullOrEmpty(ObjComMail.ProCCRW))
                {
                    arrCCEMail = ObjComMail.ProCCRW.Split(new char[] { ',', ';' });
                    foreach (string strMailAddress in arrCCEMail)
                    {
                        if (!string.IsNullOrEmpty(strMailAddress))
                        {
                            ObjMailMessage.CC.Add(strMailAddress);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(ObjComMail.ProBCCRW))
                {
                    arrBCcEMail = ObjComMail.ProBCCRW.Split(new char[] { ',', ';' });
                    foreach (string strMailAddress in arrBCcEMail)
                    {
                        if (!string.IsNullOrEmpty(strMailAddress))
                        {
                            ObjMailMessage.Bcc.Add(strMailAddress);
                        }
                    }
                }
                ObjMailMessage.Subject = string.IsNullOrEmpty(ObjComMail.ProSubjectRW) ? string.Empty : ObjComMail.ProSubjectRW;
                ObjMailMessage.IsBodyHtml = true;
                //ObjMailMessage.Body = ObjComMail.ProMessageRW;
                //ObjMailMessage.Headers.Add("Disposition-Notification-To", "email address");
                ObjMailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess & DeliveryNotificationOptions.Delay & DeliveryNotificationOptions.OnFailure;
                if(ObjComMail.ProImgPathRW!=null)
                    ObjMailMessage.AlternateViews.Add(Mail_Body(ObjComMail.ProImgPathRW, ObjComMail.ProMessageRW));
                else
                    ObjMailMessage.Body = ObjComMail.ProMessageRW;

                ////single file attachment
                //if (!string.IsNullOrEmpty(FileAttachmentPath))
                //{
                //    Attachment objAttachment = new Attachment(FileAttachmentPath);
                //    ObjMailMessage.Attachments.Add(objAttachment);
                //}
                //Multi file attachment

                if (ObjComMail.ProFileAttachementRW != null)
                {
                    foreach (string ObjAttachmentCollection in ObjComMail.ProFileAttachementRW)
                    {
                        if (!string.IsNullOrEmpty(ObjAttachmentCollection))
                        {
                            Attachment objAttachmentS = new Attachment(ObjAttachmentCollection);
                            ObjMailMessage.Attachments.Add(objAttachmentS);
                        }
                    }
                }
                ObjMailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                //added by kali on 23-Mar-2023 to solve err during office 365 domain changes
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                //end 
                SmtpClient ObjClient = new SmtpClient(strSMTPServerName);
                //Added on 07Jul22 for opc038
                ObjClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                ObjClient.EnableSsl = true;
                ObjClient.Port = 587;

                // setup Smtp authentication
                System.Net.NetworkCredential credentials =
                    new System.Net.NetworkCredential(ObjComMail.ProFromRW, strEmail_Pwd);
                ObjClient.UseDefaultCredentials = false;
                ObjClient.Credentials = credentials;
                //ObjClient.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                //opc038 end
                ObjClient.Send(ObjMailMessage);
                
            }
            catch (Exception ex)
            {
                ClsPubCommErrorLogDal.CustomErrorRoutine(ex);
                throw ex;
            }

        }
        //opc038 start
        private AlternateView Mail_Body(string imgPath,string strBody)
        {
            LinkedResource Img = new LinkedResource(imgPath, MediaTypeNames.Image.Jpeg);
            Img.ContentId = "CompanyLogoEmail";
            AlternateView AV =
            AlternateView.CreateAlternateViewFromString(strBody, null, MediaTypeNames.Text.Html);
            AV.LinkedResources.Add(Img);
            return AV;
        }
        //end
        public string[] GetData(string prefixText, int count)
        {
            DataView data = GetView();

            data = FilterData(data, prefixText);

            int intTotalCount = data.Count;
            if (data.Count > count)
                intTotalCount = count;

            string[] suggestion = new string[intTotalCount];

            int i = 0;
            foreach (DataRowView row in data)
            {
                suggestion[i++] = row["Program_Name"] as string;
                if (i >= count) break;
            }
            return suggestion;
        }

        private DataView GetView()
        {
            //   DataView view = (DataView)HttpContext.Current.Cache["Suggestion"];
            // if (view == null)
            //{
            //string strConnectionString = Convert.ToString(ConfigurationManager.AppSettings["connectionString"]);

            DataView view = new DataView();

            SqlDataAdapter adapter = new SqlDataAdapter("select Program_Name from snxg_program_Master", sqlcon);

            DataTable table = new DataTable();
            adapter.Fill(table);
            view = table.DefaultView;

            //  HttpContext.Current.Cache["Suggestion"] = view;
            //}
            return view;
        }
        private DataView FilterData(DataView view, string strPrefixText)
        {
            view.RowFilter = string.Format("Program_Name like '{0}%'", strPrefixText);
            return view;
        }
    }
}
