using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDataCreditoCorpOUT
    {
        #region Propiedades SERVICIO 12

        public string ws12_In_TipoDocumento { get; set; }
        public string ws12_In_NumeroDocumento { get; set; }
        public string ws12_In_ApellidoPaterno { get; set; }
        public string ws12_In_ApellidoMaterno { get; set; }
        public string ws12_In_Nombres { get; set; }
        public string ws12_In_TipoPersona { get; set; }
        public string ws12_In_TipoServicio { get; set; }
        public string ws12_Out_Header_Transaccion { get; set; }
        public string ws12_Out_Header_TipoServicio { get; set; }
        public string ws12_Out_Header_CodigoRetorno { get; set; }
        public string ws12_Out_Header_NumeroOperacion { get; set; }
        public string ws12_Out_Error_CodigoMensajes { get; set; }
        public string ws12_Out_Error_Mensaje { get; set; }

        #endregion

        #region Propiedades SERVICIO 50

        public string ws50_In_TipoServicio { get; set; }
        public string ws50_In_NumeroOperacion { get; set; }
        public string ws50_Out_Header_Transaccion { get; set; }
        public string ws50_Out_Header_TipoServicio { get; set; }
        public string ws50_Out_Header_CodigoRetorno { get; set; }
        public string ws50_Out_Header_NumeroOperacion { get; set; }
        public string ws50_Out_GrupoIntegrantes_IntegranteTipoDocumento { get; set; }
        public string ws50_Out_GrupoIntegrantes_IntegranteNumeroDocumento { get; set; }
        public string ws50_Out_GrupoIntegrantes_IntegranteNombres { get; set; }
        public string ws50_Out_CampoNombre_Accion { get; set; }
        public string ws50_Out_CampoExisteCampo_Accion { get; set; }
        public string ws50_Out_CampoValor_Accion { get; set; }
        public string ws50_Out_CampoNombre_MsgIngTarjeta { get; set; }
        public string ws50_Out_CampoExisteCampo_MsgIngTarjeta { get; set; }
        public string ws50_Out_CampoValor_MsgIngTarjeta { get; set; }
        public string ws50_Out_CampoNombre_MsgIngDHipotecaria { get; set; }
        public string ws50_Out_CampoExisteCampo_MsgIngDHipotecaria { get; set; }
        public string ws50_Out_CampoValor_MsgIngDHipotecaria { get; set; }
        public string ws50_Out_CampoNombre_MsgIngDnHipoTarjeta { get; set; }
        public string ws50_Out_CampoExisteCampo_MsgIngDnHipoTarjeta { get; set; }
        public string ws50_Out_CampoValor_MsgIngDnHipoTarjeta { get; set; }
        public string ws50_Out_CampoNombre_Explicacion { get; set; }
        public string ws50_Out_CampoExisteCampo_Explicacion { get; set; }
        public string ws50_Out_CampoValor_Explicacion { get; set; }
        public string ws50_Out_CampoNombre_ExplicacionAuxiliar { get; set; }
        public string ws50_Out_CampoExisteCampo_ExplicacionAuxiliar { get; set; }
        public string ws50_Out_CampoValor_ExplicacionAuxiliar { get; set; }

        #endregion

        #region Propiedades SERVICIO 53

        public string ws53_In_TipoServicio { get; set; }
        public string ws53_In_NumeroOperacion { get; set; }
        public string ws53_Out_Header_Transaccion { get; set; }
        public string ws53_Out_Header_TipoServicio { get; set; }
        public string ws53_Out_Header_CodigoRetorno { get; set; }
        public string ws53_Out_Header_NumeroOperacion { get; set; }
        /*INI PROY 32438*/
        public string ws53_Out_Header_TipContribuyente { get; set; }
        public string ws53_Out_Header_NomComercial { get; set; }
        public string ws53_Out_Header_FecIniActividades { get; set; }
        public string ws53_Out_Header_EstContribuyente { get; set; }
        public string ws53_Out_Header_CondContribuyente { get; set; }
        public string ws53_Out_Header_CiiuContribuyente { get; set; }
        public string ws53_Out_Header_CantTrabajadores { get; set; }
        public string ws53_Out_Header_EmisionComp { get; set; }
        public string ws53_Out_Header_SistEmielectronica { get; set; }
        public Int64 ws53_Out_Header_CantMesIniAct { get; set; }
        /*INI PROY 32438*/


        #endregion

        public List<BERepresentanteLegal> oRepresentantesLegal { get; set; }
        public List<BERepresentanteLegalDC> oRepresentantesLegalDC { get; set; }

        //INICIO PROY-20054-IDEA-23849
        public int buro_consultado { get; set; }
        //FIN
    }
}
