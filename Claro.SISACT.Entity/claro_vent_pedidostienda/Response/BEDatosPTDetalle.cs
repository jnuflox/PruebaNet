using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
//PROY-140618
namespace Claro.SISACT.Entity.claro_vent_pedidostienda.Response
{
    [DataContract]
    [Serializable]
    public class BEDatosPTDetalle
    {
        public BEDatosPTDetalle()
        {
        }

        [DataMember(Name = "idSolicitud")]
        public string idSolicitud { get; set; }

        [DataMember(Name = "fechaSolicitud")]
        public string fechaSolicitud { get; set; }


         [DataMember(Name = "solinCodigo")]
        public string solinCodigo { get; set; }

         [DataMember(Name = "pedidoTienda")]
         public string pedidoTienda { get; set; }

         [DataMember(Name = "pedidoSinergia")]
         public string pedidoSinergia { get; set; }


        [DataMember(Name = "precioTienda")]
        public string precioTienda { get; set; }


         [DataMember(Name = "precioSisact")]
        public string precioSisact { get; set; }


         [DataMember(Name = "codigoOficina")]
        public string codigoOficina { get; set; }

        [DataMember(Name = "descOficina")]
        public string descOficina { get; set; }

        [DataMember(Name = "tipoOperacion")]
        public string tipoOperacion { get; set; }


        [DataMember(Name = "fechaRegistro")]
        public string fechaRegistro { get; set; }


          [DataMember(Name = "usuarioRegistro")]
        public string usuarioRegistro { get; set; }

           [DataMember(Name = "estadoSolicitud")]
        public string estadoSolicitud { get; set; }

           [DataMember(Name = "tipoVenta")]
        public string tipoVenta { get; set; }

           [DataMember(Name = "modalidadVenta")]
        public string modalidadVenta { get; set; }

          [DataMember(Name = "estadoPosterior")]
        public string estadoPosterior { get; set; }


          [DataMember(Name = "idFlujo")]
        public string idFlujo { get; set; }


          [DataMember(Name = "comentario")]
        public string comentario { get; set; }

          [DataMember(Name = "usuarioAprobador")]
        public string usuarioAprobador { get; set; }

        [DataMember(Name = "fechaAprobacion")]
        public string fechaAprobacion { get; set; }

        [DataMember(Name = "fechaVenta")]
        public string fechaVenta { get; set; }

        [DataMember(Name = "tipoProducto")]
        public string tipoProducto { get; set; }
        //Aprobar

        [DataMember(Name = "id")]
        public string id { get; set; }


        [DataMember(Name = "usuarioAprob")]
        public string usuarioAprob { get; set; }


        [DataMember(Name = "nodoAprob")]
        public string nodoAprob { get; set; }

        //Anular


        [DataMember(Name = "codigo")]
        public string codigo { get; set; }

        [DataMember(Name = "usuario")]
        public string usuario { get; set; }

        [DataMember(Name = "oficinaSinergia")]
        public string oficinaSinergia { get; set; }

        [DataMember(Name = "estado")]
        public string estado { get; set; }

        [DataMember(Name = "oficinaVenta")]
        public string oficinaVenta { get; set; }

        [DataMember(Name = "cuotaInicialTV")]
        public string cuotaInicialTV { get; set; }

        [DataMember(Name = "cuotaInicialSISACT")]
        public string cuotaInicialSISACT { get; set; }


        [DataMember(Name = "campana")]
        public string campana { get; set; }

        [DataMember(Name = "plazo")]
        public string plazo { get; set; }

        [DataMember(Name = "codMaterial")]
        public string codMaterial { get; set; }

        [DataMember(Name = "plan")]
        public string plan { get; set; }

        [DataMember(Name = "nodoVenta")]
        public string nodoVenta { get; set; }

        [DataMember(Name = "precioVenta")]
        public string precioVenta { get; set; }

         [DataMember(Name = "usuarioValidador")]
        public string usuarioValidador { get; set; }
        
         [DataMember(Name = "nodoValidador")]
         public string nodoValidador { get; set; }

         [DataMember(Name = "observacionValidador")]
         public string observacionValidador { get; set; }
    }
}
