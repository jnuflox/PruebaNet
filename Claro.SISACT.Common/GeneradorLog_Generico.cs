using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Claro.SISACT.Common
{

   public class GeneradorLog_Generico
    {
      private static ILog _loggerManager = LogManager.GetLogger("Log_SISACT_CONSUMER");


      public void EscribirLog(String strTexto)
      {
          _loggerManager.Info(strTexto);

      }

    }
}
