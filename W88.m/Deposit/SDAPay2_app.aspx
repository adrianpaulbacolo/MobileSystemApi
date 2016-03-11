﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SDAPay2.aspx.cs" Inherits="Deposit_SDAPay2" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dSDAPayAlipay", commonVariables.PaymentMethodsXML))%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <h1 class="title"><%=string.Format("{0} - {1}", commonCulture.ElementValues.getResourceString("deposit", commonVariables.LeftMenuXML), commonCulture.ElementValues.getResourceString("dSDAPayAlipay", commonVariables.PaymentMethodsXML))%></h1>
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
                            <asp:Label ID="txtBankHolderName" runat="server" />
                            <a href="#" id="copyAccountName" hidden><%=commonCulture.ElementValues.getResourceString("copy", commonVariables.LeftMenuXML)%></a>
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:Literal ID="lblBankAccountNo" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Label ID="txtBankAccountNo" runat="server" /> 
                            <a href="#" id="copyAccountNo" hidden><%=commonCulture.ElementValues.getResourceString("copy", commonVariables.LeftMenuXML)%></a>
                        </div>
                    </li>
                    <li class="row">
                        <div class="col">
                            <asp:HyperLink ID="btnSubmit" runat="server" CssClass="ui-btn btn-primary" data-corners="false" Target="_blank"/>
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
                        case "0":
                            break;
                        case '-1':
                            alert('<%=strAlertMessage%>');

                            window.location.replace('SDAPay.aspx')
                            break;
                        default:
                            break;
                    }
                }

                $('#copyAccountName').on('click', function () {
                    var accountName = $("#txtBankHolderName").text().slice(2);
                    copyToClipboard(accountName)
                });

                $('#copyAccountNo').on('click', function () {
                    var accountNo = $("#txtBankAccountNo").text().slice(2);
                    copyToClipboard(accountNo)
                });


                function copyToClipboard(text) {
                    var input = document.createElement('textarea', { "permissions": ["clipboardWrite"] });
                    document.body.appendChild(input);
                    input.value = text;
                    input.focus();
                    input.select();

                    var s = document.execCommand('copy', false, null);
                    alert(s == true ? "Copied" : "Unable to Copy");

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
                                    window.location.replace('/FundTransfer/Default.aspx');
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