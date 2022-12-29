using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using SingleData;
using Autodesk.Revit.UI.Selection;
using Utility;
using Autodesk.Revit.DB.Structure;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class GridCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region Initial
            var singleTon = Singleton.Instance = new Singleton();
            var revitData = singleTon.RevitData;
            revitData.UIApplication = commandData.Application;
            var sel = revitData.Selection;
            var doc = revitData.Document;
            var activeView = revitData.ActiveView;
            var tx = revitData.Transaction;
            var uidoc = revitData.UIDocument;
            var app = revitData.Application;
            tx.Start();
            #endregion

            Func<Autodesk.Revit.DB.Element, bool> filterLine = x =>
            {
                var cate = x.Category.Id.IntegerValue;
                if (cate == (int)Autodesk.Revit.DB.BuiltInCategory.OST_Lines) return true;
                return false;
            };
            Func<Autodesk.Revit.DB.Element, bool> filterGrid = x =>
            {
                var cate = x.Category.Id.IntegerValue;
                if (cate == (int)Autodesk.Revit.DB.BuiltInCategory.OST_Grids) return true;
                return false;
            };



            double startP2Line = 0;
            double endP2Line = 0;
            //var line = (sel.PickObject(ObjectType.Element, new SelectionUtil(filterLine), "Pick a Line").ToRevitElement() as DetailLine).GeometryCurve;
            var gridRefs = sel.PickObjects(ObjectType.Element, new SelectionUtil(filterGrid), "Select grids").ToList();
            List<Grid> grids = new List<Grid>();
            foreach (Reference gridRef in gridRefs)
            {
                grids.Add(gridRef.ToRevitElement() as Grid);
            }
            Curve gridCurve = null;


            Grid firstGrid = grids.First();
            var normal = firstGrid.GetDirection().Normalize();
            var dir = new XYZ(0, 0, 1);
            var cross = normal.CrossProduct(dir); // vecto cross la vecto vuong goc voi vector normal va vuong goc voi mat phang 0,0,1
            var point1 = sel.PickPoint();
            //doc.Create.NewDetailCurve(activeView, Line.CreateBound(point1, point1 + 1000.0.Milimet2Feet() * cross));
            var b1 = Line.CreateBound(point1, point1 + 1000000.0.Milimet2Feet() * cross) as Curve;
            var b2 = Line.CreateBound(point1, point1 + 1000000.0.Milimet2Feet() * -cross) as Curve;
            //var b = Line.CreateBound(point1, point1 + 1000.0.Milimet2Feet() * cross) as Curve;
            var line = Line.CreateBound(b1.GetEndPoint(1), b2.GetEndPoint(1));
            XYZ startPoint = null;
            XYZ endPoint = null;
            Line newGridLine = null;
            XYZ gridDir = null;
            XYZ projectP = null;

            foreach (Grid grid in grids)
            {
                gridCurve = grid.GetCurvesInView(DatumExtentType.ViewSpecific, activeView).ToList().FirstOrDefault();
                gridDir = grid.GetDirection();
                startPoint = gridCurve.GetEndPoint(0);
                endPoint = gridCurve.GetEndPoint(1);
                startP2Line = line.Distance(startPoint);
                endP2Line = line.Distance(endPoint);
                if (startP2Line < endP2Line)
                {
                    projectP = line.Project(startPoint).XYZPoint;
                    newGridLine = Line.CreateBound(endPoint, new XYZ(projectP.X, projectP.Y, startPoint.Z));
                }
                else if (endP2Line < startP2Line)
                {
                    projectP = line.Project(endPoint).XYZPoint;
                    newGridLine = Line.CreateBound(startPoint, new XYZ(projectP.X, projectP.Y, endPoint.Z));
                }



                //    //XYZ projectPXYZ = new XYZ(projectP.X, projectP.Y, startPoint.Z);

                grid.SetCurveInView(DatumExtentType.ViewSpecific, activeView, newGridLine);


                //    //doc.Create.NewDetailCurve(activeView, Line.CreateBound(projectP, projectP + 1000.0.Milimet2Feet() * XYZ.BasisX));
                //    //doc.Create.NewDetailCurve(activeView, Line.CreateBound(projectPXYZ, endPoint));
                //    //TaskDialog.Show("Revit", $"Projection Point:{projectP.Z.Feet2Milimeter()}");
                //    //TaskDialog.Show("Revit", $"Start Point:{startPoint.Z.Feet2Milimeter()}");
                //    //TaskDialog.Show("Revit", $"End Point:{endPoint.Z.Feet2Milimeter()}");




                //    //doc.Create.NewDetailCurve(activeView, Line.CreateBound(endPoint, endPoint + 1000.0.Milimet2Feet() * XYZ.BasisX));

            }

            //IList<Curve> gridCurves = grid.GetCurvesInView(
            //  DatumExtentType.Model, activeView);



            //    foreach (Curve c in gridCurves)
            //    {
            //        XYZ start = c.GetEndPoint(0);
            //        XYZ end = c.GetEndPoint(1);

            //        XYZ newStart = start + 1000.0.Milimet2Feet() * XYZ.BasisY;
            //        XYZ newEnd = end - 1000.0.Milimet2Feet() * XYZ.BasisY;

            //        Line newLine = Line.CreateBound(newStart, newEnd);

            //        grid.SetCurveInView(
            //          DatumExtentType.ViewSpecific, activeView, newLine);
            //    }


            tx.Commit();
            return Result.Succeeded;
        }
        }
    }
