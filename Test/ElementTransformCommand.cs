using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Utility;
using SingleData;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Constant;
using Utility;


namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class ElementTransformCommand : IExternalCommand
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
            var app = revitData.Application;
            tx.Start();
            #endregion

            #region Move

            //var col1 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(ConstantValue.FilterColumns)).ToRevitElement();
            //var lcol1 = col1.Location;
            //lcol1.Move(XYZ.BasisX * 1500.0.Milimet2Feet());

            //var beam1 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(ConstantValue.FilterFramings)).ToRevitElement();
            ////var lbeam1 = beam1.Location;
            ////lbeam1.Move(XYZ.BasisX * 1500.0.Milimet2Feet());

            ////var level = LevelUtil.GetLevel("TẦNG 2");
            //// var llevel = level.Location;
            ////llevel.Move(XYZ.BasisZ * 1000.0.Milimet2Feet());

            //ElementTransformUtils.MoveElement(doc, beam1.Id, XYZ.BasisX * 1500.0.Milimet2Feet());

            //var pickedElemIds143 = sel.PickObjects(ObjectType.Element, new FuncSelectionFilter(ConstantValue.FilterColumns)).Select(x=>x.ToRevitElement().Id).ToList();
            //XYZ vector143 = XYZ.BasisX * 1500.0.Milimet2Feet() + XYZ.BasisY * 2000.0.Milimet2Feet();
            //ElementTransformUtils.MoveElements(doc, pickedElemIds143, vector143);

            #endregion

            #region Copy Element

            //var col2 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(ConstantValue.FilterColumns)).ToRevitElement();
            //ElementTransformUtils.CopyElement(doc, col2.Id, XYZ.BasisX * 1500.0.Milimet2Feet());


            //var viewLevel2 = revitData.InstanceElements.Where(x => x is View && x.Name == "TẦNG 2") as View; 
            //var elems2 = sel.PickElementsByRectangle(new FuncSelectionFilter(x => x is DetailLine));
            //ElementTransformUtils.CopyElements(doc.ActiveView, elems2.Select(x => x.Id).ToList(), viewLevel2,Transform.Identity,
            //    new CopyPasteOptions());  

            //Copy Element Type
            //var col2 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(ConstantValue.FilterColumns)).ToRevitElement();
            //var col2Type = col2.GetTypeId().ToRevitElement();
            //string newname2 = "300x800";
            //var newcol2Type = ElementTransformUtils.CopyElement(doc, col2Type.Id, XYZ.Zero).First().ToRevitElement();
            //newcol2Type.Name = newname2;
            //newcol2Type.LookupParameter("b").SetValue(300.0.Milimet2Feet());
            //newcol2Type.LookupParameter("h").SetValue(800.0.Milimet2Feet());
            //(col2 as FamilyInstance).Symbol = newcol2Type as FamilySymbol;

            //var wall3 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(ConstantValue.FilterWalls)).ToRevitElement();
            //string newname3 = "Generic - 200";
            //var newwallType3 = ElementTransformUtils.CopyElement(doc, wall3.GetTypeId().ToRevitElement().Id, XYZ.Zero).First().ToRevitElement() as WallType ;
            //newwallType3.Name = newname3;
            //var layerWall3 = newwallType3.GetCompoundStructure().GetLayers().First();
            //layerWall3.Width = 200.0.Milimet2Feet();
            //newwallType3.SetCompoundStructure(CompoundStructure.CreateSimpleCompoundStructure(new List<CompoundStructureLayer> { layerWall3 }));
            //(wall3 as Wall).WallType = newwallType3;





            #endregion

            #region Rotate

            //var col3 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(ConstantValue.FilterColumns)).ToRevitElement();
            //var point3 = sel.PickPoint();
            //ElementTransformUtils.RotateElement(doc, col3.Id,Line.CreateBound(point3, point3 + XYZ.BasisZ), Math.PI / 4);

            //#region Mirror
            //var col4 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(ConstantValue.FilterColumns)).ToRevitElement();

            //var point41 = sel.PickPoint();
            //var point42 = sel.PickPoint();
            //var origin4 = point41;
            //var basicX = (point42 - point41).Normalize(); // tạo ra 1 véc tơ đi từ điểm 1 tới điểm 2, normalize sẽ ra véc tơ đơn vị của vectow đó
            //var basicY = XYZ.BasisY;
            //var plane4 = Plane.CreateByOriginAndBasis(origin4, basicX, basicY);
            //ElementTransformUtils.MirrorElement(doc, col4.Id, plane4);



            #endregion

            #region Group, Array, Assembly

            //var elems6 = sel.PickElementsByRectangle();
            //var group6 = doc.Create.NewGroup(elems6.Select(x => x.Id).ToList());


            //var elems7 = sel.PickElementsByRectangle();
            //var array7 = LinearArray.Create(doc, doc.ActiveView, elems7.Select(x => x.Id).ToList(),4,
            //    XYZ.BasisY * 2, ArrayAnchorMember.Second);
            //group6.Name = "Group 6";

            //var elems8 = sel.PickElementsByRectangle();
            //var assembly8 = AssemblyInstance.Create(doc, elems8.Select(x => x.Id).ToList(), 
            //    new ElementId(BuiltInCategory.OST_StructuralColumns));

            #endregion

            #region Pin & Delete

            //var col4 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(ConstantValue.FilterColumns)).ToRevitElement();
            //col4.Pinned = true;

            //var cols6 = sel.PickElementsByRectangle(new FuncSelectionFilter(ConstantValue.FilterColumns));
            //doc.Delete(cols6.Select(x => x.Id).ToList());

            #endregion

            #region Join Element

            //var elems9 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var joinedElemsId9 = JoinGeometryUtils.GetJoinedElements(doc, elems9);
            //sel.SetElementIds(joinedElemsId9);

            //var elems10 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var elems11 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var isJoin = JoinGeometryUtils.AreElementsJoined(doc, elems10, elems11); // Kiểm tra xem 2 đối tượng đã join với nhau chưa
            //TaskDialog.Show("Revit", $"{isJoin}");

            //var elems12 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var elems13 = sel.PickObject(ObjectType.Element).ToRevitElement();

            //var isCuted13 = JoinGeometryUtils.IsCuttingElementInJoin(doc, elems12, elems13);
            //TaskDialog.Show("Revit", $"{isCuted13}"); // true thì elems12 cắt elems13


            //JoinGeometryUtils.JoinGeometry(doc,elems12,elements)



            #endregion

            #region Create Element

            //var pnt13 = sel.PickPoint();
            //var pnt14 = sel.PickPoint();
            //Grid.Create(doc, Line.CreateBound(pnt13, pnt14));
            //Level.Create(doc, (double)10000.0.Milimet2Feet());


            //var pnts = new List<XYZ>();
            //while (true)
            //{
            //    try
            //    {
            //        pnts.Add(sel.PickPoint());
            //    }
            //    catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            //    {
            //        break;
            //    }
            //}
            //if(pnts.Count<3)
            //{
            //    tx.Commit(); return Result.Succeeded;
            //}
            //var curveArray1 = new CurveArray();
            //for (int i = 0; i < pnts.Count; i++)
            //{
            //    Line line1 = null;
            //    if(i != pnts.Count-1)
            //    {
            //        line1 = Line.CreateBound(pnts[i], pnts[i + 1]);

            //    }
            //    else
            //    {
            //        line1 = Line.CreateBound(pnts[i], pnts[0]);
            //    }
            //    curveArray1.Append(line1);
            //}
            ////var floorType1 = new FilteredElementCollector(doc).WhereElementIsElementType()
            ////    .Where(x => (x is FloorType)).Where(x=>x.Name == "Generic 300mm").Single() as FloorType;
            //var floorType2 = ElementTypeUtil.GetFloorType("Generic 300mm");

            //Level level2 = LevelUtil.GetLevel("TẦNG 2");

            //doc.Create.NewFloor(curveArray1, floorType2,level2,true);

            //Curve curve1 = Line.CreateBound(sel.PickPoint(), sel.PickPoint()) as Curve;
            //Level level1 = LevelUtil.GetLevel("TẦNG 1");
            //WallType walltype1 = ElementTypeUtil.GetWallType("VÁCH_300");
            //Wall.Create(doc, curve1, walltype1.Id, level1.Id, 5000.0.Milimet2Feet(),500.0.Milimet2Feet(), true, true);


            //XYZ pnt13 = sel.PickPoint();
            //FamilySymbol columntype13 = ElementTypeUtil.GetColumnType("200x300");
            //Level level113 = LevelUtil.GetLevel("TẦNG 1");
            //Level level213 = LevelUtil.GetLevel("TẦNG 2");
            //doc.Create.NewFamilyInstance(pnt13, columntype13,level113, Autodesk.Revit.DB.Structure.StructuralType.Column);


            //Curve curve13 = Line.CreateBound(sel.PickPoint(), sel.PickPoint());
            //FamilySymbol framingtypes13 = ElementTypeUtil.GetFramingType("200x300");
            //doc.Create.NewFamilyInstance(curve13, framingtypes13, level213, Autodesk.Revit.DB.Structure.StructuralType.Beam);

            //Dimension

            //View view13 = doc.ActiveView;
            //ReferenceArray refArr13 = new ReferenceArray();
            //var grids13 = sel.PickElementsByRectangle(new FuncSelectionFilter(ConstantValue.FilterGrids)) as List<Element>;
            //grids13.ForEach(x => refArr13.Append(new Reference(x)));
            //var pnt13 = sel.PickPoint();
            //Line line13 = Line.CreateBound(pnt13, pnt13 + XYZ.BasisX);
            //doc.Create.NewDimension(view13, line13, refArr13);

            //View view14 = doc.ActiveView;
            //ReferenceArray refArrDim14 = new ReferenceArray();
            //var grids14 = sel.PickElementsByRectangle(new FuncSelectionFilter(ConstantValue.FilterGrids)) as List<Element>;
            //var firstGrid14 = grids14.First();
            //XYZ firstVector14 = (((firstGrid14 as Grid).Curve) as Line).Direction;
            //foreach (var item in grids14)
            //{
            //    var gridDirection = GridUtil.GetDirection(item as Grid);
            //    if(firstVector14.IsParallelDirection(gridDirection))
            //    {
            //        refArrDim14.Append(new Reference(item));
            //    }
            //}
            //var pnt14 = sel.PickPoint();
            //var lineVector14 = firstVector14.CrossProduct(XYZ.BasisZ);
            //Line line14 = Line.CreateBound(pnt14, pnt14 + lineVector14);
            //doc.Create.NewDimension(view14, line14, refArrDim14);

            //Tag

            //View view15 = doc.ActiveView;
            //Reference rfTag = sel.PickObject(ObjectType.Element,new FuncSelectionFilter(ConstantValue.FilterColumns));
            //bool leader = true;
            //var pnt15 = sel.PickPoint();
            //IndependentTag.Create(doc, view15.Id, rfTag, leader, TagMode.TM_ADDBY_CATEGORY, TagOrientation.Horizontal, pnt15);

            #endregion

            tx.Commit();
            return Result.Succeeded;
        }
    }
}
