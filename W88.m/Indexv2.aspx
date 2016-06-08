<!doctype html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <link rel="stylesheet" href="/_Static/css/vendor/onsen/onsenui.css" />
    <link rel="stylesheet" href="/_Static/css/vendor/onsen/onsen-css-components.css" />
    <link rel="stylesheet" href="/_Static/css/style.css" />
    <script src="/_Static/js/vendor/lodash.js"></script>
    <script src="/_Static/js/vendor/angular.js"></script>
    <script src="/_Static/js/vendor/angular-ui-router.min.js"></script>
    <script src="/_Static/js/vendor/onsenui.js"></script>
    <script src="/_Static/js/vendor/angular-onsenui.js"></script>
    <script src="/_Static/js/app/app.js"></script>
    <script src="/_Static/js/app/MainCtrl.js"></script>
    <script src="/_Static/js/app/user/controllers/LoginCtrl.js"></script>
    <script src="/_Static/js/app/user/controllers/RegistrationCtrl.js"></script>
    <script src="/_Static/js/app/menu/directives/menu.js"></script>
    <script src="/_Static/js/app/menu/directives/toolbar.js"></script>
    <script src="/_Static/js/app/menu/services/menuService.js"></script>
    <base href="/" />
</head>
<body ng-controller="MainCtrl as main">
    <ons-sliding-menu main-page="_Static/js/app/user/template/index.html"
                      side="left"
                      var="menu">
        <div class="menu">
            <app-menu items="main.menuItems"></app-menu>
        </div>
    </ons-sliding-menu>
</body>
</html>