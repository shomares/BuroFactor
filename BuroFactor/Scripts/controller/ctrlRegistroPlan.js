var RegistroPlanController = function ($scope,  RegistroPlanFactory) {

    $scope.diplayPlanes = { 'display': 'none' };
    $scope.diplayFinancieras = { 'display': 'block' };
    $scope.diplayLoad = { 'display': 'block' };


    $scope.current = {financiera: null, plan: null};


    RegistroPlanFactory.addManejadorEventos("onError", function (args, func) {
        $scope.diplayPlanes = { 'display': 'none' };
        $scope.diplayFinancieras = { 'display': 'block' };
        $scope.diplayLoad = { 'display': 'none' };
        alert(args);
       
    });

    RegistroPlanFactory.addManejadorEventos("onBuscaFinanciera", function (args, func) {
        $scope.diplayPlanes = { 'display': 'none' };
        $scope.diplayFinancieras = { 'display': 'block' };
        $scope.diplayLoad = { 'display': 'none' };

        $scope.financieras = args;
    });
    RegistroPlanFactory.addManejadorEventos("onCargaPlanes", function (args, func) {
      
        $scope.planes = args;
        RegistroPlanFactory.cargaTodosPlanes();
    });
    RegistroPlanFactory.addManejadorEventos("onCargaPlanesTodos", function (args, func) {
        $scope.diplayPlanes = { 'display': 'block' };
        $scope.diplayFinancieras = { 'display': 'none' };
        $scope.diplayLoad = { 'display': 'none' };
        $scope.todosPlanes = args;
    });


    RegistroPlanFactory.addManejadorEventos("onSavePlanFinanciera", function (args, func) {
        alertify.alert('Se ha enviado un correo para poder activar el plan', function () {
            window.location.reload();
        });

    });

    $scope.guardaPlan = function () {
        $scope.diplayLoad = { 'display': 'block' };
        RegistroPlanFactory.savePlanFinanciera($scope.current);
    };
  


    $scope.cargaPlanes = function (idFinanciera) {
        var financiera = $scope.financieras.find(function (element) { return element.idFinanciera === idFinanciera; });
        $scope.diplayLoad = { 'display': 'block' };
        $scope.current.financiera = financiera;
        $scope.current.idFinanciera = idFinanciera;
        RegistroPlanFactory.cargaFinancieraPlan(idFinanciera);
    };

    $scope.buscaFinanciera = function () {
        $scope.diplayLoad = { 'display': 'block' };
        RegistroPlanFactory.buscaFinanciera($scope.RFC);
    };

    $scope.cancelarPlan = function (idPlan) {
        $scope.diplayLoad = { 'display': 'block' };
        RegistroPlanFactory.cancelarPlan(idPlan);
    };

    $scope.regresar = function () {
        window.location.reload();
    };

    RegistroPlanFactory.buscaFinanciera();


};

RegistroPlanController.$inject = ['$scope', 'RegistroPlanFactory'];
root.factory('RegistroPlanFactory', RegistroPlanFactory);
root.controller('RegistroPlanController', RegistroPlanController);
