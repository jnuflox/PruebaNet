using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using Claro.SISACT.Web.Base;//PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_detalle_oferta : Sisact_Webbase //PROY-140126 - IDEA 140248 
    {
        GeneradorLog objLog = new GeneradorLog("    sisact_pop_detalle_oferta   ", null, null, "WEB");

        int intNroItem = 0;
        float sngCargoFijoMensual = 0;
        int intNroItemE = 0;
        string strCodTipoProducto3Play = ConfigurationManager.AppSettings["consTipoProducto3Play"];
        string constTipoProducto3PlayInalam = ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"];
        string constTipoProductoInterInalam = ConfigurationManager.AppSettings["constTipoProductoInterInalam"]; //PROY-31812 - IDEA-43340
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

            if (!Page.IsPostBack)
            {
                string strDetalleHFC = Funciones.CheckStr(Request.QueryString["strDetalleHFC"]);
                string strEquipoHFC = Funciones.CheckStr(Request.QueryString["strEquipoHFC"]);
                string strIdProd = Funciones.CheckStr(Request.QueryString["strIdProd"]) == null ? null : Funciones.CheckStr(Request.QueryString["strTipoProducto"]); //PROY-31812 - IDEA-43340
                CargarGrilla(strDetalleHFC, strEquipoHFC, Funciones.CheckInt64(Request.QueryString["nroSEC"]), strIdProd);

                 objLog.CrearArchivolog("    Page_Load/Req/DetalleHFC   ", strDetalleHFC.ToString(), null);
                 objLog.CrearArchivolog("    Page_Load/Req/EquipoHFC    ", strEquipoHFC.ToString(), null);
                 objLog.CrearArchivolog("    Page_Load/Req/IdProd    ", strIdProd.ToString(), null);
            
            }
        }

        //PROY-24740
          public void CargarGrilla(string strDetalleHFC, string strEquipoHFC, Int64 nroSEC, string strIdProd)
        {
              objLog.CrearArchivolog("    CargarGrilla    ", null, null);

            if (strIdProd == strCodTipoProducto3Play)
            {
                lblTitulo.Text = "ALQUILER DE EQUIPOS 3 PLAY";
            }
            if (strIdProd == constTipoProducto3PlayInalam)
            {
                lblTitulo.Text = "ALQUILER DE EQUIPOS 3 PLAY INALAMBRICO";
            }
            //PROY-31812 - IDEA-43340 - INICIO
            if (strIdProd == constTipoProductoInterInalam)
            {
                lblTitulo.Text = "INTERNET INALÁMBRICO";

                lblPlan.Text = "Plan";
            }
            //PROY-31812 - IDEA-43340 - FIN
            List<BEPlanDetalleHFC> oLista = new List<BEPlanDetalleHFC>();
            List<BEPlanDetalleHFC> oLista1 = new List<BEPlanDetalleHFC>();

            if (strDetalleHFC.Length > 0)
            {
                txtCampana.Text = Request.QueryString["strCampana"].Replace("*", "+");
                txtPlan.Text = Request.QueryString["strPlan"].Replace("*", "+");

                objLog.CrearArchivolog("    CargarGrilla/DetalleSplit    ", null, null);

                oLista.AddRange(strDetalleHFC.Split('|').Select(s => s.Split(';'))
                    .Select(c => new BEPlanDetalleHFC()
                {
                                       Grupo = Funciones.CheckInt(c[0]),
                                       Tipo = Funciones.CheckStr(c[1]) == "1" ? "Serv.Principal" : Funciones.CheckStr(c[1]) == "0" ? "Adicional" : null,
                                       Producto = c[2],
                                       Servicio = Funciones.CheckStr(c[4]).Replace("(*)", ""),
                                       Promocion = c[6],
                                       Precio = Funciones.CheckDbl(c[5]),
                                       FlagPrincipal = c[7],
                                       GrupoDescripcion = c[8]
                                   }).ToList());


                if (strEquipoHFC.Length > 0)
                {
                    objLog.CrearArchivolog("    CargarGrilla/EquipoHFCSplit    ", null, null);

                    oLista1.AddRange(strEquipoHFC.Split('|').Select(s => s.Split(';')).Where(w => w.Length > 1)
                        .Select(a => new BEPlanDetalleHFC()
                    {
                            Agrupa = Funciones.CheckStr(a[1]),
                            Servicio = Funciones.CheckStr(a[2]).Replace("(*)", ""),
                            Precio = Funciones.CheckDbl(a[3])
                        }).ToList());
                }
            }
            else
            {
                objLog.CrearArchivolog("    CargarGrilla/DetalleOferta/nroSEC    ", nroSEC.ToString(), null);
                objLog.CrearArchivolog("    CargarGrilla/DetalleOferta/strIdProd    ", strIdProd.ToString(), null);

                oLista = (new BLGeneral_II()).DetalleOferta3Play(nroSEC, strIdProd);

                if (oLista != null && oLista.Count > 0)
                {
                    txtCampana.Text = oLista[oLista.Count-1].Campana;
                    txtPlan.Text = oLista[oLista.Count-1].Plan;
                }

                objLog.CrearArchivolog("    CargarGrilla/EquiposOferta/nroSEC    ", nroSEC.ToString(), null);
                objLog.CrearArchivolog("    CargarGrilla/EquiposOferta/nroSEC    ", strIdProd.ToString(), null);
                oLista1 = (new BLGeneral_II()).EquiposOferta3Play(nroSEC, strIdProd);

            }

            dgCabLista.DataSource = "";
            dgCabLista.DataBind();
            this.dgLista.DataSource = oLista;
            this.dgLista.DataBind();

            dgCabListaEquipo.DataSource = "";
            dgCabListaEquipo.DataBind();
            this.dgListaEquipo.DataSource = oLista1;
            this.dgListaEquipo.DataBind();

            string cadItems = oLista.Select(s => s.FlagPrincipal).Aggregate((a, b) => string.Format("{0};{1}", a, b));

            objLog.CrearArchivolog("    CargarGrilla/oDetalleVentaHFC    ", null, null);
           
            hidString.Value = cadItems;
            ClientScript.RegisterStartupScript(Page.GetType(), "script", "<script>pintarRows();</script>");

            objLog.CrearArchivolog("    CargarGrilla/SALIDA    ", null, null);
        }

        protected void dgLista_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem | e.Item.ItemType == ListItemType.Item)
            {
                intNroItem += 1;
                e.Item.Cells[0].Text = intNroItem.ToString();
                sngCargoFijoMensual += Funciones.CheckFloat(e.Item.Cells[3].Text);
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                lblCargoFijoMensual.Text = sngCargoFijoMensual.ToString();
            }
        }

        protected void dgListaEquipo_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem | e.Item.ItemType == ListItemType.Item)
            {
                intNroItemE += 1;
                e.Item.Cells[0].Text = intNroItemE.ToString();
                sngCargoFijoMensual += Funciones.CheckFloat(e.Item.Cells[3].Text);
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                lblCargoFijoMensual.Text = sngCargoFijoMensual.ToString();
            }
        }
    }
}