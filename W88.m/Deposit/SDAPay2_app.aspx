<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SDAPay2.aspx.cs" Inherits="Deposit_SDAPay2" %>

<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>
<%@ Register TagPrefix="uc" TagName="AppFooterMenu" Src="~/UserControls/AppFooterMenu.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dSDAPayAlipay", commonVariables.PaymentMethodsXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dSDAPayAlipay", commonVariables.PaymentMethodsXML))%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet ID="uMainWallet" runat="server" />
            </div>

            <div data-role="navbar" id="depositTabs" runat="server">
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br />
                <ul class="list fixed-tablet-size">
                    <li class="row">
                        <div class="col col-40">
                            <asp:Literal ID="lblStatus" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtStatus" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col col-40">
                            <asp:Literal ID="lblTransactionId" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtTransactionId" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col col-40">
                            <asp:Literal ID="lblAmount" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Label ID="txtAmount" runat="server" />
                        </div>
                        <div class="col col-20">
                            <a href="#" class="ui-btn btn-small btn-bordered" id="copyAmount"><span class="icon ion-ios-copy-outline"></span> <%=commonCulture.ElementValues.getResourceString("copy", commonVariables.LeftMenuXML)%></a>
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <h5 style="font-style: italic">
                                <asp:Label ID="lblAmountNote" runat="server" /></h5>
                        </div>
                    </li>
                    <li class="row">
                        <div class="col col-40">
                            <asp:Literal ID="lblBankName" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Literal ID="txtBankName" runat="server" />
                        </div>
                    </li>
                    <li class="row">
                        <div class="col col-40">
                            <asp:Literal ID="lblBankHolderName" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Label ID="txtBankHolderName" runat="server" />
                        </div>
                        <div class="col col-20">
                            <a href="#" class="ui-btn btn-small btn-bordered" id="copyAccountName"><span class="icon ion-ios-copy-outline"></span> <%=commonCulture.ElementValues.getResourceString("copy", commonVariables.LeftMenuXML)%></a>
                        </div>
                    </li>
                    <li class="row">
                        <div class="col col-40">
                            <asp:Literal ID="lblBankAccountNo" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Label ID="txtBankAccountNo" runat="server" class="with-colon"/>
                        </div>
                        <div class="col col-20">
                            <a href="#" class="ui-btn btn-small btn-bordered" id="copyAccountNo"><span class="icon ion-ios-copy-outline"></span> <%=commonCulture.ElementValues.getResourceString("copy", commonVariables.LeftMenuXML)%></a>
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:HyperLink ID="btnSubmit" runat="server" CssClass="ui-btn btn-primary" data-corners="false" Target="_blank" />
                        </div>
                    </li>
                </ul>

                <uc:AppFooterMenu runat="server" ID="AppFooterMenu" />

            </form>
        </div>

        <script type="text/javascript">
            $('#form1').submit(function (e) {
                window.w88Mobile.FormValidator.disableSubmitButton('#btnSubmit');
            });
            $(function () {
                window.history.forward();

                if ($('#depositTabs li').length == 0) {
                    window.location.reload();
                }

                var responseCode = '<%=strAlertCode%>';
                var responseMsg = '<%=strAlertMessage%>';
                if (responseCode.length > 0) {
                    switch (responseCode) {
                        case '-1':
                            alert(responseMsg);
                            window.location.replace('SDAPay_app.aspx');
                            break;
                        case '0':
                            break;
                        default:
                            break;
                    }
                }
                
                (function (a) { (jQuery.browser = jQuery.browser || {}).ios = /ip(hone|od|ad)/i.test(a) })(navigator.userAgent || navigator.vendor || window.opera);

                if ($.browser.ios) {
                    $(".col-20").hide();
                }
                else {
                    $(".col-20").show();
                }

                $('#copyAmount').on('click', function () {
                    var amount = $("#txtAmount").text().slice(2); //this will removed the ": "
                    copyToClipboard(amount)
                });

                $('#copyAccountName').on('click', function () {
                    var accountName = $("#txtBankHolderName").text().slice(2); //this will removed the ": "
                    copyToClipboard(accountName)
                });

                $('#copyAccountNo').on('click', function () {
                    var accountNo = $("#txtBankAccountNo").text().slice(2); //this will removed the ": "
                    copyToClipboard(accountNo)
                });

                function copyToClipboard(text) {
                    var input = document.createElement('textarea', { "permissions": ["clipboardWrite"] });
                    document.body.appendChild(input);
                    input.value = text;
                    input.focus();
                    input.select();

                    var s = document.execCommand('copy', false, null);
                    window.w88Mobile.Growl.shout(s == true ? "<%=commonCulture.ElementValues.getResourceString("copied", commonVariables.LeftMenuXML)%>" : "Unable to Copy");

                    input.remove();
                }

                var intervalId = setInterval(function () {
                    $.ajax({
                        dataType: "json",
                        url: "SDAPay2.aspx/CheckDeposit?strTransactionId=" + '<%=strTransactionId%>',
                        contentType: "application/json;",
                        type: "GET",
                        cache: false,
                        success: function (data) {
                            var result = data.d;
                            $('#txtStatus').text(": " + result);
                            if (result.indexOf("Successful") == 0) {
                                clearInterval(intervalId);
                                $('#btnSubmit').hide();

                                setTimeout(function () {
                                    window.location.replace('/FundTransfer/Default_app.aspx');
                                }, 2000);

                            } else if (result.indexOf("Failed") == 0) {
                                clearInterval(intervalId);
                                $('#btnSubmit').hide();
                            }
                        },
                        error: function (err) {
                            clearInterval(intervalId);
                        }
                    });
                }, 5000);

            });

        </script>
    </div>
</body>
</html>
