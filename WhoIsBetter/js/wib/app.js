'use strict';

// declare modules
angular.module('Autenticacion', []);

var torneo = angular.module('Torneo', []);
var usuario = angular.module('Usuario', []);
var contendor = angular.module('Contendor', ['angularFileUpload']);
var votacion = angular.module('Votacion', []);

// directives
var datepicker = function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {
                element.datepicker({
                    dateFormat: 'dd/mm/yy',
                    onSelect: function (date) {
                        scope.$apply(function () {
                            ngModelCtrl.$setViewValue(date);
                        });
                    }
                });
            });
        }
    }
},
    passwordVerify = function () {
        return {
            require: "ngModel",
            scope: {
                passwordVerify: '='
            },
            link: function (scope, element, attrs, ctrl) {
                scope.$watch(function () {
                    var combined;

                    if (scope.passwordVerify || ctrl.$viewValue) {
                        combined = scope.passwordVerify + '_' + ctrl.$viewValue;
                    }
                    return combined;
                }, function (value) {
                    if (value) {
                        ctrl.$parsers.unshift(function (viewValue) {
                            var origin = scope.passwordVerify;
                            if (origin !== viewValue) {
                                ctrl.$setValidity("passwordVerify", false);
                                return undefined;
                            } else {
                                ctrl.$setValidity("passwordVerify", true);
                                return viewValue;
                            }
                        });
                    }
                });
            }
        };
    },
    spinner = function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                $(function () {
                    var $value = 0;

                    element.spinner({
                        min: 0,
                        max: 100,
                        spin: function (event, ui) {
                            $value = ui.value;
                            element.trigger("change");
                        }
                    }).change(function () {
                        element.val(element.val() || $value);
                    });
                });
            }
        }
    },
    openDialog = function () {
        return {
            restrict: 'A',
            link: function (scope, elem, attr, ctrl) {
                var dialogId = '#' + attr.openDialog;
                elem.bind('click', function (e) {
                    $(dialogId).dialog({
                        resizable: false,
                        modal: true,
                        buttons: {
                            "si": function () {
                                var $this = $(this);

                                scope.confirmationAction(attr.entityid, function () {
                                    $this.dialog("close");
                                });
                            },
                            "no": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                });
            }
        };
    },
    ngThumb = function ($window) {
        var helper = {
            support: !!($window.FileReader && $window.CanvasRenderingContext2D),
            isFile: function (item) {
                return angular.isObject(item) && item instanceof $window.File;
            },
            isImage: function (file) {
                var type = '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
                return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
            }
        };

        return {
            restrict: 'A',
            template: '<canvas/>',
            link: function (scope, element, attributes) {
                if (!helper.support) return;

                var params = scope.$eval(attributes.ngThumb);

                if (!helper.isFile(params.file)) return;
                if (!helper.isImage(params.file)) return;

                var canvas = element.find('canvas');
                var reader = new FileReader();

                reader.onload = onLoadFile;
                reader.readAsDataURL(params.file);

                function onLoadFile(event) {
                    var img = new Image();
                    img.onload = onLoadImage;
                    img.src = event.target.result;
                }

                function onLoadImage() {
                    var width = params.width || this.width / this.height * params.height;
                    var height = params.height || this.height / this.width * params.width;
                    canvas.attr({ width: width, height: height });
                    canvas[0].getContext('2d').drawImage(this, 0, 0, width, height);
                }
            }
        };
    };

// configuration
torneo.directive('datepicker', datepicker);

torneo.directive('spinner', spinner);

torneo.directive('openDialog', openDialog);

usuario.directive("passwordVerify", passwordVerify);

usuario.directive('datepicker', datepicker);

contendor.directive('ngThumb', ['$window', ngThumb]);

angular.module('BasicHttpAuthExample', [
    'Autenticacion',
    'Contendor',
    'Torneo',
    'Usuario',
    'Votacion',
    'ngRoute',
    'ngCookies'
])

.config(['$routeProvider', function ($routeProvider) {

    $routeProvider
        .when('/IniciarSesion', {
            controller: 'IniciarSesionController',
            templateUrl: 'js/wib/autenticacion/views/iniciarSesion.html'
        })

        .when('/Registrarte', {
            controller: 'UsuarioController',
            templateUrl: 'js/wib/usuario/views/registrarCuenta.html'
        })

        .when('/Torneos', {
            controller: 'TorneoController',
            templateUrl: 'js/wib/torneo/views/listarTorneos.html'
        })

        .when('/', {
            controller: 'TorneoController',
            templateUrl: 'js/wib/torneo/views/consultarTorneos.html'
        })

        .when('/ModificarPerfil', {
            controller: 'UsuarioController',
            templateUrl: 'js/wib/usuario/views/modificarPerfil.html'
        })

        .when('/CambiarClave', {
            controller: 'UsuarioController',
            templateUrl: 'js/wib/usuario/views/cambiarClave.html'
        })

        .when('/GestionarTorneo', {
            controller: 'TorneoController',
            templateUrl: 'js/wib/torneo/views/gestionarTorneo.html'
        })

        .when('/ReporteTorneo', {
            controller: 'TorneoController',
            templateUrl: 'js/wib/torneo/views/reporteTorneo.html'
        })

        .when('/Contendores', {
            controller: 'ContendorController',
            templateUrl: 'js/wib/contendor/views/consultarContendores.html'
        })

        .when('/GestionarContendor', {
            controller: 'ContendorController',
            templateUrl: 'js/wib/contendor/views/gestionarContendor.html'
        })

        .otherwise({
            redirectTo: function (routeParams, route, other) {
                if (route.split('/')[1] === 'Torneos') {
                    this.controller = 'VotacionController';
                    this.templateUrl = 'js/wib/votacion/views/votaPorTuFavorito.html';

                    return route;
                }

                return '/IniciarSesion';
            }
        });
}])

.run(['$rootScope', '$location', '$cookieStore', '$http',
    function ($rootScope, $location, $cookieStore, $http) {
        // keep user logged in after page refresh
        $rootScope.globals = $cookieStore.get('globals') || {};
        if ($rootScope.globals.usuarioActual) {
            $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.usuarioActual.token; // jshint ignore:line
        }

        $rootScope.$on('$locationChangeStart', function (event, next, current) {
            if ($location.path() === '/Registrarte' ||
                $location.path().split('/')[1] === 'Torneos' ||
                $location.path() === '/ReporteTorneo') return;

            // redirect to login page if not logged in
            if ($location.path() !== '/IniciarSesion' && !$rootScope.globals.usuarioActual) {
                $location.path('/#/IniciarSesion');
            }
        });
    }]);