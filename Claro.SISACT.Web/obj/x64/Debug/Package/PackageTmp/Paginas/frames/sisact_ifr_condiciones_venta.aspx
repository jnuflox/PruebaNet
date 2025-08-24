<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_ifr_condiciones_venta.aspx.cs"
    Inherits="Claro.SISACT.Web.frames.sisact_ifr_condiciones_venta" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>sisact_ifr_condiciones_venta</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <link title="win2k-cold-1" media="all" href="../../estilos/calendar-blue.css" type="text/css"
        rel="stylesheet" />
    <script language='javascript' type='text/javascript' src='../../Scripts/funciones_sec.js'></script>
    <script language='javascript' type="text/javascript" src="../../Scripts/calendar/calendar.js"></script>
    <script language='javascript' type="text/javascript" src="../../Scripts/calendar/calendar_es.js"></script>
    <script language='javascript' type="text/javascript" src="../../Scripts/calendar/calendario_call.js"></script>
    <script language='javascript' type="text/javascript" src="../../Scripts/calendar/calendar_setup.js"></script>
    <script language="JavaScript" type="text/javascript">

        var columnaPaquete = 4;
        var columnaEquipo = 9;
        var columnaEquipoPrecio = 10;
        var columnaCuota = 11;
        var columnaPortabilidad = 12;

        var flujoAlta = '<%= ConfigurationManager.AppSettings["flujoAlta"] %>';
        var flujoPortabilidad = '<%= ConfigurationManager.AppSettings["flujoPortabilidad"] %>';

        var topeConsumoAuto = '<%= ConfigurationManager.AppSettings["constCodTopeAutomatico"] %>';
        var topeConsumoCero = '<%= ConfigurationManager.AppSettings["constCodTopeCeroServicio"] %>';
        var topeConsumoSinCF = '<%= ConfigurationManager.AppSettings["constCodTopeSinCFServicio"] %>';
        var codTipoOficinaCAC = '<%= ConfigurationManager.AppSettings["constCodTipoOficinaCAC"] %>';

        var codTipoProdMovil = '<%= ConfigurationManager.AppSettings["constTipoProductoMovil"] %>';
        var tipoProductoBussiness = '<%= ConfigurationManager.AppSettings["constCodTipoProductoBUS"] %>';
        var codTipoProdFijo = '<%= ConfigurationManager.AppSettings["constTipoProductoFijo"] %>';
        var codTipoProdBAM = '<%= ConfigurationManager.AppSettings["constTipoProductoBAM"] %>';
        var codTipoProdDTH = '<%= ConfigurationManager.AppSettings["constTipoProductoDTH"] %>';
        var codTipoProd3Play = '<%= ConfigurationManager.AppSettings["constTipoProductoHFC"] %>';
        var codTipoProdVentaVarios = '<%= ConfigurationManager.AppSettings["constTipoProductoVentaVarios"] %>';
        var codModalidadPagoEnCuota = '<%= ConfigurationManager.AppSettings["constCodModalidadCuota"] %>';
        var codModalidadChipSuelto = '<%= ConfigurationManager.AppSettings["constCodModalidadChipSuelto"] %>';
        var codModalidadContratoCede = '<%= ConfigurationManager.AppSettings["constCodModalidadContrato"] %>';
        var codPlazoAcuerdoSinPlazo = '<%= ConfigurationManager.AppSettings["constCodPlazoAcuerdoSinPlazo"] %>';
        var codTipoOperMigracion = '<%= ConfigurationManager.AppSettings["constTipoOperacionMIG"] %>';
        var codTipoProd3PlayInalam = '<%= ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"] %>';

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
            if (ddlOferta.value == '<%= ConfigurationManager.AppSettings["constCodTipoProductoB2E"] %>') {
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

            if (idFilaX > 0)
                idFila = idFilaX + 1;

            oCell = newRow.insertCell();
            oCell.style.width = '20px';
            oCell.align = 'center';
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
            if (parent.getValue('hidTienePortabilidad') == 'S')
                strFlujo = flujoPortabilidad;

            if (strPlazoActual.length > 0) {
                if (!booVeriConf)
                    asignarPaquete(idFila, getValue('hidPaqActCompleto'));
            }

            cerrarServicio();
            cerrarCuota();

            if (verificarCombo() && booVeriConf)
                llenarCampanasPlanesCombo(idFila);
        }

        function estructuraGrilla(newRow, idFila, desTipoProductoActual, flgSecPendiente) {
            
            var disabled = "";
            var readonly = "";
            var flujoPorta = parent.getValue('hidTienePortabilidad');
            var tienePaquete = document.getElementById('hidTienePaquete').value;

            if (flgSecPendiente) {
                disabled = " disabled";
                readonly = " readonly";
            }

            var oCell = newRow.insertCell();
            oCell.style.width = '192px';
            oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlCampana" + idFila + "' id='ddlCampana" + idFila + "' onchange='cambiarCampana(" + idFila + ", this.value);'><option value=''>SELECCIONE...</option></select>";

            if (desTipoProductoActual != 'DTH' && desTipoProductoActual != 'HFC' && desTipoProductoActual != '3PlayInalam') {

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

                if (desTipoProductoActual != 'VentaVarios') {

                    oCell = newRow.insertCell();
                    oCell.style.width = '152px';
                    oCell.innerHTML = "<select" + disabled + " style='width:100%' class='clsSelectEnable0' name='ddlPlan" + idFila + "' id='ddlPlan" + idFila + "' onchange='cambiarPlan(this.value, " + idFila + ");'><option>SELECCIONE...</option></select>";

                    oCell = newRow.insertCell();
                    oCell.style.width = '32px';
                    oCell.align = 'center';

                    if (flgSecPendiente == "S")
                        oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicioGuardado(" + idFila + ");' />";
                    else
                        oCell.innerHTML = "<img src = '../../Imagenes/abrir.gif' border='0' style='cursor:hand' alt='Ver Servicios' onclick='mostrarServicio(" + idFila + ");' />";

                    oCell = newRow.insertCell();
                    oCell.style.width = '47px';
                    oCell.align = 'center';
                    oCell.innerHTML = "<input style='width:90%; text-align:right' readonly class='clsInputDisable' name='txtCFPlanServicio" + idFila + "' id='txtCFPlanServicio" + idFila + "' /><input type='hidden' id='hidMontoServicios" + idFila + "' name='hidMontoServicios" + idFila + "' /><input type='hidden' id='hidMontoCuota" + idFila + "' name='hidMontoCuota" + idFila + "' />";

                    if (desTipoProductoActual == 'Movil' || desTipoProductoActual == 'Fijo') {

                        oCell = newRow.insertCell();
                        oCell.style.width = '52px';
                        oCell.align = 'center';
                        oCell.innerHTML = "<input style='width:90%; text-align:right' readonly class='clsInputDisable' name='txtMontoTopeConsumo" + idFila + "' id='txtMontoTopeConsumo" + idFila + "' />";
                    }
                }

                oCell = newRow.insertCell();
                oCell.style.width = '192px';
                oCell.style.whiteSpace = 'nowrap';
                oCell.innerHTML = "<div class='AutoComplete_Div'><input type='hidden' id='hidMaterial" + idFila + "' name='hidMaterial" + idFila + "' /><input type='hidden' id='hidValorEquipo" + idFila + "' name='hidValorEquipo" + idFila + "' /><input" + disabled + " type='text' id='txtTextoEquipo" + idFila + "' name='txtTextoEquipo" + idFila + "' class='clsSelectEnable0' style='width: 165px' onclick=mostrarListaSel(" + idFila + ") onkeyup=buscarCoincidente(" + idFila + ") onblur=ocultarListaTxt(" + idFila + ") /><img id='imgListaEquipo" + idFila + "' src='../../Imagenes/cmb.gif' style='height: 17px; cursor: pointer' align='absMiddle' title='Mostrar Lista' alt='Mostrar Lista' onclick=mostrarListaSel(" + idFila + ") onmouseover='imgSel(this)' onmouseout='imgNoSel(this)'; onblur=ocultarListaTxt(" + idFila + ") /></div><div id='divListaEquipo" + idFila + "' class='AutoComplete_List' style='width: 255px; display: none; z-index:10'; onblur=ocultarListaTxt(" + idFila + ")></div><input type='hidden' id='hidKit" + idFila + "' name='hidKit" + idFila + "' />";

                if (flgSecPendiente) {
                    var imgListaEquipo = document.getElementById('imgListaEquipo' + idFila);
                    imgListaEquipo.style.display = 'none';
                }

                if (parent.getValue('ddlModalidadVenta') == codModalidadChipSuelto) {
                    oCell.style.display = 'none';
                }

                if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota && desTipoProductoActual != 'VentaVarios') {

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

                if (parent.getValue('ddlModalidadVenta') == codModalidadChipSuelto) {
                    oCell.style.display = 'none';
                }

                if (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion || (flujoPorta == 'S' && desTipoProductoActual != 'VentaVarios')) {
                    oCell = newRow.insertCell();
                    oCell.style.width = '62px';
                    oCell.innerHTML = "<input" + disabled + readonly + " style='width:90%; text-align:right' class='clsInputEnable' maxlength='9' name='txtNroTelefono" + idFila + "' id='txtNroTelefono" + idFila + "' onkeypress='eventoSoloNumerosEnteros();' />";
                }

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

                if (flujoPorta == 'S') {
                    oCell = newRow.insertCell();
                    oCell.style.width = '62px';
                    oCell.innerHTML = "<input" + disabled + readonly + " style='width:90%; text-align:right; display:none' class='clsInputEnable' maxlength='9' name='txtNroTelefono" + idFila + "' id='txtNroTelefono" + idFila + "' onkeypress='eventoSoloNumerosEnteros();' />";
            }
            }
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

            var ver = "inline";
            if (!visible) ver = "none";

            tblTablaTituloMovil.rows[0].cells[10].style.display = ver;
            tblTablaTituloFijo.rows[0].cells[10].style.display = ver;
            tblTablaTituloBAM.rows[0].cells[9].style.display = ver;
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

            var ver = "inline";
            if (!visible) ver = "none";

            // Equipo
            tblTablaTituloMovil.rows[0].cells[11].style.display = ver;
            tblTablaTituloFijo.rows[0].cells[11].style.display = ver;
            tblTablaTituloBAM.rows[0].cells[10].style.display = ver;
            // Precio Equipo
            tblTablaTituloMovil.rows[0].cells[9].style.display = ver;
            tblTablaTituloFijo.rows[0].cells[9].style.display = ver;
            tblTablaTituloBAM.rows[0].cells[8].style.display = ver;
        }

        function mostrarColumnaTelefono(visible) {
            var tblTablaTituloMovil = document.getElementById('tblTablaTituloMovil');
            var tblTablaTituloBAM = document.getElementById('tblTablaTituloBAM');
            var tblTablaTituloFijo = document.getElementById('tblTablaTituloFijo');
            var tblTablaTituloHFC = document.getElementById('tblTablaTituloHFC');

            var txtTitulo = "Nro. a Portar";
            if (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion) txtTitulo = "Nro. a Migrar";

            var ver = "inline";
            if (!visible) ver = "none";

            tblTablaTituloMovil.rows[0].cells[12].style.display = ver;
            tblTablaTituloBAM.rows[0].cells[11].style.display = ver;
            tblTablaTituloFijo.rows[0].cells[12].style.display = ver;
            tblTablaTituloHFC.rows[0].cells[12].style.display = ver;

            if (parent.getValue('hidTienePortabilidad') != 'S')
                tblTablaTituloHFC.rows[0].cells[12].style.display = "none";

            tblTablaTituloMovil.rows[0].cells[12].innerHTML = txtTitulo;
            tblTablaTituloBAM.rows[0].cells[11].innerHTML = txtTitulo;
            tblTablaTituloFijo.rows[0].cells[12].innerHTML = txtTitulo;
            tblTablaTituloHFC.rows[0].cells[12].innerHTML = txtTitulo;
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
            calcularLCxProductoFijo();
        }

        function llenarMaterial(idFila, strValor) {
            document.getElementById('hidMaterial' + idFila).value = strValor;
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
            var strTienePortabilidad = parent.document.getElementById('hidTienePortabilidad').value;
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
                cambiarPlazo(ddlPlazo.value, idFila);
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
            if (codigoTipoProductoActual == codTipoProdDTH || codigoTipoProductoActual == codTipoProd3Play || codigoTipoProductoActual == codTipoProd3PlayInalam) {
                if (imgVerCuota != null)
                    imgVerCuota.style.display = 'none';

                var imgDirInst = document.getElementById('imgDirInst' + idFila);

                if (imgDirInst != null)
                    imgDirInst.style.display = '';

                if ((codigoTipoProductoActual == codTipoProd3Play) || (codigoTipoProductoActual == codTipoProd3PlayInalam))
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

        function llenarCampanaPlazoIfr(idFila) {
            var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
            var ddlCampana = document.getElementById('ddlCampana' + idFila);
            var strCampanasDTH = getValue('hidCampanasDTH');
            var strCampanasHFC = getValue('hidCampanasHFC');
            var strCampanasMovil = getValue('hidCampanasMovil');
            var strCampanasFijo = getValue('hidCampanasFijo');
            var strCampanasBAM = getValue('hidCampanasBAM');
            var strCampanasVentaVarios = getValue('hidCampanasVentaVarios');
            var strCampanasHFCInalamb = getValue('hidCampanasHFCInalamb');
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
            url += '&strOficina=' + parent.getValue('hidOficina');
            url += '&strOferta=' + parent.getValue('ddlOferta');
            url += '&strCampana=' + getValue('ddlCampana' + idFila);
            url += '&strCombo=' + parent.getValue('ddlCombo');
            url += '&strNroDoc=' + parent.getValue('txtNroDoc');
            url += '&strModalidadVenta=' + parent.getValue('ddlModalidadVenta');
            url += '&strFlujo=' + obtenerFlujo();
            url += '&strTipoDocumento=' + parent.getValue('ddlTipoDocumento');
            url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
            url += '&strMetodo=' + 'LlenarCampanaPlazo';

            self.frames['iframeAuxiliar'].location.replace(url);
        }

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
                case codTipoProdVentaVarios:
                    setValue('hidCampanasVentaVarios', strCampanas);
                    setValue('hidPlazosVentaVarios', strPlazos);
                    break;
                case codTipoProd3PlayInalam:
                    setValue('hidCampanasHFCInalamb', strCampanas);
                    setValue('hidPlazosHFCInalamb', strPlazos);
                    break;
            }

            llenarDatosCombo(ddlCampana, strCampanas, true);
            llenarDatosCombo(ddlPlazo, strPlazos, true);
        }

        function mostrarDirInst(idFila) {
            var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
            
            if (codigoTipoProductoActual != codTipoProdDTH && codigoTipoProductoActual != codTipoProd3Play && codigoTipoProductoActual != codTipoProd3PlayInalam)
                return true;

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
                if (codigoTipoProductoActual == codTipoProd3PlayInalam) {

                    if (document.getElementById('ddlServicio' + idFila).disabled)
                           intDeshabilitado = 'S';

                       url += '&flgReadOnly=' + intDeshabilitado;
                       url += '&flgVentaProactiva=' + flgVentaProactiva;
                       
                       url += '&codigoTipoProductoActual=' + codigoTipoProductoActual;
                       
                       strTieneDireccion = window.showModalDialog(url, '', 'dialogHeight:350px; dialogWidth:1000px');
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

                        ddl.add(option);
                    }
                }
            }
        }
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

            if (document.getElementById('hidValidarGuardarCuota').value.length > 0) {
                alert('Debe guardar las cuotas antes de ejecutar esta acción');
                return false;
            }

            var hidGrupoPaquete = document.getElementById('hidGrupoPaquete');
            var strGrupoPaquete = hidGrupoPaquete.value;
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
                    hidGrupoPaquete.value = strGrupoPaquete.replace(strGrupoPaquete.substring(intPosIni, intPosFin), '');
                else
                    return strGrupoPaquete.substring(intPosIni, intPosFin);
            }
            else
                eliminarFila(fila, idFila);

            return true;
        }

        function eliminarFilaGrupal(idFila) {
            var producto = obtenerProductoxIdFila(idFila);
            var tabla = document.getElementById('tblTabla' + producto);
            var cont = tabla.rows.length;
            var strValor;
            var intPosIni;

            cerrarServicio();
            cerrarCuota();
            cerrarEquipo();

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
            borrarServicio(idFila);
            borrarCuota(idFila);
            borrarEquipo(idFila);

            cerrarServicio();
            cerrarCuota();
            cerrarEquipo();

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
            var blnCasoEspecialCMA = (strCasoEpecial == '<%= ConfigurationManager.AppSettings["constCETrabajadoresCMA"] %>');

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
            document.getElementById('hidPlanServicio').value = '';
            document.getElementById('hidPlanServicioNo').value = '';
            document.getElementById('hidPlanServicioNoGrupo').value = '';
            document.getElementById('hidPlanServicioNGTemp').value = '';
            document.getElementById('hidLineaActual').value = '0';
            document.getElementById('hidTotalLineas').value = '0';
            document.getElementById('hidPlazoActual').value = '';
            document.getElementById('hidCuota').value = '';
            document.getElementById('hidNroCuotaActual').value = '';
            document.getElementById('hidGrupoPaquete').value = '';

            document.getElementById('hidCampanasMovil').value = '';
            document.getElementById('hidCampanasFijo').value = '';
            document.getElementById('hidCampanasBAM').value = '';
            document.getElementById('hidCampanasDTH').value = '';
            document.getElementById('hidCampanasHFC').value = '';
            document.getElementById('hidCampanasVentaVarios').value = '';

            document.getElementById('hidPlazosMovil').value = '';
            document.getElementById('hidPlazosFijo').value = '';
            document.getElementById('hidPlazosBAM').value = '';
            document.getElementById('hidPlazosDTH').value = '';
            document.getElementById('hidPlazosHFC').value = '';
            document.getElementById('hidPlazosVentaVarios').value = '';

            document.getElementById('hidPlanesMovil').value = '';
            document.getElementById('hidPlanesFijo').value = '';
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
            traerPlazo('S');
            document.getElementById('txtCFTotal').value = '0';
        }

        function editarFila(idFila, soloHabilitar) {
            var codigoTipoProducto = getValue('hidCodigoTipoProductoActual');
            var imgEditarFila = document.getElementById('imgEditarFila' + idFila);
            var strTienePaquete = getValue('hidTienePaquete');

            if (imgEditarFila == null)
                return;

            var booEs3Play = false;

            if ((codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam) && !soloHabilitar) {
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

            if (imgListaEquipo != null && !booEs3Play) {
                var txtTextoEquipo = document.getElementById('txtTextoEquipo' + idFila);
                txtTextoEquipo.disabled = false;
                imgListaEquipo.style.display = '';
            }

            if (txtNroTelefono != null)
                setEnabled('txtNroTelefono' + idFila, true, 'clsInputEnabled');

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

            cerrarServicio();
            cerrarCuota();
            cerrarEquipo();

            autoSizeProducto();

            if (getValue('hidPlanesCombo').length > 0)
                ddlPlan.disabled = true;
        }

        function preservarFila(idFila) {
            var ddlPlan = document.getElementById('ddlPlan' + idFila);
            var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
            var ddlServicio = document.getElementById('ddlServicio' + idFila);
            var ddlCampana = document.getElementById('ddlCampana' + idFila);
            var hidValorEquipo = document.getElementById('hidValorEquipo' + idFila);
            var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
            var txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
            var ddlListaPrecio = document.getElementById('ddlListaPrecio');

            if (ddlCampana.value.length == 0) {
                if (!ddlCampana.disabled) {
                    ddlCampana.focus();
                    alert('Debe seleccionar una campaña');
                    return false;
                }
            }

            if (ddlPlazo != null) {
                if (ddlPlazo.value.length == 0) {
                    if (!ddlPlan.disabled) {
                        ddlPlazo.focus();
                        alert('Debe seleccionar un plazo');
                        return false;
                    }
                }
            }

            if (ddlPlan != null) {
                if (ddlPlan.value.length == 0) {
                    if (!ddlPlan.disabled) {
                        ddlPlan.focus();
                        alert('Debe seleccionar un plan');
                        return false;
                    }
                }
            }

            if (getValue('hidCodigoTipoProductoActual') != codTipoProd3Play && getValue('hidCodigoTipoProductoActual') != codTipoProd3PlayInalam) {
                var plazo = ddlPlazo.value;
                var listaPlazoEquipo = '<%= ConfigurationManager.AppSettings["constPlazoConEquipo"] %>';
                if (parent.getValue('ddlModalidadVenta') != codModalidadChipSuelto) {
                    if (listaPlazoEquipo.indexOf(plazo) > 0 || parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota) {
                        if (hidValorEquipo.value.length == 0) {
                            alert('Debe seleccionar un equipo');
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
                        return false;
                    }
                }
            }

            if (ddlListaPrecio != null) {
                if (ddlListaPrecio.value.length == 0) {
                    if (!ddlListaPrecio.disabled) {
                        ddlListaPrecio.focus();
                        alert('Debe seleccionar una lista de precio');
                        return false;
                    }
                }
            }

            if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota) {
                var arrCuota = obtenerCuotaValores(idFila);
                if (arrCuota == null)
                {
                    alert('Debe seleccionar la cuota');
                    return false;
                }
            }

            if (document.getElementById('tblServicios').style.display != 'none') {
                alert('Debe guardar los servicios');
                return false;
            }

            if (document.getElementById('tblCuotas').style.display != 'none') {
                alert('Debe guardar las cuotas');
                return false;
            }

            if (document.getElementById('tblEquipos').style.display != 'none') {
                alert('Debe guardar los equipos');
                return false;
            }

            if (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion || parent.getValue('hidTienePortabilidad') == 'S') {

                var nroTelefonoValido = 9;
                if (getValue('hidCodigoTipoProductoActual') == codTipoProdFijo || getValue('hidCodigoTipoProductoActual') == codTipoProd3Play)
                    nroTelefonoValido = 8;

           if (getValue('hidCodigoTipoProductoActual') != codTipoProd3PlayInalam) { 
                if (txtNroTelefono != null) {
                if (txtNroTelefono.value.length != nroTelefonoValido) {
                    if (!txtNroTelefono.disabled && txtNroTelefono.style.display != 'none') {
                        txtNroTelefono.focus();
                        alert('Debe ingresar un número de teléfono válido');
                        return false;
                    }
                }
            }
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
            
            cerrarServicio();
            cerrarCuota();
            cerrarEquipo();

            if (ddlPlazo != null)
                hidPlazoActual.value = ddlPlazo.value + "_" + obtenerTextoSeleccionado(ddlPlazo);

            var strNroCuotas = document.getElementById('ddlNroCuotas').value;
            var hidNroCuotaActual = document.getElementById('hidNroCuotaActual');

            if (strNroCuotas.length > 0) {
                if (parseInt(strNroCuotas.substring(0, 2)) > 0)
                    hidNroCuotaActual.value = strNroCuotas;
            }
        }
        
        function inicializarPlan(idFila) {
            borrarServicio(idFila);
            cerrarServicio();

            inicializarEquipo(idFila);
            borrarEquipo(idFila);
            cerrarEquipo();

            borrarCuota(idFila)
            cerrarCuota();

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
            var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');

            inicializarPlan(idFila);
            if (strPlan.length == 0) {
                document.getElementById('hidLineaActual').value = idFila;

                calcularLCxProductoFijo();
                calcularCFxProducto();

                llenarDatosCombo1(document.getElementById('divListaEquipo' + idFila), '', idFila, 'Equipo', false);

                return;
            }

            if (codigoTipoProductoActual == codTipoProd3Play || codigoTipoProductoActual == codTipoProd3PlayInalam) {
               LlenarServicioHFCIfr(idFila, strPlan, codigoTipoProductoActual);
                return;
            }

            var strPlanCodigo = getValor(strPlan, 0);
            var strPlanCodigoSap = getValor(strPlan, 2);
            var strPlanCF = getValor(strPlan, 1);
            var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
            var ddlCampana = document.getElementById('ddlCampana' + idFila);
            var strCampana = ddlCampana.value;
            var strPlazo = ddlPlazo.value;

            var strPlanesBolsa = window.parent.document.getElementById('hidPlanBase').value;
            var ddlPlan = document.getElementById('ddlPlan' + idFila);

            if (strPlanesBolsa.indexOf('|' + strPlanCodigo) > -1) {
                var datosBolsa = mostrarPopupPlanCombo(strPlanCodigo, obtenerTextoSeleccionado(ddlPlan));

                if (datosBolsa != undefined) {
                    if (datosBolsa.length > 0) {
                        var strPlanActualizado = datosBolsa.split('|')[0];

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

            if (codigoTipoProductoActual != codTipoProdDTH){
                LlenarServicioMaterialIfr(idFila, strPlanCodigo, strPlanCodigoSap, strCampana, strPlazo);
            }else{
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

        function LlenarServicioHFCIfr(idFila, strPlan,codigoTipoProductoActual) {
            cargarImagenEsperando();

            var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
            url += 'idFila=' + idFila + '&strPlan=' + strPlan;
            url += '&strNroDoc=' + parent.getValue('txtNroDoc');
            url += '&strMetodo=' + 'LlenarServicioHFC';
            url += '&strTipoProducto=' + codigoTipoProductoActual;
            self.frames['iframeAuxiliar'].location.replace(url);
        }

        function LlenarServicioMaterialIfr(idFila, strPlan, pstrPlanCodigoSap, strCampana, strPlazo) {
            cargarImagenEsperando();

            var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
            url += 'idFila=' + idFila
            url += '&strCasoEspecial=' + getValor(parent.getValue('ddlCasoEspecial'), 0);
            url += '&strTipoProducto=' + getValue('hidCodigoTipoProductoActual');
            url += '&strPlan=' + strPlan;
            url += '&strPlanSap=' + pstrPlanCodigoSap;
            url += '&strFlagServicioRI=' + parent.getValue('hidFlagRoamingI');
            url += '&strCampana=' + strCampana;
            url += '&strOficina=' + parent.getValue('hidOficina');
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
            url += '&strOficina=' + parent.getValue('hidOficina');
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

                    agregarGrupo(idFila, true);
                    guardarServicio();
                }
                else
                    calcularCFxProducto();

                calcularLCxProductoFijo();
            }
        }

        function asignarServicioMaterial(idFila, pstrResultado) {
            var arrResultado;
            var strServicios;

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

                    agregarGrupo(idFila, true);
                    guardarServicio();
                }
                else
                    calcularCFxProducto();

                calcularLCxProductoFijo();
            }
        }

        function LlenarServCampCorpTot(strPlanes) {
            cargarImagenEsperando();

            var strCuota = '';
            if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota) {
                var arrCuota = obtenerCuotaValores(idFila);
                if (arrCuota != null) {
                    strCuota = arrCuota[0].split('_')[0];
                }
            }

            var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
            url += 'strTipoDocumento=' + parent.getValue('ddlTipoDocumento');
            url += '&strOficina=' + parent.getValue('hidOficina');
            url += '&strCasoEspecial=' + getValor(parent.getValue('ddlCasoEspecial'), 0);
            url += '&strRiesgo=' + parent.getValue('hidRiesgoDC');
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
            var strPlazo = ddlPlazo.value + "_" + obtenerTextoSeleccionado(ddlPlazo);
            var strPlazos = '|' + ddlPlazo.value + ";" + obtenerTextoSeleccionado(ddlPlazo);
            var strPlazoCodigo = ddlPlazo.value;
            var hidPlazoActual = document.getElementById('hidPlazoActual');
            var hidPaqueteActual = document.getElementById('hidPaqueteActual');
            var hidGrupoPaquete = document.getElementById('hidGrupoPaquete');
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

            hidGrupoPaquete.value += '{'

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
                hidGrupoPaquete.value += ',[' + idFila + ']';
                llenarMaterial(idFila, '');
                //LlenarMaterialIfr(idFila, ddlCampana.value, strPlanCodigo);
                asignarMaterial(idFila);
            }

            LlenarServCampCorpTot(strPlanes);

            hidGrupoPaquete.value += '}'

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
            var strPlazo = '|' + ddlPlazo.value + ";" + obtenerTextoSeleccionado(ddlPlazo);
            var ddlPlan = document.getElementById('ddlPlan' + idFila);
            var strPlanActual = '|' + ddlPlan.value + ";" + obtenerTextoSeleccionado(ddlPlan);
            var hidPlazoActual = document.getElementById('hidPlazoActual');
            var hidPaqueteActual = document.getElementById('hidPaqueteActual');
            var hidGrupoPaquete = document.getElementById('hidGrupoPaquete');
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
            var hidPlanServicio = document.getElementById('hidPlanServicio');
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

            hidGrupoPaquete.value += '{'

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
                       if ((intSecuenciaAct == 1) || (intSecuenciaAct == '<%= ConfigurationManager.AppSettings["constConf3PITelefonia"] %>')) {//|| intSecuenciaAct == 209) {
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

                    hidGrupoPaquete.value += ',[' + idFila + ']';
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

                        hidGrupoPaquete.value += ',[' + idFila + ']';

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

            hidPlanServicio.value += strPlanServicio;
            hidPlanServicioNo.value += strPlanServicioNo;

            hidGrupoPaquete.value += '}';

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
               var strPlazo = '|' + ddlPlazo.value + ";" + obtenerTextoSeleccionado(ddlPlazo);
               var ddlPlan = document.getElementById('ddlPlan' + idFila);
               var strPlanActual = '|' + ddlPlan.value + ";" + obtenerTextoSeleccionado(ddlPlan);
               var hidPlazoActual = document.getElementById('hidPlazoActual');
               var hidPaqueteActual = document.getElementById('hidPaqueteActual');
               var hidGrupoPaquete = document.getElementById('hidGrupoPaquete');
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
               var hidPlanServicio = document.getElementById('hidPlanServicio');
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

               hidGrupoPaquete.value += '{'
               //------
               if (parent.getValue('hidTipoOperacion') == 2 && parent.getValue('hidTipoOferta') == 01 && parent.getValue('hidModalidadVenta') == 2) {
                  
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
                       case "|" + '<%= ConfigurationManager.AppSettings["constConf3PIClaroTVDig"] %>' || "|" + '<%= ConfigurationManager.AppSettings["constConf3PIClaroTVAna"] %>':   //203 204
                           alert('<%= ConfigurationManager.AppSettings["constMsgNoClaroTvMigra"] %>');
                           break;

                       case "|" + '<%= ConfigurationManager.AppSettings["constConf3PITelefonia"] %>' + "|" + '<%= ConfigurationManager.AppSettings["constConf3PIInternet"] %>' || "|" + '<%= ConfigurationManager.AppSettings["constConf3PIInternet"] %>' + "|" + '<%= ConfigurationManager.AppSettings["constConf3PITelefonia"] %>':  //200+201 // 201+202
                           alert('<%= ConfigurationManager.AppSettings["constMsgNoTelefInterMigra"] %>');
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
                                   if ((intSecuenciaAct == 1) || (intSecuenciaAct == '<%= ConfigurationManager.AppSettings["constConf3PITelefonia"] %>')) {//|| intSecuenciaAct == 209) {
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

                                   hidGrupoPaquete.value += ',[' + idFila + ']';
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

                                       hidGrupoPaquete.value += ',[' + idFila + ']';

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

                           hidPlanServicio.value += strPlanServicio;
                           hidPlanServicioNo.value += strPlanServicioNo;

                           hidGrupoPaquete.value += '}';

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
            var strServicio = getValue('hidPlanServicio');
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
            var hidPromocion;
            var arrSrv = strSrvSel.split(';');

            if (!haciaTemp)
                hidPromocion = document.getElementById('hidPromocion');
            else
                hidPromocion = document.getElementById('hidPromocionTemp');

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

            hidPromocion.value += '*ID*' + idFila + strResultado;
        }

        function extraerCodigoServicio(idFila) {
            var strResultado = '';
            var strServicio = getValue('hidPlanServicio');
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
            var strServicio = getValue('hidPlanServicio');
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
            var hidPromocion = document.getElementById('hidPromocion');
            var strPromocion = hidPromocion.value;
            var strIdFila = '*ID*' + idFila;

            var intPosIni = strPromocion.indexOf(strIdFila);
            var intPosFin = 0;

            if (intPosIni > -1) {
                intPosFin = strPromocion.substring(intPosIni + 4).indexOf('*ID*');

                if (intPosFin == -1)
                    intPosFin = strPromocion.length;
                else
                    intPosFin += intPosIni + 4;

                hidPromocion.value = strPromocion.replace(strPromocion.substring(intPosIni, intPosFin), '');
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
                if (vCodigoTipoProductoActual != codTipoProd3Play && vCodigoTipoProductoActual != codTipoProd3PlayInalam) {                    
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
            self.frames['iframeAuxiliar'].location.replace(url);
        }

        function cambiarPlazo(strPlazo, idFila) {
            var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');
            var ddlPlan = document.getElementById('ddlPlan' + idFila);

            if (ddlPlan == null)
                ddlPlan = document.getElementById('ddlPaquete' + idFila);

            if (verificarCombo() && (codigoTipoProductoActual == codTipoProdDTH || codigoTipoProductoActual == codTipoProd3Play)) {
                verificarDireccion(idFila);
            }

            var strFlujo = flujoAlta;
            if (parent.getValue('hidTienePortabilidad') == 'S')
                strFlujo = flujoPortabilidad;

            var strTienePaquete = document.getElementById('hidTienePaquete').value;
            inicializarPlan(idFila);

           // if (codigoTipoProductoActual != codTipoProd3Play) {
              if (!(codigoTipoProductoActual == codTipoProd3Play || codigoTipoProductoActual == codTipoProd3PlayInalam)) {                
                if (getValue('hidPlanesCombo').length == 0)
                llenarDatosCombo(ddlPlan, '', true);
                else
                    return;

                llenarDatosCombo1(document.getElementById('divListaEquipo' + idFila), '', idFila, 'Equipo', false);

                calcularCFxProducto();
            }

            if (strPlazo.length > 0) {
                if (codigoTipoProductoActual == codTipoProd3Play || codigoTipoProductoActual == codTipoProd3PlayInalam) {                    
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

                    if ((codigoTipoProductoActual != codTipoProd3Play) && (codigoTipoProductoActual != codTipoProd3PlayInalam)) {
                        if (codigoTipoProductoActual == codTipoProdMovil) {
                            LlenarPaquetePlanIfr(idFila, strPlazo, strFlujo); //Movil y otros
                        }
                        else {
                            LlenarPlanIfr(idFila, strPlazo, strFlujo);
                        }
                    }
                }
                else
                    LlenarPlanIfr(idFila, strPlazo, strFlujo);

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
            url += '&strOficina=' + parent.getValue('hidOficina');
            url += '&strCasoEspecial=' + getValor(parent.getValue('ddlCasoEspecial'), 0);
            url += '&strRiesgo=' + parent.getValue('hidRiesgoDC');
            url += '&strNroDoc=' + parent.getValue('txtNroDoc');
            url += '&strCombo=' + parent.getValue('ddlCombo');
            url += '&strEvaluarSoloFijo=' + parent.getValue('hidEvaluarSoloFijo');
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
            url += '&strOficina=' + parent.getValue('hidOficina');
            url += '&strCasoEspecial=' + parent.getValue('ddlCasoEspecial');
            url += '&strRiesgo=' + parent.getValue('hidRiesgoDC');
            url += '&strNroDoc=' + parent.getValue('txtNroDoc');
            url += '&strCampana=' + getValue('ddlCampana' + idFila);
            url += '&strCombo=' + parent.getValue('ddlCombo');
            url += '&strMetodo=' + 'LlenarPaquetePlan';

            self.frames['iframeAuxiliar'].location.replace(url);
        }

        function cambiarCampana(idFila, strValor) {
            cambiarPlazo('', idFila);
            var flag = true;
            var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
            if (ddlPlazo.disabled) {
                cambiarPlazo(getValue('hidPlazoActual').split('_')[0], idFila) 
                return;
            }

            var strPlazosDTH = getValue('hidPlazosDTH');
            var strPlazosHFC = getValue('hidPlazosHFC');
            if (parent.getValue('ddlModalidadVenta') == codModalidadChipSuelto && parent.getValue('hidTienePortabilidad') == 'S') {
                var strPlazosMovil = "";
                var PlazosMovil = getValue('hidPlazosMovil');
                var arrPlazosMovil = PlazosMovil.split('|');
                if (strValor == '<%= ConfigurationManager.AppSettings["constCampanhaRegresoACasa"] %>') {

                    if (arrPlazosMovil.length == 3) {
                        
                        strPlazosMovil =  " " + '|' + arrPlazosMovil[2];
                        
                     }
                    else{
                        for (var i = arrPlazosMovil.length-1; i > 1; i--) {
                              strPlazosMovil += arrPlazosMovil[i] + '|';

                          }
                     
                      }
                      flag = false;   
                }
                else{
                    strPlazosMovil = getValue('hidPlazosMovil').split('|')[1] + '|' + " ";
                    flag = true;   
                }
            }
            else {
                strPlazosMovil = getValue('hidPlazosMovil');
            }
            var strPlazosFijo = getValue('hidPlazosFijo');
            var strPlazosBAM = getValue('hidPlazosBAM');
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
            if (ddlPlazo.length == 2) {
                ddlPlazo.selectedIndex = 1;
                cambiarPlazo(ddlPlazo.value, idFila);
            }
        }

        function LlenarMaterialIfr(idFila, strCampana, strPlan, strCombo, strProducto) {
            cargarImagenEsperando();

            var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
            url += 'idFila=' + idFila;
            url += '&strCampana=' + strCampana;
            url += '&strPlan=' + strPlan;
            url += '&strOficina=' + parent.getValue('hidOficina');
            url += '&nroDoc=' + parent.getValue('txtNroDoc');
            url += '&strOrgVenta=' + parent.getValue('hidOrgVenta');
            url += '&strCentro=' + parent.getValue('hidCentro');
            url += '&strFlagPorta=' + parent.getValue('hidTienePortabilidad');
            url += '&strCombo=' + strCombo;
            url += '&strTipoProducto=' + strProducto; 
            url += '&strTipoOperacion=' + parent.getValue('ddlTipoOperacion');
            url += '&strMetodo=' + 'LlenarMaterial';

            self.frames['iframeAuxiliar'].location.replace(url);
        }

        function LlenarEquipoPrecioIfr(idFila, strPlan, strPlanSisact, strPlazo, strCampana, strEquipo) {
            cargarImagenEsperando();

            var strCuota = '';
            if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota) {
                var arrCuota = obtenerCuotaValores(idFila);
                if (arrCuota != null) {
                    strCuota = arrCuota[0].split('_')[0];
                }
            }

            var url = '../frames/sisact_ifr_consulta_unificada.aspx?';
            url += 'idFila=' + idFila;
            url += '&strOficina=' + parent.getValue('hidOficina');
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
            url += '&strMetodo=' + "LlenarEquipoPrecio";

            self.frames['iframeAuxiliar'].location.replace(url);
        }

        function asignarPrecio(idFila, strValor) {
            if (parent.getValue('ddlModalidadVenta') == '<%= ConfigurationManager.AppSettings["constCodModalidadCuota"] %>')
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
                        if (getValue('ddlCampana' + idFila) == '<%= ConfigurationManager.AppSettings["CampanaDiaEnamorados"] %>' && getValue('hidTipoProductoActual') == 'Movil')
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

        function agregarFila1(booVeriConf) {
            if (agregarFila(booVeriConf, 0, false) != false) {
                var idFila = getValue('hidTotalLineas');
                cambiarTipoProducto(idFila);

                // Validación Modalidad / Operador Cedente
                if (parent.getValue('hidTienePortabilidad') == 'S' && parent.getValue('ddlOperadorCedente') == "")
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
            var strPorcentajeCuotas = '100';
            var strMontoCuota = 0;
            var strEquipoEnCuotas = '';
            var strPrecioLista = '';
            var strPrecioVenta = '';
            var strNroTelefono = '';
            var strServicio = '';
            var strServicioTexto = '';
            var strTopeConsumo = '';
            var strTopeConsumoFijo = '';
            var strTopeConsumoTexto = '';
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
                if (ddlPlazo != null) {
                    strPlazo = ddlPlazo.value;
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
                     if (codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam) {
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
                        strPorcentajeCuotas = arrCuota[1];
                        strMontoCuota = arrCuota[3];
                        strEquipoEnCuotas = 'S';

                    var strCasoEpecial = getValor(parent.getValue('ddlCasoEspecial'), 0);
                    var blnCasoEspecialCMA = (strCasoEpecial == '<%= ConfigurationManager.AppSettings["constCETrabajadoresCMA"] %>');
                    if (!blnCasoEspecialCMA)
                            strMontoCuota = arrCuota[3];

                    strCFTotMensual = parseFloat(strCFTotMensual) + parseFloat(strMontoCuota);
                }
                }

                var txtNroTelefono = document.getElementById('txtNroTelefono' + idFila);
                if (txtNroTelefono != null)
                    strNroTelefono = txtNroTelefono.value;
                if (codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam) {
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
                if (ddlTopeConsumo != null) {
                    if (ddlTopeConsumo.style.display != 'none') {
                        strTopeConsumoFijo = ddlTopeConsumo.value;
                        strTopeConsumoTexto = obtenerTextoSeleccionado(ddlTopeConsumo);
                    }
                }

                if (idx != '') {
                    strCadena = ''; 
                }

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
                    strCadena += strTopeConsumoTexto + '|';

                if (idx != '' && idx == idFila) {
                    return strCadena;
                }
            }

            strCadena = strCadena + strCadenaDetalle;
            return strCadena;
        }

        function obtenerPaqueteActual(idFila) {
            var strGrupoPaquete = getValue('hidGrupoPaquete');
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
            var strPlanServicio = document.getElementById('hidPlanServicio').value;
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
                    if (codServicio == topeConsumoCero || codServicio == topeConsumoSinCF || codServicio == topeConsumoAuto) {
                        servicio = codServicio;
                        break;
                    }
                }
            }

            return servicio;
        }

        function obtenerCuotaValores(idFila, strCadenaCuota) {
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

            var idFila, idProducto, tipoProducto, nroSec;

            var arrCadena = strCadena.split('#');

            var listaGeneral = arrCadena[0];
            arrGeneral = listaGeneral.split('©');

            parent.document.getElementById('ddlTipoOperacion').value = arrGeneral[1];
            parent.document.getElementById('ddlOferta').value = arrGeneral[2];
            parent.document.getElementById('hidTipoOferta').value = arrGeneral[2];
            parent.document.getElementById('ddlModalidadVenta').value = arrGeneral[5];
            parent.document.getElementById('hidListaTipoProducto').value = arrGeneral[6].split('¬')[0];
            document.getElementById('hidTotalLineas').value = arrGeneral[7];
            document.getElementById('hidGrupoPaquete').value = arrGeneral[8];

            var ddlCasoEspecial = parent.document.getElementById('ddlCasoEspecial');
            var ddlCombo = parent.document.getElementById('ddlCombo');

            llenarDatosCombo(ddlCasoEspecial, arrGeneral[6].split('¬')[1], true);
            llenarDatosCombo(ddlCombo, arrGeneral[6].split('¬')[2], true);

            if (parent.getValue('ddlOferta') == '<%= ConfigurationManager.AppSettings["TipoProductoBusiness"] %>') {
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
            document.getElementById('hidPlanServicio').value = listaServicio;

            for (var i = 0; i < arrPlan.length; i++) {
                if (arrPlan[i] != '') {
                    var arrPlanDet = arrPlan[i].split('~');

                    idFila = arrPlanDet[0];
                    idProducto = arrPlanDet[1];

//                    //gaa20160211
//                    var strCampanaCodigo = arrPlanDet[2].split(';')[0];
//                    if (strCampanaCodigo == '<%= ConfigurationManager.AppSettings["CampanaDiaEnamorados"] %>' ||
//                        strCampanaCodigo == '<%= ConfigurationManager.AppSettings["CampanaDiaEnamoradosAsociada"] %>')
//                        continue;
//                    //fin gaa20160211

                    switch (idProducto) {
                        case codTipoProdMovil: tipoProducto = 'Movil'; break;
                        case codTipoProdFijo: tipoProducto = 'Fijo'; break;
                        case codTipoProdBAM: tipoProducto = 'BAM'; break;
                        case codTipoProdDTH: tipoProducto = 'DTH'; break;
                        case codTipoProd3Play: tipoProducto = 'HFC'; break;
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

            hidCodigoTipoProductoActual.value = codTipoProd3PlayInalam;
            hidTipoProductoActual.value = '3PlayInalam';
            guardarItem();
            agregarCarrito(false);
           
            hidCodigoTipoProductoActual.value = codTipoProdMovil;
            hidTipoProductoActual.value = 'Movil';

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

                if (ddlPlazo.value.length > 0)
                    return '|' + ddlPlazo.value + ';' + obtenerTextoSeleccionado(ddlPlazo);
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

            if (getValue('hidTieneCuotas') == 'S')
                nCell++;

            if (parent.getValue('hidTienePortabilidad') == 'S')
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
            var strPlanServicio = document.getElementById('hidPlanServicio').value;
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
                    if (codServSelected[3] == '<%= ConfigurationManager.AppSettings["codServRoamingI"] %>') {
                        document.getElementById('tblRoamingI').style.display = 'inline';
                        if (codServSelected[0] == '0') {
                            if (codServSelected[5] == '<%= ConfigurationManager.AppSettings["codPlazoDeterminado"] %>') {
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
                            if (codServSelected[5] == '<%= ConfigurationManager.AppSettings["codPlazoDeterminado"] %>') {
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
                strPromocion = document.getElementById('hidPromocion').value;
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
            if (document.getElementById('hidValidarGuardarCuota').value.length > 0) {
                alert('Debe guardar las cuotas antes de ejecutar esta acción');
                return;
            }

            var strPlanServicio = document.getElementById('hidPlanServicio').value;
            var strPlanServicioNo = document.getElementById('hidPlanServicioNo').value;
            var ddlPlan = document.getElementById('ddlPlan' + idFila);
            var ddlServicio = document.getElementById('ddlServicio' + idFila);
            var strPlan = ddlPlan.value;
            var ddlCampana = document.getElementById('ddlCampana' + idFila);

            //- Oculta y resetear Tabla para Servicio Roaming
            document.getElementById('tblRoamingI').style.display = 'none';
            reseteartblRoaming();

            if (strPlan.length > 0) {
                document.getElementById('tblServicios').style.display = 'inline';
                  if ((getValue('hidCodigoTipoProductoActual') == codTipoProd3Play) || (getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam)) {
                    var strIdProducto = ddlPlan.value.split('.')[1];
                    if (strIdProducto == '<%= ConfigurationManager.AppSettings["IdProductoServicioEmail"] %>') {
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
                cerrarEquipo();
            }
            else
                cerrarServicio();

            parent.autoSizeIframe();
        }

        function mostrarServicioGuardado(idFila) {
            var strPlan = getValue('hidCodPlan1');
            cerrarCuota();
            document.getElementById('tblServicios').style.display = 'none';

            var strPlanServicio = document.getElementById('hidPlanServicio').value;
            if (strPlanServicio.indexOf('*ID*' + idFila) > -1) {
                llenarServicio(idFila);
                document.getElementById('tblServicios').style.display = 'inline';
            }

            document.getElementById('lblIdLista').innerHTML = obtenerTextoSeleccionado(document.getElementById('ddlPlan' + idFila));
            if ((getValue('hidCodigoTipoProductoActual') == codTipoProd3Play)|| (getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam)) {
                var ddlPlan = document.getElementById('ddlPlan' + idFila);
                var strPlan = ddlPlan.value;
                var strIdProducto = ddlPlan.value.split('.')[1];
                if (strIdProducto == '<%= ConfigurationManager.AppSettings["IdProductoServicioEmail"] %>') {
                    document.getElementById('tblServicios').style.display = 'none';
                    return;
                }

                document.getElementById('tblServicios').style.display = 'inline';
            }

            habilitarServicio(true);
            habilitarCuota(idFila);

            parent.autoSizeIframe();
        }

        function agregarServicio() {
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

            for (var i = 0; i < contSD; i++) {
                if (lbxSD.options[i].selected) {
                    var option = document.createElement('option');
                    option.value = lbxSD.options[i].value;
                    option.text = lbxSD.options[i].text;
                    lbxSA.options[contSA] = option;

                    arrCodigo = lbxSA.options[contSA].value.split('_');
                    strGrupo = arrCodigo[1];

                    contSA++;
                    lbxSD.remove(i);
                    i--;
                    contSD--;

                    asignarPromocion3PlayTemp(linea);

                }
            }

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

            if (strGrupoResultado.length > 0)
                strGrupoResultado += '{/' + strGrupo2 + '}';

            strPlanServicioNGTemp = strPlanServicioNGTemp.replace(strFinLinea, strGrupoResultado + strFinLinea);
            hidPlanServicioNGTemp.value = strPlanServicioNGTemp;

            lbxSD.selectedIndex = -1;
            lbxSA.selectedIndex = -1;
        }

        function quitarServicio() {
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

            for (var i = 0; i < contSA; i++) {
                if (lbxSA.options[i].selected) {
                    arrCodigo = lbxSA.options[i].value.split('_');
                    var codServSelected = arrCodigo[3];
                    if (codServSelected == '<%= ConfigurationManager.AppSettings["codServRoamingI"] %>') {
                        document.getElementById('tblRoamingI').style.display = 'none';
                        reseteartblRoaming();
                    }
                    //Si SELECCIONADO_OBLIGATORIO es igual a vacio
                    //if (arrCodigo[2].length == 0)
                    //Si SELECCIONADO_OBLIGATORIO no aparece
                    if (arrCodigo[2] != '2') {
                        var option = document.createElement('option');
                        option.value = lbxSA.options[i].value;
                        option.text = lbxSA.options[i].text;
                        lbxSD.options[contSD] = option;

                        strGrupo = arrCodigo[1];

                        lbxSA.remove(i);
                        contSD++;
                        i--;
                        contSA--;

                        //asignarPromocion3PlayTemp(linea);
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
        }

        function resetServicio() {
            var idFila = document.getElementById('hidLineaActual').value;
            var strPlan = document.getElementById('ddlPlan' + idFila).value;

            if (strPlan.length > 0) {
                var lbxSA = document.getElementById('lbxServiciosAgregados1');
                var contSA = lbxSA.options.length;

                for (var i = 0; i < contSA; i++) {
                    arrCodigo = lbxSA.options[i].value.split('_');

                    if (arrCodigo[2] != '2' && arrCodigo[2] != '3') {
                        lbxSA.options[i].selected = true;

                        quitarServicio();
                        contSA--;
                        i--;
                    }
                }
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
            if (fec.getDate() < 10) {
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

        function guardarServicio() {
            if (document.getElementById('btnCerrarServicios').value == 'Cerrar') {
                cerrarServicio()
                parent.autoSizeIframe();
                return;
            }

            var hidPlanServicio = document.getElementById('hidPlanServicio');
            var hidPlanServicioNo = document.getElementById('hidPlanServicioNo');
            var lbxSD = document.getElementById('lbxServiciosDisponibles1');
            var lbxSA = document.getElementById('lbxServiciosAgregados1');
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

            for (var i = 0; i < contSA; i++) {
                strCodSA = lbxSA.options[i].value;
                arrCodSA = strCodSA.split('_');

                //Verificar si el servicio es Roaming Internacional

                var codServSelected = arrCodSA[3];
                if (codServSelected == '<%= ConfigurationManager.AppSettings["codServRoamingI"] %>') {
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

                if (arrCodSA[3] == topeConsumoAuto) {
                    var strPlanCodigo = getValor(document.getElementById('ddlPlan' + linea).value, 0);
                    LlenarMontoTopeConsumoIfr(linea, strPlanCodigo);
                }
            }
            for (var i = 0; i < contSD; i++) {
                strPlanServicioNo += '|' + lbxSD.options[i].value + ';' + lbxSD.options[i].text
            }

            hidPlanServicio.value += strPlanServicio;
            hidPlanServicioNo.value += strPlanServicioNo;

            cerrarServicio();
            borrarGrupoServicio(linea);
            guardarGrupo(linea);

            hidMontoServicios.value = totalServicios;

            calcularCFxProducto();
            if ((getValue('hidCodigoTipoProductoActual') == codTipoProd3Play) || (getValue('hidCodigoTipoProductoActual') == codTipoProd3PlayInalam))
                asignarPromocion3Play(linea);

            //document.getElementById('lbxPromocionesDisponibles').innerHTML = "";
            //document.getElementById('lbxPromocionesSeleccionadas').innerHTML = "";
            document.getElementById('lbxServiciosDisponibles1').selectedIndex = -1;
            document.getElementById('lbxServiciosAgregados1').selectedIndex = -1;

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
            url += '&strPlan=' + strPlan;
            url += '&strNroDoc=' + parent.getValue('txtNroDoc'); 
            url += '&strMetodo=' + 'LlenarMontoTopeConsumo';

            self.frames['iframeAuxiliar'].location.replace(url);
        }

        function borrarServicio(idFila) {
            var hidPlanServicio = document.getElementById('hidPlanServicio');
            var hidPlanServicioNo = document.getElementById('hidPlanServicioNo');
            var strPlanServicio = '*ID*' + idFila;

            var intPosIni = hidPlanServicio.value.indexOf(strPlanServicio);
            var intPosFin = 0;

            if (intPosIni > -1) {
                intPosFin = hidPlanServicio.value.substring(intPosIni + 4).indexOf('*ID*');

                if (intPosFin == -1)
                    intPosFin = hidPlanServicio.value.length;
                else
                    intPosFin += intPosIni + 4;

                hidPlanServicio.value = hidPlanServicio.value.replace(hidPlanServicio.value.substring(intPosIni, intPosFin), '');
            }

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
                if (intPosIni > -1) {
                    intPosIni += strId.length;
                    strCuota = strCuotas.substring(intPosIni);
                    intPosFin = strCuota.indexOf(';') + intPosIni;
                    strCuota = strCuotas.substring(intPosIni, intPosFin);

                    strNroCuotaActual = strCuota;
                }

                cambiarCuota(strNroCuotaActual);
            }

            quitarImagenEsperando();
            autoSizeIframe();
        }

        function mostrarCuota(idFila) {
            if (document.getElementById('hidValidarGuardarCuota').value.length > 0) {
                alert('Debe guardar las cuotas antes de ejecutar esta acción');
                return;
            }

            document.getElementById('tblCuotas').style.display = 'none';

            if (document.getElementById('hidValorEquipo' + idFila).value.length == 0) {
                alert('Para seleccionar cuotas debe escoger un equipo');
                return;
            }

            document.getElementById('hidLineaActual').value = idFila;
            document.getElementById('tblCuotas').style.display = 'inline';
            cerrarServicio();
            cerrarEquipo();

            habilitarCuota(idFila);

            var strCadenaDetalle = consultarItem(idFila);
            parent.consultaReglasCreditosCuotas(idFila, strCadenaDetalle);
        }

        function mostrarCuotaGuardada(idFila) {
            document.getElementById('tblServicios').style.display = 'none';

            if (document.getElementById('hidCuota').value.indexOf('*ID' + idFila + '*') > -1) {
                document.getElementById('tblCuotas').style.display = 'inline';
                obtenerCuota(idFila);
                document.getElementById('hidLineaActual').value = idFila;
                habilitarCuota(idFila);
            }
            else
                document.getElementById('tblCuotas').style.display = 'none';

            autoSizeIframe();
        }

        function cambiarCuota(strCuota) {
            var idFila = document.getElementById('hidLineaActual').value;

            var txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
            var hidListaPrecio = document.getElementById('hidListaPrecio' + idFila);
            var txtPorcCuotaIni = document.getElementById('txtPorcCuotaIni');
            var txtMontoCuotaIni = document.getElementById('txtMontoCuotaIni');
            var txtMontoCuota = document.getElementById('txtMontoCuota');
            var ddlNroCuotas = document.getElementById('ddlNroCuotas');
            var fltPrecio;
            var fltMontoCuotaIni;
            var strListaPrecio;

            txtEquipoPrecio.value = '';
            txtPorcCuotaIni.value = '';
            txtMontoCuotaIni.value = '';
            txtMontoCuota.value = '';

            if (strCuota != '') {
                var idCuota = getValor(strCuota, 0);
                var intNroCuotas = parseFloat(idCuota);
                var fltPorcCuotaIni = parseFloat(getValor(strCuota, 1));

                strListaPrecio =  document.getElementById('hidListaPrecio' + idFila).value;

                if (strListaPrecio == '') {
                    ddlNroCuotas.value = '';
                    alert('<%= ConfigurationManager.AppSettings["constMsjErrorConfigListaPrecio"] %>');
                    return;
                }

                hidListaPrecio.value = strListaPrecio;
                fltPrecio = strListaPrecio.split('_')[0];
                txtEquipoPrecio.value = fltPrecio;

                if (seleccionarCuota(strCuota)) {
                    txtPorcCuotaIni.value = fltPorcCuotaIni;
                    fltMontoCuotaIni = (fltPrecio * fltPorcCuotaIni / 100).toFixed(2);
                    txtMontoCuotaIni.value = fltMontoCuotaIni;
                    txtMontoCuota.value = ((fltPrecio - fltMontoCuotaIni) / intNroCuotas).toFixed(2);
                }

                document.getElementById('hidValidarGuardarCuota').value = 'S';
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
            document.getElementById('tblCuotas').style.display = 'none';
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

            if (intPosIni > -1) {
                intPosFin = strCuotas.indexOf(strValFin);

                strCuota = strCuotas.substring(intPosIni, intPosFin + strValFin.length);

                hidCuota.value = strCuotas.replace(strCuota, '');

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

        function guardarCuota() {
            var ddlNroCuotas = document.getElementById('ddlNroCuotas');
            if (ddlNroCuotas.value.length == 0) {
                ddlNroCuotas.focus();
                alert('Debe seleccionar una cuota');
                return false;
            }

            document.getElementById('hidValidarGuardarCuota').value = '';

            cerrarCuota();

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

            var strCasoEpecial = getValor(parent.getValue('ddlCasoEspecial'), 0);
            var blnCasoEspecialCMA = (strCasoEpecial == '<%= ConfigurationManager.AppSettings["constCETrabajadoresCMA"] %>');
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
            var planBase = '<%= ConfigurationManager.AppSettings["ConstPlanSmart25P"] %>';
            var strCodPlanBscs = planBase.split('|')[0];
            var strCodPlanSisact = parseInt(planBase.split('|')[1], 10);
            var intNroMaxPlanes = parseInt(planBase.split('|')[2], 10);

            if (strCodPlanSisact == idPlanxAgregar) {
                nroPlanesTotal = 0;
                nroPlanesTotal = parseInt(getNroPlanesActivos(strCodPlanBscs), 10) + parseInt(getNroPlanesAgregados(strCodPlanSisact), 10) + 1;

                if (nroPlanesTotal > intNroMaxPlanes) {
                    alert('<%= ConfigurationManager.AppSettings["ConstMensajeExcedePlanes"] %>');
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
            var CE_BBEnamorados = '<%= ConfigurationManager.AppSettings["ConstCasoEspBBEnamorados"] %>';
            var CE_SmartEnamorados = '<%= ConfigurationManager.AppSettings["ConstCasoEspSmartEnamorados"] %>';

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
            var strPlanesClaroExacto = '<%= ConfigurationManager.AppSettings["PlanesClaroExactoIlimitado"] %>';
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
            var strPlanesModemCIlimitado = '<%= ConfigurationManager.AppSettings["PlanesModemCIlimitado"] %>';
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
            var strListaPlanesRPC = '<%= ConfigurationManager.AppSettings["constCodPlanesRPC"] %>';
            if (validarExclusionPlanes(idPlan, strListaPlanesRPC)) {
                alert("Los planes Exacto RPC6 o Exato RPC12 no pueden ser combinados con otros");
                return false;
            }
            //2Play Corporativo
            var strListaPlanes2Play = '<%=ConfigurationManager.AppSettings["CodPlanes2PlayCorporativo"] %>';
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

        function window.confirm(str) {
            execScript('n = msgbox("' + str + '","4132")', "vbscript");
            return (n == 6);
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
                divLista.style.display = 'inline';
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
                document.getElementById(txtTexto).style.color = '<%= ConfigurationManager.AppSettings["BloqueoEquipoSinStockColor"] %>';
                alert('<%= ConfigurationManager.AppSettings["constMsjEquipoSinStockSel"] %>');
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

            var strEquipo = document.getElementById('hidValorEquipo' + idFila).value;
            var codigoTipoProductoActual = getValue('hidCodigoTipoProductoActual');

            if (strEquipo.length == 0) {
                inicializarEquipo(idFila);
                return;
            }

            if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota) {
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

            if (ddlPlazo != null)
                strPlazo = ddlPlazo.value;

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
                    LlenarEquipoPrecioIfr(idFila, strPlan, strPlanSisact, strPlazo, strCampana, strEquipo);
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
                                divOpcion.style.color = '<%= ConfigurationManager.AppSettings["BloqueoEquipoSinStockColor"] %>';
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

            if (codigoTipoProductoActual == codTipoProd3PlayInalam) {
                tabla = document.getElementById('tblTabla3PlayInalam');
            };
           
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

            for (var i = 0; i < cont; i++) {
                nCell = 1; //0: Imagen Confirmar, 1:Imagen Eliminar

                fila = tabla.rows[i];

                strTipoProducto = getValue('hidCodigoTipoProductoActual');

                //if (strTipoProducto == codTipoProd3Play) {
                if (strTipoProducto == codTipoProd3Play || strTipoProducto == codTipoProd3PlayInalam) {
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
            }

            return strResultado;
        }

        function obtenerCadenaDetOfertServ(idFila) {
            var strPlanServicio = document.getElementById('hidPlanServicio').value;
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
                    strGrupo = arrServicioCodigo[7];
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
            var strPromocion = getValue('hidPromocion');
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
            tblTablaTituloVentaVarios.style.display = 'none';
            tblTablaTitulo3PlayInalam.style.display = 'none';
            tblTablaMovil.style.display = 'none';
            tblTablaFijo.style.display = 'none';
            tblTablaBAM.style.display = 'none';
            tblTablaDTH.style.display = 'none';
            tblTablaHFC.style.display = 'none';
            tblTablaVentaVarios.style.display = 'none';
            tblTabla3PlayInalam.style.display = 'none';
            

        }

        function mostrarTablaActivo(tipoProducto) {
            document.getElementById('tblTablaTitulo' + tipoProducto).style.display = '';
            document.getElementById('tblTabla' + tipoProducto).style.display = '';
        }

        function verificarCondicServRI() {
            //Validar Si es CAC / DAC / Corner
            var strConstCanalCAC = '<%= ConfigurationManager.AppSettings["constRoamingCAC"] %>';
            var strConstCanalDAC = '<%= ConfigurationManager.AppSettings["constRoamingDAC"] %>';
            var strConstCanalCorner = '<%= ConfigurationManager.AppSettings["constRoamingCorner"] %>';

            //Validar Si es MASIVO / B2E / Corporativo
            var strConstMasivo = '<%= ConfigurationManager.AppSettings["constRoamingMasivo"] %>';
            var strConstB2E = '<%= ConfigurationManager.AppSettings["constRoamingB2E"] %>';
            var strConstCorporativo = '<%= ConfigurationManager.AppSettings["constRoamingCorporativo"] %>';

            if (parent.getValue('hidCanal') == strConstCanalCAC || parent.getValue('hidCanal') == strConstCanalDAC || parent.getValue('hidCanal') == strConstCanalCorner) {
                if (parent.getValue('ddlOferta') == strConstMasivo || parent.getValue('ddlOferta') == strConstB2E || parent.getValue('ddlOferta') == strConstCorporativo) {
                    //Activar Flag para mostrar Roaming Internacional
                    parent.setValue('hidFlagRoamingI', '1');
                }
            }
        }

        function f_MostrarTab(tipoProducto) {
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
            if ((tipoProducto == 'Fijo') || (tipoProducto == 'HFC') || (tipoProducto =='3PlayInalam'))
                calcularLCxProductoFijo();
            else
                calcularLCxProducto(idProducto);

            calcularCFxProducto();
        }

        function calcularLCxProducto(idProducto) {
            var strLCDisponiblexProd = parent.document.getElementById('hidLCDisponiblexProd').value;
            var arrLCDisponiblexProd = strLCDisponiblexProd.split('|');

            for (var i = 0; i < arrLCDisponiblexProd.length; i++) {
                var array = arrLCDisponiblexProd[i].split(';');
                if (idProducto == array[0])
                    parent.document.getElementById('txtLCDisponiblexProd').value = parseFloat(array[1]).toFixed(2);
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
            var strServicio = getValue('hidPlanServicio');
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
            if (ddlCombo.value.length > 0)
                strCombo = obtenerTextoSeleccionado(ddlCombo);
            
            if (tipoProductoVisual == 'HFC')
                tipoProductoVisual = '3 Play';

             if (tipoProductoVisual == '3PlayInalam')
                 tipoProductoVisual = '3 Play Inalambrico';

            if (tipoProductoVisual == 'DTH')
                tipoProductoVisual = 'Claro TV SAT';

            if (tipoProductoVisual == 'FIJO' || tipoProductoVisual == 'Fijo')
                tipoProductoVisual = 'FIJO INALAMBRICO';

            if (tipoProductoVisual == 'VENTAVARIOS')
                tipoProductoVisual = 'VENTA VARIOS';

            if (parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuota) {
                tablaResumen.rows[0].cells[8].style.display = 'none';
                tablaResumen.rows[0].cells[9].style.display = 'none';
                tablaResumen.rows[0].cells[10].style.display = 'none';
            }
            else {
                tablaResumen.rows[0].cells[8].style.display = '';
                tablaResumen.rows[0].cells[9].style.display = '';
                tablaResumen.rows[0].cells[10].style.display = '';
            }

            tablaResumen.rows[0].cells[11].style.display = '';
            if (parent.getValue('ddlModalidadVenta') != codModalidadContratoCede) {
                tablaResumen.rows[0].cells[11].style.display = 'none';
            }

            if (strCombo.length == 0)
                tablaResumen.rows[0].cells[2].style.display = 'none';
            else
                tablaResumen.rows[0].cells[2].style.display = '';

            for (var i = 0; i < cont; i++) {
                fila = tabla.rows[i];
                blnAgregarItem = true;

                if (fila.style.display != 'none') {

                    idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(10);

                    if (idFila.length == 0)
                        idFila = fila.cells[2].getElementsByTagName("select")[0].id.substring(8);

					strPlazo = '&nbsp;';
                    ddlPlazo = document.getElementById('ddlPlazo' + idFila);
                    if (ddlPlazo != null)
                        strPlazo = obtenerTextoSeleccionado(ddlPlazo);

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

                    strPrecioVentaEquipo = "&nbsp;";
                    txtEquipoPrecio = document.getElementById('txtEquipoPrecio' + idFila);
                    if (txtEquipoPrecio != null)
                        strPrecioVentaEquipo = (txtEquipoPrecio.value * 1).toFixed(2);

                    if (strPrecioVentaEquipo == '') strPrecioVentaEquipo = "&nbsp;";

                    if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota) {
                        arrCuota = obtenerCuotaValores(idFila);
                        strNroCuotas = parseInt(arrCuota[0]);
                        var porcenCuotaIni = parseInt(arrCuota[1]);
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

                    if (codigoTipoProducto == codTipoProd3Play || codigoTipoProducto == codTipoProd3PlayInalam) {
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

                    if (blnAgregarItem) {
                        filaCarrito = obtenerFilaCarrito(idFila);

                        if (filaCarrito == null) //Si es una fila nueva
                        {
                            newRow = tablaResumen.insertRow();

                            oCell = newRow.insertCell();
                            oCell.align = 'center';
                            oCell.innerHTML = tipoProductoVisual + "<input type='hidden' value=" + idFila + "><input type='hidden' value=" + tipoProducto + ">";
                            oCell.className = 'TablaFilasGrid';

                            oCell = newRow.insertCell();
                            oCell.align = 'center';
                            oCell.innerHTML = strPlazo;
                            oCell.className = 'TablaFilasGrid';

                            oCell = newRow.insertCell();
                            oCell.align = 'center';
                            oCell.innerHTML = strCombo;
                            oCell.className = 'TablaFilasGrid';
                            
                            if (strCombo.length == 0)
                                oCell.style.display = 'none';
                            else
                                oCell.style.display = '';

                            oCell = newRow.insertCell();
                            oCell.align = 'center';
                            oCell.innerHTML = strPlan;
                            oCell.className = 'TablaFilasGrid';

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
                                    oCell.style.color = '<%= ConfigurationManager.AppSettings["BloqueoEquipoSinStockColor"] %>';
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

                            oCell = newRow.insertCell();
                            oCell.align = 'center';
                            oCell.innerHTML = strEquipoEnCuotas;
                            oCell.className = 'TablaFilasGrid';

                            if (parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuota)
                                oCell.style.display = 'none';
                            else
                                oCell.style.display = '';

                            oCell = newRow.insertCell();
                            oCell.align = 'center';
                            oCell.innerHTML = strNroCuotas;
                            oCell.className = 'TablaFilasGrid';

                            if (parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuota)
                                oCell.style.display = 'none';
                            else
                                oCell.style.display = '';

                            oCell = newRow.insertCell();
                            oCell.align = 'center';
                            oCell.innerHTML = strMontoCuota;
                            oCell.className = 'TablaFilasGrid';

                            if (parent.getValue('ddlModalidadVenta') != codModalidadPagoEnCuota)
                                oCell.style.display = 'none';
                            else
                                oCell.style.display = '';

                            oCell = newRow.insertCell();
                            oCell.align = 'center';
                            oCell.innerHTML = "&nbsp;";
                            oCell.className = 'TablaFilasGrid';

                            if (parent.getValue('ddlModalidadVenta') != codModalidadContratoCede)
                                oCell.style.display = 'none';
                            else
                                oCell.style.display = '';

                            oCell = newRow.insertCell();
                            oCell.align = 'center';
                            oCell.innerHTML = "&nbsp;";
                            oCell.className = 'TablaFilasGrid';

                            oCell = newRow.insertCell();
                            oCell.align = 'center';
                            oCell.innerHTML = "<img src='../../Imagenes/editar.gif' border='0' style='cursor:hand' alt='Editar Fila' onclick='editarFilaCompra(" + idFila + ")' />";
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
                            filaCarrito.cells[8].innerHTML = strEquipoEnCuotas;
                            filaCarrito.cells[9].innerHTML = strNroCuotas;
                            filaCarrito.cells[10].innerHTML = strMontoCuota;
                            filaCarrito.cells[11].innerHTML = '&nbsp;';
                            filaCarrito.cells[12].innerHTML = '&nbsp;';
                        }
                    }
                    fila.style.display = 'none';
                }

                calcularCFCarrito();
            }

            if (mostrar)
                trResumenCompras.style.display = '';

            document.getElementById('txtCFTotal').value = 0;
        }

        function cambiarLucesCarrito(strPlanAutonomia) {
            var arrPlanAutonomia = strPlanAutonomia.split('|');
            var idFila;
            var flgAutonomia;
            var costoInstalacion;
            for (var x = 0; x < arrPlanAutonomia.length; x++) {
                if (arrPlanAutonomia[x] != '') {
                    idFila = arrPlanAutonomia[x].split(';')[0];
                    flgAutonomia = arrPlanAutonomia[x].split(';')[1];
                    costoInstalacion = arrPlanAutonomia[x].split(';')[2];
                    var imagen = '&nbsp;';

                    // Cambio Temporal
                    if (parent.getValue('ddlCanal') != '<%=  ConfigurationManager.AppSettings["constCodTipoOficinaCorner"] %>') {
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
                                if (filaCarrito.cells[0].innerText == 'Claro TV SAT' || filaCarrito.cells[0].innerText == '3 Play')
                                    filaCarrito.cells[11].innerHTML = costoInstalacion;
                                filaCarrito.cells[12].innerHTML = imagen;
                            }
                        }
                    }
                }
            }
        }

        function tieneProductosFueraCarrito() {
            var fila;

            var tablaMovil = tblTablaMovil;
            var tablaFijo = tblTablaFijo;
            var tablaBAM = tblTablaBAM;
            var tablaDTH = tblTablaDTH;
            var tablaHFC = tblTablaHFC;
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
            if ((codigoTipoProductoActual != codTipoProd3Play) && (codigoTipoProductoActual != codTipoProd3PlayInalam)) {
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

            if (txtCFPlanServicio   != null)
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

        function editarFilaCompra(idFila) {
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

                            tablaResumen.deleteRow(i);
                            eliminarItem(arrIdFilas[k]);
                            cont--;
                            i--;
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
            calcularCFxProducto();
            evaluar();
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

                            tablaResumen.deleteRow(i);

                            cont--;
                            i--;
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
            }
            else {
                setValue('hidFlgOrigen', 'EDIT');
                parent.consultaReglasCreditos();
            }
        }

        function obtenerFilasGrupo(idFila, valorReemplazoGrupo, booQuitarCorchetes) {
            var hidGrupoPaquete = document.getElementById('hidGrupoPaquete');
            var strGrupoPaquete = hidGrupoPaquete.value;
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

                hidGrupoPaquete.value = strGrupoPaquete.replace(valorReemplazoGrupo, '');

                return strResultado;
            }
            else
                return idFila;
        }

        function eliminarFilaTotal(fila, idFila, mostrarAdvertencia) {
            //gaa20160211
            if (!validarEliminarFila(idFila))
                return;
            //fin gaa20160211
            var valor = eliminarFilaGrupo(fila, idFila, mostrarAdvertencia, false);
            if (valor != false)
                eliminarFilaCompra(idFila, valor);

            calcularCFCarrito();
            calcularLCxProductoFijo();

            // Validación Modalidad / Operador Cedente
            if (parent.getValue('hidTienePortabilidad') == 'S') {
                var tipoProducto = getValue('hidTipoProductoActual');
                var tabla = document.getElementById('tblTabla' + tipoProducto);
                if (tabla.rows.length == 0) {
                    parent.document.getElementById('ddlModalidadPorta').disabled = false;
                    parent.document.getElementById('ddlOperadorCedente').disabled = false;
                    parent.document.getElementById('ddlModalidadPorta').selectedIndex = 0;
                    llenarDatosCombo(parent.document.getElementById('ddlOperadorCedente'), '', true);
                }
            }

            parent.autoSizeIframe();
        }

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

            document.getElementById('txtCFTotalCarrito').value = total.toFixed(2);
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
            if (parent.getValue('hidTienePortabilidad') == 'S') {
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

            if (parent.getValue('ddlTipoOperacion') == codTipoOperMigracion || parent.getValue('hidTienePortabilidad') == 'S') {
                mostrarColumnaTelefono(true);
            }
            if (parent.getValue('ddlModalidadVenta') == codModalidadPagoEnCuota) {
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

            hidTienePaquete.value = 'N';
            mostrarColumna(columnaPaquete, false);

            if (ddlOferta.value == '<%= ConfigurationManager.AppSettings["TipoProductoBusiness"] %>') {
                hidTienePaquete.value = 'S';
                mostrarColumna(columnaPaquete, true);
            }

            if (parent.getValue('ddlModalidadVenta') == codModalidadChipSuelto) {
                document.getElementById('divGrillaCabecera').style.width = 850;
                document.getElementById('divGrillaDetalle').style.width = 850;
            }
            else {
                document.getElementById('divGrillaCabecera').style.width = screen.width - 40;
                document.getElementById('divGrillaDetalle').style.width = screen.width - 40;
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
                if (strCodSrv == '<%= ConfigurationManager.AppSettings["codServRoamingI"] %>')
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
        }

        function quitarEquipo3Play() {
            var tabla = document.getElementById('tblResumenAlqEquipo3Play');
            var cont = tabla.rows.length;
            var fila;
            var filaSel;

            for (var i = 1; i < cont; i++) {
                fila = tabla.rows[i];

                filaSel = fila.cells[0].getElementsByTagName("input")[0].checked;

                if (filaSel) {
                    tabla.deleteRow(i);
                    calcularCFEquipo3PlayTotal();
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

            var idFila = document.getElementById('hidLineaActual').value;
            var hidEquiposXfila3Play = document.getElementById('hidEquiposXfila3Play');

            borrarEquipo(idFila);

            hidEquiposXfila3Play.value += obtenerCadenaEquipo3Play(idFila);

            setValue('hidMontoEquipo' + idFila, getValue('txtCFTotalEquipo'));

            calcularCFxProducto();
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
            url += '&strMetodo=' + 'LlenarTopesConsumo';
            self.frames['iframeAuxiliar'].location.replace(url);
        }

        function asignarTopeConsumo(idFila) {
            var ddlTopeConsumo = document.getElementById('ddlTopeConsumo' + idFila);
            llenarDatosCombo(ddlTopeConsumo, getValue('hidTopesConsumo'), false);
            ddlTopeConsumo.value = '0';
        }

        function asignarPlanesCombo(strResultado) {
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
            var strCampanasVentaVarios = '';
            var strCampanasHFCInalamb = '';

            var strPlazosMovil = '';
            var strPlazosFijo = '';
            var strPlazosBAM = '';
            var strPlazosDTH = '';
            var strPlazosHFC = '';
            var strPlazosVentaVarios = '';
            var strPlazosHFCInalamb = '';

            var strPlanesMovil = '';
            var strPlanesFijo = '';
            var strPlanesBAM = '';
            var strPlanesDTH = '';
            var strPlanesHFC = '';
            var strPlanesVentaVarios = '';
            var strPlanesHFCI = '';

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
                    case codTipoProdBAM:
                        strCampanasBAM += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
                        break;
                    case codTipoProdDTH:
                        strCampanasDTH += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
                        break;
                    case codTipoProd3Play:
                        strCampanasHFC += '|' + arrCampana[0] + ';' + arrCampanas[i].split(';')[1];
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

                        LlenarMaterialIfr(idFila, arrCampana[0], '', parent.getValue('ddlCombo'), codTipoProdVentaVarios);

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

            llenarPlanesCombo(strPlanesMovil, strPlanesFijo, strPlanesBAM, strPlanesDTH, strPlanesHFC, strPlanesVentaVarios, strPlanesHFCI);

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

        function llenarPlanesCombo(strPlanesMovil, strPlanesFijo, strPlanesBAM, strPlanesDTH, strPlanesHFC, strPlanesVentaVarios, strPlanesHFCI) {
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
            url += '&strOficina=' + parent.getValue('hidOficina');
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
            url += '&strOficina=' + parent.getValue('hidOficina');
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
            var campanaDECodigo = '<%= ConfigurationManager.AppSettings["CampanaDiaEnamoradosAsociada"] %>';
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

            var ddlPlazo = document.getElementById('ddlPlazo' + idFila);
            var strPlazo = getValue('ddlPlazo' + idFila);
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
                    if (strCampana == '<%= ConfigurationManager.AppSettings["CampanaDiaEnamorados"] %>' ||
                        strCampana == '<%= ConfigurationManager.AppSettings["CampanaDiaEnamoradosAsociada"] %>') {
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
    </script>
</head>
<body onload="inicio();" style="margin: 0px;">
    <form id="frmPrincipal2" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true"></asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
            <td>
                <table class="Contenido3" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr id="trGrilla">
                        <td>
                            <div class="clsGridRow1" id="divCondVent" style="width: 980px; height: 175px;">
                                <div class="clsGridRow" id="divGrillaCabecera" style="width: 970px; overflow: hidden">
                                    <table id="tblTablaTituloMovil" style="border-color:#95b7f3" cellspacing="0" cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
                                            <td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
											<td class="TablaTitulos" id="tdPaqueteMovil" style="DISPLAY: none" align="center" width="200">Paquete</td>
											<td class="TablaTitulos" align="center" width="150">Plan</td>
											<td class="TablaTitulos" align="center" width="30">Serv</td>
											<td class="TablaTitulos" align="center" width="45">Cargo Fijo</td>
											<td class="TablaTitulos" align="center" width="50">Monto Tope S/.</td>
											<td class="TablaTitulos" align="center" width="190">Equipo</td>
											<td class="TablaTitulos" align="center" width="35">Cuotas</td>
											<td class="TablaTitulos" align="center" width="60">Precio Equipo</td>
											<td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Nro. a
												Portar</td>
										</tr>
									</table>
									<table id="tblTablaTituloFijo" style="display: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
                                            <td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
											<td class="TablaTitulos" id="tdPaqueteFijo" style="DISPLAY: none" align="center" width="200">Paquete</td>
											<td class="TablaTitulos" align="center" width="150">Plan</td>
											<td class="TablaTitulos" align="center" width="30">Serv</td>
											<td class="TablaTitulos" align="center" width="45">Cargo Fijo</td>
                                            <td class="TablaTitulos" align="center" width="50">Monto Tope S/.</td>
											<td class="TablaTitulos" align="center" width="190">Equipo</td>
											<td class="TablaTitulos" align="center" width="35">Cuotas</td>
											<td class="TablaTitulos" align="center" width="60">Precio Equipo</td>
											<td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Nro. a
												Portar
											</td>
										</tr>
									</table>
									<table id="tblTablaTituloBAM" style="display: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
                                            <td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
											<td class="TablaTitulos" id="tdPaqueteBAM" style="DISPLAY: none" align="center" width="200">Paquete</td>
											<td class="TablaTitulos" align="center" width="150">Plan</td>
											<td class="TablaTitulos" align="center" width="30">Serv</td>
											<td class="TablaTitulos" align="center" width="45">Cargo Fijo</td>
											<td class="TablaTitulos" align="center" width="190">Equipo</td>
											<td class="TablaTitulos" align="center" width="35">Cuotas</td>
											<td class="TablaTitulos" align="center" width="60">Precio Equipo</td>
											<td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Nro. a
												Portar</td>
										</tr>
									</table>
									<table id="tblTablaTituloDTH" style="display: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="190">Campaña</td>
                                            <td class="TablaTitulos" align="center" width="150">Plazo</td>
											<td class="TablaTitulos" align="center" width="200">Plan</td>
											<td class="TablaTitulos" align="center" width="30">Paq. Adic.</td>
											<td class="TablaTitulos" align="center" width="200">Kits</td>
											<td class="TablaTitulos" align="center" width="50">CF Plan Mensual</td>
											<td class="TablaTitulos" align="center" width="50">CF Men. Alq. Kit</td>
											<td class="TablaTitulos" align="center" width="50">Tot. CF Mensual</td>
											<td class="TablaTitulos" align="center" width="35">Dir. Inst.</td>
										</tr>
									</table>
									<table id="tblTablaTituloHFC" style="DISPLAY: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
											<td class="TablaTitulos" align="center" width="160">Solución</td>
											<td class="TablaTitulos" align="center" width="300">Paquete</td>
											<td class="TablaTitulos" align="center" width="275">Servicios</td>
                                            <td class="TablaTitulos" align="center" width="200">Tope Consumo</td>
											<td class="TablaTitulos" align="center" width="30">Serv. Adic.</td>
											<td class="TablaTitulos" align="center" width="40">Alq. Equipo</td>
											<td class="TablaTitulos" align="center" width="35">Dir. Inst.</td>
											<td class="TablaTitulos" align="center" width="35">Det. Ofert.</td>
                                            <td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Nro. a
												Portar</td>
										</tr>
									</table>
									<table id="tblTablaTituloVentaVarios" style="DISPLAY: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
                                            <td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="190">Equipo</td>
                                            <td class="TablaTitulos" align="center" width="150">Lista Precio</td>
											<td class="TablaTitulos" align="center" width="60">Precio</td>
											<td class="TablaTitulos" align="center" width="35" style="display:none">Cuotas</td>
										</tr>
									</table>
                                    <table id="tblTablaTitulo3PlayInalam" style="DISPLAY: none; border-color:#95b7f3" cellspacing="0"
										cellpadding="0" border="0">
										<tr>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="18">&nbsp;</td>
											<td class="TablaTitulos" align="center" width="190">Campaña</td>
											<td class="TablaTitulos" align="center" width="145">Plazo</td>
											<td class="TablaTitulos" align="center" width="160">Solución</td>
											<td class="TablaTitulos" align="center" width="300">Paquete</td>
											<td class="TablaTitulos" align="center" width="275">Servicios</td>
                                            <td class="TablaTitulos" align="center" width="200">Tope Consumo3</td>
											<td class="TablaTitulos" align="center" width="30">Serv. Adic.</td>
											<td class="TablaTitulos" align="center" width="40">Alq. Equipo</td>
											<td class="TablaTitulos" align="center" width="35">Dir. Inst.</td>
											<td class="TablaTitulos" align="center" width="35">Det. Ofert.</td>
                                            <td class="TablaTitulos" style="DISPLAY: none" align="center" width="60">Nro. a
												Portar3</td>
										</tr>
									</table>
                                </div>
                                <div class="clsGridRow" id="divGrillaDetalle" style="width: 950px; height: 100px">
                                    <table id="tblTablaMovil" style="border-color:#95b7f3" cellspacing="0" cellpadding="0" border="0">
                                    </table>
                                    <table id="tblTablaFijo" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                    <table id="tblTablaBAM" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                    <table id="tblTablaDTH" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                    <table id="tblTablaHFC" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                    <table id="tblTablaVentaVarios" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                     <table id="tblTabla3PlayInalam" style="display: none; border-color:#95b7f3" cellspacing="0"
                                        cellpadding="0" border="0">
                                    </table>
                                </div>
                                <table id="tblCFTotal" cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="Arial10B" style="white-space:normal" align="center">
                                            CF Total:&nbsp;&nbsp;
                                            <input class="clsInputDisable" id="txtCFTotal" style="width: 50px; text-align: right"
                                                readonly="readonly" name="txtCFTotal" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="Contenido" id="tblServicios" style="display: none" height="270" cellspacing="1"
                                cellpadding="0" width="500" align="center" border="0" runat="server">
                                <tr>
                                    <td class="Header">
                                        Servicios Adicionales - Plan
                                        <asp:Label ID="lblIdLista" runat="server" CssClass="Arial10B" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Contenido">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr valign="top">
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <u>Servicios Adicionales</u>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <u>Servicios Contratados </u>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <select class="clsSelectEnable" id="lbxServiciosDisponibles1" style="width: 310px"
                                                        onclick="mostrarPromociones('D', this.value)" size="5" name="lbxServiciosDisponibles1">
                                                    </select>
                                                </td>
                                                <td class="Arial10B" style="width: 150px; background-color: white" valign="top" align="center">
                                                    <input class="Boton" id="btnAgregarServicio" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 90px; cursor: hand; height: 19px" onclick="javascript:agregarServicio();"
                                                        onmouseout="this.className='Boton';" type="button" value="Agregar >" name="btnAgregarServicio" /><br />
                                                    <br />
                                                    <input class="Boton" id="btnQuitarServicio" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 90px; cursor: hand; height: 19px" onclick="javascript:quitarServicio();"
                                                        onmouseout="this.className='Boton';" type="button" value="< Quitar" name="btnQuitarServicio" /><br />
                                                    <br />
                                                    <input class="Boton" id="btnResetServicios" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 90px; cursor: hand; height: 19px" onclick="javascript:resetServicio('');"
                                                        onmouseout="this.className='Boton';" type="button" value="Limpiar" name="btnResetServicios" /><br />
                                                    <br />
                                                    <input class="Boton" id="btnCerrarServicios" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 120px; cursor: hand; height: 19px" onclick="javascript:guardarServicio();"
                                                        onmouseout="this.className='Boton';" type="button" value="Cerrar y Guardar" name="btnCerrarServicios" />
                                                </td>
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <select class="clsSelectEnable" id="lbxServiciosAgregados1" style="width: 310px"
                                                        onclick="mostrarPromociones('A', this.value)" size="5" name="lbxServiciosAgregados1">
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr id="trPromocion" style="display: none" valign="top">
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <u>Promociones Adicionales</u>
                                                </td>
                                                <td>
                                                </td>
                                                <td class="Arial10B" style="width: 280px; background-color: white" valign="top">
                                                    <u>Promociones Agregadas</u>
                                                </td>
                                            </tr>
                                            <tr id="trPromocion1" style="display: none" valign="top">
                                                <td>
                                                    <select class="clsSelectEnable" id="lbxPromocionesDisponibles" style="width: 310px"
                                                        size="5" name="lbxPromocionesDisponibles">
                                                    </select>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                    <select class="clsSelectEnable" id="lbxPromocionesAgregadas" style="display: none;
                                                        width: 270px" size="5" name="lbxPromocionesAgregadas">
                                                    </select><select class="clsSelectEnable" id="lbxPromocionesSeleccionadas" style="width: 310px"
                                                        size="5" name="lbxPromocionesSeleccionadas"></select>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table class="Contenido" id="tblRoamingI" style="display: none" cellspacing="1" cellpadding="0"
                                            width="200" align="right" border="0" runat="server">
                                            <tr>
                                                <td class="Contenido">
                                                    <table cellspacing="5" cellpadding="0" width="100%" border="0">
                                                        <tr>
                                                            <td class="Arial10B" style="background-color: white">
                                                                PLAZO
                                                            </td>
                                                            <td class="Arial10B" style="width: 320px">
                                                                <input id="rbtValDeterminado" onclick="f_cambiarPlazoRI(this.value)" type="radio"
                                                                    value="01" name="rbtTipoPlazo" />Determinado
                                                            </td>
                                                            <td class="Arial10B" style="width: 400px">
                                                                <input id="rbtValIndeterminado" onclick="f_cambiarPlazoRI(this.value)" type="radio"
                                                                    value="02" name="rbtTipoPlazo" />Indeterminado
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Arial10B" id="tdLblFechaDesde" style="display: none">
                                                                Desde:
                                                            </td>
                                                            <td id="tdTxtFechaDesde" style="display: none" colspan="2">
                                                                <asp:TextBox ID="txtFechaDesde" runat="server" Width="64px" CssClass="clsInputDisable"
                                                                    ReadOnly="true" MaxLength="10"></asp:TextBox>
                                                                <asp:ImageButton ID="btnFechaDesde" runat="server" ImageUrl="../../Imagenes/Botones/btn_Calendario.gif" />
                                                                <script type="text/javascript">
                                                                    Calendar.setup(
																				{
																				    inputField: "txtFechaDesde",      // id of the input field
																				    ifFormat: "%d/%m/%Y",           // format of the input field                                                        
																				    button: "btnFechaDesde",      // trigger for the calendar (button ID)
																				    singleClick: true,                 // double-click mode
																				    step: 1                     // show all years in drop-down boxes (instead of every other year as default)
																				}
																			);
                                                                </script>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="Arial10B" id="tdLblFechaHasta" style="display: none">
                                                                Hasta:
                                                            </td>
                                                            <td id="tdTxtFechaHasta" style="display: none" colspan="2">
                                                                <asp:TextBox ID="txtFechaHasta" runat="server" Width="64px" CssClass="clsInputDisable"
                                                                    ReadOnly="true" MaxLength="10"></asp:TextBox><asp:ImageButton ID="btnFechaHasta" runat="server"
                                                                        ImageUrl="../../Imagenes/Botones/btn_Calendario.gif"></asp:ImageButton><script type="text/javascript">
                                                                                                                                                 Calendar.setup(
																			{
																			    inputField: "txtFechaHasta",      // id of the input field
																			    ifFormat: "%d/%m/%Y",           // format of the input field                                                        
																			    button: "btnFechaHasta",      // trigger for the calendar (button ID)
																			    singleClick: true,                 // double-click mode
																			    step: 1                     // show all years in drop-down boxes (instead of every other year as default)
																			}
																		);
                                                                        </script>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table class="Contenido" id="tblCuotas" style="display: none" cellspacing="1" cellpadding="0"
                                width="400" align="center" border="0" runat="server">
                                <tr>
                                    <td class="Header" height="20">
                                        Cuotas
                                    </td>
                                </tr>
                                <tr>
                                    <td class="Contenido">
                                        <table cellspacing="5" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    Nro Cuotas
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <select class="clsSelectEnable0" id="ddlNroCuotas" style="width: 100px" onchange="cambiarCuota(this.value)"
                                                        name="ddlNroCuotas">
                                                    </select>
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    % Cuota Inicial
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <input class="clsInputDisable" id="txtPorcCuotaIni" style="width: 50px; text-align: right"
                                                        readonly="readonly" type="text" name="txtPorcCuotaIni" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    Monto Cuota Inicial
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <input class="clsInputDisable" id="txtMontoCuotaIni" style="width: 50px; text-align: right"
                                                        readonly="readonly" type="text" name="txtMontoCuotaIni" />
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    Monto Cuota
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <input class="clsInputDisable" id="txtMontoCuota" style="width: 50px; text-align: right"
                                                        readonly="readonly" type="text" name="txtMontoCuota" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <input class="Boton" id="btnCerrarCuotas" onmouseover="this.className='BotonResaltado';"
                                                        style="width: 120px; cursor: hand; height: 19px" onclick="javascript:guardarCuota();"
                                                        onmouseout="this.className='Boton';" type="button" value="Cerrar y Guardar" name="btnCerrarCuotas" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table class="Contenido" id="tblEquipos" style="display: none" cellspacing="1" cellpadding="0"
                                width="650" align="center" border="0" runat="server">
                                <tr>
                                    <td class="Arial10B" valign="top" align="center" width="45%">
                                        <table style="height:20" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="TablaTitulos" align="center" width="100%">
                                                    ALQUILER DE EQUIPOS
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <table style="height:25" cellspacing="2" cellpadding="0" border="0">
                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    Equipo&nbsp;&nbsp;
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <select class="clsSelectEnable0" id="ddlEquipo3Play" style="width: 170px" name="ddlEquipo3Play"
                                                        onchange="cambiarEquipo3Play(this.value)" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    CF Alquiler&nbsp;&nbsp;
                                                </td>
                                                <td class="Arial10B" style="background-color: white" valign="top">
                                                    <input class="clsInputDisable" id="txtPrecAlqEquipo3Play" style="width: 50px" readonly="readonly"
                                                        type="text" name="txtPrecAlqEquipo3Play" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                    <td align="center" width="10%">
                                        <input class="Boton" id="btnAgregarEquipo3Play" onmouseover="this.className='BotonResaltado';"
                                            style="width: 90px; cursor: hand; height: 19px" onclick="javascript:agregarEquipo3Play();"
                                            onmouseout="this.className='Boton';" type="button" value="Agregar >" name="btnAgregarEquipo3Play" /><br />
                                        <br />
                                        <input class="Boton" id="btnQuitarEquipo3Play" onmouseover="this.className='BotonResaltado';"
                                            style="width: 90px; cursor: hand; height: 19px" onclick="javascript:quitarEquipo3Play();"
                                            onmouseout="this.className='Boton';" type="button" value="< Quitar" name="btnQuitarEquipo3Play" /><br />
                                        <br />
                                        <input class="Boton" id="btnCerrarEquipo3Play" onmouseover="this.className='BotonResaltado';"
                                            style="width: 120px; cursor: hand; height: 19px" onclick="javascript:guardarEquipo3Play();"
                                            onmouseout="this.className='Boton';" type="button" value="Cerrar y Guardar" name="btnCerrarEquipo3Play" />
                                    </td>
                                    <td class="Arial10B" valign="top" align="center" width="45%">
                                        <table style="height:20" cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td class="TablaTitulos" align="center" width="100%">
                                                    RESUMEN DE ALQUILER DE EQUIPOS
                                                </td>
                                            </tr>
                                        </table>
                                        <b />
                                        <table style="height:25" cellspacing="2" cellpadding="0" border="0">
                                            <tr>
                                                <td>
                                                    <div class="clsGridRow" style="width: 200PX; height: 100px; overflow: auto">
                                                        <table id="tblResumenAlqEquipo3Play" style="border-color:#95b7f3" cellspacing="0" cellpadding="0"
                                                            width="100%" border="0">
                                                            <tr>
                                                                <td class="TablaTitulos" align="center">
                                                                </td>
                                                                <td class="TablaTitulos" align="center">
                                                                    Equipo
                                                                </td>
                                                                <td class="TablaTitulos" align="center">
                                                                    CF Alq.
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <table cellpadding="1" width="100%" border="0">
                                                        <tr>
                                                            <td class="Arial10B" align="center">
                                                                Total CF Alquiler:&nbsp;&nbsp;<input class="clsInputDisable" id="txtCFTotalEquipo"
                                                                    style="width: 50px; text-align: right" readonly name="txtCFTotalEquipo" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trResumenCompras" style="display: none">
            <td>
                <table cellpadding="1" width="100%" border="0">
                    <tr>
                        <td class="Header" align="left" height="18">
                            &nbsp;Resumen de Compras
                        </td>
                    </tr>
                </table>
                <table id="tbResumenCompras" style="border-color:#95b7f3" cellspacing="0" cellpadding="0"
                    width="100%" border="0">
                    <tr>
                        <td class="TablaTitulos" align="center">
                            Tipo de Producto
                        </td>
                        <td class="TablaTitulos" align="center">
                            Plazo
                        </td>
                        <td class="TablaTitulos" align="center">
                            Grupo Producto
                        </td>
                        <td class="TablaTitulos" align="center">
                            Plan
                        </td>
                        <td class="TablaTitulos" align="center">
                            Campaña
                        </td>
                        <td class="TablaTitulos" align="center">
                            Equipo
                        </td>
                        <td class="TablaTitulos" align="center">
                            Cargo Fijo Total
                        </td>
                        <td class="TablaTitulos" align="center">
                            Precio de Venta Equipo
                        </td>
                        <td class="TablaTitulos" align="center">
                            Equipo en Cuotas
                        </td>
                        <td class="TablaTitulos" align="center">
                            Nro. Cuotas
                        </td>
                        <td class="TablaTitulos" align="center">
                            Monto Cuota
                        </td>
                        <td class="TablaTitulos" align="center">
                            Precio Inst.
                        </td>
                        <td class="TablaTitulos" align="center">
                            &nbsp;
                        </td>
                        <td class="TablaTitulos" align="center">
                            &nbsp;
                        </td>
                        <td class="TablaTitulos" align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table cellpadding="1" width="100%" border="0">
                    <tr>
                        <td class="Arial10B" align="center">
                            CF Total:&nbsp;&nbsp;<input class="clsInputDisable" id="txtCFTotalCarrito" style="width: 50px;
                                text-align: right" readonly="readonly" name="txtCFTotalCarrito" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <input id="hidPlanServicio" type="hidden" name="hidPlanServicio" />
                <input id="hidPlanServicioNo" type="hidden" name="hidPlanServicioNo" />
                <input id="hidPlanServicioNoGrupo" type="hidden" name="hidPlanServicioNoGrupo" />
                <input id="hidPlanServicioNGTemp" type="hidden" name="hidPlanServicioNGTemp" />
                <input id="hidLineaActual" type="hidden" name="hidLineaActual" />
                <input id="hidTotalLineas" type="hidden" value="0" name="hidTotalLineas" />
                <input id="hidPlazoActual" type="hidden" name="hidPlazoActual" runat="server" />
                <input id="hidPaqueteActual" type="hidden" name="hidPaqueteActual" />
                <input id="hidTienePaquete" type="hidden" value="N" name="hidTienePaquete" />
                <input id="hidPlan" type="hidden" name="hidPlan" />
                <input id="hidTraerPlazo" type="hidden" name="hidTraerPlazo" />
                <input id="hidCuota" type="hidden" name="hidCuota" />
                <input id="hidNroCuotaActual" type="hidden" name="hidNroCuotaActual" />
                <input id="hidCampana" type="hidden" name="hidCampana" />
                <input id="hidGrupoPaquete" type="hidden" name="hidGrupoPaquete" />
                <input id="hidTipoClienteActual" type="hidden" name="hidTipoClienteActual" />
                <input id="hidTipoProductoActual" type="hidden" value="Movil" name="hidTipoProductoActual" />
                <input id="hidTieneCuotas" type="hidden" name="hidTieneCuotas" />
                <input id="hidValidarGuardarCuota" type="hidden" name="hidValidarGuardarCuota" runat="server" />
                <input id="hidPlazos" type="hidden" name="hidPlazos" runat="server" />
                <input id="hidAccion" type="hidden" name="hidAccion" runat="server" />
                <input id="hidTipoDocumento" type="hidden" name="hidTipoDocumento" runat="server" />
                <input id="hidEquiposSel" type="hidden" name="hidEquiposSel" />
                <input id="hidPaqActCompleto" type="hidden" name="hidPaqActCompleto" />
                <input id="hidListaTipoProducto" type="hidden" name="hidListaTipoProducto" />
                <input id="hidServicioOriginal" type="hidden" name="hidServicioOriginal" />
                <input id="hidServicioOriginalNo" type="hidden" name="hidServicioOriginalNo" />
                <input id="hidPromociones" type="hidden" name="hidPromociones" />
                <input id="hidPromocion" type="hidden" name="hidPromocion" />
                <input id="hidPromocionTemp" type="hidden" name="hidPromocionTemp" />
                <input id="hidSolucion3Play" type="hidden" name="hidSolucion3Play" />
                <input id="hidPaquete3Play" type="hidden" name="hidPaquete3Play" />
                <input id="hidCodigoTipoProductoActual" type="hidden" value="01" name="hidCodigoTipoProductoActual" />
                <input id="hidCampanasDTH" type="hidden" name="hidCampanasDTH" />
                <input id="hidCadenaDetalle" type="hidden" name="hidCadenaDetalle" />
                <input id="hidFlgOrigen" type="hidden" name="hidFlgOrigen" />
                <input id="hidFlagVOD" type="hidden" name="hidFlagVOD" />
                <input id="hidCampanasHFC" type="hidden" name="hidCampanasHFC" />
                <input id="hidEquiposXplan" type="hidden" name="hidEquiposXplan" />
                <input id="hidEquiposXfila3Play" type="hidden" name="hidEquiposXfila3Play" />
                <input id="hidTopesConsumo" type="hidden" name="hidTopesConsumo" />
                <input id="hidCampanasMovil" type="hidden" name="hidCampanasMovil" />
                <input id="hidCampanasFijo" type="hidden" name="hidCampanasFijo" />
                <input id="hidCampanasBAM" type="hidden" name="hidCampanasBAM" />
                <input id="hidCampanasVentaVarios" type="hidden" name="hidCampanasVentaVarios" />
                <input id="hidPlazosMovil" type="hidden" name="hidPlazosMovil" />
                <input id="hidPlazosFijo" type="hidden" name="hidPlazosFijo" />
                <input id="hidPlazosBAM" type="hidden" name="hidPlazosBAM" />
                <input id="hidPlazosHFC" type="hidden" name="hidPlazosHFC" />
                <input id="hidPlazosDTH" type="hidden" name="hidPlazosDTH" />
                <input id="hidPlazosVentaVarios" type="hidden" name="hidPlazosVentaVarios" /> 
                <input id="hidPlanesMovil" type="hidden" name="hidPlanesMovil" />
                <input id="hidPlanesFijo" type="hidden" name="hidPlanesFijo" />
                <input id="hidPlanesBAM" type="hidden" name="hidPlanesBAM" />
                <input id="hidPlanesDTH" type="hidden" name="hidPlanesDTH" />
                <input id="hidCampanasHFCInalamb" type="hidden" name="hidCampanasHFCInalamb" />                                
                <input id="hidPlanesHFC" type="hidden" name="hidPlanesHFC" />
                <input id="hidPlazosHFCInalamb" type="hidden" name="hidPlazosHFCInalamb" />
                <input id="hidPlanesHFCInalamb" type="hidden" name="hidPlanesHFCInalamb" />
 
                <input id="hidPlanesVentaVarios" type="hidden" name="hidPlanesVentaVarios" />
                <input id="hidPlanesCombo" type="hidden" name="hidPlanesCombo" />
                <input id='hidListaDE' type="hidden" name="hidListaDE" />
                <input id='hidEsSecPendiente' type="hidden" name="hidEsSecPendiente" />
                <iframe id="iframeAuxiliar" name="iframeAuxiliar" frameborder="0" width="0" height="0" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>