using System;

namespace Claro.SISACT.Entity.claro_inte_cuparticipante.consultarParticipante
{
	/// <summary>
	/// Summary description for Participante.
	/// </summary>
	[Serializable]
	public class Participante
	{
		public Participante()
		{
		}

		private string _numeroDocumento;
		private string _codigoClienteUnico;
		private int _participanteId;
		private string _apellidoMaterno;
		private string _apellidoPaterno;
		private string _codigoAplicacion;
		private string _fechaCreacionProspecto;
		private string _fechaNacimiento;
		private string _nombre;
		private string _tipoDocDescripcion;
		private string _tipoDocumento;
		private string _razonSocial;
		private string _sexo;
		private string _tipoPersona;
		private string _sistemaOrigen;
		private string _correoElectronico;
		private string _clasificacionDeMercado;
		private string _telefonoReferencia1;
		private string _sectorComercial;
		private string _ubigeoDireccion;
		private string _estadoCivil;
		private string _estadoCivilDesc;
		private string _limiteCredito;
		private string _lugarNacimiento;
		private string _nombreComercial;
		private string _telefonoReferencia2;
		private string _nacionalidadId;
		private string _nacionalidadDes;
		private string _estado;
		private string _segmentoComercial;
		private string _fechaCreacionCliente;
		private string _direccionLegal;
		private string _direccionReferenciaLegal;
		private string _paisLegal;
		private string _departamentoLegal;
		private string _provinciaLegal;
		private string _distritoLegal;
		private string _urbanizacionLegal;
		private string _codigoPostalLegal;
		private ListaDetalleSegmento[] _listaDetalleSegmento;

		public string numeroDocumento
		{
			set{_numeroDocumento = value;}
			get{ return _numeroDocumento;}
		}

		public string codigoClienteUnico
		{
			set{_codigoClienteUnico = value;}
			get{ return _codigoClienteUnico;}
		}

		public int participanteId
		{
			set{_participanteId = value;}
			get{ return _participanteId;}
		}

		public string apellidoMaterno
		{
			set{_apellidoMaterno = value;}
			get{ return _apellidoMaterno;}
		}

		public string apellidoPaterno
		{
			set{_apellidoPaterno = value;}
			get{ return _apellidoPaterno;}
		}

		public string codigoAplicacion
		{
			set{_codigoAplicacion = value;}
			get{ return _codigoAplicacion;}
		}

		public string fechaCreacionProspecto
		{
			set{_fechaCreacionProspecto = value;}
			get{ return _fechaCreacionProspecto;}
		}

		public string fechaNacimiento
		{
			set{_fechaNacimiento = value;}
			get{ return _fechaNacimiento;}
		}

		public string nombre
		{
			set{_nombre = value;}
			get{ return _nombre;}
		}

		public string tipoDocDescripcion
		{
			set{_tipoDocDescripcion = value;}
			get{ return _tipoDocDescripcion;}
		}

		public string tipoDocumento
		{
			set{_tipoDocumento = value;}
			get{ return _tipoDocumento;}
		}

		public string razonSocial
		{
			set{_razonSocial = value;}
			get{ return _razonSocial;}
		}

		public string sexo
		{
			set{_sexo = value;}
			get{ return _sexo;}
		}

		public string tipoPersona
		{
			set{_tipoPersona = value;}
			get{ return _tipoPersona;}
		}

		public string sistemaOrigen
		{
			set{_sistemaOrigen = value;}
			get{ return _sistemaOrigen;}
		}

		public string correoElectronico
		{
			set{_correoElectronico = value;}
			get{ return _correoElectronico;}
		}

		public string clasificacionDeMercado
		{
			set{_clasificacionDeMercado = value;}
			get{ return _clasificacionDeMercado;}
		}

		public string telefonoReferencia1
		{
			set{_telefonoReferencia1 = value;}
			get{ return _telefonoReferencia1;}
		}

		public string sectorComercial
		{
			set{_sectorComercial = value;}
			get{ return _sectorComercial;}
		}

		public string ubigeoDireccion
		{
			set{_ubigeoDireccion = value;}
			get{ return _ubigeoDireccion;}
		}

		public string estadoCivil
		{
			set{_estadoCivil = value;}
			get{ return _estadoCivil;}
		}

		public string estadoCivilDesc
		{
			set{_estadoCivilDesc = value;}
			get{ return _estadoCivilDesc;}
		}

		public string limiteCredito
		{
			set{_limiteCredito = value;}
			get{ return _limiteCredito;}
		}

		public string lugarNacimiento
		{
			set{_lugarNacimiento = value;}
			get{ return _lugarNacimiento;}
		}

		public string nombreComercial
		{
			set{_nombreComercial = value;}
			get{ return _nombreComercial;}
		}

		public string telefonoReferencia2
		{
			set{_telefonoReferencia2 = value;}
			get{ return _telefonoReferencia2;}
		}

		public string nacionalidadId
		{
			set{_nacionalidadId = value;}
			get{ return _nacionalidadId;}
		}

		public string nacionalidadDes
		{
			set{_nacionalidadDes = value;}
			get{ return _nacionalidadDes;}
		}

		public string estado
		{
			set{_estado = value;}
			get{ return _estado;}
		}

		public string segmentoComercial
		{
			set{_segmentoComercial = value;}
			get{ return _segmentoComercial;}
		}

		public string fechaCreacionCliente
		{
			set{_fechaCreacionCliente = value;}
			get{ return _fechaCreacionCliente;}
		}

		public string direccionLegal
		{
			set{_direccionLegal = value;}
			get{ return _direccionLegal;}
		}

		public string direccionReferenciaLegal
		{
			set{_direccionReferenciaLegal = value;}
			get{ return _direccionReferenciaLegal;}
		}

		public string paisLegal
		{
			set{_paisLegal = value;}
			get{ return _paisLegal;}
		}

		public string departamentoLegal
		{
			set{_departamentoLegal = value;}
			get{ return _departamentoLegal;}
		}

		public string provinciaLegal
		{
			set{_provinciaLegal = value;}
			get{ return _provinciaLegal;}
		}

		public string distritoLegal
		{
			set{_distritoLegal = value;}
			get{ return _distritoLegal;}
		}

		public string urbanizacionLegal
		{
			set{_urbanizacionLegal = value;}
			get{ return _urbanizacionLegal;}
		}

		public string codigoPostalLegal
		{
			set{_codigoPostalLegal = value;}
			get{ return _codigoPostalLegal;}
		}

		public ListaDetalleSegmento[] listaDetalleSegmento
		{
			set{_listaDetalleSegmento = value;}
			get{ return _listaDetalleSegmento;}
		}

	}
}
