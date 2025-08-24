using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPlan_AP
    {
        private int _PAQPN_SECUENCIA;

        private string _PLNV_CODIGO;
        private string _PLNV_DESCRIPCION;
        private string _PLNC_ESTADO;
        private string _TVENC_CODIGO;
        private string _TPROC_CODIGO;
        private int _PLNN_TIPO_PLAN;
        private double _PLNN_CARGO_FIJO;
        private double _PAQPN_CARGO_FIJO;
        private DateTime _PLND_FECHA_CREA;
        private string _PLNV_USUARIO_CREA;

        private BEPaquete_AP _PAQUETE;
        private ArrayList _SERVICIOS;
        //JAR
        private ArrayList _KITS;

        private string _PLANC_CORREOCLIENTEOBLIGA;
        private string _TCTRL_CONSUMO;
        private string _PACUC_CODIGO;
        private string _PACUC_DESCRIPCION;

        public int PAQPN_SECUENCIA { get { return _PAQPN_SECUENCIA; } set { _PAQPN_SECUENCIA = value; } }

        public string PLNV_CODIGO { get { return _PLNV_CODIGO; } set { _PLNV_CODIGO = value; } }
        public string PLNV_DESCRIPCION { get { return _PLNV_DESCRIPCION; } set { _PLNV_DESCRIPCION = value; } }
        public string PLNC_ESTADO { get { return _PLNC_ESTADO; } set { _PLNC_ESTADO = value; } }
        public string TVENC_CODIGO { get { return _TVENC_CODIGO; } set { _TVENC_CODIGO = value; } }
        public string TPROC_CODIGO { get { return _TPROC_CODIGO; } set { _TPROC_CODIGO = value; } }
        public int PLNN_TIPO_PLAN { get { return _PLNN_TIPO_PLAN; } set { _PLNN_TIPO_PLAN = value; } }
        public double CARGO_FIJO_BASE { get { return _PLNN_CARGO_FIJO; } set { _PLNN_CARGO_FIJO = value; } }
        public double CARGO_FIJO_EN_PAQUETE { get { return _PAQPN_CARGO_FIJO; } set { _PAQPN_CARGO_FIJO = value; } }
        public DateTime PLND_FECHA_CREA { get { return _PLND_FECHA_CREA; } set { _PLND_FECHA_CREA = value; } }
        public string PLNV_USUARIO_CREA { get { return _PLNV_USUARIO_CREA; } set { _PLNV_USUARIO_CREA = value; } }

        public BEPaquete_AP PAQUETE { get { return _PAQUETE; } set { _PAQUETE = value; } }
        public ArrayList SERVICIOS { get { return _SERVICIOS; } set { _SERVICIOS = value; } }

        public string PLANC_CORREOCLIENTEOBLIGA { get { return _PLANC_CORREOCLIENTEOBLIGA; } set { _PLANC_CORREOCLIENTEOBLIGA = value; } }
        public string TCTRL_CONSUMO { get { return _TCTRL_CONSUMO; } set { _TCTRL_CONSUMO = value; } }
        public string PACUC_CODIGO { get { return _PACUC_CODIGO; } set { _PACUC_CODIGO = value; } }
        public string PACUC_DESCRIPCION { get { return _PACUC_DESCRIPCION; } set { _PACUC_DESCRIPCION = value; } }

        public enum TIPO_PLAN
        {
            VOZ = 1,
            DATOS = 2,
            EXCEPCION = 9
        }
        public double CARGO_FIJO_TOTAL
        {
            get
            {
                double CFtotal = 0;
                if (PAQUETE.PAQTV_CODIGO != string.Empty && PAQUETE.PAQTV_CODIGO != "00")
                {
                    CFtotal += CARGO_FIJO_EN_PAQUETE;
                }
                else
                {
                    CFtotal += CARGO_FIJO_BASE;
                }

                foreach (BEServicio_AP oServ in SERVICIOS)
                {
                    Int64 SELECCIONABLE_EN_PAQUETE;
                    Int64.TryParse(oServ.SELECCIONABLE_EN_PAQUETE, out SELECCIONABLE_EN_PAQUETE);
                    Int64 SELECCIONADO;
                    Int64.TryParse(SERVICIOS_SELECCIONABLE.SELECCIONADO.ToString(), out SELECCIONADO);
                    Int64 OBLIGATORIO;
                    Int64.TryParse(SERVICIOS_SELECCIONABLE.OBLIGATORIO.ToString(), out OBLIGATORIO);
                    Int64 SELECCIONABLE_EN_PLAN;
                    Int64.TryParse(oServ.SELECCIONABLE_EN_PLAN, out SELECCIONABLE_EN_PLAN);

                    if (PAQUETE.PAQTV_CODIGO != string.Empty && PAQUETE.PAQTV_CODIGO != "00")
                    {
                        if ((SELECCIONABLE_EN_PAQUETE & (SELECCIONADO | OBLIGATORIO)) != 0)
                        {
                            CFtotal += oServ.CARGO_FIJO_EN_PAQUETE;
                        }
                    }
                    else
                    {
                        if ((SELECCIONABLE_EN_PLAN & (SELECCIONADO | OBLIGATORIO)) != 0)
                        {
                            CFtotal += oServ.CARGO_FIJO_BASE - oServ.DESCUENTO_EN_PLAN;
                        }
                    }
                }
                return CFtotal;
            }
        }

        public int PLANES_TOTAL
        {

            get
            {
                int TOT_TIPO_PLAN = 0;

                if (PAQUETE.PAQTV_CODIGO == string.Empty && PAQUETE.PAQTV_CODIGO == "00" || PAQUETE.PAQTV_CODIGO == "")
                {
                    TOT_TIPO_PLAN += TOT_TIPO_PLAN + 1;
                }

                return TOT_TIPO_PLAN;
            }
        }

        public int PLANES_TOT_DATOS
        {

            get
            {
                int TOT_PLAN_DATOS = 0;

                if (PAQUETE.PAQTV_CODIGO == string.Empty && PAQUETE.PAQTV_CODIGO == "00" || PAQUETE.PAQTV_CODIGO == "")
                {
                    //					if ((bool) TIPO_PLAN.DATOS == true)
                    if (PLNN_TIPO_PLAN == 2)
                    {
                        TOT_PLAN_DATOS += TOT_PLAN_DATOS + 1;
                    }
                }

                return TOT_PLAN_DATOS;
            }
        }
        //
        public int PLANES_TOT_VOZ
        {

            get
            {
                int TOT_PLAN_VOZ = 0;

                if (PAQUETE.PAQTV_CODIGO == string.Empty && PAQUETE.PAQTV_CODIGO == "00" || PAQUETE.PAQTV_CODIGO == "")
                {
                    //				if ((bool) TIPO_PLAN.VOZ  == true)
                    if (PLNN_TIPO_PLAN == 1)
                    {
                        TOT_PLAN_VOZ += TOT_PLAN_VOZ + 1;
                    }
                }

                return TOT_PLAN_VOZ;
            }
        }

        //		public int PAQUETES_TOTAL
        //		{
        //			
        //			get
        //			{
        //				int TOT_TIPO_PAQ = 0;
        //				
        //				
        //				if (PAQUETE.PAQTV_CODIGO != string.Empty && PAQUETE.PAQTV_CODIGO != "00")
        //				
        //				{
        //		
        //							TOT_TIPO_PAQ += TOT_TIPO_PAQ + 1;
        //				}	
        //		
        //				return TOT_TIPO_PAQ;
        //			}
        //		}
        //JAR
        public ArrayList KITS { get { return _KITS; } set { _KITS = value; } }

        public BEPlan_AP()
        {
            PLNV_CODIGO = string.Empty;
            PLNV_DESCRIPCION = string.Empty;
            PLNC_ESTADO = string.Empty;
            TVENC_CODIGO = string.Empty;
            TPROC_CODIGO = string.Empty;
            CARGO_FIJO_BASE = 0;
            CARGO_FIJO_EN_PAQUETE = 0;
            PLND_FECHA_CREA = new DateTime();
            PLNV_USUARIO_CREA = string.Empty;

            PAQUETE = new BEPaquete_AP();
            SERVICIOS = new ArrayList();

            PLANC_CORREOCLIENTEOBLIGA = string.Empty;
            PACUC_CODIGO = string.Empty;
            PACUC_DESCRIPCION = string.Empty;
            //JAR
            KITS = new ArrayList();
        }
    }
}
