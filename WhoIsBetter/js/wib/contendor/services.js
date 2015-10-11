'use strict';

angular.module('Contendor')

.factory('ContendorService', 
    ['$http',
    function ($http) {
        var service = {};

        service.crearContendor = function (idTorneo, entity, callback) {
            $http.post('http://localhost/WhoIsBetter_WSREST/api/torneo/' + idTorneo + '/contendor', entity)
                .success(function (response) {
                    callback(response);
                });
        };

        service.obtenerContendor = function (idContendor, callback) {
            $http.get('http://localhost/WhoIsBetter_WSREST/api/contendor/' + idContendor)
                .success(function (response) {
                    callback(response);
                });
        };

        service.actualizarContendor = function (idContendor, entity, callback) {
            $http.put('http://localhost/WhoIsBetter_WSREST/api/contendor/' + idContendor, entity)
                .success(function (response) {
                    callback(response);
                });
        };

        service.eliminarContendor = function (idContendor, callback) {
            $http.delete('http://localhost/WhoIsBetter_WSREST/api/contendor/' + idContendor)
                .success(function (response) {
                    callback(response);
                });
        };

        service.obtenerContendores = function (idTorneo, callback) {
            $http.get('http://localhost/WhoIsBetter_WSREST/api/torneo/' + idTorneo + '/contendor')
                .success(function (response) {
                    callback(response);
                });
        };

        return service;
    }]);