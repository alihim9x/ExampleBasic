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
    public class ElementSelection : IExternalCommand
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

            #region Truy xuất bằng Identified

            //Truy xuất bằng 3 cách: ID, UniqueId, Reference(Reference có thể là Ref của cạnh hoặc mặt hoặc của chính đối tượng đó)
            //ElementId elemId = new ElementId(288922);
            //var guid = "d81e3bb6-6861-4dbf-bde2-f0d90ac6d18d-0004689a";
            //var elem = doc.GetElement(elemId); Trả về Element từ ID
            //var elem1 = doc.GetElement(guid); Trả về Element từ UniqueId
            //var rf = Reference.ParseFromStableRepresentation(doc, "d81e3bb6-6861-4dbf-bde2-f0d90ac6d18d-0004689a:0:INSTANCE:d81e3bb6-6861-4dbf-bde2-f0d90ac6d18d-000468b5:15:SURFACE");
            //var rf1 = Reference.ParseFromStableRepresentation(doc, guid); //Cái này chính là Ref của đối tượng
            //var elem2 = doc.GetElement(rf); // Trả về ELement của 1 mặt trong đối tượng.
            //var edgeRf = Reference.ParseFromStableRepresentation(doc, "d81e3bb6-6861-4dbf-bde2-f0d90ac6d18d-0004689a:0:INSTANCE:d81e3bb6-6861-4dbf-bde2-f0d90ac6d18d-000468b5:13:LINEAR");

            //var edge = elem.GetGeometryObjectFromReference(edgeRf) as Edge; // Lấy đối tượng Edge từ Reference chính nó.




            #endregion


            #region Selection
            tx.Start();
            // Tập hợp các đối tượng được chọn sẵn
            var elemIds = sel.GetElementIds(); // Tập hợp ElementID chọn trước 
            var elem = elemIds.Select(x => doc.GetElement(x)); // Tập hợp Element chọn trước
            foreach (var item in elem.ToList())
            {
                StructuralFramingUtils.AllowJoinAtEnd(item as FamilyInstance, 0);
                StructuralFramingUtils.AllowJoinAtEnd(item as FamilyInstance, 1);

            }
            tx.Commit();
            //TaskDialog.Show("Revit", $"Số vách được chọn: {elem.Where(x => x is Wall).Count()}" +
            //                $"\n Số cột được chọn:{elem.Where(x => x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns).Count()}");
            // Cột là FamilyInstance nên ko thể is Column, phải kiểm tra Category.Id.intergerValue == ...

            // Pick đối tượng khi chạy lệnh
            //var rf = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element,"Bạn hãy chọn đối tượng");
            //TaskDialog.Show("Revit", $"{rf.ConvertToStableRepresentation(doc)}");

            //var rf1 = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.PointOnElement); // Pick 1 điểm trên đối tượng và trả về Ref của điểm đó
            //TaskDialog.Show("Revit", $"{rf1.GlobalPoint}");

            //var rf2 = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge); //Pick 1 cạnh trên đối tượng và trả về Ref của cạnh đó
            //TaskDialog.Show("Revit", $"{rf2.ConvertToStableRepresentation(doc)}");

            //var rf3 = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face); // Pick vào 1 mặt của element và trả về Ref của mặt đó
            //TaskDialog.Show("Revit", $"{rf3.ConvertToStableRepresentation(doc)}");

            // IselectionFilter: Lọc ra các đối tượng phụ hợp với điều kiện

            //var rf4 = sel.PickObject(ObjectType.Element, new WallSelectionFilter()); //nếu ko có singleton phải truyền doc vô WallSelectionFilter(doc)
            //rf4 = sel.PickObject(ObjectType.Face, new WallSelectionFilter());

            //var rf4 = sel.PickObject(ObjectType.Element, new BeamOrColumnSelectionFilter());

            Func<Element, bool> filterWall = x => x is Wall;
            Func<Element, bool> filterWall1 = x =>
             {
                 var wall = x as Wall;
                 WallType wallType = wall.WallType;


                 if (wall != null && wallType.LookupParameter("Width").AsDouble() * 304.8 > 200) return true; // HỎi sao chỗ này .Milimet2Feet ko được
                 return false;
             };
            Func<Reference, bool> filterRemoveTopBotFace = x =>
             {

                 var face = doc.GetElement(x).GetGeometryObjectFromReference(x) as PlanarFace;
                 if (Math.Round(face.FaceNormal.Z, 0) == XYZ.BasisZ.Z || Math.Round(face.FaceNormal.Z, 0) == -XYZ.BasisZ.Z) return false;
                 return true;

             };
            Func<Element, bool> filterColumnOrBeam = x =>
             {
                 var cateId = x.Category.Id.IntegerValue;
                 if (cateId == (int)BuiltInCategory.OST_StructuralColumns
                     || cateId == (int)BuiltInCategory.OST_StructuralFraming) return true;
                 return false;
             };

            //var rf5 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(filterWall, null));
            //var rf55 = sel.PickObject(ObjectType.Face, new FuncSelectionFilter(filterWall, filterRemoveTopBotFace));
            //rf5 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(filterColumnOrBeam, null));


            Func<Element, bool> rebarFilter = x =>
             {
                 var cateId = x.Category.Id.IntegerValue;
                 if (cateId == (int)BuiltInCategory.OST_Rebar) return true;
                 return false;
             };

            Func<Reference, bool> point = x =>
            {
                return true;
            };

            Func<Element, bool> filterWallBeamColumn = x =>
             {
                 var wall = x is Wall;
                 var cateId = x.Category.Id.IntegerValue;
                 if (wall != null || cateId == (int)BuiltInCategory.OST_StructuralColumns || cateId == (int)BuiltInCategory.OST_StructuralFraming) return true;
                 return false;
             };
            Func<Reference, bool> edge = x =>
             {
                 var e = doc.GetElement(x).GetGeometryObjectFromReference(x) as Edge;
                 if (e != null) return true;
                 return false;
             };


            //var rf6 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(rebarFilter, null));
            //var rf7 = sel.PickObject(ObjectType.PointOnElement, new FuncSelectionFilter(rebarFilter, point));
            //var rf8 = sel.PickObject(ObjectType.Edge, new FunctionSelectionFilter(filterWallBeamColumn, edge));

            //Func<Element, bool> wallFilter = x => x is Wall;
            //var rf8  = sel.PickObjects(ObjectType.Element) as List<Reference>;
            //foreach (var item in rf8)
            //{
            //    TaskDialog.Show("Revit", doc.GetElement(item).Name);
            //}
            //// hoặc linQ
            //(sel.PickObjects(ObjectType.Element) as List<Reference>).ForEach(x => TaskDialog.Show("Revit", doc.GetElement(x).Name));

            #region Cho phép chọn các đối tượng là tường cột dầm
            // Pick lần 2 sẽ lưu kết quả lần 1 đã pick
            //Func<Element, bool> allowAllElement = x => true;
            //TaskDialog.Show("Revit", "Pick PreSelected Elements!");
            //var preSelectedRfs = sel.PickObjects(ObjectType.Element, new FuncSelectionFilter(allowAllElement, null));
            //TaskDialog.Show("Revit", "Pick Next Elements");
            //var rf9 = sel.PickObjects(ObjectType.Element, new FuncSelectionFilter(allowAllElement, null), "Chọn các đối tượng tiếp theo", preSelectedRfs) as List<Reference>;
            //rf9.ForEach(x => TaskDialog.Show("Revit", doc.GetElement(x).Name));


            //Chọn các đối tượng là Wall, Beam hoặc Column
            Func<Element, bool> wallbeamcolumnFilter = null;
            //wallbeamcolumnFilter = x =>
            // {

            //     //var cateId = x.Category.Id.IntegerValue;
            //     //if (x is Wall || cateId == (int)BuiltInCategory.OST_StructuralFraming || cateId == (int)BuiltInCategory.OST_StructuralColumns) return true;
            //     //return false;

            //     // hoặc

            //     if (x is Wall) return true;
            //     var cate = (BuiltInCategory)x.Category.Id.IntegerValue;
            //     switch(cate)
            //     {
            //         case BuiltInCategory.OST_StructuralColumns: return true;
            //         case BuiltInCategory.OST_StructuralFraming: return true;
            //     }
            //     return false;
            // };


            //Hoặc cách tổng quát hơn
            Func<Element, bool> wallFilter = x => x is Wall;
            Func<Element, bool> beamFilter = x => (BuiltInCategory)x.Category.Id.IntegerValue
            == BuiltInCategory.OST_StructuralFraming;
            Func<Element, bool> columnFilter = x => (BuiltInCategory)x.Category.Id.IntegerValue
            == BuiltInCategory.OST_StructuralColumns;
            wallbeamcolumnFilter = x => wallFilter(x) || beamFilter(x) || columnFilter(x);

            //(sel.PickObjects(ObjectType.Element, new FuncSelectionFilter(wallbeamcolumnFilter),
            //    "Vui lòng chọn đối tượng.") as List<Reference>)
            //    .ForEach(x=>TaskDialog.Show("Revit",doc.GetElement(x).Name));
            #endregion


            #endregion

            #region PickElementsByRectangle


            //var elems1 = sel.PickElementsByRectangle() as List<Element>;
            //TaskDialog.Show("Revit", $"{elems1.Count}");

            //var elems2 = sel.PickElementsByRectangle(new FuncSelectionFilter(x => x is Wall)) as List<Element>;
            //TaskDialog.Show("Revit", $"{elems2.Count}");


            #endregion

            #region Pick Point

            //var pnt = sel.PickPoint(ObjectSnapTypes.Centers);
            //TaskDialog.Show("Revit", $"{pnt}");


            #endregion

            #region Trả về các đối tượng ra màn hình 
            //List<Element> elems4 = new List<Element>();
            //while(true) //Pick mãi

            //{
            //    try
            //    {
            //        elems4.AddRange(sel.PickElementsByRectangle()); // add vào 1 mảng, quét được 1 mảng nên ko thể dùng .Add
            //    }
            //    catch (Autodesk.Revit.Exceptions.OperationCanceledException) // Bấm escape để thoát lệnh
            //    {

            //        break;
            //    }  
            //}
            //// Hàm sel.SetElementIds trả về các đối tượng ra màn hình (highlight xanh)
            //sel.SetElementIds(elems4.Select(x => x.Id).GroupBy(x => x.IntegerValue).Select(x => x.First()).ToList()); //GroupBy sẽ group các đối tượng giống nhau lại rồi chọn First



            #endregion


            #region FilteredElementCollector: tập hợp các đối tượng được lọc
            // Phương thức gọi nhanh của ElementIssElementTYpeFilter
            //var collector0 = new FilteredElementCollector(doc).WhereElementIsNotElementType();

            //// Cách gọi bình thường, tham số inverted kiểu bool nhằm đảo ngược kết quả trả về - false trả về Type, true trả về instace
            //ElementIsElementTypeFilter typeFilter = new ElementIsElementTypeFilter();
            //ElementIsElementTypeFilter instanceFilter = new ElementIsElementTypeFilter(true);
            //var collector1 = new FilteredElementCollector(doc).WherePasses(instanceFilter);
            // Line 242 = Line 246+247
            #region BoundingBoxIntersectFilter
            //BoundingBox Outline được xác định bởi 2 điểm có tọa độ là (Xmin,YMin,Zmin) và (Xmax,YMax,ZMax)

            //var elem10 = doc.GetElement(sel.PickObject(ObjectType.Element));
            //var bb = elem10.get_BoundingBox(null);
            //var ol = new Outline(bb.Min, bb.Max);
            //var bbIntersectFil = new BoundingBoxIntersectsFilter(ol,((double)50).Milimet2Feet());
            //var collector2 = new FilteredElementCollector(doc).WherePasses(bbIntersectFil);
            //sel.SetElementIds(collector2.Select(x => x.Id).ToList());
            #endregion




            #endregion

            #region Filter by Category n class
            //var wallFilter1 = new ElementClassFilter(typeof(Wall));
            //var wallElem = new FilteredElementCollector(doc).WherePasses(wallFilter1);
            //wallElem = new FilteredElementCollector(doc).OfClass(typeof(Wall)); //Cách gọi nhanh shorcut của 2 dòng trên

            //var columnFilter1 = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
            //var columnElem = new FilteredElementCollector(doc).WherePasses(columnFilter1);
            ////Hoặc
            //columnElem = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralColumns);
            ////Hoặc
            //var columnElems = new FilteredElementCollector(doc).OfCategoryId(new ElementId(BuiltInCategory.OST_StructuralColumns));
            #endregion

            #region ElementLogicalFilter : Bộ lọc nhiều điều kiện
            //Bài toán: Lấy các đối tượng là Wall hoặc FamilyInstance (Category là Cột hoặc dầm)
            
            var wallFilter3 = new ElementClassFilter(typeof(Wall));
            var fiFilter = new ElementClassFilter(typeof(FamilyInstance));
            var beamCateFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming); // BuiltInCategory trả về cả Type và Instance
            var columncateFilter = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
            //ElementLogicalFilter
            //Cách 1:
            var beamOrcolumnCateFilter = new LogicalOrFilter(beamCateFilter, columncateFilter);

            //Cách 2:
            var beamOrcolumnCateFilter1 = new LogicalOrFilter(new List<ElementFilter> { beamCateFilter, columncateFilter });

            //Filter FamilyInstance và Category cột hoặc dầm
            //Cách 1:
            var bOcFiFilter = new LogicalAndFilter(fiFilter, beamOrcolumnCateFilter);
            //cách 2:
            var bOcFiFilter1 = new LogicalAndFilter(new List<ElementFilter> { fiFilter, beamOrcolumnCateFilter });

            //Filter Wall hoặc FamilyInsstance
            var wallOrfiFilter = new LogicalOrFilter(new List<ElementFilter> { wallFilter3, bOcFiFilter1 });

            //sel.SetElementIds(new FilteredElementCollector(doc).WherePasses(wallOrfiFilter).Select(x => x.Id).ToList());

            //Cách 3: Sử dụng Linq

            //var elems = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x is Wall);
            //sel.SetElementIds(elems.Select(x => x.Id).ToList());

            //var elems2 = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x is Wall || (x is FamilyInstance
            //&& (x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns)));            
            //sel.SetElementIds(elems2.Select(x => x.Id).ToList());

            //var elems2 = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x =>
            //{
            //    if (x is Wall) return true;
            //    else if (x is FamilyInstance && x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns) return true;
            //    else if (x is FamilyInstance && x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming) return true;
            //    return false;
            //});
            //sel.SetElementIds(elems2.Select(x => x.Id).ToList());

            //Func<Element, bool> wallFil4 = x => x is Wall;
            //Func<Element, bool> columnfamIns1 = x => x is FamilyInstance && x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns;

            //var elems3 = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where( x=>wallFil4(x) || columnfamIns1(x));





            #endregion


            #region Chỉ cho chọn Wall có Width > 200 hoặc Column có H>200

            Func <Element, bool> wallOrcolumnsFilter1 = x =>
            {
                var cateId = x.Category.Id.IntegerValue;
                var typeId = x.GetTypeId();
                if (x is Wall && ((x as Wall).WallType.LookupParameter("Width").AsDouble() * 304.8 > 200)) return true;

                else if (cateId == (int)BuiltInCategory.OST_StructuralColumns && (doc.GetElement(typeId) as ElementType).LookupParameter("h").AsDouble() * 304.8 > 200) return true;

                return false;
            };

            //var rf10 = sel.PickObjects(ObjectType.Element, new FuncSelectionFilter(wallOrcolumnsFilter1)) as List<Reference>;
            //List<ElementId> rf10Id = new List<ElementId>();
            //foreach (var item in rf10)
            //{
            //    rf10Id.Add(item.ToElement().RevitElement.GetTypeId());
            //}
            //rf10Id.ForEach(x => TaskDialog.Show("Revit", ((doc.GetElement(x) as ElementType).LookupParameter("h").AsDouble().ToString())));

            #endregion


            #region Chọn các đối tượng là tường có Width >200 hoặc Columns có H>200 trong Project

            // Hỏi sao as ElementType . LookupParameter ("h") ko được
            var columnFiFilter = new LogicalAndFilter(columncateFilter, fiFilter);
            var wallOrColumnFiFilter = new LogicalOrFilter(new List<ElementFilter> { wallFilter3, columnFiFilter });
            // Hỏi Where dùng hoặc được ko
            //sel.SetElementIds(new FilteredElementCollector(doc).WherePasses(columnFiFilter)
            //                    .Where(x=>(x as FamilyInstance).Symbol.LookupParameter("h").AsDouble()*304.8 > 300)
            //                    .Select(x => x.Id).ToList());
            //sel.SetElementIds(new FilteredElementCollector(doc).WherePasses(wallOrColumnFiFilter)
            //                    .Where(x =>
            //                    {
            //                        if (x is Wall)
            //                        {
            //                            return (doc.GetElement(x.GetTypeId()).LookupParameter("Width").AsDouble() * 304.8 > 200);
            //                        }
            //                        else
            //                        {
            //           return (doc.GetElement(x.GetTypeId()).LookupParameter("h").AsDouble() * 304.8 > 300); }
            //                     })
            //                    .Select(x => x.Id).ToList());
            #endregion

            //var fillregionFilter1 = new ElementClassFilter(typeof(FilledRegion));
            //sel.SetElementIds(new FilteredElementCollector(doc).WherePasses(fillregionFilter1).Select(x => x.Id).ToList());

            #region Đối tượng va chạm với đối tượng được chọn trước đó
            //var notanalyticalColumn = new ElementClassFilter(typeof(AnalyticalModelColumn), true);
            //var notanalyticalNode = new ElementCategoryFilter(BuiltInCategory.OST_AnalyticalNodes, true);
            //var pickedRef1 = sel.PickObject(ObjectType.Element, "Vui lòng chọn đối tượng");
            //var pickedElem1 = doc.GetElement(pickedRef1);
            //var bbElem1 = pickedElem1.get_BoundingBox(null);
            //var olElem1 = new Outline(bbElem1.Min, bbElem1.Max);
            //var bbIntersectFil1 = new LogicalAndFilter(new List<ElementFilter> { new BoundingBoxIntersectsFilter(olElem1), notanalyticalColumn, notanalyticalNode });
            //var pickedElemId = pickedElem1.Id;
            //var colector2 = new FilteredElementCollector(doc).WherePasses(bbIntersectFil1);
            //sel.SetElementIds(colector2.Select(x => x.Id).Where(x =>!= pickedElemId).ToList());




            #endregion

            #region ElementIntersectsFilter (slow filter)
            //Cách 1:

            //ElementClassFilter wallFil5 = new ElementClassFilter(typeof(Wall));
            //ElementClassFilter fiFil5 = new ElementClassFilter(typeof(FamilyInstance));
            //ElementCategoryFilter beamCateFil5 = new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming);
            //ElementCategoryFilter colCateFil5 = new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns);
            //var bOcFil5 = new LogicalOrFilter(beamCateFil5, colCateFil5);
            //var fibOcfil5 = new LogicalAndFilter(bOcFil5, fiFil5);
            //var wOcObfiFil5 = new LogicalOrFilter(wallFil5, fibOcfil5);

            //var elem5 = doc.GetElement( sel.PickObject(ObjectType.Element, "Vui lòng chọn đối tượng!"));
            //var bb5 = elem5.get_BoundingBox(null);
            //var ol5 = new Outline(bb5.Min, bb5.Max);
            //var bbIntersectFil5 = new BoundingBoxIntersectsFilter(ol5);
            //var elementIntersec5 = new ElementIntersectsElementFilter(elem5);
            //var collector5 = new FilteredElementCollector(doc).WherePasses(wOcObfiFil5).
            //    WherePasses(bbIntersectFil5).WherePasses(elementIntersec5);
            //var joinElems5 = JoinGeometryUtils.GetJoinedElements(doc, elem5).Select(x => doc.GetElement(x)); // Những element join với element đã cho
            //var elems5 = collector5.Union(joinElems5);

            //sel.SetElementIds(elems5.Select(x => x.Id).ToList());


            // Cách 2: Linq
            //var w0fiElems = new FilteredElementCollector(doc).WhereElementIsNotElementType()
            //    .Where(x => x is Wall || (x is FamilyInstance && (x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns
            //    || x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming)));
            //var elem6 = doc.GetElement(sel.PickObject(ObjectType.Element, "Vui lòng chọn đối tượng!"));
            //var bb6 = elem6.get_BoundingBox(null);
            //var ol6 = new Outline(bb6.Min, bb6.Max);
            //var bbIntersect6 = new BoundingBoxIntersectsFilter(ol6);
            //var collector6 = new FilteredElementCollector(doc, w0fiElems.Select(x => x.Id).ToList()).WherePasses(bbIntersect6);
            //var joinElems6 = JoinGeometryUtils.GetJoinedElements(doc, elem6).Select(x=>doc.GetElement(x));
            //var elems6 = collector6.Union(joinElems6);
            //sel.SetElementIds(elems6.Select(x => x.Id).ToList());



            #endregion


            return Result.Succeeded;
        }
    }
    public class WallSelectionFilter : ISelectionFilter
    {
        #region nếu ko có single ton
        //public Document Document { get; set; }
        //public WallSelectionFilter(Document doc)
        //{
        //    Document = doc;
        //}
        #endregion


        public bool AllowElement(Element elem)
        {
            if (elem is Wall) return true; // Khi element thuộc kiểu Wall thì hợp lệ,
            return false; // Khi element thuộc kiểu khác, ko thỏa mãn điều kiện

        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            var doc = Singleton.Instance.RevitData.Document;
            var face = doc.GetElement(reference).GetGeometryObjectFromReference(reference) as PlanarFace;
            if (Math.Round(face.FaceNormal.Z, 0) == XYZ.BasisZ.Z || Math.Round(face.FaceNormal.Z, 0) == -XYZ.BasisZ.Z) return false;
            return true;
        }
    }
    public class BeamOrColumnSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            var cateId = elem.Category.Id.IntegerValue;
            if (cateId == (int)BuiltInCategory.OST_StructuralColumns
                || cateId == (int)BuiltInCategory.OST_StructuralFraming) return true;
            return false;

        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;  // ko lọc điều kiện pick mặt hoặc cạnh hoặc point....
        }
    }
    public class FuncSelectionFilter : ISelectionFilter
    {
        private Func<Element, bool> filterElement;
        private Func<Reference, bool> filterReference;

        public FuncSelectionFilter(Func<Element, bool> filterElement, Func<Reference, bool> filterReference = null)
        {
            this.filterElement = filterElement; this.filterReference = filterReference;
        }
        public bool AllowElement(Element elem)
        {
            return filterElement(elem);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return filterReference(reference);
        }
    }
    public class FunctionSelectionFilter : ISelectionFilter
    {
        Func<Element, bool> filterElement;
        Func<Reference, bool> filterReference;
        public FunctionSelectionFilter(Func<Element, bool> filterE, Func<Reference, bool> filterR = null) // = null thì ko cần truyền vào, nó mặc định đã = null r
        {
            this.filterElement = filterE; this.filterReference = filterR;
        }
        public bool AllowElement(Element elem)
        {
            return filterElement(elem);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {

            return filterReference(reference);
        }
    }
}
