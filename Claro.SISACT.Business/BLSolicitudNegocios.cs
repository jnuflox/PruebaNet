using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Claro.SISACT.Entity; 
using Claro.SISACT.Data; 
using System.Data;

namespace Claro.SISACT.Business
{
    public class BLSolicitudNegocios
    {
        public ArrayList ObtenerConsultaSolicitudCons(string nroSEC, string tipoDocumento, string nroDocumento)
        {
            DASolicitudDatos obj = new DASolicitudDatos();
            return obj.ObtenerConsultaSolicitudCons(nroSEC, tipoDocumento, nroDocumento);
        }

        public bool ModificacionEvaluacionNaturalConsumer(BEVistaSolicitud objVistaSolicitud)
        {
            return new DASolicitudDatos().ModificacionEvaluacionNaturalConsumer(objVistaSolicitud);
        }

        public bool InsertarSolDireccion(BEDireccionCliente oDireccion, Int64 nroSEC)
        {
            return new DASolicitudDatos().InsertarSolDireccion(oDireccion, nroSEC);
        }

        public ArrayList ListarPromociones(string pstrSoplnCodigo)
        {
            return new DASolicitudDatos().ListarPromociones(pstrSoplnCodigo);
        }
//gaa20140414
        public List<BEDireccionCliente> ConsultarSolDireccion(Int64 nroSEC)
        {
            return new DASolicitudDatos().ConsultarSolDireccion(nroSEC);
        }
//fin gaa20140414
        public string ValidarVendedorDNI(string pstrNroDocumento)
        {
            DASolicitudDatos obj = new DASolicitudDatos();
            return obj.ValidarVendedorDNI(pstrNroDocumento);
        }

        public ArrayList ObtenerPlanesCliente(int tipoDoc, string numeroDoc)
        {
            return new DASolicitudDatos().ObtenerPlanesCliente(tipoDoc, numeroDoc);
        }
//gaa20140414
        public DataTable ListarEdificio(string pstrCodPlano, string pstrCodEdificio)
        {
            DASolicitudDatos obj = new DASolicitudDatos();
            return obj.ListarEdificio(pstrCodPlano, pstrCodEdificio);
        }

        public List<BEItemGenerico> ListarEquiposHFCxSopln(Int64 pintSoplnCodigo)
        {
            DASolicitudDatos obj = new DASolicitudDatos();
            return obj.ListarEquiposHFCxSopln(pintSoplnCodigo);
        }
//fin gaa20140414


        // Inicio - ConsultaDeclaracionConocimiento - Negocio - Evalenzs
        public ArrayList ConsultaDeclaracionConocimiento(String SegmentoVenta)
        {
            DASolicitudDatos obj = new DASolicitudDatos();
            return obj.ConsultaDeclaracionConocimiento(SegmentoVenta);
        }
        //Fin - Evalenzs

        // Inicio - IngresaDeclaracionConocimiento - Negocio - Evalenzs
        public bool IngresaDeclaracionConocimiento(int SolinCodigo, String P_flagItem, String SegmentoVenta, String Usuario)
        {
            DASolicitudDatos obj = new DASolicitudDatos();
            return obj.IngresaDeclaracionConocimiento(SolinCodigo, P_flagItem, SegmentoVenta, Usuario);
        }
        //Fin - Evalenzs

        //PROY-140690 INICIO
        public string ConsultarSolDireccionIfi(string tipoDocumento, string nroDocumento, ref string codRpta, ref string msgRpta)
        {
            return new DASolicitudDatos().ConsultarSolDireccionIfi(tipoDocumento, nroDocumento, ref codRpta, ref msgRpta);
        }
        //PROY-140690 FIN

        //[PROY-140600] INI 
        public static void GrabarInfoContLineasTipi(BEValidarLinea objLineasTipi, ref string codRpta, ref string msjRpta)
        {
            new DASolicitudDatos().GrabarInfoContLineasTipi(objLineasTipi, ref codRpta, ref msjRpta);
        }
            //[PROY-140600] INI 
    }
}
