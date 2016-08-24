(function () {
    var app = ons.bootstrap('w88Mobile', [
        'onsen'
        , 'ui.router'
        , 'angular-storage'
        , 'pascalprecht.translate'
        , 'slick']);
    app.constant(
        "W88KEYS", {
            "user": "currentUser",
            "storage": "mob_w88",
            "deposit": "depositSettings",
            "withdrawal": "withdrawalSettings"
        });
    app.constant(
        "W88CACHE", {
            "expires": "1200000", // 20 minutes / 1200 secs
        });
    app.constant(
        "W88URL", {
            "api": "/api",
            "app": "/_Static/JS/app"
        });
    app.config(function ($translateProvider) {
        $translateProvider.useLoader('translationService');
        $translateProvider.preferredLanguage('en-us');
        $translateProvider.forceAsyncReload(true);
        $translateProvider.useSanitizeValueStrategy(null);

    });
    app.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.interceptors.push('httpInterceptor');
    }]);
    app.config(function ($locationProvider, $stateProvider, $urlRouterProvider) {
        function getNavigator() {
            return document.getElementById('pageNavigator');
        }

        function popPage($rootScope) {
            if ($rootScope.navi.pages.length > 1) {
                $rootScope.navi.popPage($rootScope);
            }
        }

        var transition = "fade";

        $locationProvider.html5Mode(true);
        $urlRouterProvider.otherwise('/v2');
        var _app = {
            abstract: true
        }

        var _app_index = {
            parent: 'app',
            url: "/v2",
            data: {
                requireLogin: false,
            },
            resolve: {
                loaded: ['$rootScope', 'authService', function ($rootScope, authSvc) {
                    currentUser = authSvc.getUser();
                    if (_.isEmpty(currentUser))
                        return $rootScope.navi.resetToPage('_Static/js/app/user/template/index.html', { animation: transition });
                    else
                        return $rootScope.navi.resetToPage('_Static/js/app/slots/template/games.html', { animation: transition });
                }]
            }
        }

        var _app_home = {
            parent: 'app.index',
            data: {
                requireLogin: false,
            },
            onEnter: function ($rootScope) {
                $rootScope.navi.resetToPage('_Static/js/app/user/template/index.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }

        var _app_language = {
            parent: 'app.index',
            url: "/language",
            data: {
                requireLogin: false,
            },
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/user/template/language.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }

        var _app_games = {
            parent: 'app.index',
            url: "/game/:page",
            data: {
                requireLogin: false,
            },
            params: {
                page: 'slots'
            },
            onEnter: function ($rootScope) {
                $rootScope.navi.resetToPage('_Static/js/app/slots/template/games.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }

        var _app_login = {
            parent: 'app.index',
            url: "/login",
            data: {
                requireLogin: false,
            },
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/user/template/login.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }

        var _app_logout = {
            parent: 'app',
            url: "/v2/logout",
            data: {
                requireLogin: false,
            },
            resolve: {
                loaded: ['$rootScope', 'authService', 'userService', 'communicationChannel', function ($rootScope, authService, userService, communicationChannel) {
                     var user = angular.copy(authService.getUser());
                     authService.removeUser();
                     communicationChannel.userLoggedOut(user, userService);
                    return $rootScope.navi.resetToPage('_Static/js/app/user/template/index.html', { animation: transition });
                }]
            }
            
        }

        var _app_registration = {
            parent: 'app.index',
            url: "/registration",
            data: {
                requireLogin: false,
            },
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/user/template/registration.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }

        var _app_transfer_form = {
            parent: 'app.funds',
            url: "/transfer",
            data: {
                requireLogin: true,
            },
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/funds/template/transfer.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }

        var _app_history_form = {
            parent: 'app.funds',
            url: "/history/deposit-withdrawal",
            data: {
                requireLogin: true,
            },
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/history/template/history-form.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }

        var _app_history_result = {
            parent: 'app.index',
            url: "/history/result",
            data: {
                requireLogin: true,
            },
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/history/template/history-results.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }


        var _app_promotions = {
            parent: 'app.index',
            url: "/promotions",
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/promo/template/promotions.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }


        var _app_promotion_details = {
            parent: 'app.index',
            url: "/promotions/promo-details",
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/promo/template/promotion-details.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }


        var _app_download_center = {
            parent: 'app.index',
            url: "/download-center",
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/download-center/template/download-center.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }


        var _app_download_instructions = {
            parent: 'app.index',
            url: "/download-center/download-instructions",
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/download-center/template/download-instructions.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }


        var _app_funds = {
            parent: 'app.index',
            url: "/funds",
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/funds/template/funds.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }

        var _app_gateway_select = {
            parent: 'app.index',
            url: "/funds/gateway-select",
            onEnter: function ($rootScope) {
                $rootScope.navi.resetToPage('_Static/js/app/funds/template/gateway-select.html', {animation: transition});
            }
        }


        var _app_payments = {
            parent: 'app.funds',
            url: "/:type",
            params: {
                type: "deposit"
            },
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/funds/template/payments.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }

        var _app_error = {
            parent: 'app.index',
            url: "/error",
            data: {
                requireLogin: false,
            },
            onEnter: function ($rootScope) {
                $rootScope.navi.pushPage('_Static/js/app/user/template/error.html', { animation: transition });
            },
            onExit: function ($rootScope) {
                popPage($rootScope);
            }
        }



        $stateProvider.state('app', _app);
        $stateProvider.state('app.index', _app_index);
        $stateProvider.state('app.language', _app_language);
        $stateProvider.state('app.home', _app_home);
        $stateProvider.state('app.games', _app_games);
        $stateProvider.state('app.login', _app_login);
        $stateProvider.state('app.logout', _app_logout);
        $stateProvider.state('app.registration', _app_registration);
        $stateProvider.state('app.funds', _app_funds);
        $stateProvider.state('app.funds.transfer', _app_transfer_form);
        $stateProvider.state('app.history', _app_history_form); 
        $stateProvider.state('app.history-result', _app_history_result);
        $stateProvider.state('app.promotions', _app_promotions);
        $stateProvider.state('app.promotion-details', _app_promotion_details);
        $stateProvider.state('app.download-center', _app_download_center);
        $stateProvider.state('app.download-instructions', _app_download_instructions);
        $stateProvider.state('app.gateway-select', _app_gateway_select);
        $stateProvider.state('app.funds.payments', _app_payments);
        $stateProvider.state('app.error', _app_error);

    });

    app.filter('capitalize', function () {
        return function (input) {
            return (!!input) ? input.charAt(0).toUpperCase() + input.substr(1).toLowerCase() : '';
        }
    });

    app.run(['$rootScope', '$state', 'authService', function ($rootScope, $state, authService) {
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams) {
            var requireLogin = toState.data.requireLogin,
                currentUser = authService.getUser();

            if (requireLogin && !currentUser) {
                event.preventDefault();
                $state.go("app.index");
            }
            if (toState.name == "app.registration" && currentUser) {
                event.preventDefault();
                $state.go("app.index");
            }
        });
    }]);

})();;
(function () {
    var communicationChannel = function ($rootScope) {

        var USER_LOGGED_IN = 'USER_LOGGED_IN',
            USER_LOGGED_OUT = 'USER_LOGGED_OUT';

        var userLoggedIn = function (data) {
            $rootScope.$broadcast(USER_LOGGED_IN, data);
        };

        var onUserLoggedIn = function ($scope, handler) {
            $scope.$on(USER_LOGGED_IN, function (event, data) {
                handler(event, data);
            });
        };

        var userLoggedOut = function (user, userService) {
            $rootScope.$broadcast(USER_LOGGED_OUT);
            userService.logout(user.MemberId);
        };

        var onUserLoggedOut = function ($scope, handler) {
            $scope.$on(USER_LOGGED_OUT, function (event) {
                handler(event);
            });
           
        };

        return {
            userLoggedIn: userLoggedIn,
            onUserLoggedIn: onUserLoggedIn,
            userLoggedOut: userLoggedOut,
            onUserLoggedOut: onUserLoggedOut
        };
    };

    angular.module('w88Mobile').factory('communicationChannel', communicationChannel);
    communicationChannel.$inject = ['$rootScope']
})();;
(function () {
    var httpBase = function ($http, $q) {

        function handleError(response) {
            if (
                !angular.isObject(response.data) ||
                !response.data.message
                ) {
                return ($q.reject("An unknown error occurred."));
            }
            return ($q.reject(response.data.message));
        }

        function handleSuccess(response) {
            return (response.data);
        }

        return ({
            handleError: handleError,
            handleSuccess: handleSuccess
        });
    };
    angular.module('w88Mobile').factory('httpBase', httpBase);
    httpBase.$inject = ["$http", "$q"]
})();;
(function () {

    angular.module('w88Mobile').factory('httpInterceptor', httpInterceptor);
    httpInterceptor.$inject = ["$q", "authService"]

    function httpInterceptor($q, AuthService) {
        var service = this,
            requests = [];

        var isAPIRequest = function (url) {

            var apiPaths = ['/api'];

            for (var i in apiPaths) {
                if (_.includes(url, apiPaths[i])) {
                    return true;
                }
            }
            return false;
        };


        service.request = function (config) {
            if (isAPIRequest(config.url)) {
                var currentUser = angular.copy(AuthService.getUser())
                    , token = currentUser ? currentUser.Token : null
                    , lang = currentUser ? currentUser.LanguageCode : null;

                if (token)
                    config.headers.Token = token;
                if (lang)
                    config.headers.LanguageCode = lang;

                var canceller = $q.defer();
                config.timeout = canceller.promise;
                config.canceller = canceller;

                requests.push(config);
            }

            return config;
        };

        service.response = function (response) {

            return response;
        };

        service.responseError = function (response) {
            // handle response error here
            // should be able to capture session expired

            return $q.reject(response) || response;
        };

        return service;
    }

})();;
$( document ).ready(function() {
    $('.home-banner').slick({
    	 dots: true,
    	 arrows: false,
		  autoplay: true,
		  autoplaySpeed: 6000,
		  speed: 300,
		  fade: true,
    });
});;
(function () {
    var contentService = function ($http, $q, httpBase, URL) {

        var service = Object.create(httpBase);
        var url = URL.api;

        service.countryphonelist = function () {
            var request = $http({
                method: "get",
                url: url + "/countryphonelist",
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        service.currencylist = function () {
            var request = $http({
                method: "get",
                url: url + "/currencylist",
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        service.countryPhoneCode = function (data) {
            var request = $http({
                method: "get",
                url: url + "/countryinfo",
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        service.ip2loc = function (data) {
            var request = $http({
                datatype: "jsonp",
                url: "https://ip2loc.w2script.com/IP2LOC?v=" + new Date().getTime(),
                contentType: "application/json; charset=utf-8",
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        service.depositChannel = function () {
            var request = $http({
                method: "get",
                url: url + "/depositchannel"
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        service.systemBank = function () {
            var request = $http({
                method: "get",
                url: url + "/banks/system"
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        service.memberBank = function () {
            var request = $http({
                method: "get",
                url: url + "/banks/member"
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        service.vendorBank = function (id) {
            var request = $http({
                method: "get",
                url: url + "/banks/vendor/" + id
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        return service;
    };
    angular.module('w88Mobile').factory('contentService', contentService);
    contentService.$inject = ["$http", "$q", "httpBase", "W88URL"]
})();;
(function () {
    angular.module('w88Mobile').factory('toasterService', toaster);

    function toaster() {
        var isMaterial = true;
        var baseOptions = { modifier: isMaterial ? 'material' : undefined };
        var alert = function (msg, options) {

            if (!_.isUndefined(options)) {
                baseOptions = _.extend(baseOptions, options);
            }

            if (!_.isEmpty(msg)) {
                baseOptions.message = msg;
            }

            ons.notification.alert(baseOptions);
        };

        var confirm = function (msg, callback) {
            ons.notification.confirm({
                message: msg,
                modifier: isMaterial,
                callback: callback
            });
        };

        var prompt = function (msg, callback) {
            if (!msg.trim()) return;
            ons.notification.prompt({
                message: msg,
                modifier: isMaterial,
                callback: callback
            });
        };

        return {
            alert: alert,
            confirm: confirm,
            prompt: prompt,
            isMaterial: isMaterial
        }
    };

})();;
(function () {
    var translationService = function ($http, $q, httpBase, URL) {
        var fallback;
        function fallbackTranslation() {
            var url = URL.app + '/i18n/locale-en.json';
            $http.get(url).then(function (response) {
                fallback = response.data;
            })
        }
        var service = Object.create(httpBase);
        var url = URL.api + "/contents";

        service = function () {
            fallbackTranslation();
            var request = $http({
                method: "get",
                url: url,
            });
            return (request.then(handleSuccess, service.handleError));

            function handleSuccess(response)
            {
                if (_.isUndefined(response.data.ResponseData)) {
                    return fallback;
                }
                return response.data.ResponseData;
            }

            function handleError(response) {
                return fallback;
            }
        }

        return service;
    };
    angular.module('w88Mobile').factory('translationService', translationService);
    translationService.$inject = ["$http", "$q", "httpBase", "W88URL"]
})();;
(function () {
    var storageService = function (store, KEYS) {
        return store.getNamespacedStore(KEYS.storage);
    };
    angular.module('w88Mobile').factory('storageService', storageService);
    storageService.$inject = ["store", "W88KEYS"]
})();;
(function () {
    var Footer = function ($rootScope, $state, communicationChannel, authService, footerService) {
        function link(scope, elem, attr) {

            scope.currentUser = authService.getUser()

            communicationChannel.onUserLoggedIn(scope, onUserLoggedInListener);

            function onUserLoggedInListener(event, data) {
                scope.currentUser = data
            }

            switch (scope.mode) {
                case 'login':
                    scope.items = footerService.login();
                    break;
                case 'register':
                    scope.items = footerService.register();
                    break;
                default:
                    scope.items = footerService.index;
                    break;
            }

            scope.footerAction = function (url) {
                if ($state.current.name != "app.index" || $state.current.name != "app.home") {
                    if ($rootScope.navi.pages.length > 1) {
                        $rootScope.navi.popPage().then(function () {
                            $state.go(url);
                        });
                    } else $state.go(url);
                } else $state.go(url);
            };
        }
        return {
            restrict: 'AE',
            templateUrl: '_Static/js/app/menu/template/footer.html',
            link: link,
            replace: true,
            scope: {
                mode: '@',
            }
        };
    }
    angular.module('w88Mobile').directive('footerDiv', Footer);
    Footer.$inject = ['$rootScope', '$state', 'communicationChannel', 'authService', 'footerService'];
})();;
(function () {
    var AppMenu = function (communicationChannel, authService, menuService) {
        function link(scope, elem, attr) {
            init()

            function init() {
                scope.toolbarMenu = scope.menu;
                scope.toggleMenu = function () {
                    $(elem).collapse('toggle');
                }

                scope.userInfo = authService.getUser();
                setUserMenus();
            }

            communicationChannel.onUserLoggedIn(scope, onUserLoggedInListener);

            function onUserLoggedInListener(event, data) {
                scope.userInfo = data;

                setUserMenus();
            }

            communicationChannel.onUserLoggedOut(scope, onUserLoggedOutListener);

            function onUserLoggedOutListener(event) {

                scope.userInfo = authService.getUser();
                setUserMenus();
            }

            function setUserMenus() {
                if (scope.userInfo) {
                    scope.userMenus = menuService.userItems;
                } else {
                    scope.userMenus = menuService.publicMenuItems();
                }
                scope.menuItems = menuService.menuItems;
            }
        }
        return {
            restrict: 'AE',
            templateUrl: '_Static/js/app/menu/template/menu.html',
            link: link,
            replace: true,
            scope: {

            }
        };
    }
    angular.module('w88Mobile').directive('appMenu', AppMenu);
    AppMenu.$inject = ['communicationChannel', 'authService', 'menuService'];
})();;
(function () {
    angular.module('w88Mobile').directive('appToolbar', AppToolbar);
    AppToolbar.$inject = ['authService']

    function AppToolbar(authService) {
        function link(scope, elem, attr) {
            scope.toolbarMenu = scope.menu;
        }
        function ctrl($scope, $rootScope) {
            var user = authService.getUser();
            if (user) {
                $scope.userCurrency = user.CurrencyCode;
            }

            $scope.popPage = function () {
                $rootScope.navi.popPage($rootScope);
            }
            $scope.backUri = (_.isEmpty($scope.parentState)) ? "app.index" : $scope.parentState;
        }
        return {
            restrict: 'AE',
            templateUrl: '_Static/js/app/menu/template/toolbar.html',
            link: link,
            replace: true,
            controller: ctrl,
            scope: {
                userMenu: '=',
                menu: '=',
                title: '=',
                isIndex: '@',
                parentState: '='
            }
        };
    }
})();;
(function () {
    var footerService = function () {
        indexItems = [
            {
                title: 'BUTTON_LOGIN',
                url: 'app.login',
                requireLogin: false,
                css: {
                    icon: 'icon-login',
                    btn: 'btn-icon-top'
                }
            },
            {
                title: 'LABEL_MENU_REGISTER',
                url: 'app.registration',
                requireLogin: false,
                css: {
                    icon: 'icon-user-add',
                    btn: 'btn-icon-top btn-light ',
                }
            },
            {
                title: 'LABEL_MENU_FREE_PLAY',
                url: 'app.games',
                css: {
                    icon: 'icon-dice',
                    btn: 'btn-icon-top btn-lighter',
                }
            },
        ];

        loginItems = function () {
            return _.filter(indexItems, function (data) {
                return data.title !='BUTTON_LOGIN'
            });
        };

        registerItems = function () {
            return _.filter(indexItems, function (data) {
                return data.title != 'LABEL_MENU_REGISTER'
            });
        };

        return {
            index: indexItems,
            login: loginItems,
            register: registerItems
        }
    };
    var module = angular.module('w88Mobile').factory('footerService', footerService);
})();;
(function () {
    var menuService = function () {
        userMenuItems = [
            { title: 'Profile', url: '/', icon: 'icon-profile'},
            { title: 'Rewards', url: '//mrewards.w88live.com', icon: 'icon-reward', isPublic: true, isNewTab: true },
            { title: 'Messages', url: '/', icon: 'icon-mail'},
            { title: 'Promotions', url: 'app.promotions', icon: 'icon-promo', isPublic: true },
            { title: 'Downloads', url: 'app.download-center', icon: 'icon-download', isPublic: true },
            { title: 'Language', url: '/', icon: 'icon-globe', isPublic: true },
            { title: 'Login', url: 'app.login', icon: 'icon-login', isPublic: true },
            { title: 'Logout', url: 'app.logout', icon: 'icon-logout'}
        ];

        menuItems = [
            { title: 'Home', url: 'app.home', icon: 'icon-home' },
            { title: 'Slots', url: 'app.games', icon: 'icon-slots' },
            { title: 'Lottery', url: 'app.download-instructions', icon: 'icon-keno' },
            { title: 'Poker', url: 'app.download-instructions', icon: 'icon-spade' },
            { title: 'Texas Mahjong', url: 'app.download-instructions', icon: 'icon-mahjong' },
            { title: 'Live Casino', url: 'app.download-instructions', icon: 'icon-casino' }
        ];

        currencyHtmlItems = [
            { title: 'UUS', html: '&#x0024;' },
            { title: 'AUD', html: '&#x0024;' },
            { title: 'EUR', html: '&#x20ac;' },
            { title: 'GBP', html: '&#x00a3;' },
            { title: 'IDR', html: '&#x52;&#x70;' },
            { title: 'MYR', html: '&#x52;&#x4d;' },
            { title: 'RMB', html: '&#x00a5;' },
            { title: 'THB', html: '&#x0e3f;' },
            { title: 'USD', html: '&#x0024;' },
            { title: 'JPY', html: '&#x00a5;' },
            { title: 'KRW', html: '&#x20a9;' },
            { title: 'VND', html: '&#x20ab;' },
            { title: 'INR', html: '&#x20b9;' }
        ]

        publicMenuItems = function () {
            return _.filter(userMenuItems, function (data) {
                return data.isPublic == true
            });
        };

        return {
            userItems: userMenuItems,
            publicMenuItems: publicMenuItems,
            menuItems: menuItems
        }
    };
    var module = angular.module('w88Mobile').factory('menuService', menuService);
})();;
(function () {
    var GameCtrl = function ($scope, $stateParams, menuService, slotService, bannerService, authService) {
        var ctrl = this;
        ctrl.title = 'LABEL_MENU_SLOTS';
        ctrl.isIndex = authService.getUser() ? 1 : 0;
        ctrl.pages = [
            { label: "Sports", key: "sports" }
            , { label: "Slots", key: "slots" }
            , { label: "Texas Mahjong", key: "texas-mahjong" }
            , { label: "Lottery", key: "lottery" }];
        ctrl.currentPageKey = $stateParams.page;
        ctrl.slotItem = {};

        bannerService.banner().then(function (response) {
            if (response.ResponseCode == 1)
                ctrl.carouselItems = response.ResponseData;
        });

        slotService.slot("bs").then(function (response) {
            if (response.ResponseCode == 1)
                ctrl.categories = response.ResponseData;
        });

        ctrl.showDialog = function (id, game) {
            $('#' + id).show();

            ctrl.slotItem = game;
        };

        ctrl.hideDialog = function (id) {
            $('#' + id).hide();
        };

        ons.ready(function () {
            // Init code here
        });
    };
    angular.module('w88Mobile').controller('GameCtrl', GameCtrl);
    GameCtrl.$inject = ['$scope', '$stateParams', 'menuService', 'slotService', 'bannerService', 'authService']
})();;
(function () {
    var GameCategory = function () {
        function link(scope, elem, attr) {
           
        }
        return {
            restrict: 'AE',
            templateUrl: '_Static/js/app/slots/template/game-category.html',
            link: link,
            replace: true,
            scope: {
                category: '=',
                showDialog: "="
            }
        };
    }
    angular.module('w88Mobile').directive('gameCategory', GameCategory);
})();;
(function () {
    var slotService = function ($http, $q, httpBase, URL) {

        var service = Object.create(httpBase);
        var url = URL.api + "/game/";

        service.slot = function (gameprovider, data) {
            var request = $http({
                method: "get",
                url: url + gameprovider,
                data: data
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        return service;
    };
    angular.module('w88Mobile').factory('slotService', slotService);
    slotService.$inject = ["$http", "$q", "httpBase", "W88URL"]
})();;
(function () {
    angular.module('w88Mobile').controller('LoginCtrl', LoginCtrl);
    LoginCtrl.$inject = ['$scope', '$state', '$stateParams', 'userService', 'authService', 'communicationChannel', 'toasterService'];

    function LoginCtrl($scope, $state, $stateParams, userSvc, authSvc, communicationChannel, growl) {
        var ctrl = this;
        ctrl.title = 'BUTTON_LOGIN';
        ctrl.menuItems = [];
        ctrl.user = {
            ioBlackBox: "0400jrKIR8+",
            captcha: ""
        };
        ctrl.isProcessing = false;

        ctrl.submit = function (data) {
            ctrl.isProcessing = true;
            userSvc.login(data).then(
                function (response) {
                    if (response.ResponseCode == 1 || response.ResponseCode == 2) {
                        authSvc.setUser(response.ResponseData);
                        var currentUser = authSvc.getUser();
                        communicationChannel.userLoggedIn(currentUser);
                        userSvc.wallet().then(function (response) {
                            currentUser.wallets = response.ResponseData
                            authSvc.setUser(currentUser);
                            paymentSvc.removeSettings();
                            $state.go('app.games');
                        });
                    } else {
                        ctrl.isProcessing = false;
                        growl.alert(response.ResponseMessage);
                    }
                }, function (response) {
                    ctrl.isProcessing = false;
                });
        }
        ons.ready(function () {
            // Init code here
        });
    };
})();;
(function () {
    var MainCtrl = function ($scope, bannerService) {
        var ctrl = this;
        ctrl.title = "Test Title";
        bannerService.banner().then(function (response) {
            if (response.ResponseCode == 1)
                ctrl.carouselItems = response.ResponseData;
        });

        ons.ready(function () {
            // Init code here
        });
    };
    angular.module('w88Mobile').controller('MainCtrl', MainCtrl);
    MainCtrl.$inject = ['$scope', 'bannerService']
})();;
(function () {
    angular.module('w88Mobile').controller('RegistrationCtrl', RegistrationCtrl);
    RegistrationCtrl.$inject = ['$scope', '$location', '$state', 'contentService', 'userService', 'toasterService'];

    function RegistrationCtrl($scope, $location, $state, contentService, userSvc, growl) {
        var ctrl = this;
        ctrl.title = 'LABEL_MENU_REGISTER';
        ctrl.menuItems = [];
        ctrl.user = {};
        ctrl.isProcessing = false;
        // hacks hacks hacks
        ctrl.user.UserInfo = {
            Username: ""
        }
        function init() {
            var affiliate = $location.search()['AffiliateId'];

            if (affiliate) {
                ctrl.user.AffiliateId = parseInt(affiliate);
                ctrl.isDisabled = true;
            } else {
                ctrl.isDisabled = false;
            }
        }

        init();

        contentService.countryphonelist().then(
            function (response) {
                ctrl.countryCode = response.ResponseData.PhoneList;

                ctrl.user.CountryCode = response.ResponseData.PhoneSelected;
            });

        contentService.currencylist().then(
            function (response) {
                ctrl.currency = response.ResponseData.CurrencyList;

                ctrl.user.CurrencyCode = response.ResponseData.CurrencySelected;
            });

        ctrl.submit = function (data) {
            ctrl.isProcessing = true;
            userSvc.register(data).then(
                function (response) {
                    if (response.ResponseCode == 1) {
                        $state.go('app.games');
                    } else {
                        ctrl.isProcessing = false;
                        growl.alert(response.ResponseMessage);
                    }
                }, function (response) {
                    ctrl.isProcessing = false;
                });
        }


        ons.ready(function () {
            // Init code here
        });
    };
})();;
(function () {
    var UserWallet = function () {
        function link(scope, elem, attr) {
        }
        return {
            restrict: 'AE',
            templateUrl: '_Static/js/app/user/template/user-wallet.html',
            link: link,
            replace: true,
            scope: {
                title: '=',
                amount: '=',
                currency: '='
            }
        };
    }
    angular.module('w88Mobile').directive('userWallet', UserWallet);
    UserWallet.$inject = [];
})();;
(function () {
    var authService = function (KEYS, storageSvc) {
        var currentUser = null;
        return {
            getUser: getCurrentUser,
            setUser: setCurrentUser,
            removeUser: removeCurrentUser
        };

        function getCurrentUser() {
            if (!currentUser)
                currentUser = storageSvc.get(KEYS.user);

            return currentUser;
        }

        function setCurrentUser(user) {
            currentUser = user;
            storageSvc.set(KEYS.user, user);

            return currentUser;
        }

        function removeCurrentUser() {
            currentUser = null;
            storageSvc.remove(KEYS.user);
        }

    };
    angular.module('w88Mobile').factory('authService', authService);
    authService.$inject = ["W88KEYS", "storageService"]
})();;
(function () {
    var userService = function ($http, $q, httpBase, URL) {

        var service = Object.create(httpBase);
        var url = URL.api + "/user";

        service.login = function(data) {
            var request = $http({
                method: "post",
                url: url + "/login",
                data: data
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        service.register = function (data) {
            var request = $http({
                method: "post",
                url: url + "/register",
                data: data
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        service.logout = function (id) {
            var user = { memberId: id };
            var request = $http({
                method: "get",
                url: url + "/logout",
                params: user
        });
            return (request);
        }

        service.wallet = function () {
            var request = $http({
                method: "get",
                url: url + "/wallets"
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        return service;
    };
    angular.module('w88Mobile').factory('userService', userService);
    userService.$inject = ["$http", "$q", "httpBase", "W88URL"]
})();;
(function () {
    angular.module('w88Mobile').controller('PaymentCtrl', PaymentCtrl);
    PaymentCtrl.$inject = ['$scope', '$state', '$stateParams', '$filter', 'userService', 'authService', 'paymentService', 'toasterService']

    function PaymentCtrl($scope, $state, $stateParams, $filter, userSvc, authSvc, paymentSvc, growl) {
        var ctrl = this;
        ctrl.title = $stateParams.type;
        ctrl.menuItems = [];
        ctrl.paymentInfo = {};
        ctrl.isSettingsEmpty = false;
        ctrl.step = 1;
        ctrl.parentState = $state.current.parent;

        // payments

        function setSingleSetting() {
            if (ctrl.paymentGateways && _.size(ctrl.paymentGateways) == 1) {
                ctrl.fieldSettings = _.first(ctrl.paymentGateways);
                ctrl.paymentInfo.Id = ctrl.fieldSettings.Id;
                ctrl.title = $stateParams.type + " - " + ctrl.fieldSettings.TabName;;

            }
        }

        function getPaymentSettings() {
            paymentSvc.settings($stateParams.type).then(function (response) {
                if (!_.isEmpty(response.ResponseData)) {
                    paymentSvc.setSettings($stateParams.type, response.ResponseData);
                    getLocalSettings();
                }
            }, function () {
                ctrl.isSettingsEmpty = true;
            });
        }


        function getLocalSettings() {
            var settings = paymentSvc.getSettings($stateParams.type);
            ctrl.paymentGateways = settings ? settings.payments : null;
            setSingleSetting();
        }

        if ($stateParams.type) {
            getLocalSettings();

            if (ctrl.paymentGateways) {
                if (paymentSvc.getSettings($stateParams.type).expires <= _.now()) {
                    getPaymentSettings();
                }
            }
            else {
                getPaymentSettings();
            }
        }

        ctrl.isCurrentStep = function (step) {
            return step == ctrl.step;
        }
        ctrl.next = function () {
            ctrl.step++;
        }
        ctrl.previous = function () {
            ctrl.step--;
        }
        ctrl.hasSettings = function () {
            return !ctrl.isSettingsEmpty;
        }
        ctrl.hasSelectedSettings = function () {
            return ctrl.fieldSettings;
        }
        ctrl.setActivePayment = function (id) {
            ctrl.fieldSettings = _.find(ctrl.paymentGateways, function (data) { return data.Id == id; });
            ctrl.title = $stateParams.type + " - " + ctrl.fieldSettings.TabName;;
        }

        // user wallets
        var user = authSvc.getUser();
        ctrl.userWallets = user.wallets;
        ctrl.user = user;

        ctrl.wallet = {
            title: "Main Wallet",
            amount: user.Balance,
            currency: user.CurrencyCode
        }

        ctrl.submit = function (data) {
            paymentSvc.submit(data).then(
                function (response) {
                    if (response.ResponseCode == 1) {
                        growl.alert("", { messageHTML: "<p>" + response.ResponseMessage + "</p> <p>" + $filter('translate')('LABEL_TRANSACTION_ID') + ": " + response.ResponseData.TransactionId + "</p>" });
                        $state.go('app.funds');
                    }
                    else {
                        var message = "<ul>";
                        for (i = 0; i < response.ResponseMessage.length; i++) {
                            message = message + "<li>" + response.ResponseMessage[i] + "</li>";
                        }

                        message = message + "</ul>";

                        growl.alert("", { messageHTML: message });
                    }
                });
        }

        ons.ready(function () {
            // Init code here
        });
    };
})();;
(function () {
    angular.module('w88Mobile').controller('transferCtrl', TransferCtrl);
    TransferCtrl.$inject = ['$scope', '$timeout', '$state', 'userService', 'authService', 'transferService', 'toasterService'];

    function TransferCtrl($scope, $timeout, $state, userSvc, authSvc, transferSvc, growl) {
        var ctrl = this;
        ctrl.title = "Transfer";
        ctrl.menuItems = [];
        ctrl.transferData = {};

        ctrl.parentState = $state.current.parent;

        ctrl.user = authSvc.getUser();
        ctrl.userWallets = ctrl.user.wallets;
        var selectedFromIndex = 0;
        var selectedToIndex = 1;
        ctrl.from = ctrl.userWallets[selectedFromIndex];
        ctrl.to = ctrl.userWallets[selectedToIndex];

        var slickOptions = {
            speed: 150,
            infinite: false,
            slidesToShow: 3,
            swipeToSlide: true,
            centerMode: true,
            centerPadding: '40px',
            arrows: false,
            variableWidth: true
        }
        
        ctrl.transferFrom = function (wallet) {
            ctrl.from = wallet;
        }

        ctrl.transferTo = function (event) {
            ctrl.to = ctrl.userWallets[event.activeIndex];
        }

        ctrl.transfer = function () {
            var fromIndex = $(".wallet-from").slick("slickCurrentSlide");
            var toIndex = $(".wallet-to").slick("slickCurrentSlide");
            ctrl.from = ctrl.userWallets[fromIndex];
            ctrl.to = ctrl.userWallets[toIndex];

            ctrl.transferData.TransferFrom = ctrl.from.Id;
            ctrl.transferData.TransferTo = ctrl.to.Id;

            if (_.isEmpty(ctrl.transferData.TransferAmount)) {
                console.log(ctrl.transferData.TransferAmount);
                growl.alert("Transfer amount is required");
                return;
            } else if (ctrl.transferData.TransferFrom == ctrl.transferData.TransferTo) {
                growl.alert("Invalid transfer recipient.");
                return
            }

            transferSvc.transfer(ctrl.transferData).then(function (response) {

                if (response.ResponseCode == 1) {
                    growl.alert(response.ResponseData.Message);
                    userSvc.wallet().then(function (response) {
                        ctrl.user.wallets = response.ResponseData;
                        authSvc.setUser(ctrl.user);
                        _.forEach(ctrl.user.wallets, function (wallet, key) {
                            ctrl.userWallets[key].Balance = wallet.Balance;
                        });
                    });
                } else {
                    growl.alert(response.ResponseMessage);
                }
            });
        }

        ons.ready(function () {
        });
    };
})();;
(function () {
    var creditCard = function () {
        function link(scope, elem, attr) {
            scope.hasField = function (field) {
                if (!_.isUndefined(field) && field == 1) return true;
                else return false;
            }
        }
        return {
            restrict: 'AE',
            templateUrl: '_Static/js/app/funds/template/credit-card.html',
            link: link,
            scope: {
                settings: '='
            }
        };
    }
    angular.module('w88Mobile').directive('creditCard', creditCard);
})();;
(function () {
    var PostTrans = function (contentSvc) {
        function link(scope, elem, attr) {
            scope.hasField = function (field) {
                if (!_.isEmpty(field)) return true;
                else return false;
            }

            contentSvc.depositChannel().then(function (response) {
                if (!_.isEmpty(response.ResponseData)) {
                    scope.depositChannel = response.ResponseData;
                }
            });

            contentSvc.systemBank().then(function (response) {
                if (!_.isEmpty(response.ResponseData)) {
                    scope.systemBank = response.ResponseData;
                }
            });

            if (_.isEqual(scope.settings.BankList, "vendor")) {
                contentSvc.vendorBank(scope.settings.Id).then(function (response) {
                    if (!_.isEmpty(response.ResponseData)) {
                        scope.bank = response.ResponseData;
                    }
                });
            } else if (_.isEqual(scope.settings.BankList, "member")) {
                contentSvc.memberBank().then(function (response) {
                    if (!_.isEmpty(response.ResponseData)) {
                        scope.bank = response.ResponseData;
                    }
                });
            }

            scope.setSystemBank = function (item) {
                scope.paymentInfo.SystemBank = _.find(scope.systemBank, function (data) {
                    return data.Value == item;
                });
            };

            scope.setBank = function (item) {
                scope.paymentInfo.Bank = _.find(scope.bank, function (data) {
                    return data.Value == item;
                });
            };

            scope.setDepositChannel = function (item) {
                scope.paymentInfo.DepositChannel = _.find(scope.depositChannel, function (data) {
                    return data.Value == item;
                });
            };
        }
        return {
            restrict: 'AE',
            templateUrl: '_Static/js/app/funds/template/post-trans.html',
            link: link,
            scope: {
                settings: '=',
                paymentInfo: '='
            }
        };
    }
    angular.module('w88Mobile').directive('postTrans', PostTrans);
    PostTrans.$inject = ['contentService'];
})();;
(function () {
    angular.module('w88Mobile').factory('paymentService', paymentService);
    paymentService.$inject = ["$http", "$q", "httpBase", "W88URL", 'W88KEYS', 'W88CACHE', 'storageService']

    function paymentService($http, $q, httpBase, URL, KEYS, CACHE, storageSvc) {

        var service = Object.create(httpBase);
        var url = URL.api + "/payments";


        service.settings = function (type) {
            var request = $http({
                method: "get",
                url: url + "/settings/" + type
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        service.submit = function (data) {
            var request = $http({
                method: "post",
                url: url + "/" + data.Id,
                data: data
            });
            return (request.then(service.handleSuccess, service.handleError));
        }


        function setSettings(type, data) {
            var settings = {
                expires: new Date().setMilliseconds(CACHE.expires),
                payments: data
            };

            if (_.isEqual(type, "deposit")) {
                storageSvc.set(KEYS.deposit, settings);
            }
            else {
                storageSvc.set(KEYS.withdrawal, settings);
            }
        }

        function getSettings(type) {
            if (_.isEqual(type, "deposit")) {
                return storageSvc.get(KEYS.deposit);
            }
            else {
                return storageSvc.get(KEYS.withdrawal);
            }
        }

        function removeSettings(type) {
            if (_.isEqual(type, "deposit")) {
                storageSvc.remove(KEYS.deposit);
            }
            else {
                storageSvc.remove(KEYS.withdrawal);
            }
        }

        return {
            settings: service.settings,
            submit: service.submit,
            setSettings: setSettings,
            getSettings: getSettings,
            removeSettings: removeSettings
        };
    };
})();;
(function () {
    angular.module('w88Mobile').factory('transferService', transferService);
    transferService.$inject = ["$http", "$q", "httpBase", "W88URL"]

    function transferService($http, $q, httpBase, URL) {

        var service = Object.create(httpBase);
        var url = URL.api + "/payments/transfer";

        service.transfer = function (data) {
            var request = $http({
                method: "post",
                url: url,
                data: data
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        return service;
    };
})();;
(function () {
    var PromosCtrl = function ($scope, userSvc, authSvc) {
        var ctrl = this;
        ctrl.title = "Promotions";
        ctrl.menuItems = [];
        
        ons.ready(function () {
            // Init code here
        });
    };
    angular.module('w88Mobile').controller('PromosCtrl', PromosCtrl);
    PromosCtrl.$inject = ['$scope', 'userService', 'authService']
})();;
(function () {
    var bannerService = function ($http, $q, httpBase, URL) {

        var service = Object.create(httpBase);
        var url = URL.api + "/banner";

        service.banner = function () {
            var request = $http({
                method: "get",
                url: url,
            });
            return (request.then(service.handleSuccess, service.handleError));
        }

        return service;
    };
    angular.module('w88Mobile').factory('bannerService', bannerService);
    bannerService.$inject = ["$http", "$q", "httpBase", "W88URL"]
})();;
(function () {
    var HistoryCtrl = function ($scope, $state, userSvc, authSvc) {
        var ctrl = this;
        ctrl.title = "History";
        ctrl.menuItems = [];
        ctrl.parentState = $state.current.parent;
        
        ctrl.showDialog = function (id) {
            document
              .getElementById(id)
              .show();
        };

        ctrl.hideDialog = function (id) {
            document
              .getElementById(id)
              .hide();
        };

        ons.ready(function () {
            // Init code here
        });
    };
    angular.module('w88Mobile').controller('HistoryCtrl', HistoryCtrl);
    HistoryCtrl.$inject = ['$scope', '$state', 'userService', 'authService']
})();;
(function () {
    var DownloadCenterCtrl = function ($scope, userSvc, authSvc) {
        var ctrl = this;
        ctrl.title = "Download Center";
        ctrl.menuItems = [];
        
        ons.ready(function () {
            // Init code here
        });
    };
    angular.module('w88Mobile').controller('DownloadCenterCtrl', DownloadCenterCtrl);
    DownloadCenterCtrl.$inject = ['$scope', 'userService', 'authService']
})();