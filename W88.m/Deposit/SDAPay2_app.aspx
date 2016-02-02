<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SDAPay2.aspx.cs" Inherits="Deposit_SDAPay2" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("sdapay", commonVariables.LeftMenuXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("sdapay", commonVariables.LeftMenuXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>

            <div data-role="navbar">
                <ul id="depositTabs" runat="server">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br />
                <ul class="list fixed-tablet-size">
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblStatus" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtStatus" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblTransactionId" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtTransactionId" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblAmount" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtAmount" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Label ID="lblAmountNote" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblBankName" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtBankName" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblBankHolderName" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtBankHolderName" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblBankAccountNo" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtBankAccountNo" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="login" CssClass="button-blue" data-corners="false" />
                        </div>
                    </li>
                </ul>

                <div class="row">
                    <div class="col">
                        <input type="button" data-theme="b" onclick="location.href = '/Withdrawal/Withrawal.aspx?source=app';" value="<%=commonCulture.ElementValues.getResourceString("withrawal", commonVariables.LeftMenuXML)%>" class="button-blue" data-corners="false" />
                    </div>
                    <div class="col">
                        <input type="button" data-theme="b" onclick="location.href = '/FundTransfer/FundTransfer.aspx';" value="<%=commonCulture.ElementValues.getResourceString("fundTransfer", commonVariables.LeftMenuXML)%>" class="button-blue" data-corners="false" />
                    </div>
                </div>

            </form>
        </div>

        <script type="text/javascript">
            $(function () {
                window.history.forward();

                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case '-1':
                            alert('<%=strAlertMessage%>');
                            break;
                        case '0':
                            alert('<%=strAlertMessage%>');

                            window.location.replace('/FundTransfer/Default.aspx');

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
