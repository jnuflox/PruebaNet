﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Diagnostics;

using Claro.SISACT.Configuracion;
using Claro.SISACT.IData;


namespace Claro.SISACT.Data
{
   public abstract class BDConexion : Conexion
    {
        public BDConexion(string aplicacion)
            : base(aplicacion) { }

        protected abstract IClaroBDConfiguracion Configuracion { get; }

        public override DAABRequest CreaRequest()
        {
            DAABRequest.TipoOrigenDatos obOrigen;
            if (Configuracion.Provider.IndexOf("ORA") > 0 || Configuracion.Provider == "")
            {
                obOrigen = DAABRequest.TipoOrigenDatos.ORACLE;
            }
            else
            {
                obOrigen = DAABRequest.TipoOrigenDatos.SQL;
            }
            return new DAABRequest(obOrigen, Configuracion.CadenaConexion);
        }

        public DAABRequest CreaRequest(StackTrace stTrace)
        {
            DAABRequest.TipoOrigenDatos obOrigen;
            if (Configuracion.Provider.IndexOf("ORA") > 0 || Configuracion.Provider == "")
            {
                obOrigen = DAABRequest.TipoOrigenDatos.ORACLE;
            }
            else
            {
                obOrigen = DAABRequest.TipoOrigenDatos.SQL;
            }
            return new DAABRequest(obOrigen, Configuracion.CadenaConexion, Configuracion.BaseDatos, stTrace);
        }

        public DAABRequest CreaRequest(StackTrace stTrace, string idAplicacion)
        {
            DAABRequest.TipoOrigenDatos obOrigen;
            if (Configuracion.Provider.IndexOf("ORA") > 0 || Configuracion.Provider == "")
            {
                obOrigen = DAABRequest.TipoOrigenDatos.ORACLE;
            }
            else
            {
                obOrigen = DAABRequest.TipoOrigenDatos.SQL;
            }
            return new DAABRequest(obOrigen, Configuracion.CadenaConexion, Configuracion.BaseDatos, stTrace, idAplicacion);
        }
    }
}
  
