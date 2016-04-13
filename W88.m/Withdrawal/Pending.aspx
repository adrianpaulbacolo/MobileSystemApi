<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pending.aspx.cs" Inherits="Withdrawal_Pending" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("lblHeader", xeResources)%></title>
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
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("lblHeader", xeResources)%></h1>
        </header>

        <div class="ui-content" role="main">
            <div class="wallet main-wallet">
                <label class="label"><%=commonCulture.ElementValues.getResourceString("mainWallet", commonVariables.LeftMenuXML)%></label>
                <h2 class="value"><%=Session["Main"].ToString()%></h2>
                <small class="currency"><%=commonVariables.GetSessionVariable("CurrencyCode")%></small>
            </div>

            <form class="form" id="form1" runat="server" data-ajax="false">
                <br>
                <ul class="list fixed-tablet-size">
                    <li class="item item-input">
                        <asp:Literal ID="litPending" runat="server"></asp:Literal>
                    </li>
                </ul>
            </form>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
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
                                window.location.replace('/Withdrawal/Pending.aspx');
                            },
                            data: { TrxId: $(obj).attr('data-id'), MethodId: $(obj).attr('data-method') },
                            success: function (html) {
                                if (html.trim() == '0') {
                                    GPINTMOBILE.HideSplash();
                                    //alert(html);
                                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Withdrawal/CancelSuccess", xeErrors)%>'.replace('{trxId}', $(obj).attr('data-id')));
                                    $(obj).parentsUntil('tbody').fadeOut();
                                    window.setTimeout(function () { $(obj).parentsUntil('tbody').remove(); if ($('#table-reflow tbody tr').length == 0) { window.location.replace('/Withdrawal/Default.aspx'); } }, 2000);

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
                                    window.location.replace('/Withdrawal/Pending.aspx');
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
