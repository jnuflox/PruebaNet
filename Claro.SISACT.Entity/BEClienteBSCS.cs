using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEClienteBSCS
    {
        private int _customerId;
        private string _cuenta;
        private string _nombre;
        private string _apellidos;
        private string _razonSocial;
        private string _tip_doc;
        private string _num_doc;
        private string _titulo;
        private string _telef_principal;
        private string _estado_civil;
        private DateTime _fecha_nac;
        private string _lug_nac;
        private string _ruc_dni;
        private string _nomb_comercial;
        private string _contacto_cliente;
        private string _rep_legal;
        private string _telef_contacto;
        private string _fax;
        private string _email;
        private string _cargo;
        private string _consultor;
        private string _asesor;
        private string _direccion_fac;
        private string _urbanizacion_fac;
        private string _distrito_fac;
        private string _provincia_fac;
        private string _cod_postal_fac;
        private string _departamento_fac;
        private string _pais_fac;
        private string _direccion_leg;
        private string _urbanizacion_leg;
        private string _distrito_leg;
        private string _provincia_leg;
        private string _cod_postal_leg;
        private string _departamento_leg;
        private string _pais_leg;
        private int _co_id;
        private string _nicho_id;
        private int _num_cuentas;
        private int _num_lineas;
        private string _ciclo_fac;
        private string _status_cuenta;
        private string _modalidad;
        private string _tipo_cliente;
        private DateTime _fecha_act;
        private double _limite_credito;
        private string _segmento;
        private string _respon_pago;
        private string _credit_score;
        private string _forma_pago;
        private string _codigo_tipo_cliente;
        private string _sexo;
        private int _nacionalidad;
        private int _estado_civil_id;

        public BEClienteBSCS()
        {
        }

        public int CustomerId
        {
            get { return _customerId; }
            set { _customerId = value; }
        }

        public string Cuenta
        {
            get { return _cuenta; }
            set { _cuenta = value; }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Apellidos
        {
            get { return _apellidos; }
            set { _apellidos = value; }
        }

        public string RazonSocial
        {
            get { return _razonSocial; }
            set { _razonSocial = value; }
        }

        public string Tip_doc
        {
            get { return _tip_doc; }
            set { _tip_doc = value; }
        }

        public string Num_doc
        {
            get { return _num_doc; }
            set { _num_doc = value; }
        }

        public string Titulo
        {
            get { return _titulo; }
            set { _titulo = value; }
        }

        public string Telef_principal
        {
            get { return _telef_principal; }
            set { _telef_principal = value; }
        }

        public string Estado_civil
        {
            get { return _estado_civil; }
            set { _estado_civil = value; }
        }

        public DateTime Fecha_nac
        {
            get { return _fecha_nac; }
            set { _fecha_nac = value; }
        }

        public string Lug_nac
        {
            get { return _lug_nac; }
            set { _lug_nac = value; }
        }

        public string Ruc_dni
        {
            get { return _ruc_dni; }
            set { _ruc_dni = value; }
        }

        public string Nomb_comercial
        {
            get { return _nomb_comercial; }
            set { _nomb_comercial = value; }
        }

        public string Contacto_cliente
        {
            get { return _contacto_cliente; }
            set { _contacto_cliente = value; }
        }

        public string Rep_legal
        {
            get { return _rep_legal; }
            set { _rep_legal = value; }
        }

        public string Telef_contacto
        {
            get { return _telef_contacto; }
            set { _telef_contacto = value; }
        }

        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Cargo
        {
            get { return _cargo; }
            set { _cargo = value; }
        }

        public string Consultor
        {
            get { return _consultor; }
            set { _consultor = value; }
        }

        public string Asesor
        {
            get { return _asesor; }
            set { _asesor = value; }
        }

        public string Direccion_fac
        {
            get { return _direccion_fac; }
            set { _direccion_fac = value; }
        }

        public string Urbanizacion_fac
        {
            get { return _urbanizacion_fac; }
            set { _urbanizacion_fac = value; }
        }

        public string Distrito_fac
        {
            get { return _distrito_fac; }
            set { _distrito_fac = value; }
        }

        public string Provincia_fac
        {
            get { return _provincia_fac; }
            set { _provincia_fac = value; }
        }

        public string Cod_postal_fac
        {
            get { return _cod_postal_fac; }
            set { _cod_postal_fac = value; }
        }

        public string Departamento_fac
        {
            get { return _departamento_fac; }
            set { _departamento_fac = value; }
        }

        public string Pais_fac
        {
            get { return _pais_fac; }
            set { _pais_fac = value; }
        }

        public string Direccion_leg
        {
            get { return _direccion_leg; }
            set { _direccion_leg = value; }
        }

        public string Urbanizacion_leg
        {
            get { return _urbanizacion_leg; }
            set { _urbanizacion_leg = value; }
        }

        public string Distrito_leg
        {
            get { return _distrito_leg; }
            set { _distrito_leg = value; }
        }

        public string Provincia_leg
        {
            get { return _provincia_leg; }
            set { _provincia_leg = value; }
        }

        public string Cod_postal_leg
        {
            get { return _cod_postal_leg; }
            set { _cod_postal_leg = value; }
        }

        public string Departamento_leg
        {
            get { return _departamento_leg; }
            set { _departamento_leg = value; }
        }

        public string Pais_leg
        {
            get { return _pais_leg; }
            set { _pais_leg = value; }
        }

        public int Co_id
        {
            get { return _co_id; }
            set { _co_id = value; }
        }

        public string Nicho_id
        {
            get { return _nicho_id; }
            set { _nicho_id = value; }
        }

        public int Num_cuentas
        {
            get { return _num_cuentas; }
            set { _num_cuentas = value; }
        }

        public int Num_lineas
        {
            get { return _num_lineas; }
            set { _num_lineas = value; }
        }

        public string Ciclo_fac
        {
            get { return _ciclo_fac; }
            set { _ciclo_fac = value; }
        }

        public string Status_cuenta
        {
            get { return _status_cuenta; }
            set { _status_cuenta = value; }
        }

        public string Modalidad
        {
            get { return _modalidad; }
            set { _modalidad = value; }
        }

        public string Tipo_cliente
        {
            get { return _tipo_cliente; }
            set { _tipo_cliente = value; }
        }

        public DateTime Fecha_act
        {
            get { return _fecha_act; }
            set { _fecha_act = value; }
        }

        public double Limite_credito
        {
            get { return _limite_credito; }
            set { _limite_credito = value; }
        }

        public string Segmento
        {
            get { return _segmento; }
            set { _segmento = value; }
        }

        public string Respon_pago
        {
            get { return _respon_pago; }
            set { _respon_pago = value; }
        }

        public string Credit_score
        {
            get { return _credit_score; }
            set { _credit_score = value; }
        }

        public string Forma_pago
        {
            get { return _forma_pago; }
            set { _forma_pago = value; }
        }

        public string Codigo_tipo_cliente
        {
            get { return _codigo_tipo_cliente; }
            set { _codigo_tipo_cliente = value; }
        }

        public string Sexo
        {
            get { return _sexo; }
            set { _sexo = value; }
        }

        public int Nacionalidad
        {
            get { return _nacionalidad; }
            set { _nacionalidad = value; }
        }

        public int Estado_civil_id
        {
            get { return _estado_civil_id; }
            set { _estado_civil_id = value; }
        }
    }
}
