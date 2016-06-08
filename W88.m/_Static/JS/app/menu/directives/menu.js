(function () {
    var AppMenu = function () {
        function link(scope, elem, attr) {
            scope.toolbarMenu = scope.menu;
        }
        return {
            restrict: 'AE',
            templateUrl: '_Static/js/app/menu/template/menu.html',
            link: link,
            replace: true,
            scope: {
                items: '='
            }
        };
    }
    angular.module('w88Mobile').directive('appMenu', AppMenu);
})();