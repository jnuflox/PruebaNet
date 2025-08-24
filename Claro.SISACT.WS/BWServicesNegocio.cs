using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Configuration;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS.MASServiceServiceWS;

namespace Claro.SISACT.WS
{
//INI PROY-31948_Migracion
   public class BWServicesNegocio
    {
        MASServiceService _oSMS = new MASServiceService();        

        public BWServicesNegocio()
        {
            _oSMS.Url = Funciones.CheckStr(ConfigurationManager.AppSettings["constSMSRuta"]);            
        }

        public bool EnvioSMS(string telefono, string from, string mensaje, string cod_aplicacion)
        {
            try
            {
                _oSMS.sendSMSPush(cod_aplicacion, from, telefono, mensaje);                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }//FIN PROY-31948_Migracion
}
