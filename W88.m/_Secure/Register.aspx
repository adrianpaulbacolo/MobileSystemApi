<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="_Secure_Register" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("register", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page" id="register">

        <header id="header" data-role="header" data-position="fixed" data-theme="b" data-tap-toggle="false">
            <a href="" role="button" data-rel="back" class="btn-clear ui-btn-left ui-btn ion-ios-arrow-back" id="aMenu" data-load-ignore-splash="true">
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
                        <i class="icon icon-mail"></i>
                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="email" />
                        <asp:TextBox ID="txtEmail" runat="server" data-mini="true" type="email" data-clear-btn="true" />
                    </li>
                    <li class="item item-icon-left item-select">
                        <i class="icon icon-phone"></i>
                        <asp:Label ID="lblContact" runat="server" AssociatedControlID="txtContact" Text="email" />
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
                        <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="drpCurrency" Text="currency" />
                        <asp:DropDownList ID="drpCurrency" runat="server" data-mini="true" />
                    </li>
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-profile"></i>
                        <div class="row">
                            <div class="col">
                                <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="fName" />
                                <asp:TextBox ID="txtFirstName" runat="server" data-mini="true" MaxLength="12" data-clear-btn="true" />
                            </div>
                            <div class="col">
                                <asp:Label ID="lblLastName" runat="server" AssociatedControlID="txtLastName" Text="lName" />
                                <asp:TextBox ID="txtLastName" runat="server" data-mini="true" data-clear-btn="true" />
                            </div>
                        </div>
                    </li>
                    <li class="item item-icon-left item-select">
                        <i class="icon icon-event"></i>
                        <asp:Label ID="lblDOB" runat="server" AssociatedControlID="drpDay" Text="DOB" />
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
                        <asp:Label ID="lblAffiliateID" runat="server" AssociatedControlID="txtAffiliateID" Text="email" />
                        <asp:TextBox ID="txtAffiliateID" runat="server" data-mini="true" type="number" />
                    </li>
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-security"></i>
                        <asp:Label ID="lblCaptcha" runat="server" AssociatedControlID="txtCaptcha" Text="code" />
                        <asp:Image ID="imgCaptcha" runat="server" ImageUrl="/Captcha" CssClass="imgCaptcha" />
                        <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" type="tel" data-mini="true" data-clear-btn="true" />
                    </li>
                    <li class="item-checkbox item-text-wrap">
                        <label id="lblDisclaimer" runat="server">I agree</label>
                        <a id="btnTermsConditionsLink" runat="server" href="https://info.w88live.com/termofuse_en.shtml" data-ajax="false" target="_blank"></a>
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
                <asp:HiddenField id="hidValues" runat="server" />
                <asp:HiddenField runat="server" ID="ioBlackBox" Value="" />
            </form>
        </div>
        <!-- /content -->
        <script type="text/javascript">

            var strContactCountry = '';
            var CDNCountry = '<%= (!String.IsNullOrEmpty(CDNCountryCode)) ? CDNCountryCode : "" %>';

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
                }
                $('#drpCurrency').change();
            }

            $(function () {
                $("#drpContact").attr("disabled", "disabled").off('click');
                $("#drpDOB").attr("disabled", "disabled").off('click');
                if(CDNCountry.length > 0){
                    setCurrency(CDNCountry);
                    $('#hidValues').val(CDNCountry + "|||");
                }else{
                    $.ajax({
                        contentType: "application/json; charset=utf-8",
                        url: "https://ip2loc.w2script.com/IP2LOC?v=" + new Date().getTime(),
                        dataType: "jsonp",
                        success: function (data) {
                            if ($('#hidValues').val().trim().length == 0) {
                                setCurrency(data.country);

                                $.ajax({
                                    type: "POST",
                                    url: "/AjaxHandlers/GetCountryInfo.ashx",
                                    data: { CountryCode: data.country.toString().toUpperCase() },
                                    success: function (data) {
                                        strContactCountry = data;
                                    
                                        if ($.trim(data).trim().length > 0) { $('#drpContactCountry').val(strContactCountry).change(); }
                                        return;
                                    },
                                    error: function (err) { }
                                });
                            }

                            $('#hidValues').val(data.country.toString().toUpperCase() + "|" + data.domainName + "|" + data.ip + "|" + (data.permission == '' ? '-' : data.permission));

                            return;
                        },
                        error: function (err) {
                            //window.location.href = '/Default.aspx';
                        }
                    });

                }

                if ('<%=strAlertCode%>'.length > 0) {
                    switch ('<%=strAlertCode%>') {
                        case '-1':
                            alert('<%=strAlertMessage%>');
                            break;
                        case '0':
                            alert('<%=strAlertMessage%>');
                            break;
                        case '1':
                            alert('<%=strAlertMessage%>');
                            window.location.replace('/Index.aspx?lang=<%=commonVariables.SelectedLanguage.ToLower()%>');
                            break;
                        default:
                            break;
                    }
                }
            });

            $('#imgCaptcha').click(function () { $(this).attr('src', '/Captcha'); });

            $('#form1').submit(function (e) {
                if ($('#txtUsername').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingUsername", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtUsername').val().trim().length < 5 || $('#txtUsername').val().trim().length > 16) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidUsername", xeErrors)%>');
                     $('#btnSubmit').attr("disabled", false);
                     e.preventDefault();
                     return;
                 }
                 else if (!/^[a-zA-Z0-9]+$/.test($('#txtUsername').val().trim()))
                {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidUsername", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtUsername').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidUsername", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtPassword').val().trim().length < 8 || $('#txtPassword').val().trim().length > 10) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidPassword", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtPassword').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidPassword", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtEmail').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingEmail", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtEmail').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidEmail", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtContact').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtContact').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if (!$('#txtContact').val().trim().match('([0-9]{6,12})$')) {
                    //else if (isNaN($('#txtContact').val())) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtFirstName').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingFName", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtLastName').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingLName", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtCaptcha').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceString("MissingVCode", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#drpCurrency').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingCurrency", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if (!EmailValidation($('#txtEmail').val())) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidEmail", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else {
                    if (strContact.length < 6 || strContact.length > 12) {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%>');
                        $('#btnSubmit').attr("disabled", false);
                        e.preventDefault();
                        return;
                    }
                    else {
                        GPINTMOBILE.ShowSplash();
                        
                        $('#btnSubmit').attr("disabled", false);
                    }
                }
            });

            function EmailValidation(value) {
                if (value.length > 0) {
                    var regExEmail = /^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$/;
                    return regExEmail.test(value);
                } else
                    return false;
            }
        </script>

        <script type="text/javascript" id="iovs_script">
        	var io_operation = 'ioBegin';
        	var io_bbout_element_id = 'ioBlackBox';
        	//var io_submit_element_id = 'btnSubmit';
        	var io_submit_form_id = 'form1';
        	var io_max_wait = 5000;
        	var io_install_flash = false;
        	var io_install_stm = false;
        	var io_exclude_stm = 12;
        </script>
        <script type="text/javascript" src="//mpsnare.iesnare.com/snare.js"></script>
    </div>
    <!-- /page -->
</body>
</html>
