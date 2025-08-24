using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Configuration;

namespace Claro.SISACT.Common
{
    public class HelperLog
    {
        public static void EscribirLog(string vRuta, string vNombreFila, string vMsg, bool vSobreScribir)
        {
            //PROY-140126 H8 -> INICIO
            GeneradorLog_Generico glog = new GeneradorLog_Generico();
            //PROY-140126 H8 -> FIN

            if (ConfigurationManager.AppSettings["flag_log"] == "1")
            {
                if (vMsg != null && vMsg.ToUpper().IndexOf("ERROR") > -1)
                {

                    try
                    {           
                        //PROY-140126 H8 -> INICIO
                        String str = DateTime.Now + " - " + vMsg;
                        glog.EscribirLog(str.ToString());
                        //PROY-140126 H8 -> FIN
                    }
                    catch (Exception ex)
                    {
                        throw ex;
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
                //PROY-140126 H8 -> INICIO
                String str = "[" + vNombreFila + "][" + DateTime.Now + "] - " + vMsg;//INC000004280198
                
                glog.EscribirLog(str.ToString());
                //PROY-140126 H8 -> FIN
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               
            }
        }
        }

        private static bool CheckLoggingEnabled()
        {
            string strLoggingStatusConfig = string.Empty;

            strLoggingStatusConfig = GetLoggingStatusConfigFileName();
            if (strLoggingStatusConfig.Equals(string.Empty))
                return false;
            //If it's empty then enable the logging status 
            if (strLoggingStatusConfig.Equals(string.Empty))
            {
                return true;
            }
            else
            {

                //Read the value from xml and set the logging status
                bool bTemp = false;
                string loggingEnabled = getValue("LoggingEnabled");
                if (loggingEnabled.Equals("0"))
                {
                    bTemp = false;
                }
                else if (loggingEnabled.Equals("1"))
                {
                    bTemp = true;
                }
                return bTemp;
            }
        }

        private static string GetLoggingStatusConfigFileName()
        {
            string strCheckinBaseDirecotry = AppDomain.CurrentDomain.BaseDirectory + "SisAct.config";

            if (File.Exists(strCheckinBaseDirecotry))
                return strCheckinBaseDirecotry;
            else
            {
                string strCheckinApplicationDirecotry = GetApplicationPath() + "SisAct.config";
                if (File.Exists(strCheckinApplicationDirecotry))
                    return strCheckinApplicationDirecotry;
                else
                    return "";
            }
        }

        private static string GetPathLog()
        {
            string strBaseDirectory = GetApplicationPath();
            try
            {
                // creamos la carpeta log
                string carpeta = "AppLog";
                string ruta = strBaseDirectory + carpeta;
                if (Directory.Exists(ruta) == false)
                {
                    Directory.CreateDirectory(strBaseDirectory + "\\" + carpeta);
                }
                return ruta;

            }
            catch (Exception)
            {
                return strBaseDirectory;
            }
        }

        private static string GetApplicationPath()
        {
            try
            {
                string strBaseDirectory = AppDomain.CurrentDomain.BaseDirectory.ToString();
                int nFirstSlashPos = strBaseDirectory.LastIndexOf("\\");
                string strTemp = string.Empty;

                if (0 < nFirstSlashPos)
                    strTemp = strBaseDirectory.Substring(0, nFirstSlashPos);

                int nSecondSlashPos = strTemp.LastIndexOf("\\");
                string strTempAppPath = string.Empty;
                if (0 < nSecondSlashPos)
                    strTempAppPath = strTemp.Substring(0, nSecondSlashPos);

                string strAppPath = strTempAppPath.Replace("bin", "");
                if (strAppPath == "")
                    strAppPath = strBaseDirectory;
                return strAppPath;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string getValue(string vClave)
        {
            string strXmlPath = GetLoggingStatusConfigFileName();
            if (strXmlPath == "") { return ""; }
            try
            {
                //Open a FileStream on the Xml file
                FileStream docIn = new FileStream(strXmlPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlDocument contactDoc = new XmlDocument();
                //Load the Xml Document
                contactDoc.Load(docIn);
                //Get a node
                XmlNodeList UserList = contactDoc.GetElementsByTagName(vClave);
                //get the value
                string strGetValue = UserList.Item(0).InnerText.ToString();
                return strGetValue;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return "";
            }
        }
    }
}
