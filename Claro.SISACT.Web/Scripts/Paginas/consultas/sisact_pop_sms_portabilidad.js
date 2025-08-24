function EnviarSMSPortabilidades(tipoDocumento, nroDocumento, nroPortabilidad, codPortabilidad) { //INC-SMS_PORTA
    PageMethods.EnviarSMSPortabilidades(tipoDocumento, nroDocumento, nroPortabilidad, codPortabilidad, EnviarSMSPortabilidades_callback); //INC-SMS_PORTA
}

function EnviarSMSPortabilidades_callback(response) {
    var mensaje = document.getElementById("hidMensajeSMS");
    var timer = document.getElementById("hidTimer");
    var codigoSms = document.getElementById("hidCodSMS");

    if (response != null) {
        if (response.CodigoError != "0") {
            mensaje.value = response.Mensaje;
            retornoValue(mensaje.value); //INC-SMS_PORTA
        }
        else {
            var arrData = response.Cadena.split('|');
            timer.value = arrData[0];
            codigoSms.value = arrData[1];
            startTimer(timer.value);
        }
    }
}

function getValidarPin() {
    var lineas = document.getElementById("hidlineas").value;
    var cont = 0;

    for (var i = 0; i < lineas; i++) {
        if (document.getElementById("codPin" + i).value.length < 4) {
            alert("Ingresar código en la línea " + (i + 1) + " de 4 o 5 dígitos");
        }
        if (document.getElementById("codPin" + i).value.length == 4 || document.getElementById("codPin" + i).value.length == 5) {
            cont++;
        }
    }
    if (cont == lineas) {
        EnviarPinSMS(lineas);
    }
}

function EnviarPinSMS(lineas) {
    var arryLineas = "";
    var arrayCodSms = document.getElementById("hidCodSMS").value;

    for (var i = 0; i < lineas; i++) {
        arryLineas += arrayCodSms.split(';')[i] + ";";
        arryLineas += document.getElementById("codPin" + i).value + ";";
        arryLineas += document.getElementById("hidEstado" + i).value + ";";
        arryLineas += "|";
    }
    ValidarCodigoSMSPortabilidades(arryLineas)
}

function startTimer(duration) {
    window.returnValue = '';
    var timer = duration, minutes, seconds;
    setInterval(function () {
        minutes = parseInt(timer / 60, 10);
        seconds = parseInt(timer % 60, 10);
        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;

        var counter = document.getElementById("temporizador");
        counter.innerHTML = minutes.toString() + ":" + String(seconds);

        if (--timer < 0) {
            var mensaje = document.getElementById("hidMensajeSMS");
            mensaje.value = 'Se acabó el tiempo de validación.';
            retornoValue(mensaje.value); //INC-SMS_PORTA
        }
    }, 1000);
}

function ValidarCodigoSMSPortabilidades(arrayLineas) {
    PageMethods.ValidarCodigoSMSPortabilidades(arrayLineas, ValidarCodigoSMSPortabilidades_callback);
}

function ValidarCodigoSMSPortabilidades_callback(response) {
    if (response != null) {
        if (response.CodigoError != "0") {
            retornoValue(response.Mensaje);
        }
        else {
            var data = response.Cadena.toString().split('|');
            setRespuestaValidacion(data);
        }
    }
}

function setRespuestaValidacion(data) {
    var cont = 0;
    for (var i = 0; i < data.length - 1; i++) {
        var posicion = data[i].split(';')[0];
        var respCodResp = data[i].split(';')[2];
        var respMsgResp = data[i].split(';')[3];
        var estado = document.getElementById("hidEstado" + posicion);
        var msg = document.getElementById("msjServicio" + posicion);
        var pin = document.getElementById("codPin" + posicion);
        var telefono = document.getElementById("nrotelefonos" + posicion).value;

        estado.value = respCodResp;
        msg.value = respMsgResp;

        if (respCodResp != "0") {
            msg.style.color = "red";
            pin.value = "";
            pin.disabled = false;
            cont++;
            if (respCodResp == "5" || respCodResp == "1") {
                retornoValue("ERROR: " + msg.value);
            }
        } else {
            pin.disabled = true;
            msg.style.color = "green";
            document.getElementById("codPin" + posicion).disabled = true;
        }
    }
    if (cont == 0) {
        alert("Validación Exitosa");
        retornoValue("true");
    }
}

//INC-SMS_PORTA_INI
function retornoValue(msg) {
    if (msg == "true") {
        ActualizarTrazabilidadLineas(msg);
    }
    else {
        window.returnValue = msg;
        window.close();
        return;
    }
}

function ActualizarTrazabilidadLineas(msg) {
    var tipoDocumento = document.getElementById("hidtipoDocumento").value;
    var numeroDoc = document.getElementById("hidnroDocumento").value;
    var codPortabilidad = document.getElementById("hidcodportabilidad").value;
    PageMethods.ActualizarTrazabilidadLineas(tipoDocumento, numeroDoc, codPortabilidad, msg, ActualizarTrazabilidadLineas_callback);
}

function ActualizarTrazabilidadLineas_callback(objResponse) {

    if (objResponse.CodigoError == '0') {
        var codigoValidador = objResponse.Cadena;
        window.returnValue = "true|" + codigoValidador;
        window.close();
        return;
    }
    else {
        window.returnValue = objResponse.DescripcionError;
        window.close();
        return;
    }
}
//INC-SMS_PORTA_FIN
//PROY-SMSPORTA::FIN