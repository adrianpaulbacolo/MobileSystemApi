using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Configuration;

public partial class LiveChat_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            string strPageName = "LiveChat";
            string strMemberId = string.Empty;
            string strMemberCode = string.Empty;
            string riskId = string.Empty;

            try
            {
                    string shortlang = commonVariables.SelectedLanguageShort;
                    string lang = commonVariables.SelectedLanguage.ToLower();
                    bool isVIP = false;

                    string CurrentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
                    string chatLang = string.Empty;
                    string skill = string.Empty;

                    string platform = "Mobile";
                    string redirectLink = string.Empty;


                    if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                    {
                        strMemberId = commonVariables.GetSessionVariable("MemberId");
                        strMemberCode = commonVariables.GetSessionVariable("MemberCode");
                        riskId = commonVariables.GetSessionVariable("RiskId");
                        if (riskId.Length >= 3)
                        {
                            if (riskId.Trim().ToLower() == "vipg" || riskId.ToLower() == "vipd" || riskId.ToLower() == "vipp")
                                isVIP = true;
                        }
                    }

                    //BO settings integration
                    try
                    {
                        var livechat_type = string.Empty;

                        DataSet dataSet = new DataSet();
                        dataSet = GetLiveChatLinks(Convert.ToInt64(ConfigurationManager.AppSettings["W88_Operator"]), lang);

                        string KM = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("KM"));
                        var code1 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(strMemberCode));
                        var code2 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(platform));
                        var code3 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(CurrentUrl));
                        var code4 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(strMemberId));
                        var code5 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString()));

                        if (isVIP)
                        {
                            redirectLink = dataSet.Tables[0].Rows[0]["VIPLink"].ToString();
                            livechat_type = dataSet.Tables[0].Rows[0]["VIPLinkType"].ToString();
                        }
                        else
                        {
                            redirectLink = dataSet.Tables[0].Rows[0]["NONVIPLink"].ToString();
                            livechat_type = dataSet.Tables[0].Rows[0]["NONVIPLinkType"].ToString();
                        }


                        if (livechat_type.ToLower() == "liveperson")
                        {
                            chatLang = string.Empty;
                            skill = "English";
                            switch (lang)
                            {
                                case "id-id": chatLang = "Indonesia"; skill = "Indonesia"; break;
                                case "ja-jp": chatLang = "Japanese"; skill = "Japanese"; break;
                                case "km-kh": chatLang = "Cambodia"; skill = "Cambodia"; break;
                                case "ko-kr": chatLang = "Korean"; skill = "Korean"; break;
                                case "th-th": chatLang = "Thailand"; skill = "Thailand"; break;
                                case "vi-vn": chatLang = "Vietnamese"; skill = "Vietnamese"; break;
                                case "zh-cn": chatLang = "Chinese"; skill = "Chinese"; break;
                                default: chatLang = "English"; break;
                            }
                            if (isVIP)
                            {
                                switch (lang)
                                {
                                    case "id-id": chatLang = "VIP-Bahasa"; skill = "VIP-Bahasa"; break;
                                    case "ja-jp": chatLang = "Japanese"; skill = "Japanese"; break;
                                    case "km-kh": chatLang = "VIP-Cambodian"; skill = "VIP-Cambodian"; break;
                                    case "ko-kr": chatLang = "VIP-Korean"; skill = "VIP-Korean"; break;
                                    case "th-th": chatLang = "VIP-Thailand"; skill = "VIP-Thailand"; break;
                                    case "vi-vn": chatLang = "VIP-TiengViet "; skill = "VIP-TiengViet "; break;
                                    case "zh-cn": chatLang = "VIP-Chinese "; skill = "VIP-Chinese "; break;
                                    case "en-us": chatLang = "VIP-English"; skill = "VIP-English"; break;
                                    //default: chatLang = "VIP-English"; skill = "VIP-English"; break;
                                    default: break;
                                }
                            }

                            code1 = skill;
                            code2 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(CurrentUrl));
                            code3 = chatLang;

                            redirectLink = string.Format(redirectLink, code1, code2, code3);
                        }
                        else
                        {
                            redirectLink = string.Format(redirectLink, code1, code2, code3, code4, code5);
                        }

                    }
                    catch (Exception)
                    {
                        if (shortlang == "en" || shortlang == "kh" || shortlang == "kr" || shortlang == "th" || shortlang == "jp" || shortlang == "id" || shortlang == "vn")
                        {
                            //if (shortlang == "en" || shortlang == "kh" || shortlang == "kr" || shortlang == "th" || shortlang == "jp" || shortlang == "id" || shortlang == "cn")
                            //{
                            #region livezilla

                            string KM = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("KM"));
                            var code1 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(strMemberCode));
                            var code2 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(platform));
                            var code3 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(CurrentUrl));
                            var code4 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(strMemberId));
                            var code5 = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString()));

                            if (string.IsNullOrEmpty(strMemberCode) || string.IsNullOrEmpty(strMemberId))
                            {
                                code1 = "";
                                code4 = "";

                                switch (shortlang)//null member
                                {
                                    case "en":
                                        redirectLink = "http://en.chat.liveperson88.com/chat.php?a=1bb6a&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__";
                                        break;
                                    case "kh":
                                        redirectLink = "http://kh.chat.liveperson88.com/chat.php?a=d1712&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__&el=" + KM;
                                        break;
                                    case "kr":
                                        redirectLink = "http://kr.chat.liveperson88.com/chat.php?a=7b233&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    case "th":
                                        redirectLink = "http://th.chat.liveperson88.com/chat.php?a=d4a99&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    case "id":
                                        redirectLink = "http://id.chat.liveperson88.com/chat.php?a=5c303&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    case "jp":
                                        redirectLink = "http://jp.chat.liveperson88.com/chat.php?a=f3c22&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    case "vn":
                                        redirectLink = "http://vn.chat.liveperson88.com/chat.php?a=1052a&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    case "cn":
                                        redirectLink = "http://cn.chat.liveperson88.com/chat.php?a=1cbe6&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    default:
                                        redirectLink = "http://en.chat.liveperson88.com/chat.php?a=1bb6a&hg=P1ZJUA__&hg=P01hbmFnZXI_&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__";
                                        break;

                                }
                            }
                            else
                            {
                                switch (shortlang)
                                {
                                    case "en":
                                        if (isVIP)
                                            redirectLink = "http://en.chat.liveperson88.com/chat.php?a=a08c2&intgroup=VklQ&hg=Pw__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__";
                                        else
                                            redirectLink = "http://en.chat.liveperson88.com/chat.php?a=1bb6a&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__";
                                        break;
                                    case "kh":
                                        if (isVIP)
                                            redirectLink = "http://kh.chat.liveperson88.com/chat.php?a=35df1&intgroup=VklQ&hg=Pw__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&el=" + KM;
                                        else
                                            redirectLink = "http://kh.chat.liveperson88.com/chat.php?a=d1712&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__&el=" + KM;
                                        break;
                                    case "kr":
                                        if (isVIP)
                                            redirectLink = "http://kr.chat.liveperson88.com/chat.php?a=581f1&intgroup=VklQ&hg=Pw__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                        else
                                            redirectLink = "http://kr.chat.liveperson88.com/chat.php?a=7b233&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    case "th":
                                        if (isVIP)
                                            redirectLink = "http://th.chat.liveperson88.com/chat.php?a=0db09&intgroup=VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                        else
                                            redirectLink = "http://th.chat.liveperson88.com/chat.php?a=d4a99&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    case "id":
                                        if (isVIP)
                                            redirectLink = "http://id.chat.liveperson88.com/chat.php?a=f7bd2&intgroup=VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                        else
                                            redirectLink = "http://id.chat.liveperson88.com/chat.php?a=5c303&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    case "jp":
                                        if (isVIP)
                                            redirectLink = "http://jp.chat.liveperson88.com/chat.php?a=e94d5&intgroup=VklQ&hg=Pw__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                        else
                                            redirectLink = "http://jp.chat.liveperson88.com/chat.php?a=f3c22&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    case "vn":
                                        if (isVIP)
                                            redirectLink = "http://vn.chat.liveperson88.com/chat.php?a=f9d71&intgroup=VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                        else
                                            redirectLink = "http://vn.chat.liveperson88.com/chat.php?a=1052a&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    case "cn":
                                        if (isVIP)
                                            redirectLink = "http://cn.chat.liveperson88.com/chat.php?a=dd1c7&intgroup=VklQ&hg=Pw__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                        else
                                            redirectLink = "http://cn.chat.liveperson88.com/chat.php?a=1cbe6&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                        break;
                                    default:
                                        if (isVIP)
                                            redirectLink = "http://en.chat.liveperson88.com/chat.php?a=a08c2&intgroup=VklQ&hg=Pw__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__";
                                        else
                                            redirectLink = "http://en.chat.liveperson88.com/chat.php?a=1bb6a&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__";
                                        break;
                                }
                            }
                            redirectLink = string.Format(redirectLink, code1, code2, code3, code4, code5);

                            #endregion livezilla

                        }
                        else
                        {
                            #region liveperson
                            if (isVIP)
                            {
                                switch (lang)
                                {
                                    case "id":
                                        chatLang = "VIP-Bahasa"; skill = "VIP-Bahasa";
                                        break;
                                    //case "th":
                                    //    chatLang = "VIP-Thailand"; skill = "VIP-Thailand";
                                    //    break;
                                    case "vn":
                                        chatLang = "VIP-TiengViet "; skill = "VIP-TiengViet ";
                                        break;
                                    case "cn":
                                        chatLang = "VIP-Chinese "; skill = "VIP-Chinese ";
                                        break;
                                    default:
                                        chatLang = "VIP-English"; skill = "VIP-English";
                                        break;
                                }
                            }
                            else
                            {
                                switch (shortlang)
                                {
                                    case "id": chatLang = "Indonesia"; skill = "Indonesia"; break;
                                    case "jp": chatLang = "Japanese"; skill = "Japanese"; break;
                                    //case "kh": chatLang = "Cambodia"; skill = "Cambodia"; break;
                                    //case "kr": chatLang = "Korean"; skill = "Korean"; break;
                                    //case "th": chatLang = "Thailand"; skill = "Thailand"; break;
                                    case "vn": chatLang = "Vietnamese"; skill = "Vietnamese"; break;
                                    case "cn": chatLang = "Chinese"; skill = "Chinese"; break;
                                    default: chatLang = "English"; break;
                                }
                            }
                            redirectLink = "https://server.iad.liveperson.net/hc/88942816/?cmd=file&file=visitorWantsToChat&site=88942816&SV!skill=" + skill + "&leInsId=88942816527642465&skId=1&leEngId=88942816_29aeab82-a5fc-4de7-b801-c6a87c638106&leEngTypeId=8&leEngName=LiveHelp_default&leRepAvState=3&referrer=" + CurrentUrl + "&SESSIONVAR!visitor_profile=" + chatLang;

                            #endregion liveperson
                        }
                    }

                    Response.Redirect(redirectLink);
            }
            catch (Exception ex)
            {

                commonAuditTrail.appendLog("system", strPageName, "PageLoad", "LivechatDefault.DLL", "", "", "Ex: " + ex.Message, "StackTrace: " + ex.StackTrace, "MemberId:" + strMemberId + ", MemberCode:" + strMemberCode + ", RiskId: " + riskId, "", "", true);
            }
        }
    }


  /// <summary>
  /// Invoke GetLiveChatLinks Web Method
  /// </summary>
  /// <param name="operatorId"></param>
  /// <param name="languageCode"></param>
  /// <returns></returns>
  
    public DataSet GetLiveChatLinks(long operatorId, string languageCode)
    {
        DataSet dataset = new DataSet();
        HttpWebRequest request = CreateWebRequest();
        XmlDocument soapEnvelopeXml = new XmlDocument();

        string XML = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                                    <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                                      <soap:Body>
                                        <GetLiveChatLinks xmlns=""http://tempuri.org/"">
                                          <operatorId>{0}</operatorId>
                                          <languageCode>{1}</languageCode>
                                        </GetLiveChatLinks>
                                      </soap:Body>
                                    </soap:Envelope>", operatorId, languageCode);

        soapEnvelopeXml.LoadXml(XML);

        using (Stream stream = request.GetRequestStream())
        {
            soapEnvelopeXml.Save(stream);
        }

        using (WebResponse response = request.GetResponse())
        {
            using (StreamReader rd = new StreamReader(response.GetResponseStream()))
            {
                string soapResult = rd.ReadToEnd();
                StringReader stringReader = new StringReader(soapResult);
                dataset.ReadXml(stringReader, XmlReadMode.ReadSchema);
            }
        }

        return dataset;
    }


    /// <summary>
    ///Create HttpWebRequest 
    /// </summary>
    /// <returns></returns>
    public HttpWebRequest CreateWebRequest()
    {
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["ProductWS"]);
        webRequest.Headers.Add(@"SOAP:Action");
        webRequest.ContentType = "text/xml;charset=\"utf-8\"";
        webRequest.Accept = "text/xml";
        webRequest.Method = "POST";
        return webRequest;
    }




}