using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claro.SISACT.Web.Base;//PROY-140126 - IDEA 140248 

namespace Claro.SISACT.Web.Paginas.consultas
{
    public partial class sisact_direccion_mapaweb : Sisact_Webbase //PROY-140126 - IDEA 140248  
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hidValorRetorno.Value = Request.QueryString["codPlano"];
        }
    }
}