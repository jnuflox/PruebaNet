using System;
using System.Configuration;
using Claro.SISACT.Configuracion;
using Claro.SISACT.IData;
using System.Diagnostics;

namespace Claro.SISACT.Data
{
    public class BDBSCS : BDConexion
    {

        public BDBSCS(string aplicacion)
            : base(aplicacion) { }

        protected override IClaroBDConfiguracion Configuracion
        {
            get
            {
                return Conexion.GeneraConfiguracion<ConfigConexionBSCS>(this.Aplicacion);
            }
        }

    }
}