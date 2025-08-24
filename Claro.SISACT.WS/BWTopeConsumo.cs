using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.ConfiguracionTopeConsumoWS;

namespace Claro.SISACT.WS
{
    public class BWTopeConsumo
    {
        #region [Declaracion de Constantes - Config]

        ConfiguracionTopeConsumoWSService _objTransaccion = new ConfiguracionTopeConsumoWSService();

        GeneradorLog _objLog = null;
        string _idAplicacion = null;
        string _ipAplicacion = null;
        string _usuAplicacion = null;
        string _idTransaccion = null;

        #endregion [Declaracion de Constantes - Config]

        public BWTopeConsumo()
        {
            _objLog = new GeneradorLog(null, "Constructor BWTopeConsumo()", _idTransaccion, "WEB");

            _objLog.CrearArchivolog("INICIO", null, null);

            _objLog.CrearArchivolog("Url = " + ConfigurationManager.AppSettings["constUrlTopeConsumoWS"].ToString(), null, null);
            _objLog.CrearArchivolog("Credentials = " + System.Net.CredentialCache.DefaultCredentials.ToString(), null, null);
            _objLog.CrearArchivolog("Timeout = " + ConfigurationManager.AppSettings["constTimeOutTopeConsumoWS"].ToString(), null, null);

            _objTransaccion.Url = ConfigurationManager.AppSettings["constUrlTopeConsumoWS"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["constTimeOutTopeConsumoWS"].ToString());

            _objLog.CrearArchivolog("nombreServer = " + System.Net.Dns.GetHostName(), null, null);
            string nombreServer = System.Net.Dns.GetHostName();

            _objLog.CrearArchivolog("_ipAplicacion = " + System.Net.Dns.GetHostEntry(nombreServer).AddressList[0].ToString(), null, null);
            _objLog.CrearArchivolog("_idAplicacion = " + ConfigurationManager.AppSettings["CodigoAplicacion"].ToString(), null, null);
            _objLog.CrearArchivolog("_usuAplicacion = " + ConfigurationManager.AppSettings["constNombreAplicacion"].ToString(), null, null);
            _objLog.CrearArchivolog("_idTransaccion = " + DateTime.Now.ToString("hhmmssfff"), null, null);

            _ipAplicacion = System.Net.Dns.GetHostEntry(nombreServer).AddressList[0].ToString();
            _idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _usuAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");

            _objLog.CrearArchivolog("FIN", null, null);
        }

        public List<BETopeConsumo> LlenarMontosTopeConsumo(BETopeConsumo objTopeConsumoIN, ref BEItemMensaje objMensaje)
        {
            List<BETopeConsumo> listObjTopeConsumoOUT = new List<BETopeConsumo>();
            BETopeConsumo objTopeConsumoOUT = null;
            objMensaje = new BEItemMensaje();
            _objLog = new GeneradorLog(null, "Método LlenarMontosTopeConsumo()", _idTransaccion, "WEB");

            try
            {
                _objLog.CrearArchivolog("INICIO WS ConfiguracionTopeConsumoWS", null, null);

                listarConfiguracionTopeConsumoRequest objRequest = new listarConfiguracionTopeConsumoRequest();
                listarConfiguracionTopeConsumoResponse objResponse = new listarConfiguracionTopeConsumoResponse();
                auditRequestType objAuditRequestType = new auditRequestType();

                objAuditRequestType.idTransaccion = _idAplicacion;
                objAuditRequestType.ipAplicacion = _ipAplicacion;
                objAuditRequestType.nombreAplicacion = _usuAplicacion;
                objAuditRequestType.usuarioAplicacion = _usuAplicacion;

                _objLog.CrearArchivolog("codigoProducto = " + objTopeConsumoIN.CodigoProducto, null, null);
                _objLog.CrearArchivolog("codigoPlan = " + objTopeConsumoIN.CodigoPlan, null, null);
                _objLog.CrearArchivolog("codigoServicio = " + objTopeConsumoIN.CodigoServicio, null, null);

                objRequest.auditRequest = objAuditRequestType;
                objRequest.codigoProducto = objTopeConsumoIN.CodigoProducto;
                objRequest.codigoPlan = objTopeConsumoIN.CodigoPlan;
                objRequest.codigoServicio = objTopeConsumoIN.CodigoServicio;

                objResponse = _objTransaccion.listarConfiguracionTopeConsumo(objRequest);

                if (objResponse != null && objResponse.auditResponse.codigoRespuesta == "0")
                {
                    _objLog.CrearArchivolog("codigoRptaWs = " + objResponse.auditResponse.codigoRespuesta, null, null);
                    _objLog.CrearArchivolog("descripcionRptaWs = " + objResponse.auditResponse.mensajeRespuesta, null, null);

                    objMensaje.codigo = objResponse.auditResponse.codigoRespuesta;
                    objMensaje.descripcion = objResponse.auditResponse.mensajeRespuesta;

                    foreach (listaConfiguracionTopeConsumoResponseType objMontosXTopeConsumo in objResponse.listaConfiguracionTopeConsumo)
                    {
                        objTopeConsumoOUT = new BETopeConsumo();
                        objTopeConsumoOUT.ID = objMontosXTopeConsumo.tpmvCodigo;
                        objTopeConsumoOUT.CodigoServicio = objMontosXTopeConsumo.servvCodigo;
                        objTopeConsumoOUT.DescripcionServicio = objMontosXTopeConsumo.servvDescripcion;
                        objTopeConsumoOUT.Monto = objMontosXTopeConsumo.tpmvMonto;
                        objTopeConsumoOUT.Minuto = objMontosXTopeConsumo.tpmvMinuto;
                        listObjTopeConsumoOUT.Add(objTopeConsumoOUT);
                    }
                }
            }
            catch (Exception ex)
            {
                listObjTopeConsumoOUT = null;
                objMensaje = new BEItemMensaje(ex.Source, ex.Message, false);

                _objLog.CrearArchivolog("ERROR WS ConfiguracionTopeConsumoWS", null, ex);
            }
            finally
            {
                _objLog.CrearArchivolog("FIN WS ConfiguracionTopeConsumoWS", null, null);
                _objTransaccion.Dispose();
            }
            return listObjTopeConsumoOUT;
        }
    }
}
