"use strict";

var ValidationTokenFactory = function ($http) {
    return new ValidationTokenService($http);
};

var ValidationTokenService = function ($http) {
    this.$http = $http;
    this.onError = new Eventos("onError");
    this.onvalidateToken = new Eventos("onvalidateToken");
    this.Config = Settings.getInstance();
};

ValidationTokenService.prototype = new ConEventos();
ValidationTokenService.prototype.validateToken = function (data) {
    var root = this;
    this.$http.post(root.Config.rootURL + "/api/Contratos/TokenApi/validateToken", data)
        .success(function (args) {
            root.onvalidateToken.onEvent(args);
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