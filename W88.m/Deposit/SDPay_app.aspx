<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SDPay_app.aspx.cs" Inherits="Deposit_SDPay" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("sdpay", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link rel="stylesheet" type="text/css" href="/_Static/Css/Deposit.css" />
    <style type="text/css">
        .div-content-wrapper {
            margin-bottom:1em;
        }
        .div-content-wrapper > div {
            table-layout:fixed;
            display:table;
            width:75%;
            margin:auto;
        }
        .div-content-wrapper > div > div {
            display:table-cell;
        }
    </style>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="a">
        <div class="ui-content" role="main">
            <div class="div-page-header"><span>Wallet Transfer</span></div>
            <div class="page-content">
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
                    <br />
                </div>

                <form id="form1" runat="server" data-ajax="false">
                <div class="div-content-wrapper">
                    <div>
                        <div><asp:Literal ID="lblMode" runat="server" /></div>
                        <div><asp:Literal ID="txtMode" runat="server" /></div>
                    </div>
                    <div>
                        <div><asp:Literal ID="lblMinMaxLimit" runat="server" /></div>
                        <div><asp:Literal ID="txtMinMaxLimit" runat="server" /></div>
                    </div>
                    <div>
                        <div><asp:Literal ID="lblDailyLimit" runat="server" /></div>
                        <div><asp:Literal ID="txtDailyLimit" runat="server" /></div>
                    </div>
                    <div>
                        <div><asp:Literal ID="lblTotalAllowed" runat="server" /></div>
                        <div><asp:Literal ID="txtTotalAllowed" runat="server" /></div>
                    </div>
                    <div>&nbsp;</div>
                    <div>
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" Text="from" CssClass="ui-hidden-accessible" />
                            <asp:TextBox ID="txtDepositAmount" runat="server" placeholder="amount" type="number" step="any" min="1" data-clear-btn="true" />
                        </div>
                    </div>
                    <div>&nbsp;</div>
                    <div class="div-submit">
                        <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" data-corners="false" OnClientClick="javascript:postsdpay();return false;" />
                    </div>
                    <div>&nbsp;</div>
                    <div>
                        <div><asp:Literal ID="lblNoticeDownload" runat="server" /></div>
                    </div>
                    <div>&nbsp;</div>
                    <div>
                        <a href="http://mobile.unionpay.com/getclient?platform=android&type=securepayplugin"><%=commonCulture.ElementValues.getResourceString("lblAndroidDownload", xeResources)%></a>
                    </div>
                    <div>&nbsp;</div>
                    <div>
                        <a href="http://mobile.unionpay.com/getclient?platform=ios&type=securepayplugin"><%=commonCulture.ElementValues.getResourceString("lblIOSDownload", xeResources)%></a>
                    </div>
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