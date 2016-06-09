(function () {
    var MainCtrl = function ($scope, menuService) {
        var ctrl = this;
        ctrl.title = "Test Title";
        ctrl.carouselItems = [
            {
                url: '/Promotions#CLOUDTALES',
                src: '/_Static/Images/Download/W88-Mobile-TexasMahjong.jpg',
                alt: 'Cloud Tales'
            },
            {
                url: '/Promotions#CLOUDTALES',
                src: '/_Static/Images/Download/W88-Mobile-ClubW-Casino.jpg',
                alt: 'Cloud Tales'
            },
        ];

        ctrl.menuItems = menuService.items;
        ctrl.dashboardItems = menuService.dashboardItems;

        ons.ready(function () {
            // Init code here
        });
    };
    var module = angular.module('w88Mobile').controller('MainCtrl', MainCtrl);
    MainCtrl.$inject = ['$scope', 'menuService']
})();