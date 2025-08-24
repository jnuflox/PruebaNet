using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity.DataPowerRest.Generic;
using Claro.SISACT.Common;
using Claro.SISACT.WS.RestServices;
using Claro.SISACT.Entity;
using Claro.SISACT.Entity.claro_vent_ventascontingencia;
using System.Collections;
using System.Configuration;


namespace Claro.SISACT.WS.RestReferences
{
    public class RestConsultarVentasContingencia //PROY-140715
    {
        static string strArchivo = "RestConsultarVentasContingencia";

        public MessageResponseVentasContingencia ConsultarVentasContingencia(BEVenta objBEVenta, BEAuditoriaRequest objBEAuditoriaRequest)
        {
            string idLogDatos = "ConsultarVentasContingencia";
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}{1}{0}", "*********************", "INICIO MÉTODO ConsultarVentasContingencia"));
            try
            {
                Hashtable paramHeader = new Hashtable();
                paramHeader.Add("timestamp", objBEAuditoriaRequest.timestamp);
                paramHeader.Add("userId", objBEAuditoriaRequest.userId);
                paramHeader.Add("idTransaccion", objBEAuditoriaRequest.idTransaccion);

                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}{1}{0}", "--------------------", "Parametros de entrada Inicio"));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("timestamp => {0}", objBEAuditoriaRequest.timestamp));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("userId => {0}", objBEAuditoriaRequest.userId));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("idTransaccion => {0}", objBEAuditoriaRequest.idTransaccion));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("accept => {0}", objBEAuditoriaRequest.accept));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("ipApplication => {0}", objBEAuditoriaRequest.ipApplication));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}{1}{0}", "--------------------", "Parametros de entrada Fin"));

                string userEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["User_VentasContingencia"]);
                string passEncriptado = Funciones.CheckStr(ConfigurationManager.AppSettings["Pass_VentasContingencia"]);
                string strNombreTimeOut = Funciones.CheckStr(ConfigurationManager.AppSettings["Time_Out_VentasContingencia"]);

                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("userEncriptado => {0}", userEncriptado));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("passEncriptado => {0}", passEncriptado));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("strNombreTimeOut => {0}", strNombreTimeOut));

                Dictionary<string, string> parametros = new Dictionary<string, string>();

                parametros = ObtenerValores(objBEVenta);

                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO ConsultarVentasContingencia"));

                return RestServiceConsultarVentasContingencia.GetInvoque<MessageResponseVentasContingencia>("consUrlConsultarVentasCtg", paramHeader, parametros, userEncriptado, passEncriptado, strNombreTimeOut);
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("Error => {0}", ex.Message));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("Error => {0}", ex.StackTrace));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO ConsultarVentasContingencia"));
                throw;
            }
        }

        public Dictionary<string, string> ObtenerValores(BEVenta objBEVenta)
        {
            string idLogDatos = "ObtenerValores";
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}{1}{0}", "*********************", "INICIO MÉTODO ObtenerValores"));

            Dictionary<string, string> dcParameters = new Dictionary<string, string>();
            try
            {
                string tipoDoc = Funciones.CheckStr(objBEVenta.tipoDoc);
                string numDoc = Funciones.CheckStr(objBEVenta.numDoc);
                string numSec = Funciones.CheckStr(objBEVenta.numSec);
                string numPedido = Funciones.CheckStr(objBEVenta.numPedido);
                string fechaInicio = Funciones.CheckStr(objBEVenta.fechaInicio);
                string fechaFin = Funciones.CheckStr(objBEVenta.fechaFin);
                string estadoValidacion = Funciones.CheckStr(objBEVenta.estadoValidacion);
                string codCanal = Funciones.CheckStr(objBEVenta.codCanal);
                string codPdv = Funciones.CheckStr(objBEVenta.codPdv);
                string tipoContingencia = Funciones.CheckStr(objBEVenta.tipoContingencia);
                string estadoPago = Funciones.CheckStr(objBEVenta.estadoPago);

                dcParameters.Add("tipoDoc", tipoDoc);
                dcParameters.Add("numDoc", numDoc);
                dcParameters.Add("numSec", numSec);
                dcParameters.Add("numPedido", numPedido);
                dcParameters.Add("fechaInicio", fechaInicio);
                dcParameters.Add("fechaFin", fechaFin);
                dcParameters.Add("estadoValidacion", estadoValidacion);
                dcParameters.Add("codCanal", codCanal);
                dcParameters.Add("codPdv", codPdv);
                dcParameters.Add("tipoContingencia", tipoContingencia);
                dcParameters.Add("estadoPago", estadoPago);

                foreach (KeyValuePair<string, string> param in dcParameters)
                {
                    GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0} => {1}", param.Key, param.Value));
                }
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("Error => {0}", ex.Message));
                GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("Error => {0}", ex.StackTrace));
            }
            GeneradorLog.EscribirLog(strArchivo, idLogDatos, string.Format("{0}{1}{0}", "*********************", "FIN MÉTODO ObtenerValores"));
            return dcParameters;
        }

    }
}
