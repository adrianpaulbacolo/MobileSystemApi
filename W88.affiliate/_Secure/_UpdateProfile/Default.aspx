<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Secure_UpdateProfile_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("updateProfile", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link type="text/css" rel="stylesheet" href="/_Static/Css/UpdateProfile.css" />
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("updateProfile", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content">
                <div>
                    <div data-role="navbar">
                        <ul>
                            <li><a href="/_Secure/UpdateProfile/" data-ajax="false" class="ui-btn-active">uneditable</a></li>
                            <li><a href="/_Secure/UpdateProfile/Address" data-ajax="false">address</a></li>
                            <li><a href="/_Secure/UpdateProfile/SecurityQA" data-ajax="false">security</a></li>
                        </ul>
                    </div>
                    <div id="divUneditable">
                        <%--
                        <table>
                            <tr><td><asp:Literal ID="lblEmail" runat="server" /></td><td><asp:Literal ID="txtEmail" runat="server" /></td></tr>
                            <tr><td><asp:Literal ID="lblUsername" runat="server" /></td><td><asp:Literal ID="txtUsername" runat="server" /></td></tr>
                            <tr><td><asp:Literal ID="lblContact" runat="server" /></td><td><asp:Literal ID="txtContact" runat="server" /></td></tr>
                            <tr><td><asp:Literal ID="lblFName" runat="server" /></td><td><asp:Literal ID="txtFName" runat="server" /></td></tr>
                            <tr><td><asp:Literal ID="lblLName" runat="server" /></td><td><asp:Literal ID="txtLName" runat="server" /></td></tr>
                            <tr><td><asp:Literal ID="lblDOB" runat="server" /></td><td><asp:Literal ID="txtDOB" runat="server" /></td></tr>
                            <tr><td><asp:Literal ID="lblCountry" runat="server" /></td><td><asp:Literal ID="txtCountry" runat="server" /></td></tr>
                            <tr><td><asp:Literal ID="lblCurrency" runat="server" /></td><td><asp:Literal ID="txtCurrency" runat="server" /></td></tr>
                        </table>
                        --%>
                        <div>
                            <div><asp:Literal ID="lblEmail" runat="server" /></div>
                            <div><asp:Literal ID="txtEmail" runat="server" /></div>
                        </div>
                        <div>
                            <div><asp:Literal ID="lblUsername" runat="server" /></div>
                            <div><asp:Literal ID="txtUsername" runat="server" /></div>
                        </div>
                        <div>
                            <div><asp:Literal ID="lblContact" runat="server" /></div>
                            <div><asp:Literal ID="txtContact" runat="server" /></div>
                        </div>
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
                        <div>
                            <div><asp:Literal ID="lblCurrency" runat="server" /></div>
                            <div><asp:Literal ID="txtCurrency" runat="server" /></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">
            $(function () {
                //$('#divProfileNavBar > ul > li:first-child > a').addClass('ui-btn-active');
            });
        </script>
    </div>
</body>
</html>
