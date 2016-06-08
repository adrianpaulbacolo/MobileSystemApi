(function () {
    var LoginCtrl = function ($scope, menuService) {
        var ctrl = this;
        ctrl.title = "Login";

        ons.ready(function () {
            // Init code here
        });
    };
    angular.module('w88Mobile').controller('LoginCtrl', LoginCtrl);
    LoginCtrl.$inject = ['$scope']
})();