<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="_Index" %>

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
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("affiliate", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content">

                <div class="div-content-wrapper">
                    <table>
                        <tr>
                            <td style="text-align: center; width: 350px">
                                <a href="/OurProducts/OurProductsHeader.aspx" runat="server" id="ourProductlink" data-ajax="false">Our Product</a>
                            </td>
                            <td style="text-align: center; width: 350px">
                                <a href="/CommissionPlans.aspx" data-ajax="false" runat="server" id="commissionPlanlink">Commission Plans</a>
                            </td>
                        </tr>
                    </table>

                    <div id="divDefaultContent" runat="server">
                        <asp:Literal ID="mainContent" runat="server"></asp:Literal>
                    </div>

                    <div id="divAfterLoginContent" runat="server">
                        <table>
                            <tr>
                                <td colspan="2" style="text-align: center;">
                                    <a href="/_Secure/AccountInfo.aspx" data-ajax="false" runat="server" id="myAccountLink">My Account</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: center; width: 350px;">
                                    <a href="/Overview.aspx" data-ajax="false" runat="server" id="overviewLink">Overview</a>
                                </td>
                                <td style="text-align: center; width: 350px"></td>
                            </tr>
                        </table>
                    </div>
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
