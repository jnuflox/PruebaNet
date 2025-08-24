using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Data;
using System.Collections;

namespace Claro.SISACT.Business
{
    public class BLIntegracionDTHNegocio
    {
        public ArrayList ListarCentrosPobladosDistrito(string strUbigeo)
        {
            return new DAIntegracionDTHDatos().ListarCentrosPobladosDistrito(strUbigeo);
        }

        public ArrayList ListarCentrosPobladosDistrito_LTE(string strUbigeo, int iCoberturaDTH, int iCoberturaLTE)
        {
 
            return new DAIntegracionDTHDatos().ListarCentrosPobladosDistrito_LTE(strUbigeo, iCoberturaDTH, iCoberturaLTE);
        }

        public ArrayList ListarCentrosPoblados(string CentroPoblado, ref int codError)
        {
            return new DAIntegracionDTHDatos().ListarCentrosPoblados(CentroPoblado, ref codError);
        }
    }
}
