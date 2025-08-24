using System;

namespace Claro.SISACT.Entity.claro_inte_cuparticipante.consultarParticipante
{
	/// <summary>
	/// Summary description for ListaDetalleSegmento.
	/// </summary>
	public class ListaDetalleSegmento
	{
		public ListaDetalleSegmento()
		{
		}

		private int _listaID;
		private string _descripcionLista;
		private string _flagActive;
		private string _fechaRegistro;
		private string _sistemaOrigen;
		private string _usuarioRegistro;

		public int listaID
		{
			set{_listaID = value;}
			get{ return _listaID;}
		}

		public string descripcionLista
		{
			set{_descripcionLista = value;}
			get{ return _descripcionLista;}
		}

		public string flagActive
		{
			set{_flagActive = value;}
			get{ return _flagActive;}
		}

		public string fechaRegistro
		{
			set{_fechaRegistro = value;}
			get{ return _fechaRegistro;}
		}

		public string sistemaOrigen
		{
			set{_sistemaOrigen = value;}
			get{ return _sistemaOrigen;}
		}

		public string usuarioRegistro
		{
			set{_usuarioRegistro = value;}
			get{ return _usuarioRegistro;}
		}
	}
}
