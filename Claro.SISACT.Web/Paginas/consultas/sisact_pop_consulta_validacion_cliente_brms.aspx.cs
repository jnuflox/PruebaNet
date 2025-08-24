//PROY-32439 MAS INI Nuevo
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Web.Base;
using Claro.SISACT.Entity;

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_consulta_validacion_cliente_brms : Sisact_Webbase
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
            Int64 nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]);
            GeneradorLog objLog = null;
            objLog = new GeneradorLog(null, Funciones.CheckStr(nroSEC), null, "WEB");
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][INICIO]", ""), null);
            List<BEParametro> lstBEEvaluacionProactiva = new List<BEParametro>();
            DataSet obj = new DataSet();
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][ObtenerDatos_Validacion_Cliente_BRMS][INICIO]", ""), null);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][ObtenerDatos_Validacion_Cliente_BRMS][IN | NUMERO DE SEC]", Funciones.CheckStr(nroSEC)), null);
            obj = (new BLEvaluacion()).ObtenerDatos_Validacion_Cliente_BRMS(nroSEC);
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][ObtenerDatos_Validacion_Cliente_BRMS][FIN]", ""), null);
            DataTable tblAuxSolicitud = obj.Tables["cout_request_datos"];
            DataTable tblAuxCliente = obj.Tables["cout_request_cliente"];
            DataTable tblAuxLinea = obj.Tables["cout_request_linea"];
            DataTable tblAuxPdv = obj.Tables["cout_request_pdv"];
            try
            {
                //Se valida objetos llenos como minimo
                if (!Object.Equals(tblAuxSolicitud, null) || !Object.Equals(tblAuxCliente, null) || !Object.Equals(tblAuxPdv, null))
                {
                    //El recorrido de cada objeto es para que solo imprima el RUC
                    //Sin el recorrido y el Break imprimiría todos los RRLL
                    #region Filtrar
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][CARGA DE DATOS][INICIO]", ""), null);
                    #region Solicitud
                    DataTable tblSolicitud = new DataTable();
                    tblSolicitud.Columns.Add("sistema_evaluacion",typeof(string));
                    tblSolicitud.Columns.Add("tipo_operacion", typeof(string));
                    DataRow rowSolicitud = tblSolicitud.NewRow();
                    foreach (DataRow item in tblAuxSolicitud.Rows)
                    {
                        rowSolicitud["sistema_evaluacion"] = item["sistema_evaluacion"];
                        rowSolicitud["tipo_operacion"] = item["tipo_operacion"];
                        tblSolicitud.Rows.Add(rowSolicitud);
                        objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][CARGA DE DATOS][CARGA DATOS SOLICITUD]", ""), null);
                        break;
                    }
                    #endregion
                    #region Filtrar Cliente
                    DataTable tblCliente = new DataTable();
                    tblCliente.Columns.Add("antiguedad_deuda", typeof(string));
                    tblCliente.Columns.Add("bloqueos", typeof(string));
                    tblCliente.Columns.Add("cantidad_documentos_deuda", typeof(string));
                    tblCliente.Columns.Add("comportamiento_pago", typeof(string));
                    tblCliente.Columns.Add("disputa_antiguedad", typeof(string));

                    tblCliente.Columns.Add("disputa_cantidad", typeof(string));
                    tblCliente.Columns.Add("disputa_monto", typeof(string));
                    tblCliente.Columns.Add("documento_numero", typeof(string));
                    tblCliente.Columns.Add("documento_tipo", typeof(string));
                    tblCliente.Columns.Add("flag_bloqueos", typeof(string));

                    tblCliente.Columns.Add("flag_suspensiones", typeof(string));
                    tblCliente.Columns.Add("monto_deuda", typeof(string));
                    tblCliente.Columns.Add("monto_deuda_castigada", typeof(string));
                    tblCliente.Columns.Add("monto_deuda_vencida", typeof(string));
                    tblCliente.Columns.Add("monto_total_pago", typeof(string));

                    tblCliente.Columns.Add("promedio_facturado_soles", typeof(string));
                    tblCliente.Columns.Add("suspensiones", typeof(string));
                    tblCliente.Columns.Add("tiempo_permamencia", typeof(string));
                    tblCliente.Columns.Add("tipos_fraude", typeof(string));
                    tblCliente.Columns.Add("segmento_cliente", typeof(string)); // PROY-140422

                    DataRow rowCliente = tblCliente.NewRow();
                    foreach (DataRow item in tblAuxCliente.Rows)
                    {
                        rowCliente["antiguedad_deuda"] = item["antiguedad_deuda"];
                        rowCliente["bloqueos"] = item["bloqueos"];
                        rowCliente["cantidad_documentos_deuda"] = item["cantidad_documentos_deuda"];
                        rowCliente["comportamiento_pago"] = item["comportamiento_pago"];
                        rowCliente["disputa_antiguedad"] = item["disputa_antiguedad"];

                        rowCliente["disputa_cantidad"] = item["disputa_cantidad"];
                        rowCliente["disputa_monto"] = item["disputa_monto"];
                        rowCliente["documento_numero"] = "'" + item["documento_numero"];
                        rowCliente["documento_tipo"] = item["documento_tipo"];
                        rowCliente["flag_bloqueos"] = item["flag_bloqueos"];

                        rowCliente["flag_suspensiones"] = item["flag_suspensiones"];
                        rowCliente["monto_deuda"] = item["monto_deuda"];
                        rowCliente["monto_deuda_castigada"] = item["monto_deuda_castigada"];
                        rowCliente["monto_deuda_vencida"] = item["monto_deuda_vencida"];
                        rowCliente["monto_total_pago"] = item["monto_total_pago"];

                        rowCliente["promedio_facturado_soles"] = item["promedio_facturado_soles"];
                        rowCliente["suspensiones"] = item["suspensiones"];
                        rowCliente["tiempo_permamencia"] = item["tiempo_permamencia"];
                        rowCliente["tipos_fraude"] = item["tipos_fraude"];
                        rowCliente["segmento_cliente"] = item["segmento_cliente"]; //PROY-140422
                        tblCliente.Rows.Add(rowCliente);
                        objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][CARGA DE DATOS][CARGA DATOS CLIENTE]", ""), null);
                        break;
                    }
                    #endregion
                    #region Filtrar Linea
                    DataTable tblLinea = null;
                    if (!Object.Equals(tblAuxLinea, null))
                    {
                        tblLinea = new DataTable();
                        tblLinea.Columns.Add("antiguedad", typeof(string));
                        tblLinea.Columns.Add("fecha_activacion", typeof(string));
                        DataRow rowLinea = tblLinea.NewRow();
                        foreach (DataRow item in tblAuxLinea.Rows)
                        {
                            rowLinea["antiguedad"] = item["antiguedad"];
                            rowLinea["fecha_activacion"] = item["fecha_activacion"];
                            tblLinea.Rows.Add(rowLinea);
                            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][CARGA DE DATOS][CARGA DATOS LINEA]", ""), null);
                            break;
                        }
                    }
                    #endregion
                    #region Filtrar PdV
                    DataTable tblPdv = new DataTable();
                    tblPdv.Columns.Add("canal", typeof(string));
                    tblPdv.Columns.Add("codigo", typeof(string));
                    tblPdv.Columns.Add("departamento", typeof(string));
                    tblPdv.Columns.Add("nombre", typeof(string));
                    tblPdv.Columns.Add("region", typeof(string));
                    tblPdv.Columns.Add("vendedor_codigo", typeof(string));
                    tblPdv.Columns.Add("vendedor_nombre", typeof(string));
                    DataRow rowPdV = tblPdv.NewRow();
                    foreach (DataRow item in tblAuxPdv.Rows)
                    {
                        rowPdV["canal"] = item["canal"];
                        rowPdV["codigo"] = "'" + item["codigo"];
                        rowPdV["departamento"] = item["departamento"];
                        rowPdV["nombre"] = item["nombre"];
                        rowPdV["region"] = item["region"];
                        rowPdV["vendedor_codigo"] = item["vendedor_codigo"];
                        rowPdV["vendedor_nombre"] = item["vendedor_nombre"];
                        tblPdv.Rows.Add(rowPdV);
                        objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][CARGA DE DATOS][CARGA DATOS PDV]", ""), null);
                        break;
                    }
                    #endregion
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][CARGA DE DATOS][FIN]", ""), null);
                    #endregion

                    #region Llenar datos
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][LLENAR DE DATOS][INICIO]", ""), null);
                    #region Solicitud
                    dgSolicitud.DataSource = tblSolicitud;
                    dgSolicitud.DataBind();
                    #endregion

                    #region Cliente
                    dgCliente.DataSource = tblCliente;
                    dgCliente.DataBind();
                    #endregion

                    #region Bloqueos

                    //INC000002880679 - INI
                    DataTable dtBloqueos = new DataTable();
                    dtBloqueos.Columns.Add("tipo");
                    dtBloqueos.Columns.Add("tipoLinea");
                    //INC000002880679 - FIN

                    ValidacionDeudaBRMSrequest.Cliente.Bloqueo objBloqueo = null;
                    foreach (DataRow row in tblCliente.Rows)
                    {
                        objLog.CrearArchivolog(null, string.Format("INC000002880679 Cadena de bloqueos: {0}", Funciones.CheckStr(row["bloqueos"])), null); //INC000002880679
                        objLog.CrearArchivolog(null, string.Format("INC000002880679 (!Object.Equals(row['bloqueos'], null)): {0}", Funciones.CheckStr(!Object.Equals(row["bloqueos"], null))), null); //INC000002880679

                        if (!Object.Equals(row["bloqueos"], null))
                        {
                            objBloqueo = new ValidacionDeudaBRMSrequest.Cliente.Bloqueo();

                            foreach (var bloqueo in row["bloqueos"].ToString().Split(';').ToList())
                            {
                                objLog.CrearArchivolog(null, string.Format("INC000002880679 bloqueo: {0}", Funciones.CheckStr(bloqueo)), null); //INC000002880679
                                objLog.CrearArchivolog(null, string.Format("INC000002880679 (!String.IsNullOrWhiteSpace(bloqueo)): {0}", Funciones.CheckStr(!String.IsNullOrWhiteSpace(bloqueo))), null); //INC000002880679

                                if (!String.IsNullOrWhiteSpace(bloqueo))
                                {
                                    objBloqueo.tipo = bloqueo.Split(',')[0];
                                    objBloqueo.tipoLinea = bloqueo.Split(',')[1];
                                    objLog.CrearArchivolog(null, string.Format("INC000002880679 objBloqueo.tipo: {0}", Funciones.CheckStr(objBloqueo.tipo)), null); //INC000002880679
                                    objLog.CrearArchivolog(null, string.Format("INC000002880679 objBloqueo.tipoLinea: {0}", Funciones.CheckStr(objBloqueo.tipoLinea)), null); //INC000002880679
                                    dtBloqueos.Rows.Add(objBloqueo.tipo, objBloqueo.tipoLinea); //INC000002880679
                                }
                            }
                        }
                        break;
                    }

                    dgBloqueos.DataSource = dtBloqueos; //INC000002880679
                    dgBloqueos.DataBind();
                    #endregion

                    #region Disputa
                    dgDisputa.DataSource = tblCliente;
                    dgDisputa.DataBind();
                    #endregion

                    #region Documento
                    dgDocumento.DataSource = tblCliente;
                    dgDocumento.DataBind();
                    #endregion

                    #region Suspensiones
                    List<ValidacionDeudaBRMSrequest.Cliente.Suspension> lstSuspensiones = null;
                    ValidacionDeudaBRMSrequest.Cliente.Suspension objSuspencion = null;
                    foreach (DataRow row in tblCliente.Rows)
                    {
                        if (!Object.Equals(row["suspensiones"], null))
                        {
                            lstSuspensiones = new List<ValidacionDeudaBRMSrequest.Cliente.Suspension>();
                            objSuspencion = new ValidacionDeudaBRMSrequest.Cliente.Suspension();
                            foreach (var suspencion in row["suspensiones"].ToString().Split(';').ToList())
                            {
                                if (!String.IsNullOrWhiteSpace(suspencion))
                                {
                                    objSuspencion.tipo = suspencion.Split(',')[0];
                                    objSuspencion.tipoLinea = suspencion.Split(',')[1];
                                    lstSuspensiones.Add(objSuspencion);
                                }
                            }
                        }
                        break;//Break para solo pintar fila Ruc y no los de RRLL
                    }
                    dgSuspensiones.DataSource = lstSuspensiones;
                    dgSuspensiones.DataBind();
                    #endregion

                    #region Linea
                    dgLineaARenovar.DataSource = tblLinea;
                    dgLineaARenovar.DataBind();
                    #endregion

                    #region PDV
                    dgPuntoDeVenta.DataSource = tblPdv;
                    dgPuntoDeVenta.DataBind();
                    #endregion

                    #region Vendedor
                    dgVendedor.DataSource = tblPdv;
                    dgVendedor.DataBind();
                    #endregion
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][LLENAR DE DATOS][FIN]", ""), null);
                    #endregion
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][GENERAR_ARCHIVO.XLS][INICIO]", ""), null);
                    Response.Clear();
                    Response.Charset = "UTF-8";
                    Response.AddHeader("Content-Disposition", "Attachment;Filename=Reporte_Validacion_Cliente_BRMS.xls");
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.ContentEncoding = System.Text.Encoding.GetEncoding(1252);
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][GENERAR_ARCHIVO.XLS][GENERO EXITOSAMENTE]", ""), null);
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][GENERAR_ARCHIVO.XLS][FIN]", ""), null);
                }
                else
                {
                    objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][NO SE ENCONTRÓ DATOS AL CARGAR]", ""), null);
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE][ERROR EXPORTAR BRMS]", ""), null);
            }
            objLog.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][EXPORTARBRMSVALIDACIÓNCLIENTE]][FIN]", ""), null);
        }
    }
}
//PROY-32439 MAS FIN Nuevo