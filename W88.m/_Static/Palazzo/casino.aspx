<%@ Page Language="C#" AutoEventWireup="true" CodeFile="casino.aspx.cs" Inherits="_Static_Palazzo_casino" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="static-content">
                <div>
                    <img src="/_Static/Images/Download/W88-Mobile-ClubPalazzo-Casino.jpg" alt="banner" class="img-responsive">
                </div>
                <div class="downloadmsg">
                    <span runat="server" id="spanMsg"></span>
                    <a runat="server" data-ajax="false" href="http://mlive.w88palazzo.com" id="sDownload" target="_blank"></a>
                </div>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    <!-- /page -->
    </div>
</body>
</html>
