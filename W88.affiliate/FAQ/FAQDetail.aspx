<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FAQDetail.aspx.cs" Inherits="FAQ_FAQDetail" %>

<!DOCTYPE html>

<html>
<head>
   
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
        <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="application/javascript" src="/_Static/Js/add2home.js"></script>
    
    <link type="text/css" href="/_Static/Css/IndexScroll.css" rel="stylesheet">
    <link type="text/css" href="/_Static/Css/sprite.css" rel="stylesheet">
</head>
<body>
        <!--#include virtual="~/_static/splash.shtml" -->
     <div id="divMain" data-role="page">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">

            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("faq", commonVariables.LeftMenuXML)%></span></div>
                <div class="page-content">
                   <%-- <form id="form1" runat="server" data-ajax="false">--%>
                        <div class="div-content-wrapper">
            <%--<div class="div-page-header" id="divDefaultHeader" runat="server"><span id="lblDefaultHeader" runat="server"></span></div>--%>
            <%--<div id="divContent">--%>
                    <div>         
                    <asp:Literal ID="mainContent" runat="server"></asp:Literal>
                    </div>

            <%--</div>--%>
                            </div>
         <%-- </form>--%>
         </div>
              </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
