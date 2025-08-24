using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    //INI PROY-31948_Migracion
    [Serializable] //PROY-32439 Serializable
public class BEEvaluacionItem
    {
        public BEMotivoObservacion MOTIVO_OBSERVACION;
        public string ESTADO_EVALUACION;
        public string COMENTARIO_CA;
        public string COMENTARIO_PDV;
    }
    //FIN PROY-31948_Migracion
}
