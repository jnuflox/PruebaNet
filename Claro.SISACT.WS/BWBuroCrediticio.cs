using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSBuroCrediticio;

namespace Claro.SISACT.WS
{
    public class BWBuroCrediticio
    {
        #region [Declaracion de Constantes - Config]

        ebsBuroCrediticioService _objTransaccion = new ebsBuroCrediticioService();
        auditoria _objAuditoria = new auditoria();
        GeneradorLog _objLog = null;
        string _idTransaccion = null;

        #endregion [Declaracion de Constantes - Config]

        public BWBuroCrediticio()
		{
            _objTransaccion.Url = ConfigurationManager.AppSettings["constUrlBuroCrediticio"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["constTimeOutBuroCrediticio"].ToString());

            _objAuditoria.usuarioAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _objAuditoria.nombreAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            
            string nombreServer = System.Net.Dns.GetHostName();
            string ipServer = System.Net.Dns.GetHostEntry(nombreServer).AddressList[0].ToString();

            _objAuditoria.ipAplicacion = ipServer;
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");
		}

        public BEDataCreditoOUT ConsultaBuroCrediticio(BEDataCreditoIN objIN, ref BEItemMensaje objMensaje)
        {
            BEDataCreditoOUT objOUT = null;
            objMensaje = new BEItemMensaje();
            _objLog = new GeneradorLog(null, objIN.istrNumeroDocumento, _idTransaccion, "WEB");

            try
            {
                auditoria objAuditoria = new auditoria();

                objEvaluarCreditoReq objRequest = new objEvaluarCreditoReq();
                objEvaluarCreditoResp objResponse = new objEvaluarCreditoResp();

                RequestOpcionalComplexType[] objRequestOpcional = null;
                ResponseOpcionalComplexType[] objResponseOpcional = null;

                string codRespuesta = string.Empty;
                string msjRespuesta = string.Empty;
                string strRespuesta = string.Empty;

                objAuditoria.idTransaccion = objIN.istrNumeroDocumento;

                objRequest.istrSecuencia = objIN.istrSecuencia;
                objRequest.istrTipoDocumento = objIN.istrTipoDocumento;
                objRequest.istrNumeroDocumento = objIN.istrNumeroDocumento;
                objRequest.istrAPELLIDOPATERNO = objIN.istrAPELLIDOPATERNO.ToUpper();
                objRequest.istrAPELLIDOMATERNO = objIN.istrAPELLIDOMATERNO.ToUpper();
                objRequest.istrNOMBRES = objIN.istrNOMBRES.ToUpper();
                objRequest.istrDatoEntrada = objIN.istrDatoEntrada;
                objRequest.istrDatoComplemento = objIN.istrDatoComplemento;
                objRequest.istrTIPOPRODUCTO = objIN.istrTIPOPRODUCTO;
                objRequest.istrTIPOCLIENTE = objIN.istrTIPOCLIENTE;
                objRequest.istrEDAD = objIN.istrEDAD;
                objRequest.istrIngresoOLineaCredito = objIN.istrIngresoOLineaCredito;
                objRequest.istrTIPOTARJETA = objIN.istrTIPOTARJETA;
                objRequest.istrRUC = objIN.istrRUC;
                objRequest.istrANTIGUEDADLABORAL = objIN.istrANTIGUEDADLABORAL;
                objRequest.istrNumOperaPVU = objIN.istrNumOperaPVU;
                objRequest.istrRegion = objIN.istrRegion;
                objRequest.istrArea = objIN.istrArea;
                objRequest.istrCanal = "9896708";
                objRequest.istrPuntoVenta = objIN.istrPuntoVenta;
                objRequest.istrIDCanal = "08MPD";
                objRequest.istrIDTerminal = objIN.istrIDTerminal;
                objRequest.istrUsuarioACC = objIN.istrUsuarioACC;
                objRequest.istrNumOperaEFT = objIN.ostrNumOperaEFT;
                objRequest.istrNUMCUENTAS = objIN.istrNUMCUENTAS;
                objRequest.istrEstadoCivil = objIN.istrEstadoCivil;

                _objLog.CrearArchivologWS("INICIO WS BURO CREDITICIO", _objTransaccion.Url, objRequest, null);

                strRespuesta = _objTransaccion.evaluarCredito(objAuditoria, objRequest, objRequestOpcional,
                                                            out codRespuesta, out msjRespuesta, out objResponse, out objResponseOpcional);

                if (objResponse != null)
                {
                    objOUT.ACCION = objResponse.accion;
                    objOUT.ANALISIS = objResponse.analisis;
                    objOUT.ANTIGUEDAD_LABORAL = Funciones.CheckInt(objResponse.antiguedadLaboral);
                    objOUT.APELLIDO_MATERNO = objResponse.apellidoMaterno;
                    objOUT.APELLIDO_PATERNO = objResponse.apellidoPaterno;
                    objOUT.CLASE_DE_CLIENTE = objResponse.claseDeCliente;
                    objOUT.CODIGOMODELO = objResponse.codigoModelo;
                    objOUT.CREDIT_SCORE = objResponse.creditScore;
                    objOUT.DIRECCIONES = objResponse.direcciones;
                    objOUT.EDAD = Funciones.CheckInt(objResponse.edad);
                    objOUT.EXPLICACION = objResponse.explicacion;
                    objOUT.FECHACONSULTA = objResponse.fechaConsulta;
                    objOUT.FECHANACIMIENTO = objResponse.fechaNacimiento;
                    objOUT.INGRESO_O_LC = Funciones.CheckDbl(objResponse.ingresoOLC);
                    objOUT.LC_DISPONIBLE = Funciones.CheckDbl(objResponse.lcDisponible);
                    objOUT.LIMITE_DE_CREDITO = objResponse.limiteCreditoBaseExterna;
                    objOUT.LIMITECREDITOBASEEXTERNA = objResponse.limiteCreditoClaro;
                    objOUT.LIMITECREDITOCLARO = objResponse.limiteCreditoDataCredito;
                    objOUT.LIMITECREDITODATACREDITO = Funciones.CheckDbl(objResponse.limiteDeCredito);
                    objOUT.LINEA_DE_CREDITO_EN_EL_SISTEMA = Funciones.CheckDbl(objResponse.lineaDeCreditoEnElSistema);
                    objOUT.NOMBRES = objResponse.nombres;
                    objOUT.NUMERO_DOCUMENTO = objResponse.numeroDocumento;
                    objOUT.NUMERO_ENTIDADES_RANKIN_OPERATIVO = objResponse.numeroEntidadesRankinOperativo;
                    objOUT.NUMEROOPERACION = objResponse.numeroOperacion;
                    objOUT.NV_LC_MAXIMO = Funciones.CheckDbl(objResponse.nvLCMaximo);
                    objOUT.NV_TOTAL_CARGOS_FIJOS = Funciones.CheckDbl(objResponse.nvTotalCargosFijos);
                    objOUT.PUNTAJE = Funciones.CheckDbl(objResponse.puntaje);
                    objOUT.RAZONES = objResponse.razones;
                    objOUT.REGSCALIFICACION = objResponse.regsCalificacion;
                    objOUT.RESPUESTA = objResponse.respuesta;
                    objOUT.SCORE = Funciones.CheckInt(objResponse.score);
                    objOUT.SCORE_RANKIN_OPERATIVO = objResponse.scoreRankinOperativo;
                    objOUT.TIPO_DE_CLIENTE = objResponse.tipoDeCliente;
                    objOUT.TIPO_DE_PRODUCTO = objResponse.tipoDeProducto;
                    objOUT.TIPO_DE_TARJETA = objResponse.tipoDeTarjeta;
                    objOUT.TOP_10000 = objResponse.top10000;
                    objOUT.CODIGOBURO = Funciones.CheckInt(objResponse.urlEjecutado);
                }
            }
            catch (Exception ex)
            {
                objOUT = null;
                objMensaje = new BEItemMensaje(ex.Source, ex.Message, false);

                _objLog.CrearArchivologWS("ERROR WS BURO CREDITICIO", null, null, ex);
            }
            finally
            {
                _objLog.CrearArchivologWS("FIN WS BURO CREDITICIO", null, null, null);
                _objTransaccion.Dispose();
            }
            return objOUT;
        }
    }
}
