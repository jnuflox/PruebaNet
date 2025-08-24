using System;

namespace Claro.SISACT.Entity.claro_inte_cuparticipante.consultarParticipante
{
	/// <summary>
	/// Summary description for BuscarCuParticipanteResponse.
	/// </summary>
	[Serializable]
	public class BuscarCuParticipanteResponse
	{
		public BuscarCuParticipanteResponse()
		{
			_claroFault = new ClaroFault();
		}

		private int _codigoRespuesta;
		private string _mensajeRespuesta;
		private string _mensajeError;
		private Participante[] _participante;
		private ClaroFault _claroFault;

		public int codigoRespuesta
		{
			set{_codigoRespuesta = value;}
			get{ return _codigoRespuesta;}
		}

		public string mensajeRespuesta
		{
			set{_mensajeRespuesta = value;}
			get{ return _mensajeRespuesta;}
		}

		public string mensajeError
		{
			set{_mensajeError = value;}
			get{ return _mensajeError;}
		}

		public Participante[] participante
		{
			set{_participante = value;}
			get{ return _participante;}
		}

		public ClaroFault claroFault
		{
			set{_claroFault = value;}
			get{ return _claroFault;}
		}

	}
}
