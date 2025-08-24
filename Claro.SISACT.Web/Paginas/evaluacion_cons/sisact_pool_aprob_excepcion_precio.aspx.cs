using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Web.Base;
using Claro.SISACT.Common;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.WS.RestReferences;
using Claro.SISACT.Entity.claro_vent_pedidostienda.Response;
using Claro.SISACT.Entity.claro_vent_pedidostienda.Request;
using Claro.SISACT.Web.Comun;

namespace Claro.SISACT.Web.Paginas.evaluacion_cons
{
    public partial class sisact_pool_aprob_excepcion_precio : Sisact_Webbase
    {
        #region currentPage
        public object CurrentPage
        {

            get
            {
                return ViewState["CurrentPage"];
            }
            set
            {
                ViewState["CurrentPage"] = value;
            }
        }
        #endregion

        GeneradorLog _objLog;
        static string archivoLog = "sisact_pool_aprob_excepcion_precio";
        static string idEvento = "INICIATIVA-803 IDEA-142474";
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write("<script language='javascript' src='../../Scripts/funciones_sec.js'></script>");

            if (Funciones.CheckStr(HttpContext.Current.Request.QueryString["cu"]).Length == 0)
            {
                Response.Write("<script language=javascript>validarUrl();</script>");
            }
            else
            {
                Response.Write("<script language='javascript'>restringirEventos();</script>");
            }
            _objLog = new GeneradorLog(CurrentUsers, null, null, archivoLog);
            if (!IsPostBack)
            {
                _objLog.CrearArchivolog(idEvento, "INICIATIVA-803 IDEA-142474 | INICIO Page_Load ", null);
                HttpContext.Current.Session["CurrentTerminal"] = null;
                HttpContext.Current.Session["CurrentUser"] = null;

                string codUsuarioExt = Request.QueryString["cu"];
                HttpContext.Current.Session["CurrentTerminal"] = CurrentTerminal;
                HttpContext.Current.Session["CurrentUser"] = CurrentUser;

                if (!AccesoUsuario.ValidarAcceso(codUsuarioExt, CurrentUser))
                {
                    string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                    Response.Redirect(strRutaSite);
                    return;
                }

                Inicio();

                _objLog.CrearArchivolog(idEvento, "INICIATIVA-803 IDEA-142474 | FIN Page_Load ", null);
            }

        }


        private void Inicio()
        {
            txtbusqueda.Enabled = false;
            DateTime datFechaFin = DateTime.Now;
            txtFecha.Text = string.Format("{0:dd/MM/yyyy}", datFechaFin);
            CargarCombos();
            dllEstadoSolicitud.Items.Insert(0, new ListItem(ConfigurationManager.AppSettings["constTextoSELECCIONE"], string.Empty));
            dllTipoBusqueda.Items.Insert(0, new ListItem(ConfigurationManager.AppSettings["constTextoSELECCIONE"], string.Empty));

            dllEstadoSolicitud.SelectedValue = string.Empty;
            dllTipoBusqueda.SelectedValue = string.Empty;
            Consultar();
        }


        private void Limpiar()
        {
            gvPool.DataSource = null;
            gvPool.DataBind();
            txtbusqueda.Text = string.Empty;
            dllEstadoSolicitud.SelectedValue = string.Empty;
            dllTipoBusqueda.SelectedValue = string.Empty;
            lblCantReg.Text = string.Empty;
            Session.Remove("listaPedidosExcepPre");
        }

        private void CargarCombos()
        {
            var comboEstado = ReadKeySettings.Key_EstadosSolicitud;
            var comboBusqueda = ReadKeySettings.Key_TipoBusquedaPool;


            char[] separador = { '|' };
            //Cargando combo Estado
            var selectListItemsEstado = new List<EstadoSolicitud>();

            var strlist = comboEstado.Split(separador);


            foreach (var combo in strlist)
            {
                if (combo != string.Empty)
                {
                    EstadoSolicitud c = new EstadoSolicitud();
                    var tamanio = combo.Length - 2;
                    c.Text = combo.Substring(0, 1);
                    c.Value = combo.Substring(2, tamanio);

                    selectListItemsEstado.Add(c);
                }
            }
            selectListItemsEstado.Add(new EstadoSolicitud() { Text = "-1", Value = ConfigurationManager.AppSettings["constTextoTODOS"] });
            dllEstadoSolicitud.DataSource = selectListItemsEstado.ToList();
            dllEstadoSolicitud.DataValueField = "Text";
            dllEstadoSolicitud.DataTextField = "Value";

            //Data Bind Estado
            dllEstadoSolicitud.DataBind();

            //Cargando combo Busqueda
            var selectListItemsBusqueda = new List<TipoBusqueda>();

            var strlist2 = comboBusqueda.Split(separador);


            foreach (var combo in strlist2)
            {
                if (combo != string.Empty)
                {
                    var c = new TipoBusqueda();
                    var tamanio = combo.Length - 2;
                    c.Text = combo.Substring(0, 1);
                    c.Value = combo.Substring(2, tamanio);

                    selectListItemsBusqueda.Add(c);
                }
            }

            dllTipoBusqueda.DataSource = selectListItemsBusqueda.ToList();
            dllTipoBusqueda.DataValueField = "Text";
            dllTipoBusqueda.DataTextField = "Value";

            //Data Bind Busqueda
            dllTipoBusqueda.DataBind();
        }


        class EstadoSolicitud
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        class TipoBusqueda
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        protected void btnConsultar_Click(object sender, EventArgs e)
        {
            Consultar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        public void Consultar()
        {

            HttpContext.Current.Session["listaPedidosExcepPre"] = null;
            var strFechaInicio = (!Page.IsPostBack) ? txtFecha.Text : Request.Form[txtFecha.UniqueID];
            hdFechaG.Value = strFechaInicio;
            var strTipoBusqueda = dllTipoBusqueda.SelectedValue;
            var strTxtBusqueda = txtbusqueda.Text;
            var estadoPa = ReadKeySettings.Key_CodEstadoPendAprobacion;
            var strEstado = estadoPa;
            if (!string.IsNullOrEmpty(dllEstadoSolicitud.SelectedValue))
            {
                strEstado = dllEstadoSolicitud.SelectedValue;
                if (strEstado == "-1")
                {
                    strEstado = string.Empty;
                }
            }
            //declarando las variables para el parametro
            var pedidoTienda = string.Empty;
            var pedidoSinergia = string.Empty;
            var solincodigo = string.Empty;

            var aplicacion = ConfigurationManager.AppSettings["constAplicacion"];

            switch (strTipoBusqueda)
            {
                case "1":
                    solincodigo = strTxtBusqueda;
                    pedidoSinergia = string.Empty;
                    pedidoTienda = string.Empty;
                    break;
                case "2":
                    pedidoSinergia = strTxtBusqueda;
                    solincodigo = string.Empty;
                    pedidoTienda = string.Empty;
                    break;
                case "3":
                    pedidoSinergia = string.Empty;
                    solincodigo = string.Empty;
                    pedidoTienda = strTxtBusqueda;
                    break;
                case "":
                    pedidoSinergia = string.Empty;
                    solincodigo = string.Empty;
                    pedidoTienda = string.Empty;
                    break;
            }


            _objLog.CrearArchivolog(string.Format("{0} | {1}", idEvento, " ******************************************* INICIO Consultar() *******************************************"), null, null);
            try
            {
                var objRestReference = new RestConsultarPedidosTienda();
                var dtconversionfecha = Convert.ToDateTime(strFechaInicio);
                var fechasolicitud = dtconversionfecha.ToString("yyyy-MM-dd");

                var oPuntoVenta = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];
                var puntoVenta = oPuntoVenta.OficinaVenta;

                var dcParameters = ValidarRol(pedidoTienda, pedidoSinergia, solincodigo, fechasolicitud, aplicacion, puntoVenta);

                _objLog.CrearArchivolog(string.Format("INICIATIVA-803 IDEA-142474 | pedidoTienda : {0} ", pedidoTienda), null, null);
                _objLog.CrearArchivolog(string.Format("INICIATIVA-803 IDEA-142474 | pedidoSinergia : {0} ", pedidoSinergia), null, null);
                _objLog.CrearArchivolog(string.Format("INICIATIVA-803 IDEA-142474 | solinCodigo : {0} ", solincodigo), null, null);
                _objLog.CrearArchivolog(string.Format("INICIATIVA-803 IDEA-142474 | fechaSolicitud : {0} ", strFechaInicio), null, null);
                _objLog.CrearArchivolog(string.Format("INICIATIVA-803 IDEA-142474 | estado : {0} ", strEstado), null, null);
                _objLog.CrearArchivolog(string.Format("INICIATIVA-803 IDEA-142474 | aplicacion : {0} ", aplicacion), null, null);
                _objLog.CrearArchivolog(string.Format("INICIATIVA-803 IDEA-142474 | aplicacion : {0} ", puntoVenta), null, null);

                var objBeAuditoriaRequest = WebComunes.obtenerAuditoriaDataPower("Time_Out_PedidosTV", CurrentUsers);
                objBeAuditoriaRequest.urlRest = "PedidoVenta_Url";

                _objLog.CrearArchivolog("INICIATIVA-803 IDEA-142474 ", "*****************************************restConsultarPedidosVenta*****************************************", null);
                var obj = objRestReference.restConsultarPedidosVenta(dcParameters, objBeAuditoriaRequest);
                var objResponseBody = (PedidosTiendaResponseBody)obj.getMessageResponse().getBody();

                if (objResponseBody.dataResponse == null)
                {
                    lblCantReg.Text = "No Existen Registros";
                    gvPool.DataSource = null;
                    gvPool.DataBind();
                    Session.Remove("listaPedidosExcepPre");
                }
                else
                {

                    lblCantReg.Text = string.Format("{0} registro(s) encontrado(s)", objResponseBody.dataResponse.listaPedidosExcepPre.Count.ToString());
                    gvPool.DataSource = objResponseBody.dataResponse.listaPedidosExcepPre;
                    CurrentPage = objResponseBody.dataResponse.listaPedidosExcepPre;
                    gvPool.DataBind();
                    HttpContext.Current.Session["listaPedidosExcepPre"] = objResponseBody.dataResponse.listaPedidosExcepPre;
                }

                ScriptManager.RegisterStartupScript(this, typeof(Page), new Guid().ToString(), "funcionRetornaConsultar();", true);
                ScriptManager.RegisterStartupScript(this, typeof(Page), new Guid().ToString(), "FechaAnterior();", true);

            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog("INICIATIVA-803 IDEA-142474 ", "ERROR btnConsultar_Click", ex);
                Response.Write("<script language=javascript>alert('A ocurrido un error en la busqueda " + ex.Message + "');</script>");
            }
            _objLog.CrearArchivolog(string.Format("{0} | {1}", idEvento, " ******************************************* INICIO Consultar() *******************************************"), null, null);
        }

        public Dictionary<string, string> ValidarRol(string pedidoTienda, string pedidoSinergia, string solincodigo,
        string fechasolicitud, string aplicacion, string PuntoVenta)
        {
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|INICIO PROCESO {0}", "VALIDAR ROL"));
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|pedidoTienda : {0} | pedidoSinergia : {1} | solincodigo : {2} | fechasolicitud : {3} | aplicacion : {4} | PuntoVenta : {5} ", pedidoTienda, pedidoSinergia, solincodigo, fechasolicitud, aplicacion, PuntoVenta));
            var isAprobador = false;
            var isValidador = false;

            var dcParameters = new Dictionary<string, string>();

            var operfil = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|cadenaPerfil : {0} ", Funciones.CheckStr(operfil.CadenaPerfil)));
            var arrperfil = operfil.CadenaPerfil.Split(',');

            var keyPerfilesAprobadores = ReadKeySettings.keyPerfilesAprobadores;
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|PERFIL APROBADORES {0}", keyPerfilesAprobadores));
            var keyPerfilesValidadores = ReadKeySettings.keyPerfilesValidadores;
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|PERFIL VALIDADORES {0}", keyPerfilesValidadores));
            var estadoPa = ReadKeySettings.Key_CodEstadoPendAprobacion;
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|ESTADO DE APROBADORES {0}", estadoPa));
            var estadoPv = ReadKeySettings.Key_EstadoPendValidacion;
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|ESTADO DE VALIDADORES {0}", estadoPv));
            string strEstado;

            var arrListaParametroAprobadores = keyPerfilesAprobadores.Split('|');
            if (arrListaParametroAprobadores.Any(item => arrperfil.Any(itemPerfil => item == itemPerfil)))
            {
                isAprobador = true;

            }

            if (isAprobador == false)
            {
                var arrListaParametroValidadores = keyPerfilesValidadores.Split('|');
                if (arrListaParametroValidadores.Any(item => arrperfil.Any(itemPerfil => item == itemPerfil)))
                    {
                    isValidador = true;

                }
            }
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|USUARIO LOGEADO--> {0}", operfil.Login));
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|ES APROBADOR--> {0}", isAprobador));
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|ES VALIDADOR--> {0}", isValidador));

            if (isAprobador)
            {
                strEstado = estadoPa;

                if (!string.IsNullOrEmpty(dllEstadoSolicitud.SelectedValue))
                {
                    strEstado = dllEstadoSolicitud.SelectedValue;
                    if (strEstado == "-1")
                    {
                        strEstado = string.Empty;
                    }
                }
                hdFlagRol.Value = "A";
            }

            else if (isValidador)
            {
                strEstado = estadoPv;

                if (!string.IsNullOrEmpty(dllEstadoSolicitud.SelectedValue))
                {
                    strEstado = dllEstadoSolicitud.SelectedValue;
                    if (strEstado == "-1")
                    {
                        strEstado = string.Empty;
                    }
                }

                hdFlagRol.Value = "V";
            }

            else
            {
                strEstado = estadoPv;
                if (!string.IsNullOrEmpty(dllEstadoSolicitud.SelectedValue))
                {
                    strEstado = dllEstadoSolicitud.SelectedValue;
                    if (strEstado == "-1")
                    {
                        strEstado = string.Empty;
                    }
                }
                hdFlagRol.Value = "K";
            }

                dcParameters.Add("pedidoTienda", pedidoTienda);
                dcParameters.Add("pedidoSinergia", pedidoSinergia);
                dcParameters.Add("solinCodigo", solincodigo);
                dcParameters.Add("fechaSolicitud", fechasolicitud);
            dcParameters.Add("estado", (dllTipoBusqueda.SelectedValue != string.Empty) ? string.Empty : strEstado);
            dcParameters.Add("aplicacion", (isValidador || isAprobador) ? string.Empty : aplicacion);
            dcParameters.Add("puntoVenta", (isValidador || isAprobador) ? string.Empty : PuntoVenta);
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|FIN PROCESO {0}", "VALIDAR ROL"));
            return dcParameters;
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool AprobarAnular(string Id, string flag_, string comentario)
        {
            var dev = false;

            var objConsultarPedidosT = new RestConsultarPedidosTienda();
            var objAprobar = new MessageRequestPT();
            var objrechazar = new MessageRequestPT();
            var objrevaluar = new MessageRequestPT();
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|INICIO PROCESO {0}", flag_));
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|id : {0} | flag : {1} | comentario : {2}", Id, flag_, comentario));
            try
            {
                #region Auditoria
                var objBEAuditoriaRequest = WebComunes.obtenerAuditoriaDataPower("Time_Out_PedidosTV", CurrentUsers);
                #endregion

                var objBody = ObtenerDatosAnularAprobar(Id, flag_, comentario);
                var dcParameters = new Dictionary<string, string>();
                var headersRequest = objConsultarPedidosT.GetHeader_v2();
                headersRequest.dispositivo = Sisact_Webbase.CurrentTerminal;
                headersRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
                headersRequest.modulo = Funciones.CheckStr(ConfigurationManager.AppSettings["DP_consModulo_Generico"]);
                headersRequest.operation = Funciones.CheckStr(flag_);
                headersRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["conswsipPedidosTienda"]);
                PedidosTiendaResponse ptresponse;
                switch (flag_)
                {
                    case "Aprobar":
                    objBEAuditoriaRequest.urlRest = "urlAprobarPedidosTienda";
                    objAprobar.getMessageRequest().getHeader().setHeader(headersRequest);
                    objAprobar.getMessageRequest().setBody(objBody);

                    ptresponse = objConsultarPedidosT.AprobarPedidosTienda(dcParameters, objAprobar, objBEAuditoriaRequest);
                        break;
                    case "Anular":
                    objBEAuditoriaRequest.urlRest = "urlAnularPedidosTienda";
                    objrechazar.getMessageRequest().getHeader().setHeader(headersRequest);
                    objrechazar.getMessageRequest().setBody(objBody);
                    ptresponse = objConsultarPedidosT.AnularPedidosTienda(dcParameters, objrechazar, objBEAuditoriaRequest);
                        break;
                    default:
                    objBEAuditoriaRequest.urlRest = "urlActualizarPedidosTienda";
                    objrevaluar.getMessageRequest().getHeader().setHeader(headersRequest);
                    objrevaluar.getMessageRequest().setBody(objBody);
                    ptresponse = objConsultarPedidosT.ValidarPedidosTienda(dcParameters, objrevaluar, objBEAuditoriaRequest);
                        break;
                }

                var objResponseBody = (PedidosTiendaResponseBody)ptresponse.getMessageResponse().getBody();
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|objResponseBody.auditResponse.codigoRespuesta {0}", objResponseBody.auditResponse.codigoRespuesta));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|objResponseBody.auditResponse.mensajeRespuesta {0}", objResponseBody.auditResponse.mensajeRespuesta));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|objResponseBody.auditResponse.idTransaccion {0}", objResponseBody.auditResponse.idTransaccion));
                if (objResponseBody.auditResponse.codigoRespuesta == "0")
                {
                    dev = true;
                }
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("Dev {0}", dev));
            }
            catch (Exception ex)
            {
                GeneradorLog.EscribirLog(archivoLog, idEvento, "INICIATIVA-803 IDEA-142474|ERROR AprobarAnular", ex);
                dev = false;
            }
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|FIN PROCESO {0}", flag_));
            return dev;
        }

        public static BodyRequestAprobarPT ObtenerDatosAnularAprobar(string Id, string flag, string comentario)
        {
            var usuario = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];
            var objBody = new BodyRequestAprobarPT();
            var listaPedidosExcepPre = (List<BEDatosPTDetalle>)HttpContext.Current.Session["listaPedidosExcepPre"];
            var pedidoExcepPre = listaPedidosExcepPre.FirstOrDefault(x => x.idSolicitud == Id);
            GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("id {0} | flag {1} | comentario {2}", Id, flag, comentario));
            GeneradorLog.EscribirLog(archivoLog, idEvento, "INICIATIVA-803 IDEA-142474|INICIO|ObtenerDatosAnularAprobar");
            switch (flag)
            {
                case "Aprobar":

                    if (pedidoExcepPre != null)
                    {
                        objBody.aprobarExcepPreRequest = new BEDatosPTDetalle
                        {
                            idSolicitud = pedidoExcepPre.idSolicitud,
                            idFlujo = pedidoExcepPre.idFlujo,
                            usuarioAprobador = usuario.Login,
                            nodoAprob = CurrentHostName,
                            solinCodigo = pedidoExcepPre.solinCodigo,
                            estadoPosterior = pedidoExcepPre.estadoPosterior,
                            pedidoSinergia = pedidoExcepPre.pedidoSinergia
                        };
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|idSolicitud : {0}", objBody.aprobarExcepPreRequest.idSolicitud));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|idFlujo:  {0}", objBody.aprobarExcepPreRequest.idFlujo));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|usuarioAprobador : {0}", objBody.aprobarExcepPreRequest.usuarioAprobador));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|nodoAprob : {0}", objBody.aprobarExcepPreRequest.nodoAprob));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|solinCodigo : {0}", objBody.aprobarExcepPreRequest.solinCodigo));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|estadoPosterior : {0}", objBody.aprobarExcepPreRequest.estadoPosterior));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|pedidoSinergia {0}", objBody.aprobarExcepPreRequest.pedidoSinergia));
            }

                    break;
                case "Anular":
                    {
                        if (pedidoExcepPre != null)
                        {
                            objBody.anularExcepPreRequest = new BEDatosPTDetalle
            {
                                idSolicitud = pedidoExcepPre.idSolicitud,
                                idFlujo = pedidoExcepPre.idFlujo,
                                solinCodigo = pedidoExcepPre.solinCodigo,
                                comentario = comentario,
                                usuarioRegistro = usuario.Login,
                                pedidoSinergia = pedidoExcepPre.pedidoSinergia,
                                oficinaSinergia = pedidoExcepPre.oficinaSinergia
                            };
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|idSolicitud : {0}", objBody.anularExcepPreRequest.idSolicitud));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|idFlujo:  {0}", objBody.anularExcepPreRequest.idFlujo));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|solinCodigo : {0}", objBody.anularExcepPreRequest.solinCodigo));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|idEvento : {0}", objBody.anularExcepPreRequest.comentario));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|usuarioAprobador : {0}", objBody.anularExcepPreRequest.usuarioAprobador));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|pedidoSinergia : {0}", objBody.anularExcepPreRequest.pedidoSinergia));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|oficinaSinergia : {0}", objBody.anularExcepPreRequest.oficinaSinergia));
            }

                        break;
                    }
                default:
            {
                        if (pedidoExcepPre != null)
                        {
                            objBody.actualizarExcepPreRequest = new BEDatosPTDetalle
            {
                                codigo = pedidoExcepPre.idSolicitud,
                                pedidoSinergia = pedidoExcepPre.pedidoSinergia,
                                estado = ReadKeySettings.Key_CodEstadoPendAprobacion,
                                usuarioValidador = usuario.Login,
                                nodoValidador = CurrentHostName,
                                observacionValidador = string.Empty
                            };
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|oficinaSinergia : {0}", objBody.actualizarExcepPreRequest.codigo));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|pedidoSinergia : {0}", objBody.actualizarExcepPreRequest.pedidoSinergia));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|estado : {0}", objBody.actualizarExcepPreRequest.estado));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|usuarioValidador : {0}", objBody.actualizarExcepPreRequest.usuarioValidador));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|nodoValidador : {0}", objBody.actualizarExcepPreRequest.nodoValidador));
                GeneradorLog.EscribirLog(archivoLog, idEvento, string.Format("INICIATIVA-803 IDEA-142474|observacionValidador : {0}", objBody.actualizarExcepPreRequest.observacionValidador));
            }

                        break;
                    }
            }
            GeneradorLog.EscribirLog(archivoLog, idEvento, "INICIATIVA-803 IDEA-142474|FIN|ObtenerDatosAnularAprobar");
            return objBody;
        }

        protected void gvPool_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string rol = hdFlagRol.Value;
                var imgAnular = (Image)e.Row.FindControl("imgRechazar");
                var imgaprobar = (Image)e.Row.FindControl("imgEvaluar");
                var imgValidar = (Image)e.Row.FindControl("imgValidar");

                var cuotainiTv = gvPool.DataKeys[e.Row.RowIndex].Values[1] == null ? "0" : gvPool.DataKeys[e.Row.RowIndex].Values[1].ToString();
                var cuotainiSisact = gvPool.DataKeys[e.Row.RowIndex].Values[2] == null ? "0" : gvPool.DataKeys[e.Row.RowIndex].Values[2].ToString();
                var preciotienda = gvPool.DataKeys[e.Row.RowIndex].Values[3] == null ? "0" : gvPool.DataKeys[e.Row.RowIndex].Values[3].ToString();
                var precioSisact = gvPool.DataKeys[e.Row.RowIndex].Values[4] == null ? "0" : gvPool.DataKeys[e.Row.RowIndex].Values[4].ToString();

                if (preciotienda != "0")
                {
                    decimal d;
                    decimal.TryParse(e.Row.Cells[14].Text, out d);
                    e.Row.Cells[14].Text = d.ToString("N2");
                }
                if (precioSisact != "0")
                {
                    decimal f;
                    decimal.TryParse(e.Row.Cells[15].Text, out f);
                    e.Row.Cells[15].Text = f.ToString("N2");
                }

                if (cuotainiTv != "0")
                {
                    decimal g;
                    decimal.TryParse(e.Row.Cells[12].Text, out g);
                    e.Row.Cells[12].Text = g.ToString("N2");
                }

                if (cuotainiSisact != "0")
                {
                    decimal y;
                    decimal.TryParse(e.Row.Cells[13].Text, out y);
                    e.Row.Cells[13].Text = y.ToString("N2");
                }
                var estado = gvPool.DataKeys[e.Row.RowIndex].Values[0] == null ? "1" : gvPool.DataKeys[e.Row.RowIndex].Values[0].ToString();


                if (rol == "A")
                {
                    if (Convert.ToString(ReadKeySettings.key_EstadosVisualizaBotones).IndexOf(estado, StringComparison.Ordinal) != -1)
                    {
                        imgAnular.Visible = true;
                        imgaprobar.Visible = true;
                    }
                    else
                    {
                        imgAnular.Visible = false;
                        imgaprobar.Visible = false;
                        imgValidar.Visible = false;
                    }
                    if (estado == ReadKeySettings.Key_EstadoPendValidacion)
                    {
                        imgValidar.Visible = false;
                        imgAnular.Visible = false;
                    }
                }
                else if (rol == "V")
                {
                    if (Convert.ToString(ReadKeySettings.key_EstadosVisualizaBotones).IndexOf(estado, StringComparison.Ordinal) != -1)
                    {
                        imgAnular.Visible = true;
                        imgValidar.Visible = true;
                    }
                    else
                    {
                        imgAnular.Visible = false;
                        imgaprobar.Visible = false;
                        imgValidar.Visible = false;
                    }
                    if (estado == ReadKeySettings.Key_CodEstadoPendAprobacion)
                    {

                        imgaprobar.Visible = false;
                        imgAnular.Visible = false;
                    }
                }

                else
                {
                    imgAnular.Visible = false;
                    imgaprobar.Visible = false;
                    imgValidar.Visible = false;

                }

                if (estado == ReadKeySettings.Key_EstadoPendValidacion)
                {
                    imgaprobar.Visible = false;

                }
                else if (estado == ReadKeySettings.Key_CodEstadoPendAprobacion)
                {
                    imgValidar.Visible = false;
                }
            }
        }

        protected void gvPool_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPool.PageIndex = e.NewPageIndex;
            gvPool.DataSource = CurrentPage;
            gvPool.DataBind();
        }
    }
}