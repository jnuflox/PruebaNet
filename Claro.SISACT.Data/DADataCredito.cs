using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class DADataCredito
    {
        public List<BEItemGenerico> ListarRespuestaDC()
        {
            DAABRequest.Parameter[] arrParam = { 
                new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output)
            };

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_LISTAR_RPTA_DC";
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
                    objItem.Codigo = Funciones.CheckStr(dr["RDCV_TIPO_RESP_CODIGO"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["RDCV_DESCRIPCION"]);
                    objItem.Estado = Funciones.CheckStr(dr["RDCC_ESTADO"]);
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

        public string AsignarBuroCrediticio(string tipoDocumento, ref string strUrl, ref string strKey)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_DOCUMENTO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ID_BURO", DbType.Int16, ParameterDirection.Output),
				new DAABRequest.Parameter("P_URL", DbType.String, 150, ParameterDirection.Output),
				new DAABRequest.Parameter("P_KEY", DbType.String, 10, ParameterDirection.Output)
            };
            arrParam[0].Value = tipoDocumento;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_BURO_CREDITICIO + ".SP_ID_BURO_CONSULTA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            string idBuro;
            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter p1, p2, p3;
                p1 = (IDataParameter)objRequest.Parameters[1];
                p2 = (IDataParameter)objRequest.Parameters[2];
                p3 = (IDataParameter)objRequest.Parameters[3];
                idBuro = Funciones.CheckStr(p1.Value);
                strUrl = Funciones.CheckStr(p2.Value);
                strKey = Funciones.CheckStr(p3.Value);
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
            return idBuro;
        }

        //PROY-24740
        public BEDataCreditoOUT ConsultarDCRepositorioPersona(BEDataCreditoIN objIN, string tipoSEC, ref BEItemMensaje objMensaje)
        {
            DAABRequest.Parameter[] arrParam = {												   
				new DAABRequest.Parameter("P_istrTipoDocumento", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrNumeroDocumento", DbType.String, 11, ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrApellidoPaterno", DbType.String, 30, ParameterDirection.Input),												
				new DAABRequest.Parameter("P_TipoSEC", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_DiasAntiguedad", DbType.Int32, ParameterDirection.Output),
				new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output)
			};
            int i = 0; arrParam[i].Value = objIN.istrTipoDocumento;
            i++; arrParam[i].Value = objIN.istrNumeroDocumento;
            i++; arrParam[i].Value = objIN.istrAPELLIDOPATERNO;
            i++; arrParam[i].Value = tipoSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objIN.istrNumeroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_HISTORICO_DC + ".SISACT_LISTAR_DATOS_DC";
            objRequest.Parameters.AddRange(arrParam);

            BEDataCreditoOUT objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;

                while (dr.Read())
                {
                    objItem = new BEDataCreditoOUT()
                    {
                        NUMEROOPERACION = Funciones.CheckStr(dr["NUMERO_OPERACION"]),
                        ACCION = Funciones.CheckStr(dr["ACCION"]),
                        LIMITE_DE_CREDITO = Funciones.CheckDbl(dr["LIMITE_DE_CREDITO"]),
                        LC_DISPONIBLE = Funciones.CheckDbl(dr["LC_DISPONIBLE"]),
                        NV_LC_MAXIMO = Funciones.CheckDbl(dr["NV_LC_MAXIMO"]),
                        NV_TOTAL_CARGOS_FIJOS = Funciones.CheckDbl(dr["NV_TOTAL_CARGOS_FIJOS"]),
                        SCORE = Funciones.CheckInt(dr["SCORE"]),
                        CREDIT_SCORE = Funciones.CheckStr(dr["CREDIT_SCORE"]),
                        TIPO_DE_PRODUCTO = Funciones.CheckStr(dr["TIPO_DE_PRODUCTO"]),
                        INGRESO_O_LC = Funciones.CheckDbl(dr["INGRESO_O_LC"]),
                        TIPO_DE_CLIENTE = Funciones.CheckStr(dr["TIPO_DE_CLIENTE"]),
                        TIPO_DE_TARJETA = Funciones.CheckStr(dr["TIPO_DE_TARJETA"]),
                        NUMERO_DOCUMENTO = Funciones.CheckStr(dr["NUMERO_DOCUMENTO"]),
                        APELLIDO_PATERNO = Funciones.CheckStr(dr["APELLIDO_PATERNO"]),
                        APELLIDO_MATERNO = Funciones.CheckStr(dr["APELLIDO_MATERNO"]),
                        NOMBRES = Funciones.CheckStr(dr["NOMBRES"]),
                        RESPUESTA = Funciones.CheckStr(dr["RESPUESTA"]),
                        ESTADOCIVIL = Funciones.CheckStr(dr["ISRTESTADOCIVIL"]),
                        TOP_10000 = Funciones.CheckStr(dr["TOP_1000"]),
                        RAZONES = Funciones.CheckStr(dr["RAZONES"]),
                        ANALISIS = Funciones.CheckStr(dr["ANALISIS"]),
                        SCORE_RANKIN_OPERATIVO = Funciones.CheckStr(dr["SCORE_RANKIN_OPERATIVO"]),
                        NUMERO_ENTIDADES_RANKIN_OPERATIVO = Funciones.CheckStr(dr["NUM_ENT_RANKIN_OPERATIVO"]),
                        PUNTAJE = Funciones.CheckDbl(dr["PUNTAJE"]),
                        LIMITECREDITODATACREDITO = Funciones.CheckDbl(dr["LIMITECREDITODATACREDITO"]),
                        LIMITECREDITOBASEEXTERNA = Funciones.CheckDbl(dr["LIMITECREDITOBASEEXTERNA"]),
                        LIMITECREDITOCLARO = Funciones.CheckDbl(dr["LIMITECREDITOCLARO"]),
                        FECHANACIMIENTO = Funciones.CheckStr(dr["FECHANACIMIENTO"]),
                        CODIGOBURO = Funciones.CheckInt(dr["BURO_CREDITICIO"]),
                        FUENTECONSULTA = BEDataCreditoOUT.FUENTE_CONSULTA.REPOSITORIO.ToString()
                        //gaa20170215
                        ,
                        BUROCONSULTADO = Funciones.CheckStr(dr["BURO_CREDITICIO"])
                        //fin gaa20170215
                    };
                    break;
                }
            }
            catch (Exception ex)
            {
                objMensaje = new BEItemMensaje(ex.Source, ex.Message, false);
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objItem;
        }

        //PROY-24740
        public BEDataCreditoCorpOUT ConsultarDCRepositorioEmpresa(BEDataCreditoCorpIN objIN)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_TIPO_DOC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_RUC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_SEC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_HISTORIAL_DC", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("K_CUR_HISTORIAL_REPLEG", DbType.Object, ParameterDirection.Output)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objIN.istrTipoDocumento.Substring(objIN.istrTipoDocumento.Length - 1, 1);
            arrParam[1].Value = objIN.istrNumeroDocumento;
            arrParam[2].Value = objIN.istrTipoSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objIN.istrNumeroDocumento);
            string[] sTab = { "historico", "representantes" };
            objRequest.TableNames = sTab;
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_HISTORICO_DC + ".SPS_OBTENER_HISTORICO_DC_CORP";
            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            BEDataCreditoCorpOUT objItem = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                
                while (dr.Read())
                {
                    objItem = new BEDataCreditoCorpOUT()
                {
                        ws53_Out_Header_CodigoRetorno = Funciones.CheckStr(dr["COD_RETORNO"]),
                        ws12_Out_Header_NumeroOperacion = Funciones.CheckStr(dr["NUM_OPERACION"]),
                        ws50_Out_GrupoIntegrantes_IntegranteNombres = Funciones.CheckStr(dr["RAZON_SOCIAL"]),
                        ws50_Out_CampoValor_Accion = Funciones.CheckStr(dr["RIESGO"]),
                        ws12_In_Nombres = Funciones.CheckStr(dr["NOMBRES"]),
                        ws12_In_ApellidoPaterno = Funciones.CheckStr(dr["APE_PAT"]),
                        ws12_In_ApellidoMaterno = Funciones.CheckStr(dr["APE_MAT"]),
                        ws50_Out_CampoValor_MsgIngTarjeta = Funciones.CheckStr(dr["MSG_ING_TAR"]),
                        ws50_Out_CampoValor_MsgIngDHipotecaria = Funciones.CheckStr(dr["MSG_ING_DHIP"]),
                    ws50_Out_CampoValor_MsgIngDnHipoTarjeta = Funciones.CheckStr(dr["MSG_ING_DNHIPO_TAR"]),
                    buro_consultado = Funciones.CheckInt(dr["BURO_CREDITICIO"]) //ADD PROY-20054-IDEA-23849
                    };
                    break;
                }

                // Lista Documentos
                List<BETipoDocumento> objListaDocumento = (new DAGeneral()).ListarTipoDocumento();

                dr.NextResult();

                // Para los representantes legal
                List<BERepresentanteLegal> objLista = new List<BERepresentanteLegal>();

                string strNombreCompletoRRLL = string.Empty;
                string strApePaterno = string.Empty;
                string strApeMaterno = string.Empty;
                string strNombre = string.Empty;
                    string[] arrNombre;

                if (objItem != null)
                {
                    while (dr.Read())
                    {
                        BERepresentanteLegal oRepresentanteLegal = new BERepresentanteLegal();
                        oRepresentanteLegal.APODC_TIP_DOC_REP = Funciones.CheckStr(dr["TIPO_DOCUMENTO"]);
                        oRepresentanteLegal.APODV_NUM_DOC_REP = Funciones.CheckStr(dr["NUMERO_DOCUMENTO"]);


                        //INICIO|PROY-20054
                        if (oRepresentanteLegal.APODC_TIP_DOC_REP == "" || oRepresentanteLegal.APODC_TIP_DOC_REP == null)
                        {
                            oRepresentanteLegal.APODC_TIP_DOC_REP = "1";
                        }
                        //FIN|PROY-20054

                        var descripcionRep =  objListaDocumento.Where(w => w.ID_DC == oRepresentanteLegal.APODC_TIP_DOC_REP).FirstOrDefault();
                        oRepresentanteLegal.TDOCV_DESCRIPCION_REP = descripcionRep == null ? string.Empty : descripcionRep.DESCRIPCION;

                        strNombreCompletoRRLL = Funciones.CheckStr(dr["NOMBRES"]);
                        arrNombre = strNombreCompletoRRLL.Split(' ');
                        strApePaterno = string.Empty;
                        strApeMaterno = string.Empty;
                        strNombre = string.Empty;
                        if (arrNombre.Length == 3)
                        {
                            strApePaterno = arrNombre[0];
                            strApeMaterno = arrNombre[1];
                            strNombre = arrNombre[2];
                        }
                        else
                        {
                            if (arrNombre.Length > 3)
                            {
                                strApePaterno = arrNombre[0];
                                strApeMaterno = arrNombre[1];
                                strNombre = string.Empty;
                                for (int z = 2; z < arrNombre.Length; z++)
                                {
                                    strNombre = string.Format("{0} {1}", strNombre.Trim(), arrNombre[z]);
                                }
                                strNombre = strNombre.Trim();
                            }
                        }
                        oRepresentanteLegal.APODV_APA_REP_LEG = strApePaterno;
                        oRepresentanteLegal.APODV_AMA_REP_LEG = strApeMaterno;
                        oRepresentanteLegal.APODV_NOM_REP_LEG = strNombre;
                        oRepresentanteLegal.APODV_CAR_REP = Funciones.CheckStr(dr["CARGO"]);
                        objLista.Add(oRepresentanteLegal);
                    }

                    objItem.oRepresentantesLegal = new List<BERepresentanteLegal>(objLista);
                }

            }
            finally
            {
                if (dr !=null && dr.IsClosed == false)  dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }

            return objItem;
        }

        public BEItemMensaje GrabarDatosDCPersona(BEDataCreditoIN objIN, BEDataCreditoOUT objOUT)
        {
            DAABRequest.Parameter[] arrParam =
			{
				new DAABRequest.Parameter("P_istrSecuencia", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrTipoDocumento",DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrNumeroDocumento",DbType.String,11,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrApellidoPaterno",DbType.String,50,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrApellidoMaterno",DbType.String,30,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrNombres",DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrTipoProducto",DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrTipoCliente",DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrNumOperaPVU",DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrRegion",DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrArea",DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_istrPuntoVenta",DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NUMOPERA_EFT",DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NUMERO_OPERACION",DbType.String,15,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ACCION",DbType.String,30,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LIMITE_DE_CREDITO",DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LC_DISPONIBLE",DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NV_LC_MAXIMO",DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NV_TOTAL_CARGOS_FIJOS",DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SCORE",DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CREDIT_SCORE",DbType.String,6,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DE_PRODUCTO",DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_INGRESO_O_LC",DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DE_CLIENTE",DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DE_TARJETA",DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NUMERO_DOCUMENTO",DbType.String,11,ParameterDirection.Input),
				new DAABRequest.Parameter("P_APELLIDO_PATERNO",DbType.String,30,ParameterDirection.Input),
				new DAABRequest.Parameter("P_APELLIDO_MATERNO",DbType.String,30,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NOMBRES",DbType.String,30,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RESPUESTA",DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ISRTESTADOCIVIL",DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TOP_1000",DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RAZONES",DbType.String,50,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ANALISIS",DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SCORE_RANKIN_OPERATIVO",DbType.String,30,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NUM_ENT_RANKIN_OPERATIVO",DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PUNTAJE",DbType.String,30,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LIMITECREDITODATACREDITO",DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LIMITECREDITOBASEEXTERNA",DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LIMITECREDITOCLARO",DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FECHANACIMIENTO",DbType.Date,ParameterDirection.Input),
				new DAABRequest.Parameter("P_BURO_CREDITICIO",DbType.Int16,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RETORNO", DbType.Int32,ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objIN.istrSecuencia;
            arrParam[1].Value = objIN.istrTipoDocumento;
            arrParam[2].Value = objIN.istrNumeroDocumento;
            arrParam[3].Value = objIN.istrAPELLIDOPATERNO;
            arrParam[4].Value = objIN.istrAPELLIDOMATERNO;
            arrParam[5].Value = objIN.istrNOMBRES;
            arrParam[6].Value = objIN.istrTIPOPRODUCTO;
            arrParam[7].Value = objIN.istrTIPOCLIENTE;
            arrParam[8].Value = objIN.istrNumOperaPVU;
            arrParam[9].Value = objIN.istrRegion;
            arrParam[10].Value = objIN.istrArea;
            arrParam[11].Value = objIN.istrPuntoVenta;
            arrParam[12].Value = objIN.ostrNumOperaEFT;
            arrParam[13].Value = objOUT.NUMEROOPERACION;
            arrParam[14].Value = objOUT.ACCION;
            arrParam[15].Value = objOUT.LIMITE_DE_CREDITO;
            arrParam[16].Value = objOUT.LC_DISPONIBLE;
            arrParam[17].Value = objOUT.NV_LC_MAXIMO;
            arrParam[18].Value = objOUT.NV_TOTAL_CARGOS_FIJOS;
            arrParam[19].Value = objOUT.SCORE;
            arrParam[20].Value = objOUT.CREDIT_SCORE;
            arrParam[21].Value = objOUT.TIPO_DE_PRODUCTO;
            arrParam[22].Value = objOUT.INGRESO_O_LC;
            arrParam[23].Value = objOUT.TIPO_DE_CLIENTE;
            arrParam[24].Value = objOUT.TIPO_DE_TARJETA;
            arrParam[25].Value = objOUT.NUMERO_DOCUMENTO;
            arrParam[26].Value = objOUT.APELLIDO_PATERNO;
            arrParam[27].Value = objOUT.APELLIDO_MATERNO;
            arrParam[28].Value = objOUT.NOMBRES;
            arrParam[29].Value = objOUT.RESPUESTA;
            arrParam[30].Value = objIN.istrEstadoCivil;
            arrParam[31].Value = objOUT.TOP_10000;
            arrParam[32].Value = objOUT.RAZONES;
            arrParam[33].Value = objOUT.ANALISIS;
            arrParam[34].Value = objOUT.SCORE_RANKIN_OPERATIVO;
            arrParam[35].Value = objOUT.NUMERO_ENTIDADES_RANKIN_OPERATIVO;
            arrParam[36].Value = objOUT.PUNTAJE;
            arrParam[37].Value = objOUT.LIMITECREDITODATACREDITO;
            arrParam[38].Value = objOUT.LIMITECREDITOBASEEXTERNA;
            arrParam[39].Value = objOUT.LIMITECREDITOCLARO;
            arrParam[40].Value = objOUT.FECHANACIMIENTO;
            arrParam[41].Value = objOUT.CODIGOBURO;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objIN.istrNumeroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_HISTORICO_DC + ".SISACT_INSERTAR_DATOS_DC";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            int intDatoRetorno = 0;
            BEItemMensaje objMensaje = new BEItemMensaje();
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter pSalida;
                pSalida = (IDataParameter)objRequest.Parameters[objRequest.Parameters.Count - 1];
                intDatoRetorno = Funciones.CheckInt(pSalida.Value);

                objMensaje.exito = (intDatoRetorno > 0);
            }
            catch (Exception ex)
            {
                objMensaje = new BEItemMensaje(ex.Source, ex.Message, false);
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objMensaje;
        }

        public BEItemMensaje GrabarDatosDCEmpresa(BEDataCreditoCorpOUT objItem)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("K_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_WS12_IN_TIPDOC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_IN_NUMDOC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_IN_APEPAT", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_IN_APEMAT", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_IN_NOM", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_IN_TIPPER", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_IN_TIPSER", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_OUT_HEADER_TRA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_OUT_HEADER_TIPSER", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_OUT_HEADER_CODRET", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_OUT_HEADER_NUMOPE", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_OUT_ERROR_CODMEN", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS12_OUT_ERROR_MEN", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_IN_TIPSER", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_IN_NUMOPE", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_HEADER_TRA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_HEADER_TIPSER", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_HEADER_CODRET", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_HEADER_NUMOPE", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_GI_INTTIPDOC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_GI_INTNUMDOC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_GI_INTNOM", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CN_ACCION", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CECAMPO_ACCION", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CV_ACCION", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CN_MSGINGTAR", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CEC_MSGINGTAR", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CV_MSGINGTAR", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CN_MSGINGDHIP", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CEC_MSGINGDHIP", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CV_MSGINGDHIP", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CN_MSGINGDNHIPOTAR", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CEC_MSGINGDNHIPOTAR", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CV_MSGINGDNHIPOTAR", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CN_EXP", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CEC_EXP", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CV_EXP", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CN_EXPAUX", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CEC_EXPAUX", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS50_OUT_CV_EXPAUX", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_IN_TIPSER", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_IN_NUMOPE", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_OUT_HEADER_TRA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_OUT_HEADER_TIPSER", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_OUT_HEADER_CODRET", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_OUT_HEADER_NUMOPE", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_BURO_CREDITICIO", DbType.Int64, ParameterDirection.Input), //ADD: PROY-20054-IDEA-23849
		new DAABRequest.Parameter("P_WS53_OUT_TIPCONTRIBUYENTE", DbType.String, ParameterDirection.Input), //ADD: INI-PROY-32438 IDEA-41376
                new DAABRequest.Parameter("P_WS53_OUT_NOMCOMERCIAL", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_WS53_OUT_FEC_INIACTIVIDADES", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_WS53_OUT_ESTCONTRIBUYENTE", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_WS53_OUT_CONDCONTRIBUYENTE", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_WS53_OUT_CIIUCONTRIBUYENTE", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_WS53_OUT_CANTTRABAJADORES", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_WS53_OUT_EMISIONCOMP", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_WS53_OUT_SIST_EMIELECTRONICA", DbType.String, ParameterDirection.Input) //ADD: FIN-PROY-32438 IDEA-41376
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = objItem.ws12_In_TipoDocumento;
            ++i; arrParam[i].Value = objItem.ws12_In_NumeroDocumento;
            ++i; if (!Funciones.CheckStr(objItem.ws12_In_ApellidoPaterno).Equals("")) arrParam[i].Value = objItem.ws12_In_ApellidoPaterno;
            ++i; if (!Funciones.CheckStr(objItem.ws12_In_ApellidoMaterno).Equals("")) arrParam[i].Value = objItem.ws12_In_ApellidoMaterno;
            ++i; if (!Funciones.CheckStr(objItem.ws12_In_Nombres).Equals("")) arrParam[i].Value = objItem.ws12_In_Nombres;
            ++i; arrParam[i].Value = objItem.ws12_In_TipoPersona;
            ++i; arrParam[i].Value = objItem.ws12_In_TipoServicio;
            ++i; arrParam[i].Value = objItem.ws12_Out_Header_Transaccion;
            ++i; arrParam[i].Value = objItem.ws12_Out_Header_TipoServicio;
            ++i; arrParam[i].Value = objItem.ws12_Out_Header_CodigoRetorno;
            ++i; arrParam[i].Value = objItem.ws12_Out_Header_NumeroOperacion;
            ++i; arrParam[i].Value = objItem.ws12_Out_Error_CodigoMensajes;
            ++i; arrParam[i].Value = objItem.ws12_Out_Error_Mensaje;
            ++i; arrParam[i].Value = objItem.ws50_In_TipoServicio;
            ++i; arrParam[i].Value = objItem.ws50_In_NumeroOperacion;
            ++i; arrParam[i].Value = objItem.ws50_Out_Header_Transaccion;
            ++i; arrParam[i].Value = objItem.ws50_Out_Header_TipoServicio;
            ++i; arrParam[i].Value = objItem.ws50_Out_Header_CodigoRetorno;
            ++i; arrParam[i].Value = objItem.ws50_Out_Header_NumeroOperacion;
            ++i; arrParam[i].Value = objItem.ws50_Out_GrupoIntegrantes_IntegranteTipoDocumento;
            ++i; arrParam[i].Value = objItem.ws50_Out_GrupoIntegrantes_IntegranteNumeroDocumento;
            ++i; arrParam[i].Value = objItem.ws50_Out_GrupoIntegrantes_IntegranteNombres;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoNombre_Accion;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoExisteCampo_Accion;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoValor_Accion;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoNombre_MsgIngTarjeta;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoExisteCampo_MsgIngTarjeta;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoValor_MsgIngTarjeta;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoNombre_MsgIngDHipotecaria;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoExisteCampo_MsgIngDHipotecaria;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoValor_MsgIngDHipotecaria;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoNombre_MsgIngDnHipoTarjeta;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoExisteCampo_MsgIngDnHipoTarjeta;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoValor_MsgIngDnHipoTarjeta;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoNombre_Explicacion;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoExisteCampo_Explicacion;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoValor_Explicacion;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoNombre_ExplicacionAuxiliar;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoExisteCampo_ExplicacionAuxiliar;
            ++i; arrParam[i].Value = objItem.ws50_Out_CampoValor_ExplicacionAuxiliar;
            ++i; arrParam[i].Value = objItem.ws53_In_TipoServicio;
            ++i; arrParam[i].Value = objItem.ws53_In_NumeroOperacion;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_Transaccion;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_TipoServicio;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_CodigoRetorno;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_NumeroOperacion;
            ++i; arrParam[i].Value = objItem.buro_consultado;//ADD: PROY-20054-IDEA-23849
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_TipContribuyente;//ADD: INI-PROY-32438 IDEA-41376
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_NomComercial;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_FecIniActividades;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_EstContribuyente;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_CondContribuyente;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_CiiuContribuyente;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_CantTrabajadores;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_EmisionComp;
            ++i; arrParam[i].Value = objItem.ws53_Out_Header_SistEmielectronica;//ADD: FIN-PROY-32438 IDEA-41376

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objItem.ws12_In_NumeroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_HISTORICO_DC + ".SPI_GRABAR_HISTORICO_DC_CORP";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;

            BEItemMensaje objMensaje = new BEItemMensaje();
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                if (objItem.oRepresentantesLegalDC != null)
                {
                    foreach (BERepresentanteLegalDC oItem in objItem.oRepresentantesLegalDC)
                    {
                        GrabarDCRepresentanteLegal(oItem);
                    }
                }
            }
            catch (Exception ex)
            {
                objMensaje.exito = false;
                objMensaje.mensajeCliente = "Error al Insertar Historial de Consulta a Data Crédito Corp.";
                objMensaje.mensajeSistema = ex.Message;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }

            return objMensaje;
        }

        public BEItemMensaje GrabarDCRepresentanteLegal(BERepresentanteLegalDC objItem)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("K_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_WS12_IN_NUMOPE", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_OUT_RL_TIPPER", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_OUT_RL_TIPDOC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_OUT_RL_NUMDOC", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_OUT_RL_NOM", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_OUT_RL_CAR", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_WS53_OUT_RL_INI_CAR", DbType.String, ParameterDirection.Input) //ADD: PROY-32438 IDEA-41376
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = objItem.ws53_In_NumeroOperacionRepLeg;
            ++i; arrParam[i].Value = objItem.ws53_Out_RepresentanteLegalTipoPersona;
            ++i; arrParam[i].Value = objItem.ws53_Out_RepresentanteLegalTipoDocumento;
            ++i; arrParam[i].Value = objItem.ws53_Out_RepresentanteLegalNumeroDocumento;
            ++i; arrParam[i].Value = objItem.ws53_Out_RepresentanteLegalNombre;
            ++i; arrParam[i].Value = objItem.ws53_Out_RepresentanteLegalCargo;
            ++i; arrParam[i].Value = "";//ADD: FIN-PROY-32438 IDEA-41376

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_HISTORICO_DC + ".SPI_GRABAR_HIST_DC_RL_CORP";
            objRequest.Parameters.AddRange(arrParam);

            BEItemMensaje objMensaje = new BEItemMensaje();
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
            }
            catch (Exception ex)
            {
                objMensaje.exito = false;
                objMensaje.mensajeCliente = "Error al Insertar Historial de Representantes Legales de Data Crédito Corp.";
                objMensaje.mensajeSistema = ex.Message;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return objMensaje;
        }

        public BEItemMensaje GrabarDCInputOutput(BEDataCreditoINOUT objDC)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("K_RESULTADO", DbType.Int64,ParameterDirection.Output),
				new DAABRequest.Parameter("P_NUM_OPERACION", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_INPUT_VALORES", DbType.String,4000,ParameterDirection.Input),
				new DAABRequest.Parameter("P_OUTPUT_VALORES", DbType.String,4000,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_DOCUMENTO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NUM_DOCUMENTO", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_USUARIO", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_COD_PUNTO_VENTA", DbType.String,5,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FORMA_PAGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_ACTIVACION", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_CLIENTE", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_VENTA", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAZO_ACUERDO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAN1", DbType.String,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAN2", DbType.String,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLAN3", DbType.String,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CONTROL_CONSUMO", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESSALUD", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SUNAT", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RIESGO", DbType.String,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LIMITE_CREDITO", DbType.Double,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SCORE_TEXTO", DbType.String,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SCORE_NUMERO", DbType.String,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RESPUESTA_DC", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_APE_PATERNO", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_APE_MATERNO", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NOMBRES", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_UBIGEO", DbType.String,3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIPO_CLIENTE_DC", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESTADO_CIVIL_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ORIGEN_LC_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ANALISIS_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SCORE_RANKING_OPER_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PUNTAJE_DC", DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LC_DATA_CREDITO_DC", DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LC_BASE_EXTERNA_DC", DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LC_CLARO_DC", DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RAZONES_DC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FECHA_NACE_CLIENTE_DC", DbType.Date,ParameterDirection.Input),
				new DAABRequest.Parameter("P_BURO_CREDITICIO", DbType.Int64,ParameterDirection.Input //ADD PROY-20054-IDEA-23849    
                )		
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(objDC.IODCV_NUM_OPERACION)) arrParam[1].Value = objDC.IODCV_NUM_OPERACION;
            if (!string.IsNullOrEmpty(objDC.IODCV_INPUT_VALORES)) arrParam[2].Value = objDC.IODCV_INPUT_VALORES;
            if (!string.IsNullOrEmpty(objDC.IODCV_OUTPUT_VALORES)) arrParam[3].Value = objDC.IODCV_OUTPUT_VALORES;
            if (!string.IsNullOrEmpty(objDC.IODCV_TIPO_DOCUMENTO)) arrParam[4].Value = objDC.IODCV_TIPO_DOCUMENTO;
            if (!string.IsNullOrEmpty(objDC.IODCV_NUM_DOCUMENTO)) arrParam[5].Value = objDC.IODCV_NUM_DOCUMENTO;
            if (!string.IsNullOrEmpty(objDC.IODCV_USUARIO_REGISTRO)) arrParam[6].Value = objDC.IODCV_USUARIO_REGISTRO;
            if (!string.IsNullOrEmpty(objDC.IODCV_COD_PUNTO_VENTA)) arrParam[7].Value = objDC.IODCV_COD_PUNTO_VENTA;
            if (!string.IsNullOrEmpty(objDC.IODCC_FORMA_PAGO)) arrParam[8].Value = objDC.IODCC_FORMA_PAGO;
            if (!string.IsNullOrEmpty(objDC.IODCC_TIPO_ACTIVACION)) arrParam[9].Value = objDC.IODCC_TIPO_ACTIVACION;
            if (!string.IsNullOrEmpty(objDC.IODCC_TIPO_CLIENTE)) arrParam[10].Value = objDC.IODCC_TIPO_CLIENTE;
            if (!string.IsNullOrEmpty(objDC.IODCC_TIPO_VENTA)) arrParam[11].Value = objDC.IODCC_TIPO_VENTA;
            if (!string.IsNullOrEmpty(objDC.IODCC_PLAZO_ACUERDO)) arrParam[12].Value = objDC.IODCC_PLAZO_ACUERDO;
            if (!string.IsNullOrEmpty(objDC.IODCC_PLAN1)) arrParam[13].Value = objDC.IODCC_PLAN1;
            if (!string.IsNullOrEmpty(objDC.IODCC_PLAN2)) arrParam[14].Value = objDC.IODCC_PLAN2;
            if (!string.IsNullOrEmpty(objDC.IODCC_PLAN3)) arrParam[15].Value = objDC.IODCC_PLAN3;
            if (!string.IsNullOrEmpty(objDC.IODCC_CONTROL_CONSUMO)) arrParam[16].Value = objDC.IODCC_CONTROL_CONSUMO;
            if (!string.IsNullOrEmpty(objDC.IODCC_FLAG_ESSALUD)) arrParam[17].Value = objDC.IODCC_FLAG_ESSALUD;
            if (!string.IsNullOrEmpty(objDC.IODCC_FLAG_SUNAT)) arrParam[18].Value = objDC.IODCC_FLAG_SUNAT;
            if (!string.IsNullOrEmpty(objDC.IODCC_RIESGO)) arrParam[19].Value = objDC.IODCC_RIESGO;
            if (!string.IsNullOrEmpty(objDC.IODCC_LIMITE_CREDITO)) arrParam[20].Value = objDC.IODCC_LIMITE_CREDITO;
            if (!string.IsNullOrEmpty(objDC.IODCC_SCORE_TEXTO)) arrParam[21].Value = objDC.IODCC_SCORE_TEXTO;
            if (!string.IsNullOrEmpty(objDC.IODCC_SCORE_NUMERO)) arrParam[22].Value = objDC.IODCC_SCORE_NUMERO;
            if (!string.IsNullOrEmpty(objDC.IODCC_RESPUESTA_DC)) arrParam[23].Value = objDC.IODCC_RESPUESTA_DC;
            if (!string.IsNullOrEmpty(objDC.IODCV_APE_PATERNO)) arrParam[24].Value = objDC.IODCV_APE_PATERNO;
            if (!string.IsNullOrEmpty(objDC.IODCV_APE_MATERNO)) arrParam[25].Value = objDC.IODCV_APE_MATERNO;
            if (!string.IsNullOrEmpty(objDC.IODCV_NOMBRES)) arrParam[26].Value = objDC.IODCV_NOMBRES;
            if (!string.IsNullOrEmpty(objDC.IODCV_UBIGEO)) arrParam[27].Value = objDC.IODCV_UBIGEO;
            if (!string.IsNullOrEmpty(objDC.IODCC_TIPO_CLIENTE_DC)) arrParam[28].Value = objDC.IODCC_TIPO_CLIENTE_DC;
            if (!string.IsNullOrEmpty(objDC.IODCC_ESTADO_CIVIL_DC)) arrParam[29].Value = objDC.IODCC_ESTADO_CIVIL_DC;
            if (!string.IsNullOrEmpty(objDC.IODCC_ORIGEN_LC_DC)) arrParam[30].Value = objDC.IODCC_ORIGEN_LC_DC;
            if (!string.IsNullOrEmpty(objDC.IODCC_ANALISIS_DC)) arrParam[31].Value = objDC.IODCC_ANALISIS_DC;
            if (!string.IsNullOrEmpty(objDC.IODCC_SCORE_RANKING_OPER_DC)) arrParam[32].Value = objDC.IODCC_SCORE_RANKING_OPER_DC;
            arrParam[33].Value = objDC.IODCN_PUNTAJE_DC;
            arrParam[34].Value = objDC.IODCN_LC_DATA_CREDITO_DC;
            arrParam[35].Value = objDC.IODCN_LC_BASE_EXTERNA_DC;
            arrParam[36].Value = objDC.IODCN_LC_CLARO_DC;
            if (!string.IsNullOrEmpty(objDC.IODCC_RAZONES_DC)) arrParam[37].Value = objDC.IODCC_RAZONES_DC;
            if (!string.IsNullOrEmpty(objDC.IODCD_FECHA_NACE_CLIENTE_DC)) arrParam[38].Value = objDC.IODCD_FECHA_NACE_CLIENTE_DC;
            arrParam[39].Value = objDC.IODCC_CODIGOBURO; //ADD PROY-20054-IDEA-23849

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objDC.IODCV_NUM_DOCUMENTO);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_INSERTAR_VALORES_IODC1"; //ADD PROY-20054-IDEA-23849
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            BEItemMensaje objMensaje = new BEItemMensaje();
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[0];

                objMensaje.exito = (Funciones.CheckInt64(parSalida1.Value) != 0);
            }
            catch (Exception ex)
            {
                objMensaje = new BEItemMensaje(ex.Source, ex.Message, false);
            }
            finally
            {
                objRequest.Factory.Dispose();
                objRequest.Parameters.Clear();
            }
            return objMensaje;
        }

        public void GrabarDCHistorico(BEDataCreditoHistorico objItem)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_HISTV_NUM_OPERACION", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTC_TIPO_DOCUMENTO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTV_NUM_DOCUMENTO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTV_APELLIDO_PAT", DbType.String, 40, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTV_APELLIDO_MAT", DbType.String, 40, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTV_NOMBRE", DbType.String, 40, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTC_TIPO_RESPUESTA", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTC_TIPO_RIESGO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTN_CANT_INTENTOS", DbType.Int32, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTV_OVEN_CODIGO", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTV_TERMINAL_ID", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("P_HISTN_USUARIO_REG", DbType.String, ParameterDirection.Input)
            };
            int intCont = 0;
            for (intCont = 0; intCont < arrParam.Length; intCont++)
                arrParam[intCont].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(objItem.HISTV_NUM_OPERACION)) arrParam[0].Value = objItem.HISTV_NUM_OPERACION;
            if (!string.IsNullOrEmpty(objItem.HISTC_TIPO_DOCUMENTO)) arrParam[1].Value = objItem.HISTC_TIPO_DOCUMENTO;
            if (!string.IsNullOrEmpty(objItem.HISTV_NUM_DOCUMENTO)) arrParam[2].Value = objItem.HISTV_NUM_DOCUMENTO;
            if (!string.IsNullOrEmpty(objItem.HISTV_APELLIDO_PAT)) arrParam[3].Value = objItem.HISTV_APELLIDO_PAT;
            if (!string.IsNullOrEmpty(objItem.HISTV_APELLIDO_MAT)) arrParam[4].Value = objItem.HISTV_APELLIDO_MAT;
            if (!string.IsNullOrEmpty(objItem.HISTV_NOMBRE)) arrParam[5].Value = objItem.HISTV_NOMBRE;
            if (!string.IsNullOrEmpty(objItem.HISTC_TIPO_RESPUESTA)) arrParam[6].Value = objItem.HISTC_TIPO_RESPUESTA;
            if (!string.IsNullOrEmpty(objItem.HISTC_TIPO_RIESGO)) arrParam[7].Value = objItem.HISTC_TIPO_RIESGO;
            arrParam[8].Value = objItem.HISTN_CANT_INTENTOS;
            if (!string.IsNullOrEmpty(objItem.HISTV_OVEN_CODIGO)) arrParam[9].Value = objItem.HISTV_OVEN_CODIGO;
            if (!string.IsNullOrEmpty(objItem.HISTV_TERMINAL_ID)) arrParam[10].Value = objItem.HISTV_TERMINAL_ID;
            if (!string.IsNullOrEmpty(objItem.HISTN_USUARIO_REG)) arrParam[11].Value = objItem.HISTN_USUARIO_REG;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objItem.HISTV_NUM_DOCUMENTO);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_INSERTAR_HISTORICO_DC";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

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

        public bool GrabarRazonesEvaluacion(Int64 nroSEC, string strNodo)
        {
            DAABRequest.Parameter[] arrParam = {												   
				new DAABRequest.Parameter("P_SRDCN_SOLIN_CODIGO",DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SRDCN_COD_RAZON_NODO",DbType.String,ParameterDirection.Input)																					   
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSEC;
            arrParam[1].Value = strNodo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_INS_RAZONES_EVALUACION";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            bool salida = false;

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

        public bool GrabarDCReporte(Vista_SolicitudDC_Reporte objItem)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_DCREV_NUM_OPERACION", DbType.String,ParameterDirection.Input),
		        new DAABRequest.Parameter("P_DCREN_SOLIN_CODIGO", DbType.String,ParameterDirection.Input),
		        new DAABRequest.Parameter("P_DCREV_OVEN_CODIGO", DbType.String,ParameterDirection.Input),
		        new DAABRequest.Parameter("P_DCREN_USUARIO_REG", DbType.String,ParameterDirection.Input),
		        new DAABRequest.Parameter("P_DCREC_TIPO_DOCUMENTO", DbType.String,ParameterDirection.Input),
		        new DAABRequest.Parameter("P_DCREV_NUM_DOCUMENTO", DbType.String,ParameterDirection.Input),
		        new DAABRequest.Parameter("P_DCREV_APELLIDO_PAT", DbType.String,ParameterDirection.Input),
		        new DAABRequest.Parameter("P_DCREV_APELLIDO_MAT", DbType.String,ParameterDirection.Input),
		        new DAABRequest.Parameter("P_DCREV_NOMBRE", DbType.String,ParameterDirection.Input),
		        new DAABRequest.Parameter("P_DCREN_CANT_INTENTOS", DbType.Int32,ParameterDirection.Input),
		        new DAABRequest.Parameter("P_DCREC_VALIDAR_CLIENTE", DbType.String,ParameterDirection.Input)
	        };
            int intCont = 0;
            for (intCont = 0; intCont < arrParam.Length; intCont++)
                arrParam[intCont].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(objItem.DCREV_NUM_OPERACION)) arrParam[0].Value = objItem.DCREV_NUM_OPERACION;
            arrParam[1].Value = objItem.DCREN_SOLIN_CODIGO;
            if (!string.IsNullOrEmpty(objItem.DCREV_OVEN_CODIGO)) arrParam[2].Value = objItem.DCREV_OVEN_CODIGO;
            if (!string.IsNullOrEmpty(objItem.DCREN_USUARIO_REG)) arrParam[3].Value = objItem.DCREN_USUARIO_REG;
            if (!string.IsNullOrEmpty(objItem.DCREC_TIPO_DOCUMENTO)) arrParam[4].Value = objItem.DCREC_TIPO_DOCUMENTO;
            if (!string.IsNullOrEmpty(objItem.DCREV_NUM_DOCUMENTO)) arrParam[5].Value = objItem.DCREV_NUM_DOCUMENTO;
            if (!string.IsNullOrEmpty(objItem.DCREV_APELLIDO_PAT)) arrParam[6].Value = objItem.DCREV_APELLIDO_PAT;
            if (!string.IsNullOrEmpty(objItem.DCREV_APELLIDO_MAT)) arrParam[7].Value = objItem.DCREV_APELLIDO_MAT;
            if (!string.IsNullOrEmpty(objItem.DCREV_NOMBRE)) arrParam[8].Value = objItem.DCREV_NOMBRE;
            if (!objItem.DCREN_CANT_INTENTOS.Equals(-1)) arrParam[9].Value = objItem.DCREN_CANT_INTENTOS;
            if (!string.IsNullOrEmpty(objItem.DCREC_VALIDAR_CLIENTE)) arrParam[10].Value = objItem.DCREC_VALIDAR_CLIENTE;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_INSERTAR_REPORTE_DC";
            objRequest.Parameters.AddRange(arrParam);
            bool salida = false;

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
                objRequest.Parameters.Clear();
            }
            return salida;
        }

        public bool ActualizarDCHistorico(string nroOperacion, string validarCliente)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_NUM_OPERACION", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("P_VALIDAR_CLIENTE", DbType.String,ParameterDirection.Input)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(nroOperacion)) arrParam[0].Value = nroOperacion;
            if (!string.IsNullOrEmpty(validarCliente)) arrParam[1].Value = validarCliente;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACT_UPD_VALIDAR_NOMBRES";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            bool salida = false;

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
                objRequest.Parameters.Clear();
            }
            return salida;
        }
    }
}
