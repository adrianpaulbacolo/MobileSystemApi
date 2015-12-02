<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Overview.aspx.cs" Inherits="_Overview" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("overview", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
   <link type="text/css" href="/_Static/Css/Overview.css" rel="stylesheet" />
     <%--<link type="text/css" href="/_Static/Css/Main.css" rel="stylesheet" />--%>
    <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <script type="text/javascript">
        //alert('<%=commonCulture.ElementValues.getResourceString("lblAffiliateInfo", xeResources)%>');

       
  
        $(function () { getOverview(); });
        $(function () { getSummary(); });
        $(function () { getTransaction(); });
        $(function () { getSubAffiliate(); });

        //$(window).resize(function () { $('.div-promo-row > a > div:last-child > div').css({ maxWidth: ($(window).width() - 200) + 'px' }); });
        //function timerV2(pid, start_date, end_date) { if (new Date('<%=System.DateTime.Now.ToString(commonVariables.DateTimeFormat)%>') < new Date(start_date) || new Date('<%=System.DateTime.Now.ToString(commonVariables.DateTimeFormat)%>') > new Date(end_date)) { $('div#' + pid).hide(); } }
       
        var listObj = $("#divAffInfo").append('<ul></ul>').find('ul');
        var liOverview = $('<li />');
        var divOverviewWrapper = $('<div />', { id: $(this).attr('id'), class: 0 % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
        var hrefOverview = $('<a />', { href: "javascript:void(0)", onclick: "javascript:OpenOverviewDetails(this);" });

        function getOverview() {

            var listObj = $("#divAffInfo").append('<ul></ul>').find('ul');
            var liOverview = $('<li />');
            //var divOverviewWrapper = $('<div />', { id: $(this).attr('id'), class: 0 % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
            var divOverviewWrapper = $('<div />', { id: 1, class: 0 % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
            var hrefOverview = $('<a />', { href: "javascript:void(0)", onclick: "javascript:OpenOverviewDetails(1);" });

            //Affiliate Info
            var divTitle = $('<div />', { class: 'div-overview-header' }).text('<%=commonCulture.ElementValues.getResourceString("lblAffiliateInfo", xeResources)%>');
            //with hyperlink
            //var divContent = $('<div />', { class: 'div-overview-content' }).html('<%=commonCulture.ElementValues.getResourceString("lblAffiliateId", xeResources)%><%=HttpContext.Current.Session["MemberCode"]%>' + "<br/>" +
                            //'<%=commonCulture.ElementValues.getResourceString("lblLastLogin", xeResources)%><% =lastLogin.ToString() %>' + "<br/>" +
                            //'<%=commonCulture.ElementValues.getResourceString("lblAffiliateLink", xeResources)%>' + "<a href=" + '<% =affiliateLink.ToString() %>' + ">" + '<% =affiliateUrl.ToString() %>' + "</a><br/>" +
                            //'<%=commonCulture.ElementValues.getResourceString("lblCreative", xeResources)%>');

            //without hyperlink
            //var divContent = $('<div />', { class: 'div-overview-content' }).html('<%=commonCulture.ElementValues.getResourceString("lblAffiliateId", xeResources)%><%=HttpContext.Current.Session["MemberCode"]%>' + "<br/>" +
                            //'<%=commonCulture.ElementValues.getResourceString("lblLastLogin", xeResources)%><% =lastLogin.ToString() %>' + "<br/>" +
                            //'<%=commonCulture.ElementValues.getResourceString("lblAffiliateLink", xeResources)%>' + '<% =affiliateUrl.ToString() %>' + "<br/>" +
                           //'<%=commonCulture.ElementValues.getResourceString("lblCreative", xeResources)%>');

            //var divContent = $('<div />', { class: 'div-overview-content' }).html('<%=commonCulture.ElementValues.getResourceString("lblAffiliateId", xeResources)%><%=HttpContext.Current.Session["MemberCode"]%>' + "<br/>" +
            //'<%=commonCulture.ElementValues.getResourceString("lblLastLogin", xeResources)%><% =lastLogin.ToString() %>');

            var divContent = $('<div />', { class: 'div-overview-content' }).html('<%=commonCulture.ElementValues.getResourceString("lblAffiliateId", xeResources)%><%=HttpContext.Current.Session["MemberCode"]%>' + "<br/>" +
                            '<%=commonCulture.ElementValues.getResourceString("lblLastLogin", xeResources)%><% =lastLogin.ToString() %>' + "<br/>" +
                            '<%=commonCulture.ElementValues.getResourceString("lblAffiliateLink", xeResources)%>' + "<a href=" + '<% =affiliateLink.ToString() %>' + ">" + '<% =affiliateUrl.ToString() %>' + "</a><br/>");
  
            listObj.append($(liOverview).append($(divOverviewWrapper).append($(hrefOverview).append($('<div />', {}).append(divTitle)))).append(divContent));
            $('.div-promo-row > a > div:last-child > div').css({ maxWidth: ($(window).width() - 500) + 'px' });
            $(this).find('script').each(function () { $.globalEval(this.text || this.textContent || this.innerHTML || ''); });
         
            //if (location.hash != '') {
            //    $(location.hash).next().slideToggle();
            //    var divObj = $(location.hash).find('div')[1];
            //    if ($(divObj).css('background-image').indexOf('arrow-up') > 0) { $(divObj).css('background-image', "url('/_Static/Images/arrow-down.png')"); }
            //    else { $(divObj).css('background-image', "url('/_Static/Images/arrow-up.png')"); }
            //}
        }

        function getSummary() {

            //var listObj = $("#divAffInfo").append('<ul></ul>').find('ul');
            var listObj = $("#divAffInfo").find('ul');
            var liOverview = $('<li />');
            //var divOverviewWrapper = $('<div />', { id: $(this).attr('id'), class: 1 % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
            var divOverviewWrapper = $('<div />', { id: 2, class: 1 % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
            var hrefOverview = $('<a />', { href: "javascript:void(0)", onclick: "javascript:OpenOverviewDetails(2);" });
            var divSummaryTable = $('<div />', { class: 'div-overview-content' });

            //Overall Summary
            var divTitle = $('<div />', { class: 'div-overview-header' }).text('<%=commonCulture.ElementValues.getResourceString("lblOverallSummary", xeResources)%>');
            //var divContent = $('<div />', { class: 'div-overview-content' }).html('<% =overallSummary.ToString() %>' + "<br/>" + '<% =overallComm.ToString() %>' + "<br/>" + '<% =overallNegBal.ToString() %>' + "<br/>" + '<% =overallSummary2.ToString() %>');
            var divContent = $('<div />', { class: 'div-overview-content' }).html('<% =overallSummary.ToString() %> <% =overallComm.ToString() %> <% =overallNegBal.ToString() %> <% =overallSummary2.ToString() %>');

            listObj.append($(liOverview).append($(divOverviewWrapper).append($(hrefOverview).append($('<div />', {}).append(divTitle)))).append(divContent));
            $('.div-promo-row > a > div:last-child > div').css({ maxWidth: ($(window).width() - 200) + 'px' });
            $(this).find('script').each(function () { $.globalEval(this.text || this.textContent || this.innerHTML || ''); });

                 
                    //if (location.hash != '') {
                    //    $(location.hash).next().slideToggle();
                    //    var divObj = $(location.hash).find('div')[1];
                    //    if ($(divObj).css('background-image').indexOf('arrow-up') > 0) { $(divObj).css('background-image', "url('/_Static/Images/arrow-down.png')"); }
                    //    else { $(divObj).css('background-image', "url('/_Static/Images/arrow-up.png')"); }
                    //}
        }

        function getTransaction() {

            //var listObj = $("#divAffInfo").append('<ul></ul>').find('ul');
            var listObj = $("#divAffInfo").find('ul');
            var liOverview = $('<li />');
            //var divOverviewWrapper = $('<div />', { id: $(this).attr('id'), class: 1 % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
            var divOverviewWrapper = $('<div />', { id: 3, class: 0 % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
            var hrefOverview = $('<a />', { href: "javascript:void(0)", onclick: "javascript:OpenOverviewDetails(3);" });

            //Transaction Data
            var divTitle = $('<div />', { class: 'div-overview-header' }).text('<%=commonCulture.ElementValues.getResourceString("lblTrans", xeResources)%>');
            var divContent = $('<div />', { class: 'div-overview-content-tranData' }).html('<% =transactionData.ToString() %>');
           
            listObj.append($(liOverview).append($(divOverviewWrapper).append($(hrefOverview).append($('<div />', {}).append(divTitle)))).append(divContent));
            //listObj.append($(liOverview).append($(divOverviewWrapper).append($(hrefOverview).append($('<div />', {}).append(divTitle)))));
            $('.div-promo-row > a > div:last-child > div').css({ maxWidth: ($(window).width() - 200) + 'px' });
            $(this).find('script').each(function () { $.globalEval(this.text || this.textContent || this.innerHTML || ''); });


            //if (location.hash != '') {
            //    $(location.hash).next().slideToggle();
            //    var divObj = $(location.hash).find('div')[1];
            //    if ($(divObj).css('background-image').indexOf('arrow-up') > 0) { $(divObj).css('background-image', "url('/_Static/Images/arrow-down.png')"); }
            //    else { $(divObj).css('background-image', "url('/_Static/Images/arrow-up.png')"); }
            //}
        }

        function getSubAffiliate() {

            //var listObj = $("#divAffInfo").append('<ul></ul>').find('ul');
            var listObj = $("#divAffInfo").find('ul');
            var liOverview = $('<li />');
            //var divOverviewWrapper = $('<div />', { id: $(this).attr('id'), class: 1 % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
            var divOverviewWrapper = $('<div />', { id: 4, class: 1 % 2 == 0 ? 'div-promo-row' : 'div-promo-row div-promo-row-alt' });
            var hrefOverview = $('<a />', { href: "javascript:void(0)", onclick: "javascript:OpenOverviewDetails(4);" });

            //Sub Affiliate
            var divTitle = $('<div />', { class: 'div-overview-header' }).text('<%=commonCulture.ElementValues.getResourceString("lblSubAff", xeResources)%>');
            var divContent = $('<div />', { class: 'div-overview-content' }).html('<% =SubAffiliate.ToString() %> <% =SubAffiliateProduct.ToString() %>');

            listObj.append($(liOverview).append($(divOverviewWrapper).append($(hrefOverview).append($('<div />', {}).append(divTitle)))).append(divContent));
            //listObj.append($(liOverview).append($(divOverviewWrapper).append($(hrefOverview).append($('<div />', {}).append(divTitle)))));
            $('.div-promo-row > a > div:last-child > div').css({ maxWidth: ($(window).width() - 200) + 'px' });
            $(this).find('script').each(function () { $.globalEval(this.text || this.textContent || this.innerHTML || ''); });


            //if (location.hash != '') {
            //    $(location.hash).next().slideToggle();
            //    var divObj = $(location.hash).find('div')[1];
            //    if ($(divObj).css('background-image').indexOf('arrow-up') > 0) { $(divObj).css('background-image', "url('/_Static/Images/arrow-down.png')"); }
            //    else { $(divObj).css('background-image', "url('/_Static/Images/arrow-up.png')"); }
            //}
        }
         
        function OpenOverviewDetails(obj) {
            //var selected_promo_id = $(obj).parent().attr('id');
            var selected_promo_id = obj

            $('.div-promo-row').each(function () {
                if ($(this).attr('id') != selected_promo_id) {
                    $(this).find('a > div:last-child').css('background-image', "url('/_Static/Images/arrow-down.png')");
                    $(this).next().slideUp();
                }
                else {
                    if ($(this).find('a > div:last-child').css('background-image').indexOf('arrow-up') > 0){$(this).find('a > div:last-child').css('background-image', "url('/_Static/Images/arrow-down.png')");}
                    else{$(this).find('a > div:last-child').css('background-image', "url('/_Static/Images/arrow-up.png')");}
                    $(this).next().slideToggle();
                }
            });
        }

    </script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page" data-theme="b" data-ajax="false">
        <!--#include virtual="~/_static/header.shtml" -->

        <div class="ui-content" role="main">
            <img id="promoLoader" src="/_Static/Css/images/ajax-loader.gif" style="display: none;" />
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("overview", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content div-promotions-wrapper" id="divAffInfo"></div>
        </div>

        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
    </div>
    <!-- /page -->
</body>
</html>
