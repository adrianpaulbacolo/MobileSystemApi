<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountInfo.aspx.cs" Inherits="_Secure_AccountInfo" %>

<!DOCTYPE html>
<html>
<head>
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("myAccount", commonVariables.LeftMenuXML)%></title>
    <!--#include virtual="~/_static/head.inc" -->
   <%-- <script type="text/javascript" src="/_Static/Js/PreLoad.js"></script>--%>
     <script type="text/javascript" src="/_Static/Js/Main.js"></script>
    <link type="text/css" rel="stylesheet" href="/_Static/Css/MyAccount.css" />
</head>
<body>
    <!--#include virtual="~/_static/splash.shtml" -->
    <div id="divMain" data-role="page">
        <!--#include virtual="~/_static/header.shtml" -->
        <div class="ui-content" role="main">
             <img id="promoLoader" src="/_Static/Css/images/ajax-loader.gif" style="display: none;" />
            <div class="div-page-header"><span><%=commonCulture.ElementValues.getResourceString("myAccount", commonVariables.LeftMenuXML)%></span></div>
            <div class="page-content">
                <form id="form1" runat="server" data-ajax="false">
                <div class="div-content-wrapper">

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="Label3" Visible ="false" />
                        </div>
                        <div>
                            <span><%=commonCulture.ElementValues.getResourceString("accountinfo", commonVariables.LeftMenuXML)%></span>
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblFullName" runat="server" CssClass="Notmandatory" />
                        </div>
                        <div>
                            <asp:Label ID="lblMemberFullName" runat="server" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblUsername" runat="server" CssClass="Notmandatory" />
                        </div>
                        <div>
                            <asp:Label ID="lblMemberUsername" runat="server" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblEmail" runat="server" CssClass="Notmandatory" />
                        </div>
                        <div>
                            <asp:Label ID="lblMemberEmail" runat="server" />
                        </div>
                    </div>

                    <div class="div-register-dob">
                        <div><asp:Label ID="lblDOB" runat="server" CssClass="mandatory" AssociatedControlID="drpDay" Text="" /></div>
                        <div>
                            <div><asp:DropDownList ID="drpDay" runat="server"/></div>
                            <div><asp:DropDownList ID="drpMonth" runat="server"/></div>
                            <div><asp:DropDownList ID="drpYear" runat="server"/></div>
                        </div>                            
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblCountry" runat="server" CssClass="mandatory" />
                        </div>
                        <div>
                            <asp:DropDownList ID="drpCountry" runat="server" data-mini="true" />
                        </div>
                    </div>

                    <div class="div-register-normal2">
                        <div>
                            <asp:Label ID="lblContact" runat="server" AssociatedControlID="drpContactCountry" Text="" CssClass="mandatory" />
                        </div>
                        <div>
                            <div><asp:DropDownList ID="drpContactCountry" runat="server" data-icon="false" data-mini="true" /></div>                                
                            <div><asp:TextBox ID="txtContact" runat="server" type="tel" placeholder=" " data-mini="true" data-clear-btn="true" /></div>        
                        </div>                        
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblAccount" runat="server" CssClass="Notmandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtAccount" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblAddress" runat="server" CssClass="Notmandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtAddress" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                        </div>
                    </div>

                    <div class="div-register-normal3">
                        <div>
                            <asp:Label ID="lblCity" runat="server" AssociatedControlID="txtCity" Text="" CssClass="Notmandatory" />
                        </div>        
                        <div>                
                            <div>
                                <asp:TextBox ID="txtCity" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                            </div>                        
                            <div>                        
                                <asp:TextBox ID="txtPostal" Width="" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                            </div>
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblLanguage" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                            <asp:DropDownList ID="drpLanguage" runat="server" data-mini="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblCurrency" runat="server" CssClass="Notmandatory" />
                        </div>
                        <div>
                            <asp:Label ID="lblMemberCurrency" runat="server" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblCommissionType" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                           <asp:DropDownList ID="drpCommissionType" runat="server" data-mini="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblSecQues" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                           <asp:DropDownList ID="drpSecQues" runat="server" data-mini="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblSecAns" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtSecAns" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                        </div>
                    </div>
                    
                    <div class="div-register-websiteUrl">
                        <div><asp:Label ID="lblWebsiteUrl" runat="server" AssociatedControlID="txtURL1" Text="DOB" CssClass="Notmandatory" /></div>
                        <div>
                            <div>
                                <asp:Label ID="lblURL1" runat="server" AssociatedControlID="txtURL1" Text="URL1" CssClass="ui-hidden-accessible" />
                                <asp:TextBox ID="txtURL1" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                            </div>
                            <div>
                                <asp:Label ID="lblURL2" runat="server" AssociatedControlID="txtURL2" Text="URL2" CssClass="ui-hidden-accessible" />
                                <asp:TextBox ID="txtURL2" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                            </div>
                            <div>
                                <asp:Label ID="lblURL3" runat="server" AssociatedControlID="txtURL3" Text="City" CssClass="ui-hidden-accessible" />
                                <asp:TextBox ID="txtURL3" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                            </div>
                        </div>                            
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblBankAccName" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtBankAccName" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblBankAccNo" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtBankAccNo" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblSwiftCode" runat="server" CssClass="Notmandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtSwiftCode" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblBankName" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtBankName" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="lblBankAdd" runat="server" CssClass="mandatory" />
                        </div>                        
                        <div>
                            <asp:TextBox ID="txtBankAdd" runat="server" placeholder=" " data-mini="true" data-clear-btn="true" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="Label1" runat="server" Visible="false" />
                        </div> 
                        <div>
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="button-blue" data-corners="false" OnClick="btnUpdate_Click" />
                        </div>
                    </div>

                    <div class="div-register-normal">
                        <div>
                            <asp:Label ID="Label2" runat="server" Visible="false" />
                        </div> 
                        <div>
                            <a data-theme="c" ID="btnCancel" runat="server" Text="cancel" class="ui-btn" data-corners="false" data-ajax="false" href="/Index" />
                        </div>
                     </div>
                    <asp:HiddenField id="hidValues" runat="server" />
                	<asp:HiddenField runat="server" ID="ioBlackBox" Value="" />
                </div>
                </form>
            </div>
        </div>
        <!-- /content -->
        <!--#include virtual="~/_static/footer.shtml" -->
        <!--#include virtual="~/_static/navMenu.shtml" -->
        <script type="text/javascript">

            var strContactCountry = '';

            $(function () {
                $("#drpContact").attr("disabled", "disabled").off('click');
                $("#drpDOB").attr("disabled", "disabled").off('click');

                $.ajax({
                    contentType: "application/json; charset=utf-8",
                    url: "http://w88uat.com/IP2LOC?v=" + new Date().getTime(),
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
                            window.location.reload(true);
                            //window.location.replace('/Index.aspx');
                            break;
                        default:
                            break;
                    }
                }
            });

            //$('#imgCaptcha').click(function () { $(this).attr('src', '/Captcha'); });

            $('#form1').submit(function (e) {

                if ($('#drpCountry').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingCountryCode", xeErrors)%>');
                            $('#btnUpdate').attr("disabled", false);
                            e.preventDefault();
                            return;
                }
                else if ($('#txtContact').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%>');
                     $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtContact').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if (!$('#txtContact').val().trim().match('([0-9]{6,12})$')) {
                    //else if (isNaN($('#txtContact').val())) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }

                else if ($('#drpLanguage').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingLanguageCode", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#drpCommissionType').val() == '-1') {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/MissingCommissionType", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtSecAns').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingSecurityAnswer", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtSecAns').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidSecurityAnswer", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtBankAccName').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingBankAccName", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtBankAccName').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankAccName", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtBankAccNo').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingBankAccNo", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtBankAccNo').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankAccNo", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtBankName').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingBankName", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtBankName').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankName", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtBankAdd').val().trim().length == 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingBankAdd", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else if ($('#txtBankAdd').val().trim().indexOf(' ') >= 0) {
                    alert('<%=commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankAdd", xeErrors)%>');
                    $('#btnUpdate').attr("disabled", false);
                    e.preventDefault();
                    return;
                }
                else {
                    //if ($('#txtContact').val().indexOf('-') > 0) {
                    //var strContact = $('#txtContact').val().substring($('#txtContact').val().indexOf('-') + 1, $('#txtContact').val().length);

                    if (strContact.length < 6 || strContact.length > 12) {
                        alert('<%=commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors)%>');
                        $('#btnUpdate').attr("disabled", false);
                        e.preventDefault();
                        return;
                    }
                        //}
                    else {
                        GPINTMOBILE.ShowSplash();
                        $.ajax({
                            contentType: "application/json; charset=utf-8",
                            url: "http://w88uat.com/IP2LOC?v=" + new Date().getTime(),
                            dataType: "jsonp",
                            success: function (data) {
                                //initiateLogin(data);
                                $('#btnUpdate').attr("disabled", false);
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