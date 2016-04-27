<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClubBravado.aspx.cs" Inherits="Slots_ClubBravado" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceXPathString("Products/ClubBravado/Label", commonVariables.ProductsXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b" id="slots">

        <header data-role="header" data-theme="b" data-position="fixed" id="header">
            <a class="btn-clear ui-btn-left ui-btn" href="#divPanel" data-role="none" role="button" id="aMenu" data-load-ignore-splash="true">
                <i class="icon-navicon"></i>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceXPathString("/Products/ClubBravado/Label", commonVariables.ProductsXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            <div id="divContainer" runat="server"></div>
        </div>
        
        <script type="text/javascript">
            $(function () {
                w88Mobile.Slots.club = "ClubBravado";
                w88Mobile.Slots.init();
            });
        </script>
        <!--#include virtual="~/_static/navMenu.shtml" -->

    </div>
</body>
</html>
