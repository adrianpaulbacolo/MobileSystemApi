<%@ Page Language="C#" AutoEventWireup="true" CodeFile="w88.aspx.cs" Inherits="_Static_Downloads_w88" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
</head>
<body>
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="static-content">
                <div class="banner slider">
                    <img src="/_Static/Images/Download/Bnr-ClubW88-iOS.jpg" alt="banner" class="img-responsive">
                </div>
                <div class="downloadmsg">
                    <span runat="server" id="spanMsg"></span>
                    <a href="#" id="sDownload" class="ui-btn btn-primary" runat="server" data-ajax="false" target="_blank"></a>
                </div>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
