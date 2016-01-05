<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateProfile.aspx.cs" Inherits="_Secure_UpdateProfile" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("updateProfile", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link type="text/css" rel="stylesheet" href="/_Static/Css/_UpdateProfile.css" />
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
                            <div><a href="/_Secure/UpdatePassword" data-ajax="false"><img src="/_Static/Images/icon-changepass.png" /></a></div>
                        </div>
                    </div>
                    <div class="div-profile-personal">
                        <div>
                            <div><asp:Literal ID="lblFirstName" runat="server" /></div>
                            <div><asp:Literal ID="txtFirstName" runat="server" /></div>
                        </div>
                        <div>
                            <div><asp:Literal ID="lblLastName" runat="server" /></div>
                            <div><asp:Literal ID="txtLastName" runat="server" /></div>
                        </div>
                        <div>
                            <div><asp:Literal ID="lblDOB" runat="server" /></div>
                            <div><asp:Literal ID="txtDOB" runat="server" /></div>
                        </div>
                        <div>
                            <div><asp:Literal ID="lblCountry" runat="server" /></div>
                            <div><asp:Literal ID="txtCountry" runat="server" /></div>
                        </div>
                    </div>
                    <form id="form1" runat="server" data-ajax="false">
                        <div class="div-profile-preferred">
                           <div>
                                <div><asp:Label ID="lblGender" runat="server" AssociatedControlID="drpGender" /></div>
                                <div><asp:DropDownList ID="drpGender" runat="server" data-role="flipswitch" data-corners="false" CssClass="ui-mini" /></div>
                           </div>
                           <div>
                                <div><asp:Label ID="lblLanguage" runat="server" AssociatedControlID="drpLanguage" /></div>
                                <div><asp:DropDownList ID="drpLanguage" runat="server" CssClass="ui-mini" /></div>
                            </div>
                            <div>
                                <div><asp:Label ID="lblOdds" runat="server" AssociatedControlID="drpOdds" /></div>
                                <div><asp:DropDownList ID="drpOdds" runat="server" CssClass="ui-mini" /></div>
                            </div>
                        </div>
                        <div class="div-profile-address">
                            <div>
                                <div><asp:Label ID="lblAddress" runat="server" AssociatedControlID="txtAddress" /></div>
                                <div><asp:TextBox ID="txtAddress" TextMode="MultiLine" runat="server" /></div>
                            </div>
                            <div>
                                <div><asp:Label ID="lblCity" runat="server" AssociatedControlID="txtCity" /></div>
                                <div><asp:TextBox ID="txtCity" runat="server" /></div>
                            </div>
                            <div>
                                <div><asp:Label ID="lblPostal" runat="server" AssociatedControlID="txtPostal" /></div>
                                <div><asp:TextBox ID="txtPostal" runat="server" /></div>
                            </div>
                        </div>
                        <div class="div-profile-security">
                            <div>
                                <div><asp:Label ID="lblSecurityQuestion" runat="server" AssociatedControlID="drpSecurityQuestion" /></div>
                                <div><asp:DropDownList ID="drpSecurityQuestion" runat="server" CssClass="ui-mini" /></div>
                            </div>
                            <div>
                                <div><asp:Label ID="lblSecurityAnswer" runat="server" AssociatedControlID="txtSecurityAnswer" /></div>
                                <div><asp:TextBox ID="txtSecurityAnswer" runat="server" /></div>
                            </div>
                        </div>
                        <div class="div-profile-submit">
                            <div>
                                <div><asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" /></div>
                                <div><asp:TextBox ID="txtPassword" runat="server" CssClass="ui-mini" TextMode="Password" MaxLength="10" /></div>
                            </div>

                            <asp:Button ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" />
                        </div>
                   </form>
                </div>
            </div>
        </div>
        <!-- /content -->
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
                if ($('#txtAddress').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingAddress", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtCity').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingCity", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtPostal').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingPostal", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#drpSecurityQuestion').val() == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingSecurityQuestion", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtSecurityAnswer').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingSecurityAnswer", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtPassword').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingPassword", xeErrors)%>');
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
