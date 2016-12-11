"use strict";
var TokenFactory = function ($http) {
    return new TokenService($http);
};
var TokenService = function ($http) {
    this.$http = $http;
    this.Config = Settings.getInstance();
    this.onSuccess = new Eventos("onSuccess");
    this.on404 = new Eventos("on404");
    this.onErrorToken = new Eventos("onErrorToken");
    this.onErrorProceso = new Eventos("onErrorProceso");
    this.onError = new Eventos("onError");
    this.errorToken = "";
    this.id = "";
    var root = this;
    
};
TokenService.prototype = new ConEventos();

TokenService.prototype.validateToken = function (idWork, token, antiForgeryToken) {
    var root = this;
    root.$http({
        method: 'POST',
        url: root.Config.rootURL + "/api/Operacion/TokenValidate/Execute",
        data: {
            IdTicket: idWork,
            Token: token
        },
        headers: {
            'RequestVerificationToken': antiForgeryToken
        }
    }).success(function (response, status) {
        if (status !== 403) {
            if (response !== null && response!=="null") {
                if (response.notificacion !== undefined)
                    root.onSuccess.onEvent(response);
            } else
                root.onErrorToken.onEvent(root.errorToken);
        } else root.on404.onEvent("El recurso esta protegido");
    }).error(function (args, state) {
        if (state !== 403 || state !== 401) root.onError.onEvent('Ha expirado el tiempo de espera, no se completó la operación.');
        else root.on404.onEvent('Ha expirado el tiempo de espera, no se completó la operación.');
    });
};
TokenService.$inject = ['$http'];