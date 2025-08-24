function inicio() {
    if (getValue('hidnMensajeValue') != '')
        alert(getValue('hidnMensajeValue'));

    setValue('hidnMensajeValue', '');
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
    //gaa20161020
    if (getValue('hidMetodo') == 'LlenarFamiliaPlan')
        LlenarFamiliaPlan();
    //fin gaa20161020
    //Proy - 30748 Inicio
    if (getValue('hidMetodo') == 'LlenarPlanProactivo')
        FLlenarPlanProactivo();
    //Proy - 30748 Fin

    //INI PROY-29296
    if (getValue('hidMetodo') == 'LlenarMontosTopeConsumo')
        LlenarMontosTopeConsumo();

    if (getValue('hidMetodo') == 'LlenarMontosTopeConsumoLTE')
        LlenarMontosTopeConsumoLTE();
    //FIN PROY-29296

    //PROY-140743 - INI
    if (getValue('hidMetodo') == 'llenarPromociones')
        llenarPromociones();
    //PROY-140743 - FIN

    parent.quitarImagenEsperando();
    parent.autoSizeIframe();
}
//Proy - 30748 Inicio
function FLlenarPlanProactivo() {
    var strdestabla = getValue('hiddestable');
    var strdescodmodalidad = getValue('hidcodmodalidad');
    var strResultado = getValue('hidnResultadoValue');

    parent.FConsultaPlanes(strdescodmodalidad, strResultado, strdestabla);
}
//Proy - 30748 Fin
function LlenarPlanPaq() {
    var idFila = getValue('hidIdFila');
    var strPaqueteActual = parent.document.getElementById('hidPaqueteActual').value;
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarPlanMultiLinea(idFila, strResultado, strPaqueteActual);
}
function asignarPlan() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarPlan(idFila, strResultado);
}
function asignarEquipoPrecio() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarPrecio(idFila, strResultado);
}
function LlenarMontoTopeConsumo() {
    var strResultado = getValue('hidnResultadoValue');
    var idFila = getValue('hidIdFila');
    var txtMontoTopeConsumo = parent.document.getElementById('txtMontoTopeConsumo' + idFila);
    txtMontoTopeConsumo.value = strResultado;
}
function LlenarServCampCorpTot() {
    var strResultado = getValue('hidnResultadoValue');
    parent.retornarServCampCorpTot(strResultado);
}
function MostrarSecPendiente() {
    var strResultado = getValue('hidnResultadoValue');
    parent.mostrarSecPendiente(strResultado);
}
function LlenarPaquetePlan() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
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
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarPlan3Play(idFila, strResultado);
}
function LlenarPaquete3Play() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarPaquete3Play(idFila, strResultado);
}
function LlenarServicioHFC() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarServicioMultiLinea3Play(idFila, strResultado);
}
function LlenarEquipo3Play() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarEquipo3Play(idFila, strResultado);
}
function LlenarTopesConsumo() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.document.getElementById('hidTopesConsumo').value = strResultado;
    parent.asignarTopeConsumo(idFila);
}
function LlenarCampanaPlazo() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarCampanaPlazo(idFila, strResultado);
}
function LlenarServicioMaterial() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarServicioMaterial(idFila, strResultado);
}
function LlenarPlanesCombo() {
    var strResultado = getValue('hidnResultadoValue');
    parent.retornarPlanesCombo(strResultado);
}
function LlenarServicioKit() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarServicioKit(idFila, strResultado);
}
function LlenarMaterial() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.llenarMaterial(idFila, strResultado);
    parent.asignarMaterial(idFila);
}
function LlenarListaPrecio() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarListaPrecio(idFila, strResultado);
}
function LlenarListaPrecioPrecio() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarListaPrecioPrecio(idFila, strResultado);
}
//gaa20161020
function LlenarFamiliaPlan() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarFamiliaPlan(idFila, strResultado);
}
//fin gaa20161020
//INI PROY-29296
function LlenarMontosTopeConsumo() {
    var strResultado = getValue('hidnResultadoValue');
    parent.listarMontosXTopeConsumo(strResultado);
}

function LlenarMontosTopeConsumoLTE() {
    var strResultado = getValue('hidnResultadoValue');
    parent.listarMontosXTopeConsumoLTE(strResultado);
}

//PROY-140736 INI
function EliminarBuyBack(fila) {
    parent.EliminarBuyBack(fila);
    parent.asignarMaterial(fila);
    }

function EnviarDatosBuyback() {
    var strResultado = getValue('hdbuyback_frame');
    parent.AsignarDatosBuyback(strResultado);
}


//PROY-140736 FIN

//PROY-140743 - INI
function llenarPromociones() {
    var idFila = getValue('hidIdFila');
    var strResultado = getValue('hidnResultadoValue');
    parent.asignarPromociones(idFila, strResultado);
}
//PROY-140743 - FIN