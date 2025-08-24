using System;

namespace Claro.SISACT.Entity.claro_inte_cuparticipante.consultarParticipante
{
	/// <summary>
	/// Summary description for BuscarCuParticipanteRequest.
	/// </summary>
	[Serializable]
	public class BuscarCuParticipanteRequest
	{
		public BuscarCuParticipanteRequest()
		{
		}

		private string _codigoClienteUnico;
		private string _participanteID;
		private string _tipoDocumento;
		private string _numeroDocumento;

		public string codigoClienteUnico
		{
			set{_codigoClienteUnico = value;}
			get{ return _codigoClienteUnico;}
		}

		public string participanteID
		{
			set{_participanteID = value;}
			get{ return _participanteID;}
		}

		public string tipoDocumento
		{
			set{_tipoDocumento = value;}
			get{ return _tipoDocumento;}
		}

		public string numeroDocumento
		{
			set{_numeroDocumento = value;}
			get{ return _numeroDocumento;}
		}
	}
}
