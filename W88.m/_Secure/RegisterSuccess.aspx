<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterSuccess.aspx.cs" Inherits="_Secure_RegisterSuccess" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("register", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <%--<script type="text/javascript" src="/_Static/JS/PreLoad.js"></script>--%>
    <script type="application/javascript" src="/_Static/JS/add2home.js"></script>
    <script type="text/javascript" src="/_Static/JS/radar.js"></script>
</head>
<body>
    <div id="register" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->

        <div class="ui-content" role="main">
            <div class="register-success-message">
                <div class="register-success-icon"><span class="icon icon-check"></span></div>
                <%=commonCulture.ElementValues.getResourceString("regSuccessDesc", regTrans) %>
                <p id="paymentNote">
                </p>
            </div>

            <div class="ui-navbar ui-navbar-justified register-gateway" role="navigation" id="depositTabs" runat="server">
            </div>

            <div class="bank_logo">
                <p class="reg-contact">
                    <%
                        var contactText = commonCulture.ElementValues.getResourceString("regContact", regTrans);
                        contactText = contactText.Replace("{chatlink}", "/LiveChat/Default.aspx");
                    %>
                    <%=contactText %>
                </p>
                <i class="logo_10"></i>
                <i class="logo_11"></i>
                <i class="logo_12"></i>
                <i class="logo_13"></i>
                <i class="logo_14"></i>
                <i class="logo_15"></i>
            </div>
        </div>

        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>

    <script type="text/javascript">
        $(function () {
            window.history.forward();
            $(".register-success-content").css("display", "none");
            // piwik tracking signup
            w88Mobile.PiwikManager.trackSignup();

            if ($('#depositTabs li').length == 0) {
                w88Mobile.PiwikManager.trackEvent({
                    category: "RegSuccess",
                    action: "<%=base.strCountryCode %>",
                    name: "<%=base.strMemberID %>"
                });
                $('#paymentNote').append('<%= commonCulture.ElementValues.getResourceString("paymentNotice", commonVariables.PaymentMethodsXML)%>');
            } else {
                <%
                var pDesc = commonCulture.ElementValues.getResourceString("paymentDescription", commonVariables.PaymentMethodsXML);
                pDesc = pDesc.Replace("{termslink}", commonCulture.ElementValues.getResourceString("termsConditionsUrl", regTrans)); 
                %>
                $('#paymentNote').append('<%= pDesc%>');
                $(".register-success-content").css("display", "");
            }
        });
    </script>
    <!-- /page -->
</body>
</html>
