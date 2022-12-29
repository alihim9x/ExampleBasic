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
using Constant;
using System.IO;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class GeometryCommand : IExternalCommand
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

            #region Get Solid
            //var col1 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var geoCol1 = col1.get_Geometry(new Options());
            //Autodesk.Revit.DB.Solid solid1 = null;
            //foreach (GeometryObject item in geoCol1)
            //{
            //    if (item is GeometryInstance)

            //    {
            //        var geoIns1 = item as GeometryInstance; // Áp dụng cho FamilyInstance mà chưa qua join hoặc cut...
            //        foreach (var item2 in geoIns1.GetInstanceGeometry())
            //        {
            //            var s = item2 as Solid;
            //            if (s != null && s.Faces.Size != 0 && s.Edges.Size != 0)
            //            {
            //                solid1 = s;
            //                break;
            //            }
            //        }
            //    }
            //    if (item is Solid) // Áp dụng cho System Family hoặc FamilyInstance đã bị xử lý ̣(join cut)
            //    {
            //        var s = item as Solid;
            //        if (s != null && s.Faces.Size != 0 && s.Edges.Size != 0)
            //        {
            //            solid1 = s;
            //            break;
            //        }
            //    }
            //}

            // Get Single Solid
            //var elem1 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var singleSolid1 = elem1.GetSingleSolid();
            //var originalSolid1 = elem1.GetOriginalSolid();
            //Action<Document> action1 = ((x) =>
            //{
            //    var translateSingleSolid1 = singleSolid1.MoveToOrigin();
            //    FreeFormElement.Create(x, translateSingleSolid1);

            //});
            //Action<Document> action2 = ((x) =>
            //{
            //    var translateOriginalSolid1 = originalSolid1.MoveToOrigin();
            //    FreeFormElement.Create(x, translateOriginalSolid1);

            //});
            //var family1 = FamilyUtil.Create($"Solid_{Guid.NewGuid()}", action1);
            //family1.Insert(sel.PickPoint());
            //var family2 = FamilyUtil.Create($"Solid_{Guid.NewGuid()}", action2);
            //family2.Insert(sel.PickPoint());
            #endregion



            #region Merge Solid
            //var elem2 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var elem3 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var solid2 = elem2.GetOriginalSolid();
            //var solid3 = elem3.GetOriginalSolid();
            //var mergeSolid1 = BooleanOperationsUtils.ExecuteBooleanOperation(solid2, solid3, BooleanOperationsType.Union);


            //Action<Document> action3 = ((x) =>
            //{
            //    var translateSingleSolid1 = mergeSolid1.MoveToOrigin();
            //    FreeFormElement.Create(x, translateSingleSolid1);

            //});

            //var family3 = FamilyUtil.Create($"Solid_{Guid.NewGuid()}", action3);
            //family3.Insert(sel.PickPoint());
            #endregion

            #region Difference Solid
            //var elem4 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var elem5 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var elem6 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var solid4 = elem4.GetOriginalSolid();
            //var solid5 = elem5.GetOriginalSolid();
            //var solid6 = elem6.GetOriginalSolid();
            //var beamSolid2 = BooleanOperationsUtils.ExecuteBooleanOperation(solid5, solid6, BooleanOperationsType.Union);
            //var mergeSolid2 = BooleanOperationsUtils.ExecuteBooleanOperation(beamSolid2, solid4, BooleanOperationsType.Union);
            //var columnSolid1 = BooleanOperationsUtils.ExecuteBooleanOperation(mergeSolid2, beamSolid2, BooleanOperationsType.Difference);

            //Action<Document> mergeAction1 = ((x) =>
            //{
            //    var translateSingleSolid1 = mergeSolid2.MoveToOrigin();
            //    FreeFormElement.Create(x, translateSingleSolid1);

            //});

            //var mergeFamily1 = FamilyUtil.Create($"Solid_{Guid.NewGuid()}", mergeAction1);
            //mergeFamily1.Insert(sel.PickPoint());

            //Action<Document> differenceAction1 = ((x) =>
            //{
            //    var translateSingleSolid1 = columnSolid1.MoveToOrigin();
            //    FreeFormElement.Create(x, translateSingleSolid1);

            //});

            //var columnFamily1 = FamilyUtil.Create($"Solid_{Guid.NewGuid()}", differenceAction1);
            //columnFamily1.Insert(sel.PickPoint());

            //var elem7 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var otherElems = sel.PickObjects(ObjectType.Element).Select(x => x.ToRevitElement());
            //var solid7 = elem7.GetOriginalSolid();
            //var otherSolids7 = otherElems.Select(x => x.GetOriginalSolid());
            //var diffSolid7 = solid7.DifferenceSolid(otherSolids7);
            //Action<Document> differenceAction7 = ((x) =>
            //{
            //    var translateSingleSolid1 = diffSolid7.MoveToOrigin();
            //    FreeFormElement.Create(x, translateSingleSolid1);

            //});

            //var columnFamily7 = FamilyUtil.Create($"Solid_{Guid.NewGuid()}", differenceAction7);
            //columnFamily7.Insert(sel.PickPoint());

            // Lấy Difference Solid của đối tượng pick với các đối tượng giao với nó
            //List<BuiltInCategory> bic = new List<BuiltInCategory>
            //{
            //    BuiltInCategory.OST_Walls,
            //    BuiltInCategory.OST_StructuralFraming,
            //    BuiltInCategory.OST_StructuralColumns,
            //    BuiltInCategory.OST_StructuralFoundation,
            //};

            //var elem8 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var otherElems8 = elem8.GetIntersectElements(bic);
            //var solid8 = elem8.GetOriginalSolid();
            //var otherSolids8 = otherElems8.Select(x => x.GetOriginalSolid());
            //var diffSolid8 = solid8.DifferenceSolid(otherSolids8);
            
            //Action<Document> differenceAction8 = ((x) =>
            //{
            //    var translateSolid8 = diffSolid8.MoveToOrigin();
            //    FreeFormElement.Create(x,translateSolid8);

            //});

            //var columnFamily8 = FamilyUtil.Create($"Solid_{Guid.NewGuid()}", differenceAction8);
            //columnFamily8.Insert(sel.PickPoint()); 



            #endregion

            #region Lấy đối tượng giao với model line

            ElementClassFilter wallFil5 = new ElementClassFilter(typeof(Wall));
            ElementClassFilter fiFil5 = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter beamCateFil5 = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            ElementCategoryFilter colCateFil5 = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
            var bOcFil5 = new LogicalOrFilter(beamCateFil5, colCateFil5);
            var fibOcfil5 = new LogicalAndFilter(bOcFil5, fiFil5);
            var wOcObfiFil5 = new LogicalOrFilter(wallFil5, fibOcfil5);

            var elem5 = doc.GetElement(sel.PickObject(ObjectType.Element, "Vui lòng chọn đối tượng!"));
            var bb5 = elem5.get_BoundingBox(null);
            var ol5 = new Outline(bb5.Min, bb5.Max);
            var bbIntersectFil5 = new BoundingBoxIntersectsFilter(ol5);
            //var elementIntersec5 = new ElementIntersectsElementFilter(elem5);
            var collector5 = new FilteredElementCollector(doc).WherePasses(wOcObfiFil5).
                WherePasses(bbIntersectFil5)/*.WherePasses(elementIntersec5)*/;
            var joinElems5 = JoinGeometryUtils.GetJoinedElements(doc, elem5).Select(x => doc.GetElement(x)); // Những element join với element đã cho
            var elems5 = collector5.Union(joinElems5);

            //sel.SetElementIds(elems5.Select(x => x.Id).ToList());
            foreach (var item in elems5)
            {
                TaskDialog.Show("Revit", $"{item.AsValue("Comments").ValueText}");
            }
            #endregion


            tx.Commit();
            return Result.Succeeded;
        }
    }
}



