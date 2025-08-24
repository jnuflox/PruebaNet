<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_direccion.aspx.cs"
    Inherits="Claro.SISACT.Web.Paginas.consultas.sisact_direccion" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DIRECCION DE INSTALACION</title>
    <link href="../../Estilos/general.css" type="text/css" rel="stylesheet" />
    <script language="JavaScript" type="text/javascript" src="../../Scripts/Lib_FuncValidacion.js"></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/funciones_sec.js"></script>
    <script language="javascript" type="text/javascript" src="../../Scripts/funciones_direccion.js"></script>
    <script language="javascript" type="text/javascript"src="../../Scripts/funciones_evaluacion.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/jquery-1.9.1.js"></script>
        <script language="javascript" type="text/javascript" src="../../Scripts/security.js"></script>
        <script type="text/javascript" language="JavaScript" src="../../Scripts/KeySettings/KeySettings.js"></script><!-- INICIATIVA 992 -->
    <base target="_self" />
    <script type="text/javascript" language="javascript">
        //gaa20120206
        var K_CANTIDAD_DIRECCION = 40;

        var codTipoProductoMovil = '<%= ConfigurationManager.AppSettings["constTipoProductoMovil"] %>';
        var codTipoProductoFijo = '<%= ConfigurationManager.AppSettings["constTipoProductoFijo"] %>';
        var codTipoProductoBAM = '<%= ConfigurationManager.AppSettings["constTipoProductoBAM"] %>';
        var codTipoProductoDTH = '<%= ConfigurationManager.AppSettings["constTipoProductoDTH"] %>';
        var codTipoProductoHFC = '<%= ConfigurationManager.AppSettings["constTipoProductoHFC"] %>';

        var codTipoProd3PlayInalam = '<%= ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"] %>';
        var codTipoProductoInterInalam = '<%= ConfigurationManager.AppSettings["constTipoProductoInterInalam"] %>';//140690
        function eventoSoloNumerosEnteros() {
            var CaracteresPermitidos = '0123456789';
            var key = String.fromCharCode(window.event.keyCode)
            var valid = new String(CaracteresPermitidos)
            var ok = "no";
            for (var i = 0; i < valid.length; i++) {
                if (key == valid.substring(i, i + 1))
                    ok = "yes"
            }
            if (window.event.keyCode != 16) {
                if (ok == "no")
                    window.event.keyCode = 0
            }
        }

        //********* desde aqui
        function mostrarProvincia(ddlDep) {

            var pos_usc;

            var dpto_id, provincia, distrito, cad;
            cad = getValue('hidProvincias');
            dpto_id = '';
            dpto_id = getValue('ddlDepartamento');
            provincia = document.getElementById('ddlProvincia');
            distrito = document.getElementById('ddlDistrito');
            //ddlCentroPoblado
            CentroPoblado = document.getElementById('ddlCentroPoblado');

            setValue('txtCodigoPostal', '');
            setValue('hidDptoId', dpto_id);

            var i, j;
            var oElm;
            for (j = provincia.length - 1; j >= 0; j--)
                provincia.remove(j);

            for (j = distrito.length - 1; j >= 0; j--)
                distrito.remove(j);

            for (j = CentroPoblado.length - 1; j >= 0; j--)
                CentroPoblado.remove(j);

            oElm = document.createElement("OPTION");
            oElm.value = "";
            oElm.text = "--Seleccione--";
            provincia.add(oElm);

            oElm = document.createElement("OPTION");
            oElm.value = "";
            oElm.text = "--Seleccione--";
            distrito.add(oElm);

            oElm = document.createElement("OPTION");
            oElm.value = "";
            oElm.text = "--Seleccione--";
            CentroPoblado.add(oElm);

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
            setValue('txtUbigeo', "");
        }
        function mostrarDistrito(ddlProv) {
            var pos_usc;
            var dpto_id, provincia_id, distrito, cad;
            cad = getValue('hidDistritos');

            dpto_id = getValue('ddlDepartamento');
            provincia_id = getValue('ddlProvincia');
            distrito = document.getElementById('ddlDistrito');
            CentroPoblado = document.getElementById('ddlCentroPoblado');
            setValue('txtCodigoPostal', '');

            document.getElementById('hidProvinciaId').value = provincia_id;

            var i, j;
            var oElm;
            for (j = distrito.length - 1; j >= 0; j--)
                distrito.remove(j);

            for (j = CentroPoblado.length - 1; j >= 0; j--)
                CentroPoblado.remove(j);

            oElm = document.createElement("OPTION");
            oElm.value = "";
            oElm.text = "--Seleccione--";
            distrito.add(oElm);

            oElm = document.createElement("OPTION");
            oElm.value = "";
            oElm.text = "--Seleccione--";
            CentroPoblado.add(oElm);

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
            setValue('txtUbigeo', "");
        }

        function mostrarUbigeo(ddldist) {
            var pos_usc, ubigeo, departamento, provincia;

            departamento = document.getElementById('ddlDepartamento');
            provincia = document.getElementById('ddlProvincia');

            ubigeo = departamento[departamento.selectedIndex].value + provincia[provincia.selectedIndex].value + ddldist[ddldist.selectedIndex].value;

            if (ddldist[ddldist.selectedIndex].value != "") {
                setValue('txtUbigeo', ubigeo);
            } else {
                setValue('txtUbigeo', "");
            }
            //alert(ubigeo);
        }

        function mostrarCodigoPostal(ddldist) {
            var pos_usc;

            var distrito;
            var codigoPostal = '';
            distrito = document.getElementById('ddlDistrito');
            codigoPostal = distrito[distrito.selectedIndex].id;
            if (codigoPostal == '' || codigoPostal == 'undefined') {
                codigoPostal = obtenerCodigoPostal(distrito.value);
                if (codigoPostal == 'undefined') codigoPostal = '';
            }
            setValue('txtCodigoPostal', codigoPostal);
            setValue('hidDistritoId', distrito.value);

            document.getElementById('hidDistritoId').value = ddldist.value;
        }
        function obtenerCodigoPostal(distrito_id) {

            if (distrito_id == '-1' || distrito_id == '') return '';
            var cad = getValue('hidListaCodigoPostal');
            if (cad == '') return '';
            var aDistrito = cad.split("|")
            var i, aDatos;

            for (i = 0; i < aDistrito.length; i++) {
                aDatos = aDistrito[i].split(";");
                if (aDatos[0] == distrito_id) {
                    //alert(aDatos[0] + '-'+ aDatos[1]);
                    return aDatos[1];
                }
            }
            return '';
        }

        function sinNumero(chk) {
            var pos_usc;

            if (chk.checked == true) {
                setEnabled('txtNroPuerta', false, 'clsInputDisable');
                setEnabled('ddlEdificacion', true, 'clsSelectEnable0');
                setValue('txtNroPuerta', getValue('hidSinNumero'));
            } else {
                setEnabled('txtNroPuerta', true, 'clsInputEnable');
                setValue('ddlEdificacion', '-1');
                setEnabled('ddlEdificacion', false, 'clsSelectDisable');
                setValue('txtNroPuerta', '');
                setFocus('txtNroPuerta');

                setValue('txtManzana', '');
                setValue('txtLote', '');
                setEnabled('txtManzana', false, 'clsInputDisable');
                setEnabled('txtLote', false, 'clsInputDisable');
            }
            onkeyup_NroPuerta(document.getElementById('txtNroPuerta'));
        }
        function onkeyup_NroPuerta(txt) {
            var pos_usc;

            var key = event.keyCode;

            var salida = txt.value;
            salida = eliminaCaracteresNroPuerta(salida);
            if (salida != '') {

                setEnabled('ddlEdificacion', true, '');
                setValue('ddlEdificacion', '-1');
                setValue('txtManzana', '');
                setValue('txtLote', '');
                setEnabled('txtManzana', false, 'clsInputDisable');
                setEnabled('txtLote', false, 'clsInputDisable');
                if (salida != 'S/N')
                    setEnabled('ddlEdificacion', false, 'clsSelectDisable');
                else
                    setEnabled('ddlEdificacion', true, 'clsSelectEnable0');
            } else {
                setEnabled('ddlEdificacion', false, 'clsSelectDisable');
            }
            var total = contador_d('D', pos_usc);
            if (total > K_CANTIDAD_DIRECCION) {
                salida = salida.substr(0, salida.length - 1);
                setValueHTML('lblContadorDireccion', K_CANTIDAD_DIRECCION);
            }
            txt.value = salida;
        }

        function onchange_prefijo(idusc) {
            var pos_usc;

			//INICIO INICIATIVA-932
            if (getValue('hidFlagIFI') == '1') {
				//INICIO INICIATIVA-932
	            //setValue('txtDireccion', ''); MOVIDILIDAD-IFI
	            //setValue('txtNroPuerta', '');
				//FIN INICIATIVA-932
			}
			else{
            setValue('txtDireccion', '');
            setValue('txtNroPuerta', '');
			}
			//FIN INICIATIVA-932
            //document.getElementById(pos_usc + 'hidDistritoId').value=ddldist.value;	
            var cbo = document.getElementById('ddlPrefijo'); // idusc;

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
                    setValueHTML('lblContadorDireccion', K_CANTIDAD_DIRECCION);
                    cbo.value = '-1';
                    return;
                } else {
                    total = parseInt(total, 10) - 2;
                }

				//INICIO INICIATIVA-932
                if (getValue('hidFlagIFI') == '1') {
					if (getValue('hidTipoProducto') != codTipoProductoInterInalam) {
                setEnabled('txtDireccion', true, 'clsInputEnable');
                setEnabled('txtNroPuerta', true, 'clsInputEnable');
                setEnabled('divSinNumero', true, ''); //panelSinNumero
                //setEnabled( 'ddlEdificacion',true,'clsSelectEnable0');
	                    setValue('txtDireccion', '');
	                    setValue('txtNroPuerta', '');
                	}
				}
				else{
					setEnabled('txtDireccion', true, 'clsInputEnable');
	                setEnabled('txtNroPuerta', true, 'clsInputEnable');
	                setEnabled('divSinNumero', true, ''); //panelSinNumero
	                //setEnabled( 'ddlEdificacion',true,'clsSelectEnable0');
				}
				//FIN INICIATIVA-932

            } else {
                setEnabled('txtDireccion', false, 'clsInputDisable');
                setEnabled('txtNroPuerta', false, 'clsInputDisable');
                setEnabled('divSinNumero', false, '');
                setValue('txtDireccion', '');
                setValue('txtNroPuerta', '');

                setValue('txtManzana', '');
                setValue('txtLote', '');
                setValue('ddlEdificacion', '-1');
                //setEnabled( 'ddlEdificacion',false,'clsSelectDisable');
                setEnabled('txtManzana', false, 'clsInputDisable');
                setEnabled('txtLote', false, 'clsInputDisable');
                document.getElementById('chkSinNumero').checked = false;
                total = 0;
            }

            // Cambios 3Play
            document.getElementById('chkSinNumero').checked = false;
            document.getElementById('chkSinNumero').disabled = false;
			
			//INICIO INICIATIVA-932
            if (getValue('hidFlagIFI') == '1') {
				if (getValue('hidTipoProducto') != codTipoProductoInterInalam) {
	                setEnabled('txtDireccion', true, 'clsInputEnable');
	            }
			}
			else{
            setEnabled('txtDireccion', true, 'clsInputEnable');
			}
			//FIN INICIATIVA-932

            if (prefijo == '<%= ConfigurationManager.AppSettings["constPrefijoCalleSinNombre"] %>') {
                setValue('ddlEdificacion', '-1');
                setValue('txtManzana', '');
                setValue('txtLote', '');
                setValue('txtDireccion', ' ');
                setEnabled('txtDireccion', false, 'clsInputDisable');
                document.getElementById('chkSinNumero').checked = true;
                document.getElementById('chkSinNumero').disabled = true;
                sinNumero(document.getElementById('chkSinNumero'));
            }

            if (total == 0) total = '';
            setValueHTML('lblContadorDireccion', total);
        }

        function onkeyup_direccion(txt, cbo_id, flagReferencia) {
            var pos_usc;
            //adicionado cuando es llamado de otro user control

            var key = event.keyCode;

            var maximo = 40;
            var cbo_des = getText(cbo_id);
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
                    setValueHTML('lblContadorDireccion', K_CANTIDAD_DIRECCION);
                else
                    setValueHTML('lblContadorReferencia', K_CANTIDAD_DIRECCION);
            }
            txt.value = salida;
        }
        function onchange_edificacion(cbo) {
            var pos_usc;

            //ddlEdificacion
            cbo = document.getElementById('ddlEdificacion');
            setValue('txtManzana', '');
            setValue('txtLote', '');
            var total = contador_d('D', pos_usc);

            if (cbo.value != '-1') {
                if (total > K_CANTIDAD_DIRECCION) {
                    setValueHTML('lblContadorDireccion', K_CANTIDAD_DIRECCION);
                    cbo.value = '-1';
                    return;
                } else {
                    total = parseInt(total, 10) - 3;
                    if (total == 0) total = '';
                    setValueHTML('lblContadorDireccion', total);
                }
                setEnabled('txtManzana', true, 'clsInputEnable');
                setEnabled('txtLote', true, 'clsInputEnable');
            } else {
                setEnabled('txtManzana', false, 'clsInputDisable');
                setEnabled('txtLote', false, 'clsInputDisable');
            }
        }
        function onkeyup_mz_lote(txt, flagReferencia) {
            var pos_usc;
            //adicionado cuando es llamado de otro user control
            //gaa20120206
            //if(txt.id.indexOf("Usc_direccionInstalacion") != -1 && flagReferencia == 'R')
            //	K_CANTIDAD_DIRECCION = 236;
            //else
            //	K_CANTIDAD_DIRECCION = 40;
            //fin gaa20120206
            if (document.all) {
                var salida = txt.value;
                salida = eliminaCaracteresInvalidos(salida);
                var total = contador_d(flagReferencia, pos_usc);
                if (total > K_CANTIDAD_DIRECCION) {
                    salida = salida.substr(0, salida.length - 1);
                    if (flagReferencia == 'D')
                        setValueHTML('lblContadorDireccion', K_CANTIDAD_DIRECCION);
                    else
                        setValueHTML('lblContadorReferencia', K_CANTIDAD_DIRECCION);
                }
                txt.value = salida;
            }
            //gaa20120206
            K_CANTIDAD_DIRECCION = 40;
            //fin gaa20120206
        }

        function onchange_interior(cbo) {
            var pos_usc;

            //ddlTipoInterior
            cbo = document.getElementById('ddlTipoInterior');
            setValue('txtNroInterior', '');
            var total = contador_d('D', pos_usc);
            if (cbo.value != '-1') {
                if (total > K_CANTIDAD_DIRECCION) {
                    setValueHTML('lblContadorDireccion', total);
                    cbo.value = '-1';
                    return;
                } else {
                    total = parseInt(total, 10) - 4;
                    if (total == 0) total = '';
                    setValueHTML('lblContadorDireccion', total);
                }
                setEnabled('txtNroInterior', true, 'clsInputEnable');
            } else {
                setEnabled('txtNroInterior', false, 'clsInputDisable');
            }
        }
        function onchange_urbanizacion(cbo) {
            var pos_usc;
            //adicionado cuando es llamado de otro user control

            //gaa20120206
            //if(cbo.id.indexOf("Usc_direccionInstalacion") != -1)
            //	K_CANTIDAD_DIRECCION = 236;
            //else
            //	K_CANTIDAD_DIRECCION = 40;
            //fin gaa20120206

            //ddlUrbanizacion
            cbo = document.getElementById('ddlUrbanizacion');

			//INICIO INICIATIVA-932
            if (getValue('hidFlagIFI') == '1') {
				if (getValue('hidTipoProducto') != codTipoProductoInterInalam) {
            setValue('txtUrbanizacion', '');
	            }
			}
			else{
            setValue('txtUrbanizacion', '');
			}
			//FIN INICIATIVA-932

            var total = contador_d('R', pos_usc);
            if (cbo.value != '-1') {
                if (total > K_CANTIDAD_DIRECCION) {
                    setValueHTML('lblContadorReferencia', K_CANTIDAD_DIRECCION);
                    cbo.value = '-1';
                    return;
                } else {
                    total = parseInt(total, 10) - 4;
                    if (total == 0) total = '';
                    setValueHTML('lblContadorReferencia', total);
                }

				//INICIO INICIATIVA-932
                if (getValue('hidFlagIFI') == '1') {
					if (getValue('hidTipoProducto') != codTipoProductoInterInalam) {
                setEnabled('txtUrbanizacion', true, 'clsInputEnable');
	                }
				}
				else{
					setEnabled('txtUrbanizacion', true, 'clsInputEnable');
				}
				//FIN INICIATIVA-932

            } else {
                setEnabled('txtUrbanizacion', false, 'clsInputDisable');
            }
            //gaa20120206
            K_CANTIDAD_DIRECCION = 40;
            //fin gaa20120206
        }
        function onchange_zona(cbo) {
            var pos_usc;
            //adicionado cuando es llamado de otro user control

            //gaa20120206
            //if(cbo.id.indexOf("Usc_direccionInstalacion") != -1)
            //	K_CANTIDAD_DIRECCION = 236;
            //else
            //	K_CANTIDAD_DIRECCION = 40;
            //fin gaa20120206
            cbo = document.getElementById('ddlZona');
            setValue('txtNombreZona', '');
            var total = contador_d('R', pos_usc);
            if (cbo.value != '-1') {
                if (total > K_CANTIDAD_DIRECCION) {
                    setValueHTML('lblContadorReferencia', K_CANTIDAD_DIRECCION);
                    cbo.value = '-1';
                    return;
                } else {
                    total = parseInt(total, 10) - 4;
                    if (total == 0) total = '';
                    setValueHTML('lblContadorReferencia.ClientId', total);
                }
                setEnabled('txtNombreZona', true, 'clsInputEnable');
            } else {
                setEnabled('txtNombreZona', false, 'clsInputDisable');
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
                var tipoVia = getValue('ddlPrefijo');
                var nombreVia = getValue('txtDireccion');
                var nroPuerta = getValue('txtNroPuerta');
                var tipoEdificacion = getValue('ddlEdificacion');
                var nroEdificacion = getValue('txtManzana');
                var lote = getValue('txtLote');
                var tipoInterior = getValue('ddlTipoInterior');
                var nroInterior = getValue('txtNroInterior');
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
                setValueHTML('lblContadorDireccion', total);
            } else if (flagReferencia == 'R') {//Para la Referencia
                var tipoUrbanizacion = getValue('ddlUrbanizacion');
                var nombreUrbanizacion = getValue('txtUrbanizacion');
                var tipoZona = getValue('ddlZona');
                var nombreZona = getValue('txtNombreZona');
                var referencia = getValue('txtReferencia');
                if (tipoUrbanizacion != '-1') completaR += ' ' + tipoUrbanizacion;
                if (nombreUrbanizacion != '') completaR += ' ' + nombreUrbanizacion;
                if (tipoZona != '-1') completaR += ' ' + tipoZona;
                if (nombreZona != '') completaR += ' ' + nombreZona;
                if (referencia != '') completaR += ' ' + referencia;
                total = '';
                if (completaR != '') total = trim(completaR).length;
                setValueHTML('lblContadorReferencia', total);
            }
            if (total == '') total = 0;
            return total;
        }


        function eliminaCaracteresInvalidos(cadena) {
            //gaa20130520
            //var invalidos = "\/°~#+!$%=?¿¡|;*\'\\\""
            var invalidos = "´&()¨{[}]:\/°~#+!$%=?¿¡|;*\'\\\""
            //fin gaa20130520	
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

            var distrito;
            var ubigeoINEI = '';
            distrito = document.getElementById('ddlDistrito');
            //if(ddldist.id.indexOf("Usc_direccion_dth") == -1)
            //{
            //	ubigeoINEI = distrito[distrito.selectedIndex].id;
            //}	

            if (ubigeoINEI == '' || ubigeoINEI == 'undefined') {
                ubigeoINEI = obtenerUbigeoINEI(distrito.value, pos_usc);
                if (ubigeoINEI == 'undefined') ubigeoINEI = '';
            }
            setValue('hidUbigeoINEI', ubigeoINEI);
            setValue('hidDistritoId', distrito.value);

            document.getElementById('hidDistritoId').value = ddldist.value;
            return ubigeoINEI;
        }

        function obtenerUbigeoINEI(distrito_id, pos_usc) {
            if (distrito_id == '-1' || distrito_id == '') return '';
            var cad = getValue('hidListUbigeoINEI');

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
        ///****hasta aqui

        //** nuevos para domicilio
        function ListarCentroPoblado() {
            if ((document.getElementById('hidTipoProducto').value == codTipoProductoDTH) || (document.getElementById('hidTipoProducto').value == codTipoProd3PlayInalam)) {
                var ddlCentroPoblado = document.getElementById('ddlCentroPoblado');
                ddlCentroPoblado.length = 0;

                var _serie = document.getElementById('hidUbigeoINEI').value;

                if (_serie == "") {
                    return;
                }

                document.getElementById('txtCodPlano').value = '';
                document.getElementById('ifrGrillaCentroPoblado').width = "0%";
                document.getElementById('ifrGrillaCentroPoblado').height = "0%";

                if (document.getElementById("chkDTH").checked) {
                    var _chkDTH = 1;
                }
                else{
                    var _chkDTH = 0;
                }

                if (document.getElementById("chkLTE").checked) {
                    var _chkLTE = 1;
                }
                else {
                    var _chkLTE = 0;
                }

                document.getElementById("ifrConsultaSerie").src = "../frames/sisact_ifr_CentroPoblado.aspx?codSerie=" + _serie + "&iDTH=" + _chkDTH + "&iLTE=" + _chkLTE + "&CodProd=" + document.getElementById('hidTipoProducto').value;
            
                document.getElementById("ifrConsultaSerie").contentWindow.opener = window.opener;
            }
            else
                limpiarGrilla();
        }

        function LlenarDatosCombo(datos, rspta) {
            var ddlCentroPoblado = document.getElementById('ddlCentroPoblado');
            ddlCentroPoblado.length = 0;

            var oElm;
            oElm = document.createElement("OPTION");
            oElm.value = "";
            oElm.text = "--Seleccione--";
            ddlCentroPoblado.add(oElm);

            if (rspta == "Error" || datos == '') {
                alert('El distrito seleccionado no cuenta con cobertura.');
            }
            else {
                var arrayDatos = datos.split('|');
                for (var i = 0; i < arrayDatos.length; i++) {
                    var opcion = arrayDatos[i].split(';');
                    oElm = document.createElement("OPTION");
                    oElm.value = opcion[0];
                    oElm.text = opcion[1];
                    ddlCentroPoblado.add(oElm);
                }
            }
        }

        function MostrarGrillaCentropoblado() {
            //gaa20131029
            if ('<%=ConfigurationManager.AppSettings["constIrMapaWeb"] %>' == '1') {
                llamarMapaWeb();
                return;
            }
            //fin gaa20131029
            var ubigeo = document.getElementById('hidUbigeoINEI').value;
            if (document.getElementById('ddlDistrito').value != '')
                //document.getElementById('ifrGrillaCentroPoblado').src = "../frames/sisact_ifr_GrillaCentroPoblado.aspx?strValor=" + ubigeo;
                document.getElementById('ifrGrillaCentroPoblado').src = "../frames/sisact_ifr_GrillaCentroPoblado.aspx?strValor=" + ubigeo + "&CodProd=" + document.getElementById('hidTipoProducto').value; //FTTH -Cod. Producto Plano
            else {
                alert('Seleccione un distrito');
                return false;
            }
        }

        function validarCargaGrilla(datos, rspta, nroFilas) {
            if (rspta == "ERROR") {
                alert('El Centro Poblado seleccionado, no existen Planos asociados.');
                document.getElementById('ifrGrillaCentroPoblado').width = "0%";
                document.getElementById('ifrGrillaCentroPoblado').height = "0%";
            }
            else {
                document.getElementById('ifrGrillaCentroPoblado').width = "100%";
                if (parseInt(nroFilas, 10) > 9)
                    document.getElementById('ifrGrillaCentroPoblado').height = "270px";
                else {
                    var size = (parseInt(nroFilas) + parseInt(2)) * parseInt(22) + parseInt(5);
                    document.getElementById('ifrGrillaCentroPoblado').height = size + "px";
                }
            }
        }
        //gaa20130906
        function MostrarGrillaEdificio() {
            var strCodPlano = document.getElementById('txtCodPlano').value;
            var strCodEdificio = document.getElementById('txtCodEdificio').value;
            if (strCodPlano != "")
                document.getElementById('ifrGrillaCentroPoblado').src = "../frames/sisact_ifr_GrillaEdificio.aspx?strCodPlano=" + strCodPlano + "&strCodEdificio=" + strCodEdificio;
            else {
                alert('Seleccione un Plano');
                setValue('txtCodEdificio', '');
                return false;
            }
        }

        function MostrarGrillaEdificioA() {
            if (!document.getElementById('btnCargarGrillaEdificio').disabled)
                MostrarGrillaEdificio();
        }

        function validarCargaGrillaEdificio(datos, rspta, nroFilas) {
            if (rspta == "ERROR") {
                alert('No existe el código del edificio ingresado.');
                document.getElementById('txtCodEdificio').value = '';
                document.getElementById('ifrGrillaCentroPoblado').width = "0%";
                document.getElementById('ifrGrillaCentroPoblado').height = "0%";
            }
            else {
                document.getElementById('ifrGrillaCentroPoblado').width = "100%";
                if (parseInt(nroFilas, 10) > 9)
                    document.getElementById('ifrGrillaCentroPoblado').height = "270px";
                else {
                    var size = (parseInt(nroFilas) + parseInt(2)) * parseInt(22) + parseInt(5);
                    document.getElementById('ifrGrillaCentroPoblado').height = size + "px";
                }
            }
        }

        function asignarValorEdificio(datos, rspta) {
            document.getElementById('txtCodEdificio').value = '';
            document.getElementById('txtCodEdificio').value = datos;
        }
        //fin gaa20130906			
        function autoSizeIframe() {
            var id = document.getElementById('ifrGrillaCentroPoblado');
            if (!window.opera && document.all && document.getElementById) {
                id.style.height = id.contentWindow.document.body.scrollHeight;
            } else if (document.getElementById) {
                id.style.height = id.contentDocument.body.scrollHeight + "px";
            }
        }

        function limpiarGrilla() {
            //INICIO INICIATIVA-932
            if (document.getElementById('hidTipoProducto').value != codTipoProductoInterInalam) {
            document.getElementById('ifrGrillaCentroPoblado').width = "0%";
            document.getElementById('ifrGrillaCentroPoblado').height = "0%";
            document.getElementById('txtCodPlano').value = '';
        }
            //FIN INICIATIVA-932
        }

        function asignarValorCentroPoblado(datos, flgVOD, rspta) {
            document.getElementById('txtCodPlano').value = '';
            document.getElementById('txtCodPlano').value = datos;

            if (flgVOD.length == 0)
                flgVOD = 0;

            document.getElementById('hidFlagVOD').value = flgVOD;
        }

        /*función que cierra la ventana*/
        function f_CerrarVentana() {
            window.close();
            return false;
        }

        function llamarPadre() {
            window.opener.refrescarPlanes();
            window.close();
        }

        function flgReadOnly() {
            setEnabled('ddlPrefijo', false, 'clsSelectDisable');
            setEnabled('ddlTipoInterior', false, 'clsSelectDisable');
            setEnabled('ddlUrbanizacion', false, 'clsSelectDisable');
            setEnabled('ddlTipoDomicilio', false, 'clsSelectDisable');
            setEnabled('ddlZona', false, 'clsSelectDisable');
            setEnabled('ddlDepartamento', false, 'clsSelectDisable');
            setEnabled('ddlProvincia', false, 'clsSelectDisable');
            setEnabled('ddlDistrito', false, 'clsSelectDisable');
            setEnabled('ddlCentroPoblado', false, 'clsSelectDisable');
            setEnabled('txtReferencia', false, 'clsInputDisable');
            setEnabled('txtPrefijoTelefonoReferencia', false, 'clsInputDisable');
            setEnabled('txtTelefonoReferencia', false, 'clsInputDisable');
            setEnabled('txtCodPlano', false, 'clsInputDisable');
            setEnabled('txtRefSecundaria', false, 'clsInputDisable');
            setEnabled('btnCargarGrilla', false, '');
            document.getElementById('chkVentaProactiva').disabled = true;
            document.getElementById('chkVtaProgramada').disabled = true;
            setEnabled('txtVendedorDNI', false, 'clsInputDisable');
            //gaa20130906
            setEnabled('txtCodEdificio', false, 'clsInputDisable');
            setEnabled('btnCargarGrillaEdificio', false, '');
            //fin gaa20130906
        }
        function f_EditarDireccion() {
            setEnabled('ddlPrefijo', true, 'clsSelectEnable');
            setEnabled('ddlTipoInterior', true, 'clsSelectEnable');
            setEnabled('ddlUrbanizacion', true, 'clsSelectEnable');
            setEnabled('ddlTipoDomicilio', true, 'clsSelectEnable');
            setEnabled('ddlZona', true, 'clsSelectEnable');
            setEnabled('ddlDepartamento', false, 'clsSelectDisable');
            setEnabled('ddlProvincia', false, 'clsSelectDisable');
            setEnabled('ddlDistrito', false, 'clsSelectDisable');
            setEnabled('ddlCentroPoblado', false, 'clsSelectDisable');
            setEnabled('txtReferencia', true, 'clsInputEnable');
            setEnabled('txtPrefijoTelefonoReferencia', true, 'clsInputEnable');
            setEnabled('txtTelefonoReferencia', true, 'clsInputEnable');
            setEnabled('txtCodPlano', false, 'clsInputDisable');
            setEnabled('txtRefSecundaria', true, 'clsInputEnable');
            setEnabled('btnCargarGrilla', false, '');
            document.getElementById('chkVentaProactiva').disabled = true;
            document.getElementById('chkVtaProgramada').disabled = true;
            setEnabled('txtVendedorDNI', false, 'clsInputDisable');
            //gaa20130906
            setEnabled('txtCodEdificio', false, 'clsInputDisable');
            setEnabled('btnCargarGrillaEdificio', false, '');
            //fin gaa20130906
            setEnabled('divSinNumero', true, ''); 

            if (getValue('txtDireccion') != '')
                setEnabled('txtDireccion', true, 'clsInputEnable');
            if (getValue('txtNroPuerta') != '' && getValue('txtNroPuerta') != 'S/N')
                setEnabled('txtNroPuerta', true, 'clsInputEnable');
            if (getValue('txtManzana') != '')
                setEnabled('txtManzana', true, 'clsInputEnable');
            if (getValue('txtLote') != '')
                setEnabled('txtLote', true, 'clsInputEnable');
            if (getValue('txtNroInterior') != '')
                setEnabled('txtNroInterior', true, 'clsInputEnable');
            if (getValue('txtUrbanizacion') != '')
                setEnabled('txtUrbanizacion', true, 'clsInputEnable');
            if (getValue('txtNombreZona') != '')
                setEnabled('txtNombreZona', true, 'clsInputEnable');
            if (getValue('txtReferencia') != '')
                setEnabled('txtReferencia', true, 'clsInputEnable');
            if (getValue('txtRefSecundaria') != '')
                setEnabled('txtRefSecundaria', true, 'clsInputEnable');
        }

        //IdCodPlano
        function HideCodPlano() {
            document.getElementById('IdCodPlano').style.display = 'none';
            //gaa20130517
            document.getElementById('tdVentaProactiva').style.display = '';

            if (getValue('txtVendedorDNI').length > 0) {
                document.getElementById('chkVentaProactiva').checked = true;
                divVendedorDNI.style.display = '';
            }
            //fin gaa20130517
        }

        function btnAceptar_Click() {
            window.close();
        }

        function validarRetorno() {
            //gaa20130517
            if (getValue('hidAceptarSinValidar') == 'S')
                return true;
            //gaa20130517	
            var inp = document.getElementById('ddlCentroPoblado').value;
            if (document.getElementById('hidFlgReadOnly').value == 'N' || getValue('hidEditarDirecion') == "S") {
                if (ValidarDireccion('', '3', '') == '0') {
                    return false;
                }
                //gaa20130517
                if (document.getElementById('chkVentaProactiva').checked) {
                    if (getValue('txtVendedorDNI').length != 8) {
                        alert('El vendedor no es válido');
                        return false;
                    }
                    //gaa20130918						
                    //ValidarVendedorDNI();
                    if (getValue('hidTipoProducto') == codTipoProductoHFC)
                        ValidarVendedorDNIHFC();
                    else
                        ValidarVendedorDNI();
                    //fin gaa20130918							
                    return false;
                }
                //fin gaa20130517              
            }
            if (getValue('hidEditarDirecion') == 'S') {
                setEnabled('ddlPrefijo', true, 'clsSelectEnable');
                setEnabled('ddlTipoInterior', true, 'clsSelectEnable');
                setEnabled('ddlUrbanizacion', true, 'clsSelectEnable');
                setEnabled('ddlTipoDomicilio', true, 'clsSelectEnable');
                setEnabled('ddlZona', true, 'clsSelectEnable');
                setEnabled('ddlDepartamento', true, 'clsSelectDisable');
                setEnabled('ddlProvincia', true, 'clsSelectDisable');
                setEnabled('ddlDistrito', true, 'clsSelectDisable');
                setEnabled('ddlCentroPoblado', true, 'clsSelectDisable');
                cargarImagenEsperando();
            }
            
			if (getValue('hidTipoProducto') == codTipoProductoInterInalam) {
                if (getValue('hidFlagIFI') == '1') {
                    setEnabled('ddlDepartamento', true, 'clsSelectEnable0');
                    setEnabled('ddlProvincia', true, 'clsSelectEnable0');
                    setEnabled('ddlDistrito', true, 'clsSelectEnable0');
                    setEnabled('ddlPrefijo', true, 'clsSelectEnable0');
                }
            }
			
            return true;
        }

        function ValidarDireccion(Panel, NroPanel, Prefijo) {
            Panel = 'tdPanel_3';
            var ArrayInput = document.getElementById(Panel).getElementsByTagName('INPUT');
            var ArraySelect = document.getElementById(Panel).getElementsByTagName('SELECT');
            var strValida = '1';
            var NroPuerta = false;
            var strddlEdificacion;

            // For Para revisar el contenido de los Select Departamento, Provincia, Distrito
            for (i = 0; i < ArraySelect.length; i++) {
                if (ArraySelect[i].id == "ddlPrefijo" && (ArraySelect[i].value == '' || ArraySelect[i].value == '-1')) {
                    if (NroPanel == '3') {
                        strValida = '0';
                        alert("Seleccione un Tipo de domicilio de la Dirección de Instalacion");
                    }
                    ArraySelect[i].focus();
                    return strValida;
                }
                if (ArraySelect[i].id == "ddlDepartamento" && (ArraySelect[i].value == '' || ArraySelect[i].value == '-1')) {
                    if (NroPanel == '3') {
                        strValida = '0';
                        alert("Seleccione un Departamento de la Dirección del Cliente");
                    }
                    else if (NroPanel == '5') {
                        strValida = '0';
                        alert("Seleccione un Departamento de la Dirección de Instalacion");
                    }
                    else if (NroPanel == '4') {
                        strValida = '0';
                        alert("Seleccione un Departamento de la Dirección de Facturación");
                    }
                    ArraySelect[i].focus();
                    return strValida;
                }
                if (ArraySelect[i].id == "ddlProvincia" && ArraySelect[i].value == '') {
                    if (NroPanel == '3') {
                        strValida = '0';
                        alert("Seleccione una Provincia de la Dirección del Cliente");
                    }
                    else if (NroPanel == '5') {
                        strValida = '0';
                        alert("Seleccione una Provincia de la Dirección de Instalacion");
                    }
                    else if (NroPanel == '4') {
                        strValida = '0';
                        alert("Seleccione una Provincia de la Dirección de Facturación");
                    }
                    ArraySelect[i].focus();
                    return strValida;
                }
                if (ArraySelect[i].id == "ddlDistrito" && ArraySelect[i].value == '') {
                    if (NroPanel == '3') {
                        strValida = '0';
                        alert("Seleccione un Distrito de la Dirección del Cliente");
                    }
                    else if (NroPanel == '5') {
                        strValida = '0';
                        alert("Seleccione un Distrito de la Dirección de Instalacion");
                    }
                    else if (NroPanel == '4') {
                        strValida = '0';
                        alert("Seleccione un Distrito de la Dirección de Facturación");
                    }
                    ArraySelect[i].focus();
                    return strValida;
                }

                if (ArraySelect[i].id == "ddlUrbanizacion" && ArraySelect[i].value == '-1') {
                    if (NroPanel == '3') {
                        strValida = '0';
                        alert("Seleccione un tipo de Urbanización de la Dirección de Instalación");
                    }
                    ArraySelect[i].focus();
                    return strValida;
                }

                if (document.getElementById('ddlTipoDomicilio').value == '' || document.getElementById('ddlTipoDomicilio').value == '-1') {
                    if (NroPanel == '3') {
                        strValida = '0';
                        alert("Seleccione un Tipo de domicilio de la Dirección del Cliente");
                    }
                    else if (NroPanel == '5') {
                        strValida = '0';
                        alert("Seleccione un Tipo de domicilio de la Dirección de Instalacion");
                    }
                    else if (NroPanel == '4') {
                        strValida = '0';
                        alert("Seleccione un Tipo de domicilio de la Dirección de Facturación");
                    }
                    document.getElementById('ddlTipoDomicilio').focus();
                    return strValida;
                }
                if (getValue('hidTipoProducto') == codTipoProductoDTH || getValue('hidTipoProducto') == codTipoProd3PlayInalam) {
                    if (document.getElementById('ddlCentroPoblado').value == '' || document.getElementById('ddlCentroPoblado').value == '-1') {
                        if (NroPanel == '3') {
                            strValida = '0';
                            alert("Seleccione un Centro Poblado de la Dirección del Cliente");
                        }
                        else if (NroPanel == '5') {
                            strValida = '0';
                            alert("Seleccione un Centro Poblado de la Dirección de Instalacion");
                        }
                        else if (NroPanel == '4') {
                            strValida = '0';
                            alert("Seleccione un Centro Poblado de la Dirección de Facturación");
                        }
                        document.getElementById('ddlCentroPoblado').focus();
                        return strValida;
                    }
                }
            }
            // For Para revisar el contenido de los Select Departamento, Provincia, Distrito

            // For Para revisar el contenido de los Inputs
            for (i = 0; i < ArrayInput.length; i++) {
                //Solo los Text
                if (ArrayInput[i].type == "text") {
                    //campos que son obligatorios
                    //Direccion
                    if (ArrayInput[i].id == "txtDireccion" && ArrayInput[i].value == '') {
                        if (ArrayInput[i].className == 'clsInputDisable') {
                            if (NroPanel == '3') {
                                strValida = '0';
                                alert("Seleccione una Av/Calle/Jr de la Dirección del Cliente");
                            }
                            else if (NroPanel == '5') {
                                strValida = '0';
                                alert("Seleccione una Av/Calle/Jr de la Dirección de Instalacion");
                            }
                            else if (NroPanel == '4') {
                                strValida = '0';
                                alert("Seleccione una Av/Calle/Jr de la Dirección de Facturación");
                            }
                            ArrayInput[i].focus();
                            return strValida;
                        }
                        else {
                            var strAbr = document.getElementById(Panel).all["ddlPrefijo"].value;
                            if (strAbr == '<%= ConfigurationManager.AppSettings["constPrefijoCalleSinNombre"] %>') {
                                var strUrb = document.getElementById(Panel).all["ddlUrbanizacion"].value;
                                if (strUrb == '-1') {
                                    switch (NroPanel) {
                                        case "3":
                                            strValida = '0';
                                            alert("Debe indicar la Urbanización de la Dirección del Cliente");
                                            break;
                                        case "5":
                                            strValida = '0';
                                            alert("Debe indicar la Urbanización de la Dirección de Instalacion");
                                            break;
                                        case "4":
                                            strValida = '0';
                                            alert("Debe indicar la Urbanización de la Dirección de Facturación");
                                            break;
                                    }
                                    return strValida;
                                }
                            }
                            else {
                                if (NroPanel == '3') {
                                    strValida = '0';
                                    alert("Ingrese la Dirección del Cliente");
                                }
                                else if (NroPanel == '5') {
                                    strValida = '0';
                                    alert("Ingrese la Dirección de Instalacion");
                                }
                                else if (NroPanel == '4') {
                                    strValida = '0';
                                    alert("Ingrese la Dirección de Facturación");
                                }
                                ArrayInput[i].focus();
                                return strValida;
                            }
                        }
                    }
                    //Direccion
                    //Nro Puerta
                    if (ArrayInput[i].id == "txtNroPuerta") {
                        if (ArrayInput[i].value == '') {
                            if (NroPanel == '3') {
                                strValida = '0';
                                alert("Ingrese el Número de la Dirección del Cliente");
                            }
                            else if (NroPanel == '5') {
                                strValida = '0';
                                alert("Ingrese el Número de la Dirección de Instalacion");
                            }
                            else if (NroPanel == '4') {
                                strValida = '0';
                                alert("Ingrese el Número de la Dirección de Facturación");
                            }
                            ArrayInput[i].focus();
                            return strValida;
                        }
                        else if (ArrayInput[i].value != 'S/N' && ArrayInput[i].value != '') {
                            NroPuerta = true;
                        }
                    }
                    //Nro Puerta
                    //Referencia
                    if (ArrayInput[i].id == "txtReferencia" && ArrayInput[i].value == '') {
                        if (NroPanel == '3') {
                            strValida = '0';
                            alert("Ingrese una Referencia de la Dirección del Cliente");
                        }
                        else if (NroPanel == '5') {
                            strValida = '0';
                            alert("Ingrese una Referencia de la Dirección de Instalacion");
                        }
                        else if (NroPanel == '4') {
                            strValida = '0';
                            alert("Ingrese una Referencia de la Dirección de Facturación");
                        }
                        ArrayInput[i].focus();
                        return strValida;
                    }
                    //Referencia
                    //Telefono Referencia
                    //Prefijo
                    if (ArrayInput[i].id == "txtPrefijoTelefonoReferencia" && ArrayInput[i].value == '') {
                        if (NroPanel == '3') {
                            strValida = '0';
                            alert("Ingrese el Código de Teléfono del Cliente");
                        }
                        else if (NroPanel == '5') {
                            strValida = '0';
                            alert("Ingrese el Código de Teléfono de Instalacion");
                        }
                        else if (NroPanel == '4') {
                            strValida = '0';
                            alert("Ingrese el Código de Teléfono de Facturación");
                        }
                        ArrayInput[i].focus();
                        return strValida;
                    }
                    else if (ArrayInput[i].id == "txtPrefijoTelefonoReferencia" && ArrayInput[i].value != '') {
                        if (ArrayInput[i].value.length < 2) {
                            if (NroPanel == '3') {
                                strValida = '0';
                                alert("Verifique el Código de Teléfono del Cliente, debe considerar minimo 2 dígitos");
                            }
                            else if (NroPanel == '5') {
                                strValida = '0';
                                alert("Verifique el Código de Teléfono de Instalacion, debe considerar minimo 2 dígitos");
                            }
                            else if (NroPanel == '4') {
                                strValida = '0';
                                alert("Verifique el Código de Teléfono de facturación, debe considerar minimo 2 dígitos");
                            }
                            ArrayInput[i].focus();
                            return strValida;
                        }
                        if (ArrayInput[i].value == '0' || ArrayInput[i].value == '00') {
                            if (NroPanel == '3') {
                                strValida = '0';
                                alert("Verifique el Código de Teléfono del Cliente, no es un valor válido");
                            }
                            else if (NroPanel == '5') {
                                strValida = '0';
                                alert("Verifique el Código de Teléfono de Instalacion, no es un valor válido");
                            }
                            else if (NroPanel == '4') {
                                strValida = '0';
                                alert("Verifique el Código de Teléfono de Facturación, no es un valor válido");
                            }
                            ArrayInput[i].focus();
                            return strValida;
                        }
                    }
                    //Prefijo
                    //Nro Telefono
                    if (ArrayInput[i].id == "txtTelefonoReferencia" && ArrayInput[i].value == '') {
                        if (NroPanel == '3') {
                            strValida = '0';
                            alert("Ingrese el Número de Teléfono del Cliente");
                        }
                        else if (NroPanel == '5') {
                            strValida = '0';
                            alert("Ingrese el Número de Teléfono de Instalacion");
                        }
                        else if (NroPanel == '4') {
                            strValida = '0';
                            alert("Ingrese el Número de Teléfono de Facturación");
                        }
                        ArrayInput[i].focus();
                        return strValida;
                    }
                    else if (ArrayInput[i].id == "txtTelefonoReferencia" && ArrayInput[i].value != '') {
                        if (ArrayInput[i].value.length < 6) {
                            if (NroPanel == '3') {
                                strValida = '0';
                                alert("Verifique el Teléfono del Cliente, debe considerar minimo 6 dígitos");
                            }
                            else if (NroPanel == '5') {
                                strValida = '0';
                                alert("Verifique el Teléfono de Instalacion, debe considerar minimo 6 dígitos");
                            }
                            else if (NroPanel == '4') {
                                strValida = '0';
                                alert("Verifique el Teléfono de Facturación, debe considerar minimo 6 dígitos");
                            }
                            ArrayInput[i].focus();
                            return strValida;
                        }
                    }
                    //Nro Telefono
                    //Telefono Referencia
                    //campos que son obligatorios

                    //Campos Opcionales
                    //Manzana
                    if (ArrayInput[i].id == "txtManzana" && ArrayInput[i].value == '') {
                        if (ArrayInput[i].className == 'clsInputDisable' && !NroPuerta) {
                            if (NroPanel == '3') {
                                strValida = '0';
                                alert("Seleccione una Mz/Bloq/Edif de la Dirección del Cliente");
                            }
                            else if (NroPanel == '5') {
                                strValida = '0';
                                alert("Seleccione una Mz/Bloq/Edif de la Dirección de Instalacion");
                            }
                            else if (NroPanel == '4') {
                                strValida = '0';
                                alert("Seleccione una Mz/Bloq/Edif de la Dirección de Facturación");
                            }
                            ArrayInput[i].focus();
                            return strValida;
                        }
                        else if (ArrayInput[i].className == 'clsInputEnable') {
                            if (NroPanel == '3') {
                                strValida = '0';
                                alert("Ingrese Nro de Mz/Bloq/Edif de la Dirección del Cliente");
                            }
                            else if (NroPanel == '5') {
                                strValida = '0';
                                alert("Ingrese Nro de Mz/Bloq/Edif de la Dirección de Instalacion");
                            }
                            else if (NroPanel == '4') {
                                strValida = '0';
                                alert("Ingrese Nro de Mz/Bloq/Edif de la Dirección de Facturación");
                            }
                            ArrayInput[i].focus();
                            return strValida;
                        }
                    }
                    //Manzana
                    //Lote
                    if (ArrayInput[i].id == "txtLote" && ArrayInput[i].value == '' && ArrayInput[i].className == 'clsInputEnable') {
                        if (NroPanel == '3') {
                            strValida = '0';
                            alert("Ingrese Nro de Lote de la Dirección del Cliente");
                        }
                        else if (NroPanel == '5') {
                            strValida = '0';
                            alert("Ingrese Nro de Lote de la Dirección de Instalacion");
                        }
                        else if (NroPanel == '4') {
                            strValida = '0';
                            alert("Ingrese Nro de Lote de la Dirección de Facturación");
                        }
                        ArrayInput[i].focus();
                        return strValida;
                    }
                    //Lote
                    //Nro Interior
                    if (ArrayInput[i].id == "txtNroInterior" && ArrayInput[i].value == '' && ArrayInput[i].className == 'clsInputEnable') {
                        if (NroPanel == '3') {
                            strValida = '0';
                            alert("Ingrese Nro de Dpto/Int de la Dirección del Cliente");
                        }
                        else if (NroPanel == '5') {
                            strValida = '0';
                            alert("Ingrese Nro de Dpto/Int de la Dirección de Instalacion");
                        }
                        else if (NroPanel == '4') {
                            strValida = '0';
                            alert("Ingrese Nro de Dpto/Int de la Dirección de Facturación");
                        }
                        ArrayInput[i].focus();
                        return strValida;
                    }
                    //Nro Interior
                    //Urbanizacion
                    if (ArrayInput[i].id == "txtUrbanizacion" && ArrayInput[i].value == '' && ArrayInput[i].className == 'clsInputEnable') {
                        if (NroPanel == '3') {
                            strValida = '0';
                            alert("Ingrese el nombre de la Urbanización de la Dirección del Cliente");
                        }
                        else if (NroPanel == '5') {
                            strValida = '0';
                            alert("Ingrese el nombre de la Urbanización de la Dirección de Instalacion");
                        }
                        else if (NroPanel == '4') {
                            strValida = '0';
                            alert("Ingrese el nombre de la Urbanización de la Dirección de Facturación");
                        }
                        ArrayInput[i].focus();
                        return strValida;
                    }
                    //Urbanizacion 
                    //Zona/Etapa
                    if (ArrayInput[i].id == "txtNombreZona" && ArrayInput[i].value == '' && ArrayInput[i].className == 'clsInputEnable') {
                        if (NroPanel == '3') {
                            strValida = '0';
                            alert("Ingrese el nombre de la Zona/Etapa de la Dirección del Cliente");
                        }
                        else if (NroPanel == '5') {
                            strValida = '0';
                            alert("Ingrese el nombre de la Zona/Etapa de la Dirección de Instalacion");
                        }
                        else if (NroPanel == '4') {
                            strValida = '0';
                            alert("Ingrese el nombre de la Zona/Etapa de la Dirección de Facturación");
                        }
                        ArrayInput[i].focus();
                        return strValida;
                    }

                    /********************************************************************\
                    /***************Verificar la validacion cuando es HFC/DTH************/
                    if (document.getElementById('hidFlgReadOnly').value == 'N') {
                        if (document.getElementById('hidTipoProducto').value == codTipoProductoHFC) {
                            if (document.getElementById('txtCodPlano').value.length == 0) {
                                if (NroPanel == '3') {
                                    strValida = '0';
                                    alert("Ingrese el código de Plano de la Dirección del Cliente");
                                }
                                document.getElementById('txtCodPlano').focus();
                                return strValida;
                            }
                        }
                    }
                }
                //Solo los Text
            }
            // For Para revisar el contenido de los Inputs
            return strValida;
        }

        function inicio() {
            quitarImagenEsperando();
			//INICIO INICIATIVA-932
            if (getValue('hidFlagIFI') == '0') {
            //PROY-140690 INICIO
            if (getValue('hidTipoProducto') == codTipoProductoInterInalam) {
				document.getElementById('lbDirectInstalacion').innerHTML = 'Dirección de uso del servicio IFI';
                document.getElementById('Table3').style.display = 'none';
                document.getElementById('txtRefenciaSecundaria').style.display = 'none';
                document.getElementById('lblRefenciaSecundaria').style.display = 'none';
            }
            //PROY-140690 FIN
			}
			//FIN INICIATIVA-932

            if (getValue('hidTipoProducto') == codTipoProductoDTH || getValue('hidTipoProducto') == codTipoProd3PlayInalam) {  //maquino
                HideCodPlano();
                document.getElementById('tdTxtCentroPoblado').style.display = '';
                document.getElementById('tdDdlCentroPoblado').style.display = '';
            }

            if (getValue('hidTipoProducto') == codTipoProd3PlayInalam) {
                document.getElementById('tdChkTec').style.visibility = "visible";
                document.getElementById('tdLblTec').style.visibility = "visible";
            }

            if (getValue('hidFlgReadOnly') == 'S') {
                document.getElementById('btnCancelar').value = 'Cerrar';
                document.getElementById('btnAceptar').style.display = 'none';
                if (getValue('hidEditarDirecion') == 'S') {
                    f_EditarDireccion();
                    document.getElementById('btnAceptar').value = 'Grabar';
                    document.getElementById('btnAceptar').style.display = '';
                } else {
                    flgReadOnly();
                }

            }
            else
                flgEditar();

			//INICIO INICIATIVA-932
            if (getValue('hidFlagIFI') == '1') {
				//PROY-140690 INICIO
	            if (getValue('hidTipoProducto') == codTipoProductoInterInalam) {
	                document.getElementById('lbDirectInstalacion').innerHTML = 'Dirección de uso del servicio IFI';
	                document.getElementById('Table3').style.display = 'none';
	                document.getElementById('txtRefenciaSecundaria').style.display = 'none';
	                document.getElementById('lblRefenciaSecundaria').style.display = 'none';
	
	                document.getElementById('trMapa').style.display = '';
	
	                setEnabled('txtDireccion', false, 'clsInputDisable');
	                setEnabled('divSinNumero', false, '');
	                setEnabled('ddlPrefijo', false, 'clsSelectDisable');
	                setEnabled('txtNroInterior', false, 'clsInputDisable');
	                setEnabled('txtUrbanizacion', false, 'clsInputDisable');
	                setEnabled('txtNombreZona', false, 'clsInputDisable');
	                setEnabled('txtNroPuerta', false, 'clsInputDisable');
	                setEnabled('ddlTipoInterior', false, 'clsSelectDisable');
	                setEnabled('ddlUrbanizacion', false, 'clsSelectDisable');
	                setEnabled('ddlTipoDomicilio', false, 'clsSelectDisable');
	                setEnabled('ddlZona', false, 'clsSelectDisable');
	                setEnabled('txtReferencia', false, 'clsInputDisable');
	                setEnabled('ddlDepartamento', false, 'clsSelectDisable');
	                setEnabled('ddlProvincia', false, 'clsSelectDisable');
	                setEnabled('ddlDistrito', false, 'clsSelectDisable');
	                setEnabled('txtPrefijoTelefonoReferencia', false, 'clsInputDisable');
	                setEnabled('txtTelefonoReferencia', false, 'clsInputDisable');
	                document.getElementById('btnAceptar').disabled = true;
	                document.getElementById('btnCancelar').disabled = true;
	
                    //if (getValue('hidTieneDireccion') == '0') {
                    //    document.getElementById('tdMantenerDireccion').style.display = '';
                    //}
                    //else {
                    //    document.getElementById('tdMantenerDireccion').style.display = 'none';
                    //}
	
	                /*FIN INICIATIVA -932 MOVILIDAD IFI*/
	            }
	            //PROY-140690 FIN
			}
			//FIN INICIATIVA-932

            if (getValue('hidRetornar').length > 0) {
                window.returnValue = getValue('hidRetornar') + '|' + getValue('hidFlagVOD');
                window.close();
            }

              if ((getValue('hidTipoProducto') == codTipoProductoDTH || getValue('hidTipoProducto') == codTipoProd3PlayInalam) && (getValue('hidVentaProactiva') == 'S' || getValue('hidNroSEC') > 0)) {
                document.getElementById('tdVentaProactiva').style.display = '';
                document.getElementById('divChkVentaProactiva').style.display = '';
            }

            if (getValue('hidTipoProducto') == codTipoProductoHFC) {
                document.getElementById('tdVentaProactiva').style.display = '';
                document.getElementById('divChkVentaProactiva').style.display = '';

                if (getValue('txtVendedorDNI').length > 0)
                    divVendedorDNI.style.display = '';
            }
        }

        function flgEditar() {
            if (!(getValue('ddlPrefijo') == '' || getValue('ddlPrefijo') == '-1')) {
                setEnabled('txtDireccion', true, 'clsInputEnable');
                setEnabled('divSinNumero', true, '');
            } else
                setEnabled('divSinNumero', false, '');

            if (!(getValue('ddlEdificacion') == '' || getValue('ddlEdificacion') == '-1')) {
                setEnabled('ddlEdificacion', true, 'clsSelectEnable0');
                setEnabled('txtManzana', true, 'clsInputEnable');
                setEnabled('txtLote', true, 'clsInputEnable');
            }

            if (!(getValue('ddlTipoInterior') == '' || getValue('ddlTipoInterior') == '-1')) {
                setEnabled('ddlTipoInterior', true, 'clsSelectEnable0');
                setEnabled('txtNroInterior', true, 'clsInputEnable');
            }

            if (!(getValue('ddlUrbanizacion') == '' || getValue('ddlUrbanizacion') == '-1'))
                setEnabled('txtUrbanizacion', true, 'clsInputEnable');

            if (!(getValue('ddlZona') == '' || getValue('ddlZona') == '-1'))
                setEnabled('txtNombreZona', true, 'clsInputEnable');

            if (getValue('txtNroPuerta') != 'S/N')
                setEnabled('txtNroPuerta', true, 'clsInputEnable');
        }

        function onkeyup_ReferenciaSec() {
            var maximo = 250;
            var texto = getValue('txtRefSecundaria');
            var tamanio = texto.length;
            if (tamanio >= maximo) {
                setValue('txtRefSecundaria', texto.substring(0, maximo));
                setValueHTML('lblContRefSecundaria', maximo);
            }
            else
                setValueHTML('lblContRefSecundaria', tamanio);
        }

        function onkeyUp_ReferenciaSec() {
            //gaa20130520
            var texto = getValue('txtRefSecundaria');
            if (event.keyCode != 32) {
                texto = eliminaCaracteresInvalidos(texto);
                setValue('txtRefSecundaria', texto);
            }
            //fin gaa20130520
        }
        //gaa20130517
        function mostrarVendedorDNI(chk) {
            if (chk.checked) {
                divVendedorDNI.style.display = '';

                if (getValue('hidTipoProducto') == codTipoProductoHFC) {
                    document.getElementById('lblVtaProgramada').style.display = 'none';
                    document.getElementById('chkVtaProgramada').style.display = 'none';
                }
            }
            else {
                divVendedorDNI.style.display = 'none';
                document.getElementById('txtVendedorDNI').value = '';
                document.getElementById('chkVtaProgramada').checked = false;
            }
        }

        function ValidarVendedorDNI() {
            //var serverURL = "../frames/sisact_ifr_evaluacion_dth.aspx";
            //RSExecute(serverURL, "ValidarVendedorDNI", getValue('txtVendedorDNI'), ValidarVendedorDNIReturn, CallbackError, "X");
            PageMethods.ValidarVendedorDNI(getValue('txtVendedorDNI'), callbacks.ValidarVendedorDNI);
        }

        function ValidarVendedorDNIHFC() {
            //var serverURL = "../frames/sisact_ifr_evaluacion_dth.aspx";
            //RSExecute(serverURL, "ValidarVendedorDNIHFC", getValue('txtVendedorDNI'), ValidarVendedorDNIReturn, CallbackError, "X");
            PageMethods.ValidarVendedorDNIHFC(getValue('txtVendedorDNI'), callbacks.ValidarVendedorDNI);
        }
        //fin gaa20130517
        //gaa20131003
        function llamarMapaWeb() {
            var ubigeo = getValue('hidUbigeoINEI'); //getValue('txtUbigeo1');
            var via = getValue('txtDireccion');
            var cuadra = getValue('txtNroPuerta');
            var agrupacion = getValue('txtUrbanizacion');
            var manzana = getValue('txtManzana');
            var usuario = '<%=CurrentUser %>';

            if (cuadra != '')
                cuadra = Math.floor(parseInt(cuadra) / 100);

            var estilo = "dialogHeight:900px;dialogWidth:1300px";
            var url = '<%= ConfigurationManager.AppSettings["constRutaMapaWeb"] %>'; //"http://172.19.74.153:8901/claroservmap/claroservmap.htm"
            url += "?accion=validarCobertura&usuario=" + usuario + "&ubigeo=" + ubigeo + "&via=" + via + "&cuadra=" + cuadra + "&agrupacion=" + agrupacion + "&manzana=" + manzana;
            var plano = window.showModalDialog(url, "ValidaCobertura", estilo);

            setTimeout('obtenerCookiePlano();', 1000);
        }

        function createCookie(name, value, days) {
            if (days) {
                var date = new Date();
                date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                var expires = "; expires=" + date.toGMTString();
            } else {
                var expires = "";
            }
            document.cookie = name + "=" + value + expires + "; path=/";
        }

        function readCookie(name) {
            var nameEQ = name + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ')
                    c = c.substring(1, c.length);
                if (c.indexOf(nameEQ) == 0)
                    return c.substring(nameEQ.length, c.length);
            }
            return null;
        }

        function eraseCookie(name) {
            createCookie(name, "", -1);
        }

        function obtenerCookiePlano() {
            var plano = readCookie("pruebaMapaWeb");

            if (typeof (plano) == "undefined" || plano == null)
                document.getElementById('txtCodPlano').value = "";
            else {
                document.getElementById('txtCodPlano').value = plano;
                eraseCookie("pruebaMapaWeb");
            }
        }

        var callbacks = {

            ValidarVendedorDNI: function (objResponse) {
                var msg = objResponse.Cadena;

                if (objResponse.Boleano == false) {
                    alert("Error en el proceso, favor intentar nuevamente.");
                    return;
                }

                if (msg.split("|")[0] == "ERROR")
                    alert(msg.split("|")[1]);
                else {
                    if (msg == '0') {
                        alert('El vendedor no es válido');
                    }
                    else {
                        setEnabled('txtVendedorDNI', false, 'clsInputDisable');
                        setValue('hidAceptarSinValidar', 'S')
                        document.getElementById('btnAceptar').click();
                    }
                }
            }
        }
        //fin gaa20131003  

        //PROY-140690
        function TipoProductoIfi(MensajeTipoProductoIfi) {
            document.getElementById('lbDirectInstalacion').innerHTML = 'Dirección de uso del servicio IFI';
            document.getElementById('Table3').style.display = 'none';
            document.getElementById('txtRefenciaSecundaria').style.display = 'none';
            document.getElementById('lblRefenciaSecundaria').style.display = 'none';
            document.title = MensajeTipoProductoIfi;
        }      

		//INICIO INICIATIVA-932
        function mostrarMapaCobertura() {

            setValue('hidCoordenadas', '');

            var resultadoMapa;
            var urlMapa = '../Map/sisact_mapa_cobertura.aspx';
            resultadoMapa = window.showModalDialog(urlMapa, '', 'dialogHeight:620px; dialogWidth:800px');

            //resultadoMapa = 'NICOLAS ARRIOLA,LA VICTORIA, LIMA, LIMA|Av|NICOLAS ARRIOLA|314|SANTA CATALINA'
            if (resultadoMapa == undefined || resultadoMapa.indexOf('|') < 0)
                return false;

            var strFlagCoordenadas = resultadoMapa.split('|')[0];
            var strCoordenadas = resultadoMapa.split('|')[6];
            setValue('hidCoordenadas', strCoordenadas);

            if (strFlagCoordenadas == '1') {

                setValue('txtDireccion', '');
                setValue('txtNroPuerta', '');
                setValue('txtUrbanizacion', '');

                setEnabled('txtDireccion', true, 'clsInputEnable');
                setEnabled('txtNroPuerta', true, 'clsInputEnable');
                setEnabled('txtUrbanizacion', true, 'clsInputEnable');

                setEnabled('ddlDepartamento', true, 'clsSelectEnable0');
                setEnabled('ddlProvincia', true, 'clsSelectEnable0');
                setEnabled('ddlDistrito', true, 'clsSelectEnable0');
                setEnabled('ddlPrefijo', true, 'clsSelectEnable0');
                setEnabled('divSinNumero', true, '');

                document.getElementById('ddlDepartamento').selectedIndex = 0;
                document.getElementById('ddlProvincia').selectedIndex = 0;
                document.getElementById('ddlDistrito').selectedIndex = 0;
            }
            else {
                var strDireccion = resultadoMapa.split('|')[1];
                var strTipoVia = resultadoMapa.split('|')[2].trim();
                var strNombreVia = resultadoMapa.split('|')[3];
                var strNumeroVia = resultadoMapa.split('|')[4];
                var strUrbanizacion = resultadoMapa.split('|')[5];

//            var strDistritoMap = strDireccion.split(',')[1].trim();
//            var strProvinciaMap = strDireccion.split(',')[2].trim();
//            var strDepartamentoMap = strDireccion.split(',')[3].trim();
            //-----------------INICIATIVA 992 INICIO---------------------
                var strDistritoMap = "";
                var strProvinciaMap = "";
                var strDepartamentoMap = "";
                var arrdireccion = strDireccion.split(',');

                if (arrdireccion.length == '5') {
                    strDistritoMap = arrdireccion[1].trim();
                    strProvinciaMap = arrdireccion[2].trim();
                    strDepartamentoMap = arrdireccion[3].trim();

                } else {
                    strProvinciaMap = arrdireccion[1].trim();
                    strDepartamentoMap = arrdireccion[2].trim();

                    setEnabled('ddlDepartamento', true, 'clsSelectDisable');
                    setEnabled('ddlProvincia', true, 'clsSelectDisable');
                    setEnabled('ddlDistrito', true, 'clsSelectDisable');
                }

                //-------------------INICIATIVA 992 FIN------------------

            seleccionarDepartamento(strDepartamentoMap.toUpperCase());
            var strDepart = document.getElementById('ddlDepartamento');
            mostrarProvincia(strDepart);
            seleccionarProvincia(strProvinciaMap.toUpperCase());
            var strProv = document.getElementById('ddlProvincia');
            mostrarDistrito(strProv);
            seleccionarDistrito(strDistritoMap.toUpperCase());

            var distritoCobertura = document.getElementById('ddlDistrito');
            mostrarCodigoPostal(distritoCobertura);
            mostrarUbigeo(distritoCobertura);
            mostrarUbigeoINEI(distritoCobertura);
            ListarCentroPoblado();
            document.getElementById('hidDistrito').value = document.getElementById('ddlDistrito').value;
            var flagVia = false;

            if (strTipoVia != '') {

                if (strTipoVia == 'Pj') {
                    strTipoVia = 'PS';
                } else if (strTipoVia == 'Ps') {
                    strTipoVia = 'Pa';
                }
                strTipoVia = strTipoVia.toUpperCase();

                var posicion = 0;

                $("#ddlPrefijo option").each(function () {

                    if (strTipoVia == $(this).attr('value')) {
                        document.getElementById('ddlPrefijo').selectedIndex = posicion
                        flagVia = true;
                       

                    }
                    posicion++;
                });
            }

            if (flagVia) {
                setEnabled('ddlPrefijo', false, 'clsSelectDisable');
            }
            else {
                setEnabled('ddlPrefijo', true, 'clsSelectEnable0');
            }
            onchange_prefijo();

            setValue('txtDireccion', strNombreVia.toUpperCase());
            setValue('txtNroPuerta', strNumeroVia.toUpperCase());
            setValue('txtUrbanizacion', strUrbanizacion.toUpperCase());

            if (getValue('txtDireccion') != '') {
                setEnabled('txtDireccion', false, 'clsInputDisable');
            } else {
                setEnabled('txtDireccion', true, 'clsInputEnable');
            }

            if (getValue('txtNroPuerta') != '') {
                setEnabled('txtNroPuerta', false, 'clsInputDisable');
            } else {
                setEnabled('txtNroPuerta', true, 'clsInputEnable');
            }

            if (getValue('txtUrbanizacion') != '') {
                setEnabled('txtUrbanizacion', false, 'clsInputDisable');
            } else {
                setEnabled('txtUrbanizacion', true, 'clsInputEnable');
            }


               

            }
           
            setEnabled('ddlTipoInterior', true, 'clsSelectEnable0');
            setEnabled('ddlUrbanizacion', true, 'clsSelectEnable0');
            setEnabled('ddlTipoDomicilio', true, 'clsSelectEnable0');
            setEnabled('ddlZona', true, 'clsSelectEnable0');
            setEnabled('txtReferencia', true, 'clsInputEnable');

            setEnabled('txtPrefijoTelefonoReferencia', true, 'clsInputEnable');
            setEnabled('txtTelefonoReferencia', true, 'clsInputEnable');

            document.getElementById('btnAceptar').disabled = false;
            document.getElementById('btnCancelar').disabled = false;

            document.getElementById('ddlEdificacion').selectedIndex = 0;
            document.getElementById('ddlTipoInterior').selectedIndex = 0;
            document.getElementById('ddlUrbanizacion').selectedIndex = 0;
            document.getElementById('ddlTipoDomicilio').selectedIndex = 0;
            document.getElementById('ddlZona').selectedIndex = 0;

            setValue('txtManzana', '');
            setValue('txtLote', '');
            setValue('txtNroInterior', '');
            setValue('txtNombreZona', '');
            setValue('txtReferencia', '');
            setValue('txtPrefijoTelefonoReferencia', '');
            setValue('txtTelefonoReferencia', '');

        }
        //INICIATIVA 992 INICIO
        var reemplazarAcentos=function(cadena)
            {
	            var chars={
		            "á":"a", "é":"e", "í":"i", "ó":"o", "ú":"u",
		            "à":"a", "è":"e", "ì":"i", "ò":"o", "ù":"u",
		            "Á":"A", "É":"E", "Í":"I", "Ó":"O", "Ú":"U",
		            "À":"A", "È":"E", "Ì":"I", "Ò":"O", "Ù":"U"}
	            var expr=/[áàéèíìóòúù]/ig;
	            var res=cadena.replace(expr,function(e){return chars[e]});
	            return res;
            }
        //INICIATIVA 992 FIN
        function seleccionarDepartamento(descripcionDpto) {

            var selDpto = document.getElementById('ddlDepartamento');

            for (var i = 0; i < selDpto.length; i++) {
                var opt = selDpto[i];
                var indice = opt.index;
                var descripcion = opt.innerText;

                var departamentoDescripcion = reemplazarAcentos(descripcionDpto); //INICIATIVA 992

                if (descripcion == departamentoDescripcion) {

                    document.getElementById('ddlDepartamento').selectedIndex = indice;
                    setEnabled('ddlDepartamento', false, 'clsSelectDisable'); //INICIATIVA 992
                    break;
                }
                else {
                    setEnabled('ddlDepartamento', true, 'clsSelectDisable'); //INICIATIVA 992
                }
            }

        }

        function seleccionarProvincia(descripcionProv) {

            var selProv = document.getElementById('ddlProvincia');

            for (var i = 0; i < selProv.length; i++) {
                var opt = selProv[i];
                var indice = opt.index;
                var descripcion = opt.innerText;

                var provinciaDescripcion = reemplazarAcentos(descripcionProv); //INICIATIVA 992

                if (descripcion == provinciaDescripcion) {

                    document.getElementById('ddlProvincia').selectedIndex = indice;
                    setEnabled('ddlProvincia', false, 'clsSelectDisable'); //INICIATIVA 992
                    break;
                } else {
                    setEnabled('ddlProvincia', true, 'clsSelectDisable'); //INICIATIVA 992
                }
            }
        }

        function seleccionarDistrito(descripcionDist) {

            var selDist = document.getElementById('ddlDistrito');

            for (var i = 0; i < selDist.length; i++) {
                var opt = selDist[i];
                var indice = opt.index;
                var descripcion = opt.innerText;

                var distritoDescripcion = reemplazarAcentos(descripcionDist); //INICIATIVA 992

                if (descripcion == distritoDescripcion) {

                    document.getElementById('ddlDistrito').selectedIndex = indice;
                    setEnabled('ddlDistrito', false, 'clsSelectDisable'); //INICIATIVA 992
                    break;
                } else {
                    setEnabled('ddlDistrito', true, 'clsSelectDisable'); //INICIATIVA 992
                }
            }
        }

        function validarDireccion(chk) {
            if (chk.checked) {

                document.getElementById('btnAceptar').disabled = false;
                document.getElementById('btnCancelar').disabled = false;

            }
            else {
                document.getElementById('btnAceptar').disabled = true;
                document.getElementById('btnCancelar').disabled = true;
            }
        }
		//FIN INICIATIVA-932

    </script>
    <style type="text/css">
        .style1
        {
            font-size: 11px;
            color: navy;
            font-family: Arial;
            text-decoration: none;
            font-weight: bold;
            width: 153px;
            height: 29px;
        }
        .style2
        {
            font-size: 11px;
            color: navy;
            font-family: Arial;
            text-decoration: none;
            font-weight: bold;
            height: 29px;
        }
        </style>
</head>
<body onload="inicio();">
    <form id="frmUscDireccionHfc" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="True" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" />
    <table style="border-right: #95b7f3 1px solid; border-top: #95b7f3 1px solid; border-left: #95b7f3 1px solid;
        border-bottom: #95b7f3 1px solid" cellspacing="1" cellpadding="1" width="100%"
        align="center">
        <tr height="20">
            <td class="TablaTitulos" style="font-size: 11px" align="left">
				<span id="lbDirectInstalacion">Dirección de Instalación</span>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbltitulodireccion" runat="server" CssClass="Arial10B"></asp:Label>
            </td>
        </tr>
        <tr>
            <td id="tdPanel_3">
                <table id="Table2" style="table-layout: fixed" cellspacing="0" cellpadding="1" width="810"
                    border="0">
                    <colgroup>
                        <col width="135">
                        <col width="130">
                        <col width="60">
                        <col width="10">
                        <col width="45">
                        <col width="120">
                        <col width="60">
                        <col width="60">
                        <col width="135">
                        <col width="60">
                        <col width="70">
                    </colgroup>
                    <tr id="trMapa" style="display: none">
                        <td align="left" height="30" colspan="2">
                            <input class="Boton" id="btnMapaCobertura" style="width: 185px; cursor: hand" onclick="mostrarMapaCobertura();"
                                type="button" value="Consultar Dirección" />
                        </td>
                        <td id="tdMantenerDireccion" align="left" height="30" colspan="9" style="display: none">
                            <input id="chkMantenerDireccion" type="checkbox" onclick="validarDireccion(this);" />
                            <asp:Label ID="Label4" runat="server" CssClass="Arial10B">¿Desea mantener su dirección?</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label29" runat="server" CssClass="Arial10B">&nbsp;Av/Calle/Jr</asp:Label>
                        </td>
                        <td style="width: 142px" nowrap align="left">
                            <asp:Label ID="Label26" runat="server" CssClass="Arial10B">Nombre de la Via (Av/Calle/Jr)</asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label19" runat="server" CssClass="Arial10B">Número</asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                            <label style="font-weight: bold; font-size: 8pt; color: red; font-family: Arial">
                            </label>
                        </td>
                        <td style="width: 79px" align="center">
                            <asp:Label ID="Label47" runat="server" CssClass="Arial10B">Mz/Bloq/Edif</asp:Label>
                        </td>
                        <td nowrap align="center">
                            <asp:Label ID="Label20" runat="server" CssClass="Arial10B">Nro Mz/Bq</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label21" runat="server" CssClass="Arial10B">Lote</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label23" runat="server" CssClass="Arial10B">Tipo Dpto/int</asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label25" runat="server" CssClass="Arial10B">Número</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label50" runat="server" CssClass="Arial10B" Visible="True">Contador</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlPrefijo" runat="server" CssClass="clsSelectEnable0" onChange="onchange_prefijo(this);"
                                Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox onpaste="return false;" ID="txtDireccion" onkeydown="onkeyup_direccion(this,'ddlPrefijo','D')"
                                onblur="onkeyup_direccion(this,'ddlPrefijo','D')" onkeyup="onkeyup_direccion(this,'ddlPrefijo','D')"
                                ondrop="return false;" runat="server" CssClass="clsInputDisable" Width="110px"
                                ReadOnly="True" MaxLength="25"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox onkeypress="eventoSoloNumerosEnteros();" onpaste="return false;" ID="txtNroPuerta"
                                onblur="onkeyup_NroPuerta(this)" onkeyup="onkeyup_NroPuerta(this);" ondrop="return false;"
                                runat="server" CssClass="clsInputDisable" Width="52px" ReadOnly="True" MaxLength="5"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            <div id="divSinNumero" disabled runat="server">
                                <asp:CheckBox ID="chkSinNumero" onclick="sinNumero(this)" runat="server" CssClass="Arial10B"
                                    Text="S/N" TextAlign="Left"></asp:CheckBox></div>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlEdificacion" disabled runat="server" CssClass="clsSelectDisable"
                                Width="110px" onchange="onchange_edificacion(this)">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox onpaste="return false;" ID="txtManzana" onkeydown="onkeyup_mz_lote(this,'D')"
                                onblur="onkeyup_mz_lote(this,'D')" onkeyup="onkeyup_mz_lote(this,'D')" ondrop="return false;"
                                runat="server" CssClass="clsInputDisable" Width="52px" ReadOnly="True" MaxLength="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox onpaste="return false;" ID="txtLote" onblur="onkeyup_mz_lote(this,'D')"
                                onkeyup="onkeyup_mz_lote(this,'D')" ondrop="return false;" runat="server" CssClass="clsInputDisable"
                                Width="52px" ReadOnly="True" MaxLength="5"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTipoInterior" runat="server" CssClass="clsSelectEnable0"
                                Width="120px" onchange="onchange_interior(this);">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox onpaste="return false;" ID="txtNroInterior" onblur="onkeyup_mz_lote(this,'D')"
                                onkeyup="onkeyup_mz_lote(this,'D')" ondrop="return false;" runat="server" CssClass="clsInputDisable"
                                Width="52px" ReadOnly="True" MaxLength="5" onkeypress="eventoSoloNumerosEnteros();"></asp:TextBox>
                        </td>
                        <td align="center">
                            <asp:Label ID="lblContadorDireccion" runat="server" CssClass="Arial10B" Visible="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="Label45" runat="server" CssClass="Arial10B" DESIGNTIMEDRAGDROP="72">&nbsp;Tipo y Nombre Urbanización</asp:Label>
                        </td>
                        <td nowrap colspan="3">
                            <asp:Label ID="lblTipoDomicilio" runat="server" CssClass="Arial10B">Tipo Domicilio</asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label48" runat="server" CssClass="Arial10B">Zona/Etapa</asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="Label49" runat="server" CssClass="Arial10B">Nombre Zona/Etapa</asp:Label>
                        </td>
                        <td align="center">
                            <asp:Label ID="Label24" runat="server" CssClass="Arial10B">&nbsp;Referencia Principal</asp:Label>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 153px">
                            <asp:DropDownList ID="ddlUrbanizacion" runat="server" CssClass="clsSelectEnable0"
                                Width="120px" onchange="onchange_urbanizacion(this);" DESIGNTIMEDRAGDROP="86">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 142px">
                            <asp:TextBox onpaste="return false;" ID="txtUrbanizacion" onblur="onkeyup_mz_lote(this,'R')"
                                onkeyup="onkeyup_mz_lote(this,'R')" ondrop="return false;" runat="server" CssClass="clsInputDisable"
                                Width="110px" ReadOnly="True" MaxLength="40"></asp:TextBox>
                        </td>
                        <td nowrap colspan="3">
                            <asp:DropDownList ID="ddlTipoDomicilio" runat="server" CssClass="clsSelectEnable0"
                                Width="105px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 79px" align="center">
                            <asp:DropDownList ID="ddlZona" runat="server" CssClass="clsSelectEnable0" Width="110px"
                                onchange="onchange_zona(this)">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:TextBox onpaste="return false;" ID="txtNombreZona" onblur="onkeyup_mz_lote(this,'R')"
                                onkeyup="onkeyup_mz_lote(this,'R')" ondrop="return false;" runat="server" CssClass="clsInputDisable"
                                Width="110px" ReadOnly="True" MaxLength="40"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox onpaste="return false;" ID="txtReferencia" onblur="onkeyup_mz_lote(this,'R')"
                                onkeyup="onkeyup_mz_lote(this,'R')" ondrop="return false;" runat="server" CssClass="clsInputEnable"
                                Width="128px" MaxLength="340"></asp:TextBox>
                        </td>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="lblContadorReferencia" runat="server" CssClass="Arial10B" Visible="True"
                                ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 153px">
                        </td>
                        <td style="width: 142px">
                        </td>
                        <td nowrap>
                        </td>
                        <td style="width: 32px">
                        </td>
                        <td>
                        </td>
                        <td style="width: 79px" align="center">
                        </td>
                        <td colspan="5">
                        </td>
                    </tr>
                    <tr>
                        <td class="Arial10B" style="width: 153px">
                            Departamento
                        </td>
                        <td style="width: 142px">
                            <strong><font face="Arial10B" color="#000080" size="2">
                                <asp:Label ID="Label1" runat="server" CssClass="Arial10B">Provincia</asp:Label></font></strong>
                        </td>
                        <td nowrap>
                            <strong><font face="Arial" color="#000080" size="2"></font></strong>
                        </td>
                        <td style="width: 32px">
                        </td>
                        <td>
                        </td>
                        <td style="width: 79px" align="center">
                            <strong><font face="Arial10B" color="#000080" size="2">
                                <asp:Label ID="Label2" runat="server" CssClass="Arial10B">Distrito</asp:Label></font></strong>
                        </td>
                        <td colspan="2">
                            <strong><font face="Arial10B" color="#000080" size="2">
                                <asp:Label ID="Label3" runat="server" CssClass="Arial10B">Cod. Postal</asp:Label></font></strong>
                        </td>
                        <td>
                            <asp:Label ID="lblUbigeo" runat="server" CssClass="Arial10B">Ubigeo</asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblTelfRef" runat="server" CssClass="Arial10B" DESIGNTIMEDRAGDROP="189">Teléfono Referencia</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 153px">
                            <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="clsSelectEnable0"
                                Width="120px" onchange="mostrarProvincia(this);">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 142px">
                            <asp:DropDownList ID="ddlProvincia" runat="server" CssClass="clsSelectEnable0" Width="120px"
                                onchange="mostrarDistrito(this);">
                            </asp:DropDownList>
                        </td>
                        <td nowrap>
                        </td>
                        <td style="width: 32px">
                        </td>
                        <td>
                        </td>
                        <td style="width: 79px" align="center">
                            <asp:DropDownList ID="ddlDistrito" runat="server" CssClass="clsSelectEnable0" Width="110px"
                                onchange="mostrarCodigoPostal(this);mostrarUbigeo(this);mostrarUbigeoINEI(this);ListarCentroPoblado();document.getElementById('hidDistrito').value=document.getElementById('ddlDistrito').value;">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <asp:TextBox onpaste="return false;" ID="txtCodigoPostal" ondrop="return false;"
                                runat="server" CssClass="clsInputDisable" Width="110px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUbigeo" runat="server" CssClass="clsInputDisable" Width="110px"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <table id="Table1" border="0">
                                <tr>
                                    <td>
                                        <asp:TextBox onkeypress="eventoSoloNumerosEnteros();" onpaste="return false;" ID="txtPrefijoTelefonoReferencia"
                                            ondrop="return false;" runat="server" CssClass="clsInputEnable" Width="25px"
                                            MaxLength="3"></asp:TextBox>&nbsp;-
                                    </td>
                                    <td>
                                        <asp:TextBox onkeypress="eventoSoloNumerosEnteros();" onpaste="return false;" ID="txtTelefonoReferencia"
                                            ondrop="return false;" runat="server" CssClass="clsInputEnable" Width="80px"
                                            MaxLength="9"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td id="tdLblTec" class="style1" style= "visibility:hidden" >
                            <asp:Label ID="lblTecnologia" runat="server" Text="Tecnología"></asp:Label>
                        </td>
                        <td id="tdChkTec" class="style2" colspan="10" style= "visibility:hidden" >
                            <asp:CheckBox ID="chkDTH" runat="server" Text="DTH" Width="100px" onclick="ListarCentroPoblado();"/>
                            <asp:CheckBox ID="chkLTE" runat="server" Text="LTE" Width="100px" onclick="ListarCentroPoblado();"/>
                        </td>
                    </tr>
                    <tr id="lblRefenciaSecundaria">
                        <td class="Arial10B" colspan="11">
                            Referencia Secundaria
                        </td>
                    </tr>
                    <tr id="txtRefenciaSecundaria">
                        <td class="Arial10B" colspan="11">
                            <asp:TextBox onpaste="return false;" ID="txtRefSecundaria" onkeydown="onkeyup_ReferenciaSec()"
                                onblur="onkeyup_ReferenciaSec()" onkeyup="onkeyUp_ReferenciaSec()" ondrop="return false;"
                                runat="server" CssClass="clsInputEnable" Width="860px" MaxLength="250"></asp:TextBox>&nbsp;<asp:Label
                                    ID="lblContRefSecundaria" runat="server" CssClass="Arial10B" Visible="True" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <input id="hidProvincias" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidProvincias" runat="server" />
                <input id="hidDistritos" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidDistritos" runat="server" />
                <input id="hidListaCodigoPostal" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidListaCodigoPostal" runat="server" />
                <input id="hidDptoDefault" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidDptoDefault" runat="server" />
                <input id="hidProvinciaDefault" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidProvinciaDefault" runat="server" />
                <input id="hidDistritoDefault" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidDistritoDefault" runat="server" />
                <input id="hidDptoId" style="width: 8px; height: 22px" type="hidden" size="1" name="hidDptoId"
                    runat="server" />
                <input id="hidDistritoId" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidDistritoId" runat="server" />
                <input id="hidProvinciaId" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidProvinciaId" runat="server" />
                <input id="hidSinNumero" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidSinNumero" runat="server" />
                <input id="hidShowRUC" style="width: 8px; height: 22px" type="hidden" size="1" name="hidShowRUC"
                    runat="server" />
                <input id="hidListUbigeo" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidListUbigeo" runat="server" />
                <input id="hidListUbigeoINEI" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidListUbigeoINEI" runat="server" />
                <input id="hidUbigeoINEI" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidUbigeoINEI" runat="server" />
                <input id="hidCentroPoblado" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidCentroPoblado" runat="server" />
                <input id="hidProvincia" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidProvincia" runat="server" />
                <input id="hidDistrito" style="width: 8px; height: 22px" type="hidden" size="1" name="hidDistrito"
                    runat="server" />
                <input id="hidTipoProducto" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidTipoProducto" runat="server" />
                <input id="hidFlgReadOnly" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidFlgReadOnly" runat="server" />
                <input id="hidIdFila" style="width: 8px; height: 22px" type="hidden" size="1" name="hidIdFila"
                    runat="server" />
                <input id="hidVentaProactiva" style="width: 8px; height: 22px" type="hidden" size="1"
                    name="hidVentaProactiva" runat="server" />
                <asp:Label ID="Label10" runat="server" CssClass="Arial10B" Visible="False">Ej.01-6131000</asp:Label>
            </td>
        </tr>
    </table>
    <table style="border-right: #95b7f3 1px solid; border-top: #95b7f3 1px solid; border-left: #95b7f3 1px solid;
        border-bottom: #95b7f3 1px solid" cellspacing="1" cellpadding="1" width="100%"
        align="center">
        <tr>
            <td>
                <table id="Table3" style="table-layout: fixed" cellspacing="0" cellpadding="0" width="810"
                    border="0">
                    <!--colgroup>
								<col width="135">
								<col width="130">
								<col width="60">
								<col width="10">
								<col width="45">
								<col width="120">
								<col>
								<col>
								<col>
								<col>
								<col>
							</colgroup-->
                    <tbody>
                        <!--TR>
									<TD style="WIDTH: 153px"></TD>
									<TD style="WIDTH: 142px"></TD>
									<TD style="WIDTH: 70px" noWrap></TD>
									<TD style="WIDTH: 18px"></TD>
									<TD style="WIDTH: 49px"></TD>
									<TD style="WIDTH: 79px" align="center"></TD>
									<TD style="WIDTH: 385px" colSpan="5"></TD>
								</TR-->
                        <tr>
                            <td id="tdTxtCentroPoblado" class="Arial10B" style="width: 135px; display: none">
                                Centro Poblado
                            </td>
                            <td id="tdDdlCentroPoblado" style="width: 135px; display: none">
                                <asp:DropDownList ID="ddlCentroPoblado" runat="server" CssClass="clsSelectEnable0"
                                    Width="120px" onchange="limpiarGrilla();document.getElementById('hidCentroPoblado').value=document.getElementById('ddlCentroPoblado').value;">
                                </asp:DropDownList>
                            </td>
                            <!--TD class="Arial10B" style="WIDTH: 70px" align="right"></TD>
									<TD style="WIDTH: 18px" align="center"></TD-->
                            <!--TD style="WIDTH: 49px"></TD-->
                            <td class="Arial10B" id="IdCodPlano" style="width: 505px" valign="top" colspan="6"
                                runat="Server">
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td class="Arial10B" valign="top" nowrap>
                                            Plano&nbsp;&nbsp;
                                            <asp:TextBox ID="txtCodPlano" onblur="onkeyup_mz_lote(this,'R')" onkeyup="onkeyup_mz_lote(this,'R')"
                                                runat="server" CssClass="clsInputDisable" Width="70px" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td valign="top">
                                            &nbsp;
                                            <input class="Boton" id="btnCargarGrilla" onclick="MostrarGrillaCentropoblado();"
                                                type="button" size="20" value="Validar Plano" name="btnCargarGrilla" width="80">&nbsp;
                                        </td>
                                        <td class="Arial10B" valign="top" nowrap>
                                            Edificio Liberado&nbsp;
                                        </td>
                                        <td valign="top">
                                            <asp:TextBox onkeypress="return f_EventoSoloAlfanumericos();" ID="txtCodEdificio"
                                                onblur="MostrarGrillaEdificioA()" runat="server" CssClass="clsInputEnable" Width="50px"
                                                MaxLength="50"></asp:TextBox>
                                        </td>
                                        <td valign="top">
                                            &nbsp;
                                            <input class="Boton" id="btnCargarGrillaEdificio" style="width: 90px; height: 17px"
                                                onclick="MostrarGrillaEdificio();" type="button" size="20" value="Validar Edificio"
                                                name="btnCargarGrillaEdificio" width="90">&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdVentaProactiva" style="width: 330px; display: none" colspan="12">
                                <table cellspacing="0" cellpadding="0" border="0">
                                    <tr>
                                        <td class="Arial10B" nowrap>
                                            <div id="divChkVentaProactiva" style="display: none" align="left">
                                                &nbsp;Venta Puerta Puerta:&nbsp;&nbsp;<input id="chkVentaProactiva" onclick="mostrarVendedorDNI(this);"
                                                    type="checkbox" name="chkVentaProactiva" runat="server">
                                            </div>
                                        </td>
                                        <td class="Arial10B" nowrap>
                                            <div id="divVendedorDNI" style="display: none">
                                                DNI Vendedor
                                                <asp:TextBox onkeypress="eventoSoloNumerosEnteros();" ID="txtVendedorDNI" CssClass="clsInputEnable"
                                                    Width="100px" MaxLength="8" runat="server">
                                                </asp:TextBox>
                                                &nbsp;<label id="lblVtaProgramada">Venta Programada</label>
                                                <input id="chkVtaProgramada" type="checkbox" name="chkVtaProgramada" runat="server">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <asp:Label ID="lblMsj" Visible="False" Width="72px" ForeColor="Red" runat="server"
                    Font-Size="7.5pt" Font-Bold="True">* Obligatorio</asp:Label>
            </td>
        </tr>
        <tr align="center">
            <td align="center">
                <iframe id="ifrGrillaCentroPoblado" name="ifrGrillaCentroPoblado" frameborder="0"
                    width="0" scrolling="no" height="0" runat="server"></iframe>
            </td>
        </tr>
    </table>
    <table align="center">
        <tr>
            <td>
                <asp:Button ID="btnAceptar" onmouseover="this.className='BotonResaltado';" onmouseout="this.className='Boton';"
                    runat="server" CssClass="Boton" Text="Aceptar" OnClick="btnAceptar_Click"></asp:Button>&nbsp;&nbsp;
                <input class="Boton" id="btnCancelar" onmouseover="this.className='BotonResaltado';"
                    onclick="window.close();" onmouseout="this.className='Boton';" type="button"
                    value="Cancelar" name="btnCancelar" runat="server">
            </td>
        </tr>
    </table>
    <input id="hidRetornar" type="hidden" name="hidRetornar" runat="server" />
    <input id="hidAceptarSinValidar" type="hidden" value="N" name="hidAceptarSinValidar" />
    <input id="hidFlagVOD" type="hidden" value="0" name="hidFlagVOD" runat="server" />
    <input id="hidNroDocumento" type="hidden" value="0" name="hidNroDocumento" runat="server" />
    <input id="hidNroSEC" type="hidden" value="0" name="hidNroSEC" runat="server" />
    <input id="hidEditarDirecion" type="hidden" name="hidEditarDirecion" runat="server" />
    <input id="hidNroSot" type="hidden" name="hidNroSot" runat="server" />
    <input id="hidTipoProductoActual" type="hidden" name="hidTipoProductoActual" runat="server" />
    <input id="hidResultadoMapa" type="hidden" name="hidResultadoMapa" runat="server" />
    <input id="hidDepartamentos" type="hidden" name="hidDepartamentos" runat="server" />
    <input id="hidTieneDireccion" type="hidden" name="hidTieneDireccion" runat="server" />
    <input id="hidCoordenadas" type="hidden" name="hidCoordenadas" runat="server" />

    <input id="hidFlagIFI" type="hidden" name="hidFlagIFI" runat="server" value="0"/>

    <iframe id="ifrConsultaSerie" name="ifrConsultaSerie" width="0" scrolling="no" height="0">
    </iframe>
    </form>
</body>
</html>
