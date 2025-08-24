using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.WS.ConsultaDeudaClienteWS;
using Claro.SISACT.Entity;

namespace Claro.SISACT.WS
{
    public class BWConsultaDeudaCliente
    {

        ConsultaDeudaClienteWSService objTransaction = null;

        public BWConsultaDeudaCliente()
        {
            objTransaction = new ConsultaDeudaClienteWSService();
            objTransaction.Url = ConfigurationManager.AppSettings["RutaWS_ConsultaDeudaClienteWS"];
            objTransaction.Credentials = System.Net.CredentialCache.DefaultCredentials;
            objTransaction.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_ConsultaDeudaClienteWS"]);


        }

        public int consultaDeudaXProducto(String strTdo, String strNdo, BEItemGenerico objAudit, out String strCodigo, out string strDescripcion)
        {

            int intFechaUltimaDeuda = 0;
            List<int> lstFechaUltimaDeuda = new List<int>();

            consultaDeudaResponse objResponse = new consultaDeudaResponse();
            consultaDeudaRequest objRequest = new consultaDeudaRequest();

            strCodigo = "";
            strDescripcion = "";

            try
            {
                
                String nombreServer = System.Net.Dns.GetHostName();
                String strIpAplicacion = System.Net.Dns.GetHostEntry(nombreServer).AddressList[0].ToString();
               
                auditRequestType objRequestAudit = new auditRequestType();
                objRequestAudit.idTransaccion = objAudit.Codigo;
                objRequestAudit.ipAplicacion = strIpAplicacion;
                objRequestAudit.nombreAplicacion = objAudit.Descripcion;
                objRequestAudit.usuarioAplicacion = objAudit.Codigo2;
                objRequest.auditRequest = objRequestAudit;

                objRequest.usuarioAplicacion = objAudit.Codigo2;
                objRequest.codigoAplicacion = ConfigurationManager.AppSettings["constAplicacion"];
                objRequest.numeroDocumentoId = strNdo;
                objRequest.tipoDocumentoId = strTdo;


                objResponse = objTransaction.consultaDeudaXProducto(objRequest);

                if (objResponse != null)
                {
                    
                    strCodigo = objResponse.auditResponse.codigoRespuesta;
                    strDescripcion = objResponse.auditResponse.mensajeRespuesta; 
                    
                    if (objResponse.auditResponse.codigoRespuesta == "0")
                    {

                        if (objResponse.listaDetalle != null)
                        {
                            foreach (DetalleType detalle in objResponse.listaDetalle)
                            {

                                if (detalle.listaRecibos != null && detalle.listaRecibos.Length > 0)
                                {

                                    foreach (ReciboType recibo in detalle.listaRecibos)
                                    {
                                        DateTime fVen = Convert.ToDateTime(recibo.fechaVencimiento);
                                    int intDias = (DateTime.Now - fVen).Days;

                                        if (intDias > 0)
                                        {
                                    lstFechaUltimaDeuda.Add(intDias);
                                            break;
                                        }
                                    }


                                }
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                strCodigo = "1";
                strDescripcion = ex.Message; 
            }
            finally
            {
                objTransaction.Dispose();
            }


            if (lstFechaUltimaDeuda.Count > 0)
            {
                lstFechaUltimaDeuda.Sort();
                intFechaUltimaDeuda = lstFechaUltimaDeuda[0] < 0 ? 0 : lstFechaUltimaDeuda[0]; 
            }

            return intFechaUltimaDeuda;

        }

    }
}
