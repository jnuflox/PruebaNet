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
using System.Text;
using Claro.SISACT.Web.Base; //PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_lineas_desactivas : Sisact_Webbase//PROY-140126 - IDEA 140248 
    {
        #region [Declaracion de Constantes - Config]

        int nroLineaMaxMostrar = 0;
        int nroLineaMaxMostrarPopup = 0;
        int nroLineaTotalMostrar = 0;
        int nroLineaTotalMostrarPopup = 0;
        string flgConsultaPopup = string.Empty;
        string flgConsultaEval = string.Empty;

        #endregion [Declaracion de Constantes - Config]

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
                Inicio();
            }
        }

        //PROY-24740
        private void Inicio()
        {
            DataTable dtBSCS = new DataTable();
            DataTable dtSGA = new DataTable();
           // string tipoDocumento = Request.QueryString["tipoDocumento"];
            string nroDocu = (string) HttpContext.Current.Session["docCliente"];
            string origen = Request.QueryString["origen"];

            BEClienteCuenta objCliente = null;        
            objCliente = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocu];

            string tipoDocumento = Funciones.CheckStr(objCliente.tipoDoc);
            string nroDocumento = Funciones.CheckStr(objCliente.nroDoc);

            GeneradorLog _objLog = new GeneradorLog("", nroDocumento, null, "WEB_LineasDesactivas");
            _objLog.CrearArchivolog("[Inicio][ValidacionConsultaMasiva]", null, null);

            _objLog.CrearArchivolog("HttpContext.Current.Session[docCliente]=>" + Funciones.CheckStr(HttpContext.Current.Session["docCliente"]), null, null);

            BEClienteCuenta objClienteConsulta = new BEClienteCuenta();
            objClienteConsulta = (BEClienteCuenta)HttpContext.Current.Session["objCliente" + nroDocu];
            _objLog.CrearArchivolog("objClienteConsulta.tipoDoc =>" + Funciones.CheckStr(objClienteConsulta.tipoDoc), null, null);
            _objLog.CrearArchivolog("objClienteConsulta.nroDoc =>" + Funciones.CheckStr(objClienteConsulta.nroDoc), null, null);

            flgConsultaEval = Request.QueryString["flgConsultaEval"];
            flgConsultaPopup = Request.QueryString["flgConsultaPopup"];
            StringBuilder sblCantidadMotivoBloqueo = new StringBuilder();

            DataRow dr;
            WS.WSLineaDesactiva.LineasDesactivasBSCSComplexType[] objListaLineasDesactivasBSCS;
            WS.WSLineaDesactiva.LineasDesactivasSGAComplexType[] objListaLineasDesactivasSGA;
            WS.WSLineaDesactiva.AdicionalResponseComplexType[] objAdicionalResponse;

                List<BEItemGenerico> objListaItem = (new BLGeneral()).ListarParametroGeneral("1");

            var lineaMaxMostrar = objListaItem.Where(w => w.Codigo == ConfigurationManager.AppSettings["consConsultaLineasDesactivasMaxMos"].ToString()).ToList();
            if (lineaMaxMostrar != null && lineaMaxMostrar.Count>0)
                    {
                nroLineaMaxMostrar = Funciones.CheckInt(lineaMaxMostrar[lineaMaxMostrar.Count-1].Valor);
                    }

            var lineaMaxMostrarPopup = objListaItem.Where(w => w.Codigo == ConfigurationManager.AppSettings["consConsultaLineasDesactivasMaxMosPopup"].ToString()).ToList();
            if (lineaMaxMostrarPopup != null && lineaMaxMostrarPopup.Count>0)
                    {
                nroLineaMaxMostrarPopup = Funciones.CheckInt(lineaMaxMostrarPopup[lineaMaxMostrarPopup.Count-1].Valor);
                }

                if (flgConsultaPopup != "S")
                {
                    List<BETipoDocumento> objListaDocumento = (new BLGeneral()).ListarTipoDocumento();
                tipoDocumento = objListaDocumento.Where(w => w.ID_SISACT == tipoDocumento).FirstOrDefault().ID_BSCS.ToString();

                    estructuraTable(ref dtBSCS, ref dtSGA);

                    WS.BWLineaDesactiva objConsulta = new WS.BWLineaDesactiva();
                    BEItemMensaje objMensaje = objConsulta.detalleLineaDesactiva(nroDocumento, tipoDocumento, nroDocumento, out objListaLineasDesactivasBSCS, out objListaLineasDesactivasSGA, out objAdicionalResponse);

                    if (!objMensaje.exito)
                    {
                    StringBuilder sblscript = new StringBuilder();
                    sblscript.Append("<script>");
                    sblscript.Append(string.Format("alert('{0}');", ConfigurationManager.AppSettings["consMsjClienteLineaDesactiva"].ToString()));
                    sblscript.Append("window.parent.trLineasDesactivas.style.display = 'none';");
                    
                        
                    //FVERGARA  .:Inicio:.  
                    //Implementacion del Activador para Consultas 3G    Date: 18/08/2017
                    string strSwicht_3G = ConfigurationManager.AppSettings["Swicht_3G"];

                    if (strSwicht_3G == "0")
                    {
                        sblscript.Append("window.parent.trDetalleCliente.style.display = '';");
                        sblscript.Append("window.parent.trConsultarDC.style.display = '';");
                    }

                    //.:Fin:.



                    /*ECM s8
                    sblscript.Append("window.parent.trDetalleCliente.style.display = '';");
                    sblscript.Append("window.parent.trConsultarDC.style.display = '';");
                    FIN ECM S8*/

                    sblscript.Append(";</script>");
                    ClientScript.RegisterStartupScript(Page.GetType(), "script", sblscript.ToString());
                        return;
                    }

                    if (objListaLineasDesactivasBSCS != null)
                    {
                        foreach (WS.WSLineaDesactiva.LineasDesactivasBSCSComplexType item in objListaLineasDesactivasBSCS)
                        {
                            dr = dtBSCS.NewRow();
                            dr["numeroLinea"] = item.numeroLinea;
                            dr["tipoProducto"] = item.tipoProducto;
                            dr["estadoLinea"] = item.estadoLinea;
                            dr["codigoMotivoDesactivacion"] = item.codigoMotivoDesactivacion;
                            dr["motivoDesactivacion"] = item.motivoDesactivacion;
                            dr["fechaActivacion"] = item.fechaActivacion;
                            dr["fechaDesactivacion"] = item.fechaDesactivacion;
                            dr["vigenciaApadece"] = item.vigenciaApadece;
                            dr["recomendacion"] = item.recomendacion;
                            dtBSCS.Rows.Add(dr);
                        }
                        Session["dtLineasDesactivaBSCS" + nroDocumento] = dtBSCS;
                    }

                    if (objListaLineasDesactivasSGA != null)
                    {
                        foreach (WS.WSLineaDesactiva.LineasDesactivasSGAComplexType item in objListaLineasDesactivasSGA)
                        {
                            dr = dtSGA.NewRow();
                            dr["numeroLinea"] = item.numeroLinea;
                            dr["tipoServicio"] = item.tipoServicio;
                            dr["estadoServicioLinea"] = item.estadoServicioLinea;
                            dr["codigoMotivoDesactivacion"] = item.codigoMotivoDesactivacion;
                            dr["motivoDesactivacion"] = item.motivoDesactivacion;
                            dr["fechaActivacion"] = item.fechaActivacion;
                            dr["fechaDesactivacion"] = item.fechaDesactivacion;
                            dr["agrupacionPaquete"] = item.agrupacionPaquete;
                            dr["recomendacion"] = item.recomendacion;
                            dtSGA.Rows.Add(dr);
                        }
                        Session["dtLineasDesactivaSGA" + nroDocumento] = dtSGA;
                    }
                    gvDetalleBSCS.DataSource = dtBSCS;
                    gvDetalleSGA.DataSource = dtSGA;
                }
                else
                {
                    dtBSCS = (DataTable)Session["dtLineasDesactivaBSCS" + nroDocumento];
                    dtSGA = (DataTable)Session["dtLineasDesactivaSGA" + nroDocumento];
                    gvDetalleBSCS.DataSource = dtBSCS;
                    gvDetalleSGA.DataSource = dtSGA;
                }

                int nroRegistroBSCS = 0;
                int nroRegistroSGA = 0;
                BEItemGenerico itemBSCS, itemSGA;
                DataTable dtMotivo = new BLGeneral().ListarMotivoDesactivaLinea();

                foreach (DataRow drMotivo in dtMotivo.Rows)
                {
                    itemBSCS = new BEItemGenerico();
                    itemSGA = new BEItemGenerico();
                    if (dtBSCS != null)
                    {
                        foreach (DataRow dr1 in dtBSCS.Rows)
                        {
                            if (Funciones.CheckStr(drMotivo["MOLDN_CODIGO"]) == Funciones.CheckStr(dr1["codigoMotivoDesactivacion"]))
                            {
                                itemBSCS.Codigo = Funciones.CheckStr(drMotivo["MOLDN_CODIGO"]);
                                itemBSCS.Descripcion = Funciones.CheckStr(dr1["motivoDesactivacion"]);
                                itemBSCS.Cantidad = itemBSCS.Cantidad + 1;
                                nroRegistroBSCS += 1;
                            }
                        }
                        if (itemBSCS.Cantidad > 0)
                        {
                            lblCantLineaBSCS.Text += string.Format("Cant. {0}: {1} ", itemBSCS.Descripcion, itemBSCS.Cantidad);
                        }
                    }

                    if (dtSGA != null)
                    {
                        foreach (DataRow dr1 in dtSGA.Rows)
                        {
                            if (Funciones.CheckStr(drMotivo["MOLDN_CODIGO"]) == Funciones.CheckStr(dr1["codigoMotivoDesactivacion"]))
                            {
                                itemSGA.Codigo = Funciones.CheckStr(drMotivo["MOLDN_CODIGO"]);
                                itemSGA.Descripcion = Funciones.CheckStr(dr1["motivoDesactivacion"]);
                                itemSGA.Cantidad = itemSGA.Cantidad + 1;
                                nroRegistroSGA += 1;
                            }
                        }
                        if (itemSGA.Cantidad > 0)
                        {
                            lblCantLineaSGA.Text += string.Format("Cant. {0}: {1} ", itemSGA.Descripcion, itemSGA.Cantidad);
                        }
                    }
                sblCantidadMotivoBloqueo.Append("|");
                sblCantidadMotivoBloqueo.Append(Funciones.CheckStr(drMotivo["MOLDN_CODIGO"]));
                sblCantidadMotivoBloqueo.Append(";");
                sblCantidadMotivoBloqueo.Append(itemSGA.Cantidad + itemBSCS.Cantidad);
                }

                if (nroRegistroBSCS == 0)
                {
                    trDetalleBSCS.Visible = false;
                }
                if (nroRegistroSGA == 0)
                {
                    trDetalleSGA.Visible = false;
                }

                if (origen == "P")
                {
                    trDetalleBSCS.Visible = true;
                    trDetalleSGA.Visible = true;
                }

                gvDetalleBSCS.DataBind();
                gvDetalleSGA.DataBind();

                if ((flgConsultaPopup == "S") || ((nroRegistroBSCS + nroRegistroSGA) <= nroLineaMaxMostrar))
                {
                    trDetalleMasPlanes.Visible = false;
                }

                if (flgConsultaEval == "S")
                {
                    trBotonCerrar.Visible = false;
                    lblCantLineaBSCS.Visible = false;
                    lblCantLineaSGA.Visible = false;

                StringBuilder sblscript = new StringBuilder();
                    if ((nroRegistroBSCS + nroRegistroSGA) == 0)
                    {
                    sblscript.Append("<script>");
                    sblscript.Append("window.parent.trLineasDesactivas.style.display = 'none';");
                    sblscript.Append("window.parent.trDetalleCliente.style.display = '';");
                    sblscript.Append("window.parent.trConsultarDC.style.display = '';");
                    sblscript.Append(";</script>");
                    ClientScript.RegisterStartupScript(Page.GetType(), "script", sblscript.ToString());
                    }
                    else
                    {
                        if (flgConsultaEval == "S")
                        {
                        sblscript.Append("<script>");
                        sblscript.Append("window.parent.trDetalleCliente.style.display = '';");
                        sblscript.Append("window.parent.trConsultarDC.style.display = '';");
                        sblscript.Append(string.Format("window.parent.setValue('hidCantidadMotivoBloqueo', '{0}');", sblCantidadMotivoBloqueo.ToString()));
                        sblscript.Append(";</script>");
                        ClientScript.RegisterStartupScript(Page.GetType(), "script", sblscript.ToString());
                            // [INC000002442213] VALIDACION HIDDEN INI
                        HttpContext.Current.Session["sessionVal_hidCantidadMotivoBloqueo"] = sblCantidadMotivoBloqueo.ToString();
                        _objLog.CrearArchivolog("[INC000002442213][LOG_VALIDACIONES_HIDDEN][sessionVal_hidCantidadMotivoBloqueo]" + sblCantidadMotivoBloqueo.ToString(), null, null);
                            // [INC000002442213] VALIDACION HIDDEN FIN
                        }
                    }
                }
            }

        protected void gvDetalleBSCS_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (flgConsultaPopup != "S")
                {
                    nroLineaTotalMostrar++;
                    if (e.Row.DataItemIndex >= nroLineaMaxMostrar)
                    {
                        e.Row.Visible = false;
                    }
                }
                else
                {
                    nroLineaTotalMostrarPopup++;
                    if (e.Row.DataItemIndex >= nroLineaMaxMostrarPopup)
                    {
                        e.Row.Visible = false;
                    }
                }
            }
        }

        protected void gvDetalleSGA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (flgConsultaPopup != "S")
                {
                    if (nroLineaTotalMostrar >= nroLineaMaxMostrar)
                        e.Row.Visible = false;
                    else
                        nroLineaTotalMostrar++;
                }
                else
                {
                    if (nroLineaTotalMostrarPopup >= nroLineaMaxMostrarPopup)
                        e.Row.Visible = false;
                    else
                        nroLineaTotalMostrarPopup++;
                }
            }
        }

        private void estructuraTable(ref DataTable dtBSCS, ref DataTable dtSGA)
        {
            dtBSCS.Columns.Add(new DataColumn("numeroLinea", System.Type.GetType("System.String")));
            dtBSCS.Columns.Add(new DataColumn("tipoProducto", System.Type.GetType("System.String")));
            dtBSCS.Columns.Add(new DataColumn("estadoLinea", System.Type.GetType("System.String")));
            dtBSCS.Columns.Add(new DataColumn("codigoMotivoDesactivacion", System.Type.GetType("System.String")));
            dtBSCS.Columns.Add(new DataColumn("motivoDesactivacion", System.Type.GetType("System.String")));
            dtBSCS.Columns.Add(new DataColumn("fechaActivacion", System.Type.GetType("System.String")));
            dtBSCS.Columns.Add(new DataColumn("fechaDesactivacion", System.Type.GetType("System.String")));
            dtBSCS.Columns.Add(new DataColumn("vigenciaApadece", System.Type.GetType("System.String")));
            dtBSCS.Columns.Add(new DataColumn("recomendacion", System.Type.GetType("System.String")));

            dtSGA.Columns.Add(new DataColumn("numeroLinea", System.Type.GetType("System.String")));
            dtSGA.Columns.Add(new DataColumn("tipoServicio", System.Type.GetType("System.String")));
            dtSGA.Columns.Add(new DataColumn("estadoServicioLinea", System.Type.GetType("System.String")));
            dtSGA.Columns.Add(new DataColumn("codigoMotivoDesactivacion", System.Type.GetType("System.String")));
            dtSGA.Columns.Add(new DataColumn("motivoDesactivacion", System.Type.GetType("System.String")));
            dtSGA.Columns.Add(new DataColumn("fechaActivacion", System.Type.GetType("System.String")));
            dtSGA.Columns.Add(new DataColumn("fechaDesactivacion", System.Type.GetType("System.String")));
            dtSGA.Columns.Add(new DataColumn("agrupacionPaquete", System.Type.GetType("System.String")));
            dtSGA.Columns.Add(new DataColumn("recomendacion", System.Type.GetType("System.String")));
        }
    }
}