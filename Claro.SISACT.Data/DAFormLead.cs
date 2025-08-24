using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Data;
using Claro.SISACT.Common;

namespace Claro.SISACT.Data
{
    public class DAFormLead
    {
        //INI PROY-140739 Formulario Leads
        public bool RegistrarFormularioLeads(BEFormLead objLead, ref string oNroError, ref string oDescError)
        {
            oNroError = "-1";
            oDescError = string.Empty;

            DAABRequest.Parameter[] arrParam = {
													new DAABRequest.Parameter("PI_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("PI_COD_LEADS",DbType.String, ParameterDirection.Input),
													new DAABRequest.Parameter("PI_PEDIN_CODIGO", DbType.Int64, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("PI_COD_OFICINA", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("PI_DES_OFICINA", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("PI_USUARIO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("PO_COD_RESPUESTA", DbType.String, ParameterDirection.Output),
                                                    new DAABRequest.Parameter("PO_MSJ_RESPUESTA", DbType.String,1000, ParameterDirection.Output)
												};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = objLead.SOALN_SOLIN_CODIGO;
            arrParam[1].Value = objLead.SOALC_COD_LEAD;
            arrParam[2].Value = objLead.SOALN_PEDIN_CODIGO;
            arrParam[3].Value = objLead.SOALV_COD_OFICINA;
            arrParam[4].Value = objLead.SOALV_DES_OFICINA;
            arrParam[5].Value = objLead.SOALV_USU_CREACION;
            arrParam[6].Value = oNroError;
            arrParam[7].Value = oDescError;

            bool salida = false;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_SOLICITUD_CONS + ".SISACTSI_FORMULARIO_LEADS";
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                oNroError = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[6]).Value);
                oDescError = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[7]).Value);

                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                obRequest.Factory.Dispose();
            }
            return salida;
        }

        public bool RechazarSEC(Int64 solinCodigo, string estacCcodigo, string usuario, string comentario, ref string oNroError, ref string oDescError) 
        {
            oNroError = "-1";
            oDescError = string.Empty;

            DAABRequest.Parameter[] arrParam = {
													new DAABRequest.Parameter("PI_SOLIN_CODIGO", DbType.Int64, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("PI_ESTAC_CODIGO",DbType.String, ParameterDirection.Input),
													new DAABRequest.Parameter("PI_USUARIO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("PI_COMENTARIO", DbType.String, ParameterDirection.Input),
                                                    new DAABRequest.Parameter("PO_COD_RESPUESTA", DbType.String, ParameterDirection.Output),
                                                    new DAABRequest.Parameter("PO_MSJ_RESPUESTA", DbType.String,1000, ParameterDirection.Output)
												};

            for (int i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = solinCodigo;
            arrParam[1].Value = estacCcodigo;
            arrParam[5].Value = usuario;
            arrParam[4].Value = comentario;            
            arrParam[6].Value = oNroError;
            arrParam[7].Value = oDescError;

            bool salida = false;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_EVALUACION_CONS_2 + ".SISACTSU_RECHAZAR_SEC";
            obRequest.Parameters.AddRange(arrParam);

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);
                oNroError = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[6]).Value);
                oDescError = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[7]).Value);

                salida = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                obRequest.Factory.Dispose();
            }
            return salida; 
        }
        //FIN PROY-140739 Formulario Leads
    }
}
