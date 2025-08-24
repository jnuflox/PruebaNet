function mostrarProvincia(ddlDep) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (ddlDep.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = ddlDep.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (ddlDep.id.indexOf("Usc_direccion") != -1) {
        pos_usc = ddlDep.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }

    var dpto_id, provincia, distrito, cad;
    cad = getValue(pos_usc + 'hidProvincias');
    dpto_id = '';
    dpto_id = getValue(pos_usc + 'ddlDepartamento');
    provincia = document.getElementById(pos_usc + 'ddlProvincia');
    distrito = document.getElementById(pos_usc + 'ddlDistrito');

    setValue(pos_usc + 'txtCodigoPostal', '');
    setValue(pos_usc + 'hidDptoId', dpto_id);

    var i, j;
    var oElm;
    for (j = provincia.length - 1; j >= 0; j--)
        provincia.remove(j);

    for (j = distrito.length - 1; j >= 0; j--)
        distrito.remove(j);

    oElm = document.createElement("OPTION");
    oElm.value = "";
    oElm.text = "--Seleccionar--";
    provincia.add(oElm);

    oElm = document.createElement("OPTION");
    oElm.value = "";
    oElm.text = "--Seleccionar--";
    distrito.add(oElm);

    if (dpto_id != '') {
        var aProvincia = cad.split("|")
        var aDatos;
        for (i = 0; i < aProvincia.length; i++) {
            aDatos = aProvincia[i].split(";");
            if (aDatos[2] == dpto_id) {
                oElm = document.createElement("OPTION");
                oElm.value = aDatos[0];
                oElm.text = aDatos[1];
                provincia.add(oElm);
            }
        }
    }
    setValue(pos_usc + 'txtUbigeo', "");
}
function mostrarDistrito(ddlProv) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (ddlProv.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = ddlProv.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (ddlProv.id.indexOf("Usc_direccion") != -1) {
        pos_usc = ddlProv.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }

    var dpto_id, provincia_id, distrito, cad;
    cad = getValue(pos_usc + 'hidDistritos');

    dpto_id = getValue(pos_usc + 'ddlDepartamento');
    provincia_id = getValue(pos_usc + 'ddlProvincia');
    distrito = document.getElementById(pos_usc + 'ddlDistrito');
    setValue(pos_usc + 'txtCodigoPostal', '');

    document.getElementById(pos_usc + 'hidProvinciaId').value = provincia_id;

    var i, j;
    var oElm;
    for (j = distrito.length - 1; j >= 0; j--)
        distrito.remove(j);

    oElm = document.createElement("OPTION");
    oElm.value = "";
    oElm.text = "Seleccione...";
    distrito.add(oElm);

    if (provincia_id != '') {
        var aDistrito = cad.split("|")
        var aDatos;
        for (i = 0; i < aDistrito.length; i++) {
            aDatos = aDistrito[i].split(";");
            if (aDatos[2] == provincia_id) {
                oElm = document.createElement("OPTION");
                oElm.value = aDatos[0];
                oElm.text = aDatos[1];
                oElm.id = aDatos[3];
                distrito.add(oElm);
            }
        }
    }
    setValue(pos_usc + 'txtUbigeo', "");
}

function mostrarUbigeo(ddldist) {
    var pos_usc, ubigeo, departamento, provincia;
    //adicionado cuando es llamado de otro user control
    if (ddldist.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = ddldist.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (ddldist.id.indexOf("Usc_direccion") != -1) {
        pos_usc = ddldist.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }
    departamento = document.getElementById(pos_usc + 'ddlDepartamento');
    provincia = document.getElementById(pos_usc + 'ddlProvincia');

    ubigeo = departamento[departamento.selectedIndex].value + provincia[provincia.selectedIndex].value + ddldist[ddldist.selectedIndex].value;

    if (ddldist[ddldist.selectedIndex].value != "") {
        setValue(pos_usc + 'txtUbigeo', ubigeo);
    } else {
        setValue(pos_usc + 'txtUbigeo', "");
    }
}

function mostrarCodigoPostal(ddldist) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (ddldist.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = ddldist.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (ddldist.id.indexOf("Usc_direccion") != -1) {
        pos_usc = ddldist.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }
    var distrito;
    var codigoPostal = '';
    distrito = document.getElementById(pos_usc + 'ddlDistrito');
    codigoPostal = distrito[distrito.selectedIndex].id;

    if (codigoPostal == '' || codigoPostal == 'undefined') {
        codigoPostal = obtenerCodigoPostal(distrito.value, pos_usc);
        if (codigoPostal == 'undefined') codigoPostal = '';
    }
    setValue(pos_usc + 'txtCodigoPostal', codigoPostal);
    setValue(pos_usc + 'hidDistritoId', distrito.value);

    document.getElementById(pos_usc + 'hidDistritoId').value = ddldist.value;
}
function obtenerCodigoPostal(distrito_id, pos_usc) {
    if (distrito_id == '-1' || distrito_id == '') return '';
    var cad = getValue(pos_usc + 'hidListaCodigoPostal');

    if (cad == '') return '';
    var aDistrito = cad.split("|")
    var i, aDatos;

    for (i = 0; i < aDistrito.length; i++) {
        aDatos = aDistrito[i].split(";");
        if (aDatos[0] == distrito_id) {
            return aDatos[1];
        }
    }
    return '';
}

function sinNumero(chk) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (chk.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = chk.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (chk.id.indexOf("Usc_direccion") != -1) {
        pos_usc = chk.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }
    if (chk.checked == true) {
        setEnabled1(pos_usc + 'txtNroPuerta', false, 'clsInputDisable');
        setEnabled1(pos_usc + 'ddlEdificacion', true, 'clsSelectEnable0');
        setValue(pos_usc + 'txtNroPuerta', getValue(pos_usc + 'hidSinNumero'));
    } else {
        setEnabled1(pos_usc + 'txtNroPuerta', true, 'clsInputEnable');
        setValue(pos_usc + 'ddlEdificacion', '-1');
        setEnabled1(pos_usc + 'ddlEdificacion', false, 'clsSelectDisable');
        setValue(pos_usc + 'txtNroPuerta', '');
        setFocus(pos_usc + 'txtNroPuerta');
    }
    onkeyup_NroPuerta(document.getElementById(pos_usc + 'txtNroPuerta'));
}
function onkeyup_NroPuerta(txt) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (txt.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = txt.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (txt.id.indexOf("Usc_direccion") != -1) {
        pos_usc = txt.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }
    var key = event.keyCode;

    var salida = txt.value;
    salida = eliminaCaracteresNroPuerta(salida);
    if (salida != '') {

        setEnabled1(pos_usc + 'ddlEdificacion', true, '');
        setValue(pos_usc + 'ddlEdificacion', '-1');
        setValue(pos_usc + 'txtManzana', '');
        setValue(pos_usc + 'txtLote', '');
        setEnabled1(pos_usc + 'txtManzana', false, 'clsInputDisable');
        setEnabled1(pos_usc + 'txtLote', false, 'clsInputDisable');
        if (salida != 'S/N')
            setEnabled1(pos_usc + 'ddlEdificacion', false, 'clsSelectDisable');
        else
            setEnabled1(pos_usc + 'ddlEdificacion', true, 'clsSelectEnable0');
    } else {
        setEnabled1(pos_usc + 'ddlEdificacion', false, 'clsSelectDisable');
    }
    var total = contador_d('D', pos_usc);
    if (total > K_CANTIDAD_DIRECCION) {
        salida = salida.substr(0, salida.length - 1);
        setValueHTML(pos_usc + 'lblContadorDireccion', K_CANTIDAD_DIRECCION);
    }
    txt.value = salida;
}

function onchange_prefijo(idusc) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (idusc.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = idusc.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (idusc.id.indexOf("Usc_direccion") != -1) {
        pos_usc = idusc.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }
    setValue(pos_usc + 'txtDireccion', '');
    setValue(pos_usc + 'txtNroPuerta', '');

    var cbo = idusc;

    var total = contador_d('D', pos_usc);
    var prefijo = cbo.value;
    var cambiar = false;
    if (prefijo != '-1') {
        cambiar = true;
    }
    if (prefijo == '')
        cambiar = false;

    if (cambiar == true) {
        if (total > K_CANTIDAD_DIRECCION) {
            setValueHTML(pos_usc + 'lblContadorDireccion', K_CANTIDAD_DIRECCION);
            cbo.value = '-1';
            return;
        } else {
            total = parseInt(total, 10) - 2;
        }

        setEnabled1(pos_usc + 'txtDireccion', true, 'clsInputEnable');
        setEnabled1(pos_usc + 'txtNroPuerta', true, 'clsInputEnable');
        setEnabled1(pos_usc + 'divSinNumero', true, ''); //panelSinNumero
    } else {
        setEnabled1(pos_usc + 'txtDireccion', false, 'clsInputDisable');
        setEnabled1(pos_usc + 'txtNroPuerta', false, 'clsInputDisable');
        setEnabled1(pos_usc + 'divSinNumero', false, '');
        setValue(pos_usc + 'txtManzana', '');
        setValue(pos_usc + 'txtLote', '');
        setValue(pos_usc + 'ddlEdificacion', '-1');
        setEnabled1(pos_usc + 'ddlEdificacion', false, 'clsSelectDisable');
        setEnabled1(pos_usc + 'txtManzana', false, 'clsInputDisable');
        setEnabled1(pos_usc + 'txtLote', false, 'clsInputDisable');
        document.getElementById(pos_usc + 'chkSinNumero').checked = false;
    }
    if (total == 0) total = '';
    setValueHTML(pos_usc + 'lblContadorDireccion', total);
}
function onkeyup_direccion(txt, cbo_id, flagReferencia) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (txt.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = txt.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (txt.id.indexOf("Usc_direccion") != -1) {
        pos_usc = txt.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }
    var key = event.keyCode;

    var maximo = 40;
    var cbo_des = getText(pos_usc + cbo_id);
    var tamanno_cbo = cbo_des.length + 1;
    var txt_des = Trim(txt.value);
    var tamanno_txt = txt_des.length;
    var total = tamanno_txt + tamanno_cbo;
    var total_cortado = 0;
    if (total >= maximo) {
        total_cortado = maximo - (tamanno_cbo + 1);
        txt.value = txt_des.substring(0, total_cortado);
    }
    var salida = txt.value;
    salida = eliminaCaracteresInvalidos(salida);
    var total = contador_d(flagReferencia, pos_usc);
    if (total > K_CANTIDAD_DIRECCION) {
        salida = salida.substr(0, salida.length - 1);
        if (flagReferencia == 'D')
            setValueHTML(pos_usc + 'lblContadorDireccion', K_CANTIDAD_DIRECCION);
        else
            setValueHTML(pos_usc + 'lblContadorReferencia', K_CANTIDAD_DIRECCION);
    }
    txt.value = salida;
}
function onchange_edificacion(cbo) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (cbo.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = cbo.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (cbo.id.indexOf("Usc_direccion") != -1) {
        pos_usc = cbo.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }
    setValue(pos_usc + 'txtManzana', '');
    setValue(pos_usc + 'txtLote', '');
    var total = contador_d('D', pos_usc);

    if (cbo.value != '-1') {
        if (total > K_CANTIDAD_DIRECCION) {
            setValueHTML(pos_usc + 'lblContadorDireccion', K_CANTIDAD_DIRECCION);
            cbo.value = '-1';
            return;
        } else {
            total = parseInt(total, 10) - 3;
            if (total == 0) total = '';
            setValueHTML(pos_usc + 'lblContadorDireccion', total);
        }
        setEnabled1(pos_usc + 'txtManzana', true, 'clsInputEnable');
        setEnabled1(pos_usc + 'txtLote', true, 'clsInputEnable');
    } else {
        setEnabled1(pos_usc + 'txtManzana', false, 'clsInputDisable');
        setEnabled1(pos_usc + 'txtLote', false, 'clsInputDisable');
    }
}
function onkeyup_mz_lote(txt, flagReferencia) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (txt.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = txt.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (txt.id.indexOf("Usc_direccion") != -1) {
        pos_usc = txt.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }
    //gaa20120206
    if (txt.id.indexOf("Usc_direccionInstalacion") != -1 && flagReferencia == 'R')
        K_CANTIDAD_DIRECCION = 236;
    else
        K_CANTIDAD_DIRECCION = 40;
    //fin gaa20120206
    if (document.all) {
        var salida = txt.value;
        salida = eliminaCaracteresInvalidos(salida);
        var total = contador_d(flagReferencia, pos_usc);
        if (total > K_CANTIDAD_DIRECCION) {
            salida = salida.substr(0, salida.length - 1);
            if (flagReferencia == 'D')
                setValueHTML(pos_usc + 'lblContadorDireccion', K_CANTIDAD_DIRECCION);
            else
                setValueHTML(pos_usc + 'lblContadorReferencia', K_CANTIDAD_DIRECCION);
        }
        txt.value = salida;
    }
    //gaa20120206
    K_CANTIDAD_DIRECCION = 40;
    //fin gaa20120206
}

function onchange_interior(cbo) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (cbo.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = cbo.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (cbo.id.indexOf("Usc_direccion") != -1) {
        pos_usc = cbo.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }

    setValue(pos_usc + 'txtNroInterior', '');
    var total = contador_d('D', pos_usc);
    if (cbo.value != '-1') {
        if (total > K_CANTIDAD_DIRECCION) {
            setValueHTML(pos_usc + 'lblContadorDireccion', total);
            cbo.value = '-1';
            return;
        } else {
            total = parseInt(total, 10) - 4;
            if (total == 0) total = '';
            setValueHTML(pos_usc + 'lblContadorDireccion', total);
        }
        setEnabled1(pos_usc + 'txtNroInterior', true, 'clsInputEnable');
    } else {
        setEnabled1(pos_usc + 'txtNroInterior', false, 'clsInputDisable');
    }
}
function onchange_urbanizacion(cbo) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (cbo.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = cbo.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (cbo.id.indexOf("Usc_direccion") != -1) {
        pos_usc = cbo.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }

    //gaa20120206
    if (cbo.id.indexOf("Usc_direccionInstalacion") != -1)
        K_CANTIDAD_DIRECCION = 236;
    else
        K_CANTIDAD_DIRECCION = 40;
    //fin gaa20120206

    setValue(pos_usc + 'txtUrbanizacion', '');
    var total = contador_d('R', pos_usc);
    if (cbo.value != '-1') {
        if (total > K_CANTIDAD_DIRECCION) {
            setValueHTML(pos_usc + 'lblContadorReferencia', K_CANTIDAD_DIRECCION);
            cbo.value = '-1';
            return;
        } else {
            total = parseInt(total, 10) - 4;
            if (total == 0) total = '';
            //setValueHTML(pos_usc + 'lblContadorReferencia',total);
        }
        setEnabled1(pos_usc + 'txtUrbanizacion', true, 'clsInputEnable');
    } else {
        setEnabled1(pos_usc + 'txtUrbanizacion', false, 'clsInputDisable');
    }
    //gaa20120206
    K_CANTIDAD_DIRECCION = 40;
    //fin gaa20120206
}
function onchange_zona(cbo) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (cbo.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = cbo.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (cbo.id.indexOf("Usc_direccion") != -1) {
        pos_usc = cbo.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }

    //gaa20120206
    if (cbo.id.indexOf("Usc_direccionInstalacion") != -1)
        K_CANTIDAD_DIRECCION = 236;
    else
        K_CANTIDAD_DIRECCION = 40;
    //fin gaa20120206
    setValue(pos_usc + 'txtNombreZona', '');
    var total = contador_d('R', pos_usc);
    if (cbo.value != '-1') {
        if (total > K_CANTIDAD_DIRECCION) {
            setValueHTML(pos_usc + 'lblContadorReferencia', K_CANTIDAD_DIRECCION);
            cbo.value = '-1';
            return;
        } else {
            total = parseInt(total, 10) - 4;
            if (total == 0) total = '';
            setValueHTML(pos_usc + 'lblContadorReferencia.ClientId', total);
        }
        setEnabled1(pos_usc + 'txtNombreZona', true, 'clsInputEnable');
    } else {
        setEnabled1(pos_usc + 'txtNombreZona', false, 'clsInputDisable');
    }
    //gaa20120206
    K_CANTIDAD_DIRECCION = 40;
    //fin gaa20120206				
}

function contador_d(flagReferencia, pos_usc) {
    var completaD = '';
    var completaR = '';
    var total = '';

    if (flagReferencia == 'D') {
        var tipoVia = getValue(pos_usc + 'ddlPrefijo');
        var nombreVia = getValue(pos_usc + 'txtDireccion');
        var nroPuerta = getValue(pos_usc + 'txtNroPuerta');
        var tipoEdificacion = getValue(pos_usc + 'ddlEdificacion');
        var nroEdificacion = getValue(pos_usc + 'txtManzana');
        var lote = getValue(pos_usc + 'txtLote');
        var tipoInterior = getValue(pos_usc + 'ddlTipoInterior');
        var nroInterior = getValue(pos_usc + 'txtNroInterior');
        if (tipoVia != '-1') completaD = 'XX';
        if (nombreVia != '') completaD += ' ' + nombreVia;
        if (nroPuerta != '') completaD += ' ' + nroPuerta;
        if (tipoEdificacion != '-1') completaD += ' ' + tipoEdificacion;
        if (nroEdificacion != '') completaD += ' ' + nroEdificacion;
        if (lote != '') completaD += ' LT ' + lote;
        if (tipoInterior != '-1') completaD += ' ' + tipoInterior;
        if (nroInterior != '') completaD += ' ' + nroInterior;

        total = '';
        if (completaD != '') total = completaD.length;
        setValueHTML(pos_usc + 'lblContadorDireccion', total);
    } else if (flagReferencia == 'R') {//Para la Referencia
        var tipoUrbanizacion = getValue(pos_usc + 'ddlUrbanizacion');
        var nombreUrbanizacion = getValue(pos_usc + 'txtUrbanizacion');
        var tipoZona = getValue(pos_usc + 'ddlZona');
        var nombreZona = getValue(pos_usc + 'txtNombreZona');
        var referencia = getValue(pos_usc + 'txtReferencia');
        if (tipoUrbanizacion != '-1') completaR += ' ' + tipoUrbanizacion;
        if (nombreUrbanizacion != '') completaR += ' ' + nombreUrbanizacion;
        if (tipoZona != '-1') completaR += ' ' + tipoZona;
        if (nombreZona != '') completaR += ' ' + nombreZona;
        if (referencia != '') completaR += ' ' + referencia;
        total = '';
        if (completaR != '') total = trim(completaR).length;
        setValueHTML(pos_usc + 'lblContadorReferencia', total);
    }
    if (total == '') total = 0;
    return total;
}


function eliminaCaracteresInvalidos(cadena) {
    var invalidos = "\/°~#+!$%=?¿¡|;*\'\\\""
    var c = "";
    for (var i = 0; i < invalidos.length; i++) {
        c = invalidos.substr(i, 1);
        cadena = cadena.replace(c, "");
    }
    return cadena;
}
function eliminaCaracteresNroPuerta(cadena) {
    var invalidos = "°~#+!$%=?¿¡|;*\'\\\""
    var c = '';
    for (var i = 0; i < invalidos.length; i++) {
        c = invalidos.substr(i, 1);
        cadena = cadena.replace(c, "");
    }
    cadena = cadena.replace('\/\/', "/");
    return cadena;
}
function mostrarUbigeoINEI(ddldist) {
    var pos_usc;
    //adicionado cuando es llamado de otro user control
    if (ddldist.id.indexOf("Usc_direccion_dth") != -1) {
        pos_usc = ddldist.name;
        //pos_usc = pos_usc.split(':')[0] + "_" + pos_usc.split(':')[1] + "_";
        pos_usc = pos_usc.split('$')[0] + "_" + pos_usc.split('$')[1] + "_";
    }
    //-------------------------------------------
    else if (ddldist.id.indexOf("Usc_direccion") != -1) {
        pos_usc = ddldist.name;
        //pos_usc = pos_usc.split(':')[0] + "_";
        pos_usc = pos_usc.split('$')[0] + "_";
    }
    var distrito;
    var ubigeoINEI = '';
    distrito = document.getElementById(pos_usc + 'ddlDistrito');
    if (ddldist.id.indexOf("Usc_direccion_dth") == -1) {
        ubigeoINEI = distrito[distrito.selectedIndex].id;
    }

    if (ubigeoINEI == '' || ubigeoINEI == 'undefined') {
        ubigeoINEI = obtenerUbigeoINEI(distrito.value, pos_usc);
        if (ubigeoINEI == 'undefined') ubigeoINEI = '';
    }
    setValue(pos_usc + 'hidUbigeoINEI', ubigeoINEI);
    setValue(pos_usc + 'hidDistritoId', distrito.value);

    document.getElementById(pos_usc + 'hidDistritoId').value = ddldist.value;
    return ubigeoINEI;
}
function obtenerUbigeoINEI(distrito_id, pos_usc) {
    if (distrito_id == '-1' || distrito_id == '') return '';
    var cad = getValue(pos_usc + 'hidListUbigeoINEI');

    if (cad == '') return '';
    var aDistrito = cad.split("|")
    var i, aDatos;

    for (i = 0; i < aDistrito.length; i++) {
        aDatos = aDistrito[i].split(";");
        if (aDatos[0] == distrito_id) {
            return aDatos[1];
        }
    }
    return '';
}