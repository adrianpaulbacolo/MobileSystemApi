<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OurProductsHeader.aspx.cs" Inherits="OurProducts_OurProductsHeader" %>

<!DOCTYPE html>

<html>
<head>
   
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML).Replace(" -", "")/* + commonCulture.ElementValues.getResourceString("home", commonVariables.LeftMenuXML)*/%></title>
    <!--#include virtual="~/_static/head.inc" -->
        <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link type="text/css" href="/_Static/Css/FAQ.css" rel="stylesheet" />
    <script type="application/javascript" src="/_Static/Js/add2home.js"></script>
    
    <%--<link type="text/css" href="/_Static/Css/IndexScroll.css" rel="stylesheet">
    <link type="text/css" href="/_Static/Css/sprite.css" rel="stylesheet">--%>
   </head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
    <%--<div id="divMain" >--%>
        <!--#include virtual="~/_static/header.shtml" -->

        <div class="ui-content" role="main">
            <img id="promoLoader" src="/_Static/Css/images/ajax-loader.gif" style="display: none;" />
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("ourProducts", commonVariables.LeftMenuXML)%></span></div>
            <%--<div class="page-content div-promotions-wrapper" id="divFAQ"></div>--%>
            
            <div class="page-content div-promotions-wrapper" >
                <ul>
                    <li>
                        <div class="div-promo-row">
                            <a href="/OurProducts/OurProductsDetail.aspx?OurProducts=Sports" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lblSports", xeResources)%></div>
                                </div>
                            </a>
                        </div>
                    </li>
                    <li>
                        <div class="div-promo-row">
                            <a href="/OurProducts/OurProductsDetail.aspx?OurProducts=LiveCasino" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lblLiveCasino", xeResources)%></div>
                                </div>
                            </a>
                        </div>
                    </li>
                    <li>
                        <div class="div-promo-row">
                            <a href="/OurProducts/OurProductsDetail.aspx?OurProducts=Lottery" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lblLottery", xeResources)%></div>
                                </div>
                            </a>
                        </div>
                    </li>
                    <li>
                        <div class="div-promo-row">
                            <a href="/OurProducts/OurProductsDetail.aspx?OurProducts=Slots" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lblSlots", xeResources)%></div>
                                </div>
                            </a>
                        </div>
                    </li>
                    <li>
                        <div class="div-promo-row">
                            <a href="/OurProducts/OurProductsDetail.aspx?OurProducts=Poker" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lblPoker", xeResources)%></div>
                                </div>
                            </a>
                        </div>
                    </li>
                  
                </ul>
            </div>

        </div>

        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
