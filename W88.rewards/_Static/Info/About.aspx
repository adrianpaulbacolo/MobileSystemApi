<%@ Page Language="C#" AutoEventWireup="true" CodeFile="About.aspx.cs" Inherits="_Info_About" Async="true"%>

<!DOCTYPE html>
<html>
<head>
    <title>About Rewards</title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div data-role="page" data-theme="b">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="main-content static-content">
            <div class="container">
                <asp:Literal ID="htmltext" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</body>
</html>
