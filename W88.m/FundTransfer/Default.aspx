<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="FundTransfer_Default" %>
<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("transfer", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <div data-role="page">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("transfer", commonVariables.LeftMenuXML)%></h1>
        </header>
        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
               <uc:Wallet id="uMainWallet" runat="server" />
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <ul class="list fixed-tablet-size">
                    <li class="item" runat="server" id="divBalance" data-role="collapsible">
                    </li>
                    <li class="item item-select">
                        <asp:Label ID="lblTransferFrom" runat="server" AssociatedControlID="drpTransferFrom" Text="from" />
                        <asp:DropDownList ID="drpTransferFrom" runat="server" data-corners="false" />
                    </li>
                    <li class="btn-swap ion-arrow-swap">
                        <asp:Button ID="btnSwap" runat="server" Text="Swap Wallets" OnClick="btnSwap_Click"  />
                    </li>
                    <%--<div><a href="javascript:void(0)" onclick="javascript:switchWallets();">switch</a></div>--%>
                    <li class="item item-select">
                        <asp:Label ID="lblTransferTo" runat="server" AssociatedControlID="drpTransferTo" Text="to" />
                        <asp:DropDownList ID="drpTransferTo" runat="server" data-corners="false" />
                    </li>
                    <li class="item item-input">
                        <asp:Label ID="lblTransferAmount" runat="server" AssociatedControlID="txtPromoCode" Text="amount" />
                        <asp:TextBox ID="txtTransferAmount" runat="server" placeholder="amount" type="number" step="any" min="1" />
                    </li>
                    <li class="item item-input">
                        <asp:Literal ID="litExchangeRate" runat="server" />
                    </li>
                    <li class="item item-input" id="divPromoCode">
                        <div class="ui-field-contain ui-hide-label">
                            <asp:Label ID="lblPromoCode" runat="server" AssociatedControlID="txtPromoCode" Text="code" CssClass="ui-hidden-accessible" />
                            <asp:TextBox ID="txtPromoCode" runat="server" placeholder="code" />
                        </div>
                    </li>
                    <li class="item item-input">
                        <span id="litPromoDetails" />
                    </li>
                    <li class="item row">
                        <div class="col">
                            <a href="/Funds.aspx" role="button" class="ui-btn btn-bordered" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col">
                            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
                        </div>
                    </li>
                </ul>
            </form>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/navMenu.shtml" -->

        <script type="text/javascript">

            $('#form1').submit(function (e) {

                if ($('#drpTransferFrom').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("/FundTransfer/SelectTransferFrom", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#drpTransferTo').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("/FundTransfer/SelectTransferTo", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                else if ($('#drpTransferFrom').val() == $('#drpTransferTo').val()) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("/FundTransfer/InvalidFundTransfer", xeErrors)%>');
                    e.preventDefault();
                    return;
                }
                setTimeout(function () {
                    $('#btnSubmit').prop("disabled", true);
                }, 0);
                GPINTMOBILE.ShowSplash();
            });

            function switchWallets() {
                var valTransferFrom = $('#drpTransferFrom').val();
                $('#drpTransferFrom').val($('#drpTransferTo').val()).change();
                $('#drpTransferTo').val(valTransferFrom).change();
            }

            $(function () {

                window.history.forward();

                $('#drpTransferFrom').change(function () {

                    $('#drpTransferTo option').each(function () {
                        if ($(this).val() != '-1') {
                            if ($(this).val() == $('#drpTransferFrom').val()) { $(this).hide(); $('#drpTransferTo').val('-1').change(); }
                            else { $(this).show(); }
                        }
                    });

                    if ($('#drpTransferFrom').val() == '6') {
                        $('#txtTransferAmount').attr('placeholder', ($('#txtTransferAmount').attr('placeholder').replace('(<%=commonVariables.GetSessionVariable("CurrencyCode")%>)', '(USD)')));
                    } else {
                        $('#txtTransferAmount').attr('placeholder', ($('#txtTransferAmount').attr('placeholder').replace('(USD)', '(<%=commonVariables.GetSessionVariable("CurrencyCode")%>)')));
                    }

                    if ($('#drpTransferFrom').val() == '0') { $('#divPromoCode').show(); }
                    else { $('#divPromoCode').hide(); }
                });

                $('#txtPromoCode').on('input', function () {
                   var strCode = $('#txtPromoCode').val();
                   if (parseInt($('#drpTransferFrom').val()) == 0 && parseInt($('#drpTransferTo').val()) > 0 && strCode.length > 0) {
                       var strWallet = $('#drpTransferTo').val();
                       var strAmount = $('#txtTransferAmount').val();

                       $.ajax({
                           type: 'POST',
                           url: '/AjaxHandlers/CheckPromo.ashx',
                           data: { Wallet: strWallet, Amount: strAmount, Code: strCode },
                           //dataType: "text/xml",
                           success: function (xml) {
                               var strStatusCode = $(xml).find('statusCode').text();
                               var strBonus = $(xml).find('bonusAmount').text();
                               var strRollover = $(xml).find('rolloverAmount').text();
                               var strMin = $(xml).find('minTransferAmount').text();

                               switch (strStatusCode) {
                                   case '00':
                                       $('#litPromoDetails').text('<%=commonCulture.ElementValues.getResourceXPathString("/FundTransfer/BonusAmount", xeErrors)%>' + strBonus);
                                       break;
                                   case '103':
                                       $('#litPromoDetails').text('<%=commonCulture.ElementValues.getResourceXPathString("/FundTransfer/RolloverNotMet", xeErrors)%>');
                                       break;
                                   case '109':
                                       $('#litPromoDetails').text('<%=commonCulture.ElementValues.getResourceXPathString("/Promotion/PromoAlreadyClaimed", xeErrors)%>');
                                       break;
                                   case '100':
                                   case '101':
                                   case '102':
                                   case '104':
                                   case '105':
                                   case '106':
                                   case '107':
                                   case '108':
                                       $('#litPromoDetails').text('<%=commonCulture.ElementValues.getResourceXPathString("/Promotion/InvalidPromo", xeErrors)%>');
                                       break;
                               }
                           }
                       });
                   } else { $('#litPromoDetails').text(''); }
                });

                var responseCode = '<%=strAlertCode%>';
                var responseMsg = '<%=strAlertMessage%>';

                if (responseMsg.length > 0) { alert(responseMsg.split('[break]').join('\n')); }
                if (responseCode == "-1") {
                    window.location.replace('/default.aspx');
                }
            });

            function hBalanceToggle(obj, strShow, strHide) {
                if ($(obj).hasClass('ui-collapsible-heading-collapsed')) {
                    $(obj).find(".ui-btn").text(strHide);
                    getBalance();
                } else {
                    $(obj).find(".ui-btn").text(strShow);
                }
            }

            function getBalance()
            {
                $(document).ready(function () {
                    $('span[name="WalletBalance"]').each(function () {
                        var objWallet = $(this);
                        var strWallet = objWallet.attr('id');
                        $.ajax({
                            type: "POST",
                            url: '/AjaxHandlers/GetBalance.ashx',
                            data: { Wallet: strWallet },
                            success: function (html) {
                                objWallet.text(html);
                            }
                        });
                    });
                });
            }
        </script>
    </div>
</body>
</html>
