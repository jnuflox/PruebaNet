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
    public class DASolicitud
    {
        GeneradorLog objLog = new GeneradorLog("    DASolicitud  ", null, null, "DATA_LOG");
        public DataTable ObtenerSolicitudPersona(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
            };
            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS + ".SP_DETALLE_SOL_PERSONA"; //IMP SD_866068
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
        }

        //PROY-24740
        public List<BESolicitudEmpresa> ObtenerHistoricoPersona(Int64 nroSEC, string tipoDocumento, string nroDocumento, DateTime fechaInicio, DateTime fechaFin, string estado)
        {
            DAABRequest.Parameter[] arrParam = {	
			    new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String, 2, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String, 16, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_FECHA_INICIO", DbType.DateTime, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_FECHA_FIN", DbType.DateTime, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_ESTAC_CODIGO", DbType.String, 2, ParameterDirection.Input),
			    new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
			};

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (nroSEC > 0) arrParam[0].Value = nroSEC;
            ++i; if (!string.IsNullOrEmpty(tipoDocumento)) arrParam[i].Value = tipoDocumento;
            ++i; if (!string.IsNullOrEmpty(nroDocumento)) arrParam[i].Value = nroDocumento.PadLeft(16, '0');
            ++i; if (fechaInicio != new DateTime(1, 1, 1)) arrParam[i].Value = fechaInicio;
            ++i; if (fechaFin != new DateTime(1, 1, 1)) arrParam[i].Value = fechaFin;
            ++i; if (estado != "00") arrParam[i].Value = estado;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACS_CON_HISTORICO_SOL_CONS";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BESolicitudEmpresa> objLista = new List<BESolicitudEmpresa>();
            BESolicitudEmpresa objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BESolicitudEmpresa();
                    objItem.SOLIN_CODIGO = Funciones.CheckInt(dr["SOLIN_CODIGO"]);
                    objItem.ESTAV_DESCRIPCION = Funciones.CheckStr(dr["ESTAV_DESCRIPCION"]);
                    objItem.TDOCC_CODIGO = Funciones.CheckStr(dr["TDOCC_CODIGO"]);
                    objItem.TDOCV_DESCRIPCION = Funciones.CheckStr(dr["TDOCV_DESCRIPCION"]);
                    objItem.NUM_DOCU = Funciones.FormatoNroDocumento(Funciones.CheckStr(dr["TDOCC_CODIGO"]), Funciones.CheckStr(dr["CLIEC_NUM_DOC"]));
                    objItem.CLIEV_NOMBRE = Funciones.CheckStr(dr["CLIEV_NOMBRE"]);
                    objItem.CLIEV_APE_PAT = Funciones.CheckStr(dr["CLIEV_APE_PAT"]);
                    objItem.CLIEV_APE_MAT = Funciones.CheckStr(dr["CLIEV_APE_MAT"]);
                    objItem.CLIEV_RAZ_SOC = Funciones.CheckStr(dr["CLIEV_RAZ_SOC"]);

                    if (!string.IsNullOrEmpty(objItem.CLIEV_NOMBRE) && !string.IsNullOrEmpty(objItem.CLIEV_APE_PAT))
                        objItem.RAZON_SOCIAL = string.Format("{0} {1} {2}", objItem.CLIEV_NOMBRE, objItem.CLIEV_APE_PAT, objItem.CLIEV_APE_MAT);

                    objItem.FECHA_REG_APROBACION = Funciones.CheckStr(dr["SOLID_FEC_APR"]);
                    objItem.OVENV_DESCRIPCION = Funciones.CheckStr(dr["OVENV_DESCRIPCION"]);
                    objItem.TCARV_DESCRIPCION = Funciones.CheckStr(dr["TCARV_DESCRIPCION"]);
                    objItem.SOLIN_IMP_DG_MAN = Funciones.CheckDbl(dr["SOLIN_IMP_DG_MAN"]);
                    objItem.SOLIN_NUM_CAR_FIJ = Funciones.CheckDbl(dr["SOLIN_NUM_CAR_FIJ"]);
                    objItem.TDOCC_CODIGO = Funciones.CheckStr(dr["TDOCC_CODIGO"]);
                    objItem.ESTAC_CODIGO = Funciones.CheckStr(dr["ESTAC_CODIGO"]);
                    objItem.SOLIC_FLA_TER = Funciones.CheckStr(dr["SOLIC_FLA_TER"]);
                    objItem.MOTIVO_RECHAZO = Funciones.CheckStr(dr["SOLIV_MOTIVO_RECHAZO"]);
                    objItem.CANTIDAD_LINEAS = Funciones.CheckInt(dr["SOLIN_CAN_LIN"]);
                    objItem.SOLIC_FLAG_REINGRESO = Funciones.CheckStr(dr["SOLIC_FLAG_REINGRESO"]);
                    objItem.ACTIVADOR_ID = Funciones.CheckStr(dr["SOLIC_COD_APROB"]);
                    objItem.SOLID_FEC_REG = Funciones.CheckDate(dr["SOLID_FEC_REG"]);
                    objItem.SOLIN_SUM_CAR_CON = Funciones.CheckDbl(dr["SOLIN_SUM_CAR_CON"]);
                    objItem.SOLIV_NUM_CON = Funciones.CheckStr(dr["SOLIV_NUM_CON"]);
                    objItem.RIESGO = Funciones.CheckStr(dr["SOLIV_RES_EXP_CON"]);
                    objItem.PRDV_DESCRIPCION = Funciones.CheckStr(dr["PRDV_DESCRIPCION"]);
                    objItem.PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    objItem.TCESC_DESCRIPCION = Funciones.CheckStr(dr["TCESC_DESCRIPCION"]);

                    objLista.Add(objItem);
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        //PROY-24740
        public List<BESolicitudEmpresa> ObtenerHistoricoEmpresa(Int64 nroSEC, string tipoDocumento, string nroDocumento, DateTime fechaInicio, DateTime fechaFin, string estado)
        {
            DAABRequest.Parameter[] arrParam = {	
			    new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String, 2, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String, 16, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_FECHA_INICIO", DbType.DateTime, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_FECHA_FIN", DbType.DateTime, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_ESTAC_CODIGO", DbType.String, 2, ParameterDirection.Input),
			    new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
			};

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (nroSEC > 0) arrParam[0].Value = nroSEC;
            ++i; if (!string.IsNullOrEmpty(tipoDocumento)) arrParam[i].Value = tipoDocumento;
            ++i; if (!string.IsNullOrEmpty(nroDocumento)) arrParam[i].Value = nroDocumento;
            ++i; if (fechaInicio != new DateTime(1, 1, 1)) arrParam[i].Value = fechaInicio;
            ++i; if (fechaFin != new DateTime(1, 1, 1)) arrParam[i].Value = fechaFin;
            ++i; if (estado != "00") arrParam[i].Value = estado;

            objLog.CrearArchivolog("[INICIO][ObtenerHistoricoEmpresa]", null, null);
            objLog.CrearArchivolog("[Entrada][nroSEC]", nroSEC.ToString(), null);
            objLog.CrearArchivolog("[Entrada][tipoDocumento]", tipoDocumento.ToString(), null);
            objLog.CrearArchivolog("[Entrada][nroDocumento]", nroDocumento.ToString(), null);
            objLog.CrearArchivolog("[Entrada][fechaInicio]", fechaInicio.ToString(), null);
            objLog.CrearArchivolog("[Entrada][fechaFin]", fechaFin.ToString(), null);
            objLog.CrearArchivolog("[Entrada][estado]", estado.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACS_CON_HISTORICO_SOL";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BESolicitudEmpresa> objLista = new List<BESolicitudEmpresa>();
            BESolicitudEmpresa objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BESolicitudEmpresa();
                    objItem.SOLIN_CODIGO = Funciones.CheckInt64(dr["SOLIN_CODIGO"]);
                    objItem.ESTAV_DESCRIPCION = Funciones.CheckStr(dr["ESTAV_DESCRIPCION"]);
                    objItem.TDOCV_DESCRIPCION = Funciones.CheckStr(dr["TDOCV_DESCRIPCION"]);
                    objItem.NUM_DOCU = Funciones.CheckStr(dr["CLIEC_NUM_DOC"]);
                    objItem.RAZON_SOCIAL = Funciones.CheckStr(dr["CLIEV_RAZ_SOC"]);
                    objItem.FECHA_APROBACION = Funciones.CheckDate(dr["SOLID_FEC_APR"]);
                    objItem.OVENV_DESCRIPCION = Funciones.CheckStr(dr["OVENV_DESCRIPCION"]);
                    objItem.TCARV_DESCRIPCION = Funciones.CheckStr(dr["TCARV_DESCRIPCION"]);
                    objItem.SOLIN_IMP_DG_MAN = Funciones.CheckDbl(dr["SOLIN_IMP_DG_MAN"]);
                    objItem.SOLIN_NUM_CAR_FIJ = Funciones.CheckDbl(dr["SOLIN_NUM_CAR_FIJ"]);
                    objItem.TDOCC_CODIGO = Funciones.CheckStr(dr["TDOCC_CODIGO"]);
                    objItem.ESTAC_CODIGO = Funciones.CheckStr(dr["ESTAC_CODIGO"]);
                    objItem.SOLIC_FLA_TER = Funciones.CheckStr(dr["SOLIC_FLA_TER"]);
                    objItem.MOTIVO_RECHAZO = Funciones.CheckStr(dr["SOLIV_MOTIVO_RECHAZO"]);
                    objItem.CANTIDAD_LINEAS = Funciones.CheckInt(dr["SOLIN_CAN_LIN"]);
                    objItem.SOLIC_FLAG_REINGRESO = Funciones.CheckStr(dr["SOLIC_FLAG_REINGRESO"]);
                    objItem.ACTIVADOR_ID = Funciones.CheckStr(dr["SOLIC_COD_APROB"]);
                    objItem.SOLID_FEC_REG = Funciones.CheckDate(dr["SOLID_FEC_REG"]);
                    objItem.TCESC_DESCRIPCION = Funciones.CheckStr(dr["TCESC_DESCRIPCION"]);
                    objItem.DPCHN_CODIGO = Funciones.CheckInt(dr["DPCHN_CODIGO"]);
                    objItem.PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ObtenerHistoricoEmpresa]", null, ex);
                throw ;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[SALIDA][ObtenerHistoricoEmpresa]", null, null);
            return objLista;
        }

        public List<BEEstado> ObtenerLogEstados(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {	
			    new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
			    new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
		    };
            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACS_CON_ESTADOS";
            objRequest.Parameters.AddRange(arrParam);

            List<BEEstado> objFilas = new List<BEEstado>();
            BEEstado objItem;
            try
            {
                DataTable dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEEstado();
                    objItem.NroSEC = Funciones.CheckInt64(dr["SOLIN_CODIGO"]);
                    objItem.ESTAC_CODIGO = Funciones.CheckStr(dr["ESTAC_CODIGO"]);
                    objItem.HISEV_USUA_REG = Funciones.CheckStr(dr["HISEV_USUA_REG"]);
                    objItem.HISED_FEC_REG = Funciones.CheckDate(dr["HISED_FEC_REG"]);
                    objItem.HISEV_COMENTARIO = Funciones.CheckStr(dr["HISEV_COMENTARIO"]);
                    objItem.HISEV_FLAG_ARCHIVO = Funciones.CheckStr(dr["HISEV_FLAG_ARCHIVO"]);
                    objItem.ESTAV_DESCRIPCION = Funciones.CheckStr(dr["ESTAV_DESCRIPCION"]);
                    objFilas.Add(objItem);
                }
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
            return objFilas;
        }

        public List<BEEstado> ObtenerHistoricoEstadosSOT(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {	
				new DAABRequest.Parameter("an_numsec", DbType.String, 15, ParameterDirection.Input),		   												   
				new DAABRequest.Parameter("cur_hist_sot_o", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("an_coderror_o", DbType.Int32, ParameterDirection.Output),
				new DAABRequest.Parameter("ac_mensaje_o", DbType.String, 500, ParameterDirection.Output)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = Convert.ToString(nroSEC);

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_INT_SISACT_CONSULTA + ".p_consulta_historial_sot";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEEstado> objFilas = new List<BEEstado>();
            BEEstado objItem;
            try
            {
                DataTable dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEEstado();
                    objItem.NroSEC = nroSEC;
                    objItem.NroSOT = Funciones.CheckInt64(dr["SOT"]);
                    objItem.ESTAV_DESCRIPCION = Funciones.CheckStr(dr["DESCRIPCION"]);
                    objItem.HISED_FEC_REG = Funciones.CheckDate(dr["FECHA"]);
                    objItem.HISEV_USUA_REG = Funciones.CheckStr(dr["CODUSU"]);
                    objItem.HISEV_COMENTARIO = Funciones.CheckStr(dr["OBSERVACION"]);
                    objFilas.Add(objItem);

                }
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
            return objFilas;

        }

        public List<BEArchivo> ObtenerArchivos(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
			};
            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SECSS_CON_ARCHIVO";
            objRequest.Parameters.AddRange(arrParam);

            BEArchivo objItem = null;
            List<BEArchivo> objFilas = new List<BEArchivo>();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEArchivo();
                    objItem.ARCH_ID = Funciones.CheckInt64(dr["ARCHN_CODIGO"]);
                    objItem.ARCH_NOMBRE = Funciones.CheckStr(dr["ARCHV_NOM_ARC"]);
                    objItem.ARCH_RUTA = Funciones.CheckStr(dr["ARCHV_RUT_ARC"]).Replace("\\", "\\\\");
                    objFilas.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Clear();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objFilas;
        }

        public bool AsignarUsuarioSEC(Int64 nroSEC, string strUsuario, string strNroDocumento, string strOrigen)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("K_RESULTADO", DbType.Int64, ParameterDirection.Output),				
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),												   
				new DAABRequest.Parameter("P_SOLIC_COD_APROB", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String,16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_BANDEJA", DbType.String, 1, ParameterDirection.Input)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = nroSEC;
            ++i; arrParam[i].Value = strUsuario;
            ++i; arrParam[i].Value = strNroDocumento;
            ++i; if (!string.IsNullOrEmpty(strOrigen)) arrParam[i].Value = strOrigen;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACU_ASIGNA_ACTIV_DESPACH2";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
        }

        public bool LiberarSEC(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input) };
            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SP_LIBERAR_SEC_POOL";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
        }

        public bool RechazarSEC(Int64 nroSEC, string strComentarioPdv, string strComentarioEval, string strAprobador, string strFlgReingreso)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_COM_PUN_VEN", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_COM_EVAL", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_COD_APROB", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_REINGRESO_SEC", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_COST_INST", DbType.Double, ParameterDirection.Input)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;
            i++; if (!string.IsNullOrEmpty(strComentarioPdv)) arrParam[i].Value = strComentarioPdv;
            i++; if (!string.IsNullOrEmpty(strComentarioEval)) arrParam[i].Value = strComentarioEval;
            i++; arrParam[i].Value = strAprobador;
            i++; if (!string.IsNullOrEmpty(strFlgReingreso)) arrParam[i].Value = strFlgReingreso;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SP_RECHAZAR_CREDITOS";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                return true;
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

        public bool RechazarPoolSEC(Int64 nroSEC, string strAprobador, string strMotivoId, string strObservacion, string strCodEstado)
        {
            DAABRequest.Parameter[] arrParam = {					
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_USUARIO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_MOTIVO_ID", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OBS", DbType.String, 300, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESTAC_CODIGO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PROCESO", DbType.String, 500, ParameterDirection.Output)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = strAprobador;
            i++; if (!string.IsNullOrEmpty(strMotivoId)) arrParam[i].Value = strMotivoId;
            i++; arrParam[i].Value = strObservacion;
            i++; arrParam[i].Value = strCodEstado;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACU_RECHAZO_SOLICITUD_CONS";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
        }

        public bool GrabarLogHistorico(Int64 nroSEC, string strEstado, string strUsuario)
        {
            objLog.CrearArchivolog("[ENTRADA][GrabarLogHistorico] - Estado: " + strEstado, null, null);
            DAABRequest.Parameter[] arrParam = {												   
				new DAABRequest.Parameter("P_SOLIN_CODIGO" ,DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESTADO" ,DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_USU_CRE" ,DbType.String, 10, ParameterDirection.Input)												   
			};
            arrParam[0].Value = nroSEC;
            arrParam[1].Value = strEstado;
            arrParam[2].Value = strUsuario;

            objLog.CrearArchivolog("[Parametro nroSEC][GrabarLogHistorico] - Estado: " + strEstado, Funciones.CheckStr(nroSEC), null);
            objLog.CrearArchivolog("[Parametro strEstado][GrabarLogHistorico] - Estado: " + strEstado, Funciones.CheckStr(strEstado), null);
            objLog.CrearArchivolog("[Parametro strUsuario][GrabarLogHistorico] - Estado: " + strEstado, Funciones.CheckStr(strUsuario), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACTI_INS_LOG";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;
            objLog.CrearArchivolog("[PACKAGE] - Estado: " + strEstado, BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACTI_INS_LOG", null);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                return true;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[Exception][GrabarLogHistorico] - Estado: " + strEstado, ex.Message, null);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("[SALIDA][GrabarLogHistorico] - Estado: " + strEstado, null, null);
                objRequest.Factory.Dispose();
            }
        }

        // Pool Creditos
        public void ObtenerUsuarioAsignadoSEC(Int64 nroSEC, string flagBandeja, ref string strCodUsuAsignado, ref string strUsuAsignado)
        {
            DAABRequest.Parameter[] arrParam = {
			    new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
			    new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
		    };
            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACS_DET_VALIDACION";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                DataTable dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    if (flagBandeja == "A")
                    {
                        strCodUsuAsignado = Funciones.CheckStr(dr["SOLIC_COD_APROB"]);
                        strUsuAsignado = Funciones.CheckStr(dr["NOMBRE_APROB"]);
                    }
                    else if (flagBandeja == "E")
                    {
                        strCodUsuAsignado = Funciones.CheckStr(dr["SOLIC_COD_EVAL"]);
                        strUsuAsignado = Funciones.CheckStr(dr["NOMBRE_EVALUADOR"]);
                    }
                    else
                    {
                        strCodUsuAsignado = Funciones.CheckStr(dr["SOLIV_COD_DESPACHADOR"]);
                        strUsuAsignado = Funciones.CheckStr(dr["NOMBRE_DESPACHADOR"]);
                    }
                }
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

        public Int64 AsignarSECAutomatica(string strUsuario)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_USUARIO", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Output)
            };

            arrParam[0].Value = strUsuario;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SP_ASIGNAR_SEC_POOL";
            objRequest.Parameters.AddRange(arrParam);

            Int64 nroSEC;
            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter p1;
                p1 = (IDataParameter)objRequest.Parameters[1];
                nroSEC = Funciones.CheckInt64(p1.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }

        public DataTable ObtenerPoolEvaluadorPersona(string estado, DateTime fecha_inicio, DateTime fecha_fin)
        {
            DAABRequest.Parameter[] arrParam = {				
				new DAABRequest.Parameter("P_ESTAC_CODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FECHA_INICIO", DbType.DateTime, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FECHA_FIN", DbType.DateTime, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = estado;
            i++; arrParam[i].Value = fecha_inicio;
            i++; arrParam[i].Value = fecha_fin;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACTS_CON_POOL_EVAL_LIB_CON";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
        }

        public int ObtenerNroSECPendiente(string tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam =
			{
				new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String,2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String,16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_VIGENCIA", DbType.Int64, ParameterDirection.Input),					
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
			};
            arrParam[0].Value = tipoDocumento;
            arrParam[1].Value = Funciones.FormatoNroDocumentoBD(tipoDocumento, nroDocumento);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_SOLICITUD_CONS + ".SECSS_CANTIDAD_SOL_PENDIENTE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            int intCantidad = 0;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    intCantidad = Funciones.CheckInt(dr["CANTIDAD"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return intCantidad;
        }

        public DataTable ObtenerSECPendiente(string tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam =
			{
				new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String, 16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
			};
            arrParam[0].Value = tipoDocumento;
            arrParam[1].Value = Funciones.FormatoNroDocumentoBD(tipoDocumento, nroDocumento);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_SOL_PENDIENTE";
            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }

        public Int64 ObtenerSECPendienteVentaSinPago(string tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String, 16, ParameterDirection.Input)
			};
            arrParam[1].Value = tipoDocumento;
            arrParam[2].Value = Funciones.FormatoNroDocumentoBD(tipoDocumento, nroDocumento);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_SOL_PENDIENTE_PAGO";
            objRequest.Parameters.AddRange(arrParam);

            Int64 nroSEC = 0;
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        nroSEC = Funciones.CheckInt64(dr["SOLIN_GRUPO_SEC"]);
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Clear();
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }

        //PROY-24740
        public BESolicitudEmpresa ObtenerSolicitudEmpresa(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("K_CUR_REP_LEGAL", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("K_CUR_DIRECCION", DbType.Object, ParameterDirection.Output)
			};
            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            string[] sTab = { "solicitud", "representante", "direccion" };
            objRequest.TableNames = sTab;
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SECSS_DET_SOL_CORP";
            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            BESolicitudEmpresa item = new BESolicitudEmpresa();
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                
                while(dr.Read())
                {
                    item.SOLIN_CODIGO = Funciones.CheckInt64(dr["SOLIN_CODIGO"]);
                    item.TPROC_CODIGO = Funciones.CheckStr(dr["TPROC_CODIGO"]);
                    item.OVENV_DESCRIPCION = Funciones.CheckStr(dr["OVENV_DESCRIPCION"]);
                    item.OVENC_CODIGO = Funciones.CheckStr(dr["OVENC_CODIGO"]);
                    item.CANAC_CODIGO = Funciones.CheckStr(dr["CANAC_CODIGO"]);
                    item.ESTAV_DESCRIPCION = Funciones.CheckStr(dr["ESTAV_DESCRIPCION"]);
                    item.SOLID_FEC_REG = Funciones.CheckDate(dr["SOLID_FEC_REG"]);
                    item.CLIEV_NOMBRE = Funciones.CheckStr(dr["CLIEV_NOMBRE"]);
                    item.CLIEV_APE_PAT = Funciones.CheckStr(dr["CLIEV_APE_PAT"]);
                    item.CLIEV_APE_MAT = Funciones.CheckStr(dr["CLIEV_APE_MAT"]);
                    item.CLIEV_RAZ_SOC = Funciones.CheckStr(dr["CLIEV_RAZ_SOC"]);
                    item.TDOCC_CODIGO = Funciones.CheckStr(dr["TDOCC_CODIGO"]);
                    item.ESTAC_CODIGO = Funciones.CheckStr(dr["ESTAC_CODIGO"]);
                    item.SOLIC_FLA_TER = Funciones.CheckStr(dr["SOLIC_FLA_TER"]);
                    item.TEVAC_CODIGO = Funciones.CheckStr(dr["TEVAC_CODIGO"]);
                    item.SOLIC_EXI_BSC_FIN = Funciones.CheckStr(dr["SOLIC_EXI_BSC_FIN"]);
                    item.CLIEC_NUM_DOC = Funciones.FormatoNroDocumento(Funciones.CheckStr(dr["TDOCC_CODIGO"]), Funciones.CheckStr(dr["CLIEC_NUM_DOC"]));
                    item.CLIEN_CAP_SOC = Funciones.CheckStr(dr["CLIEN_CAP_SOC"]);
                    item.FPAGC_CODIGO = Funciones.CheckStr(dr["FPAGC_CODIGO"]);
                    item.SOLIN_CAN_LIN = Funciones.CheckInt(dr["SOLIN_CAN_LIN"]);
                    item.RFINC_CODIGO = Funciones.CheckStr(dr["RFINC_CODIGO"]);
                    item.RFINV_DESCRIPCION = Funciones.CheckStr(dr["RFINV_DESCRIPCION"]);
                    item.TCARV_DESCRIPCION = Funciones.CheckStr(dr["TCARV_DESCRIPCION"]);
                    item.SOLIN_IMP_DG_MAN = Funciones.CheckDbl(dr["SOLIN_IMP_DG_MAN"]);
                    item.TDOCV_DESCRIPCION = Funciones.CheckStr(dr["TDOCV_DESCRIPCION"]);
                    item.CCLIC_CODIGO = Funciones.CheckStr(dr["CCLIC_CODIGO"]);
                    item.FLEXN_CODIGO = Funciones.CheckInt(dr["FLEXN_CODIGO"]);
                    item.FLEXV_DESCRIPCION = Funciones.CheckStr(dr["FLEXV_DESCRIPCION"]);
                    item.AUTORIZADOR = Funciones.CheckStr(dr["AUTORIZADOR"]);
                    item.USUAN_CODIGO = Funciones.CheckInt64(dr["USUAN_CODIGO"]);
                    item.CLASC_CODIGO = Funciones.CheckStr(dr["CLASC_CODIGO"]);
                    item.CLASV_DESCRIPCION = Funciones.CheckStr(dr["CLASV_DESCRIPCION"]);
                    item.OPERV_DESCRIPCION = Funciones.CheckStr(dr["OPERV_DESCRIPCION"]);
                    item.SOLIN_CAN_LIN_EXCOMP = Funciones.CheckInt(dr["SOLIN_CAN_LIN_EXCOMP"]);
                    item.TRIEC_CODIGO = Funciones.CheckStr(dr["TRIEC_CODIGO"]);

                    item.SOLIN_NUM_CAR_FIJ = Funciones.CheckDbl(dr["SOLIN_NUM_CAR_FIJ"]);
                    item.CANTIDAD_CARGOS_FIJOS = Funciones.CheckDbl(dr["SOLIN_NUM_CAR_FIJ"]);

                    item.VENDEDOR_ID = Funciones.CheckInt64(dr["SOLIN_VENDEDOR"]);
                    item.DISTRIBUIDOR_ID = Funciones.CheckStr(dr["DISTC_CODIGO"]);

                    item.CONSULTOR_ID = Funciones.CheckInt64(dr["CONSULTOR_CODIGO"]);
                    item.CONSULTOR_DES = Funciones.CheckStr(dr["CONSULTOR_NOMBRE"]);

                    item.TOFIC_CODIGO = Funciones.CheckStr(dr["TOFIC_CODIGO"]);
                    item.TOFIV_DESCRIPCION = Funciones.CheckStr(dr["TOFIV_DESCRIPCION"]);
                    item.SOLIV_COM_PUN_VEN = Funciones.CheckStr(dr["SOLIV_COM_PUN_VEN"]);
                    item.SOLIN_DEUDA_CLIENTE = Funciones.CheckDbl(dr["SOLIN_DEUDA_CLIENTE"]);
                    item.SOLIN_LINEA_CLIENTE = Funciones.CheckInt(dr["SOLIN_LINEA_CLIENTE"]);
                    item.SOLIN_ANTIGUEDAD = Funciones.CheckDbl(dr["SOLIN_ANTIGUEDAD"]);
                    item.SOLIC_TIP_CAR_MAN = Funciones.CheckStr(dr["SOLIC_TIP_CAR_MAN"]);
                    item.OPERC_CODIGO = Funciones.CheckStr(dr["OPERC_CODIGO"]);
                    item.CLIEN_PROM_VEN = Funciones.CheckDbl(dr["CLIEN_CAP_SOC"]);
                    item.SOLID_FEC_DEPOSITO = Funciones.CheckDate(dr["SOLID_FEC_DEPOSITO"]);
                    item.SOLIV_COD_VOUCHER = Funciones.CheckStr(dr["SOLIV_COD_VOUCHER"]);
                    item.ACTIVADOR_ID = Funciones.CheckStr(dr["SOLIC_COD_APROB"]);
                    item.MOTIVO_RECHAZO = Funciones.CheckStr(dr["SOLIV_MOTIVO_RECHAZO"]);
                    item.SOLIN_CODIGO_PADRE = Funciones.CheckInt64(dr["SOLIN_CODIGO_PADRE"]);
                    item.ALMAC_CODIGO = Funciones.CheckStr(dr["ALMAC_CODIGO"]);
                    item.ALMAV_DESCRIPCION = Funciones.CheckStr(dr["ALMAV_DESCRIPCION"]);
                    item.SOLIV_COM_DESPACHO = Funciones.CheckStr(dr["SOLIV_COM_DESPACHO"]);
                    item.SOLID_FEC_ACTIV = Funciones.CheckDate(dr["SOLID_FEC_ACTIV"]);
                    item.SOLIV_NUM_OPE_CON = Funciones.CheckStr(dr["SOLIV_NUM_OPE_CON"]);
                    item.SOLIN_NUM_CAR_FIJ_LINEA = Funciones.CheckDbl(dr["SOLIN_NUM_CAR_FIJ_LINEA"]);
                    item.SOLIN_ANTIGUEDAD_CLIENTE = Funciones.CheckDbl(dr["SOLIN_ANTIGUEDAD_CLIENTE"]);
                    item.SOLIC_FLAG_EMPRESA_TRAFICO = Funciones.CheckStr(dr["SOLIC_FLAG_EMPRESA_TRAFICO"]);

                    item.TIPO_OPERACION_DES = Funciones.CheckStr(dr["TOPEV_DESCRIPCION"]);
                    item.NRO_CONTRATO = Funciones.CheckStr(dr["SOLIV_NUM_ACU"]);
                    item.FLAG_RESPONSABLE_PUNTO_VENTA = Funciones.CheckStr(dr["SOLIV_FLA_VER_RES"]);

                    item.SOLIN_SUM_CAR_FIN = Funciones.CheckDbl(dr["SOLIN_SUM_CAR_FIN"]);
                    item.SOLID_FEC_OBS = Funciones.CheckDate(dr["SOLID_FEC_OBS"]);
                    item.SOLIN_CAR_FIJO_ACTUAL = Funciones.CheckDbl(dr["SOLIN_CAR_FIJO_ACTUAL"]);
                    item.SOLIN_NUM_CAR_FIJ_ADI = Funciones.CheckDbl(dr["SOLIN_NUM_CAR_FIJ_ADI"]);
                    item.SOLIN_SUBSIDIO_TOTAL = Funciones.CheckDbl(dr["SOLIN_SUBSIDIO_TOTAL"]);
                    item.SOLIN_NUM_RA = Funciones.CheckDbl(dr["SOLIN_NUM_RA"]);
                    item.SOLIN_IMP_RA = Funciones.CheckDbl(dr["SOLIN_IMP_RA"]);
                    item.SOLIN_NUM_CAR_FIJ_SIS = Funciones.CheckDbl(dr["SOLIN_NUM_CAR_FIJ_SIS"]);
                    item.SOLIV_COM_EVALUADOR = Funciones.CheckStr(dr["SOLIV_COM_EVAL"]);

                    item.NRO_LINEAS_RECURRENTE_ACTUAL = Funciones.CheckInt(dr["LINEAS_RECURRENTE_ACTUAL"]);
                    item.NRO_LINEAS_MAYOR_N_DIAS = Funciones.CheckInt(dr["LINEAS_MAYOR_N_DIAS"]);
                    item.NRO_LINEAS_MENOR_N_DIAS = Funciones.CheckInt(dr["LINEAS_MENOR_N_DIAS"]);
                    item.DIAS_LINEAS_RECURRENTE = Funciones.CheckInt(dr["PARAM_DIAS_RECURRENTE"]);
                    item.TIPO_RIESGO_DES = Funciones.CheckStr(dr["TIPO_RIESGO_DES"]);
                    item.SOLIV_COM_DG = Funciones.CheckStr(dr["SOLIV_COM_DG"]);
                    item.SOLIV_FLAG_ENVIO = Funciones.CheckStr(dr["SOLIV_FLAG_ENVIO"]);

                    item.DISTRIBUIDOR_DES = Funciones.CheckStr(dr["DISTRIBUIDOR_DES"]);
                    item.SOLIN_BOLSA_REF = Funciones.CheckDbl(dr["SOLIN_BOLSA_REF"]);

                    item.CAMPN_CODIGO = Funciones.CheckInt64(dr["CAMPN_CODIGO"]);

                    //E75785
                    item.CAMPV_DESCRIPCION = Funciones.CheckStr(dr["CAMPV_DESCRIPCION"]);

                    item.ANEXO2 = Funciones.CheckStr(dr["ANEXO_2"]);
                    //T12639 - Datos para la portabilidad 
                    item.FLAG_PORTABILIDAD = Funciones.CheckStr(dr["FLAG_PORTABILIDAD"]);
                    item.PORT_OPER_CED = Funciones.CheckInt(dr["PORT_OPER_CED"]);
                    item.PORT_CARGO_FIJO_OPE_CED = Funciones.CheckStr(dr["PORT_CARGO_FIJO_OPE_CED"]);
                    item.TLINC_CODIGO_CEDENTE = Funciones.CheckStr(dr["TLINC_CODIGO_CEDENTE"]);
                    item.PORT_SOLIN_NRO_FORMATO = Funciones.CheckStr(dr["PORT_SOLIN_NRO_FORMATO"]);
                    item.SOLIN_CAN_LIN_REG = Funciones.CheckInt(dr["SOLIN_CAN_LIN_REG"]);

                    //INICIO - E75688
                    item.FLAG_CORREO = Funciones.CheckStr(dr["CLIEV_FLAG_CORREO"]);
                    item.SOLIV_CORREO = Funciones.CheckStr(dr["CLIEV_CORREO"]);
                    item.SOLIV_TELCONF_SMS = Funciones.CheckStr(dr["CLIEV_TEL_SMS"]);
                    //FIN

                    item.SOLIN_LINEA_CREDITO_CALC = Funciones.CheckDbl(dr["SOLIC_LINEA_CREDITO_CALC"]);
                    item.DPCHV_DESCRIPCION = Funciones.CheckStr(dr["DPCHV_DESCRIPCION"]); //maryta
                    item.DPCHN_CODIGO = Funciones.CheckInt(dr["DPCHN_CODIGO"]);
                    item.RGLPC_PODERES = Funciones.CheckStr(dr["RGLPC_PODERES"]);
                    item.TPREC_CODIGO = Funciones.CheckStr(dr["TPREC_CODIGO"]);//JAR
                    item.PACUV_DESCRIPCION = Funciones.CheckStr(dr["PACUV_DESCRIPCION"]);//JAR
                    item.CLIEV_CODIGO_SAP = Funciones.CheckStr(dr["CLIEV_CODIGO_SAP"]);

                    item.SOLIN_SUM_CAR_CON = Funciones.CheckDbl(dr["SOLIN_SUM_CAR_CON"]);
                    item.CREDV_OBS_FLEXIBILIZACION = Funciones.CheckStr(dr["CREDV_OBS_FLEXIBILIZACION"]);
                    item.CREDV_MOTIVO = Funciones.CheckStr(dr["CREDV_MOTIVO"]);
                    item.CLIEV_CALIFICACION_PAGO = Funciones.CheckStr(dr["CLIEV_CALIFICACION_PAGO"]);
                    item.BURO_DESCRIPCION = Funciones.CheckStr(dr["BURO_DESCRIPCION"]);
                    item.TOPEV_DESCRIPCION = Funciones.CheckStr(dr["TOPEV_DESCRIPCION"]);
                    item.CLIEN_MONTO_VENCIDO = Funciones.CheckDbl(dr["CLIEN_MONTO_VENCIDO"]);

                    item.TCESC_DESCRIPCION = Funciones.CheckStr(dr["TCESC_DESCRIPCION"]);
                    item.TPROV_DESCRIPCION = Funciones.CheckStr(dr["TOFV_DESCRIPCION"]);

                    item.CLIEV_RIESGO_CLARO = Funciones.CheckStr(dr["CLIEV_RIESGO_CLARO"]);
                    item.CLIEV_COMPORTA_PAGO = Funciones.CheckStr(dr["CLIEV_COMPORTA_PAGO"]);
                    item.CLIEC_FLAG_EXONERAR_RA = Funciones.CheckStr(dr["CLIEC_FLAG_EXONERAR_RA"]);

                    item.SOLIV_FACTOR_RENOVACION = Funciones.CheckDbl(dr["SOLIV_FACTOR_RENOVACION"]);
                    item.TELEFONO = Funciones.CheckStr(dr["TELEFONO"]);
                    item.RENOF_CFACTUAL = Funciones.CheckDbl(dr["RENOF_CFACTUAL"]);
                    item.PLAN_ACTUAL = Funciones.CheckStr(dr["PLAN_ACTUAL"]);
                    item.COMBV_DESCRIPCION = Funciones.CheckStr(dr["COMBV_DESCRIPCION"]);

                    item.TOPEN_CODIGO = Funciones.CheckInt(dr["TOPEN_CODIGO"]);

                    //Inicio IDEA-30067
                    item.PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    //Fin IDEA-30067
                    item.SOLIC_DEUDA_CLIENTE = Funciones.CheckStr(dr["SOLIC_DEUDA_CLIENTE"]); //PROY-29121
                    break;
                }

                // para los representantes legal
                dr.NextResult();

                List<BERepresentanteLegal> representantes = new List<BERepresentanteLegal>();
                while (dr.Read())
                    {
                        Int64 representante_id = Funciones.CheckInt64(dr["APODN_CODIGO"]);
                        if (representante_id > 0)
                        {
                            BERepresentanteLegal oRepre = new BERepresentanteLegal();
                            oRepre.APODN_CODIGO = representante_id;
                            oRepre.APODV_NOM_REP_LEG = Funciones.CheckStr(dr["APODV_NOM_REP_LEG"]);
                            oRepre.APODV_APA_REP_LEG = Funciones.CheckStr(dr["APODV_APA_REP_LEG"]);
                            oRepre.APODV_AMA_REP_LEG = Funciones.CheckStr(dr["APODV_AMA_REP_LEG"]);
                            oRepre.APODC_TIP_DOC_REP = Funciones.CheckStr(dr["APODC_TIP_DOC_REP"]);
                            oRepre.APODV_NUM_DOC_REP = Funciones.CheckStr(dr["APODV_NUM_DOC_REP"]);
                            oRepre.TPODC_CODIGO = Funciones.CheckStr(dr["TPODC_CODIGO"]);
                            oRepre.APODV_CAR_REP = Funciones.CheckStr(dr["APODV_CAR_REP"]);
                            oRepre.TDOCV_DESCRIPCION_REP = Funciones.CheckStr(dr["TDOCV_DESCRIPCION_REP"]);
                            representantes.Add(oRepre);
                        }
                    }
                item.REPRESENTANTE_LEGAL = new List<BERepresentanteLegal>(representantes);

                dr.NextResult();

                while(dr.Read())
                {
                    item.CLIEV_PRE_DIR = Funciones.CheckStr(dr["CLIEV_PRE_DIR"]);
                    item.CLIEV_REF_DIR = Funciones.CheckStr(dr["CLIEV_REF_DIR"]);
                    item.CLIEC_COD_DEP_DIR = Funciones.CheckStr(dr["CLIEC_COD_DEP_DIR"]);
                    item.CLIEC_COD_PRO_DIR = Funciones.CheckStr(dr["CLIEC_COD_PRO_DIR"]);
                    item.CLIEC_COD_DIS_DIR = Funciones.CheckStr(dr["CLIEC_COD_DIS_DIR"]);
                    item.CLIEC_COD_POS_DIR = Funciones.CheckStr(dr["CLIEC_COD_POS_DIR"]);
                    item.CLIEV_TEL_LEG = Funciones.CheckStr(dr["CLIEV_TEL_LEG"]);
                    item.CLIEV_PRE_DIR_FAC = Funciones.CheckStr(dr["CLIEV_PRE_DIR_FAC"]);
                    item.CLIEV_REF_DIR_FAC = Funciones.CheckStr(dr["CLIEV_REF_DIR_FAC"]);
                    item.CLIEC_COD_DEP_FAC = Funciones.CheckStr(dr["CLIEC_COD_DEP_FAC"]);
                    item.CLIEC_COD_PRO_FAC = Funciones.CheckStr(dr["CLIEC_COD_PRO_FAC"]);
                    item.CLIEC_COD_DIS_FAC = Funciones.CheckStr(dr["CLIEC_COD_DIS_FAC"]);
                    item.CLIEC_COD_POS_FAC = Funciones.CheckStr(dr["CLIEC_COD_POS_FAC"]);
                    break;
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return item;
        }

        public BESolicitudEmpresa obtenerEstadoSolEmp(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("K_CUR_REP_LEGAL", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("K_CUR_DIRECCION", DbType.Object, ParameterDirection.Output)
			};

            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            string[] sTab = { "solicitud", "representante", "direccion" };
            objRequest.TableNames = sTab;
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SECSS_DET_SOL_CORP";
            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            BESolicitudEmpresa item = new BESolicitudEmpresa();
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    item.SOLIN_CODIGO = Funciones.CheckInt64(dr["SOLIN_CODIGO"]);
                    item.ESTAC_CODIGO = Funciones.CheckStr(dr["ESTAC_CODIGO"]);
                    break;
                }
                
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return item;
        }

        public string ValidarSECRecurrente(string strTipoDocumento, string strNroDocumento, string strOferta, string strCasoEspecial,
                                            string strCadenaDetalle, ref string flgReingresoSec)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SEC_RECURRENTE", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_FLG_REINGRESO", DbType.String, 2, ParameterDirection.Output),
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_DOC", DbType.String, 16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OFERTA", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CADENA", DbType.String, 5000, ParameterDirection.Input)
            };
            int i;
            string SEC = "";
            i = 2; arrParam[i].Value = strTipoDocumento;
            i++; arrParam[i].Value = strNroDocumento;
            i++; arrParam[i].Value = strOferta;
            i++; arrParam[i].Value = strCasoEspecial;
            i++; arrParam[i].Value = strCadenaDetalle;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_VALIDA_SEC_RECURRENTE";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                SEC = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[0]).Value);
                flgReingresoSec = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[1]).Value);
            }
            catch
            {
                SEC = "0";
                flgReingresoSec = "";
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return SEC;
        }

        public Int64 RegistrarEvaluacionDTH_HFC(BESolicitudPersona objSolicitud)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("K_RESULTADO", DbType.Double,ParameterDirection.Output),
				new DAABRequest.Parameter("P_OVENC_CODIGO", DbType.String,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CANAC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_USU_VEN", DbType.AnsiStringFixedLength,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_EXI_BSC_FIN", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ANALC_CODIGO", DbType.AnsiStringFixedLength,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.AnsiStringFixedLength,16,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_RAZ_SOC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEN_PROM_VEN", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TPROC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SEGMN_CODIGO", DbType.Int32,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TCLIC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TVENC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TACTC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TOPEN_CODIGO", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_PLA_MAX_1", DbType.AnsiStringFixedLength,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_PLA_MAX_2", DbType.AnsiStringFixedLength,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_PLA_MAX_3", DbType.AnsiStringFixedLength,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PACUC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FPAGC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CAN_LIN", DbType.Int32,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RFINC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_MRECC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_TIP_CAR_MAN", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_IMP_DG_MAN", DbType.String,14,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESTAC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TEVAC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_FLA_TER", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_EST", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_OFI_VEN", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_RES_FIN", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_TIP_ACT", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_COM_PUN_VEN", DbType.String,200,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_COM_EVALUADOR", DbType.String,200,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_USU_CRE", DbType.AnsiStringFixedLength,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_NOM", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_APE_PAT", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_APE_MAT", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_EVA_ESS", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_EVA_SUN", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_COD_RES_DIR", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_RES_DIR", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_CAR_CLI", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_TIP_CAR_FIJ", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_IMP_DG", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_RES_EXP_CON", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_NUM_OPE_CON", DbType.String,15,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_LIM_CRE_CON", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_SUM_CAR_CON", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_NUM_CAR_FIJ", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TCESC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_SCO_TXT_CON", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_SCO_NUM_CON", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CODIGO_PADRE", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_INFOCORP", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_HINFV_MENSAJE", DbType.String,1000,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RUCEMPLEADOR", DbType.String,15,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NOMBREEMPRESA", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CODCAMPANNA", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_EXI_BSC_CON", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENDEDOR_ID", DbType.String,8,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_CONSUMO", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_FLAG_CORR", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_CORREO", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_EST_CIV", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_UBIGEO_INEI", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_ORIGEN_LC_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_ANALISIS_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_SCORE_RANKING_OPER_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_PUNTAJE_DC", DbType.Double,ParameterDirection.Input),										
				new DAABRequest.Parameter("P_SOLIN_LC_DATA_CREDITO_DC", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_LC_BASE_EXTERNA_DC", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_LC_CLARO_DC", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_REGLAS_DURAS_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_ALERT_COMPORT_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_ALERTAS_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_CORRESP_SALDO_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIED_FEC_NAC", DbType.Date,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIED_FEC_NAC_PDV", DbType.Date,ParameterDirection.Input),								
				new DAABRequest.Parameter("P_LC_DISPONIBLE", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CF_MENORES", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CF_MAYORES", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_DEUDA", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_BLOQUEO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEN_SEC_ASOCIADA", DbType.Int64 ,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RESPUESTA_DC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PRODUCTO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64 ,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_CALIFICACION_PAGO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_BURO_CREDITICIO", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_IMP_DG_GRUPO_SEC", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CF_GRUPO_SEC", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_RIESGO_CLARO", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_COMPORTA_PAGO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_FLAG_EXONERAR_RA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_VTA_PROACTIVA", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPV_CODIGO", DbType.AnsiStringFixedLength,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_VEN_DNI", DbType.AnsiStringFixedLength,8,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_KIT_COS_INST", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_BLOQUEO", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_NRO_CARTA_PRESELEC", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_OPERADOR", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_PAGINA_CLARO", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CF_ALQUILER_KIT", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_FLAG_VALIDARSECPEN", DbType.String, 1, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_SOLIC_DEUDA_CLIENTE", DbType.String, 2, ParameterDirection.Input)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = objSolicitud.OVENC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.CANAC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_USU_VEN;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EXI_BSC_FIN;
            i++; arrParam[i].Value = objSolicitud.ANALC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TDOCC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.CLIEC_NUM_DOC;
            i++; arrParam[i].Value = objSolicitud.CLIEV_RAZ_SOC;
            i++; arrParam[i].Value = objSolicitud.CLIEN_PROM_VEN;
            i++; arrParam[i].Value = objSolicitud.TPROC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SEGMN_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TCLIC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TVENC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TACTC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TOPEN_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_PLA_MAX_1;
            i++; arrParam[i].Value = objSolicitud.SOLIC_PLA_MAX_2;
            i++; arrParam[i].Value = objSolicitud.SOLIC_PLA_MAX_3;
            i++; arrParam[i].Value = objSolicitud.PACUC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.FPAGC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_CAN_LIN;
            i++; arrParam[i].Value = objSolicitud.RFINC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.MRECC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_TIP_CAR_MAN;
            i++; arrParam[i].Value = objSolicitud.SOLIN_IMP_DG_MAN;
            i++; arrParam[i].Value = objSolicitud.ESTAC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TEVAC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_FLA_TER;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_EST;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_OFI_VEN;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_RES_FIN;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_TIP_ACT;
            i++; arrParam[i].Value = objSolicitud.SOLIV_COM_PUN_VEN;
            i++; arrParam[i].Value = objSolicitud.SOLIV_COM_EVALUADOR;
            i++; arrParam[i].Value = objSolicitud.SOLIC_USU_CRE;
            i++; arrParam[i].Value = objSolicitud.CLIEV_NOM;
            i++; arrParam[i].Value = objSolicitud.CLIEV_APE_PAT;
            i++; arrParam[i].Value = objSolicitud.CLIEV_APE_MAT;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EVA_ESS;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EVA_SUN;
            i++; arrParam[i].Value = objSolicitud.SOLIC_COD_RES_DIR;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_RES_DIR;
            i++; arrParam[i].Value = objSolicitud.SOLIV_CAR_CLI;
            i++; arrParam[i].Value = objSolicitud.SOLIC_TIP_CAR_FIJ;
            i++; arrParam[i].Value = objSolicitud.SOLIN_IMP_DG;
            i++; arrParam[i].Value = objSolicitud.SOLIV_RES_EXP_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIV_NUM_OPE_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LIM_CRE_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_SUM_CAR_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_NUM_CAR_FIJ;
            i++; arrParam[i].Value = objSolicitud.TCESC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_SCO_TXT_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_SCO_NUM_CON;

            if (objSolicitud.SOLIN_CODIGO_PADRE == 0)
            {
                i++; arrParam[i].Value = DBNull.Value;
            }
            else
            {
                i++; arrParam[i].Value = objSolicitud.SOLIN_CODIGO_PADRE;
            }

            i++; arrParam[i].Value = objSolicitud.FLAG_INFOCORP;
            i++; arrParam[i].Value = objSolicitud.HINFV_MENSAJE;
            i++; arrParam[i].Value = objSolicitud.RUCEMPLEADOR;
            i++; arrParam[i].Value = objSolicitud.NOMBREEMPRESA;
            i++; arrParam[i].Value = objSolicitud.CODCAMPANNA;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EXI_BSC_CON;
            i++; arrParam[i].Value = objSolicitud.VENDEDOR_ID;
            i++; arrParam[i].Value = objSolicitud.FLAG_CONSUMO;
            i++; arrParam[i].Value = objSolicitud.SOLIV_FLAG_CORR;
            i++; arrParam[i].Value = objSolicitud.SOLIV_CORREO;
            i++; arrParam[i].Value = objSolicitud.CLIEV_EST_CIV;
            i++; arrParam[i].Value = objSolicitud.SOLIV_UBIGEO_INEI;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ORIGEN_LC_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ANALISIS_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_SCORE_RANKING_OPER_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_PUNTAJE_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LC_DATA_CREDITO_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LC_BASE_EXTERNA_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LC_CLARO_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_REGLAS_DURAS_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ALERT_COMPORT_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ALERTAS_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_CORRESP_SALDO_DC;
            i++; arrParam[i].Value = objSolicitud.CLIED_FEC_NAC;
            i++; arrParam[i].Value = objSolicitud.CLIED_FEC_NAC_PDV;
            i++; arrParam[i].Value = objSolicitud.LC_DISPONIBLE;
            i++; arrParam[i].Value = objSolicitud.CF_MENORES;
            i++; arrParam[i].Value = objSolicitud.CF_MAYORES;
            i++; arrParam[i].Value = objSolicitud.DEUDA;
            i++; arrParam[i].Value = objSolicitud.BLOQUEO;
            i++; arrParam[i].Value = objSolicitud.CLIEN_SEC_ASOCIADA;
            i++; arrParam[i].Value = objSolicitud.RESPUESTA_DC;
            i++; arrParam[i].Value = objSolicitud.PRDC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_GRUPO_SEC;
            i++; arrParam[i].Value = objSolicitud.CLIEV_CALIFICACION_PAGO;
            i++; arrParam[i].Value = objSolicitud.BURO_CREDITICIO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_IMP_DG_GRUPO_SEC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_CF_GRUPO_SEC;
            i++; arrParam[i].Value = objSolicitud.CLIEV_RIESGO_CLARO;
            i++; arrParam[i].Value = objSolicitud.CLIEV_COMPORTA_PAGO;
            i++; arrParam[i].Value = objSolicitud.CLIEC_FLAG_EXONERAR_RA;

            i++; arrParam[i].Value = objSolicitud.FLAG_VTA_PROACTIVA;
            i++; arrParam[i].Value = objSolicitud.CAMPV_CODIGO;
            i++; arrParam[i].Value = objSolicitud.CLIEC_VEN_DNI;
            i++; arrParam[i].Value = objSolicitud.SOLIN_KIT_COS_INST;
            i++; arrParam[i].Value = objSolicitud.CLIEC_BLOQUEO;
            i++; arrParam[i].Value = objSolicitud.CLIEV_NRO_CARTA_PRESELEC;
            i++; arrParam[i].Value = objSolicitud.CLIEV_OPERADOR;
            i++; arrParam[i].Value = objSolicitud.CLIEV_PAGINA_CLARO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_CF_ALQUILER_KIT;
            //gaa20151210
            i++; arrParam[i].Value = objSolicitud.FLAG_VALIDARSECPENDIENTE;
            //fin gaa20151210
            i++; arrParam[i].Value = objSolicitud.DEUDA_CLIENTE;//PROY-29121
            
            Int64 nroSEC;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objSolicitud.NRO_DOCUMENTO);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_SOLICITUD_CONS + ".SECSI_INS_SOL_PERSONA_SGA";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter pSalida1;
                pSalida1 = (IDataParameter)objRequest.Parameters[0];
                nroSEC = Funciones.CheckInt64(pSalida1.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }

        public Int64 RegistrarEvaluacion(BESolicitudPersona objSolicitud)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("K_RESULTADO", DbType.Double,ParameterDirection.Output),
				new DAABRequest.Parameter("P_OVENC_CODIGO", DbType.String,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CANAC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_USU_VEN", DbType.AnsiStringFixedLength,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_EXI_BSC_FIN", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ANALC_CODIGO", DbType.AnsiStringFixedLength,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.AnsiStringFixedLength,16,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_RAZ_SOC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEN_PROM_VEN", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TPROC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SEGMN_CODIGO", DbType.Int32,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TCLIC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TVENC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TACTC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TOPEN_CODIGO", DbType.String,2,ParameterDirection.Input), //PROY-140743 - Vnta Cuotas
				new DAABRequest.Parameter("P_SOLIC_PLA_MAX_1", DbType.AnsiStringFixedLength,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_PLA_MAX_2", DbType.AnsiStringFixedLength,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_PLA_MAX_3", DbType.AnsiStringFixedLength,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PACUC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FPAGC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CAN_LIN", DbType.Int32,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RFINC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_MRECC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_TIP_CAR_MAN", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_IMP_DG_MAN", DbType.String,14,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESTAC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TEVAC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_FLA_TER", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_EST", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_OFI_VEN", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_RES_FIN", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_TIP_ACT", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_COM_PUN_VEN", DbType.String,200,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_COM_EVALUADOR", DbType.String,200,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_USU_CRE", DbType.AnsiStringFixedLength,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_NOM", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_APE_PAT", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_APE_MAT", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_EVA_ESS", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_EVA_SUN", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_COD_RES_DIR", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_RES_DIR", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_CAR_CLI", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_TIP_CAR_FIJ", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_IMP_DG", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_RES_EXP_CON", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_NUM_OPE_CON", DbType.String,15,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_LIM_CRE_CON", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_SUM_CAR_CON", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_NUM_CAR_FIJ", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TCESC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_SCO_TXT_CON", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_SCO_NUM_CON", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CODIGO_PADRE", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_INFOCORP", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_HINFV_MENSAJE", DbType.String,1000,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RUCEMPLEADOR", DbType.String,15,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NOMBREEMPRESA", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CODCAMPANNA", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_EXI_BSC_CON", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENDEDOR_ID", DbType.String,8,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_CONSUMO", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_FLAG_CORR", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_CORREO", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_EST_CIV", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_UBIGEO_INEI", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_ORIGEN_LC_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_ANALISIS_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_SCORE_RANKING_OPER_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_PUNTAJE_DC", DbType.Double,ParameterDirection.Input),										
				new DAABRequest.Parameter("P_SOLIN_LC_DATA_CREDITO_DC", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_LC_BASE_EXTERNA_DC", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_LC_CLARO_DC", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_REGLAS_DURAS_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_ALERT_COMPORT_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_ALERTAS_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_CORRESP_SALDO_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIED_FEC_NAC", DbType.Date,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIED_FEC_NAC_PDV", DbType.Date, ParameterDirection.Input),
				new DAABRequest.Parameter("P_LC_DISPONIBLE", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CF_MENORES", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CF_MAYORES", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DEUDA", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_BLOQUEO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEN_SEC_ASOCIADA", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_RESPUESTA_DC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PRODUCTO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_CALIFICACION_PAGO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_BURO_CREDITICIO", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_IMP_DG_GRUPO_SEC", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CF_GRUPO_SEC", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_RIESGO_CLARO", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_COMPORTA_PAGO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_FLAG_EXONERAR_RA", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_FLAG_VALIDARSECPEN", DbType.String, 1, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_CODNACION", DbType.String, 20, ParameterDirection.Input), //PROY-31636
                new DAABRequest.Parameter("P_CLIEV_DESCNACION", DbType.String, 80, ParameterDirection.Input), //PROY-31636
                new DAABRequest.Parameter("PI_SOLIC_DEUDA_CLIENTE", DbType.String, 2, ParameterDirection.Input)//PROY-29121
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i=1; arrParam[i].Value = objSolicitud.OVENC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.CANAC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_USU_VEN;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EXI_BSC_FIN;
            i++; arrParam[i].Value = objSolicitud.ANALC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TDOCC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.CLIEC_NUM_DOC;
            i++; arrParam[i].Value = objSolicitud.CLIEV_RAZ_SOC;
            i++; arrParam[i].Value = objSolicitud.CLIEN_PROM_VEN;
            i++; arrParam[i].Value = objSolicitud.TPROC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SEGMN_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TCLIC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TVENC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TACTC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TOPEN_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_PLA_MAX_1;
            i++; arrParam[i].Value = objSolicitud.SOLIC_PLA_MAX_2;
            i++; arrParam[i].Value = objSolicitud.SOLIC_PLA_MAX_3;
            i++; arrParam[i].Value = objSolicitud.PACUC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.FPAGC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_CAN_LIN;
            i++; arrParam[i].Value = objSolicitud.RFINC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.MRECC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_TIP_CAR_MAN;
            i++; arrParam[i].Value = objSolicitud.SOLIN_IMP_DG_MAN;
            i++; arrParam[i].Value = objSolicitud.ESTAC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TEVAC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_FLA_TER;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_EST;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_OFI_VEN;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_RES_FIN;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_TIP_ACT;
            i++; arrParam[i].Value = objSolicitud.SOLIV_COM_PUN_VEN;
            i++; arrParam[i].Value = objSolicitud.SOLIV_COM_EVALUADOR;
            i++; arrParam[i].Value = objSolicitud.SOLIC_USU_CRE;
            i++; arrParam[i].Value = objSolicitud.CLIEV_NOM;
            i++; arrParam[i].Value = objSolicitud.CLIEV_APE_PAT;
            i++; arrParam[i].Value = objSolicitud.CLIEV_APE_MAT;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EVA_ESS;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EVA_SUN;
            i++; arrParam[i].Value = objSolicitud.SOLIC_COD_RES_DIR;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_RES_DIR;
            i++; arrParam[i].Value = objSolicitud.SOLIV_CAR_CLI;
            i++; arrParam[i].Value = objSolicitud.SOLIC_TIP_CAR_FIJ;
            i++; arrParam[i].Value = objSolicitud.SOLIN_IMP_DG;
            i++; arrParam[i].Value = objSolicitud.SOLIV_RES_EXP_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIV_NUM_OPE_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LIM_CRE_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_SUM_CAR_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_NUM_CAR_FIJ;
            i++; arrParam[i].Value = objSolicitud.TCESC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_SCO_TXT_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_SCO_NUM_CON;

            if (objSolicitud.SOLIN_CODIGO_PADRE == 0)
            {
                i++; arrParam[i].Value = DBNull.Value;
            }
            else
            {
                i++; arrParam[i].Value = objSolicitud.SOLIN_CODIGO_PADRE;
            }

            i++; arrParam[i].Value = objSolicitud.FLAG_INFOCORP;
            i++; arrParam[i].Value = objSolicitud.HINFV_MENSAJE;
            i++; arrParam[i].Value = objSolicitud.RUCEMPLEADOR;
            i++; arrParam[i].Value = objSolicitud.NOMBREEMPRESA;
            i++; arrParam[i].Value = objSolicitud.CODCAMPANNA;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EXI_BSC_CON;
            i++; arrParam[i].Value = objSolicitud.VENDEDOR_ID;
            i++; arrParam[i].Value = objSolicitud.FLAG_CONSUMO;
            i++; arrParam[i].Value = objSolicitud.SOLIV_FLAG_CORR;
            i++; arrParam[i].Value = objSolicitud.SOLIV_CORREO;
            i++; arrParam[i].Value = objSolicitud.CLIEV_EST_CIV;
            i++; arrParam[i].Value = objSolicitud.SOLIV_UBIGEO_INEI;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ORIGEN_LC_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ANALISIS_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_SCORE_RANKING_OPER_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_PUNTAJE_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LC_DATA_CREDITO_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LC_BASE_EXTERNA_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LC_CLARO_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_REGLAS_DURAS_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ALERT_COMPORT_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ALERTAS_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_CORRESP_SALDO_DC;
            i++; arrParam[i].Value = objSolicitud.CLIED_FEC_NAC;
            i++; arrParam[i].Value = objSolicitud.CLIED_FEC_NAC_PDV;
            i++; arrParam[i].Value = objSolicitud.LC_DISPONIBLE;
            i++; arrParam[i].Value = objSolicitud.CF_MENORES;
            i++; arrParam[i].Value = objSolicitud.CF_MAYORES;
            i++; arrParam[i].Value = objSolicitud.DEUDA;
            i++; arrParam[i].Value = objSolicitud.BLOQUEO;
            i++; arrParam[i].Value = objSolicitud.CLIEN_SEC_ASOCIADA;
            i++; arrParam[i].Value = objSolicitud.RESPUESTA_DC;
            i++; arrParam[i].Value = objSolicitud.PRDC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_GRUPO_SEC;
            i++; arrParam[i].Value = objSolicitud.CLIEV_CALIFICACION_PAGO;
            i++; arrParam[i].Value = objSolicitud.BURO_CREDITICIO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_IMP_DG_GRUPO_SEC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_CF_GRUPO_SEC;
            i++; arrParam[i].Value = objSolicitud.CLIEV_RIESGO_CLARO;
            i++; arrParam[i].Value = objSolicitud.CLIEV_COMPORTA_PAGO;
            i++; arrParam[i].Value = objSolicitud.CLIEC_FLAG_EXONERAR_RA;
            //gaa20151210
            i++; arrParam[i].Value = objSolicitud.FLAG_VALIDARSECPENDIENTE;
            //fin gaa20151210
            i++; arrParam[i].Value = objSolicitud.CLIEC_CODNACION; //PROY-31636
            i++; arrParam[i].Value = objSolicitud.CLIEV_DESCNACION; //PROY-31636
            i++; arrParam[i].Value = objSolicitud.DEUDA_CLIENTE;//PROY-29121
            Int64 nroSEC;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objSolicitud.NRO_DOCUMENTO);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_SOLICITUD_CONS + ".SECSI_INS_SOL_PERSONA";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter pSalida1;
                pSalida1 = (IDataParameter)objRequest.Parameters[0];
                nroSEC = Funciones.CheckInt64(pSalida1.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }

        public Int64 GrabarEvaluacionEmpresa(BESolicitudEmpresa item)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("K_RESULTADO", DbType.Int64,ParameterDirection.Output),
                new DAABRequest.Parameter("P_OVENC_CODIGO",DbType.String,4,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CANAC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_ANEXO2", DbType.String,7,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIC_EXI_BSC_FIN", DbType.String,1,ParameterDirection.Input),
                new DAABRequest.Parameter("P_ANALC_CODIGO", DbType.String,4,ParameterDirection.Input),
                new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String,16,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_RAZ_SOC", DbType.String,40,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_NOMBRE", DbType.String,40,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_APE_PAT", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_APE_MAT", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEN_PROM_VEN", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_TPROC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SEGMN_CODIGO", DbType.Int64,ParameterDirection.Input),
                new DAABRequest.Parameter("P_TCLIC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_TVENC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_TACTC_CODIGO", DbType.String,2,ParameterDirection.Input),												   
                new DAABRequest.Parameter("P_SOLIN_CAN_LIN", DbType.Int32,ParameterDirection.Input),
                new DAABRequest.Parameter("P_RFINC_CODIGO", DbType.String,2,ParameterDirection.Input),												   
                new DAABRequest.Parameter("P_SOLIC_TIP_CAR_MAN", DbType.String,1,ParameterDirection.Input),												   
                new DAABRequest.Parameter("P_SOLIN_NUM_CAR_FIJ", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_ESTAC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_TEVAC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIC_FLA_TER", DbType.String,1,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_DES_EST", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_DES_OFI_VEN", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_DES_RES_FIN", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_DES_TIP_ACT", DbType.String,20,ParameterDirection.Input),												   
                new DAABRequest.Parameter("P_SOLIV_COM_EVALUADOR", DbType.String,500,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIC_USU_CRE", DbType.String,10,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLASC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_USUAN_CODIGO", DbType.Int64,ParameterDirection.Input),
                new DAABRequest.Parameter("P_FLEXN_CODIGO", DbType.Int64,ParameterDirection.Input),												   
                new DAABRequest.Parameter("P_TRIEC_CODIGO", DbType.String,4,ParameterDirection.Input),	// PROY-140257											   
                new DAABRequest.Parameter("P_OPERC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_CAN_LIN_EXCOMP", DbType.Int32,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_CONSULTOR", DbType.Int64,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_DEUDA_CLIENTE", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_LINEA_CLIENTE", DbType.Int32,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_ANTIGUEDAD", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_CODIGO_PADRE", DbType.Int64,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIC_FLAG_REINGRESO", DbType.String,1,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_NUM_OPE_CON", DbType.String,15,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_NUM_CAR_FIJ_LINEA", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_ANTIGUEDAD_CLIENTE", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIC_FLAG_EMPRESA_TRAFICO", DbType.String,1,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_FLA_VER_RES", DbType.String,1,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIC_FLAG_EMPRESA_TOLERAN", DbType.String,1,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_SUM_CAR_FIN", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_CAR_FIJO_ACTUAL", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_SUBSIDIO_TOTAL", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_LINEAS_RECURRENTE_ACTUAL", DbType.Int32,ParameterDirection.Input),
                new DAABRequest.Parameter("P_LINEAS_MAYOR_N_DIAS", DbType.Int32,ParameterDirection.Input),
                new DAABRequest.Parameter("P_LINEAS_MENOR_N_DIAS", DbType.Int32,ParameterDirection.Input),
                new DAABRequest.Parameter("P_PARAM_DIAS_RECURRENTE", DbType.Int32,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_COM_DG", DbType.String,200,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_FLAG_ENVIO", DbType.String,1,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_BOLSA_REF", DbType.Double,1,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CAMPN_CODIGO", DbType.Int64,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_EMAIL_AUTORIZADO", DbType.String, 100, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_FLAG_CORR", DbType.String, 1, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIV_CORREO", DbType.String, 200, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIC_LINEA_CREDITO_CALC", DbType.Decimal, ParameterDirection.Input),
                new DAABRequest.Parameter("P_DPCHN_CODIGO", DbType.Int64, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_PRE_DIR", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_DIRECCION", DbType.String, 4000,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_REF_DIR", DbType.String, 4000, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_COD_DEP_DIR", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_COD_PRO_DIR", DbType.String, 3,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_COD_DIS_DIR", DbType.String, 4, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_COD_POS_DIR", DbType.String, 3, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_PRE_TEL_LEG", DbType.String,3, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_TEL_LEG", DbType.String, 13, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_PRE_DIR_FAC", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_DIR_FAC", DbType.String, 4000,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_REF_DIR_FAC", DbType.String, 40, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_COD_DEP_FAC", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_COD_PRO_FAC", DbType.String, 3,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_COD_DIS_FAC", DbType.String, 4, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_COD_POS_FAC", DbType.String, 3, ParameterDirection.Input),
                new DAABRequest.Parameter("P_RGLPC_PODERES", DbType.String, 1, ParameterDirection.Input),
                new DAABRequest.Parameter("P_PACUC_CODIGO", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TOPEN_CODIGO", DbType.Int64, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIC_TIP_CAR_FIJ", DbType.String,1,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_IMP_DG", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_IMP_DG_MAN", DbType.Double,ParameterDirection.Input),
                new DAABRequest.Parameter("P_TPREC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_FPAGC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_USU_VEN", DbType.String,10,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_LIM_CRE_FIN", DbType.Decimal,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_SUM_CAR_CON", DbType.Decimal,ParameterDirection.Input),
                new DAABRequest.Parameter("P_TCESC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_PRDC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_CALIFICACION_PAGO", DbType.String, 4, ParameterDirection.Input),
                new DAABRequest.Parameter("P_BURO_CREDITICIO", DbType.Int16, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_IMP_DG_GRUPO_SEC", DbType.Double, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_CF_GRUPO_SEC", DbType.Double, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEN_MONTO_VENCIDO", DbType.Double, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_RIESGO_CLARO", DbType.String, 50, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEV_COMPORTA_PAGO", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_FLAG_EXONERAR_RA", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_KIT_COS_INST", DbType.Double, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_SOLIC_DEUDA_CLIENTE", DbType.String, 2, ParameterDirection.Input)//PROY-29121
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = item.OVENC_CODIGO;
            ++i; arrParam[i].Value = item.CANAC_CODIGO;
            ++i; arrParam[i].Value = item.ANEXO2;
            ++i; arrParam[i].Value = item.SOLIC_EXI_BSC_FIN;
            ++i; arrParam[i].Value = item.ANALC_CODIGO;
            ++i; arrParam[i].Value = item.TDOCC_CODIGO;
            ++i; arrParam[i].Value = item.CLIEC_NUM_DOC;
            ++i; arrParam[i].Value = item.CLIEV_RAZ_SOC;
            ++i; arrParam[i].Value = item.CLIEV_NOMBRE;
            ++i; arrParam[i].Value = item.CLIEV_APE_PAT;
            ++i; arrParam[i].Value = item.CLIEV_APE_MAT;
            ++i; arrParam[i].Value = item.CLIEN_PROM_VEN;
            ++i; arrParam[i].Value = item.TPROC_CODIGO;
            ++i; arrParam[i].Value = item.SEGMN_CODIGO;
            ++i; arrParam[i].Value = item.TCLIC_CODIGO;
            ++i; arrParam[i].Value = item.TVENC_CODIGO;
            ++i; arrParam[i].Value = item.TACTC_CODIGO;
            ++i; arrParam[i].Value = item.SOLIN_CAN_LIN;
            ++i; arrParam[i].Value = item.RFINC_CODIGO;
            ++i; arrParam[i].Value = item.SOLIC_TIP_CAR_MAN;
            ++i; arrParam[i].Value = item.SOLIN_NUM_CAR_FIJ;
            ++i; arrParam[i].Value = item.ESTAC_CODIGO;
            ++i; arrParam[i].Value = item.TEVAC_CODIGO;
            ++i; arrParam[i].Value = item.SOLIC_FLA_TER;
            ++i; arrParam[i].Value = item.SOLIV_DES_EST;
            ++i; arrParam[i].Value = item.SOLIV_DES_OFI_VEN;
            ++i; arrParam[i].Value = item.SOLIV_DES_RES_FIN;
            ++i; arrParam[i].Value = item.SOLIV_DES_TIP_ACT;
            ++i; arrParam[i].Value = item.SOLIV_COM_EVALUADOR;
            ++i; arrParam[i].Value = item.SOLIC_USU_CRE;
            ++i; arrParam[i].Value = item.CLASC_CODIGO;
            ++i; if (item.USUAN_CODIGO > 0) arrParam[i].Value = item.USUAN_CODIGO;
            ++i; arrParam[i].Value = item.FLEXN_CODIGO;
            ++i; arrParam[i].Value = item.TRIEC_CODIGO;
            ++i; arrParam[i].Value = item.OPERC_CODIGO;
            ++i; if (item.SOLIN_CAN_LIN_EXCOMP > 0) arrParam[i].Value = item.SOLIN_CAN_LIN_EXCOMP;
            ++i; if (item.CONSULTOR_ID > 0) arrParam[i].Value = item.CONSULTOR_ID;
            ++i; if (item.SOLIN_DEUDA_CLIENTE > 0) arrParam[i].Value = item.SOLIN_DEUDA_CLIENTE;
            ++i; if (item.SOLIN_LINEA_CLIENTE > 0) arrParam[i].Value = item.SOLIN_LINEA_CLIENTE;
            ++i; if (item.SOLIN_ANTIGUEDAD > 0) arrParam[i].Value = item.SOLIN_ANTIGUEDAD;
            ++i; if (item.SOLIN_CODIGO_PADRE > 0) arrParam[i].Value = item.SOLIN_CODIGO_PADRE;
            ++i; if (item.SOLIC_FLAG_REINGRESO != "") arrParam[i].Value = item.SOLIC_FLAG_REINGRESO;
            ++i; if (item.SOLIV_NUM_OPE_CON != "") arrParam[i].Value = item.SOLIV_NUM_OPE_CON;
            ++i; if (item.SOLIN_NUM_CAR_FIJ_LINEA > 0) arrParam[i].Value = item.SOLIN_NUM_CAR_FIJ_LINEA;
            ++i; if (item.SOLIN_ANTIGUEDAD_CLIENTE > 0) arrParam[i].Value = item.SOLIN_ANTIGUEDAD_CLIENTE;
            ++i; if (item.SOLIC_FLAG_EMPRESA_TRAFICO != "") arrParam[i].Value = item.SOLIC_FLAG_EMPRESA_TRAFICO;
            ++i; if (item.FLAG_RESPONSABLE_PUNTO_VENTA != "") arrParam[i].Value = item.FLAG_RESPONSABLE_PUNTO_VENTA;
            ++i; if (item.SOLIC_FLAG_EMPRESA_TOLERAN != "") arrParam[i].Value = item.SOLIC_FLAG_EMPRESA_TOLERAN;
            ++i; arrParam[i].Value = item.SOLIN_SUM_CAR_FIN;
            ++i; arrParam[i].Value = item.SOLIN_CAR_FIJO_ACTUAL;
            ++i; arrParam[i].Value = item.SOLIN_SUBSIDIO_TOTAL;

            ++i; arrParam[i].Value = item.NRO_LINEAS_RECURRENTE_ACTUAL;
            ++i; arrParam[i].Value = item.NRO_LINEAS_MAYOR_N_DIAS;
            ++i; arrParam[i].Value = item.NRO_LINEAS_MENOR_N_DIAS;
            ++i; arrParam[i].Value = item.DIAS_LINEAS_RECURRENTE;
            ++i; arrParam[i].Value = item.SOLIV_COM_DG;
            ++i; arrParam[i].Value = item.SOLIV_FLAG_ENVIO;
            ++i; if (item.SOLIN_BOLSA_REF > 0) arrParam[i].Value = item.SOLIN_BOLSA_REF;
            ++i; arrParam[i].Value = item.CAMPN_CODIGO;
            ++i; if (item.EMAIL_AUTORIZADO != "") arrParam[i].Value = item.EMAIL_AUTORIZADO;

            //INICIO - E75688
            ++i; arrParam[i].Value = item.FLAG_CORREO;
            ++i; arrParam[i].Value = item.SOLIV_CORREO;
            //FIN - E75688
            ++i; arrParam[i].Value = item.SOLIN_LINEA_CREDITO_CALC;  //E75810

            //JAR
            ++i; arrParam[i].Value = item.DPCHN_CODIGO;
            ++i; arrParam[i].Value = item.CLIEV_PRE_DIR;
            ++i; arrParam[i].Value = item.CLIEV_DIRECCION;
            ++i; arrParam[i].Value = item.CLIEV_REF_DIR;
            ++i; arrParam[i].Value = item.CLIEC_COD_DEP_DIR;
            ++i; arrParam[i].Value = item.CLIEC_COD_PRO_DIR;
            ++i; arrParam[i].Value = item.CLIEC_COD_DIS_DIR;
            ++i; arrParam[i].Value = item.CLIEC_COD_POS_DIR;
            ++i; arrParam[i].Value = item.CLIEV_PRE_TEL_LEG;
            ++i; arrParam[i].Value = item.CLIEV_TEL_LEG;
            ++i; arrParam[i].Value = item.CLIEV_PRE_DIR_FAC;
            ++i; arrParam[i].Value = item.CLIEV_DIR_FAC;
            ++i; arrParam[i].Value = item.CLIEV_REF_DIR_FAC;
            ++i; arrParam[i].Value = item.CLIEC_COD_DEP_FAC;
            ++i; arrParam[i].Value = item.CLIEC_COD_PRO_FAC;
            ++i; arrParam[i].Value = item.CLIEC_COD_DIS_FAC;
            ++i; arrParam[i].Value = item.CLIEC_COD_POS_FAC;
            ++i; arrParam[i].Value = item.RGLPC_PODERES;
            ++i; arrParam[i].Value = item.PACUC_CODIGO;
            ++i; arrParam[i].Value = item.TOPEN_CODIGO;
            ++i; arrParam[i].Value = item.SOLIC_TIP_CAR_FIJ;
            ++i; arrParam[i].Value = item.SOLIN_IMP_DG;
            ++i; arrParam[i].Value = item.SOLIN_IMP_DG_MAN;
            ++i; arrParam[i].Value = item.TPREC_CODIGO;
            ++i; arrParam[i].Value = item.FPAGC_CODIGO;
            ++i; arrParam[i].Value = item.SOLIN_USU_VEN;
            ++i; if (item.SOLIN_LIM_CRE_FIN > 0) arrParam[i].Value = item.SOLIN_LIM_CRE_FIN;
            ++i; if (item.SOLIN_SUM_CAR_CON > 0) arrParam[i].Value = item.SOLIN_SUM_CAR_CON;
            ++i; arrParam[i].Value = item.TCESC_CODIGO;
            ++i; arrParam[i].Value = item.PRDC_CODIGO;
            ++i; arrParam[i].Value = item.CLIEV_CALIFICACION_PAGO;
            ++i; arrParam[i].Value = item.BURO_CODIGO;
            ++i; arrParam[i].Value = item.SOLIN_GRUPO_SEC;
            ++i; arrParam[i].Value = item.SOLIN_IMP_DG_GRUPO_SEC;
            ++i; arrParam[i].Value = item.SOLIN_CF_GRUPO_SEC;
            ++i; arrParam[i].Value = item.CLIEN_MONTO_VENCIDO;

            ++i; arrParam[i].Value = item.CLIEV_RIESGO_CLARO;
            ++i; arrParam[i].Value = item.CLIEV_COMPORTA_PAGO;
            ++i; arrParam[i].Value = item.CLIEC_FLAG_EXONERAR_RA;

            ++i; arrParam[i].Value = item.SOLIN_KIT_COS_INST; //Nuevo: By EJ
            
            //FIN JAR
            ++i; arrParam[i].Value = item.SOLIC_DEUDA_CLIENTE; //PROY-29121
            Int64 nroSEC;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SECSI_INS_SOL_CORP_PDV";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[0];
                nroSEC = Funciones.CheckInt64(parSalida1.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }	

        public bool GrabarSolicitudRepLegal(BERepresentanteLegal item)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("K_RESULTADO", DbType.Int64,ParameterDirection.Output),
	            new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
	            new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String,16,ParameterDirection.Input),
	            new DAABRequest.Parameter("P_APODC_TIP_DOC_REP", DbType.String,2,ParameterDirection.Input),
	            new DAABRequest.Parameter("P_APODV_NUM_DOC_REP", DbType.String,20,ParameterDirection.Input),
	            new DAABRequest.Parameter("P_APODV_NOM_REP_LEG", DbType.String,40,ParameterDirection.Input),
	            new DAABRequest.Parameter("P_APODV_APA_REP_LEG", DbType.String,20,ParameterDirection.Input),
	            new DAABRequest.Parameter("P_APODV_AMA_REP_LEG", DbType.String,20,ParameterDirection.Input),
	            new DAABRequest.Parameter("P_APODV_CAR_REP", DbType.String,40,ParameterDirection.Input),
                new DAABRequest.Parameter("P_APODC_CODNACION", DbType.String,20,ParameterDirection.Input), //PROY-31636
	            new DAABRequest.Parameter("P_APODV_DESCNACION", DbType.String,80,ParameterDirection.Input), //PROY-31636
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = item.SOLIN_CODIGO;
            ++i; arrParam[i].Value = item.CLIEC_NUM_DOC;
            ++i; arrParam[i].Value = item.APODC_TIP_DOC_REP;
            ++i; arrParam[i].Value = item.APODV_NUM_DOC_REP;
            ++i; arrParam[i].Value = item.APODV_NOM_REP_LEG;
            ++i; arrParam[i].Value = item.APODV_APA_REP_LEG;
            ++i; arrParam[i].Value = item.APODV_AMA_REP_LEG;
            ++i; arrParam[i].Value = item.APODV_CAR_REP;
            ++i; arrParam[i].Value = item.SRLC_CODNACIONALIDAD; //PROY-31636
            ++i; arrParam[i].Value = item.SRLV_DESCNACIONALIDAD; //PROY-31636

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SECSI_INS_REP_LEGAL";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public bool GrabarComentario(BEComentario item)
        {
            DAABRequest.Parameter[] arrParam = {
		        new DAABRequest.Parameter("K_RESULTADO", DbType.String, ParameterDirection.Output),
		        new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int32, ParameterDirection.Input),
		        new DAABRequest.Parameter("P_COMEV_COMENTARIO", DbType.String,500, ParameterDirection.Input),
		        new DAABRequest.Parameter("P_COMEC_USU_REG", DbType.String, ParameterDirection.Input),
		        new DAABRequest.Parameter("P_COMEC_ESTADO", DbType.String, ParameterDirection.Input),
		        new DAABRequest.Parameter("P_COMEC_FLA_COM", DbType.String, ParameterDirection.Input),
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; if (item.SOLIN_CODIGO > 0) arrParam[i].Value = item.SOLIN_CODIGO;
            ++i; if (!string.IsNullOrEmpty(item.COMEV_COMENTARIO)) arrParam[i].Value = item.COMEV_COMENTARIO;
            ++i; if (!string.IsNullOrEmpty(item.COMEC_USU_REG)) arrParam[i].Value = item.COMEC_USU_REG;
            ++i; if (!string.IsNullOrEmpty(item.COMEC_ESTADO)) arrParam[i].Value = item.COMEC_ESTADO;
            ++i; if (!string.IsNullOrEmpty(item.COMEC_FLA_COM)) arrParam[i].Value = item.COMEC_FLA_COM;

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), item.SOLIN_CODIGO.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACTI_INS_COMENTARIO";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return salida;
        }	

        public bool GrabarArchivo(Int64 P_SOLIN_CODIGO, string P_ARCHV_NOM_ARC, string P_ARCHV_RUT_ARC, string P_ARCHC_USU_REG)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("K_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ARCHV_NOM_ARC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ARCHV_RUT_ARC", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ARCHC_USU_REG", DbType.String, 20, ParameterDirection.Input),
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = P_SOLIN_CODIGO;
            ++i; arrParam[i].Value = P_ARCHV_NOM_ARC;
            ++i; arrParam[i].Value = P_ARCHV_RUT_ARC;
            ++i; arrParam[i].Value = P_ARCHC_USU_REG;

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SECSI_INS_ARCHIVO";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public bool InsertarSolDireccion(BEDireccionCliente oDireccion, Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DIR", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PREFIJO", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PREFIJO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIRECCION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_PUERTA", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_EDIFICACION", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_EDIFICACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_MANZANA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_LOTE", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_INTERIOR", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_INTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_INTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_URBANIZACION", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_URBANIZACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TXT_URBANIZACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DOMICILIO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DOMICILIO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_ZONA", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ZONA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NOMBRE_ZONA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA_SEC", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DEPARTAMENTO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PROVINCIA", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DISTRITO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_POSTAL", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_UBIGEO", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_ID_UBIGEO_SGA", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_TELEFONO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_POBLADO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PLANO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIR_COMPLETA", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIRECCION_SAP", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA_SAP", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO_REF_1", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO_REF_2", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENTA_PROACTIVA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DNI_VENDEDOR", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENTA_PROGRAMADA", DbType.String, 2, ParameterDirection.Input),
//gaa20131002
				new DAABRequest.Parameter("P_ID_EDIFICIO", DbType.String, 10, ParameterDirection.Input)
//fin gaa20131002
			};
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = oDireccion.IdTipoDireccion;
            i++; arrParam[i].Value = oDireccion.IdPrefijo;
            i++; arrParam[i].Value = oDireccion.Prefijo;
            i++; arrParam[i].Value = oDireccion.Direccion;
            i++; arrParam[i].Value = oDireccion.NroPuerta;
            i++; arrParam[i].Value = oDireccion.IdEdificacion;
            i++; arrParam[i].Value = oDireccion.Edificacion;
            i++; arrParam[i].Value = oDireccion.Manzana;
            i++; arrParam[i].Value = oDireccion.Lote;
            i++; arrParam[i].Value = oDireccion.IdTipoInterior;
            i++; arrParam[i].Value = oDireccion.TipoInterior;
            i++; arrParam[i].Value = oDireccion.NroInterior;
            i++; arrParam[i].Value = oDireccion.IdUrbanizacion;
            i++; arrParam[i].Value = oDireccion.Urbanizacion;
            i++; arrParam[i].Value = oDireccion.TxtUrbanizacion;
            i++; arrParam[i].Value = oDireccion.IdDomicilio;
            i++; arrParam[i].Value = oDireccion.Domicilio;
            i++; arrParam[i].Value = oDireccion.IdZona;
            i++; arrParam[i].Value = oDireccion.Zona;
            i++; arrParam[i].Value = oDireccion.NombreZona;
            i++; arrParam[i].Value = oDireccion.Referencia;
            i++; arrParam[i].Value = oDireccion.Referencia_Sec;
            i++; arrParam[i].Value = oDireccion.IdDepartamento;
            i++; arrParam[i].Value = oDireccion.IdProvincia;
            i++; arrParam[i].Value = oDireccion.IdDistrito;
            i++; arrParam[i].Value = oDireccion.IdPostal;
            i++; arrParam[i].Value = oDireccion.IdUbigeo;
            i++; arrParam[i].Value = oDireccion.IdUbigeoSGA;
            i++; arrParam[i].Value = oDireccion.IdTelefono;
            i++; arrParam[i].Value = oDireccion.Telefono;
            i++; arrParam[i].Value = oDireccion.IdCentroPoblado;
            i++; arrParam[i].Value = oDireccion.IdPlano;
            i++; arrParam[i].Value = oDireccion.DirCompleta;
            i++; arrParam[i].Value = oDireccion.DirCompletaSAP;
            i++; arrParam[i].Value = oDireccion.DirReferenciaSAP;
            i++; arrParam[i].Value = oDireccion.TelefonoReferencia1;
            i++; arrParam[i].Value = oDireccion.TelefonoReferencia2;
            i++; arrParam[i].Value = oDireccion.VentaProactiva;
            i++; arrParam[i].Value = oDireccion.DniVendedor;
            i++; arrParam[i].Value = oDireccion.VentaProgramada;
            //gaa20131002
            i++; arrParam[i].Value = oDireccion.IdEdificio;
            //fin gaa20131002

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_INS_SOL_DIRECCION";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
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
            return salida;
        }

        public bool GrabarEvaluacionDatosCreditos(BEDatosCreditos item)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.String, 2, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_LC_CLIENTE", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_MONTO_X_PRODUCTO", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_MSJ_AUTONOMIA", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_MOTIVO", DbType.String, 500, ParameterDirection.Input),
				new DAABRequest.Parameter("P_RANGO_LC", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_LINEA", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_LINEA_7", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_LINEA_30", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_LINEA_90", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_LINEA_180", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_LINEA_MAY180", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_USUARIO_CREA", DbType.String, 10,ParameterDirection.Input)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = item.SOLIN_CODIGO;
            i++; arrParam[i].Value = item.LC_DISPONIBLE;
            i++; if (!string.IsNullOrEmpty(item.PRODUCTO_MONTO)) arrParam[i].Value = item.PRODUCTO_MONTO;
            i++; if (!string.IsNullOrEmpty(item.MSJ_AUTONOMIA)) arrParam[i].Value = item.MSJ_AUTONOMIA;
            i++; if (!string.IsNullOrEmpty(item.MOTIVO)) arrParam[i].Value = item.MOTIVO;
            i++; if (!string.IsNullOrEmpty(item.RANGO_LC_DISPONIBLE)) arrParam[i].Value = item.RANGO_LC_DISPONIBLE;
            i++; arrParam[i].Value = item.nroLineas;
            i++; arrParam[i].Value = item.nroLineaMenor7Dia;
            i++; arrParam[i].Value = item.nroLineaMenor30Dia;
            i++; arrParam[i].Value = item.nroLineaMenor90Dia;
            i++; arrParam[i].Value = item.nroLineaMenor180Dia;
            i++; arrParam[i].Value = item.nroLineaMayor180Dia;
            i++; if (item.USUARIO_CREA != "") arrParam[i].Value = item.USUARIO_CREA;

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), item.SOLIN_CODIGO.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_INS_DATOS_CREDITOS";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter pSalida1;
                pSalida1 = (IDataParameter)objRequest.Parameters[0];
                if (Funciones.CheckStr(pSalida1.Value) == "0")
                    salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        //PROY-24740
        public BEEvaluacion ObtenerComentarioSEC(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
			    new DAABRequest.Parameter("V_CODSEC", DbType.Int64, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output)
		    };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_EVALUACION_SEC + ".LISTAREVALUACIONPORCOD";
            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            BEEvaluacion objEval = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objEval = new BEEvaluacion();

                    objEval.cod_analista = Funciones.CheckStr(dr["cod_analista"]);
                    objEval.correcion_comentario_pdv = Funciones.CheckStr(dr["correccionpdv_com_pdv"]);
                    objEval.correcion_comentario_ca = Funciones.CheckStr(dr["correccionpdv_com_ca"]);
                    objEval.adjuntar_comentario_pdv = Funciones.CheckStr(dr["adjuntar_com_pdv"]);
                    objEval.adjuntar_comentario_ca = Funciones.CheckStr(dr["adjuntar_com_ca"]);
                    objEval.propuesta_comentario_pdv = Funciones.CheckStr(dr["propuesta_com_pdv"]);
                    objEval.propuesta_comentario_ca = Funciones.CheckStr(dr["propuesta_com_ca"]);
                    objEval.comentario_final_pdv = Funciones.CheckStr(dr["comentario_final_pdv"]);
                    objEval.comentario_final_ca = Funciones.CheckStr(dr["comentario_final_ca"]);
                    objEval.nueva_propuesta = (Funciones.CheckInt(dr["nueva_propuesta"].ToString()) == 1);
                    objEval.adjuntar_voucher = (Funciones.CheckInt(dr["adjuntar_voucher"].ToString()) == 1);
                    objEval.flag_evaluacion = Funciones.CheckStr(dr["flag_evaluacion"]);
                    objEval.comentario_final_del_pdv = Funciones.CheckStr(dr["comentario_final_pdv_s"]);
                    objEval.existe_rechazo = (Funciones.CheckInt(dr["correccion_pdv"].ToString()) == 1);
                    objEval.comentario_final_credito = Funciones.CheckStr(dr["comentario_final_credito"]);
                    objEval.comentario_despacho = Funciones.CheckStr(dr["comentario_despacho"]);
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();

                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objEval;
        }

        //PROY-24740
        public List<BEComentario> ObtenerComentarioSEC(Int64 nroSEC, string tipoComentario)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_COMEC_FLA_COM", DbType.String,ParameterDirection.Input),												   
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (nroSEC > 0) arrParam[0].Value = nroSEC;
            if (!string.IsNullOrEmpty(tipoComentario)) arrParam[1].Value = tipoComentario;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACTS_CON_COMENTARIO";
            objRequest.Parameters.AddRange(arrParam);

            BEComentario objItem = null;
            List<BEComentario> objLista = new List<BEComentario>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while(dr.Read())
                {
                    objItem = new BEComentario();
                    objItem.COMEC_CODIGO = Funciones.CheckInt(dr["COMEC_CODIGO"]);
                    objItem.COMEC_ESTADO = Funciones.CheckStr(dr["COMEC_ESTADO"]);
                    objItem.COMEC_FLA_COM = Funciones.CheckStr(dr["COMEC_FLA_COM"]);
                    objItem.COMEC_USU_REG = Funciones.CheckStr(dr["COMEC_USU_REG"]);
                    objItem.COMED_FEC_REG = Funciones.CheckDate(dr["COMED_FEC_REG"]);
                    objItem.COMEV_COMENTARIO = Funciones.CheckStr(dr["COMEV_COMENTARIO"]);
                    objItem.SOLIN_CODIGO = Funciones.CheckInt(dr["SOLIN_CODIGO"]);
                    objItem.COMEC_FLA_COM_DES = Funciones.CheckStr(dr["TABLN_DESCRIPCION"]);

                    objLista.Add(objItem);
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public DataTable ObtenerCostoInstalacion(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
            };
            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_COSTO_INSTALACION";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
        }

        //INICIO: Nuevo by EJ
        public DataTable ObtenerCostoInstalacionHFC(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64, ParameterDirection.Input),
               // new DAABRequest.Parameter("P_CODPROD", DbType.String,5, ParameterDirection.Input),
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
            };
            arrParam[0].Value = nroSEC;
            //arrParam[1].Value = codProd;

            objLog.CrearArchivolog("[INICIO][ObtenerCostoInstalacion]", null, null);
            objLog.CrearArchivolog("[Entrada][SEC]", nroSEC.ToString(), null);
           // objLog.CrearArchivolog("[Entrada][codProd]", codProd.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_COSTO_INSTALACION_HFC";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ObtenerCostoInstalacion]", null, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("[SALIDA][ObtenerCostoInstalacion]", null, null);
                objRequest.Factory.Dispose();
            }
        }

        public void Insertar_Correccion_Nombres(List<string> oItem)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_TIPO_DOCUMENTO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_DOCUMENTO", DbType.String, 11, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NOMBRE_ANTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_AP_PAT_ANTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_AP_MAT_ANTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NOMBRE_NUEVO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_AP_PAT_NUEVO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_AP_MAT_NUEVO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TERMINAL", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_USU_CREA", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIED_FEC_NAC", DbType.Date, ParameterDirection.Input)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = oItem[0];
            arrParam[1].Value = oItem[1];
            arrParam[2].Value = oItem[2];
            arrParam[3].Value = oItem[3];
            arrParam[4].Value = oItem[4];
            arrParam[5].Value = oItem[5];
            arrParam[6].Value = oItem[6];
            arrParam[7].Value = oItem[7];
            arrParam[8].Value = Funciones.CheckInt64(oItem[8]);
            arrParam[9].Value = oItem[9];
            arrParam[10].Value = oItem[10];
            arrParam[11].Value = Funciones.CheckDate(oItem[11]);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SP_ACTUALIZAR_NOMBRES_DC";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
                objRequest.Parameters.Clear();
            }
        }

        public void AprobarCreditos(List<string> oItem)
        {
            objLog.CrearArchivolog("[ENTRADA][AprobarCreditos]", null, null);
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("p_solin_grupo_sec", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_portabilidad", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("p_flg_convergente", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("p_comentario_al_pdv", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("p_comentario_eval", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("p_lista_garantia", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("p_lista_costo_instal", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("p_usuario", DbType.String, ParameterDirection.Input)
			};
            int i = 0;
            i = 0; arrParam[i].Value = Funciones.CheckInt64(oItem[i]);
            objLog.CrearArchivolog("p_solin_grupo_sec arrParam - " + Funciones.CheckStr(i), Funciones.CheckStr(arrParam[i].Value), null);
            i++; arrParam[i].Value = oItem[i];
            objLog.CrearArchivolog("p_portabilidad arrParam - " + Funciones.CheckStr(i), Funciones.CheckStr(arrParam[i].Value), null);
            i++; arrParam[i].Value = oItem[i];
            objLog.CrearArchivolog("p_flg_convergente arrParam - " + Funciones.CheckStr(i), Funciones.CheckStr(arrParam[i].Value), null);
            i++; arrParam[i].Value = oItem[i];
            objLog.CrearArchivolog("p_comentario_al_pdv arrParam - " + Funciones.CheckStr(i), Funciones.CheckStr(arrParam[i].Value), null);
            i++; arrParam[i].Value = oItem[i];
            objLog.CrearArchivolog("p_comentario_eval arrParam - " + Funciones.CheckStr(i), Funciones.CheckStr(arrParam[i].Value), null);
            i++; arrParam[i].Value = oItem[i];
            objLog.CrearArchivolog("p_lista_garantia arrParam - " + Funciones.CheckStr(i), Funciones.CheckStr(arrParam[i].Value), null);
            i++; arrParam[i].Value = oItem[i];
            objLog.CrearArchivolog("p_lista_costo_instal arrParam - " + Funciones.CheckStr(i), Funciones.CheckStr(arrParam[i].Value), null);
            i++; arrParam[i].Value = oItem[i];
            objLog.CrearArchivolog("p_usuario arrParam - " + Funciones.CheckStr(i), Funciones.CheckStr(arrParam[i].Value), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_APROBAR_CREDITOS";
            objRequest.Parameters.AddRange(arrParam);
            objLog.CrearArchivolog("[PROCEDURE] ", BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_APROBAR_CREDITOS", null);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[Exception][AprobarCreditos] ", ex.Message, null);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("[SALIDA][AprobarCreditos] ", null, null);
                objRequest.Factory.Dispose();
                objRequest.Parameters.Clear();
            }
        }

        public void ActualizarGarantia(List<string> oItem)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("p_solin_grupo_sec", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_flg_convergente", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("p_lista_garantia", DbType.String, 4000, ParameterDirection.Input),
				new DAABRequest.Parameter("p_lista_costo_instal", DbType.String, 4000, ParameterDirection.Input),
				new DAABRequest.Parameter("p_usuario", DbType.String, ParameterDirection.Input)
			};
            int i = 0;
            i = 0; arrParam[i].Value = Funciones.CheckInt64(oItem[i]);
            i++; arrParam[i].Value = oItem[i];
            i++; arrParam[i].Value = oItem[i];
            i++; arrParam[i].Value = oItem[i];
            i++; arrParam[i].Value = oItem[i];

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".sp_actualizar_garantia";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
                objRequest.Parameters.Clear();
            }
        }

        public void ActualizarReingresoSEC(Int64 nroSEC, string flgReingresoSEC)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("p_solin_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_flag_reingreso_sec", DbType.String, ParameterDirection.Input)
			};
            int i = 0;
            i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = flgReingresoSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".sp_act_reingreso_sec";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
                objRequest.Parameters.Clear();
            }
        }

        public void AprobarSustentoIngreso(Int64 nroSEC, string usuario, double ingreso)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("K_RESULTADO", DbType.Int64,ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_USU_CRE", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_FLAG_ENVIO", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_ING_SUS", DbType.Double,ParameterDirection.Input)
            };
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = usuario;
            i++; arrParam[i].Value = "1";
            i++; arrParam[i].Value = ingreso;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACU_ACT_SOL_TERMINAR_CONS";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
        }

        public DataSet ObtenerDetalleSECPendiente(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("p_cur_sec", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("p_cur_plan", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("p_cur_servicio", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("p_solin_grupo_sec", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_ovenc_codigo", DbType.String, 4, ParameterDirection.Input)
            };
            arrParam[3].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_SEC_PENDIENTE";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
        }


        public void GrabarSOTMigracionHFC(Int64 nroSEC, string nroSOT)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_SOT_MIG", DbType.String, 10, ParameterDirection.Input)
            };
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = nroSOT;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_SOLICITUD_CONS + ".SP_INS_SOT_MIGRACION";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
        }

        public List<BEEstado> ObtenerEstadoSot(Int64 nroSEC, Int64 nroSot)
        {
            DAABRequest.Parameter[] arrParam = {	
				new DAABRequest.Parameter("av_numsec", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("an_solot", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("ac_status_sot", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("an_resultado", DbType.Int32, ParameterDirection.Output),
				new DAABRequest.Parameter("av_mensaje", DbType.String, 500, ParameterDirection.Output)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = Funciones.CheckStr(nroSEC);
            arrParam[1].Value = nroSot;

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_CONVERGENTE + ".p_estado_sot";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEEstado> objFilas = new List<BEEstado>();
            BEEstado objItem;
            try
            {
                DataTable dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEEstado();
                    objItem.ESTAC_CODIGO = Funciones.CheckStr(dr["estsol"]);
                    objItem.ESTAV_DESCRIPCION = Funciones.CheckStr(dr["descripcion"]);
                    objFilas.Add(objItem);
                }
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
            return objFilas;
        }

        public BEAgendamiento ObtenerAgendamientoSga(Int64 nroSot)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("p_codsolot", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("ac_fec_contratista", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("p_cod_resp", DbType.Int32, ParameterDirection.Output),
				new DAABRequest.Parameter("p_msg_resp", DbType.String, 500, ParameterDirection.Output)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSot;

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_CONVERGENTE + ".p_consulta_fec_contratista";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            BEAgendamiento objItem = new BEAgendamiento();
            try
            {
                DataTable dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem.IdContratista = Funciones.CheckStr(dr["CODCON"]);
                    objItem.Contratista = Funciones.CheckStr(dr["NOMBRE"]);
                    objItem.Fecha = Funciones.CheckStr(dr["FECHA"]);
                    objItem.Hora = Funciones.CheckStr(dr["HORA"]);
                }
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
            return objItem;
        }
        public string ActualizaDirreccionSga(Int64 nroSot, BEDireccionCliente oDireccion, ref string msgResp)
        {
            DAABRequest.Parameter[] arrParam = {	
                new DAABRequest.Parameter("p_codsolot", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tipvia_i", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("p_nomvia_i", DbType.String, 200, ParameterDirection.Input),
                new DAABRequest.Parameter("p_nrovia_i", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("p_tipourb_i", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("p_nomurb_i", DbType.String, 200, ParameterDirection.Input),
                new DAABRequest.Parameter("p_manzana_i", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("p_lote_i", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("p_codubi", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("p_referencia_i", DbType.String, 400, ParameterDirection.Input),
                new DAABRequest.Parameter("p_idplano", DbType.String, 100, ParameterDirection.Input),                
				new DAABRequest.Parameter("p_cod_resp", DbType.Int32, ParameterDirection.Output),
				new DAABRequest.Parameter("p_msg_resp", DbType.String, 1000, ParameterDirection.Output)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;


            i = 0; arrParam[i].Value = nroSot;
            i++; arrParam[i].Value = oDireccion.IdPrefijoSga;
            i++; arrParam[i].Value = (oDireccion.Direccion == "") ? "-" : oDireccion.Direccion;
            i++; arrParam[i].Value = oDireccion.NroPuerta;
            i++; arrParam[i].Value = oDireccion.IdUrbanizacionSga;
            i++; arrParam[i].Value = oDireccion.Urbanizacion;
            i++; arrParam[i].Value = oDireccion.Manzana;
            i++; arrParam[i].Value = oDireccion.Lote;
            i++; arrParam[i].Value = oDireccion.IdUbigeoInei;
            i++; arrParam[i].Value = oDireccion.Referencia + " " + oDireccion.Referencia_Sec;
            i++; arrParam[i].Value = oDireccion.IdPlano;

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_CONVERGENTE + ".p_actualizar_direccion";
            objRequest.Transactional = true;
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            string CodResp = "";
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
                CodResp = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[11]).Value);
                msgResp = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[12]).Value);
            }
            catch (Exception ex)
            {
                objRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return CodResp;
        }

        //PROY-24740
        public static void ObtenerAcuerdosBySec(Int64 nroSec, ref BEAcuerdo beAcuerdo, ref List<BEAcuerdoDetalle> listAcuerdoDetalle, ref List<BEAcuerdoServicio> listAcuerdoServicio)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("p_solin_codigo", DbType.Int64,ParameterDirection.Input),
                                                   new DAABRequest.Parameter("p_cod_resp", DbType.String,20,ParameterDirection.Output),
                                                   new DAABRequest.Parameter("p_msg_resp", DbType.String,500,ParameterDirection.Output),
												   new DAABRequest.Parameter("c_contrato", DbType.Object,ParameterDirection.Output),
												   new DAABRequest.Parameter("c_contrato_det", DbType.Object,ParameterDirection.Output),
                                                   new DAABRequest.Parameter("c_contrato_serv", DbType.Object,ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSec;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_ACUERDO + ".sp_con_acuerdos_x_sec";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;

                string CodResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[1]).Value);
                if (CodResp == "0")
                {
                    while(dr.Read())
                    {
                        BEAcuerdo oAcuerdo = new BEAcuerdo();
                        oAcuerdo.IdContrato = Funciones.CheckInt64(dr["CONTN_NUMERO_CONTRATO"]);
                        oAcuerdo.CodPuntoVenta = Funciones.CheckStr(dr["CONTC_OFICINA_VENTA"]);
                        oAcuerdo.DesPuntoVenta = Funciones.CheckStr(dr["CONTV_OFICINA_VENTA_DESC"]);
                        oAcuerdo.NumeroPCS = Funciones.CheckStr(dr["CONTV_NUMERO_PCS"]);
                        oAcuerdo.FechaContrato = Funciones.CheckStr(dr["CONTD_FECHA_CONTRATO"]);
                        oAcuerdo.Solin_codigo = Funciones.CheckInt64(dr["CONTN_NUMERO_SEC"]);
                        oAcuerdo.Resultado = Funciones.CheckStr(dr["CONTV_RESULTADO"]);
                        oAcuerdo.CodVendedor = Funciones.CheckStr(dr["CONTV_CODIGO_VENDEDOR"]);
                        oAcuerdo.LimiteCredito = Funciones.CheckStr(dr["CONTN_LIM_CREDITO"]);
                        oAcuerdo.ScoreCrediticio = Funciones.CheckStr(dr["CONTC_SCORE_CREDITICIO"]);
                        oAcuerdo.ControlFraude = Funciones.CheckStr(dr["CONTC_CONTROL_FRAUDE"]);
                        oAcuerdo.NroDocCliente = Funciones.CheckStr(dr["CONTV_NRO_DOC_CLIENTE"]);
                        oAcuerdo.TipoDocCliente = Funciones.CheckStr(dr["CONTC_TIPO_DOC_CLIENTE"]);
                        oAcuerdo.DesTipoDocCliente = Funciones.CheckStr(dr["TDOCV_DESCRIPCION"]);
                        oAcuerdo.CodTipoVenta = Funciones.CheckStr(dr["CONTC_TIPO_VENTA"]);
                        oAcuerdo.DesTipoVenta = Funciones.CheckStr(dr["TVENV_DESCRIPCION"]);
                        oAcuerdo.CodTipoOperacion = Funciones.CheckStr(dr["CONTC_TIPO_OPERACION"]);
                        oAcuerdo.DesTipoOperacion = Funciones.CheckStr(dr["TOPEV_DESCRIPCION"]);
                        oAcuerdo.Usuario = Funciones.CheckStr(dr["CONTV_USUARIO_CREACION"]);
                        oAcuerdo.CargoFijoTotal = Funciones.CheckStr(dr["CONTN_CF_TOTAL"]);
                        oAcuerdo.CodTipoClienteActivacion = Funciones.CheckStr(dr["CONTC_TIPO_CLIENTE_ACTIV"]);
                        oAcuerdo.DesTipoClienteActivacion = Funciones.CheckStr(dr["DESC_TIPO_CLIENTE"]);
                        oAcuerdo.NombreVendedor = Funciones.CheckStr(dr["CONTV_VENDEDOR"]);
                        oAcuerdo.NombreCliente = Funciones.CheckStr(dr["CONTV_NOMBRE"]);
                        oAcuerdo.ApellidoPatCliente = Funciones.CheckStr(dr["CONTV_APE_PAT"]);
                        oAcuerdo.ApellidoMatCliente = Funciones.CheckStr(dr["CONTV_APE_MAT"]);
                        oAcuerdo.RazonSocial = Funciones.CheckStr(dr["CONTV_RAZONSOCIAL"]);
                        oAcuerdo.AnalistaCredito = Funciones.CheckStr(dr["CONTV_ANALISTA_CRED"]);
                        oAcuerdo.Aprobador = Funciones.CheckStr(dr["CONTV_APROBADOR"]);
                        beAcuerdo = oAcuerdo;
                    }

                    dr.NextResult();

                    while(dr.Read())
                    {
                        BEAcuerdoDetalle oAcuerdoDetalle = new BEAcuerdoDetalle();
                        oAcuerdoDetalle.IdContrato = Funciones.CheckInt64(dr["ID_CONTRATO"]);
                        oAcuerdoDetalle.Material = Funciones.CheckStr(dr["MATERIAL"]);
                        oAcuerdoDetalle.Material_des = Funciones.CheckStr(dr["MATERIAL_DES"]);
                        oAcuerdoDetalle.Campana = Funciones.CheckStr(dr["CAMPANA"]);
                        oAcuerdoDetalle.Campana_desc = Funciones.CheckStr(dr["CAMPANA_DESC"]);
                        oAcuerdoDetalle.Plan_tarifar = Funciones.CheckStr(dr["PLAN_TARIFAR"]);
                        oAcuerdoDetalle.Plan_tarifar_desc = Funciones.CheckStr(dr["PLAN_TARIFAR_DESC"]);
                        oAcuerdoDetalle.Precio_lista = Funciones.CheckDbl(dr["PRECIO_LISTA"]);
                        oAcuerdoDetalle.Precio_venta = Funciones.CheckDbl(dr["PRECIO_VENTA"]);
                        oAcuerdoDetalle.Telefono = Funciones.CheckStr(dr["TELEFONO"]);
                        oAcuerdoDetalle.Co_id = Funciones.CheckInt64(dr["CO_ID"]);
                        oAcuerdoDetalle.IdDetalle = Funciones.CheckInt(dr["CORRELATIVO"]);
                        oAcuerdoDetalle.Imei19 = Funciones.CheckStr(dr["IMEI19"]);
                        oAcuerdoDetalle.Utilizacion = Funciones.CheckStr(dr["UTILIZACION"]);
                        oAcuerdoDetalle.Des_utilizacion = Funciones.CheckStr(dr["DES_UTILIZACION"]);
                        oAcuerdoDetalle.Descuento = Funciones.CheckDbl(dr["DESCUENTO"]);
                        oAcuerdoDetalle.Impuesto = Funciones.CheckDbl(dr["IMPUESTO"]);
                        oAcuerdoDetalle.Cod_equipo = Funciones.CheckStr(dr["COD_EQUIPO"]);
                        oAcuerdoDetalle.Des_equipo = Funciones.CheckStr(dr["DES_EQUIPO"]);
                        oAcuerdoDetalle.Serie_equipo = Funciones.CheckStr(dr["SERIE_EQUIPO"]);
                        oAcuerdoDetalle.Principal = Funciones.CheckStr(dr["PRINCIPAL"]);
                        oAcuerdoDetalle.Prdc_codigo = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                        oAcuerdoDetalle.Pacuc_codigo = Funciones.CheckStr(dr["PACUC_CODIGO"]);
                        oAcuerdoDetalle.Pacuv_descripcion = Funciones.CheckStr(dr["PACUV_DESCRIPCION"]);
                        oAcuerdoDetalle.NroRecibo = Funciones.CheckInt(dr["RECIBO"]);
                        oAcuerdoDetalle.Sub_contrato = Funciones.CheckInt64(dr["SUB_CONTRATO"]);
                        oAcuerdoDetalle.Solin_codigo = Funciones.CheckInt64(dr["SOLIN_CODIGO"]);
                        oAcuerdoDetalle.NroSot = Funciones.CheckInt64(dr["nro_sot"]);
                        listAcuerdoDetalle.Add(oAcuerdoDetalle);
                    }
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }

        public static string AnularSot(Int64 nroSot, ref string mensaje)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("p_codsolot", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_cod_resp", DbType.Int32, ParameterDirection.Output),
				new DAABRequest.Parameter("p_msg_resp", DbType.String, 500, ParameterDirection.Output)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSot;

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_CONVERGENTE + ".p_anular_solot";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;
            string CodResp = "";
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                CodResp = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[1]).Value);
                mensaje = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[2]).Value);
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
            return CodResp;
        }

        public static bool AnularSEC(string pstrSEC, string pstrNroDocumento, string pstrCodUsuario)
        {
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_USU_CRE", DbType.String, ParameterDirection.Input)
											   };
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = pstrSEC;
            ++i; arrParam[i].Value = pstrNroDocumento;
            ++i; arrParam[i].Value = pstrCodUsuario;

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_DTH + ".MANTSM_ANULAR_SEC";
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                obRequest.Factory.Dispose();
            }
            return salida;
        }
        public bool InsertarSolDireccionVenta(BEDireccionCliente oDireccion, Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DIR", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PREFIJO", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PREFIJO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIRECCION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_PUERTA", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_EDIFICACION", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_EDIFICACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_MANZANA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_LOTE", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_INTERIOR", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_INTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_INTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_URBANIZACION", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_URBANIZACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TXT_URBANIZACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DOMICILIO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DOMICILIO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_ZONA", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ZONA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NOMBRE_ZONA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA_SEC", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DEPARTAMENTO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PROVINCIA", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DISTRITO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_POSTAL", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_UBIGEO", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_ID_UBIGEO_SGA", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_TELEFONO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_POBLADO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PLANO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIR_COMPLETA", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIRECCION_SAP", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA_SAP", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO_REF_1", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO_REF_2", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENTA_PROACTIVA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DNI_VENDEDOR", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENTA_PROGRAMADA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_EDIFICIO", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_RECIBO", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CORREO", DbType.String, 100, ParameterDirection.Input)
			};
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = oDireccion.IdTipoDireccion;
            i++; arrParam[i].Value = oDireccion.IdPrefijo;
            i++; arrParam[i].Value = oDireccion.Prefijo;
            i++; arrParam[i].Value = oDireccion.Direccion;
            i++; arrParam[i].Value = oDireccion.NroPuerta;
            i++; arrParam[i].Value = oDireccion.IdEdificacion;
            i++; arrParam[i].Value = oDireccion.Edificacion;
            i++; arrParam[i].Value = oDireccion.Manzana;
            i++; arrParam[i].Value = oDireccion.Lote;
            i++; arrParam[i].Value = oDireccion.IdTipoInterior;
            i++; arrParam[i].Value = oDireccion.TipoInterior;
            i++; arrParam[i].Value = oDireccion.NroInterior;
            i++; arrParam[i].Value = oDireccion.IdUrbanizacion;
            i++; arrParam[i].Value = oDireccion.Urbanizacion;
            i++; arrParam[i].Value = oDireccion.TxtUrbanizacion;
            i++; arrParam[i].Value = oDireccion.IdDomicilio;
            i++; arrParam[i].Value = oDireccion.Domicilio;
            i++; arrParam[i].Value = oDireccion.IdZona;
            i++; arrParam[i].Value = oDireccion.Zona;
            i++; arrParam[i].Value = oDireccion.NombreZona;
            i++; arrParam[i].Value = oDireccion.Referencia;
            i++; arrParam[i].Value = oDireccion.Referencia_Sec;
            i++; arrParam[i].Value = oDireccion.IdDepartamento;
            i++; arrParam[i].Value = oDireccion.IdProvincia;
            i++; arrParam[i].Value = oDireccion.IdDistrito;
            i++; arrParam[i].Value = oDireccion.IdPostal;
            i++; arrParam[i].Value = oDireccion.IdUbigeo;
            i++; arrParam[i].Value = oDireccion.IdUbigeoSGA;
            i++; arrParam[i].Value = oDireccion.IdTelefono;
            i++; arrParam[i].Value = oDireccion.Telefono;
            i++; arrParam[i].Value = oDireccion.IdCentroPoblado;
            i++; arrParam[i].Value = oDireccion.IdPlano;
            i++; arrParam[i].Value = oDireccion.DirCompleta;
            i++; arrParam[i].Value = oDireccion.DirCompletaSAP;
            i++; arrParam[i].Value = oDireccion.DirReferenciaSAP;
            i++; arrParam[i].Value = oDireccion.TelefonoReferencia1;
            i++; arrParam[i].Value = oDireccion.TelefonoReferencia2;
            i++; arrParam[i].Value = oDireccion.VentaProactiva;
            i++; arrParam[i].Value = oDireccion.DniVendedor;
            i++; arrParam[i].Value = oDireccion.VentaProgramada;
            i++; arrParam[i].Value = oDireccion.IdEdificio;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactVentasExpress3Play + ".SISACT_INS_SOL_DIRECCION";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
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
            return salida;
        }

public bool InsertarSolDireccion_LTE(BEDireccionCliente oDireccion, Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DIR", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PREFIJO", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PREFIJO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIRECCION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_PUERTA", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_EDIFICACION", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_EDIFICACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_MANZANA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_LOTE", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_INTERIOR", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_INTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_INTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_URBANIZACION", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_URBANIZACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TXT_URBANIZACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DOMICILIO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DOMICILIO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_ZONA", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ZONA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NOMBRE_ZONA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA_SEC", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DEPARTAMENTO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PROVINCIA", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DISTRITO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_POSTAL", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_UBIGEO", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_ID_UBIGEO_SGA", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_TELEFONO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_POBLADO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PLANO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIR_COMPLETA", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIRECCION_SAP", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA_SAP", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO_REF_1", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO_REF_2", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENTA_PROACTIVA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DNI_VENDEDOR", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENTA_PROGRAMADA", DbType.String, 2, ParameterDirection.Input),
//gaa20131002
				new DAABRequest.Parameter("P_ID_EDIFICIO", DbType.String, 10, ParameterDirection.Input),
//fin gaa20131002
                new DAABRequest.Parameter("p_cobertura_dth", DbType.Int32, ParameterDirection.Input),
                new DAABRequest.Parameter("p_cobertura_lte", DbType.Int32, ParameterDirection.Input)
			};
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = oDireccion.IdTipoDireccion;
            i++; arrParam[i].Value = oDireccion.IdPrefijo;
            i++; arrParam[i].Value = oDireccion.Prefijo;
            i++; arrParam[i].Value = oDireccion.Direccion;
            i++; arrParam[i].Value = oDireccion.NroPuerta;
            i++; arrParam[i].Value = oDireccion.IdEdificacion;
            i++; arrParam[i].Value = oDireccion.Edificacion;
            i++; arrParam[i].Value = oDireccion.Manzana;
            i++; arrParam[i].Value = oDireccion.Lote;
            i++; arrParam[i].Value = oDireccion.IdTipoInterior;
            i++; arrParam[i].Value = oDireccion.TipoInterior;
            i++; arrParam[i].Value = oDireccion.NroInterior;
            i++; arrParam[i].Value = oDireccion.IdUrbanizacion;
            i++; arrParam[i].Value = oDireccion.Urbanizacion;
            i++; arrParam[i].Value = oDireccion.TxtUrbanizacion;
            i++; arrParam[i].Value = oDireccion.IdDomicilio;
            i++; arrParam[i].Value = oDireccion.Domicilio;
            i++; arrParam[i].Value = oDireccion.IdZona;
            i++; arrParam[i].Value = oDireccion.Zona;
            i++; arrParam[i].Value = oDireccion.NombreZona;
            i++; arrParam[i].Value = oDireccion.Referencia;
            i++; arrParam[i].Value = oDireccion.Referencia_Sec;
            i++; arrParam[i].Value = oDireccion.IdDepartamento;
            i++; arrParam[i].Value = oDireccion.IdProvincia;
            i++; arrParam[i].Value = oDireccion.IdDistrito;
            i++; arrParam[i].Value = oDireccion.IdPostal;
            i++; arrParam[i].Value = oDireccion.IdUbigeo;
            i++; arrParam[i].Value = oDireccion.IdUbigeoSGA;
            i++; arrParam[i].Value = oDireccion.IdTelefono;
            i++; arrParam[i].Value = oDireccion.Telefono;
            i++; arrParam[i].Value = oDireccion.IdCentroPoblado;
            i++; arrParam[i].Value = oDireccion.IdPlano;
            i++; arrParam[i].Value = oDireccion.DirCompleta;
            i++; arrParam[i].Value = oDireccion.DirCompletaSAP;
            i++; arrParam[i].Value = oDireccion.DirReferenciaSAP;
            i++; arrParam[i].Value = oDireccion.TelefonoReferencia1;
            i++; arrParam[i].Value = oDireccion.TelefonoReferencia2;
            i++; arrParam[i].Value = oDireccion.VentaProactiva;
            i++; arrParam[i].Value = oDireccion.DniVendedor;
            i++; arrParam[i].Value = oDireccion.VentaProgramada;
            //gaa20131002
            i++; arrParam[i].Value = oDireccion.IdEdificio;
            //fin gaa20131002
            
            i++; arrParam[i].Value = oDireccion.Cobertura_dth;
            i++; arrParam[i].Value = oDireccion.Cobertura_lte;

            objLog.CrearArchivolog("[INICIO][InsertarSolDireccion_LTE]", null, null);
            objLog.CrearArchivolog("[Entrada][nroSEC]", nroSEC.ToString(), null);
            objLog.CrearArchivolog("[Entrada][Cobertura_dth]", oDireccion.Cobertura_dth.ToString(), null);
            objLog.CrearArchivolog("[Entrada][Cobertura_lte]", oDireccion.Cobertura_lte.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            //objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_INS_SOL_DIRECCION";
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_INS_SOL_DIRECCION_LTE";  //MAC maquino
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][InsertarSolDireccion_LTE]", null, ex);
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();               
            }
            objLog.CrearArchivolog("[SALIDA][InsertarSolDireccion_LTE]", null, null);
            return salida;
        }

        public bool InsertarSolDireccionVenta_LTE(BEDireccionCliente oDireccion, Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DIR", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PREFIJO", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PREFIJO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIRECCION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_PUERTA", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_EDIFICACION", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_EDIFICACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_MANZANA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_LOTE", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_INTERIOR", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_INTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_INTERIOR", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_URBANIZACION", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_URBANIZACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TXT_URBANIZACION", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DOMICILIO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DOMICILIO", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_ZONA", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ZONA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NOMBRE_ZONA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA_SEC", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DEPARTAMENTO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PROVINCIA", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_DISTRITO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_POSTAL", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_UBIGEO", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_ID_UBIGEO_SGA", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_TELEFONO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_POBLADO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_PLANO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIR_COMPLETA", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DIRECCION_SAP", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_REFERENCIA_SAP", DbType.String, 200, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO_REF_1", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TELEFONO_REF_2", DbType.String, 9, ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENTA_PROACTIVA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DNI_VENDEDOR", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENTA_PROGRAMADA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_EDIFICIO", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_RECIBO", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CORREO", DbType.String, 100, ParameterDirection.Input),
                new DAABRequest.Parameter("p_cobertura_dth", DbType.Int32, ParameterDirection.Input),
                new DAABRequest.Parameter("p_cobertura_lte", DbType.Int32, ParameterDirection.Input)
			};
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = oDireccion.IdTipoDireccion;
            i++; arrParam[i].Value = oDireccion.IdPrefijo;
            i++; arrParam[i].Value = oDireccion.Prefijo;
            i++; arrParam[i].Value = oDireccion.Direccion;
            i++; arrParam[i].Value = oDireccion.NroPuerta;
            i++; arrParam[i].Value = oDireccion.IdEdificacion;
            i++; arrParam[i].Value = oDireccion.Edificacion;
            i++; arrParam[i].Value = oDireccion.Manzana;
            i++; arrParam[i].Value = oDireccion.Lote;
            i++; arrParam[i].Value = oDireccion.IdTipoInterior;
            i++; arrParam[i].Value = oDireccion.TipoInterior;
            i++; arrParam[i].Value = oDireccion.NroInterior;
            i++; arrParam[i].Value = oDireccion.IdUrbanizacion;
            i++; arrParam[i].Value = oDireccion.Urbanizacion;
            i++; arrParam[i].Value = oDireccion.TxtUrbanizacion;
            i++; arrParam[i].Value = oDireccion.IdDomicilio;
            i++; arrParam[i].Value = oDireccion.Domicilio;
            i++; arrParam[i].Value = oDireccion.IdZona;
            i++; arrParam[i].Value = oDireccion.Zona;
            i++; arrParam[i].Value = oDireccion.NombreZona;
            i++; arrParam[i].Value = oDireccion.Referencia;
            i++; arrParam[i].Value = oDireccion.Referencia_Sec;
            i++; arrParam[i].Value = oDireccion.IdDepartamento;
            i++; arrParam[i].Value = oDireccion.IdProvincia;
            i++; arrParam[i].Value = oDireccion.IdDistrito;
            i++; arrParam[i].Value = oDireccion.IdPostal;
            i++; arrParam[i].Value = oDireccion.IdUbigeo;
            i++; arrParam[i].Value = oDireccion.IdUbigeoSGA;
            i++; arrParam[i].Value = oDireccion.IdTelefono;
            i++; arrParam[i].Value = oDireccion.Telefono;
            i++; arrParam[i].Value = oDireccion.IdCentroPoblado;
            i++; arrParam[i].Value = oDireccion.IdPlano;
            i++; arrParam[i].Value = oDireccion.DirCompleta;
            i++; arrParam[i].Value = oDireccion.DirCompletaSAP;
            i++; arrParam[i].Value = oDireccion.DirReferenciaSAP;
            i++; arrParam[i].Value = oDireccion.TelefonoReferencia1;
            i++; arrParam[i].Value = oDireccion.TelefonoReferencia2;
            i++; arrParam[i].Value = oDireccion.VentaProactiva;
            i++; arrParam[i].Value = oDireccion.DniVendedor;
            i++; arrParam[i].Value = oDireccion.VentaProgramada;
            i++; arrParam[i].Value = oDireccion.IdEdificio;
            i++; arrParam[i].Value = oDireccion.Cobertura_dth;
            i++; arrParam[i].Value = oDireccion.Cobertura_lte;

            objLog.CrearArchivolog("[INICIO][InsertarSolDireccionVenta_LTE]", null, null);
            objLog.CrearArchivolog("[Entrada][nroSEC]", nroSEC.ToString(), null);
            objLog.CrearArchivolog("[Entrada][Cobertura_dth]", oDireccion.Cobertura_dth.ToString(), null);
            objLog.CrearArchivolog("[Entrada][Cobertura_lte]", oDireccion.Cobertura_lte.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            //objRequest.Command = BaseDatos.PkgSisactVentasExpress3Play + ".SISACT_INS_SOL_DIRECCION";
            objRequest.Command = BaseDatos.PkgSisactVentasExpress3Play + ".SISACT_INS_SOL_DIRECCION_LTE";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][InsertarSolDireccionVenta_LTE]", null, null);
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[SALIDA][InsertarSolDireccionVenta_LTE]", null, null);
            return salida;
        }
        public static void verificarPago(Int64 nroSec, ref string p_resultado)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("p_solin_codigo", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("p_resultado", DbType.String, ParameterDirection.Output)            
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSec;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_DRA_CVE + ".sisacss_verificar_pago";
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                obRequest.Factory.ExecuteReader(ref obRequest);

                IDataParameter pSalida;
                pSalida = (IDataParameter)obRequest.Parameters[1];
                p_resultado = Funciones.CheckStr(pSalida.Value);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //if (dr != null && dr.IsClosed==false ) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }        

        //PROY-29121-INI
        public static List<BEEmpresaCategoria> ListaEmpresaReferencia(string tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String,2,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_NUM_DOC", DbType.String,20,ParameterDirection.Input),
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
											   };


            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = tipoDocumento;
            arrParam[1].Value = nroDocumento;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACT_CON_EMPRESA";
            obRequest.Parameters.AddRange(arrParam);

            List<BEEmpresaCategoria> filas = new List<BEEmpresaCategoria>();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEEmpresaCategoria item = new BEEmpresaCategoria();

                    item.FLAG_TIPO = Funciones.CheckStr(dr["EMPV_FLAG_TIPO"]);
                    item.CATEGORIA_ID = Funciones.CheckInt(dr["CATEN_CODIGO"]);
                    item.FLAG_TOLERANCIA = Funciones.CheckStr(dr["EMPCV_FLAG_TOLERANCIA"]);
                    item.TOLERANCIA = Funciones.CheckDbl(dr["EMPCN_TOLERANCIA"]);
                    item.TOLERANCIA_BLOQUEO = Funciones.CheckDbl(dr["CATEN_TOLERANCIA_BLOQUEO"]);
                    item.CATEN_DIAS_VENCIDO = Funciones.CheckInt(dr["CATEN_DIAS_VENCIDO"]);
                    item.CATEGORIA_DES = Funciones.CheckStr(dr["CATEV_DESCRIPCION"]);
                    item.SEGMENTO_COD = Funciones.CheckStr(dr["SEGN_CODIGO"]);
                    try
                    {
                        item.CATEN_MONTO_VENCIDO = Funciones.CheckInt(dr["CATEN_MONTO_VENCIDO"]);
                    }
                    catch { }
                    filas.Add(item);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return filas;
        }
        //PROY-29121-FIN

        //INC000001337773 - INICIO
        public static void rechazoEvaluacion(BEClienteCuenta objCliente, string strMotivo, string strusuario, string tipoOrigen, string strLinea, ref string p_nrolog, ref string p_deslog)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_REVAN_MONTODEUDA", DbType.Int64 , ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_REVAC_SUSPENSION", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_REVAV_SUSPENSION", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_REVAN_PERMANENCIA", DbType.Int64, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_REVAV_CODOPERACION", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_REVAV_TIPODOCUMENTO", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_REVAV_NRODOCUMENTO", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_REVAV_NROLINEA", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_REVAV_MOTIVORECHAZO", DbType.String,500, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_REVAN_ANTIGUEDADDEUDA", DbType.Int64, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_REVAV_USUARIOCREA", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_NRO_LOG", DbType.String, ParameterDirection.Output),
												   new DAABRequest.Parameter("P_DES_LOG", DbType.String, ParameterDirection.Output)            
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objCliente.deudaTotal;
            if (objCliente.nroSuspension > 0)
            { arrParam[1].Value = "S"; }
            else { arrParam[1].Value = "N"; }
            arrParam[2].Value = "";
            arrParam[3].Value = objCliente.tiempoPermanencia;
            arrParam[4].Value = tipoOrigen;
            arrParam[5].Value = objCliente.tipoDoc;
            arrParam[6].Value = objCliente.nroDoc;
            arrParam[7].Value = strLinea;
            arrParam[8].Value = strMotivo;
            arrParam[9].Value = objCliente.nroDiasDeuda;
            arrParam[10].Value = strusuario;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS + ".SISACTSI_RECHAZO_EVAL";
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);

                IDataParameter pSalida1, pSalida2;
                pSalida1 = (IDataParameter)obRequest.Parameters[11];
                pSalida2 = (IDataParameter)obRequest.Parameters[12];
                p_nrolog = Funciones.CheckStr(pSalida1.Value);
                p_deslog = Funciones.CheckStr(pSalida2.Value);

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }

        //INC000001337773 - FIN

        //PROY-32439 MAS INI Consulta SP
        public static BEPuntoVenta ConsultarRegionDepartamentoporOficina(String strCodigoPuntoVenta, out String strRespuestaCodigo, out String strRespuestaMensaje)
        {
            DAABRequest.Parameter[] arrParam = {
                                    new DAABRequest.Parameter("p_ovenc_codigo", DbType.String , ParameterDirection.Input),
                                    new DAABRequest.Parameter("p_codigo_respuesta", DbType.String, ParameterDirection.Output),
                                    new DAABRequest.Parameter("p_mensaje_respuesta", DbType.String, ParameterDirection.Output),
                                    new DAABRequest.Parameter("k_cur_salida", DbType.Object,ParameterDirection.Output)
            };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(strCodigoPuntoVenta))
                arrParam[0].Value = strCodigoPuntoVenta;

            var objRequest = (new BDSISACT(BaseDatos.BdSisact)).CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactConsultas + ".sisactss_depa_region_x_pdv";
            objRequest.Parameters.AddRange(arrParam);
            var objResponse = new BEPuntoVenta();
            IDataReader dr = null;

            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objResponse.RegionCodigo = Funciones.CheckStr(dr["REGIC_CODIGO"]);
                    objResponse.RegionDescripcion = Funciones.CheckStr(dr["REGIV_DESCRIPCION"]);
                    objResponse.DepacCodigo = Funciones.CheckStr(dr["DEPAC_CODIGO"]);
                    objResponse.DepavDescripcion = Funciones.CheckStr(dr["DEPAV_DESCRIPCION"]);
                    objResponse.OvenvDescripcion = Funciones.CheckStr(dr["OVENV_DESCRIPCION"]);
                    objResponse.ToficCodigo = Funciones.CheckStr(dr["TOFIC_DESCRIPCION"]);
                }
                strRespuestaCodigo = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[1]).Value);
                strRespuestaMensaje = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[2]).Value);
            }
            catch (Exception e)
            {
                objResponse = null;
                strRespuestaCodigo = "-1";
                strRespuestaMensaje = "Error_SISACTSS_DEPA_REGION_X_PDV[" + e.Message + "]";
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                objRequest.Factory.Dispose();
            }
            return objResponse;
        }
        //PROY-32439 MAS FIN Consulta SP

        //INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18

        public static List<BEClienteSAP> ConsultaCorreoHistorico(string strTipoDocumento, string strNumeroDocumento, out string strMensajeRpt)
        {
            var lstCliente = new List<BEClienteSAP>();
            strMensajeRpt = String.Empty;
         
                DAABRequest.Parameter[] arrParam = {   new DAABRequest.Parameter("PI_TIPO_DOCUMENTO", DbType.String,ParameterDirection.Input),
                                                       new DAABRequest.Parameter("PI_NRO_DOCUMENTO", DbType.String,ParameterDirection.Input),    
                                                       new DAABRequest.Parameter("PO_CURSOR_DATOS", DbType.Object,ParameterDirection.Output),
                                                       new DAABRequest.Parameter("PO_MSJ_RESPUESTA", DbType.String,ParameterDirection.Output)
                                                   };

                for (int i = 0; i < arrParam.Length; i++)
                    arrParam[i].Value = DBNull.Value;

                arrParam[0].Value = strTipoDocumento;
                arrParam[1].Value = strNumeroDocumento;

                BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
                DAABRequest obRequest = obj.CreaRequest();
           try
           {
                 obRequest.CommandType = CommandType.StoredProcedure;
                 obRequest.Command = BaseDatos.PkgConsMaestraSap + ".SISACT_SEL_CORREO_ELECTRONICA";
                 obRequest.Parameters.AddRange(arrParam);

                 IDataReader dr = null;
             
                 dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                    while (dr.Read())
                    {
                            BEClienteSAP item = new BEClienteSAP();
                            item.TipoDocCliente = Funciones.CheckStr(dr["CLIEC_TIPO_DOCUMENTO"]);
                            item.Cliente = Funciones.CheckStr(dr["CLIEV_NRO_DOCUMENTO"]);
                            item.Nombre = Funciones.CheckStr(dr["CLIEV_NOMBRE"]);
                            item.ApellidoPaterno = Funciones.CheckStr(dr["CLIEV_APELLIDO_PATERNO"]);
                            item.ApellidoMaterno = Funciones.CheckStr(dr["CLIEV_APELLIDO_MATERNO"]);
                            item.EmailFact = Funciones.CheckStr(dr["CLIEV_CORREO_FACT"]);

                            lstCliente.Add(item);
                     }

                    if (lstCliente.Count < 1)
                    {
                        lstCliente = null;
                    }
            }
            catch (Exception ex)
            {
                strMensajeRpt = Funciones.CheckStr(ex.Message);
                lstCliente = null;
            }
            //RGP VALIDACION PP| INI
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
           //RGP VALIDACION PP| FIN
            return lstCliente;
        }

        public string ActualizarCorreoElectronico(BEClienteSAP objCliente, string strUsuario, string strComentario, out string strMsjRpta)
        {
            var strCodRpta = String.Empty;
            var obRequest = new DAABRequest();

            try
            {
                DAABRequest.Parameter[] arrParam = {
                                                   new DAABRequest.Parameter("PI_TIPO_DOCUMENTO", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_NRO_DOCUMENTO", DbType.String,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_RAZON_SOCIAL", DbType.String,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_NOMBRES_CLIENTE", DbType.String,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_APELL_PATERNO", DbType.String,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_APELL_MATERNO", DbType.String,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_USUARIO", DbType.String,   ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_CORREO_ELECTRONICO", DbType.String,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_COMENTARIO", DbType.String,   ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PO_COD_RESULTADO", DbType.String, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MSJ_RESULTADO", DbType.String, 1000, ParameterDirection.Output)
                                               };



                int i;
                for (i = 0; i < arrParam.Length; i++)
                    arrParam[i].Value = DBNull.Value;

                arrParam[0].Value = Funciones.CheckStr(objCliente.TipoDocCliente);
                arrParam[1].Value = Funciones.CheckStr(objCliente.Cliente);
                arrParam[2].Value = Funciones.CheckStr(objCliente.RazonSocial);
                arrParam[3].Value = Funciones.CheckStr(objCliente.Nombre);
                arrParam[4].Value = Funciones.CheckStr(objCliente.ApellidoPaterno);
                arrParam[5].Value = Funciones.CheckStr(objCliente.ApellidoMaterno);
                arrParam[6].Value = Funciones.CheckStr(strUsuario);
                arrParam[7].Value = Funciones.CheckStr(objCliente.EmailFact);
                arrParam[8].Value = Funciones.CheckStr(strComentario);

                BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);

                obRequest = obj.CreaRequest();
                obRequest.CommandType = CommandType.StoredProcedure;
                obRequest.Command = BaseDatos.PkgConsMaestraSap + ".SISACT_UPD_FACT_ELECTRONICA";
                obRequest.Parameters.AddRange(arrParam);
                obRequest.Transactional = true;


                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();

                IDataParameter parRetorno1;
                IDataParameter parRetorno2;
                parRetorno1 = (IDataParameter)obRequest.Parameters[9];
                strMsjRpta = Funciones.CheckStr(parRetorno1.Value);

                parRetorno2 = (IDataParameter)obRequest.Parameters[10];
                strCodRpta = Funciones.CheckStr(parRetorno2.Value);
            }
            catch (Exception ex)
            {
                strCodRpta = "-1";
                strMsjRpta = Funciones.CheckStr(ex.Message);
                obRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return strCodRpta;
        }

        public bool RegistrarCliente(BEClienteSAP oClienteSAP)
        {
            DAABRequest.Parameter[] arrParam = {
													new DAABRequest.Parameter("V_CLIEV_NRO_DOCUMENTO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_TIPO_DOCUMENTO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_NOMBRE", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_APELLIDO_PATERNO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_APELLIDO_MATERNO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_RAZON_SOCIAL", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIED_FECHA_NACIMIENTO", DbType.Date, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_TELEFONO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_E_MAIL", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_SEXO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_ESTADO_CIVIL", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_TITULO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_CARGA_FAMILIAR", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_CONYUGE_NOMBRE", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_CONYUGE_APE_PAT", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_CONYUGE_APE_MAT", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_DIRECCION_LEGAL_PREF", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_DIRECCION_LEGAL", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_DIRECCION_LEGAL_REFER", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_UBIGEO_LEGAL", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_TELEF_LEGAL_PREF", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_TELEF_LEGAL", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_DIRECCION_FACT_PREF", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_DIRECCION_FACT", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_DIRECCION_FACT_REFER", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_UBIGEO_FACT", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_TELEF_FACT_PREF", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_TELEF_FACT", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_REPLEGAL_TIPO_DOC", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_REPLEGAL_NRO_DOC", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_REPLEGAL_NOMBRE", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_REPLEGAL_APE_PAT", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_REPLEGAL_APE_MAT", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIED_REPLEGAL_FECHA_NAC", DbType.Date, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_REPLEGAL_TELEFONO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_REPLEGAL_SEXO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_REPLEGAL_EST_CIV", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_REPLEGAL_TITULO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_CONTACTO_TIPO_DOC", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_CONTACTO_NRO_DOC", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_CONTACTO_NOMBRE", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_CONTACTO_APE_PAT", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_CONTACTO_APE_MAT", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_CONTACTO_TELEFONO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEN_COND_CLIENTE", DbType.Int32, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_EMPRESA_LABORA", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_EMPRESA_CARGO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_EMPRESA_TELEFONO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEN_INGRESO_BRUTO", DbType.Double, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEN_OTROS_INGRESOS", DbType.Double, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_TCREDITO_TIPO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_TCREDITO_NUM", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_TCREDITO_MONEDA", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEN_TCREDITO_LINEA_CRED", DbType.Double, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEC_TCREDITO_FECHA_VENC", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_OBSERVACIONES", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_CODIGO_SAP", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_VENDEDOR_SAP", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_USUARIO_CREA", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("V_CLIEV_TIPO_CLIENTE", DbType.String, ParameterDirection.Input), 
                                                    new DAABRequest.Parameter("V_CLIEC_CODNACION", DbType.String, ParameterDirection.Input), //INC000003442281 
                                                    new DAABRequest.Parameter("V_CLIEV_DESCNACION", DbType.String, ParameterDirection.Input), //INC000003442281
													new DAABRequest.Parameter("P_RESULTADO", DbType.String, ParameterDirection.Output)													
												};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            bool salida = false;

            int z = 0;
            arrParam[z].Value = oClienteSAP.Cliente;
            z++; arrParam[z].Value = oClienteSAP.TipoDocCliente;
            z++; arrParam[z].Value = oClienteSAP.Nombre;
            z++; arrParam[z].Value = oClienteSAP.ApellidoPaterno;
            z++; arrParam[z].Value = oClienteSAP.ApellidoMaterno;
            z++; arrParam[z].Value = oClienteSAP.RazonSocial;
            
            z++; arrParam[z].Value = Funciones.CheckDate(oClienteSAP.FechaNacimiento);
            z++; arrParam[z].Value = oClienteSAP.Telefono;
            z++; arrParam[z].Value = oClienteSAP.EMail;
            z++; arrParam[z].Value = oClienteSAP.Sexo;
            z++; arrParam[z].Value = oClienteSAP.EstadoCivil;
            z++; arrParam[z].Value = oClienteSAP.TitCliente;
            z++; arrParam[z].Value = oClienteSAP.CargaFamiliar;
            z++; arrParam[z].Value = oClienteSAP.NombreConyuge;
            z++; arrParam[z].Value = oClienteSAP.ApePatConyuge;
            z++; arrParam[z].Value = oClienteSAP.ApeMatConyuge;
            z++; arrParam[z].Value = "";
            z++; arrParam[z].Value = oClienteSAP.DireccionLegal;
            z++; arrParam[z].Value = oClienteSAP.DireccionLegalPref;
            z++; arrParam[z].Value = oClienteSAP.UbigeoLegal;
            z++; arrParam[z].Value = oClienteSAP.TelefLegalPref;
            z++; arrParam[z].Value = oClienteSAP.TelefLegal;

            z++; arrParam[z].Value = oClienteSAP.DireccionFactPref;
            z++; arrParam[z].Value = oClienteSAP.DireccionFact;
            z++; arrParam[z].Value = oClienteSAP.DireccionFactRefe;
            z++; arrParam[z].Value = oClienteSAP.UbigeoFact;

            z++; arrParam[z].Value = oClienteSAP.TelfPref;
            z++; arrParam[z].Value = oClienteSAP.TelfFact;
            z++; arrParam[z].Value = oClienteSAP.ReplegalTipDoc;
            z++; arrParam[z].Value = oClienteSAP.ReplegalNroDoc;
            z++; arrParam[z].Value = oClienteSAP.ReplegalNombre;
            z++; arrParam[z].Value = oClienteSAP.ReplegalApePat;
            z++; arrParam[z].Value = oClienteSAP.ReplegalApeMat;
            
            z++; arrParam[z].Value = Funciones.CheckDate(oClienteSAP.ReplegalFnac);
            z++; arrParam[z].Value = oClienteSAP.ReplegalTelefon;
            z++; arrParam[z].Value = oClienteSAP.ReplegalSexo;
            z++; arrParam[z].Value = oClienteSAP.ReplegalEstCiv;
            z++; arrParam[z].Value = oClienteSAP.ReplegalTit;
            z++; arrParam[z].Value = oClienteSAP.ContactoTipDoc;
            z++; arrParam[z].Value = oClienteSAP.ContactoNroDoc;
            z++; arrParam[z].Value = oClienteSAP.ContactoNombre;
            z++; arrParam[z].Value = oClienteSAP.ContactoApePat;
            z++; arrParam[z].Value = oClienteSAP.ContactoApeMat;
            z++; arrParam[z].Value = oClienteSAP.ContactoTelefon;
            z++; arrParam[z].Value = 0;
            z++; arrParam[z].Value = oClienteSAP.EmpresaLabora;
            z++; arrParam[z].Value = oClienteSAP.EmpresaCargo;
            z++; arrParam[z].Value = oClienteSAP.EmpresaTelefono;
            z++; arrParam[z].Value = oClienteSAP.IngBruto;
            z++; arrParam[z].Value = oClienteSAP.OtrosIngresos;
            z++; arrParam[z].Value = oClienteSAP.TarjetaCredito;
            z++; arrParam[z].Value = oClienteSAP.NumTarjCredito;
            z++; arrParam[z].Value = oClienteSAP.NumTarjCredito;
            z++; arrParam[z].Value = oClienteSAP.LineaCredito;            
            z++; arrParam[z].Value = Funciones.CheckStr(oClienteSAP.FechaVencTcred);
            z++; arrParam[z].Value = oClienteSAP.Observaciones;
            z++; arrParam[z].Value = oClienteSAP.Kunnr;
            z++; arrParam[z].Value = oClienteSAP.VendedorSap;
            z++; arrParam[z].Value = oClienteSAP.UsuarioCrea;
            z++; arrParam[z].Value = oClienteSAP.TipoCliente;
            z++; arrParam[z].Value = oClienteSAP.CliCodNacion; //INC000003442281 
            z++; arrParam[z].Value = oClienteSAP.CliDescNacion; //INC000003442281 

            var obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;

            //obRequest.Command = BaseDatos.PkgConsMaestraSap + ".SSAPSU_CLIENTE";
            obRequest.Command = BaseDatos.PkgConsMaestraSap + ".SSAPSU_CLIENTE_3442281"; //INC000003442281 
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);

                salida = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return salida;
        }

        //FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_1


        //INICIO PROYECTO KV FASE 2

        public string DGrabarSecTransac(string strTrsn_id, int strTpsn_sec, string strTpsv_num, string strTpsv_user, string strTpsv_email)
        {

            objLog.CrearArchivolog("[INICIO][DGrabarSecTransac]", null, null);
            objLog.CrearArchivolog("[Entrada][strTrsn_id]", strTrsn_id, null);
            objLog.CrearArchivolog("[Entrada][strTpsn_sec]", strTpsn_sec.ToString(), null);
            objLog.CrearArchivolog("[Entrada][strTpsv_num]", strTpsv_num.ToString(), null);
            objLog.CrearArchivolog("[Entrada][strTpsv_user]", strTpsv_user.ToString(), null);
            objLog.CrearArchivolog("[Entrada][strTpsv_email]", strTpsv_email.ToString(), null);

            DAABRequest.Parameter[] arrParam = {
		        new DAABRequest.Parameter("PI_TRSV_ID", DbType.String,100, ParameterDirection.Input),
		        new DAABRequest.Parameter("PI_TPSN_SEC", DbType.Int32, ParameterDirection.Input),
		        new DAABRequest.Parameter("PI_TPSV_NUMERO", DbType.String,50, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_TPSV_USER_REG", DbType.String,50, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_TRSV_EMAIL", DbType.String,500, ParameterDirection.Input),
		        new DAABRequest.Parameter("PO_COD_RESPUESTA", DbType.Int32, ParameterDirection.Output),
                new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String, ParameterDirection.Output)
			};
            int i = 0;
            string salida="";
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0;
            if (!string.IsNullOrEmpty(strTrsn_id)) arrParam[i].Value = strTrsn_id;
            ++i; if (strTpsn_sec > 0) arrParam[i].Value = strTpsn_sec;
            ++i; if (!string.IsNullOrEmpty(strTpsv_num)) arrParam[i].Value = strTpsv_num;
            ++i; if (!string.IsNullOrEmpty(strTpsv_user)) arrParam[i].Value = strTpsv_user;
            ++i; if (!string.IsNullOrEmpty(strTpsv_email)) arrParam[i].Value = strTpsv_email;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SIACSI_TRANS_CONTRATO";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                IDataParameter parRetorno1;
                parRetorno1 = (IDataParameter)objRequest.Parameters[5];

                salida = Funciones.CheckStr(parRetorno1.Value);

            }
            catch (Exception ex)
            {
                salida = ex.ToString();
                objLog.CrearArchivolog("[ERROR][DGrabarSecTransac]", null, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("[Out][salida]", salida, null);
                objLog.CrearArchivolog("[FIN][DGrabarSecTransac]", null, null);
                objRequest.Factory.Dispose();
                objRequest.Parameters.Clear();
            }
            return salida;
        }

//PROY-31948 INI
        public static BECuota ConsultaCuotasPendientesPVU(string strTipoDocumento, string strNroDocumento, ref string strCodRespuestaPVUDB, ref string strMsjRespuestaPVUDB)
        {
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_NRO_DOC", DbType.String, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_TOT_IMP_SOL_PEND", DbType.Double, ParameterDirection.Output),
                                                 new DAABRequest.Parameter("P_TOT_CANT_LIN", DbType.Double, ParameterDirection.Output),                                                 
                                                 new DAABRequest.Parameter("P_CANT_MAX_CUO", DbType.Double, ParameterDirection.Output),
                                                 new DAABRequest.Parameter("P_COD_RESP", DbType.String, ParameterDirection.Output),                                                 
                                                 new DAABRequest.Parameter("P_MSG_RESP", DbType.String, ParameterDirection.Output)
                                                };

            arrParam[0].Value = Funciones.CheckStr(strTipoDocumento);
            arrParam[1].Value = Funciones.CheckStr(strNroDocumento);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_DRA_CVE + ".SISACSS_CONSULTAR_CVE_PEND";
            objRequest.Parameters.AddRange(arrParam);

            BECuota objBECuota = new BECuota();

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);


                objBECuota.montoPendienteCuotasUltimasVentas = Funciones.CheckDbl(((IDataParameter)objRequest.Parameters[2]).Value);
                objBECuota.cantidadPlanesCuotasPendientesUltimasVentas = Funciones.CheckInt(((IDataParameter)objRequest.Parameters[3]).Value);
                objBECuota.cantidadMaximaCuotasPendientesUltimasVentas = Funciones.CheckInt(((IDataParameter)objRequest.Parameters[4]).Value);
                strCodRespuestaPVUDB = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[5]).Value);
                strMsjRespuestaPVUDB = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[6]).Value);
            }
            catch (Exception ex)
            {
                strCodRespuestaPVUDB = Funciones.CheckStr(ex.Source);
                strMsjRespuestaPVUDB = Funciones.CheckStr(ex.Message);
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objBECuota;
        }
        //PROY-31948 FIN

        //INI PROY-31948_Migracion
        public static void AprobarCreditosReno(BESolicitudEmpresa oItem)
        {
            DAABRequest.Parameter[] arrParam = {   new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_NUM_CAR_FIJ_ADI", DbType.Double, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_NUM_RA", DbType.Double, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_IMP_DG_MAN", DbType.Double, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_COM_PUN_VEN", DbType.String, 200, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_COM_EVAL", DbType.String, 200, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TIP_CAR_MAN", DbType.String, 4, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_COD_APROB", DbType.String, 20, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_COST_INST", DbType.Double, ParameterDirection.Input)
											   };
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = oItem.SOLIN_CODIGO;
            i++; arrParam[i].Value = oItem.SOLIN_NUM_CAR_FIJ_ADI;
            i++; arrParam[i].Value = oItem.SOLIN_NUM_RA;
            i++; arrParam[i].Value = oItem.SOLIN_IMP_DG_MAN;
            i++; arrParam[i].Value = oItem.SOLIV_COM_PUN_VEN;
            i++; arrParam[i].Value = oItem.SOLIV_COM_EVALUADOR;
            i++; arrParam[i].Value = oItem.SOLIC_TIP_CAR_MAN;
            i++; arrParam[i].Value = oItem.SOLIC_USU_CRE;
            //i++; arrParam[i].Value = oItem.SOLIN_COSTO_INST;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SP_APROBAR_CREDITOS";
            obRequest.Parameters.AddRange(arrParam);
            obRequest.Transactional = true;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
            }
            catch (Exception e)
            {
                obRequest.Factory.RollBackTransaction();
                throw e;
            }
            finally
            {
                obRequest.Factory.Dispose();
                obRequest.Parameters.Clear();
            }
        }

        public static bool GrabarDatosEvaluadorCheckList(Int64 nroSEC,
            double nroDG, double nroRA,
            string estadoSEC, string estadoSECDes,
            double cargo_fijo_eva, double total_garantia,
            string comentario_pdv, string comentario_evaluador,
            string comentario_sistema,
            string login, string loginAutorizador,
            ref string rFlagProceso,
            ref string rMsg)
        {
            DAABRequest.Parameter[] arrParam = {					
												   new DAABRequest.Parameter("P_SOLIN_CODIGO" ,DbType.Int64,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_NRO_DG" ,DbType.Double,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_NRO_RA" ,DbType.Double,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ESTADO_ID" ,DbType.String,2,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ESTADO_DES" ,DbType.String,20,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CARGO_FIJO_EVA" ,DbType.Double,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TOTAL_GARANTIA_EVA" ,DbType.Double,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_COMENTARIO_PDV" ,DbType.String,500,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_COMENTARIO_EVA" ,DbType.String,500,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_COMENTARIO_SISTEMA" ,DbType.String,500,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_USU_CRE" ,DbType.String,10,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_USU_AUT_DG" ,DbType.String,15,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_FLAG_PROCESO" ,DbType.String,10,ParameterDirection.Output),
												   new DAABRequest.Parameter("P_MSG" ,DbType.String,255,ParameterDirection.Output)
											   };
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;
            ++i; arrParam[i].Value = nroDG;
            ++i; arrParam[i].Value = nroRA;
            ++i; arrParam[i].Value = estadoSEC;
            ++i; arrParam[i].Value = estadoSECDes;
            ++i; arrParam[i].Value = cargo_fijo_eva;
            ++i; arrParam[i].Value = total_garantia;
            ++i; arrParam[i].Value = comentario_pdv;
            ++i; arrParam[i].Value = comentario_evaluador;
            ++i; arrParam[i].Value = comentario_sistema;
            ++i; arrParam[i].Value = login;
            ++i; arrParam[i].Value = loginAutorizador;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();

            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACU_ACT_SOL_EVALUADOR_CHK";
            obRequest.Parameters.AddRange(arrParam);
            obRequest.Transactional = true;

            bool salida = false;
            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
                salida = true;
            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                rMsg = "Error al Insertar Historico Estado. \nMensaje : " + ex.Message;
                throw ex;
            }
            finally
            {
                IDataParameter p1, p2;
                p1 = (IDataParameter)obRequest.Parameters[arrParam.Length - 2];
                p2 = (IDataParameter)obRequest.Parameters[arrParam.Length - 1];
                rFlagProceso = Funciones.CheckStr(p1.Value);
                rMsg = Funciones.CheckStr(p2.Value);
                obRequest.Factory.Dispose();
            }
            return salida;
        }

        public static bool GrabarRechazoSolicitud(Int64 secId, string usuario_login, string motivo_id, string obs, string codestado, ref string rProceso, ref string rMsg)
        {
            DAABRequest.Parameter[] arrParam = {					
												   new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_USUARIO", DbType.String,20,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_MOTIVO_ID", DbType.String,2,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_OBS", DbType.String,300,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ESTAC_CODIGO", DbType.String,4,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_PROCESO", DbType.String,ParameterDirection.Output)
											   };
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = secId;
            arrParam[1].Value = usuario_login;
            if (motivo_id != null && motivo_id != "") arrParam[2].Value = motivo_id;
            arrParam[3].Value = obs;
            arrParam[4].Value = codestado;

            bool salida = false;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Transactional = true;

            obRequest.Parameters.Clear();
            obRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACU_RECHAZO_SOLICITUD";
            obRequest.Parameters.AddRange(arrParam);
            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
                salida = true;
            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                rMsg = "Error al Grabar Rechazo de la solicitud. Mensaje : " + ex.Message;
                throw ex;
            }
            finally
            {
                IDataParameter pSalida1;
                pSalida1 = (IDataParameter)obRequest.Parameters[obRequest.Parameters.Count - 1];
                rProceso = Funciones.CheckStr(pSalida1.Value);
                obRequest.Factory.Dispose();
            }
            return salida;
        }	

        public static bool ActualizarComentarios(long pCOD_SEC, string pComentarioPDV, string pComentarioEval)
        {

            bool bSeActualizo = false;
            //--define parametros
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_COMENTARIO_PDV", DbType.String,500,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_COMENTARIO_EVA", DbType.String,500,ParameterDirection.Input)
											   };
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (pCOD_SEC > 0) arrParam[i].Value = pCOD_SEC;
            i++; if (pComentarioPDV.Length > 0) arrParam[i].Value = pComentarioPDV;
            i++; if (pComentarioEval.Length > 0) arrParam[i].Value = pComentarioEval;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_EVALUACION_SEC + ".ActualizarComentariosSEC";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                //--
                bSeActualizo = true;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                objRequest.Factory.Dispose();
                objRequest.Parameters.Clear();
            }
            //----
            return bSeActualizo;
        }

        public static bool ObtenerParamSolicitud(string nroSEC, ref string Idcanal, ref string idPtoVen, ref Int64 idConsultor, ref Int64 IdFlex, ref Int64 idAutorizador)
        {
            bool salida = false;
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.String,20,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CANAC_CODIGO", DbType.String,2,ParameterDirection.Output),
												   new DAABRequest.Parameter("P_OVENC_CODIGO", DbType.String,4,ParameterDirection.Output),
												   new DAABRequest.Parameter("P_SOLIN_CONSULTOR", DbType.Int64,ParameterDirection.Output),
												   new DAABRequest.Parameter("P_FLEXN_CODIGO", DbType.Int64,ParameterDirection.Output),
												   new DAABRequest.Parameter("P_USUAN_CODIGO", DbType.Int64,ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACTS_PARAM_SOLICITUD";
            obRequest.Parameters.AddRange(arrParam);
            
            try
            {
                obRequest.Factory.ExecuteScalar(ref obRequest);
                IDataParameter parSalida1, parSalida2, parSalida3, parSalida4, parSalida5;
                parSalida1 = (IDataParameter)obRequest.Parameters[1];
                parSalida2 = (IDataParameter)obRequest.Parameters[2];
                parSalida3 = (IDataParameter)obRequest.Parameters[3];
                parSalida4 = (IDataParameter)obRequest.Parameters[4];
                parSalida5 = (IDataParameter)obRequest.Parameters[5];

                Idcanal = Funciones.CheckStr(parSalida1.Value);
                idPtoVen = Funciones.CheckStr(parSalida2.Value);
                idConsultor = Funciones.CheckInt64(parSalida3.Value);
                IdFlex = Funciones.CheckInt64(parSalida4.Value);
                idAutorizador = Funciones.CheckInt64(parSalida5.Value);

                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                obRequest.Factory.Dispose();
            }

            return salida;
        }

        public static string obtenerUsuarioXSec(Int64 solin_codigo, ref string canac_codigo)
        {
            DAABRequest.Parameter[] arrParam = {   new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_USU_CRE", DbType.String, ParameterDirection.Output),
												   new DAABRequest.Parameter("P_CANAC_CODIGO", DbType.String, ParameterDirection.Output),
			};

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = solin_codigo;

            string salida = "";
            canac_codigo = "";

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACTSS_USUARIO_X_SEC";
            obRequest.Parameters.AddRange(arrParam);
            obRequest.Transactional = true;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();

                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)obRequest.Parameters[1];
                salida = Funciones.CheckStr(parSalida1.Value);

                IDataParameter parSalida2;
                parSalida2 = (IDataParameter)obRequest.Parameters[2];
                canac_codigo = Funciones.CheckStr(parSalida2.Value);
            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                obRequest.Factory.Dispose();
            }

            return salida;
        }
        
        public static ArrayList obtenerDatoAuxiliarRepresentanteLegalD(Int64 SOLIN_CODIGO, string USUAC_CTARED)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_USUAC_CTARED", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_USUAV_EMAIL", DbType.String, ParameterDirection.Output),
												   new DAABRequest.Parameter("P_SOLIV_DES_EST", DbType.String, ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = SOLIN_CODIGO;
            arrParam[1].Value = USUAC_CTARED;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SECSC_CON_ENV_CORREO_CONSUL";
            obRequest.Parameters.AddRange(arrParam);

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                IDataParameter parSalida2;
                parSalida2 = (IDataParameter)obRequest.Parameters[2];
                filas.Insert(0, Funciones.CheckStr(parSalida2.Value));

                IDataParameter parSalida3;
                parSalida3 = (IDataParameter)obRequest.Parameters[3];
                filas.Insert(1, Funciones.CheckStr(parSalida3.Value));

                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return filas;
        }


        public static void insertarEvaluacionAccion(BEAccion objAccion, int codsec)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("v_cod_accion", DbType.Int32,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_codsec", DbType.Int32,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_evaluacion", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_comentario_ca", DbType.String,200,ParameterDirection.Input),
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objAccion.CODIGO;
            arrParam[1].Value = codsec;
            arrParam[2].Value = objAccion.EVALUACION.ESTADO_EVALUACION;
            arrParam[3].Value = objAccion.EVALUACION.COMENTARIO_CA;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);

            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_EVALUACION_SEC + ".InsertarAccionEvaluacion";
            obRequest.Parameters.AddRange(arrParam);
            obRequest.Transactional = true;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }

        public static void insertarEvaluacionDocumentoSec(BEDocumentoEvaluacion objdoc, int codsec)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("v_cod_documento_sect", DbType.Int32,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_codsec", DbType.Int32,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_cod_motivo_observacion", DbType.Int32,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_evaluacion", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_comentario_ca", DbType.String,200,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_comentario_pdv", DbType.String,200,ParameterDirection.Input)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objdoc.CODIGO;
            arrParam[1].Value = codsec;
            arrParam[3].Value = objdoc.EVALUACION.ESTADO_EVALUACION;

            arrParam[5].Value = objdoc.EVALUACION.COMENTARIO_PDV;

            if (objdoc.EVALUACION.ESTADO_EVALUACION == ConfigurationManager.AppSettings["ConstEvalConstantes_DESAPROBADO"])
            {
                arrParam[4].Value = objdoc.EVALUACION.COMENTARIO_CA;

                if (objdoc.EVALUACION.MOTIVO_OBSERVACION.CODIGO != 0)
                {
                    arrParam[2].Value = objdoc.EVALUACION.MOTIVO_OBSERVACION.CODIGO;
                }
            }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);

            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_EVALUACION_SEC + ".InsertarDocumentoSecEvaluacion";
            obRequest.Parameters.AddRange(arrParam);
            obRequest.Transactional = true;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();

            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }


        public static void actualizarCargoFijoSec(int codsec, double cargofijo, string tipocargo, double num_ra_dg, double importe)
        {

            ArrayList comandosTran = new ArrayList();

            DAABRequest.Parameter[] arrParam = {
				
												   new DAABRequest.Parameter("v_codsec", DbType.Int32,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_cargo", DbType.Decimal,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_tipo_cargo", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("v_num_ra_dg", DbType.Decimal,ParameterDirection.Input),
												   new DAABRequest.Parameter("importe", DbType.Decimal,ParameterDirection.Input)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = codsec;
            arrParam[1].Value = cargofijo;
            arrParam[2].Value = tipocargo;
            arrParam[3].Value = num_ra_dg;
            arrParam[4].Value = importe;


            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);

            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_EVALUACION_SEC + ".ActualizarSecCargoFijo";
            obRequest.Parameters.AddRange(arrParam);
            obRequest.Transactional = true;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }

        public static void grabarComentarios(BEEvaluacion objEvaluacion)
        {
            DAABRequest.Parameter[] arrParam = {
                                                   new DAABRequest.Parameter("v_codsec", DbType.Int64, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_cod_analista", DbType.String,20, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_ra_dg", DbType.Int64, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_cantidad", DbType.Int64, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_monto", DbType.Decimal, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_correccionpdv_com_ca", DbType.String,200, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_adjuntar_com_ca", DbType.String,200, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_propuesta_com_ca", DbType.String,200, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_comentario_final_ca", DbType.String,1000, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_comentario_final_pdv", DbType.String,1000, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_adjuntar_voucher", DbType.Int64, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_nueva_propuesta", DbType.Int64, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_flag_eval", DbType.String, ParameterDirection.Input), //
                                                   new DAABRequest.Parameter("v_correccion_pdv", DbType.Int64, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("v_comentario_final_credito", DbType.String,1000, ParameterDirection.Input),
                                               };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objEvaluacion.cod_sec;
            arrParam[1].Value = objEvaluacion.cod_analista;
            arrParam[6].Value = objEvaluacion.adjuntar_comentario_ca;
            arrParam[8].Value = objEvaluacion.comentario_final_ca;
            arrParam[9].Value = objEvaluacion.comentario_final_pdv;
            arrParam[10].Value = objEvaluacion.adjuntar_voucher == true ? 1 : 0;
            arrParam[11].Value = objEvaluacion.nueva_propuesta == true ? 1 : 0;
            arrParam[12].Value = objEvaluacion.flag_evaluacion;
            arrParam[13].Value = objEvaluacion.existe_rechazo == true ? 1 : 0;
            arrParam[14].Value = objEvaluacion.comentario_final_credito;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);

            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_EVALUACION_SEC + ".InsertarEvaluacion";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
            }
            catch (Exception ex)
            {
                objRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }

        //FIN PROY-31948_Migracion

    }
}
