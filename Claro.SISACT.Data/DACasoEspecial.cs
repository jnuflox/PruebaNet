//PROY-32129 :: INI
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;
using System.Diagnostics;
namespace Claro.SISACT.Data
{
    public class DACasoEspecial
    {
        public List<BEItemGenerico> ListarInstituciones()
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("pout_respuesta_codigo", DbType.Int64,ParameterDirection.Output),
                new DAABRequest.Parameter("pout_respuesta_mensaje", DbType.String,ParameterDirection.Output),
                new DAABRequest.Parameter("pout_respuesta_cursor", DbType.Object,ParameterDirection.Output)
			};
            arrParam[0].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CAMPANA_ESPECIAL + ".sisacss_listado_instituciones";
            objRequest.Parameters.AddRange(arrParam);
            objRequest.SaveLog = false;

            List<BEItemGenerico> objLista = new List<BEItemGenerico>();
            BEItemGenerico objItem = null;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                while (dr.Read())
                {
                    objItem = new BEItemGenerico();
                    objItem.Codigo = Funciones.CheckStr(dr["INSTN_ID"]);
                    objItem.Descripcion = Funciones.CheckStr(dr["INSTV_DESCRIPCION"]);
                    objLista.Add(objItem);                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                objRequest.Factory.Dispose();
            }
            return objLista;
        }



        public bool GrabarDatosAlumno(string strTipoDocumento, string strNumeroDocumento, Int64 intCodInstitucion, string strCodPersona, string strUsuario, ref Int64 intCodResp, ref string strMensResp)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("pin_tipo_documento", DbType.String,2,ParameterDirection.Input),								
				new DAABRequest.Parameter("pin_nro_documento", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("pin_cod_institucion", DbType.Int64, ParameterDirection.Input),
                new DAABRequest.Parameter("pin_cod_persona", DbType.String,20, ParameterDirection.Input),
				new DAABRequest.Parameter("pin_usuario", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("pout_respuesta_codigo", DbType.Int64, ParameterDirection.Output),
                new DAABRequest.Parameter("pout_respuesta_mensaje", DbType.String, ParameterDirection.Output)
			};

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strTipoDocumento;
            arrParam[1].Value = strNumeroDocumento;
            arrParam[2].Value = intCodInstitucion;
            arrParam[3].Value = strCodPersona;
            arrParam[4].Value = strUsuario;
           
            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CAMPANA_ESPECIAL + ".sisacsiu_log_whitelist";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                intCodResp = Funciones.CheckInt64(((IDataParameter)objRequest.Parameters[5]).Value);
                strMensResp = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[6]).Value);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return true;
        }

        public bool AnularDatosAlumno(string strTipoDocumento, string strNumeroDocumento, string strUsuario, ref Int64 intCodResp, ref string strMensResp)
        {
            DAABRequest.Parameter[] arrParam = {
                new DAABRequest.Parameter("pin_tipo_documento", DbType.String,2,ParameterDirection.Input),								
				new DAABRequest.Parameter("pin_nro_documento", DbType.String, 20, ParameterDirection.Input),
				new DAABRequest.Parameter("pin_usuario", DbType.String, 20, ParameterDirection.Input),
                new DAABRequest.Parameter("pout_respuesta_codigo", DbType.Int64, ParameterDirection.Output),
				new DAABRequest.Parameter("pout_respuesta_mensaje", DbType.String, ParameterDirection.Output)				
			};

            int i = 0;
            for (i = 0; i < arrParam.Length; i++)
                arrParam[i].Value = DBNull.Value;

            arrParam[0].Value = strTipoDocumento;
            arrParam[1].Value = strNumeroDocumento;
            arrParam[2].Value = strUsuario;
            arrParam[3].Value = DBNull.Value;
            arrParam[4].Value = DBNull.Value;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true));
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SISACT_CAMPANA_ESPECIAL + ".sisacsd_log_whitelist";
            objRequest.Parameters.AddRange(arrParam);

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);
                intCodResp = Funciones.CheckInt64(((IDataParameter)objRequest.Parameters[3]).Value);
                strMensResp = Funciones.CheckStr(((IDataParameter)objRequest.Parameters[4]).Value);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return true;
        }       
        //PROY FASE 2 INICIO EIQ
        public bool WhiteList_x_IdCampana(string strNroDocumento, ref Int64 intCodResp, ref string strMensResp)
        {
            bool estadoConsulta = false;
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("pin_id_campana", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("pout_respuesta_codigo", DbType.Int64, ParameterDirection.Output),
                                                   new DAABRequest.Parameter("pout_respuesta_mensaje", DbType.String,400, ParameterDirection.Output)                                                  
											   };
            arrParam[0].Value = strNroDocumento;

            BDSISACT obj = new BDSISACT(BaseDatos.BdSisact);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_SISACT_CAMPANA_ESPECIAL + ".sisacss_flag_campana";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;

            try
            {
                obRequest.Factory.ExecuteNonQuery(ref obRequest);

                intCodResp = Funciones.CheckInt64(((IDataParameter)obRequest.Parameters[1]).Value);
                strMensResp = Funciones.CheckStr(((IDataParameter)obRequest.Parameters[2]).Value);
                if (intCodResp == 1)
                {
                    estadoConsulta = true;
                }
            }
            catch (Exception ex)
            {
                intCodResp = -1;
                strMensResp = "Error al ejecutar el PKG_SISACT_CAMPANA_ESPECIAL.sisacss_flag_campana: " + ex.Message.ToString();
            }
            finally
            {
                if (dr != null && dr.IsClosed == false) dr.Close();
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
            return estadoConsulta;
        }
        //PROY FASE 2 FIN EIQ
    }
}
//PROY-32129 :: FIN