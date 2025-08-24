using System;
using System.Configuration;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.Web.Base;

namespace Claro.SISACT.Web
{
    public partial class Inicio1 : Sisact_Webbase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Usuario"] ==  null)
            {
                string strRutaSite = ConfigurationManager.AppSettings["RutaSite"] + "/" + ConfigurationManager.AppSettings["constPageHome"];
                    Response.Redirect(strRutaSite);
                    Response.End();
                    return;
            }

        }
    }
}