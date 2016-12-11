"use strict";
var LiquidarFactory = function ($http) {
    return new LiquidarService($http);
};
var LiquidarService= function ($http) {
    this.$http = $http;
    this.onDelete = new Eventos("onDelete");
    this.onLoadFacturas = new Eventos("onLoadFacturas");
    this.onError = new Eventos("onError");
    this.onLoad = new Eventos("onLoad");
    this.Config = Settings.getInstance();
};
LiquidarService.prototype = new ConEventos();
LiquidarService.prototype.buscarFacturas = function (empresa, FechaIni, FechaFin, divisa) {
    var root = this;
    root.$http.post(root.Config.rootURL + "/api/Operacion/OperacionApi/BuscarFacturas", {
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
LiquidarService.prototype.todasFacturas = function () {
    var root = this;
    root.$http.post(root.Config.rootURL + "/api/Operacion/OperacionApi/BuscarFacturas").success(function (response) {
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
LiquidarService.prototype.eliminar = function (id) {
    var root = this;
    root.$http.post(root.Config.rootURL + "/api/Operacion/OperacionApi/PagaDocumentos", {
        idFactura: id
    }).success(function (response) {
        var salida = response;
        root.onDelete.onEvent(response);
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
LiquidarService.prototype.eliminarLista = function (arg) {
    var root = this;
    root.$http.post(root.Config.rootURL + "/api/Operacion/OperacionApi/PagaDocumentos", arg)
        .success(function (response) {
            var salida = response;
            root.onDelete.onEvent(response);
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
LiquidarService.prototype.load = function () {
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
LiquidarFactory.$inject = ['$http'];