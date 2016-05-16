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
                    <li class="item item-input">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" Text="from" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>
                    <li class="item item-text-wrap">
                        <div class="div-limit">
                            <div>
                                <asp:Literal ID="lblDailyLimit" runat="server" />
                            </div>
                            <div>
                                <asp:Literal ID="lblTotalAllowed" runat="server" />
                            </div>
                        </div>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblReferenceId" runat="server" AssociatedControlID="txtReferenceId" Text="from" />
                        <asp:TextBox ID="txtReferenceId" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-select" id="divDepositDateTime" runat="server">
                        <div class="row">
                            <div class="col">
                                <asp:DropDownList ID="drpDepositDate" runat="server" />
                            </div>
                            <div class="col">
                                <asp:DropDownList ID="drpHour" runat="server" />
                            </div>
                            <div class="col">
                                <asp:DropDownList ID="drpMinute" runat="server" />
                            </div>
                        </div>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
                        <asp:TextBox ID="txtAccountName" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
                        <asp:TextBox ID="txtAccountNumber" runat="server" data-clear-btn="true" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <a href="/Funds.aspx" role="button" class="ui-btn btn-bordered" id="btnCancel" runat="server" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" OnClick="btnSubmit_Click" />
                        </div>
                    </li>
                </ul>
            </form>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            $('#form1').submit(function (e) {
                window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');
            });
            $(function () {
                window.history.forward();

                var responseCode = '<%=strAlertCode%>';
                var responseMsg = '<%=strAlertMessage%>';
                if (responseCode.length > 0) {
                    switch (responseCode) {
                        case '-1':
                            alert(responseMsg);
                            break;
                        case '0':
                            alert(responseMsg);
                            window.location.replace('/Deposit/WingMoney.aspx');
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
