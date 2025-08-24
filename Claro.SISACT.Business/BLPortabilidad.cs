using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;
using Claro.SISACT.Common;
using System.Collections;
using System.Configuration;
using System.Data;

namespace Claro.SISACT.Business
{
    public class BLPortabilidad
    {
        public List<BEParametroPortabilidad> ListarParametroPortabilidad(string tipo_parametro, string codigo_parametro, string ref1, string ref2, string ref3, int status)
        {
            DAPortabilidad obj = new DAPortabilidad();
            return obj.ListarParametroPortabilidad(tipo_parametro, codigo_parametro, ref1, ref2, ref3, status);
        }

        public bool ValidarDisponiblePortabilidad(string[] listaTelefono, ref Int64 nroSEC)
        {
            foreach (string nroTelefono in listaTelefono)
            {
                if (!string.IsNullOrEmpty(nroTelefono))
                {
                    nroSEC = new DAPortabilidad().ValidarDisponibleNroPorta(nroTelefono);
                    if (nroSEC > 0) return false;
                }
            }
            return true;
        }

        public static bool VigenciaPYLC_PosPago(string strDocCliente, Int64 grupo_camp, out String o_codigo, out String o_mensaje)
        {
            DAPortabilidad o_da = new DAPortabilidad();
            return o_da.VigenciaPYLC_PosPago(strDocCliente, grupo_camp, out o_codigo, out o_mensaje);
        }

        public String ValidarTienePortabilidadTablet(string tipoDocumento, string nroDocumento, int strCodParamCampanaPortaMasTablet, int strCodParamNumDiasVigenciaCampanaPortaMasTablet) //CAMPAÑA PORTA+TABLET - INICIO
        {
            return (new DAPortabilidad().ValidarTienePortabilidadTablet(tipoDocumento, nroDocumento, strCodParamCampanaPortaMasTablet, strCodParamNumDiasVigenciaCampanaPortaMasTablet));
        } //CAMPAÑA PORTA+TABLET - FIN
        
        public Int64 GrabarEvaluacionPersona(BESolicitudPersona objSolicitud)
        {
            return new DAPortabilidad().GrabarEvaluacionPersona(objSolicitud);
        }

        public Int64 GrabarEvaluacionEmpresa(BESolicitudEmpresa objSolicitud)
        {
            Int64 nroSEC = new DAPortabilidad().GrabarEvaluacionEmpresa(objSolicitud);

            if (objSolicitud.REPRESENTANTE_LEGAL.Count > 0)
            {
                DASolicitud obj = new DASolicitud();
                for (int i = 0; i < objSolicitud.REPRESENTANTE_LEGAL.Count; i++)
                {
                    BERepresentanteLegal objRepLegal = new BERepresentanteLegal();
                    objRepLegal = (BERepresentanteLegal)objSolicitud.REPRESENTANTE_LEGAL[i];
                    objRepLegal.SOLIN_CODIGO = nroSEC;
                    obj.GrabarSolicitudRepLegal(objRepLegal);
                }
            }
            return nroSEC;
        }

        public bool GrabarNumeroPortabilidad(BENumeroPortabilidad objDetalle)
        {
            return new DAPortabilidad().GrabarNumeroPortabilidad(objDetalle);
        }

        public bool GrabarArchivoPortabilidad(BEArchivo objArchivo)
        {
            return new DAPortabilidad().GrabarArchivoPortabilidad(objArchivo);
        }

        public bool EnviarMesaPortabilidad(Int64 nroSEC, string strUsuario)
        {
            return new DAPortabilidad().EnviarMesaPortabilidad(nroSEC, strUsuario);
        }

        public List<BEArchivo> ListarAchivosAdjunto(Int64 idArchivo, Int64 nroSEC, string tipoArchivo, string flagEstado)
        {
            return new DAPortabilidad().ListarAchivosAdjunto(idArchivo, nroSEC, tipoArchivo, flagEstado);
        }

        public List<BEReciboDeposito> ListarRecibo(Int64 nroSEC, int banco_id, string nro_recibo)
        {
            return new DAPortabilidad().ListarRecibo(nroSEC, banco_id, nro_recibo);
        }

        public bool GrabarDatosEvaluador(Int64 nroSEC, double nroRA, string tipoGarantia, string estadoSEC, string estadoSECDes, double total_garantia,
                                        string comentario_pdv, string comentario_evaluador, string comentario_sistema, string login, string loginAutorizador,
                                        string estadoPort, ref string rFlagProceso)
        {
            return new DAPortabilidad().GrabarDatosEvaluador(nroSEC, nroRA, tipoGarantia, estadoSEC, estadoSECDes, total_garantia,
                                                            comentario_pdv, comentario_evaluador, comentario_sistema, login, loginAutorizador,
                                                            estadoPort, ref rFlagProceso);
        }

        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        public static List<BEPorttSolicitud> ValidarRespuestaWSCP(string strNumeroSecuencial, ref string strCodResp, ref string strMensResp)
        {
            return new DAPortabilidad().ValidarRespuestaWSCP(strNumeroSecuencial, ref strCodResp, ref strMensResp);
        }

        public static bool  ValidarConsultaPreviaSEC(BeConsultaPrevia objConsultaPrevia, ref string strNumeroSecuencial, ref string strCodResp, ref string strMensResp)
        {
            return new DAPortabilidad().ValidarConsultaPreviaSEC(objConsultaPrevia, ref strNumeroSecuencial, ref strCodResp, ref strMensResp);
        }

        public static BEItemMensaje ActualizarConsultaPreviaCP(BeConsultaPrevia objConsultaPrevia, ref string rstrCodResp, ref string rstrMensResp)
        {
            return new DAPortabilidad().ActualizarConsultaPreviaCP(objConsultaPrevia, ref rstrCodResp, ref rstrMensResp);
        }

        //INI: PROY-140223 IDEA-140462
        public static void Actualizar_SEC_sin_CP(BeConsultaPrevia objConsultaPrevia, ref string rstrCodResp, ref string rstrMensResp)
        {
            new DAPortabilidad().Actualizar_SEC_sin_CP(objConsultaPrevia, ref rstrCodResp, ref rstrMensResp);
        }
        //FIN: PROY-140223 IDEA-140462

        
        public static List<BeConsultaPrevia> ObtenerListaSolicitudPortabilidadBySec(Int64 p_id_sec)
        {
            return new DAPortabilidad().ObtenerListaSolicitudPortabilidadBySec(p_id_sec);
        }

        public static bool EliminarRegistroCPPortabilidad(Int64 idPortabilidad)
        {
            return new DAPortabilidad().EliminarRegistroCPPortabilidad(idPortabilidad);
        }

        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

//PROY-31393 INI
        public static List<string> PorttValidaABCDP(BEPorttConfiguracion objPortConfiguracion)
        {
            return new DAPortabilidad().PorttValidaABCDP(objPortConfiguracion);
        }
        //PROY-31393 FIN

        //INI: PROY-140223 IDEA-140462
        public static bool Envio_mesa_portabilidad_sin_cp(List<BENumeroPortabilidad> pLista, ref string pMensaje)
        {
            return new DAPortabilidad().Envio_mesa_portabilidad_sin_cp(pLista, ref pMensaje);
        }
        //FIN: PROY-140223 IDEA-140462

        //INI: PROY-140223 IDEA-140462
        public string Obtener_Class_PDV(string pClasificacion)
        {
            return new DAPortabilidad().Obtener_Class_PDV(pClasificacion);
        }
        //FIN: PROY-140223 IDEA-140462

        //INI PROY-CAMPANA LG
        public static void validaVentaCampanaLG(string numDoc, string tipoDoc, string codCampana, string paranGrupo, ref string strCodResp, ref string strMensResp)
        {
            new DAPortabilidad().validaVentaCampanaLG(numDoc,tipoDoc ,codCampana,paranGrupo, ref strCodResp, ref strMensResp);
        }
        //FIN PROY-CAMPANA LG

        #region INI: PROY-140335 IDEA-140307 
        /// <summary>
        /// Valida la lineas disponibles, que no tengan portabilidad y lineas con portabilidad
        /// pendiente a utilizar. Además, se recupera sec de lineas pedientes para anularlas.
        /// </summary>
        /// <param name="arrTelefono"></param>
        /// <returns></returns>

        public static List<BEValidalineaPorta> ValidarDisponibilidadLinea(string[] arrTelefono)
        {
            BEItemMensaje objMensaje = new BEItemMensaje();
            BEValidalineaPorta dtoLineaPorta = null;
            List<BEValidalineaPorta> ListPortabilidad = new List<BEValidalineaPorta>();
            if (arrTelefono != null)
            {
            arrTelefono.ToList().ForEach(a =>
            {
                if (!string.IsNullOrEmpty(a))
                {
                    dtoLineaPorta = new BEValidalineaPorta();
                    dtoLineaPorta = new DAPortabilidad().ValidarDisponibilidadLinea(a);
                    ListPortabilidad.Add(dtoLineaPorta);

                }
            });
            }
            return ListPortabilidad;
        }
        /// <summary>
        /// Metodo que Anula la Sec de lineas pendiente
        /// </summary>
        /// <param name="Sec"></param>
        /// <returns></returns>
            public   string AnularSecPortabilidad(int Sec)
            {
                return new DAPortabilidad().AnularSecPortabilidad(Sec);
            }
            //PROY-140335 RF1
            // Valida si la linea tiene Consultas previas registrados en la tabla SISACTSS_REPOSITORIO_ABDCP
            public static BEPorttSolicitud ValidarRepositorioABDCP(BEPorttSolicitud objConsultaPrevia, ref string strCodRespuesta, ref string strMsgRespuesta)
            {
                return new DAPortabilidad().ValidarRepositorioABDCP(objConsultaPrevia,ref strCodRespuesta,ref strMsgRespuesta);
            }
            //PROY-140335 RF1
        
        #endregion



          
    }
}