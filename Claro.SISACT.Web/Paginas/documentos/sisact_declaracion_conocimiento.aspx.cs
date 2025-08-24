using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Entity; 
using Claro.SISACT.Business;
using System.Text;
using System.Collections;
using Claro.SISACT.Common;
using System.Configuration;
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.documentos
{
    public partial class sisact_declaracion_conocimiento : Sisact_Webbase //PROY-140126 - IDEA 140248  
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                Response.Redirect(strRutaSite);
                Response.End();
                return;
            }
            if (!Page.IsPostBack)
                  CargarDeclConocimiento();

        }

        public void CargarDeclConocimiento()
        {
            string CurrentUser = string.Empty;
            GeneradorLog objLog = new GeneradorLog(CurrentUser, null, null, "WEB");

            try
            { 
            
            CurrentUser = Session["Usuario"].ToString();
            objLog.CrearArchivolog("[Inicio][Carga - DeclaracionConocimiento]", null, null);

            BLSolicitudNegocios oSolicitudNegocios = new BLSolicitudNegocios();

            hidCadenaItems.Value = "";
            String SegmentoVenta = ConfigurationManager.AppSettings["consSegmentoPortaDeclConocimiento"]; 
            StringBuilder sbCadenaItems = new StringBuilder();
            String CadenaItems = "";
            
                objLog.CrearArchivolog("Segmento: " + SegmentoVenta, null, null);
               
                ArrayList arrItems = oSolicitudNegocios.ConsultaDeclaracionConocimiento(SegmentoVenta);
                objLog.CrearArchivolog("Items: " + arrItems.Count.ToString(), null, null);
                if (arrItems.Count > 0)
                {

                    foreach (BEItemGenerico Item in arrItems)
                    {
                        sbCadenaItems.AppendFormat("{0}&{1}&{2}&{3}|", Funciones.CheckStr(Item.Codigo2), Item.Descripcion, Item.Codigo3, Item.Codigo);
                        objLog.CrearArchivolog("Items: " + Funciones.CheckStr(Item.Codigo2)+"&"+ Item.Codigo3+"&"+ Item.Codigo, null, null);
                
                    }
                    CadenaItems = sbCadenaItems.ToString();
                    CadenaItems = CadenaItems.Substring(0, sbCadenaItems.Length - 1);
                    hidCadenaItems.Value = CadenaItems;
                }
                else
                {
                    hidCadenaItems.Value = "";
                }
            }
            catch(Exception ex)
            {
                objLog.CrearArchivolog("Error - "+ ex.Message, null, null);
                hidCadenaItems.Value = "";
            }

            objLog.CrearArchivolog("[Fin][Carga - DeclaracionConocimiento]", null, null);
        }
    }
}
