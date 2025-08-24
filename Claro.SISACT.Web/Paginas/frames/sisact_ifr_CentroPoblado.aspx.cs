using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using Claro.SISACT.Business;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.Web.Base;
namespace Claro.SISACT.Web.Paginas.frames
{
    public partial class sisact_ifr_CentroPoblado : Sisact_Webbase //System.Web.UI.Page
    {
        private void Page_Load(System.Object sender, System.EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (Session["Usuario"] == null)
            {
                string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                Response.Redirect(strRutaSite);
                Response.End();
                return;
            }

            if (!IsPostBack)
            {
                Inicio();
            }
        }

        private void Inicio()
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, "sisact_ifr_CentroPoblado",null,"WEB");
            objLog.CrearArchivolog("[Inicio][Inicio2]", null, null);

            int iDTH;
            int iLTE;
            string strCodProd;
          
            hidCodSerie.Value = Request.QueryString["codSerie"];
            iDTH = Convert.ToInt32(Request.QueryString["iDTH"]);
            iLTE = Convert.ToInt32(Request.QueryString["iLTE"]);
            strCodProd = Request.QueryString["CodProd"];

            if (string.IsNullOrEmpty(hidCodSerie.Value))
            {
                hidNResponseValue.Value = "ExtraerDatos";
            }
            else
            {
                objLog.CrearArchivolog("[DTH]", iDTH.ToString(), null);
                objLog.CrearArchivolog("[LTE]", iLTE.ToString(), null);
                objLog.CrearArchivolog("[ID_PROD]", strCodProd.ToString(), null);
                 ConsultarCentroPoblado(Request.QueryString["codSerie"], iDTH, iLTE, strCodProd);
            }
            objLog.CrearArchivolog("[Fin][Inicio]", null, null);
        }

        public void ConsultarCentroPoblado(string Codigo, int iDTH, int iLTE, string strCodProd)
        {
      GeneradorLog objLog = new GeneradorLog(CurrentUser, "sisact_ifr_CentroPoblado",null,"WEB");
            objLog.CrearArchivolog("[Inicio][ConsultarCentroPoblado]",null,null);

            try
            {
                string consTipoProductoHFC = ConfigurationManager.AppSettings["consTipoProducto3Play"].ToString();
                string consTipoProducto3Play_I = ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"].ToString();
 
                string datos = "";
                ArrayList lista;
                BLIntegracionDTHNegocio oIntegracioDTHNegocio = new BLIntegracionDTHNegocio();

                if (strCodProd == consTipoProductoHFC || strCodProd == consTipoProducto3Play_I) {

                    objLog.CrearArchivolog("[DTH]", iDTH.ToString(), null);
                    objLog.CrearArchivolog("[LTE]", iLTE.ToString(), null);
                     lista = oIntegracioDTHNegocio.ListarCentrosPobladosDistrito_LTE(Codigo, iDTH, iLTE);
                }
                else {
                    objLog.CrearArchivolog("[CODIGO]", Codigo.ToString(), null);
                    lista = oIntegracioDTHNegocio.ListarCentrosPobladosDistrito(Codigo);
                    }
                foreach (BECentroPoblado item in lista)
                {
                    if (string.IsNullOrEmpty(datos))
                        datos = item.IDPOBLADO + "-" + item.COBERTURA + "_" + item.IDUBIGEO + ";" + item.NOMBRE;
                    else
                        datos = datos + "|" + item.IDPOBLADO + "-" + item.COBERTURA + "_" + item.IDUBIGEO + ";" + item.NOMBRE;
                }

                hidNResponseValue.Value = "RetornarDatos";
                hidDatosRetorno.Value = datos;
            }
            catch (Exception )
            {
                hidNResponseValue.Value = "RetornarDatos";
                hidDatosRetorno.Value = "";
            }
            objLog.CrearArchivolog("[Fin][ConsultarCentroPoblado]", null, null);
        }
    }
}