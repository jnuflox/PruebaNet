using System;
using System.Collections.Generic;
using Claro.SISACT.Entity;
using System.Configuration; 

namespace Claro.SISACT.Common
{
    public static class Constantes
    {

        //   INC000003802012
        #region  Documentos de Identidad
        public static string constTipoDoc_DNI = "01";
        public static string constTipoDoc_CIP = "02";
        public static string constTipoDoc_CE = "04";
        public static string constTipoDoc_RUC = "06";
        public static string constTipoDoc_PAS = "07";

        public static int constLongTipoDoc_DNI = 8;
        public static int constLongTipoDoc_CIP = 10;
        public static int constLongTipoDoc_CE = 9;
        public static int constLongTipoDoc_RUC = 11;


        #endregion

        //   INC000003802012

#region Variables Estaticas - Singleton
        public static string UsuarioSap { get; set; }
        public static List<BESolicitudPersona> OSolicitudPersona { get; set; }
        public static string OficinaVenta { get; set; }
        public static string CodCanalSeleccionado { get; set; }
#endregion

#region Tipo Billetera
        public const string constTipoBilleteraMovil = "2";
        public const string constTipoBilleteraInter = "4";
        public const string constTipoBilleteraTV = "8";
        public const string constTipoBilleteraTelef = "16";
        public const string constTipoBilleteraBAM = "32";
#endregion

#region MSSAP 4.6
        public static string SISACConsultaPrecios = ".SISACSS_CONSULTAR_LISTAPRECIOS";        
        public static string SSAPPreciosBase = ".SSAPSS_PRECIOBASE";
        public static string SSAPMaterialesXOficina = ".SSAPSS_MATERIALXOFICINA";
        public static string SISACTConsultaTipoDocumentos = ".SISACT_OFICINA_DOCU_CONS";
        public static string SISACTMaterialesXPlanes = ".SISACT_SEL_MATERIALESCAMPANIA";
        public static string SISACSDatosOficina = ".SISACS_DATOS_OFICINA";
        public static string SSAPDatosOficina = ".SSAPSS_OFICINA";
#endregion

#region Mejoras Beneficio Lineas Adicionales
        public static string KeyFlagCampanasBeneficio = "KeyFlagCampanasBeneficio";
        public static string keyCasoEspecialPermitido = "keyCasoEspecialPermitido";
        public static string KeyMsgWhitelistSi = "KeyMsgWhitelistSi";
        public static string KeyMsgWhitelistNo = "KeyMsgWhitelistNo";
        public static string KeyMsgDtpNo_PortaNo = "KeyMsgDtpNo_PortaNo";
        public static string KeyMsgDtpNo_PortaSi = "KeyMsgDtpNo_PortaSi";
        public static string KeyMsgDtpSi = "KeyMsgDtpSi";
        public static string KeyVigenciaCampanas_Cantidad = "KeyVigenciaCampanas_Cantidad";
        public static string KeyCantidadCampanasVigentes = "KeyCantidadCampanasVigentes";
#endregion
    }
}
