"use strict";
var LoginFactory = function ($http) {
    return new LoginService($http);
};
var LoginService = function ($http) {
    this.$http = $http;
    this.onLogin = new Eventos("onLogin");
    this.onError = new Eventos("onError");
    this.Config = Settings.getInstance();
};
LoginService.prototype = new ConEventos();
LoginService.prototype.login = function (username, password, page, antiFogeryToken) {
    var root = this;
    root.$http({
        method: 'POST',
        url: window.location.origin + Settings.getInstance().rootURL + "Home/Logger?ReturnUrl=" + page,
        data: {
            Username: username,
            Password: password
        },
        headers: {
            'RequestVerificationToken': antiFogeryToken
        }
    }).success(function (response) {
        var salida = response;
        if (salida.State === 1)
            root.onLogin.onEvent(response);
        else
            root.onError.onEvent(response);
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
LoginFactory.$inject = ['$http'];