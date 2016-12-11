"use strict";
var OperacionController = function ($scope, $location, $window, $uibModal, TokenFactory, FileUploader) {

    $scope.Config = Settings.getInstance();
    $scope.displayBusqueda = { 'display': 'block' };
    $scope.displayError = { 'display': 'none' };
    $scope.displayCorrecto = { 'display': 'none' };
    $scope.displayLoading = { 'display': 'none' };

    var uploader = $scope.uploader = new FileUploader({
        url: $scope.Config.rootURL + '/api/Operacion/OperacionApi/CargaLayout'
    });

    uploader.onCompleteItem = function (fileItem, response, status, headers) {
        if (response !== undefined) {
            if (response.error) {
                $scope.errores = response.errores;
                $scope.displayError = { 'display': 'block' };
                $scope.displayLoading = { 'display': 'none' };
                $scope.displayCorrecto = { 'display': 'none' };
                $scope.displayBusqueda = { 'display': 'none' };
            } else {
                if (response.exceptionMessage === undefined) {
                    $scope.token = response.token;
                    $scope.displayError = { 'display': 'none' };
                    $scope.displayLoading = { 'display': 'none' };
                    $scope.displayCorrecto = { 'display': 'block' };
                    $scope.displayBusqueda = { 'display': 'none' };
                    if (response.riesgos !== undefined && response.riesgos.length > 0)
                        $scope.riesgos = response.riesgos;
                } else
                    alertify.alert("Surgio un error grave: " + response.exceptionMessage, function () {
                        window.location.reload();
                    });
            }
        }
    };

    //uploader.onCompleteAll = function (args) {
    //};
    $scope.send = function () {
        $scope.displayLoading = { 'display': 'block' };
        var files = $scope.uploader.getNotUploadedItems();
        $scope.uploader.uploadItem(files.length - 1);
    };
    $scope.refrescar = function () {
        window.location.reload();
    };

    $scope.publicar = function () {
        $scope.displayLoading = { 'display': 'block' };

        var modalInstance = $uibModal.open({
            backdrop: 'static',
            keyboard: false,
            animation: true,
            templateUrl: window.location.origin + $scope.Config.rootURL + "Operacion/Token?id=" + $scope.token,
            controller: 'ValidateTokenController',
            resolve: {
                id: function () {
                    return $scope.token;
                }
            }
        });
        modalInstance.result.then(function (args) {
            var message = '';
            if (args.notificacion === undefined) message = args;
            else message = args.notificacion;

            alertify.alert("<p style='font-size:large'>" + message + "</p></div>", function () {
                window.location.reload();
            });
        }, function (args) {
            $window.location.reload();
        });
    };
};
OperacionController.$inject = ['$scope', '$location', '$window', '$uibModal', 'TokenFactory', 'FileUploader'];
root.factory('TokenFactory', TokenFactory);
root.controller('OperacionController', OperacionController);