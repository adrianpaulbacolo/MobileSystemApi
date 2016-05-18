<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MainWalletBalance.ascx.cs" Inherits="UserControls.UserControlsMainWalletBalance" %>


<label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
<h2 class="value"><%=Session["Main"].ToString()%></h2>
<small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>