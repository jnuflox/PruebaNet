using System.Data;

namespace Claro.SISACT.IData
{
     public abstract class DAABDataReader
     {
        public abstract IDataReader ReturnDataReader
        {
            get;
            set;
        }
    }
}
