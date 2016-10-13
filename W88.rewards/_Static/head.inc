<meta charset="utf-8" />
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<meta name="mobile-web-app-capable" content="yes"/>
<meta name="apple-mobile-web-app-status-bar-style" content="black" />
<meta name="apple-mobile-web-app-title" content="W88" />
<link rel="stylesheet" href="/_Static/Css/Mobile/jquery.mobile-1.4.5.min.css" />
<link rel="stylesheet" href="/_Static/Css/base.css" />
<link rel="stylesheet" href="/_Static/Css/styles.css" />      
<link rel="apple-touch-icon" href="/_Static/Images/icon_76.png"/>
<link rel="apple-touch-startup-image" href="/_Static/Images/icon_120.png" />
<script src="/_Static/JS/Mobile/jquery-1.11.3.min.js"></script>
<script src="/_Static/JS/GPINT.js"></script>
<script src="/_Static/JS/Mobile/jquery.mobile-1.4.5.min.js"></script>
<script src="/_Static/JS/vendor/lodash.js"></script>
<script src="/_Static/JS/bootstrap.min.js"></script>
<script src="/_Static/JS/vendor/slick.min.js"></script>
<script type="text/javascript" src="/_Static/JS/modules/growl.js"></script>
<script type="text/javascript">
    // set initial values for notification
    if(w88Mobile.Growl) {
        w88Mobile.Growl.options.lang = '<%=W88.BusinessLogic.Shared.Helpers.LanguageHelpers.SelectedLanguageShort%>';
        w88Mobile.Growl.options.close = '<%=W88.BusinessLogic.Rewards.Helpers.RewardsHelper.GetTranslation(W88.BusinessLogic.Rewards.Models.TranslationKeys.Label.Close)%>';
    }
    var sessionInterval = '<%= System.Configuration.ConfigurationManager.AppSettings.Get("sessionInterval") %>';
</script>
<script src="/_Static/JS/modules/User.js"></script>
<script src="/_Static/JS/Main.js"></script>
<meta name="viewport" id="viewport" content="width=device-width, initial-scale=1.0" />