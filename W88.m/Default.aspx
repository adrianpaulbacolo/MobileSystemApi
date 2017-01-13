<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/JS/PreLoad.js"></script>
    <script type="application/javascript" src="/_Static/JS/add2home.js"></script>
    <script type="application/javascript" src="/_Static/JS/checkManifest.js"></script>
</head>
<body id="language">
    <div data-role="page" data-theme="b" data-ajax="false">

        <header id="header" data-role="header" data-position="fixed" data-tap-toggle="false">
            <h1 class="title">
                <img src="/_Static/Images/logo-<%=commonVariables.SelectedLanguageShort%>.png" class="logo" alt="logo">
            </h1>
        </header>

        <div class="ui-content" role="main">
            <h2 class="page-title">Select Language</h2>
            <ul id="divLanguageContainer" runat="server" class="lang-list"></ul>
        </div>


    </div>
    <!-- /page -->
    <script type="text/javascript">
        var strMessage = '<%=strAlertMessage%>';
        if (strMessage != '') { alert(strMessage); }
    </script>
</body>
</html>
