'use strict';

angular.module('Usuario')

.factory('UsuarioService', 
    ['$http',
    function ($http) {
        var service = {};

        service.registrarCuenta = function (entity, callback) {
            $http.post('http://localhost/WhoIsBetter_WSREST/api/usuario', entity)
                .success(function (response) {
                    callback(response);
                });
        };

        service.obtenerCuenta = function (idUsuario, callback) {
            $http.get('http://localhost/WhoIsBetter_WSREST/api/usuario/' + idUsuario)
                .success(function (response) {
                    callback(response);
                });
        };

        service.modificarPerfil = function (idUsuario, entity, callback) {
            $http.put('http://localhost/WhoIsBetter_WSREST/api/usuario/' + idUsuario, entity)
                .success(function (response) {
                    callback(response);
                });
        };

        return service;
    }]);