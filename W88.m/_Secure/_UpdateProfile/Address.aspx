<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Address.aspx.cs" Inherits="_Secure_UpdateProfile_Address" %>

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
                            <li><a href="/_Secure/UpdateProfile/" data-ajax="false">uneditable</a></li>
                            <li><a href="/_Secure/UpdateProfile/Address" data-ajax="false" class="ui-btn-active">address</a></li>
                            <li><a href="/_Secure/UpdateProfile/SecurityQA" data-ajax="false">security</a></li>
                        </ul>
                    </div>
                    <form id="frmSecurity" runat="server" data-ajax="false">
                        <div id="divAddress">
                            <div class="ui-field-contain">
                                <div><asp:Label ID="lblAddress" runat="server" /></div>
                                <div><asp:TextBox ID="txtAddress" TextMode="MultiLine" runat="server" /></div>
                            </div>
                            <div>
                                <div><asp:Label ID="lblCity" runat="server" /></div>
                                <div><asp:TextBox ID="txtCity" runat="server" /></div>
                            </div>
                            <div>
                                <div><asp:Label ID="lblPostal" runat="server" /></div>
                                <div><asp:TextBox ID="txtPostal" runat="server" /></div>
                            </div>
                            <div>
                                <asp:Button ID="btnSubmit" runat="server" CssClass="button-blue" data-corners="false" />
                            </div>
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
                //$('#divProfileNavBar > ul > li:first-child > a').addClass('ui-btn-active');
            });
        </script>
    </div>
</body>
</html>