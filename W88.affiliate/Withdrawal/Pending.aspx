<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pending.aspx.cs" Inherits="Withdrawal_Pending" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("lblHeader", xeResources)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link type="text/css" href="/_Static/Css/Pending.css" rel="stylesheet" />
    <style type="text/css">
        .page-content { max-width:700px; margin-left:auto; margin-right:auto; }
    </style>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="a">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("lblHeader", xeResources)%></span></div>
            <div class="page-content">

                <form id="form1" runat="server" data-ajax="false">
                    <div class="div-content-wrapper">
                        <asp:Literal ID="litPending" runat="server"></asp:Literal>                        
                        <asp:HiddenField runat="server" ID="_repostcheckcode" />
                    </div>
                </form>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
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