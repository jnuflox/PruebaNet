using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSLineaDesactiva;

namespace Claro.SISACT.WS
{
    public class BWLineaDesactiva
    {
        #region [Declaracion de Constantes - Config]

        ebsConsultaLineasDesactivasWS _objTransaccion = new ebsConsultaLineasDesactivasWS();
        GeneradorLog _objLog = null;
        string _input = null;
        string _idAplicacion = null;
        string _usuAplicacion = null;
        string _idTransaccion = null;

        #endregion [Declaracion de Constantes - Config]

        public BWLineaDesactiva()
		{
            _objTransaccion.Url = ConfigurationManager.AppSettings["consRutaWSConsultaLineasDesactivas"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_LineaDesactiva"].ToString());

            _idAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
            _usuAplicacion = ConfigurationManager.AppSettings["constNombreAplicacion"].ToString();
            _idTransaccion = DateTime.Now.ToString("hhmmssfff");
		}

        public BEItemMensaje detalleLineaDesactiva(string strTransaccion, string tipoDocumento, string nroDocumento, 
                                                    out LineasDesactivasBSCSComplexType[] objListaLineasDesactivasBSCS,
                                                    out LineasDesactivasSGAComplexType[] objListaLineasDesactivasSGA,
                                                    out AdicionalResponseComplexType[] objAdicionalResponse)
		{
            BEItemMensaje objMensaje = new BEItemMensaje();
            string mensaje = string.Empty;
            _objLog = new GeneradorLog(null, nroDocumento, _idTransaccion, "WEB");

			try
			{
                _input = string.Format("[{0}][{1}][{2}][{3}][{4}][{5}]", strTransaccion, _idAplicacion, _usuAplicacion, _usuAplicacion, tipoDocumento, nroDocumento);
                _objLog.CrearArchivologWS("INICIO WS LINEA DESACTIVA", _objTransaccion.Url, _input, null);

                objMensaje.codigo = _objTransaccion.consultarLineasDesactivas(ref strTransaccion,
                                                                                _idAplicacion,
                                                                                _usuAplicacion,
                                                                                _usuAplicacion,
                                                                                tipoDocumento,
                                                                                nroDocumento, 
                                                                                null,
                                                                                out mensaje,
                                                                                out objListaLineasDesactivasBSCS, 
                                                                                out objListaLineasDesactivasSGA, 
                                                                                out objAdicionalResponse);
                objMensaje.mensajeSistema = mensaje;
                if (objMensaje.codigo != "0")
                {
                    objMensaje.exito = false;
                }
			}
			catch(Exception ex)
			{
                mensaje = mensaje + ".exception: " + ex.Message;
                objMensaje = new BEItemMensaje("99", mensaje, false);
                objListaLineasDesactivasBSCS = null;
                objListaLineasDesactivasSGA = null;
                objAdicionalResponse = null;

                _objLog.CrearArchivologWS("ERROR WS LINEA DESACTIVA", null, null, ex);
            }
            finally
            {
                _objLog.CrearArchivologWS("FIN WS LINEA DESACTIVA", null, null, null);
                _objTransaccion.Dispose();
            }
            return objMensaje;
		}
    }
}
