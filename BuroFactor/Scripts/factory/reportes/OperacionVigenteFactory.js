"use strict";
var OperacionVigenteFactory = function ($http) {
    return new OperacionVigenteService($http);
};
var OperacionVigenteService= function ($http) {
    this.$http = $http;
    this.onDelete = new Eventos("onDelete");
    this.onLoadFacturas = new Eventos("onLoadFacturas");
    this.onError = new Eventos("onError");
    this.onLoad = new Eventos("onLoad");
    this.Config = Settings.getInstance();
};
OperacionVigenteService.prototype = new ConEventos();
OperacionVigenteService.prototype.buscarFacturas = function (empresa, FechaIni, FechaFin, divisa) {
    var root = this;
    root.$http.post(root.Config.rootURL + "/api/Reporte/Engine/BuscarFacturasActivas", {
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
OperacionVigenteService.prototype.todasFacturas = function () {
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
OperacionVigenteService.prototype.load = function () {
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
OperacionVigenteFactory.$inject = ['$http'];