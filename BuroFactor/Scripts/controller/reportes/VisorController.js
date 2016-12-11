"use strict";
var VisorController = function ($scope, url, $uibModalInstance) {
    $scope.url = url;
    $scope.salir = function () {
        $scope.token = "";
        if ($uibModalInstance !== null) {
            $uibModalInstance.close();
        }
    };
};