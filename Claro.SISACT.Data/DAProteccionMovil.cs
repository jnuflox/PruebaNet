using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Configuration;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class DAProteccionMovil //PROY-24724-IDEA-28174 - NUEVA CLASE
    {
        public void BorrarServicioProteccionMovil(string strNroSec, string strCodServProteccionMovil, string strSoplnCodigo, ref string strCodRespuesta, ref string strMsjRespuesta)
        {
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("K_COD_SEC", DbType.Int64, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_COD_SERV", DbType.String, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_SLPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_COD_RESULTADO", DbType.String, ParameterDirection.Output),
				                                 new DAABRequest.Parameter("K_MSJ_RESULTADO", DbType.String, 100, ParameterDirection.Output)
                                                };

            arrParam[0].Value = Funciones.CheckInt64(strNroSec);
            arrParam[1].Value = strCodServProteccionMovil;
            arrParam[2].Value = Funciones.CheckInt64(strSoplnCodigo);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_TRANS_ASURION + ".SISACTSD_BORRAR_SERV_VENTA";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[3];
                strCodRespuesta = Funciones.CheckStr(parSalida1.Value);

                IDataParameter parSalida2;
                parSalida2 = (IDataParameter)objRequest.Parameters[4];
                strMsjRespuesta = Funciones.CheckStr(parSalida2.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }
        
        public List<BEPrima> BuscarProteccionMovilPvu(string strNroCertif, string strNroSec, string strSoplnCodigo, ref string strCodRespuesta, ref string strMsjRespuesta)
        {
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("K_NRO_CERTIF", DbType.String, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_NRO_SEC", DbType.String, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_SOPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_CURSOR_DATOS", DbType.Object, ParameterDirection.Output),                                                 
                                                 new DAABRequest.Parameter("K_COD_RESULTADO", DbType.String, ParameterDirection.Output),
				                                 new DAABRequest.Parameter("K_MSJ_RESULTADO", DbType.String, ParameterDirection.Output)
                                                };

            arrParam[0].Value = Funciones.CheckStr(strNroCertif);
            arrParam[1].Value = Funciones.CheckStr(strNroSec);

            if (Funciones.CheckStr(strSoplnCodigo) == "")
                arrParam[2].Value = null;
            else
            arrParam[2].Value = Funciones.CheckInt64(strSoplnCodigo);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_TRANS_ASURION + ".SISACTSS_MOSTRAR_EVALUACION";
            objRequest.Parameters.AddRange(arrParam);

            List<BEPrima> lstBEPrimas = new List<BEPrima>();
            IDataReader dr = null;

            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    lstBEPrimas.Add(new BEPrima(){
                        CodEval = Funciones.CheckStr(dr["EVALN_COD_EVAL_SEQ"]),
                        CodMaterial = Funciones.CheckStr(dr["EVALV_COD_MAT_REQ"]),
                        SoplnCodigo = Funciones.CheckStr(dr["EVALN_SOPLN_CODIGO"]),
                        DeducibleDanio = Funciones.CheckStr(dr["EVALV_DEDUCIBLE_DANO"]),
                        DeducibleRobo = Funciones.CheckStr(dr["EVALV_DEDUCIBL_ROBO"]),
                        DescProd = Funciones.CheckStr(dr["EVALV_DESC_PROD_RPTA"]),
                        DescProt = Funciones.CheckStr(dr["EVALV_DESC_PROT_REQ"]),
                        FechaCreacion = Funciones.CheckStr(dr["EVALD_FECHA_CREACION"]),
                        FechaEvaluacion = Funciones.CheckStr(dr["EVALD_FECHA_EVAL"]),
                        FechaModif = Funciones.CheckStr(dr["EVALD_FECHA_MODIF"]),
                        FlagEstado = Funciones.CheckStr(dr["EVALC_FLAG_ESTADO"]),
                        IncidenciaTipoDanio = Funciones.CheckStr(dr["EVALV_INC_TIPO_DANO"]),
                        IncidenciaTipoRobo = Funciones.CheckStr(dr["EVALV_INC_TIPO_ROBO"]),
                        Ip = Funciones.CheckStr(dr["EVALV_IP"]),
                        MontoPrima = Funciones.CheckStr(dr["EVALN_MONT_PRIM_RPTA"]),
                        NombreProd = Funciones.CheckStr(dr["EVALV_NOMB_PROD_RPTA"]),
                        NroCertif = Funciones.CheckStr(dr["EVALV_NRO_CERTF_RPTA"]),
                        NroDoc = Funciones.CheckStr(dr["EVALV_NRO_DOC_REQ"]),
                        NroSec = Funciones.CheckStr(dr["EVALV_NRO_SEC"]),
                        Resultado = Funciones.CheckStr(dr["EVALV_RESULTADO_RPTA"]),
                        TipoCliente = Funciones.CheckStr(dr["EVALV_TIPO_CLINT_REQ"]),
                        TipoDoc = Funciones.CheckStr(dr["EVALV_TIPO_DOC_REQ"]),
                        TipoOperacion = Funciones.CheckStr(dr["EVALV_TIPO_OPERACION"]),
                        UsrAplicacion = Funciones.CheckStr(dr["EVALV_USR_APLICACION"]),
                        UsrMod = Funciones.CheckStr(dr["EVALV_USR_MOD"])
                    });
                }

                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[4];
                strCodRespuesta = Funciones.CheckStr(parSalida1.Value);

                IDataParameter parSalida2;
                parSalida2 = (IDataParameter)objRequest.Parameters[5];
                strMsjRespuesta = Funciones.CheckStr(parSalida2.Value);
            }
            catch (Exception)
            {
                strCodRespuesta = "2";
                strMsjRespuesta = "ERROR AL EJECUTAR EL STORED PROCEDURE.";
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return lstBEPrimas;
        }

        public void ActualizarMontoCargoFijoServicioProteccionMovil(string strNroSec, string strSoplnCodigo, string strCodServicio, ref string strCodRespuesta, ref string strMsjRespuesta)
        {
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("K_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),                                                 
                                                 new DAABRequest.Parameter("K_SLPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_SERVV_CODIGO", DbType.String, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_COD_RESULTADO", DbType.String, ParameterDirection.Output),
				                                 new DAABRequest.Parameter("P_MSJ_RESULTADO", DbType.String, ParameterDirection.Output)
                                                };

            arrParam[0].Value = Funciones.CheckInt64(strNroSec);
            arrParam[1].Value = Funciones.CheckInt64(strSoplnCodigo);
            arrParam[2].Value = Funciones.CheckStr(strCodServicio);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_TRANS_ASURION + ".SISASU_ACT_SERV";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[3];
                strCodRespuesta = Funciones.CheckStr(parSalida1.Value);

                IDataParameter parSalida2;
                parSalida2 = (IDataParameter)objRequest.Parameters[4];
                strMsjRespuesta = Funciones.CheckStr(parSalida2.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }
    }
}
