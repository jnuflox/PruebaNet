using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;
using Claro.SISACT.Common;
using System.Collections;
using System.Configuration;
using System.Data;

namespace Claro.SISACT.Business
{
    public class BLProteccionMovil //PROY-24724-IDEA-28174 - NUEVA CLASE
    {
        public void BorrarServicioProteccionMovil(string strNroSec, string strCodServProteccionMovil, string strSoplnCodigo, ref string strCodRespuesta, ref string strMsjRespuesta)
        {
            new DAProteccionMovil().BorrarServicioProteccionMovil(strNroSec, strCodServProteccionMovil, strSoplnCodigo, ref strCodRespuesta, ref strMsjRespuesta);
        }        
        public List<BEPrima> BuscarProteccionMovilPvu(string strNroCertif, string strNroSec, string strSoplnCodigo, ref string strCodRespuesta, ref string strMsjRespuesta)
        {
            return new DAProteccionMovil().BuscarProteccionMovilPvu(strNroCertif, strNroSec, strSoplnCodigo, ref strCodRespuesta, ref strMsjRespuesta);
        }
        public void ActualizarMontoCargoFijoServicioProteccionMovil(string strNroSec, string strSoplnCodigo, string strCodServicio, ref string strCodRespuesta, ref string strMsjRespuesta)
        {
            new DAProteccionMovil().ActualizarMontoCargoFijoServicioProteccionMovil(strNroSec, strSoplnCodigo, strCodServicio, ref strCodRespuesta, ref strMsjRespuesta);
        }
    }
}
