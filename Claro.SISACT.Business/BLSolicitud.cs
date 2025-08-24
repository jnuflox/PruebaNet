using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;
using System.Collections;
using System.Configuration;
using System.Data;

namespace Claro.SISACT.Business
{
    public class BLSolicitud
    {
        public DataTable ObtenerSolicitudPersona(Int64 nroSEC)
        {
            return new DASolicitud().ObtenerSolicitudPersona(nroSEC);
        }
        public List<BESolicitudEmpresa> ObtenerHistoricoPersona(Int64 nroSEC, string tipoDocumento, string nroDocumento, DateTime fechaInicio, DateTime fechaFin, string estado)
        {
            return new DASolicitud().ObtenerHistoricoPersona(nroSEC, tipoDocumento, nroDocumento, fechaInicio, fechaFin, estado);
        }
        public List<BESolicitudEmpresa> ObtenerHistoricoEmpresa(Int64 nroSEC, string tipoDocumento, string nroDocumento, DateTime fechaInicio, DateTime fechaFin, string estado)
        {
            return new DASolicitud().ObtenerHistoricoEmpresa(nroSEC, tipoDocumento, nroDocumento, fechaInicio, fechaFin, estado);
        }
        public List<BEEstado> ObtenerLogEstados(Int64 nroSEC)
        {
            return new DASolicitud().ObtenerLogEstados(nroSEC);
        }
        public List<BEEstado> ObtenerHistoricoEstadosSOT(Int64 nroSEC)
        {
            return new DASolicitud().ObtenerHistoricoEstadosSOT(nroSEC);
        }
        public List<BEArchivo> ObtenerArchivos(Int64 nroSEC)
        {
            return new DASolicitud().ObtenerArchivos(nroSEC);
        }
        public bool AsignarUsuarioSEC(Int64 nroSEC, string strUsuario, string strNroDocumento, string strOrigen)
        {
            return new DASolicitud().AsignarUsuarioSEC(nroSEC, strUsuario, strNroDocumento, strOrigen);
        }
        public bool LiberarSEC(Int64 nroSEC)
        {
            return new DASolicitud().LiberarSEC(nroSEC);
        }
        public bool RechazarSEC(Int64 nroSEC, string strComentarioPdv, string strComentarioEval, string strAprobador, string strFlgReingreso)
        {
            return new DASolicitud().RechazarSEC(nroSEC, strComentarioPdv, strComentarioEval, strAprobador, strFlgReingreso);
        }
        public bool RechazarPoolSEC(Int64 nroSEC, string strAprobador, string strMotivoId, string strObservacion, string strCodEstado)
        {
            return new DASolicitud().RechazarPoolSEC(nroSEC, strAprobador, strMotivoId, strObservacion, strCodEstado);
        }
        public bool GrabarLogHistorico(Int64 nroSEC, string strEstado, string strUsuario)
        {
            return new DASolicitud().GrabarLogHistorico(nroSEC, strEstado, strUsuario);
        }

        public void ObtenerUsuarioAsignadoSEC(Int64 nroSEC, string flagBandeja, ref string strCodUsuAsignado, ref string strUsuAsignado)
        {
            new DASolicitud().ObtenerUsuarioAsignadoSEC(nroSEC, flagBandeja, ref strCodUsuAsignado, ref strUsuAsignado);
        }
        public Int64 AsignarSECAutomatica(string strUsuario)
        {
            return new DASolicitud().AsignarSECAutomatica(strUsuario);
        }
        public DataTable ObtenerPoolEvaluadorPersona(string estado, DateTime fecha_inicio, DateTime fecha_fin)
        {
            return new DASolicitud().ObtenerPoolEvaluadorPersona(estado, fecha_inicio, fecha_fin);
        }
        public int ObtenerNroSECPendiente(string strTipoDoc, string strNroDoc)
        {
            return new DASolicitud().ObtenerNroSECPendiente(strTipoDoc, strNroDoc);
        }
        public DataTable ObtenerSECPendiente(string strTipoDoc, string strNroDoc)
        {
            return new DASolicitud().ObtenerSECPendiente(strTipoDoc, strNroDoc);
        }
        public Int64 ObtenerSECPendienteVentaSinPago(string strTipoDoc, string strNroDoc)
        {
            return new DASolicitud().ObtenerSECPendienteVentaSinPago(strTipoDoc, strNroDoc);
        }
        public BESolicitudEmpresa ObtenerSolicitudEmpresa(Int64 nroSEC)
        {
            return new DASolicitud().ObtenerSolicitudEmpresa(nroSEC);
        }

        public BESolicitudEmpresa obtenerEstadoSolEmp(Int64 nroSEC)
        {
            return new DASolicitud().obtenerEstadoSolEmp(nroSEC);
        }

        public string ValidarSECRecurrente(string strTipoDocumento, string strNroDocumento, string strOferta, string strCasoEspecial,
                                            string strCadenaDetalle, ref string flgReingresoSec)
        {
            return new DASolicitud().ValidarSECRecurrente(strTipoDocumento, strNroDocumento, strOferta, strCasoEspecial, strCadenaDetalle, ref flgReingresoSec);
        }
        public Int64 RegistrarEvaluacionDTH_HFC(BESolicitudPersona objSolicitud)
        {
            return new DASolicitud().RegistrarEvaluacionDTH_HFC(objSolicitud);
        }
        public Int64 RegistrarEvaluacion(BESolicitudPersona objSolicitud)
        {
            return new DASolicitud().RegistrarEvaluacion(objSolicitud);
        }
        public Int64 GrabarEvaluacionEmpresa(BESolicitudEmpresa item)
        {
            DASolicitud obj = new DASolicitud();
            Int64 nroSEC;
            int i = 0;
            nroSEC = obj.GrabarEvaluacionEmpresa(item);
            if (item.REPRESENTANTE_LEGAL.Count > 0)
            {
                for (i = 0; i < item.REPRESENTANTE_LEGAL.Count; i++)
                {
                    BERepresentanteLegal oRepLegal = new BERepresentanteLegal();
                    oRepLegal = (BERepresentanteLegal)item.REPRESENTANTE_LEGAL[i];
                    oRepLegal.SOLIN_CODIGO = nroSEC;
                    obj.GrabarSolicitudRepLegal(oRepLegal);
                }
            }
            return nroSEC;
        }

        public bool GrabarSolicitudRepLegal(BERepresentanteLegal item)
        {
            return new DASolicitud().GrabarSolicitudRepLegal(item);
        }

        public bool GrabarComentario(BEComentario item)
        {
            return new DASolicitud().GrabarComentario(item);
        }

        public bool GrabarArchivo(Int64 P_SOLIN_CODIGO, string P_ARCHV_NOM_ARC, string P_ARCHV_RUT_ARC, string P_ARCHC_USU_REG)
        {
            return new DASolicitud().GrabarArchivo(P_SOLIN_CODIGO, P_ARCHV_NOM_ARC, P_ARCHV_RUT_ARC, P_ARCHC_USU_REG);
        }

        public bool InsertarSolDireccion(BEDireccionCliente oDireccion, Int64 nroSEC)
        {
            return new DASolicitud().InsertarSolDireccion(oDireccion, nroSEC);
        }
        public bool GrabarEvaluacionDatosCreditos(BEDatosCreditos item)
        {
            return new DASolicitud().GrabarEvaluacionDatosCreditos(item);
        }
        public BEEvaluacion ObtenerComentarioSEC(Int64 nroSEC)
        {
            return new DASolicitud().ObtenerComentarioSEC(nroSEC);
        }
        public List<BEComentario> ObtenerComentarioSEC(Int64 nroSEC, string tipoComentario)
        {
            return new DASolicitud().ObtenerComentarioSEC(nroSEC, tipoComentario);
        }
        public DataTable ObtenerCostoInstalacion(Int64 nroSEC)
        {
            return new DASolicitud().ObtenerCostoInstalacion(nroSEC);
        }
        public DataTable ObtenerCostoInstalacionHFC(Int64 nroSEC)
        {
            return new DASolicitud().ObtenerCostoInstalacionHFC(nroSEC);
        }
        public void Insertar_Correccion_Nombres(List<string> oItem)
        {
            new DASolicitud().Insertar_Correccion_Nombres(oItem);
        }
        public void AprobarCreditos(List<string> oItem)
        {
            new DASolicitud().AprobarCreditos(oItem);
        }
        public void ActualizarGarantia(List<string> oItem)
        {
            new DASolicitud().ActualizarGarantia(oItem);
        }
        public void ActualizarReingresoSEC(Int64 nroSEC, string flgReingresoSEC)
        {
            new DASolicitud().ActualizarReingresoSEC(nroSEC, flgReingresoSEC);
        }
        public void AprobarSustentoIngreso(Int64 nroSEC, string usuario, double ingreso)
        {
            new DASolicitud().AprobarSustentoIngreso(nroSEC, usuario, ingreso);
        }
        public DataSet ObtenerDetalleSECPendiente(Int64 nroSEC)
        {
            return new DASolicitud().ObtenerDetalleSECPendiente(nroSEC);
        }
        public void GrabarSOTMigracionHFC(Int64 nroSEC, string nroSOT)
        {
            new DASolicitud().GrabarSOTMigracionHFC(nroSEC, nroSOT);
        }
        public List<BEEstado> ObtenerEstadoSot(Int64 nroSEC, Int64 nroSot)
        {
            return new DASolicitud().ObtenerEstadoSot(nroSEC, nroSot);
        }
        public BEAgendamiento ObtenerAgendamientoSga(Int64 nroSot)
        {
            return new DASolicitud().ObtenerAgendamientoSga(nroSot);
        }
        public string ActualizaDirreccionSga(Int64 nroSot, BEDireccionCliente oDireccion, ref string msgResp)
        {
            return new DASolicitud().ActualizaDirreccionSga(nroSot, oDireccion, ref msgResp);
        }
        public static void ObtenerAcuerdosBySec(Int64 nroSec, ref BEAcuerdo beAcuerdo, ref List<BEAcuerdoDetalle> listAcuerdoDetalle, ref List<BEAcuerdoServicio> listAcuerdoServicio)
        {
            DASolicitud.ObtenerAcuerdosBySec(nroSec, ref  beAcuerdo, ref listAcuerdoDetalle, ref listAcuerdoServicio);
        }
        public static string AnularSot(Int64 nroSot, ref string mensaje)
        {
            return DASolicitud.AnularSot(nroSot, ref mensaje);
        }
        public static bool AnularSEC(string pstrSEC, string pstrNroDocumento, string pstrCodUsuario)
        {
            return DASolicitud.AnularSEC(pstrSEC, pstrNroDocumento, pstrCodUsuario);
        }
        public bool InsertarSolDireccionVenta(BEDireccionCliente oDireccion, Int64 nroSEC)
        {
            return new DASolicitud().InsertarSolDireccionVenta(oDireccion, nroSEC);
        }
        public bool InsertarSolDireccionVenta_LTE(BEDireccionCliente oDireccion, Int64 nroSEC)
        {
             return new DASolicitud().InsertarSolDireccionVenta_LTE(oDireccion, nroSEC);
        }
        public bool InsertarSolDireccion_LTE(BEDireccionCliente oDireccion, Int64 nroSEC)
        {
            return new DASolicitud().InsertarSolDireccion_LTE(oDireccion, nroSEC);
        }

        public static void verificarPago(Int64 idSoli, ref string p_resultado)
        {
            DASolicitud.verificarPago(idSoli, ref p_resultado);
        }
        //PROY-29121-INI
        public List<BEEmpresaCategoria> ListaEmpresaReferencia(string tipoDocumento, string nroDocumento)
        {
            return DASolicitud.ListaEmpresaReferencia(tipoDocumento, nroDocumento);
        }
        //PROY-29121-FIN

        //INC000001337773 - INICIO
        public static void rechazoEvaluacion(BEClienteCuenta objCliente, string strMotivo, string strusuario, string tipoOrigen, string strLinea, ref string p_nrolog, ref string p_deslog)
        {
            DASolicitud.rechazoEvaluacion(objCliente, strMotivo, strusuario, tipoOrigen, strLinea, ref p_nrolog, ref p_deslog);
        }
        //INC000001337773 - FIN

        //PROY-32439 MAS INI Consulta Departamento + Region
        public BEPuntoVenta ConsultarRegionDepartamentoporOficina(String strCodigoPuntoVenta, out String strDepRegCodigo, out String strDepRegRespuesta)
        {
            return DASolicitud.ConsultarRegionDepartamentoporOficina(strCodigoPuntoVenta, out strDepRegCodigo, out strDepRegRespuesta);
        }
        //PROY-32439 MAS FIN Consulta Departamento + Region

        //INICIO|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18
        public static List<BEClienteSAP> ConsultaCorreoHistorico(string strTipoDocumento, string strNumeroDocumento, out string strMensajeRpt)
        {
            return DASolicitud.ConsultaCorreoHistorico(strTipoDocumento, strNumeroDocumento, out strMensajeRpt);
        }

        public static string ActualizarCorreoElectronico(BEClienteSAP objCliente, string strUsuario, string strComentario, out string strMsjRpta)
        {
            return new DASolicitud().ActualizarCorreoElectronico(objCliente, strUsuario, strComentario, out strMsjRpta);
        }

        public bool RegistrarCliente(BEClienteSAP oClienteSAP)
        {
            return new DASolicitud().RegistrarCliente(oClienteSAP);
        }
        //FIN|PROY-30162-IDEA-32487 ENVIO BOLETA ELECTRONICA PREPAGO //RGP_BOL_18

        //INICIO PROYECTO 140141 - KV FASE 2

        public string BGrabarSecTransac(string strTrsn_id, int strTpsn_sec, string strTpsv_num, string strTpsv_user, string strTpsv_email)
        {
            return new DASolicitud().DGrabarSecTransac(strTrsn_id, strTpsn_sec, strTpsv_num, strTpsv_user, strTpsv_email);
        }
       //FIN PROYECTO 140141 - KV FASE 2        
       
//INI PROY-31948 INI
        public static BECuota ConsultaCuotasPendientesPVU(string strTipoDocumento, string strNroDocumento, ref string CodRespuesta, ref string MsjRespuesta)
        {
            return DASolicitud.ConsultaCuotasPendientesPVU(strTipoDocumento, strNroDocumento, ref CodRespuesta, ref MsjRespuesta);
        }
//INI PROY-31948 FIN
//INI PROY-31948_Migracion
        public void AprobarCreditosReno(BESolicitudEmpresa oItem)
        {
            DASolicitud.AprobarCreditosReno(oItem);
        }

        public static bool GrabarDatosEvaluadorCheckList(Int64 nroSEC, double nroDG, double nroRA, string estadoSEC, string estadoSECDes,
                                                    double cargo_fijo_eva, double total_garantia, string comentario_pdv, string comentario_evaluador,
                                                    string comentario_sistema, string login, string loginAutorizador, ref string rFlagProceso, ref string rMsg)
        {

            return DASolicitud.GrabarDatosEvaluadorCheckList(nroSEC, nroDG, nroRA, estadoSEC, estadoSECDes, cargo_fijo_eva, total_garantia,
                                                        comentario_pdv, comentario_evaluador, comentario_sistema, login, loginAutorizador,
                                                        ref rFlagProceso, ref rMsg);
        } 

        public static bool GrabarRechazoSolicitud(Int64 secId, string usuario_login, string motivo_id, string obs, string codestado, ref string rProceso, ref string rMsg)
        {
            return DASolicitud.GrabarRechazoSolicitud(secId, usuario_login, motivo_id, obs, codestado, ref rProceso, ref rMsg);
        }

        public static bool ActualizarComentarios(long pCOD_SEC, string pComentarioPDV, string pComentarioEval)
        {
            return DASolicitud.ActualizarComentarios(pCOD_SEC, pComentarioPDV, pComentarioEval);
        }

        public static bool ObtenerParamSolicitud(string nroSEC, ref string Idcanal, ref string idPtoVen, ref Int64 idConsultor, ref Int64 IdFlex, ref Int64 idAutorizador)
        {
            return DASolicitud.ObtenerParamSolicitud(nroSEC, ref Idcanal, ref idPtoVen, ref idConsultor, ref IdFlex, ref idAutorizador);
        }

        public static string obtenerUsuarioXSec(Int64 solin_codigo, ref string canac_codigo)
        {
            return DASolicitud.obtenerUsuarioXSec(solin_codigo, ref canac_codigo);
            }

        public static ArrayList obtenerDatoAuxiliarRepresentanteLegalD(Int64 numSec, string usuario)
        {
            return DASolicitud.obtenerDatoAuxiliarRepresentanteLegalD(numSec, usuario);
        }

        public static void grabarComentarios(BEEvaluacion objEvaluacion)
        {
            DASolicitud.grabarComentarios(objEvaluacion);
        }
        //FIN PROY-31948_Migracion


    }
}
