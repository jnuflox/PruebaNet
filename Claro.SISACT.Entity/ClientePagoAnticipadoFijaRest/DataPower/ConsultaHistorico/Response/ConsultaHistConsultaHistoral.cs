//PROY-140546
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity.ClientePagoAnticipadoFijaRest.DataPower.ConsultaHistorico.Response
{
    [Serializable]
    public class ConsultaHistConsultaHistoral
    {
        public string solinCodigo { get; set; }
        public string slplnCodigo { get; set; }
        public string prdcCodigo { get; set; }
        public string plnvCodigo { get; set; }
        public string plnvDescripcion { get; set; }
        public string solidFechaRegistro { get; set; }
        public string solinCostoInstalacion { get; set; }
        public string solinCostoInstalacionMan { get; set; }
        public string solivFormaPago { get; set; }
        public string solivFormaPagoMan { get; set; }
        public string solinNumeroCuota { get; set; }
        public string solinNumeroCuotaMan { get; set; }
        public string solinMontoAntiInst { get; set; }
        public string solinMontoAntInstMan { get; set; }
        public string solinGrupoSec { get; set; }
        public string solivPtota { get; set; }
        public string pacEstado { get; set; }
    }
}
