using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSMetricaEvaluacion;

namespace Claro.SISACT.WS
{
    public class BWMetricaEvaluacion
    {
        #region [Declaracion de Constantes - Config]

        RegistroMetricasVentasWSService _objTransaccion = new RegistroMetricasVentasWSService();
        GeneradorLog _objLog = null;
        string _input = null;
        string _idAplicacion = null;
        string _usuAplicacion = null;
        string _idTransaccion = null;

        #endregion [Declaracion de Constantes - Config]

        public BWMetricaEvaluacion()
		{
            _objTransaccion.Url = ConfigurationManager.AppSettings["WSRegMetricaVenta_url"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["WSRegMetricaVenta_timeout"].ToString());

            _idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _usuAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");
		}

        public void registrarMetrica(string nroDocumento, BEItemGenerico objAudit, List<BEItemGenerico> objCabecera)
        {
            _objLog = new GeneradorLog(null, nroDocumento, _idTransaccion, "WEB");

            try
            {
                registrarMetricaVentasRequest objRequest = new registrarMetricaVentasRequest();

                // Auditoria
                auditRequest objRequestAudit = new auditRequest();
                objRequestAudit.idTransaccion = objAudit.Codigo;
                objRequestAudit.ipAplicacion = objAudit.Descripcion2;
                objRequestAudit.aplicacion = objAudit.Descripcion;
                objRequestAudit.usrAplicacion = objAudit.Codigo2;

                objRequest.audit = objRequestAudit;

                // ID
                objRequest.idMetrica = ConfigurationManager.AppSettings["WSIDRegMetricaVenta"].ToString();

                // Datos
                ventaComplexType objDatos = new ventaComplexType();

                RequestOpcionalCabeceraComplexType[] oHeader = new RequestOpcionalCabeceraComplexType[objCabecera.Count];
                int i = 0;
                foreach (BEItemGenerico item in objCabecera)
                {
                    RequestOpcionalCabeceraComplexType obj = new RequestOpcionalCabeceraComplexType();
                    obj.clave = item.Codigo;
                    obj.valor = item.Descripcion;
                    _input += obj.clave + ": " + obj.valor + "|";
                    oHeader[i] = obj;
                    i++;
                }
                objDatos.cuerpoMetrica = new ListaRequestMetrica();
                objDatos.cuerpoMetrica.RequestOpcinalCabecera = oHeader;
                objRequest.datosMetrica = objDatos;

                _input = string.Format("[{0}][{1}][{2}][{3}]", nroDocumento, _idAplicacion, _usuAplicacion, _input);
                _objLog.CrearArchivologWS("INICIO WS METRICA", _objTransaccion.Url, _input, null);

                _objTransaccion.registrarMetricaVentas(objRequest);
            }
            finally
            {
                _objLog.CrearArchivologWS("FIN WS METRICA", null, null, null);
                _objTransaccion.Dispose();
            }
        }

    }
}
