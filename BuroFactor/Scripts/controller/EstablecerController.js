var EstablecerController = function ($scope, EstablecerFactory) {
    $scope.diplayLoad = { 'display': 'none' };
    EstablecerFactory.addManejadorEventos("onError", function (args, func) {
        $scope.diplayLoad = { 'display': 'none' };
        alertify.alert(args, function () {
            if (func !== undefined)
                func();
        });
    });
    EstablecerFactory.addManejadorEventos("onEstablecer", function (args, func) {
        $scope.diplayPlanes = { 'display': 'none' };
        window.location.reload();
    });
    $scope.establecer = function () {
        $scope.diplayLoad = { 'display': 'block' };
        if ($scope.passwordNueva === $scope.passwordCopia)
            EstablecerFactory.establecer($scope.password, $scope.passwordNueva, $scope.passwordCopia);
        else
            alertify.alert('Las contrasenas no coinciden');
    };
};

EstablecerController.$inject = ['$scope', 'EstablecerFactory'];
root.factory('EstablecerFactory', EstablecerFactory);
root.controller('EstablecerController', EstablecerController);