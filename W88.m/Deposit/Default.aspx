<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Deposit_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("fastdeposit", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link rel="stylesheet" type="text/css" href="/_Static/Css/Deposit.css" />
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="a">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("fastdeposit", commonVariables.LeftMenuXML))%></span></div>
            <div class="page-content">
                <div data-role="navbar">
                    <ul>
                      <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.FastDeposit))%>'><a class="ui-btn-active" href="/Deposit/FastDeposit" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("fastdeposit", commonVariables.LeftMenuXML)%></a></li>
                         <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "usd", true) == 0)
                          { %>
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.WingMoney))%>'><a href="/Deposit/WingMoney" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML)%></a></li>
                        <% } %>
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.SDPay))%>'><a href="/Deposit/SDPay" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("sdpay", commonVariables.LeftMenuXML)%></a></li>
                         <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "myr", true) == 0 || string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "thb", true) == 0)
                          { %>
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.Help2Pay))%>'><a href="/Deposit/Help2Pay" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("help2pay", commonVariables.LeftMenuXML)%></a></li>
                        <% } %>

                    </ul>
                    <br />
                </div>

                <form id="form1" runat="server" data-ajax="false">
                <div class="div-content-wrapper">
                    <div>
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" Text="from" CssClass="ui-hidden-accessible" />
                            <asp:TextBox ID="txtDepositAmount" runat="server" placeholder="amount" type="number" step="any" min="1" data-clear-btn="true" />
                        </div>
                    </div>
                    <div>
                        <div class="ui-field-contain div-limit"><div><asp:Literal ID="lblDailyLimit" runat="server" /></div><div>&nbsp;</div><div><asp:Literal ID="lblTotalAllowed" runat="server" /></div></div>
                    </div>
                    <div>
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" Text="from" CssClass="ui-hidden-accessible" />
                            <asp:TextBox ID="txtReferenceId" runat="server" placeholder="referenceid" data-clear-btn="true" />
                        </div>
                    </div>
                    <div>
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Label ID="lblSystemAccount" runat="server" AssociatedControlID="drpSystemAccount" Text="to" CssClass="ui-hidden-accessible" />
                            <asp:DropDownList ID="drpSystemAccount" runat="server" data-corners="false" />
                        </div>
                    </div>
                    <div id="divDepositDateTime" runat="server" class="div-fastdeposit-depositdatetime">
                        <div><asp:DropDownList ID="drpDepositDate" runat="server" /></div>
                        <div><asp:DropDownList ID="drpHour" runat="server"/></div>
                        <div><asp:DropDownList ID="drpMinute" runat="server"/></div>
                    </div>
                    <div>
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Label ID="lblDepositChannel" runat="server" AssociatedControlID="drpDepositChannel" Text="to" CssClass="ui-hidden-accessible" />
                            <asp:DropDownList ID="drpDepositChannel" runat="server" data-corners="false" />
                        </div>
                    </div>                        
                    <div>
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" Text="to" CssClass="ui-hidden-accessible" />
                            <asp:DropDownList ID="drpBank" runat="server" data-corners="false" />
                        </div>
                    </div>
                    <div id="divBankName" style="display:none;">
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" Text="other" CssClass="ui-hidden-accessible" />
                            <asp:TextBox ID="txtBankName" runat="server" placeholder="bankname" data-clear-btn="true" />
                        </div>
                    </div>
                    <div>
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" Text="to" CssClass="ui-hidden-accessible" />
                            <asp:TextBox ID="txtAccountName" runat="server" placeholder="accountname" data-clear-btn="true" />
                        </div>
                    </div>
                    <div>
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" Text="to" CssClass="ui-hidden-accessible" />
                            <asp:TextBox ID="txtAccountNumber" runat="server" placeholder="accountnum" data-clear-btn="true" />
                        </div>
                    </div>
                    <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" OnClick="btnSubmit_Click" data-corners="false" />
                    <asp:HiddenField runat="server" ID="_repostcheckcode" />
                </div>
                </form>
            </div>
        </div>
        <!-- /content -->
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
                        if (strMethodId == '<%=Convert.ToInt32(commonVariables.DepositMethod.FastDeposit)%>') { document.location.assign('/Index'); }
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

            $('#drpBank').change(function () {
                if ($(this).val() == 'OTHER') { $('#divBankName').show(); }
                else { $('#divBankName').hide(); }
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
                else if ($('#drpSystemAccount').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/SelectSystemAccount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#drpDepositChannel').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/SelectDepositChannel", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#drpBank').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/SelectBank", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#drpBank').val() == 'OTHER') {
                    if ($('#txtBankName').val().length == 0) {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingBankName", xeErrors)%>');
                        e.preventDefault();
                        return;
                    }
                }
                if ($('#txtAccountName').val().length == 0) {
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