<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/jquery-2.2.4.min.js"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/lodash.min.js"></script>

<!-- Include all compiled plugins (below), or include individual files as needed -->
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/bootstrap.min.js"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/amplify.min.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/Cookie.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/pubsub.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/vendor/bootstrap-validator.min.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

<script type="text/javascript">
    var siteCookie = new Cookies();

    _.templateSettings = {
    interpolate: /\{\{(.+?)\}\}/g,      // print value: {{ value_name }}
    evaluate: /\{%([\s\S]+?)%\}/g,   // excute code: {% code_to_execute %}
    escape: /\{%-([\s\S]+?)%\}/g
    };
    _.templateSettings.variable = "tplData";

    window.w88Mobile = {};
    window.User = {};
    window.User.hasSession = <%= (!String.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) ? 1 : 0 %>;
    window.User.token = '<%= commonVariables.CurrentMemberSessionId %>';
    window.User.lang = '<%=commonVariables.SelectedLanguage%>';
    window.User.storageExpiration = { expires: 300000 };
    w88Mobile.APIUrl = '<%= ConfigurationManager.AppSettings.Get("APIUrl") %>';
    window.w88Mobile.Gateway = {};
    window.w88Mobile.Gateways = {};
</script>

<script type="text/javascript" src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/i18n/contents-<%=commonVariables.SelectedLanguageShort%>.js"></script>
<script type="text/javascript" src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/translate.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/constants.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/loader.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/interceptor.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>
<script type="text/javascript" src="<%=ConfigurationManager.AppSettings.Get("AssetsPath") %>/assets/js/modules/growl.js?v=<%=ConfigurationManager.AppSettings.Get("scriptVersion") %>"></script>

<script>
    if (!_.isUndefined(w88Mobile.Growl)) {
    w88Mobile.Growl.init();
    w88Mobile.Growl.options.lang = '<%=commonVariables.SelectedLanguageShort%>';
    w88Mobile.Growl.options.close = '<%=commonCulture.ElementValues.getResourceString("close", commonVariables.LeftMenuXML)%>';
    }

</script>
