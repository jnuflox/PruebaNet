using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Claro.SISACT.Entity;
using Claro.SISACT.WS.WSLogin;
using Claro.SISACT.Common;

namespace Claro.SISACT.WS
{
    public class BWLogin
    {
        EbsDatosEmpleadoService _objTransaccion = new EbsDatosEmpleadoService();

        public BWLogin()
		{
            _objTransaccion.Url = ConfigurationManager.AppSettings["urlWS"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["time"].ToString());
		}

        public BEUsuarioSession DatosEmpleado(Int64 pCodUsuario, string pCuentaRed)
        {
            BEUsuarioSession objUsuario = new BEUsuarioSession();
            try
            {
                DatosEmpleadoRequest objRequest = new DatosEmpleadoRequest();
                DatosEmpleadoResponse objResponse = new DatosEmpleadoResponse();

                objRequest.idUsu = Funciones.CheckStr(pCodUsuario);
                objRequest.loginNt = pCuentaRed;

                objResponse = _objTransaccion.obtenerDatosEmpleadoPorId(objRequest);

                if (objResponse.codRes.Equals("0"))
                {
                    objUsuario = new BEUsuarioSession();
                    objUsuario.idUsuario = Funciones.CheckInt64(objResponse.empleado.idEmp);
                    objUsuario.Login = objResponse.empleado.login;
                    objUsuario.Nombre = objResponse.empleado.nombre;
                    objUsuario.Apellido_Pat = objResponse.empleado.apellido;
                    objUsuario.Apellido_Mat = objResponse.empleado.apellidoMaterno;
                    objUsuario.NombreCompleto = objResponse.empleado.nomCompleto;
                    objUsuario.idArea = objResponse.empleado.idArea;
                    objUsuario.Area = objResponse.empleado.descArea;
                    objUsuario.idVendedorSap = objResponse.empleado.idCodvendedorSap;
                }

                return objUsuario;
            }
            finally
            {
                _objTransaccion.Dispose();
            }
           
        }
    }
}
