
<meta charset="utf-8" />
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
<!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->

<!-- Bootstrap -->
<link href="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/css/base.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet" />

<!-- JTSageDateBox -->
<link href="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/css/vendor/jtsage-datebox-4.1.1.bootstrap.min.css" rel="stylesheet" type="text/css">

<link href="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/css/styles.css?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>" rel="stylesheet" />



<!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
<!--[if lt IE 9]>
    <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
    <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    
<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/jquery.js"></script>

<!-- Include all compiled plugins from vendor (below), or include individual files as needed -->
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/bootstrap.min.js"></script>

<!-- jQuery Widget Factory (necessary for JTSageDateBox's JavaScript plugins) -->
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/jquery-ui.js" type="text/javascript"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/jtsage-datebox-4.1.1.bootstrap.min.js" type="text/javascript"></script>

<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/bootstrap-validator.min.js"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/slidebars.min.js"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/slick.min.js"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/lodash.min.js"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/amplify.min.js"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/pubsub.js"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/fingerprint2.min.js"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/cookie.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>  


<!-- Include all related variables (below), or include individual files as needed -->
<script type="text/javascript">
    var siteCookie = new Cookies();

    _.templateSettings = {
    interpolate: /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
    evaluate: /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
    escape: /\{%-([\s\S]+?)%\}/g
    };
    _.templateSettings.variable = "tplData";

    window.w88Mobile = {};
    w88Mobile.Gateways = {};
    w88Mobile.Gateway = {};
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
    
<!-- Include all compiled modules (below), or include individual files as needed -->
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/main.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/usercheck.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/loader.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/growl.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script> 
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/constants.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

<!-- Include all PIWIK related modules (below), or include individual files as needed -->
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/piwik/piwik.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/piwik/piwikManager.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/piwik/piwikConfig.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript">
    w88Mobile.PiwikManager.setGoals(window.w88Mobile.PiwikConfig.goals);
    w88Mobile.PiwikManager.setDomain();
    w88Mobile.PiwikManager.setUserId('<%=UserSession.MemberId %>');
    
    $(function () {
      if (_.isEmpty(siteCookie.getCookie("fingerprint"))) {
          new Fingerprint2().get(function (result, components) {
              var domain = "." + location.hostname.split('.').slice(-2).join('.');
              siteCookie.setCookie("fingerprint", result, 5, domain);
          });
      } else {
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

<!-- Include all Translation related modules (below), or include individual files as needed -->
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/i18n/contents-<%=commonVariables.SelectedLanguageShort%>.js"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/translate.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script>
    
    //amplify clear
    amplify.clearStore = function() {
        $.each(amplify.store(), function (storeKey) {
            // Delete the current key from Amplify storage
            amplify.store(storeKey, null);
        });
    };
    
    var _w88_contents = new w88Mobile.Translate();
    _w88_contents.init();
    
    $(document).ready(function () {
        // set initial values for notification
        if(!_.isUndefined(w88Mobile.Growl)){
            w88Mobile.Growl.init();
            w88Mobile.Growl.options.lang = '<%=commonVariables.SelectedLanguageShort%>';
            w88Mobile.Growl.options.close = '<%=commonCulture.ElementValues.getResourceString("close", commonVariables.LeftMenuXML)%>';
        }
    });
    
    w88Mobile.Loader.init();
</script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/interceptor.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
    