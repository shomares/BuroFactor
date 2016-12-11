"use strict";
var OperacionVencidaFactory = function ($http) {
    return new OperacionVencidaService($http);
};
var OperacionVencidaService= function ($http) {
    this.$http = $http;
    this.onDelete = new Eventos("onDelete");
    this.onLoadFacturas = new Eventos("onLoadFacturas");
    this.onError = new Eventos("onError");
    this.onLoad = new Eventos("onLoad");
    this.Config = Settings.getInstance();
};
OperacionVencidaService.prototype = new ConEventos();
OperacionVencidaService.prototype.buscarFacturas = function (empresa, FechaIni, FechaFin, divisa) {
    var root = this;
    root.$http.post(root.Config.rootURL + "/api/Reporte/Engine/BuscarFacturasVencidas", {
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
OperacionVencidaService.prototype.todasFacturas = function () {
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
OperacionVencidaService.prototype.load = function () {
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
OperacionVencidaFactory.$inject = ['$http'];