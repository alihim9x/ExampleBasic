using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Exception
{
    class FloorRebarCaseNotCheckException : System.Exception
    {
        public override string Message  {get {return "Trường hợp thép sàn chưa được kiểm tra!";} } // Ghi đè thuộc tính Message của System.Exception
    }
    
}
