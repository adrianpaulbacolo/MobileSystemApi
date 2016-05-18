<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FundTransfer.aspx.cs" Inherits="History_FundTransfer" %>
<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>

<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("fundtransfer", commonVariables.LeftMenuXML)%></title>
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
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("fundtransfer", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            
            <div class="wallet main-wallet">
                <uc:Wallet id="uMainWallet" runat="server" />
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <p>&nbsp;</p>
                <ul class="list fixed-tablet-size">
                    <li class="item item-select">
                        <asp:Label ID="lblDateFrom" runat="server" AssociatedControlID="txtDateFrom" Text="" />
                        <asp:TextBox ID="txtDateFrom"  type="date" runat="server"></asp:TextBox>
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblDateTo" runat="server" Text="" />
                        <asp:TextBox ID="txtDateTo" type="date" runat="server"></asp:TextBox>
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblType" runat="server" AssociatedControlID="ddlType" Text="" />
                        <asp:DropDownList ID="ddlType" runat="server" data-corners="false" >
                            <asp:ListItem Text="ALL" Value="-1" />
                            <asp:ListItem Text="MAIN" Value="0" />
                            <asp:ListItem Text="A-SPORTS" Value="2" />
                            <asp:ListItem Text="E-SPORTS" Value="7" />
                            <asp:ListItem Text="I-SPORTS" Value="9" />
                            <asp:ListItem Text="W-SPORTS" Value="13" />
                            <asp:ListItem Text="CLUB W, BRAVADO, APOLLO, CRESCENDO, DIVINO & MASSIMO, VIRTUAL" Value="3" />
                            <asp:ListItem Text="PALAZZO" Value="4" />
                            <asp:ListItem Text="NUOVO" Value="12" />
                            <asp:ListItem Text="LOTTERY" Value="1" />
                            <asp:ListItem Text="POKER (USD)" Value="6" />
                        </asp:DropDownList>
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblStatus" runat="server" AssociatedControlID="ddlStatus" Text="" />
                        <asp:DropDownList ID="ddlStatus" runat="server" data-corners="false">
                             <asp:ListItem Text="ALL" Value="-1" />
                            <asp:ListItem Text="PENDING" Value="0" />
                            <asp:ListItem Text="SUCCESSFUL" Value="1" />
                            <asp:ListItem Text="FAILED" Value="2" />
                            <asp:ListItem Text="DECLINED" Value="3" />
                        </asp:DropDownList>
                    </li>
                    <li class="item row">
                        <div class="col">
                            <a href="/Funds.aspx" role="button" class="ui-btn btn-bordered" ID="btnCancel" runat="server" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" Text="Submit" CssClass="button-blue" OnClick="btnSubmit_Click" />
                        </div>
                    </li>
                </ul>
            </form>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/navMenu.shtml" -->

        <script type="text/javascript">

            $('#<%= txtDateFrom.ClientID %>').on('focusout', function () {

                $('#<%= txtDateTo.ClientID %>').val("");
                $('#<%= txtDateTo.ClientID %>').attr("min", $('#<%= txtDateFrom.ClientID %>').val());

                var date = new Date($('#<%= txtDateFrom.ClientID %>').val());
                var maxDays = parseInt(90);
                date.setDate(date.getDate() + maxDays);

                var month = date.getMonth() + 1;
                var day = date.getDate();

                var maxDate = date.getFullYear() + '-' +
                    (month < 10 ? '0' : '') + month + '-' +
                    (day < 10 ? '0' : '') + day;

                $('#<%= txtDateTo.ClientID %>').attr("max", maxDate);
            });

            $('#form1').submit(function (e) {
                if ($('#<%= txtDateTo.ClientID %>').val().length == 0) {
                    alert('Please Select a valid End Date');
                    e.preventDefault();
                    return;
                }

                if ($('#<%= txtDateFrom.ClientID %>').val().length == 0) {
                    alert('Please Select a valid Start Date');
                    e.preventDefault();
                    return;
                }
                GPINTMOBILE.ShowSplash();
            });
        </script>
    </div>
</body>
</html>
