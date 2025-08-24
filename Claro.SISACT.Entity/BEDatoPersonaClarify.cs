using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//PROY-25906-CNH
namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
  public class BEDatoPersonaClarify
  {

    public string TipoDocumento { get; set; }
    public string NroDocumento { get; set; }
    public string Nombres { get; set; }
    public string ApePaterno { get; set; }
    public string ApeMaterno { get; set; }
    public string RazonSocial { get; set; }
    public string FecNacimiento { get; set; }
    public string Email { get; set; }
    public string Campo1 { get; set; }
    public string Campo2 { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? HoraRegistro { get; set; }
    public string NomAplicacion { get; set; }
    public string IdTransaccion { get; set; }
    public string IpdAplicacion { get; set; }
    public string UsuarioAplicacion { get; set; }

    public string Genero { get; set; }
    public string FechaCaducidad { get; set; }
    public string TipoValidacion { get; set; }

    
    
  }
}

