

function cargarImagenEsperando() {
    var loading1 = document.createElement("div");
    loading1.id = "loading1";
    loading1.style.color = "white";
    loading1.style.backgroundColor = "white";
    loading1.style.position = "absolute";
    loading1.style.right = "0px";
    loading1.style.top = "0px";
    loading1.style.zIndex = "9998";
    loading1.style.width = screen.width;
    loading1.style.height = screen.height;
    loading1.className = 'transparente';
    document.body.appendChild(loading1);

    var loading = document.createElement("div");
    loading.id = "loading";
    loading.style.position = "absolute";
    loading.style.right = screen.width / 3;
    loading.style.top = screen.height / 2;
    loading.style.zIndex = "9999";
    loading.innerHTML = "<img src='../../Imagenes/cargando3.gif'>";
    document.body.appendChild(loading);
}

function quitarImagenEsperando() {
    var loading1 = document.getElementById("loading1");
    if (loading1 != null)
        document.body.removeChild(loading1);
    var loading = document.getElementById("loading");
    if (loading != null)
        document.body.removeChild(loading);
}

function autoSizeIframe() {
    var id = document.getElementById('ifraCondicionesVenta');
    if (!window.opera && document.all && document.getElementById) {
        id.style.height = id.contentWindow.document.body.scrollHeight;
    } else if (document.getElementById) {
        id.style.height = id.contentDocument.body.scrollHeight + "px";
    }
}

function cambiarAltoIframe(alto) {
    var id = document.getElementById('ifraCondicionesVenta');
    if (!window.opera && document.all && document.getElementById) {
        id.style.height = alto + "px";
    } else if (document.getElementById) {
        id.style.height = id.contentDocument.body.scrollHeight + "px";
    }
}

function autoSizeIframeLineas() {
    var id = document.getElementById('ifrLineasDesactivas');
    if (!window.opera && document.all && document.getElementById) {
        id.style.height = id.contentWindow.document.body.scrollHeight;
    } else if (document.getElementById) {
        id.style.height = id.contentDocument.body.scrollHeight + "px";
    }
}

function mostrarAllTabProducto(visible) {
    setVisible('tdMovil', visible);
    setVisible('tdFijo', visible);
    setVisible('tdDTH', visible);
    setVisible('tdBAM', visible);
    setVisible('tdHFC', visible);
    setVisible('tdVentaVarios', visible);
    setVisible('td3PlayInalam', visible);
    setVisible('tdInterInalam', visible); //PROY-31812 - IDEA-43340
    setVisible('tdFTTH', visible); //FTTH
}

function mostrarAllTabInactivo() {
    document.getElementById('tdMovil').className = 'tab_inactivo';
    document.getElementById('tdFijo').className = 'tab_inactivo';
    document.getElementById('tdBAM').className = 'tab_inactivo';
    document.getElementById('tdDTH').className = 'tab_inactivo';
    document.getElementById('tdHFC').className = 'tab_inactivo';
    document.getElementById('tdVentaVarios').className = 'tab_inactivo';
    document.getElementById('td3PlayInalam').className = 'tab_inactivo';
    document.getElementById('tdInterInalam').className = 'tab_inactivo'; //PROY-31812 - IDEA-43340
    document.getElementById('tdFTTH').className = 'tab_inactivo'; //FTTH
}

function mostrarTabActivo(idProducto) {
    document.getElementById(idProducto).className = 'tab_activo';
}

function mostrarTabProducto(idProducto, visible) {
    switch (idProducto) {
        case codTipoProductoMovil: setVisible('tdMovil', visible); break;
        case codTipoProductoFijo: setVisible('tdFijo', visible); break;
        case codTipoProductoDTH: setVisible('tdDTH', visible); break;
        case codTipoProductoBAM: setVisible('tdBAM', visible); break;
        case codTipoProductoHFC: setVisible('tdHFC', visible); break;
        case codTipoProductoVentaVarios: setVisible('tdVentaVarios', visible); break;
        case codTipoProd3PlayInalam: setVisible('td3PlayInalam', visible); break;
        case codTipoProdInterInalam: setVisible('tdInterInalam', visible); break; //PROY-31812 - IDEA-43340
        case codTipoProductoFTTH: setVisible('tdFTTH', visible); break; //FTTH
    }
}

function textoProducto(idProducto) {
    var strProducto = '';
    switch (idProducto) {
        case codTipoProductoMovil: strProducto = 'Movil'; break;
        case codTipoProductoFijo: strProducto = 'Fijo'; break;
        case codTipoProductoDTH: strProducto = 'DTH'; break;
        case codTipoProductoBAM: strProducto = 'BAM'; break;
        case codTipoProductoHFC: strProducto = 'HFC'; break;
        case codTipoProductoFTTH: strProducto = 'FTTH'; break; //FTTH - function textoProducto(idProducto)
        case codTipoProductoVentaVarios: strProducto = 'VentaVarios'; break;
        case codTipoProd3PlayInalam: strProducto = '3PlayInalam'; break;
    }
    return strProducto;
}

// ******************************************************* REGLAS COMERCIALES ******************************************************* //

function nroPaqEvaluadosHFC() {
    var strCadenaDetalle = self.frames['ifraCondicionesVenta'].consultarItem('');
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var idProducto;
    var idFila;
    var nroPlanes = 0;
    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            idFila = arrCadenaDetalle[i].split(';')[0];
            idProducto = arrCadenaDetalle[i].split(';')[1];
            if (idProducto == codTipoProductoHFC || idProducto == codTipoProductoFTTH) { 
                var strGrupoPaquete = arrCadenaDetalle[i].split(';')[8];
                var idFilaPaquete = obtenerFilaPaquete(strGrupoPaquete);
                if (idFila == idFilaPaquete) {
                    nroPlanes += parseInt(1);
                }
            }
        }
    }
    return nroPlanes;
}
// ******************************************************* REGLAS COMERCIALES 3PI******************************************************* //

function nroPaqEvaluadosHFCI() {
    var strCadenaDetalle = self.frames['ifraCondicionesVenta'].consultarItem('');
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var idProducto;
    var idFila;
    var nroPlanes = 0;
    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            idFila = arrCadenaDetalle[i].split(';')[0];
            idProducto = arrCadenaDetalle[i].split(';')[1];
            //LR
            if (idProducto == codTipoProd3PlayInalam) {
                //FLR
                var strGrupoPaquete = arrCadenaDetalle[i].split(';')[8];
                var idFilaPaquete = obtenerFilaPaquete(strGrupoPaquete);
                if (idFila == idFilaPaquete) {
                    nroPlanes += parseInt(1);
                }
            }
        }
    }
    return nroPlanes;
}

function nroPlanesEvaluados(idPlan, idProducto) {
    var nroPlanes = 0;
    var strPlanes = self.frames['ifraCondicionesVenta'].consultarItem('');
    var arrPlanes = strPlanes.split('|');
    for (var i = 0; i < arrPlanes.length; i++) {
        if (arrPlanes[i] != '') {
            if (idPlan == 'ALL' && idProducto == 'ALL')
                nroPlanes += parseInt(1);
            else {
                if (idProducto != '') {
                    var producto = arrPlanes[i].split(';')[1];
                    if (producto == idProducto)
                        nroPlanes += parseInt(1);
                }
                else {
                    var plan = arrPlanes[i].split(';')[10];
                    if (parseInt(plan, 10) == parseInt(idPlan, 10))
                        nroPlanes += parseInt(1);
                }
            }
        }
    }
    return nroPlanes;
}

function nroPlanesActivosxId(idPlan) {
    var nroPlanes = 0;
    var strPlanesActivos = getValue('hidnPlanesActivos');
    var arrPlanesActivos = strPlanesActivos.split('|');
    for (var i = 0; i < arrPlanesActivos.length; i++) {
        if (arrPlanesActivos[i] != '') {
            if (idPlan == 'ALL')
                nroPlanes += parseInt(1);
            else {
                var plan = arrPlanesActivos[i].split(';')[0];
                if (parseInt(plan, 10) == parseInt(idPlan, 10))
                    nroPlanes += parseInt(1);
            }
        }
    }
    return nroPlanes;
}

function nroPlanesActivosxCE(idProdComercial) {
    var nroPlanes = 0;
    var arrPlanesActivos = getValue('hidlistaCENroPlanxProd').split('|');

    for (var i = 0; i < arrPlanesActivos.length; i++) {
        if (arrPlanesActivos[i] != '') {

            var idPlanBscs = arrPlanesActivos[i].split(';')[0];
            var idProducto = arrPlanesActivos[i].split(';')[1];
            var nroPlan = arrPlanesActivos[i].split(';')[2];

            if (idProdComercial != '') {
                if (idProdComercial == idProducto) {
                    nroPlanes += parseInt(0) + parseInt(nroPlan);
                }
            } else {
                nroPlanes += parseInt(0) + parseInt(nroPlan);
            }
        }
    }
    return nroPlanes;
}

function nroPlanesActivosVozDatosxCE(flgVozDatos) //1:VOZ-2:DATOS
{
    var nroPlanes = 0;
    var strPlanesCE = getValue('hidlistaCEPlanBscs');
    var arrPlanesActivos = getValue('hidnPlanesActivoVozDatos').split('|');
    for (var i = 0; i < arrPlanesActivos.length; i++) {
        if (arrPlanesActivos[i] != '') {
            var plan = arrPlanesActivos[i].split(';')[0];
            var tipo = arrPlanesActivos[i].split(';')[1];
            var cant = arrPlanesActivos[i].split(';')[2];
            if (strPlanesCE.indexOf(plan) > 0 && tipo == flgVozDatos)
                nroPlanes += parseInt(cant, 10);
        }
    }
    return nroPlanes;
}

function validarNroPlanesMaxCE(flg) {

    var plazo;
    var plan;
    var planBscs;
    var nroPlanes = 0;
    var nroPlanesActivos = 0;
    var nroPlanesAgregados = 0;

    if (getValue('hidlistaCEPlan') == '')
        return true;
    else {
        var strCadenaDetalle = self.frames['ifraCondicionesVenta'].consultarItem('');
        var strPlanDetalle = self.frames['ifraCondicionesVenta'].planesEvaluados(strCadenaDetalle);
        var strPlanesAgregados = self.frames['ifraCondicionesVenta'].planesxCantidad(strPlanDetalle);

        var arrPlanesAgregados = strPlanesAgregados.split('|'); //[nroPlanes;plan;planBscs;plazo]
        var arrPlanCE = getValue('hidlistaCEPlan').split('|'); 	//[plazo;plan;nroPlanes]
        var plazoCE;
        var planCE;
        var nroPlanMaxCE;
    }

    // Nro Planes Máximo [Condición Comercial - Mantenimiento Caso Especial]
    for (var i = 0; i < arrPlanesAgregados.length; i++) {
        nroPlanesAgregados = parseInt(arrPlanesAgregados[i].split(';')[0], 10);
        plan = arrPlanesAgregados[i].split(';')[1];
        planBscs = arrPlanesAgregados[i].split(';')[2];
        plazo = arrPlanesAgregados[i].split(';')[3];

        nroPlanesActivos = 0;
        if (flg == '1')
            nroPlanesActivos = parseInt(nroPlanesActivosxId(planBscs), 10);

        nroPlanes = nroPlanesActivos + nroPlanesAgregados;

        for (var ii = 0; ii < arrPlanCE.length; ii++) {
            if (arrPlanCE[ii] != '') {
                plazoCE = arrPlanCE[ii].split(';')[0];
                planCE = arrPlanCE[ii].split(';')[1];
                nroPlanMaxCE = parseInt(arrPlanCE[ii].split(';')[2], 10);

                if (plazo == plazoCE && plan == planCE) {
                    if (nroPlanes > nroPlanMaxCE) {
                        return false
                    }
                }
            }
        }
    }

    return true;
}

function validarNroPlanesMaxGeneralCE(flgActivos) {

    var nroPlanesActivos = 0;
    var nroPlanesActivosMovil = 0;
    var nroPlanesActivosFijo = 0;
    var nroPlanesActivosClaroTv = 0;
    var nroPlanesActivosBAM = 0;
    var nroPlanesActivosHFC = 0;
    var nroPlanesActivosHFCI = 0;

    var nroPlanesMovil = 0;
    var nroPlanesFijo = 0;
    var nroPlanesClaroTv = 0;
    var nroPlanesBAM = 0;
    var nroPlanesHFC = 0;
    var nroPlanesHFCI = 0;
    var nroMaxPlanesMovil = 0;
    var nroMaxPlanesFijo = 0;
    var nroMaxPlanesClaroTv = 0;
    var nroMaxPlanesBAM = 0;
    var nroMaxPlanesHFC = 0;
    var nroMaxPlanesHFCI = 0;

    if (flgActivos == '1') {

        nroPlanesActivos = nroPlanesActivosxCE('');
        nroPlanesActivosMovil =  nroPlanesActivosxCE(codTipoProductoMovil);
        nroPlanesActivosFijo = nroPlanesActivosxCE(codTipoProductoFijo);
        nroPlanesActivosClaroTv = nroPlanesActivosxCE(codTipoProductoDTH);
        nroPlanesActivosBAM = nroPlanesActivosxCE(codTipoProductoBAM);
        nroPlanesActivosHFC = nroPlanesActivosxCE(codTipoProductoHFC);
        nroPlanesActivosHFCI = nroPlanesActivosxCE(codTipoProd3PlayInalam);
    }

    // Cantidad Planes Máximo Caso Especial
    var arrPlanxProducto = getValue('hidlistaCEPlanxProd').split('|');
    for (var i = 0; i < arrPlanxProducto.length; i++) {
        if (arrPlanxProducto[i] != '') {       
            
            var idProducto = arrPlanxProducto[i].split(';')[0];
            var nroPlanes = arrPlanxProducto[i].split(';')[1];

            if (idProducto == codTipoProductoMovil)
                nroMaxPlanesMovil = nroPlanes;

            if (idProducto == codTipoProductoFijo)
                nroMaxPlanesFijo = nroPlanes;

            if (idProducto == codTipoProductoDTH)
                nroMaxPlanesClaroTv = nroPlanes;

            if (idProducto == codTipoProductoBAM)
                nroMaxPlanesBAM = nroPlanes;

            if (idProducto == codTipoProductoHFC)
                nroMaxPlanesHFC = nroPlanes;

            if (idProducto == codTipoProd3PlayInalam)
                nroMaxPlanesHFCI = nroPlanes;
        }
    }

    var nroPlanes = parseInt(nroPlanesActivos, 10) + parseInt(nroPlanesEvaluados('ALL', 'ALL'), 10);
    var nroMaxPlanes = getValor(getValue('ddlCasoEspecial'), 2);
    var blnOKPlanes = (parseInt(nroPlanes, 10) <= parseInt(nroMaxPlanes, 10));

    if (!blnOKPlanes) return false;

    var nroPlanesMovil = parseInt(nroPlanesActivosMovil, 10) + parseInt(nroPlanesEvaluados('', codTipoProductoMovil), 10);
    var blnOKPlanesMovil = (parseInt(nroPlanesMovil, 10) <= parseInt(nroMaxPlanesMovil, 10));

    if (!blnOKPlanesMovil) return false;

    var nroPlanesFijo = parseInt(nroPlanesActivosFijo, 10) + parseInt(nroPlanesEvaluados('', codTipoProductoFijo), 10);
    var blnOKPlanesFijo = (parseInt(nroPlanesFijo, 10) <= parseInt(nroMaxPlanesFijo, 10));

    if (!blnOKPlanesFijo) return false;

    var nroPlanesClaroTv = parseInt(nroPlanesActivosClaroTv, 10) + parseInt(nroPlanesEvaluados('', codTipoProductoDTH), 10);
    var blnOKPlanesClaroTv = (parseInt(nroPlanesClaroTv, 10) <= parseInt(nroMaxPlanesClaroTv, 10));

    if (!blnOKPlanesClaroTv) return false;

    var nroPlanesBAM = parseInt(nroPlanesActivosBAM, 10) + parseInt(nroPlanesEvaluados('', codTipoProductoBAM), 10);
    var blnOKPlanesBAM = (parseInt(nroPlanesBAM, 10) <= parseInt(nroMaxPlanesBAM, 10));

    if (!blnOKPlanesBAM) return false;

    var nroPlanesHFC = parseInt(nroPlanesActivosHFC, 10) + parseInt(nroPaqEvaluadosHFC(), 10);
    var blnOKPlanesHFC = (parseInt(nroPlanesHFC, 10) <= parseInt(nroMaxPlanesHFC, 10));

    if (!blnOKPlanesHFC) return false;
    var nroPlanesHFCI = parseInt(nroPlanesActivosHFCI, 10) + parseInt(nroPaqEvaluadosHFCI(), 10);
    var blnOKPlanesHFCI = (parseInt(nroPlanesHFCI, 10) <= parseInt(nroMaxPlanesHFCI, 10));

    if (!blnOKPlanesHFCI) return false;
    return true;
}

function validarCargoFijoMaxCE() {
    var dblCF = 0;
    var strCadenaDetalle = self.frames['ifraCondicionesVenta'].consultarItem('');
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '')
            dblCF += parseFloat(arrCadenaDetalle[i].split(';')[23]);
    }
    var dblCFMax = getValue('hidCargoFijoMaxCE');
    if (getValue('hidWhitelistCE') != 'S')
        return true;
    else
        return (parseFloat(dblCF) <= parseFloat(dblCFMax));
}

function validarOfertaB2E() {
    var ddlOferta = document.getElementById('ddlOferta');
    var ddlCasoEspecial = document.getElementById('ddlCasoEspecial');
    if (ddlOferta.value == constTipoOfertaB2E) {
        if (ddlCasoEspecial.value == '') {
            ddlOferta.focus();
            alert('Este Tipo de Cliente solo se puede vender por Caso Especial.');
            return false;
        }
    }
    return true;
}

function validarOfertaCorporativa() {
    //ESALASB
    var tipoPlan;
    var codPlan;
    var CFPlan;
    var strPaquete;
    var strMensaje = '';
    var flgPlan = false;
    var idProducto = '';
    var strCadenaDetalle = getValue('hidCadenaDetalle');

    if (getValue('ddlTipoDocumento') != constTipoDocumentoRUC) {
        if (getValue('ddlOferta') == constTipoOfertaBusiness) {
            var arrPlanDatosVoz = getValue('hidPlanesDatosVoz').split('|');
            var nroPlanDatos = arrPlanDatosVoz[1].split(';')[1];
            var nroPlanVoz = arrPlanDatosVoz[0].split(';')[0];
            var CFPlanDatos = arrPlanDatosVoz[1].split(';')[1];
            var CFPlanVoz = arrPlanDatosVoz[0].split(';')[1];

            var strParametros = constCodigoParamOfertaCorp;
            var arrParametros = strParametros.split(',');

            var minPlanesDatos = getParametroGeneral(arrParametros[0]);
            var minCFdatos = getParametroGeneral(arrParametros[1]);
            var minPlanesVoz = getParametroGeneral(arrParametros[2]);
            var minCFvoz = getParametroGeneral(arrParametros[3]);

            var arrPlanDetalle = strCadenaDetalle.split('|');
            for (var i = 0; i < arrPlanDetalle.length; i++) {
                if (arrPlanDetalle[i] != '') {
                    idProducto = arrPlanDetalle[i].split(';')[1];
                    tipoPlan = arrPlanDetalle[i].split(';')[13];
                    CFPlan = parseFloat(arrPlanDetalle[i].split(';')[23]);
                    strPaquete = arrPlanDetalle[i].split(';')[6];

                    if (strPaquete == '' && idProducto != codTipoProductoFijo) {
                        flgPlan = true;
                        if (tipoPlan == '1') //Voz
                        {
                            nroPlanVoz = parseInt(nroPlanVoz, 10) + 1;
                            CFPlanVoz = parseFloat(CFPlanVoz) + parseFloat(CFPlan);
                        }
                        else if (tipoPlan == '2') //Datos
                        {
                            nroPlanDatos = parseInt(nroPlanDatos, 10) + 1;
                            CFPlanDatos = parseFloat(CFPlanDatos) + parseFloat(CFPlan);
                        }
                    }
                }
            }

            var nroPlanesMovil = nroPlanesEvaluados('', codTipoProductoMovil);
            var nroPlanesBAM = nroPlanesEvaluados('', codTipoProductoBAM);

            if (flgPlan) {
                if (parseInt(nroPlanesMovil, 10) > 0 && ((parseFloat(CFPlanVoz) < parseFloat(minCFvoz)) || (parseInt(nroPlanVoz) < parseInt(minPlanesVoz)))) {
                    strMensaje = 'Debe elegir minimo ' + minPlanesVoz + ' Planes Voz con CF total mayor a ' + minCFvoz;
                }
                if (parseInt(nroPlanesBAM, 10) > 0 && ((parseFloat(CFPlanDatos) < parseFloat(minCFdatos)) || (parseInt(nroPlanDatos) < parseInt(minPlanesDatos)))) {
                    if (strMensaje == '')
                        strMensaje = 'Debe elegir minimo ' + minPlanesDatos + ' Planes Datos con CF total mayor a ' + minCFdatos;
                    else
                        strMensaje += ' y ' + minPlanesDatos + ' Planes Datos con CF total mayor a ' + minCFdatos;
                }

                if (strMensaje != '') {
                    alert(strMensaje);
                    return false;
                }
            }
        }
    }

    // ESALASB - INICIO - VALIDAR CANT. LINEAS DESPACHO PDV CAMPAÑA PORTABILIDAD
    if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
        if (getValue('ddlOferta') == constTipoOfertaBusiness) {
            var flgCampPorta = false;
            var flgValidacionPlanesI35 = false;
            var blnPortabilidad = document.getElementById('chkPortabilidad').checked;
            var codCampPorta = getValue('hidCodCampValidacion');
            var codPlanPorta = getValue('hidCodPlanValidacion');

            /*INICIO VALIDACION PLAN I35*/
            var codPlanesValidacionI35 = getValue('hidCodPlanesValidacionI35');

            var descripcionPlanI35 = '';
            /*FIN VALIDACION PLAN I35*/

            var strTipOpeSol = getValue('ddlTipoOperacion');
            var strTipOpeVNA = constTipoOperAlta;
            var codCamp = '';
            var descripcionCamp = '';
            var descripcionPlan = '';
            var nroSolPlanPorta = 0;

            var arrPlanDetalle = strCadenaDetalle.split('|');
            for (var i = 0; i < arrPlanDetalle.length; i++) {
                if (arrPlanDetalle[i] != '') {
                    idProducto = arrPlanDetalle[i].split(';')[1];
                    codPlan = arrPlanDetalle[i].split(';')[10];
                    tipoPlan = arrPlanDetalle[i].split(';')[13];
                    strPaquete = arrPlanDetalle[i].split(';')[6];

                    if (idProducto == codTipoProductoMovil) {
                        if (tipoPlan == '1') {
                            codCamp = arrPlanDetalle[i].split(';')[15];

                            if (codCamp == codCampPorta && codPlan == codPlanPorta) {
                                flgCampPorta = true;
                            }

                            if (blnPortabilidad || strTipOpeSol == strTipOpeVNA) {
                                if (codCamp == codCampPorta && codPlan == codPlanPorta) {
                                    descripcionPlan = arrPlanDetalle[i].split(';')[11];
                                    descripcionCamp = arrPlanDetalle[i].split(';')[16];
                                    nroSolPlanPorta = nroSolPlanPorta + 1;
                                }

                            }

                            /*INICIO VALIDACION PLAN I35*/
                            if (codPlanesValidacionI35.indexOf(codPlan) > -1) {
                                if (descripcionPlanI35 == "") {
                                    descripcionPlanI35 = arrPlanDetalle[i].split(';')[11];
                                } else {
                                    descripcionPlanI35 = descripcionPlanI35 + ", " + arrPlanDetalle[i].split(';')[11];
                                }
                                flgValidacionPlanesI35 = true;
                            }
                            /*FIN VALIDACION PLAN I35*/
                        }
                    }
                }
            }


            if (flgCampPorta) {

                if (blnPortabilidad || strTipOpeSol == strTipOpeVNA) {

                    if (getValue('ddlCasoEspecial') == '') {

                        var nroMinPlanPorta = parseInt(getValue('hidNroMinPlanesPorta'));

                        if (nroSolPlanPorta < nroMinPlanPorta) {
                            if (strMensaje == '')
                                strMensaje = 'Debe contratar como minimo ' + nroMinPlanPorta + ' lineas de voz con Plan "' + descripcionPlan + '", para acceder a la campaña "' + descripcionCamp + '"';
                            else
                                strMensaje += ' y ' + 'Debe contratar como minimo ' + nroMinPlanPorta + ' lineas de voz con Plan "' + descripcionPlan + '", para acceder a la campaña "' + descripcionCamp + '"';
                        }
                    }
                }
            }

            if (flgValidacionPlanesI35) {
                if (blnPortabilidad || strTipOpeSol == strTipOpeVNA) {

                    if (getValue('ddlCasoEspecial') == '') {

                        var nroMinPlanesI35 = parseInt(getValue('hidNroMinPlanesI35'));

                        if ((arrPlanDetalle.length - 1) < nroMinPlanesI35) {
                            if (strMensaje == '')
                                strMensaje = 'Usted debe contratar como minimo ' + nroMinPlanesI35 + ' planes para adquirir alguno de los siguientes planes "' + descripcionPlanI35 + '"';
                            else
                                strMensaje += ' y ' + ' Usted debe contratar como minimo ' + nroMinPlanesI35 + ' planes para adquirir alguno de los siguientes planes "' + descripcionPlanI35 + '"';
                        }
                    }
                }
            }

            if (strMensaje != '') {
                alert(strMensaje);
                return false;
            }
        }
    }
    // FIN - VALIDAR CANT. LINEAS DESPACHO PDV CAMPAÑA PORTABILIDAD

    return true;
}

function validarTitularidadDTH() {
    var strCadenaDetalle = self.frames['ifraCondicionesVenta'].consultarItem('');
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var idProducto;
    var flgCambio = false;
    var flg = false;

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            idProducto = arrCadenaDetalle[i].split(';')[1];
            if (flgCambio)
                return false;

            if (idProducto == codTipoProductoDTH) {
                var strEquipo = arrCadenaDetalle[i].split(';')[17];
                if (strEquipo == constKitCambioTitularidad) {
                    if (flg)
                        return false;
                    else
                        flgCambio = true;
                }
            }
            else
                flg = true;
        }
    }
    return true;
}

function validarBolsaCompartidaII() {
    var strPlanBolsa = getValue('hidPlanCombo');
    var arrPlanBolsa = strPlanBolsa.split('|');
    var strCodPlan;
    var nroPlanes = 0;

    var strPlanes = self.frames['ifraCondicionesVenta'].consultarItem('');
    var arrPlanes = strPlanes.split('|');

    for (var i = 0; i < arrPlanBolsa.length; i++) {
        strCodPlan = arrPlanBolsa[i];
        if (strCodPlan != '') {
            for (var ii = 0; ii < arrPlanes.length; ii++) {
                if (arrPlanes[ii] != '') {
                    var plan = arrPlanes[ii].split(';')[10];
                    if (parseInt(plan, 10) == parseInt(strCodPlan, 10))
                        nroPlanes += parseInt(1);
                }
            }

            if (parseInt(nroPlanes, 10) > 1) return false;
        }
    }

    return true;
}

function validarSoloPlanesFijo() {
    var strCadenaDetalle = self.frames['ifraCondicionesVenta'].consultarItem('');
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var strCadena = '';
    var idFila = '';
    var idProducto = '';
    var arrDetalle;
    var plan;

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            idFila = arrCadenaDetalle[i].split(';')[0];
            idProducto = arrCadenaDetalle[i].split(';')[1];

            if (idProducto != codTipoProductoFijo)
                return false;

            arrDetalle = arrCadenaDetalle[i].split(';');
            plan = arrDetalle[9];

            var claseProducto = getValor(plan, 7).replace(',', '');
            if (claseProducto != constTipoBilleteraFijo)
                return false;
        }
    }

    return true;
}

function validarPlanesCombo() {
    if (getValue('hidPlazoReno').length > 0)
        return true;

    if (getValue('ddlCombo') != '') {
        var nroPlanCombo = self.frames['ifraCondicionesVenta'].getValue('hidPlanesCombo');
        var arrPlanCombo = nroPlanCombo.split('|');
        var nroPlanesEval = 0;

        for (var i = 0; i < arrPlanCombo.length; i++) {
            if (arrPlanCombo[i] != '') {
                var idProducto = arrPlanCombo[i].split('_')[0];
                var nroPlanes = arrPlanCombo[i].split('_')[1];
if ((idProducto == codTipoProductoHFC) || (idProducto == codTipoProd3PlayInalam))
                if (idProducto == codTipoProductoHFC){
                    nroPlanesEval = parseInt(0) + nroPaqEvaluadosHFC();
                } else {
                      nroPlanesEval = parseInt(0) + nroPaqEvaluadosHFCI();
                    }
                else
                    nroPlanesEval = parseInt(0) + nroPlanesEvaluados('', idProducto);
                //gaa20151124: Poner un minimo en vez de una igualdad
                //if (nroPlanesEval != parseInt(nroPlanes)) {
                if (nroPlanesEval < parseInt(nroPlanes)) {
                    alert('Debe agregar todos los planes/servicios configurados para el Grupo Producto.');
                    return false;
                }
                //fin gaa20151124
            }
        }
    }
    return true;
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function validarDatosCliente() {
    if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {
        if (!validarControl('txtRazonSocial', '', 'Ingresar la razón social del cliente.')) return false;
    } else {
        if ((getValue('ddlTipoDocumento') == constTipoDocumentoCE) || (getValue('ddlTipoDocumento') == constCodTipoDocumentoPAS)) { //INC000002748172
            if (!validarControl('txtNombre', '', 'Ingresar nombres del cliente.')) return false;
            if (!validarControl('txtApePat', '', 'Ingresar apellido paterno del cliente.')) return false;
        }
        else {
        if (!validarControl('txtNombre', '', 'Ingresar nombres del cliente.')) return false;
        if (!validarControl('txtApePat', '', 'Ingresar apellido paterno del cliente.')) return false;
        if (!validarControl('txtApeMat', '', 'Ingresar apellido materno del cliente.')) return false;
    }
    }
    return true;
}

function validarDatosRRLL() {
    if (getValue('ddlTipoDocumento') == constTipoDocumentoRUC) {

        var listaRepresentante = ifraRepresentante.obtenerRepresentanteSeleccionado();
        if (listaRepresentante == '') {
            alert('Debe seleccionar al menos un Representante Legal.');
            return false;
        }
        //INI PROY-31636
        if (listaRepresentante == '-1') {
            alert('Debe seleccionar la nacionalidad de todos los representantes legales.');
            return false;
        }
        //FIN PROY-31636
        setValue('hidListaRepresentante', listaRepresentante);
    }
    return true;
}

function validarPlanesNoCarrito() {
    if (getValue('ddlCombo') == '') {
        if (self.frames['ifraCondicionesVenta'].tieneProductosFueraCarrito() && getValue('ddlTipoOperacion') != constTipoOperMigracion && getValue('hidNTienePortabilidadValues') != 'S') {
            if (!confirm('Existen planes/servicios pendientes de agregar a carrito, ¿desea grabar SEC sin incluirlos?'))
                return false;
        }
    }
    return true;
}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function listaTelefonosEvaluados() {
    var strCadenaDetalle = self.frames['ifraCondicionesVenta'].consultarItem('');
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var telefono = '';

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            telefono += '|' + arrCadenaDetalle[i].split(';')[30];
        }
    }
    return telefono;
}

function listaTelefonosEvaluacionCP() {
    var strCadenaDetalle = self.frames['ifraCondicionesVenta'].obtenerLineasCP();
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var telefono = '';

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            telefono += '|' + arrCadenaDetalle[i];
        }
    }
    return telefono;
}

//PROY-2X1
function obtenerDetalleCarrito() {
    var strCadenaDetalle = self.frames['ifraCondicionesVenta'].obtenerDetalleCarrito();
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var telefono = '';

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            telefono += '|' + arrCadenaDetalle[i];
        }
    }
    return telefono;
} //PROY-2X1

//INI PROY-CAMPANA LG
function obtenerDetalleCarritoPorta() {
    var strCadenaDetalle = self.frames['ifraCondicionesVenta'].obtenerDetalleCarritoPorta();
    return strCadenaDetalle;
} //FIN PROY-CAMPANA LG

//PROY-SMSPORTA::INICIO
function obtenerDatosPortabilidad() {
    var strCadenaNroTelefono = self.frames['ifraCondicionesVenta'].ObtenerNroTelefono();
    return strCadenaNroTelefono;
}
//PROY-SMSPORTA::FIN 

//PROY-FULLCLARO.v2-INI
function ObtenerPlanesServicios() {
    var strCadenaDatosProducto = self.frames['ifraCondicionesVenta'].ObtenerDatosProducto();
    return strCadenaDatosProducto;
}
//PROY-FULLCLARO.v2-INI
/*CNH-2016-05-23*/
function GetCodigoCampaigns() {
    var strCadenaDetalle = self.frames['ifraCondicionesVenta'].consultarItem('');
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var varCampa = '';

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            varCampa = arrCadenaDetalle[i].split(';')[15];
            break;
        }
    }
    return varCampa;
}




function validarTelefonosRepetidos(listaTelefono) {
    var arrTelefono = listaTelefono.split('|');

    for (var i = 0; i < arrTelefono.length; i++) {
        if (arrTelefono[i] != '') {
            for (var ii = 0; ii < arrTelefono.length; ii++) {
                if (arrTelefono[i] == arrTelefono[ii]) {
                    return false;
                }
            }
        }
    }
    return true;
}

function validarNroProductosPorta(codTipoProductoActual) {
    var nroPlanesEval = 0;
    if (codTipoProductoActual != codTipoProductoMovil) {
        nroPlanesEval += parseInt(0) + nroPlanesEvaluados('', codTipoProductoMovil)
    }
    if (codTipoProductoActual != codTipoProductoFijo) {
        nroPlanesEval += parseInt(0) + nroPlanesEvaluados('', codTipoProductoFijo)
    }
    if (codTipoProductoActual != codTipoProductoDTH) {
        nroPlanesEval += parseInt(0) + nroPlanesEvaluados('', codTipoProductoDTH)
    }
    if (codTipoProductoActual != codTipoProductoBAM) {
        nroPlanesEval += parseInt(0) + nroPlanesEvaluados('', codTipoProductoBAM)
    }
    if (codTipoProductoActual != codTipoProductoHFC) {
        nroPlanesEval += parseInt(0) + nroPaqEvaluadosHFC();
    }
    if (codTipoProductoActual != codTipoProd3PlayInalam) {
            nroPlanesEval += parseInt(0) + nroPaqEvaluadosHFCI();
        }
    if (nroPlanesEval > 0) {
        return false;
    }
    return true;
}

function validarTipoPortabilidad(codTipoProductoActual) {
    var tipoPortabilidad = 'M';

     if (codTipoProductoActual == codTipoProductoFijo || codTipoProductoActual == codTipoProductoHFC || codTipoProductoActual == codTipoProd3PlayInalam)
        tipoPortabilidad = 'F';

    return tipoPortabilidad;
}

function validarPortabilidad() {
    var ddlOperadorCedente = document.getElementById('ddlOperadorCedente');
    var ddlModalidadPorta = document.getElementById('ddlModalidadPorta');
    var txtNumContacto = ifraPortabilidad.document.getElementById('txtNumContacto');
    var txtNroFolio = ifraPortabilidad.document.getElementById('txtNroFolio');

    if (ddlModalidadPorta.value == '') {
        ddlModalidadPorta.focus();
        alert('Seleccione la Modalidad.');
        return false;
    }
    if (ddlOperadorCedente.value == '') {
        ddlOperadorCedente.focus();
        alert('Seleccione el Operador Cedente.');
        return false;
    }
    if (txtNumContacto.value.length < 7) {
        txtNumContacto.focus();
        alert('Ingrese un número de contacto válido.');
        return false;
    }

    //Lista archivos de Portabilidad
    setValue('hidArchivos', ifraPortabilidad.getValue('hidListaArchivos'));

    //PS Venta Cuotas: Televentas NO deberá ser obligatorio adjuntar cualquier tipo de documento ya sea la portabilidad PostPago o Prepago
    

    //Grabar
    setValue('hidOperadorCedente', ddlOperadorCedente.value);
    setValue('hidModalidad', ddlModalidadPorta.value);
    setValue('hidNumeroFolio', txtNroFolio.value);
    setValue('hidNumeroContacto', txtNumContacto.value);

    return true;
}


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function getMaxLengthDocumento(tipoDoc) {
    var nroCaracter;
    if (tipoDoc == constTipoDocumentoDNI) nroCaracter = 8; //PROY-31636
    else if (tipoDoc == constTipoDocumentoCE) nroCaracter = 9; //PROY-31636
    else if (tipoDoc == constTipoDocumentoRUC) nroCaracter = 11; //PROY-31636
    else nroCaracter = 12; //PROY-31636
    return nroCaracter;
}

function buscarPerfil(id) {
    var lista = getValue('hidListaPerfiles').split('|');
    for (var i = 0; i < lista.length; i++) {
        if (lista[i] == id) {
            return 'S';
        }
    }
    return 'N';
}

function getParametroGeneral(id) {
    var valor;
    var lista = getValue('hidListaParametro').split('|');
    for (var i = 0; i < lista.length; i++) {
        var col = lista[i].split(';');
        if (col[0] == id) {
            valor = col[1];
            break;
        }
    }
    return valor;
}

function getParametroGeneralII(id) {
    var valor;
    var lista = getValue('hidListaParametroII').split('|');
    for (var i = 0; i < lista.length; i++) {
        var col = lista[i].split(';');
        if (col[0] == id) {
            valor = col[1];
            break;
        }
    }
    return valor;
}

function obtenerTextoSeleccionado(ddl) {
    var resultado = '';

    for (var i = 0; i < ddl.options.length; i++) {
        if (ddl.options[i].selected)
            resultado = ddl.options[i].text;
    }

    return resultado;
}

function getValor(strValor, idValor)//0:Codigo, 1:CF
{
    if (strValor.indexOf('_') > -1) {
        var arrCodigo = strValor.split('_');
        return arrCodigo[idValor];
    }
    else
        return '';
}

function llenarDatosCombo(ddl, strDatos, booSeleccione) {
    var arrDatos;
    var arrItem;
    var strDato;
    var option;
    ddl.innerHTML = "";

    if (booSeleccione) {
        option = document.createElement('option');
        option.value = '';
        option.text = 'SELECCIONE...';
        ddl.add(option);
    }
    if (strDatos != null)
        var arrDatos = strDatos.split("|");
    else
        return;

    for (i = 0; i < arrDatos.length; i++) {
        option = document.createElement('option');

        if (arrDatos[i].length > 0) {
            strDato = arrDatos[i];
            arrItem = strDato.split(";");

            if (strDato != 'null') {
                if (arrItem.length > 1) {
                    option.value = arrItem[0];
                    option.text = arrItem[1];
                }
                else {
                    option.value = arrDatos[i];
                    option.text = arrDatos[i];
                }

                ddl.add(option);
            }
        }
    }
}

function obtenerFilaPaquete(strGrupoPaquete) {
    var idFila = '';
    strGrupoPaquete = strGrupoPaquete.replace('{', '').replace('}', '');
    var arrGrupoPaquete = strGrupoPaquete.split(',');
    for (i = 0; i < arrGrupoPaquete.length; i++) {
        if (arrGrupoPaquete[i] != '') {
            idFila = arrGrupoPaquete[i].replace('[', '').replace(']', '');
            break;
        }
    }
    return idFila;
}

// Consulta BRMS
function cadenaGeneral() {
    var strCadena = "";
    var strComboTexto = "";
    var strCasoEspecial = "";
    var strModalidad = "";
    var strOperadorCedente = "";
    //PROY-140618-INI
    var ifr = self.frames['ifraCondicionesVenta'];
    var TipoProductoActual = ifr.getValue('hidTipoProductoActual');    
    var desNacionalidad = getText('ddlNacionalidad');
    var strEmail = getValue('hdnEmail');
    //PROY-140618-FIN
    var tipoOperacion = getText('ddlTipoOperacion');
    if (getValue('hidNTienePortabilidadValues') == 'S') {
        tipoOperacion = 'PORTABILIDAD';

        if (getValue('ddlModalidadPorta') != "")
            strModalidad = getText('ddlModalidadPorta');

        if (getValue('ddlOperadorCedente') != "")
            strOperadorCedente = getText('ddlOperadorCedente');   
    }

    if (getValue('ddlCasoEspecial') != "")
        strCasoEspecial = getText('ddlCasoEspecial');

    strCadena = tipoOperacion;
    strCadena += "|" + getText('ddlOferta');
    strCadena += "|" + strCasoEspecial;
    strCadena += "|" + strModalidad;
    strCadena += "|" + strOperadorCedente;
//gaa20161024
    //strCadena += "|" + getText('ddlModalidadVenta');
    if (document.getElementById('ddlModalidadVenta') != null) {
        if (document.getElementById('ddlModalidadVenta').selectedIndex > -1)
    strCadena += "|" + getText('ddlModalidadVenta');
    }
    else
        strCadena += "|";
//fin gaa20161024
    strCadena += "|" + getValue('ddlCombo');

    if (getValue('ddlCombo') != '')
        strComboTexto = obtenerTextoSeleccionado(document.getElementById('ddlCombo'))

    strCadena += "|" + strComboTexto;

    //INC000002547199
    var strCodModalidadPorta = getValue('ddlModalidadPorta');
    var strCodOperadorCedente = getValue('ddlOperadorCedente');

    strCadena += "|" + strCodModalidadPorta + "_" + getText('ddlModalidadPorta'); //INC000002628010 - (INC000002547199) - fdq1
    strCadena += "|" + strCodOperadorCedente + ";" + getText('ddlOperadorCedente'); //INC000002628010 + 3
    strCadena += "|" + getValue('ddlOferta');
    strCadena += "|" + getValue('ddlCasoEspecial');
    strCadena += "|" + getValue('ddlTipoOperacion');
    strCadena += "|" + TipoProductoActual;
    strCadena += "|" + desNacionalidad;
    strCadena += "|" + strEmail;

    return strCadena;
}

function cadenaPlanesDetalle(strCadenaDetalle) {
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var strCadena = '';
    var idFila = '';
    var idProducto = '';
    var dblCF = 0;
    var arrDetalle;
    var strGrupoPaquete;
    var idFilaPaquete;
    var topeConsumo = '';
//gaa20170215
    var cfAcumulado = 0;
    var cantidadPlanes = 0;
    var montoActual;
    var idProductoAnt = '';
    //PROY-140618-INI
    var ifr = self.frames['ifraCondicionesVenta'];
    var FamiliaPlan = ifr.getValue('hidFamiliaPlan');
    //PROY-140618-FIN

//fin gaa20170215
    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            idFila = arrCadenaDetalle[i].split(';')[0];
            idProducto = arrCadenaDetalle[i].split(';')[1];
            arrDetalle = arrCadenaDetalle[i].split(';');
            dblCF = 0;
            strGrupoPaquete = arrCadenaDetalle[i].split(';')[8];
//gaa20170215
            if (idProductoAnt != idProducto) {
                cfAcumulado = 0;
                cantidadPlanes = 0;
                idProductoAnt = idProducto;
            }
//fin gaa20170215
              if ((idProducto == codTipoProductoHFC) || (idProducto == codTipoProd3PlayInalam)) {
                idFilaPaquete = obtenerFilaPaquete(strGrupoPaquete);

                if (idFila == idFilaPaquete) {
                    for (var ii = 0; ii < arrCadenaDetalle.length; ii++) {
                        if (arrCadenaDetalle[ii] != '') {
                            if (strGrupoPaquete == arrCadenaDetalle[ii].split(';')[8]) {
                                montoActual = parseFloat(arrCadenaDetalle[ii].split(';')[23]);
                                dblCF += montoActual;
//gaa20170215
                                cfAcumulado += montoActual;
//fin gaa20170215
                            }
                            if (arrCadenaDetalle[ii].split(';')[35] != '')
                                topeConsumo = arrCadenaDetalle[ii].split(';')[35];
                        }
                    }

                    strCadena += idFila + ";"; 		    //[0] - idFila
                    strCadena += idProducto + ";"; 	    //[1] - idProducto
                    strCadena += arrDetalle[2] + ";";   //[2] - idPlazo
                    strCadena += arrDetalle[6] + ";";   //[3] - idPaquete
                    strCadena += arrDetalle[7] + ";";   //[4] - Paquete Texto
                    strCadena += arrDetalle[10] + ";"; 	//[5] - idPlan
                    strCadena += arrDetalle[11] + ";"; 	//[6] - Plan Texto
                    strCadena += topeConsumo + ";";     //[7] - Tope Consumo
                    strCadena += arrDetalle[15] + ";"; 	//[8] - idCampana
                    strCadena += arrDetalle[16] + ";"; 	//[9] - Campana Texto
//gaa20170215
                    //strCadena += dblCF + "|"; 		    //[10]- CF
                    strCadena += dblCF + ";"; 		    //[10]- CF
                    strCadena += cfAcumulado + ';';
                    cantidadPlanes += 1;
                    strCadena += cantidadPlanes + ';';
                    strCadena += "|";        		    //[13]- Fecha Activacion CP
//fin gaa20170215
                }
            }
            else {
                strCadena += idFila + ";"; 		    //[0] - idFila
                strCadena += idProducto + ";"; 	    //[1] - idProducto
                strCadena += arrDetalle[2] + ";";   //[2] - idPlazo
                strCadena += arrDetalle[6] + ";";   //[3] - idPaquete
                strCadena += arrDetalle[7] + ";";   //[4] - Paquete Texto
                strCadena += arrDetalle[10] + ";";  //[5] - idPlan
                strCadena += arrDetalle[11] + ";";  //[6] - Plan Texto
                strCadena += arrDetalle[14] + ";";  //[7] - Tope Consumo
                strCadena += arrDetalle[15] + ";";  //[8] - idCampana
                strCadena += arrDetalle[16] + ";";  //[9] - Campana Texto
//gaa20170215
                dblCF = parseFloat(arrDetalle[23]);
                cfAcumulado += dblCF;
                //strCadena += arrDetalle[23] + ";";  //[10]- CF
                strCadena += dblCF + ";"; 		    //[10]- CF
                strCadena += cfAcumulado + ';';
                cantidadPlanes += 1;
                strCadena += cantidadPlanes + ';';
                strCadena += arrDetalle[38] + ";";  //[13]- Fecha Activacion CP
                strCadena += FamiliaPlan + ';';    // [14] - Familia Plan  //PROY-140618
                strCadena += arrDetalle[3] + ';';  // [15] - Descripcion Plazo  //PROY-140618 //PROY-140335 RF1
                strCadena += arrDetalle[30] + "|"; // [16] -  NroLinea  PROY-140335 RF1
//fin gaa20170215
            }
        }
    }
    strCadena = strCadena.replace(/\+/g, '*');
    return strCadena;
}

function cadenaEquiposDetalle(strCadenaDetalle) {
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var strCadena = '';
    var idFila = '';
    var idProducto = '';
    var arrDetalle;
    var strGrupoPaquete;
    var idFilaPaquete;

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            idFila = arrCadenaDetalle[i].split(';')[0];
            idProducto = arrCadenaDetalle[i].split(';')[1];
            arrDetalle = arrCadenaDetalle[i].split(';');
            strGrupoPaquete = arrCadenaDetalle[i].split(';')[8];

            if ((idProducto != codTipoProductoHFC) || (idProducto != codTipoProd3PlayInalam)) {
                strCadena += idFila + ";"; 		    //[0] - idFila
                strCadena += idProducto + ";"; 	    //[1] - idProducto
                strCadena += arrDetalle[17] + ";";  //[2] - idEquipo
                strCadena += arrDetalle[18] + ";";  //[3] - Equipo
                strCadena += arrDetalle[25] + ";";  //[4] - Costo
                strCadena += arrDetalle[26] + ";";  //[5] - Precio Venta
                strCadena += arrDetalle[28] + ";";  //[6] - nro Cuota
                strCadena += arrDetalle[29] + ";";  //[7] - porcentaje Cuota
                strCadena += arrDetalle[31] + ";";  //[8] - Monto Cuota
                strCadena += arrDetalle[24] + ";";  //[9] - idListaPrecio //PROY-30166
                strCadena += arrDetalle[30] + "|";  //[10] - NroLinea    //PROY-140618
                
            }
        }
    }
    strCadena = strCadena.replace(/\+/g, '*');
    return strCadena;
}

function cadenaServiciosDetalle(strCadenaDetalle) {
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var strCadena = '';
    var idFila = '';
    var idProducto = '';
    var arrDetalle;
    var strGrupoPaquete;
    var idFilaPaquete;
    var strCadenaSrv = '';

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            idFila = arrCadenaDetalle[i].split(';')[0];
            idProducto = arrCadenaDetalle[i].split(';')[1];
            arrDetalle = arrCadenaDetalle[i].split(';');
            strGrupoPaquete = arrCadenaDetalle[i].split(';')[8];

            if (idProducto == codTipoProductoHFC || idProducto == codTipoProd3PlayInalam || idProducto == codTipoProductoFTTH) { 
                idFilaPaquete = obtenerFilaPaquete(strGrupoPaquete);

                if (idFila == idFilaPaquete) {
                    for (var ii = 0; ii < arrCadenaDetalle.length; ii++) {
                        if (arrCadenaDetalle[ii] != '') {
                            if (strGrupoPaquete == arrCadenaDetalle[ii].split(';')[8]) {
                                strCadenaSrv += arrCadenaDetalle[ii].split(';')[33] + ";";
                            }
                        }
                    }

                    strCadena += idFila + ";"; 		//[0] - idFila
                    strCadena += strCadenaSrv + "|"; //[1] - servicios
                }
            }
        }
    }

    strCadena = strCadena.replace(/\+/g, '*');
    setValue('hidCadServicio', strCadena);//PROY-140618

    return strCadena;
}

function cadenaSECRecurrente() {
    var strCadenaDetalle = getValue('hidCadenaDetalle');
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var strCadena = '';
    var idFila = '';
    var idProducto = '';
    var arrDetalle;

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            idFila = arrCadenaDetalle[i].split(';')[0];
            idProducto = arrCadenaDetalle[i].split(';')[1];
            arrDetalle = arrCadenaDetalle[i].split(';');

            strCadena += idFila + ";"; 		//[0] - idFila
            strCadena += idProducto + ";"; 	//[1] - idProducto
            strCadena += arrDetalle[2] + ";"; //[2] - strPlazo
            strCadena += arrDetalle[4] + ";"; //[3] - strSolucion

            var paquete = arrDetalle[6];
             if ((idProducto == codTipoProductoHFC) || (idProducto == codTipoProd3PlayInalam))
                strCadena += paquete.split('_')[0] + ";"; //[4] - strPaquete
            else
                strCadena += paquete + ";"; 				//[4] - strPaquete

            var plan = arrDetalle[10];
             if ((idProducto == codTipoProductoHFC) || (idProducto == codTipoProd3PlayInalam))
                strCadena += plan.split('.')[2] + ";"; //[5] - strPlanCodigo
            else
                strCadena += plan + ";"; 			//[5] - strPlanCodigo

            strCadena += arrDetalle[15] + ";"; 	//[6] - strCampana
            strCadena += arrDetalle[25] + ";"; 	//[7] - strPrecioLista
            strCadena += arrDetalle[26] + ";"; 	//[8]- strPrecioVenta

              if (idProducto == codTipoProductoDTH || idProducto == codTipoProductoHFC || idProducto == codTipoProd3PlayInalam) {
                strCadena += '00' + ";"; 		//[9]- strCuotas
                strCadena += '100' + ";"; 		//[10]- strPorcentajeCuotas
            } else {
                strCadena += arrDetalle[28] + ";"; //[9]- strCuotas
                strCadena += arrDetalle[29] + ";"; //[10]- strPorcentajeCuotas
            }
            strCadena += arrDetalle[14] + "|"; 	//[11] - strTopeConsumo
        }
    }

    return strCadena;
}

//INICIO PROY-140546
function ValidarCamposCAI() {
    var strRangoHoras = getValue('hidTiempoSecPendientePagoLink');
    var ifr = self.frames['ifraCondicionesVenta'];
    var CodTipoProductoActual = ifr.getValue('hidCodigoTipoProductoActual');
    var bEsCanalPermitido = EsValorPermitido(getValue('ddlCanal'), Key_CanalesPermitidosCAI, ",");
    var nCobroAnticipado = parseInt(getValue('hidCobroAnticipadoInstalacion'));

    //(nCobroAnticipado > 0) && SE QUITÓ A SOLICITUD DE ABEL

    if (bEsCanalPermitido &&
        (CodTipoProductoActual == codTipoProductoHFC ||
        CodTipoProductoActual == codTipoProd3PlayInalam ||
        CodTipoProductoActual == codTipoProductoFTTH)) {

        if (getValue('txtFechaAgendamiento1') == "") {
            document.getElementById('txtFechaAgendamiento1').focus();
            alert(getValue('hidMsjValidacionSubformularioCAI'));
            return false;
        }

        if (getValue('txtFechaAgendamiento2') == "") {
            document.getElementById('txtFechaAgendamiento2').focus();
            alert(getValue('hidMsjValidacionSubformularioCAI'));
            return false;
        }

        if (getValue('txtFechaAgendamiento3') == "") {
            document.getElementById('txtFechaAgendamiento3').focus();
            alert(getValue('hidMsjValidacionSubformularioCAI'));
            return false;
        }

        if (getValue('ddlFranja1') == "") {
            document.getElementById('ddlFranja1').focus();
            alert(getValue('hidMsjValidacionSubformularioCAI'));
            return false;
        }

        if (getValue('ddlFranja2') == "") {
            document.getElementById('ddlFranja2').focus();
            alert(getValue('hidMsjValidacionSubformularioCAI'));
            return false;
        }

        if (getValue('ddlFranja3') == "") {
            document.getElementById('ddlFranja3').focus();
            alert(getValue('hidMsjValidacionSubformularioCAI'));
            return false;
        }

        //PROY-140546 LM
        if (document.getElementById('td7').style.display != 'none' && document.getElementById('td8').style.display != 'none'){
            if (getValue('ddlMedioPago') == "") {
                document.getElementById('ddlMedioPago').focus();
                alert(getValue('hidMsjValidacionSubformularioCAI'));
                return false;
            }
        }        

        if (getValue('txtCasillaCorreoiClaro') == "") {
            document.getElementById('txtCasillaCorreoiClaro').focus();
            alert(getValue('hidMsjValidacionSubformularioCAI'));
            return false;
        }
    }

    return true;
}
//FIN PROY-140546