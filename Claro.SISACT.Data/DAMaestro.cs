using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class DAMaestro
    {
        GeneradorLog objLog = new GeneradorLog("    DAMaestro  ", null, null, "DATA_LOG_DAMaestro");
        public Hashtable ListarItemsGenericos(int[] tablas)
        {
            /*
                Lista de tablas 
                -------------------
                1: TIPO DE DOCUMENTO
                2: Tipo Cliente
                3: APN
                4: Prefijo
                5: Departamento
                6; Score
                7: Actividad comercial
                8: Promoción
                9: Plazo Equipo
                10: Tipo Operacion
                11: Almacen
                12: Tipo Interior
                13:	Tipo Edificacion
                14:	Lista Urbanizacion
                15: Tipo Zona
                16: Tipo Domicilio
             */
            Hashtable salida = new Hashtable();
            foreach (int i in tablas)
            {
                switch (i)
                {
                    case 1: // TIPO DE DOCUMENTO
                        salida.Add(i, ListaTipoDocumento("R"));
                        break;
                    case 2: // Tipo Cliente
                        salida.Add(i, ListaTipoCliente(0));
                        break;
                    case 3: // APN
                        salida.Add(i, ListaAPN());
                        break;
                    case 4: // Prefijo
                        salida.Add(i, ListaPrefijo("01", "A"));
                        break;
                    case 5: // Departamento
                        salida.Add(i, ListaDepartamento("00", "A"));
                        break;
                    case 6: // Score
                        salida.Add(i, ListaScore());
                        break;
                    case 7:
                        salida.Add(i, ListaActividadComercial());
                        break;
                    case 8:
                        salida.Add(i, ListaModalidadCampanna("02"));
                        break;
                    case 9:
                        salida.Add(i, ListaPlazoEquipo());
                        break;
                    case 10:
                        salida.Add(i, ListaTipoOperacion());
                        break;
                    case 11:
                        salida.Add(i, ListaAlmacen());
                        break;
                    case 12:
                        salida.Add(i, ListaTipoInterior());
                        break;
                    case 13:    //Tipo Edificacion
                        salida.Add(i, ListaTipoEdificacion());
                        break;
                    case 14:	//Lista Urbanizacion
                        salida.Add(i, ListaUrbanizacion());
                        break;
                    case 15:	//Tipo Zona
                        salida.Add(i, ListaTipoZona());
                        break;
                    case 16:	//Tipo Domicilio
                        salida.Add(i, ListaTipoDomicilio());
                        break;
                }
            }
            return salida;
        }

        public ArrayList ListaTipoDocumento(string flag_ruc)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_FLAG_CON", DbType.String,1,ParameterDirection.Input),												   
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (flag_ruc != "") arrParam[0].Value = flag_ruc;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACT_CON_TIPO_DOC";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["TDOCC_CODIGO"]);
                    item.Descripcion = Funciones.CheckStr(dr["TDOCV_DESCRIPCION"]);
                    filas.Add(item);
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
            return filas;
        }

        public static List<BEItemGenerico> ListTipoDocumento(string pruc)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_FLAG_CON", DbType.String,1,ParameterDirection.Input),												   
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
											   };

            foreach (DAABRequest.Parameter t in arrParam)
                t.Value = DBNull.Value;

            if (pruc != "") arrParam[0].Value = pruc;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACT_CON_TIPO_DOC";
            obRequest.Parameters.AddRange(arrParam);

            var filas = new List<BEItemGenerico>();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico
                    {
                        Codigo = Funciones.CheckStr(dr["TDOCC_CODIGO"]),
                        Descripcion = Funciones.CheckStr(dr["TDOCV_DESCRIPCION"]),
                        Codigo2 = Funciones.CheckStr(dr["ID_BSCS"]),
                        Codigo3 = Funciones.CheckStr(dr["ID_CCLUB"]),
                        Codigo4 = Funciones.CheckStr(dr["ID_INFOCORP"]),
                        Codigo5 = Funciones.CheckStr(dr["ID_ABDCP"])
                    };
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

        public ArrayList ListaTipoCliente(int P_TCLIN_CODIGO)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_TCLIN_CODIGO", DbType.Int32,ParameterDirection.Input),
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (P_TCLIN_CODIGO > 0) arrParam[0].Value = P_TCLIN_CODIGO;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACT_CON_TIPO_CLIENTE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["TCLIN_CODIGO"]);
                    item.Descripcion = Funciones.CheckStr(dr["TCLIV_DESCRIPCION"]);
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

        public ArrayList ListaScore()
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACT_CON_SCORE";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["SCREC_CODIGO"]);
                    item.Descripcion = item.Codigo;
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

        public ArrayList ListaAPN()
        {
            return ListaTablaGeneral("A");
        }

        public ArrayList ListaModalidadCampanna(string producto)
        {
            DAABRequest.Parameter[] arrParam = {												   
                new DAABRequest.Parameter("P_TPROC_CODIGO", DbType.String,3,ParameterDirection.Input),
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };

            arrParam[0].Value = DBNull.Value;
            if (producto != null && producto != "") arrParam[0].Value = producto;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACT_CON_CAMPANA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["CAMPN_CODIGO"]);
                    item.Descripcion = Funciones.CheckStr(dr["CAMPV_DESCRIPCION"]);
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

        public ArrayList ListaPlazoEquipo()
        {
            return ListaTablaGeneral("E");
        }

        public ArrayList ListaTipoOperacion()
        {
            return ListaTablaGeneralSISACT("O");
        }

        public ArrayList ListaAlmacen()
        {
            return ListaTablaGeneralSISACT("A");
        }

        public ArrayList ListaTablaGeneral(string tipo)
        {
            DAABRequest.Parameter[] arrParam = {												   
                new DAABRequest.Parameter("P_DETC_TIPO", DbType.String,2,ParameterDirection.Input),												   
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };
            arrParam[0].Value = tipo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACT_CON_DET_TABLAS";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["DETN_CODIGO"]);
                    item.Descripcion = Funciones.CheckStr(dr["DETV_DESCRIPCION"]);
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

        public ArrayList ListaTablaGeneralSISACT(string tipo)
        {
            DAABRequest.Parameter[] arrParam = {												   
                new DAABRequest.Parameter("P_TABLN_TIPO", DbType.String,2,ParameterDirection.Input),												   
                new DAABRequest.Parameter("P_TABLN_ESTADO", DbType.String,1,ParameterDirection.Input),
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };
            arrParam[0].Value = tipo;
            arrParam[1].Value = "1";

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACT_CON_TABLA_TABLAS";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["TABLN_CODIGO"]);
                    item.Descripcion = Funciones.CheckStr(dr["TABLN_DESCRIPCION"]);
                    item.Descripcion2 = Funciones.CheckStr(dr["TABLN_CODIGO"]) + " - " + Funciones.CheckStr(dr["TABLN_DESCRIPCION"]);
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

        public ArrayList ListaPrefijo(string prefijo, string estado)
        {
            DAABRequest.Parameter[] arrParam = {												   
                new DAABRequest.Parameter("K_PRE_DIRECCION", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (!prefijo.Equals("")) arrParam[0].Value = prefijo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACT_CON_PREFIJO_DIR";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["PDIRC_CODIGO"]);
                    item.Codigo2 = Funciones.CheckStr(dr["PDIRV_ABREVIATURA"]);
                    item.Descripcion = Funciones.CheckStr(dr["PDIRV_DESCRIPCION2"]);
                    item.Descripcion2 = Funciones.CheckStr(dr["PDIRV_ABREVIATURA"]) + " - " + Funciones.CheckStr(dr["PDIRV_DESCRIPCION2"]);
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

        public ArrayList ListaDepartamento(string cod_dpto, string estado)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("K_COD_DEPARTAMENTO", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("K_ESTADO", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (!cod_dpto.Equals("")) arrParam[0].Value = cod_dpto;
            if (!estado.Equals("")) arrParam[1].Value = estado;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSecpMaestros + ".SECSS_CON_DEPARTAMENTO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEDepartamento item = new BEDepartamento();
                    item.DEPAC_CODIGO = Funciones.CheckStr(dr["DEPAC_CODIGO"]);
                    item.DEPAV_DESCRIPCION = Funciones.CheckStr(dr["DEPAV_DESCRIPCION"]);
                    item.DEPAV_COD_CIU = Funciones.CheckStr(dr["DEPAV_COD_CIU"]);
                    item.DEPAC_ESTADO = Funciones.CheckStr(dr["DEPAC_ESTADO"]);
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

        public ArrayList ListaProvincia(string cod_provincia, string cod_dpto, string estado)
        {
            DAABRequest.Parameter[] arrParam = {												   
                new DAABRequest.Parameter("K_COD_PROVINCIA", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("K_COD_DEPARTAMENTO", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("K_ESTADO", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (!cod_provincia.Equals("")) arrParam[0].Value = cod_provincia;
            if (!cod_dpto.Equals("")) arrParam[1].Value = cod_dpto;
            if (!estado.Equals("")) arrParam[2].Value = estado;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactSolicitud + ".SECSS_CON_PROVINCIA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEProvincia item = new BEProvincia();
                    item.PROVC_CODIGO = Funciones.CheckStr(dr["PROVC_CODIGO"]);
                    item.DEPAC_CODIGO = Funciones.CheckStr(dr["DEPAC_CODIGO"]);
                    item.PROVV_DESCRIPCION = Funciones.CheckStr(dr["PROVV_DESCRIPCION"]);
                    item.PROVC_ESTADO = Funciones.CheckStr(dr["PROVC_ESTADO"]);
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

        public ArrayList ListaDistrito(string cod_distrio, string cod_provincia, string cod_dpto, string estado)
        {
            DAABRequest.Parameter[] arrParam = {												   
                new DAABRequest.Parameter("K_COD_DISTRITO", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("K_COD_PROVINCIA", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("K_COD_DEPARTAMENTO", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("K_ESTADO", DbType.String,20,ParameterDirection.Input),
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (!cod_distrio.Equals("")) arrParam[0].Value = cod_distrio;
            if (!cod_provincia.Equals("")) arrParam[1].Value = cod_provincia;
            if (!cod_dpto.Equals("")) arrParam[2].Value = cod_dpto;
            if (!estado.Equals("")) arrParam[3].Value = estado;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactSolicitud + ".SECSS_CON_DISTRITO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEDistrito item = new BEDistrito();
                    item.DISTC_CODIGO = Funciones.CheckStr(dr["DISTC_CODIGO"]);
                    item.DEPAC_CODIGO = Funciones.CheckStr(dr["DEPAC_CODIGO"]);
                    item.PROVC_CODIGO = Funciones.CheckStr(dr["PROVC_CODIGO"]);
                    item.DISTC_CODIGO_POSTAL = Funciones.CheckStr(dr["DISTC_CODIGO_POSTAL"]);
                    item.DISTV_DESCRIPCION = Funciones.CheckStr(dr["DISTV_DESCRIPCION"]);
                    item.DISTC_ESTADO = Funciones.CheckStr(dr["DISTC_ESTADO"]);
                    item.ALMACEN = Funciones.CheckStr(dr["DISTC_ALMACEN"]);
                    if (estado == "A1")
                    {
                        item.UBIGEO_INEI = Funciones.CheckStr(dr["UBIGEO_INEI"]);
                    }
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

        public ArrayList ListaActividadComercial()
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactConsultas + ".SISACT_CON_ACTIVIDAD_COMERCIAL";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["ACOMV_CODIGO"]);
                    item.Descripcion = Funciones.CheckStr(dr["ACOMV_DESCRIPCION"]);
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

        public ArrayList ListaParametros(Int64 codigo)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_PARAN_CODIGO", DbType.Int64,ParameterDirection.Input),												   
                new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
            };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (codigo > 0) arrParam[0].Value = codigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PkgSisactConsultas + ".SECSS_CON_PARAMETRO";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEParametro item = new BEParametro();
                    item.Codigo = Funciones.CheckInt64(dr["PARAN_CODIGO"]);
                    item.Valor = Funciones.CheckStr(dr["PARAV_VALOR"]);
                    item.Valor1 = Funciones.CheckStr(dr["PARAV_VALOR1"]);
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

        public ArrayList ListaTipoInterior()
        {
            return ListaTablaGeneralSISACT("D");
        }

        public ArrayList ListaTipoEdificacion()
        {
            return ListaTablaGeneralSISACT("M");
        }

        public ArrayList ListaUrbanizacion()
        {
            return ListaTablaGeneralSISACT("U");
        }

        public ArrayList ListaTipoZona()
        {
            return ListaTablaGeneralSISACT("Z");
        }

        public ArrayList ListaTipoDomicilio()
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_CONSULTA", DbType.Object,ParameterDirection.Output)
            };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".MANTSS_LISTAR_TIDOM";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["CODIGO"]);
                    item.Descripcion = Funciones.CheckStr(dr["DESCRIPCION"]);
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

        public DataTable ObtenerDatosRegistroTiempo(Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("pTiean_codsec", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_TATENCION + ".P_DATOS_SEC";
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
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }

        public bool RegistroTiempoPoolEval(Int64 nroSEC, string pCodpdv, string pCodcanal, string pUsupdv, string pAnaeval, string pFech_inievasec, string pFech_finevasec, string pFlag_revasec)
        {
            objLog.CrearArchivolog("[ENTRADA][RegistroTiempoPoolEval]", null, null);
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("K_RESULTADO" ,DbType.Int64,ParameterDirection.Output),
				new DAABRequest.Parameter("pTiean_codsec" ,DbType.Double,ParameterDirection.Input),        
				new DAABRequest.Parameter("pTieac_codpdv" ,DbType.String,4,ParameterDirection.Input),        
				new DAABRequest.Parameter("pTieac_codcanal" ,DbType.String,2,ParameterDirection.Input),      
				new DAABRequest.Parameter("pTieav_usupdv" ,DbType.String,20,ParameterDirection.Input),        
				new DAABRequest.Parameter("pTieav_anaeval" ,DbType.String,20,ParameterDirection.Input),       
				new DAABRequest.Parameter("pTiead_fech_inievasec" ,DbType.DateTime,ParameterDirection.Input),
				new DAABRequest.Parameter("pTiead_fech_finevasec" ,DbType.DateTime,ParameterDirection.Input),
				new DAABRequest.Parameter("pTieac_flag_revasec" ,DbType.String,1,ParameterDirection.Input)       
			};

            arrParam[0].Value = DBNull.Value;
            arrParam[1].Value = nroSEC;
            arrParam[2].Value = pCodpdv;
            arrParam[3].Value = pCodcanal;
            arrParam[4].Value = pUsupdv;
            arrParam[5].Value = pAnaeval;
            arrParam[6].Value = pFech_inievasec;
            arrParam[7].Value = pFech_finevasec;
            arrParam[8].Value = pFlag_revasec;

            objLog.CrearArchivolog("[RegistroTiempoPoolEval][PARAMETRO nroSEC] ", Funciones.CheckStr(nroSEC), null);
            objLog.CrearArchivolog("[RegistroTiempoPoolEval][PARAMETRO pCodpdv] ", Funciones.CheckStr(pCodpdv), null);
            objLog.CrearArchivolog("[RegistroTiempoPoolEval][PARAMETRO pCodcanal] ", Funciones.CheckStr(pCodcanal), null);
            objLog.CrearArchivolog("[RegistroTiempoPoolEval][PARAMETRO pUsupdv] ", Funciones.CheckStr(pUsupdv), null);
            objLog.CrearArchivolog("[RegistroTiempoPoolEval][PARAMETRO pAnaeval] ", Funciones.CheckStr(pAnaeval), null);
            objLog.CrearArchivolog("[RegistroTiempoPoolEval][PARAMETRO pFech_inievasec] ", Funciones.CheckStr(pFech_inievasec), null);
            objLog.CrearArchivolog("[RegistroTiempoPoolEval][PARAMETRO pFech_finevasec] ", Funciones.CheckStr(pFech_finevasec), null);
            objLog.CrearArchivolog("[RegistroTiempoPoolEval][PARAMETRO pFlag_revasec] ", Funciones.CheckStr(pFlag_revasec), null);

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_TATENCION + ".P_EVALUACION_SEC";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;
            objRequest.Transactional = true;
            objLog.CrearArchivolog("[RegistroTiempoPoolEval][PACKAGE] ", BaseDatos.PKG_SISACT_TATENCION + ".P_EVALUACION_SEC", null);

            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                objRequest.Factory.CommitTransaction();

                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[0];
                objLog.CrearArchivolog("[RegistroTiempoPoolEval][parSalida1]", Funciones.CheckStr(parSalida1.Value), null);
                if (Funciones.CheckInt(parSalida1.Value).Equals(1))
                    salida = true;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[Exception][RegistroTiempoPoolEval]", ex.Message, null);
                objRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("[RegistroTiempoPoolEval][salida] ", Funciones.CheckStr(salida) , null);
                objLog.CrearArchivolog("[Fin][RegistroTiempoPoolEval]", null, null);
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public bool RegistroTiempoActivaIni(Int64 nroSEC, string pCodpdv, string pCodcanal, string pUsupdv, string pFech_iniactsec)
        {
            objLog.CrearArchivolog("[ENTRADA][RegistroTiempoActivaIni]", null, null);
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("K_RESULTADO" ,DbType.Int64,ParameterDirection.Output),
				new DAABRequest.Parameter("pTiean_codsec" ,DbType.Double,ParameterDirection.Input),        
				new DAABRequest.Parameter("pTieac_codpdv" ,DbType.String,4,ParameterDirection.Input),        
				new DAABRequest.Parameter("pTieac_codcanal" ,DbType.String,2,ParameterDirection.Input),      
				new DAABRequest.Parameter("pTieav_usupdv" ,DbType.String,20,ParameterDirection.Input),        
				new DAABRequest.Parameter("pTiead_fech_iniactsec" ,DbType.DateTime,ParameterDirection.Input)												   
			};

            arrParam[0].Value = DBNull.Value;
            arrParam[1].Value = nroSEC;
            arrParam[2].Value = pCodpdv;
            arrParam[3].Value = pCodcanal;
            arrParam[4].Value = pUsupdv;
            arrParam[5].Value = pFech_iniactsec;

            objLog.CrearArchivolog("[RegistroTiempoActivaIni][PARAMETRO nroSEC] ", Funciones.CheckStr(nroSEC), null);
            objLog.CrearArchivolog("[RegistroTiempoActivaIni][PARAMETRO pCodpdv] ", Funciones.CheckStr(pCodpdv), null);
            objLog.CrearArchivolog("[RegistroTiempoActivaIni][PARAMETRO pCodcanal] ", Funciones.CheckStr(pCodcanal), null);
            objLog.CrearArchivolog("[RegistroTiempoActivaIni][PARAMETRO pUsupdv] ", Funciones.CheckStr(pUsupdv), null);
            objLog.CrearArchivolog("[RegistroTiempoActivaIni][PARAMETRO pFech_iniactsec] ", Funciones.CheckStr(pFech_iniactsec), null);

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_TATENCION + ".P_ACTIVACION_SEC_I";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;
            objRequest.Transactional = true;

            objLog.CrearArchivolog("[RegistroTiempoActivaIni][PACKAGE] ", BaseDatos.PKG_SISACT_TATENCION + ".P_ACTIVACION_SEC_I", null);

            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                objRequest.Factory.CommitTransaction();

                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[0];
                objLog.CrearArchivolog("[RegistroTiempoActivaIni][parSalida1]", Funciones.CheckStr(parSalida1.Value), null);
                if (Funciones.CheckInt(parSalida1.Value).Equals(1))
                    salida = true;

            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[Exception][RegistroTiempoActivaIni]", ex.Message, null);
                objRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                objLog.CrearArchivolog("[RegistroTiempoActivaIni][salida] ", Funciones.CheckStr(salida), null);
                objLog.CrearArchivolog("[Fin][RegistroTiempoActivaIni]", null, null);
                objRequest.Factory.Dispose();
            }
            return salida;
        }
        //PROY-29121-INI
        public List<BEItemGenerico> ListaParametroslst(Int64 codigo)
        {
            DAABRequest.Parameter[] arrParam = {   new DAABRequest.Parameter("P_PARAN_CODIGO", DbType.Int64,ParameterDirection.Input),												   
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
											   };
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (codigo > 0) arrParam[0].Value = codigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PkgSisactConsultas + ".SECSS_CON_PARAMETRO";
            obRequest.Parameters.AddRange(arrParam);

            List<BEItemGenerico> filas = new List<BEItemGenerico>();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();

                    item.Codigo = Funciones.CheckStr(dr["PARAN_CODIGO"]);
                    item.Valor = Funciones.CheckStr(dr["PARAV_VALOR"]);
                    item.Valor1 = Funciones.CheckStr(dr["PARAV_VALOR1"]);
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
//INI PROY-31948_Migracion
        public static bool GrabarRegHorasPoll(Int64 pCodsec, string pAnaeval, string pFech_finevasec, string pOperacion)
        {
            DAABRequest.Parameter[] arrParam = {
			    new DAABRequest.Parameter("P_TIEAN_CODSEC", DbType.Int64,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAC_CODPDV", DbType.String,4,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAC_CODCANAL", DbType.String,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAV_USUPDV", DbType.String,20,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAD_FECH_INIREGSEC", DbType.DateTime,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAD_FECH_FINREGSEC", DbType.DateTime,ParameterDirection.Input),												  
			    new DAABRequest.Parameter("P_TIEAV_ANAEVAL", DbType.String,20,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAD_FECH_INIEVASEC", DbType.DateTime,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAD_FECH_FINEVASEC", DbType.DateTime,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAV_ANAACT", DbType.String,20,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAD_FECH_INIACTSEC", DbType.DateTime,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAD_FECH_FINACTSEC", DbType.DateTime,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAV_ANADES", DbType.String,20,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAD_FECH_INIDESSEC", DbType.DateTime,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TIEAD_FECH_FINDESSEC", DbType.DateTime,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_OPERACION", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("K_RESULTADO", DbType.Int64,ParameterDirection.Output)
            };

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;
            
            arrParam[0].Value = pCodsec;                        
            arrParam[6].Value = pAnaeval;
            arrParam[8].Value = pFech_finevasec;
            arrParam[15].Value = pOperacion;
            
            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest();
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_TATENCION + ".InsertarRegHorasPoll";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;
            int bb;

            try
            {
                bb = Convert.ToInt16(objRequest.Factory.ExecuteScalar(ref objRequest));
                objRequest.Factory.CommitTransaction();
            }
            catch (Exception ex)
            {
                objRequest.Factory.RollBackTransaction();

                throw ex;
            }
            finally
            {
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[16];
                if (Funciones.CheckInt(parSalida1.Value).Equals(1))
                salida = true;
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public static BEUsuario ObtenerUsuario(Int64 cod_usuario)
        {

            DAABRequest.Parameter[] arrParam = {   new DAABRequest.Parameter("K_USUAN_CODIGO", DbType.Int64,ParameterDirection.Input),												   
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
											   };

        
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (cod_usuario > 0) arrParam[0].Value = cod_usuario;
        

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_CONSULTAS + ".SECSS_DET_USUARIO";
            obRequest.Parameters.AddRange(arrParam);
        
            BEUsuario item = new BEUsuario();
            IDataReader dr = null;
            try
            {
        
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    item.UsuarioId = Funciones.CheckInt64(dr["USUAN_CODIGO"]);                    
                    item.CtaRed = Funciones.CheckStr(dr["USUAC_CTARED"]);                    
                    item.Telefono = Funciones.CheckStr(dr["USUAV_TELEFONO"]);
                    item.Nombre = Funciones.CheckStr(dr["USUAV_NOMBRE"]);
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
            return item;
        }
        //FIN PROY-31948_Migracion

        //Inicio Proy-140397 LVR
        public BEValidarMultipunto ListaValidarMultipunto(string cod_canal)
        {
            BEValidarMultipunto response = new BEValidarMultipunto();
            DAABRequest.Parameter[] arrParam = { new DAABRequest.Parameter("PI_OFICINA", DbType.String,ParameterDirection.Input), 
                                                 new DAABRequest.Parameter("PO_CURSOR_CANALES",DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("PO_CURSOR_DATOS",DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("PO_CURSOR_OFICINAS",DbType.Object,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("PO_COD",DbType.String,ParameterDirection.Output),
                                                 new DAABRequest.Parameter("PO_MSG",DbType.String,ParameterDirection.Output)
                                               };
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = cod_canal;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PkgMantConvergente + ".SISACTSS_MANMULTIPUNTO";
            obRequest.Parameters.AddRange(arrParam);
            var Lista1 = new List<BEItemGenerico>();
            var Lista2 = new List<BEMultipunto>();
            var Lista3 = new List<BEPuntoVenta>();
            DataSet ds;
            try
            {
                ds = obRequest.Factory.ExecuteDataset(ref obRequest);

                foreach (DataRow dr1 in ds.Tables[0].Rows)
                {
                    BEItemGenerico fila = new BEItemGenerico
                    {
                        Codigo = Funciones.CheckStr(dr1["TOFIC_CODIGO"]),
                        Descripcion = Funciones.CheckStr(dr1["TOFIV_DESCRIPCION"]),
                     
                    };
                    Lista1.Add(fila);
                }
                response.Canales = Lista1;

                foreach (DataRow dr2 in ds.Tables[1].Rows)
                {
                    BEMultipunto fila = new BEMultipunto
                    {
                        N_IDOFICINA = Funciones.CheckInt(dr2["N_IDOFICINA"]),
                        V_OFICINA_ORIGEN = Funciones.CheckStr(dr2["V_OFICINA_ORIGEN"]),
                        OVENV_DESCRIPCION = Funciones.CheckStr(dr2["OVENV_DESCRIPCION"]),
                        CANAC_CODIGO = Funciones.CheckStr(dr2["CANAC_CODIGO"]),
                        TOFIC_CODIGO = Funciones.CheckStr(dr2["TOFIC_CODIGO"]),
                        OVENC_REGION = Funciones.CheckStr(dr2["OVENC_REGION"]),
                        C_FLAGEVALUACION = Funciones.CheckStr(dr2["C_FLAGEVALUACION"]),
                        C_FLAGVENTA = Funciones.CheckStr(dr2["C_FLAGVENTA"]),
                        C_ESTADO = Funciones.CheckStr(dr2["C_ESTADO"])

                    };
                    Lista2.Add(fila);
                }
                response.Datos = Lista2;

                foreach (DataRow dr3 in ds.Tables[2].Rows)
                {
                    BEPuntoVenta fila = new BEPuntoVenta
                    {
                        OvencCodigo = Funciones.CheckStr(dr3["OVENC_CODIGO"]),
                        OvenvDescripcion = Funciones.CheckStr(dr3["OVENV_DESCRIPCION"]),
                        ToficCodigo = Funciones.CheckStr(dr3["TOFIC_CODIGO"]),
                        CanacCodigo = Funciones.CheckStr(dr3["CANAC_CODIGO"]),
                        OvencRegion = Funciones.CheckStr(dr3["OVENC_REGION"])
                      
                    };
                    Lista3.Add(fila);
                }
                response.Oficinas = Lista3;
            }
            catch (Exception e)
            {
               
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
                throw;
            }
            finally
            {
                
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return response;
        }
        //Fin Proy-140397 LVR

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Metodo para consultar las campañas existentes para las ventas varias]

        public List<BEItemGenerico> ConsultaCampanaXTipoVenta(BEParametrosMSSAP oParamMSSAP)
        {
            DAABRequest.Parameter[] arrParam = {
												new DAABRequest.Parameter("P_CAMPV_CODIGO", DbType.String, ParameterDirection.Input),
                                                new DAABRequest.Parameter("P_CAMPV_DESCRIPCION", DbType.String, ParameterDirection.Input),
                                                new DAABRequest.Parameter("P_CAMPV_TIPO_VENTA", DbType.String, ParameterDirection.Input),
                                                new DAABRequest.Parameter("P_CAMPC_ESTADO", DbType.String, ParameterDirection.Input),                                                
                                                new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
											   };

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0;
            arrParam[i].Value = oParamMSSAP.CodigoGenerico;
            i++; arrParam[i].Value = oParamMSSAP.DescripcionGenerico;
            i++; arrParam[i].Value = oParamMSSAP.TipoVenta;
            i++; arrParam[i].Value = oParamMSSAP.EstadoGenerico;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PkgMantConvergente + ".SP_CON_CAMPANHAS_TIPO_VENTA";
            obRequest.Parameters.AddRange(arrParam);

            List<BEItemGenerico> filas = new List<BEItemGenerico>();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["CAMPV_CODIGO"]);
                    item.Descripcion = Funciones.CheckStr(dr["CAMPV_DESCRIPCION"]);
                    item.Codigo2 = Funciones.CheckStr(dr["CAMPV_CODIGO_SAP"]);
                    item.Codigo3 = Funciones.CheckStr(dr["CAMPV_TIPO_VENTA"]);
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

        #endregion

    }
}
