using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    //PROY-140457-DEBITO AUTOMATICO
    [Serializable]
    public class BEDebitoAutomatico
    {
        public Int64 Solin_Codigo { get; set; }

        public string Descripcion_TipoDoc { get; set; }

        public string Tipo_Documento { get; set; }

        public string Numero_Documento { get; set; }

        public string Nombre_Cliente { get; set; }

        public string Cod_Solicitud { get; set; }

        public string Desc_Solicitud { get; set; }

        public string Codigo_Entidad { get; set; }

        public string Descripcion_Entidad { get; set; }

        public string Tipo_Cuenta { get; set; }

        public string Descripcion_Cuenta { get; set; }

        public string Numero_Cuenta { get; set; }

        public string Fecha_Vencimiento { get; set; }

        public string Flag_MontoTope { get; set; }

        public string MontoTope { get; set; }

        public string Tipo_Cliente { get; set; }

        public string Telefono_Contacto { get; set; }

        public string Correo_Contacto { get; set; }

        public string Codigo_Cliente { get; set; }

        public string Tipo_Operacion { get; set; }

        public string Flag_Portabilidad { get; set; }

        public string Canal { get; set; }

        public string Codigo_PuntoVenta { get; set; }

        public string Punto_Venta { get; set; }

        public string Datos_Detalle { get; set; }

        public string Usuario { get; set; }

        public int flagTarjetaAfiliacion { get; set; }

        public string idAfiliacion { get; set; }

        public string idTarjeta { get; set; }

        public string nroTarjeta { get; set; }
    }
}
