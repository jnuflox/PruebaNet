<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_pop_debito_automatico.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_pop_debito_automatico" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registrar Afiliación / Desafiliación Individuales</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_evaluacion.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/funciones_sec.js"></script>
    <script type="text/javascript" language="JavaScript" src="../../Scripts/jquery-1.9.1.js"></script>
	<script type="text/javascript" language="JavaScript" src="../../Scripts/KeySettings/KeySettings.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
    <script type="text/javascript" language="JavaScript">

        function inicio() {

            var numeroDocumento = $('#hidNumeroDocumento').val();
            var nombres = $('#hidNombresRazonSocial').val();
            var tipoDoc = $('#hidTipoDocumento').val();
            var montoMaximo = $('#hidMontoMaximo').val();
            var montoMax = parseFloat(montoMaximo).toFixed(2);
            var correoNoti = $('#hidCorreoElectronico').val();
            codSolicitud = '-2';
            $('#hidIdSolicitud').val(codSolicitud);
            document.getElementById('rbtTarjeta').checked = true;
            document.getElementById('rbtConMonto').checked = true;
            llenarDatosCombo(document.getElementById('ddlTipoCuenta'), '', true);
            $('#txtMontoMax').val(montoMax);
            $('#hidMontoMaximo').val(montoMax);
            $('#txtnumDocCli').val(numeroDocumento);
            $('#txtNombreCli').val(nombres);
            $('#ddlTipoDoc').val(tipoDoc);

            document.getElementById('chkNuevaTarjeta').click(); // iniciativa 941

            limpiarDatos();
            CargarEntidad(codSolicitud);
						
            //PROY-140657 INI 
			
			 if ($('#FlagConsultarAltaDEAU').val() == "0") {
                document.getElementById('BtnConsultar').style.display = 'none';
            }
			
            document.getElementById('td_TAfiliados').style.display = 'none';
                     
            if ($('#hidOfertaDEAU').val() == "01") //si tipo de oferta MASIVO (configurable)
            {
                document.getElementById("td_rbtCuenta").style.display = "none";
                codSolicitud = '-2';
                $('#hidIdSolicitud').val(codSolicitud);
            } else {
                codSolicitud = '-3';
            }
            f_cambiarSolicitud(codSolicitud);

            //PROY-140657 FIN

            // INI INICIATIVA 941 - IDEA 142525
            if ($('#hidnEnvioLinkFallido').val() === "true") {
                $('#btnAceptar').hide();
                $('#BtnConsultar').hide();
                $('#btnCancelar').hide();
                $('#btnReenvioLink').show();
                document.getElementById('tdRowUsarTarjeta').style.display = 'none';
            }


            $('#txtCorreo').val(correoNoti);
            // FIN INICIATIVA 941 - IDEA 142525
        }

        function cambiarTipoEntidad(ddl) {
            var idEntidad = ddl.value;
            if (idEntidad != '') {
                PageMethods.cargarTipoCuenta(idEntidad, cargarTipoCuenta_Callback);
                validarLongitud(idEntidad);
            }
            else {
                var ddlCombo = document.getElementById('ddlTipoCuenta');
                llenarDatosCombo(ddlCombo, '', true);
                ddlCombo.selectedIndex = 0;
            }
        }

        function cargarTipoCuenta_Callback(objResponse) {

            quitarImagenEsperando();
            if (objResponse.Error) {
                alert(objResponse.Mensaje);
                return;
            }

            var arrResultado = objResponse.Cadena;

            var ddlCombo = document.getElementById('ddlTipoCuenta');
            llenarDatosCombo(ddlCombo, arrResultado, true);
            ddlCombo.selectedIndex = 1;
        }

        function f_cambiarSolicitud(codSolicitud) {

            if (codSolicitud == '-2') {
                //PROY-140657 INI               
                document.getElementById('trDTarjeta1').style.display = 'none';
                document.getElementById('trDTarjeta2').style.display = 'none';
                //document.getElementById('BtnConsultar').style.display = 'none';
                document.getElementById('hidMontoMaxCalc').value = '';
                $('#txtMontoMax').removeAttr('readonly');
                document.getElementById('hidMontoMaxCalc').value = getValue('txtMontoMax');

                if ($('#hidOfertaDEAU').val() == "01") //si tipo de oferta MASIVO (configurable)
                {
                document.getElementById('rbtTarjeta').checked = true;
                    $("#td_rbtCuenta").hide();
                } else {
                    $("#td_rbtCuenta").show();
                }
                //PROY-140657 FIN
            }
            else {
                //PROY-140657 INI
                document.getElementById('trDTarjeta1').style.display = '';
                document.getElementById('trDTarjeta2').style.display = '';
                //document.getElementById('BtnConsultar').style.display = 'none';
                document.getElementById("td_GvTarjeta").style.display = "none";
                
                //PROY-140657 FIN
                document.getElementById('tdFecha').style.display = 'none';
                document.getElementById('tddFecha').style.display = 'none';
                document.getElementById('rbtCuenta').checked = true;
                document.getElementById('rbtTarjeta').checked = false;

            }

            limpiarDatos();

            $('#hidIdSolicitud').val(codSolicitud);
            llenarDatosCombo(document.getElementById('ddlTipoCuenta'), '', true);

            CargarEntidad(codSolicitud);
        }

        function CargarEntidad(codSolicitud) {
            PageMethods.CargarEntidadFront(codSolicitud, CargarEntidadFront_Callback);
        }

        function CargarEntidadFront_Callback(objResponse) {

            quitarImagenEsperando();
            if (objResponse.Error) {
                alert(objResponse.Mensaje);
                return;
            }

            var arrResultado = objResponse.Cadena;

            var ddlCombo = document.getElementById('ddlEntidad');
            llenarDatosCombo(ddlCombo, arrResultado, true);

        }

        function f_cambiarMonto(flagMonto) {
            if (flagMonto == 'C') {
                var monto = $('#hidMontoMaximo').val();
                setValue('txtMontoMax', monto);
                setEnabled('txtMontoMax', true, 'clsInputEnabled');
                document.getElementById('txtMontoMax').disabled = false;
                document.getElementById('rbtSinMonto').checked = false;
                document.getElementById('rbtConMonto').checked = true;
            }
            else {
                setEnabled('txtMontoMax', false, 'clsInputDisabled');
                setValue('txtMontoMax', '0');
                document.getElementById('rbtSinMonto').checked = true;
                document.getElementById('rbtConMonto').checked = false;
            }
        }

        function validarLongitud(idEntidad) {
            document.getElementById('txtNumCuenta').value = '';
            document.getElementById('txtNumCuenta').maxLength = getMaxLengthTarjeta(idEntidad);
            setFocus('txtNumCuenta');
        }


        function getMaxLengthTarjeta(idEntidad) {
            var nroCaracter;
            if (idEntidad == '-1' || idEntidad == '-2') nroCaracter = 16;
            else if (idEntidad == '-3') nroCaracter = 15;
            else if (idEntidad == '-4') nroCaracter = 14;
            else nroCaracter = 16;
            return nroCaracter;
        }

        function limpiarDatos() {

            $('#ddlEntidad').val('');
            $('#ddlTipoCuenta').val('');
            $('#txtNumCuenta').val('');
            $('#txtFechaVenc').val('');
            $('#txtCorreo').val('');
            $('#txtNumeroNot').val('');

        }

        function Grabar() {
            if (!nuevaTarjetaMarcada && !columnaUsarTarjetaEstaMarcada) {
                alert('Por favor seleccione "Nueva Tarjeta"'); 
                return;
            }
            var nombres = $('#hidNombresRazonSocial').val();
            var tipoDocumento = getValue('ddlTipoDoc');
            var descTipoDoc = getText('ddlTipoDoc');
            var numDocumento = $('#hidNumeroDocumento').val();

            var codEntidad = getValue('ddlEntidad');
            var descEntidad = getText('ddlEntidad');
            var codCuenta = getValue('ddlTipoCuenta');
            var descCuenta = getText('ddlTipoCuenta');
            var numCuenta = $('#txtNumCuenta').val();
            var fecVencimiento = $('#txtFechaVenc').val();
            var flagMonto = '';
            var montoMax = $('#txtMontoMax').val();

            var correo = $('#txtCorreo').val();
            var numeroANoti = $('#txtNumeroNot').val();

            var FlagAfiliacion = $('#FlagAfiliacionDEAU').val();
            var MsjValidacionMonto = $('#MsjValidacionMontoDEAU').val();
            var MsjCorreoObligatorio = $('#MsjCorreoObligatorioDEAU').val();

            if (document.getElementById("rbtSinMonto").checked == true)
                flagMonto = $('#rbtSinMonto').val();
            else
                flagMonto = $('#rbtConMonto').val();

            if (descCuenta == 'SELECCIONE...')
                descCuenta = "";

            var codSolicitud = '';
            var descSolicitud = '';

            var idAfiliacion = '';
            var idTarjeta = '';
            var nroTarjeta = '';
            var flagUsarTarjeta = 0;

            if (columnaUsarTarjetaEstaMarcada) {
                flagUsarTarjeta = 1;
                idAfiliacion = $('#idAfiliacion').val();
                idTarjeta = $('#idTarjeta').val();
                nroTarjeta = $('#nroTarjeta').val();
            }


            if (document.getElementById("rbtCuenta").checked == true) {
                codSolicitud = $('#rbtCuenta').val();
                descSolicitud = "Cuenta Bancaria";
            }
            else {
                codSolicitud = $('#rbtTarjeta').val();
                descSolicitud = "Tarjeta";
            }
            //PROY-140657 INI
            if (codSolicitud == "-2" && FlagAfiliacion ) {

                if (flagMonto == 'C' && montoMax < $('#hidMontoMaxCalc').val()) {
                    alert(MsjValidacionMonto);
                    var monto = $('#hidMontoMaximo').val();
                    setValue('txtMontoMax', monto);
                    return;
                }
                if (correo == "" || typeof correo == "undefined") {
                    alert(MsjCorreoObligatorio);
                    return;
                }
            }
            else {
                //PROY-140657 FIN
                if (codSolicitud == "-3") {
                if (codEntidad == "" || typeof codEntidad == "undefined") {
                    alert('Debe seleccionar una Entidad.');
                    return;
                }
                if (codCuenta == "" || typeof codCuenta == "undefined") {
                        alert('Debe seleccionar un Tipo de Cuenta.');
                    return;
                }
                if (numCuenta == "" || typeof numCuenta == "undefined") {
                        alert('Debe ingresar un Número de Cuenta.');
                    return;
                }
                if (numCuenta.length != getMaxLengthTarjeta(document.getElementById('ddlEntidad').value)) {
                        alert('Debe introducir un Número correcto de Cuenta.')
                    return;
                }
            }

            } //PROY-140657
            if (fecVencimiento.length == 4) {
                var mm = fecVencimiento.substr(0, 2);
                var yy = fecVencimiento.substr(2, 2);

                var dateNow = new Date();
                var mesActual = parseInt(dateNow.getMonth()) + 1;
                var anioActual = dateNow.getFullYear().toString();
                var year = anioActual.substr(2, 2);

                if (parseInt(mm, 10) >= 13 || parseInt(mm, 10) == 0) {
                    alert('Fecha de vencimiento (mes) inválido.')
                    return;
                }
                if (yy < year) {
                    alert('Fecha de vencimiento (año) inválido.');
                    return;
                }
                if (parseInt(mm, 10) <= mesActual && yy <= year) {
                    alert('Fecha de vencimiento inválida.');
                    return;
                }

                var fecha = mm + '/' + yy;
                setValue('txtFechaVenc', fecha);
            }

            if (flagMonto == "C" && (montoMax == "" || typeof montoMax == "undefined" || montoMax == '0')) {
                alert('Debe ingresar un Monto máximo mayor a 0.');
                return;
            }

            if (correo != "" && typeof correo != "undefined") {
                if (!validarEmail(correo)) {
                    alert('Debe ingresar una dirección de correo electrónico valida.');
                    return;
                }
            }
            if (numeroANoti == "" || typeof numeroANoti == "undefined") {
                alert('Debe ingresar un número de teléfono.');
                return;
            }

            if (numeroANoti.length != 9 || (numeroANoti.substr(0, 1) != "9")) {
                alert('Formato de teléfono a notificar inválido.');
                return;
            }

            if (fecVencimiento != '' && typeof fecVencimiento != "undefined") {
                fecVencimiento = '01' + '/' + getValue('txtFechaVenc');
            }

            if (columnaUsarTarjetaEstaMarcada) {
                if (!confirm('¿Esta utilizando los datos de la tarjeta ' + $('#nroTarjeta').val() + ', esta seguro que desea continuar?')) { 
                    return
                }
            }

            if (confirm("¿Está seguro de asignar la Afiliación al débito automático?")) {
                PageMethods.Grabar(codEntidad, descEntidad, codCuenta, descCuenta, numCuenta, fecVencimiento, flagMonto, montoMax, nombres, numDocumento,
            descTipoDoc, tipoDocumento, correo, numeroANoti, codSolicitud, descSolicitud, flagUsarTarjeta, idAfiliacion, idTarjeta, nroTarjeta, Grabar_Callback);
            }
        }

        //INICIATIVA 941: INI
        function Grabar_Callback(objResponse) {
            if (objResponse.Boleano == true) {
                if (objResponse.Error == false) {
                    retornoValue('');
                } else {
                    if (confirm(objResponse.Mensaje)) {
                        return;
                    }
                    else {
                retornoValue('');
            }
                }
            }
            else {
                retornoValue(objResponse.Mensaje);
            }
        }
        //INICIATIVA 941: FIN
        

        function retornoValue(msg) {
            window.returnValue = msg;
            window.close();
            return;
        }
        //PROY-140657 INI
        
        //GRILLA INI
        function f_ConsultarAfiliados() {
            var tipoDocumento = getValue('hidTipoDocumento');
            var numDocumento = getValue('hidNumeroDocumento');
            document.getElementById('tdNoRegistros').style.display = 'none';
            

            PageMethods.ConsultarAfiliados2(tipoDocumento, numDocumento, ConsultarAfiliados_Callback);
            
        }

        function ConsultarAfiliados_Callback(objResponse) {
            var cadenaAfiliados = objResponse.Cadena;
            var idFilaFCRef = 0;

            if (objResponse.CodigoError != "0") {
                //poner mensaje en el label
                document.getElementById('tdNoRegistros').style.display = '';
                document.getElementById('lblNoRegistros').innerHTML = objResponse.Mensaje;
                return;
            }
            
			document.getElementById('td_TAfiliados').style.display = '';
			var divClear= document.getElementById('divTableAfiliados');
				divClear.innerHTML="";
			
			var tblCrea='';
						tblCrea='<table id="Table1" class="Contenido" cellpadding="3" cellspacing="5" bgcolor="white" align="center"style="MAX-WIDTH: 500px">';
						tblCrea = tblCrea + '<thead>	<tr><th>CUSTOMER ID</th><th>NUMERO DE TARJETA</th><th>ESTADO</th><th>FECHA AFILIACION</th><th>ORIGEN AFILIACION</th><th>USAR TARJETA</th></tr></thead>';
						tblCrea= tblCrea + '<tbody id="tbodyAfiliadoDebito"></tbody></table>';						
			divClear.innerHTML=tblCrea;
		
            if (cadenaAfiliados != '' || cadenaAfiliados.length > 0) {

                var arrDetLineasRef = cadenaAfiliados.split('|');

                for (var index = 0; index < arrDetLineasRef.length; index++) {

                    if (arrDetLineasRef[index] != "") {
                        var arrItemDetLinRef = arrDetLineasRef[index].split('#');

                        idFilaFCRef++;

                        var newRow = document.all('tbodyAfiliadoDebito').insertRow();
                        newRow.style.verticalAlign = 'top';
                        newRow.style.backgroundColor = '#FFFFFF';

                        //CustomerID
                        var oCell = newRow.insertCell();
                        oCell.style.width = '120px';
                        oCell.align = 'center';
                        oCell.innerHTML = "<input  class='Arial10B' readonly  style='width:100px;text-align:center;border:0;' name='txtCustomerId" + idFilaFCRef + "' id='txtCustomerId" + idFilaFCRef + "' value='" + arrItemDetLinRef[0] + "' />";


                        //Numero Tarjeta
                        var oCell = newRow.insertCell();
                        oCell.style.width = '100px';
                        oCell.align = 'center';
                        oCell.innerHTML = "<input  class='Arial10B' readonly style='width:150px;text-align:center;border:0;' name='txtNroTarjeta" + idFilaFCRef + "' id='txtNroTarjeta" + idFilaFCRef + "' value='" + arrItemDetLinRef[2] + "' />";

                        //Estado
                        var oCell = newRow.insertCell();
                        oCell.style.width = '100px';
                        oCell.align = 'center';
                        oCell.innerHTML = "<input  class='Arial10B' readonly style='width:150px;text-align:center;border:0;' name='txtEstado" + idFilaFCRef + "' id='txtEstado" + idFilaFCRef + "' value='AFILIADO' />";


                        //Fecha de registro
                        var oCell = newRow.insertCell();
                        oCell.style.width = '150px';
                        oCell.align = 'center';
                        oCell.innerHTML = "<input  class='Arial10B' readonly style='width:150px;text-align:center;border:0;' name='txtFechaRegistro" + idFilaFCRef + "' id='txtFechaRegistro" + idFilaFCRef + "' value='" + arrItemDetLinRef[4] + "' />";

                        //Origen
                        var oCell = newRow.insertCell();
                        oCell.style.width = '120px';
                        oCell.align = 'center';
                        oCell.innerHTML = "<input  class='Arial10B' readonly  style='width:100px;text-align:center;border:0;' name='txtOrigen" + idFilaFCRef + "' id='txtOrigen" + idFilaFCRef + "' value='" + arrItemDetLinRef[1] + "' />";                   

                        var estadoAfiliacion = arrItemDetLinRef[3];
                        var bloqueoDesafiliacion = estadoAfiliacion == "1" ? "disabled" : "";

                        var oCell = newRow.insertCell();
                        oCell.style.width = '90px';
                        oCell.align = 'center';
                        oCell.innerHTML = "<input type='checkbox' onclick='getRowValuesForUseCard(" + idFilaFCRef + ")' class='Arial10B' style='width:90px;text-align:center;border:0;' id='chkUsarTarjeta" + idFilaFCRef + "' />";
                        if (arrItemDetLinRef[6] != '3' || nuevaTarjetaMarcada) $('#chkUsarTarjeta' + idFilaFCRef).attr('disabled', true);
                        oCell.innerHTML += "<input type='hidden' id='hidIdAfiliacion" + idFilaFCRef + "' value='" + arrItemDetLinRef[5] + "' />";
                        oCell.innerHTML += "<input type='hidden' id='hidOrigen" + idFilaFCRef + "' value='" + arrItemDetLinRef[6] + "' />";
                        oCell.innerHTML += "<input type='hidden' id='hidTarjetaId" + idFilaFCRef + "' value='" + arrItemDetLinRef[7] + "' />";                        

                    }

                }
                setValue('hidCantRows', idFilaFCRef);
            }
        }

        //PROY-140657 FIN

        function CaracterPermitidoEmail(e) {
            var tecla = window.event.keyCode;
            if (tecla == 8) return true;
            patron = /[A-Za-z\s  0-9 @ . _]/;
            te = String.fromCharCode(tecla);
            return patron.test(te);
        }

        function validarEmail(valor) {
            var regex = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,4})+$/;
            if (regex.test(valor)) {
                return true;
            } else {
                return false;
            }
        }

        //INI INICIATIVA 941 - IDEA 142525
        function enviarLinkAfiliacion() {
            var _correoContacto = $('#txtCorreo').val();
            var _telefonoContacto = $('#txtNumeroNot').val();
            var _idAfiliacionPrevio = $('#hidnIdAfiliacionPrevio').val();

            $.ajax({
                type: "POST",
                url: "sisact_pop_debito_automatico.aspx/EnviarLinkAfiliacionDebito",
                data: "{'correoContacto':'" + _correoContacto + "', 'telefonoContacto':'" + _telefonoContacto + "', 'idAfiliacionPrevio':'" + _idAfiliacionPrevio + "'}", //JSON.stringify(objRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                cache: false,
                success: function (objResponse) {
                    alert("Envio de Link para afiliación al débito automático, " + objResponse.d.mensajeRespuesta); //result = objResponse;
                    if (objResponse.d.codigoRespuesta === "0") {
                        window.close();
                        return;
                    }
                },
                error: function (objResponse) {
                    alert("Error al intentar enviar el Link para afiliación al débito automático");
                }
            });
        }
        //FIN INICIATIVA 941 - IDEA 142525

        //INI INICIATIVA 941 - IDEA 142525
        function window.confirm(str) {
            execScript('n = msgbox("' + str + '","4132")', "vbscript");
            return (n == 6);
        }
        //FIN INICIATIVA 941 - IDEA 142525


        function validarEsLineaClaro() {
            var _telefono = $('#txtNumeroNot').val();
            

            $.ajax({
                type: "POST",
                url: "sisact_pop_debito_automatico.aspx/wbValidarLineaClaro",
                data: "{'numeroNotificar':'" + _telefono + "'}", //JSON.stringify(objRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                cache: false,
                success: function (objResponse) {
                    if (!objResponse.d.esClaro) {
                        if (confirm(objResponse.d.mensajeInformativo)) {
                            return;
                        }
                        else {
                            enviarLinkAfiliacion();
                        }
                    }
                    else {
                        enviarLinkAfiliacion();
                    }
                },
                error: function (objResponse) {
                    alert("Error al consultar la linea.");
                }
            });
        }

        // INI INICIATIVA 941
        var columnaUsarTarjetaEstaMarcada = false;
        function getRowValuesForUseCard(idFilaSeleccionada) {
            var marcarCheck = $("#chkUsarTarjeta" + idFilaSeleccionada).is(":checked");

            if (marcarCheck && !columnaUsarTarjetaEstaMarcada) {
                if (confirm('¿Esta seguro que quiere utilizar la tarjeta ' + $('#txtNroTarjeta' + idFilaSeleccionada).val() + '?')) {
                    document.getElementById('tdRowUsarTarjeta').style.display = '';
                    document.getElementById('tdLabelUsarTarjeta').innerHTML = 'Se esta reutilizando una tarjeta';
                    setValue('btnAceptar', 'Usar Tarjeta');
                    columnaUsarTarjetaEstaMarcada = true;
                    nuevaTarjetaMarcada = false;                  
                } else {
                    $("#chkUsarTarjeta" + idFilaSeleccionada).prop("checked", false);
                    setValue('idAfiliacion', '');
                    setValue('idTarjeta', '');
                    setValue('nroTarjeta', '');
                    return;                  
                }
            } 
            else if (marcarCheck && columnaUsarTarjetaEstaMarcada) {
                $("#chkUsarTarjeta" + idFilaSeleccionada).prop("checked", false);
                alert("No se pueden seleccionar dos tarjetas.");
                return;
            }

            if (!marcarCheck) {
                columnaUsarTarjetaEstaMarcada = false;
                document.getElementById('tdRowUsarTarjeta').style.display = 'none';
                setValue('btnAceptar', 'Aceptar');
                setValue('idAfiliacion', '');
                setValue('idTarjeta', '');
                setValue('nroTarjeta', '');                
                return;
            }

            var idAfiliacionSeleccionado = $('#hidIdAfiliacion' + idFilaSeleccionada).val();
            var idOrigenAfiliacionSeleccionado = $('#hidOrigen' + idFilaSeleccionada).val();
            var tarjetaIdSeleccionado = $('#hidTarjetaId' + idFilaSeleccionada).val();
            var nroTarjetaSeleccionado = $('#txtNroTarjeta' + idFilaSeleccionada).val();
            setValue('idAfiliacion', idAfiliacionSeleccionado);
            setValue('idTarjeta', tarjetaIdSeleccionado);
            setValue('nroTarjeta', nroTarjetaSeleccionado);


        }

        var nuevaTarjetaMarcada = false;
        function desactivateTblAfiliaciones() {
            var isChkNuevaTarjeta = $('#chkNuevaTarjeta').is(':checked');
            if (isChkNuevaTarjeta) {
                for (var i = 0; i <= $('#hidCantRows').val(); i++) {
                    var prueba = $('#hidCantRows').val();
                    if ($('#chkUsarTarjeta' + i).is(':checked')) $("#chkUsarTarjeta" + i).prop("checked", false);
                    $('#chkUsarTarjeta' + i).attr('disabled', true);
                }
                document.getElementById('tdRowUsarTarjeta').style.display = '';
                document.getElementById('tdLabelUsarTarjeta').innerHTML = 'Se esta afiliando una nueva tarjeta';
                setValue('btnAceptar', 'Enviar Link');
                nuevaTarjetaMarcada = true;
            } else {
                for (var i = 1; i <= $('#hidCantRows').val(); i++) {
                    if ($('#hidOrigen' + i).val() === "3") $('#chkUsarTarjeta' + i).attr('disabled', false);
                }
                document.getElementById('tdRowUsarTarjeta').style.display = 'none';
                setValue('btnAceptar', 'Aceptar');
                nuevaTarjetaMarcada = false;
            }
            setValue('idAfiliacion', '');
            setValue('idTarjeta', '');
            setValue('nroTarjeta', '');
            columnaUsarTarjetaEstaMarcada = false;
        }

        // FIN INICIATIVA 941

    </script>
</head>
<body onload="inicio()">
    <form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" EnablePartialRendering="true">
    </asp:ScriptManager>
    <table cellspacing="0" cellpadding="0" width="100%" align="center">
        <tr>
            <td class="Header" style="font-size: 11px" align="center" colspan="5">
                Registrar Afiliación / Desafiliación Individuales
            </td>
        </tr>
    </table>
    <table class="Contenido" cellspacing="2" cellpadding="2" width="100%" align="center">
        <tr>
            <td class="Arial11BVNegrita" colspan="5">
                DATOS DEL CLIENTE
            </td>
        </tr>
        <tr>
            <td class="Arial10B" style="width:150px;">
                Nombre del Cliente:
            </td>
            <td class="Arial10B" colspan="5">
                <asp:TextBox onpaste="return false;" ID="txtNombreCli" runat="server" CssClass="clsInputDisable"
                    Width="360px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="Arial10B">
                Tipo de Documento:
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoDoc" runat="server" CssClass="clsSelectDisable" Width="200px"
                    Enabled="false">
                </asp:DropDownList>
            </td>
            <td class="Arial10B" colspan="2" align="right">
                Número de Documento:
            </td>
            <td class="Arial10B" colspan="2">
                <asp:TextBox onpaste="return false;" ID="txtnumDocCli" runat="server" CssClass="clsInputDisabled"
                    Width="200px" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="width: 5px;">
            </td>
        </tr>
        <tr>
            <td class="clsArial10B" colspan="6">
                DATOS DE CUENTA/TARJETA
            </td>
        </tr>
        <tr>
            <td class="Arial10B">
                Origen de Solicitud:
            </td>
            <td id="td_rbtCuenta" class="Arial10B" style="width: 110px;" align="center">
                <input class="clsArial10" id="rbtCuenta" onclick="f_cambiarSolicitud(this.value)"
                    type="radio" value="-3" name="rbtTipoPlazo" runat="server" />Cuenta Bancaria
            </td>
            <td id="td_rbTarjeta" class="Arial10B" style="width: 110px;" align="center">
                <input class="clsArial10" id="rbtTarjeta" onclick="f_cambiarSolicitud(this.value)"
                    type="radio" value="-2" name="rbtTipoPlazo" runat="server" />Tarjeta
            </td>
            <td colspan="3"></td>
        </tr>
        <tr id="trDTarjeta1"><%--PROY-140657--%>
            <td class="Arial10B">
                Entidad:
            </td>
            <td colspan="2">
                <asp:DropDownList ID="ddlEntidad" runat="server" CssClass="clsSelectEnable0" Width="200px"
                    onchange="cambiarTipoEntidad(this);">
                </asp:DropDownList>
            </td>
            <td style="width: 5px;">
            </td>
            <td class="Arial10B">
                Tipo de Cuenta/Tarjeta:
            </td>
            <td colspan="2">
                <asp:DropDownList ID="ddlTipoCuenta" runat="server" CssClass="clsSelectEnable0" Width="200px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trDTarjeta2"><%--PROY-140657--%>
            <td class="Arial10B">
                Num. cuenta/tarjeta:
            </td>
            <td colspan="2">
                <input class="clsInputEnabled" onkeypress="validaCaracteres('0123456789')" id="txtNumCuenta"
                    maxlength="16" style="width: 200px;" />
            </td>
            
            <td class="Arial10B" id="tdFecha">
                Fecha de Venc:
            </td>
            <td id="tddFecha" class="Arial10B" colspan="2">
                <input class="clsInputEnabled" onkeypress="validaCaracteres('0123456789')" id="txtFechaVenc"
                    maxlength="4" style="width: 100px;" />MM/YY
            </td>
        </tr>
        <tr>
            <td class="Arial10B">
                Monto Max:
            </td>
            <td class="Arial10B" colspan="2" align="center">
                <input class="clsArial10" id="rbtSinMonto" onclick="f_cambiarMonto(this.value)" type="radio"
                    value="S" name="rbtSinMonto" runat="server" />Sin Monto
                <input class="clsArial10" id="rbtConMonto" onclick="f_cambiarMonto(this.value)" type="radio"
                    value="C" name="rbtConMonto" runat="server" />Con Monto
            </td>
            <td colspan="1"><%--PROY-140657--%>
                <input class="clsInputDisable" onkeypress="validaCaracteres('0123456789')" id="txtMontoMax"
                    maxlength="6" style="width: 100px;" readonly="readonly" />
            </td>
            <%--PROY-140657 INI--%>
            <td> 
               <input type="button" id="BtnConsultar" class="Boton" onclick="f_ConsultarAfiliados()" value="Consultar" style="cursor: hand; width:80px;" />
            </td>
            <td id="tdNoRegistros" style="display: none" align="left">
                <asp:Label id="lblNoRegistros" class="Arial10BRed" runat="server">&nbsp;</asp:Label>
            </td>
            <%--PROY-140657 FIN--%>
        </tr>
        <tr>     
     <td  id="td_TAfiliados" colspan="6">
						<div class="clsGridRow" id="divTableAfiliados">							
						</div>						
            </td>
        </tr>
        <tr>
            <td class="Arial10B">
                <input type = "checkbox" style='width:20px;text-align:left;border:0;' id="chkNuevaTarjeta" onclick="desactivateTblAfiliaciones()"/>
                Nueva Tarjeta
            </td>
            <td class="Arial10B">             
            </td>
            <td id="tdRowUsarTarjeta" class="Arial10B" style="display: none" align="left">
                <asp:Label id="tdLabelUsarTarjeta" class="Arial10BRed" runat="server">&nbsp;</asp:Label>
            </td>
        </tr>
        <tr>
            <td class="Arial10B">
                Correo Electrónico:
            </td>
            <td class="Arial10B" colspan="5">
                <input class="clsInputEnabled" style="text-transform: uppercase; width: 200px;" onkeypress="return CaracterPermitidoEmail(event);"
                    id="txtCorreo" />
            </td>
        </tr>
        <tr>
            <td class="Arial10B">
                Teléfono a Notificar:
            </td>
            <td class="Arial10B" colspan="1">
                <input class="clsInputEnabled" onkeypress="validaCaracteres('0123456789')" id="txtNumeroNot"
                    maxlength="9" style="width: 200px;" />
            </td>
            <%-- INI INICIATIVA 941 - IDEA 142525 --%>
            <td> 
               <input type="button" id="btnReenvioLink" class="Boton" onclick="validarEsLineaClaro()" value="Reenviar link afiliación" style="display:none; cursor: hand; width:140px;" />
            </td>
            <%-- FIN INICIATIVA 941 - IDEA 142525 --%>
        </tr>
    </table>
    <table cellspacing="2" cellpadding="2" width="100%" align="center">
        <tr align="center">
            <td colspan="6">
                <input type="button" id="btnAceptar" class="Boton" onclick="Grabar()" value="Aceptar"
                    style="cursor: hand" style="width: 150px;" />
                <input type="button" id="btnCancelar" class="Boton" onclick="retornoValue('C')" value="Cancelar"
                    style="cursor: hand" style="width: 150px;" />
            </td>
        </tr>
    </table>
    <input id="hidIdSolicitud" type="hidden" name="hidIdSolicitud" runat="server" />
    <input id="hidNumeroDocumento" type="hidden" name="hidNumeroDocumento" runat="server" />
    <input id="hidNombresRazonSocial" type="hidden" name="hidNombresRazonSocial" runat="server" />
    <input id="hidTipoDocumento" type="hidden" name="hidTipoDocumento" runat="server" />
    <input id="hidMontoMaximo" type="hidden" name="hidMontoMaximo" runat="server"/>
    <input id="hidMontoMaxCalc" type="hidden" name="hidMontoMaxCalc" runat="server"/>
    <input id="hidOfertaDEAU" type="hidden" name="hidOfertaDEAU" runat="server" />
    <input id="FlagAfiliacionDEAU" type="hidden" name="FlagAfiliacionDEAU" runat="server" />
    <input id="MsjValidacionMontoDEAU" type="hidden" name="MsjValidacionMontoDEAU" runat="server" />
    <input id="MsjCorreoObligatorioDEAU" type="hidden" name="MsjCorreoObligatorioDEAU" runat="server" />
    <input id="flagNoRegistros" type="hidden" name="flagNoRegistros" runat="server" />    
	<input id="FlagConsultarAltaDEAU" type="hidden" name="FlagConsultarAltaDEAU" runat="server" />

    <%-- INI INICIATIVA 941 - IDEA 142525 --%>
     <input id="hidnEnvioLinkFallido" type="hidden" name="hidnTipoOferta" runat="server"/>
     <input id="hidnIdAfiliacionPrevio" type="hidden" name="hidnIdAfiliacionPrevio" runat="server"/>   
     <input id="hidCorreoElectronico" type="hidden" name="hidCorreoElectronico" runat="server"/>  
     <input id="nroTarjeta" type="hidden" name="nroTarjeta" runat="server"/>
     <input id="idAfiliacion" type="hidden" name="idAfiliacion" runat="server"/>
     <input id="idTarjeta" type="hidden" name="idTarjeta" runat="server"/>    
     <input id="hidCantRows" type="hidden" name="hidCantRows" runat="server"/>    
     <%-- FIN INICIATIVA 941 - IDEA 142525 --%>
    </form>
</body>
</html>
