<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sisact_ifr_consulta_unificada.aspx.cs"
    Inherits="Claro.SISACT.Web.frames.sisact_ifr_consulta_unificada" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>sisact_ifr_consulta_unificada</title>
    <script language='javascript' type='text/javascript' src='../../Scripts/funciones_sec.js'></script>
    <script language="JavaScript" type="text/javascript">
        function inicio() {
            if (getValue('hidMensaje') != '')
                alert(getValue('hidMensaje'));

            setValue('hidMensaje', '');
            if (getValue('hidMetodo') == 'LlenarPlan')
                asignarPlan();
            if (getValue('hidMetodo') == 'LlenarEquipoPrecio')
                asignarEquipoPrecio();
            if (getValue('hidMetodo') == 'LlenarMontoTopeConsumo')
                LlenarMontoTopeConsumo();
            if (getValue('hidMetodo') == 'LlenarServCampCorpTot')
                LlenarServCampCorpTot();
            if (getValue('hidMetodo') == 'MostrarSecPendiente')
                MostrarSecPendiente();
            if (getValue('hidMetodo') == 'LlenarPlanPaq')
                LlenarPlanPaq();
            if (getValue('hidMetodo') == 'LlenarPaquetePlan')
                LlenarPaquetePlan();
            if (getValue('hidMetodo') == 'LlenarPlanPaq3Play')
                LlenarPlanPaq3Play();
            if (getValue('hidMetodo') == 'LlenarPaquete3Play')
                LlenarPaquete3Play();
            if (getValue('hidMetodo') == 'LlenarServicioHFC')
                LlenarServicioHFC();
            if (getValue('hidMetodo') == 'LlenarEquipo3Play')
                LlenarEquipo3Play();
            if (getValue('hidMetodo') == 'LlenarCampanaPlazo')
                LlenarCampanaPlazo();
            if (getValue('hidMetodo') == 'LlenarServicioMaterial')
                LlenarServicioMaterial();
            if (getValue('hidMetodo') == 'LlenarPlanesCombo')
                LlenarPlanesCombo();
            if (getValue('hidMetodo') == 'LlenarServicioKit')
                LlenarServicioKit();
            if (getValue('hidMetodo') == 'LlenarTopesConsumo')
                LlenarTopesConsumo();
            if (getValue('hidMetodo') == 'LlenarMaterial')
                LlenarMaterial();
            if (getValue('hidMetodo') == 'LlenarListaPrecio')
                LlenarListaPrecio();
            if (getValue('hidMetodo') == 'LlenarListaPrecioPrecio')
                LlenarListaPrecioPrecio();

            parent.quitarImagenEsperando();
            parent.autoSizeIframe();
        }

        function LlenarPlanPaq() {
            var idFila = getValue('hidIdFila');
            var strPaqueteActual = parent.document.getElementById('hidPaqueteActual').value;
            var strResultado = getValue('hidResultado');
            parent.asignarPlanMultiLinea(idFila, strResultado, strPaqueteActual);
        }
        function asignarPlan() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarPlan(idFila, strResultado);
        }
        function asignarEquipoPrecio() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarPrecio(idFila, strResultado);
        }
        function LlenarMontoTopeConsumo() {
            var strResultado = getValue('hidResultado');
            var idFila = getValue('hidIdFila');
            var txtMontoTopeConsumo = parent.document.getElementById('txtMontoTopeConsumo' + idFila);
            txtMontoTopeConsumo.value = strResultado;
        }
        function LlenarServCampCorpTot() {
            var strResultado = getValue('hidResultado');
            parent.retornarServCampCorpTot(strResultado);
        }
        function MostrarSecPendiente() {
            var strResultado = getValue('hidResultado');
            parent.mostrarSecPendiente(strResultado);
        }
        function LlenarPaquetePlan() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            var arrListas = strResultado.split('¬');
            var ddlPaquete = parent.document.getElementById('ddlPaquete' + idFila);
            var strPaqueteActual = parent.document.getElementById('hidPaqueteActual').value;

            parent.llenarDatosCombo(ddlPaquete, arrListas[0], true);

            if (strPaqueteActual.length > 0)
                ddlPaquete.value = strPaqueteActual;

            parent.asignarPlan(idFila, arrListas[1]);
        }
        function LlenarPlanPaq3Play() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarPlan3Play(idFila, strResultado);
        }
        function LlenarPaquete3Play() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarPaquete3Play(idFila, strResultado);
        }
        function LlenarServicioHFC() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarServicioMultiLinea3Play(idFila, strResultado);
        }
        function LlenarEquipo3Play() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarEquipo3Play(idFila, strResultado);
        }
        function LlenarTopesConsumo() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.document.getElementById('hidTopesConsumo').value = strResultado;
            parent.asignarTopeConsumo(idFila);
        }
        function LlenarCampanaPlazo() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarCampanaPlazo(idFila, strResultado);
        }
        function LlenarServicioMaterial() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarServicioMaterial(idFila, strResultado);
        }
        function LlenarPlanesCombo() {
            var strResultado = getValue('hidResultado');
            parent.retornarPlanesCombo(strResultado);
        }
        function LlenarServicioKit() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarServicioKit(idFila, strResultado);
        }
        function LlenarMaterial() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.llenarMaterial(idFila, strResultado);
            parent.asignarMaterial(idFila);
        }
        function LlenarListaPrecio() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarListaPrecio(idFila, strResultado);
        }
        function LlenarListaPrecioPrecio() {
            var idFila = getValue('hidIdFila');
            var strResultado = getValue('hidResultado');
            parent.asignarListaPrecioPrecio(idFila, strResultado);            
        }

    </script>
</head>
<body onload="inicio()">
    <form id="Form1" method="post" runat="server">
    <input id="hidIdFila" type="hidden" name="hidIdFila" runat="server" />
    <input id="hidResultado" type="hidden" name="hidResultado" runat="server" />
    <input id="hidMetodo" type="hidden" name="hidMetodo" runat="server" />
    <input id="hidMensaje" type="hidden" name="hidMensaje" runat="server" />
    </form>
</body>
</html>
