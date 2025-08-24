using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Claro.SISACT.Data
{
    class BaseDatos
    {
        static BaseDatos()
        {
            BdSisact = "BD_SISACT";
            BdAudit = "BD_DBAUDIT";
            BdPel = "BD_PEL";
            BdPel2 = "BD_PEL2";
            BdSiscad = "BD_SISCAD";
            BdClarify = "BD_CLARIFY";
            BdSiac = "BD_SIAC";
            BdPuntosCC = "BD_PUNTOSCC";
            BdOds = "BD_ODS";
            BdBscs = "BD_BSCS";
            BdSga = "BD_SGA";
            BdDWH ="BD_DWH";
            BdMSSAP = "BD_MSSAP";
        }

        public static string BdSiscad { get; private set; }
        public static string BdPel { get; private set; }
        public static string BdPel2 { get; private set; }
        public static string BdAudit { get; private set; }
        public static string BdSisact { get; private set; }
        public static string BdClarify { get; private set; }
        public static string BdSiac { get; private set; }
        public static string BdOds { get; private set; }
        public static string BdPuntosCC { get; private set; }
        public static string BdBscs { get; private set; }
        public static string BdSga { get; private set; }
        public static string BdDWH { get; private set; }
        public static string BdMSSAP { get; private set; }
                   
        public static string PkgSisactConsultas
        {
            get
            {
                string p = "SISACT_PKG_CONSULTA_GENERAL";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }

        public static string PkgCustomerClfy
        {
            get
            {
                string p = "PCK_CUSTOMER_CLFY";
                string esquema = ConfigurationManager.AppSettings["EsquemaCLF"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }
        public static string PkgSisactVentasExpress
        {
            get
            {
                string p = "SISACT_PKG_VENTA_EXPRESS_6"; 
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"]; 
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }

        public static string PkgExpressPortabilidad
        {
            get
            {
                string p = "SISACT_PKG_EXPRESS_PORTA_";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }

        public static string PkgSecpMaestros
        {
            get
            {
                string p = "SECP_PKG_MAESTROS";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }

        public static string PkgSisactSolicitud
        {
            get
            {
                string p = "SISACT_PKG_SOLICITUD";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }

        public static string PkgParametrico
        {
            get
            {
                string p = "PKG_PARAMETRICO";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaBSCS"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string Tim127CompPago
        {
            get
            {
                string p = "TIM127_COMP_PAGO";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaBSCS"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PqConsultaSiacSrv
        {
            get
            {
                string p = "PQ_CONSULTA_SIAC_SRV";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["Esquema_SGA2"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_EVALUACION
        {
            get
            {
                string p = "SISACT_PKG_EVALUACION";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
		public static string PKG_SISACT_EVALUACION_CONS_2
		{
			get
			{
				string p = "SISACT_PKG_EVALUACION_CONS_2_";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
				if ( esquema != null && esquema != "") p = esquema  + "." + p;
				return p ;
			}
		}

		//INICIO IMP SD_866068
        public static string PKG_SISACT_EVALUACION_CONS
        {
            get
            {
                //string p = "SISACT_PKG_EVALUACION_CONS_2";
                string p = "Sisact_Pkg_Eval_Cons_2_140743";//MBC REPLICA
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
		//FIN IMP SD_866068

        public static string PKG_SISACT_EVALUACION_UNI
        {
            get
            {
                string p = "SISACT_PKG_EVALUACION_UNI_";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_CONSULTA_GENNERAL
        {
            get
            {
                string p = "SISACT_PKG_CONSULTA_GENERAL";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_GENERAL
        {
            get
            {
                //string p = "SISACT_PKG_GENERAL_3PLAY_6";
                string p = "SISACT_PKG_GRAL_3PLAY_6_140743";//MBC REPLICA
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //gaa20161127
        public static string PKG_SISACT_GENERAL_
        {
            get
            {
                string p = "SISACT_PKG_GENERAL";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //fin gaa20161127
        public static string PKG_SISACT_CONSULTA_BRMS
        {
            get
            {
                //string p = "SISACT_PKG_CONSULTA_BRMS"; //PROY-24724-IDEA-28174
                string p = "SISACT_PKG_CONSULTA_BRMS140743";//MBC REPLICA
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_DTH
        {
            get
            {
                string p = "SISACT_PKG_DTH_3PLAY_6";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_HISTORICO_DC
        {
            get
            {
                string p = "SISACT_PKG_HISTORICO_DC";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_BURO_CREDITICIO
        {
            get
            {
                string p = "SISACT_PKG_BURO_CREDITICIO";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_SOLICITUD_CONS
        {
            get
            {
                //string p = "SISACT_PKG_SOLICITUD_CONS";
                string p = "PKG_SOLICITUD_CONS_140743";//PKG REPLICA
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_CONSULTAS
        {
            get
            {
                string p = "SISACT_PKG_CONSULTA_GENERAL";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_MAESTROS
        {
            get
            {
                string p = "SISACT_PKG_MAESTROS_6";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;

            }
        }

        public static string PKG_SISACT_GENERAL_II
        {
            get
            {
                string p = "SISACT_PKG_GENERAL_II";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string SISACT_PKG_DRA_CVE
        {
            get
            {
                string p = "SISACT_PKG_DRA_CVE_6";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;

            }
        }

        public static string TIM098_LISTA_PLAN_TC
        {
            get
            {
                string p = "TIM098_LISTA_PLAN_TC";
                string esquema = ConfigurationManager.AppSettings["EsquemaBSCS"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PQ_INT_SISACT_CONSULTA
        {
            get
            {
                string p = "PQ_INT_SISACT_CONSULTA";
                string esquema = ConfigurationManager.AppSettings["Esquema_SGA2"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_PORTABILIDAD
        {
            get
            {
                string p = "SISACT_PKG_PORTABILIDAD_6";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_PORTABILIDAD_CORP
        {
            get
            {
                string p = "SISACT_PKG_PORTABILIDAD_CORP";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_ROAMING_SERV
        {
            get
            {
                string p = "SISACT_PKG_ROAMING_SERV_";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PQ_INTEGRACION_DTH
        {
            get
            {
                string p = "PQ_INTEGRACION_DTH";
                string esquema = ConfigurationManager.AppSettings["EsquemaSGA"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string TIM142_PKG_EMPRESAS
        {
            get
            {
                string p = "TIM142_PKG_EMPRESAS";
                string esquema = ConfigurationManager.AppSettings["EsquemaBSCS"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_REGLAS
        {
            get
            {
                string p = "SISACT_PKG_REGLAS";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_DWH_ABONADOS_CLARO
        {
            get
            {
                string p = "pkg_abonados_claro";
                string esquema = ConfigurationManager.AppSettings["EsquemaDWH"].ToString();
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SIAC_GENERICO
        {
            get
            {
                string p = "pkg_siac_generico";
                string esquema = ConfigurationManager.AppSettings["EsquemaSIAC"].ToString();
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_EVALUACION_SEC
        {
            get
            {
                string p = "SISACT_PKG_EVALUACION_SEC";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"].ToString();
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_TATENCION
        {
            get
            {
                string p = "SISACT_PKG_TATENCION";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"].ToString();
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;

            }
        }

        public static string pq_migracion
        {
            get
            {
                string p = "pq_migracion";
                string esquema = ConfigurationManager.AppSettings["Esquema_SGA2"].ToString();
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;

            }
        }
        public static string SISACT_PKG_ACUERDO
        {
            get
            {
              string p = "SISACT_PKG_ACUERDO_6";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"].ToString();
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;

            }
        } 
        public static string PQ_CONVERGENTE
        {
            get
            {
                string p = "pq_convergente";
                string esquema = ConfigurationManager.AppSettings["Esquema_SGA2"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        public static string PkgSisactVentasExpress3Play
        {
            get
            {
                string p = "SISACT_PKG_VENTA_EXP_3PLAY_6";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }

        //11-10-2014
        public static string PKG_NUEVA_LISTAPRECIOS
        {
            get
            {
                string p = "SISACT_PKG_NUEVA_LISTAPRECIOS";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"].ToString();
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }


        //SinergiaSAP

        public static string PKG_CONSULTA
        {
            get
            {
                //string p = "PKG_CONSULTA";
                string p = "PKG_CONSULTA_140743";//MBC REPLICA
                string esquema = ConfigurationManager.AppSettings["EsquemaSinergiaSap"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }

        public static string PkgConsMaestraSap
        {
            get
            {
                string p = "SISACT_PKG_CONS_MAESTRA_SAP_6";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_CC_TRANSACCION
        {
            get
            {
                string p = "PKG_CC_TRANSACCION";
                string esquema = ConfigurationManager.AppSettings["EsquemaCLAROCLUB"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        public static string PKG_MATERIAL_LISTA
        {
            get
            {
                string p = "PKG_MATERIAL_LISTA";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }

        // PROY-26358 - Inicio - SISACT_PKG_DECLARA_PORTA - Evalenzs
        public static string SISACT_PKG_DECLARA_PORTA
        {
            get
            {
                string p = "PKG_SISACT_DECLARA_PORTA";							
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        // PROY-26358 -Fin - Evalenzs

	public static string SISACT_PKG_NUEVA_LISTAPRE_6 //PROY-24724-IDEA-28174 - INICIO
        {
            get
            {
                string p = "SISACT_PKG_NUEVA_LISTAPRE_6";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        }

        public static string SISACT_PKG_TRANS_ASURION
        {
            get
            {
                string p = "PKG_TRANS_ASURION";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (!string.IsNullOrEmpty(esquema)) p = esquema + "." + p;
                return p;
            }
        } //PROY-24724-IDEA-28174 - FIN
        
        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        public static string PKG_PORTABILIDAD_MIGRA
        {
            get
            {
                string p = "SISACT_PKG_PORTABILIDAD_MIGRA";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

        //PROY-25335 - Inicio
        public static string PKG_SISACT_CARTA_PODER
        {
            get
            {
                string p = "PKG_SISACT_CARTA_PODER";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_SISACT_REPRESENTANTE_LEGAL
        {
            get
            {
                string p = "PKG_SISACT_REPRESENTANTE_LEGAL";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //PROY-25335 FIN
        public static string PKG_SISACT_PORTA_VALIDA
        {
            get
            {
                string p = "PKG_PORTABILIDAD_PREP_26963";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //PROY-32129 :: INI
        public static string PKG_SISACT_CAMPANA_ESPECIAL
        {
            get
            {
                string p = "PKG_SISACT_CAMPANA_ESPECIAL";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //PROY-32129 :: FIN

        //INI PROY-CAMPANA LG
        public static string PKG_SISACT_PORTA_CAMP_LG
        {
            get
            {
                string p = "PKG_SISACT_PORTA_CAMP_LG";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //FIN PROY-CAMPANA LG

        //INC-SMS_PORTA_INI
        public static string PKG_SMS_PORTABILIDADES
        {
            get
            {
                string p = "PKG_SMS_PORTABILIDADES";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //INC-SMS_PORTA_FIN

        //INICIO PROY-140419 Autorizar Portabilidad sin PIN
        public static string PKG_BONOS_FULLCLARO 
        {
            get 
            {
                string p = "PKG_BONOS_FULLCLARO";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //FIN PROY-140419 Autorizar Portabilidad sin PIN

       //Inicio Proy-140397 LVR
        public static string PkgMantConvergente
        {
            get
            {
               
                string p = "SISACT_PKG_MANT_CONVERGENTE_6";
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //Fin Proy-140397 LVR

        //PROY-140457-DEBITO AUTOMATICO-INI
        public static string PKG_DOMICILIACION
        {
            get
            {
                string p = "TIM027_PKG_DOMICILIACION";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaBSCS"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_DEBITO_AUTOMATICO 
        {
            get 
            {
                string p = "PKG_DEBITO_AUTOMATICO";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //PROY-140457-DEBITO AUTOMATICO-FIN

        //INI: INICIATIVA-219
        public static string SISACT_PKG_GENERAL_CBIO
        {
            get
            {
                string p = "SISACT_PKG_GENERAL_CBIO";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_CBIO_CATALOGO_POSTV
        {
            get
            {
                string p = "PKG_CBIO_CATALOGO_POSTV";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //FIN: INICIATIVA-219
        //INC000003048070 
        public static string PKG_BONOS_FULLCLARO_BSCS
        {
            get
            {
                string p = "PKG_BONOS_FULLCLARO";
                string esquema = ConfigurationManager.AppSettings["EsquemaBSCS"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //FIN INC000003048070 

        //PROY-140736
        public static string PKG_SISACT_BUYBACK
        {
            get
            {
                string p = "SISACT_PKG_CAMPANA_BUYBACK";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        //PROY-140600 INI
        public static string SISACT_PKG_TITULARIDAD_CLIENTE
        {
            get
            {
                string p = "SISACT_PKG_TITULARIDAD_CLIENTE";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        //PROY-140600 FIN

        #region [PROY-140743 - IDEA-141192 - Venta en cuotas accesorios con cargo al recibo fijo móvil]
        public static string SISACT_PKG_MANT_PROMO_ACC
        {
            get
            {
                //string p = "SISACT_PKG_MANT_PROMO_ACC";
                string p = "SISACT_PKG_MANT_PROMO_140743"; //HARDCODEO
                string esquema = ConfigurationManager.AppSettings["EsquemaSISACT"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PKG_CONSULTAS_SIACU
        {
            get
            {
                string p = "PKG_CONSULTAS_SIACU";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaBSCS"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }

        public static string PP004_SIAC_HFC
        {
            get
            {
                string p = "PP004_SIAC_HFC";
                string esquema = System.Configuration.ConfigurationManager.AppSettings["EsquemaBSCS"];
                if (esquema != null && esquema != "") p = esquema + "." + p;
                return p;
            }
        }
        #endregion
    }
}
