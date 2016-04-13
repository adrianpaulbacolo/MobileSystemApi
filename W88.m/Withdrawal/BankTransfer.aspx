<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankTransfer.aspx.cs" Inherits="Withdrawal_BankTransfer" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wBankTransfer", commonVariables.PaymentMethodsXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="text/javascript" src="/_Static/JS/jquery.mask.min.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("withdrawal", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("wBankTransfer", commonVariables.PaymentMethodsXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>

            <div data-role="navbar">
                <ul id="withdrawalTabs" runat="server">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br />
                <ul class="list fixed-tablet-size">
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblMode" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtMode" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblMinMaxLimit" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtMinMaxLimit" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblDailyLimit" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtDailyLimit" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblTotalAllowed" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtTotalAllowed" runat="server" />
                        </div>
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblWithdrawAmount" runat="server" AssociatedControlID="txtWithdrawAmount" Text="from" />
                        <asp:TextBox ID="txtWithdrawAmount" runat="server" type="number" step="any" min="1" />
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblBank" runat="server" AssociatedControlID="drpBank" />
                        <asp:DropDownList ID="drpBank" runat="server" data-corners="false" />
                    </li>
                    <li class="item item-input" id="divBankName" style="display: none;">
                        <asp:Label ID="lblBankName" runat="server" AssociatedControlID="txtBankName" />
                        <asp:TextBox ID="txtBankName" runat="server" />
                    </li>
                    <li class="item item-input" id="divBankBranch" runat="server">
                        <asp:Label ID="lblBankBranch" runat="server" AssociatedControlID="txtBankBranch" />
                        <asp:TextBox ID="txtBankBranch" runat="server" />
                    </li>
                    <li class="item item-input" id="divAddress" runat="server">
                        <asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" />
                        <asp:TextBox ID="txtAddress" runat="server" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountName" runat="server" AssociatedControlID="txtAccountName" />
                        <asp:TextBox ID="txtAccountName" runat="server" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblAccountNumber" runat="server" AssociatedControlID="txtAccountNumber" />
                        <asp:TextBox ID="txtAccountNumber" runat="server" />
                    </li>
                    <%-- <% if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "myr", true) == 0)
                       { %>
                    <li class="item item-input">
                        <asp:Label ID="lblMyKad" runat="server" AssociatedControlID="txtMyKad" Text="to" />
                        <asp:TextBox ID="txtMyKad" runat="server" />
                    </li>
                    <% } %>--%>
                    <!--
                    <li class="item item-input">
                        <asp:Label ID="lblMobile" runat="server" AssociatedControlID="txtMobile" Text="to" />
                        <asp:TextBox ID="txtMobile" runat="server" />
                    </li>
                    -->
                    <li class="item row">
                        <div class="col">
                            <a href="/Funds.aspx" role="button" class="ui-btn btn-bordered" data-ajax="false"><%=base.strbtnCancel%></a>
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
            //$(document).ready(function () { });
            $(function () {
                window.history.forward();

                <% if (string.Compare(commonVariables.GetSessionVariable("CurrencyCode"), "myr", true) == 0)
                   { %>
                $('#txtMyKad').mask('999999-99-9999');
                <% } %>

                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case '-1':
                            alert('<%=strAlertMessage%>');

                            toogleBank($('#drpBank').val());

                            break;
                        case '0':
                            alert('<%=strAlertMessage%>');

                            window.location.replace('/Withdrawal/Default.aspx');
                            break;
                        default:
                            break;
                    }
                }
            });


            $('#drpBank').change(function () {
                toogleBank(this.value);
            });


            function toogleBank(bankId) {
                if (bankId == "OTHER") {
                    $('#divBankName').show();
                }
                else {
                    $('#divBankName').hide();
                }
            }

        </script>
    </div>
</body>
</html>
