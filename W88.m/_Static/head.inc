<meta charset="utf-8">
<meta http-equiv="Content-type" content="text/html;charset=utf-8">
<meta name="viewport" id="viewport" content="minimal-ui, width=device-width, initial-scale=1.0, maximum-scale=1.0,user-scalable=no, target-densityDpi=device-dpi">
<meta name="apple-mobile-web-app-capable" content="yes">
<meta name="mobile-web-app-capable" content="yes">
<meta name="apple-mobile-web-app-status-bar-style" content="black">
<meta name="apple-mobile-web-app-title" content="W88">
<link rel="stylesheet" href="/_Static/Css/style.css">
<link rel="apple-touch-icon" href="/_Static/Images/icon_76.png">
<link rel="apple-touch-startup-image" href="/_Static/Images/icon_120.png">
<script src="/_Static/JS/Mobile/jquery-1.11.3.min.js"></script>
<script src="/_Static/JS/Mobile/jquery.mobile-1.4.5.min.js"></script>
<script src="/_Static/JS/Mobile/iscroll.js"></script>
<script src="/_Static/JS/GPINT.js"></script>
<script src="/_Static/JS/piwik.js"></script>
<script type="text/javascript">
  window.User = {};
  window.User.hasSession = <%= (!String.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) ? 1 : 0 %>;
  window.User.sessionInterval = '<%= System.Configuration.ConfigurationManager.AppSettings.Get("sessionInterval") %>';
</script>