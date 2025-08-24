// Controles Resultado de Créditos
function cambiarTipoGarantia(valor) {
    $("#gvResultadoCredito tbody tr select").val(valor);
}

function cambiarTipoGarantia2(valor) {
    var tabla = $("table[id*=gvResultadoCredito2]")[1];
    var cont = tabla.rows.length;
    var fila;

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        fila.cells[3].getElementsByTagName("select")[0].value = valor;
    }
}

function validarGarantia() {

    var objTabla = $("table[id*=gvResultadoCredito2]")[1];
    if (getValue('hidFlgConvergente') != 'S') {
        objTabla = document.getElementById('gvResultadoCredito');
    }
    var nroFila = objTabla.rows.length;
    var objFila;
    var idx = 0;
    var iPosNroCf = 6;
    var iPosMonto = 7;

    if (getValue('hidFlgConvergente') != 'S') {
        idx = 2;
        nroFila = nroFila - 1;
        iPosNroCf = iPosNroCf - 1;
        iPosMonto = iPosMonto - 1;
    }

    for (var i = idx; i < nroFila; i++) {
        objFila = objTabla.rows[i];

        if (objFila.cells[iPosNroCf].getElementsByTagName("input")[0].value.length == 0) {
            alert('Ingresar número de cargo fijos.');
            return false;
        }
        if (objFila.cells[iPosMonto].getElementsByTagName("input")[0].value.length == 0) {
            alert('Ingresar monto de garantía.');
            return false;
        }
    }
    return true;
}

function validarCostoInstalacion() {
    if (getValue('hidFlgProductoDTH') == 'S') {
        var objTabla = document.getElementById('dgCostoInstalacion');
        var nroFila = objTabla.rows.length;
        var objFila;

        for (var i = 2; i < nroFila - 1; i++) {
            objFila = objTabla.rows[i];

            if (objFila.cells[4].getElementsByTagName("input")[0].value.length == 0) {
                alert('Ingresar el costo de instalación.');
                return false;
            }
        }
    }

    if (getValue('hidFlgProductoHFC') == 'S') {
        var objTabla = document.getElementById('dgCostoInstalacionHFC');
        var nroFila = objTabla.rows.length;
        var objFila;

        for (var i = 2; i < nroFila - 1; i++) {
            objFila = objTabla.rows[i];

            if (objFila.cells[4].getElementsByTagName("input")[0].value.length == 0) {
                alert('Ingresar el costo de instalación.');
                return false;
            }
        }
    }
    return true;
}

function obtenerGarantia() {
    var objTabla = document.getElementById('gvResultadoCredito');
    var nroFila = objTabla.rows.length;
    var objFila;
    var nroSEC, tipoGarantia, nroCF, montoGarantia;
    var cadena = '';

    for (var i = 2; i < nroFila - 1; i++) {
        objFila = objTabla.rows[i];

        nroSec = objFila.cells[8].innerHTML;
        tipoGarantia = objFila.cells[2].getElementsByTagName("select")[0].value;
        nroCF = objFila.cells[5].getElementsByTagName("input")[0].value;
        montoGarantia = objFila.cells[6].getElementsByTagName("input")[0].value;

        cadena += nroSec + ';' + tipoGarantia + ';' + nroCF + ';' + montoGarantia + '|';
    }
    return cadena;
}

function obtenerGarantia2() {
    var objTabla = $("table[id*=gvResultadoCredito2]")[1];
    var nroFila = objTabla.rows.length;
    var objFila;
    var nroSEC, slplnCodigo, tipoGarantia, nroCF, montoGarantia;
    var cadena = '';

    for (var i = 0; i < nroFila; i++) {
        objFila = objTabla.rows[i];

        nroSec = objFila.cells[9].innerHTML;
        tipoGarantia = objFila.cells[3].getElementsByTagName("select")[0].value;
        nroCF = objFila.cells[6].getElementsByTagName("input")[0].value;
        montoGarantia = objFila.cells[7].getElementsByTagName("input")[0].value;
        slplnCodigo = objFila.cells[11].innerHTML;

        cadena += nroSec + ';' + tipoGarantia + ';' + nroCF + ';' + montoGarantia + ';' + slplnCodigo + '|';
    }
    return cadena;
}

function obtenerCadenaGarantia() {
    var cadenaGarantia = '';
    if (getValue('hidFlgConvergente') == 'S') {
        cadenaGarantia = obtenerGarantia2();
    } else {
        cadenaGarantia = obtenerGarantia();
    }
    return cadenaGarantia;
}

// INC000003135879 INICIO
function obtenerCadenaCostoInstalacionAnterior() {
    var cadenaCostoInstalacion = '';
    if (getValue('hidFlgProductoDTH') == 'S') {
        cadenaCostoInstalacion = obtenerCostoInstalacionAnterior('DTH');
    }
    if (getValue('hidFlgProductoHFC') == 'S') {
        cadenaCostoInstalacion += obtenerCostoInstalacionAnterior('HFC');
    }
    return cadenaCostoInstalacion;
}

function obtenerCostoInstalacionAnterior(tipo) {
    var objTabla;
    if (tipo == "DTH") {
        objTabla = document.getElementById('dgCostoInstalacion');
    } else if (tipo == "HFC") {
        objTabla = document.getElementById('dgCostoInstalacionHFC');
    }

    var nroFila = objTabla.rows.length;
    var objFila;
    var costoInstalacion;
    var cadena = '';

    for (var i = 2; i < nroFila - 1; i++) {
        objFila = objTabla.rows[i];

        if (tipo == "DTH" || tipo == "HFC") {
            costoInstalacion = objFila.cells[6].getElementsByTagName("input")[0].value;
        }
        else {
            costoInstalacion = objFila.cells[4].getElementsByTagName("input")[0].value;
        }    

        cadena = costoInstalacion;
    }
    return cadena;
}
// INC000003135879 FIN 

function obtenerCadenaCostoInstalacion() {
    var cadenaCostoInstalacion = '';
    if (getValue('hidFlgProductoDTH') == 'S') {
        cadenaCostoInstalacion = obtenerCostoInstalacion('DTH');
    }
    if (getValue('hidFlgProductoHFC') == 'S') {
        cadenaCostoInstalacion += obtenerCostoInstalacion('HFC');
    }
    return cadenaCostoInstalacion;
}

//PROY-29215 INICO
function obtenerCadenaFomarPagoCuota(valor) {
    var cadenaFormaPagoCuota = '';
    if (getValue('hidFlgProductoDTH') == 'S') {
        cadenaFormaPagoCuota = obtenerFormaPagoCuota('DTH', valor);
    }
    if (getValue('hidFlgProductoHFC') == 'S') {
        cadenaFormaPagoCuota = obtenerFormaPagoCuota('HFC', valor);
    }
    return cadenaFormaPagoCuota;
}
//PROY-29215 FIN

function obtenerCostoInstalacion(tipo) {
    var objTabla;
    if (tipo == "DTH") {
        objTabla = document.getElementById('dgCostoInstalacion');
    } else if (tipo == "HFC") {
        objTabla = document.getElementById('dgCostoInstalacionHFC');
    }

    var nroFila = objTabla.rows.length;
    var objFila;
    var nroSEC, costoInstalacion;
    var cadena = '';

    for (var i = 2; i < nroFila - 1; i++) {
        objFila = objTabla.rows[i];

        //PROY-140546 Inicio
        var nColumnaCostoInstalacion = 0;
        var nColumnaFormaPago = 0;
        if (objFila.cells.length > 9) {
            nColumnaCostoInstalacion = 8;
        }
        else {
            nColumnaCostoInstalacion = 6;
        }
        //PROY-140546 Fin

        nroSec = objFila.cells[0].innerHTML;
        //PROY-29215 INICIO
        if(tipo == "DTH" || tipo == "HFC"){
            costoInstalacion = objFila.cells[nColumnaCostoInstalacion].getElementsByTagName("input")[0].value; //PROY-140546
        }
        else{
            costoInstalacion = objFila.cells[4].getElementsByTagName("input")[0].value;
        }
        //PROY-29215 FIN        

        cadena += nroSec + ';' + costoInstalacion + '|';
    }
    return cadena;
}

//PROY-140546 Cobro Anticipado de Instalacion
function obtenerMAI() {
    var tipo = '';

    if (getValue('hidFlgProductoHFC') == 'S') {
        tipo = 'HFC';
    }

    var objTabla;
    if (tipo == "HFC") {
        objTabla = document.getElementById('dgCostoInstalacionHFC');
    }

    var nroFila = objTabla.rows.length;
    var objFila;
    var nMAI;
    var cadena = '';

    if (nroFila > 1) {
        objFila = objTabla.rows[2];
        if (objFila.cells.length > 10) {
            nMAI = objFila.cells[11].getElementsByTagName("input")[0].value;
        }
        else {
            nMAI = 0;
        }
    }

    return nMAI;
}
//PROY-140546 Cobro Anticipado de Instalacion

//PROY-29215 INICIO
function obtenerFormaPagoCuota(tipo,valor) {
    var objTabla;
    if (tipo == "DTH") {
        objTabla = document.getElementById('dgCostoInstalacion');
    } else if (tipo == "HFC") {
        objTabla = document.getElementById('dgCostoInstalacionHFC');

    }

    var nroFila = objTabla.rows.length;
    var objFila,str, obj;
    var cadena = '';

    for (var i = 2; i < nroFila - 1; i++) {
        objFila = objTabla.rows[i];

        //PROY-140546 Inicio
        var nColumnaCuota = 0;
        var nColumnaFormaPago = 0;
        if (objFila.cells.length > 9) {
            nColumnaCuota = 10;
            nColumnaFormaPago = 9;
        }
        else {
            nColumnaCuota = 8;
            nColumnaFormaPago = 7;
        }
        //PROY-140546 Fin

        if (valor == "FP") {
            obj = objFila.cells[nColumnaFormaPago].getElementsByTagName("select")[0]; //PROY-140546 Cobro
        }
        else if (valor == "C") {
            obj = objFila.cells[nColumnaCuota].getElementsByTagName("select")[0]; //PROY-140546 Cobro
        }

        for (var o = 0; o < obj.length; o++) {
            if (obj[o].selected) {
                 str = obj[o].text;
            }
        }
            cadena += str;
    }
    return cadena;
}
//PROY-29215 FIN

function calcularMontoGarantias(nroCargosFijos, txtMontoGarantia, cargoFijo, tipoProducto, grilla, tipo) {
    var fltResultado = 0;
    if (nroCargosFijos.length == 0)
        nroCargosFijos = 0;

    if (nroCargosFijos.length > 0) {
        fltResultado = cargoFijo * nroCargosFijos * 1;

        if (tipoProducto == constTipoProductoDTH)
            fltResultado = redondeoDTH(fltResultado);
        else
            fltResultado = Math.floor(fltResultado);

        txtMontoGarantia.value = fltResultado;
        txtMontoGarantia.disabled = true;
        txtMontoGarantia.className = 'clsInputDisable';
    }
    else {
        txtMontoGarantia.value = '';
        txtMontoGarantia.disabled = false;
        txtMontoGarantia.className = 'clsInputEnable';
    }

    if (tipo != '2')
        calcularTotalGarantia();
    else
        calcularTotalGarantia2();
}

function calcularNroCargosFijos(montoGarantia, txtMontoGarantia, txtNroCargoFijo, cargoFijo, tipoProducto, grilla, tipo) {
    var fltResultado = 0;
    if (montoGarantia.length > 0) {
        if (tipoProducto == constTipoProductoDTH)
            montoGarantia = redondeoDTH(montoGarantia);

        fltResultado = (montoGarantia / cargoFijo);

        if (fltResultado == Infinity)
            fltResultado = 0;

        fltResultado = fltResultado.toFixed(2);
        txtMontoGarantia.value = montoGarantia;

        txtNroCargoFijo.value = fltResultado;
        txtNroCargoFijo.disabled = true;
        txtNroCargoFijo.className = 'clsInputDisable';
    }
    else {
        txtNroCargoFijo.value = '';
        txtNroCargoFijo.disabled = false;
        txtNroCargoFijo.className = 'clsInputEnable';
    }

    if (tipo != '2')
        calcularTotalGarantia();
    else
        calcularTotalGarantia2();
}

function redondeoDTH(fltValor) {
    var fltResultado = fltValor;
    if (fltValor % 5 > 0)
        fltResultado = 5 * (Math.floor(fltValor / 5) + 1);

    return fltResultado;
}

function calcularTotalGarantia() {
    var tabla = document.getElementById('gvResultadoCredito');
    var cont = tabla.rows.length;
    var fila;
    var montoGarantia;
    var monto = 0;
    var nCelda = 6;

    for (var i = 2; i < cont - 1; i++) {
        fila = tabla.rows[i];
        montoGarantia = fila.cells[nCelda].getElementsByTagName("input")[0].value;

        if (montoGarantia.length == 0)
            montoGarantia = 0;

        monto += parseFloat(montoGarantia);
    }

    fila = tabla.rows[cont - 1];
    fila.cells[nCelda].innerHTML = monto.toFixed(2);

    return monto;
}

function calcularTotalGarantia2() {
    var tabla = $("table[id*=gvResultadoCredito2]")[1];
    var cont = tabla.rows.length;
    var fila;
    var montoGarantia;
    var monto = 0;
    var nCelda = 7;

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];
        montoGarantia = fila.cells[nCelda].getElementsByTagName("input")[0].value;

        if (montoGarantia.length == 0)
            montoGarantia = 0;

        monto += parseFloat(montoGarantia);
    }

    fila = $("table[id*=gvResultadoCredito2]")[2].rows[0];
    fila.cells[nCelda].innerHTML = monto.toFixed(2);

    return monto;
}

function calcularTotalCostoInstalacion(valor) {
    var tabla;
    if (valor == undefined) {
        tabla = document.getElementById('dgCostoInstalacion');
    } else {
        tabla = document.getElementById('dgCostoInstalacionHFC');
    }

    var cont = tabla.rows.length;
    var fila;
    var costoInstalacion;
    var monto = 0;

    for (var i = 2; i < cont - 1; i++) {
        fila = tabla.rows[i];
        costoInstalacion = fila.cells[4].getElementsByTagName("input")[0].value;

        if (costoInstalacion.length == 0)
            costoInstalacion = 0;

        monto += parseFloat(costoInstalacion);
    }

    fila = tabla.rows[cont - 1];
    fila.cells[4].innerHTML = monto.toFixed(2);

    return monto;
}

function copiarNroCargoFijo(valor) {
    if (!valor)
        return false;

    var tabla = $("table[id*=gvResultadoCredito2]")[1];
    var cont = tabla.rows.length;
    var fila;
    var txtCargoFijo;
    var txtNroCargoFijo_id;
    var txtMontoGarantia;
    var montoGarantia;
    var nroCargoFijo;

    for (var i = 0; i < cont; i++) {
        fila = tabla.rows[i];

        txtCargoFijo = fila.cells[6].getElementsByTagName("input")[0];
        txtNroCargoFijo_id = txtCargoFijo.id;
        txtMontoGarantia = fila.cells[7].getElementsByTagName("input")[0];
        montoGarantia = fila.cells[10].innerHTML;
        nroCargoFijo = fila.cells[8].innerHTML;

        if (i == 0) {
            nroCargosFijos = txtCargoFijo.value;

            if (nroCargosFijos.length == 0)
                txtCargoFijo.value = 0;
        }
        else {
            txtCargoFijo.value = nroCargosFijos;
            calcularMontoGarantias(txtCargoFijo.value, txtMontoGarantia, montoGarantia, nroCargoFijo, 'gvResultadoCredito2', '2');
        }
    }
}