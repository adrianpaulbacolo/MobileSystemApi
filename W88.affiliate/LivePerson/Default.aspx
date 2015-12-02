<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="LivePerson_Default" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("liveHelp", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/JS/lpChatV3.min.js?v=<%=System.DateTime.Now.Ticks%>"></script>
    <%--<script type="text/javascript" src="//base.liveperson.net/hcp/html/lpChatV3.min.js?v=<%=System.DateTime.Now%>"></script>--%>
    <script type="text/javascript" src="/_Static/JS/LP.js?v=<%=System.DateTime.Now.Ticks%>"></script>
    <link type="text/css" href="/_Static/Css/LP.css" rel="stylesheet" />
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" data-theme="a">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("liveHelp", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content">
                <div class="div-content-wrapper">
                <div id="divChatContainer">
                    <div id="chatLines"></div>
                </div>
                <div id="divChatSend">
                    <div>
                        <input type="text" id="txtSendMsg" runat="server" />
                    </div>
                    <div>
                        <div id="btnSend" runat="server" class="ui-btn ui-btn-a">send</div>
                    </div>
                </div>
                <a id="btnReqChat" runat="server" class="ui-btn ui-blue" data-corners="false">start chat</a>
                <a id="btnEndChat" runat="server" data-corners="false" class="ui-button">end chat</a>
                </div>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
