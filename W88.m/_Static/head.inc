<meta charset="utf-8" />
<meta http-equiv="Content-type" content="text/html;charset=utf-8" />

<meta name="apple-mobile-web-app-capable" content="yes" />
<meta name="mobile-web-app-capable" content="yes" />
<meta name="apple-mobile-web-app-status-bar-style" content="black" />
<meta name="apple-mobile-web-app-title" content="W88" />
<meta name="viewport" id="viewport" content="width=device-width, initial-scale=1.0" />
<link rel="stylesheet" href="/_Static/Css/style.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" />
<link rel="apple-touch-icon" href="/_Static/Images/icon_76.png" />
<link rel="apple-touch-startup-image" href="/_Static/Images/icon_120.png" />

<script type="text/javascript">
    window.w88Mobile = {};
    w88Mobile.Gateways = {};
    window.User = {};
    window.User.hasSession = <%= (!String.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) ? 1 : 0 %>;
    window.User.token = '<%= commonVariables.CurrentMemberSessionId %>';
    window.User.sessionInterval = '<%= System.Configuration.ConfigurationManager.AppSettings.Get("sessionInterval") %>';
    window.User.lang = '<%=commonVariables.SelectedLanguage%>';
    window.User.storageExpiration = { expires: 300000 };
    window.User.memberId = '<%=UserSession.MemberId %>';
    window.User.countryCode = '<%=UserSession.CountryCode %>';
    w88Mobile.APIUrl = '<%= System.Configuration.ConfigurationManager.AppSettings.Get("APIUrl") %>';
</script>
<script type="text/javascript" src="/_Static/JS/Cookie.js"></script>
<script type="text/javascript" src="/_Static/JS/Mobile/jquery-1.11.3.min.js"></script>
<script type="text/javascript" src="/_Static/JS/Mobile/jquery.mobile-1.4.5.min.js"></script>
<script type="text/javascript" src="/_Static/JS/jquery.mask.min.js"></script>
<script type="text/javascript" src="/_Static/JS/vendor/lodash.min.js"></script>
<script type="text/javascript" src="/_Static/JS/vendor/pubsub.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="/_Static/JS/vendor/moment.min.js"></script>
<script type="text/javascript" src="/_Static/JS/vendor/amplify.min.js"></script>
<script type="text/javascript" src="/_Static/JS/Mobile/iscroll.js"></script>
<script type="text/javascript" src="/_Static/JS/GPINT.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="/_Static/JS/modules/growl.js"></script>
<script type="text/javascript" src="/_Static/JS/Main.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="/_Static/JS/fingerprint2.min.js"></script>
<script type="text/javascript" src="/_Static/JS/piwik.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="/_Static/JS/i18n/contents-<%=commonVariables.SelectedLanguageShort%>.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/translate.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script>
    var siteCookie = new Cookies();
    //amplify clear
    amplify.clearStore = function() {
        $.each(amplify.store(), function (storeKey) {
            // Delete the current key from Amplify storage
            amplify.store(storeKey, null);
        });
    };
    
    var _w88_contents = new w88Mobile.Translate();
    _w88_contents.init();
    
    // set initial values for notification
    if(!_.isUndefined(w88Mobile.Growl)){
        w88Mobile.Growl.options.lang = '<%=commonVariables.SelectedLanguageShort%>';
        w88Mobile.Growl.options.close = '<%=commonCulture.ElementValues.getResourceString("close", commonVariables.LeftMenuXML)%>';
    }
</script>
<script type="text/javascript" src="/_Static/JS/modules/slots.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="/_Static/JS/modules/constants.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="/_Static/JS/modules/formValidator.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
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

<script type="text/javascript" src="/_Static/JS/modules/gateways/postForm.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="/_Static/JS/modules/products.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>