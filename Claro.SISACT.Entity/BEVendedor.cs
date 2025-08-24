using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEVendedor
    {

        private string _nombre;
        private string _apellidos;
        private string _nombreCompleto;

        //Pablo Zea - Vendedores 20/05/2009 Inicio
        //private Estado _estado;
        //private Distribuidor _distribuidor;
        //private PuntoVenta _puntoVenta;
        //private Usuario _usuarioReg;
        //private Usuario _usuarioMod;
        //Pablo Zea - Vendedores 20/05/2009 Fin

        //T12646 Vendedores Inicio
        //T12646 Vendedores Fin

        public BEVendedor() { }

        public BEVendedor(string vCodigo, string vDescripcion)
        {
            IdDistribuidorVendedor = vCodigo;
            _nombre = vDescripcion;
        }

        public long VendedorId { get; set; }

        public string Nombre
        {
            set { _nombre = value; }
            get { return _nombre; }
        }
        public string Apellidos
        {
            set { _apellidos = value; }
            get { return _apellidos; }
        }
        public string NombreCompleto
        {
            set { _nombreCompleto = value; }
            get
            {
                if (_nombre != "" && _apellidos != "")
                    _nombreCompleto = _nombre + " " + _apellidos;
                return _nombreCompleto;
            }
        }

        public string TipoDocumento { get; set; }

        public string NumeroDocumento { get; set; }

        public string FechaNacimiento { get; set; }

        public string Direccion { get; set; }

        public string FechaRegistro { get; set; }

        public string FechaModificacion { get; set; }

        public string VerificacionReniec { get; set; }

        public string Motivo { get; set; }

        public string EstadoId { get; set; }

        public string EstadoDescripcion { get; set; }

        public string DistribuidorId { get; set; }

        public string PuntoVentaId { get; set; }

        public string PuntoVentaDescripcion { get; set; }

        public string DistribuidorDescripcion { get; set; }

        public string UsuarioRegistroId { get; set; }

        public string UsuarioModificacionId { get; set; }

        //Pablo Zea - Vendedores 20/05/2009 Inicio
        //public Estado Estado
        //{
        //    set { _estado = value; }
        //    get { return _estado; }
        //}
        //public string EstadoDesc
        //{
        //    set { _estado.ESTAV_DESCRIPCION = value; }
        //    get { return _estado.ESTAV_DESCRIPCION; }
        //}
        //public Distribuidor Distribuidor
        //{
        //    set { _distribuidor = value; }
        //    get { return _distribuidor; }
        //}
        //public string DistribuidorDesc
        //{
        //    set { _distribuidor.DISTV_DESCRIPCION = value; }
        //    get { return _distribuidor.DISTV_DESCRIPCION; }
        //}
        //public PuntoVenta PuntoVenta
        //{
        //    set { _puntoVenta = value; }
        //    get { return _puntoVenta; }
        //}
        //public string PuntoVentaDesc
        //{
        //    set { _puntoVenta.OVENV_DESCRIPCION = value; }
        //    get { return _puntoVenta.OVENV_DESCRIPCION; }
        //}
        //public Usuario UsuarioReg
        //{
        //    set { _usuarioReg = value; }
        //    get { return _usuarioReg; }
        //}
        //public string UsuarioRegDesc
        //{
        //    set { _usuarioReg.NombreCompleto = value; }
        //    get { return _usuarioReg.NombreCompleto; }
        //}
        //public Usuario UsuarioMod
        //{
        //    set { _usuarioMod = value; }
        //    get { return _usuarioMod; }
        //}
        //public string UsuarioModDesc
        //{
        //    set { _usuarioMod.NombreCompleto = value; }
        //    get { return _usuarioMod.NombreCompleto; }
        //}

        public ArrayList Historial { get; set; }

        public string FechaHabilitado { get; set; }

        public string FechaDeshabilitado { get; set; }

        //Pablo Zea - Vendedores 20/05/2009 Fin

        //T12646 Vendedores Inicio
        public string FlagBl { get; set; }

        public string IdDistribuidor { get; set; }

        public string IdDistribuidorVendedor { get; set; }

        //T12646 Vendedores Fin

        //DRC
        public string ClaveAcceso { get; set; }

    }

}
