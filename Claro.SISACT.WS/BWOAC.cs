using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSOAC;

namespace Claro.SISACT.WS
{
    public class BWOAC
    {
        #region [Declaracion de Constantes - Config]

        ConsultaDeudaCuenta _objTransaccion = new ConsultaDeudaCuenta();
        GeneradorLog _objLog = null;
        string _input = null;
        string _idAplicacion = null;
        string _usuAplicacion = null;
        string _idTransaccion = null;

        #endregion [Declaracion de Constantes - Config]

        public BWOAC()
		{
            _objTransaccion.Url = ConfigurationManager.AppSettings["RutaWS_OAC"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_OAC"].ToString());

            _idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _usuAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");
		}

        public DetalleClienteType[] detalleClienteOAC(string tipoDocumento, string nroDocumento)
		{
            DetalleClienteType[] objDetalle = null;
            _objLog = new GeneradorLog(null, nroDocumento, _idTransaccion, "WEB");

            try
            {
                _input = string.Format("[{0}][{1}][{2}][{3}][{4}]", nroDocumento, _idAplicacion, _usuAplicacion, tipoDocumento, nroDocumento);
                _objLog.CrearArchivologWS("INICIO WS OAC", _objTransaccion.Url, _input, null);

                AuditType objAuditType = _objTransaccion.consultaDeuda(nroDocumento, _idAplicacion, _usuAplicacion, tipoDocumento, nroDocumento, out objDetalle);
			}
			catch(Exception ex)
			{
                _objLog.CrearArchivologWS("ERROR WS OAC", null, null, ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                _objLog.CrearArchivologWS("FIN WS OAC", null, null, null);
                _objTransaccion.Dispose();
            }
            return objDetalle;
		}

    }
}
