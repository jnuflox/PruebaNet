using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.IData;
using System.Diagnostics;
using System.Data;

namespace Claro.SISACT.Data
{
   public class DASincronizaSap
    {
        GeneradorLog objLog = new GeneradorLog("Consumer", null, null, "DATA_LOG"); //PROY-24724-IDEA-28174
      
        public List<BEConsultarPrecioBase> ConsultarPrecioBase( string codMaterial, string desMaterial)
        {
            GeneradorLog objLog = new GeneradorLog("DASincronizaMSSAP", null, null, "DATA_LOG");
            //Store Procedure devuelve los materiales con su precio Base y de Compra (filtro por Codigo y Descripcion de Materiales)
            List<BEConsultarPrecioBase> obeLisConsultaPrecioBase = new List<BEConsultarPrecioBase>();
            DAABRequest.Parameter[] arrParam = {
                                                 new DAABRequest.Parameter("K_MATEC_CODMATERIAL",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_MATEV_DESCMATERIAL",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_RESULT_SET",DbType.Object,ParameterDirection.Output)
                                             };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = codMaterial;
            arrParam[1].Value = desMaterial;

            var conext = new BDSincronizaSap(BaseDatos.BdMSSAP);
            var obRequest = conext.CreaRequest(new StackTrace(true), codMaterial.ToString());
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_CONSULTA + Constantes.SSAPPreciosBase;
            obRequest.Parameters.AddRange(arrParam);

            objLog.CrearArchivolog(obRequest.Command, null, null);

            for (int p = 0; p < arrParam.Length; p++)
            {
                if (arrParam[p].DbType != DbType.Object)
                {
                    objLog.CrearArchivolog(arrParam[p].ParameterName, arrParam[p].Value, null);
                }
            }

            
            IDataReader dr = null;

            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        BEConsultarPrecioBase rowsConsultaPrecioBase = new BEConsultarPrecioBase();
                        rowsConsultaPrecioBase.CodigoMaterial = Funciones.CheckStr(dr["MATEC_CODMATERIAL"]);
                        rowsConsultaPrecioBase.DescripcionMaterial = Funciones.CheckStr(dr["MATEV_DESCMATERIAL"]);
                        rowsConsultaPrecioBase.PrecioBase = Funciones.CheckDecimal(dr["MATEN_PRECIOBASE"]);
                        rowsConsultaPrecioBase.PrecioCompra = Funciones.CheckDecimal(dr["MATEN_PRECIOCOMPRA"]);
                        obeLisConsultaPrecioBase.Add(rowsConsultaPrecioBase);
                    }
                }
            }

            catch ( Exception Ex)
            {
                objLog.CrearArchivolog("ERROR", Ex.Message, Ex);
                throw Ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            
            return obeLisConsultaPrecioBase;
        }

        //public List<BEConsultarMaterialXCampania> ConsultarMaterialXCampania(string codMaterial, string desMaterial, string codCentro, string codAlmacen, string codOficina, string desOficina)
        public List<BEConsultarMaterialXCampania> ConsultarMaterialXCampania(string vCodigoOficina, string vDescripcionOficina,  string vCodigoCentro, string vCodigoAlmacen, string vTipoOficina)
        {
            GeneradorLog objLog = new GeneradorLog("MSSAP", null, null, "Log");
            //Sp que devuelve los materiales que pertenecen a la oficina de venta (filtro por Codigo y Tipo de Oficina)
            List<BEConsultarMaterialXCampania> obeLisConsultaPrecioBase = new List<BEConsultarMaterialXCampania>();
            DAABRequest.Parameter[] arrParam = {
                                                 //new DAABRequest.Parameter("K_MATEC_CODMATERIAL",DbType.String,ParameterDirection.Input),
                                                 //new DAABRequest.Parameter("K_MATEV_DESCMATERIAL",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_OFICV_CODOFICINA",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_OFICV_DESCRIPCION",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_OFICC_CODCENTRO",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_OFICC_CODALMACEN",DbType.String,ParameterDirection.Input),
                                                 //new DAABRequest.Parameter("K_OFICC_CODOFICINA",DbType.String,ParameterDirection.Input),
                                                 //new DAABRequest.Parameter("K_OFICV_DESCRIPCIONOFICINA",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_OFICC_TIPOOFICINA",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_RESULT_SET",DbType.Object,ParameterDirection.Output)
                                             };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = vCodigoOficina;
            arrParam[1].Value = vDescripcionOficina;
            arrParam[2].Value = vCodigoCentro;
            arrParam[3].Value = vCodigoAlmacen;
            arrParam[4].Value = vTipoOficina;

            var conext = new BDSincronizaSap(BaseDatos.BdMSSAP);
            var obRequest = conext.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_CONSULTA + Constantes.SSAPMaterialesXOficina;
            //obRequest.Transactional = true;
            obRequest.Parameters.AddRange(arrParam);


            objLog.CrearArchivolog(obRequest.Command, null, null);

            for (int p = 0; p < arrParam.Length; p++)
            {
                if (arrParam[p].DbType != DbType.Object)
                {
                    objLog.CrearArchivolog(arrParam[p].ParameterName, arrParam[p].Value, null);
                }
            }
            

            IDataReader dr = null;

            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        BEConsultarMaterialXCampania rowsConsultaPrecioBase = new BEConsultarMaterialXCampania();
                        rowsConsultaPrecioBase.CodigoMaterial = Funciones.CheckStr(dr["MATEC_CODMATERIAL"]);
                        rowsConsultaPrecioBase.DescripcionMaterial = Funciones.CheckStr(dr["MATEV_DESCMATERIAL"]);
                        rowsConsultaPrecioBase.CodigoCentro = Funciones.CheckStr(dr["OFICC_CODCENTRO"]);
                        rowsConsultaPrecioBase.CodigoAlmacen = Funciones.CheckStr(dr["OFICC_CODALMACEN"]);
                        rowsConsultaPrecioBase.CodigoOficina = Funciones.CheckStr(dr["OFICV_CODOFICINA"]);
                        rowsConsultaPrecioBase.DescripcionOficina = Funciones.CheckStr(dr["OFICV_DESCRIPCION"]);
                        rowsConsultaPrecioBase.TipoMaterial = Funciones.CheckStr(dr["MATEC_TIPOMATERIAL"]); 
                        obeLisConsultaPrecioBase.Add(rowsConsultaPrecioBase);
                    }
                }
            }

            catch (Exception Ex)
            {
                objLog.CrearArchivolog("ERROR", Ex.Message, Ex);
                throw Ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return obeLisConsultaPrecioBase;
        }
        
        public List<BEConsultaListaPrecios> ConsultarPrecio( BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog("MSSAP", null, null, "Log");
            //devuelve la lista de precios vigente
            List<BEConsultaListaPrecios> obeConsultaListaPrecios = new List<BEConsultaListaPrecios>();
            DAABRequest.Parameter[] arrParam = {
                                                 new DAABRequest.Parameter("P_TPROC_CODIGO",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_TVENC_CODIGO",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_CANAC_CODIGO",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_DEPARTAMENTO",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_MATEC_CODIGO",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_CAMPC_CODIGO",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_TOPEC_CODIGO",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_PLAZC_CODIGO",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("P_PLANC_CODIGO",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_SALIDA",DbType.Object,ParameterDirection.Output)
                                             };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            int f = 0;
            
            arrParam[f].Value = objForm.idOferta;
            f++; arrParam[f].Value = objForm.idTipoVenta;
            f++; arrParam[f].Value = objForm.tipoOficina;
            f++; arrParam[f].Value = objForm.idDepartamento;
            f++; arrParam[f].Value = objForm.idMaterial;
            f++; arrParam[f].Value = objForm.idCampanaSap;
            f++; arrParam[f].Value= objForm.idTipoOperacion;
            f++; arrParam[f].Value = objForm.idPlazo;
            f++; arrParam[f].Value = Funciones.CheckStr(objForm.idPlanSap).Length == 0 ? objForm.idPlan : objForm.idPlanSap;

            BDSISACT conext = new BDSISACT(BaseDatos.BdSisact);
            var obRequest = conext.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_NUEVA_LISTAPRECIOS + Constantes.SISACConsultaPrecios;
            obRequest.Parameters.AddRange(arrParam);

            objLog.CrearArchivolog(obRequest.Command, null, null);

            for (int p = 0; p < arrParam.Length; p++)
            {
                if (arrParam[p].DbType != DbType.Object)
                {
                    objLog.CrearArchivolog(arrParam[p].ParameterName, arrParam[p].Value, null);
                }
            }
                        
            IDataReader dr = null;

            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        BEConsultaListaPrecios rowsListaPrecios = new BEConsultaListaPrecios();
                        rowsListaPrecios.CodigoListaPrecio = Funciones.CheckStr(dr["LIPRN_CODIGOLISTAPRECIO"]);
                        rowsListaPrecios.DescripcionListaPrecios = Funciones.CheckStr(dr["LIPRV_DESCRIPCION"]);
                        rowsListaPrecios.PrecioBase = Funciones.CheckDbl(dr["MATED_PRECIOVENTA"]);
                        rowsListaPrecios.CodigoMaterial = Funciones.CheckStr(dr["MATEC_CODMATERIAL"]);
                        obeConsultaListaPrecios.Add(rowsListaPrecios);
                    }
                }
            }

            catch ( Exception ex)
            {
                objLog.CrearArchivolog("Error", ex.Message, ex);
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return obeConsultaListaPrecios;
        }

       //140736 INI
        public List<BEConsultaListaPrecios> ConsultarPreciobBuyback(BEFormEvaluacion objForm)
        {
            GeneradorLog objLog = new GeneradorLog("MSSAP", null, null, "Log");
            //devuelve la lista de precios vigente
            List<BEConsultaListaPrecios> obeConsultaListaPrecios = new List<BEConsultaListaPrecios>();
            DAABRequest.Parameter[] arrParam = {
                                             new DAABRequest.Parameter("PI_CODIGO_MATERIAL", DbType.String, ParameterDirection.Input),
                                             new DAABRequest.Parameter("PI_CODIGO_LP", DbType.String, ParameterDirection.Input),
                                             new DAABRequest.Parameter("PI_COD_MATERIAL_VENTA", DbType.String, ParameterDirection.Input),
                                             new DAABRequest.Parameter("PI_CODIGO_PLAN", DbType.String, ParameterDirection.Input),
                                             new DAABRequest.Parameter("PI_CODIGO_PLAZO", DbType.String, ParameterDirection.Input),
                                             new DAABRequest.Parameter("PO_RESPUESTACODIGO", DbType.Int64, ParameterDirection.Output),
                                             new DAABRequest.Parameter("PO_RESPUESTAMENSAJE", DbType.String, 500, ParameterDirection.Output),
                                             new DAABRequest.Parameter("PO_CURSOR", DbType.Object, ParameterDirection.Output)
                                              
                                             };
            int i = 0;
            for ( i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objForm.strmaterialbuyback;
            arrParam[2].Value = objForm.idMaterial;
            arrParam[3].Value = Funciones.CheckStr(objForm.idPlanSap).Length == 0 ? objForm.idPlan : objForm.idPlanSap;
            arrParam[4].Value = objForm.idPlazo;
            
            BDSISACT conext = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = conext.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_BUYBACK + ".SISACTSS_LISTA_PRECIO_CANJE";
            obRequest.Parameters.AddRange(arrParam);

            objLog.CrearArchivolog(obRequest.Command, null, null);

            for (int p = 0; p < arrParam.Length; p++)
            {
                if (arrParam[p].DbType != DbType.Object)
                {
                    objLog.CrearArchivolog(arrParam[p].ParameterName, arrParam[p].Value, null);
                }
            }

            IDataReader dr = null;

            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        BEConsultaListaPrecios rowsListaPrecios = new BEConsultaListaPrecios();
                        rowsListaPrecios.CodigoListaPrecio = Funciones.CheckStr(dr["CODIGO_LP"]);
                        rowsListaPrecios.DescripcionListaPrecios = Funciones.CheckStr(dr["MATERIAL_VENTA"]);
                        obeConsultaListaPrecios.Add(rowsListaPrecios);
                    }
                }
            }

            catch (Exception ex)
            {
                objLog.CrearArchivolog("Error", ex.Message, ex);
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return obeConsultaListaPrecios;
        }
       //140736 FIN

        //13-10-2014
        public List<BETipoDocOficina> ConsultaTipoDocumentoOficina(string tipOficina, out int result)
        {
            GeneradorLog objLog = new GeneradorLog("MSSAP", null, null, "Log");
            //Devuelve los tipos de Documentos y su Clase (FAC/BOL/NC/ND)
            result = 0;
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("P_TOFIC_CODIGO", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_DOCU_ESTADO", DbType.String, ParameterDirection.Input),
													new DAABRequest.Parameter("P_RESULT", DbType.Int32, ParameterDirection.Output),                                                
                                                   new DAABRequest.Parameter("P_LISTADO", DbType.Object, ParameterDirection.Output) };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = tipOficina;
            arrParam[1].Value = "1";

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PkgConsMaestraSap + Constantes.SISACTConsultaTipoDocumentos;
            obRequest.Parameters.AddRange(arrParam);

            objLog.CrearArchivolog(obRequest.Command, null, null);

            for (int p = 0; p < arrParam.Length; p++)
            {
                if (arrParam[p].DbType != DbType.Object)
                {
                    objLog.CrearArchivolog(arrParam[p].ParameterName, arrParam[p].Value, null);
                }
            }
	
            var filas = new List<BETipoDocOficina>();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                if (dr != null)
                {
                    IDataParameter pSalida;
                    pSalida = (IDataParameter)obRequest.Parameters[2];
                    result = Funciones.CheckInt(pSalida.Value);
                
                    while (dr.Read())
                    {
                        var item = new BETipoDocOficina();
                        item.CodDoc = Funciones.CheckStr(dr["DOCU_CODIGO"]);
                        item.DesDoc = Funciones.CheckStr(dr["DOCU_DESCRIP"]);
                        item.ClasDoc = Funciones.CheckStr(dr["DOCU_CLASE"]);
                        filas.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR", ex.Message, ex);
                throw ex;
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

        private List<BEConsultaDatosOficina> ConsultaDatosOficinaMSSAP(string vCodigoOficina, string vDescripcionOficina)
        {
            //SP que devuelve los parametros de la oficina segun pto de venta
            GeneradorLog objLog = new GeneradorLog("MSSAP", null, null, "Log");
            List<BEConsultaDatosOficina> obeLisConsultaDatosOficina = new List<BEConsultaDatosOficina>();
            DAABRequest.Parameter[] arrParam = {
                                                 new DAABRequest.Parameter("K_OFICV_CODOFICINA",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_OFICV_DESCRIPCION",DbType.String,ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_RESULT_SET",DbType.Object,ParameterDirection.Output)
                                             };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            int f = 0;
            arrParam[f].Value = vCodigoOficina;
            f++; arrParam[f].Value = vDescripcionOficina;

            var conext = new BDSincronizaSap(BaseDatos.BdMSSAP);            
            DAABRequest obRequest = conext.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_CONSULTA + Constantes.SSAPDatosOficina;
            obRequest.Parameters.AddRange(arrParam);

            objLog.CrearArchivolog(obRequest.Command, null, null);

            for (int p = 0; p < arrParam.Length; p++)
            {
                if (arrParam[p].DbType != DbType.Object)
                {
                    objLog.CrearArchivolog(arrParam[p].ParameterName, arrParam[p].Value, null);
                }
            }
	
            IDataReader dr = null;

            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        BEConsultaDatosOficina rowsConsultaDatosOficina = new BEConsultaDatosOficina()
                        {
                            CodOficina = Funciones.CheckStr(dr["OFICV_CODOFICINA"]),
                            DescripcionOficina = Funciones.CheckStr(dr["OFICV_DESCRIPCION"]),
                            CodCentro = Funciones.CheckStr(dr["OFICC_CODCENTRO"]),
                            CodAlmacen = Funciones.CheckStr(dr["OFICC_CODALMACEN"]),
                            CodigoRegion = Funciones.CheckStr(dr["OFICC_REGION"]),
                            OrgVta = Funciones.CheckStr(dr["OFICC_ORGVENTA"])                            
                        };
                        obeLisConsultaDatosOficina.Add(rowsConsultaDatosOficina);
                    }
                }
            }

            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR", ex.Message, ex);
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return obeLisConsultaDatosOficina;
        }

        public BEConsultaDatosOficina ConsultaDatosOficina(string vCodigoOficina, string vDescripcionOficina)
        {
            GeneradorLog objLog = new GeneradorLog(null, vCodigoOficina.ToString(), null, "DATA_LOG");
            //SP que devuelve los parametros de la oficina segun pto de venta
            string pCodigoOficina = string.Empty;

            BEConsultaDatosOficina obeDatosOficina = new BEConsultaDatosOficina();
            List<BEConsultaMaterialesXPlanes> oListaMaterialesxPlanes = new List<BEConsultaMaterialesXPlanes>();
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("P_OVENC_CODIGO", DbType.String, ParameterDirection.Input),                                                   
                                                   new DAABRequest.Parameter("K_SALIDA", DbType.Object, ParameterDirection.Output) };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = vCodigoOficina;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true), vCodigoOficina.ToString());
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_NUEVA_LISTAPRECIOS + Constantes.SISACSDatosOficina;
            obRequest.Parameters.AddRange(arrParam);


            objLog.CrearArchivolog("[Inicio][ConsultaDatosOficina]", null, null);

            for (int p = 0; p < arrParam.Length; p++)
            {
                if (arrParam[p].DbType != DbType.Object)
                {
                    objLog.CrearArchivolog(arrParam[p].ParameterName, arrParam[p].Value, null);
                }
            }

            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                 List<BEConsultaDatosOficina> oDatosOficinaSISACT = new List<BEConsultaDatosOficina>();
                if (dr != null)
                {
                   
                    while (dr.Read())
                    {
                        var item = new BEConsultaDatosOficina()
                        {
                            CodOficina = Funciones.CheckStr(dr["OVENC_CODIGO"]),
                            Canal = Funciones.CheckStr(dr["CANAC_CODIGO"]),
                            CodigoTipoProducto = Funciones.CheckStr(dr["TPROC_CODIGO"]),
                            DescripcionOficina = Funciones.CheckStr(dr["OVENV_DESCRIPCION"]),
                            TipoOficina = Funciones.CheckStr(dr["TOFIC_CODIGO"]),
                            CodigoUsuario = Funciones.CheckStr(dr["OVENV_CODUSUARIO"]),
                            CodigoRegion = Funciones.CheckStr(dr["DEPAC_CODIGO"]),
                            CodigoInterlocutor = Funciones.CheckStr(dr["OVENV_CODIGO_SINERGIA"])
                        };
                        if (item != null)
                            pCodigoOficina = item.CodigoInterlocutor;

                        oDatosOficinaSISACT.Add(item);
                    }
                }

                List<BEConsultaDatosOficina> oDatosOficinaMSSAP = ConsultaDatosOficinaMSSAP(pCodigoOficina, vDescripcionOficina);

                var query = from a in oDatosOficinaSISACT
                            join b in oDatosOficinaMSSAP on a.CodigoInterlocutor equals b.CodOficina
                            select new { a.CodOficina, a.DescripcionOficina, a.Canal, a.CodigoTipoProducto, a.TipoOficina, a.CodigoRegion, b.OrgVta, b.CodCentro, b.CodAlmacen, a.CodigoInterlocutor };

                foreach (var item in query)
                {
                    obeDatosOficina.CodOficina = item.CodOficina;
                    obeDatosOficina.DescripcionOficina = item.DescripcionOficina;
                    obeDatosOficina.Canal = item.Canal;
                    obeDatosOficina.CodigoTipoProducto = item.CodigoTipoProducto;
                    obeDatosOficina.TipoOficina = item.TipoOficina;
                    obeDatosOficina.CodigoRegion = item.CodigoRegion;
                    obeDatosOficina.OrgVta = item.OrgVta;
                    obeDatosOficina.CodAlmacen = item.CodAlmacen;
                    obeDatosOficina.CodCentro = item.CodCentro;
                    obeDatosOficina.CodigoInterlocutor = item.CodigoInterlocutor;
                }                    
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("ERROR", ex.Message, ex);
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            objLog.CrearArchivolog("[Fin][ConsultaDatosOficina]", null, null);
            return obeDatosOficina;
        }

        public DataTable ConsultarDatosMaterial(string strCodMaterial) //PROY-24724-IDEA-28174 - INICIO
        {
            objLog.CrearArchivolog("[Inicio][ConsultarDatosMaterial][strCodMaterial] " + strCodMaterial, null, null);
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("K_MATEC_CODMATERIAL", DbType.String, ParameterDirection.Input),                                                   
                                                 new DAABRequest.Parameter("K_RESULT_SET", DbType.Object, ParameterDirection.Output) };

            arrParam[0].Value = strCodMaterial;

            BDSISACT obj = new BDSISACT(BaseDatos.BdMSSAP);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_CONSULTA + ".SSAPSS_MATERIAL";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[Error][ConsultarDatosMaterial]", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("[Fin][ConsultarDatosMaterial]", null, null);
                objRequest.Factory.Dispose();
            }
        } //PROY-24724-IDEA-28174 - FIN

        //INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
        public static BEClienteSAP ConsultaCliente(string strTipoDoc, string strNumDoc, out string strCodRpta, out string strMsgRpta)
        {
            //Invocara al SP SSAPSS_CLIENTE     
            strCodRpta = "0";
            strMsgRpta = "";
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("K_CLIEC_TIPODOCCLIENTE", DbType.String, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_CLIEV_NRODOCCLIENTE", DbType.String, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("K_NROLOG", DbType.Int32, ParameterDirection.Output),
                                                 new DAABRequest.Parameter("K_DESLOG", DbType.String, ParameterDirection.Output),
                                                 new DAABRequest.Parameter("K_CU_CODCLIENTE", DbType.Object, ParameterDirection.Output) };


            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strTipoDoc;
            arrParam[1].Value = strNumDoc;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PkgConsMaestraSap + ".SSAPSS_CLIENTE";
            obRequest.Parameters.AddRange(arrParam);

            BEClienteSAP oConsultaCliente = null;

            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                if (dr != null)
                {

                    strCodRpta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[2]).Value);
                    strMsgRpta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[3]).Value);

                    while (dr.Read())
                    {
                        oConsultaCliente = new BEClienteSAP()
                        {
                            Cliente = Funciones.CheckStr(dr["CLIEV_NRO_DOCUMENTO"]),
                            TipoDocCliente = Funciones.CheckStr(dr["CLIEC_TIPO_DOCUMENTO"]),
                            Nombre = Funciones.CheckStr(dr["CLIEV_NOMBRE"]),
                            ApellidoPaterno = Funciones.CheckStr(dr["CLIEV_APELLIDO_PATERNO"]),
                            ApellidoMaterno = Funciones.CheckStr(dr["CLIEV_APELLIDO_MATERNO"]),
                            RazonSocial = Funciones.CheckStr(dr["CLIEV_RAZON_SOCIAL"]),
                            FechaNacimiento = Funciones.CheckStr(dr["CLIED_FECHA_NACIMIENTO"]) == null || Funciones.CheckStr(dr["CLIED_FECHA_NACIMIENTO"]).Length < 10 ? "" : Funciones.CheckStr(dr["CLIED_FECHA_NACIMIENTO"]).Substring(0, 10),
                            Telefono = Funciones.CheckStr(dr["CLIEV_TELEFONO"]),
                            EMail = Funciones.CheckStr(dr["CLIEV_E_MAIL"]),
                            Sexo = Funciones.CheckStr(dr["CLIEC_SEXO"]),
                            EstadoCivil = Funciones.CheckStr(dr["CLIEC_ESTADO_CIVIL"]),
                            TitCliente = Funciones.CheckStr(dr["CLIEC_TITULO"]),
                            CargaFamiliar = Funciones.CheckStr(dr["CLIEC_CARGA_FAMILIAR"]),
                            NombreConyuge = Funciones.CheckStr(dr["CLIEV_CONYUGE_NOMBRE"]),
                            ApePatConyuge = Funciones.CheckStr(dr["CLIEV_CONYUGE_APE_PAT"]),
                            ApeMatConyuge = Funciones.CheckStr(dr["CLIEV_CONYUGE_APE_MAT"]),
                            DireccionLegalPref = Funciones.CheckStr(dr["CLIEV_DIRECCION_LEGAL_PREF"]),
                            DireccionLegal = Funciones.CheckStr(dr["CLIEV_DIRECCION_LEGAL"]),
                            ReferDireccion = Funciones.CheckStr(dr["CLIEV_DIRECCION_LEGAL_REFER"]),
                            UbigeoLegal = Funciones.CheckStr(dr["CLIEV_UBIGEO_LEGAL"]),
                            TelfPref = Funciones.CheckStr(dr["CLIEV_TELEF_LEGAL_PREF"]),
                            TelefLegal = Funciones.CheckStr(dr["CLIEV_TELEF_LEGAL"]),
                            DireccionFactPref = Funciones.CheckStr(dr["CLIEV_DIRECCION_FACT_PREF"]),
                            DireccionFact = Funciones.CheckStr(dr["CLIEV_DIRECCION_FACT"]),
                            DireccionFactRefe = Funciones.CheckStr(dr["CLIEV_DIRECCION_FACT_REFER"]),
                            UbigeoFact = Funciones.CheckStr(dr["CLIEV_UBIGEO_FACT"]),
                            TelefLegalPref = Funciones.CheckStr(dr["CLIEV_TELEF_FACT_PREF"]),
                            TelfFact = Funciones.CheckStr(dr["CLIEV_TELEF_FACT"]),
                            ReplegalTipDoc = Funciones.CheckStr(dr["CLIEC_REPLEGAL_TIPO_DOC"]),
                            ReplegalNroDoc = Funciones.CheckStr(dr["CLIEV_REPLEGAL_NRO_DOC"]),
                            ReplegalNombre = Funciones.CheckStr(dr["CLIEV_REPLEGAL_NOMBRE"]),
                            ReplegalApePat = Funciones.CheckStr(dr["CLIEV_REPLEGAL_APE_PAT"]),
                            ReplegalApeMat = Funciones.CheckStr(dr["CLIEV_REPLEGAL_APE_MAT"]),
                            ReplegalFnac = Funciones.CheckStr(dr["CLIED_REPLEGAL_FECHA_NAC"]),
                            ReplegalTelefon = Funciones.CheckStr(dr["CLIEV_REPLEGAL_TELEFONO"]),
                            ReplegalSexo = Funciones.CheckStr(dr["CLIEC_REPLEGAL_SEXO"]),
                            ReplegalEstCiv = Funciones.CheckStr(dr["CLIEC_REPLEGAL_EST_CIV"]),
                            ReplegalTit = Funciones.CheckStr(dr["CLIEC_REPLEGAL_TITULO"]),
                            ContactoTipDoc = Funciones.CheckStr(dr["CLIEC_CONTACTO_TIPO_DOC"]),
                            ContactoNroDoc = Funciones.CheckStr(dr["CLIEV_CONTACTO_NRO_DOC"]),
                            ContactoNombre = Funciones.CheckStr(dr["CLIEV_CONTACTO_NOMBRE"]),
                            ContactoApePat = Funciones.CheckStr(dr["CLIEV_CONTACTO_APE_PAT"]),
                            ContactoApeMat = Funciones.CheckStr(dr["CLIEV_CONTACTO_APE_MAT"]),
                            ContactoTelefon = Funciones.CheckStr(dr["CLIEV_CONTACTO_TELEFONO"]),
                            ClienCondCliente = Funciones.CheckInt(dr["CLIEN_COND_CLIENTE"]),
                            EmpresaLabora = Funciones.CheckStr(dr["CLIEV_EMPRESA_LABORA"]),
                            EmpresaCargo = Funciones.CheckStr(dr["CLIEV_EMPRESA_CARGO"]),
                            EmpresaTelefono = Funciones.CheckStr(dr["CLIEV_EMPRESA_TELEFONO"]),
                            IngBruto = Funciones.CheckDecimal(dr["CLIEN_INGRESO_BRUTO"]),
                            OtrosIngresos = Funciones.CheckDecimal(dr["CLIEN_OTROS_INGRESOS"]),
                            TarjetaCredito = Funciones.CheckStr(dr["CLIEV_TCREDITO_TIPO"]),
                            NumTarjCredito = Funciones.CheckStr(dr["CLIEV_TCREDITO_NUM"]),
                            MonedaTcred = Funciones.CheckStr(dr["CLIEC_TCREDITO_MONEDA"]),
                            LineaCredito = Funciones.CheckDecimal(dr["CLIEN_TCREDITO_LINEA_CRED"]),
                            FechaVencTcred = Funciones.CheckStr(dr["CLIEC_TCREDITO_FECHA_VENC"]),
                            Observaciones = Funciones.CheckStr(dr["CLIEV_OBSERVACIONES"]),
                            CliCodigSap = Funciones.CheckStr(dr["CLIEV_CODIGO_SAP"]),
                            VendedorSap = Funciones.CheckStr(dr["CLIEV_VENDEDOR_SAP"]),
                            UsuarioCrea = Funciones.CheckStr(dr["CLIEV_USUARIO_CREA"]),
                            FecCrea = Funciones.CheckStr(dr["CLIED_FECHA_CREA"]),
                            TipoCliente = System.Configuration.ConfigurationManager.AppSettings["ConsTipoCliente"],
                            EmailFact = Funciones.CheckStr(dr["CLIEV_CORREO_FACT"]),
                            CliCodNacion = Funciones.CheckStr(dr["CLIEC_CODNACION"]), //INC000003442281
                            CliDescNacion = Funciones.CheckStr(dr["CLIEV_DESCNACION"]) //INC000003442281
                        };
                    }
                }
                else
                {
                    oConsultaCliente = null;
                }

            }
            catch (Exception ex)
            {
                strCodRpta = "-1";
                strMsgRpta = Funciones.CheckStr(ex.Message);
                oConsultaCliente = null;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return oConsultaCliente;
        }
        //FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
		
		//PROY-30859-IDEA-39316-RU02-by-LCEJ INI
        public List<BEConsultarMaterialXCampania> ConsultaArticulosBam(String StrCanal, String StrTipoOper, String StrTipoVent, String StrProd,String CodOfic,String DesOfic, String CodCentroOfic,String CodAlmacOfic,String strEval)
        {
            objLog.CrearArchivolog("INICIO ConsultaArticulosBam ", null, null);

            List<BEConsultarMaterialXCampania> objLista = new List<BEConsultarMaterialXCampania>();

            string strCodResult = string.Empty;
            string strMsjResult = string.Empty;
            DAABRequest.Parameter[] arrParam = {
                                                   new DAABRequest.Parameter("PI_OFICV_CODOFICINA",DbType.String,10, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_OFICV_DESCRIPCION",DbType.String,50, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_OFICC_CODCENTRO",DbType.String,4, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_OFICC_CODALMACEN",DbType.String,4, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_CANAL",DbType.String,2, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_TIPO_OPERACION",DbType.String,10, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_TIPO_VENTA",DbType.String, 2, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_PRODUCTO",DbType.String, 10, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_TIPO_EVAL_VENTA",DbType.String, 2, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PO_CUR_RESULTADO",DbType.Object, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_COD_RESULTADO",DbType.String, 3, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MSJ_RESULTADO",DbType.String, 200, ParameterDirection.Output)
                                               };
            int i = 0;
            arrParam[i].Value = CodOfic;
            i++; arrParam[i].Value = DesOfic;
            i++; arrParam[i].Value = CodCentroOfic;
            i++; arrParam[i].Value = CodAlmacOfic; ;
            i++; arrParam[i].Value = StrCanal;
            i++; arrParam[i].Value = StrTipoOper;
            i++; arrParam[i].Value = StrTipoVent;
            i++; arrParam[i].Value = StrProd;
            i++; arrParam[i].Value = strEval;
            i++; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = DBNull.Value;
            i++; arrParam[i].Value = DBNull.Value;

            objLog.CrearArchivolog("    Parametro Entrada ", arrParam, null);

            BDSincronizaSap obj = new BDSincronizaSap(BaseDatos.BdMSSAP);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;

            obRequest.Command = BaseDatos.PKG_CONSULTA + ".SSAPSS_MATERIALES_BAM";
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                IDataReader dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        var item = new BEConsultarMaterialXCampania()
                        {


                            CodigoMaterial = Funciones.CheckStr(dr["MATEC_CODMATERIAL"]),
                            DescripcionMaterial = Funciones.CheckStr(dr["MATEV_DESCMATERIAL"]),
                            CodigoCentro = Funciones.CheckStr(dr["TIPO_DATO"]),
                            CodigoAlmacen = String.Empty,
                            CodigoOficina = String.Empty,
                            DescripcionOficina = String.Empty,
                            TipoMaterial = Funciones.CheckStr(dr["MATEC_TIPOMATERIAL"])


                        };
                        objLista.Add(item);
                    }
                }
                strCodResult = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[5]).Value);
                strMsjResult = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[6]).Value);
                objLog.CrearArchivolog("RESULTADO ConsultaArticulosBam " + objLista.Count.ToString() + " Registros.", null, null);
            }
            catch (Exception ex)
            {
                strCodResult = "-1";
                strMsjResult = ex.Message;
                objLog.CrearArchivolog("ERROR ConsultaArticulosBam ", ex.Message, ex);
            }
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
                objLog.CrearArchivolog("FIN ConsultaArticulosBam ", null, null);
            }
            return objLista;
        }
        //PROY-30859-IDEA-39316-RU02-by-LCEJ FIN

        //INICIO|PROY-140533 - CONSULTA STOCK
        public static String ConsultarStock(String strPDV, String strCodMaterial, out String strCodRpta, out String strMsgRpta)
        {
            GeneradorLog objLog = new GeneradorLog("Consumer", null, null, "DATA_LOG"); //PROY-24724-IDEA-28174
            objLog.CrearArchivolog("[Inicio][ConsultarStock][strPDV] " + strPDV, null, null);
            objLog.CrearArchivolog("[Inicio][ConsultarStock][strCodMaterial] " + strCodMaterial, null, null);

            String strCantidad = String.Empty;
            strCodRpta = String.Empty;
            strMsgRpta = String.Empty;

            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("PI_OFICINAVENTA", DbType.String, ParameterDirection.Input), 
                                                 new DAABRequest.Parameter("PI_MATERIAL", DbType.String, ParameterDirection.Input),
                                                 new DAABRequest.Parameter("PO_CANTIDAD", DbType.String, ParameterDirection.Output), 
                                                 new DAABRequest.Parameter("PO_MSG_ERROR", DbType.String, ParameterDirection.Output),
                                                 new DAABRequest.Parameter("PO_COD_ERROR", DbType.String, ParameterDirection.Output)};

            arrParam[0].Value = strPDV;
            arrParam[1].Value = strCodMaterial;

            BDSISACT obj = new BDSISACT(BaseDatos.BdMSSAP);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_CONSULTA + ".SSAPSS_CONSULTA_STOCK";
            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;

            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                if (dr != null)
                {
                    strCantidad = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[2]).Value);
                    strCodRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[3]).Value);
                    strMsgRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[4]).Value);
                }

            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[Error][ConsultarStock]", ex.Message, ex);
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("[Fin][ConsultarStock]", null, null);

                if (dr != null && dr.IsClosed == false)
                    dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }

            return strCantidad;
        }
        //FIN|PROY-140533 - CONSULTA STOCK

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
        public List<BEConsultaStock> ConsultarStockXTipoMaterial(BEParametrosMSSAP oParamMSSAP)
        {
            DAABRequest.Parameter[] arrParam = {
                                                    new DAABRequest.Parameter("K_OFICV_CODOFICINA", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("K_OFICV_DESCRIPCION", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("K_OFICC_TIPOOFICINA", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("K_OFICC_FLAGSERVICIO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("K_GRUPO_MATERIAL", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("K_FLAG_PROCESA_PLC", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("K_RESULT_SET", DbType.Object, ParameterDirection.Output)
                                                };

            for (int i = 0; i < arrParam.Length; i++)
            {
                arrParam[i].Value = DBNull.Value;
            }

            arrParam[0].Value = oParamMSSAP.CodigoOficina;
            arrParam[1].Value = null;
            arrParam[2].Value = oParamMSSAP.TipoOficina;
            arrParam[3].Value = oParamMSSAP.FlagServicio;
            arrParam[4].Value = oParamMSSAP.tipoMaterial;
            arrParam[5].Value = oParamMSSAP.strFlagProcesaPLC;


            BDSincronizaSap obj = new BDSincronizaSap(BaseDatos.BdMSSAP);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_CONSULTA + ".SSAPSS_STOCK_X_GRUPO_MATERIAL";
            obRequest.Parameters.AddRange(arrParam);

            var filas = new List<BEConsultaStock>();
            IDataReader dr = null;

            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                int count = 0;
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        var item = new BEConsultaStock();
                        item.CodigOficina = Funciones.CheckStr(dr["OFICV_CODOFICINA"]);
                        item.DescripcionMaterial = Funciones.CheckStr(dr["OFICV_DESCRIPCION"]);
                        item.CodigoMaterial = Funciones.CheckStr(dr["MATEC_CODMATERIAL"]);
                        item.DescripcionMaterial = Funciones.CheckStr(dr["MATEV_DESCMATERIAL"]);
                        item.TipoMaterial = Funciones.CheckStr(dr["MATEC_TIPOMATERIAL"]);
                        filas.Add(item);
                        count++;
                        if (count == oParamMSSAP.TopSeries && oParamMSSAP.TopSeries > 0) { break; }
                    }
                }
            }
            catch
            {
                throw;
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

        public List<BEConsultaStock> listarAccesoriosCuotas()
        {
            DAABRequest.Parameter[] arrParam = {
                                                    new DAABRequest.Parameter("PO_COD_RPTA", DbType.String, ParameterDirection.Output),
                                                    new DAABRequest.Parameter("PO_MSJ_RPTA", DbType.String, ParameterDirection.Output),
                                                    new DAABRequest.Parameter("PO_CUR_DATOS", DbType.Object, ParameterDirection.Output)
                                                };

            for (int i = 0; i < arrParam.Length; i++)
            {
                arrParam[i].Value = DBNull.Value;
            }

            BDSincronizaSap obj = new BDSincronizaSap(BaseDatos.BdMSSAP);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_CONSULTA + ".SSAPSS_LISTA_MATERIALES_ACC";
            obRequest.Parameters.AddRange(arrParam);

            var filas = new List<BEConsultaStock>();
            IDataReader dr = null;

            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;

                if (dr != null)
                {
                    while (dr.Read())
                    {
                        var item = new BEConsultaStock();
                        item.CodigoMaterial = Funciones.CheckStr(dr["MCN_COD_MATERIAL"]);
                        item.DescripcionMaterial = Funciones.CheckStr(dr["MCV_NOM_LARGO"]);
                        item.TipoMaterial = Funciones.CheckStr(dr["MCV_TIPO_ACC"]);
                        filas.Add(item);
                    }
                }
            }
            catch
            {
                throw;
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
        #endregion

    }
}
