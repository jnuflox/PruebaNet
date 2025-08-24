using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSDataCredito;
using System.Xml;
using System.Xml.Serialization;
using Claro.SISACT.Configuracion;
using Claro.SISACT.Common;
using System.Data;

namespace Claro.SISACT.WS
{
    public class BWCreditoWS
    {
        #region [Declaracion de Constantes - Config]

        string _Url;
        GeneradorLog _objLog = null;
        string _idAplicacion = null;
        string _usuAplicacion = null;
        string _idTransaccion = null;
        static string nombreServer = System.Net.Dns.GetHostName();
        string ipServer = System.Net.Dns.GetHostEntry(nombreServer).AddressList[0].ToString();

        #endregion [Declaracion de Constantes - Config]

        public BWCreditoWS()
		{
            _Url = ConfigurationManager.AppSettings["RutaWS_Orquestador"].ToString();
            _idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _usuAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");
		}


        public BEDataCreditoOUT EvaluarCredito(BEDataCreditoIN objIN, BEUsuarioSession objUsuario, ref BEItemMensaje objMensaje)
        {
            BEDataCreditoOUT objOUT = null;
            objMensaje = new BEItemMensaje();
            EbsCreditosWS.EbsCreditosWSService _objTransaccion = new EbsCreditosWS.EbsCreditosWSService();
            EbsCreditosWS.ConsultarDatosDCRequest _request = new EbsCreditosWS.ConsultarDatosDCRequest();
            EbsCreditosWS.ConsultarDatosDCResponse _response = new EbsCreditosWS.ConsultarDatosDCResponse();
            EbsCreditosWS.AuditRequestType _auditRequest = new EbsCreditosWS.AuditRequestType();
            EbsCreditosWS.BEUsuario _beUsuario = new EbsCreditosWS.BEUsuario();

            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOutEbsCreditosWS"]);

            _auditRequest.idTransaccion = _idTransaccion;
            _auditRequest.ipAplicacion = ipServer;
            _auditRequest.nombreAplicacion = nombreServer;
            _auditRequest.usuarioAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();

            _request.flagConsulta = ConfigurationManager.AppSettings["flagConsultaRepositorio"].ToString();//Nuevo
            _request.nroDocumento = objIN.istrNumeroDocumento;
            _request.strApePaterno = objIN.istrAPELLIDOPATERNO;
            _request.strApeMaterno = objIN.istrAPELLIDOMATERNO;
            _request.strNombres = objIN.istrNOMBRES;
            _request.strRazonSocial = objIN.istrNOMBRES;
            _request.tipoDocumento = objIN.istrTipoDocumento;
            _request.oficina = objUsuario.OficinaVenta;

            _beUsuario.apellido_Mat = objUsuario.Apellido_Mat;
            _beUsuario.apellido_Pat = objUsuario.Apellido_Pat;
            _beUsuario.area = objUsuario.Area;
            _beUsuario.cadenaOpcionesPagina = objUsuario.CadenaOpcionesPagina;
            _beUsuario.cadenaPerfil = objUsuario.CadenaPerfil;
            _beUsuario.canalVenta = objUsuario.CanalVenta;
            _beUsuario.canalVentaDescripcion = objUsuario.CanalVentaDescripcion;
            _beUsuario.estadoAcceso = objUsuario.EstadoAcceso;
            _beUsuario.host = objUsuario.Host;
            _beUsuario.idArea = objUsuario.idArea;
            _beUsuario.idCuentaRed = objUsuario.idCuentaRed;
            _beUsuario.idUsuario = objUsuario.idUsuario;
            _beUsuario.idUsuarioSisact = objUsuario.idUsuarioSisact;
            _beUsuario.idUsuarioSisactSpecified = (Funciones.CheckStr(objUsuario.idUsuarioSisact).Length > 0) ? true : false;
            _beUsuario.idUsuarioSpecified = (Funciones.CheckStr(objUsuario.idUsuario).Length > 0) ? true : false;
            _beUsuario.idVendedorSap = objUsuario.idVendedorSap;
            _beUsuario.login = objUsuario.Login;
            _beUsuario.nombre = objUsuario.Nombre;
            _beUsuario.nombreCompleto = objUsuario.NombreCompleto;
            _beUsuario.oficinaVenta = objUsuario.OficinaVenta;
            _beUsuario.oficinaVentaDescripcion = objUsuario.OficinaVentaDescripcion;
            _beUsuario.perfil149 = objUsuario.Perfil149;
            _beUsuario.perfil149Specified = (Funciones.CheckStr(objUsuario.Perfil149).Length > 0) ? true : false;
            _beUsuario.terminal = objUsuario.Terminal;
            _beUsuario.tipoOficina = objUsuario.TipoOficina;

            _request.objUsuario = _beUsuario;

            _request.auditRequestType = _auditRequest;

            _objLog = new GeneradorLog(null, objIN.istrNumeroDocumento, _idTransaccion, "EbsCredito");

            try
            {
                _objTransaccion.Url = _Url;
                _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;

                _objLog.CrearArchivologWS("INICIO WS EbsCredito", _objTransaccion.Url, objIN, null);

                _response = _objTransaccion.evaluarCredito(_request);

                string strDataCreditoOUT = _response.cadena;
                XmlNode XmlNodo;
                XmlDocument XmlDoc = new XmlDocument();
                string xmlStringResult = strDataCreditoOUT.Replace("&lt;", "<");
                XmlDoc.LoadXml(xmlStringResult);
                XmlNodo = XmlDoc.DocumentElement;

                objOUT = new BEDataCreditoOUT();

                int intBuroConsultado = Convert.ToInt32(_response.buroConsultado);
                
                foreach (XmlAttribute atributo in XmlNodo.Attributes)
                {
                    if (atributo.Name.Equals("NOMBRES")) { objOUT.NOMBRES = atributo.Value.ToUpper(); }
                    else if (atributo.Name.Equals("APELLIDO_PATERNO")) { objOUT.APELLIDO_PATERNO = atributo.Value.ToUpper(); }
                    else if (atributo.Name.Equals("APELLIDO_MATERNO")) { objOUT.APELLIDO_MATERNO = atributo.Value.ToUpper(); }
                    else if (atributo.Name.Equals("NUMERO_DOCUMENTO")) { objOUT.NUMERO_DOCUMENTO = atributo.Value; }
                    else if (atributo.Name.Equals("ANTIGUEDAD_LABORAL")) { objOUT.ANTIGUEDAD_LABORAL = Funciones.CheckInt(atributo.Value); }
                    else if (atributo.Name.Equals("TOP_10000")) { objOUT.TOP_10000 = atributo.Value; }
                    else if (atributo.Name.Equals("TIPO_DE_TARJETA")) { objOUT.TIPO_DE_TARJETA = atributo.Value; }
                    else if (atributo.Name.Equals("TIPO_DE_CLIENTE")) { objOUT.TIPO_DE_CLIENTE = atributo.Value; }
                    else if (atributo.Name.Equals("INGRESO_O_LC")) { objOUT.INGRESO_O_LC = Funciones.CheckDbl(atributo.Value); }
                    else if (atributo.Name.Equals("EDAD")) { objOUT.EDAD = Funciones.CheckInt(atributo.Value); }
                    else if (atributo.Name.Equals("LINEA_DE_CREDITO_EN_EL_SISTEMA")) { objOUT.LINEA_DE_CREDITO_EN_EL_SISTEMA = Funciones.CheckDbl(atributo.Value); }
                    else if (atributo.Name.Equals("TIPO_DE_PRODUCTO")) { objOUT.TIPO_DE_PRODUCTO = atributo.Value; }
                    else if (atributo.Name.Equals("CREDIT_SCORE")) { objOUT.CREDIT_SCORE = atributo.Value; }
                    //else if (atributo.Name.Equals("SCORE")) { objOUT.SCORE = Funciones.CheckInt(atributo.Value); }
                    else if (atributo.Name.Equals("SCORE")) { objOUT.SCORE = (int)Math.Truncate(decimal.Parse(atributo.Value)); }
                    else if (atributo.Name.Equals("EXPLICACION")) { objOUT.EXPLICACION = atributo.Value; }
                    else if (atributo.Name.Equals("NV_TOTAL_CARGOS_FIJOS")) { objOUT.NV_TOTAL_CARGOS_FIJOS = Funciones.CheckDbl(atributo.Value); }
                    else if (atributo.Name.Equals("NV_LC_MAXIMO")) { objOUT.NV_LC_MAXIMO = Funciones.CheckDbl(atributo.Value); }
                    else if (atributo.Name.Equals("LC_DISPONIBLE")) { objOUT.LC_DISPONIBLE = Funciones.CheckDbl(atributo.Value); }
                    else if (atributo.Name.Equals("CLASE_DE_CLIENTE")) { objOUT.CLASE_DE_CLIENTE = atributo.Value; }
                    else if (atributo.Name.Equals("LIMITE_DE_CREDITO")) { objOUT.LIMITE_DE_CREDITO = Funciones.CheckDbl(atributo.Value); }
                    else if (atributo.Name.Equals("DIRECCIONES")) { objOUT.DIRECCIONES = atributo.Value; }
                    else if (atributo.Name.Equals("ACCION")) { objOUT.ACCION = atributo.Value; }
                    else if (atributo.Name.Equals("RegsCalificacion")) { objOUT.REGSCALIFICACION = atributo.Value; }
                    else if (atributo.Name.Equals("CODIGOMODELO")) { objOUT.CODIGOMODELO = atributo.Value; }
                    else if (atributo.Name.Equals("NUMEROOPERACION")) { objOUT.NUMEROOPERACION = atributo.Value; }
                    else if (atributo.Name.Equals("respuesta")) { objOUT.RESPUESTA = atributo.Value; }
                    else if (atributo.Name.Equals("fechaConsulta")) { objOUT.FECHACONSULTA = atributo.Value; }
                    else if (atributo.Name.Equals("RAZONES")) { objOUT.RAZONES = atributo.Value; }
                    else if (atributo.Name.Equals("ANALISIS")) { objOUT.ANALISIS = atributo.Value; }
                    else if (atributo.Name.Equals("SCORE_RANKIN_OPERATIVO")) { objOUT.SCORE_RANKIN_OPERATIVO = atributo.Value; }
                    else if (atributo.Name.Equals("NUMERO_ENTIDADES_RANKIN_OPERATIVO")) { objOUT.NUMERO_ENTIDADES_RANKIN_OPERATIVO = atributo.Value; }
                    else if (atributo.Name.Equals("PUNTAJE")) { objOUT.PUNTAJE = Funciones.CheckDbl(atributo.Value); }
                    else if (atributo.Name.Equals("limiteCreditoDataCredito")) { objOUT.LIMITECREDITODATACREDITO = Funciones.CheckDbl(atributo.Value); }
                    else if (atributo.Name.Equals("limiteCreditoBaseExterna")) { objOUT.LIMITECREDITOBASEEXTERNA = Funciones.CheckDbl(atributo.Value); }
                    else if (atributo.Name.Equals("limiteCreditoClaro")) { objOUT.LIMITECREDITOCLARO = Funciones.CheckDbl(atributo.Value); }
                    else if (atributo.Name.Equals("fechaNacimiento")) { objOUT.FECHANACIMIENTO = string.IsNullOrEmpty(atributo.Value) ? "01/01/1900" : atributo.Value;}
                }
                objOUT.FUENTECONSULTA = BEDataCreditoOUT.FUENTE_CONSULTA.BURO.ToString();
                objOUT.CODIGOBURO = intBuroConsultado;
                objOUT.BUROCONSULTADO = _response.buroConsultado;
            }
            catch (Exception ex)
            {
                objOUT = null;
                objMensaje = new BEItemMensaje(ex.Source, ex.Message, false);

                _objLog.CrearArchivologWS("ERROR WS EbsCredito", null, null, ex);
            }
            finally
            {
                _objLog.CrearArchivologWS("FIN WS EbsCredito", null, null, null);
                _objTransaccion.Dispose();
            }
            return objOUT;
        }

        //INICIO: PROY-20054-IDEA-23849
        /// <summary>
        /// Evaluación de credito CORPORATIVO : PROY-20054-IDEA-23849
        /// </summary>
        /// <param name="objIN"></param>
        /// <param name="objUsuario"></param>
        /// <param name="objMensaje"></param>
        /// <returns></returns>
        public BEEmpresaExperto EvaluarCreditoCorp(BEDataCreditoCorpIN objDataCreditoIN, BEUsuarioSession objUsuario, ref BEItemMensaje objMensaje)
        {
            objMensaje = new BEItemMensaje();
            EbsCreditosWS.EbsCreditosWSService _objTransaccion = new EbsCreditosWS.EbsCreditosWSService();
            EbsCreditosWS.ConsultarDatosDCRequest _request = new EbsCreditosWS.ConsultarDatosDCRequest();
            EbsCreditosWS.ConsultarDatosDCResponse _response = new EbsCreditosWS.ConsultarDatosDCResponse();
            EbsCreditosWS.AuditRequestType _auditRequest = new EbsCreditosWS.AuditRequestType();
            EbsCreditosWS.BEUsuario _beUsuario = new EbsCreditosWS.BEUsuario();
            GeneradorLog _objLog = null;
            //PROY-140257 INI
            BLEvaluacion objEvaluacion = new BLEvaluacion();
            //PROY-140257 FIN
            BEEmpresaExperto rptaEmpresaDC = new BEEmpresaExperto();
            rptaEmpresaDC.strFlagInterno = string.Empty;
            rptaEmpresaDC.origen_experto = "DC";
            string[] _ServsDc = new string[4];

            _ServsDc[0] = ConfigurationManager.AppSettings["DcCorpServicio12"].ToString();
            _ServsDc[1] = ConfigurationManager.AppSettings["DcCorpServicio50"].ToString();
            _ServsDc[2] = ConfigurationManager.AppSettings["DcCorpServicio53"].ToString();

            BEDataCreditoCorpOUT objDCOUT = null;
            List<BERepresentanteLegal> objListaRRLL = new List<BERepresentanteLegal>();

            _objLog = new GeneradorLog(null, objDataCreditoIN.istrNumeroDocumento, _idTransaccion, "WEB");
            _objLog.CrearArchivologWS("INICIO WS DATACREDITO CORP", _objTransaccion.Url, objDataCreditoIN, null);
            _objLog.CrearArchivolog("- INICIO: PARAMETROS DE ENTRADA" + null, null, null);

            _objTransaccion.Url = _Url;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOutEbsCreditosWS"]);
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;

            _auditRequest.idTransaccion = _idTransaccion;
            _auditRequest.ipAplicacion = ipServer;
            _auditRequest.nombreAplicacion = nombreServer;
            _auditRequest.usuarioAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();

            _request.auditRequestType = _auditRequest;

            _objLog.CrearArchivolog("- _objTransaccion.Url -> " + _objTransaccion.Url, null, null);
            _objLog.CrearArchivolog("- _auditRequest.idTransaccion -> " + _auditRequest.idTransaccion, null, null);
            _objLog.CrearArchivolog("- _auditRequest.tipoDocumento -> " + _auditRequest.ipAplicacion, null, null);
            _objLog.CrearArchivolog("- _auditRequest.nombreAplicacion -> " + _auditRequest.nombreAplicacion, null, null);
            _objLog.CrearArchivolog("- _auditRequest.usuarioAplicacion -> " + _auditRequest.usuarioAplicacion, null, null);

            _request.tipoDocumento = objDataCreditoIN.istrTipoDocumento;
            _request.nroDocumento = objDataCreditoIN.istrNumeroDocumento;
            _request.strRazonSocial = objDataCreditoIN.istrNombres;
            _request.strNombres = objDataCreditoIN.istrNombres;
            _request.strApePaterno = objDataCreditoIN.istrApellidoPaterno;
            _request.strApeMaterno = objDataCreditoIN.istrApellidoMaterno;
            _request.oficina = objUsuario.OficinaVenta;

            _objLog.CrearArchivolog("- _request.tipoDocumento -> " + _request.tipoDocumento, null, null);
            _objLog.CrearArchivolog("- _request.nroDocumento -> " + _request.nroDocumento, null, null);
            _objLog.CrearArchivolog("- _request.strRazonSocial -> " + objDataCreditoIN.istrNombres, null, null);
            _objLog.CrearArchivolog("- _request.strNombres -> " + objDataCreditoIN.istrNombres, null, null);
            _objLog.CrearArchivolog("- _request.strApePaterno -> " + _request.strApePaterno, null, null);
            _objLog.CrearArchivolog("- _request.strApeMaterno -> " + _request.strApeMaterno, null, null);
            _objLog.CrearArchivolog("- _request.oficina -> " + _request.oficina, null, null);

            _beUsuario.apellido_Mat = objUsuario.Apellido_Mat;
            _beUsuario.apellido_Pat = objUsuario.Apellido_Pat;
            _beUsuario.area = objUsuario.Area;
            _beUsuario.cadenaOpcionesPagina = objUsuario.CadenaOpcionesPagina;
            _beUsuario.cadenaPerfil = objUsuario.CadenaPerfil;
            _beUsuario.canalVenta = objUsuario.CanalVenta;
            _beUsuario.canalVentaDescripcion = objUsuario.CanalVentaDescripcion;
            _beUsuario.estadoAcceso = objUsuario.EstadoAcceso;
            _beUsuario.host = objUsuario.Host;
            _beUsuario.idArea = objUsuario.idArea;
            _beUsuario.idCuentaRed = objUsuario.idCuentaRed;
            _beUsuario.idUsuario = objUsuario.idUsuario;
            _beUsuario.idUsuarioSisact = objUsuario.idUsuarioSisact;
            _beUsuario.idUsuarioSisactSpecified = (Funciones.CheckStr(objUsuario.idUsuarioSisact).Length > 0) ? true : false;
            _beUsuario.idUsuarioSpecified = (Funciones.CheckStr(objUsuario.idUsuario).Length > 0) ? true : false;
            _beUsuario.idVendedorSap = objUsuario.idVendedorSap;
            _beUsuario.login = objUsuario.Login;
            _beUsuario.nombre = objUsuario.Nombre;
            _beUsuario.nombreCompleto = objUsuario.NombreCompleto;
            _beUsuario.oficinaVenta = objUsuario.OficinaVenta;
            _beUsuario.oficinaVentaDescripcion = objUsuario.OficinaVentaDescripcion;
            _beUsuario.perfil149 = objUsuario.Perfil149;
            _beUsuario.perfil149Specified = (Funciones.CheckStr(objUsuario.Perfil149).Length > 0) ? true : false;
            _beUsuario.terminal = objUsuario.Terminal;
            _beUsuario.tipoOficina = objUsuario.TipoOficina;

            _request.objUsuario = _beUsuario;
            _request.flagConsulta = ConfigurationManager.AppSettings["flagConsultaRepositorio"].ToString();

            _objLog.CrearArchivolog("- _beUsuario.apellido_Mat -> " + _beUsuario.apellido_Mat, null, null);
            _objLog.CrearArchivolog("- _beUsuario.apellido_Pat -> " + _beUsuario.apellido_Pat, null, null);
            _objLog.CrearArchivolog("- _beUsuario.area -> " + _beUsuario.area, null, null);
            _objLog.CrearArchivolog("- _beUsuario.cadenaOpcionesPagina -> " + _beUsuario.cadenaOpcionesPagina, null, null);
            _objLog.CrearArchivolog("- _beUsuario.cadenaPerfil -> " + _beUsuario.cadenaPerfil, null, null);
            _objLog.CrearArchivolog("- _beUsuario.canalVenta -> " + _beUsuario.canalVenta, null, null);
            _objLog.CrearArchivolog("- _beUsuario.canalVentaDescripcion -> " + _beUsuario.canalVentaDescripcion, null, null);
            _objLog.CrearArchivolog("- _beUsuario.estadoAcceso -> " + _beUsuario.estadoAcceso, null, null);
            _objLog.CrearArchivolog("- _beUsuario.host -> " + _beUsuario.host, null, null);
            _objLog.CrearArchivolog("- _beUsuario.idArea -> " + _beUsuario.idArea, null, null);
            _objLog.CrearArchivolog("- _beUsuario.idCuentaRed -> " + _beUsuario.idCuentaRed, null, null);
            _objLog.CrearArchivolog("- _beUsuario.idUsuario -> " + _beUsuario.idUsuario, null, null);
            _objLog.CrearArchivolog("- _beUsuario.idUsuarioSisact -> " + _beUsuario.idUsuarioSisact, null, null);
            _objLog.CrearArchivolog("- _beUsuario.idUsuarioSisactSpecified -> " + _beUsuario.idUsuarioSisactSpecified, null, null);
            _objLog.CrearArchivolog("- _beUsuario.idUsuarioSpecified -> " + _beUsuario.idUsuarioSpecified, null, null);
            _objLog.CrearArchivolog("- _beUsuario.idVendedorSap -> " + _beUsuario.idVendedorSap, null, null);
            _objLog.CrearArchivolog("- _beUsuario.login -> " + _beUsuario.login, null, null);
            _objLog.CrearArchivolog("- _beUsuario.nombre -> " + _beUsuario.nombre, null, null);
            _objLog.CrearArchivolog("- _beUsuario.nombreCompleto -> " + _beUsuario.nombreCompleto, null, null);
            _objLog.CrearArchivolog("- _beUsuario.oficinaVenta -> " + _beUsuario.oficinaVenta, null, null);
            _objLog.CrearArchivolog("- _beUsuario.oficinaVentaDescripcion -> " + _beUsuario.oficinaVentaDescripcion, null, null);
            _objLog.CrearArchivolog("- _beUsuario.perfil149 -> " + _beUsuario.perfil149, null, null);
            _objLog.CrearArchivolog("- _beUsuario.perfil149Specified -> " + _beUsuario.perfil149Specified, null, null);
            _objLog.CrearArchivolog("- _beUsuario.terminal -> " + _beUsuario.terminal, null, null);
            _objLog.CrearArchivolog("- _beUsuario.tipoOficina -> " + _beUsuario.tipoOficina, null, null);
            _objLog.CrearArchivolog("- _request.flagConsulta -> " + _request.flagConsulta, null, null);
            _objLog.CrearArchivolog("- FIN: PARAMETROS DE ENTRADA" + null, null, null);

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

                _response = _objTransaccion.evaluarCredito(_request);

                _objLog.CrearArchivolog("- _response.codigoError -> " + _response.codigoError, null, null);
                _objLog.CrearArchivolog("- _response.descripcionError -> " + _response.descripcionError, null, null);
                _objLog.CrearArchivolog("- _response.tipo -> " + _response.tipo, null, null);
                _objLog.CrearArchivolog("- _response.auditResponseType.codigoRespuesta -> " + _response.auditResponseType.codigoRespuesta, null, null);
                _objLog.CrearArchivolog("- _response.auditResponseType.mensajeRespuesta -> " + _response.auditResponseType.mensajeRespuesta, null, null);

                string rptaReturn = _response.cadena;
                _objLog.CrearArchivolog("- rptaReturn -> " + rptaReturn, null, null);

                string strRpta12 = System.Text.RegularExpressions.Regex.Split(rptaReturn, "RespuestaSVC12>")[1];
                strRpta12 = strRpta12.Substring(0, strRpta12.Length - 2);
                strRpta12 = "<RespuestaSVC12>" + strRpta12 + "</RespuestaSVC12>";
                string strRpta50 = System.Text.RegularExpressions.Regex.Split(rptaReturn, "RespuestaSVC50>")[1];
                strRpta50 = strRpta50.Substring(0, strRpta50.Length - 2);
                strRpta50 = "<RespuestaSVC50>" + strRpta50 + "</RespuestaSVC50>";
                string strRpta53 = System.Text.RegularExpressions.Regex.Split(rptaReturn, "RespuestaSVC53>")[1];
                strRpta53 = strRpta53.Substring(0, strRpta53.Length - 2);
                strRpta53 = "<RespuestaSVC53>" + strRpta53 + "</RespuestaSVC53>";

                #region INICIO SERVICIO 12
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
                #endregion

                #region INICIO SERVICIO 50
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
                        //string valor = Funciones.CheckStr(oEvaluacionCliente.Rows[i]["Valor"]).ToUpper(); 
                        string valor = Funciones.CheckStr(oEvaluacionCliente.Rows[i]["Valor"]); //PROY-140257

                        if (nombreCampo.Equals(strConstAccion))
                        {
                            //INICIO PROY-140257
                            List<BEParametro> lstRiesgo = new List<BEParametro>();
                            _objLog.CrearArchivolog(string.Format("{0}", "-----------------------------------------------------------------------------------"), null, null);
                            _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWCreditoWS][EvaluarCredito] [INICIO]--------------------------------"), null, null);
                            strCodGrupoParamRiesgo = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamRiesgo"]);
                            _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257][BWCreditoWS][EvaluarCredito] strCodGrupoParamRiesgo", strCodGrupoParamRiesgo), null, null);
                            if (!string.IsNullOrEmpty(strCodGrupoParamRiesgo))
                            {
                                lstRiesgo = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamRiesgo));
                                _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257][BWCreditoWS][EvaluarCredito] lstRiesgo.Count", lstRiesgo.Count), null, null);

                                if (lstRiesgo.Count > 0)
                                {
                                    _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWCreditoWS][EvaluarCredito] ListaParametrosGrupo INI----------------"), null, null);
                                    //se recupera el Tipo de Documento de la Key AppSettings["ConsCorporativo"] por defecto valor "C".
                                    tipoDoc = lstRiesgo.Where(p => p.Valor1 == "Key_ConsCorporativo").SingleOrDefault().Valor;
                                    _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | tipoDoc", tipoDoc), null, null);
                                    msjErrorCodRiesgo = lstRiesgo.Where(p => p.Valor1 == "Key_MsjErrorCodRiesgo").SingleOrDefault().Valor;
                                    _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | msjErrorCodRiesgo]", msjErrorCodRiesgo), null, null);
                                    msjRiesgoBaja = lstRiesgo.Where(p => p.Valor1 == "key_MsjRiesgoBaja").SingleOrDefault().Valor;
                                    _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | msjRiesgoBaja]", msjRiesgoBaja), null, null);
                                    codRiesgoBaja = lstRiesgo.Where(p => p.Valor1 == "key_CodRiesgoBaja").SingleOrDefault().Valor;
                                    _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | codRiesgoBaja]", codRiesgoBaja), null, null);
                                    _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWCreditoWS][EvaluarCredito] ListaParametrosGrupo FIN----------------"), null, null);
                                }
                            }

                            //strRiesgo guarda el riesgo que te retorna el Buro.
                            strRiesgo = valor;
                            _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] strRiesgo", strRiesgo), null, null);

                            //Consultamos al Sp SISACT_PKG_CONSULTA_BRMS.SISACTSS_RIESGO enviando el
                            //tipo de documento y la descripcion de riesgo el cual retorna el codigo de Riesgo
                            objEvaluacion.Consultar_Riesgo_Corp(tipoDoc, strRiesgo, ref strCodRiesgo, ref strCodRes, ref strMsjRespuesta);

                            _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWCreditoWS][EvaluarCredito] Consultar_Riesgo_Corp INI---------------"), null, null);
                            _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] IN | tipoDoc", tipoDoc), null, null);
                            _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] IN | strCodRiesgo", strCodRiesgo), null, null);
                            _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | strCodRes", strCodRes), null, null);
                            _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] OUT | strMsjRespuesta", strMsjRespuesta), null, null);
                            _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWCreditoWS][EvaluarCredito] Consultar_Riesgo_Corp FIN---------------"), null, null);
                            //asignamos el codigo de riesgo que nos retorna el sp "strCodRiesgo"

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
                                        _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] objMensaje.mensajeCliente", objMensaje.mensajeCliente), null, null);
                                return rptaEmpresaDC;
                            }
                                }
                            }
                            else
                            {
                                objMensaje.exito = false;
                                objMensaje.mensajeCliente = msjErrorCodRiesgo;
                                _objLog.CrearArchivolog(string.Format("{0} -> {1}", "[PROY-140257] objMensaje.mensajeCliente", objMensaje.mensajeCliente), null, null);
                                rptaEmpresaDC.strCodError = "RIESGO";
                                return rptaEmpresaDC;
                            }
                            _objLog.CrearArchivolog(string.Format("{0}", "[PROY-140257][BWCreditoWS][EvaluarCredito] [FIN]-----------------------------------"), null, null);
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
                #endregion

                #region INICIO SERVICIO 53
                // INICIO SERVICIO 53
                 //INI-32438
                List<BERepresentanteLegalDC> objListaRepresentanteLegal = new List<BERepresentanteLegalDC>();
                Int16 numValue=0;   //32438
                string fechaIniContri = string.Empty ; //32438
              //FIN-32438
                if (strRpta53!=null)
                {
                    objDCOUT.ws53_In_TipoServicio = _ServsDc[2].ToString();
                    objDCOUT.ws53_In_NumeroOperacion = strNroOperacion;

                    System.Xml.XmlDocument oXMLDocument53 = new System.Xml.XmlDocument();
                    oXMLDocument53.LoadXml(strRpta53);

                    oDataSet = new DataSet();
                    oDataSet.ReadXml(new System.Xml.XmlNodeReader(oXMLDocument53));

                    DataTable oDatosRRLL = new DataTable();
                            DataTable oDatosContribuyente = new DataTable();//PROY-32438
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
                                        // INI - PROY-32438
                                        case "CONTRIBUYENTE":
                                            oDatosContribuyente = oDataSet.Tables[i];
                                            break;
                                        //INI-PROY-32438
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

                    if (oDatosRRLL.Rows.Count > 0)
                    {
                        for (int i = 0; i < oDatosRRLL.Rows.Count; i++)
                        {

                                    BERepresentanteLegal oRepresentanteLegal = getDatosRRLL(oDatosRRLL.Rows[i]); //PROY-32438

                            foreach (BETipoDocumento objDoc in objListaDocumento)
                            {
                                if (objDoc.ID_DC == oRepresentanteLegal.APODC_TIP_DOC_REP)
                                {
                                    oRepresentanteLegal.TDOCV_DESCRIPCION_REP = objDoc.DESCRIPCION;
                                    break;
                                }
                            }

                            objListaRRLL.Add(oRepresentanteLegal);

                            BERepresentanteLegalDC oRepresentanteLegalDC = new BERepresentanteLegalDC();
                            oRepresentanteLegalDC.ws53_In_NumeroOperacionRepLeg = strNroOperacion;
                            oRepresentanteLegalDC.ws53_Out_RepresentanteLegalTipoPersona = oDatosRRLL.Rows[i]["TipoPersona"].ToString();
                                    oRepresentanteLegalDC.ws53_Out_RepresentanteLegalTipoDocumento = oRepresentanteLegal.APODC_TIP_DOC_REP; //PROY-32438
                                    oRepresentanteLegalDC.ws53_Out_RepresentanteLegalNumeroDocumento = oRepresentanteLegal.APODV_NUM_DOC_REP; //PROY-32438
                                    oRepresentanteLegalDC.ws53_Out_RepresentanteLegalNombre = oRepresentanteLegal.APODV_NOM_REP_LEG; //PROY-32438
                                    oRepresentanteLegalDC.ws53_Out_RepresentanteLegalCargo = oRepresentanteLegal.APODV_CAR_REP; //PROY-32438
                                    oRepresentanteLegalDC.ws53_Out_RepresentanteLegalFechNomb =oRepresentanteLegal.APODD_FECHA_NOMBRAMIENTO; //PROY-32438
                                    oRepresentanteLegalDC.ws53_Out_RepresentanteLegalCantMesesNomb = Convert.ToInt32(oRepresentanteLegal.APODI_CANT_MESES_NOMBRAMIENTO); //PROY-32438
                            objListaRepresentanteLegal.Add(oRepresentanteLegalDC);
                        }
                    }

                            if (oDatosContribuyente.Rows.Count > 0)
                    {
                                for (int i = 0; i < oDatosContribuyente.Rows.Count; i++)
                        {
                                    objDCOUT.ws53_Out_Header_TipContribuyente = Funciones.CheckStr(oDatosContribuyente.Rows[i]["TipoContribuyente"].ToString());
                                    objDCOUT.ws53_Out_Header_NomComercial = Funciones.CheckStr(oDatosContribuyente.Rows[i]["NombreComercial"].ToString());
                                    objDCOUT.ws53_Out_Header_FecIniActividades = Funciones.ConvertirFecha(oDatosContribuyente.Rows[i]["FechaInicioActividades"].ToString());
                                    objDCOUT.ws53_Out_Header_EstContribuyente = Funciones.CheckStr(oDatosContribuyente.Rows[i]["EstadoContribuyente"].ToString());
                                    objDCOUT.ws53_Out_Header_CondContribuyente = Funciones.CheckStr(oDatosContribuyente.Rows[i]["CondicionContribuyente"].ToString());
                                    objDCOUT.ws53_Out_Header_CiiuContribuyente = Funciones.CheckStr(oDatosContribuyente.Rows[i]["ActividadEconomicaCIIU"].ToString());
                                    objDCOUT.ws53_Out_Header_CantTrabajadores = Funciones.CheckStr( Int16.TryParse(oDatosContribuyente.Rows[i]["CantidadTrabajadores"].ToString(),out numValue)==true?oDatosContribuyente.Rows[i]["CantidadTrabajadores"].ToString():numValue.ToString());
                                    objDCOUT.ws53_Out_Header_EmisionComp = Funciones.CheckStr(oDatosContribuyente.Rows[i]["GrupoEmisionComprobantes"].ToString());
                                    objDCOUT.ws53_Out_Header_SistEmielectronica = Funciones.CheckStr(oDatosContribuyente.Rows[i]["SistemasEmisionElectronica"].ToString());
                        }
                                fechaIniContri = objDCOUT.ws53_Out_Header_FecIniActividades;
                                objDCOUT.ws53_Out_Header_CantMesIniAct = (fechaIniContri == "" || fechaIniContri == null) ? 0 : getCantidadMeses(fechaIniContri); 

                    }
                    /*FIN PROY-32438*/


                }//FIN PROY-32438

                oHeader = null;
                oDataSet = null;
                // FIN SERVICIO 53
                #endregion

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
                rptaEmpresaDC.buro_consultado = Funciones.CheckInt(_response.buroConsultado);
                /*INI PROY-32438*/
                rptaEmpresaDC.TipContribuyente = objDCOUT.ws53_Out_Header_TipContribuyente;
                rptaEmpresaDC.NomComercial = objDCOUT.ws53_Out_Header_NomComercial;
                rptaEmpresaDC.FecIniActividades =Funciones.CheckDate(objDCOUT.ws53_Out_Header_FecIniActividades).ToString();
                rptaEmpresaDC.EstContribuyente  = objDCOUT.ws53_Out_Header_EstContribuyente;
                rptaEmpresaDC.CondContribuyente = objDCOUT.ws53_Out_Header_CondContribuyente;
                rptaEmpresaDC.CiiuContribuyente = objDCOUT.ws53_Out_Header_CiiuContribuyente;
                rptaEmpresaDC.CantTrabajadores =Funciones.CheckInt16(objDCOUT.ws53_Out_Header_CantTrabajadores);
                rptaEmpresaDC.EmisionComp = objDCOUT.ws53_Out_Header_EmisionComp;
                rptaEmpresaDC.SistEmielectronica = objDCOUT.ws53_Out_Header_SistEmielectronica;
                rptaEmpresaDC.CantMesIniActividades = Funciones.CheckInt16(objDCOUT.ws53_Out_Header_CantMesIniAct);
                /*FIN PROY-32438*/
                _objLog.CrearArchivolog("- INICIO: PARAMETROS DE SALIDA" + null, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.strFlagInterno -> " + rptaEmpresaDC.strFlagInterno, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.strCodRetorno -> " + rptaEmpresaDC.strCodRetorno, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.strNroOperacion -> " + rptaEmpresaDC.strNroOperacion, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.strRazonSocial -> " + rptaEmpresaDC.strRazonSocial, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.strRiesgo -> " + rptaEmpresaDC.strRiesgo, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.strNombres -> " + rptaEmpresaDC.strNombres, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.strApellidoPaterno -> " + rptaEmpresaDC.strApellidoPaterno, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.strApellidoMaterno -> " + rptaEmpresaDC.strApellidoMaterno, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.buro_consultado -> " + rptaEmpresaDC.buro_consultado, null, null);
                _objLog.CrearArchivolog("- FIN: PARAMETROS DE SALIDA" + null, null, null);
                _objLog.CrearArchivolog("---------------------------------------------------------------" + null, null, null);
                _objLog.CrearArchivolog("- INI: PARAMETROS DE SALIDA PROY-32438" + null, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.TipContribuyente -> " + rptaEmpresaDC.TipContribuyente, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.NomComercial -> " + rptaEmpresaDC.NomComercial, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.FecIniActividades -> " + rptaEmpresaDC.FecIniActividades, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.EstContribuyente -> " + rptaEmpresaDC.EstContribuyente, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.CondContribuyente -> " + rptaEmpresaDC.CondContribuyente, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.CiiuContribuyente -> " + rptaEmpresaDC.CiiuContribuyente, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.CantMesIniActividades -> " + rptaEmpresaDC.CantMesIniActividades, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.EmisionComp -> " + rptaEmpresaDC.EmisionComp, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.CantMesIniActividades -> " + rptaEmpresaDC.CantMesIniActividades, null, null);
                _objLog.CrearArchivolog("- rptaEmpresaDC.SistEmielectronica -> " + rptaEmpresaDC.SistEmielectronica, null, null);
                _objLog.CrearArchivolog("- FIN: PARAMETROS DE SALIDA PROY-32438" + null, null, null);

            }
            catch (Exception ex)
            {
                objMensaje.exito = false;
                objMensaje.mensajeSistema = ex.Message;
                objMensaje.mensajeCliente = "Error Consulta DataCredito " + ex.Message;
                objMensaje = new BEItemMensaje(ex.Source, ex.Message, false);
                _objLog.CrearArchivologWS("ERROR WS DATACREDITO CORP", null, null, ex);
            }
            finally
            {
                _objLog.CrearArchivologWS("FIN WS DATACREDITO CORP", null, null, null);
                _objTransaccion.Dispose();
            }
            return rptaEmpresaDC;
        }
        //FIN


        private BERepresentanteLegal getDatosRRLL(DataRow oDatosRRLL)
        {


            string strTipoDocumentoRRLL, strNumeroDocumentoRRLL, strCargo, strNombreCompletoRRLL, strNombre, strApePaterno, strApeMaterno,strFechaNombramiento;
            string[] arrNombre;

            
                
                strTipoDocumentoRRLL = oDatosRRLL["TipoDocumento"].ToString();
                strNumeroDocumentoRRLL = oDatosRRLL["NumeroDocumento"].ToString();
                strCargo = oDatosRRLL["Cargo"].ToString();
                /*INI PROY-32438*/
                strFechaNombramiento =Funciones.ConvertirFecha(oDatosRRLL["FechaInicioCargo"].ToString());

                /*FIN PROY-32438*/
                strNombreCompletoRRLL = oDatosRRLL["NombresRazonSocial"].ToString();
                arrNombre = strNombreCompletoRRLL.Split(' ');
                strApePaterno = "";
                strApeMaterno = "";
                strNombre = "";

                /*INI PROY-32438*/
                int totalMonths = (strFechaNombramiento == "" || strFechaNombramiento==null) ? 0 : getCantidadMeses(strFechaNombramiento);
                /*FIN PROY-32438*/
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
                    else {
                        /*INI PROY-32438*/
                           strApePaterno = arrNombre[1];
                         strApeMaterno = "";
                         strNombre = arrNombre[0];
                        /*FIN PROY-32438*/
                        
                    }
                }

                BERepresentanteLegal oRepresentanteLegal = new BERepresentanteLegal();
                oRepresentanteLegal.APODC_TIP_DOC_REP = strTipoDocumentoRRLL.Trim().ToUpper();
                oRepresentanteLegal.APODV_NUM_DOC_REP = strNumeroDocumentoRRLL.Trim().ToUpper();
                oRepresentanteLegal.APODV_APA_REP_LEG = strApePaterno.Trim().ToUpper();
                oRepresentanteLegal.APODV_AMA_REP_LEG = strApeMaterno.Trim().ToUpper();
                oRepresentanteLegal.APODV_NOM_REP_LEG = strNombre.Trim().ToUpper();
                /*INI PROY-32438*/
                oRepresentanteLegal.APODV_CAR_REP = strCargo.Trim().ToUpper();
                oRepresentanteLegal.APODD_FECHA_NOMBRAMIENTO =Funciones.CheckDate(strFechaNombramiento).ToString();
                oRepresentanteLegal.APODI_CANT_MESES_NOMBRAMIENTO = totalMonths.ToString();
                /*FIN PROY-32438*/
                return oRepresentanteLegal;
            
        }

        /*INI PROY-32438*/
        private int getCantidadMeses(string fecha) {
            DateTime fechaCalculo = Convert.ToDateTime(fecha);
            int totalMonths = 0;
            totalMonths = Math.Abs((fechaCalculo.Month - DateTime.Now.Month) + 12 * (fechaCalculo.Year - DateTime.Now.Year));
            return totalMonths;
        }
        /*FIN PROY-32438*/
    }
}
