$(document).ready(function () {

    CambioPorBusqueda();
    closeBeneficio();
    FechaAnterior();

});

function procesar(Id, nroPedido, SolinCodigo) {

    if (confirm('¿Esta seguro de aprobar la excepción de precios?')) {
        cargarImagenEsperando();
        var Flag = 'Aprobar';
        var comentario = "";
        $('#hdFlag').val(Flag);
        $('#hdId').val(Id);
        if (nroPedido != '' && parseInt(nroPedido) > 0) {
            $('#txtnroSolicitud').val(nroPedido);
            $('#lblsolicitud').text('Nro Pedido');
        } else {
            $('#txtnroSolicitud').val(SolinCodigo)
            $('#lblsolicitud').text('Nro de Solicitud');
        }
        $('#txtnroSolicitud').css('background-color', '#95b7f3');
        PageMethods.AprobarAnular(Id, Flag, comentario, ObtenerDatos_Callback);
    }
    else return;
}

function validar(Id, nroPedido, SolinCodigo) {

    if (confirm('¿Esta seguro de validar la excepción de precios?')) {
        cargarImagenEsperando();
        var Flag = 'Validar';
        var comentario = "";
        $('#hdFlag').val(Flag);
        $('#hdId').val(Id);
        if (nroPedido != '' && parseInt(nroPedido) > 0) {
            $('#txtnroSolicitud').val(nroPedido);
            $('#lblsolicitud').text('Nro Pedido');
        } else {
            $('#txtnroSolicitud').val(SolinCodigo)
            $('#lblsolicitud').text('Nro de Solicitud');
        }
        $('#txtnroSolicitud').css('background-color', '#95b7f3');
        PageMethods.AprobarAnular(Id, Flag, comentario, ObtenerDatos_Callback_validar);
    }
    else return;
}

function rechazar(Id, nroPedido, SolinCodigo) {

    var Flag = 'Anular';
    $('#hdFlag').val(Flag);
    $('#hdId').val(Id);
    $('#PanelCarga').show();
    $('.ModalPopupBG').show();
    if (nroPedido != '' && parseInt(nroPedido) > 0) {
        $('#txtnroSolicitud').val(nroPedido);
        $('#lblsolicitud').text('Nro Pedido');
    } else {
        $('#txtnroSolicitud').val(SolinCodigo)
        $('#lblsolicitud').text('Nro de Solicitud');
    }

    $('#txtnroSolicitud').css('background-color', '#95b7f3');
}

function AprobarAnular() {
    if (getValue('txtComentario') == '') {
        alert('Debe ingresar un motivo de rechazo')
        return;
    }
    if (!confirm('¿Esta seguro de anular la solicitud de excepcion de precios?')) {
        return;
    }
    cargarImagenEsperando();
    var flag = $('#hdFlag').val();
    var Id = $('#hdId').val();
    var comentario = $('#txtComentario').val();
    PageMethods.AprobarAnular(Id, flag, comentario, ObtenerDatos_Callback_anular);
}

function ObtenerDatos_Callback(objResponse) {
    if (objResponse == true) {
        quitarImagenEsperando();
        alert("La Solicitud a sido aprobado con exito!")
        var fechaG = $('#hdFechaG').val()
        $('#txtFecha').val(fechaG);
        $('#btnConsultar').click();
    }
    else {
        quitarImagenEsperando();
        alert('Ha Ocurrido un error inesperado en la aplicacion')
    }
}

function ObtenerDatos_Callback_validar(objResponse) {
    if (objResponse == true) {
        quitarImagenEsperando();
        alert("La Solicitud a sido Validado con exito!")
        var fechaG = $('#hdFechaG').val()
        $('#txtFecha').val(fechaG);
        $('#btnConsultar').click();
    }
    else {
        quitarImagenEsperando();
        alert('Ha Ocurrido un error inesperado en la aplicacion')
    }
}

function ObtenerDatos_Callback_anular(objResponse) {
    if (objResponse == true) {
        quitarImagenEsperando();
        alert("La Solicitud a sido anulado con exito!")
        var fechaG = $('#hdFechaG').val()
        $('#txtFecha').val(fechaG);
        $('#btnConsultar').click();
    }
    else {
        quitarImagenEsperando();
        alert('Ha Ocurrido un error inesperado en la aplicacion')
    }
}

function ValidarConsulta() {

    var valorbusqueda = $('#dllTipoBusqueda').val()
    if (valorbusqueda != "") {
        var txtbusqueda = $("#txtbusqueda").val();
        if (parseInt(txtbusqueda) == 0 || txtbusqueda == "" || txtbusqueda == undefined) {
            alert('Ingrese un número de solicitud válido');
            return false;
        }
    }
    return true;
}

function validaNumeros(evt) {
    var code = (evt.which) ? evt.which : evt.keyCode;

    if (code == 8) {
        return true;
    } else if (code >= 48 && code <= 57) {
        return true;
    } else {
        return false;
    }
}

function Correcto() {
    frmPrincipal.submit
    var fechaG = $('#hdFechaG').val()
    $('#txtFecha').val(fechaG);
    $('#btnConsultar').click();
}

function FechaAnterior() {
    var fechaG = $('#hdFechaG').val()
    $('#txtFecha').val(fechaG);
}

function funcionRetornaConsultar() {

    var valorbusqueda = $('#dllTipoBusqueda').val()
    if (valorbusqueda == "") {
        document.getElementById("txtbusqueda").disabled = true;
        $("#txtbusqueda").val("");
    }
    else {
        document.getElementById("txtbusqueda").disabled = false;
    }
}

function CambioPorBusqueda() {

    $('#dllTipoBusqueda').on('change', function () {

        var valorbusqueda = $('#dllTipoBusqueda').val()
        if (valorbusqueda == "") {
            document.getElementById("txtbusqueda").disabled = true;
            $("#txtbusqueda").val("");

        }
        else {
            document.getElementById("txtbusqueda").disabled = false;
            $("#txtbusqueda").val("");
        }
    })
}

function closeBeneficio() {
    $('#txtComentario').val('');
    $('#PanelCarga').hide();
    $('.ModalPopupBG').hide();
}

