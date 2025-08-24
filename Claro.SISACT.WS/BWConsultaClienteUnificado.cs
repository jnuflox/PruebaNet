using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Claro.SISACT.WS.ConsultaClienteUnificadoWS;
using Claro.SISACT.Entity;
using System.Configuration;
using Claro.SISACT.Common;

namespace Claro.SISACT.WS
{
  public class BWConsultaClienteUnificado
  {
    ConsultaClienteUnificadoWSService _objTransaccion = new ConsultaClienteUnificadoWSService();

    public BWConsultaClienteUnificado(int intTimeout)
    {
      _objTransaccion.Url = ConfigurationManager.AppSettings["RutaWS_ConsultaClienteUnificadoWS"].ToString();
      _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
      _objTransaccion.Timeout = intTimeout;
    }

    public BEDatoPersonaClarify ConsultarDatoCliente(string strTipoDoc, string strNroDoc, BEItemGenerico objAudit, ref BEItemMensaje objMensaje)
    {
      BEDatoPersonaClarify entity = new BEDatoPersonaClarify();

      try
      {
        ConsultarClienteUnificadoResponse objResponse = new ConsultarClienteUnificadoResponse();
        ConsultarClienteUnificadoRequest objRequest = new ConsultarClienteUnificadoRequest();
        auditRequestType objRequestAudit = new auditRequestType();

        string strTipoBusqueda = ""; string strValorBusqueda = "";


        objRequestAudit.idTransaccion = objAudit.Codigo;
        objRequestAudit.ipAplicacion = objAudit.Descripcion2;
        objRequestAudit.nombreAplicacion = objAudit.Descripcion;
        objRequestAudit.usuarioAplicacion = objAudit.Codigo2;


        strTipoBusqueda = "";
        strTipoBusqueda = Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoBusqueda"]);


        strValorBusqueda = "";
        strValorBusqueda = strTipoDoc + "|" + strNroDoc;

        objRequest.auditRequest = objRequestAudit;
        objRequest.tipoBusqueda = strTipoBusqueda;
        objRequest.valorBusqueda = strValorBusqueda;

        objResponse = _objTransaccion.consultarClienteUnificado(objRequest);

        objMensaje.codigo = Funciones.CheckStr(objResponse.auditResponse.codigoRespuesta);
        objMensaje.descripcion = Funciones.CheckStr(objResponse.auditResponse.mensajeRespuesta);

        if (objMensaje.codigo == "0")
        {
          objMensaje.exito = true;

          entity.TipoDocumento = Funciones.CheckStr(objResponse.listaResponseOpcional[0].valor);
          entity.NroDocumento = Funciones.CheckStr(objResponse.listaResponseOpcional[1].valor);
          entity.Nombres = Funciones.CheckStr(objResponse.listaResponseOpcional[2].valor);
          entity.ApePaterno = Funciones.CheckStr(objResponse.listaResponseOpcional[3].valor);
          entity.ApeMaterno = Funciones.CheckStr(objResponse.listaResponseOpcional[4].valor);
          entity.RazonSocial = Funciones.CheckStr(objResponse.listaResponseOpcional[5].valor);
          entity.TipoValidacion = Funciones.CheckStr(objResponse.listaResponseOpcional[12].valor);


          //entity.FecNacimiento = objResponse.listaResponseOpcional[6].valor;
          //genero
          //fecha de caducidad
          //entity.Email = Funciones.CheckStr(objResponse.listaResponseOpcional[9].valor);
          //entity.Campo1 = Funciones.CheckStr(objResponse.listaResponseOpcional[10].valor);
          //entity.Campo2 = Funciones.CheckStr(objResponse.listaResponseOpcional[11].valor);
          
        }
        else
        {
          objMensaje.exito = false;
        }
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

      return entity;
    }

    public BEItemMensaje RegistrarDatoCliente(BEDatoPersonaClarify entity,string strModo, BEItemGenerico objAudit)
    {
      BEItemMensaje objMensaje = new BEItemMensaje(false);
      try
      {
        RegistrarPersonaResponse objResponse = new RegistrarPersonaResponse();
        RegistrarPersonaRequest objRequest = new RegistrarPersonaRequest();
        auditRequestType objRequestAudit = new auditRequestType();
        basePersonaType objBase = new basePersonaType();
        


        objRequestAudit.idTransaccion = objAudit.Codigo;
        objRequestAudit.ipAplicacion = objAudit.Descripcion2;
        objRequestAudit.nombreAplicacion = objAudit.Descripcion;
        objRequestAudit.usuarioAplicacion = objAudit.Codigo2;

        objBase.modo = strModo;
        objBase.tipoDocumento = Funciones.CheckStr(entity.TipoDocumento);
        objBase.nroDocumento = Funciones.CheckStr(entity.NroDocumento);
        objBase.nombres = Funciones.CheckStr(entity.Nombres);
        objBase.apellidoPaterno = Funciones.CheckStr(entity.ApePaterno);
        objBase.apellidoMaterno = Funciones.CheckStr(entity.ApeMaterno);
        objBase.razonSocial = Funciones.CheckStr(entity.RazonSocial);
        objBase.fechaNacimiento = "";
        objBase.genero = Funciones.CheckStr(entity.Genero);
        objBase.fechaCaducidad = Funciones.CheckStr(entity.FechaCaducidad);
        objBase.email = Funciones.CheckStr(entity.Email);
        objBase.campo1 = Funciones.CheckStr(entity.Campo1);
        objBase.campo2 = Funciones.CheckStr(entity.Campo2);
        objBase.origen = Funciones.CheckStr(entity.TipoValidacion);
        objBase.sistema = Funciones.CheckStr(objRequestAudit.nombreAplicacion);
        objBase.servicio = Funciones.CheckStr(ConfigurationManager.AppSettings["constNameService"]);

        objRequest.auditRequest = objRequestAudit;
        objRequest.basePersona = objBase;

        

        objResponse = _objTransaccion.registrarPersona(objRequest);
        objMensaje.codigo = Funciones.CheckStr(objResponse.auditResponse.codigoRespuesta);
        objMensaje.descripcion = Funciones.CheckStr(objResponse.auditResponse.mensajeRespuesta);
        if (objMensaje.codigo == "0")
        {
          objMensaje.exito = true;
        }
        else
        {
          objMensaje.exito = false;
        }
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

