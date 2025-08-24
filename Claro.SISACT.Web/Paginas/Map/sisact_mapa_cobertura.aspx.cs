using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.Web.Base;
using System.Configuration;
using System.Text;
using Claro.SISACT.WS.RestReferences;
using Claro.SISACT.Entity.claroventacobertura.validarcobertura;

namespace Claro.SISACT.Web.Paginas.Map
{
    public partial class sisact_mapa_cobertura : Sisact_Webbase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region INICIATIVA-932 | MOVILIDAD IFI | BRYAN CHUMBES LIZARRAGA

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod ValidarCobertura(String strDireccion, String strLatitudLongitud, String flagCoordenadas)
        {

            GeneradorLog _objLog = new GeneradorLog(CurrentUsers, "", null, "WEB");
            _objLog.CrearArchivolog("INICIO INICIATIVA 932 - Movilidad IFI", null, null);

            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            
            try
            {
                BEDireccionCliente objDireccion = new BEDireccionCliente();

                if (!string.IsNullOrEmpty(flagCoordenadas))
                {
                    //objDireccion.Latitud = strLatitudLongitud.Split(';')[0].Trim();
                    //objDireccion.Longitud = strLatitudLongitud.Split(';')[1].Trim();
                     //INICIATIVA 992 INICIO
                    objDireccion.Latitud = strDireccion.Split(';')[0].Trim();
                    objDireccion.Longitud = strDireccion.Split(';')[1].Trim();
                    //INICIATIVA 992 FIN
                    HttpContext.Current.Session["objDireccion"] = objDireccion;

                    objResponse.Boleano = true;
                }
                else if (!string.IsNullOrEmpty(strDireccion) && !string.IsNullOrEmpty(strLatitudLongitud))
                {
			//INICIATIVA 992 INICIO
                    //objDireccion.Direccion = strDireccion.Split(',')[0].Trim();
                    //objDireccion.Distrito = strDireccion.Split(',')[1].Trim();
                    //objDireccion.Provincia = strDireccion.Split(',')[2].Trim();
                    //objDireccion.Departamento = strDireccion.Split(',')[3].Trim();

                    //objDireccion.Latitud = strLatitudLongitud.Split(';')[0].Trim();
                    //objDireccion.Longitud = strLatitudLongitud.Split(';')[1].Trim();

                    objDireccion.Latitud = strLatitudLongitud.Split(';')[0].Trim();
                    objDireccion.Longitud = strLatitudLongitud.Split(';')[1].Trim();
			 //INICIATIVA 992 INICIO
                    HttpContext.Current.Session["objDireccion"] = objDireccion;

                    objResponse.Boleano = true;
                }
                else
                {
                    _objLog.CrearArchivolog("[INICIO INICIATIVA 932 - Movilidad IFI (ValidarCoberturaIFI)][Error][Se ha enviado valores vacios]", null, null);
                }

            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(string.Format("{0} => [{1}|{2}]", "[INICIO INICIATIVA 932 - Movilidad IFI (ValidarCoberturaIFI)][Error]", ex.Message, ex.StackTrace), null, null);
            }
            _objLog.CrearArchivolog("FIN INICIATIVA 932 - Movilidad IFI (ValidarCoberturaIFI)", null, null);

            return objResponse;
        }

        #endregion
    }
}
