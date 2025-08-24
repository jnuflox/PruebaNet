using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDireccionCliente
    {
        public int IdFila { get; set; }
        public string IdTipoProducto { get; set; }
        public string IdTipoOperacion { get; set; }
        public string IdTipoDireccion { get; set; }
        public string IdPrefijo { get; set; }
        public string IdPrefijoSga { get; set; }
        public string Prefijo { get; set; }
        public string Direccion { get; set; }
        public string NroPuerta { get; set; }
        public string IdEdificacion { get; set; }
        public string Edificacion { get; set; }
        public string Manzana { get; set; }
        public string Lote { get; set; }
        public string IdTipoInterior { get; set; }
        public string TipoInterior { get; set; }
        public string NroInterior { get; set; }
        public string IdUrbanizacion { get; set; }
        public string IdUrbanizacionSga { get; set; }
        public string Urbanizacion { get; set; }
        public string TxtUrbanizacion { get; set; }
        public string IdDomicilio { get; set; }
        public string Domicilio { get; set; }
        public string IdZona { get; set; }
        public string Zona { get; set; }
        public string NombreZona { get; set; }
        public string Referencia { get; set; }
        public string Referencia_Sec { get; set; }
        public string IdDepartamento { get; set; }
        public string IdProvincia { get; set; }
        public string IdDistrito { get; set; }
        public string IdPostal { get; set; }
        public string IdUbigeo { get; set; }
        public string IdUbigeoInei { get; set; }
        public string IdTelefono { get; set; }
        public string Telefono { get; set; }
        public string IdCentroPoblado { get; set; }
        public string IdPlano { get; set; }
        public string DirCompleta { get; set; }
        public string DirCompletaSAP { get; set; }
        public string DirReferenciaSAP { get; set; }
        public string TelefonoReferencia1 { get; set; }
        public string TelefonoReferencia2 { get; set; }
        public string VentaProactiva { get; set; }
        public string DniVendedor { get; set; }
        public string FlagVOD { get; set; }
//gaa20140414
        public string IdEdificio { get; set; }
        public string IdUbigeoSGA { get; set; }
        public string VentaProgramada { get; set; }
//fin gaa20140414
        public int Cobertura_dth { get; set; }
        public int Cobertura_lte { get; set; }
        
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        
    }
}
