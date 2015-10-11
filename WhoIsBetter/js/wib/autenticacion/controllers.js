'use strict';

angular.module('Autenticacion')

.controller('IniciarSesionController',
    ['$scope', '$rootScope', '$location', 'AutenticacionService',
    function ($scope, $rootScope, $location, AutenticacionService) {
        // reset login status
        AutenticacionService.limpiarCredenciales();

        $scope.iniciarSesion = function () {
            $scope.dataLoading = true;

            AutenticacionService.iniciarSesion($scope.usuario, $scope.clave, function (response) {
                if (response.success) {
                    AutenticacionService.establecerCredenciales(response.idUsuario, $scope.usuario, $scope.clave);
                    $location.path('/');
                } else {
                    $scope.error = response.message;
                    $scope.dataLoading = false;
                }
            });
        };

        $scope.registrarte = function () {
            $location.path('/Registrarte');
        };
    }]);