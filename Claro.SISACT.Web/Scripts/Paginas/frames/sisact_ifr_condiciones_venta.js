function agregarFila(booVeriConf, idFilaX, booPlanAdicionalCombo) {

    var totalFilas = document.getElementById('tblTablaMovil').rows.length;
    var hidValidarGuardarCuota = document.getElementById('hidValidarGuardarCuota');
    if (totalFilas == 0)
        hidValidarGuardarCuota.value = '';

    if (hidValidarGuardarCuota.value.length > 0) {
        alert('Debe guardar las cuotas antes de ejecutar esta acción');
        return;
    }

    var ddlOferta = parent.document.getElementById('ddlOferta');
    if (ddlOferta.value.length == 0) {
        ddlOferta.focus();
        alert('Seleccione la oferta.');
        return;
    }

    var ddlCasoEspecial = parent.document.getElementById('ddlCasoEspecial');
    if (ddlOferta.value == constCodTipoProductoB2E) {
        if (ddlCasoEspecial.value.length == 0) {
            ddlCasoEspecial.focus();
            alert('Este Tipo de Cliente solo se puede vender por Caso Especial.');
            return;
        }
    }

    if (booVeriConf) {
        if (!verificarPlanes())
            return false;
    }

    var strPlazoActual = document.getElementById('hidPlazoActual').value;
    var hidTraerPlazo = document.getElementById('hidTraerPlazo');
    var hidPaqueteActual = document.getElementById('hidPaqueteActual');
    var hidTotalLineas = document.getElementById('hidTotalLineas');
    hidTotalLineas.value = parseInt(hidTotalLineas.value) + 1;
    var idFila = hidTotalLineas.value;
    var desTipoProductoActual = getValue('hidTipoProductoActual');
    var newRow = document.all('tblTabla' + desTipoProductoActual).insertRow();

    newRow.style.verticalAlign = 'top'; //jvv
    newRow.style.backgroundColor = '#FFFFFF'; //jvv

    if (idFilaX > 0)
        idFila = idFilaX + 1;

    oCell = newRow.insertCell();
    oCell.style.width = '20px';
    oCell.align = 'center';
    oCell.id = idFila;
    oCell.innerHTML = "<img name='imgEditarFila" + idFila + "' id='imgEditarFila" + idFila + "' src='../../Imagenes/editar.gif' border='0' style='cursor:hand' alt='Editar Fila' onclick='editarFila(" + idFila + ", false);' />";

    oCell = newRow.insertCell();
    oCell.style.width = '20px';
    oCell.align = 'center';

    if (!verificarCombo() || booPlanAdicionalCombo)
        oCell.innerHTML = "<img src='../../Imagenes/rechazar.gif' border='0' style='cursor:hand' alt='Eliminar Fila' onclick='eliminarFilaTotal(this, " + idFila + ", true);' />";

    estructuraGrilla(newRow, idFila, desTipoProductoActual, false);

    if (hidTraerPlazo.value == 'S') {
        hidTraerPlazo.value = 'N';
    }

    if (booVeriConf)
        hidPaqueteActual.value = '';

    var strFlujo = flujoAlta;
    if (parent.getValue('hidNTienePortabilidadValues') == 'S')
        strFlujo = flujoPortabilidad;

    if (strPlazoActual.length > 0) {
        if (!booVeriConf)
            asignarPaquete(idFila, getValue('hidPaqActCompleto'));
    }

    cerrarServicio();
    cerrarCuota();
    cerrarLineasCuentas();//PROY-140743

    if (verificarCombo() && booVeriConf)
        llenarCampanasPlanesCombo(idFila);
}

function estructuraGrilla(newRow, idFila, desTipoProductoActual, flgSecPendiente) {

    var disabled = "";
    var readonly = "";
    var flujoPorta = parent.getValue('hidNTienePortabilidadValues');
    var tienePaquete = document.getElementById('hidTienePaquete').value;

    var codOperacion = parent.getValue('ddlTipoOperacion'); //PROY-140743

    if (flgSecPendiente) {
        disabled = " disabled";
        readonly = " readonly";
    }

    var oCell = newRow.insertCell();
    oCell.style.width = '192px';
    oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlCampana" + idFila + "' id='ddlCampana" + idFila + "' onchange='cambiarCampana(" + idFila + ", this.value);'><option value=''>SELECCIONE...</option></select>";
//PROY-140743-INI
    if (codOperacion == '25') {
        oCell = newRow.insertCell();
        oCell.style.width = '190px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPromocion" + idFila + "' id='ddlPromocion" + idFila + "' onchange='cambiarPromocion(" + idFila + ", this.value);'><option value=''>SELECCIONE...</option></select>";
    }
//PROY-140743-FIN    
    //FTTH - desTipoProductoActual != 'FTTH') - GRILLA
    if (desTipoProductoActual != 'DTH' && desTipoProductoActual != 'HFC' && desTipoProductoActual != '3PlayInalam' && desTipoProductoActual != 'FTTH') {

        if (desTipoProductoActual != 'VentaVarios') {
            oCell = newRow.insertCell();
            oCell.style.width = '147px';
            oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlazo" + idFila + "' id='ddlPlazo" + idFila + "' onchange='cambiarPlazo(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";
        }

        if (tienePaquete == 'S' && desTipoProductoActual == 'Movil') {
            oCell = newRow.insertCell();
            oCell.style.width = '202px';
            oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPaquete" + idFila + "' id='ddlPaquete" + idFila + "' onchange='cambiarPaquete(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";
        }
        //gaa20161020: Si es masivo y movil
        if (tienePaquete != 'S' && desTipoProductoActual == 'Movil' && familiaFlag == '1') {
            oCell = newRow.insertCell();
            oCell.style.width = '152px';
            oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlFamiliaPlan" + idFila + "' id='ddlFamiliaPlan" + idFila + "'onchange='cambiarFamiliaPlan(this.value, " + idFila + ");'></select>";
        }
        //fin gaa20161020
        if (desTipoProductoActual != 'VentaVarios') {

            oCell = newRow.insertCell();
            oCell.style.width = '152px';
            oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlan" + idFila + "' id='ddlPlan" + idFila + "' onchange='cambiarPlan(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";
//PROY-140743-INI
            if (codOperacion != '25') {

            oCell = newRow.insertCell();
            oCell.style.width = '32px';
            oCell.align = 'center';

            if (flgSecPendiente == "S")
                oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicioGuardado(" + idFila + ");' />";
            else
                oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicio(" + idFila + ");' />";
            }
//PROY-140743-FIN            

            oCell = newRow.insertCell();
            oCell.style.width = '47px';
            oCell.align = 'center';
            oCell.innerHTML = "<input style='width:90%; text-align:right' readonly class='clsInputDisable' name='txtCFPlanServicio" + idFila + "' id='txtCFPlanServicio" + idFila + "' /><input type='hidden' id='hidMontoServicios" + idFila + "' name='hidMontoServicios" + idFila + "' /><input type='hidden' id='hidMontoCuota" + idFila + "' name='hidMontoCuota" + idFila + "' />";

            if (desTipoProductoActual == 'Movil' || desTipoProductoActual == 'Fijo') {

                oCell = newRow.insertCell();
                oCell.align = 'center';               
                //PROY-140743 - INICIO
                if (codOperacion == '25') {
                    oCell.style.width = '0px';
                    oCell.innerHTML = "<div style='display: none'><input style='width:90%; text-align:right' readonly class='clsInputDisable' name='txtMontoTopeConsumo" + idFila + "' id='txtMontoTopeConsumo" + idFila + "' /></div>";
                } else {
                oCell.style.width = '52px';
                oCell.innerHTML = "<input style='width:90%; text-align:right' readonly class='clsInputDisable' name='txtMontoTopeConsumo" + idFila + "' id='txtMontoTopeConsumo" + idFila + "' />";
            }
                //PROY-140743 - FIN
            }
        }

        oCell = newRow.insertCell();
        //jvv
        if (emuladorIE == '')
            oCell.style.width = '192px';
        else
            oCell.style.width = '197px';
        oCell.style.whiteSpace = 'nowrap';

        if (emuladorIE == '')
            oCell.innerHTML = "<div class='AutoComplete_Div'><input type='hidden' id='hidMaterial" + idFila + "' name='hidMaterial" + idFila + "' /><input type='hidden' id='hidValorEquipo" + idFila + "' name='hidValorEquipo" + idFila + "' /><input" + disabled + " type='text' id='txtTextoEquipo" + idFila + "' name='txtTextoEquipo" + idFila + "' class='clsSelectEnable0' style='width: 165px' onclick=mostrarListaSel(" + idFila + ") onkeyup=buscarCoincidente(" + idFila + ") onblur=ocultarListaTxt(" + idFila + ") /><img id='imgListaEquipo" + idFila + "' src='../../Imagenes/cmb.gif' style='height: 17px; cursor: pointer' align='absMiddle' title='Mostrar Lista' alt='Mostrar Lista' onclick=mostrarListaSel(" + idFila + ") onmouseover='imgSel(this)' onmouseout='imgNoSel(this)'; onblur=ocultarListaTxt(" + idFila + ") /></div><div id='divListaEquipo" + idFila + "' class='AutoComplete_List' style='width: 255px; display: none; z-index:10'; onblur=ocultarListaTxt(" + idFila + ")></div><input type='hidden' id='hidKit" + idFila + "' name='hidKit" + idFila + "' />";
        else
            oCell.innerHTML = "<div class='AutoComplete_Div'><input type='hidden' id='hidMaterial" + idFila + "' name='hidMaterial" + idFila + "' /><input type='hidden' id='hidValorEquipo" + idFila + "' name='hidValorEquipo" + idFila + "' /><input" + disabled + " type='text' id='txtTextoEquipo" + idFila + "' name='txtTextoEquipo" + idFila + "' class='clsSelectEnable0' style='width: 230px' onclick=mostrarListaSel(" + idFila + ") onkeyup=buscarCoincidente(" + idFila + ") onblur=ocultarListaTxt(" + idFila + ") /><img id='imgListaEquipo" + idFila + "' src='../../Imagenes/cmb.gif' style='height: 17px; cursor: pointer' align='absMiddle' title='Mostrar Lista' alt='Mostrar Lista' onclick=mostrarListaSel(" + idFila + ") onmouseover='imgSel(this)' onmouseout='imgNoSel(this)'; onblur=ocultarListaTxt(" + idFila + ") /></div><div id='divListaEquipo" + idFila + "' class='AutoComplete_List2' style='width: 255px; display: none; z-index:10'; onblur=ocultarListaTxt(" + idFila + ")></div><input type='hidden' id='hidKit" + idFila + "' name='hidKit" + idFila + "' />"; //jvv

        if (flgSecPendiente) {
            var imgListaEquipo = document.getElementById('imgListaEquipo' + idFila);
            imgListaEquipo.style.display = 'none';
        }

        if (parent.getValue('ddlModalidadVenta') == codModalidadChipSuelto) {
            oCell.style.display = 'none';
        }
        //INICIATIVA 920
        if ((parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota || parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode) && desTipoProductoActual != 'VentaVarios') {

            oCell = newRow.insertCell();
            oCell.style.width = '37px';
            oCell.align = 'center';

            if (flgSecPendiente == "S")
                oCell.innerHTML = "<img name='imgVerCuota" + idFila + "' id='imgVerCuota" + idFila + "' src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Cuotas' onclick='mostrarCuotaGuardada(" + idFila + ");' />";
            else
                oCell.innerHTML = "<img name='imgVerCuota" + idFila + "' id='imgVerCuota" + idFila + "' src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Cuotas' onclick='mostrarCuota(" + idFila + ");' />";
        }

        if (desTipoProductoActual == 'VentaVarios') {

            oCell = newRow.insertCell();
            oCell.style.width = '152px';
            oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlListaPrecio" + idFila + "' id='ddlListaPrecio" + idFila + "' onchange='cambiarListaPrecio(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";
        }

        oCell = newRow.insertCell();
        oCell.style.width = '62px';
        oCell.align = 'center';
        oCell.innerHTML = "<input style='width:90%; text-align:right' readonly class='clsInputDisable' name='txtEquipoPrecio" + idFila + "' id='txtEquipoPrecio" + idFila + "' /><input id='hidListaPrecio" + idFila + "' type='hidden' name='hidListaPrecio" + idFila + "' />";

        //PROY-140743 - INI
        if (codOperacion == '25') {
            oCell = newRow.insertCell();
            oCell.style.width = '60px';
            oCell.align = 'center';
            oCell.innerHTML = "<img name='imgVerLineasAsociadas" + idFila + "' id='imgVerLineasAsociadas" + idFila + "' src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Nro. Asociar' onclick='mostrarPopNroAsociar(" + idFila + ");' />";
        }
        //PROY-140743 - FIN

        if (parent.getValue('ddlModalidadVenta') == codModalidadChipSuelto) {
            oCell.style.display = 'none';
        }

        if (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion || (flujoPorta == 'S' && desTipoProductoActual != 'VentaVarios')) {
            oCell = newRow.insertCell();
            oCell.style.width = '62px';
            oCell.id = idFila + "txtNroTelefono";
            oCell.innerHTML = "<input" + disabled + readonly + " style='width:97%; text-align:right' class='clsInputEnable' maxlength='9' name='txtNroTelefono" + idFila + "' id='txtNroTelefono" + idFila + "' onkeypress='eventoSoloNumerosEnteros();' />";

            if (flujoPorta == 'S' && ((desTipoProductoActual != 'VentaVarios') && (desTipoProductoActual == 'Movil' || desTipoProductoActual == 'BAM'))) {
                //if (!(parent.validarEnvioConsultaPrevia())) { //PROY-140223 IDEA-140462
                  
                //INI: PROY-140335 RF1
                    oCell = newRow.insertCell();
                    oCell.style.width = '202px';
                    oCell.align = 'center';
                    oCell.id = idFila + "txtEstadoCP";
                    oCell.innerHTML = "<input style='width:97%; text-align:right' readonly class='clsInputDisable' name='txtEstadoCP" + idFila + "' id='txtEstadoCP" + idFila + "' />";
                oCell.style.display = 'none';

                    oCell = newRow.insertCell();
                    oCell.style.width = '1px';
                    oCell.align = 'center';
                    oCell.id = idFila + "txtFlagCPPermitida";
                    oCell.innerHTML = "<input style='width:1%; text-align:right' readonly class='clsInputDisable' name='txtFlagCPPermitida" + idFila + "' id='txtFlagCPPermitida" + idFila + "' />";
                oCell.style.visibility = 'hidden';    

                    oCell = newRow.insertCell();
                    oCell.style.width = '1px';
                    oCell.align = 'center';
                    oCell.id = idFila + "txtIdPortabilidad";
                    oCell.innerHTML = "<input style='width:1%; text-align:right' readonly class='clsInputDisable' name='txtIdPortabilidad" + idFila + "' id='txtIdPortabilidad" + idFila + "' />";
                    oCell.style.display = 'none';

                    //oCell = newRow.insertCell();
                    //oCell.style.width = '72px';
                    //oCell.align = 'center';
                    //oCell.id = idFila + "txtFecActivacionCP";
                    //oCell.innerHTML = "<input style='width:95%; text-align:right' readonly class='clsInputDisable' name='txtFecActivacionCP" + idFila + "' id='txtFecActivacionCP" + idFila + "' />";
                    //oCell.style.display = 'none'; //PROY-140335 RF1

                    //oCell = newRow.insertCell();
                    //oCell.style.width = '52px';
                    //oCell.align = 'center';
                    //oCell.id = idFila + "txtDeudaCP";
                    //oCell.innerHTML = "<input style='width:95%; text-align:right' readonly class='clsInputDisable' name='txtDeudaCP" + idFila + "' id='txtDeudaCP" + idFila + "' />";
                    //oCell.style.display = 'none'; //PROY-140335 RF1
                    //FIN: PROY-140335 RF1
                    //PROY-140335 - INICIO EJRC
            }
        } //PROY-140223 IDEA-140462

    }

    else if (desTipoProductoActual == 'DTH') {

        oCell = newRow.insertCell();
        oCell.style.width = '152px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlazo" + idFila + "' id='ddlPlazo" + idFila + "' onchange='cambiarPlazo(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '202px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlan" + idFila + "' id='ddlPlan" + idFila + "' onchange='cambiarPlan(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '32px';
        oCell.align = 'center';

        if (flgSecPendiente == "S")
            oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicioGuardado(" + idFila + ");' />";
        else
            oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicio(" + idFila + ");' />";

        oCell = newRow.insertCell();
        oCell.style.width = '202px';
        oCell.style.whiteSpace = 'nowrap';
        oCell.innerHTML = "<div class='AutoComplete_Div'><input type='hidden' id='hidMaterial" + idFila + "' name='hidMaterial" + idFila + "' /><input type='hidden' id='hidValorEquipo" + idFila + "' name='hidValorEquipo" + idFila + "' /><input" + disabled + " type='text' id='txtTextoEquipo" + idFila + "' name='txtTextoEquipo" + idFila + "' class='clsSelectEnable0' style='width: 170px' onclick=mostrarListaSel(" + idFila + ") onkeyup=buscarCoincidente(" + idFila + ") onblur=ocultarListaTxt(" + idFila + ") /><img id='imgListaEquipo" + idFila + "' src='../../Imagenes/cmb.gif' style='height: 17px; cursor: pointer' align='absMiddle' title='Mostrar Lista' alt='Mostrar Lista' onclick=mostrarListaSel(" + idFila + ") onmouseover='imgSel(this)' onmouseout='imgNoSel(this)'; onblur=ocultarListaTxt(" + idFila + ") /></div><div id='divListaEquipo" + idFila + "' class='AutoComplete_List' style='width: 262px; display: none; z-index:10'; onblur=ocultarListaTxt(" + idFila + ")></div><input type='hidden' id='hidKit" + idFila + "' name='hidKit" + idFila + "' />";

        if (flgSecPendiente) {
            var imgListaEquipo = document.getElementById('imgListaEquipo' + idFila);
            imgListaEquipo.style.display = 'none';
        }

        oCell = newRow.insertCell();
        oCell.style.display = 'none';
        oCell.innerHTML = "<input style='width:90%; text-align:right' readonly class='clsInputDisable' name='txtPrecioInst" + idFila + "' id='txtPrecioInst" + idFila + "' />";

        oCell = newRow.insertCell();
        oCell.style.width = '52px';
        oCell.innerHTML = "<input style='width:90%; text-align:right' readonly class='clsInputDisable' name='txtCFPlanServicio" + idFila + "' id='txtCFPlanServicio" + idFila + "' /><input type='hidden' id='hidMontoServicios" + idFila + "' name='hidMontoServicios" + idFila + "' />";

        oCell = newRow.insertCell();
        oCell.style.width = '52px';
        oCell.innerHTML = "<input style='width:90%; text-align:right' readonly class='clsInputDisable' name='txtCFMenAlqKit" + idFila + "' id='txtCFMenAlqKit" + idFila + "' />";

        oCell = newRow.insertCell();
        oCell.style.width = '52px';
        oCell.innerHTML = "<input style='width:90%; text-align:right' readonly class='clsInputDisable' name='txtCFTotMensual" + idFila + "' id='txtCFTotMensual" + idFila + "' />";

        oCell = newRow.insertCell();
        oCell.style.width = '37px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgDirInst" + idFila + "' id='imgDirInst" + idFila + "' src='../../Imagenes/ico_lupa.gif' border='0' style='cursor:hand' alt='Dir. Inst.' onclick='mostrarDirInst(" + idFila + ");' />";

    }

    else if (desTipoProductoActual == 'HFC') {

        oCell = newRow.insertCell();
        oCell.style.width = '145px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlazo" + idFila + "' id='ddlPlazo" + idFila + "' onchange='cambiarPlazo(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '162px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPaquete" + idFila + "' id='ddlPaquete" + idFila + "' onchange='cambiarPaquete(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '302px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlan" + idFila + "' id='ddlPlan" + idFila + "' onchange='cambiarPlan(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '277px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlServicio" + idFila + "' id='ddlServicio" + idFila + "' onchange='cambiarServicio(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '202px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%; display:none' class='clsSelectEnable0' name='ddlTopeConsumo" + idFila + "' id='ddlTopeConsumo" + idFila + "' onChange='mostrarTopesLTE(" + idFila + ");'><option>SELECCIONE...</option></select>"; // PROY-29296

        oCell = newRow.insertCell();
        oCell.style.width = '32px';
        oCell.align = 'center';

        if (flgSecPendiente == "S")
            oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicioGuardado(" + idFila + ");' />";
        else
            oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicio(" + idFila + ");' />";

        oCell = newRow.insertCell();
        oCell.style.width = '42px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgVerEquipo3Play" + idFila + "' id='imgVerEquipo3Play" + idFila + "' src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand; display:none' alt='Ver Equipos' onclick='mostrarEquipo(" + idFila + ");' /><input style='display:none' name='txtCFPlanServicio" + idFila + "' id='txtCFPlanServicio" + idFila + "' /><input type='hidden' id='hidMontoServicios" + idFila + "' name='hidMontoServicios" + idFila + "' /><input type='hidden' id='hidMontoCuota" + idFila + "' name='hidMontoCuota" + idFila + "' /><input type='hidden' id='hidMontoEquipo" + idFila + "' name='hidMontoEquipo" + idFila + "' />";

        oCell = newRow.insertCell();
        oCell.style.width = '37px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgDirInst" + idFila + "' id='imgDirInst" + idFila + "' src='../../Imagenes/ico_lupa.gif' border='0' style='cursor:hand;display:none' alt='Dir. Inst.' onclick='mostrarDirInst(" + idFila + ");' />";

        oCell = newRow.insertCell();
        oCell.style.width = '37px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgDetOfert" + idFila + "' id='imgDetOfert" + idFila + "' src='../../Imagenes/ico_lupa.gif' border='0' style='cursor:hand;display:none' alt='Det. Ofert.' onclick='mostrarDetOfert(" + idFila + ");' />";

        if (flujoPorta == 'S') {
            oCell = newRow.insertCell();
            oCell.style.width = '62px';
            oCell.innerHTML = "<input" + disabled + readonly + " style='width:90%; text-align:right; display:none' class='clsInputEnable' maxlength='9' name='txtNroTelefono" + idFila + "' id='txtNroTelefono" + idFila + "' onkeypress='eventoSoloNumerosEnteros();' />";
        }
    } else if (desTipoProductoActual == '3PlayInalam') {

        oCell = newRow.insertCell();
        oCell.style.width = '145px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlazo" + idFila + "' id='ddlPlazo" + idFila + "' onchange='cambiarPlazo(this.value, " + idFila + ");'><option value=''>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '162px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPaquete" + idFila + "' id='ddlPaquete" + idFila + "' onchange='cambiarPaquete(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '302px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlan" + idFila + "' id='ddlPlan" + idFila + "' onchange='cambiarPlan(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '277px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlServicio" + idFila + "' id='ddlServicio" + idFila + "' onchange='cambiarServicio(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '202px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%; display:none' class='clsSelectEnable0' name='ddlTopeConsumo" + idFila + "' id='ddlTopeConsumo" + idFila + "' onChange='mostrarTopesLTE(" + idFila + ");'><option>SELECCIONE...</option></select>"; //PROY-29296

        oCell = newRow.insertCell();
        oCell.style.width = '32px';
        oCell.align = 'center';

        if (flgSecPendiente == "S")
            oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicioGuardado(" + idFila + ");' />";
        else
            oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicio(" + idFila + ");' />";

        oCell = newRow.insertCell();
        oCell.style.width = '42px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgVerEquipo3Play" + idFila + "' id='imgVerEquipo3Play" + idFila + "' src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand; display:none' alt='Ver Equipos' onclick='mostrarEquipo(" + idFila + ");' /><input style='display:none' name='txtCFPlanServicio" + idFila + "' id='txtCFPlanServicio" + idFila + "' /><input type='hidden' id='hidMontoServicios" + idFila + "' name='hidMontoServicios" + idFila + "' /><input type='hidden' id='hidMontoCuota" + idFila + "' name='hidMontoCuota" + idFila + "' /><input type='hidden' id='hidMontoEquipo" + idFila + "' name='hidMontoEquipo" + idFila + "' />";

        oCell = newRow.insertCell();
        oCell.style.width = '37px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgDirInst" + idFila + "' id='imgDirInst" + idFila + "' src='../../Imagenes/ico_lupa.gif' border='0' style='cursor:hand;display:none' alt='Dir. Inst.' onclick='mostrarDirInst(" + idFila + ");' />";

        oCell = newRow.insertCell();
        oCell.style.width = '37px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgDetOfert" + idFila + "' id='imgDetOfert" + idFila + "' src='../../Imagenes/ico_lupa.gif' border='0' style='cursor:hand;display:none' alt='Det. Ofert.' onclick='mostrarDetOfert(" + idFila + ");' />";

        if (flujoPorta == 'S') {
            oCell = newRow.insertCell();
            oCell.style.width = '62px';
            oCell.innerHTML = "<input" + disabled + readonly + " style='width:90%; text-align:right; display:none' class='clsInputEnable' maxlength='9' name='txtNroTelefono" + idFila + "' id='txtNroTelefono" + idFila + "' onkeypress='eventoSoloNumerosEnteros();' />";
        } //FTTH - inicio -estructuraGrilla()
    } else if (desTipoProductoActual == 'FTTH') {
        oCell = newRow.insertCell();
        oCell.style.width = '145px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlazo" + idFila + "' id='ddlPlazo" + idFila + "' onchange='cambiarPlazo(this.value, " + idFila + ");'><option value=''>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '162px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPaquete" + idFila + "' id='ddlPaquete" + idFila + "' onchange='cambiarPaquete(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '302px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlan" + idFila + "' id='ddlPlan" + idFila + "' onchange='cambiarPlan(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '277px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlServicio" + idFila + "' id='ddlServicio" + idFila + "' onchange='cambiarServicio(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '202px';
        oCell.innerHTML = "<select" + disabled + " style='width:100%; display:none' class='clsSelectEnable0' name='ddlTopeConsumo" + idFila + "' id='ddlTopeConsumo" + idFila + "'><option>SELECCIONE...</option></select>";

        oCell = newRow.insertCell();
        oCell.style.width = '32px';
        oCell.align = 'center';

        if (flgSecPendiente == "S")
            oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicioGuardado(" + idFila + ");' />";
        else
            oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicio(" + idFila + ");' />";

        oCell = newRow.insertCell();
        oCell.style.width = '42px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgVerEquipo3Play" + idFila + "' id='imgVerEquipo3Play" + idFila + "' src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand; display:none' alt='Ver Equipos' onclick='mostrarEquipo(" + idFila + ");' /><input style='display:none' name='txtCFPlanServicio" + idFila + "' id='txtCFPlanServicio" + idFila + "' /><input type='hidden' id='hidMontoServicios" + idFila + "' name='hidMontoServicios" + idFila + "' /><input type='hidden' id='hidMontoCuota" + idFila + "' name='hidMontoCuota" + idFila + "' /><input type='hidden' id='hidMontoEquipo" + idFila + "' name='hidMontoEquipo" + idFila + "' />";

        oCell = newRow.insertCell();
        oCell.style.width = '37px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgDirInst" + idFila + "' id='imgDirInst" + idFila + "' src='../../Imagenes/ico_lupa.gif' border='0' style='cursor:hand;display:none' alt='Dir. Inst.' onclick='mostrarDirInst(" + idFila + ");' />";

        oCell = newRow.insertCell();
        oCell.style.width = '37px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgDetOfert" + idFila + "' id='imgDetOfert" + idFila + "' src='../../Imagenes/ico_lupa.gif' border='0' style='cursor:hand;display:none' alt='Det. Ofert.' onclick='mostrarDetOfert(" + idFila + ");' />";

        //Validar -FTTH -estructuraGrilla-portabilidaad
        if (flujoPorta == 'S') {
            oCell = newRow.insertCell();
            oCell.style.width = '62px';
            oCell.innerHTML = "<input" + disabled + readonly + " style='width:90%; text-align:right; display:none' class='clsInputEnable' maxlength='9' name='txtNroTelefono" + idFila + "' id='txtNroTelefono" + idFila + "' onkeypress='eventoSoloNumerosEnteros();' />";
        }
    }
    //FTTH - fin -estructuraGrilla
    //PROY-31812 - IDEA-43340 - INICIO
    if (desTipoProductoActual == 'InterInalam') {
        oCell = newRow.insertCell();
        oCell.style.width = '37px';
        oCell.align = 'center';
        oCell.innerHTML = "<img name='imgDetOfert" + idFila + "' id='imgDetOfert" + idFila + "' src='../../Imagenes/ico_lupa.gif' border='0' style='cursor:hand;' alt='Det. Ofert.' onclick='mostrarDetOfert(" + idFila + ");' />";
    }
    //PROY-31812 - IDEA-43340 - FIN
}

function obtenerTextoSeleccionado(ddl) {
    var resultado = '';

    for (var i = 0; i < ddl.options.length; i++) {
        if (ddl.options[i].selected)
            resultado = ddl.options[i].text;
    }

    return resultado;
}

function mostrarColumna(idCol, booVer) {
    var tblTablaTituloMovil = document.getElementById('tblTablaTituloMovil');

    if (booVer)
        tblTablaTituloMovil.rows[0].cells[idCol].style.display = "inline";
    else
        tblTablaTituloMovil.rows[0].cells[idCol].style.display = "none";
}

function mostrarColumnaCuota(visible) {
    var tblTablaTituloMovil = document.getElementById('tblTablaTituloMovil');
    var tblTablaTituloFijo = document.getElementById('tblTablaTituloFijo');
    var tblTablaTituloBAM = document.getElementById('tblTablaTituloBAM');
    var tblTablaTituloInterInalam = document.getElementById('tblTablaTituloInterInalam'); //PROY-31812 - IDEA-43340

    var ver = "inline";
    if (!visible) ver = "none";
    //gaa20161024
    //tblTablaTituloMovil.rows[0].cells[10].style.display = ver;
    document.getElementById('tdCuotaMovil').style.display = ver;
    //fin gaa20161024
    tblTablaTituloFijo.rows[0].cells[10].style.display = ver;
    tblTablaTituloBAM.rows[0].cells[9].style.display = ver;

    tblTablaTituloInterInalam.rows[0].cells[8].style.display = ver; //PROY-31812 - IDEA-43340

}

function mostrarColumnaPlazo(visible) {
    var tblTablaTituloMovil = document.getElementById('tblTablaTituloMovil');
    var tblTablaTituloFijo = document.getElementById('tblTablaTituloFijo');
    var tblTablaTituloBAM = document.getElementById('tblTablaTituloBAM');

    var ver = "inline";
    if (!visible) ver = "none";

    tblTablaTituloMovil.rows[0].cells[3].style.display = ver;
    tblTablaTituloFijo.rows[0].cells[3].style.display = ver;
    tblTablaTituloBAM.rows[0].cells[3].style.display = ver;
}

function mostrarColumnaEquipo(visible) {
    var tblTablaTituloMovil = document.getElementById('tblTablaTituloMovil');
    var tblTablaTituloFijo = document.getElementById('tblTablaTituloFijo');
    var tblTablaTituloBAM = document.getElementById('tblTablaTituloBAM');
    var tblTablaTituloInterInalam = document.getElementById('tblTablaTituloInterInalam'); // PROY-31812 - IDEA-43340

    var ver = "inline";
    if (!visible) ver = "none";

    // Equipo
    //gaa20161024
    //tblTablaTituloMovil.rows[0].cells[11].style.display = ver;
    document.getElementById('tdEquipoMovil').style.display = ver;
    //fin gaa20161024
    tblTablaTituloFijo.rows[0].cells[11].style.display = ver;
    tblTablaTituloBAM.rows[0].cells[10].style.display = ver;

    tblTablaTituloInterInalam.rows[0].cells[7].style.display = ver; //PROY-31812 - IDEA-43340

    // Precio Equipo
    //gaa20161024
    //tblTablaTituloMovil.rows[0].cells[9].style.display = ver;
    document.getElementById('tdEquipoPrecioMovil').style.display = ver;
    //fin gaa20161024
    tblTablaTituloFijo.rows[0].cells[9].style.display = ver;
    tblTablaTituloBAM.rows[0].cells[8].style.display = ver;

    tblTablaTituloInterInalam.rows[0].cells[9].style.display = ver; //PROY-31812 - IDEA-43340

}

function mostrarColumnaTelefono(visible) {
    var tblTablaTituloMovil = document.getElementById('tblTablaTituloMovil');
    var tblTablaTituloBAM = document.getElementById('tblTablaTituloBAM');
    var tblTablaTituloFijo = document.getElementById('tblTablaTituloFijo');
    var tblTablaTituloHFC = document.getElementById('tblTablaTituloHFC');

    //PROY-32581 - INICIO
    var tblTablaTitulo3PlayInalam = document.getElementById('tblTablaTitulo3PlayInalam');
    //PROY-32581 - FIN

    var txtTitulo = "Nro. a Portar";
    if (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion) txtTitulo = "Nro. a Migrar";

    var ver = "inline";
    if (!visible) ver = "none";
    //gaa20161024
    //tblTablaTituloMovil.rows[0].cells[12].style.display = ver;
    document.getElementById('tdTelefonoMovil').style.display = ver;
    //fin gaa20161024
    tblTablaTituloBAM.rows[0].cells[11].style.display = ver;
    tblTablaTituloFijo.rows[0].cells[12].style.display = ver;
    tblTablaTituloHFC.rows[0].cells[12].style.display = ver;

    tblTablaTitulo3PlayInalam.rows[0].cells[12].style.display = ver; //PROY-32581

    if (parent.getValue('hidNTienePortabilidadValues') != 'S') {
        tblTablaTituloHFC.rows[0].cells[12].style.display = "none";
        tblTablaTitulo3PlayInalam.rows[0].cells[12].style.display = "none"; //PROY-32581
    }
    //gaa20161024
    //tblTablaTituloMovil.rows[0].cells[12].innerHTML = txtTitulo;
    document.getElementById('tdTelefonoMovil').innerHTML = txtTitulo;
    //fin gaa20161024
    tblTablaTituloBAM.rows[0].cells[11].innerHTML = txtTitulo;
    tblTablaTituloFijo.rows[0].cells[12].innerHTML = txtTitulo;
    tblTablaTituloHFC.rows[0].cells[12].innerHTML = txtTitulo;

    tblTablaTitulo3PlayInalam.rows[0].cells[12].innerHTML = txtTitulo; //PROY-32581
}

function getValor(strValor, idValor) {
    if (strValor.indexOf('_') > -1) {
        var arrCodigo = strValor.split('_');
        return arrCodigo[idValor];
    }
    else
        return '';
}

function asignarPlan(idFila, strValor) {
    var ddlPlan = document.getElementById('ddlPlan' + idFila);

    llenarDatosCombo(ddlPlan, strValor, true);
    //INI PROY-140434
    var ddlPlanAnterior = "";
    var codTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    var totalFilas = document.getElementById('tblTablaMovil').rows.length;

    if (codTipoProductoActual == codTipoProdMovil && (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion || parent.getValue('ddlTipoOperacion') == flujoAlta)) {
        if (idFila > 1 && totalFilas > 1) {
            ddlPlanAnterior = document.getElementById('ddlPlan' + (idFila - 1)).value;
        }
        if (ddlPlanAnterior == "") {
            PlanxDefecto(idFila, strValor);
        }
    }
    //FIN PROY-140434
    calcularLCxProductoFijo();
}

function llenarMaterial(idFila, strValor) {
    document.getElementById('hidMaterial' + idFila).value = strValor;
    document.getElementById('hidFilaProa').value = idFila; //PROY-30748
}

function llenarCampana(idFila, strValor) {
    var hidCampana = document.getElementById('hidCampana');
    hidCampana.value = strValor;

    asignarCampana(idFila);
}

function asignarCampana(idFila) {
    if (idFila < 0)
        idFila = document.getElementById('hidLineaActual').value;

    var strCampana = document.getElementById('hidCampana').value;
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    llenarDatosCombo(ddlCampana, strCampana, true);
}

function traerPlazo(strValor) {
    var hidTraerPlazo = document.getElementById('hidTraerPlazo');
    hidTraerPlazo.value = strValor;
}

function obtenerFlujo() {
    var strTienePortabilidad = parent.document.getElementById('hidNTienePortabilidadValues').value;
    var strFlujo = flujoAlta;

    if (strTienePortabilidad == 'S')
        strFlujo = flujoPortabilidad;

    return strFlujo;
}

function cambiarTipoProducto(idFila) {
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    var tipoProductoActual = getValue('hidTipoProductoActual');
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    var ddlCampana = document.getElementById('ddlCampana' + idFila);

    if (ddlPlazo == null)
        return;

    var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
    var tabla = document.getElementById('tblTabla' + tipoProductoActual);
    var strPlazos;
    var imgVerCuota = document.getElementById('imgVerCuota' + idFila);

    if (!verificarCombo()) {
        if (ddlPaquete != null)
            llenarDatosCombo(ddlPaquete, '', true);

        llenarDatosCombo(document.getElementById('ddlPlan' + idFila), '', true);
    }

    if (!verificarCombo()) {
        if (codigoTipoProductoActual == codTipoProdDTH && ddlCampana != null)
            llenarDatosCombo(ddlCampana, '', true);
    }

    if (!mostrarDirInst(idFila)) {
        eliminarFilaTotal(null, idFila, false);
        return;
    }

    visualizarIconosFinales(idFila);

    ddlPlazo.value = '';
    strPlazos = verificarPlazo(getValue('hidTipoProductoActual'));

    if (strPlazos.length > 0) {
        llenarDatosCombo(ddlPlazo, strPlazos, false);
        ddlPlazo.disabled = true;
        cambiarPlazo(ddlPlazo.value.split('-')[0], idFila);//INICIATIVA 920
    }
    else {
        ddlPlazo.disabled = false;
    }

    if (!verificarCombo())
        llenarCampanaPlazoIfr(idFila);
}

function visualizarIconosFinales(idFila) {
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    var imgVerCuota = document.getElementById('imgVerCuota' + idFila);

    //visualizar iconos Dir.Instalación - Oferta

    if (codigoTipoProductoActual == codTipoProdDTH || codigoTipoProductoActual == codTipoProd3Play || codigoTipoProductoActual == codTipoProd3PlayInalam || codigoTipoProductoActual == codTipoProdFTTH) {
        if (imgVerCuota != null)
            imgVerCuota.style.display = 'none';

        var imgDirInst = document.getElementById('imgDirInst' + idFila);

        if (imgDirInst != null)
            imgDirInst.style.display = '';



        if ((codigoTipoProductoActual == codTipoProd3Play) || (codigoTipoProductoActual == codTipoProd3PlayInalam) || (codigoTipoProductoActual == codTipoProdFTTH))
            document.getElementById('imgDetOfert' + idFila).style.display = '';

        var imgVerEquipo3Play = document.getElementById('imgVerEquipo3Play' + idFila);

        if (imgVerEquipo3Play != null)
            imgVerEquipo3Play.style.display = '';
    }
    else {
        if (imgVerCuota != null)
            imgVerCuota.style.display = '';
    }
}
//PROY-140743-INI
function llenarPromociones(idFila) {

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
    url += '&strCasoEspecial=' + getValor(parent.getValue('ddlCasoEspecial'), 0);
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&strOferta=' + parent.getValue('ddlOferta');
    url += '&strCampana=' + getValue('ddlCampana' + idFila);
    url += '&strCombo=' + parent.getValue('ddlCombo');
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strModalidadVenta=' + parent.getValue('ddlModalidadVenta');
    url += '&strFlujo=' + obtenerFlujo();
    url += '&strTipoDocumento=' + parent.getValue('ddlTipoDocumento');
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strTipoOficina=' + parent.getValue('ddlCanal');
    url += '&strMetodo=' + 'llenarPromociones';

    self.frames['iframeAuxiliar'].location.replace(url);

}
//PROY-140743-FIN

function llenarCampanaPlazoIfr(idFila) {
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var strCampanasDTH = getValue('hidCampanasDTH');
    var strCampanasHFC = getValue('hidCampanasHFC');
    var strCampanasFTTH = getValue('hidCampanasFTTH'); //FTTH -llenarCampanaPlazoIfr() -hidCampanasFTTH
    var strCampanasMovil = getValue('hidCampanasMovil');
    var strCampanasFijo = getValue('hidCampanasFijo');
    var strCampanasBAM = getValue('hidCampanasBAM');

    var strCampanasInterInalam = getValue('hidCampanasInterInalam'); //PROY-31812 - IDEA-43340

    var strCampanasVentaVarios = getValue('hidCampanasVentaVarios');
    var strCampanasHFCInalamb = getValue('hidCampanasHFCInalamb');

    //FTTH - Inicio -  strCampanasHFC = ''  - Evalenzs 
    if (ConsPlanoCampanaFTTH != '' && codigoTipoProductoActual == codTipoProd3Play) {
        strCampanasHFC = '';
    }
    if (codigoTipoProductoActual == codTipoProdFTTH) {
        strCampanasFTTH = '';
    }

    //INC000002510501
    var flagPortabilidadc = parent.getValue('hidNTienePortabilidadValues');
    var modalidad1 = getValue('hidModalidadPorta');
    var modalidad2 = parent.getValue('ddlModalidadPorta');
    var operador1 = getValue('hidOperadorCedente');
    var operador2 = parent.getValue('ddlOperadorCedente');
    var flagCampanaR = getValue('hidFlagCampana');
    if (flagCampanaR == "1" && flagPortabilidadc == "S" && (modalidad1 != modalidad2 || operador1 != operador2)) {
        if (codigoTipoProductoActual == codTipoProdMovil) {
            strCampanasMovil = "";
        }
    }

    //FTTH - Fin -  strCampanasHFC = ''  - Evalenzs  
    switch (codigoTipoProductoActual) {
        case codTipoProdMovil:
            if (strCampanasMovil.length > 0) {
                llenarDatosCombo(ddlCampana, strCampanasMovil, true);
                return;
            }
            break;
        case codTipoProdFijo:
            if (strCampanasFijo.length > 0) {
                llenarDatosCombo(ddlCampana, strCampanasFijo, true);
                return;
            }
            break;
        //PROY-31812 - IDEA-43340 - INICIO  
        case codTipoProdInterInalam:
            if (strCampanasInterInalam.length > 0) {
                llenarDatosCombo(ddlCampana, strCampanasInterInalam, true);
                return;
            }
            break;
        //PROY-31812 - IDEA-43340 - FIN  
        case codTipoProdBAM:
            if (strCampanasBAM.length > 0) {
                llenarDatosCombo(ddlCampana, strCampanasBAM, true);
                return;
            }
            break;
        case codTipoProdDTH:
            if (strCampanasDTH.length > 0) {
                llenarDatosCombo(ddlCampana, strCampanasDTH, true);
                return;
            }
            break;
        case codTipoProd3Play:
            if (strCampanasHFC.length > 0) {
                llenarDatosCombo(ddlCampana, strCampanasHFC, true);
                return;
            }
            break;
        //FTTH - Inicio strCampanasFTTH.length    
        case codTipoProdFTTH:
            if (strCampanasFTTH.length > 0) {
                llenarDatosCombo(ddlCampana, strCampanasFTTH, true);
                return;
            }
            break;
        //FTTH - Fin strCampanasFTTH.length    
        case codTipoProdVentaVarios:
            if (strCampanasVentaVarios.length > 0) {
                llenarDatosCombo(ddlCampana, strCampanasVentaVarios, true);
                return;
            }
            break;
        case codTipoProd3PlayInalam:
            if (strCampanasHFCInalamb.length > 0) {
                llenarDatosCombo(ddlCampana, strCampanasHFCInalamb, true);
                return;
            }
            break;
    }

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
    url += '&strCasoEspecial=' + getValor(parent.getValue('ddlCasoEspecial'), 0);
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&strOferta=' + parent.getValue('ddlOferta');
    url += '&strCampana=' + getValue('ddlCampana' + idFila);
    url += '&strCombo=' + parent.getValue('ddlCombo');
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strModalidadVenta=' + parent.getValue('ddlModalidadVenta');
    url += '&strFlujo=' + obtenerFlujo();
    url += '&strTipoDocumento=' + parent.getValue('ddlTipoDocumento');
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strModalidadPorta=' + parent.getValue('ddlModalidadPorta'); //INC000002510501
    url += '&strOperadorPorta=' + parent.getValue('ddlOperadorCedente'); //INC000002510501
    url += '&strMetodo=' + 'LlenarCampanaPlazo';

    self.frames['iframeAuxiliar'].location.replace(url);
}

//PROY-140743-INI
function asignarPromociones(idFila, strValor) {
    var strPromociones = strValor;
    var ddlPromocion = document.getElementById('ddlPromocion' + idFila);

    llenarDatosCombo(ddlPromocion, strPromociones, true);
}
//PROY-140743-FIN

function asignarCampanaPlazo(idFila, strValor) {
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    var arrValor = strValor.split('¬');
    var strCampanas = arrValor[0];
    var strPlazos = arrValor[1];
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);

    switch (codigoTipoProductoActual) {
        case codTipoProdMovil:
            setValue('hidCampanasMovil', strCampanas);
            setValue('hidPlazosMovil', strPlazos);
            break;
        case codTipoProdFijo:
            setValue('hidCampanasFijo', strCampanas);
            setValue('hidPlazosFijo', strPlazos);
            break;
        //PROY-31812 - IDEA-43340 - INICIO   
        case codTipoProdInterInalam:
            setValue('hidCampanasInterInalam', strCampanas);
            setValue('hidPlazosInterInalam', strPlazos);
            break;
        //PROY-31812 - IDEA-43340 - FIN  
        case codTipoProdBAM:
            setValue('hidCampanasBAM', strCampanas);
            setValue('hidPlazosBAM', strPlazos);
            break;
        case codTipoProdDTH:
            setValue('hidCampanasDTH', strCampanas);
            setValue('hidPlazosDTH', strPlazos);
            break;
        case codTipoProd3Play:
            setValue('hidCampanasHFC', strCampanas);
            setValue('hidPlazosHFC', strPlazos);
            break;

        //FTTH - Inicio hidCampanasFTTH - hidPlazosFTTH     
        case codTipoProdFTTH:
            setValue('hidCampanasFTTH', strCampanas);
            setValue('hidPlazosFTTH', strPlazos);
            break;
        //FTTH - Fin hidCampanasFTTH - hidPlazosFTTH     

        case codTipoProdVentaVarios:
            setValue('hidCampanasVentaVarios', strCampanas);
            setValue('hidPlazosVentaVarios', strPlazos);
            break;
        case codTipoProd3PlayInalam:
            setValue('hidCampanasHFCInalamb', strCampanas);
            setValue('hidPlazosHFCInalamb', strPlazos);
            break;
    }

    //INC000002510501
    var flagPortabilidadc = parent.getValue('hidNTienePortabilidadValues');
    if (flagPortabilidadc == "S") {
        var ddlModalidad1 = parent.getValue('ddlModalidadPorta');
        var ddlOperador1 = parent.getValue('ddlOperadorCedente');
        document.getElementById('hidModalidadPorta').value = ddlModalidad1;
        document.getElementById('hidOperadorCedente').value = ddlOperador1;
    }

    llenarDatosCombo(ddlCampana, strCampanas, true);
    llenarDatosCombo(ddlPlazo, strPlazos, true);
}

function mostrarDirInst(idFila) {
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    //FTTH - mostrarDirInst(idFila)-if ( codigoTipoProductoActual != codTipoProdFTTH

    //PROY-140690 - SE AGREGO MOSTRAR POPUP PARA => codTipoProdInterInalam
    if (codigoTipoProductoActual != codTipoProdFTTH && codigoTipoProductoActual != codTipoProdDTH && codigoTipoProductoActual != codTipoProd3Play && codigoTipoProductoActual != codTipoProd3PlayInalam && codigoTipoProductoActual != codTipoProdInterInalam) {
        return true;
    }

    var tipoDocumento = parent.getValue('ddlTipoDocumento');//PROY-140690
    var strTieneDireccion = '2';
    var intDeshabilitado = 'N';
    var flgVentaProactiva = parent.getValue('hidVerVentaProactiva');
    var url = '../consultas/sisact_direccion.aspx?idFila=' + idFila;

    url += '&nroDocumento=' + parent.getValue('hidNroDocumento');
    url += '&tipoProducto=' + codigoTipoProductoActual;

    if (codigoTipoProductoActual == codTipoProdDTH) {
        if (document.getElementById('ddlPlan' + idFila).disabled)
            intDeshabilitado = 'S';

        url += '&flgReadOnly=' + intDeshabilitado;
        url += '&flgVentaProactiva=' + flgVentaProactiva;
        url += '&codigoTipoProductoActual=' + codigoTipoProductoActual;
        strTieneDireccion = window.showModalDialog(url, '', 'dialogHeight:500px; dialogWidth:1000px');
    }
    else {
        if (codigoTipoProductoActual == codTipoProd3Play) {
            if (document.getElementById('ddlServicio' + idFila).disabled)
                intDeshabilitado = 'S';

            url += '&flgReadOnly=' + intDeshabilitado;
            url += '&codigoTipoProductoActual=' + codigoTipoProductoActual;

            if (document.getElementById('ddlServicio' + idFila).disabled)
                strTieneDireccion = window.showModalDialog(url, '', 'dialogHeight:320px; dialogWidth:1000px');
            else
                strTieneDireccion = window.showModalDialog(url, '', 'dialogHeight:550px; dialogWidth:1000px');
        }
        //FTTH - inicio  - mostrarDirInst(idFila)
        if (codigoTipoProductoActual == codTipoProdFTTH) {
            //FTTH - inicio  - mostrarDirInst(idFila)  DOS VECES LA DIRECCION
            if (document.getElementById('ddlServicio' + idFila).disabled)
                intDeshabilitado = 'S';

            url += '&flgReadOnly=' + intDeshabilitado;
            url += '&codigoTipoProductoActual=' + codigoTipoProductoActual;

            if (document.getElementById('ddlServicio' + idFila).disabled)
                strTieneDireccion = window.showModalDialog(url, '', 'dialogHeight:320px; dialogWidth:1000px');
            else
                strTieneDireccion = window.showModalDialog(url, '', 'dialogHeight:550px; dialogWidth:1000px');
        }
        //FTTH - fin  - mostrarDirInst(idFila)
        if (codigoTipoProductoActual == codTipoProd3PlayInalam) {

            if (document.getElementById('ddlServicio' + idFila).disabled)
                intDeshabilitado = 'S';

            url += '&flgReadOnly=' + intDeshabilitado;
            url += '&flgVentaProactiva=' + flgVentaProactiva;

            url += '&codigoTipoProductoActual=' + codigoTipoProductoActual;

            strTieneDireccion = window.showModalDialog(url, '', 'dialogHeight:350px; dialogWidth:1000px');
        }
        //PROY-140690 - INTERNET INALAMBRICO
        if (codigoTipoProductoActual == codTipoProdInterInalam) {
			//INICIO INICIATIVA-932
            if (Key_FlagGeneralCobertura == '1') {
				var validarDireccion = parent.getValue('hidRestringirCoberturaIFI');
            
	            if (validarDireccion == '0') {
	                if (document.getElementById('ddlPlan' + idFila).disabled)
	                    intDeshabilitado = 'S';
	
	                url += '&flgReadOnly=' + intDeshabilitado;
	                url += '&flgVentaProactiva=' + flgVentaProactiva;
	                url += '&codigoTipoProductoActual=' + codigoTipoProductoActual;
	                url += '&tipoDocumento=' + tipoDocumento;
	                strTieneDireccion = window.showModalDialog(url, '', 'dialogHeight:350px; dialogWidth:1000px');
	
	            }
	            else {
	                return true;
	            }
			}
			else{
            if (document.getElementById('ddlPlan' + idFila).disabled)
                intDeshabilitado = 'S';

            url += '&flgReadOnly=' + intDeshabilitado;
            url += '&flgVentaProactiva=' + flgVentaProactiva;
            url += '&codigoTipoProductoActual=' + codigoTipoProductoActual;
            url += '&tipoDocumento=' + tipoDocumento;
            strTieneDireccion = window.showModalDialog(url, '', 'dialogHeight:350px; dialogWidth:1000px');
        }
			//FIN INICIATIVA-932
        }
    }

    if (strTieneDireccion == undefined || strTieneDireccion.indexOf('|') < 0)
        return false;

    var hidFlagVOD = document.getElementById('hidFlagVOD');
    var strFlagsVOD = hidFlagVOD.value;
    var strCadena = '[' + idFila + ']:' + strTieneDireccion.split('|')[1] + '|';
    var intPosIni = strFlagsVOD.indexOf('[' + idFila + ']');
    var str = '';
    var intPosFin;

    if (intPosIni > -1) {
        str = strFlagsVOD.substring(intPosIni);
        intPosFin = str.indexOf('|');
        intPosFin += intPosIni + 1;

        hidFlagVOD.value = strFlagsVOD.replace(strFlagsVOD.substring(intPosIni, intPosFin), strCadena);
    }
    else
        hidFlagVOD.value += strCadena;

    return true;
}

function llenarDatosCombo(ddl, strDatos, booSeleccione) {
    var arrDatos;
    var arrItem;
    var strDato;
    var option;

    if (ddl == null)
        return;

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

                //INICIATIVA-1012
				if (parent.getValue('hdiRestriccionCampanasFullClaro') == '0') {
					var datosRestriccionCampanas = getValue('hdiValoresRestringirCampanaFullClaro');
					var arrDatosRestriccionCampanas = datosRestriccionCampanas.split("|");
					var validadorAgregar = '0';
					
					for (x = 0; x < arrDatosRestriccionCampanas.length; x++)
					{
						if (option.value == arrDatosRestriccionCampanas[x]) {
							validadorAgregar = '0';
							break;
						}else{
							validadorAgregar = '1';
						}
					}

					if (validadorAgregar == '1') {
						ddl.add(option);
					}
				} else {
					ddl.add(option);
				}
            }
        }
    }
}
//INI PROY-140434
function PlanxDefecto(idFila, strDatos) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][PlanxDefecto()] ", "Entro en la funcion PlanxDefecto()");

    var lCredito = parent.getValue('txtLCDisponiblexProd');
    var difIni = 0;
    var difFin = 0;
    var planLC = "";

    if (strDatos != null)
        var arrDatos = strDatos.split("|");
    else
        return;

    for (i = 0; i < arrDatos.length; i++) {
        option = document.createElement('option');

        if (arrDatos[i].length > 0) {
            strDato = arrDatos[i];
            arrItem = strDato.split(";");
            arrCF = arrItem[0].split("_");
            if (difIni > 0) {
                difFin = parseFloat(lCredito) - parseFloat(arrCF[1]);
                if (difFin == 0) {
                    planLC = arrItem[0];
                    break;
                }
                if (difIni > difFin && difFin >= 0) {
                    difIni = difFin;
                    planLC = arrItem[0];
                }
            }
            else {
                difIni = parseFloat(lCredito) - parseFloat(arrCF[1]);
                planLC = arrItem[0];
                if (difIni == 0) {
                    planLC = arrItem[0];
                    break;
                }
            }
        }
        else {
            difIni = parseFloat(lCredito);
            if (difIni == 0) {
                planLC = "";
                break;
            }
        }
    }
    document.getElementById('ddlPlan' + idFila).value = planLC;

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][PlanxDefecto()] planLC ", planLC);

    if (planLC != "") {
        cambiarPlan(planLC, idFila);
    }
}
//FIN PROY-140434

//gaa20160211
function validarEliminarFila(idFila) {
    var tabla = document.getElementById('tblTabla' + getValue('hidTipoProductoActual'));
    var cont = tabla.rows.length;
    var fila;
    var idFilaX;

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];

        idFilaX = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        if (idFila == idFilaX) {
            if (fila.cells[1].innerHTML == '') {
                alert('No se puede eliminar esta fila');
                return false;
            }
        }
    }

    return true;
}
//fin gaa20160211
function eliminarFilaGrupo(fila, idFila, mostrarAdvertencia, borrarGrupo) {
    if (mostrarAdvertencia) {
        if (!confirm('¿Desea eliminar Solución/Paquete/Plan?'))
            return false;
    }

    //PROY-140743 - INI
    var codOperacion = parent.getValue('ddlTipoOperacion');
    if (codOperacion == 25 || codOperacion == '25') {
        parent.document.getElementById('btnAgregarPlan').disabled = false;
    }
    //PROY-140743 - FIN


    //PROY-140245
    var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');

    if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0) {

        var producto = obtenerProductoxIdFila(idFila);
        var strCodProducto;

        switch (producto) {
            case 'Movil':
                strCodProducto = codTipoProdMovil;
                break;
            case 'BAM':
                strCodProducto = codTipoProdBAM;
                break;
            case 'DTH':
                strCodProducto = codTipoProdDTH;
                break;
            case 'HFC':
                strCodProducto = codTipoProd3Play;
                break;
            case 'Fijo':
                strCodProducto = codTipoProdFijo;
                break;
            case '3PlayInalam':
                strCodProducto = codTipoProd3PlayInalam;
                break;
            case 'InterInalam':
                strCodProducto = codTipoProdInterInalam;
                break;

        }

        actualizarCantidadProductosCasoEspColab(strCodProducto);
    }


    //FIN PROY-140245

    if (document.getElementById('hidValidarGuardarCuota').value.length > 0) {
        alert('Debe guardar las cuotas antes de ejecutar esta acción');
        return false;
    }

    var hidNGrupoPaqueteValues = document.getElementById('hidNGrupoPaqueteValues');
    var strGrupoPaquete = hidNGrupoPaqueteValues.value;
    var strVB = '[' + idFila + ']';
    var intPosFin = strGrupoPaquete.indexOf(strVB);
    var intPosIni;
    var intPosFin;
    var arrGrupo;

    if (intPosFin > -1) {
        intPosIni = strGrupoPaquete.substring(0, intPosFin).lastIndexOf('{') + 1;
        intPosFin = strGrupoPaquete.substring(intPosIni).indexOf('}') + intPosIni;

        arrGrupo = strGrupoPaquete.substring(intPosIni, intPosFin).split(',')

        for (var i = 1; i < arrGrupo.length; i++) {
            eliminarFilaGrupal(arrGrupo[i].replace('[', '').replace(']', ''));
        }

        intPosIni = strGrupoPaquete.substring(0, intPosFin).lastIndexOf('{');
        intPosFin = strGrupoPaquete.substring(intPosIni).indexOf('}') + intPosIni + 1;

        if (borrarGrupo)
            hidNGrupoPaqueteValues.value = strGrupoPaquete.replace(strGrupoPaquete.substring(intPosIni, intPosFin), '');
        else
            return strGrupoPaquete.substring(intPosIni, intPosFin);
    }
    else
        eliminarFila(fila, idFila);

    return true;
}

function eliminarFilaGrupal(idFila) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][eliminarFilaGrupal()] ", "Entro en la funcion eliminarFilaGrupal()");

    var producto = obtenerProductoxIdFila(idFila);
    var tabla = document.getElementById('tblTabla' + producto);
    var cont = tabla.rows.length;
    var strValor;
    var intPosIni;

    cerrarServicio();
    cerrarCuota();
    cerrarEquipo();
    cerrarLineasCuentas(); //PROY-140743

    for (var i = 0; i < cont; i++) {
        strValor = tabla.rows(i).cells[2].innerHTML;

        /*if (producto != 'DTH' && producto != 'HFC') //ddlPlazo
        intPosIni = strValor.indexOf('ddlPlazo') + 8;
        else*/
        intPosIni = strValor.indexOf('ddlCampana') + 10;

        intPosFin = strValor.substring(intPosIni).indexOf(' ') + intPosIni;

        if (idFila == strValor.substring(intPosIni, intPosFin)) {
            tabla.deleteRow(i);

            eliminarItem(idFila);
            borrarServicio(idFila);
            borrarEquipo(idFila);
            borrarCuota(idFila);

            if (tabla.rows.length == 0) {
                document.getElementById('hidPlazoActual').value = '';
                traerPlazo('S');
                document.getElementById('txtCFTotal').value = '0';
            }
            else
                calcularCFxProducto();

            return;
        }
    }
}

function eliminarFila(fila, idFila) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][eliminarFila()] ", "Entro en la funcion eliminarFila()");

    borrarServicio(idFila);
    borrarCuota(idFila);
    borrarEquipo(idFila);

    cerrarServicio();
    cerrarCuota();
    cerrarEquipo();
    cerrarLineasCuentas(); //PROY-140743

    if (fila == null)
        fila = document.getElementById('ddlPlazo' + idFila);

    if (fila == null)
        return;

    var id = fila.parentNode.parentNode.rowIndex;
    var tabla = document.getElementById(fila.parentNode.parentNode.parentNode.parentNode.id);

    tabla.deleteRow(id);
    eliminarItem(idFila);

    if (tabla.rows.length == 0) {
        document.getElementById('hidPlazoActual').value = '';
        traerPlazo('S');
        document.getElementById('txtCFTotal').value = '0';
    }
    else
        calcularCFxProducto();
}

function verificarPlanes() {
    var tabla = document.getElementById('tblTabla' + getValue('hidTipoProductoActual'));
    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var codigoSinCuota = '00';
    var codigoOtro = '';
    var codigoCuota;

    var strCasoEpecial = getValor(parent.getValue('ddlCasoEspecial'), 0);
    var blnCasoEspecialCMA = (strCasoEpecial == constCETrabajadoresCMA);

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];

        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        if (idFila.length == 0)
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);

        if (!preservarFila(idFila))
            return false;

        arrCuota = obtenerCuotaValores(idFila);

        if (arrCuota != null) {
            codigoCuota = arrCuota[0].split('_')[0];

            if (codigoCuota.length == 0)
                codigoCuota = codigoSinCuota;

            if (blnCasoEspecialCMA && codigoCuota == codigoSinCuota) {
                alert('Debe seleccionar una cuota para el plan');
                return false;
            }

            if (codigoOtro.length == 0) {
                codigoOtro = codigoCuota;
                if (codigoOtro.length == 0)
                    codigoOtro = codigoSinCuota;
            }
            else {
                if (codigoOtro != codigoCuota) {
                    alert('Se debe emplear una misma cuota para todos los planes');
                    return false;
                }
            }
        }
        else {
            codigoCuota = codigoSinCuota;
            if (codigoOtro.length == 0)
                codigoOtro = codigoSinCuota;

            if (blnCasoEspecialCMA && codigoOtro == codigoSinCuota) {
                alert('Debe seleccionar una cuota para el plan');
                return false;
            }

            if (codigoOtro != codigoCuota) {
                alert('Se debe emplear una misma cuota para todos los planes');
                return false;
            }
        }
    }
    return true;
}

function quitarFilas() {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][quitarFilas()]", "Entro a la funcion quitarFilas()");

    var tabla = document.getElementById('tblTablaMovil');
    var cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        tabla.deleteRow(0);
    }
    tabla = document.getElementById('tblTablaFijo');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        tabla.deleteRow(0);
    }
    //PROY-31812 - IDEA-43340 - INICIO
    tabla = document.getElementById('tblTablaInterInalam');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        tabla.deleteRow(0);
    }
    //PROY-31812 - IDEA-43340 - FIN
    tabla = document.getElementById('tblTablaBAM');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        tabla.deleteRow(0);
    }
    tabla = document.getElementById('tblTablaDTH');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        tabla.deleteRow(0);
    }
    tabla = document.getElementById('tblTablaHFC');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        tabla.deleteRow(0);
    }
    //FTTH  - Inicio - quitarFilas()
    tabla = document.getElementById('tblTablaFTTH');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        tabla.deleteRow(0);
    }
    //FTTH  - Inicio - quitarFilas()

    tabla = document.getElementById('tblTabla3PlayInalam');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        tabla.deleteRow(0);
    }
    tabla = document.getElementById('tblTablaVentaVarios');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        tabla.deleteRow(0);
    }
    document.getElementById('hidnPlanServicioValue').value = '';
    document.getElementById('hidPlanServicioNo').value = '';
    document.getElementById('hidPlanServicioNoGrupo').value = '';
    document.getElementById('hidPlanServicioNGTemp').value = '';
    document.getElementById('hidLineaActual').value = '0';
    document.getElementById('hidTotalLineas').value = '0';
    document.getElementById('hidPlazoActual').value = '';
    document.getElementById('hidCuota').value = '';
    document.getElementById('hidNroCuotaActual').value = '';
    document.getElementById('hidNGrupoPaqueteValues').value = '';
    document.getElementById('hidConcatCuotaIni').value = ''; //PROY-30166-IDEA–38863

    document.getElementById('hidCampanasMovil').value = '';
    document.getElementById('hidCampanasFijo').value = '';
    document.getElementById('hidCampanasInterInalam').value = ''; //PROY-31812 - IDEA-43340
    document.getElementById('hidCampanasBAM').value = '';
    document.getElementById('hidCampanasDTH').value = '';
    document.getElementById('hidCampanasHFC').value = '';
    document.getElementById('hidCampanasFTTH').value = ''; //FTTH -'hidCampanasFTTH').value = ''
    document.getElementById('hidCampanasVentaVarios').value = '';

    document.getElementById('hidPlazosMovil').value = '';
    document.getElementById('hidPlazosFijo').value = '';
    document.getElementById('hidPlazosInterInalam').value = ''; //PROY-31812 - IDEA-43340
    document.getElementById('hidPlazosBAM').value = '';
    document.getElementById('hidPlazosDTH').value = '';
    document.getElementById('hidPlazosHFC').value = '';
    document.getElementById('hidPlazosFTTH').value = ''; //FTTH- 'hidPlazosFTTH').value = ''
    document.getElementById('hidPlazosVentaVarios').value = '';

    document.getElementById('hidPlanesMovil').value = '';
    document.getElementById('hidPlanesFijo').value = '';
    document.getElementById('hidPlanesInterInalam').value = ''; //PROY-31812 - IDEA-43340
    document.getElementById('hidPlanesBAM').value = '';
    document.getElementById('hidPlanesDTH').value = '';
    document.getElementById('hidPlanesHFC').value = '';
    document.getElementById('hidPlanesVentaVarios').value = '';

    document.getElementById('hidPlanesHFCInalamb').value = '';
    document.getElementById('hidCampanasHFCInalamb').value = '';
    document.getElementById('hidPlazosHFCInalamb').value = '';


    document.getElementById('hidPlanesCombo').value = '';

    cerrarServicio();
    cerrarCuota();
    cerrarEquipo();
    cerrarLineasCuentas(); //PROY-140743
    traerPlazo('S');
    document.getElementById('txtCFTotal').value = '0';
}

function editarFila(idFila, soloHabilitar) {
    //PROY-140223 IDEA-140462
    modoEdicion = 'SI';
    //PROY-140223 IDEA-140462
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var imgEditarFila = document.getElementById('imgEditarFila' + idFila);
    var strTienePaquete = getValue('hidTienePaquete');

    if (imgEditarFila == null)
        return;

    var booEs3Play = false;
    //FTTH - editarFila(idFila, soloHabilitar)
    if ((codigoTipoProducto == codTipoProdFTTH || codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam) && !soloHabilitar) {
        if (!confirm('¿Cambiar de Solución y Paquete? presione Sí, sino se editarán planes y/o servicios y dirección de instalación'))
            booEs3Play = true;
        else {
            eliminarFilaTotal(null, idFila, false);
            agregarFila1(false);

            return;
        }
    }

    if (document.getElementById('hidValidarGuardarCuota').value.length > 0) {
        alert('Debe guardar las cuotas antes de ejecutar esta acción');
        return;
    }

    var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
    var imgListaEquipo = document.getElementById('imgListaEquipo' + idFila);
    var ddlServicio = document.getElementById('ddlServicio' + idFila);
    var ddlListaPrecio = document.getElementById('ddlListaPrecio' + idFila);
    //gaa20161024
    var ddlFamiliaPlan = document.getElementById('ddlFamiliaPlan' + idFila);
    //fin gaa20161024
    if (imgListaEquipo != null && !booEs3Play) {
        var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
        txtTextoEquipo.disabled = false;
        imgListaEquipo.style.display = '';
    }

    //PROY-140223 IDEA-140462
    var dar = parent.getValue('hidNTienePortabilidadValues');
    //PROY-140223 IDEA-140462

    //INI: PROY-140335 RF1
    if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM)) {
        parent.document.getElementById('tdCarrito').style.display = '';
        parent.document.getElementById('tdConsultaPrevia').style.display = 'none';
        if (parent.getValue('hidEjecucionCPBRMS') == '1') {
            parent.limpiarConsultaPrevia();
        }
        parent.document.getElementById('hidEjecucionCPBRMS').value = '0';
        parent.document.getElementById('btnConsultaPrevia').value = "Consulta Previa";
        //parent.limpiarConsultaPrevia();

    }
    //FIN: PROY-140335 RF1

    if (txtNroTelefono != null) {
        if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM)) {
            if ((parent.document.getElementById('tdCarrito').style.display != 'none')) {
                //PROY-140223 IDEA-140462                      
                if (modoEdicion == 'SI') {
                    setEnabled('txtNroTelefono' + idFila, true, 'clsInputEnabled');
                } else { //PROY-140223 IDEA-140462
                    setEnabled('txtNroTelefono' + idFila, false, 'clsInputDisabled');
                }
            } //PROY-140223 IDEA-140462
            else {
                setEnabled('txtNroTelefono' + idFila, true, 'clsInputEnabled');
            }
        }
        else {
            setEnabled('txtNroTelefono' + idFila, true, 'clsInputEnabled');
        }
    }

    if (ddlPaquete != null) {
        if (ddlPaquete.value.length == 0)
            ddlPlan.disabled = false;
    }
    else {
        if (ddlPlan != null)
            ddlPlan.disabled = false;
    }

    if (strTienePaquete != 'S' && !verificarCombo()) {
        if (ddlCampana != null)
            ddlCampana.disabled = false;
    }

    if (ddlPlan != null)
        ddlPlan.disabled = false;

    if (ddlServicio != null)
        ddlServicio.disabled = false;

    if (ddlListaPrecio != null)
        ddlListaPrecio.disabled = false;

    //Paquete Corporativo
    if ((ddlPaquete != null) && (codigoTipoProducto != codTipoProd3Play)) {
        if (ddlPlan != null)
            ddlPlan.disabled = true;
    }
    if (codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam) {
        ddlCampana.disabled = true;
        ddlPlan.disabled = true;
    }
    //gaa20161024
    if (ddlFamiliaPlan != null)
        if (!(parent.getValue('ddlTipoOperacion') == '25' || parent.getValue('ddlTipoOperacion') == 25))//PROY-140743
        ddlFamiliaPlan.disabled = false;
    //fin gaa20161024
    cerrarServicio();
    cerrarCuota();
    cerrarEquipo();
    cerrarLineasCuentas(); //PROY-140743
    autoSizeProducto();

    if (getValue('hidPlanesCombo').length > 0)
        ddlPlan.disabled = true;
    //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
    if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM)) {
        if ((parent.document.getElementById('tdCarrito').style.display != 'none')) {
            if (parent.document.getElementById('ddlOperadorCedente').disabled == true) {
                parent.document.getElementById('tdCarrito').style.display = '';
                parent.document.getElementById('btnAgregarPlan').disabled = true;

                //CAMBIADO POR EL PROY-140335 RF1
                //parent.document.getElementById('btnConsultaPrevia').disabled = true;
                parent.document.getElementById('btnConsultaPrevia').disabled = false;
                parent.document.getElementById('btnConsultaPrevia').value = "Consulta Previa";
                parent.document.getElementById('hidEjecucionCPBRMS').value = '0';
                estructuraGrillaPortabilidad();
                //CAMBIADO POR EL PROY-140335 RF1
            }
            else {
                //PROY-140223 IDEA-140462
                if (modoEdicion == "SI") {
                    parent.document.getElementById('tdCarrito').style.display = '';
                    parent.document.getElementById('btnAgregarPlan').disabled = false;
                    parent.document.getElementById('btnConsultaPrevia').disabled = false;
                    parent.document.getElementById('btnConsultaPrevia').value = "Consulta Previa";
                    //INI:140335 RF1
                    parent.document.getElementById('hidEjecucionCPBRMS').value = '0';
                    //LimpiarCamposPortabilidad();
                    estructuraGrillaPortabilidad();
                    //FIN:140335 RF1
                } else { //PROY-140223 IDEA-140462
                    parent.document.getElementById('tdCarrito').style.display = 'none';
                    parent.document.getElementById('btnAgregarPlan').disabled = true;
                    parent.document.getElementById('btnConsultaPrevia').disabled = false;
                    parent.document.getElementById('btnConsultaPrevia').value = "Consulta Previa";
                }
            }
        } //PROY-140223 IDEA-140462
        else {
            parent.document.getElementById('tdCarrito').style.display = 'none';
            parent.document.getElementById('btnAgregarPlan').disabled = false;
            parent.document.getElementById('btnConsultaPrevia').disabled = false;
        }
    }
    //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
    modoEdicion = "NO"; //PROY-140223 IDEA-140462
    parent.f_EliminarTemporal(idFila);  //PROY 30748
    LimpiarCamposPortabilidad(); //PROY-140335 RF1
}


//PROY-30748 - INICIO
function obtenerValorRC(fila, indice) {
    var tablaResumen = document.getElementById('tbResumenCompras');
    var filaCarrito = tablaResumen.rows[fila];
    var valorCelda = '';

    //valor de la celda plan filaCarrito.cells[3]
    if (filaCarrito != undefined) {
        //                if (fila - 1 > 0) {
        valorCelda = filaCarrito.cells[indice].innerText;
        //                }

    }

    return valorCelda;
}

//Proy 30748 I
function modificarCondicionVenta(ddl, valor, fila, idFila, strGama) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][modificarCondicionVenta()] ", "Entro en la funcion modificarCondicionVenta()");

    var strPlan; //PROY 30748 F2 FALLA MDE
    for (var i = 0; i < ddl.options.length; i++) {

        if (ddl.options[i].text.toUpperCase() == valor.toUpperCase()) {
            ddl.options[i].selected = true;
            strPlan = ddl.options[i].value; //PROY 30748 F2 FALLA MDE

            PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][modificarCondicionVenta()] strPlan ", strPlan);

            var hidCuotaProa = document.getElementById('hidCuota').value; //PROY 30748 F2 MDE
            cambiarPlan(strPlan, idFila);
            var strPlanCodigo = getValor(document.getElementById('ddlPlan' + idFila).value, 0);
            var valEqui = parent.getValue('hidCadenaDetalle').split('|')[idFila - 1].split(';')[17];
            setValue('hidValorEquipo' + idFila, valEqui);
            setValue('hidListaPrecio' + idFila, strGama);
            var txtEqui = parent.getValue('hidCadenaDetalle').split('|')[idFila - 1].split(';')[18];
            document.getElementById('txtTextoEquipo' + idFila).value = txtEqui;
            var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
            var txtNewEquipoPrecio = strGama.split("_");
            txtEquipoPrecio.value = txtNewEquipoPrecio[0];
            var cuota = document.getElementById('hidStrCuota').value;
            var modalidad = parent.getValue('ddlModalidadVenta');
            //INICIATIVA 920
            if (modalidad == codModalidadPagoEnCuota || modalidad == codModalidadPagoEnCuotaSinCode) {
                document.getElementById('hidCuotaProa').value = hidCuotaProa; //PROY 30748 F2 MDE
                cambiarSelecCuota(cuota);
                guardarCuota();
            }
            quitarImagenEsperando();
        }
    }
    LlenarMontoTopeConsumoIfr(fila, strPlan.split('_')[0]); //PROY 30748 F2 FALLA MDE
}
//30748 Fin

function editarPlanProactivo(ddl, strFilaRC, idFila, strGama) {
    var varPlan = '';
    var ddlPlan = ddl;
    var varPlanProactivo = '';

    if (ddlPlan != null) {
        varPlan = obtenerTextoSeleccionado(ddlPlan);
        varPlanProactivo = obtenerValorRC(strFilaRC, 3);

        if (varPlanProactivo != '' && varPlan != varPlanProactivo) {

            modificarCondicionVenta(ddlPlan, varPlanProactivo, strFilaRC, idFila, strGama);
        }
    }

}
//PROY-30748 - FIN

function preservarFila(idFila) {
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    var ddlServicio = document.getElementById('ddlServicio' + idFila);
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var hidValorEquipo = document.getElementById('hidValorEquipo' + idFila);
    var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
    var txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
    var txtMontoTopeConsumo = document.getElementById('txtMontoTopeConsumo' + idFila); //INC000000676006

    var ddlListaPrecio = document.getElementById('ddlListaPrecio');
    //gaa20161024
    var ddlFamiliaPlan = document.getElementById('ddlFamiliaPlan' + idFila);
    //fin gaa20161024
    //PROY-140245 
    var strCodCasEspColab = '';

    // FIN PROY-140245 
    if (ddlCampana.value.length == 0) {
        if (!ddlCampana.disabled) {
            ddlCampana.focus();
            alert('Debe seleccionar una campaña');
            //PROY-140245 
            var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
            if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
                var strCodProd = getValue('hidCodigoTipoProductoActual');
                actualizarCantidadProductosCasoEspColab(strCodProd);
            }
            // FIN PROY-140245 
            return false;
        }
    }

    if (ddlPlazo != null) {
        if (ddlPlazo.value.length == 0) {
            if (!ddlPlan.disabled) {
                ddlPlazo.focus();
                alert('Debe seleccionar un plazo');
                //PROY-140245 
                var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
                if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
                    var strCodProd = getValue('hidCodigoTipoProductoActual');
                    actualizarCantidadProductosCasoEspColab(strCodProd);
                }
                // FIN PROY-140245 
                return false;
            }
        }
    }
    //gaa20161024
    if (ddlFamiliaPlan != null) {
        if (ddlFamiliaPlan.value.length == 0) {
            if (!ddlFamiliaPlan.disabled) {
                ddlFamiliaPlan.focus();
                alert('Debe seleccionar una familia de plan');
                //PROY-140245 
                var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
                if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
                    var strCodProd = getValue('hidCodigoTipoProductoActual');
                    actualizarCantidadProductosCasoEspColab(strCodProd);
                }
                // FIN PROY-140245 
                return false;
            }
        }
    }
    //fin gaa20161024
    if (ddlPlan != null) {
        if (ddlPlan.value.length == 0) {
            if (!ddlPlan.disabled) {
                ddlPlan.focus();
                alert('Debe seleccionar un plan');
                //PROY-140245 
                var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
                if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
                    var strCodProd = getValue('hidCodigoTipoProductoActual');
                    actualizarCantidadProductosCasoEspColab(strCodProd);
                }
                // FIN PROY-140245 
                return false;
            }
        }
    }

    // INC000000676006
    var varFamiliaPlan = '';

    if (ddlFamiliaPlan != null) {
        varFamiliaPlan = obtenerTextoSeleccionado(ddlFamiliaPlan);
        if (varFamiliaPlan == GvarFamiliaPlan) {
            if (txtMontoTopeConsumo.value == '') {
                alert('Seleccione e ingrese parametros para Servicio Roaming Internacional');

                //PROY-140245 
                var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
                if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
                    var strCodProd = getValue('hidCodigoTipoProductoActual');
                    actualizarCantidadProductosCasoEspColab(strCodProd);
                }
                // FIN PROY-140245 

                return false;
            }
        } // FIN
    }

    //FTTH -preservarFila(idFila) - getValue('hidCodigoTipoProductoActual') != codTipoProdFTTH
    if (getValue('hidCodigoTipoProductoActual') != codTipoProdFTTH && getValue('hidCodigoTipoProductoActual') != codTipoProd3Play && getValue('hidCodigoTipoProductoActual') != codTipoProd3PlayInalam) {
        //INICIATIVA 920 PLAZOS
        var plazo = ddlPlazo.value.split('-')[0];

        var listaPlazoEquipo = constPlazoConEquipo;
        if (parent.getValue('ddlModalidadVenta') != codModalidadChipSuelto) {
            if (listaPlazoEquipo.indexOf(plazo) > 0 || parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota ||
                parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode) {//INICIATIVA 920
                if (hidValorEquipo.value.length == 0) {
                    alert('Debe seleccionar un equipo');
                    //PROY-140245 
                    var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
                    if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
                        var strCodProd = getValue('hidCodigoTipoProductoActual');
                        actualizarCantidadProductosCasoEspColab(strCodProd);
                    }
                    // FIN PROY-140245 
                    return false;
                }
            }
        }
    }

    if (ddlServicio != null) {
        if (ddlServicio.value.length == 0) {
            if (!ddlServicio.disabled) {
                ddlServicio.focus();
                alert('Debe seleccionar un servicio');
                //PROY-140245 
                var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
                if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
                    var strCodProd = getValue('hidCodigoTipoProductoActual');
                    actualizarCantidadProductosCasoEspColab(strCodProd);
                }
                // FIN PROY-140245 
                return false;
            }
        }
    }

    if (ddlListaPrecio != null) {
        if (ddlListaPrecio.value.length == 0) {
            if (!ddlListaPrecio.disabled) {
                ddlListaPrecio.focus();
                alert('Debe seleccionar una lista de precio');
                //PROY-140245 
                var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
                if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
                    var strCodProd = getValue('hidCodigoTipoProductoActual');
                    actualizarCantidadProductosCasoEspColab(strCodProd);
                }
                // FIN PROY-140245 
                return false;
            }
        }
    }

    if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota || parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode) {//INICIATIVA 920
        var arrCuota = obtenerCuotaValores(idFila);
        if (arrCuota == null) {
            alert('Debe seleccionar la cuota');
            //PROY-140245 
            var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
            if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
                var strCodProd = getValue('hidCodigoTipoProductoActual');
                actualizarCantidadProductosCasoEspColab(strCodProd);
            }
            // FIN PROY-140245 
            return false;
        }
    }

    if (document.getElementById('tbTopeConsumosLTE').style.display != 'none') {
        alert('Debe guardar el Monto del tope');
        //PROY-140245 
        var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
        if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
            var strCodProd = getValue('hidCodigoTipoProductoActual');
            actualizarCantidadProductosCasoEspColab(strCodProd);
        }
        // FIN PROY-140245 
        return false;
    } //PROY-29296
    if (document.getElementById('tblServicios').style.display != 'none') {
        alert('Debe guardar los servicios');
        //PROY-140245 
        var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
        if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
            var strCodProd = getValue('hidCodigoTipoProductoActual');
            actualizarCantidadProductosCasoEspColab(strCodProd);
        }
        // FIN PROY-140245 
        return false;
    }

    if (document.getElementById('tblFormnCuotas').style.display != 'none') {
        alert('Debe guardar las cuotas');
        //PROY-140245 
        var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
        if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
            var strCodProd = getValue('hidCodigoTipoProductoActual');
            actualizarCantidadProductosCasoEspColab(strCodProd);
        }
        // FIN PROY-140245 
        return false;
    }

    if (document.getElementById('tblEquipos').style.display != 'none') {
        alert('Debe guardar los equipos');
        //PROY-140245 
        var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
        if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
            var strCodProd = getValue('hidCodigoTipoProductoActual');
            actualizarCantidadProductosCasoEspColab(strCodProd);
        }
        // FIN PROY-140245 
        return false;
    }

    if (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion || parent.getValue('hidNTienePortabilidadValues') == 'S') {

        var nroTelefonoValido = 9;
        if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProd3Play || getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam)
            nroTelefonoValido = 8;

        var strTxtNroTelefono = txtNroTelefono.value.trim(); //INC000004030766

        //if (txtNroTelefono != null) { //INC000004030766
        if (strTxtNroTelefono != null) { //INC000004030766
            //if (txtNroTelefono.value.length != nroTelefonoValido) { //INC000004030766
            if (strTxtNroTelefono.length != nroTelefonoValido) { //INC000004030766
                if (!txtNroTelefono.disabled && txtNroTelefono.style.display != 'none') {
                    txtNroTelefono.focus();
                    alert('Debe ingresar un número de teléfono válido');
                    //PROY-140245 
                    var strCodCasEspColab = parent.getValue('hidcasosEspecialesColaborador');
                    if (getValor(parent.getValue('hidcasoEspecial'), 0) != '' && parent.getValue('hidcasoEspecial') != null && strCodCasEspColab.search(getValor(parent.getValue('hidcasoEspecial'), 0)) > 0 && parent.getValue('hidFlagAgregarCarrito') != 'S') {
                        var strCodProd = getValue('hidCodigoTipoProductoActual');
                        //var strCodProd = parent.getValue('hidCodigoTipoProductoActual');
                        actualizarCantidadProductosCasoEspColab(strCodProd);
                    }
                    // FIN PROY-140245 
                    return false;
                }
            }
            else //INI INC000004030766
            {
                if (strTxtNroTelefono.length == 9) {
                    if (strTxtNroTelefono.slice(0, 1) != '9') {
                        alert('Debe ingresar un número de teléfono válido.');
                        return false;
                    }
                }
            }
            //FIN INC000004030766
        }

    }

    preservarFila1(idFila);

    return true;
}

function preservarFila1(idFila) {
    var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var ddlServicio = document.getElementById('ddlServicio' + idFila);
    var ddlTopeConsumo = document.getElementById('ddlTopeConsumo' + idFila);
    var ddlListaPrecio = document.getElementById('ddlListaPrecio' + idFila);
    var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
    var hidPlazoActual = document.getElementById('hidPlazoActual');
    var txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
    //gaa20161024
    var ddlFamiliaPlan = document.getElementById('ddlFamiliaPlan' + idFila);
    //fin gaa20161024
    if (txtTextoEquipo != null) {
        txtTextoEquipo.disabled = true;

        var imgListaEquipo = document.getElementById('imgListaEquipo' + idFila);
        if (imgListaEquipo != null)
            imgListaEquipo.style.display = 'none';
    }

    if (txtNroTelefono != null)
        setEnabled('txtNroTelefono' + idFila, false, 'clsInputDisabled');

    if (ddlPaquete != null)
        ddlPaquete.disabled = true;

    if (ddlPlan != null)
        ddlPlan.disabled = true;

    if (ddlPlazo != null)
        ddlPlazo.disabled = true;

    if (ddlCampana != null)
        ddlCampana.disabled = true;

    if (ddlServicio != null)
        ddlServicio.disabled = true;

    if (ddlTopeConsumo != null)
        ddlTopeConsumo.disabled = true;

    if (ddlListaPrecio != null)
        ddlListaPrecio.disabled = true;
    //gaa20161024
    if (ddlFamiliaPlan != null)
        ddlFamiliaPlan.disabled = true;
    //fin gaa20161024
    cerrarServicio();
    cerrarCuota();
    cerrarLineasCuentas(); //PROY-140743
    cerrarEquipo();

    //INICIATIVA 920
    if (ddlPlazo != null)
        hidPlazoActual.value = ddlPlazo.value.split('-')[0] + "_" + obtenerTextoSeleccionado(ddlPlazo);

    var strNroCuotas = document.getElementById('ddlNroCuotas').value;
    var hidNroCuotaActual = document.getElementById('hidNroCuotaActual');

    if (strNroCuotas.length > 0) {
        if (parseInt(strNroCuotas.substring(0, 2)) > 0)
            hidNroCuotaActual.value = strNroCuotas;
    }
}

function inicializarPlan(idFila) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][inicializarPlan()] ", "Entro en la funcion inicializarPlan()");

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][inicializarPlan()] idFila ", idFila);

    borrarServicio(idFila);
    cerrarServicio();

    inicializarEquipo(idFila);
    borrarEquipo(idFila);
    cerrarEquipo();

    borrarCuota(idFila)
    cerrarCuota();
    cerrarLineasCuentas(); //PROY-140743

    var txtCFPlanServicio = document.getElementById('txtCFPlanServicio' + idFila);
    var txtMontoTopeConsumo = document.getElementById('txtMontoTopeConsumo' + idFila);
    var txtCFMenAlqKit = document.getElementById('txtCFMenAlqKit' + idFila);
    var txtCFTotMensual = document.getElementById('txtCFTotMensual' + idFila);
    var hidMontoServicios = document.getElementById('hidMontoServicios' + idFila);
    var hidMontoCuota = document.getElementById('hidMontoCuota' + idFila);

    if (txtCFPlanServicio != null)
        txtCFPlanServicio.value = '';

    if (txtMontoTopeConsumo != null)
        txtMontoTopeConsumo.value = '';

    if (txtCFMenAlqKit != null)
        txtCFMenAlqKit.value = '';

    if (txtCFTotMensual != null)
        txtCFTotMensual.value = '';

    if (hidMontoServicios != null)
        hidMontoServicios.value = '0';

    if (hidMontoCuota != null)
        hidMontoCuota.value = '0';
}

function mostrarPopupPlanCombo(strCodPlan, strDesPlan) {
    var opciones = "dialogHeight: 250px; dialogWidth: 350px; edge: Raised; center:Yes; help: No; resizable=no; status: No";
    var url = '../consultas/sisact_pop_plan_combo.aspx?idPlanBase=' + strCodPlan + '&planBase=' + strDesPlan;

    return window.showModalDialog(url, '', opciones);
}

function cambiarPlan(strPlan, idFila) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][cambiarPlan()] ", "Entro en la funcion cambiarPlan()");

    document.getElementById('hidCuotaProa').value = ''; //PROY 30748 F2 MDE
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    //PROY-140618-INI
    var strFamiliaPlan = getText('ddlFamiliaPlan' + idFila);
    setValue('hidFamiliaPlan', strFamiliaPlan);
    //PROY-140618-FIN
    inicializarPlan(idFila);
    if (strPlan.length == 0) {
        document.getElementById('hidLineaActual').value = idFila;

        calcularLCxProductoFijo();
        calcularCFxProducto();

        llenarDatosCombo1(document.getElementById('divListaEquipo' + idFila), '', idFila, 'Equipo', false);
        if (codigoTipoProductoActual == '05' && codigoTipoProductoActual == '08') {
            document.getElementById('ddlTopeConsumo' + idFila).selectedIndex = 2;
        } //PROY-29296
        return;
    }
    //FTTH -cambiarPlan(strPlan, idFila) -codigoTipoProductoActual == codTipoProdFTTH 
    if (codigoTipoProductoActual == codTipoProdFTTH || codigoTipoProductoActual == codTipoProd3Play || codigoTipoProductoActual == codTipoProd3PlayInalam) {
        LlenarServicioHFCIfr(idFila, strPlan, codigoTipoProductoActual);
        if (codigoTipoProductoActual == '05' && codigoTipoProductoActual == '08') {
            document.getElementById('ddlTopeConsumo' + idFila).selectedIndex = 2;
        }  //PROY-29296
        return;
    }

    var strPlanCodigo = getValor(strPlan, 0);
    var strPlanCodigoSap = getValor(strPlan, 2);
    var strPlanCF = getValor(strPlan, 1);
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var strCampana = ddlCampana.value;
    var strPlazo = ddlPlazo.value.split('-')[0];//INICIATIVA 920
    //PROY-140743-INI
    var strPromocion = '';
    var codOperacion = parent.getValue('ddlTipoOperacion');
    if (codOperacion == '25') {
        strPromocion = document.getElementById('ddlPromocion' + idFila).value;
    }
    //PROY-140743-FIN



    var strPlanesBolsa = window.parent.document.getElementById('hidPlanBase').value;
    var ddlPlan = document.getElementById('ddlPlan' + idFila);

    if (strPlanesBolsa.indexOf('|' + strPlanCodigo) > -1) {
        var datosBolsa = mostrarPopupPlanCombo(strPlanCodigo, obtenerTextoSeleccionado(ddlPlan));

        if (datosBolsa != undefined) {
            if (datosBolsa.length > 0) {
                var strPlanActualizado = datosBolsa.split('|')[0];

                PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][cambiarPlan()] datosBolsa ", datosBolsa);

                llenarDatosCombo(ddlPlan, '|' + strPlanActualizado, false);
                ddlPlan.value = strPlanActualizado.split(';')[0];
                cambiarPlan(strPlanActualizado, idFila);
                ddlPlan.disabled = true;
                return;
            }
        }
    }

    document.getElementById('hidLineaActual').value = idFila;
    document.getElementById('txtCFPlanServicio' + idFila).value = strPlanCF;
    document.getElementById('btnCerrarServicios').value = 'Cerrar y Guardar';

    calcularCFxProducto();

    if (codigoTipoProductoActual != codTipoProdDTH) {
        LlenarServicioMaterialIfr(idFila, strPlanCodigo, strPlanCodigoSap, strCampana, strPlazo, strPromocion); //PROY-140743-INI
    } else {
        LlenarServicioKitIfr(idFila, strPlanCodigo, strPlanCodigoSap, strCampana, strPlazo);
    }
    //gaa20151123
    if (verificarPlanesAdicionales()) {
        if (ddlPlan.disabled)
            return;

        if (confirm('¿Desea agregar otro plan?')) {
            ddlPlan.disabled = true;
            agregarFila(false, idFila, true);
            idFila++;

            ddlPlazo.disabled = true;

            ddlCampana = document.getElementById('ddlCampana' + idFila);
            ddlPlazo = document.getElementById('ddlPlazo' + idFila);
            ddlPlan = document.getElementById('ddlPlan' + idFila);
            var strPlanes;

            switch (codigoTipoProductoActual) {
                case codTipoProdMovil:
                    llenarDatosCombo(ddlCampana, getValue('hidCampanasMovil'), false);
                    llenarDatosCombo(ddlPlazo, getValue('hidPlazosMovil'), false);
                    strPlanes = obtenerPlanesSeleccionables(getValue('hidPlanesMovil'));
                    llenarDatosCombo(ddlPlan, strPlanes, true);
                    break;
                case codTipoProdFijo:
                    llenarDatosCombo(ddlCampana, getValue('hidCampanasFijo'), false);
                    llenarDatosCombo(ddlPlazo, getValue('hidPlazosFijo'), false);
                    strPlanes = obtenerPlanesSeleccionables(getValue('hidPlanesFijo'));
                    llenarDatosCombo(ddlPlan, strPlanes, true);
                    break;
                //PROY-31812 - IDEA-43340 - INICIO   
                case codTipoProdInterInalam:
                    llenarDatosCombo(ddlCampana, getValue('hidCampanasInterInalam'), false);
                    llenarDatosCombo(ddlPlazo, getValue('hidPlazosInterInalam'), false);
                    strPlanes = obtenerPlanesSeleccionables(getValue('hidPlanesInterInalam'));
                    llenarDatosCombo(ddlPlan, strPlanes, true);
                    break; false
                    //PROY-31812 - IDEA-43340 - FIN 
                case codTipoProdBAM:
                    llenarDatosCombo(ddlCampana, getValue('hidCampanasBAM'), false);
                    llenarDatosCombo(ddlPlazo, getValue('hidPlazosBAM'), false)
                    strPlanes = obtenerPlanesSeleccionables(getValue('hidPlanesBAM'));
                    llenarDatosCombo(ddlPlan, strPlanes, true);
                    break; false
                case codTipoProdDTH:
                    llenarDatosCombo(ddlCampana, getValue('hidCampanasDTH'), false);
                    llenarDatosCombo(ddlPlazo, getValue('hidPlazosDTH'), false);
                    strPlanes = obtenerPlanesSeleccionables(getValue('hidPlanesDTH'));
                    llenarDatosCombo(ddlPlan, strPlanes, true);
                    break;
                case codTipoProd3Play:
                    llenarDatosCombo(ddlCampana, getValue('hidCampanasHFC'), false);
                    llenarDatosCombo(ddlPlazo, getValue('hidPlazosHFC'), false);
                    strPlanes = obtenerPlanesSeleccionables(getValue('hidPlanesHFC'));
                    llenarDatosCombo(ddlPlan, strPlanes, true);
                    break;
                // FTTH -inicio- llenarDatosCombo(ddlCampana -ddlPlazo    
                case codTipoProdFTTH:
                    llenarDatosCombo(ddlCampana, getValue('hidCampanasFTTH'), false);
                    llenarDatosCombo(ddlPlazo, getValue('hidPlazosFTTH'), false);
                    strPlanes = obtenerPlanesSeleccionables(getValue('hidPlanesFTTH'));
                    llenarDatosCombo(ddlPlan, strPlanes, true);
                    break;
                // FTTH -Fin- llenarDatosCombo(ddlCampana -ddlPlazo   
                case codTipoProdVentaVarios:
                    llenarDatosCombo(ddlCampana, getValue('hidCampanasVentaVarios'), false);
                    llenarDatosCombo(ddlPlazo, getValue('hidPlazosVentaVarios'), false);
                    strPlanes = obtenerPlanesSeleccionables(getValue('hidPlanesVentaVarios'));
                    llenarDatosCombo(ddlPlan, strPlanes, true);
                    break;
            }

            ddlCampana.value = strCampana;
            ddlPlazo.value = strPlazo;
            ddlCampana.disabled = true;
            ddlPlazo.disabled = true;
        }
    }
    if (codigoTipoProductoActual == '05' && codigoTipoProductoActual == '08') {
        document.getElementById('ddlTopeConsumo' + idFila).selectedIndex = 2;
    } //PROY-29296
    //fin gaa20151123
}
//gaa20151123
function obtenerPlanesSeleccionables(strPlanes) {
    var arrPlan = strPlanes.split('|');
    var cont = arrPlan.length;

    var desTipoProductoActual = getValue('hidTipoProductoActual');
    var tblTabla = document.all('tblTabla' + desTipoProductoActual);
    var contTabla = tblTabla.rows.length;
    var idFila;
    var strPlan;

    for (var x = 0; x < contTabla; x++) {
        fila = tblTabla.rows[x];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
        strPlanActual = document.getElementById('ddlPlan' + idFila).value;

        if (strPlanActual.length > 0) {
            for (var i = 0; i < cont; i++) {
                if (arrPlan[i].indexOf(strPlanActual) > -1) {
                    strPlanes = strPlanes.replace('|' + arrPlan[i], '');
                    break;
                }
            }
        }
    }

    return strPlanes;
}

function verificarPlanesAdicionales() {
    var booResultado = false;

    if (getValue('hidPlanesCombo').length > 0) {
        var desTipoProductoActual = getValue('hidTipoProductoActual');
        var tblTabla = document.all('tblTabla' + desTipoProductoActual);
        var cont = tblTabla.rows.length;

        var contPlanes = getValue('hidPlanes' + desTipoProductoActual).split('|').length;
        if (contPlanes > 0)
            contPlanes--;

        if (contPlanes > cont)
            booResultado = true;
    }

    return booResultado;
}
//fin gaa20151123

function LlenarServicioHFCIfr(idFila, strPlan, codigoTipoProductoActual) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila + '&strPlan=' + strPlan;
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strMetodo=' + 'LlenarServicioHFC';
    url += '&strTipoProducto=' + codigoTipoProductoActual;
    self.frames['iframeAuxiliar'].location.replace(url);
}

function LlenarServicioMaterialIfr(idFila, strPlan, pstrPlanCodigoSap, strCampana, strPlazo,strPromocion) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila
    url += '&strCasoEspecial=' + getValor(parent.getValue('ddlCasoEspecial'), 0);
    url += '&strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
    url += '&strPlan=' + strPlan;
    url += '&strPlanSap=' + pstrPlanCodigoSap;
    url += '&strFlagServicioRI=' + parent.getValue('hidFlagRoamingI');
    url += '&strCampana=' + strCampana;
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&nroDoc=' + parent.getValue('txtNroDoc');
    url += '&strOrgVenta=' + parent.getValue('hidOrgVenta');
    url += '&strCentro=' + parent.getValue('hidCentro');
    url += '&strOferta=' + parent.getValue('ddlOferta');
    url += '&strCombo=' + parent.getValue('ddlCombo');
    url += '&strPlazo=' + strPlazo;
    url += '&strModalidadVenta=' + parent.getValue('ddlModalidadVenta');
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strTipoOficina=' + parent.getValue('ddlCanal');
    url += '&strTipoDocumento=' + parent.getValue('ddlTipoDocumento');
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strPromocion=' + strPromocion; //PROY-140743-INI
    url += '&strMetodo=' + 'LlenarServicioMaterial';

    self.frames['iframeAuxiliar'].location.replace(url);
}

function LlenarServicioKitIfr(idFila, strPlan, pstrPlanCodigoSap, strCampana, strPlazo) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila
    url += '&strCasoEspecial=' + getValor(parent.getValue('ddlCasoEspecial'), 0);
    url += '&strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
    url += '&strPlan=' + strPlan;
    url += '&strPlanSap=' + pstrPlanCodigoSap;
    url += '&strFlagServicioRI=' + parent.getValue('hidFlagRoamingI');
    url += '&strCampana=' + strCampana;
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&nroDoc=' + parent.getValue('txtNroDoc');
    url += '&strOrgVenta=' + parent.getValue('hidOrgVenta');
    url += '&strCentro=' + parent.getValue('hidCentro');
    url += '&strOferta=' + parent.getValue('ddlOferta');
    url += '&strCombo=' + parent.getValue('ddlCombo');
    url += '&strPlazo=' + strPlazo;

    url += '&strMetodo=' + 'LlenarServicioKit';

    self.frames['iframeAuxiliar'].location.replace(url);
}

function asignarServicioKit(idFila, pstrResultado) {
    var arrResultado;
    var strServicios;

    if (pstrResultado.indexOf('~') > -1) {
        arrResultado = pstrResultado.split('~');

        strServicios = arrResultado[0];

        asignarKits(idFila, arrResultado[1]);

        if (strServicios.indexOf('¬') > -1) {
            arrResultado = strServicios.split('¬');
            var lbxServiciosDisponibles1 = document.getElementById('lbxServiciosDisponibles1');
            var lbxServiciosAgregados1 = document.getElementById('lbxServiciosAgregados1');

            llenarDatosCombo(lbxServiciosDisponibles1, arrResultado[0], false);
            llenarDatosCombo(lbxServiciosAgregados1, arrResultado[1], false);
                    //PROY-140383-INI
                    if (getValue('hidFlagServvExcluyentes') == '1') {
                        if (getValue('hidCodigoTipoProductoActual') != codTipoProdDTH) {
                            agregarGrupo(idFila, true);
                        }

                    } else {
            agregarGrupo(idFila, true);
                    }
                    //PROY-140383-FIN
            guardarServicio();
        }
        else
            calcularCFxProducto();

        calcularLCxProductoFijo();
    }
}

function asignarServicioMaterial(idFila, pstrResultado) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][asignarServicioMaterial()]", "Entro a la funcion asignarServicioMaterial()");

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][asignarServicioMaterial()] pstrResultado", pstrResultado);

    var arrResultado;
    var strServicios;
            //PROY-140383-INI
            var CodProductosExc = getValue('hidProductosExc'); 
            var strCodTipoProductoActual = getValue('hidCodigoTipoProductoActual');
            //PROY-140383-FIN
    if (pstrResultado.indexOf('~') > -1) {
        arrResultado = pstrResultado.split('~');

        strServicios = arrResultado[0];

        llenarMaterial(idFila, arrResultado[1]);
        asignarMaterial(idFila);

        if (strServicios.indexOf('¬') > -1) {
            arrResultado = strServicios.split('¬');
            var lbxServiciosDisponibles1 = document.getElementById('lbxServiciosDisponibles1');
            var lbxServiciosAgregados1 = document.getElementById('lbxServiciosAgregados1');

            llenarDatosCombo(lbxServiciosDisponibles1, arrResultado[0], false);
            llenarDatosCombo(lbxServiciosAgregados1, arrResultado[1], false);

                    //PROY-140383-INI
                    if (getValue('hidFlagServvExcluyentes') == '1') {
                        if (CodProductosExc.indexOf(strCodTipoProductoActual) == -1) {
            agregarGrupo(idFila, true);
                        }
                    } else {
                       agregarGrupo(idFila, true);
                    }
                    //PROY-140383-FIN
            //Agregar Servicio Adicional EMMH I
            var strBoolProactivaAdicional = getValue('hidProactiva');

            PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][asignarServicioMaterial()] getValue('hidProactiva')", getValue('hidProactiva'));
            
            if (strBoolProactivaAdicional != '') {

                PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][asignarServicioMaterial()] parent.getValue('hidNuevoServicio')", parent.getValue('hidNuevoServicio'));
                
                var strNuevoServicio = parent.getValue('hidNuevoServicio');
                strNuevoServicio = strNuevoServicio.replace(" ", "_"); //PROY 30748 F2 MDE
                var arrNuevoServicio = strNuevoServicio.split('_');
                for (var i = 0; i < arrNuevoServicio.length; i++) {
                    agregarServicio(arrNuevoServicio[i].trim());
                    i = i + 1;
                }
                strBoolProactivaAdicional = '';

                PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][asignarServicioMaterial()] parent.document.getElementById('hidNuevoServicio').value antes", parent.document.getElementById('hidNuevoServicio').value);

                parent.document.getElementById('hidNuevoServicio').value = strBoolProactivaAdicional; //PROY 30748 F2 MDE

                PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][asignarServicioMaterial()] parent.document.getElementById('hidNuevoServicio').value despues", parent.document.getElementById('hidNuevoServicio').value);                
            }
            //EMMH F
            guardarServicio('d'); //INC000002245048
        }
        else
            calcularCFxProducto();

        calcularLCxProductoFijo();
    }


    //PROY-30748 INICIO
    var strBoolProactiva = getValue('hidProactiva');

    if (strBoolProactiva != '') {
        strBoolProactiva = '';
        setValue('hidProactiva', strBoolProactiva);

        var stridEquipo = getValue('hidEquipoN');
        var divCP = document.createElement("div");

        divCP.id = stridEquipo; //"0_" + valItemEquipo + "_" + varEquipo;
        seleccionarItem('hidValor', 'txtTexto', 'divLista', divCP, idFila, 'Equipo');
        quitarImagenEsperando();
        autoSizeIframe();

        setValue('hidEPSelect', '');
        setValue('hidContRC ', '');

        //EMMH i exp
        var strValorEquipo = getValue('txtTextoEquipo' + idFila);
        var cuota = document.getElementById('hidStrCuota').value;
        var strGama = getValue('hidPEquipoGama');
        var modalidad = parent.getValue('ddlModalidadVenta');
        //PROY 30748 F2 INI EMMH
        var newCodequipo = strGama.split('_');
        setValue('hidValorEquipo' + idFila, newCodequipo[1]);
        //PROY 30748 F2 FIN EMMH
        setValue('hidListaPrecio' + idFila, strGama);

        if (modalidad == codModalidadPagoEnCuota || modalidad==codModalidadPagoEnCuotaSinCode) {//INICIATIVA 920
            cambiarSelecCuota(cuota);
            guardarCuota();
        }

    }
    //PROY-30748 FIN

}

function LlenarServCampCorpTot(strPlanes) {
    cargarImagenEsperando();

    var strCuota = '';
    if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota
       || parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode) {//INICIATIVA 920
        var arrCuota = obtenerCuotaValores(idFila);
        if (arrCuota != null) {
            strCuota = arrCuota[0].split('_')[0];
        }
    }

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'strTipoDocumento=' + parent.getValue('ddlTipoDocumento');
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&strCasoEspecial=' + getValor(parent.getValue('ddlCasoEspecial'), 0);
    url += '&strRiesgo=' + parent.getValue('hidnRiesgoDCValue');
    url += '&strPlanes=' + strPlanes;
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strOrgVenta=' + parent.getValue('hidOrgVenta');
    url += '&strCentro=' + parent.getValue('hidCentro');
    url += '&strModalidadVenta=' + parent.getValue('ddlModalidadVenta');
    url += '&strTipoOficina=' + parent.getValue('ddlCanal');
    url += '&strCuota=' + strCuota;
    url += '&strMetodo=' + 'LlenarServCampCorpTot';
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    self.frames['iframeAuxiliar'].location.replace(url);
}

function retornarServCampCorpTot(pstrResultado) {
    var arrResultado;
    var arrResultado1;
    var j = 0;

    if (pstrResultado.indexOf('°') > -1) {
        arrResultado = pstrResultado.split('°');

        var total = arrResultado.length;

        for (j = 0; j < total; j++) {
            arrResultado1 = arrResultado[j].split('!');

            if (arrResultado1 != null && arrResultado1 != undefined) {
                if (arrResultado1[1] != null && arrResultado1[1] != undefined)
                    retornarServicioCampanaCorp(arrResultado1[0], arrResultado1[1]);
            }
        }
    }
}

function retornarServicioCampanaCorp(idFila, pstrResultado) {
    var arrResultado;
    var strServicios;

    asignarLineaActual(idFila);

    if (pstrResultado.indexOf('~') > -1) {
        arrResultado = pstrResultado.split('~');

        //llenarCampana(-1, arrResultado[0]);
        llenarMaterial(idFila, arrResultado[0]);
        asignarMaterial(idFila);

        strServicios = arrResultado[1];

        if (strServicios.indexOf('¬') > -1) {
            arrResultado = strServicios.split('¬');
            var lbxServiciosDisponibles1 = document.getElementById('lbxServiciosDisponibles1');
            var lbxServiciosAgregados1 = document.getElementById('lbxServiciosAgregados1');

            llenarDatosCombo(lbxServiciosDisponibles1, arrResultado[0], false);
            llenarDatosCombo(lbxServiciosAgregados1, arrResultado[1], false);

            agregarGrupo(-1, true);
            guardarServicio();
        }
    }
}

function asignarPlanMultiLinea(idFila, strValor, strPaquete) {
    var strTipoProducto = getValue('hidCodigoTipoProductoActual');
    var ddlTipoProducto;
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    var strPlazo = ddlPlazo.value.split('-')[0] + "_" + obtenerTextoSeleccionado(ddlPlazo);//INICIATIVA 920
    var strPlazos = '|' + ddlPlazo.value.split('-')[0] + ";" + obtenerTextoSeleccionado(ddlPlazo);//INICIATIVA 920
    var strPlazoCodigo = ddlPlazo.value.split('-')[0];//INICIATIVA 920
    var hidPlazoActual = document.getElementById('hidPlazoActual');
    var hidPaqueteActual = document.getElementById('hidPaqueteActual');
    var hidNGrupoPaqueteValues = document.getElementById('hidNGrupoPaqueteValues');
    var txtCFPlanServicio;
    var strPlan;
    var strPlanCodigo;
    var strPlanCF;
    var strSecuencia;
    var strPlanCodigoSap;
    var strPlanProducto;
    idFila = parseInt(idFila);

    if (strValor == null)
        return;

    var arrPlanes = strValor.split("|");
    var arrPlan;
    var ddlPlan;
    var ddlPaquete;
    var strPlanes = '';

    hidPlazoActual.value = strPlazo;

    hidNGrupoPaqueteValues.value += '{'

    for (var i = 1; i < arrPlanes.length; i++) {
        if (i > 1) {
            agregarFila(false, 0, false);
            idFila += 1;
        }

        ddlPlan = document.getElementById('ddlPlan' + idFila);
        llenarDatosCombo(ddlPlan, strValor, true);
        arrPlan = arrPlanes[i].split(";");
        strPlan = arrPlan[0];
        ddlPlan.value = strPlan;
        strPlanCodigo = getValor(strPlan, 0);
        strPlanCF = getValor(strPlan, 1);
        strSecuencia = getValor(strPlan, 4);
        strPlanCodigoSap = getValor(strPlan, 2);
        strPlanProducto = getValor(strPlan, 8);

        txtCFPlanServicio = document.getElementById('txtCFPlanServicio' + idFila);
        txtCFPlanServicio.value = strPlanCF;

        ddlPaquete = document.getElementById('ddlPaquete' + idFila);
        ddlPlazo = document.getElementById('ddlPlazo' + idFila);

        llenarDatosCombo(ddlPlazo, strPlazos, false);

        ddlPlazo.disabled = true;
        ddlPaquete.disabled = true;
        ddlPlan.disabled = true;
        //gaa20140730
        var ddlCampana = document.getElementById('ddlCampana' + idFila);

        if (ddlCampana != null) {
            if (ddlCampana.value.length > 0)
                setValue('hidCampana', '|' + ddlCampana.value + ';' + obtenerTextoSeleccionado(ddlCampana));
            else
                llenarDatosCombo(ddlCampana, getValue('hidCampana'), false);

            ddlCampana.disabled = true;
        }
        //fin gaa20140730

        //strPlanes += '|' + strPlanCodigo + ',' + strPlazoCodigo + ',' + idFila + ',' + strPlanCodigoSap + ',' + strPaquete + ',' + strSecuencia;
        strPlanes += '|' + strPlanCodigo + ',' + strPlazoCodigo + ',' + idFila + ',' + strPlanCodigoSap + ',' + strPaquete + ',' + strSecuencia + ',' + ddlCampana.value + ',' + strPlanProducto;
        hidNGrupoPaqueteValues.value += ',[' + idFila + ']';
        llenarMaterial(idFila, '');
        //LlenarMaterialIfr(idFila, ddlCampana.value, strPlanCodigo);
        asignarMaterial(idFila);
    }

    LlenarServCampCorpTot(strPlanes);

    hidNGrupoPaqueteValues.value += '}'

    quitarImagenEsperando();
}

function seleccionarValorCombo(ddl, valor) {
    for (var i = 0; i < ddl.options.length; i++) {
        if (ddl.options[i].value == valor) {
            ddl.value = valor;
            return true;
        }
    }

    return false;
}

function asignarPlan3Play(idFila, strValor) {
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    llenarDatosCombo(ddlPlan, strValor, true);
}

function asignarServicioMultiLinea3Play(idFila, strValor) {
    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam && parent.getValue('hidTipoOperacion') == "2") {
        aServicioMultiLinea3PlayMigra(idFila, strValor);
    } else {
        aServicioMultiLinea3Play(idFila, strValor);
    }
}

function aServicioMultiLinea3Play(idFila, strValor) {
    if (strValor == '' || strValor == null) return;

    cargarImagenEsperando();

    //var arrServProm = strValor.split('¬');
    var strTipoProducto = getValue('hidCodigoTipoProductoActual');
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var strCampanaActual = '|' + ddlCampana.value + ";" + obtenerTextoSeleccionado(ddlCampana);
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    var strPlazo = '|' + ddlPlazo.value.split('-')[0] + ";" + obtenerTextoSeleccionado(ddlPlazo);//INICIATIVA 920
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var strPlanActual = '|' + ddlPlan.value + ";" + obtenerTextoSeleccionado(ddlPlan);
    var hidPlazoActual = document.getElementById('hidPlazoActual');
    var hidPaqueteActual = document.getElementById('hidPaqueteActual');
    var hidNGrupoPaqueteValues = document.getElementById('hidNGrupoPaqueteValues');
    var idFilaIni = idFila;
    idFila = parseInt(idFila);
    hidPlazoActual.value = strPlazo;
    var arrPlanes = strValor.split('|');
    var arrPlanCodigo;
    var cont = arrPlanes.length;
    var strPlanesCombo = '';
    var strPlan;
    var strPlanCodigo;
    var intSecuenciaAnt = 0;
    var intSecuenciaAct = 0;
    var intDefecto = 0;
    var intPrincipal = 0;
    var intOpcional = 0;
    var fltCF = 0;
    var strPlanxDefecto = '';
    var ddlPlan;
    var txtCFPlanServicio;
    var strPlanCodigo1;
    var hidnPlanServicioValue = document.getElementById('hidnPlanServicioValue');
    var hidPlanServicioNo = document.getElementById('hidPlanServicioNo');
    var strPlanServicio = '*ID*' + idFila;
    var strPlanServicioNo = '*ID*' + idFila;
    var booIngreso;
    var hidMontoServicios;
    var fltMonto = 0;

    var ddlCampana;
    var imgVerCuota;
    var txtTextoEquipo;
    var imgListaEquipo;
    var txtEquipoPrecio;
    var strSolucion = '';
    var arrPlan;
    //gaa20140521
    var txtNroTelefono;
    //fin gaa20140521
    var ddlTopeConsumo;

    hidNGrupoPaqueteValues.value += '{'

    for (j = 1; j < cont; j++) {
        strPlan = arrPlanes[j];
        strPlanCodigo = strPlan.split(';')[0];
        arrPlanCodigo = strPlanCodigo.split('_');

        intSecuenciaAct = arrPlanCodigo[4];
        intDefecto = arrPlanCodigo[6];
        intPrincipal = arrPlanCodigo[7];
        intOpcional = arrPlanCodigo[8];
        strPlanCodigo1 = arrPlanCodigo[0];
        fltCF = parseFloat(arrPlanCodigo[1]);

        if (intSecuenciaAnt == 0) {
            intSecuenciaAnt = intSecuenciaAct;

            //ddlTipoProducto = document.getElementById('ddlTipoProducto_' + idFila);
            ddlCampana = document.getElementById('ddlCampana' + idFila);
            ddlPlazo = document.getElementById('ddlPlazo' + idFila);
            ddlPaquete = document.getElementById('ddlPaquete' + idFila);
            ddlPlan = document.getElementById('ddlPlan' + idFila);
            //gaa20140521: Valida si pertenece a telefonia fija
            if ((intSecuenciaAct == 1) || (intSecuenciaAct == constConf3PITelefonia)) {//|| intSecuenciaAct == 209) {
                txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
                if (txtNroTelefono != null)
                    txtNroTelefono.style.display = '';

                ddlTopeConsumo = document.getElementById('ddlTopeConsumo' + idFila);
                if (ddlTopeConsumo != null)
                    ddlTopeConsumo.style.display = '';
                //gaa20140801
                if (getValue('hidTopesConsumo').length == 0)
                    llenarTopesConsumoIfr(idFila);
                else
                    asignarTopeConsumo(idFila);
                //fin gaa20140801
            }
            //fin gaa20140521
            ddlCampana.disabled = true;
            ddlPlazo.disabled = true;
            ddlPaquete.disabled = true;
            ddlPlan.disabled = true;

            hidNGrupoPaqueteValues.value += ',[' + idFila + ']';
        }

        if (intPrincipal == 1) //Si es plan
        {
            if (intSecuenciaAnt == intSecuenciaAct) {
                strPlanesCombo += '|' + strPlan;

                if (intDefecto == 1)
                    strPlanxDefecto = strPlan.split(';')[0];

                //document.getElementById('ddlSolucion' + idFila).disabled = true;
            }
            else {
                ddlServicio = document.getElementById('ddlServicio' + idFila);
                llenarDatosCombo(ddlServicio, strPlanesCombo, true);

                if (seleccionarValorCombo(ddlServicio, strPlanxDefecto)) {
                    txtCFPlanServicio = document.getElementById('txtCFPlanServicio' + idFila);
                    txtCFPlanServicio.value = strPlanxDefecto.split('_')[1];

                    calcularCFxProducto();
                }

                if (intDefecto == 1)
                    strPlanxDefecto = strPlan.split(';')[0];

                strPlanesCombo = '|' + strPlan;
                //gaa20140826
                if (verificarCombo())
                    agregarFila(false, idFila, false);
                else
                    agregarFila(false, 0, false);
                //fin gaa20140826
                /*if (strSolucion.length == 0)	
                {
                var ddlSolucion = document.getElementById('ddlSolucion' + idFila);
                var strSolucion = '|' + ddlSolucion.value + ";" + obtenerTextoSeleccionado(ddlSolucion);
                ddlSolucion.disabled = true;
                }*/

                idFila += 1;

                /*ddlSolucion = document.getElementById('ddlSolucion' + idFila);
                ddlSolucion.disabled = true;
                llenarDatosCombo(ddlSolucion, strSolucion, false);*/

                intSecuenciaAnt = intSecuenciaAct;

                //INICIATIVA - 733 - INI - C26
                //                    var cods = "3|"
                if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play) {

                    CodGrupoServicioHFC = false;
                    var CodGrupoServicio = Key_CodGrupoServicio.split('|');
                    for (var i = 0; i < CodGrupoServicio.length; i++) {
                        if (CodGrupoServicio[i] == intSecuenciaAct) {
                            CodGrupoServicioHFC = true;
                        }
                    }

                    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
                        //                    if (intSecuenciaAct == Key_CodGrupoServicio) {
                        IDFilaDinamicaServAdicional = idFila;
                        //                    }
                    }
                }
                //INICIATIVA - 733 - FIN - C26

                ddlPlazo = document.getElementById('ddlPlazo' + idFila);
                llenarDatosCombo(ddlPlazo, strPlazo, false);

                ddlPaquete = document.getElementById('ddlPaquete' + idFila);

                ddlPlazo.disabled = true;
                ddlPaquete.disabled = true;

                ddlCampana = document.getElementById('ddlCampana' + idFila);
                llenarDatosCombo(ddlCampana, strCampanaActual, false);
                ddlPlan = document.getElementById('ddlPlan' + idFila);
                llenarDatosCombo(ddlPlan, strPlanActual, false);

                ddlCampana.disabled = true;
                ddlPlan.disabled = true;

                hidNGrupoPaqueteValues.value += ',[' + idFila + ']';

                strPlanServicio += '*ID*' + idFila;
                strPlanServicioNo += '*ID*' + idFila;
            }
        }
        else //Si es servicio
        {
            //if (intOpcional == 0)
            if (intOpcional == 1) {
                arrPlan = strPlan.split(';');
                //strPlanServicio += strPlanServicio = '|' + '__2_' + strPlan;
                strPlanServicio += '|' + '__2_' + arrPlan[0] + ';(*) ' + arrPlan[1];

                hidMontoServicios = document.getElementById('hidMontoServicios' + idFila);
                fltMonto = (hidMontoServicios.value.length > 0) ? parseFloat(hidMontoServicios.value) : 0;
                hidMontoServicios.value = fltMonto + fltCF;
            }
            else {
                strPlanServicioNo += '|' + '__0_' + strPlan;
            }
        }

        if (j == cont - 1) {
            ddlServicio = document.getElementById('ddlServicio' + idFila);
            llenarDatosCombo(ddlServicio, strPlanesCombo, true);

            if (seleccionarValorCombo(ddlServicio, strPlanxDefecto)) {
                txtCFPlanServicio = document.getElementById('txtCFPlanServicio' + idFila);
                txtCFPlanServicio.value = strPlanxDefecto.split('_')[1];
            }
        }
    }

    hidnPlanServicioValue.value += strPlanServicio;
    hidPlanServicioNo.value += strPlanServicioNo;

    hidNGrupoPaqueteValues.value += '}';

    calcularCFxProducto();
    calcularLCxProductoFijo();

    //document.getElementById('hidPromociones').value += arrServProm[1];

    //asignarPromocion3Play(idFilaIni, idFila);

    quitarImagenEsperando();
}
function aServicioMultiLinea3PlayMigra(idFila, strValor) {
    if (strValor == '' || strValor == null) return;

    cargarImagenEsperando();

    //var arrServProm = strValor.split('¬');
    var strTipoProducto = getValue('hidCodigoTipoProductoActual');
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var strCampanaActual = '|' + ddlCampana.value + ";" + obtenerTextoSeleccionado(ddlCampana);
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    var strPlazo = '|' + ddlPlazo.value.split('-')[0] + ";" + obtenerTextoSeleccionado(ddlPlazo);//INICIATIVA 920
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var strPlanActual = '|' + ddlPlan.value + ";" + obtenerTextoSeleccionado(ddlPlan);
    var hidPlazoActual = document.getElementById('hidPlazoActual');
    var hidPaqueteActual = document.getElementById('hidPaqueteActual');
    var hidNGrupoPaqueteValues = document.getElementById('hidNGrupoPaqueteValues');
    var idFilaIni = idFila;
    idFila = parseInt(idFila);
    hidPlazoActual.value = strPlazo;
    var arrPlanes = strValor.split('|');
    var arrPlanCodigo;
    var cont = arrPlanes.length;
    var strPlanesCombo = '';
    var strPlan;
    var strPlanCodigo;
    var intSecuenciaAnt = 0;
    var intSecuenciaAct = 0;
    var intDefecto = 0;
    var intPrincipal = 0;
    var intOpcional = 0;
    var fltCF = 0;
    var strPlanxDefecto = '';
    var ddlPlan;
    var txtCFPlanServicio;
    var strPlanCodigo1;
    var hidnPlanServicioValue = document.getElementById('hidnPlanServicioValue');
    var hidPlanServicioNo = document.getElementById('hidPlanServicioNo');
    var strPlanServicio = '*ID*' + idFila;
    var strPlanServicioNo = '*ID*' + idFila;
    var booIngreso;
    var hidMontoServicios;
    var fltMonto = 0;

    var ddlCampana;
    var imgVerCuota;
    var txtTextoEquipo;
    var imgListaEquipo;
    var txtEquipoPrecio;
    var strSolucion = '';
    var arrPlan;
    //gaa20140521
    var txtNroTelefono;
    //fin gaa20140521
    var ddlTopeConsumo;

    hidNGrupoPaqueteValues.value += '{'
    //------
    if (parent.getValue('hidTipoOperacion') == 2 && parent.getValue('hidnTipoOfertaValue') == 01 && parent.getValue('hidModalidadVenta') == 2) {

        var vPlan = "";
        for (j = 1; j < cont; j++) {
            strPlan = arrPlanes[j];
            strPlanCodigo = strPlan.split(';')[0];
            arrPlanCodigo = strPlanCodigo.split('_');

            if (arrPlanCodigo[7] == 1) {
                vPlan = vPlan + "|" + arrPlanCodigo[4];
            }
        }
        //salio
        switch (vPlan) {
            case "|" + constConf3PIClaroTVDig || "|" + constConf3PIClaroTVAna:   //203 204
                alert(constMsgNoClaroTvMigra);
                break;

            case "|" + constConf3PITelefonia + "|" + constConf3PIInternet || "|" + constConf3PIInternet + "|" + constConf3PITelefonia:  //200+201 // 201+202
                alert(constMsgNoTelefInterMigra);
                break;

            default:
                //***
                //------
                for (j = 1; j < cont; j++) {
                    strPlan = arrPlanes[j];
                    strPlanCodigo = strPlan.split(';')[0];
                    arrPlanCodigo = strPlanCodigo.split('_');

                    intSecuenciaAct = arrPlanCodigo[4];
                    intDefecto = arrPlanCodigo[6];
                    intPrincipal = arrPlanCodigo[7];
                    intOpcional = arrPlanCodigo[8];
                    strPlanCodigo1 = arrPlanCodigo[0];
                    fltCF = parseFloat(arrPlanCodigo[1]);

                    if (intSecuenciaAnt == 0) {
                        intSecuenciaAnt = intSecuenciaAct;

                        //ddlTipoProducto = document.getElementById('ddlTipoProducto_' + idFila);
                        ddlCampana = document.getElementById('ddlCampana' + idFila);
                        ddlPlazo = document.getElementById('ddlPlazo' + idFila);
                        ddlPaquete = document.getElementById('ddlPaquete' + idFila);
                        ddlPlan = document.getElementById('ddlPlan' + idFila);
                        //gaa20140521: Valida si pertenece a telefonia fija
                        if ((intSecuenciaAct == 1) || (intSecuenciaAct == constConf3PITelefonia)) {//|| intSecuenciaAct == 209) {
                            txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
                            if (txtNroTelefono != null)
                                txtNroTelefono.style.display = '';

                            ddlTopeConsumo = document.getElementById('ddlTopeConsumo' + idFila);
                            if (ddlTopeConsumo != null)
                                ddlTopeConsumo.style.display = '';
                            //gaa20140801
                            if (getValue('hidTopesConsumo').length == 0)
                                llenarTopesConsumoIfr(idFila);
                            else
                                asignarTopeConsumo(idFila);
                            //fin gaa20140801
                        }
                        //fin gaa20140521
                        ddlCampana.disabled = true;
                        ddlPlazo.disabled = true;
                        ddlPaquete.disabled = true;
                        ddlPlan.disabled = true;

                        hidNGrupoPaqueteValues.value += ',[' + idFila + ']';
                    }

                    if (intPrincipal == 1) //Si es plan
                    {
                        if (intSecuenciaAnt == intSecuenciaAct) {
                            strPlanesCombo += '|' + strPlan;

                            if (intDefecto == 1)
                                strPlanxDefecto = strPlan.split(';')[0];

                            //document.getElementById('ddlSolucion' + idFila).disabled = true;
                        }
                        else {
                            ddlServicio = document.getElementById('ddlServicio' + idFila);
                            llenarDatosCombo(ddlServicio, strPlanesCombo, true);

                            if (seleccionarValorCombo(ddlServicio, strPlanxDefecto)) {
                                txtCFPlanServicio = document.getElementById('txtCFPlanServicio' + idFila);
                                txtCFPlanServicio.value = strPlanxDefecto.split('_')[1];

                                calcularCFxProducto();
                            }

                            if (intDefecto == 1)
                                strPlanxDefecto = strPlan.split(';')[0];

                            strPlanesCombo = '|' + strPlan;
                            //gaa20140826
                            if (verificarCombo())
                                agregarFila(false, idFila);
                            else
                                agregarFila(false, 0);
                            //fin gaa20140826
                            //                                       if (strSolucion.length == 0)	
                            //                                       {
                            //                                       var ddlSolucion = document.getElementById('ddlSolucion' + idFila);
                            //                                       var strSolucion = '|' + ddlSolucion.value + ";" + obtenerTextoSeleccionado(ddlSolucion);
                            //                                       ddlSolucion.disabled = true;
                            //                                       }

                            idFila += 1;

                            //                                       ddlSolucion = document.getElementById('ddlSolucion' + idFila);
                            //                                       ddlSolucion.disabled = true;
                            //                                       llenarDatosCombo(ddlSolucion, strSolucion, false);

                            intSecuenciaAnt = intSecuenciaAct;

                            ddlPlazo = document.getElementById('ddlPlazo' + idFila);
                            llenarDatosCombo(ddlPlazo, strPlazo, false);

                            ddlPaquete = document.getElementById('ddlPaquete' + idFila);

                            ddlPlazo.disabled = true;
                            ddlPaquete.disabled = true;

                            ddlCampana = document.getElementById('ddlCampana' + idFila);
                            llenarDatosCombo(ddlCampana, strCampanaActual, false);
                            ddlPlan = document.getElementById('ddlPlan' + idFila);
                            llenarDatosCombo(ddlPlan, strPlanActual, false);

                            ddlCampana.disabled = true;
                            ddlPlan.disabled = true;

                            hidNGrupoPaqueteValues.value += ',[' + idFila + ']';

                            strPlanServicio += '*ID*' + idFila;
                            strPlanServicioNo += '*ID*' + idFila;
                        }
                    }
                    else //Si es servicio
                    {
                        //if (intOpcional == 0)
                        if (intOpcional == 1) {
                            arrPlan = strPlan.split(';');
                            //strPlanServicio += strPlanServicio = '|' + '__2_' + strPlan;
                            strPlanServicio += '|' + '__2_' + arrPlan[0] + ';(*) ' + arrPlan[1];

                            hidMontoServicios = document.getElementById('hidMontoServicios' + idFila);
                            fltMonto = (hidMontoServicios.value.length > 0) ? parseFloat(hidMontoServicios.value) : 0;
                            hidMontoServicios.value = fltMonto + fltCF;
                        }
                        else {
                            strPlanServicioNo += '|' + '__0_' + strPlan;
                        }
                    }

                    if (j == cont - 1) {
                        ddlServicio = document.getElementById('ddlServicio' + idFila);
                        llenarDatosCombo(ddlServicio, strPlanesCombo, true);

                        if (seleccionarValorCombo(ddlServicio, strPlanxDefecto)) {
                            txtCFPlanServicio = document.getElementById('txtCFPlanServicio' + idFila);
                            txtCFPlanServicio.value = strPlanxDefecto.split('_')[1];
                        }
                    }
                }

                hidnPlanServicioValue.value += strPlanServicio;
                hidPlanServicioNo.value += strPlanServicioNo;

                hidNGrupoPaqueteValues.value += '}';

                calcularCFxProducto();
                calcularLCxProductoFijo();

                //document.getElementById('hidPromociones').value += arrServProm[1];

                //asignarPromocion3Play(idFilaIni, idFila);

                quitarImagenEsperando();
                //***

                break;

        }


    }


    quitarImagenEsperando();


}
function asignarPromocion3Play(idFila) {
    var tabla;
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    if (codigoTipoProductoActual == codTipoProd3Play) {
        tabla = document.getElementById('tblTablaHFC');
    };

    if (codigoTipoProductoActual == codTipoProd3PlayInalam) {
        tabla = document.getElementById('tblTabla3PlayInalam');
    };

    var cont = tabla.rows.length;
    var nCell; //0: Imagen, 1:Imagen
    var strSrvSel = '';
    var strPromociones = getValue('hidPromociones');
    var arrPromociones = strPromociones.split('|');
    var strServicio = getValue('hidnPlanServicioValue');
    var strAgrupaPaquete = obtenerPaqueteActual(idFila);

    //Obtengo promociones del plan(es) seleccionado
    for (var i = 0; i < cont; i++) {
        nCell = 3; //0: Imagen Confirmar, 1:Imagen Eliminar, 2:Plazo, 3:Solucion
        fila = tabla.rows[i];

        //idFila
        idFilaX = fila.cells[nCell].getElementsByTagName("select")[0].id.substring(11);

        //if (idFila == 0 || (idFila < idFilaX && idFilaX < idFilaFinal) || idFila == idFilaX) 
        //if (idFila == 0 || (idFila <= idFilaX))
        if (strAgrupaPaquete.indexOf('[' + idFilaX + ']') > -1) {
            strSrvSel = '';

            nCell += 2;
            planSel = fila.cells[nCell].getElementsByTagName("select")[0].value.split('_')[0];
            strSrvSel += ';' + planSel;

            borrarPromocion(idFilaX);

            strSrvSel += extraerCodigoServicio(idFilaX);

            asignarPromocion3Play1(idFilaX, strSrvSel, false);
        }
    }
}
//fin gaa20131111
function asignarPromocion3Play1(idFila, strSrvSel, haciaTemp) {
    var strPromociones = getValue('hidPromociones');
    //gaa20131108
    var strAgrupaPaquete = obtenerPaqueteActual(idFila);
    var idFilaPaquete = obtenerFilaPaquete(strAgrupaPaquete);
    var a = '{' + idFilaPaquete + '}';
    var z = '{/' + idFilaPaquete + '}';

    a = strPromociones.indexOf(a);
    aIni = strPromociones.substring(a).indexOf('}') + 1;
    aIni = a + aIni;
    var zIni = strPromociones.indexOf(z);

    strPromociones = strPromociones.substring(aIni, zIni);
    //fin gaa20131108
    var arrPromociones = strPromociones.split('|');
    var arrPromocion;
    var strPromocion;
    var strCodProm = '';
    var arrCodProm;
    var strIDDET;
    var strIDPRODUCTO;
    var strIDLINEA;
    var intPosIni;
    var strCodSrv;
    var strResultado = '';
    var hidNPromocionValues;
    var arrSrv = strSrvSel.split(';');

    if (!haciaTemp)
        hidNPromocionValues = document.getElementById('hidNPromocionValues');
    else
        hidNPromocionValues = document.getElementById('hidPromocionTemp');

    for (var i = 1; i < arrSrv.length; i++) {
        strCodSrv = arrSrv[i];
        intPosIni = strPromociones.indexOf(strCodSrv);

        if (intPosIni > -1) {
            for (var j = 1; j < arrPromociones.length; j++) {
                strPromocion = arrPromociones[j];
                arrPromocion = strPromocion.split('_');
                strCodProm = arrPromocion[0];
                arrCodProm = strCodProm.split('.');
                strIDDET = arrCodProm[0];
                strIDPRODUCTO = arrCodProm[1];
                strIDLINEA = arrCodProm[2];

                strFLGEDI = arrPromocion[3];

                if (strCodSrv == strIDDET + '.' + strIDPRODUCTO + '.' + strIDLINEA) {
                    if (strFLGEDI == '0')
                        strResultado += '|' + '__2_' + strCodProm + ';' + strPromocion.split(';')[1];
                    else
                        strResultado += '|' + '__1_' + strCodProm + ';' + strPromocion.split(';')[1];
                }
            }
        }
    }

    hidNPromocionValues.value += '*ID*' + idFila + strResultado;
}

function extraerCodigoServicio(idFila) {
    var strResultado = '';
    var strServicio = getValue('hidnPlanServicioValue');
    var arrServicios;
    var strIdFila = '*ID*' + idFila;
    var strCodigo;

    var intPosIni = strServicio.indexOf(strIdFila);
    var intPosFin = 0;

    if (intPosIni > -1) {
        intPosFin = strServicio.substring(intPosIni + 4).indexOf('*ID*');

        if (intPosFin == -1)
            intPosFin = strServicio.length;
        else
            intPosFin += intPosIni + 4;

        strServicio = strServicio.substring(intPosIni, intPosFin);

        arrServicios = strServicio.split('|');

        for (var i = 1; i < arrServicios.length; i++) {
            strCodigo = arrServicios[i].split('_')[3];
            strResultado += ';' + strCodigo;
        }
    }

    return strResultado;
}

function calcularCFServicio(idFila) {
    var strServicio = getValue('hidnPlanServicioValue');
    var arrServicios;
    var strIdFila = '*ID*' + idFila;
    var dblCF = 0;
    var dblCFTotal = 0;

    var intPosIni = strServicio.indexOf(strIdFila);
    var intPosFin = 0;

    if (intPosIni > -1) {
        intPosFin = strServicio.substring(intPosIni + 4).indexOf('*ID*');

        if (intPosFin == -1)
            intPosFin = strServicio.length;
        else
            intPosFin += intPosIni + 4;

        strServicio = strServicio.substring(intPosIni, intPosFin);
        arrServicios = strServicio.split('|');

        for (var i = 1; i < arrServicios.length; i++) {
            if (arrServicios[i] != '') {
                dblCF = parseFloat(arrServicios[i].split('_')[4]);
                dblCFTotal += dblCF;
            }
        }
    }

    return dblCFTotal;
}

function borrarPromocion(idFila) {
    var hidNPromocionValues = document.getElementById('hidNPromocionValues');
    var strPromocion = hidNPromocionValues.value;
    var strIdFila = '*ID*' + idFila;

    var intPosIni = strPromocion.indexOf(strIdFila);
    var intPosFin = 0;

    if (intPosIni > -1) {
        intPosFin = strPromocion.substring(intPosIni + 4).indexOf('*ID*');

        if (intPosFin == -1)
            intPosFin = strPromocion.length;
        else
            intPosFin += intPosIni + 4;

        hidNPromocionValues.value = strPromocion.replace(strPromocion.substring(intPosIni, intPosFin), '');
    }
}

function asignarPaquete(idFila, strValor) {
    var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
    var strPaqueteActual = document.getElementById('hidPaqueteActual').value;

    llenarDatosCombo(ddlPaquete, strValor, true);

    if (strPaqueteActual.length > 0)
        ddlPaquete.value = strPaqueteActual;
}

function cambiarPaquete(strPaquete, idFila) {
    if (strPaquete.length > 0) {
        var strPaqueteDesc = obtenerTextoSeleccionado(document.getElementById('ddlPaquete' + idFila));
        var strPaqCompleto = '|' + strPaquete + ";" + strPaqueteDesc;
        setValue('hidPaqueteActual', strPaquete);
        setValue('hidPaqActCompleto', strPaqCompleto);

        var vCodigoTipoProductoActual = getValue('hidCodigoTipoProductoActual')
        //-FTTH  cambiarPaquete() -    if (vCodigoTipoProductoActual != codTipoProd3Play 
        if (vCodigoTipoProductoActual != codTipoProdFTTH && vCodigoTipoProductoActual != codTipoProd3Play && vCodigoTipoProductoActual != codTipoProd3PlayInalam) {
            LlenarPlanPaqIfr(strPaquete, idFila);
        } else {
            LlenarPlanPaq3PlayIfr(strPaquete, idFila, vCodigoTipoProductoActual);
            setValue('hidPaquete3Play', strPaqueteDesc);
        }
    }
    else
        calcularLCxProductoFijo();
}

function LlenarPlanPaqIfr(strPaquete, idFila) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strPaquete=' + strPaquete;
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strMetodo=' + 'LlenarPlanPaq';

    self.frames['iframeAuxiliar'].location.replace(url);
}
function LlenarPlanPaq3PlayIfr(strPaquete, idFila, vCodigoTipoProductoActual) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strPaquete=' + strPaquete;
    url += '&strCampana=' + getValue('ddlCampana' + idFila);
    url += '&strPlazo=' + getValue('ddlPlazo' + idFila);
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strFlujo=' + obtenerFlujo();
    url += '&strEvaluarSoloFijo=' + parent.getValue('hidEvaluarSoloFijo');
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strCombo=' + parent.getValue('ddlCombo');
    url += '&strMetodo=' + 'LlenarPlanPaq3Play';
    url += '&strTipoProducto=' + vCodigoTipoProductoActual;
    url += '&strCumpleReglaAClienteRRLL=' + parent.getValue('hidCumpleReglaAClienteRRLL'); //PROY-29121
    self.frames['iframeAuxiliar'].location.replace(url);
}

function cambiarPlazo(strPlazo, idFila) {
    strPlazo = strPlazo.split('-')[0];//INICIATIVA 920
    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][cambiarPlazo()] ", "Entro en la funcion cambiarPlazo()");

    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    var ddlPlan = document.getElementById('ddlPlan' + idFila);

    if (ddlPlan == null)
        ddlPlan = document.getElementById('ddlPaquete' + idFila);

    if (verificarCombo() && (codigoTipoProductoActual == codTipoProdDTH || codigoTipoProductoActual == codTipoProd3Play)) {
        verificarDireccion(idFila);
    }

    var strFlujo = flujoAlta;
    if (parent.getValue('hidNTienePortabilidadValues') == 'S')
        strFlujo = flujoPortabilidad;

    var strTienePaquete = document.getElementById('hidTienePaquete').value;
    inicializarPlan(idFila);

    // if (codigoTipoProductoActual != codTipoProd3Play) {
    //FTTH -! llenarDatosCombo1
    if (!(codigoTipoProductoActual == codTipoProd3Play || codigoTipoProductoActual == codTipoProd3PlayInalam || codigoTipoProductoActual == codTipoProdFTTH)) {
        if (getValue('hidPlanesCombo').length == 0)
            llenarDatosCombo(ddlPlan, '', true);
        else
            return;

        llenarDatosCombo1(document.getElementById('divListaEquipo' + idFila), '', idFila, 'Equipo', false);

        calcularCFxProducto();
    }

    if (strPlazo.length > 0) {
        //FTTH -! LlenarPaquete3PlayIfr(idFila, strPlazo, strFlujo, codigoTipoProductoActual);
        if (codigoTipoProductoActual == codTipoProd3Play || codigoTipoProductoActual == codTipoProd3PlayInalam || codigoTipoProductoActual == codTipoProdFTTH) {
            var strCampana = getValue('ddlCampana' + idFila);
            LlenarPaquete3PlayIfr(strCampana, strPlazo, idFila, codigoTipoProductoActual);
            return;
        }
    }

    if (strPlazo.length > 0) {
        if (strTienePaquete == 'S') {
            var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
            if (ddlPaquete != null)
                llenarDatosCombo(ddlPaquete, '', true);
            //FTTH -! LlenarPaquetePlanIfr(idFila, strPlazo, strFlujo); //Movil y otros
            if ((codigoTipoProductoActual != codTipoProdFTTH) && (codigoTipoProductoActual != codTipoProd3Play) && (codigoTipoProductoActual != codTipoProd3PlayInalam)) {
                if (codigoTipoProductoActual == codTipoProdMovil) {
                    LlenarPaquetePlanIfr(idFila, strPlazo, strFlujo); //Movil y otros
                }
                else {
                    LlenarPlanIfr(idFila, strPlazo, strFlujo);
                }
            }
        }
        else {
            //gaa20161020
            //LlenarPlanIfr(idFila, strPlazo, strFlujo);
            if (document.getElementById('tdFamiliaPlanMovil').style.display != 'none' && codigoTipoProductoActual == codTipoProdMovil)
                LlenarFamiliaPlanIfr(idFila, strPlazo);
            else
                LlenarPlanIfr(idFila, strPlazo, strFlujo);
            //fin gaa20161020
        }


    }
    else {
        asignarPlan(idFila, '');
        if (strTienePaquete == 'S')
            asignarPaquete(idFila, '');
    }
}

function LlenarPlanIfr(idFila, strPlazo, strFlujo) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strOferta=' + parent.getValue('ddlOferta');
    url += '&strPlazo=' + strPlazo;
    url += '&strFlujo=' + strFlujo;
    url += '&strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
    url += '&strTipoDocumento=' + parent.getValue('ddlTipoDocumento');
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strCampana=' + getValue('ddlCampana' + idFila);
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&strCasoEspecial=' + getValor(parent.getValue('ddlCasoEspecial'), 0);
    url += '&strRiesgo=' + parent.getValue('hidnRiesgoDCValue');
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strCombo=' + parent.getValue('ddlCombo');
    url += '&strEvaluarSoloFijo=' + parent.getValue('hidEvaluarSoloFijo');
    //gaa20161020
    if (getValue('hidCodigoTipoProductoActual') == codTipoProdMovil) {
        if (document.getElementById('tdFamiliaPlanMovil').style.display != 'none')
            url += '&strFamiliaPlan=' + document.getElementById('ddlFamiliaPlan' + idFila).value;
        parent.document.getElementById('hidFamilia').value = document.getElementById('ddlFamiliaPlan' + idFila).value; //PROY - 30748
    }
    url += '&strCumpleReglaAClienteRRLL=' + parent.getValue('hidCumpleReglaAClienteRRLL'); //PROY-29121
    //fin gaa20161020
    url += '&strMetodo=' + 'LlenarPlan';

    self.frames['iframeAuxiliar'].location.replace(url);
}

function LlenarPaquetePlanIfr(idFila, strPlazo, strFlujo) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strOferta=' + parent.getValue('ddlOferta');
    url += '&strPlazo=' + strPlazo;
    url += '&strFlujo=' + strFlujo;
    url += '&strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
    url += '&strTipoDocumento=' + parent.getValue('ddlTipoDocumento');
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&strCasoEspecial=' + parent.getValue('ddlCasoEspecial');
    url += '&strRiesgo=' + parent.getValue('hidnRiesgoDCValue');
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strCampana=' + getValue('ddlCampana' + idFila);
    url += '&strCombo=' + parent.getValue('ddlCombo');
    url += '&strMetodo=' + 'LlenarPaquetePlan';

    self.frames['iframeAuxiliar'].location.replace(url);
}

function cambiarCampana(idFila, strValor) {
    //PROY 140736 INI
    var ddlCampanaB = document.getElementById('ddlCampana' + idFila);
    parent.document.getElementById('hdCampaniaBuyBack').value = strValor;
    var campBuyback = Key_CodCampaniaBuyBack.split('|');
    var SeleccionoBuyback = false;
    var ExistenFilasBuyback = false;
    for (var i = 0; i < campBuyback.length; i++) {
        if (campBuyback[i] == strValor) {
            SeleccionoBuyback = true;
        }
    }

    var IdFilas = "";
    var IdFilasFinal = "";
    var Filas = getValue('hidCadenaDetalle').split('|');
    for (var i = 0; i < Filas.length; i++) {
        IdFilas = Filas[i].split(';')[15] + '|';
        IdFilasFinal = IdFilasFinal + IdFilas
    }

    var ExistenFilas = IdFilasFinal.split('|');
    for (var i = 0; i < ExistenFilas.length; i++) {
        for (var j = 0; j < campBuyback.length; j++) {
            if (ExistenFilas[i] == campBuyback[j]) {
                ExistenFilasBuyback = true;
                break;
            }
        }
      
    }


    if (parseInt(idFila) > 1) {
        if (ExistenFilasBuyback == true) {
            if (SeleccionoBuyback == false) {
                alert(Key_Msj_Error_CBO_BuyBack)
                ddlCampanaB.value = "";
                return false;
            }
         }
    
    }

    //PROY-140743 - INI
    var codOperacion = parent.getValue('ddlTipoOperacion');
    if (codOperacion == '25')
        llenarPromociones(idFila);
    //PROY-140743 - FIN


//PROY-140736 FIN
    //PROY-32129 FASE 2:: INI
    PageMethods.ValidarCampEspUniv(strValor, idFila, ValidarCampEspUniv_Callback);
    //PROY-32129 FASE 2:: FIN

    //INI IDEA-142717
    var strTipoDoc = parent.document.getElementById('ddlTipoDocumento').value;
    var strNroDoc = parent.getValue('txtNroDoc');
    var catCampanas = obtenerDetalleCarritoCampana();
    PageMethods.ValidarCampVacunaton(strValor,strTipoDoc,strNroDoc,catCampanas, ValidarCampVacunaton_Callback);
    //FIN IDEA-142717

    cambiarPlazo('', idFila);
    var flag = true;
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    if (ddlPlazo.disabled) {
        cambiarPlazo(getValue('hidPlazoActual').split('_')[0], idFila)
        return;
    }

    var strPlazosDTH = getValue('hidPlazosDTH');
    var strPlazosHFC = getValue('hidPlazosHFC');
    var strPlazosFTTH = getValue('hidPlazosFTTH'); // FTTH - cambiarCampana
    var strPlazosMovil = getValue('hidPlazosMovil');
    var strPlazosFijo = getValue('hidPlazosFijo');
    var strPlazosBAM = getValue('hidPlazosBAM');
    var strPlazosInterInalam = getValue('hidPlazosInterInalam'); //PROY-31812 - IDEA-43340
    var strPlazosVentaVarios = getValue('hidPlazosVentaVarios');
    var strPlazosHFCInalamb = getValue('hidPlazosHFCInalamb');
    switch (getValue('hidCodigoTipoProductoActual')) {
        case codTipoProdMovil:
            if (strPlazosMovil.length > 0) {
                llenarDatosCombo(ddlPlazo, strPlazosMovil, flag);
            }
            break;
        case codTipoProdFijo:
            if (strPlazosFijo.length > 0) {
                llenarDatosCombo(ddlPlazo, strPlazosFijo, true);
            }
            break;
        //PROY-31812 - IDEA-43340 - INICIO   
        case codTipoProdInterInalam:
            if (strPlazosInterInalam.length > 0) {
                llenarDatosCombo(ddlPlazo, strPlazosInterInalam, true);
            }
            break;
        //PROY-31812 - IDEA-43340 - FIN   
        case codTipoProdBAM:
            if (strPlazosBAM.length > 0) {
                llenarDatosCombo(ddlPlazo, strPlazosBAM, true);
            }
            break;
        case codTipoProdDTH:
            if (strPlazosDTH.length > 0) {
                llenarDatosCombo(ddlPlazo, strPlazosDTH, true);
                return;
            }
            break;
        case codTipoProd3Play:
            if (strPlazosHFC.length > 0) {
                llenarDatosCombo(ddlPlazo, strPlazosHFC, true);
            }
            break;

        //FTTH -INICIO - cambiarCampana              
        case codTipoProdFTTH:
            if (strPlazosFTTH.length > 0) {
                llenarDatosCombo(ddlPlazo, strPlazosFTTH, true);
            }
            break;
        //FTTH -FIN - cambiarCampana   

        case codTipoProdVentaVarios:
            if (strPlazosVentaVarios.length > 0) {
                llenarDatosCombo(ddlPlazo, strPlazosVentaVarios, true);
            }
            break;
        case codTipoProd3PlayInalam:
            if (strPlazosHFCInalamb.length > 0) {
                llenarDatosCombo(ddlPlazo, strPlazosHFCInalamb, true);
            }
            break;
    }

    // Seleccionar plazo x defecto
    if (ddlPlazo.length == 2 || ddlPlazo.length == 5) {
        ddlPlazo.selectedIndex = 1;
        cambiarPlazo(ddlPlazo.value.split('-')[0], idFila);
    }
}

//PROY-32129 :: INI
function ValidarCampEspUniv_Callback(objResponse) {
    if (objResponse.split('|')[0] == "1") {
        //PROY -32129 FASE 2 INICIO
        parent.document.getElementById('btnAgregarPlan').disabled = true;
        //PROY -32129 FASE 2 FIN

        //campaña universitaria valida
        parent.document.getElementById('txtCodAlumno').value = '';
        parent.document.getElementById('ddlInstitucion').selectedIndex = 0;

        parent.document.getElementById('trDatosAlumnoInstitucion').style.display = '';
        parent.document.getElementById('hidIdCampSelec').value = 'ddlCampana' + objResponse.split('|')[1];
        DesabilitarDetalleGrilla();
        //PROY -32129 FASE 2 INICIO
        DesabilitarImgs();
        //PROY -32129 FASE 2 FIN
    }
}

//INI IDEA-142717
function ValidarCampVacunaton_Callback(objResponse) {
    if (objResponse.split('|')[0] == "-1" || objResponse.split('|')[0] == "-99") {
        alert(objResponse.split('|')[1]);

        parent.document.getElementById('btnAgregarPlan').disabled = true;
        DesabilitarDetalleGrilla();
        DesabilitarImgs();
        return;
    }
}
//FIN IDEA-142717

function DesabilitarDetalleGrilla() {
    var tblDetalleItems = document.getElementById('tblTabla' + document.getElementById('hidTipoProductoActual').value);
    var controlComboBox = tblDetalleItems.getElementsByTagName("select");
    for (var i = 0; i < controlComboBox.length; i++)
        controlComboBox[i].disabled = true;
}

//PROY -32129 FASE 2 INICIO EIQ
function DesabilitarImgs() {
    var tblDetalleItems = document.getElementById('tblTabla' + document.getElementById('hidTipoProductoActual').value);
    var imgTag = tblDetalleItems.getElementsByTagName("img");
    for (var i = 0; i < imgTag.length; i++) imgTag[i].style.visibility = 'hidden';
}
//PROY -32129 FASE 2 FIN EIQ

//gaa20161020
function cambiarFamiliaPlan(strValor, idFila) {
    var strPlazo = document.getElementById('ddlPlazo' + idFila).value.split('-')[0];
    var strFlujo = flujoAlta;
    if (parent.getValue('hidNTienePortabilidadValues') == 'S')
        strFlujo = flujoPortabilidad;

    LlenarPlanIfr(idFila, strPlazo, strFlujo);
}
//fin gaa20161020

function LlenarMaterialIfr(idFila, strCampana, strPlan, strCombo, strProducto,strPromocion) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strCampana=' + strCampana;
    url += '&strPlan=' + strPlan;
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&nroDoc=' + parent.getValue('txtNroDoc');
    url += '&strOrgVenta=' + parent.getValue('hidOrgVenta');
    url += '&strCentro=' + parent.getValue('hidCentro');
    url += '&strFlagPorta=' + parent.getValue('hidNTienePortabilidadValues');
    url += '&strCombo=' + strCombo;
    url += '&strTipoProducto=' + strProducto;
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strPromocion=' + strPromocion; //PROY-140743-INI
    url += '&strMetodo=' + 'LlenarMaterial';

    self.frames['iframeAuxiliar'].location.replace(url);
}

function LlenarEquipoPrecioIfr(idFila, strPlan, strPlanSisact, strPlazo, strCampana, strEquipo, materialBuyback) {//PROY-140736
    cargarImagenEsperando();

    var strCuota = '';
    //INICIATIVA 920
    if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota || parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode) {
        var arrCuota = obtenerCuotaValores(idFila);
        if (arrCuota != null) {
            strCuota = arrCuota[0].split('_')[0];
        }
    }

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&strOferta=' + parent.getValue('ddlOferta');
    url += '&strPlazo=' + strPlazo;
    url += '&strPlanSap=' + strPlan;
    url += '&strPlan=' + strPlanSisact;
    url += '&strCampana=' + strCampana;
    url += '&strMaterial=' + strEquipo;
    url += '&strCanalSap=' + parent.getValue('hidCanalSap');
    url += '&strOrgVenta=' + parent.getValue('hidOrgVenta');
    url += '&strTipoDocVentaSap=' + parent.getValue('hidTipoDocVentaSap');
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strModalidadVenta=' + parent.getValue('ddlModalidadVenta');
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strTipoOficina=' + parent.getValue('ddlCanal');
    url += '&strCuota=' + strCuota;
    url += '&strmaterialbuyback=' + materialBuyback; //PROY-140736
    url += '&strMetodo=' + "LlenarEquipoPrecio";

    self.frames['iframeAuxiliar'].location.replace(url);
}
//gaa20161020
function LlenarFamiliaPlanIfr(idFila, strPlazo) {
    cargarImagenEsperando();

    var strFlujo = flujoAlta;
    if (parent.getValue('hidNTienePortabilidadValues') == 'S')
        strFlujo = flujoPortabilidad;

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strOferta=' + parent.getValue('ddlOferta');
    url += '&strPlazo=' + strPlazo;
    url += '&strFlujo=' + strFlujo;
    url += '&strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
    url += '&strTipoDocumento=' + parent.getValue('ddlTipoDocumento');
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strCampana=' + getValue('ddlCampana' + idFila);
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&strCasoEspecial=' + getValor(parent.getValue('ddlCasoEspecial'), 0);
    url += '&strRiesgo=' + parent.getValue('hidnRiesgoDCValue');
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strCombo=' + parent.getValue('ddlCombo');
    url += '&strEvaluarSoloFijo=' + parent.getValue('hidEvaluarSoloFijo');
    url += '&strModalidadVenta=' + parent.getValue('ddlModalidadVenta');
    url += '&strMetodo=' + "LlenarFamiliaPlan";

    self.frames['iframeAuxiliar'].location.replace(url);
}
//fin gaa20161020
function asignarPrecio(idFila, strValor) {
   //INICIATIVA 920
    if (parent.getValue('ddlModalidadVenta') == constCodModalidadCuota || parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode)
        asignarEquipoPrecioCuota(idFila, strValor);
    else
        asignarEquipoPrecio(idFila, strValor);
}

function asignarEquipoPrecio(idFila, strValor) {
    //gaa20160211
    if (strValor.indexOf('|') > -1) {
        setValue('hidListaDE', strValor.split('|')[1]);
        strValor = strValor.split('|')[0];
    }
    //fin gaa20160211
    var cfPlanServicio = getValue('txtCFPlanServicio' + idFila);

    if (strValor == 0) {
        inicializarEquipo(idFila);
    }
    else {
        var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
        var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);

        if (hidListaPrecio != null) {
            hidListaPrecio.value = strValor;
            txtEquipoPrecio.value = strValor.split('_')[0];
            //gaa20160210
            if (getValue('hidEsSecPendiente') == 'S')
                setValue('hidEsSecPendiente') == '';
            else {
                if (getValue('ddlCampana' + idFila) == consCampanaDiaEnamorados && getValue('hidTipoProductoActual') == 'Movil')
                    campanaDiaEnamorados(idFila);
            }
            //fin gaa20160210
        }
        else {
            if (strValor.indexOf('_') > -1) {
                var arrValor = strValor.split('_');
                var cfMenAlqKit = arrValor[1];

                document.getElementById('txtPrecioInst' + idFila).value = arrValor[0];
                document.getElementById('txtCFMenAlqKit' + idFila).value = (parseFloat(cfMenAlqKit)).toFixed(2);
                document.getElementById('txtCFTotMensual' + idFila).value = (parseFloat(cfPlanServicio) + parseFloat(cfMenAlqKit)).toFixed(2);
            }
        }
    }
}

function asignarEquipoPrecioCuota(idFila, strValor) {
    //gaa20160211
    if (strValor.indexOf('|') > -1) {
        setValue('hidListaDE', strValor.split('|')[1]);
        strValor = strValor.split('|')[0];
    }
    //fin gaa20160211
    document.getElementById('hidListaPrecio' + idFila).value = strValor;
}

//gaa20161020
function asignarFamiliaPlan(idFila, strValor) {
    var ddlFamiliaPlan = document.getElementById('ddlFamiliaPlan' + idFila);
    var arrValor = strValor.split('¬');

    llenarDatosCombo(ddlFamiliaPlan, arrValor[0], false);

    ddlFamiliaPlan.value = FamiliaPlanxDefecto;

    asignarPlan(idFila, arrValor[1]);

    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);

    if (ddlPlazo.disabled && ddlPlazo.value == '') {
        ddlPlazo.value = getValue('hidPlazoActual').split('_')[0];
    }

    // PROY-140743 - IDEA-141192 - Venta en cuotas | INI
    if (Key_FlagGeneralVtaCuotas == "1" && parent.getValue('hidFlagVentaVV') == "1" && parent.getValue('ddlTipoOperacion') == '25') {
        cambiarPromocion();
        LlenarPlanIfr(idFila, ddlPlazo.value, flujoAlta);
    }
    // PROY-140743 - IDEA-141192 - Venta en cuotas | FIN

}
//fin gaa20161020

function agregarFila1(booVeriConf) {
    if (agregarFila(booVeriConf, 0, false) != false) {
        var idFila = getValue('hidTotalLineas');
        cambiarTipoProducto(idFila);

        // Validación Modalidad / Operador Cedente
        if (parent.getValue('hidNTienePortabilidadValues') == 'S' && parent.getValue('ddlOperadorCedente') == "")
            parent.llenarOperadorCedente();
    }
}

function eliminarItem(idx) {
    var strCadenaDetalle = getValue('hidCadenaDetalle');
    var arrCadenaDetalle = strCadenaDetalle.split('|');

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        var id = arrCadenaDetalle[i].split(';')[0];
        if (id == idx) {
            arrCadenaDetalle.splice(i, 1);
            setValue('hidCadenaDetalle', arrCadenaDetalle.join('|'));
            parent.setValue('hidCadenaDetalle', getValue('hidCadenaDetalle'));
            break;
        }
    }
}

function eliminarItem1(strCadenaDetalle, idx) {
    var arrCadenaDetalle = strCadenaDetalle.split('|');

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        var id = arrCadenaDetalle[i].split(';')[0];
        if (id == idx) {
            arrCadenaDetalle.splice(i, 1);
            strCadenaDetalle = arrCadenaDetalle.join('|');
            break;
        }
    }
    return strCadenaDetalle;
}

function planesxCantidad(strPlanes) {
    var plan;
    var arrPlanes = strPlanes.split('|');
    var nroPlanes = 0;
    var strCadena = '';
    var planBscs;
    var plazo;

    for (var i = 0; i < arrPlanes.length; i++) {
        nroPlanes = 0;
        if (arrPlanes[i] != '') {
            for (var ii = 0; ii < arrPlanes.length; ii++) {
                if (arrPlanes[i].split(';')[1] == arrPlanes[ii].split(';')[1]) {
                    nroPlanes += parseInt(1);
                }
            }
            plan = arrPlanes[i].split(';')[1];
            planBscs = arrPlanes[i].split(';')[2];
            plazo = arrPlanes[i].split(';')[3];

            if (strCadena == '')
                strCadena = nroPlanes + ';' + plan + ';' + planBscs + ';' + plazo;
            else
                strCadena = strCadena + '|' + nroPlanes + ';' + plan + ';' + planBscs + ';' + plazo;
        }
    }
    return strCadena;
}

function planesEvaluados(strPlanDetalle) {
    var idFila = '';
    var strProducto = '';
    var strPlanes = '';
    var strPlan = '';
    var strPlanBscs = '';
    var strPlazo = '';
    var strCuota = '';
    var strCuotaMonto = '';
    var arrPlanDetalle = strPlanDetalle.split('|');

    for (var i = 0; i < arrPlanDetalle.length; i++) {
        if (arrPlanDetalle[i] != '') {
            idFila = arrPlanDetalle[i].split(';')[0];
            strProducto = arrPlanDetalle[i].split(';')[1];
            strPlan = arrPlanDetalle[i].split(';')[10];
            strPlanBscs = getValor(arrPlanDetalle[i].split(';')[9], 6);
            strPlazo = arrPlanDetalle[i].split(';')[2];
            strCuota = arrPlanDetalle[i].split(';')[28];
            strCuotaMonto = arrPlanDetalle[i].split(';')[29];

            strPlanes += '|' + strProducto;
            strPlanes += ';' + strPlan;
            strPlanes += ';' + strPlanBscs;
            strPlanes += ';' + strPlazo;
            strPlanes += ';' + strCuota;
            strPlanes += ';' + strCuotaMonto;
            strPlanes += ';' + idFila;
        }
    }
    return strPlanes;
}

function guardarItem() {
    var strCadenaDetalle = consultarItem('');
    setValue('hidCadenaDetalle', strCadenaDetalle);
    parent.setValue('hidCadenaDetalle', getValue('hidCadenaDetalle'));

    return strCadenaDetalle;
}

function consultarItem(idx) {
    var tipoProducto = getValue('hidTipoProductoActual');
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var strCadena = '';
    var strPlazo = '';
    var strPlazoTexto = '';
    var strSolucion = '';
    var strSolucionTexto = '';
    var strPaquete = '';
    var strPaqueteTexto = '';
    var strAgrupaPaquete = '';
    var strPlan = '';
    var strPlanCodigo = '';
    var strPlanTexto = '';
    var strPlanGrupoTexto = '';
    var strPlanTipo = '';
    var strCargoFijo = '';
    var strTopeConsumo = '';
    var strCampana = '';
    var strCampanaTexto = '';
    var strEquipo = '';
    var strEquipoTexto = '';
    var strMontoTopeConsumo = '';
    var strCargoFijo = '';
    var strPrecioInst = '';
    var strCFMenAlqKit = '';
    var strCFTotMensual = '';
    var strListaPrecioEquipo = '';
    var strNroCuotas = '00';
    //PROY-30166-IDEA–38863-INICIO
    var strPorcentajeCuotas = '100'; //getValue("txtPorcCuotaIni"); 
    var strMontoCuota = '0'; //getValue("txtMontoCuota"); 
    //PROY-30166-IDEA–38863-FIN
    var strEquipoEnCuotas = '';
    var strPrecioLista = '';
    var strPrecioVenta = '';
    var strNroTelefono = '';
    var strServicio = '';
    var strServicioTexto = '';
    var strTopeConsumo = '';
    var strTopeConsumoFijo = '';
    var strTopeConsumoTexto = '';
    var strFamiliaPlan = '';
    var strFecActivacionCP = '';
    var strEstadoCP = '';
    var strFlagCPPermitida = '';
    var deudaCP = '';
    var strMontoCuotaBRMS = ''; //PROY-30166-IDEA–38863
    var strTopeMontoValue = ''; //PROY-29296
    var strTopeMontoTxt = ''; //PROY-29296

    var strCadenaDetalle = getValue('hidCadenaDetalle');

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        if (idFila.length == 0)
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);

        strCadenaDetalle = eliminarItem1(strCadenaDetalle, idFila);

        var ddlCampana = document.getElementById('ddlCampana' + idFila);
        if (ddlCampana != null) {
            strCampana = ddlCampana.value;
            strCampanaTexto = obtenerTextoSeleccionado(ddlCampana);
        }

        var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
        //INICIATIVA 920
        if (ddlPlazo != null) {
            strPlazo = ddlPlazo.value.split('-')[0];
            //INICIATIVA 920
            if(parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode)
                strPlazoTexto = obtenerTextoSeleccionado(ddlPlazo).split('-')[0].trim();
            else
            strPlazoTexto = obtenerTextoSeleccionado(ddlPlazo);
        }

        var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
        if (ddlPaquete != null && ddlPaquete.value != '') {
            strPaquete = ddlPaquete.value;
            strPaqueteTexto = obtenerTextoSeleccionado(ddlPaquete);
            strAgrupaPaquete = obtenerPaqueteActual(idFila);
        }

        var ddlPlan = document.getElementById('ddlPlan' + idFila);
        if (ddlPlan != null) {
            if (codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam || codigoTipoProducto == codTipoProdFTTH) {
                strPlan = ddlPlan.value;
                strPlanTexto = obtenerTextoSeleccionado(ddlPlan);
                strPlanCodigo = ddlPlan.value;
            }
            else {
                strPlan = ddlPlan.value;
                strPlanTexto = obtenerTextoSeleccionado(ddlPlan);
                strPlanCodigo = getValor(ddlPlan.value, 0);
                strCargoFijo = getValor(ddlPlan.value, 1);
                strPlanGrupoTexto = getValor(ddlPlan.value, 5);
                strPlanTipo = getValor(ddlPlan.value, 3);
            }
        }

        if (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdFijo)
            strTopeConsumo = tieneTope(idFila);

        var ddlEquipo = document.getElementById('hidValorEquipo' + idFila);
        if (ddlEquipo != null)
            strEquipo = ddlEquipo.value;

        if (strEquipo != '') {
            var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
            if (txtTextoEquipo != null)
                strEquipoTexto = txtTextoEquipo.value;
        }

        var txtMontoTopeConsumo = document.getElementById('txtMontoTopeConsumo' + idFila);
        if (txtMontoTopeConsumo != null)
            strMontoTopeConsumo = txtMontoTopeConsumo.value;

        if (codigoTipoProducto == codTipoProdDTH) {
            var txtPrecioInst = document.getElementById('txtPrecioInst' + idFila);
            if (txtPrecioInst != null)
                strPrecioInst = txtPrecioInst.value;

            var txtCFMenAlqKit = document.getElementById('txtCFMenAlqKit' + idFila);
            if (txtCFMenAlqKit != null)
                strCFMenAlqKit = txtCFMenAlqKit.value;

            var txtCFTotMensual = document.getElementById('txtCFTotMensual' + idFila);
            if (txtCFTotMensual != null)
                strCFTotMensual = txtCFTotMensual.value;
        }
        else {
            var txtCargoFijo = document.getElementById('txtCFPlanServicio' + idFila);
            if (txtCargoFijo != null)
                strCFTotMensual = txtCargoFijo.value;
        }

        var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
        if (hidListaPrecio != null) {
            if (hidListaPrecio.value != '') {
                strListaPrecioEquipo = hidListaPrecio.value;
                strPrecioLista = hidListaPrecio.value.split('_')[3];
                strPrecioVenta = hidListaPrecio.value.split('_')[0];
            }
        }

        if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota) {
            var arrCuota = obtenerCuotaValores(idFila);
            if (arrCuota != null) {
                strNroCuotas = arrCuota[0].split('_')[0];
                strMontoCuotaBRMS = arrCuota[0].split('_')[1]; //PROY-30166-IDEA–38863
                strPorcentajeCuotas = arrCuota[1];
                strMontoCuota = arrCuota[3];
                strEquipoEnCuotas = 'S';

                var strCasoEpecial = getValor(parent.getValue('ddlCasoEspecial'), 0);
                var blnCasoEspecialCMA = (strCasoEpecial == constCETrabajadoresCMA);
                if (!blnCasoEspecialCMA)
                    strMontoCuota = arrCuota[3];

                strCFTotMensual = parseFloat(strCFTotMensual) + parseFloat(strMontoCuota);
            }
        }
        //INICIATIVA 920 INI
        if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode) {
            var arrCuota = obtenerCuotaValores(idFila);
            var cuotasSinCode = ddlPlazo.value.split('-');
            strNroCuotas = cuotasSinCode[1];
            if (arrCuota != null) {
                strNroCuotas = arrCuota[0].split('_')[0];
                strMontoCuotaBRMS = arrCuota[0].split('_')[1]; //PROY-30166-IDEA–38863
                strPorcentajeCuotas = arrCuota[1];
                strMontoCuota = arrCuota[3];
                strEquipoEnCuotas = 'S';

                var strCasoEpecial = getValor(parent.getValue('ddlCasoEspecial'), 0);
                var blnCasoEspecialCMA = (strCasoEpecial == constCETrabajadoresCMA);
                if (!blnCasoEspecialCMA)
                    strMontoCuota = arrCuota[3];

                strCFTotMensual = parseFloat(strCFTotMensual) + parseFloat(strMontoCuota);
            }
        }
        //INICIATIVA 920 FIN 
        var txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
        if (txtNroTelefono != null)
            strNroTelefono = txtNroTelefono.value;
        if (codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam || codigoTipoProducto == codTipoProdFTTH) { // FTTH
            var ddlServicio = document.getElementById('ddlServicio' + idFila);
            if (ddlServicio != null) {
                strServicio = ddlServicio.value;
                strServicioTexto = obtenerTextoSeleccionado(ddlServicio);
            }

            var montoEquipo = document.getElementById('hidMontoEquipo' + idFila).value;
            if (montoEquipo.length > 0) {
                strCFTotMensual = parseFloat(strCFTotMensual) + parseFloat(montoEquipo);
            }
        }

        var ddlTopeConsumo = document.getElementById('ddlTopeConsumo' + idFila);
        var lbxMonto = document.getElementById('idMonto' + idFila); //PROY-29296
        if (ddlTopeConsumo != null) {
            if (ddlTopeConsumo.style.display != 'none') {
                strTopeConsumoFijo = ddlTopeConsumo.value;
                strTopeConsumoTexto = obtenerTextoSeleccionado(ddlTopeConsumo);
                strTopeMontoValue = lbxMonto.value; //PROY-29296

                if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProd3Play || getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam) {

                    //INI PROY-29296
                    if (document.getElementById('lbxMonto').length !== 0) {
                        strTopeMontoTxt = getText('lbxMonto');
                        strTopeMontoTxt = strTopeMontoTxt.substring(3, strTopeMontoTxt.lastIndexOf(' - '));
                    } else {
                        strTopeMontoTxt = codValorDefectoTope.split(',')[1];
                    } //FIN PROY-29296
                }

            }
        }


        var ddlFamiliaPlan = document.getElementById('ddlFamiliaPlan' + idFila);
        if (ddlFamiliaPlan != null) {
            if (ddlFamiliaPlan.style.display != 'none') {
                parent.document.getElementById('hidFamilia').value = document.getElementById('ddlFamiliaPlan' + idFila).value; //PROY - 30748 - SEC GUARDADA //MOD RMZ																																	  
                strFamiliaPlan = ddlFamiliaPlan.value;
            }
        }

        var txtEstadoCP = document.getElementById('txtEstadoCP' + idFila);
        if (txtEstadoCP != null) {
            strEstadoCP = txtEstadoCP.value;
        }

        var txtFecActivacionCP = document.getElementById('txtFecActivacionCP' + idFila);
        if (txtFecActivacionCP != null) {
            strFecActivacionCP = txtFecActivacionCP.value;
        }

        var txtFlagCPPermitida = document.getElementById('txtFlagCPPermitida' + idFila);
        if (txtFlagCPPermitida != null) {
            strFlagCPPermitida = txtFlagCPPermitida.value;
        }

        var txtDeudaCP = document.getElementById('txtDeudaCP' + idFila);
        if (txtDeudaCP != null) {
            deudaCP = txtDeudaCP.value;
        }

        if (idx != '') {
            strCadena = '';
        }

        if (strFlagCPPermitida == '' || strFlagCPPermitida == '0') {
            strCadena += idFila + ';';
            strCadena += codigoTipoProducto + ';';
            strCadena += strPlazo + ';';
            strCadena += strPlazoTexto + ';';
            strCadena += strSolucion + ';';
            strCadena += strSolucionTexto + ';';
            strCadena += strPaquete + ';';
            strCadena += strPaqueteTexto + ';';
            strCadena += strAgrupaPaquete + ';';
            strCadena += strPlan + ';';
            strCadena += strPlanCodigo + ';';
            strCadena += strPlanTexto + ';';
            strCadena += strPlanGrupoTexto + ';';
            strCadena += strPlanTipo + ';';
            strCadena += strTopeConsumo + ';';
            strCadena += strCampana + ';';
            strCadena += strCampanaTexto + ';';
            strCadena += strEquipo + ';';
            strCadena += strEquipoTexto + ';';
            strCadena += strMontoTopeConsumo + ';';
            strCadena += strCargoFijo + ';';
            strCadena += strPrecioInst + ';';
            strCadena += strCFMenAlqKit + ';';
            strCadena += strCFTotMensual + ';';
            strCadena += strListaPrecioEquipo + ';';
            strCadena += strPrecioLista + ';';
            strCadena += strPrecioVenta + ';';
            strCadena += strEquipoEnCuotas + ';';
            strCadena += strNroCuotas + ';';
            strCadena += strPorcentajeCuotas + ';';
            strCadena += strNroTelefono + ';';
            strCadena += strMontoCuota + ';';
            strCadena += strServicio + ';';
            strCadena += strServicioTexto + ';';
            strCadena += strTopeConsumoFijo + ';';
            strCadena += strTopeConsumoTexto + ';';

            strCadena += strFamiliaPlan + ';';

            strCadena += strEstadoCP + ';';
            strCadena += strFecActivacionCP + ';';
            strCadena += deudaCP + ';'; //PROY-30166-IDEA–38863-INICIO 
            strCadena += strMontoCuotaBRMS + ';'; //PROY-29296 
            strCadena += strTopeMontoValue + ';'; //PROY-29296
            strCadena += strTopeMontoTxt; //PROY-29296

            strCadena += '|';

            if (idx != '' && idx == idFila) {
                return strCadena;
            }
        }
    }

    strCadena = strCadena + strCadenaDetalle;
    return strCadena;
}


function obtenerLineasCP() {
    var tipoProducto = getValue('hidTipoProductoActual');
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var strNroTelefono = '';
    var strResultado = '';

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        if (idFila.length == 0)
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);

        var txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
        if (txtNroTelefono != null)
            strNroTelefono = txtNroTelefono.value;

        if (strNroTelefono != null && strNroTelefono != '') {
            strResultado = strResultado + '|' + strNroTelefono;
        }
    }
    return strResultado;
}

//PROY-2X1
function obtenerDetalleCarrito() {
    var tipoProducto = getValue('hidTipoProductoActual');
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var strNroTelefono = '';
    var strResultado = '';

    for (var i = 0; i < cont; i++) {
        var cadenaItem = '';
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        if (idFila.length == 0)
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);


        var ddlCampana = document.getElementById('ddlCampana' + idFila);
        if (ddlCampana != null) {
            cadenaItem = cadenaItem + ddlCampana.value;
        }
        else {
            cadenaItem = cadenaItem + ';'
        }


        //INICIATIVA 920
        var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
        if (ddlPlazo != null) {
                cadenaItem = cadenaItem + ';' + ddlPlazo.value.split('-')[0];
        }
        else {
            cadenaItem = cadenaItem + ';'
        }

        var ddlFamiliaPlan = document.getElementById('ddlFamiliaPlan' + idFila);
        if (ddlFamiliaPlan != null) {
            cadenaItem = cadenaItem + ';' + ddlFamiliaPlan.value;
        }
        else {
            cadenaItem = cadenaItem + ';'
        }

        var ddlPlan = document.getElementById('ddlPlan' + idFila);
        if (ddlPlan != null) {
            cadenaItem = cadenaItem + ';' + ddlPlan.value;
        }
        else {
            cadenaItem = cadenaItem + ';'
        }

        var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
        if (ddlPaquete != null) {
            cadenaItem = cadenaItem + ';' + ddlPaquete.value;
        }
        else {
            cadenaItem = cadenaItem + ';'
        }

        var txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
        if (txtNroTelefono != null) {
            cadenaItem = cadenaItem + ';' + txtNroTelefono.value;
        }
        else {
            cadenaItem = cadenaItem + ';'
        }

        var ddlEquipo = document.getElementById('hidValorEquipo' + idFila);
        if (ddlEquipo != null) {
            cadenaItem = cadenaItem + ';' + ddlEquipo.value;
        }
        else {
            cadenaItem = cadenaItem + ';'
        }

        strResultado = strResultado + '|' + cadenaItem;

    }
    return strResultado;
} //PROY-2X1


function ObtenerTelefonoSMS() {
    var strCadenaDetalle = consultarItem('');
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var telefono = '';

    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            telefono += '|' + arrCadenaDetalle[i].split(';')[30];
        }
    }
    return telefono;
}

//PROY-SMS_PORTABILIDAD :: INI
function ObtenerNroTelefono() {
    var strResultado = '';

    var tipoProducto = 'Movil';
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var nroTelefono
    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        if (idFila.length == 0)
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);

        nroTelefono = document.getElementById('txtNroTelefono' + idFila);
        if (nroTelefono != null) {
            strResultado += nroTelefono.value + ';';
        }
        else {
            strResultado = strResultado + ';';
        }
    }
    return strResultado.substring(strResultado.length - 1, 0);
}
//PROY-SMS_PORTABILIDAD :: FIN
//PROY-FULLCLARO.v2-INI
function ObtenerDatosProducto() {
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tipoProducto = getValue('hidTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    var strPlan = "";
    var strCadenaPlan = "";
    var strServicio = "";
    var strCadenaServicio = "";
    var strDatosProducto = "";

    if (codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam || codigoTipoProducto == codTipoProdFTTH) {

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
            var ddlPlan = document.getElementById('ddlPlan' + idFila);
            var ddlServicio = document.getElementById('ddlServicio' + idFila);
            if (ddlPlan != null) {
                strPlan = ddlPlan.value;
                strServicio = getValor(ddlServicio.value, 0);
                strCadenaServicio += strServicio + "|";
            }
        }
        strDatosProducto = strPlan + ";" + strCadenaServicio + ";";

    }
    else {
        var strCadenaDetalle = parent.getValue('hidCadenaDetalle');
        var strCadenaPlan = obtenerPlanesFullClaro(strCadenaDetalle);
        strDatosProducto = strCadenaPlan.substring(strCadenaPlan.length - 1, 0);

    }

    return strDatosProducto;
}
// PROY-FULLCLARO.v2-FIN

//INI PROY-CAMPANA LG
function obtenerDetalleCarritoPorta() {
    var tipoProducto = getValue('hidTipoProductoActual');
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var strNroTelefono = '';
    var strResultado = '';

    for (var i = 0; i < cont; i++) {
        var cadenaItem = '';
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        if (idFila.length == 0)
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);


        var ddlCampana = document.getElementById('ddlCampana' + idFila);
        if (ddlCampana != null) {
            cadenaItem = cadenaItem + ddlCampana.value;
        }
        else {
            cadenaItem = cadenaItem + ';'
        }

        var ddlPrecio = document.getElementById('txtEquipoPrecio' + idFila);
        if (ddlPrecio != null) {
            cadenaItem = cadenaItem + ';' + ddlPrecio.value;
        }
        else {
            cadenaItem = cadenaItem + ';'
        }

        strResultado = strResultado + '|' + cadenaItem;

    }
    return strResultado;
} //INI PROY-CAMPANA LG

function obtenerPaqueteActual(idFila) {
    var strGrupoPaquete = getValue('hidNGrupoPaqueteValues');
    var strResultado = '';
    var intPosFin = strGrupoPaquete.indexOf('[' + idFila + ']');
    var intPosIni = -1;
    var nroPos;

    if (intPosFin > -1)
        intPosIni = strGrupoPaquete.substring(0, intPosFin).lastIndexOf('{');

    if (intPosIni > -1) {
        nroPos = strGrupoPaquete.substring(intPosFin).indexOf('}');
        strResultado = strGrupoPaquete.substring(intPosIni, intPosFin + nroPos + 1);
    }

    return strResultado;
}

function tieneTope(idFila) {
    var strPlanServicio = document.getElementById('hidnPlanServicioValue').value;
    var posIniPS = strPlanServicio.indexOf('*ID*' + idFila);
    var posFinPS = 0;
    var servicio = "";

    if (posIniPS > -1) {
        posFinPS = strPlanServicio.substring(posIniPS + 4).indexOf('*ID*') + 4;

        if (posFinPS == 3)
            posFinPS = strPlanServicio.length;
        else
            posFinPS += posIniPS;

        var arrPS = strPlanServicio.substring(posIniPS, posFinPS).split('|');

        for (var i = 1; i < arrPS.length; i++) {
            var arrCodDes = arrPS[i].split(';');
            var codServicio = arrCodDes[0].split('_')[3];
            if (codServicio == codConsumoExacto || codServicio == codConsumoAbierto || codServicio == codConsumoAdicional) {
                servicio = codServicio;
                break;
            } //PROY-29296
        }
    }

    return servicio;
}

function obtenerCuotaValores(idFila, strCadenaCuota) {
    //PROY 30748 F2 INI MDE
    var hidProa = document.getElementById('hidCuotaProa').value;
    if (hidProa != '') {
        document.getElementById('hidCuota').value = hidProa;
    }
    //PROY 30748 F2 FIN MDE
    var strCuota = document.getElementById('hidCuota').value;

    if (strCadenaCuota != null)
        strCuota = strCadenaCuota;

    var strValIni = '|*ID' + idFila + '*';
    var strValFin = '*/ID' + idFila + '*';
    var arrCuota;
    var intPosIni = strCuota.indexOf(strValIni);
    var intPosFin;

    if (intPosIni > -1) {
        intPosIni += strValIni.length;
        intPosFin = strCuota.indexOf(strValFin);

        strCuota = strCuota.substring(intPosIni, intPosFin);
        arrCuota = strCuota.split(';')

        return arrCuota;
    }
}

function agregarSECPendiente(strCadena) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][agregarSECPendiente()]", "Entro a la funcion agregarSECPendiente()");

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][agregarSECPendiente()] strCadena ", strCadena);

    var idFila, idProducto, tipoProducto, nroSec;

    var arrCadena = strCadena.split('#');

    var listaGeneral = arrCadena[0];
    arrGeneral = listaGeneral.split('©');

    parent.document.getElementById('ddlTipoOperacion').value = arrGeneral[1];
    parent.document.getElementById('ddlOferta').value = arrGeneral[2];
    parent.document.getElementById('hidnTipoOfertaValue').value = arrGeneral[2];
    parent.document.getElementById('ddlModalidadVenta').value = arrGeneral[5];
    parent.document.getElementById('hidListaTipoProducto').value = arrGeneral[6].split('¬')[0];
    document.getElementById('hidTotalLineas').value = arrGeneral[7];
    document.getElementById('hidNGrupoPaqueteValues').value = arrGeneral[8];

    var ddlCasoEspecial = parent.document.getElementById('ddlCasoEspecial');
    var ddlCombo = parent.document.getElementById('ddlCombo');

    llenarDatosCombo(ddlCasoEspecial, arrGeneral[6].split('¬')[1], true);
    llenarDatosCombo(ddlCombo, arrGeneral[6].split('¬')[2], true);

    if (parent.getValue('ddlOferta') == TipoProductoBusiness) {
        mostrarColumna(columnaPaquete, true);
        document.all('hidTienePaquete').value = 'S';
    }
    else {
        mostrarColumna(columnaPaquete, false);
        document.all('hidTienePaquete').value = 'N';
    }

    var listaPlan = arrCadena[1];
    arrPlan = listaPlan.split('©');

    var listaServicio = arrCadena[2];

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][agregarSECPendiente()] hidnPlanServicioValue antes ", document.getElementById('hidnPlanServicioValue').value);

    document.getElementById('hidnPlanServicioValue').value = listaServicio;

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][agregarSECPendiente()] hidnPlanServicioValue despues ", document.getElementById('hidnPlanServicioValue').value);

    for (var i = 0; i < arrPlan.length; i++) {
        if (arrPlan[i] != '') {
            var arrPlanDet = arrPlan[i].split('~');

            idFila = arrPlanDet[0];
            idProducto = arrPlanDet[1];

            //                    //gaa20160211
            //                    var strCampanaCodigo = arrPlanDet[2].split(';')[0];
            //                    if (strCampanaCodigo == consCampanaDiaEnamorados ||
            //                        strCampanaCodigo == '<%= ConfigurationManager.AppSettings["CampanaDiaEnamoradosAsociada"] %>')
            //                        continue;
            //                    //fin gaa20160211

            switch (idProducto) {
                case codTipoProdMovil: tipoProducto = 'Movil'; break;
                case codTipoProdFijo: tipoProducto = 'Fijo'; break;
                case codTipoProdBAM: tipoProducto = 'BAM'; break;
                case codTipoProdInterInalam: tipoProducto = 'InterInalam'; break; //PROY-31812 - IDEA-43340
                case codTipoProdDTH: tipoProducto = 'DTH'; break;
                case codTipoProd3Play: tipoProducto = 'HFC'; break;
                case codTipoProdFTTH: tipoProducto = 'FTTH'; break; //FTTH -agregarSECPendiente()
                case codTipoProdVentaVarios: tipoProducto = 'VentaVarios'; break;
            }

            var newRow = document.all('tblTabla' + tipoProducto).insertRow();

            oCell = newRow.insertCell();
            oCell.style.width = '20px';
            oCell.innerHTML = "<input type='hidden' id='hidNroSec" + idFila + "' name='hidNroSec" + idFila + "' value='" + nroSec + "' />";

            oCell = newRow.insertCell();
            oCell.style.width = '20px';
            oCell.innerHTML = "<img src='../../Imagenes/rechazar.gif' border='0' style='cursor:hand' alt='Eliminar Fila' onclick='eliminarFilaTotal(this, " + idFila + ", true);' />";

            estructuraGrilla(newRow, idFila, tipoProducto, true);

            var ddlCampana = document.getElementById('ddlCampana' + idFila);
            var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
            var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
            var ddlPlan = document.getElementById('ddlPlan' + idFila);
            var txtCFPlanServicio = document.getElementById('txtCFPlanServicio' + idFila);
            var txtMontoTopeConsumo = document.getElementById('txtMontoTopeConsumo' + idFila);
            var hidMontoServicios = document.getElementById('hidMontoServicios' + idFila);
            //gaa20161024
            var ddlFamiliaPlan = document.getElementById('ddlFamiliaPlan' + idFila);
            //fin gaa20161024
            if (ddlCampana != null) {
                llenarDatosCombo(ddlCampana, arrPlanDet[2], false);
                ddlCampana.selectedIndex = 0;
            }

            if (ddlPlazo != null) {
                llenarDatosCombo(ddlPlazo, arrPlanDet[3], false);
                ddlPlazo.selectedIndex = 0;
            }

            document.getElementById('hidPlazoActual').value = arrPlanDet[3].split(';')[0] + '_' + arrPlanDet[3].split(';')[1];

            if (ddlPaquete != null) {
                llenarDatosCombo(ddlPaquete, arrPlanDet[4], false);
                ddlPaquete.selectedIndex = 0;
            }

            if (ddlPlan != null) {
                llenarDatosCombo(ddlPlan, arrPlanDet[5], false);
                ddlPlan.selectedIndex = 0;
            }

            if (txtCFPlanServicio != null) {
                txtCFPlanServicio.value = arrPlanDet[6];
            }

            if (txtMontoTopeConsumo != null) {
                txtMontoTopeConsumo.value = arrPlanDet[7];
            }

            if (hidMontoServicios != null) {
                hidMontoServicios.value = calcularCFServicio(idFila);
            }
            //gaa20161024
            if (ddlFamiliaPlan != null) {
                llenarDatosCombo(ddlFamiliaPlan, arrPlanDet[8], false);
                ddlFamiliaPlan.selectedIndex = 0;
            }
            //fin gaa20161024
        }
    }

    var listaEquipo = arrCadena[3];
    arrEquipo = listaEquipo.split('©');

    for (var i = 0; i < arrEquipo.length; i++) {
        if (arrEquipo[i] != '') {
            var arrEquipoDet = arrEquipo[i].split('~');

            idFila = arrEquipoDet[0];

            var hidValorEquipo = document.getElementById('hidValorEquipo' + idFila);
            var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);

            if (txtTextoEquipo != null) {
                hidValorEquipo.value = arrEquipoDet[1].split(';')[0];
                txtTextoEquipo.value = arrEquipoDet[1].split(';')[1];
            }

            asignarEquipoPrecio(idFila, arrEquipoDet[2]);
        }
    }

    var hidCodigoTipoProductoActual = document.getElementById('hidCodigoTipoProductoActual');
    var hidTipoProductoActual = document.getElementById('hidTipoProductoActual');

    hidCodigoTipoProductoActual.value = codTipoProdMovil;
    hidTipoProductoActual.value = 'Movil';
    guardarItem();
    agregarCarrito(false);

    hidCodigoTipoProductoActual.value = codTipoProdFijo;
    hidTipoProductoActual.value = 'Fijo';
    guardarItem();
    agregarCarrito(false);

    hidCodigoTipoProductoActual.value = codTipoProdBAM;
    hidTipoProductoActual.value = 'BAM';
    guardarItem();
    agregarCarrito(false);

    hidCodigoTipoProductoActual.value = codTipoProdDTH;
    hidTipoProductoActual.value = 'DTH';
    guardarItem();
    agregarCarrito(false);

    hidCodigoTipoProductoActual.value = codTipoProd3Play;
    hidTipoProductoActual.value = 'HFC';
    guardarItem();
    agregarCarrito(false);
    //FTTH -inicio - agregarSECPendiente()
    hidCodigoTipoProductoActual.value = codTipoProdFTTH;
    hidTipoProductoActual.value = 'FTTH';
    guardarItem();
    agregarCarrito(false);
    //FTTH -fin  agregarSECPendiente()
    hidCodigoTipoProductoActual.value = codTipoProd3PlayInalam;
    hidTipoProductoActual.value = '3PlayInalam';
    guardarItem();
    agregarCarrito(false);

    hidCodigoTipoProductoActual.value = codTipoProdMovil;
    hidTipoProductoActual.value = 'Movil';

    parent.validarOperacionVV(); //PROY-140743 - INICIO

    parent.mostrarTabxOferta();
    setValue('hidFlgOrigen', 'EDIT');
    parent.consultaReglasCreditos();
    trResumenCompras.style.display = '';

    autoSizeProducto();
}

function verificarPlazo(strTipoProducto) {
    var tabla = document.getElementById('tblTabla' + strTipoProducto);
    var cont = tabla.rows.length;
    var ddlPlazo;
    var idFila;

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];

        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10); //ddlCampana
        ddlPlazo = document.getElementById('ddlPlazo' + idFila);
        //INICIATIVA 920
        if (ddlPlazo.value.length > 0)
                return '|' + ddlPlazo.value.split('-')[0] + ';' + obtenerTextoSeleccionado(ddlPlazo);
    }

    return '';
}

function calcularCFTotal() {
    var tabla = document.getElementById('tblTablaMovil');
    var cont = tabla.rows.length;

    var nCell = 12;
    var suma = 0;
    var nro;

    if (document.getElementById('hidTienePaquete').value == 'S')
        nCell++;
    //gaa20161020
    else {
        if (familiaFlag == '1')
            nCell++;
    }
    //fin gaa20161020
    if (getValue('hidTieneCuotas') == 'S')
        nCell++;

    if (parent.getValue('hidNTienePortabilidadValues') == 'S')
        nCell++;

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        nro = fila.cells[nCell].getElementsByTagName("input")[0].value;
        if (nro.length == 0)
            nro = 0;
        suma += parseFloat(nro);
    }

    document.getElementById('txtCFTotal').value = suma.toFixed(2);
}

function habilitarServicio(hab) {
    document.getElementById('btnAgregarServicio').disabled = hab;
    document.getElementById('btnQuitarServicio').disabled = hab;
    document.getElementById('btnResetServicios').disabled = hab;

    if (hab)
        document.getElementById('btnCerrarServicios').value = 'Cerrar';
    else
        document.getElementById('btnCerrarServicios').value = 'Cerrar y Guardar';
}

function borrarGrupoServicio(idFila) {
    var hidPlanServicioNoGrupo = document.getElementById('hidPlanServicioNoGrupo');
    var strPSNG = hidPlanServicioNoGrupo.value;
    var intPosIni;
    var intPosFin;
    var intPosFin1;

    intPosIni = strPSNG.indexOf('{ID' + idFila + '}');
    intPosFin = strPSNG.indexOf('{/ID' + idFila);
    intPosFin1 = strPSNG.substring(intPosFin).indexOf('}');
    intPosFin = intPosFin + intPosFin1 + 1;

    if (intPosIni > -1)
        hidPlanServicioNoGrupo.value = strPSNG.replace(strPSNG.substring(intPosIni, intPosFin), '');
}

function guardarGrupoTemp(strValor) {
    var hidPlanServicioNGTemp = document.getElementById('hidPlanServicioNGTemp');
    hidPlanServicioNGTemp.value = strValor;
}

function asignarGrupoTemp(idFila) {
    var strPlanServicioNoGrupo = document.getElementById('hidPlanServicioNoGrupo').value;
    var hidPlanServicioNGTemp = document.getElementById('hidPlanServicioNGTemp');
    var intPosIni = strPlanServicioNoGrupo.indexOf('{ID' + idFila + '}');
    var intPosFin1 = strPlanServicioNoGrupo.indexOf('{/ID' + idFila + '}');
    var intPosFin = strPlanServicioNoGrupo.substring(intPosFin1).indexOf('}') + intPosFin1 + 1;

    hidPlanServicioNGTemp.value = strPlanServicioNoGrupo.substring(intPosIni, intPosFin);
}

function guardarGrupo(idFila) {
    var hidPlanServicioNoGrupo = document.getElementById('hidPlanServicioNoGrupo');
    var strPlanServicioNGTemp = document.getElementById('hidPlanServicioNGTemp').value;

    hidPlanServicioNoGrupo.value += strPlanServicioNGTemp
}

function agregarGrupo(idFila, esNuevo) {
    if (esNuevo) {
        var linea = document.getElementById('hidLineaActual').value;
        if (idFila < 0)
            idFila = linea;

        var hidPlanServicioNoGrupo = document.getElementById('hidPlanServicioNoGrupo');
        var lbxSD = document.getElementById('lbxServiciosDisponibles1');
        var lbxSA = document.getElementById('lbxServiciosAgregados1');
        var strGrpSA;
        var strGrpSD;
        var arrCodSA;
        var arrCodSD;
        var strPSNG = hidPlanServicioNoGrupo.value;
        var strGrupoTotal = '{ID' + idFila + '}';
        var contSD = lbxSD.length;
        var contSA = lbxSA.length;

        for (var i = 0; i < contSA; i++) {
            arrCodSA = lbxSA.options[i].value.split('_');
            strGrpSA = arrCodSA[1];

            strGrupoTotal += '{' + strGrpSA + '}';

            for (var x = 0; x < contSD; x++) {
                arrCodSD = lbxSD.options[x].value.split('_');
                strGrpSD = arrCodSD[1];

                if (strGrpSA == strGrpSD && strGrpSA.length > 0) {
                    strGrupoTotal += '|' + lbxSD.options[x].value + ';' + lbxSD.options[x].text;
                    lbxSD.remove(x);
                    x--;
                    contSD--;
                }
            }

            strGrupoTotal += '{/' + strGrpSA + '}';
        }

        strGrupoTotal += '{/ID' + idFila + '}';

        borrarGrupoServicio(idFila);
        hidPlanServicioNoGrupo.value += strGrupoTotal;
        guardarGrupoTemp(strGrupoTotal);
    }
    else
        asignarGrupoTemp(idFila);
}

function reseteartblRoaming() {
    document.getElementById('rbtValDeterminado').checked = false;
    document.getElementById('rbtValIndeterminado').checked = false;
    document.getElementById('tdLblFechaDesde').style.display = 'none';
    document.getElementById('tdLblFechaHasta').style.display = 'none';
    document.getElementById('tdTxtFechaDesde').style.display = 'none';
    document.getElementById('tdTxtFechaHasta').style.display = 'none';
    setValue('txtFechaDesde', FechaActual());
    setValue('txtFechaHasta', FechaEspecifica(1));
    setVisible('btnFechaDesde', true);
    setVisible('btnFechaHasta', true);
    setEnabled('rbtValDeterminado', true, '');
    setEnabled('rbtValIndeterminado', true, '');
}

function llenarServicio(idFila) {
    var strPlanServicio = document.getElementById('hidnPlanServicioValue').value;
    var strPlanServicioNo = document.getElementById('hidPlanServicioNo').value;

    var posIniPS = strPlanServicio.indexOf('*ID*' + idFila);
    var posFinPS = 0;
    var posIniPSNo = strPlanServicioNo.indexOf('*ID*' + idFila);
    var posFinPSNo = 0;
    var lbxSD = document.getElementById('lbxServiciosDisponibles1');
    var lbxSA = document.getElementById('lbxServiciosAgregados1');

    lbxSD.length = 0;
    lbxSA.length = 0;

    if (posIniPS > -1) {
        posFinPS = strPlanServicio.substring(posIniPS + 4).indexOf('*ID*') + 4;

        if (posFinPS == 3)
            posFinPS = strPlanServicio.length;
        else
            posFinPS += posIniPS;

        var arrPS = strPlanServicio.substring(posIniPS, posFinPS).split('|');

        for (i = 1; i < arrPS.length; i++) {
            var arrCodDes = arrPS[i].split(';');
            var option = document.createElement('option');
            //-Limpiar Plazo -Fecha Activacion - Fecha Desactivacion
            var codServSelected = arrCodDes[0].split('_');
            var arrayList = new Array; ;
            var j = 0;
            if ((codServSelected.length >= 8 && getValue('hidCodigoTipoProductoActual') != codTipoProd3Play) && (codServSelected.length >= 8 && getValue('hidCodigoTipoProductoActual') != codTipoProd3PlayInalam)) {
                for (j >= 0; j <= codServSelected.length - 4; j++)
                    arrayList[j] = codServSelected[j];

                option.value = arrayList.join("_");
            }
            else
                option.value = arrCodDes[0];

            option.text = arrCodDes[1];
            lbxSA.options[i - 1] = option;

            //Verificar si es Servicio Roaming Internacional
            if (codServSelected[3] == codServRoamingI) {
                document.getElementById('tblRoamingI').style.display = 'inline';
                if (codServSelected[0] == '0') {
                    if (codServSelected[5] == codPlazoDeterminado) {
                        document.getElementById('rbtValDeterminado').checked = true;
                        document.getElementById('rbtValIndeterminado').checked = false;
                        document.getElementById('tdLblFechaDesde').style.display = '';
                        document.getElementById('tdLblFechaHasta').style.display = '';
                        document.getElementById('tdTxtFechaDesde').style.display = '';
                        document.getElementById('tdTxtFechaHasta').style.display = '';
                        setValue('txtFechaDesde', codServSelected[6]);
                        setValue('txtFechaHasta', codServSelected[7]);
                        setVisible('btnFechaDesde', false);
                        setVisible('btnFechaHasta', false);
                        setEnabled('rbtValDeterminado', false, '');
                        setEnabled('rbtValIndeterminado', false, '');
                    }
                    else {
                        document.getElementById('rbtValDeterminado').checked = false;
                        document.getElementById('rbtValIndeterminado').checked = true;
                        document.getElementById('tdLblFechaDesde').style.display = '';
                        document.getElementById('tdLblFechaHasta').style.display = 'none';
                        document.getElementById('tdTxtFechaDesde').style.display = '';
                        document.getElementById('tdTxtFechaHasta').style.display = 'none';
                        setValue('txtFechaDesde', codServSelected[6]);
                        setValue('txtFechaHasta', '');
                        setVisible('btnFechaDesde', false);
                        setVisible('btnFechaHasta', false);
                        setEnabled('rbtValDeterminado', false, '');
                        setEnabled('rbtValIndeterminado', false, '');
                    }
                }
                else {
                    if (codServSelected[5] == codPlazoDeterminado) {
                        document.getElementById('rbtValDeterminado').checked = true;
                        document.getElementById('rbtValIndeterminado').checked = false;
                        document.getElementById('tdLblFechaDesde').style.display = '';
                        document.getElementById('tdLblFechaHasta').style.display = '';
                        document.getElementById('tdTxtFechaDesde').style.display = '';
                        document.getElementById('tdTxtFechaHasta').style.display = '';
                        setValue('txtFechaDesde', codServSelected[6]);
                        setValue('txtFechaHasta', codServSelected[7]);
                        setVisible('btnFechaDesde', true);
                        setVisible('btnFechaHasta', true);
                        setEnabled('rbtValDeterminado', true, '');
                        setEnabled('rbtValIndeterminado', true, '');
                    }
                    else {
                        document.getElementById('rbtValDeterminado').checked = false;
                        document.getElementById('rbtValIndeterminado').checked = true;
                        document.getElementById('tdLblFechaDesde').style.display = '';
                        document.getElementById('tdLblFechaHasta').style.display = 'none';
                        document.getElementById('tdTxtFechaDesde').style.display = '';
                        document.getElementById('tdTxtFechaHasta').style.display = 'none';
                        setValue('txtFechaDesde', codServSelected[6]);
                        setValue('txtFechaHasta', '');
                        setVisible('btnFechaDesde', true);
                        setVisible('btnFechaHasta', true);
                        setEnabled('rbtValDeterminado', true, '');
                        setEnabled('rbtValIndeterminado', true, '');
                    }
                }
            }

        }
    }
    if (posIniPSNo > -1) {
        posFinPSNo = strPlanServicioNo.substring(posIniPSNo + 4).indexOf('*ID*') + 4;

        if (posFinPSNo == 3)
            posFinPSNo = strPlanServicioNo.length;
        else
            posFinPSNo += posIniPSNo;

        var arrPSNo = strPlanServicioNo.substring(posIniPSNo, posFinPSNo).split('|');

        for (i = 1; i < arrPSNo.length; i++) {
            var arrCodDes = arrPSNo[i].split(';');
            var option = document.createElement('option');
            option.value = arrCodDes[0];
            option.text = arrCodDes[1];
            lbxSD.options[i - 1] = option;
        }
    }

    agregarGrupo(idFila, false);
    if ((getValue('hidCodigoTipoProductoActual') == codTipoProd3Play) || (getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam))
        llenarPromocion(idFila, false);
}

function llenarPromocion(idFila, desdeTemp) {
    var strPromocion;

    if (!desdeTemp)
        strPromocion = document.getElementById('hidNPromocionValues').value;
    else
        strPromocion = document.getElementById('hidPromocionTemp').value;

    var posIniPS = strPromocion.indexOf('*ID*' + idFila);
    var posFinPS = 0;
    var lbxSA = document.getElementById('lbxPromocionesAgregadas');

    lbxSA.length = 0;

    if (posIniPS > -1) {
        posFinPS = strPromocion.substring(posIniPS + 4).indexOf('*ID*') + 4;

        if (posFinPS == 3)
            posFinPS = strPromocion.length;
        else
            posFinPS += posIniPS;

        var arrPS = strPromocion.substring(posIniPS, posFinPS).split('|');

        for (i = 1; i < arrPS.length; i++) {
            var arrCodDes = arrPS[i].split(';');
            var option = document.createElement('option');
            option.value = arrCodDes[0];
            option.text = arrCodDes[1];
            lbxSA.options[i - 1] = option;
        }
    }
}

function asignarPromocion3PlayTemp(idFila) {
    var strSrvSel = '';
    var strPlanCodigo = document.getElementById('ddlPlan' + idFila).value;
    var strCodServ = strPlanCodigo.split('_')[0];

    setValue('hidPromocionTemp', '');

    strSrvSel += ';' + strCodServ;

    strSrvSel += extraerCodigoServicioTemp();

    asignarPromocion3Play1(idFila, strSrvSel, true);

    llenarPromocion(idFila, true);
}

function extraerCodigoServicioTemp() {
    var lbxSA = document.getElementById('lbxServiciosAgregados1');
    var strResultado = '';

    for (var i = 0; i < lbxSA.length; i++) {
        strResultado += ';' + lbxSA.options[i].value.split('_')[3];
    }

    return strResultado;
}

function mostrarServicio(idFila) {
    //    INICIATIVA - 733 - INI - C3
    //    Agregar el ID de la fila a la variable global
    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
        VallineaAdicional = idFila;
    }
    //    INICIATIVA - 733 - FIN - C3

    if (document.getElementById('hidValidarGuardarCuota').value.length > 0) {
        alert('Debe guardar las cuotas antes de ejecutar esta acción');
        return;
    }

    setValue('hidFilaSeleccionada', idFila); //PROY-29296

    var strPlanServicio = document.getElementById('hidnPlanServicioValue').value;
    var strPlanServicioNo = document.getElementById('hidPlanServicioNo').value;
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var ddlServicio = document.getElementById('ddlServicio' + idFila);
    var strPlan = ddlPlan.value;
    var ddlCampana = document.getElementById('ddlCampana' + idFila);

    //- Oculta y resetear Tabla para Servicio Roaming
    document.getElementById('tblRoamingI').style.display = 'none';
    reseteartblRoaming();

    if (strPlan.length > 0) {
        //    INICIATIVA - 733 - INI - C4
        if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
            if (ContieneDecoIPTV == true) {
                document.getElementById('tblServicios').style.display = 'none';
                //            document.getElementById('tblEquipos').style.display = 'inline';
            } else {
                document.getElementById('tblServicios').style.display = 'inline';
            }
        } else {
            document.getElementById('tblServicios').style.display = 'inline';
        }
        //    INICIATIVA - 733 - FIN - C4
//        document.getElementById('tblServicios').style.display = 'inline';
        if ((getValue('hidCodigoTipoProductoActual') == codTipoProd3Play) || (getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam)) {
            var strIdProducto = ddlPlan.value.split('.')[1];
            if (strIdProducto == IdProductoServicioEmail) {
                document.getElementById('tblServicios').style.display = 'none';
                return;
            }
            habilitarServicio(ddlServicio.disabled);
        }
        else {
            var imgEditarFila = document.getElementById('imgEditarFila' + idFila);
            if (imgEditarFila != null)
                habilitarServicio(false);
            else
                habilitarServicio(true);
        }

        document.getElementById('hidLineaActual').value = idFila;
        document.getElementById('lblIdLista').innerHTML = obtenerTextoSeleccionado(ddlPlan);

        if (strPlanServicio.indexOf('*ID*' + idFila) > -1 || strPlanServicioNo.indexOf('*ID*' + idFila) > -1)
            llenarServicio(idFila);

        cerrarCuota();
        cerrarLineasCuentas(); //PROY-140743
        //        cerrarEquipo();
        //    INICIATIVA - 733 - INI - C5
        if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
            if (ContieneDecoIPTV == false) {
                cerrarEquipo();
            }
        } else {
            cerrarEquipo();
        }
        //    INICIATIVA - 733 - FIN - C5

        if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProd3Play || getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam) {

            //INI PROY-29296
            var lbxSA = document.getElementById('lbxServiciosAgregados1');
            var contSA = lbxSA.options.length;
            var arrCodTopesCons = new Array();
            var strCodTopesCons = codConsumoAdicional + '-' + codConsumoAbierto + '-' + codConsumoExacto;
            var iContador = 0;
            for (var i = 0; i < contSA; i++) {
                arrCodTopesCons = strCodTopesCons.split('-');
                for (var j = 0; j < arrCodTopesCons.length; j++) {
                    if (lbxSA.options[i].value.split('_')[3] == arrCodTopesCons[j]) {
                        iContador++;
                    }
                }
            }
            (iContador == 0) ? BloqueaDesbloqueaTopesConsumo(false) : BloqueaDesbloqueaTopesConsumo(true);
            //INI PROY-29296
        }
    }
    else
        cerrarServicio();

    cerrarLineasCuentas(); //PROY-140743
    parent.autoSizeIframe();
}

function mostrarServicioGuardado(idFila) {
    var strPlan = getValue('hidCodPlan1');
    cerrarCuota();
    cerrarLineasCuentas(); //PROY-140743
    document.getElementById('tblServicios').style.display = 'none';

    var strPlanServicio = document.getElementById('hidnPlanServicioValue').value;
    if (strPlanServicio.indexOf('*ID*' + idFila) > -1) {
        llenarServicio(idFila);
        document.getElementById('tblServicios').style.display = 'inline';
    }

    document.getElementById('lblIdLista').innerHTML = obtenerTextoSeleccionado(document.getElementById('ddlPlan' + idFila));
    if ((getValue('hidCodigoTipoProductoActual') == codTipoProd3Play) || (getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam)) {
        var ddlPlan = document.getElementById('ddlPlan' + idFila);
        var strPlan = ddlPlan.value;
        var strIdProducto = ddlPlan.value.split('.')[1];
        if (strIdProducto == IdProductoServicioEmail) {
            document.getElementById('tblServicios').style.display = 'none';
            return;
        }

        document.getElementById('tblServicios').style.display = 'inline';
    }

    habilitarServicio(true);
    habilitarCuota(idFila);

    parent.autoSizeIframe();
}
//INI PROY-29296
function LlenarTablaMontosTopeConsumo(codServicio, idPlan) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
    url += '&strCodServicio=' + codServicio;
    url += '&strPlan=' + idPlan;
    url += '&strMetodo=' + 'LlenarMontosTopeConsumo';

    self.frames['iframeAuxiliar'].location.replace(url);
}

function LlenarTablaMontosTopeConsumoLTE(codServicio) {

    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
    url += '&strCodServicio=' + codServicio.value;
    url += '&strPlan=' + getValue('ddlPlan' + getValue('hidFilaSeleccionada'));
    url += '&strMetodo=' + 'LlenarMontosTopeConsumoLTE';

    self.frames['iframeAuxiliar'].location.replace(url);
}

function listarMontosXTopeConsumo(strValor) {
    var arrError = strValor.split('~');
    if (arrError.length > 1) {
        alert(arrError[0]);
    } else {
        var arrValor = strValor.split('¬');
        var lbxTCD = document.getElementById('lbxTopeConsumoDisponibles');

        for (var i = 0; i < arrValor.length - 1; i++) {
            var arrMontoXTope = arrValor[i].split(';');

            var option = document.createElement('option');
            option.value = arrMontoXTope[0];
            option.text = 'S/.' + arrMontoXTope[4] + ' - ' + arrMontoXTope[5] + ' min.';

            lbxTCD.options[i] = option;
        }

        document.getElementById('tbTopeConsumos').style.display = 'inline';
        document.getElementById('btnAgregarServicio').disabled = true;
        document.getElementById('btnQuitarServicio').disabled = true;
        document.getElementById('btnResetServicios').disabled = true;
        document.getElementById('btnCerrarServicios').disabled = true;
        document.getElementById('lbxServiciosDisponibles1').disabled = true;

        if (lbxTCD.options.length > 0) lbxTCD.selectedIndex = 0;
    }

    parent.autoSizeIframe();
}

function listarMontosXTopeConsumoLTE(strValor) {
    var arrValor = strValor.split('¬');
    var lbxTCD = document.getElementById('lbxMonto');
    var length = lbxTCD.options.length;
    document.getElementById('lbxMonto').length = 0;
    for (var i = 0; i < arrValor.length - 1; i++) {
        var arrMontoXTope = arrValor[i].split(';');

        var option = document.createElement('option');
        option.value = arrMontoXTope[0];
        option.text = 'S/.' + arrMontoXTope[4] + ' - ' + arrMontoXTope[5] + ' min.';

        lbxTCD.options[i] = option;
    }

    /*document.getElementById('btnAgregarServicio').disabled = true;
    document.getElementById('btnQuitarServicio').disabled = true;
    document.getElementById('btnResetServicios').disabled = true;
    document.getElementById('btnCerrarServicios').disabled = true;*/


    if (lbxTCD.options.length > 0) lbxTCD.selectedIndex = 0; if (vbCargaIni == false) guardarTopeConsumoLTE(); vbCargaIni = true;

    parent.autoSizeIframe();
}
        //PROY-140383-INI
        function ServiciosExcluyentes_CallBack(objResponse) {
            if (objResponse.Cadena != null) {
                var strResp = objResponse.Cadena;
                setValue('hidServExcluyentes', strResp);              
            } else {
                var strMensServCaido = getValue('hidServicioExcCaidoifr');
                setValue('hidServExcluyentes', '');   
                setValue('hidMensServCaido', strMensServCaido);
            }
            validarServicio();

        }
        
        function ValidarServicioExcluyentes() {

            var hidFlagServvExcluyentes = getValue('hidFlagServvExcluyentes');
            var CodProductosExc = getValue('hidProductosExc'); 
            var codTipoProductoActual = getValue('hidCodigoTipoProductoActual');
            if (hidFlagServvExcluyentes == '1') {

                if (CodProductosExc.indexOf(codTipoProductoActual) > -1) {

                    var lbxServA = document.getElementById('lbxServiciosAgregados1');
                    var lbxServD = document.getElementById('lbxServiciosDisponibles1');
                    var strCodServAgre = '';
                    var contServA = lbxServA.options.length;
                    var contServD = lbxServD.options.length;
                    var codServicio;

                    //    INICIATIVA - 733 - INI - C6
                    // Valida si esta en la lista adicional claro video y lo selecciona.
                    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
                        if (ContieneDecoIPTV == true) {
                            var CodClaroVideo;
                            for (var i = 0; i < contServD; i++) {
                                //                            if (lbxServD.options[i].selected) {
                                CodClaroVideo = lbxServD.options[i].value.split('_')[3];
                                //                            if (CodClaroVideo == "351") {
                                if (CodClaroVideo == Key_CodClaroVideoIPTV) {
                                    lbxServD.options[i].selected = true;
                                }
                                //                            }
                            }
                        }
                    }
                    //    INICIATIVA - 733 - FIN - C6

                    for (var j = 0; j < contServD; j++) {
                        if (lbxServD.options[j].selected) {
                            codServicio = lbxServD.options[j].value.split('_')[3];
                        }
                    }

                    for (var i = 0; i < contServA; i++) {
                        var CodServAgre = lbxServA.options[i].value.split("_")[3];
                        strCodServAgre += CodServAgre + '|';
                    }

                    

                    PageMethods.ServiciosExcluyentes(codServicio, strCodServAgre, ServiciosExcluyentes_CallBack);

                } else {
                    validarServicio();
                }
            } else {
                validarServicio();
            }          
        }
        //PROY-140383-FIN

function validarServicio() {
    var lbxSD = document.getElementById('lbxServiciosDisponibles1');
    var contSD = lbxSD.options.length;
    var codServicio = '';

    //    INICIATIVA - 733 - INI - C7
    // Valida si esta en la lista adicional claro video y lo selecciona.
    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
        if (ContieneDecoIPTV == true) {
            var CodClaroVideo;
            for (var i = 0; i < contSD; i++) {
                CodClaroVideo = lbxSD.options[i].value.split('_')[3];
                if (CodClaroVideo == Key_CodClaroVideoIPTV) {
                    lbxSD.options[i].selected = true;
                }
            }
        }
    }
    //    INICIATIVA - 733 - FIN - C7

    for (var i = 0; i < contSD; i++) {
        if (lbxSD.options[i].selected) {
            codServicio = lbxSD.options[i].value.split('_')[3];
        }
    }

    if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProd3Play || getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam) {

        if (codServicio == codConsumoAdicional || codServicio == codConsumoAbierto || codServicio == codConsumoExacto) {

            var idFila = getValue('hidFilaSeleccionada');
            var ddlPlan = document.getElementById('ddlPlan' + idFila);
            var strPlanCodigo = getValor(ddlPlan.value, 0);

            LlenarTablaMontosTopeConsumo(codServicio, strPlanCodigo);
        } else {
            agregarServicio();
        }
    } else {
        agregarServicio();
    }
    /* FIN */

    //INI-INC000002245048 AGREGAR
    var p_lbxSA = document.getElementById('lbxServiciosAgregados1');
    var p_lbxSD = document.getElementById('lbxServiciosDisponibles1');
    var p_lbxServiciosDisponibles1 = new Array();
    var p_lbxServiciosAgregados1 = new Array();
    for (var i = 0; i < p_lbxSD.options.length; i++) {
        p_lbxServiciosDisponibles1.push(p_lbxSD.options[i].value + ";" + p_lbxSD.options[i].text);
    }
    for (var i = 0; i < p_lbxSA.options.length; i++) {
        p_lbxServiciosAgregados1.push(p_lbxSA.options[i].value + ";" + p_lbxSA.options[i].text);
    }
    var Accion = "AGREGAR"
    PageMethods.MostrarLogs("*ID*1|" + p_lbxServiciosAgregados1.join("|"), "*ID*1|" + p_lbxServiciosDisponibles1.join("|"), parent.getValue('txtNroDoc'), Accion);
    //FIN-INC000002245048 AGREGAR
}

function mostrarTopesLTE(idFila) {
    var lbxSD = document.getElementById('ddlTopeConsumo' + idFila);
    var lbxIdTope = document.getElementById('lbxIdTope');
    var contSD = lbxSD.options.length;
    var codServicio = '';
    setValue('hidFilaSeleccionada', idFila);
    for (var i = 0; i < contSD; i++) {
        if (lbxSD.options[i].selected) {
            codServicio = lbxSD.options[i].value;
            lbxIdTope.value = lbxSD.options[i].value;
            document.getElementById('tbTopeConsumosLTE').style.display = 'inline';
            if (vbCargaIni == false) {
                document.getElementById('tbTopeConsumosLTE').style.display = 'none';
            }
            LlenarTablaMontosTopeConsumoLTE(lbxIdTope);
        }
    }
}

function BloqueaDesbloqueaTopesConsumo(bool) {
    /* aquí debería ocultar los topes NO seleccionados*/
    var arrCodTopesCons = new Array();
    var strCodTopesCons = codConsumoAdicional + '-' + codConsumoAbierto + '-' + codConsumoExacto;
    var lbxSD = document.getElementById('lbxServiciosDisponibles1');
    var contSD = lbxSD.options.length;

    for (var i = 0; i < contSD; i++) {
        arrCodTopesCons = strCodTopesCons.split('-');
        for (var j = 0; j < arrCodTopesCons.length; j++) {
            if (lbxSD.options[i].value.split("_")[3] == arrCodTopesCons[j]) {
                lbxSD.options[i].disabled = bool;
            }
        }
    }
}

function guardarTopeConsumo() {

    agregarServicio();

    document.getElementById('tbTopeConsumos').style.display = 'none';
    document.getElementById('lbxTopeConsumoDisponibles').length = 0;
    document.getElementById('btnAgregarServicio').disabled = false;
    document.getElementById('btnQuitarServicio').disabled = false;
    document.getElementById('btnResetServicios').disabled = false;
    document.getElementById('btnCerrarServicios').disabled = false;
    document.getElementById('lbxServiciosDisponibles1').disabled = false;

    BloqueaDesbloqueaTopesConsumo(true);

    parent.autoSizeIframe();
}

function guardarTopeConsumoLTE() {

    document.getElementById('tbTopeConsumosLTE').style.display = 'none';
    document.getElementById('lbxTopeConsumoDisponibles').length = 0;
    document.getElementById('btnAgregarServicio').disabled = false;
    document.getElementById('btnQuitarServicio').disabled = false;
    document.getElementById('btnResetServicios').disabled = false;
    document.getElementById('btnCerrarServicios').disabled = false;

    var idTope = document.getElementById('ddlTopeConsumo' + getValue('hidFilaSeleccionada'));
    var idTopeTb = document.getElementById('lbxIdTope');
    idTope.value = idTopeTb.value;

    for (var i = 0; i < idTope.length; i++) {
        idTope.options[i].text = idTopeTb.options[i].text;
    }

    idTope.options[idTope.selectedIndex].text = idTopeTb.options[idTopeTb.selectedIndex].text + ' - ' + getText('lbxMonto');
    crearControlIdMonto("otros");
    parent.autoSizeIframe();
}

function crearControlIdMonto(val) {
    if (document.getElementById("idMonto" + getValue('hidFilaSeleccionada')) != null) {
        var elem = document.getElementById("idMonto" + getValue('hidFilaSeleccionada'));
        elem.parentNode.removeChild(elem);
    }

    var input = document.createElement("input");
    var theform = document.forms["frmPrincipal2"];
    input.setAttribute("type", "hidden");
    input.setAttribute("id", "idMonto" + getValue('hidFilaSeleccionada'));
    input.setAttribute("name", "idMonto" + getValue('hidFilaSeleccionada'));
    if (val == "Exacto") {
        input.setAttribute("value", codValorDefectoTope.split(',')[0]);
    } else {
        input.setAttribute("value", getValue('lbxMonto'));
    }
    theform.appendChild(input);
}
//FIN PROY-29296
//gaa20161025
//function agregarServicio() {
function agregarServicio(strServicio) {
    //fin gaa20161025
    var lbxSD = document.getElementById('lbxServiciosDisponibles1');
    var lbxSA = document.getElementById('lbxServiciosAgregados1');
    var contSD = lbxSD.options.length;
    var contSA = lbxSA.options.length;
    var arrCodigo;
    var strGrupo;
    var linea = document.getElementById('hidLineaActual').value;
    var strFinLinea = '{/ID' + linea + '}';
    var hidPlanServicioNGTemp = document.getElementById('hidPlanServicioNGTemp');
    var strPlanServicioNGTemp = hidPlanServicioNGTemp.value;
    var strGrupoResultado = '';
    var strGrupo1 = '';
    var strGrupo2 = '';
    //PROY-140383-INI
    var strServExcluyentes = '';
    var strServExContra = '0';
    var strRespExc = '';
    var strposRemov = '';
    var CodProductosExc = getValue('hidProductosExc');
    //PROY-140383-FIN

    for (var i = 0; i < contSD; i++) {
        //gaa20161025: Permite agregar un servicio por defecto 
        var codServicio = lbxSD.options[i].value.split("_")[3]; //PROY-29296
        //if (lbxSD.options[i].selected) {
        if (lbxSD.options[i].selected || (strServicio != null && codServicio == strServicio)) {
            //fin gaa20161025
            var option = document.createElement('option');

                        //PROY-140383-INI                  
                        var strRespServExc = getValue('hidServExcluyentes');
                        var arrRespServ = strRespServExc.split("_");
                        var strServContExc = getValue('hidServExCont');
                        var strServContExcTemp = "";
                        strRespExc = arrRespServ[0];
                        if (strRespExc == '1') {                                                 
                            
                            for (var l = 0; l < contSA; l++) {
                                 var arrSerExc = arrRespServ[1].split("|");
                                 for (var n = 0; n < arrSerExc.length; n++) {
                                     if (lbxSA.options[l].value.split("_")[3] == arrSerExc[n]) {
                                         if (lbxSA.options[l].text.indexOf('(*)') == -1) {
                                             strServExcluyentes += lbxSA.options[l].value + ';' + lbxSA.options[l].text + '|';
                                             strposRemov += l + '_';
                                             
                                             if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProdDTH || getValue('codTipoProdInterInalam') == codTipoProdInterInalam) {
                                                 strServContExcTemp += lbxSA.options[l].value.split("_")[3] + ',' + lbxSA.options[l].value.split("_")[1] + '|';
                                             } else {
                                                 strServContExcTemp += lbxSA.options[l].value.split("_")[3] + ',' + lbxSA.options[l].value.split("_")[7] + '|';
                                             }
                                         } else {
                                             strServExContra = '1';
                                         }
                                     }
                                }
                             }

                             if (strServContExc.length > 0) {
                                 var arrServContExcTemp = strServContExcTemp.split('|');
                                 for (var n = 0; n < arrServContExcTemp.length; n++) {
                                     if (arrServContExcTemp[n].length > 0) {
                                         if (strServContExc.indexOf(arrServContExcTemp[n].split(',')[0]) == -1) {
                                             strServContExc += arrServContExcTemp[n] + '|';
                                         }
                                     }
                                 }
                             } else {
                                strServContExc += strServContExcTemp;
                             }

                             setValue('hidServExCont', strServContExc);
                             var arrPosRem = strposRemov.split("_");
                             var contSAtem = lbxSA.options.length;
                             for (var m = 0; m < arrPosRem.length; m++) {
                                 if (arrPosRem[m] != "") {
                                     lbxSA.remove(arrPosRem[m]);
                                     contSA--;
                                 }
                             }                           
                        }                    
                        //PROY-140383-FIN               

            if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProd3Play || getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam) {

                //INI PROY-29296
                if (codServicio == codConsumoAdicional || codServicio == codConsumoAbierto || codServicio == codConsumoExacto) {
                    var lbxTCD = document.getElementById('lbxTopeConsumoDisponibles');
                    var lbTCDValor = '';
                    var lbTCDTexto = '';

                    for (var j = 0; j < lbxTCD.options.length; j++) {
                        if (lbxTCD.options[j].selected) {
                            lbTCDValor = lbxTCD.options[j].value.split(';')[0];
                            lbTCDTexto = lbxTCD.options[j].text;
                        }
                    }

                    option.value = lbxSD.options[i].value + '_' + lbTCDValor;


                    option.text = lbxSD.options[i].text + ' ~ ' + lbTCDTexto;

                    //option.text = lbxSD.options[i].text;

                } else {
                    option.value = lbxSD.options[i].value;
                    option.text = lbxSD.options[i].text;
                }
            } else {
                option.value = lbxSD.options[i].value;
                option.text = lbxSD.options[i].text;
            }
            //FIN PROY-29296
                    //PROY-140383-INI
                    if (strRespExc == '1') {
                        if (strServExContra == '1') {
                            contSA--;
                            var mensajeServExc = getValue('hidMensValidacion');
                            alert(mensajeServExc);
                        } else {
            lbxSA.options[contSA] = option;
                            lbxSD.remove(i);
                            i--;
                            contSD--;
                        }
                    } else {
            lbxSA.options[contSA] = option;
                        lbxSD.remove(i);
                        i--;
                        contSD--;
                    }
                    //PROY-140383-FIN

            arrCodigo = lbxSA.options[contSA].value.split('_');
            strGrupo = arrCodigo[1];
                    contSA++;
                    //PROY-140383-INI
                    
                    if (strRespExc == '1') {
                        var arrInsExclu = strServExcluyentes.split("|");
                        for (var y = 0; y < arrInsExclu.length; y++) {
                             if (arrInsExclu[y] != '') {
                                var optionExc = document.createElement('option');
                                optionExc.value = arrInsExclu[y].split(";")[0];
                                optionExc.text = arrInsExclu[y].split(";")[1];
                                lbxSD.options[contSD] = optionExc;
                                contSD++;
                             }
                        }
                    }                                 
                    //PROY140383-FIN


            asignarPromocion3PlayTemp(linea);

            if (arrCodigo[3] == codServProteccionMovil) //PROY-24724-IDEA-28174 - INICIO
                strServicioProteccionMovil = lbxSA.options[contSA - 1].value + ';' + lbxSA.options[contSA - 1].text; //PROY-24724-IDEA-28174 - FIN
            //gaa20161025
            break;
            //fin gaa20161025
        }
    }
            if (getValue('hidFlagServvExcluyentes') == '1') {
                if (CodProductosExc.indexOf(getValue('hidCodigoTipoProductoActual')) == -1) {
                    for (var i = 0; i < contSD; i++) {
                        arrCodigo = lbxSD.options[i].value.split('_');
                        strGrupo1 = arrCodigo[1];

                        if (strGrupo == strGrupo1 && strGrupo.length > 0) {
                            if (strGrupoResultado.length == 0) {
                                strGrupo2 = strGrupo1
                                strGrupoResultado = '{' + strGrupo1 + '}';
                            }
                            strGrupoResultado += '|' + lbxSD.options[i].value + ';' + lbxSD.options[i].text;
                            lbxSD.remove(i);
                            i--;
                            contSD--;
                        }
                    }
                }
            } else {
    for (var i = 0; i < contSD; i++) {
        arrCodigo = lbxSD.options[i].value.split('_');
        strGrupo1 = arrCodigo[1];

        if (strGrupo == strGrupo1 && strGrupo.length > 0) {
            if (strGrupoResultado.length == 0) {
                strGrupo2 = strGrupo1
                strGrupoResultado = '{' + strGrupo1 + '}';
            }
            strGrupoResultado += '|' + lbxSD.options[i].value + ';' + lbxSD.options[i].text;
            lbxSD.remove(i);
            i--;
            contSD--;
        }
    }
            }


    if (strGrupoResultado.length > 0)
        strGrupoResultado += '{/' + strGrupo2 + '}';

    strPlanServicioNGTemp = strPlanServicioNGTemp.replace(strFinLinea, strGrupoResultado + strFinLinea);
    hidPlanServicioNGTemp.value = strPlanServicioNGTemp;

    lbxSD.selectedIndex = -1;
    lbxSA.selectedIndex = -1;
}

function quitarServicio(strParametro) {
    var lbxSD = document.getElementById('lbxServiciosDisponibles1');
    var lbxSA = document.getElementById('lbxServiciosAgregados1');
    var contSD = lbxSD.options.length;
    var contSA = lbxSA.options.length;
    var strGrupo = '';
    var hidPlanServicioNGTemp = document.getElementById('hidPlanServicioNGTemp');
    var strPlanServicioNGTemp = hidPlanServicioNGTemp.value;
    var intPosIni;
    var intPosFin;
    var intPosFin2;
    var arrServicios;
    var arrValores;
    var arrCodigo;
    var linea = document.getElementById('hidLineaActual').value;
    //gaa20161025
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    var codTopeAutomatico = constCodTopeAutomaticoServicio;
    var codTopeCero = constCodTopeCeroServicio;
    var strServicioTope = '';
    var codFamiliaLocal = FamiliaPlanLocal;
    //fin gaa20161025

    //INICIATIVA - 733 - INI - C12
    //Quitar Claro video cuando es IPTV
    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
        var DecoIPTVAgregado = false;
        if (ContieneDecoIPTV == true) {
            var CodClaroVideo;
            for (var i = 0; i < contSA; i++) {
                CodClaroVideo = lbxSA.options[i].value.split('_')[3];
                if (CodClaroVideo == Key_CodClaroVideoIPTV) {
                    lbxSA.options[i].selected = true;
                }
            }
        } else {

            var tabla = document.getElementById('tblResumenAlqEquipo3Play');
            var cont = tabla.rows.length;
            var CodigoEquipoIPTV = Key_CodEquipoIPTV.split('|');
            //        var CodigoEquipoIPTV = ["4236", "4235", "4759"];
            for (var i = 0; i < contSA; i++) {
                CodClaroVideo = lbxSA.options[i].value.split('_')[3];
                if (CodClaroVideo == Key_CodClaroVideoIPTV && lbxSA.options[i].selected == true) {

                    for (var j = 1; j < cont; j++) {
                        fila = tabla.rows[j];
                        var Codigo = fila.cells[0].getElementsByTagName("input")[1].value;

                        for (var k = 0; k < CodigoEquipoIPTV.length; k++) {

                            if (CodigoEquipoIPTV[k] == Codigo) {
                                DecoIPTVAgregado = true;
                            }
                        }
                    }
                }
            }
        }

        if (DecoIPTVAgregado == true) {
            alert(Key_strMensajeClaroVideoIPTV);
            return;
        }
    }
    //INICIATIVA - 733 - FIN - C12

    for (var i = 0; i < contSA; i++) {
        if (lbxSA.options[i].selected) {
            arrCodigo = lbxSA.options[i].value.split('_');
            var codServSelected = arrCodigo[3];
            if (codServSelected == codServRoamingI) {
                document.getElementById('tblRoamingI').style.display = 'none';
                reseteartblRoaming();
            }
            //Si SELECCIONADO_OBLIGATORIO es igual a vacio
            //if (arrCodigo[2].length == 0)
            //Si SELECCIONADO_OBLIGATORIO no aparece
            if (arrCodigo[2] != '2') {
                var option = document.createElement('option');

                        if (codServSelected == codConsumoAdicional || codServSelected == codConsumoAbierto || codServSelected == codConsumoExacto) { //PROY-140383

                if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProd3Play || getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam) {

                    // INI-PROY-29296
                    var valueServicio = lbxSA.options[i].value;
                    var textoServicio = lbxSA.options[i].text;
                    var intPosFin1 = valueServicio.lastIndexOf('_');
                    var intPosFin2 = textoServicio.lastIndexOf(' ~ ');
                    if (arrCodigo[5] !== undefined) {
                        option.value = valueServicio.substring(0, intPosFin1);
                        option.text = textoServicio.substring(0, intPosFin2);
                    } else {
                        option.value = valueServicio;
                        option.text = textoServicio;
                    } // FIN-PROY-29296
                            //PROY-140383-INI
                            } else {
                                option.value = lbxSA.options[i].value;
                                option.text = lbxSA.options[i].text;
                            }
                            //PROY-140383-FIN
                } else {
                    option.value = lbxSA.options[i].value;
                    option.text = lbxSA.options[i].text;
                }


                lbxSD.options[contSD] = option;

                strGrupo = arrCodigo[1];

                lbxSA.remove(i);
                contSD++;
                i--;
                contSA--;

                //asignarPromocion3PlayTemp(linea);
                //gaa20161025
                if (document.getElementById('tdFamiliaPlanMovil').style.display != 'none' && codigoTipoProductoActual == codTipoProdMovil) {
                    if (document.getElementById('ddlFamiliaPlan' + linea).value == codFamiliaLocal && strParametro != 'r') {
                        if (codServSelected == codTopeAutomatico)
                            strServicioTope = codTopeCero;
                        else {
                            if (codServSelected == codTopeCero)
                                strServicioTope = codTopeAutomatico;
                        }
                    }
                }

                break;
                //fin gaa20161025
            }
        }
    }

    if (strGrupo.length > 0) {
        intPosIni = strPlanServicioNGTemp.indexOf('{' + strGrupo + '}');

        if (intPosIni > -1) {
            intPosFin = strPlanServicioNGTemp.indexOf('{/' + strGrupo + '}');
            intPosFin2 = strPlanServicioNGTemp.substring(intPosFin).indexOf('}') + intPosFin;

            arrServicios = strPlanServicioNGTemp.substring(intPosIni, intPosFin).split('|');

            for (var i = 1; i < arrServicios.length; i++) {
                arrValores = arrServicios[i].split(';');

                var option = document.createElement('option');
                option.value = arrValores[0];
                option.text = arrValores[1];
                lbxSD.options[contSD] = option;
                contSD++;
            }

            hidPlanServicioNGTemp.value = strPlanServicioNGTemp.replace(strPlanServicioNGTemp.substring(intPosIni, intPosFin2), '');
        }
    }

    //document.getElementById('lbxPromocionesDisponibles').innerHTML = "";
    //document.getElementById('lbxPromocionesSeleccionadas').innerHTML = "";
    lbxSD.selectedIndex = -1;
    lbxSA.selectedIndex = -1;
    //gaa20161025
    if (strServicioTope.length > 0)
        agregarServicio(strServicioTope);
    //fin gaa20161025

    if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProd3Play || getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam) {
        //INI-29296
        var arrCodTopesCons = new Array();
        var strCodTopesCons = codConsumoAdicional + '-' + codConsumoAbierto + '-' + codConsumoExacto;
        var iContador = 0;
        for (var i = 0; i < contSA; i++) {
            arrCodTopesCons = strCodTopesCons.split('-');
            for (var j = 0; j < arrCodTopesCons.length; j++) {
                if (lbxSA.options[i].value.split('_')[3] == arrCodTopesCons[j]) {
                    iContador++;
                }
            }
        }
        if (iContador == 0) BloqueaDesbloqueaTopesConsumo(false);  //FIN-29296
    }
    //INI-INC000002245048 QUITAR
    if (strParametro != 'r') {
        var p_lbxSA = document.getElementById('lbxServiciosAgregados1');
        var p_lbxSD = document.getElementById('lbxServiciosDisponibles1')
        var p_lbxServiciosDisponibles1 = new Array();
        var p_lbxServiciosAgregados1 = new Array();
        for (var i = 0; i < p_lbxSD.options.length; i++) {
            p_lbxServiciosDisponibles1.push(p_lbxSD.options[i].value + ";" + p_lbxSD.options[i].text);
        }
        for (var i = 0; i < p_lbxSA.options.length; i++) {
            p_lbxServiciosAgregados1.push(p_lbxSA.options[i].value + ";" + p_lbxSA.options[i].text);
        }
        var Accion = "QUITAR"
        PageMethods.MostrarLogs("*ID*1|" + p_lbxServiciosAgregados1.join("|"), "*ID*1|" + p_lbxServiciosDisponibles1.join("|"), parent.getValue('txtNroDoc'), Accion);
    }
    //FIN-INC000002245048 QUITAR
}

function resetServicio() {
    var idFila = document.getElementById('hidLineaActual').value;
    var strPlan = document.getElementById('ddlPlan' + idFila).value;

    if (strPlan.length > 0) {
        var lbxSA = document.getElementById('lbxServiciosAgregados1');
        var contSA = lbxSA.options.length;

        //INICIATIVA - 733 - INI - C23
        if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
            var boolEliminarClaroVideo = true;
            var DecoIPTVAgregado = false;
            var tabla = document.getElementById('tblResumenAlqEquipo3Play');
            var cont = tabla.rows.length;
            var CodigoEquipoIPTV = Key_CodEquipoIPTV.split('|');

            for (var j = 1; j < cont; j++) {
                fila = tabla.rows[j];
                var Codigo = fila.cells[0].getElementsByTagName("input")[1].value;

                for (var k = 0; k < CodigoEquipoIPTV.length; k++) {

                    if (CodigoEquipoIPTV[k] == Codigo) {
                        DecoIPTVAgregado = true;
                    }
                }
            }
        }
        //INICIATIVA - 733 - FIN - C23

        for (var i = 0; i < contSA; i++) {
            arrCodigo = lbxSA.options[i].value.split('_');

            if (arrCodigo[2] != '2' && arrCodigo[2] != '3') {
                //INICIATIVA - 733 - INI - C24
                if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
                    if (DecoIPTVAgregado == true && arrCodigo[3] == Key_CodClaroVideoIPTV) {
                        boolEliminarClaroVideo = false;
                    } else {

                        lbxSA.options[i].selected = true;

                        quitarServicio('r');
                        contSA--;
                        i--;
                    }

                } else {
                    lbxSA.options[i].selected = true;

                    quitarServicio('r');
                    contSA--;
                    i--;
                }
            //INICIATIVA - 733 - FIN - C24
            }
        }

        //INI-INC000002245048 LIMPIAR
        var p_lbxSA = document.getElementById('lbxServiciosAgregados1');
        var p_lbxSD = document.getElementById('lbxServiciosDisponibles1')
        var p_lbxServiciosDisponibles1 = new Array();
        var p_lbxServiciosAgregados1 = new Array();
        for (var i = 0; i < p_lbxSD.options.length; i++) {
            p_lbxServiciosDisponibles1.push(p_lbxSD.options[i].value + ";" + p_lbxSD.options[i].text);
        }
        for (var i = 0; i < p_lbxSA.options.length; i++) {
            p_lbxServiciosAgregados1.push(p_lbxSA.options[i].value + ";" + p_lbxSA.options[i].text);
        }
        var Accion = "LIMPIAR"
        PageMethods.MostrarLogs("*ID*1|" + p_lbxServiciosAgregados1.join("|"), "*ID*1|" + p_lbxServiciosDisponibles1.join("|"), parent.getValue('txtNroDoc'), Accion);
        //FIN-INC000002245048 LIMPIAR

        //INICIATIVA - 733 - INI - C25
        if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
            if (boolEliminarClaroVideo == false) {
                alert(Key_strMensajeClaroVideoIPTV);
            }
        }
        //INICIATIVA - 733 - FIN - C25

        return
    }

    //document.getElementById('lbxPromocionesDisponibles').innerHTML = "";
    //document.getElementById('lbxPromocionesSeleccionadas').innerHTML = "";
    document.getElementById('lbxServiciosDisponibles1').selectedIndex = -1;
    document.getElementById('lbxServiciosAgregados1').selectedIndex = -1;
}

function cerrarServicio() {
    document.getElementById('lbxServiciosDisponibles1').length = 0;
    document.getElementById('lbxServiciosAgregados1').length = 0;
    document.getElementById('tblServicios').style.display = 'none';
}

function asignarLineaActual(idFila) {
    var hidLineaActual = document.getElementById('hidLineaActual');

    hidLineaActual.value = idFila;
}

function FechaActual() {
    var fec;
    var cad;

    fec = new Date()
    if (fec.getDate() < 10) {
        cad = '0' + fec.getDate();
    } else {
        cad = '' + fec.getDate();
    }

    if ((fec.getMonth() + 1) < 10) {
        cad = cad + '/0' + (fec.getMonth() + 1);
    } else {
        cad = cad + '/' + (fec.getMonth() + 1);
    }

    cad = cad + '/' + fec.getFullYear();

    return cad;
}

function FechaEspecifica(dias) {
    var fec;
    var cad;

    fec = new Date()
    var dia = fec.getDate() + dias;
    if (fec.getDate() + 1 < 10) {
        cad = '0' + dia;
    } else {
        cad = '' + dia;
    }

    if ((fec.getMonth() + 1) < 10) {
        cad = cad + '/0' + (fec.getMonth() + 1);
    } else {
        cad = cad + '/' + (fec.getMonth() + 1);
    }

    cad = cad + '/' + fec.getFullYear();

    return cad;
}

function ValidaFechaMayor(control1, control2, campo1, campo2) {
    var cadena1;
    var cadena2;
    eval("cadena1 = " + control1 + ".value");
    eval("cadena2 = " + control2 + ".value");
    if ((cadena1 != '') && (cadena2 != '')) {
        comp1 = cadena1.substr(6, 4) + '' + cadena1.substr(3, 2) + '' + cadena1.substr(0, 2);
        comp2 = cadena2.substr(6, 4) + '' + cadena2.substr(3, 2) + '' + cadena2.substr(0, 2);
        if ((comp1) <= (comp2)) {
            alert('' + campo1 + ' debe ser MAYOR que la ' + campo2 + '');
            return false;
        }
    }
    return true;
}

function ValidaFechaMayorActual(control1, campo1) {
    var cadena1;
    var cadena2 = FechaActual();
    eval("cadena1 = " + control1 + ".value");
    if (cadena1 != '') {
        comp1 = cadena1.substr(6, 4) + '' + cadena1.substr(3, 2) + '' + cadena1.substr(0, 2);
        comp2 = cadena2.substr(6, 4) + '' + cadena2.substr(3, 2) + '' + cadena2.substr(0, 2);
        if ((comp1) < (comp2)) {
            alert('' + campo1 + ' debe ser MAYOR o IGUAL que la fecha actual');
            return false;
        }
    }
    return true;
}

function validarParametrosServRI() {
    if (document.getElementById('tblRoamingI').style.display == 'none') {
        alert('Seleccione e ingrese parametros para Servicio Roaming Internacional');
        return false;
    }

    if (document.getElementById('rbtValDeterminado').checked == false && document.getElementById('rbtValIndeterminado').checked == false) {
        alert('Debe seleccionar un plazo');
        return false;
    }

    if (getValue('txtFechaDesde') == '') {
        alert('Debe ingresar la fecha de Activación');
        return false;
    }

    if (!ValidaFechaMayorActual("document.getElementById('txtFechaDesde')", 'La Fecha Desde')) {
        return false;
    }

    if (isVisible('tdTxtFechaHasta')) {
        if (getValue('txtFechaHasta') == '') {
            alert('Debe ingresar la fecha de Desactivación');
            return false;
        }

        if (!ValidaFechaMayor("document.getElementById('txtFechaHasta')", "document.getElementById('txtFechaDesde')", 'La Fecha Hasta', 'Fecha Desde')) {
            return false;
        }
    }

    document.getElementById('tblRoamingI').style.display = 'none';
    return true;
}

function guardarServicio(Acc) { //INC000002245048 - Se agrego el parametro Accion

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][guardarServicio()] ", "Entro en la funcion guardarServicio()");

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][guardarServicio()] Acc ", Acc);

    if (document.getElementById('btnCerrarServicios').value == 'Cerrar') {
        cerrarServicio()
        parent.autoSizeIframe();
        return;
    }

    var hidnPlanServicioValue = document.getElementById('hidnPlanServicioValue');
    var hidPlanServicioNo = document.getElementById('hidPlanServicioNo');
    var lbxSD = document.getElementById('lbxServiciosDisponibles1');
    var lbxSA = document.getElementById('lbxServiciosAgregados1');

    //INICIATIVA - 733 - INI - C13
    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
        document.getElementById('hidLineaActual').value = VallineaAdicional
    }
    //INICIATIVA - 733 - FIN - C13

    var linea = document.getElementById('hidLineaActual').value;
    var strPlanServicio = '*ID*' + linea;
    var strPlanServicioNo = '*ID*' + linea;
    var contSD = lbxSD.options.length;
    var contSA = lbxSA.options.length;
    var strCodSA;
    var arrCodSA;
    var totalServicios = 0;

    var hidMontoServicios = document.getElementById('hidMontoServicios' + linea);

    borrarServicio(linea);
    var txtMontoTopeConsumo = document.getElementById('txtMontoTopeConsumo' + linea);
    if (txtMontoTopeConsumo != null)
        txtMontoTopeConsumo.value = '';

    if (getValue('hidCodigoTipoProductoActual') == codTipoProdMovil) {
        if (contSA == 0) {
            txtMontoTopeConsumo.value = constMontoTope;
        }
    }

    for (var i = 0; i < contSA; i++) {
        strCodSA = lbxSA.options[i].value;
        arrCodSA = strCodSA.split('_');

        //Verificar si el servicio es Roaming Internacional

        var codServSelected = arrCodSA[3];
        if (codServSelected == codServRoamingI) {
            if (!validarParametrosServRI())
                return;

            if (document.getElementById('rbtValDeterminado').checked == true)
                strPlanServicio += '|' + strCodSA + '_' + document.getElementById('rbtValDeterminado').value + '_' + getValue('txtFechaDesde') + '_' + getValue('txtFechaHasta') + ';' + lbxSA.options[i].text
            else
                strPlanServicio += '|' + strCodSA + '_' + document.getElementById('rbtValIndeterminado').value + '_' + getValue('txtFechaDesde') + '_' + getValue('txtFechaHasta') + ';' + lbxSA.options[i].text

        }
        else {
            strPlanServicio += '|' + strCodSA + ';' + lbxSA.options[i].text
        }

        //-----------------------
        totalServicios += parseFloat(arrCodSA[4]);


        if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProd3Play || getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam) {

            //INI-PROY-29296
            var arrCodTopesCons = new Array();
            var strCodTopesCons = codConsumoAdicional + '-' + codConsumoAbierto + '-' + codConsumoExacto;
            arrCodTopesCons = strCodTopesCons.split('-');
            for (var j = 0; j < arrCodTopesCons.length; j++) {
                if (arrCodSA[3] == arrCodTopesCons[j]) {
                    var idFila = getValue('hidLineaActual');
                    var indexInicio = lbxSA.options[i].text.indexOf('S/.') + 3;
                    var indexFin = lbxSA.options[i].text.lastIndexOf(' - ');
                    if (lbxSA.options[i].text.indexOf('S/.') != -1) {
                        document.getElementById('txtMontoTopeConsumo' + idFila).value = parseFloat(lbxSA.options[i].text.substring(indexInicio, indexFin)).toFixed(2);
                    } else {
                        document.getElementById('txtMontoTopeConsumo' + idFila).value = codValorDefectoTope.split(',')[1] + ".00";
                    }

                    //FIN-PROY-29296
                }
            }

        } else {

            if (arrCodSA[3] == topeConsumoAuto) {
                var strPlanCodigo = getValor(document.getElementById('ddlPlan' + linea).value, 0);
                LlenarMontoTopeConsumoIfr(linea, strPlanCodigo);
            }
            else if (getValue('hidCodigoTipoProductoActual') == codTipoProdMovil) {
                txtMontoTopeConsumo.value = '0';
            }
        }


    }
    for (var i = 0; i < contSD; i++) {
        var strCodSD = lbxSD.options[i].value.split('_')[3]; //PROY-24724-IDEA-28174 - INICIO
        if (strCodSD == codServProteccionMovil) {
            if (concatCodTipoPdvProteccionMovil.indexOf(parent.getValue('hidnCanalValue')) >= 0
                     && concatCodTipoOfertaProteccionMovil.indexOf(parent.getValue('ddlOferta')) >= 0
                     && concatCodTipoModalidadVentaProteccionMovil.indexOf(parent.getValue('ddlModalidadVenta')) >= 0)
                strPlanServicioNo += '|' + lbxSD.options[i].value + ';' + lbxSD.options[i].text
            else {
                lbxSD.remove(i);
                i--;
                contSD--;
            }
        }
        else
            strPlanServicioNo += '|' + lbxSD.options[i].value + ';' + lbxSD.options[i].text
    } //PROY-24724-IDEA-28174 - FIN

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][guardarServicio()] hidnPlanServicioValue antes ", hidnPlanServicioValue.value);

    hidnPlanServicioValue.value += strPlanServicio;

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][guardarServicio()] hidnPlanServicioValue despues ", hidnPlanServicioValue.value);

    hidPlanServicioNo.value += strPlanServicioNo;

    cerrarServicio();
    borrarGrupoServicio(linea);
    guardarGrupo(linea);
    cerrarLineasCuentas(); //PROY-140743

    hidMontoServicios.value = totalServicios;

    calcularCFxProducto();
    if ((getValue('hidCodigoTipoProductoActual') == codTipoProd3Play) || (getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam))
        asignarPromocion3Play(linea);

    //document.getElementById('lbxPromocionesDisponibles').innerHTML = "";
    //document.getElementById('lbxPromocionesSeleccionadas').innerHTML = "";
    //INC000000676006
    //document.getElementById('lbxServiciosDisponibles1').selectedIndex = -1;
    //document.getElementById('lbxServiciosAgregados1').selectedIndex = -1;
    document.getElementById('rbtValDeterminado').checked = false;
    document.getElementById('rbtValIndeterminado').checked = false;
    //FIN

    //INI-INC000002245048 CERRAR Y GUARDAR - SELECCIONAR PLAN

    if (Acc == 'c') {
        var ddlPlan = document.getElementById('ddlPlan' + linea); // //INC000002464679 
        var Accion = "CERRAR Y GUARDAR" + "   || " + ddlPlan.value + "   || " + linea; // //INC000002464679 
        PageMethods.MostrarLogs(hidnPlanServicioValue.value, hidPlanServicioNo.value, parent.getValue('txtNroDoc'), Accion);
    }
    else if (Acc == 'd') {
        var ddlPlan = document.getElementById('ddlPlan' + linea);
        var id = ddlPlan.selectedIndex;
        var Accion = "POR DEFECTO - SELECCIONO PLAN: " + ddlPlan.options[id].text + " - FILA: " + linea + "   || " + ddlPlan.value + "   || " + linea; // INC000002464679 
        PageMethods.MostrarLogs(hidnPlanServicioValue.value, hidPlanServicioNo.value, parent.getValue('txtNroDoc'), Accion);
    }
    //FIN-INC000002245048 CERRAR Y GUARDAR - SELECCIONAR PLAN

    parent.autoSizeIframe();
}

function asignarKits(idFila, strResultado) {
    document.getElementById('hidKit' + idFila).value = strResultado;

    var arrKits = strResultado.split('|');
    strResultado = '';
    for (var i = 0; i < arrKits.length; i++) {
        if (arrKits[i] != '') {
            strResultado += '|' + arrKits[i].split(';')[0].split('_')[0];
            strResultado += ';' + arrKits[i].split(';')[1];
        }
    }

    llenarMaterial(idFila, strResultado);
    asignarMaterial(idFila);
}

function obtenerPrecioKit(idFila, codigoKit) {
    var strKits = document.getElementById('hidKit' + idFila).value;
    var arrKits = strKits.split('|');
    var contKits = arrKits.length;
    var arrKit;
    var arrCodigo;
    var resultado = 0;

    for (i = 1; i < contKits; i++) {
        arrKit = arrKits[i].split(';');
        arrCodigo = arrKit[0].split('_');

        if (arrCodigo[0] == codigoKit)
            resultado = arrCodigo[3] + '_' + arrCodigo[2];
    }

    return resultado;
}

function LlenarMontoTopeConsumoIfr(idFila, strPlan) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strTipoProducto=' + getValue('hidCodigoTipoProductoActual'); //INICIATIVA-219
    url += '&strPlan=' + strPlan;
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strMetodo=' + 'LlenarMontoTopeConsumo';

    self.frames['iframeAuxiliar'].location.replace(url);
}

function borrarServicio(idFila) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][borrarServicio()] ", "Entro en la funcion borrarServicio()");

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][borrarServicio()] idFila  ", idFila);
    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][borrarServicio()] hidnPlanServicioValue antes ", document.getElementById('hidnPlanServicioValue').value);

    var hidnPlanServicioValue = document.getElementById('hidnPlanServicioValue');
    var hidPlanServicioNo = document.getElementById('hidPlanServicioNo');
    var strPlanServicio = '*ID*' + idFila;

    var intPosIni = hidnPlanServicioValue.value.indexOf(strPlanServicio);
    var intPosFin = 0;

    if (intPosIni > -1) {
        intPosFin = hidnPlanServicioValue.value.substring(intPosIni + 4).indexOf('*ID*');

        if (intPosFin == -1)
            intPosFin = hidnPlanServicioValue.value.length;
        else
            intPosFin += intPosIni + 4;

        hidnPlanServicioValue.value = hidnPlanServicioValue.value.replace(hidnPlanServicioValue.value.substring(intPosIni, intPosFin), '');
    }

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][borrarServicio()] hidnPlanServicioValue despues ", document.getElementById('hidnPlanServicioValue').value);


    intPosIni = hidPlanServicioNo.value.indexOf(strPlanServicio);

    if (intPosIni > -1) {
        intPosFin = hidPlanServicioNo.value.substring(intPosIni + 4).indexOf('*ID*');

        if (intPosFin == -1)
            intPosFin = hidPlanServicioNo.value.length;
        else
            intPosFin += intPosIni + 4;

        hidPlanServicioNo.value = hidPlanServicioNo.value.replace(hidPlanServicioNo.value.substring(intPosIni, intPosFin), '');
    }

    borrarGrupoServicio(idFila);

    setValue('hidMontoServicios' + idFila, '');
}
// ******************************************************* SERVICIOS ******************************************************* //

// ******************************************************* CUOTAS ******************************************************* //
function habilitarCuota(idFila) {
    var ddlCampana = document.getElementById('ddlCampana' + idFila);

    if (ddlCampana == null)
        return;

    var hab = (document.getElementById('imgEditarFila' + idFila) == null);
    var ddlNroCuotas = document.getElementById('ddlNroCuotas');

    ddlNroCuotas.disabled = hab;

    if (hab)
        document.getElementById('btnCerrarCuotas').value = 'Cerrar';
    else
        document.getElementById('btnCerrarCuotas').value = 'Cerrar y Guardar';
}

function llenarDatosCuota(idFila, strDatos) {
    var ddlNroCuotas = document.getElementById('ddlNroCuotas');
    var txtPorcCuotaIni = document.getElementById('txtPorcCuotaIni');
    var txtMontoCuotaIni = document.getElementById('txtMontoCuotaIni');
    var txtMontoCuota = document.getElementById('txtMontoCuota');
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var strNroCuotaActual = document.getElementById('hidNroCuotaActual').value;
    //PROY-29123
    var datosCuotaPrecioMax = "";
    var arrDatosMonto = new Array();
    var cuotasMax = 0, montoMax = 0;
    var mensajeBRMS = ""; //Mensaje para mostrar cuotas
    var strCanal = parent.getValue('ddlCanal');
    var tipoDoc = parent.getValue('ddlTipoDocumento');
    var tipoOpe = parent.getValue('ddlTipoOperacion');

    datosCuotaPrecioMax = strDatos.substr(strDatos.indexOf("^") + 1, strDatos.length);
    strDatos = strDatos.substr(0, strDatos.indexOf("^"));

    arrDatosMonto = datosCuotaPrecioMax.split("^");

    cuotasMax = parseInt(arrDatosMonto[0]);
    montoMax = parseFloat(arrDatosMonto[1]);
    mensajeBRMS = arrDatosMonto[2];

    if (mensajeBRMS && mensajeBRMS != "" && mensajeBRMS == "SI") {
        document.getElementById('lblMensajeCuotas').innerHTML = '"Cliente califica hasta ' + cuotasMax + ' cuotas con un equipo maximo de ' + montoMax.toLocaleString("es-Mx") + ' soles"'; ;

    }

    llenarDatosCombo(ddlNroCuotas, strDatos, true);

    txtPorcCuotaIni.value = '';
    txtMontoCuotaIni.value = '';
    txtMontoCuota.value = '';

    if (ddlCampana.disabled)
        obtenerCuota(idFila);
    else {
        var strCuotas = document.getElementById('hidCuota').value;
        var strCuota;
        var strId = '|*ID' + idFila + '*';
        var intPosIni = strCuotas.indexOf(strId);
        var intPosFin;
        var ddlNroCuotas = document.getElementById('ddlNroCuotas');
        var strCuotasItem; //PROY-30166-IDEA–38863
        var strIdFin = '*/ID' + idFila + '*'; //PROY-30166-IDEA–38863
        if (intPosIni > -1) {
            intPosIni += strId.length;
            strCuota = strCuotas.substring(intPosIni);
            intPosFin = strCuota.indexOf(';') + intPosIni;
            strCuota = strCuotas.substring(intPosIni, intPosFin);
            strCuotasItem = strCuotas.substring(strCuotas.indexOf(strId), strCuotas.indexOf(strIdFin)); //PROY-30166-IDEA–38863

            strNroCuotaActual = strCuota;
        }

        cambiarSelecCuota(strNroCuotaActual);
        actualizarCuota(strCuotasItem); //PROY-30166-IDEA–38863
    }

    quitarImagenEsperando();
    autoSizeIframe();
}

function actualizarCuota(strCuota) { //PROY-30166-IDEA–38863 - INICIO
    var txtPorcCuotaIni = document.getElementById('txtPorcCuotaIni');
    var txtMontoCuotaIni = document.getElementById('txtMontoCuotaIni');
    var txtMontoCuota = document.getElementById('txtMontoCuota');
    var arrCuota;

    if (strCuota != null && strCuota != '') {
        arrCuota = strCuota.split(';');

        txtPorcCuotaIni.value = arrCuota[1].toString();
        txtMontoCuotaIni.value = arrCuota[2].toString();
        txtMontoCuota.value = arrCuota[3].toString();
    }
} //PROY-30166-IDEA–38863 - FIN
function mostrarCuota(idFila) {
//PROY-140743-INI
    if (parent.getValue('hidAccPermiteCuotasVV') == '1' || parent.document.getElementById('hidAccPermiteCuotasVV').value == '1') {
        document.getElementById('hidValorEquipo' + obtenerIdFila()).value = '';
        alert(Key_MsjAccNoCuotas);
        return;
    }
//PROY-140743-FIN
    if (document.getElementById('hidValidarGuardarCuota').value.length > 0) {
        alert('Debe guardar las cuotas antes de ejecutar esta acción');
        return;
    }

    document.getElementById('tblFormnCuotas').style.display = 'none';

    if (document.getElementById('hidValorEquipo' + idFila).value.length == 0) {
        alert('Para seleccionar cuotas debe escoger un equipo');
        return;
    }
    if (parent.getValue('hidNTienePortabilidadValues') == 'S') {

        var valddlModalidadVenta = parent.document.getElementById('ddlModalidadVenta').value;

        if (valddlModalidadVenta == '3' || valddlModalidadVenta == '5') {//INICIATIVA 920

            if (parent.document.getElementById('ddlModalidadPorta').value == '' || parent.document.getElementById('ddlModalidadPorta').value == '00') {
                alert('Seleccione la Modalidad.');
                return;
            }
            else {

                if (parent.document.getElementById('ddlOperadorCedente').value == '' || parent.document.getElementById('ddlOperadorCedente').value == '00') {
                    alert('Seleccione el Operador Cedente.');
                    return;
                }

                parent.document.getElementById('ddlModalidadPorta').disabled = true;
                parent.document.getElementById('ddlOperadorCedente').disabled = true;
            }
        }
    }

    document.getElementById('hidLineaActual').value = idFila;
    document.getElementById('tblFormnCuotas').style.display = 'inline';
    //PROY-140743 INI
    if (parent.getValue('ddlTipoOperacion') == '25') {
        document.getElementById('tblFormnCuotas').style.display = 'none';
    }
    //PROY-140743 FIN
    cerrarServicio();
    cerrarEquipo();
    cerrarLineasCuentas();//PROY-140743

    habilitarCuota(idFila);

    var strCadenaDetalle = consultarItem(idFila);
    parent.consultaReglasCreditosCuotas(idFila, strCadenaDetalle);
}

function mostrarCuotaGuardada(idFila) {
    document.getElementById('tblServicios').style.display = 'none';

    if (document.getElementById('hidCuota').value.indexOf('*ID' + idFila + '*') > -1) {
        document.getElementById('tblFormnCuotas').style.display = 'inline';
        obtenerCuota(idFila);
        document.getElementById('hidLineaActual').value = idFila;
        habilitarCuota(idFila);
    }
    else
        document.getElementById('tblFormnCuotas').style.display = 'none';

    autoSizeIframe();
}

//PROY-30166-IDEA–38863-INICIO
function CambiarMontoIni() {
    var varMontoCuotaMenor = document.getElementById('hidMontoCuotaMenorA').value;
    var varMontoCuotaMayor = document.getElementById('hidMontoCuotaMayorA').value;
    var varMaxPorcentajeCuotaInicial = document.getElementById('hidMaxPorcentajeCuotaInicial').value;
    var varMontoCuotaMayorPorcentaje = document.getElementById('hidMontoCuotaMayorAPorcentaje').value;
    var varMsjActualizCuotaInicial = document.getElementById('hidMsjActualizCuotaInicial').value;

    var idFila = document.getElementById('hidLineaActual').value;
    var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
    var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
    var txtPorcCuotaIni = document.getElementById('txtPorcCuotaIni');  //Porcentual
    var txtMontoCuotaIni = document.getElementById('txtMontoCuotaIni'); //Valor que se ingresa
    var txtMontoCuota = document.getElementById('txtMontoCuota');
    var precio = txtEquipoPrecio.value;
    var strDetalleCuotas = document.getElementById('hidCuotaCalculada').value;

    var vNroCuota;
    var vPorcentajeBRMS;
    var vMontoCuotaInicial;
    var vMontoCuotaInicialBRMS;
    var vPorcentajeCalculado;
    var vMontoCuota;
    //********
    var ddlNroCuotas = (document.getElementById('ddlNroCuotas').value);
    ddlNroCuotas = ddlNroCuotas.split('_')[0];

    if (strDetalleCuotas.length > 0) {
        var strValIni = '|*ID' + idFila + '*';
        var strValFin = '*/ID' + idFila + '*';
        var intPosIni = strDetalleCuotas.indexOf(strValIni);
        var arrCuota;
        var intPosFin;
        if (intPosIni > -1) {
            intPosIni += strValIni.length;
            intPosFin = strDetalleCuotas.indexOf(strValFin);
            strDetalleCuotas = strDetalleCuotas.substring(intPosIni, intPosFin);
            arrCuota = strDetalleCuotas.split(';');
            vNroCuota = parseFloat(getValor(arrCuota[0], 0));
            vPorcentajeBRMS = parseFloat(getValor(arrCuota[0], 1));
            vPorcentajeCalculado = arrCuota[1];
            vMontoCuotaInicial = arrCuota[2];
            vMontoCuota = arrCuota[3];
            //Obtener el Monto de cuota Inicial del brms
            vMontoCuotaInicialBRMS = (precio * vPorcentajeBRMS / 100).toFixed(2);
        }

        var maxMontoCuotaInicial = parseFloat(precio) * parseFloat(varMaxPorcentajeCuotaInicial);
        if (parseFloat(txtMontoCuotaIni.value) > parseFloat(precio) || parseFloat(txtMontoCuotaIni.value) < parseFloat(vMontoCuotaInicialBRMS) || parseFloat(txtMontoCuotaIni.value) > parseFloat(maxMontoCuotaInicial)) {
            if (parseFloat(txtMontoCuotaIni.value) < parseFloat(vMontoCuotaInicialBRMS)) {
                alert(varMontoCuotaMenor + vMontoCuotaInicialBRMS);
            } else if (parseFloat(txtMontoCuotaIni.value) > parseFloat(precio)) {
                alert(varMontoCuotaMayor);
            } else {
                alert(varMontoCuotaMayorPorcentaje.replace("{0}", (parseFloat(varMaxPorcentajeCuotaInicial) * 100).toFixed(2)));
            }
            txtMontoCuotaIni.value = vMontoCuotaInicial;
            txtPorcCuotaIni.value = vPorcentajeCalculado;
            txtMontoCuota.value = vMontoCuota;
        }
        else if (isNaN(parseFloat(txtMontoCuotaIni.value))) {
            alert(varMontoCuotaMenor + vMontoCuotaInicialBRMS);
            txtMontoCuotaIni.value = vMontoCuotaInicial;
            txtPorcCuotaIni.value = vPorcentajeCalculado;
            txtMontoCuota.value = vMontoCuota;
        }
        else { // Flujo Normal
            txtPorcCuotaIni.value = ((100 * txtMontoCuotaIni.value) / precio).toFixed(2);
            txtMontoCuota.value = ((precio - txtMontoCuotaIni.value) / vNroCuota).toFixed(2);
            txtMontoCuotaIni.value;
        }
    } // fin If strDetalleCuotas
} //PROY-30166-IDEA–38863-FIN




function esMontoInicialValido() {
    var varMontoCuotaMenor = document.getElementById('hidMontoCuotaMenorA').value;
    var varMontoCuotaMayor = document.getElementById('hidMontoCuotaMayorA').value;
    var varMaxPorcentajeCuotaInicial = document.getElementById('hidMaxPorcentajeCuotaInicial').value;
    var varMontoCuotaMayorPorcentaje = document.getElementById('hidMontoCuotaMayorAPorcentaje').value;
    var varMsjActualizCuotaInicial = document.getElementById('hidMsjActualizCuotaInicial').value;

    var idFila = document.getElementById('hidLineaActual').value;
    var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
    var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
    var txtPorcCuotaIni = document.getElementById('txtPorcCuotaIni');  //Porcentual
    var txtMontoCuotaIni = document.getElementById('txtMontoCuotaIni'); //Valor que se ingresa
    var txtMontoCuota = document.getElementById('txtMontoCuota');
    var precio = txtEquipoPrecio.value;
    var strDetalleCuotas = document.getElementById('hidCuotaCalculada').value;

    var hidMsjMontoCuotaValido = document.getElementById('hidMsjMontoCuotaValido').value;


    var vNroCuota;
    var vPorcentajeBRMS;
    var vMontoCuotaInicial;
    var vMontoCuotaInicialBRMS;
    var vPorcentajeCalculado;
    var vMontoCuota;
    //********
    var ddlNroCuotas = (document.getElementById('ddlNroCuotas').value);
    ddlNroCuotas = ddlNroCuotas.split('_')[0];

    if (strDetalleCuotas.length > 0) {
        var strValIni = '|*ID' + idFila + '*';
        var strValFin = '*/ID' + idFila + '*';
        var intPosIni = strDetalleCuotas.indexOf(strValIni);
        var arrCuota;
        var intPosFin;
        if (intPosIni > -1) {
            intPosIni += strValIni.length;
            intPosFin = strDetalleCuotas.indexOf(strValFin);
            strDetalleCuotas = strDetalleCuotas.substring(intPosIni, intPosFin);
            arrCuota = strDetalleCuotas.split(';');
            vNroCuota = parseFloat(getValor(arrCuota[0], 0));
            vPorcentajeBRMS = parseFloat(getValor(arrCuota[0], 1));
            vPorcentajeCalculado = arrCuota[1];
            vMontoCuotaInicial = arrCuota[2];
            vMontoCuota = arrCuota[3];
            //Obtener el Monto de cuota Inicial del brms
            vMontoCuotaInicialBRMS = (precio * vPorcentajeBRMS / 100).toFixed(2);
        }

          //var maxMontoCuotaInicial = parseFloat(precio) * parseFloat(varMaxPorcentajeCuotaInicial);
          var maxMontoCuotaInicial = (parseFloat(precio) * parseFloat(varMaxPorcentajeCuotaInicial)).toFixed(2);
          //if (parseFloat(txtMontoCuotaIni.value) > parseFloat(precio) || (parseFloat(txtMontoCuotaIni.value) + 0.99) < parseFloat(vMontoCuotaInicialBRMS) || parseFloat(txtMontoCuotaIni.value) > parseFloat(maxMontoCuotaInicial)) {//G.G.A - INC000002592222 - SOLUCION
          //G.G.A - INC000002592222 - SOLUCION - INICIO
          if (parseFloat(txtMontoCuotaIni.value) > parseFloat(precio) ||
            (parseFloat(txtMontoCuotaIni.value) + 0.99) < parseFloat(vMontoCuotaInicialBRMS) || 
              parseFloat(txtMontoCuotaIni.value) > parseFloat(maxMontoCuotaInicial)) { 
          //G.G.A - INC000002592222 - SOLUCION - FIN


            if ((parseFloat(txtMontoCuotaIni.value) + 0.99) < parseFloat(vMontoCuotaInicialBRMS)) {
                alert(varMontoCuotaMenor + vMontoCuotaInicialBRMS);
            } else if (parseFloat(txtMontoCuotaIni.value) > parseFloat(precio)) {
                alert(varMontoCuotaMayor);
            } else {
                alert(varMontoCuotaMayorPorcentaje.replace("{0}", (parseFloat(varMaxPorcentajeCuotaInicial) * 100).toFixed(2)));
            }
            txtMontoCuotaIni.value = vMontoCuotaInicial;
            txtPorcCuotaIni.value = vPorcentajeCalculado;
            txtMontoCuota.value = vMontoCuota;

            return false;
        }
        else if (isNaN(parseFloat(txtMontoCuotaIni.value))) {

            alert(varMontoCuotaMenor + vMontoCuotaInicialBRMS);
            txtMontoCuotaIni.value = vMontoCuotaInicial;
            txtPorcCuotaIni.value = vPorcentajeCalculado;
            txtMontoCuota.value = vMontoCuota;
            return false;

        }
        else {


            var auxVMontoCuota = ((precio - txtMontoCuotaIni.value) / vNroCuota).toFixed(2);

            if (isNaN(parseFloat(txtMontoCuota.value))) {
                alert(hidMsjMontoCuotaValido.replace("{0}", auxVMontoCuota));
                txtMontoCuota.value = auxVMontoCuota;
                return false;
            }

            if (auxVMontoCuota != parseFloat(txtMontoCuota.value)) {
                alert(hidMsjMontoCuotaValido.replace("{0}", auxVMontoCuota));
                txtMontoCuota.value = auxVMontoCuota;
                return false;
            }

            return true;
        }
    } // fin If strDetalleCuotas

    return false;
}



function cambiarSelecCuota(strCuota) {
    var idFila = document.getElementById('hidLineaActual').value;
    parent.document.getElementById('hdnCuotasBRMS').value = strCuota;

    document.getElementById('hidStrCuota').value = parent.document.getElementById('hdnCuotasBRMS').value = strCuota;

    var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
    var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
    var txtPorcCuotaIni = document.getElementById('txtPorcCuotaIni');
    var txtMontoCuotaIni = document.getElementById('txtMontoCuotaIni');
    var txtMontoCuota = document.getElementById('txtMontoCuota');
    var ddlNroCuotas = document.getElementById('ddlNroCuotas');
    var fltPrecio;
    var fltMontoCuotaIni;
    var strListaPrecio;
    var fltPorcCuotaIni = parseFloat(getValor(strCuota, 1)); //PROY-30166-IDEA–38863-INICIO
    var strCanal = parent.getValue('ddlCanal');
    var vCuotaCalculada = '1';
    var vCalculo = '1';
    txtEquipoPrecio.value = '';
    txtPorcCuotaIni.value = '';
    txtMontoCuotaIni.value = '';
    txtMontoCuota.value = '';

    if (strCuota != '') {
        var idCuota = getValor(strCuota, 0);
        var intNroCuotas = parseFloat(idCuota);
        var strDetalleCuotas = document.getElementById('hidCuota').value; //PROY-30166-IDEA–38863-INICIO

        if (strDetalleCuotas.length > 0) {
            var strValIni = '|*ID' + idFila + '*';
            var strValFin = '*/ID' + idFila + '*';
            var arrCuota;
            var strNroCuotas = '';
            var strPorcCuotaIni = '';
            var strMontoCuotaIni = '';
            var strMontoCuota = '';
            var intPosIni = strDetalleCuotas.indexOf(strValIni);
            var intPosFin;

            if (intPosIni > -1) {
                intPosIni += strValIni.length;
                intPosFin = strDetalleCuotas.indexOf(strValFin);
                strDetalleCuotas = strDetalleCuotas.substring(intPosIni, intPosFin);
                arrCuota = strDetalleCuotas.split(';');
                fltPorcCuotaIni = arrCuota[1];  //Porcentaje Calculado
                vCalculo = '0';
                txtPorcCuotaIni.value = fltPorcCuotaIni;
                txtMontoCuotaIni.value = arrCuota[2];
                txtMontoCuota.value = arrCuota[3];
            }
        }
        else {
            fltPorcCuotaIni = parseFloat(getValor(strCuota, 1));
        }

        strListaPrecio = document.getElementById('hidListaPrecio' + idFila).value; //PROY-30166-IDEA–38863-FIN

        if (strListaPrecio == '') {
            ddlNroCuotas.value = '';
            alert(constMsjErrorConfigListaPrecio);
            return;
        }

        hidListaPrecio.value = strListaPrecio;
        fltPrecio = strListaPrecio.split('_')[0];
        txtEquipoPrecio.value = fltPrecio;

        if (seleccionarCuota(strCuota)) {
            if (vCalculo == '1') { //PROY-30166-IDEA–38863
                txtPorcCuotaIni.value = Math.round(fltPorcCuotaIni * 100) / 100; //PROY-30166
                //fltMontoCuotaIni = (Math.round((fltPrecio * fltPorcCuotaIni / 100), 1)).toFixed(2); //G.G.A - INC000002592222 - SOLUCION
                fltMontoCuotaIni = (fltPrecio * fltPorcCuotaIni / 100).toFixed(2);  //G.G.A - INC000002592222 - SOLUCION
                txtMontoCuotaIni.value = fltMontoCuotaIni;
                txtMontoCuota.value = ((fltPrecio - fltMontoCuotaIni) / intNroCuotas).toFixed(2);
            } //PROY-30166-IDEA–38863
        }

        document.getElementById('hidValidarGuardarCuota').value = 'S';

        var strPorcCuotaIni = document.getElementById('txtPorcCuotaIni').value; //PROY-30166-IDEA–38863-INICIO
        var strMontoCuotaIni = document.getElementById('txtMontoCuotaIni').value;
        var strMontoCuota = document.getElementById('txtMontoCuota').value;
        document.getElementById('hidCuotaCalculada').value = '|*ID' + idFila + '*' + strCuota + ';' + document.getElementById('txtPorcCuotaIni').value + ';' + document.getElementById('txtMontoCuotaIni').value + ';' + document.getElementById('txtMontoCuota').value + '*/ID' + idFila + '*'; //PROY-30166-IDEA–38863-FIN

        txtMontoCuotaIni.disabled = false; //INC000002297602
    }
}

function seleccionarCuota(strCuota) {
    var booExiste = false;
    var ddlNroCuotas = document.getElementById('ddlNroCuotas');

    for (var i = 0; i < ddlNroCuotas.options.length; i++) {
        if (ddlNroCuotas.options[i].value == strCuota)
            booExiste = true;
    }

    if (booExiste)
        ddlNroCuotas.value = strCuota;

    return booExiste;
}

function cerrarCuota() {
    document.getElementById('tblFormnCuotas').style.display = 'none';
    document.getElementById('hidValidarGuardarCuota').value = '';
}

function borrarCuota(idFila) {
    var hidCuota = document.getElementById('hidCuota');
    var strValIni = '|*ID' + idFila + '*';
    var strValFin = '*/ID' + idFila + '*';
    var strCuotas = hidCuota.value;
    var strCuota;
    var intPosIni = strCuotas.indexOf(strValIni);
    var intPosFin;
    var hidCuotaInicial = document.getElementById('hidConcatCuotaIni'); //PROY-30166-IDEA–38863-INICIO
    var strCuotaInicial = hidCuotaInicial.value;
    var strCuotaInicialItem;
    var intPosIniCI = strCuotaInicial.indexOf(strValIni);
    var intPosFinCI; //PROY-30166-IDEA–38863-FIN

    if (intPosIni > -1) {
        intPosFin = strCuotas.indexOf(strValFin);
        intPosFinCI = strCuotaInicial.indexOf(strValFin); //PROY-30166-IDEA–38863

        strCuota = strCuotas.substring(intPosIni, intPosFin + strValFin.length);
        strCuotaInicialItem = strCuotaInicial.substring(intPosIniCI, intPosFinCI + strValFin.length); //PROY-30166-IDEA–38863

        hidCuota.value = strCuotas.replace(strCuota, '');
        hidCuotaInicial.value = strCuotaInicial.replace(strCuotaInicialItem, ''); //PROY-30166-IDEA–38863

        asignarCuotaActual(idFila);
    }

    setValue('hidMontoCuota' + idFila, '');

    var imgVerCuota = document.getElementById('imgVerCuota' + idFila);
    if (imgVerCuota != null)
        imgVerCuota.src = '../../Imagenes/abrir.gif';
}

function asignarCuotaActual() {
    var strCuotas = document.getElementById('hidCuota').value;
    var hidNroCuotaActual = document.getElementById('hidNroCuotaActual');
    var arrCuotas;
    var arrCuota;
    var strNroCuota = '';
    var strValIni = '|*ID';
    var intPosIni;
    var strCuota;

    if (strCuotas.length > 0) {
        arrCuotas = strCuotas.split('|');

        for (var i = 0; i < arrCuotas.length; i++) {
            strCuota = arrCuotas[i];

            intPosIni = strCuota.indexOf(strValIni) + 3;
            strCuota = strCuota.substring(intPosIni);
            intPosIni = strCuota.indexOf('*') + 1;
            strCuota = strCuota.substring(intPosIni);

            arrCuota = strCuota.split(";");

            if (arrCuota[0].length > 0)
                strNroCuota = arrCuota[0];
        }
    }

    hidNroCuotaActual.value = strNroCuota;
}

function guardarCuotaBRMS() {

    if (esMontoInicialValido() == false) {
        return false;
    }
    guardarCuota();

}



function guardarCuota() {


    var ddlNroCuotas = document.getElementById('ddlNroCuotas');
    if (ddlNroCuotas.value.length == 0) {
        ddlNroCuotas.focus();
        alert('Debe seleccionar una cuota');
        return false;
    }

    document.getElementById('hidValidarGuardarCuota').value = '';

    cerrarCuota();
    cerrarLineasCuentas(); //PROY-140743

    if (document.getElementById('btnCerrarCuotas').value == 'Cerrar') {
        parent.autoSizeIframe();
        return;
    }

    var idFila = document.getElementById('hidLineaActual').value;
    var hidCuota = document.getElementById('hidCuota');
    var strNroCuotas = document.getElementById('ddlNroCuotas').value;
    var strPorcCuotaIni = document.getElementById('txtPorcCuotaIni').value;
    var strMontoCuotaIni = document.getElementById('txtMontoCuotaIni').value;
    var strMontoCuota = document.getElementById('txtMontoCuota').value;
    var hidMontoCuota = document.getElementById('hidMontoCuota' + idFila);

    borrarCuota(idFila);

    hidCuota.value += '|*ID' + idFila + '*' + strNroCuotas + ';' + strPorcCuotaIni + ';' + strMontoCuotaIni + ';' + strMontoCuota + '*/ID' + idFila + '*';

    var hidCuotaInicial = document.getElementById('hidConcatCuotaIni'); //PROY-30166-IDEA–38863-INICIO
    hidCuotaInicial.value += '|*ID' + idFila + '*' + strMontoCuotaIni + '*/ID' + idFila + '*'; //PROY-30166-IDEA–38863-FIN
    var strCasoEpecial = getValor(parent.getValue('ddlCasoEspecial'), 0);
    var blnCasoEspecialCMA = (strCasoEpecial == constCETrabajadoresCMA);
    if (!blnCasoEspecialCMA)
        hidMontoCuota.value = strMontoCuota;

    calcularCFxProducto();

    var imgVerCuota = document.getElementById('imgVerCuota' + idFila);
    imgVerCuota.src = '../../Imagenes/btn_seleccionar.gif';

    parent.autoSizeIframe();
}

function obtenerCuota(idFila) {
    var strCuota = document.getElementById('hidCuota').value;
    var strValIni = '|*ID' + idFila + '*';
    var strValFin = '*/ID' + idFila + '*';
    var arrCuota;
    var strNroCuotas = '';
    var strPorcCuotaIni = '';
    var strMontoCuotaIni = '';
    var strMontoCuota = '';
    var intPosIni = strCuota.indexOf(strValIni);
    var intPosFin;
    var ddlNroCuotas = document.getElementById('ddlNroCuotas');
    var txtPorcCuotaIni = document.getElementById('txtPorcCuotaIni');
    var txtMontoCuotaIni = document.getElementById('txtMontoCuotaIni');
    var txtMontoCuota = document.getElementById('txtMontoCuota');
    var hab = document.getElementById('ddlCampana' + idFila).disabled;

    if (intPosIni > -1) {
        intPosIni += strValIni.length;
        intPosFin = strCuota.indexOf(strValFin);

        strCuota = strCuota.substring(intPosIni, intPosFin);
        arrCuota = strCuota.split(';')

        strNroCuotas = arrCuota[0];
        strPorcCuotaIni = arrCuota[1];
        strMontoCuotaIni = arrCuota[2];
        strMontoCuota = arrCuota[3];
    }

    ddlNroCuotas.value = strNroCuotas;
    txtPorcCuotaIni.value = strPorcCuotaIni;
    txtMontoCuotaIni.value = strMontoCuotaIni;
    txtMontoCuota.value = strMontoCuota;
}

function confirmarCuota(idFila) {
    var strCuota = document.getElementById('hidCuota').value;
    if (strCuota.indexOf('|*ID' + idFila + '*') > -1)
        return true;
    else
        return false;
}
// ******************************************************* CUOTAS ******************************************************* //

function validarReglasComerciales(idPlan) {
    var idPlanxAgregar = idPlan;
    var ddlCasoEspecial = parent.document.getElementById('ddlCasoEspecial');
    var strCasoEspecial = ddlCasoEspecial.value;
    strCasoEspecial = strCasoEspecial.split('_')[0];

    var plan = 0;
    var nroPlanesTotal = 0;
    var strCodPlanBscs = 0;
    var strCodPlanSisact = 0;
    var strPlanesAgregados = getPlanesAgregados();
    var arrPlanesAgregados = strPlanesAgregados.split('|');
    idPlanxAgregar = parseInt(idPlanxAgregar, 10);

    //*********************************************************************************************************************
    //Plan Exacto Y
    if (strCasoEspecial == '') {
        if (idPlanxAgregar == 140) {
            alert("PLAN EXACTO Y es de uso exclusivo para CASO ESPECIAL OFERTA BACKUS O YANACOCHA");
            return false;
        }
    }
    //*********************************************************************************************************************
    //Plan Smart 25P
    var planBase = ConstPlanSmart25P;
    var strCodPlanBscs = planBase.split('|')[0];
    var strCodPlanSisact = parseInt(planBase.split('|')[1], 10);
    var intNroMaxPlanes = parseInt(planBase.split('|')[2], 10);

    if (strCodPlanSisact == idPlanxAgregar) {
        nroPlanesTotal = 0;
        nroPlanesTotal = parseInt(getNroPlanesActivos(strCodPlanBscs), 10) + parseInt(getNroPlanesAgregados(strCodPlanSisact), 10) + 1;

        if (nroPlanesTotal > intNroMaxPlanes) {
            alert(ConstMensajeExcedePlanes);
            return false;
        }
    }
    //*********************************************************************************************************************
    //Validación Nro Planes Caso Especial
    if (strCasoEspecial != "") {
        nroPlanesTotal = 0;
        nroPlanesTotal = parseInt(getNroPlanesActivos('ALL'), 10) + parseInt(getNroPlanesAgregados('ALL'), 10) + 1;
        var nroMaxPlanes = getValor(ddlCasoEspecial.value, 2);
        if (nroPlanesTotal > parseInt(nroMaxPlanes, 10)) {
            alert("La suma Total de Planes excede al permitido");
            return false;
        }
    }
    //*********************************************************************************************************************
    //Campaña Enamorados 2012
    var CE_BBEnamorados = ConstCasoEspBBEnamorados;
    var CE_SmartEnamorados = ConstCasoEspSmartEnamorados;

    if (strCasoEspecial == CE_BBEnamorados || strCasoEspecial == CE_SmartEnamorados) {
        nroPlanes = 0;
        for (var i = 0; i < arrPlanesAgregados.length; i++) {
            if (arrPlanesAgregados[i] != '') {
                plan = parseInt(arrPlanesAgregados[i].split('_')[0], 10);
                if (plan != idPlanxAgregar) {
                    alert("Debe elegir planes iguales para esta campaña.");
                    return false;
                }
            }
        }
        nroPlanes = parseInt(getNroPlanesAgregados('ALL'), 10);
        if (nroPlanes + 1 > 2) {
            alert("La suma total de Planes excede al Maximo permitido.");
            return false;
        }
    }
    //*********************************************************************************************************************
    //Claro Exacto
    var strPlanesClaroExacto = PlanesClaroExactoIlimitado;
    var arrPlanesClaroExacto = strPlanesClaroExacto.split('|');
    var blnPlanesClaroExacto = false;

    for (var i = 0; i < arrPlanesClaroExacto.length; i++) {
        plan = parseInt(arrPlanesClaroExacto[i].split(';')[0], 10);
        if (plan == idPlanxAgregar) {
            blnPlanesClaroExacto = true;
            break;
        }
    }
    if (blnPlanesClaroExacto) {
        nroPlanesTotal = 0;
        for (var i = 0; i < arrPlanesClaroExacto.length; i++) {
            strCodPlanBscs = arrPlanesClaroExacto[i].split(';')[1];
            strCodPlanSisact = parseInt(arrPlanesClaroExacto[i].split(';')[0], 10);
            nroPlanesTotal = nroPlanesTotal + parseInt(getNroPlanesActivos(strCodPlanBscs), 10) + parseInt(getNroPlanesAgregados(strCodPlanSisact), 10);
            if (nroPlanesTotal + 1 > 3) {
                alert("La suma total de planes Exacto + Ilimitado excede al permitido.");
                return false;
            }
        }
    }
    //*********************************************************************************************************************
    //Planes C Ilimitado
    var strPlanesModemCIlimitado = consPlanesModemCIlimitado;
    var arrPlanesModemCIlimitado = strPlanesModemCIlimitado.split('|');
    var blnPlanesModemCIlimitado = false;

    for (var i = 0; i < arrPlanesModemCIlimitado.length; i++) {
        plan = parseInt(arrPlanesModemCIlimitado[i].split(';')[0], 10);
        if (plan == idPlanxAgregar) {
            blnPlanesModemCIlimitado = true;
            break;
        }
    }
    if (blnPlanesModemCIlimitado) {
        nroPlanesTotal = 0;
        for (var i = 0; i < arrPlanesModemCIlimitado.length; i++) {
            strCodPlanBscs = arrPlanesModemCIlimitado[i].split(';')[1];
            strCodPlanSisact = parseInt(arrPlanesModemCIlimitado[i].split(';')[0], 10);
            nroPlanesTotal = nroPlanesTotal + parseInt(getNroPlanesActivos(strCodPlanBscs), 10) + parseInt(getNroPlanesAgregados(strCodPlanSisact), 10);
            if (nroPlanesTotal + 1 > 3) {
                alert("La suma total de Planes Modem (C) excede al permitido.");
                return false;
            }
        }
    }
    //*********************************************************************************************************************
    //Exclusion Planes
    var strListaPlanesRPC = constCodPlanesRPC;
    if (validarExclusionPlanes(idPlan, strListaPlanesRPC)) {
        alert("Los planes Exacto RPC6 o Exato RPC12 no pueden ser combinados con otros");
        return false;
    }
    //2Play Corporativo
    var strListaPlanes2Play = CodPlanes2PlayCorporativo;
    if (validarExclusionPlanes(idPlan, strListaPlanes2Play)) {
        alert("Los planes Fono Claro Internet son excluyentes, no pueden evaluarse junto a otros planes.");
        return false;
    }
    return true;
}

//Bloqueamos la tecla BACKSPACE en la ventana
function cancelarBackSpace() {
    if (event.keyCode == 119) {
        if (trEvaluacion.style.display != 'none')
            document.getElementById('btnEvaluar').click();
    }

    if ((event.keyCode == 8 || (event.keyCode == 37 && event.altKey) || (event.keyCode == 39 && event.altKey))
			&& (event.srcElement.form == null || (event.srcElement.isTextEdit == false && !event.srcElement.readOnly))) {
        event.cancelBubble = true;
        event.returnValue = false;
    }
}

function cargarImagenEsperando() {
    parent.cargarImagenEsperando();
}

function quitarImagenEsperando() {
    parent.quitarImagenEsperando();
}

//------------------------------------------------Combo Lista------------------------------------------------------------
function mostrarLista(strDivLista, idFila) {
    var divLista = document.getElementById(strDivLista + idFila);
    if (divLista.style.display == 'none') {
        //jvv
        if (emuladorIE == '')
            divLista.style.display = 'inline';
        else
            divLista.style.display = 'block';
        divGrillaDetalle.style.height = '500px';
        divCondVent.style.height = '575px';
        parent.cambiarAltoIframe(1000);
    }
    else {
        divLista.style.display = 'none';
        divGrillaDetalle.style.height = '100px';
        divCondVent.style.height = '175px';
        autoSizeIframe();
    }
}

function ocultarListaTxt(idFila) {
    ocultarLista('txt', idFila);
}

function ocultarLista(control, idFila) {
    if (control != 'txt')
        estiloNoSel(control);

    var idElementoActivo = document.activeElement.id;

    if (idElementoActivo.indexOf('txtTextoEquipo' + idFila) < 0 &&
			idElementoActivo.indexOf('divOption' + idFila + '_') < 0 &&
			idElementoActivo.indexOf('divListaEquipo' + idFila) < 0) {
        document.getElementById('divListaEquipo' + idFila).style.display = 'none';
        divGrillaDetalle.style.height = '100px';
        divCondVent.style.height = '175px';
    }
    autoSizeIframe();
}

function seleccionarItem(txtValor, txtTexto, divLista, div, idFila, prefijo) {
    var arrValores = div.id.split('_');
    var valor = '';
    if (arrValores.length > 1)
        valor = arrValores[1];
    txtValor += prefijo + idFila;
    txtTexto += prefijo + idFila;
    divLista += prefijo + idFila;
    //gaa20151102
    if (valor.indexOf('^') > -1) {
        valor = valor.replace('^', '');
        document.getElementById(txtTexto).style.color = BloqueoEquipoSinStockColor;
        alert(constMsjEquipoSinStockSel);
    }
    else
        document.getElementById(txtTexto).style.color = '';
    //fin gaa20151102
    document.getElementById(txtValor).value = valor;
    document.getElementById(txtTexto).value = arrValores[2];
    document.getElementById(divLista).style.display = 'none';
    divGrillaDetalle.style.height = '100px';
    divCondVent.style.height = '175px';

    if (prefijo == 'Equipo')
        cambiarEquipo1(idFila)
}

function estiloSel(div) {
    div.className = "AutoComplete_Highlight";
}
function estiloNoSel(div) {
    div.className = "AutoComplete_Item";
}
function imgSel(img) {
    img.src = '../../Imagenes/cmb.gif';
}
function imgNoSel(img) {
    img.src = '../../Imagenes/cmb.gif';
}

function mostrarListaSel(idFila) {
    var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
    mostrarLista('divListaEquipo', idFila);
    txtTextoEquipo.select();
}

function selText(input, inicio, fin) {
    if (typeof document.selection != 'undefined' && document.selection) {
        tex = input.value;
        input.value = '';
        input.focus();
        var str = document.selection.createRange();
        input.value = tex;
        str.move('character', inicio);
        str.moveEnd("character", fin - inicio);
        str.select();
    }
    else
        if (typeof input.selectionStart != 'undefined') {
            input.setSelectionRange(inicio, fin);
            input.focus();
        }
}

function buscarCoincidente(idFila) {
    var valorIngresado = window.event.keyCode;
    if (valorIngresado == 46)
        return;

    var hidValorEquipo = document.getElementById('hidValorEquipo' + idFila);
    var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
    var strTexto = txtTextoEquipo.value.toUpperCase();
    var strTextoCoincid;
    var strTextoEnc = '';
    var intLongTexto = strTexto.length;
    var strEquipos = getValue('hidMaterial' + idFila);
    var strCoincid = '';
    var arrEquipos = strEquipos.split('|');
    var arrEquipo;
    var strEquiposC = '';
    var divListaEquipo = document.getElementById('divListaEquipo' + idFila);
    var intIndEncontro = -1;

    if (intLongTexto > 0) {
        for (i = 0; i < arrEquipos.length; i++) {
            arrEquipo = arrEquipos[i].split(';');

            if (arrEquipo[0].length > 0) {
                strTextoCoincid = arrEquipo[1];

                if (strTextoCoincid.length >= intLongTexto) {
                    strTextoCoincid = strTextoCoincid.toUpperCase();
                    intIndEncontro = strTextoCoincid.indexOf(strTexto);

                    if (intIndEncontro > -1)
                        strEquiposC += '|' + arrEquipo[0] + ';' + arrEquipo[1];
                }
            }
        }
    }

    if (strEquiposC.length == 0)
        strEquiposC = strEquipos;

    llenarDatosCombo1(divListaEquipo, strEquiposC, idFila, 'Equipo', false);

    divListaEquipo.style.display = 'inline';
    divGrillaDetalle.style.height = '500px';
    divCondVent.style.height = '575px';

    if (valorIngresado == 40) {
        var arrDatos = strEquiposC.split('|');
        if (arrDatos.length > 1) {
            var arrItem = arrDatos[1].split(';');
            var divOpcion = document.getElementById('divOption' + idFila + '_' + arrItem[0] + '_' + arrItem[1] + '_' + 1);
            divOpcion.focus();

            setValue('hidEquiposSel', strEquiposC);
        }
    }
    else {
        hidValorEquipo.value = '';
        document.getElementById('txtEquipoPrecio' + idFila).value = 0;
        borrarCuota(idFila);

        calcularCFxProducto();
    }
}

function cambiarFocoSpan(strId, idFila) {
    var idSel = parseInt(strId.split('_')[3]);
    var idAnterior = idSel - 1;
    var idSiguiente = idSel + 1;
    var strEquipos = getValue('hidEquiposSel');
    var arrDatos = strEquipos.split('|');
    var strDato;
    var arrItem;
    var divAnterior = 0;
    var divSiguiente = 1;

    if (arrDatos[idAnterior].length > 0) {
        strDato = arrDatos[idAnterior];
        arrItem = strDato.split(";");

        if (strDato != 'null') {
            if (arrItem.length > 1)
                divAnterior = document.getElementById('divOption' + idFila + '_' + arrItem[0] + '_' + arrItem[1] + '_' + idAnterior);
        }
    }

    if (idSiguiente == arrDatos.length)
        idSiguiente = 0;

    if (arrDatos[idSiguiente].length > 0) {
        strDato = arrDatos[idSiguiente];
        arrItem = strDato.split(";");

        if (strDato != 'null') {
            if (arrItem.length > 1)
                divSiguiente = document.getElementById('divOption' + idFila + '_' + arrItem[0] + '_' + arrItem[1] + '_' + idSiguiente);
        }
    }


    if (window.event.keyCode == 40) {
        if (divSiguiente != 1)
            divSiguiente.focus();
    }
    else {
        if (window.event.keyCode == 38) {
            if (divAnterior != 0)
                divAnterior.focus();
            else {
                var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
                txtTextoEquipo.select();
                txtTextoEquipo.focus();
                return false;
            }

        }
        else {
            if (window.event.keyCode == 13) {
                seleccionarItem('hidValor', 'txtTexto', 'divLista', document.getElementById(strId), idFila, 'Equipo');
                return false;
            }
        }
    }
}
//------------------------------------------------Combo Lista------------------------------------------------------------

function inicializarEquipo(idFila) {
    var hidValorEquipo = document.getElementById('hidValorEquipo' + idFila);
    var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
    var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
    var txtCFMenAlqKit = document.getElementById('txtCFMenAlqKit' + idFila);
    var txtCFTotMensual = document.getElementById('txtCFTotMensual' + idFila);
    var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
    var hidMaterial = document.getElementById('hidMaterial' + idFila);

    if (hidValorEquipo != null) {
        hidValorEquipo.value = '';
        txtTextoEquipo.value = 'SELECCIONE...';
    }

    if (txtEquipoPrecio != null)
        txtEquipoPrecio.value = '';

    if (txtCFMenAlqKit != null)
        txtCFMenAlqKit.value = '';

    if (txtCFTotMensual != null)
        txtCFTotMensual.value = '';

    if (hidListaPrecio != null)
        hidListaPrecio.value = '';

    //            if (hidMaterial != null)
    //                hidMaterial.value = '';

    calcularCFxProducto();
}

function inicializarPrecioEquipo(idFila) {
    var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);

    if (txtEquipoPrecio != null) {
        txtEquipoPrecio.value = '';

        calcularCFxProducto();
    }
}

function cambiarEquipo1(idFila) {
    borrarCuota(idFila);
    cerrarCuota();
    cerrarLineasCuentas(); //PROY-140743
    EliminarBuyBack(idFila);

    //PROY-140743 - IDEA-141192 - Venta en cuotas | INI
    if (parent.getValue('ddlTipoOperacion') == '25') {
        var ValorEquipo = document.getElementById('hidValorEquipo' + idFila).value;
        parent.document.getElementById('hidAccPermiteCuotasVV').value = '';
        PageMethods.ValidarMaterialAccCuotas(ValorEquipo, function (response) {
            if (response == '1') {
                parent.document.getElementById('hidAccPermiteCuotasVV').value = response;
                alert(Key_MsjAccNoCuotas);
                return;
            }
        });
    }
    //PROY-140743 - IDEA-141192 - Venta en cuotas | FIN

    setValue('hidFilaSeleccionada', idFila); //PROY-29296
    var CampaniaBuyBack = parent.getValue('hdCampaniaBuyBack');
    var hdbuyback = parent.getValue('hdbuyback').split('|');
    //PROY-140736 INI
    var campBuyback = Key_CodCampaniaBuyBack.split('|');
    var campValido = false;
    var editar = false;
    for (var i = 0; i < campBuyback.length; i++) {
        if (campBuyback[i] == CampaniaBuyBack) {
            campValido = true;
        }
    }
    if (campValido) {
        parent.document.getElementById('tblFormnBuyBackIphone').style.display = 'inline';
        parent.document.getElementById('hidValorEquipo_cv').value = document.getElementById('hidValorEquipo' + idFila).value;
    } else {

        parent.document.getElementById('tblFormnBuyBackIphone').style.display = 'none';
        ContinuarCambiarEquipo1(idFila);
    }
}


function ContinuarCambiarEquipo1(idFila) {
    var strEquipo = document.getElementById('hidValorEquipo' + idFila).value;
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');

    //PROY-140736 INI
    var valorBuyBack = parent.getValue('hdbuyback');
    var arrBuyback = valorBuyBack.split('|');
    var materialBuyback = '';
    for (var i = 0; i < arrBuyback.length; i++) {
        if (arrBuyback[i].split(';')[0] == idFila) {
            materialBuyback = arrBuyback[i].split(';')[3];
        }
    }
    //PROY-140736 FIN
    if (strEquipo.length == 0) {
        inicializarEquipo(idFila);
        return;
    }
    //INICIATIVA 920
    if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota || parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode) {
        inicializarPrecioEquipo(idFila);
    }

    var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
    if (txtEquipoPrecio != null)
        txtEquipoPrecio.value = '';

    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var strCampana = '';
    var strPlan = '';
    var strPlanSisact = '';
    var strPlazo = '';

    if (ddlCampana != null)
        strCampana = ddlCampana.value;

    if (ddlPlan != null) {
        strPlan = ddlPlan.value;
        strPlanSisact = getValor(strPlan, 0);
        strPlan = getValor(strPlan, 2);
    }

    //INICIATIVA 920
    if (ddlPlazo != null){
            strPlazo = ddlPlazo.value.split('-')[0];
    }
        
        

    if (ddlPlan != null && ddlPlazo != null) {
        if (ddlPlan.value.length == 0 || strPlazo.length == 0 || strCampana.length == 0) {
            alert('Debe seleccionar el plan, plazo y campaña antes que el equipo')
            document.getElementById('hidValorEquipo' + idFila).value = '';
            document.getElementById('txtTextoEquipo' + idFila).value = '';
            return;
        }
    }

    if (codigoTipoProductoActual != codTipoProdDTH) {
        if (codigoTipoProductoActual != codTipoProdVentaVarios)
            LlenarEquipoPrecioIfr(idFila, strPlan, strPlanSisact, strPlazo, strCampana, strEquipo, materialBuyback);//PROY-140736
        else
            LlenarListaPrecioIfr(idFila, strCampana, strEquipo);
    }
    else {
        asignarEquipoPrecio(idFila, obtenerPrecioKit(idFila, document.getElementById('hidValorEquipo' + idFila).value));
        calcularCFxProducto();
    }
}

function asignarMaterial(idFila) {
    var idFilaAnt;
    var strMaterial = document.getElementById('hidMaterial' + idFila).value;
    var divListaEquipo = document.getElementById('divListaEquipo' + idFila);

    inicializarEquipo(idFila);


    setValue('hidFilaSeleccionada', idFila); //PROY-140736
    llenarDatosCombo1(divListaEquipo, strMaterial, idFila, 'Equipo', false);

    if (document.getElementById('imgVerCuota' + idFila) != null)
        borrarCuota(idFila);

    //gaa20160211
    var strListaDE = getValue('hidListaDE');

    if (strListaDE.length > 0) {
        idFilaAnt = parseInt(idFila) - 1;
        setValue('hidValorEquipo' + idFila, getValue('hidValorEquipo' + idFilaAnt));
        setValue('txtTextoEquipo' + idFila, getValue('txtTextoEquipo' + idFilaAnt));

        asignarPrecio(idFila, strListaDE);
        setValue('hidListaDE', '');

        preservarFila1(idFilaAnt);
        preservarFila1(idFila);

        var txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
        if (txtNroTelefono != null)
            setEnabled('txtNroTelefono' + idFila, true, 'clsInputEnabled');

        txtNroTelefono = document.getElementById('txtNroTelefono' + idFilaAnt);
        if (txtNroTelefono != null)
            setEnabled('txtNroTelefono' + idFilaAnt, true, 'clsInputEnabled');

        var tabla = document.getElementById('tblTablaMovil');
        var cont = tabla.rows.length;
        var nCell = 2;

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];

            idFilaX = fila.cells[nCell].getElementsByTagName("select")[0].id.substring(10);

            if (idFila == idFilaX || idFilaAnt == idFilaX) {
                fila.cells[0].innerHTML = '';
                fila.cells[1].innerHTML = '';
            }
        }
    }
    //fin gaa20160211
}

function llenarDatosCombo1(div, strDatos, idFila, prefijo, booSeleccione) {
    var arrDatos;
    var arrItem;
    var strDato;
    var prefijo1 = prefijo + idFila;
    var strAnterior = '';
    var strSiguiente = '';
    div.innerHTML = "";

    if (booSeleccione) {
        divOpcion = document.createElement('span');
        divOpcion.style.width = '100%';
        divOpcion.id = '';
        divOpcion.innerHTML = 'SELECCIONE...';
        document.getElementById('hidValor' + prefijo1).value = '';
        document.getElementById('txtTexto' + prefijo1).value = 'SELECCIONE...';

        divOpcion.className = "AutoComplete_Item";
        divOpcion.onmouseover = function () { estiloSel(this); };
        divOpcion.onmouseout = function () { estiloNoSel(this); };
        divOpcion.onclick = function () { seleccionarItem('hidValor', 'txtTexto', 'divLista', this, idFila, prefijo); };
        divOpcion.onfocus = function () { estiloSel(this); };
        //divOpcion.onblur = function() {estiloNoSel(this);};
        divOpcion.onblur = function () { ocultarLista(this, idFila); };
        div.appendChild(divOpcion);
    }
    if (strDatos != null)
        var arrDatos = strDatos.split("|");
    else
        return;

    for (i = 0; i < arrDatos.length; i++) {
        divOpcion = document.createElement('span');
        divOpcion.style.width = '90%';
        if (emuladorIE == '')//jvv
            divOpcion.style.display = 'inline-block';
        divOpcion.className = "AutoComplete_Item";
        divOpcion.onmouseover = function () { estiloSel(this); };
        divOpcion.onmouseout = function () { estiloNoSel(this); };
        divOpcion.onclick = function () { seleccionarItem('hidValor', 'txtTexto', 'divLista', this, idFila, prefijo); };
        divOpcion.onfocus = function () { estiloSel(this); };
        //divOpcion.onblur = function() {estiloNoSel(this);};
        divOpcion.onblur = function () { ocultarLista(this, idFila); };

        if (arrDatos[i].length > 0) {
            strDato = arrDatos[i];
            arrItem = strDato.split(";");

            if (strDato != 'null') {
                if (arrItem.length > 1) {
                    //gaa20151029
                    if (arrItem[0].indexOf('^') > -1) {
                        divOpcion.style.color = BloqueoEquipoSinStockColor;
                        //arrItem[0] = arrItem[0].replace('^', '');
                        //divOpcion.title = 'sto';
                    }
                    //fin gaa20151029
                    divOpcion.id = 'divOption' + idFila + '_' + arrItem[0] + '_' + arrItem[1] + '_' + i;
                    //divOpcion.innerHTML = arrItem[1];
                    divOpcion.innerHTML = arrItem[1] + '<br />';

                    divOpcion.onkeydown = function () { return cambiarFocoSpan(this.id, idFila); };
                }
                else {
                    option.value = 'divOption_' + arrDatos[i] + '_' + arrDatos[i];
                    //divOpcion.innerHTML = arrDatos[i];
                    divOpcion.innerHTML = arrItem[1] + '<br />';
                }

                div.appendChild(divOpcion);
            }
        }
    }
}

function LlenarPaquete3PlayIfr(strCampana, strPlazo, idFila, codigoTipoProductoActual) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strCampana=' + strCampana;
    url += '&strPlazo=' + strPlazo;
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strCombo=' + parent.getValue('ddlCombo');
    url += '&strMetodo=' + "LlenarPaquete3Play";
    url += '&strTipoProducto=' + codigoTipoProductoActual;

    self.frames['iframeAuxiliar'].location.replace(url);
}

function asignarPaquete3Play(idFila, strValores) {
    var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
    llenarDatosCombo(ddlPaquete, strValores, true);
}

function mostrarDetOfert(idFila) {
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var strPlan = ddlPlan.value;
    var strPlanDes, strCampana;
    var strDetalle = '';
    var strEquipo = '';

    if (strPlan != '') {
        var nroSec = getValue('hidNroSec' + idFila);
        strPlanDes = obtenerTextoSeleccionado(ddlPlan).replace(/\+/g, '*');
        strCampana = obtenerTextoSeleccionado(document.getElementById('ddlCampana' + idFila)).replace(/\+/g, '*');

        if (nroSec.length == 0) {
            strDetalle = obtenerCadenaDetOfert(idFila);
            strEquipo = obtenerCadenaEquipoDetOfert(idFila);
        }

        var w = 850;
        var h = 400;
        var leftScreen = (screen.width - w) / 2;
        var topScreen = (screen.height - h) / 2;
        var opciones = "directories=no,menubar=no,scrollbars=yes,status=yes,resizable=yes,width=" + w + ",height=" + h + ",left=" + leftScreen + ",top=" + topScreen;
        var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
        var url = '';
        url = '../consultas/sisact_pop_detalle_oferta.aspx?strDetalleHFC=' + strDetalle;
        url += '&strCampana=' + strCampana + '&strPlan=' + strPlanDes + '&nroSec=' + nroSec + '&strEquipoHFC=' + strEquipo;
        url += '&strTipoProducto=' + codigoTipoProductoActual;
        window.open(url, '_blank', opciones);
    }
}

function obtenerCadenaDetOfert(idFila) {
    var tabla;
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');

    if (codigoTipoProductoActual == codTipoProd3Play) {
        tabla = document.getElementById('tblTablaHFC');
    };

    //FTTH  - Inicio - obtenerCadenaDetOfert(idFila) 
    if (codigoTipoProductoActual == codTipoProdFTTH) {
        tabla = document.getElementById('tblTablaFTTH');
    };
    //FTTH  - Inicio - obtenerCadenaDetOfert(idFila) 

    if (codigoTipoProductoActual == codTipoProd3PlayInalam) {
        tabla = document.getElementById('tblTabla3PlayInalam');
    };
    //PROY-31812 - IDEA-43340 - INICIO
    if (codigoTipoProductoActual == codTipoProdInterInalam) {
        tabla = document.getElementById('tblTablaInterInalam');
    };
    //PROY-31812 - IDEA-43340 - FIN
    var fila;
    var cont = tabla.rows.length;
    var nCell; //0: Imagen, 1:Imagen
    var arrPlanCodigo;
    var strTipoProducto;
    var strPlanCodigo;
    var strGrupo;
    var strTipo;
    var strProducto;
    var strServicio
    var strIdServicio;
    var strPromocion;
    var strResultado = '';
    var strFlgPrincipal;
    var strServicioCodigo;
    var idFila;
    var arrServicio;
    var strPrecio;
    var strGrupoDescripcion;

    //GRUPO/TIPO/PRODUCTO/IDSERVICIO/SERVICIO/PROMOCION/PRINCIPAL

    var strIdFilas = obtenerFilasGrupo(idFila, '', false);

    //PROY-31812 - IDEA-43340 - INICIO
    if (isNumber(strIdFilas)) {
        strIdFilas = '[' + strIdFilas + ']';
    }
    //PROY-31812 - IDEA-43340 - FIN

    for (var i = 0; i < cont; i++) {
        nCell = 1; //0: Imagen Confirmar, 1:Imagen Eliminar

        fila = tabla.rows[i];

        strTipoProducto = getValue('hidCodigoTipoProductoActual');


        if (strTipoProducto == codTipoProd3Play || strTipoProducto == codTipoProd3PlayInalam || strTipoProducto == codTipoProdFTTH) {
            nCell += 1;

            idFila = fila.cells[nCell].getElementsByTagName("select")[0].id.substring(10);

            if (strIdFilas.indexOf('[' + idFila + ']') < 0)
                continue;

            nCell += 4;

            //Servicio Principal
            arrPlanCodigo = fila.cells[nCell].getElementsByTagName("select")[0].value.split('_');
            strPlanCodigo = arrPlanCodigo[0];
            strServicio = obtenerTextoSeleccionado(fila.cells[nCell].getElementsByTagName("select")[0]);
            strServicioCodigo = arrPlanCodigo[0];
            strGrupo = arrPlanCodigo[4];
            strTipo = '1'; //Serv. Principal';
            strProducto = arrPlanCodigo[9];
            strIdServicio = arrPlanCodigo[10];
            strPrecio = arrPlanCodigo[1];
            strFlgPrincipal = '1';
            strGrupoDescripcion = arrPlanCodigo[5];

            //Servicios Adicionales
            if (strPlanCodigo != '') {
                strResultado += obtenerPromocionDetOfert(idFila, strServicioCodigo, strGrupo, strTipo, strProducto, strIdServicio, strServicio, strPrecio, strFlgPrincipal, strResultado.length, strGrupoDescripcion);
                strResultado += obtenerCadenaDetOfertServ(idFila);
            }
        }
        //PROY-31812 - IDEA-43340 - INICIO
        else if (strTipoProducto == codTipoProdInterInalam) {
            nCell += 1;

            idFila = fila.cells[nCell].getElementsByTagName("select")[0].id.substring(10);

            if (strIdFilas.indexOf('[' + idFila + ']') < 0) //PROY-31812 - IDEA-43340
                continue;

            nCell += 2;
            //Servicio Principal
            arrPlanCodigo = fila.cells[nCell].getElementsByTagName("select")[0].value.split('_');

            strPlanCodigo = arrPlanCodigo[0];
            strServicio = obtenerTextoSeleccionado(fila.cells[nCell].getElementsByTagName("select")[0]);
            strServicioCodigo = arrPlanCodigo[0];
            strGrupo = arrPlanCodigo[4];
            strTipo = '1'; //Serv. Principal';
            strProducto = arrPlanCodigo[8];
            strIdServicio = arrPlanCodigo[10];
            strPrecio = arrPlanCodigo[1];
            strFlgPrincipal = '1';
            strGrupoDescripcion = arrPlanCodigo[5];

            //Servicios Adicionales
            if (strPlanCodigo != '') {
                strResultado += obtenerPromocionDetOfert(idFila, strServicioCodigo, strGrupo, strTipo, strProducto, strIdServicio, strServicio, strPrecio, strFlgPrincipal, strResultado.length, strGrupoDescripcion);
                strResultado += obtenerCadenaDetOfertServ(idFila);
            }
        }
        //PROY-31812 - IDEA-43340 - FIN
    }

    return strResultado;
}

function obtenerCadenaDetOfertServ(idFila) {
    var strPlanServicio = document.getElementById('hidnPlanServicioValue').value;
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual'); //PROY-31812 - IDEA-43340
    var posIniPS = strPlanServicio.indexOf('*ID*' + idFila);
    var posFinPS = 0;
    var arrServicio;
    var strServicio;
    var arrServicioCodigo;
    var strServicioCodigo;
    var strResultado = '';
    var strGrupo;
    var strTipo;
    var strProducto;
    var strIdServicio;
    var strPromocion;
    var strFlgPrincipal;
    var strPrecio;

    if (posIniPS > -1) {
        posFinPS = strPlanServicio.substring(posIniPS + 4).indexOf('*ID*') + 4;

        if (posFinPS == 3)
            posFinPS = strPlanServicio.length;
        else
            posFinPS += posIniPS;

        var arrPS = strPlanServicio.substring(posIniPS, posFinPS).split('|');

        for (var i = 1; i < arrPS.length; i++) {
            arrServicio = arrPS[i].split(';');
            strServicio = arrServicio[1];

            arrServicioCodigo = arrServicio[0].split('_');

            strServicioCodigo = arrServicioCodigo[3];
            strServicio = arrServicio[1];
            //PROY-31812 - IDEA-43340 - INICIO
            if (codigoTipoProductoActual == codTipoProdInterInalam) {
                strGrupo = arrServicioCodigo[0];
                strGrupoDescripcion = arrServicioCodigo[5];
            }
            else {
                //PROY-31812 - IDEA-43340 - FIN         
                strGrupo = arrServicioCodigo[7];
                strGrupoDescripcion = arrServicioCodigo[8]; //PROY-31812 - IDEA-43340
            }
            strTipo = '0'; //Adicional';
            strProducto = arrServicioCodigo[12];
            strIdServicio = arrServicioCodigo[13];
            strPrecio = arrServicioCodigo[4];
            strFlgPrincipal = '0';
            strGrupoDescripcion = arrServicioCodigo[8];

            strResultado += obtenerPromocionDetOfert(idFila, strServicioCodigo, strGrupo, strTipo, strProducto, strIdServicio, strServicio, strPrecio, strFlgPrincipal, 1, strGrupoDescripcion);
        }
    }

    return strResultado;
}

function obtenerPromocionDetOfert(idFila, strCodSrv, strGrupo, strTipo, strProducto, strIdServicio, strServicio, strPrecio, strFlgPrincipal, intCantidadCar, strGrupoDescripcion) {
    var strPromocion = getValue('hidNPromocionValues');
    var posIni = strPromocion.indexOf('*ID*' + idFila);
    var posFin = 0;
    var arrPromociones;
    var arrPromocion;
    var strCodigo;
    var arrCodigo;
    var strIDDET;
    var strIDPRODUCTO;
    var strIDLINEA;
    var strPromoComodin = '{Promocion}';
    var strResultado = strGrupo + ';' + strTipo + ';' + strProducto + ';' + strIdServicio + ';' + strServicio + ';' + strPrecio + ';' + strPromoComodin + ';' + strFlgPrincipal + ';' + strGrupoDescripcion;
    if (intCantidadCar > 0)
        strResultado = '|' + strResultado;
    var booEntro = false;

    if (posIni > -1) {
        posFin = strPromocion.substring(posIni + 4).indexOf('*ID*') + 4;

        if (posFin == 3)
            posFin = strPromocion.length;
        else
            posFin += posIni;

        arrPromociones = strPromocion.substring(posIni, posFin).split('|');

        for (var i = 1; i < arrPromociones.length; i++) {
            arrPromocion = arrPromociones[i].split(';');

            arrCodigo = arrPromocion[0].split('_');

            arrCodigo = arrCodigo[3].split('.');
            strIDDET = arrCodigo[0];
            strIDPRODUCTO = arrCodigo[1];
            strIDLINEA = arrCodigo[2];

            if (strCodSrv == strIDDET + '.' + strIDPRODUCTO + '.' + strIDLINEA) {
                if (booEntro)
                    strResultado += '|' + strGrupo + ';' + ';' + ';' + ';' + ';' + ';' + arrPromocion[1] + ';' + strFlgPrincipal + ';' + strGrupoDescripcion;
                else {
                    strResultado = strResultado.replace(strPromoComodin, arrPromocion[1]);
                    booEntro = true;
                }
            }
        }
    }

    strResultado = strResultado.replace(strPromoComodin, '');
    return strResultado;
}

function mostrarAllTablaInactivo() {
    tblTablaTituloMovil.style.display = 'none';
    tblTablaTituloFijo.style.display = 'none';
    tblTablaTituloBAM.style.display = 'none';
    tblTablaTituloDTH.style.display = 'none';
    tblTablaTituloHFC.style.display = 'none';
    tblTablaTituloFTTH.style.display = 'none'; // FTTH -  mostrarAllTablaInactivo() -tblTablaTituloFTTH
    tblTablaTituloVentaVarios.style.display = 'none';
    tblTablaTitulo3PlayInalam.style.display = 'none';
    tblTablaMovil.style.display = 'none';
    tblTablaFijo.style.display = 'none';
    tblTablaBAM.style.display = 'none';
    tblTablaDTH.style.display = 'none';
    tblTablaHFC.style.display = 'none';
    tblTablaFTTH.style.display = 'none'; // FTTH -  mostrarAllTablaInactivo() - tblTablaFTTH
    tblTablaVentaVarios.style.display = 'none';
    tblTabla3PlayInalam.style.display = 'none';
    //PROY-31812 - IDEA-43340 - INICIO
    tblTablaTituloInterInalam.style.display = 'none';
    tblTablaInterInalam.style.display = 'none';
    //PROY-31812 - IDEA-43340 - FIN


}

function mostrarTablaActivo(tipoProducto) {
    document.getElementById('tblTablaTitulo' + tipoProducto).style.display = '';
    document.getElementById('tblTabla' + tipoProducto).style.display = '';
}

function verificarCondicServRI() {
    //Validar Si es CAC / DAC / Corner
    var strConstCanalCAC = constRoamingCAC;
    var strConstCanalDAC = constRoamingDAC;
    var strConstCanalCorner = constRoamingCorner;

    //Validar Si es MASIVO / B2E / Corporativo
    var strConstMasivo = constRoamingMasivo;
    var strConstB2E = constRoamingB2E;
    var strConstCorporativo = constRoamingCorporativo;

    if (parent.getValue('hidnCanalValue') == strConstCanalCAC || parent.getValue('hidnCanalValue') == strConstCanalDAC || parent.getValue('hidnCanalValue') == strConstCanalCorner) {
        if (parent.getValue('ddlOferta') == strConstMasivo || parent.getValue('ddlOferta') == strConstB2E || parent.getValue('ddlOferta') == strConstCorporativo) {
            //Activar Flag para mostrar Roaming Internacional
            parent.setValue('hidFlagRoamingI', '1');
        }
    }
}

function f_MostrarTab(tipoProducto) {
    //INI PROY-140434
    parent.document.getElementById('trLCDisponible').style.display = 'none';

    if (getValue('hidCodigoTipoProductoActual') == codTipoProdMovil) {
        if (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion || parent.getValue('ddlTipoOperacion') == flujoAlta) {
            parent.document.getElementById('trLCDisponible').style.display = '';
        }
    }
    //FIN PROY-140434
    if (!verificarCombo()) {
        if (!verificarPlanes()) return;
    }

    mostrarAllTablaInactivo();
    mostrarTablaActivo(tipoProducto);

    parent.mostrarAllTabInactivo();
    parent.mostrarTabActivo('td' + tipoProducto);

    //Seteo Codigo Tipo Producto Actual
    setValue('hidTipoProductoActual', tipoProducto);
    verificarCondicServRI();
    var idProducto;
    var desProducto;
    switch (tipoProducto) {
        case 'Movil': setValue('hidCodigoTipoProductoActual', codTipoProdMovil);
            idProducto = '2'; desProducto = 'Móvil';
            if (parent.getValue('hidFlagRoamingI') == '1') {
                parent.setValue('hidFlagRoamingI', '1');
            }
            break;
        case 'Fijo': setValue('hidCodigoTipoProductoActual', codTipoProdFijo);
            desProducto = 'Fijo Inalámbrico';
            if (parent.getValue('hidFlagRoamingI') == '1') {
                parent.setValue('hidFlagRoamingI', '');
            }
            break;
        case 'BAM': setValue('hidCodigoTipoProductoActual', codTipoProdBAM);
            idProducto = '32'; desProducto = tipoProducto;
            if (parent.getValue('hidFlagRoamingI') == '1') {
                parent.setValue('hidFlagRoamingI', '1');
            }
            break;
        //PROY-31812 - IDEA-43340 - INICIO   
        case 'InterInalam': setValue('hidCodigoTipoProductoActual', codTipoProdInterInalam);
            idProducto = '06'; desProducto = tipoProducto;
            if (parent.getValue('hidFlagRoamingI') == '1') {
                parent.setValue('hidFlagRoamingI', '1');
            }
            break;
        //PROY-31812 - IDEA-43340 - FIN   
        case 'DTH': setValue('hidCodigoTipoProductoActual', codTipoProdDTH);
            idProducto = '8'; desProducto = 'Claro TV SAT';
            if (parent.getValue('hidFlagRoamingI') == '1') {
                parent.setValue('hidFlagRoamingI', '');
            }
            break;
        case 'HFC': setValue('hidCodigoTipoProductoActual', codTipoProd3Play);
            desProducto = '3Play';
            if (parent.getValue('hidFlagRoamingI') == '1') {
                parent.setValue('hidFlagRoamingI', '');
            }
            break;
        //FTTH - INICIO  
        case 'FTTH': setValue('hidCodigoTipoProductoActual', codTipoProdFTTH);
            desProducto = 'FTTH';
            if (parent.getValue('hidFlagRoamingI') == '1') {  //Validar el impacto
                parent.setValue('hidFlagRoamingI', '');
            }
            break;
        //FTTH - FIN      
        case 'VentaVarios': setValue('hidCodigoTipoProductoActual', codTipoProdVentaVarios);
            desProducto = 'Venta Varios';
            if (parent.getValue('hidFlagRoamingI') == '1') {
                parent.setValue('hidFlagRoamingI', '');
            }
            break;
        case '3PlayInalam': setValue('hidCodigoTipoProductoActual', codTipoProd3PlayInalam);
            desProducto = '3Play Inalambrico';
            if (parent.getValue('hidFlagRoamingI') == '1') {
                parent.setValue('hidFlagRoamingI', '');
            }
            break;
    }

    autoSizeProducto();

    parent.document.getElementById('lblLCDisponiblexProd').innerHTML = desProducto + ':&nbsp;&nbsp;&nbsp;&nbsp;';
    if ((tipoProducto == 'Fijo') || (tipoProducto == 'HFC') || (tipoProducto == '3PlayInalam'))
        calcularLCxProductoFijo();
    else
        calcularLCxProducto(idProducto);

    //INI PROY-140434
    parent.document.getElementById('trLCDisponible').style.display = 'none';
    if (getValue('hidCodigoTipoProductoActual') == codTipoProdMovil) {
        if (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion || parent.getValue('ddlTipoOperacion') == flujoAlta) {
            parent.document.getElementById('trLCDisponible').style.display = '';
        }
    }
    //FIN PROY-140434

    calcularCFxProducto();

    /*INICIO PROY-140380 - Beneficio Full Claro*/
    var tipoProducto = getValue('hidCodigoTipoProductoActual');
    parent.activaBotonFullClaro(tipoProducto);
    /*FIN PROY-140380 - Beneficio Full Claro*/

    //INICIO PROY-140419 Autorizar Portabilidad sin PIN
    mostrarPanelSMSSupervisor();
    //FIN PROY-140419 Autorizar Portabilidad sin PIN

}

function calcularLCxProducto(idProducto) {
    var strLCDisponiblexProd = parent.document.getElementById('hidLCDisponiblexProd').value;
    var arrLCDisponiblexProd = strLCDisponiblexProd.split('|');
    //INI PROY-140434
    var montoLimite = constLimiteCredito;
    //FIN PROY-140434

    for (var i = 0; i < arrLCDisponiblexProd.length; i++) {
        var array = arrLCDisponiblexProd[i].split(';');
        if (idProducto == array[0]) {
            //INI PROY-140434
            if (parseFloat(array[1]) > parseFloat(montoLimite)) {
                parent.document.getElementById('txtLCDisponiblexProd').value = parseFloat(montoLimite).toFixed(2);
            }
            else {
                parent.document.getElementById('txtLCDisponiblexProd').value = parseFloat(array[1]).toFixed(2);
            }
            //FIN PROY-140434
        }
    }
}

function calcularLCxProductoFijo() {
    var tipoProducto = getValue('hidTipoProductoActual');
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var strProductos = '';
    var dblLC = 0;

    if (codigoTipoProducto == codTipoProdFijo || codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam) {
        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];

            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

            if (idFila.length == 0)
                idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);
            if (codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam) {
                var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
                if (ddlPaquete != null) {
                    strProductos += getValor(ddlPaquete.value, 2) + ',';
                }
            }
            else {
                var ddlPlan = document.getElementById('ddlPlan' + idFila);
                if (ddlPlan != null) {
                    strProductos += getValor(ddlPlan.value, 7) + ',';
                }
            }
        }

        if (strProductos != '') {
            var strLCDisponiblexProd = parent.document.getElementById('hidLCDisponiblexProd').value;
            var arrLCDisponiblexProd = strLCDisponiblexProd.split('|');
            for (var i = 0; i < arrLCDisponiblexProd.length; i++) {
                var array = arrLCDisponiblexProd[i].split(';');
                if (array[0] != '') {
                    if (strProductos.indexOf(array[0]) > 0)
                        dblLC += parseFloat(array[1]);
                }
            }
        }

        parent.document.getElementById('txtLCDisponiblexProd').value = parseFloat(dblLC).toFixed(2);
    }
}

function tieneServicioVOD(idFila) {
    var strResultado = '';
    var strServicio = getValue('hidnPlanServicioValue');
    var arrServicios;
    var strIdFila = '*ID*' + idFila;
    var strCodigo;
    var arrServicio;
    var intPosIni = strServicio.indexOf(strIdFila);
    var intPosFin = 0;

    if (intPosIni > -1) {
        intPosFin = strServicio.substring(intPosIni + 4).indexOf('*ID*');

        if (intPosFin == -1)
            intPosFin = strServicio.length;
        else
            intPosFin += intPosIni + 4;

        strServicio = strServicio.substring(intPosIni, intPosFin);

        arrServicios = strServicio.split('|');

        for (var i = 1; i < arrServicios.length; i++) {
            arrServicio = arrServicios[i].split('_');

            if (arrServicio != undefined) {
                if (arrServicio[13].split(';')[0] == '1')
                    return true;
            }
        }
    }

    return false;
}
function verificarVOD(codTipoProductoActual) {
    var strFlagsVOD = document.getElementById('hidFlagVOD').value;
    var arrVOD;
    var arrVODactual;
    var tabla;
    if (codTipoProductoActual == codTipoProd3Play) {
        tabla = document.getElementById('tblTablaHFC');
    }
    // FTTH Inicio- verificarVOD(codTipoProductoActual) VALIDAR
    if (codTipoProductoActual == codTipoProdFTTH) {
        tabla = document.getElementById('tblTablaFTTH');
    }
    // FTTH Fin- verificarVOD(codTipoProductoActual)
    if (codTipoProductoActual == codTipoProd3PlayInalam) {
        tabla = document.getElementById('tblTabla3PlayInalam');
    }

    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var strFilas = '';
    var strFilasRevisadas = '';
    var strPlan;

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        //evaluo si servicio principal seleccionado tiene vod = 1
        strPlanCodigo = document.getElementById('ddlPlan' + idFila).value;

        if (fila.style.display != 'none') {
            if (strPlanCodigo.split('_')[11] == '1' || tieneServicioVOD(idFila)) {
                strFilas = obtenerFilasGrupo(idFila, '', false) + '';

                if (strFlagsVOD.indexOf('|') > -1) {
                    arrVOD = strFlagsVOD.split('|');

                    for (var j = 0; j < arrVOD.length; j++) {
                        arrVODactual = arrVOD[j].split(':');
                        if (strFilas.indexOf(arrVODactual[0]) && arrVODactual[1] == '0') {
                            alert('El HUB asociado no está habilitado para VOD, no puede realizar la venta de este servicio.')
                            return false;
                        }
                    }
                }
            }
        }
    }

    return true;
}

function agregarCarrito(mostrar) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][agregarCarrito()] ", "Entro en la funcion agregarCarrito()");

    if (!verificarPlanes())
        return;

    var tipoProducto = getValue('hidTipoProductoActual');
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var tablaResumen = document.getElementById('tbResumenCompras');
    var cont = tabla.rows.length;
    var tienePaquete = getValue('hidTienePaquete');
    var fila;
    var idFila;
    var newRow;
    var oCell;

    var strPlazo = '';
    var strPlan = '';
    var strCampana = '';
    var strEquipo = '';
    var strCargoFijoTotal = '';
    var strPrecioVentaEquipo = '';
    var strEquipoEnCuotas = '';
    var strNroCuotas = '';
    var strMontoCuota = '';
    var arrCuota;

    var ddlPlazo;
    var ddlPlan;
    var ddlCampana;
    var ddlEquipo;
    var hidValorEquipo;

    var strAgrupaPaquete;
    var blnAgregarItem;

    var tipoProductoVisual = tipoProducto;

    var ddlCombo = parent.document.getElementById('ddlCombo');
    var strCombo = '';
    var strMontoCuotaInicial; //PROY-30166-IDEA–38863
    if (ddlCombo.value.length > 0)
        strCombo = obtenerTextoSeleccionado(ddlCombo);

    var flagCPPermitida = 0;
    //INI: PROY-140335 RF1
    var numero;
    var E_CP;
    var hidNroTelefono; 
    var hidEstacoCP; 
    //FIN: PROY-140335 RF1

    if (tipoProductoVisual == 'FTTH')
        tipoProductoVisual = 'FTTH';

    if (tipoProductoVisual == 'HFC')
        tipoProductoVisual = '3 Play';

    if (tipoProductoVisual == '3PlayInalam')
        tipoProductoVisual = '3 Play Inalambrico';

    //PROY-31812 - IDEA-43340 - INICIO
    if (tipoProductoVisual == 'InterInalam')
        tipoProductoVisual = 'Internet Inalámbrico';
    //PROY-31812 - IDEA-43340 - FIN

    if (tipoProductoVisual == 'DTH')
        tipoProductoVisual = 'Claro TV SAT';

    if (tipoProductoVisual == 'FIJO' || tipoProductoVisual == 'Fijo')
        tipoProductoVisual = 'FIJO INALAMBRICO';

    if (tipoProductoVisual == 'VENTAVARIOS')
        tipoProductoVisual = 'VENTA VARIOS';

    //PROY-140743 - INI
    if (parent.getValue('ddlTipoOperacion') == '25') {
        tablaResumen.rows[0].cells[1].style.display = 'none';
        tablaResumen.rows[0].cells[3].style.display = 'none';
    }
    //PROY-140743 - FIN

    //INICIATIVA 920
    if (parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuota && parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuotaSinCode) {
        tablaResumen.rows[0].cells[8].style.display = 'none';
        tablaResumen.rows[0].cells[9].style.display = 'none';
        tablaResumen.rows[0].cells[10].style.display = 'none';
        tablaResumen.rows[0].cells[11].style.display = 'none'; //PROY-30166-IDEA–38863 - INICIO
        tablaResumen.rows[0].cells[12].style.display = 'none';
        //PROY-30166-IDEA–38863 - FIN 
    }
    else {
        tablaResumen.rows[0].cells[8].style.display = '';
        tablaResumen.rows[0].cells[9].style.display = '';
        tablaResumen.rows[0].cells[10].style.display = '';
        tablaResumen.rows[0].cells[11].style.display = ''; //PROY-30166-IDEA–38863-INICIO
        tablaResumen.rows[0].cells[12].style.display = '';
        //PROY-30166-IDEA–38863-FIN
    }
    //PROY-29215 INICIO
    //EMEREGENCIA-29215-INICIO
    if (tipoProductoVisual == '3 Play' || tipoProductoVisual == 'Claro TV SAT' || tipoProductoVisual == '3 Play Inalambrico' || tipoProductoVisual == 'FTTH') {
        //EMEREGENCIA-29215-FIN
        tablaResumen.rows[0].cells[14].style.display = '';//Forma de Pago
        tablaResumen.rows[0].cells[15].style.display = '';//Cuotas
        tablaResumen.rows[0].cells[16].style.display = ''; //CM

        ////INICIO PROY-140546
        if (tipoProductoVisual != 'Claro TV SAT') {
            var bEsCanalPermitido = EsValorPermitido(parent.getValue('ddlCanal'), getValue('hidCanalesPermitidosCAI'), ",");
            var nCobroAnticipado = parseInt(parent.document.getElementById("hidCobroAnticipadoInstalacion").value);
            //if (bEsCanalPermitido && nCobroAnticipado > 0) {
            if (bEsCanalPermitido) {
                tablaResumen.rows[0].cells[17].style.display = ''; //MAI
            }
        }
        //FIN PROY-140546
    }
    //PROY-29215 FIN

    //INI: PROY-140335 RF1
    if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (tipoProductoVisual == 'Movil' || tipoProductoVisual == 'BAM')) {
        //tablaResumen.rows[0].cells[17].style.display = '';
		//tablaResumen.rows[0].cells[18].style.display = '';
        tablaResumen.rows[0].cells[18].style.display = ''; //PROY-140546
        tablaResumen.rows[0].cells[19].style.display = ''; //PROY-140546
    } else {
        //tablaResumen.rows[0].cells[17].style.display = 'none';
		//tablaResumen.rows[0].cells[18].style.display = 'none';
        tablaResumen.rows[0].cells[18].style.display = 'none'; //PROY-140546
        tablaResumen.rows[0].cells[19].style.display = 'none'; //PROY-140546
    }
    //FIN: PROY-140335 RF1

    tablaResumen.rows[0].cells[12].style.display = ''; //PROY-30166-IDEA–38863
    if (parent.getValue('ddlModalidadVenta') != codModalidadContratoCede && parent.getValue('ddlModalidadVenta') != codModalidadContratoSinCode) {//INICIATIVA 920
        tablaResumen.rows[0].cells[12].style.display = 'none'; //PROY-30166-IDEA–38863
    }

    if (strCombo.length == 0)
        tablaResumen.rows[0].cells[2].style.display = 'none';
    else
        tablaResumen.rows[0].cells[2].style.display = '';

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        blnAgregarItem = true;
        var movil = "0.00"; //INC000004476511


        if (fila.style.display != 'none') {

            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

            if (idFila.length == 0)
                idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);

            strPlazo = '&nbsp;';
            ddlPlazo = document.getElementById('ddlPlazo' + idFila);

            //INICIATIVA 920
            if (ddlPlazo != null) {
                if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode)
                    strPlazo = obtenerTextoSeleccionado(ddlPlazo).split('-')[0].trim();
                else
                strPlazo = obtenerTextoSeleccionado(ddlPlazo);
            }

            strPlan = '&nbsp;';
            ddlPlan = document.getElementById('ddlPlan' + idFila);
            if (ddlPlan != null)
                strPlan = obtenerTextoSeleccionado(ddlPlan);

            strCampana = '&nbsp;';
            ddlCampana = document.getElementById('ddlCampana' + idFila);
            if (ddlCampana != null) {
                strCampana = obtenerTextoSeleccionado(ddlCampana);
            }

            strEquipo = '&nbsp;';
            ddlEquipo = document.getElementById('hidValorEquipo' + idFila);
            if (ddlEquipo != null) {
                if (ddlEquipo.value != '') {
                    txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
                    if (txtTextoEquipo != null)
                        strEquipo = txtTextoEquipo.value;
                }
            }

            strCargoFijoTotal = calcularCF(idFila);

            //INI: PROY-140335 RF1
            if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (tipoProductoVisual == 'Movil' || tipoProductoVisual == 'BAM')) {

                numero = '&nbsp;';
                hidNroTelefono = document.getElementById('txtNroTelefono' + idFila);
                if (hidNroTelefono != null) {
                    if (hidNroTelefono.value != '') {
                        numero = document.getElementById('txtNroTelefono' + idFila);
                        if (numero != null)
                            numero = numero.value;
                    }
                }

                E_CP = '&nbsp;';
                var LineasSinCP = parent.getValue('hidLineasSinCP');
                hidEstacoCP = document.getElementById('txtEstadoCP' + idFila);
                if (hidEstacoCP != null) {
                    if (hidEstacoCP.value != '') {
                        E_CP = document.getElementById('txtEstadoCP' + idFila);
                        if (E_CP != null)
                            if (LineasSinCP.indexOf(numero) != -1)
                                E_CP = '&nbsp;';
                            else
                                E_CP = E_CP.value;
                    }
                }

            }
            //FIN: PROY-140335 RF1

            strPrecioVentaEquipo = "&nbsp;";
            txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
            if (txtEquipoPrecio != null)
                strPrecioVentaEquipo = (txtEquipoPrecio.value * 1).toFixed(2);

            if (strPrecioVentaEquipo == '') strPrecioVentaEquipo = "&nbsp;";

            //INICIATIVA 920
            if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota || parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode) {
                arrCuota = obtenerCuotaValores(idFila);
                strNroCuotas = parseInt(arrCuota[0]);
                var porcenCuotaIni = arrCuota[1]; //PROY-30166-IDEA–38863
                strMontoCuotaInicial = parseFloat(arrCuota[2]).toFixed(2); ; //PROY-30166-IDEA–38863
                var fltMontoCuotaIni = ((strPrecioVentaEquipo * porcenCuotaIni) / 100).toFixed(2);
                strMontoCuota = ((strPrecioVentaEquipo - fltMontoCuotaIni) / strNroCuotas).toFixed(2);

                strEquipoEnCuotas = 'Si';

                if (isNaN(strNroCuotas))
                    strNroCuotas = '0';

                if (strNroCuotas == '0')
                    strEquipoEnCuotas = 'No';
            }
            else {
                strNroCuotas = '0';
                strEquipoEnCuotas = 'No';
            }


            if (codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam || codigoTipoProducto == codTipoProdFTTH) {
                var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
                if (ddlPaquete != null) {
                    strAgrupaPaquete = obtenerPaqueteActual(idFila);
                }
                var idFilaPaquete = obtenerFilaPaquete(strAgrupaPaquete);
                if (idFila == idFilaPaquete)
                    strCargoFijoTotal = calcularCF3Play(strAgrupaPaquete);
                else
                    blnAgregarItem = false;

                strEquipo = "<img src = '../../Imagenes/ico_lupa.gif' border='0' style='cursor:hand' alt='Ver Equipos' onclick=mostrarEquipoCarrito(" + idFila + "); />";
            }

            if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM)) {
                //                //PROY-140223 IDEA-140462
                  //MODIFICADO POR EL PROY-140335 RF1
//                if (!(parent.validarEnvioConsultaPrevia())) {
//                    flagCPPermitida = document.getElementById('txtFlagCPPermitida' + idFila).value;
//                }
            }

            if (blnAgregarItem && flagCPPermitida == 0) {
                filaCarrito = obtenerFilaCarrito(idFila);

                if (filaCarrito == null) //Si es una fila nueva
                {
                    newRow = tablaResumen.insertRow();

                    var strConcatPrima = document.getElementById('hidConcatPrima').value; //PROY-24724-IDEA-28174 - INICIO
                    var arrConcatPrima;
                    var strPrima = "";
                    var strDeducible = "";
                    var strTotalPrima = parseFloat(getValue('txtTotalPrima'));

                    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][agregarCarrito()] hidnPlanServicioValue ", getValue('hidnPlanServicioValue'));

                    var strTotalAsegurados = hallarOcurrencias(getValue('hidnPlanServicioValue'), "_" + codServProteccionMovil + "_");

                    //INICIATIVA - 803 - INI
                        parent.setValue('hidMontoEquipoVenta', strPrecioVentaEquipo);
                        parent.document.getElementById('hidMontoEquipoVenta').value = strPrecioVentaEquipo
                    //INICIATIVA - 803 - FIN


                    if (strConcatPrima.length > 0) {
                        arrConcatPrima = strConcatPrima.split('|');
                        for (var a = 0; a < arrConcatPrima.length; a++) {
                            if (arrConcatPrima[a].split(';')[0] == idFila) {
                                strPrima = arrConcatPrima[a].split(';')[1];
                                strDeducible = arrConcatPrima[a].split(';')[2];
                                strTotalPrima = (parseFloat(strTotalPrima) + parseFloat(strPrima)).toFixed(2);
                              
                            }
                        }
                        setValue('txtCantEquAsegurados', strTotalAsegurados);
                        setValue('txtTotalPrima', strTotalPrima);

                        if (strTotalPrima > 0) {   //si el codigo de strPrima contiene Datos aparece detalles de proteccion movil INC000004476511 

                            setVisible('tbProteccionMovil', true); //INC000004476511 

                        } else {
                            setVisible('tbProteccionMovil', false); //INC000004476511 
                         }                                       
                        
                    }

                    var cadena = "";

                    if (strPrima != "") {
                        var strConcatItemSeguros = strEquipo + ';S/.' + strPrima + ';' + strDeducible;
                        strConcatItemSeguros = strConcatItemSeguros.replace(new RegExp("[' ']", 'g'), '_');
                        cadena = "<img src='../../Imagenes/ico_lupa.gif' border='0' style='cursor:hand' alt='Protección Móvil' onclick=mostrarPopSeguros(" + '"' + strConcatItemSeguros + '"' + "); />";
                        var tablaResumen = document.getElementById('tbResumenCompras');
                        tablaResumen.rows[i].cells[13].style.display = ''; //PROY-30166-IDEA–38863

                        movil = "0.00"; //INC000004476511
                    }
                    else {

                        cadena = "&nbsp;"; //PROY-24724-IDEA-28174 - FIN
                        movil = "&nbsp;"; //INC000004476511
                    }

               

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = tipoProductoVisual + "<input type='hidden' value=" + idFila + "><input type='hidden' value=" + tipoProducto + ">";
                    oCell.className = 'TablaFilasGrid';

                    //PROY-140743 - INI
                    if (parent.getValue('ddlTipoOperacion') != '25') {
                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strPlazo;
                    oCell.className = 'TablaFilasGrid';
                    }
                    //PROY-140743 - FIN

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strCombo;
                    oCell.className = 'TablaFilasGrid';

                    if (strCombo.length == 0)
                        oCell.style.display = 'none';
                    else
                        oCell.style.display = '';

                    //PROY-140743 - INI
                    if (parent.getValue('ddlTipoOperacion') != '25') {
                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strPlan;
                    oCell.className = 'TablaFilasGrid';
                    }
                    //PROY-140743 - FIN

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strCampana;
                    oCell.className = 'TablaFilasGrid';

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strEquipo;
                    oCell.className = 'TablaFilasGrid';
                    //gaa20151126
                    if (document.getElementById('txtTextoEquipo' + idFila) != null) {
                        if (document.getElementById('txtTextoEquipo' + idFila).style.color != '')
                            oCell.style.color = BloqueoEquipoSinStockColor;
                    }
                    //fin gaa20151126
                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strCargoFijoTotal;
                    oCell.className = 'TablaFilasGrid';

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strPrecioVentaEquipo;
                    oCell.className = 'TablaFilasGrid';

                    //PROY-30166-IDEA–38863–INICIO
                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strMontoCuotaInicial;
                    oCell.className = 'TablaFilasGrid';
                    //INICIATIVA 920
                    if (parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuota && parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuotaSinCode)//INIICIATIVA 920
                        oCell.style.display = 'none';
                    else
                        oCell.style.display = '';
                    //PROY-30166-IDEA–38863–FIN
                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strEquipoEnCuotas;
                    oCell.className = 'TablaFilasGrid';

                    if (parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuota && parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuotaSinCode)//INIICIATIVA 920
                        oCell.style.display = 'none';
                    else
                        oCell.style.display = '';

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strNroCuotas;
                    oCell.className = 'TablaFilasGrid';
                    //INICIATIVA 920
                    if (parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuota && parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuotaSinCode)//INIICIATIVA 920
                        oCell.style.display = 'none';
                    else
                        oCell.style.display = '';

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = strMontoCuota;
                    oCell.className = 'TablaFilasGrid';
                    //INICIATIVA 920
                    if (parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuota && parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuotaSinCode)//INIICIATIVA 920
                        oCell.style.display = 'none';
                    else
                        oCell.style.display = '';

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                 //   oCell.innerHTML = "&nbsp;";
				    oCell.innerHTML = movil;//INC000004476511
                    oCell.className = 'TablaFilasGrid';

                    if (parent.getValue('ddlModalidadVenta') != codModalidadContratoCede && parent.getValue('ddlModalidadVenta') != codModalidadContratoSinCode)
                        oCell.style.display = 'none';
                    else
                        oCell.style.display = '';

                    //PROY-29215 INICIO
                    // EMERGENCIA-29215-INICIO
                    if (tipoProductoVisual == '3 Play' || tipoProductoVisual == 'Claro TV SAT' || tipoProductoVisual == '3 Play Inalambrico' || tipoProductoVisual == 'FTTH') {
                        // EMERGENCIA-29215-FIN
                        oCell = newRow.insertCell();
                        oCell.align = 'center';
                        oCell.innerHTML = "&nbsp;";				
                        oCell.className = 'TablaFilasGrid';

                        oCell = newRow.insertCell();
                        oCell.align = 'center';
                        oCell.innerHTML = "&nbsp;";					 
                        oCell.className = 'TablaFilasGrid';

                        oCell = newRow.insertCell();
                        oCell.align = 'center';
                        oCell.innerHTML = "&nbsp;";
                        oCell.className = 'TablaFilasGrid';

                        //PROY-140546 Cobro Anticipado de Instalacion
                        if (tipoProductoVisual != 'Claro TV SAT') {
                            var bEsCanalPermitido = EsValorPermitido(parent.getValue('ddlCanal'), getValue('hidCanalesPermitidosCAI'), ",");
                            var nCobroAnticipado = parent.document.getElementById("hidCobroAnticipadoInstalacion").value;
                            //alert('nCobroAnticipado: ' + nCobroAnticipado + ' bEsCanalPermitido: ' + bEsCanalPermitido);
                            //if (bEsCanalPermitido && nCobroAnticipado > 0) {
                            if (bEsCanalPermitido) {
                                //16
                                oCell = newRow.insertCell();
                                oCell.align = 'center';
                                oCell.className = 'TablaFilasGrid';
                                oCell.innerHTML = "0";  //parent.document.getElementById('hidMontoAnticipadoInstalacion').value; ;
                            }
                        }                 
                        //PROY-140546 Cobro Anticipado de Instalacion
                    }
                    //PROY-29215 FIN 

                    oCell = newRow.insertCell(); //PROY-24724-IDEA-28174 - INICIO
                    oCell.align = 'center';
                    oCell.innerHTML = cadena;
                    oCell.className = 'TablaFilasGrid';

                    if (parseFloat(getValue('txtCantEquAsegurados')) > 0) {
                        tablaResumen.rows[0].cells[13].style.display = ''; //PROY-30166-IDEA–38863
                        oCell.style.display = '';
						
                    }
                    else {
                        tablaResumen.rows[0].cells[13].style.display = 'none'; //PROY-30166-IDEA–38863
                        oCell.style.display = 'none';
						
                    } //PROY-24724-IDEA-28174 - FIN

                    //INI: PROY-140335 RF1
                    if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM)) {
                        oCell = newRow.insertCell();
                        oCell.align = 'center';
                        oCell.innerHTML = numero;
                        oCell.className = 'TablaFilasGrid';

                        oCell = newRow.insertCell();
                        oCell.align = 'center';
                        oCell.innerHTML = E_CP;
                        oCell.className = 'TablaFilasGrid';
                    }
                    //FIN: PROY-140335 RF1

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                 
                    oCell.innerHTML = "&nbsp;";
                    oCell.className = 'TablaFilasGrid';

                

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = "<img src='../../Imagenes/editar.gif' border='0' style='cursor:hand' alt='Editar Fila' onclick='editarFilaCompra(" + idFila + ", " + (document.getElementById('tbResumenCompras').rows.length - 1) + ")' />";
                    oCell.className = 'TablaFilasGrid';

                    oCell = newRow.insertCell();
                    oCell.align = 'center';
                    oCell.innerHTML = "<img src='../../Imagenes/rechazar.gif' border='0' style='cursor:hand' alt='Eliminar Fila' onclick='eliminarFilaTotal(null, " + idFila + ", true)' />";
                    oCell.className = 'TablaFilasGrid';
                }
                else {

                   

                    filaCarrito.cells[1].innerHTML = strPlazo;
                    filaCarrito.cells[2].innerHTML = strCombo;
                    filaCarrito.cells[3].innerHTML = strPlan;
                    filaCarrito.cells[4].innerHTML = strCampana;
                    filaCarrito.cells[5].innerHTML = strEquipo;
                    filaCarrito.cells[6].innerHTML = strCargoFijoTotal;
                    filaCarrito.cells[7].innerHTML = strPrecioVentaEquipo;
                    filaCarrito.cells[8].innerHTML = strMontoCuotaInicial; //PROY-30166-IDEA–38863-INICIO
                    filaCarrito.cells[9].innerHTML = strEquipoEnCuotas;
                    filaCarrito.cells[10].innerHTML = strNroCuotas;
                    filaCarrito.cells[11].innerHTML = strMontoCuota;                   
                    filaCarrito.cells[12].innerHTML = '&nbsp;'; 
                    filaCarrito.cells[13].innerHTML = '&nbsp;';
                    filaCarrito.cells[14].innerHTML = '&nbsp;'; //PROY-24724-IDEA-28174 //PROY-30166-IDEA–38863-FIN
                }
            }
            fila.style.display = 'none';
        }

        calcularCFCarrito();
    }

    if (parseFloat(getValue('txtCantEquAsegurados')) > 0) { //PROY-24724-IDEA-28174 - INICIO

        setValue('txtCFTotalCarrito', (parseFloat(getValue('txtCFTotalCarrito')) + parseFloat(getValue('txtTotalPrima'))).toFixed(2));
    }

    document.getElementById('btnConsultaPM').onclick = llamarMostrarPopSeguros; //PROY-24724-IDEA-28174 - FIN

    if (mostrar)
        trResumenCompras.style.display = '';

    document.getElementById('txtCFTotal').value = 0;

 
}

//PROY-29215 INICIO
function calcularCM(midFila, obj) {
    var tablaResumen = document.getElementById('tbResumenCompras');
    var filaCarrito = tablaResumen.rows[midFila - 1];
    var objCombo = filaCarrito.cells[14].getElementsByTagName("select")[0];

    //INICIO PROY-140546
    var tipoProducto = filaCarrito.cells[0].innerText;
    var costoInstalacion = 0.0;
    var costoInstalacionReal = 0.0;
    var cobroAnticipadoInstalacion = 0.0;
    var nuevoCostoAnticipadoInstalacion = 0.0;
    if (tipoProducto != 'Claro TV SAT') {
        costoInstalacion = (parseFloat(filaCarrito.cells[12].innerText)).toFixed(2);
        costoInstalacionReal = (parseFloat(filaCarrito.cells[12].innerText)).toFixed(2);
        cobroAnticipadoInstalacion = (parseFloat(parent.document.getElementById("hidCobroAnticipadoInstalacion").value)).toFixed(2);
        var tipoCobroAnticipado = parent.document.getElementById('hidTipoCobroAnticipadoInstalacion').value;
        var nMAI = 0.0;
        var bEsCanalPermitido = EsValorPermitido(parent.getValue('ddlCanal'), getValue('hidCanalesPermitidosCAI'), ",");

        if (bEsCanalPermitido) {
            //alert('tipoCobroAnticipado: ' + tipoCobroAnticipado + ' - cobroAnticipadoInstalacion: ' + cobroAnticipadoInstalacion);
            if (tipoCobroAnticipado == "ABSOLUTO") {
                nuevoCostoAnticipadoInstalacion = costoInstalacion - cobroAnticipadoInstalacion;
            }
            else if (tipoCobroAnticipado == "REFERENCIAL") {
                nMAI = costoInstalacion * (cobroAnticipadoInstalacion / 100);
                nuevoCostoAnticipadoInstalacion = costoInstalacion - nMAI;
				cobroAnticipadoInstalacion = nMAI;
            }
        }
    }
	//FIN PROY-140546    

    for (var o = 0; o < objCombo.length; o++) {
        if (objCombo[o].selected) {
            var valCombo = objCombo[o].text;
        }
    }

    var monto = 0;
    //INICIO PROY-140546
    if (tipoProducto != 'Claro TV SAT') {
        //alert('costoInstalacion: ' + costoInstalacion + ' nuevoCostoAnticipadoInstalacion: ' + nuevoCostoAnticipadoInstalacion + ' cobroAnticipadoInstalacion: ' + cobroAnticipadoInstalacion + ' valCombo: ' + valCombo + ' costoInstalacionReal: ' + costoInstalacionReal);
        if (parseFloat(nuevoCostoAnticipadoInstalacion) > 0) {
            costoInstalacion = nuevoCostoAnticipadoInstalacion;
        }

        if (nuevoCostoAnticipadoInstalacion == 0 || nuevoCostoAnticipadoInstalacion == 0.0 || nuevoCostoAnticipadoInstalacion == "0") {
            //alert('monto0: ' + monto);
			monto = 0;
        }
        else {
            if (parseFloat(cobroAnticipadoInstalacion) > parseFloat(costoInstalacionReal)) {
                //alert('monto1: ' + monto);
                monto = 0;
            }
            else {
                if ((costoInstalacion != "" || costoInstalacion == "0" || costoInstalacion != " " || costoInstalacion != 'undefined' || costoInstalacion != undefined) && valCombo != "0") {
                    monto = costoInstalacion / valCombo;
                    //alert('monto3: ' + monto);
                }
                else {
                    //alert('monto4: ' + monto);
                    monto;
                }
            }
        }
    }
    else {
        var valor0 = filaCarrito.cells[12].innerText;
        if ((valor0 != "" || valor0 == "0" || valor0 != " " || valor0 != 'undefined' || valor0 != undefined) && valCombo != "0") {
            monto = valor0 / valCombo;
        }
        else {
            monto;
        }
    }
    //FIN PROY-140546

    filaCarrito.cells[15].innerHTML = monto;
}
//PROY-29215 FIN 

function llamarMostrarPopSeguros() { //PROY-24724-IDEA-28174 - INICIO
    var strConcatItemsPopUp = getValue('hidConcatItemsPopUp');
    mostrarPopSeguros(strConcatItemsPopUp);
}

function mostrarPopSeguros(strConcatItemsPopUp) {
    var cantItems = strConcatItemsPopUp.split('|').length;
    var altoVentana = "82";
    if (cantItems == 2) altoVentana = "96";
    if (cantItems == 3) altoVentana = "110";
    strConcatItemsPopUp = strConcatItemsPopUp.replace(new RegExp("['ñ']", 'g'), '=');
    strConcatItemsPopUp = strConcatItemsPopUp.replace(new RegExp("[' ']", 'g'), '_');
    var opciones = "dialogHeight: " + altoVentana + "px; dialogWidth: 500px; edge: raised; center: yes; resizable: no; status: no; scroll: no";
    var url = '../evaluacion_cons/sisact_pop_seguros.aspx?concatSeguros=' + strConcatItemsPopUp;
    window.showModalDialog(url, '', opciones);
}

function hallarOcurrencias(string, subString, allowOverlapping) {
    string += "";
    subString += "";
    if (subString.length <= 0) return (string.length + 1);

    var n = 0,
            pos = 0,
            step = allowOverlapping ? 1 : subString.length;

    while (true) {
        pos = string.indexOf(subString, pos);
        if (pos >= 0) {
            ++n;
            pos += step;
        } else break;
    }
    return n;
} //PROY-24724-IDEA-28174 - FIN

//PROY-29215 INICIO 
function obtenerValorCombo(objId) {
    var tablaResumen = document.getElementById('tbResumenCompras');
    var contResumen = tablaResumen.rows.length;
    var cadena = "";

    for (var i = 1; i <= contResumen - 1; i++) {
        var filaCarrito = tablaResumen.rows[i];
        if (filaCarrito != null) {
            // EMERGENCIA-29215-INICIO
            if (filaCarrito.cells[0].innerText == 'Claro TV SAT' || filaCarrito.cells[0].innerText == '3 Play' || filaCarrito.cells[0].innerText == '3 Play Inalambrico' || filaCarrito.cells[0].innerText == 'FTTH') {
                // EMERGENCIA-29215-FIN
                if (objId == 'FP') {
                    var objCombo = filaCarrito.cells[13].getElementsByTagName("select")[0];
                    for (var o = 0; o < objCombo.length; o++) {
                        if (objCombo[o].selected) {
                            var valCombo = objCombo[o].text;
                            cadena += valCombo + '|';
                        }
                    }
                }
                else if (objId == 'CP') {
                    var objCombo = filaCarrito.cells[14].getElementsByTagName("select")[0];
                    for (var o = 0; o < objCombo.length; o++) {
                        if (objCombo[o].selected) {
                            var valCombo = objCombo[o].text;
                            cadena += valCombo + '|';
                        }
                    }
                }
            }
        }

    }
    return cadena;

}
function modificarCuota(midFila, obj) {
    var tipoProducto = getValue('hidTipoProductoActual');
    var tablaResumen = document.getElementById('tbResumenCompras');
    var contResumen = tablaResumen.rows.length;
    var idFila, fila;
    var strCuotaPago = parent.document.getElementById('hidNroCuotas').value;
    var arrCP = strCuotaPago.split("|")

    for (var i = 1; i <= contResumen - 1; i++) {
        var filaCarrito = tablaResumen.rows[i];
        if (filaCarrito != null) {

            var objCombo = filaCarrito.cells[13].getElementsByTagName("select")[0];
            var valCombo;
            for (var o = 0; o < objCombo.length; o++) {
                if (objCombo[o].selected) {
                    valCombo = objCombo[o].text;
                }
            }

            var objCuota = filaCarrito.cells[14].getElementsByTagName("select")[0];
            var objCuotaOpcion = filaCarrito.cells[14].getElementsByTagName("option");
            var ddlCuota = document.getElementById("ddlCuotaPago_" + i);
            var optionCuota = ddlCuota.getElementsByTagName('OPTION');
            var option = document.createElement("option");

            if (valCombo == "CONTRATA") {
                for (var cu = 0; cu < objCuotaOpcion.length; cu++) {
                    objCuota.remove(cu);
                    cu--;
                }

                for (var cp in arrCP) {
                    if (arrCP[cp] != "" && arrCP[cp] == "0") {
                        option.text = arrCP[cp];
                        option.value = arrCP[cp];

                        objCuota.add(option);
                    }
                }
                calcularCM(midFila, obj);
            }
            else {

                for (var cu = 0; cu < objCuotaOpcion.length; cu++) {
                    objCuota.remove(cu);
                    cu--;
                }

                for (var cp in arrCP) {
                    if (arrCP[cp] != "" && arrCP[cp] != "0") {
                        var option1 = document.createElement("option");
                        option1.text = arrCP[cp];
                        option1.value = arrCP[cp];

                        objCuota.add(option1);
                    }
                }
                calcularCM(midFila, obj);
            }
        }
    }
}

function agregarCombo() {
    var tipoProducto = getValue('hidTipoProductoActual');
    var tablaResumen = document.getElementById('tbResumenCompras');
    var contResumen = tablaResumen.rows.length;
    var idFila, fila;
    var strFormaPago = parent.document.getElementById('hidFormaPago').value;
    var strCuotaPago = parent.document.getElementById('hidNroCuotas').value;
    var strCobroAnticipadoInstalacion = parent.document.getElementById('hidCobroAnticipadoInstalacion').value; //PROY-140546

    for (var i = 1; i <= contResumen - 1; i++) {
        //if (arrIdFilas[i] != '') {
        var filaCarrito = tablaResumen.rows[i];
        if (filaCarrito != null) {
            // EMERGENCIA-29215-INICIO
            if (filaCarrito.cells[0].innerText == 'Claro TV SAT' || filaCarrito.cells[0].innerText == '3 Play' || filaCarrito.cells[0].innerText == '3 Play Inalambrico' || filaCarrito.cells[0].innerText == 'FTTH') {
                // EMERGENCIA-29215-FIN
                var type1 = document.createElement("select");
                var type2 = document.createElement("select");
                var arrFP = strFormaPago.split("|").reverse();
                var arrCP = strCuotaPago.split("|").reverse();

                filaCarrito.cells[13].name = "ddlFormaPago_" + i;
                filaCarrito.cells[13].id = "ddlFormaPago_" + i;
                filaCarrito.cells[14].name = "ddlCuotaPago_" + i;
                filaCarrito.cells[14].id = "ddlCuotaPago_" + i;

                for (var fp in arrFP) {
                    var optionFP = document.createElement("option");
                    if (arrFP[fp] != "" && arrFP[fp] == "FACTURACION") {
                        optionFP.text = arrFP[fp];
                        optionFP.value = arrFP[fp];
                        type1.add(optionFP);

                        if (arrFP.length < 3) {
                            for (var cp in arrCP) {
                                var optionCP = document.createElement("option");

                                if (arrCP[cp] != "" && arrCP[cp] != "0") {
                                    optionCP.text = arrCP[cp];
                                    optionCP.value = arrCP[cp];
                                    type2.add(optionCP);
                                }
                            }
                        }
                    }
                    else if (arrFP[fp] != "" && arrFP[fp] == "CONTRATA") {
                        optionFP.text = arrFP[fp];
                        optionFP.value = arrFP[fp];
                        type1.add(optionFP);

                        for (var cp in arrCP) {
                            var optionCP = document.createElement("option");
                            if (arrCP[cp] != "" && arrCP[cp] == "0") {
                                optionCP.text = arrCP[cp];
                                optionCP.value = arrCP[cp];
                                type2.add(optionCP);
                            }
                        }
                    }
                }
                filaCarrito.cells[13].appendChild(type1);

                if (window.addEventListener) {
                    type1.addEventListener("change", function () { modificarCuota(i, "ddlCuotaPago_"); });
                    type2.addEventListener("change", function () { calcularCM(i, "ddlCuotaPago_"); });
                } else if (window.attachEvent) {
                    type1.attachEvent("onchange", function () { modificarCuota(i, "ddlCuotaPago_"); });
                    type2.attachEvent("onchange", function () { calcularCM(i, "ddlCuotaPago_"); });
                }

                filaCarrito.cells[14].appendChild(type2);

                var combo = document.getElementById("ddlCuotaPago_" + i);
                var obj = filaCarrito.cells[14].getElementsByTagName("select")[0];
                var costoInstalacion = 0; //PROY-140546

                //INICIO PROY-140546
                var costoInstalacionReal = 0;
                var montoAnticipadoInstalacion = (parseFloat(strCobroAnticipadoInstalacion)).toFixed(2);
                var nuevoCostoAnticipadoInstalacion = 0.0;
                if (filaCarrito.cells[0].innerText != 'Claro TV SAT') {
                    costoInstalacion = (parseFloat(filaCarrito.cells[12].innerText)).toFixed(2);
                    costoInstalacionReal = (parseFloat(filaCarrito.cells[12].innerText)).toFixed(2);
                    var tipoCobroAnticipado = parent.document.getElementById('hidTipoCobroAnticipadoInstalacion').value;
                    var nMAI = 0.0;
                    var bEsCanalPermitido = EsValorPermitido(parent.getValue('ddlCanal'), getValue('hidCanalesPermitidosCAI'), ",");

                    if (bEsCanalPermitido) {
                        //alert('tipoCobroAnticipadoX: ' + tipoCobroAnticipado + ' - montoAnticipadoInstalacionX: ' + montoAnticipadoInstalacion);
                        if (tipoCobroAnticipado == "ABSOLUTO") {
                            nuevoCostoAnticipadoInstalacion = costoInstalacion - montoAnticipadoInstalacion;
                            filaCarrito.cells[16].innerHTML = montoAnticipadoInstalacion;
                        }
                        else if (tipoCobroAnticipado == "REFERENCIAL") {
                            nMAI = costoInstalacion * (montoAnticipadoInstalacion / 100);
                            filaCarrito.cells[16].innerHTML = nMAI;
                            nuevoCostoAnticipadoInstalacion = costoInstalacion - nMAI;
							montoAnticipadoInstalacion = nMAI;
                        }						
                    }
                }
                else {
                    costoInstalacion = filaCarrito.cells[11].innerText;
                }
				//FIN PROY-140546

                for (var o = 0; o < obj.length; o++) {
                    if (obj[o].selected) {
                        var valCombo = obj[o].text;
                    }
                }

                var valorCM = 0;

                //INICIO PROY-140546
                if (filaCarrito.cells[0].innerText != 'Claro TV SAT') {
                    //alert('costoInstalacion: ' + costoInstalacion + ' nuevoCostoAnticipadoInstalacion: ' + nuevoCostoAnticipadoInstalacion + ' montoAnticipadoInstalacion: ' + montoAnticipadoInstalacion + ' valCombo: ' + valCombo + ' costoInstalacionReal: ' + costoInstalacionReal);
                    if (parseFloat(nuevoCostoAnticipadoInstalacion) > 0) {
                        costoInstalacion = nuevoCostoAnticipadoInstalacion;
                    }

                    if (nuevoCostoAnticipadoInstalacion == 0 || nuevoCostoAnticipadoInstalacion == 0.0 || nuevoCostoAnticipadoInstalacion == "0") {
                        //alert('valorCM0: ' + valorCM);
                        valorCM = 0;
                    }
                    else {
                        if (parseFloat(montoAnticipadoInstalacion) > parseFloat(costoInstalacionReal)) {
                            //alert('valorCM1: ' + valorCM);
                            valorCM = 0;
                        }
                        else {
                            if (costoInstalacion != "" && valCombo != "0") {
                                valorCM = costoInstalacion / valCombo;
                                //alert('valorCM2: ' + valorCM);
                            }
                            else {
                                //alert('valorCM3: ' + valorCM);
                                valorCM;
                            }
                        }
                    }
                }
                else {
                    if (costoInstalacion != "" && valCombo != "0") {
                        valorCM = costoInstalacion / valCombo;
                    }
                    else {
                        valorCM;
                    }
                }
                //FIN PROY-140546
                
                filaCarrito.cells[15].innerHTML = valorCM;
            }
        }
    }
    // EMERGENCIA 2-29215-INICIO
    if (filaCarrito.cells[0].innerText == 'Claro TV SAT' || filaCarrito.cells[0].innerText == '3 Play' || filaCarrito.cells[0].innerText == '3 Play Inalambrico' || filaCarrito.cells[0].innerText == 'FTTH') {
        calcularCM(i, "ddlCuotaPago_");
    }
    // EMERGENCIA 2-29215-FIN
}
//PROY-29215 FIN 

function cambiarLucesCarrito(strPlanAutonomia) {
    var arrPlanAutonomia = strPlanAutonomia.split('|');
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual'); //INI: PROY-140335 RF1
    var idFila;
    var flgAutonomia;
    var costoInstalacion;
    var TipoCobroAnticipadoInstalacion = parent.getValue("hidTipoCobroAnticipadoInstalacion"); ////PROY-140546
    var CobroAnticipadoInstalacion = parent.getValue("hidCobroAnticipadoInstalacion"); ////PROY-140546

    for (var x = 0; x < arrPlanAutonomia.length; x++) {
        if (arrPlanAutonomia[x] != '') {
            idFila = arrPlanAutonomia[x].split(';')[0];
            flgAutonomia = arrPlanAutonomia[x].split(';')[1];
            costoInstalacion = arrPlanAutonomia[x].split(';')[2];
            var imagen = '&nbsp;';

            // Cambio Temporal
            if (parent.getValue('ddlCanal') != constCodTipoOficinaCorner) {
                if (parent.getValue('hidCreditosxLineaDesactiva') == 'S') {
                    imagen = "<img src='../../Imagenes/imgColorAmarillo.gif' border='0' style='height:16px; width:16px' alt='Requiere ir a créditos' />";
                }
                else {
                    switch (flgAutonomia) {
                        case 'S':
                            imagen = "<img src='../../Imagenes/imgColorVerde.gif' border='0' style='height:16px; width:16px' alt='Aprobado' />";
                            break;
                        case 'N':
                            imagen = "<img src='../../Imagenes/imgColorAmarillo.gif' border='0' style='height:16px; width:16px' alt='Requiere ir a créditos' />";
                            break;
                        case 'SIN_REGLAS':
                            imagen = "<img src='../../Imagenes/imgColorAmarillo.gif' border='0' style='height:16px; width:16px' alt='Requiere ir a créditos' />";
                            break;
                        case 'NO_CONDICION':
                            imagen = "<img src='../../Imagenes/imgColorRojo.gif' border='0' style='height:16px; width:16px' alt='No aplica condiciones de venta' />";
                            break;
                    }
                }
            }

            var strIdFilas = obtenerFilasGrupo(idFila, '', true)
            var arrIdFilas = strIdFilas.split(',');
            for (var i = 0; i < arrIdFilas.length; i++) {
                if (arrIdFilas[i] != '') {
                    var filaCarrito = obtenerFilaCarrito(arrIdFilas[i]);
                    if (filaCarrito != null) {
                        //PROY-29215 INICIO 
                        // EMERGENCIA-29215-INICIO
                        if (filaCarrito.cells[0].innerText == 'Claro TV SAT' || filaCarrito.cells[0].innerText == '3 Play' || filaCarrito.cells[0].innerText == '3 Play Inalambrico' || filaCarrito.cells[0].innerText == 'FTTH') {
                            //PROY-140546 Cobro Anticipado de Instalacion
                            if (filaCarrito.cells[0].innerText != 'Claro TV SAT') {
                                filaCarrito.cells[12].innerHTML = costoInstalacion;
                                filaCarrito.cells[16].innerHTML = CalcularMAI(costoInstalacion, CobroAnticipadoInstalacion, TipoCobroAnticipadoInstalacion);
                                filaCarrito.cells[18].innerHTML = imagen;
                            }
                            else {
                                filaCarrito.cells[12].innerHTML = costoInstalacion;
                                filaCarrito.cells[17].innerHTML = imagen;
                            }                            
                            //PROY-140546 Cobro Anticipado de Instalacion
                        }
                        else
                         //INI: CAMBIADO POR EL PROY-140335 RF1

                         if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM)) {
                            filaCarrito.cells[16].innerHTML = imagen;
                        } else if (parent.getValue('ddlTipoOperacion') == '25') {//PROY-140743
                            filaCarrito.cells[12].innerHTML = imagen;
                        } else {
                            filaCarrito.cells[14].innerHTML = imagen;
                        }
                        //FIN: CAMBIADO POR EL PROY-140335 RF1 
                        //PROY-29215 FIN
                    }
                }
            }
        }
    }
}

//INICIO PROY-140546
function CalcularMAI(pCostoInstalacion, pCobroAnticipadoInstalacion, pTipoCAI) {
    var nMAI = 0;
    if (pTipoCAI == "ABSOLUTO") {
        //nMAI = pCostoInstalacion - pCobroAnticipadoInstalacion;
        nMAI = pCobroAnticipadoInstalacion;
    }
    else if (pTipoCAI == "REFERENCIAL") {
        nMAI = pCostoInstalacion * (pCobroAnticipadoInstalacion / 100);
    }

    //alert('pTipoCAI: ' + pTipoCAI + ' pCostoInstalacion: ' + pCostoInstalacion + ' pCobroAnticipadoInstalacion: ' + pCobroAnticipadoInstalacion + ' nMAI: ' + nMAI);

    return nMAI;
}
//FIN PROY-140546

function tieneProductosFueraCarrito() {
    var fila;

    var tablaMovil = tblTablaMovil;
    var tablaFijo = tblTablaFijo;
    var tablaBAM = tblTablaBAM;
    var tablaDTH = tblTablaDTH;
    var tablaHFC = tblTablaHFC;
    var tablaFTTH = tblTablaFTTH; // FTTH -tieneProductosFueraCarrito() 

    var tabla3PlayInalam = tblTabla3PlayInalam;
    var cont = tablaMovil.rows.length;

    for (var i = 0; i < cont; i++) {
        fila = tablaMovil.rows[i];
        if (fila.style.display != 'none')
            return true;
    }

    var cont = tablaFijo.rows.length;

    for (var i = 0; i < cont; i++) {
        fila = tablaFijo.rows[i];
        if (fila.style.display != 'none')
            return true;
    }

    var cont = tablaBAM.rows.length;

    for (var i = 0; i < cont; i++) {
        fila = tablaBAM.rows[i];
        if (fila.style.display != 'none')
            return true;
    }

    var cont = tablaDTH.rows.length;

    for (var i = 0; i < cont; i++) {
        fila = tablaDTH.rows[i];
        if (fila.style.display != 'none')
            return true;
    }

    var cont = tablaHFC.rows.length;

    for (var i = 0; i < cont; i++) {
        fila = tablaHFC.rows[i];
        if (fila.style.display != 'none')
            return true;
    }
    var cont = tabla3PlayInalam.rows.length;

    for (var i = 0; i < cont; i++) {
        fila = tabla3PlayInalam.rows[i];
        if (fila.style.display != 'none')
            return true;
    }

    return false;
}

function obtenerFilaCarrito(idFila) {
    var tablaResumen = document.getElementById('tbResumenCompras');
    var cont = tablaResumen.rows.length;
    var fila;
    var idFilaX;

    for (var i = 0; i < cont; i++) {
        fila = tablaResumen.rows[i];

        hidFila = fila.cells[0].getElementsByTagName("input")[0];

        if (hidFila != null) {
            idFilaX = hidFila.value;

            if (idFila == idFilaX)
                return fila;
        }
    }
}

function obtenerFilaPaquete(strAgrupaPaquete) {
    var idFila = '';
    var strGrupo = strAgrupaPaquete.replace('{', '').replace('}', '');
    var arrGrupo = strGrupo.split(',');
    for (i = 0; i < arrGrupo.length; i++) {
        if (arrGrupo[i] != '') {
            idFila = arrGrupo[i].replace('[', '').replace(']', '');
            break;
        }
    }
    return idFila;
}

function calcularCF3Play(strGrupoPaquete) {
    var tabla;
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');

    if (codigoTipoProductoActual == codTipoProd3Play) {
        tabla = document.getElementById('tblTablaHFC');
    };
    //FTTH  - Inicio - calcularCF3Play(strGrupoPaquete) //SETEABA CERO EL COSTO
    if (codigoTipoProductoActual == codTipoProdFTTH) {
        tabla = document.getElementById('tblTablaFTTH');
    };
    //FTTH - Fin  - calcularCF3Play(strGrupoPaquete)

    if (codigoTipoProductoActual == codTipoProd3PlayInalam) {
        tabla = document.getElementById('tblTabla3PlayInalam');
    };


    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var strCargoFijo = 0;

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        var ddlPaquete = document.getElementById('ddlPaquete' + idFila);
        if (ddlPaquete != null) {
            strPlan = obtenerTextoSeleccionado(ddlPaquete);
            strAgrupaPaquete = obtenerPaqueteActual(idFila);
        }
        if (strGrupoPaquete == strAgrupaPaquete) {
            strCargoFijo += parseFloat(calcularCF(idFila));
        }
    }

    return strCargoFijo.toFixed(2);
}

function calcularCF(idFila) {
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var strPlan = '';
    //codTipoProdFT  NO tome en cuenta en  calcularCF
    if ((codigoTipoProductoActual != codTipoProdFTTH) && (codigoTipoProductoActual != codTipoProd3Play) && (codigoTipoProductoActual != codTipoProd3PlayInalam)) { //FTTH
        if (ddlPlan != null)
            strPlan = ddlPlan.value;
    }
    else
        strPlan = document.getElementById('ddlServicio' + idFila).value;

    var fltPlanCF = getValor(strPlan, 1);
    var txtCFPlanServicio = document.getElementById('txtCFPlanServicio' + idFila);
    var txtMontoServicios = document.getElementById('hidMontoServicios' + idFila);
    var fltMontoServicios = 0;

    if (txtMontoServicios != null)
        fltMontoServicios = txtMontoServicios.value;

    var hidMontoCuota = document.getElementById('hidMontoCuota' + idFila);
    var hidMontoEquipo = document.getElementById('hidMontoEquipo' + idFila);
    var fltMontoEquipos;
    var fltMontoCuota = 0;
    if (hidMontoCuota != null)
        fltMontoCuota = hidMontoCuota.value;

    if (isNaN(fltPlanCF))
        fltPlanCF = 0;

    if (isNaN(fltMontoServicios))
        fltMontoServicios = 0;

    if (isNaN(fltMontoCuota))
        fltMontoCuota = 0;

    if (hidMontoEquipo != null)
        fltMontoEquipos = hidMontoEquipo.value

    if (isNaN(fltMontoEquipos))
        fltMontoEquipos = 0;

    if (fltPlanCF.length == 0) fltPlanCF = 0;
    if (fltMontoServicios.length == 0) fltMontoServicios = 0;
    if (fltMontoCuota.length == 0) fltMontoCuota = 0;
    if (fltMontoEquipos.length == 0) fltMontoEquipos = 0;

    if (txtCFPlanServicio != null)
        txtCFPlanServicio.value = (parseFloat(fltPlanCF) + parseFloat(fltMontoServicios)).toFixed(2);

    var txtSubTotalCF = document.getElementById('txtSubTotalCF' + idFila);
    if (getValue('hidCodigoTipoProductoActual') != codTipoProdDTH)
        return (parseFloat(fltPlanCF) + parseFloat(fltMontoServicios) + parseFloat(fltMontoCuota) + parseFloat(fltMontoEquipos)).toFixed(2);
    else {
        var fltEquipoPrecio = document.getElementById('txtCFTotMensual' + idFila).value;

        if (fltEquipoPrecio.length == 0)
            fltEquipoPrecio = 0;

        fltEquipoPrecio = parseFloat(fltEquipoPrecio);

        if (fltEquipoPrecio > 0)
            return fltEquipoPrecio;
        else
            return (parseFloat(fltPlanCF) + parseFloat(fltMontoServicios)).toFixed(2);
    }
}

function editarFilaCompra(idFila, FilaOK) {
    var strIdFilas = obtenerFilasGrupo(idFila, '', true) + '';
    strIdFilas = strIdFilas.replace('{', '').replace('}', '').replace('[', '').replace(']', '');
    var arrIdFilas = strIdFilas.split(',');
    var tablaResumen = document.getElementById('tbResumenCompras');
    var cont = tablaResumen.rows.length;
    var fila;
    var idFilaX;
    var hidFila;
    var booMostrarTab = false;
    var strTipoProducto = '';
    var strTienePaquete = getValue('hidTienePaquete');
    //  INC000002267567
    if (!(parent.validarEnvioConsultaPrevia())) {
        parent.document.getElementById('btnConsultaPrevia').disabled = true; //fdq // Habilitamos botón Consulta
        parent.document.getElementById('tdCarrito').style.display = 'none'; //fdq //Ocultamos Carrito
    }

    setValue('hidContRC', cont);
    setValue('hidFilaRS', FilaOK);

    for (var i = 0; i < cont; i++) {
        fila = tablaResumen.rows[i];

        hidFila = fila.cells[0].getElementsByTagName("input")[0];

        if (hidFila != null) {
            idFilaX = hidFila.value;

            for (var k = 0; k < arrIdFilas.length; k++) {
                if (arrIdFilas[k] == idFilaX) {
                    if (!booMostrarTab) {
                        //f_MostrarTab(fila.cells[0].innerText);
                        strTipoProducto = fila.cells[0].getElementsByTagName("input")[1].value;
                        f_MostrarTab(strTipoProducto);
                    }

                    if (!(fila.cells[12].innerHTML == "&nbsp;")) { //PROY-24724-IDEA-28174 - INICIO
                        var arrConcatItemsPopUp = getValue('hidConcatItemsPopUp').split('|');
                        var arrConcatPrima = getValue('hidConcatPrima').split('|');
                        var arrConcatPrimaItem;

                        for (var a = 0; a < arrConcatPrima.length; a++) {
                            if (arrConcatPrima[a].split(';')[0] == idFilaX) {
                                arrConcatPrimaItem = arrConcatPrima[a].split(';');
                              //  setValue('txtTotalPrima', (getValue('txtTotalPrima') - arrConcatPrimaItem[0]).toFixed(2)); 
				                setValue('txtTotalPrima', (getValue('txtTotalPrima') - arrConcatPrimaItem[1]).toFixed(2)); //INC000004476511
                                setValue('txtCantEquAsegurados', getValue('txtCantEquAsegurados') - 1);
                                arrConcatItemsPopUp.splice(a, 1);
                                arrConcatPrima.splice(a, 1);
							  
                                break; // INC000004476511 reemplazo el a-- por el break
                            }
                        }
                        setValue('hidConcatItemsPopUp', arrConcatItemsPopUp.join('|'));
                        setValue('hidConcatPrima', arrConcatPrima.join('|'));
                    } //PROY-24724-IDEA-28174 - FIN

                    tablaResumen.deleteRow(i);
                    eliminarItem(arrIdFilas[k]);
                    cont--;
                    i--;

                    var flagOcultarProteccionMovil = true; //PROY-24724-IDEA-28174 - INICIO
                    if (tablaResumen.rows.length > 1) {
                        for (var a = 1; a < tablaResumen.rows.length; a++) {
                            if (!(tablaResumen.rows[a].cells[13].innerHTML == "&nbsp;")) { //PROY-30166-IDEA–38863
                                flagOcultarProteccionMovil = false;
                                break;
                            }
                        }
                        if (flagOcultarProteccionMovil) {
                            setVisible('tbProteccionMovil', false);
                            for (var a = 0; a < tablaResumen.rows.length; a++) {
                                tablaResumen.rows[a].cells[13].style.display = 'none'; //PROY-30166-IDEA–38863
                            }
                        }
                    }
                    else {
                        setVisible('tbProteccionMovil', false);
                        tablaResumen.cells[13].style.display = 'none'; //PROY-30166-IDEA–38863
                    } //PROY-24724-IDEA-28174 - FIN

                    arrIdFilas.splice(k, 1);
                }

                eliminarItem(arrIdFilas[k]);
            }
        }
    }

    arrIdFilas = strIdFilas.split(',');

    for (var x = 0; x < arrIdFilas.length; x++) {
        idFilaX = arrIdFilas[x];

        if (idFilaX.length > 0) {
            var ddlPlazo = document.getElementById('ddlPlazo' + idFilaX);
            if (ddlPlazo != null)
                ddlPlazo.parentElement.parentElement.style.display = '';
            else
                document.getElementById('ddlCampana' + idFilaX).parentElement.parentElement.style.display = '';

            editarFila(idFilaX, true);
            //gaa20130523
            var imgEditarFila = document.getElementById('imgEditarFila' + idFila);

            if (document.getElementById('tblTabla' + strTipoProducto).rows.length == 1 && imgEditarFila != null) {
                if (strTienePaquete != 'S') {
                    if (ddlPlazo != null) {
                        if (!parent.document.getElementById('ddlOferta').disabled)
                            ddlPlazo.disabled = false;
                    }
                }
            }

            //fin gaa20130523
            if (strTienePaquete == 'S' || verificarCombo()) {
                var ddlCampana = document.getElementById('ddlCampana' + idFilaX);
                if (ddlCampana != null)
                    ddlCampana.disabled = true;
            }
        }
    }

    calcularCFCarrito();
    setValue('txtCFTotalCarrito', (parseFloat(getValue('txtCFTotalCarrito')) + parseFloat(getValue('txtTotalPrima'))).toFixed(2)); //PROY-24724-IDEA-28174
    calcularCFxProducto();
    evaluar();
    //PROY-30748
    setValue('hidFilaRS', '');
    //INI: PROY-140335 RF1
    LimpiarCamposPortabilidad();
    //FIN: PROY-140335 RF1
}

function eliminarFilaCompra(idFila, valorReemplazoGrupo) {
    var strIdFilas = obtenerFilasGrupo(idFila, valorReemplazoGrupo, true) + '';
    strIdFilas = strIdFilas.replace('{', '').replace('}', '').replace('[', '').replace(']', '');
    var arrIdFilas = strIdFilas.split(',');
    var tablaResumen = document.getElementById('tbResumenCompras');
    var cont = tablaResumen.rows.length;
    var fila;
    var idFilaX;
    var hidFila;
    var booElimino = false;

    for (var x = 0; x < arrIdFilas.length; x++) {
        for (var i = 0; i < cont; i++) {
            fila = tablaResumen.rows[i];

            hidFila = fila.cells[0].getElementsByTagName("input")[0];

            if (hidFila != null) {
                idFilaX = hidFila.value;

                if (arrIdFilas[x] == idFilaX) {
                    booElimino = true;

                    if (!(fila.cells[12].innerHTML == "&nbsp;")) { //PROY-24724-IDEA-28174 - INICIO
                        var arrConcatItemsPopUp = getValue('hidConcatItemsPopUp').split('|');
                        var arrConcatPrima = getValue('hidConcatPrima').split('|');
                        var arrConcatPrimaItem;

                        for (var a = 0; a < arrConcatPrima.length; a++) {
                            if (arrConcatPrima[a].split(';')[0] == idFilaX) {
                                arrConcatPrimaItem = arrConcatPrima[a].split(';');
                                setValue('txtTotalPrima', (getValue('txtTotalPrima') - arrConcatPrimaItem[1]).toFixed(2));//INC000004476511
                                setValue('txtCantEquAsegurados', getValue('txtCantEquAsegurados') - 1);
                                arrConcatItemsPopUp.splice(a, 1);
                                arrConcatPrima.splice(a, 1);
                                break;// INC000004476511 reemplazo el a-- por el break
                            }
                        }
                        setValue('hidConcatItemsPopUp', arrConcatItemsPopUp.join('|'));
                        setValue('hidConcatPrima', arrConcatPrima.join('|'));
                    } //PROY-24724-IDEA-28174 - FIN

                    tablaResumen.deleteRow(i);

                    cont--;
                    i--;

                    var flagOcultarProteccionMovil = true; //PROY-24724-IDEA-28174 - INICIO
                    if (tablaResumen.rows.length > 1) {
                        for (var a = 1; a < tablaResumen.rows.length; a++) {
                            if (!(tablaResumen.rows[a].cells[13].innerHTML == "&nbsp;")) { //PROY-30166-IDEA–38863
                                flagOcultarProteccionMovil = false;
                                break;
                            }
                        }
                        if (flagOcultarProteccionMovil) {
                            setVisible('tbProteccionMovil', false);
                            for (var a = 0; a < tablaResumen.rows.length; a++) {
                                tablaResumen.rows[a].cells[13].style.display = 'none'; //PROY-30166-IDEA–38863
                            }
                        }
                    }
                    else {
                        setVisible('tbProteccionMovil', false);
                        tablaResumen.cells[13].style.display = 'none' //PROY-30166-IDEA–38863
                    } //PROY-24724-IDEA-28174 - FIN
                }
            }
        }
    }

    // Calcular nuevamente rentas
    if (booElimino) {
        calcularCFCarrito();
        evaluar();
    }
}

function evaluar() {
    if (getValue('hidCadenaDetalle') == '') {
        trResumenCompras.style.display = 'none';
        document.getElementById('txtCFTotalCarrito').value = 0;

        parent.trAdjuntoPorta.style.display = 'none';
        parent.inicializarPanelResultado();
        parent.inicializarPanelComentarios();
        parent.inicializarPanelGrabar();
        parent.InicializarPanelExepcionPrecio(); // INICIATIVA 803
    }
    else {
        setValue('hidFlgOrigen', 'EDIT');
        parent.consultaReglasCreditos();
    }
}

function obtenerFilasGrupo(idFila, valorReemplazoGrupo, booQuitarCorchetes) {
    var hidNGrupoPaqueteValues = document.getElementById('hidNGrupoPaqueteValues');
    var strGrupoPaquete = hidNGrupoPaqueteValues.value;
    var strVB = '[' + idFila + ']';
    var intPosFin = strGrupoPaquete.indexOf(strVB);
    var intPosIni;
    var intPosFin;
    var arrGrupo;
    var strResultado = '';

    if (intPosFin > -1) {
        intPosIni = strGrupoPaquete.substring(0, intPosFin).lastIndexOf('{') + 1;
        intPosFin = strGrupoPaquete.substring(intPosIni).indexOf('}') + intPosIni;

        arrGrupo = strGrupoPaquete.substring(intPosIni, intPosFin).split(',')

        for (var i = 1; i < arrGrupo.length; i++) {
            if (booQuitarCorchetes)
                strResultado += ',' + arrGrupo[i].replace('[', '').replace(']', '');
            else
                strResultado += ',' + arrGrupo[i];
        }

        hidNGrupoPaqueteValues.value = strGrupoPaquete.replace(valorReemplazoGrupo, '');

        return strResultado;
    }
    else
        return idFila;
}

function eliminarFilaTotal(fila, idFila, mostrarAdvertencia) {
    parent.document.getElementById("btnFullClaro2").style.display = "none";  //FullClaro.v2
    //gaa20160211
    if (!validarEliminarFila(idFila))
        return;
    //fin gaa20160211

    var pkPortabilidad = '0';

    if (parent.getValue('hidNTienePortabilidadValues') == 'S') {
        var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
        if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM)) {
            //PROY-140223 IDEA-140462
            if (!(parent.validarEnvioConsultaPrevia())) {
                var pkPortabilidad = document.getElementById('txtIdPortabilidad' + idFila).value;
            }
        }
    }

    var valor = eliminarFilaGrupo(fila, idFila, mostrarAdvertencia, false);
    if (valor != false)
        eliminarFilaCompra(idFila, valor);

    calcularCFCarrito();
   
    setValue('txtCFTotalCarrito', (parseFloat(getValue('txtCFTotalCarrito')) + parseFloat(getValue('txtTotalPrima'))).toFixed(2)); //PROY-24724-IDEA-28174
    calcularLCxProductoFijo();

    // Validación Modalidad / Operador Cedente
    if (parent.getValue('hidNTienePortabilidadValues') == 'S') {
        var tipoProducto = getValue('hidTipoProductoActual');
        var tabla = document.getElementById('tblTabla' + tipoProducto);
        if (tabla.rows.length == 0) {
            parent.document.getElementById('ddlModalidadPorta').disabled = false;
            parent.document.getElementById('ddlOperadorCedente').disabled = false;
            parent.document.getElementById('ddlModalidadPorta').selectedIndex = 0;

            llenarDatosCombo(parent.document.getElementById('ddlOperadorCedente'), '', true);

            //PROY-140223 IDEA-140462
            if (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM) {
                if (parent.validarEnvioConsultaPrevia()) {
                    parent.document.getElementById('tdCarrito').style.display = 'inline';
                }
                else {
                    parent.document.getElementById('tdCarrito').style.display = 'none';
                    parent.document.getElementById('btnAgregarPlan').disabled = false;
                    parent.document.getElementById('btnConsultaPrevia').disabled = false;
                }
            }
            //PROY-140223 IDEA-140462

        }

        if (pkPortabilidad != "" && pkPortabilidad != "0") {
            eliminarLineasCPPortabilidad(pkPortabilidad);
        }

        //INI: PROY-140335 RF1
        if (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM) {
            parent.document.getElementById('tdCarrito').style.display = '';
            parent.document.getElementById('tdConsultaPrevia').style.display = 'none';
            parent.document.getElementById('btnConsultaPrevia').value = "Consulta Previa";
            //estructuraGrillaPortabilidad(); //MODIFICADO JCC PENDIENTE BORRAR
        }
        //FIN: PROY-140335 RF1
    }
    parent.f_EliminarTemporal(idFila); //30748
    parent.autoSizeIframe();

    document.getElementById('tbTopeConsumosLTE').style.display = 'none';
    document.getElementById('tbTopeConsumos').style.display = 'none';

    var TelefonosSMS = ObtenerTelefonoSMS();

    //INC000002510501
    if (parent.getValue('ddlOperadorCedente') == "")
        parent.llenarOperadorCedente();

    //PROY-32129 FASE 2 INICIO
    PageMethods.ValidarAnularDatosAlumInst(idFila, parent.getValue('ddlTipoDocumento'), parent.getValue('txtNroDoc'), TelefonosSMS, ValidarAnularDatosAlumInst_callback);
    //PROY-32129 FASE 2 FIN     

    eliminarLineaCuenta(); //PROY-140743
      
    //PROY-140736 INI   
     EliminarBuyBack(idFila)
     //PROY-140736 FIN
}
  
    

 function EliminarBuyBack(idFila) {
    var buyback = parent.document.getElementById('hdbuyback')
    var hdbuyback = parent.getValue('hdbuyback')
    var filtrobuyback = hdbuyback.split('|');
    var campValido = false;
    for (var i = 0; i < filtrobuyback.length; i++) {
        if (filtrobuyback[i].split(';')[0] == idFila) {
            filtrobuyback.splice(i, 1);
            buyback.value = filtrobuyback;
}

}
    buyback.value = buyback.value.replace(",", "|");
    parent.QuitarComasBuyback(idFila);
    }
    //PROY-140736 FIN

//PROY-140245 
function actualizarCantidadProductosCasoEspColab(strCodProd) {
    switch (strCodProd) {
        case codTipoProdMovil:
            {
                var cantMovilAct = parent.getValue('hidcantMovilAct');
                if (cantMovilAct > 0) {
                    cantMovilAct--;
                    parent.setValue('hidcantMovilAct', cantMovilAct);
                }
                break;
            }
        case codTipoProdFijo:

            {
                var cantFijoInalAct = parent.getValue('hidcantFijoInalAct');
                if (cantFijoInalAct > 0) {
                    cantFijoInalAct--;
                    parent.setValue('hidcantFijoInalAct', cantFijoInalAct);
                }
                break;
            }
        case codTipoProdDTH:
            {
                var cantClaroTvAct = parent.getValue('hidcantClaroTvAct');
                if (cantClaroTvAct > 0) {
                    cantClaroTvAct--;
                    parent.setValue('hidcantClaroTvAct', cantClaroTvAct);
                }
                break;
            }
        case codTipoProdBAM:
            {
                var cantBamAct = parent.getValue('hidcantBamAct');
                if (cantBamAct > 0) {
                    cantBamAct--;
                    parent.setValue('hidcantBamAct', cantBamAct);
                }
                break;
            }
        case codTipoProd3Play:
            {
                var cant3PlayAct = parent.getValue('hidcant3PlayAct');
                if (cant3PlayAct > 0) {
                    cant3PlayAct--;
                    parent.setValue('hidcant3PlayAct', cant3PlayAct);
                }
                break;
            }
        case codTipoProdInterInalam:
            {
                var cantInterInalAct = parent.getValue('hidcantInterInalAct');
                if (cantInterInalAct > 0) {
                    cantInterInalAct--;
                    parent.setValue('hidcantInterInalAct', cantInterInalAct);
                }
                break;
            }

        case codTipoProd3PlayInalam:
            {
                var cantPlayInalAct = parent.getValue('hidcantPlayInalAct');
                if (cantPlayInalAct > 0) {
                    cantPlayInalAct--;
                    parent.setValue('hidcantPlayInalAct', cantPlayInalAct);
                }
                break;
            }
    }
}

//PROY-32129 FASE 2 INICIO
function ValidarAnularDatosAlumInst_callback(objResponse) {

    if (objResponse === undefined) {
        alert('Error eliminar el registro en el whitelist');
        return false;
    }
    var sResponse = objResponse.value;
    if (sResponse == '0') {
        alert('SE ELIMINO EL REGISTRO EN EL WHITELIST');
    }
    //PROY-32129 FASE 2: FIN
}

//PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
function validarCPLineasPortabilidad() {
    var result = 0;
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tipoProducto = getValue('hidTipoProductoActual');
    if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM)) {
        var tabla = document.getElementById('tblTabla' + tipoProducto);
        var cont = tabla.rows.length;
        var countPermitidos = 0;
        var countRechazados = 0;
        var idFila;
        var CadenaLineaRechazada = '';
        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            if (fila.style.display != 'none') {
                idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
                if (idFila.length == 0)
                    idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);

                //PROY-140335 RF1 INI
                var NroTelefono = document.getElementById('txtNroTelefono' + idFila).value;
                var hidLineasSinCP = parent.getValue('hidLineasSinCP');
                if (hidLineasSinCP != '') {
                    var index = hidLineasSinCP.indexOf(NroTelefono);
                    if (index > -1) {
                        document.getElementById('txtFlagCPPermitida' + idFila).value = '0';
                    }
                }
                var flagCPPermitida = document.getElementById('txtFlagCPPermitida' + idFila).value;
                //PROY-140335 RF1 FIN
                if (flagCPPermitida == 0) {
                    countPermitidos = countPermitidos + 1;
                }
                else {
                    countRechazados = countRechazados + 1;
                    //PROY-140335 RF1 INI
                    var LineaRechazada = NroTelefono;
                    CadenaLineaRechazada = CadenaLineaRechazada + LineaRechazada + ",";
                    //PROY-140335 RF1 FIN
        }
              
                }
            }
        if (CadenaLineaRechazada != '') {
            CadenaLineaRechazada = CadenaLineaRechazada.substring(0, CadenaLineaRechazada.length - 1);
        }
        parent.setValue('hidLineasRec', CadenaLineaRechazada); //PROY-140335 RF1
        if (countPermitidos == 0) {
            //No hay items a considerar
            return 2;
        }
        else if (countRechazados > 0) {
            //Filtrar los items permitidos
            return 1;
        }
    }
    return result;
}

function validarCPLineasPortabilidadRechazadas() {
    var result = {
        lineas: '',
        PkPortabilidad: ''
    };

    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tipoProducto = getValue('hidTipoProductoActual');
    if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProducto == codTipoProdMovil || codigoTipoProducto == codTipoProdBAM)) {
        var tabla = document.getElementById('tblTabla' + tipoProducto);
        var cont = tabla.rows.length;
        var countPermitidos = 0;
        var countRechazados = 0;
        var idFila;
        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            if (fila.style.display != 'none') {
                idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
                if (idFila.length == 0)
                    idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);

                var flagCPPermitida = document.getElementById('txtFlagCPPermitida' + idFila).value;
                var idPortabilidad = document.getElementById('txtIdPortabilidad' + idFila).value;
                var linea = document.getElementById('txtNroTelefono' + idFila).value;

                if (flagCPPermitida != 0) {
                    result.PkPortabilidad = result.PkPortabilidad + "|" + idPortabilidad;
                    if (result.lineas == '') {
                        result.lineas = linea;
                    }
                    else {
                        result.lineas = result.lineas + ", " + linea;
                    }
                }
            }
        }
    }
    return result;
}

function eliminarLineasCPPortabilidad(strCodigosPorta) {
    PageMethods.EliminarCPPortabilidad(strCodigosPorta, EliminarCPPortabilidad_CallBack);
}

function EliminarCPPortabilidad_CallBack(objResponse) {

}
//PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

function calcularCFxProducto() {
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tipoProducto = getValue('hidTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var total = 0;

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];

        if (fila.style.display != 'none') {

            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

            if (idFila.length == 0)
                idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);

            total += parseFloat(calcularCF(idFila));
        }
    }

    document.getElementById('txtCFTotal').value = total.toFixed(2);

    parent.autoSizeIframe();
}

function calcularCFCarrito() {
    var tablaResumen = document.getElementById('tbResumenCompras');
    var cont = tablaResumen.rows.length;
    var fila;
    var total = 0;

    for (var i = 0; i < cont; i++) {
        fila = tablaResumen.rows[i];

        hidFila = fila.cells[0].getElementsByTagName("input")[0];

        if (hidFila != null)
            total += parseFloat(fila.cells[6].innerHTML);
    }
    //PROY - 30748 - INICIO
    var idFila = cont - 1;
    if (document.getElementById('hidMontoServicios' + idFila) != null) {
        parent.document.getElementById('hidCFServAdicionales').value = document.getElementById('hidMontoServicios' + idFila).value; //PROY - 30748  - HID SERVICIOS ADICIONALES CF 
    }
    else {
        parent.document.getElementById('hidCFServAdicionales').value = '0';
    }
    //PROY - 30748 - FIN
    //PROY-140743 - INI
    if (parent.getValue('ddlTipoOperacion') == '25') {
        document.getElementById('txtCFTotalCarrito').value = document.getElementById('txtCFTotal').value;
    } else {
    document.getElementById('txtCFTotalCarrito').value = total.toFixed(2);
}
    //PROY-140743 - FIN
}

function autoSizeIframe() {
    parent.autoSizeIframe();
}

function obtenerProductoxIdFila(idFila) {
    var idProducto;
    var tablaProducto;
    var arrDetalle = getValue('hidCadenaDetalle').split('|');
    for (var x = 0; x < arrDetalle.length; x++) {
        if (arrDetalle[x] != '') {
            if (arrDetalle[x].split(';')[0] == idFila)
                idProducto = arrDetalle[x].split(';')[1];
        }
    }
    switch (idProducto) {
        case codTipoProdMovil: tablaProducto = 'Movil'; break;
        case codTipoProdFijo: tablaProducto = 'Fijo'; break;
        case codTipoProdBAM: tablaProducto = 'BAM'; break;
        case codTipoProdDTH: tablaProducto = 'DTH'; break;
        case codTipoProd3Play: tablaProducto = 'HFC'; break;
        case codTipoProdFTTH: tablaProducto = 'FTTH'; break; //FTTH -  idProducto

    }

    if (tablaProducto == null && tablaProducto == undefined)
        tablaProducto = getValue('hidTipoProductoActual');

    return tablaProducto;
}

function inicio() {
    parent.document.getElementById('hidCadenaDetalle').value = '';
    //gaa20151127
    llenarDatosCombo(parent.document.getElementById('ddlOperadorCedente'), '', true);
    //fin gaa20151127
    // Validación Modalidad / Operador Cedente
    if (parent.getValue('hidNTienePortabilidadValues') == 'S') {
        parent.document.getElementById('ddlModalidadPorta').disabled = false;
        parent.document.getElementById('ddlOperadorCedente').disabled = false;
        parent.document.getElementById('ddlModalidadPorta').selectedIndex = 0;
        //gaa20151127
        //llenarDatosCombo(parent.document.getElementById('ddlOperadorCedente'), '', true);
        // Validación Modalidad / Operador Cedente
        if (parent.getValue('ddlOperadorCedente') == "")
            parent.llenarOperadorCedente();
        //fin gaa20151127
    }

    // Carga Combo
    if (parent.getValue('ddlCombo') != '') {
        consultarPlanesCombo();
    }
    else {
        //SEC Pendiente
        var strValor = parent.getValue('hidCadenaSECPendiente');
        if (strValor != '') {
            //gaa20160211
            setValue('hidEsSecPendiente', 'S');
            //fin gaa20160211
            parent.document.getElementById('hidCadenaSECPendiente').value = '';
            parent.mostrarTabxOferta();
            agregarSECPendiente(strValor);
        }
        else
            parent.mostrarTabxOferta();
    }

    if (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion || parent.getValue('hidNTienePortabilidadValues') == 'S') {
        mostrarColumnaTelefono(true);
    }
    if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota || parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuotaSinCode) {//INICIATIVA 920
        setValue('hidTieneCuotas', 'S');
        mostrarColumnaCuota(true);
    } else if (parent.getValue('ddlModalidadVenta') == codModalidadChipSuelto) {
        setValue('hidTieneCuotas', 'N');
        mostrarColumnaCuota(false);
        mostrarColumnaEquipo(false);
    } else {
        setValue('hidTieneCuotas', 'N');
        mostrarColumnaCuota(false);
    }

    codServProteccionMovil = parent.document.getElementById('hidCodServProteccionMovil').value; //PROY-24724-IDEA-28174 - INICIO
    concatCodTipoPdvProteccionMovil = parent.document.getElementById('hidConcatCodTipoPdvProteccionMovil').value;
    concatCodTipoOfertaProteccionMovil = parent.document.getElementById('hidConcatCodTipoOfertaProteccionMovil').value;
    concatCodTipoModalidadVentaProteccionMovil = parent.document.getElementById('hidConcatCodTipoModalidadVentaProteccionMovil').value; //PROY-24724-IDEA-28174 - FIN

    //PROY-140743 - INICIO
    var codOperacion = parent.getValue('ddlTipoOperacion');
    if (codOperacion == '25') {
        var MontoTope = document.getElementById('tdMontoTope');
        MontoTope.style.display = "none";
    }
    //PROY-140743 - FIN
}

function consultarPlanesCombo() {
    var btnAgregarFila = document.getElementById('btnAgregarPlan');

    cargarImagenEsperando();
    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'strCombo=' + parent.getValue('ddlCombo');
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strMetodo=' + 'LlenarPlanesCombo';
    self.frames['iframeAuxiliar'].location.replace(url);
}

function retornarPlanesCombo(strResultado) {
    if (strResultado.indexOf('~') > -1) {
        asignarPlanesCombo(strResultado);
    }
}

function autoSizeProducto() {
    var ddlOferta = parent.document.getElementById('ddlOferta');
    var hidTienePaquete = document.getElementById('hidTienePaquete');

    //PROY-140743 - INI
    var codOperacion = parent.getValue('ddlTipoOperacion');

    if (codOperacion == '25') {

        mostrarColumna('3', true);
        mostrarColumna('4', true);
        mostrarColumna('15', true);
        mostrarColumna('5', false);
        mostrarColumna('9', false);
        
    } else {
        mostrarColumna('15', false);
        mostrarColumna('4', false);
    };
    //PROY-140743 - FIN    
    

    hidTienePaquete.value = 'N';
    mostrarColumna(columnaPaquete, false);
    //gaa20161020
    document.getElementById('tdFamiliaPlanMovil').style.display = 'none';
    //fin gaa20161020
    if (ddlOferta.value == TipoProductoBusiness) {
        hidTienePaquete.value = 'S';
        mostrarColumna(columnaPaquete, true);
    }
    else {
        //gaa20161020
        if (familiaFlag == '1')
            document.getElementById('tdFamiliaPlanMovil').style.display = 'inline';
        //fin gaa20161020
    }

    if (parent.getValue('ddlModalidadVenta') == codModalidadChipSuelto) {
        var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
        if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProductoActual == codTipoProdMovil || codigoTipoProductoActual == codTipoProdBAM)) {
            document.getElementById('divGrillaCabecera').style.width = 1200;
            document.getElementById('divGrillaDetalle').style.width = 1200;
        }
        else {
            document.getElementById('divGrillaCabecera').style.width = 850;
            document.getElementById('divGrillaDetalle').style.width = 850;
        }
    }
    else {
        var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
        if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (codigoTipoProductoActual == codTipoProdMovil || codigoTipoProductoActual == codTipoProdBAM)) {
            document.getElementById('divGrillaCabecera').style.width = screen.width - 5;
            document.getElementById('divGrillaDetalle').style.width = screen.width - 5;
        }
        else {
            document.getElementById('divGrillaCabecera').style.width = screen.width - 40;
            document.getElementById('divGrillaDetalle').style.width = screen.width - 40;
        }
    }

    if (parent.getValue('hidNTienePortabilidadValues') == 'S') {
        var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
        var productoActual = getValue('hidTipoProductoActual');
        if (codigoTipoProductoActual == codTipoProdMovil || codigoTipoProductoActual == codTipoProdBAM) {
           
           //INI: CAMBIADO POR EL PROY-140335 RF1
            //PROY-140223 IDEA-140462
//            if (parent.validarEnvioConsultaPrevia()) {
//                //Sino continuar con flujo actual
//                document.getElementById('tdEstadoCP' + productoActual).style.display = 'none';
//                document.getElementById('tdFechaCP' + productoActual).style.display = 'none';
//                document.getElementById('tdDeudaCP' + productoActual).style.display = 'none';
//            }
//            else {
//                //Sino continuar con flujo actual
//                document.getElementById('tdEstadoCP' + productoActual).style.display = 'inline';
//                document.getElementById('tdFechaCP' + productoActual).style.display = 'inline';
//                document.getElementById('tdDeudaCP' + productoActual).style.display = 'inline';
            //            }

            if (parent.document.getElementById('tdConsultaPrevia').style.display != 'none') {
                document.getElementById('tdEstadoCP' + productoActual).style.display = 'inline';
                document.getElementById('tdFechaCP' + productoActual).style.display = 'inline';
                document.getElementById('tdDeudaCP' + productoActual).style.display = 'inline';
            }
            //FIN: CAMBIADO POR EL PROY-140335 RF1
        }
    }
}

//gaa20130504
function mostrarPromociones(tipoServicio, strCodSrv) {
    var lbxPD;
    if (tipoServicio == 'D')
        lbxP = document.getElementById('lbxPromocionesDisponibles');
    else
        lbxP = document.getElementById('lbxPromocionesSeleccionadas');

    var strPromociones = getValue('hidPromociones');
    //gaa20131111
    var idFila = document.getElementById('hidLineaActual').value;
    var strAgrupaPaquete = obtenerPaqueteActual(idFila);
    var idFilaPaquete = obtenerFilaPaquete(strAgrupaPaquete); //Obtiene la primera fila del paquete
    var a = '{' + idFilaPaquete + '}';
    var z = '{/' + idFilaPaquete + '}';

    a = strPromociones.indexOf(a);

    if (a > -1) {
        z = strPromociones.indexOf(z);

        strPromociones = strPromociones.substring(a, z);
    }
    //fin gaa20131111				
    var arrPromociones = strPromociones.split('|');
    var arrPromocion;
    var strPromocion;
    var strCodProm = '';
    var arrCodProm;
    var strIDDET;
    var strIDPRODUCTO;
    var strIDLINEA;
    var intPosIni;
    var k = -1;

    strCodSrv = strCodSrv.split('_')[3];
    intPosIni = strPromociones.indexOf(strCodSrv);

    lbxP.innerHTML = "";

    if (intPosIni > -1) {
        for (var j = 1; j < arrPromociones.length; j++) {
            strPromocion = arrPromociones[j];
            arrPromocion = strPromocion.split('_');
            strCodProm = arrPromocion[0];
            arrCodProm = strCodProm.split('.');
            strIDDET = arrCodProm[0];
            strIDPRODUCTO = arrCodProm[1];
            strIDLINEA = arrCodProm[2];

            strFLGEDI = arrPromocion[3];

            if (strCodSrv == strIDDET + '.' + strIDPRODUCTO + '.' + strIDLINEA) {
                k++;

                var option = document.createElement('option');
                option.value = strCodProm;
                option.text = strPromocion.split(';')[1];
                lbxP.options[k] = option;
            }
        }
    }

    //Se valida que es Servicio Roaming Internacional
    if (tipoServicio == 'A') {
        if (strCodSrv == codServRoamingI)
            document.getElementById('tblRoamingI').style.display = 'inline';
        else
            document.getElementById('tblRoamingI').style.display = 'none';

    }
}

function f_cambiarPlazoRI(codPlazo) {
    var objectValue = codPlazo;
    if (objectValue == '02') {
        document.getElementById('tdTxtFechaDesde').style.display = '';
        document.getElementById('tdLblFechaDesde').style.display = '';
        document.getElementById('tdTxtFechaHasta').style.display = 'none';
        document.getElementById('tdLblFechaHasta').style.display = 'none';
    }
    else {
        document.getElementById('tdTxtFechaDesde').style.display = '';
        document.getElementById('tdLblFechaDesde').style.display = '';
        document.getElementById('tdTxtFechaHasta').style.display = '';
        document.getElementById('tdLblFechaHasta').style.display = '';
    }
}

//gaa20130627
function cambiarServicio(valor, idFila) {
    txtCFPlanServicio = document.getElementById('txtCFPlanServicio' + idFila);
    if (valor.length > 0)
        txtCFPlanServicio.value = valor.split('_')[1];
    else
        txtCFPlanServicio.value = '0';

    calcularCFxProducto();
}

function mostrarEquipo(idFila) {
    //INICIATIVA - 733 - INI - C14
    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
        ValmostrarEquipo = idFila;
    }
    //INICIATIVA - 733 - FIN - C14
    var ddlServicio = document.getElementById('ddlServicio' + idFila);
    var habilitar = ddlServicio.disabled;
    habilitarEquipo(habilitar);

    var strPlan = document.getElementById('ddlPlan' + idFila).value;
    if (strPlan.length == 0) {
        cerrarEquipo();
        return;
    }
    document.getElementById('hidLineaActual').value = idFila;

    limpiarEquipos();
    document.getElementById('txtCFTotalEquipo').value = '';

    var strEquipos = document.getElementById('hidEquiposXfila3Play').value;

    //Si tiene equipos configurados en la fila los trae
    if (strEquipos.indexOf('|*ID' + idFila + '*') > -1)
        traerEquipo(idFila);

    //Traer equipos para el combo
    var strEquiposXplan = document.getElementById('hidEquiposXplan').value;
    if (strEquiposXplan.indexOf('[' + strPlan + ']') > -1)
        llenarEquipo3Play(idFila);
    else
        LlenarEquipo3PlayIfr(idFila);

    cerrarCuota();
    cerrarLineasCuentas(); //PROY-140743
    cerrarServicio();

    document.getElementById('txtPrecAlqEquipo3Play').value = '';
    document.getElementById('tblEquipos').style.display = 'inline';
    parent.autoSizeIframe();
}

function cerrarEquipo() {
    document.getElementById('tblEquipos').style.display = 'none';
}


function LlenarEquipo3PlayIfr(idFila) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
    url += 'idFila=' + idFila;
    url += '&strPlan=' + document.getElementById('ddlPlan' + idFila).value;
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strMetodo=' + "LlenarEquipo3Play";
    url += '&strTipoProducto=' + codigoTipoProductoActual;

    self.frames['iframeAuxiliar'].location.replace(url);
}

function llenarEquipo3Play(idFila) {
    var strEquiposXplan = document.getElementById('hidEquiposXplan').value;
    var strPlan = document.getElementById('ddlPlan' + idFila).value;
    var ddlEquipo = document.getElementById('ddlEquipo3Play');
    var strValIni = '[' + strPlan + ']';
    var ini = strEquiposXplan.indexOf(strValIni) + strValIni.length;
    var fin = strEquiposXplan.indexOf('[/' + strPlan + ']');
    var strEquipos = strEquiposXplan.substring(ini, fin);

    llenarDatosCombo(ddlEquipo, strEquipos, true);
}

function asignarEquipo3Play(idFila, strResultado) {
    var hidEquiposXplan = document.getElementById('hidEquiposXplan');
    var ddlEquipo = document.getElementById('ddlEquipo3Play');
    var strPlan = document.getElementById('ddlPlan' + idFila).value;
    var ini = strResultado.indexOf(']') + 1;
    var fin = strResultado.lastIndexOf('[');

    hidEquiposXplan.value += strResultado;

    strResultado = strResultado.substring(ini, fin);

    llenarDatosCombo(ddlEquipo, strResultado, true);
}

function cambiarEquipo3Play(codigo) {
    var txtPrecAlqEquipo = document.getElementById('txtPrecAlqEquipo3Play');
    txtPrecAlqEquipo.value = getValor(codigo, 1);
}

function agregarEquipo3Play() {
    //gaa20130910

    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');

    if (codigoTipoProductoActual != codTipoProd3PlayInalam) {

        if (!validarMaximoNroEquipos3Play()) {
            alert('El máximo n° de equipos para la venta es ' + parent.document.getElementById('hidNroEquipos3PlayMax').value);
            return;
        }
    }



    //fin gaa20130910
    var tabla = document.getElementById('tblResumenAlqEquipo3Play');
    var cont = tabla.rows.length;
    var ddlEquipo = document.getElementById('ddlEquipo3Play');
    var codigoTot = ddlEquipo.value;

    if (codigoTot.length == 0) {
        alert('Debe seleccionar un equipo válido');
        return;
    }

    var codigo = getValor(codigoTot, 0);

    //gaa20131105
    var idFila = document.getElementById('hidLineaActual').value;

    if (!validarDuplicidadEquipo3Play(idFila, codigo)) {
        alert('Ya esta agregado el equipo');
        return;
    }
    //fin gaa20131105				

    var descrip = obtenerTextoSeleccionado(ddlEquipo);
    var CF = getValor(codigoTot, 1);
    var tipoMaterial = getValor(codigoTot, 2);
    var fila;
    //var idFila;				
    var newRow;
    var oCell;

    newRow = tabla.insertRow();

    oCell = newRow.insertCell();
    oCell.align = 'center';
    oCell.innerHTML = "<input type='radio' name='rbtEquipo3Play'><input type='hidden' value=" + codigo + "><input type='hidden' value=" + tipoMaterial + ">";
    oCell.className = 'TablaFilasGrid';

    oCell = newRow.insertCell();
    oCell.align = 'center';
    oCell.innerHTML = descrip;
    oCell.className = 'TablaFilasGrid';

    oCell = newRow.insertCell();
    oCell.align = 'center';
    oCell.innerHTML = CF;
    oCell.className = 'TablaFilasGrid';

    calcularCFEquipo3PlayTotal()

    parent.autoSizeIframe();

    //    INICIATIVA - 733 - INI - C1
    //Agregar Adicionales desde Alquiler de Equipos
    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
        AgregarIPTVdefault();
    }
    //    INICIATIVA - 733 - FIN - C1

}

//    INICIATIVA - 733 - INI - C2
//Variables Globales
var ContieneDecoIPTV = false;
var AgregarEliminarDecoIPTV = false;
var ValmostrarEquipo;
var VallineaAdicional;
var IDFilaDinamicaServAdicional;
var CodGrupoServicioHFC = false;
//var strMensajeIPTV = "No puede eliminar Claro Video por que existe DECO IPTV Asignado en alquiler de equipos";

function AgregarIPTVdefault() {

    //    Codigo de equipo IPTV
    var CodigoEquipoIPTV = Key_CodEquipoIPTV.split('|');
//    var cods = "4236|4235|4759"
//    var CodigoEquipoIPTV = cods.split('|');

    var tabla = document.getElementById('tblResumenAlqEquipo3Play');
    var cont = tabla.rows.length;

    for (var i = 1; i < cont; i++) {
        fila = tabla.rows[i];
        var Codigo = fila.cells[0].getElementsByTagName("input")[1].value;

        for (var j = 0; j < CodigoEquipoIPTV.length; j++) {

            if (CodigoEquipoIPTV[j] == Codigo) {
                ContieneDecoIPTV = true;
            }

        }
    }

    if (ContieneDecoIPTV == true) {

        mostrarServicio(IDFilaDinamicaServAdicional);
        ValidarServicioExcluyentes();
        AgregarEliminarDecoIPTV = true;

    }

}
//    INICIATIVA - 733 - FIN - C2

function quitarEquipo3Play() {
    var tabla = document.getElementById('tblResumenAlqEquipo3Play');
    var cont = tabla.rows.length;
    var fila;
    var filaSel;

    //  INICIATIVA - 733 - INI - C10
    //Validar Cantidad de decos IPTV
    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
        var ContEquipIPTV = 0;
        var CodigoEquipoIPTV = Key_CodEquipoIPTV.split('|');
        //    var CodigoEquipoIPTV = ["4236", "4235", "4759"];
        var SeleccionIPTV = false;
        var fila1;
        var equiposeleccionado;

        for (var i = 1; i < cont; i++) {
            fila1 = tabla.rows[i];

            var innerhtml = tabla.rows[i].innerHTML
            var innerhtml1 = innerhtml.split('value=')[1];
            var CodigoEquip = innerhtml1.substring(0, 4)
            SeleccionIPTV = fila1.cells[0].getElementsByTagName("input")[0].checked

            if (SeleccionIPTV == true) {
                equiposeleccionado = CodigoEquip
            }

            for (var j = 0; j < CodigoEquipoIPTV.length; j++) {

                if (CodigoEquipoIPTV[j] == CodigoEquip) {
                    ContEquipIPTV++;
                    ContieneDecoIPTV = true
                }

            }

        }
    }
    //  INICIATIVA - 733 - FIN - C10

    for (var i = 1; i < cont; i++) {
        fila = tabla.rows[i];

        filaSel = fila.cells[0].getElementsByTagName("input")[0].checked;

        if (filaSel) {
            tabla.deleteRow(i);
            calcularCFEquipo3PlayTotal();

            //  INICIATIVA - 733 - INI - C11
            //Elimina servicio adicional Claro video
            if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
                for (var j = 0; j < CodigoEquipoIPTV.length; j++) {

                    if (CodigoEquipoIPTV[j] == equiposeleccionado) {

                        if (ContEquipIPTV == 1 && ContieneDecoIPTV == true) {
                            mostrarServicio(IDFilaDinamicaServAdicional);
                            quitarServicio();
                            AgregarEliminarDecoIPTV = true;
                        }

                    }

                }
            }
            //  INICIATIVA - 733 - FIN - C11

            parent.autoSizeIframe();
            return;
        }
    }

    alert('Debe seleccionar un equipo');
}

function calcularCFEquipo3PlayTotal() {
    var tabla = document.getElementById('tblResumenAlqEquipo3Play');
    var txtCFTotalEquipo = document.getElementById('txtCFTotalEquipo');
    var cont = tabla.rows.length;
    var total = 0;

    for (var i = 1; i < cont; i++) {
        fila = tabla.rows[i];
        total += parseFloat(fila.cells[2].innerHTML);
    }

    txtCFTotalEquipo.value = total.toFixed(2);
}

function guardarEquipo3Play() {
    cerrarEquipo();
    parent.autoSizeIframe();

    if (document.getElementById('btnAgregarEquipo3Play').disabled)
        return;

    //  INICIATIVA - 733 - INI - C8
    //  Agregar Id fila global 
    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
        document.getElementById('hidLineaActual').value = ValmostrarEquipo;
    }
    //  INICIATIVA - 733 - FIN - C8
    var idFila = document.getElementById('hidLineaActual').value;
    var hidEquiposXfila3Play = document.getElementById('hidEquiposXfila3Play');

    borrarEquipo(idFila);

    hidEquiposXfila3Play.value += obtenerCadenaEquipo3Play(idFila);

    setValue('hidMontoEquipo' + idFila, getValue('txtCFTotalEquipo'));

    calcularCFxProducto();

    //  INICIATIVA - 733 - INI - C9
    //  Guadar o elimina los adicionales cuando el DECO es IPTV
    if (getValue('hidCodigoTipoProductoActual') == codTipoProd3Play && CodGrupoServicioHFC == true) {
        if (ContieneDecoIPTV == true && AgregarEliminarDecoIPTV == true) {
            guardarServicio('c');
            AgregarEliminarDecoIPTV = false;
        }

        ContieneDecoIPTV = false;
    }
    //  INICIATIVA - 733 - FIN - C9

}

function obtenerCadenaEquipo3Play(idFila) {
    var tabla = document.getElementById('tblResumenAlqEquipo3Play');
    var cont = tabla.rows.length;
    var resultado = '|*ID' + idFila + '*';

    for (var i = 1; i < cont; i++) {
        fila = tabla.rows[i];
        resultado += '|' + fila.cells[0].getElementsByTagName("input")[1].value;
        resultado += ';' + fila.cells[0].getElementsByTagName("input")[2].value;
        resultado += ';' + fila.cells[1].innerHTML;
        resultado += ';' + fila.cells[2].innerHTML;
    }

    resultado += '*/ID' + idFila + '*';

    return resultado;
}

function borrarEquipo(idFila) {
    var hidEquiposXfila3Play = document.getElementById('hidEquiposXfila3Play');
    var strValIni = '|*ID' + idFila + '*';
    var strValFin = '*/ID' + idFila + '*';
    var strEquipos = hidEquiposXfila3Play.value;
    var strEquipo;
    var intPosIni = strEquipos.indexOf(strValIni);
    var intPosFin;

    if (intPosIni > -1) {
        intPosFin = strEquipos.indexOf(strValFin);

        strEquipo = strEquipos.substring(intPosIni, intPosFin + strValFin.length);

        hidEquiposXfila3Play.value = strEquipos.replace(strEquipo, '');

        setValue('hidMontoEquipo' + idFila, '0');
    }
}

function traerEquipo(idFila) {
    var hidEquiposXfila3Play = document.getElementById('hidEquiposXfila3Play');
    var tabla = document.getElementById('tblResumenAlqEquipo3Play');
    var cont;
    var newRow;
    var oCell;
    var strValIni = '|*ID' + idFila + '*';
    var strValFin = '*/ID' + idFila + '*';
    var strEquipos = hidEquiposXfila3Play.value;
    var arrEquipos;
    var arrEquipo;
    var intPosIni = strEquipos.indexOf(strValIni);

    if (intPosIni > -1) {
        intPosFin = strEquipos.indexOf(strValFin);

        strEquipo = strEquipos.substring(intPosIni + strValIni.length, intPosFin);

        arrEquipos = strEquipo.split('|');

        cont = arrEquipos.length;

        for (var i = 1; i < cont; i++) {
            arrEquipo = arrEquipos[i].split(';');

            newRow = tabla.insertRow();

            oCell = newRow.insertCell();
            oCell.align = 'center';
            oCell.innerHTML = "<input type='radio' name='rbtEquipo3Play'><input type='hidden' value=" + arrEquipo[0] + "><input type='hidden' value=" + arrEquipo[1] + ">";
            oCell.className = 'TablaFilasGrid';

            oCell = newRow.insertCell();
            oCell.align = 'center';
            oCell.innerHTML = arrEquipo[2];
            oCell.className = 'TablaFilasGrid';

            oCell = newRow.insertCell();
            oCell.align = 'center';
            oCell.innerHTML = arrEquipo[3];
            oCell.className = 'TablaFilasGrid';
        }

        calcularCFEquipo3PlayTotal()

        parent.autoSizeIframe();
    }
}

function limpiarEquipos() {
    var tabla = document.getElementById('tblResumenAlqEquipo3Play');
    var cont = tabla.rows.length;

    for (var i = 1; i < cont; i++) {
        tabla.deleteRow(1);
        cont--;
        i--;
    }
}

function habilitarEquipo(hab) {
    document.getElementById('btnAgregarEquipo3Play').disabled = hab;
    document.getElementById('btnQuitarEquipo3Play').disabled = hab;

    if (hab)
        document.getElementById('btnCerrarEquipo3Play').value = 'Cerrar';
    else
        document.getElementById('btnCerrarEquipo3Play').value = 'Cerrar y Guardar';
}

function obtenerCadenaEquipoDetOfert(idFila) {
    var hidEquiposXfila3Play = document.getElementById('hidEquiposXfila3Play');
    var strValIni = '|*ID' + idFila + '*';
    var strValFin = '*/ID' + idFila + '*';
    var strEquipos = hidEquiposXfila3Play.value;
    var strEquipo;
    var intPosIni = strEquipos.indexOf(strValIni);
    var intPosFin;
    var strResultado = '';

    if (intPosIni > -1) {
        intPosIni += strValIni.length;
        intPosFin = strEquipos.indexOf(strValFin);

        strResultado = strEquipos.substring(intPosIni, intPosFin);
    }

    return strResultado;
}

function mostrarEquipoCarrito(idFila) {
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var strPlan = ddlPlan.value;

    if (strPlan != '') {
        var nroSec = getValue('hidNroSec' + idFila);
        var str = obtenerCadenaEquipoDetOfert(idFila);
        if (str != '') {
            var url = ''
            var w = 280;
            var h = 200;
            var leftScreen = (screen.width - w) / 2;
            var topScreen = (screen.height - h) / 2;
            var opciones = "directories=no,menubar=no,scrollbars=yes,status=yes,resizable=yes,width=" + w + ",height=" + h + ",left=" + leftScreen + ",top=" + topScreen;
            url = '../consultas/sisact_pop_equipo_3play.aspx?nroSec=' + nroSec + '&strEquipoHFC=' + str;

            window.open(url, '_blank', opciones);
        }
    }
}

//fin gaa20130627
//gaa20130910
function validarMaximoNroEquipos3Play() {
    var strListaComodato = parent.document.getElementById('hidListaComodato').value;
    var intNroEquipos3PlayMax = parseInt(parent.document.getElementById('hidNroEquipos3PlayMax').value);
    var strPlanSel = '';
    var strSrvSel = '';
    var tabla;
    var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');

    if (codigoTipoProductoActual == codTipoProd3Play) {
        tabla = document.getElementById('tblTablaHFC');
    };

    // FTTH Inicio- validarMaximoNroEquipos3Play()
    if (codigoTipoProductoActual == codTipoProdFTTH) {
        tabla = document.getElementById('tblTablaFTTH');
    };
    // FTTH Fin- validarMaximoNroEquipos3Play()

    if (codigoTipoProductoActual == codTipoProd3PlayInalam) {
        tabla = document.getElementById('tblTabla3PlayInalam');
    };

    var cont = tabla.rows.length;
    var arrComodatos = strListaComodato.split('|');
    var arrComodato;
    var arrServicios;
    var i;
    var intContComoEnco = 0;
    var strCodEquipo;
    var strEquipos3PlayGuardados = document.getElementById('hidEquiposXfila3Play').value;
    var idFilaActual = document.getElementById('hidLineaActual').value;
    var strEquipos = '';
    var arrEquipos;
    //Contar planes y servicios comodato
    for (i = 0; i < cont; i++) {
        nCell = 3; //0: Imagen Confirmar, 1:Imagen Eliminar, 2:Campana, 3:Plazo
        fila = tabla.rows[i];

        //idFila
        idFilaX = fila.cells[nCell].getElementsByTagName("select")[0].id.substring(10);

        if (idFilaX.length == 0)
            idFilaX = fila.cells[nCell].getElementsByTagName("select")[0].id.substring(8);

        nCell += 3;
        planSel = fila.cells[nCell].getElementsByTagName("select")[0].value.split('_')[0];

        strSrvSel += extraerCodigoServicio(idFilaX) + ';' + planSel;
    }

    arrServicios = strSrvSel.split(';');

    for (i = 1; i < arrServicios.length; i++) {
        if (strListaComodato.indexOf(arrServicios[i] + ';') > 1) {
            strCodEquipo = arrServicios[i];

            for (var x = 1; x < arrComodatos.length; x++) {
                arrComodato = arrComodatos[x].split(';');

                if (strCodEquipo == arrComodato[0])
                    intContComoEnco += parseInt(arrComodato[1]);
            }
        }
    }
    //a partir de la fila actual, sacar la fila que tiene los equipos
    strEquipos = traerCadenaEquiposSinActual(idFilaActual);

    strEquipos += obtenerCadenaEquipo3Play(idFilaActual);

    arrEquipos = strEquipos.split('|');

    for (i = 1; i < arrEquipos.length; i++) {
        if (arrEquipos[i].indexOf(';') > 0)
            intContComoEnco++;
    }

    if (intNroEquipos3PlayMax <= intContComoEnco)
        return false;
    else
        return true;
}

function traerCadenaEquiposSinActual(idFila) {
    var strEquipos = document.getElementById('hidEquiposXfila3Play').value;
    var strValIni = '|*ID' + idFila + '*';
    var strValFin = '*/ID' + idFila + '*';
    var strEquipo;
    var intPosIni = strEquipos.indexOf(strValIni);
    var intPosFin;

    if (intPosIni > -1) {
        intPosFin = strEquipos.indexOf(strValFin);

        strEquipo = strEquipos.substring(intPosIni, intPosFin + strValFin.length);

        strEquipos = strEquipos.replace(strEquipo, '');
    }

    return strEquipos;
}

function validarDuplicidadEquipo3Play(idFila, codEquipo) {
    if (obtenerCadenaEquipo3Play(idFila).indexOf('|' + codEquipo + ';') == -1)
        return true;
    else
        return false;
}

function llenarTopesConsumoIfr(idFila) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strTipoProducto=' + document.getElementById('hidCodigoTipoProductoActual').value; //PROY-29296		
    url += '&strMetodo=' + 'LlenarTopesConsumo';
    self.frames['iframeAuxiliar'].location.replace(url);
}

function asignarTopeConsumo(idFila) {
    var ddlTopeConsumo = document.getElementById('ddlTopeConsumo' + idFila);
    llenarDatosCombo(ddlTopeConsumo, getValue('hidTopesConsumo'), false);

    if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProd3Play || getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam) {

        ddlTopeConsumo.value = constCodTopeCeroServicio; //PROY-29296
        setValue('hidFilaSeleccionada', idFila); //PROY-29296

        crearControlIdMonto("Exacto"); //PROY-29296

        var lbxIdTope = document.getElementById('lbxIdTope'); //PROY-29296
        llenarDatosCombo(lbxIdTope, getValue('hidTopesConsumo'), false); //PROY-29296

        vbCargaIni = false; //PROY-29296
        ddlTopeConsumo.selectedIndex = 2; //PROY-29296
        mostrarTopesLTE(idFila); //PROY-29296
    } else {
        ddlTopeConsumo.value = '0';
    }

}

function asignarPlanesCombo(strResultado) {

    PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][asignarPlanesCombo()] ", "Entro en la funcion asignarPlanesCombo()");

    quitarFilas();

    var arrResultado = strResultado.split('~');
    var strCampanasTotal = arrResultado[0];
    var strPlazosTotal = arrResultado[1];
    var strPlanesTotal = arrResultado[2];
    setValue('hidPlanesCombo', arrResultado[3]);
    var idFila = getValue('hidTotalLineas');

    var arrCampanas = strCampanasTotal.split('|');
    var arrPlazos = strPlazosTotal.split('|');
    var arrPlanes = strPlanesTotal.split('|');

    var strCampanasMovil = '';
    var strCampanasFijo = '';
    var strCampanasBAM = '';
    var strCampanasDTH = '';
    var strCampanasHFC = '';
    var strCampanasFTTH = '';  //FTTH  -strCampanasFTTH
    var strCampanasVentaVarios = '';
    var strCampanasHFCInalamb = '';
    var strCampanasInterInalam = ''; //PROY-31812 - IDEA-43340

    var strPlazosMovil = '';
    var strPlazosFijo = '';
    var strPlazosBAM = '';
    var strPlazosDTH = '';
    var strPlazosHFC = '';
    var strPlazosFTTH = '';  //FTTH  -strPlazosFTTH
    var strPlazosVentaVarios = '';
    var strPlazosHFCInalamb = '';
    var strPlazosInterInalam = ''; //PROY-31812 - IDEA-43340

    var strPlanesMovil = '';
    var strPlanesFijo = '';
    var strPlanesBAM = '';
    var strPlanesDTH = '';
    var strPlanesHFC = '';
    var strPlanesVentaVarios = '';
    var strPlanesHFCI = '';
    var strPlanesInterInalam = ''; //PROY-31812 - IDEA-43340

    var arrCampana;

    var i = 0;

    parent.mostrarAllTabProducto(false);

    for (var i = 1; i < arrCampanas.length; i++) {
        arrCampana = arrCampanas[i].split(';')[0].split('_');

        switch (arrCampana[1]) {
            case codTipoProdMovil:
                strCampanasMovil += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
                break;
            case codTipoProdFijo:
                strCampanasFijo += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
                break;
            //PROY-31812 - IDEA-43340 - INICIO   
            case codTipoProdInterInalam:
                strCampanasInterInalam += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
                break;
            //PROY-31812 - IDEA-43340 - FIN  
            case codTipoProdBAM:
                strCampanasBAM += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
                break;
            case codTipoProdDTH:
                strCampanasDTH += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
                break;
            case codTipoProd3Play:
                strCampanasHFC += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
                break;
            case codTipoProdFTTH:
                strCampanasFTTH += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
                break;
            case codTipoProd3PlayInalam:
                strCampanasHFCInalamb += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
                break;
            case codTipoProdVentaVarios:
                strCampanasVentaVarios += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];

                setValue('hidCodigoTipoProductoActual', codTipoProdVentaVarios);
                setValue('hidTipoProductoActual', 'VentaVarios');

                agregarFila(false, 0, false);
                idFila++;

                ddlCampana = document.getElementById('ddlCampana' + idFila);

                llenarDatosCombo(ddlCampana, strCampanasVentaVarios, false);
                setValue('hidCampanasVentaVarios', strCampanasVentaVarios);

                ddlCampana.disabled = true;

                parent.mostrarTabProducto(getValue('hidCodigoTipoProductoActual'), true);

                visualizarIconosFinales(idFila);
                //PROY-140743-INI
                var strPromocion = '';
                var codOperacion = parent.getValue('ddlTipoOperacion');
                if (codOperacion == '25') {
                    strPromocion = document.getElementById('ddlPromocion' + idFila).value;
                }

                LlenarMaterialIfr(idFila, arrCampana[0], '', parent.getValue('ddlCombo'), codTipoProdVentaVarios, strPromocion);
                //PROY-140743-FIN

                break;
        }
    }

    for (var i = 1; i < arrPlazos.length; i++) {
        arrPlazo = arrPlazos[i].split(';')[0].split('_');

        switch (arrPlazo[1]) {
            case codTipoProdMovil:
                strPlazosMovil += '|' + arrPlazo[0] + ';' + arrPlazos[i].split(';')[1];
                break;
            case codTipoProdFijo:
                strPlazosFijo += '|' + arrPlazo[0] + ';' + arrPlazos[i].split(';')[1];
                break;
            //PROY-31812 - IDEA-43340 - INICIO   
            case codTipoProdInterInalam:
                strPlazosInterInalam += '|' + arrPlazo[0] + ';' + arrPlazos[i].split(';')[1];
                break;
            //PROY-31812 - IDEA-43340 - FIN   
            case codTipoProdBAM:
                strPlazosBAM += '|' + arrPlazo[0] + ';' + arrPlazos[i].split(';')[1];
                break;
            case codTipoProdDTH:
                strPlazosDTH += '|' + arrPlazo[0] + ';' + arrPlazos[i].split(';')[1];
                break;
            case codTipoProd3Play:
                strPlazosHFC += '|' + arrPlazo[0] + ';' + arrPlazos[i].split(';')[1];
                break;
            case codTipoProd3PlayInalam:
                strPlazosHFCInalamb += '|' + arrPlazo[0] + ';' + arrPlazos[i].split(';')[1];
                break;

            //                    case codTipoProdVentaVarios:  
            //                        strPlazosVentaVarios += '|' + arrPlazo[0] + ';' + arrPlazos[i].split(';')[1];  
            //                        break;  
        }
    }

    var strTiposProducto = '';

    for (var i = 1; i < arrPlanes.length; i++) {
        arrPlan = arrPlanes[i].split('_');

        strTipoProducto = arrPlan[8];

        if (strTiposProducto.indexOf(strTipoProducto) < 0) {
            strTiposProducto += '|' + strTipoProducto;

            switch (strTipoProducto) {
                case codTipoProdMovil:
                    setValue('hidCodigoTipoProductoActual', codTipoProdMovil);
                    setValue('hidTipoProductoActual', 'Movil');
                    break;
                case codTipoProdFijo:
                    setValue('hidCodigoTipoProductoActual', codTipoProdFijo);
                    setValue('hidTipoProductoActual', 'Fijo');
                    break;
                //PROY-31812 - IDEA-43340 - INICIO   
                case codTipoProdInterInalam:
                    setValue('hidCodigoTipoProductoActual', codTipoProdInterInalam);
                    setValue('hidTipoProductoActual', 'InterInalam');
                    break;
                //PROY-31812 - IDEA-43340 - FIN   
                case codTipoProdBAM:
                    setValue('hidCodigoTipoProductoActual', codTipoProdBAM);
                    setValue('hidTipoProductoActual', 'BAM');
                    break;
                case codTipoProdDTH:
                    setValue('hidCodigoTipoProductoActual', codTipoProdDTH);
                    setValue('hidTipoProductoActual', 'DTH');
                    break;
                case codTipoProd3Play:
                    setValue('hidCodigoTipoProductoActual', codTipoProd3Play);
                    setValue('hidTipoProductoActual', 'HFC');
                    break;
                //FTTH - inicio - hidCodigoTipoProductoActual   
                case codTipoProdFTTH:
                    setValue('hidCodigoTipoProductoActual', codTipoProdFTTH);
                    setValue('hidTipoProductoActual', 'FTTH');
                    break;
                //FTTH - fin - hidCodigoTipoProductoActual   
                case codTipoProd3PlayInalam:
                    setValue('hidCodigoTipoProductoActual', codTipoProd3PlayInalam);
                    setValue('hidTipoProductoActual', '3PlayInalam');
                    break;

                //                        case codTipoProdVentaVarios:  
                //                            setValue('hidCodigoTipoProductoActual', codTipoProdVentaVarios);  
                //                            setValue('hidTipoProductoActual', 'VentaVarios');  
                //                            break;  
            }
            //gaa20140826
            //if (getValue('hidTipoProductoActual') == 'HFC') {
            var hidTotalLineas = document.getElementById('hidTotalLineas');
            hidTotalLineas.value = parseInt(hidTotalLineas.value) + 5;
            idFila = getValue('hidTotalLineas');
            //}
            //fin gaa20140826
            agregarFila(false, 0, false);
            idFila++;

            ddlCampana = document.getElementById('ddlCampana' + idFila);
            ddlPlazo = document.getElementById('ddlPlazo' + idFila);
            ddlPlan = document.getElementById('ddlPlan' + idFila);
            ddlPaquete = document.getElementById('ddlPaquete' + idFila);

            switch (strTipoProducto) {
                case codTipoProdMovil:
                    llenarDatosCombo(ddlCampana, strCampanasMovil, false);
                    llenarDatosCombo(ddlPlazo, strPlazosMovil, true);

                    setValue('hidCampanasMovil', strCampanasMovil);
                    setValue('hidPlazosMovil', strPlazosMovil);
                    break;
                case codTipoProdFijo:
                    llenarDatosCombo(ddlCampana, strCampanasFijo, false);
                    llenarDatosCombo(ddlPlazo, strPlazosFijo, true);

                    setValue('hidCampanasFijo', strCampanasFijo);
                    setValue('hidPlazosFijo', strPlazosFijo);
                    break;
                //PROY-31812 - IDEA-43340 - INICIO  
                case codTipoProdInterInalam:
                    llenarDatosCombo(ddlCampana, strCampanasInterInalam, false);
                    llenarDatosCombo(ddlPlazo, strPlazosInterInalam, true);

                    setValue('hidCampanasInterInalam', strCampanasInterInalam);
                    setValue('hidPlazosInterInalam', strPlazosInterInalam); //INC000001299982
                    break;
                //PROY-31812 - IDEA-43340 - FIN  
                case codTipoProdBAM:
                    llenarDatosCombo(ddlCampana, strCampanasBAM, false);
                    llenarDatosCombo(ddlPlazo, strPlazosBAM, true);

                    setValue('hidCampanasBAM', strCampanasBAM);
                    setValue('hidPlazosBAM', strPlazosBAM);
                    break;
                case codTipoProdDTH:
                    llenarDatosCombo(ddlCampana, strCampanasDTH, false);
                    llenarDatosCombo(ddlPlazo, strPlazosDTH, true);

                    setValue('hidCampanasDTH', strCampanasDTH);
                    setValue('hidPlazosDTH', strPlazosDTH);
                    break;
                case codTipoProd3Play:
                    llenarDatosCombo(ddlCampana, strCampanasHFC, false);
                    llenarDatosCombo(ddlPlazo, strPlazosHFC, true);

                    setValue('hidCampanasHFC', strCampanasHFC);
                    setValue('hidPlazosHFC', strPlazosHFC);
                    break;

                //FTTH - Inicio strCampanasFTTH - strPlazosFTTH    
                case codTipoProdFTTH:
                    llenarDatosCombo(ddlCampana, strCampanasFTTH, false);
                    llenarDatosCombo(ddlPlazo, strPlazosFTTH, true);

                    setValue('hidCampanasFTTH', strCampanasFTTH);
                    setValue('hidPlazosFTTH', strPlazosFTTH);
                    break;

                //FTTH - Fin strCampanasFTTH - strPlazosFTTH     
                case codTipoProd3PlayInalam:
                    llenarDatosCombo(ddlCampana, strCampanasHFCInalamb, false);
                    llenarDatosCombo(ddlPlazo, strPlazosHFCInalamb, true);

                    setValue('hidCampanasHFCInalamb', strCampanasHFCInalamb);
                    setValue('hidPlazosHFCInalamb', strPlazosHFCInalamb);
                    break;

                //                        case codTipoProdVentaVarios:  
                //                            llenarDatosCombo(ddlCampana, strCampanasVentaVarios, false);  
                //                            llenarDatosCombo(ddlPlazo, strPlazosVentaVarios, true);  

                //                            setValue('hidCampanasVentaVarios', strCampanasVentaVarios);  
                //                            setValue('hidPlazosVentaVarios', strPlazosVentaVarios);  
                //                            break;  
            }

            ddlCampana.disabled = true;

            parent.mostrarTabProducto(getValue('hidCodigoTipoProductoActual'), true);

            visualizarIconosFinales(idFila);
        }

        switch (strTipoProducto) {
            case codTipoProdMovil:
                strPlanesMovil += '|' + arrPlanes[i];
                break;
            case codTipoProdFijo:
                strPlanesFijo += '|' + arrPlanes[i];
                break;
            //PROY-31812 - IDEA-43340 - INICIO   
            case codTipoProdInterInalam:
                strPlanesInterInalam += '|' + arrPlanes[i];
                break;
            //PROY-31812 - IDEA-43340 - FIN   
            case codTipoProdBAM:
                strPlanesBAM += '|' + arrPlanes[i];
                break;
            case codTipoProdDTH:
                strPlanesDTH += '|' + arrPlanes[i];
                break;
            case codTipoProd3Play:
                //strPlanesHFC += '|' + arrPlanes[i];
                strPlanesHFC += '|' + arrPlanes[i].split('_')[0] + ';' + arrPlanes[i].split(';')[1];
                break;
            case codTipoProd3PlayInalam:
                strPlanesHFCI += '|' + arrPlanes[i].split('_')[0] + ';' + arrPlanes[i].split(';')[1];
                break;
            case codTipoProdVentaVarios:
                strPlanesVentaVarios += '|' + arrPlanes[i];
                break;
        }
    }
    //PROY-31812 - IDEA-43340 Se agrega la variable strPlanesInterInalam
    llenarPlanesCombo(strPlanesMovil, strPlanesFijo, strPlanesInterInalam, strPlanesBAM, strPlanesDTH, strPlanesHFC, strPlanesVentaVarios, strPlanesHFCI);

    //gaa20151203
    var plazoReno = parent.document.getElementById('hidPlazoReno').value;
    var planReno = parent.document.getElementById('hidPlanReno').value;

    if (plazoReno.length > 0) {
        ddlPlazo.value = plazoReno;
        var plan;
        var planCodigo;

        for (var i = 1; i < ddlPlan.options.length; i++) {
            plan = ddlPlan.options[i].value;
            planCodigo = plan.split('_')[0];

            if (planCodigo == planReno) {
                ddlPlazo.disabled = true;
                ddlPlan.disabled = true;

                ddlPlan.value = plan;

                PageMethods.PintarLogsGenerico("INC000004509732", "[" + parent.document.getElementById("hidNroDocumento").value + "][sisact_ifr_condiciones_venta.js][asignarPlanesCombo()] plan ", plan);

                cambiarPlan(plan, idFila);
                break;
            }
        }
    }
    quitarImagenEsperando();
    //fin gaa20151203

    if (parent.document.getElementById('tdMovil').style.display != 'none') { f_MostrarTab('Movil'); return; }
    if (parent.document.getElementById('tdBAM').style.display != 'none') { f_MostrarTab('BAM'); return; }
    if (parent.document.getElementById('tdDTH').style.display != 'none') { f_MostrarTab('DTH'); return; }
    if (parent.document.getElementById('tdHFC').style.display != 'none') { f_MostrarTab('HFC'); return; }
    if (parent.document.getElementById('tdFijo').style.display != 'none') { f_MostrarTab('Fijo'); return; }
    if (parent.document.getElementById('tdVentaVarios').style.display != 'none') { f_MostrarTab('VentaVarios'); return; }
    if (parent.document.getElementById('td3PlayInalam').style.display != 'none') { f_MostrarTab('3PlayInalam'); return; }
}

function llenarCampanasPlanesCombo(idFila) {
    var strTipoProducto = getValue('hidCodigoTipoProductoActual');
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    ddlCampana.disabled = true;

    switch (strTipoProducto) {
        case codTipoProdMovil:
            llenarDatosCombo(ddlCampana, getValue('hidCampanasMovil'), false);
            llenarDatosCombo(ddlPlan, getValue('hidPlanesMovil'), true);
            if (ddlPlazo.value.length == 0)
                llenarDatosCombo(ddlPlazo, getValue('hidPlazosMovil'), true);
            break;
        case codTipoProdFijo:
            llenarDatosCombo(ddlCampana, getValue('hidCampanasFijo'), false);
            llenarDatosCombo(ddlPlan, getValue('hidPlanesFijo'), true);
            if (ddlPlazo.value.length == 0)
                llenarDatosCombo(ddlPlazo, getValue('hidPlazosFijo'), true);
            break;
        //PROY-31812 - IDEA-43340 - INICIO  
        case codTipoProdInterInalam:
            llenarDatosCombo(ddlCampana, getValue('hidCampanasInterInalam'), false);
            llenarDatosCombo(ddlPlan, getValue('hidPlanesInterInalam'), false);
            if (ddlPlazo.value.length == 0)
                llenarDatosCombo(ddlPlazo, getValue('hidPlazosInterInalam'), true);
            break;
        //PROY-31812 - IDEA-43340 - FIN  
        case codTipoProdBAM:
            llenarDatosCombo(ddlCampana, getValue('hidCampanasBAM'), false);
            llenarDatosCombo(ddlPlan, getValue('hidPlanesBAM'), true);
            if (ddlPlazo.value.length == 0)
                llenarDatosCombo(ddlPlazo, getValue('hidPlazosBAM'), true);
            break;
        case codTipoProdDTH:
            llenarDatosCombo(ddlCampana, getValue('hidCampanasDTH'), false);
            llenarDatosCombo(ddlPlan, getValue('hidPlanesDTH'), true);
            if (ddlPlazo.value.length == 0)
                llenarDatosCombo(ddlPlazo, getValue('hidPlazosDTH'), true);
            break;
        case codTipoProd3Play:
            llenarDatosCombo(ddlCampana, getValue('hidCampanasHFC'), false);
            llenarDatosCombo(ddlPlan, getValue('hidPlanesHFC'), true);
            if (ddlPlazo.value.length == 0)
                llenarDatosCombo(ddlPlazo, getValue('hidPlazosHFC'), true);
            break;
        case codTipoProdVentaVarios:
            llenarDatosCombo(ddlCampana, getValue('hidCampanasVentaVarios'), false);
            llenarDatosCombo(ddlPlan, getValue('hidPlanesVentaVarios'), true);
            if (ddlPlazo.value.length == 0)
                llenarDatosCombo(ddlPlazo, getValue('hidPlazosVentaVarios'), true);
            break;
    }
}
//PROY-31812 - IDEA-43340 Se agrega la variable strPlanesInterInalam
function llenarPlanesCombo(strPlanesMovil, strPlanesFijo, strPlanesInterInalam, strPlanesBAM, strPlanesDTH, strPlanesHFC, strPlanesVentaVarios, strPlanesHFCI) {
    var tabla;
    var fila;
    var cont = 0;
    var idFila = '';

    if (strPlanesMovil.length > 0) {
        tabla = document.getElementById('tblTablaMovil');
        cont = tabla.rows.length;

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
            llenarDatosCombo(document.getElementById('ddlPlan' + idFila), strPlanesMovil, true);
            setValue('hidPlanesMovil', strPlanesMovil);
        }
    }

    if (strPlanesFijo.length > 0) {
        tabla = document.getElementById('tblTablaFijo');
        cont = tabla.rows.length;

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
            llenarDatosCombo(document.getElementById('ddlPlan' + idFila), strPlanesFijo, true);
            setValue('hidPlanesFijo', strPlanesFijo);
        }
    }
    //PROY-31812 - IDEA-43340 - INICIO
    if (strPlanesInterInalam.length > 0) {
        tabla = document.getElementById('tblTablaInterInalam');
        cont = tabla.rows.length;

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
            llenarDatosCombo(document.getElementById('ddlPlan' + idFila), strPlanesInterInalam, true);
            setValue('hidPlanesInterInalam', strPlanesInterInalam);
        }
    }
    //PROY-31812 - IDEA-43340 - FIN
    if (strPlanesBAM.length > 0) {
        tabla = document.getElementById('tblTablaBAM');
        cont = tabla.rows.length;

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
            llenarDatosCombo(document.getElementById('ddlPlan' + idFila), strPlanesBAM, true);
            setValue('hidPlanesBAM', strPlanesBAM);
        }
    }

    if (strPlanesDTH.length > 0) {
        tabla = document.getElementById('tblTablaDTH');
        cont = tabla.rows.length;

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
            llenarDatosCombo(document.getElementById('ddlPlan' + idFila), strPlanesDTH, true);
            setValue('hidPlanesDTH', strPlanesDTH);
        }
    }

    if (strPlanesHFC.length > 0) {
        var tabla;
        var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');

        if (codigoTipoProductoActual == codTipoProd3Play) {
            tabla = document.getElementById('tblTablaHFC');
        };
        // FTTH Inicio- llenarPlanesCombo()
        if (codigoTipoProductoActual == codTipoProdFTTH) {
            tabla = document.getElementById('tblTablaFTTH');
        };
        // FTTH Fin- llenarPlanesCombo()
        if (codigoTipoProductoActual == codTipoProd3PlayInalam) {
            tabla = document.getElementById('tblTabla3PlayInalam');
        };

        cont = tabla.rows.length;

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
            llenarDatosCombo(document.getElementById('ddlPaquete' + idFila), strPlanesHFC, true);
            setValue('hidPlanesHFC', strPlanesHFC);
        }
    }

    if (strPlanesVentaVarios.length > 0) {
        tabla = document.getElementById('tblTablaVentaVarios');
        cont = tabla.rows.length;

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
            llenarDatosCombo(document.getElementById('ddlPlan' + idFila), strPlanesVentaVarios, true);
            setValue('hidPlanesVentaVarios', strPlanesVentaVarios);
        }
    }
}

function verificarCombo() {
    if (parent.document.getElementById('ddlCombo').value.length > 0)
        return true;
    else
        return false;
}

function verificarDireccion(idFila) {
    PageMethods.verificarDireccion(parent.getValue('hidNroDocumento'), idFila, verificarDireccion_Callback);
}

function verificarDireccion_Callback(objResponse) {
    if (objResponse.Error) {
        alert(objResponse.Mensaje);
    }
    else {
        if (objResponse.Boleano)
            return true;
        else {
            var idFila = objResponse.Cadena;
            if (!mostrarDirInst(idFila)) {
                eliminarFilaTotal(null, idFila, false);
                return;
            }
        }
    }
}

function LlenarListaPrecioIfr(idFila, strCampana, strEquipo) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&strOferta=' + parent.getValue('ddlOferta');
    url += '&strCampana=' + strCampana;
    url += '&strMaterial=' + strEquipo;
    url += '&strCanal=' + parent.getValue('hidCanalSap');
    url += '&strOrgVenta=' + parent.getValue('hidOrgVenta');
    url += '&strTipoDocVenta=' + parent.getValue('hidTipoDocVentaSap');
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strTipoOficina=' + parent.getValue('ddlCanal');
    url += '&strMetodo=' + "LlenarListaPrecio";

    self.frames['iframeAuxiliar'].location.replace(url);
}

function asignarListaPrecio(idFila, strResultado) {
    var ddlListaPrecio = document.getElementById('ddlListaPrecio' + idFila);
    llenarDatosCombo(ddlListaPrecio, strResultado, true);
}

function cambiarListaPrecio(strValor, idFila) {
    var strEquipo = document.getElementById('hidValorEquipo' + idFila).value;
    if (strValor != '') {
        LlenarListaPrecioPrecioIfr(idFila, strEquipo, strValor);
    } else {
        var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
        var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
        txtEquipoPrecio.value = '';
        hidListaPrecio.value = '';
    }
}

function LlenarListaPrecioPrecioIfr(idFila, strEquipo, strListaPrecio) {
    cargarImagenEsperando();

    var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
    url += 'idFila=' + idFila;
    url += '&strOficina=' + parent.getValue('hidnOficinaValue');
    url += '&strOrgVenta=' + parent.getValue('hidOrgVenta');
    url += '&strCanalSap=' + parent.getValue('hidCanalSap');
    url += '&strMaterial=' + strEquipo;
    url += '&strListaPrecio=' + strListaPrecio;
    url += '&strTipoDocVentaSap=' + parent.getValue('hidTipoDocVentaSap');
    url += '&strNroDoc=' + parent.getValue('txtNroDoc');
    url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
    url += '&strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
    url += '&strCampana=' + getValue('ddlCampana' + idFila);
    url += '&strOferta=' + parent.getValue('ddlOferta');
    url += '&strTipoOficina=' + parent.getValue('ddlCanal');
    url += '&strMetodo=' + "LlenarListaPrecioPrecio";

    self.frames['iframeAuxiliar'].location.replace(url);
}

function asignarListaPrecioPrecio(idFila, strResultado) {
    var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
    var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
    var ddlListaPrecio = document.getElementById('ddlListaPrecio' + idFila);
    var listaPrecioText = obtenerTextoSeleccionado(ddlListaPrecio);

    var arrResultado = strResultado.split('_');
    if (arrResultado.length > 0) {
        txtEquipoPrecio.value = arrResultado[0];
        arrResultado[2] = listaPrecioText;
        hidListaPrecio.value = arrResultado.join('_');
    }
    else {
        txtEquipoPrecio.value = '';
        hidListaPrecio.value = '';
    }
}
//gaa20151204
function validarPlanesRestringidosCombo() {
    var resultado;
    resultado = validarPlanesRestringidos1('Movil');
    if (!resultado) return false;
    resultado = validarPlanesRestringidos1('Fijo');
    if (!resultado) return false;
    resultado = validarPlanesRestringidos1('BAM');
    if (!resultado) return false;
    resultado = validarPlanesRestringidos1('DTH');
    if (!resultado) return false;
    resultado = validarPlanesRestringidos1('HFC');
    if (!resultado) return false;
    resultado = validarPlanesRestringidos1('VentaVarios');
    if (!resultado) return false;

    return true;
}

function validarPlanesRestringidos1(productoDes) {
    if (parent.getValue('hidPlazoReno').length > 0)
        return true;

    var planesRestringidos = parent.getValue('hidPlanComboRestringido');
    var fila;
    var idFila;
    var strPlanCodigo;
    var contR = 0;
    var contN = 0;

    var tabla = document.getElementById('tblTabla' + productoDes);
    var cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        strPlanCodigo = getValue('ddlPlan' + idFila).split('_')[0];

        if (planesRestringidos.indexOf('l' + strPlanCodigo + 'l') > -1)
            contR++;
        else
            contN++;
    }

    if (contN == 0 && contR > 0) {
        alert('¡Alerta! Primero selecciona el Plan Postpago con el cargo fijo mayor y luego realiza la Alta/Porta');
        return false;
    }
    else
        return true;

}
//fin gaa20151204
//gaa20160210
function campanaDiaEnamorados(idFila) {
    var campanaDECodigo = CampanaDiaEnamoradosAsociada;
    //var campanaDEDescripcion = '<%= ConfigurationManager.AppSettings["CampanaDiaEnamoradosAsociadaDes"] %>';
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var campanaDEDescripcion = obtenerTextoSeleccionado(ddlCampana);
    var idFilaNew = parseInt(idFila) + 1;
    //Obtener info de la fila
    agregarFila(false, 0, false);
    /*
    var ddlCampana = document.getElementById('ddlCampana' + idFila);
    var strCampana = getValue('ddlCampana' + idFila);
    var strCampanaSeleccionada = strCampana + ';' + obtenerTextoSeleccionado(ddlCampana);
    */
    var strCampanaSeleccionada = campanaDECodigo + ';' + campanaDEDescripcion;

    //INICIATIVA 920
    var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
    var plazoSinCode = ddlPlazo.split('-');
    var strPlazo = '';
    if(plazoSinCode > 1)
        strPlazo = plazoSinCode[0];
    else 
        strPlazo = getValue('ddlPlazo' + idFila);

    var strPlazoSeleccionado = strPlazo + ';' + obtenerTextoSeleccionado(ddlPlazo);

    var ddlPlan = document.getElementById('ddlPlan' + idFila);
    var strPlan = getValue('ddlPlan' + idFila);
    var strPlanSeleccionado = strPlan + ';' + obtenerTextoSeleccionado(ddlPlan);

    var strValorEquipo = getValue('hidValorEquipo' + idFila);
    var strTextoEquipo = getValue('txtTextoEquipo' + idFila);

    var hidTotalLineas = document.getElementById('hidTotalLineas');
    hidTotalLineas.value = idFilaNew
    var ddlCampanaNew = document.getElementById('ddlCampana' + idFilaNew);
    var ddlPlazoNew = document.getElementById('ddlPlazo' + idFilaNew);
    var ddlPlanNew = document.getElementById('ddlPlan' + idFilaNew);

    llenarDatosCombo(ddlCampanaNew, strCampanaSeleccionada, false);
    llenarDatosCombo(ddlPlazoNew, strPlazoSeleccionado, false);
    llenarDatosCombo(ddlPlanNew, strPlanSeleccionado, false);

    cambiarPlan(strPlan, idFilaNew);

    setValue('hidValorEquipo' + idFilaNew, strValorEquipo);
    setValue('txtTextoEquipo' + idFilaNew, strTextoEquipo);
}

function validarDEFueraCarrito() {
    var fila;
    var strCampana;
    var tablaMovil = document.getElementById('tblTablaMovil');
    var cont = tablaMovil.rows.length;

    for (var i = 0; i < cont; i++) {
        fila = tablaMovil.rows[i];
        if (fila.style.display != 'none') {
            strCampana = fila.cells[2].getElementsByTagName("select")[0].value;
            if (strCampana == consCampanaDiaEnamorados ||
                        strCampana == CampanaDiaEnamoradosAsociada) {
                alert('Para continuar debe agregar la promoción completa al carrito de compra');
                return false;
            }
        }
    }
    return true;
}
//fin gaa20160210

//gaa20151109
function validarEquipoSinStock() {
    var tabla = document.getElementById('tblTablaMovil');
    var contEquipoSinStock = 0;
    var cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10); //ddlCampana
        if (document.getElementById('txtTextoEquipo' + idFila).style.color != '')
            contEquipoSinStock++;
    }
    tabla = document.getElementById('tblTablaFijo');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10); //ddlCampana
        if (document.getElementById('txtTextoEquipo' + idFila).style.color != '')
            contEquipoSinStock++;
    }
    //PROY-31812 - IDEA-43340 - INICIO
    tabla = document.getElementById('tblTablaInterInalam');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10); //ddlCampana
        if (document.getElementById('txtTextoEquipo' + idFila).style.color != '')
            contEquipoSinStock++;
    }
    //PROY-31812 - IDEA-43340 - FIN
    tabla = document.getElementById('tblTablaBAM');
    cont = tabla.rows.length;
    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10); //ddlCampana
        if (document.getElementById('txtTextoEquipo' + idFila).style.color != '')
            contEquipoSinStock++;
    }

    return contEquipoSinStock;
}
//fin gaa20151109

//INICIO PROY-140419 Autorizar Portabilidad sin PIN
function mostrarPanelSMSSupervisor() {
    var strCanal = parent.document.getElementById('ddlCanal').value;
    var strTipoDoc = parent.document.getElementById('ddlTipoDocumento').value;
    var strTipoProd = getValue('hidCodigoTipoProductoActual');
    var strOferta = parent.document.getElementById('ddlOferta').value;
    PageMethods.ValidarParametroSmsSupervisor(strCanal, strTipoDoc, strTipoProd, strOferta, ValidarParametroSmsSupervisor_callback);
}

function ValidarParametroSmsSupervisor_callback(resp) {
    var blnPortabilidad = parent.document.getElementById('chkPortabilidad').checked;
    if (resp == true && blnPortabilidad) {
        parent.document.getElementById('pSmsSupervisor').style.display = 'block';
    } else {
        parent.document.getElementById('pSmsSupervisor').style.display = 'none';
    }
}
//FIN PROY-140419 Autorizar Portabilidad sin PIN

function obtenerPlanesFullClaro(strCadenaDetalle) {
    var arrCadenaDetalle = strCadenaDetalle.split('|');
    var strCadena = '';
    var arrDetalle;
    for (var i = 0; i < arrCadenaDetalle.length; i++) {
        if (arrCadenaDetalle[i] != '') {
            arrDetalle = arrCadenaDetalle[i].split(';');
            strCadena += arrDetalle[10] + "|";  //[5] - idPlan
        }
    }
    return strCadena;
}

//INI: PROY-140335 RF1
function LimpiarCamposPortabilidad() {
    var tipoProducto = getValue('hidTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;

    if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (tipoProducto == 'Movil' || tipoProducto == 'BAM')) {
        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            if (fila.style.display != 'none') {
                idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
                document.getElementById('txtEstadoCP' + idFila).value = '';
                //document.getElementById('txtFecActivacionCP' + idFila).value = '';
                //document.getElementById('txtDeudaCP' + idFila).value = '';
                document.getElementById('txtEstadoCP' + idFila).style.display = 'none';
            }
        }
    }
}

function estructuraGrillaPortabilidad() {

    var tipoProducto = getValue('hidTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (tipoProducto == 'Movil' || tipoProducto == 'BAM')) {
        //PROY-140335 - EJRC INICIO
        //if (parent.document.getElementById('tdConsultaPrevia').style.display != 'none') {
            //document.getElementById('tdEstadoCP' + tipoProducto).style.display = 'inline';
            //document.getElementById('tdFechaCP' + tipoProducto).style.display = 'inline';
            //document.getElementById('tdDeudaCP' + tipoProducto).style.display = 'inline';
        //} else {
            //document.getElementById('tdEstadoCP' + tipoProducto).style.display = 'none';
            //document.getElementById('tdFechaCP' + tipoProducto).style.display = 'none';
            //document.getElementById('tdDeudaCP' + tipoProducto).style.display = 'none';
        //}
        //PROY-140335 - EJRC FIN

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            if (fila.style.display != 'none') {
                idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

                document.getElementById('txtEstadoCP' + idFila).value = '';
                //document.getElementById('txtFecActivacionCP' + idFila).value = '';
                //document.getElementById('txtDeudaCP' + idFila).value = '';

                //PROY-140335 - EJRC INICIO
                //if (parent.document.getElementById('tdConsultaPrevia').style.display != 'none') {
                    document.getElementById(idFila + 'txtEstadoCP').style.display = 'inline';
                    //document.getElementById(idFila + 'txtFecActivacionCP').style.display = 'inline';
                    //document.getElementById(idFila + 'txtDeudaCP').style.display = 'inline';
                //} else {
                    //document.getElementById(idFila + 'txtEstadoCP').style.display = 'none';
                    //document.getElementById(idFila + 'txtFecActivacionCP').style.display = 'none';
                    //document.getElementById(idFila + 'txtDeudaCP').style.display = 'none';
                //}
                //PROY-140335 - EJRC INICIO
            }
        }
    }
}

function estructuraGrillaPortaCP() {

    var tipoProducto = getValue('hidTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    if (parent.getValue('hidNTienePortabilidadValues') == 'S' && (tipoProducto == 'Movil' || tipoProducto == 'BAM')) {

        for (var i = 0; i < cont; i++) {
            fila = tabla.rows[i];
            if (fila.style.display != 'none') {
                idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);
                document.getElementById(idFila + 'txtEstadoCP').style.display = '';
            }
        }
        document.getElementById('tdEstadoCPMovil').style.display = '';
    }
}
//FIN: PROY-140335 RF1

//IDEA-142010 INICIO
function obtenerDetalleCarritoCampana() {
    var tipoProducto = getValue('hidTipoProductoActual');
    var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    var fila;
    var idFila;
    var strNroTelefono = '';
    var strResultado = '';

    for (var i = 0; i < cont; i++) {
        var cadenaItem = '';
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        if (idFila.length == 0)
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);


        var ddlCampana = document.getElementById('ddlCampana' + idFila);
        if (ddlCampana != null) {
            cadenaItem = cadenaItem + ddlCampana.value + ';' + obtenerTextoSeleccionado(ddlCampana); 
        }
        else {
            cadenaItem = cadenaItem + ';'
        }

        strResultado = strResultado + '|' + cadenaItem;

    }
    return strResultado;
}
//IDEA-142010 FIN

//INICIO INICIATIVA-932
function obtenerPlanesEvaluados(strPlanDetalle) {

    var idFilaEval = '';
    var strProductoEval = '';
    var strPlanesEval = '';
    var strPlanEval = '';
    var strPlanBscsEval = '';
    var strDescripPlanEval = '';
    var arrPlanDetalleEval = strPlanDetalle.split('|');

    for (var i = 0; i < arrPlanDetalleEval.length; i++) {
        if (arrPlanDetalleEval[i] != '') {
            idFilaEval = arrPlanDetalleEval[i].split(';')[0];
            strProductoEval = arrPlanDetalleEval[i].split(';')[1];
            strPlanEval = arrPlanDetalleEval[i].split(';')[10];
            strPlanBscsEval = getValor(arrPlanDetalleEval[i].split(';')[9], 6);
            strDescripPlanEval = arrPlanDetalleEval[i].split(';')[11];

            strPlanesEval += idFilaEval + ';';
            strPlanesEval += strPlanEval + ';';
            strPlanesEval += strPlanBscsEval + ';';
            strPlanesEval += strDescripPlanEval + ';';
            strPlanesEval += strProductoEval + '|';
        }
    }
    return strPlanesEval;
}
//FIN INICIATIVA-932

//PROY-140743 - IDEA-141192 - Venta en cuotas | INI
function cambiarPromocion() {

    var ddlFamiliaPlan = document.getElementById('ddlFamiliaPlan' + obtenerIdFila());
    ddlFamiliaPlan.value = '0002';
    ddlFamiliaPlan.disabled = true;

}

function mostrarPopNroAsociar() {

    if (document.getElementById('hidValidarGuardarCuota').value.length > 0) {
        alert('Debe guardar las cuotas antes de ejecutar esta acción');
        return;
    }

    if (document.getElementById('hidValorEquipo' + obtenerIdFila()).value.length == 0) {
        alert(Key_MsjSelecAcc);
        return;
    }

    llenarCombo(document.getElementById('ddlProductoPU'), Key_OpcionLineaCuenta, true);

    cerrarServicio();
    cerrarEquipo();

    var guardarLineas = parent.getValue('hidGuardarLineas');
    var datosLineaCuenta = parent.getValue('hidDatosEvalVV');
    if (datosLineaCuenta != '') {
        var arrDatos = datosLineaCuenta.split('|');
        setValue('ddlProductoPU', arrDatos[10]);
        setValue('ddlLineasPU', arrDatos[9]);
    }

    document.getElementById('tbLineasFacturar').style.display = 'inline';
    parent.autoSizeIframe();
}

function obtenerIdFila() {

    var tipoProducto = getValue('hidTipoProductoActual');
    var tabla = document.getElementById('tblTabla' + tipoProducto);
    var cont = tabla.rows.length;
    var fila;
    var idFila = '';

    for (var i = 0; i < cont; i++) {
        var cadenaItem = '';
        fila = tabla.rows[i];
        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

        if (idFila.length == 0)
            idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);
    }

    return idFila;

}

function guardarLinea() {

    var imgVerLineasAsociadas = document.getElementById('imgVerLineasAsociadas' + obtenerIdFila());

    if (document.getElementById('ddlLineasPU').value == '00' || document.getElementById('ddlLineasPU').value == '') {
        alert(Key_MsjSelecLineaCuenta);
        return;
    }

    var datosEvalVV = '';
    var idFila = obtenerIdFila();

    datosEvalVV = getValue('ddlCampana' + idFila);//0
    datosEvalVV += '|' + getText('ddlCampana' + idFila); //1
    datosEvalVV += '|' + getValue('ddlPromocion' + idFila); //2
    datosEvalVV += '|' + getText('ddlPromocion' + idFila); //3
    datosEvalVV += '|' + document.getElementById('hidValorEquipo' + idFila).value; //4
    datosEvalVV += '|' + document.getElementById('ddlNroCuotas').value; //5
    datosEvalVV += '|' + getText('ddlLineasPU'); //6
    datosEvalVV += '|' + document.getElementById('hidListaPrecio' + idFila).value; //7
    datosEvalVV += '|' + getText('ddlProductoPU'); //8
    datosEvalVV += '|' + getValue('ddlLineasPU'); //9
    datosEvalVV += '|' + getValue('ddlProductoPU'); //10

    parent.setValue('hidProdCuentaFact', getText('ddlProductoPU')); //PROY-140743
    parent.setValue('hidDatosEvalVV', datosEvalVV);
    parent.setValue('hidGuardarLineas', '1');

    imgVerLineasAsociadas.src = '../../Imagenes/btn_seleccionar.gif';
    document.getElementById('tbLineasFacturar').style.display = 'none';
    parent.autoSizeIframe();

}

function cargarLineasCuentas() {

    var ddlProductoPU = document.getElementById('ddlProductoPU');

    PageMethods.obtenerLineasCuentas(ddlProductoPU.value, function (response) {
        llenarCombo(document.getElementById('ddlLineasPU'), response, true);
        
    });

}

function cerrarLineasCuentas() {
    document.getElementById('tbLineasFacturar').style.display = 'none';
    parent.setValue('hidGuardarLineas', '0');
    parent.autoSizeIframe();
}

function eliminarLineaCuenta() {
    llenarCombo(document.getElementById('ddlProductoPU'), '', true);
    llenarCombo(document.getElementById('ddlLineasPU'), '', true);
    parent.setValue('hidProdCuentaFact', '');
    parent.setValue('hidDatosEvalVV', '');
    parent.setValue('hidGuardarLineas', '0');
}

//PROY-140743 - IDEA-141192 - Venta en cuotas | FIN