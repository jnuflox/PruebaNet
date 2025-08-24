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
    public class DAGeneral_II
    {
        GeneradorLog objLog = new GeneradorLog("    DAGeneral_II  ", null, null, "DATA_LOG");

        public DataTable ListarEquipoGama()
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output)
			};
            arrParam[0].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
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

        public ArrayList ListarKitsDTH(string p_tipo_operacion, string p_cod_campania, string p_plazo_acuerdo, string p_plan)
        {
            DAABRequest.Parameter[] arrParam = {
			    new DAABRequest.Parameter("P_TIPO_OPERACION", DbType.String, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CAMPV_CODIGO", DbType.String, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PLZAC_CODIGO", DbType.String, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PLANV_CODIGO", DbType.String, ParameterDirection.Input),
			    new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
		    };

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (p_tipo_operacion != "") { arrParam[i].Value = p_tipo_operacion; }
            ++i; if (p_cod_campania != "") { arrParam[i].Value = p_cod_campania; }
            ++i; if (p_plazo_acuerdo != "") { arrParam[i].Value = p_plazo_acuerdo; }
            ++i; if (p_plan != "") { arrParam[i].Value = p_plan; }

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

        public List<BEItemGenerico> ListarTipoProductoxOferta(string strOferta, string strFlujo, string strCasoEspecial, string strTipoDoc, string strModalidad)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_OFERTA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLUJO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_MODALIDAD_VENTA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)                                                                        
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strOferta;
            arrParam[1].Value = strFlujo;
            arrParam[2].Value = strCasoEspecial;
            arrParam[3].Value = strTipoDoc;
            arrParam[4].Value = strModalidad;

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

        //PROY-24740
        public List<BECasoEspecial> ListarCasoEspecial(string strOferta, string strTipoFlujo, string strTipoOperacion, string strOficina)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_OFERTA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_FLUJO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_OPERACION", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OFICINA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output)                                                                        
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strOferta;
            arrParam[1].Value = strTipoFlujo;
            arrParam[2].Value = strTipoOperacion;
            arrParam[3].Value = strOficina;

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
                    StringBuilder sblDescripcion = new StringBuilder();
                    sblDescripcion.Append(objItem.TCESC_CODIGO);
                    sblDescripcion.Append("_");
                    sblDescripcion.Append((objItem.FLAG_WHITELIST == "1" ? "S" : "N"));
                    sblDescripcion.Append("_");
                    sblDescripcion.Append(objItem.TCEN_MAX_PLANES);
                    objItem.TCESC_DESCRIPCION2 = sblDescripcion.ToString();
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

        public List<BEItemGenerico> ListarPlazoAcuerdo(string strTipoProducto, string strCasoEspecial, string strModalidadVenta)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_TIPO_PRODUCTO", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_MODALIDAD_VENTA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strTipoProducto;
            arrParam[1].Value = strModalidadVenta;
            arrParam[2].Value = strCasoEspecial;

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

        public List<BEItemGenerico> ListarPaquete(string strDocumento, string strOferta, string strPlazo)
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

        public List<BESecServicio_AP> ListarPlanTopeConfig(string pstrPlan, string pstrCasoEspecial)
        {
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

            List<BESecServicio_AP> objLista = new List<BESecServicio_AP>();
            BESecServicio_AP objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BESecServicio_AP();
                    objItem.SERVV_CODIGO = Funciones.CheckStr(dr["SERVICIO"]);
                    objItem.SELECCIONABLE_BASE = Funciones.CheckStr(dr["ESTADO"]);
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

        //PROY-24740
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

        //PROY-24740
        public List<BESecPlan_AP> ListarPlanesXPaquete(string codPaquete)
        {
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
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_CON_DET_BY_PAQUETE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BESecPlan_AP> objLista = new List<BESecPlan_AP>();
            IDataReader dr = null;

            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                int idGen = Funciones.CheckInt(DateTime.Now.ToString("hhmmss")); //Autogenerado

                List<BESecPlan_AP> lisPlanesAP_All = new List<BESecPlan_AP>();
                List<BESecServicio_AP> lisServiciosAP_All = new List<BESecServicio_AP>();

                while (dr.Read())
                {
                    lisPlanesAP_All.Add(new BESecPlan_AP
                {
                        PAQPN_SECUENCIA = Funciones.CheckInt(dr["PAQPN_SECUENCIA"]),
                        PLNV_CODIGO = Funciones.CheckStr(dr["PLNV_CODIGO"]),
                        PLNV_DESCRIPCION = Funciones.CheckStr(dr["PLNV_DESCRIPCION"]),
                        PLNC_ESTADO = Funciones.CheckStr(dr["PLNC_ESTADO"]),
                        TVENC_CODIGO = Funciones.CheckStr(dr["TVENC_CODIGO"]),
                        TPROC_CODIGO = Funciones.CheckStr(dr["TPROC_CODIGO"]),
                        PLNN_TIPO_PLAN = Funciones.CheckInt(dr["PLNN_TIPO_PLAN"]),
                        CARGO_FIJO_BASE = Funciones.CheckInt(dr["PLNN_CARGO_FIJO"]),
                        CARGO_FIJO_EN_PAQUETE = Funciones.CheckInt(dr["PAQPN_CARGO_FIJO"]),
                        PLND_FECHA_CREA = Funciones.CheckDate(dr["PAQPD_FECHA_CREA"]),
                        PLNV_USUARIO_CREA = Funciones.CheckStr(dr["PAQPV_USUARIO_CREA"]),
                        PAQUETE = new BEPaquete_AP()
                        {
                            PAQTV_CODIGO = Funciones.CheckStr(dr["PAQTV_CODIGO"]),
                            PAQTV_DESCRIPCION = Funciones.CheckStr(dr["PAQTV_DESCRIPCION"]),
                            PAQTC_ESTADO = Funciones.CheckStr(dr["PAQTC_ESTADO"]),
                            TPAQTV_CODIGO = Funciones.CheckStr(dr["TPAQTV_CODIGO"]),
                            TPROC_CODIGO = Funciones.CheckStr(dr["TPROC_CODIGO"])
                        },
                        GPLNV_DESCRIPCION = Funciones.CheckStr(dr["GPLNV_DESCRIPCION"]),
                        PLANC_EQUI_SAP = Funciones.CheckStr(dr["PLANC_EQUI_SAP"]),
                        PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"]),
                        CODIGO_BSCS = Funciones.CheckStr(dr["CODIGO_BSCS"]),
                        SOPLN_CODIGO = idGen
                    });
                }

                dr.NextResult();

                while (dr.Read())
                    {
                    lisServiciosAP_All.Add(new BESecServicio_AP()
                        {
                        PAQPN_SECUENCIA = Funciones.CheckInt(dr["PAQPN_SECUENCIA"]),
                        PLNV_CODIGO = Funciones.CheckInt(dr["PLNV_CODIGO"]),
                        SERVV_CODIGO = Funciones.CheckStr(dr["SERVV_CODIGO"]),
                        SERVV_DESCRIPCION = Funciones.CheckStr(dr["SERVV_DESCRIPCION"]),
                        SERVC_ESTADO = Funciones.CheckStr(dr["SERVC_ESTADO"]),
                        GSRVC_CODIGO = Funciones.CheckStr(dr["GSRVC_CODIGO"]),
                        SERVN_ORDEN = Funciones.CheckInt(dr["SERVN_ORDEN"]),
                        SELECCIONABLE_BASE = Funciones.CheckStr(dr["SELECCIONABLE_BASE"]),
                        CARGO_FIJO_BASE = Funciones.CheckInt(dr["CARGO_FIJO_BASE"]),
                        SELECCIONABLE_EN_PAQUETE = Funciones.CheckStr(dr["SELECCIONABLE_EN_PAQUETE"]),
                        CARGO_FIJO_EN_PAQUETE = Funciones.CheckInt(dr["CARGO_FIJO_EN_PAQUETE"]),
                        SERVD_FECHA_CREA = Funciones.CheckDate(dr["SERVD_FECHA_CREA"]),
                        SERVV_USUARIO_CREA = Funciones.CheckStr(dr["SERVV_USUARIO_CREA"])
                    });
                }

                foreach (BESecPlan_AP iResult in lisPlanesAP_All)
                {
                    foreach (BESecServicio_AP iServicio in lisServiciosAP_All)
                    {
                        if (iResult.PAQPN_SECUENCIA == iServicio.PAQPN_SECUENCIA && Funciones.CheckInt(iResult.PLNV_CODIGO) == iServicio.PLNV_CODIGO)
                        {
                            iServicio.PLAN = iResult;
                            iResult.SERVICIOS.Add(iServicio);
                        }
                        }
                    }

                objLista = lisPlanesAP_All;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
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

        //PROY-24740
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

            objLog.CrearArchivolog("    [INICIO][ListarPlanesXPaquete2]   ", null, null);
            objLog.CrearArchivolog("    [Entrada][Paquete]   ", strPaquete.ToString(), null);

            BDSGA obj = new BDSGA(BaseDatos.BdSga);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strPaquete.ToString());
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
                objLog.CrearArchivolog("[ERROR][ListarPlanesXPaquete2]", null, e);
                throw;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[SALIDA][ListarPlanesXPaquete2]", null, null);
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

        //PROY-24740
         public List<BEPlanDetalleHFC> DetalleOferta3Play(Int64 nroSEC, string strTipoProducto)
        {
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
                                                new DAABRequest.Parameter("P_PRDC_CODIGO", DbType.String,5,ParameterDirection.Input),
                                                new DAABRequest.Parameter("P_CURSOR", DbType.Object ,ParameterDirection.Output)};

            arrParam[0].Value = nroSEC;
            arrParam[1].Value = strTipoProducto;

            GeneradorLog objLog = new GeneradorLog(null, nroSEC.ToString(),null,"DATA_LOG");
            objLog.CrearArchivolog("[INICIO][DetalleOferta]", null, null);
            objLog.CrearArchivolog("[SEC]", nroSEC.ToString(), null);
            objLog.CrearArchivolog("[TIPO_PROD]  ", strTipoProducto.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
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
                objLog.CrearArchivolog("[ERROR][DetalleOferta]", null, e);
                throw;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
                objLog.CrearArchivolog("[Fin][DetalleOferta]", null, null);
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

        public bool ValidarDNIVendedor(string pstrDNI)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_DNI", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_VALIDO", DbType.String, ParameterDirection.Output)};
            string strEsValido;
            bool booResultado = false;
            int i;
            i = 0; arrParam[i].Value = pstrDNI;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".MANTSS_VALIDA_VENDEDOR_PP";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                strEsValido = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[1]).Value);

                if (strEsValido == "S")
                    booResultado = true;
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
            return booResultado;
        }
        
        //PROY-24740
        public string ListarCampanasCombo(string pstrComboCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_COMBO", DbType.String, 5, ParameterDirection.Input)                                                                   
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrComboCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_CAMPANA_X_COMBO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            IDataReader dr = null;
            StringBuilder strResultado = new StringBuilder();
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    strResultado.Append("|");
                    strResultado.Append(Funciones.CheckStr(dr["CAMPV_CODIGO"]));
                    strResultado.Append("_");
                    strResultado.Append(Funciones.CheckStr(dr["PRDC_CODIGO"]));
                    strResultado.Append(";");
                    strResultado.Append(Funciones.CheckStr(dr["CAMPV_DESCRIPCION"]));
            }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return strResultado.ToString();
        }

        //PROY-24740
        public string ListarPlazosCombo(string pstrComboCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output), 
				new DAABRequest.Parameter("P_COMBO", DbType.String, 5, ParameterDirection.Input)                                                                  
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrComboCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAZO_X_COMBO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            IDataReader dr = null;
            StringBuilder strResultado = new StringBuilder();
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    strResultado.Append("|");
                    strResultado.Append(Funciones.CheckStr(dr["PLZAC_CODIGO"]));
                    strResultado.Append("_");
                    strResultado.Append(Funciones.CheckStr(dr["PRDC_CODIGO"]));
                    strResultado.Append(";");
                    strResultado.Append(Funciones.CheckStr(dr["PLZAV_DESCRIPCION"]));
            }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return strResultado.ToString();
        }

        public List<BEEquipo> ListarEquipo3Play(string pstrTipoProducto, string pstrPlan)
        {
            List<BEEquipo> filas;
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_PRDC_CODIGO", DbType.String, 2, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_PLAN_CODIGO", DbType.String,ParameterDirection.Input),//INC000004228995
												   new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output)
											   };

            arrParam[0].Value = pstrTipoProducto;
            arrParam[1].Value = pstrPlan;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".MANTSS_LISTAR_MATE_X_PLAN";

            objRequest.Parameters.AddRange(arrParam);

            filas = new List<BEEquipo>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEEquipo oEquipo = new BEEquipo();
                    oEquipo.NRO_SERIE = Funciones.CheckStr(dr["matv_codigo"]);
                    oEquipo.EQUIPO_INFO = Funciones.CheckStr(dr["matv_descripcion"]);
                    oEquipo.PRECIO_VENTA = Funciones.CheckDbl(dr["precio_base"]);
                    oEquipo.TIPO_MATERIAL_ID = Funciones.CheckStr(dr["tmatc_codigo"]);
                    oEquipo.GRUPO_MATERIAL = Funciones.CheckStr(dr["tmatv_descripcion"]);
                    filas.Add(oEquipo);
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

        public List<BEItemGenerico> ListarPlanListaPrecio(string strPlan)
        {
            DAABRequest.Parameter[] arrParam = { 
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_PLAN", DbType.String, 5, ParameterDirection.Input)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (strPlan != string.Empty) arrParam[1].Value = strPlan;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAN_LP";
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
                    objItem.Codigo = Funciones.CheckStr(dr["PLAN"]);
                    objItem.Codigo2 = Funciones.CheckStr(dr["LP"]);
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

        public List<BEPlanDetalleHFC> EquiposOferta3Play(Int64 nroSEC, string strTipoProducto)
        {
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_CURSOR", DbType.Object ,ParameterDirection.Output),
						new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
                                                new DAABRequest.Parameter("P_PRDC_CODIGO", DbType.String,5,ParameterDirection.Input)};

            arrParam[1].Value = nroSEC;
            arrParam[2].Value = strTipoProducto;

            GeneradorLog objLog = new GeneradorLog(null, nroSEC.ToString(),null,"DATA_LOG");
            objLog.CrearArchivolog("[INICIO][EquiposOferta]", null, null);
            objLog.CrearArchivolog("[SEC]", nroSEC.ToString(), null);
            objLog.CrearArchivolog("[TIPO_PROD]", strTipoProducto.ToString(), null);
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_SOLICITUD_EQUIPO";
            objRequest.Parameters.AddRange(arrParam);

            List<BEPlanDetalleHFC> filas = new List<BEPlanDetalleHFC>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEPlanDetalleHFC item = new BEPlanDetalleHFC();
                    item.Agrupa = Funciones.CheckStr(dr["TIPO_MATERIAL"]);
                    item.Servicio = Funciones.CheckStr(dr["equipo"]);
                    item.Precio = Funciones.CheckInt64(dr["cf_alquiler"]);

                    filas.Add(item);
                }

            }
            catch (Exception e)
            {
                objLog.CrearArchivolog("[ERROR][EquiposOferta]", null, e);
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[FIN][EquiposOferta]", null, null);
            return filas;
        }

        public List<BEItemGenerico> ListarCombo(string pstrOficina, string pstrOferta, string pstrTipoOperacion, string pstrTipoFlujo, string pstrTipoDocumento, string pstrModalidad)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output),
                new DAABRequest.Parameter("P_OFICINA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_FLUJO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_DOCUMENTO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_OFERTA", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_OPERACION", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_MODALIDAD_VENTA", DbType.String, 2, ParameterDirection.Input)
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrOficina;
            i++; arrParam[i].Value = pstrTipoFlujo;
            i++; arrParam[i].Value = pstrTipoDocumento;
            i++; arrParam[i].Value = pstrOferta;
            i++; arrParam[i].Value = pstrTipoOperacion;
            i++; arrParam[i].Value = pstrModalidad;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_COMBO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            BEItemGenerico objItem = null;
            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["COMBV_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["COMBV_DESCRIPCION"]);
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
         public List<BEItemGenerico> ListarPaquete3Play(string pstrCombo, string pstrCampana, string pstrPlazo, string pstrIdProducto)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_COMBO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPANA", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CODPRODUCT", DbType.String, 5, ParameterDirection.Input)
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrCombo;
            i++; arrParam[i].Value = pstrCampana;
            i++; arrParam[i].Value = pstrPlazo;
            i++; arrParam[i].Value = pstrIdProducto;

            GeneradorLog objLog = new GeneradorLog(null, pstrCampana.ToString(),null,"DATA_LOG");
            objLog.CrearArchivolog("[INICIO][ListarPaquete]", null, null);
            objLog.CrearArchivolog("[COMBO]", pstrCombo.ToString(), null);
            objLog.CrearArchivolog("[CAMPAÑA]", pstrCampana.ToString(), null);
            objLog.CrearArchivolog("[PLAZO]", pstrPlazo.ToString(), null);
            objLog.CrearArchivolog("[ID_PROD]", pstrIdProducto.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), pstrIdProducto);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PAQUETE_3PLAY";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            BEItemGenerico objItem = null;
            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["PAQTV_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["PAQTV_DESCRIPCION"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ListarPaquete]", null, ex);
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Clear();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][ListarPaquete]", null, null);
            return objLista;
        }

        //PROY-24740
        public List<BEPlan> ListarPlanesXPaquete3Play(string pstrCombo, string pstrCampana, string pstrPlazo, string pstrPaquete, string pstrTipoOperacion, string pstrFlagPorta, string pstrIdProducto)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_COMBO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPANA", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PAQUETE", DbType.String, 5, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_OPERACION", DbType.String, 5, ParameterDirection.Input),
                new DAABRequest.Parameter("P_FLAG_PORTA", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CODPRODUCT", DbType.String, 5, ParameterDirection.Input)
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrCombo;
            i++; arrParam[i].Value = pstrCampana;
            i++; arrParam[i].Value = pstrPlazo;
            i++; arrParam[i].Value = pstrPaquete;
            i++; arrParam[i].Value = pstrTipoOperacion;
            i++; arrParam[i].Value = pstrFlagPorta;
            i++; arrParam[i].Value = pstrIdProducto;

            GeneradorLog objLog = new GeneradorLog(null,pstrCampana.ToString(),null,"DATA_LOG");
            objLog.CrearArchivolog("[INICIO][ListarPlanesXPaquete]", pstrCombo.ToString(), null);
            objLog.CrearArchivolog("[COMBO]   ", pstrCombo.ToString(), null);
            objLog.CrearArchivolog("[CAMPAÑA]", pstrCampana.ToString(), null);
            objLog.CrearArchivolog("[PLAZO]", pstrPlazo.ToString(), null);
            objLog.CrearArchivolog("[PAQUETE]", pstrPaquete.ToString(), null);
            objLog.CrearArchivolog("[TIPO_OPER]", pstrTipoOperacion.ToString(), null);
            objLog.CrearArchivolog("[FLAG_PORTA]", pstrFlagPorta.ToString(), null);
            objLog.CrearArchivolog("[ID_PROD]", pstrIdProducto.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), pstrCombo.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PAQUETE_PLAN_3PLAY";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEPlan> objLista = new List<BEPlan>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while(dr.Read())
                {
                    objLista.Add(new BEPlan()
                {
                        PLANC_CODIGO = Funciones.CheckStr(dr["PLNV_CODIGO"]),
                        PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLNV_DESCRIPCION"]),
                        CODIGO_BSCS = Funciones.CheckStr(dr["PLNV_CODIGO_BSCS"])
                    });                    
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ListarPlanesXPaquete]", null, ex);
                throw;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][ListarPlanesXPaquete]", pstrCombo.ToString(), null);
            return objLista;
        }

        //PROY-24740
        public List<BEServicioHFC> ListarServiciosXPlan3Play(string pstrPlan, string pstrIdProducto)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
                                new DAABRequest.Parameter("P_PLAN", DbType.String, 5, ParameterDirection.Input),
                                new DAABRequest.Parameter("P_CODPRODUCT", DbType.String, 5, ParameterDirection.Input)
                
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrPlan;
            i++; arrParam[i].Value = pstrIdProducto;
            GeneradorLog objLog = new GeneradorLog(null, pstrPlan,null,"DATA_LOG");
            objLog.CrearArchivolog("[Inicio][ListarServiciosXPlan]", null, null);
            objLog.CrearArchivolog("[PLAN]", pstrPlan.ToString(), null);
            objLog.CrearArchivolog("[ID_PROD]", pstrIdProducto.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), pstrIdProducto);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAN_SERV_3PLAY";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            BEServicioHFC objItem = null;
            List<BEServicioHFC> objLista = new List<BEServicioHFC>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while(dr.Read())
                {
                    objItem = new BEServicioHFC();
                    objItem.Grupo = Funciones.CheckInt(dr["GSRVC_CODIGO"]);
                    objItem.GrupoDescripcion = Funciones.CheckStr(dr["GSRVV_DESCRIPCION"]);
                    objItem.IdServicio = Funciones.CheckStr(dr["SERVV_CODIGO"]);
                    objItem.Servicio = Funciones.CheckStr(dr["SERVV_DESCRIPCION"]);
                    objItem.CF_Precio = Funciones.CheckDbl(dr["PSRVN_CARGO_FIJO"]);
                    objItem.FlagPrincipal = Funciones.CheckStr(dr["GSRVC_PRINCIPAL"]);
                    objItem.FlagOpcional = Funciones.CheckStr(dr["PSRVV_SELECCIONABLE"]);
                    objItem.FlagDefecto = Funciones.CheckStr(dr["PSRVC_FLG_DEFECTO"]);
                    objItem.FlagVOD = "0";

                    if (Funciones.CheckStr(dr["SERVV_CODIGO"]) == ConfigurationManager.AppSettings["codigoVOD"])
                        objItem.FlagVOD = "1";

                    objLista.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ListarServiciosXPlan]", null, ex);
                throw ;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }

            objLog.CrearArchivolog("[Fin][ListarServiciosXPlan]", null, null);
            return objLista;
        }
        //gaa20150714
        //public List<BEItemGenerico> ListarCampana(string pstrComboCodigo, string pstrOficina, string pstrOferta, 
        //                                          string strTipoProducto, string pstrCasoEspecial, string pModalidadVenta)
        public List<BEItemGenerico> ListarCampana(string pstrComboCodigo, string pstrOficina, string pstrOferta, 
            string strTipoProducto, string pstrCasoEspecial, string pModalidadVenta, string pstrFlujo, string pstrTipoOperacion)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_COMBO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_OFERTA", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OFICINA", DbType.String, 4, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_PRODUCTO", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, 2, ParameterDirection.Input),                             
                new DAABRequest.Parameter("P_MODALIDAD_VENTA", DbType.String, 2, ParameterDirection.Input)
                //gaa20150714
                ,new DAABRequest.Parameter("P_FLUJO", DbType.String, 3, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_OPERACION", DbType.String, 3, ParameterDirection.Input)
                //fin gaa20150714
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrComboCodigo;
            i++; arrParam[i].Value = pstrOferta;
            i++; arrParam[i].Value = pstrOficina;
            i++; arrParam[i].Value = strTipoProducto;
            i++; arrParam[i].Value = pstrCasoEspecial;
            i++; arrParam[i].Value = pModalidadVenta;
            //gaa20150714
            i++; arrParam[i].Value = pstrFlujo;
            i++; arrParam[i].Value = pstrTipoOperacion;
            //fin gaa20150714
            GeneradorLog objLog = new GeneradorLog(null, pstrComboCodigo.ToString(),null,"DATA_LOG");
            objLog.CrearArchivolog("[Inicio][ListarCampana]", null, null);
            objLog.CrearArchivolog("[COMBO_CODIGO]", pstrComboCodigo.ToString(), null);
            objLog.CrearArchivolog("[OFERTA]", pstrOferta.ToString(), null);
            objLog.CrearArchivolog("[OFICINA]", pstrOficina.ToString(), null);
            objLog.CrearArchivolog("[TIPO_PROD]", strTipoProducto.ToString(), null);
            objLog.CrearArchivolog("[CASO_ESP]", pstrCasoEspecial.ToString(), null);
            objLog.CrearArchivolog("[MODALIDAD_VENT]", pModalidadVenta.ToString(), null);
            objLog.CrearArchivolog("[TIPO_OPER]", pstrTipoOperacion.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), strTipoProducto);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_CAMPANA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            BEItemGenerico objItem = null;
            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            DataTable dt = null;
            try
            {
                //PROY-140245
                DAGeneral daGeneral = new DAGeneral();
                List<BEParametro> lstParametro = new List<BEParametro>();

                //FIN PROY-140245
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["CAMPV_CODIGO"]);
                    objItem.Codigo2 = Funciones.CheckStr(dr["CAMPV_CODIGO_SAP"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["CAMPV_DESCRIPCION"]);
                    objItem.Tipo = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    objLista.Add(objItem);
                }

 //PROY-140245
                lstParametro = daGeneral.ListaParametrosGrupo(Int64.Parse(ConfigurationManager.AppSettings["consGrupoCasoEspecialColab"].ToString()));
                var lstCodigosCasoEspecial = lstParametro.Where(p => p.Valor1 == "1").SingleOrDefault().Valor;
                var lstCodigosCampania = lstParametro.Where(p => p.Valor1 == "2").SingleOrDefault().Valor.Split('|');
                var cantCampaniasEspecial = lstCodigosCampania.Length;
              
                    if (!lstCodigosCasoEspecial.Contains(pstrCasoEspecial) || pstrCasoEspecial == "")
                    {
                        for (int k = 0; k < lstCodigosCampania.Length; k++)
                        {
                            objLista.RemoveAll(x => x.Codigo.ToString().Trim() == lstCodigosCampania[k].ToString().Trim());
                        }

                    }

                //FIN PROY-140245
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ListarCampana]", null, ex);
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Clear();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][ListarCampana]", null, null);
            return objLista;
        }

        public List<BEItemGenerico> ListarPlazo(string pstrTipoProducto, string pstrCasoEspecial, string pstrModalidadVenta)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output), 
				new DAABRequest.Parameter("P_TIPO_PRODUCTO", DbType.String,2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String,2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_MODALIDAD_VENTA", DbType.String, 2, ParameterDirection.Input),                                          
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrTipoProducto;
            i++; arrParam[i].Value = pstrCasoEspecial;
            i++; arrParam[i].Value = pstrModalidadVenta;

            objLog.CrearArchivolog("[INICIO][ListarPlazo]", null, null);
            objLog.CrearArchivolog("[Entrada][pstrTipoProducto]", pstrTipoProducto.ToString(), null);
            objLog.CrearArchivolog("[Entrada][pstrCasoEspecial]", pstrCasoEspecial.ToString(), null);
            objLog.CrearArchivolog("[Entrada][pstrModalidadVenta]", pstrModalidadVenta.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), pstrTipoProducto);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAZO_ACUERDO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem;
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["PLZAC_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["PLZAV_DESCRIPCION"]);
                    objItem.Codigo2 = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ListarPlazo]", null, ex);
            
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Clear();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[SALIDA][ListarPlazo]", null, null);
            return objLista;
        }
        //gaa20161020
        /*public List<BEPlan> ListarPlanTarifario(string strTipoFlujo, string strTipoDocumento, string strTipoOferta, string strTipoOperacion, string strTipoProducto,
                                                string strCasoEspecial, string strCampana, string strPlazo, string strOficina, string strCombo, string strFamilia, ref string filtro)*/
        public List<BEPlan> ListarPlanTarifario(string strTipoFlujo, string strTipoDocumento, string strTipoOferta, string strTipoOperacion, string strTipoProducto,
                                                string strCasoEspecial, string strCampana, string strPlazo, string strOficina, string strCombo, string strFamilia, ref string filtro)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("P_FILTRO", DbType.String, ParameterDirection.Output),
                new DAABRequest.Parameter("P_TIPO_FLUJO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_DOCUMENTO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_OFERTA", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_OPERACION", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_PRODUCTO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CAMPANA", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_PLAZO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_OFICINA", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_COMBO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_FAMILIA", DbType.String, ParameterDirection.Input)
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = strTipoFlujo;
            i++; arrParam[i].Value = strTipoDocumento;
            i++; arrParam[i].Value = strTipoOferta;
            i++; arrParam[i].Value = strTipoOperacion;
            i++; arrParam[i].Value = strTipoProducto;
            i++; arrParam[i].Value = strCasoEspecial;
            i++; arrParam[i].Value = strCampana;
            i++; arrParam[i].Value = strPlazo;
            i++; arrParam[i].Value = strOficina;
            i++; arrParam[i].Value = strCombo;
            //gaa20161020
            i++; arrParam[i].Value = strFamilia;
            //fin gaa20161020
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAN_TARIFARIO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEPlan> objLista = new List<BEPlan>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while(dr.Read())
                {
                    objLista.Add(new BEPlan() {
                        PLANC_CODIGO = Funciones.CheckStr(dr["PLNV_CODIGO"]),
                        PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLNV_DESCRIPCION"]),
                        PLANN_CAR_FIJ = Funciones.CheckDbl(dr["PLNN_CARGO_FIJO"]),
                        PLANC_EQUI_SAP = Funciones.CheckStr(dr["PLNV_CODIGO_SAP"]),
                        PLNN_TIPO_PLAN = Funciones.CheckInt(dr["PLNV_TIPO_PLAN"]),
                        GPLNV_DESCRIPCION = Funciones.CheckStr(dr["GPLNV_DESCRIPCION"]),
                        CODIGO_BSCS = Funciones.CheckStr(dr["PLNV_CODIGO_BSCS"]),
                        TIPO_PRODUCTOS = Funciones.CheckStr(dr["TIPO_PRODUCTOS"]),
                        PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"])
                    });               
                }

                filtro = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[1]).Value);
            }
            finally
            {
                if (dr != null && dr.IsClosed==false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BESecServicio_AP> ListarServiciosXPlan(string pstrTipoProducto, string pstrPlan)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_CURSOR", DbType.Object ,ParameterDirection.Output),
                new DAABRequest.Parameter("P_TIPO_PRODUCTO", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_PLAN_CODIGO", DbType.String, 5, ParameterDirection.Input) 
			}; //INICIATIVA-803
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrTipoProducto;
            i++; arrParam[i].Value = pstrPlan;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAN_X_SERVICIO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            BESecServicio_AP objItem = null;
            List<BESecServicio_AP> objLista = new List<BESecServicio_AP>();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BESecServicio_AP();
                    objItem.SERVV_CODIGO = Funciones.CheckStr(dr["servv_codigo"]);
                    objItem.SERVV_DESCRIPCION = Funciones.CheckStr(dr["servv_descripcion"]);
                    objItem.CARGO_FIJO_BASE = Funciones.CheckDbl(dr["psrvn_cargo_fijo"]);
                    objItem.DESCUENTO_EN_PLAN = 0;
                    objItem.GSRVC_CODIGO = Funciones.CheckStr(dr["gsrvc_codigo"]);
                    objItem.SERVN_ORDEN = Funciones.CheckInt(dr["servn_orden"]);
                    objItem.SELECCIONABLE_BASE = Funciones.CheckStr(dr["psrvv_seleccionable"]);
                    objItem.SELECCIONABLE_EN_PLAN = Funciones.CheckStr(dr["psrvv_seleccionable"]);
                    objItem.GSRVV_DESCRIPCION = Funciones.CheckStr(dr["gsrvv_descripcion"]); //PROY-31812 - IDEA-43340
                    objLista.Add(objItem);
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
            return objLista;
        }

        //PROY-24740
        public List<BEPlan> ListarPlanesCombo(string pstrComboCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_COMBO", DbType.String, 5, ParameterDirection.Input)                                                                     
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrComboCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAN_X_COMBO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEPlan> objLista = new List<BEPlan>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objLista.Add(new BEPlan()
                {
                        PLANC_CODIGO = Funciones.CheckStr(dr["PLNV_CODIGO"]),
                        PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLNV_DESCRIPCION"]),
                        PLANN_CAR_FIJ = Funciones.CheckDbl(dr["PLNN_CARGO_FIJO"]),
                        PLANC_EQUI_SAP = Funciones.CheckStr(dr["PLNV_CODIGO_SAP"]),
                        PLNN_TIPO_PLAN = Funciones.CheckInt(dr["PLNV_TIPO_PLAN"]),
                        GPLNV_DESCRIPCION = Funciones.CheckStr(dr["GPLNV_DESCRIPCION"]),
                        CODIGO_BSCS = Funciones.CheckStr(dr["PLNV_CODIGO_BSCS"]),
                        TIPO_PRODUCTOS = Funciones.CheckStr(dr["TIPO_PRODUCTOS"]),
                        PRDC_CODIGO = Funciones.CheckStr(dr["PLNC_TIPO_PRODUCTO"]),
                        CMBV_CODIGO = Funciones.CheckStr(dr["COMBV_CODIGO"])
                    });
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

        public List<BEDescuento> ListarComboDescuento(string pstrComboCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_COMBO", DbType.String, 5, ParameterDirection.Input)                                                                    
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrComboCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_COMBO_DESCUENTO";
            objRequest.Parameters.AddRange(arrParam);

            BEDescuento objItem = null;
            List<BEDescuento> objLista = new List<BEDescuento>();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEDescuento();
                    objItem.idCombo = Funciones.CheckStr(dr["COMBV_CODIGO"]);
                    objItem.idProducto = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    objItem.tipoDescuento = Funciones.CheckStr(dr["COMBV_TIPO_DSCTO"]);
                    objItem.montoDescuento = Funciones.CheckDbl(dr["COMBN_MONTO_DSCTO"]);
                    objItem.mesesAplicacion = Funciones.CheckInt(dr["COMBN_MESES_DSCTO"]);

                    objLista.Add(objItem);
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
            return objLista;
        }

        //PROY-24740
        public string ListarComboxProducto(string pstrComboCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_COMBO", DbType.String, 5, ParameterDirection.Input)                                                                    
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrComboCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_COMBO_X_PRODUCTO";
            objRequest.Parameters.AddRange(arrParam);

            StringBuilder strResultado = new StringBuilder();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    strResultado.Append("|");
                    strResultado.Append(Funciones.CheckStr(dr["PRDC_CODIGO"]));
                    strResultado.Append("_");
                    strResultado.Append(Funciones.CheckStr(dr["NRO_PLANES"]));
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return strResultado.ToString();
        }

        public List<BEItemGenerico> ListarComboEquipo(string pstrComboCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_COMBO", DbType.String, 5, ParameterDirection.Input)                                                                    
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrComboCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_COMBO_EQUIPO";
            objRequest.Parameters.AddRange(arrParam);

            BEItemGenerico objItem = null;
            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["MATV_CODIGO"]);
                    objLista.Add(objItem);
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
            return objLista;
        }

        public string ObtenerCampanaSap(string strCampana)
        {
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.Text;
            objRequest.Command = string.Format("select campv_codigo_sap from sisact_ap_campana s where s.campv_codigo = '{0}'", strCampana);

            string strCampanaSap;
            try
            {
                strCampanaSap = Funciones.CheckStr(objRequest.Factory.ExecuteScalar(ref objRequest));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return strCampanaSap;
        }

        public List<BEItemGenerico> ListarItemxPDV(int intTipoItem, string strOficina)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_TIPO_ITEM", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_OFICINA", DbType.String, 10, ParameterDirection.Input)   
			};
            arrParam[0].Value = DBNull.Value;
            arrParam[1].Value = intTipoItem;
            arrParam[2].Value = strOficina;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_FILTRO_ITEM_X_PDV";
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
                    objItem.Codigo = Funciones.CheckStr(dr["ITEM"]);
                    objLista.Add(objItem);
                }
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
            return objLista;
        }
        //gaa20161020
        public List<BEItemGenerico> ListarFamiliaPlan(string strModalidad, string strCampana)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output)/*,
				new DAABRequest.Parameter("P_MODALIDAD", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPANA", DbType.String, 4, ParameterDirection.Input),
                new DAABRequest.Parameter("P_FAMILIAPLAN", DbType.String, 4, ParameterDirection.Input)*/
			};
            arrParam[0].Value = DBNull.Value;/*
            arrParam[1].Value = strModalidad;
            arrParam[2].Value = strCampana;
            arrParam[3].Value = DBNull.Value;*/

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL_ + ".SISACSS_FAMILIAPLAN";
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
                    objItem.Codigo = Funciones.CheckStr(dr["FAMILIA_PLAN"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["FAMILIA_DES"]);
                    objLista.Add(objItem);
                }
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
            return objLista;
        }
        //fin gaa20161020

        //PROY-29121-INI

        public List<BEItemGenerico> ListarPlanTelFija(string strProductoCododigo)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("PI_PRDC_CODIGO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("PO_CUR_SALIDA", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.Int32, ParameterDirection.Output),
                new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String, ParameterDirection.Output)
				                                                                    
			};
            int i = 0; arrParam[i].Value = strProductoCododigo;
            i++; arrParam[i].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACTSS_OBT_PLAN_TELFIJA";
            objRequest.Parameters.AddRange(arrParam);

            BEItemGenerico objItem = null;
            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["PLNV_CODIGO"]);
                    objLista.Add(objItem);
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
            return objLista;
        }


        //PROY-29121-FIN

        //PROY_33313 INICIO 
        public void ListarEstadoFlaj(Int64 nroSEC, out string P_RESULTADO, out string P_NRO_RESULTADO, out string P_DES_RESULTADO)
        {
            P_RESULTADO = "";
            P_NRO_RESULTADO = "";
            P_DES_RESULTADO = "";
            DAABRequest.Parameter[] arrParam = {
                                                    new DAABRequest.Parameter("P_SECT_SOLICITUD", DbType.Int64, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("P_RESULTADO", DbType.String, ParameterDirection.Output),
                                                    new DAABRequest.Parameter("P_NRO_RESULTADO", DbType.String, ParameterDirection.Output),
                                                    new DAABRequest.Parameter("P_DES_RESULTADO", DbType.String, ParameterDirection.Output),
                                               };
            for (int i = 0; i < arrParam.Length; i++)
            { arrParam[i].Value = DBNull.Value; }
            arrParam[0].Value = nroSEC;
            var obj = new BDSISACT(BaseDatos.BdSisact);
            var obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_DRA_CVE + ".SISACTSS_CHIP_CUOTA";
            obRequest.Transactional = true;
            obRequest.Parameters.AddRange(arrParam);
            BEItemGenerico fila = new BEItemGenerico();
            IDataReader dr = null;
            dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                if (dr != null)
                {
                    IDataParameter codMaterialOut;
                    codMaterialOut = (IDataParameter)obRequest.Parameters[1];
                    P_RESULTADO = Funciones.CheckStr(codMaterialOut.Value);
                    IDataParameter desMaterialOut;
                    desMaterialOut = (IDataParameter)obRequest.Parameters[2];
                    P_NRO_RESULTADO = Funciones.CheckStr(desMaterialOut.Value);
                    IDataParameter codSerieOut;
                    codSerieOut = (IDataParameter)obRequest.Parameters[3];
                    P_DES_RESULTADO = Funciones.CheckStr(codSerieOut.Value);
                }
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                    obRequest.Parameters.Clear();
                    obRequest.Factory.Dispose();
        }
        //PROY_33313 FIN

        //PROY-140360  inicio
        public void ListarEstadoFlaj2(Int64 nroSEC, out string P_RESULTADO, out string P_NRO_RESULTADO, out string P_DES_RESULTADO)
        {
            P_RESULTADO = "";
            P_NRO_RESULTADO = "";
            P_DES_RESULTADO = "";
            DAABRequest.Parameter[] arrParam = {
                                                    new DAABRequest.Parameter("P_SECT_SOLICITUD", DbType.Int64, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("P_RESULTADO", DbType.String, ParameterDirection.Output),
                                                    new DAABRequest.Parameter("P_NRO_RESULTADO", DbType.String, ParameterDirection.Output),
                                                    new DAABRequest.Parameter("P_DES_RESULTADO", DbType.String, ParameterDirection.Output),
                                               };
            for (int i = 0; i < arrParam.Length; i++)
            { arrParam[i].Value = DBNull.Value; }
            arrParam[0].Value = nroSEC;
            var obj = new BDSISACT(BaseDatos.BdSisact);
            var obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_DRA_CVE + ".SISACTSS_VENTA_CUOTA_UNO"; //PKG_REPLICA 
            obRequest.Transactional = true;
            obRequest.Parameters.AddRange(arrParam);
            BEItemGenerico fila = new BEItemGenerico();
            IDataReader dr = null;
            dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
            if (dr != null)
            {
                IDataParameter codMaterialOut;
                codMaterialOut = (IDataParameter)obRequest.Parameters[1];
                P_RESULTADO = Funciones.CheckStr(codMaterialOut.Value);
                IDataParameter desMaterialOut;
                desMaterialOut = (IDataParameter)obRequest.Parameters[2];
                P_NRO_RESULTADO = Funciones.CheckStr(desMaterialOut.Value);
                IDataParameter codSerieOut;
                codSerieOut = (IDataParameter)obRequest.Parameters[3];
                P_DES_RESULTADO = Funciones.CheckStr(codSerieOut.Value);
            }
            if (dr != null && dr.IsClosed == false)
                dr.Close();
            obRequest.Parameters.Clear();
            obRequest.Factory.Dispose();
        }
        //PROY-140360 fin

        #region INI - PROY-32581
        public List<BEPlan> ListarPlanesXPaqueteLTE(string pstrCombo, string pstrCampana, string pstrPlazo, string pstrPaquete, string pstrTipoOperacion, string pstrFlagPorta, string pstrIdProducto, string pstrGsrvcCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_COMBO", DbType.String, 5, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPANA", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PAQUETE", DbType.String, 5, ParameterDirection.Input),
                new DAABRequest.Parameter("P_TIPO_OPERACION", DbType.String, 5, ParameterDirection.Input),
                new DAABRequest.Parameter("P_FLAG_PORTA", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CODPRODUCT", DbType.String, 5, ParameterDirection.Input),
                new DAABRequest.Parameter("P_GSRVC_CODIGO", DbType.String, 3, ParameterDirection.Input),
			};
            int i = 0; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = pstrCombo;
            i++; arrParam[i].Value = pstrCampana;
            i++; arrParam[i].Value = pstrPlazo;
            i++; arrParam[i].Value = pstrPaquete;
            i++; arrParam[i].Value = pstrTipoOperacion;
            i++; arrParam[i].Value = pstrFlagPorta;
            i++; arrParam[i].Value = pstrIdProducto;
            i++; arrParam[i].Value = pstrGsrvcCodigo;

            GeneradorLog objLog = new GeneradorLog(null, pstrCampana.ToString(), null, "DATA_LOG");
            objLog.CrearArchivolog("[INICIO][ListarPlanesXPaquete-LTE]", pstrCombo.ToString(), null);
            objLog.CrearArchivolog("[COMBO]   ", pstrCombo.ToString(), null);
            objLog.CrearArchivolog("[CAMPAÑA]", pstrCampana.ToString(), null);
            objLog.CrearArchivolog("[PLAZO]", pstrPlazo.ToString(), null);
            objLog.CrearArchivolog("[PAQUETE]", pstrPaquete.ToString(), null);
            objLog.CrearArchivolog("[TIPO_OPER]", pstrTipoOperacion.ToString(), null);
            objLog.CrearArchivolog("[FLAG_PORTA]", pstrFlagPorta.ToString(), null);
            objLog.CrearArchivolog("[ID_PROD]", pstrIdProducto.ToString(), null);
            objLog.CrearArchivolog("[GSRVC_CODIGO]", pstrGsrvcCodigo.ToString(), null);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), pstrCombo.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_CON_PAQUETE_PLAN_LTE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEPlan> objLista = new List<BEPlan>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objLista.Add(new BEPlan()
                    {
                        PLANC_CODIGO = Funciones.CheckStr(dr["PLNV_CODIGO"]),
                        PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLNV_DESCRIPCION"]),
                        CODIGO_BSCS = Funciones.CheckStr(dr["PLNV_CODIGO_BSCS"])
                    });
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][ListarPlanesXPaquete-LTE]", null, ex);
                throw;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][ListarPlanesXPaquete-LTE]", pstrCombo.ToString(), null);
            return objLista;
        }
        #endregion FIN - PROY-32581

        //INI: INICIATIVA-219
        public BEPlan_CBIO ListarPlanesCBIO(string strPO_ID)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("PI_PO_ID", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("PO_CUR_PLAN", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("PO_CUR_SERV", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("PO_CUR_BILL", DbType.Object, ParameterDirection.Output)
				                                                                    
			};
            int i = 0; arrParam[i].Value = strPO_ID;
            i++; arrParam[i].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_GENERAL_CBIO + ".SISACTSS_PLAN_CBIO";
            objRequest.Parameters.AddRange(arrParam);

            BEPlan_CBIO objPlan = new BEPlan_CBIO();
            BEPlan_ServiciosCBIO objServicio = new BEPlan_ServiciosCBIO();
            List<BEPlan_ServiciosCBIO> objListaServicio = new List<BEPlan_ServiciosCBIO>();
            BEPlan_BilleterasCBIO objBilletera = new BEPlan_BilleterasCBIO();
            List<BEPlan_BilleterasCBIO> objListaBilletera = new List<BEPlan_BilleterasCBIO>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    objPlan.PLANC_CODIGO = Funciones.CheckStr(dr["PLANC_CODIGO"]);
                    objPlan.PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLANV_DESCRIPCION"]);
                    objPlan.PLANN_CAR_FIJ = Funciones.CheckDbl(dr["PLANN_CAR_FIJ"]);
                    objPlan.CODIGO_BSCS = Funciones.CheckInt(dr["CODIGO_BSCS"]);
                    objPlan.PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                }

                dr.NextResult();

                while (dr.Read())
                {
                    objServicio.SERVV_CODIGO = Funciones.CheckStr(dr["SERVV_CODIGO"]);
                    objServicio.SERVV_DESCRIPCION = Funciones.CheckStr(dr["SERVV_DESCRIPCION"]);
                    objServicio.GSRVC_CODIGO = Funciones.CheckStr(dr["GSRVC_CODIGO"]);
                    objServicio.SERVN_PRECIO_BASE = Funciones.CheckDbl(dr["SERVN_PRECIO_BASE"]);
                    objServicio.SERVV_ID_BSCS = Funciones.CheckStr(dr["SERVV_ID_BSCS"]);
                    objServicio.S_PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    objServicio.SERVV_DES_BSCS = Funciones.CheckStr(dr["SERVV_DES_BSCS"]);
                    objServicio.SERVV_PO_ID = Funciones.CheckStr(dr["SERVV_PO_ID"]);
                    objListaServicio.Add(objServicio);
                    objPlan.lstServicios = objListaServicio;
                }

                dr.NextResult();

                while (dr.Read())
                {
                    objBilletera.B_PLANC_CODIGO = Funciones.CheckStr(dr["PLANC_CODIGO"]);
                    objBilletera.B_PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    objBilletera.PLNV_PO_ID = Funciones.CheckStr(dr["PLNV_PO_ID"]);
                    objBilletera.PRCLN_CODIGO = Funciones.CheckInt(dr["PRCLN_CODIGO"]);
                    objBilletera.PRCLV_DESCRIPCION = Funciones.CheckStr(dr["PRCLV_DESCRIPCION"]);
                    objListaBilletera.Add(objBilletera);
                    objPlan.lstBilletera = objListaBilletera;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && !(dr.IsClosed)) dr.Close();
                objRequest.Factory.Dispose();
            }
            return objPlan;
        }

        public BEPlan_CBIO ListarDatosPlanesCBIO(string strPO_ID, string estado, string tipCliente)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("PI_PO_ID", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_ESTADO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_TIP_PROD", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("PO_CUR_PLAN", DbType.Object, ParameterDirection.Output),                                                                    
			};
            int i = 0; arrParam[i].Value = strPO_ID;
            i++; arrParam[i].Value = estado;
            i++; arrParam[i].Value = tipCliente; 
            i++; arrParam[i].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_GENERAL_CBIO + ".SISACTSS_OBTENER_DATOS_PLAN";
            objRequest.Parameters.AddRange(arrParam);

            BEPlan_CBIO objPlan = new BEPlan_CBIO();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    objPlan.PLANC_CODIGO = Funciones.CheckStr(dr["PLANC_CODIGO"]);
                    objPlan.PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLANV_DESCRIPCION"]);
                    objPlan.PLANN_CAR_FIJ = Funciones.CheckDbl(dr["PLANN_CAR_FIJ"]);
                    objPlan.CODIGO_BSCS = Funciones.CheckInt(dr["CODIGO_BSCS"]);
                    objPlan.PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    objPlan.ESTADO = Funciones.CheckStr(dr["PLANC_ESTADO"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && !(dr.IsClosed)) dr.Close();
                objRequest.Factory.Dispose();
            }
            return objPlan;
        }

        public BEPlan_CBIO ListarPlanesCBIO_Catalogo(string strPO_ID)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("in_vPOID", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("ou_cCURSOR", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("ou_cCURSOR1", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("ou_nERR_CODE", DbType.Int16, ParameterDirection.Output),
				new DAABRequest.Parameter("ou_vERR_TEXT", DbType.String, ParameterDirection.Output)                                                                   
			};
            int i = 0; arrParam[i].Value = strPO_ID;
            //i++; arrParam[i].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_CBIO_CATALOGO_POSTV + ".CBC_CONS_PLAN_BILL_X_POID";
            objRequest.Parameters.AddRange(arrParam);

            BEPlan_CBIO objPlan = new BEPlan_CBIO();
            BEPlan_BilleterasCBIO objBilletera = new BEPlan_BilleterasCBIO();
            List<BEPlan_BilleterasCBIO> objListaBilletera = new List<BEPlan_BilleterasCBIO>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    objPlan.PLANC_CODIGO = Funciones.CheckStr(dr["CBV_COD_PROD"]);
                    objPlan.PLANV_DESCRIPCION = Funciones.CheckStr(dr["CBV_DESCRIPCION"]);
                    objPlan.PLANN_CAR_FIJ = Funciones.CheckDbl(dr["CBN_CARGOFIJO"]);
                    objPlan.TECNOLOGIA_CODIGO = Funciones.CheckStr(dr["CBV_ID_TECNOLOGIA"]);
                    objPlan.TECNOLOGIA_DESCRIPCION = Funciones.CheckStr(dr["CBV_DESCRIPCION"]);
                }
                dr.NextResult();

                while (dr.Read())
                {
                    objBilletera.B_PLANC_CODIGO = Funciones.CheckStr(dr["CBV_COD_PROD"]);
                    objBilletera.B_PRDC_CODIGO = Funciones.CheckStr(dr["CBV_ID_TECNOLOGIA"]);
                    objBilletera.PLNV_PO_ID = Funciones.CheckStr(dr["CBV_POIDBASICA"]);
                    objBilletera.PRCLN_CODIGO = Funciones.CheckInt(dr["CBV_TIPO_BILLE"]);
                    objBilletera.PRCLV_DESCRIPCION = Funciones.CheckStr(dr["CBV_DESCRIPCION"]);
                    objListaBilletera.Add(objBilletera);
                    objPlan.lstBilletera = objListaBilletera;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && !(dr.IsClosed)) dr.Close();
                objRequest.Factory.Dispose();
            }
            return objPlan;
        }

        public void ObtenerDescripcionTicklers(string strCodigoTicklerCBIO, ref string strDescripcionTickler, ref string strTipoTickler, ref string strCodigoTicklerBSCS, ref string strMsjRespuesta)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_CODIGO_RESTRICCION_CBIO", DbType.String,20,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PO_DESCRIPCION_RESTRICCION", DbType.String,200, ParameterDirection.Output),
												   new DAABRequest.Parameter("PO_TIPO_RESTRICCION", DbType.String,20, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_CODIGO_RESTRICCION_BSCS", DbType.String,20, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String,200, ParameterDirection.Output),
											   };


            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = Funciones.CheckStr(strCodigoTicklerCBIO);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_GENERAL_CBIO + ".SISACTSS_DESCRIPCION_TICKLER";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                strDescripcionTickler = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[1]).Value);
                strTipoTickler = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[2]).Value);
                strCodigoTicklerBSCS = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[3]).Value);
                strMsjRespuesta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[4]).Value);
            }
            catch (Exception ex)
            {
                strDescripcionTickler = string.Empty;
                strTipoTickler = "-1";
                strMsjRespuesta = ex.ToString();
            }
            finally
            {
                if (dr != null && !(dr.IsClosed)) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }

        public void ObtenerDescripcionServiciosAdic(string strCodigoServicio, ref string strDescripcionServicio, ref string strCodigoRespuesta, ref string strMsjRespuesta)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_PO_ID", DbType.String,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PO_PLAN_DESCRIPCION", DbType.String, ParameterDirection.Output),
												   new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String,400,ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String,400,ParameterDirection.Output),
											   };


            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = Funciones.CheckStr(strCodigoServicio);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_GENERAL_CBIO + ".SISACTSS_SERVICIOS_ADICIONALES";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                strDescripcionServicio = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[1]).Value);
                strCodigoRespuesta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[2]).Value);
                strMsjRespuesta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[3]).Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && !(dr.IsClosed)) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }
 //INICIO INC000003048070 

        public void ConsultarCandidatoBono(string strTipoDocumento, string nroDocumento, ref string estadoBonoBSCSFullClaro)
        {
            


            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("PI_TIPODOCUMENTO", DbType.String, 50, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_DOCUMENTO", DbType.String, 50, ParameterDirection.Input),
                new DAABRequest.Parameter("PO_COD_ETIQ",  DbType.Int32, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_CURSOR", DbType.Object,ParameterDirection.Output),
                new DAABRequest.Parameter("PO_CODERROR",DbType.Int32, ParameterDirection.Output),
                new DAABRequest.Parameter("PO_MSJERROR", DbType.String, 200, ParameterDirection.Output)
						};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strTipoDocumento;
            arrParam[1].Value = nroDocumento;

            objLog.CrearArchivolog("[strTipoDocumento - INC000003048070]  ", strTipoDocumento, null);
            objLog.CrearArchivolog("[nroDocumento - INC000003048070]  ", nroDocumento, null);

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_BONOS_FULLCLARO_BSCS + ".bscsss_consu_candidato_bono";
            objRequest.Parameters.AddRange(arrParam);


             try
            {
                
                estadoBonoBSCSFullClaro = "";

                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter p1, p2, p3;
                p1 = (IDataParameter)objRequest.Parameters[2];
                p2 = (IDataParameter)objRequest.Parameters[4];
                p3 = (IDataParameter)objRequest.Parameters[5];

                estadoBonoBSCSFullClaro = Funciones.CheckStr(p1.Value);

                objLog.CrearArchivolog("[estadoBonoBSCSFullClaro - INC000003048070]  ", estadoBonoBSCSFullClaro, null);
                

            }
            catch (Exception e)
            {
                objLog.CrearArchivolog("[ERROR ConsultarCandidatoBono - INC000003048070]", e.Message, null);
                throw e;
            }
            finally
            {
                
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();

            }
            

        }

        //FIN INC000003048070 
        public void ConsultarFlagCBIO(string[] arrParamConsultaCBIO, ref string strCodigoRespuesta, ref string strMensajeRespuesta)
        {
            DAABRequest.Parameter[] arrParam = {
                                                   new DAABRequest.Parameter("PI_SOLIN_CODIGO", DbType.Int64,  ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_ID_CONTRATO", DbType.Int64,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_PDV", DbType.String,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_TIPO_DOCUMENTO", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_NRO_DOCUMENTO", DbType.String,ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_FLUJO", DbType.Int64,ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String,ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String,ParameterDirection.Output),
											   };


            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[2].Value = Funciones.CheckStr(arrParamConsultaCBIO[2]);
            arrParam[3].Value = Funciones.CheckStr(arrParamConsultaCBIO[3]);
            arrParam[4].Value = Funciones.CheckStr(arrParamConsultaCBIO[4]);
            arrParam[5].Value = Funciones.CheckInt64(arrParamConsultaCBIO[5]);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_GENERAL_CBIO + ".SISACTSS_WHITELIST_CBIO";
            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                strCodigoRespuesta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[6]).Value);
                strMensajeRespuesta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[7]).Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && !(dr.IsClosed)) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }

        public BEItemGenerico ListarTopeAutomaticoCBIO(string strPlanCodigo, int idConsulta)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("TMCODE", DbType.Double,ParameterDirection.InputOutput),
				new DAABRequest.Parameter("PO_ID", DbType.String, ParameterDirection.InputOutput),
				new DAABRequest.Parameter("PO_DESCRIPCION", DbType.String, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_CARGOFIJO", DbType.Decimal, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_MONTO_TOPE", DbType.Decimal,ParameterDirection.Output),
                new DAABRequest.Parameter("PO_PLANBASE", DbType.String,ParameterDirection.Output),
				new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String, ParameterDirection.Output),
				new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String, ParameterDirection.Output)

			};
            
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;
            int indice = (idConsulta == 0) ? 0 : 1;//idConsulta = 0: consulta por TMCODE, idConsulta = 1: consulta por POID

            arrParam[indice].Value = strPlanCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_GENERAL_CBIO + ".SISACTSS_DATOSPLAN";
            objRequest.Parameters.AddRange(arrParam);
            
            BEItemGenerico objItem;
            IDataReader dr = null;
            try
            {                
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
               
                objItem = new BEItemGenerico();
                objItem.Valor = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[0]).Value);//CAMBIO 29032020
                objItem.Valor1 = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[1]).Value);
                objItem.Descripcion = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[2]).Value);
                objItem.CargoFijo = Funciones.CheckDbl(((IDataParameter)objRequest.Parameters[3]).Value);
                objItem.Monto = Funciones.CheckDbl(((IDataParameter)objRequest.Parameters[4]).Value);
                objItem.Codigo = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[5]).Value);
                objItem.Codigo2 = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[6]).Value);
                objItem.Descripcion2 = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[7]).Value);
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
            return objItem;
        }

        public BEPlan ObtenerPlanBSCS(string strtipoServ, string strcodPlan, ref string codRespuesta, ref string msjRespuesta)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("PI_PLANSERVICIOSV_CODIGO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_TIPO_SERVICIO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("K_SALIDA", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("PO_COD_RESULTADO", DbType.String, ParameterDirection.Output),
                new DAABRequest.Parameter("PO_MSJ_RESULTADO", DbType.String, ParameterDirection.Output)
				                                                                    
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strcodPlan;
            arrParam[1].Value = strtipoServ;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_BONOS_FULLCLARO + ".SISACTSS_PLANBSCS";
            objRequest.Parameters.AddRange(arrParam);

            BEPlan objPlan = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    objPlan = new BEPlan();
                    objPlan.PLANC_CODIGO = Funciones.CheckStr(dr["PLANC_CODIGO"]);
                    objPlan.CODIGO_BSCS = Funciones.CheckStr(dr["CODIGO_BSCS"]);
                    objPlan.PLANV_DESCRIPCION = Funciones.CheckStr(dr["PLANV_DESCRIPCION"]);
                }
                codRespuesta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[3]).Value);
                msjRespuesta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[4]).Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && !(dr.IsClosed)) dr.Close();
                objRequest.Factory.Dispose();
            }
            return objPlan;
        }

        public Boolean ValidaPlanFullClaro(string strPoBasic, ref string codPlanRespuesta, ref string codRespuesta, ref string msjRespuesta)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("in_vPOBASIC", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("ou_FULLCLARO", DbType.String, ParameterDirection.Output),
				new DAABRequest.Parameter("ou_cCURSOR", DbType.Object, ParameterDirection.Output),
                new DAABRequest.Parameter("ou_nERR_CODE", DbType.Int32, ParameterDirection.Output),
                new DAABRequest.Parameter("ou_vERR_TEXT", DbType.String, ParameterDirection.Output)
				                                                                    
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strPoBasic;

            Boolean resp = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_CBIO_CATALOGO_POSTV + ".CBC_VALIDA_FULLCLARO";
            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                resp = (Funciones.CheckStr(((IDataParameter)objRequest.Parameters[1]).Value) == "S") ? true : false;
                codPlanRespuesta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[1]).Value);
                codRespuesta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[3]).Value);
                msjRespuesta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[4]).Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && !(dr.IsClosed)) dr.Close();
                objRequest.Factory.Dispose();
            }
            return resp;
        }
        //FIN: INICIATIVA-219

    }
}