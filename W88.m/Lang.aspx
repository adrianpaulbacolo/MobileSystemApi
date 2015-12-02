<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lang.aspx.cs" Inherits="_Lang" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("language", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>
    <script type="application/javascript" src="/_Static/Js/checkManifest.js"></script>
</head>
<body id="language">
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="b" data-ajax="false">

        <header id="header" data-role="header" data-position="fixed" data-tap-toggle="false">
            <a href="" role="button" data-rel="back" class="btn-clear ui-btn-left ui-btn"> < </a>
            <h1 class="title">
                <img src="/_Static/Images/logo-<%=commonVariables.SelectedLanguageShort%>.png" class="logo" alt="logo">
            </h1>
        </header>

        <div class="ui-content" role="main">
            <h2 class="page-title">Select Language</h2>
            <ul id="divLanguageContainer" runat="server" class="row row-wrap row-no-padding"></ul>
        </div>

        <!--#include virtual="~/_static/footer.shtml" -->

    </div>
    <!-- /page -->
    <script type="text/javascript">
        var strMessage = '<%=strAlertMessage%>';
        if (strMessage != '') { alert(strMessage); }
    </script>
</body>
</html>
