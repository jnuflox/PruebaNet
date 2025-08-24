using System;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BECliente
    {
        public  string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public string NumeroLinea { get; set; }

        public string IdCliente { get; set; }
        public double Deuda { get; set; }
        public Int64 Lineas { get; set; }
        public double Antiguedad { get; set; }
        public double AntiguedadCliente { get; set; }
        public string RazonSocial { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string CodigoBloqueo { get; set; }
        public string DireccionFactura { get; set; }
        public string ContactoFactura { get; set; }
        public string TelefonoDomicilio { get; set; }
        public Int64 CustomerId { get; set; }
        public string Custcode { get; set; }
        public string Rucdni { get; set; }
        public double FactProm { get; set; }
        public double MontoVenc { get; set; }
        public double MontoPend { get; set; }
        public double FactSec { get; set; }
        public string Direccion { get; set; }
        public int IdTipDoc { get; set; }
        public string TipDoc { get; set; }
        public double MontoCast { get; set; }
        public string FechaVenc { get; set; }
        public string DiasVenc { get; set; }
        public string Provincia { get; set; }
        public int CantidadCorteParcial { get; set; }
        public int CantidadCorteTotal { get; set; }
        public int CantidadCorteBajas { get; set; }
        public int CantidadLineasActivas { get; set; }
        public int CantidadLineasSuspendidas { get; set; }
        public int BloqueosPedidoCliente { get; set; }
        public int BloqueosCobranza { get; set; }
        public int BloqueosFinanciamiento { get; set; }
        public int BloqueosFraude { get; set; }
        public int BloqueosLimiteCredito { get; set; }
        public int BloqueosRobo { get; set; }
        public int LineasMayorNDias { get; set; }
        public int LineasMenorNDias { get; set; }
        public double PromFact { get; set; }
        public string ChkCorreo { get; set; }
        public string Correo { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NroDoc { get; set; }
        public string NombreCompleto { get; set; }
        public string Domicilio { get; set; }
        public string EstadoContacto { get; set; }
        public string TelefReferencia { get; set; }
        public string Ciudad { get; set; }
        public string FechaNac { get; set; }
        public string LugarNacimientoDes { get; set; }
        public string MotivoRegistro { get; set; }
        public string Usuario { get; set; }
        public string ObjidContacto { get; set; }
        public int FlagRegistrado { get; set; }
        public string Modalidad { get; set; }
        public string Telefono { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public string Ocupacion { get; set; }
        public string Cargo { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Distrito { get; set; }
        public string Zipcode { get; set; }
        public string Urbanizacion { get; set; }
        public string Departamento { get; set; }
        public string Referencia { get; set; }
        public string FlagEmail { get; set; }
        public string ObjIdSite { get; set; }
        public string Cuenta { get; set; }
        public string Segmento { get; set; }
        public string RolContacto { get; set; }
        public string EstadoContrato { get; set; }
        public string EstadoSite { get; set; }
        public string SNombres { get; set; }
        public string SApellidos { get; set; }
        public DateTime FechaAct { get; set; }
        public string PuntoVenta { get; set; }
        public string CantReg { get; set; }
        public string Funcion { get; set; }
    }
}
