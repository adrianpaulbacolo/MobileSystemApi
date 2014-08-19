<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdatePassword.aspx.cs" Inherits="_Secure_UpdatePassword" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("updateProfile", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link type="text/css" rel="stylesheet" href="/_Static/Css/UpdatePassword.css" />
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("updateProfile", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content">
                <div>
                    <div class="div-profile-avatar">
                        <div><img src="/_Static/Images/avatar.jpg" /></div>
                        <div>
                            <div><asp:Literal id="txtUserName" runat="server" /></div>
                            <div><asp:Literal id="txtEmail" runat="server" /></div>
                            <div><asp:Literal id="txtContact" runat="server" /></div>
                            <div><asp:Literal id="txtCurrency" runat="server" /></div>
                        </div>
                        <div>
                            <!-- <div><a href="javascript:void(0);" data-ajax="false"><img src="/_Static/Images/icon-message.png" /></a></div> -->
                            <div><a href="/_Secure/UpdateProfile" data-ajax="false"><img src="/_Static/Images/icon-profile.png" /></a></div>
                        </div>
                    </div>
                    <form id="form1" runat="server" data-ajax="false">
                        <div class="div-password-change">
                            <div>
                                <div><asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" /></div>
                                <div><asp:TextBox ID="txtPassword" runat="server" CssClass="ui-mini" TextMode="Password" MaxLength="10" /></div>
                            </div>
                            <div>
                                <div><asp:Label ID="lblPasswordNew" runat="server" AssociatedControlID="txtPasswordNew" /></div>
                                <div><asp:TextBox ID="txtPasswordNew" runat="server" CssClass="ui-mini" TextMode="Password" MaxLength="10" /></div>
                            </div>
                            <div>
                                <div><asp:Label ID="lblPasswordConfirm" runat="server" AssociatedControlID="txtPasswordConfirm" /></div>
                                <div><asp:TextBox ID="txtPasswordConfirm" runat="server" CssClass="ui-mini" TextMode="Password" MaxLength="10" /></div>
                            </div>
                        </div>
                        <div class="div-password-submit">
                            <asp:Button ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" />
                        </div>
                   </form>
                </div>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
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
                            //window.location.replace('/Index.aspx');
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
