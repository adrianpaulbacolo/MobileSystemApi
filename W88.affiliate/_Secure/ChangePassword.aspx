<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="_Secure_ChangePassword" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("myAccount", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
   <%-- <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>--%>
    <link type="text/css" rel="stylesheet" href="/_Static/Css/MyAccount.css" />
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("myAccount", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content">
                <form id="form1" runat="server" data-ajax="false">
                <div class="div-content-wrapper">

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="Label3" Visible ="false" />
                        </div>
                        <div>
                            <span><%=commonCulture.ElementValues.getResourceString("changePassword", commonVariables.LeftMenuXML)%></span>
                        </div>
                    </div>

                    <div class="div-register-normal">
                       <div>
                            <asp:Label ID="lblCurrentPassword" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtCurrentPassword" runat="server" placeholder=" " CssClass="ui-mini" TextMode="Password" MaxLength="10" data-clear-btn="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblNewPassword" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtNewPassword" runat="server" placeholder=" " CssClass="ui-mini" TextMode="Password" MaxLength="10"  data-clear-btn="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblConfirmPassword" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" placeholder=" " CssClass="ui-mini" TextMode="Password" MaxLength="10" data-clear-btn="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="Label1" runat="server" Visible="false" />
                        </div> 
                        <div>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" />
                        </div>
                     </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="Label2" runat="server" Visible="false" />
                        </div> 
                        <div>
                            <a data-theme="c" ID="btnCancel" runat="server" Text="cancel" class="ui-btn" data-corners="false" data-ajax="false" href="/Index" />
                        </div>
                    </div>
                  <%--  <asp:HiddenField id="hidValues" runat="server" />
                	<asp:HiddenField runat="server" ID="ioBlackBox" Value="" />--%>
                </div>
                </form>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">

    
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
           
            $('#form1').submit(function (e) {
                $('#btnSubmit').attr("disabled", true);
                if ($('#txtCurrentPassword').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPassword", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtNewPassword').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPasswordNew", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtConfirmPassword').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/MissingPasswordNew", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtNewPassword').val().trim() != $('#txtConfirmPassword').val().trim()) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdatePassword/UnmatchedPassword", xeErrors)%>');
                        $('#btnSubmit').attr("disabled", false);
                        e.preventDefault();
                        return;
                    }
                    else { $('#btnSubmit').attr("disabled", false); }
            });


        </script>
               
    </div>
    <!-- /page -->
</body>
</html>