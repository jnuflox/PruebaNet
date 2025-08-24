using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Claro.SISACT.Common;
using Claro.SISACT.Business;
using Claro.SISACT.Entity;
using Claro.SISACT.Web.Base;//PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_con_agenda_3play : Sisact_Webbase //PROY-140126 - IDEA 140248  
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
            try
            {
                Int64 nroSec = Funciones.CheckInt64(Request.QueryString["nroSec"]);
                BEAcuerdo oAcuerdo = new BEAcuerdo();
                List<BEAcuerdoDetalle> listAcuerdoDetalle = new List<BEAcuerdoDetalle>();
                List<BEAcuerdoServicio> listAcuerdoServicio = new List<BEAcuerdoServicio>();
                BLSolicitud.ObtenerAcuerdosBySec(nroSec, ref oAcuerdo, ref listAcuerdoDetalle, ref listAcuerdoServicio);

                txtNroSec.Text = nroSec.ToString();
                txtNroDoc.Text = oAcuerdo.NroDocCliente;
                txtNombre.Text = (oAcuerdo.RazonSocial != "") ? oAcuerdo.RazonSocial : oAcuerdo.NombreCliente + " " + oAcuerdo.ApellidoPatCliente + " " + oAcuerdo.ApellidoMatCliente;
                txtNroContrato.Text = oAcuerdo.IdContrato.ToString();

                foreach (BEAcuerdoDetalle oDetalle in listAcuerdoDetalle) {
                    if (oDetalle.Prdc_codigo == ConfigurationManager.AppSettings["consTipoProducto3Play"]) {
                        nroSec = oDetalle.Solin_codigo;
                        break;
                    }
                }
                List<BEEstado> listEstodosSot = (new BLSolicitud()).ObtenerHistoricoEstadosSOT(nroSec);
                if (listEstodosSot.Count > 0)
                {
                    txtNroSot.Text = ((BEEstado)listEstodosSot[0]).NroSEC.ToString();
                    txtEstadoSGA.Text = ((BEEstado)listEstodosSot[0]).ESTAV_DESCRIPCION;
                    BEAgendamiento oAgendamiento = new BLSolicitud().ObtenerAgendamientoSga(((BEEstado)listEstodosSot[0]).NroSEC);
                    txtFechaInst.Text = oAgendamiento.Fecha;
                    txtHora.Text = oAgendamiento.Hora;
                    txtContratista.Text = oAgendamiento.Contratista;
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Ocurrió un error en la consulta.'); window.close();</script>");
            }
        }
    }
}