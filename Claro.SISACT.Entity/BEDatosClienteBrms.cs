//PROY-32439 MAS INI
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126
    public class BEDatosClienteBrms
    {
        private string _DAVCV_VAL_CLIENTE = string.Empty;
        private string _DAVCV_RESTR_PRODCOM = string.Empty;
        private string _DAVCV_RESTR_PROD = string.Empty;
        private string _DAVCV_RESTR_OPEN_CODIGO = string.Empty;
        private string _DAVCV_MSJ_VALCLIENTE = string.Empty;
        private string _DAVCV_MENSAJEWS = string.Empty;
        private string _DAVCV_IN_SOLICITUD = string.Empty;
        private string _DAVCV_IN_PDV = string.Empty;
        private string _DAVCV_IN_LINEA = string.Empty;
        private string _DAVCV_IN_CLIENTE = string.Empty;
        private string _DAVCN_SOLIN_GRUPO_SEC = string.Empty;
        private string _DAVCN_SOLIN_CODIGO = string.Empty;
        private string _DAVCD_FEC_REG = string.Empty;

        public string DAVCV_VAL_CLIENTE
        {
            get{return this._DAVCV_VAL_CLIENTE;}
            set{this._DAVCV_VAL_CLIENTE = value;}
        }
        public string DAVCV_RESTR_PRODCOM
        {
            get { return this._DAVCV_RESTR_PRODCOM; }
            set { this._DAVCV_RESTR_PRODCOM = value; }
        }
        public string DAVCV_RESTR_PROD
        {
            get { return this._DAVCV_RESTR_PROD; }
            set { this._DAVCV_RESTR_PROD = value; }
        }
        public string DAVCV_RESTR_OPEN_CODIGO
        {
            get { return this._DAVCV_RESTR_OPEN_CODIGO; }
            set { this._DAVCV_RESTR_OPEN_CODIGO = value; }
        }
        public string DAVCV_MSJ_VALCLIENTE
        {
            get { return this._DAVCV_MSJ_VALCLIENTE; }
            set { this._DAVCV_MSJ_VALCLIENTE = value; }
        }
        public string DAVCV_MENSAJEWS
        {
            get { return this._DAVCV_MENSAJEWS; }
            set { this._DAVCV_MENSAJEWS = value; }
        }
        public string DAVCV_IN_SOLICITUD
        {
            get { return this._DAVCV_IN_SOLICITUD; }
            set { this._DAVCV_IN_SOLICITUD = value; }
        }
        public string DAVCV_IN_PDV
        {
            get { return this._DAVCV_IN_PDV; }
            set { this._DAVCV_IN_PDV = value; }
        }
        public string DAVCV_IN_LINEA
        {
            get { return this._DAVCV_IN_LINEA; }
            set { this._DAVCV_IN_LINEA = value; }
        }
        public string DAVCV_IN_CLIENTE
        {
            get { return this._DAVCV_IN_CLIENTE; }
            set { this._DAVCV_IN_CLIENTE = value; }
        }
        public string DAVCN_SOLIN_GRUPO_SEC
        {
            get { return this._DAVCN_SOLIN_GRUPO_SEC; }
            set { this._DAVCN_SOLIN_GRUPO_SEC = value; }
        }
        public string DAVCN_SOLIN_CODIGO
        {
            get { return this._DAVCN_SOLIN_CODIGO; }
            set { this._DAVCN_SOLIN_CODIGO = value; }
        }
        public string DAVCD_FEC_REG
        {
            get { return this._DAVCD_FEC_REG; }
            set { this._DAVCD_FEC_REG = value; }
        }
    }

}
//PROY-32439 MAS FIN