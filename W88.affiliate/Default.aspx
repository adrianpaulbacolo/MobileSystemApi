<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>
    <link type="text/css" rel="stylesheet" href="/_Static/Css/Language.css" />
    <link rel="stylesheet" href="/_Static/Css/add2home.css">
    <script type="application/javascript" src="/_Static/Js/add2home.js"></script>
    <script type="application/javascript" src="/_Static/Js/checkManifest.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="a" data-ajax="false">
        <div data-role="header" data-position="fixed" class="div-nav-header">
            <div class="text-center"></div>
        </div>
        <div class="ui-content" role="main">
            <div class="page-content">
                <div id="divLanguageContainer" runat="server" class="div-langContainer"></div>
            </div>
        </div>
        <!-- /content -->
        <div data-role="footer" data-theme="b" data-position="fixed">
            <!--#include virtual="~/_static/footer.shtml" -->
        </div>
        <!-- /footer -->
    </div>
    <!-- /page -->
    <script type="text/javascript">
        var strMessage = '<%=strAlertMessage%>';
        if (strMessage != '') { alert(strMessage); }
    </script>
</body>
</html>