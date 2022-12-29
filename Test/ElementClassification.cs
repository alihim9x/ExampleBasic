using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using SingleData;
using Utility;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class ElementClassification : IExternalCommand
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
            #endregion

            #region Tính diện tích sàn
            //var elem = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToElement().RevitElement;
            ////var rf =HostObjectUtils.GetTopFaces(elem as HostObject).First();
            ////var face = elem.GetGeometryObjectFromReference(rf) as Face;
            ////TaskDialog.Show("Revit", $"Diện tích mặt trên của sàn là: {face.Area.FeetSq2MeterSq() :0.00}");

            #endregion

            #region Diện tích mặt tường
            var elem = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToElement().RevitElement;
            //var rfs = new List<Reference> { HostObjectUtils.GetSideFaces(elem as HostObject, ShellLayerType.Exterior).First(),
            //                                HostObjectUtils.GetSideFaces(elem as HostObject,ShellLayerType.Interior).First()};
            //var faces = rfs.Select(x => elem.GetGeometryObjectFromReference(x) as Face);
            //TaskDialog.Show("Revit", $"Diện tích hai mặt bên của tường là: {faces.Sum(x => x.Area).FeetSq2MeterSq():0.00}");
            #endregion
         
            
            #region Hệ tọa độ địa phương của đối tượng
            //var famIns = elem as FamilyInstance;
            //var tf = famIns.GetTransform();
            //TaskDialog.Show("Revit", $"Hệ tọa độ địa phương của đối tượng: Gốc tọa độ: {tf.Origin}, Trục X: {tf.BasisX}, Trục Z:{tf.BasisZ}");
            //var famSym = famIns.Symbol;
            //var fam = famSym.Family;
            //TaskDialog.Show("Revit", $"Symbol: {famSym.Name}, Family: {fam.Name}");
            #endregion


            tx.Start();
            #region Tạo grid
            //XYZ P1 = sel.PickPoint(), P2 = sel.PickPoint();
            //DetailCurve dc = doc.Create.NewDetailCurve(view, Line.CreateBound(P1, P2));

            //XYZ center = sel.PickPoint();
            //DetailCurve dc1 = doc.Create.NewDetailCurve(activeView, Arc.Create(center,((double)5000).Milimet2Feet(),0,Math.PI*2,XYZ.BasisX,XYZ.BasisY));

            //ReferenceArray refArray = new ReferenceArray();
            //while(true) //Pick hoài cho đến khi escape 
            //{
            //    try
            //    {
            //        refArray.Append(sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element));
            //    }
            //    catch(Autodesk.Revit.Exceptions.OperationCanceledException)
            //    {
            //        break;
            //    }
            //}
            //TaskDialog.Show("Revit", "Pick one point for line!");
            //XYZ p1 = sel.PickPoint();
            //doc.Create.NewDimension(activeView, Line.CreateBound(p1, p1 + XYZ.BasisX), refArray); // P1 + XYZ.BasicX là kéo điểm đó theo phương X (Vector chỉ phương X)

            //var gr = Grid.Create(doc, Line.CreateBound(sel.PickPoint(), sel.PickPoint()));
            //gr.Name = "X100";
            //var lv = Level.Create(doc, ((double)10000).Milimet2Feet());
            //lv.Name = "Tầng 20";
            #endregion

            #region Array Linear đối tượng
            //var elemIds = new List<ElementId>();
            //while(true)
            //{
            //    try
            //    {
            //        elemIds.Add(sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToElement().RevitElement.Id);
            //    }
            //    catch(Autodesk.Revit.Exceptions.OperationCanceledException)
            //    {
            //        break;
            //    }
            //}
            //var group = doc.Create.NewGroup(elemIds);

            //var elemId = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToElement().RevitElement.Id;
            //LinearArray.Create(doc, activeView, elemId, 5, XYZ.BasisX * ((double)1500).Milimet2Feet(), ArrayAnchorMember.Second);
            #endregion

            #region Array Radial đối tượng
            //var elemId2 = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToElement().RevitElement.Id;
            //var center = sel.PickPoint();
            //RadialArray.Create(doc, activeView, elemId2, 5, Line.CreateBound(center, center + XYZ.BasisZ), Math.PI * 2, ArrayAnchorMember.Last);

            #endregion

            #region Tạo assembly 
            //var elemIds4 = new List<ElementId>();
            //ElementId cateId = null;
            //while (true)
            //{
            //    try
            //    {
            //        var elem = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToElement().RevitElement;
            //        elemIds4.Add(elem.Id);
            //        if(cateId == null) { cateId = elem.Category.Id; }
            //    }
            //    catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            //    {
            //        break;
            //    }
            //}
            //var assembly = AssemblyInstance.Create(doc, elemIds4, cateId);
            #endregion

            #region Information Element
            #region ElementType
            // ElementType  là Informatino Element 
            // system family thì có loại Type (WallType, FloorType)
            // cái nào có thể load được family thì là FamilyInstance

            //Wall wall = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToElement().RevitElement as Wall;
            //WallType walltype = wall.WallType;
            //HostObjAttributes hostObjAttri = walltype; // Ép kiểu ngụ ý từ kiểu con sang kiểu cha nên ko cần phải as HostObjAttributes
            //TaskDialog.Show("Revit", $"{walltype.Name}");

            //Floor floor = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToElement().RevitElement as Floor;
            //WallType floortype = wall.WallType;
            //TaskDialog.Show("Revit", $"{floortype.Name}");
            //TaskDialog.Show("Revit", $"{walltype.Name} - {hostObjAttri.Name}");

            //FamilyInstance fi = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToElement().RevitElement as FamilyInstance;
            //FamilySymbol fSym = fi.Symbol;
            //FamilySymbol là ElementType của FamilyInstance

            // Lấy ElementType của 1 Element bất kì
            var elem1 = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToElement().RevitElement;
            //Lấy ElementId của ElementTYpe
            var elemTypeId = elem.GetTypeId();
            var elemType = doc.GetElement(elemTypeId) as ElementType;
            TaskDialog.Show("Revit", $"{elemType.LookupParameter("Description").AsString().ToString()}");
            #endregion

            //Fillpattern, LinePattern, ProjectInfo, Material...
            //var projectInfo = doc.ProjectInformation;

            //var materials = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x is Material);

            #region View

            //View view = null;
            //View3D v3d = null;
            //ViewPlan vpl = null;
            //ViewSection vSec = null;
            //ViewSchedule vSchedule = null;

            //XYZ origin = activeView.Origin;
            //XYZ vectorX = activeView.RightDirection;
            //XYZ vectorY = activeView.UpDirection;
            //XYZ vectorZ = activeView.ViewDirection;

            //TaskDialog.Show("Revit", $"Origin:{origin}\n VectorX:{vectorX}\n VectorY:{vectorY}\n VectorZ:{vectorZ}");

            #endregion

            #region SketchElement
            // SketchElement là những đối tượng tạm thời để dựng hình, tính toán hình học
            //SketchPlan là đối tượng Sketch 2d, quy định mặt phẳng làm việc. (Ref plan)
            //XYZ origin = activeView.Origin;
            //XYZ vectorX = activeView.RightDirection;
            //XYZ vectorY = activeView.UpDirection;
            //Plane plane = Plane.CreateByOriginAndBasis(origin, vectorX, vectorY);
            //SketchPlane skPlane = SketchPlane.Create(doc, plane);
            //doc.Create.NewModelCurve(Line.CreateBound(origin, origin + vectorX * 10), skPlane);

            //XYZ origin1 = activeView.Origin;
            //Level level = doc.GetElement(new ElementId(2375)) as Level;
            //SketchPlane skPlane1 = SketchPlane.Create(doc, level.Id);
            //doc.Create.NewModelCurve(Line.CreateBound(new XYZ(0, 0, level.Elevation), new XYZ(200, 200, level.Elevation)), skPlane1);


            #endregion

            #region CombinableElement
            //CombinableElement là kiểu dữ liệu tạm thời để mô phỏng các hình khối

            //Extrusion ext = null;





            #endregion

            #endregion




            tx.Commit();
            return Result.Succeeded;
        }
    }
}
