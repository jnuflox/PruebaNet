using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Claro.SISACT.Entity;

namespace Claro.SISACT.Web.Comun
{
    public static class AppSettings
    {
        public static string consMsgConsultaPreviaPendienteABCDP { get; set; }
        public static string consMsgConsultaPreviaServicioNoRespondio { get; set; }
        public static int consReintentosConsultaCP { get; set; }
        public static int consFrecuenciaConsultaCP { get; set; }
        public static string consProductosConsultaCP { get; set; }
        public static string consTipoServicioConsultaCP { get; set; }
        public static string consTipoPortabilidadPortIN { get; set; }
        public static string consTipoModalidadCPPortIN { get; set; }
        public static string consMensajeTimeOutRegistraPortaWS { get; set; }
        public static string consMsgConsultaPreviaExitosa { get; set; }
        public static string consObservacionActualizarCP { get; set; }
        public static string consObservacionRealizarCP { get; set; }
        public static string consMensajeErrorActualizarCP { get; set; }
        public static string consEstadosCPPermitidos { get; set; }
        public static string consMensajesCPCarrito { get; set; }
        public static int consReintentosActualizaSEC { get; set; }//IN000000772139

        public static string consflagVigenciaPromo2x1 { get; set; }
        public static string consConfiguracionPromo2x1 { get; set; }
        public static string consGrupoPromoPorta2x1 = "consGrupoPromoPorta2x1";  //promo2x1_RS_02      
        public static string consPromoPortaErrorCantidadLineas { get; set; }
        public static string consPromoPortaErrorCampanias { get; set; }
        public static string consPromoPortaErrorPlanes { get; set; }
        public static string consPromoPortaErrorGrupoMaterial { get; set; }                              
        public static string consPromoPortaErrorFueradeAlcance { get; set; }        
        public static string consCodigoCampaniaPorta50Dscto { get; set; } // 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
        public static string consNroDiasPermitidosOP { get; set; } // 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
        public static string consMsgPermanenciaOP { get; set; } // 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
        public static List<BEItemGenerico> listMaterialesPromoPorta { get; set; }

        //IDEA-42590
        public static Int32 consBenefPortaDiasAntiguedad { get; set; }
        public static string consBenefPortaCampanas { get; set; }
        public static Int32 consBenefPortaMinLineas { get; set; }
        public static Int32 consBenefPortaMaxLineas { get; set; }
        public static string consBenefPortaMensajeError { get; set; }
        public static string consBenefPortaMensajeWhiteList { get; set; }
        public static string consBenefPortaCasoEspecialWhiteList { get; set; }
        //IDEA-42590

        //PROY 29121-INI
        public static int consAntiguedadDeuda { get; set; }
        public static string consFlagFlexibilidad { get; set; }
        //PROY 29121-FIN      
        //PROY-30166-IDEA–38863-INICIO
        public static string consMontoCuotaMenorA { get; set; } 
        public static string consMontoCuotaMayorA { get; set; }
        public static string consMsjActualizacionCuotaIncial { get; set; }
        public static string consMaxPorcentajeCuotaInicial { get; set; }
        public static string consMontoCuotaMayorAPorcentaje { get; set; }
        //PROY-30166-IDEA–38863-FIN
                
        //INI: PROY-140223 IDEA-140462
        public static string consFlagConsultaPreviaChip { get; set; }
        public static string consCPModdalidadVenta { get; set; }
        public static string consCPCanalVenta { get; set; }
        public static string consMensajeCPApagada { get; set; }
        public static string consCPPuntoVenta { get; set; }
        public static string consClasificacionPDV { get; set; }
        //FIN: PROY-140223 IDEA-140462

        //INI: PROY-BLACKOUT
        public static int consFlagBlackOut { get; set; }
        public static string consMensajeCPExitosoBlackOut { get; set; }
        public static string consMensajeEvaluacionCACBlackOut { get; set; }        
        //FIN: PROY-BLACKOUT

      
        public static string consMsjMontoCuotaValido { get; set; }
        
        public static string consMensValidaOpe { get; set; } //INC000002628010 + 3
        public static string consValorPost { get; set; } //INC000002628010 + 3
        public static string consValorPre { get; set; } //INC000002628010 + 3
        
 //PROY-140383-INI
        public static string consFlagServvAdicionales { get; set; }
        public static string consMensValidaServ { get; set; }
        public static string consProdExc { get; set; }
        public static string consMensServExcCaido { get; set; }
        public static string consDescProceso { get; set; }

        //Serv. Validar Excluyente
        public static string ConsCountryValidServExc { get; set; }
        public static string ConsDispositivoValidServExc { get; set; }
        public static string ConsLanguajeValidServExc { get; set; }
        public static string ConsModuloValidServExc { get; set; }
        public static string ConsMsgtypeValidServExc { get; set; }
        public static string ConsOperationValidServExc { get; set; }
        public static string ConsSystemValidServExc { get; set; }
        public static string ConsWsipValidServExc { get; set; }
        public static string ConsUsuarioEncripServExc { get; set; }
        public static string ConsContraEncripServExc { get; set; }
        //Serv. Insert Excluyente
        public static string ConsCountryInsertServExc { get; set; }
        public static string ConsDispositivoInsertServExc { get; set; }
        public static string ConsLanguajeInsertServExc { get; set; }
        public static string ConsModuloInsertServExc { get; set; }
        public static string ConsMsgtypeInsertServExc { get; set; }
        public static string ConsOperationInsertServExc { get; set; }
        public static string ConsSystemInsertServExc { get; set; }
        public static string ConsWsipInsertServExc { get; set; }

        //PROY-140383-FIN

        public static string consFlagCBIO { get; set; } // INICIATIVA-219

        //INICIO|PROY-140533 - CONSULTA STOCK
        public static string Key_StockNoDisponible { get; set; }
        public static string Key_ProductosPermitidosPicking { get; set; }
        //FIN|PROY-140533 - CONSULTA STOCK

        public static string Key_PaginaSec_AntiFraude { get; set; }//INC000003427525

        //PROY-140618 - IDEA-142181 - Mejora Proceso de Portabilidad INI
        public static string Key_DiasAntiguedad { get; set; }
        public static string Key_CantidadRegistros { get; set; }
        public static string Key_TipoOperPermitida { get; set; }
        public static string Key_FlagPortaPermitido { get; set; }
        public static string Key_TipoProdPermitido { get; set; }
        //PROY-140618 - IDEA-142181 - Mejora Proceso de Portabilidad FIN

 public static string key_ConstMsjeConBonoFC { get; set; }//INC000003048070 

        //    INICIATIVA - 733 - INI - C19
        public static string Key_CodEquipoIPTV { get; set; }
        public static string Key_CodClaroVideoIPTV { get; set; }
        public static string Key_strMensajeClaroVideoIPTV { get; set; }
        public static string Key_CodGrupoServicio { get; set; }
        //    INICIATIVA - 733 - FIN - C19

        public static string consCampanasVacunaton { get; set; } //IDEA-142717

//[PROY-140600] INI
        public static string Key_CLFlagGeneralPost { get; set; }
        public static string Key_CLCodDocPermitido { get; set; }
        public static string Key_CLTipoVentaPos { get; set; }
        public static string Key_CLProductoPermitidoPost { get; set; }
        public static string Key_CLOperacionPermitidaPost { get; set; }
        public static string Key_CLCodCanalPermitido { get; set; }
        //[PROY-140600] FIN

        //INICIATIVA 920 INI
        public static string KeyModalidadDefecto { get; set; }
        public static string Key_Canal_Permitido { get; set; }
        public static string Val_Canal_Permitido { get; set; }
        public static string Key_PDV_Permitido { get; set; }
        public static string Val_PDV_Permitido { get; set; }
        public static string Key_Operacion_Permitido { get; set; }
        public static string Val_Operacion_Permitido { get; set; }
        public static string Key_CuotaSinCode_Permitido { get; set; }
        //INICIATIVA 920 FIN
    }
}