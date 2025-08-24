using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class DAPortabilidad
    {
        //PROY-24740
        public List<BEParametroPortabilidad> ListarParametroPortabilidad(string tipo_parametro, string codigo_parametro, string ref1, string ref2, string ref3, int status)
        {
            DAABRequest.Parameter[] arrParam = {
		        new DAABRequest.Parameter("P_PK_PARAT_PARAC_TP", DbType.String, ParameterDirection.Input),
		        new DAABRequest.Parameter("P_PK_PARAT_PARAC_COD", DbType.String, ParameterDirection.Input),
		        new DAABRequest.Parameter("P_PARAV_REF1", DbType.String, ParameterDirection.Input),
		        new DAABRequest.Parameter("P_PARAV_REF2", DbType.String, ParameterDirection.Input),
		        new DAABRequest.Parameter("P_PARAV_REF3", DbType.String, ParameterDirection.Input),
		        new DAABRequest.Parameter("P_PARAN_STATUS", DbType.Int32, ParameterDirection.Input),
		        new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
	        };
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (tipo_parametro != "") { arrParam[i].Value = tipo_parametro; }
            ++i; if (codigo_parametro != "") { arrParam[i].Value = codigo_parametro; }
            ++i; if (ref1 != "") { arrParam[i].Value = ref1; }
            ++i; if (ref2 != "") { arrParam[i].Value = ref2; }
            ++i; if (ref3 != "") { arrParam[i].Value = ref3; }
            ++i; arrParam[i].Value = status;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SP_LISTAR_PARAMETROS_PORTA";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEParametroPortabilidad> objLista = new List<BEParametroPortabilidad>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEParametroPortabilidad item = new BEParametroPortabilidad();
                    item.PK_PARAT_PARAC_TP = Funciones.CheckStr(dr["PK_PARAT_PARAC_TP"]);
                    item.PK_PARAT_PARAC_COD = Funciones.CheckStr(dr["PK_PARAT_PARAC_COD"]);
                    item.DESCRIPCION = Funciones.CheckStr(dr["PARAV_DESCRIPCION"]);
                    item.STATUS = Funciones.CheckInt(dr["PARAN_STATUS"]);
                    item.REF1 = Funciones.CheckStr(dr["PARAV_REF1"]);
                    item.REF2 = Funciones.CheckStr(dr["PARAV_REF2"]);
                    item.REF3 = Funciones.CheckStr(dr["PARAV_REF3"]);
                    item.REF4 = Funciones.CheckStr(dr["PARAV_REF4"]);
                    item.REF5 = Funciones.CheckStr(dr["PARAV_REF5"]);
                    item.REF6 = Funciones.CheckStr(dr["PARAV_REF6"]);
                    item.REF7 = Funciones.CheckStr(dr["PARAV_REF7"]);
                    item.FECHA_CREACION = Funciones.CheckDate(dr["PARAT_FECHA_CREACION"]);
                    item.USUARIO_CREA = Funciones.CheckStr(dr["PARAV_USUARIO_CREA"]);
                    objLista.Add(item);
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }
        //INICIO PROY 2X1
        public bool VigenciaPYLC_PosPago(string strDocCliente, Int64 grupo_camp, out String o_codigo, out String o_mensaje)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_CLIEC_NUM_DOC", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_PARAN_GRUPO", DbType.Int64,ParameterDirection.Input),
												   new DAABRequest.Parameter("PO_CODIGO_RPTA", DbType.String, ParameterDirection.Output),
												   new DAABRequest.Parameter("PO_MENSAJE_RPTA", DbType.String,200, ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;
        
            if (strDocCliente != null && strDocCliente != "") arrParam[0].Value = strDocCliente;
            if (grupo_camp > 0) arrParam[1].Value = grupo_camp;
            bool b_retorno = false;
            var obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_PORTA_VALIDA + ".SISACTSS_VIGENCIAPYLC_POSTPAGO";
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                IDataParameter parSalida1, parSalida2;
                parSalida1 = (IDataParameter)obRequest.Parameters[2];
                parSalida2 = (IDataParameter)obRequest.Parameters[3];
                o_codigo = Funciones.CheckStr(parSalida1.Value);
                o_mensaje = Funciones.CheckStr(parSalida2.Value.ToString());
                b_retorno = true;

            }
            catch (Exception ex)
            {
                o_mensaje = ex.Message.ToString();
                o_codigo = "1";
                b_retorno = false;
            }
            finally
            {
                obRequest.Factory.Dispose();
            }
            return b_retorno;
        }
        //FIN PROY_2X1
        public Int64 ValidarDisponibleNroPorta(string nroTelefono)
        {
            DAABRequest.Parameter[] arrParam = {   
                new DAABRequest.Parameter("P_NUMERO", DbType.String,11,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SEC", DbType.Object,ParameterDirection.Output)
			};
            arrParam[0].Value = nroTelefono;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SP_VERIFICAR_NUMERO";
            objRequest.Parameters.AddRange(arrParam);

            Int64 nroSEC = 0;
            IDataReader dr = null;

            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    nroSEC = Funciones.CheckInt64(dr["SEC"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();

                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }

        public String ValidarTienePortabilidadTablet(string tipoDocumento, string nroDocumento, int strCodParamCampanaPortaMasTablet, int strCodParamNumDiasVigenciaCampanaPortaMasTablet) //CAMPAÑA PORTA+TABLET - INICIO
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("P_RESULTADO", DbType.String,1,ParameterDirection.Output),
                new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String,16,ParameterDirection.Input),
                new DAABRequest.Parameter("P_COD_PARAM_COD_CAMPANA", DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("P_COD_PARAM_VIGENCIA_CAMPANA", DbType.Int32,ParameterDirection.Input)
			};

            arrParam[1].Value = tipoDocumento;
            arrParam[2].Value = Funciones.FormatoNroDocumentoBD(tipoDocumento, nroDocumento);
            arrParam[3].Value = strCodParamCampanaPortaMasTablet;
            arrParam[4].Value = strCodParamNumDiasVigenciaCampanaPortaMasTablet;
            
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SP_CON_VALIDA_PORTA_TABLET";
            objRequest.Parameters.AddRange(arrParam);

            String nroSEC = string.Empty;
            IDataReader dr = null;

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[0];
                nroSEC = Funciones.CheckStr(parSalida1.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();

                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        } //CAMPAÑA PORTA+TABLET - FIN
        
        public Int64 GrabarEvaluacionPersona(BESolicitudPersona objSolicitud)
        {
            DAABRequest.Parameter[] arrParam = {
			    new DAABRequest.Parameter("K_RESULTADO", DbType.Double,ParameterDirection.Output),
			    new DAABRequest.Parameter("P_OVENC_CODIGO", DbType.String,4,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CANAC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_USU_VEN", DbType.AnsiStringFixedLength,10,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_EXI_BSC_FIN", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_ANALC_CODIGO", DbType.AnsiStringFixedLength,4,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.AnsiStringFixedLength,16,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEV_RAZ_SOC", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEN_PROM_VEN", DbType.String,10,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TPROC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SEGMN_CODIGO", DbType.Int32,4,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TCLIC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TVENC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TACTC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TOPEN_CODIGO", DbType.String,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_PLA_MAX_1", DbType.AnsiStringFixedLength,3,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_PLA_MAX_2", DbType.AnsiStringFixedLength,3,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_PLA_MAX_3", DbType.AnsiStringFixedLength,3,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PACUC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_FPAGC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_CAN_LIN", DbType.Int32,4,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_RFINC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_MRECC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_TIP_CAR_MAN", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_IMP_DG_MAN", DbType.String,14,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_ESTAC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TEVAC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_FLA_TER", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_DES_EST", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_DES_OFI_VEN", DbType.String,20,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_DES_RES_FIN", DbType.String,20,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_DES_TIP_ACT", DbType.String,20,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_COM_PUN_VEN", DbType.String,200,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_COM_EVALUADOR", DbType.String,200,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_USU_CRE", DbType.AnsiStringFixedLength,10,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEV_NOM", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEV_APE_PAT", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEV_APE_MAT", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_EVA_ESS", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_EVA_SUN", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_COD_RES_DIR", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_DES_RES_DIR", DbType.String,20,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_CAR_CLI", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_TIP_CAR_FIJ", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_IMP_DG", DbType.Double,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_RES_EXP_CON", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_NUM_OPE_CON", DbType.String,15,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_LIM_CRE_CON", DbType.Double,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_SUM_CAR_CON", DbType.Double,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_NUM_CAR_FIJ", DbType.Double,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TCESC_CODIGO", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_SCO_TXT_CON", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_SCO_NUM_CON", DbType.String,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_FLAG_INFOCORP", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_HINFV_MENSAJE", DbType.String,1000,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_RUCEMPLEADOR", DbType.String,15,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_NOMBREEMPRESA", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CODCAMPANNA", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_EXI_BSC_CON", DbType.AnsiStringFixedLength,2,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_VENDEDOR_ID", DbType.String,8,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_FLAG_CONSUMO", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_FLAG_PORTABILIDAD", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PORT_OPER_CED", DbType.Int32,4,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PORT_ESTADO", DbType.String,5,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PORT_TELEF_CONT", DbType.String,15,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PORT_FLAG_REC_OPE_CED", DbType.AnsiStringFixedLength,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PORT_CARGO_FIJO_OPE_CED", DbType.String, 10,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_PORT_NRO_FOLIO", DbType.String, 10,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_TLINC_CODIGO_CEDENTE", DbType.String, 10,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_FLAG_CORREO",DbType.String,1,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_CORREO",DbType.String,200,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_TEL_SMS", DbType.String,10,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEV_EST_CIV", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIV_UBIGEO_INEI", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_ORIGEN_LC_DC", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_ANALISIS_DC", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_SCORE_RANKING_OPER_DC", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_PUNTAJE_DC", DbType.Double,ParameterDirection.Input),										
			    new DAABRequest.Parameter("P_SOLIN_LC_DATA_CREDITO_DC", DbType.Double,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_LC_BASE_EXTERNA_DC", DbType.Double,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_LC_CLARO_DC", DbType.Double,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_REGLAS_DURAS_DC", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_ALERT_COMPORT_DC", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_ALERTAS_DC", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIC_CORRESP_SALDO_DC", DbType.String,40,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIED_FEC_NAC", DbType.Date,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIED_FEC_NAC_PDV", DbType.Date,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_LC_DISPONIBLE", DbType.Double, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CF_MENORES", DbType.Double, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CF_MAYORES", DbType.Double, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_KIT_COS_INST", DbType.Double, ParameterDirection.Input),
                new DAABRequest.Parameter("P_PRDC_CODIGO", DbType.String, 2, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64 ,ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEV_CALIFICACION_PAGO", DbType.String, 4, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_BURO_CREDITICIO", DbType.Int16, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEV_RIESGO_CLARO", DbType.String, 50, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEV_COMPORTA_PAGO", DbType.String, 20, ParameterDirection.Input),
			    new DAABRequest.Parameter("P_CLIEC_FLAG_EXONERAR_RA", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_CLIEC_CODNACION", DbType.String, 20, ParameterDirection.Input), //PROY-31636
                new DAABRequest.Parameter("P_CLIEV_DESCNACION", DbType.String, 80, ParameterDirection.Input), //PROY-31636
                new DAABRequest.Parameter("PI_SOLIC_DEUDA_CLIENTE", DbType.String, 2, ParameterDirection.Input)//PROY-29121
		    };
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = objSolicitud.OVENC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.CANAC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_USU_VEN;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EXI_BSC_FIN;
            i++; arrParam[i].Value = objSolicitud.ANALC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TDOCC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.CLIEC_NUM_DOC;
            i++; arrParam[i].Value = objSolicitud.CLIEV_RAZ_SOC;
            i++; arrParam[i].Value = objSolicitud.CLIEN_PROM_VEN;
            i++; arrParam[i].Value = objSolicitud.TPROC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SEGMN_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TCLIC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TVENC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TACTC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TOPEN_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_PLA_MAX_1;
            i++; arrParam[i].Value = objSolicitud.SOLIC_PLA_MAX_2;
            i++; arrParam[i].Value = objSolicitud.SOLIC_PLA_MAX_3;
            i++; arrParam[i].Value = objSolicitud.PACUC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.FPAGC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_CAN_LIN;
            i++; arrParam[i].Value = objSolicitud.RFINC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.MRECC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_TIP_CAR_MAN;
            i++; arrParam[i].Value = objSolicitud.SOLIN_IMP_DG_MAN;
            i++; arrParam[i].Value = objSolicitud.ESTAC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.TEVAC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_FLA_TER;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_EST;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_OFI_VEN;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_RES_FIN;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_TIP_ACT;
            i++; arrParam[i].Value = objSolicitud.SOLIV_COM_PUN_VEN;
            i++; arrParam[i].Value = objSolicitud.SOLIV_COM_EVALUADOR;
            i++; arrParam[i].Value = objSolicitud.SOLIC_USU_CRE;
            i++; arrParam[i].Value = objSolicitud.CLIEV_NOM;
            i++; arrParam[i].Value = objSolicitud.CLIEV_APE_PAT;
            i++; arrParam[i].Value = objSolicitud.CLIEV_APE_MAT;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EVA_ESS;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EVA_SUN;
            i++; arrParam[i].Value = objSolicitud.SOLIC_COD_RES_DIR;
            i++; arrParam[i].Value = objSolicitud.SOLIV_DES_RES_DIR;
            i++; arrParam[i].Value = objSolicitud.SOLIV_CAR_CLI;
            i++; arrParam[i].Value = objSolicitud.SOLIC_TIP_CAR_FIJ;
            i++; arrParam[i].Value = objSolicitud.SOLIN_IMP_DG;
            i++; arrParam[i].Value = objSolicitud.SOLIV_RES_EXP_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIV_NUM_OPE_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LIM_CRE_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_SUM_CAR_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_NUM_CAR_FIJ;
            i++; arrParam[i].Value = objSolicitud.TCESC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIC_SCO_TXT_CON;
            i++; arrParam[i].Value = objSolicitud.SOLIN_SCO_NUM_CON;
            i++; arrParam[i].Value = objSolicitud.FLAG_INFOCORP;
            i++; arrParam[i].Value = objSolicitud.HINFV_MENSAJE;
            i++; arrParam[i].Value = objSolicitud.RUCEMPLEADOR;
            i++; arrParam[i].Value = objSolicitud.NOMBREEMPRESA;
            i++; arrParam[i].Value = objSolicitud.CODCAMPANNA;
            i++; arrParam[i].Value = objSolicitud.SOLIC_EXI_BSC_CON;
            i++; arrParam[i].Value = objSolicitud.VENDEDOR_ID;
            i++; arrParam[i].Value = objSolicitud.FLAG_CONSUMO;

            i++; arrParam[i].Value = objSolicitud.FLAG_PORTABILIDAD;
            i++; arrParam[i].Value = objSolicitud.PORT_OPER_CED;
            i++; arrParam[i].Value = objSolicitud.PORT_ESTADO;
            i++; arrParam[i].Value = objSolicitud.PORT_TELEF_CONT;
            i++; arrParam[i].Value = objSolicitud.PORT_FLAG_REC_OPE_CED;
            i++; arrParam[i].Value = objSolicitud.PORT_CARGO_FIJO_OPE_CED;
            i++; arrParam[i].Value = objSolicitud.PORT_NRO_FOLIO;
            i++; arrParam[i].Value = objSolicitud.TLINC_CODIGO_CEDENTE;

            i++; arrParam[i].Value = objSolicitud.SOLIV_FLAG_CORR;
            i++; arrParam[i].Value = objSolicitud.SOLIV_CORREO;

            i++; arrParam[i].Value = objSolicitud.SOLIV_TEL_SMS;

            i++; arrParam[i].Value = objSolicitud.CLIEV_EST_CIV;
            i++; arrParam[i].Value = objSolicitud.SOLIV_UBIGEO_INEI;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ORIGEN_LC_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ANALISIS_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_SCORE_RANKING_OPER_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_PUNTAJE_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LC_DATA_CREDITO_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LC_BASE_EXTERNA_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIN_LC_CLARO_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_REGLAS_DURAS_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ALERT_COMPORT_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_ALERTAS_DC;
            i++; arrParam[i].Value = objSolicitud.SOLIC_CORRESP_SALDO_DC;
            i++; arrParam[i].Value = objSolicitud.CLIED_FEC_NAC;
            i++; arrParam[i].Value = objSolicitud.CLIED_FEC_NAC_PDV;
            i++; arrParam[i].Value = objSolicitud.LC_DISPONIBLE;
            i++; arrParam[i].Value = objSolicitud.CF_MENORES;
            i++; arrParam[i].Value = objSolicitud.CF_MAYORES;
            i++; arrParam[i].Value = objSolicitud.SOLIN_KIT_COS_INST;
            //i++; arrParam[i].Value = objSolicitud.DEUDA;
            //i++; arrParam[i].Value = objSolicitud.BLOQUEO;
            //i++; arrParam[i].Value = objSolicitud.CLIEN_SEC_ASOCIADA;
            //i++; arrParam[i].Value = objSolicitud.RESPUESTA_DC;
            i++; arrParam[i].Value = objSolicitud.PRDC_CODIGO;
            i++; arrParam[i].Value = objSolicitud.SOLIN_GRUPO_SEC;
            i++; arrParam[i].Value = objSolicitud.CLIEV_CALIFICACION_PAGO;
            i++; arrParam[i].Value = objSolicitud.BURO_CREDITICIO;
            //i++; arrParam[i].Value = objSolicitud.SOLIN_IMP_DG_GRUPO_SEC;
            //i++; arrParam[i].Value = objSolicitud.SOLIN_CF_GRUPO_SEC;
            i++; arrParam[i].Value = objSolicitud.CLIEV_RIESGO_CLARO;
            i++; arrParam[i].Value = objSolicitud.CLIEV_COMPORTA_PAGO;
            i++; arrParam[i].Value = objSolicitud.CLIEC_FLAG_EXONERAR_RA;
            i++; arrParam[i].Value = objSolicitud.CLIEC_CODNACION; //PROY-31636
            i++; arrParam[i].Value = objSolicitud.CLIEV_DESCNACION; //PROY-31636
            i++; arrParam[i].Value = objSolicitud.DEUDA_CLIENTE;//PROY-29121

            Int64 nroSEC;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objSolicitud.NRO_DOCUMENTO);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SP_INSERT_SOL_POSTPAGO_PDV";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter pSalida1;
                pSalida1 = (IDataParameter)objRequest.Parameters[0];
                nroSEC = Funciones.CheckInt64(pSalida1.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }

        public Int64 GrabarEvaluacionEmpresa(BESolicitudEmpresa item)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("K_RESULTADO", DbType.Int64,ParameterDirection.Output),
				new DAABRequest.Parameter("P_OVENC_CODIGO",DbType.String,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CANAC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ANEXO2", DbType.String,7,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_EXI_BSC_FIN", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ANALC_CODIGO", DbType.String,4,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String,16,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_RAZ_SOC", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_NOMBRE", DbType.String,40,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_APE_PAT", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_APE_MAT", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEN_PROM_VEN", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TPROC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SEGMN_CODIGO", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TCLIC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TVENC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TACTC_CODIGO", DbType.String,2,ParameterDirection.Input),												   
				new DAABRequest.Parameter("P_SOLIN_CAN_LIN", DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RFINC_CODIGO", DbType.String,2,ParameterDirection.Input),												   
				new DAABRequest.Parameter("P_SOLIC_TIP_CAR_MAN", DbType.String,1,ParameterDirection.Input),												   
				new DAABRequest.Parameter("P_SOLIN_NUM_CAR_FIJ", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESTAC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TEVAC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_FLA_TER", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_EST", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_OFI_VEN", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_RES_FIN", DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_DES_TIP_ACT", DbType.String,20,ParameterDirection.Input),												   
				new DAABRequest.Parameter("P_SOLIV_COM_EVALUADOR", DbType.String,500,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_USU_CRE", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLASC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_USUAN_CODIGO", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLEXN_CODIGO", DbType.Int64,ParameterDirection.Input),												   
				new DAABRequest.Parameter("P_TRIEC_CODIGO", DbType.String,4,ParameterDirection.Input),		//PROY-140257										   
				new DAABRequest.Parameter("P_OPERC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CAN_LIN_EXCOMP", DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CONSULTOR", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_DEUDA_CLIENTE", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_LINEA_CLIENTE", DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_ANTIGUEDAD", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CODIGO_PADRE", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_FLAG_REINGRESO", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_NUM_OPE_CON", DbType.String,15,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_NUM_CAR_FIJ_LINEA", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_ANTIGUEDAD_CLIENTE", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_FLAG_EMPRESA_TRAFICO", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_FLA_VER_RES", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_FLAG_EMPRESA_TOLERAN", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_SUM_CAR_FIN", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CAR_FIJO_ACTUAL", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_SUBSIDIO_TOTAL", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LINEAS_RECURRENTE_ACTUAL", DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LINEAS_MAYOR_N_DIAS", DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("P_LINEAS_MENOR_N_DIAS", DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PARAM_DIAS_RECURRENTE", DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_COM_DG", DbType.String,200,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_FLAG_ENVIO", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_BOLSA_REF", DbType.Double,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CAMPN_CODIGO", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_PORTABILIDAD", DbType.String,1,ParameterDirection.Input),												   
				new DAABRequest.Parameter("P_PORT_OPER_CED", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TLINC_CODIGO_CEDENTE", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PORT_SOLIN_NRO_FORMATO", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PORT_CARGO_FIJO_OPE_CED", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PORT_ESTADO", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_PORT_TELEF_CONT", DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_FLAG_CORR", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIV_CORREO", DbType.String,200,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_LINEA_CREDITO_CALC", DbType.Double,ParameterDirection.Input),
				//JAR
				new DAABRequest.Parameter("P_DPCHN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_PRE_DIR", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_DIRECCION", DbType.String, 4000,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_REF_DIR", DbType.String, 4000, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_COD_DEP_DIR", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_COD_PRO_DIR", DbType.String, 3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_COD_DIS_DIR", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_COD_POS_DIR", DbType.String, 3, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_PRE_TEL_LEG", DbType.String,3, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_TEL_LEG", DbType.String, 13, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_PRE_DIR_FAC", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_DIR_FAC", DbType.String, 4000,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_REF_DIR_FAC", DbType.String, 40, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_COD_DEP_FAC", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_COD_PRO_FAC", DbType.String, 3,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_COD_DIS_FAC", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_COD_POS_FAC", DbType.String, 3, ParameterDirection.Input),
				new DAABRequest.Parameter("P_RGLPC_PODERES", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PACUC_CODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TOPEN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_TIP_CAR_FIJ", DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_IMP_DG", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_IMP_DG_MAN", DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TPREC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FPAGC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_LIM_CRE_FIN", DbType.Decimal,ParameterDirection.Input),
				//FIN JAR
				new DAABRequest.Parameter("P_SOLIN_SUM_CAR_CON", DbType.Decimal,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TCESC_CODIGO", DbType.String,2,ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_KIT_COS_INST", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PRDC_CODIGO", DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_CALIFICACION_PAGO", DbType.String, 4, ParameterDirection.Input),
				new DAABRequest.Parameter("P_BURO_CREDITICIO", DbType.Int16, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEN_MONTO_VENCIDO", DbType.Double, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_RIESGO_CLARO", DbType.String, 50, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEV_COMPORTA_PAGO", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CLIEC_FLAG_EXONERAR_RA", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_SOLIC_DEUDA_CLIENTE", DbType.String, 2, ParameterDirection.Input),//PROY-29121
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = item.OVENC_CODIGO;
            ++i; arrParam[i].Value = item.CANAC_CODIGO;
            ++i; arrParam[i].Value = item.ANEXO2;
            ++i; arrParam[i].Value = item.SOLIC_EXI_BSC_FIN;
            ++i; arrParam[i].Value = item.ANALC_CODIGO;
            ++i; arrParam[i].Value = item.TDOCC_CODIGO;
            ++i; arrParam[i].Value = item.CLIEC_NUM_DOC;
            ++i; arrParam[i].Value = item.CLIEV_RAZ_SOC;
            ++i; arrParam[i].Value = item.CLIEV_NOMBRE;
            ++i; arrParam[i].Value = item.CLIEV_APE_PAT;
            ++i; arrParam[i].Value = item.CLIEV_APE_MAT;
            ++i; arrParam[i].Value = item.CLIEN_PROM_VEN;
            ++i; arrParam[i].Value = item.TPROC_CODIGO;
            ++i; arrParam[i].Value = item.SEGMN_CODIGO;
            ++i; arrParam[i].Value = item.TCLIC_CODIGO;
            ++i; arrParam[i].Value = item.TVENC_CODIGO;
            ++i; arrParam[i].Value = item.TACTC_CODIGO;
            ++i; arrParam[i].Value = item.SOLIN_CAN_LIN;
            ++i; arrParam[i].Value = item.RFINC_CODIGO;
            ++i; arrParam[i].Value = item.SOLIC_TIP_CAR_MAN;
            ++i; arrParam[i].Value = item.SOLIN_NUM_RA; // item.SOLIN_NUM_CAR_FIJ;
            ++i; arrParam[i].Value = item.ESTAC_CODIGO;
            ++i; arrParam[i].Value = item.TEVAC_CODIGO;
            ++i; arrParam[i].Value = item.SOLIC_FLA_TER;
            ++i; arrParam[i].Value = item.SOLIV_DES_EST;
            ++i; arrParam[i].Value = item.SOLIV_DES_OFI_VEN;
            ++i; arrParam[i].Value = item.SOLIV_DES_RES_FIN;
            ++i; arrParam[i].Value = item.SOLIV_DES_TIP_ACT;
            ++i; arrParam[i].Value = item.SOLIV_COM_EVALUADOR;
            ++i; arrParam[i].Value = item.SOLIC_USU_CRE;
            ++i; arrParam[i].Value = item.CLASC_CODIGO;
            ++i; if (item.USUAN_CODIGO > 0) arrParam[i].Value = item.USUAN_CODIGO;
            ++i; arrParam[i].Value = item.FLEXN_CODIGO;
            ++i; arrParam[i].Value = item.TRIEC_CODIGO;
            ++i; arrParam[i].Value = item.OPERC_CODIGO;
            ++i; if (item.SOLIN_CAN_LIN_EXCOMP > 0) arrParam[i].Value = item.SOLIN_CAN_LIN_EXCOMP;
            ++i; if (item.CONSULTOR_ID > 0) arrParam[i].Value = item.CONSULTOR_ID;
            ++i; if (item.SOLIN_DEUDA_CLIENTE > 0) arrParam[i].Value = item.SOLIN_DEUDA_CLIENTE;
            ++i; if (item.SOLIN_LINEA_CLIENTE > 0) arrParam[i].Value = item.SOLIN_LINEA_CLIENTE;
            ++i; if (item.SOLIN_ANTIGUEDAD > 0) arrParam[i].Value = item.SOLIN_ANTIGUEDAD;
            ++i; if (item.SOLIN_CODIGO_PADRE > 0) arrParam[i].Value = item.SOLIN_CODIGO_PADRE;
            ++i; if (item.SOLIC_FLAG_REINGRESO != "") arrParam[i].Value = item.SOLIC_FLAG_REINGRESO;
            ++i; if (item.SOLIV_NUM_OPE_CON != "") arrParam[i].Value = item.SOLIV_NUM_OPE_CON;
            ++i; if (item.SOLIN_NUM_CAR_FIJ_LINEA > 0) arrParam[i].Value = item.SOLIN_NUM_CAR_FIJ_LINEA;
            ++i; if (item.SOLIN_ANTIGUEDAD_CLIENTE > 0) arrParam[i].Value = item.SOLIN_ANTIGUEDAD_CLIENTE;
            ++i; if (item.SOLIC_FLAG_EMPRESA_TRAFICO != "") arrParam[i].Value = item.SOLIC_FLAG_EMPRESA_TRAFICO;
            ++i; if (item.FLAG_RESPONSABLE_PUNTO_VENTA != "") arrParam[i].Value = item.FLAG_RESPONSABLE_PUNTO_VENTA;
            ++i; if (item.SOLIC_FLAG_EMPRESA_TOLERAN != "") arrParam[i].Value = item.SOLIC_FLAG_EMPRESA_TOLERAN;
            ++i; arrParam[i].Value = item.SOLIN_SUM_CAR_FIN;
            ++i; arrParam[i].Value = item.SOLIN_CAR_FIJO_ACTUAL;
            ++i; arrParam[i].Value = item.SOLIN_SUBSIDIO_TOTAL;

            ++i; arrParam[i].Value = item.NRO_LINEAS_RECURRENTE_ACTUAL;
            ++i; arrParam[i].Value = item.NRO_LINEAS_MAYOR_N_DIAS;
            ++i; arrParam[i].Value = item.NRO_LINEAS_MENOR_N_DIAS;
            ++i; arrParam[i].Value = item.DIAS_LINEAS_RECURRENTE;
            ++i; arrParam[i].Value = item.SOLIV_COM_DG;
            ++i; arrParam[i].Value = item.SOLIV_FLAG_ENVIO;
            ++i; if (item.SOLIN_BOLSA_REF > 0) arrParam[i].Value = item.SOLIN_BOLSA_REF;
            ++i; if (item.CAMPN_CODIGO > 0) arrParam[i].Value = item.CAMPN_CODIGO;
            ++i; arrParam[i].Value = item.FLAG_PORTABILIDAD;
            ++i; arrParam[i].Value = item.PORT_OPER_CED;
            ++i; arrParam[i].Value = item.TLINC_CODIGO_CEDENTE;
            ++i; arrParam[i].Value = item.PORT_SOLIN_NRO_FORMATO;
            ++i; arrParam[i].Value = item.PORT_CARGO_FIJO_OPE_CED;
            ++i; arrParam[i].Value = item.PORT_ESTADO;
            ++i; arrParam[i].Value = item.PORT_TELEF_CONT;
            ++i; arrParam[i].Value = item.FLAG_CORREO;
            ++i; arrParam[i].Value = item.SOLIV_CORREO;
            ++i; arrParam[i].Value = item.SOLIN_LINEA_CREDITO_CALC; //E75810 
            //JAR
            ++i; arrParam[i].Value = item.DPCHN_CODIGO;
            ++i; arrParam[i].Value = item.CLIEV_PRE_DIR;
            ++i; arrParam[i].Value = item.CLIEV_DIRECCION;
            ++i; arrParam[i].Value = item.CLIEV_REF_DIR;
            ++i; arrParam[i].Value = item.CLIEC_COD_DEP_DIR;
            ++i; arrParam[i].Value = item.CLIEC_COD_PRO_DIR;
            ++i; arrParam[i].Value = item.CLIEC_COD_DIS_DIR;
            ++i; arrParam[i].Value = item.CLIEC_COD_POS_DIR;
            ++i; arrParam[i].Value = item.CLIEV_PRE_TEL_LEG;
            ++i; arrParam[i].Value = item.CLIEV_TEL_LEG;
            ++i; arrParam[i].Value = item.CLIEV_PRE_DIR_FAC;
            ++i; arrParam[i].Value = item.CLIEV_DIR_FAC;
            ++i; arrParam[i].Value = item.CLIEV_REF_DIR_FAC;
            ++i; arrParam[i].Value = item.CLIEC_COD_DEP_FAC;
            ++i; arrParam[i].Value = item.CLIEC_COD_PRO_FAC;
            ++i; arrParam[i].Value = item.CLIEC_COD_DIS_FAC;
            ++i; arrParam[i].Value = item.CLIEC_COD_POS_FAC;
            ++i; arrParam[i].Value = item.RGLPC_PODERES;
            ++i; arrParam[i].Value = item.PACUC_CODIGO;
            ++i; arrParam[i].Value = item.TOPEN_CODIGO;
            ++i; arrParam[i].Value = item.SOLIC_TIP_CAR_FIJ;
            ++i; arrParam[i].Value = item.SOLIN_IMP_DG;
            ++i; arrParam[i].Value = item.SOLIN_IMP_DG_MAN;
            ++i; arrParam[i].Value = item.TPREC_CODIGO;
            ++i; arrParam[i].Value = item.FPAGC_CODIGO;
            ++i; if (item.SOLIN_LIM_CRE_FIN > 0) arrParam[i].Value = item.SOLIN_LIM_CRE_FIN;
            //FIN JAR

            ++i; if (item.SOLIN_SUM_CAR_CON > 0) arrParam[i].Value = item.SOLIN_SUM_CAR_CON;
            ++i; arrParam[i].Value = item.TCESC_CODIGO;
            ++i; arrParam[i].Value = item.SOLIN_KIT_COS_INST;
            ++i; arrParam[i].Value = item.PRDC_CODIGO;
            ++i; arrParam[i].Value = item.CLIEV_CALIFICACION_PAGO;
            ++i; arrParam[i].Value = item.BURO_CODIGO;
            ++i; arrParam[i].Value = item.CLIEN_MONTO_VENCIDO;

            ++i; arrParam[i].Value = item.CLIEV_RIESGO_CLARO;
            ++i; arrParam[i].Value = item.CLIEV_COMPORTA_PAGO;
            ++i; arrParam[i].Value = item.CLIEC_FLAG_EXONERAR_RA;

            ++i; if (item.SOLIN_GRUPO_SEC > 0) arrParam[i].Value = item.SOLIN_GRUPO_SEC;
            ++i; arrParam[i].Value = item.SOLIC_DEUDA_CLIENTE; //PROY-29121

            Int64 nroSEC;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_PORTABILIDAD_CORP + ".SECSI_INS_SOL_CORP_PORT_PDV";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[0];
                nroSEC = Funciones.CheckInt64(parSalida1.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return nroSEC;
        }

        public bool GrabarNumeroPortabilidad(BENumeroPortabilidad objDetalle)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PORT_NUM_DOC" ,DbType.String, 15, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PLANC_CODIGO", DbType.String, 3, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PORT_NUMERO", DbType.String, 15, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PORT_MODALIDAD", DbType.String, 15, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_ESTADO", DbType.String, 1, ParameterDirection.Input),
				new DAABRequest.Parameter("P_TPROC_CODIGO", DbType.String, 2, ParameterDirection.Input),
				new DAABRequest.Parameter("P_PORT_USU_CREA", DbType.String, 10, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_GRUPO_SEC", DbType.Int64, ParameterDirection.Input),
                new DAABRequest.Parameter("P_PORT_TIPO_SERVICIO", DbType.String, 2, ParameterDirection.Input),
                new DAABRequest.Parameter("P_PORT_TIPO_PLAN", DbType.String, 2, ParameterDirection.Input)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[1].Value = objDetalle.SOLIN_CODIGO;
            arrParam[2].Value = objDetalle.PORT_NUM_DOC;
            arrParam[3].Value = objDetalle.PLANC_CODIGO;
            arrParam[4].Value = objDetalle.PORT_NUMERO;
            arrParam[5].Value = objDetalle.PORT_MODALIDAD;
            arrParam[6].Value = objDetalle.FLAG_ESTADO;
            arrParam[7].Value = objDetalle.TPROC_CODIGO;
            arrParam[8].Value = objDetalle.PORT_USU_CREA;
            if (objDetalle.SOPLN_CODIGO != 0) arrParam[9].Value = objDetalle.SOPLN_CODIGO;
            arrParam[10].Value = objDetalle.SOLIN_GRUPO_SEC;
            arrParam[11].Value = objDetalle.PORT_TIPO_SERVICIO;
            arrParam[12].Value = objDetalle.PORT_TIPO_PLAN;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objDetalle.SOLIN_CODIGO.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SP_INSERT_NUMERO";
            objRequest.Parameters.AddRange(arrParam);
            bool salida = false;

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public bool GrabarArchivoPortabilidad(BEArchivo objArchivo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_RESULTADO" ,DbType.Int64,ParameterDirection.Output),
				new DAABRequest.Parameter("P_SOLIN_CODIGO" ,DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ARCH_NOMBRE" ,DbType.String,100,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ARCH_RUTA" ,DbType.String,4000,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_ESTADO" ,DbType.String,1,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ARCH_TIPO" ,DbType.String,15,ParameterDirection.Input)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[1].Value = objArchivo.SOLIN_CODIGO;
            arrParam[2].Value = objArchivo.ARCH_NOMBRE;
            arrParam[3].Value = objArchivo.ARCH_RUTA;
            arrParam[4].Value = objArchivo.FLAG_ESTADO;
            arrParam[5].Value = objArchivo.ARCH_TIPO;

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), objArchivo.SOLIN_CODIGO.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SP_INSERT_ARCHIVO";
            objRequest.Parameters.AddRange(arrParam);
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public bool EnviarMesaPortabilidad(Int64 nroSEC, string strUsuario)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOPOV_USUARIO_CREA", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_RESPUESTA", DbType.Int32, ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = nroSEC;
            arrParam[1].Value = strUsuario;

            bool salida = false;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SP_ENVIO_MP_X_SEC";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public List<BEArchivo> ListarAchivosAdjunto(Int64 idArchivo, Int64 nroSEC, string tipoArchivo, string flagEstado)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_ARCH_ID", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
				new DAABRequest.Parameter("P_ARCH_TIPO", DbType.String,100, ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_ESTADO", DbType.String,1, ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
			};
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (idArchivo != 0) { arrParam[i].Value = idArchivo; }
            ++i; if (nroSEC != 0) { arrParam[i].Value = nroSEC; }
            ++i; if (tipoArchivo != "") { arrParam[i].Value = tipoArchivo; }
            ++i; if (flagEstado != "") { arrParam[i].Value = flagEstado; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SP_LISTAR_ARCHIVOS_ADJ_SEC";
            objRequest.Parameters.AddRange(arrParam);

            BEArchivo objItem = null;
            List<BEArchivo> objLista = new List<BEArchivo>();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEArchivo();
                    objItem.ARCH_ID = Funciones.CheckInt(dr["ARCH_ID"]);
                    objItem.SOLIN_CODIGO = Funciones.CheckInt(dr["SOLIN_CODIGO"]);
                    objItem.ARCH_NOMBRE = Funciones.CheckStr(dr["ARCH_NOMBRE"]);
                    objItem.ARCH_RUTA = Funciones.CheckStr(dr["ARCH_RUTA"]).Replace("\\", "\\\\");
                    objItem.ARCH_TIPO = Funciones.CheckStr(dr["PARAV_DESCRIPCION"]);
                    objItem.FLAG_ESTADO = Funciones.CheckStr(dr["FLAG_ESTADO"]);

                    objLista.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Clear();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public List<BEReciboDeposito> ListarRecibo(Int64 nroSEC, int banco_id, string nro_recibo)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_BANCN_CODIGO", DbType.Int32,ParameterDirection.Input),
				new DAABRequest.Parameter("P_RECIV_NRO_OPERACION", DbType.String,60,ParameterDirection.Input),
				new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
			};
            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (nroSEC > 0) arrParam[0].Value = nroSEC;
            if (banco_id > 0) arrParam[1].Value = banco_id;
            if (nro_recibo != null && nro_recibo != "") arrParam[2].Value = nro_recibo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_EVALUACION + ".SISACT_CON_RECIBO";
            objRequest.Parameters.AddRange(arrParam);

            BEReciboDeposito objItem = null;
            List<BEReciboDeposito> objLista = new List<BEReciboDeposito>();
            DataTable dt = null;
            try
            {
                dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    objItem = new BEReciboDeposito();
                    objItem.SOLIN_CODIGO = Funciones.CheckInt64(dr["SOLIN_CODIGO"]);
                    objItem.RECIBO_ID = Funciones.CheckInt(dr["RECIN_CODIGO"]);
                    objItem.BANCO_ID = Funciones.CheckInt(dr["BANCN_CODIGO"]);
                    objItem.BANCO_DES = Funciones.CheckStr(dr["TABLN_DESCRIPCION"]);
                    objItem.MONTO_DEPOSITO = Funciones.CheckDbl(dr["RECIN_MONTO"]);
                    objItem.NRO_OPERACION = Funciones.CheckStr(dr["RECIV_NRO_OPERACION"]);
                    objItem.FECHA_DEPOSITO = Funciones.CheckDate(dr["RECID_FECHA_RECIBO"]);

                    objLista.Add(objItem);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dt != null) dt.Clear();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }

        public bool GrabarDatosEvaluador(Int64 nroSEC, double nroRA, string tipoGarantia, string estadoSEC, string estadoSECDes, double total_garantia,
                                                string comentario_pdv, string comentario_evaluador, string comentario_sistema, string login, string loginAutorizador, 
                                                string estadoPort, ref string rFlagProceso)
        {
            DAABRequest.Parameter[] arrParam = {					
				new DAABRequest.Parameter("P_SOLIN_CODIGO",DbType.Int64,ParameterDirection.Input),
				new DAABRequest.Parameter("P_NRO_RA",DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TIP_CAR_MAN",DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESTADO_ID",DbType.String,2,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESTADO_DES",DbType.String,20,ParameterDirection.Input),
				new DAABRequest.Parameter("P_TOTAL_GARANTIA_EVA",DbType.Double,ParameterDirection.Input),
				new DAABRequest.Parameter("P_COMENTARIO_PDV",DbType.String,500,ParameterDirection.Input),
				new DAABRequest.Parameter("P_COMENTARIO_EVA",DbType.String,500,ParameterDirection.Input),
				new DAABRequest.Parameter("P_COMENTARIO_SISTEMA",DbType.String,500,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_USU_CRE",DbType.String,10,ParameterDirection.Input),
				new DAABRequest.Parameter("P_SOLIC_USU_AUT_DG",DbType.String,15,ParameterDirection.Input),
				new DAABRequest.Parameter("P_ESTADO_PORT",DbType.String,5,ParameterDirection.Input),
				new DAABRequest.Parameter("P_FLAG_PROCESO",DbType.String,10,ParameterDirection.Output),
				new DAABRequest.Parameter("P_MSG",DbType.String,255,ParameterDirection.Output)
			};
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; arrParam[i].Value = nroSEC;
            ++i; arrParam[i].Value = nroRA;
            ++i; arrParam[i].Value = tipoGarantia;
            ++i; arrParam[i].Value = estadoSEC;
            ++i; arrParam[i].Value = estadoSECDes;
            ++i; arrParam[i].Value = total_garantia;
            ++i; arrParam[i].Value = comentario_pdv;
            ++i; arrParam[i].Value = comentario_evaluador;
            ++i; arrParam[i].Value = comentario_sistema;
            ++i; arrParam[i].Value = login;
            ++i; arrParam[i].Value = loginAutorizador;
            ++i; arrParam[i].Value = estadoPort;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_PORTABILIDAD_CORP + ".SISACU_ACT_SOL_EVALUADOR_PDV";
            objRequest.Parameters.AddRange(arrParam);

            bool salida = false;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                IDataParameter p1 = (IDataParameter)objRequest.Parameters[arrParam.Length - 1];
                if (!string.IsNullOrEmpty(Funciones.CheckStr(p1.Value)))
                {
                    throw new Exception(Funciones.CheckStr(p1.Value));
                }
                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV
        public List<BEPorttSolicitud> ValidarRespuestaWSCP(string strNumeroSecuencial, ref string rstrCodResp, ref string rstrMensResp)
        {
            List<BEPorttSolicitud> listaConsulta = new List<BEPorttSolicitud>();

            DAABRequest.Parameter[] arrParam = {   new DAABRequest.Parameter("PI_SOPOC_NUM_SECUENCIAL", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_SOPON_SOLIN_CODIGO", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_SOPOC_INICIO_RANGO", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_FLAG_CONSULTA", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PO_CURSALIDA", DbType.Object,ParameterDirection.Output),
												   new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String,ParameterDirection.Output),
												   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String,ParameterDirection.Output)
											   };


            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strNumeroSecuencial;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD_MIGRA + ".SISASS_GET_INFO_CP";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEPorttSolicitud iten = new BEPorttSolicitud();
                    iten.idPortabilidad= Funciones.CheckInt64(dr["PK_SOPOT_SOPON_ID"]);
                    iten.numeroSEC = Funciones.CheckInt64(dr["SOPON_SOLIN_CODIGO"]);
                    iten.numeroLinea = Funciones.CheckStr(dr["SOPOC_INICIO_RANGO"]);
                    iten.fechaActivacionCP = string.IsNullOrEmpty(Funciones.CheckStr(dr["SOPOD_FEC_ACTIVACION"])) ? string.Empty : Funciones.CheckDate(dr["SOPOD_FEC_ACTIVACION"]).ToString("dd/MM/yyyy");
                    iten.fechaEnvioCP = string.IsNullOrEmpty(Funciones.CheckStr(dr["SOLID_ENVIO_CP"])) ? null : Funciones.CheckDate(dr["SOLID_ENVIO_CP"]).ToString("dd/MM/yyyy");
                    iten.flagEnvioCP = Funciones.CheckStr(dr["SOLIC_FLAG_ENVIO_CP"]);
                    iten.codigoEstadoCP = Funciones.CheckStr(dr["SOPOC_ESTA_PROCESO_CP"]);
                    iten.descripcionEstadoCP= Funciones.CheckStr(dr["ESTADO_CP"]);
                    iten.codigoMotivocP = Funciones.CheckStr(dr["SOPOV_MOTIVO_CP"]);
                    iten.descripcionMotivoCP = Funciones.CheckStr(dr["SOPOV_MOTIVO_CP_DES"]);
                    iten.deudaCP = Funciones.CheckStr(dr["SOPON_DEUDA"]);
                    iten.numeroIntentosCP = Funciones.CheckInt(dr["SOPON_NUMERO_INTNETOS"]);
                    listaConsulta.Add(iten);
                }

                rstrCodResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[5]).Value);
                rstrMensResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[6]).Value);

            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return listaConsulta;
        }

        public bool ValidarConsultaPreviaSEC(BeConsultaPrevia objConsultaPrevia, ref string rstrNumeroSecuencial, ref string rstrCodResp, ref string rstrMensResp)
        {

            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_SOPOC_CODIGO_CEDENTE", DbType.String,  ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_SOPOC_MODALIDAD", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_SOPOC_INICIO_RANGO", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_SOPOC_TIPO_DOCUMENTO", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_SOPOV_NUM_DOCUMENTO", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PO_SOPON_SOLIN_CODIGO", DbType.Int64,ParameterDirection.InputOutput),
                                                   new DAABRequest.Parameter("PO_PK_SOPOT_SOPON_ID", DbType.String,ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_SOPOC_NUM_SECUENCIAL", DbType.String,ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_SOPOC_NUM_MENSAJE", DbType.String,ParameterDirection.Output),
												   new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String,ParameterDirection.Output),
												   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String,100,ParameterDirection.Output)
											   };


            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objConsultaPrevia.codigoCedente;
            arrParam[1].Value = objConsultaPrevia.modalidad;
            arrParam[2].Value = objConsultaPrevia.msisdn;
            arrParam[3].Value = objConsultaPrevia.tipoDocumento;
            arrParam[4].Value = objConsultaPrevia.numeroDocumento;
            if (objConsultaPrevia.numeroSEC > 0)
            {
                arrParam[5].Value = objConsultaPrevia.numeroSEC;
            }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD_MIGRA + ".SISASS_VALIDA_CP";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            Int64 numeroSEC = 0;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);                
                numeroSEC = Funciones.CheckInt64(((IDataParameter)obRequest.Parameters[5]).Value);                
                rstrNumeroSecuencial = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[7]).Value);                
                rstrCodResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[9]).Value);
                rstrMensResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[10]).Value);                
            }
            catch (Exception ex) {

                rstrMensResp = ex.Message;
            
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return numeroSEC==0;
        }


        public BEItemMensaje ActualizarConsultaPreviaCP (BeConsultaPrevia objConsultaPrevia,ref string rstrCodResp, ref string rstrMensResp)
        {

            BEItemMensaje objMensaje = new BEItemMensaje(false);

            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_SOPON_SOLIN_CODIGO", DbType.Int64,  ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_SOPOC_NUM_SECUENCIAL", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_SOPOV_OBSERVACIONES", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_SOPOV_NOM_RASO_ABONAD", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_SOPOV_USUARIO_MODIFIC", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String,ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String,ParameterDirection.Output),//PROY-26963
											   };


            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objConsultaPrevia.numeroSEC;
            arrParam[1].Value = objConsultaPrevia.numeroSecuencial;
            arrParam[2].Value = objConsultaPrevia.observaciones;
            arrParam[3].Value = objConsultaPrevia.nombreRSAbonado;
            arrParam[4].Value = objConsultaPrevia.auditoria.Codigo2;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD_MIGRA + ".SISASU_ACTUALIZA_CP";
            obRequest.Parameters.AddRange(arrParam);
            obRequest.SaveLog = false;
            obRequest.Transactional = true;


            IDataReader dr = null;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
                rstrCodResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[5]).Value);
                rstrMensResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[6]).Value);
              //  if (rstrCodResp == "0") objMensaje.exito = true;

                if (rstrCodResp == "1") objMensaje.exito = true; 

            }
            catch (Exception ex){

                objMensaje.exito = false;
                rstrMensResp = ex.Message; 
                rstrCodResp = "0";
                objMensaje.descripcion = ex.Message;

            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return objMensaje;
        }

        public List<BeConsultaPrevia> ObtenerListaSolicitudPortabilidadBySec(Int64 p_id_sec)
        {

            List<BeConsultaPrevia> listaResulta = new List<BeConsultaPrevia>();

            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, p_id_sec, ParameterDirection.Input),
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
											   };

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 0; if (p_id_sec != 0) { arrParam[i].Value = p_id_sec; }

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD_MIGRA + ".SP_GET_SOLICITUD_BY_CODSEC";
            obRequest.Parameters.AddRange(arrParam);
            
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BeConsultaPrevia item = new BeConsultaPrevia();

                    item.msisdn = Funciones.CheckStr(dr["PORT_NUMERO"]);                                       
                    item.modalidad = Funciones.CheckStr(dr["PORT_MODALIDAD"]);                    
                    item.numeroDocumento= Funciones.CheckStr(dr["PORT_NUM_DOC"]);
                    listaResulta.Add(item);
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return listaResulta;
        }

        public bool EliminarRegistroCPPortabilidad( Int64 idPortabilidad)
        {
            string strCodRespuesta = "0";
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_PK_SOPOT_SOPON_ID", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String,10, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String,200, ParameterDirection.Output),
											   };

            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = idPortabilidad;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD_MIGRA + ".SISASD_CONSULTA_CP";
            obRequest.Parameters.AddRange(arrParam);
            
            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                strCodRespuesta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[1]).Value);
            }
            finally
            {                
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return strCodRespuesta == "0";
        }
        //PROY-26963 - IDEA-34399: Mejora Portabilidad Despacho PDV

//PROY-31393 INI
        public List<string> PorttValidaABCDP(BEPorttConfiguracion objPortConfiguracion)
        {

            List<string> result = new List<string>() { "1", "Sin recuperar configuracion CP", "|C|P|E|" };

            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_PORTV_EST_PROCESO" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_PORTV_MOTIVO" ,DbType.String,ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_PORTN_FLAG_ACREDITA" ,DbType.Int64,ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_PORTV_OPERADOR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_PORTV_TIPO_PRODUCTO" ,DbType.String,ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_PORTV_TIPO_VENTA" ,DbType.String,ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_PORTV_APLICACION" ,DbType.String,ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_PORTV_MOD_VENTA" ,DbType.String,ParameterDirection.InputOutput),
												   new DAABRequest.Parameter("PO_CODIGO_RESPUESTA" ,DbType.String,ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA" ,DbType.String,ParameterDirection.Output)
											   };


            for (var i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objPortConfiguracion.PORTV_EST_PROCESO;
            arrParam[1].Value = objPortConfiguracion.PORTV_MOTIVO;
            arrParam[2].Value = objPortConfiguracion.PORTV_FLAG_ACREDITA;
            arrParam[3].Value = objPortConfiguracion.PORTV_OPERADOR;
            arrParam[4].Value = objPortConfiguracion.PORTC_TIPO_PRODUCTO;
            arrParam[5].Value = objPortConfiguracion.PORTV_TIPO_VENTA;
            arrParam[6].Value = objPortConfiguracion.PORTV_APLICACION;
            arrParam[7].Value = objPortConfiguracion.PORTV_MOD_VENTA;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SISACTSS_PORTT_VALIDA_ABDCP";
            obRequest.Parameters.AddRange(arrParam);
            IDataReader dr = null;

            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                result[0] = Funciones.CheckStr((((IDataParameter)obRequest.Parameters[obRequest.Parameters.Count - 2]).Value));
                result[1] = Funciones.CheckStr((((IDataParameter)obRequest.Parameters[obRequest.Parameters.Count - 1]).Value));
                result[2] = Funciones.CheckStr((((IDataParameter)obRequest.Parameters[obRequest.Parameters.Count - 3]).Value));
            }
            catch (Exception ex)
            {
                result[0] = "-1";
                result[1] = ex.Message;
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return result;
            //PROY-31393 FIN
        }

        //INI: PROY-140223 IDEA-140462
        public void Actualizar_SEC_sin_CP(BeConsultaPrevia objConsultaPrevia, ref string rstrCodResp, ref string rstrMensResp)
        {

            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_SOPON_SOLIN_CODIGO", DbType.Int64,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_SOPOV_USUARIO_MODIFIC", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String,ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String,ParameterDirection.Output),
											   };


            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objConsultaPrevia.numeroSEC;
            arrParam[1].Value = objConsultaPrevia.auditoria.Codigo2;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD_MIGRA + ".SISASU_ACTUALIZA_SEC_SINCP";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                rstrCodResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[2]).Value);
                rstrMensResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[3]).Value);
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
    }

        public bool Envio_mesa_portabilidad_sin_cp(List<BENumeroPortabilidad> p_aSPsPort, ref string rMsg)
        {
            bool salida = false;
            string observaciones = "Enviado a Mesa de Portabilidad desde Pool Sec Emitidas";
            string OPERADOR_RECEPTOR = Funciones.CheckStr(System.Configuration.ConfigurationManager.AppSettings["constCodClaroPortabilidad"].ToString());
            string TIPO_PORTABILIDAD = Funciones.CheckStr(System.Configuration.ConfigurationManager.AppSettings["constTipoPortabilidadPortIN"].ToString());
            string MODO_ENVIO = Funciones.CheckStr(System.Configuration.ConfigurationManager.AppSettings["constModoEnvioOnline"].ToString());

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SP_INSERT_SOLICITUD_CP";

            obRequest.Transactional = true;
            try
            {
                foreach (BENumeroPortabilidad oSPort in p_aSPsPort)
                {
                    DAABRequest.Parameter[] arrParam = {
														   new DAABRequest.Parameter("K_RESULTADO", DbType.Double, ParameterDirection.Output),
														   new DAABRequest.Parameter("P_SOPOV_OBSERVACIONES", DbType.String, 80, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPOC_CODIGO_RECEPTOR", DbType.String, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPOC_CODIGO_CEDENTE", DbType.String, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPOC_TIPO_DOCUMENTO", DbType.String, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPOV_NUM_DOCUMENTO", DbType.String, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPON_CANTIDAD_NUM", DbType.Int32, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPON_SOLIN_CODIGO", DbType.Int32, ParameterDirection.Input),														   
														   new DAABRequest.Parameter("P_SOPOC_INICIO_RANGO", DbType.String, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPOC_FINAL_RANGO", DbType.String, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPOV_NOM_RASO_ABONAD", DbType.String, 80, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPOC_TIPO_PORTA", DbType.String, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPOC_MODALIDAD", DbType.String, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPOC_MODO_ENVIO", DbType.String, ParameterDirection.Input),
														   new DAABRequest.Parameter("P_SOPOV_USUARIO_CREA", DbType.String, 50, ParameterDirection.Input)
													   };
                    int i = 0;
                    for (i = 0; i < arrParam.Length; i++)
                        arrParam[i].Value = DBNull.Value;

                    i = 1; if (observaciones != "") { arrParam[i].Value = observaciones; }
                    ++i; if (OPERADOR_RECEPTOR != "") { arrParam[i].Value = OPERADOR_RECEPTOR; }
                    ++i; if (oSPort.PORT_OPERADORCEDENTE != "") { arrParam[i].Value = oSPort.PORT_OPERADORCEDENTE; }
                    ++i; if (oSPort.PORT_TIPO_DOCUMENTO != "") { arrParam[i].Value = oSPort.PORT_TIPO_DOCUMENTO; }
                    ++i; if (oSPort.PORT_NUM_DOC != "") { arrParam[i].Value = oSPort.PORT_NUM_DOC; }
                    ++i; if (oSPort.PORT_CANTIDAD_NUM != 0) { arrParam[i].Value = oSPort.PORT_CANTIDAD_NUM; }
                    ++i; if (oSPort.SOLIN_CODIGO != 0) { arrParam[i].Value = oSPort.SOLIN_CODIGO; }
                    ++i; if (oSPort.INICIO_RANGO != "") { arrParam[i].Value = oSPort.INICIO_RANGO; }
                    ++i; if (oSPort.FINAL_RANGO != "") { arrParam[i].Value = oSPort.FINAL_RANGO; }
                    ++i; if (oSPort.NOM_RASO_ABONAD != "") { arrParam[i].Value = oSPort.NOM_RASO_ABONAD; }
                    ++i; if (TIPO_PORTABILIDAD != "") { arrParam[i].Value = TIPO_PORTABILIDAD; }
                    ++i; if (oSPort.PORT_MODALIDAD != "") { arrParam[i].Value = oSPort.PORT_MODALIDAD; }
                    ++i; if (MODO_ENVIO != "") { arrParam[i].Value = MODO_ENVIO; }
                    ++i; if (oSPort.USUARIO_CREA != "") { arrParam[i].Value = oSPort.USUARIO_CREA; }

                    obRequest.Parameters.Clear();
                    obRequest.Parameters.AddRange(arrParam);
                    obRequest.Factory.ExecuteNonQuery(ref obRequest);
                }
                obRequest.Factory.CommitTransaction();
                salida = true;

                IDataParameter pSalida;
                pSalida = (IDataParameter)obRequest.Parameters[0];
                rMsg = Funciones.CheckStr(pSalida.Value);
            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                rMsg = "Error al Insertar Lista de Solicitudes de Portabilidad en Mesa de Portabilidad. \nMensaje : " + ex.Message;
                throw ex;
            }
            finally
            {
                obRequest.Factory.Dispose();
    }
            return salida;
        }

        public string Obtener_Class_PDV(string pClasificacion)
        {
      

            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("P_CLASIFICACION", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("P_CONSULTA", DbType.Object, ParameterDirection.Output)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = pClasificacion;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), pClasificacion.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CONSULTA_GENNERAL + ".SISACTS_CLASS_PDV";
            objRequest.Parameters.AddRange(arrParam);

            DataTable dt = null;
            string strOvenCod = string.Empty;
             try
             {
                 dt = objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
                 foreach (DataRow dr in dt.Rows)
                 {
                     strOvenCod  = string.Format("{0}|{1}",strOvenCod, Funciones.CheckStr(dr["OVENC_CODIGO"]));
                     
                   
                 }
             }
             catch (Exception ex)
             {
                  strOvenCod = "";
                 throw ex;
             }
             finally
             {
                 if (dt != null) dt.Clear();
                 objRequest.Parameters.Clear();
                 objRequest.Factory.Dispose();
                
             }
             return strOvenCod;
         }
        //FIN: PROY-140223 IDEA-140462

        //INI PROY-CAMPANA LG
        public void validaVentaCampanaLG(string numDoc, string tipoDoc, string codCampana, string paranGrupo, ref string strCodResp, ref string strMensResp)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("PI_CLIEC_NUM_DOC", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_CLIEC_TIP_DOC", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_COD_CAMPANA", DbType.String, ParameterDirection.Input),
                new DAABRequest.Parameter("PI_PARAN_GRUPO", DbType.String, ParameterDirection.Input),
				new DAABRequest.Parameter("PO_CODIGO_RPTA", DbType.String, ParameterDirection.Output),
                new DAABRequest.Parameter("PO_MENSAJE_RPTA", DbType.String, ParameterDirection.Output)
			};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = numDoc;
            arrParam[1].Value = tipoDoc;
            arrParam[2].Value = codCampana;
            arrParam[3].Value = paranGrupo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_PORTA_CAMP_LG + ".SISACTSS_VALID_VENTA_CAMP_LG";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                strCodResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[4]).Value);
                strMensResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[5]).Value);
            }
            catch 
            {
                strCodResp = "-1";
                strMensResp = string.Empty;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }
        //FIN PROY-CAMPANA LG
        
        #region PROY-140335 IDEA-140307
        public BEValidalineaPorta ValidarDisponibilidadLinea(string listaTelefono)
        {
            BEValidalineaPorta DtoLineaPorta = new BEValidalineaPorta();
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_INICIO_RANGO", DbType.String,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PO_SOLIN_CODIGO", DbType.String, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_COD_RESPUESTA", DbType.String, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MSJ_RESPUESTA", DbType.String, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_ANULACION", DbType.String, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_OFICINA",DbType.String,ParameterDirection.Output)
											   };

            arrParam[0].Value = DBNull.Value;
            arrParam[0].Value = listaTelefono;
            DtoLineaPorta.StrLinea = listaTelefono;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SISACTSS_VALIDA_LINEA_PORTA";
            obRequest.Parameters.AddRange(arrParam);
            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                DtoLineaPorta.SolinCodigo = Funciones.CheckInt(((IDataParameter)obRequest.Parameters[1]).Value);
                DtoLineaPorta.CodigoRespuesta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[2]).Value);
                DtoLineaPorta.MsjRespuesta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[3]).Value);
                DtoLineaPorta.EstadoAnulacion = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[4]).Value);
                DtoLineaPorta.strCodigoSinergia = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[5]).Value);

            }
            catch (Exception Ex)
            {
                DtoLineaPorta.MsjRespuesta = Ex.ToString() + " - " + "Error en el metodo [ValidarDisponibilidadLinea]";
                DtoLineaPorta.CodigoRespuesta = "-1";
                DtoLineaPorta.EstadoAnulacion = "-1";
                DtoLineaPorta.strCodigoSinergia = "";
                throw Ex;
            }
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return DtoLineaPorta;
        }


        public string AnularSecPortabilidad(int Sec)
        {
            string StrResul = "";
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_SOLIN_CODIGO", DbType.Int64,  ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PO_COD_RESPUESTA", DbType.String, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MSJ_RESPUESTA", DbType.String, ParameterDirection.Output) };

            arrParam[0].Value = DBNull.Value;
            arrParam[0].Value = Sec;
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest(new StackTrace(true));
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SISACTSU_ANULA_SEC_PORTA";
            obRequest.Parameters.AddRange(arrParam);
            obRequest.Transactional = true;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
                StrResul = string.Format("{0}|{1}", Funciones.CheckStr(((IDataParameter)obRequest.Parameters[1]).Value), Funciones.CheckStr(((IDataParameter)obRequest.Parameters[2]).Value));
            }
            catch (Exception ex)
            {
                StrResul = "Error|" + ex.ToString();
                obRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }

            return StrResul;
        }
        #endregion

        //INI PROY-140335 RF1
        public BEPorttSolicitud ValidarRepositorioABDCP(BEPorttSolicitud objConsultaPrevia, ref string strCodRespuesta, ref string strMsgRespuesta) //(BeConsultaPrevia objConsultaPrevia)
        {
            BEPorttSolicitud objPortabilidad = new BEPorttSolicitud();
            

            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("PI_TIPO_DOC", DbType.String,  ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_NUMERO_DOC", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_OPERADOR_CEDENTE", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_MODALIDAD_CEDENTE", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_LINEA", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PO_FLAG_CP", DbType.String, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("PO_CURSALIDA", DbType.Object, ParameterDirection.Output)
											   };


            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objConsultaPrevia.TipoDocumento;
            arrParam[1].Value = objConsultaPrevia.NroDocumento;
            arrParam[2].Value = objConsultaPrevia.operadorCedente;
            arrParam[3].Value = objConsultaPrevia.modalidadOrigen;
            arrParam[4].Value = objConsultaPrevia.numeroLinea;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_PORTABILIDAD + ".SISACTSS_REPOSITORIO_ABDCP"; //;".SISACTSS_REPOSITORIO_ABDCP";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;

            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objPortabilidad.idPortabilidad = Funciones.CheckInt64(dr["PK_SOPOT_SOPON_ID"]);
                    objPortabilidad.numeroSEC = Funciones.CheckInt64(dr["SOPON_SOLIN_CODIGO"]);
                    objPortabilidad.numeroLinea = Funciones.CheckStr(dr["SOPOC_INICIO_RANGO"]);
                    objPortabilidad.fechaActivacionCP = string.IsNullOrEmpty(Funciones.CheckStr(dr["SOPOD_FEC_ACTIVACION"])) ? string.Empty : Funciones.CheckDate(dr["SOPOD_FEC_ACTIVACION"]).ToString("dd/MM/yyyy");
                    objPortabilidad.fechaEnvioCP = string.IsNullOrEmpty(Funciones.CheckStr(dr["SOLID_ENVIO_CP"])) ? null : Funciones.CheckDate(dr["SOLID_ENVIO_CP"]).ToString("dd/MM/yyyy");
                    objPortabilidad.flagEnvioCP = Funciones.CheckStr(dr["SOLIC_FLAG_ENVIO_CP"]);
                    objPortabilidad.codigoEstadoCP = Funciones.CheckStr(dr["SOPOC_ESTA_PROCESO_CP"]);
                    objPortabilidad.descripcionEstadoCP = Funciones.CheckStr(dr["ESTADO_CP"]);
                    objPortabilidad.codigoMotivocP = Funciones.CheckStr(dr["SOPOV_MOTIVO_CP"]);
                    objPortabilidad.descripcionMotivoCP = Funciones.CheckStr(dr["SOPOV_MOTIVO_CP_DES"]);
                    objPortabilidad.deudaCP = Funciones.CheckStr(dr["SOPON_DEUDA"]);
                    objPortabilidad.numeroIntentosCP = Funciones.CheckInt(dr["SOPON_NUMERO_INTNETOS"]);
                    objPortabilidad.secuencialCP = Funciones.CheckStr(dr["NUM_SECUENCIAL"]);

                }
                    objPortabilidad.flagConsultaPrevia = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[5]).Value);
                    strCodRespuesta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[6]).Value);
                    strMsgRespuesta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[7]).Value);
            }
            catch (Exception ex)
            {
                strMsgRespuesta = ex.Message;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return objPortabilidad;
        }
        //FIN PROY-140335 RF1
    }

}
