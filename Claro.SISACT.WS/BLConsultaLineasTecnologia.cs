using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSLineasTecnologiaCliente;

namespace Claro.SISACT.WS
{
		
	public class BLConsultaLineasTecnologia
	{
        MED_LineasTecnologiaCliente _servicio = new MED_LineasTecnologiaCliente();
		
        public ListaLineaTypeListaLineas[] consultarLineasPrePost(string K_TIPO_DOC,
			string K_NUM_DOC, out string codResp, out string mensResp)
		{
            GeneradorLog objLog = new GeneradorLog(null, K_TIPO_DOC, null, "BlConsultaTecnologia");
            _servicio.Url = ConfigurationManager.AppSettings["RutaWSConsultaLineas3G"].ToString();
            _servicio.Timeout = obtenerTimeOut();
            objLog.CrearArchivolog("INICIO WS ConsultaLIneas", null, null);
            consultarLineasPrePostRequestType requestWS=new consultarLineasPrePostRequestType();
            requestWS.numeroDocumento=K_NUM_DOC;
            requestWS.tipoDocumento = K_TIPO_DOC;
            ListaLineaTypeListaLineas[] res = null;
            try
            {
                _servicio.headerRequest = new HeaderRequestType();
                _servicio.headerRequest.fechaInicio = DateTime.Now;
                _servicio.headerRequest.nodoAdicional = "";
                consultarLineasPrePostResponseType responseWS = _servicio.consultarLineasPrePost(requestWS);
                objLog.CrearArchivolog("obtenido consulta", null, null);
                ResponseStatus status = responseWS.responseStatus;
                codResp = status.codigoRespuesta;
                mensResp = status.descripcionRespuesta;
                objLog.CrearArchivolog("respuesta" + mensResp, null, null);

                if (codResp == "0")
                { res = responseWS.responseDataConsultarLineasPrePost.listaLineasType; }
                else { res = null; }
            }
            catch (Exception e)
            {
                codResp = "";
                mensResp = e.Message;
                objLog.CrearArchivolog("excepcion en consulta: "+e.Message+" "+e.StackTrace, null, null);
                res = null;
            }
            return res;
		}

         private int obtenerTimeOut()
        {
            long codGrupoLineas3G = Funciones.CheckInt64(ConfigurationSettings.AppSettings["codGrupoLineas3g"]);
            List<BEParametro> Lista = (new BLGeneral()).ListaParametrosGrupo(codGrupoLineas3G);
            //busco de la lista el mensaje a mostrar.
            BEParametro sisactParam = Lista.SingleOrDefault(x => x.Valor1 == "36");
            return Int32.Parse(sisactParam.Valor);
          
        }
	}
}
