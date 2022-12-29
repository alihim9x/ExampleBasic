using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using SingleData;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Utility;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    class LocationCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region Initial
            var singleTon = Singleton.Instance = new Singleton();
            var revitData = singleTon.RevitData;
            revitData.UIApplication = commandData.Application;
            var doc = revitData.Document;
            Selection sel = revitData.Selection;
            #endregion


            var elem = sel.PickObject(ObjectType.Element, "Vui lòng chọn đối tượng").ToElement();

            //var locPoint = elem.Location as LocationPoint; // Kiểu cha Location không có Location Point, ép về kiểu Location Point để lấy LocationPoint
            //var Point = locPoint.Point;
            //var x = Point.X;
            //var y = Point.Y;
            //var levelId = elem.LevelId;
            ////var level = doc.GetElement(levelId) as Level; // Ép kiểu từ Element về kiểu Level để lấy tọa độ
            //var level = levelId.ToElement() as Level ;
            //var z = level.Elevation;

            

            //TaskDialog.Show("Revit", $"{elem.Name} - {elem.Id} - X: {x:0.00} - Y: {y:0.00} - Z: {z:0.00}");

            elem.LocationInfo();


            return Result.Succeeded;
        }
    }
}
