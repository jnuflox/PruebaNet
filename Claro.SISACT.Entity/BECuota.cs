using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BECuota
    {
        public string idFila { get; set; }
        public string idCuota { get; set; }
        public string cuota { get; set; }
        public int nroCuota { get; set; }
        public double porcenCuotaInicial { get; set; }
        /*---------PROY 29123----------*/
        public int maximoCuotas { get; set; }
        public double maximoPrecioSoles { get; set; }
        public string mensajeCuota { get; set; }

        public BECuota() 
        {
            idCuota = ConfigurationManager.AppSettings["constCodSinCuota"].ToString();
            cuota = ConfigurationManager.AppSettings["constSinCuota"].ToString();
            nroCuota = 0;
            porcenCuotaInicial = 100.00;        
        }

        /*INICIO PROY-31948*/
        public double montoPendienteCuotasSistema { get; set; } /*OAC*/
        public int cantidadPlanesCuotasPendientesSistema { get; set; } /*OAC*/
        public int cantidadMaximaCuotasPendientesSistema { get; set; } /*OAC*/

        public double montoPendienteCuotasUltimasVentas { get; set; } /*PVUDB*/
        public int cantidadPlanesCuotasPendientesUltimasVentas { get; set; } /*PVUDB*/
        public int cantidadMaximaCuotasPendientesUltimasVentas { get; set; } /*PVUDB*/

        public int cantidadCuotasPendientes { get; set; } /*OAC*/
        public double montoPendienteCuotas { get; set; } /*OAC*/
        /*FIN PROY-31948*/
    }
}
