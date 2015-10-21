<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NextPay.aspx.cs" Inherits="Deposit_NextPay" %>
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
        .auto-style3 {
            width: 277px;
        }
    </style>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="a">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span>DEPOSIT - NEXTPAY</span></div>
            <div class="page-content">
                <div data-role="navbar">
                    <ul>
                       <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.FastDeposit))%>'><a href="/Deposit/FastDeposit" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("fastdeposit", commonVariables.LeftMenuXML)%></a></li>
                         <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "usd", true) == 0)
                          { %>
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.WingMoney))%>'><a href="/Deposit/WingMoney" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("wingmoney", commonVariables.LeftMenuXML)%></a></li>
                        <% } %>
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.SDPay))%>'><a  href="/Deposit/SDPay" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("sdpay", commonVariables.LeftMenuXML)%></a></li>
                         <%if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "myr", true) == 0 || string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "thb", true) == 0)
                          { %>
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.Help2Pay))%>'><a href="/Deposit/Help2Pay" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("help2pay", commonVariables.LeftMenuXML)%></a></li>
                        <% } %> 
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.DaddyPay))%>'><a runat="server" id="daddyPay_link" href="/Deposit/DaddyPay.aspx?value=1" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("daddypay", commonVariables.LeftMenuXML)%></a></li>
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.DaddyPayQR))%>'><a runat="server" id="daddyPayQR_link" href="/Deposit/DaddyPay.aspx?value=2" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("daddypayqr", commonVariables.LeftMenuXML)%></a></li> 
                        <li id='<%=string.Format("d{0}", Convert.ToInt32(commonVariables.DepositMethod.NextPay))%>'><a class="ui-btn-active" runat="server" id="nextPay_link" href="/Deposit/NextPay.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("nextpay", commonVariables.LeftMenuXML)%></a></li>     

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
                    <div>

                    </div>
                    <br />
                    <div>
                            <asp:TextBox ID="amount_txt" runat="server" placeholder="Deposit Amount" type="number" step="any" min="1" data-clear-btn="true" />
                    </div>
                    <div>
                        <div class="ui-field-contain ui-hide-label">
                         <asp:DropDownList ID="bankDropDownList"  runat="server">
                             <asp:ListItem>SELECT BANK</asp:ListItem>
                         </asp:DropDownList>
                            <asp:TextBox ID="accountName_txt" runat="server" placeholder="Account Name" data-clear-btn="true" />
                            <asp:TextBox ID="account_txt" runat="server" placeholder="Account" data-clear-btn="true" />
                        </div>
                    </div>
                    <div class="div-submit">
                        <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" />
                    </div>
                    <div>&nbsp;</div>
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
        </script>
    </div>
</body>
</html>