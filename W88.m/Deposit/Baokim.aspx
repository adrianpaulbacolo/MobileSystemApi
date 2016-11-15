<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Baokim.aspx.cs" Inherits="Deposit_Baokim" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dbaokim", commonVariables.PaymentMethodsXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <% if (commonCookie.CookieIsApp != "1")
               { %>
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <% } %>

            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dbaokim", commonVariables.PaymentMethodsXML))%></h1>
        </header>


        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet ID="uMainWallet" runat="server" />
            </div>

            <div data-role="navbar">
                <ul id="depositTabs" runat="server">
                </ul>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br>
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
                    <li class="item row selection">
                        <div class="col col-50">
                            <label>
                                <input type="radio" name="depOption" value="EWALLET" />Ví Điện Tử Nội Địa</label>
                        </div>
                        <div class="col col-50">
                            <label>
                                <input type="radio" name="depOption" value="ATM" />Thẻ ATM Nội Địa</label>
                        </div>
                    </li>
                    <li class="item item-input atm ewallet">
                        <asp:Label ID="lblDepositAmount" runat="server" AssociatedControlID="txtDepositAmount" />
                        <asp:TextBox ID="txtDepositAmount" runat="server" type="number" step="any" min="1" data-clear-btn="true" />
                    </li>
                    <li class="item item-select atm" runat="server">
                        <asp:Label ID="lblBanks" runat="server" AssociatedControlID="drpBanks" />
                        <asp:DropDownList ID="drpBanks" runat="server" data-corners="false">
                        </asp:DropDownList>
                    </li>
                    <li class="item item-input atm ewallet">
                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" />
                        <asp:TextBox ID="txtEmail" runat="server" data-mini="true" type="email" data-clear-btn="true" />
                    </li>
                    <li class="item item-select atm">
                        <asp:Label ID="lblContact" runat="server" AssociatedControlID="txtContact" />
                        <asp:TextBox ID="txtContact" runat="server" type="tel" data-mini="true" data-clear-btn="true" />
                    </li>
                    <li class="item item-select">
                        <p id="notice" style="color: #ff0000"></p>
                    </li>
                    <li class="item row ewallet atm">
                        <div class="col">
                            <asp:Button data-theme="b" ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" CausesValidation="False" />
                        </div>
                    </li>
                </ul>
            </form>
        </div>

        <% if (commonCookie.CookieIsApp != "1")
           { %>
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <% } %>

        <script type="text/javascript">
            var selectName = '<%=strdrpBank%>';

            $(document).ready(function () {
                //window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');

                window.w88Mobile.Gateways.Baokim.getBanks(selectName);
                window.w88Mobile.Gateways.Baokim.getTranslations();

                $('.ewallet').hide();
                $('.atm').hide();

                $("input[name='depOption']").change(function (e) {
                    e.preventDefault();
                    var value = $(this).val();

                    if (value == "EWALLET") {
                        $('.atm').hide();
                        $('.ewallet').show();
                        $('#btnEwallet').hide();
                        window.w88Mobile.Gateways.Baokim.method = "EWALLET";
                        $("#notice").html(sessionStorage.getItem("noticeWallet"));
                    } else {
                        $('.ewallet').hide();
                        $('.atm').show();
                        $(this).hide();
                        window.w88Mobile.Gateways.Baokim.method = "";
                        $("#notice").html(sessionStorage.getItem("noticeAtm"));
                    }
                });

                $('#btnSubmit').click(function (e) {
                    e.preventDefault();
                    var data;

                    if (window.w88Mobile.Gateways.Baokim.method == "EWALLET") {
                        data = { Method: window.w88Mobile.Gateways.Baokim.method, Amount: $('#txtDepositAmount').val(), Email: $('#txtEmail').val(), ThankYouPage: location.protocol + "//" + location.host + "/Thankyou.aspx" };
                    } else {
                        data = { Method: window.w88Mobile.Gateways.Baokim.method, Amount: $('#txtDepositAmount').val(), Email: $('#txtEmail').val(), Phone: $('#txtContact').val(), Bank: { Text: $('#drpBanks option:selected').text(), Value: $('#drpBanks').val(), ThankYouPage: location.protocol + "//" + location.host + "/Thankyou.aspx" } };
                    }

                    window.w88Mobile.Gateways.Baokim.deposit(data, function (response) {
                        switch (response.ResponseCode) {
                            case 1:
                                window.w88Mobile.FormValidator.enableSubmitButton('#btnSubmit');

                                window.open(response.ResponseData.PostUrl, '_blank');
                                break;
                            default:
                                w88Mobile.Growl.shout(response.ResponseMessage);
                                break;
                        }
                    });
                });
            });
        </script>

    </div>
</body>
</html>
