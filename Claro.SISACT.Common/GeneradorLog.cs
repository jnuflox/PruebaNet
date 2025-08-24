using System;
using System.Configuration;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Data;
using Claro.SISACT.Entity;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace Claro.SISACT.Common
{
    public class GeneradorLog
    {
        private readonly static string strPath = ConfigurationManager.AppSettings["strDirectorioLogSISACT"].ToString();

        private string _usuario;
        private string _idIdentificador;
        private string _idTransaccion;
        private string _archivo;
        private string _archivoCompleto;
        public bool _flgUsuarioActual;

        public GeneradorLog(string usuario, string idIdentificador, string idIdTansaccion, string archivo)
        {
            if (archivo == null || archivo == "")
            {
                archivo = "SISACT_CONSUMER";
            }

            _usuario = usuario;
            _idIdentificador = idIdentificador;
            _idTransaccion = idIdTansaccion;
            _archivo = archivo;

            if (usuario == null && HttpContext.Current.Session["Usuario"] != null)
            {
                BEUsuarioSession ojbUsuario = (BEUsuarioSession)HttpContext.Current.Session["Usuario"];
                _usuario = ojbUsuario.idCuentaRed;
            }

            if (_usuario != null && BEGlobal.usuarioConsulta == _usuario)
            {
                _flgUsuarioActual = true;
                _archivo += "_" + _usuario;
            }
                
            _archivoCompleto = string.Format("{0}\\{1}_{2}.{3}", strPath, _archivo, DateTime.Now.ToString("dd-MM-yyyy"), "log");    

          
        }

        public void CrearArchivologSap(string evento, string rfc, object obj, Exception objException)
        {
            //PROY-140126 H8-> INI
            String strArchivo = "[" + _archivo + "]";
            //PROY-140126 H8-> FIN
            StringBuilder sblog = new StringBuilder();
            StringBuilder sblogDetalle = new StringBuilder();
            string strIdGeneral = string.Empty;
            string strId = string.Empty;
            if (ConfigurationManager.AppSettings["flag_log"] == "1")
            {

                if (objException != null)
                {
                    try
                    {
                        if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                        if (string.IsNullOrEmpty(_idIdentificador))
                            strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                        else
                            strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                        if (evento != null)
                        {
                            sblog.AppendLine(strArchivo + strId + evento); //PROY-140126 H8->
                        }

                        if (rfc != null)
                        {
                            sblog.AppendLine(strArchivo + string.Format("{0}[RFC][{1}]", strId, rfc)); //PROY-140126 H8->
                        }

                        sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, obj);
                        sblog.Append(sblogDetalle.ToString());

                       //PROY-140126 H8-> INI
                        GeneradorLog_Generico glog = new GeneradorLog_Generico();
                        glog.EscribirLog(sblog.ToString());
                        //PROY-140126 H8-> FIN
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                       
                    }
                }
            }

            else
            {
            try
            {
                if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                if (string.IsNullOrEmpty(_idIdentificador))
                    strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                else
                    strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                if (evento != null)
                {
                        sblog.AppendLine(strArchivo + strId + evento);  //PROY-140126 H8->
                }

                if (rfc != null)
                {
                        sblog.AppendLine(strArchivo + string.Format("{0}[RFC][{1}]", strId, rfc)); //PROY-140126 H8->
                }

                sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, obj);
                sblog.Append(sblogDetalle.ToString());

                    //PROY-140126 H8-> INI
                    GeneradorLog_Generico glog = new GeneradorLog_Generico();
                    glog.EscribirLog(sblog.ToString());
                    //PROY-140126 H8-> FIN
            }
            catch (Exception)
            {

            }
            finally
            {
                  
            }
        }
        }
        public void CrearArchivologWS(string evento, string url, object obj, Exception objException)
        {
            //PROY-140126 H8-> INI
            String strArchivo = "[" + _archivo + "]";
            //PROY-140126 H8-> FIN
            StringBuilder sblog = new StringBuilder();
            StringBuilder sblogDetalle = new StringBuilder();
            string strIdGeneral = string.Empty;
            string strId = string.Empty;
            if (ConfigurationManager.AppSettings["flag_log"] == "1")
            {
                if (objException != null)
                {
                    try
                    {
                        if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                        if (string.IsNullOrEmpty(_idIdentificador))
                            strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                        else
                            strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                        if (evento != null)
                        {
                            sblog.AppendLine(strArchivo + strId + evento);//PROY-140126 H8->
                        }

                        if (url != null)
                        {
                            sblog.AppendLine(strArchivo + string.Format("{0}[WS][{1}]", strId, url));//PROY-140126 H8->
                        }

                        sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, obj);
                        sblog.Append(sblogDetalle.ToString());

                        //PROY-140126 H8-> INI
                        GeneradorLog_Generico glog = new GeneradorLog_Generico();
                        glog.EscribirLog(sblog.ToString());
                        //PROY-140126 H8-> FIN
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                      
                    }
                }
            }

            else
            {
            try
            {
                if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                if (string.IsNullOrEmpty(_idIdentificador))
                    strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                else
                    strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                if (evento != null)
                {
                        sblog.AppendLine(strArchivo + strId + evento); //PROY-140126 H8->
                }

                if (url != null)
                {
                        sblog.AppendLine(strArchivo + string.Format("{0}[WS][{1}]", strId, url));//PROY-140126 H8->
                }

                sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, obj);
                sblog.Append(sblogDetalle.ToString());

                    //PROY-140126 H8-> INI
                    GeneradorLog_Generico glog = new GeneradorLog_Generico();
                    glog.EscribirLog(sblog.ToString());
                    //PROY-140126 H8-> FIN
            }
            catch (Exception)
            {

            }
            finally
            {
                   
            }
        }
        }
        public void CrearArchivolog(string evento, object obj, Exception objException)
        {
            //PROY-140126 H8-> INI
            String strArchivo = "[" + _archivo + "]";
            //PROY-140126 H8-> FIN

            StringBuilder sblog = new StringBuilder();
            StringBuilder sblogDetalle = new StringBuilder();
            string strIdGeneral = string.Empty;
            string strId = string.Empty;

            if (ConfigurationManager.AppSettings["flag_log"] == "1")
            {
                if (objException != null)
                {
            try
            {
                if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                if (string.IsNullOrEmpty(_idIdentificador))
                    strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                else
                    strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                if (evento != null)
                {
                            sblog.AppendLine(strArchivo + strId + evento);//PROY-140126 H8->
                }

                sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, obj);
                if (sblogDetalle.Length > 0)
                            sblog.Append(sblogDetalle.ToString());//PROY-140126 H8-> MEJORA JSQ strArchivo +

                        //PROY-140126 H8-> INI
                        GeneradorLog_Generico glog = new GeneradorLog_Generico();
                        glog.EscribirLog(sblog.ToString());
                        //PROY-140126 H8-> FIN
            }
            catch (Exception)
            {

            }
            finally
            {
                    
            }
        }
            }

            else
            {
                try
                {
                    if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                    if (string.IsNullOrEmpty(_idIdentificador))
                        strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                    else
                        strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                    strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                    if (evento != null)
                    {
                        sblog.AppendLine(strArchivo + strId + evento);//PROY-140126 H8->  
                    }

                    sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, obj);
                    if (sblogDetalle.Length > 0)
                        sblog.Append(sblogDetalle.ToString());

                    //PROY-140126 H8-> INI
                    GeneradorLog_Generico glog = new GeneradorLog_Generico();
                    glog.EscribirLog(sblog.ToString());
                    //PROY-140126 H8-> FIN
                }
                catch (Exception)
                {

                }
                finally
                {
                    
                }
            }
        }

        public void CrearArchivolog(string evento, List<String> lstLog, Exception objException)
        {
            //PROY-140126 H8->
            String strArchivo = "[" + _archivo + "]";
            //PROY-140126 H8->
            StringBuilder sblog = new StringBuilder();
            StringBuilder sblogDetalle = new StringBuilder();
            string strIdGeneral = string.Empty;
            string strId = string.Empty;

            if (ConfigurationManager.AppSettings["flag_log"] == "1")
            {
                if (objException != null)
                {
                    try
                    {
                        if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                        if (string.IsNullOrEmpty(_idIdentificador))
                            strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                        else
                            strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                        if (evento != null)
                        {
                            sblog.AppendLine(strArchivo + strId + evento);//PROY-140126 H8->
                        }

                        if (lstLog != null)
                        {
                            foreach (string strLog in lstLog)
                            {
                                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                                sblog.AppendLine(strArchivo + strId + strLog);//PROY-140126 H8->
                            }
                        }

                        sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, null);
                        sblog.Append(sblogDetalle.ToString());

                        //PROY-140126 H8-> INI
                        GeneradorLog_Generico glog = new GeneradorLog_Generico();
                        glog.EscribirLog(sblog.ToString());
                        //PROY-140126 H8-> FIN

                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                       
                    }
                }
            }

            else
            {
            try
            {
                if (_idTransaccion == null) _idTransaccion = DateTime.Now.ToString("hhmmssfff");

                if (string.IsNullOrEmpty(_idIdentificador))
                    strIdGeneral = string.Format("[{0}][{1}]-", _idTransaccion, _usuario);
                else
                    strIdGeneral = string.Format("[{0}][{1}][{2}]-", _idTransaccion, _usuario, _idIdentificador);

                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);

                if (evento != null)
                {
                        sblog.AppendLine(strArchivo + strId + evento);//PROY-140126 H8->
                }

                if (lstLog != null)
                {
                    foreach (string strLog in lstLog)
                    {
                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                            sblog.AppendLine(strArchivo + strId + strLog);//PROY-140126 H8->
                    }
                }

                sblogDetalle = EstructuraLog(strIdGeneral, strId, objException, null);
                sblog.Append(sblogDetalle.ToString());

                    //PROY-140126 H8-> INI
                    GeneradorLog_Generico glog = new GeneradorLog_Generico();
                    glog.EscribirLog(sblog.ToString());
                    //PROY-140126 H8-> FIN
            }
            catch (Exception)
            {

            }
            finally
            {
                   
            }
        }
        }

        //private StringBuilder EstructuraLog(string strIdGeneral, string strId, List<String> lstLog, Exception objException, object obj)
        private StringBuilder EstructuraLog(string strIdGeneral, string strId, Exception objException, object obj)
        {
            StringBuilder sblog = new StringBuilder();
            //PROY-140126 H8-> INI
            String strArchivo = "[" + _archivo + "]";
            //PROY-140126 H8-> FIN
            try
            {
                if (objException != null)
                {
                    strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                    sblog.AppendLine(strArchivo + strId + "Message = " + objException.Message);//PROY-140126 H8->
                    strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                    sblog.AppendLine(strArchivo + strId + "GetType = " + objException.GetType().ToString());//PROY-140126 H8->
                    strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                    sblog.AppendLine(strArchivo + strId + "Source  = " + (String.IsNullOrEmpty(objException.Source) ? String.Empty : objException.Source));//PROY-140126 H8->

                    if (objException.TargetSite != null)
                    {
                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                        sblog.AppendLine(strArchivo + strId + "TargetSite = " + objException.TargetSite.ToString());//PROY-140126 H8->
                    }
                    if (objException.StackTrace != null)
                    {
                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                        sblog.AppendLine(strArchivo + strId + "StackTrace = " + objException.StackTrace.ToString());//PROY-140126 H8->
                    }
                }

                if (obj != null)
                {
                    Type tyobjt = obj.GetType();

                    if (tyobjt.Name == "String")
                    {
                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                        sblog.Append(strArchivo + strId);//PROY-140126 H8->
                        sblog.AppendLine(obj.ToString());//PROY-140126 H8->
                    }
                    else if (tyobjt.Name == "DataSet")
                    {
                        int canttables = ((DataSet)(obj)).Tables.Count;

                        for (int i = 0; i < canttables; i++)
                        {
                            DataTable dt = (DataTable)((DataSet)(obj)).Tables[i];

                            strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                            sblog.Append(strArchivo + strId);//PROY-140126 H8->
                            sblog.AppendLine(string.Format("[Tabla_{0}][nroRegistro={1}]", i, dt.Rows.Count));//PROY-140126 H8-> MEJORA JSQ strArchivo + 

                            foreach (DataRow row in dt.Rows)
                            {
                                strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                                sblog.Append(strArchivo + strId);//PROY-140126 H8->

                                foreach (DataColumn column in dt.Columns)
                                {
                                    sblog.Append(string.Format("[{0}={1}]", column.ColumnName, row[column].ToString()));//PROY-140126 H8->
                                }
                                sblog.AppendLine(" ");
                            }
                        }
                    }
                    else if (tyobjt.Name == "DataTable")
                    {
                        DataTable dt = (DataTable)obj;
                        foreach (DataRow row in dt.Rows)
                        {
                            strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                            sblog.Append(strArchivo + strId);//PROY-140126 H8->

                            foreach (DataColumn column in dt.Columns)
                            {
                                sblog.Append(string.Format("[{0}={1}]", column.ColumnName, row[column].ToString()));//PROY-140126 H8->
                            }
                            sblog.AppendLine(" ");
                        }
                    }
                    else if (tyobjt.Namespace == "Claro.SISACT.Entity")
                    {
                        String atributo = string.Empty;
                        System.Reflection.PropertyInfo[] propiedades = tyobjt.GetProperties();
                        object objvalue;
                        PropertyInfo objInfo;

                        strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                        sblog.Append(strArchivo + strId);//PROY-140126 H8->

                        foreach (System.Reflection.PropertyInfo propiedad in propiedades)
                        {
                            atributo = propiedad.Name;
                            objInfo = obj.GetType().GetProperty(atributo);
                            objvalue = objInfo.GetValue(obj, null);

                            if (!(objInfo.PropertyType.Namespace == "Claro.SISACT.Entity" || objInfo.PropertyType.Namespace == "System.Collections.Generic" ||
                                objInfo.PropertyType.Name == "ArrayList" || objInfo.PropertyType.Name == "DataTable" || objInfo.PropertyType.Name == "DataSet"))
                            {
                                sblog.Append(string.Format("[{0}={1}]", propiedad.Name, Funciones.CheckStr(objvalue)));//PROY-140126 H8->strArchivo + 
                            }
                        }
                        sblog.AppendLine(" ");
                    }
                    else if (tyobjt.Namespace == "System.Collections.Generic")
                    {
                        int cantidadfilas = (int)tyobjt.GetProperty("Count").GetValue(obj, null);

                        for (int i = 0; i < cantidadfilas; i++)
                        {
                            object[] index = { i };
                            object objEntidad = obj.GetType().GetProperty("Item").GetValue(obj, index);
                            PropertyInfo[] objpropentidad = objEntidad.GetType().GetProperties();

                            strId = string.Format("[{0}]{1}", DateTime.Now.ToString("hh:mm:ss"), strIdGeneral);
                            sblog.Append(strArchivo + strId); //PROY-140126 H8->

                            foreach (PropertyInfo propiedad in objpropentidad)
                            {
                                object propertyValue = propiedad.GetValue(objEntidad, null);

                                if (!(propiedad.PropertyType.Namespace == "System.Collections.Generic" || propiedad.PropertyType.Name == "ArrayList" ||
                                    propiedad.PropertyType.Name == "DataTable" || propiedad.PropertyType.Name == "DataSet"))
                                {
                                    sblog.Append(string.Format("[{0}={1}]", propiedad.Name, propertyValue));//PROY-140126 H8->strArchivo + 
                                }
                            }
                            sblog.AppendLine(" ");
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return sblog;
        }

        public static void EscribirLog(String strNombreArchivoLog, string idLog, params object[] values)
        {
           
            try
            {
               
                String strFecha = DateTime.Now.ToString("hh:mm:ss fff");
               
                string strDirectorioLog = ConfigurationManager.AppSettings["strDirectorioLogSISACT"];
                string strTexto = "";
                for (int i = 0; i < values.Length; i++)
                {
                    strTexto += "[" + Funciones.CheckStr(values[i]) + "]";
                }

                String applicationPath = HttpContext.Current.Request.ApplicationPath;
                applicationPath = HttpContext.Current.Server.MapPath(applicationPath);



                //PROY-140126 H8-> INI
                GeneradorLog_Generico glog = new GeneradorLog_Generico();
                
                String str = "[" + strNombreArchivoLog + "]" + "[" + strFecha + "]" + idLog + " - " + strTexto + Environment.NewLine;
                glog.EscribirLog(str);


                //PROY-140126 H8-> FIN

            }
            catch (Exception)
            {
            }
            finally
            {
               
            }
        }
    }
}
