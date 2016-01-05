<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SDPay_app.aspx.cs" Inherits="Deposit_SDPay" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("sdpay", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title">Wallet Transfer</h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>

            <div data-role="navbar">
                <ul>
                   <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.FastDeposit))%>'><a href="/Deposit/Default_app.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("fastdeposit", commonVariables.LeftMenuXML)%></a></li>
                     <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "usd", true) == 0)
                      { %>
                    <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.WingMoney))%>'><a href="/Deposit/WingMoney_app.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML)%></a></li>
                    <% } %>
                    <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.SDPay))%>'><a class="ui-btn-active" href="/Deposit/SDPay_app.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("sdpay", commonVariables.LeftMenuXML)%></a></li>
                     <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "myr", true) == 0 || string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "thb", true) == 0)
                      { %>
                    <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.Help2Pay))%>'><a href="/Deposit/Help2Pay_app.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("help2pay", commonVariables.LeftMenuXML)%></a></li>
                    <% } %>
                    <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "rmb", true) == 0)
                    { %>
                    <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.DaddyPay))%>'><a runat="server" id="daddyPay_link" href="/Deposit/DaddyPay.aspx?value=1" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("daddypay", commonVariables.LeftMenuXML)%></a></li>
                    <% } %>
                    <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "myr", true) == 0 || string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "thb", true) == 0)
                    { %>
                    <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.NextPay))%>'><a  href="/Deposit/NextPay.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("nextpay", commonVariables.LeftMenuXML)%></a></li>
                   <% } %>
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br />
                <ul class="list fixed-tablet-size">
                    <li class="row">
                        <div class="col"><asp:Literal ID="lblMode" runat="server" /></div>
                        <div class="col"><asp:Literal ID="txtMode" runat="server" /></div>
                    </li>
                    <li class="row">
                        <div class="col"><asp:Literal ID="lblMinMaxLimit" runat="server" /></div>
                        <div class="col"><asp:Literal ID="txtMinMaxLimit" runat="server" /></div>
                    </li>
                    <li class="row">
                        <div class="col"><asp:Literal ID="lblDailyLimit" runat="server" /></div>
                        <div class="col"><asp:Literal ID="txtDailyLimit" runat="server" /></div>
                    </li>
                    <li class="row">
                        <div class="col"><asp:Literal ID="lblTotalAllowed" runat="server" /></div>
                        <div class="col"><asp:Literal ID="txtTotalAllowed" runat="server" /></div>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" Text="from" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" placeholder="amount" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <a href="/Funds.aspx" role="button" class="ui-btn btn-bordered" ID="btnCancel" runat="server" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col"><asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" data-corners="false" OnClientClick="javascript:postsdpay();return false;" /></div>
                    </li>
                    <li class="item-textwrap">
                        <asp:Literal ID="lblNoticeDownload" runat="server" />
                    </li>
                    <li class="item-textwrap">
                        <a href="http://mobile.unionpay.com/getclient?platform=android&type=securepayplugin" target="_blank"><%=commonCulture.ElementValues.getResourceString("lblAndroidDownload", xeResources)%></a>
                    </li>
                    <li class="item-textwrap">
                        <a href="http://mobile.unionpay.com/getclient?platform=ios&type=securepayplugin" target="_blank"><%=commonCulture.ElementValues.getResourceString("lblIOSDownload", xeResources)%></a>
                    </li>
                    <asp:HiddenField runat="server" ID="_repostcheckcode" />
                </ul>

                <div class="row">
                    <div class="col">
                        <input type="button" data-theme="b" onclick="location.href = '/Withdrawal/Withrawal.aspx';" value="<%=commonCulture.ElementValues.getResourceString("withrawal", commonVariables.LeftMenuXML)%>" class="button-blue"  data-corners="false" />
                    </div>
                    <div class="col">
                         <input type="button" data-theme="b" onclick="location.href = '/FundTransfer/FundTransfer.aspx';" value="<%=commonCulture.ElementValues.getResourceString("fundTransfer", commonVariables.LeftMenuXML)%>" class="button-blue"  data-corners="false" />
                    </div>
                </div>

            </form>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            $(function () {
                window.history.forward();

                var strMethodsUnavailable = '<%=strMethodsUnAvailable%>';

                if (strMethodsUnavailable.length > 0) {
                    var arrMethodsUnavailable = strMethodsUnavailable.split('|');
                    for (var intCount = 0 ; intCount < arrMethodsUnavailable.length; intCount++) {
                        var strMethodId = arrMethodsUnavailable[intCount].toString();
                        if (strMethodId == '<%=Convert.ToInt32(commonVariables.DepositMethod.SDPay)%>') { document.location.assign('/Index'); }
                        $('#d' + strMethodId + ' > a').attr('href', 'javascript:void(0)').html('&nbsp;').click(false);
                    }
                }

                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case '-1':
                            alert('<%=strAlertMessage%>');
                            break;
                        case '0':
                            alert('<%=strAlertMessage%>');
                            window.location.replace('/Deposit/Default.aspx');
                            break;
                        default:
                            break;
                    }
                }
            });

            function postsdpay() {
                $('.div-submit').hide();

                if ($('#txtDepositAmount').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingDepositAmount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if (isNaN($('#txtDepositAmount').val())) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/InvalidDepositAmount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if (parseFloat($('#txtDepositAmount').val()) < parseFloat('<%=strMinAmount%>')) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/AmountMinLimit", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if (parseFloat($('#txtDepositAmount').val()) > parseFloat('<%=strMaxAmount%>')) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/AmountMaxLimit", xeErrors)%>');
                    e.preventDefault();
                    return;
                }

                var sdpayurl = '/_secure/ajaxhandlers/sdpay.ashx?v=' + new Date().getTime() + '&requestAmount=' + $('#txtDepositAmount').val();
                var w = window.open(sdpayurl);
            }
        </script>
    </div>
</body>
</html>
