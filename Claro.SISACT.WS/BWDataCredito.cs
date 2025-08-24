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

namespace Claro.SISACT.WS
{
    public class BWDataCredito
    {
        #region [Declaracion de Constantes - Config]

        string _Key, _User, _Pasw, _Url;
        string strTipoDoc = ConfigurationManager.AppSettings["constCodTipoDocumentoDNICE"].ToString();
        GeneradorLog _objLog = null;
        string _idAplicacion = null;
        string _usuAplicacion = null;
        string _idTransaccion = null;

        #endregion [Declaracion de Constantes - Config]

        public BWDataCredito()
		{
            string idBuro = (new BLDataCredito()).AsignarBuroCrediticio(strTipoDoc, ref _Url, ref _Key);
            ConfigConexionDC(_Key);

            _idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _usuAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");
		}

        public BEDataCreditoOUT ConsultaBuroCrediticio(BEDataCreditoIN objIN, ref BEItemMensaje objMensaje)
        {
            BEDataCreditoOUT objOUT = ConsultaDataCredito(objIN, ref objMensaje);
            return objOUT;
        }

        private void ConfigConexionDC(string _Key)
        {
            //PROY-24740
            Claro.SISACT.Configuracion.ConfigConexionDC oConfigConexionDC = Claro.SISACT.Configuracion.ConfigConexionDC.GetInstance(_Key);
            _User = oConfigConexionDC.Parametros.Usuario;
            _Pasw = oConfigConexionDC.Parametros.Password;
        }

        private BEDataCreditoOUT ConsultaDataCredito(BEDataCreditoIN objIN, ref BEItemMensaje objMensaje)
        {
            BEDataCreditoOUT objOUT = null;
            objMensaje = new BEItemMensaje();
            WSDataCredito.ClaroServiceService _objTransaccion = new WSDataCredito.ClaroServiceService();
            _objLog = new GeneradorLog(null, objIN.istrNumeroDocumento, _idTransaccion, "WEB");

            try
            {
                _objTransaccion.Url = _Url;
                _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;

                _objLog.CrearArchivologWS("INICIO WS DATACREDITO", _objTransaccion.Url, objIN, null);

                string strDataCreditoOUT = _objTransaccion.ejecutarConsultaClaro(objIN.istrSecuencia, 
                                                                                objIN.istrTipoDocumento,
                                                                                objIN.istrNumeroDocumento, 
                                                                                objIN.istrAPELLIDOPATERNO.ToUpper(), 
                                                                                objIN.istrAPELLIDOMATERNO.ToUpper(),
                                                                                objIN.istrNOMBRES.ToUpper(), 
                                                                                objIN.istrDatoEntrada, 
                                                                                objIN.istrDatoComplemento, 
                                                                                objIN.istrTIPOPRODUCTO,
                                                                                objIN.istrTIPOCLIENTE, 
                                                                                objIN.istrEDAD, 
                                                                                objIN.istrIngresoOLineaCredito, 
                                                                                objIN.istrTIPOTARJETA,
                                                                                objIN.istrRUC, 
                                                                                objIN.istrANTIGUEDADLABORAL, 
                                                                                objIN.istrNumOperaPVU, 
                                                                                objIN.istrRegion,
                                                                                objIN.istrArea, 
                                                                                _User, 
                                                                                objIN.istrPuntoVenta, 
                                                                                _Pasw, 
                                                                                objIN.istrIDTerminal, 
                                                                                objIN.istrUsuarioACC,
                                                                                objIN.ostrNumOperaEFT, 
                                                                                objIN.istrNUMCUENTAS, 
                                                                                objIN.istrEstadoCivil);
                XmlNode XmlNodo;
                XmlDocument XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(strDataCreditoOUT);
                XmlNodo = XmlDoc.DocumentElement;

                objOUT = new BEDataCreditoOUT();

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
                    else if (atributo.Name.Equals("SCORE")) { objOUT.SCORE = Funciones.CheckInt(atributo.Value); }
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
                objOUT.CODIGOBURO = Funciones.CheckInt(ConfigurationManager.AppSettings["constCodBuroDataCreditoDNI"].ToString());
                //gaa20170215
                objOUT.BUROCONSULTADO = objOUT.CODIGOBURO.ToString();
                //fin gaa20170215
            }
            catch (Exception ex)
            {
                objOUT = null;
                objMensaje = new BEItemMensaje(ex.Source, ex.Message, false);

                _objLog.CrearArchivologWS("ERROR WS DATACREDITO", null, null, ex);
            }
            finally
            {
                _objLog.CrearArchivologWS("FIN WS DATACREDITO", null, null, null);
                _objTransaccion.Dispose();
            }
            return objOUT;
        }
    }
}
