//PROY- 140126 HALLAZGO 16
/*include('validaciones.js');*/

function nuevaVentana(ventana, nombre) {
    window.open(ventana, nombre, 'toolbar=1,location=0,status=1,scrollbars=1, width=800,height=400');
    return false;
}

function include(filename) {
    var head = document.getElementsByTagName('head')[0];

    script = document.createElement('script');
    script.src = filename;
    script.type = 'text/javascript';

    head.appendChild(script)
}

function setValue(id, v) {
    var c = document.getElementById(id);
    if (c != null)
        c.value = v;
}

function getValue(id) {
    var c = document.getElementById(id);
    if (c != null)
        return c.value;
    return '';
}
function getText(id) {
    var c = document.getElementById(id);
    if (c != null)
        return c.options[c.selectedIndex].text;
    return '';
}

function setValueHTML(id, v) {
    var c = document.getElementById(id);
    if (c != null)
        c.innerHTML = v;
}
function setFocus(id) {
    var c = document.getElementById(id);
    if (c != null)
        if (c.disabled == false && isVisible(id) == true)
            c.focus();
}
function isVisible(id) {
    var c = document.getElementById(id);
    if (c == null) return false;
    if (c.style.display == '')
        return true;
    else
        return false;
}
function isEnabled(id) {
    var c = document.getElementById(id);
    if (c == null) return false;
    if (c.disabled == true)
        return false
    else
        return true
}
function setEnabled(id, soloLectura, classname) {
    var c = document.getElementById(id);
    if (c == null) return;
    c.disabled = soloLectura;
    if (classname != '') c.className = classname;
}

function setReadOnly(id, soloLectura, classname) {
    var c = document.getElementById(id);
    if (c == null) return;
    c.readOnly = soloLectura;
    if (classname != '') c.className = classname;
}

function setVisible(id, visible) {
    var c = document.getElementById(id);
    if (c == null) return;
    if (visible == true)
        c.style.display = '';
    else
        c.style.display = 'none';

}

// Elimina los espacios en blanco que son innecesarios.
// " prueba para  ver   " -> "prueba para ver"
function trim(cadena) {
    cadena = cadena.toString().replace(/^\s+(.*)\s+$/, "$1");
    cadena = cadena.toString().replace(/\s{2,}/g, " ");
    return cadena;
}

// Convierte un String a Hash.
// De la forma: "1,r|key,value|item,60" -> { '1'=>'r', 'key'=>'value', 'item'=>'60' }
function strToHash(strCadena) {
    var temp = strCadena.split('|');
    var response = new Array();
    for (var index in temp) {
        var item = temp[index].split(',');
        if (item.length == 2) {
            response[item[0]] = item[1];
        } else {
            response[item[0]] = item[0];
        }
    }
    return response;
}

// Convierte un String a Array.
// De la forma: "1$r$value$item$60" -> ['1', 'r', 'value', 'item', '60' ]
// strToArray(strCadena, '$')
function strToArray(strCadena, sepElementos) {
    var response = strCadena.split(sepElementos);
    return response;
}

// Convierte una Hora en un equivalente Numerico (no tiene q ser necesariamente un estandar ya establecido).
// 03:30 PM -> 10330 | 12:45 AM -> 1245
function hourToInt(hora) {
    // Hora debe ester formato "HH:MM AM/PM"
    if (!isHour(hora)) {
        return;
    }
    hora = hora.toUpperCase();
    return ((hora.substr(6, 1) == 'P' ? '1' : '') + '' + hora.substr(0, 2) + '' + hora.substr(3, 2));
}

// Convierte una Fecha en un equivalente Numerico (no tiene q ser necesariamente un estandar ya establecido).
// 15/12/1982 -> 19821215 | 01/05/2009 -> 20090501
function dateToInt(fecha) {
    // Fecha debe ester formato "DD/MM/AAAA"
    if (!isDate(fecha)) {
        return;
    }
    return (fecha.substr(6, 4) + '' + fecha.substr(3, 2) + '' + fecha.substr(0, 2));
}

// Convierte el Numero de Mes a su representacion Literal.
// 01 -> Enero | 09 -> Septiembre
function mesString(strNumero) {
    // Validar que el strNumero ingresado se convierta a Numero si el caso lo requiera.
    // ejm: "02" -> 2 | "11" -> 11
    var index = strNumero.replace(/^0+/, "");
    var meses = new Array('', 'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
						'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre');
    return meses[index];
}

// Controla el evento KeyPress, permitiendo solo valores numericos (0,1,2,3,4,5,6,7,8,9) y tab
function eventoSoloNumeros(event) {
    if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105)) {
        event.returnValue = true;
    }
    else if (event.keyCode == 9) {
        event.returnValue = true;
    }
    else {
        event.returnValue = false;
    }
    /*var tecla = String.fromCharCode(event.keyCode)
    alert(isNumber(tecla));
    if( !isNumber(tecla) ) {
    event.returnValue = false;
    }*/
}

// Controla el evento KeyPress, permitiendo solo valores alfa-numericos (a,b..y,z, 0,1..8,9)
function eventoAlfaNumerico(event) {
    if ((event.keyCode >= 65 && event.keyCode <= 90)) {
        event.returnValue = true;
    }
    else {
        eventoSoloNumeros(event);
    }
}

function eventoSoloMontos(event, obj) {
    var code = (event.which) ? event.which : event.keyCode;
    var character = String.fromCharCode(code);
    var decimales = 0;
    var cantidad_decimales = 2;
    var salida = false;
    if ((code >= 48 && code <= 57) || (code >= 96 && code <= 105)) { // check digits
        if (obj.value == "0") return false;
        if (!isNaN(obj.value)) {
            if (obj.value == "0.0" && code == 48) {
                return false;
            }
        }
        if (obj.value.indexOf('.') >= 0) {
            decimales = obj.value.substring(obj.value.indexOf('.') + 1, obj.value.length);
            if (decimales.length >= cantidad_decimales) {
                //alert('decimales = ' + obj.value);
                return false;
            }
        }
        return true;
    } else if (code == 46 || code == 110) { // Check dot
        if (obj.value.indexOf(".") < 0) {
            if (obj.value.length == 0) obj.value = "0";
            return true;
        }
    } else if (code == 8 || code == 116) { // Allow backspace, F5
        return true;
    } else if (code >= 37 && code <= 40) { // Allow directional arrows
        return true;
    } else if (code == 9 || code == 16) { // tab control + tab
        return true;
    }
    return false;
}

function getIndexOf(aElementos, elemento) {
    var i;
    for (i = 0; i < aElementos.length; i++) {
        if (aElementos[i] == elemento) {
            return i;
        }
    }
    return -1;
}

function addOption(selectbox, value, text) {
    var optn = document.createElement("OPTION");
    optn.value = value;
    optn.text = text;
    selectbox.options.add(optn);
}

//Bloqueamos la tecla BACKSPACE en la ventana
function cancelBack() {
    if (
	(event.keyCode == 8 ||
	(event.keyCode == 37 && event.altKey) ||
	(event.keyCode == 39 && event.altKey)
	)
	&&
	(
	event.srcElement.form == null ||
	(event.srcElement.isTextEdit == false && !event.srcElement.readOnly)
	)
	) {
        event.cancelBubble = true;
        event.returnValue = false;
    }
}

function removeOption(selectbox, index) {
    //selectbox.remove(index);
    selectbox.options.remove(index);
}

function validarPalabra(event) {
    if ((event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122)) {
        return;
    }
    else if (event.keyCode == 8 || event.keyCode == 116 || event.keyCode == 32) { // Allow backspace, F5
        return;
    }
    else if (event.keyCode >= 37 && event.keyCode <= 40) { // Allow directional arrows
        return;
    }
    else if (event.keyCode == 9 || event.keyCode == 16) { // tab control + tab
        return;
    }
    eventoSoloLetras(event);
}

function eventoSoloLetras(event) {
    if ((event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 97 && event.keyCode <= 122)) {
        event.returnValue = true;
    }
    else if (event.keyCode == 8 || event.keyCode == 116 || event.keyCode == 32) { // Allow backspace, F5
        event.returnValue = true;
    }
    else if (event.keyCode >= 37 && event.keyCode <= 40) { // Allow directional arrows
        event.returnValue = true;
    }
    else if (event.keyCode == 9 || event.keyCode == 16) { // tab control + tab
        event.returnValue = true;
    }
    event.returnValue = false;
}

function eventoSoloNumerosEnterosL() {
    var CaracteresPermitidos = '0123456789/';
    var key = String.fromCharCode(window.event.keyCode);
    var valid = new String(CaracteresPermitidos);
    var ok = "no";
    for (var i = 0; i < valid.length; i++) {
        if (key == valid.substring(i, i + 1))
            ok = "yes";
    }
    if (window.event.keyCode != 16) {
        if (ok == "no") { }
        //window.event.keyCode=0;
    }
}

function formatoFecha(texto) {
    var arrPosCaracter = [2, 5]
    var arrCaracter = ['/', '/'];
    var tamanioMaximo = 10;
    var prefijo = '';

    var longTexto = texto.length;
    var longPrefijo = prefijo.length;

    evt = window.event;
    var keyCode = evt.keyCode ? evt.keyCode : evt.charCode;
    var blnCaracPermitido = false;

    if (!isNaN(texto.substr(longTexto - 1, longTexto))) {
        blnCaracPermitido = true;
    }
    if (texto.substr(longTexto - 1, longTexto) == " ") {
        blnCaracPermitido = false;
    }

    if (keyCode != 8) {
        if (!blnCaracPermitido) {
            texto = texto.substr(0, longTexto - 1);
        } else {
            if (longTexto > tamanioMaximo) {
                texto = texto.substr(0, tamanioMaximo);
                alert("La cantidad de caracteres no puede superar al máximo de " + tamanioMaximo);
            } else {
                for (i = 0; i < arrPosCaracter.length; i++) {
                    if (longTexto >= parseInt(arrPosCaracter[i])) {
                        if (longTexto == parseInt(arrPosCaracter[i])) {
                            texto = texto.substr(0, parseInt(arrPosCaracter[i])) + arrCaracter[i] + texto.substr(parseInt(arrPosCaracter[i]) + 1, tamanioMaximo);
                        } else {
                            if (longTexto == parseInt(arrPosCaracter[i]) + 1) {
                                if (texto.substr(parseInt(arrPosCaracter[i]), parseInt(arrPosCaracter[i]) + 1) != arrCaracter[i]) {
                                    texto = texto.substr(0, parseInt(arrPosCaracter[i])) + arrCaracter[i] + texto.substr(parseInt(arrPosCaracter[i]), tamanioMaximo);
                                }
                            }
                        }
                        if (longPrefijo > 0) {
                            if (longTexto > longPrefijo) {
                                if (texto.substr(0, longPrefijo) != prefijo) {
                                    texto = prefijo + texto.substr(longPrefijo, longTexto);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    return texto;
}

//PROY-21117-IDEA-27328-RU17 BEGIN
function esFechaValida(fecha) {
    if (fecha != "") {
        if (!/^\d{2}\/\d{2}\/\d{4}$/.test(fecha))
            return false;
        var dia = parseInt(fecha.substring(0, 2), 10);
        var mes = parseInt(fecha.substring(3, 5), 10);
        var anio = parseInt(fecha.substring(6), 10);

        switch (mes) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                numDias = 31;
                break;
            case 4: case 6: case 9: case 11:
                numDias = 30;
                break;
            case 2:
                if (comprobarSiBisisesto(anio)) { numDias = 29 } else { numDias = 28 };
                break;
            default:
                return false;
        }

        if (dia > numDias || dia == 0)
            return false;

        return true;
    }
    else
        return false;
}

function comprobarSiBisisesto(anio) {
    if ((anio % 100 != 0) && ((anio % 4 == 0) || (anio % 400 == 0)))
        return true;
    else
        return false;
} //PROY-21117-IDEA-27328-RU17 END

// Simulates PHP's date function
Date.prototype.format = function (format) {
    var returnStr = '';
    var replace = Date.replaceChars;
    for (var i = 0; i < format.length; i++) {
        var curChar = format.charAt(i);
        if (replace[curChar]) {
            returnStr += replace[curChar].call(this);
        } else {
            returnStr += curChar;
        }
    }
    return returnStr;
};
Date.replaceChars = {
    shortMonths: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
    longMonths: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
    shortDays: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
    longDays: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],

    // Day
    d: function () { return (this.getDate() < 10 ? '0' : '') + this.getDate(); },
    D: function () { return Date.replaceChars.shortDays[this.getDay()]; },
    j: function () { return this.getDate(); },
    l: function () { return Date.replaceChars.longDays[this.getDay()]; },
    N: function () { return this.getDay() + 1; },
    S: function () { return (this.getDate() % 10 == 1 && this.getDate() != 11 ? 'st' : (this.getDate() % 10 == 2 && this.getDate() != 12 ? 'nd' : (this.getDate() % 10 == 3 && this.getDate() != 13 ? 'rd' : 'th'))); },
    w: function () { return this.getDay(); },
    z: function () { return "Not Yet Supported"; },
    // Week
    W: function () { return "Not Yet Supported"; },
    // Month
    F: function () { return Date.replaceChars.longMonths[this.getMonth()]; },
    m: function () { return (this.getMonth() < 9 ? '0' : '') + (this.getMonth() + 1); },
    M: function () { return Date.replaceChars.shortMonths[this.getMonth()]; },
    n: function () { return this.getMonth() + 1; },
    t: function () { return "Not Yet Supported"; },
    // Year
    L: function () { return "Not Yet Supported"; },
    o: function () { return "Not Supported"; },
    Y: function () { return this.getFullYear(); },
    y: function () { return ('' + this.getFullYear()).substr(2); },
    // Time
    a: function () { return this.getHours() < 12 ? 'am' : 'pm'; },
    A: function () { return this.getHours() < 12 ? 'AM' : 'PM'; },
    B: function () { return "Not Yet Supported"; },
    g: function () { return this.getHours() % 12 || 12; },
    G: function () { return this.getHours(); },
    h: function () { return ((this.getHours() % 12 || 12) < 10 ? '0' : '') + (this.getHours() % 12 || 12); },
    H: function () { return (this.getHours() < 10 ? '0' : '') + this.getHours(); },
    i: function () { return (this.getMinutes() < 10 ? '0' : '') + this.getMinutes(); },
    s: function () { return (this.getSeconds() < 10 ? '0' : '') + this.getSeconds(); },
    // Timezone
    e: function () { return "Not Yet Supported"; },
    I: function () { return "Not Supported"; },
    O: function () { return (-this.getTimezoneOffset() < 0 ? '-' : '+') + (Math.abs(this.getTimezoneOffset() / 60) < 10 ? '0' : '') + (Math.abs(this.getTimezoneOffset() / 60)) + '00'; },
    T: function () { var m = this.getMonth(); this.setMonth(0); var result = this.toTimeString().replace(/^.+ \(?([^\)]+)\)?$/, '$1'); this.setMonth(m); return result; },
    Z: function () { return -this.getTimezoneOffset() * 60; },
    // Full Date/Time
    c: function () { return "Not Yet Supported"; },
    r: function () { return this.toString(); },
    U: function () { return this.getTime() / 1000; }
};

//PROY-140618  IDEA-142181 INI
function validaTxtDocGeneral(elem) {
    var tipoDoc = getValue(elem);
    if (Key_codigoDocMigraYPasaporte.indexOf(tipoDoc) > -1)
        validaCaracteres('ABCDEFGHIJKLMN?OPQRSTUVWXYZabcdefghijklmn?opqrstuvwxyz0123456789');
    else
        validaCaracteres('0123456789');
}

function getMaxLengthTipoDocGenerico(tipoDoc) {
    var nroCaracter;
    if (tipoDoc == constTipoDocumentoDNI) nroCaracter = 8;
    else if (tipoDoc == constTipoDocumentoCE) nroCaracter = 9;
    else if (tipoDoc == constTipoDocumentoRUC) nroCaracter = 11;
    else nroCaracter = 12;
    return nroCaracter;
}

function loadingGeneric() {
    var loading1 = document.createElement("div");
    loading1.id = "loading1";
    loading1.style.color = "white";
    //    loading1.style.backgroundColor = "#FFF";//negro
    loading1.style.position = "absolute";
    loading1.style.right = "0px";
    loading1.style.top = "0px";
    loading1.style.zIndex = "9998";
    loading1.style.width = screen.width;
    loading1.style.height = screen.height;
    loading1.className = 'modalBackground';
    document.body.appendChild(loading1);
    var loading = document.createElement("div");
    loading.id = "loading";
    loading.style.position = "absolute";
    loading.style.right = screen.width / 3;
    loading.style.top = screen.height / 3;
    loading.style.zIndex = "9999";
    loading.innerHTML = "<img src='../../imagenes/ajax-loader.gif'>";

    document.body.appendChild(loading);
}

function quitLoadingGeneric() {
    var loading1 = document.getElementById("loading1");
    if (loading1 != null)
        document.body.removeChild(loading1);
    var loading = document.getElementById("loading");
    if (loading != null)
        document.body.removeChild(loading);
}
//PROY-140618  IDEA-142181 FIN