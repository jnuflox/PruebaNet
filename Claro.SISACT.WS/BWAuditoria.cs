using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.Common;
using Claro.SISACT.WS.WSAuditoria;

namespace Claro.SISACT.WS
{
    public class BWAuditoria
    {
        EbsAuditoriaService _objTransaccion = new EbsAuditoriaService();
        GeneradorLog _objLog = null;

        public BWAuditoria()
		{
            _objTransaccion.Url = ConfigurationManager.AppSettings["consRutaWSSeguridad"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["time"].ToString());
		}

        public BEUsuarioSession LeerDatosUsuario(String strCuentaRed)
        {
            BEUsuarioSession objUsuario = null;
            _objLog = new GeneradorLog(null, strCuentaRed, null, "WEB");

            try
            {
                String strCodPerfilCadena = String.Empty;
                AccesoRequest objRequest = new AccesoRequest();
                AccesoResponse objResponse = new AccesoResponse();

                objRequest.usuario = strCuentaRed;
                objRequest.aplicacion = ConfigurationManager.AppSettings["CodigoAplicacion"].ToString();
                
                // Consulta Datos Usuario
                objResponse = _objTransaccion.leerDatosUsuario(objRequest);

                if (objResponse.resultado.estado == "1")
                {
                    objUsuario = new BEUsuarioSession();
                    objUsuario.idUsuario = Funciones.CheckInt64(objResponse.auditoria.AuditoriaItem.item[0].codigo);
                    //PROY-140245 
                    objUsuario.numeroDocumento = objResponse.empleado.dni;
                    for (int i = 0; i <= (objResponse.auditoria.AuditoriaItem.item.Length - 1); i++)
                    {
                        strCodPerfilCadena += objResponse.auditoria.AuditoriaItem.item[i].perfil + ",";
                    }

                    objUsuario.CadenaPerfil = strCodPerfilCadena.Substring(0, strCodPerfilCadena.Length - 1);
                    objUsuario.EstadoAcceso = objResponse.resultado.estado;
                }

                return objUsuario;
            }
            finally
            {
                _objTransaccion.Dispose();
            }
        }

        public List<BESeguridad> LeerPaginaOpcionesPorUsuario(Int64 idUsuario)
        {
            try
            {
                List<BESeguridad> lista = new List<BESeguridad>();
                PaginaOpcionesUsuarioRequest objRequest = new PaginaOpcionesUsuarioRequest();
                PaginaOpcionesUsuarioResponse objResponse = new PaginaOpcionesUsuarioResponse();
                PaginaOpcionType[] objOpcion;

                objRequest.user = Funciones.CheckInt(idUsuario);
                objRequest.aplicCod = Funciones.CheckInt(ConfigurationManager.AppSettings["CodigoAplicacion"].ToString());
                objResponse = _objTransaccion.leerPaginaOpcionesPorUsuario(objRequest);

                objOpcion = objResponse.listaOpciones;
                if (objResponse.resultado == "0")
                {
                    if (objOpcion != null)
                    {
                        for (int i = 0; i < objOpcion.Length; i++)
                        {
                            BESeguridad item = new BESeguridad();
                            item.OPCICCOD = objOpcion[i].opcicCod;
                            item.OPCICABREV = objOpcion[i].clave;
                            item.OPCICDES = objOpcion[i].opcicDes;
                            lista.Add(item);
                        }
                    }
                }
                return lista;
            }
            finally
            {
                _objTransaccion.Dispose();
            }
        }

        public bool registrarAuditoria(String strTransaccion,
                                        String strServicio,
                                        String strIPCliente,
                                        String strNombreCliente,
                                        String strIPServidor,
                                        String strNombreServidor,
                                        String strCuentaUsuario,
                                        String strTelefono,
                                        String strMonto,
                                        String strTexto)
        {
            try
            {
                RegistroRequest objRequest = new RegistroRequest();
                RegistroResponse objResponse = new RegistroResponse();

                objRequest.transaccion = strTransaccion;
                objRequest.servicio = strServicio;
                objRequest.ipCliente = strIPCliente;
                objRequest.nombreCliente = strNombreCliente;
                objRequest.ipServidor = strIPServidor;
                objRequest.nombreServidor = strNombreServidor;
                objRequest.cuentaUsuario = strCuentaUsuario;
                objRequest.telefono = strTelefono;
                objRequest.monto = strMonto;
                objRequest.texto = strTexto;

                objResponse = _objTransaccion.registroAuditoria(objRequest);

                return (objResponse.estado == "1");
            }
            finally
            {
                _objTransaccion.Dispose();
            }
        }

        public OpcionesUsuarioResponse LeerOpcionesPorUsuario(string idUsuario)
        {
            EbsAuditoriaService oAuditoria = new EbsAuditoriaService();
            oAuditoria.Url = ConfigurationManager.AppSettings["consRutaWSSeguridad"];
            oAuditoria.Credentials = System.Net.CredentialCache.DefaultCredentials;

            OpcionesUsuarioRequest OpcionesUsuarioRequest = new OpcionesUsuarioRequest();
            OpcionesUsuarioResponse OpcionesUsuarioResponse = new OpcionesUsuarioResponse();

            OpcionesUsuarioRequest.usuario = Funciones.CheckStr(idUsuario);
            OpcionesUsuarioRequest.aplicacion = Funciones.CheckStr(ConfigurationManager.AppSettings["CodigoAplicacion"].ToString());

            OpcionesUsuarioResponse = oAuditoria.leerOpcionesPorUsuario(OpcionesUsuarioRequest);

            return OpcionesUsuarioResponse;
        }

    }
}
