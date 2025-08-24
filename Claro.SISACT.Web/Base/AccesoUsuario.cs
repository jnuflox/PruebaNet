using System;
using System.Collections.Generic;
using System.Web;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;
using System.Configuration;
using System.Collections;
using Claro.SISACT.Common;
using Claro.SISACT.Business;
using Claro.SISACT.WS.WSAuditoria;

namespace Claro.SISACT.Web.Base
{
    public class AccesoUsuario
    {
        //PROY-24740
        public static bool ValidarAcceso(string codUsuarioExt, string strCuentaRed)
        {
            BEUsuarioSession objUsuario = null;
            BEUsuarioSession objUsuarioAudit = null;
            BEUsuario objUsuarioSisact = null;
            BWAuditoria objAudit = new BWAuditoria();
            bool blnOK = false;
            GeneradorLog _objLog = null;
 //PROY-140245 
            HttpContext.Current.Session["numDocUsuario"] = null;
            //FIN PROY-140245 
            try
            {
                _objLog = new GeneradorLog(strCuentaRed, null, null, "WEB");

                // Consulta Datos Usuario
                objUsuarioAudit = objAudit.LeerDatosUsuario(strCuentaRed);
  //PROY-140245 
                HttpContext.Current.Session["numDocUsuario"] = objUsuarioAudit.numeroDocumento; 
                // FIN PROY-140245 
                if (objUsuarioAudit != null && objUsuarioAudit.idUsuario > 0)
                {
                    // Consulta Datos Empleado
                    BWLogin objLogin = new BWLogin();
                    objUsuario = objLogin.DatosEmpleado(objUsuarioAudit.idUsuario, strCuentaRed);
                    objUsuario.idCuentaRed = strCuentaRed;
                    objUsuario.Terminal = Sisact_Webbase.CurrentTerminal;

                    // Consulta Opciones de Pagina
                    List<BESeguridad> objListaSeguridad = objAudit.LeerPaginaOpcionesPorUsuario(objUsuario.idUsuario);
                    string cadenaOpcionesPagina = string.Empty;
                    foreach (BESeguridad objSeguridad in objListaSeguridad)
                    {
                        cadenaOpcionesPagina = string.Format("{0}|{1}", cadenaOpcionesPagina, objSeguridad.OPCICABREV); //PROY-24740
                    }

                    // Consulta datos Usuario
                    objUsuarioSisact = (new BLGeneral()).ConsultaDatosUsuario(objUsuario.idCuentaRed);
                    if (objUsuarioSisact != null && objUsuarioSisact.UsuarioId > 0)
                    {
                        objUsuario.idUsuarioSisact = objUsuarioSisact.UsuarioId;
                        objUsuario.OficinaVenta = objUsuarioSisact.OficinaId;

                        if (objUsuario.OficinaVenta == ConfigurationManager.AppSettings["CONS_COD_PTOVTA_149"].ToString())
                        {
                            objUsuario.Perfil149 = true;
                        }
                    }

                    objUsuario.CadenaOpcionesPagina = cadenaOpcionesPagina;
                    objUsuario.CadenaPerfil = objUsuarioAudit.CadenaPerfil;
                    objUsuario.EstadoAcceso = objUsuarioAudit.EstadoAcceso;

                    blnOK = true;

                    //CNH
                    var objBlGeneral = new BLGeneral();
                    var intParametro = Funciones.CheckInt64(ConfigurationManager.AppSettings["constClienUNI_ParaTimeOut"]);

                    var strTimeout = "";
                    //_objLog = new GeneradorLog(strCuentaRed, null, null, "W");
                    _objLog.CrearArchivolog("***************************", null, null);
                    _objLog.CrearArchivolog("BUSCAR TIMEOUT SERVICIO: objBlPreventa.ListaPDVUsuario", null, null);
                    _objLog.CrearArchivolog("PARAMETROS : oMaestro.ObtenerUsuarioLogin", null, null);
                    _objLog.CrearArchivolog("- CodParam -> " + Funciones.CheckStr(intParametro), null, null);
                    List<BEParametro> ListaTimeOut = objBlGeneral.ListaParametros(intParametro);
                    if (ListaTimeOut.Count > 0)
                    {
                      _objLog.CrearArchivolog("- Result TimeOut -> " + strTimeout, null, null);

                      strTimeout = ((BEParametro)ListaTimeOut[0]).Valor;
                      objUsuario.TimeOutServicio = strTimeout;
                    }

                    HttpContext.Current.Session["Usuario"] = objUsuario;
                }
            }
            catch (Exception ex)
            {
                _objLog.CrearArchivologWS("ERROR WS ACCESO", null, null, ex);

                blnOK = false;
            }
            return blnOK;
        }

        //INC000003427525 - INI
        public static bool LeerOpcionesPorUsuario(string codUsuarioExt)
        {

            var obj = new BWAuditoria();
            var pMenuResponse = new OpcionesUsuarioResponse();
            string strCadenaAbreviaturaN2 = string.Empty;
            string strCadenaAbreviaturaN3 = string.Empty;
            bool blRespuesta = false;
            GeneradorLog _objLog = null;

            try
            {
                pMenuResponse = obj.LeerOpcionesPorUsuario(codUsuarioExt);

                _objLog = new GeneradorLog(codUsuarioExt, null, null, "WEB");
                _objLog.CrearArchivolog("*************************** Metodo ObtenerPaginas Inicio ***************************", null, null);

                if (pMenuResponse.menues.menuItem != null)
                {

                    foreach (MenuType item in pMenuResponse.menues.menuItem)
                    {
                        if (item.datosMenu != null)
                        {
                            if (item.datosMenu.padre != null)
                            {

                                foreach (PadreType itemPadre in item.padres.padreItem)
                                {

                                    if (itemPadre.datosPadre.descripcion != null)
                                    {
                                        strCadenaAbreviaturaN2 = strCadenaAbreviaturaN2 + " | " + itemPadre.datosPadre.abreviatura;
                                        
                                        if (itemPadre.hijos.hijoItem != null)
                                        {
                                            foreach (HijoType itemHijo in itemPadre.hijos.hijoItem)
                                            {
                                                if (itemHijo.datosHijos.abreviatura != null)
                                                {
                                                    strCadenaAbreviaturaN3 = strCadenaAbreviaturaN3 + " | " + itemHijo.datosHijos.abreviatura;
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                    blRespuesta = true;
                }

                _objLog.CrearArchivolog("INC000003427525 - strCadenaAbreviaturaN2 ==>" + strCadenaAbreviaturaN2, null, null);
                HttpContext.Current.Session["AbreviaturaPaginasN2"] = strCadenaAbreviaturaN2;
                //HttpContext.Current.Session["AbreviaturaPaginasN3"] = strCadenaAbreviaturaN3;

            }
            catch (Exception ex)
            {
                blRespuesta = false;
                _objLog.CrearArchivolog("*************************** Metodo ObtenerPaginas ERROR ***************************", null, null);
                _objLog.CrearArchivolog("INC000003427525 - ex.Message ==>" + ex.Message, null, null);
                _objLog.CrearArchivolog("INC000003427525 - ex.StackTrace ==> " + ex.StackTrace, null, null);

            }

            _objLog.CrearArchivolog("*************************** Metodo ObtenerPaginas Fin ***************************", null, null);

            return blRespuesta;
        }
        //INC000003427525 - FIN
    }
}