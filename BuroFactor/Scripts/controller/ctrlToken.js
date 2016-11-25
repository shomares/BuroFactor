var ctrlToken = function ($scope, $location, ValidationTokenFactory) {


    $scope.diplayLoad = { 'display': 'none' };


    $scope.data = { token: $location.search().Token };


    ValidationTokenFactory.addManejadorEventos("onError", function (args, func) {
        $scope.diplayLoad = { 'display': 'none' };
        alert(args);
        if (func !== undefined)
            func();
    });

    ValidationTokenFactory.addManejadorEventos("onvalidateToken", function (args, func) {
        $scope.diplayPlanes = { 'display': 'none' };
        window.location.reload();
    });

    $scope.validateToken = function () {
        if ($scope.data.contrasena === $scope.data.validacionContrasena)
            ValidationTokenFactory.validateToken($scope.data);
        else
            alert('Las contrasenas no coinciden');
    };


};

ctrlToken.$inject = ['$scope', '$location', 'ValidationTokenFactory'];
root.factory('ValidationTokenFactory', ValidationTokenFactory);
root.controller('ctrlToken', ctrlToken);
