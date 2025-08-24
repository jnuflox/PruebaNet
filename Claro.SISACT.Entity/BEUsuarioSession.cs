using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEUsuarioSession
    {
        public Int64 idUsuario { get; set; }
        public Int64 idUsuarioSisact { get; set; }
        public string idVendedorSap { get; set; }
        public string idCuentaRed { get; set; }
        public string EstadoAcceso { get; set; }
        public string Nombre { get; set; }
        public string Apellido_Pat { get; set; }
        public string Apellido_Mat { get; set; }
        public string NombreCompleto { get; set; }
        public string CadenaPerfil { get; set; }
        public string CadenaOpcionesPagina { get; set; }
        public bool Perfil149 { get; set; }
        public string Login { get; set; }
        public string idArea { get; set; }
        public string Area { get; set; }
        public string Host { get; set; }
        public string OficinaVenta { get; set; }
        public string OficinaVentaDescripcion { get; set; }
        public string CanalVenta { get; set; }
        public string CanalVentaDescripcion { get; set; }
        public string Terminal { get; set; } //PROY-24740
        //PROY-140245
        public string numeroDocumento { get; set; }
        //FIN  PROY-140245
        public string TipoOficina { get; set; }

        //CNH
        public string TimeOutServicio { get; set; }
    }
}
