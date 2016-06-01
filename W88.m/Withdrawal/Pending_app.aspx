<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pending.aspx.cs" Inherits="Withdrawal_Pending" %>
<%@ Register TagPrefix="uc" TagName="Wallet" Src="~/UserControls/MainWalletBalance.ascx" %>

<!DOCTYPE html>
<html>
<head>
    <title>Pending Withdrawal</title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <div data-role="page" data-theme="b">
        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("lblHeader", xeResources)%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <uc:Wallet id="uMainWallet" runat="server" />
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br>
                <ul class="list fixed-tablet-size">
                    <li class="item item-input">
                        <asp:Literal ID="litPending" runat="server"></asp:Literal>
                        <br />
                        <input type="button" onclick="location.href = '/Deposit/Default_app.aspx';" value="Back" class="button-blue"  data-corners="false" />
                    </li>
                </ul>
            </form>
        </div>
        <!-- /content -->
        <script type="text/javascript">
            function CancelWithdrawal(obj)
            {
                if (confirm('<%=commonCulture.ElementValues.getResourceString("lblConfirmWithdrawal", xeResources)%>')) {
                    if ($(obj).attr('data-id').trim() != '') {
                        $.ajax({
                            type: "POST",
                            url: '/_Secure/AjaxHandlers/CancelWithdrawal.ashx',
                            beforeSend: function () { GPINTMOBILE.ShowSplash(); },
                            timeout: function () {
                                alert('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                                window.location.replace('/Withdrawal/Pending_app.aspx');
                            },
                            data: { TrxId: $(obj).attr('data-id'), MethodId: $(obj).attr('data-method') },
                            success: function (html) {
                                if (html.trim() == '0') {
                                    GPINTMOBILE.HideSplash();
                                    //alert(html);
                                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/CancelSuccess", xeErrors)%>'.replace('{trxId}', $(obj).attr('data-id')));
                                    $(obj).parentsUntil('tbody').fadeOut();
                                    window.setTimeout(function () {
                                        $(obj).parentsUntil('tbody').remove(); if ($('#table-reflow tbody tr').length == 0) {
                                            window.location.replace('/Withdrawal/Default_app.aspx');
                                        }
                                    }, 2000);
                                }
                                else {
                                    switch (html)
                                    {
                                        case "-1":
                                            alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/error-1", xeErrors)%>');
                                            break;
                                        case "-2":
                                            alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/error-2", xeErrors)%>');
                                            break;
                                        case "-3":
                                            alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/error-3", xeErrors)%>');
                                            break;
                                        case "-4":
                                            alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/error-4", xeErrors)%>');
                                            break;
                                        case "-5":
                                            alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/error-5", xeErrors)%>');
                                            break;
                                        case "-6":
                                            alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/error-6", xeErrors)%>');
                                            break;
                                        case "-7":
                                            alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/error-7", xeErrors)%>');
                                            break;
                                        default:
                                            alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/error-99", xeErrors)%>');
                                            break;
                                    }
                                    window.location.replace('/Withdrawal/Pending_app.aspx');
                                }
                            },
                            error: function (err) {
                            }
                        });
                    }
                }

                //return false;
            }
        </script>
    </div>
</body>
</html>
