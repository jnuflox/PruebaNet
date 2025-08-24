function obtenerRepresentanteSeleccionado() {
    var tbl = document.getElementById('dgRepresentanteLegal');
    var filas = tbl.rows.length;
    var lista_salida = '';
    for (var i = 0; i < filas - 1; i++) {
        var control = 'dgRepresentanteLegal_chkRepresentanteLegal_' + i;
        var chk = document.getElementById(control);
        //INI PROY-31636
        if (chk != null) {
            if (chk.checked) {
                if ($("#dgRepresentanteLegal_ddlNacionalidad_" + i).val() == "") {
                    return '-1';
                }
                var controlNumero = 'dgRepresentanteLegal_lblNumero_' + i;
                var controlTipoDocumento = 'dgRepresentanteLegal_hidTipoDocumento_' + i;
                var controlNombre = 'dgRepresentanteLegal_lblNombre_' + i;
                var controlApePaterno = 'dgRepresentanteLegal_lblApellidoPaterno_' + i;
                var controlApeMaterno = 'dgRepresentanteLegal_lblApellidoMaterno_' + i;
                var controlCargo = 'dgRepresentanteLegal_lblCargo_' + i;
                var controlNacionalidad = 'dgRepresentanteLegal_ddlNacionalidad_' + i;

                var numero = getValueHTML(controlNumero);
                var tipoDocumento = getValue(controlTipoDocumento);
                var nombre = getValueHTML(controlNombre).replace("'", "");
                var apePaterno = getValueHTML(controlApePaterno).replace("'", "");
                var apeMaterno = getValueHTML(controlApeMaterno).replace("'", "");
                var cargo = getValueHTML(controlCargo);
                var codNacionalidad = getValue(controlNacionalidad);
                var desNacionalidad = getText(controlNacionalidad);
                var datosRepresentante = tipoDocumento + ";" + numero + ";" + nombre + ";" + apePaterno + ";" + apeMaterno + ";" + cargo + ";" + codNacionalidad + ";" + desNacionalidad;
                if (numero != '') {
                    lista_salida += datosRepresentante + "|";
                }
            }
        }
        //FIN PROY-31636
    }
    if (lista_salida != '') {
        lista_salida = lista_salida.substring(0, lista_salida.length - 1);
    }
    return lista_salida;
}

function seleccionarTodo(chk) {
    var xState = chk.checked;
    var theBox = chk;
    var nombre = "_chkRepresentanteLegal";
    var idBox;
    var i = 0;
    elm = theBox.form.elements;
    for (i = 0; i < elm.length; i++) {
        if (elm[i].type == "checkbox" && elm[i].id != theBox.id) {
            idBox = elm[i].id;
            var index = idBox.indexOf(nombre);
            if (index > -1) {
                if (elm[i].checked != xState)
                    elm[i].click();
            }
        }
    }
}

//INI PROY-31636
window.onload = function () {
    var tbl = document.getElementById('dgRepresentanteLegal');
    var filas = tbl.rows.length;
    for (var i = 0; i < filas - 1; i++) {
        var tipoDoc = $("#dgRepresentanteLegal_hidTipoDocumento_" + i).val();
        tipoDoc = tipoDoc.length == 1 ? "0" + tipoDoc : tipoDoc;
        if (tipoDoc == constCodTipoDocumentoDNI) {
            $("#dgRepresentanteLegal_ddlNacionalidad_" + i).val("154");
            $("#dgRepresentanteLegal_ddlNacionalidad_" + i).attr("disabled", true);
        }
        else if (!(tipoDoc == constCodTipoDocumentoRUC)) {
            $("#dgRepresentanteLegal_ddlNacionalidad_" + i + " option[value='154']").remove();
        }
    }
}
//FIN PROY-31636

// PROY-29121-INI
function setearSituacionRRLL(strCadenaEstadoRRLL) {
    var tbl = document.getElementById('dgRepresentanteLegal');
    var filas = tbl.rows.length;
    var i, j;
    var ArrEstadoRRLL = strCadenaEstadoRRLL.split("|")

    for (i = 0; i < filas - 1; i++) {
        var control = 'dgRepresentanteLegal_chkRepresentanteLegal_' + i;
        var chk = document.getElementById(control);
        if (chk != null) {
            var controlNumero = 'dgRepresentanteLegal_lblNumero_' + i;
            var controlTipoDocumento = 'dgRepresentanteLegal_hidTipoDocumento_' + i;
            var numero = getValueHTML(controlNumero);
            var tipoDocumento = getValue(controlTipoDocumento);


            j = 0
            for (j = 0; j < ArrEstadoRRLL.length; j++) {
                var ArrDatos = ArrEstadoRRLL[j].split(";")

                if (ArrDatos[0] == tipoDocumento && ArrDatos[1] == numero) {

                    chk.checked == true
                    var controlaux1 = 'dgRepresentanteLegal_lblSituacion_' + i;
                    var controlaux2 = 'dgRepresentanteLegal_hidMensajeSituacion_' + i;
                    var strImagen = '';

                    if (ArrDatos[2] == document.getElementById('frmPrincipal').hidSituacionOK.value) {
                        strImagen = "<img name='imgSituacion' id='imgSituacion' src='../../Imagenes/aceptar.gif'>"

                    }
                    else {
                        strImagen = "<img name='imgSituacion' id='imgSituacion' src='../../Imagenes/rechazar.gif'>"

                    }
                    setValueHTML(controlaux1, strImagen);
                    //setValue(controlaux2, ArrDatos[3]);
                }
            }

        }
    }
}

function seleccionarItem(chk) {
    //CAMBIOS RATIFICACION PROY-29121
    var strControl1 = chk.id;
    var strControl2 = chk.id;

    var controlaux1 = strControl1.replace("chkRepresentanteLegal", "lblSituacion")
    var controlaux2 = strControl1.replace("chkRepresentanteLegal", "hidMensajeSituacion")

    setValueHTML(controlaux1, "");
    setValue(controlaux2, "");
    //CAMBIOS RATIFICACION PROY-29121

    if (parent.document.getElementById('hidFlagPrimerClic').value) {
        parent.document.getElementById('trCondicionVenta').style.display = 'none';
        var ddl = parent.document.getElementById('ddlTipoOperacion').value;
        window.parent.cambiarTipoOperacion(ddl);
    }

}
//PROY - 29121 - FIN