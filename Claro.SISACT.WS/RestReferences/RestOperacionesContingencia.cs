using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using System.Collections;
using System.Configuration;
using Claro.SISACT.WS.RestServices;
using Claro.SISACT.Entity.claro_vent_operacionescontingencia.desactivarContingencia;
using Claro.SISACT.Entity.claro_vent_operacionescontingencia.consultarconfiguracion;

namespace Claro.SISACT.WS.RestReferences
{
    public class RestOperacionesContingencia
    {
        static string strArchivo = "RestOperacionesContingencia";

        public MessageResponseDesactivarContingencia DesactivarContingencia(BEAuditoriaRequest objBEAuditoriaRequest)
        {
            string idLogDatos = "DesactivarContingencia";
            GeneradorLog objlog = new GeneradorLog(string.Format("[{0}][{1}][{2}]", strArchivo, "PROY-140715", idLogDatos),null,null,null);
            objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "INICIO MÉTODO DesactivarContingencia"), null, null);

            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaRequest.timestamp);
                paramHeader.Add("userId", objBEAuditoriaRequest.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaRequest.idTransaccion);

                objlog.CrearArchivolog(string.Format("{0}{1}{0}", "--------------------", "Parametros de entrada Inicio"), null, null);
                objlog.CrearArchivolog(string.Format("timestamp => {0}", objBEAuditoriaRequest.timestamp), null, null);
                objlog.CrearArchivolog(string.Format("userId => {0}", objBEAuditoriaRequest.userId), null, null);
                objlog.CrearArchivolog(string.Format("idTransaccion => {0}", objBEAuditoriaRequest.idTransaccion), null, null);
                objlog.CrearArchivolog(string.Format("accept => {0}", objBEAuditoriaRequest.accept), null, null);
                objlog.CrearArchivolog(string.Format("ipApplication => {0}", objBEAuditoriaRequest.ipApplication), null, null);
                objlog.CrearArchivolog(string.Format("{0}{1}{0}", "--------------------", "Parametros de entrada Fin"), null, null);

                string userEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["User_VentasContingencia"]);
                string passEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["Pass_VentasContingencia"]);
                string strNombreTimeOut = Funciones.CheckStr(ConfigurationManager.AppSettings["Time_Out_VentasContingencia"]);

                objlog.CrearArchivolog(string.Format("userEncriptado => {0}", userEncriptado), null, null);
                objlog.CrearArchivolog(string.Format("passEncriptado => {0}", passEncriptado), null, null);
                objlog.CrearArchivolog(string.Format("strNombreTimeOut => {0}", strNombreTimeOut), null, null);

                Dictionary<string, string> parametros = new Dictionary<string, string>();

                objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO DesactivarContingencia"), null, null);

                return RestServiceConsultarVentasContingencia.GetInvoque<MessageResponseDesactivarContingencia>("consUrlDesactivarContingencia", paramHeader, parametros, userEncriptado, passEncriptado, strNombreTimeOut);
            }
            catch (Exception ex)
            {
                objlog.CrearArchivolog(string.Format("Error => {0}", ex.Message), null, null);
                objlog.CrearArchivolog(string.Format("Error => {0}", ex.StackTrace), null, null);
                objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO DesactivarContingencia"), null, null);
                throw;
            }
        }


        public MessageResponseConsultarConfiguracion ConsultarConfiguracion(BEAuditoriaRequest objBEAuditoriaRequest, string sistema, string codCtg, string codCanal, string codOficina, string descOficina)
        {
            string idLogDatos = "ConsultarConfiguracion";
            GeneradorLog objlog = new GeneradorLog(string.Format("[{0}][{1}][{2}]", strArchivo, "PROY-140715", idLogDatos), null, null, null);
            objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "INICIO MÉTODO ConsultarConfiguracion"), null, null);

            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaRequest.timestamp);
                paramHeader.Add("userId", objBEAuditoriaRequest.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaRequest.idTransaccion);

                objlog.CrearArchivolog(string.Format("{0}{1}{0}", "--------------------", "Parametros de entrada Inicio"), null, null);
                objlog.CrearArchivolog(string.Format("timestamp => {0}", objBEAuditoriaRequest.timestamp), null, null);
                objlog.CrearArchivolog(string.Format("userId => {0}", objBEAuditoriaRequest.userId), null, null);
                objlog.CrearArchivolog(string.Format("idTransaccion => {0}", objBEAuditoriaRequest.idTransaccion), null, null);
                objlog.CrearArchivolog(string.Format("accept => {0}", objBEAuditoriaRequest.accept), null, null);
                objlog.CrearArchivolog(string.Format("ipApplication => {0}", objBEAuditoriaRequest.ipApplication), null, null);
                objlog.CrearArchivolog(string.Format("{0}{1}{0}", "--------------------", "Parametros de entrada Fin"), null, null);

                string userEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["User_VentasContingencia"]);
                string passEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["Pass_VentasContingencia"]);
                string strNombreTimeOut = Funciones.CheckStr(ConfigurationManager.AppSettings["Time_Out_VentasContingencia"]);

                objlog.CrearArchivolog(string.Format("userEncriptado => {0}", userEncriptado), null, null);
                objlog.CrearArchivolog(string.Format("passEncriptado => {0}", passEncriptado), null, null);
                objlog.CrearArchivolog(string.Format("strNombreTimeOut => {0}", strNombreTimeOut), null, null);

                Dictionary<string, string> parametros = new Dictionary<string, string>();
                parametros = ObtenerValores(sistema, codCtg, codCanal, codOficina, descOficina);

                objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO ConsultarConfiguracion"), null, null);

                return RestServiceConsultarVentasContingencia.GetInvoque<MessageResponseConsultarConfiguracion>("consUrlConsultarContingencia", paramHeader, parametros, userEncriptado, passEncriptado, strNombreTimeOut);
            }
            catch (Exception ex)
            {
                objlog.CrearArchivolog(string.Format("Error => {0}", ex.Message), null, null);
                objlog.CrearArchivolog(string.Format("Error => {0}", ex.StackTrace), null, null);
                objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO DesactivarContingencia"), null, null);
                throw;
            }
        }

        public Dictionary<string, string> ObtenerValores(string Strsistema, string StrcodCtg, string StrcodCanal, string StrcodOficina, string StrdescOficina)
        {
            string idLogDatos = "ObtenerValores";
            GeneradorLog objlog = new GeneradorLog(string.Format("[{0}][{1}][{2}]", strArchivo, "PROY-140715", idLogDatos), null, null, null);
            objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "INICIO MÉTODO ObtenerValores"), null, null);

            Dictionary<string, string> dcParameters = new Dictionary<string, string>();
            try
            {
                string sistema = Funciones.CheckStr(Strsistema);
                string codCtg = Funciones.CheckStr(StrcodCtg);
                string codCanal = Funciones.CheckStr(StrcodCanal);
                string codOficina = Funciones.CheckStr(StrcodOficina);
                string descOficina = Funciones.CheckStr(StrdescOficina);

                dcParameters.Add("sistema", sistema);
                dcParameters.Add("codCtg", codCtg);
                dcParameters.Add("codCanal", codCanal);
                dcParameters.Add("codOficina", codOficina);
                dcParameters.Add("descOficina", descOficina);

                foreach (KeyValuePair<string, string> param in dcParameters)
                {
                    objlog.CrearArchivolog(string.Format("{0} => {1}", param.Key, param.Value), null, null);
                }
            }
            catch (Exception ex)
            {
                objlog.CrearArchivolog(string.Format("Error => {0}", ex.Message), null, null);
                objlog.CrearArchivolog(string.Format("Error => {0}", ex.StackTrace), null, null);
            }
            objlog.CrearArchivolog(string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO ObtenerValores"), null, null);
            return dcParameters;
        }
    }
}
