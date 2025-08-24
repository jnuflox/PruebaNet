using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEConsultaDatosOficina
    {
        public string CodMaterial { get; set; } //Codigo Material
        public string CodOficina { get; set; } //Codigo Oficina   
        public string TipoOficina { get; set; }// Tipo o Codigo de Clasificacion Oficina Venta
        
        public string Canal { get; set; } //Canal de Distribucion
        
        public string Sector { get; set; } //Sector
        public string CodCentro { get; set; } //Codigo Centro
        public string CodAlmacen { get; set; } //Codigo Almacen
        public string OrgVta { get; set; } //Organizacion de Ventas

        public string IndModFecVta { get; set; } //Indicador Modificacion de Fecha Venta
        public int PlazoDevolucion { get; set; } //Plazo Devolucion
        public int MontoMinRecarga { get; set; } //Monto Minimo Recarga
        public int MontoMaxRecarga { get; set; } //Monto Maximo Recarga
        public string CodClienteVarios { get; set; } //Codigo Cliente Varios
        public string NroIdentificacionFiscalCPD { get; set; } //Numero de Identificacion Fiscal CPD


        public string CodigoRegion { get; set; } //Codigo de Departamento o Region del 01 al 25
        public string DescripcionOficina { get; set; } //Descripcion o nombre de la Oficina
        
        public string CodigoUsuario { get; set; } //Codigo de Usuario
        public string CodigoTipoProducto { get; set; }//Codigo de Tipo de Producto

        public string CodigoInterlocutor { get; set; }//Codigo Sinergia para CAD y DAD = Interlocutor para CAC=Codigo Oficina MSSAP 4.6
    
    }
}
