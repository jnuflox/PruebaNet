using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Configuration;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class DAGeneralII
    {
        //public BESolicitudPersona DetalleEvaluacionNatural(Int64 nroSEC)
        //{
        //    DAABRequest.Parameter[] arrParam = {
        //        new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
        //        new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
        //    };
        //    for (int i = 0; i < arrParam.Length; i++)
        //        arrParam[i].Value = DBNull.Value;

        //    arrParam[0].Value = nroSEC;

        //    BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
        //    DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
        //    objRequest.CommandType = CommandType.StoredProcedure;
        //    objRequest.Command = BaseDatos.PKG_SISACT_SOLICITUD_CONS + ".SECSS_DET_SOL_NAT";
        //    objRequest.Parameters.AddRange(arrParam);

        //    BESolicitudPersona item = new BESolicitudPersona();
        //    IDataReader dr = null;
        //    try
        //    {
        //        dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
        //        while (dr.Read())
        //        {
        //            item.SOLIC_EXI_BSC_FIN = Funciones.CheckStr(dr["SOLIC_EXI_BSC_FIN"]);
        //            item.CANAC_CODIGO = Funciones.CheckStr(dr["CANAC_CODIGO"]);
        //            item.OVENC_CODIGO = Funciones.CheckStr(dr["OVENC_CODIGO"]);
        //            item.SOLIC_EXI_BSC_FIN = Funciones.CheckStr(dr["SOLIC_EXI_BSC_FIN"]);
        //            item.OVENV_DESCRIPCION = Funciones.CheckStr(dr["OVENV_DESCRIPCION"]);
        //            item.TDOCC_CODIGO = Funciones.CheckStr(dr["TDOCC_CODIGO"]);
        //            item.TDOCV_DESCRIPCION = Funciones.CheckStr(dr["TDOCV_DESCRIPCION"]);
        //            item.CLIEC_NUM_DOC = Funciones.FormatoNroDocumento(Funciones.CheckStr(dr["TDOCC_CODIGO"]), Funciones.CheckStr(dr["CLIEC_NUM_DOC"]));
        //            item.CLIEV_NOMBRE = Funciones.CheckStr(dr["CLIEV_NOMBRE"]);
        //            item.CLIEV_APE_PAT = Funciones.CheckStr(dr["CLIEV_APE_PAT"]);
        //            item.CLIEV_APE_MAT = Funciones.CheckStr(dr["CLIEV_APE_MAT"]);
        //            item.TPROC_CODIGO = Funciones.CheckStr(dr["TPROC_CODIGO"]);
        //            item.CCLIV_DESCRIPCION = Funciones.CheckStr(dr["CCLIV_DESCRIPCION"]);
        //            item.SOLIV_DES_TIP_ACT = Funciones.CheckStr(dr["SOLIV_DES_TIP_ACT"]);
        //            item.TVENV_DESCRIPCION = Funciones.CheckStr(dr["TVENV_DESCRIPCION"]);
        //            item.PLANV_DESCRIPCION1 = Funciones.CheckStr(dr["PLANV_DESCRIPCION1"]);
        //            item.PLANV_DESCRIPCION2 = Funciones.CheckStr(dr["PLANV_DESCRIPCION2"]);
        //            item.PLANV_DESCRIPCION3 = Funciones.CheckStr(dr["PLANV_DESCRIPCION3"]);
        //            item.PACUV_DESCRIPCION = Funciones.CheckStr(dr["PACUV_DESCRIPCION"]);
        //            item.FPAGV_DESCRIPCION = Funciones.CheckStr(dr["FPAGV_DESCRIPCION"]);
        //            item.SOLIN_CAN_LIN = Funciones.CheckStr(dr["SOLIN_CAN_LIN"]);
        //            item.SOLIC_EVA_SUN = Funciones.CheckStr(dr["SOLIC_EVA_SUN"]);
        //            item.SOLIC_EVA_ESS = Funciones.CheckStr(dr["SOLIC_EVA_ESS"]);
        //            item.CCLIC_CODIGO = Funciones.CheckStr(dr["CCLIC_CODIGO"]);
        //            item.RDIRC_CODIGO = Funciones.CheckStr(dr["RDIRC_CODIGO"]);
        //            item.RDIRV_DESCRIPCION = Funciones.CheckStr(dr["RDIRV_DESCRIPCION"]);
        //            item.MRDIV_CAD_CODIGO = Funciones.CheckStr(dr["MRDIV_CAD_CODIGO"]);
        //            item.TACTC_CODIGO = Funciones.CheckStr(dr["TACTC_CODIGO"]);
        //            item.CLIEV_PRE_DIR = Funciones.CheckStr(dr["CLIEV_PRE_DIR"]);
        //            item.CLIEV_DIRECCION = Funciones.CheckStr(dr["CLIEV_DIRECCION"]);
        //            item.CLIEV_REF_DIR = Funciones.CheckStr(dr["CLIEV_REF_DIR"]);
        //            item.CLIEC_COD_DEP_DIR = Funciones.CheckStr(dr["CLIEC_COD_DEP_DIR"]);
        //            item.CLIEC_COD_PRO_DIR = Funciones.CheckStr(dr["CLIEC_COD_PRO_DIR"]);
        //            item.CLIEC_COD_DIS_DIR = Funciones.CheckStr(dr["CLIEC_COD_DIS_DIR"]);
        //            item.CLIEC_COD_POS_DIR = Funciones.CheckStr(dr["CLIEC_COD_POS_DIR"]);
        //            item.CLIEV_PRE_DIR_TRA = Funciones.CheckStr(dr["CLIEV_PRE_DIR_TRA"]);
        //            item.CLIEV_DIR_TRA = Funciones.CheckStr(dr["CLIEV_DIR_TRA"]);
        //            item.CLIEV_REF_DIR_TRA = Funciones.CheckStr(dr["CLIEV_REF_DIR_TRA"]);
        //            item.CLIEC_COD_DEP_TRA = Funciones.CheckStr(dr["CLIEC_COD_DEP_TRA"]);
        //            item.CLIEC_COD_PRO_TRA = Funciones.CheckStr(dr["CLIEC_COD_PRO_TRA"]);
        //            item.CLIEC_COD_DIS_TRA = Funciones.CheckStr(dr["CLIEC_COD_DIS_TRA"]);
        //            item.CLIEC_COD_POS_TRA = Funciones.CheckStr(dr["CLIEC_COD_POS_TRA"]);
        //            item.CLIEV_TEL_FIJ_TRA = Funciones.CheckStr(dr["CLIEV_TEL_FIJ_TRA"]);
        //            //Inicio TS-CJF - Agregacion de Datos Domicilio y Trabajo
        //            item.DEPAV_DESCRIPCION_LEGAL = Funciones.CheckStr(dr["DEPAV_DESCRIPCION_LEGAL"]);
        //            item.DEPAV_DESCRIPCION_TRABAJO = Funciones.CheckStr(dr["DEPAV_DESCRIPCION_TRABAJO"]);
        //            item.PROVV_DESCRIPCION_LEGAL = Funciones.CheckStr(dr["PROVV_DESCRIPCION_LEGAL"]);
        //            item.PROVV_DESCRIPCION_TRABAJO = Funciones.CheckStr(dr["PROVV_DESCRIPCION_TRABAJO"]);
        //            item.DISTV_DESCRIPCION_LEGAL = Funciones.CheckStr(dr["DISTV_DESCRIPCION_LEGAL"]);
        //            item.DISTV_DESCRIPCION_TRABAJO = Funciones.CheckStr(dr["DISTV_DESCRIPCION_TRABAJO"]);
        //            item.SOLIV_RUC_EMP_SUS = Funciones.CheckStr(dr["SOLIV_RUC_EMP_SUS"]);
        //            item.SOLIV_EMP_SUS = Funciones.CheckStr(dr["SOLIV_EMP_SUS"]);
        //            item.ESTAV_DESCRIPCION = Funciones.CheckStr(dr["ESTAV_DESCRIPCION"]);
        //            item.SOLIN_CODIGO = Funciones.CheckInt64(dr["SOLIN_CODIGO"]);
        //            item.SOLIC_SCO_TXT_CON = Funciones.CheckStr(dr["SOLIC_SCO_TXT_CON"]);
        //            item.SOLIN_LIM_CRE_CON = Funciones.CheckDbl(dr["SOLIN_LIM_CRE_CON"]);
        //            item.SOLIN_SCO_NUM_CON = Funciones.CheckDbl(dr["SOLIN_SCO_NUM_CON"]);
        //            //Fin TS-CJF - Agregacion de Datos Domicilio y Trabajo
        //            item.CLIEV_TEL_REF_1 = Funciones.CheckStr(dr["CLIEV_TEL_REF_1"]);
        //            item.CLIEV_TEL_REF_2 = Funciones.CheckStr(dr["CLIEV_TEL_REF_2"]);
        //            item.TVENC_CODIGO = Funciones.CheckStr(dr["TVENC_CODIGO"]);
        //            item.PLANC_CODIGO1 = Funciones.CheckStr(dr["PLANC_CODIGO1"]);
        //            item.PLANC_CODIGO2 = Funciones.CheckStr(dr["PLANC_CODIGO2"]);
        //            item.PLANC_CODIGO3 = Funciones.CheckStr(dr["PLANC_CODIGO3"]);
        //            item.PACUC_CODIGO = Funciones.CheckStr(dr["PACUC_CODIGO"]);
        //            item.FPAGC_CODIGO = Funciones.CheckStr(dr["FPAGC_CODIGO"]);
        //            item.PLANN_CAR_FIJ1 = Funciones.CheckDbl(dr["PLANN_CAR_FIJ1"]);
        //            item.PLANN_CAR_FIJ2 = Funciones.CheckDbl(dr["PLANN_CAR_FIJ2"]);
        //            item.PLANN_CAR_FIJ3 = Funciones.CheckDbl(dr["PLANN_CAR_FIJ3"]);
        //            item.SOLIC_TIP_CAR_MAN = Funciones.CheckStr(dr["SOLIC_TIP_CAR_MAN"]);
        //            item.SOLIC_TIP_CAR_FIJ = Funciones.CheckStr(dr["SOLIC_TIP_CAR_FIJ"]);
        //            item.SOLIN_IMP_DG = Funciones.CheckStr(dr["SOLIN_IMP_DG"]);
        //            item.SOLIN_IMP_DG_MAN = Funciones.CheckDbl(dr["SOLIN_IMP_DG_MAN"]);
        //            item.SOLIV_COM_PUN_VEN = Funciones.CheckStr(dr["SOLIV_COM_PUN_VEN"]);
        //            item.MRECV_DESCRIPCION = Funciones.CheckStr(dr["MRECV_DESCRIPCION"]);
        //            item.TCESC_DESCRIPCION = Funciones.CheckStr(dr["TCESC_DESCRIPCION"]);

        //            item.CLIEV_TEL_LEG = Funciones.CheckStr(dr["CLIEV_TEL_LEG"]);

        //            // T12618 - Portabilidad - INICIO
        //            item.OPERADOR_CEDENTE = Funciones.CheckStr(dr["OPERADOR_CEDENTE"]);
        //            item.ESTADO_PORTABILIDAD = Funciones.CheckStr(dr["ESTADO_PORTABILIDAD"]);
        //            item.SOLIV_RES_EXP_CON = Funciones.CheckStr(dr["SOLIV_RES_EXP_CON"]);
        //            switch (item.SOLIV_RES_EXP_CON)
        //            {
        //                case "A": item.SOLIV_RES_EXP_CON = "APROBAR"; break;
        //                case "B": item.SOLIV_RES_EXP_CON = "APROBAR__VERIFICAR"; break;
        //                case "C": item.SOLIV_RES_EXP_CON = "OBSERVAR"; break;
        //                case "D": item.SOLIV_RES_EXP_CON = "ALTO_RIESGO"; break;
        //                case "S": item.SOLIV_RES_EXP_CON = "SIN_HISTORIAL"; break;
        //            }
        //            // T12618 - Portabilidad - FIN

        //            item.FECHA_NACIMIENTO = Funciones.CheckDate(dr["CLIED_FEC_NAC"]);
        //            item.ESTADO_CIVIL_ID = Funciones.CheckStr(dr["CLIEV_EST_CIV"]);
        //            //DTH
        //            item.TPROD_COMERCIALIZAR = Funciones.CheckStr(dr["TPROD_COMERCIALIZAR"]);
        //            //FIN DTH
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        if (dr != null && dr.IsClosed == false) dr.Close();

        //        objRequest.Parameters.Clear();
        //        objRequest.Factory.Dispose();
        //    }
        //    return item;
        //}

        public ArrayList ListarPlanesSolicitudPersona(string p_sec)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = p_sec;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactVentasExpress + ".SP_LISTAR_PLANES_BY_SEC_2";
            objRequest.Parameters.AddRange(arrParam);

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEPlanDetalleConsumer objDetallePlan = new BEPlanDetalleConsumer();
                    objDetallePlan.PLANC_CODIGO = Funciones.CheckStr(dr["PLANC_CODIGO"]);
                    objDetallePlan.PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLANV_DESCRIPCION"]);
                    objDetallePlan.SOLIN_CODIGO = Funciones.CheckInt(dr["SOLIN_CODIGO"]);
                    objDetallePlan.SOPLC_MONTO_TOTAL = Funciones.CheckDbl(dr["SOPLC_MONTO_TOTAL"]);
                    objDetallePlan.SOPLN_CANTIDAD = Funciones.CheckInt(dr["SOPLN_CANTIDAD"]);
                    objDetallePlan.SOPLN_CODIGO = Funciones.CheckInt(dr["SOPLN_CODIGO"]);
                    objDetallePlan.SOPLN_MONTO_UNIT = Funciones.CheckDbl(dr["SOPLN_MONTO_UNIT"]);
                    objDetallePlan.TPROC_CODIGO = Funciones.CheckStr(dr["TPROC_CODIGO"]);
                    objDetallePlan.PAQTV_CODIGO = Funciones.CheckStr(dr["PAQTV_CODIGO"]);
                    objDetallePlan.SOPLN_SECUENCIA = Funciones.CheckInt(dr["SOPLN_SECUENCIA"]);

                    filas.Add(objDetallePlan);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public List<BEPlan> ListarPlanTarifario(string strOferta, string strProducto, string strDespacho, string strFlujo, string strDocumento,
                                                string strOficina, string strCasoEspecial, string strPlazo, string strRiesgo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_OFERTA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_PROD", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DESPACHO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLUJO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DOCUMENTO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OFICINA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_RIESGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strOferta;
            arrParam[1].Value = strProducto;
            arrParam[2].Value = strDespacho;
            arrParam[3].Value = strFlujo;
            arrParam[4].Value = strDocumento;
            arrParam[5].Value = strOficina;
            arrParam[6].Value = strCasoEspecial;
            arrParam[7].Value = strPlazo;
            arrParam[8].Value = strRiesgo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAN_TARIFARIO1";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEPlan> objLista = new List<BEPlan>();
            BEPlan objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEPlan();
                    objItem.PLANC_CODIGO = Funciones.CheckStr(dr["PLANC_CODIGO"]);
                    objItem.PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLANV_DESCRIPCION"]);
                    objItem.PLANN_CAR_FIJ = Funciones.CheckDbl(dr["PLANN_CAR_FIJ"]);
                    objItem.PLANC_EQUI_SAP = Funciones.CheckStr(dr["PLANC_EQUI_SAP"]);
                    objItem.PLNN_TIPO_PLAN = Funciones.CheckInt(dr["PLNV_TIPO_PLAN"]);
                    objItem.GPLNV_DESCRIPCION = Funciones.CheckStr(dr["GPLNV_DESCRIPCION"]);
                    objItem.CODIGO_BSCS = Funciones.CheckStr(dr["CODIGO_BSCS"]);
                    objItem.TIPO_PRODUCTOS = Funciones.CheckStr(dr["TIPO_PRODUCTOS"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public DataTable ListarEquipoGama()
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output)
			};
            arrParam[0].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL_II + ".MANTSS_LISTAR_EQUIPOGAMA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
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
        }

        public List<BEItemGenerico> ListarCampaniaCE(string strCasoEspecial)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strCasoEspecial;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_CAMPANA_CE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["CMPV_CODIGO"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEItemGenerico> ListarNoMostrarCampania(string strDocumento, string strRiesgo, string strPlan, string strPlazo, string strOficina)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_DOCUMENTO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_RIESGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAN", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OFICINA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strDocumento;
            arrParam[1].Value = strRiesgo;
            arrParam[2].Value = strPlan;
            arrParam[3].Value = strPlazo;
            arrParam[4].Value = strOficina;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_NO_MOSTRAR_CAMPANA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["CMPV_CODIGO"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEItemGenerico> ListarCampanaDTH(string strCodPlazo, string strCodPlan)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_PLAZO_CODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAN_CODIGO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strCodPlazo;
            arrParam[1].Value = strCodPlan;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_CAMPANA_DTH";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["CAMPV_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["CAMPV_DESCRIPCION"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEServicio> ListarServicioDTH(string strCodPlan)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_PLAN_TARIFARIO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strCodPlan;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SP_LISTAR_SERVICIOS_DTH";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEServicio> objLista = new List<BEServicio>();
            BEServicio objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEServicio();
                    objItem.SERVV_CODIGO = Funciones.CheckStr(dr["SERVICIO_SOLICIT"]);
                    objItem.SERVV_DESCRIPCION = Funciones.CheckStr(dr["DESCRIPCION"]);
                    objItem.CARGO_FIJO_BASE = Funciones.CheckDbl(dr["CARGO_FIJO"]);
                    objItem.GSRVC_CODIGO = Funciones.CheckStr(dr["GRUPO"]);
                    objItem.SERVN_ORDEN = Funciones.CheckInt(dr["ORDEN"]);
                    objItem.SELECCIONABLE_BASE = Funciones.CheckStr(dr["SELECCIONABLE"]);
                    objItem.SELECCIONABLE_EN_PLAN = Funciones.CheckStr(dr["SELECCIONABLE"]);
                    objItem.oPLAN.PLNV_CODIGO = strCodPlan;
                    objItem.TSERVC_CODIGO = Funciones.CheckStr(dr["TIPO_SERVICIO"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public ArrayList ListarKitsDTH(string p_tipo_operacion, string p_cod_campania, string p_plazo_acuerdo)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_TIPO_OPERACION", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CAMPV_CODIGO", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_PLZAC_CODIGO", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
											   };

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (p_tipo_operacion != "") { arrParam[i].Value = p_tipo_operacion; }
            ++i; if (p_cod_campania != "") { arrParam[i].Value = p_cod_campania; }
            ++i; if (p_plazo_acuerdo != "") { arrParam[i].Value = p_plazo_acuerdo; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SP_LISTAR_KITS_DTH";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BESecKit_AP oKit = new BESecKit_AP();
                    oKit.KITV_CODIGO = Funciones.CheckInt(dr["KITV_CODIGO"]);
                    oKit.KITV_DESCRIPCION = Funciones.CheckStr(dr["DESCRIPCION"]);
                    oKit.CARGO_FIJO_BASE = Funciones.CheckDbl(dr["CARGO_FIJO"]);
                    oKit.TKITC_CODIGO = Funciones.CheckStr(dr["TIPO_KIT"]);
                    oKit.KITN_PRECIO_BASE = Funciones.CheckDbl(dr["KITN_PRECIO_BASE"]);
                    oKit.SELECCIONABLE_EN_PLAN = "0";
                    oKit.CARGO_FIJO_EN_SEC = Funciones.CheckDbl(dr["CF_ALQUILER_KIT"]);
                    oKit.KITN_COSTO_INST = Funciones.CheckDbl(dr["KITN_COSTO_INST"]);
                    filas.Add(oKit);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public List<BEItemGenerico> ListarTopeAutomatico(string strPlanCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("P_COD_PLAN", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DESCRIPCION", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int32, ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[1].Value = strPlanCodigo;

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.TIM098_LISTA_PLAN_TC;
            objRequest.Parameters.AddRange(arrParam);

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["COD_PLAN"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["DESCRIPCION"]);
                    objItem.Estado = Funciones.CheckStr(dr["ESTADO"]);
                    objItem.Monto = Funciones.CheckDbl(dr["MONTO_TOPE"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEItemGenerico> ListarTipoProductoxOferta(string strOferta, string strFlujo, string strCasoEspecial, string strTipoDoc, string strOficina)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_OFERTA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLUJO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OFICINA", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strOferta;
            arrParam[1].Value = strFlujo;
            arrParam[2].Value = strCasoEspecial;
            arrParam[3].Value = strTipoDoc;
            arrParam[4].Value = strOficina;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_TIPO_PRODUCTO_X_ITEM";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["PRDV_DESCRIPCION"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BECasoEspecial> ListarCasoEspecial(string strOferta, string strDocumento, string strOficina)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_OFERTA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DOCUMENTO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OFICINA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strOferta;
            arrParam[1].Value = strDocumento;
            arrParam[2].Value = strOficina;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_CASO_ESPECIAL";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BECasoEspecial> objLista = new List<BECasoEspecial>();
            BECasoEspecial objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BECasoEspecial();
                    objItem.TCESC_CODIGO = Funciones.CheckStr(dr["TCESC_CODIGO"]);
                    objItem.TCESC_DESCRIPCION = Funciones.CheckStr(dr["TCESC_DESCRIPCION"]);
                    objItem.TCEN_MAX_PLANES = Funciones.CheckInt(dr["TCEN_MAX_PLANES"]);
                    objItem.TCEN_MAX_PLAN_VOZ = Funciones.CheckInt(dr["TCEN_MAX_PLAN_VOZ"]);
                    objItem.TCEN_MAX_PLAN_DATOS = Funciones.CheckInt(dr["TCEN_MAX_PLAN_DATOS"]);
                    objItem.FLAG_WHITELIST = Funciones.CheckStr(dr["TCESI_FLAG_WHITELIST"]);

                    objItem.TCESC_DESCRIPCION2 = objItem.TCESC_CODIGO + "_";
                    objItem.TCESC_DESCRIPCION2 += objItem.FLAG_WHITELIST + "_";
                    objItem.TCESC_DESCRIPCION2 += objItem.TCEN_MAX_PLANES + "_";
                    objItem.TCESC_DESCRIPCION2 += objItem.TCEN_MAX_PLAN_VOZ + "_";
                    objItem.TCESC_DESCRIPCION2 += objItem.TCEN_MAX_PLAN_DATOS;

                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEItemGenerico> ListarPlazoAcuerdo(string strTipoProducto, string strCasoEspecial)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_TIPO_PRODUCTO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strTipoProducto;
            arrParam[1].Value = strCasoEspecial;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".sp_con_plazo_acuerdo_prd";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["PLZAC_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["PLZAV_DESCRIPCION"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }
//gaa20140414
        public List<BEItemGenerico> ListarPlazoAcuerdoHFC(string pstrCampana)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_CAMPANA", DbType.String, 4, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = pstrCampana;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAZO_ACUERDO_3PLAY";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico oPlazo;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    oPlazo = new BEItemGenerico();
                    oPlazo.Codigo = Funciones.CheckStr(dr["PLZAC_CODIGO"]);
                    oPlazo.Descripcion = Funciones.CheckStr(dr["PLZAV_DESCRIPCION"]);
                    objLista.Add(oPlazo);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }
//fin gaa20140414
        public List<BEItemGenerico> ListarPaqueteUni(string strDocumento, string strOferta, string strPlazo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_DOCUMENTO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OFERTA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strDocumento;
            arrParam[1].Value = strOferta;
            arrParam[2].Value = strPlazo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PAQUETE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["PAQTV_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["PAQTV_DESCRIPCION"]);
                    objItem.Tipo = Funciones.CheckStr(dr["TPAQTV_CODIGO"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public ArrayList ListarPlanIndiRestServ(string pstrPlan, string pstrCasoEspecial)
        {
            ArrayList filas = null;
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_PLAN", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output)                                                                        
											   };

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = pstrPlan;
            arrParam[1].Value = pstrCasoEspecial;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_REGLAS_PLAN_SERVICIO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BESecServicio_AP oServ = new BESecServicio_AP();
                    oServ.SERVV_CODIGO = Funciones.CheckStr(dr["SERVICIO"]);
                    oServ.SELECCIONABLE_BASE = Funciones.CheckStr(dr["ESTADO"]);
                    filas.Add(oServ);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public ArrayList ListarServiciosXPaqPlan(string codPaquete, string codPlan, int idSecuencia)
        {
            ArrayList filas = null;
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_PAQTV_COD", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLNV_CODIGO", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PAQPN_SECUENCIA", DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
			};

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (codPaquete != String.Empty) { arrParam[i].Value = codPaquete; }
            i++; if (codPlan != String.Empty) { arrParam[i].Value = codPlan; }
            i++; if (idSecuencia != 0) { arrParam[i].Value = idSecuencia; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            string[] sTab = { "Planes", "Servicios" };
            objRequest.TableNames = sTab;
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_CON_SERV_BY_PAQ_PLAN";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BESecServicio_AP oServ = new BESecServicio_AP();
                    oServ.PLAN = new BESecPlan_AP();
                    oServ.PLAN.PLNV_CODIGO = Funciones.CheckStr(dr["PLNV_CODIGO"]);
                    oServ.PLAN.PAQPN_SECUENCIA = Funciones.CheckInt(dr["PAQPN_SECUENCIA"]);
                    oServ.SERVV_CODIGO = Funciones.CheckStr(dr["SERVV_CODIGO"]);
                    oServ.SERVV_DESCRIPCION = Funciones.CheckStr(dr["SERVV_DESCRIPCION"]);
                    oServ.SERVC_ESTADO = Funciones.CheckStr(dr["SERVC_ESTADO"]);
                    oServ.GSRVC_CODIGO = Funciones.CheckStr(dr["GSRVC_CODIGO"]);
                    oServ.SERVN_ORDEN = Funciones.CheckInt(dr["SERVN_ORDEN"]);
                    oServ.SELECCIONABLE_BASE = Funciones.CheckStr(dr["SELECCIONABLE_BASE"]);
                    oServ.CARGO_FIJO_BASE = Funciones.CheckInt(dr["CARGO_FIJO_BASE"]);
                    oServ.SELECCIONABLE_EN_PAQUETE = Funciones.CheckStr(dr["SELECCIONABLE_EN_PAQUETE"]);
                    oServ.CARGO_FIJO_EN_PAQUETE = Funciones.CheckInt(dr["CARGO_FIJO_EN_PAQUETE"]);
                    oServ.SERVD_FECHA_CREA = Funciones.CheckDate(dr["SERVD_FECHA_CREA"]);
                    oServ.SERVV_USUARIO_CREA = Funciones.CheckStr(dr["SERVV_USUARIO_CREA"]);
                    filas.Add(oServ);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public ArrayList ConsultarListaServicios(string p_plan_tarifario, string p_tipo_cliente, string p_mandt)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_PLAN_TARIFARIO", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TIPO_CLIENTE", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_MANDT", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
											   };

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (p_plan_tarifario != "") { arrParam[i].Value = p_plan_tarifario; }
            ++i; if (p_tipo_cliente != "") { arrParam[i].Value = p_tipo_cliente; }
            ++i; if (p_mandt != "") { arrParam[i].Value = p_mandt; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactVentasExpress + ".SP_LISTAR_SERVICIOS_2";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BESecServicio_AP oServ = new BESecServicio_AP();
                    oServ.SERVV_CODIGO = Funciones.CheckStr(dr["SERVICIO_SOLICIT"]);
                    oServ.SERVV_DESCRIPCION = Funciones.CheckStr(dr["DESCRIPCION"]);
                    oServ.CARGO_FIJO_BASE = Funciones.CheckFloat(dr["CARGO_FIJO"]);
                    oServ.DESCUENTO_EN_PLAN = 0;
                    oServ.GSRVC_CODIGO = Funciones.CheckStr(dr["GRUPO"]);
                    oServ.SERVN_ORDEN = Funciones.CheckInt(dr["ORDEN"]);
                    oServ.SELECCIONABLE_BASE = Funciones.CheckStr(dr["SELECCIONABLE"]);
                    oServ.SELECCIONABLE_EN_PLAN = Funciones.CheckStr(dr["SELECCIONABLE"]);
                    oServ.PLAN.PLNV_CODIGO = p_plan_tarifario; //Funciones.CheckStr(dr["Plan_Tarifario"]);

                    filas.Add(oServ);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public ArrayList ListarPlanesXPaquete(string codPaquete)
        {
            ArrayList filas = null;
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_PAQTV_COD", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("K_CUR_PLANES", DbType.Object,ParameterDirection.Output),
												   new DAABRequest.Parameter("K_CUR_SERVICIOS", DbType.Object,ParameterDirection.Output)
											   };

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (codPaquete != String.Empty) { arrParam[i].Value = codPaquete; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            string[] sTab = { "Planes", "Servicios" };
            objRequest.TableNames = sTab;
            objRequest.CommandType = CommandType.StoredProcedure;
            //objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_CON_DET_BY_PAQUETE";
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_CON_DET_BY_PAQUETE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            filas = new ArrayList();
            DataSet ds = null;
            try
            {
                ds = objRequest.Factory.ExecuteDataset(ref objRequest);
                DataTable dt1 = ds.Tables["Planes"];
                DataTable dt2 = ds.Tables["Servicios"];

                int idGen = Funciones.CheckInt(DateTime.Now.ToString("hhmmss")); //Autogenerado

                for (int k = 0; k < dt1.Rows.Count; k++)
                {
                    DataRow dr = dt1.Rows[k];
                    BESecPlan_AP oPlan = new BESecPlan_AP();
                    oPlan.PAQPN_SECUENCIA = Funciones.CheckInt(dr["PAQPN_SECUENCIA"]);
                    oPlan.PLNV_CODIGO = Funciones.CheckStr(dr["PLNV_CODIGO"]);
                    oPlan.PLNV_DESCRIPCION = Funciones.CheckStr(dr["PLNV_DESCRIPCION"]);
                    oPlan.PLNC_ESTADO = Funciones.CheckStr(dr["PLNC_ESTADO"]);
                    oPlan.TVENC_CODIGO = Funciones.CheckStr(dr["TVENC_CODIGO"]);
                    oPlan.TPROC_CODIGO = Funciones.CheckStr(dr["TPROC_CODIGO"]);
                    oPlan.PLNN_TIPO_PLAN = Funciones.CheckInt(dr["PLNN_TIPO_PLAN"]);
                    oPlan.CARGO_FIJO_BASE = Funciones.CheckInt(dr["PLNN_CARGO_FIJO"]);
                    oPlan.CARGO_FIJO_EN_PAQUETE = Funciones.CheckInt(dr["PAQPN_CARGO_FIJO"]);
                    oPlan.PLND_FECHA_CREA = Funciones.CheckDate(dr["PAQPD_FECHA_CREA"]);
                    oPlan.PLNV_USUARIO_CREA = Funciones.CheckStr(dr["PAQPV_USUARIO_CREA"]);
                    oPlan.PAQUETE.PAQTV_CODIGO = Funciones.CheckStr(dr["PAQTV_CODIGO"]);
                    oPlan.PAQUETE.PAQTV_DESCRIPCION = Funciones.CheckStr(dr["PAQTV_DESCRIPCION"]);
                    oPlan.PAQUETE.PAQTC_ESTADO = Funciones.CheckStr(dr["PAQTC_ESTADO"]);
                    oPlan.PAQUETE.TPAQTV_CODIGO = Funciones.CheckStr(dr["TPAQTV_CODIGO"]);
                    oPlan.PAQUETE.TPROC_CODIGO = Funciones.CheckStr(dr["TPROC_CODIGO"]);
                    oPlan.GPLNV_DESCRIPCION = Funciones.CheckStr(dr["GPLNV_DESCRIPCION"]);
                    oPlan.PLANC_EQUI_SAP = Funciones.CheckStr(dr["PLANC_EQUI_SAP"]);

                    oPlan.SOPLN_CODIGO = idGen;

                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        DataRow dr2 = dt2.Rows[j];
                        if (Funciones.CheckInt(dr["PAQPN_SECUENCIA"]) == Funciones.CheckInt(dr2["PAQPN_SECUENCIA"]) &&
                            Funciones.CheckInt(dr["PLNV_CODIGO"]) == Funciones.CheckInt(dr2["PLNV_CODIGO"]))
                        {
                            BESecServicio_AP oServ = new BESecServicio_AP();
                            oServ.PLAN = oPlan;
                            oServ.SERVV_CODIGO = Funciones.CheckStr(dr2["SERVV_CODIGO"]);
                            oServ.SERVV_DESCRIPCION = Funciones.CheckStr(dr2["SERVV_DESCRIPCION"]);
                            oServ.SERVC_ESTADO = Funciones.CheckStr(dr2["SERVC_ESTADO"]);
                            oServ.GSRVC_CODIGO = Funciones.CheckStr(dr2["GSRVC_CODIGO"]);
                            oServ.SERVN_ORDEN = Funciones.CheckInt(dr2["SERVN_ORDEN"]);
                            oServ.SELECCIONABLE_BASE = Funciones.CheckStr(dr2["SELECCIONABLE_BASE"]);
                            oServ.CARGO_FIJO_BASE = Funciones.CheckInt(dr2["CARGO_FIJO_BASE"]);
                            oServ.SELECCIONABLE_EN_PAQUETE = Funciones.CheckStr(dr2["SELECCIONABLE_EN_PAQUETE"]);
                            oServ.CARGO_FIJO_EN_PAQUETE = Funciones.CheckInt(dr2["CARGO_FIJO_EN_PAQUETE"]);
                            oServ.SERVD_FECHA_CREA = Funciones.CheckDate(dr2["SERVD_FECHA_CREA"]);
                            oServ.SERVV_USUARIO_CREA = Funciones.CheckStr(dr2["SERVV_USUARIO_CREA"]);

                            oPlan.SERVICIOS.Add(oServ);
                        }
                    }

                    filas.Add(oPlan);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //if (ds != null && ds.IsClosed==false ) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public List<BEItemGenerico> ListarCuota(string strDocumento, string strRiesgo, string strPlan, string strPlazo, int intNroPlanes, string strCasoEspecial)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_DOCUMENTO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_RIESGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAN", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_PLANES", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strDocumento;
            arrParam[1].Value = strRiesgo;
            arrParam[2].Value = strPlan;
            arrParam[3].Value = strPlazo;
            arrParam[4].Value = intNroPlanes;
            arrParam[5].Value = strCasoEspecial;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_CUOTAS";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["CUOC_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["CUOV_DESCRIPCION"]);
                    objItem.Codigo2 = objItem.Codigo + "_" + Funciones.CheckStr(dr["CUON_INICIAL"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEItemGenerico> ListarTipoProducto(string strOferta, string strFlujo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_OFERTA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLUJO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strOferta;
            arrParam[1].Value = strFlujo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_TIPO_PRODUCTO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["PRDV_DESCRIPCION"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public ArrayList ListarPromocionesXPaquete3Play(string pstrPaquete)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("an_idpaq", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("cur_prom_srv", DbType.Object, ParameterDirection.Output),
												   new DAABRequest.Parameter("an_coderror_o", DbType.Int32, ParameterDirection.Output),
												   new DAABRequest.Parameter("ac_mensaje_o", DbType.String, ParameterDirection.Output)
											   };
            ArrayList filas;
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = pstrPaquete;

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_INT_SISACT_CONSULTA + ".p_consulta_prom_paq";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BESecPlan_AP oPlan = new BESecPlan_AP();
                    oPlan.PLNV_CODIGO = Funciones.CheckStr(dr["IDDET"]);
                    oPlan.CAMP_CODIGO = Funciones.CheckStr(dr["IDPRODUCTO"]);
                    oPlan.CAMP_DESCRIPCION = Funciones.CheckStr(dr["IDLINEA"]);
                    oPlan.PLNC_ESTADO = Funciones.CheckStr(dr["IDPROM"]);
                    oPlan.PLNN_TIPO_PLAN = Funciones.CheckInt(dr["FLGEDI"]);
                    oPlan.PAQPN_SECUENCIA = Funciones.CheckInt(dr["PAQUETE"]);
                    oPlan.PLNV_DESCRIPCION = Funciones.CheckStr(dr["DSCPROM"]);
                    filas.Add(oPlan);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public DataTable ListarSolucion3Play(string pstrTipoServicio, out int pintCodError, out string pstrMsjError)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("an_tipsrv", DbType.String, 4, ParameterDirection.Input),
												   new DAABRequest.Parameter("cur_solucion_o", DbType.Object, ParameterDirection.Output),
												   new DAABRequest.Parameter("an_coderror_o", DbType.Int32, ParameterDirection.Output),
												   new DAABRequest.Parameter("ac_mensaje_o", DbType.String, ParameterDirection.Output)
											   };

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = pstrTipoServicio;

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_INT_SISACT_CONSULTA + ".p_consulta_solucion";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            DataTable dt;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];

                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[2];
                pintCodError = Convert.ToInt32(parSalida1.Value);

                IDataParameter parSalida2;
                parSalida2 = (IDataParameter)objRequest.Parameters[3];
                pstrMsjError = Convert.ToString(parSalida2.Value);
            }
            catch (Exception e)
            {
                dt = null;
                pintCodError = 1;
                pstrMsjError = e.Message;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return dt;
        }

        public DataTable ListarPaquete3Play(Int64 plngIdSolucion, out int pintCodError, out string pstrMsjError)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("an_idsolucion", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("cur_paquete_o", DbType.Object, ParameterDirection.Output),
												   new DAABRequest.Parameter("an_coderror_o", DbType.Int32, ParameterDirection.Output),
												   new DAABRequest.Parameter("ac_mensaje_o", DbType.String, ParameterDirection.Output)
											   };

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = plngIdSolucion;

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_INT_SISACT_CONSULTA + ".p_consulta_paquete";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            DataTable dt;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];

                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[2];
                pintCodError = Convert.ToInt32(parSalida1.Value);

                IDataParameter parSalida2;
                parSalida2 = (IDataParameter)objRequest.Parameters[3];
                pstrMsjError = Convert.ToString(parSalida2.Value);
            }
            catch (Exception e)
            {
                dt = null;
                pintCodError = 1;
                pstrMsjError = e.Message;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return dt;
        }

        public List<BEServicioHFC> ListarPlanesXPaquete3Play(string strPaquete)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("an_idpaq", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("cur_srv_o", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("an_coderror_o", DbType.Int32, ParameterDirection.Output),
				new DAABRequest.Parameter("ac_mensaje_o", DbType.String, ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strPaquete;

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PQ_INT_SISACT_CONSULTA + ".p_consulta_srv";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEServicioHFC> objLista = new List<BEServicioHFC>();
            BEServicioHFC objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEServicioHFC();
                    objItem.IDDET = Funciones.CheckInt64(dr["IDDET"]);
                    objItem.IdProducto = Funciones.CheckInt64(dr["IDPRODUCTO"]);
                    objItem.IdLinea = Funciones.CheckInt64(dr["IDLINEA"]);
                    objItem.Producto = Funciones.CheckStr(dr["PRODUCTO"]);
                    objItem.Grupo = Funciones.CheckInt(dr["PAQUETE"]);
                    objItem.IdServicio = Funciones.CheckStr(dr["CODSRV"]);
                    objItem.Servicio = Funciones.CheckStr(dr["SERVICIO"]);
                    objItem.IdEquipo = Funciones.CheckStr(dr["CODEQUIPO"]);
                    objItem.Equipo = Funciones.CheckStr(dr["EQUIPO"]);
                    objItem.CF_Precio = Funciones.CheckDbl(dr["PRECIO"]);
                    objItem.FlagPrincipal = Funciones.CheckStr(dr["FLGPRINCIPAL"]);
                    objItem.FlagOpcional = Funciones.CheckStr(dr["FLG_OPCIONAL"]);
                    objItem.FlagDefecto = Funciones.CheckStr(dr["DEFECTO"]);
                    objItem.CantVenta = Funciones.CheckInt(dr["CANTIDAD"]);
                    objItem.FlagVOD = "0";

                    if (Funciones.CheckStr(dr["DESCRIP_CODEXT"]) == ConfigurationManager.AppSettings["codigoVOD"])
                        objItem.FlagVOD = "1";

                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEItemGenerico> ListarCampanaDTH1(string strOficina)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("P_OFICINA", DbType.String, 4, ParameterDirection.Input)
			};
            arrParam[0].Value = DBNull.Value;
            arrParam[1].Value = strOficina;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".MANTSS_LISTAR_CAMPANA_DTH1";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["CAMPV_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["CAMPV_DESCRIPCION"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEPlan> ListarPlanDTH(string pstrOferta, string pstrCampana)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("P_OFERTA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPANA", DbType.String, 4, ParameterDirection.Input)
			};
            arrParam[0].Value = DBNull.Value;
            arrParam[1].Value = pstrOferta;
            arrParam[2].Value = pstrCampana;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".MANTSS_LISTAR_PLAN_DTH";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEPlan> objLista = new List<BEPlan>();
            BEPlan objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEPlan();
                    objItem.PLANC_CODIGO = Funciones.CheckStr(dr["PLANC_CODIGO"]);
                    objItem.PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLANV_DESCRIPCION"]);
                    objItem.PLANN_CAR_FIJ = Funciones.CheckDbl(dr["PLANN_CAR_FIJ"]);
                    objItem.PLANC_EQUI_SAP = Funciones.CheckStr(dr["PLANC_EQUI_SAP"]);
                    objItem.PLNN_TIPO_PLAN = Funciones.CheckInt(dr["PLNV_TIPO_PLAN"]);
                    objItem.GPLNV_DESCRIPCION = Funciones.CheckStr(dr["GPLNV_DESCRIPCION"]);
                    objItem.CODIGO_BSCS = Funciones.CheckStr(dr["CODIGO_BSCS"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEItemGenerico> ListarPlazoDTH(string strCodPlan)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_PLAN_CODIGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(strCodPlan)) { arrParam[0].Value = strCodPlan; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SP_CON_PLAZO_DTH";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["PLZAC_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["PLZAV_DESCRIPCION"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public string ConsultarProductoPaquete(string pPaquete)
        {
            string strProductos;
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_PLAN", DbType.String, 10, ParameterDirection.Input),
												new DAABRequest.Parameter("V_PRODUCTOS", DbType.String, 100, ParameterDirection.Output)};
            arrParam[0].Value = pPaquete;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;

            objRequest.Command = "SP_OBTENER_PRODUCTOS_PLAN";
            string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
            if (esquema != null && esquema != "")
                objRequest.Command = esquema + "." + objRequest.Command;

            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter p;
                p = (IDataParameter)objRequest.Parameters[1];
                strProductos = Funciones.CheckStr(p.Value);
            }
            catch (Exception)
            {
                strProductos = "";
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return strProductos;
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
            string nroDocumento = Funciones.Right("0000000000000000" + strNroDocumento, 16);

            i = 2; arrParam[i].Value = strTipoDocumento;
            i++; arrParam[i].Value = nroDocumento;
            i++; arrParam[i].Value = strOferta;
            i++; arrParam[i].Value = strCasoEspecial;
            i++; arrParam[i].Value = strCadenaDetalle;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strNroDocumento);
            objRequest.Transactional = true;
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_VALIDA_SEC_RECURRENTE";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                SEC = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[0]).Value);
                flgReingresoSec = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[1]).Value);
            }
            catch (Exception)
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

        public List<BEPlanDetalleHFC> DetalleOferta3Play(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
                                                new DAABRequest.Parameter("P_CURSOR", DbType.Object ,ParameterDirection.Output)};

            arrParam[0].Value = nroSEC;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CONS_DETALLE_HFC";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEPlanDetalleHFC> filas = new List<BEPlanDetalleHFC>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEPlanDetalleHFC item = new BEPlanDetalleHFC();
                    item.IdSolucion = Funciones.CheckInt64(dr["IDSOLUCION"]);
                    item.Solucion = Funciones.CheckStr(dr["SOLUCION"]);
                    item.IdPaquete = Funciones.CheckInt64(dr["IDPAQ"]);
                    item.Paquete = Funciones.CheckStr(dr["PAQUETE"]);
                    item.Grupo = Funciones.CheckInt(dr["GRUPO"]);
                    item.Tipo = Funciones.CheckStr(dr["TIPO"]);
                    item.IdProducto = Funciones.CheckInt64(dr["IDPRODUCTO"]);
                    item.Producto = Funciones.CheckStr(dr["PRODUCTO"]);
                    item.IdServicio = Funciones.CheckStr(dr["IDSERVICIO"]);
                    item.Servicio = Funciones.CheckStr(dr["SERVICIO"]);
                    item.IdPromocion = Funciones.CheckInt64(dr["IDPROM"]);
                    item.Promocion = Funciones.CheckStr(dr["PROMOCION"]);
                    item.FlagPrincipal = Funciones.CheckStr(dr["FLG_PRINCIPAL"]);
                    item.Precio = Funciones.CheckDbl(dr["CF_PRECIO"]);
                    item.GrupoDescripcion = Funciones.CheckStr(dr["GRUPODESCRIPCION"]);
                    item.Campana = Funciones.CheckStr(dr["CAMPV_DESCRIPCION"]);
                    item.Plan = Funciones.CheckStr(dr["PLNV_DESCRIPCION"]);

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
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public ArrayList ListarPlanDTH(string codPlan, string codTipoProd, string codTipoVenta, string codPlazo, string tipoPlan, string codCampana)
        {
            ArrayList filas = null;
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_PLNV_CODIGO", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TPROC_CODIGO", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TVENC_CODIGO", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_PACUC_CODIGO", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_PLNC_TIPO_PRO", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CAMPV_CODIGO", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)                                                                        
											   };

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (codPlan != String.Empty) { arrParam[i].Value = codPlan; }
            i++; if (codTipoProd != String.Empty) { arrParam[i].Value = codTipoProd; }
            i++; if (codTipoVenta != String.Empty) { arrParam[i].Value = codTipoVenta; }
            i++; if (codPlazo != String.Empty) { arrParam[i].Value = codPlazo; }
            i++; if (tipoPlan != String.Empty) { arrParam[i].Value = tipoPlan; }
            i++; if (codCampana != String.Empty) { arrParam[i].Value = codCampana; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SISACT_CON_PLAN_DTH";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BESecPlan_AP oPlan = new BESecPlan_AP();
                    oPlan.PLNV_CODIGO = Funciones.CheckStr(dr["PLNV_CODIGO"]);
                    oPlan.PLNV_DESCRIPCION = Funciones.CheckStr(dr["PLNV_DESCRIPCION"]);
                    oPlan.PLNC_ESTADO = Funciones.CheckStr(dr["PLNC_ESTADO"]);
                    oPlan.TVENC_CODIGO = Funciones.CheckStr(dr["TVENC_CODIGO"]);
                    oPlan.TPROC_CODIGO = Funciones.CheckStr(dr["TPROC_CODIGO"]);
                    oPlan.PLNN_TIPO_PLAN = Funciones.CheckInt(dr["PLNN_TIPO_PLAN"]);
                    oPlan.CARGO_FIJO_BASE = Funciones.CheckDbl(dr["PLNN_CARGO_FIJO"]);
                    oPlan.PLND_FECHA_CREA = Funciones.CheckDate(dr["PLND_FECHA_CREA"]);
                    oPlan.PLNV_USUARIO_CREA = Funciones.CheckStr(dr["PLNV_USUARIO_CREA"]);
                    //gaa20120131
                    oPlan.PACUC_DESCRIPCION = Funciones.CheckStr(dr["PLZAV_DESCRIPCION"]);
                    //fin gaa20120131
                    //gaa20120202
                    oPlan.CAMP_CODIGO = Funciones.CheckStr(dr["CAMPV_CODIGO"]);
                    oPlan.PLZO_CODIGO = Funciones.CheckStr(dr["PLZAC_CODIGO"]);
                    //fin gaa20120202
                    filas.Add(oPlan);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public ArrayList ConsultarListaServiciosDTH(string p_plan_tarifario)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_PLAN_TARIFARIO", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
											   };

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (p_plan_tarifario != "") { arrParam[i].Value = p_plan_tarifario; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SP_LISTAR_SERVICIOS_DTH";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BESecServicio_AP oServ = new BESecServicio_AP();
                    oServ.SERVV_CODIGO = Funciones.CheckStr(dr["SERVICIO_SOLICIT"]);
                    oServ.SERVV_DESCRIPCION = Funciones.CheckStr(dr["DESCRIPCION"]);
                    oServ.CARGO_FIJO_BASE = Funciones.CheckDbl(dr["CARGO_FIJO"]);
                    oServ.GSRVC_CODIGO = Funciones.CheckStr(dr["GRUPO"]);
                    oServ.SERVN_ORDEN = Funciones.CheckInt(dr["ORDEN"]);
                    oServ.SELECCIONABLE_BASE = Funciones.CheckStr(dr["SELECCIONABLE"]);
                    oServ.SELECCIONABLE_EN_PLAN = Funciones.CheckStr(dr["SELECCIONABLE"]);
                    oServ.PLAN.PLNV_CODIGO = p_plan_tarifario; //Funciones.CheckStr(dr["Plan_Tarifario"]);
                    oServ.TSERVC_CODIGO = Funciones.CheckStr(dr["TIPO_SERVICIO"]);
                    filas.Add(oServ);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public double ObtenerPrecioKit(string strCodCampana, string strCodPlaza, int intcodKit)
        {
            Double dblPrecio = 0;
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CAMP_CODIGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLZA_CODIGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_KIT_CODIGO", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PRECIO", DbType.Double, ParameterDirection.Output)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = strCodCampana;
            i++; arrParam[i].Value = strCodPlaza;
            i++; arrParam[i].Value = intcodKit;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".MANTSS_TRAER_PRECIO_LISTA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[3];
                dblPrecio = Convert.ToDouble(parSalida1.Value);
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
            return dblPrecio;
        }

        public double ObtenerCFAlquilerKit(int intcodKit, int intCampania, string strPlazo)
        {
            Double dblCosto = 0;
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_KIT_CODIGO", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPV_CODIGO", DbType.Int32, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLZAC_CODIGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CF_ALQUILER_KIT", DbType.Double, ParameterDirection.Output)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = intcodKit;
            i++; arrParam[i].Value = intCampania;
            i++; arrParam[i].Value = strPlazo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SP_CON_CFALQUILERKIT";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[3];
                if (parSalida1.Value != System.DBNull.Value)
                    dblCosto = Convert.ToDouble(parSalida1.Value);
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
            return dblCosto;
        }

    }
}