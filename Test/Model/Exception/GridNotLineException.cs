using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Exception
{
    class GridNotLineException : System.Exception
    {
        public override string Message  {get {return "Grid không phải là đường thẳng!";} } // Ghi đè thuộc tính Message của System.Exception
    }
    
}
