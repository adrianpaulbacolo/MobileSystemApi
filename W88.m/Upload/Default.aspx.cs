using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Upload_Default : BasePage
{
    private System.Xml.Linq.XElement xeResources = null;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e) { base.CheckLogin(); }

    protected void Page_Load(object sender, EventArgs e)
    {
        commonCulture.appData.getLocalResource(out xeResources);

        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceString("submitUpload", commonVariables.LeftMenuXML));

        lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", xeResources);
        txtUsername.Text = commonVariables.GetSessionVariable("MemberCode");

        lblCurrency.Text = commonCulture.ElementValues.getResourceString("lblCurrency", xeResources);
        txtCurrency.Text = commonVariables.GetSessionVariable("CurrencyCode");

        lblRemarks.Text = commonCulture.ElementValues.getResourceString("lblRemarks", xeResources);

        lblFileUpload.Text = commonCulture.ElementValues.getResourceString("lblFileUpload", xeResources);

        btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

        lblSuccess.Text = commonCulture.ElementValues.getResourceString("Success", xeResources);
        lblSuccess.Visible = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string strSMTPHost = System.Configuration.ConfigurationManager.AppSettings.Get("SMTPHOST");
        string strEmailFrom = "fileupload-mobile@w88.com";
        string strUsername = commonVariables.GetSessionVariable("MemberCode");
        string strRemarks = txtRemarks.Text;
        string strCurrency = commonVariables.GetSessionVariable("CurrencyCode");
        string strSubmissionID = System.DateTime.Now.ToString("hhmmssddMMyy");
        //string strUploadRecipients = System.Configuration.ConfigurationManager.AppSettings.Get("UploadRecipients");
        int fileSize = fuFileUpload.PostedFile.ContentLength;
        string fileExtension = System.IO.Path.GetExtension(fuFileUpload.PostedFile.FileName.ToString());
        System.Text.RegularExpressions.Regex rexFileExt = new System.Text.RegularExpressions.Regex("(.gif|.jpg|.png)");

        if (fuFileUpload.HasFile)
        {
            if (rexFileExt.IsMatch(fileExtension))
            {
                if (fileSize <= (3 * 1024 * 1024))
                {
                    try
                    {
                        System.Net.Mail.SmtpClient sClient = new System.Net.Mail.SmtpClient();

                        sClient.Host = strSMTPHost;
                        sClient.Port = 25;

                        if (string.Compare(sClient.Host, "retail.smtp.com", true) == 0)
                        {
                            System.Net.NetworkCredential nCredentials = new System.Net.NetworkCredential();
                            nCredentials.UserName = "dev@w88.com";
                            nCredentials.Password = "2NDbr0isFAT!";
                            sClient.UseDefaultCredentials = false;
                            sClient.Credentials = nCredentials;
                        }

                        using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage())
                        {
                            message.From = new System.Net.Mail.MailAddress(strEmailFrom);
                            message.To.Add("banking@w88.com");
                            message.To.Add("doc@w88.com");

                            switch (strCurrency.ToLower())
                            {
                                case "myr":
                                    message.CC.Add("CS_English@aquozsolutions.com");
                                    message.CC.Add("team_cscambodia@aquozsolutions.com");
                                    break;

                                case "usd":
                                    message.CC.Add("team_cscambodia@aquozsolutions.com");
                                    message.CC.Add("CS_English@aquozsolutions.com");
                                    break;

                                case "rmb":
                                    message.CC.Add("inforcn@w88.com");
                                    break;

                                case "idr":
                                    message.CC.Add("team_csindo@aquozsolutions.com");
                                    break;

                                case "thb":
                                    message.CC.Add("team_csthai@aquozsolutions.com");
                                    break;

                                case "krw":
                                    message.CC.Add("DG-CsKorea@aquozsolutions.com");
                                    break;

                                case "vnd":
                                    message.CC.Add("team_csvietnam@aquozsolutions.com");
                                    break;
                            }

                            message.Body = string.Format("Username: {0}{1}Currency: {2}{3}Remarks: {4}", strUsername, System.Environment.NewLine, strCurrency, System.Environment.NewLine, strRemarks);
                            message.Subject = string.Format("Attachment Upload - {0} / {1} / {2}", strSubmissionID, strUsername, strCurrency);
                            message.Attachments.Add(new System.Net.Mail.Attachment(fuFileUpload.PostedFile.InputStream, fuFileUpload.PostedFile.FileName));
                            sClient.Send(message);
                        }

                        strAlertCode = "00";
                        lblSuccess.Text = commonCulture.ElementValues.getResourceString("Success", xeResources).Replace("[SubmissionID]", strSubmissionID);
                        lblSuccess.Visible = true;
                    }
                    catch (Exception ex)
                    {
                        //textBox4.Text += Environment.NewLine + ex.Message;
                    }
                    GC.Collect();
                }
                else
                {
                    strAlertCode = "01";
                    strAlertMessage = commonCulture.ElementValues.getResourceString("ExceedSizeLimit", xeResources);
                    return;
                }
            }
            else
            {
                strAlertCode = "01";
                strAlertMessage = commonCulture.ElementValues.getResourceString("InvalidFileType", xeResources);
            }
        }
        else
        {
            strAlertCode = "01";
            strAlertMessage = commonCulture.ElementValues.getResourceString("MissingAttachment", xeResources);
        }
    }

}