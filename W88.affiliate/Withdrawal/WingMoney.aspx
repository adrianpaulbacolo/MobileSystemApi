<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WingMoney.aspx.cs" Inherits="Withdrawal_WingMoney" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="text/javascript" src="/_Static/JS/jquery.mask.min.js"></script>
    <link rel="stylesheet" type="text/css" href="/_Static/Css/Withdrawal.css" />
</head>
<body>    
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="a">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML))%></span></div>

            <div class="page-content">
                <div data-role="navbar">
                    <ul>
                        <li id="<%=string.Format("w{0}", commonVariables.WithdrawalMethod.BankTransfer)%>"><a href="/Withdrawal/BankTransfer" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("banktransfer", commonVariables.LeftMenuXML)%></a></li>
                        <li id="<%=string.Format("w{0}", commonVariables.WithdrawalMethod.WingMoney)%>"><a href="/Withdrawal/WingMoney" data-ajax="false" class="ui-btn-active"><%=commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML)%></a></li>
                    </ul>
                    <br />
                </div>

                <form id="form1" runat="server" data-ajax="false">
                    <div class="div-content-wrapper">
                        <div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="lblWithdrawAmount" runat="server" AssociatedControlID="txtWithdrawAmount" Text="from" CssClass="ui-hidden-accessible" />
                                <asp:TextBox ID="txtWithdrawAmount" runat="server" placeholder="amount" type="number" step="any" min="1" />
                            </div>
                        </div>
                        <div>
                            <div class="ui-field-contain div-limit"><div><asp:Literal ID="lblDailyLimit" runat="server" /></div><div>&nbsp;</div><div><asp:Literal ID="lblTotalAllowed" runat="server" /></div></div>
                        </div>
                        <div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" Text="to" CssClass="ui-hidden-accessible" />
                                <asp:TextBox ID="txtAccountName" runat="server" placeholder="accountname" />
                            </div>
                        </div>
                        <div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" Text="to" CssClass="ui-hidden-accessible" />
                                <asp:TextBox ID="txtAccountNumber" runat="server" placeholder="accountnum" />
                            </div>
                        </div>
                        <!--
                        <div>                            
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="lblMobile" runat="server" AssociatedControlID="txtMobile" Text="to" CssClass="ui-hidden-accessible" />
                                <asp:TextBox ID="txtMobile" runat="server" placeholder="mykad" />
                            </div>
                        </div>
                        -->
                        <div>
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" OnClick="btnSubmit_Click" data-corners="false" />
                        </div>
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

                var strMethodsUnavailable = '<%=strMethodsUnAvailable%>';

                if (strMethodsUnavailable.length > 0) {
                    var arrMethodsUnavailable = strMethodsUnavailable.split('|');
                    for (var intCount = 0 ; intCount < arrMethodsUnavailable.length; intCount++) {
                        var strMethodId = arrMethodsUnavailable[intCount].toString();
                        if (strMethodId == '<%=Convert.ToInt32(commonVariables.WithdrawalMethod.WingMoney)%>') { document.location.assign('/Index'); }
                        $('#w' + strMethodId + ' > a').attr('href', 'javascript:void(0)').html('&nbsp;').click(false);
                    }
                }

                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case '-1':
                            alert('<%=strAlertMessage%>');
                            break;
                        case '0':
                            alert('<%=strAlertMessage%>');
                            window.location.replace('/Withdrawal/WingMoney');
                            break;
                        default:
                            break;
                    }
                }
            });

            $('#form1').submit(function (e) {

                if ($('#txtWithdrawAmount').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingWithdrawAmount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if (isNaN($('#txtWithdrawAmount').val()))
                {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingWithdrawAmount", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtReferenceId').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Deposit/MissingReferenceId", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtAccountName').val().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingAccountName", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#txtAccountNumber').val().length == 0) {
                    if ($('#drpBank').val() == 'VIETIN') {
                        //if ($('#txtAccountNumber').val().length != 16 || isNaN($('#txtAccountNumber').val())) {
                            alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingAccountNumber", xeErrors)%>');
                            e.preventDefault();
                            return;
                        //}
                    } else {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/MissingAccountNumber", xeErrors)%>');
                        e.preventDefault();
                        return;
                    }
                }
                GPINTMOBILE.ShowSplash();
            });
        </script>
    </div>
</body>
</html>