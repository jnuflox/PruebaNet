using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class DASolicitudDatos
    {
        public DASolicitudDatos() { }
        GeneradorLog objLog = new GeneradorLog("", string.Empty, null, "log_GrabarLineasTipi");//[PROY-140600] 
        //PROY-24740
         public ArrayList ObtenerConsultaSolicitudCons(string nroSEC, string tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String,2,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String,20,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.String,20,ParameterDirection.Input),
												   new DAABRequest.Parameter("K_CUR_SALIDA", DbType.Object,ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            if (tipoDocumento != "") arrParam[0].Value = tipoDocumento;
            if (nroDocumento != "") arrParam[1].Value = nroDocumento;
            if (nroSEC != "") arrParam[2].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_SOLICITUD_CONS + ".SECSS_CON_SOL_CONS";
            objRequest.Parameters.AddRange(arrParam);

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEConsultaSolicitud item = new BEConsultaSolicitud();
                    item.SOLIN_CODIGO = Funciones.CheckStr(dr["SOLIN_CODIGO"]);
                    item.SEGMV_DESCRIPCION = Funciones.CheckStr(dr["SEGMV_DESCRIPCION"]);
                    item.OVENV_DESCRIPCION = Funciones.CheckStr(dr["OVENV_DESCRIPCION"]);
                    item.ESTAV_DESCRIPCION = Funciones.CheckStr(dr["ESTAV_DESCRIPCION"]);
                    item.TVENV_DESCRIPCION = Funciones.CheckStr(dr["TVENV_DESCRIPCION"]);
                    item.SOLID_FEC_REG = Funciones.CheckDate(dr["SOLID_FEC_REG"]);
                    item.SOLID_FEC_APR = Funciones.CheckDate(dr["SOLID_FEC_APR"]);
                    if (Funciones.CheckDate(dr["SOLID_FEC_APR"]) != DateTime.MinValue)
                        item.SOLID_FEC_APR_STR = Funciones.CheckStr(Funciones.CheckDate(dr["SOLID_FEC_APR"]));
                    else
                        item.SOLID_FEC_APR_STR = "";
                    item.CLIEV_NOMBRE = Funciones.CheckStr(dr["CLIEV_NOMBRE"]);
                    item.CLIEV_APE_PAT = Funciones.CheckStr(dr["CLIEV_APE_PAT"]);
                    item.CLIEV_APE_MAT = Funciones.CheckStr(dr["CLIEV_APE_MAT"]);
                    item.CLIEV_RAZ_SOC = Funciones.CheckStr(dr["CLIEV_RAZ_SOC"]);
                    item.TDOCC_CODIGO = Funciones.CheckStr(dr["TDOCC_CODIGO"]);
                    item.CLIEC_NUM_DOC = Funciones.FormatoNroDocumento(Funciones.CheckStr(dr["TDOCC_CODIGO"]), Funciones.CheckStr(dr["CLIEC_NUM_DOC"]));
                    item.ESTAC_CODIGO = Funciones.CheckStr(dr["ESTAC_CODIGO"]);
                    item.MRECC_CODIGO = Funciones.CheckStr(dr["MRECC_CODIGO"]);
                    item.MRECV_DESCRIPCION = Funciones.CheckStr(dr["MRECV_DESCRIPCION"]);
                    item.SOLIC_FLA_TER = Funciones.CheckStr(dr["SOLIC_FLA_TER"]);
                    item.TEVAC_CODIGO = Funciones.CheckStr(dr["TEVAC_CODIGO"]);
                    item.TACTC_CODIGO = Funciones.CheckStr(dr["TACTC_CODIGO"]);
                    item.TACTV_DESCRIPCION = Funciones.CheckStr(dr["TACTV_DESCRIPCION"]);
                    item.TCARV_DESCRIPCION = Funciones.CheckStr(dr["TCARV_DESCRIPCION"]);
                    item.SOLIN_IMP_DG = Funciones.CheckStr(dr["SOLIN_IMP_DG"]);
                    item.RDIRC_CODIGO = Funciones.CheckStr(dr["RDIRC_CODIGO"]);
                    item.RDIRV_DESCRIPCION = Funciones.CheckStr(dr["RDIRV_DESCRIPCION"]);
                    item.MRDIV_CAD_CODIGO = Funciones.CheckStr(dr["MRDIV_CAD_CODIGO"]);
                    item.TIPO_GARANTIA_DES = Funciones.CheckStr(dr["TCARV_DESCRIPCION_MAN"]);
                    item.CANTIDAD_CARGOS_FIJOS = Funciones.CheckDbl(dr["SOLIN_NUM_CAR_FIJ"]);
                    item.SOLIC_TIPO_EVALUACION = Funciones.CheckStr(dr["SOLIC_TIPO_EVALUACION"]);
                    item.TDOCV_DESCRIPCION = Funciones.CheckStr(dr["TDOCV_DESCRIPCION"]);
                    item.SOLIV_MOTIVO_RECHAZO = Funciones.CheckStr(dr["SOLIV_MOTIVO_RECHAZO"]);
                    //item.TCESC_DESCRIPCION = Funciones.CheckStr(dr["TCESC_DESCRIPCION"]);

                    //INICIO - E75688	  
                    //*******************Se comento por DESIN 33870-1	   	**************************//	
                    //if (item.CLIEV_NOMBRE != "" && item.CLIEV_APE_PAT != "")
                    //item.CLIEV_RAZ_SOC = item.CLIEV_NOMBRE + " " + item.CLIEV_APE_PAT + " " + item.CLIEV_APE_MAT ;
                    //FIN - E75688					

                    //T12618 - Portabilidad - INICIO
                    item.FLAG_PORTABILIDAD = Funciones.CheckStr(dr["FLAG_PORTABILIDAD"]);
                    //T12618 - Portabilidad - FIN

                    item.ESTAC_CODIGO = Funciones.CheckStr(dr["ESTAC_CODIGO"]);
                    item.SOLIC_COD_APROB = Funciones.CheckStr(dr["SOLIC_COD_APROB"]);

                    //INICIO - E75688
                    item.CLIEV_FLAG_CORREO = Funciones.CheckStr(dr["CLIEV_FLAG_CORREO"]);
                    item.CLIEV_CORREO = Funciones.CheckStr(dr["CLIEV_CORREO"]);
                    item.CLIEV_TEL_SMS = Funciones.CheckStr(dr["CLIEV_TEL_SMS"]);
                    item.CLIED_FEC_NAC = Funciones.CheckDate(dr["CLIED_FEC_NAC"]);
                    item.CLIEV_EST_CIV = Funciones.CheckStr(dr["CLIEV_EST_CIV"]);
                    item.TITULO_PERSONA_COD = Funciones.CheckStr(dr["TITULO_COD"]);
                    //FIN - E75688
                    item.DEPAC_CODIGO = Funciones.CheckStr(dr["DEPAC_CODIGO"]);
                    item.TPROD_COMERCIALIZAR = Funciones.CheckStr(dr["TPROD_COMERCIALIZAR"]);

                    item.PRDV_DESCRIPCION = Funciones.CheckStr(dr["PRDV_DESCRIPCION"]);
                    item.SOLIN_SUM_CAR_CON = Funciones.CheckDbl(dr["SOLIN_SUM_CAR_CON"]);
                    item.TPROV_DESCRIPCION = Funciones.CheckStr(dr["TOFV_DESCRIPCION"]);
                    item.PRDC_CODIGO = Funciones.CheckStr(dr["PRDC_CODIGO"]);

                    filas.Add(item);
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        public bool ModificacionEvaluacionNaturalConsumer(BEVistaSolicitud objVistaSolicitud)
        {

            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("K_RESULTADO" ,DbType.Int32,ParameterDirection.Output),
												   new DAABRequest.Parameter("P_SOLIN_CODIGO" ,DbType.Int32,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_FLA_TER" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_PRE_TEL_LEG" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_TEL_LEG" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_PRE_DIR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_DIRECCION" ,DbType.String,2000,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_REF_DIR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_DEP_DIR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_PRO_DIR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_DIS_DIR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_POS_DIR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_PRE_DIR_FAC" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_DIR_FAC" ,DbType.String,2000,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_REF_DIR_FAC" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_DEP_FAC" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_PRO_FAC" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_DIS_FAC" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_POS_FAC" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_PRE_DIR_TRA" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_DIR_TRA" ,DbType.String,2000, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_REF_DIR_TRA" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_DEP_TRA" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_PRO_TRA" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_DIS_TRA" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_COD_POS_TRA" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_TEL_REF_1" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_TEL_REF_2" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEV_TEL_FIJ_TRA" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIN_LIM_CRE_FIN" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_SCO_TXT_FIN" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIN_SCO_NUM_FIN" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIN_LIM_MAX_FIN" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_POSTCONTROL" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_RDIRC_CODIGO" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_RFINC_CODIGO" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_MRECC_CODIGO" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIN_IMP_DG_MAN" ,DbType.Double,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_TIP_CAR_MAN" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIV_COM_DG" ,DbType.String,200,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TEVAC_CODIGO" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ESTAC_CODIGO" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIV_DES_EST" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_USU_CRE" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TAFIC_CODIGO" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_COD_TAR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_COD_TIP_MON" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIV_NUM_TAR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIV_NOM_TIT_TAR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIV_DOC_TIT_TAR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIV_FEC_VEN_TAR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIN_MNTOMAX" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIV_NUM_OPE_FIN" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TOPEN_CODIGO" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_RUCEMPLEADOR" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_NOMBREEMPRESA" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TACTC_CODIGO" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIC_COD_APROB" ,DbType.String,ParameterDirection.Input),
                                                                                                   //INICIO - E75688
												   new DAABRequest.Parameter("P_CLIED_FEC_NAC" ,DbType.DateTime,ParameterDirection.Input),
												   new DAABRequest.Parameter("p_CLIEV_EST_CIV" ,DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TITUC_CODIGO" ,DbType.String,ParameterDirection.Input),
												   //FIN  -E75688
 												   new DAABRequest.Parameter("P_SOLIV_FLAG_CORR", DbType.String, 1, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SOLIV_CORREO", DbType.String, 200, ParameterDirection.Input)
											   };

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = 0;
            arrParam[1].Value = objVistaSolicitud.solin_codigo;
            arrParam[2].Value = objVistaSolicitud.solic_fla_ter;
            arrParam[3].Value = objVistaSolicitud.cliev_pre_tel_leg;
            arrParam[4].Value = objVistaSolicitud.cliev_tel_leg;
            arrParam[5].Value = objVistaSolicitud.cliev_pre_dir;
            arrParam[6].Value = objVistaSolicitud.cliev_direccion;
            arrParam[7].Value = objVistaSolicitud.cliev_ref_dir;
            arrParam[8].Value = objVistaSolicitud.cliec_cod_dep_dir;
            arrParam[9].Value = objVistaSolicitud.cliec_cod_pro_dir;
            arrParam[10].Value = objVistaSolicitud.cliec_cod_dis_dir;
            arrParam[11].Value = objVistaSolicitud.cliec_cod_pos_dir;
            arrParam[12].Value = objVistaSolicitud.cliev_pre_dir_fac;
            arrParam[13].Value = objVistaSolicitud.cliev_dir_fac;
            arrParam[14].Value = objVistaSolicitud.cliev_ref_dir_fac;
            arrParam[15].Value = objVistaSolicitud.cliec_cod_dep_fac;
            arrParam[16].Value = objVistaSolicitud.cliec_cod_pro_fac;
            arrParam[17].Value = objVistaSolicitud.cliec_cod_dis_fac;
            arrParam[18].Value = objVistaSolicitud.cliec_cod_pos_fac;
            arrParam[19].Value = objVistaSolicitud.cliev_pre_dir_tra;
            arrParam[20].Value = objVistaSolicitud.cliev_dir_tra;
            arrParam[21].Value = objVistaSolicitud.cliev_ref_dir_tra;
            arrParam[22].Value = objVistaSolicitud.cliec_cod_dep_tra;
            arrParam[23].Value = objVistaSolicitud.cliec_cod_pro_tra;
            arrParam[24].Value = objVistaSolicitud.cliec_cod_dis_tra;
            arrParam[25].Value = objVistaSolicitud.cliec_cod_pos_tra;
            arrParam[26].Value = objVistaSolicitud.cliev_tel_ref_1;
            arrParam[27].Value = objVistaSolicitud.cliev_tel_ref_2;
            arrParam[28].Value = objVistaSolicitud.cliev_tel_fij_tra;
            arrParam[29].Value = objVistaSolicitud.solin_lim_cre_fin;
            arrParam[30].Value = objVistaSolicitud.solic_sco_txt_fin;
            arrParam[31].Value = objVistaSolicitud.solin_sco_num_fin;
            arrParam[32].Value = objVistaSolicitud.solin_lim_max_fin;
            arrParam[33].Value = objVistaSolicitud.solic_postcontrol;
            arrParam[34].Value = objVistaSolicitud.rdirc_codigo;
            arrParam[35].Value = objVistaSolicitud.rfinc_codigo;
            arrParam[36].Value = objVistaSolicitud.mrecc_codigo;
            arrParam[37].Value = objVistaSolicitud.solin_imp_dg_man;
            arrParam[38].Value = objVistaSolicitud.solic_tip_car_man;
            arrParam[39].Value = objVistaSolicitud.soliv_com_dg;
            arrParam[40].Value = objVistaSolicitud.tevac_codigo;
            arrParam[41].Value = objVistaSolicitud.estac_codigo;
            arrParam[42].Value = objVistaSolicitud.soliv_des_est;
            arrParam[43].Value = objVistaSolicitud.solic_usu_cre;
            arrParam[44].Value = objVistaSolicitud.tafic_codigo;
            arrParam[45].Value = objVistaSolicitud.solic_cod_tar;
            arrParam[46].Value = objVistaSolicitud.solic_cod_tip_mon;
            arrParam[47].Value = objVistaSolicitud.soliv_num_tar;
            arrParam[48].Value = objVistaSolicitud.soliv_nom_tit_tar;
            arrParam[49].Value = objVistaSolicitud.soliv_doc_tit_tar;
            arrParam[50].Value = objVistaSolicitud.soliv_fec_ven_tar;
            arrParam[51].Value = objVistaSolicitud.solin_mntomax;
            arrParam[52].Value = objVistaSolicitud.soliv_num_ope_fin;
            arrParam[53].Value = objVistaSolicitud.topen_codigo;
            arrParam[54].Value = objVistaSolicitud.rucempleador;
            arrParam[55].Value = objVistaSolicitud.nombreempresa;
            arrParam[56].Value = objVistaSolicitud.TACTC_CODIGO;
            arrParam[57].Value = objVistaSolicitud.SOLIC_COD_APROB;
            //INICIO - E75688
            arrParam[58].Value = objVistaSolicitud.CLIED_FEC_NAC;
            arrParam[59].Value = objVistaSolicitud.CLIEV_EST_CIV;
            arrParam[60].Value = objVistaSolicitud.TITULO_PERSONA_COD;
            //FIN - E75688
            arrParam[61].Value = objVistaSolicitud.CLIEV_FLAG_CORREO;
            arrParam[62].Value = objVistaSolicitud.CLIEV_CORREO;

            bool salida = false;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_SOLICITUD_CONS + ".SECSU_ACT_SOL_NAT_CONS";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
                salida = true;
            }
            catch (Exception ex)
            {
                objRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public bool InsertarSolDireccion(BEDireccionCliente oDireccion, Int64 nroSEC)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_RESULTADO", DbType.Int64, ParameterDirection.Output),
												   new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TIPO_DIR", DbType.String, 1, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_PREFIJO", DbType.String, 10, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_PREFIJO", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_DIRECCION", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_NRO_PUERTA", DbType.String, 5, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_EDIFICACION", DbType.String, 4, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_EDIFICACION", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_MANZANA", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_LOTE", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_INTERIOR", DbType.String, 4, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TIPO_INTERIOR", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_NRO_INTERIOR", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_URBANIZACION", DbType.String, 4, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_URBANIZACION", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TXT_URBANIZACION", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_DOMICILIO", DbType.String, 4, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_DOMICILIO", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_ZONA", DbType.String, 4, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ZONA", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_NOMBRE_ZONA", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_REFERENCIA", DbType.String, 40, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_REFERENCIA_SEC", DbType.String, 250, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_DEPARTAMENTO", DbType.String, 5, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_PROVINCIA", DbType.String, 5, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_DISTRITO", DbType.String, 5, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_POSTAL", DbType.String, 10, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_UBIGEO", DbType.String, 10, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_TELEFONO", DbType.String, 5, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TELEFONO", DbType.String, 9, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_POBLADO", DbType.String, 20, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_ID_PLANO", DbType.String, 20, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_DIR_COMPLETA", DbType.String, 200, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_DIRECCION_SAP", DbType.String, 200, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_REFERENCIA_SAP", DbType.String, 200, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TELEFONO_REF_1", DbType.String, 9, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_TELEFONO_REF_2", DbType.String, 9, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_VENTA_PROACTIVA", DbType.String, 2, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_DNI_VENDEDOR", DbType.String, 10, ParameterDirection.Input)
											   };
            bool salida = false;
            int i;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            i = 1; arrParam[i].Value = nroSEC;
            i++; arrParam[i].Value = oDireccion.IdTipoDireccion;
            i++; arrParam[i].Value = oDireccion.IdPrefijo;
            i++; arrParam[i].Value = oDireccion.Prefijo;
            i++; arrParam[i].Value = oDireccion.Direccion;
            i++; arrParam[i].Value = oDireccion.NroPuerta;
            i++; arrParam[i].Value = oDireccion.IdEdificacion;
            i++; arrParam[i].Value = oDireccion.Edificacion;
            i++; arrParam[i].Value = oDireccion.Manzana;
            i++; arrParam[i].Value = oDireccion.Lote;
            i++; arrParam[i].Value = oDireccion.IdTipoInterior;
            i++; arrParam[i].Value = oDireccion.TipoInterior;
            i++; arrParam[i].Value = oDireccion.NroInterior;
            i++; arrParam[i].Value = oDireccion.IdUrbanizacion;
            i++; arrParam[i].Value = oDireccion.Urbanizacion;
            i++; arrParam[i].Value = oDireccion.TxtUrbanizacion;
            i++; arrParam[i].Value = oDireccion.IdDomicilio;
            i++; arrParam[i].Value = oDireccion.Domicilio;
            i++; arrParam[i].Value = oDireccion.IdZona;
            i++; arrParam[i].Value = oDireccion.Zona;
            i++; arrParam[i].Value = oDireccion.NombreZona;
            i++; arrParam[i].Value = oDireccion.Referencia;
            i++; arrParam[i].Value = oDireccion.Referencia_Sec;
            i++; arrParam[i].Value = oDireccion.IdDepartamento;
            i++; arrParam[i].Value = oDireccion.IdProvincia;
            i++; arrParam[i].Value = oDireccion.IdDistrito;
            i++; arrParam[i].Value = oDireccion.IdPostal;
            i++; arrParam[i].Value = oDireccion.IdUbigeo;
            i++; arrParam[i].Value = oDireccion.IdTelefono;
            i++; arrParam[i].Value = oDireccion.Telefono;
            i++; arrParam[i].Value = oDireccion.IdCentroPoblado;
            i++; arrParam[i].Value = oDireccion.IdPlano;
            i++; arrParam[i].Value = oDireccion.DirCompleta;
            i++; arrParam[i].Value = oDireccion.DirCompletaSAP;
            i++; arrParam[i].Value = oDireccion.DirReferenciaSAP;
            i++; arrParam[i].Value = oDireccion.TelefonoReferencia1;
            i++; arrParam[i].Value = oDireccion.TelefonoReferencia2;
            i++; arrParam[i].Value = oDireccion.VentaProactiva;
            i++; arrParam[i].Value = oDireccion.DniVendedor;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroSEC.ToString());
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_INS_SOL_DIRECCION";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.Transactional = true;
            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                objRequest.Factory.CommitTransaction();
                salida = true;
            }
            catch (Exception ex)
            {
                objRequest.Factory.RollBackTransaction();
                salida = false;
                throw ex;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public ArrayList ListarPromociones(string pstrSoplnCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_SOPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
											   };
            ArrayList filas;
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = pstrSoplnCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".MANTSS_LISTAR_PROMOCION";
            objRequest.Parameters.AddRange(arrParam);

            filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico oItem = new BEItemGenerico();
                    oItem.Codigo2 = Funciones.CheckStr(dr["sopln_codigo"]);
                    oItem.Codigo = Funciones.CheckStr(dr["promn_codigo"]);
                    oItem.Descripcion = Funciones.CheckStr(dr["promocion"]);
                    filas.Add(oItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        //public ArrayList ConsultarSolDireccion(Int64 nroSEC)
        //{
        //    ArrayList filas;
        //    DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
        //                                        new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input)};
        //    arrParam[1].Value = nroSEC;

        //    BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
        //    DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
        //    objRequest.CommandType = CommandType.StoredProcedure;
        //    objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_CON_SOL_DIRECCION";
        //    objRequest.Parameters.AddRange(arrParam);

        //    filas = new ArrayList();
        //    IDataReader dr = null;
        //    try
        //    {
        //        dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
        //        while (dr.Read())
        //        {
        //            BEDireccionCliente oDireccion = new BEDireccionCliente();
        //            oDireccion.IdTipoDireccion = Funciones.CheckStr(dr["TIPO_DIR"]);
        //            oDireccion.IdPrefijo = Funciones.CheckStr(dr["ID_PREFIJO"]);
        //            oDireccion.Prefijo = Funciones.CheckStr(dr["PREFIJO"]);
        //            oDireccion.Direccion = Funciones.CheckStr(dr["DIRECCION"]);
        //            oDireccion.NroPuerta = Funciones.CheckStr(dr["NRO_PUERTA"]);
        //            oDireccion.IdEdificacion = Funciones.CheckStr(dr["ID_EDIFICACION"]);
        //            oDireccion.Edificacion = Funciones.CheckStr(dr["EDIFICACION"]);
        //            oDireccion.Manzana = Funciones.CheckStr(dr["MANZANA"]);
        //            oDireccion.Lote = Funciones.CheckStr(dr["LOTE"]);
        //            oDireccion.IdTipoInterior = Funciones.CheckStr(dr["ID_INTERIOR"]);
        //            oDireccion.TipoInterior = Funciones.CheckStr(dr["TIPO_INTERIOR"]);
        //            oDireccion.NroInterior = Funciones.CheckStr(dr["NRO_INTERIOR"]);
        //            oDireccion.IdUrbanizacion = Funciones.CheckStr(dr["ID_URBANIZACION"]);
        //            oDireccion.Urbanizacion = Funciones.CheckStr(dr["URBANIZACION"]);
        //            oDireccion.TxtUrbanizacion = Funciones.CheckStr(dr["TXT_URBANIZACION"]);
        //            oDireccion.IdDomicilio = Funciones.CheckStr(dr["ID_DOMICILIO"]);
        //            oDireccion.Domicilio = Funciones.CheckStr(dr["DOMICILIO"]);
        //            oDireccion.IdZona = Funciones.CheckStr(dr["ID_ZONA"]);
        //            oDireccion.Zona = Funciones.CheckStr(dr["ZONA"]);
        //            oDireccion.NombreZona = Funciones.CheckStr(dr["NOMBRE_ZONA"]);
        //            oDireccion.Referencia = Funciones.CheckStr(dr["REFERENCIA"]);
        //            oDireccion.Referencia_Sec = Funciones.CheckStr(dr["REFERENCIA_SEC"]);
        //            oDireccion.IdDepartamento = Funciones.CheckStr(dr["ID_DEPARTAMENTO"]);
        //            oDireccion.IdProvincia = Funciones.CheckStr(dr["ID_PROVINCIA"]);
        //            oDireccion.IdDistrito = Funciones.CheckStr(dr["ID_DISTRITO"]);
        //            oDireccion.IdPostal = Funciones.CheckStr(dr["ID_POSTAL"]);
        //            oDireccion.IdUbigeo = Funciones.CheckStr(dr["ID_UBIGEO"]);
        //            oDireccion.IdTelefono = Funciones.CheckStr(dr["ID_TELEFONO"]);
        //            oDireccion.Telefono = Funciones.CheckStr(dr["TELEFONO"]);
        //            oDireccion.IdCentroPoblado = Funciones.CheckStr(dr["ID_POBLADO"]);
        //            oDireccion.IdPlano = Funciones.CheckStr(dr["ID_PLANO"]);
        //            oDireccion.DirCompleta = Funciones.CheckStr(dr["DIR_COMPLETA"]);
        //            oDireccion.IdTipoProducto = Funciones.CheckStr(dr["PRDC_CODIGO"]);
        //            oDireccion.VentaProactiva = Funciones.CheckStr(dr["VENTA_PROACTIVA"]);
        //            oDireccion.DniVendedor = Funciones.CheckStr(dr["DNI_VENDEDOR"]);

        //            filas.Add(oDireccion);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (dr != null && dr.IsClosed == false) dr.Close();
        //        objRequest.Parameters.Clear();
        //        objRequest.Factory.Dispose();
        //    }
        //    return filas;
        //}

        public string ValidarVendedorDNI(string pstrNroDocumento)
        {
            string salida = string.Empty;
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_NRO_DOC", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("K_RESULTADO", DbType.String,ParameterDirection.Output)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = pstrNroDocumento;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_DTH + ".MANTSS_VALI_VEND_DOCU";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteScalar(ref objRequest);
                IDataParameter parSalida1;
                parSalida1 = (IDataParameter)objRequest.Parameters[1];

                salida = Funciones.CheckStr(parSalida1.Value);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return salida;
        }

        public ArrayList ObtenerPlanesCliente(int tipoDoc, string numeroDoc)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_TIPO_DOC", DbType.Int64,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_NUM_DOC", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("P_PLANES", DbType.Object,ParameterDirection.Output)
												   
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = tipoDoc;
            arrParam[1].Value = numeroDoc;

            BDBSCS obj = new BDBSCS(BaseDatos.BdBscs);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.TIM142_PKG_EMPRESAS + ".CONSULTAR_CLIENTE";
            objRequest.Parameters.AddRange(arrParam);

            ArrayList filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEPlan_AP item = new BEPlan_AP();
                    item.PLNV_CODIGO = Funciones.CheckStr(dr["TMCODE"]);
                    item.CARGO_FIJO_BASE = Funciones.CheckFloat(dr["CF"]);
                    item.PLNN_TIPO_PLAN = Funciones.CheckInt(dr["TIPO"]);
                    item.TPROC_CODIGO = Funciones.CheckStr(dr["PRGCODE"]);
                    filas.Add(item);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }
//gaa20140414
        public DataTable ListarEdificio(string pstrCodPlano, string pstrCodEdificio)
        {
            DAABRequest.Parameter[] arrParam = {	
												   new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
												   new DAABRequest.Parameter("P_COD_PLANO", DbType.String, ParameterDirection.Input),	
												   new DAABRequest.Parameter("P_COD_EDIFICIO", DbType.String, ParameterDirection.Input)
											   };

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[1].Value = pstrCodPlano;
            arrParam[2].Value = pstrCodEdificio;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".MANTSS_LISTA_EDIFICIOHFC";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                return objRequest.Factory.ExecuteDataset(ref objRequest).Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
        }

        public List<BEItemGenerico> ListarEquiposHFCxSopln(Int64 pintSoplnCodigo)
        {
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("P_SOPLN_CODIGO", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output)
											   };
            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = pintSoplnCodigo;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".MANTSS_LISTAR_MATE_X_SOLICITUD";
            objRequest.Parameters.AddRange(arrParam);

            List<BEItemGenerico> filas = new List<BEItemGenerico>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico oItem = new BEItemGenerico();
                    //oItem.Codigo2 = Funciones.CheckStr(dr["IDFILA"]);
                    oItem.Codigo = Funciones.CheckStr(dr["IDEQUIPO"]);
                    oItem.Descripcion = Funciones.CheckStr(dr["EQUIPO"]);
                    oItem.Descripcion2 = Funciones.CheckStr(dr["TIPO_MATERIAL"]);
                    oItem.Valor = Funciones.CheckDbl(dr["CF_ALQUILER"]).ToString();
                    filas.Add(oItem);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }

        //PROY-24740
        public List<BEDireccionCliente> ConsultarSolDireccion(Int64 nroSEC)
        {
            List<BEDireccionCliente> filas;
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_CURSOR", DbType.Object, ParameterDirection.Output),
												new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input)};
            arrParam[1].Value = nroSEC;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACT_CON_SOL_DIRECCION";
            objRequest.Parameters.AddRange(arrParam);

            filas = new List<BEDireccionCliente>();
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEDireccionCliente oDireccion = new BEDireccionCliente();
                    oDireccion.IdTipoDireccion = Funciones.CheckStr(dr["TIPO_DIR"]);
                    oDireccion.IdPrefijo = Funciones.CheckStr(dr["ID_PREFIJO"]);
                    oDireccion.IdPrefijoSga = Funciones.CheckStr(dr["ID_PREFIJO_SGA"]);
                    oDireccion.Prefijo = Funciones.CheckStr(dr["PREFIJO"]);
                    oDireccion.Direccion = Funciones.CheckStr(dr["DIRECCION"]);
                    oDireccion.NroPuerta = Funciones.CheckStr(dr["NRO_PUERTA"]);
                    oDireccion.IdEdificacion = Funciones.CheckStr(dr["ID_EDIFICACION"]);
                    oDireccion.Edificacion = Funciones.CheckStr(dr["EDIFICACION"]);
                    oDireccion.Manzana = Funciones.CheckStr(dr["MANZANA"]);
                    oDireccion.Lote = Funciones.CheckStr(dr["LOTE"]);
                    oDireccion.IdTipoInterior = Funciones.CheckStr(dr["ID_INTERIOR"]);
                    oDireccion.TipoInterior = Funciones.CheckStr(dr["TIPO_INTERIOR"]);
                    oDireccion.NroInterior = Funciones.CheckStr(dr["NRO_INTERIOR"]);
                    oDireccion.IdUrbanizacion = Funciones.CheckStr(dr["ID_URBANIZACION"]);
                    oDireccion.IdUrbanizacionSga = Funciones.CheckStr(dr["ID_URB_SGA"]);
                    oDireccion.Urbanizacion = Funciones.CheckStr(dr["URBANIZACION"]);
                    oDireccion.TxtUrbanizacion = Funciones.CheckStr(dr["TXT_URBANIZACION"]);
                    oDireccion.IdDomicilio = Funciones.CheckStr(dr["ID_DOMICILIO"]);
                    oDireccion.Domicilio = Funciones.CheckStr(dr["DOMICILIO"]);
                    oDireccion.IdZona = Funciones.CheckStr(dr["ID_ZONA"]);
                    oDireccion.Zona = Funciones.CheckStr(dr["ZONA"]);
                    oDireccion.NombreZona = Funciones.CheckStr(dr["NOMBRE_ZONA"]);
                    oDireccion.Referencia = Funciones.CheckStr(dr["REFERENCIA"]);
                    oDireccion.Referencia_Sec = Funciones.CheckStr(dr["REFERENCIA_SEC"]);
                    oDireccion.IdDepartamento = Funciones.CheckStr(dr["ID_DEPARTAMENTO"]);
                    oDireccion.IdProvincia = Funciones.CheckStr(dr["ID_PROVINCIA"]);
                    oDireccion.IdDistrito = Funciones.CheckStr(dr["ID_DISTRITO"]);
                    oDireccion.IdPostal = Funciones.CheckStr(dr["ID_POSTAL"]);
                    oDireccion.IdUbigeo = Funciones.CheckStr(dr["ID_UBIGEO"]);
                    oDireccion.IdUbigeoInei = Funciones.CheckStr(dr["UBIGEO_INEI"]);
                    oDireccion.IdUbigeoSGA = Funciones.CheckStr(dr["ID_UBIGEO_SGA"]);
                    oDireccion.IdTelefono = Funciones.CheckStr(dr["ID_TELEFONO"]);
                    oDireccion.Telefono = Funciones.CheckStr(dr["TELEFONO"]);
                    oDireccion.IdCentroPoblado = Funciones.CheckStr(dr["ID_POBLADO"]);
                    oDireccion.IdPlano = Funciones.CheckStr(dr["ID_PLANO"]);
                    oDireccion.DirCompleta = Funciones.CheckStr(dr["DIR_COMPLETA"]);
                    oDireccion.IdTipoProducto = Funciones.CheckStr(dr["PRDC_CODIGO"]);
                    oDireccion.VentaProactiva = Funciones.CheckStr(dr["VENTA_PROACTIVA"]);
                    oDireccion.VentaProgramada = Funciones.CheckStr(dr["VENTA_PROGRAMADA"]);
                    oDireccion.DniVendedor = Funciones.CheckStr(dr["DNI_VENDEDOR"]);
                    oDireccion.DirCompletaSAP = Funciones.CheckStr(dr["DIRECCION_SAP"]);
                    oDireccion.DirReferenciaSAP = Funciones.CheckStr(dr["REFERENCIA_SAP"]);
                    oDireccion.IdEdificio = Funciones.CheckStr(dr["ID_EDIFICIO_LIBERADO"]);

                    filas.Add(oDireccion);
                }
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return filas;
        }
//fin gaa20140414


        // PROY-26358 -Inicio - ConsultaDeclaracionConocimiento - Datos - Evalenzs

        public ArrayList ConsultaDeclaracionConocimiento(String SegmentoVenta) // SegmentoVenta = PREPAGO
        {
            ArrayList filas;
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("p_cur_salida", DbType.Object, ParameterDirection.Output),
												   new DAABRequest.Parameter("p_sdpov_segmento", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("p_res_msge", DbType.String, ParameterDirection.Output),
												   new DAABRequest.Parameter("p_cod_msge", DbType.String, ParameterDirection.Output)};
            arrParam[1].Value = SegmentoVenta;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_DECLARA_PORTA + ".sisactss_declara_porta"; //SISACTSI_SOLI_DECLA_PORTA";
            obRequest.Parameters.AddRange(arrParam);

            filas = new ArrayList();
            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;
                while (dr.Read())
                {
                    BEItemGenerico item = new BEItemGenerico();
                    item.Codigo = Funciones.CheckStr(dr["SDPON_ID"]); //codigo
                    item.Descripcion = Funciones.CheckStr(dr["SDPOV_DESCRIPCION"]); //Descripcion
                    item.Codigo2 = Funciones.CheckStr(dr["SPDON_ORDEN"]); //Orden de los item
                    item.Codigo3= Funciones.CheckStr(dr["SPDON_OBLIGA"]); //Opcional el Item

                    filas.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return filas;
        }

        // PROY-26358 - Fin - Evalenzs


        // PROY-26358 - Inicio - IngresaDeclaracionConocimiento - Datos - Evalenzs

        public bool IngresaDeclaracionConocimiento(int SolinCodigo, String flagItem, String SegmentoVenta, String Usuario)
        {

            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SDPON_ID_FLAG", DbType.String, 200, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SSDPN_SEGMENTO", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_SSDPV_USUARIO_CREACION", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_COD_MSGE", DbType.String, ParameterDirection.Output),
												   new DAABRequest.Parameter("P_RES_MSGE", DbType.String, ParameterDirection.Output)};
            arrParam[0].Value = SolinCodigo;
            arrParam[1].Value = flagItem;
            arrParam[2].Value = SegmentoVenta;
            arrParam[3].Value = Usuario;

            bool salida = false;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = "PKG_SISACT_SOLI_DECLA_PORTA.SISACTSI_SOLI_DECLA_PORTA"; //SISACTSI_SOLI_DECLA_PORTA";
            obRequest.Parameters.AddRange(arrParam);

            obRequest.Transactional = true;
            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
                salida = true;
            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                throw ex;
            }
            finally
            {
                obRequest.Factory.Dispose();
            }
            return salida;
        }

        // PROY-26358 - Fin - Evalenzs

        //PROY-140690 INICIO
        public string ConsultarSolDireccionIfi(string tipoDocumento, string nroDocumento, ref string codRpta, ref string msgRpta)
        {
            string strSecAnteriorIfi = "";
            DAABRequest.Parameter[] arrParam = {new DAABRequest.Parameter("P_TDOCC_CODIGO", DbType.String, 2, ParameterDirection.Input),
												   new DAABRequest.Parameter("P_CLIEC_NUM_DOC", DbType.String, 16, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("P_CURSOR_DIR_OUT", DbType.Object, ParameterDirection.Output),
												   new DAABRequest.Parameter("P_CODIGO_RESPUESTA", DbType.String,1000, ParameterDirection.Output),
												   new DAABRequest.Parameter("P_MENSAJE_RESPUESTA", DbType.String,1000, ParameterDirection.Output)};
            arrParam[0].Value = tipoDocumento;
            arrParam[1].Value = Funciones.FormatoNroDocumentoBD(tipoDocumento, nroDocumento);
            //arrParam[1].Value = nroDocumento;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_GENERAL + ".SISACTSS_ULT_DIRMOV_IFI";

            objRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    strSecAnteriorIfi = Funciones.CheckStr(dr["solin_codigo"]);
                }

                codRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[3]).Value);
                msgRpta = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[4]).Value);
            }
            catch ( Exception Ex)
            {
                strSecAnteriorIfi="";
                codRpta = "-1";
                msgRpta = Ex.Message.ToString();
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Parameters.Clear();
                objRequest.Factory.Dispose();
            }
            return strSecAnteriorIfi;
        }
        //PROY-140690 FIN

        //[PROY-140600] INI 

        public void GrabarInfoContLineasTipi(BEValidarLinea objLineasTipi, ref string codRpta, ref string msjRpta)
        {

            DAABRequest.Parameter[] arrParam = {   new DAABRequest.Parameter("PI_NRO_PEDIDO", DbType.Int64, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_NRO_SEC", DbType.Int64, ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_TIPO_VENTA", DbType.String,ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_TIPO_DOC_CLIENTE", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_NRO_DOC_CLIENTE", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_NRO_LINEAS_ACT", DbType.Int32, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_LISTA_LINEAS_ACT", DbType.String,10000,ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_NRO_LINEAS_NUEVAS", DbType.Int32, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_USUARIO_VENTA", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PI_NOMBRE_USUARIO_VENTA", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("PI_USUARIO", DbType.String, ParameterDirection.Input),
                                                   new DAABRequest.Parameter("PO_CODIGO_RESPUESTA", DbType.String, ParameterDirection.Output),
												   new DAABRequest.Parameter("PO_MENSAJE_RESPUESTA", DbType.String, ParameterDirection.Output)
                                               };

            arrParam[0].Value = Funciones.CheckInt64(string.Empty);
            arrParam[1].Value = objLineasTipi.solin_codigo;
            arrParam[2].Value = objLineasTipi.tipoVenta;
            arrParam[3].Value = objLineasTipi.tdoc_cliente;
            arrParam[4].Value = objLineasTipi.nroDoc_cliente;
            arrParam[5].Value = objLineasTipi.intCantidadLineasActivas;
            arrParam[6].Value = objLineasTipi.strLineasActivas;
            arrParam[7].Value = objLineasTipi.nLineasNuevas;
            arrParam[8].Value = objLineasTipi.CuentaUsuario;
            arrParam[9].Value = objLineasTipi.NombreUsuario;
            arrParam[10].Value = objLineasTipi.usuario;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.SISACT_PKG_TITULARIDAD_CLIENTE + ".SISACTSI_LINEAS_DECLARA_JURADA";
            obRequest.Parameters.AddRange(arrParam);

            obRequest.Transactional = true;
            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                obRequest.Factory.CommitTransaction();
                codRpta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[11]).Value);
                msjRpta = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[12]).Value);
            }
            catch (Exception ex)
            {
                obRequest.Factory.RollBackTransaction();
                objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140600][DASolicitudDatos][GrabarInfoEvalLineasTipi]|ex.StackTrace: ", Funciones.CheckStr(ex.StackTrace)), null, null);
                objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140600][DASolicitudDatos][GrabarInfoEvalLineasTipi]|ex.Message: ", Funciones.CheckStr(ex.Message)), null, null);
            }
            finally
            {
                objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140600][DASolicitudDatos][GrabarInfoEvalLineasTipi][CODIGO_RESPUESTA]|codRpta: ", Funciones.CheckStr(codRpta)), null, null);
                objLog.CrearArchivolog(string.Format("{0} : {1}", "[PROY-140600][DASolicitudDatos][GrabarInfoEvalLineasTipi][MENSAJE_RESPUESTA]|msjRpta: ", Funciones.CheckStr(msjRpta)), null, null);
                obRequest.Factory.Dispose();
            }

        }

        //[PROY-140600] FIN

    }
}
