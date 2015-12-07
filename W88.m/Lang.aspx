<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lang.aspx.cs" Inherits="_Lang" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("language", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body id="language">
    <div data-role="page" data-theme="b">

        <form id="form1" runat="server">
            <header id="header" data-role="header" data-position="fixed" data-tap-toggle="false">
                <h1 class="title">
                    <img src="/_Static/Images/logo-<%=commonVariables.SelectedLanguageShort%>.png" class="logo" alt="logo">
                </h1>
                <a href="" role="button" data-rel="back" class="btn-clear ui-btn-right ui-btn">
                    Cancel &nbsp;
                </a>
            </header>

            <div class="ui-content" role="main">
                <h2 class="page-title">Select Language</h2>
                <ul id="divLanguageContainer" runat="server" class="row row-wrap row-no-padding"></ul>
            </div>
        </form>

    </div>
</body>
</html>
