using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSDataCreditoCorp;
using System.Xml;
using System.Data;
using System.Xml.Serialization;

namespace Claro.SISACT.WS
{
    class BWDataCreditoCorp
    {
        #region [Declaracion de Constantes - Config]

        string _Key, _User, _Pasw, _Url, _ModeloDc;
        string strTipoDoc = ConfigurationManager.AppSettings["constCodTipoDocumentoRUC"].ToString();
        string[] _ServsDc = new string[4];
        BEUsuarioSession objSession;
        GeneradorLog _objLog = null;
        string _idAplicacion = null;
        string _usuAplicacion = null;
        string _idTransaccion = null;

        #endregion [Declaracion de Constantes - Config]

        public BWDataCreditoCorp()
		{
            string idBuro = (new BLDataCredito()).AsignarBuroCrediticio(strTipoDoc, ref _Url, ref _Key);
            ConfigConexionDC(_Key);

            _idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _usuAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");
		}

        public BEEmpresaExperto ConsultaBuroCrediticio(BEDataCreditoCorpIN objIN, BEUsuarioSession objUsuario, ref BEItemMensaje objMensaje)
        {
            return ConsultaDataCredito(objIN, objUsuario, ref objMensaje);
        }

        private void ConfigConexionDC(string _Key)
        {
            //PROY-24740
            Claro.SISACT.Configuracion.ConfigConexionDC oConfigConexionDC = Claro.SISACT.Configuracion.ConfigConexionDC.GetInstance(_Key);
            _User = oConfigConexionDC.Parametros.Usuario;
            _Pasw = oConfigConexionDC.Parametros.Password;
            _ModeloDc = ConfigurationManager.AppSettings["DcCorpModelo"].ToString();
            _ServsDc[0] = ConfigurationManager.AppSettings["DcCorpServicio12"].ToString();
            _ServsDc[1] = ConfigurationManager.AppSettings["DcCorpServicio50"].ToString();
            _ServsDc[2] = ConfigurationManager.AppSettings["DcCorpServicio53"].ToString();
            _ServsDc[3] = "";
        }

        private BEEmpresaExperto ConsultaDataCredito(BEDataCreditoCorpIN objDataCreditoIN, BEUsuarioSession objUsuario, ref BEItemMensaje objMensaje)
        {
            BEEmpresaExperto rptaEmpresaDC = new BEEmpresaExperto();
            rptaEmpresaDC.strFlagInterno = string.Empty;
            rptaEmpresaDC.origen_experto = "DC";
            //INICIO PROY-140257
            BLEvaluacion objEvaluacion = new BLEvaluacion();
            //FIN PROY-140257
            objSession = objUsuario;
            BEDataCreditoCorpOUT objDCOUT = (new BLDataCredito()).ConsultarDCRepositorioEmpresa(objDataCreditoIN);
            List<BERepresentanteLegal> objListaRRLL = new List<BERepresentanteLegal>();
            BLDataCredito obj = new BLDataCredito();

            if (objDCOUT == null)
            {
                WS.WSDataCreditoCorp.ServicioClaroCorporativo _objTransaccion = new WS.WSDataCreditoCorp.ServicioClaroCorporativo();
                _objLog = new GeneradorLog(null, objDataCreditoIN.istrNumeroDocumento, _idTransaccion, "WEB");

                try
                {
                    // Declarando las variables que serán usadas para la obtención de datos
                    DataSet oDataSet;
                    DataTable oHeader;
                    DataTable oError;

                    string strCodigoRetorno = string.Empty;
                    string strNroOperacion = string.Empty;

                    string strNroOperacionws50 = string.Empty;
                    string strNroOperacionws53 = string.Empty;

                    string strCodError = string.Empty;
                    string strMsgError = string.Empty;

                    string strRazonSocial = string.Empty;
                    string strRiesgo = string.Empty;

                    // Lista Documentos
                    List<BETipoDocumento> objListaDocumento = (new BLGeneral()).ListarTipoDocumento();

                    if (objDataCreditoIN.istrTipoDocumento.Length == 2)
                    {
                        foreach (BETipoDocumento item in objListaDocumento)
                        {
                            if (item.ID_SISACT == objDataCreditoIN.istrTipoDocumento)
                            {
                                objDataCreditoIN.istrTipoDocumento = item.ID_DC;
                                break;
                            }
                        }
                    }

                    _objTransaccion.Url = _Url;
                    _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;

                    _objLog.CrearArchivologWS("INICIO WS DATACREDITO CORP", _objTransaccion.Url, objDataCreditoIN, null);

                    string rptaReturn = _objTransaccion.metodosReturn(objDataCreditoIN.istrTipoDocumento,
                                                                        objDataCreditoIN.istrNumeroDocumento,
                                                                        objDataCreditoIN.istrApellidoPaterno,
                                                                        objDataCreditoIN.istrApellidoMaterno,
                                                                        objDataCreditoIN.istrNombres,
                                                                        objDataCreditoIN.istrTipoPersona,
                                                                        _User, _Pasw, _ModeloDc);

                    string strRpta12 = System.Text.RegularExpressions.Regex.Split(rptaReturn, "RespuestaSVC12>")[1];
                    strRpta12 = strRpta12.Substring(0, strRpta12.Length - 2);
                    strRpta12 = "<RespuestaSVC12>" + strRpta12 + "</RespuestaSVC12>";
                    string strRpta50 = System.Text.RegularExpressions.Regex.Split(rptaReturn, "RespuestaSVC50>")[1];
                    strRpta50 = strRpta50.Substring(0, strRpta50.Length - 2);
                    strRpta50 = "<RespuestaSVC50>" + strRpta50 + "</RespuestaSVC50>";
                    string strRpta53 = System.Text.RegularExpressions.Regex.Split(rptaReturn, "RespuestaSVC53>")[1];
                    strRpta53 = strRpta53.Substring(0, strRpta53.Length - 2);
                    strRpta53 = "<RespuestaSVC53>" + strRpta53 + "</RespuestaSVC53>";

                    //_objLog.CrearArchivologWS("RETORNO WS DATACREDITO CORP", rptaReturn, null, null);

                    // INICIO SERVICIO 12
                    objDCOUT = new BEDataCreditoCorpOUT();
                    objDCOUT.ws12_In_TipoDocumento = objDataCreditoIN.istrTipoDocumento;
                    objDCOUT.ws12_In_NumeroDocumento = objDataCreditoIN.istrNumeroDocumento;
                    objDCOUT.ws12_In_TipoPersona = objDataCreditoIN.istrTipoPersona;
                    objDCOUT.ws12_In_TipoServicio = _ServsDc[0].ToString();

                    System.Xml.XmlDocument oXMLDocument12 = new System.Xml.XmlDocument();
                    oXMLDocument12.LoadXml(strRpta12);

                    oDataSet = new DataSet();
                    oDataSet.ReadXml(new System.Xml.XmlNodeReader(oXMLDocument12));

                    oHeader = new DataTable();
                    oError = new DataTable();

                    if (oDataSet.Tables.Count > 0)
                    {
                        for (int i = 0; i < oDataSet.Tables.Count; i++)
                        {
                            switch (oDataSet.Tables[i].TableName.ToUpper())
                            {
                                case "HEADER":
                                    oHeader = oDataSet.Tables[i];
                                    break;
                                case "ERROR":
                                    oError = oDataSet.Tables[i];
                                    break;
                            }
                        }
                    }

                    if (oError.Rows.Count > 0)
                    {
                        strCodError = oError.Rows[0]["CodigoMensajes"].ToString();
                        strMsgError = oError.Rows[0]["Mensaje"].ToString();

                        objDCOUT.ws12_Out_Error_CodigoMensajes = strCodError;
                        objDCOUT.ws12_Out_Error_Mensaje = strMsgError;

                    }
                    if (oHeader.Rows.Count > 0)
                    {
                        strNroOperacion = oHeader.Rows[0]["NumeroOperacion"].ToString();
                        strCodigoRetorno = oHeader.Rows[0]["CodigoRetorno"].ToString();

                        objDCOUT.ws12_Out_Header_Transaccion = oHeader.Rows[0]["Transaccion"].ToString();
                        objDCOUT.ws12_Out_Header_TipoServicio = _ServsDc[0].ToString(); ;

                        objDCOUT.ws12_Out_Header_CodigoRetorno = strCodigoRetorno;
                        objDCOUT.ws12_Out_Header_NumeroOperacion = strNroOperacion;

                        if (strCodigoRetorno != ConfigurationManager.AppSettings["DcCorpCodigoRetornoOk"].ToString())
                        {
                            rptaEmpresaDC.strCodError = strCodError;
                            rptaEmpresaDC.strMensajeError = strMsgError;
                            rptaEmpresaDC.strCodRetorno = strCodigoRetorno;
                            rptaEmpresaDC.strFlagInterno = "1";

                            objMensaje.exito = false;
                            objMensaje.mensajeCliente = "Error Consulta DataCredito Servicio 12";
                            return rptaEmpresaDC;
                        }
                    }

                    oHeader = null;
                    oError = null;
                    oDataSet = null;
                    // FIN SERVICIO 12

                    // INICIO SERVICIO 50
                    objDCOUT.ws50_In_TipoServicio = _ServsDc[1];
                    objDCOUT.ws50_In_NumeroOperacion = strNroOperacion;

                    System.Xml.XmlDocument oXMLDocument50 = new System.Xml.XmlDocument();
                    oXMLDocument50.LoadXml(strRpta50);

                    oDataSet = new DataSet();
                    oDataSet.ReadXml(new System.Xml.XmlNodeReader(oXMLDocument50));

                    DataTable oDatosCliente = new DataTable();
                    DataTable oEvaluacionCliente = new DataTable();

                    if (oDataSet.Tables.Count > 0)
                    {
                        for (int i = 0; i < oDataSet.Tables.Count; i++)
                        {
                            switch (oDataSet.Tables[i].TableName.ToUpper())
                            {
                                case "HEADER":
                                    oHeader = oDataSet.Tables[i];
                                    break;
                                case "INTEGRANTE":
                                    oDatosCliente = oDataSet.Tables[i];
                                    break;
                                case "CAMPO":
                                    oEvaluacionCliente = oDataSet.Tables[i];
                                    break;
                            }
                        }
                    }
                    if (oHeader.Rows.Count > 0)
                    {
                        strCodigoRetorno = oHeader.Rows[0]["CodigoRetorno"].ToString();
                        strNroOperacionws50 = oHeader.Rows[0]["NumeroOperacion"].ToString();

                        objDCOUT.ws50_Out_Header_Transaccion = oHeader.Rows[0]["Transaccion"].ToString();
                        objDCOUT.ws50_Out_Header_TipoServicio = oHeader.Rows[0]["TipoServicio"].ToString();

                        objDCOUT.ws50_Out_Header_CodigoRetorno = strCodigoRetorno;
                        objDCOUT.ws50_Out_Header_NumeroOperacion = strNroOperacionws50;

                        if (strCodigoRetorno != ConfigurationManager.AppSettings["DcCorpCodigoRetornoOk"].ToString())
                        {
                            rptaEmpresaDC.strCodRetorno = strCodigoRetorno;
                            rptaEmpresaDC.strFlagInterno = "2";

                            objMensaje.exito = false;
                            objMensaje.mensajeCliente = "Error Consulta DataCredito Servicio 50";
                            return rptaEmpresaDC;
                        }
                    }

                    if (oDatosCliente.Rows.Count > 0)
                    {
                        if (objDataCreditoIN.istrTipoPersona.Equals("J"))
                        {
                            strRazonSocial = oDatosCliente.Rows[0]["Nombres"].ToString().Trim().ToUpper();
                            if (strRazonSocial.Length > 40)
                            {
                                strRazonSocial = strRazonSocial.Substring(0, 40);
                            }
                        }
                        else
                        {
                            strRazonSocial = "";
                            string strNombreCompletoDC = "";
                            string strNombresDC = "";
                            string strApePatDC = "";
                            string strApeMatDC = "";
                            string[] arrNombresDC;

                            strNombreCompletoDC = oDatosCliente.Rows[0]["Nombres"].ToString().Trim().ToUpper();
                            strRazonSocial = strNombreCompletoDC;
                            if (strRazonSocial.Length > 40)
                            {
                                strRazonSocial = strRazonSocial.Substring(0, 40);
                            }

                            arrNombresDC = strNombreCompletoDC.Split(' ');
                            strApePatDC = "";
                            strApeMatDC = "";
                            strNombresDC = "";
                            if (arrNombresDC.Length == 3)
                            {
                                strApePatDC = arrNombresDC[0].Trim();
                                strApeMatDC = arrNombresDC[1].Trim();
                                strNombresDC = arrNombresDC[2].Trim();
                            }
                            else
                            {
                                if (arrNombresDC.Length > 3)
                                {
                                    strApePatDC = arrNombresDC[0].Trim();
                                    strApeMatDC = arrNombresDC[1].Trim();
                                    strNombresDC = "";
                                    for (int z = 2; z < arrNombresDC.Length; z++)
                                    {
                                        strNombresDC = strNombresDC.Trim() + " " + arrNombresDC[z];
                                    }
                                    strNombresDC = strNombresDC.Trim();
                                }
                                else
                                {
                                    strApePatDC = arrNombresDC[0].Trim();
                                    strNombresDC = arrNombresDC[1].Trim();
                                }
                            }

                            objDCOUT.ws12_In_ApellidoPaterno = strApePatDC;
                            objDCOUT.ws12_In_ApellidoMaterno = strApeMatDC;
                            objDCOUT.ws12_In_Nombres = strNombresDC;
                        }

                        objDCOUT.ws50_Out_GrupoIntegrantes_IntegranteTipoDocumento = oDatosCliente.Rows[0]["TipoDocumento"].ToString();
                        objDCOUT.ws50_Out_GrupoIntegrantes_IntegranteNumeroDocumento = oDatosCliente.Rows[0]["NumeroDocumento"].ToString();
                        objDCOUT.ws50_Out_GrupoIntegrantes_IntegranteNombres = oDatosCliente.Rows[0]["Nombres"].ToString().Trim().ToUpper();
                    }

                    string strConstAccion, strConstConcepto1, strConstConcepto2, strConstConcepto3;
                    strConstAccion = ConfigurationManager.AppSettings["DcCorpServ50Accion"].ToUpper();
                    strConstConcepto1 = ConfigurationManager.AppSettings["DcCorpServ50Campo1"].ToUpper();
                    strConstConcepto2 = ConfigurationManager.AppSettings["DcCorpServ50Campo2"].ToUpper();
                    strConstConcepto3 = ConfigurationManager.AppSettings["DcCorpServ50Campo3"].ToUpper();

                    string strConstExplicacion, strConstExplicacionAux;
                    strConstExplicacion = ConfigurationManager.AppSettings["DcCorpServ50CampoExplicacion"].ToUpper();
                    strConstExplicacionAux = ConfigurationManager.AppSettings["DcCorpServ50CampoExplicacionAuxiliar"].ToUpper(); ;

                    string montoConcepto1 = "";
                    string montoConcepto2 = "";
                    string montoConcepto3 = "";

                    if (oEvaluacionCliente.Rows.Count > 0)
                    {
                        //INICIO PROY-140257
                        //Declaración de variables 
                        string strCodGrupoParamRiesgo = string.Empty, tipoDoc = string.Empty, strCodRes = string.Empty, strMsjRespuesta = string.Empty, strCodRiesgo = string.Empty, msjErrorCodRiesgo = string.Empty, codRiesgoBaja = string.Empty, msjRiesgoBaja = string.Empty;
                        //FIN PROY-140257
                        for (int i = 0; i < oEvaluacionCliente.Rows.Count; i++)
                        {
                            string nombreCampo = Funciones.CheckStr(oEvaluacionCliente.Rows[i]["Nombre"]).ToUpper();
                            string existeCampo = Funciones.CheckStr(oEvaluacionCliente.Rows[i]["ExisteCampo"]).ToUpper();
                            string valor = Funciones.CheckStr(oEvaluacionCliente.Rows[i]["Valor"]).ToUpper();

                            if (nombreCampo.Equals(strConstAccion))
                            {
                                //INICIO PROY-140257
                                List<BEParametro> lstRiesgo = new List<BEParametro>();
                                _objLog.CrearArchivolog(string.Format("{0}", "-----------------------------------------------------------------------------------"), null, null);
                                _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWDataCreditoCorp][ConsultaDataCredito] [INICIO]---------------------"), null, null);
                                strCodGrupoParamRiesgo = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamRiesgo"]);
                                _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257][BWDataCreditoCorp][ConsultaDataCredito] strCodGrupoParamRiesgo", strCodGrupoParamRiesgo), null, null);
                                if (!string.IsNullOrEmpty(strCodGrupoParamRiesgo))
                                {
                                    lstRiesgo = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamRiesgo));
                                    _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257][BWDataCreditoCorp][ConsultaDataCredito] lstRiesgo.Count", lstRiesgo.Count), null, null);

                                    if (lstRiesgo.Count > 0)
                                    {
                                        _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWDataCreditoCorp][ConsultaDataCredito] ListaParametrosGrupo INI-----"), null, null);
                                        //se recupera el Tipo de Documento de la Key AppSettings["ConsCorporativo"] por defecto valor "C".
                                        tipoDoc = lstRiesgo.Where(p => p.Valor1 == "Key_ConsCorporativo").SingleOrDefault().Valor;
                                        _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | tipoDoc", tipoDoc), null, null);
                                        msjErrorCodRiesgo = lstRiesgo.Where(p => p.Valor1 == "Key_MsjErrorCodRiesgo").SingleOrDefault().Valor;
                                        _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | msjErrorCodRiesgo", msjErrorCodRiesgo), null, null);
                                        msjRiesgoBaja = lstRiesgo.Where(p => p.Valor1 == "key_MsjRiesgoBaja").SingleOrDefault().Valor;
                                        _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | msjRiesgoBaja", msjRiesgoBaja), null, null);
                                        codRiesgoBaja = lstRiesgo.Where(p => p.Valor1 == "key_CodRiesgoBaja").SingleOrDefault().Valor;
                                        _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | codRiesgoBaja", codRiesgoBaja), null, null);

                                        _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWDataCreditoCorp][ConsultaDataCredito] ListaParametrosGrupo FIN-----"), null, null);
                                    }
                                }
                                strRiesgo = valor.ToUpper();
                                _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] strRiesgo]", strRiesgo), null, null);

                                objEvaluacion.Consultar_Riesgo_Corp(tipoDoc, strRiesgo, ref strCodRiesgo, ref strCodRes, ref strMsjRespuesta);

                                _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWDataCreditoCorp][ConsultaDataCredito] Consultar_Riesgo_Corp INI----"), null, null);
                                _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] IN | tipoDoc", tipoDoc), null, null);
                                _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] IN | strCodRiesgo", strCodRiesgo), null, null);
                                _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | strCodRes", strCodRes), null, null);
                                _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | strMsjRespuesta", strMsjRespuesta), null, null);
                                _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWDataCreditoCorp][ConsultaDataCredito] Consultar_Riesgo_Corp FIN----"), null, null);

                                if (strCodRes.Equals("0"))
                                {
                                    strRiesgo = strCodRiesgo;
                                    _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] strRiesgo", strRiesgo), null, null);
                                    var arrRiesgo = codRiesgoBaja.Split(';');

                                    foreach (string item in arrRiesgo)
                                    {
                                        if (strRiesgo.Equals(item))
                                {
                                    rptaEmpresaDC.estado_ruc = "B";
                                    rptaEmpresaDC.strCodRetorno = strCodigoRetorno;
                                    rptaEmpresaDC.strNroOperacion = strNroOperacion;
                                    rptaEmpresaDC.strRazonSocial = strRazonSocial;
                                    rptaEmpresaDC.strRiesgo = strRiesgo;
                                    rptaEmpresaDC.strNombres = objDataCreditoIN.istrNombres;
                                    rptaEmpresaDC.strApellidoPaterno = objDataCreditoIN.istrApellidoPaterno;
                                    rptaEmpresaDC.strApellidoMaterno = objDataCreditoIN.istrApellidoMaterno;
                                    rptaEmpresaDC.strFlagInterno = "0";

                                    objMensaje.exito = false;
                                            objMensaje.mensajeCliente = msjRiesgoBaja;
                                            _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] objMensaje.mensajeCliente]", objMensaje.mensajeCliente), null, null);
                                    return rptaEmpresaDC;
                                }
                                    }
                                }
                                else
                                {
                                    objMensaje.exito = false;
                                    objMensaje.mensajeCliente = msjErrorCodRiesgo;
                                    _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] objMensaje.mensajeCliente]", objMensaje.mensajeCliente), null, null);
                                    rptaEmpresaDC.strCodError = "RIESGO";
                                    return rptaEmpresaDC;
                                }
                                _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWDataCreditoCorp][ConsultaDataCredito] FIN--------------------------"), null, null);
                                _objLog.CrearArchivolog(string.Format("{0}", "-----------------------------------------------------------------------------------"), null, null);
                                //FIN PROY-140257
                                objDCOUT.ws50_Out_CampoNombre_Accion = strConstAccion;
                                objDCOUT.ws50_Out_CampoExisteCampo_Accion = "0";
                                objDCOUT.ws50_Out_CampoValor_Accion = strRiesgo;
                            }
                            else if (nombreCampo.Equals(strConstConcepto1))
                            {
                                montoConcepto1 = valor.Replace("S/.", "").Trim();

                                objDCOUT.ws50_Out_CampoNombre_MsgIngTarjeta = strConstConcepto1;
                                objDCOUT.ws50_Out_CampoExisteCampo_MsgIngTarjeta = "0";
                                objDCOUT.ws50_Out_CampoValor_MsgIngTarjeta = montoConcepto1;
                            }
                            else if (nombreCampo.Equals(strConstConcepto2))
                            {
                                montoConcepto2 = valor.Replace("S/.", "").Trim();

                                objDCOUT.ws50_Out_CampoNombre_MsgIngDHipotecaria = strConstConcepto2;
                                objDCOUT.ws50_Out_CampoExisteCampo_MsgIngDHipotecaria = "0";
                                objDCOUT.ws50_Out_CampoValor_MsgIngDHipotecaria = montoConcepto2;
                            }
                            else if (nombreCampo.Equals(strConstConcepto3))
                            {
                                montoConcepto3 = valor.Replace("S/.", "").Trim();

                                objDCOUT.ws50_Out_CampoNombre_MsgIngDnHipoTarjeta = strConstConcepto3;
                                objDCOUT.ws50_Out_CampoExisteCampo_MsgIngDnHipoTarjeta = "0";
                                objDCOUT.ws50_Out_CampoValor_MsgIngDnHipoTarjeta = montoConcepto3;
                            }
                            else if (nombreCampo.Equals(strConstExplicacion))
                            {
                                objDCOUT.ws50_Out_CampoNombre_Explicacion = strConstExplicacion;
                                objDCOUT.ws50_Out_CampoExisteCampo_Explicacion = "0";
                                objDCOUT.ws50_Out_CampoValor_Explicacion = valor;
                            }
                            else if (nombreCampo.Equals(strConstExplicacionAux))
                            {
                                objDCOUT.ws50_Out_CampoNombre_ExplicacionAuxiliar = strConstExplicacionAux;
                                objDCOUT.ws50_Out_CampoExisteCampo_ExplicacionAuxiliar = "0";
                                objDCOUT.ws50_Out_CampoValor_ExplicacionAuxiliar = valor;
                            }
                        }
                    }
                    double monto1 = 0;
                    double monto2 = 0;
                    double monto3 = 0;

                    if (Funciones.isNumeric(montoConcepto1)) monto1 = Funciones.CheckDbl(montoConcepto1);
                    if (Funciones.isNumeric(montoConcepto2)) monto2 = Funciones.CheckDbl(montoConcepto2);
                    if (Funciones.isNumeric(montoConcepto3)) monto3 = Funciones.CheckDbl(montoConcepto3);

                    // Por regla se toma el Monto Mayor
                    double montoMayor = monto1;
                    if (monto2 > montoMayor) montoMayor = monto2;
                    if (monto3 > montoMayor) montoMayor = monto3;

                    rptaEmpresaDC.estado_ruc = "A";
                    rptaEmpresaDC.deuda_financiera = montoMayor;

                    oHeader = null;
                    oDataSet = null;

                    oEvaluacionCliente = null;
                    oDatosCliente = null;
                    // FIN SERVICIO 50

                    // INICIO SERVICIO 53
                    List<BERepresentanteLegalDC> objListaRepresentanteLegal = new List<BERepresentanteLegalDC>();

                    if (objDataCreditoIN.istrTipoPersona.Equals("J"))
                    {
                        objDCOUT.ws53_In_TipoServicio = _ServsDc[2].ToString();
                        objDCOUT.ws53_In_NumeroOperacion = strNroOperacion;

                        System.Xml.XmlDocument oXMLDocument53 = new System.Xml.XmlDocument();
                        oXMLDocument53.LoadXml(strRpta53);

                        oDataSet = new DataSet();
                        oDataSet.ReadXml(new System.Xml.XmlNodeReader(oXMLDocument53));

                        DataTable oDatosRRLL = new DataTable();

                        if (oDataSet.Tables.Count > 0)
                        {
                            for (int i = 0; i < oDataSet.Tables.Count; i++)
                            {
                                switch (oDataSet.Tables[i].TableName.ToUpper())
                                {
                                    case "HEADER":
                                        oHeader = oDataSet.Tables[i];
                                        break;
                                    case "REPRESENTANTELEGAL":
                                        oDatosRRLL = oDataSet.Tables[i];
                                        break;
                                }
                            }
                        }
                        if (oHeader.Rows.Count > 0)
                        {
                            strCodigoRetorno = oHeader.Rows[0]["CodigoRetorno"].ToString();
                            strNroOperacionws53 = oHeader.Rows[0]["NumeroOperacion"].ToString();

                            objDCOUT.ws53_Out_Header_Transaccion = oHeader.Rows[0]["Transaccion"].ToString();
                            objDCOUT.ws53_Out_Header_TipoServicio = _ServsDc[2].ToString();

                            objDCOUT.ws53_Out_Header_CodigoRetorno = strCodigoRetorno;
                            objDCOUT.ws53_Out_Header_NumeroOperacion = strNroOperacionws53;

                            if (strCodigoRetorno != ConfigurationManager.AppSettings["DcCorpCodigoRetornoOk"])
                            {
                                rptaEmpresaDC.strCodRetorno = strCodigoRetorno;
                                rptaEmpresaDC.strNroOperacion = strNroOperacion;
                                rptaEmpresaDC.strRazonSocial = strRazonSocial;
                                rptaEmpresaDC.strRiesgo = strRiesgo;
                                rptaEmpresaDC.strNombres = objDataCreditoIN.istrNombres;
                                rptaEmpresaDC.strApellidoPaterno = objDataCreditoIN.istrApellidoPaterno;
                                rptaEmpresaDC.strApellidoMaterno = objDataCreditoIN.istrApellidoMaterno;
                                rptaEmpresaDC.strFlagInterno = "3";

                                rptaEmpresaDC.oRepresentanteLegal = objListaRRLL;

                                objMensaje.exito = false;
                                objMensaje.mensajeCliente = "Error Consulta DataCredito Servicio 53";
                                return rptaEmpresaDC;
                            }
                        }

                        string strTipoDocumentoRRLL, strNumeroDocumentoRRLL, strCargo, strNombreCompletoRRLL, strNombre, strApePaterno, strApeMaterno;
                        string[] arrNombre;

                        if (oDatosRRLL.Rows.Count > 0)
                        {
                            for (int i = 0; i < oDatosRRLL.Rows.Count; i++)
                            {
                                strTipoDocumentoRRLL = oDatosRRLL.Rows[i]["TipoDocumento"].ToString();
                                strNumeroDocumentoRRLL = oDatosRRLL.Rows[i]["NumeroDocumento"].ToString();
                                strCargo = oDatosRRLL.Rows[i]["Cargo"].ToString();
                                strNombreCompletoRRLL = oDatosRRLL.Rows[i]["NombresRazonSocial"].ToString();
                                arrNombre = strNombreCompletoRRLL.Split(' ');
                                strApePaterno = "";
                                strApeMaterno = "";
                                strNombre = "";
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
                                        strNombre = "";
                                        for (int z = 2; z < arrNombre.Length; z++)
                                        {
                                            strNombre = strNombre.Trim() + " " + arrNombre[z];
                                        }
                                        strNombre = strNombre.Trim();
                                    }
                                }

                                BERepresentanteLegal oRepresentanteLegal = new BERepresentanteLegal();
                                oRepresentanteLegal.APODC_TIP_DOC_REP = strTipoDocumentoRRLL.Trim().ToUpper();
                                oRepresentanteLegal.APODV_NUM_DOC_REP = strNumeroDocumentoRRLL.Trim().ToUpper();
                                oRepresentanteLegal.APODV_APA_REP_LEG = strApePaterno.Trim().ToUpper();
                                oRepresentanteLegal.APODV_AMA_REP_LEG = strApeMaterno.Trim().ToUpper();
                                oRepresentanteLegal.APODV_NOM_REP_LEG = strNombre.Trim().ToUpper();
                                oRepresentanteLegal.APODV_CAR_REP = strCargo.Trim().ToUpper();

                                //INI-PROY-20054
                                var strDescripcionDocumentoRRLL = oRepresentanteLegal.APODC_TIP_DOC_REP;

                                _objLog.CrearArchivologWS("1. oRepresentanteLegal.APODC_TIP_DOC_REP", null, oRepresentanteLegal.APODC_TIP_DOC_REP, null);

                                oRepresentanteLegal.APODC_TIP_DOC_REP = Funciones.Right(("00" + strDescripcionDocumentoRRLL), 2);

                                if (oRepresentanteLegal.APODC_TIP_DOC_REP == "" || oRepresentanteLegal.APODC_TIP_DOC_REP == null)
                                {
                                    oRepresentanteLegal.APODC_TIP_DOC_REP = "01";
                                }
                                _objLog.CrearArchivologWS("2. oRepresentanteLegal.APODC_TIP_DOC_REP", null, oRepresentanteLegal.APODC_TIP_DOC_REP, null);
                                //FIN-PROY-20054

                                foreach (BETipoDocumento objDoc in objListaDocumento)
                                {
                                    if (objDoc.ID_DC == oRepresentanteLegal.APODC_TIP_DOC_REP)
                                    {
                                        oRepresentanteLegal.TDOCV_DESCRIPCION_REP = objDoc.DESCRIPCION;
                                        break;
                                    }
                                }

                                //INICIO|PROY-20054
                                _objLog.CrearArchivologWS("3. oRepresentanteLegal.TDOCV_DESCRIPCION_REP", null, oRepresentanteLegal.TDOCV_DESCRIPCION_REP, null);

                                if (oRepresentanteLegal.TDOCV_DESCRIPCION_REP == "" || oRepresentanteLegal.TDOCV_DESCRIPCION_REP == null)
                                {
                                    oRepresentanteLegal.TDOCV_DESCRIPCION_REP = "DNI";
                                }

                                _objLog.CrearArchivologWS("4. oRepresentanteLegal.TDOCV_DESCRIPCION_REP", null, oRepresentanteLegal.TDOCV_DESCRIPCION_REP, null);

                                //FIN|PROY-20054

                                objListaRRLL.Add(oRepresentanteLegal);

                                BERepresentanteLegalDC oRepresentanteLegalDC = new BERepresentanteLegalDC();
                                oRepresentanteLegalDC.ws53_In_NumeroOperacionRepLeg = strNroOperacion;
                                oRepresentanteLegalDC.ws53_Out_RepresentanteLegalTipoPersona = oDatosRRLL.Rows[i]["TipoPersona"].ToString();
                                oRepresentanteLegalDC.ws53_Out_RepresentanteLegalTipoDocumento = strTipoDocumentoRRLL;
                                oRepresentanteLegalDC.ws53_Out_RepresentanteLegalNumeroDocumento = strNumeroDocumentoRRLL;
                                oRepresentanteLegalDC.ws53_Out_RepresentanteLegalNombre = strNombreCompletoRRLL;
                                oRepresentanteLegalDC.ws53_Out_RepresentanteLegalCargo = strCargo;

                                objListaRepresentanteLegal.Add(oRepresentanteLegalDC);
                            }
                        }
                    }
                    else
                    {
                        string nroDocumentoDNI = objDataCreditoIN.istrNumeroDocumento.Substring(2, 8);
                        BERepresentanteLegal oRepresentanteLegal = new BERepresentanteLegal();
                        oRepresentanteLegal.APODC_TIP_DOC_REP = ConfigurationManager.AppSettings["DcCorpTipoDocDNI"].ToString();
                        oRepresentanteLegal.APODV_NUM_DOC_REP = nroDocumentoDNI;
                        oRepresentanteLegal.APODV_APA_REP_LEG = objDCOUT.ws12_In_ApellidoPaterno;
                        oRepresentanteLegal.APODV_AMA_REP_LEG = objDCOUT.ws12_In_ApellidoMaterno;
                        oRepresentanteLegal.APODV_NOM_REP_LEG = objDCOUT.ws12_In_Nombres;
                        oRepresentanteLegal.TDOCV_DESCRIPCION_REP = "DNI";//PROY-20054
                        oRepresentanteLegal.APODV_CAR_REP = ConfigurationManager.AppSettings["DcCorpCargoRUC10"].ToString();

                        objListaRRLL.Add(oRepresentanteLegal);

                        BERepresentanteLegalDC oRepresentanteLegalDC = new BERepresentanteLegalDC();
                        oRepresentanteLegalDC.ws53_In_NumeroOperacionRepLeg = strNroOperacion;
                        oRepresentanteLegalDC.ws53_Out_RepresentanteLegalTipoPersona = objDataCreditoIN.istrTipoPersona;
                        oRepresentanteLegalDC.ws53_Out_RepresentanteLegalTipoDocumento = ConfigurationManager.AppSettings["DcCorpTipoDocDNI"].ToString();
                        oRepresentanteLegalDC.ws53_Out_RepresentanteLegalNumeroDocumento = nroDocumentoDNI;
                        oRepresentanteLegalDC.ws53_Out_RepresentanteLegalNombre = objDCOUT.ws12_In_ApellidoPaterno + " " + objDCOUT.ws12_In_ApellidoMaterno + " " + objDCOUT.ws12_In_Nombres;
                        oRepresentanteLegalDC.ws53_Out_RepresentanteLegalCargo = ConfigurationManager.AppSettings["DcCorpCargoRUC10"].ToString();

                        objListaRepresentanteLegal.Add(oRepresentanteLegalDC);
                    }

                    oHeader = null;
                    oDataSet = null;
                    // FIN SERVICIO 53

                    // Llenando los datos del cliente
                    rptaEmpresaDC.strFlagInterno = "0";
                    rptaEmpresaDC.strCodRetorno = strCodigoRetorno;
                    rptaEmpresaDC.strNroOperacion = strNroOperacion;
                    rptaEmpresaDC.strRazonSocial = strRazonSocial;
                    rptaEmpresaDC.strRiesgo = strRiesgo;
                    rptaEmpresaDC.strNombres = objDataCreditoIN.istrNombres;
                    rptaEmpresaDC.strApellidoPaterno = objDataCreditoIN.istrApellidoPaterno;
                    rptaEmpresaDC.strApellidoMaterno = objDataCreditoIN.istrApellidoMaterno;
                    rptaEmpresaDC.oRepresentanteLegal = objListaRRLL;
                    rptaEmpresaDC.strMensaje = "";

                    objDCOUT.oRepresentantesLegalDC = objListaRepresentanteLegal;

                    BEDataCreditoINOUT objINOUT = new BEDataCreditoINOUT();
                    objINOUT.IODCV_NUM_OPERACION = objDCOUT.ws12_Out_Header_NumeroOperacion;

                    string paramEntradas = objDCOUT.ws12_In_TipoDocumento + "|" +
                                            objDCOUT.ws12_In_NumeroDocumento + "|" +
                                            objDataCreditoIN.istrApellidoPaterno + "|" +
                                            objDataCreditoIN.istrApellidoMaterno + "|" +
                                            objDataCreditoIN.istrNombres + "|" +
                                            objDCOUT.ws12_In_TipoPersona + "|" +
                                            objDCOUT.ws12_In_TipoServicio;

                    string paramSalidas = objDCOUT.ws12_Out_Header_Transaccion + "|" +
                                           objDCOUT.ws12_Out_Header_TipoServicio + "|" +
                                           objDCOUT.ws12_Out_Header_CodigoRetorno + "|" +
                                           objDCOUT.ws12_Out_Header_NumeroOperacion + "|" +
                                           objDCOUT.ws12_Out_Error_CodigoMensajes + "|" +
                                           objDCOUT.ws12_Out_Error_Mensaje;

                    objINOUT.IODCV_INPUT_VALORES = paramEntradas;
                    objINOUT.IODCV_OUTPUT_VALORES = paramSalidas;
                    objINOUT.IODCV_TIPO_DOCUMENTO = objDCOUT.ws12_In_TipoDocumento;
                    objINOUT.IODCV_NUM_DOCUMENTO = objDCOUT.ws12_In_NumeroDocumento;
                    objINOUT.IODCV_USUARIO_REGISTRO = objUsuario.idCuentaRed;
                    objINOUT.IODCV_COD_PUNTO_VENTA = objUsuario.OficinaVenta; //ADD PROY-20054-IDEA-23849
                    objINOUT.IODCC_RIESGO = objDCOUT.ws50_Out_CampoValor_Accion;
                    objINOUT.IODCC_RESPUESTA_DC = _ServsDc[0].ToString();

                    BERepresentanteLegal oRepresentanteLegalInOut = new BERepresentanteLegal();
                    if (objListaRRLL != null && objListaRRLL.Count != 0)
                        oRepresentanteLegalInOut = (BERepresentanteLegal)objListaRRLL[0];

                    if (objDataCreditoIN.istrTipoPersona.Equals("J"))
                    {
                        objINOUT.IODCV_APE_PATERNO = "";
                        objINOUT.IODCV_APE_MATERNO = "";

                        string strRazonSocialDC = objDCOUT.ws50_Out_GrupoIntegrantes_IntegranteNombres.Trim().ToUpper();
                        if (strRazonSocialDC.Length > 40)
                        {
                            strRazonSocialDC = strRazonSocialDC.Substring(0, 40);
                        }
                        objINOUT.IODCV_NOMBRES = strRazonSocialDC;
                    }
                    else
                    {
                        if (oRepresentanteLegalInOut != null)
                        {
                            objINOUT.IODCV_APE_PATERNO = oRepresentanteLegalInOut.APODV_APA_REP_LEG;
                            objINOUT.IODCV_APE_MATERNO = oRepresentanteLegalInOut.APODV_AMA_REP_LEG;
                            objINOUT.IODCV_NOMBRES = oRepresentanteLegalInOut.APODV_NOM_REP_LEG;
                        }
                        else
                        {
                            objINOUT.IODCV_APE_PATERNO = "";
                            objINOUT.IODCV_APE_MATERNO = "";
                            objINOUT.IODCV_NOMBRES = "";
                        }
                    }

                    objINOUT.IODCV_TIPO_SEC = objDataCreditoIN.istrTipoSEC;

                    objINOUT.IODCC_TIPO_CLIENTE = "";
                    objINOUT.IODCC_FORMA_PAGO = "";
                    objINOUT.IODCC_TIPO_ACTIVACION = "";
                    objINOUT.IODCC_TIPO_CLIENTE = "";
                    objINOUT.IODCC_TIPO_VENTA = "";
                    objINOUT.IODCC_PLAZO_ACUERDO = "";
                    objINOUT.IODCC_PLAN1 = "";
                    objINOUT.IODCC_PLAN2 = "";
                    objINOUT.IODCC_PLAN3 = "";
                    objINOUT.IODCC_CONTROL_CONSUMO = "";
                    objINOUT.IODCC_FLAG_ESSALUD = "";
                    objINOUT.IODCC_FLAG_SUNAT = "";
                    objINOUT.IODCC_LIMITE_CREDITO = "";
                    objINOUT.IODCC_SCORE_TEXTO = "";
                    objINOUT.IODCC_SCORE_NUMERO = "";

                    //INICIO: PROY-20054-IDEA-23849
                    int CODIGOBURO = Funciones.CheckInt(ConfigurationManager.AppSettings["constCodBuroDataCreditoRUC"].ToString());
                    objINOUT.IODCC_CODIGOBURO = CODIGOBURO;
                    objDCOUT.buro_consultado = CODIGOBURO;
                    rptaEmpresaDC.buro_consultado = CODIGOBURO;
                    //FIN

                    // Grabar Datos INPUT/ OUTPUT DC
                    objMensaje = obj.GrabarDCInputOutput(objINOUT);

                    // Grabar Repositorio DC
                    objMensaje = obj.GrabarDatosDCEmpresa(objDCOUT);

                    // Auditoria
                    //string codTransaccion = ConfigurationManager.AppSettings["DcAuditRegistroHistorialDCCorporativo"].ToString();
                    //string detalle = "Registro Tabla Corporativo Exitosa. (" + " " + objDCOUT.ws12_Out_Header_NumeroOperacion + " " + objDCOUT.ws12_In_TipoDocumento + " " + objDCOUT.ws12_In_NumeroDocumento + " " + objDCOUT.ws50_Out_CampoValor_Accion + " " + objDCOUT.ws12_Out_Header_CodigoRetorno + " " + objDCOUT.ws50_Out_GrupoIntegrantes_IntegranteNombres + " " + objDataCreditoIN.istrTipoSEC + ")";
                    //Auditoria(codTransaccion, detalle);
                }
                catch (Exception ex)
                {
                    objMensaje.exito = false;
                    objMensaje.mensajeSistema = ex.Message;
                    objMensaje.mensajeCliente = "Error Consulta DataCredito " + ex.Message;

                    _objLog.CrearArchivologWS("ERROR WS DATACREDITO CORP", null, null, ex);
                }
                finally
                {
                    _objLog.CrearArchivologWS("FIN WS DATACREDITO CORP", null, null, null);
                    _objTransaccion.Dispose();
                }
            }
            else
            {
                // Se evalúan los campos de estado_ruc y deuda_financiera tal y como se realiza cuando se hace la consulta a DC
                rptaEmpresaDC.origen_experto = "BD";
                if (objDCOUT.ws50_Out_CampoValor_Accion.Equals("5"))
                    rptaEmpresaDC.estado_ruc = "B";
                else
                    rptaEmpresaDC.estado_ruc = "A";

                double monto1 = 0;
                double monto2 = 0;
                double monto3 = 0;
                if (Funciones.isNumeric(objDCOUT.ws50_Out_CampoValor_MsgIngTarjeta)) monto1 = Funciones.CheckDbl(objDCOUT.ws50_Out_CampoValor_MsgIngTarjeta);
                if (Funciones.isNumeric(objDCOUT.ws50_Out_CampoValor_MsgIngDHipotecaria)) monto2 = Funciones.CheckDbl(objDCOUT.ws50_Out_CampoValor_MsgIngDHipotecaria);
                if (Funciones.isNumeric(objDCOUT.ws50_Out_CampoValor_MsgIngDnHipoTarjeta)) monto3 = Funciones.CheckDbl(objDCOUT.ws50_Out_CampoValor_MsgIngDnHipoTarjeta);
                
                // Por regla se toma el Monto Mayor
                double montoMayor = monto1;
                if (monto2 > montoMayor) montoMayor = monto2;
                if (monto3 > montoMayor) montoMayor = monto3;
                rptaEmpresaDC.deuda_financiera = montoMayor;

                // Campos traídos de la BD
                rptaEmpresaDC.strCodRetorno = objDCOUT.ws53_Out_Header_CodigoRetorno;
                rptaEmpresaDC.strNroOperacion = objDCOUT.ws12_Out_Header_NumeroOperacion;
                rptaEmpresaDC.strRazonSocial = objDCOUT.ws50_Out_GrupoIntegrantes_IntegranteNombres;
                rptaEmpresaDC.strRiesgo = objDCOUT.ws50_Out_CampoValor_Accion;
                rptaEmpresaDC.strNombres = objDataCreditoIN.istrNombres;
                rptaEmpresaDC.strApellidoPaterno = objDataCreditoIN.istrApellidoPaterno;
                rptaEmpresaDC.strApellidoMaterno = objDataCreditoIN.istrApellidoMaterno;
                rptaEmpresaDC.oRepresentanteLegal = objDCOUT.oRepresentantesLegal;

                rptaEmpresaDC.buro_consultado = objDCOUT.buro_consultado; //ADD PROY-20054-IDEA-23849

                // Siempre el mismo valor, 0 y vacío cuando es correcto
                rptaEmpresaDC.strFlagInterno = "0";
                rptaEmpresaDC.strMensaje = "";

                // Auditoria
                string codTransaccion = ConfigurationManager.AppSettings["DcAuditConsultaHistorialDCCorporativo"].ToString();
                string detalle = "Consulta Tabla Corporativo Exitosa. (" + objDCOUT.ws12_Out_Header_NumeroOperacion + " " + objDCOUT.ws12_In_TipoDocumento + " " + objDCOUT.ws12_In_NumeroDocumento + " " + objDataCreditoIN.istrTipoSEC + ")";
                Auditoria(codTransaccion, detalle);
            }
            return rptaEmpresaDC;
        }

        //PROY-24740
        private bool Auditoria(String strCodTrx, String strDesTrx)
        {
            bool blnOK = false;
            try
            {
                String nombreHost = System.Net.Dns.GetHostName();
                String nombreServer = System.Net.Dns.GetHostName();
                String ipServer = System.Net.Dns.GetHostAddresses(nombreServer)[0].ToString();
                String usuarioId = objSession.idCuentaRed;
                String ipcliente = objSession.Terminal;
                String strCodServ = ConfigurationManager.AppSettings["CONS_COD_SACT_SERV"];
                String vMonto = "0";

                blnOK = (new WS.BWAuditoria()).registrarAuditoria(strCodTrx, strCodServ, ipcliente, nombreHost, ipServer, nombreServer, usuarioId, "", vMonto, strDesTrx);
            }
            catch
            {
                blnOK = false;
            }
            return blnOK;
        }
    }
}
