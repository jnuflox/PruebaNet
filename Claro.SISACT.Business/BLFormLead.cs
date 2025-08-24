using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Data;
using Claro.SISACT.Entity;

namespace Claro.SISACT.Business
{
    public class BLFormLead
    {
        //INI PROY-140739 Formulario Leads
        public static bool RegistrarFormularioLeads(BEFormLead objLead, ref string oNroError, ref string oDescError)
        {
            return new DAFormLead().RegistrarFormularioLeads(objLead, ref oNroError, ref oDescError);
        }

        public static bool RechazarSEC(Int64 solinCodigo, string estacCcodigo, string usuario, string comentario, ref string oNroError, ref string oDescError) 
        {
            return new DAFormLead().RechazarSEC(solinCodigo, estacCcodigo, usuario, comentario, ref oNroError, ref oDescError);
        }
        //FIN PROY-140739 Formulario Leads
    }
}
