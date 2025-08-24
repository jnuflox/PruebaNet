//INICIO PROY-140419 Autorizar Portabilidad sin PIN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.WS.ConsultaSeguridad;
using System.Configuration;
using System.Collections;

namespace Claro.SISACT.WS
{
    public class BWConsultaSeguridad
    {
        ConsultaSeguridad.ConsultaSeguridad _objTransaccion = new ConsultaSeguridad.ConsultaSeguridad();

        public BWConsultaSeguridad()
        {
            _objTransaccion.Url = ConfigurationManager.AppSettings["strWebServiceDBAUDIT"].ToString();
            _objTransaccion.Credentials = System.Net.CredentialCache.DefaultCredentials;
            _objTransaccion.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["TimeoutWS"].ToString());
        }

        public List<BEConsultaSeguridad> verificaUsuario(ref string idTrans, string IpAplicacion, string Aplicacion, string usuario, Int64 appCod, ref string errorMsg, ref string codError)
        {
            List<BEConsultaSeguridad> lista = new List<BEConsultaSeguridad>();
            try
            {
                ConsultaSeguridad.seguridadType[] objSeg;
                codError = _objTransaccion.verificaUsuario(ref idTrans, IpAplicacion, Aplicacion, usuario, appCod, out errorMsg, out objSeg);

                if (objSeg != null)
                {
                    for (int i = 0; i < objSeg.Length; i++)
                    {
                        BEConsultaSeguridad item = new BEConsultaSeguridad();
                        item.USUACCOD = objSeg[i].UsuacCod;
                        item.PERFCCOD = objSeg[i].PerfcCod;
                        item.USUACCODVENSAP = objSeg[i].UsuacCodVenSap;
                        lista.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message.ToString();
            }
            return lista;
        }
    }
}
//FIN PROY-140419 Autorizar Portabilidad sin PIN