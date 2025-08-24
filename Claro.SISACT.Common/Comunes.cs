using System.Collections.Generic;
using System.Web.UI.WebControls;
using Claro.SISACT.Entity;
using System.Configuration;
using System.Collections;

namespace Claro.SISACT.Common
{
    public static class Comunes
    {

        public static void LlenaCombo(List<BEItemGenerico > datos, DropDownList  ddlCombo, bool blnInsertarSeleccionar = false)
        {
            if (datos == null) { return; } 
            if (ddlCombo == null) { return; }
            if (blnInsertarSeleccionar )
            {
                string seleccionar = ConfigurationManager.AppSettings["Seleccionar"];
                BEItemGenerico item = new BEItemGenerico() { Codigo = "00", Descripcion = seleccionar, Descripcion2 = seleccionar };
                datos.Insert(0, item);
                
                ddlCombo.DataSource = datos;
                ddlCombo.DataValueField = "Codigo";
                ddlCombo.DataTextField = "Descripcion";
                ddlCombo.DataBind();
            }
        }

        public static void LlenaCombo(ArrayList source,
                      System.Web.UI.WebControls.ListControl ddlCombo,
                      string campoValue,
                      string campoText,
                      bool blnInsertarTodos = false,
                      bool blnInsertarSeleccionar = false,
                      string seleccionar = "")
        {
            if (source == null) return;
            if (ddlCombo == null) return;
            BEItemGenerico item = new BEItemGenerico();

            if (blnInsertarSeleccionar)
            {
                if (seleccionar == string.Empty) seleccionar = ConfigurationManager.AppSettings["Seleccionar"];
                item = new BEItemGenerico("-1", seleccionar);
                item.Descripcion2 = seleccionar;
                source.Insert(0, item);
            }

            if (blnInsertarTodos)
            {
                item = new BEItemGenerico("-1", ConfigurationManager.AppSettings["Todos"]);
                item.Descripcion2 = ConfigurationManager.AppSettings["Todos"];
                source.Insert(0, item);
            }

            ddlCombo.DataSource = source;
            ddlCombo.DataValueField = campoValue;
            ddlCombo.DataTextField = campoText;
            ddlCombo.DataBind();
        }

        public static  string GetUrlByCodigoPortal(string id_portal)
        {

            string strRutaSite = ConfigurationManager.AppSettings["RutaSiteSisactCorporativo"];

            strRutaSite = strRutaSite.Substring(0, strRutaSite.LastIndexOf("/"));

            string strRutas = ConfigurationManager.AppSettings["contRutaSitePortal"];
            string[] arrRutas = strRutas.Split(';');

            string url = "";

            foreach (string strPortal in arrRutas)
            {
                if (id_portal == strPortal.Split(',')[0])
                {
                    url = strRutaSite + "/" + strPortal.Split(',')[1];
                }
            }
            return url;
        }

    }
}
