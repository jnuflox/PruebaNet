using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.WS.WSConsultarDeudaCuentaBRMS;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Claro.SISACT.WS
{
    public class BWConsultaDatosBRMSOAC
    {

        WSConsultarDeudaCuentaBRMS.DAT_ConsultaDatosBRMSOACSOAP11BindingQSService _objTransaccion = new DAT_ConsultaDatosBRMSOACSOAP11BindingQSService();

        public BWConsultaDatosBRMSOAC()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["RutaWS_ConsultaDatosBRMSOAC"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut_ConsultaDatosBRMSOAC"].ToString());
        }


        public void consultarDeudaCuentaBRMS(String strTdo, String strNdo, String strMesDisputa, BEItemGenerico objAudit,
                                             out String strCodigo, out string strDescripcion, out int intDeudaCantidadDocumentos,
                                             out double dblDeudaCastigadaMonto, out double dblDeudaVencidaMonto, out double dblDeudaTotal,
                                             out int intDisputaAntiguedad, out int intDisputaCantidad, out double dblDisputaMonto,
                                             out double dblPagoTotalMonto, out int intAntiguedadDeuda, out String strStatus, out String strMessage)
        {

            WSConsultarDeudaCuentaBRMS.ConsultarDeudaCuentaBRMS_dbRequestType inPut = new ConsultarDeudaCuentaBRMS_dbRequestType();
            WSConsultarDeudaCuentaBRMS.ConsultarDeudaCuentaBRMS_dbResponseType outPut = new ConsultarDeudaCuentaBRMS_dbResponseType();

            strCodigo = "";
            strDescripcion = "";
            strStatus = "";
            strMessage = "";

            intDeudaCantidadDocumentos = 0;
            dblDeudaCastigadaMonto = 0.0;
            dblDeudaVencidaMonto = 0.0;
            dblDeudaTotal = 0.0;
            intDisputaAntiguedad = 0;
            intDisputaCantidad = 0;
            dblDisputaMonto = 0.0;
            dblPagoTotalMonto = 0.0;
            intAntiguedadDeuda = 0;

            try
            {

                inPut.PV_TRX_ID_WS = objAudit.Codigo;
                inPut.PV_COD_APLICACION = objAudit.Codigo3;
                inPut.PV_USUARIO_APLIC = objAudit.Codigo2;
                inPut.PV_CLI_TIPO_DOC_IDENT = strTdo;
                inPut.PV_CLI_NRO_DOC_IDENT = strNdo;
                inPut.PN_NRO_MESES_DISPUTA = strMesDisputa; 


                #region RequestLog
                //DIL :: INI
                GeneradorLog log = null;
                log = new GeneradorLog(null, "ConsultarDeudaCuentaBRMS", null, "ConsultarDeudaCuentaBRMS");
                log.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][NUEVO-BRMS][REQUEST][INICIO]", ""), null);
                XmlSerializer xmlRequest = new XmlSerializer(typeof(WSConsultarDeudaCuentaBRMS.ConsultarDeudaCuentaBRMS_dbRequestType));
                StringBuilder builderRequest = new StringBuilder();
                TextWriter writerRequest = new StringWriter(builderRequest);
                xmlRequest.Serialize(writerRequest, inPut);
                writerRequest.Close();
                log.CrearArchivolog(null, builderRequest.ToString(), null);
                log.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][NUEVO-ConsultarDeudaCuentaBRMS][REQUEST][FIN]", ""), null);
                //DIL :: FIN
                #endregion

                outPut = _objTransaccion.consultarDeudaCuentaBRMS_db(inPut);


                #region ResponseLog
                //DIL :: INI
                log.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][NUEVO-ConsultarDeudaCuentaBRMS][RESPONSE][INICIO]", ""), null);
                XmlSerializer xmlResponse = new XmlSerializer(typeof(ConsultarDeudaCuentaBRMS_dbResponseType));
                StringBuilder builderResponse = new StringBuilder();
                TextWriter writerResponse = new StringWriter(builderResponse);
                xmlResponse.Serialize(writerResponse, outPut);
                writerRequest.Close();
                log.CrearArchivolog(null, builderResponse.ToString(), null);
                log.CrearArchivolog(null, string.Format("{0}-->{1}", "[PROY-32439][NUEVO-ConsultarDeudaCuentaBRMS][RESPONSE][FIN]", ""), null);
                //DIL :: FIN
                #endregion


                strCodigo = outPut.responseStatus.codeResponse;
                strDescripcion = outPut.responseStatus.descriptionResponse;

                if (outPut.responseData != null)
                {

                    foreach (ConsultarType row in outPut.responseData)
                    {

                        strStatus = row.XV_STATUS;
                        strMessage = row.XV_MESSAGE;
                        
                        if (row.XT_ESTADO_CUENTA != null)
                        {
                        foreach (XT_ESTADO_CUENTA_Row col in row.XT_ESTADO_CUENTA)
                        {

                            if (col != null)
                            {
                        
                                    dblDeudaVencidaMonto = Funciones.CheckDbl(col.DEUDA_VENCIDA);
                                    dblDeudaCastigadaMonto = Funciones.CheckDbl(col.DEUDA_CASTIGADA);
                                    dblDeudaTotal = Funciones.CheckDbl(col.DEUDA_TOTAL);
                                    intAntiguedadDeuda = Funciones.CheckInt(col.ANT_DEUDA);
                                    dblPagoTotalMonto = Funciones.CheckDbl(col.MONTO_PAG_TOTAL);
                                    dblDisputaMonto = Funciones.CheckDbl(col.MONTO_DISPUTA);
                                    intDisputaCantidad = Funciones.CheckInt(col.CANTIDAD_DISPUTA);
                                        intDisputaAntiguedad = Funciones.CheckInt(Math.Round(Funciones.CheckDbl(col.ANT_DISPUTA)));
                                    intDeudaCantidadDocumentos = Funciones.CheckInt(col.CANT_DOCUMENTOS);

                            }
                            

                        }
                        }


                    }

                }


            }
            catch (Exception ex)
            {
                strCodigo = "1";
                strDescripcion = "SISACT EX-" + ex.Message;
            }


        }


    }
}
