using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using Claro.SISACT.Common;
using Claro.SISACT.Configuracion;
using Claro.SISACT.IData;
using Claro.SISACT.Entity;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class DAIntegracionDTHDatos
    {
        GeneradorLog objLog = new GeneradorLog("    DAIntegracionDTHDatos  ", null, null, "DATA_LOG");

        public ArrayList ListarCentrosPobladosDistrito(string strUbigeo)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("AC_UBIGEO", DbType.String, 10, ParameterDirection.Input),
												   new DAABRequest.Parameter("AO_CURSOR", DbType.Object, ParameterDirection.Output),
												   new DAABRequest.Parameter("AN_CODIGO_ERROR", DbType.Int64, ParameterDirection.Output),
												   new DAABRequest.Parameter("AC_MENSAJE_ERROR", DbType.String, ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strUbigeo;
            objLog.CrearArchivolog("[Inicio][ListarCentrosPobladosDistrito]", null, null);
            objLog.CrearArchivolog("[Entrada][strUbigeo]", strUbigeo.ToString(), null);

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strUbigeo);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_INTEGRACION_DTH + ".P_CENTRO_POBLADO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList Objretorno = new ArrayList();

            DataSet ds = new DataSet();

            try
            {
                ds = objRequest.Factory.ExecuteDataset(ref objRequest);
                DataTable objTblPlanRestric = ds.Tables[0];

                foreach (DataRow objItem in objTblPlanRestric.Rows)
                {
                    //gaa20120521
                    if (Funciones.CheckStr(objItem["COBERTURA"]) == "1")
                    {
                        //fin gaa20120521
                        BECentroPoblado objItemCP = new BECentroPoblado();
                        objItemCP.IDPOBLADO = Funciones.CheckInt64(objItem["IDPOBLADO"]);
                        objItemCP.CODCLASIFICACION = Funciones.CheckStr(objItem["CODCLASIFICACION"]);
                        objItemCP.NOMBRE = Funciones.CheckStr(objItem["NOMBRE"]);
                        objItemCP.COBERTURA = Funciones.CheckStr(objItem["COBERTURA"]);
                        objItemCP.IDUBIGEO = Funciones.CheckStr(objItem["IDUBIGEO"]);
                        Objretorno.Add(objItemCP);
                        //gaa20120521
                    }
                    //fin gaa20120521
                }

            }
            catch (Exception e)
            {
                objLog.CrearArchivolog("[ERROR][ListarCentrosPobladosDistrito]", null, e);
                throw e;
            }
            finally
            {
                ds.Dispose();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Salida][ListarCentrosPobladosDistrito]", null, null);
            return Objretorno;
        }

        public ArrayList ListarCentrosPobladosDistrito_LTE(string strUbigeo, int iCoberturaDTH, int iCoberturaLTE)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("AC_UBIGEO", DbType.String, 10, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("ac_cobertura_dth", DbType.Int32, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("ac_cobertura_lte", DbType.Int32, ParameterDirection.Input),
												   new DAABRequest.Parameter("AO_CURSOR", DbType.Object, ParameterDirection.Output),
												   new DAABRequest.Parameter("AN_CODIGO_ERROR", DbType.Int64, ParameterDirection.Output),
												   new DAABRequest.Parameter("AC_MENSAJE_ERROR", DbType.String, ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;
                arrParam[0].Value = strUbigeo;
                arrParam[1].Value = iCoberturaDTH;
                arrParam[2].Value = iCoberturaLTE;

                GeneradorLog objLog = new GeneradorLog(null, strUbigeo.ToString(),null,"DATA_LOG");
                objLog.CrearArchivolog("[Inicio][ListarCentrosPobladosDistrito_LTE]", null, null);
                objLog.CrearArchivolog("[UBIGEO]", strUbigeo.ToString(), null);
                objLog.CrearArchivolog("[COBERTURA_DTH]", iCoberturaDTH.ToString(), null);
                objLog.CrearArchivolog("[COBERTURA_LTE]", iCoberturaLTE.ToString(), null);

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strUbigeo);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_INTEGRACION_DTH + ".P_CENTRO_POBLADO_LTE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList Objretorno = new ArrayList();

            DataSet ds = new DataSet();
           

            try
            {
                ds = objRequest.Factory.ExecuteDataset(ref objRequest);
                DataTable objTblPlanRestric = ds.Tables[0];
             
                foreach (DataRow objItem in objTblPlanRestric.Rows)
                {
                    //gaa20120521
                    //if (Funciones.CheckStr(objItem["COBERTURA"]) == "1" )
              
                    if (Funciones.CheckStr(objItem["COBERTURA"]) == "1" || Funciones.CheckStr(objItem["COBERTURA_LTE"]) == "1") 
                    {
                        //fin gaa20120521
                   
                        BECentroPoblado objItemCP = new BECentroPoblado();
                        objItemCP.IDPOBLADO = Funciones.CheckInt64(objItem["IDPOBLADO"]);
                        objItemCP.CODCLASIFICACION = Funciones.CheckStr(objItem["CODCLASIFICACION"]);
                        objItemCP.NOMBRE = Funciones.CheckStr(objItem["NOMBRE"]);
                        objItemCP.COBERTURA = Funciones.CheckStr(objItem["COBERTURA"]);
                        objItemCP.IDUBIGEO = Funciones.CheckStr(objItem["IDUBIGEO"]);
                        objItemCP.COBERTURA_LTE = Funciones.CheckStr(objItem["COBERTURA_LTE"]);
                        Objretorno.Add(objItemCP);
                        //gaa20120521
                    }
                    //fin gaa20120521
                }

            }
            catch (Exception e)
            {
                objLog.CrearArchivolog("[ERROR][ListarCentrosPobladosDistrito]", null, e);
                 throw e;
            }
            finally
            {
                ds.Dispose();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
                objLog.CrearArchivolog("[Fin][ListarCentrosPobladosDistrito_LTE]", null, null);
            }
            return Objretorno;
        }

        public ArrayList ListarCentrosPoblados(string CentroPoblado, ref int codError)
        {
            DAABRequest.Parameter[] arrParam = {
                                                   new DAABRequest.Parameter("p_ubigeo", DbType.String,500,ParameterDirection.Input),
												   new DAABRequest.Parameter("cur_salida_o", DbType.Object,ParameterDirection.Output),
												   new DAABRequest.Parameter("an_coderror_o", DbType.Int32,ParameterDirection.Output),
												   new DAABRequest.Parameter("ac_mensaje_o", DbType.String,250,ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = CentroPoblado;
            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_INT_SISACT_CONSULTA + ".p_consulta_centro_poblado";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList Objretorno = new ArrayList();

            DataSet ds = new DataSet();

            try
            {
                ds = objRequest.Factory.ExecuteDataset(ref objRequest);
                IDataParameter pSalida;
                pSalida = (IDataParameter)objRequest.Parameters[2];
                codError = Funciones.CheckInt(pSalida.Value);
                DataTable objTblPlanRestric = ds.Tables[0];

                foreach (DataRow objItem in objTblPlanRestric.Rows)
                {
                    BEDireccionPlano objItemCP = new BEDireccionPlano();
                    objItemCP.IdPlano = Funciones.CheckStr(objItem["IdPlano"]);
                    objItemCP.Descripcion = Funciones.CheckStr(objItem["Descripcion"]);
                    objItemCP.Distrito_Desc = Funciones.CheckStr(objItem["Distrito_Desc"]);
                    objItemCP.Centro_Poblado = Funciones.CheckStr(objItem["Centro_Poblado"]);
                    objItemCP.FlagVOD = Funciones.CheckStr(objItem["FLG_VOD"]);

                    Objretorno.Add(objItemCP);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                ds.Dispose();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return Objretorno;
        }
    }
}
