<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="_Secure_Register" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("register", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/JS/PreLoad.js"></script>
</head>
<body>
    <div data-role="page" id="register">

        <header id="header" data-role="header" data-position="fixed" data-theme="b" data-tap-toggle="false">
            <a href="" role="button" data-rel="back" class="btn-clear ui-btn-left ui-btn" id="aMenu" data-load-ignore-splash="true">
                <span class="icon icon-back"></span>
                <%=commonCulture.ElementValues.getResourceString("back", commonVariables.LeftMenuXML)%>
            </a>
            <h1 class="title"><%=commonCulture.ElementValues.getResourceString("register", commonVariables.LeftMenuXML)%></h1>
        </header>

        <div class="ui-content" role="main">
            <form class="form" id="form1" runat="server" data-ajax="false">
                <ul class="list fixed-tablet-size">
                    <li class="item item-text-wrap text-center">
                        <%=commonCulture.ElementValues.getResourceString("reminder1", commonVariables.LeftMenuXML)%><br>
                        <%=commonCulture.ElementValues.getResourceString("reminder2", commonVariables.LeftMenuXML)%>
                    </li>
                    <li class="item item-icon-left item-input" data-mini="true">
                        <i class="icon icon-profile"></i>
                        <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" Text="username" />
                        <asp:TextBox ID="txtUsername" runat="server" data-mini="true" MaxLength="16" data-clear-btn="true" />
                    </li>
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-password"></i>
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="password" />
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" data-mini="true" MaxLength="10" data-clear-btn="true" />
                    </li>
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-password"></i>
                        <asp:Label ID="lblConfirmPassword" runat="server" AssociatedControlID="txtConfirmPassword" Text="password" />
                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" data-mini="true" MaxLength="10" data-clear-btn="true" />
                    </li>
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-mail"></i>
                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" />
                        <asp:TextBox ID="txtEmail" runat="server" data-mini="true" type="email" data-clear-btn="true" />
                    </li>
                    <li class="item item-icon-left item-select">
                        <i class="icon icon-phone"></i>
                        <asp:Label ID="lblContact" runat="server" AssociatedControlID="txtContact" />
                        <div class="row">
                            <div class="col col-25">
                                <asp:DropDownList ID="drpContactCountry" runat="server" data-icon="false" data-mini="true" />
                            </div>
                            <div class="col col-75">
                                <asp:TextBox ID="txtContact" runat="server" type="tel" data-mini="true" data-clear-btn="true" />
                            </div>
                        </div>
                    </li>
                    <li class="item item-icon-left item-select">
                        <i class="icon icon-currency"></i>
                        <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="drpCurrency" />
                        <asp:DropDownList ID="drpCurrency" runat="server" data-mini="true" />
                    </li>
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-profile"></i>
                        <div class="row">
                            <%-- <div class="col">
                                <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" />
                                <asp:TextBox ID="txtFirstName" runat="server" data-mini="true" MaxLength="12" data-clear-btn="true" />
                            </div>
                            <div class="col">
                                <asp:Label ID="lblLastName" runat="server" AssociatedControlID="txtLastName" />
                                <asp:TextBox ID="txtLastName" runat="server" data-mini="true" data-clear-btn="true" />
                            </div>--%>
                            <div class="col">
                                <asp:Label ID="lblName" runat="server" AssociatedControlID="txtName" />
                                <asp:TextBox ID="txtName" runat="server" data-mini="true" MaxLength="50" data-clear-btn="true" />
                            </div>
                        </div>
                    </li>
                    <li class="item item-icon-left">
                        <div class="row item-text-wrap">
                            <asp:Label ID="lblNote" runat="server" />
                        </div>
                    </li>
                    <li class="item item-icon-left item-select">
                        <i class="icon icon-event"></i>
                        <asp:Label ID="lblDOB" runat="server" AssociatedControlID="drpDay" />
                        <div class="row">
                            <div class="col">
                                <asp:DropDownList ID="drpDay" runat="server" />
                            </div>
                            <div class="col">
                                <asp:DropDownList ID="drpMonth" runat="server" />
                            </div>
                            <div class="col">
                                <asp:DropDownList ID="drpYear" runat="server" />
                            </div>
                        </div>
                    </li>
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-referral-bonus"></i>
                        <asp:Label ID="lblAffiliateID" runat="server" AssociatedControlID="txtAffiliateID" />
                        <asp:TextBox ID="txtAffiliateID" runat="server" data-mini="true" type="number" />
                    </li>
                   <%-- <li class="item item-icon-left item-input">
                        <i class="icon icon-security"></i>
                        <asp:Label ID="lblCaptcha" runat="server" AssociatedControlID="txtCaptcha" Text="code" />
                        <asp:Image ID="imgCaptcha" runat="server" ImageUrl="/Captcha" CssClass="imgCaptcha" />
                        <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" type="tel" data-mini="true" data-clear-btn="true" />
                    </li>--%>
                    <li class="item-checkbox item-text-wrap">
                        <label id="lblDisclaimer" runat="server">I agree</label>
                        <a id="btnTermsConditionsLink" runat="server" href="" data-ajax="false" target="_blank"></a>
                    </li>
                    <li class="item row">
                        <div class="col">
                            <a class="ui-btn btn-bordered" id="btnCancel" runat="server" text="cancel" data-corners="false" data-ajax="false" href="/Index" />
                        </div>
                        <div class="col">
                            <asp:Button ID="btnSubmit" runat="server" Text="submit" data-corners="false" OnClick="btnSubmit_Click" />
                        </div>
                    </li>
                </ul>
                <asp:HiddenField ID="hidValues" runat="server" />
                <asp:HiddenField runat="server" ID="ioBlackBox" Value="" />
            </form>
        </div>
        <!-- /content -->
        <script type="text/javascript">

            var strContactCountry = '';
            var CDNCountry = '<%= (!String.IsNullOrEmpty(CDNCountryCode)) ? CDNCountryCode : "" %>';
            var domain = '<%= (!String.IsNullOrEmpty(headers.host)) ? headers.host : "" %>';

            function setCurrency(country) {
                switch (country.toString().toUpperCase()) {
                    case "SG":
                        if ($('#drpCurrency option[value="UUS"]').length > 0) { $('#drpCurrency').val('UUS'); }
                        break;
                    case 'CN':
                        if ($('#drpCurrency option[value="RMB"]').length > 0) { $('#drpCurrency').val('RMB'); }
                        break;
                    case "TH":
                        if ($('#drpCurrency option[value="THB"]').length > 0) { $('#drpCurrency').val('THB'); }
                        break;
                    case "VN":
                        if ($('#drpCurrency option[value="VND"]').length > 0) { $('#drpCurrency').val('VND'); }
                        break;
                    case "ID":
                        if ($('#drpCurrency option[value="IDR"]').length > 0) { $('#drpCurrency').val('IDR'); }
                        break;
                    case "MY":
                        if ($('#drpCurrency option[value="MYR"]').length > 0) { $('#drpCurrency').val('MYR'); }
                        break;
                    case "KR":
                        if ($('#drpCurrency option[value="KRW"]').length > 0) { $('#drpCurrency').val('KRW'); }
                        break;
                    default:
                        return false;
                        break;
                }
                $('#drpCurrency').change();
                return true;
            }

            $(function () {
                $("#drpContact").attr("disabled", "disabled").off('click');
                $("#drpDOB").attr("disabled", "disabled").off('click');
                if (isValidCountry(CDNCountry) && setCurrency(CDNCountry)) {
                    $('#hidValues').val(CDNCountry + "|" + domain + "|" + "<%= !String.IsNullOrEmpty(headers.ip) ? headers.ip : commonIp.UserIP %>" + "|");
                } else {
                    $.ajax({
                        contentType: "application/json; charset=utf-8",
                        url: "https://ip2loc.w2script.com/IP2LOC?v=" + new Date().getTime(),
                        dataType: "jsonp",
                        success: function (data) {

                            var ipCountry = data.country.toString().toUpperCase();

                            if ($('#hidValues').val().trim().length == 0) {
                                setCurrency(ipCountry);

                                $.ajax({
                                    type: "POST",
                                    url: "/AjaxHandlers/GetCountryInfo.ashx",
                                    data: { CountryCode: ipCountry },
                                    success: function (data) {
                                        strContactCountry = data;

                                        if ($.trim(data).trim().length > 0) { $('#drpContactCountry').val(strContactCountry).change(); }
                                        return;
                                    },
                                    error: function (err) { }
                                });
                            }

                            $('#hidValues').val(ipCountry + "|" + data.domainName + "|" + data.ip + "|" + (data.permission == '' ? '-' : data.permission));

                            return;
                        },
                        error: function (err) {
                            //window.location.href = '/Default.aspx';
                        }
                    });

                }

                var responseMsg = '<%=strAlertMessage%>';
                var responseCode = '<%=strAlertCode%>';
                if (responseCode != "1" && responseMsg.length > 0) {
                    window.w88Mobile.Growl.shout(responseMsg);
                }
            });

            function isValidCountry(country) {
                return (country.length > 0 && country.toUpperCase() != "XX")
            }

            $('#imgCaptcha').click(function () { $(this).attr('src', '/Captcha'); });

            $('#form1').submit(function (e) {

                var message = ('<ul>');
                var hasError = false;

                if ($('#txtUsername').val().trim().length == 0) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/MissingUsername", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }
                else if ($('#txtUsername').val().trim().length < 5 || $('#txtUsername').val().trim().length > 16) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidUsername", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }
                else if (!/^[a-zA-Z0-9]+$/.test($('#txtUsername').val().trim())) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidUsername", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }
                else if ($('#txtUsername').val().trim().indexOf(' ') >= 0) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidUsername", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }

                if ($('#txtPassword').val().trim().length < 8 || $('#txtPassword').val().trim().length > 10) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidPassword", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }
                else if ($('#txtPassword').val().trim().indexOf(' ') >= 0) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidPassword", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                } else if ($('#txtPassword').val().trim() != $('#txtConfirmPassword').val().trim()) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidConfirmPass", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }

                if ($('#txtEmail').val().trim().length == 0) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/MissingEmail", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }
                else if ($('#txtEmail').val().trim().indexOf(' ') >= 0) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidEmail", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }
                else if (!EmailValidation($('#txtEmail').val())) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidEmail", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }

                if ($('#txtContact').val().trim().length == 0) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }
                else if ($('#txtContact').val().trim().indexOf(' ') >= 0) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }
                else if (!$('#txtContact').val().trim().match('([0-9]{6,12})$')) {
                    //else if (isNaN($('#txtContact').val())) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }

                if ($('#txtName').val().trim().length == 0) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/MissingName", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }

                if (!CheckDob()) {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/Required18", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }

                if ($('#drpCurrency').val() == '-1') {
                    message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/MissingCurrency", xeErrors)%></li>');
                    hasError = true;
                    e.preventDefault();
                }
                

                if (hasError) {
                    message += ('</ul>');
                    $('#btnSubmit').attr("disabled", false);
                    window.w88Mobile.Growl.shout(message);
                    return;
                } else {
                    GPINTMOBILE.ShowSplash();
                    $('#btnSubmit').attr("disabled", false);
                }
            });

            function EmailValidation(value) {
                if (value.length > 0) {
                    var regExEmail = /\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*/;
                    return regExEmail.test(value);
                } else
                    return false;
            }

            function CheckDob() {
                try {
                    var item = new Date($("#<%=drpYear.ClientID%>").val() + "-" + pad($("#<%=drpMonth.ClientID%>").val(), 2) + "-" + pad($("#<%=drpDay.ClientID%>").val(), 2));
                    var today = new Date();
                    
                    $("#<%=drpYear.ClientID%>").val(item.getFullYear());
                    $("#<%=drpMonth.ClientID%>").val(item.getMonth() + 1);
                    $("#<%=drpDay.ClientID%>").val(item.getDate());

                    if ((today - item) / 1000 / 3600 / 24 / 365 < 18)
                        return false;
                    else
                        return true;
                } catch (e) {
                    return false;
                }
            }

            function pad(num, size) {
                var s = num + "";
                while (s.length < size) s = "0" + s;
                return s;
            }

        </script>

    </div>
    <!-- /page -->
</body>
</html>
