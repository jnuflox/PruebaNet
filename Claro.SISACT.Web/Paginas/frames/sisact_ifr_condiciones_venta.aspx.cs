using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.Business;//PROY-32129
using Claro.SISACT.Web.Base;//PROY-32129 FASE 2
using Claro.SISACT.Entity.ValidarServExcluyentesRest;//PROY-140383
using Claro.SISACT.WS.RestReferences;//PROY-140383
using Claro.SISACT.Entity.VentasCuotas.ValidarMaterialAccCuota.Response;//PROY-140743

namespace Claro.SISACT.Web.frames
{
    public partial class sisact_ifr_condiciones_venta : Sisact_Webbase //PROY-140126 - IDEA 140248  
    {
        GeneradorLog objLog = new GeneradorLog("sisact_ifr_condiciones_venta", null, null, "WEB");
        //PROY-32129 FASE 2 INICIO
        Sisact_Webbase user = new Sisact_Webbase();
        //PROY-32129 FASE 2 FIN
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
            CargarParametrosClaroUp(); //PROY-30166-IDEA–38863
            CargarParametrosServvAdi();//PROY-140383
            CargarParametrosCAI();//PROY-140546 Cobro Anticipado de Instalacion

            //INI-INC000002510501  Campañas
            hidFlagCampana.Value = Funciones.CheckStr(ReadKeySettings.Key_R_FlagCampana);
            //FIN-INC000002510501  Campañas
        }

        //PROY-140546 Cobro Anticipado de Instalacion
        private void CargarParametrosCAI()
        {
            try
            {
                objLog.CrearArchivolog("[Inicio][sisact_ifr_condiciones_venta.aspx.CargarParametrosCAI]", null, null);
                hidFlagAplicaCAI.Value = ReadKeySettings.ConsFlagAplicaCAI;
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Inicio][CargarParametrosCAI][ConsFlagAplicaCAI] ", ReadKeySettings.ConsFlagAplicaCAI), null, null);
                hidTiempoSecPendientePagoLink.Value = Funciones.CheckStr(ReadKeySettings.ConsTiempoSecPendientePagoLink);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Inicio][CargarParametrosCAI][ConsTiempoSecPendientePagoLink] ", ReadKeySettings.ConsTiempoSecPendientePagoLink), null, null);

                hidCanalesPermitidosCAI.Value = ReadKeySettings.Key_CanalesPermitidosCAI;
                objLog.CrearArchivolog(string.Format("{0}{1}", "CargarParametrosCAI][Key_CanalesPermitidosCAI] ", ReadKeySettings.Key_CanalesPermitidosCAI), null, null);

                hdiValoresRestringirCampanaFullClaro.Value = Funciones.CheckStr(ReadKeySettings.key_msjCampanasFullClaro);
                objLog.CrearArchivolog(string.Format("{0} -> {1}", "[INICIATIVA-1012][ValorRestringirCampanaFullClaro]", ReadKeySettings.key_msjCampanasFullClaro), null, null);

                objLog.CrearArchivolog("[Fin][sisact_ifr_condiciones_venta.aspx.CargarParametrosCAI]", null, null);
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[Error][sisact_ifr_condiciones_venta.aspx.CargarParametrosCAI]" + ex.Message, null, null);
            }
            
        }
        //PROY-140546 Cobro Anticipado de Instalacion

        //PROY-32129 :: INI
        [System.Web.Services.WebMethod()]
        public static string ValidarCampEspUniv(string strCampSelect, string strFilaSelect)
        {
            Int64 strResultadoCodigo = 0;
            string  strResultadoMensaje = "";
            bool lstParamCampUniv = (new BLCasoEspecial()).WhiteList_x_IdCampana(strCampSelect, ref strResultadoCodigo, ref strResultadoMensaje);

            if (lstParamCampUniv == true)
            {
                //campaña valida universitaria
                return "1|" + strFilaSelect;
            }
            else
            {
                return "0|" + strFilaSelect;
            }
        }
        //PROY-32129 :: FIN

        [System.Web.Services.WebMethod()]//INC000002245048
        public static void MostrarLogs(string ServiciosSi, string ServiciosNo,string nro_doc,string accion)
        {
// INC000002464679 INICIO 
            GeneradorLog objLog = new GeneradorLog("sisact_ifr_condiciones_venta-[" + CurrentTerminal + "]-[" + CurrentUsers + "]-[" + nro_doc + "]", null, null, "WEB"); //INC000002464679 


            if (ReadKeySettings.Key_FlagActivacionTopeConsumoAlta == "0")
            {
            objLog.CrearArchivolog("***", null, null);
                objLog.CrearArchivolog("ACCION: " + accion, null, null);
            objLog.CrearArchivolog("*************Inicio - Servicios Agregados*************", null, null);
            objLog.CrearArchivolog("[ServiciosSi]", ServiciosSi, null);
            objLog.CrearArchivolog("*************Fin - Servicios Agregados*************", null, null);
            objLog.CrearArchivolog("*************Inicio - Servicios No Agregados*************", null, null);
            objLog.CrearArchivolog("[ServiciosNo]", ServiciosNo, null);
            objLog.CrearArchivolog("*************Fin - Servicios No Agregados*************", null, null);
            objLog.CrearArchivolog("***", null, null);
        }

            // *ID*5|0_TCC_3_00219_0_GRUPO TCC;(*) Consumo Adicional|1_BMO_1_3007_9.99_GRUPO BMO;ADICIONAL INTERNACIONAL 30|1_VOZ_1_1734_60_GRUPO VOZ;PAQ CDI 200 MIN


            String[] strCad = { "*ID*" };
            String[] arrServiciosSi = ServiciosSi.Split(strCad, StringSplitOptions.None);


            String[] strCad1 = { "||" };
            String idPlan = Funciones.CheckStr(accion.Split(strCad1, StringSplitOptions.None)[1].Split('_')[0]);
            int idFilaGrilla = Funciones.CheckInt(accion.Split(strCad1, StringSplitOptions.None)[2]);

            Double dblCargoFijoAdi = 0;
            Double CargoFijoActual = Funciones.CheckDbl(accion.Split(strCad1, StringSplitOptions.None)[1].Split('_')[1]); ;
            try
            {

                for (int i1 = 1; i1 <= arrServiciosSi.Length -1; i1++)
                {
                    objLog.CrearArchivolog("{farude}arrServiciosSi[i1]" + arrServiciosSi[i1], null, null);

                    int idFila = 0;
                    String[] arrCF = arrServiciosSi[i1].Split('|');
                    for (int i2 = 0; i2 <= arrCF.Length - 1; i2++)
                    {

                        idFila = Funciones.CheckInt(arrServiciosSi[i1].Split('|')[0]);

                        objLog.CrearArchivolog("{farude}idFilaGrilla" + idFilaGrilla, null, null);
                        objLog.CrearArchivolog("{farude}idFila" + idFila, null, null);

                        if (i2 > 0 && idFilaGrilla == idFila)
                        {
                            Double dblCFAdi = Funciones.CheckDbl(arrCF[i2].Split('_')[4]);
                            List<BEPlan> objPLan = (List<BEPlan>)HttpContext.Current.Session["objplan" + Funciones.CheckStr(idFila)];
                            
                            
                            objLog.CrearArchivolog("{farude}idPlan" + idPlan, null, null);
                            objLog.CrearArchivolog("{farude}CargoFijoActual" + CargoFijoActual, null, null);

                            foreach (BEPlan item in objPLan)
                            {
                               
                                if (idPlan == item.PLANC_CODIGO)
                                {
                                    dblCargoFijoAdi += dblCFAdi;
                                    objLog.CrearArchivolog("{farude}objPLan[i].PLANN_CAR_FIJ: ANTES" + item.PLANN_CAR_FIJ, null, null);
                                    objLog.CrearArchivolog("{farude}objPLan[i].dblCFAdi: ANTES" + dblCFAdi, null, null);
                                    item.PLANN_CAR_FIJ = dblCargoFijoAdi + CargoFijoActual;
                                    objLog.CrearArchivolog("{farude}objPLan[i].PLANN_CAR_FIJ: DESPUES" + item.PLANN_CAR_FIJ, null, null);
                                    
                                }

                            }
                            HttpContext.Current.Session["objplan" + Funciones.CheckStr(idFila)] = objPLan;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                objLog.CrearArchivolog("{farude}objPLan[i].PLANN_CAR_FIJ: e" + e.Message, null, null);
            }

            objLog.CrearArchivolog("{farude}FINAL------------------------------------", null, null);


            // FIN INC000002464679 


        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod verificarDireccion(string pstrNroDocumento, int pintIdFila)
        {
            GeneradorLog objLog = new GeneradorLog("sisact_ifr_condiciones_venta", null, null, "WEB");
            
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            try
            {
                objResponse.Boleano = false;
                objResponse.Cadena = pintIdFila.ToString();
                objResponse.Mensaje = string.Empty;

                objLog.CrearArchivolog("    verificarDireccion  ", pstrNroDocumento.ToString(), null);
                objLog.CrearArchivolog("    verificarDireccion  ", pintIdFila.ToString(), null);

                if (HttpContext.Current.Session["objDireccion" + pstrNroDocumento] != null)
                {
                    List<BEDireccionCliente> LBEDireccionCliente = (List<BEDireccionCliente>)HttpContext.Current.Session["objDireccion" + pstrNroDocumento];

                    foreach (BEDireccionCliente item in LBEDireccionCliente)
                    {
                        if (item.IdFila == pintIdFila)
                            objResponse.Boleano = true;
                    }
                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[ERROR][verificarDireccion]", null, ex);

                objResponse.Boleano = false;
                objResponse.DescripcionError = ex.Message;
                objResponse.Mensaje = "Error en el proceso, favor intentar nuevamente.";
            }
            objLog.CrearArchivolog("    verificarDireccion/SALIDA  ", pintIdFila.ToString(), null);

            return objResponse;
        }

        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod EliminarCPPortabilidad(string strCodigosPorta)
        {
            BEResponseWebMethod objResponse = new BEResponseWebMethod();            
            GeneradorLog objLog = new GeneradorLog("sisact_ifr_condiciones_venta", null, null, "WEB");
            objLog.CrearArchivolog("[Inicio][EliminarCPPortabilidad][strCodigosPorta]", strCodigosPorta, null);
            //if (!String.IsNullOrEmpty(strCodigosPorta))
            //{
            //    string[] arrCodigosPKPortabilidad = strCodigosPorta.Split('|').Select(s => s).Where(w => !string.IsNullOrEmpty(w)).ToArray();
            //    foreach (string codigo in arrCodigosPKPortabilidad)
            //    {
            //        if (Funciones.CheckInt64(codigo) > 0)
            //        {
            //            objLog.CrearArchivolog("[Fin][BLPortabilidad.EliminarRegistroCPPortabilidad][codigo]", codigo, null);
            //            bool result = Claro.SISACT.Business.BLPortabilidad.EliminarRegistroCPPortabilidad(Funciones.CheckInt64(codigo));
            //            objLog.CrearArchivolog("[Fin][BLPortabilidad.EliminarRegistroCPPortabilidad][result]", result.ToString(), null);
            //        }
            //    }
            //}
            objLog.CrearArchivolog("[Fin][EliminarCPPortabilidad][]", "", null);
            return objResponse;

        }
        // PROY-32129 FASE 2 INICIO
        [System.Web.Services.WebMethod()]
        public static string ValidarAnularDatosAlumInst(string strCampSelect, string strTipoDocumento, string strNroDocumento, string TelefonosSMS)
        {
            GeneradorLog objLog = new GeneradorLog(Sisact_Webbase.CurrentUsers , strNroDocumento, null, "WEB");
            objLog.CrearArchivolog("[INICIO][PROY-32129][ValidarAnularDatosAlumInst]", "", null);
            string strCampanasPermitidas;
            bool bolAnularDatosAlumno = true;
            string listaLineas = string.Empty;
            objLog.CrearArchivolog("[PROY-32129][IN strTipoDocumento: ] ", strCampSelect, null);
            objLog.CrearArchivolog("[PROY-32129][IN strTipoDocumento: ] ", strTipoDocumento, null);
            objLog.CrearArchivolog("[PROY-32129][IN strTipoDocumento: ] ", strNroDocumento, null);
            objLog.CrearArchivolog("[PROY-32129][IN TelefonosSMS: ] ", TelefonosSMS, null);

            /*INC-SMS_PORTA_INI*/
            if (!string.IsNullOrEmpty(TelefonosSMS))
            {
                listaLineas = TelefonosSMS.Replace('|', ';');
                if (string.Equals(listaLineas.Substring(0, 1), ";"))
                    listaLineas = listaLineas.Substring(1, (listaLineas.Length - 1));

                objLog.CrearArchivolog(String.Format("{0} : {1}", "[Session][listTelefono]", listaLineas), string.Empty, null);

                HttpContext.Current.Session["listTelefono"] = listaLineas;
            }
            objLog.CrearArchivolog("[PROY-32129][IN listaLineas: ] ", listaLineas, null);
            /*INC-SMS_PORTA_FIN*/


            List<BEParametro> lstParamCampUniv = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(ConfigurationManager.AppSettings["consGrupoCasoEspecial"].ToString()));
            strCampanasPermitidas = lstParamCampUniv.Where(p => p.Valor1 == "2").SingleOrDefault().Valor;
            string[] arrCampanasSel = strCampSelect.Split('|');
            objLog.CrearArchivolog("[PROY-32129][CampanasPermitidas: ] ", strCampanasPermitidas, null);
            for (int intIndexFila1 = 0; intIndexFila1 < arrCampanasSel.Length; intIndexFila1++)
            {
                if (strCampanasPermitidas.Contains("|" + arrCampanasSel[intIndexFila1] + "|"))
                {
                    bolAnularDatosAlumno = false;
                }
            }

            if (bolAnularDatosAlumno)
            {
                Int64 intCodResp = 0;
                string strMensResp = "";
                bool bolEstadoEjecucion;

                bolEstadoEjecucion = new BLCasoEspecial().AnularDatosAlumno(strTipoDocumento, strNroDocumento, Sisact_Webbase.CurrentUsers, ref intCodResp, ref strMensResp);
                objLog.CrearArchivolog("[PROY-32129][OUT intCodResp: ] ", intCodResp, null);
                objLog.CrearArchivolog("[PROY-32129][OUT strMensResp: ] ", strMensResp, null);
                objLog.CrearArchivolog("[EstadoEjecucion: ]", bolEstadoEjecucion, null);
                objLog.CrearArchivolog("[FIN][PROY-32129][FIN ObtenerCodigosCampEspecUniv]", "", null);
                return intCodResp.ToString() + "|" + strMensResp;
            }
            else
            {
                return "";
            }
        }
        // PROY-32129 FASE 2 FIN

        //PROY-30166-IDEA–38863-INICIO
        public void CargarParametrosClaroUp() 
        {
            objLog.CrearArchivolog("[Inicio][CargarParametrosClaroUp]", null, null);
            hidMontoCuotaMenorA.Value = Comun.AppSettings.consMontoCuotaMenorA;
            objLog.CrearArchivolog(string.Format("{0}{1}","[Inicio][CargarParametrosClaroUp][consMontoCuotaMenorA] ", Comun.AppSettings.consMontoCuotaMenorA), null, null);
            hidMontoCuotaMayorA.Value = Comun.AppSettings.consMontoCuotaMayorA;
            objLog.CrearArchivolog(string.Format("{0}{1}","[Inicio][CargarParametrosClaroUp][consMontoCuotaMayorA] ", Comun.AppSettings.consMontoCuotaMayorA), null, null);
            hidMsjActualizCuotaInicial.Value = Comun.AppSettings.consMsjActualizacionCuotaIncial;
            objLog.CrearArchivolog(string.Format("{0}{1}","[Inicio][CargarParametrosClaroUp][consMsjActualizacionCuotaIncial] ", Comun.AppSettings.consMsjActualizacionCuotaIncial), null, null);
            hidMaxPorcentajeCuotaInicial.Value = Comun.AppSettings.consMaxPorcentajeCuotaInicial;
            objLog.CrearArchivolog(string.Format("{0}{1}","[Inicio][CargarParametrosClaroUp][hidMaxPorcentajeCuotaInicial] ", Comun.AppSettings.consMaxPorcentajeCuotaInicial), null, null);
            hidMontoCuotaMayorAPorcentaje.Value = Comun.AppSettings.consMontoCuotaMayorAPorcentaje;
            objLog.CrearArchivolog(string.Format("{0}{1}","[Inicio][CargarParametrosClaroUp][hidMontoCuotaMayorAPorcentaje] ", Comun.AppSettings.consMontoCuotaMayorAPorcentaje), null, null);

         
            hidMsjMontoCuotaValido.Value = Comun.AppSettings.consMsjMontoCuotaValido;
            objLog.CrearArchivolog(string.Format("{0}{1}", "[Inicio][CargarParametrosClaroUp][hidMsjMontoCuotaValido] ", Comun.AppSettings.consMsjMontoCuotaValido), null, null);
           
            
            objLog.CrearArchivolog("[Fin][CargarParametrosClaroUp]", null, null);
        }
        //PROY-30166-IDEA–38863-FIN
        //PROY-140383-INI
        public void CargarParametrosServvAdi()
        {
            objLog.CrearArchivolog("[Inicio][CargarServvAdicionales]", null, null);
            hidFlagServvExcluyentes.Value = Comun.AppSettings.consFlagServvAdicionales;
            objLog.CrearArchivolog(string.Format("{0}{1}", "[Inicio][CargarParametrosServvAdi][consFlagServvAdicionales] ", Comun.AppSettings.consFlagServvAdicionales), null, null);
            hidMensValidacion.Value = Comun.AppSettings.consMensValidaServ;
            hidServicioExcCaidoifr.Value = Comun.AppSettings.consMensServExcCaido;
            hidProductosExc.Value = Comun.AppSettings.consProdExc;
            objLog.CrearArchivolog("[Fin][CargarParametrosServvAdi]", null, null);
        }
        //PROY-140383-FIN
        //PROY-140383-INI
        [System.Web.Services.WebMethod()]
        public static BEResponseWebMethod ServiciosExcluyentes(string strcodServicioAdi, string strCodServAgre)
        {
            GeneradorLog _objLog = new GeneradorLog(null, "ServiciosExcluyentes", null, "log_ServiciosExcluyentes");
            BEResponseWebMethod objResponse = new BEResponseWebMethod();
            _objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-Servicios Excluyentes] [INICIO]", string.Empty), null, null);
            ValidarServiciosExcluyentesResponse objValidServExcResponse = new ValidarServiciosExcluyentesResponse();
            string strCadServExc;
            string strCodRespExc = "0";
            string strMensRespExc;
            string strCurrentUser = string.Empty;
            string strCurrentTerminal = string.Empty;

            try
            {
                ValidarServiciosExcluyentesRequest objValidServiciosExcRequest = new ValidarServiciosExcluyentesRequest();
                BEAuditoriaRequest objBeAuditoriaRequest = new BEAuditoriaRequest();
                Claro.SISACT.Entity.DataPowerRest.HeaderRequest objHeaderRequest = new Claro.SISACT.Entity.DataPowerRest.HeaderRequest();
                ValidarServExcluRequest objValServExc = new ValidarServExcluRequest();
                RestValidarServiciosExcluyentes objRestValidServEscluyentes = new RestValidarServiciosExcluyentes();
                strCurrentUser = Funciones.CheckStr(Claro.SISACT.Web.Base.Sisact_Webbase.CurrentUsers);
                strCurrentTerminal = Funciones.CheckStr(Claro.SISACT.Web.Base.Sisact_Webbase.CurrentTerminal);


                objBeAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBeAuditoriaRequest.userId = Funciones.CheckStr(ConfigurationManager.AppSettings["constUsuarioAplicacion"]);
                objBeAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");

                objHeaderRequest.consumer = Funciones.CheckStr(ConfigurationManager.AppSettings["constUsuarioAplicacion"]);
                objHeaderRequest.country = Comun.AppSettings.ConsCountryValidServExc;
                objHeaderRequest.dispositivo = Comun.AppSettings.ConsDispositivoValidServExc;
                objHeaderRequest.language = Comun.AppSettings.ConsLanguajeValidServExc;
                objHeaderRequest.modulo = Comun.AppSettings.ConsModuloValidServExc;
                objHeaderRequest.msgType = Comun.AppSettings.ConsMsgtypeValidServExc;
                objHeaderRequest.operation = Comun.AppSettings.ConsOperationValidServExc;
                objHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objHeaderRequest.system = Comun.AppSettings.ConsSystemValidServExc;//system
                objHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                objHeaderRequest.userId = strCurrentUser;
                objHeaderRequest.wsIp = Comun.AppSettings.ConsWsipValidServExc;

                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.consumer : " + objHeaderRequest.consumer.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.country : " + objHeaderRequest.country.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.dispositivo : " + objHeaderRequest.dispositivo.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.language : " + objHeaderRequest.language.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.modulo : " + objHeaderRequest.modulo.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.msgType : " + objHeaderRequest.msgType.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.operation : " + objHeaderRequest.operation.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.pid : " + objHeaderRequest.pid.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.system : " + objHeaderRequest.system.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.timestamp : " + objHeaderRequest.timestamp.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.userId : " + objHeaderRequest.userId.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] -> objHeaderRequest.wsIp : " + objHeaderRequest.wsIp.ToString(), "", null);

                objValidServiciosExcRequest.MessageRequest.header.HeaderRequest = objHeaderRequest;

                objValidServiciosExcRequest.MessageRequest.body.objValidarRequest.strIdServicioAdi = strcodServicioAdi;
                objValidServiciosExcRequest.MessageRequest.body.objValidarRequest.strIdServicioContra = strCodServAgre;

                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] ->Parametros MessageRequest-body-validarCantidadCampaniaRequest: ", "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] ->CodigoTipoDocumento: " + objValidServiciosExcRequest.MessageRequest.body.objValidarRequest.strIdServicioAdi.ToString(), "", null);
                _objLog.CrearArchivolog("[PROY 140383 ServiciosExcluyentes()] ->NumeroDocumento: " + objValidServiciosExcRequest.MessageRequest.body.objValidarRequest.strIdServicioContra.ToString(), "", null);

                string strUsuarioValidencrip = Comun.AppSettings.ConsUsuarioEncripServExc;
                string strContraValidencrip = Comun.AppSettings.ConsContraEncripServExc;

                objValidServExcResponse = objRestValidServEscluyentes.ValidarServiciosExcluyentes(objValidServiciosExcRequest, objBeAuditoriaRequest, strUsuarioValidencrip, strContraValidencrip);

                strCodRespExc = objValidServExcResponse.MessageResponse.body.strCodigoRespuesta;
                strMensRespExc = objValidServExcResponse.MessageResponse.body.mensajeRespuesta;

                if (strCodRespExc.Equals("0"))
                {
                    strCadServExc = objValidServExcResponse.MessageResponse.body.ValidarServicio.codigoServExcluyente;
                    objResponse.Cadena = "1" + "_" + strCadServExc;
                }
                else if (strCodRespExc.Equals("2") || objValidServExcResponse == null)
                {
                    objResponse.Cadena = "0" + "_" ;
                }

            }
            catch (Exception ex)
            {
                objResponse.Boleano = false;
                _objLog.CrearArchivolog("----Validar Servicios Excluyentes----", null, ex);
                objResponse.CodigoError = "-99";
                objResponse.DescripcionError = "Servicio Validacion Excluyentes Caido";
            }
            return objResponse;
        }
        //PROY-140383-FIN

        //INICIO PROY-140419 Autorizar Portabilidad sin PIN
        [System.Web.Services.WebMethod()]
        public static bool ValidarParametroSmsSupervisor(string strCodCanal, string strTipoDoc, string strTipoProd, string strOferta)
        {
            GeneradorLog objLog = new GeneradorLog("sisact_ifr_condiciones_venta", null, null, "WEB");
            bool valida = false;
            int flagSmsPortaSuperv = ReadKeySettings.key_flagSmsPortaSuperv;
            string codigoSinergia = (string)HttpContext.Current.Session["CodigoSINERGIA"];

            try
            {
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][Codigo canal]", strCodCanal), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][Tipo de documento]", strTipoDoc), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][Tipo producto]", strTipoProd), null, null);
                objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][Tipo oferta]", strOferta), null, null);

                if (flagSmsPortaSuperv == 1)
                {
                    objLog.CrearArchivolog("[PROY-140419 Autorizar Portabilidad sin PIN][Flag encendido]", null, null);
                    if (ReadKeySettings.key_SmsPortaSupervCanales.IndexOf(strCodCanal) > -1 &&
                        (ReadKeySettings.key_SmsPortaSupervTipoDoc.IndexOf(strTipoDoc) > -1 || ReadKeySettings.Key_FlagPermitirTodosDocumentos == "1") &&
                        ReadKeySettings.key_SupervTipoProducto.IndexOf(strTipoProd) > -1 &&
                        ReadKeySettings.key_SupervOferta.IndexOf(strOferta) > -1 &&
                        ReadKeySettings.key_CacNoPresencial.IndexOf(codigoSinergia) == -1)
                    {
                        valida = true;
                        objLog.CrearArchivolog("[PROY-140419 Autorizar Portabilidad sin PIN][Se puede solicitar validación de Supervisor]", null, null);
                    }
                    else
                    {
                        objLog.CrearArchivolog("[PROY-140419 Autorizar Portabilidad sin PIN][No se solicita validación de Supervisor]", null, null);
                    }
                }
                else
                {
                    objLog.CrearArchivolog("[PROY-140419 Autorizar Portabilidad sin PIN][Flag apagado]", null, null);
                }

            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0} => ERROR[{1}|{2}]", "[PROY-140419 Autorizar Portabilidad sin PIN][ValidarParametroSmsSupervisor]", Funciones.CheckStr(ex.Message), Funciones.CheckStr(ex.StackTrace)), null, null);
            }
            objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140419 Autorizar Portabilidad sin PIN][OUT]", valida), null, null);
            return valida;
        }
        //FIN PROY-140419 Autorizar Portabilidad sin PIN

        //INI IDEA-142717
        [System.Web.Services.WebMethod()]
        public static string ValidarCampVacunaton(string strCampSelect, string strTipoDoc, string strNroDoc, string catCampanas)
        {
            BLGeneral objConsulta = new BLGeneral();
            GeneradorLog _objLog = new GeneradorLog(null, "ValidarCampVacunaton", null, "log_ValidarCampVacunaton");
            string codRespuesta = string.Empty;
            string msjRespuesta = string.Empty;
            string strResultado = string.Empty;
            string campanasVacunaton = Funciones.CheckStr(Comun.AppSettings.consCampanasVacunaton);
            int count = 0;

            _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142717][Campanas permitidas]", campanasVacunaton), null, null);
            _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142717][Campana seleccionada]", strCampSelect), null, null);

            if (campanasVacunaton.Contains(strCampSelect) && !strCampSelect.Equals(""))
            {
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142717][Consulta a SP: SISACTSS_CAMPANA_VACUNATON][TipoDocumento]", strTipoDoc), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142717][Consulta a SP: SISACTSS_CAMPANA_VACUNATON][NroDocumento]", strNroDoc), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142717][Consulta a SP: SISACTSS_CAMPANA_VACUNATON][catCampanas]", catCampanas), null, null);

                string[] fila = catCampanas.Split('|');
                string codigoCamp = string.Empty;
                for (int i = 0; i <= fila.Length - 1; i++)
                {
                    string codigoCampFila = Funciones.CheckStr(fila[i].Split(';')[0]);

                    if (strCampSelect == codigoCampFila)
                    {
                        count++;
                    }
                }
                if (count <= 1)
                {
                    _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142717][La campana no esta duplicada]", string.Empty), null, null);
                    objConsulta.ValidarCampVacunaton(strTipoDoc, strNroDoc, ref codRespuesta, ref msjRespuesta);
                }
                else 
                {
                    _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142717][Campana seleccionada mas de una vez]", string.Empty), null, null);
                    codRespuesta = "-1";
                    msjRespuesta = "Esta promocion solo se aplica una vez.";
                }                
            }
            else
            {
                codRespuesta = "2";
                msjRespuesta = "Campana no aplica para validacion de Vacunaton";
            }

            _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142717][codRespuesta]", codRespuesta), null, null);
            _objLog.CrearArchivolog(string.Format("{0} => {1}", "[IDEA-142717][msjRespuesta]", msjRespuesta), null, null);

            return strResultado = string.Format("{0}|{1}", codRespuesta, msjRespuesta); ;
        }
        //FIN IDEA-142717

        //INC000004509732 INI
        [System.Web.Services.WebMethod()]
        public static void PintarLogsGenerico(string ticket, string ruta, string valor)
        {
            GeneradorLog _objLog = new GeneradorLog(CurrentUsers, "", null, "WEB");

            _objLog.CrearArchivolog(string.Format("{0}: {1}", "[" + ticket + "]" + ruta, Funciones.CheckStr(valor)), null, null);
        }
        //INC000004509732 FIN

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
        [System.Web.Services.WebMethod()]
        public static string ValidarMaterialAccCuotas(string strCodMaterial)
        {
            RestVentasCuotas rService = new RestVentasCuotas();
            ValidarMaterialAccCuotaResponse objResponse = new ValidarMaterialAccCuotaResponse();
            Dictionary<string, string> dcParameters = new Dictionary<string, string>();
            GeneradorLog _objLog = new GeneradorLog(null, "ValidarMaterialAccCuotas", null, "log_ValidarMaterialAccCuotas");
            string Rpta = "1";

            try
            {
                dcParameters.Add("codigoAccesorio", Funciones.CheckStr(strCodMaterial));

                #region Datos Auditoria
                BEAuditoriaRequest objBeAuditoriaRequest = new BEAuditoriaRequest();
                objBeAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                objBeAuditoriaRequest.userId = Funciones.CheckStr(CurrentUsers);
                objBeAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                objBeAuditoriaRequest.dataPower = true;
                objBeAuditoriaRequest.accept = "application/json";
                objBeAuditoriaRequest.urlTimeOut_Rest = Funciones.CheckStr(ConfigurationManager.AppSettings["ConsMejorasPorta_TimeOut"]);
                objBeAuditoriaRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["conswsipPedidosTienda"]);
                objBeAuditoriaRequest.ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]);
                objBeAuditoriaRequest.usuarioAplicacion = Funciones.CheckStr(CurrentUsers);
                objBeAuditoriaRequest.urlRest = "urlValMatAccCuotas";
                objBeAuditoriaRequest.ipApplication = CurrentServer;
                #endregion

                objResponse = rService.ValidarMaterialAccCuota(dcParameters, objBeAuditoriaRequest);

                string strCodRespuesta = Funciones.CheckStr(objResponse.MessageResponse.Body.validarMaterialAccCuotaResponse.responseStatus.codigoRespuesta);
                string strMsjRespuesta = Funciones.CheckStr(objResponse.MessageResponse.Body.validarMaterialAccCuotaResponse.responseStatus.mensajeRespuesta);

                if (strCodRespuesta.Equals("0"))
                {
                    foreach (BEValidarMaterialAccCuota item in objResponse.MessageResponse.Body.validarMaterialAccCuotaResponse.responseData)
                    { 
                        HttpContext.Current.Session["nomLargoMaterialVV"] = Funciones.CheckStr(item.nomLargoMaterial);
                        _objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas][nomLargoMaterial]", Funciones.CheckStr(item.nomLargoMaterial)), null, null);
                    }
                    Rpta = "0";
                }
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas][Message]", ex.Message), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas][StackTrace]", ex.StackTrace), null, null);
                Rpta = "1";
            }

            return Rpta;
        }

        [System.Web.Services.WebMethod()]
        public static string obtenerLineasCuentas(string codProducto) {

            string rpta = string.Empty;
            GeneradorLog _objLog = new GeneradorLog(null, "obtenerLineasCuentas", null, "log_obtenerLineasCuentas");

            try
            {
                if (codProducto.Equals("01"))
	            {
		            rpta = Funciones.CheckStr(HttpContext.Current.Session["objLineasAsociadas"]);
	            }else
	            {
                    rpta = Funciones.CheckStr(HttpContext.Current.Session["objCuentasAsociadas"]);
	            }

            }
            catch (Exception ex)
            {
                rpta = string.Empty;
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas][Message]", ex.Message), null, null);
                _objLog.CrearArchivolog(string.Format("{0} => {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas][StackTrace]", ex.StackTrace), null, null);
            }

            return rpta;
        }
        #endregion
    }
}
