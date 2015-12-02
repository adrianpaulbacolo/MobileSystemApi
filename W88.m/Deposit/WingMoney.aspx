<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WingMoney.aspx.cs" Inherits="Deposit_WingMoney" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML))%></title>
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
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div data-role="navbar">
                <ul>
                    <li id="<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.FastDeposit))%>"><a href="/Deposit/FastDeposit" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("fastdeposit", commonVariables.LeftMenuXML)%></a></li>
                     <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "usd", true) == 0)
                      { %>
                    <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.WingMoney))%>'><a class="ui-btn-active" href="/Deposit/WingMoney" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML)%></a></li>
                    <% } %>
                    <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.SDPay))%>'><a href="/Deposit/SDPay" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("sdpay", commonVariables.LeftMenuXML)%></a></li>
                     <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "myr", true) == 0 || string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "thb", true) == 0)
                      { %>
                    <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.Help2Pay))%>'><a href="/Deposit/Help2Pay" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("help2pay", commonVariables.LeftMenuXML)%></a></li>
                    <% } %>
                    <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "rmb", true) == 0)
                    { %>
                    <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.DaddyPay))%>'><a runat="server" id="daddyPay_link" href="/Deposit/DaddyPay.aspx?value=1" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("daddypay", commonVariables.LeftMenuXML)%></a></li>
                        <%--<li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.DaddyPayQR))%>'><a runat="server" id="daddyPayQR_link" href="/Deposit/DaddyPay.aspx?value=2" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("daddypayqr", commonVariables.LeftMenuXML)%></a></li>  --%>
                    <% } %>
                    <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "thb", true) == 0)
                    { %>
                    <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.NextPay))%>'><a href="/Deposit/NextPay.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("nextpay", commonVariables.LeftMenuXML)%></a></li>
                   <% } %>
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <ul class="list fixed-tablet-size">
                    <li class="item item-input">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" Text="from" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>
                    <li class="item">
                        <div class="div-limit">
                            <div><asp:Literal ID="lblDailyLimit" runat="server" /></div>
                            <div><asp:Literal ID="lblTotalAllowed" runat="server" /></div>
                        </div>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" Text="from" />
                        <asp:TextBox ID="txtReferenceId" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-select" id="divDepositDateTime" runat="server">
                        <div class="row">
                            <div class="col"><asp:DropDownList ID="drpDepositDate" runat="server" /></div>
                            <div class="col"><asp:DropDownList ID="drpHour" runat="server" /></div>
                            <div class="col"><asp:DropDownList ID="drpMinute" runat="server" /></div>
                        </div>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" Text="to" />
                        <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" Text="to" />
                        <asp:TextBox ID="txtAccountNumber" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <a href="/Index" role="button" class="ui-btn btn-bordered" ID="btnCancel" runat="server" data-ajax="false">Cancel</a>
                        </div>
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" OnClick="btnSubmit_Click" />
                        </div>
                    </li>
                    <asp:HiddenField runat="server" ID="_repostcheckcode" />
                </ul>
            </form>
        </div>

        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            $(function () {
                window.history.forward();

                var strMethodsUnavailable = '<%=strMethodsUnAvailable%>';

                if (strMethodsUnavailable.length > 0) {
                    var arrMethodsUnavailable = strMethodsUnavailable.split('|');
                    for (var intCount = 0 ; intCount < arrMethodsUnavailable.length; intCount++) {
                        var strMethodId = arrMethodsUnavailable[intCount].toString();
                        if (strMethodId == '<%=Convert.ToInt32(commonVariables.DepositMethod.WingMoney)%>') { document.location.assign('/Index'); }
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
                            window.location.replace('/Deposit/WingMoney');
                            break;
                        default:
                            break;
                    }
                }
            });

            $('#form1').submit(function (e) {
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
                else if ($('#txtReferenceId').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingReferenceId", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtAccountName').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingAccountName", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtAccountNumber').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingAccountNumber", xeErrors)%>');
                        e.preventDefault();
                        return;
                    }
                GPINTMOBILE.ShowSplash();
            });
        </script>
    </div>
</body>
</html>
