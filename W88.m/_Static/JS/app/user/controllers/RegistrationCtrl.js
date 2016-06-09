(function () {
    var RegistrationCtrl = function ($scope, menuService) {
        var ctrl = this;
        ctrl.title = "Register";

        ons.ready(function () {
            // Init code here
        });
    };
    angular.module('w88Mobile').controller('RegistrationCtrl', RegistrationCtrl);
    RegistrationCtrl.$inject = ['$scope']
})();