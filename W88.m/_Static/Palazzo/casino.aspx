<%@ Page Language="C#" AutoEventWireup="true" CodeFile="casino.aspx.cs" Inherits="_Static_Palazzo_casino" %>
<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <style type="text/css">
        #divContent {
            margin:auto;
            text-align:center;
        }

            #divContent > div > img {
                width:100%;
                max-width:900px;
            }

        .downloadmsg {
            max-width:900px;
            width:100%;
            margin:auto;
            text-align:left;
                font-weight:bold;
        }

            .downloadmsg > span {
                display:block;
                padding:.5em;
            }
        .downloadmsg > span > span:first-child {
            display:block;
                text-align:center;
        }

        .blue {
            color:#2ad;
            font-weight:bold;
        }

        #sDownload {
            display:block;
            background-color:#2ad;
            color:#fff;
            text-align:center;
            padding:.75em;
            text-decoration:none;
            text-transform:uppercase;
            margin:1em .5em;
        }
    </style>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
            <div id="divContent">
                <div>
                    <img src="/_Static/Images/Download/W88-Mobile-ClubPalazzo-Casino.jpg" />
                </div>
                <div class="downloadmsg">
                    <span runat="server" id="spanMsg"></span>
                    <a runat="server" data-ajax="false" href="http://mlive.w88palazzo.com" id="sDownload"></a>
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