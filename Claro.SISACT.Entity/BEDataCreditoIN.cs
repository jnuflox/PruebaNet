using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEDataCreditoIN
    {
        public BEDataCreditoIN() { }

        public string istrSecuencia { get; set; }
        public string istrTipoDocumento { get; set; }
        public string istrNumeroDocumento { get; set; }
        public string istrAPELLIDOPATERNO { get; set; }
        public string istrAPELLIDOMATERNO { get; set; }
        public string istrNOMBRES { get; set; }
        public int istrDatoEntrada { get; set; }
        public string istrDatoComplemento { get; set; }
        public string istrTIPOPRODUCTO { get; set; }
        public string istrTIPOCLIENTE { get; set; }
        public string istrEDAD { get; set; }
        public string istrIngresoOLineaCredito { get; set; }
        public string istrTIPOTARJETA { get; set; }
        public string istrRUC { get; set; }
        public string istrANTIGUEDADLABORAL { get; set; }
        public string istrNumOperaPVU { get; set; }
        public string istrRegion { get; set; }
        public string istrArea { get; set; }
        public string istrCanal { get; set; }
        public string istrPuntoVenta { get; set; }
        public string istrIDCanal { get; set; }
        public string istrIDTerminal { get; set; }
        public string istrUsuarioACC { get; set; }
        public string ostrNumOperaEFT { get; set; }
        public string istrNUMCUENTAS { get; set; }
        public string istrEstadoCivil { get; set; }

        public enum EXPERTO
        {
            DATACREDITO = 1,
            EQUIFAX = 2
        }

        //PROY-24740
        public String toString()
        {
            return string.Format("{0}{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}",
                                String.Empty,
                                istrSecuencia,
                                istrTipoDocumento,
                                istrNumeroDocumento,
                                istrAPELLIDOPATERNO,
                                istrAPELLIDOMATERNO,
                                istrNOMBRES,
                                istrDatoEntrada,
                                istrDatoComplemento,
                                istrTIPOPRODUCTO,
                                istrTIPOCLIENTE,
                                istrEDAD,
                                istrIngresoOLineaCredito,
                                istrTIPOTARJETA,
                                istrRUC,
                                istrANTIGUEDADLABORAL,
                                istrNumOperaPVU,
                                istrRegion,
                                istrArea,
                                istrPuntoVenta,
                                istrIDTerminal,
                                istrUsuarioACC,
                                ostrNumOperaEFT,
                                istrNUMCUENTAS,
                                istrEstadoCivil);
        }
    }
}
