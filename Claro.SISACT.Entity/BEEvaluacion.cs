using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Claro.SISACT.Entity
{
    [Serializable] //PROY-140126 - IDEA 140248 
    public class BEEvaluacion
    {
        public ArrayList acciones { get; set; }
        public ArrayList documentos { get; set; }
        public ArrayList activaciones { get; set; }
        public ArrayList despachos { get; set; }
        public bool nueva_propuesta { get; set; }
        public bool adjuntar_voucher { get; set; }
        public int cod_sec { get; set; }
        public string cod_analista { get; set; }
        public int ra_dg { get; set; }
        public double cantidad { get; set; }
        public double monto { get; set; }
        public double cargo_fijo { get; set; }
        public string correcion_comentario_ca { get; set; }
        public string correcion_comentario_pdv { get; set; }
        public string adjuntar_comentario_ca { get; set; }
        public string adjuntar_comentario_pdv { get; set; }
        public string propuesta_comentario_ca { get; set; }
        public string propuesta_comentario_pdv { get; set; }
        public string comentario_final_ca { get; set; }
        public string comentario_final_pdv { get; set; }
        public string comentario_final_del_pdv { get; set; }
        public DateTime fecha_creacion { get; set; }
        public DateTime fecha_modificacion { get; set; }
        public bool existe_rechazo { get; set; }
        public string flag_evaluacion { get; set; }
        public string mail_pdv { get; set; }
        public string comentario_final_credito { get; set; }
        public string comentario_despacho { get; set; }
    }
}
