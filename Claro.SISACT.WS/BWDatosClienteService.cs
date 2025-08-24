using System;
using System.Collections.Generic;
using Claro.SISACT.Entity;
using System.Configuration;
using Claro.SISACT.Business;

namespace Claro.SISACT.WS
{
    public class BWDatosClienteService
    {

        public BEItemGenerico ConsultaDatosCliente(String nroTelefono, ref String mensajeError)
        {
            BEItemGenerico itemDatos = null;

            try
            {
                WSDatosClienteService.ebsDatosClienteService DatosCliente = new WSDatosClienteService.ebsDatosClienteService
                {
                    Url = ConfigurationManager.AppSettings["consDatosCliente"],
                    Credentials = System.Net.CredentialCache.DefaultCredentials,
                    Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeoutWS"])
                };
                WSDatosClienteService.DatosPostPagoxMSISDNRequest objPostPagoReq = new WSDatosClienteService.DatosPostPagoxMSISDNRequest();
                WSDatosClienteService.autenticacionType objAutenTypeReq = new WSDatosClienteService.autenticacionType();
                WSDatosClienteService.DatosPostPagoResponse objPostPagoResp = new WSDatosClienteService.DatosPostPagoResponse();
                objAutenTypeReq.usuario = ConfigurationManager.AppSettings["consUserMigracion"];
                objAutenTypeReq.clave = ConfigurationManager.AppSettings["consPassMigracion"];
                objPostPagoReq.autenticacion = objAutenTypeReq;
                objPostPagoReq.msisdn = nroTelefono;
                objPostPagoReq.idTransaccionConsumidora = DateTime.Now.ToString("yyyyMMddHHmmss");
                objPostPagoReq.aplicacionConsumidora = ConfigurationManager.AppSettings["ConstSistemaConsumer"];
                objPostPagoResp = DatosCliente.leerDatosPostPagoXMSISDN(objPostPagoReq);

                if (objPostPagoResp.resultado.Trim() == "0")
                {
                    itemDatos = new BEItemGenerico();
                    itemDatos.Descripcion = objPostPagoResp.datosPostpago.des_plan_tarifario;
                    itemDatos.Descripcion2 = objPostPagoResp.datosPostpago.estado;
                    mensajeError = "PostPago";
                }
                else
                {
                    mensajeError = "No PostPago";
                }
            }

            catch (Exception ex)
            {
                mensajeError = ex.Message;
            }

            return itemDatos;
        }
    }
}
