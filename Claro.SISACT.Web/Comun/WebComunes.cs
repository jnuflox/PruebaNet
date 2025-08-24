using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using System.IO;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.Web;
using Claro.SISACT.WS;
using System.Net.Mail;
using Claro.SISACT.Business;
using System.Collections;
//INICIO PROY-140546
using Claro.SISACT.Web.Base;
using System.Net;
using Claro.SISACT.WS.RestReferences;
using Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Response;
using Claro.SISACT.Entity.VentasCuotas.ValidarCuoAccPendXLinea.Request;
//FIN PROY-140546

namespace Claro.SISACT.Web.Comun
{
    public class WebComunes
    {
        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Validar Nro Telefono]
        public static bool isNroTelefono(string NroTelefono)
        {
            string dig = NroTelefono.Substring(0, 1);
            if (dig.Equals("9"))
                return true;

            return false;
        }
        #endregion

        public static bool EsMayorEdad(string strFechaNac)
        {
            string strFechaHoy = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime dtFechaNac = DateTime.Parse(strFechaNac);
            DateTime dtFechaHoy = DateTime.Parse(strFechaHoy);
            int intEdad = CalcularEdad(strFechaNac);
            int intMayorEdad = Funciones.CheckInt(ConfigurationManager.AppSettings["ConstAnioMayorEdad"].ToString());

            if (intEdad < intMayorEdad)
                return false;

            return true;
        }

        public static int CalcularEdad(string strFechaNac)
        {
            string strFechaHoy = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime dtFechaNac = DateTime.Parse(strFechaNac);
            DateTime dtFechaHoy = DateTime.Parse(strFechaHoy);
            int intEdad = Funciones.CheckInt(dtFechaHoy.Year - dtFechaNac.Year);

            if ((dtFechaHoy.Month < dtFechaNac.Month) || (dtFechaHoy.Month == dtFechaNac.Month && dtFechaHoy.Day < dtFechaNac.Day))
            {
                intEdad -= 1;
            }

            return intEdad;
        }

        public static string obtenerParametro(List<BEItemGenerico> objListaItem, string idParametro)
        {
            string valor = string.Empty;
            if (objListaItem != null)
            {
                foreach (BEItemGenerico objItem in objListaItem)
                {
                    if (objItem.Codigo == idParametro)
                    {
                        valor = Funciones.CheckStr(objItem.Valor);
                        break;
                    }
                }
            }
            return valor;
        }

        public static string tienePerfil(string listaOpciones, string opcion)
        {
            string ok = string.Empty;
            string[] arrOpciones = listaOpciones.Split('|');

            foreach (string obj in arrOpciones)
            {
                if (obj == opcion)
                {
                    ok = "S";
                    break;
                }
            }
            return ok;
        }

        public static string ObtenerIdFilaPaquete(string idFila, string strAgrupaPaquete)
        {
            idFila = string.Format("[{0}]", idFila); //PROY-24740
            strAgrupaPaquete = strAgrupaPaquete.Replace("{", "").Replace(",", "");
            string[] arrAgrupaPaquete = strAgrupaPaquete.Split('}');
            string strIdFilas = string.Empty;

            foreach (string item in arrAgrupaPaquete)
            {
                if (item.IndexOf(idFila) > -1)
                {
                    strIdFilas = item;
                    break;
                }
            }
            return strIdFilas;
        }

        //PROY-24740
        public static bool Auditoria(string strCodTrx, string strDesTrx)
        {
            bool blnOK = false;
            try
            {
                BEUsuarioSession objUsuario = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];

                String nombreHost = System.Net.Dns.GetHostName();
                String nombreServer = System.Net.Dns.GetHostName();
                String ipServer = System.Net.Dns.GetHostAddresses(nombreServer)[0].ToString();
                String usuarioId = objUsuario.idCuentaRed;
                String ipcliente = objUsuario.Terminal;
                String strCodServ = ConfigurationManager.AppSettings["CONS_COD_SACT_SERV"];
                String vMonto = "0";

                blnOK = (new BWAuditoria()).registrarAuditoria(strCodTrx, strCodServ, ipcliente, nombreHost, ipServer, nombreServer, usuarioId, "", vMonto, strDesTrx);
                objUsuario = null;
            }
            catch
            {
                blnOK = false;
            }
            return blnOK;
        }

        public static string EnviarEmail(string vRemitente, string vPara, string vCC, string vBCC, string vAsunto, string vMensaje, string vAdjunto)
        {
            string salida = "";
            System.Net.Mail.MailMessage oMail = new System.Net.Mail.MailMessage();
            oMail.From = new System.Net.Mail.MailAddress(vRemitente);
            oMail.To.Add(vPara);
            oMail.CC.Add(vCC);
            oMail.Bcc.Add(vBCC);
            oMail.Subject = vAsunto;
            oMail.Body = System.Web.HttpContext.Current.Server.HtmlDecode(vMensaje);

            oMail.IsBodyHtml = true;


            try
            {
                string[] arrAdjuntos = vAdjunto.Split(char.Parse("|"));
                foreach (string sArchivo in arrAdjuntos)
                {
                    if (System.IO.File.Exists(sArchivo))
                    {
                        oMail.Attachments.Add(new Attachment(sArchivo));
                    }
                }

                SmtpClient enviar = new SmtpClient();

                enviar.Host = ConfigurationManager.AppSettings["strEmailSmtp"].ToString();
                enviar.Send(oMail);

                salida = "OK";
            }
            catch (Exception ex)
            {
                salida = ex.Message;
            }
            finally
            {
                oMail = null;
            }
            return salida;
        }

        public static string ObtenerParametroGeneral(string CodParametro)
        {
            string listaParametros = null;
            List<BEItemGenerico> arrayListaParametro = new BLGeneral().ListarParametroGeneral("1");
            foreach (BEItemGenerico oItem in arrayListaParametro)
            {
                listaParametros = listaParametros + oItem.Codigo + ";" + Funciones.CheckStr(oItem.Valor) + "|";
            }
            string valor = null;
            string[] arrListaParametro = listaParametros.Split('|');
            foreach (string item in arrListaParametro)
            {
                string[] parametro = item.Split(';');
                if (parametro[0] == CodParametro)
                {
                    valor = parametro[1];
                }
            }
            return valor;
        }

        public static List<BEItemGenerico> ListaTipoDocumento()
        {
            List<BEItemGenerico> listTipoDocCliente = new List<BEItemGenerico>();
            if (HttpContext.Current.Session["listTipoDocCliente"] == null)
            {
                listTipoDocCliente = BLMaestro.ListaTipoDocumento("");
                HttpContext.Current.Session["listTipoDocCliente"] = listTipoDocCliente;
            }
            else
            {
                listTipoDocCliente = (List<BEItemGenerico>)HttpContext.Current.Session["listTipoDocCliente"];
            }
            return listTipoDocCliente;
        }
        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        public BEItemMensaje ActualizarConsultaPrevia(BeConsultaPrevia objConsultaPrevia)
        {
            GeneradorLog objLog = new GeneradorLog("    DASolicitud  ", null, null, "DATA_LOG");            
            BEItemMensaje objMensaje = new BEItemMensaje();

            try
            {
                objLog.CrearArchivolog("*******************************************************************************************", null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}","[Inicio][Actualizar la Consulta Previa]", objConsultaPrevia.numeroSEC),null, null);
                objLog.CrearArchivolog("********************************************************************************************", null, null);

                List<BeConsultaPrevia> listaLineasCP = BLPortabilidad.ObtenerListaSolicitudPortabilidadBySec(objConsultaPrevia.numeroSEC);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Inicio][ObtenerListaSolicitudPortabilidadBySec =>]", listaLineasCP.Count()),null, null);

                if (listaLineasCP.Count > 0)
                {
                    //INI: PROY-140335 RF1
                    string LineasSinCP = string.Empty;
                    string LineasConCP = string.Empty;
                    string strCodRespuesta = string.Empty;
                    string strMsgRespuesta = string.Empty;

                    List<BEPorttSolicitud> lstDetalleCPRepositorio = new List<BEPorttSolicitud>();
                    BEPorttSolicitud DetalleCPLinea = new BEPorttSolicitud();

                    DetalleCPLinea.operadorCedente = objConsultaPrevia.codigoCedente;
                    DetalleCPLinea.modalidadOrigen = objConsultaPrevia.modalidad;
                    DetalleCPLinea.TipoDocumento = Comun.WebComunes.ListaTipoDocumento().Where(w => w.Codigo == objConsultaPrevia.tipoDocumento).FirstOrDefault().Codigo5;
                    DetalleCPLinea.NroDocumento = objConsultaPrevia.numeroDocumento;

                    List<string> lstLineas = listaLineasCP.Select(x => x.msisdn).ToList();
                    string LineasAPortar = Funciones.CheckStr(listaLineasCP.Select(s => s.msisdn).Aggregate((a, b) => a + "," + b));
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Validar la Consulta Previa][Numeros a Portar : ]", LineasAPortar), null, null);

                    foreach (var Linea in lstLineas)
                    {
                        DetalleCPLinea.numeroLinea = Linea;
                        BEPorttSolicitud DetalleCP = BLPortabilidad.ValidarRepositorioABDCP(DetalleCPLinea, ref strCodRespuesta, ref strMsgRespuesta);
                        DetalleCP.flagCPPermitida = 0;
                        DetalleCP.numeroLinea = Linea;
                        lstDetalleCPRepositorio.Add(DetalleCP);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][Consultar Repositorio ABDCP] - INPUT [Linea]=>", Linea), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][Consultar Repositorio ABDCP] - INPUT [codigoCedente]=>", DetalleCPLinea.operadorCedente), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][Consultar Repositorio ABDCP] - INPUT [modalidad]=>", DetalleCPLinea.modalidadOrigen), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][Consultar Repositorio ABDCP] - INPUT [tipoDocumentoABDCP]=>", DetalleCPLinea.TipoDocumento), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][Consultar Repositorio ABDCP] - INPUT [numeroDocumento]=>", DetalleCPLinea.NroDocumento), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][Consultar Repositorio ABDCP] - OUTPUT [IdSecuencialCP]=>", DetalleCP.secuencialCP), null, null);
                    }
                   
                    
                    if (lstDetalleCPRepositorio.Where(w => w.secuencialCP == "").Count() > 0)
                    {
                        LineasSinCP = lstDetalleCPRepositorio.Where(w => w.secuencialCP == "").Select(s => s.numeroLinea).Aggregate((a, b) => a + "," + b);
                    }


                    if (lstDetalleCPRepositorio.Where(w => w.secuencialCP != "").Count() > 0)
                    {
                        LineasConCP = lstDetalleCPRepositorio.Where(w => w.secuencialCP != "").Where(x => x.numeroLinea != LineasSinCP).Select(s => s.numeroLinea).Aggregate((a, b) => a + "," + b);
                    }
                    //FIN: PROY-140335 RF1

                    objConsultaPrevia.msisdn = LineasConCP;
                    objConsultaPrevia.observaciones = Comun.AppSettings.consObservacionActualizarCP;
                    string strNumeroSecuencial = string.Empty;
                    string strCodResp = string.Empty;
                    string strMensResp = string.Empty;

                    BLPortabilidad.ValidarConsultaPreviaSEC(objConsultaPrevia, ref strNumeroSecuencial, ref strCodResp, ref strMensResp); //INI: PROY-140223 IDEA-140462

                    //INI: PROY-140223 IDEA-140462 //CAMBIADO POR EL PROY-140335 RF1
                    bool blConsultaPrevia = lstDetalleCPRepositorio.All(x => x.secuencialCP == ""); 
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][Parametro][blConsultaPrevia]=> ",blConsultaPrevia.ToString()), null, null);
                    if (blConsultaPrevia) //PROY-140335
                    {
                        objConsultaPrevia.msisdn = LineasSinCP;
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][Parametro][LineasSinCP]=> ", LineasSinCP.ToString()), null, null);

                        objLog.CrearArchivolog(string.Format("{0}", "[Inicio de proceso de actualización sin CP]"), null, null);

                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar_SEC_sin_CP][in objConsultaPrevia.numeroSEC: ]", objConsultaPrevia.numeroSEC), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar_SEC_sin_CP][in objConsultaPrevia.auditoria.Codigo2: ]", objConsultaPrevia.auditoria.Codigo2), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar_SEC_sin_CP][out strCodResp: ]", strCodResp), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar_SEC_sin_CP][out strMensResp: ]", strMensResp), null, null);

                        BLPortabilidad.Actualizar_SEC_sin_CP(objConsultaPrevia, ref strCodResp, ref strMensResp);

                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar_SEC_sin_CP][RESPUESTA - out strCodResp: ]", strCodResp), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar_SEC_sin_CP][RESPUESTA - out strMensResp: ]", strMensResp), null, null);
                        objLog.CrearArchivolog(string.Format("{0}", "[Fin de proceso de Consulta Previa Anticipada sin CP]"), null, null);

                        objMensaje.codigo = strCodResp;
                        objMensaje.descripcion = strMensResp;

                     }//FIN: PROY-140223 IDEA-140462 //CAMBIADO POR EL PROY-140335 RF1
                    else
                    {
                        objConsultaPrevia.numeroSecuencial = strNumeroSecuencial;
                        objConsultaPrevia.msisdn = LineasConCP; //PROY-140335 RF1
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][Parametro][LineasConCP]=> ", LineasConCP.ToString()), null, null); //PROY-140335 RF1
                        objLog.CrearArchivolog(string.Format("{0}{1}","[Actualizar la Consulta Previa][numeroSEC : ]",objConsultaPrevia.numeroSEC),null , null);
                        objLog.CrearArchivolog(string.Format("{0}{1}","[Actualizar la Consulta Previa][Numero Secuencial : ]",strNumeroSecuencial),null , null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar la Consulta Previa][strCodResp : ]", Funciones.CheckStr(strCodResp)), null, null); 
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar la Consulta Previa][strMensResp : ]", Funciones.CheckStr(strMensResp)), null, null); 

                        objMensaje = new BWRegistrarPorta().ActualizarConsultaPrevia(objConsultaPrevia);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar la Consulta Previa][objMensaje.codigo: ]", objMensaje.codigo), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar la Consulta Previa][objMensaje.descripcion: ]", objMensaje.descripcion), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar la Consulta Previa][objMensaje.exito: ]", objMensaje.exito), null, null);


                        objLog.CrearArchivolog(string.Format("{0}", "[consReintentosActualizaSEC : " + Funciones.CheckStr(AppSettings.consReintentosActualizaSEC) + " ]"), null, null);

                        //IN000000772139 INI 
                        for (int i = 0; i < Funciones.CheckInt(AppSettings.consReintentosActualizaSEC); i++)
                        {
                            objLog.CrearArchivolog(String.Format("[Actualizar la Consulta Previa][ActualizarConsultaPrevia][Intento]{0}", i), null, null);

                        if (objMensaje.exito != true)
                        {
                            objLog.CrearArchivolog("[Actualizar la Consulta Previa][Ocurrio un error en el Metodo ActualizarConsultaPrevia]", null, null);
                            objLog.CrearArchivolog("[Actualizar la Consulta Previa][Inicio SP de contingencia : SISASU_ACTUALIZA_CP]", null, null);

                            objMensaje = BLPortabilidad.ActualizarConsultaPreviaCP(objConsultaPrevia, ref strCodResp, ref strMensResp);
                            if (objMensaje.exito == true)
                        {
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar la Consulta Previa][Se Actualizo la Consulta Previa ]", objConsultaPrevia.numeroSEC), null,null);
                                    break;
                        }
                        else
                        {
                                objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar la Consulta Previa][No se Actualizo la Consulta Previa ]", objConsultaPrevia.numeroSEC), null, null);
                            }
                        }
                        else
                        {
                            objLog.CrearArchivolog(string.Format("{0}{1}", "[Actualizar la Consulta Previa][Se Actualizo la Consulta Previa ]", objConsultaPrevia.numeroSEC), null, null);
                                break;
                        }
                    }
                        
                        if (objMensaje.exito != true)
                        {
                            objMensaje.mensajeCliente = Comun.AppSettings.consMensajeErrorActualizarCP;
                        }
                        //IN000000772139 FIN
                    }
                    

                    objLog.CrearArchivolog("*******************************************************************************************", null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][Actualizar la Consulta Previa]", objConsultaPrevia.numeroSEC), null, null);
                    objLog.CrearArchivolog("*******************************************************************************************", null, null);

                }
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog("[FIN][Mensaje de Error : ]", ex.Message, null);
                objLog.CrearArchivolog("[FIN][Mensaje de Error : ]", Comun.AppSettings.consMensajeErrorActualizarCP, null);
                objLog.CrearArchivolog("[FIN][Error en el Metodo ActualizarConsultaPrevia]", objConsultaPrevia.numeroSEC, null);

                objLog.CrearArchivolog("[FIN][Mensaje de Error Sistema: ]", Funciones.CheckStr(objMensaje.mensajeSistema), null);

                objLog.CrearArchivolog("*******************************************************************************************", null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[FIN][Actualizar la Consulta Previa]", objConsultaPrevia.numeroSEC), null, null);
                objLog.CrearArchivolog("*******************************************************************************************", null, null);
                objMensaje.mensajeCliente = Comun.AppSettings.consMensajeErrorActualizarCP;
            }

            return objMensaje;
        }

  //INICIO INC000003048070 
        public static void CargarParametroFC()
        {
            GeneradorLog objLog = new GeneradorLog("WebComunes", null, null, "Bono Full Claro");
            List<BEParametro> lstBEParametroFC = new List<BEParametro>();//INC000003048070 
          
            objLog.CrearArchivolog("[Inicio][CargarParametrosFC]", null, null);
            string strCodGrupoParamFC = Funciones.CheckStr(ConfigurationManager.AppSettings["consCodGrupoParamFC"]);

            objLog.CrearArchivolog(string.Format("{0}{1}", "[Inicio][ListaParametrosGrupo][strCodGrupoParamFC] ", strCodGrupoParamFC), null, null);
            if (!string.IsNullOrEmpty(strCodGrupoParamFC))
                lstBEParametroFC = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoParamFC));

            objLog.CrearArchivolog(string.Format("{0}{1}", "[Resultado OK][ListaParametrosGrupo][lstBEParametroFC.Count] ", lstBEParametroFC.Count), null, null);

            if (lstBEParametroFC.Count > 0) lstBEParametroFC = lstBEParametroFC.OrderBy(p => p.Valor1).ToList();


            AppSettings.key_ConstMsjeConBonoFC = lstBEParametroFC.Where(p => p.Valor1 == "Key_msjCandidatoBono").SingleOrDefault().Valor;

            objLog.CrearArchivolog(string.Format("{0}{1}", "[AppSettings.key_ConstMsjeConBonoFC][CargarParametroFC][hidConstMsjeConBonoFC] ", Funciones.CheckStr(AppSettings.key_ConstMsjeConBonoFC)), null, null);

        }
        //FIN INC000003048070 


        public static void CargarAppSettings()
        {
            GeneradorLog objLog = new GeneradorLog("WebComunes", null, null, "Promo2X1"); //PROY-30166-IDEA–38863-INICIO
            objLog.CrearArchivolog("Inicio Cargar Parametros Promo Porta 2X1", null, null); //PROY-30166-IDEA–38863-FIN
            try
            {
                Int64 codigoParametrosCP = Funciones.CheckInt64(ConfigurationManager.AppSettings["ConsParametrosPortabildiadCP"].ToString());
                List<BEParametro> listParametros = new BLGeneral().ListaParametrosGrupo(codigoParametrosCP);

                if (listParametros != null && listParametros.Count() > 0)
                {
                    //INI PROY-CAMPANA LG
                    objLog.CrearArchivolog("[PROMO COMBO LG]INICIO CARGAR PARAMETROS CAMPANA LG", null, null);
                    Int64 key_ParamGrupoLG = 0;
                    key_ParamGrupoLG = Funciones.CheckInt64(ConfigurationManager.AppSettings["codComboLG"]);
                    objLog.CrearArchivolog("[PROMO COMBO LG]key_ParamGrupoLG: " + key_ParamGrupoLG, null, null);
                    List<BEParametro> listParametrosLG = new BLGeneral().ListaParametrosGrupo(key_ParamGrupoLG);
                    if (listParametrosLG != null && listParametrosLG.Count() > 0)
                    {
                        objLog.CrearArchivolog("[PROMO COMBO LG]listParametrosLG: " + listParametrosLG.Count, null, null);

                        ReadKeySettings.keyComboLG_CampanasAutorizadas = listParametrosLG
                         .Where(w => w.Valor1.Equals("keyComboLG_CampanasAutorizadas")).ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLG.Where(w => w.Valor1.Equals("keyComboLG_CampanasAutorizadas")).ToList()[0].Valor) : string.Empty;

                        objLog.CrearArchivolog("[PROMO COMBO LG]keyComboLG_CampanasAutorizadas: " + ReadKeySettings.keyComboLG_CampanasAutorizadas, null, null);

                        ReadKeySettings.keyComboLG_CampanasReglas = listParametrosLG
                         .Where(w => w.Valor1.Equals("keyComboLG_CampanasReglas")).ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLG.Where(w => w.Valor1.Equals("keyComboLG_CampanasReglas")).ToList()[0].Valor) : string.Empty;

                        objLog.CrearArchivolog("[PROMO COMBO LG]keyComboLG_CampanasReglas: " + ReadKeySettings.keyComboLG_CampanasReglas, null, null);

                        ReadKeySettings.keyComboLG_CampanaMensaje_ClienteConExistencia = listParametrosLG
                         .Where(w => w.Valor1.Equals("keyComboLG_CampanaMensaje_ClienteConExistencia")).ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLG.Where(w => w.Valor1.Equals("keyComboLG_CampanaMensaje_ClienteConExistencia")).ToList()[0].Valor) : string.Empty;

                        objLog.CrearArchivolog("[PROMO COMBO LG]keyComboLG_CampanaMensaje_ClienteConExistencia: " + ReadKeySettings.keyComboLG_CampanaMensaje_ClienteConExistencia, null, null);

                        ReadKeySettings.keyComboLG_CampanaMensaje_ClienteNoAplica = listParametrosLG
                         .Where(w => w.Valor1.Equals("keyComboLG_CampanaMensaje_ClienteNoAplica")).ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLG.Where(w => w.Valor1.Equals("keyComboLG_CampanaMensaje_ClienteNoAplica")).ToList()[0].Valor) : string.Empty;

                        objLog.CrearArchivolog("[PROMO COMBO LG]keyComboLG_CampanaMensaje_ClienteNoAplica: " + ReadKeySettings.keyComboLG_CampanaMensaje_ClienteNoAplica, null, null);

                        List<BEItemGenericoLG> _list_CampanasReglas = new List<BEItemGenericoLG>();
                        _list_CampanasReglas.AddRange(ReadKeySettings.keyComboLG_CampanasReglas.Split('|').Select(s => new BEItemGenericoLG() { Codigo = s.ToString().Split(';')[0], GamaAlta = s.ToString().Split(';')[1], GamaBaja = s.ToString().Split(';')[2], Umbral = s.ToString().Split(';')[3] }));
                        ReadKeySettings.list_CampanasReglas = _list_CampanasReglas;

                        objLog.CrearArchivolog("[PROMO COMBO LG]_list_CampanasReglas: " + _list_CampanasReglas.Count, null, null);
                    }
                    else
                    {
                        objLog.CrearArchivolog("listParametrosLG: nulo", null, null);
                    }
                    objLog.CrearArchivolog("[PROMO COMBO LG]FIN CARGAR PARAMETROS CAMPANA LG", null, null);
                    //FIN PROY-CAMPANA LG

                    AppSettings.consFrecuenciaConsultaCP = listParametros
                        .Where(w => w.Descripcion.Contains("[consFrecuenciaConsultaCP]")).ToList().Count > 0 ?
                        Funciones.CheckInt(listParametros.Where(w => w.Descripcion.Contains("[consFrecuenciaConsultaCP]")).ToList()[0].Valor) : 0;

                    AppSettings.consMsgConsultaPreviaPendienteABCDP = listParametros
                        .Where(w => w.Descripcion.Contains("[consMsgConsultaPreviaPendienteABCDP]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consMsgConsultaPreviaPendienteABCDP]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consMsgConsultaPreviaServicioNoRespondio = listParametros
                        .Where(w => w.Descripcion.Contains("[consMsgConsultaPreviaServicioNoRespondio]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consMsgConsultaPreviaServicioNoRespondio]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consProductosConsultaCP = listParametros
                        .Where(w => w.Descripcion.Contains("[consProductosConsultaCP]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consProductosConsultaCP]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consReintentosConsultaCP = listParametros
                        .Where(w => w.Descripcion.Contains("[consReintentosConsultaCP]")).ToList().Count > 0 ?
                        Funciones.CheckInt(listParametros.Where(w => w.Descripcion.Contains("[consReintentosConsultaCP]")).ToList()[0].Valor) : 0;

                    AppSettings.consTipoModalidadCPPortIN = listParametros
                        .Where(w => w.Descripcion.Contains("[consTipoModalidadCPPortIN]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consTipoModalidadCPPortIN]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consTipoPortabilidadPortIN = listParametros
                        .Where(w => w.Descripcion.Contains("[consTipoPortabilidadPortIN]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consTipoPortabilidadPortIN]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consTipoServicioConsultaCP = listParametros
                        .Where(w => w.Descripcion.Contains("[consTipoServicioConsultaCP]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consTipoServicioConsultaCP]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consMensajeTimeOutRegistraPortaWS = listParametros
                        .Where(w => w.Descripcion.Contains("[consMensajeTimeOutRegistraPortaWS]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consMensajeTimeOutRegistraPortaWS]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consMsgConsultaPreviaExitosa = listParametros
                        .Where(w => w.Descripcion.Contains("[consMsgConsultaPreviaExitosa]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consMsgConsultaPreviaExitosa]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consObservacionActualizarCP = listParametros
                        .Where(w => w.Descripcion.Contains("[consObservacionActualizarCP]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consObservacionActualizarCP]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consObservacionRealizarCP = listParametros
                        .Where(w => w.Descripcion.Contains("[consObservacionRealizarCP]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consObservacionRealizarCP]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consMensajeErrorActualizarCP = listParametros
                        .Where(w => w.Descripcion.Contains("[consMensajeErrorActualizarCP]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consMensajeErrorActualizarCP]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consEstadosCPPermitidos = listParametros
                        .Where(w => w.Descripcion.Contains("[consEstadosCPPermitidos]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consEstadosCPPermitidos]")).ToList()[0].Valor) : string.Empty;

                    AppSettings.consMensajesCPCarrito = listParametros
                        .Where(w => w.Descripcion.Contains("[consMensajesCPCarrito]")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametros.Where(w => w.Descripcion.Contains("[consMensajesCPCarrito]")).ToList()[0].Valor) : string.Empty;
                    
                     AppSettings.consReintentosActualizaSEC = listParametros
                        .Where(w => w.Descripcion.Contains("[consReintentosActualizaSEC]")).ToList().Count > 0 ?
                        Funciones.CheckInt(listParametros.Where(w => w.Descripcion.Contains("[consReintentosConsultaCP]")).ToList()[0].Valor) : 0;
                    
                    //INI: PROY-BLACKOUT       
                    AppSettings.consFlagBlackOut = listParametros.Where(w => w.Valor1.Equals("consFlagBlackOut")).ToList().Count > 0 ?
                        Funciones.CheckInt(listParametros.Where(w => w.Valor1.Equals("consFlagBlackOut")).ToList()[0].Valor) : 0;
                     
                     AppSettings.consMensajeEvaluacionCACBlackOut = listParametros.Where(w => w.Valor1.Equals("consMensajeEvaluacionCACBlackOut")).ToList().Count > 0 ?
                          Funciones.CheckStr(listParametros.Where(w => w.Valor1.Equals("consMensajeEvaluacionCACBlackOut")).ToList()[0].Valor) : string.Empty;
                          
                     AppSettings.consMensajeCPExitosoBlackOut = listParametros.Where(w => w.Valor1.Equals("consMensajeCPExitosoBlackOut")).ToList().Count > 0 ?
                          Funciones.CheckStr(listParametros.Where(w => w.Valor1.Equals("consMensajeCPExitosoBlackOut")).ToList()[0].Valor) : string.Empty;
                    //FIN: PROY-BLACKOUT
                }

                //INI IDEA-142717
                Int64 codParanGrupoCampVacunaton = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_codParanGrupoCampVacunaton"]);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[codParanGrupoCampVacunaton]", Funciones.CheckStr(codParanGrupoCampVacunaton)), null, null);
                List<BEParametro> lstParamCampVacunaton = new BLGeneral().ListaParametrosGrupo(codParanGrupoCampVacunaton);

                if (lstParamCampVacunaton != null && lstParamCampVacunaton.Count() > 0)
                {
                    AppSettings.consCampanasVacunaton = lstParamCampVacunaton.Where(w => w.Valor1 == "KEY_CAMPANAS_PERMITIDAS").ToList().Count > 0 ?
                    Funciones.CheckStr(lstParamCampVacunaton.Where(w => w.Valor1 == "KEY_CAMPANAS_PERMITIDAS").ToList()[0].Valor) : string.Empty;
                }                
                //FIN IDEA-142717

                //INI PROY-140739 Formulario Leads
                Int64 codParanGrupoLeads = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_codParanGrupoLeads"]);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[codParanGrupoCampVacunaton]", Funciones.CheckStr(codParanGrupoCampVacunaton)), null, null);
                List<BEParametro> lstParamLeads = new BLGeneral().ListaParametrosGrupo(codParanGrupoLeads);

                if (lstParamLeads != null && lstParamLeads.Count() > 0)
                {
                    ReadKeySettings.KeyLeadsCanal = lstParamLeads.Where(w => w.Valor1 == "KeyLeadsCanal").ToList().Count > 0 ?
                    Funciones.CheckStr(lstParamLeads.Where(w => w.Valor1 == "KeyLeadsCanal").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.KeyLeadsPDV = lstParamLeads.Where(w => w.Valor1 == "KeyLeadsPDV").ToList().Count > 0 ?
                    Funciones.CheckStr(lstParamLeads.Where(w => w.Valor1 == "KeyLeadsPDV").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.KeyLeadsTopenPostPago = lstParamLeads.Where(w => w.Valor1 == "KeyLeadsTopenPostPago").ToList().Count > 0 ?
                    Funciones.CheckStr(lstParamLeads.Where(w => w.Valor1 == "KeyLeadsTopenPostPago").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.KeyLeadsFlag = lstParamLeads.Where(w => w.Valor1 == "KeyLeadsFlag").ToList().Count > 0 ?
                    Funciones.CheckStr(lstParamLeads.Where(w => w.Valor1 == "KeyLeadsFlag").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.KeyLeadsMensajeObligatorio = lstParamLeads.Where(w => w.Valor1 == "KeyLeadsMensajeObligatorio").ToList().Count > 0 ?
                    Funciones.CheckStr(lstParamLeads.Where(w => w.Valor1 == "KeyLeadsMensajeObligatorio").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.KeyLeadsMaxLength = lstParamLeads.Where(w => w.Valor1 == "KeyLeadsMaxLength").ToList().Count > 0 ?
                    Funciones.CheckStr(lstParamLeads.Where(w => w.Valor1 == "KeyLeadsMaxLength").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.KeyLeadsProductosPermitidosPostpago = lstParamLeads.Where(w => w.Valor1 == "KeyLeadsProductosPermitidosPostpago").ToList().Count > 0 ?
                    Funciones.CheckStr(lstParamLeads.Where(w => w.Valor1 == "KeyLeadsProductosPermitidosPostpago").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.KeyLeadsMensajeError = lstParamLeads.Where(w => w.Valor1 == "KeyLeadsMensajeError").ToList().Count > 0 ?
                    Funciones.CheckStr(lstParamLeads.Where(w => w.Valor1 == "KeyLeadsMensajeError").ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[KeyLeadsCanal]", Funciones.CheckStr(ReadKeySettings.KeyLeadsCanal)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[KeyLeadsPDV]", Funciones.CheckStr(ReadKeySettings.KeyLeadsPDV)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[KeyLeadsTopenPostPago]", Funciones.CheckStr(ReadKeySettings.KeyLeadsTopenPostPago)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[KeyLeadsFlag]", Funciones.CheckStr(ReadKeySettings.KeyLeadsFlag)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[KeyLeadsMensajeObligatorio]", Funciones.CheckStr(ReadKeySettings.KeyLeadsMensajeObligatorio)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[KeyLeadsMaxLength]", Funciones.CheckStr(ReadKeySettings.KeyLeadsMaxLength)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[KeyLeadsProductosPermitidosPostpago]", Funciones.CheckStr(ReadKeySettings.KeyLeadsProductosPermitidosPostpago)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[KeyLeadsMensajeError]", Funciones.CheckStr(ReadKeySettings.KeyLeadsMensajeError)), null, null);
                }
                //FIN PROY-140739 Formulario Leads

                Int64 codigoParamPromocionPorta2x1 = Funciones.CheckInt64(ConfigurationManager.AppSettings["codigoParamPromocionPorta2x1"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[Codigo de Paran_Grupo]", Funciones.CheckStr(codigoParamPromocionPorta2x1)), null, null);

                List<BEItemGenerico> _listMaterialesPromoPorta = new List<BEItemGenerico>();
                List<BEParametro> listParametrosPromo = new BLGeneral().ListaParametrosGrupo(codigoParamPromocionPorta2x1);

                objLog.CrearArchivolog(string.Format("{0}{1}", "[Cantidad de Parametros ]", Funciones.CheckStr(listParametrosPromo.Count())), null, null);
              //promo 2x1 rs_02
                if (listParametrosPromo != null && listParametrosPromo.Count() > 0)
                {
                    AppSettings.consflagVigenciaPromo2x1 = listParametrosPromo
                      .Where(w => w.Valor1 == "consflagVigenciaPromo2x1").ToList().Count > 0 ?
                      Funciones.CheckStr(listParametrosPromo.Where(w => w.Valor1 == "consflagVigenciaPromo2x1").ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[consflagVigenciaPromo2x1]",  AppSettings.consflagVigenciaPromo2x1), null, null);

                    listParametrosPromo.Where(w => w.Valor1.Contains(AppSettings.consGrupoPromoPorta2x1)).Select(
                            (s => new BEItemGenerico() { Valor = s.Valor1.Replace(AppSettings.consGrupoPromoPorta2x1,""), Descripcion = s.Valor })
                        ).OrderBy(s => s.Valor).ToList().ForEach(ig=>
                           _listMaterialesPromoPorta
                       .AddRange(ig.Descripcion.Split(';')
                      .Select(s => new BEItemGenerico() { Codigo = ig.Valor, Descripcion = s.PadLeft(18, '0') })
                       .ToList())

                        );

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[listParametrosPromo]", Funciones.CheckStr(_listMaterialesPromoPorta.Count())), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[listParametrosPromo]", Funciones.CheckStr(listParametrosPromo.Count())), null, null);

                    AppSettings.listMaterialesPromoPorta = _listMaterialesPromoPorta;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[listMaterialesPromoPorta]", Funciones.CheckStr(AppSettings.listMaterialesPromoPorta.Count())), null, null);

               //promo 2x1 rs_02
                    AppSettings.consConfiguracionPromo2x1 = listParametrosPromo
                      .Where(w => w.Valor1 == "consConfiguracionPromo2x1").ToList().Count > 0 ?
                      Funciones.CheckStr(listParametrosPromo.Where(w => w.Valor1 == "consConfiguracionPromo2x1").ToList()[0].Valor) : string.Empty;

                    AppSettings.consPromoPortaErrorCantidadLineas = listParametrosPromo
                      .Where(w => w.Valor1 == "consPromoPortaErrorCantidadLineas").ToList().Count > 0 ?
                      Funciones.CheckStr(listParametrosPromo.Where(w => w.Valor1 == "consPromoPortaErrorCantidadLineas").ToList()[0].Valor) : string.Empty;

                    AppSettings.consPromoPortaErrorCampanias = listParametrosPromo
                      .Where(w => w.Valor1 == "consPromoPortaErrorCampanias").ToList().Count > 0 ?
                      Funciones.CheckStr(listParametrosPromo.Where(w => w.Valor1 == "consPromoPortaErrorCampanias").ToList()[0].Valor) : string.Empty;

                    AppSettings.consPromoPortaErrorPlanes = listParametrosPromo
                      .Where(w => w.Valor1 == "consPromoPortaErrorPlanes").ToList().Count > 0 ?
                      Funciones.CheckStr(listParametrosPromo.Where(w => w.Valor1 == "consPromoPortaErrorPlanes").ToList()[0].Valor) : string.Empty;

                    AppSettings.consPromoPortaErrorGrupoMaterial = listParametrosPromo
                      .Where(w => w.Valor1 == "consPromoPortaErrorGrupoMaterial").ToList().Count > 0 ?
                      Funciones.CheckStr(listParametrosPromo.Where(w => w.Valor1 == "consPromoPortaErrorGrupoMaterial").ToList()[0].Valor) : string.Empty;

                    AppSettings.consPromoPortaErrorFueradeAlcance = listParametrosPromo
                      .Where(w => w.Valor1 == "consPromoPortaErrorFueradeAlcance").ToList().Count > 0 ?
                      Funciones.CheckStr(listParametrosPromo.Where(w => w.Valor1 == "consPromoPortaErrorFueradeAlcance").ToList()[0].Valor) : string.Empty;


                    objLog.CrearArchivolog(string.Format("{0}{1}", "[consConfiguracionPromo2x1]", Funciones.CheckStr(AppSettings.consConfiguracionPromo2x1)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[consPromoPortaErrorCantidadLineas]", Funciones.CheckStr(AppSettings.consPromoPortaErrorCantidadLineas)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[consPromoPortaErrorCampanias]", Funciones.CheckStr(AppSettings.consPromoPortaErrorCampanias)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[consPromoPortaErrorPlanes]", Funciones.CheckStr(AppSettings.consPromoPortaErrorPlanes)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[consPromoPortaErrorGrupoMaterial]", Funciones.CheckStr(AppSettings.consPromoPortaErrorGrupoMaterial)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[consPromoPortaErrorFueradeAlcance]", Funciones.CheckStr(AppSettings.consPromoPortaErrorFueradeAlcance)), null, null);


                    //INI: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ
                    Int64 codigoParamPromocionPorta50Dscto = Funciones.CheckInt64(ConfigurationManager.AppSettings["codigoParamPromocionPorta50Dscto"].ToString());
                    List<BEParametro> listParametrosPromo50Dscto = new BLGeneral().ListaParametrosGrupo(codigoParamPromocionPorta50Dscto);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[Codigo de Paran_Grupo]", Funciones.CheckStr(codigoParamPromocionPorta50Dscto)), null, null);

                    AppSettings.consCodigoCampaniaPorta50Dscto = listParametrosPromo50Dscto
                        .Where(w => w.Valor1 == "consCodigoCampaniaPorta50Dscto").ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosPromo50Dscto.Where(w => w.Valor1 == "consCodigoCampaniaPorta50Dscto").ToList()[0].Valor) : string.Empty;

                    AppSettings.consNroDiasPermitidosOP = listParametrosPromo50Dscto
                        .Where(w => w.Valor1 == "consNroDiasPermitidosOP").ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosPromo50Dscto.Where(w => w.Valor1 == "consNroDiasPermitidosOP").ToList()[0].Valor) : string.Empty;

                    AppSettings.consMsgPermanenciaOP = listParametrosPromo50Dscto
                        .Where(w => w.Valor1 == "consMsgPermanenciaOP").ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosPromo50Dscto.Where(w => w.Valor1 == "consMsgPermanenciaOP").ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[consCodigoCampaniaPorta50Dscto]", Funciones.CheckStr(AppSettings.consCodigoCampaniaPorta50Dscto)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[consNroDiasPermitidosOP]", Funciones.CheckStr(AppSettings.consNroDiasPermitidosOP)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[consMsgPermanenciaOP]", Funciones.CheckStr(AppSettings.consMsgPermanenciaOP)), null, null);

                    //FIN: 2014 Campaña PORTABILIDAD 50% DSCTO - RMZ

                }


                //INI: PROY-140223 IDEA-140462
                Int64 codigo_paran_grupo = Funciones.CheckInt64(ConfigurationManager.AppSettings["CodParametroInConsultaPrevia"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY - 31583][PARAN_GRUPO]", Funciones.CheckStr(codigo_paran_grupo)), null, null);
                List<BEParametro> lista_valores = new BLGeneral().ListaParametrosGrupo(codigo_paran_grupo);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY - 31583][RESULTADO]", Funciones.CheckStr(lista_valores.Count())), null, null);

                AppSettings.consFlagConsultaPreviaChip = lista_valores.Where(w => w.Valor1 == "consFlagConsultaPreviaChip").ToList().Count > 0 ?
                Funciones.CheckStr(lista_valores.Where(w => w.Valor1 == "consFlagConsultaPreviaChip").ToList()[0].Valor) : string.Empty;
                AppSettings.consCPModdalidadVenta = lista_valores.Where(w => w.Valor1 == "consCPModdalidadVenta").ToList().Count > 0 ?
               Funciones.CheckStr(lista_valores.Where(w => w.Valor1 == "consCPModdalidadVenta").ToList()[0].Valor) : string.Empty;
                AppSettings.consCPCanalVenta = lista_valores.Where(w => w.Valor1 == "consCPCanalVenta").ToList().Count > 0 ?
               Funciones.CheckStr(lista_valores.Where(w => w.Valor1 == "consCPCanalVenta").ToList()[0].Valor) : string.Empty;
                AppSettings.consMensajeCPApagada = lista_valores.Where(w => w.Valor1 == "consMensajeCPApagada").ToList().Count > 0 ?
                Funciones.CheckStr(lista_valores.Where(w => w.Valor1 == "consMensajeCPApagada").ToList()[0].Valor) : string.Empty;
                AppSettings.consClasificacionPDV = lista_valores.Where(w => w.Valor1 == "consClasificacionPDV").ToList().Count > 0 ?
                 Funciones.CheckStr(lista_valores.Where(w => w.Valor1 == "consClasificacionPDV").ToList()[0].Valor) : string.Empty;
                
                
                AppSettings.consCPPuntoVenta = new BLPortabilidad().Obtener_Class_PDV(AppSettings.consClasificacionPDV);
                ReadKeySettings.Key_ConsCPPuntoVenta = AppSettings.consCPPuntoVenta; //PROY-140335 - INICIO EJRC
                

                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY - 140223][FLAG]", Funciones.CheckStr(AppSettings.consFlagConsultaPreviaChip)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY - 140223][consCPModdalidadVenta]", Funciones.CheckStr(AppSettings.consCPCanalVenta)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY - 140223][consCPCanalVenta]", Funciones.CheckStr(AppSettings.consCPModdalidadVenta)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY - 140223][consMensajeCPApagada]", Funciones.CheckStr(AppSettings.consMensajeCPApagada)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY - 140223][consCPPuntoVenta]", Funciones.CheckStr(AppSettings.consCPPuntoVenta)), null, null);
                //INI: PROY-140223 IDEA-140462

                CargarParametrosFlexibilizacion();//PROY-29121

                /*PROY-31636 - RENTESEG Inicio*/

                //VALIDACION FRAUDE - INI
                objLog.CrearArchivolog("Inicio IDEA-141448 - VALIDACION FRAUDE", null, null);
                Int64 key_ParanGrupoValidacionesFraude = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoValidacionesFraude"]);
                List<BEParametro> listParametrosFraude = new BLGeneral().ListaParametrosGrupo(key_ParanGrupoValidacionesFraude);

                objLog.CrearArchivolog(string.Format("Se cargaron {0} parametros", Funciones.CheckStr(listParametrosFraude.Count())), null, null);
                if (listParametrosFraude != null && listParametrosFraude.Count() > 0)
                {
                    ReadKeySettings.Key_MensajeResponse_Venta = listParametrosFraude
                        .Where(w => w.Valor1.Equals("Key_MensajeResponse_Venta")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosFraude.Where(w => w.Valor1.Equals("Key_MensajeResponse_Venta")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_MensajeResponse_Evaluacion = listParametrosFraude
                       .Where(w => w.Valor1.Equals("Key_MensajeResponse_Evaluacion")).ToList().Count > 0 ?
                       Funciones.CheckStr(listParametrosFraude.Where(w => w.Valor1.Equals("Key_MensajeResponse_Evaluacion")).ToList()[0].Valor) : string.Empty;
                }
                //VALIDACION FRAUDE - FIN


                objLog.CrearArchivolog("Inicio PROY-31636 Carga de Parámetros", null, null);

                Int64 key_ParanGrupoRenteseg = 0;

                key_ParanGrupoRenteseg = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoRenteseg"].ToString());
                List<BEParametro> listParametrosRenteseg = new BLGeneral().ListaParametrosGrupo(key_ParanGrupoRenteseg);

                objLog.CrearArchivolog(String.Format("Se cargaron {0} parámetros", Funciones.CheckStr(listParametrosRenteseg.Count())), null, null);

                if (listParametrosRenteseg != null && listParametrosRenteseg.Count() > 0)
                {

                    ReadKeySettings.Key_codigoDocMigratorios = listParametrosRenteseg
                     .Where(w => w.Valor1.Equals("Key_codigoDocMigratorios")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_codigoDocMigratorios")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_codigoDocMigraYPasaporte = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_codigoDocMigraYPasaporte")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_codigoDocMigraYPasaporte")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_codDocMigra_Pasaporte_CE = listParametrosRenteseg
                     .Where(w => w.Valor1.Equals("Key_codDocMigra_Pasaporte_CE")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_codDocMigra_Pasaporte_CE")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_codigoDocCIRE = listParametrosRenteseg
                     .Where(w => w.Valor1.Equals("Key_codigoDocCIRE")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_codigoDocCIRE")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_codigoDocCIE = listParametrosRenteseg
                     .Where(w => w.Valor1.Equals("Key_codigoDocCIE")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_codigoDocCIE")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_codigoDocCPP = listParametrosRenteseg
                     .Where(w => w.Valor1.Equals("Key_codigoDocCPP")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_codigoDocCPP")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_codigoDocCTM = listParametrosRenteseg
                     .Where(w => w.Valor1.Equals("Key_codigoDocCTM")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_codigoDocCTM")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_minLengthDocMigratorios = listParametrosRenteseg
                     .Where(w => w.Valor1.Equals("Key_minLengthDocMigratorios")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_minLengthDocMigratorios")).ToList()[0].Valor) : string.Empty;

                    //INC000003430042 - INICIO
                    ReadKeySettings.Key_minLengthDocPass = listParametrosRenteseg
                     .Where(w => w.Valor1.Equals("Key_minLengthDocPass")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_minLengthDocPass")).ToList()[0].Valor) : string.Empty;
                    //INC000003430042 - FIN

                    ReadKeySettings.Key_maxLengthDocMigratorios = listParametrosRenteseg
                     .Where(w => w.Valor1.Equals("Key_maxLengthDocMigratorios")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_maxLengthDocMigratorios")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_codigoDocPasaporte07 = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_codigoDocPasaporte07")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_codigoDocPasaporte07")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_codigoDocPasaporte08 = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_codigoDocPasaporte08")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_codigoDocPasaporte08")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_flagPermitirProductosLTE = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_flagPermitirProductosLTE")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_flagPermitirProductosLTE")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_docsExistEvaluacionPost = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_docsExistEvaluacionPost")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_docsExistEvaluacionPost")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_tipoDocPermitidoPostCAC = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_tipoDocPermitidoPostCAC")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_tipoDocPermitidoPostCAC")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_tipoDocPermitidoPostDAC = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_tipoDocPermitidoPostDAC")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_tipoDocPermitidoPostDAC")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_tipoDocPermitidoPostCAD = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_tipoDocPermitidoPostCAD")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_tipoDocPermitidoPostCAD")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_codigoDocClienUNI = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_codigoDocClienUNI")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_codigoDocClienUNI")).ToList()[0].Valor) : string.Empty;

                    //INC000002245048-INICIO
                    ReadKeySettings.Key_FlagActivacionTopeConsumoAlta = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_FlagActivacionTopeConsumoAlta")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_FlagActivacionTopeConsumoAlta")).ToList()[0].Valor) : string.Empty;
                    //INC000002245048-FIN

                    //INC000002396378
                    ReadKeySettings.Key_EstadoSecRechazado = listParametrosRenteseg
                  .Where(w => w.Valor1.Equals("Key_EstadoSecRechazado")).ToList().Count > 0 ?
                  Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_EstadoSecRechazado")).ToList()[0].Valor) : string.Empty;
                    //INC000002396378

                    //INC000003013199 - INI
                    ReadKeySettings.Key_ReintentosRegistroRRLL = listParametrosRenteseg
                    .Where(w => w.Valor1.Equals("Key_ReintentosRegistroRRLL")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_ReintentosRegistroRRLL")).ToList()[0].Valor) : string.Empty;
                    //INC000003013199 - FIN

                    //INC000003443673
                    ReadKeySettings.Key_ValorSinApellidoPaternoOMaterno = listParametrosRenteseg
                  .Where(w => w.Valor1.Equals("Key_ValorSinApellidoPaternoOMaterno")).ToList().Count > 0 ?
                  Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_ValorSinApellidoPaternoOMaterno")).ToList()[0].Valor) : string.Empty;
                    //INC000003443673

                    //INC000003910770 - INI
                    ReadKeySettings.Key_TipoDocumentoBSCS7 = listParametrosRenteseg
                  .Where(w => w.Valor1.Equals("Key_TipoDocumentoBSCS7")).ToList().Count > 0 ?
                  Funciones.CheckStr(listParametrosRenteseg.Where(w => w.Valor1.Equals("Key_TipoDocumentoBSCS7")).ToList()[0].Valor) : string.Empty;
                    //INC000003910770 - FIN

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_codigoDocMigratorios] => ", ReadKeySettings.Key_codigoDocMigratorios), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_codigoDocMigraYPasaporte] => ", ReadKeySettings.Key_codigoDocMigraYPasaporte), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_codDocMigra_Pasaporte_CE] => ", ReadKeySettings.Key_codDocMigra_Pasaporte_CE), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_codigoDocCIRE] => ", ReadKeySettings.Key_codigoDocCIRE), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_codigoDocCIE] => ", ReadKeySettings.Key_codigoDocCIE), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_codigoDocCPP] => ", ReadKeySettings.Key_codigoDocCPP), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_codigoDocCTM] => ", ReadKeySettings.Key_codigoDocCTM), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_minLengthDocMigratorios] => ", ReadKeySettings.Key_minLengthDocMigratorios), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_minLengthDocPass] => ", ReadKeySettings.Key_minLengthDocPass), null, null); //INC000003430042
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_maxLengthDocMigratorios] => ", ReadKeySettings.Key_maxLengthDocMigratorios), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_codigoDocPasaporte07] => ", ReadKeySettings.Key_codigoDocPasaporte07), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_codigoDocPasaporte08] => ", ReadKeySettings.Key_codigoDocPasaporte08), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_flagPermitirProductosLTE] => ", ReadKeySettings.Key_flagPermitirProductosLTE), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_docsExistEvaluacionPost] => ", ReadKeySettings.Key_docsExistEvaluacionPost), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_tipoDocPermitidoPostCAC] => ", ReadKeySettings.Key_tipoDocPermitidoPostCAC), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_tipoDocPermitidoPostDAC] => ", ReadKeySettings.Key_tipoDocPermitidoPostDAC), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_tipoDocPermitidoPostCAD] => ", ReadKeySettings.Key_tipoDocPermitidoPostCAD), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-31636][ReadKeySettings.Key_codigoDocClienUNI] => ", ReadKeySettings.Key_codigoDocClienUNI), null, null);
                    objLog.CrearArchivolog("Fin PROY-31636 Carga de Parámetros", null, null);
                    //INC000002245048-INICIO
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002245048][ReadKeySettings.Key_FlagActivacionTopeConsumoAlta] => ", ReadKeySettings.Key_FlagActivacionTopeConsumoAlta), null, null);
                    //INC000002245048-FIN
                    //INC000002396378-INICIO
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002396378][ReadKeySettings.Key_EstadoSecRechazado] => ", ReadKeySettings.Key_EstadoSecRechazado), null, null);
                    //INC000002396378-FIN
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000003013199][ReadKeySettings.Key_ReintentosRegistroRRLL] => ", ReadKeySettings.Key_ReintentosRegistroRRLL), null, null); //INC000003013199

                    //INC000003443673-INICIO
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000003443673][ReadKeySettings.Key_ValorSinApellidoPaternoOMaterno] => ", ReadKeySettings.Key_ValorSinApellidoPaternoOMaterno), null, null);
                    //INC000003443673-FIN

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000003910770][ReadKeySettings.Key_TipoDocumentoBSCS7] => ", ReadKeySettings.Key_TipoDocumentoBSCS7), null, null); //INC000003910770
                }
                /*PROY-31636 - RENTESEG Fin*/

                //PROY-30166-IDEA–38863-INICIO
                objLog.CrearArchivolog(" === INICIO Cargar Parametros Claro UP ====", null, null);

                Int64 codigoParametrosClaroUP = Funciones.CheckInt64(ConfigurationManager.AppSettings["CodGrupoParamClaroUp"].ToString());
                List<BEParametro> listParametrosUP = new BLGeneral().ListaParametrosGrupo(codigoParametrosClaroUP);
                if (listParametrosUP != null && listParametrosUP.Count() > 0)
                {
                    AppSettings.consMontoCuotaMenorA = listParametrosUP
                     .Where(w => w.Valor1.Equals("1")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosUP.Where(w => w.Valor1.Equals("1")).ToList()[0].Valor) : "";

                    AppSettings.consMontoCuotaMayorA = listParametrosUP
                      .Where(w => w.Valor1.Equals("2")).ToList().Count > 0 ?
                      Funciones.CheckStr(listParametrosUP.Where(w => w.Valor1.Equals("2")).ToList()[0].Valor) : "";

                    AppSettings.consMsjActualizacionCuotaIncial = listParametrosUP
                     .Where(w => w.Valor1.Equals("3")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosUP.Where(w => w.Valor1.Equals("3")).ToList()[0].Valor) : "";

                    AppSettings.consMaxPorcentajeCuotaInicial = listParametrosUP
                     .Where(w => w.Valor1.Equals("4")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosUP.Where(w => w.Valor1.Equals("4")).ToList()[0].Valor) : "";

                    AppSettings.consMontoCuotaMayorAPorcentaje = listParametrosUP
                     .Where(w => w.Valor1.Equals("5")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosUP.Where(w => w.Valor1.Equals("5")).ToList()[0].Valor) : "";

                    AppSettings.consMsjMontoCuotaValido = listParametrosUP
                         .Where(w => w.Valor1.Equals("6")).ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosUP.Where(w => w.Valor1.Equals("6")).ToList()[0].Valor) : "";
             

                    objLog.CrearArchivolog(string.Format("{0}{1}", "consMontoCuotaMenorA: ", Funciones.CheckStr(AppSettings.consMontoCuotaMenorA)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "consMontoCuotaMayorA: ", Funciones.CheckStr(AppSettings.consMontoCuotaMayorA)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "consMsjActualizacionCuotaIncial: ", Funciones.CheckStr(AppSettings.consMsjActualizacionCuotaIncial)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "consMaxPorcentajeCuotaInicial: ", Funciones.CheckStr(AppSettings.consMaxPorcentajeCuotaInicial)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "consMontoCuotaMayorAPorcentaje: ", Funciones.CheckStr(AppSettings.consMontoCuotaMayorAPorcentaje)), null, null);
                    objLog.CrearArchivolog(" ===  FIN Cargar Parametros Claro UP === ", null, null);
                }
                //PROY-30166-IDEA–38863-FIN

                //INICIO PROY-140419 Autorizar Portabilidad sin PIN
                objLog.CrearArchivolog("Inicio PROY-140419 Carga de Parámetros", null, null);
                string strCodGrupoSMSPortaSuperv = Funciones.CheckStr(ConfigurationManager.AppSettings["key_ParanGrupoSMSPortaSuperv"]);
                List<BEParametro> ListParamSmsPortaSuperv = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoSMSPortaSuperv));
                objLog.CrearArchivolog(String.Format("Se cargaron {0} parámetros", Funciones.CheckStr(ListParamSmsPortaSuperv.Count())), null, null);
                if (ListParamSmsPortaSuperv != null && ListParamSmsPortaSuperv.Count() > 0)
                {
                    ReadKeySettings.key_flagSmsPortaSuperv = ListParamSmsPortaSuperv
                    .Where(w => w.Valor1.Equals("key_flag_smsporta_supervisor")).ToList().Count > 0 ?
                    Funciones.CheckInt(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_flag_smsporta_supervisor")).ToList()[0].Valor) : 0;

                    ReadKeySettings.key_SmsPortaSupervPerfil = ListParamSmsPortaSuperv
                    .Where(w => w.Valor1.Equals("key_perfiles")).ToList().Count > 0 ?
                    Funciones.CheckStr(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_perfiles")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_SmsPortaSupervCanales = ListParamSmsPortaSuperv
                        .Where(w => w.Valor1.Equals("key_canales")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_canales")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_SmsPortaSupervTipoDoc = ListParamSmsPortaSuperv
                        .Where(w => w.Valor1.Equals("key_tipo_documento")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_tipo_documento")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_CacNoPresencial = ListParamSmsPortaSuperv
                        .Where(w => w.Valor1.Equals("key_CacNoPresencial")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_CacNoPresencial")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_MsjExito = ListParamSmsPortaSuperv
                        .Where(w => w.Valor1.Equals("key_msjExito")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_msjExito")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_MsjPerfilSinPermisos = ListParamSmsPortaSuperv
                        .Where(w => w.Valor1.Equals("key_msjPerfil_SinPermiso")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_msjPerfil_SinPermiso")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_MsjPerfilIncorrecto = ListParamSmsPortaSuperv
                        .Where(w => w.Valor1.Equals("key_msjPerfil_Incorrecto")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_msjPerfil_Incorrecto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_SupervTipoProducto = ListParamSmsPortaSuperv
                        .Where(w => w.Valor1.Equals("key_tipo_producto")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_tipo_producto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_SupervOferta = ListParamSmsPortaSuperv
                        .Where(w => w.Valor1.Equals("key_tipo_oferta")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_tipo_oferta")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_SupervMsjNoCumpleValidacion = ListParamSmsPortaSuperv
                        .Where(w => w.Valor1.Equals("key_msjNoValidacion")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamSmsPortaSuperv.Where(w => w.Valor1.Equals("key_msjNoValidacion")).ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_flagSmsPortaSuperv] => ", ReadKeySettings.key_flagSmsPortaSuperv), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_SmsPortaSupervPerfil] => ", ReadKeySettings.key_flagSmsPortaSuperv), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_SmsPortaSupervCanales] => ", ReadKeySettings.key_SmsPortaSupervCanales), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_SmsPortaSupervTipoDoc] => ", ReadKeySettings.key_SmsPortaSupervTipoDoc), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_CacNoPresencial] => ", ReadKeySettings.key_CacNoPresencial), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_MsjExito] => ", ReadKeySettings.key_MsjExito), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_MsjPerfilSinPermisos] => ", ReadKeySettings.key_MsjPerfilSinPermisos), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_MsjPerfilIncorrecto] => ", ReadKeySettings.key_MsjPerfilIncorrecto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_SupervTipoProducto] => ", ReadKeySettings.key_SupervTipoProducto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_SupervOferta] => ", ReadKeySettings.key_SupervOferta), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140419][ReadKeySettings.key_SupervMsjNoCumpleValidacion] => ", ReadKeySettings.key_SupervMsjNoCumpleValidacion), null, null);
                    
                    objLog.CrearArchivolog("Fin PROY-140419 Carga de Parámetros", null, null);
                }
                //FIN PROY-140419 Autorizar Portabilidad sin PIN

                /*INICIO PROY-140380 - Beneficio Full Claro*/

                objLog.CrearArchivolog("Inicio PROY-140380 Carga de Parámetros", null, null);

                Int64 key_ParanGrupoFullClaro = 0;

                key_ParanGrupoFullClaro = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoFullClaro"].ToString());
                List<BEParametro> listParametrosFullClaro = new BLGeneral().ListaParametrosGrupo(key_ParanGrupoFullClaro);

                objLog.CrearArchivolog(String.Format("Se cargaron {0} parámetros", Funciones.CheckStr(listParametrosFullClaro.Count())), null, null);

                if (listParametrosFullClaro != null && listParametrosFullClaro.Count() > 0)
                {

                    ReadKeySettings.key_TipoCanal = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_TipoCanal")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_TipoCanal")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_TipoOperacion = listParametrosFullClaro
                    .Where(w => w.Valor1.Equals("key_TipoOperacion")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_TipoOperacion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_TipoOferta = listParametrosFullClaro
                    .Where(w => w.Valor1.Equals("key_TipoOferta")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_TipoOferta")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_FlagPorta = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_FlagPorta")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_FlagPorta")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_TipoProducto = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_TipoProducto")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_TipoProducto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_TipoDocumento = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_TipoDocumento")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_TipoDocumento")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_msjSinServicios = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_msjSinServicios")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_msjSinServicios")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_msjConServicios = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_msjConServicios")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_msjConServicios")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_msjServicioMovil = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_msjServicioMovil")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_msjServicioMovil")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_msjServicioFijo = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_msjServicioFijo")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_msjServicioFijo")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_FlagGeneralBeneficio = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_FlagGeneralBeneficio")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_FlagGeneralBeneficio")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_msjBeneficioFija = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_msjBeneficioFija")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_msjBeneficioFija")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_msjBeneficioMovil = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_msjBeneficioMovil")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_msjBeneficioMovil")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_msjCampanasFullClaro = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_msjCampanasFullClaro")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_msjCampanasFullClaro")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_msjConfigFullClaro = listParametrosFullClaro
                     .Where(w => w.Valor1.Equals("key_msjConfigFullClaro")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaro.Where(w => w.Valor1.Equals("key_msjConfigFullClaro")).ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_TipoCanal] => ", ReadKeySettings.key_TipoCanal), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_TipoOperacion] => ", ReadKeySettings.key_TipoOperacion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_FlagPorta] => ", ReadKeySettings.key_FlagPorta), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_TipoProducto] => ", ReadKeySettings.key_TipoProducto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_TipoDocumento] => ", ReadKeySettings.key_TipoDocumento), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_msjSinServicios] => ", ReadKeySettings.key_msjSinServicios), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_msjConServicios] => ", ReadKeySettings.key_msjConServicios), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_msjServicioMovil] => ", ReadKeySettings.key_msjServicioMovil), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_msjServicioFijo] => ", ReadKeySettings.key_msjServicioFijo), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_FlagGeneralBeneficio] => ", ReadKeySettings.key_FlagGeneralBeneficio), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_msjBeneficioFija] => ", ReadKeySettings.key_msjBeneficioFija), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_msjBeneficioMovil] => ", ReadKeySettings.key_msjBeneficioMovil), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-1012][ReadKeySettings.key_msjCampanasFullClaro] => ", ReadKeySettings.key_msjCampanasFullClaro), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-1012][ReadKeySettings.key_msjConfigFullClaro] => ", ReadKeySettings.key_msjConfigFullClaro), null, null);

                    objLog.CrearArchivolog("Fin PROY-140380 Carga de Parámetros", null, null);
                }

                /*FIN PROY-140380 - Beneficio Full Claro*/


                /*INICIO PROY-140560 - Beneficio Full Claro antes grabar popup*/
                Int64 key_ParanGrupoFullClaroPopup = 0;

                key_ParanGrupoFullClaroPopup = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoFullClaroPopup"].ToString());
                List<BEParametro> listParametrosFullClaroPopup = new BLGeneral().ListaParametrosGrupo(key_ParanGrupoFullClaroPopup);

                ReadKeySettings.key_MsjSinServicioActivos = listParametrosFullClaroPopup
                     .Where(w => w.Valor1.Equals("key_MsjSinServicioActivos")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosFullClaroPopup.Where(w => w.Valor1.Equals("key_MsjSinServicioActivos")).ToList()[0].Valor) : string.Empty;

                ReadKeySettings.key_MsjConBeneficio = listParametrosFullClaroPopup
                 .Where(w => w.Valor1.Equals("key_MsjConBeneficio")).ToList().Count > 0 ?
                 Funciones.CheckStr(listParametrosFullClaroPopup.Where(w => w.Valor1.Equals("key_MsjConBeneficio")).ToList()[0].Valor) : string.Empty;

                ReadKeySettings.key_MsjConBeneficioFCVentPag = listParametrosFullClaroPopup
                 .Where(w => w.Valor1.Equals("key_MsjConBeneficioFCVentPag")).ToList().Count > 0 ?
                 Funciones.CheckStr(listParametrosFullClaroPopup.Where(w => w.Valor1.Equals("key_MsjConBeneficioFCVentPag")).ToList()[0].Valor) : string.Empty;




                ReadKeySettings.key_FlagServicioMovil = listParametrosFullClaroPopup
                 .Where(w => w.Valor1.Equals("key_FlagServicioMovil")).ToList().Count > 0 ?
                 Funciones.CheckStr(listParametrosFullClaroPopup.Where(w => w.Valor1.Equals("key_FlagServicioMovil")).ToList()[0].Valor) : string.Empty;

                ReadKeySettings.key_FlagServicioFijo = listParametrosFullClaroPopup
                 .Where(w => w.Valor1.Equals("key_FlagServicioFijo")).ToList().Count > 0 ?
                 Funciones.CheckStr(listParametrosFullClaroPopup.Where(w => w.Valor1.Equals("key_FlagServicioFijo")).ToList()[0].Valor) : string.Empty;

                ReadKeySettings.key_FlagConServicios = listParametrosFullClaroPopup
                 .Where(w => w.Valor1.Equals("key_FlagConServicios")).ToList().Count > 0 ?
                 Funciones.CheckStr(listParametrosFullClaroPopup.Where(w => w.Valor1.Equals("key_FlagConServicios")).ToList()[0].Valor) : string.Empty;

                ReadKeySettings.key_FlagMsjSinServicioActivos = listParametrosFullClaroPopup
                .Where(w => w.Valor1.Equals("key_FlagMsjSinServicioActivos")).ToList().Count > 0 ?
                Funciones.CheckStr(listParametrosFullClaroPopup.Where(w => w.Valor1.Equals("key_FlagMsjSinServicioActivos")).ToList()[0].Valor) : string.Empty;

                ReadKeySettings.key_FlagMsjConBeneficio = listParametrosFullClaroPopup
                 .Where(w => w.Valor1.Equals("key_FlagMsjConBeneficio")).ToList().Count > 0 ?
                 Funciones.CheckStr(listParametrosFullClaroPopup.Where(w => w.Valor1.Equals("key_FlagMsjConBeneficio")).ToList()[0].Valor) : string.Empty;

                ReadKeySettings.key_FlagMsjConBeneficioFCVentPag = listParametrosFullClaroPopup
                 .Where(w => w.Valor1.Equals("key_FlagMsjConBeneficioFCVentPag")).ToList().Count > 0 ?
                 Funciones.CheckStr(listParametrosFullClaroPopup.Where(w => w.Valor1.Equals("key_FlagMsjConBeneficioFCVentPag")).ToList()[0].Valor) : string.Empty;

                ReadKeySettings.key_msjConfigFullClaroPopup = listParametrosFullClaroPopup
                 .Where(w => w.Valor1.Equals("key_msjConfigFullClaroPopup")).ToList().Count > 0 ?
                 Funciones.CheckStr(listParametrosFullClaroPopup.Where(w => w.Valor1.Equals("key_msjConfigFullClaroPopup")).ToList()[0].Valor) : string.Empty;

                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140560][ReadKeySettings.key_MsjSinServicioActivos] => ", ReadKeySettings.key_MsjSinServicioActivos), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140560][ReadKeySettings.key_MsjConBeneficio] => ", ReadKeySettings.key_MsjConBeneficio), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140560][ReadKeySettings.key_MsjConBeneficioFCVentPag] => ", ReadKeySettings.key_MsjConBeneficioFCVentPag), null, null);

                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140560][ReadKeySettings.key_FlagServicioMovil] => ", ReadKeySettings.key_FlagServicioMovil), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140560][ReadKeySettings.key_FlagServicioFijo] => ", ReadKeySettings.key_FlagServicioFijo), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140560][ReadKeySettings.key_FlagConServicios] => ", ReadKeySettings.key_FlagConServicios), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140560][ReadKeySettings.key_FlagMsjSinServicioActivos] => ", ReadKeySettings.key_FlagMsjSinServicioActivos), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140560][ReadKeySettings.key_FlagMsjConBeneficio] => ", ReadKeySettings.key_FlagMsjConBeneficio), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140560][ReadKeySettings.key_FlagMsjConBeneficioFCVentPag] => ", ReadKeySettings.key_FlagMsjConBeneficioFCVentPag), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-1012][ReadKeySettings.key_msjConfigFullClaroPopup] => ", ReadKeySettings.key_msjConfigFullClaroPopup), null, null);

                /*FIN PROY-140560 - Beneficio Full Claro antes grabar popup*/



                //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::INI
                objLog.CrearArchivolog("Inicio PROY-140439 BRMS Carga de Parámetros", null, null);
                string strCodGrupoBRMSS = Funciones.CheckStr(ConfigurationManager.AppSettings["key_ParanGrupoBRMS"]);
                List<BEParametro> ListParamBRMS = (new BLGeneral()).ListaParametrosGrupo(Funciones.CheckInt64(strCodGrupoBRMSS));
                objLog.CrearArchivolog(String.Format("Se cargaron {0} parámetros", Funciones.CheckStr(ListParamBRMS.Count())), null, null);
                if (ListParamBRMS != null && ListParamBRMS.Count() > 0)
                {
                    ReadKeySettings.Key_msjSinValidacionCamp = ListParamBRMS
                        .Where(w => w.Valor1.Equals("Key_msjSinValidacionCamp")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamBRMS.Where(w => w.Valor1.Equals("Key_msjSinValidacionCamp")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_canalesBRMSCamp = ListParamBRMS
                        .Where(w => w.Valor1.Equals("Key_canalesBRMSCamp")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamBRMS.Where(w => w.Valor1.Equals("Key_canalesBRMSCamp")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_mventaBRMSCamp = ListParamBRMS
                        .Where(w => w.Valor1.Equals("Key_mventaBRMSCamp")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamBRMS.Where(w => w.Valor1.Equals("Key_mventaBRMSCamp")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_tofertaBRMSCamp = ListParamBRMS
                        .Where(w => w.Valor1.Equals("Key_tofertaBRMSCamp")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamBRMS.Where(w => w.Valor1.Equals("Key_tofertaBRMSCamp")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_tdocumentoBRMSCamp = ListParamBRMS
                        .Where(w => w.Valor1.Equals("Key_tdocumentoBRMSCamp")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamBRMS.Where(w => w.Valor1.Equals("Key_tdocumentoBRMSCamp")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_toperacionBRMSCamp = ListParamBRMS
                        .Where(w => w.Valor1.Equals("Key_toperacionBRMSCamp")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamBRMS.Where(w => w.Valor1.Equals("Key_toperacionBRMSCamp")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_tproductoBRMSCamp = ListParamBRMS
                        .Where(w => w.Valor1.Equals("Key_tproductoBRMSCamp")).ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamBRMS.Where(w => w.Valor1.Equals("Key_tproductoBRMSCamp")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_fportabilidaBRMSCamp = ListParamBRMS
                       .Where(w => w.Valor1.Equals("Key_fportabilidaBRMSCamp")).ToList().Count > 0 ?
                       Funciones.CheckStr(ListParamBRMS.Where(w => w.Valor1.Equals("Key_fportabilidaBRMSCamp")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_flagBRMSCamp = ListParamBRMS
                       .Where(w => w.Valor1.Equals("Key_flagBRMSCamp")).ToList().Count > 0 ?
                       Funciones.CheckStr(ListParamBRMS.Where(w => w.Valor1.Equals("Key_flagBRMSCamp")).ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140439 CAMPAÑAS][ReadKeySettings.Key_msjSinValidacionCamp] => ", ReadKeySettings.Key_msjSinValidacionCamp), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140439 CAMPAÑAS][ReadKeySettings.Key_canalesBRMSCamp] => ", ReadKeySettings.Key_canalesBRMSCamp), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140439 CAMPAÑAS][ReadKeySettings.Key_mventaBRMSCamp] => ", ReadKeySettings.Key_mventaBRMSCamp), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140439 CAMPAÑAS][ReadKeySettings.Key_tofertaBRMSCamp] => ", ReadKeySettings.Key_tofertaBRMSCamp), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140439 CAMPAÑAS][ReadKeySettings.Key_tdocumentoBRMSCamp] => ", ReadKeySettings.Key_tdocumentoBRMSCamp), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140439 CAMPAÑAS][ReadKeySettings.Key_toperacionBRMSCamp] => ", ReadKeySettings.Key_toperacionBRMSCamp), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140439 CAMPAÑAS][ReadKeySettings.Key_tproductoBRMSCamp] => ", ReadKeySettings.Key_tproductoBRMSCamp), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140439 CAMPAÑAS][ReadKeySettings.Key_fportabilidaBRMSCamp] => ", ReadKeySettings.Key_fportabilidaBRMSCamp), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140439 CAMPAÑAS][ReadKeySettings.Key_flagBRMSCamp] => ", ReadKeySettings.Key_flagBRMSCamp), null, null);
                }
                //PROY-140439 BRMS CAMPAÑA NAVIDEÑA::FIN

                //PROY-140457-DEBITO AUTOMATICO-INI
                objLog.CrearArchivolog("Inicio PROY-140457-DEBITO AUTOMATICO Carga de Parametros", null, null);
                Int64 strCodGrupoDebito = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoDebito"]);
                List<BEParametro> listParametrosDebito = new BLGeneral().ListaParametrosGrupo(strCodGrupoDebito);
                objLog.CrearArchivolog(string.Format("Se cargaron {0} parametros", Funciones.CheckStr(listParametrosDebito.Count())), null, null);
                if (listParametrosDebito != null && listParametrosDebito.Count() > 0) 
                {
                    ReadKeySettings.Key_flagDebitoAuto = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_flagDebitoAuto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_flagDebitoAuto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_canalesDebitoAuto = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_canalesDebitoAuto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_canalesDebitoAuto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_tdocumentoDebitoAuto = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_tdocumentoDebitoAuto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_tdocumentoDebitoAuto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_tproductoDebitoAuto = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_tproductoDebitoAuto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_tproductoDebitoAuto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_toperacionDebitoAuto = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_toperacionDebitoAuto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_toperacionDebitoAuto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_flagPortaDebitoAuto = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_flagPortaDebitoAuto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_flagPortaDebitoAuto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_OfertaDebitoAuto = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_OfertaDebitoAuto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_OfertaDebitoAuto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_eTarjetaDebitoAuto = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_eTarjetaDebitoAuto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_eTarjetaDebitoAuto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_CuentaEDebitoAuto = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_CuentaEDebitoAuto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_CuentaEDebitoAuto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_tOperacionRestriccion = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_tOperacionRestriccion")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_tOperacionRestriccion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_canalesRestriccion = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_canalesRestriccion")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_canalesRestriccion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_productoRestriccion = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_productoRestriccion")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_productoRestriccion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_mVentaRestriccion = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_mVentaRestriccion")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_mVentaRestriccion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_campanasRestriccion = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_campanasRestriccion")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_campanasRestriccion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_flagPortaRestriccion = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_flagPortaRestriccion")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_flagPortaRestriccion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_msjErrorDebitoAuto = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_msjErrorDebitoAuto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_msjErrorDebitoAuto")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_msjErrorFraudeDebito = listParametrosDebito
                        .Where(w => w.Valor1.Equals("Key_msjErrorFraudeDebito")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosDebito.Where(w => w.Valor1.Equals("Key_msjErrorFraudeDebito")).ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_flagDebitoAuto] => ", ReadKeySettings.Key_flagDebitoAuto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_canalesDebitoAuto] => ", ReadKeySettings.Key_canalesDebitoAuto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_tdocumentoDebitoAuto] => ", ReadKeySettings.Key_tdocumentoDebitoAuto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_tproductoDebitoAuto] => ", ReadKeySettings.Key_tproductoDebitoAuto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_toperacionDebitoAuto] => ", ReadKeySettings.Key_toperacionDebitoAuto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_flagPortaDebitoAuto] => ", ReadKeySettings.Key_flagPortaDebitoAuto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_OfertaDebitoAuto] => ", ReadKeySettings.Key_OfertaDebitoAuto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_eTarjetaDebitoAuto] => ", ReadKeySettings.Key_eTarjetaDebitoAuto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_CuentaEDebitoAuto] => ", ReadKeySettings.Key_CuentaEDebitoAuto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_tOperacionRestriccion] => ", ReadKeySettings.Key_tOperacionRestriccion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_canalesRestriccion] => ", ReadKeySettings.Key_canalesRestriccion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_productoRestriccion] => ", ReadKeySettings.Key_productoRestriccion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_mVentaRestriccion] => ", ReadKeySettings.Key_mVentaRestriccion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_campanasRestriccion] => ", ReadKeySettings.Key_campanasRestriccion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_flagPortaRestriccion] => ", ReadKeySettings.Key_flagPortaRestriccion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_msjErrorDebitoAuto] => ", ReadKeySettings.Key_msjErrorDebitoAuto), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140457-DEBITO AUTOMATICO][ReadKeySettings.Key_msjErrorFraudeDebito] => ", ReadKeySettings.Key_msjErrorFraudeDebito), null, null);
                }
                //PROY-140457-DEBITO AUTOMATICO-FIN


                //INC000002977281-INI
                objLog.CrearArchivolog("Inicio INC000002977281", null, null);
                Int64 strFlagBonoFullClaroFija = Funciones.CheckInt64(ConfigurationManager.AppSettings["strFlagBonoFullClaroFija"]);
                List<BEParametro> listFlagBonoFullClaroFija = new BLGeneral().ListaParametrosGrupo(strFlagBonoFullClaroFija);
                objLog.CrearArchivolog(string.Format("Se cargaron {0} parametros", Funciones.CheckStr(listParametrosDebito.Count())), null, null);
                if (listFlagBonoFullClaroFija != null && listFlagBonoFullClaroFija.Count() > 0)
                {
                    ReadKeySettings.Key_codFlagBFClaroFijaApagado = listFlagBonoFullClaroFija
                        .Where(w => w.Valor1.Equals("Key_codFlagBFClaroFijaApagado")).ToList().Count > 0 ?
                        Funciones.CheckStr(listFlagBonoFullClaroFija.Where(w => w.Valor1.Equals("Key_codFlagBFClaroFijaApagado")).ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002977281][ReadKeySettings.Key_codFlagBFClaroFijaApagado] => ", ReadKeySettings.Key_codFlagBFClaroFijaApagado), null, null);
                }
                //INC000002977281-FIN

                //INC000003062381  - INICIO -- Key_codSecRechazaAnteriormente
                Int64 strCodSecRechazaAnteriormente = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_codSecRechazaAnteriormente"]);
                List<BEParametro> lstCodSecRechazaAnteriormente = new BLGeneral().ListaParametrosGrupo(Funciones.CheckInt(strCodSecRechazaAnteriormente));
                objLog.CrearArchivolog(String.Format("Se cargaron {0} parámetros - INC000002561949 ", Funciones.CheckStr(lstCodSecRechazaAnteriormente.Count())), null, null);

                if (lstCodSecRechazaAnteriormente != null && lstCodSecRechazaAnteriormente.Count() > 0)
                {
                    ReadKeySettings.Key_EstadoPendienteAprobacion = lstCodSecRechazaAnteriormente
                    .Where(w => w.Valor1.Equals("Key_EstadoPendienteAprobacion")).ToList().Count > 0 ?
                    Funciones.CheckStr(lstCodSecRechazaAnteriormente.Where(w => w.Valor1.Equals("Key_EstadoPendienteAprobacion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_MensajeDeEstadoSec = lstCodSecRechazaAnteriormente
                    .Where(w => w.Valor1.Equals("Key_MensajeDeEstadoSec")).ToList().Count > 0 ?
                    Funciones.CheckStr(lstCodSecRechazaAnteriormente.Where(w => w.Valor1.Equals("Key_MensajeDeEstadoSec")).ToList()[0].Valor) : string.Empty;

                }
                objLog.CrearArchivolog(String.Format(" INC000002561949 - Key_EstadoPendienteAprobacion : " + Funciones.CheckStr(ReadKeySettings.Key_EstadoPendienteAprobacion)), null, null);

                //INC000003062381  - FIN

                //INC-SMS_PORTA_INI
                /*INICIO PROY-140356 IDEA-140951 Enviar SMS al solicitar portabilidad*/

                objLog.CrearArchivolog("Inicio PROY-140356 Carga de Parámetros", null, null);

                Int64 key_ParanGrupoPinSMS = 0;

                key_ParanGrupoPinSMS = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoPinSMS"]);
                List<BEParametro> listParametrosPinSMS = new BLGeneral().ListaParametrosGrupo(key_ParanGrupoPinSMS);

                objLog.CrearArchivolog(String.Format("Se cargaron {0} parámetros", Funciones.CheckStr(listParametrosFullClaro.Count())), null, null);

                if (listParametrosPinSMS != null && listParametrosPinSMS.Count() > 0)
                {

                    ReadKeySettings.key_flag_smsportabilidad = listParametrosPinSMS
                     .Where(w => w.Valor1.Equals("key_flag_smsportabilidad")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosPinSMS.Where(w => w.Valor1.Equals("key_flag_smsportabilidad")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_OfertasPermitidas = listParametrosPinSMS
                    .Where(w => w.Valor1.Equals("key_OfertasPermitidas")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosPinSMS.Where(w => w.Valor1.Equals("key_OfertasPermitidas")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_ProductosPermitidos = listParametrosPinSMS
                     .Where(w => w.Valor1.Equals("key_ProductosPermitidos")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosPinSMS.Where(w => w.Valor1.Equals("key_ProductosPermitidos")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_CanalesPermitidos = listParametrosPinSMS
                     .Where(w => w.Valor1.Equals("key_CanalesPermitidos")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosPinSMS.Where(w => w.Valor1.Equals("key_CanalesPermitidos")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_MsjNoCumpleValidacion = listParametrosPinSMS
                    .Where(w => w.Valor1.Equals("key_MsjNoCumpleValidacion")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosPinSMS.Where(w => w.Valor1.Equals("key_MsjNoCumpleValidacion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_CodEncriptacion = listParametrosPinSMS
                    .Where(w => w.Valor1.Equals("key_CodEncriptacion")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosPinSMS.Where(w => w.Valor1.Equals("key_CodEncriptacion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_MsjErrorValidacionPIN = listParametrosPinSMS
                    .Where(w => w.Valor1.Equals("key_MsjErrorValidacionPIN")).ToList().Count > 0 ?
                    Funciones.CheckStr(listParametrosPinSMS.Where(w => w.Valor1.Equals("key_MsjErrorValidacionPIN")).ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_flag_smsportabilidad] => ", ReadKeySettings.key_flag_smsportabilidad), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_OfertasPermitidas] => ", ReadKeySettings.key_OfertasPermitidas), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_ProductosPermitidos] => ", ReadKeySettings.key_ProductosPermitidos), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_CanalesPermitidos] => ", ReadKeySettings.key_CanalesPermitidos), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_MsjNoCumpleValidacion] => ", ReadKeySettings.key_MsjNoCumpleValidacion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_CodEncriptacion] => ", ReadKeySettings.key_CodEncriptacion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140380][ReadKeySettings.key_MsjErrorValidacionPIN] => ", ReadKeySettings.key_MsjErrorValidacionPIN), null, null);
                    objLog.CrearArchivolog("Fin PROY-140356 Carga de Parámetros", null, null);
                }

                /*FIN PROY-140356 IDEA-140951 Enviar SMS al solicitar portabilidad*/
                //INC-SMS_PORTA_FIN

                //INI-INC000002510501  Campañas
                objLog.CrearArchivolog("Inicio INC000002510501 Carga de Parámetros", null, null);

                Int64 key_ParanGrupoReguCampanas = 0;

                key_ParanGrupoReguCampanas = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoReguCampanas"]);
                List<BEParametro> listParametrosReguCamp = new BLGeneral().ListaParametrosGrupo(key_ParanGrupoReguCampanas);

                objLog.CrearArchivolog(String.Format("Se cargaron {0} parámetros", Funciones.CheckStr(listParametrosReguCamp.Count())), null, null);

                if (listParametrosReguCamp != null && listParametrosReguCamp.Count() > 0)
                {
                    ReadKeySettings.Key_R_Flag = listParametrosReguCamp
                     .Where(w => w.Valor1.Equals("Key_R_Flag")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosReguCamp.Where(w => w.Valor1.Equals("Key_R_Flag")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_R_Modalidad = listParametrosReguCamp
                     .Where(w => w.Valor1.Equals("Key_R_Modalidad")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosReguCamp.Where(w => w.Valor1.Equals("Key_R_Modalidad")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_R_Operador = listParametrosReguCamp
                     .Where(w => w.Valor1.Equals("Key_R_Operador")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosReguCamp.Where(w => w.Valor1.Equals("Key_R_Operador")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_R_Campana = listParametrosReguCamp
                     .Where(w => w.Valor1.Equals("Key_R_Campana")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosReguCamp.Where(w => w.Valor1.Equals("Key_R_Campana")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_R_FlagCampana = listParametrosReguCamp
                     .Where(w => w.Valor1.Equals("Key_R_FlagCampana")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosReguCamp.Where(w => w.Valor1.Equals("Key_R_FlagCampana")).ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002510501][ReadKeySettings.Key_R_Flag] => ", ReadKeySettings.Key_R_Flag), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002510501][ReadKeySettings.Key_R_Modalidad] => ", ReadKeySettings.Key_R_Modalidad), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002510501][ReadKeySettings.Key_R_Operador] => ", ReadKeySettings.Key_R_Operador), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002510501][ReadKeySettings.Key_R_Campana] => ", ReadKeySettings.Key_R_Campana), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002510501][ReadKeySettings.Key_R_FlagCampana] => ", ReadKeySettings.Key_R_FlagCampana), null, null);
                    objLog.CrearArchivolog("Fin INC000002510501 Carga de Parámetros", null, null);
                }
                //FIN-INC000002510501  Campañas

                //PROY-140335-ANULACION - INI
                objLog.CrearArchivolog("Inicio PROY-140335 Carga de Parámetros", null, null);

                Int64 Key_ParanGrupoAnulacion = 0;
                Key_ParanGrupoAnulacion = Funciones.CheckInt64(ConfigurationManager.AppSettings["ParanGrupoAnulacion"]);
                List<BEParametro> listParametrosAnulacion = new BLGeneral().ListaParametrosGrupo(Key_ParanGrupoAnulacion);

                objLog.CrearArchivolog(String.Format("Se cargaron {0} parámetros", Funciones.CheckStr(listParametrosAnulacion.Count())), null, null);

                if (listParametrosAnulacion != null && listParametrosAnulacion.Count() > 0)
                {
                    ReadKeySettings.Key_FlagAnulacion = listParametrosAnulacion
                     .Where(w => w.Valor1.Equals("Key_FlagAnulacion")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosAnulacion.Where(w => w.Valor1.Equals("Key_FlagAnulacion")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_CodigoCanales = listParametrosAnulacion
                     .Where(w => w.Valor1.Equals("Key_CodigoCanales")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosAnulacion.Where(w => w.Valor1.Equals("Key_CodigoCanales")).ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][ReadKeySettings.Key_FlagAnulacion] => ", ReadKeySettings.Key_FlagAnulacion), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140335][ReadKeySettings.Key_CodigoCanales] => ", ReadKeySettings.Key_CodigoCanales), null, null);
                    objLog.CrearArchivolog("Fin PROY-140335 Carga de Parámetros", null, null);
                }
                //PROY-140335-ANULACION - FIN

                //INI: INICIATIVA-219 - CBIO
                Int64 codParanGrupoCBIO = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParamGrupoCBIO"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-219][PARAN_GRUPO]", Funciones.CheckStr(codParanGrupoCBIO)), null, null);
                List<BEParametro> lstParamCBIO = new BLGeneral().ListaParametrosGrupo(codParanGrupoCBIO);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-219][RESULTADO]", Funciones.CheckStr(lstParamCBIO.Count())), null, null);

                AppSettings.consFlagCBIO = lstParamCBIO.Where(w => w.Valor1 == "key_flagCBIO").ToList().Count > 0 ?
                Funciones.CheckStr(lstParamCBIO.Where(w => w.Valor1 == "key_flagCBIO").ToList()[0].Valor) : string.Empty;

                objLog.CrearArchivolog(string.Format("{0} => {1}", "[INICIATIVA-219][key_flagCBIO]", Funciones.CheckStr(AppSettings.consFlagCBIO)), null, null);
                //INI: INICIATIVA-219 - CBIO

                /*INICIO PROY-140542 - IDEA141640 Mejora en Proceso de Omision de PIN SMS de portabilidad*/
                objLog.CrearArchivolog("Inicio PROY-140542 Carga de Par?metros", null, null);

                Int64 Key_ParamOmisionPIN = 0;
                Key_ParamOmisionPIN = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_ParamOmisionPIN"]);
                List<BEParametro> listParametrosOmision = new BLGeneral().ListaParametrosGrupo(Key_ParamOmisionPIN);

                objLog.CrearArchivolog(String.Format("Se cargaron {0} par?metros", Funciones.CheckStr(listParametrosOmision.Count())), null, null);

                if (listParametrosOmision != null && listParametrosOmision.Count() > 0)
                {
                    ReadKeySettings.Key_TituloCausalOmision = listParametrosOmision
                     .Where(w => w.Valor1.Equals("Key_TituloCausalOmision")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosOmision.Where(w => w.Valor1.Equals("Key_TituloCausalOmision")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_CausalesOmisionPIN = listParametrosOmision
                     .Where(w => w.Valor1.Equals("Key_CausalesOmisionPIN")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosOmision.Where(w => w.Valor1.Equals("Key_CausalesOmisionPIN")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_FlagPermitirTodosDocumentos = listParametrosOmision
                     .Where(w => w.Valor1.Equals("Key_FlagPermitirTodosDocumentos")).ToList().Count > 0 ?
                     Funciones.CheckStr(listParametrosOmision.Where(w => w.Valor1.Equals("Key_FlagPermitirTodosDocumentos")).ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140542][ReadKeySettings.Key_TituloCausalOmision] => ", ReadKeySettings.Key_TituloCausalOmision), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140542][ReadKeySettings.Key_CausalesOmisionPIN] => ", ReadKeySettings.Key_CausalesOmisionPIN), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140542][ReadKeySettings.Key_FlagPermitirTodosDocumentos] => ", ReadKeySettings.Key_FlagPermitirTodosDocumentos), null, null);
                    objLog.CrearArchivolog("Fin PROY-140542 Carga de Par?metros", null, null);
                }
                /*fin PROY-140542 - IDEA141640 Mejora en Proceso de Omision de PIN SMS de portabilidad*/

                /*INICIO PROY-140585- IDEA142064 Mejora en los sistemas de venta*/
                objLog.CrearArchivolog("Inicio PROY-140585 Carga de Parametros", null, null);

                Int64 Key_MejorasSisact = 0;
                Key_MejorasSisact = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_MejorasSisact"]);
                List<BEParametro> listaParamMejorasSisact = new BLGeneral().ListaParametrosGrupo(Key_MejorasSisact);

                objLog.CrearArchivolog(String.Format("Se cargaron {0} parametros", Funciones.CheckStr(listaParamMejorasSisact.Count())), null, null);

                if (listaParamMejorasSisact != null && listaParamMejorasSisact.Count() > 0)
                {
                    ReadKeySettings.Key_FlagGeneralOfertaDefault = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("Key_FlagGeneralOfertaDefault")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("Key_FlagGeneralOfertaDefault")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_CanalPermitidoOfertaDefault = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("Key_CanalPermitidoOfertaDefault")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("Key_CanalPermitidoOfertaDefault")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_OperacionPermitidaOfertaDefault = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("Key_OperacionPermitidaOfertaDefault")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("Key_OperacionPermitidaOfertaDefault")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_DocumentosPermitidosOfertaDefault = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("Key_DocumentosPermitidosOfertaDefault")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("Key_DocumentosPermitidosOfertaDefault")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_CodigoOfertaDefault = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("Key_CodigoOfertaDefault")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("Key_CodigoOfertaDefault")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_IsPortabilidadOfertaDefault = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("Key_IsPortabilidadOfertaDefault")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("Key_IsPortabilidadOfertaDefault")).ToList()[0].Valor) : string.Empty;

                    //PROY-140585 F2 INI 
                    ReadKeySettings.key_FlagSMSxPDVEvaluacionPrePost = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("key_FlagSMSxPDVEvaluacionPrePost")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("key_FlagSMSxPDVEvaluacionPrePost")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_flag_validacionPDV = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("key_flag_validacionPDV")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("key_flag_validacionPDV")).ToList()[0].Valor) : string.Empty;

                   
                        ReadKeySettings.key_CanalPermMsjeBRMSCamp = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("key_CanalPermMsjeBRMSCamp")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("key_CanalPermMsjeBRMSCamp")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.key_OperaPermMsjeBRMSCamp = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("key_OperaPermMsjeBRMSCamp")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("key_OperaPermMsjeBRMSCamp")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.key_MsjAplicaCuotas = listaParamMejorasSisact
                     .Where(w => w.Valor1.Equals("key_MsjAplicaCuotas")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaParamMejorasSisact.Where(w => w.Valor1.Equals("key_MsjAplicaCuotas")).ToList()[0].Valor) : string.Empty;
                    //PROY-140585 F2 FIN

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585][ReadKeySettings.Key_FlagGeneralOfertaDefault] => ", ReadKeySettings.Key_FlagGeneralOfertaDefault), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585][ReadKeySettings.Key_CanalPermitidoOfertaDefault] => ", ReadKeySettings.Key_CanalPermitidoOfertaDefault), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585][ReadKeySettings.Key_OperacionPermitidaOfertaDefault] => ", ReadKeySettings.Key_OperacionPermitidaOfertaDefault), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585][ReadKeySettings.Key_DocumentosPermitidosOfertaDefault] => ", ReadKeySettings.Key_DocumentosPermitidosOfertaDefault), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585][ReadKeySettings.Key_CodigoOfertaDefault] => ", ReadKeySettings.Key_CodigoOfertaDefault), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585][ReadKeySettings.Key_IsPortabilidadOfertaDefault] => ", ReadKeySettings.Key_IsPortabilidadOfertaDefault), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585_F2][ReadKeySettings.key_FlagSMSxPDVEvaluacionPrePost] => ", ReadKeySettings.key_FlagSMSxPDVEvaluacionPrePost), null, null);//PROY-140585 F2
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585_F2][ReadKeySettings.key_flag_validacionPDV] => ", ReadKeySettings.key_flag_validacionPDV), null, null);//PROY-140585 F2
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585_F2][ReadKeySettings.key_CanalPermMsjeBRMSCamp] => ", ReadKeySettings.key_CanalPermMsjeBRMSCamp), null, null);//PROY-140585 F2
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585_F2][ReadKeySettings.key_OperaPermMsjeBRMSCamp] => ", ReadKeySettings.key_OperaPermMsjeBRMSCamp), null, null);//PROY-140585 F2
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140585_F2][ReadKeySettings.key_MsjAplicaCuotas] => ", ReadKeySettings.key_MsjAplicaCuotas), null, null);//PROY-140585 F2
                    objLog.CrearArchivolog("Fin PROY-140585 Carga de Parametros", null, null);
                }
                /*FIN PROY-140585- IDEA142064 Mejora en los sistemas de venta*/

                //IDEA-42590
                Int64 ConsConfigCampanaBeneficioPorta = Funciones.CheckInt64(ConfigurationManager.AppSettings["ConsConfigCampanaBeneficioPorta"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[IDEA-42590][PARAN_GRUPO]", Funciones.CheckStr(ConsConfigCampanaBeneficioPorta)), null, null);
                List<BEParametro> listConfigCampanaBeneficioPorta = new BLGeneral().ListaParametrosGrupo(ConsConfigCampanaBeneficioPorta);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[IDEA-42590][RESUL]", Funciones.CheckStr(listConfigCampanaBeneficioPorta.Count())), null, null);

                if (listConfigCampanaBeneficioPorta != null && listConfigCampanaBeneficioPorta.Count() > 0)
                {

                    string strConsConfigCampanaBEF_DSCTO_PORTA = listConfigCampanaBeneficioPorta
                      .Where(w => w.Valor1 == "ConsConfigCampanaBEF_DSCTO_PORTA").ToList().Count > 0 ?
                      Funciones.CheckStr(listConfigCampanaBeneficioPorta.Where(w => w.Valor1 == "ConsConfigCampanaBEF_DSCTO_PORTA").ToList()[0].Valor) : string.Empty;
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[IDEA-42590][strConsConfigCampanaBEF_DSCTO_PORTA]", Funciones.CheckStr(strConsConfigCampanaBEF_DSCTO_PORTA)), null, null);
                    
                    if (!string.IsNullOrEmpty(strConsConfigCampanaBEF_DSCTO_PORTA))
                    {
                        var arrConfigCampanaBef = strConsConfigCampanaBEF_DSCTO_PORTA.Split('|');
                        AppSettings.consBenefPortaDiasAntiguedad = Funciones.CheckInt(arrConfigCampanaBef[0]);
                        AppSettings.consBenefPortaCampanas = Funciones.CheckStr(arrConfigCampanaBef[1]);
                        AppSettings.consBenefPortaMinLineas = Funciones.CheckInt(arrConfigCampanaBef[2]);
                        AppSettings.consBenefPortaMaxLineas = Funciones.CheckInt(arrConfigCampanaBef[3]);
                        AppSettings.consBenefPortaMensajeError = Funciones.CheckStr(arrConfigCampanaBef[4]);
                        AppSettings.consBenefPortaMensajeWhiteList = Funciones.CheckStr(arrConfigCampanaBef[5]);
                        AppSettings.consBenefPortaCasoEspecialWhiteList = Funciones.CheckStr(arrConfigCampanaBef[6]);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[IDEA-42590][AppSettings.consBenefPortaDiasAntiguedad]", Funciones.CheckStr(AppSettings.consBenefPortaDiasAntiguedad)), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[IDEA-42590][AppSettings.consBenefPortaCampanas]", Funciones.CheckStr(AppSettings.consBenefPortaCampanas)), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[IDEA-42590][AppSettings.consBenefPortaMinLineas]", Funciones.CheckStr(AppSettings.consBenefPortaMinLineas)), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[IDEA-42590][AppSettings.consBenefPortaMaxLineas]", Funciones.CheckStr(AppSettings.consBenefPortaMaxLineas)), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[IDEA-42590][AppSettings.consBenefPortaMensajeError]", Funciones.CheckStr(AppSettings.consBenefPortaMensajeError)), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[IDEA-42590][AppSettings.consBenefPortaMensajeWhiteList]", Funciones.CheckStr(AppSettings.consBenefPortaMensajeWhiteList)), null, null);
                        objLog.CrearArchivolog(string.Format("{0}{1}", "[IDEA-42590][AppSettings.consBenefPortaCasoEspecialWhiteList]", Funciones.CheckStr(AppSettings.consBenefPortaCasoEspecialWhiteList)), null, null);
                    }
                }
                //IDEA-42590

                //INICIO INC000002628010 + 3
                objLog.CrearArchivolog(" === [INC000002628010] INICIO Cargar mensaje  de validacion operador cedente ====", null, null);

                Int64 codigovalidacion = Funciones.CheckInt64(ConfigurationManager.AppSettings["msjValidacionModalidad"].ToString());
                List<BEParametro> listmensaOper = new BLGeneral().ListaParametrosGrupo(codigovalidacion);
                if (listmensaOper != null && listmensaOper.Count() > 0)
                {
                    AppSettings.consMensValidaOpe = listmensaOper
                     .Where(w => w.Valor1.Equals("Key_validaOperador")).ToList().Count > 0 ?
                     Funciones.CheckStr(listmensaOper.Where(w => w.Valor1.Equals("Key_validaOperador")).ToList()[0].Valor) : "";

                    AppSettings.consValorPost = listmensaOper
                     .Where(w => w.Valor1.Equals("Key_validaPostpago")).ToList().Count > 0 ?
                     Funciones.CheckStr(listmensaOper.Where(w => w.Valor1.Equals("Key_validaPostpago")).ToList()[0].Valor) : "";

                    AppSettings.consValorPre = listmensaOper
                     .Where(w => w.Valor1.Equals("Key_validaPrepago")).ToList().Count > 0 ?
                     Funciones.CheckStr(listmensaOper.Where(w => w.Valor1.Equals("Key_validaPrepago")).ToList()[0].Valor) : "";

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002628010] [AppSettings.consMensValidaOpe]: ", Funciones.CheckStr(AppSettings.consMensValidaOpe)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002628010] [AppSettings.consValorPost]: ", Funciones.CheckStr(AppSettings.consValorPost)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000002628010] [AppSettings.consValorPre]: ", Funciones.CheckStr(AppSettings.consValorPre)), null, null);
                    objLog.CrearArchivolog(" === [INC000002628010] FIN Cargar mensaje  de validacion operador cedente  === ", null, null);
                }
                //FIN INC000002628010 + 3 

            //PROY-140383-INI
                Int64 ConsParamCodServvAdicionales = Funciones.CheckInt64(ConfigurationManager.AppSettings["ConsParamCodServvAdicionales"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140383][PARAN_GRUPO]", Funciones.CheckStr(ConsParamCodServvAdicionales)), null, null);
                List<BEParametro> ListParamCodServvAdicionales = new BLGeneral().ListaParametrosGrupo(ConsParamCodServvAdicionales);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140383][RESUL]", Funciones.CheckStr(ListParamCodServvAdicionales.Count())), null, null);

                if (ListParamCodServvAdicionales != null && ListParamCodServvAdicionales.Count > 0)
                {
                    AppSettings.consFlagServvAdicionales = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_FlagServvAdicionales").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_FlagServvAdicionales").ToList()[0].Valor) : string.Empty;

                    AppSettings.consMensValidaServ = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_ConsMensajeValidacion").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_ConsMensajeValidacion").ToList()[0].Valor) : string.Empty;

                    AppSettings.consProdExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_CodProductosExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_CodProductosExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.consMensServExcCaido = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_MensServicioCaido").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_MensServicioCaido").ToList()[0].Valor) : string.Empty;

                    AppSettings.consDescProceso = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_DescProceso").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_DescProceso").ToList()[0].Valor) : string.Empty;
                    //fffff

                    AppSettings.ConsCountryValidServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_CountryValidServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_CountryValidServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsDispositivoValidServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_DispositivoValidServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_DispositivoValidServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsLanguajeValidServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_LanguajeValidServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_LanguajeValidServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsModuloValidServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_ModuloValidServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_ModuloValidServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsMsgtypeValidServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_MsgtypeValidServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_MsgtypeValidServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsOperationValidServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_OperationValidServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_OperationValidServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsSystemValidServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_SystemValidServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_SystemValidServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsWsipValidServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_WsipValidServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_WsipValidServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsUsuarioEncripServExc = ListParamCodServvAdicionales
                    .Where(w => w.Valor1 == "Key_UsuarioEncripServExc").ToList().Count > 0 ?
                    Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_UsuarioEncripServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsContraEncripServExc = ListParamCodServvAdicionales
                    .Where(w => w.Valor1 == "Key_ContraEncripServExc").ToList().Count > 0 ?
                    Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_ContraEncripServExc").ToList()[0].Valor) : string.Empty;
                    //insert
                    AppSettings.ConsCountryInsertServExc = ListParamCodServvAdicionales
                    .Where(w => w.Valor1 == "Key_CountryInserServExc").ToList().Count > 0 ?
                    Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_CountryInserServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsDispositivoInsertServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_DispositivoInserServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_DispositivoInserServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsLanguajeInsertServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_LanguajeInserServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_LanguajeInserServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsModuloInsertServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_ModuloInserServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_ModuloInserServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsMsgtypeInsertServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_MsgtypeInserServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_MsgtypeInserServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsOperationInsertServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_OperationInserServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_OperationInserServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsSystemInsertServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_SystemInserServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_SystemInserServExc").ToList()[0].Valor) : string.Empty;

                    AppSettings.ConsWsipInsertServExc = ListParamCodServvAdicionales
                     .Where(w => w.Valor1 == "Key_WsipInserServExc").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodServvAdicionales.Where(w => w.Valor1 == "Key_WsipInserServExc").ToList()[0].Valor) : string.Empty;

                }
                //PROY-140383-FIN

                // INC000003427525 - INI

                Int64 ConsParamSecAntiFraude = Funciones.CheckInt64(ConfigurationManager.AppSettings["ConsParamSecAntiFraude"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000003427525][PARAN_GRUPO]", Funciones.CheckStr(ConsParamSecAntiFraude)), null, null);
                List<BEParametro> ListParamSecAntiFraude = new BLGeneral().ListaParametrosGrupo(ConsParamSecAntiFraude);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000003427525][RESUL]", Funciones.CheckStr(ListParamSecAntiFraude.Count())), null, null);

                if (ListParamSecAntiFraude != null && ListParamSecAntiFraude.Count > 0)
                {
                    AppSettings.Key_PaginaSec_AntiFraude = ListParamSecAntiFraude
                     .Where(w => w.Valor1 == "Key_PaginaSec_AntiFraude").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamSecAntiFraude.Where(w => w.Valor1 == "Key_PaginaSec_AntiFraude").ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INC000003427525] [AppSettings.Key_PaginaSec_AntiFraude]: ", Funciones.CheckStr(AppSettings.Key_PaginaSec_AntiFraude)), null, null);
                }

                // INC000003427525 - FIN

                //PROY-140618 - IDEA-142181 - Mejora Proceso de Portabilidad INI
                Int64 Key_ParanGrupoMejorasProcPorta = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_ParanGrupoMejorasProcPorta"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140618][PARAN_GRUPO]", Funciones.CheckStr(ConsParamCodServvAdicionales)), null, null);
                List<BEParametro> MejorasProcPortabilidad = new BLGeneral().ListaParametrosGrupo(Key_ParanGrupoMejorasProcPorta);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140618][RESUL]", Funciones.CheckStr(MejorasProcPortabilidad.Count())), null, null);

                if (MejorasProcPortabilidad != null && MejorasProcPortabilidad.Count > 0)
                {
                    AppSettings.Key_DiasAntiguedad = MejorasProcPortabilidad
                     .Where(w => w.Valor1 == "Key_DiasAntiguedad").ToList().Count > 0 ?
                     Funciones.CheckStr(MejorasProcPortabilidad.Where(w => w.Valor1 == "Key_DiasAntiguedad").ToList()[0].Valor) : string.Empty;

                    AppSettings.Key_CantidadRegistros = MejorasProcPortabilidad
                     .Where(w => w.Valor1 == "Key_CantidadRegistros").ToList().Count > 0 ?
                     Funciones.CheckStr(MejorasProcPortabilidad.Where(w => w.Valor1 == "Key_CantidadRegistros").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_TipoOperPermitida = MejorasProcPortabilidad
                     .Where(w => w.Valor1 == "Key_TipoOperPermitida").ToList().Count > 0 ?
                     Funciones.CheckStr(MejorasProcPortabilidad.Where(w => w.Valor1 == "Key_TipoOperPermitida").ToList()[0].Valor) : string.Empty;

                    AppSettings.Key_FlagPortaPermitido = MejorasProcPortabilidad
                     .Where(w => w.Valor1 == "Key_FlagPortaPermitido").ToList().Count > 0 ?
                     Funciones.CheckStr(MejorasProcPortabilidad.Where(w => w.Valor1 == "Key_FlagPortaPermitido").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_TipoProdPermitido = MejorasProcPortabilidad
                     .Where(w => w.Valor1 == "Key_TipoProdPermitido").ToList().Count > 0 ?
                     Funciones.CheckStr(MejorasProcPortabilidad.Where(w => w.Valor1 == "Key_TipoProdPermitido").ToList()[0].Valor) : string.Empty;

                }
                //PROY-140618 - IDEA-142181 - Mejora Proceso de Portabilidad FIN

CargarParametroFC();//INC000003048070 
                //INICIATIVA - 733 - INI - C15

                Int64 key_ParanGrupoCodIPTV = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoCodIPTV_733"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 733][PARAN_GRUPO]", Funciones.CheckStr(ConsParamCodServvAdicionales)), null, null);
                List<BEParametro> ListaCodIPTV = new BLGeneral().ListaParametrosGrupo(key_ParanGrupoCodIPTV);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 733][RESUL]", Funciones.CheckStr(ListaCodIPTV.Count())), null, null);

                if (ListaCodIPTV != null && ListaCodIPTV.Count > 0)
                {
                    ReadKeySettings.Key_CodEquipoIPTV = ListaCodIPTV
                     .Where(w => w.Valor1 == "Key_CodEquipoIPTV").ToList().Count > 0 ?
                     Funciones.CheckStr(ListaCodIPTV.Where(w => w.Valor1 == "Key_CodEquipoIPTV").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_CodClaroVideoIPTV = ListaCodIPTV
                     .Where(w => w.Valor1 == "Key_CodClaroVideoIPTV").ToList().Count > 0 ?
                     Funciones.CheckStr(ListaCodIPTV.Where(w => w.Valor1 == "Key_CodClaroVideoIPTV").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_strMensajeClaroVideoIPTV = ListaCodIPTV
                     .Where(w => w.Valor1 == "Key_strMensajeClaroVideoIPTV").ToList().Count > 0 ?
                     Funciones.CheckStr(ListaCodIPTV.Where(w => w.Valor1 == "Key_strMensajeClaroVideoIPTV").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_CodGrupoServicio = ListaCodIPTV
                     .Where(w => w.Valor1 == "Key_CodGrupoServicio").ToList().Count > 0 ?
                     Funciones.CheckStr(ListaCodIPTV.Where(w => w.Valor1 == "Key_CodGrupoServicio").ToList()[0].Valor) : string.Empty;

                    //INICIATIVA - 733 - FIN - C15
                    
                    //PROY-140657 INI
                objLog.CrearArchivolog("[WebComunes][CargarAppSettings][PROY-140657]INCIO ", null, null);

                Int64 Key_AfiliacionDEAU = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoAfiliacionDEAU"]);
                List<BEParametro> listaAfiliacionDEAU = new BLGeneral().ListaParametrosGrupo(Key_AfiliacionDEAU);

                objLog.CrearArchivolog(String.Format("[WebComunes][CargarAppSettings][PROY-140657]Se cargaron {0} parametros: ", Funciones.CheckStr(listaAfiliacionDEAU.Count())), null, null);

                if (listaAfiliacionDEAU != null && listaAfiliacionDEAU.Count() > 0)
                {
                    ReadKeySettings.key_idEntidadDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_idEntidadDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_idEntidadDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_OrigenSolicitudDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_OrigenSolicitudDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_OrigenSolicitudDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_idOrigenCuentaDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_idOrigenCuentaDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_idOrigenCuentaDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_idMonedaDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_idMonedaDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_idMonedaDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_OrigenAfiliacionDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_OrigenAfiliacionDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_OrigenAfiliacionDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_EstadoMpDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_EstadoMpDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_EstadoMpDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_EstadoEnvioLinkDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_EstadoEnvioLinkDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_EstadoEnvioLinkDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_EstadoVentaDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_EstadoVentaDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_EstadoVentaDEAU")).ToList()[0].Valor) : string.Empty;//

                    ReadKeySettings.key_ComentarioDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_ComentarioDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_ComentarioDEAU")).ToList()[0].Valor) : string.Empty;//

                    ReadKeySettings.key_EstadoAfiliacionAltaDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_EstadoAfiliacionAltaDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_EstadoAfiliacionAltaDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_TipoAutorizadoDocumentoDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_TipoAutorizadoDocumentoDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_TipoAutorizadoDocumentoDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_MsjCorreoObligatorioDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_MsjCorreoObligatorioDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_MsjCorreoObligatorioDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_MsjValidacionMontoDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_MsjValidacionMontoDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_MsjValidacionMontoDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_FlagAfiliacionDEAU=listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_FlagAfiliacionDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_FlagAfiliacionDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_TipoConsultaDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_TipoConsultaDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_TipoConsultaDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_TipoFlujoEnvioLinkDEAU=listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_TipoFlujoEnvioLinkDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_TipoFlujoEnvioLinkDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_CanalMPSisactDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_CanalMPSisactDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_CanalMPSisactDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_HorasFechaVencimientoDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_HorasFechaVencimientoDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_HorasFechaVencimientoDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_TipoTipiMovilDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_TipoTipiMovilDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_TipoTipiMovilDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_TipoTipi3PlayDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_TipoTipi3PlayDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_TipoTipi3PlayDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_FlagMontoDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_FlagMontoDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_FlagMontoDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_DocumentosMotorDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_DocumentosMotorDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_DocumentosMotorDEAU")).ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_FlagConsultarAltaDEAU = listaAfiliacionDEAU
                     .Where(w => w.Valor1.Equals("key_FlagConsultarAltaDEAU")).ToList().Count > 0 ?
                     Funciones.CheckStr(listaAfiliacionDEAU.Where(w => w.Valor1.Equals("key_FlagConsultarAltaDEAU")).ToList()[0].Valor) : string.Empty;
                    

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_idEntidadDEAU] => ", ReadKeySettings.key_idEntidadDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_TipoSolicitudDEAU] => ", ReadKeySettings.key_OrigenSolicitudDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_idOrigenCuentaDEAU] => ", ReadKeySettings.key_idOrigenCuentaDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_idMonedaDEAU] => ", ReadKeySettings.key_idMonedaDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_OrigenAfiliacionDEAU] => ", ReadKeySettings.key_OrigenAfiliacionDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_EstadoMpDEAU] => ", ReadKeySettings.key_EstadoMpDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_EstadoEnvioLinkDEAU] => ", ReadKeySettings.key_EstadoEnvioLinkDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_EstadoVentaDEAU] => ", ReadKeySettings.key_EstadoVentaDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_ComentarioDEAU] => ", ReadKeySettings.key_ComentarioDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_EstadoAfiliacionAltaDEAU] => ", ReadKeySettings.key_EstadoAfiliacionAltaDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_TipoAutorizadoDocumentoDEAU] => ", ReadKeySettings.key_TipoAutorizadoDocumentoDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_MsjCorreoObligatorioDEAU] => ", ReadKeySettings.key_MsjCorreoObligatorioDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_MsjValidacionMontoDEAU] => ", ReadKeySettings.key_MsjValidacionMontoDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_FlagAfiliacionDEAU] => ", ReadKeySettings.key_FlagAfiliacionDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_TipoConsultaDEAU] => ", ReadKeySettings.key_TipoConsultaDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_TipoFlujoEnvioLinkDEAU] => ", ReadKeySettings.key_TipoFlujoEnvioLinkDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_CanalMPSisactDEAU] => ", ReadKeySettings.key_CanalMPSisactDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_HorasFechaVencimientoDEAU] => ", ReadKeySettings.key_HorasFechaVencimientoDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_TipoTipiMovilDEAU] => ", ReadKeySettings.key_TipoTipiMovilDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_TipoTipi3PlayDEAU] => ", ReadKeySettings.key_TipoTipi3PlayDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_FlagMontoDEAU] => ", ReadKeySettings.key_FlagMontoDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_DocumentosMotorDEAU] => ", ReadKeySettings.key_DocumentosMotorDEAU), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[WebComunes][CargarAppSettings][PROY-140657][ReadKeySettings.key_FlagConsultarAltaDEAU] => ", ReadKeySettings.key_FlagConsultarAltaDEAU), null, null);                    
                    objLog.CrearArchivolog("[WebComunes][CargarAppSettings][PROY-140657]FIN ", null, null);
                    
                }
                //PROY-140657 FIN
                }

                // INICIATIVA - 803 - INI
                try
                {
                Int64 key_ParanGrupoTiendaVirtual = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoTiendaVirtual"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] ==> ", Funciones.CheckStr(key_ParanGrupoTiendaVirtual)), null, null);
                List<BEParametro> ListaTiendaVirtual = new BLGeneral().ListaParametrosGrupo(key_ParanGrupoTiendaVirtual);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][RESUL]", Funciones.CheckStr(ListaTiendaVirtual.Count())), null, null);

                if (ListaTiendaVirtual != null && ListaTiendaVirtual.Count > 0)
                {
                    ReadKeySettings.Key_FlagAprobPrecio = ListaTiendaVirtual
                     .Where(w => w.Valor1 == "Key_FlagAprobPrecio").ToList().Count > 0 ?
                     Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_FlagAprobPrecio").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_MsjFactorSubsidio = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_MsjFactorSubsidio").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_MsjFactorSubsidio").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_MsjIngresarIdPedido = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_MsjIngresarIdPedido").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_MsjIngresarIdPedido").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_PorcentSubsidio = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_PorcentSubsidio").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_PorcentSubsidio").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_MsjErrorExcepPrec = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_MsjErrorExcepPrec").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_MsjErrorExcepPrec").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_CodEstadoAprobFactorSub = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_CodEstadoAprobFactorSub").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_CodEstadoAprobFactorSub").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_DesEstadoAprobFactorSub = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_DesEstadoAprobFactorSub").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_DesEstadoAprobFactorSub").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_MsjSecEnviadoPoolExcepcionPrecio = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_MsjSecEnviadoPoolExcepcionPrecio").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_MsjSecEnviadoPoolExcepcionPrecio").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_CodEstadoSinExcepcionPrecio = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_CodEstadoSinExcepcionPrecio").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_CodEstadoSinExcepcionPrecio").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_CodEstadoPendAprobacion = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_CodEstadoPendAprobacion").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_CodEstadoPendAprobacion").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_IdFlujoEvalConsumer = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_IdFlujoEvalConsumer").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_IdFlujoEvalConsumer").ToList()[0].Valor) : string.Empty;
                    
                   ReadKeySettings.Key_EstadosSolicitud = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_EstadosSolicitud").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_EstadosSolicitud").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_TipoBusquedaPool = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_TipoBusquedaPool").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_TipoBusquedaPool").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.key_EstadosVisualizaBotones = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "key_EstadosVisualizaBotones").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "key_EstadosVisualizaBotones").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_EstadoPendValidacion = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_EstadoPendValidacion").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_EstadoPendValidacion").ToList()[0].Valor) : string.Empty;

                     ReadKeySettings.keyPerfilesAprobadores = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "keyPerfilesAprobadores").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "keyPerfilesAprobadores").ToList()[0].Valor) : string.Empty;

                  ReadKeySettings.keyPerfilesValidadores = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "keyPerfilesValidadores").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "keyPerfilesValidadores").ToList()[0].Valor) : string.Empty;

                  ReadKeySettings.Key_FlagApagadoExcepcionPrecio = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_FlagApagadoExcepcionPrecio").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_FlagApagadoExcepcionPrecio").ToList()[0].Valor) : string.Empty;
               
                  ReadKeySettings.Key_FlagApagadoValidacionSubsidio = ListaTiendaVirtual
                    .Where(w => w.Valor1 == "Key_FlagApagadoValidacionSubsidio").ToList().Count > 0 ?
                    Funciones.CheckStr(ListaTiendaVirtual.Where(w => w.Valor1 == "Key_FlagApagadoValidacionSubsidio").ToList()[0].Valor) : string.Empty;

                    
                }

                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_FlagAprobPrecio ==> ", Funciones.CheckStr(ReadKeySettings.Key_FlagAprobPrecio)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_MsjFactorSubsidio ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjFactorSubsidio)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_MsjIngresarIdPedido ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjIngresarIdPedido)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_PorcentSubsidio ==> ", Funciones.CheckStr(ReadKeySettings.Key_PorcentSubsidio)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_MsjErrorExcepPrec ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjErrorExcepPrec)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_CodEstadoAprobFactorSub ==> ", Funciones.CheckStr(ReadKeySettings.Key_CodEstadoAprobFactorSub)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_DesEstadoAprobFactorSub ==> ", Funciones.CheckStr(ReadKeySettings.Key_DesEstadoAprobFactorSub)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_MsjSecEnviadoPoolExcepcionPrecio ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjSecEnviadoPoolExcepcionPrecio)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_CodEstadoSinExcepcionPrecio ==> ", Funciones.CheckStr(ReadKeySettings.Key_CodEstadoSinExcepcionPrecio)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_CodEstadoSinExcepcionPrecio ==> ", Funciones.CheckStr(ReadKeySettings.Key_CodEstadoSinExcepcionPrecio)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_CodEstadoPendAprobacion ==> ", Funciones.CheckStr(ReadKeySettings.Key_CodEstadoPendAprobacion)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_EstadoPendValidacion ==> ", Funciones.CheckStr(ReadKeySettings.Key_EstadoPendValidacion)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_IdFlujoEvalConsumer ==> ", Funciones.CheckStr(ReadKeySettings.Key_IdFlujoEvalConsumer)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_EstadosSolicitud ==> ", Funciones.CheckStr(ReadKeySettings.Key_EstadosSolicitud)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_TipoBusquedaPool ==> ", Funciones.CheckStr(ReadKeySettings.Key_TipoBusquedaPool)), null, null);                
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - keyPerfilesAprobadores ==> ", Funciones.CheckStr(ReadKeySettings.keyPerfilesAprobadores)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - keyPerfilesValidadores ==> ", Funciones.CheckStr(ReadKeySettings.keyPerfilesValidadores)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_FlagApagadoExcepcionPrecio ==> ", Funciones.CheckStr(ReadKeySettings.Key_FlagApagadoExcepcionPrecio)), null, null);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Key_FlagApagadoValidacionSubsidio ==> ", Funciones.CheckStr(ReadKeySettings.Key_FlagApagadoValidacionSubsidio)), null, null);

                }
                catch (Exception ex)
                {
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Message  ==> ", Funciones.CheckStr(ex.Message)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - StackTrace  ==> ", Funciones.CheckStr(ex.StackTrace)), null, null);
                }
                // INICIATIVA - 803 - FIN

                //[PROY-140600] INI
                Int64 ParanGrupoContabilizarLineas = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_ContabilizacionLineasExt"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140600][PARAN_GRUPO]", Funciones.CheckStr(ParanGrupoContabilizarLineas)), null, null);
                List<BEParametro> ListParanGrupoContabilizarLineas = new BLGeneral().ListaParametrosGrupo(ParanGrupoContabilizarLineas);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140600][RESUL]", Funciones.CheckStr(ListParanGrupoContabilizarLineas.Count())), null, null);

                if (ListParanGrupoContabilizarLineas != null && ListParanGrupoContabilizarLineas.Count > 0)
                {
                    AppSettings.Key_CLFlagGeneralPost = ListParanGrupoContabilizarLineas
                     .Where(w => w.Valor1 == "Key_CLFlagGeneralPost").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoContabilizarLineas.Where(w => w.Valor1 == "Key_CLFlagGeneralPost").ToList()[0].Valor) : string.Empty;

                    AppSettings.Key_CLCodCanalPermitido=ListParanGrupoContabilizarLineas
                     .Where(w => w.Valor1 == "Key_CLCodCanalPermitido").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoContabilizarLineas.Where(w => w.Valor1 == "Key_CLCodCanalPermitido").ToList()[0].Valor) : string.Empty;

                    AppSettings.Key_CLCodDocPermitido = ListParanGrupoContabilizarLineas
                     .Where(w => w.Valor1 == "Key_CLCodDocPermitido").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoContabilizarLineas.Where(w => w.Valor1 == "Key_CLCodDocPermitido").ToList()[0].Valor) : string.Empty;

                    AppSettings.Key_CLTipoVentaPos= ListParanGrupoContabilizarLineas
                        .Where(w => w.Valor1 == "Key_CLTipoVentaPos").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoContabilizarLineas.Where(w => w.Valor1 == "Key_CLTipoVentaPos").ToList()[0].Valor) : string.Empty;

                    AppSettings.Key_CLProductoPermitidoPost = ListParanGrupoContabilizarLineas
                        .Where(w => w.Valor1 == "Key_CLProductoPermitidoPos").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoContabilizarLineas.Where(w => w.Valor1 == "Key_CLProductoPermitidoPos").ToList()[0].Valor) : string.Empty;

                    AppSettings.Key_CLOperacionPermitidaPost = ListParanGrupoContabilizarLineas
                        .Where(w => w.Valor1 == "Key_CLOperacionPermitidaPos").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoContabilizarLineas.Where(w => w.Valor1 == "Key_CLOperacionPermitidaPos").ToList()[0].Valor) : string.Empty;

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140600][WebComunes][CargarAppSettings()] Key_CLCodCanalPermitido", Funciones.CheckStr(AppSettings.Key_CLCodCanalPermitido)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140600][WebComunes][CargarAppSettings()] Key_CLFlagGeneralPost: ", Funciones.CheckStr(AppSettings.Key_CLFlagGeneralPost)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140600][WebComunes][CargarAppSettings()] Key_CLCodDocPermitido: ", Funciones.CheckStr(AppSettings.Key_CLTipoVentaPos)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140600][WebComunes][CargarAppSettings()] Key_CLTipoVentaPos: ", Funciones.CheckStr(AppSettings.Key_CLTipoVentaPos)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140600][WebComunes][CargarAppSettings()] Key_CLProductoPermitidoPost: ", Funciones.CheckStr(AppSettings.Key_CLProductoPermitidoPost)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140600][WebComunes][CargarAppSettings()] Key_CLOperacionPermitidaPost: ", Funciones.CheckStr(AppSettings.Key_CLOperacionPermitidaPost)), null, null);
                }
                //[PROY-140600] FIN

                // PROY-140736 INI
                try
                {
                    Int64 key_ParanBuyback = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_CodCampaniaBuyBack"].ToString());
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY 308[PARAN_GRUPO] ==> ", Funciones.CheckStr(key_ParanBuyback)), null, null);
                    List<BEParametro> ListaBuyback = new BLGeneral().ListaParametrosGrupo(key_ParanBuyback);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY 308][RESUL]", Funciones.CheckStr(ListaBuyback.Count())), null, null);

                    if (ListaBuyback != null && ListaBuyback.Count > 0)
                    {
                        ReadKeySettings.Key_CodCampaniaBuyBack = ListaBuyback
                         .Where(w => w.Valor1 == "Key_CodCampaniaBuyBack").ToList().Count > 0 ?
                         Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_CodCampaniaBuyBack").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_Max_Length_Cupon = ListaBuyback
                       .Where(w => w.Valor1 == "Key_Max_Length_Cupon").ToList().Count > 0 ?
                       Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Max_Length_Cupon").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_Min_Length_IMEI = ListaBuyback
                       .Where(w => w.Valor1 == "Key_Min_Length_IMEI").ToList().Count > 0 ?
                       Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Min_Length_IMEI").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_Max_Length_IMEI = ListaBuyback
                       .Where(w => w.Valor1 == "Key_Max_Length_IMEI").ToList().Count > 0 ?
                       Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Max_Length_IMEI").ToList()[0].Valor) : string.Empty;


                        ReadKeySettings.Key_Msj_Error_LP_BuyBack = ListaBuyback
                       .Where(w => w.Valor1 == "Key_Msj_Error_LP_BuyBack").ToList().Count > 0 ?
                       Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Msj_Error_LP_BuyBack").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_Msj_Error_CBO_BuyBack = ListaBuyback
                     .Where(w => w.Valor1 == "Key_Msj_Error_CBO_BuyBack").ToList().Count > 0 ?
                     Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Msj_Error_CBO_BuyBack").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_Msj_Error_Cupon_BuyBack = ListaBuyback
                     .Where(w => w.Valor1 == "Key_Msj_Error_Cupon_BuyBack").ToList().Count > 0 ?
                     Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Msj_Error_Cupon_BuyBack").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_Msj_Error_Fila_BuyBack = ListaBuyback
                     .Where(w => w.Valor1 == "Key_Msj_Error_Fila_BuyBack").ToList().Count > 0 ?
                     Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Msj_Error_Fila_BuyBack").ToList()[0].Valor) : string.Empty;
                     
                      ReadKeySettings.Key_Msj_Error_Igual_Cupon = ListaBuyback
                     .Where(w => w.Valor1 == "Key_Msj_Error_Igual_Cupon").ToList().Count > 0 ?
                     Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Msj_Error_Igual_Cupon").ToList()[0].Valor) : string.Empty;
                        
                      ReadKeySettings.Key_Msj_Error_Igual_Imei = ListaBuyback
                   .Where(w => w.Valor1 == "Key_Msj_Error_Igual_Imei").ToList().Count > 0 ?
                   Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Msj_Error_Igual_Imei").ToList()[0].Valor) : string.Empty;

                      ReadKeySettings.Key_Ingr_Cupon_Buyback = ListaBuyback
                     .Where(w => w.Valor1 == "Key_Ingr_Cupon_Buyback").ToList().Count > 0 ?
                     Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Ingr_Cupon_Buyback").ToList()[0].Valor) : string.Empty;

                      ReadKeySettings.Key_Ingr_IMEI_Buyback = ListaBuyback
                .Where(w => w.Valor1 == "Key_Ingr_IMEI_Buyback").ToList().Count > 0 ?
                Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Ingr_IMEI_Buyback").ToList()[0].Valor) : string.Empty;

                      ReadKeySettings.Key_Sel_Equipo_Buyback = ListaBuyback
                .Where(w => w.Valor1 == "Key_Sel_Equipo_Buyback").ToList().Count > 0 ?
                Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_Sel_Equipo_Buyback").ToList()[0].Valor) : string.Empty;

                 ReadKeySettings.Key_CuponConSecPagado = ListaBuyback
                .Where(w => w.Valor1 == "Key_CuponConSecPagado").ToList().Count > 0 ?
                Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_CuponConSecPagado").ToList()[0].Valor) : string.Empty;
                
                 ReadKeySettings.Key_CuponConSecPendPago = ListaBuyback
                .Where(w => w.Valor1 == "Key_CuponConSecPendPago").ToList().Count > 0 ?
                Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_CuponConSecPendPago").ToList()[0].Valor) : string.Empty;

                 ReadKeySettings.Key_CuponExpirado = ListaBuyback
                .Where(w => w.Valor1 == "Key_CuponExpirado").ToList().Count > 0 ?
                Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_CuponExpirado").ToList()[0].Valor) : string.Empty;

                 ReadKeySettings.Key_CuponSecAprobado = ListaBuyback
                .Where(w => w.Valor1 == "Key_CuponSecAprobado").ToList().Count > 0 ?
                Funciones.CheckStr(ListaBuyback.Where(w => w.Valor1 == "Key_CuponSecAprobado").ToList()[0].Valor) : string.Empty;
                        

                    }

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_CodCampaniaBuyBack ==> ", Funciones.CheckStr(ReadKeySettings.Key_CodCampaniaBuyBack)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Max_Length_Cupon ==> ", Funciones.CheckStr(ReadKeySettings.Key_Max_Length_Cupon)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Min_Length_IMEI ==> ", Funciones.CheckStr(ReadKeySettings.Key_Min_Length_IMEI)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Max_Length_IMEI ==> ", Funciones.CheckStr(ReadKeySettings.Key_Max_Length_IMEI)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Msj_Error_LP_BuyBack ==> ", Funciones.CheckStr(ReadKeySettings.Key_Msj_Error_LP_BuyBack)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Msj_Error_CBO_BuyBack ==> ", Funciones.CheckStr(ReadKeySettings.Key_Msj_Error_CBO_BuyBack)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Msj_Error_Cupon_BuyBack ==> ", Funciones.CheckStr(ReadKeySettings.Key_Msj_Error_Cupon_BuyBack)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Msj_Error_Fila_BuyBack ==> ", Funciones.CheckStr(ReadKeySettings.Key_Msj_Error_Fila_BuyBack)), null, null);
                     objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Msj_Error_Igual_Cupon ==> ", Funciones.CheckStr(ReadKeySettings.Key_Msj_Error_Igual_Cupon)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Msj_Error_Igual_Imei ==> ", Funciones.CheckStr(ReadKeySettings.Key_Msj_Error_Igual_Imei)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Ingr_Cupon_Buyback ==> ", Funciones.CheckStr(ReadKeySettings.Key_Ingr_Cupon_Buyback)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Ingr_IMEI_Buyback ==> ", Funciones.CheckStr(ReadKeySettings.Key_Ingr_IMEI_Buyback)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_Sel_Equipo_Buyback ==> ", Funciones.CheckStr(ReadKeySettings.Key_Sel_Equipo_Buyback)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_CuponConSecPagado ==> ", Funciones.CheckStr(ReadKeySettings.Key_CuponConSecPagado)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_CuponConSecPendPago ==> ", Funciones.CheckStr(ReadKeySettings.Key_CuponConSecPendPago)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_CuponExpirado ==> ", Funciones.CheckStr(ReadKeySettings.Key_CuponExpirado)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Key_CuponSecAprobado ==> ", Funciones.CheckStr(ReadKeySettings.Key_CuponSecAprobado)), null, null);
                }

                catch (Exception ex)
                {
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736][PARAN_GRUPO] - Message  ==> ", Funciones.CheckStr(ex.Message)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140736]][PARAN_GRUPO] - StackTrace  ==> ", Funciones.CheckStr(ex.StackTrace)), null, null);
                }
                // PROY-140736 FIN

				//PROY-140546 Cobro Anticipado de Instalacion
                Int64 ConsParamCodCobroAnticipadoInst = Funciones.CheckInt64(ConfigurationManager.AppSettings["codigoParamCobroAnticipadoInstalacion"]);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140546][PARAN_GRUPO]", Funciones.CheckStr(ConsParamCodCobroAnticipadoInst)), null, null);
                List<BEParametro> ListParamCodCobroAnticipado = new BLGeneral().ListaParametrosGrupo(ConsParamCodCobroAnticipadoInst);

                if (ListParamCodCobroAnticipado != null && ListParamCodCobroAnticipado.Count > 0)
                {
                    ReadKeySettings.Key_EstadosPago = ListParamCodCobroAnticipado
                     .Where(w => w.Valor1 == "Key_EstadosPago").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_EstadosPago").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.Key_OperationConsultaHistorico = ListParamCodCobroAnticipado
                     .Where(w => w.Valor1 == "Key_OperationConsultaHistorico").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_OperationConsultaHistorico").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.ConsFlagAplicaCAI = ListParamCodCobroAnticipado
                     .Where(w => w.Valor1 == "Key_FlagAplicaCAI").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_FlagAplicaCAI").ToList()[0].Valor) : string.Empty;

                    ReadKeySettings.ConsTiempoSecPendientePagoLink = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_TiempoSecPendientePagoLink").ToList().Count > 0 ?
                         Funciones.CheckInt(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_TiempoSecPendientePagoLink").ToList()[0].Valor) : 0;

                    ReadKeySettings.ConsConsumerConsultaPA = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_Consumer").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_Consumer").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsCountryConsultaPA = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_Country").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_Country").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsDispositivoConsultaPA = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_Dispositivo").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_Dispositivo").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsLanguageConsultaPA = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_Language").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_Language").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsModuloConsultaPA = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_Modulo").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_Modulo").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsMsgTypeConsultaPA = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_MsgType").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_MsgType").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsOperationConsultaPA = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_Operation").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_Operation").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsSystemConsultaPA = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_System").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_System").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsWsIpConsultaPA = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_wsIp").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_wsIp").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsCodigoPDVTeleventas = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_PDV_TELEVENTAS").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_PDV_TELEVENTAS").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsMsjValidacionSecPendPagoLink = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_MsjValidacionExisteSecPendientePago").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_MsjValidacionExisteSecPendientePago").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsMsjValidacionSubFormularioCAI = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_MsjValidacionCamposObligatoriosCAI").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_MsjValidacionCamposObligatoriosCAI").ToList()[0].Valor) : "";

                    String nombreServer = Dns.GetHostName();
                    String ipServer = System.Net.Dns.GetHostEntry(nombreServer).AddressList[0].ToString();
                    
                    ReadKeySettings.ConsCurrentIP = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_wsIp").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_wsIp").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "IP ConsCurrentIP: ", ipServer), null, null);

                    ReadKeySettings.ConsCurrentUser = Sisact_Webbase.CurrentUsers;

                    ReadKeySettings.ConsCadValorFormaPago = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_CadValorFormaPago").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_CadValorFormaPago").ToList()[0].Valor) : "";

                    ReadKeySettings.Key_CanalesPermitidosCAI = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_CanalesPermitidosCAI").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_CanalesPermitidosCAI").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140546] [AppSettings.Key_CanalesPermitidosCAI]: ", Funciones.CheckStr(ReadKeySettings.Key_CanalesPermitidosCAI)), null, null);

                    ReadKeySettings.Key_MontoDescuentoPorFullClaroCAI = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_MontoDescuentoPorFullClaroCAI").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_MontoDescuentoPorFullClaroCAI").ToList()[0].Valor) : "";

                    /* rmr ca fase 2: ini */
                    ReadKeySettings.ConsCadEstadoAnulado = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_EstadoAnulado").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_EstadoAnulado").ToList()[0].Valor) : "";

                    ReadKeySettings.ConsCadEstadoPagado = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_EstadoPagado").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_EstadoPagado").ToList()[0].Valor) : "";
                    /* rmr ca fase 2: fin */

                    //INICIO FALLAS PROY-140546
                    ReadKeySettings.Key_MensajeMaiMayor = ListParamCodCobroAnticipado
                         .Where(w => w.Valor1 == "Key_MensajeMaiMayor").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodCobroAnticipado.Where(w => w.Valor1 == "Key_MensajeMaiMayor").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140546] [AppSettings.Key_MensajeMaiMayor]: ", Funciones.CheckStr(ReadKeySettings.Key_MensajeMaiMayor)), null, null);
                    //FIN FALLAS PROY-140546
                }
                //PROY-140546 Cobro Anticipado de Instalacion

                //INICIO INICIATIVA-932
                Int64 paranGrupoMovilidadIFI = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_MovilidadIFI"].ToString());
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-932][PARAN_GRUPO]", Funciones.CheckStr(paranGrupoMovilidadIFI)), null, null);
                List<BEParametro> ListParanGrupoMovilidadIFI = new BLGeneral().ListaParametrosGrupo(paranGrupoMovilidadIFI);
                objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-932][RESUL]", Funciones.CheckStr(ListParanGrupoMovilidadIFI.Count())), null, null);

                if (ListParanGrupoMovilidadIFI != null && ListParanGrupoMovilidadIFI.Count > 0)
                {
                    ReadKeySettings.Key_FlagGeneralCobertura = ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_FlagGeneralCobertura").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_FlagGeneralCobertura").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-932][WebComunes][CargarAppSettings()] Key_FlagGeneralCobertura", Funciones.CheckStr(ReadKeySettings.Key_FlagGeneralCobertura)), null, null);

                    ReadKeySettings.Key_MsgCoberturaIFI = ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgCoberturaIFI").ToList().Count > 0 ?
                      Funciones.CheckStr(ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgCoberturaIFI").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-992][WebComunes][CargarAppSettings()] Key_MsgCoberturaIFI", Funciones.CheckStr(ReadKeySettings.Key_MsgCoberturaIFI)), null, null);

                    ReadKeySettings.Key_MsgParametrosIFI = ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgParametrosIFI").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgParametrosIFI").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-992][WebComunes][CargarAppSettings()] Key_MsgParametrosIFI", Funciones.CheckStr(ReadKeySettings.Key_MsgParametrosIFI)), null, null);

                    ReadKeySettings.Key_MsgAreaCoberturaIFI = ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgAreaCoberturaIFI").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgAreaCoberturaIFI").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-992][WebComunes][CargarAppSettings()] Key_MsgAreaCoberturaIFI", Funciones.CheckStr(ReadKeySettings.Key_MsgAreaCoberturaIFI)), null, null);

                    ReadKeySettings.Key_MsgErrorServicioIFI = ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgErrorServicioIFI").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgErrorServicioIFI").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-992][WebComunes][CargarAppSettings()] Key_MsgErrorServicioIFI", Funciones.CheckStr(ReadKeySettings.Key_MsgErrorServicioIFI)), null, null);

                    ReadKeySettings.Key_MsgErrorConsultaIFI = ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgErrorConsultaIFI").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgErrorConsultaIFI").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-992][WebComunes][CargarAppSettings()] Key_MsgErrorConsultaIFI", Funciones.CheckStr(ReadKeySettings.Key_MsgErrorConsultaIFI)), null, null);

                    ReadKeySettings.Key_MsgErrorTimeOutIFI = ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgErrorTimeOutIFI").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgErrorTimeOutIFI").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-992][WebComunes][CargarAppSettings()] Key_MsgErrorTimeOutIFI", Funciones.CheckStr(ReadKeySettings.Key_MsgErrorTimeOutIFI)), null, null);

                    ReadKeySettings.Key_MsgErrorDisponibilidadIFI = ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgErrorDisponibilidadIFI").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgErrorDisponibilidadIFI").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-992][WebComunes][CargarAppSettings()] Key_MsgErrorDisponibilidadIFI", Funciones.CheckStr(ReadKeySettings.Key_MsgErrorDisponibilidadIFI)), null, null);

                    ReadKeySettings.Key_MsgErrorInesperadoIFI = ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgErrorInesperadoIFI").ToList().Count > 0 ?
                     Funciones.CheckStr(ListParanGrupoMovilidadIFI.Where(w => w.Valor1 == "Key_MsgErrorInesperadoIFI").ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-992][WebComunes][CargarAppSettings()] Key_MsgErrorInesperadoIFI", Funciones.CheckStr(ReadKeySettings.Key_MsgErrorInesperadoIFI)), null, null);
                
                }
                //FIN INICIATIVA-932

                //PROY-140715- INI ANGEL
                try
                {
                    Int64 Key_ParanGrupoContingencia = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_ParanGrupoContingencia"].ToString());
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715][Key_ParanGrupoContingencia] => ", Funciones.CheckStr(Key_ParanGrupoContingencia)), null, null);
                    List<BEParametro> listParametrosContingencia = new BLGeneral().ListaParametrosGrupo(Key_ParanGrupoContingencia);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715]", Funciones.CheckStr(listParametrosContingencia.Count())), null, null);

                    if (listParametrosContingencia != null && listParametrosContingencia.Count > 0)
                    {
                        ReadKeySettings.Key_MsjInformativo = listParametrosContingencia
                         .Where(w => w.Valor1 == "Key_MsjInformativo").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1 == "Key_MsjInformativo").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjRecordatorio = listParametrosContingencia
                       .Where(w => w.Valor1 == "Key_MsjRecordatorio").ToList().Count > 0 ?
                       Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1 == "Key_MsjRecordatorio").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjMostrar = listParametrosContingencia
                       .Where(w => w.Valor1 == "Key_MsjMostrar").ToList().Count > 0 ?
                       Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1 == "Key_MsjMostrar").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjMostrarOperacion = listParametrosContingencia
                       .Where(w => w.Valor1 == "Key_MsjMostrarOperacion").ToList().Count > 0 ?
                       Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1 == "Key_MsjMostrarOperacion").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjMostrarVenta = listParametrosContingencia
                        .Where(w => w.Valor1 == "Key_MsjMostrarVenta").ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1 == "Key_MsjMostrarVenta").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_OperacionAlta = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_OperacionAlta")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_OperacionAlta")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_OperacionPorta = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_OperacionPorta")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_OperacionPorta")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_OperacionRepo = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_OperacionRepo")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_OperacionRepo")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_OperacionReno = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_OperacionReno")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_OperacionReno")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_OperacionMigracion = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_OperacionMigracion")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_OperacionMigracion")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PreOperacionAlta = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_PreOperacionAlta")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_PreOperacionAlta")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PreOperacionPorta = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_PreOperacionPorta")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_PreOperacionPorta")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PreOperacionRenoChip = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_PreOperacionRenoChip")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_PreOperacionRenoChip")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PreOperacionRenoPack = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_PreOperacionRenoPack")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_PreOperacionRenoPack")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PreOperacionRenoEqui = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_PreOperacionRenoEqui")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_PreOperacionRenoEqui")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_SistemaSISACTPRE = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_SistemaSISACTPRE")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_SistemaSISACTPRE")).ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_SistemaSISACT = listParametrosContingencia
                        .Where(w => w.Valor1.Equals("Key_SistemaSISACT")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosContingencia.Where(w => w.Valor1.Equals("Key_SistemaSISACT")).ToList()[0].Valor) : string.Empty;

                    }

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715][PARAN_GRUPO] - Key_MsjInformativo ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjInformativo)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715][PARAN_GRUPO] - Key_MsjRecordatorio ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjRecordatorio)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715][PARAN_GRUPO] - Key_MsjMostrar ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjMostrar)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715][PARAN_GRUPO] - Key_MsjMostrarOperacion ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjMostrarOperacion)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715][PARAN_GRUPO] - Key_MsjMostrarVenta ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjMostrarVenta)), null, null);
                    
               // INICIATIVA - 920 - INI
                try
                {
                    Int64 key_ParanGrupoTiendaVirtual = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoTiendaVirtual"].ToString());
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] ==> ", Funciones.CheckStr(key_ParanGrupoTiendaVirtual)), null, null);
                    List<BEParametro> ListaTiendaVirtual = new BLGeneral().ListaParametrosGrupo(key_ParanGrupoTiendaVirtual);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][RESUL]", Funciones.CheckStr(ListaTiendaVirtual.Count())), null, null);

                    /*IDEA142785 - Desarrollo nuevas Modalidades*/

                    Int64 Key_ParanGrupoModalidad = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_ParanGrupoModalidad"].ToString());
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 920][PARAN_GRUPO] ==> ", Funciones.CheckStr(Key_ParanGrupoModalidad)), null, null);
                    List<BEParametro> listParametrosModalidades = new BLGeneral().ListaParametrosGrupo(Key_ParanGrupoModalidad);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 920][RESUL]", Funciones.CheckStr(listParametrosModalidades.Count())), null, null);

                    if (listParametrosModalidades != null && listParametrosModalidades.Count() > 0)
                    {
                        //INICIATIVA 920 INI
                        AppSettings.KeyModalidadDefecto = listParametrosModalidades
                        .Where(w => w.Valor1.Equals("KeyModalidadDefecto")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosModalidades.Where(w => w.Valor1.Equals("KeyModalidadDefecto")).ToList()[0].Valor) : string.Empty;

                        AppSettings.Key_Canal_Permitido = listParametrosModalidades
                       .Where(w => w.Valor1.Equals("Key_Canal_Permitido")).ToList().Count > 0 ?
                       Funciones.CheckStr(listParametrosModalidades.Where(w => w.Valor1.Equals("Key_Canal_Permitido")).ToList()[0].Valor) : string.Empty;

                        AppSettings.Val_Canal_Permitido = listParametrosModalidades
                       .Where(w => w.Valor1.Equals("Val_Canal_Permitido")).ToList().Count > 0 ?
                       Funciones.CheckStr(listParametrosModalidades.Where(w => w.Valor1.Equals("Val_Canal_Permitido")).ToList()[0].Valor) : string.Empty;

                        AppSettings.Key_PDV_Permitido = listParametrosModalidades
                      .Where(w => w.Valor1.Equals("Key_PDV_Permitido")).ToList().Count > 0 ?
                      Funciones.CheckStr(listParametrosModalidades.Where(w => w.Valor1.Equals("Key_PDV_Permitido")).ToList()[0].Valor) : string.Empty;

                        AppSettings.Val_PDV_Permitido = listParametrosModalidades
                      .Where(w => w.Valor1.Equals("Val_PDV_Permitido")).ToList().Count > 0 ?
                      Funciones.CheckStr(listParametrosModalidades.Where(w => w.Valor1.Equals("Val_PDV_Permitido")).ToList()[0].Valor) : string.Empty;


                        AppSettings.Key_Operacion_Permitido = listParametrosModalidades
                       .Where(w => w.Valor1.Equals("Key_Operacion_Permitido")).ToList().Count > 0 ?
                       Funciones.CheckStr(listParametrosModalidades.Where(w => w.Valor1.Equals("Key_Operacion_Permitido")).ToList()[0].Valor) : string.Empty;


                        AppSettings.Val_Operacion_Permitido = listParametrosModalidades
                       .Where(w => w.Valor1.Equals("Val_Operacion_Permitido")).ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosModalidades.Where(w => w.Valor1.Equals("Val_Operacion_Permitido")).ToList()[0].Valor) : string.Empty;

                        AppSettings.Key_CuotaSinCode_Permitido = listParametrosModalidades
                      .Where(w => w.Valor1.Equals("Key_CuotaSinCode_Permitido")).ToList().Count > 0 ?
                        Funciones.CheckStr(listParametrosModalidades.Where(w => w.Valor1.Equals("Key_CuotaSinCode_Permitido")).ToList()[0].Valor) : string.Empty;

                        //INICIATIVA 920 FIN
                    }
                }
                catch (Exception ex)
                {
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - Message  ==> ", Funciones.CheckStr(ex.Message)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA - 803][PARAN_GRUPO] - StackTrace  ==> ", Funciones.CheckStr(ex.StackTrace)), null, null);
                }                
                }

                catch (Exception ex)
                {
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715][PARAN_GRUPO] - Message  ==> ", Funciones.CheckStr(ex.Message)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715]][PARAN_GRUPO] - StackTrace  ==> ", Funciones.CheckStr(ex.StackTrace)), null, null);
                }
                //PROY-140715- FIN ANGEL

                try
                {
                    Int64 Key_ParanGrupoLineasAdicionales = Funciones.CheckInt64(ConfigurationManager.AppSettings["Key_ParanGrupoLineasAdicionales"].ToString());
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715][Key_ParanGrupoLineasAdicionales] => ", Funciones.CheckStr(Key_ParanGrupoLineasAdicionales)), null, null);
                    List<BEParametro> listParametrosLineasAdicionales = new BLGeneral().ListaParametrosGrupo(Key_ParanGrupoLineasAdicionales);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140715]", Funciones.CheckStr(listParametrosLineasAdicionales.Count())), null, null);

                    if (listParametrosLineasAdicionales != null && listParametrosLineasAdicionales.Count > 0)
                    {
                        ReadKeySettings.Key_FlagGeneralLineasAdicionales = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_FlagGeneralLineasAdicionales").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_FlagGeneralLineasAdicionales").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_ProductosPermitidosLineasAdic = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_ProductosPermitidosLineasAdic").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_ProductosPermitidosLineasAdic").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_CanalesPermitidosLineasAdic = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_CanalesPermitidosLineasAdic").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_CanalesPermitidosLineasAdic").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_DocumentosPermitidosLineasAdic = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_DocumentosPermitidosLineasAdic").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_DocumentosPermitidosLineasAdic").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_ModalidadVentaPermitidoLineasAdic = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_ModalidadVentaPermitidoLineasAdic").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_ModalidadVentaPermitidoLineasAdic").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_TipoOperacionPermitidoLineasAdic = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_TipoOperacionPermitidoLineasAdic").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_TipoOperacionPermitidoLineasAdic").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_TipoOfertaPermitidoLineasAdic = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_TipoOfertaPermitidoLineasAdic").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_TipoOfertaPermitidoLineasAdic").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_TipoRUCPermitidoLineasAdic = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_TipoRUCPermitidoLineasAdic").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_TipoRUCPermitidoLineasAdic").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjClienteSITieneLineasAdic = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_MsjClienteSITieneLineasAdic").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_MsjClienteSITieneLineasAdic").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjClienteNOTieneLineasAdic = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_MsjClienteNOTieneLineasAdic").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_MsjClienteNOTieneLineasAdic").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjClienteNuevo = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_MsjClienteNuevo").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_MsjClienteNuevo").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjClienteNoTieneLineasMayorCFMinimo = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_MsjClienteNoTieneLineasMayorCFMinimo").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_MsjClienteNoTieneLineasMayorCFMinimo").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjClienteTieneLineasMayorCFMinimo = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_MsjClienteTieneLineasMayorCFMinimo").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_MsjClienteTieneLineasMayorCFMinimo").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PlanesPermitidosASIS = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_PlanesPermitidosASIS").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_PlanesPermitidosASIS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PlanesPermitidosTOBE1 = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_PlanesPermitidosTOBE1").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_PlanesPermitidosTOBE1").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PlanesPermitidosTOBE2 = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_PlanesPermitidosTOBE2").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_PlanesPermitidosTOBE2").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PlanesPermitidosTOBE3 = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_PlanesPermitidosTOBE3").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_PlanesPermitidosTOBE3").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PlanesPermitidosTOBE4 = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_PlanesPermitidosTOBE4").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_PlanesPermitidosTOBE4").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PlanesPermitidosTOBE5 = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_PlanesPermitidosTOBE5").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_PlanesPermitidosTOBE5").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_CargoFijoMinimo = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_CargoFijoMinimo").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_CargoFijoMinimo").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_FlagGeneralCampanasDcto = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_FlagGeneralCampanasDcto").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_FlagGeneralCampanasDcto").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_FlagOcultarCampanasRegulares = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_FlagOcultarCampanasRegulares").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_FlagOcultarCampanasRegulares").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_CampanasDscto = listParametrosLineasAdicionales
                         .Where(w => w.Valor1 == "Key_CampanasDscto").ToList().Count > 0 ?
                         Funciones.CheckStr(listParametrosLineasAdicionales.Where(w => w.Valor1 == "Key_CampanasDscto").ToList()[0].Valor) : string.Empty;

                    }

                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_FlagGeneralLineasAdicionales ==> ", Funciones.CheckStr(ReadKeySettings.Key_FlagGeneralLineasAdicionales)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_ProductosPermitidosLineasAdic ==> ", Funciones.CheckStr(ReadKeySettings.Key_ProductosPermitidosLineasAdic)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_CanalesPermitidosLineasAdic ==> ", Funciones.CheckStr(ReadKeySettings.Key_CanalesPermitidosLineasAdic)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_DocumentosPermitidosLineasAdic ==> ", Funciones.CheckStr(ReadKeySettings.Key_DocumentosPermitidosLineasAdic)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_ModalidadVentaPermitidoLineasAdic ==> ", Funciones.CheckStr(ReadKeySettings.Key_ModalidadVentaPermitidoLineasAdic)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_TipoOperacionPermitidoLineasAdic ==> ", Funciones.CheckStr(ReadKeySettings.Key_TipoOperacionPermitidoLineasAdic)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_TipoOfertaPermitidoLineasAdic ==> ", Funciones.CheckStr(ReadKeySettings.Key_TipoOfertaPermitidoLineasAdic)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_TipoRUCPermitidoLineasAdic ==> ", Funciones.CheckStr(ReadKeySettings.Key_TipoRUCPermitidoLineasAdic)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_MsjClienteSITieneLineasAdic ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjClienteSITieneLineasAdic)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_MsjClienteNOTieneLineasAdic ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjClienteNOTieneLineasAdic)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_MsjClienteNuevo ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjClienteNuevo)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_MsjClienteNoTieneLineasMayorCFMinimo ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjClienteNoTieneLineasMayorCFMinimo)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_MsjClienteTieneLineasMayorCFMinimo ==> ", Funciones.CheckStr(ReadKeySettings.Key_MsjClienteTieneLineasMayorCFMinimo)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_PlanesPermitidosASIS ==> ", Funciones.CheckStr(ReadKeySettings.Key_PlanesPermitidosASIS)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_PlanesPermitidosTOBE1 ==> ", Funciones.CheckStr(ReadKeySettings.Key_PlanesPermitidosTOBE1)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_PlanesPermitidosTOBE2 ==> ", Funciones.CheckStr(ReadKeySettings.Key_PlanesPermitidosTOBE1)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_PlanesPermitidosTOBE3 ==> ", Funciones.CheckStr(ReadKeySettings.Key_PlanesPermitidosTOBE1)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_PlanesPermitidosTOBE4 ==> ", Funciones.CheckStr(ReadKeySettings.Key_PlanesPermitidosTOBE1)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_PlanesPermitidosTOBE5 ==> ", Funciones.CheckStr(ReadKeySettings.Key_PlanesPermitidosTOBE1)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_CargoFijoMinimo ==> ", Funciones.CheckStr(ReadKeySettings.Key_CargoFijoMinimo)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_FlagGeneralCampanasDcto ==> ", Funciones.CheckStr(ReadKeySettings.Key_FlagGeneralCampanasDcto)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_FlagOcultarCampanasRegulares ==> ", Funciones.CheckStr(ReadKeySettings.Key_FlagOcultarCampanasRegulares)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Key_CampanasDscto ==> ", Funciones.CheckStr(ReadKeySettings.Key_CampanasDscto)), null, null);

                }

                catch (Exception ex)
                {
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888][PARAN_GRUPO] - Message  ==> ", Funciones.CheckStr(ex.Message)), null, null);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[INICIATIVA-888]][PARAN_GRUPO] - StackTrace  ==> ", Funciones.CheckStr(ex.StackTrace)), null, null);
                }

                #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Consulta parametricas]
                try
                {
                    Int64 ConsParamCodVentaCuotas = Funciones.CheckInt64(ConfigurationManager.AppSettings["key_ParanGrupoVtaCuotas"]);
                    objLog.CrearArchivolog(string.Format("{0}{1}", "[PROY-140546][PARAN_GRUPO]", Funciones.CheckStr(ConsParamCodVentaCuotas)), null, null);
                    List<BEParametro> ListParamCodVentaCuotas = new BLGeneral().ListaParametrosGrupo(ConsParamCodVentaCuotas);

                    if (ListParamCodVentaCuotas != null && ListParamCodVentaCuotas.Count > 0)
                    {
                        ReadKeySettings.Key_OpcionSinPromocion = ListParamCodVentaCuotas
                         .Where(w => w.Valor1 == "Key_OpcionSinPromocion").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_OpcionSinPromocion").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_TipoMatVentaVarias = ListParamCodVentaCuotas
                         .Where(w => w.Valor1 == "Key_TipoMatVentaVarias").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_TipoMatVentaVarias").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_FlagGeneralVtaCuotas = ListParamCodVentaCuotas
                         .Where(w => w.Valor1 == "Key_FlagGeneralVtaCuotas").ToList().Count > 0 ?
                         Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_FlagGeneralVtaCuotas").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_RangoHoras = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_RangoHoras").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_RangoHoras").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjAccNoCuotas = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_MsjAccNoCuotas").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_MsjAccNoCuotas").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjClienteNoAplicaCuotas = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_MsjClienteNoAplicaCuotas").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_MsjClienteNoAplicaCuotas").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjNoAplicaCuotas = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_MsjNoAplicaCuotas").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_MsjNoAplicaCuotas").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_OpcionLineaCuenta = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_OpcionLineaCuenta").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_OpcionLineaCuenta").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_DesVentaVarias = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_DesVentaVarias").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_DesVentaVarias").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjSelecAcc = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_MsjSelecAcc").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_MsjSelecAcc").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjSelecLineaCuenta = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_MsjSelecLineaCuenta").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_MsjSelecLineaCuenta").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_MsjNoVariablesBRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_MsjNoVariablesBRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_MsjNoVariablesBRMS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_CodActivacionBSCS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_CodActivacionBSCS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_CodActivacionBSCS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_CodOperacionSOT = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_CodOperacionSOT").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_CodOperacionSOT").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_IpWhitelistDatosSOT = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_IpWhitelistDatosSOT").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_IpWhitelistDatosSOT").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_OperationServiceHeader = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_OperationServiceHeader").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_OperationServiceHeader").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_DesEstadoSOT = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_DesEstadoSOT").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_DesEstadoSOT").ToList()[0].Valor) : string.Empty;
                        
                        ReadKeySettings.Key_MsjVntaCuotasBRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_MsjVntaCuotasBRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_MsjVntaCuotasBRMS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.KEY_CONSTDNIBRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "KEY_CONSTDNIBRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "KEY_CONSTDNIBRMS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.KEY_CONSTCEBRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "KEY_CONSTCEBRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "KEY_CONSTCEBRMS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.KEY_CONSTRUC10BRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "KEY_CONSTRUC10BRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "KEY_CONSTRUC10BRMS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.KEY_CONSTRUC20BRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "KEY_CONSTRUC20BRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "KEY_CONSTRUC20BRMS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.KEY_CONSTPASSBRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "KEY_CONSTPASSBRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "KEY_CONSTPASSBRMS").ToList()[0].Valor) : string.Empty; 

                        ReadKeySettings.KEY_CONSTCIREBRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "KEY_CONSTCIREBRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "KEY_CONSTCIREBRMS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.KEY_CONSTCIEBRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "KEY_CONSTCIEBRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "KEY_CONSTCIEBRMS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.KEY_CONSTCPPBRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "KEY_CONSTCPPBRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "KEY_CONSTCPPBRMS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.KEY_CONSTCTMBRMS = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "KEY_CONSTCTMBRMS").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "KEY_CONSTCTMBRMS").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_PlazoAccCuota = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_PlazoAccCuota").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_PlazoAccCuota").ToList()[0].Valor) : string.Empty;

                        ReadKeySettings.Key_vtaCuo_tipoOpeBRMSCamp = ListParamCodVentaCuotas
                        .Where(w => w.Valor1 == "Key_vtaCuo_tipoOpeBRMSCamp").ToList().Count > 0 ?
                        Funciones.CheckStr(ListParamCodVentaCuotas.Where(w => w.Valor1 == "Key_vtaCuo_tipoOpeBRMSCamp").ToList()[0].Valor) : string.Empty;
                    }

                }
                catch (Exception ex)
                {
                    objLog.CrearArchivolog(string.Format("{0} ==> {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] Message", ex.Message), null, null);
                    objLog.CrearArchivolog(string.Format("{0} ==> {1}", "[PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] StackTrace", ex.StackTrace), null, null);
                }
                #endregion
            }
            catch (Exception ex)
            {
                objLog.CrearArchivolog(string.Format("{0}{1}", "Error en carga de AppSettings: ", ex.Message), null, null); //PROY-30166-IDEA–38863
            }
        }
        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

        //PROY-29121 INICIO
        private static void CargarParametrosFlexibilizacion()
        {
            try
            {
                GeneradorLog objLog = new GeneradorLog("WebComunes", null, null, "CargarParametrosFlexibilizacion");
                objLog.CrearArchivolog("Inicio_Cargar_ParametrosFlexibilizacion", null, null);
                Int64 codigoParametroNuevaRegla = Funciones.CheckInt64(ConfigurationManager.AppSettings["consParamgrupoNuevasReglas"].ToString());
                List<BEParametro> listParametros = new BLGeneral().ListaParametrosGrupo(codigoParametroNuevaRegla);

                objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-29121][ParametrosFlexibilizacion][Cantidad]", Funciones.CheckStr(listParametros.Count())), null, null);
                
                if (listParametros != null && listParametros.Count() > 0)
                {
                    AppSettings.consAntiguedadDeuda = listParametros
                         .Where(w => w.Valor1.Equals("consAntiguedadDeuda")).ToList().Count > 0 ?
                         Funciones.CheckInt(listParametros.Where(w => w.Valor1.Equals("consAntiguedadDeuda")).ToList()[0].Valor) : 0;
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-29121][consAntiguedadDeuda]", Funciones.CheckStr(AppSettings.consAntiguedadDeuda)), null, null);

                    AppSettings.consFlagFlexibilidad = listParametros
                         .Where(w => w.Valor1.Equals("consFlagFlexibilidad")).ToList().Count > 0 ?
                         Funciones.CheckStr(listParametros.Where(w => w.Valor1.Equals("consFlagFlexibilidad")).ToList()[0].Valor) : "";
                    objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-29121][consFlagFlexibilidad", Funciones.CheckStr(AppSettings.consFlagFlexibilidad)), null, null);
                }
                objLog.CrearArchivolog("Fin_Cargar_ParametrosFlexibilizacion", null, null);
            }
            catch 
            {
                
            }
          
        }
        //PROY-29121 FIN
        //PROY-32438 INI
        public static string getTipoContribuyente(string codigo, string tipo = "C")//C=>TIPOCONTRIBUYENTE E:ESTADO//D:CONDICION//I:CIU
        {
            
            string strTipoContribuyente = "";
            List<BEParametro> lstTipoCon = new List<BEParametro>();
            Int64 codTipoContrib = 0;
            GeneradorLog objLog = new GeneradorLog("    DASolicitud  ", null, null, "DATA_LOG"); 
            switch (tipo)
            {

                case "C":
                    codTipoContrib = Funciones.CheckInt64(ConfigurationManager.AppSettings["CodBuroBRMSTipoCont"].ToString());
                    break;
                case "E":
                    codTipoContrib = Funciones.CheckInt64(ConfigurationManager.AppSettings["CodBuroBRMSEstCont"].ToString());
                    break;
                case "D":
                    codTipoContrib = Funciones.CheckInt64(ConfigurationManager.AppSettings["CodBuroBRMSCondDomic"].ToString());
                    break;
                case "I":
                    codTipoContrib = Funciones.CheckInt64(ConfigurationManager.AppSettings["CodBuroBRMSCIU"].ToString());
                    break;
            
    }
            
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-32438][codTipoContrib]", Funciones.CheckStr(codTipoContrib.ToString())), null, null);


            List<BEParametro> listParametroBRMS = new BLGeneral().ListaParametrosGrupo(codTipoContrib);
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-32438][listParametroBRMS]", Funciones.CheckStr(listParametroBRMS.Count().ToString())), null, null);
            if (listParametroBRMS != null && listParametroBRMS.Count() > 0)
            {
                lstTipoCon = listParametroBRMS.Where(w => w.Valor.Equals(codigo)).ToList();
            }
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-32438][lstTipoCon]", Funciones.CheckStr(lstTipoCon.Count().ToString())), null, null);

            if (lstTipoCon.Count() > 0)
            {
                strTipoContribuyente = lstTipoCon[0].Valor1;
            }
            objLog.CrearArchivolog(string.Format("{0}-->{1}", "[PROY-32438][strTipoContribuyente]", Funciones.CheckStr(strTipoContribuyente.ToString())), null, null);
            objLog.CrearArchivolog(" === FIN PROY-32438 ====", null, null);
            return strTipoContribuyente;


        }
        //PROY-32438 FIN
            
        //PROY-31948 INI
        public static void ConsultarCuotasPendientes(string strTipoDocumento, string strNroDocumento, string strNroLinea, ref BECuota objCuotaOAC, ref BECuota objCuotaPVU)
        {

            GeneradorLog objLog = new GeneradorLog(Base.Sisact_Webbase.CurrentUsers, "ConsultarCuotasPendientes", null, "WEB");
            objLog.CrearArchivolog("[PROY-31948 - INICIO ConsultarCuotasPendientes]", null, null);

            string strCodRespuestaOAC = string.Empty;
            string strMsjRespuestaOAC = string.Empty;
            string strCodRespuestaPVUDB = string.Empty;
            string strMsjRespuestaPVUDB = string.Empty;

            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - strTipoDocumento]", strTipoDocumento), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - strNroDocumento]", strNroDocumento), null, null);

            objLog.CrearArchivolog("[PROY-31948 - INICIO ConsultaCuotasPendientesOAC]", null, null);

            objCuotaOAC = (new BWConsultaCuotaCliente()).consultarCuotaCliente(strTipoDocumento, strNroDocumento, strNroLinea, ref strCodRespuestaOAC, ref strMsjRespuestaOAC);

            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotasSistema]", objCuotaOAC.montoPendienteCuotasSistema), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadPlanesCuotasPendientesSistema]", objCuotaOAC.cantidadPlanesCuotasPendientesSistema), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadMaximaCuotasPendientesSistema]", objCuotaOAC.cantidadMaximaCuotasPendientesSistema), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadCuotasPendientes]", objCuotaOAC.cantidadCuotasPendientes), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotas]", objCuotaOAC.montoPendienteCuotas), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - strCodRespuestaOAC]", strCodRespuestaOAC), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - strMsjRespuestaOAC]", strMsjRespuestaOAC), null, null);
            objLog.CrearArchivolog("[PROY-31948 - FIN consultarCuotaClienteOAC]", null, null);

            objLog.CrearArchivolog("[PROY-31948 - INICIO ConsultaCuotasPendientesPVU]", null, null);

            objCuotaPVU = BLSolicitud.ConsultaCuotasPendientesPVU(strTipoDocumento, strNroDocumento, ref strCodRespuestaPVUDB, ref strMsjRespuestaPVUDB);

            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - montoPendienteCuotasUltimasVentas]", objCuotaPVU.montoPendienteCuotasUltimasVentas), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadPlanesCuotasPendientesUltimasVentas]", objCuotaPVU.cantidadPlanesCuotasPendientesUltimasVentas), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - cantidadMaximaCuotasPendientesUltimasVentas]", objCuotaPVU.cantidadMaximaCuotasPendientesUltimasVentas), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - strCodRespuestaPVUDB]", strCodRespuestaPVUDB), null, null);
            objLog.CrearArchivolog(String.Format("{0} : {1}", "[PROY-31948 - strMsjRespuestaPVUDB]", strMsjRespuestaPVUDB), null, null);
            objLog.CrearArchivolog("[PROY-31948 - FIN ConsultaCuotasPendientesPVU]", null, null);

            HttpContext.Current.Session["objCuotaOAC"] = objCuotaOAC;
            HttpContext.Current.Session["objCuotaPVU"] = objCuotaPVU;

            objLog.CrearArchivolog("[PROY-31948 - FIN ConsultarCuotasPendientes]", null, null);
        }
        //PROY-31948 FIN

        #region INICIATIVA-803 -IDEA 142474  Tienda Virtual - Adecuación SISACT
        public static BEAuditoriaRequest obtenerAuditoriaDataPower(string TimeOut, string usuario)
        {
            BEAuditoriaRequest objBeAuditoriaRequest = new BEAuditoriaRequest();

            objBeAuditoriaRequest.idTransaccion = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            objBeAuditoriaRequest.timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            objBeAuditoriaRequest.userId = Funciones.CheckStr(usuario); ;
            objBeAuditoriaRequest.msgid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            objBeAuditoriaRequest.dataPower = true;
            objBeAuditoriaRequest.accept = "application/json";
            objBeAuditoriaRequest.urlTimeOut_Rest = Funciones.CheckStr(ConfigurationManager.AppSettings[TimeOut]);
            
            GeneradorLog.EscribirLog(string.Format("INICIATIVA-803 IDEA-142474 | objBeAuditoriaRequest.idTransaccion : {0} ", objBeAuditoriaRequest.idTransaccion), null, null);
            GeneradorLog.EscribirLog(string.Format("INICIATIVA-803 IDEA-142474 | objBeAuditoriaRequest.timestamp : {0} ", objBeAuditoriaRequest.timestamp), null, null);
            GeneradorLog.EscribirLog(string.Format("INICIATIVA-803 IDEA-142474 | objBeAuditoriaRequest.userId : {0} ", objBeAuditoriaRequest.userId), null, null);
            GeneradorLog.EscribirLog(string.Format("INICIATIVA-803 IDEA-142474 | objBeAuditoriaRequest.msgid : {0} ", objBeAuditoriaRequest.msgid), null, null);
            GeneradorLog.EscribirLog(string.Format("INICIATIVA-803 IDEA-142474 | objBeAuditoriaRequest.dataPower : {0} ", Funciones.CheckStr(objBeAuditoriaRequest.dataPower)), null, null);
            GeneradorLog.EscribirLog(string.Format("INICIATIVA-803 IDEA-142474 | objBeAuditoriaRequest.urlTimeOut_Rest : {0} ", Funciones.CheckStr(objBeAuditoriaRequest.urlTimeOut_Rest)), null, null);

            //consulta claves
            objBeAuditoriaRequest.wsIp = Funciones.CheckStr(ConfigurationManager.AppSettings["conswsipPedidosTienda"]);
            objBeAuditoriaRequest.ipTransaccion = Funciones.CheckStr(HttpContext.Current.Session["CurrentTerminal"]);
            objBeAuditoriaRequest.usuarioAplicacion = Funciones.CheckStr(usuario);
            objBeAuditoriaRequest.ipApplication = Claro.SISACT.Web.Base.Sisact_Webbase.CurrentServer;

            GeneradorLog.EscribirLog(string.Format("INICIATIVA-803 IDEA-142474   | objBeAuditoriaRequest.wsIp  : {0} ", objBeAuditoriaRequest.wsIp), null, null);
            GeneradorLog.EscribirLog(string.Format("INICIATIVA-803 IDEA-142474   | objBeAuditoriaRequest.ipTransaccion : {0} ", Funciones.CheckStr(objBeAuditoriaRequest.ipTransaccion)), null, null);
            GeneradorLog.EscribirLog(string.Format("INICIATIVA-803 IDEA-142474   | objBeAuditoriaRequest.usuarioAplicacion : {0} ", Funciones.CheckStr(objBeAuditoriaRequest.usuarioAplicacion)), null, null);
            return objBeAuditoriaRequest;
        }
        #endregion
        
        //PROY-140657 INI
        public static Claro.SISACT.Entity.DataPowerRest.HeaderRequest GenerarHeader(string nroDoc, string CurrentUsers)
        {
            GeneradorLog _objLog = new GeneradorLog(CurrentUsers, nroDoc, null, "WEB");
            _objLog.CrearArchivolog("[WebComunes][GenerarHeader][INICIO]", null, null);

            Claro.SISACT.Entity.DataPowerRest.HeaderRequest objHeaderRequest = new Claro.SISACT.Entity.DataPowerRest.HeaderRequest();

            #region Header
            objHeaderRequest.consumer = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidos_consumer"]);
            objHeaderRequest.country = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidos_country"]);
            objHeaderRequest.dispositivo = Claro.SISACT.Web.Base.Sisact_Webbase.CurrentTerminal;
            objHeaderRequest.language = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidos_language"]);
            objHeaderRequest.modulo = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidos_modulo"]);
            objHeaderRequest.msgType = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidos_msgtype"]);
            objHeaderRequest.operation = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidos_operation"]);
            objHeaderRequest.pid = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            objHeaderRequest.system = Funciones.CheckStr(ConfigurationManager.AppSettings["AfiliacionDEAUAsistidos_system"]);
            objHeaderRequest.timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            objHeaderRequest.userId = Claro.SISACT.Web.Base.Sisact_Webbase.CurrentUsers;
            objHeaderRequest.wsIp = ConfigurationManager.AppSettings["AfiliacionDEAUAsistidosWS_wsip"];

            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.consumer]", Funciones.CheckStr(objHeaderRequest.consumer)), null, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.country]", Funciones.CheckStr(objHeaderRequest.country)), null, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.dispositivo]", Funciones.CheckStr(objHeaderRequest.dispositivo)), null, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.modulo]", Funciones.CheckStr(objHeaderRequest.modulo)), null, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.msgType]", Funciones.CheckStr(objHeaderRequest.msgType)), null, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.operation]", Funciones.CheckStr(objHeaderRequest.operation)), null, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.pid]", Funciones.CheckStr(objHeaderRequest.pid)), null, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.system]", Funciones.CheckStr(objHeaderRequest.system)), null, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.timestamp]", Funciones.CheckStr(objHeaderRequest.timestamp)), null, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.userId]", Funciones.CheckStr(objHeaderRequest.userId)), null, null);
            _objLog.CrearArchivolog(String.Format("{0} --> {1}", "[WebComunes][GenerarHeader][objHeaderRequest.wsIp]", Funciones.CheckStr(objHeaderRequest.wsIp)), null, null);

            #endregion
            return objHeaderRequest;
        }
        //PROY-140657 FIN
    }
}
