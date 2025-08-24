using System;
using System.Collections;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPlan
    {
        public BEPlan() { }

        public BEPlan(int vPLANN_CODIGO, string vPLANV_DESCRIPCION)
        {
            PLANN_CODIGO = vPLANN_CODIGO;
            PLANV_DESCRIPCION = vPLANV_DESCRIPCION;
        }

        public int PLANN_CODIGO { get; set; }
        public string PLANC_CODIGO { get; set; }
        public string PLNV_CODIGO { get; set; }
        public string TPROC_CODIGO { get; set; }
        public string TVENC_CODIGO { get; set; }
        public string PLANV_DESCRIPCION { get; set; }
        public string PLANV_UNIDAD_TOPE { get; set; }
        public int ID_BSCS { get; set; }
        public string PLANC_ESTADO { get; set; }
        public string TPROV_DESCRIPCION { get; set; }
        public string TVENV_DESCRIPCION { get; set; }
        public double PLANN_CAR_FIJ { get; set; }
        public double PLANN_LIM_CRED_CON { get; set; }
        public string PLANC_SCO_TXT_CON { get; set; }
        public int PLANN_SCO_NUM_CON { get; set; }
        public string PLANC_TIPO { get; set; }
        public string PLANC_TIPO_REGLA { get; set; }
        public double CARGO_PLAN { get; set; }
        public string CODIGO_BSCS { get; set; }
        public string PLANC_EQUI_SAP { get; set; }
        public int PLNN_TIPO_PLAN { get; set; }
        public string GPLNV_DESCRIPCION { get; set; }
        public string TIPO_PRODUCTOS { get; set; }

        public string SERV_CODIGO { get; set; }
        public string SERV_DESCRIPCION { get; set; }
        public string PRDC_CODIGO { get; set; }
        public string CMBV_CODIGO { get; set; }

        //Para Gama de Planes - INICIO
        public int CompareTo(object obj)
        {
            BEPlan Compare = (BEPlan)obj;
            int result = this.PLANN_CAR_FIJ.CompareTo(Compare.PLANN_CAR_FIJ);
            if (result == 0)
                result = this.PLANV_DESCRIPCION.CompareTo(Compare.PLANV_DESCRIPCION);
            return result;
        }
        //Para Gama de Planes - FIN

        public string TCLIC_CODIGO { get; set; }
        public string TCLIV_DESCRIPCION { get; set; }
        public int ID_SAP { get; set; }
        public string CODIGO_SAP { get; set; }
        public string CODIGO_MATRIZ_REGLA { get; set; }
        public string CODIGO_AUTONOMIA { get; set; }
        public string CODIGO_EXCLUYENTE { get; set; }
        public int CANTIDAD_MINIMA_MATRIZ { get; set; }
        public int CANTIDAD_MAXIMA_MATRIZ { get; set; }
        public int PLANN_MAX_NUM { get; set; }
        public decimal TCARC_CODIGO { get; set; }
        public decimal TCARN_NUM_CAR_FIJ { get; set; }
        public decimal MONTN_MONTO_FLAT { get; set; }
        public int FAM_PLANN_CODIGO { get; set; }
        public int CANTIDAD { get; set; }
        public ArrayList SERVICIOS { get; set; }


        //INICIO PROY-30748
        private decimal _PrecionVenta;
        public decimal PrecionVenta { set { _PrecionVenta = value; } get { return _PrecionVenta; } }
        //FIN PROY-30748
    }
}
