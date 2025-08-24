//PROY-140579 NN - INICIO BRMS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity.RegistrarEvaluacionBRMSDP.Request
{
    [DataContract]
    [Serializable] 
    public class RegistrarEvaluacionBrmsRequest
    {
        [DataMember(Name = "codigoTipoCliente")]
        public string codigoTipoCliente { get; set; }

        [DataMember(Name = "numeroDocumentoCliente")]
        public string numeroDocumentoCliente { get; set; }

        [DataMember(Name = "tipoTransaccion")]
        public string tipoTransaccion { get; set; }

        [DataMember(Name = "codigoSolin")]
        public string codigoSolin { get; set; }

        [DataMember(Name = "codigoSlpl")]
        public string codigoSlpl { get; set; }

        [DataMember(Name = "solicitud")]
        public string solicitud { get; set; }

        [DataMember(Name = "cliente")]
        public string cliente { get; set; }

        [DataMember(Name = "direccionCliente")]
        public string direccionCliente { get; set; }

        [DataMember(Name = "documentoCliente")]
        public string documentoCliente { get; set; }

        [DataMember(Name = "clienteRrll")]
        public string clienteRrll { get; set; }

        [DataMember(Name = "equipo")]
        public string equipo { get; set; }

        [DataMember(Name = "oferta")]
        public string oferta { get; set; }

        [DataMember(Name = "campana")]
        public string campana { get; set; }

        [DataMember(Name = "planActual")]
        public string planActual { get; set; }

        [DataMember(Name = "planSolicitada")]
        public string planSolicitada { get; set; }

        [DataMember(Name = "servicio")]
        public string servicio { get; set; }

        [DataMember(Name = "pdv")]
        public string pdv { get; set; }

        [DataMember(Name = "direccionPdv")]
        public string direccionPdv { get; set; }

        [DataMember(Name = "cantidadLineasAdicionales")]
        public string cantidadLineasAdicionales { get; set; }

        [DataMember(Name = "cantidadLineasMaxima")]
        public string cantidadLineasMaxima { get; set; }

        [DataMember(Name = "cantidadLineasRenovacion")]
        public string cantidadLineasRenovacion { get; set; }

        [DataMember(Name = "montoCfRuc")]
        public string montoCfRuc { get; set; }

        [DataMember(Name = "tipoCargoFijo")]
        public string tipoCargoFijo { get; set; }

        [DataMember(Name = "campaniaRestrinccion")]
        public string campaniaRestrinccion { get; set; }

        [DataMember(Name = "cantidadCuotasCi")]
        public string cantidadCuotasCi { get; set; }

        [DataMember(Name = "cobroAnticipadoInstalacion")]
        public string cobroAnticipadoInstalacion { get; set; }

        [DataMember(Name = "controlConsumo")]
        public string controlConsumo { get; set; }

        [DataMember(Name = "costoInstalacion")]
        public string costoInstalacion { get; set; }

        [DataMember(Name = "cuotaCantidad")]
        public string cuotaCantidad { get; set; }

        [DataMember(Name = "cuotaPorcentajeInicial")]
        public string cuotaPorcentajeInicial { get; set; }

        [DataMember(Name = "ejecucionConsultaBuro")]
        public string ejecucionConsultaBuro { get; set; }

        [DataMember(Name = "formaPagoCi")]
        public string formaPagoCi { get; set; }

        [DataMember(Name = "cantidadAplicacionRenta")]
        public string cantidadAplicacionRenta { get; set; }

        [DataMember(Name = "frencuenciaApliMensual")]
        public string frencuenciaApliMensual { get; set; }

        [DataMember(Name = "mesInicioRenta")]
        public string mesInicioRenta { get; set; }

        [DataMember(Name = "montoGarantia")]
        public string montoGarantia { get; set; }

        [DataMember(Name = "tipoCobro")]
        public string tipoCobro { get; set; }

        [DataMember(Name = "tipoGarantia")]
        public string tipoGarantia { get; set; }

        [DataMember(Name = "limiteCreditoCobranza")]
        public string limiteCreditoCobranza { get; set; }

        [DataMember(Name = "limiteCreditoDisponible")]
        public string limiteCreditoDisponible { get; set; }

        [DataMember(Name = "montoAutomatico")]
        public string montoAutomatico { get; set; }

        [DataMember(Name = "mostrarMotivoRestriccion")]
        public string mostrarMotivoRestriccion { get; set; }

        [DataMember(Name = "motivoRestriccion")]
        public string motivoRestriccion { get; set; }

        [DataMember(Name = "opCuotaMaxima")]
        public string opCuotaMaxima { get; set; }

        [DataMember(Name = "opCuotaMostrarRespuesta")]
        public string opCuotaMostrarRespuesta { get; set; }

        [DataMember(Name = "opCuotaTopeMaximo")]
        public string opCuotaTopeMaximo { get; set; }

        [DataMember(Name = "prioridadPublica")]
        public string prioridadPublica { get; set; }

        [DataMember(Name = "procesoExoneraRenta")]
        public string procesoExoneraRenta { get; set; }

        [DataMember(Name = "procesoIdValitor")]
        public string procesoIdValitor { get; set; }

        [DataMember(Name = "validacionInternaClaro")]
        public string validacionInternaClaro { get; set; }

        [DataMember(Name = "publicar")]
        public string publicar { get; set; }

        [DataMember(Name = "restriccion")]
        public string restriccion { get; set; }

        [DataMember(Name = "resultadoEvaluacionCuota")]
        public string resultadoEvaluacionCuota { get; set; }

        [DataMember(Name = "tipoCobroAnticipadoIns")]
        public string tipoCobroAnticipadoIns { get; set; }

        [DataMember(Name = "resCapacidadPago")]
        public string resCapacidadPago { get; set; }

        [DataMember(Name = "resComportamientoConsolidado")]
        public string resComportamientoConsolidado { get; set; }

        [DataMember(Name = "resComportamientoPago")]
        public string resComportamientoPago { get; set; }

        [DataMember(Name = "resCostoTotalEquipo")]
        public string resCostoTotalEquipo { get; set; }

        [DataMember(Name = "resFactorDeudamientoCli")]
        public string resFactorDeudamientoCli { get; set; }

        [DataMember(Name = "resFactorRenovacionCli")]
        public string resFactorRenovacionCli { get; set; }

        [DataMember(Name = "resPrecioVentaEquipo")]
        public string resPrecioVentaEquipo { get; set; }

        [DataMember(Name = "resRiesgoClaro")]
        public string resRiesgoClaro { get; set; }

        [DataMember(Name = "resRiesgoOferta")]
        public string resRiesgoOferta { get; set; }

        [DataMember(Name = "resRiesgoTotalEquipo")]
        public string resRiesgoTotalEquipo { get; set; }

        [DataMember(Name = "resRiesgoTotalReplegable")]
        public string resRiesgoTotalReplegable { get; set; }

        [DataMember(Name = "respuestaWs")]
        public string respuestaWs { get; set; }

    }
}
//PROY-140579 NN - FIN BRMS