<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Async="true"%> 
<%@ Import Namespace="W88.BusinessLogic.Rewards.Helpers" %>
<%@ Import Namespace="W88.BusinessLogic.Rewards.Models" %>
<%@ Import Namespace="W88.BusinessLogic.Shared.Helpers" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Brand)%></title>
    <!--#include virtual="~/_static/head.inc" -->
</head>
<body>
    <div data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->
        <section class="splash-screen">
            <div class="splash-screen-box">
                <img src="/_Static/Css/images/logo-large.png" class="img-responsive center-block" alt="">
            </div>
        </section>
        <div data-role="footer">
            <section class="footer footer-public">
                <div class="btn-group btn-group-justified" role="group">
                    <div class="btn-group" role="group">                        
                        <a id="loginFooterButton" style="display: none;" data-ajax="false" class="btn" href="/_Secure/Login.aspx">
                            <span class="icon icon-login"></span><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Login)%>
                        </a>
                        <a id="logoutFooterButton" href="javascript: logout();" class="btn">
                            <span class="icon icon-login"></span><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Logout)%>
                        </a>
                    </div>
                    <div class="btn-group" role="group">
                        <a data-ajax="false" class="btn" href="/Catalogue?categoryId=0&sortBy=2">
                            <span class="icon icon-catalog"></span><%=RewardsHelper.GetTranslation(TranslationKeys.Label.Catalogue)%>
                        </a>
                    </div>
                </div>
            </section>
        </div>
    </div>
    <script type="text/javascript">
        var message = '<%=AlertMessage%>';
        if (!_.isEmpty(message)) { window.w88Mobile.Growl.shout(message); }
    </script>
</body>
</html>