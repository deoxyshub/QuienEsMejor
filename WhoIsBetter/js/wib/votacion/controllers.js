'use strict';

angular.module('Votacion')

.controller('VotacionController',
    ['$scope', '$location', '$sce', 'VotacionService',
    function ($scope, $location, $sce, VotacionService) {

        var rondas = {
            16: 'Octavos de final',
            8: 'Cuartos de final',
            4: 'Semifinal',
            2: 'Final'
        };

        var dialog = $('#inscripcion').dialog({
            resizable: false,
            modal: true,
            close: function () {
                if ($scope.dataLoading1 === false) return;

                $location.path('/Torneos').replace();
                $scope.$apply()
            }
        });

        var obtenerVotacion = function () {
            var etapas = [];

            VotacionService.obtenerContendoresAndFavoritos($scope.idTorneo, $scope.idParticipante, function (contendores, favoritos) {

                if (favoritos.filter(function (o) { return o.etapa == 2; }).length > 0) {
                    $scope.dataLoading1 = false;
                    dialog.dialog("close");

                    $location.path('/Torneos');
                    //$location.search('idTorneo', $scope.idTorneo);
                    return;
                }

                var fIDs;
                var fLength = favoritos.length;

                if (fLength > 0) {
                    for (var i = 0; i < fLength; i++) {
                        var favorito = favoritos[i];
                        var exponent = Math.log(favorito.etapa) / Math.log(2);
                        var records = Math.pow(2, exponent - 1);
                        var data = favoritos.filter(function (o) { return o.etapa == favorito.etapa; }).map(function (o) { return o.idGanador; });

                        if (records != data.length) {
                            if (fIDs) {
                                contendores = contendores.filter(function (o) { return fIDs.indexOf(o.id) !== -1; });
                            }
                            fIDs = data;
                            break;
                        } else {
                            fIDs = data;
                            $scope.etapa = favorito.etapa;

                            contendores = contendores.filter(function (o) { return fIDs.indexOf(o.id) !== -1; });

                            i += (data.length- 1);
                        }
                    }
                }

                $scope.etapa = contendores.length;
                etapas.push(rondas[$scope.etapa]);

                var dIndex;
                var duelos = [];

                [].forEach.call(contendores, function (item, index) {
                    if (index % 2 == 0) {
                        duelos.push({
                            idContendor1: item.id,
                            nombreContendor1: item.nombre,
                            imagenContendor1: item.imagen,
                            textoContendor1: item.texto
                        });
                        etapas.push('- Opción ' + (index + 1) + ' vs ');
                    }
                    else {
                        var xindex = duelos.length - 1;

                        duelos[xindex].idContendor2 = item.id;
                        duelos[xindex].nombreContendor2 = item.nombre;
                        duelos[xindex].imagenContendor2 = item.imagen;
                        duelos[xindex].textoContendor2 = item.text;

                        if (dIndex === undefined &&
                            fIDs &&
                            (fIDs.indexOf(duelos[xindex].idContendor1) === -1) &&
                            (fIDs.indexOf(duelos[xindex].idContendor2) === -1)) {
                            dIndex = xindex;
                        }

                        etapas[etapas.length - 1] += 'Opción ' + (index + 1);
                    }
                });

                angular.extend($scope, duelos[dIndex || 0]);
                $scope.etapas = etapas;

                var options = etapas[(dIndex || 0) + 1].split(' vs ');

                $scope.opcion1 = options[0].replace('- ', '');
                $scope.opcion2 = options[1];

                $scope.dataLoading1 = false;
                dialog.dialog("close");
            });
        };

        $scope.inscribirse = function () {
            $scope.dataLoading1 = true;

            var $this = $(this);
            
            VotacionService.obtenerTorneo($location.absUrl(), function (response) {

                $scope.idTorneo = response.id;
                $scope.nombreTorneo = response.nombre;
                $scope.etapa = response.cantidadContendores;

                VotacionService.inscribirParticipante($scope.idTorneo, { correo: $scope.correo, nombre: $scope.nombre }, function (response) {

                    if (!response.success) {
                        $scope.dataLoading1 = false;
                        dialog.dialog("close");

                        $location.path('/Torneos');
                        //$location.search('idTorneo', $scope.idTorneo);
                        return;
                    }

                    $scope.idParticipante = response.idParticipante;

                    obtenerVotacion();
                });
            });
        };

        $scope.getHTML = function (imagen) {
            if (!imagen) {
                return $sce.trustAsHtml('<img data-src="holder.js/140x140" class="img-thumbnail" alt="140x140" src="data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9InllcyI/PjxzdmcgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB3aWR0aD0iMTQwIiBoZWlnaHQ9IjE0MCIgdmlld0JveD0iMCAwIDE0MCAxNDAiIHByZXNlcnZlQXNwZWN0UmF0aW89Im5vbmUiPjwhLS0KU291cmNlIFVSTDogaG9sZGVyLmpzLzE0MHgxNDAKQ3JlYXRlZCB3aXRoIEhvbGRlci5qcyAyLjYuMC4KTGVhcm4gbW9yZSBhdCBodHRwOi8vaG9sZGVyanMuY29tCihjKSAyMDEyLTIwMTUgSXZhbiBNYWxvcGluc2t5IC0gaHR0cDovL2ltc2t5LmNvCi0tPjxkZWZzPjxzdHlsZSB0eXBlPSJ0ZXh0L2NzcyI+PCFbQ0RBVEFbI2hvbGRlcl8xNTAyZWM0ZTU5MSB0ZXh0IHsgZmlsbDojQUFBQUFBO2ZvbnQtd2VpZ2h0OmJvbGQ7Zm9udC1mYW1pbHk6QXJpYWwsIEhlbHZldGljYSwgT3BlbiBTYW5zLCBzYW5zLXNlcmlmLCBtb25vc3BhY2U7Zm9udC1zaXplOjEwcHQgfSBdXT48L3N0eWxlPjwvZGVmcz48ZyBpZD0iaG9sZGVyXzE1MDJlYzRlNTkxIj48cmVjdCB3aWR0aD0iMTQwIiBoZWlnaHQ9IjE0MCIgZmlsbD0iI0VFRUVFRSIvPjxnPjx0ZXh0IHg9IjQ1LjUiIHk9Ijc0LjUiPjE0MHgxNDA8L3RleHQ+PC9nPjwvZz48L3N2Zz4=" data-holder-rendered="true" style="width: 140px; height: 140px;">');
            }

            return $sce.trustAsHtml('<img width="140" height="140" style="padding: 2px; border: 1px solid #ccc;" src="' + imagen + '" />');
        };

        $scope.votar = function () {
            $scope.dataLoading = true;

            var entity = {
                idTorneo: $scope.idTorneo,
                idParticipante: $scope.idParticipante,
                idContendor1: $scope.idContendor1,
                idContendor2: $scope.idContendor2,
                etapa: $scope.etapa
            };

            entity.idGanador = entity['idContendor' + $scope.marcaOpt];

            VotacionService.registrarFavorito($scope.idTorneo, $scope.idParticipante, entity, function (response) {
                $scope.dataLoading = false;

                if (response.success) {
                    obtenerVotacion();
                } else {
                    $scope.error = response.message;
                }
            });
        };
    }]);