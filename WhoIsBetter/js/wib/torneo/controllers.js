'use strict';

angular.module('Torneo')

.controller('TorneoController',
    ['$scope', '$rootScope', '$routeParams', '$location', 'TorneoService',
    function ($scope, $rootScope, $routeParams, $location, TorneoService) {

        var path = $location.path();
        
        switch (path) {
            case '/': {
                $scope.idUsuario = $rootScope.globals.usuarioActual.id;

                $scope.dateFormat = function (json) {
                    var fecha = new Date(json);
                    fecha.setDate(fecha.getDate() + 1);

                    return $.datepicker.formatDate('dd/mm/yy', fecha);
                };

                $scope.getEstadoCss = function (id) {
                    return ({
                        '1': 'info',
                        '2': 'primary',
                        '3': 'success',
                        '4': 'danger'
                    })[id];
                };

                $scope.getEstado = function (id) {
                    return ({
                        '1': 'Inicial',
                        '2': 'En Curso',
                        '3': 'Finalizado',
                        '4': 'Cancelado'
                    })[id];
                };

                TorneoService.obtenerTorneos($scope.idUsuario, function (response) {
                    $scope.torneos = response;
                });

                $scope.confirmationAction = function (idTorneo, callback) {
                    TorneoService.eliminarTorneo(idTorneo, function (response) {
                        TorneoService.obtenerTorneos($scope.idUsuario, function (response) {
                            $scope.torneos = response;
                            callback();
                        });
                    });
                }
            } break;
            case '/Torneos': {
                $scope.dateFormat = function (json) {
                    var fecha = new Date(json);
                    fecha.setDate(fecha.getDate() + 1);

                    return $.datepicker.formatDate('dd/mm/yy', fecha);
                };

                $scope.getEstadoCss = function (id) {
                    return ({
                        '1': 'info',
                        '2': 'primary',
                        '3': 'success',
                        '4': 'danger'
                    })[id];
                };

                $scope.getEstado = function (id) {
                    return ({
                        '1': 'Inicial',
                        '2': 'En Curso',
                        '3': 'Finalizado',
                        '4': 'Cancelado'
                    })[id];
                };

                TorneoService.listarTorneos(function (response) {
                    $scope.torneos = response;
                });
            } break;
            case '/GestionarTorneo': {

                $scope.mode = $routeParams.mode.toLowerCase();

                var entity = {},
                    formatDate = function (json) {
                        var fecha = new Date(json);
                        fecha.setDate(fecha.getDate() + 1);
                        return $.datepicker.formatDate("dd/mm/yy", fecha);
                    };

                if ($scope.mode === 'edit') {
                    $scope.dataLoading = true;

                    TorneoService.obtenerTorneo($routeParams.idTorneo, function (response) {
                        response.fechaInicio = formatDate(response.fechaInicio);
                        response.fechaFin = formatDate(response.fechaFin);

                        angular.extend($scope, entity, response);
                        $scope.dataLoading = false;
                    });
                }

                $scope.onKeyUp = function () {
                    if (!$scope.nombre) {
                        $scope.enlace = '';
                        return;
                    }

                    $scope.enlace = 'http://localhost/WhoIsBetter/index.html#/Torneos';
                    $scope.enlace += ('/' + $scope.nombre.replace(/ /g, '').toLowerCase()) || '';
                };

                $scope.guardarTorneo = function () {
                    $scope.dataLoading = true;

                    var fechaInicio = $scope.fechaInicio.split('/').map(function (a) { return parseInt(a); });
                    var fechaFin = $scope.fechaFin.split('/').map(function (a) { return parseInt(a); });

                    entity.id = $scope.id || 0;
                    entity.enlace = $scope.enlace;
                    entity.nombre = $scope.nombre;
                    entity.cantidadParticipantes = $scope.cantidadParticipantes;
                    entity.cantidadContendores = $scope.cantidadContendores;
                    entity.fechaInicio = new Date(fechaInicio[2], fechaInicio[1] - 1, fechaInicio[0]).toJSON();
                    entity.fechaFin = new Date(fechaFin[2], fechaFin[1] - 1, fechaFin[0]).toJSON();
                    entity.idEstado = 1;
                
                    if ($scope.mode === 'new') {
                        TorneoService.crearTorneo($routeParams.idUsuario, entity, function (response) {
                            if (response.success) {
                                $location.path('/');
                                $location.search('idUsuario', $routeParams.idUsuario);
                            } else {
                                $scope.error = response.message;
                                $scope.dataLoading = false;
                            }
                        });
                    } else {
                        TorneoService.actualizarTorneo(entity.id, entity, function (response) {
                            if (response.success) {
                                history.back();
                            } else {
                                $scope.error = response.message;
                                $scope.dataLoading = false;
                            }
                        });
                    }
                }
            } break;
            case '/ReporteTorneo':
                {
                    function doFixture(root, table, rows) {

                        //meet the following criteria
                        /*
                            ((2^0)x4)-2 ; ((2^1)x4)-2 ; ((2^2)x4)-2 ; ((2^3)x4)-2 ...
                        */
                        var cols = (((root / 2) * 4) - 2);
                        var isCombined;
                        var node = { 'current': root, 'old': 0, 'new': 0 };
                        var reverse;
                        var operation = true;
                        var nodes = [];

                        var calcBlockEmpty = function () {
                            var w = (operation ? (node['current'] / 2) : (node['current'] * 2));
                            if (w == 1) {
                                if (reverse === undefined) { reverse = true; }
                                if (reverse === false) { reverse = null; return; }
                            }

                            if (reverse === true) { reverse = false; }
                            else {
                                if (reverse === undefined) {
                                    if (operation) {
                                        node['new'] = ((((root / node['current']) * 2) - 1) * 2);
                                        node['current'] = w;
                                        nodes[nodes.length] = { 'current': node['current'], 'old': node['old'], 'new': node['new'] };
                                    } else {
                                        var lastIndex = nodes.length - 1;
                                        node = nodes[lastIndex];
                                        nodes.splice(lastIndex, 1);
                                    }
                                } else {
                                    if (reverse == null) {
                                        operation = false;
                                        reverse = undefined;
                                    }
                                }
                            }
                        }

                        //initialize
                        calcBlockEmpty();

                        var isCalled;
                        var onlyTwoRow = 0;

                        for (var r = 0; r < rows; r++) {
                            var $row = $('<tr></tr>');
                            var onlyTwoCell = 0;
                            var newNode = 0;
                            var stepRow = -1;
                            var sw = false;
                            for (var c = 0; c < cols; c++) {
                                var $col = $("<td></td>");

                                //are the first and last rows
                                if (r <= 1 || r >= (rows - 2)) {
                                    //if ((c%2)==1) => is odd => continue;
                                    if ((c % 2) == 1 && (c & 2) == 0) { $col = $("<td toremove=true></td>"); }
                                    if ((c % 2) == 0 && (c & 2) == 0) {
                                        if (!isCombined) {
                                            $col = $("<td tocombine=true></td>");
                                            isCombined = false;
                                        }
                                        else { if (isCombined) { $col = $("<td toremove=true></td>"); } }
                                    }
                                } else {
                                    //after of row´s space
                                    if ((r % 3) == 0 || (r % 3) == 1) {
                                        //--------------
                                        var doNewNode = function () {
                                            if (onlyTwoCell > 1) {
                                                newNode = node['new'] + c;
                                                onlyTwoCell = 0;
                                            }
                                        }

                                        if (newNode <= node['new']) {
                                            if (newNode == 0) { newNode = node['old']; }
                                            else { doNewNode(); }
                                        } else { doNewNode(); }

                                        if ((c & newNode) == newNode) {
                                            if (onlyTwoRow < 1) {
                                                if (onlyTwoCell == 0) {
                                                    sw = true;
                                                    $col = $("<td tocombine=true></td>");
                                                }
                                                onlyTwoCell++;
                                                isCalled = false;

                                                if (onlyTwoCell == 2) { $col = $("<td toremove=true></td>"); }
                                            }
                                            else {
                                                stepRow++;
                                                //if (stepRow < (node["current"]*2)){ $col = $("<td toremove=true>x</td>"); }
                                                if (onlyTwoCell < 2) { $col = $("<td toremove=true></td>"); }
                                                onlyTwoCell++;
                                            }
                                        }

                                    } else {

                                        if (!isCalled) {
                                            node['old'] = node['new'];
                                            calcBlockEmpty();
                                            isCalled = true;
                                        }

                                        //is row´s space
                                    }

                                }

                                $col.appendTo($row);
                            }

                            //activa el salto de linea
                            if (sw) { onlyTwoRow++; }
                            else { onlyTwoRow = 0; }

                            //resetear variable "isCombined"
                            isCombined = ((!isCombined) ? true : undefined);

                            $row.appendTo(table);

                        }
                    }

                    function drawLines($fixture) {
                        var excludeTd;
                        var cerradura = true;
                        var cancelLineas = false;
                        $fixture.find("td:empty:not([colspan])").each(function (i, td) {
                            var $td = $(td);

                            if (excludeTd != undefined) {
                                if ((excludeTd + 1) == td.cellIndex) {
                                    excludeTd = undefined;
                                    return;
                                }
                                else excludeTd = undefined;
                            }

                            var prev = $td.parent().prev().prev();
                            var hasCSInPrev = (prev.find("td").eq(td.cellIndex).attr("tocombine") != undefined);
                            var next = $td.parent().next();
                            var hasCSInNext = (next.find("td").eq(td.cellIndex).attr("tocombine") != undefined);

                            //there are colspan in Top & Bottom
                            if (hasCSInPrev && hasCSInNext) {
                                $td.attr("class", "line-br"); excludeTd = td.cellIndex;
                            } else {
                                var recursive = function (tD, attrName, action, clone) {
                                    if (tD.length > 0) {
                                        if ((action == "n" ? tD.next() : tD.prev()).attr(attrName) != undefined) return true;
                                        else {
                                            var result = recursive((action == "n" ? tD.next() : tD.prev()), attrName, action, (action == "n" ? clone.next() : clone.prev()));
                                            if (result) {
                                                tD.attr("class", "line-bb");
                                                clone.attr("class", "line-bt");
                                                (action == "n" ? tD.next() : tD.prev()).attr("class", "line-bb");
                                                (action == "n" ? clone.next() : clone.prev()).attr("class", "line-bt");
                                            }
                                            return result;
                                        }
                                    } return false;
                                };

                                if (cancelLineas) { return; }

                                cancelLineas = (prev.find("td[tocombine]").length == 1)

                                if (hasCSInPrev) {
                                    var self = $td.parent().prop("rowIndex");
                                    var index = next.prop("rowIndex");

                                    if (cerradura == false) {
                                        var tdx = next.find("td").eq(td.cellIndex);
                                        var clone = $fixture.find("tr:nth-child(" + ($fixture.find("tr").length - index) + ")").find("td").eq(td.cellIndex);
                                        hasCSInPrev = recursive(tdx, "toremove", "p", clone);
                                        if (hasCSInPrev) {
                                            cerradura = true;
                                            tdx.attr("class", "line-br line-bb");
                                            clone.attr("class", "line-br line-bt");
                                            $td.attr("class", "line-br");
                                            $fixture.find("tr:nth-child(" + ($fixture.find("tr").length - self) + ")").find("td").eq(td.cellIndex).attr("class", "line-br");
                                            excludeTd = td.cellIndex;
                                        }
                                    }
                                    else {
                                        var tdx = next.find("td").eq(td.cellIndex).next();
                                        var clone = $fixture.find("tr:nth-child(" + ($fixture.find("tr").length - index) + ")").find("td").eq(td.cellIndex).next();
                                        hasCSInNext = recursive(tdx, "tocombine", "n", clone);
                                        if (hasCSInNext) {
                                            cerradura = false;
                                            tdx.attr("class", "line-bl line-bb");
                                            clone.attr("class", "line-bl line-bt");
                                            $td.attr("class", "line-br");
                                            $fixture.find("tr:nth-child(" + ($fixture.find("tr").length - self) + ")").find("td").eq(td.cellIndex).attr("class", "line-br");
                                            excludeTd = td.cellIndex;
                                        }
                                    }
                                }
                            }
                        });

                        $fixture.find("td[tocombine]").prop({ colspan: '2', rowspan: '2' }).attr("class", "line-b").removeAttr("tocombine");
                        $fixture.find("td[toremove]").remove();

                        $fixture.css('margin-left', -($fixture.width() / 2) + 'px');
                    }

                    TorneoService.obtenerReporteTorneo($routeParams.idTorneo, function (response) {

                        var torneo = response[0].torneo;
                        $scope.nombreTorneo = torneo.nombre;
                        //$("#FechaInicio").text(torneo.fechaInicio);
                        //$("#FechaFin").text(torneo.fechaFin);
                        //$("#NumContendores").text(torneo.numContendores);
                        //$("#NumParticipantes").text(torneo.numParticipantes);
                        console.info(response);

                        doFixture(torneo.cantidadContendores, $('.fixture'), { 2: 8, 4: 14, 8: 20, 16: 26, 32: 32, 64: 38 }[torneo.cantidadContendores])
                        drawLines($('.fixture'));

                        var boxes = $('.fixture').find('td.line-b');
                        var fHeight = $('.fixture').height() - 1;
                        var center = (Math.round(boxes.length / 2)) - 1;

                        var j = 0;
                        var img = '<img class="rotate" width="53" height="53" />';

                        var __ = function (left) {
                            var data = boxes.filter(function (i, elm) {
                                return $(elm).position().left == left;
                            });

                            data.sort(function (a, b) {
                                return $(a).position().top - $(b).position().top;
                            });

                            for (var i = 0; i < data.length; i += 2) {
                                $(data[i]).append($(img).attr("src", response[j].contendor1.imagen).attr("title", response[j].contendor1.nombre));
                                $(data[i + 1]).append($(img).attr("src", response[j].contendor2.imagen).attr("title", response[j].contendor2.nombre));
                                j++;
                            }
                        };

                        var lC = boxes.eq(center - 1);
                        var c = boxes.eq(center);
                        var rC = boxes.eq(center + 1);
                        
                        var bWidth = c.outerWidth();
                        var w = 28 + bWidth;

                        var lI = -w;
                        var lF = (fHeight - bWidth) + w;

                        while (true) {
                            lI += w;
                            lF -= w;

                            if (lI == lC.position().left && lF == rC.position().left) break;

                            __(lI);
                            __(lF);
                        }
                        
                        if (response.length - 1 === j) {
                            lC.append($(img).attr("src", response[j].contendor1.imagen).attr("title", response[j].contendor1.nombre));
                            c.append($(img).attr("src", response[j].ganador.imagen).attr("title", response[j].ganador.nombre));
                            rC.append($(img).attr("src", response[j].contendor2.imagen).attr("title", response[j].contendor2.nombre));
                        }
                    })
                } break;
            default:

        }
    }]);