//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Claro.SISACT.Entity;
using Claro.SISACT.Entity.DataPowerRest;
using Claro.SISACT.Data;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistroPA;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.RegistraHistorial;
using Claro.SISACT.Business.RestReferences;
using Claro.SISACT.Common;
using System.Configuration;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest;
using Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower;

namespace Claro.SISACT.Business
{
    public class BLPagoAnticipado
    {
        public void RegistroPagoAnticipado(RegistroPARequest pPago)
        {
            RestConsultarPagoAnticipadoFija oService = new RestConsultarPagoAnticipadoFija();
            RegistroPAGenericRequest oGenericRequest = new RegistroPAGenericRequest();
            RegistroPAMessageRequest oMessageRequest = new RegistroPAMessageRequest();
            HeadersRequest oHeader = new HeadersRequest();

            Claro.SISACT.Entity.DataPowerRest.HeaderRequest oHeaderRequest = new Entity.DataPowerRest.HeaderRequest();
            oHeaderRequest.consumer = ReadKeySettings.ConsConsumerConsultaPA;
            oHeaderRequest.country = ReadKeySettings.ConsCountryConsultaPA;
            oHeaderRequest.dispositivo = ReadKeySettings.ConsDispositivoConsultaPA;
            oHeaderRequest.language = ReadKeySettings.ConsLanguageConsultaPA;
            oHeaderRequest.modulo = ReadKeySettings.ConsModuloConsultaPA;
            oHeaderRequest.msgType = ReadKeySettings.ConsMsgTypeConsultaPA;
            oHeaderRequest.operation = "registroPA";
            oHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            oHeaderRequest.system = ReadKeySettings.ConsSystemConsultaPA;
            oHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            oHeaderRequest.userId = ReadKeySettings.ConsCurrentUser;
            oHeaderRequest.wsIp = ReadKeySettings.ConsCurrentIP;
            oMessageRequest.Header = new HeadersRequest();
            oMessageRequest.Header.HeaderRequest = oHeaderRequest;

            RegistroPARequest oRequest = new RegistroPARequest();
            oRequest = pPago;

            oMessageRequest.Body = new RegistroPABody();
            oMessageRequest.Body.registroPARequest = oRequest;
            oGenericRequest.MessageRequest = oMessageRequest;

            bool bResultado = oService.RegistrarPagoAnticipado(oGenericRequest);
        }

        public bool ActualizaPagoAnticipado(ActualizaPARequestType objRequest)
        {
            RestConsultarPagoAnticipadoFija oService = new RestConsultarPagoAnticipadoFija();
            ActualizaPAGenericRequest oGenericRequest = new ActualizaPAGenericRequest();
            ActualizaPAMessageRequest oMessageRequest = new ActualizaPAMessageRequest();

            HeaderRequest oHeaderRequest = new HeaderRequest();
            oHeaderRequest.consumer = ReadKeySettings.ConsConsumerConsultaPA;
            oHeaderRequest.country = ReadKeySettings.ConsCountryConsultaPA;
            oHeaderRequest.dispositivo = ReadKeySettings.ConsDispositivoConsultaPA;
            oHeaderRequest.language = ReadKeySettings.ConsLanguageConsultaPA;
            oHeaderRequest.modulo = ReadKeySettings.ConsModuloConsultaPA;
            oHeaderRequest.msgType = ReadKeySettings.ConsMsgTypeConsultaPA;
            oHeaderRequest.operation = "actualizaPA";
            oHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            oHeaderRequest.system = ReadKeySettings.ConsSystemConsultaPA;
            oHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            oHeaderRequest.userId = ReadKeySettings.ConsCurrentUser;
            oHeaderRequest.wsIp = ReadKeySettings.ConsCurrentIP;

            oMessageRequest.Header = new HeadersRequest();
            oMessageRequest.Header.HeaderRequest = oHeaderRequest;

            List<ActualizaPARequestType> oListaRequest = new List<ActualizaPARequestType>();
            ActualizaPARequestType oBeanRequest = new ActualizaPARequestType();
            oBeanRequest = objRequest;

            oListaRequest.Add(oBeanRequest);

            oMessageRequest.Body = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPABody();
            oMessageRequest.Body.actualizaPARequest = new Entity.ClientePagoAnticipadoFijaRest.DataPower.ActualizaPA.ActualizaPARequest();
            oMessageRequest.Body.actualizaPARequest.actualizaPARequestType = oListaRequest;

            oGenericRequest.MessageRequest = oMessageRequest;

            bool bResultado = oService.ActualizarPagoAnticipado(oGenericRequest);

            return bResultado;
        }

        public List<PagoAnticipado> ConsultarPagosAnticipados(ConsultaPARequest pRequest)
        {
            RestConsultarPagoAnticipadoFija oServiceRequest = new RestConsultarPagoAnticipadoFija();
            ConsultaPAGenericRequest oGlobalRequest = new ConsultaPAGenericRequest();
            oGlobalRequest.MessageRequest = new ConsultaPAMessageRequest();
            ConsultaPAHeaderRequest oHeaderRequest = new ConsultaPAHeaderRequest();

            oHeaderRequest.consumer = ReadKeySettings.ConsConsumerConsultaPA;
            oHeaderRequest.country = ReadKeySettings.ConsCountryConsultaPA;
            oHeaderRequest.dispositivo = ReadKeySettings.ConsDispositivoConsultaPA;
            oHeaderRequest.language = ReadKeySettings.ConsLanguageConsultaPA;
            oHeaderRequest.modulo = ReadKeySettings.ConsModuloConsultaPA;
            oHeaderRequest.msgType = ReadKeySettings.ConsMsgTypeConsultaPA;
            oHeaderRequest.operation = ReadKeySettings.ConsOperationConsultaPA;
            oHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            oHeaderRequest.system = ReadKeySettings.ConsSystemConsultaPA;
            oHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            oHeaderRequest.userId = ReadKeySettings.ConsCurrentUser;
            oHeaderRequest.wsIp = ReadKeySettings.ConsCurrentIP;
            ConsultaPAHeader oHeader = new ConsultaPAHeader();
            oHeader.HeaderRequest = oHeaderRequest;

            ConsultaPAMessageRequest oMessageRequest = new ConsultaPAMessageRequest();
            ConsultaPABody oBodyRequest = new ConsultaPABody();

            oBodyRequest.consultaPARequest = pRequest;
            oMessageRequest.Header = oHeader;
            oMessageRequest.Body = oBodyRequest;
            oGlobalRequest.MessageRequest = oMessageRequest;
            List<PagoAnticipado> oListaRespuesta = oServiceRequest.ConsultarPagosAnticipados(oGlobalRequest);

            if (oListaRespuesta != null)
            {
                return oListaRespuesta;
            }
            else
            {
                return new List<PagoAnticipado>();
            }
        }

        public void RegistraHistorialPagoAnticipado(RegistraHistorialRequest pPago)
        {
            RestConsultarPagoAnticipadoFija oService = new RestConsultarPagoAnticipadoFija();
            RegistraHistorialGenericRequest oGenericRequest = new RegistraHistorialGenericRequest();
            RegistraHistorialMessageRequest oMessageRequest = new RegistraHistorialMessageRequest();
            HeadersRequest oHeader = new HeadersRequest();

            Claro.SISACT.Entity.DataPowerRest.HeaderRequest oHeaderRequest = new Entity.DataPowerRest.HeaderRequest();
            oHeaderRequest.consumer = ReadKeySettings.ConsConsumerConsultaPA;
            oHeaderRequest.country = ReadKeySettings.ConsCountryConsultaPA;
            oHeaderRequest.dispositivo = ReadKeySettings.ConsDispositivoConsultaPA;
            oHeaderRequest.language = ReadKeySettings.ConsLanguageConsultaPA;
            oHeaderRequest.modulo = ReadKeySettings.ConsModuloConsultaPA;
            oHeaderRequest.msgType = ReadKeySettings.ConsMsgTypeConsultaPA;
            oHeaderRequest.operation = "registraHistorial";
            oHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            oHeaderRequest.system = ReadKeySettings.ConsSystemConsultaPA;
            oHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            oHeaderRequest.userId = ReadKeySettings.ConsCurrentUser;
            oHeaderRequest.wsIp = ReadKeySettings.ConsCurrentIP;
            oMessageRequest.Header = new HeadersRequest();
            oMessageRequest.Header.HeaderRequest = oHeaderRequest;

            RegistraHistorialRequest oRequest = new RegistraHistorialRequest();
            oRequest = pPago;
            oMessageRequest.Body = new RegistraHistorialBody();
            oMessageRequest.Body.registraHistorialRequest = oRequest;
            oGenericRequest.MessageRequest = oMessageRequest;

            bool bResultado = oService.RegistrarHistorialPagoAnticipado(oGenericRequest);
        }
    }
}
