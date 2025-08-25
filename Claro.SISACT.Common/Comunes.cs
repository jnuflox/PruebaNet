using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Claro.SISACT.Entity;
using System.Configuration;
using System.Collections;

namespace Claro.SISACT.Common
{
    public static class Comunes
    {

        public static List<SelectListItem> LlenaCombo(List<BEItemGenerico > datos, bool blnInsertarSeleccionar = false)
        {
            if (datos == null) { return null; }

            List<SelectListItem> items = new List<SelectListItem>();

            if (blnInsertarSeleccionar)
            {
                string seleccionar = ConfigurationManager.AppSettings["Seleccionar"];
                BEItemGenerico item = new BEItemGenerico() { Codigo = "00", Descripcion = seleccionar, Descripcion2 = seleccionar };
                datos.Insert(0, item);
            }

            foreach (var item in datos)
            {
                items.Add(new SelectListItem { Value = item.Codigo, Text = item.Descripcion });
            }

            return items;
        }

        public static List<SelectListItem> LlenaCombo(ArrayList source,
                      string campoValue,
                      string campoText,
                      bool blnInsertarTodos = false,
                      bool blnInsertarSeleccionar = false,
                      string seleccionar = "")
        {
            if (source == null) return null;

            List<SelectListItem> items = new List<SelectListItem>();
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

            foreach (BEItemGenerico obj in source)
            {
                items.Add(new SelectListItem
                {
                    Value = obj.GetType().GetProperty(campoValue)?.GetValue(obj)?.ToString(),
                    Text = obj.GetType().GetProperty(campoText)?.GetValue(obj)?.ToString()
                });
            }

            return items;
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