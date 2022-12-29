using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class GridUtil
    {
        public static Autodesk.Revit.DB.XYZ GetDirection (this Autodesk.Revit.DB.Grid grid)
        {
            if(grid.Curve as Autodesk.Revit.DB.Line == null)
            {
                throw new Model.Exception.GridNotLineException();
            }
            return (grid.Curve as Autodesk.Revit.DB.Line).Direction;
        }

    }
}
