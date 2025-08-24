using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEPlanEquipoHFC
    {

        private int _Orden;
        private string _Plan;
        private string _IdServicio;
        private string _Servicio;
        private string _IdEquipo;
        private string _Equipo;
        private string _FlagComodato;
        private double _CF_Alquiler;
        private string _Cantidad;
        private string _Prdc_Codigo;

        public string Prdc_Codigo
        {
            get { return this._Prdc_Codigo; }
            set { this._Prdc_Codigo = value; }
        }
        public int Orden
        {
            get { return this._Orden; }
            set { this._Orden = value; }
        }

        public string Plan
        {
            get { return this._Plan; }
            set { this._Plan = value; }
        }

        public string IdServicio
        {
            get { return this._IdServicio; }
            set { this._IdServicio = value; }
        }

        public string Servicio
        {
            get { return this._Servicio; }
            set { this._Servicio = value; }
        }

        public string IdEquipo
        {
            get { return this._IdEquipo; }
            set { this._IdEquipo = value; }
        }

        public string Equipo
        {
            get { return this._Equipo; }
            set { this._Equipo = value; }
        }

        public string FlagComodato
        {
            get { return this._FlagComodato; }
            set { this._FlagComodato = value; }
        }

        public double CF_Alquiler
        {
            get { return this._CF_Alquiler; }
            set { this._CF_Alquiler = value; }
        }

        public string Cantidad
        {
            get { return this._Cantidad; }
            set { this._Cantidad = value; }
        }
    }
}
