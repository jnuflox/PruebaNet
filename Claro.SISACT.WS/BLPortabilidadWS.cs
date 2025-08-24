using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.WS;

namespace Claro.SISACT.WS
{
    public class BLPortabilidadWS
    {
        public static BEItemMensaje ValidarNroPortabilidad(string[] listaTelefono)
        {
            Int64 nroSEC = 0;
            BEItemMensaje objMensaje = new BEItemMensaje();

            bool blnOK = new BLPortabilidad().ValidarDisponiblePortabilidad(listaTelefono, ref nroSEC);           
            if (! blnOK)
            {
                objMensaje.exito = false;
                objMensaje.mensajeSistema = string.Format(ConfigurationManager.AppSettings["consMsjErrorTelefonoPortaIII"], nroSEC.ToString());
            }

            return objMensaje;
        }
    }
}
