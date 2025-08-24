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
    public class DAPrepago
    {
        public List<BELineaAbonado> ListarLineasAbonado(string tipoDocumento, string nroDocumento)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("v_tipo_doc", DbType.String,200, ParameterDirection.Input),
				new DAABRequest.Parameter("v_nro_doc", DbType.String,200, ParameterDirection.Input),				
				new DAABRequest.Parameter("p_cursor", DbType.Object, ParameterDirection.Output),
				new DAABRequest.Parameter("v_resultado", DbType.String, 100, ParameterDirection.Output),
				new DAABRequest.Parameter("v_msg_error", DbType.String, 100, ParameterDirection.Output)
			};
            arrParam[0].Value = tipoDocumento;
            arrParam[1].Value = nroDocumento;

            BDDWH obj = new BDDWH(BaseDatos.BdDWH);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_DWH_ABONADOS_CLARO + ".sp_lista_abonado_recupero";
            objRequest.Parameters.AddRange(arrParam);

            List<BELineaAbonado> objLista = new List<BELineaAbonado>();
            BELineaAbonado objLineaAbonado;
            IDataReader dr = null;
            try
            {
                dr = objRequest.Factory.ExecuteReader(ref objRequest).ReturnDataReader;
                
                
                    while(dr.Read())
                    {
                        objLineaAbonado = new BELineaAbonado();
                        objLineaAbonado.nro_telefono = Funciones.CheckStr(dr["NRO_TELEFONO"]);
                        objLineaAbonado.tipo_documento = Funciones.CheckStr(dr["TIPO_DOCUMENTO"]);
                        objLineaAbonado.nro_documento = Funciones.CheckStr(dr["NRO_DOCUMENTO"]);
                        objLineaAbonado.nombres = Funciones.CheckStr(dr["NOMBRES"]);
                        objLineaAbonado.apellidos = Funciones.CheckStr(dr["APELLIDOS"]);
                        objLineaAbonado.fecha_activacion = Funciones.CheckStr(dr["FECHA_ACTIVACION"]);
                        objLineaAbonado.plan_tarifario = Funciones.CheckStr(dr["PLAN_TARIFARIO"]);
                        objLineaAbonado.segmento = Funciones.CheckStr(dr["SEGMENTO"]);
                        objLineaAbonado.estado = Funciones.CheckStr(dr["ESTADO"]);
                        objLineaAbonado.create_date = Funciones.CheckStr(dr["CREATE_DATE"]);
                        objLista.Add(objLineaAbonado);
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

        public string ValidarBloqueoLinea(string nroDocumento, string telefono)
        {
            DAABRequest.Parameter[] arrParam = {
				new DAABRequest.Parameter("p_phone", DbType.String,ParameterDirection.Input),
				new DAABRequest.Parameter("txtbloqueo", DbType.String,ParameterDirection.Output)
			};
            arrParam[0].Value = telefono;

            BDSIAC obj = new BDSIAC(BaseDatos.BdSiac);
            DAABRequest objRequest = obj.CreaRequest(new StackTrace(true), nroDocumento);
            objRequest.CommandType = CommandType.StoredProcedure;
            objRequest.Command = BaseDatos.PKG_SIAC_GENERICO + ".valida_bloqueo_linea_detalle";
            objRequest.Parameters.AddRange(arrParam);
            string resultado = "";

            try
            {
                objRequest.Factory.ExecuteNonQuery(ref objRequest);

                IDataParameter parSalida;
                parSalida = (IDataParameter)objRequest.Parameters[1];
                resultado = Funciones.CheckStr(parSalida.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objRequest.Factory.Dispose();
            }
            return resultado;
        }
    }
}
