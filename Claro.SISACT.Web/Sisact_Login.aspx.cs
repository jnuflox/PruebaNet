using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using Claro.SISACT.Entity;
using Claro.SISACT.WS;
using System.Configuration;
using System.Data;
using System.Web.Security;
using Claro.SISACT.Common;
using Claro.SISACT.Business;

namespace Claro.SISACT.Web
{
    public partial class sisact_login : Funcions.Sisact_Webbase
    {
        
        GeneradorLog _objlog;

        public sisact_login()
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentEncoding = System.Text.Encoding.GetEncoding(1252);

            if ( !Page.IsPostBack ) 
            {

                _objlog = new GeneradorLog(this.CurrentUser);
                _objlog.CrearArchivolog("*************************************************************************");
                _objlog.CrearArchivolog("INCIO DEL MODULO Sisact_Login.aspx");

                LeeDatos();
            }
        }

        private void LeeDatos()
        {
            

            _objlog.CrearArchivolog("INCIO LEER DATOS : LeeDatos");


            String strUsuario = "";
            var objEmpleado = new BWLogin();
            String resultado = null;
            String mensaje = null;
            strUsuario = this.CurrentUser;

            string codAplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"];

            try
            {
                var oAuditoria = new BWAuditoria();
                _objlog.CrearArchivolog("***************************");
                _objlog.CrearArchivolog("LEER DATOS DE USUARIO : oAuditoria.LeerDatosUsuario");
                _objlog.CrearArchivolog("PARAMETROS : oAuditoria.LeerDatosUsuario");
                _objlog.CrearArchivolog("- strUsuario -> " + strUsuario);
                _objlog.CrearArchivolog("- codAplicacion -> " + codAplicacion);

                BEUsuarioSession objUsuarioSession = oAuditoria.LeerDatosUsuario(strUsuario, codAplicacion, this.CurrentUser);

                _objlog.CrearArchivolog("RESULTADO : oAuditoria.LeerDatosUsuario");

                if (objUsuarioSession != null)
                {
                    
                    _objlog.CrearArchivolog("- RESULTADO DIFERENTE A NULL -> CORRECTO");

                    _objlog.CrearArchivolog("ESTADO DE ACCESO -> " + objUsuarioSession.EstadoAcceso);
                    if (objUsuarioSession.EstadoAcceso == "1")
                    {

                        objUsuarioSession.Host = Request.ServerVariables["REMOTE_ADDR"];
                        _objlog.CrearArchivolog("objUsuarioSession.Host -> " + objUsuarioSession.Host);

                        bool bResultado = false;
                        String vstrNombres = null;
                        String vstrApellidos = null;                   
                        String vstrUsuario = null;
                        String vstrArea = null;

                        _objlog.CrearArchivolog("***************************");
                        _objlog.CrearArchivolog("VALIDAR LOGIN : objEmpleado.ValidarLogin");
                        _objlog.CrearArchivolog("PARAMETROS : objEmpleado.ValidarLogin");
                        _objlog.CrearArchivolog("- vCodigoUsuario -> " + objUsuarioSession.CodigoUsuario);
                        _objlog.CrearArchivolog("- vNombreCompleto ref -> ");
                        _objlog.CrearArchivolog("- vNombre ref ->");
                        _objlog.CrearArchivolog("- vApellido ref ->");
                        _objlog.CrearArchivolog("- vArea ref ->");
                        _objlog.CrearArchivolog("- vResultado ref ->");
                        _objlog.CrearArchivolog("- vMensaje ref ->");
                        
                        bResultado = objEmpleado.ValidarLogin(objUsuarioSession.CodigoUsuario, ref vstrUsuario, ref vstrNombres, ref vstrApellidos, ref vstrArea, ref resultado, ref mensaje);

                        _objlog.CrearArchivolog("RESULTADO : objEmpleado.ValidarLogin");
                        _objlog.CrearArchivolog("- resultado -> " + bResultado);
                        _objlog.CrearArchivolog("- ref vNombreCompleto -> " + vstrUsuario);
                        _objlog.CrearArchivolog("- ref vNombre -> " + vstrNombres);
                        _objlog.CrearArchivolog("- ref vApellido -> " + vstrApellidos);
                        _objlog.CrearArchivolog("- ref vArea -> " + vstrArea);
                        _objlog.CrearArchivolog("- ref vResultado -> " + resultado);
                        _objlog.CrearArchivolog("- ref vMensaje -> " + mensaje);

                        if (bResultado)
                        {
                            
                            lblUsuario.Text = vstrUsuario;
                            lblArea.Text = vstrArea;
                            txtCodUsuario.Text = objUsuarioSession.CodigoUsuario;

                            objUsuarioSession.NombreCompleto = vstrUsuario;
                            objUsuarioSession.AreaDes = vstrArea;

                            //Cargamos Opciones Permitidas para el usuario.
                            _objlog.CrearArchivolog("***************************");
                            _objlog.CrearArchivolog("OBTIENE LAS OPCIONES PERMITIDAS PARA EL USUARIO : ListarAccesosPagina");
                            _objlog.CrearArchivolog("PARAMETROS : ListarAccesosPagina");
                            _objlog.CrearArchivolog("- strCodUsuario -> " + objUsuarioSession.CodigoUsuario);

                            objUsuarioSession.CadenaOpciones = ListarAccesosPagina(objUsuarioSession.CodigoUsuario);
                            _objlog.CrearArchivolog("RESULTADO : ListarAccesosPagina");
                            _objlog.CrearArchivolog("- resultado -> " + objUsuarioSession.CadenaOpciones);


                            //Cargar codigo de usuario de SISACT
                            BLMaestro  oMaestro =  new BLMaestro();

                            _objlog.CrearArchivolog("***************************");
                            _objlog.CrearArchivolog("CARGAR CODIGO DE USUARIO SISCACT : oMaestro.ObtenerUsuarioLogin");
                            _objlog.CrearArchivolog("PARAMETROS : oMaestro.ObtenerUsuarioLogin");
                            _objlog.CrearArchivolog("- login -> " + objUsuarioSession.CodigoUsuarioRed);

                            BEUsuario oUsuario = oMaestro.ObtenerUsuarioLogin(objUsuarioSession.CodigoUsuarioRed);
                            _objlog.CrearArchivolog("RESULTADO : oMaestro.ObtenerUsuarioLogin");

                            objUsuarioSession.CodigoUsuarioSisact = oUsuario.UsuarioId.ToString(CultureInfo.InvariantCulture);

                            _objlog.CrearArchivolog("- resultado -> " + objUsuarioSession.CodigoUsuarioSisact);


                            _objlog.CrearArchivolog("CODIGO DE USUARIO SISACT : " + objUsuarioSession.CodigoUsuarioSisact);

                            if (objUsuarioSession.CodigoUsuarioSisact == "0")
                            {

                                _objlog.CrearArchivolog("CODIGO DE USUARIO SISACT IGUAL A CERO(0): -> Usuario no registrado en SISACT.");

                                Response.Write("<script language='javascript'>alert('Usuario no registrado en SISACT.');</script>");
                                Response.End();
                                
                            }
                            else
                            {

                                // Buscar Punto de Venta
                                var objBlPreventa = new BLPreventa();
                                _objlog.CrearArchivolog("***************************");
                                _objlog.CrearArchivolog("BUSCAR PUNTO DE VENTA : objBlPreventa.ListaPDVUsuario");
                                _objlog.CrearArchivolog("PARAMETROS : oMaestro.ObtenerUsuarioLogin");
                                _objlog.CrearArchivolog("- cod_usuario -> " + objUsuarioSession.CodigoUsuarioSisact);
                                _objlog.CrearArchivolog("- cod_producto -> ");

                                List<BEPuntoVenta> listaPuntoVenta = objBlPreventa.ListaPDVUsuario(long.Parse(objUsuarioSession.CodigoUsuarioSisact), "");

                                _objlog.CrearArchivolog("RESULTADO : objBlPreventa.ListaPDVUsuario");
                                if (listaPuntoVenta.Count == 0)
                                {
                                    _objlog.CrearArchivolog("resultado -> " + listaPuntoVenta.Count + " REGISTROS");
                                    Response.Write("<script language='javascript'>alert('Usuario no pertenece a algun Punto de Venta.');</script>");
                                    Response.End();
                                }

                                // Retorna los datos escogidos 
                                var itemPuntoVenta = (BEPuntoVenta)listaPuntoVenta[0];

                                objUsuarioSession.OficinaVenta = itemPuntoVenta.OvencCodigo;
                                objUsuarioSession.OficinaVentaDescripcion = itemPuntoVenta.OvenvDescripcion;
                                objUsuarioSession.CanalVenta = itemPuntoVenta.ToficCodigo;
                                objUsuarioSession.CanalVentaDescripcion = itemPuntoVenta.CanavDescripcion;
                                objUsuarioSession.Terminar = CurrentTerminal;


                                _objlog.CrearArchivolog("- OficinaVenta -> " + objUsuarioSession.OficinaVenta);
                                _objlog.CrearArchivolog("- OficinaVentaDescripcion -> " + objUsuarioSession.OficinaVentaDescripcion);
                                _objlog.CrearArchivolog("- CanalVenta -> " + objUsuarioSession.CanalVenta);
                                _objlog.CrearArchivolog("- CanalVentaDescripcion -> " + objUsuarioSession.CanalVentaDescripcion);
                                _objlog.CrearArchivolog("- Terminar -> " + objUsuarioSession.Terminar); 
                            
                                Session["Usuario"] = objUsuarioSession;

                                _objlog.CrearArchivolog("SE CREAR LA SESSION USUARIO -> Session['Usuario']");
                                _objlog.CrearArchivolog("*************************************************************************");
                            
                            }


                        }
                        else
                        {
                            
                            _objlog.CrearArchivolog("RESULTADO objEmpleado.ValidarLogin -> FALSE");
                            _objlog.CrearArchivolog("El usuario no se encuentra dentro de la Base de Datos");
                            _objlog.CrearArchivolog("*************************************************************************"); 

                            Response.Write("<script language='javascript'>alert('El usuario no se encuentra dentro de la Base de Datos');</script>");
                            Response.Write("<script language='javascript'>window.close()</script>");
                            Response.End();
                        }
                    }
                    else
                    {
                        _objlog.CrearArchivolog("ERROR SE REDIRECCIONA A Sisact_ErrorIngreso.aspx");
                        _objlog.CrearArchivolog("*************************************************************************"); 
                        Response.Redirect("Sisact_ErrorIngreso.aspx");
                    }

                }
                else
                {
                    _objlog.CrearArchivolog("ERROR SE REDIRECCIONA A Sisact_ErrorIngreso.aspx");
                    _objlog.CrearArchivolog("*************************************************************************"); 
                    Response.Redirect("Sisact_ErrorIngreso.aspx");
                }
            }
            catch (Exception ex)
            {

                _objlog.CrearArchivolog("EXCEPTION");
                _objlog.CrearArchivolog("EXCEPTION -> " + ex.Message);
                

                imgIngresar.Enabled = false;

                _objlog.CrearArchivolog("MUESTRA ERROR EN LA PAGINA : " + ex.Message);
                ManejaError(ex);

                _objlog.CrearArchivolog("*************************************************************************"); 

            }

        }


        protected void imgIngresar_Click(object sender, ImageClickEventArgs e)
        {

            Response.Redirect("Sisact_Index.aspx");

        }

   
        public string ListarAccesosPagina(string strCodUsuario)
        {
            string strClaves = "";

            string codApp = ConfigurationManager.AppSettings["CodigoAplicacion"];
            ArrayList lista = null;
            string errorMsg = null;
            string codError = null;

            var objAuditoria = new BWAuditoria();


            try
            {
                _objlog.CrearArchivolog("*******");
                _objlog.CrearArchivolog("BUSCAR PUNTO DE VENTA : objAuditoria.LeerPaginaOpcionesPorUsuario");
                _objlog.CrearArchivolog("PARAMETROS : objAuditoria.LeerPaginaOpcionesPorUsuario");
                _objlog.CrearArchivolog("- usuario -> " + strCodUsuario);
                _objlog.CrearArchivolog("- appCod -> " + codApp);
                _objlog.CrearArchivolog("- errorMsg ref -> ");
                _objlog.CrearArchivolog("- codError ref -> ");

                lista = objAuditoria.LeerPaginaOpcionesPorUsuario(strCodUsuario, codApp, ref errorMsg, ref codError);

                _objlog.CrearArchivolog("RESULTADO : objAuditoria.LeerPaginaOpcionesPorUsuario");
                _objlog.CrearArchivolog("- resultado -> " + lista.Count + " registros");
                _objlog.CrearArchivolog("- ref errorMsg -> " + errorMsg);
                _objlog.CrearArchivolog("- ref codError -> " + codError);

                if (lista.Count > 0)
                {
                    strClaves = "";
                    var item = new BEConsulSeguridad();
                    for (int j = 0; j <= lista.Count - 1; j++)
                    {
                        item = (BEConsulSeguridad)lista[j];
                        strClaves = strClaves + item.OPCICABREV + ",";
                    }
                }

                strClaves = strClaves.Substring(0, strClaves.Length - 1);

            }
            catch (Exception ex)
            {
                
                _objlog.CrearArchivolog("EXCEPTION");
                _objlog.CrearArchivolog("EXCEPTION -> " + ex.Message);

            }
            
            strClaves = strClaves.ToUpper();

            _objlog.CrearArchivolog("return -> " + strClaves);
            _objlog.CrearArchivolog("*******");

            return strClaves;
        }


    }
}