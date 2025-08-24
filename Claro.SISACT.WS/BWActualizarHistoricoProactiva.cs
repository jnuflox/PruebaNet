using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using System.Web;
using Claro.SISACT.Entity.DataPowerRest;
using Claro.SISACT.Entity.ActualizarHistoricoProactiva.Request;
using Claro.SISACT.Entity.ActualizarHistoricoProactiva.Response;
using System.Collections;
using System.Configuration;
using Claro.SISACT.WS.RestReferences;

namespace Claro.SISACT.WS
{
	/// <summary>
	/// Summary description for BWActualizarHistoricoProactiva.
	/// </summary>
	public class BWActualizarHistoricoProactiva
	{
		public BWActualizarHistoricoProactiva()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool ActualizarHistoricoProactivaNegocios(string Solin_Codigo)
		{
			bool respuestaMetodo = true;
			string nameArchivo = "ActualizarHistoricoProactivaNegocios";
			HelperLog.EscribirLog("", nameArchivo, "" + "[PROY-140579][ActualizarHistoricoProactivaNegocios][INICIO]", false);

			try
			{
				string strIdsHistoricoProactiva = (String)HttpContext.Current.Session["objIdsHistoricoProactiva"];
				HelperLog.EscribirLog("", nameArchivo, "" + "[PROY-140579][ActualizarHistoricoProactivaNegocios][Ids Actualizar] -> " +Funciones.CheckStr(strIdsHistoricoProactiva), false);
				HelperLog.EscribirLog("", nameArchivo, "" + "[PROY-140579][ActualizarHistoricoProactivaNegocios][Solin_Codigo] -> " +Solin_Codigo, false);

				if (Funciones.CheckStr(strIdsHistoricoProactiva) !=  string.Empty && Funciones.CheckStr(Solin_Codigo) !=  string.Empty)
				{
					ActualizarHistoricoRequest request = new ActualizarHistoricoRequest();
					ActualizarHistoricoResponse response = new ActualizarHistoricoResponse();
					RestActualizarHistoricoProactiva objActualizarHistoricoProa= new RestActualizarHistoricoProactiva();
					
					#region Header Request
					HeaderRequest objHeaderRequest = new HeaderRequest();
                    objHeaderRequest.consumer = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                    objHeaderRequest.country = ConfigurationManager.AppSettings["BRMS_country"].ToString();
                    objHeaderRequest.dispositivo = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                    objHeaderRequest.language = ConfigurationManager.AppSettings["BRMS_language"].ToString();
                    objHeaderRequest.modulo = ConfigurationManager.AppSettings["BRMS_modulo"].ToString();
                    objHeaderRequest.msgType = ConfigurationManager.AppSettings["BRMS_msgType"].ToString();
					objHeaderRequest.operation = "ActualizarHistoricoProactiva";
					objHeaderRequest.pid = Convert.ToString(DateTime.Now.Year + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
                    objHeaderRequest.system = ConfigurationManager.AppSettings["BRMS_codSystem"].ToString();
					objHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    objHeaderRequest.userId = ConfigurationManager.AppSettings["constRBRMS"].ToString();
                    objHeaderRequest.wsIp = ConfigurationManager.AppSettings["WsipActualizarHistoricoProactiva"].ToString();
					request.MessageRequest.header.HeaderRequest = objHeaderRequest;
					#endregion

					#region Datos Body
                    request.MessageRequest.body.actualizarHistoricoProactivaRequest.codigoIOBHN = strIdsHistoricoProactiva;
					request.MessageRequest.body.actualizarHistoricoProactivaRequest.codigoSolin = Solin_Codigo;
					#endregion

					response = objActualizarHistoricoProa.ActualizarHistoricoProactivaRest(request);

					string codigoRespuesta = Funciones.CheckStr(response.MessageResponse.body.actualizarHistoricoProactivaResponse.responseStatus.codigoRespuesta);
					string mensajeRespuesta = Funciones.CheckStr(response.MessageResponse.body.actualizarHistoricoProactivaResponse.responseStatus.mensajeRespuesta);

					HelperLog.EscribirLog("", nameArchivo, "" + "[PROY-140579][ActualizarHistoricoProactivaNegocios][codigoRespuesta] -> " +codigoRespuesta, false);
					HelperLog.EscribirLog("", nameArchivo, "" + "[PROY-140579][ActualizarHistoricoProactivaNegocios][mensajeRespuesta] -> " +mensajeRespuesta, false);

					respuestaMetodo = (codigoRespuesta == "0") ? true : false;
                   
					HelperLog.EscribirLog("", nameArchivo, "" + "[PROY-140579][ActualizarHistoricoProactivaNegocios][bool Respuesta] -> " +respuestaMetodo, false);
				}
				else
				{
					HelperLog.EscribirLog("", nameArchivo, "" + "[PROY-140579][ActualizarHistoricoProactivaNegocios] -- PARAMETROS DE ENTRADA VACIOS", false);
					respuestaMetodo = false;
				}                
			}
			catch (Exception ex)
			{
				respuestaMetodo = false;
				HelperLog.EscribirLog("", nameArchivo, "" + "[PROY-140579][ActualizarHistoricoProactivaNegocios][ERROR] -> " +ex.Message.ToString() , false);
			}

			HelperLog.EscribirLog("", nameArchivo, "" + "[PROY-140579][ActualizarHistoricoProactivaNegocios][FIN]", false);

			return respuestaMetodo;		
		}
	}
}
