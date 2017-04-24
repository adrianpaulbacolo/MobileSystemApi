<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="_Secure_Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width,height=device-height,initial-scale=1.0" />
    <title><%=commonCulture.ElementValues.getResourceString("brand", commonVariables.LeftMenuXML) + commonCulture.ElementValues.getResourceString("login", commonVariables.LeftMenuXML)%></title>


    <!-- Bootstrap -->
    <link href="/_Secure/VIP/js/jquery.modal.min.css" rel="stylesheet" />
    <link href="/_Static/css/style.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet">

    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->

    <script src="/_Static/JS/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="/_Static/JS/Mobile/jquery.mobile-1.4.5.min.js"></script>
    <script src="/_Secure/VIP/js/jquery.modal.min.js"></script>
    <script src="/_Static/JS/vendor/lodash.min.js"></script>
    <script type="text/javascript" src="/_Static/v2/assets/js/vendor/pubsub.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="/_Static/JS/Cookie.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="/_Static/JS/vendor/amplify.min.js"></script>

    <script type="text/javascript">
        window.w88Mobile = {}; 
        window.User = {};
        window.User.hasSession = <%= (!String.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) ? 1 : 0 %>;
        window.User.token = '<%= commonVariables.CurrentMemberSessionId %>';
        window.User.sessionInterval = '<%=ConfigurationManager.AppSettings.Get("sessionInterval") %>';
        window.User.lang = '<%=commonVariables.SelectedLanguage%>';
        window.User.storageExpiration = { expires: 1200000 };
    </script>

    <script src="/_Static/JS/i18n/contents-<%=commonVariables.SelectedLanguageShort%>.js"></script>
    <script src="/_Static/v2/assets/js/modules/translate.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script src="/_Static/v2/assets/js/loader.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

    <script>
        w88Mobile.APIUrl = '<%= ConfigurationManager.AppSettings.Get("APIUrl") %>';

        var _w88_contents = new w88Mobile.Translate();
        _w88_contents.init();
        
        w88Mobile.Loader.init();

        var siteCookie = new Cookies();
        //amplify clear
        amplify.clearStore = function() {
            $.each(amplify.store(), function (storeKey) {
                // Delete the current key from Amplify storage
                amplify.store(storeKey, null);
            });
        };
    
    </script>
    
    <script type="text/javascript" src="/_Static/JS/fingerprint2.min.js"></script>
    <script type="text/javascript" src="/_Static/JS/piwik.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/JS/modules/piwikManager.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/JS/modules/piwikConfig.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript" src="/_Static/JS/modules/interceptor.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    <script type="text/javascript">
    w88Mobile.PiwikManager.setGoals(window.w88Mobile.PiwikConfig.goals);
    w88Mobile.PiwikManager.setDomain();
    w88Mobile.PiwikManager.setUserId('<%=UserSession.MemberId %>');

    $(function () {
        if(_.isEmpty(siteCookie.getCookie("fingerprint"))){
            new Fingerprint2().get(function (result, components) {
                var domain = "." + location.hostname.split('.').slice(-2).join('.');
                siteCookie.setCookie("fingerprint", result, 5, domain);
            });
        }else{
            var deviceObj = {
                index: 1
                , name: "deviceId"
                , value: siteCookie.getCookie("fingerprint")
                , scope: "visit"
            }
            w88Mobile.PiwikManager.setDeviceId(deviceObj);
        }
    });
    </script>

    <script src="/_Static/v2/assets/js/products.js"></script>
</head>
<body>
    <div data-role="page" data-close-btn="right" data-corners="false" id="login">
        <!--#include virtual="~/_static/logoOnly.inc" -->

        <div class="ui-content" role="main">
            <form class="form" id="form1" runat="server" data-ajax="false">
                <ul class="list fixed-tablet-size">
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-profile"></i>
                        <asp:Label ID="lblUsername" runat="server" AssociatedControlID="txtUsername" Text="Username" />
                        <asp:TextBox ID="txtUsername" runat="server" data-corners="false" autofocus="on" MaxLength="16" data-clear-btn="true" />
                    </li>
                    <li class="item item-icon-left item-input">
                        <i class="icon icon-password"></i>
                        <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" Text="password" />
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" data-corners="false" MaxLength="10" data-clear-btn="true" />
                    </li>
                    <li class="item item-icon-left item-input hide capt">
                        <i class="icon icon-check"></i>
                        <asp:Label ID="lblCaptcha" runat="server" AssociatedControlID="txtCaptcha" Text="code" />
                        <div class="ui-grid-a">
                            <div class="ui-block-b">
                                <asp:Image ID="imgCaptcha" runat="server" CssClass="imgCaptcha" />
                            </div>
                        </div>
                        <div class="ui-grid-a">
                            <div class="ui-block-a">
                                <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="4" type="tel" data-mini="true" data-corners="false" data-clear-btn="true" />
                            </div>
                        </div>
                    </li>
                    <li class="item row">
                        <div class="col">
                            <asp:Button ID="btnSubmit" runat="server" Text="login" data-corners="false" />
                        </div>
                    </li>
                     <li class="item row">
                        <div class="col">
                           <a href="ForgotPassword.aspx" data-ajax="false"><%=commonCulture.ElementValues.getResourceString("forgotpassword", commonVariables.LeftMenuXML)%></a>
                        </div>
                    </li>
                </ul>

                <asp:HiddenField runat="server" ID="ioBlackBox" Value="" />
                <asp:Literal ID="lblRegister" runat="server" Visible="false" />
            </form>
        </div>

        <script type="text/javascript">
            
            function showModal(message) {
                $('#ModalMessage').html(message);
                $("#jqPopUpModal").modal();
            }

            $(function () { $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });
            $('#<%=imgCaptcha.ClientID%>').click(function () { $(this).attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime()); });

            var counter = 0;
            $('#<%=imgCaptcha.ClientID%>').attr('class', 'hide');
            $('#<%=lblCaptcha.ClientID%>').attr('class', 'hide');
            $('#<%=txtCaptcha.ClientID%>').attr('class', 'hide');

            $(document).ready(function () {

                $(".ui-overlay-a").css('text-shadow', '0 1px 0 #2a8fbd');

                $('#<%=btnSubmit.ClientID%>').click(function (e) {
                    var message = ('<ul>');
                    $('#btnSubmit').attr("disabled", true);

                    var hasError = false;

                    if ($('#txtUsername').val().trim().length == 0) {
                        message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Login/MissingUsername", xeErrors)%></li>');
                        hasError = true;
                        e.preventDefault();
                    }
                    if (!/^[a-zA-Z0-9]+$/.test($('#txtUsername').val().trim())) {
                        message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors)%></li>');
                        hasError = true;
                        e.preventDefault();
                    }
                    if ($('#txtUsername').val().trim().indexOf(' ') >= 0) {
                        message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors)%></li>');
                        hasError = true;
                        e.preventDefault();
                    }
                    if ($('#txtPassword').val().trim().length == 0) {
                        message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Login/MissingPassword", xeErrors)%></li>');
                        hasError = true;
                        e.preventDefault();
                    }

                    if (counter >= 3) {
                        if ($('#txtCaptcha').val().trim().length == 0) {
                            message += ('<li><%=commonCulture.ElementValues.getResourceXPathString("Register/MissingVCode", xeErrors)%></li>');
                            hasError = true;
                            e.preventDefault();
                        }
                    }

                    if (hasError) {
                        message += ('</ul>');
                        $('#btnSubmit').attr("disabled", false);
                        showModal(message);
                        return;
                    } else {
                        e.preventDefault();
                        initiateLogin();
                    }
                });
            });

            function initiateLogin() {
                var udata = { Username: $('#txtUsername').val(), Password: $('#txtPassword').val(), Captcha: $('#txtCaptcha').val(), ioBlackBox: $('#ioBlackBox').val() };
                $.ajax({
                    type: "POST",
                    contentType: "application/json",
                    url: '/_Secure/AjaxHandlers/Login.ashx',
                    beforeSend: function() {
                        pubsub.publish('startLoadItem', { selector: '' });
                    },
                    timeout: function () {
                        $('#<%=btnSubmit.ClientID%>').prop('disabled', false);
                        showModal('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                        window.location.replace('/Default.aspx');
                    },
                    data: JSON.stringify(udata),
                    success: function (xml) {

                        var message = xml.Message;

                        if (xml.Code == undefined) {
                            initiateLogin();
                            return;
                        }

                        switch (xml.Code) {

                        case "resetPassword":
                        case "1":

                                Cookies().setCookie('IsApp', '1', 1);

                            window.User.token = Cookies().getCookie('s');

                            pubsub.subscribe('checkFreeRounds', onCheckFreeRounds);
                            _w88_products.checkFreeRounds();

                            function onCheckFreeRounds() {

                                if (!_.isUndefined(_w88_products.FreeRoundsGameUrl)) {
                                    var gameTemplate = '<div class="free-rounds"><img src="/_Static/images/v2/freerounds/Popup-free-round-<%=commonVariables.SelectedLanguageShort.ToLower()%>.jpg"> </img> <div class="free-round-btns"><a id="btnClaimNow" href="{0}" data-ajax="false" class="ui-btn btn-primary"></a><a id="btnClaimLater" href="{1}" data-ajax="false" class="ui-btn btn-primary"></a></div></div>';
                                    gameTemplate = gameTemplate.replace("{0}", _w88_products.FreeRoundsGameUrl);
                                    gameTemplate = gameTemplate.replace("{1}", "/ClubBravado");

                                    showModal(gameTemplate);
                                    
                                    $("#btnClaimNow").text(_w88_contents.translate("BUTTON_CLAIM"));
                                    $("#btnClaimLater").text(_w88_contents.translate("BUTTON_CLAIM_LATER"));

                                    $('#jqPopUpModal').on($.modal.BEFORE_CLOSE, function() {
                                        window.location = "/index";
                                    });
                                    } else {
                                        window.location.replace('/Funds.aspx');
                                    }
                                }

                                break;
                            case "22":
                                $('#btnSubmit').attr("disabled", false);
                            showModal('<div>' + message + '</div>');
                                break;

                            default:
                                counter += 1;

                                if (counter >= 3) {
                                    $(".capt").removeClass("hide");
                                    $('#<%=imgCaptcha.ClientID%>').attr('class', 'show imgCaptcha');
                                    $('#<%=lblCaptcha.ClientID%>').attr('class', 'show imgCaptcha');
                                    $('#<%=txtCaptcha.ClientID%>').attr('class', 'show imgCaptcha');
                                    $('#<%=imgCaptcha.ClientID%>').attr('src', '/_Secure/Captcha.aspx?t=' + new Date().getTime());
                                    $('#<%=txtCaptcha.ClientID%>').val('');
                                    $('#<%=txtPassword.ClientID%>').val('');
                                }

                            $('#btnSubmit').attr("disabled", false);
                            showModal('<div>' + message + '</div>');
                            pubsub.publish('stopLoadItem', { selector: '' });
                            break;
                        }
                    },
                    error: function (err) {
                        pubsub.publish('stopLoadItem', { selector: '' });
                        showModal('<%=commonCulture.ElementValues.getResourceString("Exception", xeErrors)%>');
                        window.location.replace('<%=strRedirect%>');
                    }
                });
            }

        </script>

    </div>
</body>
    
    
<div id="jqPopUpModal" class="modal" style="display: none;">
    <div id="ModalMessage" class="modal-content padding text-center"></div>
</div>

</html>
