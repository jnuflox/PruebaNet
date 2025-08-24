using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEClienteSAP
    {

        public string Cliente { get; set; }
        public string TipoDocCliente { get; set; }
        public string TipoCliente { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string RazonSocial { get; set; }
        public string FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string EMail { get; set; }
        public string NombreConyuge { get; set; }
        public string CargaFamiliar { get; set; }
        public string Sexo { get; set; }
        public string CalleLegal { get; set; }
        public string UbigeoLegal { get; set; }
        public string CalleFact { get; set; }
        public string UbigeoFact { get; set; }
        public string ReplegalTipDoc { get; set; }
        public string ReplegalNroDoc { get; set; }
        public string ReplegalNombre { get; set; }
        public string ReplegalApePat { get; set; }
        public string ReplegalApeMat { get; set; }
        public string ReplegalTelefon { get; set; }
        public string ContactoTipDoc { get; set; }
        public string ContactoNroDoc { get; set; }
        public string ContactoNombre { get; set; }
        public string ContactoApePat { get; set; }
        public string ContactoApeMat { get; set; }
        public string ContactoTelefon { get; set; }
        public string Cargo { get; set; }
        public string Dependiente { get; set; }
        public string EmpresaLabora { get; set; }
        public string EmpresaCargo { get; set; }
        public string EmpresaTelefono { get; set; }
        public string Actividad { get; set; }
        public Decimal IngBruto { get; set; } //decimal
        public Decimal OtrosIngresos { get; set; } //decimal
        public string TarjetaCredito { get; set; }
        public string NumTarjCredito { get; set; }
        public string MonedaTcred { get; set; }
        public Decimal LineaCredito { get; set; } //decimal
        public string FechaVencTcred { get; set; }
        public string Clasificacion { get; set; }
        public string ClaseCliente { get; set; }
        public string Ramo { get; set; }
        public string Observaciones { get; set; }
        public string EstadoCivil { get; set; }
        public string TitCliente { get; set; }
        public string ReplegalTit { get; set; }
        public string ReplegalFnac { get; set; }
        public string ReplegalSexo { get; set; }
        public string ReplegalEstCiv { get; set; }
        public string Ktokd { get; set; }
        public string ReferDireccion { get; set; }
        public string TelfPref { get; set; }
        public string FaxPref { get; set; }
        public string TelefLegal { get; set; }
        public string Operador { get; set; }
        public string DenomOperador { get; set; }
        public string TipoProdOperad { get; set; }
        public string DenomTpoProdOp { get; set; }
        public string TelefLegalPref { get; set; }
        public string ReferLegal { get; set; }
        public string Kunnr { get; set; }

        // Estos parametros no son los que sap devuelve
        public string DireccionLegalPref { get; set; } // Nuevo
        public string DireccionFactPref { get; set; } // Nuevo 
        public string DireccionFact { get; set; } // Nuevo 
        public int ClienCondCliente { get; set; } // Nuevo  

        //INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
        public string EmailFact { get; set; }
        public string ApePatConyuge { get; set; }
        public string ApeMatConyuge { get; set; }
        public string DireccionLegal { get; set; }
        public string DireccionFactRefe { get; set; }
        public string TelfFact { get; set; }
        public string VendedorSap { get; set; }
        public string UsuarioCrea { get; set; }
        public string FecCrea { get; set; }
        public string CliCodigSap { get; set; }
        //FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18

        public string CliCodNacion { get; set; } //INC000003442281
        public string CliDescNacion { get; set; } //INC000003442281

    }
}
