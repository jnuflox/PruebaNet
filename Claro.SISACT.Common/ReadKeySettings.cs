using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//INI PROY-CAMPANA LG
using Claro.SISACT.Entity;
//FIN PROY-CAMPANA LG
namespace Claro.SISACT.Common
{
    #region PROY-31636 | RENTESEG | Bryan Chumbes Lizarraga
    [Serializable]
    public static class ReadKeySettings
    {
        public static string Key_codigoDocMigratorios { get; set; }
        public static string Key_codigoDocMigraYPasaporte { get; set; }
        public static string Key_codDocMigra_Pasaporte_CE { get; set; }
        public static string Key_codigoDocCIRE { get; set; }
        public static string Key_codigoDocCIE { get; set; }
        public static string Key_codigoDocCPP { get; set; }
        public static string Key_codigoDocCTM { get; set; }
        public static string Key_minLengthDocMigratorios { get; set; }
        public static string Key_maxLengthDocMigratorios { get; set; }
        public static string Key_codigoDocPasaporte07 { get; set; }
        public static string Key_codigoDocPasaporte08 { get; set; }
        public static string Key_flagPermitirProductosLTE { get; set; }
        public static string Key_docsExistEvaluacionPost { get; set; }
        public static string Key_tipoDocPermitidoPostCAC { get; set; }
        public static string Key_tipoDocPermitidoPostDAC { get; set; }
        public static string Key_tipoDocPermitidoPostCAD { get; set; }
        public static string Key_codigoDocClienUNI { get; set; }
        //INC000002396378
        public static string Key_EstadoSecRechazado { get; set; }
        //INC000002396378

        //INC000003443673
        public static string Key_ValorSinApellidoPaternoOMaterno { get; set; }
        //INC000003443673

        //INI PROY-CAMPANA LG
        public static string keyComboLG_CampanasAutorizadas { get; set; }
        public static string keyComboLG_CampanasReglas { get; set; }
        public static string keyComboLG_CampanaMensaje_ClienteConExistencia { get; set; }
        public static string keyComboLG_CampanaMensaje_ClienteNoAplica { get; set; }
        public static List<BEItemGenericoLG> list_CampanasReglas { get; set; }
        //FIN PROY-CAMPANA LG


        /*INICIO PROY-140380 - Beneficio Full Claro*/
        public static string key_TipoCanal { get; set; }
        public static string key_TipoOperacion { get; set; }
        public static string key_TipoOferta { get; set; }
        public static string key_FlagPorta { get; set; }
        public static string key_TipoProducto { get; set; }
        public static string key_TipoDocumento { get; set; }
        public static string key_msjSinServicios { get; set; }
        public static string key_msjConServicios { get; set; }
        public static string key_msjServicioMovil { get; set; }
        public static string key_msjServicioFijo { get; set; }
        public static string key_FlagGeneralBeneficio { get; set; }
        public static string key_msjBeneficioFija { get; set; }
        public static string key_msjBeneficioMovil { get; set; }
        public static string key_msjCampanasFullClaro { get; set; }
        public static string key_msjConfigFullClaro { get; set; }
        public static string key_msjConfigFullClaroPopup { get; set; }
        
        /*FIN PROY-140380 - Beneficio Full Claro*/

        
        /*INICIO PROY-140560 - Beneficio Full Claro popu antes de grabar sec*/
        public static string key_MsjSinServicioActivos { get; set; }
        public static string key_MsjConBeneficio { get; set; }
        public static string key_MsjConBeneficioFCVentPag { get; set; }
        public static string key_FlagServicioMovil { get; set; }
        public static string key_FlagServicioFijo { get; set; }
        public static string key_FlagConServicios { get; set; }
        public static string key_FlagMsjSinServicioActivos { get; set; }
        public static string key_FlagMsjConBeneficio { get; set; }
        public static string key_FlagMsjConBeneficioFCVentPag { get; set; }
        /*FIN PROY-140560 - Beneficio Full Claro popu antes de grabar sec*/
        
        /*FIN PROY-140380 - Beneficio Full Claro*/
        
        // INC000003062381  - INI
        public static string Key_EstadoPendienteAprobacion { get; set; }
        public static string Key_MensajeDeEstadoSec { get; set; }
        // INC000003062381  - FIN
        
        //INC-SMS_PORTA_INI
        /*INICIO PROY-140356 IDEA-140951 Enviar SMS al solicitar portabilidad*/
        public static string key_flag_smsportabilidad { get; set; }
        public static string key_canales { get; set; }
        public static string key_OfertasPermitidas { get; set; }
        public static string key_ProductosPermitidos { get; set; }
        public static string key_CanalesPermitidos { get; set; }
        public static string key_MsjNoCumpleValidacion { get; set; } 
        public static string key_CodEncriptacion { get; set; }
        public static string key_MsjErrorValidacionPIN { get; set; }
        /*FIN PROY-140356 IDEA-140951 Enviar SMS al solicitar portabilidad*/
        //INC-SMS_PORTA_FIN

        //INICIO PROY-140419 Autorizar Portabilidad sin PIN
        public static int key_flagSmsPortaSuperv { get; set; }
        public static string key_SmsPortaSupervPerfil { get; set; }
        public static string key_SmsPortaSupervCanales { get; set; }
        public static string key_CacNoPresencial { get; set; }
        public static string key_SmsPortaSupervTipoDoc { get; set; }
        public static string key_MsjExito { get; set; }
        public static string key_MsjPerfilSinPermisos { get; set; }
        public static string key_MsjPerfilIncorrecto { get; set; }
        public static string key_SupervTipoProducto { get; set; }
        public static string key_SupervOferta { get; set; }
        public static string key_SupervMsjNoCumpleValidacion { get; set; }
        //FIN PROY-140419 Autorizar Portabilidad sin PIN

        //INC000002245048-INICIO
        public static string Key_FlagActivacionTopeConsumoAlta { get; set; }
        //INC000002245048-FIN  

        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
        public static string Key_msjSinValidacionCamp { get; set; }
        public static string Key_canalesBRMSCamp { get; set; }
        public static string Key_mventaBRMSCamp { get; set; }
        public static string Key_tofertaBRMSCamp { get; set; }
        public static string Key_tdocumentoBRMSCamp { get; set; }
        public static string Key_toperacionBRMSCamp { get; set; }
        public static string Key_tproductoBRMSCamp { get; set; }
        public static string Key_fportabilidaBRMSCamp { get; set; }
        public static string Key_flagBRMSCamp { get; set; }
        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

        //INI-INC000002510501  Campañas
        public static string Key_R_Flag { get; set; }
        public static string Key_R_Modalidad { get; set; }
        public static string Key_R_Operador { get; set; }
        public static string Key_R_Campana { get; set; }
        public static string Key_R_FlagCampana { get; set; }
        //FIN-INC000002510501  Campañas

        //VALIDACION-FRAUDE - INI
        public static string Key_MensajeResponse_Venta { get; set; }
        public static string Key_MensajeResponse_Evaluacion { get; set; }
        //VALIDACION-FRAUDE - INI

        //PROY-140457-DEBITO AUTOMATICO-INI
        public static string Key_flagDebitoAuto { get; set; }
        public static string Key_canalesDebitoAuto { get; set; }
        public static string Key_tdocumentoDebitoAuto { get; set; }
        public static string Key_tproductoDebitoAuto { get; set; }
        public static string Key_toperacionDebitoAuto { get; set; }
        public static string Key_flagPortaDebitoAuto { get; set; }
        public static string Key_OfertaDebitoAuto { get; set; }
        public static string Key_eTarjetaDebitoAuto { get; set; }
        public static string Key_CuentaEDebitoAuto { get; set; }
        public static string Key_tOperacionRestriccion { get; set; }
        public static string Key_canalesRestriccion { get; set; }
        public static string Key_productoRestriccion { get; set; }
        public static string Key_mVentaRestriccion { get; set; }
        public static string Key_campanasRestriccion { get; set; }
        public static string Key_flagPortaRestriccion { get; set; }
        public static string Key_msjErrorDebitoAuto { get; set; }
        public static string Key_msjErrorFraudeDebito { get; set; }
        //PROY-140457-DEBITO AUTOMATICO-FIN

        //PROY-140335-ANULACION - INI
        public static string Key_FlagAnulacion { get; set; }
        public static string Key_CodigoCanales { get; set; }
        //PROY-140335-ANULACION - FIN

        /*INICIO PROY140542 - IDEA141640 Mejora en Proceso de Omision de PIN SMS de portabilidad*/
        public static string Key_TituloCausalOmision { get; set; }
        public static string Key_CausalesOmisionPIN{ get; set; }
        public static string Key_FlagPermitirTodosDocumentos { get; set; }
        /*FIN PROY140542 - IDEA141640 Mejora en Proceso de Omision de PIN SMS de portabilidad*/
    //INC000002977281-INI
        public static string Key_codFlagBFClaroFijaApagado { get; set; }
        //INC000002977281-FIN

        public static string Key_ReintentosRegistroRRLL { get; set; } //INC000003013199

        //INICIATIVA-219 - INI
        public static string Key_TipoDocumentoFullClaro { get; set; }
        public static string Key_TipoClienteFullClaro { get; set; }
        //INICIATIVA-219 - FIN

        /*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
        public static string Key_FlagGeneralOfertaDefault { get; set; }
        public static string Key_CanalPermitidoOfertaDefault { get; set; }
        public static string Key_OperacionPermitidaOfertaDefault { get; set; }
        public static string Key_DocumentosPermitidosOfertaDefault { get; set; }
        public static string Key_CodigoOfertaDefault { get; set; }
        public static string Key_IsPortabilidadOfertaDefault { get; set; }
        /*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta*/

        //PROY-140618-INI
        public static string Key_TipoOperPermitida { get; set; }
        public static string Key_TipoProdPermitido { get; set; }
        //PROY-140618-FIN

        public static string key_FlagSMSxPDVEvaluacionPrePost { get; set; }//PROY-140585 F2
        public static string key_flag_validacionPDV { get; set; }//PROY-140585 F2
        public static string key_CanalPermMsjeBRMSCamp{ get; set; }//PROY-140585 F2
        public static  string key_OperaPermMsjeBRMSCamp{ get; set; }//PROY-140585 F2
        public static string key_MsjAplicaCuotas { get; set; }//PROY-140585 F2

        //PROY-140335 - INICIO EJRC
        public static string Key_ConsCPPuntoVenta { get; set; }
        //PROY-140335 - FIN EJRC

        //INICIATIVA - 733 - INI - C16
        public static string Key_CodEquipoIPTV { get; set; }
        public static string Key_CodClaroVideoIPTV { get; set; }
        public static string Key_strMensajeClaroVideoIPTV { get; set; }
        public static string Key_CodGrupoServicio { get; set; }
        //INICIATIVA - 733 - FIN - C16

        //INC000003430042 - INICIO
        public static string Key_minLengthDocPass { get; set; }
        //INC000003430042 - FIN

        // INICIATIVA - 803 - INI
        public static string Key_FlagAprobPrecio { get; set; }
        public static string Key_MsjFactorSubsidio { get; set; }
        public static string Key_MsjIngresarIdPedido { get; set; }

        public static string Key_PorcentSubsidio { get; set; }
        public static string Key_MsjErrorExcepPrec { get; set; }
        public static string Key_CodEstadoAprobFactorSub { get; set; }
        public static string Key_DesEstadoAprobFactorSub { get; set; }
        public static string Key_MsjSecEnviadoPoolExcepcionPrecio { get; set; }

        public static string Key_CodEstadoSinExcepcionPrecio { get; set; }
        public static string Key_CodEstadoPendAprobacion { get; set; }
        public static string Key_IdFlujoEvalConsumer { get; set; }
        public static string Key_EstadosSolicitud { get; set; }
        public static string Key_TipoBusquedaPool { get; set; }
        public static string key_EstadosVisualizaBotones { get; set; }
        public static string Key_EstadoPendValidacion { get; set; }
        public static string keyPerfilesAprobadores { get; set; }
        public static string keyPerfilesValidadores { get; set; }
        public static string Key_FlagApagadoExcepcionPrecio { get; set; }   
        public static string Key_FlagApagadoValidacionSubsidio { get; set; }
        // INICIATIVA - 803 - FIN

        //INC000003910770 - INI
        public static string Key_TipoDocumentoBSCS7 { get; set; }
        //INC000003910770 - FIN

    //PROY-140736 INI
        public static string Key_CodCampaniaBuyBack { get; set; }
        public static string Key_Max_Length_Cupon { get; set; }
        public static string Key_Min_Length_IMEI { get; set; }
        public static string Key_Max_Length_IMEI { get; set; }
        public static string Key_Msj_Error_LP_BuyBack { get; set; }
        public static string Key_Msj_Error_CBO_BuyBack { get; set; }
        public static string Key_Msj_Error_Cupon_BuyBack { get; set; }
        public static string Key_Msj_Error_Fila_BuyBack { get; set; }
        public static string Key_Msj_Error_Igual_Cupon { get; set; }
        public static string Key_Msj_Error_Igual_Imei { get; set; }
        public static string Key_Ingr_Cupon_Buyback { get; set; }
        public static string Key_Ingr_IMEI_Buyback { get; set; }
        public static string Key_Sel_Equipo_Buyback { get; set; }
        public static string Key_CuponConSecPagado { get; set; }
        public static string Key_CuponConSecPendPago { get; set; }
        public static string Key_CuponExpirado { get; set; }
        public static string Key_CuponSecAprobado { get; set; }
        //PROY-140736-FIN

                //PROY-140657 INI
        public static string key_idEntidadDEAU { get; set; }
        public static string key_OrigenSolicitudDEAU { get; set; }
        public static string key_idOrigenCuentaDEAU { get; set; }
        public static string key_idMonedaDEAU { get; set; }
        public static string key_OrigenAfiliacionDEAU { get; set; }
        public static string key_EstadoMpDEAU { get; set; }
        public static string key_EstadoEnvioLinkDEAU { get; set; }
        public static string key_EstadoVentaDEAU { get; set; }
        public static string key_ComentarioDEAU { get; set; }
        public static string key_EstadoAfiliacionAltaDEAU { get; set; }
        public static string key_TipoAutorizadoDocumentoDEAU { get; set; }
        public static string key_MsjCorreoObligatorioDEAU { get; set; }
        public static string key_MsjValidacionMontoDEAU { get; set; }
        public static string key_FlagAfiliacionDEAU { get; set; }
        public static string key_TipoConsultaDEAU { get; set; }
        public static string key_TipoFlujoEnvioLinkDEAU { get; set; }
        public static string key_CanalMPSisactDEAU { get; set; }
        public static string key_HorasFechaVencimientoDEAU { get; set; }
        public static string key_TipoTipiMovilDEAU { get; set; }
        public static string key_TipoTipi3PlayDEAU { get; set; }
        public static string key_FlagMontoDEAU { get; set; }
        public static string key_DocumentosMotorDEAU { get; set; }
        public static string key_FlagConsultarAltaDEAU { get; set; }
        //PROY-140657 FIN

        //INI PROY-140739 Formulario Leads
        public static string KeyLeadsCanal { get; set; }
        public static string KeyLeadsPDV { get; set; }
        public static string KeyLeadsTopenPostPago { get; set; }
        public static string KeyLeadsFlag { get; set; }
        public static string KeyLeadsMensajeObligatorio { get; set; }
        public static string KeyLeadsMaxLength { get; set; }
        public static string KeyLeadsProductosPermitidosPostpago { get; set; }
        public static string KeyLeadsMensajeError { get; set; }
        //FIN PROY-140739 Formulario Leads

		//INICIO PROY-140546 Cobro Anticipado de Instalacion
        public static string Key_EstadosPago { get; set; }
        public static string ConsFlagAplicaCAI { get; set; }
        public static int ConsTiempoSecPendientePagoLink { get; set; } // En Horas 
        public static string Key_OperationConsultaHistorico { get; set; }
        public static string ConsConsumerConsultaPA { get; set; }
        public static string ConsCountryConsultaPA { get; set; }
        public static string ConsDispositivoConsultaPA { get; set; }
        public static string ConsLanguageConsultaPA { get; set; }
        public static string ConsModuloConsultaPA { get; set; }
        public static string ConsMsgTypeConsultaPA { get; set; }
        public static string ConsOperationConsultaPA { get; set; }
        public static string ConsPidConsultaPA { get; set; }
        public static string ConsSystemConsultaPA { get; set; }
        public static string ConsWsIpConsultaPA { get; set; }
        public static string ConsCodigoPDVTeleventas { get; set; }
        public static string ConsMsjValidacionSecPendPagoLink { get; set; }
        public static string ConsMsjValidacionSubFormularioCAI { get; set; }
        public static string ConsMontoDescuentoPorFullClaroCAI { get; set; }
        public static string ConsCurrentUser { get; set; }
        public static string ConsCurrentIP { get; set; }
        public static string ConsCadValorFormaPago { get; set; }
        public static string Key_CanalesPermitidosCAI { get; set; }
        public static string Key_MontoDescuentoPorFullClaroCAI { get; set; }
        /* rmr ca fase 2: ini */
        public static string ConsCadEstadoAnulado { get; set; }
        public static string ConsCadEstadoPagado { get; set; }
        /* rmr ca fase 2: fin */        
        //FIN PROY-140546 Cobro Anticipado de Instalacion
        public static string Key_MensajeMaiMayor { get; set; } //FALLAS PROY-140546
        public static string Key_FlagGeneralCobertura { get; set; } //INICIATIVA-932


        //INICIATIVA-992 INICIO
        public static string Key_MsgCoberturaIFI { get; set; } 
        public static string Key_MsgParametrosIFI { get; set; }
        public static string Key_MsgAreaCoberturaIFI { get; set; }
        public static string Key_MsgErrorServicioIFI { get; set; }
        public static string Key_MsgErrorConsultaIFI { get; set; }
        public static string Key_MsgErrorTimeOutIFI { get; set; }
        public static string Key_MsgErrorDisponibilidadIFI { get; set; }
        public static string Key_MsgErrorInesperadoIFI { get; set; }

        //PROY-140715- INI ANGEL
        public static string Key_MsjInformativo { get; set; }
        public static string Key_MsjRecordatorio { get; set; }
        public static string Key_MsjMostrar { get; set; }
        public static string Key_MsjMostrarOperacion { get; set; }
        public static string Key_MsjMostrarVenta { get; set; }
        public static string Key_OperacionAlta { get; set; }
        public static string Key_OperacionMigracion { get; set; }
        public static string Key_OperacionPorta { get; set; }
        public static string Key_OperacionReno { get; set; }
        public static string Key_OperacionRepo { get; set; }
        public static string Key_PreOperacionAlta { get; set; }
        public static string Key_PreOperacionPorta { get; set; }
        public static string Key_PreOperacionRenoChip { get; set; }
        public static string Key_PreOperacionRenoPack { get; set; }
        public static string Key_PreOperacionRenoEqui { get; set; }
        public static string Key_SistemaSISACTPRE { get; set; }
        public static string Key_SistemaSISACT { get; set; }
        //PROY-140715- FIN ANGEL

        public static string Key_FlagGeneralLineasAdicionales { get; set; }
        public static string Key_ProductosPermitidosLineasAdic { get; set; }
        public static string Key_CanalesPermitidosLineasAdic { get; set; }
        public static string Key_DocumentosPermitidosLineasAdic { get; set; }
        public static string Key_ModalidadVentaPermitidoLineasAdic { get; set; }
        public static string Key_TipoOperacionPermitidoLineasAdic { get; set; }
        public static string Key_TipoOfertaPermitidoLineasAdic { get; set; }
        public static string Key_TipoRUCPermitidoLineasAdic { get; set; }
        public static string Key_MsjClienteSITieneLineasAdic { get; set; }
        public static string Key_MsjClienteNOTieneLineasAdic { get; set; }
        public static string Key_MsjClienteNuevo { get; set; }
        public static string Key_MsjClienteNoTieneLineasMayorCFMinimo { get; set; }
        public static string Key_MsjClienteTieneLineasMayorCFMinimo { get; set; }
        public static string Key_PlanesPermitidosASIS { get; set; }
        public static string Key_PlanesPermitidosTOBE1 { get; set; }
        public static string Key_PlanesPermitidosTOBE2 { get; set; }
        public static string Key_PlanesPermitidosTOBE3 { get; set; }
        public static string Key_PlanesPermitidosTOBE4 { get; set; }
        public static string Key_PlanesPermitidosTOBE5 { get; set; }
        public static string Key_CargoFijoMinimo { get; set; }

        public static string Key_FlagGeneralCampanasDcto { get; set; }
        public static string Key_FlagOcultarCampanasRegulares { get; set; }
        public static string Key_CampanasDscto { get; set; }

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Keys de parametros]
        public static string Key_OpcionSinPromocion { get; set; }
        public static string Key_TipoMatVentaVarias { get; set; }
        public static string Key_FlagGeneralVtaCuotas { get; set; }
        public static string Key_RangoHoras { get; set; }
        public static string Key_MsjNoAplicaCuotas { get; set; }
        public static string Key_MsjAccNoCuotas { get; set; }
        public static string Key_MsjClienteNoAplicaCuotas { get; set; }
        public static string Key_OpcionLineaCuenta { get; set; }
        public static string Key_DesVentaVarias { get; set; }
        public static string Key_MsjSelecAcc { get; set; }
        public static string Key_MsjSelecLineaCuenta { get; set; }
        public static string Key_MsjNoVariablesBRMS { get; set; }
        public static string Key_CodActivacionBSCS { get; set; }
        public static string Key_CodOperacionSOT { get; set; }
        public static string Key_IpWhitelistDatosSOT { get; set; }
        public static string Key_OperationServiceHeader { get; set; }
        public static string Key_DesEstadoSOT { get; set; }
        public static string Key_MsjVntaCuotasBRMS { get; set; }
        public static string KEY_CONSTDNIBRMS { get; set; }
        public static string KEY_CONSTCEBRMS { get; set; }
        public static string KEY_CONSTRUC10BRMS { get; set; }
        public static string KEY_CONSTRUC20BRMS { get; set; }
        public static string KEY_CONSTPASSBRMS { get; set; }
        public static string KEY_CONSTCIREBRMS { get; set; }
        public static string KEY_CONSTCIEBRMS { get; set; }
        public static string KEY_CONSTCPPBRMS { get; set; }
        public static string KEY_CONSTCTMBRMS { get; set; }
        public static string Key_PlazoAccCuota { get; set; }
        public static string Key_vtaCuo_tipoOpeBRMSCamp { get; set; }
        #endregion
}


    public class JSReadKeySettings
    {
        public string Key_codigoDocMigratorios { get; set; }
        public string Key_codigoDocMigraYPasaporte { get; set; }
        public string Key_codDocMigra_Pasaporte_CE { get; set; }
        public string Key_codigoDocCIRE { get; set; }
        public string Key_codigoDocCIE { get; set; }
        public string Key_codigoDocCPP { get; set; }
        public string Key_codigoDocCTM { get; set; }
        public string Key_minLengthDocMigratorios { get; set; }
        public string Key_maxLengthDocMigratorios { get; set; }

        //INC000003430042 - INICIO
        public string Key_minLengthDocPass { get; set; }
        //INC000003430042 - FIN

        public string Key_codigoDocPasaporte07 { get; set; }
        public string Key_flagPermitirProductosLTE { get; set; }
        public string Key_docsExistEvaluacionPost { get; set; }
        public string Key_tipoDocPermitidoPostCAC { get; set; }
        public string Key_tipoDocPermitidoPostDAC { get; set; }
        public string Key_tipoDocPermitidoPostCAD { get; set; }
        //INC000002396378
        public string Key_EstadoSecRechazado { get; set; }
        //INC000002396378

        //INC000003443673
        public string Key_ValorSinApellidoPaternoOMaterno { get; set; }
        //INC000003443673

        //INI PROY-CAMPANA LG
        public string keyComboLG_CampanasAutorizadas { get; set; }
        public string keyComboLG_CampanasReglas { get; set; }
        public string keyComboLG_CampanaMensaje_ClienteConExistencia { get; set; }
        public string keyComboLG_CampanaMensaje_ClienteNoAplica { get; set; }
        //FIN PROY-CAMPANA LG

        //INC000002245048-INICIO
        public string Key_FlagActivacionTopeConsumoAlta { get; set; }
        //INC000002245048-FIN

        //PROY-FULLCLARO.V2-INI
        public string key_msjBeneficioFija { get; set; }
        public string key_msjBeneficioMovil { get; set; }
        //PROY-FULLCLARO.V2-FIN

        public string key_msjCampanasFullClaro { get; set; }
        public string key_msjConfigFullClaro { get; set; }
        public string key_msjConfigFullClaroPopup { get; set; }

        //PROY-140457-DEBITO AUTOMATICO-INI
        public string Key_flagDebitoAuto { get; set; }
        //PROY-140457-DEBITO AUTOMATICO-FIN

        //INC000002977281-INI
        public string Key_codFlagBFClaroFijaApagado { get; set; }
        //INC000002977281-FIN

        public string Key_ReintentosRegistroRRLL { get; set; } //INC000003013199

        /*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
        public string Key_FlagGeneralOfertaDefault { get; set; }
        public string Key_CanalPermitidoOfertaDefault { get; set; }
        public string Key_OperacionPermitidaOfertaDefault { get; set; }
        public string Key_DocumentosPermitidosOfertaDefault { get; set; }
        public string Key_CodigoOfertaDefault { get; set; }
        public string Key_IsPortabilidadOfertaDefault { get; set; }
        /*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta*/

        //PROY-140618-INI
        public string Key_TipoOperPermitida { get; set; }
        public string Key_TipoProdPermitido { get; set; }
        //PROY-140618-FIN

        public string key_CanalPermMsjeBRMSCamp { get; set; }//PROY-140585 F2
        public string key_OperaPermMsjeBRMSCamp { get; set; }//PROY-140585 F2
        public string key_MsjAplicaCuotas { get; set; }//PROY-140585 F2

        //INICIATIVA - 733 - INI - C20
        public string Key_CodEquipoIPTV { get; set; }
        public string Key_CodClaroVideoIPTV { get; set; }
        public string Key_strMensajeClaroVideoIPTV { get; set; }
        public string Key_CodGrupoServicio { get; set; }
        //INICIATIVA - 733 - FIN - C20

        // INICIATIVA - 803 - INI
        public string Key_MsjFactorSubsidio { get; set; }
        public string Key_MsjIngresarIdPedido { get; set; }
        public string Key_PorcentSubsidio { get; set; }
        public string Key_MsjErrorExcepPrec { get; set; }
        public string Key_FlagAprobPrecio { get; set; }
        public string Key_FlagApagadoExcepcionPrecio { get; set; }
        public string Key_FlagApagadoValidacionSubsidio { get; set; }
       
        // INICIATIVA - 803 - FIN
        //PROY-140736 INI
        public  string Key_CodCampaniaBuyBack { get; set; }
        public  string Key_Max_Length_Cupon { get; set; }
        public  string Key_Min_Length_IMEI { get; set; }
        public  string Key_Max_Length_IMEI { get; set; }
        public  string Key_Msj_Error_LP_BuyBack { get; set; }
        public  string Key_Msj_Error_CBO_BuyBack { get; set; }
        public  string Key_Msj_Error_Cupon_BuyBack { get; set; }
        public  string Key_Msj_Error_Fila_BuyBack { get; set; }
        public  string Key_Msj_Error_Igual_Cupon { get; set; }
        public  string Key_Msj_Error_Igual_Imei { get; set; }
        public  string Key_Ingr_Cupon_Buyback { get; set; }
        public  string Key_Ingr_IMEI_Buyback { get; set; }
        public  string Key_Sel_Equipo_Buyback { get; set; }
        public  string Key_CuponConSecPagado { get; set; }
        public  string Key_CuponConSecPendPago { get; set; }
        public  string Key_CuponExpirado { get; set; }
        public  string Key_CuponSecAprobado { get; set; }
        //PROY-140736-FIN


        //PROY-140657 INI
        public string key_idEntidadDEAU { get; set; }
        public string key_OrigenSolicitudDEAU { get; set; }
        public string key_idOrigenCuentaDEAU { get; set; }
        public string key_idMonedaDEAU { get; set; }
        public string key_OrigenAfiliacionDEAU { get; set; }
        public string key_EstadoMpDEAU { get; set; }
        public string key_EstadoEnvioLinkDEAU { get; set; }
        public string key_EstadoVentaDEAU { get; set; }
        public string key_ComentarioDEAU { get; set; }
        public string key_EstadoAfiliacionAltaDEAU { get; set; }
        public string key_TipoAutorizadoDocumentoDEAU { get; set; }
        public string key_MsjCorreoObligatorioDEAU { get; set; }
        public string key_MsjValidacionMontoDEAU { get; set; }
        public string key_FlagAfiliacionDEAU { get; set; }
        public string key_TipoConsultaDEAU { get; set; }
        public string key_TipoFlujoEnvioLinkDEAU { get; set; }
        public string key_CanalMPSisactDEAU { get; set; }
        public string key_HorasFechaVencimientoDEAU { get; set; }
        public string key_TipoTipiMovilDEAU { get; set; }
        public string key_TipoTipi3PlayDEAU { get; set; }
        public string key_FlagMontoDEAU { get; set; }
        public string key_DocumentosMotorDEAU { get; set; }
        public string key_FlagConsultarAltaDEAU { get; set; }
        //PROY-140657 FIN

        //INI PROY-140739 Formulario Leads
        public string KeyLeadsCanal { get; set; }
        public string KeyLeadsPDV { get; set; }
        public string KeyLeadsTopenPostPago { get; set; }
        public string KeyLeadsFlag { get; set; }
        public string KeyLeadsMensajeObligatorio { get; set; }
        public string KeyLeadsMaxLength { get; set; }
        public string KeyLeadsProductosPermitidosPostpago { get; set; }
        //FIN PROY-140739 Formulario Leads

		public string Key_CadValorFormaPago { get; set; } //PROY-140546
        public string Key_CanalesPermitidosCAI { get; set; } //PROY-140546
        public string Key_MontoDescuentoPorFullClaroCAI { get; set; } //PROY-140546
        public string Key_MensajeMaiMayor { get; set; } //FALLAS PROY-140546
        public string Key_FlagGeneralCobertura { get; set; } //INICIATIVA-932

        //INICIATIVA-992 INICIO 

        public string Key_MsgCoberturaIFI { get; set; } 
        public string Key_MsgParametrosIFI { get; set; }
        public string Key_MsgAreaCoberturaIFI { get; set; }
        public string Key_MsgErrorServicioIFI { get; set; }
        public string Key_MsgErrorConsultaIFI { get; set; }
        public string Key_MsgErrorTimeOutIFI { get; set; }
        public string Key_MsgErrorDisponibilidadIFI { get; set; }
        public string Key_MsgErrorInesperadoIFI { get; set; }

        //INICIATIVA-992 FIN 

        //PROY-140715- INI ANGEL
        public string Key_MsjInformativo { get; set; }
        public string Key_MsjRecordatorio { get; set; }
        public string Key_MsjMostrar { get; set; }
        public string Key_MsjMostrarOperacion { get; set; }
        public string Key_MsjMostrarVenta { get; set; }
        public string Key_OperacionAlta { get; set; }
        public string Key_OperacionMigracion { get; set; }
        public string Key_OperacionPorta { get; set; }
        public string Key_OperacionReno { get; set; }
        public string Key_OperacionRepo { get; set; }
        public string Key_PreOperacionAlta { get; set; }
        public string Key_PreOperacionPorta { get; set; }
        public string Key_PreOperacionRenoChip { get; set; }
        public string Key_PreOperacionRenoPack { get; set; }
        public string Key_PreOperacionRenoEqui { get; set; }
        //PROY-140715- FIN ANGEL
              
        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Keys de parametros]
        public string Key_OpcionSinPromocion { get; set; }
        public string Key_TipoMatVentaVarias { get; set; }
        public string Key_FlagGeneralVtaCuotas { get; set; }
        public string Key_MsjAccNoCuotas { get; set; }
        public string Key_MsjClienteNoAplicaCuotas { get; set; }
        public string Key_MsjNoAplicaCuotas { get; set; }
        public string Key_OpcionLineaCuenta { get; set; }
        public string Key_MsjSelecAcc { get; set; }
        public string Key_MsjSelecLineaCuenta { get; set; }
        public string Key_MsjVntaCuotasBRMS { get; set; }
        #endregion
    }
        #endregion
}
