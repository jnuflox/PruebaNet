using System;

namespace Claro.SISACT.Entity.claro_inte_cuparticipante.consultarParticipante
{
	/// <summary>
	/// Summary description for HeaderRequest.
	/// </summary>
	/// 
	[Serializable]
	public class HeaderRequest
	{
		public HeaderRequest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private string _idTransaccion;
		private string _msgid;
		private string _timestamp;
		private string _userId;

		public string idTransaccion
		{
			set{_idTransaccion = value;}
			get{ return _idTransaccion;}
		}
		public string msgid
		{
			set{_msgid = value;}
			get{ return _msgid;}
		}
		public string timestamp
		{
			set{_timestamp = value;}
			get{ return _timestamp;}
		}
		public string userId
		{
			set{_userId = value;}
			get{ return _userId;}
		}



	}
}
