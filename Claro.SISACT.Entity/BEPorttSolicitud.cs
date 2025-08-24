using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPorttSolicitud
    {
        public Int64 idPortabilidad { get; set; }
        public Int64 numeroSEC { get; set; }
        public string numeroLinea { get; set; }
        public string fechaActivacionCP { get; set; }
        public string fechaEnvioCP { get; set; }
        public string flagEnvioCP { get; set; }
        public string codigoEstadoCP { get; set; }
        public string descripcionEstadoCP { get; set; }
        public string codigoMotivocP { get; set; }
        public string descripcionMotivoCP { get; set; }
        public string deudaCP { get; set; }
        public Int32 numeroIntentosCP { get; set; }
        public Int32 flagCPPermitida { get; set; }
        public int cantidadMesesOperadorCedente
        {
            get
            {
                DateTime _fechaActivacionCP = DateTime.MinValue;
                DateTime _fechaProceso = DateTime.Now;
                if (!string.IsNullOrEmpty(fechaActivacionCP) && DateTime.TryParse(fechaActivacionCP, out _fechaActivacionCP))
                {
                    if (_fechaActivacionCP <= Convert.ToDateTime(_fechaProceso))
                    {
                        _fechaProceso = (_fechaProceso.Month == 2 && _fechaActivacionCP.Month != _fechaProceso.Month) ? _fechaProceso.AddDays(1) : _fechaProceso;                
                        _fechaProceso =
                            (_fechaProceso.Month != 2 &&
                                ((_fechaProceso.Day == 30 && DateTime.DaysInMonth(_fechaProceso.Year, _fechaProceso.Month) == 30) &&
                                (_fechaActivacionCP.Day == 31 && DateTime.DaysInMonth(_fechaActivacionCP.Year, _fechaActivacionCP.Month) == 31))
                             ) ? _fechaProceso.AddDays(1) : _fechaProceso;
                        //Factor
                        int factorCP = _fechaProceso.Day >= _fechaActivacionCP.Day ? 0 : -1;
                        //Calculo                        
                        return Math.Abs(factorCP + ((_fechaProceso.Year - _fechaActivacionCP.Year) * 12) + _fechaProceso.Month - _fechaActivacionCP.Month);
                    }
                    else
                    {
                        return -1;
                    }                    
                }
                else
                {
                    return -1;
                }
            }
        }
    
        //PROY-31393 INI
        public string operadorCedente { get; set; }
        public string tipoProducto { get; set; }
        public string modalidadOrigen { get; set; }
        public string flagCPPermitidos { get; set; }
        //PROY-31393  FIN


        //INI: PROY-140335 RF1
        public string TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public Int64 idLinea { get; set; }
        public string secuencialCP { get; set; }
        public string flagConsultaPrevia { get; set; }
        public string ejecucionConsultaPrevia { get; set; }
        //FIN: PROY-140335 RF1
    }
}
