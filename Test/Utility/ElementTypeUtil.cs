using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;
namespace Utility
{
    public static class ElementTypeUtil
    {
        private static RevitData revitData = RevitData.Instance;
        public static Autodesk.Revit.DB.FloorType GetFloorType (string name)
        {
            var floortype = revitData.FloorTypes.SingleOrDefault(x=>x.Name == name);
            if(floortype == null)

            {
                throw new Model.Exception.ElementNotFoundException();

            }
            return floortype;

        }
        public static Autodesk.Revit.DB.WallType GetWallType (string name)
        {
            var walltype = revitData.WallTypes.SingleOrDefault(x => x.Name == name);
            if(walltype == null)
            {
                throw new Model.Exception.ElementNotFoundException();
            }
            return walltype;
              
        }
        public static Autodesk.Revit.DB.FamilySymbol GetColumnType (string name)
        {
            var columntype = revitData.ColumnTypes.SingleOrDefault(x => x.Name == name);
                if(columntype == null)
            {
                throw new Model.Exception.ElementNotFoundException();
            }
            return columntype;
        }
        public static Autodesk.Revit.DB.FamilySymbol GetFramingType  (string name)
        {
            var framingtype = revitData.FramingTypes.SingleOrDefault(x => x.Name == name);
            if(framingtype == null)
            {
                throw new Model.Exception.ElementNotFoundException();
            }
            return framingtype;
        }
    }
}
