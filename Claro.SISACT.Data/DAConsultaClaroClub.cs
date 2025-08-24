using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Claro.SISACT.Common;
using Claro.SISACT.Entity;
using Claro.SISACT.IData;

namespace Claro.SISACT.Data
{
    public class DAConsultaClaroClub
    {
        public static void ValidaBloqueoBolsa(string k_tipo_doc,
           string k_num_doc,
           string k_tipo_clie,
           ref string k_tipo_doc2,
           ref string k_estado,
           ref double k_coderror,
           ref string k_descerror)
        {
            DataTable dtResultado = new DataTable();
            DAABRequest.Parameter[] arrParam = {
												   new DAABRequest.Parameter("K_TIPO_DOC", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("K_NUM_DOC", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("K_TIPO_CLIE", DbType.String, ParameterDirection.Input),
												   new DAABRequest.Parameter("K_TIPO_DOC2", DbType.String, ParameterDirection.Output),
												   new DAABRequest.Parameter("K_ESTADO", DbType.String, ParameterDirection.Output),
												   new DAABRequest.Parameter("K_CODERROR", DbType.Double, ParameterDirection.Output),
												   new DAABRequest.Parameter("K_DESCERROR", DbType.String, ParameterDirection.Output)
											   };
            for (int j = 0; j < arrParam.Length; j++)
                arrParam[j].Value = System.DBNull.Value;

            arrParam[0].Value = k_tipo_doc;
            arrParam[1].Value = k_num_doc;
            arrParam[2].Value = k_tipo_clie;

            BDCLAROCLUB obj = new BDCLAROCLUB(BaseDatos.BdPuntosCC);
            DAABRequest obRequest = obj.CreaRequest();
            obRequest.CommandType = CommandType.StoredProcedure;
            obRequest.Command = BaseDatos.PKG_CC_TRANSACCION + ".ADMPS_VALBLOQUEOBOLSA";
            obRequest.Parameters.AddRange(arrParam);

            IDataReader dr = null;
            try
            {
                dr = obRequest.Factory.ExecuteReader(ref obRequest).ReturnDataReader;

                IDataParameter parTIPO_DOC2;
                IDataParameter parESTADO;
                IDataParameter parCODERROR;
                IDataParameter parDESCERROR;

                parTIPO_DOC2 = (IDataParameter)obRequest.Parameters[3];
                parESTADO = (IDataParameter)obRequest.Parameters[4];
                parCODERROR = (IDataParameter)obRequest.Parameters[5];
                parDESCERROR = (IDataParameter)obRequest.Parameters[6];

                k_tipo_doc2 = Funciones.CheckStr(parTIPO_DOC2.Value);
                k_estado = Funciones.CheckStr(parESTADO.Value);
                k_coderror = Funciones.CheckDbl(parCODERROR.Value);
                k_descerror = Funciones.CheckStr(parDESCERROR.Value);
                obRequest.Factory.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dr != null && dr.IsClosed == false)
                {
                    dr.Close();
                }
                obRequest.Parameters.Clear();
                obRequest.Factory.Dispose();
            }
        }
    }
}
