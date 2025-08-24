using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    //INI PROY-31948_Migracion
    [Serializable] //PROY-32439 Serializable
    public class BEAccion
    {

     public BEAccion(int codigo,string nombre,string comentarioca,string codevaluacion)
		{
            EVALUACION = new BEEvaluacionItem();
            EVALUACION.COMENTARIO_CA = comentarioca;
            EVALUACION.ESTADO_EVALUACION = codevaluacion;
            CODIGO = codigo;
            NOMBRE = nombre;
		}
		
        public int CODIGO { get; set; }
        public string NOMBRE { get; set; }
        public BEEvaluacionItem EVALUACION { get; set; }
        public BERegistro REGISTRO { get; set; }
        public string NombreCampo { get; set; }
	}

   //FIN PROY-31948_Migracion
}
