using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;

namespace Utility
{
    public static class FillPatternUtil
    {
        private static RevitData revitData = RevitData.Instance;
        public static Autodesk.Revit.DB.FillPatternElement GetFillPatternElement(string name)
        {
            var fillPatternElement = revitData.FillPatternElements.SingleOrDefault(x => x.Name == name);
            if(fillPatternElement == null)
            {
                throw new Model.Exception.ElementNotFoundException();
            }
            return fillPatternElement;
        }
    }
}
