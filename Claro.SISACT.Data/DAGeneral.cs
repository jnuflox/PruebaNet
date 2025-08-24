using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class DAGeneral
    {
        private static string nameLog = "LogDAGeneral";//INC000004280198

        //PROY-24740
        public List<BETipoDocumento> ListarTipoDocumento()
        {
            DAABRequest.Parameter[] arrParam = { 
                new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
            };

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_GENERAL_CBIO + ".SISACTSS_TIPO_DOCUMENTO"; //INICIATIVA-219
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BETipoDocumento> objLista = new List<BETipoDocumento>();
            BETipoDocumento objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BETipoDocumento();
                    objItem.ID_SISACT = Funciones.CheckStr(dr["DOCC_CODIGO"]) == ReadKeySettings.Key_codigoDocPasaporte08 ? ReadKeySettings.Key_codigoDocPasaporte07 : Funciones.CheckStr(dr["DOCC_CODIGO"]); //PROY-31636
                    objItem.DESCRIPCION = Funciones.CheckStr(dr["DOCV_DESCRIPCION"]);
                    objItem.ID_BSCS = Funciones.CheckInt(dr["DOCC_COD_BSCS"]);
                    objItem.ID_SGA = Funciones.CheckStr(dr["DOCC_COD_SGA"]);
                    objItem.ID_DC = Funciones.CheckStr(dr["DOCC_COD_DC"]);
                    objItem.ID_BSCS_IX = Funciones.CheckStr(dr["DOCC_COD_BSCS_IX"]); // INICIATIVA-219
                    objLista.Add(objItem);
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEItemGenerico> ListarProducto()
        {
            DAABRequest.Parameter[] arrParam = { 
                new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output) 
            };

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PRODUCTO";
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

        public List<BEBilletera> ListarBilletera()
        {
            DAABRequest.Parameter[] arrParam = { 
                new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output) 
            };

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PRODUCTO_CLASE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEBilletera> objLista = new List<BEBilletera>();
            BEBilletera objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEBilletera();
                    objItem.idBilletera = Funciones.CheckInt(dr["PRCLN_CODIGO"]);
                    objItem.billetera = Funciones.CheckStr(dr["PRCLV_DESCRIPCION"]);
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

        public List<BEItemGenerico> ListarTipoGarantia(string strTipoGarantia, string strEstado)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_TCARC_CODIGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TCARC_ESTADO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
			};
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (!string.IsNullOrEmpty(strTipoGarantia)) { arrParam[i].Value = strTipoGarantia; }
            i++; if (!string.IsNullOrEmpty(strEstado)) { arrParam[i].Value = strEstado; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_CON_TIPO_GARANTIA";
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
                    objItem.Codigo = Funciones.CheckStr(dr["TCARC_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["TCARV_DESCRIPCION"]);
                    objItem.Estado = Funciones.CheckStr(dr["TCARC_ESTADO"]);
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

        public List<BETipoGarantia> ListaTipoGarantia(string tipoGarantia, string estado)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_TCARC_CODIGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TCARC_ESTADO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
			};

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (!(string.IsNullOrEmpty(tipoGarantia))) { arrParam[i].Value = tipoGarantia; }
            i++; if (!(string.IsNullOrEmpty(estado))) { arrParam[i].Value = estado; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_CON_TIPO_GARANTIA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BETipoGarantia> objLista = new List<BETipoGarantia>();
            BETipoGarantia objItem;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BETipoGarantia();
                    objItem.TCARC_CODIGO = Funciones.CheckStr(dr["TCARC_CODIGO"]);
                    objItem.TCARV_DESCRIPCION = Funciones.CheckStr(dr["TCARV_DESCRIPCION"]);
                    objItem.TCARC_ESTADO = Funciones.CheckStr(dr["TCARC_ESTADO"]);
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

        //PROY-24740
        public string ListaPrefijosApellidoCompuesto()
        {
            DAABRequest.Parameter[] arrParam = { 
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
            };

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_GENNERAL + ".SISACT_LIST_PREFIJO_APELLIDO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            StringBuilder cadTokens = new StringBuilder();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    cadTokens.Append(",");
                    cadTokens.Append(Funciones.CheckStr(dr["PREFIJOAP"]));
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return cadTokens.Length>0? cadTokens.ToString().Substring(1) : cadTokens.ToString();
        }

        public List<BEItemGenerico> ListarParametroGeneral(string strCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CODIGO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)                                                                        
			};
            arrParam[0].Value = strCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PARAM_GENERAL";
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
                    objItem.Codigo = Funciones.CheckStr(dr["PCONI_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["PCONV_DESCRIPCION"]);
                    objItem.Valor = Funciones.CheckStr(dr["PCONV_VALOR"]);
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

        public List<BEParametro> ListaParametros(Int64 intCodigo)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_PARAN_CODIGO", DbType.Int64,ParameterDirection.Input),												   
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (intCodigo > 0) arrParam[0].Value = intCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTAS + ".SECSS_CON_PARAMETRO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEParametro> objLista = new List<BEParametro>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEParametro objItem = new BEParametro();
                    objItem.Codigo = Funciones.CheckInt64(dr["PARAN_CODIGO"]);
                    objItem.Valor = Funciones.CheckStr(dr["PARAV_VALOR"]);
                    objItem.Valor1 = Funciones.CheckStr(dr["PARAV_VALOR1"]);
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

        public BEUsuario ConsultaDatosUsuario(string strCtaRed)
        {
            DAABRequest.Parameter[] arrParam = {												   
				new DAABRequest.Parameter("P_CTA_RED", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
			};
            arrParam[0].Value = strCtaRed;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_UNI + ".SP_CONSULTA_PDV_USUARIO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            BEUsuario oUsuario = new BEUsuario();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        oUsuario.CtaRed = strCtaRed;
                        oUsuario.UsuarioId = Funciones.CheckInt64(dr["USUAN_CODIGO"]);
                        oUsuario.TipoOficinaId = Funciones.CheckStr(dr["TOFIC_CODIGO"]);
                        oUsuario.OficinaId = Funciones.CheckStr(dr["OVENC_CODIGO"]);
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
            return oUsuario;
        }

        public List<BEPuntoVenta> ConsultaPDVUsuario(Int64 intCodUsuario, string strCodProducto)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("K_USUAN_CODIGO", DbType.Int64, ParameterDirection.Input),												   
				new DAABRequest.Parameter("K_TPROC_CODIGO", DbType.String, ParameterDirection.Input),												   
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (intCodUsuario > 0) arrParam[0].Value = intCodUsuario;
            arrParam[1].Value = strCodProducto;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTAS + ".SECSS_CON_PDV_X_USUARIO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEPuntoVenta> objLista = new List<BEPuntoVenta>();
            BEPuntoVenta objItem = null;
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        objItem = new BEPuntoVenta();
                        objItem.OvencCodigo = Funciones.CheckStr(dr["OVENC_CODIGO"]);
                        objItem.OvenvDescripcion = Funciones.CheckStr(dr["OVENV_DESCRIPCION"]);
                        objItem.ToficCodigo = Funciones.CheckStr(dr["TOFIC_CODIGO"]);
                        objItem.CanacCodigo = Funciones.CheckStr(dr["CANAC_CODIGO"]);
                        objItem.OvencRegion = Funciones.CheckStr(dr["OVENC_REGION"]);
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

        public List<BEItemGenerico> ConsultaTipoOficinaUsuario(Int64 intCodUsuario, string strCodProducto)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("K_USUAN_CODIGO", DbType.Int64,ParameterDirection.Input),												   
				new DAABRequest.Parameter("K_TPROC_CODIGO", DbType.String,ParameterDirection.Input),												   
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (intCodUsuario > 0) arrParam[0].Value = intCodUsuario;
            arrParam[1].Value = strCodProducto;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTAS + ".SECSS_CON_TIPO_OFI_X_USUARIO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        objItem = new BEItemGenerico();
                        objItem.Codigo = Funciones.CheckStr(dr["TOFIC_CODIGO"]);
                        objItem.Descripcion = Funciones.CheckStr(dr["TOFIV_DESCRIPCION"]);
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

        public List<BEItemGenerico> ListarTipoOferta(string strTipoDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output)                                                                        
			};
            arrParam[0].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_TIPO_OFERTA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["TOFC_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["TOFV_DESCRIPCION"]);

                    if (strTipoDocumento != ConfigurationManager.AppSettings["TipoDocumentoRUC"].ToString())
                    {
                        objLista.Add(objItem);
                    }
                    else
                    {
                        if (objItem.Codigo != ConfigurationManager.AppSettings["constCodTipoProductoB2E"].ToString())
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
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEItemGenerico> ListarTipoOperacion()
        {
            DAABRequest.Parameter[] arrParam = { 
                new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output) 
            };

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_TIPO_OPERACION";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["TOFIC_CODIGO"]);
                    objItem.Tipo = Funciones.CheckStr(dr["DOCC_CODIGO"]);
                    objItem.Codigo2 = Funciones.CheckStr(dr["TOPEN_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["TOPEV_DESCRIPCION"]);
                    objLista.Add(objItem);
                }

                //PROY-140743 - INI - HARD
                objItem = new BEItemGenerico();
                objItem.Codigo = "01";
                objItem.Tipo = "01";
                objItem.Codigo2 = "25";
                objItem.Descripcion = "VENTA VARIAS/RAPIDAS";
                objLista.Add(objItem);

                objItem = new BEItemGenerico();
                objItem.Codigo = "01";
                objItem.Tipo = "06";
                objItem.Codigo2 = "25";
                objItem.Descripcion = "VENTA VARIAS/RAPIDAS";
                objLista.Add(objItem);

                objItem = new BEItemGenerico();
                objItem.Codigo = "02";
                objItem.Tipo = "01";
                objItem.Codigo2 = "25";
                objItem.Descripcion = "VENTA VARIAS/RAPIDAS";
                objLista.Add(objItem);

                objItem = new BEItemGenerico();
                objItem.Codigo = "03";
                objItem.Tipo = "01";
                objItem.Codigo2 = "25";
                objItem.Descripcion = "VENTA VARIAS/RAPIDAS";
                objLista.Add(objItem);
                //PROY-140743 - FIN - HARD
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

        public List<BEItemGenerico> ListarPlazoAcuerdo(string strCasoEspecial)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CASO_ESPECIAL", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (!(string.IsNullOrEmpty(strCasoEspecial))) { arrParam[0].Value = strCasoEspecial; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAZO_ACUERDO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
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

        public List<BEItemGenerico> ConsultaLPrecioxPlazo(string strPlazo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_PLAZO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object, ParameterDirection.Output)
			};

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (!(string.IsNullOrEmpty(strPlazo))) { arrParam[i].Value = strPlazo; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_MAESTROS + ".SP_LIST_LPRECIO_X_PLAZO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            DataTable dt = null;
            try
            {  
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        objItem = new BEItemGenerico();
                        objItem.Codigo = Funciones.CheckStr(dr["LPRECIO"]);
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

        public DataTable ListarMotivoDesactivaLinea()
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_ERROR", DbType.String, 2, ParameterDirection.Output),
				new DAABRequest.Parameter("P_MENSAJE", DbType.String, 250, ParameterDirection.Output),
			};

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = "SP_CON_MOTIVO_DESAC_LINEA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return dt;
        }

        public List<BEItemGenerico> ListarComodato()
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output)
			};

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".MANTSS_LISTA_COMODATO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["idservicio"]);
                    objItem.Cantidad = Funciones.CheckInt(dr["cant_equipo"]);
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

        public void ConsultarDatosDireccion(string idDepartamento, string idProvincia, string idDistrito,
                                            ref string strDepartamento, ref string strProvincia, ref string strDistrito)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_CUR_DIR", DbType.Object,ParameterDirection.Output),
				new DAABRequest.Parameter("P_DEPARTAMENTO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PROVINCIA", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DISTRITO", DbType.String, 4, ParameterDirection.Input)
            };

            arrParam[1].Value = idDepartamento;
            arrParam[2].Value = idProvincia;
            arrParam[3].Value = idDistrito;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_BRMS + ".SP_CON_DATOS_DISTRITO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    strDepartamento = Funciones.CheckStr(dr["DEPAV_DESCRIPCION"]);
                    strProvincia = Funciones.CheckStr(dr["PROVV_DESCRIPCION"]);
                    strDistrito = Funciones.CheckStr(dr["DISTV_DESCRIPCION"]);
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
        }


        public List<BEItemGenerico> ListarTopesConsumoHFC(string prod) //PROY-29296
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_PROD", DbType.String, 4 , ParameterDirection.Input),  //PROY-29296
				new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output)
			};

            arrParam[0].Value = prod;  //PROY-29296

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISASS_TOPE_CONSUMO"; //PROY-29296 REPLICA: SP_CON_TOPE_CONSUMO
            objRequest.Parameters.AddRange(arrParam);

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["TPCV_CODIGO_SAP"]);//Funciones.CheckStr(dr["TPCN_CODIGO"]);// "0" + Funciones.CheckStr(dr["TPCV_CODIGO_SAP"]); //PROY-29296
                    objItem.Descripcion = Funciones.CheckStr(dr["TPCV_DESCRIPCION"]); //PROY-29296
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
        
        public List<BEItemGenerico> ListarPlanBase()
        {
            DAABRequest.Parameter[] arrParam = { 
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
			};

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAN_BASE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["PLAN_BASE"]);
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

        public List<BEItemGenerico> ListarPlanCombo()
        {
            DAABRequest.Parameter[] arrParam = { 
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
			};

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAN_COMBO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["PLAN_COMBO"]);
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

        //PROY-24740
        public List<BEPlan> ListarPlanBaseCombo(string strPlanBase)
        {
            DAABRequest.Parameter[] arrParam = { 
				new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("P_PLAN_BASE", DbType.String, 5, ParameterDirection.Input)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(strPlanBase)) arrParam[1].Value = strPlanBase;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_PLAN_BASE_COMBO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEPlan> objLista = new List<BEPlan>();
            BEPlan objItem = null;
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
                    objItem.SERV_CODIGO = Funciones.CheckStr(dr["SERVICIO"]);
                    objItem.SERV_DESCRIPCION = Funciones.CheckStr(dr["SERVICIO_DES"]);
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

        public List<BECuota> ListarTipoCuota()
        {
            DAABRequest.Parameter[] arrParam = { 
                new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
            };

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_TIPO_CUOTA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BECuota> objLista = new List<BECuota>();
            BECuota objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BECuota();
                    objItem.idCuota = Funciones.CheckStr(dr["CUOC_CODIGO"]);
                    objItem.cuota = Funciones.CheckStr(dr["CUOV_DESCRIPCION"]);
                    objItem.nroCuota = Funciones.CheckInt(dr["CUON_VIGENCIA"]);
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

        public List<BEItemGenerico> ListarTipoItem(string strTipoItem)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output),
                new DAABRequest.Parameter("P_TIPO_ITEM", DbType.String, 50, ParameterDirection.Input)
			};
            arrParam[1].Value = strTipoItem;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_ITEM";
            objRequest.Parameters.AddRange(arrParam);

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;

            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["ITEMN_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["ITEMN_DESCRIPCION"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }

            objLista.Reverse(); //INC000001299982
            return objLista;
        }

        public List<BEItemGenerico> ListarListaPrecioxCuota(string strCuota)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_CURSOR", DbType.Object,ParameterDirection.Output),
                new DAABRequest.Parameter("P_CUOTA", DbType.String, 4, ParameterDirection.Input)
			};
            arrParam[1].Value = strCuota;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SP_CON_LPRECIO_CUOTA";
            objRequest.Parameters.AddRange(arrParam);

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;

            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["LPRECIO"]);
                    objItem.Codigo2 = Funciones.CheckStr(dr["CUOC_CODIGO"]);
                    objLista.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }

            return objLista;
        }

        // PROY-26358 - Inicio  - Evalenzs Datos

        public List<BEParametro> ListaParametrosGrupo(Int64 intCodigo)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_PARAN_GRUPO", DbType.Int64,ParameterDirection.Input),												   
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (intCodigo > 0) arrParam[0].Value = intCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTAS + ".SECSS_CON_PARAMETRO_GP";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEParametro> objLista = new List<BEParametro>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEParametro objItem = new BEParametro();
                    objItem.Codigo = Funciones.CheckInt64(dr["PARAN_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["PARAV_DESCRIPCION"]);
                    objItem.Valor = Funciones.CheckStr(dr["PARAV_VALOR"]);
                    objItem.Valor1 = Funciones.CheckStr(dr["PARAV_VALOR1"]);
                    objItem.flagSistema = Funciones.CheckStr(dr["PARAN_FLAG_SISTEMA"]); // PROY 33313
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

        // PROY-26358 - Fin - Evalenzs

	public void ConsultarPrecioListaPrepago(string strCodMaterial, string strCodListaPrecio, ref double dblPrecioPrepago) //PROY-24724-IDEA-28174 - INICIO
        {
            DAABRequest.Parameter[] arrParam = {    new DAABRequest.Parameter("P_MATERIAL", DbType.String, ParameterDirection.Input),
		                                            new DAABRequest.Parameter("P_LISTAPRECIO", DbType.String, ParameterDirection.Input),
				                                    new DAABRequest.Parameter("P_PRECIOPREPAGO", DbType.Double, ParameterDirection.Output)
            };

            arrParam[0].Value = strCodMaterial;
            arrParam[1].Value = strCodListaPrecio;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.SISACT_PKG_NUEVA_LISTAPRE_6 + ".SISACTSI_PRECIO_PREPAGO";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter parSalida;
                parSalida = (IDataParameter)objRequest.Parameters[2];
                dblPrecioPrepago = Funciones.CheckDbl(parSalida.Value);
                objRequest.Factory.Dispose();
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
        } //PROY-24724-IDEA-28174 - FIN

        //INICIO|PROY-140533 - CONSULTA STOCK
        public static BEItemGenerico ConsultarFlagsPicking(string codigo_oficina, ref string codigo_rpta, ref string mensaje_rpta)
        {
            BEItemGenerico filas = null;
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("PI_COD_OFI", DbType.String, ParameterDirection.Input),
                                                new DAABRequest.Parameter("C_CURSOR", DbType.Object, ParameterDirection.Output),
                                                new DAABRequest.Parameter("PO_COD_RESP", DbType.Int64, ParameterDirection.Output),
                                                new DAABRequest.Parameter("PO_MSG_RESP", DbType.String, ParameterDirection.Output)
												};
            arrParam[0].Value = codigo_oficina;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACTSS_OFI_PICKING";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    filas = new BEItemGenerico()
                    {
                        Descripcion = Funciones.CheckStr(dr["DESCRIPCION_OFI"]),
                        Codigo2 = Funciones.CheckStr(dr["FLAG_PDK"]),
                        Codigo3 = Funciones.CheckStr(dr["FLAG_DELIVERY"]),
                    };
                }
                IDataParameter pSalida1;
                IDataParameter pSalida2;
                pSalida1 = (IDataParameter)obRequest.Parameters[2];
                pSalida2 = (IDataParameter)obRequest.Parameters[3];
                codigo_rpta = Funciones.CheckStr(pSalida1.Value);
                mensaje_rpta = Funciones.CheckStr(pSalida2.Value);
            }

            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return filas;
        }
        //FIN|PROY-140533 - CONSULTA STOCK

        //INICIO - IDEA-141897
        public static void consultaBeneficioFullClaro(String strTipoDocumento, String strNumeroDocumento, String strFlagCondicion, out List<BEFullClaroBeneficio> lstFullClaro, out List<BEAcuerdoDetalle> lstPedido, ref string codigo_rpta, ref string mensaje_rpta)
        {
            BEFullClaroBeneficio filas = null;
            BEAcuerdoDetalle filas2 = null;
            lstFullClaro = new List<BEFullClaroBeneficio>();
            lstPedido = new List<BEAcuerdoDetalle>();
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("PI_BONOC_TIPO_DOC", DbType.String, ParameterDirection.Input),
                                                new DAABRequest.Parameter("PI_BONOV_NUM_DOC", DbType.String, ParameterDirection.Input),
                                                new DAABRequest.Parameter("PI_FLAG_CONDICION", DbType.String, ParameterDirection.Input),
                                                new DAABRequest.Parameter("K_SALIDA", DbType.Object, ParameterDirection.Output),
                                                new DAABRequest.Parameter("K_SALIDA_PAGADOS", DbType.Object, ParameterDirection.Output),
                                                new DAABRequest.Parameter("PO_COD_RESULTADO", DbType.String, ParameterDirection.Output),
                                                new DAABRequest.Parameter("PO_MSJ_RESULTADO", DbType.String, ParameterDirection.Output)
												};

            arrParam[0].Value = strTipoDocumento;
            arrParam[1].Value = strNumeroDocumento;
            arrParam[2].Value = strFlagCondicion;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_BONOS_FULLCLARO + ".SISACTSU_CANDIDATOFC_XESTADO";
            obRequest.Parameters.AddRange(arrParam);


            HelperLog.EscribirLog(string.Empty, nameLog, string.Format("{0}{1}","[" + strNumeroDocumento + "]", "[consultaBeneficioFullClaro][INICIO]"), false);//INC000004280198
            HelperLog.EscribirLog(string.Empty, nameLog, string.Format("{0}{1}", "[" + strNumeroDocumento + "]", "[SP : " + obRequest.Command + "]"), false);//INC000004280198
            HelperLog.EscribirLog(string.Empty, nameLog, string.Format("{0}{1}", "[" + strNumeroDocumento + "]", "[PI_BONOC_TIPO_DOC : " + strTipoDocumento + "]"), false);//INC000004280198
            HelperLog.EscribirLog(string.Empty, nameLog, string.Format("{0}{1}", "[" + strNumeroDocumento + "]", "[PI_BONOV_NUM_DOC : " + strNumeroDocumento + "]"), false);//INC000004280198
            HelperLog.EscribirLog(string.Empty, nameLog, string.Format("{0}{1}", "[" + strNumeroDocumento + "]", "[PI_FLAG_CONDICION : " + strFlagCondicion + "]"), false);//INC000004280198


            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    filas = new BEFullClaroBeneficio();

                    if (strFlagCondicion == "SEC-PAGADA")//INC000004280198
                    {

                        filas.NumeroSEC = Funciones.CheckStr(dr["CONTN_NUMERO_SEC"]);//INC000004280198
                        filas.NumeroContrato = Funciones.CheckStr(dr["CONTN_NUMERO_CONTRATO"]);//INC000004280198
                        filas.FechaContrato = Funciones.CheckStr(dr["CONTD_FECHA_CONTRATO"]);//INC000004280198
                        filas.Pedido = Funciones.CheckStr(dr["PEDIN_NROPEDIDO"]);//INC000004280198

                    }
                    else
                    {

                    filas.IdCandidato = Funciones.CheckStr(dr["BONON_IDCANDIDATO"]);
                    filas.TipoDocumento = Funciones.CheckStr(dr["BONOC_TIPO_DOC"]);
                    filas.NumeroDocumento = Funciones.CheckStr(dr["BONOV_NUM_DOC"]);
                    filas.CodigoProducto = Funciones.CheckStr(dr["BONOV_COD_PRODUCTO"]);
                    filas.TipoOperacion = Funciones.CheckStr(dr["BONOV_TIPO_OPERACION"]);
                    filas.Linea = Funciones.CheckStr(dr["BONOV_LINEA"]);
                    filas.EstadoCandidato = Funciones.CheckStr(dr["BONON_ESTADO"]);

                    }


                    lstFullClaro.Add(filas);

                }

                HelperLog.EscribirLog(string.Empty, nameLog, string.Format("{0}{1}", "[" + strNumeroDocumento + "]", "[lstFullClaro.Count : " + lstFullClaro.Count.ToString() + "]"), false);//INC000004280198


                dr.NextResult();

                while (dr.Read())
                {
                    filas2 = new BEAcuerdoDetalle();
                    filas2.Plan_tarifar = Funciones.CheckStr(dr["PLAN_TARIFAR"]);
                    filas2.Plan_tarifar_desc = Funciones.CheckStr(dr["PLAN_TARIFAR_DESC"]);
                    filas2.Telefono = Funciones.CheckStr(dr["TELEFONO"]);
                    lstPedido.Add(filas2);
                }

                HelperLog.EscribirLog(string.Empty, nameLog, string.Format("{0}{1}", "[" + strNumeroDocumento + "]", "[lstPedido.Count : " + lstPedido.Count.ToString() + "]"), false);//INC000004280198


                IDataParameter pSalida1;
                IDataParameter pSalida2;
                pSalida1 = (IDataParameter)obRequest.Parameters[5];
                pSalida2 = (IDataParameter)obRequest.Parameters[6];
                codigo_rpta = Funciones.CheckStr(pSalida1.Value);
                mensaje_rpta = Funciones.CheckStr(pSalida2.Value);
            }

            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }
        //FIN - IDEA-141897

        //IDEA-142010 INICIO
        public int ValidarVigenciaCampana(string strCodigosCampanas,ref string strCodigoRespuesta, ref string strMensajeRespuesta)
        {
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_COD_CAMPANA", DbType.String, ParameterDirection.Input),
                                                new DAABRequest.Parameter("PO_CUR_SALIDA", DbType.Object, ParameterDirection.Output),
                                                new DAABRequest.Parameter("P_CODIGO_RESPUESTA", DbType.String, ParameterDirection.Output),
                                                new DAABRequest.Parameter("P_MENSAJE_RESPUESTA", DbType.String, ParameterDirection.Output)
												};

            arrParam[0].Value = strCodigosCampanas;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_GENNERAL + ".SISACTSS_VIGENCIA_CAMPANA";
            objRequest.Parameters.AddRange(arrParam);

            int countCampanasVigentes = 0;
            BEApCampana datosCampana = null;
            List<BEApCampana> lstDatosCampana = new List<BEApCampana>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    countCampanasVigentes = countCampanasVigentes + 1;
                    datosCampana = new BEApCampana();
                    datosCampana.CAMPV_CODIGO = Funciones.CheckStr(dr["CAMPV_CODIGO"]);
                    datosCampana.CAMPV_DESCRIPCION = Funciones.CheckStr(dr["CAMPV_DESCRIPCION"]);
                    datosCampana.CAMPV_TIPO_PRODUCTO = Funciones.CheckStr(dr["CAMPV_TIPO_PRODUCTO"]);
                    datosCampana.CAMPD_FECHA_INICIO = Funciones.CheckStr(dr["CAMPD_FECHA_INICIO"]);
                    datosCampana.CAMPD_FECHA_FIN = Funciones.CheckStr(dr["CAMPD_FECHA_FIN"]);
                    lstDatosCampana.Add(datosCampana);
                }

                IDataParameter pSalida1;
                IDataParameter pSalida2;
                pSalida1 = (IDataParameter)objRequest.Parameters[2];
                pSalida2 = (IDataParameter)objRequest.Parameters[3];
                strCodigoRespuesta = Funciones.CheckStr(pSalida1.Value);
                strMensajeRespuesta = Funciones.CheckStr(pSalida2.Value);
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return countCampanasVigentes;
        }
        //IDEA-142010 FIN

        //INI IDEA-142717
        public void ValidarCampVacunaton(string strTipoDocumento, string strNroDocumento, ref string strCodigoRespuesta, ref string strMensajeRespuesta)
        {
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("PI_DOC_CLIENTE", DbType.String, ParameterDirection.Input),
                                                new DAABRequest.Parameter("PI_NUM_CLIENTE", DbType.String, ParameterDirection.Input),
                                                new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String, ParameterDirection.Output),
                                                new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String,1000, ParameterDirection.Output)
												};

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strTipoDocumento;
            arrParam[1].Value = strNroDocumento;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACTSS_CAMPANA_VACUNATON";
            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                strCodigoRespuesta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[2]).Value);
                strMensajeRespuesta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[3]).Value);
            }
            catch (Exception ex)
            {
                strCodigoRespuesta = "-99";
                strMensajeRespuesta = Funciones.CheckStr(ex.Message + ex.StackTrace);
            }
            finally
            {
                if (dr != null && !(dr.IsClosed)) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }

        }
        //FIN IDEA-142717

  //PROY 140736 INI
        public List<BEItemGenerico> ListarComboBuyback()
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter( "PO_RESPUESTACODIGO", DbType.Int64,ParameterDirection.Output),		
				 new DAABRequest.Parameter("PO_RESPUESTAMENSAJE", DbType.String,ParameterDirection.Output),								   
				new DAABRequest.Parameter("PO_CURSOR", DbType.Object,ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;


            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_BUYBACK + ".SISACTSS_CATAL_EQUI_LIST_CANJE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["N_CODIGO_MATERIAL"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["V_DESC_MATERIAL"]);
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

        public List<BEItemGenerico> ListarBuyback(long nrosec)
        {
            DAABRequest.Parameter[] arrParam = {   
                 new DAABRequest.Parameter( "PI_NRO_SEC", DbType.Int64,ParameterDirection.Input),
                 new DAABRequest.Parameter( "PO_RESPUESTACODIGO", DbType.Int64,ParameterDirection.Output),		
				 new DAABRequest.Parameter("PO_RESPUESTAMENSAJE", DbType.String,ParameterDirection.Output),								   
				 new DAABRequest.Parameter("PO_CURSOR", DbType.Object,ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;
            arrParam[0].Value = nrosec;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_BUYBACK + ".SISACTSS_LISTA_BUYBACK";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["SOPLN_ORDEN"]);
                    objItem.Codigo2 = Funciones.CheckStr(dr["BUYV_CODIGO_CUPON"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["BUYV_IMEI_ENT"]);
                    objItem.Descripcion2 = Funciones.CheckStr(dr["BUYV_CODIGO_MATERIAL"]);
                    objItem.Valor = Funciones.CheckStr(dr["BUYN_SOPLN_CODIGO"]);
                        
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


        public void EliminarBuyback(string sopln, ref int codigo, ref string mensajerpta)
        {
            DAABRequest.Parameter[] arrParam = {
                 new DAABRequest.Parameter( "PI_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
                 new DAABRequest.Parameter( "PO_RESPUESTACODIGO", DbType.Int64,ParameterDirection.Output),		
				 new DAABRequest.Parameter("PO_RESPUESTAMENSAJE", DbType.String,ParameterDirection.Output),								   
		
            };

            arrParam[0].Value = Funciones.CheckInt64(sopln);
           

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_BUYBACK + ".SISACTSS_DEL_BUYBACK";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            IDataReader dr = null;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
              
                IDataParameter pSalida1;
                IDataParameter pSalida2;
                pSalida1 = (IDataParameter)objRequest.Parameters[1];
                pSalida2 = (IDataParameter)objRequest.Parameters[2];
                codigo = Funciones.CheckInt(pSalida1.Value);
                mensajerpta = Funciones.CheckStr(pSalida2.Value);
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


        public void ValidarBuyback(string strIMEI, string strcupon, ref int sec,ref int codigo, ref string mensajebuyback)
        {
            DAABRequest.Parameter[] arrParam = {
                 new DAABRequest.Parameter( "PI_IMEI", DbType.String,ParameterDirection.Input),
                 new DAABRequest.Parameter( "PI_BUYV_CODIGO_CUPON", DbType.String,ParameterDirection.Input),
                 new DAABRequest.Parameter( "PO_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Output),	
                 new DAABRequest.Parameter( "PO_RESPUESTACODIGO", DbType.Int64,ParameterDirection.Output),		
				 new DAABRequest.Parameter( "PO_RESPUESTAMENSAJE", DbType.String,200,ParameterDirection.Output),								   
		
            };

            arrParam[0].Value = Funciones.CheckStr(strIMEI);
            arrParam[1].Value = Funciones.CheckStr(strcupon);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_BUYBACK + ".SISACTSS_VALIDAR_BUYBACK";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            IDataReader dr = null;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                IDataParameter pSalida1;
                IDataParameter pSalida2;
                IDataParameter pSalida3;
                pSalida1 = (IDataParameter)objRequest.Parameters[2];
                pSalida2 = (IDataParameter)objRequest.Parameters[3];
                pSalida3 = (IDataParameter)objRequest.Parameters[4];
                sec = Funciones.CheckInt(pSalida1.Value);
                codigo = Funciones.CheckInt(pSalida2.Value);
                mensajebuyback = Funciones.CheckStr(pSalida3.Value);
               
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

        public void EliminarBuybackEvalAnt(string strsec, string strusuario, string strtevac, string nrodoc, ref int codigo, ref string mensajebuyback)
        {
            DAABRequest.Parameter[] arrParam = {
                 new DAABRequest.Parameter( "PI_SOLIN_CODIGO", DbType.Int32,ParameterDirection.Input),
                 new DAABRequest.Parameter( "PI_SOLIC_USU_CRE", DbType.String,ParameterDirection.Input),
                 new DAABRequest.Parameter( "PI_TEVAC_CODIGO", DbType.String,ParameterDirection.Input),
                 new DAABRequest.Parameter( "PI_CLIEC_NUM_DOC", DbType.String,ParameterDirection.Input),
                 new DAABRequest.Parameter( "PO_RESPUESTACODIGO", DbType.Int64,ParameterDirection.Output),		
				 new DAABRequest.Parameter( "PO_RESPUESTAMENSAJE", DbType.String,200,ParameterDirection.Output),								   
		
            };

            arrParam[0].Value = Funciones.CheckInt(strsec);
            arrParam[1].Value = Funciones.CheckStr(strusuario);
            arrParam[2].Value = Funciones.CheckStr(strtevac);
            arrParam[3].Value = Funciones.CheckStr(nrodoc);

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_BUYBACK + ".SISACTSD_ANULARSEC_BUYBACK";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            IDataReader dr = null;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                IDataParameter pSalida1;
                IDataParameter pSalida2;
                pSalida1 = (IDataParameter)objRequest.Parameters[4];
                pSalida2 = (IDataParameter)objRequest.Parameters[5];
                codigo = Funciones.CheckInt(pSalida1.Value);
                mensajebuyback = Funciones.CheckStr(pSalida2.Value);
               
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

        
      //PROY- 140736 FIN

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Consulta para obtener las promociones vigentes]
        public static DataSet ConsultarPromocionesVigentesxCampania(string strCodigoCampania)
        {
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("P_COD_CAMPANA", DbType.String, ParameterDirection.Input),
                                                  new DAABRequest.Parameter("P_CUR_PROMOCIONES", DbType.Object, ParameterDirection.Output),
                                                  new DAABRequest.Parameter("P_CUR_PROMO_COMBI", DbType.Object, ParameterDirection.Output),
                                                  new DAABRequest.Parameter("P_CUR_ACCESORIO_PROMO", DbType.Object, ParameterDirection.Output)
                                                };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strCodigoCampania;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_MANT_PROMO_ACC + ".SISACTSS_PROMO_VIG_POR_CAMPANA";
            obRequest.Parameters.AddRange(arrParam);

            DataSet ds;
            try
            {
                ds = obRequest.Factory.ExecuteDataset(ref obRequest);
                DataTable dtPromociones = ds.Tables[0];
                DataTable dtPromoCombinaciones = ds.Tables[1];
                DataTable dtAccesoriosPromocion = ds.Tables[2];
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
            return ds;
        }

        public List<BESolicitud> ConsultarSolicitudesCliente(string strNroDocCliente, int intMaxDiasBuscaSEC)
        {
            string strArchivo = "ConsultarSolicitudesCliente()";
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("P_NUM_DOC_CLIENTE", DbType.String, ParameterDirection.Input),
                                                  new DAABRequest.Parameter("P_MAX_DIAS_BUSCAR", DbType.Int32, ParameterDirection.Input),
                                                  new DAABRequest.Parameter("P_LISTADO", DbType.Object, ParameterDirection.Output)
                                                };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strNroDocCliente;
            arrParam[1].Value = intMaxDiasBuscaSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_MANT_PROMO_ACC + ".SISACTSS_SOLIC_CLIENTE_PROMO";
            obRequest.Parameters.AddRange(arrParam);

            GeneradorLog.EscribirLog(strArchivo, "-", "Solicitudes Cliente : " + obRequest.Command + "| P_NUM_DOC_CLIENTE: " + strNroDocCliente + "|P_MAX_DIAS_BUSCAR" + Funciones.CheckStr(intMaxDiasBuscaSEC));

            List<BESolicitud> filas = new List<BESolicitud>();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BESolicitud item = new BESolicitud();
                    item.SOLIN_CODIGO = Funciones.CheckInt64(dr["SOLIN_CODIGO"]);
                    item.NUM_CONTRATO = Funciones.CheckInt(dr["CONTN_NUMERO_CONTRATO"]);
                    item.NUM_CORRELA_CONTRATO = Funciones.CheckInt(dr["CORRELATIVO"]);
                    item.NUM_VENTA = Funciones.CheckInt(dr["ID_VENTA"]);
                    item.NUM_PEDIDO = Funciones.CheckInt(dr["NRO_DOCUMENTO"]);
                    item.COD_EQUIPO = Funciones.CheckStr(dr["COD_EQUIPO"]);
                    item.PRECIOBASE_EQUIPO = Funciones.CheckDbl(dr["MATEN_PRECIOBASE"]);
                    item.SERIE_EQUIPO = Funciones.CheckStr(dr["SERIE_EQUIPO"]);
                    item.PLAN_TARIFARIO = Funciones.CheckStr(dr["PLAN_TARIFAR"]);
                    item.PLANN_CAR_FIJ = Funciones.CheckDbl(dr["PLANN_CAR_FIJ"]);
                    item.CAMPN_CODIGO = Funciones.CheckStr(dr["CAMPANA"]);
                    item.TELEFONO = Funciones.CheckStr(dr["TELEFONO"]);
                    item.FECHA_CONTRATO = Funciones.CheckDate(dr["CONTD_FECHA_CONTRATO"]);
                    item.USUARIO_CREA_CONTRATO = Funciones.CheckStr(dr["CONTV_USUARIO_CREACION"]);
                    item.TOFIC_CODIGO = Funciones.CheckStr(dr["TOFIC_CODIGO"]);
                    item.TCLIC_CODIGO = Funciones.CheckStr(dr["TCLIC_CODIGO"]);
                    item.TOPEN_CODIGO = Funciones.CheckStr(dr["TOPEN_CODIGO"]);
                    item.FLAG_PORTABILIDAD = Funciones.CheckStr(dr["FLAG_PORTABILIDAD"]);
                    item.MODALIDAD_VENTA = Funciones.CheckInt(dr["MODALIDAD_VENTA"]);
                    item.DIAS_ANTIGUEDAD = Funciones.CheckInt(dr["DIAS_ANTIGUEDAD"]);
                    filas.Add(item);
                }
            }
            catch (Exception e)
            {
                GeneradorLog.EscribirLog(strArchivo, "-", "Solicitudes Cliente SISACTSS_SOLIC_CLIENTE_PROMO : " + e.Message);
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            GeneradorLog.EscribirLog(strArchivo, "-", "Total Registros Solicitudes Cliente SISACTSS_SOLIC_CLIENTE_PROMO: " + filas == null ? 0 : filas.Count);
            return filas;
        }

        public List<BESolicitud> ConsultarSolicitudesPrepago(string strNroDocCliente, int intMaxDiasBuscaSEC)
        {
            string strArchivo = "ConsultarSolicitudesPrepago()";
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("P_NUM_DOC_CLIENTE", DbType.String, ParameterDirection.Input),
                                                  new DAABRequest.Parameter("P_MAX_DIAS_BUSCAR", DbType.Int32, ParameterDirection.Input),
                                                  new DAABRequest.Parameter("P_LISTADO", DbType.Object, ParameterDirection.Output)
                                                };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strNroDocCliente;
            arrParam[1].Value = intMaxDiasBuscaSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_MANT_PROMO_ACC + ".sisactss_solic_cli_promo_pre";
            obRequest.Parameters.AddRange(arrParam);

            GeneradorLog.EscribirLog(strArchivo, "-", "Solicitudes Cliente : " + obRequest.Command + "| P_NUM_DOC_CLIENTE: " + strNroDocCliente + "|P_MAX_DIAS_BUSCAR" + Funciones.CheckStr(intMaxDiasBuscaSEC));

            List<BESolicitud> filas = new List<BESolicitud>();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BESolicitud item = new BESolicitud();
                    item.SOLIN_CODIGO = Funciones.CheckInt64(dr["SOLIN_CODIGO"]);
                    item.NUM_CONTRATO = Funciones.CheckInt(dr["CONTN_NUMERO_CONTRATO"]);
                    item.NUM_CORRELA_CONTRATO = Funciones.CheckInt(dr["CORRELATIVO"]);
                    item.NUM_VENTA = Funciones.CheckInt(dr["ID_VENTA"]);
                    item.NUM_PEDIDO = Funciones.CheckInt(dr["NRO_DOCUMENTO"]);
                    item.COD_EQUIPO = Funciones.CheckStr(dr["COD_EQUIPO"]);
                    item.PRECIOBASE_EQUIPO = Funciones.CheckDbl(dr["MATEN_PRECIOBASE"]);
                    item.SERIE_EQUIPO = Funciones.CheckStr(dr["SERIE_EQUIPO"]);
                    item.PLAN_TARIFARIO = Funciones.CheckStr(dr["PLAN_TARIFAR"]);
                    item.PLANN_CAR_FIJ = Funciones.CheckDbl(dr["PLANN_CAR_FIJ"]);
                    item.CAMPN_CODIGO = Funciones.CheckStr(dr["CAMPANA"]);
                    item.TELEFONO = Funciones.CheckStr(dr["TELEFONO"]);
                    item.FECHA_CONTRATO = Funciones.CheckDate(dr["CONTD_FECHA_CONTRATO"]);
                    item.USUARIO_CREA_CONTRATO = Funciones.CheckStr(dr["CONTV_USUARIO_CREACION"]);
                    item.TOFIC_CODIGO = Funciones.CheckStr(dr["TOFIC_CODIGO"]);
                    item.TCLIC_CODIGO = Funciones.CheckStr(dr["TCLIC_CODIGO"]);
                    item.TOPEN_CODIGO = Funciones.CheckStr(dr["TOPEN_CODIGO"]);
                    item.FLAG_PORTABILIDAD = Funciones.CheckStr(dr["FLAG_PORTABILIDAD"]);
                    item.MODALIDAD_VENTA = Funciones.CheckInt(dr["MODALIDAD_VENTA"]);
                    item.DIAS_ANTIGUEDAD = Funciones.CheckInt(dr["DIAS_ANTIGUEDAD"]);
                    filas.Add(item);
                }
            }
            catch (Exception e)
            {
                GeneradorLog.EscribirLog(strArchivo, "-", "Solicitudes Cliente SISACTSS_SOLIC_CLI_PROMO_PRE : " + e.Message);
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            GeneradorLog.EscribirLog(strArchivo, "-", "Total Registros Solicitudes Cliente SISACTSS_SOLIC_CLIENTE_PROMO: " + filas == null ? 0 : filas.Count);
            return filas;
        }

         #endregion
    }
}
