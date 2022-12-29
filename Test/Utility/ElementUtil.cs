using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;
using Autodesk.Revit.DB;

namespace Utility
{
    public  static class ElementUtil
    {
        private static RevitData revitData = RevitData.Instance;
        public static Model.Element ToElement(this Autodesk.Revit.DB.Element elem)
        {
            return new Model.Element { Guid = elem.UniqueId };
        }

        public static Model.Element ToElement(this Autodesk.Revit.DB.Reference rf)
        {
            var doc = Singleton.Instance.RevitData.Document;
            return doc.GetElement(rf).ToElement();

        }
        public static Model.Element ToElement(this Autodesk.Revit.DB.ElementId elemId)
        {
            var doc = Singleton.Instance.RevitData.Document;
            return doc.GetElement(elemId).ToElement();
        }
        public static Model.Element ToElement(this string guid)
        {

            return new Model.Element { Guid = guid };
        }
        public static Model.XYZ GetLocationPoint(this Autodesk.Revit.DB.Element elem)
        {


            var locPoint = elem?.Location as LocationPoint; // Kiểu cha Location không có Location Point, ép về kiểu Location Point để lấy LocationPoint
                var Point = locPoint?.Point; 
                if(Point != null)
                {
                    var x = Point.X;
                    var y = Point.Y;
                    var levelId = elem.LevelId;
                    var level = levelId.ToElement().RevitElement as Level;
                    var z = level.Elevation;
                    return new Model.XYZ (x,y,z);
                
                
                }

            return null;


        }
        public static Model.Curve GetLocationCurve (this Autodesk.Revit.DB.Element elem)
        {

            
                var locCurve = elem?.Location as LocationCurve;
                var Curve = locCurve?.Curve;

            
            if(Curve != null)
            {
                var length = Curve.Length;
                
                var x1C = Curve.GetEndPoint(0).X;
                var y1C = Curve.GetEndPoint(0).Y;
                var z1C = Curve.GetEndPoint(0).Z;
                var x2C = Curve.GetEndPoint(1).X;
                var y2C = Curve.GetEndPoint(1).Y;
                var z2C = Curve.GetEndPoint(1).Z;
                //var levelID = elem.LevelId;
                //var level = levelID.ToElement().RevitElement as Level;
                //var z = level.Elevation;
                return new Model.Curve { SPoint = new Model.XYZ (x1C,y1C,z1C), EPoint = new Model.XYZ (x2C,y2C,z2C ), Length = length };
            }
            else

            return null;

        }
        public static void LocationInfo(this Model.Element elem)
        {
            var doc = Singleton.Instance.RevitData.Document;
            FilteredElementCollector allGrid = new FilteredElementCollector(doc).WhereElementIsNotElementType()
                                                 .OfCategory(BuiltInCategory.OST_Grids);
            List<Grid> XGrid = new List<Grid>();
            List<Grid> YGrid = new List<Grid>();
            //List<double> xDis = new List<double>();
            //List<double> yDis = new List<double>();
            foreach (var item in allGrid)
            {

                var grid = item as Grid;
                var gridCurve = grid.Curve;
                var gridLine = gridCurve as Line;
                
                var gridDir = gridLine.Direction;
                
                var angle = Math.Round(gridDir.AngleTo(XYZ.BasisX) * 180 / Math.PI, 0);
                if ((angle == 90) || (angle == 270))
                {
                    YGrid.Add(grid);
                }
                else if ((angle == 0) || (angle == 180) || (angle == 360))
                {
                    XGrid.Add(grid);
                }
            }

            //Sort theo Origin để sắp xếp lại Grid trong List Grid
            //var XGrid2 = XGrid.OrderBy(x => (x.Curve as Line).Origin.Y);

            Grid gridY1st = null;
            double tamO = 0;
            foreach (var item in YGrid)
            {
                if(tamO == 0)
                {
                    gridY1st = item;
                    tamO = (item.Curve as Line).Origin.X;
                }

                if(tamO!= 0)
                {
                    
                    if (tamO > (item.Curve as Line).Origin.X )
                    {
                        gridY1st = item;
                        tamO = (item.Curve as Line).Origin.X;
                    }
                }
            }
            Grid gridX1st = null;
            double tamOX = 0;
            foreach (var item in XGrid)
            {
                if (tamOX == 0)
                {
                    gridX1st = item;
                    tamOX = (item.Curve as Line).Origin.Y;
                }

                if (tamOX != 0)
                {

                    if (tamOX > (item.Curve as Line).Origin.Y)
                    {
                        gridX1st = item;
                        tamOX = (item.Curve as Line).Origin.Y;
                    }
                }
            }

            var xyz = elem?.XYZ;
            XYZ xyz1 = new XYZ(xyz.X, xyz.Y, xyz.Z);
            var curve = elem?.Curve;
            var isLeftRight = true;
            if(xyz != null)
            {
                double minDisX = 0;
                double minDisY = 0;
                double tamX = 0;
                double tamY = 0;
                Grid gridX = null;
                Grid gridY = null;
                foreach (var item in XGrid)
                {
                    if(minDisX == 0)
                    {
                        tamX = item.Curve.Distance(xyz1);
                        minDisX = tamX;
                        gridX = item;
                    }
                    if(minDisX != 0 )
                    {
                        tamX = item.Curve.Distance(xyz1);
                        if(minDisX > tamX)
                        {
                            minDisX = tamX;
                            gridX = item;
                        }
                    }


                }
                foreach (var item in YGrid)
                {
                    if (minDisY == 0)
                    {
                        tamY = item.Curve.Distance(xyz1);
                        minDisY = tamY;
                        gridY = item;
                    }
                    if (minDisY != 0)
                    {
                        tamY = item.Curve.Distance(xyz1);
                        if (minDisY > tamY)
                        {
                            minDisY = tamY;
                            gridY = item;
                        }
                    }
                }
                if(((gridY.Curve as Line).Origin.X) > xyz1.X)
                {
                    minDisY = -minDisY;
                }
                else if(((gridY.Curve as Line).Origin.X) < xyz1.X)
                {
                    minDisY = minDisY;
                }
                else
                {
                    minDisY = 0;
                }
                if (((gridX.Curve as Line).Origin.Y) > xyz1.Y)
                {
                    minDisX = -minDisX;
                }
                else if (((gridX.Curve as Line).Origin.Y) < xyz1.Y)
                {
                    minDisX = minDisX;
                }
                else
                {
                    minDisX = 0;
                }

                //if((gridY1st.Curve.Distance((gridY.Curve as Line).Origin + XYZ.BasisX) > gridY1st.Curve.Distance(xyz1 + XYZ.BasisX) && Math.Round(minDisY,3) !=0))
                //{
                //    Autodesk.Revit.UI.TaskDialog.Show("Revit", $"X(mm): {gridY.Name}:-{minDisY.Feet2Milimeter():0} || Y(mm): {gridX.Name}:{minDisX.Feet2Milimeter():0}");
                //}
                //else if (gridY1st.Curve.Distance((gridY.Curve as Line).Origin + XYZ.BasisX) < gridY1st.Curve.Distance(xyz1 + XYZ.BasisX)&& Math.Round(minDisY,3) !=0)
                //{
                //    Autodesk.Revit.UI.TaskDialog.Show("Revit", $"X(mm): {gridY.Name}:+{minDisY.Feet2Milimeter():0} || Y(mm): {gridX.Name}:{minDisX.Feet2Milimeter():0}");
                //}
                ////else if ((gridY1st.Curve.Distance((gridY.Curve as Line).Origin + XYZ.BasisX) == gridY1st.Curve.Distance(xyz1 + XYZ.BasisX)))
                //else
                //{
                //    Autodesk.Revit.UI.TaskDialog.Show("Revit", $"X(mm): {gridY.Name} || Y(mm): {gridX.Name}:{minDisX.Feet2Milimeter():0}");
                //}
                //if ((((gridY.Curve as Line).Origin.X) > xyz1.X && Math.Round(minDisY, 3) != 0))
                //{
                //    Autodesk.Revit.UI.TaskDialog.Show("Revit", $"X(mm): {gridY.Name}:-{minDisY.Feet2Milimeter():0} || Y(mm): {gridX.Name}:{minDisX.Feet2Milimeter():0}");
                //}
                //else if (((gridY.Curve as Line).Origin.X) < xyz1.X && Math.Round(minDisY, 3) != 0)
                //{
                //    Autodesk.Revit.UI.TaskDialog.Show("Revit", $"X(mm): {gridY.Name}:+{minDisY.Feet2Milimeter():0} || Y(mm): {gridX.Name}:{minDisX.Feet2Milimeter():0}");
                //}
                ////else if ((gridY1st.Curve.Distance((gridY.Curve as Line).Origin + XYZ.BasisX) == gridY1st.Curve.Distance(xyz1 + XYZ.BasisX)))
                //else
                //{
                //    Autodesk.Revit.UI.TaskDialog.Show("Revit", $"X(mm): {gridY.Name} || Y(mm): {gridX.Name}:{minDisX.Feet2Milimeter():0}");
                //}
                Autodesk.Revit.UI.TaskDialog.Show("Revit", $"X(mm): {gridY.Name}:{minDisY.Feet2Milimeter():0} || Y(mm): {gridX.Name}:{minDisX.Feet2Milimeter():0}");

            }
            else if(curve != null)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", $" Start Point: X1(mm): {curve.SPoint.X / 0.00328084:0.000} - Y1(mm): {curve.SPoint.Y / 0.00328084:0.000} - Z1(mm): {curve.SPoint.Z / 0.00328084:0.000} \n" +
                                                    $" End Point: X2(mm): {curve.EPoint.X / 0.00328084:0.000} - Y2(mm): {curve.EPoint.Y / 0.00328084:0.000} - Z2(mm): {curve.EPoint.Z / 0.00328084:0.000} \n" +
                                                    $" Length(mm): {curve.Length/0.00328084:0.000} ");
            }
            else if(xyz == null)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", $"Đối tượng bạn chọn không có thuộc tính Location Point! ");
            }
            else if(curve == null)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Revit", $"Đối tượng bạn chọn không có thuộc tính Location Curve! ");
            }
        }
        public static Autodesk.Revit.DB.Element ToRevitElement(this Autodesk.Revit.DB.Reference rf)
        {
            return revitData.Document.GetElement(rf);
        }
        public static Autodesk.Revit.DB.Element ToRevitElement(this Autodesk.Revit.DB.ElementId elemID)
        {
            return revitData.Document.GetElement(elemID);
        }
        public static IEnumerable<Autodesk.Revit.DB.Element> GetIntersectElements(this Autodesk.Revit.DB.Element elem,
            IEnumerable<Autodesk.Revit.DB.BuiltInCategory> bic)
        {
            var insElem = revitData.InstanceElements.Where(x =>
            {
                if (x.Id.IntegerValue == elem.Id.IntegerValue) return false;
                var cate = x.Category;
                if (cate == null) return false;
                return bic.Contains((Autodesk.Revit.DB.BuiltInCategory)cate.Id.IntegerValue);
            });
            var bb = elem.get_BoundingBox(null);
            var outline = new Autodesk.Revit.DB.Outline(bb.Min, bb.Max);
            var bbIntersecFil = new Autodesk.Revit.DB.BoundingBoxIntersectsFilter(outline);
            var solid = elem.GetOriginalSolid().ScaleSolid(1.001); // Để va chạm với các đối tượng chạm vô đối tượng đang xét chứ ko join vô
            var solidIntersecFil = new Autodesk.Revit.DB.ElementIntersectsSolidFilter(solid);
            return new Autodesk.Revit.DB.FilteredElementCollector(revitData.Document
                , insElem.Select(x => x.Id).ToList()).WherePasses(bbIntersecFil).WherePasses(solidIntersecFil);
            
        }
        public static List<Model.Entity.Rebar> GetModelEntityRebarInHost(this Autodesk.Revit.DB.Element element)
        {
            if((element as Autodesk.Revit.DB.FamilyInstance) != null)
            {
                var rebarInHost = Autodesk.Revit.DB.Structure.RebarHostData.GetRebarHostData(element).GetRebarsInHost();
                List<Model.Entity.Rebar> rebarList = new List<Model.Entity.Rebar>();
                rebarInHost.ToList().ForEach(x => rebarList.Add(new Model.Entity.Rebar(x)));
                return rebarList;
            }
            else if((element as Autodesk.Revit.DB.Floor)!=null)
            {
                var elemId = element.Id;
                var floorRebarInstances = revitData.FloorRebars.Where(x => x.GetHostId() == elemId).ToList();
                return floorRebarInstances;
            }
            throw new Model.Exception.CaseNotCheckException();
            
        }
        public static Autodesk.Revit.DB.XYZ MaxRepeatedItem (this List<Autodesk.Revit.DB.XYZ> listXYZ)
        {
            var maxRepeatedItems = listXYZ.GroupBy(x => x.Z).OrderByDescending(x => x.Count()).First().Select(x => x).First();
            return maxRepeatedItems;
            
        }
    }
}
