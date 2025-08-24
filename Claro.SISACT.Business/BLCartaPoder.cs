using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Claro.SISACT.Entity;
using Claro.SISACT.Data;
using System.Data;
using System.Configuration;

namespace Claro.SISACT.Business
{
   public class BLCartaPoder
    {
        public bool RegistraCartaPoder( BECartaPoder oCartaPoder)
        {
            return new DACartaPoder().RegistrarCartaPoder(oCartaPoder);
        }

        public bool RegistrarRepresentanteLegal(BERepresentanteLegal oRepresentanteLegal)
        {
            return new DACartaPoder().RegistrarRepresentanteLegal(oRepresentanteLegal);
        }  

       
    }
}
