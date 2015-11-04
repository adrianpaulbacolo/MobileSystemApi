<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WingMoney_app.aspx.cs" Inherits="Deposit_WingMoney" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link rel="stylesheet" type="text/css" href="/_Static/Css/Deposit.css" />
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="a">
        <div class="ui-content" role="main">
            <div class="div-page-header"><span>Wallet Transfer</span></div>
            <div class="page-content">
                <div data-role="navbar">
                    <ul>
                        <li id="<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.FastDeposit))%>"><a href="/Deposit/Default_app.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("fastdeposit", commonVariables.LeftMenuXML)%></a></li>
                         <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "usd", true) == 0)
                          { %>
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.WingMoney))%>'><a class="ui-btn-active" href="/Deposit/WingMoney_app.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML)%></a></li>
                        <% } %>
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.SDPay))%>'><a href="/Deposit/SDPay_app.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("sdpay", commonVariables.LeftMenuXML)%></a></li>
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
                            <div class="ui-field-contain div-limit">
                                <div>
                                    <asp:Literal ID="lblDailyLimit" runat="server" />
                                </div>
                                <div>&nbsp;</div>
                                <div>
                                    <asp:Literal ID="lblTotalAllowed" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="ui-field-contain ui-hide-label">
                                <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" Text="from" CssClass="ui-hidden-accessible" />
                                <asp:TextBox ID="txtReferenceId" runat="server" placeholder="referenceid" data-clear-btn="true" />
                            </div>
                        </div>
                        <div id="divDepositDateTime" runat="server" class="ui-field-contain ui-grid-b">
                            <div class="ui-block-a">
                                <asp:DropDownList ID="drpDepositDate" runat="server" />
                            </div>
                            <div class="ui-block-b">
                                <asp:DropDownList ID="drpHour" runat="server" />
                            </div>
                            <div class="ui-block-c">
                                <asp:DropDownList ID="drpMinute" runat="server" />
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
                   <br />
                    <div>
                        <table style="width:100%">
                            <tr>
                                <td style="width:50%">
                                    <input type="button" onclick="location.href = '/Withdrawal/Withrawal.aspx';" value="<%=commonCulture.ElementValues.getResourceString("withrawal", commonVariables.LeftMenuXML)%>" class="button-blue"  data-corners="false" />
                                </td>
                                <td style="width:50%">
                                     <input type="button" onclick="location.href = '/FundTransfer/FundTransfer.aspx';" value="<%=commonCulture.ElementValues.getResourceString("fundTransfer", commonVariables.LeftMenuXML)%>" class="button-blue"  data-corners="false" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </form>
            </div>
        </div>
        <!-- /content -->
        <script type="text/javascript">
            $(function () {
                window.history.forward();

                var strMethodsUnavailable = '<%=strMethodsUnAvailable%>';

                if (strMethodsUnavailable.length > 0) {
                    var arrMethodsUnavailable = strMethodsUnavailable.split('|');
                    for (var intCount = 0 ; intCount < arrMethodsUnavailable.length; intCount++) {
                        var strMethodId = arrMethodsUnavailable[intCount].toString();
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
                            window.location.replace('/Deposit/WingMoney_app');
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
