using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class DAEvaluacion
    {
        GeneradorLog objLog = new GeneradorLog("    DAEvaluacion    ", null, null, "DATA_LOG");
        public List<BEBilletera> ObtenerLCxBilletera(string strRiesgo, string strTipoDoc, string strNroDocumento, string essaludSunat, string strClienteNuevo, double dblLC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RIESGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESSALUD_SUNAT", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIENTE_NUEVO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_LC_DC", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("C_PRODUCTO_LC", DbType.Object,ParameterDirection.Output)                                                                        
			};
            int i = 0; arrParam[i].Value = strRiesgo;
            ++i; arrParam[i].Value = strTipoDoc;
            ++i; arrParam[i].Value = essaludSunat;
            ++i; arrParam[i].Value = strClienteNuevo;
            ++i; arrParam[i].Value = dblLC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strNroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CALCULO_LC_X_PRODUCTO";
            objRequest.Parameters.AddRange(arrParam);

            List<BEBilletera> objLista = new List<BEBilletera>();
            BEBilletera objItem;
            DataTable dt = null;
            try
            {  
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        objItem = new BEBilletera();
                        objItem.idBilletera = Funciones.CheckInt(dr["PRODUCTO_COD"]);
                        objItem.billetera = Funciones.CheckStr(dr["DESCRIPCION"]);
                        objItem.monto = Funciones.CheckDbl(dr["PRODUCTO_LC"]);
                        objLista.Add(objItem);
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
            return objLista;
        }

        public List<BEBilletera> ObtenerMontoFactxBilletera(string strNroDocumento, string strCadena)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_LISTA_PLANES", DbType.String, 4000, ParameterDirection.Input),
				new DAABRequest.Parameter("C_PRODUCTO_FACT", DbType.Object, ParameterDirection.Output)                                                                        
			};
            arrParam[0].Value = strCadena;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strNroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CALCULO_FACTURA_X_PRODUCTO";
            objRequest.Parameters.AddRange(arrParam);

            List<BEBilletera> objLista = new List<BEBilletera>();
            BEBilletera objItem;
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        objItem = new BEBilletera();
                        objItem.idBilletera = Funciones.CheckInt(dr["CODIGO"]);
                        objItem.billetera = Funciones.CheckStr(dr["DESCRIPCION"]);
                        objItem.monto = Funciones.CheckDbl(dr["VALOR"]);
                        objLista.Add(objItem);
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
            return objLista;
        }

        //PROY-24740
        public List<BEPlanBilletera> ObtenerBilleteraxPlan(string strListaPlan)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_PLANES", DbType.String, 4000, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
			};
            arrParam[0].Value = strListaPlan;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_PLAN_X_PRODUCTO";
            objRequest.Parameters.AddRange(arrParam);

            List<BEPlanBilletera> objLista = new List<BEPlanBilletera>();
            List<String> objListaPlan = new List<String>();
            BEPlanBilletera objItem;
            BEBilletera objBilletera;
            string plan;

            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    plan = Funciones.CheckStr(dr["PLAN"]);
                    objBilletera = new BEBilletera(Funciones.CheckInt(dr["PRODUCTO"]), Funciones.CheckStr(dr["DESCRIPCION"]));

                    if (!objListaPlan.Contains(plan))
                    {
                        objItem = new BEPlanBilletera();
                        objItem.plan = plan;
                        objItem.oBilletera = new List<BEBilletera>();
                        objItem.oBilletera.Add(objBilletera);

                        objLista.Add(objItem);
                        objListaPlan.Add(plan);
                    }
                    else
                    {
                        foreach (BEPlanBilletera objPlan in objLista)
                        {
                            if (plan == objPlan.plan)
                            {
                                objPlan.oBilletera.Add(objBilletera);
                                break;
                            }
                        }
                    }
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
        public List<BEPlanBilletera> ObtenerPlanesxBilletera(int flgSistema)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SISTEMA", DbType.Int16, ParameterDirection.Input)
			};
            arrParam[1].Value = flgSistema;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CON_PLAN_BILLETERA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEPlanBilletera> objLista = new List<BEPlanBilletera>();
            List<String> objListaPlan = new List<String>();
            BEPlanBilletera objItem;
            BEBilletera objBilletera;
            string plan;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    plan = Funciones.CheckStr(dr["SOLV_CODIGO"]);
                    objBilletera = new BEBilletera(Funciones.CheckInt(dr["PRCLV_CODIGO"]), 1);

                    if (!objListaPlan.Contains(plan))
                    {
                        objItem = new BEPlanBilletera();
                        objItem.plan = plan;
                        objItem.oBilletera = new List<BEBilletera>();
                        objItem.oBilletera.Add(objBilletera);
                        objLista.Add(objItem);
                        objListaPlan.Add(plan);
                    }
                    else
                    {
                        foreach (BEPlanBilletera objPlan in objLista)
                        {
                            if (plan == objPlan.plan)
                            {
                                objPlan.oBilletera.Add(objBilletera);
                                break;
                            }
                        }
                    }
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

        public DataSet ObtenerDatosEvaluacion(string strOficina, string strTipoDoc, string strNroDoc, string strNroOperacion)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CUR_OFICINA", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_CUR_CLIENTE", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_CUR_REP_LEGAL", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_OFICINA", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DOCUMENTO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_DOCUMENTO", DbType.String, 16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_OPERACION", DbType.String, 20, ParameterDirection.Input)
			};
            int i = 3; arrParam[i].Value = strOficina;
            i++; arrParam[i].Value = strTipoDoc;
            i++; arrParam[i].Value = strNroDoc;
            i++; arrParam[i].Value = strNroOperacion;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strNroDoc);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SP_CON_DATOS_EVALUACION";
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

        public List<BEEquipoBRMS> ConsultarDetalleDecoxKIT(string idKIT)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_CUR_EQUIPO", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_KIT", DbType.Int64, ParameterDirection.Input)}
            ;
            arrParam[1].Value = idKIT;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SP_CON_EQUIPO_DECO_KIT";
            objRequest.Parameters.AddRange(arrParam);

            List<BEEquipoBRMS> objLista = new List<BEEquipoBRMS>();
            BEEquipoBRMS objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEEquipoBRMS();
                    objItem.kit = Funciones.CheckStr(dr["KITV_DESCRIPCION"]);
                    objItem.modelo = Funciones.CheckStr(dr["MATV_DESCRIPCION"]);
                    objItem.tipoDeDeco = Funciones.CheckStr(dr["TIPO_DECO"]);
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
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public double ObtenerCFPromocional(string strIdCampana)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CAMPANA", DbType.String, ParameterDirection.Input),												   
				new DAABRequest.Parameter("P_CF_PROM", DbType.Double, ParameterDirection.Output)
			};
            arrParam[0].Value = strIdCampana;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".SP_CON_CF_PROMOCIONAL";
            objRequest.Parameters.AddRange(arrParam);

            Double dblCF = 0;
            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[1];
                dblCF = Funciones.CheckDbl(parSalida1.Value);
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
            return dblCF;
        }

        public string ConsultarTextoRangoLC(string strTipoDocumento, string strNroDocumento, double dblLC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_COMENTARIO_LC", DbType.String, 50, ParameterDirection.Output),
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_LC", DbType.Double, ParameterDirection.Input)}
            ;
            int i;
            i = 1; arrParam[i].Value = strTipoDocumento;
            i++; arrParam[i].Value = dblLC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strNroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CON_TEXTO_RANGO_LC";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            string strTextoLC = "";
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                strTextoLC = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[0]).Value); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return strTextoLC;
        }

        public List<BEItemGenerico> ObtenerPlanesBSCSxCE(string strCasoEspecial)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAN", DbType.Object, ParameterDirection.Output)
			};
            arrParam[0].Value = strCasoEspecial;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CON_CE_PLANES";
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
                    objItem.Codigo = Funciones.CheckStr(dr["CODIGO_BSCS"]);
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
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public string ConsultarBlackListCE(string strCasoEspecial, string strTipoDoc, string strNroDocumento, ref double dblCfMaximo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_EXISTE", DbType.String, ParameterDirection.Output),
				new DAABRequest.Parameter("P_CF_MAX", DbType.Double, ParameterDirection.Output),
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_DOC", DbType.String, 16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, 5, ParameterDirection.Input)
			};
            string retorno = "";
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[2].Value = strTipoDoc;
            arrParam[3].Value = strNroDocumento;
            arrParam[4].Value = strCasoEspecial;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strNroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Transactional = true;
            objRequest.Parameters.Clear();
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CON_CE_WHITELIST";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter parSalida1, parSalida2;
                parSalida1 = (IDataParameter)objRequest.Parameters[0];
                parSalida2 = (IDataParameter)objRequest.Parameters[1];

                retorno = Funciones.CheckStr(parSalida1.Value);
                dblCfMaximo = Funciones.CheckDbl(parSalida2.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return retorno;
        }

        //PROY-24740
        public void ConsultarCEReglas(string strCasoEspecial, ref string listaCEPlanBscs, ref string listaCEPlan, ref string listaCEProducto)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAN", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_CE_PLAN", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_CE_PRODUCTO", DbType.Object, ParameterDirection.Output)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strCasoEspecial;

            StringBuilder sblCEPlanBscs = new StringBuilder();
            StringBuilder sblCEPlan = new StringBuilder();
            StringBuilder sblCEProducto = new StringBuilder();

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CON_CE_REGLAS";
            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                    {
                    sblCEPlanBscs.Append("|");
                    sblCEPlanBscs.Append(dr["CODIGO_BSCS"]);
                    sblCEPlanBscs.Append(";");
                    sblCEPlanBscs.Append(dr["PRDC_CODIGO"]);
                    }
                listaCEPlanBscs = sblCEPlanBscs.ToString();

                dr.NextResult();

                while (dr.Read())
                    {
                    sblCEPlan.Append("|");
                    sblCEPlan.Append(dr["PLZAC_CODIGO"]);
                    sblCEPlan.Append(";");
                    sblCEPlan.Append(dr["PLNC_CODIGO"]);
                    sblCEPlan.Append(";");
                    sblCEPlan.Append(dr["TCEN_NRO_PLANES"]);
                    }
                listaCEPlan = sblCEPlan.ToString();

                dr.NextResult();

                while (dr.Read())
                {
                    sblCEProducto.Append("|");
                    sblCEProducto.Append(dr["PRDC_CODIGO"]);
                    sblCEProducto.Append(";");
                    sblCEProducto.Append(dr["TCEN_NRO_PLANES"]);
                    }
                listaCEProducto = sblCEProducto.ToString();
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Factory.Dispose();
            }
        }

        public string ValidarVendedorDNI(string strNroDocumento)
        {
            string salida = string.Empty;
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_NRO_DOC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("K_RESULTADO", DbType.String,ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strNroDocumento;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".MANTSS_VALI_VEND_DOCU";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[1];

                salida = Funciones.CheckStr(parSalida1.Value);
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

        public bool InsertarDatosBRMS(Int64 nroSEC, Int64 pln_codigo, BEOfrecimiento oOfrecimiento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("p_solin_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_slpln_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_prdc_codigo", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_solicitud", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_cliente", DbType.String, 4000, ParameterDirection.Input), //PROY-32438
				new DAABRequest.Parameter("p_in_direccion_cliente", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_doc_cliente", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_rrll_cliente", DbType.String, 4000, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_equipo", DbType.String, 500, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_oferta", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_campana", DbType.String, 500, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_plan_actual", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_plan_solicitado", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_servicio", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_pdv", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_direccion_pdv", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_cantidaddeaplicacionesrenta", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_nrolineasadicionalesruc", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_cantidaddelineasmaximas", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_autonomiarenovacion", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("p_capacidaddepago", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_comportamientoconsolidado", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_comportamientodepagoc1", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_controldeconsumo", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_costodeinstalacion", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_costototalequipos", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_factordeendeudamiento", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_factorderenovacion", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_frecuenciarenta", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_limitedecreditocobranza", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_mesiniciorentas", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_montocfpararuc", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_montodegarantia", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_montotopeautomatico", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_preciodeventatotalequipos", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_prioridadpublicar", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_exoneracionderentas", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_idvalidator", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_validacioninternaclaro", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_publicar", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_restriccion", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgoenclaro", DbType.String, 25, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgooferta", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgototalequipo", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgototalreplegales", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tipodeautonomiacargofijo", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tipodecobro", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tipodegarantia", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("p_mensajews", DbType.String, 500, ParameterDirection.Input),
                //PROY-29215 INICIO
                new DAABRequest.Parameter("p_formapago", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("p_nrocuota", DbType.Int32, ParameterDirection.Input),
                new DAABRequest.Parameter("p_motivorestriccion", DbType.String, 150, ParameterDirection.Input), //PROY-140579 0712- INI
		        new DAABRequest.Parameter("p_mostrarmotivorestriccion", DbType.String, 2, ParameterDirection.Input), //PROY-140579 0712- FIN             
                //PROY-29215 FIN               
                new DAABRequest.Parameter("p_ejecucionCP", DbType.String,2,ParameterDirection.Input), //PROY-140335 RF1
                new DAABRequest.Parameter("p_cobroanticipadoinstalacion", DbType.String,50,ParameterDirection.Input), //PROY-140546
                new DAABRequest.Parameter("p_tipocobroanticipadoinstala", DbType.String,50,ParameterDirection.Input) //PROY-140546
			};
            GeneradorLog objLog = new GeneradorLog(null,oOfrecimiento.In_doc_cliente,null,"DATA_LOG");
            int i;
            bool salida = false;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = pln_codigo;
            i++; arrParam[i].Value = oOfrecimiento.IdProducto;

            i++; arrParam[i].Value = oOfrecimiento.In_solicitud;
            i++; arrParam[i].Value = oOfrecimiento.In_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_direccion_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_doc_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_rrll_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_equipo;
            i++; arrParam[i].Value = oOfrecimiento.In_oferta;
            i++; arrParam[i].Value = oOfrecimiento.In_campana;
            i++; arrParam[i].Value = oOfrecimiento.In_plan_actual;
            i++; arrParam[i].Value = oOfrecimiento.In_plan_solicitado;
            i++; arrParam[i].Value = oOfrecimiento.In_servicio;
            i++; arrParam[i].Value = oOfrecimiento.In_pdv;
            i++; arrParam[i].Value = oOfrecimiento.In_direccion_pdv;

            i++; arrParam[i].Value = oOfrecimiento.CantidadDeAplicacionesRenta;
            i++; arrParam[i].Value = oOfrecimiento.CantidadDeLineasAdicionalesRUC;
            i++; arrParam[i].Value = oOfrecimiento.CantidadDeLineasMaximas;
            i++; arrParam[i].Value = oOfrecimiento.AutonomiaRenovacion;
            i++; arrParam[i].Value = oOfrecimiento.CapacidadDePago;
            i++; arrParam[i].Value = oOfrecimiento.ComportamientoConsolidado;
            i++; arrParam[i].Value = oOfrecimiento.ComportamientoDePagoC1;
            i++; arrParam[i].Value = oOfrecimiento.ControlDeConsumo;
            i++; arrParam[i].Value = oOfrecimiento.CostoDeInstalacion;
            i++; arrParam[i].Value = oOfrecimiento.CostoTotalEquipos;
            i++; arrParam[i].Value = oOfrecimiento.FactorDeEndeudamientoCliente;
            i++; arrParam[i].Value = oOfrecimiento.FactorDeRenovacionCliente;
            i++; arrParam[i].Value = oOfrecimiento.FrecuenciaDeAplicacionMensual;
            i++; arrParam[i].Value = oOfrecimiento.LimiteDeCreditoCobranza;
            i++; arrParam[i].Value = oOfrecimiento.MesInicioRentas;
            i++; arrParam[i].Value = oOfrecimiento.MontoCFParaRUC;
            i++; arrParam[i].Value = oOfrecimiento.MontoDeGarantia;
            i++; arrParam[i].Value = oOfrecimiento.MontoTopeAutomatico;
            i++; arrParam[i].Value = oOfrecimiento.PrecioDeVentaTotalEquipos;
            i++; arrParam[i].Value = oOfrecimiento.PrioridadPublicar;
            i++; arrParam[i].Value = oOfrecimiento.ProcesoDeExoneracionDeRentas;
            i++; arrParam[i].Value = oOfrecimiento.ProcesoIDValidator;
            i++; arrParam[i].Value = oOfrecimiento.ProcesoValidacionInternaClaro;
            i++; arrParam[i].Value = oOfrecimiento.Publicar;
            i++; arrParam[i].Value = oOfrecimiento.Restriccion;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoEnClaro;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoOferta;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoTotalEquipo;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoTotalRepLegales;
            i++; arrParam[i].Value = oOfrecimiento.TipoDeAutonomiaCargoFijo;
            i++; arrParam[i].Value = oOfrecimiento.Tipodecobro;
            i++; arrParam[i].Value = oOfrecimiento.TipoDeGarantia;
            i++; arrParam[i].Value = oOfrecimiento.Mensaje;
            //PROY-29215 INICIO
            i++; arrParam[i].Value = oOfrecimiento.FormaPago; 
            i++; arrParam[i].Value =  oOfrecimiento.NroCuota; 
            //PROY-29215 FIN
            i++; arrParam[i].Value = oOfrecimiento.MotivoDeRestriccion; //PROY-140579 0712- INI
            i++; arrParam[i].Value = oOfrecimiento.MostrarMotivoDeRestriccion; //PROY-140579 0712 - FIN
            i++; arrParam[i].Value = oOfrecimiento.ejecucionConsultaPrevia;  //PROY-140335
            i++; arrParam[i].Value = oOfrecimiento.MontoAnticipadoInstalacion; //PROY-140546
            i++; arrParam[i].Value = oOfrecimiento.TipoCobroAnticipadoInstalacion; //PROY-140546

            objLog.CrearArchivolog("[Inicio][InsertarDatosBRMS]",null,null);
            objLog.CrearArchivolog("[SEC]", nroSEC.ToString(), null);
                     
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            //objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SP_INS_DATOS_EVALUACION";
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SP_INS_DATOS_EVAL_SISACT";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][InsertarDatosBRMS]", null, ex);
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][InsertarDatosBRMS]", null, null);
            return salida;
        }


        public DataSet ObtenerDatosBRMS(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_EXISTE", DbType.Int64,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_SOLICITUD", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_CLIENTE", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_DIRECCION_CLIENTE", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_DOCUMENTO_CLIENTE", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_RRLL", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_EQUIPOS", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_OFERTA", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_CAMP", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_SERVICIOS", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_PLAN_ACTUAL", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_PLAN_SOLICITADO", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_PUNTO_VENTA", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_DIRECCION_PDV", DbType.Object,ParameterDirection.Output),
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            string[] sTab = { "CV_SOLICITUD", "CV_CLIENTE", "CV_DIRECCION_CLIENTE", "CV_DOCUMENTO_CLIENTE", "CV_RRLL", "CV_EQUIPOS", "CV_OFERTA", "CV_CAMP", "CV_SERVICIOS", "CV_PLAN_ACTUAL", "CV_PLAN_SOLICITADO", "CV_PUNTO_VENTA", "CV_DIRECCION_PDV" };
            objRequest.TableNames = sTab;
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SP_CONSULTA_SEC_BRMS";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            DataSet ds = new DataSet();
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
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }

         public DataTable ObtenerDetallePlanes(Int64 nroSECPadre, Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
             //                   new DAABRequest.Parameter("P_CODIGO", DbType.String,5, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_PLAN", DbType.Object, ParameterDirection.Output)
			};
            GeneradorLog objLog = new GeneradorLog(null, nroSEC.ToString(),null,"DATA_LOG");
          
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSECPadre;
            if (nroSEC > 0) arrParam[1].Value = nroSEC;
           //arrParam[2].Value = strIdProd;

           objLog.CrearArchivolog("[INICIO][ObtenerDetallePlanes]", null, null);
           objLog.CrearArchivolog("[SECPadre]", nroSECPadre.ToString(), null);
           objLog.CrearArchivolog("[SEC]", nroSEC.ToString(), null);
           //objLog.CrearArchivolog("[ID_PROD]", strIdProd.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSECPadre.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_CON_PLAN_DETALLE_I";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ObtenerDetallePlanes]", null, ex);
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
                objLog.CrearArchivolog("[Fin][ObtenerDetallePlanes]", null, null);
            }
        }

        public DataSet ObtenerDetalleSrvCuota(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SERVICIO", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("K_CUR_CUOTA", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("K_CUR_EQUIPO", DbType.Object, ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_CON_SERVICIO_DETALLE";
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
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }

        public DataTable ListarMetricaEvaluacion(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CUR_EVAL", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64, ParameterDirection.Input)
			};
            arrParam[1].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SP_CON_METRICA_EVALUACION";
            obRequest.Parameters.AddRange(arrParam);

            DataTable dt = null;
            try
            {
                dt = obRequest.Factory.ExecuteDataset(ref obRequest).Tables[0];
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
            return dt;
        }

        public string ObtenerFechaHoraBD(string nroDocumento)
        {
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.Text;
            objRequest.Command = string.Format("select {0}.FC_CON_FECHA_HORA from dual", BaseDatos.PKG_SISACT_EVALUACION_UNI);

            string fechaHora;
            try
            {
                fechaHora = Funciones.CheckStr(objRequest.Factory.ExecuteScalar(ref objRequest));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return fechaHora;
        }

        public Int64 ValidarSecConcurrente(string tipoDocumento, string nroDocumento, string strHoraInicio)
        {
            string salida = string.Empty;
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("K_SEC", DbType.Int64, ParameterDirection.Output),
                new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_NRO_DOC", DbType.String, 16, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HORA_INI", DbType.String, 16, ParameterDirection.Input)
			};
            int i = 0;
            i++; arrParam[i].Value = tipoDocumento;
            i++; arrParam[i].Value = Funciones.FormatoNroDocumentoBD(tipoDocumento, nroDocumento);
            i++; arrParam[i].Value = strHoraInicio;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_VALIDA_SEC_CONCURRENTE";
            objRequest.Parameters.AddRange(arrParam);
            Int64 nroSEC = 0;
            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter pSalida = (IDataParameter)objRequest.Parameters[0];
                nroSEC = Funciones.CheckInt64(pSalida.Value);
            }
            catch
            {
                nroSEC = 0;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }

        public bool InsertarDatosGarantia(Int64 nroSEC, Int64 pln_codigo, BEGarantia objGarantia)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("p_solin_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_slpln_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_prdc_codigo", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_plnv_codigo", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_plnv_descripcion", DbType.String, 150, ParameterDirection.Input),
				new DAABRequest.Parameter("p_solin_sum_cf", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tcarc_codigo", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("p_solin_num_cf", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_solin_importe", DbType.Double, ParameterDirection.Input)
			};
            bool salida = false;

            int i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = pln_codigo;

            i++; arrParam[i].Value = objGarantia.idProducto;
            i++; arrParam[i].Value = objGarantia.idPlan;
            i++; arrParam[i].Value = objGarantia.plan;
            i++; arrParam[i].Value = objGarantia.CF;
            i++; arrParam[i].Value = objGarantia.idGarantia;
            i++; arrParam[i].Value = objGarantia.nroGarantia;
            i++; arrParam[i].Value = objGarantia.importe;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_INS_DATOS_GARANTIA";
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

        public bool InsertarDatosDescuento(Int64 nroSEC, Int64 spln_codigo, string prdc_codigo, string plnv_codigo, string plnv_descripcion, string usuario, string idCombo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("p_solin_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_slpln_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_prdc_codigo", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_plnv_codigo", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_plnv_descripcion", DbType.String, 150, ParameterDirection.Input),
				new DAABRequest.Parameter("p_combv_codigo", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("p_usuario", DbType.String, 10, ParameterDirection.Input)
			};
            bool salida = false;

            int i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = spln_codigo;
            i++; arrParam[i].Value = prdc_codigo;
            i++; arrParam[i].Value = plnv_codigo;
            i++; arrParam[i].Value = plnv_descripcion;
            i++; arrParam[i].Value = idCombo;
            i++; arrParam[i].Value = usuario;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".sp_ins_datos_combo";
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
//gaa20170327
        public string ObtenerBuroDescripcion(string strBuroCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_BURO_COD", DbType.Int32, ParameterDirection.Input),												   
				new DAABRequest.Parameter("P_BURO_DES", DbType.String, ParameterDirection.Output)
			};
            arrParam[0].Value = strBuroCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_BUROCREDITICIO_DES";
            objRequest.Parameters.AddRange(arrParam);

            string strBuroDescripcion = string.Empty;
            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[1];
                strBuroDescripcion = Funciones.CheckStr(parSalida1.Value);
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
            return strBuroDescripcion;
        }
//fin gaa20170327


        // INICIO - PROY - 30748 
        public DataSet ObtenerDatosBRMSPROA(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_EXISTE", DbType.Int64,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_SOLICITUD", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_CLIENTE", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_DIRECCION_CLIENTE", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_DOCUMENTO_CLIENTE", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_RRLL", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_EQUIPOS", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_OFERTA", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_CAMP", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_SERVICIOS", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_PLAN_ACTUAL", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_PLAN_SOLICITADO", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_PUNTO_VENTA", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("CV_DIRECCION_PDV", DbType.Object,ParameterDirection.Output),
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            string[] sTab = { "CV_SOLICITUD", "CV_CLIENTE", "CV_DIRECCION_CLIENTE", "CV_DOCUMENTO_CLIENTE", "CV_RRLL", "CV_EQUIPOS", "CV_OFERTA", "CV_CAMP", "CV_SERVICIOS", "CV_PLAN_ACTUAL", "CV_PLAN_SOLICITADO", "CV_PUNTO_VENTA", "CV_DIRECCION_PDV" };
            objRequest.TableNames = sTab;
            objRequest.CommandType = CommandType.StoredProcedure;
            //objRequest.Command =  "SISACT_PKG_CONSULTA_PROY_30478.SP_CONSULTA_SEC_BRMS_PROA"; //PILOTO
            objRequest.Command = "SISACT_PKG_CONSULTA_BRMS.SP_CONSULTA_SEC_BRMS_PROA"; //PRODUCCION            

            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            DataSet ds = new DataSet();
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
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }

        public bool InsertarDatosBRMSPROA(Int64 nroSEC, Int64 pln_codigo, BEOfrecimiento oOfrecimiento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("p_solin_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_slpln_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_prdc_codigo", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_solicitud", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_cliente", DbType.String, 4000, ParameterDirection.Input),//PROY 30748 F2 MDE
				new DAABRequest.Parameter("p_in_direccion_cliente", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_doc_cliente", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_rrll_cliente", DbType.String, 4000, ParameterDirection.Input),//PROY 30748 F2 MDE
				new DAABRequest.Parameter("p_in_equipo", DbType.String, 500, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_oferta", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_campana", DbType.String, 500, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_plan_actual", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_plan_solicitado", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_servicio", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_pdv", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_direccion_pdv", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_cantidaddeaplicacionesrenta", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_nrolineasadicionalesruc", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_cantidaddelineasmaximas", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_autonomiarenovacion", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("p_capacidaddepago", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_comportamientoconsolidado", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_comportamientodepagoc1", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_controldeconsumo", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_costodeinstalacion", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_costototalequipos", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_factordeendeudamiento", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_factorderenovacion", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_frecuenciarenta", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_limitedecreditocobranza", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_mesiniciorentas", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_montocfpararuc", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_montodegarantia", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_montotopeautomatico", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_preciodeventatotalequipos", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_prioridadpublicar", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_exoneracionderentas", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_idvalidator", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_validacioninternaclaro", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_publicar", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_restriccion", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgoenclaro", DbType.String, 25, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgooferta", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgototalequipo", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgototalreplegales", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tipodeautonomiacargofijo", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tipodecobro", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tipodegarantia", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("p_mensajews", DbType.String, 500, ParameterDirection.Input),
                new DAABRequest.Parameter("P_EJECUCIONCP", DbType.String, 2, ParameterDirection.Input),//PROY-140335 RF1
                new DAABRequest.Parameter("P_MOTIVO_RESTRICCION", DbType.String, 150, ParameterDirection.Input),//PROY-140335 RF1
                new DAABRequest.Parameter("P_MOSTRAR_MOTIVO_RESTRICCION", DbType.String, 2, ParameterDirection.Input),//PROY-140335 RF1
			};
            GeneradorLog objLog = new GeneradorLog(null, oOfrecimiento.In_doc_cliente, null, "DATA_LOG");
            int i;
            bool salida = false;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = pln_codigo;
            i++; arrParam[i].Value = oOfrecimiento.IdProducto;

            i++; arrParam[i].Value = oOfrecimiento.In_solicitud;
            i++; arrParam[i].Value = oOfrecimiento.In_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_direccion_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_doc_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_rrll_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_equipo;
            i++; arrParam[i].Value = oOfrecimiento.In_oferta;
            i++; arrParam[i].Value = oOfrecimiento.In_campana;
            i++; arrParam[i].Value = oOfrecimiento.In_plan_actual;
            i++; arrParam[i].Value = oOfrecimiento.In_plan_solicitado;
            i++; arrParam[i].Value = oOfrecimiento.In_servicio;
            i++; arrParam[i].Value = oOfrecimiento.In_pdv;
            i++; arrParam[i].Value = oOfrecimiento.In_direccion_pdv;

            i++; arrParam[i].Value = oOfrecimiento.CantidadDeAplicacionesRenta;
            i++; arrParam[i].Value = oOfrecimiento.CantidadDeLineasAdicionalesRUC;
            i++; arrParam[i].Value = oOfrecimiento.CantidadDeLineasMaximas;
            i++; arrParam[i].Value = oOfrecimiento.AutonomiaRenovacion;
            i++; arrParam[i].Value = oOfrecimiento.CapacidadDePago;
            i++; arrParam[i].Value = oOfrecimiento.ComportamientoConsolidado;
            i++; arrParam[i].Value = oOfrecimiento.ComportamientoDePagoC1;
            i++; arrParam[i].Value = oOfrecimiento.ControlDeConsumo;
            i++; arrParam[i].Value = oOfrecimiento.CostoDeInstalacion;
            i++; arrParam[i].Value = oOfrecimiento.CostoTotalEquipos;
            i++; arrParam[i].Value = oOfrecimiento.FactorDeEndeudamientoCliente;
            i++; arrParam[i].Value = oOfrecimiento.FactorDeRenovacionCliente;
            i++; arrParam[i].Value = oOfrecimiento.FrecuenciaDeAplicacionMensual;
            i++; arrParam[i].Value = oOfrecimiento.LimiteDeCreditoCobranza;
            i++; arrParam[i].Value = oOfrecimiento.MesInicioRentas;
            i++; arrParam[i].Value = oOfrecimiento.MontoCFParaRUC;
            i++; arrParam[i].Value = oOfrecimiento.MontoDeGarantia;
            i++; arrParam[i].Value = oOfrecimiento.MontoTopeAutomatico;
            i++; arrParam[i].Value = oOfrecimiento.PrecioDeVentaTotalEquipos;
            i++; arrParam[i].Value = oOfrecimiento.PrioridadPublicar;
            i++; arrParam[i].Value = oOfrecimiento.ProcesoDeExoneracionDeRentas;
            i++; arrParam[i].Value = oOfrecimiento.ProcesoIDValidator;
            i++; arrParam[i].Value = oOfrecimiento.ProcesoValidacionInternaClaro;
            i++; arrParam[i].Value = oOfrecimiento.Publicar;
            i++; arrParam[i].Value = oOfrecimiento.Restriccion;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoEnClaro;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoOferta;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoTotalEquipo;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoTotalRepLegales;
            i++; arrParam[i].Value = oOfrecimiento.TipoDeAutonomiaCargoFijo;
            i++; arrParam[i].Value = oOfrecimiento.Tipodecobro;
            i++; arrParam[i].Value = oOfrecimiento.TipoDeGarantia;
            i++; arrParam[i].Value = oOfrecimiento.Mensaje;
            i++; arrParam[i].Value = oOfrecimiento.ejecucionConsultaPrevia; //PROY-140335 RF1
            i++; arrParam[i].Value = oOfrecimiento.MotivoDeRestriccion; //PROY-140579 F2
            i++; arrParam[i].Value = oOfrecimiento.MostrarMotivoDeRestriccion; //PROY-140579 F2

            objLog.CrearArchivolog("[Inicio][InsertarDatosProaBRMS]", null, null);
            objLog.CrearArchivolog("[SEC]", nroSEC.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            //objRequest.Command = "SISACT_PKG_CONSULTA_PROY_30478.SP_INS_DATOS_EVALUACION_PROA";  //PILOTO
            objRequest.Command = "SISACT_PKG_CONSULTA_BRMS.SP_INS_DATOS_EVALUACION_PROA";  //PRODUCCION
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][InsertarDatosProaBRMS]", null, ex);
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][InsertarDatosProaBRMS]", null, null);
            return salida;
        }
        // FIN - PROY - 30748   
        //PROY-30166-IDEA–38863- INICIO 
        public bool ActualizarMontoInicial(Int64 intSopln, double dblMontoCuota, double dblPorcentajeCuota, ref string strCodRpta, ref string strMsjRpta) //PROY-30166-IDEA–38863-INICIO 
        {
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("K_SOPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
				                                 new DAABRequest.Parameter("K_MONTOCUOTA", DbType.Double, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_CUOTA_INICIAL", DbType.Int32, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_CODIGO_RESPUESTA", DbType.String,20, ParameterDirection.Output),
                                                 new DAABRequest.Parameter("K_MENSAJE_RESPUESTA", DbType.String, 200, ParameterDirection.Output)};

            arrParam[0].Value = intSopln;
            arrParam[1].Value = dblMontoCuota;

            if (dblPorcentajeCuota == 0) 
            {
                arrParam[2].Value = DBNull.Value;
            }
            else
            {
                arrParam[2].Value = dblPorcentajeCuota;
            }  
            bool salida = false;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACTSU_CUOTA_INICIAL";
            obRequest.Parameters.AddRange(arrParam);

            obRequest.Transactional = true;
            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                strCodRpta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[3]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[4]).Value);
                if (strCodRpta == "0") salida = true;
            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                strCodRpta = "";
                strMsjRpta = "";
                throw ex;
            }
            finally
            {
                obRequest.Factory.Dispose();
            }
            return salida;
        } //PROY-30166-IDEA–38863-FIN 

        //PROY-30166-IDEA-38863-INI
        public string ObtenerCuotaIniCom(string CodListaPrecio, string CodMaterial, string CodPlazo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CODIGOLISTAPRECIO", DbType.String, ParameterDirection.Input),	
				new DAABRequest.Parameter("P_CODMATERIAL", DbType.String, ParameterDirection.Input),							   
				new DAABRequest.Parameter("P_CODPLAZO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CUOTAINICIAL", DbType.Double, ParameterDirection.Output),
                new DAABRequest.Parameter("P_MONTOCUOTA", DbType.Double, ParameterDirection.Output),
                new DAABRequest.Parameter("P_RESULTADO", DbType.Int32, ParameterDirection.Output),
                new DAABRequest.Parameter("P_MENSAJE", DbType.String, ParameterDirection.Output)
			};
            arrParam[0].Value = CodListaPrecio;
            arrParam[1].Value = CodMaterial;
            arrParam[2].Value = CodPlazo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_NUEVA_LISTAPRE_6 + ".SISACTSS_CUOTA_INI_COMERCIAL";
            objRequest.Parameters.AddRange(arrParam);
            StringBuilder sblObtDatos = new StringBuilder();
            string strObtDatos = string.Empty;
            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter parSalida1;
                IDataParameter parSalida2;
                IDataParameter parSalida3;
                IDataParameter parSalida4;


                parSalida1 = (IDataParameter)objRequest.Parameters[3]; //Obtiene CUOTAINICIAL
                parSalida2 = (IDataParameter)objRequest.Parameters[4]; //Obtiene MONTOCUOTA
                parSalida3 = (IDataParameter)objRequest.Parameters[5]; // RESULTADO 
                parSalida4 = (IDataParameter)objRequest.Parameters[6]; // MENSAJE


                sblObtDatos.Append(Funciones.CheckStr(parSalida1.Value));
                sblObtDatos.Append(";");
                sblObtDatos.Append(Funciones.CheckStr(parSalida2.Value));
                sblObtDatos.Append(";");
                sblObtDatos.Append(Funciones.CheckStr(parSalida3.Value));
                sblObtDatos.Append(";");
                sblObtDatos.Append(Funciones.CheckStr(parSalida4.Value));
                strObtDatos = sblObtDatos.ToString();
            }
            catch (Exception ex)
            {
                strObtDatos = "0;0;-1;" + ex.Message;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return strObtDatos;
        }
        //PROY-30166-IDEA-38863-FIN

        //PROY-32439 MAS INI Insert PKG+SP
        public bool InsertarDatosBRMSCliente(BEDatosClienteBrms objDatosClienteBrms)
        {
        bool boolRegistro = false;

        DAABRequest.Parameter[] arrParam = {
                                            new DAABRequest.Parameter("P_DAVCN_SOLIN_CODIGO", DbType.String , ParameterDirection.Input),                                                                                                                             
                                            new DAABRequest.Parameter("P_DAVCV_IN_CLIENTE", DbType.String , 200, ParameterDirection.Input),                                                                                                                                    
                                            new DAABRequest.Parameter("P_DAVCV_IN_SOLICITUD", DbType.String , 150, ParameterDirection.Input),                                                                                                                                
                                            new DAABRequest.Parameter("P_DAVCV_IN_PDV", DbType.String , 150, ParameterDirection.Input),                                                                                                                               
                                            new DAABRequest.Parameter("P_DAVCV_IN_LINEA", DbType.String , 150,ParameterDirection.Input),                                                                                                                            
                                            new DAABRequest.Parameter("P_DAVCV_VAL_CLIENTE", DbType.String , 2,ParameterDirection.Input),                                                                                                                                 
                                            new DAABRequest.Parameter("P_DAVCV_MSJ_VALCLIENTE", DbType.String ,100, ParameterDirection.Input),                                                                                                                           
                                            new DAABRequest.Parameter("P_DAVCV_RESTR_PRODCOM", DbType.String , 100, ParameterDirection.Input),                                                                                                                                     
                                            new DAABRequest.Parameter("P_DAVCV_RESTR_PROD", DbType.String , 100, ParameterDirection.Input),                                                                                                                                                                                                                                                                   
                                            new DAABRequest.Parameter("P_DAVCV_RESTR_OPEN_CODIGO", DbType.String , 100, ParameterDirection.Input),                                                                                                                                                                                                                                                                
                                            new DAABRequest.Parameter("P_DAVCV_MENSAJEWS", DbType.String ,500 , ParameterDirection.Input),                                                                                                                                                                                                                                                                     
                                            new DAABRequest.Parameter("P_DAVCN_SOLIN_GRUPO_SEC", DbType.String , ParameterDirection.Input),                                                                                                                                                                                                                                                                    
                                            new DAABRequest.Parameter("P_CODIGO_RESPUESTA", DbType.String, ParameterDirection.Output),
                                            new DAABRequest.Parameter("P_MENSAJE_RESPUESTA", DbType.String, ParameterDirection.Output),                                                     
                                            };                     
                                   
        int i;                            
            for (i=0; i<arrParam.Length;i++)
            {
                arrParam[i].Value = DBNull.Value;
            }

            i = 0; arrParam[i].Value = objDatosClienteBrms.DAVCN_SOLIN_CODIGO;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCV_IN_CLIENTE;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCV_IN_SOLICITUD;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCV_IN_PDV;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCV_IN_LINEA;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCV_VAL_CLIENTE;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCV_MSJ_VALCLIENTE;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCV_RESTR_PRODCOM;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCV_RESTR_PROD;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCV_RESTR_OPEN_CODIGO;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCV_MENSAJEWS;
            i++; arrParam[i].Value = objDatosClienteBrms.DAVCN_SOLIN_GRUPO_SEC;

            objLog.CrearArchivolog("[INICIO][InsertarDatosBRMSCliente]", null, null);
            objLog.CrearArchivolog("[SEC]", objDatosClienteBrms.DAVCN_SOLIN_CODIGO.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);      
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SISACTSI_DATOS_BRMS_VALCLIE";
            obRequest.Parameters.AddRange(arrParam);
            obRequest.Transactional = true;
            try
            {
                        obRequest.Factory.ExecuteNonQuery(ref obRequest);
                        obRequest.Factory.CommitTransaction();
                        boolRegistro = true;
            }
            catch(Exception ex)
            {
                        obRequest.Factory.RollBackTransaction();
                        objLog.CrearArchivolog("[ERROR][InsertarDatosBRMSCliente]", null, null);                     
                        throw ex;
            }
            finally
            {                                                          
                        obRequest.Parameters.Clear();
                        obRequest.Factory.Dispose();
                        objLog.CrearArchivolog("[FIN][InsertarDatosBRMSCliente]", null, null);
            }                                  
            return boolRegistro;
        }
        //PROY-32439 MAS FIN Insert PKG+SP

        //PROY-32439 MAS INI
        public DataSet ObtenerDatos_Validacion_Cliente_BRMS(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("pin_sec", DbType.Int64,ParameterDirection.Input),
                new DAABRequest.Parameter("cout_request_cliente", DbType.Object,ParameterDirection.Output),
                new DAABRequest.Parameter("cout_request_linea", DbType.Object,ParameterDirection.Output),
                new DAABRequest.Parameter("cout_request_pdv", DbType.Object,ParameterDirection.Output),
                new DAABRequest.Parameter("cout_request_datos", DbType.Object,ParameterDirection.Output),
                new DAABRequest.Parameter("cout_response", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("cout_datos", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("pout_respuesta_codigo", DbType.String,ParameterDirection.Output),
				new DAABRequest.Parameter("pout_respuesta_mensaje", DbType.String,ParameterDirection.Output),
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSEC;

            objLog.CrearArchivolog("[INICIO][ObtenerDatos_Validacion_Cliente_BRMS]", null, null);
            objLog.CrearArchivolog("[SEC]", nroSEC, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            string[] sTab = { "cout_request_cliente", "cout_request_linea", "cout_request_pdv", "cout_request_datos", "cout_response", "cout_datos" };
            objRequest.TableNames = sTab;
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SISACTSS_DATOS_BRMS_VALCLIE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ObtenerDatos_Validacion_Cliente_BRMS]", null, null);  
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
                objLog.CrearArchivolog("[FIN][ObtenerDatos_Validacion_Cliente_BRMS]", null, null);
            }
        }
        //PROY-32439 MAS FIN

        //PROY-32439 - CAMBIOS_LOG INI
        public bool InsertarLogNuevoBRMS(ValidacionDeudaBRMSrequest objDatosRequestNuevoBRMS, string strTipoBloqueoBRMS, string strTipoLineaBloqueoBRMS, string strTipoSusBRMS, string strTipoLineaSusBRMS, string strTipoFraudeBRMS, ValidacionDeudaBRMSresponse objResponse, Int64 strFlagWhilist, Int64 strFlagTieneDeuda, string strCodUsuario, Int64 intFlagErrorBRMS, string strMensajeErrorBRMS, string strResProComercial, string strResProducto, string strResTipoOperacion, string strMensaje) //PROY-140743
        {
            bool boolRegistro = false;

            DAABRequest.Parameter[] arrParam = {
                                            new DAABRequest.Parameter("P_Req_DecisionID", DbType.String,500 , ParameterDirection.Input),                                                                                                                             
                                            new DAABRequest.Parameter("P_Req_cli_antiguedadDeuda", DbType.Int64, ParameterDirection.Input),                                                                                                                                    
                                            new DAABRequest.Parameter("P_Req_cli_Tipos_bloqueos", DbType.String, 32767,  ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_cli_TiposLinea_bloqueos", DbType.String, 32767,  ParameterDirection.Input),                                                                                   
                                            new DAABRequest.Parameter("P_Req_cantidadDocumentosDeuda", DbType.Int64, ParameterDirection.Input),                                                                                                                               
                                            new DAABRequest.Parameter("P_Req_cli_comportamientoPago", DbType.String,500,ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_cli_flagBloqueos", DbType.String,500,ParameterDirection.Input),                                                                                                                                                                                                         
                                            new DAABRequest.Parameter("P_Req_cli_flagSuspensiones", DbType.String,500, ParameterDirection.Input),                                                                                                                           
                                            new DAABRequest.Parameter("P_Req_cli_montoDeuda", DbType.Double, ParameterDirection.Input),                                                                                                                                     
                                            new DAABRequest.Parameter("P_Req_cli_montoDeudaCastigada", DbType.Double, ParameterDirection.Input),                                                                                                                                                                                                                                                                   
                                            new DAABRequest.Parameter("P_Req_cli_montoDeudaVencida", DbType.Double, ParameterDirection.Input),                                                                                                                                                                                                                                                                
                                            new DAABRequest.Parameter("P_Req_cli_montoTotalPago", DbType.Double, ParameterDirection.Input),                                                                                                                                                                                                                                                                     
                                            new DAABRequest.Parameter("P_Req_promedioFacturadoSoles", DbType.Double, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_cli_segmento", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_Tipo_suspensiones", DbType.String,32767, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_TipoLinea_suspensiones", DbType.String,32767, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_cli_tiempoPermanencia", DbType.Int64, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_cli_tiposFraude", DbType.String,32767, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_dis_antiguedad", DbType.Int64, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_dis_cantidad", DbType.Int64, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_dis_monto", DbType.Double, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_doc_tipo", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_doc_numero", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_lin_antiguedad", DbType.Int64, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_lin_fechaActivacion", DbType.Date, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_pdv_canal", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_pdv_codigo", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_pdv_departamento", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_pdv_nombre", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_pdv_region", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_pdv_segmento", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_cod_vendedor", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_nom_vendedor", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_sol_sistemaEvaluacion", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Req_sol_tipoOperacion", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Res_DecisionID", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Res_validacionCliente", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Res_mensajeValidacionCliente", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Res_resProductoComercial", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Res_restriccionProducto", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Res_restriccionTipoOperacion", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Res_errorBrms", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Res_mensajeDeudaBloqueo", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Flag_WhiteList", DbType.Int64, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Flag_Deuda", DbType.Int64, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Audi_Fecha_Creacion", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("P_Audi_Usu_Creacion", DbType.String,500, ParameterDirection.Input),
                                            new DAABRequest.Parameter("p_MONTOPENDCUOTASACC", DbType.Double, ParameterDirection.Input), /*PROY-140743-INI*/
                                            new DAABRequest.Parameter("p_CANTLINEASCUOTASPENDACC", DbType.Int64, ParameterDirection.Input),
                                            new DAABRequest.Parameter("p_CANTMAXCUOTASPENDACC", DbType.Int64, ParameterDirection.Input),
                                            new DAABRequest.Parameter("p_MONTOPENDCUOTASACC_ULTVTAS", DbType.Double, ParameterDirection.Input),
                                            new DAABRequest.Parameter("p_CATLINASCUOPENDACC_ULTVTAS", DbType.Int64, ParameterDirection.Input),
                                            new DAABRequest.Parameter("p_CANTMAXCUOTASPENDACC_ULTVTAS", DbType.Int64, ParameterDirection.Input), /*PROY-140743-FIN*/
                                            new DAABRequest.Parameter("P_CODIGO_RESPUESTA", DbType.String, ParameterDirection.Output),
                                            new DAABRequest.Parameter("P_MENSAJE_RESPUESTA", DbType.String, ParameterDirection.Output)                                                     
                                            };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objDatosRequestNuevoBRMS.decisionID;
            arrParam[1].Value = objDatosRequestNuevoBRMS.cliente.antiguedadDeuda;
            arrParam[2].Value = strTipoBloqueoBRMS;
            arrParam[3].Value = strTipoLineaBloqueoBRMS;
            arrParam[4].Value = objDatosRequestNuevoBRMS.cliente.cantidadDocumentosDeuda;
            arrParam[5].Value = objDatosRequestNuevoBRMS.cliente.comportamientoPago;
            arrParam[6].Value = objDatosRequestNuevoBRMS.cliente.flagBloqueos.ToString();
            arrParam[7].Value = objDatosRequestNuevoBRMS.cliente.flagSuspensiones.ToString();
            arrParam[8].Value = objDatosRequestNuevoBRMS.cliente.montoDeuda;
            arrParam[9].Value = objDatosRequestNuevoBRMS.cliente.montoDeudaCastigada;
            arrParam[10].Value = objDatosRequestNuevoBRMS.cliente.montoDeudaVencida;
            arrParam[11].Value = objDatosRequestNuevoBRMS.cliente.montoTotalPago;
            arrParam[12].Value = objDatosRequestNuevoBRMS.cliente.promedioFacturadoSoles;
            arrParam[13].Value = objDatosRequestNuevoBRMS.cliente.segmento;
            arrParam[14].Value = strTipoSusBRMS;
            arrParam[15].Value = strTipoLineaSusBRMS;
            arrParam[16].Value = objDatosRequestNuevoBRMS.cliente.tiempoPermanencia;
            arrParam[17].Value = strTipoFraudeBRMS;
            arrParam[18].Value = objDatosRequestNuevoBRMS.cliente.disputa.antiguedad;
            arrParam[19].Value = objDatosRequestNuevoBRMS.cliente.disputa.cantidad;
            arrParam[20].Value = objDatosRequestNuevoBRMS.cliente.disputa.monto;
            arrParam[21].Value = objDatosRequestNuevoBRMS.cliente.documento.tipo;
            arrParam[22].Value = objDatosRequestNuevoBRMS.cliente.documento.numero;
            arrParam[23].Value = 0;
            arrParam[24].Value = null;
            arrParam[25].Value = objDatosRequestNuevoBRMS.puntoDeVenta.canal;
            arrParam[26].Value = objDatosRequestNuevoBRMS.puntoDeVenta.codigo;
            arrParam[27].Value = objDatosRequestNuevoBRMS.puntoDeVenta.departamento;
            arrParam[28].Value = objDatosRequestNuevoBRMS.puntoDeVenta.nombre;
            arrParam[29].Value = objDatosRequestNuevoBRMS.puntoDeVenta.region;
            arrParam[30].Value = objDatosRequestNuevoBRMS.puntoDeVenta.segmento;
            arrParam[31].Value = objDatosRequestNuevoBRMS.puntoDeVenta.vendedor.codigo;
            arrParam[32].Value = objDatosRequestNuevoBRMS.puntoDeVenta.vendedor.nombre;
            arrParam[33].Value = objDatosRequestNuevoBRMS.sistemaEvaluacion;
            arrParam[34].Value = objDatosRequestNuevoBRMS.tipoOperacion;
            arrParam[35].Value = objResponse.decisionID;
            arrParam[36].Value = objResponse.validacionCliente.ToString();
            arrParam[37].Value = objResponse.mensajeValidacionCliente;
            arrParam[38].Value = strResProComercial;
            arrParam[39].Value = strResProducto;
            arrParam[40].Value = strResTipoOperacion;
            arrParam[41].Value = intFlagErrorBRMS;
            arrParam[42].Value = strMensajeErrorBRMS;
            arrParam[43].Value = strFlagWhilist;
            arrParam[44].Value = strFlagTieneDeuda;
            arrParam[45].Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
            arrParam[46].Value = strCodUsuario;

            if (!string.IsNullOrEmpty(strMensaje))
            {
                arrParam[47].Value = Funciones.CheckDbl(objDatosRequestNuevoBRMS.cliente.montoCuotasPendientesAcc);
                arrParam[48].Value = Funciones.CheckInt64(objDatosRequestNuevoBRMS.cliente.cantidadLineaCuotasPendientesAcc);
                arrParam[49].Value = Funciones.CheckInt64(objDatosRequestNuevoBRMS.cliente.cantidadMaximaCuotasPendientesAcc);
                arrParam[50].Value = Funciones.CheckDbl(objDatosRequestNuevoBRMS.cliente.montoCuotasPendientesAccUltiVenta);
                arrParam[51].Value = Funciones.CheckInt64(objDatosRequestNuevoBRMS.cliente.cantidadLineaCuotasPendientesAccUltiVenta);
                arrParam[52].Value = Funciones.CheckInt64(objDatosRequestNuevoBRMS.cliente.cantidadMaximaCuotasPendientesAccUltiVenta);
            }
            else
            {
                arrParam[47].Value = null;
                arrParam[48].Value = null;
                arrParam[49].Value = null;
                arrParam[50].Value = null;
                arrParam[51].Value = null;
                arrParam[52].Value = null;
            }

            objLog.CrearArchivolog("[INICIO][InsertarLogNuevoBRMS]", null, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.decisionID]", objDatosRequestNuevoBRMS.decisionID, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.antiguedadDeuda]", objDatosRequestNuevoBRMS.cliente.antiguedadDeuda, null);
            objLog.CrearArchivolog("[strTipoBloqueoBRMS]", strTipoBloqueoBRMS, null);
            objLog.CrearArchivolog("[strTipoLineaBloqueoBRMS]", strTipoLineaBloqueoBRMS, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.cantidadDocumentosDeuda]", objDatosRequestNuevoBRMS.cliente.cantidadDocumentosDeuda, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.comportamientoPago]", objDatosRequestNuevoBRMS.cliente.comportamientoPago, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.flagBloqueos.ToString()]", objDatosRequestNuevoBRMS.cliente.flagBloqueos.ToString(), null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.flagSuspensiones.ToString()]", objDatosRequestNuevoBRMS.cliente.flagSuspensiones.ToString(), null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.montoDeuda]", objDatosRequestNuevoBRMS.cliente.montoDeuda, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.montoDeudaCastigada]", objDatosRequestNuevoBRMS.cliente.montoDeudaCastigada, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.montoDeudaVencida]", objDatosRequestNuevoBRMS.cliente.montoDeudaVencida, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.montoTotalPago]", objDatosRequestNuevoBRMS.cliente.montoTotalPago, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.promedioFacturadoSoles]", objDatosRequestNuevoBRMS.cliente.promedioFacturadoSoles, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.segmento]", objDatosRequestNuevoBRMS.cliente.segmento, null);
            objLog.CrearArchivolog("[strTipoSusBRMS]", strTipoSusBRMS, null);
            objLog.CrearArchivolog("[strTipoLineaSusBRMS]", strTipoLineaSusBRMS, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.tiempoPermanencia]", objDatosRequestNuevoBRMS.cliente.tiempoPermanencia, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.disputa.antiguedad]", objDatosRequestNuevoBRMS.cliente.disputa.antiguedad, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.disputa.cantidad]", objDatosRequestNuevoBRMS.cliente.disputa.cantidad, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.disputa.monto]", objDatosRequestNuevoBRMS.cliente.disputa.monto, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.documento.tipo]", objDatosRequestNuevoBRMS.cliente.documento.tipo, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.cliente.documento.numero]", objDatosRequestNuevoBRMS.cliente.documento.numero, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.puntoDeVenta.canal]", objDatosRequestNuevoBRMS.puntoDeVenta.canal, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.puntoDeVenta.codigo]", objDatosRequestNuevoBRMS.puntoDeVenta.codigo, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.puntoDeVenta.departamento]", objDatosRequestNuevoBRMS.puntoDeVenta.departamento, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.puntoDeVenta.nombre]", objDatosRequestNuevoBRMS.puntoDeVenta.nombre, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.puntoDeVenta.region]", objDatosRequestNuevoBRMS.puntoDeVenta.region, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.puntoDeVenta.segmento]", objDatosRequestNuevoBRMS.puntoDeVenta.segmento, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.puntoDeVenta.vendedor.codigo]", objDatosRequestNuevoBRMS.puntoDeVenta.vendedor.codigo, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.puntoDeVenta.vendedor.nombre]", objDatosRequestNuevoBRMS.puntoDeVenta.vendedor.nombre, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.sistemaEvaluacion]", objDatosRequestNuevoBRMS.sistemaEvaluacion, null);
            objLog.CrearArchivolog("[objDatosRequestNuevoBRMS.tipoOperacion]", objDatosRequestNuevoBRMS.tipoOperacion, null);
            objLog.CrearArchivolog("[objResponse.decisionID]", objResponse.decisionID, null);
            objLog.CrearArchivolog("[objResponse.validacionCliente.ToString()]", objResponse.validacionCliente.ToString(), null);
            objLog.CrearArchivolog("[objResponse.mensajeValidacionCliente]", objResponse.mensajeValidacionCliente, null);
            objLog.CrearArchivolog("[strResProComercial]", strResProComercial, null);
            objLog.CrearArchivolog("[strResProducto]", strResProducto, null);
            objLog.CrearArchivolog("[strResTipoOperacion]", strResTipoOperacion, null);
            objLog.CrearArchivolog("[intFlagErrorBRMS]", intFlagErrorBRMS, null);
            objLog.CrearArchivolog("[strMensajeErrorBRMS]", strMensajeErrorBRMS, null);
            objLog.CrearArchivolog("[strFlagWhilist]", strFlagWhilist, null);
            objLog.CrearArchivolog("[strFlagTieneDeuda]", strFlagTieneDeuda, null);
            objLog.CrearArchivolog("[strFlagWhilist]", strFlagWhilist, null);
            objLog.CrearArchivolog("[strCodUsuario]", strCodUsuario, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SISACTSI_DATOS_NVO_BRMS";
            obRequest.Parameters.AddRange(arrParam);
            obRequest.Transactional = true;
            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
                boolRegistro = true;
            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                objLog.CrearArchivolog("[ERROR][InsertarLogNuevoBRMS]", null, null);
                throw ex;
            }
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
                objLog.CrearArchivolog("[FIN][InsertarLogNuevoBRMS]", null, null);
            }
            return boolRegistro;
        }
        //PROY-32439 - CAMBIOS_LOG FIN

        //PROY-FULLCLARO ::INICIO
        public static bool InsertarBonoCabecera_FullClaro(Int64 nroSEC, string beneficio, bool flagPorta, string usuario, string tipoDoc, string numDoc, string codProd, string desEst, string strPorta, ref string strCodRpta, ref string strMsjRpta)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("PI_BONO_NROSEC", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_TIPODOC", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_NUMDOC", DbType.String, 16, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_PRODCODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_ESTADOSEC", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_ETDSC_SEC", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_FLAGPORTA", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_BENEFICIO", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_USUARIO", DbType.String, 15, ParameterDirection.Input),
				new DAABRequest.Parameter("PO_CODERROR", DbType.String, 2, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_MSJERROR", DbType.String, 100, ParameterDirection.Output)};

            GeneradorLog objLog = new GeneradorLog(null, numDoc, null, "DATA_LOG");
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = tipoDoc;
            i++; arrParam[i].Value = numDoc;
            i++; arrParam[i].Value = codProd;
            i++; arrParam[i].Value = "1";
            i++; arrParam[i].Value = desEst;
            i++; arrParam[i].Value = (flagPorta) ? strPorta : string.Empty;
            i++; arrParam[i].Value = beneficio;
            i++; arrParam[i].Value = usuario;

            objLog.CrearArchivolog("[Inicio][InsertarBonoCabecera_FullClaro]", null, null);
            objLog.CrearArchivolog("[InsertarBonoCabecera_FullClaro][SEC]", Funciones.CheckStr(nroSEC), null);
            objLog.CrearArchivolog("[InsertarBonoCabecera_FullClaro][TIPO DOCUMENTO]", Funciones.CheckStr(tipoDoc), null);
            objLog.CrearArchivolog("[InsertarBonoCabecera_FullClaro][NRO DOCUMENTO]", Funciones.CheckStr(numDoc), null);
            objLog.CrearArchivolog("[InsertarBonoCabecera_FullClaro][TIPO PRODUCTO]", Funciones.CheckStr(codProd), null);
            objLog.CrearArchivolog("[InsertarBonoCabecera_FullClaro][CODIGO ESTADO]", "1", null);
            objLog.CrearArchivolog("[InsertarBonoCabecera_FullClaro][DESC ESTADO]", Funciones.CheckStr(desEst), null);
            objLog.CrearArchivolog("[InsertarBonoCabecera_FullClaro][FLAG PORTA]", strPorta, null);
            objLog.CrearArchivolog("[InsertarBonoCabecera_FullClaro][BENEFICIO]", Funciones.CheckStr(beneficio), null);
            objLog.CrearArchivolog("[InsertarBonoCabecera_FullClaro][USUARIO]", Funciones.CheckStr(usuario), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), Funciones.CheckStr(nroSEC));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISASI_REC_BONO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
                strCodRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[9]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[10]).Value);
                if (strCodRpta == "0") salida = true;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][InsertarBonoCabecera_FullClaro]", null, ex);
                objRequest.Factory.RollBackTransaction();
                strCodRpta = string.Empty;
                strMsjRpta = string.Empty;
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][InsertarBonoCabecera_FullClaro]", null, null);
            return salida;
        }

        public static bool InsertarBonoDetalle_FullClaro(Int64 nroSEC, string nroDoc, string plan, string linea, string usuario, ref string strCodRpta, ref string strMsjRpta)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("PI_BONO_NROSEC", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_PLAN", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_LINEA", DbType.String, 16, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_BONO_USUARIO", DbType.String, 15, ParameterDirection.Input),
				new DAABRequest.Parameter("PO_CODERROR", DbType.String, 2, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_MSJERROR", DbType.String, 100, ParameterDirection.Output)};

            GeneradorLog objLog = new GeneradorLog(null, nroDoc, null, "DATA_LOG");
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = Funciones.CheckStr(plan);
            i++; arrParam[i].Value = Funciones.CheckStr(linea);
            i++; arrParam[i].Value = usuario;

            objLog.CrearArchivolog("[Inicio][InsertarBonoDetalle_FullClaro]", null, null);
            objLog.CrearArchivolog("[InsertarBonoDetalle_FullClaro][SEC]", Funciones.CheckStr(nroSEC), null);
            objLog.CrearArchivolog("[InsertarBonoDetalle_FullClaro][PLAN]", Funciones.CheckStr(plan), null);
            objLog.CrearArchivolog("[InsertarBonoDetalle_FullClaro][LINEA]", Funciones.CheckStr(linea), null);
            objLog.CrearArchivolog("[InsertarBonoDetalle_FullClaro][USUARIO]", Funciones.CheckStr(usuario), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), Funciones.CheckStr(nroSEC));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISASI_RED_BONO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
                strCodRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[4]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[5]).Value);
                if (strCodRpta == "0") salida = true;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][InsertarBonoDetalle_FullClaro]", null, ex);
                objRequest.Factory.RollBackTransaction();
                strCodRpta = string.Empty;
                strMsjRpta = string.Empty;
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][InsertarBonoDetalle_FullClaro]", null, null);
            return salida;
        }
        //PROY-FULLCLARO ::FIN

        //INC-SMS_PORTA_INI
        public static bool RegistrarTrazabilidadPinSMSPorta(string strTipoDocumento, string strNumeroDocumento, string strLinea, string strUserCreacion, string strNodoSisact, ref string strCodRpta, ref string strMsjRpta, ref Int64 intCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("PI_SMSPC_TDOCCLIENTE", DbType.String,2, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_SMSPV_NUMDOCCLIENTE", DbType.String, 20, ParameterDirection.Input),
				                            new DAABRequest.Parameter("PI_SMSPV_LINEA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_SMSPV_USUCRE", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_SMSPV_NODO", DbType.String, 30, ParameterDirection.Input),
                                            new DAABRequest.Parameter("PO_CORRELATIVO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_COD_RPTA", DbType.String, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_MSJ_RPTA", DbType.String, ParameterDirection.Output)};


            GeneradorLog objLog = new GeneradorLog(null, strNumeroDocumento, null, "DATA_LOG");
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = strTipoDocumento;
            i++; arrParam[i].Value = strNumeroDocumento;
            i++; arrParam[i].Value = strLinea;
            i++; arrParam[i].Value = strUserCreacion;
            i++; arrParam[i].Value = strNodoSisact;

            objLog.CrearArchivolog("[Inicio][RegistrarTrazabilidadPinSMSPorta]", null, null);
            objLog.CrearArchivolog("[RegistrarTrazabilidadPinSMSPorta][strTipoDocumento]", strTipoDocumento, null);
            objLog.CrearArchivolog("[RegistrarTrazabilidadPinSMSPorta][strNumeroDocumento]", strNumeroDocumento, null);
            objLog.CrearArchivolog("[RegistrarTrazabilidadPinSMSPorta][strLinea]", strLinea, null);
            objLog.CrearArchivolog("[RegistrarTrazabilidadPinSMSPorta][strUserCreacion]", strUserCreacion, null);
            objLog.CrearArchivolog("[RegistrarTrazabilidadPinSMSPorta][strNodoSisact]", strNodoSisact, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SMS_PORTABILIDADES + ".SISACTI_SMS_PORTABILIDADES";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
                intCodigo = Funciones.CheckInt64(((IDataParameter)objRequest.Parameters[5]).Value);
                strCodRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[6]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[7]).Value);
                if (strCodRpta == "0") salida = true;

                objLog.CrearArchivolog("[RegistrarTrazabilidadPinSMSPorta][strCodRpta]", strCodRpta, null);
                objLog.CrearArchivolog("[RegistrarTrazabilidadPinSMSPorta][strMsjRpta]", strMsjRpta, null);
                objLog.CrearArchivolog("[RegistrarTrazabilidadPinSMSPorta][strMsjRpta]", intCodigo, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][RegistrarTrazabilidadPinSMSPorta]", null, ex);
                objRequest.Factory.RollBackTransaction();
                strCodRpta = "-99";
                strMsjRpta = String.Format("{0}|{1}",Funciones.CheckStr(ex.Message),Funciones.CheckStr(ex.StackTrace));
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][RegistrarTrazabilidadPinSMSPorta]", null, null);
            return salida;
        }

        public static bool ActualizarTrazabilidadPinSMSPorta(string strTipoDocumento, string strNumeroDocumento, string strLinea, Int64 intCodigo, ref string strCodRpta, ref string strMsjRpta)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("PI_SMSPC_TDOCCLIENTE", DbType.String,2, ParameterDirection.Input),
				new DAABRequest.Parameter("PI_SMSPV_NUMDOCCLIENTE", DbType.String, 20, ParameterDirection.Input),
				                            new DAABRequest.Parameter("PI_SMSPV_LINEA", DbType.String, ParameterDirection.Input),
                                            new DAABRequest.Parameter("PI_SMSPN_CODIGO",DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("PO_COD_RPTA", DbType.String, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_MSJ_RPTA", DbType.String, ParameterDirection.Output)};

            GeneradorLog objLog = new GeneradorLog(null, strNumeroDocumento, null, "DATA_LOG");
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = strTipoDocumento;
            i++; arrParam[i].Value = strNumeroDocumento;
            i++; arrParam[i].Value = strLinea;
            i++; arrParam[i].Value = intCodigo;

            objLog.CrearArchivolog("[Inicio][ActualizarTrazabilidadPinSMSPorta]", null, null);
            objLog.CrearArchivolog("[ActualizarTrazabilidadPinSMSPorta][strTipoDocumento]", strTipoDocumento, null);
            objLog.CrearArchivolog("[ActualizarTrazabilidadPinSMSPorta][strNumeroDocumento]", strNumeroDocumento, null);
            objLog.CrearArchivolog("[ActualizarTrazabilidadPinSMSPorta][strLinea]", strLinea, null);
            objLog.CrearArchivolog("[ActualizarTrazabilidadPinSMSPorta][strCodigo]", Funciones.CheckStr(intCodigo), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SMS_PORTABILIDADES + ".SISACTU_SMS_PORTABILIDADES";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
                strCodRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[4]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[5]).Value);
                if (strCodRpta == "0") salida = true;

                objLog.CrearArchivolog("[ActualizarTrazabilidadPinSMSPorta][strCodRpta]", strCodRpta, null);
                objLog.CrearArchivolog("[ActualizarTrazabilidadPinSMSPorta][strMsjRpta]", strMsjRpta, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ActualizarTrazabilidadPinSMSPorta]", null, ex);
                objRequest.Factory.RollBackTransaction();
                strCodRpta = "-99";
                strMsjRpta = String.Format("{0}|{1}",Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace));
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][ActualizarTrazabilidadPinSMSPorta]", null, null);
            return salida;
        }

        public static string ObtenerTrazabilidadPinSMSPorta(string strTipoDocumento, string strNumeroDocumento, Int64 codigoPorta, ref string strCodRpta, ref string strMsjRpta)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("PI_SMSPC_TDOCCLIENTE", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_SMSPV_NUMDOCCLIENTE", DbType.String, 20, ParameterDirection.Input),
                                            new DAABRequest.Parameter("PI_SMSPN_CODIGO",DbType.Int64,ParameterDirection.Input),
                                            new DAABRequest.Parameter("PO_LINEAS", DbType.String, ParameterDirection.Output),
                new DAABRequest.Parameter("PO_COD_RPTA", DbType.String, ParameterDirection.Output),
                                            new DAABRequest.Parameter("PO_MSJ_RPTA", DbType.String, ParameterDirection.Output)};

            GeneradorLog objLog = new GeneradorLog(null, strNumeroDocumento, null, "DATA_LOG");

            arrParam[0].Value = strTipoDocumento;
            arrParam[1].Value = strNumeroDocumento;
            arrParam[2].Value = codigoPorta;

            objLog.CrearArchivolog("[Inicio][ObtenerRegistrosPinSMSPorta]", null, null);
            objLog.CrearArchivolog("[ObtenerRegistrosPinSMSPorta][strTipoDocumento]", strTipoDocumento, null);
            objLog.CrearArchivolog("[ObtenerRegistrosPinSMSPorta][strNumeroDocumento]", strNumeroDocumento, null);
            objLog.CrearArchivolog("[ObtenerRegistrosPinSMSPorta][codigoPorta]", codigoPorta, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SMS_PORTABILIDADES + ".SISACTS_SMS_PORTABILIDADES";
            objRequest.Parameters.AddRange(arrParam);
            string strLineas = string.Empty;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                strLineas = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[3]).Value);
                strCodRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[4]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[5]).Value);
                objLog.CrearArchivolog("[ObtenerRegistrosPinSMSPorta][strLineas]", strLineas, null);
                objLog.CrearArchivolog("[ObtenerRegistrosPinSMSPorta][strCodRpta]", strCodRpta, null);
                objLog.CrearArchivolog("[ObtenerRegistrosPinSMSPorta][strMsjRpta]", strCodRpta, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ObtenerRegistrosPinSMSPorta][ERROR]",string.Format("{0}|{1}",Funciones.CheckStr(ex.Message),Funciones.CheckStr(ex.StackTrace)), null);
                strCodRpta = "-99";
                strMsjRpta = String.Format("{0}|{1}",Funciones.CheckStr(ex.Message),Funciones.CheckStr(ex.StackTrace));
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }

            objLog.CrearArchivolog("[Fin][ObtenerRegistrosPinSMSPorta]", null, null);
            return strLineas;
        }
        //INC-SMS_PORTA_FIN

        //INICIO PROY-140419 Autorizar Portabilidad sin PIN
        public static bool GrabarValidaSupervisor(string DatosCabeceraValidador, string DatosDetalleValidador, ref string strCodRpta, ref string strMsjRpta) 
        {
            DAABRequest.Parameter[] arrParam ={
                                                  new DAABRequest.Parameter("p_datos_pin_cabecera",DbType.String,10000,ParameterDirection.Input),
                                                  new DAABRequest.Parameter("p_datos_pin_detalle",DbType.String,90000,ParameterDirection.Input),
                                                  new DAABRequest.Parameter("PO_COD_RESULTADO",DbType.String,ParameterDirection.Output),
                                                  new DAABRequest.Parameter("PO_MSJ_RESULTADO",DbType.String,ParameterDirection.Output)
            };

            GeneradorLog objLog = new GeneradorLog(null, "[Evaluación]", null, "DATA_LOG");
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = DatosCabeceraValidador;
            i++; arrParam[i].Value = DatosDetalleValidador;

            objLog.CrearArchivolog("[Inicio][GrabarValidaSupervisor]", null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarValidaSupervisor][DatosCabecera]", Funciones.CheckStr(DatosCabeceraValidador)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarValidaSupervisor][DatosDetalle]", Funciones.CheckStr(DatosDetalleValidador)), null, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objrequest = obj.CreaRequest();
            objrequest.CommandType = CommandType.StoredProcedure;
            objrequest.Command = BaseDatos.PKG_BONOS_FULLCLARO + ".SISACTTSI_AUTORIZA_SINPIN";
            objrequest.Parameters.AddRange(arrParam);
            objrequest.Transactional = true;

            try
            {
                objrequest.Factory.ExecuteNonQuery(ref objrequest);
                objrequest.Factory.CommitTransaction();
                strCodRpta = Funciones.CheckStr(((IDataParameter)objrequest.Parameters[2]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)objrequest.Parameters[3]).Value);
                if (strCodRpta == "0") salida = true;

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarValidaSupervisor][strCodRpta]", strCodRpta), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarValidaSupervisor][strMsjRpta]", strMsjRpta), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[ERROR][GrabarValidaSupervisor]", ex), null, null);
                objrequest.Factory.RollBackTransaction();
                strCodRpta = "-99";
                strMsjRpta = String.Format("{0}|{1}", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace));
            }
            finally 
            {
                objrequest.Parameters.Clear();
                objrequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][GrabarValidaSupervisor]", null, null);
            return salida;
        }

        //FIN PROY-140419 Autorizar Portabilidad sin PIN

        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
        public static Int64 InsertarDatosBRMSValidacionCampanas(BEOfrecimiento oOfrecimiento)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("k_resultado", DbType.Double,ParameterDirection.Output),
				new DAABRequest.Parameter("p_solin_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_slpln_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_prdc_codigo", DbType.String, 2, ParameterDirection.Input),
                                new DAABRequest.Parameter("p_tipoDocCli", DbType.String, 2, ParameterDirection.Input),
                                new DAABRequest.Parameter("p_numDocClie", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_solicitud", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_cliente", DbType.String, 4000, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_direccion_cliente", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_doc_cliente", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_rrll_cliente", DbType.String, 4000, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_equipo", DbType.String, 500, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_oferta", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_campana", DbType.String, 4000, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_plan_actual", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_plan_solicitado", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_servicio", DbType.String, 250, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_pdv", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_in_direccion_pdv", DbType.String, 100, ParameterDirection.Input),
				new DAABRequest.Parameter("p_cantidaddeaplicacionesrenta", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_nrolineasadicionalesruc", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_cantidaddelineasmaximas", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_autonomiarenovacion", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("p_capacidaddepago", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_comportamientoconsolidado", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_comportamientodepagoc1", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_controldeconsumo", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_costodeinstalacion", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_costototalequipos", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_factordeendeudamiento", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_factorderenovacion", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_frecuenciarenta", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_limitedecreditocobranza", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_mesiniciorentas", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("p_montocfpararuc", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_montodegarantia", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_montotopeautomatico", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_preciodeventatotalequipos", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("p_prioridadpublicar", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_exoneracionderentas", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_idvalidator", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_validacioninternaclaro", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_publicar", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_restriccion", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgoenclaro", DbType.String, 25, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgooferta", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgototalequipo", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_riesgototalreplegales", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tipodeautonomiacargofijo", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tipodecobro", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("p_tipodegarantia", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("p_mensajews", DbType.String, 500, ParameterDirection.Input),
                new DAABRequest.Parameter("p_formapago", DbType.String, 20, ParameterDirection.Input),
                                new DAABRequest.Parameter("p_nrocuota", DbType.Int32, ParameterDirection.Input),
                new DAABRequest.Parameter("p_lista_ofrecimientocampanas", DbType.String, 4000, ParameterDirection.Input),
                new DAABRequest.Parameter("p_maximocuotas", DbType.Int64, ParameterDirection.Input),//PROY-140585
                new DAABRequest.Parameter("p_precioequipomaximo", DbType.Double, ParameterDirection.Input),//PROY-140585
                new DAABRequest.Parameter("p_mostrarmensaje", DbType.String, 20, ParameterDirection.Input), //PROY-140585
                new DAABRequest.Parameter("p_motivorestriccion", DbType.String, 150, ParameterDirection.Input),//PROY-140579
                new DAABRequest.Parameter("p_mostrarmotivorestriccion", DbType.String, 2, ParameterDirection.Input)//PROY-140579
			};
            GeneradorLog objLog = new GeneradorLog(null, oOfrecimiento.In_doc_cliente, null, "DATA_LOG");
            int i;
            Int64 idBRMS;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = 0;
            i++; arrParam[i].Value = 0;
            i++; arrParam[i].Value = oOfrecimiento.IdProducto;
            i++; arrParam[i].Value = oOfrecimiento.TipoDocCli;
            i++; arrParam[i].Value = oOfrecimiento.NumDocCli;
            i++; arrParam[i].Value = oOfrecimiento.In_solicitud;
            i++; arrParam[i].Value = oOfrecimiento.In_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_direccion_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_doc_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_rrll_cliente;
            i++; arrParam[i].Value = oOfrecimiento.In_equipo;
            i++; arrParam[i].Value = oOfrecimiento.In_oferta;
            i++; arrParam[i].Value = oOfrecimiento.In_campana;
            i++; arrParam[i].Value = oOfrecimiento.In_plan_actual;
            i++; arrParam[i].Value = oOfrecimiento.In_plan_solicitado;
            i++; arrParam[i].Value = oOfrecimiento.In_servicio;
            i++; arrParam[i].Value = oOfrecimiento.In_pdv;
            i++; arrParam[i].Value = oOfrecimiento.In_direccion_pdv;
            i++; arrParam[i].Value = oOfrecimiento.CantidadDeAplicacionesRenta;
            i++; arrParam[i].Value = oOfrecimiento.CantidadDeLineasAdicionalesRUC;
            i++; arrParam[i].Value = oOfrecimiento.CantidadDeLineasMaximas;
            i++; arrParam[i].Value = oOfrecimiento.AutonomiaRenovacion;
            i++; arrParam[i].Value = oOfrecimiento.CapacidadDePago;
            i++; arrParam[i].Value = oOfrecimiento.ComportamientoConsolidado;
            i++; arrParam[i].Value = oOfrecimiento.ComportamientoDePagoC1;
            i++; arrParam[i].Value = oOfrecimiento.ControlDeConsumo;
            i++; arrParam[i].Value = oOfrecimiento.CostoDeInstalacion;
            i++; arrParam[i].Value = oOfrecimiento.CostoTotalEquipos;
            i++; arrParam[i].Value = oOfrecimiento.FactorDeEndeudamientoCliente;
            i++; arrParam[i].Value = oOfrecimiento.FactorDeRenovacionCliente;
            i++; arrParam[i].Value = oOfrecimiento.FrecuenciaDeAplicacionMensual;
            i++; arrParam[i].Value = oOfrecimiento.LimiteDeCreditoCobranza;
            i++; arrParam[i].Value = oOfrecimiento.MesInicioRentas;
            i++; arrParam[i].Value = oOfrecimiento.MontoCFParaRUC;
            i++; arrParam[i].Value = oOfrecimiento.MontoDeGarantia;
            i++; arrParam[i].Value = oOfrecimiento.MontoTopeAutomatico;
            i++; arrParam[i].Value = oOfrecimiento.PrecioDeVentaTotalEquipos;
            i++; arrParam[i].Value = oOfrecimiento.PrioridadPublicar;
            i++; arrParam[i].Value = oOfrecimiento.ProcesoDeExoneracionDeRentas;
            i++; arrParam[i].Value = oOfrecimiento.ProcesoIDValidator;
            i++; arrParam[i].Value = oOfrecimiento.ProcesoValidacionInternaClaro;
            i++; arrParam[i].Value = oOfrecimiento.Publicar;
            i++; arrParam[i].Value = oOfrecimiento.Restriccion;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoEnClaro;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoOferta;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoTotalEquipo;
            i++; arrParam[i].Value = oOfrecimiento.RiesgoTotalRepLegales;
            i++; arrParam[i].Value = oOfrecimiento.TipoDeAutonomiaCargoFijo;
            i++; arrParam[i].Value = oOfrecimiento.Tipodecobro;
            i++; arrParam[i].Value = oOfrecimiento.TipoDeGarantia;
            i++; arrParam[i].Value = oOfrecimiento.Mensaje;
            i++; arrParam[i].Value = oOfrecimiento.FormaPago;
            i++; arrParam[i].Value = oOfrecimiento.NroCuota;
            i++; arrParam[i].Value = oOfrecimiento.Lista_ofrecimientocampanas;
            i++; arrParam[i].Value = oOfrecimiento.MaximoCuotas;//PROY-140585
            i++; arrParam[i].Value = oOfrecimiento.PrecioEquipoMaximo;//PROY-140585
            i++; arrParam[i].Value = oOfrecimiento.MostrarMensaje;//PROY-140585
            i++; arrParam[i].Value = oOfrecimiento.MotivoDeRestriccion;//PROY-140579
            i++; arrParam[i].Value = oOfrecimiento.MostrarMotivoDeRestriccion;//PROY-140579

            objLog.CrearArchivolog("[Inicio][InsertarDatosBRMSValidacionCampanas]", null, null);
            objLog.CrearArchivolog("[NRODOC]", oOfrecimiento.In_doc_cliente.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), oOfrecimiento.In_doc_cliente.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SISACTSI_CAMPANA_BRMS_SISACT";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
                IDataParameter pSalida1;
                pSalida1 = (IDataParameter)objRequest.Parameters[0];
                idBRMS = Funciones.CheckInt64(pSalida1.Value);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][InsertarDatosBRMSValidacionCampanas]", null, ex);
                objRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][InsertarDatosBRMSValidacionCampanas]", null, null);
            return idBRMS;
        }

        public static DataSet ObtenerDatosBRMSValidacionCampana(Int64 nroSEC) 
        {
            DAABRequest.Parameter[] arrParam = {
                                                 new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC",DbType.Int64,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_EXISTE",DbType.Int64,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_SOLICITUD", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_CLIENTE", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_DIRECCION_CLIENTE", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_DOCUMENTO_CLIENTE", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_RRLL", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_EQUIPOS", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_OFERTA", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_CAMP", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_SERVICIOS", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_PLAN_ACTUAL", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_PLAN_SOLICITADO", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_PUNTO_VENTA", DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("CV_DIRECCION_PDV", DbType.Object,ParameterDirection.Output)
                                             };
            GeneradorLog objLog = new GeneradorLog(null, "[Evaluación]", null, "DATA_LOG");
            int i;
            for (i = 0; i < arrParam.Length; i++) 
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;

            objLog.CrearArchivolog("[Inicio][ObtenerDatosBRMSValidacionCampana]", null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[ObtenerDatosBRMSValidacionCampana][nroSEC]", Funciones.CheckStr(nroSEC)), null, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            string[] sTab = { "CV_SOLICITUD", "CV_CLIENTE", "CV_DIRECCION_CLIENTE", "CV_DOCUMENTO_CLIENTE", "CV_RRLL", "CV_EQUIPOS", "CV_OFERTA", "CV_CAMP", "CV_SERVICIOS", "CV_PLAN_ACTUAL", "CV_PLAN_SOLICITADO", "CV_PUNTO_VENTA", "CV_DIRECCION_PDV" };
            objRequest.TableNames = sTab;
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SISACTSS_VALIDACAMPANA_BRMS";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[ERROR][ObtenerDatosBRMSValidacionCampana]", ex), null, null);
                throw ex;
            }
            finally 
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
                objLog.CrearArchivolog("[Fin][ObtenerDatosBRMSValidacionCampana]", null, null);
            }
        }

        public static bool ActualizarBRMSValidacionCampanas(Int64 nroSEC, Int64 idsBRMS, ref string strCodRpta, ref string strMsjRpta)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("p_solin_codigo", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("p_ids_brmscamp", DbType.Int64, ParameterDirection.Input),
                new DAABRequest.Parameter("po_codigo_respuesta", DbType.String,ParameterDirection.Output),
                new DAABRequest.Parameter("po_mensaje_respuesta", DbType.String,ParameterDirection.Output)};

            GeneradorLog objLog = new GeneradorLog(null, Funciones.CheckStr(nroSEC), null, "DATA_LOG");
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = idsBRMS;

            objLog.CrearArchivolog("[Inicio][ActualizarBRMSValidacionCampanas]", null, null);
            objLog.CrearArchivolog("[ActualizarBRMSValidacionCampanas][nroSEC]", nroSEC, null);
            objLog.CrearArchivolog("[ActualizarBRMSValidacionCampanas][idsBRMS]", idsBRMS, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SISACTSU_VALIDACAMPANA_BRMS";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
                strCodRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[2]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[3]).Value);
                if (strCodRpta == "0") salida = true;

                objLog.CrearArchivolog("[ActualizarBRMSValidacionCampanas][strCodRpta]", strCodRpta, null);
                objLog.CrearArchivolog("[ActualizarBRMSValidacionCampanas][strMsjRpta]", strMsjRpta, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ActualizarBRMSValidacionCampanas]", null, ex);
                objRequest.Factory.RollBackTransaction();
                strCodRpta = "-99";
                strMsjRpta = String.Format("{0}|{1}", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace));
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][ActualizarBRMSValidacionCampanas]", null, null);
            return salida;
        }
        //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

        //PROY-140457-DEBITO AUTOMATICO-INI
        public static List<BEItemGenerico> ListarEntidad(string idSolicitud)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("s_tipo_domiciliacion", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("c_ref_cusor", DbType.Object,ParameterDirection.Output)
	        };

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = idSolicitud;

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_DOMICILIACION + ".sp_getall_entidad";
            objRequest.Parameters.AddRange(arrParam);

            BEItemGenerico objItem;
            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["ENTIDAD"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["DESCRIPCION_LARGA"]);
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

        public static List<BEItemGenerico> ListarCuenta(string idCuenta)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("s_cod_entidad", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("c_ref_cusor", DbType.Object,ParameterDirection.Output)
	        };

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = idCuenta;

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_DOMICILIACION + ".sp_getall_tcuenta";
            objRequest.Parameters.AddRange(arrParam);

            BEItemGenerico objItem;
            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["TIPO_CUENTA"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["DESCRIPCION_LARGA"]);
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

        public static bool GrabarAfiliacionDebitoAutomatico(BEDebitoAutomatico objDebito, string cadenaDetalle, ref string strCodRpta, ref string strMsjRpta)
        {
            DAABRequest.Parameter[] arrParam = { 
                             new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
                             new DAABRequest.Parameter("P_TIPO_DOCUMENTO",DbType.String,2,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_DESCTIPO_DOCUMENTO",DbType.String,16,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_NUMERO_DOCUMENTO",DbType.String,16,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_NOMBRE_CLIENTE",DbType.String,150,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_COD_SOLICITUD",DbType.String,2,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_DESC_SOLICITUD",DbType.String,50,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_CODIGO_ENTIDAD",DbType.String,2,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_DESCRIPCION_ENTIDAD",DbType.String,100,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_TIPO_CUENTA",DbType.String,2,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_DESCRIPCION_CUENTA",DbType.String,100,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_NUMERO_CUENTA",DbType.String,16,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_FECHA_VENCIMIENTO",DbType.String,10,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_FLAG_MONTOTOPE",DbType.String,1,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_MONTO_TOPE",DbType.String,6,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_TIPOCLIENTE",DbType.String,20,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_TELEFONO_CONTACTO",DbType.String,9,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_CORREO_CONTACTO",DbType.String,50,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_CODIGO_CLIENTE",DbType.String,100,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_TIPO_OPERACION",DbType.String,1,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_FLAG_PORTABILIDAD",DbType.String,1,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_CANAL",DbType.String,2,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_CODIGO_PUNTO_VENTA",DbType.String,10,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_PUNTO_VENTA",DbType.String,50,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_DATOS_DETALLE",DbType.String,90000,ParameterDirection.Input),
                             new DAABRequest.Parameter("P_USUARIO_REG",DbType.String,50,ParameterDirection.Input),
                             new DAABRequest.Parameter("PO_COD_RESULTADO",DbType.String,ParameterDirection.Output),
                             new DAABRequest.Parameter("PO_MSJ_RESULTADO",DbType.String,ParameterDirection.Output)
                                               };

            GeneradorLog objLog = new GeneradorLog(null, "[Evaluación]", null, "DATA_LOG");
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = objDebito.Solin_Codigo;
            i++; arrParam[i].Value = objDebito.Tipo_Documento;
            i++; arrParam[i].Value = objDebito.Descripcion_TipoDoc;
            i++; arrParam[i].Value = objDebito.Numero_Documento;
            i++; arrParam[i].Value = objDebito.Nombre_Cliente;
            i++; arrParam[i].Value = objDebito.Cod_Solicitud;
            i++; arrParam[i].Value = objDebito.Desc_Solicitud;
            i++; arrParam[i].Value = objDebito.Codigo_Entidad;
            i++; arrParam[i].Value = objDebito.Descripcion_Entidad;
            i++; arrParam[i].Value = objDebito.Tipo_Cuenta;
            i++; arrParam[i].Value = objDebito.Descripcion_Cuenta;
            i++; arrParam[i].Value = objDebito.Numero_Cuenta;
            i++; arrParam[i].Value = objDebito.Fecha_Vencimiento;
            i++; arrParam[i].Value = objDebito.Flag_MontoTope;
            i++; arrParam[i].Value = objDebito.MontoTope;
            i++; arrParam[i].Value = objDebito.Tipo_Cliente;
            i++; arrParam[i].Value = objDebito.Telefono_Contacto;
            i++; arrParam[i].Value = objDebito.Correo_Contacto;
            i++; arrParam[i].Value = objDebito.Codigo_Cliente;
            i++; arrParam[i].Value = objDebito.Tipo_Operacion;
            i++; arrParam[i].Value = objDebito.Flag_Portabilidad;
            i++; arrParam[i].Value = objDebito.Canal;
            i++; arrParam[i].Value = objDebito.Codigo_PuntoVenta;
            i++; arrParam[i].Value = objDebito.Punto_Venta;
            i++; arrParam[i].Value = cadenaDetalle;
            i++; arrParam[i].Value = objDebito.Usuario;

            objLog.CrearArchivolog("[Inicio][GrabarAfiliacionDebitoAutomatico]", null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_SOLIN_CODIGO]", Funciones.CheckStr(objDebito.Solin_Codigo)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_TIPO_DOCUMENTO]", Funciones.CheckStr(objDebito.Tipo_Documento)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_DESCTIPO_DOCUMENTO]", Funciones.CheckStr(objDebito.Descripcion_TipoDoc)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_NUMERO_DOCUMENTO]", Funciones.CheckStr(objDebito.Numero_Documento)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_NOMBRE_CLIENTE]", Funciones.CheckStr(objDebito.Nombre_Cliente)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_COD_SOLICITUD]", Funciones.CheckStr(objDebito.Cod_Solicitud)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_DESC_SOLICITUD]", Funciones.CheckStr(objDebito.Desc_Solicitud)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_CODIGO_ENTIDAD]", Funciones.CheckStr(objDebito.Codigo_Entidad)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_DESCRIPCION_ENTIDAD]", Funciones.CheckStr(objDebito.Descripcion_Entidad)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_TIPO_CUENTA]", Funciones.CheckStr(objDebito.Tipo_Cuenta)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_DESCRIPCION_CUENTA]", Funciones.CheckStr(objDebito.Descripcion_Cuenta)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_NUMERO_CUENTA]", Funciones.CheckStr(objDebito.Numero_Cuenta)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_FECHA_VENCIMIENTO]", Funciones.CheckStr(objDebito.Fecha_Vencimiento)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_FLAG_MONTOTOPE]", Funciones.CheckStr(objDebito.Flag_MontoTope)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_MONTO_TOPE]", Funciones.CheckStr(objDebito.MontoTope)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_TIPOCLIENTE]", Funciones.CheckStr(objDebito.Tipo_Cliente)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_TELEFONO_CONTACTO]", Funciones.CheckStr(objDebito.Telefono_Contacto)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_CORREO_CONTACTO]", Funciones.CheckStr(objDebito.Correo_Contacto)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_CODIGO_CLIENTE]", Funciones.CheckStr(objDebito.Codigo_Cliente)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_TIPO_OPERACION]", Funciones.CheckStr(objDebito.Tipo_Operacion)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_FLAG_PORTABILIDAD]", Funciones.CheckStr(objDebito.Flag_Portabilidad)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_CANAL]", Funciones.CheckStr(objDebito.Canal)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_CODIGO_PUNTO_VENTA]", Funciones.CheckStr(objDebito.Codigo_PuntoVenta)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_PUNTO_VENTA]", Funciones.CheckStr(objDebito.Punto_Venta)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_DATOS_DETALLE]", Funciones.CheckStr(cadenaDetalle)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][P_USUARIO_REG]", Funciones.CheckStr(objDebito.Usuario)), null, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objrequest = obj.CreaRequest();
            objrequest.CommandType = CommandType.StoredProcedure;
            objrequest.Command = BaseDatos.PKG_DEBITO_AUTOMATICO + ".SISACTSI_DEBITO_AUTOMATICO";
            objrequest.Parameters.AddRange(arrParam);
            objrequest.Transactional = true;

            try
            {
                objrequest.Factory.ExecuteNonQuery(ref objrequest);
                objrequest.Factory.CommitTransaction();
                strCodRpta = Funciones.CheckStr(((IDataParameter)objrequest.Parameters[26]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)objrequest.Parameters[27]).Value);
                if (strCodRpta == "0") salida = true;

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][strCodRpta]", strCodRpta), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[GrabarAfiliacionDebitoAutomatico][strMsjRpta]", strMsjRpta), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}|{2}", "[ERROR][GrabarAfiliacionDebitoAutomatico]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
                objrequest.Factory.RollBackTransaction();
                strCodRpta = "-99";
                strMsjRpta = String.Format("{0}|{1}", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace));
            }
            finally 
            {
                objrequest.Parameters.Clear();
                objrequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][GrabarAfiliacionDebitoAutomatico]", null, null);
            return salida;
        }
        //PROY-140457-DEBITO AUTOMATICO-FIN
        //PROY-140257 INICIO
        public void Consultar_Riesgo_Corp(string TipoDoc, string DescripcionRiesgo, ref string strCodRiesgo, ref string strCodRes, ref string strMsjRespuesta)
        {
            DAABRequest.Parameter[] arrParam ={                                            
            new DAABRequest.Parameter("P_TIPO_DOC",DbType.String,ParameterDirection.Input),
            new DAABRequest.Parameter("P_DESCRIPCION_RIESGO",DbType.String,ParameterDirection.Input),
             new DAABRequest.Parameter("P_COD_RIESGO",DbType.String,ParameterDirection.Output),
             new DAABRequest.Parameter("P_COD_RESULTADO",DbType.String,ParameterDirection.Output),
             new DAABRequest.Parameter("P_MSJ_RESULTADO",DbType.String,ParameterDirection.Output)
            };
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = TipoDoc;
            arrParam[1].Value = DescripcionRiesgo;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SISACTSS_RIESGO";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[2];
                strCodRiesgo = Funciones.CheckStr(parSalida1.Value);

                IDataParameter parSalida2;
                parSalida2 = (IDataParameter)objRequest.Parameters[3];
                strCodRes = Funciones.CheckStr(parSalida2.Value);

                IDataParameter parSalida3;
                parSalida3 = (IDataParameter)objRequest.Parameters[4];
                strMsjRespuesta = Funciones.CheckStr(parSalida3.Value);

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
        //PROY-140257 FIN

        //PROY-140585 F2 - INICIO
        public static bool ActualizarSECSMSPortabilidad(Int64 intCodigoSMSPN, Int64 intNroSEC, ref string strCodRpta, ref string strMsjRpta)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("PI_SMSPN_CODIGO",DbType.Int64,ParameterDirection.Input),
                new DAABRequest.Parameter("PI_NRO_SEC",DbType.Int64,ParameterDirection.Input),
                new DAABRequest.Parameter("PO_COD_RPTA", DbType.String, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_MSJ_RPTA", DbType.String, ParameterDirection.Output)};

            GeneradorLog objLog = new GeneradorLog(null, "[Evaluación]", null, "DATA_LOG");
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = intCodigoSMSPN;
            i++; arrParam[i].Value = intNroSEC;

            objLog.CrearArchivolog("[Inicio][ActualizarSECSMSPortabilidad]", null, null);
            objLog.CrearArchivolog("[ActualizarSECSMSPortabilidad][strTipoDocumento]", intCodigoSMSPN, null);
            objLog.CrearArchivolog("[ActualizarSECSMSPortabilidad][strNumeroDocumento]", intNroSEC, null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SMS_PORTABILIDADES + ".SISACTU_SMSPORTA_NROSEC";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
                strCodRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[2]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[3]).Value);
                if (strCodRpta == "0") salida = true;

                objLog.CrearArchivolog("[ActualizarSECSMSPortabilidad][strCodRpta]", strCodRpta, null);
                objLog.CrearArchivolog("[ActualizarSECSMSPortabilidad][strMsjRpta]", strMsjRpta, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ActualizarSECSMSPortabilidad]", null, ex);
                objRequest.Factory.RollBackTransaction();
                strCodRpta = "-99";
                strMsjRpta = String.Format("{0}|{1}", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace));
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][ActualizarSECSMSPortabilidad]", null, null);
            return salida;
        }       
        //PROY-140585 F2 - FIN


        //PROY-140736- INICIO
        public static int RegistrarBuyBack(int intsopln_codigo, string strcodigocupon, string strimei,string strcodmaterial,string straplicacion,
            string strusuario,string strnodo,ref int intrpta)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("PI_BUYN_SOPLN_CODIGO",DbType.Int64,ParameterDirection.Input),
                new DAABRequest.Parameter("PI_BUYV_CODIGO_CUPON",DbType.String,ParameterDirection.Input),
                new DAABRequest.Parameter("PI_BUYV_IMEI_ENT",DbType.String,ParameterDirection.Input),
                new DAABRequest.Parameter("PI_BUYN_VALOR_CUPON",DbType.String,ParameterDirection.Input),
                new DAABRequest.Parameter("PI_BUYV_CODIGO_MATERIAL",DbType.String,ParameterDirection.Input),
                new DAABRequest.Parameter("PI_BUYV_APLICACION",DbType.String,ParameterDirection.Input),
                new DAABRequest.Parameter("PI_BUYV_USUARIO_REGISTRO",DbType.String,ParameterDirection.Input),
                new DAABRequest.Parameter("PI_BUYV_NODO_EVALUACION",DbType.String,ParameterDirection.Input),
                new DAABRequest.Parameter("PO_RESPUESTACODIGO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_RESPUESTAMENSAJE", DbType.String, ParameterDirection.Output)};

            GeneradorLog objLog = new GeneradorLog(null, "[RegistrarBuyBack]", null, "DATA_LOG");
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = intsopln_codigo;
            i++; arrParam[i].Value = strcodigocupon;
            i++; arrParam[i].Value = strimei;
            i++; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = strcodmaterial;
            i++; arrParam[i].Value = straplicacion;
            i++; arrParam[i].Value = strusuario;
            i++; arrParam[i].Value = strnodo;
       

            objLog.CrearArchivolog("[Inicio][Grabar Buyback]", null, null);
            objLog.CrearArchivolog("[RegistrarBuyBack][intsopln_codigo]", intsopln_codigo, null);
            objLog.CrearArchivolog("[RegistrarBuyBack][strcodigocupon]", strcodigocupon, null);
            objLog.CrearArchivolog("[RegistrarBuyBack][strimei]", strimei, null);
            objLog.CrearArchivolog("[RegistrarBuyBack][strcodmaterial]", strcodmaterial, null);
            objLog.CrearArchivolog("[RegistrarBuyBack][straplicacion]", straplicacion, null);
            objLog.CrearArchivolog("[RegistrarBuyBack][strusuario]", strusuario, null);
            objLog.CrearArchivolog("[RegistrarBuyBack][strnodo]", strnodo, null);


            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_BUYBACK + ".SISACTSI_DET_VENTA_BUYBACK";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                intrpta = Funciones.CheckInt(((IDataParameter)objRequest.Parameters[8]).Value);

                objLog.CrearArchivolog("[GuardarBuyback][intrpta]", intrpta, null);
              
            }
            catch (Exception ex)
            {
                intrpta = -99;
                objLog.CrearArchivolog("[ERROR][GuardarBuyback]", null, ex);
              
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][GuardarBuyback]", null, null);
            return Funciones.CheckInt(intrpta);
        }
        //140736 - FIN

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Consultas BSCS]
        public static List<BEItemGenerico> ConsultaClienteBSCS(string strDesDoc, string strNroDoc, ref string strCodRpta, ref string strMsjRpta)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_NU_DOCUMENTO", DbType.String,ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_DOCUMENTO", DbType.String,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CODIGO_SALIDA",DbType.Int64,ParameterDirection.Output),
                new DAABRequest.Parameter("P_MENSAJE_SALIDA",DbType.String,ParameterDirection.Output),
                new DAABRequest.Parameter("P_CURSOR_SERVICIO", DbType.Object,ParameterDirection.Output)
	        };

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = strNroDoc;
            i++; arrParam[i].Value = strDesDoc;

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strNroDoc);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_CONSULTAS_SIACU + ".SIUNSS_CLIENTE_SERV_SIMPLIF";
            objRequest.Parameters.AddRange(arrParam);

            BEItemGenerico objItem;
            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            DataTable dt = null;

            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];

                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        objItem = new BEItemGenerico();
                        objItem.Codigo = Funciones.CheckStr(dr["CUSTOMER_ID"]);
                        objItem.Codigo2 = Funciones.CheckStr(dr["CO_ID"]);
                        objItem.Codigo3 = Funciones.CheckStr(dr["CUENTA"]);
                        objItem.Descripcion = Funciones.CheckStr(dr["PRODUCTO"]);
                        objLista.Add(objItem);
                    }
                }

                strCodRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[2]).Value);
                strMsjRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[3]).Value);
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


            return objLista;
        }
        #endregion
    }
}
