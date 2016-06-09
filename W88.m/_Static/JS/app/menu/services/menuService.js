(function () {
    var menuService = function () {
        menuItems = [
            { title: 'Home', url: 'app.index' },
            { title: 'Login', url: 'app.login' },
            { title: 'Sports', url: '/', icon: 'icon-sports' },
            { title: 'Live Casino', url: '/', icon: 'icon-casino' },
            { title: 'Texas Mahjong', url: '/', icon: 'icon-mahjong' },
            { title: 'Slots', url: '/', icon: 'icon-slots' },
            { title: 'Lottery', url: '/', icon: 'icon-keno' },
            { title: 'Poker', url: '/', icon: 'icon-spade' }
        ];

        dashboardItems = [
            { title: 'Sports', url: '/', icon: 'icon-sports' },
            { title: 'Live Casino', url: '/', icon: 'icon-casino' },
            { title: 'Texas Mahjong', url: '/', icon: 'icon-mahjong' },
            { title: 'Slots', url: '/', icon: 'icon-slots' },
            { title: 'Lottery', url: '/', icon: 'icon-keno' },
            { title: 'Poker', url: '/', icon: 'icon-spade' },
            { title: 'Promotions', url: '/', icon: 'icon-promo' },
            { title: 'Download', url: '/', icon: 'icon- ion-ios-download-outline' },
            { title: 'Language', url: '/', icon: 'icon- ion-ios-world-outline' },
            { title: 'Live Chat', url: '/', icon: 'icon-chat' }
        ]
        return {
            items: menuItems,
            dashboardItems: dashboardItems
        }
    };
    var module = angular.module('w88Mobile').factory('menuService', menuService);
})();