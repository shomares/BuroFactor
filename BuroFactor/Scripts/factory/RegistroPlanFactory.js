"use strict";
var RegistroPlanFactory = function ($http) {
    return new RegistroPlanService($http);
};

var RegistroPlanService = function ($http) {
    this.$http = $http;
    this.onBuscaFinanciera = new Eventos("onBuscaFinanciera");
    this.onCargaPlanes = new Eventos("onCargaPlanes");

    this.onCargaPlanesTodos = new Eventos("onCargaPlanesTodos");
    this.onError = new Eventos("onError");
    this.onSavePlanFinanciera = new Eventos("onSavePlanFinanciera");
    this.Config = Settings.getInstance();
};

RegistroPlanService.prototype = new ConEventos();

RegistroPlanService.prototype.buscaFinanciera = function (rfc) {
    var root = this;
    if (rfc !== undefined || rfc=="") {
        this.$http.post(root.Config.rootURL + "/api/Contratos/CatalogoApi/getFinancierasPorRFC?RFC=" + rfc)
            .success(function (args) {
                root.onBuscaFinanciera.onEvent(args);
            }).error(function (args, status) {
                if (status === 401) {
                    root.onError.onEvent("Ha expirado la sesión", function () {
                        window.location.reload();
                    });
                } else
                    root.onError.onEvent(args, function () {
                        window.location.reload();
                    });
            });
    } else {
        this.$http.post(root.Config.rootURL + "/api/Contratos/CatalogoApi/getFinancieras")
                  .success(function (args) {
                      root.onBuscaFinanciera.onEvent(args);
                  }).error(function (args, status) {
                      if (status === 401) {
                          root.onError.onEvent("Ha expirado la sesión", function () {
                              window.location.reload();
                          });
                      } else
                          root.onError.onEvent(args, function () {
                              window.location.reload();
                          });
                  });

    }
};
RegistroPlanService.prototype.cargaFinancieraPlan = function (idFinanciera) {
    var root = this;
    if (idFinanciera !== null) {
        this.$http.post(root.Config.rootURL + "/api/Contratos/CatalogoApi/getPlanesPorFinanciera?financiera=" + idFinanciera)
            .success(function (args) {
                root.onCargaPlanes.onEvent(args);
            }).error(function (args, status) {
                if (status === 401) {
                    root.onError.onEvent("Ha expirado la sesión", function () {
                        window.location.reload();
                    });
                } else
                    root.onError.onEvent(args, function () {
                        window.location.reload();
                    });
            });
    }
};

RegistroPlanService.prototype.cargaTodosPlanes = function () {
    var root = this;
    this.$http.post(root.Config.rootURL + "/api/Contratos/CatalogoApi/getPlanes")
        .success(function (args) {
            root.onCargaPlanesTodos.onEvent(args);
        }).error(function (args, status) {
            if (status === 401) {
                root.onError.onEvent("Ha expirado la sesión", function () {
                    window.location.reload();
                });
            } else
                root.onError.onEvent(args, function () {
                    window.location.reload();
                });
        });

};

RegistroPlanService.prototype.savePlanFinanciera = function (data) {
    var root = this;
    this.$http.post(root.Config.rootURL + "/api/Contratos/RegistroPlanApi/registraContrato", data)
        .success(function (args) {
            root.onSavePlanFinanciera.onEvent(args);
        }).error(function (args, status) {
            if (status === 401) {
                root.onError.onEvent("Ha expirado la sesión", function () {
                    window.location.reload();
                });
            } else
                root.onError.onEvent(args, function () {
                    window.location.reload();
                });
        });

};



RegistroPlanService.$inject = ['$http'];