'use strict';

angular.module('Usuario')

.controller('UsuarioController',
    ['$scope', '$location', '$routeParams', 'UsuarioService', 'AutenticacionService',
    function ($scope, $location, $routeParams, UsuarioService, AutenticacionService) {

        var path = $location.path();
        var entity = {};

        switch (path) {
            case '/Registrarte': {

                $scope.registrarCuenta = function () {
                    $scope.dataLoading = true;

                    entity.id = 0;
                    entity.correo = $scope.correo;
                    entity.nombre = $scope.nombre;
                    entity.apellidoPaterno = $scope.apellidoPaterno;
                    entity.apellidoMaterno = $scope.apellidoMaterno;
                    entity.clave = $scope.clave;

                    UsuarioService.registrarCuenta(entity, function (response) {
                        if (response.success) {
                            $location.path('/IniciarSesion');
                        } else {
                            $scope.error = response.message;
                            $scope.dataLoading = false;
                        }
                    });
                };

                $scope.iniciarSesion = function () {
                    $location.path('/IniciarSesion');
                };
            } break;
            case '/ModificarPerfil': {
                $scope.sexos = [{ key: 'Femenino', value: false }, { key: 'Masculino', value: true }];
                
                UsuarioService.obtenerCuenta($routeParams.idUsuario, function (response) {
                    var fecha = new Date(response.fechaNacimiento);

                    fecha.setDate(fecha.getDate() + 1);

                    $scope.usuario = response;
                    $scope.usuario.fechaNacimiento = $.datepicker.formatDate("dd/mm/yy", fecha);
                });

                $scope.modificarPerfil = function () {
                    $scope.dataLoading = true;

                    var fecha = $scope.usuario.fechaNacimiento.split('/').map(function (a) { return parseInt(a); });

                    entity.id = $scope.usuario.id;
                    entity.correo = $scope.usuario.correo;
                    entity.nombre = $scope.usuario.nombre;
                    entity.apellidoPaterno = $scope.usuario.apellidoPaterno;
                    entity.apellidoMaterno = $scope.usuario.apellidoMaterno;
                    entity.clave = $scope.usuario.clave;
                    entity.sexo = $scope.usuario.sexo;
                    entity.numeroCelular = $scope.usuario.numeroCelular;
                    entity.numeroTelefono = $scope.usuario.numeroTelefono;
                    entity.fechaNacimiento = new Date(fecha[2], fecha[1] - 1, fecha[0]).toJSON();

                    UsuarioService.modificarPerfil($scope.usuario.id, entity, function (response) {
                        if (response.success) {
                            history.back();
                        } else {
                            $scope.error = response.message;
                            $scope.dataLoading = false;
                        }
                    });
                }
            } break;
            case '/CambiarClave': {

                UsuarioService.obtenerCuenta($routeParams.idUsuario, function (response) {
                    angular.extend(entity, response);
                    angular.extend($scope, entity);
                });

                $scope.cambiarClave = function () {
                    entity.clave = $scope.newClave;

                    UsuarioService.modificarPerfil(entity.id, entity, function (response) {
                        if (response.success) {
                            AutenticacionService.establecerCredenciales(entity.correo, entity.clave);

                            $location.path('/IniciarSession');
                        } else {
                            $scope.error = response.message;
                            $scope.dataLoading = false;
                        }
                    });

                };
            } break;
        }
    }]);