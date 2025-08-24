using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Business;
using Claro.SISACT.Entity;
using System.Collections;
using System.Data;
using System.Web.Services;
using Claro.SISACT.Web.Base;

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_direccion : Sisact_Webbase
    {
        #region [Declaracion de Constantes - Config]

        string consTipoProductoDTH = ConfigurationManager.AppSettings["consTipoProductoDTH"].ToString();
        string consTipoProductoHFC = ConfigurationManager.AppSettings["consTipoProducto3Play"].ToString();
        string consTipoProducto3Play_I = ConfigurationManager.AppSettings["constTipoProducto3PlayInalam"].ToString();

        #endregion [Declaracion de Constantes - Config]

        #region "Propiedades"

        public string pTituloDireccion
        {
            set { this.lbltitulodireccion.Text = value; }
        }
        public string pIdprefijo
        {
            get { return this.ddlPrefijo.SelectedValue; }
        }

        public string pTxtprefijo
        {
            get { return this.ddlPrefijo.SelectedItem.ToString(); }
        }

        public string pTxtdireccion
        {
            get { return this.txtDireccion.Text; }
        }
        public string pTxtnropuerta
        {
            get { return this.txtNroPuerta.Text; }
        }
        public string pddlEdificacion
        {
            get { return this.ddlEdificacion.SelectedValue; }
        }
        public string ptxtManzana
        {
            get { return this.txtManzana.Text; }
        }
        public string ptxtLote
        {
            get { return this.txtLote.Text; }
        }
        public string pddlTipoInterior
        {
            get { return this.ddlTipoInterior.SelectedValue; }
        }

        public string ptxtNroInterior
        {
            get { return this.txtNroInterior.Text; }
        }
        public string plblContadorDireccion
        {
            get { return this.lblContadorDireccion.Text; }
        }
        public string pddlUrbanizacion
        {
            get { return this.ddlUrbanizacion.SelectedValue; }
        }
        public string ptxtUrbanizacion
        {
            get { return this.txtUrbanizacion.Text; }
        }
        public string pddlZona
        {
            get { return this.ddlZona.SelectedValue; }
        }
        public string ptxtNombreZona
        {
            get { return this.txtNombreZona.Text; }
        }
        public string ptxtReferencia
        {
            get { return this.txtReferencia.Text; }
        }
        public string plblContadorReferencia
        {
            get { return this.lblContadorReferencia.Text; }
        }

        public string pddlDepartamento
        {
            get { return this.ddlDepartamento.SelectedValue; }
        }
        public string pddlProvincia
        {
            get { return this.ddlProvincia.SelectedValue; }
        }
        public string pddlDistrito
        {
            get { return this.ddlDistrito.SelectedValue; }
        }
        public string ptxtCodigoPostal
        {
            get { return this.txtCodigoPostal.Text; }
        }

        public string ptxtTituloTelefonoReferencia
        {
            get { return this.lblTelfRef.Text; }
            set { this.lblTelfRef.Text = value; }
        }
        public string ptxtTelefonoReferencia
        {
            get { return this.txtTelefonoReferencia.Text; }
        }
        public string ptxtPrefijoTelefonoRef
        {
            get { return this.txtPrefijoTelefonoReferencia.Text; }
        }

        public string pShowRUC
        {
            set { this.hidShowRUC.Value = value; }
        }
        //Public ReadOnly Property ptxtNumeroRUC() As String
        //    Get
        //        Return Me.txtRUCEmpleador.Value
        //    End Get

        //End Property
        //Public ReadOnly Property ptxtNombreEmpleador() As String
        //    Get
        //        Return Me.txtNombreEmpresa.Value
        //    End Get
        //End Property
        //gaa20120206'
        public int txtReferencia_MaxLength
        {
            get { return txtReferencia.MaxLength; }
            set { txtReferencia.MaxLength = value; }
        }
        //fin gaa20120206'
        //gaa20120214
        public bool TipoDomicilioVisibilidad
        {
            get { return ddlTipoDomicilio.Visible; }
            set
            {
                lblTipoDomicilio.Visible = value;
                ddlTipoDomicilio.Visible = value;
            }
        }
        //Public Property Ref_Secundaria_Visibilidad() As Boolean
        //    Get
        //        Return txtRefSec.Visible
        //    End Get
        //    Set(ByVal Value As Boolean)
        //        lblRefSec.Visible = Value
        //        txtRefSec.Visible = Value
        //        lblContRefSec.Visible = Value
        //    End Set
        //End Property
        //fin gaa20120214

        //ddlEdificacion   'txtManzana   'txtLote   'ddlTipoInterior   'txtNroInterior    'lblContadorDireccion
        //ddlUrbanizacion  'txtUrbanizacion  'ddlZona   'txtNombreZona   'txtReferencia   'lblContadorReferencia
        //ddlDepartamento   'ddlProvincia   'ddlDistrito   'txtCodigoPostal   'txtTelefonoReferencia
        #endregion

        private void Page_Load(System.Object sender, System.EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (Session["Usuario"] == null)
            {
                string strRutaSite = ConfigurationManager.AppSettings["RutaSite"];
                Response.Redirect(strRutaSite);
                Response.End();
                return;
            }

            btnAceptar.Attributes.Add("onclick", "return validarRetorno();");
            if (!IsPostBack)
            {
               
                hidTipoProducto.Value = Request.QueryString["tipoProducto"];
                hidFlgReadOnly.Value = Request.QueryString["flgReadOnly"];  // S=Solo Lectura
                hidIdFila.Value = Request.QueryString["idFila"];
                hidVentaProactiva.Value = Request.QueryString["flgVentaProactiva"];
                hidNroDocumento.Value = Request.QueryString["nroDocumento"];

                int nroPlanes = Funciones.CheckInt(Request.QueryString["nroPlanes"]);
                Int64 nroSEC = Funciones.CheckInt64(Request.QueryString["nroSEC"]);
                GeneradorLog objLog = new GeneradorLog(CurrentUser, hidNroDocumento.Value, null, "WEB");//PROY-140690
                inicio();

                //PROY-140690 - INICIO
                objLog.CrearArchivolog("[Inicio][!IsPostBack]", null, null);
                string tipoDocumento = Funciones.CheckStr(Request.QueryString["tipoDocumento"]);
                string codigoTipoProductoActual = Funciones.CheckStr(Request.QueryString["codigoTipoProductoActual"]);
                hidTipoProductoActual.Value = codigoTipoProductoActual;
				objLog.CrearArchivolog("[PROY-140690][Nro Documento] => ", Funciones.CheckStr(hidNroDocumento.Value), null);
                objLog.CrearArchivolog("[PROY-140690][tipoDocumento] => ", tipoDocumento, null);
                objLog.CrearArchivolog("[PROY-140690][codigoTipoProductoActual] => ", codigoTipoProductoActual, null);

				//INICIO INICIATIVA-932
                if (codigoTipoProductoActual == Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoInterInalam"]))
                {
                    BLGeneral objConsulta = new BLGeneral();
                    Int64 Key_MovilidadIFI = Funciones.CheckInt(ConfigurationManager.AppSettings["Key_MovilidadIFI"]);
                    List<BEParametro> ListParanGrupoMovilidadIFI = objConsulta.ListaParametrosGrupo(Key_MovilidadIFI);

                    if (ListParanGrupoMovilidadIFI != null && ListParanGrupoMovilidadIFI.Count > 0)
                    {
                        hidFlagIFI.Value = ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_FlagGeneralCobertura").ToList().Count > 0 ?
                             Funciones.CheckStr(ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_FlagGeneralCobertura").ToList()[0].Valor) : "";
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-932][sisact_direccion][Page_Load()] hidFlagIFI.Value", Funciones.CheckStr(hidFlagIFI.Value)), null, null);
                    }
                }

                if (Funciones.CheckStr(hidFlagIFI.Value) == "1")
                {
                	hidTieneDireccion.Value = string.Empty;
				}
				//FIN INICIATIVA-932

                if (codigoTipoProductoActual == Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoInterInalam"]))
                {
                    Page.Title = "Dirección de uso del servicio IFI";
                    string codRpta = "";
                    string msgRpta = "";
                    string strSecAnteriorIfi = new BLSolicitudNegocios().ConsultarSolDireccionIfi(tipoDocumento, Funciones.CheckStr(hidNroDocumento.Value), ref codRpta, ref msgRpta);
                    objLog.CrearArchivolog("[PROY-140690][Respuesta ConsultarSolDireccionIfi()][strSecAnteriorIfi] => ", Funciones.CheckStr(strSecAnteriorIfi), null);
					objLog.CrearArchivolog("[PROY-140690][Respuesta ConsultarSolDireccionIfi()][codRpta] => ", codRpta, null);
                    objLog.CrearArchivolog("[PROY-140690][Respuesta ConsultarSolDireccionIfi()][msgRpta] => ", msgRpta, null);

                    if (strSecAnteriorIfi == "")
                    {
                        hidNroSEC.Value = nroSEC.ToString();
                        //Cargar Info Dirección
                        CargarDatos(nroSEC, nroPlanes);
                    }
                    else
                    {
                        hidNroSEC.Value = strSecAnteriorIfi;
                        CargarDatosSecAnteriorIfi(Funciones.CheckInt64(strSecAnteriorIfi));
                    }
                    

                }//PROY-140690 - FIN
                else
                {
                    hidNroSEC.Value = nroSEC.ToString();
                    //Cargar Info Dirección
                    CargarDatos(nroSEC, nroPlanes);
                }                
            }
        }

        #region "Funciones"

        public void inicio()
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, hidNroDocumento.Value, null, "WEB");
            objLog.CrearArchivolog("[Inicio][Inicio]", null, null);
            string nroDoc = hidNroDocumento.Value;

            string codigoTipoProductoActual = Funciones.CheckStr(Request.QueryString["codigoTipoProductoActual"]);
           
            objLog.CrearArchivolog("[COD_PROD1]", codigoTipoProductoActual.ToString(), null);
            hidSinNumero.Value = ConfigurationManager.AppSettings["constSinNumero"];
            if (string.IsNullOrEmpty(hidSinNumero.Value))
                hidSinNumero.Value = "S/N";

            ArrayList lista;
            Hashtable hasListas = new Hashtable();
            int[] tablas = {
		                    4,
                            5,
		                    12,
		                    13,
		                    14,
		                    15,
		                    16
	                    };

            BLMaestro oConsulta = new BLMaestro();
            hasListas = oConsulta.ListarItemsGenericos(tablas);

            lista = (ArrayList)hasListas[4];
            Comunes.LlenaCombo(lista, ddlPrefijo, "CODIGO2", "DESCRIPCION2", false, true, "--Seleccione--");

            //tipo edificacion
            lista = new ArrayList();
            lista = (ArrayList)hasListas[13];
            Comunes.LlenaCombo(lista, ddlEdificacion, "Codigo", "Descripcion2", false, true, "--SEL--");

            //tipo interior
            lista = new ArrayList();
            lista = (ArrayList)hasListas[12];
            Comunes.LlenaCombo(lista, ddlTipoInterior, "Codigo", "Descripcion2", false, true, "");

            //Lista Urbanizacion
            lista = new ArrayList();
            lista = (ArrayList)hasListas[14];
            Comunes.LlenaCombo(lista, ddlUrbanizacion, "Codigo", "Descripcion2", false, true, "");

            //Tipo Zona
            lista = new ArrayList();
            lista = (ArrayList)hasListas[15];
            Comunes.LlenaCombo(lista, ddlZona, "Codigo", "Descripcion2", false, true, "--SEL--");

            //Tipo Domicilio
            lista = new ArrayList();
            lista = (ArrayList)hasListas[16];
            Comunes.LlenaCombo(lista, ddlTipoDomicilio, "Codigo", "Descripcion", false, true, "--SEL--");

            ///'''''''''''''''''''''''''''''''
            ArrayList provincias;
            ArrayList distritos;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbCodigoPostal = new StringBuilder();
            StringBuilder sbAlmacenDistrito = new StringBuilder();
            StringBuilder sbUbigeo = new StringBuilder();
            StringBuilder sbUbigeoINEI = new StringBuilder();
            //cadena para provincia
            int total = 0;
            int i = 0;
            string linea = null;

            provincias = oConsulta.ListaProvincia("000", "00", "A");
            distritos = oConsulta.ListaDistrito("0000", "000", "00", "A1");
            oConsulta = null;
            total = provincias.Count - 1;

            hidDptoDefault.Value = ConfigurationManager.AppSettings["DptoDefault"];
            // "01"
            hidProvinciaDefault.Value = ConfigurationManager.AppSettings["ProvinciaDefault"];
            // "127"

            hidDptoId.Value = hidDptoDefault.Value;
            ddlProvincia.Items.Add(new ListItem("--Seleccione--", ""));
            ddlDistrito.Items.Add(new ListItem("--Seleccione--", ""));
            ddlCentroPoblado.Items.Add(new ListItem("--Seleccione--", ""));

            string dpto_id = "";
            string provincia_id = "";
            string provincia_des = "";

            string distrito_id = "";
            string distrito_des = "";

            for (i = 0; i <= total; i++)
            {
                BEProvincia item = (BEProvincia)provincias[i];
                dpto_id = item.DEPAC_CODIGO;
                provincia_id = item.PROVC_CODIGO;
                provincia_des = item.PROVV_DESCRIPCION;
                if (i == total)
                {
                    linea = string.Format("{0};{1};{2}", provincia_id, provincia_des, dpto_id);
                }
                else
                {
                    linea = string.Format("{0};{1};{2}|", provincia_id, provincia_des, dpto_id);
                }
                if (hidDptoId.Value == dpto_id)
                {
                    ddlProvincia.Items.Add(new ListItem(provincia_des, provincia_id));
                    if (hidProvinciaDefault.Value == provincia_id)
                    {
                        ddlProvincia.Items[ddlProvincia.Items.Count - 1].Selected = true;
                    }
                }
                sb.Append(linea);
            }
            hidProvincias.Value = sb.ToString();

            //cadena para distrito
            total = distritos.Count - 1;
            string Ubigeo = string.Empty;
            sb = new StringBuilder();
            for (i = 0; i <= total; i++)
            {
                BEDistrito item = (BEDistrito)distritos[i];
                string codigoPostal = item.DISTC_CODIGO_POSTAL;
                distrito_id = item.DISTC_CODIGO;
                distrito_des = item.DISTV_DESCRIPCION;
                provincia_id = item.PROVC_CODIGO;
                dpto_id = item.DEPAC_CODIGO;
                string ubigeoINEI = item.UBIGEO_INEI;
                string almacenId = item.ALMACEN;
                if (i == total)
                {
                    linea = string.Format("{0};{1};{2};{3};{4}", distrito_id, distrito_des, provincia_id, codigoPostal, ubigeoINEI);
                }
                else
                {
                    linea = string.Format("{0};{1};{2};{3};{4}|", distrito_id, distrito_des, provincia_id, codigoPostal, ubigeoINEI);
                }
                if (hidProvinciaDefault.Value == provincia_id)
                {
                    ddlDistrito.Items.Add(new ListItem(distrito_des, distrito_id));
                    ddlDistrito.Items[ddlDistrito.Items.Count - 1].Attributes.Add("id", codigoPostal);
                    sbUbigeo.AppendFormat("{0}{1}{2};{3}|", dpto_id, provincia_id, distrito_id, distrito_id);
                    if (!string.IsNullOrEmpty(codigoPostal))
                        sbCodigoPostal.AppendFormat("{0};{1}|", distrito_id, codigoPostal);
                }
                sbUbigeoINEI.AppendFormat("{0};{1}|", distrito_id, ubigeoINEI);
                sbAlmacenDistrito.AppendFormat("{0};{1}|", distrito_id, almacenId);
                sb.Append(linea);
            }
            hidDistritos.Value = sb.ToString();
            hidListaCodigoPostal.Value = sbCodigoPostal.ToString();
            hidListUbigeo.Value = sbUbigeo.ToString();
            hidListUbigeoINEI.Value = sbUbigeoINEI.ToString();

            //Departamento
            lista = (ArrayList)hasListas[5];
            lista.Insert(0, new BEDepartamento("-1", "--Seleccione--", ""));
            var _with1 = ddlDepartamento;
            _with1.DataSource = lista;
            _with1.DataValueField = "DEPAC_CODIGO";
            _with1.DataTextField = "DEPAV_DESCRIPCION";
            _with1.DataBind();

            try
            {
                ddlDepartamento.SelectedValue = hidDptoId.Value;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][Inicio]", null, ex);
            }

            hidProvinciaId.Value = ddlProvincia.SelectedValue;
            objLog.CrearArchivolog("[Fin][Inicio]", null, null);
        }

        //PROY-24740
        public void llenarCboProvincias(string IdDepartamento)
        {
            ArrayList oLista = new ArrayList();
            string[] ostr = hidProvincias.Value.Split('|');

            oLista.AddRange(ostr.Select(s => s.Split(';'))
                          .Select(c => new BEProvincia()
            {
                              PROVC_CODIGO = c[0],
                              PROVV_DESCRIPCION = c[1],
                              DEPAC_CODIGO = c[2],
                          }).Where(w => w.DEPAC_CODIGO == IdDepartamento).ToList());

            ddlProvincia.DataSource = oLista;
            ddlProvincia.DataTextField = "PROVV_DESCRIPCION";
            ddlProvincia.DataValueField = "PROVC_CODIGO";
            ddlProvincia.DataBind();
        }

        //PROY-24740
        public void llenarCboDistritos(string IdProvincia)
        {
            ArrayList oLista = new ArrayList();
            string[] ostr = hidDistritos.Value.Split('|');

            oLista.AddRange(ostr.Select(s => s.Split(';'))
                           .Select(c => new BEDistrito()
                {
                               DISTC_CODIGO = c[0],
                               DISTV_DESCRIPCION = c[1],
                               PROVC_CODIGO = c[2],
                               UBIGEO_INEI = c[3]
                           }).Where(w => w.PROVC_CODIGO == IdProvincia).ToList());

            ddlDistrito.DataSource = oLista;
            ddlDistrito.DataTextField = "DISTV_DESCRIPCION";
            ddlDistrito.DataValueField = "DISTC_CODIGO";
            ddlDistrito.DataBind();
        }

        public void llenarCboCentroPoblado(string IdDistrito)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, hidNroDocumento.Value,null,"WEB");
            objLog.CrearArchivolog("[Inicio][llenarCboCentroPoblado]",null,null);
            objLog.CrearArchivolog("ID_DISTRITO", IdDistrito.ToString(), null);

            string idTipoProducto = hidTipoProducto.Value;
            int ichkDTH;
            int ichkLTE;
           
            if (chkDTH.Checked)
            {
                ichkDTH = 1;
            }
            else
            {
                ichkDTH = 0;
            }
            if (chkLTE.Checked)
            {
                ichkLTE = 1;
            }
            else
            {
                ichkLTE = 0;
            }

            BLIntegracionDTHNegocio oIntegracioDTHNegocio = new BLIntegracionDTHNegocio();
            ArrayList lista = new ArrayList();
            if (idTipoProducto == consTipoProducto3Play_I)
            {
                objLog.CrearArchivolog("[ID_DISTRITO]", IdDistrito.ToString(), null);
                objLog.CrearArchivolog("[CHK_DTH]", ichkDTH.ToString(), null);
                objLog.CrearArchivolog("[CHK_LTE]", ichkLTE.ToString(), null);
                lista = oIntegracioDTHNegocio.ListarCentrosPobladosDistrito_LTE(obtenerUbigeoInei(IdDistrito), ichkDTH, ichkLTE);
            }
            else
            {                
             lista = oIntegracioDTHNegocio.ListarCentrosPobladosDistrito(obtenerUbigeoInei(IdDistrito));
            }

            DataTable dtDatos = new DataTable();
            dtDatos.Columns.Add("Id");
            dtDatos.Columns.Add("Descripcion");
            DataRow dr = default(DataRow);
            if (lista.Count > 0)
            {
                for (int i = 0; i <= lista.Count - 1; i++)
                {
                    dr = dtDatos.NewRow();
                    BECentroPoblado oCentroPoblado = (BECentroPoblado)lista[i];
                    dr[0] = oCentroPoblado.IDPOBLADO + "-" + oCentroPoblado.COBERTURA;
                    dr[1] = oCentroPoblado.NOMBRE;
                    dtDatos.Rows.Add(dr);
                }

                ddlCentroPoblado.DataSource = dtDatos;
                ddlCentroPoblado.DataTextField = "Descripcion";
                ddlCentroPoblado.DataValueField = "Id";
                ddlCentroPoblado.DataBind();

                objLog.CrearArchivolog("    llenarCboCentroPoblado/SALIDA   ", IdDistrito.ToString(), null);
            }
        }

        //PROY-24740
        public string obtenerUbigeoInei(string IdDistrito)
        {
            if (string.IsNullOrEmpty(IdDistrito))
                return string.Empty;
                       
            if (string.IsNullOrEmpty(hidListUbigeoINEI.Value))
                return string.Empty;

            string[] ostrLista = hidListUbigeoINEI.Value.Split('|');

            var strUbigeoInei = ostrLista.Select(s => s.Split(';')).Select(c => new string[] { c[0], c[1] }).Where(w => w[0] == IdDistrito).FirstOrDefault();

            return strUbigeoInei == null ? string.Empty : strUbigeoInei[1];
        }

        #endregion

        public void CargarDatos(Int64 nroSEC, int nroPlanes)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, hidNroDocumento.Value,null,"WEB");
            objLog.CrearArchivolog("[Inicio][CargarDatos]",null,null);
            objLog.CrearArchivolog("SEC", nroSEC.ToString(), null);
            objLog.CrearArchivolog("PLAN", nroPlanes.ToString(), null);

            BEDireccionCliente oDireccion = null;
            string idTipoProducto = hidTipoProducto.Value;
            string nroDocumento = hidNroDocumento.Value;
            int idFila = Funciones.CheckInt(hidIdFila.Value);

            if (nroSEC > 0)
            {
                objLog.CrearArchivolog("[CONSULTA_SOL_DIREC]", nroSEC.ToString(), null);
                List<BEDireccionCliente> arrDireccion = new BLSolicitudNegocios().ConsultarSolDireccion(nroSEC);
                foreach (BEDireccionCliente item in arrDireccion)
                {
                    if (item.IdTipoDireccion == "I")
                    {
                        oDireccion = new BEDireccionCliente();
                        oDireccion = item;
                        break;
                    }
                }

                objLog.CrearArchivolog("[TIPO_PROD]", hidTipoProducto.Value, null);
                hidTipoProducto.Value = oDireccion.IdTipoProducto;
                idTipoProducto = hidTipoProducto.Value;

                //validar edicion de direccion 

                // Consulta Datos Solicitud
                DataRow drSolicitud = null;
                BLSolicitud objSolicitud = new BLSolicitud();
                drSolicitud = objSolicitud.ObtenerSolicitudPersona(nroSEC).Rows[0];
                string codOficina = drSolicitud["OVENC_CODIGO"].ToString();
                BEUsuarioSession objUsuario = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];
                if (objUsuario.OficinaVenta == codOficina)
                {
                    List<BEEstado> listEstodosSot = (new BLSolicitud()).ObtenerHistoricoEstadosSOT(nroSEC);
                    if (listEstodosSot.Count > 0)
                    {
                        Int64 nroSot = ((BEEstado)listEstodosSot[0]).NroSOT;
                        if (listEstodosSot.Count > 0 && nroSot > 0)
                        {
                            hidNroSot.Value = nroSot.ToString();
                            objLog.CrearArchivolog("[SOT]", nroSot.ToString(), null);
                            listEstodosSot = (new BLSolicitud()).ObtenerEstadoSot(nroSEC, nroSot);
                            string strEstadoSot = ((BEEstado)listEstodosSot[0]).ESTAC_CODIGO;
                            string cadenaEstadosSot = ConfigurationManager.AppSettings["constCodEstadosSot_EditarDireccionInst"];
                            if (cadenaEstadosSot.Contains(strEstadoSot))
                            {
                                hidEditarDirecion.Value = "S";
                            }
                        }
                    }
                    else
                    {
                        hidEditarDirecion.Value = "S";
                    }
                }
                
            }
            else
            {
                List<BEDireccionCliente> objLista = (List<BEDireccionCliente>)Session["objDireccion" + nroDocumento];
                if (objLista != null)
                {
                    foreach (BEDireccionCliente obj in objLista)
                    {
                        if (obj.IdTipoProducto == idTipoProducto && obj.IdFila == idFila)
                        {
                            oDireccion = new BEDireccionCliente();
                            oDireccion = (BEDireccionCliente)obj;
                            break;
                        }
                    }
                }
            }

            if (oDireccion != null)
            {
                ddlPrefijo.SelectedValue = oDireccion.IdPrefijo;
                txtDireccion.Text = oDireccion.Direccion;
                txtNroPuerta.Text = oDireccion.NroPuerta;
                if (string.IsNullOrEmpty(oDireccion.IdEdificacion))
                {
                    oDireccion.IdEdificacion = "-1";
                }
                ddlEdificacion.SelectedValue = oDireccion.IdEdificacion;
                txtManzana.Text = oDireccion.Manzana;
                txtLote.Text = oDireccion.Lote;
                if (string.IsNullOrEmpty(oDireccion.IdTipoInterior))
                {
                    oDireccion.IdTipoInterior = "-1";
                }
                ddlTipoInterior.SelectedValue = oDireccion.IdTipoInterior;
                txtNroInterior.Text = oDireccion.NroInterior;
                if (string.IsNullOrEmpty(oDireccion.IdUrbanizacion))
                {
                    oDireccion.IdUrbanizacion = "-1";
                }
                ddlUrbanizacion.SelectedValue = oDireccion.IdUrbanizacion;
                txtUrbanizacion.Text = oDireccion.TxtUrbanizacion;
                if (string.IsNullOrEmpty(oDireccion.IdDomicilio))
                {
                    oDireccion.IdDomicilio = "-1";
                }
                ddlTipoDomicilio.SelectedValue = oDireccion.IdDomicilio;
                if (string.IsNullOrEmpty(oDireccion.IdZona))
                {
                    oDireccion.IdZona = "-1";
                }
                ddlZona.SelectedValue = oDireccion.IdZona;
                txtNombreZona.Text = oDireccion.NombreZona;
                txtReferencia.Text = oDireccion.Referencia;
                ddlDepartamento.SelectedValue = oDireccion.IdDepartamento;

                llenarCboProvincias(oDireccion.IdDepartamento);
                ddlProvincia.SelectedValue = oDireccion.IdProvincia;
                hidProvinciaId.Value = oDireccion.IdProvincia;

                llenarCboDistritos(oDireccion.IdProvincia);
                ddlDistrito.SelectedValue = oDireccion.IdDistrito;
                hidDistritoId.Value = oDireccion.IdDistrito;

                txtCodigoPostal.Text = oDireccion.IdPostal;
                txtUbigeo.Text = oDireccion.IdUbigeo;
                txtPrefijoTelefonoReferencia.Text = oDireccion.IdTelefono;
                txtTelefonoReferencia.Text = oDireccion.Telefono;

                if (idTipoProducto == consTipoProductoDTH)
                {
                    llenarCboCentroPoblado(oDireccion.IdDistrito);
                    ddlCentroPoblado.SelectedValue = oDireccion.IdCentroPoblado;
                    hidCentroPoblado.Value = oDireccion.IdCentroPoblado + "_" + oDireccion.IdUbigeoSGA;
                }

                txtCodPlano.Text = oDireccion.IdPlano;
                txtRefSecundaria.Text = oDireccion.Referencia_Sec;

                //DTH
                if ((idTipoProducto == consTipoProductoDTH || idTipoProducto == consTipoProducto3Play_I) & (hidVentaProactiva.Value == "S" || nroSEC > 0))
                {
                    chkVentaProactiva.Checked = (oDireccion.VentaProactiva == "S");
                    txtVendedorDNI.Text = oDireccion.DniVendedor;
                    chkVtaProgramada.Checked = (oDireccion.VentaProgramada == "S" ? true : false);
                }

                hidFlagVOD.Value = oDireccion.FlagVOD;
                txtCodEdificio.Text = oDireccion.IdEdificio;

                //HFC
                if (idTipoProducto == consTipoProductoHFC)
                {
                    chkVentaProactiva.Checked = (oDireccion.VentaProactiva == "S");
                    txtVendedorDNI.Text = oDireccion.DniVendedor;
                }

                //3PLAY INALAMBRICO
                if (idTipoProducto == consTipoProducto3Play_I)
                {
                    chkDTH.Checked = (oDireccion.Cobertura_dth == 1);
                    chkLTE.Checked = (oDireccion.Cobertura_lte == 1);

                    llenarCboCentroPoblado(oDireccion.IdDistrito);
                    ddlCentroPoblado.SelectedValue = oDireccion.IdCentroPoblado;
                    hidCentroPoblado.Value = oDireccion.IdCentroPoblado + "_" + oDireccion.IdUbigeoSGA;
                }

				//INICIO INICIATIVA-932
                if (Funciones.CheckStr(hidFlagIFI.Value) == "1")
                {
                	hidTieneDireccion.Value = "0";
				}
				//FIN INICIATIVA-932

                objLog.CrearArchivolog("[Fin][CargarDatos]", null, null);
            }
            //PROY-32581 - INICIO
            else
            {
                if (Session["objDireccion" + nroDocumento] != null)
                {
                    List<BEDireccionCliente> objLista = (List<BEDireccionCliente>)Session["objDireccion" + nroDocumento];
                    if (objLista != null)
                    {
                        oDireccion = objLista.FirstOrDefault();
                    }

                    if (oDireccion != null)
                    {
                        ddlPrefijo.SelectedValue = oDireccion.IdPrefijo;
                        txtDireccion.Text = oDireccion.Direccion;
                        txtNroPuerta.Text = oDireccion.NroPuerta;
                        if (string.IsNullOrEmpty(oDireccion.IdEdificacion))
                        {
                            oDireccion.IdEdificacion = "-1";
                        }
                        ddlEdificacion.SelectedValue = oDireccion.IdEdificacion;
                        txtManzana.Text = oDireccion.Manzana;
                        txtLote.Text = oDireccion.Lote;
                        if (string.IsNullOrEmpty(oDireccion.IdTipoInterior))
                        {
                            oDireccion.IdTipoInterior = "-1";
                        }
                        ddlTipoInterior.SelectedValue = oDireccion.IdTipoInterior;
                        txtNroInterior.Text = oDireccion.NroInterior;
                        if (string.IsNullOrEmpty(oDireccion.IdUrbanizacion))
                        {
                            oDireccion.IdUrbanizacion = "-1";
                        }
                        ddlUrbanizacion.SelectedValue = oDireccion.IdUrbanizacion;
                        txtUrbanizacion.Text = oDireccion.TxtUrbanizacion;
                        if (string.IsNullOrEmpty(oDireccion.IdDomicilio))
                        {
                            oDireccion.IdDomicilio = "-1";
                        }
                        ddlTipoDomicilio.SelectedValue = oDireccion.IdDomicilio;
                        if (string.IsNullOrEmpty(oDireccion.IdZona))
                        {
                            oDireccion.IdZona = "-1";
                        }
                        ddlZona.SelectedValue = oDireccion.IdZona;
                        txtNombreZona.Text = oDireccion.NombreZona;
                        txtReferencia.Text = oDireccion.Referencia;
                        ddlDepartamento.SelectedValue = oDireccion.IdDepartamento;

                        llenarCboProvincias(oDireccion.IdDepartamento);
                        ddlProvincia.SelectedValue = oDireccion.IdProvincia;
                        hidProvinciaId.Value = oDireccion.IdProvincia;

                        llenarCboDistritos(oDireccion.IdProvincia);
                        ddlDistrito.SelectedValue = oDireccion.IdDistrito;
                        hidDistritoId.Value = oDireccion.IdDistrito;

                        txtCodigoPostal.Text = oDireccion.IdPostal;
                        txtUbigeo.Text = oDireccion.IdUbigeo;
                        txtPrefijoTelefonoReferencia.Text = oDireccion.IdTelefono;
                        txtTelefonoReferencia.Text = oDireccion.Telefono;

                        if (idTipoProducto == consTipoProductoDTH)
                        {
                            llenarCboCentroPoblado(oDireccion.IdDistrito);
                            ddlCentroPoblado.SelectedValue = oDireccion.IdCentroPoblado;
                            hidCentroPoblado.Value = oDireccion.IdCentroPoblado + "_" + oDireccion.IdUbigeoSGA;
                        }

                        txtCodPlano.Text = oDireccion.IdPlano;
                        txtRefSecundaria.Text = oDireccion.Referencia_Sec;

                        //DTH
                        if ((idTipoProducto == consTipoProductoDTH || idTipoProducto == consTipoProducto3Play_I) & (hidVentaProactiva.Value == "S" || nroSEC > 0))
                        {
                            chkVentaProactiva.Checked = (oDireccion.VentaProactiva == "S");
                            txtVendedorDNI.Text = oDireccion.DniVendedor;
                            chkVtaProgramada.Checked = (oDireccion.VentaProgramada == "S" ? true : false);
                        }

                        hidFlagVOD.Value = oDireccion.FlagVOD;
                        txtCodEdificio.Text = oDireccion.IdEdificio;

                        //HFC
                        if (idTipoProducto == consTipoProductoHFC)
                        {
                            chkVentaProactiva.Checked = (oDireccion.VentaProactiva == "S");
                            txtVendedorDNI.Text = oDireccion.DniVendedor;
                        }

                        //3PLAY INALAMBRICO
                        if (idTipoProducto == consTipoProducto3Play_I)
                        {
                            chkDTH.Checked = (oDireccion.Cobertura_dth == 1);
                            chkLTE.Checked = (oDireccion.Cobertura_lte == 1);

                            llenarCboCentroPoblado(oDireccion.IdDistrito);
                            ddlCentroPoblado.SelectedValue = oDireccion.IdCentroPoblado;
                            hidCentroPoblado.Value = oDireccion.IdCentroPoblado + "_" + oDireccion.IdUbigeoSGA;
                        }

						//INICIO INICIATIVA-932
                        if (Funciones.CheckStr(hidFlagIFI.Value) == "1")
                        {
		                	hidTieneDireccion.Value = "0";
						}
						//FIN INICIATIVA-932

                        objLog.CrearArchivolog("[Fin][CargarDatos]", null, null);
                    }
                }     
            }
            //PROY-32581 - FIN
        }

        //PROY-24740
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, hidNroDocumento.Value, null, "WEB");
            objLog.CrearArchivolog("[Inicio][btnAceptar_Click]", null, null);

            string nroDocumento = hidNroDocumento.Value;
            string idTipoProducto = hidTipoProducto.Value;

			//INICIO INICIATIVA-932
			string valorDpto = string.Empty;
            if (Funciones.CheckStr(hidFlagIFI.Value) == "1" && hidTipoProductoActual.Value == Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoInterInalam"]))
            {
		    	valorDpto = ddlDepartamento.SelectedValue == "-1" ? hidDptoId.Value : ddlDepartamento.SelectedValue;	            
			}
			else
	        {
	        	valorDpto = ddlDepartamento.SelectedValue;
	        }
			//FIN INICIATIVA-932

            if (hidFlgReadOnly.Value == "N" || hidEditarDirecion.Value == "S")
            {
                objLog.CrearArchivolog("###########INGRESO 1#########", null, null);

                txtDireccion.Text = Request.Form[txtDireccion.UniqueID];
                txtNroPuerta.Text = Request.Form[txtNroPuerta.UniqueID];
                txtManzana.Text = Request.Form[txtManzana.UniqueID];
                txtLote.Text = Request.Form[txtLote.UniqueID];
                txtNroInterior.Text = Request.Form[txtNroInterior.UniqueID];
                txtUrbanizacion.Text = Request.Form[txtUrbanizacion.UniqueID];
                txtNombreZona.Text = Request.Form[txtNombreZona.UniqueID];
                txtCodigoPostal.Text = Request.Form[txtCodigoPostal.UniqueID];
                txtUbigeo.Text = Request.Form[txtUbigeo.UniqueID];
                txtCodPlano.Text = Request.Form[txtCodPlano.UniqueID];
                txtVendedorDNI.Text = Request.Form[txtVendedorDNI.UniqueID];

                string strVendedorDNI = txtVendedorDNI.Text;
                BEDireccionCliente oDireccion = new BEDireccionCliente();


                oDireccion.IdFila = Funciones.CheckInt(hidIdFila.Value);
                oDireccion.IdPrefijo = ddlPrefijo.SelectedValue;
                oDireccion.IdTipoProducto = idTipoProducto;
                oDireccion.Prefijo = ddlPrefijo.SelectedItem.Text;
                oDireccion.Direccion = txtDireccion.Text.ToUpper();
                oDireccion.NroPuerta = txtNroPuerta.Text;
                oDireccion.IdEdificacion = ddlEdificacion.SelectedValue;
                oDireccion.Edificacion = ddlEdificacion.SelectedItem.Text.ToUpper();
                oDireccion.Manzana = txtManzana.Text.ToUpper();
                oDireccion.Lote = txtLote.Text.ToUpper();
                oDireccion.IdTipoInterior = ddlTipoInterior.SelectedValue;
                oDireccion.TipoInterior = ddlTipoInterior.SelectedItem.Text.ToUpper();
                oDireccion.NroInterior = txtNroInterior.Text;
                oDireccion.IdUrbanizacion = ddlUrbanizacion.SelectedValue;
                oDireccion.Urbanizacion = ddlUrbanizacion.SelectedItem.Text.ToUpper();
                oDireccion.TxtUrbanizacion = txtUrbanizacion.Text.ToUpper();
                oDireccion.IdDomicilio = ddlTipoDomicilio.SelectedValue;
                oDireccion.Domicilio = ddlTipoDomicilio.SelectedItem.Text.ToUpper();
                oDireccion.IdZona = ddlZona.SelectedValue;
                oDireccion.Zona = ddlZona.SelectedItem.Text.ToUpper();
                oDireccion.NombreZona = txtNombreZona.Text.ToUpper();
                oDireccion.Referencia = txtReferencia.Text.ToUpper();
                oDireccion.IdDepartamento = valorDpto;
                oDireccion.IdProvincia = hidProvinciaId.Value;
                oDireccion.IdDistrito = hidDistritoId.Value;
                oDireccion.IdPostal = txtCodigoPostal.Text;
                oDireccion.IdUbigeo = txtUbigeo.Text;
                oDireccion.IdTelefono = txtPrefijoTelefonoReferencia.Text;
                oDireccion.Telefono = txtTelefonoReferencia.Text;

                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdPrefijo] => ", oDireccion.IdPrefijo, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdTipoProducto] => ", oDireccion.IdTipoProducto, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.Direccion] => ", oDireccion.Direccion, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.NroPuerta] => ", oDireccion.NroPuerta, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdEdificacion] => ", oDireccion.IdEdificacion, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.Edificacion] => ", oDireccion.Edificacion, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.Manzana] => ", oDireccion.Manzana, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.Lote] => ", oDireccion.Lote, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdTipoInterior] => ", oDireccion.IdTipoInterior, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.TipoInterior] => ", oDireccion.TipoInterior, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.NroInterior] => ", oDireccion.NroInterior, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdUrbanizacion] => ", oDireccion.IdUrbanizacion, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.Urbanizacion] => ", oDireccion.Urbanizacion, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.TxtUrbanizacion] => ", oDireccion.TxtUrbanizacion, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdDomicilio] => ", oDireccion.IdDomicilio, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.Domicilio] => ", oDireccion.Domicilio, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdZona] => ", oDireccion.IdZona, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.Zona] => ", oDireccion.Zona, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.NombreZona] => ", oDireccion.NombreZona, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.Referencia] => ", oDireccion.Referencia, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdDepartamento] => ", oDireccion.IdDepartamento, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdProvincia] => ", oDireccion.IdProvincia, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdDistrito] => ", oDireccion.IdDistrito, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdPostal] => ", oDireccion.IdPostal, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdUbigeo] => ", oDireccion.IdUbigeo, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdTelefono] => ", oDireccion.IdTelefono, null);
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.Telefono] => ", oDireccion.Telefono, null);


                if (hidCentroPoblado.Value.Length > 0)
                {
                    oDireccion.IdCentroPoblado = hidCentroPoblado.Value.Split('_')[0];
                    oDireccion.IdUbigeoSGA = hidCentroPoblado.Value.Split('_')[1];
                }
                oDireccion.IdPlano = this.txtCodPlano.Text;
                oDireccion.Referencia_Sec = txtRefSecundaria.Text.ToUpper();
                oDireccion.IdPlano = this.txtCodPlano.Text;
                oDireccion.Referencia_Sec = txtRefSecundaria.Text.ToUpper();
                oDireccion.IdEdificio = txtCodEdificio.Text.Trim().ToUpper();
                oDireccion.FlagVOD = hidFlagVOD.Value;

                if ((idTipoProducto == consTipoProductoDTH || idTipoProducto == consTipoProducto3Play_I) & hidVentaProactiva.Value == "S")
                {
                    oDireccion.VentaProactiva = (strVendedorDNI.Length > 0 ? "S" : "N");
                    oDireccion.DniVendedor = txtVendedorDNI.Text;
                    if (chkVtaProgramada.Checked)
                    {
                        oDireccion.VentaProgramada = "S";
                    }
                    else
                    {
                        oDireccion.VentaProgramada = "N";
                    }
                }

                //gaa20130918
                if (idTipoProducto == consTipoProductoHFC)
                {
                    oDireccion.VentaProactiva = (strVendedorDNI.Length == 8 ? "S" : "N");
                    oDireccion.DniVendedor = txtVendedorDNI.Text;
                    oDireccion.IdUbigeoSGA = hidUbigeoINEI.Value;
                }
                //fin gaa20130918

                StringBuilder sblDirCompleta = new StringBuilder();
                sblDirCompleta.Append(oDireccion.IdPrefijo);
                sblDirCompleta.Append(" ");
                sblDirCompleta.Append(oDireccion.Direccion);
                sblDirCompleta.Append(" ");
                sblDirCompleta.Append(oDireccion.NroPuerta);
                if (!(oDireccion.IdEdificacion == "-1"))
                {
                    sblDirCompleta.Append(" ");
                    sblDirCompleta.Append(oDireccion.IdEdificacion);
                    sblDirCompleta.Append(" ");
                    sblDirCompleta.Append(oDireccion.Manzana);
                    sblDirCompleta.Append(" LT ");
                    sblDirCompleta.Append(oDireccion.Lote);
                }
                if (!(oDireccion.IdTipoInterior == "-1"))
                {
                    sblDirCompleta.Append(" ");
                    sblDirCompleta.Append(oDireccion.IdTipoInterior);
                    sblDirCompleta.Append(" ");
                    sblDirCompleta.Append(oDireccion.NroInterior);
                }
                if (!(oDireccion.IdUrbanizacion == "-1"))
                {
                    sblDirCompleta.Append(" ");
                    sblDirCompleta.Append(oDireccion.IdUrbanizacion);
                    sblDirCompleta.Append(" ");
                    sblDirCompleta.Append(oDireccion.TxtUrbanizacion);
                }
                if (!(oDireccion.IdZona == "-1"))
                {
                    sblDirCompleta.Append(" ");
                    sblDirCompleta.Append(oDireccion.IdZona);
                    sblDirCompleta.Append(" ");
                    sblDirCompleta.Append(oDireccion.NombreZona);
                }
                oDireccion.DirCompleta = sblDirCompleta.ToString().ToUpper();

                oDireccion.FlagVOD = hidFlagVOD.Value;


                StringBuilder sblDirCompletaSAP = new StringBuilder();
                sblDirCompletaSAP.Append(pIdprefijo);
                sblDirCompletaSAP.Append(" ");
                sblDirCompletaSAP.Append(pTxtdireccion);
                sblDirCompletaSAP.Append(" ");
                sblDirCompletaSAP.Append(pTxtnropuerta);
                sblDirCompletaSAP.Append(" ");

                if (ddlEdificacion.SelectedIndex > 0)
                {
                    sblDirCompletaSAP.Append(ddlEdificacion.SelectedItem.ToString().Substring(0, ddlEdificacion.SelectedItem.ToString().IndexOf("-") - 1).ToUpper());
                    sblDirCompletaSAP.Append(" ");
                    sblDirCompletaSAP.Append(ptxtManzana.ToUpper());
                    sblDirCompletaSAP.Append(" LT ");
                    sblDirCompletaSAP.Append(ptxtLote);
                    sblDirCompletaSAP.Append(" ");
                }
                if (ddlTipoInterior.SelectedIndex > 0)
                {
                    sblDirCompletaSAP.Append(ddlTipoInterior.SelectedItem.ToString().Substring(0, ddlTipoInterior.SelectedItem.ToString().IndexOf("-") - 1).ToUpper());
                    sblDirCompletaSAP.Append(" ");
                    sblDirCompletaSAP.Append(txtNroInterior.Text);
                    sblDirCompletaSAP.Append(" ");
                }

                oDireccion.DirCompletaSAP = sblDirCompletaSAP.ToString().ToUpper();

                StringBuilder sblDirReferenciaSAP = new StringBuilder();
                if (ddlUrbanizacion.SelectedIndex > 0)
                {
                    sblDirReferenciaSAP.Append(ddlUrbanizacion.SelectedItem.ToString().Substring(0, ddlUrbanizacion.SelectedItem.ToString().IndexOf("-") - 1).ToUpper());
                    sblDirReferenciaSAP.Append(" ");
                    sblDirReferenciaSAP.Append(txtUrbanizacion.Text.ToUpper());
                    sblDirReferenciaSAP.Append(" ");
                }

                if (ddlZona.SelectedIndex > 0)
                {
                    sblDirReferenciaSAP.Append(ddlZona.SelectedItem.ToString().Substring(0, ddlZona.SelectedItem.ToString().IndexOf("-") - 1).ToUpper());
                    sblDirReferenciaSAP.Append(" ");
                    sblDirReferenciaSAP.Append(txtNombreZona.Text);
                    sblDirReferenciaSAP.Append(" ");
                }
                sblDirReferenciaSAP.Append(ptxtReferencia);
                sblDirReferenciaSAP.Append(" ");

                oDireccion.DirReferenciaSAP = sblDirReferenciaSAP.ToString().ToUpper();

                if (idTipoProducto == consTipoProducto3Play_I)
                {
                     if (chkDTH.Checked)
                    {
                        oDireccion.Cobertura_dth = 1;
                    }
                    else
                    {
                        oDireccion.Cobertura_dth = 0;
                    }
                    if (chkLTE.Checked)
                    {
                        oDireccion.Cobertura_lte = 1;
                    }
                    else
                    {
                        oDireccion.Cobertura_lte = 0;
                    }
                }

                //Guardar Session Direcciones
                bool flgExiste = false;
                List<BEDireccionCliente> objListaDir = new List<BEDireccionCliente>();
                List<BEDireccionCliente> objLista = (List<BEDireccionCliente>)Session["objDireccion" + nroDocumento];
                if (objLista != null)
                {
                    foreach (BEDireccionCliente obj in objLista)
                    {
                        if (obj.IdFila == Funciones.CheckInt(hidIdFila.Value))
                        {
                            flgExiste = true;
                            objListaDir.Add(oDireccion);
                        }
                        else
                        {
                            objListaDir.Add(obj);
                        }
                    }

                    if (!flgExiste)
                        objListaDir.Add(oDireccion);
                }
                else
                {
                    objListaDir.Add(oDireccion);
                }

                //PROY-140690 - IFI RUC
                objLog.CrearArchivolog("[btnAceptar_Click()][oDireccion.IdFila] => ", oDireccion.IdFila, null);
                if (oDireccion.IdFila != 0)
                {
                    Session["objDireccionIdFilaIFI"] = oDireccion.IdFila;                    
                }
                //PROY-140690 - IFI RUC

                Session["objDireccion" + nroDocumento] = objListaDir;

                if (hidEditarDirecion.Value == "S")
                {
                    Int64 nroSec = Funciones.CheckInt64(hidNroSEC.Value);
                    oDireccion.IdTipoDireccion = "I";
                    if (oDireccion.IdEdificacion == "-1")
                    {
                        oDireccion.IdEdificacion = "";
                        oDireccion.Edificacion = "";
                    }
                    if (oDireccion.IdTipoInterior == "-1")
                    {
                        oDireccion.IdTipoInterior = "";
                        oDireccion.TipoInterior = "";
                    }
                    if (oDireccion.IdDomicilio == "-1")
                    {
                        oDireccion.IdDomicilio = "";
                        oDireccion.Domicilio = "";
                    }
                    if (oDireccion.IdZona == "-1")
                    {
                        oDireccion.IdZona = "";
                        oDireccion.Zona = "";
                    }

                     if (idTipoProducto == consTipoProducto3Play_I)
                    {
                        new BLSolicitud().InsertarSolDireccionVenta_LTE(oDireccion, nroSec);
                    }
                    else
                    {
                    new BLSolicitud().InsertarSolDireccionVenta(oDireccion, nroSec);
                    }

                    ////
                    if (hidNroSot.Value != "")
                    {
                        List<BEDireccionCliente> listDireccion = new List<BEDireccionCliente>();
                        listDireccion = new BLSolicitudNegocios().ConsultarSolDireccion(nroSec);
                        foreach (BEDireccionCliente oDireccionInst in listDireccion)
                        {
                            if (oDireccionInst.IdTipoDireccion == "I")
                            {
                                string msgResp = "";
                                string codResp = new BLSolicitud().ActualizaDirreccionSga(Funciones.CheckInt64(hidNroSot.Value), oDireccionInst, ref msgResp);
                                break;
                            }
                        }
                    }
                }

                if (hidTipoProductoActual.Value == Funciones.CheckStr(ConfigurationManager.AppSettings["constTipoProductoInterInalam"]))
                {
                    //INICIO INICIATIVA-932
                    if (Funciones.CheckStr(hidFlagIFI.Value) == "1")
                    {
						Session["flagDireccionIFI"] = true;
					}
					//FIN INICIATIVA-932

                    List<BEParametro> objParametroMsgIfi = new BLGeneral().ListaParametrosGrupo(Funciones.CheckInt64(ConfigurationManager.AppSettings["consCodigoParametroMsgIfi"]));
                    objLog.CrearArchivolog("[btnAceptar_Click()][objParametroMsgIfi()] => ", objParametroMsgIfi.Count, null);
                    
                    string strCodigoParametroMsgIfi = Funciones.CheckStr(objParametroMsgIfi.Where(w => w.Valor1.Equals("Key_Mensaje_Grabar_Ifi")).ToList()[0].Valor);
                    objLog.CrearArchivolog("[btnAceptar_Click()][Mensaje parametro IFI] => ", strCodigoParametroMsgIfi, null);

                    string mensajeTipoIfi = "Dirección de uso del servicio IFI";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ERROR", "TipoProductoIfi('" + mensajeTipoIfi + "');alert('" + strCodigoParametroMsgIfi + "');", true);
                }                
            }

            hidRetornar.Value = "1";
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod ValidarVendedorDNI(string pstrNroDocumento)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            BLSolicitudNegocios objSolicitudNegocios;
            try
            {
                objSolicitudNegocios = new BLSolicitudNegocios();
                objResponse.Cadena = objSolicitudNegocios.ValidarVendedorDNI(pstrNroDocumento);
                objResponse.TipoRespuesta = "B";
                objResponse.Boleano = true;
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
            }
            finally
            {
                objSolicitudNegocios = null;
            }

            return objResponse;
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod ValidarVendedorDNIHFC(string pstrNroDocumento)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            BLGeneral_II objGeneral;
            try
            {
                objGeneral = new BLGeneral_II();
                if (objGeneral.ValidarDNIVendedor(pstrNroDocumento))
                {
                    objResponse.TipoRespuesta = "B";
                    objResponse.Cadena = "OK";
                    objResponse.Boleano = true;
                }
                else
                {
                    objResponse.Cadena = "0";
                    objResponse.Boleano = true;
                }
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
            }
            finally
            {
                objGeneral = null;
            }

            return objResponse;
        }

        //PROY-140690 - INICIO
        public void CargarDatosSecAnteriorIfi(Int64 nroSEC)
        {
            GeneradorLog objLog = new GeneradorLog(CurrentUser, hidNroDocumento.Value, null, "WEB");
            objLog.CrearArchivolog("[Inicio][CargarDatosSecAnteriorIfi()]", null, null);
            objLog.CrearArchivolog("[CargarDatosSecAnteriorIfi()][SEC] => ", nroSEC.ToString(), null);

            BEDireccionCliente oDireccion = null;
            string idTipoProducto = hidTipoProducto.Value;
            string nroDocumento = hidNroDocumento.Value;
            int idFila = Funciones.CheckInt(hidIdFila.Value);

            if (Session["objDireccion" + nroDocumento] != null)
            {
                objLog.CrearArchivolog("[CargarDatosSecAnteriorIfi()][Session[objDireccion]]", "", null);
                List<BEDireccionCliente> objLista = (List<BEDireccionCliente>)Session["objDireccion" + nroDocumento];
                if (objLista != null)
                {
                    oDireccion = objLista.LastOrDefault();
                }                
            }
            else
            {
                if (nroSEC > 0)
                {
                    objLog.CrearArchivolog("[CargarDatosSecAnteriorIfi()][ConsultarSolDireccion()] => ", nroSEC.ToString(), null);
                    List<BEDireccionCliente> arrDireccion = new BLSolicitudNegocios().ConsultarSolDireccion(nroSEC);
                    foreach (BEDireccionCliente item in arrDireccion)
                    {
                        if (item.IdTipoDireccion == "I")
                        {
                            oDireccion = new BEDireccionCliente();
                            oDireccion = item;
                            break;
                        }
                    }
                }
            }

            if (oDireccion != null)
            {
                objLog.CrearArchivolog("[INICIO][CargarDatosSecAnteriorIfi][oDireccion]", null, null);
                ddlPrefijo.SelectedValue = oDireccion.IdPrefijo;
                txtDireccion.Text = oDireccion.Direccion;
                txtNroPuerta.Text = oDireccion.NroPuerta;
                if (string.IsNullOrEmpty(oDireccion.IdEdificacion))
                {
                    oDireccion.IdEdificacion = "-1";
                }
                ddlEdificacion.SelectedValue = oDireccion.IdEdificacion;
                txtManzana.Text = oDireccion.Manzana;
                txtLote.Text = oDireccion.Lote;
                if (string.IsNullOrEmpty(oDireccion.IdTipoInterior))
                {
                    oDireccion.IdTipoInterior = "-1";
                }
                ddlTipoInterior.SelectedValue = oDireccion.IdTipoInterior;
                txtNroInterior.Text = oDireccion.NroInterior;
                if (string.IsNullOrEmpty(oDireccion.IdUrbanizacion))
                {
                    oDireccion.IdUrbanizacion = "-1";
                }
                ddlUrbanizacion.SelectedValue = oDireccion.IdUrbanizacion;
                txtUrbanizacion.Text = oDireccion.TxtUrbanizacion;
                if (string.IsNullOrEmpty(oDireccion.IdDomicilio))
                {
                    oDireccion.IdDomicilio = "-1";
                }
                ddlTipoDomicilio.SelectedValue = oDireccion.IdDomicilio;
                if (string.IsNullOrEmpty(oDireccion.IdZona))
                {
                    oDireccion.IdZona = "-1";
                }
                ddlZona.SelectedValue = oDireccion.IdZona;
                txtNombreZona.Text = oDireccion.NombreZona;
                txtReferencia.Text = oDireccion.Referencia;
                ddlDepartamento.SelectedValue = oDireccion.IdDepartamento;

                llenarCboProvincias(oDireccion.IdDepartamento);
                ddlProvincia.SelectedValue = oDireccion.IdProvincia;
                hidProvinciaId.Value = oDireccion.IdProvincia;

                llenarCboDistritos(oDireccion.IdProvincia);
                ddlDistrito.SelectedValue = oDireccion.IdDistrito;
                hidDistritoId.Value = oDireccion.IdDistrito;

                txtCodigoPostal.Text = oDireccion.IdPostal;
                txtUbigeo.Text = oDireccion.IdUbigeo;
                txtPrefijoTelefonoReferencia.Text = oDireccion.IdTelefono;
                txtTelefonoReferencia.Text = oDireccion.Telefono;

                txtCodPlano.Text = oDireccion.IdPlano;
                txtRefSecundaria.Text = oDireccion.Referencia_Sec;
                txtVendedorDNI.Text = oDireccion.DniVendedor;

                hidFlagVOD.Value = oDireccion.FlagVOD;
                txtCodEdificio.Text = oDireccion.IdEdificio;

				//INICIO INICIATIVA-932
                if (Funciones.CheckStr(hidFlagIFI.Value) == "1")
                {
                	hidTieneDireccion.Value = "0";
				}
				//FIN INICIATIVA-932

                objLog.CrearArchivolog("[FIN][CargarDatosSecAnteriorIfi][oDireccion]", null, null);
            }
            objLog.CrearArchivolog("[Fin][CargarDatosSecAnteriorIfi]", null, null);
        }
        //PROY-140690 - FIN
    }
}
