using System;
using System.Configuration;
using Claro.SISACT.Web.Base;

namespace Claro.SISACT.Web
{
    public partial class sisact_logout : Sisact_Webbase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            var rutaInicio = ConfigurationManager.AppSettings["RutaSite"] + "/" + ConfigurationManager.AppSettings["const_pagina_inicio"];
            Script("window.location.href = '" + rutaInicio + "';");
        }
    }
}