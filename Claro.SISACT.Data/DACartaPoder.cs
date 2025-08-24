using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Collections;
using System.Configuration;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class DACartaPoder
    {
        GeneradorLog objLog = new GeneradorLog("Biometria", null, null, "Log");

        public bool RegistrarCartaPoder(BECartaPoder oCartaPoder) 
        {
             DAABRequest.Parameter[] arrParam ={
             new DAABRequest.Parameter("P_SCPN_SOLIN_CODIGO",DbType.Int64,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPN_NRO_PEDIDO",DbType.Int64,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_ID_TX_P",DbType.String,30,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_TIPO_TRANSACCION",DbType.String,30,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_TIPO_OPERACION",DbType.String,15,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_DESC_OPERACION",DbType.String,100,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_TIPO_DOCUMENTO_AP",DbType.String,2,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_NRO_DOCUMENTO_AP",DbType.String,20,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_NOMBRES_AP",DbType.String,200,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_APELLIDOS_PAT_AP",DbType.String,200,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_APELLIDOS_MAT_AP",DbType.String,200,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_OBSERVACION",DbType.String,150,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_APLICACION",DbType.String,20,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_USUARIO_CREA",DbType.String,15,ParameterDirection.Input),
             new DAABRequest.Parameter("P_CODIGO_RESPUESTA",DbType.String,ParameterDirection.Output),
             new DAABRequest.Parameter("P_MENSAJE_RESPUESTA",DbType.String,ParameterDirection.Output),

            };
            bool salida = false;
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = oCartaPoder.codSec_Solicitud;
            arrParam[1].Value = oCartaPoder.nro_Pedido;
            arrParam[2].Value = oCartaPoder.idTransaccion;
            arrParam[3].Value = oCartaPoder.tipotransaccion;
            arrParam[4].Value = oCartaPoder.tipoOperacion;
            arrParam[5].Value = oCartaPoder.descripcionoperacion;
            arrParam[6].Value = oCartaPoder.tipodocumento;
            arrParam[7].Value = oCartaPoder.numDocumento;
            arrParam[8].Value = oCartaPoder.nomApoderado;
            arrParam[9].Value = oCartaPoder.apellidoPaterno;
            arrParam[10].Value = oCartaPoder.apellidomaterno;
            arrParam[11].Value = oCartaPoder.comentario;
            arrParam[12].Value = oCartaPoder.aplicacion;
            arrParam[13].Value = oCartaPoder.usuariocrea;
           

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CARTA_PODER + ".SISACTSI_CARTA_PODER";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch 
            {
                salida = false;
            }
            finally 
            { 
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return salida ;
        }

        public bool RegistrarRepresentanteLegal(BERepresentanteLegal oRepresentanteLegal) {

             DAABRequest.Parameter[] arrParam ={
             new DAABRequest.Parameter("P_SRLN_SOLIN_CODIGO",DbType.Int64,ParameterDirection.Input),
             new DAABRequest.Parameter("p_SCPN_NRO_PEDIDO",DbType.Int64,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SRLN_APODN_CODIGO",DbType.Int64,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SRLV_ID_TX_P",DbType.String,30,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SRLV_TIPO_DOCUMENTO_RL",DbType.String,2,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SRLV_NRO_DOCUMENTO_RL",DbType.String,20,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SRLV_NOMBRES_RL",DbType.String,200,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SRLV_APELLIDOS_PAT_RL",DbType.String,200,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SRLV_APELLIDOS_MAT_RL",DbType.String,200,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_OBSERVACION",DbType.String,150,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SCPV_APLICACION",DbType.String,20,ParameterDirection.Input),
             new DAABRequest.Parameter("p_SCPV_USUARIO_CREA",DbType.String,15,ParameterDirection.Input),
             new DAABRequest.Parameter("P_SRLC_CODNACIONALIDAD",DbType.String,20,ParameterDirection.Input), //PROY-31636
             new DAABRequest.Parameter("P_SRLV_DESCNACIONALIDAD",DbType.String,80,ParameterDirection.Input), //PROY-31636
             new DAABRequest.Parameter("P_CODIGO_RESPUESTA",DbType.String,ParameterDirection.Output),
             new DAABRequest.Parameter("P_MENSAJE_RESPUESTA",DbType.String,ParameterDirection.Output),

            };
            bool salida = false;
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = oRepresentanteLegal.SOLIN_CODIGO;
            arrParam[1].Value = oRepresentanteLegal.P_SCPN_NRO_PEDIDO;
            arrParam[2].Value = oRepresentanteLegal.APODN_CODIGO;
            arrParam[3].Value = oRepresentanteLegal.P_SRLV_ID_TX_P;
            arrParam[4].Value = oRepresentanteLegal.APODC_TIP_DOC_REP;
            arrParam[5].Value = oRepresentanteLegal.APODV_NUM_DOC_REP;
            arrParam[6].Value = oRepresentanteLegal.APODV_NOM_REP_LEG;
            arrParam[7].Value = oRepresentanteLegal.APODV_APA_REP_LEG;
            arrParam[8].Value = oRepresentanteLegal.APODV_AMA_REP_LEG;
            arrParam[9].Value = oRepresentanteLegal.P_SCPV_OBSERVACION;
            arrParam[10].Value = oRepresentanteLegal.P_SCPV_APLICACION;
            arrParam[11].Value = oRepresentanteLegal.P_SCPV_USUARIO_CREA;
            arrParam[12].Value = oRepresentanteLegal.SRLC_CODNACIONALIDAD; //PROY-31636
            arrParam[13].Value = oRepresentanteLegal.SRLV_DESCNACIONALIDAD; //PROY-31636

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_REPRESENTANTE_LEGAL + ".SISACTSI_REPRESENTANTE_LEGAL";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch 
            {
                salida = false;
                //throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return salida;
        
        
        }
        
    }
}
