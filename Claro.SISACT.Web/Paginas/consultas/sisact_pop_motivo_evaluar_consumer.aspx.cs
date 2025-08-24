using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;
using Claro.SISACT.Web.Base;
using Claro.SISACT.Web.Comun;

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_motivo_evaluar_consumer : Sisact_Webbase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (Session["Usuario"] == null)
            {
                string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                Response.Redirect(strRutaSite);
                Response.End();
                return;
            }

            Inicio();
        }

        private void Inicio()
        {
            txtNroSEC.Text = Request.QueryString["nroSEC"].ToString();
            txtNroDocumento.Text = Request.QueryString["nroDocumento"].ToString();
            txtNombres.Text = Request.QueryString["nombres"].ToString();
            hidTiempoInicio.Value = DateTime.Now.ToString();
            hidUsuarioRed.Value = CurrentUser;

            // Asignar Usuario
            (new BLSolicitud()).AsignarUsuarioSEC(Funciones.CheckInt64(txtNroSEC.Text), CurrentUser, txtNroDocumento.Text, "E");
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod liberarSEC(string nroSEC)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            try
            {
                objResponse.TipoRespuesta = "B";
                objResponse.Boleano = (new BLSolicitud()).LiberarSEC(Funciones.CheckInt64(nroSEC));
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorEvaluadorPersona"].ToString();
            }
            return objResponse;
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod rechazarSEC(string nroSEC, string usuario, string txtComentario, string tiempoInicio)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            BLSolicitud objSolicitud = new BLSolicitud();
            string strEstado = ConfigurationManager.AppSettings["constcodEstadoRECHAZADOEVALUACION"].ToString();
            try
            {
                objResponse.TipoRespuesta = "B";

                txtComentario = Funciones.CheckStr(txtComentario).ToUpper();
                objSolicitud.RechazarPoolSEC(Funciones.CheckInt64(nroSEC), usuario, string.Empty, txtComentario, strEstado);

                objResponse.Boleano = true;

                registrarTiempoRechazar(Funciones.CheckInt64(nroSEC), usuario, tiempoInicio);
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
            }
            finally
            {
                WebComunes.Auditoria(ConfigurationManager.AppSettings["CONS_COD_SACT_EVALS"].ToString(), "Aceptacion o Rechazo de la SEC|9206|POOL DE EVALUACION|");
            }
            return objResponse;
        }


        public static void registrarTiempoRechazar(Int64 nroSEC, string usuario, string strFecHrIni)
        {
            bool blnOK;
            //string strFecHrIni = hidTiempoInicio.Value;
            string strFecHrFin = DateTime.Now.ToString();
            string strCodPuntoVenta = string.Empty;
            string strCodCanal = string.Empty;
            string strCodVendedor = string.Empty;

            BLMaestro obj = new BLMaestro();
            DataTable dt = obj.ObtenerDatosRegistroTiempo(nroSEC);
            if (dt.Rows.Count > 0)
            {
                strCodPuntoVenta = dt.Rows[0]["ovenc_codigo"].ToString();
                strCodCanal = dt.Rows[0]["canac_codigo"].ToString();
                strCodVendedor = dt.Rows[0]["solin_usu_ven"].ToString();
            }

            blnOK = obj.RegistroTiempoPoolEval(nroSEC, strCodPuntoVenta, strCodCanal, strCodVendedor, usuario, strFecHrIni, strFecHrFin, "1");
        }
    }
}