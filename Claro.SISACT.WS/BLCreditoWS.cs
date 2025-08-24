using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Claro.SISACT.Business;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;

namespace Claro.SISACT.Business
{
    public class BLCreditoWS
    {
        BEUsuarioSession objSession;
        GeneradorLog _objLog = null;

        //PROY-24740
        public BEDataCreditoOUT ConsultarDatos(BEDataCreditoIN objDataCreditoIN, BEUsuarioSession objUsuario, ref BEItemMensaje objMensaje)
        {
            string tipoSEC = string.Empty;
            string tipoActivacion = string.Empty;
            string tipoCliente_Venta = string.Empty;
            string formaPago = string.Empty;
            string tipoVenta = string.Empty;
            string plazoAcuerdo = string.Empty;
            string flagEssalud = string.Empty;
            string flagSunat = string.Empty;
            string nroDocumento = objDataCreditoIN.istrNumeroDocumento;

            bool blnConsultaDC = false;
            objMensaje = new BEItemMensaje();

            objSession = objUsuario;
            BEDataCreditoOUT objDataCreditoOUT = null;
            BLDataCredito objDataCreditoNegocio= new  BLDataCredito();

            int intNumeroIntentos = Convert.ToInt32(ConfigurationManager.AppSettings["numeroIntentosBuro"]);
            for (int i = 0; i < intNumeroIntentos; i++)
            {
                try
                {
                    // Consulta WS Buro Crediticio
                    WS.BWCreditoWS obj = new WS.BWCreditoWS();
                    objDataCreditoOUT = obj.EvaluarCredito(objDataCreditoIN, objUsuario, ref objMensaje);
                }
                catch
                {
                }

                if (objDataCreditoOUT != null)
                    break;
            }

                try
                {
                    if (objDataCreditoOUT == null || !objMensaje.exito)
                    {
                        objMensaje.mensajeSistema = ConfigurationManager.AppSettings["consMsjConsultarDC"].ToString();
                        throw new Exception(objMensaje.mensajeSistema);
                    }

                if (objDataCreditoOUT.RESPUESTA == ConfigurationManager.AppSettings["strConstanteTipo1"].ToString() || objDataCreditoOUT.RESPUESTA == "14")
                {
                    if (!string.IsNullOrEmpty(objDataCreditoOUT.RAZONES))
                    {
                        string strRegla = objDataCreditoOUT.RAZONES.Substring(0, 1);
                        if (strRegla.Equals("Z") || strRegla.Equals("R") || strRegla.Equals("X") || strRegla.Equals("Q") || strRegla.Equals("V"))
                        {
                            // Grabar Datos Repositorio
                            //objMensaje = objDataCreditoNegocio.GrabarDatosDCPersona(objDataCreditoIN, objDataCreditoOUT);
                            blnConsultaDC = true;

                            //string param = "Almacenamiento exitoso de Inf. de DC. (";
                            //param += objDataCreditoOUT.NUMEROOPERACION + ";" + objDataCreditoIN.istrTipoDocumento + ";" + objDataCreditoIN.istrNumeroDocumento + ";";
                            //param += objDataCreditoIN.istrPuntoVenta + ";" + tipoSEC + ";" + ""; // iParametro;
                            //param += ";" + objDataCreditoIN.istrEstadoCivil + ";" + objDataCreditoOUT.RAZONES + ";";
                            //param += objDataCreditoOUT.ANALISIS + ";" + objDataCreditoOUT.SCORE_RANKIN_OPERATIVO + ";" + objDataCreditoOUT.NUMERO_ENTIDADES_RANKIN_OPERATIVO + ";";
                            //param += objDataCreditoOUT.PUNTAJE + ";" + objDataCreditoOUT.LIMITECREDITODATACREDITO + ";" + objDataCreditoOUT.LIMITECREDITOBASEEXTERNA + ";" + objDataCreditoOUT.LIMITECREDITOCLARO + ";";
                            //param += objDataCreditoOUT.FECHANACIMIENTO + objDataCreditoOUT.CODIGOBURO + ")";

                            //Auditoria(ConfigurationManager.AppSettings["DcAuditGuardarDCMasivo"].ToString(), param);
                        }
                        else
                        {
                            objMensaje.mensajeSistema = ConfigurationManager.AppSettings["consMsjConsultarDCData"].ToString();
                            throw new Exception(objMensaje.mensajeSistema);
                        }
                    }
                    else
                    {
                        objMensaje.mensajeSistema = ConfigurationManager.AppSettings["consMsjConsultarDCRazones"].ToString();
                        throw new Exception(objMensaje.mensajeSistema);
                    }
                }

                // Validaciones DataCredito
                if (string.IsNullOrEmpty(objDataCreditoOUT.NUMEROOPERACION))
                {
                    objMensaje.mensajeSistema = ConfigurationManager.AppSettings["consMsjConsultarDCNroOper"].ToString();
                    throw new Exception(objMensaje.mensajeSistema);
                }

                if (objDataCreditoOUT.SCORE > 99) objDataCreditoOUT.SCORE = 99;
                if (objDataCreditoOUT.RESPUESTA == "14") objDataCreditoOUT.RESPUESTA = "13";

                if (string.IsNullOrEmpty(objDataCreditoOUT.APELLIDO_MATERNO))
                    objDataCreditoOUT.APELLIDO_MATERNO = ConfigurationManager.AppSettings["CONS_SIN_APMATERNO_DEFAULT"].ToString();

                string strMensaje = string.Empty;
                if (!(objDataCreditoOUT.RESPUESTA == ConfigurationManager.AppSettings["strConstanteTipo1"].ToString() ||
                      objDataCreditoOUT.RESPUESTA == ConfigurationManager.AppSettings["strConstanteTipo6"].ToString() ||
                      objDataCreditoOUT.RESPUESTA == ConfigurationManager.AppSettings["strConstanteTipo7"].ToString()))
                {
                    List<BEItemGenerico> objLista = objDataCreditoNegocio.ListarRespuestaDC();
                    foreach (BEItemGenerico obj1 in objLista)
                    {
                        if (objDataCreditoOUT.RESPUESTA == obj1.Codigo)
                        {
                            strMensaje = obj1.Descripcion;
                            break;
                        }
                    }
                    //string mensajeMail = Body_Correo_DC(strMensaje, objDataCreditoIN, objDataCreditoOUT);
                    //Funciones.EnviarCorreo(ConfigurationManager.AppSettings["DcEmailRemitente"].ToString(), "Informe Consulta DataCredito", mensajeMail,
                    //                       ConfigurationManager.AppSettings["DcEmailDestinatarioPRD"].ToString());

                    objMensaje.mensajeSistema = strMensaje;
                    throw new Exception(objMensaje.mensajeSistema);
                }

                //if (blnConsultaDC)
                //{
                //    BEDataCreditoINOUT objINOUT = new BEDataCreditoINOUT();
                //    string paramIN = objDataCreditoIN.toString();
                //    string paramOUT = objDataCreditoOUT.toString();

                //    objINOUT.IODCV_NUM_OPERACION = Funciones.CheckStr(objDataCreditoOUT.NUMEROOPERACION);
                //    objINOUT.IODCV_INPUT_VALORES = Funciones.CheckStr(paramIN);
                //    objINOUT.IODCV_OUTPUT_VALORES = Funciones.CheckStr(paramOUT);
                //    objINOUT.IODCV_TIPO_DOCUMENTO = Funciones.CheckStr(objDataCreditoIN.istrTipoDocumento);
                //    objINOUT.IODCV_NUM_DOCUMENTO = Funciones.CheckStr(objDataCreditoIN.istrNumeroDocumento);
                //    objINOUT.IODCV_USUARIO_REGISTRO = Funciones.CheckStr(objDataCreditoIN.istrUsuarioACC);
                //    objINOUT.IODCV_COD_PUNTO_VENTA = Funciones.CheckStr(objDataCreditoIN.istrPuntoVenta);
                //    objINOUT.IODCC_FORMA_PAGO = Funciones.CheckStr(formaPago);
                //    objINOUT.IODCC_TIPO_ACTIVACION = Funciones.CheckStr(tipoActivacion);
                //    objINOUT.IODCC_TIPO_CLIENTE = Funciones.CheckStr(tipoCliente_Venta);
                //    objINOUT.IODCC_TIPO_VENTA = Funciones.CheckStr(tipoVenta);
                //    objINOUT.IODCC_PLAZO_ACUERDO = Funciones.CheckStr(plazoAcuerdo);
                //    objINOUT.IODCC_PLAN1 = "";
                //    objINOUT.IODCC_PLAN2 = "";
                //    objINOUT.IODCC_PLAN3 = "";
                //    objINOUT.IODCC_CONTROL_CONSUMO = "";
                //    objINOUT.IODCC_FLAG_ESSALUD = Funciones.CheckStr(flagEssalud);
                //    objINOUT.IODCC_FLAG_SUNAT = Funciones.CheckStr(flagSunat);
                //    objINOUT.IODCC_RIESGO = Funciones.CheckStr(objDataCreditoOUT.ACCION);
                //    objINOUT.IODCC_LIMITE_CREDITO = Funciones.CheckStr(objDataCreditoOUT.INGRESO_O_LC);
                //    objINOUT.IODCC_SCORE_TEXTO = Funciones.CheckStr(objDataCreditoOUT.CREDIT_SCORE);
                //    objINOUT.IODCC_SCORE_NUMERO = Funciones.CheckStr(objDataCreditoOUT.SCORE);
                //    objINOUT.IODCC_RESPUESTA_DC = Funciones.CheckStr(objDataCreditoOUT.RESPUESTA);
                //    objINOUT.IODCV_APE_PATERNO = Funciones.CheckStr(objDataCreditoOUT.APELLIDO_PATERNO);
                //    objINOUT.IODCV_APE_MATERNO = Funciones.CheckStr(objDataCreditoOUT.APELLIDO_MATERNO);
                //    objINOUT.IODCV_NOMBRES = Funciones.CheckStr(objDataCreditoOUT.NOMBRES);
                //    objINOUT.IODCV_UBIGEO = Funciones.CheckStr(objDataCreditoIN.istrRegion).Substring(1, 4);
                //    objINOUT.IODCC_TIPO_CLIENTE_DC = Funciones.CheckStr(objDataCreditoIN.istrArea);
                //    objINOUT.IODCC_ESTADO_CIVIL_DC = Funciones.CheckStr(objDataCreditoIN.istrEstadoCivil);
                //    objINOUT.IODCC_ORIGEN_LC_DC = Funciones.CheckStr(objDataCreditoOUT.TOP_10000);
                //    objINOUT.IODCC_ANALISIS_DC = Funciones.CheckStr(objDataCreditoOUT.ANALISIS);
                //    objINOUT.IODCC_SCORE_RANKING_OPER_DC = Funciones.CheckStr(objDataCreditoOUT.SCORE_RANKIN_OPERATIVO);

                //    objINOUT.IODCN_PUNTAJE_DC = (int)Funciones.CheckDbl(objDataCreditoOUT.PUNTAJE);

                //    objINOUT.IODCN_LC_DATA_CREDITO_DC = Funciones.CheckDbl(objDataCreditoOUT.LIMITECREDITODATACREDITO);
                //    objINOUT.IODCN_LC_BASE_EXTERNA_DC = Funciones.CheckDbl(objDataCreditoOUT.LIMITECREDITOBASEEXTERNA);
                //    objINOUT.IODCN_LC_CLARO_DC = Funciones.CheckDbl(objDataCreditoOUT.LIMITECREDITOCLARO);
                //    objINOUT.IODCC_RAZONES_DC = Funciones.CheckStr(objDataCreditoOUT.RAZONES);
                //    objINOUT.IODCD_FECHA_NACE_CLIENTE_DC = Funciones.CheckStr(objDataCreditoOUT.FECHANACIMIENTO);

                //    // Grabar Datos INPUT/ OUTPUT DC
                //    objMensaje = objDataCreditoNegocio.GrabarDCInputOutput(objINOUT);

                //    BEDataCreditoHistorico vista = new BEDataCreditoHistorico();
                //    vista.HISTV_NUM_OPERACION = objDataCreditoOUT.NUMEROOPERACION;
                //    vista.HISTC_TIPO_DOCUMENTO = "";

                //    if (objDataCreditoIN.istrTipoDocumento.Length == 1)
                //    {
                //        vista.HISTC_TIPO_DOCUMENTO = "0" + objDataCreditoIN.istrTipoDocumento;
                //    }
                //    vista.HISTV_NUM_DOCUMENTO = objDataCreditoIN.istrNumeroDocumento;
                //    vista.HISTV_APELLIDO_PAT = objDataCreditoIN.istrAPELLIDOPATERNO.ToUpper();
                //    vista.HISTV_APELLIDO_MAT = objDataCreditoIN.istrAPELLIDOMATERNO.ToUpper();
                //    vista.HISTV_NOMBRE = objDataCreditoIN.istrNOMBRES.ToUpper();
                //    vista.HISTC_TIPO_RESPUESTA = objDataCreditoOUT.RESPUESTA;
                //    vista.HISTC_TIPO_RIESGO = objDataCreditoOUT.ACCION;

                //    if (string.IsNullOrEmpty(vista.HISTC_TIPO_RIESGO))
                //    {
                //        vista.HISTC_TIPO_RIESGO = "S";
                //    }
                //    vista.HISTN_CANT_INTENTOS = 1;
                //    vista.HISTV_OVEN_CODIGO = objDataCreditoIN.istrPuntoVenta;
                //    vista.HISTV_TERMINAL_ID = System.Net.Dns.GetHostName();
                //    vista.HISTN_USUARIO_REG = objDataCreditoIN.istrUsuarioACC;

                //    // Grabar Datos HISTORICO DC
                //    objDataCreditoNegocio.GrabarDCHistorico(vista);
                //}
            }
            catch(Exception ex)
            {
                objMensaje.exito = false;
                objMensaje.codigo = ex.Source;
                objMensaje.mensajeSistema = ex.Message;
                objDataCreditoOUT = null;

                _objLog = new GeneradorLog(null, objDataCreditoIN.istrNumeroDocumento, null, "WEB");
                _objLog.CrearArchivolog("ERROR CONSULTAR DATOS DC", null, ex);
            }
            return objDataCreditoOUT;
        }

        private string Body_Correo_DC(string mensajeRespuesta, BEDataCreditoIN objDataCreditoIN, BEDataCreditoOUT objDataCreditoOUT)
        {
            StringBuilder cuerpo = new StringBuilder();
            string flag = ConfigurationManager.AppSettings["FlagPrueba"].ToString();
            if (flag.Equals("0"))
                cuerpo.Append("AMBIENTE DE PRODUCCION - CORREO DE ALERTA - DATACREDITO");
            else
                cuerpo.Append("PRUEBAS EN DESARROLLO O EN QA - CORREO DE ALERTA - DATACREDITO");

            cuerpo.Append("<br><hr align=\'left\' style=\'height:1px;width:550px;border:1px solid #000;\'>");
            cuerpo.Append(("Consulta a DataCredito N° " + objDataCreditoOUT.NUMEROOPERACION));
            cuerpo.Append("<br><br><hr align=\'left\' style=\'height:1px;width:550px;border:1px solid #000;\'>");
            cuerpo.Append("Respuesta devuelta:");
            cuerpo.Append("<br><br>");
            cuerpo.Append(("  Mensaje : " + mensajeRespuesta));
            cuerpo.Append("<br>");
            cuerpo.Append(("  Codigo  : " + objDataCreditoOUT.RESPUESTA));
            cuerpo.Append("<br><hr align=\'left\' style=\'height:1px;width:550px;border:1px solid #000;\'>");
            cuerpo.Append("Datos del Cliente:");
            cuerpo.Append("<br><br>");
            cuerpo.Append(("  Tipo Documento   : " + objDataCreditoIN.istrTipoDocumento));
            cuerpo.Append("<br>");
            cuerpo.Append(("  Numero Documento : " + objDataCreditoIN.istrNumeroDocumento));
            cuerpo.Append("<br>");
            cuerpo.Append(("  Nombre           : " + objDataCreditoOUT.NOMBRES));
            cuerpo.Append("<br>");
            cuerpo.Append(("  Apellido Paterno : " + objDataCreditoOUT.APELLIDO_PATERNO));
            cuerpo.Append("<br>");
            cuerpo.Append(("  Apellido Materno : " + objDataCreditoOUT.APELLIDO_MATERNO));
            cuerpo.Append("<br><hr align=\'left\' style=\'height:1px;width:550px;border:1px solid #000;\'>");
            cuerpo.Append(("Código de Punto de Venta que realizó la consulta: " + objDataCreditoIN.istrPuntoVenta));
            return cuerpo.ToString();
        }

        //PROY-24740
        private bool Auditoria(String strCodTrx, String strDesTrx)
        {
            bool blnOK = false;
            try
            {
                String nombreHost = System.Net.Dns.GetHostName();
                String nombreServer = System.Net.Dns.GetHostName();
                String ipServer = System.Net.Dns.GetHostAddresses(nombreServer)[0].ToString();
                String usuarioId = objSession.idCuentaRed;
                String ipcliente = objSession.Terminal; 
                String strCodServ = ConfigurationManager.AppSettings["CONS_COD_SACT_SERV"];
                String vMonto = "0";

                blnOK = (new WS.BWAuditoria()).registrarAuditoria(strCodTrx, strCodServ, ipcliente, nombreHost, ipServer, nombreServer, usuarioId, "", vMonto, strDesTrx);
            }
            catch
            {
                blnOK = false;
            }
            return blnOK;
        }

        //INICIO: PROY-20054-IDEA-23849
        public BEEmpresaExperto ConsultarDatosDataCreditoCorporativo(BEDataCreditoCorpIN objDataCreditoIN, BEUsuarioSession objUsuario, ref BEItemMensaje objMensaje)
        {
            bool blnResultado = false;
            BEEmpresaExperto objExperto = (new BWCreditoWS()).EvaluarCreditoCorp(objDataCreditoIN, objUsuario, ref objMensaje);

            try
            {
                if (objExperto == null || objExperto.strCodRetorno == null)
                {
                    //INI PROY-140257
                    if (objExperto.strCodError == "RIESGO")
                    {                        
                        return null;
                    }
                    //FIN PROY-140257
                    objMensaje.mensajeCliente = "Error: Servicio DataCredito Corporativo no disponible. " + '\r' + objMensaje.mensajeCliente;

                    ////  Registrar Auditoria DataCredito
                    //mensajeAudi = "Consulta DataCredito Corp RESPUESTA NULA. (";
                    //mensajeAudi += objDataCreditoIN.istrTipoDocumento;
                    //mensajeAudi += objDataCreditoIN.istrNumeroDocumento;
                    //mensajeAudi += objDataCreditoIN.istrPuntoVenta;
                    //mensajeAudi += objDataCreditoIN.istrApellidoPaterno;
                    //mensajeAudi += objDataCreditoIN.istrApellidoMaterno;
                    //mensajeAudi += objDataCreditoIN.istrNombres;
                    //mensajeAudi += objDataCreditoIN.istrTipoSEC + ") ERROR: " + objMensaje.mensajeCliente;

                    //Auditoria(idTrx, mensajeAudi);
                    return null;
                }

                blnResultado = objExperto.strFlagInterno.Equals("0");

                if (objExperto.strFlagInterno.Equals("3")) blnResultado = true; //Excepcion Servicio 53 (dejar Pasar)

                if (!blnResultado)
                {
                    string mensaje = "";

                    if (objExperto.strCodError != "")
                    {
                        mensaje = objMensaje.mensajeCliente + " (CodigoRetorno: " + objExperto.strCodRetorno + "; CodigoError: " + objExperto.strCodError;
                        mensaje += "; Descripcion: " + objExperto.strMensajeError + ").";
                    }
                    else
                    {
                        mensaje = objMensaje.mensajeCliente + " (CodigoRetorno: " + objExperto.strCodRetorno + ").";
                    }

                    objMensaje.mensajeCliente = "Error: " + mensaje;

                    ////  Registrar Auditoria DataCredito
                    //mensajeAudi = "Consulta DataCredito Corp RESPUESTA CON ERROR. (";
                    //mensajeAudi += objExperto.strNroOperacion + ";";
                    //mensajeAudi += objDataCreditoIN.istrTipoDocumento + ";";
                    //mensajeAudi += objDataCreditoIN.istrNumeroDocumento + ";";
                    //mensajeAudi += objDataCreditoIN.istrPuntoVenta + ";";
                    //mensajeAudi += objExperto.strRiesgo + ";";
                    //mensajeAudi += objExperto.strCodError + ";";
                    //mensajeAudi += objExperto.strApellidoPaterno + ";";
                    //mensajeAudi += objExperto.strApellidoMaterno + ";";
                    //mensajeAudi += objExperto.strNombres + ";";
                    //mensajeAudi += objDataCreditoIN.istrTipoSEC + ")";
                    //Auditoria(idTrx, mensajeAudi);

                    ////  Enviar Correo
                    //if (!objExperto.strCodRetorno.Equals(ConfigurationManager.AppSettings["DcCorpCodigoRetornoOk"].ToString()))
                    //{
                    //    string body = Body_Correo_DC(objDataCreditoIN, objExperto, objMensaje.mensajeCliente);
                    //    Funciones.EnviarCorreo(ConfigurationManager.AppSettings["DcEmailRemitente"].ToString(),
                    //                            "Informe Consulta DataCredito Corporativo",
                    //                            body,
                    //                            ConfigurationManager.AppSettings["DcEmailDestinatarioPRD"].ToString());
                    //}
                    return null;
                }

                ////  Registrar Auditoria DataCredito
                //mensajeAudi = "Consulta DataCredito Corp RESPUESTA EXITOSA. (";
                //mensajeAudi += objExperto.strNroOperacion + ";";
                //mensajeAudi += objDataCreditoIN.istrTipoDocumento + ";";
                //mensajeAudi += objDataCreditoIN.istrNumeroDocumento + ";";
                //mensajeAudi += objDataCreditoIN.istrPuntoVenta + ";";
                //mensajeAudi += objExperto.strRiesgo + ";";
                //mensajeAudi += objExperto.strCodError + ";";
                //mensajeAudi += objExperto.strApellidoPaterno + ";";
                //mensajeAudi += objExperto.strApellidoMaterno + ";";
                //mensajeAudi += objExperto.strNombres + ";";
                //mensajeAudi += objDataCreditoIN.istrTipoSEC + ")";
                //idTrx = ConfigurationManager.AppSettings["DcCorpAuditConsultaDataCredito" + objDataCreditoIN.istrTipoSEC].ToString();
                //Auditoria(idTrx, mensajeAudi);

                if (objExperto.estado_ruc.Equals("B"))
                {
                    string msg = objMensaje.mensajeCliente;
                    List<BEParametro> objLista = (new BLGeneral()).ListaParametros(70);
                    if (objLista.Count > 0)
                    {
                        msg = ((BEParametro)objLista[0]).Valor;
                        objMensaje.mensajeCliente = msg;
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                objMensaje.exito = false;
                objMensaje.codigo = ex.Source;
                objMensaje.mensajeSistema = ex.Message;
                objExperto = null;

                _objLog = new GeneradorLog(null, objDataCreditoIN.istrNumeroDocumento, null, "WEB");
                _objLog.CrearArchivolog("ERROR CONSULTAR DATOS DC CORPORATIVO", null, ex);
            }

            return objExperto;
        }
        //FIN
    }
}
