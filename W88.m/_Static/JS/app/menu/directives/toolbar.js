(function () {
    var AppToolbar = function () {
        function link(scope, elem, attr) {
            scope.toolbarMenu = scope.menu;
            scope.toolbarTitle = !_.isUndefined(scope.title) ? scope.title : '';
        }
        return {
            restrict: 'AE',
            templateUrl: '_Static/js/app/menu/template/toolbar.html',
            link: link,
            replace: true,
            scope: {
                menu: '=',
                title: '='
                }
        };
    }
    angular.module('w88Mobile').directive('appToolbar', AppToolbar);
})();