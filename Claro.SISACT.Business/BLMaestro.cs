using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;

namespace Claro.SISACT.Business
{
    public class BLMaestro
    {
        public Hashtable ListarItemsGenericos(int[] tablas)
        {
            DAMaestro obj = new DAMaestro();
            return obj.ListarItemsGenericos(tablas);
        }

        public ArrayList ListaDepartamento(string cod_dpto, string estado)
        {
            DAMaestro obj = new DAMaestro();
            return obj.ListaDepartamento(cod_dpto, estado);
        }
        
        public ArrayList ListaProvincia(string cod_provincia, string cod_dpto, string estado)
        {
            DAMaestro obj = new DAMaestro();
            return obj.ListaProvincia(cod_provincia, cod_dpto, estado);
        }
        
        public ArrayList ListaDistrito(string cod_distrio, string cod_provincia, string cod_dpto, string estado)
        {
            DAMaestro obj = new DAMaestro();
            return obj.ListaDistrito(cod_distrio, cod_provincia, cod_dpto, estado);
        }
        
        public ArrayList ListaParametros(Int64 codigo)
        {
            DAMaestro obj = new DAMaestro();
            return obj.ListaParametros(codigo);
        }

        public DataTable ObtenerDatosRegistroTiempo(Int64 nroSEC)
        {
            DAMaestro obj = new DAMaestro();
            return obj.ObtenerDatosRegistroTiempo(nroSEC);
        }

        public bool RegistroTiempoPoolEval(Int64 nroSEC, string pCodpdv, string pCodcanal, string pUsupdv, string pAnaeval, string pFech_inievasec, string pFech_finevasec, string pFlag_revasec)
        {
            DAMaestro obj = new DAMaestro();
            return obj.RegistroTiempoPoolEval(nroSEC, pCodpdv, pCodcanal, pUsupdv, pAnaeval, pFech_inievasec, pFech_finevasec, pFlag_revasec);
        }

        public bool RegistroTiempoActivaIni(Int64 nroSEC, string pCodpdv, string pCodcanal, string pUsupdv, string pFech_iniactsec)
        {
            DAMaestro obj = new DAMaestro();
            return obj.RegistroTiempoActivaIni(nroSEC, pCodpdv, pCodcanal, pUsupdv, pFech_iniactsec);
        }

        public static List<BEItemGenerico> ListaTipoDocumento(string pruc)
        {
            return DAMaestro.ListTipoDocumento(pruc);
        }
        //PROY-29121-INI
        public List<BEItemGenerico> ListaParametroslst(Int64 codigo)
        {
            DAMaestro obj = new DAMaestro();
            return obj.ListaParametroslst(codigo);
        }
        //PROY-29121-FIN
        //INI PROY-31948_Migracion
        public static bool GrabarRegHorasPoll(Int64 pCodsec, string pAnaeval, string pFech_finevasec, string pOperacion)
        {
            return DAMaestro.GrabarRegHorasPoll(pCodsec, pAnaeval, pFech_finevasec, pOperacion);
        }
        public static BEUsuario ObtenerUsuario(Int64 cod_usuario)
        {
            return DAMaestro.ObtenerUsuario(cod_usuario);
        }
        //FIN PROY-31948_Migracion

        //Inicio Proy-140397 LVR
        public static BEValidarMultipunto ListaValidarMultipunto(string cod_canal)
        {
            return new DAMaestro().ListaValidarMultipunto(cod_canal);
        }
        //Fin Proy-140397 LVR

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil] | [Metodo para consultar las campañas existentes para las ventas varias]
        public static List<BEItemGenerico> ConsultaCampanaXTipoVenta(BEParametrosMSSAP oParamMSSAP)
        {
            return new DAMaestro().ConsultaCampanaXTipoVenta(oParamMSSAP);
        }
        #endregion
    }
}
