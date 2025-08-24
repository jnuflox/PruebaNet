using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using System.Text;
using Claro.SISACT.Common;
//PROY-140657 INI
using System.Data;
using Claro.SISACT.Web.Base;
using Claro.SISACT.Web.Comun;
using Claro.SISACT.WS;
using Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.ConsultarAfiliacion;
using Claro.SISACT.WS.RestReferences;
using Claro.SISACT.Entity.AfiliaciónDEAUAsistidosWSRest.EnviaLink;
//PROY-140657 FIN
using Claro.SISACT.Entity.AfiliacionDEAUDebito.ConsultarAfiliacion;

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_pop_debito_automatico : Sisact_Webbase//System.Web.UI.Page
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

            if (!Page.IsPostBack)
            {
                Inicio();
            }

        }

        public void Inicio()
        {
            GeneradorLog objLog = new GeneradorLog("Inicio Afiliacion Debito Automatico", Funciones.CheckStr(""), null, "WEB");
            try
            {
                hidNumeroDocumento.Value = Request.QueryString["nroDocumento"];
                hidNombresRazonSocial.Value = Request.QueryString["nombres"];
                hidTipoDocumento.Value = Request.QueryString["tipoDocumento"];
                hidMontoMaximo.Value = Request.QueryString["montoMaximo"];
                hidOfertaDEAU.Value = Request.QueryString["tipoOferta"];

                // INI INICIATIVA 941 - IDEA 142525
                hidCorreoElectronico.Value = Request.QueryString["correoNoti"];
                var envioLinkFallido = Request.QueryString["envioLinkFallido"];
                if (envioLinkFallido == "true")
                {
                    hidnEnvioLinkFallido.Value = envioLinkFallido;
                    hidnIdAfiliacionPrevio.Value = Request.QueryString["idAfiliacionPrevio"];
                }
                // FIN INICIATIVA 941 - IDEA 142525

                FlagAfiliacionDEAU.Value = ReadKeySettings.key_FlagAfiliacionDEAU;
                MsjValidacionMontoDEAU.Value = ReadKeySettings.key_MsjValidacionMontoDEAU;
                MsjCorreoObligatorioDEAU.Value = ReadKeySettings.key_MsjCorreoObligatorioDEAU;
                FlagConsultarAltaDEAU.Value = ReadKeySettings.key_FlagConsultarAltaDEAU;

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][nroDocumento]", Funciones.CheckStr(hidNumeroDocumento.Value)), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][nombres]", Funciones.CheckStr(hidNombresRazonSocial.Value)), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][tipoDocumento]", Funciones.CheckStr(hidTipoDocumento.Value)), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][montoMaximo]", Funciones.CheckStr(hidMontoMaximo.Value)), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][montoMaximo]", Funciones.CheckStr(hidMontoMaximo.Value)), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][Incio] hidOfertaDEAU: ", Funciones.CheckStr(hidOfertaDEAU.Value)), null, null);
                LlenarTipoDocumento();
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}|{2}", "[PROY-140457-DEBITO AUTOMATICO][ERROR]", ex.Message, ex.StackTrace), null, null);
            }
        }

        public void CargarEntidad(string idSolicitud)
        {
            List<BEItemGenerico> lstEntidad = new List<BEItemGenerico>();
            lstEntidad = BLEvaluacion.ListarEntidad(idSolicitud);

            if (idSolicitud == "-3")
            {
                foreach (var item in lstEntidad)
                {
                    if (item.Codigo == "2")
                    {
                        item.Descripcion = "BANCO DE CREDITO DEL PERU";
                    }
                    else if (item.Codigo == "4")
                    {
                        item.Descripcion = "BANCO CONTINENTAL";
                    }
                }
            }

            ddlEntidad.DataSource = lstEntidad;
            ddlEntidad.DataValueField = "Codigo";
            ddlEntidad.DataTextField = "Descripcion";
            ddlEntidad.DataBind();
            ddlEntidad.Items.Insert(0, new ListItem("SELECCIONE...", String.Empty));
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod cargarTipoCuenta(string idEntidad)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            List<BEItemGenerico> lstCuenta = new List<BEItemGenerico>();
            StringBuilder sbCuentas = new StringBuilder();
            string strResultado = string.Empty;
            StringBuilder sbCadena = new StringBuilder();
            string strEntidadTarjeta = string.Empty;
            GeneradorLog objLog = new GeneradorLog("cargarTipoCuenta", Funciones.CheckStr(""), null, "WEB");
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][cargarTipoCuenta]", "[INICIO]"), null, null);
            try
            {
                lstCuenta = BLEvaluacion.ListarCuenta(idEntidad);

                foreach (BEItemGenerico obj in lstCuenta)
                {
                    sbCuentas.AppendFormat("|{0};{1}", obj.Codigo, obj.Descripcion);
                }
                strResultado = sbCuentas.ToString();


                if ((idEntidad == "-1" || idEntidad == "-2" || idEntidad == "-3" || idEntidad == "-4") && strResultado == "")
                {
                    strEntidadTarjeta = Funciones.CheckStr(ReadKeySettings.Key_CuentaEDebitoAuto);
                    if (!string.IsNullOrEmpty(strEntidadTarjeta))
                    {
                        string[] cadena = strEntidadTarjeta.Split('|');
                        if (cadena != null)
                        {
                            int x = 0;
                            for (int i = 0; i < cadena.Length; i++)
                            {
                                x = x + 1;
                                sbCadena.AppendFormat("|{0};{1}", x, cadena[i]);
                            }
                        }
                    }
                    strResultado = sbCadena.ToString();
                }

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][cargarTipoCuenta][strResultado]", strResultado), null, null);
                objResponse.Cadena = strResultado;
            }
            catch (Exception ex)
            {
                objResponse.Error = true;
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"].ToString();
                objLog.CrearArchivolog(string.Format("{0} => {1}|{2}", "[PROY-140457-DEBITO AUTOMATICO][cargarTipoCuenta][ERROR]", ex.Message, ex.StackTrace), null, null);
            }
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][cargarTipoCuenta]", "[FIN]"), null, null);
            return objResponse;
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod CargarEntidadFront(string idSolicitud)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            List<BEItemGenerico> lstEntidades = new List<BEItemGenerico>();
            StringBuilder sbEntidades = new StringBuilder();
            string strResultado = string.Empty;
            GeneradorLog objLog = new GeneradorLog("CargarEntidadFront", Funciones.CheckStr(""), null, "WEB");
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][CargarEntidadFront]", "[INICIO]"), null, null);
            try
            {
                lstEntidades = BLEvaluacion.ListarEntidad(idSolicitud);

                if (idSolicitud == "-3")
                {
                    foreach (var item in lstEntidades)
                    {
                        if (item.Codigo == "2")
                        {
                            item.Descripcion = "BANCO DE CREDITO DEL PERU";
                        }
                        else if (item.Codigo == "4")
                        {
                            item.Descripcion = "BANCO CONTINENTAL";
                        }
                    }
                }

                string entidadPermitida = Funciones.CheckStr(ReadKeySettings.Key_eTarjetaDebitoAuto);
                if (lstEntidades.Count > 0 && idSolicitud == "-2")
                {
                    lstEntidades.RemoveAll(x => entidadPermitida.Contains(x.Codigo));
                }

                foreach (BEItemGenerico obj in lstEntidades)
                {
                    sbEntidades.AppendFormat("|{0};{1}", obj.Codigo, obj.Descripcion);
                }
                strResultado = sbEntidades.ToString();
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][CargarEntidadFront][strResultado]", strResultado), null, null);
                objResponse.Cadena = strResultado;
            }
            catch (Exception ex)
            {
                objResponse.Error = true;
                objResponse.Mensaje = ConfigurationManager.AppSettings["consMsjErrorGeneral"].ToString();
                objLog.CrearArchivolog(string.Format("{0} => {1}|{2}", "[PROY-140457-DEBITO AUTOMATICO][CargarEntidadFront][ERROR]", ex.Message, ex.StackTrace), null, null);
            }
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][CargarEntidadFront]", "[FIN]"), null, null);
            return objResponse;
        }

        public void LlenarTipoDocumento()
        {
            List<BETipoDocumento> lstDocumento = null;
            lstDocumento = (new BLGeneral()).ListarTipoDocumento();

            ddlTipoDoc.DataSource = lstDocumento;
            ddlTipoDoc.DataValueField = "ID_SISACT";
            ddlTipoDoc.DataTextField = "DESCRIPCION";
            ddlTipoDoc.DataBind();
            ddlTipoDoc.Items.Insert(0, new ListItem("SELECCIONE...", String.Empty));
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod Grabar(string codEntidad, string descEntidad, string codCuenta, string descCuenta, string numCuenta, string fecVencimiento,
            string flagMonto, string montoMax, string nombres, string numDocumento, string descTipoDoc, string tipoDocumento, string correo, string numeroANoti,
            string codSolicitud, string descSolicitud, int flagUsarTarjeta, string idAfiliacion, string idTarjeta, string nroTarjeta)
        {
            GeneradorLog objLog = new GeneradorLog("Grabar", Funciones.CheckStr(numDocumento), null, "WEB");
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            HttpContext.Current.Session["InfDebitoAutomatico"] = null;
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar]", "[INICIO]"), null, null);
            // INI INICIATIVA 941 - IDEA 142525
            bool esClaro = false;
            try
            {
                
                BEDebitoAutomatico objdebito = new BEDebitoAutomatico()
                {
                    Descripcion_TipoDoc = descTipoDoc,
                    Tipo_Documento = tipoDocumento,
                    Numero_Documento = numDocumento,
                    Nombre_Cliente = nombres,
                    Cod_Solicitud = codSolicitud,
                    Desc_Solicitud = descSolicitud,
                    Codigo_Entidad = codEntidad,
                    Descripcion_Entidad = descEntidad,
                    Tipo_Cuenta = codCuenta,
                    Descripcion_Cuenta = descCuenta,
                    Numero_Cuenta = numCuenta,
                    Fecha_Vencimiento = fecVencimiento,
                    Flag_MontoTope = flagMonto,
                    MontoTope = montoMax,
                    Telefono_Contacto = numeroANoti,
                    Correo_Contacto = correo,
                    flagTarjetaAfiliacion = flagUsarTarjeta,
                    idAfiliacion = (flagUsarTarjeta == 1) ? idAfiliacion : string.Empty,
                    idTarjeta = (flagUsarTarjeta == 1) ? idTarjeta : string.Empty,
                    nroTarjeta = (flagUsarTarjeta == 1) ? nroTarjeta : string.Empty
                };

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Descripcion_TipoDoc]", objdebito.Descripcion_TipoDoc), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Tipo_Documento]", objdebito.Tipo_Documento), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Numero_Documento]", objdebito.Numero_Documento), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Nombre_Titular]", objdebito.Nombre_Cliente), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Cod_Solicitud]", objdebito.Cod_Solicitud), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Desc_Solicitud]", objdebito.Desc_Solicitud), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Codigo_Entidad]", objdebito.Codigo_Entidad), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Descripcion_Entidad]", objdebito.Descripcion_Entidad), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Tipo_Cuenta]", objdebito.Tipo_Cuenta), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Descripcion_Cuenta]", objdebito.Descripcion_Cuenta), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Numero_Cuenta]", objdebito.Numero_Cuenta), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Fecha_Vencimiento]", objdebito.Fecha_Vencimiento), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Flag_MontoTope]", objdebito.Flag_MontoTope), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.MontoTope]", objdebito.MontoTope), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Telefono_Contacto]", objdebito.Telefono_Contacto), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][objdebito.Correo_Contacto]", objdebito.Correo_Contacto), null, null);

                HttpContext.Current.Session["InfDebitoAutomatico"] = objdebito;

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[INICIATIVA 941-DEBITO AUTOMATICO][Grabar]", "[INICIO VALIDAR CLARO]"), null, null);
                esClaro = validarLineaClaro(numeroANoti);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[INICIATIVA 941-DEBITO AUTOMATICO][Grabar]", "[FIN VALIDAR CLARO]"), null, null);

                // INI INICIATIVA 941 - IDEA 142525
                if (esClaro)
                {
                objResponse.Boleano = true;
            }
                else
                {
                objResponse.Boleano = true;
                    objResponse.Error = true;
                    objResponse.Mensaje = (string)ConfigurationManager.AppSettings["key_operadorNoEsClaro"];
                }
            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                objResponse.Error = true;
                objResponse.Mensaje = Funciones.CheckStr(ReadKeySettings.Key_msjErrorDebitoAuto);
                HttpContext.Current.Session["InfDebitoAutomatico"] = null;
                objLog.CrearArchivolog(string.Format("{0} => {1}|{2}", "[PROY-140457-DEBITO AUTOMATICO][Grabar][ERROR]", ex.Message, ex.StackTrace), null, null);
            }
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140457-DEBITO AUTOMATICO][Grabar]", "[FIN]"), null, null);
            return objResponse;
        }
        //PROY-140657 INI
      // FIN INICIATIVA 941 - IDEA 142525
      
         [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod ConsultarAfiliados(string tipoDoc, string numeroDoc)
        {
             GeneradorLog objLog = new GeneradorLog("ConsultarAfiliados", Funciones.CheckStr(numeroDoc), null, "WEB");
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][tipoDoc]",  Funciones.CheckStr(tipoDoc)), null, null);
             objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][numeroDoc]",  Funciones.CheckStr(numeroDoc)), null, null);

             BEResponseWebMethod responseT = new BEResponseWebMethod();
             BodyRequestConsultarAfiliacionDEAU objReqConsAfiliacion = new BodyRequestConsultarAfiliacionDEAU();
             string tipoDocConsulta = string.Empty;
             bool flagMotor = false;
             StringBuilder sbCadenaTarjetas = new StringBuilder();                         
             string strCadenaTarjetas = string.Empty;
             
             try
             {

                 #region HOMOLOGACION DOCUMENTOS MOTOR
                 string[] documentosMotor = Funciones.CheckStr(ReadKeySettings.key_DocumentosMotorDEAU).Split('|');

                 objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][documentosMotor]", Funciones.CheckStr(documentosMotor)), null, null);

                 if (documentosMotor != null)
                 {
                     for (int i = 0; i < documentosMotor.Length; i++)
                     {
                         if (documentosMotor[i] == tipoDoc)
                         {
                             tipoDocConsulta = documentosMotor[i + 1];
                             flagMotor = true;
                             break;
                         }
                         else
                         {
                             i = i + 2;
                         }
                     }
                 }
                 else
                 {
                     tipoDocConsulta = "1";
                 }

                 objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][flagMotor]", Funciones.CheckStr(flagMotor)), null, null);

                 if (!flagMotor)
                 {
                     tipoDocConsulta = "1";
                 }

                 objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][tipoDocConsulta]", Funciones.CheckStr(tipoDocConsulta)), null, null);                 
                 #endregion

              objReqConsAfiliacion.tipoDocumento = Funciones.CheckStr(tipoDocConsulta);
             objReqConsAfiliacion.nroDocumento = Funciones.CheckStr(numeroDoc);
             objReqConsAfiliacion.tipoConsulta = Funciones.CheckStr(ReadKeySettings.key_TipoConsultaDEAU); 

             objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][objReqConsAfiliacion.tipoDocumento]",Funciones.CheckStr(objReqConsAfiliacion.tipoDocumento) ), null, null);
             objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][objReqConsAfiliacion.nroDocumento]",Funciones.CheckStr(objReqConsAfiliacion.nroDocumento) ), null, null);
             objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][objReqConsAfiliacion.tipoConsulta]", Funciones.CheckStr(objReqConsAfiliacion.tipoConsulta)), null, null);

                 BodyResponseConsultarAfiliacionDEAU objResConsAfiliacion = ConsultarAfiliacionDEAUAsistidos(objReqConsAfiliacion);

                 objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][objResConsAfiliacion.codigoRespuesta]",Funciones.CheckStr(objResConsAfiliacion.codigoRespuesta)), null, null);

                 if (objResConsAfiliacion.codigoRespuesta == "0")
                 {
                     objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][listAfiliacion.Count]", Funciones.CheckStr(objResConsAfiliacion.listAfiliacion.Count())), null, null);
                     if (objResConsAfiliacion.listAfiliacion.Count() > 0)
                     {                         
                         List<BEListaConsultarAfiliacion> lstAfiliacion = new List<BEListaConsultarAfiliacion>();
                         lstAfiliacion = objResConsAfiliacion.listAfiliacion;

                         objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][lstAfiliacion]", Funciones.CheckStr(lstAfiliacion)), null, null);

                         foreach (BEListaConsultarAfiliacion Tarjeta in lstAfiliacion)
                         {
                             sbCadenaTarjetas.AppendFormat("{0}#{1}#{2}#{3}|", Tarjeta.origenSolicitud, Tarjeta.tipoCliente, Tarjeta.numTarjeta, Tarjeta.fechaRegistro);
                         }

                         strCadenaTarjetas = Funciones.CheckStr(sbCadenaTarjetas);
                         objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][strCadenaTarjetas.Length]", Funciones.CheckStr(strCadenaTarjetas.Length)), null, null);
                         
                         if (strCadenaTarjetas.Length > 0)
                         {
                             strCadenaTarjetas = strCadenaTarjetas.Substring(0, strCadenaTarjetas.Length - 1);
                         }

                         objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][strCadenaTarjetas]", Funciones.CheckStr(strCadenaTarjetas)), null, null);
                         responseT.CodigoError = "0";
                     }
                 }
                 else
                 {
                     responseT.Mensaje = "No existen Registros";
                 }

                 responseT.Cadena = strCadenaTarjetas;
             }
             catch(Exception ex)
             {
                 objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][ERROR][ex.StackTrace]", Funciones.CheckStr(ex.StackTrace)), null, null);
                 objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][ERROR][ex.Messag]", Funciones.CheckStr(ex.Message)), null, null);
             }
     
            return responseT;
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod ConsultarAfiliados2(string tipoDoc, string numeroDoc)
        {
            GeneradorLog objLog = new GeneradorLog("ConsultarAfiliados", Funciones.CheckStr(numeroDoc), null, "WEB");
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][tipoDoc]", Funciones.CheckStr(tipoDoc)), null, null);
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][numeroDoc]", Funciones.CheckStr(numeroDoc)), null, null);

            BEResponseWebMethod responseT = new BEResponseWebMethod();
            BodyRequestConsultarAfiliacionDEAUDebito objReqConsAfiliacion = new BodyRequestConsultarAfiliacionDEAUDebito();
            string tipoDocConsulta = string.Empty;
            bool flagMotor = false;
            StringBuilder sbCadenaTarjetas = new StringBuilder();
            string strCadenaTarjetas = string.Empty;

            try
            {

                #region HOMOLOGACION DOCUMENTOS MOTOR
                string[] documentosMotor = Funciones.CheckStr(ReadKeySettings.key_DocumentosMotorDEAU).Split('|');

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][documentosMotor]", Funciones.CheckStr(documentosMotor)), null, null);

                if (documentosMotor != null)
                {
                    for (int i = 0; i < documentosMotor.Length; i++)
                    {
                        if (documentosMotor[i] == tipoDoc)
                        {
                            tipoDocConsulta = documentosMotor[i + 1];
                            flagMotor = true;
                            break;
                        }
                        else
                        {
                            i = i + 2;
                        }
                    }
                }
                else
                {
                    tipoDocConsulta = "1";
                }

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][flagMotor]", Funciones.CheckStr(flagMotor)), null, null);

                if (!flagMotor)
                {
                    tipoDocConsulta = "1";
                }

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][tipoDocConsulta]", Funciones.CheckStr(tipoDocConsulta)), null, null);
                #endregion

                tipoDocConsulta = "3";
                objReqConsAfiliacion.tipo = tipoDocConsulta;
                objReqConsAfiliacion.tipoConsulta = Funciones.CheckStr(tipoDocConsulta) + "," + Funciones.CheckStr(numeroDoc);

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][objReqConsAfiliacion.tipoDocumento]", Funciones.CheckStr(tipoDocConsulta)), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][objReqConsAfiliacion.nroDocumento]", Funciones.CheckStr(numeroDoc)), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][objReqConsAfiliacion.tipoConsulta]", Funciones.CheckStr(objReqConsAfiliacion.tipoConsulta)), null, null);

                BodyResponseConsultarAfiliacionDEAUDebito objResConsAfiliacion = ConsultarAfiliacionesDEAUDebito(objReqConsAfiliacion);

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][objResConsAfiliacion.codigoRespuesta]", Funciones.CheckStr(objResConsAfiliacion.codigoRespuesta)), null, null);

                if (objResConsAfiliacion.codigoRespuesta == "0")
                {
                    objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][listAfiliacion.Count]", Funciones.CheckStr(objResConsAfiliacion.listaAfiliacion.Count())), null, null);
                    if (objResConsAfiliacion.listaAfiliacion.Count() > 0)
                    {
                        List<BEListaConsultarAfiliacionDebito> lstAfiliacion = new List<BEListaConsultarAfiliacionDebito>();
                        lstAfiliacion = objResConsAfiliacion.listaAfiliacion;

                        objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][lstAfiliacion]", Funciones.CheckStr(lstAfiliacion)), null, null);

                        string[] lstOrigenAfiliacion = ConfigurationManager.AppSettings["arrOrigenAfiliacion"].Split('|');
                        foreach (BEListaConsultarAfiliacionDebito Tarjeta in lstAfiliacion)
                        {
                            if (Tarjeta.estado == "3")
                            {
                                sbCadenaTarjetas.AppendFormat("{0}#{1}#{2}#{3}#{4}#{5}#{6}#{7}|", Tarjeta.codCuenta, lstOrigenAfiliacion[Convert.ToInt32(Tarjeta.origenAfiliacion) - 1], Tarjeta.numTarjeta, Tarjeta.estado, Convert.ToDateTime(Tarjeta.fechaReg).ToString("yyyy-MM-dd"), Tarjeta.idAfiliacion, Tarjeta.origenAfiliacion, Tarjeta.tarjetaId);
                            }
                            
                        }

                        strCadenaTarjetas = Funciones.CheckStr(sbCadenaTarjetas);
                        objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][strCadenaTarjetas.Length]", Funciones.CheckStr(strCadenaTarjetas.Length)), null, null);

                        if (strCadenaTarjetas.Length > 0)
                        {
                            strCadenaTarjetas = strCadenaTarjetas.Substring(0, strCadenaTarjetas.Length - 1);
                        }

                        objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][strCadenaTarjetas]", Funciones.CheckStr(strCadenaTarjetas)), null, null);
                        responseT.CodigoError = "0";
                    }
                }
                else
                {
                    responseT.Mensaje = "No existen Registros";
                }

                responseT.Cadena = strCadenaTarjetas;
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][ERROR][ex.StackTrace]", Funciones.CheckStr(ex.StackTrace)), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ConsultarAfiliados][ERROR][ex.Messag]", Funciones.CheckStr(ex.Message)), null, null);
            }

            return responseT;
        }


        private static BodyResponseConsultarAfiliacionDEAU ConsultarAfiliacionDEAUAsistidos(BodyRequestConsultarAfiliacionDEAU objConsAfiliacion)
        {
            GeneradorLog _objLog = new GeneradorLog(CurrentUsers, objConsAfiliacion.nroDocumento, null, "WEB");
            _objLog.CrearArchivolog("[sisact_pop_debito_automatico][ConsultarAfiliacionDEAUAsistidos][INICIO]", null, null);
            BodyResponseConsultarAfiliacionDEAU objConsultarAfiliacion = new BodyResponseConsultarAfiliacionDEAU();
            try
            {
                #region Request
                RequestConsultarAfiliacionDEAU objRequestConsultarAfil = new RequestConsultarAfiliacionDEAU();
                BodyRequestConsultarAfiliacionDEAU objRegistrarAfilRequest = new BodyRequestConsultarAfiliacionDEAU();

                objRequestConsultarAfil.MessageRequest.header.HeaderRequest = WebComunes.GenerarHeader(objConsAfiliacion.nroDocumento, Claro.SISACT.Web.Base.Sisact_Webbase.CurrentUsers);
                
                #region Datos Body
                objRegistrarAfilRequest.nroDocumento = objConsAfiliacion.nroDocumento;
                objRegistrarAfilRequest.tipoConsulta = ReadKeySettings.key_TipoConsultaDEAU;
                objRegistrarAfilRequest.tipoDocumento = objConsAfiliacion.tipoDocumento;
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][ConsultarAfiliacionDEAUAsistidos][objRegistrarAfilRequest.numDocumento]: ", Funciones.CheckStr(objRegistrarAfilRequest.nroDocumento)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][ConsultarAfiliacionDEAUAsistidos][objRegistrarAfilRequest.tipoConsulta]: ", Funciones.CheckStr(objRegistrarAfilRequest.tipoConsulta)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][ConsultarAfiliacionDEAUAsistidos][objRegistrarAfilRequest.tipoDocumento]: ", Funciones.CheckStr(objRegistrarAfilRequest.tipoDocumento)), null, null);
                #endregion
                objRequestConsultarAfil.MessageRequest.body = objRegistrarAfilRequest;
                #endregion

                #region Auditoria
                BEAuditoriaRequest objBEAuditoriaRequest = new BEAuditoriaRequest();
                objBEAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBEAuditoriaRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
                objBEAuditoriaRequest.accept = "application/json";
                objBEAuditoriaRequest.ipApplication = Claro.SISACT.Web.Base.Sisact_Webbase.CurrentServer;
                #endregion

                #region Response
                ResponseConsultarAfiliacionDEAU objResponseConsAfiliacion = new ResponseConsultarAfiliacionDEAU();

                //RestAfiliaciónDEAUAsistidosWS RestAfiliaciónDEAUA = new RestAfiliaciónDEAUAsistidosWS();
                objResponseConsAfiliacion = new RestAfiliaciónDEAUAsistidosWS().ConsultarAfiliacion(objRequestConsultarAfil, objBEAuditoriaRequest);

                objConsultarAfiliacion.codigoRespuesta = objResponseConsAfiliacion.MessageResponse.body.codigoRespuesta;
                objConsultarAfiliacion.mensajeRespuesta = objResponseConsAfiliacion.MessageResponse.body.mensajeRespuesta;
                objConsultarAfiliacion.listAfiliacion = objResponseConsAfiliacion.MessageResponse.body.listAfiliacion;

                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][ConsultarAfiliacionDEAUAsistidos] objConsultarAfiliacion.codigoRespuesta: " + Funciones.CheckStr(objConsultarAfiliacion.codigoRespuesta), null, null);
                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][ConsultarAfiliacionDEAUAsistidos] objConsultarAfiliacion.mensajeRespuesta: " + Funciones.CheckStr(objConsultarAfiliacion.mensajeRespuesta), null, null);
                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][ConsultarAfiliacionDEAUAsistidos] objConsultarAfiliacion.listAfiliacion: " + Funciones.CheckStr(objConsultarAfiliacion.listAfiliacion.Count()), null, null);

                #endregion

            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(string.Format("{0} => [{1}|{2}]", "[sisact_pop_debito_automatico][ConsultarAfiliacionDEAUAsistidos]ex.Message|ex.StackTrace", ex.Message, ex.StackTrace), null, null);
            }
            _objLog.CrearArchivolog("[sisact_pop_debito_automatico][ConsultarAfiliacionDEAUAsistidos][FIN]", null, null);
            return objConsultarAfiliacion;
        }
        //PROY-140657 FIN

        //PROY-142525 INI
        private static BodyResponseConsultarAfiliacionDEAUDebito ConsultarAfiliacionesDEAUDebito(BodyRequestConsultarAfiliacionDEAUDebito objRequestConsAfiliacion)
        {
            GeneradorLog _objLog = new GeneradorLog(CurrentUsers, objRequestConsAfiliacion.tipoConsulta, null, "WEB");
            _objLog.CrearArchivolog("[sisact_pop_debito_automatico][ConsultarAfiliacionesDEAUDebito][INICIO]", null, null);
            BodyResponseConsultarAfiliacionDEAUDebito objConsultarAfiliacion = new BodyResponseConsultarAfiliacionDEAUDebito();            
            try
            {
                #region Request
                //RequestConsultarAfiliacionDebitoDEAU objRequestConsultarAfil = new RequestConsultarAfiliacionDebitoDEAU();
                //BodyRequestConsultarAfiliacionDEAU objRegistrarAfilRequest = new BodyRequestConsultarAfiliacionDEAU();

                //TODO: REVISAR NO ES NECESARIO YA Q PARA EL REQUEST SOLO SE USA EL query string
                //objRequestConsultarAfil.MessageRequest.header.HeaderRequest = WebComunes.GenerarHeader(objConsAfiliacion.nroDocumento, Claro.SISACT.Web.Base.Sisact_Webbase.CurrentUsers);
                
                #region Datos Body
                //objRegistrarAfilRequest.nroDocumento = objConsAfiliacion.nroDocumento;
                //objRegistrarAfilRequest.tipoConsulta = ReadKeySettings.key_TipoConsultaDEAU;
                //objRegistrarAfilRequest.tipoDocumento = objConsAfiliacion.tipoDocumento;FlagConsultarAltaDEAU
                //_objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][ConsultarAfiliacionesDEAUDebito][objRegistrarAfilRequest.numDocumento]: ", Funciones.CheckStr(objRegistrarAfilRequest.nroDocumento)), null, null);
                //_objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][ConsultarAfiliacionesDEAUDebito][objRegistrarAfilRequest.tipoConsulta]: ", Funciones.CheckStr(objRegistrarAfilRequest.tipoConsulta)), null, null);
                //_objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][ConsultarAfiliacionesDEAUDebito][objRegistrarAfilRequest.tipoDocumento]: ", Funciones.CheckStr(objRegistrarAfilRequest.tipoDocumento)), null, null);
                #endregion
                //objRequestConsultarAfil.MessageRequest.body = objRegistrarAfilRequest;
                #endregion

                #region Auditoria
                BEAuditoriaRequest objBEAuditoriaRequest = new BEAuditoriaRequest();
                objBEAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBEAuditoriaRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
                objBEAuditoriaRequest.accept = "application/json";
                objBEAuditoriaRequest.ipApplication = Claro.SISACT.Web.Base.Sisact_Webbase.CurrentServer;
                objBEAuditoriaRequest.usuarioAplicacion = Claro.SISACT.Web.Base.Sisact_Webbase.CurrentUsers;
                #endregion

                #region Response
                ResponseConsultarAfiliacionDEAUDebito objResponseConsultaAfiliaciones = new ResponseConsultarAfiliacionDEAUDebito();

                //RestAfiliaciónDEAUAsistidosWS RestAfiliaciónDEAUA = new RestAfiliaciónDEAUAsistidosWS();

                objResponseConsultaAfiliaciones = new RestAfiliacionDEAUDebito().ConsultarAfiliacionesDEAUDebito(objRequestConsAfiliacion, objBEAuditoriaRequest);

                objConsultarAfiliacion.codigoRespuesta = objResponseConsultaAfiliaciones.MessageResponse.body.codigoRespuesta;
                objConsultarAfiliacion.mensajeRespuesta = objResponseConsultaAfiliaciones.MessageResponse.body.mensajeRespuesta;
                objConsultarAfiliacion.listaAfiliacion = objResponseConsultaAfiliaciones.MessageResponse.body.listaAfiliacion;

                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][ConsultarAfiliacionesDEAUDebito] objConsultarAfiliacion.codigoRespuesta: " + Funciones.CheckStr(objConsultarAfiliacion.codigoRespuesta), null, null);
                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][ConsultarAfiliacionesDEAUDebito] objConsultarAfiliacion.mensajeRespuesta: " + Funciones.CheckStr(objConsultarAfiliacion.mensajeRespuesta), null, null);
                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][ConsultarAfiliacionesDEAUDebito] objConsultarAfiliacion.listAfiliacion: " + Funciones.CheckStr(objConsultarAfiliacion.listaAfiliacion.Count()), null, null);

                #endregion

            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(string.Format("{0} => [{1}|{2}]", "[sisact_pop_debito_automatico][ConsultarAfiliacionesDEAUDebito]ex.Message|ex.StackTrace", ex.Message, ex.StackTrace), null, null);
            }
            _objLog.CrearArchivolog("[sisact_pop_debito_automatico][ConsultarAfiliacionesDEAUDebito][FIN]", null, null);
            return objConsultarAfiliacion;
        }
        //PROY-142525 FIN

        // INI INICIATIVA 941 - IDEA 142525
        [System.Web.Services.WebMethod()]
        public static BodyResponseEnviaLinkDEAU EnviarLinkAfiliacionDebito(string correoContacto, string telefonoContacto, string idAfiliacionPrevio)
        {
            GeneradorLog objLog = new GeneradorLog("EnviarLinkAfiliacionDebito", Funciones.CheckStr(""), null, "WEB");


            BodyRequestEnviaLinkDEAU objEnviaLink = new BodyRequestEnviaLinkDEAU();
            objEnviaLink.codCanal = Funciones.CheckStr(ReadKeySettings.key_CanalMPSisactDEAU);
            objEnviaLink.correo = correoContacto;
            objEnviaLink.inicioVigenciaLink = DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-ddTHH:mm:ssZ");
            objEnviaLink.finVigenciaLink = DateTime.Now.AddDays(Funciones.CheckInt(ReadKeySettings.key_HorasFechaVencimientoDEAU)).ToString("yyyy-MM-ddTHH:mm:ssZ"); //Funciones.CheckStr(DateTime.Now.Date.AddDays(1));
            objEnviaLink.idTransaccion = idAfiliacionPrevio;
            objEnviaLink.msisdn = telefonoContacto;
            objEnviaLink.tipoFlujo = ReadKeySettings.key_TipoFlujoEnvioLinkDEAU;
            objEnviaLink.clickMaximoLink = "2";
            objEnviaLink.longitudHash = "7";
            objEnviaLink.descripcion = "Afiliacion Presencial";
            objEnviaLink.ip = ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_wsip"]; ;
            //llamada a EnvioLink
            BodyResponseEnviaLinkDEAU objResponseEnviaLink = EnviaLinkDEAU(objEnviaLink);

            ////set value
            //hidnMensajeEnvioLink.Value = Funciones.CheckStr(HttpContext.Current.Session["MensajeEnvioLink"]);

            objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-142525][sisact_pop_debito_automatico][Grabar] EnviaLinkDEAU=> mensajeRespuesta: ", Funciones.CheckStr(objResponseEnviaLink.mensajeRespuesta)), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-142525][sisact_pop_debito_automatico][Grabar] EnviaLinkDEAU=> codigoRespuesta: ", Funciones.CheckStr(objResponseEnviaLink.codigoRespuesta)), null, null);
            objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-142525][sisact_pop_debito_automatico][Grabar] EnviaLinkDEAU=> idTransaccion: ", Funciones.CheckStr(objResponseEnviaLink.idTransaccion)), null, null);

            return objResponseEnviaLink;
        }

        private static BodyResponseEnviaLinkDEAU EnviaLinkDEAU(BodyRequestEnviaLinkDEAU objEnviaLink)
        {
            GeneradorLog _objLog = new GeneradorLog(CurrentUsers, objEnviaLink.msisdn, null, "WEB");
            _objLog.CrearArchivolog("[sisact_pop_debito_automatico][EnviaLinkDEAU][INICIO]", null, null);
            BodyResponseEnviaLinkDEAU objRespEnviaLink = new BodyResponseEnviaLinkDEAU();
            try
            {
                #region Request
                RequestEnviaLinkDEAU objRequestEnviaLink = new RequestEnviaLinkDEAU();
                BodyRequestEnviaLinkDEAU objBodyRequestEnviaLink = new BodyRequestEnviaLinkDEAU();

                objRequestEnviaLink.MessageRequest.header.HeaderRequest = WebComunes.GenerarHeader(objEnviaLink.msisdn, CurrentUsers);


                #region Datos Body
                objBodyRequestEnviaLink.idTransaccion = objEnviaLink.idTransaccion;// "20210520124568";
                objBodyRequestEnviaLink.tipoFlujo = objEnviaLink.tipoFlujo;// "2";
                objBodyRequestEnviaLink.msisdn = objEnviaLink.msisdn; //"985955854";
                objBodyRequestEnviaLink.codCanal = objEnviaLink.codCanal;// "5";
                objBodyRequestEnviaLink.correo = objEnviaLink.correo; //"piero.xcb.231192@gmail.com";
                objBodyRequestEnviaLink.inicioVigenciaLink = objEnviaLink.inicioVigenciaLink;
                objBodyRequestEnviaLink.finVigenciaLink = objEnviaLink.finVigenciaLink;// "29/05/2021 12:43:00";	
                objBodyRequestEnviaLink.clickMaximoLink = objEnviaLink.clickMaximoLink;
                objBodyRequestEnviaLink.longitudHash = objEnviaLink.longitudHash;
                objBodyRequestEnviaLink.descripcion = objEnviaLink.descripcion;
                objBodyRequestEnviaLink.ip = objEnviaLink.ip;
                #endregion
                #region log Datos Body
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.idTransaccion]: ", Funciones.CheckStr(objBodyRequestEnviaLink.idTransaccion)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.tipoFlujo]: ", Funciones.CheckStr(objBodyRequestEnviaLink.tipoFlujo)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.msisdn]: ", Funciones.CheckStr(objBodyRequestEnviaLink.msisdn)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.codCanal]: ", Funciones.CheckStr(objBodyRequestEnviaLink.codCanal)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.correo]: ", Funciones.CheckStr(objBodyRequestEnviaLink.correo)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.inicioVigenciaLink]: ", Funciones.CheckStr(objBodyRequestEnviaLink.inicioVigenciaLink)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.fechaVencimientoLink]: ", Funciones.CheckStr(objBodyRequestEnviaLink.finVigenciaLink)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.clickMaximoLink]: ", Funciones.CheckStr(objBodyRequestEnviaLink.clickMaximoLink)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.longitudHash]: ", Funciones.CheckStr(objBodyRequestEnviaLink.longitudHash)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.descripcion]: ", Funciones.CheckStr(objBodyRequestEnviaLink.descripcion)), null, null);
                _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[sisact_pop_debito_automatico][EnviaLinkDEAU][objBodyRequestEnviaLink.ip]: ", Funciones.CheckStr(objBodyRequestEnviaLink.ip)), null, null);
                #endregion
                objRequestEnviaLink.MessageRequest.body = objBodyRequestEnviaLink;
                #endregion

                #region Auditoria
                BEAuditoriaRequest objBEAuditoriaRequest = new BEAuditoriaRequest();
                objBEAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBEAuditoriaRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBEAuditoriaRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["system_ConsultaClave"]);
                objBEAuditoriaRequest.accept = "application/json";
                objBEAuditoriaRequest.ipApplication = Claro.SISACT.Web.Base.Sisact_Webbase.CurrentServer;
                #endregion

                #region Response
                ResponseEnviaLinkDEAU objResponseEnviaLink = new ResponseEnviaLinkDEAU();

                //RestAfiliaciónDEAUAsistidosWS RestAfiliaciónDEAUA = new RestAfiliaciónDEAUAsistidosWS();
                objResponseEnviaLink = new RestAfiliaciónDEAUAsistidosWS().EnviaLink(objRequestEnviaLink, objBEAuditoriaRequest);

                objRespEnviaLink.codigoRespuesta = objResponseEnviaLink.MessageResponse.body.codigoRespuesta;
                objRespEnviaLink.mensajeRespuesta = objResponseEnviaLink.MessageResponse.body.mensajeRespuesta;
                objRespEnviaLink.idTransaccion = objResponseEnviaLink.MessageResponse.body.idTransaccion;
                objRespEnviaLink.linkCliente = objResponseEnviaLink.MessageResponse.body.linkCliente;
                objRespEnviaLink.fechaexpiracion = objResponseEnviaLink.MessageResponse.body.fechaexpiracion;


                //MENSAJE ENVIO LINK
                if (Funciones.CheckInt64(objRespEnviaLink.codigoRespuesta) != 0)
                {
                    HttpContext.Current.Session["MensajeEnvioLink"] = "Error al intentar enviar el Link para afiliación al débito automático";
                }
                else
                {
                    HttpContext.Current.Session["MensajeEnvioLink"] = "Envio de Link para afiliación al débito automático, " + Funciones.CheckStr(objRespEnviaLink.mensajeRespuesta);
                }

                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][EnviaLinkDEAU] objRespEnviaLink.codigoRespuesta: " + Funciones.CheckStr(objRespEnviaLink.codigoRespuesta), null, null);
                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][EnviaLinkDEAU] objRespEnviaLink.mensajeRespuesta: " + Funciones.CheckStr(objRespEnviaLink.mensajeRespuesta), null, null);
                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][EnviaLinkDEAU] objRespEnviaLink.idTransaccion: " + Funciones.CheckStr(objRespEnviaLink.idTransaccion), null, null);
                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][EnviaLinkDEAU] objRespEnviaLink.linkCliente: " + Funciones.CheckStr(objRespEnviaLink.linkCliente), null, null);
                _objLog.CrearArchivolog("[sisact_pop_debito_automatico][EnviaLinkDEAU] objRespEnviaLink.fechaexpiracion: " + Funciones.CheckStr(objRespEnviaLink.fechaexpiracion), null, null);

                #endregion

            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(string.Format("{0} => [{1}|{2}]", "[sisact_pop_debito_automatico][EnviaLinkDEAU]ex.Message|ex.StackTrace", ex.Message, ex.StackTrace), null, null);
            }
            _objLog.CrearArchivolog("[sisact_pop_debito_automatico][EnviaLinkDEAU][FIN]", null, null);
            return objRespEnviaLink;
        }
        // FIN INICIATIVA 941 - IDEA 142525


        //INI INICIATIVA 941 - IDEA 142525
        public static Boolean validarLineaClaro(string numeroNotificar)
        {
            bool esClaro = false;
            GeneradorLog objLog = new GeneradorLog("validarLineaClaro", numeroNotificar, null, "WEB");
            try
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ValidarLineaClaro][numeroNotificar]", Funciones.CheckStr(numeroNotificar)), null, null);
                BWOperador oServicio = new BWOperador();
                esClaro = oServicio.validaLineaClaro(numeroNotificar);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ValidarLineaClaro][numeroNotificar][Es_Claro]", Convert.ToString(esClaro)), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} => [{1}|{2}]", "[sisact_pop_debito_automatico][ValidarLineaClaro]ex.Message|ex.StackTrace", ex.Message, ex.StackTrace), null, null);
            }
            return esClaro;

        }
        


        [System.Web.Services.WebMethod()]
        public static BEValidacionLineaClaroResponse wbValidarLineaClaro(string numeroNotificar)
        {
            bool esClaro = false;
            BEValidacionLineaClaroResponse validacionClaroResponse = null;

            GeneradorLog objLog = new GeneradorLog("validarLineaClaro", numeroNotificar, null, "WEB");
            try
            {
                validacionClaroResponse = new BEValidacionLineaClaroResponse();
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ValidarLineaClaro][numeroNotificar]", Funciones.CheckStr(numeroNotificar)), null, null);
                BWOperador oServicio = new BWOperador();
                esClaro = oServicio.validaLineaClaro(numeroNotificar);
                validacionClaroResponse.esClaro = esClaro;
                validacionClaroResponse.mensajeInformativo = (esClaro) ? "" : (string)ConfigurationManager.AppSettings["key_operadorNoEsClaro"];
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[sisact_pop_debito_automatico][ValidarLineaClaro][numeroNotificar][Es_Claro]", Convert.ToString(esClaro)), null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} => [{1}|{2}]", "[sisact_pop_debito_automatico][ValidarLineaClaro]ex.Message|ex.StackTrace", ex.Message, ex.StackTrace), null, null);
            }
            return validacionClaroResponse;

        }

        //FIN INICIATIVA 941 - IDEA 142525
    }
}