"use strict";
var OperacionCanceladaFactory = function ($http) {
    return new OperacionCanceladaService($http);
};
var OperacionCanceladaService= function ($http) {
    this.$http = $http;
    this.onDelete = new Eventos("onDelete");
    this.onLoadFacturas = new Eventos("onLoadFacturas");
    this.onError = new Eventos("onError");
    this.onLoad = new Eventos("onLoad");
    this.Config = Settings.getInstance();
};
OperacionCanceladaService.prototype = new ConEventos();
OperacionCanceladaService.prototype.buscarFacturas = function (empresa, FechaIni, FechaFin, divisa) {
    var root = this;
    root.$http.post(root.Config.rootURL + "/api/Reporte/Engine/BuscarFacturasCanceladas", {
        Empresa: empresa,
        FechaIni: FechaIni,
        FechaFin: FechaFin,
        Divisa: divisa
    }).success(function (response) {
        var salida = response;
        root.onLoadFacturas.onEvent(response);
    }).error(function (args, status) {
        if (status === 401) {
            root.onError.onEvent("Ha expirado la sesión", function () {
                window.location.reload();
            });
        } else root.onError.onEvent(args, function () {
            //window.location.reload();
        });
    });
};
OperacionCanceladaService.prototype.todasFacturas = function () {
    var root = this;
    root.$http.post(root.Config.rootURL + "/api/Reporte/Engine/BuscarFacturasActivas").success(function (response) {
        var salida = response;
        root.onLoadFacturas.onEvent(response);
    }).error(function (args, status) {
        if (status === 401) {
            root.onError.onEvent("Ha expirado la sesión", function () {
                //window.location.reload();
            });
        } else root.onError.onEvent(args, function () {
            //window.location.reload();
        });
    });
};
OperacionCanceladaService.prototype.load = function () {
    var root = this;
    root.$http.post(root.Config.rootURL + "/api/Operacion/OperacionApi/Catalogos").success(function (response) {
        var salida = response;
        root.onLoad.onEvent(response);
    }).error(function (args, status) {
        if (status === 401) {
            root.onError.onEvent("Ha expirado la sesión", function () {
                window.location.reload();
            });
        } else root.onError.onEvent(args, function () {
            window.location.reload();
        });
    });
};
OperacionCanceladaFactory.$inject = ['$http'];