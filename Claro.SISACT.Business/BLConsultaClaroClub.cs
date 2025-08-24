using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;
using System.Data;

namespace Claro.SISACT.Business
{
    public class BLConsultaClaroClub
    {
        public static void ValidaBloqueoBolsa(string k_tipo_doc,
          string k_num_doc,
          string k_tipo_clie,
          ref string k_tipo_doc2,
          ref string k_estado,
          ref double k_coderror,
          ref string k_descerror)
        {
            DAConsultaClaroClub.ValidaBloqueoBolsa(k_tipo_doc,
            k_num_doc,
            k_tipo_clie,
            ref k_tipo_doc2,
            ref k_estado,
            ref k_coderror,
            ref k_descerror);
        }
    }
}
