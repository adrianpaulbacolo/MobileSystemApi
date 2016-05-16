<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="_Change_Password" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("changePassword", commonVariables.LeftMenuXML)%></title>
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
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("changePassword", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            <form id="form1" class="form"  runat="server" data-ajax="false">
                <p>&nbsp;</p>
                <ul class="list fixed-tablet-size">
                    <li class="item item-input">
                        
                        <div><asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" /></div>
                        <asp:TextBox name="txtPassword" runat="server" type="password" maxlength="20" id="txtPassword" data-mini="true" data-clear-btn="true"/>
                    </li>
                    <li class="item item-input">
                        
                        <div><asp:Label ID="lblPasswordNew" runat="server" AssociatedControlID="txtPasswordNew" /></div>
                        <asp:TextBox  name="txtPasswordNew" runat="server" type="password" maxlength="40" id="txtPasswordNew" data-mini="true" data-clear-btn="true" />
                    </li>
                    <li class="item item-input">
                        
                        <div><asp:Label ID="lblPasswordConfirm" runat="server" AssociatedControlID="txtPasswordConfirm" /></div>
                        <asp:TextBox  name="txtPasswordConfirm" runat="server" type="password" maxlength="40" id="txtPasswordConfirm" data-mini="true" data-clear-btn="true" />
                    </li>

                    <li class="item row">
                        <div class="col">
                            <a href="/Index" id="btnCancel" class="ui-btn btn-bordered" text="cancel" data-corners="false" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("cancel", commonVariables.LeftMenuXML)%></a>
                        </div>
                        <div class="col">
                            <asp:Button  ID="btnSubmit" name="btnSubmit" runat="server" Text="Submit"  OnClick="btnSubmit_Click" />
                        </div>
                    </li>

                </ul>
            </form>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            $(function () {
                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case '-1':
                            alert('<%=strAlertMessage%>');
                            break;
                        case '0':
                            alert('<%=strAlertMessage%>');
                            break;
                        case '1':
                            alert('<%=strAlertMessage%>');
                            window.location.replace('/Index.aspx');
                            break;
                        default:
                            break;
                    }
                }
            });

            $('#form1').submit(function (e) {
                $('#btnSubmit').attr("disabled", true);
                if ($('#txtPassword').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPassword", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtPasswordNew').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPasswordNew", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtPasswordConfirm').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPasswordNew", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtPasswordNew').val().trim() != $('#txtPasswordConfirm').val().trim())
                {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/UnmatchedPassword", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else { $('#btnSubmit').attr("disabled", false); }
            });
        </script>
    </div>
</body>
</html>
