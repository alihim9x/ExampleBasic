using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class TransformUtil
    {
        public static Autodesk.Revit.DB.XYZ GetPoint(this Autodesk.Revit.DB.XYZ pnt, Autodesk.Revit.DB.Transform sourceTf, Autodesk.Revit.DB.Transform destinationTf)
        {
            if(sourceTf.IsIdentity)
            {
                return destinationTf.Inverse.OfPoint(pnt);
            }
            if(destinationTf.IsIdentity)
            {
                return sourceTf.OfPoint(pnt);
            }
            return destinationTf.Inverse.OfPoint(sourceTf.OfPoint(pnt));
        }
    }
}
