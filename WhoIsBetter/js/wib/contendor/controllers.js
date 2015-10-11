'use strict';

angular.module('Contendor')

.controller('ContendorController',
    ['$scope', '$routeParams', '$location', '$rootScope', '$sce', 'FileUploader', 'ContendorService',
    function ($scope, $routeParams, $location, $rootScope, $sce, FileUploader, ContendorService) {

        var path = $location.path();
        
        switch (path) {
            case '/Contendores': {

                $scope.idTorneo = $routeParams.idTorneo;

                $scope.getHTML = function (imagen) {
                    return $sce.trustAsHtml('<img width="48" src="' + imagen + '" />');
                };

                ContendorService.obtenerContendores($routeParams.idTorneo, function (response) {
                    $scope.contendores = response;
                });

                $scope.confirmationAction = function (idContendor, callback) {
                    ContendorService.eliminarContendor(idContendor, function (response) {
                        ContendorService.obtenerContendores($routeParams.idTorneo, function (response) {
                            $scope.contendores = response;
                            callback();
                        });
                    });
                }
            } break;
            case '/GestionarContendor': {

                var uploader = $scope.uploader = new FileUploader({
                    url: 'api/upload',
                    queueLimit: 2
                });

                // FILTERS

                uploader.filters.push({
                    name: 'imageFilter',
                    fn: function (item /*{File|FileLikeObject}*/, options) {
                        var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                        return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
                    }
                });

                // CALLBACKS
                var removeBackground = false;

                uploader.onAfterAddingFile = function (fileItem) {
                    var queue = $scope.uploader.queue;
                    if (queue.length > 1) {
                        queue.shift();
                    }
                    removeBackground = true;
                };
                uploader.onBeforeUploadItem = function (item) {
                    debugger;
                    if ($scope.mode === 'new') {
                        item.url = 'http://localhost/WhoIsBetter_WSREST/api/torneo/' + $routeParams.idTorneo + '/contendor';
                    }
                    else {
                        item.url = 'http://localhost/WhoIsBetter_WSREST/api/contendor/' + $routeParams.idContendor;
                        item.method = "PUT";
                    }

                    item.headers['Authorization'] = 'Basic ' + $rootScope.globals.usuarioActual.token;
                    item.formData = [{ entity: JSON.stringify(entity) }];
                };
                uploader.onSuccessItem = function (fileItem, response, status, headers) {
                    if (response.success) {
                        history.back();
                    } else {
                        $scope.error = response.message;
                        $scope.dataLoading = false;
                    }
                };
                $scope.mode = $routeParams.mode.toLowerCase();

                var entity = { id: 0 };

                if ($scope.mode === 'edit') {
                    $scope.dataLoading = true;

                    $scope.imageAsBackground = function (imagen) {
                        if (!imagen) return '';
                        if (removeBackground) return '';

                        return $sce.trustAsHtml('background: url(' + imagen + ') no-repeat center; background-size: 132px;');
                    };

                    ContendorService.obtenerContendor($routeParams.idContendor, function (response) {
                        angular.extend(entity, response);
                        angular.extend($scope, entity);
                        $scope.dataLoading = false;
                    });
                }

                $scope.guardarContendor = function () {
                    $scope.dataLoading = true;

                    entity.nombre = $scope.nombre;
                    entity.texto = $scope.texto;
                    
                    $scope.uploader.queue[0].upload();
                }
            } break;
            default:

        }
    }]);