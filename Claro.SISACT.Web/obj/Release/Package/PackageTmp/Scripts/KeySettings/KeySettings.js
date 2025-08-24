//#Region PROY-31636 IDEA-41419 | RENTESEG| Bryan Chumbes Lizarraga
var Key_codigoDocMigratorios = null;
var Key_codigoDocMigraYPasaporte = null;
var Key_codDocMigra_Pasaporte_CE = null;
var Key_codigoDocCIRE = null;
var Key_codigoDocCIE = null;
var Key_codigoDocCPP = null;
var Key_codigoDocCTM = null;
var Key_minLengthDocMigratorios = null;
var Key_maxLengthDocMigratorios = null;
var Key_codigoDocPasaporte07 = null;
var Key_flagPermitirProductosLTE = null;
var Key_docsExistEvaluacionPost = null;
var Key_tipoDocPermitidoPostCAC = null;
var Key_tipoDocPermitidoPostDAC = null;
var Key_tipoDocPermitidoPostCAD = null;

//INC000002396378
var Key_EstadoSecRechazado = null;
//INC000002396378

//INC000003443673
var Key_ValorSinApellidoPaternoOMaterno = null;
//INC000003443673

//INI PROY-CAMPANA LG
var keyComboLG_CampanasAutorizadas = null;
var keyComboLG_CampanasReglas = null;
var keyComboLG_CampanaMensaje_ClienteConExistencia = null;
var keyComboLG_CampanaMensaje_ClienteNoAplica = null;
//FIN PROY-CAMPANA LG

//PROY-FULLCLARO.V2-INI
var  key_msjBeneficioFija = null;
var key_msjBeneficioMovil = null;
//PROY-FULLCLARO.V2-FIN

//PROY-140457-DEBITO AUTOMATICO-INI
var Key_flagDebitoAuto = null;
//PROY-140457-DEBITO AUTOMATICO-FIN

//INC000002977281-INI
var Key_codFlagBFClaroFijaApagado = null;
//INC000002977281-FIN

var Key_ReintentosRegistroRRLL = null; //INC000003013199

/*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
var Key_FlagGeneralOfertaDefault = null;
var Key_CanalPermitidoOfertaDefault = null;
var Key_OperacionPermitidaOfertaDefault = null;
var Key_DocumentosPermitidosOfertaDefault = null;
var Key_CodigoOfertaDefault = null;
var Key_IsPortabilidadOfertaDefault = null;
/*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
var key_CanalPermMsjeBRMSCamp = null; //PROY-140585 F2
var key_OperaPermMsjeBRMSCamp = null; //PROY-140585 F2
var key_MsjAplicaCuotas = null; //PROY-140585 F2

//PROY-140618-INI
var Key_TipoOperacionPermi = null;
var Key_TipoProductoPermi = null;
//PROY-140618-FIN

//INICIATIVA - 733 - INI - C17
var Key_CodEquipoIPTV = null;
var Key_CodClaroVideoIPTV = null;
var Key_strMensajeClaroVideoIPTV = null;
var Key_CodGrupoServicio = null;
//INICIATIVA - 733 - FIN - C17

//INC000003430042 - INICIO
var Key_minLengthDocPass = null;
//INC000003430042 - FIN
//PROY-140657 INI
var key_TipoAutorizadoDocumentoDEAU = null;
var key_MsjCorreoObligatorioDEAU = null;
var key_MsjValidacionMontoDEAU = null;
var key_FlagAfiliacionDEAU = null;
var key_FlagConsultarAltaDEAU = null;
//PROY-140657 FIN

//INICIATIVA - 803- INI
var Key_FlagAprobPrecio = null;
var Key_MsjFactorSubsidio = null;
var Key_MsjIngresarIdPedido = null;
var Key_PorcentSubsidio = null;
var Key_MsjErrorExcepPrec = null;
var Key_FlagApagadoExcepcionPrecio = null;
var Key_FlagApagadoValidacionSubsidio = null;
//INICIATIVA - 803 - FIN

//PROY-140736 INI
var Key_CodCampaniaBuyBack = null;
var Key_Max_Length_Cupon = null;
var Key_Min_Length_IMEI = null;
var Key_Max_Length_IMEI = null;
var Key_Msj_Error_CBO_BuyBack = null;
var Key_Msj_Error_Cupon_BuyBack = null;
var Key_Msj_Error_Fila_BuyBack = null;
var Key_Msj_Error_Igual_Cupon = null;
var Key_Msj_Error_Igual_Imei = null;
var Key_Ingr_Cupon_Buyback = null;
var Key_Ingr_IMEI_Buyback =null;
var Key_Sel_Equipo_Buyback = null;

//PROY-140736 FIN

//INI PROY-140739 Formulario Leads
var KeyLeadsCanal = null;
var KeyLeadsTopenPostPago = null;
var KeyLeadsPDV = null;
var KeyLeadsFlag = null;
var KeyLeadsMensajeObligatorio = null;
var KeyLeadsMaxLength = null;
var KeyLeadsProductosPermitidosPostpago = null;
//FIN PROY-140739 Formulario Leads

var Key_CadValorFormaPago = null; //PROY-140546
var Key_CanalesPermitidosCAI = null; //PROY-140546
var Key_MensajeMaiMayor = null; //FALLAS PROY-140546

var Key_FlagGeneralCobertura = null; //INICIATIVA-932
var Key_MsgCoberturaIFI = null; //INICIATIVA-992

//PROY-140743 - IDEA-141192 - INI
var Key_FlagGeneralVtaCuotas = null;
var Key_TipoMatVentaVarias = null;
var Key_OpcionSinPromocion = null;
var Key_MsjAccNoCuotas = null;
var Key_MsjClienteNoAplicaCuotas = null;
var Key_MsjNoAplicaCuotas = null;
var Key_OpcionLineaCuenta = null;
var Key_MsjSelecAcc = null;
var Key_MsjSelecLineaCuenta = null;
var Key_MsjVntaCuotasBRMS = null;

//PROY-140743 - IDEA-141192 - FIN

$(document).ready(function () {
    PageMethods.getParametros(getParametros_CallBack);

    function getParametros_CallBack(response) {
        Key_codigoDocMigratorios = response.Key_codigoDocMigratorios;
        Key_codigoDocMigraYPasaporte = response.Key_codigoDocMigraYPasaporte;
        Key_codDocMigra_Pasaporte_CE = response.Key_codDocMigra_Pasaporte_CE;
        Key_codigoDocCIRE = response.Key_codigoDocCIRE;
        Key_codigoDocCIE = response.Key_codigoDocCIE;
        Key_codigoDocCPP = response.Key_codigoDocCPP;
        Key_codigoDocCTM = response.Key_codigoDocCTM;
        Key_minLengthDocMigratorios = response.Key_minLengthDocMigratorios;
        Key_maxLengthDocMigratorios = response.Key_maxLengthDocMigratorios;
        //INC000003430042 - INICIO
        Key_minLengthDocPass = response.Key_minLengthDocPass;
        //INC000003430042 - FIN
        Key_codigoDocPasaporte07 = response.Key_codigoDocPasaporte07;
        Key_flagPermitirProductosLTE = response.Key_flagPermitirProductosLTE;
        Key_docsExistEvaluacionPost = response.Key_docsExistEvaluacionPost;
        Key_tipoDocPermitidoPostCAC = response.Key_tipoDocPermitidoPostCAC;
        Key_tipoDocPermitidoPostDAC = response.Key_tipoDocPermitidoPostDAC;
        Key_tipoDocPermitidoPostCAD = response.Key_tipoDocPermitidoPostCAD;
        //INC000002396378
        Key_EstadoSecRechazado = response.Key_EstadoSecRechazado;
        //INC000002396378

        //INC000003443673
        Key_ValorSinApellidoPaternoOMaterno = response.Key_ValorSinApellidoPaternoOMaterno;
        //INC000003443673

        //INI PROY-CAMPANA LG
        keyComboLG_CampanasAutorizadas = response.keyComboLG_CampanasAutorizadas;
        keyComboLG_CampanasReglas = response.keyComboLG_CampanasReglas;
        keyComboLG_CampanaMensaje_ClienteConExistencia = response.keyComboLG_CampanaMensaje_ClienteConExistencia;
        keyComboLG_CampanaMensaje_ClienteNoAplica = response.keyComboLG_CampanaMensaje_ClienteNoAplica;
        //FIN PROY-CAMPANA LG

        //PROY-FULLCLARO.V2-INI
        key_msjBeneficioFija = response.key_msjBeneficioFija;
        key_msjBeneficioMovil = response.key_msjBeneficioMovil;
        //PROY-FULLCLARO.V2-FIN

        //PROY-140457-DEBITO AUTOMATICO-INI
        Key_flagDebitoAuto = response.Key_flagDebitoAuto;
        //PROY-140457-DEBITO AUTOMATICO-FIN

        //INC000002977281-INI
        Key_codFlagBFClaroFijaApagado = response.Key_codFlagBFClaroFijaApagado;
        //INC000002977281-FIN

        Key_ReintentosRegistroRRLL = response.Key_ReintentosRegistroRRLL; //INC000003013199

        /*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
        Key_FlagGeneralOfertaDefault = response.Key_FlagGeneralOfertaDefault;
        Key_CanalPermitidoOfertaDefault = response.Key_CanalPermitidoOfertaDefault;
        Key_OperacionPermitidaOfertaDefault = response.Key_OperacionPermitidaOfertaDefault;
        Key_DocumentosPermitidosOfertaDefault = response.Key_DocumentosPermitidosOfertaDefault;
        Key_CodigoOfertaDefault = response.Key_CodigoOfertaDefault;
        Key_IsPortabilidadOfertaDefault = response.Key_IsPortabilidadOfertaDefault;
        /*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta*/

        //PROY-140618-INI
        Key_TipoOperacionPermi = response.Key_TipoOperPermitida;
        Key_TipoProductoPermi = response.Key_TipoProdPermitido;
        //PROY-140618-FIN

        key_CanalPermMsjeBRMSCamp = response.key_CanalPermMsjeBRMSCamp; //PROY-140585 F2
        key_OperaPermMsjeBRMSCamp = response.key_OperaPermMsjeBRMSCamp; //PROY-140585 F2
        key_MsjAplicaCuotas = response.key_MsjAplicaCuotas; //PROY-140585

        //INICIATIVA - 733 - INI - C18
        Key_CodEquipoIPTV = response.Key_CodEquipoIPTV;
        Key_CodClaroVideoIPTV = response.Key_CodClaroVideoIPTV;
        Key_strMensajeClaroVideoIPTV = response.Key_strMensajeClaroVideoIPTV;
        Key_CodGrupoServicio = response.Key_CodGrupoServicio; 
        //INICIATIVA - 733 - FIN - C18
        //PROY-140657 INI
        key_TipoAutorizadoDocumentoDEAU = response.key_TipoAutorizadoDocumentoDEAU;
        key_MsjCorreoObligatorioDEAU = response.key_MsjCorreoObligatorioDEAU;
        key_MsjValidacionMontoDEAU = response.key_MsjValidacionMontoDEAU;
        key_FlagAfiliacionDEAU = response.key_FlagAfiliacionDEAU;
        key_FlagConsultarAltaDEAU = response.key_FlagConsultarAltaDEAU;
        //PROY-140657 FIN

        //INICIATIVA - 803 - INI

        Key_MsjFactorSubsidio = response.Key_MsjFactorSubsidio;
        Key_MsjIngresarIdPedido = response.Key_MsjIngresarIdPedido;
        Key_PorcentSubsidio = response.Key_PorcentSubsidio;
        Key_MsjErrorExcepPrec = response.Key_MsjErrorExcepPrec;
        Key_FlagAprobPrecio = response.Key_FlagAprobPrecio;
        Key_FlagApagadoExcepcionPrecio = response.Key_FlagApagadoExcepcionPrecio
        Key_FlagApagadoValidacionSubsidio = response.Key_FlagApagadoValidacionSubsidio
        //INICIATIVA - 803 - FIN

        //PROY-140736 INI
        Key_CodCampaniaBuyBack = response.Key_CodCampaniaBuyBack
        Key_Max_Length_Cupon = response.Key_Max_Length_Cupon
        Key_Min_Length_IMEI = response.Key_Min_Length_IMEI
        Key_Max_Length_IMEI = response.Key_Max_Length_IMEI
        Key_Msj_Error_CBO_BuyBack = response.Key_Msj_Error_CBO_BuyBack
        Key_Msj_Error_Cupon_BuyBack = response.Key_Msj_Error_Cupon_BuyBack
        Key_Msj_Error_Fila_BuyBack = response.Key_Msj_Error_Fila_BuyBack
        Key_Msj_Error_Igual_Cupon = response.Key_Msj_Error_Igual_Cupon;
        Key_Msj_Error_Igual_Imei = response.Key_Msj_Error_Igual_Imei;
        Key_Ingr_Cupon_Buyback = response.Key_Ingr_Cupon_Buyback;
        Key_Ingr_IMEI_Buyback = response.Key_Ingr_IMEI_Buyback;
        Key_Sel_Equipo_Buyback = response.Key_Sel_Equipo_Buyback;
        //PROY-140736 FIN

        //INI PROY-140739 Formulario Leads
        KeyLeadsCanal = response.KeyLeadsCanal;
        KeyLeadsTopenPostPago = response.KeyLeadsTopenPostPago;
        KeyLeadsPDV = response.KeyLeadsPDV;
        KeyLeadsFlag = response.KeyLeadsFlag;
        KeyLeadsMensajeObligatorio = response.KeyLeadsMensajeObligatorio;
        KeyLeadsMaxLength = response.KeyLeadsMaxLength;
        KeyLeadsProductosPermitidosPostpago = response.KeyLeadsProductosPermitidosPostpago;
        //FIN PROY-140739 Formulario Leads

		Key_CadValorFormaPago = response.Key_CadValorFormaPago; //PROY-140546
		Key_CanalesPermitidosCAI = response.Key_CanalesPermitidosCAI; //PROY-140546
		Key_MensajeMaiMayor = response.Key_MensajeMaiMayor; //FALLAS PROY-140546
        Key_FlagGeneralCobertura = response.Key_FlagGeneralCobertura; //Key_FlagGeneralCobertura

		Key_MsgCoberturaIFI = response.Key_MsgCoberturaIFI; //INICIATIVA 992
		
		//PROY-140743 - IDEA-141192 - INI
		Key_FlagGeneralVtaCuotas = response.Key_FlagGeneralVtaCuotas;
		Key_TipoMatVentaVarias = response.Key_TipoMatVentaVarias;
		Key_OpcionSinPromocion = response.Key_OpcionSinPromocion;
		Key_MsjAccNoCuotas = response.Key_MsjAccNoCuotas;
		Key_MsjClienteNoAplicaCuotas = response.Key_MsjClienteNoAplicaCuotas;
		Key_MsjNoAplicaCuotas = response.Key_MsjNoAplicaCuotas;
		Key_OpcionLineaCuenta = response.Key_OpcionLineaCuenta;
		Key_MsjSelecAcc = response.Key_MsjSelecAcc;
		Key_MsjSelecLineaCuenta = response.Key_MsjSelecLineaCuenta;
		Key_MsjVntaCuotasBRMS = response.Key_MsjVntaCuotasBRMS;
		//PROY-140743 - IDEA-141192 - FIN	
    }
});
//#region