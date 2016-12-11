//Por qus
var root = angular.module('BuroFactor', ['angularFileUpload', 'ui.bootstrap'])
.config(function ($locationProvider, $httpProvider) {
    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });
    

    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
});
