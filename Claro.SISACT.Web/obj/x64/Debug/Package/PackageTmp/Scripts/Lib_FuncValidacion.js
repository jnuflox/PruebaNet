//*******************************************************************************
//Abre una pagina nueva 
//*******************************************************************************		
function openwindow(mywindow, name, alto, ancho, izq, arr) {
    window.open(mywindow, name, "status=no,resizable=no,toolbar=no,scrollbars=no,modal=yes,left=" + izq + ",top=" + arr + ",width=" + ancho + ",height=" + alto + "");
}

function openwindowscroll(mywindow, name, alto, ancho, izq, arr) {
    window.open(mywindow, name, "status=no,resizable=yes,toolbar=no,scrollbars=yes,modal=yes,left=" + izq + ",top=" + arr + ",width=" + ancho + ",height=" + alto + "");
}

function openwindowdiagolo(mywindow, name, alto, ancho, izq, arr) {
    window.open(mywindow, name, "status=no,resizable=no,toolbar=no,scrollbars=yes,modal=yes,left=" + izq + ",top=" + arr + ",width=" + ancho + ",height=" + alto + "");
}

function ValidaUsuarioClave(Cadena) {
    seguir = true;
    pos = 0;
    Cad = Cadena.toUpperCase();
    do {
        car = Cad.substr(pos, 1);
        if ((!((car >= "A") && (car <= "Z"))) && (!((car >= "0") && (car <= "9"))) && (car != "_")) {
            seguir = false;
        }
        else
            pos = pos + 1;
    } while ((seguir) && (pos < Cad.length));
    return seguir;
}


function ValidaFechaCadena(cadena, vacio) {
    var sCad;
    eval("sCad=" + cadena + ".value");
    if ((!vacio) && (sCad == '')) {
        alert('El campo no puede estar vacio');
        eval(cadena + ".select()");
        return false;
    }
    if (!ValidaFecha(cadena))
        return false;
    else
        return true;
}

function trim(cadena) {
    while (cadena.substr(0, 1) == " ")
        cadena = cadena.substr(1);
    while (cadena.substr(cadena.length - 1, 1) == " ")
        cadena = cadena.substr(0, cadena.length - 1);
    while (cadena.search(/  /) != -1)
        cadena = cadena.replace("  ", " ");
    return (cadena);
}

function ValidaCadena(cadena, campo, vacio) {
    var sCad;

    eval("sCad=" + cadena + ".value");
    sCad = trim(sCad);
    if ((sCad == null) || (sCad.length == 0) && (!vacio)) {
        eval("" + cadena + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }
    while (sCad.indexOf("'") != -1)
        sCad = sCad.replace("'", "");
    eval(cadena + ".value='" + sCad + "'");
    return true;
}

function numero_veces(cadena, caracter) {
    var numVeces = 0;

    while (cadena.indexOf(caracter, 0) != -1) {
        numVeces++;
        cadena = cadena.substr(cadena.indexOf(caracter, 0) + 1);
    }
    return numVeces;
}

function ValidaFecha(control) {
    var valor, dia, mes, anhio, cadena;
    cadena = eval(control + ".value");
    if (cadena.length == 0)
        return true;
    valor = numero_veces(cadena, "-")
    if (valor != 2) {
        alert("La fecha esta mal ingresada");
        eval(control + ".select()");
        return false;
    }
    dia = cadena.substr(0, cadena.indexOf("-"))
    if ((dia.length > 0) && (dia.length < 3)) {
        if (dia.length < 2) {
            dia = "0" + dia;
        }
    }
    else {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }

    cadena = cadena.substr(cadena.indexOf("-") + 1, cadena.length)

    mes = cadena.substr(0, cadena.indexOf("-"))

    if ((mes.length > 0) && (mes.length < 3)) {
        if (mes.length < 2) {
            mes = "0" + mes;
        }
    }
    else {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }

    anhio = cadena.substr(cadena.indexOf("-") + 1, cadena.length)
    if (anhio.length != 4) {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }

    FechaAux = new Date(parseInt(anhio, 10), parseInt(mes, 10) - 1, parseInt(dia, 10));

    if (FechaAux.getUTCMonth() != (mes - 1)) {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }
    Fecha = dia + "-" + mes + "-" + anhio;
    eval(control + ".value='" + Fecha + "' ");
    return true;
}

function ValidaFechaA(control, vacio) {
    var valor, dia, mes, anhio, cadena;
    cadena = eval(control + ".value");
    if (cadena.length == 0) {
        if (vacio) {
            return true;
        } else {
            alert("Debe ingresar la fecha");
            eval(control + ".select()");
            return false;
        }
    }
    valor = numero_veces(cadena, "/")
    if (valor != 2) {
        alert("La fecha esta mal ingresada");
        eval(control + ".select()");
        return false;
    }
    dia = cadena.substr(0, cadena.indexOf("/"))
    if ((dia.length > 0) && (dia.length < 3)) {
        if (dia.length < 2) {
            dia = "0" + dia;
        }
    }
    else {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }

    cadena = cadena.substr(cadena.indexOf("/") + 1, cadena.length)

    mes = cadena.substr(0, cadena.indexOf("/"))

    if ((mes.length > 0) && (mes.length < 3)) {
        if (mes.length < 2) {
            mes = "0" + mes;
        }
    }
    else {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }

    anhio = cadena.substr(cadena.indexOf("/") + 1, cadena.length)
    if (anhio.length != 4) {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }

    FechaAux = new Date(parseInt(anhio, 10), parseInt(mes, 10) - 1, parseInt(dia, 10));

    if (FechaAux.getUTCMonth() != (mes - 1)) {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }
    Fecha = dia + "/" + mes + "/" + anhio;
    eval(control + ".value='" + Fecha + "' ");
    return true;
}


function ValidaFechaB(control, vacio) {
    var valor, mes, anhio, cadena;
    cadena = eval(control + ".value");
    if (cadena.length == 0) {
        if (vacio) {
            return true;
        } else {
            alert("Debe ingresar la fecha");
            eval(control + ".select()");
            return false;
        }
    }
    valor = numero_veces(cadena, "/")
    if (valor != 1) {
        alert("La fecha esta mal ingresada");
        eval(control + ".select()");
        return false;
    }

    mes = cadena.substr(0, cadena.indexOf("/"))
    if ((mes.length > 0) && (mes.length < 3)) {
        if (mes.length < 2) {
            mes = "0" + mes;
        }
    }
    else {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }

    anhio = cadena.substr(cadena.indexOf("/") + 1, cadena.length)
    if (anhio.length != 4) {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }

    FechaAux = new Date(parseInt(anhio, 10), parseInt(mes, 10) - 1);

    if (FechaAux.getUTCMonth() != (mes - 1)) {
        alert("La fecha esta mal ingresada");
        eval(control + ".focus()");
        return false;
    }
    Fecha = mes + "/" + anhio;
    eval(control + ".value='" + Fecha + "' ");
    return true;
}


function ValidaHora(control) {
    var er_fh = /^(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23)\:([0-5]0|[0-5][1-9])$/
    var cadena;
    var hora, minuto;

    cadena = eval(control + ".value");
    if (cadena.indexOf(":") == -1) {
        hora = cadena.substr(0, cadena.length);
        minuto = "";
    }
    else {
        hora = cadena.substr(0, cadena.indexOf(":"));
        minuto = cadena.substr(cadena.indexOf(":") + 1, cadena.length);
    }
    if (hora.length == 1)
        hora = "0" + hora;
    if (hora.length == 0)
        hora = "00"
    if (minuto.length == 1)
        minuto = minuto + "0";
    if (minuto.length == 0)
        minuto = "00";
    cadena = hora + ":" + minuto;
    eval(control + ".value='" + cadena + "'");

    if (!(er_fh.test(cadena))) {
        alert("El dato en el campo hora no es válido.");
        eval(control + ".value='';");
        eval(control + ".select();");
        return false;
    }
    return true;
}

function CadenaVacia(cadena, mensaje) {
    var sCad;

    eval("sCad=" + cadena + ".value");
    if (sCad == '') {
        alert(mensaje + ' esta vacia, debe ingresarlo')
        eval("sCad=" + cadena + ".select();");
        return true;
    }
    return false;
}

function ValidaHoraA(control) {
    var er_fh = /^(00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|16|17|18|19|20|21|22|23)\:([0-5]0|[0-5][1-9])\:([0-5]0|[0-5][1-9])$/
    var cadena;
    var hora, minuto, segundo;

    cadena = eval(control + ".value");
    if (cadena.indexOf(":") == -1) {
        hora = cadena.substr(0, cadena.length);
        minuto = "";
        segundo = "";
    }
    if (cadena.indexOf(":") != -1) {
        hora = cadena.substr(0, cadena.indexOf(":"));
        minuto = cadena.substr(cadena.indexOf(":") + 1, cadena.length);
        segundo = "";
    }
    if (cadena.indexOf(":", cadena.indexOf(":") + 1) != -1) {
        hora = cadena.substr(0, cadena.indexOf(":"));
        minuto = cadena.substr(cadena.indexOf(":") + 1, cadena.indexOf(":", cadena.indexOf(":") + 1) - 3);
        segundo = cadena.substr(cadena.indexOf(":", cadena.indexOf(":") + 1) + 1, cadena.length);
    }
    if (hora.length == 1)
        hora = "0" + hora;
    if (hora.length == 0)
        hora = "00"
    if (minuto.length == 1)
        minuto = "0" + minuto;
    if (minuto.length == 0)
        minuto = "00";
    if (segundo.length == 1)
        segundo = "0" + segundo;
    if (segundo.length == 0)
        segundo = "00";
    cadena = hora + ":" + minuto + ":" + segundo;
    eval(control + ".value='" + cadena + "'");

    if (!(er_fh.test(cadena))) {
        alert("El dato en el campo hora no es válido.");
        eval(control + ".value='';");
        eval(control + ".select();");
        return false;
    }

    return true;
}

function HoraMayor(control1, control2, campo1, campo2) {
    var cadena1;
    var cadena2;

    eval("cadena1 = " + control1 + ".value");
    eval("cadena2 = " + control2 + ".value");
    if ((cadena1 != '') && (cadena2 != '')) {
        comp1 = cadena1.substr(0, 2) + '' + cadena1.substr(3, 2) + '' + cadena1.substr(6, 2);
        comp2 = cadena2.substr(0, 2) + '' + cadena2.substr(3, 2) + '' + cadena2.substr(6, 2);
        if ((comp1) < (comp2)) {
            alert('' + campo2 + ' no puede ser mayor que ' + campo1 + '');
            eval("" + control2 + ".focus()");
            return false;
        }
    }
    return true;
}

function DiferenciaFecha(valor1, valor2) {
    var diferencia;
    var date1 = new Date(valor1.substr(6, 4), valor1.substr(3, 2), valor1.substr(0, 2));
    var date2 = new Date(valor2.substr(6, 4), valor2.substr(3, 2), valor2.substr(0, 2));
    alert(datediff(date1, date2));
    alert(date2);
    alert(date1);
    return diferencia;
}

function FechaMayor(control1, control2, campo1, campo2) {
    var cadena1;
    var cadena2;

    eval("cadena1 = " + control1 + ".value");
    eval("cadena2 = " + control2 + ".value");
    if ((cadena1 != '') && (cadena2 != '')) {
        comp1 = cadena1.substr(6, 4) + '' + cadena1.substr(3, 2) + '' + cadena1.substr(0, 2);
        comp2 = cadena2.substr(6, 4) + '' + cadena2.substr(3, 2) + '' + cadena2.substr(0, 2);
        if ((comp1) < (comp2)) {
            alert('' + campo2 + ' no puede ser mayor que ' + campo1 + '');
            eval("" + control2 + ".focus()");
            return false;
        }
    }
    return true;
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

function FechaMayorSistema(control1, campo1) {
    var cadena1;
    var cadena2;

    eval("cadena1 = " + control1 + ".value");
    cadena2 = FechaActual();

    if ((cadena1 != '')) {
        comp1 = cadena1.substr(6, 4) + '' + cadena1.substr(3, 2) + '' + cadena1.substr(0, 2);
        comp2 = cadena2.substr(6, 4) + '' + cadena2.substr(3, 2) + '' + cadena2.substr(0, 2);
        if ((comp1) > (comp2)) {
            alert('' + campo1 + ' no puede ser mayor que la fecha actual');
            eval("" + control1 + ".focus()");
            return false;
        }
    }
    return true;
}

function FechaMenorSistema(control1, campo1) {
    var cadena1;
    var cadena2;

    eval("cadena1 = " + control1 + ".value");
    cadena2 = FechaActual();

    if ((cadena1 != '')) {
        comp1 = cadena1.substr(6, 4) + '' + cadena1.substr(3, 2) + '' + cadena1.substr(0, 2);
        comp2 = cadena2.substr(6, 4) + '' + cadena2.substr(3, 2) + '' + cadena2.substr(0, 2);
        if ((comp1) < (comp2)) {
            alert('' + campo1 + ' no puede ser menor que la fecha actual');
            eval("" + control1 + ".focus()");
            return false;
        }
    }
    return true;
}

function FechaMenorSistemaB(control1, campo1) {
    var cadena1;
    var cadena2;

    eval("cadena1 = " + control1 + ".value");
    cadena2 = FechaActual();

    if ((cadena1 != '')) {
        comp1 = cadena1.substr(3, 4) + '' + cadena1.substr(0, 2);
        comp2 = cadena2.substr(6, 4) + '' + cadena2.substr(3, 2);
        if ((comp1) < (comp2)) {
            //alert('' + campo1 +' no puede ser menor o igual que la fecha actual');
            alert('' + campo1 + ' no puede ser menor que la fecha actual');
            eval("" + control1 + ".focus()");
            return false;
        }
    }
    return true;
}

function FechaMenor(control1, control2, campo1, campo2) {
    var cadena1;
    var cadena2;

    eval("cadena1 = " + control1 + ".value");
    eval("cadena2 = " + control2 + ".value");
    if ((cadena1 != '') && (cadena2 != '')) {
        comp1 = cadena1.substr(6, 4) + '' + cadena1.substr(3, 2) + '' + cadena1.substr(0, 2);
        comp2 = cadena2.substr(6, 4) + '' + cadena2.substr(3, 2) + '' + cadena2.substr(0, 2);
        if ((comp1) < (comp2)) {
            alert('' + campo2 + ' debe ser menor o igual que ' + campo1 + '');
            eval("" + control2 + ".focus()");
            return false;
        }
    }
    return true;
}



/*********** Validacion de Numero decimal con expresiones regulares   ***/


function ValidaDecimal(control, mensaje, vacio) {
    var s;
    eval("s = " + control + ".value");
    if ((s == null) || (s.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + mensaje);
        return false;
    }
    if ((s.length == 0) && (vacio)) {
        return true;
    }
    if (/^([0-9]+|[0-9]{1,3}(,[0-9]{3})*)(\.[0-9]{1,2})?$/.test(s))
    { return (true) }
    alert(mensaje + ' contiene caracteres no válidos');
    eval("" + control + ".focus()");
    return false;
}

/************* Validacion de un Numero **********/

function ValidaNumero(control, campo, vacio) {
    var flag = true;
    var i, j, a, cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));
        if (a == ' ') {
            flag = false;
        }

        j = a.charCodeAt(0);
        if (!((j >= 48) && (j <= 57)))
            flag = false;
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".select()")
    }
    return flag;
}

function ValidaNumTarj(control, campo, vacio) {
    var flag = true;
    var i, j, a, cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));
        if (a == ' ') {
            flag = false;
        }

        j = a.charCodeAt(0);
        if (!(((j >= 48) && (j <= 57)) || (j = 45)))
            flag = false;
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".select()")
    }
    return flag;
}

function ValidaDecimalB(control, campo, vacio) {
    var flag = true;
    var i, j, a, cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));
        if (a == ' ') {
            flag = false;
        }

        j = a.charCodeAt(0);
        if (!(((j >= 48) && (j <= 57)) || (j == 45) || (j == 46)))
            flag = false;
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".select()")
    }
    return flag;
}

function ValidaEntero(control, campo, vacio) {
    var flag = true;
    var i, j, a, cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));
        if (a == ' ') {
            flag = false;
        }

        j = a.charCodeAt(0);
        if (!((j >= 48) && (j <= 57)))
            flag = false;
    }
    if (!flag) {
        alert(campo + " debe ingresar un valor entero");
        eval(control + ".select()")
    }
    return flag;
}

function ValidaAlfaNumM(control, campo, vacio) {
    var flag = true;
    var i, j, a, cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));
        if (a == ' ') {
            flag = false;
        }

        j = a.charCodeAt(0);
        if (!(((j >= 48) && (j <= 57)) || ((j > 64) && (j <= 91))))
            flag = false;
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".select()")
    }
    return flag;
}

function ValidaNumRefSunat(control, campo, vacio) {
    var flag = true;
    var i, j, a, cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));
        if (a == ' ') {
            flag = false;
        }

        j = a.charCodeAt(0);
        if (!(((j >= 48) && (j <= 57)) || ((j > 64) && (j <= 91)) || ((j > 96) && (j <= 123)) || (j = 45)))
            flag = false;
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos. Son validos : [A..Z] , [a..z] , [0..9] y -");
        eval(control + ".select()")
    }
    return flag;
}

function ValidaDigitos(control, campo, ndig, vacio) {
    var flag = true;
    var i, j, a, cadena, count;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }
    count = 0;
    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));

        j = a.charCodeAt(0);
        if ((j >= 48) && (j <= 57))
            count++;
    }
    if (count != 0) {
        if (count < ndig) {
            alert(campo + " debe contener al menos " + ndig + " digitos");
            eval(control + ".select()");
            return false;
        }
    }
    return true;
}

function ValidaDigitosB(control, campo, ndig, vacio) {
    var flag = true;
    var i, j, a, cadena, count;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }
    count = 0;
    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));

        j = a.charCodeAt(0);
        if ((j >= 48) && (j <= 57)) {
            count++;
        }
        else {
            flag = false;
        }
    }

    if (!flag) {
        alert(campo + " contiene caracteres no válidos. Son validos : [0..9]");
        eval(control + ".select()");
        return false;
    }

    if (count != 0) {
        if (count < ndig) {
            alert(campo + " debe contener al menos " + ndig + " digitos");
            eval(control + ".select()");
            return false;
        }
    }

    return true;
}


function ValidaCombo(control, campo, vacio) {
    var flag = true;
    var i, j, a, cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == "xx") || (cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".focus()");
        alert('Debe ingresar ' + campo);
        return false;
    }
    return flag;
}
/************* Validacion de Telefono **********/

function ValidaTelefono(control, campo, vacio) {
    var flag = true;
    var i, j, k, a, cadena, count;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    if (!ValidaDigitos(control, campo, 7, vacio)) {
        return false;
    }

    a = (cadena.substr(0, 2));
    if (a == "97") {
        eval("" + control + ".select()");
        alert(campo + ' no puede comenzar en 97');
        return false;
    }

    a = (cadena.substr(0, 3));
    if (a == "097") {
        eval("" + control + ".select()");
        alert(campo + ' no puede comenzar en 097');
        return false;
    }

    a = (cadena.substr(0, 4));
    if (a == "0097") {
        eval("" + control + ".select()");
        alert(campo + ' no puede comenzar en 0097');
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));

        j = a.charCodeAt(0);
        if (!((j >= 45) && (j <= 57) && (j != 46) && (j != 47))) {
            flag = false;
        } else {
            if (j == k) {
                count++;
            } else {
                count = 1;
                k = j;
            }
            if (count >= 4) {
                eval("" + control + ".select()");
                //Modificado para que los telefonos no esten restringidos a 4 digitos
                //alert(campo + ' no puede tener mas de 4 digitos iguales consecutivos ' );
                //return false;
            }
        }
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".select()")
    }
    return flag;
}



function ValidaTelefonoProv(control, campo, vacio) {
    var flag = true;
    var i, j, k, a, cadena, count;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    if (!ValidaDigitos(control, campo, 6, vacio)) {
        return false;
    }

    a = (cadena.substr(0, 2));
    if (a == "97") {
        eval("" + control + ".select()");
        alert(campo + ' no puede comenzar en 97');
        return false;
    }

    a = (cadena.substr(0, 3));
    if (a == "097") {
        eval("" + control + ".select()");
        alert(campo + ' no puede comenzar en 097');
        return false;
    }

    a = (cadena.substr(0, 4));
    if (a == "0097") {
        eval("" + control + ".select()");
        alert(campo + ' no puede comenzar en 0097');
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));

        j = a.charCodeAt(0);
        if (!((j >= 45) && (j <= 57) && (j != 46) && (j != 47))) {
            flag = false;
        } else {
            if (j == k) {
                count++;
            } else {
                count = 1;
                k = j;
            }
            if (count >= 4) {
                eval("" + control + ".select()");
                //Modificado para que los telefonos no esten restringidos a 4 digitos
                //alert(campo + ' no puede tener mas de 4 digitos iguales consecutivos ' );
                //return false;
            }
        }
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".select()")
    }
    return flag;
}

/*********** Validacion de E-Mail (version compacta)********/

function checkEmail(control, campo, vacio) {
    var cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena.length != 0) || (!vacio)) {
        if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
            eval("" + control + ".focus()");
            alert('Debe ingresar ' + campo);
            return false;
        }
        if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(cadena))
        { return (true) }
        alert(campo + " contiene caracteres no válidos o formato incorrecto");
        eval(control + ".select();")
        return false;
    }
    else {
        return true;
    }
}


function ValidaTxtIMEI(control, campo, vacio) {
    var flag = true;
    var i, j, a, cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));
        if (a == " ") {
            flag = false;
        }
        if (a == "'") {
            flag = false;
        }

        j = a.charCodeAt(0);
        if (a != " ") {
            if (!((j >= 48) && (j <= 57)))
                flag = false;
        }
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".focus()")
    }
    return flag;
}


function ValidaAlfanumerico(control, campo, vacio) {
    var flag = true;
    var i, j, a, cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));

        j = a.charCodeAt(0);
        if (a != " ") {
            if (!(((j >= 48) && (j <= 57)) || ((j > 96) && (j <= 123)) || ((j > 64) && (j <= 91)) || (j == 38) || (j == 45) || (j == 209) || (j == 225) || (j == 233) || (j == 237) || (j == 241) || (j == 243) || (j == 250))) flag = false;
        }
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".focus()")
    }
    return flag;
}

function ValidaAlfanumericoGC(control, campo, vacio) {
    var flag = true;
    var i, j, a, cadena;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));

        j = a.charCodeAt(0);
        if (a != " ") {
            if (!(((j >= 48) && (j <= 57)) || ((j > 96) && (j <= 123)) || ((j > 64) && (j <= 91)) || (j == 38) || (j == 45) || (j == 209) || (j == 225) || (j == 233) || (j == 237) || (j == 241) || (j == 243) || (j == 250) || (j == 13) || (j == 10) || (j >= 44 && j <= 47))) flag = false;
        }
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos. Los caracteres válidos son [A-Z], [0-9], [,.-/]");
        eval(control + ".focus()")
    }
    return flag;
}

function ValidaNombre(control, campo, vacio) {
    var flag = true;
    var flag2 = false;
    var i, j, k, a, cadena, count;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    if ((cadena.length < 2) && (!vacio)) {
        eval("" + control + ".select()");
        alert(campo + ' debe tener 2 o mas caracteres ');
        return false;
    }

    if (cadena.length == 2) {
        if (cadena.substr(0, 1) == cadena.substr(1, 1)) {
            eval("" + control + ".select()");
            alert(campo + ' no puede tener 2 letras iguales como valor ');
            return false;
        }

        if ((cadena == "nd") || (cadena == "ND") || (cadena == "nD") || (cadena == "Nd")) {
            eval("" + control + ".select()");
            alert(campo + ' no puede tener el valor ND ');
            return false;
        }
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));

        j = a.charCodeAt(0);
        if (a != " ") {
            if (!(((j > 96) && (j <= 123)) || ((j > 64) && (j <= 91)) || (j == 193) || (j == 201) || (j == 205) || (j == 211) || (j == 218) || (j == 209))) {
                flag = false;
            } else {
                if (((j > 96) && (j <= 123)) || ((j > 64) && (j <= 91)) || (j == 193) || (j == 201) || (j == 205) || (j == 211) || (j == 218) || (j == 209)) {
                    flag2 = true;
                }
                if (j == k) {
                    count++;
                } else {
                    count = 1;
                    k = j;
                }
                if (count >= 3) {
                    eval("" + control + ".select()");
                    alert(campo + ' no puede tener mas de 3 letras iguales consecutivas ');
                    return false;
                }
            }
        } else {
            count = 1;
        }
    }

    if ((!flag2) && (!vacio)) {
        alert(campo + " debe tener al menos una letra");
        eval(control + ".focus()")
        return false;
    }

    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".focus()")
    }

    return flag;
}

function ValidaDireccion(control, campo, vacio) {
    var flag = true;
    var i, j, k, a, cadena, count;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    if ((cadena.length < 2) && (!vacio)) {
        eval("" + control + ".select()");
        alert(campo + ' debe tener 2 o mas caracteres ');
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));

        j = a.charCodeAt(0);
        if (a != " ") {
            if (!(((j >= 46) && (j <= 57)) || ((j > 96) && (j <= 123)) || ((j > 64) && (j <= 91)) || (j == 35) || (j == 35) || (j == 193) || (j == 201) || (j == 205) || (j == 211) || (j == 218) || (j == 209))) {
                flag = false;
            } else {
                if (j == k) {
                    count++;
                } else {
                    count = 1;
                    k = j;
                }



                if (!(j >= 48) && (j <= 57)) {


                    if (count >= 3) {
                        eval("" + control + ".select()");
                        alert(campo + ' no puede tener mas de 3 letras iguales consecutivas ');
                        return false;
                    }
                }


            }

        }
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".focus()")
    }
    return flag;
}

//VALIDACION DEL IMEI desde
function ValidaIMEI(control, campo, vacio) {
    var sCad;
    eval("sCad=" + control + ".value");
    sCad = trim(sCad);

    if ((sCad == null) || (sCad.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }

    if ((sCad.length == 0) && (vacio)) {
        return true;
    }

    if (!ValidaNumero(control, campo, vacio)) return;

    if (sCad.length != 18) {
        eval("" + control + ".value=" + sCad + "");
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }
    return true;
}

//VALIDACION DEL Mat Desde y hasta
function ValidaMat(control, campo, vacio) {
    var sCad;
    eval("sCad=" + control + ".value");
    sCad = trim(sCad);

    if ((sCad == null) || (sCad.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }

    if ((sCad.length == 0) && (vacio)) {
        return true;
    }

    //if (!ValidaNumero(control,campo,vacio)) return;

    if (sCad.length != 18) {
        eval("" + control + ".value=" + sCad + "");
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }
    return true;
}


//VALIDACION DEL DNI
function ValidaDNI(control, campo, vacio) {
    var sCad;
    eval("sCad=" + control + ".value");
    sCad = trim(sCad);

    if ((sCad == null) || (sCad.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }

    if ((sCad.length == 0) && (vacio)) {
        return true;
    }

    if (!ValidaNumero(control, campo, vacio)) return;

    if (sCad.length != 8) {
        eval("" + control + ".value=" + sCad + "");
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }
    return true;
}

//VALIDACION DEL PASAPORTE
function ValidaPAS(control, campo, vacio) {
    var sCad;
    eval("sCad=" + control + ".value");
    sCad = trim(sCad);

    if ((sCad == null) || (sCad.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }

    if ((sCad.length == 0) && (vacio)) {
        return true;
    }

    if (!ValidaNumero(control, campo, vacio)) return;

    if (sCad.length < 10) {
        sCad = "0000000000" + sCad;
        sCad = sCad.substr(sCad.length - 10);
        eval(control + ".value='" + sCad + "'");
    }
    return true;
}

//VALIDACION DEL CARNÉ DE FUERZAS ARMADAS
function ValidaFA(control, campo, vacio) {
    var sCad;
    eval("sCad=" + control + ".value");
    sCad = trim(sCad);

    if ((sCad == null) || (sCad.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }

    if ((sCad.length == 0) && (vacio)) {
        return true;
    }

    if (!ValidaAlfaNumM(control, campo, vacio)) return;

    if (sCad.length < 8) {
        sCad = "00000000" + sCad;
        sCad = sCad.substr(sCad.length - 8);
        eval(control + ".value='" + sCad + "'");
    }

    return true;
}

//VALIDACION DEL CARNÉ de FUERZAS POLICIALES
function ValidaFP(control, campo, vacio) {
    var sCad;
    eval("sCad=" + control + ".value");
    sCad = trim(sCad);

    if ((sCad == null) || (sCad.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }

    if ((sCad.length == 0) && (vacio)) {
        return true;
    }

    if (!ValidaAlfaNumM(control, campo, vacio)) return;

    if (sCad.length < 6) {
        sCad = "000000" + sCad;
        sCad = sCad.substr(sCad.length - 6);
        eval(control + ".value='" + sCad + "'");
    }
    return true;
}

//@@@ BEGIN
/* Responsable	:	Narciso Lema Ch.
* Modificación	:	
*/
function ContieneEspaciosEnBlanco(valor) {
    var strValor = new String(valor);
    if (strValor.indexOf(" ") > 0) {
        return true;
    }
    return false;
}
//@@@ END

//VALIDACION DEL CARNÉ DE EXTRANJERIA
/*@@@ BEGIN
* Responsable	:	Narciso Lema Ch.
* Modificación	:	Requerimiento PMO. Registro de Clientes - Carné de Extranjería.
*/
function ValidaEXTR(control, campo, vacio) {
    var sCad, nCad;
    eval("sCad=" + control + ".value");
    sCad = trim(sCad);

    if ((sCad == null) || (sCad.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }

    if ((sCad.length == 0) && (vacio)) {
        return false;
    }

    if (ContieneEspaciosEnBlanco(sCad)) {
        eval("" + control + ".select()");
        alert(campo + ' no puede contener espacios en blanco.')
        return false;
    }

    if (sCad.length < 7 || sCad.length > 9) {
        eval("" + control + ".select()");
        alert(campo + ' no cumple la longitud especificada')
        return false;
    }

    if (sCad.length == 7) {
        if (sCad.substr(0, 1) != "N") {
            eval("" + control + ".value='" + sCad + "'");
            alert(campo + ' Debe empezar con N');
            return false;
        }

        nCad = sCad.substr(1);
        eval("" + control + ".value='" + nCad + "'");

        if (!ValidaNumero(control, campo, vacio)) {
            eval("" + control + ".value='" + sCad + "'");
            eval("" + control + ".select()");
            return false;
        }

    } else {
        if (!ValidaNumero(control, campo, vacio)) {
            eval("" + control + ".select()");
            return false;
        }
    }

    eval("" + control + ".value='" + sCad + "'");
    return true;
}
//@@@ END

//VALIDACION DEL RUC
function ValidaRUC(control, campo, vacio) {
    var sCad;
    var car;
    var cont;
    var resto;
    eval("sCad=" + control + ".value");
    sCad = trim(sCad);

    if ((sCad == null) || (sCad.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }

    if ((sCad.length == 0) && (vacio)) {
        return true;
    }


    if (!ValidaNumero(control, campo, vacio)) return;

    if (sCad.length != 11) {
        eval("" + control + ".value=" + sCad + "");
        eval("" + control + ".select()");
        alert(campo + ' no esta completo ');
        return false;
    }

    cont = 0;
    total = 0;
    while (sCad.length > 0) {
        cont += 1;
        car = sCad.substr(0, 1);
        sCad = sCad.substr(1, sCad.length);
        switch (cont) {
            case 1: total += parseInt(car) * 5;
                break;
            case 2: total += parseInt(car) * 4;
                break;
            case 3: total += parseInt(car) * 3;
                break;
            case 4: total += parseInt(car) * 2;
                break;
            case 5: total += parseInt(car) * 7;
                break;
            case 6: total += parseInt(car) * 6;
                break;
            case 7: total += parseInt(car) * 5;
                break;
            case 8: total += parseInt(car) * 4;
                break;
            case 9: total += parseInt(car) * 3;
                break;
            case 10: total += parseInt(car) * 2;
                break;
        }
    }
    resto = total % 11
    valor = 11 - resto
    switch (valor) {
        case 11: verificador = 1
            break;
        case 10: verificador = 0
            break;
        default: verificador = valor;
            break;
    }
    if (verificador == parseInt(car))
        return true;
    else {
        eval("" + control + ".select()");
        alert('' + campo + ' no es un numero de RUC válido');
        return false;
    }
}
/************* Validacion de Telefono referencia Black List **********/

function ValidaTelefonoRef(control, campo, vacio) {
    var flag = true;
    var i, j, k, a, cadena, count;
    eval("cadena = " + control + ".value");
    cadena = trim(cadena);
    if ((cadena == null) || (cadena.length == 0) && (!vacio)) {
        eval("" + control + ".select()");
        alert('Debe ingresar ' + campo);
        return false;
    }

    if (!ValidaDigitos(control, campo, 9, vacio)) {
        return false;
    }

    a = (cadena.substr(0, 1));
    if (a !== "0") {
        eval("" + control + ".select()");
        alert(campo + ' Debe comenzar en 0');
        return false;
    }

    a = (cadena.substr(0, 2));
    if (a == "00") {
        eval("" + control + ".select()");
        alert(campo + ' no puede comenzar en 00');
        return false;
    }

    a = (cadena.substr(0, 3));
    if (a == "010") {
        eval("" + control + ".select()");
        alert(campo + ' no puede comenzar en 010');
        return false;
    }

    for (i = 0; i < cadena.length; i++) {
        a = (cadena.substr(i, 1));

        j = a.charCodeAt(0);
        if (!((j >= 45) && (j <= 57) && (j != 46) && (j != 47))) {
            flag = false;
        } else {
            if (j == k) {
                count++;
            } else {
                count = 1;
                k = j;
            }
            if (count >= 4) {
                eval("" + control + ".select()");
                //Modificado para que los telefonos no esten restringidos a 4 digitos
                //alert(campo + ' no puede tener mas de 4 digitos iguales consecutivos ' );
                //return false;
            }
        }
    }
    if (!flag) {
        alert(campo + " contiene caracteres no válidos");
        eval(control + ".select()")
    }
    return flag;
}

 
 
