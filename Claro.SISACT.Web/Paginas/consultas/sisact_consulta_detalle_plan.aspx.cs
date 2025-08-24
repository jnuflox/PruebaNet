using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.Web.Comun;
using Claro.SISACT.Web.Base;//PROY-140126 - IDEA 140248  
using Claro.SISACT.Entity.VentasCuotas.ObtenerDatosPedidoAccCuotas.Response; //PROY-140743
using Claro.SISACT.WS; //PROY-140743

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_consulta_detalle_plan : Sisact_Webbase//PROY-140126 - IDEA 140248  
    {
        GeneradorLog objLog = new GeneradorLog("[sisact_consulta_detalle_plan]", null, null, "WEB");
        #region [Declaracion de Constantes - Config]

        int intNro = 0;
        private string tipoOperacion = string.Empty;
        string constFlgPortabilidad = ConfigurationManager.AppSettings["FlagPortabilidad"].ToString();
        string constTipoProductoMovil = ConfigurationManager.AppSettings["constTipoProductoMovil"].ToString();
        string constTipoProductoDTH = ConfigurationManager.AppSettings["constTipoProductoDTH"].ToString();
        string constTipoProductoHFC = ConfigurationManager.AppSettings["constTipoProductoHFC"].ToString();
        string constTipoProductoFijo = ConfigurationManager.AppSettings["constTipoProductoFijo"].ToString();
        string constTipoOpeMigracion = ConfigurationManager.AppSettings["constTipoOperacionMIG"].ToString();
        string validaTipoOpe = string.Empty;//PROY-140743

        string constTipoProducto3PlayInalam = ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"];
      
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

            Inicio();
        }

        private void Inicio()
        {
            string idSol = Request.QueryString["idSol"];
            string idFila = Request.QueryString["idFila"];
            string idProducto = Request.QueryString["idProducto"];
            string nroSECPadre = Request.QueryString["nroSEC"];
            string nroSEC = Request.QueryString["idSEC"];
            string tipoOperacion = Funciones.CheckStr(Request.QueryString["tipoOperacion"]);//PROY-140743
            validaTipoOpe = Funciones.CheckStr(Request.QueryString["tipoOperacion"]);//PROY-140743
            //string strIdProd = Request.QueryString["strIdProd"];
            string idAgrupaPaquete = string.Empty;
            //PROY-140743 - AE - INICIO
            if (tipoOperacion == "25")
            {
                string nroDocumento = Request.QueryString["nroDoc"];
                List<BEObtenerDatosPedidoAccCuotas> DatosPedidoAccCuotas = new List<BEObtenerDatosPedidoAccCuotas>();
                objLog.CrearArchivolog("[INICIO][Linea a Facturar]", null, null);
                DatosPedidoAccCuotas = BWReglasCreditica.ObtenerDatosPedidoAccCuotas("", "", "", nroSEC, Session["Usuario"].ToString(), CurrentUsers);
                objLog.CrearArchivolog("[SALIDA][DatosPedidoAccCuotas]", Funciones.CheckStr(DatosPedidoAccCuotas.Count), null);
                if (DatosPedidoAccCuotas.Count > 0)
                {
                    objLog.CrearArchivolog("[SALIDA][DatosPedidoAccCuotas][tipoProdFacturar]", Funciones.CheckStr(((BEObtenerDatosPedidoAccCuotas)DatosPedidoAccCuotas[0]).tipoProdFacturar), null);
                    if (!string.IsNullOrEmpty(((BEObtenerDatosPedidoAccCuotas)DatosPedidoAccCuotas[0]).tipoProdFacturar))
                    {
                        hidtxtProducto.Value = Funciones.CheckStr(((BEObtenerDatosPedidoAccCuotas)DatosPedidoAccCuotas[0]).tipoProdFacturar);
                    }

                    objLog.CrearArchivolog("[SALIDA][DatosPedidoAccCuotas][lineaFacturar]", Funciones.CheckStr(((BEObtenerDatosPedidoAccCuotas)DatosPedidoAccCuotas[0]).lineaFacturar), null);
                    if (Funciones.CheckStr(((BEObtenerDatosPedidoAccCuotas)DatosPedidoAccCuotas[0]).tipoProdFacturar).Equals("FIJA"))
                    {
                        hidtxtLinea.Value = string.Format("{0}({1})", Funciones.CheckStr(((BEObtenerDatosPedidoAccCuotas)DatosPedidoAccCuotas[0]).coID), Funciones.CheckStr(((BEObtenerDatosPedidoAccCuotas)DatosPedidoAccCuotas[0]).descPlanFijo));       
                    }
                    else
                    {
                        hidtxtLinea.Value = Funciones.CheckStr(((BEObtenerDatosPedidoAccCuotas)DatosPedidoAccCuotas[0]).lineaFacturar); 
                    }
                }
                else
                {
                    hidtxtProducto.Value = "";
                    hidtxtLinea.Value = "";
                }
                objLog.CrearArchivolog("[FIN][Linea a Facturar]", null, null);
            }
            else
            {
                hidtxtProducto.Value = "";
                hidtxtLinea.Value = "";
            }                           
            //PROY-140743 - AE - FIN

            objLog.CrearArchivolog("[INICIO][sisact_consulta_detalle_plan]", null, null);
            objLog.CrearArchivolog("[Entrada][idSol]", idSol.ToString(), null);
            objLog.CrearArchivolog("[Entrada][idFila]", idFila.ToString(), null);
            objLog.CrearArchivolog("[Entrada][Producto]", idProducto.ToString(), null);
            objLog.CrearArchivolog("[Entrada][SECPadre]", nroSECPadre.ToString(), null);
            objLog.CrearArchivolog("[Entrada][SEC]", nroSEC.ToString(), null);
            objLog.CrearArchivolog("[Entrada][tipoOperacion]", tipoOperacion.ToString(), null);
            //objLog.CrearArchivolog("[Entrada][IdProd]", strIdProd.ToString(), null);
            objLog.CrearArchivolog("[Entrada][AgrupaPaquete]", idAgrupaPaquete.ToString(), null);
            // Consulta Datos Planes
            objLog.CrearArchivolog("[Entrada][ObtenerDetallePlanes][nroSECPadre]", nroSECPadre.ToString(), null);
            objLog.CrearArchivolog("[Entrada][ObtenerDetallePlanes][nroSEC]", nroSEC.ToString(), null);
            //objLog.CrearArchivolog("[Entrada][ObtenerDetallePlanes][strIdProd]", strIdProd.ToString(), null);

            DataTable dtDetalle = (new BLEvaluacion()).ObtenerDetallePlanes(Funciones.CheckInt64(nroSECPadre), Funciones.CheckInt64(nroSEC));

            DataTable dt = dtDetalle.Clone();
            DataRow[] drSelect = dtDetalle.Select(String.Format("SOLIN_CODIGO = '{0}' AND SOPLN_CODIGO = '{1}' AND ID_PRODUCTO = '{2}'", nroSEC, idSol, idProducto));

            foreach (DataRow dr in drSelect)
            {
                idAgrupaPaquete = dr["AGRUPA"].ToString();
                dt.ImportRow(dr);
            }
            objLog.CrearArchivolog("[Salida][ObtenerDetallePlanes][idAgrupaPaquete]", idAgrupaPaquete.ToString(), null);

            if (!string.IsNullOrEmpty(idAgrupaPaquete))
            {
                dt.Clear();

                objLog.CrearArchivolog("[Entrada][ObtenerIdFilaPaquete][idFila]", idFila.ToString(), null);
                objLog.CrearArchivolog("[Entrada][ObtenerIdFilaPaquete][idAgrupaPaquete]", idAgrupaPaquete.ToString(), null);

                string idFilaPaquete = WebComunes.ObtenerIdFilaPaquete(idFila, idAgrupaPaquete);
                DataRow[] drSelect1 = dtDetalle.Select(String.Format("SOLIN_CODIGO = '{0}' AND AGRUPA = '{1}' AND ID_PRODUCTO = '{2}'", nroSEC, idAgrupaPaquete, idProducto));

                foreach (DataRow dr in drSelect1)
                {
                    if (idFilaPaquete.IndexOf(dr["ORDEN"].ToString()) > 0)
                    {
                        dt.ImportRow(dr);
                    }
                }
                objLog.CrearArchivolog("[Salida][ObtenerIdFilaPaquete][Count]", drSelect1.Count().ToString(), null);
            }

            objLog.CrearArchivolog("[Entrada][ObtenerDetalleSrvCuota][nroSEC]", nroSEC.ToString(), null);

            DataSet dtDetalleSrvCuota = (new BLEvaluacion()).ObtenerDetalleSrvCuota(Funciones.CheckInt64(nroSEC));
            string strCadenaServicio = string.Empty;

                   
            if (idProducto != constTipoProductoHFC && idProducto != constTipoProducto3PlayInalam)
            {
                foreach (DataRow dr in dtDetalleSrvCuota.Tables[0].Rows)
                {
                    strCadenaServicio += String.Format("{0};{1};{2}|", dr["SOPLN_CODIGO"].ToString(), dr["ID_SERVICIO"].ToString(), dr["SERVICIO"].ToString());
                }
                objLog.CrearArchivolog("[Salida][ObtenerDetalleSrvCuota]", strCadenaServicio.ToString(), null);
            }
            else
            {
                DataTable dtHFC = dtDetalleSrvCuota.Tables[0];
                DataRow[] drSelect1 = dtHFC.Select("FLG_PRINCIPAL = '1'");

                foreach (DataRow dr in drSelect1)
                {
                    foreach (DataRow dr1 in dtHFC.Rows)
                    {
                        if (dr["PAQUETE"].ToString() == dr1["PAQUETE"].ToString() && dr1["FLG_PRINCIPAL"].ToString() != "1")
                        {
                            strCadenaServicio += String.Format("{0};{1};{2};{3}|", dr1["SOPLN_CODIGO"].ToString(), dr["ORDEN"].ToString(), dr1["ID_SERVICIO"].ToString(), dr1["SERVICIO"].ToString());
                        }
                    }
                }
            }
            hidCadenaServicios.Value = strCadenaServicio;
            objLog.CrearArchivolog("    Inicio/dtDetalleSrvCuota   ", null, null);
            string strCadenaCuota = string.Empty;
            foreach (DataRow dr in dtDetalleSrvCuota.Tables[1].Rows)
            {
                double dblMontoCuota = 0.0;
                if (Funciones.CheckInt(dr["NRO_CUOTA"]) > 0)
                {
                    dblMontoCuota = Funciones.CheckDbl((Funciones.CheckDbl(dr["PRECIO_VENTA"]) - Funciones.CheckDbl(dr["MONTO_INICIAL"])) / Funciones.CheckInt(dr["NRO_CUOTA"]), 2);
                }

                strCadenaCuota += String.Format("{0};{1};{2};{3};{4}|", dr["CUOTA"].ToString(), dr["CUOTA_INICIAL"].ToString(), dr["MONTO_INICIAL"].ToString(), dblMontoCuota, dr["SOPLN_CODIGO"].ToString());
            }
            hidCadenaCuotas.Value = strCadenaCuota;

            string strCadenaEquipo = string.Empty;
            foreach (DataRow dr in dtDetalleSrvCuota.Tables[2].Rows)
            {
                strCadenaEquipo += String.Format("{0};{1};{2};{3}|", dr["SLPLN_CODIGO"].ToString(), dr["IDEQUIPO"].ToString(), dr["EQUIPO"].ToString(), dr["CF_ALQUILER"].ToString());
            }
            hidCadenaEquiposHFC.Value = strCadenaEquipo;

            if (idProducto == constTipoProductoDTH)
                dgrDTH.DataSource = dt;
          
            else if ((idProducto == constTipoProductoHFC) || (idProducto == constTipoProducto3PlayInalam))
                dgrHFC.DataSource = dt;
            else
            {//PROY-28061 PRESELECCION  INI
                dgrMovil.DataSource = dt;

                if (idProducto != constTipoProductoFijo)
                    dgrMovil.Columns[15].Visible = false;//PROY-140743

            }//PROY-28061 PRESELECCION FIN
            objLog.CrearArchivolog("    Inicio/Salida  ", null, null);
            dgrDTH.DataBind();
            dgrHFC.DataBind();
            dgrMovil.DataBind();

            //PROY-140743 - AE
            if (tipoOperacion != "25")
            {
                dgrMovil.Columns[10].Visible = false;
            }
        }

        protected void dgrMovil_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (tipoOperacion == constTipoOpeMigracion) e.Row.Cells[11].Text = "Nro. a Migrar";//PROY-140743
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (string.IsNullOrEmpty(((DataRowView)e.Row.DataItem).Row["PAQUETE"].ToString()))
                {
                    dgrMovil.Columns[1].Visible = false;
                }

                if (!(((DataRowView)e.Row.DataItem).Row["ID_PRODUCTO"].ToString() == constTipoProductoMovil || ((DataRowView)e.Row.DataItem).Row["ID_PRODUCTO"].ToString() == constTipoProductoFijo))
                {
                    dgrMovil.Columns[5].Visible = false;
                }

                string idTopeConsumo = ((DataRowView)e.Row.DataItem).Row["TPCON_CODIGO"].ToString();
                if (Funciones.CheckInt(idTopeConsumo) != (int)BEServicio.TIPO_TOPE_CONSUMO.TOPE_CONSUMO_ADICIONAL && validaTipoOpe != "25") //PROY-29296 //PROY-140743
                {
                    e.Row.Cells[5].Text = "&nbsp;";
                }

                if (string.IsNullOrEmpty(((DataRowView)e.Row.DataItem).Row["TELEFONO"].ToString()))
                {
                    dgrMovil.Columns[11].Visible = false;//PROY-140743
                }

                string idEquipo = Funciones.CheckStr(((DataRowView)e.Row.DataItem).Row["ID_EQUIPO"]);
                if (idEquipo == "")
                {
                    e.Row.Cells[8].Text = "&nbsp;";
                }
            }
        }

        protected void dgrHFC_ItemDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                intNro += 1;
                if (intNro > 1)
                {
                    e.Row.Cells[6].Text = string.Empty;
                    e.Row.Cells[8].Text = string.Empty;
                    e.Row.Cells[11].Text = string.Empty;//PROY-140743
                }

                string idProductoHFC = ((DataRowView)e.Row.DataItem).Row["ID_PRODUCTO_HFC"].ToString();
                if (idProductoHFC != ConfigurationManager.AppSettings["constTipoProductoTelefoniaHFC"].ToString() && idProductoHFC != ConfigurationManager.AppSettings["GSRVC_CODIGO_LTE"].ToString()) //'PROY-32581
                    e.Row.Cells[12].Text = string.Empty;//PROY-140743

                if (e.Row.Cells[9].Text == "-1")
                    e.Row.Cells[9].Text = ConfigurationManager.AppSettings["ConstTextSinTopeConsumo"].ToString();
            }
        }
    }
}