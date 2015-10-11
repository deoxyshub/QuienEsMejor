'use strict';

angular.module('Torneo')

.factory('TorneoService', 
    ['$http',
    function ($http) {
        var service = {};

        service.crearTorneo = function (idUsuario, entity, callback) {
            $http.post('http://localhost/WhoIsBetter_WSREST/api/usuario/' + idUsuario + '/torneo', entity)
                .success(function (response) {
                    callback(response);
                });
        };

        service.obtenerTorneo = function (idTorneo, callback) {
            $http.get('http://localhost/WhoIsBetter_WSREST/api/torneo/' + idTorneo)
                .success(function (response) {
                    callback(response);
                });
        };

        service.actualizarTorneo = function (idTorneo, entity, callback) {
            $http.put('http://localhost/WhoIsBetter_WSREST/api/torneo/' + idTorneo, entity)
                .success(function (response) {
                    callback(response);
                });
        };

        service.eliminarTorneo = function (idTorneo, callback) {
            $http.delete('http://localhost/WhoIsBetter_WSREST/api/torneo/' + idTorneo)
                .success(function (response) {
                    callback(response);
                });
        };

        service.obtenerTorneos = function (idUsuario, callback) {
            $http.get('http://localhost/WhoIsBetter_WSREST/api/usuario/' + idUsuario + '/torneo')
                .success(function (response) {
                    callback(response);
                });
        };

        service.listarTorneos = function (callback) {
            $http.get('http://localhost/WhoIsBetter_WSREST/api/torneo')
                .success(function (response) {
                    callback(response);
                });
        };

        service.obtenerReporteTorneo = function (idTorneo, callback) {
            $http.get('http://localhost/WhoIsBetter_WSREST/api/torneo/' + idTorneo + '/fixture')
                .success(function (response) {
                    callback(response);
                });
        };

        return service;
    }]);