// PROY- 140126 HALLAZGO 16
/*
include('funciones_generales.js');*/

function include(filename)
{
	var head = document.getElementsByTagName('head')[0];
	
	script = document.createElement('script');
	script.src = filename;
	script.type = 'text/javascript';
	
	head.appendChild(script)
}


// Valores aceptados: Todos los digitos (0,1,2,3,4,5,6,7,8,9)
function isNumber(pString) {
	return ( /^\d+$/.test(pString) );
}

// Valores aceptados: Todos los Enteros Positivos (1,2,3..100..1000...)
// Negacion de isPositiveInteger: isNegativeInteger + 0
function isPositiveInteger(pString) {
	if ( /^\+?0+$/.test(pString) ) {
		return false;
	}
	return ( /^\+?\d+$/.test(pString) );
}

// Valores aceptados: Todos los Enteros Negativos (-1,-2,-3..-100..-1000...)
// Negacion de isNegativeInteger: isPositiveInteger + 0
function isNegativeInteger(pString) {
	if ( /^\-?0+$/.test(pString) ) {
		return false;
	}
	return ( /^\-\d+$/.test(pString) );
}

// Valores aceptados: Todos los numeros fijos que incluyen su codigo de ciudad.
function isPhoneNumber(strNumero, strCodCiudades) {
	// Validar que sea un numero
	if (!isNumber(strNumero)) {
		alert('Numero de Telefono Fijo no valido.');
		return false;
	}
		
	// Validar que tenga exacto 8 digitos (codigoCiudad y numeroTelefono)
	var sizeBase = 8;
	var sizeAll = strNumero.length;
	if (sizeAll < sizeBase) {
		alert('Debe ingresar Codigo de Ciudad y Numero de Telefono Fijo.');
		return false;
	}
	if (sizeAll > sizeBase) {
		alert('Numero de Telefono Fijo no valido.');
		return false;
	}

	// Validar que codigoCiudad exista
	var sizeCod = (strNumero.substring(0, 1) == 1) ? 1 : 2;
	var phoneNumber = strNumero.substring(sizeCod);
	var CodCity = strNumero.substring(0, sizeCod);

	var arrayCodCiudades = strToHash(strCodCiudades);
	if ( !arrayCodCiudades[CodCity] ) {
	    alert('Codigo de Ciudad no existe.');
		return false;
	}

	return true;
}

// Valores aceptados: Todos los numeros celulares que incluyen su codigo de ciudad.
function isCelNumber(strNumero, strCodCiudades) {
	// Validar que sea un numero
	if (!isNumber(strNumero)) {
		alert('Numero Celular no valido.');
		return false;
	}

	// Validar que tenga más de 9 digitos (codigoCiudad y numeroCelular)
	var sizeBase = 9;
	var sizeAll = strNumero.length;
	if (sizeAll <= sizeBase) {
		alert('Debe ingresar Codigo de Ciudad y Numero Celular.');
		return false;
	}

	// Validar que codigoCiudad solo tenga entre 1 y 2 digitos
	var sizeCod = sizeAll - sizeBase;
	if (sizeCod > 2) {
		alert('Numero Celular no valido.');
		return false;
	}

	// Validar que el primer digito del numeroCelular sea "9"
	var celNumber = strNumero.substring(sizeCod);
	var CodCity = strNumero.substring(0, sizeCod);
	if (celNumber.substring(0, 1) != 9) {
		alert('Numero Celular debe empezar con 9.');
		return false;
	}

	// Validar que codigoCiudad exista
	var arrayCodCiudades = strToHash(strCodCiudades);
	if ( !arrayCodCiudades[CodCity] ) {
	    alert('Codigo de Ciudad no existe.');
		return false;
	}
	// Validar que codigoCiudad (caso especial: La Libertad, Arequipa, Piura y Lambayeque)
	// debe tener relacion con los siguientes numeros despues del "9" del numeroCelular
	if ( arrayCodCiudades[CodCity] != celNumber.substring(1, 1+arrayCodCiudades[CodCity].length) ) {
	    // Si arrayCodCiudades[CodCity] es "0" no tiene regla alguna de control
	    if (arrayCodCiudades[CodCity] != 0) {
	        alert('Numero Celular no coincide con Codigo de Ciudad.');
		    return false;
		}
	}

	return true;
}

// Mascara de una Hora: ##:## (A|P)M
function isHourMask(strHour) {
	return ( /^\d{2}\:\d{2}\s[aApP]\.?[mM]\.?$/.test(strHour) );
}

// Valores aceptados: Todas las horas entre 01:00 AM y 12:59 PM
function isHour(hora) {
	if ( !isHourMask(hora) ) {
		alert('Hora no valida. Debe ingresar el formato (HH:MM AM/PM)');
		return false;
	}
	/^(\d{2})\:(\d{2})\s([aApP]\.?[mM]\.?)$/.test(hora);
	var hour   = RegExp.$1;
	var minute = RegExp.$2;
	var time   = RegExp.$3;
	
	if (hour > 12 || hour < 1) {
		alert('Hora no valida. La Hora debe estar comprendida entre 01 y 12.');
		return false;
	}
	if (minute > 59) {
		alert('Minutos no validos. Los Minutos deben estar comprendidos entre 00 y 59');
		return false;
	}
	
	return true;
}

// Verifica si hora inicio es menor q hora fin. Si el flag esta activado, ambas horas pueden ser iguales.
function isHoursOK(horaInicio, horaFin, flagPermiteIguales) {
	var compI = hourToInt(horaInicio);
	var compF = hourToInt(horaFin);
	if (flagPermiteIguales == '1') {
		return (compI <= compF);
	}
	else if (flagPermiteIguales == '0') {
		return (compI < compF);
	}
}

// Mascara de una Fecha: ##/##/####
function isDateMask(strDate) {
	return ( /^\d{2}\/\d{2}\/\d{4}$/.test(strDate) );
}

// Valores aceptados: Todas las Fechas entre 01/01/0001 y 31/12/9999
function isDate(fecha) {
	if ( !isDateMask(fecha) ) {
		alert('Fecha no valida. Debe ingresar el formato (DD/MM/AAAA)');
		return false;
	}
	/^(\d{2})\/(\d{2})\/(\d{4})$/.test(fecha);
	var day = RegExp.$1;
	var month = RegExp.$2;
	var year = RegExp.$3;
	
	if ( !isPositiveInteger(day) ) {
		alert('Dia no valido. Debe ingresar el formato (DD/MM/AAAA).');
		return false;
	}
	if ( !isPositiveInteger(month) ) {
		alert('Mes no valido. Debe ingresar el formato (DD/MM/AAAA).');
		return false;
	}
	if ( !isPositiveInteger(year) ) {
		alert('Anio no valido. Debe ingresar el formato (DD/MM/AAAA).');
		return false;
	}
	
	// Indicador si Anio es Bisiesto
	// (Divisible entre 4, pero no debe ser divisible entre 100 a no ser que tambien sea divisible entre 400)
	var flagAnioBisiesto = ( (year%4 == 0) && ( (year%100 != 0) || (year%400 == 0) ) );
	if ( month > 12 ) {
		alert('Mes no valido. ' + month + ' no es un mes.');
		return false;
	}
	if ( /^01|03|05|07|08|10|12$/.test(month) && (day > 31) ) {
		alert('Dia no valido. ' + mesString(month) + ' solo tiene 31 dias.');
		return false;
	}
	if ( /^04|06|09|11$/.test(month) && (day > 30) ) {
		alert('Dia no valido. ' + mesString(month) + ' solo tiene 30 dias.');
		return false;
	}
	if ( /^02$/.test(month) && ( (flagAnioBisiesto && (day > 29)) || (!flagAnioBisiesto && (day > 28)) ) ) {
		var dias = (flagAnioBisiesto) ? 29 : 28;
		alert('Dia no valido. ' + mesString(month) + ' solo tiene ' + dias + ' dias.');
		return false;
	}
	
	return true;
}

// Verifica si fecha inicio es menor que fecha fin. Si el flag esta activado, ambas fechas pueden ser iguales.
function isDatesOK(fechaInicio, fechaFin, flagPermiteIguales) {
	var compI = dateToInt(fechaInicio);
	var compF = dateToInt(fechaFin);
	if (flagPermiteIguales == '1') {
		return (compI <= compF);
	}
	else if (flagPermiteIguales == '0') {
		return (compI < compF);
	}
}

// Mascara de un Monto: ##########.##
function isMoneyMask(strMoney) {
	return ( /^[1-9]\d*(\.\d{1,2})?$|^0(\.\d{1,2})?$/.test(strMoney) );
}
