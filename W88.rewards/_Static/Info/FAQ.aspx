<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FAQ.aspx.cs" Inherits="_Info_Faq" %>

<!DOCTYPE html>
<html>
<head>
    <title>FAQ</title>
    <!--#include virtual="~/_static/head.inc" -->
    <style>
        h5:before {
            display: block; 
            content: " "; 
            margin-top: 0; 
            height: 44px; 
            visibility: hidden;
        }
    </style>
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
