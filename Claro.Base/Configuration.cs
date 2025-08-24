using System;
using Microsoft.Win32;

namespace Claro.Base
{
    public class Configuration
    {
        private static Seguridad.Criptografia.DES _cryDES;

        private string _strLlaveNombre;

        public Configuration(string llaveNombre)
        {
            if (string.IsNullOrEmpty(llaveNombre))
            {
                throw new Exception("No se ha especificado la llave del registro.");
            }

            this._strLlaveNombre = llaveNombre;

            if (_cryDES == null)
            {
                string strPalabraSecreta =string.Empty;

                if (Environment.Is64BitProcess)
                {
                    strPalabraSecreta = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\TIM").GetValue("key").ToString();
                }
                else
                {
                    strPalabraSecreta = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\TIM").GetValue("key").ToString();
                }

                _cryDES = new Seguridad.Criptografia.DES(strPalabraSecreta);
            }
        }

        #region LeerValor

        private string LeerValor(string valorNombre)
        {
            string strPalabraSecreta = string.Empty;
            if (Environment.Is64BitProcess)
            {                
                return Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\TIM\" + this._strLlaveNombre).GetValue(valorNombre, "").ToString();
            }
            else
            {
                return Registry.LocalMachine.OpenSubKey(@"SOFTWARE\TIM\" + this._strLlaveNombre).GetValue(valorNombre, "").ToString();
            }                        
        }

        #endregion

        #region LeerValorEncriptado

        private string LeerValorEncriptado(string valorNombre)
        {
            string strPassword = this.LeerValor(valorNombre);

            byte[] ByteArray = System.Text.Encoding.Default.GetBytes(strPassword);

            _cryDES.DecryptByte(ref ByteArray);

            return System.Text.Encoding.Default.GetString(ByteArray);
        }

        #endregion

        #region LeerProveedor

        public string LeerProveedor()
        {
            return LeerValor("Provider");
        }

        #endregion

        #region LeerBaseDatos

        public string LeerBaseDatos()
        {
            return LeerValor("BD_Activa");
        }

        #endregion

        #region LeerServidor

        public string LeerServidor()
        {
            return LeerValor("Server");
        }

        #endregion

        #region LeerUsuario

        public string LeerUsuario()
        {
            return this.LeerValorEncriptado("User");
        }

        #endregion

        #region LeerContrasena

        public string LeerContrasena()
        {
            return this.LeerValorEncriptado("Password");
        }

        #endregion
    }
}