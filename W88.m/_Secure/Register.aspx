<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="_Secure_Register" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("register", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
    <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>
    <link type="text/css" rel="stylesheet" href="/_Static/Css/Register.css" />
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div data-role="page">
        <!--#include virtual="~/_static/headerLogoOnlyFixed.inc" -->
        <div class="ui-content" role="main">
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("register", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content">
                <form id="form1" runat="server" data-ajax="false">
                <div class="div-content-wrapper">
                    <div class="ui-field-contain ui-hide-label" data-mini="true" >
                        <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" Text="username" CssClass="ui-hidden-accessible" />
                        <asp:TextBox ID="txtUsername" runat="server" placeholder="username" data-mini="true" MaxLength="16" data-clear-btn="true" />
                    </div>
                    <div class="ui-field-contain ui-hide-label">
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="password" CssClass="ui-hidden-accessible" />
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="password" data-mini="true" MaxLength="10" data-clear-btn="true" />
                    </div>
                    <div class="ui-field-contain ui-hide-label">
                        <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" Text="email" CssClass="ui-hidden-accessible" />
                        <asp:TextBox ID="txtEmail" runat="server" placeholder="email" data-mini="true" type="email" data-clear-btn="true" />
                    </div>
                    <div class="div-register-contact">
                        <div><asp:Label ID="lblContact" runat="server" AssociatedControlID="txtContact" Text="email" CssClass="ui-hidden-accessible" /></div>
                        <div><asp:DropDownList ID="drpContactCountry" runat="server" data-icon="false" data-mini="true" /></div>                                
                        <div><asp:TextBox ID="txtContact" runat="server" type="tel" placeholder="contact" data-mini="true" data-clear-btn="true" /></div>                                
                    </div>
                    <div class="ui-field-contain ui-hide-label">
                        <asp:Label ID="lblCurrency" runat="server" AssociatedControlID="drpCurrency" Text="currency" CssClass="ui-hidden-accessible" />
                        <asp:DropDownList ID="drpCurrency" runat="server" data-mini="true" />
                    </div>
                    <div class="div-register-firstlastname">
                        <div>
                            <div><asp:Label ID="lblFirstName" runat="server" AssociatedControlID="txtFirstName" Text="fName" CssClass="ui-hidden-accessible" /></div>
                            <div><asp:TextBox ID="txtFirstName" runat="server" data-mini="true" MaxLength="12" data-clear-btn="true" /></div>
                        </div>
                        <div>
                            <div><asp:Label ID="lblLastName" runat="server" AssociatedControlID="txtLastName" Text="lName" CssClass="ui-hidden-accessible" /></div>
                            <div><asp:TextBox ID="txtLastName" runat="server" data-mini="true" data-clear-btn="true" /></div>
                        </div>
                    </div>
                    <div class="div-register-dob">
                        <div><asp:Label ID="lblDOB" runat="server" AssociatedControlID="drpDay" Text="DOB" /></div>
                        <div>
                            <div><asp:DropDownList ID="drpDay" runat="server"/></div>
                            <div><asp:DropDownList ID="drpMonth" runat="server"/></div>
                            <div><asp:DropDownList ID="drpYear" runat="server"/></div>
                        </div>                            
                    </div>
                    <div class="div-affiliateid ui-field-contain ui-hide-label">
                        <asp:Label ID="lblAffiliateID" runat="server" AssociatedControlID="txtAffiliateID" Text="email" CssClass="ui-hidden-accessible" />
                        <asp:TextBox ID="txtAffiliateID" runat="server" placeholder="affiliate" data-mini="true" type="number" />
                    </div>
                    <div class="div-register-captcha">
                        <asp:Label ID="lblCaptcha" runat="server" AssociatedControlID="txtCaptcha" Text="code" CssClass="ui-hidden-accessible ui-block-a" />
                        <div class="ui-grid-a">
                            <div class="ui-block-a"><asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" type="tel" data-mini="true" data-clear-btn="true" /></div>
                            <div class="ui-block-b"><asp:Image ID="imgCaptcha" runat="server" ImageUrl="/Captcha" CssClass="imgCaptcha" /></div>
                        </div>
                    </div>
                    <div>
                        <input type="checkbox" name="checkbox-mini-0" id="chkDisclaimer" runat="server" class="chk-disclaimer" data-theme="c">
                        <label id="lblDisclaimer" runat="server" for="chkDisclaimer">I agree</label>
                    </div>
                    <%--<div class="div-register-disclaimer">
                        <div><label id="lblDisclaimer" runat="server" for="chkDisclaimer">I agree</label></div>
                        <div><asp:DropDownList ID="drpDisclaimer" runat="server" data-role="flipswitch" data-corners="false" CssClass="ui-mini" /></div>
                    </div>--%>

                    <div>
                        <asp:Button ID="btnSubmit" runat="server" Text="submit" CssClass="button-blue" data-corners="false" OnClick="btnSubmit_Click" />
                    </div>
                    <div>
                        <a data-theme="c" ID="btnCancel" runat="server" Text="cancel" class="ui-btn" data-corners="false" data-ajax="false" href="/Index" />
                    </div>
                    <asp:HiddenField id="hidValues" runat="server" />
                	<asp:HiddenField runat="server" ID="ioBlackBox" Value="" />
                </div>
                </form>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <script type="text/javascript">

            var strContactCountry = '';

            $(function () {
                $("#drpContact").attr("disabled", "disabled").off('click');
                $("#drpDOB").attr("disabled", "disabled").off('click');

                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    url: "http://w88uat.com/ip2loc?v=" + new Date().getTime(),
                    dataType: "jsonp",
                    success: function (data) {
                        if ($('#hidValues').val().trim().length == 0) {
                            switch (data.country.toString().toUpperCase())
                            {
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

                            $.ajax({
                                type: "POST",
                                url: "/AjaxHandlers/GetCountryInfo.ashx",
                                data: { CountryCode: data.country.toString().toUpperCase() },
                                success: function (data) {
                                    strContactCountry = data;
                                    //if ($.trim(data).length > 0) { $('#txtContact').val('+' + strContactCountry + '-'); }
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
                            window.location.replace('/Index.aspx');
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
                else if (!$('#chkDisclaimer').is(':checked')) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/chkDisclaimer", xeErrors)%>');
                    $('#btnSubmit').attr("disabled", false);
                    e.preventDefault();
                    return;
                }                    
                else {
                    //if ($('#txtContact').val().indexOf('-') > 0) {
                    //var strContact = $('#txtContact').val().substring($('#txtContact').val().indexOf('-') + 1, $('#txtContact').val().length);

                    if (strContact.length < 6 || strContact.length > 12) {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%>');
                        $('#btnSubmit').attr("disabled", false);
                        e.preventDefault();
                        return;
                    }
                        //}
                    else {
                        GPINTMOBILE.ShowSplash();
                        $.ajax({
                            contentType: "application/json; charset=utf-8",
                            url: "http://w88uat.com/ip2loc?v=" + new Date().getTime(),
                            dataType: "jsonp",
                            success: function (data) {
                                //initiateLogin(data);
                                $('#btnSubmit').attr("disabled", false);
                                //e.preventDefault();
                                //hideSplash();
                                //return;
                            },
                            error: function (err) {
                                //window.location.href = '/Default.aspx';
                            }
                        });
                    }
                }
                //e.preventDefault();
                //return;
            });

            function EmailValidation(value, element) {
                if (value.length > 0) {
                    var regExEmail = /^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$/;
                    return regExEmail.test(value);
                }
                else
                    return true;
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