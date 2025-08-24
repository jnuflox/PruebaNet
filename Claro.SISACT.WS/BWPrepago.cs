using System;
using System.Collections.Generic;
using Claro.SISACT.Entity;
using System.Configuration;
using Claro.SISACT.Business;

namespace Claro.SISACT.WS
{
    public class BWPrepago
    {

        public bool TieneNroFrecuentes(string nroTelefono, ref String mensajeError)
        {
            bool bReturn = false;
            try
            {
                WSPrepago.EbsDatosPrepagoService oServicio = new WSPrepago.EbsDatosPrepagoService
                {
                    Url = ConfigurationManager.AppSettings["consRutaWSPrepago"],
                    Credentials = System.Net.CredentialCache.DefaultCredentials,
                    Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeoutWS"])
                };

                WSPrepago.INDatosPrepagoRequest oRequest = new WSPrepago.INDatosPrepagoRequest();
                oRequest.telefono = nroTelefono;
                WSPrepago.INDatosPrepagoResponse oResponse = oServicio.leerDatosPrepago(oRequest);
                bReturn = oResponse.datosPrePago.fnFNumber0 != "";
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
            }
            return bReturn;
        }

        public BEItemGenerico DatosPrepagoNegocios(String nroTelefono, ref String mensajeError, ref String mensajeAntiguedad)
        {


            BEItemGenerico itemDatos = null;

            try
            {

                String providerIdPrepago = ConfigurationManager.AppSettings["ProviderIdPrepago"];
                String providerIdControl = ConfigurationManager.AppSettings["ProviderIdControl"];


                String[] listPrepago = providerIdPrepago.Split('|');
                String[] listControl = providerIdControl.Split('|');



                WSPrepago.EbsDatosPrepagoService datosPrepago = new WSPrepago.EbsDatosPrepagoService
                {
                    Url = ConfigurationManager.AppSettings["consRutaWSPrepago"],
                    Credentials = System.Net.CredentialCache.DefaultCredentials,
                    Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeoutWS"])
                };

                WSPrepago.INDatosPrepagoRequest objPrepagoReq = new WSPrepago.INDatosPrepagoRequest();
                WSPrepago.INDatosPrepagoResponse objPrepagoResp = new WSPrepago.INDatosPrepagoResponse();

                objPrepagoReq.telefono = nroTelefono;
                objPrepagoResp = datosPrepago.leerDatosPrepago(objPrepagoReq);

                if (objPrepagoResp.resultado.Trim() == "0")
                {
                    //Validar si es prepago
                    mensajeError = "Provider ID no identificado";

                    for (int idx = 0; idx < listPrepago.Length; idx++)
                    {
                        if (objPrepagoResp.datosPrePago.providerID.Trim() == listPrepago[idx])
                        {
                            mensajeError = "Prepago";
                            itemDatos = new BEItemGenerico();
                            itemDatos.Codigo = nroTelefono;
                            itemDatos.Descripcion = objPrepagoResp.datosPrePago.imsi;
                            itemDatos.Descripcion2 = objPrepagoResp.datosPrePago.isLocked;
                            itemDatos.Codigo2 = objPrepagoResp.datosPrePago.subscriberLifeCycleStatus;
                            itemDatos.Estado = "P";
                            break;
                        }
                    }

                    for (int idy = 0; idy < listControl.Length; idy++)
                    {
                        if (objPrepagoResp.datosPrePago.providerID.Trim() == listControl[idy])
                        {
                            mensajeError = "Control";
                            itemDatos = new BEItemGenerico();
                            itemDatos.Codigo = nroTelefono;
                            itemDatos.Descripcion = objPrepagoResp.datosPrePago.imsi;
                            itemDatos.Descripcion2 = objPrepagoResp.datosPrePago.isLocked;
                            itemDatos.Codigo2 = objPrepagoResp.datosPrePago.subscriberLifeCycleStatus;
                            itemDatos.Estado = "C";
                            break;
                        }
                    }
                }
                else
                {
                    mensajeError = "Error. Número ingresado NO es prepago";
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
