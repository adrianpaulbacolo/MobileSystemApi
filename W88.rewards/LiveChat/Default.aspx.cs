using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class LiveChat_Default : System.Web.UI.Page
{

    private List<string> liveChatShortLang = new List<string>() { "en", "kh", "kr", "th", "jp", "id", "vn" };
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

                Uri myUri = new Uri(CurrentUrl);
                string[] host = myUri.Host.Split('.');
                string domain = string.Format(ConfigurationManager.AppSettings["WebHandler"], host[1]);


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

                try
                {
                    if (lang == "zh-cn" || lang == "vi-vn")
                    {
                        Uri Myuri_ = new Uri(CurrentUrl);
                        string[] host_ = myUri.Host.Split('.');
                        string domain_ = string.Format(ConfigurationManager.AppSettings["LivePersonMobile"], host_[1]);
                        redirectLink = domain_;
                    }
                    else
                    {
                        redirectLink = domain + CurrentUrl;
                    }
                }
                catch (Exception)
                {
                    if (liveChatShortLang.Exists(sl => sl.Equals(shortlang)))
                    {
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
                                    redirectLink = "http://en.liveperson88.net/live/chat.php?a=db524&hg=P01hbmFnZXI_&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__";
                                    break;
                                case "kh":
                                    redirectLink = "http://kh.liveperson88.net/live/chat.php?a=1e3b4&hg=P01hbmFnZXI_&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__&el=" + KM;
                                    break;
                                case "kr":
                                    redirectLink = "http://kr.liveperson88.net/live/chat.php?a=882d3&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                case "th":
                                    redirectLink = "http://th.liveperson88.net/live/chat.php?a=b915d&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                case "id":
                                    redirectLink = "http://id.liveperson88.net/live/chat.php?a=8e452&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                case "jp":
                                    redirectLink = "http://jp.liveperson88.net/live/chat.php?a=c846b&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                case "vn":
                                    redirectLink = "http://vn.chat.liveperson88.com/chat.php?a=1052a&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                default:
                                    redirectLink = "http://en.liveperson88.net/live/chat.php?a=db524&hg=P01hbmFnZXI_&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__";
                                    break;

                            }
                        }
                        else
                        {
                            switch (shortlang)
                            {
                                case "en":
                                    if (isVIP)
                                        redirectLink = "http://en.liveperson88.net/live/chat.php?a=a78ef&intgroup=VklQ&hg=P01hbmFnZXI_&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__";
                                    else
                                        redirectLink = "http://en.liveperson88.net/live/chat.php?a=a58c2&hg=P01hbmFnZXI,VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__";
                                    break;
                                case "kh":
                                    if (isVIP)
                                        redirectLink = "http://kh.liveperson88.net/live/chat.php?a=618aa&intgroup=VklQ&hg=P01hbmFnZXI_&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&el=" + KM;
                                    else
                                        redirectLink = "http://kh.liveperson88.net/live/chat.php?a=8d2b0&hg=P01hbmFnZXI,VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__&el=" + KM;
                                    break;
                                case "kr":
                                    if (isVIP)
                                        redirectLink = "http://kr.liveperson88.net/live/chat.php?a=a6071&intgroup=VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                    else
                                        redirectLink = "http://kr.liveperson88.net/live/chat.php?a=882d3&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                case "th":
                                    if (isVIP)
                                        redirectLink = "http://th.liveperson88.net/live/chat.php?a=55660&intgroup=VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                    else
                                        redirectLink = "http://th.liveperson88.net/live/chat.php?a=b915d&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                case "id":
                                    if (isVIP)
                                        redirectLink = "http://id.liveperson88.net/live/chat.php?a=c17ec&intgroup=VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                    else
                                        redirectLink = "http://id.liveperson88.net/live/chat.php?a=8e452&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                case "jp":
                                    if (isVIP)
                                        redirectLink = "http://jp.liveperson88.net/live/chat.php?a=a7732&intgroup=VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                    else
                                        redirectLink = "http://jp.liveperson88.net/live/chat.php?a=c846b&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                case "cn":
                                    if (isVIP)
                                        redirectLink = "http://cn.liveperson88.net/live/chat.php?a=64397&intgroup=VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                    else
                                        redirectLink = "http://cn.liveperson88.net/live/chat.php?a=13bb1&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                case "vn":
                                    if (isVIP)
                                        redirectLink = "http://vn.chat.liveperson88.com/live/chat.php?a=37d97&intgroup=VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}";
                                    else
                                        redirectLink = "http://vn.chat.liveperson88.com/live/chat.php?a=eea3e&hg=P1ZJUA__&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&mp=MQ__&rgs=MQ__";
                                    break;
                                default:
                                    if (isVIP)
                                        redirectLink = "http://en.liveperson88.net/live/chat.php?a=a78ef&intgroup=VklQ&hg=P01hbmFnZXI_&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__";
                                    else
                                        redirectLink = "http://en.liveperson88.net/live/chat.php?a=a58c2&hg=P01hbmFnZXI,VklQ&en={0}&cf0={1}&cf1={2}&cf2={3}&cf3={4}&dl=MQ__&rgs=MQ__";
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

}