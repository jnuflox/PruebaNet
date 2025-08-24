//PROY-32129 :: INI
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;

namespace Claro.SISACT.Business
{
    public class BLCasoEspecial
    {
        public List<BEItemGenerico> ListarInstituciones()
        {
            return new DACasoEspecial().ListarInstituciones();
        }

        public bool GrabarDatosAlumno(string strTipoDocumento, string strNumeroDocumento, Int64 intCodInstitucion, string strCodPersona, string strUsuario, ref Int64 intCodResp, ref string strMensResp)
        {
            return new DACasoEspecial().GrabarDatosAlumno(strTipoDocumento, strNumeroDocumento, intCodInstitucion, strCodPersona, strUsuario, ref intCodResp, ref strMensResp);
        }

        public bool AnularDatosAlumno(string strTipoDocumento, string strNumeroDocumento, string strUsuario, ref Int64 intCodResp, ref string strMensResp)
        {
            return new DACasoEspecial().AnularDatosAlumno(strTipoDocumento, strNumeroDocumento, strUsuario, ref intCodResp, ref strMensResp);
        }

        //PROY FASE 2 INICIO EIQ
        public bool WhiteList_x_IdCampana(string strNroDocumento, ref Int64 intCodResp, ref string strMensResp)
        {
            return new DACasoEspecial().WhiteList_x_IdCampana(strNroDocumento, ref intCodResp, ref strMensResp);
        }
        //PROY FASE 2 FIN EIQ
    }
}
//PROY-32129 :: FIN