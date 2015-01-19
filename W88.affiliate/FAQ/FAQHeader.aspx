<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FAQHeader.aspx.cs" Inherits="FAQ_FAQHeader" %>

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
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("faq", commonVariables.LeftMenuXML)%></span></div>
            <%--<div class="page-content div-promotions-wrapper" id="divFAQ"></div>--%>
            
            <div class="page-content div-promotions-wrapper" >
                <ul>
                    <li>
                        <div class="div-promo-row">
                            <a href="/FAQ/FAQDetail.aspx?faq=AffiliateProgramme" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lbl_tab_programme", xeResources)%></div>
                                </div>
                            </a>
                        </div>
                    </li>
                    <li>
                        <div class="div-promo-row">
                            <a href="/FAQ/FAQDetail.aspx?faq=MarketingMaterials" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lbl_tab_marketing", xeResources)%></div>
                                </div>
                            </a>
                        </div>
                    </li>
                    <li>
                        <div class="div-promo-row">
                            <a href="/FAQ/FAQDetail.aspx?faq=EarningsAndPayments" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lbl_tab_earnings", xeResources)%></div>
                                </div>
                            </a>
                        </div>
                    </li>
                    <li>
                        <div class="div-promo-row">
                            <a href="/FAQ/FAQDetail.aspx?faq=MyAccount" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lbl_tab_myaccount", xeResources)%></div>
                                </div>
                            </a>
                        </div>
                    </li>
                    <li>
                        <div class="div-promo-row">
                            <a href="/FAQ/FAQDetail.aspx?faq=Disclaimer" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lbl_tab_disclaimer", xeResources)%></div>
                                </div>
                            </a>
                        </div>
                    </li>
                    <li>
                        <div class="div-promo-row">
                            <a href="/FAQ/FAQDetail.aspx?faq=PrivacyPolicy" data-ajax="false">
                                <div>
                                     <div class="div-overview-header"><%=commonCulture.ElementValues.getResourceString("lbl_tab_privacypolicy", xeResources)%></div>
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
