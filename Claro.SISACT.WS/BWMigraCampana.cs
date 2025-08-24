using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.WS.ConsultaMigraCampanaWS;
using System.Configuration;
using Claro.SISACT.Entity;

namespace Claro.SISACT.WS
{
  public class BWMigraCampana
  {
    ConsultaMigraCampanaWSService _objTransaccion = new ConsultaMigraCampanaWSService();

    public BWMigraCampana()
    {
      _objTransaccion.Url = ConfigurationManager.AppSettings["RutaWS_MigraCampana"].ToString();
      _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
      _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_MigraCampana"].ToString());      
    }

     /// <summary>
    /// ValidarPagoFactura
     /// </summary>
     /// <param name="strTipoDoc"></param>
     /// <param name="strNroDoc"></param>
     /// <param name="objAudit"></param>
     /// <returns></returns>
    public BEItemMensaje ValidarPagoFactura(string strTipoDoc, string strNroDoc, BEItemGenerico objAudit,out string strNroLinea)
    {

      strNroLinea = "";


      BEItemMensaje objMensaje = new BEItemMensaje(false);
      try
      {

        validarPagoFacturaResponse objResponse = new validarPagoFacturaResponse();
        validarPagoFacturaRequest objRequest = new validarPagoFacturaRequest();

        
        

        auditRequestType objRequestAudit = new auditRequestType();
        objRequestAudit.idTransaccion = objAudit.Codigo;
        objRequestAudit.ipAplicacion = objAudit.Descripcion2;
        objRequestAudit.nombreAplicacion = objAudit.Descripcion;
        objRequestAudit.usuarioAplicacion = objAudit.Codigo2;

        objRequest.auditRequest = objRequestAudit;
        objRequest.tipoDocumento = strTipoDoc;
        objRequest.numeroDocumento = strNroDoc;


        objResponse = _objTransaccion.validarPagoFactura(objRequest);


        if (objResponse.linea == null)
          strNroLinea = "";
        else
          strNroLinea = objResponse.linea;

        objMensaje.codigo = objResponse.auditResponse.codigoRespuesta.ToString();
        objMensaje.descripcion = objResponse.auditResponse.mensajeRespuesta.ToString();

        if (objMensaje.codigo == "0") objMensaje.exito = true;


      }
      catch (Exception ex)
      {
        objMensaje.codigo = ex.Source;
        objMensaje.mensajeSistema = ex.Message;
        objMensaje.descripcion = ex.Message;
      }
      finally
      {
        _objTransaccion.Dispose();
      }
      return objMensaje;
    }
  }
}
