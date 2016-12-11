"use strict";
var EstablecerFactory = function ($http) {
    return new EstablecerService($http);
};
var EstablecerService = function ($http) {
    this.$http = $http;
    this.onEstablecer = new Eventos("onEstablecer");
    this.onError = new Eventos("onError");
    this.Config = Settings.getInstance();

};
EstablecerService.prototype = new ConEventos();
EstablecerService.prototype.establecer = function (password, passwordNueva, passwordCopia) {
    var root = this;
    root.$http.post(root.Config.rootURL + "/api/Principal/EstablecerApi/Cuenta", {
        password: password,
        passwordNueva: passwordNueva,
        passwordCopia: passwordCopia
    }).success(function (response) {
        var salida = response;
        root.onEstablecer.onEvent(response);
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
EstablecerFactory.$inject = ['$http'];