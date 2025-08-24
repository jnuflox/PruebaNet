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
    public class DAConsumer
    {
        GeneradorLog objLog = new GeneradorLog("Consumer", null, null, "DATA_LOG");

        public bool ConsultaValidacionCliente(string tipoDocumento, string nroDocumento, string nrotelefono, out string flagValida, out string msgText)
        {

            DAABRequest.Parameter[] arrParam = {												   
												new DAABRequest.Parameter("P_PHONE", DbType.String,20,ParameterDirection.Input),
												new DAABRequest.Parameter("P_TIPO_DOC", DbType.String,20,ParameterDirection.Input),
												new DAABRequest.Parameter("P_NUM_DOC", DbType.String,20,ParameterDirection.Input),
												new DAABRequest.Parameter("P_FLAG_VALIDA", DbType.String,20,ParameterDirection.Output),
												new DAABRequest.Parameter("P_MSG_TEXT", DbType.String,200,ParameterDirection.Output),
												new DAABRequest.Parameter("REFCURSOR", DbType.Object,ParameterDirection.Output)
											};
            int i;
            bool retorno = true;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nrotelefono; //oBeCliente.NumeroLinea;
            ++i; arrParam[i].Value = tipoDocumento; //oBeCliente.TipoDocumento;
            ++i; arrParam[i].Value = nroDocumento; //oBeCliente.NumeroDocumento;

            var obj = new BDCLARIFY(BaseDatos.BdClarify);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PkgCustomerClfy + ".SP_VALIDATITULARIDAD";
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                //obRequest.Factory.CommitTransaction();

                IDataParameter parSalida1 = (IDataParameter)obRequest.Parameters[3];
                IDataParameter parSalida2 = (IDataParameter)obRequest.Parameters[4];
                IDataParameter parSalida3 = (IDataParameter)obRequest.Parameters[5];
                flagValida = Funciones.CheckStr(parSalida1.Value);
                msgText = Funciones.CheckStr(parSalida2.Value);

                object value = parSalida3.Value;

            }
            catch (Exception e)
            {
                flagValida = String.Empty;
                msgText = "Error Conexion a BD Clarify: " + e.Message;
                retorno = false;
            }
            finally
            {

                obRequest.Factory.Dispose();
            }
            return retorno;
        }
		
        public Int64 validaSolPendientePagoMig(string tipoDocumento, string nroDocumento, string nroTelefono)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String, 16, ParameterDirection.Input),
                new DAABRequest.Parameter("P_NUMERO", DbType.String,11,ParameterDirection.Input),				
			};

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[1].Value = tipoDocumento;
            arrParam[2].Value = Funciones.FormatoNroDocumentoBD(tipoDocumento, nroDocumento);
            arrParam[3].Value = nroTelefono;

            objLog.CrearArchivolog("INICIO validaSolPendientePagoMig ", null, null);
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_SOL_PENDIENTE_PAGO_MIG";
            objRequest.Parameters.AddRange(arrParam);

            Int64 nroSEC = 0;
            IDataReader dr = null;

            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    nroSEC = Funciones.CheckInt64(dr["SEC"]);
                }
                objLog.CrearArchivolog("FIN validaSolPendientePagoMig - Mensaje: Exito", null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("FIN validaSolPendientePagoMig - Mensaje: " + ex.Message, null, null);
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();

                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }

        public Int64 validarSecPendMig(string tipoDocumento, string nroDocumento, string nroTelefono)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String, 16, ParameterDirection.Input),
                new DAABRequest.Parameter("P_NUMERO", DbType.String,11,ParameterDirection.Input)
			};

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;
		
            arrParam[1].Value = tipoDocumento;
            arrParam[2].Value = Funciones.FormatoNroDocumentoBD(tipoDocumento, nroDocumento);
            arrParam[3].Value = nroTelefono;

            objLog.CrearArchivolog("INICIO validarSecPendMig", null, null);
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL  + ".SP_VALIDA_SEC_MIG";
            objRequest.Parameters.AddRange(arrParam);

            Int64 nroSEC = 0;
            IDataReader dr = null;
		
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    nroSEC = Funciones.CheckInt64(dr["SEC"]);
                }
                objLog.CrearArchivolog("FIN validarSecPendMig - Mensaje: Exito", null, null);
            }
            catch (Exception ex)
            {
                nroSEC = 0;
                objLog.CrearArchivolog("FIN validarSecPendMig - Mensaje: " + ex.Message, null, null);
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();

                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }
		
        public DataSet ListarDetalleLineaBSCS(int tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_DOC", DbType.String, 16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CUR_CLIENTE", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_CUR_DETALLE", DbType.Object, ParameterDirection.Output)
			};
            int i = 0; arrParam[i].Value = tipoDocumento;
            i++; arrParam[i].Value = nroDocumento;

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgParametrico + ".SP_DETALLE_X_LINEA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLogDataSet = false;

            objLog.CrearArchivolog("INICIO ListarDetalleLineaBSCS ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada ", arrParam, null);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ListarDetalleLineaBSCS ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ListarDetalleLineaBSCS ", null, null);
                objRequest.Factory.Dispose();
            }
        }

        public DataSet ListarDetalleLineaSGA(string tipoDocumento, string nroDocumento, int intCantMes)
        {
            DAABRequest.Parameter[] arrParam = {
			    new DAABRequest.Parameter("p_documento", DbType.String, 15, ParameterDirection.Input),
			    new DAABRequest.Parameter("p_tipo_documento", DbType.String, 3, ParameterDirection.Input),
			    new DAABRequest.Parameter("p_cant_prom", DbType.Int32, ParameterDirection.Input),
			    new DAABRequest.Parameter("p_cabecera_inf", DbType.Object, ParameterDirection.Output),
			    new DAABRequest.Parameter("p_detalles_inf", DbType.Object, ParameterDirection.Output)
		    };
            int i = 0; arrParam[i].Value = nroDocumento;
            i++; arrParam[i].Value = tipoDocumento;
            i++; arrParam[i].Value = intCantMes;

            objLog.CrearArchivolog("INICIO ListarDetalleLineaSGA ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada ", arrParam, null);

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PqConsultaSiacSrv + ".GET_INFORMACION_CLIENTE";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ListarDetalleLineaSGA ", ex.Message, ex);
                //throw ex;
                return null;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ListarDetalleLineaSGA ", null, null);
                objRequest.Factory.Dispose();
            }
        }

        public DataSet ListarDetalleLineaSISACT(string tipoDocumento, string nroDocumento, string strTelefono)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR_CAB", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_CURSOR_DET", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String, 16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_LISTA_TELEFONO", DbType.String, 32767, ParameterDirection.Input),
			};
            arrParam[2].Value = tipoDocumento;
            arrParam[3].Value = Funciones.FormatoNroDocumentoBD(tipoDocumento, nroDocumento);
            if (strTelefono != string.Empty) arrParam[4].Value = strTelefono;

            objLog.CrearArchivolog("INICIO ListarDetalleLineaSISACT ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada ", arrParam, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_DETALLE_LINEA_SISACT"; //PROY-26963
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ListarDetalleLineaSISACT ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ListarDetalleLineaSISACT ", null, null);
                objRequest.Factory.Dispose();
            }
        }

        public DataTable ListarDetalleLineaFraude(string strFlagBuscar, int tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_FLAG_BUSCAR", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CONDICION", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("CUR_TELEFONO", DbType.Object, ParameterDirection.Output)
		    };
            int i = 0; arrParam[i].Value = strFlagBuscar;
            i++; arrParam[i].Value = nroDocumento;
            i++; arrParam[i].Value = tipoDocumento;

            objLog.CrearArchivolog("INICIO ListarDetalleLineaFraude ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada ", arrParam, null);

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgParametrico + ".SP_BUSCAR_CLIENTE_SEC";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ListarDetalleLineaFraude ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ListarDetalleLineaFraude ", null, null);
                objRequest.Factory.Dispose();
            }
           
        }

        public DataTable ListarDetalleLineaBloqueo(string strFlagBuscar, int tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_FLAG_BUSCAR", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CONDICION", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("CUR_TELEFONO", DbType.Object, ParameterDirection.Output)
		    };
            int i = 0; arrParam[i].Value = strFlagBuscar;
            i++; arrParam[i].Value = nroDocumento;
            i++; arrParam[i].Value = tipoDocumento;

            objLog.CrearArchivolog("INICIO ListarDetalleLineaBloqueo ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada ", arrParam, null);

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgParametrico + ".SP_BUSCAR_CLIENTE_SEC2";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ListarDetalleLineaBloqueo ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ListarDetalleLineaBloqueo ", null, null);
                objRequest.Factory.Dispose();
            }
        }

        public List<BEPlan> ListarDetallePlanCF(string dni, int meses, int condicion)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("CUR_CLIENTE", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_DNI", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_MESES", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CONDICION", DbType.Int16, ParameterDirection.Input)
			};
            arrParam[1].Value = dni;
            arrParam[2].Value = meses;
            arrParam[3].Value = condicion;

            objLog.CrearArchivolog("INICIO ListarDetallePlanCF ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada ", arrParam, null);

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), dni);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgParametrico + ".CLIENTE_CARGOS_FIJOS";
            objRequest.Parameters.AddRange(arrParam);

            BEPlan objItem = null;
            List<BEPlan> objLista = new List<BEPlan>();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        objItem = new BEPlan();
                        objItem.ID_BSCS = Funciones.CheckInt(dr["TMCODE"]);
                        objItem.PLANV_DESCRIPCION = Funciones.CheckStr(dr["DES"]);
                        objItem.PLANN_CAR_FIJ = Funciones.CheckDbl(dr["CF"]);
                        objItem.CANTIDAD = Funciones.CheckInt(dr["ACTIVOS"]);
                        objLista.Add(objItem);
                    }
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ListarDetallePlanCF ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ListarDetallePlanCF ", null, null);
                if (dt != null) dt.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public DataTable ListarCantPlanxBilleteraBSCS(int tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_DOC", DbType.String, 16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CUR_PLANES_PAQ", DbType.Object, ParameterDirection.Output)
			};
            int i = 0; arrParam[i].Value = tipoDocumento;
            i++; arrParam[i].Value = nroDocumento;

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgParametrico + ".SP_DETALLE_X_LINEA_CANT_PLANES";
            objRequest.Parameters.AddRange(arrParam);

            objLog.CrearArchivolog("INICIO ListarCantPlanxBilleteraBSCS ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada =>", arrParam, null);
            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ListarCantPlanxBilleteraBSCS ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ListarCantPlanxBilleteraBSCS ", null, null);
                objRequest.Factory.Dispose();
            }
        }

        public List<BEPlanBilletera> ListarDetallePlanesCorporativo(int tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NUM_DOC", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLANES", DbType.Object,ParameterDirection.Output)												   
			};
            arrParam[0].Value = tipoDocumento;
            arrParam[1].Value = nroDocumento;

            objLog.CrearArchivolog("INICIO ListarDetallePlanesCorporativo ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada => ", arrParam, null);

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.TIM142_PKG_EMPRESAS + ".CONSULTAR_CLIENTE";
            objRequest.Parameters.AddRange(arrParam);

            BEPlanBilletera objItem = null;
            List<BEPlanBilletera> objLista = new List<BEPlanBilletera>();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        objItem = new BEPlanBilletera();
                        objItem.plan = Funciones.CheckStr(dr["TMCODE"]);
                        objItem.CF = Funciones.CheckFloat(dr["CF"]);

                        objItem.tipoPlan = BEPlanBilletera.TIPO_PLAN.MOVIL;
                        if (Funciones.CheckInt(dr["TIPO"]) == (int)BEPlanBilletera.TIPO_PLAN.DATOS) objItem.tipoPlan = BEPlanBilletera.TIPO_PLAN.DATOS;

                        objItem.tipoFacturador = BEPlanBilletera.TIPO_FACTURADOR.BSCS;
                        objLista.Add(objItem);
                    }
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ListarDetallePlanesCorporativo ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ListarDetallePlanesCorporativo ", null, null);
                if (dt != null) dt.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public DataTable ListarCantidadLineasActivas(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
												new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)};
            arrParam[0].Value = nroSEC;

            objLog.CrearArchivolog("INICIO ListarCantidadLineasActivas ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada => ", arrParam, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SP_CON_NRO_LINEA_ACTIVAS_SEC";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ListarCantidadLineasActivas ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ListarCantidadLineasActivas ", null, null);
                objRequest.Factory.Dispose();
            }
        }

        public int ObtenerComportamientoPago(int tipoDocumento, string nroDocumento, ref BEItemMensaje objMensaje)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("p_tip_doc", DbType.Int16, ParameterDirection.Input),
			    new DAABRequest.Parameter("p_num_doc", DbType.String, 15, ParameterDirection.Input),
			    new DAABRequest.Parameter("p_indicador", DbType.Int16, ParameterDirection.Output),
			    new DAABRequest.Parameter("p_cod_err", DbType.Int16, ParameterDirection.Output),
			    new DAABRequest.Parameter("p_msg_err", DbType.String, 100, ParameterDirection.Output)
            };
            arrParam[0].Value = tipoDocumento;
            arrParam[1].Value = nroDocumento;

            objLog.CrearArchivolog("INICIO ObtenerComportamientoPago ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada => ", arrParam, null);

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.Tim127CompPago;
            objRequest.Parameters.AddRange(arrParam);

            objMensaje = new BEItemMensaje();
            int CP = 0, codError;
            bool blnOK;
            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter p1, p2, p3;
                p1 = (IDataParameter)objRequest.Parameters[2];
                p2 = (IDataParameter)objRequest.Parameters[3];
                p3 = (IDataParameter)objRequest.Parameters[4];
                CP = Funciones.CheckInt(p1.Value);
                codError = Funciones.CheckInt(p2.Value);
                blnOK = (codError == 0);

                objMensaje = new BEItemMensaje(Funciones.CheckStr(p2.Value), Funciones.CheckStr(p3.Value), blnOK);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ObtenerComportamientoPago ", ex.Message, ex);
                objMensaje = new BEItemMensaje(ex.Source, ex.Message, false);
            }
            finally
            {
                objLog.CrearArchivolog("FIN ObtenerComportamientoPago ", null, null);
                objRequest.Factory.Dispose();
            }
            return CP;
        }

        public Int64 ObtenerSOTxMigracion(string tipoDocumento, string nroDocumento)
        {
            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.Text;
            objRequest.Command = string.Format("select {0}.get_solot('{1}', '{2}') from dual", BaseDatos.pq_migracion, tipoDocumento, nroDocumento);

            Int64 nroSOT;
            try
            {
                nroSOT = Funciones.CheckInt64(objRequest.Factory.ExecuteScalar(ref objRequest));
            }
            catch (Exception)
            {
                nroSOT = 0;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return nroSOT;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public void ConsultaTopBlackWhiteList(string tipoDocumento, string nroDocumento, int intDiasDeuda, double dblDeuda, int intLineasActivas,
                                              int intLineasBloqueo, ref bool blnBlack, ref bool blnWhite, ref bool blnTop)
        {
            DAABRequest.Parameter[] arrParam = {
			    new DAABRequest.Parameter("P_EXISTE_BL", DbType.String, 2, ParameterDirection.Output),
			    new DAABRequest.Parameter("P_EXISTE_WH", DbType.String, 2, ParameterDirection.Output),
			    new DAABRequest.Parameter("P_EXISTE_TP", DbType.String, 2, ParameterDirection.Output),
			    new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, 4, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_NRO_DOC", DbType.String, 16, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_DIAS_DEUDA", DbType.Int64, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_DEUDA", DbType.Double, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_LINEA_ACT", DbType.Int64, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_LINEA_BLOQ", DbType.Int64, ParameterDirection.Input)
		    };
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 3; arrParam[i].Value = tipoDocumento;
            i++; arrParam[i].Value = nroDocumento;
            i++; arrParam[i].Value = intDiasDeuda;
            i++; arrParam[i].Value = dblDeuda;
            i++; arrParam[i].Value = intLineasActivas;
            i++; arrParam[i].Value = intLineasBloqueo;

            objLog.CrearArchivolog("INICIO ConsultaTopBlackWhiteList ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada => ", arrParam, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CON_BLACK_WHITELIST";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                blnBlack = (Funciones.CheckStr(((IDataParameter)objRequest.Parameters[0]).Value) == "1");
                blnWhite = (Funciones.CheckStr(((IDataParameter)objRequest.Parameters[1]).Value) == "1");
                blnTop = (Funciones.CheckStr(((IDataParameter)objRequest.Parameters[2]).Value) == "1");
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ConsultaTopBlackWhiteList ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ConsultaTopBlackWhiteList ", null, null);
                objRequest.Factory.Dispose();
            }
        }

        public string ConsultaBlackListComisiones(string tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_EXISTE", DbType.String, ParameterDirection.Output),
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_DOC", DbType.String, 16, ParameterDirection.Input)
			};
          
            GeneradorLog objLog = new GeneradorLog(null, nroDocumento,null,"DATA_LOG");
            objLog.CrearArchivolog("[Inicio][ConsultaBlackListComisiones]",null,null);

            arrParam[1].Value = tipoDocumento;
            arrParam[2].Value = nroDocumento;

            objLog.CrearArchivolog("[Parametro entrada]", tipoDocumento, null);
            objLog.CrearArchivolog("[Parametro entrada]", nroDocumento, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Transactional = true;
            objRequest.Parameters.Clear();
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CON_BLACKLIST_COMISIONES";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[0];
                return (Funciones.CheckInt(parSalida1.Value) > 0) ? "S" : "N";
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR] ConsultaBlackListComisiones ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("[Fin][ConsultaBlackListComisiones]", null, null);
                objRequest.Factory.Dispose();
            }
        }

        public List<BEPuntoVenta> ListarBlackListPdv()
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
	        };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_MAESTROS + ".SISACT_CON_BLACKLIST_CANALPDV";
            objRequest.Parameters.AddRange(arrParam);

            BEPuntoVenta objItem;
            List<BEPuntoVenta> objLista = new List<BEPuntoVenta>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    objItem = new BEPuntoVenta();
                    objItem.CanacCodigo = Funciones.CheckStr(dr["COD_CANAL"]);
                    objItem.OvencCodigo = Funciones.CheckStr(dr["COD_PUNTO_VENTA"]);
                    objLista.Add(objItem);
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
            return objLista;
        }

        public int ValidarBlackListPdv(string strCodCanal, string strCodPdv)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("K_RESULTADO", DbType.Int64,ParameterDirection.Output),
				new DAABRequest.Parameter("P_COD_CANAL", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_COD_PDV", DbType.String,10,ParameterDirection.Input)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[1].Value = strCodCanal;
            arrParam[2].Value = strCodPdv;

            int retorno = 0;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_MAESTROS + ".SISACT_GET_BL_PDVUSUARIO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[0];
                retorno = Funciones.CheckInt(parSalida1.Value);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return retorno;
        }

        public bool ValidarVendedor(string dniVendedor, string strOficina, ref string strMensaje, ref string strIdVendedor)
        {
            DAABRequest.Parameter[] arrParam = {												   
				new DAABRequest.Parameter("P_VEND_DNI", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PTO_VTA", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = dniVendedor;
            arrParam[1].Value = strOficina;

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_MAESTROS + ".SISACSS_VERIF_VENDEDOR";
            objRequest.Parameters.AddRange(arrParam);

            BEVendedor vendedor = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    vendedor = new BEVendedor();
                    vendedor.VendedorId = Funciones.CheckInt64(dr["VENDEDOR_ID"]);
                    vendedor.Nombre = Funciones.CheckStr(dr["NOMBRES"]);
                    vendedor.EstadoId = Funciones.CheckStr(dr["ESTADO_ID"]);

                    vendedor.EstadoDescripcion = Funciones.CheckStr(dr["ESTADO_DESC"]);
                    vendedor.PuntoVentaId = Funciones.CheckStr(dr["PUNTO_VENTA"]);
                    vendedor.FlagBl = Funciones.CheckStr(dr["FLAG_BL"]);
                    vendedor.IdDistribuidor = Funciones.CheckStr(dr["DISTC_CODIGO"]);
                    vendedor.IdDistribuidorVendedor = Funciones.CheckStr(dr["VEN_DAC_ACT"]);
                }

                if (vendedor != null)
                {
                    if (vendedor.EstadoId != "02")
                    {
                        if (vendedor.FlagBl == "1")
                        {
                            strMensaje = "Vendedor no autorizado. Contactarse con Soporte Operacional (soporteoperacional@claro.com.pe).";
                        }
                        else
                        {
                            strMensaje = "El Vendedor " + vendedor.Nombre.ToString() + " se encuentra en estado: " + vendedor.EstadoDescripcion.ToString() + ".";
                        }
                        strIdVendedor = "";
                    }
                    else
                    {
                        if (vendedor.IdDistribuidorVendedor != vendedor.IdDistribuidor)
                        {
                            strMensaje = "El vendedor no pertenece al Punto de Venta seleccionado.";
                            strIdVendedor = "";
                        }
                        else
                        {
                            strMensaje = "La venta se asignará al vendedor " + vendedor.Nombre.ToString() + ".";
                            strIdVendedor = vendedor.VendedorId.ToString();
                            salida = true;
                        }
                    }

                }
                else
                {
                    strMensaje = "El DNI no pertenece a ningún vendedor.";
                    strIdVendedor = "";
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
            return salida;
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public DataTable ObtenerInformacionCrediticia(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("P_NROSEC", DbType.Int64, ParameterDirection.Input)
            };
            arrParam[1].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CON_DATOS_CREDITOS";
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

        public DataTable ObtenerInformacionBilletera(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input)
            };
            arrParam[1].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CON_DATOS_BILLETERA";
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

        public DataTable ObtenerInformacionGarantia(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
			};

            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SP_CON_GARANTIA_SEC";
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

        public DataTable ObtenerInformacionGarantiaII(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("p_cursor", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("p_solin_grupo_sec", DbType.Int64, ParameterDirection.Input)
			};
            arrParam[1].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CON_GARANTIA_SEC";
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
        public void ObtenerPlanesSolicitud(Int64 nroSEC, ref List<BEPlanDetalleVenta> listaPlanes, ref List<BESecServicio_AP> listaServicios)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_PLAN", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("K_CUR_SERVICIO", DbType.Object, ParameterDirection.Output)
			};

            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            string[] sTab = { "planes", "servicios" };
            objRequest.TableNames = sTab;
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_CON_PLAN_DETALLE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {

                    listaPlanes.Add(new BEPlanDetalleVenta()
                    {
                        SOLIN_CODIGO = Funciones.CheckInt64(dr["SOLIN_CODIGO"]),
                        SOPLN_CODIGO = Funciones.CheckInt64(dr["SOPLN_CODIGO"]),
                        SOPLN_SECUENCIA = Funciones.CheckInt64(dr["SOPLN_SECUENCIA"]),
                        SOPLN_ORDEN = Funciones.CheckInt(dr["SOPLN_ORDEN"]),
                        PAQTV_CODIGO = Funciones.CheckStr(dr["PAQTV_CODIGO"]),
                        PAQTV_DESCRIPCION = Funciones.CheckStr(dr["PAQTV_DESCRIPCION"]),
                        PLANC_CODIGO = Funciones.CheckStr(dr["PLANC_CODIGO"]),
                        PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLANV_DESCRIPCION"]),
                        TPROC_CODIGO = Funciones.CheckStr(dr["TPROC_CODIGO"]),
                        SOPLN_CANTIDAD = Funciones.CheckInt(dr["SOPLN_CANTIDAD"]),
                        CARGO_FIJO = Funciones.CheckDbl(dr["SOPLN_MONTO_UNIT"]),
                        SOPLC_MONTO_TOTAL = Funciones.CheckDbl(dr["SOPLC_MONTO_TOTAL"]),
                        TOPE_MONTO = Funciones.CheckDbl(dr["SOPLN_TOPE_MONTO"]),
                        TELEFONO = Funciones.CheckStr(dr["TELEFONO"]),
                        MATERIAL = Funciones.CheckStr(dr["MATERIAL"]),
                        MATERIAL_DESC = Funciones.CheckStr(dr["MATERIAL_DESC"]),
                        PACUC_CODIGO = Funciones.CheckStr(dr["PACUC_CODIGO"]),
                        PACUV_DESCRIPCION = Funciones.CheckStr(dr["PACUV_DESCRIPCION"]),
                        CAMPANA = Funciones.CheckStr(dr["CAMPANA"]),
                        CAMPANA_DESC = Funciones.CheckStr(dr["CAMPANA_DESC"]),
                        LISTA_PRECIO = Funciones.CheckStr(dr["LISTA_PRECIO"]),
                        LISTA_PRECIO_DESC = Funciones.CheckStr(dr["LISTA_PRECIO_DESC"]),
                        PRECIO_LISTA = Funciones.CheckDbl(dr["PRECIO_LISTA"]),
                        PRECIO_VENTA = Funciones.CheckDbl(dr["PRECIO_VENTA"]),
                        CUOTA_DESCRIPCION = Funciones.CheckStr(dr["CUOTA_DESCRIPCION"]),
                        CUOTA_CODIGO = Funciones.CheckStr(dr["CUOTA_CODIGO"]),
                        CUOTA_INICIAL = Funciones.CheckDbl(dr["CUOTA_INICIAL"]),
                        SOPLV_PAQU_AGRU = Funciones.CheckStr(dr["SOPLV_PAQU_AGRU"]),
                        SUBSIDIO = Funciones.CheckStr(dr["SUBSIDIO_EQUIPO"]),
                        CARGO_FIJO_LIN = Funciones.CheckDbl(dr["CARGO_FIJO_LIN"]),
                        SOLIN_CF_ALQUILER_KIT = Funciones.CheckDbl(dr["SOLIN_CF_ALQUILER_KIT"]),
                        SOLIN_COSTO_INST_DTH = Funciones.CheckDbl(dr["SOLIN_COSTO_INST_DTH"]),
                        SOLIN_COSTO_INST_EVAL_DTH = Funciones.CheckDbl(dr["SOLIN_COSTO_INST_EVAL_DTH"]),
                        CASO_ESPECIAL = Funciones.CheckStr(dr["CASO_ESPECIAL"]),
                        OFERTA = Funciones.CheckStr(dr["OFERTA"]),
                        TIPO_PRODUCTO = Funciones.CheckStr(dr["TIPO_PRODUCTO"]),
                        FLAG_PORTABILIDAD = Funciones.CheckStr(dr["FLAG_PORTABILIDAD"]),
                        RIESGO = Funciones.CheckStr(dr["RIESGO"]),
                        TIPO_OFICINA = Funciones.CheckStr(dr["TIPO_OFICINA"]),
                        OFICINA = Funciones.CheckStr(dr["OFICINA"]),
                        TOPEN_CODIGO = Funciones.CheckStr(dr["TOPEN_CODIGO"]),
                        PRDV_DESCRIPCION = Funciones.CheckStr(dr["PRDV_DESCRIPCION"]),
                        PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"]),
                        ID_SOLUCION = Funciones.CheckInt64(dr["ID_SOLUCION"]),
                        SOLUCION = Funciones.CheckStr(dr["SOLUCION"]),
                        IDDET = Funciones.CheckInt64(dr["IDDET"]),
                        IDPRODUCTO = Funciones.CheckInt64(dr["IDPRODUCTO"]),
                        IDLINEA = Funciones.CheckInt64(dr["IDLINEA"]),
                        RIESGO_TOTAL_EQUIPO = Funciones.CheckStr(dr["RIESGOTOTALEQUIPO"]),
                        CAPACIDAD_PAGO = Funciones.CheckStr(dr["CAPACIDADDEPAGO"]),
                        EXONERACION_RENTAS = Funciones.CheckStr(dr["EXONERACIONDERENTAS"]),
                        SUBTOTAL = Funciones.CheckFloat(dr["SUBTOTAL"]),
                        TOPE_CONSUMO = Funciones.CheckStr(dr["TOPE_CODIGO"]),
                        TOPE_CONSUMO_DESC = Funciones.CheckStr(dr["TOPE_DESCRIPCION"])
                    });
                    }

                dr.NextResult();

                while (dr.Read())
                {

                    listaServicios.Add(new BESecServicio_AP()
                    {
                        SOPLN_CODIGO = Funciones.CheckInt(dr["SOPLN_CODIGO"]),
                        SERVV_CODIGO = Funciones.CheckStr(dr["SERVV_CODIGO"]),
                        SERVV_DESCRIPCION = Funciones.CheckStr(dr["DESCRIPCION"]),
                        CARGO_FIJO_BASE = Funciones.CheckDbl(dr["SERVN_PRECIO_SERV"]),
                        SERVC_PLAZO = Funciones.CheckStr(dr["SERVC_PLAZO"]),
                        SERVD_FECHA_ACTIVACION = Funciones.CheckDate(dr["SERVD_FECHA_ACTIVACION"]),
                        SERVD_FECHA_DESACTIVACION = Funciones.CheckDate(dr["SERVD_FECHA_DESACTIVACION"]),
                        IDDET = Funciones.CheckInt64(dr["IDDET"]),
                        IDPRODUCTO = Funciones.CheckInt64(dr["IDPRODUCTO"]),
                        IDLINEA = Funciones.CheckInt64(dr["IDLINEA"]),
                        PLSVN_CODIGO = Funciones.CheckInt(dr["PLSVN_CODIGO"])
                    });
                    }
                }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }

        //PROY-24740
        public bool InsertarPlanSolicitud(BEPlanDetalleVenta oPlan, ref Int64 idSol)
        {
            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), oPlan.SOLIN_CODIGO.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;

            if (oPlan.PRDC_CODIGO == ConfigurationManager.AppSettings["constTipoProductoDTH"].ToString())
            {
                DAABRequest.Parameter[] arrParam = {
					new DAABRequest.Parameter("P_RESULTADO" ,DbType.Int64,ParameterDirection.Output),
					new DAABRequest.Parameter("P_SOLIN_CODIGO" ,DbType.Int64,ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLC_MONTO_TOTAL" ,DbType.Double,ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLN_MONTO_UNIT" ,DbType.Double,ParameterDirection.Input),
					new DAABRequest.Parameter("P_PLANC_CODIGO" ,DbType.String,5,ParameterDirection.Input),
					new DAABRequest.Parameter("P_TPROC_CODIGO" ,DbType.String,2,ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLN_CANTIDAD" ,DbType.Int64,ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLN_ORDEN", DbType.Int16, ParameterDirection.Input)
				};

                for (int i = 0; i < arrParam.Length; i++)
                    arrParam[i].Value = DBNull.Value;

                arrParam[1].Value = oPlan.SOLIN_CODIGO;
                arrParam[2].Value = oPlan.CARGO_FIJO;
                arrParam[3].Value = oPlan.CARGO_FIJO;
                arrParam[4].Value = oPlan.PLANC_CODIGO;
                arrParam[5].Value = oPlan.TPROC_CODIGO;
                arrParam[6].Value = oPlan.SOPLN_CANTIDAD;
                arrParam[7].Value = oPlan.SOPLN_ORDEN;

                objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SISACT_INSERTAR_SOL_PLANES_DTH";
                objRequest.Parameters.AddRange(arrParam);
            }
            else
            {
                DAABRequest.Parameter[] arrParam = {
					new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
					new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
					new DAABRequest.Parameter("P_PLANC_CODIGO", DbType.String, 4, ParameterDirection.Input),
					new DAABRequest.Parameter("P_TPROC_CODIGO", DbType.String, 2, ParameterDirection.Input),
					new DAABRequest.Parameter("P_PAQTV_CODIGO", DbType.String, 5, ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLN_SECUENCIA", DbType.Int64, ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLN_CANTIDAD", DbType.Int64, ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLN_MONTO_UNIT", DbType.Double, ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLC_MONTO_TOTAL", DbType.Double, ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLN_TOPE_CONSUMO", DbType.Int64, ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLN_TOPE_MONTO", DbType.Double, ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLN_TOPE_CF", DbType.Double, ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLV_PAQU_AGRU", DbType.String, ParameterDirection.Input),
					new DAABRequest.Parameter("P_SOPLN_ORDEN", DbType.Int16, ParameterDirection.Input)
//gaa20161024
                    ,new DAABRequest.Parameter("P_SOPLC_FAMILIA", DbType.String, 4, ParameterDirection.Input)
//fin gaa20161024
				};

                for (int i = 0; i < arrParam.Length; i++)
                    arrParam[i].Value = DBNull.Value;

                arrParam[1].Value = oPlan.SOLIN_CODIGO;
                arrParam[2].Value = oPlan.PLANC_CODIGO;
                arrParam[3].Value = oPlan.TPROC_CODIGO;
                arrParam[4].Value = oPlan.PAQTV_CODIGO;
                arrParam[5].Value = oPlan.SOPLN_SECUENCIA;
                arrParam[6].Value = oPlan.SOPLN_CANTIDAD;
                arrParam[7].Value = oPlan.CARGO_FIJO;
                arrParam[8].Value = oPlan.CARGO_FIJO;
                arrParam[9].Value = oPlan.TOPE_CONSUMO;
                arrParam[10].Value = Funciones.CheckDbl(oPlan.TOPE_MONTO);
                if (oPlan.SOPLN_SECUENCIA > 0)
                    arrParam[12].Value = oPlan.SOPLV_PAQU_AGRU;

                arrParam[13].Value = oPlan.SOPLN_ORDEN;
//gaa20161024
                arrParam[14].Value = oPlan.FAMILIA_PLAN;
//fin gaa20161024

                objLog.CrearArchivolog("INC000003848031-InsertarPlanSolicitud - SEC: " + Funciones.CheckStr( oPlan.SOLIN_CODIGO) + " - TOPE_CONSUMO : " + Funciones.CheckStr(oPlan.TOPE_CONSUMO), null, null);//INC000003848031

                objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_INS_SOL_PLAN";
                objRequest.Parameters.AddRange(arrParam);

            }
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter pSalida1;
                pSalida1 = (IDataParameter)objRequest.Parameters[0];
                idSol = Funciones.CheckInt64(pSalida1.Value);
                if (idSol > 0) salida = true;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public bool InsertarPlanDetalleVenta(BEPlanDetalleVenta oPlan)
        {
            DAABRequest.Parameter[] arrParam = {
			    new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
			    new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TELEFONO", DbType.String, 15, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_MATERIAL", DbType.String, 18, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_MATERIAL_DESC", DbType.String, 100, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PACUC_CODIGO", DbType.String, 2, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PLAZO_DESC", DbType.String, 40, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CAMPANA", DbType.String, 4, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CAMPANA_DESC", DbType.String, 40, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_LISTA_PRECIO", DbType.String, 4, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_LISTA_PRECIO_DESC", DbType.String, 40, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PRECIO_LISTA", DbType.Double, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PRECIO_VENTA", DbType.Double, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CUOTA_CODIGO", DbType.String, 4, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CUOTA_INICIAL", DbType.Double, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SUBSIDIO", DbType.String, 50, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CARGO_FIJO_LIN", DbType.Double, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PRDC_CODIGO", DbType.String, 2, ParameterDirection.Input),
       new DAABRequest.Parameter("P_MODALIDAD_VENTA", DbType.String, 2, ParameterDirection.Input),
       //PROY-29215 INCIO              
       new DAABRequest.Parameter("P_FORMAPAGOINTS", DbType.String, 20, ParameterDirection.Input)
       //PROY-29215 FIN
		    };
            bool salida = false;
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = oPlan.SOLIN_CODIGO;
            i++; arrParam[i].Value = oPlan.SOPLN_CODIGO;
            i++; arrParam[i].Value = oPlan.TELEFONO;
            i++; arrParam[i].Value = oPlan.MATERIAL;
            i++; arrParam[i].Value = oPlan.MATERIAL_DESC;
            i++; arrParam[i].Value = oPlan.PACUC_CODIGO;
            i++; arrParam[i].Value = oPlan.PACUV_DESCRIPCION;
            i++; arrParam[i].Value = oPlan.CAMPANA;
            i++; arrParam[i].Value = oPlan.CAMPANA_DESC;
            i++; arrParam[i].Value = oPlan.LISTA_PRECIO;
            i++; arrParam[i].Value = oPlan.LISTA_PRECIO_DESC;
            i++; arrParam[i].Value = oPlan.PRECIO_LISTA;
            i++; arrParam[i].Value = oPlan.PRECIO_VENTA;
            //PROY-29215 INICIO
            if (oPlan.PRDC_CODIGO == ConfigurationManager.AppSettings["constTipoProductoDTH"] ) 
            {
                i++; arrParam[i].Value = oPlan.CUOTA_PAGO;
            }
            else{
            i++; arrParam[i].Value = oPlan.CUOTA_CODIGO;
            }
            //PROY-29215 FIN 
            
            i++; arrParam[i].Value = oPlan.CUOTA_INICIAL;
            i++; arrParam[i].Value = oPlan.SUBSIDIO;
            i++; arrParam[i].Value = oPlan.CARGO_FIJO_LIN;
            i++; arrParam[i].Value = oPlan.PRDC_CODIGO;
            i++; arrParam[i].Value = oPlan.MODALIDAD_VENTA;
            //PROY-29215 INICIO
            i++; arrParam[i].Value = oPlan.FORMA_PAGO;
            //PROY-29215 FIN
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), oPlan.SOLIN_CODIGO.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_INS_SOL_PLAN_VENTA";
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

        public bool InsertarDetalleVentaVarios(BEPlanDetalleVenta oPlan, ref Int64 idSol)
        {
            DAABRequest.Parameter[] arrParam = {
			    new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
			    new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_COMBV_CODIGO", DbType.String, 15, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_COMBV_DESCRIPCION", DbType.String, 50, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CAMPV_CODIGO", DbType.String, 15, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CAMPV_DESCRIPCION", DbType.String, 50, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_MATV_CODIGO", DbType.String, 15, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_MATV_DESCRIPCION", DbType.String, 50, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_LISTA_PRECIO", DbType.String, 15, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_LISTA_PRECIO_DESC", DbType.String, 50, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CUOTA_CODIGO", DbType.String, 15, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CUOTA_INICIAL", DbType.String, 15, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_COSTO", DbType.Double, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PRECIO_VENTA", DbType.Double, ParameterDirection.Input)
		    };
            bool salida = false;
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = oPlan.SOLIN_CODIGO;
            i++; arrParam[i].Value = oPlan.IDCOMBO;
            i++; arrParam[i].Value = oPlan.COMBO;
            i++; arrParam[i].Value = oPlan.CAMPANA;
            i++; arrParam[i].Value = oPlan.CAMPANA_DESC;
            i++; arrParam[i].Value = oPlan.MATERIAL;
            i++; arrParam[i].Value = oPlan.MATERIAL_DESC;
            i++; arrParam[i].Value = oPlan.LISTA_PRECIO;
            i++; arrParam[i].Value = oPlan.LISTA_PRECIO_DESC;
            i++; arrParam[i].Value = oPlan.CUOTA_CODIGO;
            i++; arrParam[i].Value = oPlan.CUOTA_INICIAL;
            i++; arrParam[i].Value = oPlan.PRECIO_LISTA;
            i++; arrParam[i].Value = oPlan.PRECIO_VENTA;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), oPlan.SOLIN_CODIGO.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_INS_SOL_VENTA_EQUIPO";
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

        //PROY-24740
        public bool InsertarPlanServicio(BEPlanDetalleVenta objDetallePlan, string idOficina)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVV_CODIGO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PRECIO_SERV", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_USUARIO_CREA", DbType.String, 15, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVC_ESTADO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVC_PLAZO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVD_FECHA_ACTIVACION", DbType.Date, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVD_FECHA_DESACTIVACION", DbType.Date, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVC_ORIGEN", DbType.String, 15, ParameterDirection.Input),
                new DAABRequest.Parameter("P_PLNV_CODIGO", DbType.String, 5, ParameterDirection.Input),
                new DAABRequest.Parameter("P_PRDC_CODIGO", DbType.String, 5, ParameterDirection.Input),
                new DAABRequest.Parameter("P_IDTOPEMONTO", DbType.Int64, ParameterDirection.Input), // PROY-29296
			};
            bool salida = false;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_ROAMING_SERV + ".SISASI_PLAN_SERV_DESP"; //PROY-29296 REPLICA: SISACT_INS_SOL_PLAN_SERV_DESP
            objRequest.SaveLog = false;
            try
            {
                List<BESecServicio_AP> listaServicio = (List<BESecServicio_AP>)objDetallePlan.SERVICIO;

                foreach (BESecServicio_AP oServicio in listaServicio)
                {
                    for (int i = 0; i < arrParam.Length; i++)
                        arrParam[i].Value = DBNull.Value;

                    arrParam[1].Value = objDetallePlan.SOLIN_CODIGO;
                    arrParam[2].Value = objDetallePlan.SOPLN_CODIGO;
                    arrParam[3].Value = oServicio.SERVV_CODIGO;
                    arrParam[4].Value = oServicio.CARGO_FIJO_BASE;
                    arrParam[5].Value = oServicio.SERVV_USUARIO_CREA;
                    if (oServicio.SERVC_ESTADO != null)
                    {
                        arrParam[6].Value = oServicio.SERVC_ESTADO;
                    }
                    if (oServicio.SERVC_PLAZO != null)
                    {
                        arrParam[7].Value = oServicio.SERVC_PLAZO;
                    }
                    if (oServicio.SERVD_FECHA_ACTIVACION != System.DateTime.MinValue)
                    {
                        arrParam[8].Value = oServicio.SERVD_FECHA_ACTIVACION;
                    }
                    if (oServicio.SERVD_FECHA_DESACTIVACION != System.DateTime.MinValue)
                    {
                        arrParam[9].Value = oServicio.SERVD_FECHA_DESACTIVACION;
                    }
                    arrParam[10].Value = ConfigurationManager.AppSettings["ConstSistemaConsumer"].ToString();
                    arrParam[11].Value = objDetallePlan.PLANC_CODIGO;
                    arrParam[12].Value = objDetallePlan.PRDC_CODIGO;
                    arrParam[13].Value = oServicio.SERVN_ID_MONTO_TOPE; // PROY-29296

                    objRequest.Parameters.Clear();
                    objRequest.Parameters.AddRange(arrParam);
                    objRequest.Factory.ExecuteNonQuery(ref objRequest);

                    /*********** Registro Servicio Roaming *****************/
                    if (oServicio.SERVV_CODIGO.Equals(ConfigurationManager.AppSettings["codServRoamingI"].ToString()))
                    {
                        bool vExitoRoaming = InsertarPlanServiceRoamig(oServicio, objDetallePlan.SOLIN_CODIGO, objDetallePlan.SOPLN_CODIGO, idOficina, objDetallePlan.PLANC_CODIGO);
                    }
                }
                salida = true;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public bool GrabarVentaDTH(BEVentaDTH item, Int64 nroSEC, ref Int64 idSol)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO" ,DbType.Double,ParameterDirection.Output),
				new DAABRequest.Parameter("P_MSGERR" ,DbType.String,ParameterDirection.Output),
				new DAABRequest.Parameter("P_DOCUMENTO" ,DbType.Int64,ParameterDirection.Output),
				new DAABRequest.Parameter("P_TIPO_DOCUMENTO" ,DbType.String,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CANAL" ,DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_OFICINA_VENTA" ,DbType.String,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DOC_CLIENTE" ,DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_DOC_CLIENTE" ,DbType.String,16,ParameterDirection.Input),
				new DAABRequest.Parameter("P_MONEDA" ,DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TOPEN_CODIGO" ,DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TOTAL_VENTA" ,DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SUBTOTAL_IMPUESTO" ,DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SUBTOTAL_VENTA" ,DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_OBSERVACION" ,DbType.String,200,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TVEN_CODIGO" ,DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NUMERO_REFERENCIA" ,DbType.String,16,ParameterDirection.Input),
				new DAABRequest.Parameter("P_USUARIO_CREA" ,DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NUMERO_CUOTAS" ,DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_VENDEDOR" ,DbType.String,16,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ORG_VENTA" ,DbType.String,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NUMERO_SEC" ,DbType.Int64,ParameterDirection.Input)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 3; arrParam[i].Value = item.TIPO_DOCUMENTO;
            ++i; arrParam[i].Value = item.CANAL;
            ++i; arrParam[i].Value = item.OFICINA_VENTA;
            ++i; arrParam[i].Value = item.TIPO_DOC_CLIENTE;
            ++i; arrParam[i].Value = item.NRO_DOC_CLIENTE;
            ++i; arrParam[i].Value = item.MONEDA;
            ++i; arrParam[i].Value = item.TOPEN_CODIGO;
            ++i; arrParam[i].Value = item.TOTAL_VENTA;
            ++i; arrParam[i].Value = item.SUBTOTAL_IMPUESTO;
            ++i; arrParam[i].Value = item.SUBTOTAL_VENTA;
            ++i; arrParam[i].Value = item.OBSERVACION;
            ++i; arrParam[i].Value = item.TVENC_CODIGO;
            ++i; arrParam[i].Value = item.NUMERO_REFERENCIA;
            ++i; arrParam[i].Value = item.USUARIO_CREA;
            ++i; arrParam[i].Value = item.NUMERO_CUOTAS;
            ++i; arrParam[i].Value = item.VENDEDOR;
            ++i; arrParam[i].Value = item.ORG_VENTA;
            ++i; arrParam[i].Value = nroSEC;

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SP_REGISTRA_VENTA";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter pSalida1;
                pSalida1 = (IDataParameter)objRequest.Parameters[2];
                idSol = Funciones.CheckInt64(pSalida1.Value);
                if (idSol > 0) salida = true;
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

        public bool GrabarVentaDetalleDTH(BEVentaDetalleDTH item, Int64 strIdVenta)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Double,ParameterDirection.Output),
				new DAABRequest.Parameter("P_MSGERR", DbType.String,ParameterDirection.Output),
				new DAABRequest.Parameter("P_CORRELATIVO", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_DOCUMENTO", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_MATERIAL", DbType.String,15,ParameterDirection.Input),
				new DAABRequest.Parameter("P_MATERIAL_DESC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAN", DbType.String,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAN_DESC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPANA", DbType.String,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPANA_DESC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_DESCUENTO", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO", DbType.String,4,ParameterDirection.Input)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 2; arrParam[i].Value = item.ORDEN;
            ++i; arrParam[i].Value = strIdVenta;
            ++i; arrParam[i].Value = item.MATERIAL;
            ++i; arrParam[i].Value = item.MATERIAL_DESC;
            ++i; arrParam[i].Value = item.PLAN;
            ++i; arrParam[i].Value = item.PLAN_DESC;
            ++i; arrParam[i].Value = item.CAMPANA;
            ++i; arrParam[i].Value = item.CAMPANA_DESC;
            ++i; arrParam[i].Value = item.DESCUENTO;
            ++i; arrParam[i].Value = item.PLAZO;

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SP_REGISTRA_VENTA_DETALLE_DTH";
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

        private bool InsertarPlanServiceRoamig(BESecServicio_AP oServicio, Int64 p_Solin_Codigo, Int64 p_Sopln_Codigo, string p_Oven_Codigo, string p_Planc_Codigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVV_CODIGO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLANC_CODIGO", DbType.String, 3, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_SAP", DbType.String, 15, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_NUM_TELF_VENTA", DbType.String, 15, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVN_PRECIO_SERV", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVC_PLAZO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVD_FECHA_ACTIVACION", DbType.Date, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVD_FECHA_DESACTIVACION", DbType.Date, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVC_ORIGEN", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_USUARIO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OVENC_CODIGO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PACUV_DESCRIPCION", DbType.String, 30, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOPLN_CODIGO", DbType.Int64, ParameterDirection.Input)
			};
            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_ROAMING_SERV + ".SISACT_INS_PLAN_SERV_ROAMING";

            try
            {
                for (int i = 0; i < arrParam.Length; i++)
                    arrParam[i].Value = DBNull.Value;

                arrParam[1].Value = p_Solin_Codigo;
                arrParam[2].Value = oServicio.SERVV_CODIGO;
                arrParam[3].Value = p_Planc_Codigo;
                arrParam[6].Value = oServicio.CARGO_FIJO_BASE;
                arrParam[7].Value = oServicio.SERVC_PLAZO;
                arrParam[8].Value = oServicio.SERVD_FECHA_ACTIVACION;
                arrParam[9].Value = oServicio.SERVD_FECHA_DESACTIVACION;
                arrParam[10].Value = ConfigurationManager.AppSettings["ConstSistemaConsumer"].ToString();
                arrParam[11].Value = oServicio.SERVV_USUARIO_CREA;
                arrParam[12].Value = p_Oven_Codigo;
                arrParam[14].Value = p_Sopln_Codigo;

                objRequest.Parameters.Clear();
                objRequest.Parameters.AddRange(arrParam);
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
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

        public bool InsertarPlanHFC(BEPlanSolicitudHFC oPlan, Int64 nroSEC,string PRDC_CODIGO, ref Int64 idSol)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDPAQ", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PAQUETE", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLZAC_CODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPV_CODIGO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPV_DESCRIPCION", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLNV_CODIGO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLNV_DESCRIPCION", DbType.String, 50, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TELEFONO", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_USUARIO_CREA", DbType.String, 10, ParameterDirection.Input),
    new DAABRequest.Parameter("P_CODPRODUCT", DbType.String, 5, ParameterDirection.Input),
    //PROY-29215 INICIO          
    new DAABRequest.Parameter("P_FORMAPAGOINTS", DbType.String,20, ParameterDirection.Input),
    new DAABRequest.Parameter("P_NROCUOTAINTS", DbType.Int32, ParameterDirection.Input)
    //PROY-29215 FIN

			};
            GeneradorLog objLog = new GeneradorLog(null, nroSEC.ToString(), null, "DATA_LOG");
          
            int i;
            bool salida = false;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = oPlan.IdPaquete;
            i++; arrParam[i].Value = oPlan.Paquete;
            i++; arrParam[i].Value = oPlan.IdPlazo;
            i++; arrParam[i].Value = oPlan.Plazo;
            i++; arrParam[i].Value = oPlan.IdCampana;
            i++; arrParam[i].Value = oPlan.Campana;
            i++; arrParam[i].Value = oPlan.IdPlan;
            i++; arrParam[i].Value = oPlan.Plan;
            i++; arrParam[i].Value = oPlan.Telefono;
            i++; arrParam[i].Value = oPlan.Usuario;
            i++; arrParam[i].Value = PRDC_CODIGO;
            //PROY-29215 INICIO
            i++; arrParam[i].Value = oPlan.FormaPago;
            i++; arrParam[i].Value = oPlan.NroCuota;
            //PROY-29215 FIN

            objLog.CrearArchivolog("[Inicio][InsertarPlan]", null, null);
            objLog.CrearArchivolog("[SEC]", nroSEC.ToString(), null);
            objLog.CrearArchivolog("[ID_PAQUETE]", oPlan.IdPaquete.ToString(), null);
            objLog.CrearArchivolog("[PAQUETE]", oPlan.Paquete.ToString(), null);
            objLog.CrearArchivolog("[ID_PLAZO]", oPlan.IdPlazo.ToString(), null);
            objLog.CrearArchivolog("[PLAZO]", oPlan.Plazo.ToString(), null);
            objLog.CrearArchivolog("[ID_CAMPAÑA]", oPlan.IdCampana.ToString(), null);
            objLog.CrearArchivolog("[CAMPAÑA]", oPlan.Campana.ToString(), null);
            objLog.CrearArchivolog("[ID_PLAN]", oPlan.IdPlan.ToString(), null);
            objLog.CrearArchivolog("[PLAN]", oPlan.Plan.ToString(), null);
            objLog.CrearArchivolog("COD_PROD", PRDC_CODIGO.ToString(), null);
            //PROY-29215 INICIO
            objLog.CrearArchivolog("[NroCuota]", oPlan.NroCuota.ToString(), null);
            objLog.CrearArchivolog("[FormaPago]", oPlan.FormaPago.ToString(), null);
            //PROY-29215 FIN

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_INS_SOL_PLAN_HFC";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest); IDataParameter pSalida1;
                pSalida1 = (IDataParameter)objRequest.Parameters[0];
                idSol = Funciones.CheckInt64(pSalida1.Value);
                if (idSol > 0) salida = true;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][InsertarPlan]", null, ex);
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
                objLog.CrearArchivolog("[Fin][InsertarPlan]", null, null);
            }
            return salida;
        }

        //PROY-24740
        public bool InsertarPlanServicioHFC(BEPlanSolicitudHFC oPlanDetalleHFC, Int64 idSolHFC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SLPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDDET", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDPRODUCTO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDLINEA", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PRODUCTO", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PAQUETE", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDSERVICIO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SERVICIO", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDEQUIPO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_EQUIPO", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CF_PRECIO", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CF_LINEA", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLG_PRINCIPAL", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CANT_EVAL", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ORDEN", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_AGRUPA", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLNV_CODIGO", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TOPECONSUMO", DbType.Int64, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TPMN_CODIGO", DbType.Int64, ParameterDirection.Input)   // PROY-29296
                
			};
            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISASI_PLAN_SERV_HFC"; // PROY-29296 REPLICA: SISACT_INS_SOL_PLAN_SERV_HFC

            try
            {
                List<BEPlanDetalleHFC> arrPlanDetalleHFC = (List<BEPlanDetalleHFC>)oPlanDetalleHFC.oServicio;

                foreach (BEPlanDetalleHFC oServicio in arrPlanDetalleHFC)
                {
                    int i = 0;
                    for (i = 0; i < arrParam.Length; i++)
                        arrParam[i].Value = DBNull.Value;

                    i = 1; arrParam[i].Value = idSolHFC;
                    i++; arrParam[i].Value = oServicio.IDDET;
                    i++; arrParam[i].Value = oServicio.IdProducto;
                    i++; arrParam[i].Value = oServicio.IdLinea;
                    i++; arrParam[i].Value = oServicio.Producto;
                    i++; arrParam[i].Value = oServicio.Grupo;
                    i++; arrParam[i].Value = oServicio.IdServicio;
                    i++; arrParam[i].Value = oServicio.Servicio;
                    i++; arrParam[i].Value = oServicio.IdEquipo;
                    i++; arrParam[i].Value = oServicio.Equipo;
                    i++; arrParam[i].Value = oServicio.Precio;
                    i++; arrParam[i].Value = oServicio.CF_Linea;
                    i++; arrParam[i].Value = oServicio.FlagPrincipal;
                    i++; arrParam[i].Value = oServicio.Cantidad;
                    i++; arrParam[i].Value = oServicio.Orden;
                    i++; arrParam[i].Value = oServicio.Agrupa;
                    i++; arrParam[i].Value = oPlanDetalleHFC.IdPlan;

                    if (!string.IsNullOrEmpty(oServicio.IdTope))
                    {
                        i++; arrParam[i].Value = oServicio.IdTope;
                    }
                    i++; arrParam[i].Value = oServicio.Id_MontoTope; // PROY-29296
                    objRequest.Parameters.Clear();
                    objRequest.Parameters.AddRange(arrParam);
                    objRequest.Factory.ExecuteNonQuery(ref objRequest);
                }
                salida = true;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        //PROY-24740
        public bool InsertarPlanPromocionHFC(ArrayList arrPromocionHFC, Int64 idSolHFC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SLPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDDET", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDPRODUCTO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDLINEA", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDPROM", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PROMOCION", DbType.String, 50, ParameterDirection.Input)
			};
            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_INS_SOL_PROMOCION_HFC";

            try
            {
                foreach (BEPlanPromocionHFC oPromocion in arrPromocionHFC)
                {
                    int i = 0;
                    for (i = 0; i < arrParam.Length; i++)
                        arrParam[i].Value = DBNull.Value;

                    i = 1; arrParam[i].Value = idSolHFC;
                    i++; arrParam[i].Value = oPromocion.IDDET;
                    i++; arrParam[i].Value = oPromocion.IdProducto;
                    i++; arrParam[i].Value = oPromocion.IdLinea;
                    i++; arrParam[i].Value = oPromocion.IdPromocion;
                    i++; arrParam[i].Value = oPromocion.Promocion;

                    objRequest.Parameters.Clear();
                    objRequest.Parameters.AddRange(arrParam);
                    objRequest.Factory.ExecuteNonQuery(ref objRequest);
                }
                salida = true;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        //PROY-24740
        public bool InsertarPlanEquipoHFC(BEPlanSolicitudHFC oPlanDetalleHFC, Int64 idSolHFC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SLPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_IDEQUIPO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_EQUIPO", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CF_ALQUILER", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLNV_CODIGO", DbType.String, 10, ParameterDirection.Input),
                                new DAABRequest.Parameter("P_PRDC_CODIGO", DbType.String, 5, ParameterDirection.Input)
			};

            GeneradorLog objLog = new GeneradorLog(null,null,null,"DATA_LOG");
           
            bool salida = false;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_INS_SOL_PLAN_EQUIPO_HFC";

            try
            {
                List<BEPlanEquipoHFC> arrPlanEquipoHFC = (List<BEPlanEquipoHFC>)oPlanDetalleHFC.oEquipo;
                foreach (BEPlanEquipoHFC oEquipo in arrPlanEquipoHFC)
                {
                    int i = 0;
                    for (i = 0; i < arrParam.Length; i++)
                        arrParam[i].Value = DBNull.Value;

                    i = 1; arrParam[i].Value = idSolHFC;
                    i++; arrParam[i].Value = oEquipo.IdEquipo;
                    i++; arrParam[i].Value = oEquipo.Equipo;
                    i++; arrParam[i].Value = oEquipo.CF_Alquiler;
                    i++; arrParam[i].Value = oPlanDetalleHFC.IdPlan;
                    i++; arrParam[i].Value = oEquipo.Prdc_Codigo;

                    objLog.CrearArchivolog("[Inicio][InsertarPlanEquipoHFC]", null, null);
                    objLog.CrearArchivolog("[ID_SOL]", idSolHFC.ToString(), null);
                    objLog.CrearArchivolog("[ID_EQUIPO]", oEquipo.IdEquipo.ToString(), null);
                    objLog.CrearArchivolog("[EQUIPO]", oEquipo.Equipo.ToString(), null);
                    objLog.CrearArchivolog("[ALQUILER]", oEquipo.CF_Alquiler.ToString(), null);
                    objLog.CrearArchivolog("[ID_PLAN]", oPlanDetalleHFC.IdPlan.ToString(), null);
                    objLog.CrearArchivolog("[PRODUCTO]", oEquipo.Prdc_Codigo.ToString(), null);

                    objRequest.Parameters.Clear();
                    objRequest.Parameters.AddRange(arrParam);
                    objRequest.Factory.ExecuteNonQuery(ref objRequest);
                }
                salida = true;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][InsertarPlanEquipo]", null, ex);
                throw;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
                objLog.CrearArchivolog("[Fin][InsertarPlanEquipo]", null, null);
            }
            return salida;
        }

        public void ObtenerCostoAlquilerInstalKIT(string idKit, string idCampana, string idPlazo, ref double pAlquiler, ref double pInstalacion)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_ALQUILER", DbType.Double, ParameterDirection.Output),
				new DAABRequest.Parameter("P_INSTALACION", DbType.Double, ParameterDirection.Output),
				new DAABRequest.Parameter("P_MATERIAL", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPANA", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO", DbType.String, 10, ParameterDirection.Input)
            };
            int i;
            i = 2; arrParam[i].Value = idKit;
            i++; arrParam[i].Value = idCampana;
            i++; arrParam[i].Value = idPlazo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.Transactional = true;
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SP_CON_ALQ_INSTAL_KIT";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                pAlquiler = Funciones.CheckDbl(((IDataParameter)objRequest.Parameters[0]).Value);
                pInstalacion = Funciones.CheckDbl(((IDataParameter)objRequest.Parameters[1]).Value);
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

        public bool GrabarBolsaLinea(string strLineas, Int64 nroSEC, Int64 intVersion, string strUsuario)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_TELEFONO", DbType.String, 32767, ParameterDirection.Input),
				new DAABRequest.Parameter("P_GRUPO_SEC", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_VERSION", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_USUARIO", DbType.String, 10, ParameterDirection.Input)
			};
            int i;
            bool salida = false;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = strLineas;
            i++; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = intVersion;
            i++; arrParam[i].Value = strUsuario;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_INS_TELEFONOS_BOLSA";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter pSalida1;
                pSalida1 = (IDataParameter)objRequest.Parameters[0];

                if (Funciones.CheckStr(pSalida1.Value) == "1")
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

        public double ObtenerPorcentajeLCD(int pIdTipoCliente, int pIdSegmento, int pIdTipoRiesgo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_REGV_TIPO_CLIENTE", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SEGN_CODIGO", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RIEN_CODIGO", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_REGDN_PORCENTAJE_LCD", DbType.Double,ParameterDirection.Output),
			};
            int i;
            double dPorcentaje = 0.0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (pIdTipoCliente > 0) arrParam[i].Value = pIdTipoCliente;
            i++; if (pIdSegmento > 0) arrParam[i].Value = pIdSegmento;
            i++; if (pIdTipoRiesgo > 0) arrParam[i].Value = pIdTipoRiesgo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest pObjRequest = obj.CreaRequest(new StackTrace(true));
            pObjRequest.Transactional = true;
            pObjRequest.CommandType = CommandType.StoredProcedure;
            pObjRequest.Command = BaseDatos.PKG_SISACT_REGLAS + ".SISACTS_CON_PORCENTAJE_LCD";
            pObjRequest.Parameters.AddRange(arrParam);

            try
            {
                pObjRequest.Factory.ExecuteNonQuery(ref pObjRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)pObjRequest.Parameters[3];
                dPorcentaje = Funciones.CheckDbl(parSalida1.Value);
            }
            catch
            {
                dPorcentaje = 0.0;
            }
            finally
            {
                pObjRequest.Factory.Dispose();
            }

            return dPorcentaje;
        }

        public string ConsultaBlackListCuotaPdv(string oficina)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RETORNO", DbType.Int16, ParameterDirection.Output),
				new DAABRequest.Parameter("P_OFICINA", DbType.String, 5, ParameterDirection.Input)
			};
            string retorno = "";
            arrParam[1].Value = oficina;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_BLACKLIST_CUOTA_PDV";
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)obRequest.Parameters[0];
                retorno = ((Funciones.CheckInt(parSalida1.Value) > 0) ? "S" : "N");
            }
            catch
            {
                retorno = "N";
            }
            finally
            {
                obRequest.Factory.Dispose();
                obRequest.Parameters.Clear();
            }
            return retorno;
        }
//gaa20151201
        public DataTable ListarSecReno(string strNroDocIde)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("V_NRODOC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_RESULTADO", DbType.Object, ParameterDirection.Output)
		    };
            int i = 0; arrParam[i].Value = strNroDocIde;

            objLog.CrearArchivolog("INICIO ListarSecReno ", null, null);
            objLog.CrearArchivolog("    Parametro Entrada ", arrParam, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strNroDocIde);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactVentasExpress + ".SP_VALIDAR_VR_COMBO_EVAL";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR ListarSecReno ", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("FIN ListarSecReno ", null, null);
                objRequest.Factory.Dispose();
            }

        }
//fin gaa20151201

//gaa20151929
        public bool ValidacionAccesoOpcionEP(string strCanal, string strProducto, string strTipoOperacion, string strTipoDocumento, string strTipoValidacion)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CANAL", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PRODUCTO", DbType.String, 5, ParameterDirection.Input),
    			new DAABRequest.Parameter("P_TIPOOPERACION", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPODOCUMENTO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPOVALIDACION", DbType.String, 5, ParameterDirection.Input),
                new DAABRequest.Parameter("P_RESULTADO", DbType.Int16, ParameterDirection.Output),
				new DAABRequest.Parameter("P_CODIGO_RESPUESTA", DbType.Int32, ParameterDirection.Output),
				new DAABRequest.Parameter("P_MENSAJE_RESPUESTA", DbType.String, 1000, ParameterDirection.Output)
			};
            bool booResultado = true;
            arrParam[0].Value = strCanal;
            arrParam[1].Value = strProducto;
            arrParam[2].Value = strTipoOperacion;
            arrParam[3].Value = strTipoDocumento;
            arrParam[4].Value = strTipoValidacion;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_GENERAL_II + ".SSAPSS_VALIDAEQPROM";
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)obRequest.Parameters[5];
                booResultado = ((Funciones.CheckInt(parSalida1.Value) > 0) ? true : false);
            }
            catch (Exception)
            {
                booResultado = false;
            }
            finally
            {
                obRequest.Factory.Dispose();
                obRequest.Parameters.Clear();
            }
            return booResultado;
        }
//fin gaa20151929
//gaa20160414
        public List<BEItemGenerico> ConsultaEquiposAlternativos()
        {
            DAABRequest.Parameter[] arrParam = 
				{                          
					new DAABRequest.Parameter("P_RESULT_SET", DbType.Object, ParameterDirection.Output) };

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_MATERIAL_LISTA + ".SSAPSS_MATERIAL_LISTA_T";
            obRequest.Parameters.AddRange(arrParam);

            List<BEItemGenerico> filas = new List<BEItemGenerico>();

            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        BEItemGenerico item = new BEItemGenerico();
                        item.Codigo = Funciones.CheckStr(dr["MATEC_CODMATERIAL"]);
                        item.Descripcion = Funciones.CheckStr(dr["MATEV_DESCMATERIAL"]);
                        filas.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return filas;
        }
//fin gaa20160414

        //PROY-29215 INICO
        public List<BEItemGenerico> ConsultaModoPagoyCuota(Int64 CodigoSEC)
        {
            DAABRequest.Parameter[] arrParam = 
				{      
                    new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int32, ParameterDirection.Input),
					new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output) };

            arrParam[0].Value = CodigoSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISASS_DATOSCUOTAS";
                        
            obRequest.Parameters.AddRange(arrParam);
            List<BEItemGenerico> filas = new List<BEItemGenerico>();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        BEItemGenerico item = new BEItemGenerico();
                        item.Descripcion = Funciones.CheckStr(dr["FORMA_PAGO"]);
                        item.Descripcion2 = Funciones.CheckStr(dr["NRO_CUOTA"]);
                        filas.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return filas;
        }

        public bool GrabarFormaPagoCuota( Int64 nroSEC, Int64 strCuota, string strFormaPago)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CUOTACOD", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FORMAPAGO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("K_RESULTADO", DbType.Int64, ParameterDirection.Output), 
                new DAABRequest.Parameter("K_DESC", DbType.String, ParameterDirection.Output) 
			};
            
            bool salida = false;
            int i = 0;
            i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = strCuota;
            i++; arrParam[i].Value = strFormaPago;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SISASU_APROBAR_CREDITOSXPLAN";
                    
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
               
                IDataParameter pSalida1, pSalida2;
                pSalida1 = (IDataParameter)objRequest.Parameters[3];
                pSalida2 = (IDataParameter)objRequest.Parameters[4];

                if (Funciones.CheckInt64(pSalida1.Value) == 0)
                    { 
                        salida = true;
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
            return salida;
        }

        public bool GrabarFormaPagoCuotaEmpresa(Int64 nroSEC, Int64 strCuota, string strFormaPago,string formaPagoActual, string cuotaActual, string Usuario)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CUOTACOD", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FORMAPAGO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("K_RESULTADO", DbType.Int64, ParameterDirection.Output), 
                new DAABRequest.Parameter("K_DESC", DbType.String, ParameterDirection.Output) 
			};

            bool salida = false;
            int i = 0;
            i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = strCuota;
            i++; arrParam[i].Value = strFormaPago;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SISASU_APROBAR_CREDITOSXPLAN";
            
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                IDataParameter pSalida1, pSalida2;
                pSalida1 = (IDataParameter)objRequest.Parameters[3];
                pSalida2 = (IDataParameter)objRequest.Parameters[4];

                if (Funciones.CheckInt64(pSalida1.Value) == 0)
                {
                    objLog.CrearArchivolog("[GrabarFormaPagoCuota] INICIO", null, null);
                    if (formaPagoActual != strFormaPago & cuotaActual == strCuota.ToString())
                    {
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Usuario), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | FORMA PAGO: ", strFormaPago), null, null);
                    }
                    else if (cuotaActual != strCuota.ToString() & formaPagoActual == strFormaPago)
                    {
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Usuario), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NRO. CUOTAS: ", strCuota.ToString()), null, null);
                    }
                    else if (formaPagoActual != strFormaPago & cuotaActual != strCuota.ToString())
                    {
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NOMBRE DEL ASESOR: ", Usuario), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | FORMA PAGO: ", strFormaPago), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarFormaPagoCuota] | NRO. CUOTAS: ", strCuota.ToString()), null, null);
                    }
                    
                    salida = true;
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
            return salida;
        }
//PROY-29215 FIN

        // INC000003135879 INICIO
        public bool GrabarAuditoriaCostoInstalacionCreditos(Int64 nroSEC, double costo_inst_anterior, double costo_inst_modifica, string strUsuario)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("p_solin_codigo", DbType.Int64, ParameterDirection.Input),				
				new DAABRequest.Parameter("p_costo_anterior", DbType.Double, ParameterDirection.Input),
                new DAABRequest.Parameter("p_costo_modifica", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_usuario", DbType.String, 10, ParameterDirection.Input),
                new DAABRequest.Parameter("p_codi_respuesta", DbType.String, ParameterDirection.Output),
                new DAABRequest.Parameter("p_desc_respuesta", DbType.String, ParameterDirection.Output)
			};
            bool salida = false;

            int i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = costo_inst_anterior;
            i++; arrParam[i].Value = costo_inst_modifica;
            i++; arrParam[i].Value = strUsuario;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_AUDITORIA_COSTO_INSTALACION";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                IDataParameter pSalida1, pSalida2;
                pSalida1 = (IDataParameter)objRequest.Parameters[4];
                pSalida2 = (IDataParameter)objRequest.Parameters[5];

                objLog.CrearArchivolog("[GrabarAuditoriaCostoInstalacion] INICIO", null, null);
                if (Funciones.CheckStr(pSalida1.Value) == "0")
                {
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarAuditoriaCostoInstalacion] | COSTO INSTALACION ANTERIOR: ", Funciones.CheckDecimal(costo_inst_anterior)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarAuditoriaCostoInstalacion] | COSTO INSTALACION MODIFICA: ", Funciones.CheckDecimal(costo_inst_modifica)), null, null);
                    salida = true;
                }
                objLog.CrearArchivolog(string.Format("{0}{1}", "[GrabarAuditoriaCostoInstalacion] | ESTADO: ", Funciones.CheckStr(pSalida2.Value)), null, null);
                objLog.CrearArchivolog("[GrabarAuditoriaCostoInstalacion] FIN ", null, null);
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
        // INC000003135879 FIN 

    }
}
