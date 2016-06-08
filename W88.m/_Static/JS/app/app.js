(function () {
    var app = ons.bootstrap('w88Mobile', ['onsen', 'ui.router']);

    app.config(function ($locationProvider, $stateProvider, $urlRouterProvider) {

        $locationProvider.html5Mode(true);
        $urlRouterProvider.otherwise('/v2');
        var _app = {
            abstract: true,
            url: "/v2"
        }

        var _app_index = {
            parent: 'app',
            url: "/index",
            onEnter: ['$rootScope', function ($rootScope) {
                $rootScope.menu.closeMenu();
                $rootScope.menu.setMainPage('_Static/js/app/user/template/index.html');
            }]
        }

        var _app_login = {
            parent: 'app',
            url: "/login",
            onEnter: ['$rootScope', function ($rootScope) {
                $rootScope.menu.closeMenu();
                $rootScope.menu.setMainPage('_Static/js/app/user/template/login.html');
            }]
        }

        var _app_registration = {
            parent: 'app',
            url: "/registration",
            onEnter: ['$rootScope', function ($rootScope) {
                $rootScope.menu.setMainPage('_Static/js/app/user/template/registration.html');
            }]
        }
        $stateProvider.state('app', _app);
        $stateProvider.state('app.index', _app_index);
        $stateProvider.state('app.login', _app_login);
        $stateProvider.state('app.registration', _app_registration);

    });

    app.controller('PageController', function ($scope) {
        ons.ready(function () {
            // Init code here
        });
    });
})();