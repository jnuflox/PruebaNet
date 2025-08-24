using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Business;
using Claro.SISACT.Common;

namespace Claro.SISACT.WS
{
    public class BLDataCreditoCorp
    {
        BEUsuarioSession objSession = null;

        public BEEmpresaExperto ConsultarDatosDataCredito(BEDataCreditoCorpIN objDataCreditoIN, BEUsuarioSession objUsuario, ref BEItemMensaje objMensaje)
        {
            string mensajeAudi = string.Empty;
            objSession = objUsuario;
            string idTrx = ConfigurationManager.AppSettings["DcCorpAuditConsultaDataCredito" + objDataCreditoIN.istrTipoSEC].ToString();

            bool blnResultado = false;
            BEEmpresaExperto objExperto = (new BWDataCreditoCorp()).ConsultaBuroCrediticio(objDataCreditoIN, objUsuario, ref objMensaje);

            if (objExperto == null || objExperto.strCodRetorno == null)
            {
                //INI PROY-140257
                if (objExperto.strCodError == "RIESGO")
                {
                    return null;
                }
                //FIN PROY-140257
                objMensaje.mensajeCliente = "Error: Servicio DataCredito Corporativo no disponible. " + '\r' + objMensaje.mensajeCliente;

                //  Registrar Auditoria DataCredito
                mensajeAudi = "Consulta DataCredito Corp RESPUESTA NULA. (";
                mensajeAudi += objDataCreditoIN.istrTipoDocumento;
                mensajeAudi += objDataCreditoIN.istrNumeroDocumento;
                mensajeAudi += objDataCreditoIN.istrPuntoVenta;
                mensajeAudi += objDataCreditoIN.istrApellidoPaterno;
                mensajeAudi += objDataCreditoIN.istrApellidoMaterno;
                mensajeAudi += objDataCreditoIN.istrNombres;
                mensajeAudi += objDataCreditoIN.istrTipoSEC + ") ERROR: " + objMensaje.mensajeCliente;

                Auditoria(idTrx, mensajeAudi);
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

                //  Registrar Auditoria DataCredito
                mensajeAudi = "Consulta DataCredito Corp RESPUESTA CON ERROR. (";
                mensajeAudi += objExperto.strNroOperacion + ";";
                mensajeAudi += objDataCreditoIN.istrTipoDocumento + ";";
                mensajeAudi += objDataCreditoIN.istrNumeroDocumento + ";";
                mensajeAudi += objDataCreditoIN.istrPuntoVenta + ";";
                mensajeAudi += objExperto.strRiesgo + ";";
                mensajeAudi += objExperto.strCodError + ";";
                mensajeAudi += objExperto.strApellidoPaterno + ";";
                mensajeAudi += objExperto.strApellidoMaterno + ";";
                mensajeAudi += objExperto.strNombres + ";";
                mensajeAudi += objDataCreditoIN.istrTipoSEC + ")";

                Auditoria(idTrx, mensajeAudi);

                //  Enviar Correo
                if (!objExperto.strCodRetorno.Equals(ConfigurationManager.AppSettings["DcCorpCodigoRetornoOk"].ToString()))
                {
                    string body = Body_Correo_DC(objDataCreditoIN, objExperto, objMensaje.mensajeCliente);
                    Funciones.EnviarCorreo(ConfigurationManager.AppSettings["DcEmailRemitente"].ToString(),
                                            "Informe Consulta DataCredito Corporativo",
                                            body,
                                            ConfigurationManager.AppSettings["DcEmailDestinatarioPRD"].ToString());
                }
                return null;
            }

            //  Registrar Auditoria DataCredito
            mensajeAudi = "Consulta DataCredito Corp RESPUESTA EXITOSA. (";
            mensajeAudi += objExperto.strNroOperacion + ";";
            mensajeAudi += objDataCreditoIN.istrTipoDocumento + ";";
            mensajeAudi += objDataCreditoIN.istrNumeroDocumento + ";";
            mensajeAudi += objDataCreditoIN.istrPuntoVenta + ";";
            mensajeAudi += objExperto.strRiesgo + ";";
            mensajeAudi += objExperto.strCodError + ";";
            mensajeAudi += objExperto.strApellidoPaterno + ";";
            mensajeAudi += objExperto.strApellidoMaterno + ";";
            mensajeAudi += objExperto.strNombres + ";";
            mensajeAudi += objDataCreditoIN.istrTipoSEC + ")";

            idTrx = ConfigurationManager.AppSettings["DcCorpAuditConsultaDataCredito" + objDataCreditoIN.istrTipoSEC].ToString();
            Auditoria(idTrx, mensajeAudi);

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

            return objExperto;
        }

        private string Body_Correo_DC(BEDataCreditoCorpIN oDataCreditoCorpIN, BEEmpresaExperto oDataCreditoCorpOUT, string mensajeError)
        {
            StringBuilder cuerpo = new StringBuilder();
            string flag = ConfigurationManager.AppSettings["FlagPrueba"].ToString();
            if (flag.Equals("0"))
            {
                cuerpo.Append("AMBIENTE DE PRODUCCION - CORREO DE ALERTA - DATACREDITO CORP");
            }
            else
            {
                cuerpo.Append("PRUEBAS EN DESARROLLO O EN QA - CORREO DE ALERTA - DATACREDITO CORP");
            }
            cuerpo.Append("<br><hr align=\'left\' style=\'height:1px;width:570px;border:1px solid #000;\'>");
            cuerpo.Append(mensajeError);
            cuerpo.Append(("Consulta a DataCredito N° " + oDataCreditoCorpOUT.strNroOperacion));
            cuerpo.Append("<br><br><hr align=\'left\' style=\'height:1px;width:570px;border:1px solid #000;\'>");
            cuerpo.Append("Respuesta devuelta:");
            cuerpo.Append("<br><br>");
            cuerpo.Append(("  Mensaje : " + oDataCreditoCorpOUT.strMensajeError));
            cuerpo.Append("<br>");
            cuerpo.Append(("  Codigo  : " + oDataCreditoCorpOUT.strCodError));
            cuerpo.Append("<br><hr align=\'left\' style=\'height:1px;width:570px;border:1px solid #000;\'>");
            cuerpo.Append("Datos del Cliente:");
            cuerpo.Append("<br><br>");
            cuerpo.Append(("  Tipo Documento   : " + oDataCreditoCorpIN.istrTipoDocumento));
            cuerpo.Append("<br>");
            cuerpo.Append(("  Numero Documento : " + oDataCreditoCorpIN.istrNumeroDocumento));
            cuerpo.Append("<br>");
            cuerpo.Append(("  Nombre           : " + oDataCreditoCorpOUT.strNombres));
            cuerpo.Append("<br>");
            cuerpo.Append(("  Apellido Paterno : " + oDataCreditoCorpOUT.strApellidoPaterno));
            cuerpo.Append("<br>");
            cuerpo.Append(("  Apellido Materno : " + oDataCreditoCorpOUT.strApellidoMaterno));
            cuerpo.Append("<br><hr align=\'left\' style=\'height:1px;width:570px;border:1px solid #000;\'>");
            cuerpo.Append(("Código de Punto de Venta que realizó la consulta: " + oDataCreditoCorpIN.istrPuntoVenta));
            return cuerpo.ToString();
        }

        public bool Auditoria(String strCodTrx, String strDesTrx)
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
    }
}
