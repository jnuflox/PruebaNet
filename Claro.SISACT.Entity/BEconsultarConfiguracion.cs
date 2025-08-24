using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Claro.SISACT.Entity
{
    [Serializable]
    [DataContract]
    public class BEconsultarConfiguracion
    {
        [DataMember(Name = "configuracionGeneral")]
        public List<BEconfiguracionGeneral> configuracionGeneral { get; set; }

        [DataMember(Name = "configuracionCanal")]
        public List<BEconfiguracionCanal> configuracionCanal { get; set; }

        [DataMember(Name = "configuracionPuntoVenta")]
        public List<BEconfiguracionPuntoVenta> configuracionPuntoVenta { get; set; }

        [DataMember(Name = "configuracionOperacion")]
        public List<BEconfiguracionOperacion> configuracionOperacion { get; set; }

        [DataMember(Name = "configuracionVenta")]
        public List<BEconfiguracionVenta> configuracionVenta { get; set; }

        [DataMember(Name = "configuracionProducto")]
        public List<BEconfiguracionProducto> configuracionProducto { get; set; }

        public BEconsultarConfiguracion()
        {
            configuracionGeneral = new List<BEconfiguracionGeneral>();
            configuracionCanal = new List<BEconfiguracionCanal>();
            configuracionPuntoVenta = new List<BEconfiguracionPuntoVenta>();
            configuracionOperacion = new List<BEconfiguracionOperacion>();
            configuracionVenta = new List<BEconfiguracionVenta>();
            configuracionProducto = new List<BEconfiguracionProducto>();
        }

    }
}
