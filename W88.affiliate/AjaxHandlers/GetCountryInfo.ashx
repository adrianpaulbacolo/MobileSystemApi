<%@ WebHandler Language="C#" Class="AjaxHandlers_ASHX_GetCountryInfo" %>

using System;
using System.Web;

public class AjaxHandlers_ASHX_GetCountryInfo : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string strOperatorId = commonVariables.OperatorId;
        
        string processCode = string.Empty;
        string processText = string.Empty;

        string strCountryCode = context.Request.QueryString.Get("CountryCode");
        string strCountryPhoneCode = string.Empty;
        
        if (string.IsNullOrEmpty(strCountryCode)) { strCountryCode = context.Request.Form.Get("CountryCode"); }
        
        System.Data.DataSet dsCountryInfo = null;
        #region productWalletBalance

        if (!string.IsNullOrEmpty(strOperatorId) && !string.IsNullOrEmpty(strCountryCode))
        {
            using (wsMemberMS1.memberWSSoapClient wsInstance = new wsMemberMS1.memberWSSoapClient())
            {
                dsCountryInfo = wsInstance.GetCountryInfo(Convert.ToInt64(strOperatorId));
                //strCountryPhoneCode = Convert.ToString(dsCountryInfo.Tables[0].Rows[0]["countryPhoneCode"]);
                
                try
                {
                    strCountryPhoneCode = Convert.ToString(dsCountryInfo.Tables[0].Select("[countryCode] = '" + strCountryCode + "'", "")[0]["countryPhoneCode"]);
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    //strCountryPhoneCode = Convert.ToString(dsCountryInfo.Tables[0].Select("[countryCode] = 'CN'", "")[0]["countryPhoneCode"]);
                }
                catch (Exception ex)
                {
                }
            }
        }
        
        #endregion

        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Write(strCountryPhoneCode);
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}