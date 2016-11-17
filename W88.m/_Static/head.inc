<meta charset="utf-8" />
<meta http-equiv="Content-type" content="text/html;charset=utf-8" />

<meta name="apple-mobile-web-app-capable" content="yes" />
<meta name="mobile-web-app-capable" content="yes" />
<meta name="apple-mobile-web-app-status-bar-style" content="black" />
<meta name="apple-mobile-web-app-title" content="W88" />
<link rel="stylesheet" href="/_Static/Css/style.css" />
<link rel="apple-touch-icon" href="/_Static/Images/icon_76.png" />
<link rel="apple-touch-startup-image" href="/_Static/Images/icon_120.png" />
<script src="/_Static/JS/Mobile/jquery-1.11.3.min.js"></script>
<script src="/_Static/JS/Mobile/jquery.mobile-1.4.5.min.js"></script>
<script src="/_Static/JS/vendor/lodash.js"></script>
<script src="/_Static/JS/vendor/moment.min.js"></script>
<script src="/_Static/JS/Mobile/iscroll.js"></script>
<script src="/_Static/JS/Cookie.js"></script>
<script src="/_Static/JS/GPINT.js"></script>

<script src="/_Static/JS/fingerprint2.min.js"></script>
<script src="/_Static/JS/piwik.js"></script>
<script type="text/javascript">
    window.w88Mobile = {};
    window.User = {};
    window.User.hasSession = <%= (!String.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) ? 1 : 0 %>;
    window.User.token = '<%= commonVariables.CurrentMemberSessionId %>';
    window.User.sessionInterval = '<%= System.Configuration.ConfigurationManager.AppSettings.Get("sessionInterval") %>';
    window.User.lang = '<%=commonVariables.SelectedLanguage%>';
</script>
<script type="text/javascript" src="/_Static/Js/modules/slots.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/formValidator.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/growl.js"></script>
<script type="text/javascript">
    // set initial values for notification
    if(!_.isUndefined(w88Mobile.Growl)){
        w88Mobile.Growl.options.lang = '<%=commonVariables.SelectedLanguageShort%>';
        w88Mobile.Growl.options.close = '<%=commonCulture.ElementValues.getResourceString("close", commonVariables.LeftMenuXML)%>';
        w88Mobile.Gateways = {};
        w88Mobile.APIUrl = '<%= System.Configuration.ConfigurationManager.AppSettings.Get("APIUrl") %>';
    }
    </script>
<meta name="viewport" id="viewport" content="width=device-width, initial-scale=1.0" />
<script type="text/javascript" src="/_Static/JS/modules/hideMenu.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/fundtransfer.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/piwikManager.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/piwikConfig.js"></script>
<script type="text/javascript">
    w88Mobile.PiwikManager.setGoals(window.w88Mobile.PiwikConfig.goals);
    w88Mobile.PiwikManager.setDomain();
    w88Mobile.PiwikManager.setUserId('<%=UserSession.MemberId %>');

    var siteCookie = new Cookies();

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
<script type="text/javascript" src="/_Static/Js/modules/slots.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/wallets.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/bankTransfer.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/bankDetails.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/gateways/postForm.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/gateways/nganluong.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/gateways/fastdep.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/gateways/paygo.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/gateways/venuspoint.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/gateways/paysec.js"></script>