using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEUsuario
    {

        public Int64 UsuarioId { get; set; }
		public string Login    { get; set; }
        public string Nombre   { get; set; }
        public string Apellidos { get; set; }
        public string NombreCompleto
        {
            set { NombreCompleto = value; }
            get
            {
                if (Nombre != "" && Apellidos != "")
                    NombreCompleto = Nombre + " " + Apellidos;
                return NombreCompleto;
            }
        }

        public string AreaUsuario { get; set; }
        public string Perfil { get; set; }
        public bool FlagConsulta { get; set; }
        public string OpcionesMenu { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public ArrayList OpcionesSeguridad { get; set; }
        public string CtaRed { get; set; }
        public string OficinaId { get; set; }
        public string OficinaDes { get; set; }
        public string Estado { get; set; }
        public string TipoOficinaId { get; set; }
        public string TipoUsuario { get; set; }
        public string distribuidorId { get; set; }
        public string CodigoBSCS { get; set; }
        public string CodigoVendedor { get; set; }
        public string Cadena { get; set; }
        public string Cadena_desc { get; set; }
        public string OVENV_PAR_CONF_SKU { get; set; }
    }
}
